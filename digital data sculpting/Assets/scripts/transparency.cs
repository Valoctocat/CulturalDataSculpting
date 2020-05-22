using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 
public class transparency : MonoBehaviour
{
    bool front;
    bool frontBefore = false;
    public const double DISTANCE=1.0;
    public Transform playerTransform;
    public Transform sliceTransform;
    Vector3 up = new Vector3(0, 1, 0);
    // Start is called before the first frame update
    void Start()
    {
    }
    public void FacingCheck()
    {
        if ((sliceTransform.position.x - playerTransform.position.x) < 0)
            front = true;
        else
            front = false;
        if(front ^ frontBefore)
        {
            transform.Rotate(Vector3.up, 180, Space.World);
            //transform.Rotate(Vector3.forward, 180, Space.World);
            frontBefore = !frontBefore;
        }
    }
    // Update is called once per frame
    void Update()
    {
        FacingCheck();
        //transform.Rotate(Vector3.back, 1, Space.World);

    }
}
