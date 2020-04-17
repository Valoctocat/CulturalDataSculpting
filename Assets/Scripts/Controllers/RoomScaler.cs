using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomScaler : MonoBehaviour
{
    //Public instances
    public GameObject room;
    public float scaling_speed;
    public float end_scale;
    public GameObject StackDisplayer;

    //private instances
    private float current_scale;
    private Stack2DVisualizer s2dv;
    private bool alreadyTriggered = false;

    void Start() {
        current_scale = room.transform.localScale[0];
        this.s2dv = StackDisplayer.GetComponent<Stack2DVisualizer>();
    }

    public void Triggered() {
        if(room != null && !alreadyTriggered) {
            StartCoroutine("ScaleRoom");
            s2dv.setScaling();
            alreadyTriggered = true;
        }
        return;
    }

    IEnumerator ScaleRoom() {
      yield return new WaitForSeconds(0.05f);
      int i =1;
      while(current_scale < end_scale) {
          yield return new WaitForSeconds(0.05f);
          current_scale += scaling_speed*i;
          this.room.transform.localScale = current_scale * Vector3.one;
          print(current_scale);
          i++;
      }
    }

}
