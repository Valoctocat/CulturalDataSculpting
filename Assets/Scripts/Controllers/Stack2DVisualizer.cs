using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Stack2DVisualizer : MonoBehaviour
{
    //Public instances
    public GameObject curved_plane_prefab;
    public float delay;
    public int maxAlive;
    public float spawnProbability;


    //private instances
    private Object[] objects;
    private Texture[] textures;
    private List<int> current_orientations;
    private int currently_alive = 0;
    private bool scaling = false;

    // Start is called before the first frame update
    void Start()
    {
        //Initialization
        if(spawnProbability >1f || spawnProbability<0f) spawnProbability = 0.005f;
        current_orientations = new List<int>();

        //Load textures
        this.objects = Resources.LoadAll("droso", typeof(Texture2D));
        this.textures = new Texture[this.objects.Length];
        for (int i = 0; i<this.objects.Length; i++) {
            this.textures[i] = (Texture)this.objects[i];
        }
    }

    void Update() {
        //Create new Coroutine if there is still some room
        if( currently_alive<maxAlive && Random.Range(0.0f, 1.0f)<spawnProbability && !scaling) {
            StartCoroutine("playLoop");
            currently_alive++;
        }
    }

    private GameObject instantiatePrefab(int rotation_offset) {
      // Postion & Orientation
      Vector3 position_curved_plane = curved_plane_prefab.transform.position;
      Quaternion orientation_curved_plane = Quaternion.Euler(270, rotation_offset*40, 0);

      // Instantiate
      GameObject curved_plane = Instantiate(curved_plane_prefab, position_curved_plane, orientation_curved_plane);
      curved_plane.transform.parent = this.transform.parent;

      return curved_plane;
    }

    public void setScaling() {
        this.scaling = true;
    }



    IEnumerator playLoop() {
        //Instantiate
        yield return new WaitForSeconds(0.05f);
        int rotation_offset = Random.Range(0,8);
        float previous_scale = 1;
        while(current_orientations.Contains(rotation_offset)) {
            rotation_offset = Random.Range(0,8);
        }

        current_orientations.Add(rotation_offset);


        GameObject curved_plane = instantiatePrefab(rotation_offset);
        textureApplier texture_applier = curved_plane.GetComponent<textureApplier>();
        Vector3 initScale = curved_plane.transform.localScale;
        float init_y = curved_plane.transform.position.y;
        texture_applier.updateScale(Vector3.Scale(initScale, this.transform.parent.transform.localScale), init_y*this.transform.parent.transform.localScale[0]);

        //Main Loop
        for(int frame_counter = 0; frame_counter<this.objects.Length; frame_counter++) {
                yield return new WaitForSeconds(delay);
                texture_applier.applyTexture(this.textures[frame_counter]);
                //Update scale if Room is scaling up
                if(this.transform.parent.transform.localScale[0] > previous_scale) {
                  texture_applier.updateScale(Vector3.Scale(initScale, this.transform.parent.transform.localScale), init_y*this.transform.parent.transform.localScale[0]);
                }
            }

        //End
        Destroy(curved_plane);
        current_orientations.RemoveAll(item => item == rotation_offset);
        currently_alive--;
    }
}
