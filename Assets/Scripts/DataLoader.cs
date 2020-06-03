using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataLoader : MonoBehaviour
{

    // Textures in time, space
    public static Texture[,] DATASET_COLORED;

    //Public instances
    public Stack2DVisualizer _st2dVisualizer;
    public int _timesteps;
    public int _depthsteps;

    //Dataset to load
    private string _dataset = null;

    void Start() {
        _timesteps = Mathf.Clamp(_timesteps, 5, 50);
        _depthsteps = Mathf.Clamp(_depthsteps, 5,125);
    }

    public void LoadImages(string dataset)
    {
        if(dataset != null)  {

            _dataset = dataset;

            //Initialization of array
            DATASET_COLORED = new Texture[_timesteps, _depthsteps];

            //Filling array with 2D textures
            StartCoroutine("Load");
        }
    }

    IEnumerator Load() {
      for (int i=0; i<_timesteps; i++) {

          //Image path
          string path;
          if(i<10) path = "/t00"+i;
          else     path = "/t0"+i;

          // Store texture in array
          for (int j = 0; j<_depthsteps; j++) {
              Object[] objects = Resources.LoadAll(_dataset+"_colored"+path+path+"_"+j, typeof(Texture2D));
              DATASET_COLORED[i, j] = (Texture)objects[0];
              yield return null;
          }
          print("Loaded Time: " + i);
          yield return null;
      }

      //Initialize screens
      _st2dVisualizer.SetReady(true);
      print("Finished Loading !");
      yield return null;
    }


    // Called when the dataset is removed from the Pedestal
    public void StopLoading(string dset)
    {
        _dataset = null;
        StopCoroutine("Load");
        for(int time = 0; time < _timesteps; time++) {
            for (int depth = 0; depth<_depthsteps; depth++) {
                DATASET_COLORED[time, depth] = null;
            }
        }
    }
}
