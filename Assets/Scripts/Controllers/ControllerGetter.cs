using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerGetter : MonoBehaviour
{


    //Defines types for this class
    public enum AnchorSideType : int {
        LeftHand = 0,
        RightHand = 1
    };

    //Public instances
    public bool controllers = true;     //Allows to switch between controllers or keyboard for debugging
    public AnchorSideType _anchorSide;




    void Start() {
        QualitySettings.vSyncCount = 0;
    }

    void Update()
    {
        // Update Inputs
        OVRInput.Update();
    }


    ///////////////////////
    //
    //    Retrieving Controls
    //
    //////////////////////
    public Vector2 getJoystickInput() {
      Vector2 factor = new Vector2(0.0f,0.0f);

      if(controllers) {
          if(_anchorSide == AnchorSideType.RightHand) {
              factor = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
          }
      } else {
          if(_anchorSide == AnchorSideType.RightHand) {
            if(Input.GetKey("a")) factor.x = 1.0f ;
            if(Input.GetKey("z")) factor.x = -1.0f;
            if(Input.GetKey("e")) factor.y = 1.0f;
            if(Input.GetKey("r")) factor.y = -1.0f;
          }
      }


      return factor;
    }

    public bool GetPause() {
      bool cancel_input = false;
      if(controllers) {
          if(_anchorSide == AnchorSideType.RightHand) cancel_input = OVRInput.GetDown(OVRInput.Button.SecondaryThumbstick);
      } else {
          if(_anchorSide == AnchorSideType.RightHand) cancel_input = Input.GetKeyDown("p");
      }

      return cancel_input;
    }

    ///////////////////////
    //
    //    Vibration
    //
    //////////////////////
    public void SetControllerVibrationOn(float duration) {
        if(controllers) StartCoroutine("Vibrate", duration);
    }

    IEnumerator Vibrate(float duration) {
      if(_anchorSide == AnchorSideType.LeftHand)   OVRInput.SetControllerVibration(1.0f, 1.0f, OVRInput.Controller.LTouch);
      else                                        OVRInput.SetControllerVibration(1.0f, 1.0f, OVRInput.Controller.RTouch);

      yield return new WaitForSeconds(duration);
      if(_anchorSide == AnchorSideType.LeftHand)   OVRInput.SetControllerVibration(0,0, OVRInput.Controller.LTouch);
      else                                        OVRInput.SetControllerVibration(0,0, OVRInput.Controller.RTouch);
    }
}
