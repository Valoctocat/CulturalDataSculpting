using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Stack2DVisualizer : MonoBehaviour
{
    //Public instances
    public float delay;
    public GameObject curved_plane_prefab;
    public int nb_screens;
    public float _ditsToCenter = 4.5f;
    public int _timeOffsetBetweenScreens = 2;
    public GameObject ControllerRightHand;
    public GameObject user;

    //private instances
    private GameObject[] screens;
    private TextureApplier[] screensAppliers;
    private ControllerGetter _controllerGetterRightHand;
    private IEnumerator[] screenCoroutines;
    private bool[] _looping;
    private bool _ready;
    private bool[] _paused;
    private float[] _selectedTimeFloat;
    private float[] _selectedDepthFloat;
    private int[] _selectedTime;
    private int[] _selectedDepth;

    // Start is called before the first frame update
    void Start()
    {

        // Initialization of screens
        screens = new GameObject[nb_screens];
        screensAppliers = new TextureApplier[nb_screens];
        _controllerGetterRightHand = ControllerRightHand.GetComponent<ControllerGetter>();
        screenCoroutines = new IEnumerator[nb_screens];

        _looping = new bool[screens.Length];
        _paused = new bool[screens.Length];
        _selectedTime = new int[nb_screens];
        _selectedDepth = new int[nb_screens];
        _selectedTimeFloat = new float[nb_screens];
        _selectedDepthFloat = new float[nb_screens];
        for (int i=0; i<_looping.Length; i++) {
            _looping[i] = false;
            _paused[i] = false;
            _selectedTime[i] = i;
            _selectedDepth[i] = 0;
            _selectedTimeFloat[i] = i;
            _selectedDepthFloat[i] = 0;
            instantiatePrefab(i);
        }

        this.transform.position = new Vector3(0.0f, -4.0f,0.0f);
    }

    // If GetStop
    // StopCoroutine of closest screen
    // set to stopped
    // Get input from joystick to update image
    // Wait for GetStop again on this screen
    void Update() {

      if(_ready) {
          if(_controllerGetterRightHand.GetPause()) {

              //Pause Closest Screen
              int screen = GetClosestScreen();
              _paused[screen] = !_paused[screen];

              if(_paused[screen]){
                  StopCoroutine(screenCoroutines[screen]);
                  _looping[screen] = false;
              }
          }
          //Create new Update Image of each Screen
          for (int i=0; i<screens.Length; i++) {
            if(!_paused[i]) {
              if(!_looping[i]) {
                  screensAppliers[i].SetBackground();
                  screenCoroutines[i] = playLoop(i);
                  StartCoroutine(screenCoroutines[i]);
                  _looping[i] = true;
              }
            }
            else {
              Vector2 currentInput = _controllerGetterRightHand.getJoystickInput();
              if(currentInput.magnitude>0.1f) {
                  _selectedTimeFloat[i] = (_selectedTimeFloat[i] + currentInput.y/10.0f);
                  _selectedDepthFloat[i] = (_selectedDepthFloat[i] + currentInput.x/10.0f);
                  _selectedTime[i] = (int)_selectedTimeFloat[i]%DataLoader.DROSO_COLORED.GetLength(0);
                  _selectedDepth[i] = (int)_selectedDepthFloat[i]%DataLoader.DROSO_COLORED.GetLength(1);
                  screensAppliers[i].applyTexture(DataLoader.DROSO_COLORED[_selectedTime[i],_selectedDepth[i]]);
              }
          }
        }
      }
    }

    public void SetReady(bool boolean) {
        _ready = boolean;
        if(!boolean) {
          for (int i=0; i<screens.Length; i++) {
            StopCoroutine(screenCoroutines[i]);
            _looping[i] = false;
            _selectedTimeFloat[i] = i;
            _selectedDepthFloat[i] = 0;
            _selectedTime[i] = i;
            _selectedDepth[i] = 0;
          }
        }
    }

    private int GetClosestScreen() {
        float minDist = 100.0f;
        int bestScreen = 0;
        for (int i= 0; i<nb_screens; i++) {
            if(Vector3.Distance(screens[i].transform.position, user.transform.position) < minDist) {
              minDist = Vector3.Distance(screens[i].transform.position, user.transform.position);
              bestScreen = i;
            }
        }
        return bestScreen;
    }

    /*
    IEnumerator LoadImages() {
        if(AnimationPlayer.DATASET_NAME != "Unknown") {
            this.objects = Resources.LoadAll(AnimationPlayer.DATASET_NAME + "_processed/t001/", typeof(Texture2D));
            this.textures = new Texture[this.objects.Length];
            for (int i = 0; i<this.objects.Length; i++) {
                this.textures[i] = (Texture)this.objects[i];
            }

          } else {
            // For now we play droso by default
            this.objects = Resources.LoadAll("droso", typeof(Texture2D));
            this.textures = new Texture[this.objects.Length];
            for (int i = 0; i<this.objects.Length; i++) {
                this.textures[i] = (Texture)this.objects[i];
            }
          }
          yield return null;
    }
    */
    IEnumerator playLoop(int screen) {
        yield return new WaitForSeconds(0.05f);

        //Main Loop
        while(true) {
            for(int frame_counter = _selectedDepth[screen]; frame_counter<DataLoader.DROSO_COLORED.GetLength(1); frame_counter++) {
                yield return new WaitForSeconds(delay);
                _selectedDepthFloat[screen] = frame_counter;
                _selectedDepth[screen] = frame_counter;
                screensAppliers[screen].applyTexture(DataLoader.DROSO_COLORED[_selectedTime[screen],_selectedDepth[screen]]);
            }
            _selectedDepth[screen] = 0;
        }
    }



    private void instantiatePrefab(int i) {
      // Postion & Orientation
      Vector3 position_curved_plane = curved_plane_prefab.transform.position;
      Quaternion orientation_curved_plane = Quaternion.Euler(0, i*360.0f/nb_screens, 0);

      // Instantiate
      screens[i] = Instantiate(curved_plane_prefab, position_curved_plane, orientation_curved_plane);
      screens[i].transform.position -= screens[i].transform.forward*_ditsToCenter;
      screens[i].transform.rotation *= Quaternion.Euler(0,90.0f, 0);
      screens[i].AddComponent<TextureApplier>();
      screensAppliers[i] = screens[i].GetComponent<TextureApplier>();
      screens[i].gameObject.transform.parent = this.transform;
    }
}
