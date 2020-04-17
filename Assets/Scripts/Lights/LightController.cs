using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    //Public Variable instances
    public float offset;
    public float frequency;
    public float max_intensity;

    //private varaible instances
    private Light light;
    // Start is called before the first frame update
    void Start()
    {
        light = gameObject.GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        light.intensity = max_intensity * Mathf.Cos(Time.frameCount * frequency + offset/frequency);
    }
}
