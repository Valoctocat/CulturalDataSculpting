using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToGround : MonoBehaviour
{
    public Transform playerTransform;
    public Transform screenTransform;
    public Rigidbody rb;
    public GameObject thisScreen;
    public Vector3 stackDirection = new Vector3(0, 0, 1);
    public float SCREEN_HEIGHT = 5;
    public float SPEED_MULTIPLIER = 1;
    public float HEIGHT_STACK_APPEAR = 2;
    public float STACK_SCREEN_DISTANCE;
    
    float distance;
    float dissolveSpeed = 0;
    float initialHeight;
    // Start is called before the first frame update

    void Start()
    {
        initialHeight = screenTransform.position.y/2;
    }

    // Update is called once per frame
    void Update()
    {
        distance = (playerTransform.position - screenTransform.position).magnitude;
        dissolveSpeed = distance - SCREEN_HEIGHT;
        
        if(screenTransform.position.y< initialHeight)
            rb.velocity = new Vector3(0, dissolveSpeed * SPEED_MULTIPLIER, 0);
        else
            rb.velocity = new Vector3(0, 0, 0);

        if(screenTransform.position.y < HEIGHT_STACK_APPEAR)
        {
            Vector3 startPoint = screenTransform.position + STACK_SCREEN_DISTANCE * Vector3.Normalize(stackDirection);
            thisScreen.GetComponent<StackerSlice>().SpawnStack(stackDirection, startPoint);
        }

    }
}
