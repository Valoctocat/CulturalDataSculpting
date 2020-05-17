using UnityEngine;
using System;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

public class StackerCube : MonoBehaviour
{
    public int filter = 1;
    public float space=2;
    public GameObject slice;
    public List<GameObject> slices;
    public object[] textures;
    public string stackName = "stackOne";
    //public Object[] textures;;
    // Start is called before the first frame update
    void Start()
    {
        LoadStack(stackName);
        SpawnStack();
    }

    public void LoadStack(string stackName/*GameObject s*/)
    {
        int i = 0;
        
        //set the texture from the data path stored in the structure 
        textures = Resources.LoadAll(stackName, typeof(Texture2D));
        Debug.Log("number is "+ textures.Count());
        

       // s.GetComponent<Renderer>().material.SetTexture("bob", textures[i]);
       // current_mat.mainTexture = texture;
    }
    public void CreateSlice(Texture t, float offset)
    {
        GameObject s = Instantiate(slice);
        slice.GetComponent<Transform>().position = new Vector3(offset,0,0);
        SetTexture(s,t);
        slices.Add(s);
        
    }

    public void SetTexture(GameObject obj, Texture t) 
    {
        Material mat = obj.GetComponent<Renderer>().material;
        mat.mainTexture = t;
        mat.EnableKeyword("_DETAIL_MULX2");
        mat.SetTexture("_DetailAlbedoMap", t);
        obj.GetComponent<Renderer>().material = mat;
    }



    public void SpawnStack()
    {
        int i = 0;
        int u = 0;
        float offset = 0f;
        //float SliceWidth = slice.GetComponent<Transform>().localScale.y;
        //float totalLength = slice.GetComponent<Transform>().localScale.x;
        space += slice.GetComponent<Transform>().localScale.x;
        foreach (Texture t in textures)
        {
            i++;
            if (i % filter == 0)
            {
                CreateSlice(t, offset);
                offset += space;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
