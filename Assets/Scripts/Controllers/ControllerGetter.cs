using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerGetter : MonoBehaviour
{

    /*

    Handles the input from the User

    */

    //Defines types for this class
    public enum AnchorSideType : int {
        LeftHand = 0,
        RightHand = 1
    };

    //Public instances
    public bool controllers = true;     //Allows to switch between controllers or keyboard for debugging
    public AnchorSideType _anchorSide;


    void Start() {
        QualitySettings.vSyncCount = 0; //May be removed
    }

    void Update()
    {
        // Update Inputs
        OVRInput.Update(); //May be removed
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
          } else {
              factor = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
          }
      } else {
          if(_anchorSide == AnchorSideType.RightHand) {
            if(Input.GetKey("a")) factor.x = 1.0f ;
            if(Input.GetKey("z")) factor.x = -1.0f;
            if(Input.GetKey("e")) factor.y = 1.0f;
            if(Input.GetKey("r")) factor.y = -1.0f;
          } else {
            //TODO defines some keyboard inputs for left hand
          }
      }

      return factor;
    }

    public bool GetPause() {
      bool cancel_input = false;
      if(controllers) {
          if(_anchorSide == AnchorSideType.RightHand) cancel_input = OVRInput.GetDown(OVRInput.Button.SecondaryThumbstick);
          else cancel_input = OVRInput.GetDown(OVRInput.Button.PrimaryThumbstick);
      } else {
          if(_anchorSide == AnchorSideType.RightHand) cancel_input = Input.GetKeyDown("p");
          else{} //TODO define keyboard input for left hand
      }

      return cancel_input;
    }
}
