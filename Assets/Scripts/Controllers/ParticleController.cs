using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    // Public variable instances
    public float speed;
    public GameObject OVR;
    public GameObject detector;

    // Private variable instances
    private ParticleSystem particleSystem;
    private ParticleSystem.MainModule settings;
    private FingerPinchedDetector hand_interaction_detector;
    private CollisionDetector collision_detector;
    private RoomScaler room_scaler;
    private ParticleSystemRenderer psr;

    private bool inside_radius;
    private bool pinching;
    private float timeInsidePinching = 0.0f;
    private bool triggerScaling;

    // Start is called before the first frame update
    void Start()
    {
      inside_radius = false;
      pinching = false;
      triggerScaling = false;
      particleSystem = GetComponent<ParticleSystem>();
      settings = particleSystem.main;
      psr = GetComponent<ParticleSystemRenderer>();
      settings.startColor = new Color(1,1,1,1);
      room_scaler = Object.FindObjectOfType<RoomScaler>();

      hand_interaction_detector = OVR.GetComponent<FingerPinchedDetector>();
      collision_detector = detector.GetComponent<CollisionDetector>();

      // Bind callbacks
      hand_interaction_detector.on_entering_pinching_interaction(on_entering_pinching);
      hand_interaction_detector.on_leaving_pinching_interaction(on_leaving_pinching);
      collision_detector.on_entering_collision(on_entering_circle);
      collision_detector.on_leaving_collision(on_leaving_circle);

      //StartCoroutine("TODODeleteMe");
    }

    // Update is called once per frame
    void Update()
    {
        // Animate ring
        gameObject.transform.Rotate(0,0,speed*Time.deltaTime);
        settings.startColor = new Color(1,1,1,1);

        if(inside_radius){
            settings.startColor = new Color(0.6f,1,0.6f,1);
            if(pinching){
                settings.startColor = new Color(0.6f,0.6f,1,1);
                timeInsidePinching += Time.deltaTime;
            }
        }

        //Room scaling trigger if needed
        if(timeInsidePinching > 2.0f) triggerScaling = true;
        if(triggerScaling) {
            psr.renderMode = ParticleSystemRenderMode.Billboard;
            room_scaler.Triggered();
        }
    }



    public void on_entering_pinching() {
          pinching = true;
    }

    public void on_leaving_pinching() {
          pinching = false;
    }

    public void on_entering_circle() {
          inside_radius = true;
    }

    public void on_leaving_circle() {
          inside_radius = false;
    }

    /*
    IEnumerator pinchingForTimeToTriggerScaling() {
      yield return new WaitForSeconds(0.05f);
      float time_passed = 0.0f;
      bool heldPinching_long_enough = true;

      while(time_passed < 2.0f) {
        if(!pinching || !inside_radius) {
          heldPinching_long_enough = false;
        }
        time_passed += Time.deltaTime;
      }

      if(heldPinching_long_enough) {
        triggerScaling = true;
      }
    }
    */

    IEnumerator TODODeleteMe() {
      yield return new WaitForSeconds(5.0f);
      triggerScaling = true;
    }

}
