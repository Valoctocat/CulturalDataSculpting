using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static OVRHand;

public class FingerPinchedDetector : MonoBehaviour
{
  // Delegate functions and callbacks
  public delegate void HandEventType();
  private HandEventType callback_entering_pinching_interaction;
  private HandEventType callback_leaving_pinching_interaction;

  // OVR hands
  private OVRHand left_hand;
  private OVRHand right_hand;

  void Start(){
    // Retreive the instance of the left hand
      GameObject tmp = GameObject.Find( "LeftHandAnchor" );
      if ( tmp == null ){
        Debug.LogError( "Unable to fing the left hand !" );
        return;
      }
      left_hand = tmp.GetComponentInChildren<OVRHand>();

      // Retreive the instance of the right hand
      tmp = GameObject.Find( "RightHandAnchor" );
      if( tmp == null ){
        Debug.LogError( "Unable to find the right hand !" );
        return;
      }
      right_hand = tmp.GetComponentInChildren<OVRHand>();
  }

  void Update(){
    QuestDebugHelper.Instance.Log(gameObject.transform.position.x, 1);
    QuestDebugHelper.Instance.Log(gameObject.transform.position.z, 2);
    // Check pinching both index
    if(right_hand.GetFingerIsPinching(HandFinger.Index)){
      if(callback_entering_pinching_interaction != null) callback_entering_pinching_interaction();
    }

    // Check for falling edge of the interaction
    if(!right_hand.GetFingerIsPinching( HandFinger.Index )){
      if(callback_leaving_pinching_interaction != null) callback_leaving_pinching_interaction();
    }
  }

  public void on_entering_pinching_interaction(HandEventType callback) {
      callback_entering_pinching_interaction = callback;
  }
  public void on_leaving_pinching_interaction(HandEventType callback) {
      callback_leaving_pinching_interaction = callback;
  }
}
