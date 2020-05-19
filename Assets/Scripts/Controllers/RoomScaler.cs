using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomScaler : MonoBehaviour
{
    //Public instances
    public GameObject room;
    public float scaling_speed;
    public float end_scale;

    //private instances
    private float current_scale;
    private bool alreadyTriggered = false;

    void Start() {
        current_scale = room.transform.localScale[0];
    }

    public void Triggered() {
        if(room != null && !alreadyTriggered) {
            StartCoroutine("ScaleRoom");
            alreadyTriggered = true;
        }
        return;
    }

    IEnumerator ScaleRoom() {
      yield return new WaitForEndOfFrame();
      int i =1;
      while(current_scale < end_scale) {
          current_scale += scaling_speed*i;
          this.room.transform.localScale = current_scale * Vector3.one;
          i++;
          yield return null;
      }
      yield break;
    }

}
