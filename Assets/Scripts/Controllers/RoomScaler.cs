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
            print("Triggered");
            StartCoroutine("ScaleRoom");
            alreadyTriggered = true;
        }
        return;
    }

    public void UpdateScale(float scale) {
        float final_scale = Mathf.Clamp(scale, current_scale, end_scale);
        this.room.transform.localScale = final_scale * Vector3.one;
    }

    IEnumerator ScaleRoom() {
      print("ScaleRoom");
      yield return new WaitForEndOfFrame();
      int i =1;
      print(current_scale + " " + end_scale);
      while(current_scale < end_scale) {
          current_scale += scaling_speed*i;
          print(current_scale);
          this.room.transform.localScale = current_scale * Vector3.one;
          i++;
          yield return new WaitForSeconds(0.02f);
      }
      yield break;
    }

}
