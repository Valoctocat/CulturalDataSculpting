using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    private Material _material;
    private Color _initColor;
    private Color _initEmissionColor;
    private bool _wasGrabbed = false;

    public float _probability;
    public GameObject UI;

    private OVRGrabbable _grabbable;
    private AudioController _audioController;
    private DatasetInterfaceController _interfaceController;
    private bool _interacted = false;
    private UIManager _UIManager;

    // Start is called before the first frame update
    void Start()
    {
        _material = this.GetComponent<Renderer>().materials[0];
        _initColor = _material.color;
        _initEmissionColor = _material.GetColor("_EmissionColor");

        _grabbable = this.GetComponent<OVRGrabbable>();
        _audioController = this.GetComponent<AudioController>();
        _interfaceController = this.GetComponent<DatasetInterfaceController>();
        _UIManager = UI.GetComponent<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_grabbable.isGrabbed) {
            if(!_wasGrabbed) {
                _audioController.OnGrab();
                _wasGrabbed = true;
                _material.color = _initColor +  new Color(0,0,0, 0.5f);
                _material.SetColor("_EmissionColor", _initEmissionColor);
                _UIManager.OnGrab(this);
            }
        }
        else {
            if(_wasGrabbed){
                _audioController.OnRelease();
                _UIManager.OnRelease(this);
                _wasGrabbed = false;
                _material.color = _initColor;
                _material.SetColor("_EmissionColor", _initEmissionColor);
            }

            if(_interacted) {
                _material.color = _initColor +  new Color(0,0,0, 0.5f);
            }
            else {
                if((!_audioController.isPlaying()) & (Random.Range(0.0f, 1.0f)<_probability)) {
                    _audioController.PlayCharacteristic(5.0f);
                    StartCoroutine(ChangeColor(5.0f));
                }
            }
        }
    }

    void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag == "Hand") {
            _interfaceController.OnStart();
        }
    }

    void OnCollisionExit(Collision other) {
        if(other.gameObject.tag == "Hand") {
            _interfaceController.OnStop();
        }
    }

    public void isTriggered(bool isTriggered) {
        _interacted = isTriggered;
    }

    public IEnumerator ChangeColor(float duration) {
        float currentTime = 0;
        float i=0;
        yield return new WaitForEndOfFrame();
        while(currentTime < duration / 2) {
            i+=1;
            _material.color = _initColor + Mathf.Lerp(0.0f,1.0f,currentTime/duration) * new Color(0.0f,0.0f,0.0f, 1.0f);
            _material.SetColor("_EmissionColor", _initEmissionColor + Mathf.Lerp(0.0f,2.0f,currentTime/duration) * new Color(0.5f,0.5f,0.5f, 0.0f));
            currentTime += Time.deltaTime;
            yield return null;
        }
        while(currentTime < duration) {
            i+=1;
            _material.color = _initColor + Mathf.Lerp(1.0f,0.0f,currentTime/duration) * new Color(0.0f,0.0f,0.0f, 1.0f);
            _material.SetColor("_EmissionColor", _initEmissionColor + Mathf.Lerp(2.0f,0.0f,currentTime/duration)* new Color(0.5f,0.5f,0.5f, 0.0f));
            currentTime += Time.deltaTime;
            yield return null;
        }
        _material.color = _initColor;
        _material.SetColor("_EmissionColor", _initEmissionColor);
        yield break;
    }
}
