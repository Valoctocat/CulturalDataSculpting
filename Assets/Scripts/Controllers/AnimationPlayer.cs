using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationPlayer : MonoBehaviour
{

    /*

      Manages the Animation of the cube rising when placed on the pedestal
      The enlargement of the Cube once it has reached a sufficient height
      The loading of the images corresponding to this cube

      Reverts all animation when cube is removed from the zone
    */

    // Dataset name grabbed, accessible to any other script
    public static string DATASET_NAME = "Unknown";


    //Public instances
    public ParticleController _particleSystem;
    public OVRScreenFade _fader; //Not implemented yet
    public DataLoader _dataLoader;
    public Stack2DVisualizer _st2dVisualizer;
    public RoomScaler _roomScaler;
    public GameObject _floorAnimator;

    // Animation parameters / instances
    public float max_radius = 20.0f;
    public float min_radius = 1.0f; // TODO: set private
    private float min_scale; //Set to dset scale OnEnter
    public float max_scale = 600.0f;
    public float scaling_speed;
    public float raising_speed;
    private float current_radius;
    private float current_scale;
    private float timer;
    private float timerScale;
    private Vector3 _floorScaler;

    // State variables
    private bool _forceRaiseDown = false;
    private bool _forceScaleDown = false;
    private bool _enlargingCube = false;
    private bool _raisingCube = false;
    private bool _animationFinished = false;

    // Interaction with Dataset
    private GameObject _dataset;
    private GameObject _duplicatedDset; // We need a physical copy to stay at the right position to handle grabbing
    private GameObject _waitingDSet; //In case a dataset is placed in the middle of an animation


    void Start() {
        _floorScaler = new Vector3(_floorAnimator.transform.localScale.x, 0.0f, _floorAnimator.transform.localScale.z);
        current_radius = min_radius;
        _waitingDSet = null;
        _dataset = null;
        _duplicatedDset = null;
    }

    void Update()
    {
        if(!_animationFinished) {
            if(_raisingCube || _forceRaiseDown)  {
                // Update radius of floor and raising cube altitude
                UpdateRadius();

                //Update floor
                UpdateFloor();
                UpdateDSet();
            } else if(_enlargingCube || _forceScaleDown) {
                UpdateScale();

                UpdateRoom();
                UpdateDSetScale();
            }
        }

        if(_waitingDSet != null) {
            if((!_raisingCube) && (!_enlargingCube) && (!_forceRaiseDown) && (!_forceScaleDown) && (!_waitingDSet.GetComponent<OVRGrabbable>().isGrabbed)) {
                OnEnter(_waitingDSet);
            }
        }
    }

    // UpdateFloorRadius and Cube Raising
    private void UpdateRadius() {
        if(_raisingCube && !_forceRaiseDown) {
            this.current_radius += this.timer*raising_speed;
            this.timer+=1.0f;

            if(this.current_radius>max_radius){
                OnTriggerMainEvent();
            }

        } else {
            this.current_radius -= 3.0f*Time.deltaTime;

            //Reset variables when Radius back to normal
            if(this.current_radius < min_radius){
                OnFinishRetraction();
            }
        }

        this.current_radius = Mathf.Clamp(this.current_radius, min_radius, max_radius);
    }


    //Update Cube enlarging and Room scaling
    private void UpdateScale() {
        if(_enlargingCube && !_forceScaleDown) {
            this.current_scale += this.timerScale*scaling_speed;
            this.timerScale+=1.0f;

            if(this.current_scale>max_scale) {
                OnFinishMainEvent();
            }
        } else {
            this.current_scale -= 2.0f;
            if(this.current_scale < min_scale) {
                OnFinishScalingDown();
            }
        }

        this.current_scale = Mathf.Clamp(this.current_scale, min_scale, max_scale);
    }


    // Enlarge red floor if dataset has been placed on the pedestal. Retract it otherwise
    private void UpdateFloor() {
        if(_floorAnimator != null) {
            _floorAnimator.transform.localScale = current_radius * _floorScaler + Vector3.up;
        }
    }

    // Raise Dataset if dataset has been placed on the pedestal. Lower down it otherwise
    private void UpdateDSet() {
        if(_duplicatedDset != null) {
              _duplicatedDset.transform.position = new Vector3(_duplicatedDset.transform.position.x, 1.0f + current_radius/10.0f, _duplicatedDset.transform.position.z);
              _duplicatedDset.transform.Rotate(0,5.0f*Time.deltaTime,0);
        }
    }

    // Scale Room once Dataset has rised high enough, scale down otherwise
    private void UpdateRoom() {
        if(_roomScaler != null) {
              _roomScaler.UpdateScale(current_scale);
        }
    }

    // Enlarge Cube Scale once it has raised to maximum height, shrink it otherwise
    private void UpdateDSetScale() {
        if(_duplicatedDset != null) {
            _duplicatedDset.transform.localScale = current_scale * Vector3.one;
        }
    }

    // Called from ParticleController when a dataset enters the Particle System
    public void OnEnter(GameObject dataset) {
        if(dataset.GetComponent<OVRGrabbable>() != null) {
            OVRGrabbable grabbable = dataset.GetComponent<OVRGrabbable>();
            if((_dataset == null) && (!_raisingCube) && (!_enlargingCube) && (!_forceRaiseDown) &&(!_forceScaleDown) && (!grabbable.isGrabbed)) {
                timer = 1.0f;
                _waitingDSet = null;
                _dataset = dataset;
                min_scale = _dataset.transform.localScale.x;

                //Create Phantom Cube
                _duplicatedDset = Instantiate(_dataset);
                _duplicatedDset.GetComponent<Rigidbody>().isKinematic = true;
                Destroy(_duplicatedDset.GetComponent<OVRGrabbable>());
                Destroy(_duplicatedDset.GetComponent<Rigidbody>());
                Destroy(_duplicatedDset.GetComponent<Collider>());
                Destroy(_duplicatedDset.GetComponent<CubeController>());
                _raisingCube = true;
            } else {
                _waitingDSet = dataset;
            }
        }
    }


    // Called from ParticleController once a dataset is removed from the pedestal
    public void OnExit() {

        DATASET_NAME = "Unknown";
        _waitingDSet = null;

        if(_animationFinished || _enlargingCube) {
          // Make cube small again, then make it go Down
          // MAke room right scale
          _forceScaleDown = true;
          _enlargingCube = false;
          _animationFinished = false;
          _dataLoader.StopLoading(DATASET_NAME);
          _st2dVisualizer.SetReady(false);

        } else if(_raisingCube) {
          // Make cube go down again.
          _raisingCube = false;
          _forceRaiseDown = true;
        }

    }

    // Called when the dataset position is reset back to normal.
    public void OnFinishRetraction() {
        _forceRaiseDown = false;
        timer = 1.0f;
        _dataset = null;
        _raisingCube = false;
        Destroy(_duplicatedDset);
    }

    public void OnFinishScalingDown() {
        _forceScaleDown = false;
        _forceRaiseDown = true;
        _enlargingCube = false;
        timerScale = 1.0f;
    }

    private void OnTriggerMainEvent() {
        if(_dataset != null) {
            _enlargingCube = true;
            _raisingCube = false;
            timerScale = 1.0f;
            timer = 1.0f;
        }
    }

    private void OnFinishMainEvent() {
        _animationFinished = true;
        _enlargingCube = false;
        _raisingCube = false;
        DATASET_NAME = _dataset.name;
        timerScale = 1.0f;
        _dataLoader.LoadImages(DATASET_NAME);
    }
}
