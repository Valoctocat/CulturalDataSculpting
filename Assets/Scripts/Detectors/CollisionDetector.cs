using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{

    //Delegate function and callbacks
    public delegate void CollisionEvent();
    private CollisionEvent callback_entering_collision;
    private CollisionEvent callback_leaving_collision;

    void OnTriggerEnter(Collider other) {
        if(callback_entering_collision!=null) callback_entering_collision();
    }
    void OnTriggerStay(Collider other){
        if(callback_entering_collision!=null) callback_entering_collision();

      //QuestDebugHelper.Instance.Log(Time.frameCount, 2);
    }

    void OnTriggerExit(Collider other){
        if(callback_entering_collision!=null) callback_leaving_collision();
    }

    //Sets the type of callback
    public void on_entering_collision(CollisionEvent callback) {
        callback_entering_collision = callback;
    }

    public void on_leaving_collision(CollisionEvent callback) {
        callback_leaving_collision = callback;
    }
}
