using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
{

    /*
    Controls the interactions with the physical cube of a dataset
    Update colors of the cube depending on grabbing or shimmering
    Calls UI display on grab
    */

    // Public instances
    public float _probabilityShine;
    public GameObject _UI;

    //Private instances
    private Material _material;
    private Color _initColor;
    private Color _initEmissionColor;
    private bool _wasGrabbed = false;
    private bool _interacted = false;

    //Other scripts
    private OVRGrabbable _grabbable;
    private AudioController _audioController;
    private UIManager _UIManager;

    // Start is called before the first frame update
    void Start()
    {
        _material = this.GetComponent<Renderer>().materials[0];
        _initColor = _material.color;
        _initEmissionColor = _material.GetColor("_EmissionColor");

        _grabbable = this.GetComponent<OVRGrabbable>();
        _audioController = this.GetComponent<AudioController>();
        _UIManager = _UI.GetComponent<UIManager>();
    }

    void Update()
    {
        // If it is grabbed for the first time: Set to bright Color, Display UI
        if(_grabbable.isGrabbed) {
            if(!_wasGrabbed) {
                _wasGrabbed = true;
                _material.color = _initColor +  new Color(0,0,0, 0.5f);
                _material.SetColor("_EmissionColor", _initEmissionColor);
                _UIManager.OnGrab(this);
            }
        }
        else {
            //If it is released for the first time: stop UI, Reset colors
            if(_wasGrabbed){
                _UIManager.OnRelease(this);
                _wasGrabbed = false;
                _material.color = _initColor;
                _material.SetColor("_EmissionColor", _initEmissionColor);
            }

            // If it has been put in the microscope: stay still
            // Else: Probabilistic shimerring
            if((!_audioController.isPlaying()) & (Random.Range(0.0f, 1.0f)<_probabilityShine) & (!_interacted)) {
                _audioController.PlayCharacteristic(5.0f);
                StartCoroutine(ChangeColor(5.0f));
            }
        }
    }

    // Called when the dataset hits the microscope
    public void isTriggered(bool isTriggered) {
        _interacted = isTriggered;
        if(isTriggered) {
            _material.color = _initColor +  new Color(0,0,0, 0.5f);
        }
        else {
            _material.color = _initColor;
            _material.SetColor("_EmissionColor", _initEmissionColor);
        }
    }

    // Coroutine to do shimmering of colors
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
