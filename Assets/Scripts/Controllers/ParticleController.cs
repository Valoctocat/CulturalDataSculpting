using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{


    /*

      Manages the ring of particules on the microscop
      Handles Interaction with datasets:
        If a dataset enters the ring => Starts animation and Call IsTrigger of the CubeController
        If leaves => Stop animation 

    */


    // Public variable instances
    public float speed;   //Rotation speed
    public AnimationPlayer _animationPlayer;

    // Private variable instances
    private ParticleSystem particleSystem;
    private ParticleSystem.MainModule settings;
    private ParticleSystemRenderer psr;

    private GameObject _dataset = null;  //Interacting dataset
    private Vector3 scaler; //Changes the scale of the ring
    private bool inside_radius = false; //If a dataset is in the ring collider

    void Start()
    {

      particleSystem = GetComponent<ParticleSystem>();
      settings = particleSystem.main;
      psr = GetComponent<ParticleSystemRenderer>();
      settings.startColor = new Color(1,1,1,1);

      scaler = new Vector3(particleSystem.transform.localScale.x, particleSystem.transform.localScale.y, 0.0f);
    }

    void Update()
    {
        // Animate ring
        gameObject.transform.Rotate(0,0,speed*Time.deltaTime);
        if(!inside_radius) {
            gameObject.transform.localScale = new Vector3(0.25f*Mathf.Sin(Time.time) + 0.75f, 0.25f*Mathf.Sin(Time.time) + 0.75f, 1.0f);
        }
    }



    public bool GetInside() {
        return inside_radius;
    }

    public void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Dataset") {
            settings.startColor = new Color(1.0f, 0.8f, 0.8f);
            inside_radius = true;
            _dataset = other.gameObject;

            //Starts animation for the given dataset
            _animationPlayer.OnEnter(_dataset);

            // Warns dataset it has been interacting with the microscpe
            if(_dataset.GetComponent<CubeController>() != null)  _dataset.GetComponent<CubeController>().isTriggered(true);
        }
    }

    public void OnTriggerExit(Collider other) {
        if(other.gameObject.tag == "Dataset") {
            settings.startColor = new Color(1,1,1,1);
            inside_radius = false;
            if(_dataset.GetComponent<CubeController>() != null) _dataset.GetComponent<CubeController>().isTriggered(false);
            _dataset = null;

            //Stop/Revers animation
            _animationPlayer.OnExit();
        }
    }
}
