using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    // Public variable instances
    public float speed;
    public AnimationPlayer _animationPlayer;

    // Private variable instances
    private ParticleSystem particleSystem;
    private ParticleSystem.MainModule settings;
    private ParticleSystemRenderer psr;

    private GameObject _dataset;
    private Vector3 scaler;
    private bool inside_radius;

    // Start is called before the first frame update
    void Start()
    {
      inside_radius = false;
      _dataset = null;
      particleSystem = GetComponent<ParticleSystem>();
      settings = particleSystem.main;
      psr = GetComponent<ParticleSystemRenderer>();
      settings.startColor = new Color(1,1,1,1);

      scaler = new Vector3(particleSystem.transform.localScale.x, particleSystem.transform.localScale.y, 0.0f);
    }

    // Update is called once per frame
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
            _animationPlayer.OnEnter(_dataset);
            _dataset.GetComponent<CubeController>().isTriggered(true);
        }
    }

    public void OnTriggerExit(Collider other) {
        if(other.gameObject.tag == "Dataset") {
            print("Here" + Time.frameCount);
            settings.startColor = new Color(1,1,1,1);
            inside_radius = false;
            _dataset.GetComponent<CubeController>().isTriggered(false);
            _dataset = null;
            _animationPlayer.OnExit();
        }
    }
}
