using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomScaler : MonoBehaviour
{

    /*
        Enlarge or Shrink the "Element to scales" according to instruction from Animation Player.
    */

    //Public instances
    public GameObject room;
    public GameObject stackVisualizer;
    public float scaling_speed;
    public float end_scale;
    public float raisingScreenSpeed;

    //private instances
    private float current_scale;
    private bool alreadyTriggered = false;

    void Start() {
        current_scale = room.transform.localScale[0];
    }

    public void UpdateScale(float scale) {
        float final_scale = Mathf.Clamp(scale*scaling_speed, current_scale, end_scale);
        this.room.transform.localScale = final_scale * Vector3.one;

        float factor = Mathf.Clamp(scale*raisingScreenSpeed, 0.0f, 4.0f);
        this.stackVisualizer.transform.position = Vector3.up * factor - (4*Vector3.up);
    }
}
