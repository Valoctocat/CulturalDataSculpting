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
    public float max_scale = 600.0f;
    public OVRScreenFade _fader;


    private bool _forceStop = false;
    private bool _mainAnimation = false;
    private GameObject _dataset;
    private float current_radius;

    private float timer;
    private Vector3 _floorScaler;

    void Start() {
        _floorScaler = new Vector3(_floorAnimator.transform.localScale.x, 0.0f, _floorAnimator.transform.localScale.z);
        current_radius = min_radius;
    }

    void Update()
    {
        if(!_mainAnimation)  {
            // Update radius of floor
            UpdateRadius();

            //Update floor
            UpdateFloor();
            UpdateDSet();
        }
    }

    private void UpdateRadius() {
        if(_particleSystem.GetInside() && !_forceStop) {
            Debug.Log(this.current_radius);
            this.current_radius += this.timer*0.001f;
            Debug.Log(this.current_radius);
            this.timer+=1.0f;

            //Reset variables when Radius back to normal
            if(Mathf.Abs(this.current_radius-min_radius) < 0.0001){
                //OnFinish();
            }

            if(this.current_radius>max_radius){
                print("Triggering Main Event");
                OnTriggerMainEvent();
            }

        } else {
            this.current_radius -= 3.0f*Time.deltaTime;
        }

        this.current_radius = Mathf.Clamp(this.current_radius, 1.0f, max_radius);
    }


    private void UpdateFloor() {
        if(_floorAnimator != null) {
            _floorAnimator.transform.localScale = current_radius * _floorScaler + Vector3.up;
        }
    }

    private void UpdateDSet() {
      if(_dataset != null) {
            _dataset.transform.position = new Vector3(_dataset.transform.position.x, 0.45f + current_radius/10.0f, _dataset.transform.position.z);
            _dataset.transform.Rotate(0,5.0f*Time.deltaTime,0);
      }
    }


    public void OnEnter(GameObject dataset) {
        timer = 1.0f;
        _dataset = dataset;
    }

    public void OnExit() {
        _forceStop = true;
    }

    public void OnFinish() {
        _forceStop = false;
        timer = 1.0f;
        _dataset = null;
    }

    private void OnTriggerMainEvent() {
        _mainAnimation = true;
        _forceStop = false;
        DATASET_NAME = _dataset.name;

        StartCoroutine("EnlargeCube");
        _roomScaler.Triggered();
    }

    private IEnumerator EnlargeCube() {
        _dataset.GetComponent<Rigidbody>().isKinematic = true;
        float scale = _dataset.transform.localScale.x;
        int i=0;
        while(_dataset.transform.localScale.x < max_scale) {
            scale += 0.05f*i;
            _dataset.transform.localScale += Vector3.one*scale;
            i+=1;
            yield return null;
        }

        //Fade to black
        _fader.FadeIn();
        yield break;

    }
}
