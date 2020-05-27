using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationPlayer : MonoBehaviour
{
    public static string DATASET_NAME = "Unknown";

    public ParticleController _particleSystem;
    public RoomScaler _roomScaler;
    public GameObject _floorAnimator;
    public float max_radius = 20.0f;
    public float min_radius = 1.0f;
    private float min_scale; //Set to dset scale OnEnter
    public float max_scale = 600.0f;
    public float scaling_speed;
    public float raising_speed;
    public OVRScreenFade _fader;
    public DataLoader _dataLoader;
    public Stack2DVisualizer _st2dVisualizer;

    private bool _forceRaiseDown = false;
    private bool _forceScaleDown = false;
    private bool _enlargingCube = false;
    private bool _raisingCube = false;
    private bool _animationFinished = false;

    private GameObject _dataset;
    private GameObject _duplicatedDset;
    private GameObject _waitingDSet;
    private float current_radius;
    private float current_scale;

    private float timer;
    private float timerScale;
    private Vector3 _floorScaler;

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

    private void UpdateScale() {
        if(_enlargingCube && !_forceScaleDown) {
            this.current_scale += this.timerScale*scaling_speed;
            this.timerScale+=1.0f;

            if(this.current_scale>max_scale) {
                OnFinishMainEvent();
            }
        } else {
            this.current_scale -= 2.0f;
            print("UpdateScaleDown");
            if(this.current_scale < min_scale) {
                OnFinishScalingDown();
            }
        }

        this.current_scale = Mathf.Clamp(this.current_scale, min_scale, max_scale);
    }


    private void UpdateFloor() {
        if(_floorAnimator != null) {
            _floorAnimator.transform.localScale = current_radius * _floorScaler + Vector3.up;
        }
    }

    private void UpdateDSet() {
        if(_duplicatedDset != null) {
              _duplicatedDset.transform.position = new Vector3(_duplicatedDset.transform.position.x, 1.0f + current_radius/10.0f, _duplicatedDset.transform.position.z);
              _duplicatedDset.transform.Rotate(0,5.0f*Time.deltaTime,0);
        }
    }

    private void UpdateRoom() {
        if(_roomScaler != null) {
              _roomScaler.UpdateScale(current_scale);
        }
    }

    private void UpdateDSetScale() {
        if(_duplicatedDset != null) {
            _duplicatedDset.transform.localScale = current_scale * Vector3.one;
        }
    }

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

    public void OnExit() {

        DATASET_NAME = "Unknown";
        _waitingDSet = null;

        if(_animationFinished || _enlargingCube) {
          // Make cube small again, then make it go Down
          // MAke room right scale
          _forceScaleDown = true;
          _enlargingCube = false;
          _animationFinished = false;
          GoToGround[] screenControllers = (GoToGround[]) FindObjectsOfType<GoToGround>();
          foreach (GoToGround screen in screenControllers)
           {
               screen.SetReady(false);
           }
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

        GoToGround[] screenControllers = (GoToGround[]) FindObjectsOfType<GoToGround>();
        foreach (GoToGround screen in screenControllers)
         {
             screen.SetReady(true);
         }
    }

    private IEnumerator EnlargeCube() {
        float scale = _duplicatedDset.transform.localScale.x;
        int i=0;
        while(_duplicatedDset.transform.localScale.x < max_scale) {
            scale += scaling_speed*i;
            _duplicatedDset.transform.localScale += Vector3.one*scale;
            i+=1;
            yield return new WaitForSeconds(0.02f);
        }

        //Fade to black
        _fader.FadeIn();
        _animationFinished = true;
        _enlargingCube = false;
        yield break;
    }
}
