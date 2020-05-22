using UnityEngine;
using System;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

public class StackerSlice : MonoBehaviour
{
    public int filter = 1;
    public float space=2;
    public float height=1.5f;
    public GameObject slice;
    public List<GameObject> slices;
    public object[] textures;
    public string stackName = "stackOne";

    //public Object[] textures;;
    // Start is called before the first frame update
    void Start()
    {
        LoadStack(stackName);
        SpawnStack(new Vector3(1,0,0), new Vector3(1, 0, 0));
        //SpawnStack(new Vector3(0.3f, 0, 0.3f), new Vector3(1, 0, 0));
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
    public void CreateSlice(Texture t, Vector3 offset)
    {
        GameObject s = Instantiate(slice);
        slice.GetComponent<Transform>().position = offset + Vector3.up*height;
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



    public void SpawnStack(Vector3 direction, Vector3 startPos)
    {
        int i = 0;
        int u = 0;
        Vector3 offset = startPos;
        //float SliceWidth = slice.GetComponent<Transform>().localScale.y;
        float totalLength = slice.GetComponent<Transform>().localScale.x;
        //this.space = totalLength / textures.Length;
        //print(space);

        foreach (Texture t in textures)
        {
            i++;
            if (i % filter == 0)
            {
                CreateSlice(t, offset);
                offset += direction*this.space;
                Debug.Log(offset);
            }
        }
    }

    public void DestroyStack()
    {
        for (int i = 0; i < slices.Count; i++)
        {
            Destroy(slices[i]);
        }
        slices.Clear();
    }
    // Update is called once per frame
    void Update()
    {

    }
}
