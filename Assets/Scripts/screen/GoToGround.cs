using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToGround : MonoBehaviour
{
    public float SCREEN_HEIGHT = 5;
    public float SPEED_MULTIPLIER = 3;
    public float HEIGHT_STACK_APPEAR = 0.5f;
    public float STACK_SCREEN_DISTANCE=1f;
    public float DISTANCE_START=2;
    public float WORLD_SIZE = 5;

    float distance;
    float dissolveSpeed = 0;
    float initialHeight;

    private GameObject Player;
    private bool _ready_stack=false;
    private bool _ready = false;
    public bool _spawned = false;
    private bool _topReached = false;

    // Start is called before the first frame update

    void Start()
    {

        initialHeight = 1.5f;
        Player = GameObject.FindWithTag("User");
        //SpawnStack();
    }

    // Update is called once per frame
    void Update()
    {
        if (_ready)
        {
            distance = (Player.transform.position - this.transform.position).magnitude;
            dissolveSpeed = distance - DISTANCE_START;
            float distanceFromCenter = (Player.transform.position - new Vector3(0, Player.transform.position.y, 0)).magnitude;


            if (this.transform.position.y > initialHeight)
            {
                _topReached = true;
                _ready_stack = true;
            }
            else
            {
                _topReached = false;
            }
            if (this.transform.position.y > HEIGHT_STACK_APPEAR)
            {
                _ready_stack = true;
            }
            if (_topReached)
            {
                if (dissolveSpeed < 0)
                {
                    this.transform.position += new Vector3(0, dissolveSpeed * SPEED_MULTIPLIER * Time.deltaTime, 0);
                }
            }
            else
            {
                this.transform.position += new Vector3(0, dissolveSpeed * SPEED_MULTIPLIER * Time.deltaTime, 0);
            }
            if (_ready_stack && !_spawned)
            {
                if (this.transform.position.y < HEIGHT_STACK_APPEAR)
                {
                    SpawnStack();
                    _spawned = true;
                }
            }
            else if (Player.transform.position.magnitude < WORLD_SIZE - DISTANCE_START)
            {

                _spawned = false;
                _ready_stack = false;
                this.GetComponent<StackerSlice>().DestroyStack();
            }
        }

    }

    public void SpawnStack() {
        Vector3 startPoint = this.transform.position + STACK_SCREEN_DISTANCE *2* Vector3.Normalize(this.transform.right);
       // startPoint.y += 2*(initialHeight - HEIGHT_STACK_APPEAR);


        this.GetComponent<StackerSlice>().SpawnStack(this.transform.right, startPoint, Quaternion.LookRotation(this.transform.right));
    }

    public void SetReady(bool setter) {
        _ready = setter;
        _ready_stack = setter;
    }
}
