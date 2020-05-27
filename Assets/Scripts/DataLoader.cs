using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataLoader : MonoBehaviour
{

    public static Texture[,] DROSO_COLORED;
    public Stack2DVisualizer _st2dVisualizer;


    public int _timesteps;
    public int _depthsteps;

    private string _dataset = null;

    // TODO : Should be in a function
    public void LoadImages(string dataset)
    {
        if(dataset != null)  {

            _dataset = dataset;

            //Initialization of array
            DROSO_COLORED = new Texture[_timesteps, _depthsteps];

            //Filling array with 2D textures
            StartCoroutine("Load");
        }
    }

    IEnumerator Load() {
      for (int i=0; i<_timesteps; i++) {
          //Object[] objects = new Object[_depthsteps];
          /*
          if(i<10) objects = Resources.LoadAll(_dataset+"_colored/t00"+i+"/", typeof(Texture2D));
          else     objects = Resources.LoadAll(_dataset+"_colored/t0"+i+"/", typeof(Texture2D));
          */
          string path;
          if(i<10) path = "/t00"+i;
          else     path = "/t0"+i;
          // Store texture in array
          for (int j = 0; j<_depthsteps; j++) {
              Object[] objects = Resources.LoadAll(_dataset+"_colored"+path+path+"_"+j, typeof(Texture2D));
              DROSO_COLORED[i, j] = (Texture)objects[0];
              yield return null;
          }
          print("Loaded Time: " + i);
          yield return null;
      }
      _st2dVisualizer.SetReady(true);
      print("Finished Loading !");
      yield return null;
    }


    // Update is called once per frame
    void Update()
    {

    }
}
