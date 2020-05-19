using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Stack2DVisualizer : MonoBehaviour
{
    //Public instances
    public float delay;
    public GameObject curved_plane_prefab;
    public int nb_screens;

    //private instances
    private GameObject[] screens;
    private TextureApplier[] screensAppliers;
    private bool[] playing;
    private Object[] objects;
    private Texture[] textures;

    // Start is called before the first frame update
    void Start()
    {

        // Initialization of screens
        screens = new GameObject[nb_screens];
        screensAppliers = new TextureApplier[nb_screens];

        playing = new bool[screens.Length];
        for (int i=0; i<playing.Length; i++) {
            playing[i] = false;
            instantiatePrefab(i);
        }

        //Loading images
        StartCoroutine("LoadImages");
    }


    void Update() {
        //Create new Coroutine if there is still some room
        for (int i=0; i<screens.Length; i++) {
            if(!playing[i]) {
                StartCoroutine("playLoop", i);
                playing[i] = true;
            }
        }
    }


    IEnumerator LoadImages() {
        if(AnimationPlayer.DATASET_NAME != "Unknown") {
            this.objects = Resources.LoadAll("droso", typeof(Texture2D));
            this.textures = new Texture[this.objects.Length];
            for (int i = 0; i<this.objects.Length; i++) {
                this.textures[i] = (Texture)this.objects[i];
            }
            print(textures.Length);

          } else {
            // For now we play droso by default
            this.objects = Resources.LoadAll("droso", typeof(Texture2D));
            this.textures = new Texture[this.objects.Length];
            for (int i = 0; i<this.objects.Length; i++) {
                this.textures[i] = (Texture)this.objects[i];
            }
            print(textures.Length);
          }
          yield return null;
    }

    IEnumerator playLoop(int screen) {
        yield return new WaitForSeconds(0.05f);

        //Main Loop
        for(int frame_counter = 0; frame_counter<this.objects.Length; frame_counter++) {
                yield return new WaitForSeconds(delay);
                screensAppliers[screen].applyTexture(this.textures[frame_counter]);
            }


        playing[screen] = false;
    }



    private void instantiatePrefab(int i) {
      // Postion & Orientation
      Vector3 position_curved_plane = curved_plane_prefab.transform.position;
      Quaternion orientation_curved_plane = Quaternion.Euler(270, i*360.0f/nb_screens, 0);

      // Instantiate
      screens[i] = Instantiate(curved_plane_prefab, position_curved_plane, orientation_curved_plane);
      screens[i].AddComponent<TextureApplier>();
      screensAppliers[i] = screens[i].GetComponent<TextureApplier>();
    }
}
