using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToGround : MonoBehaviour
{
    public Vector3 stackDirection = new Vector3(1, 0, 0);
    public float SCREEN_HEIGHT = 5;
    public float SPEED_MULTIPLIER = 1;
    public float HEIGHT_STACK_APPEAR = 0.5f;
    public float STACK_SCREEN_DISTANCE;

    float distance;
    float dissolveSpeed = 0;
    float initialHeight;

    private GameObject Player;
    private bool _ready = false;
    private bool _spawned = false;

    // Start is called before the first frame update

    void Start()
    {
        initialHeight = 1.5f;
        Player = GameObject.FindWithTag("User");

    }

    // Update is called once per frame
    void Update()
    {

        distance = (Player.transform.position - this.transform.position).magnitude;
        dissolveSpeed = distance - SCREEN_HEIGHT;
        /*
        if(this.transform.position.y< initialHeight)
            rb.velocity = new Vector3(0, dissolveSpeed * SPEED_MULTIPLIER, 0);
        else
            rb.velocity = new Vector3(0, 0, 0);
        */
        if(_ready && !_spawned) {
            if(this.transform.position.y < HEIGHT_STACK_APPEAR)
            {
                SpawnStack();
                _spawned = true;
            }
        }
    }

    public void SpawnStack() {
        Vector3 startPoint = this.transform.position + STACK_SCREEN_DISTANCE *2* Vector3.Normalize(this.transform.right);
        this.GetComponent<StackerSlice>().SpawnStack(this.transform.right, startPoint, Quaternion.LookRotation(this.transform.right));
    }

    public void SetReady(bool setter) {
        _ready = true;
    }
}
