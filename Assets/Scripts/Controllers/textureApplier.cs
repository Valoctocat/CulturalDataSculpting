using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class textureApplier : MonoBehaviour
{
    //private intstances
    private Material current_material;

    // Start is called before the first frame update
    void Awake()
    {
        this.current_material = GetComponent<Renderer>().materials[1];

    }

    public void applyTexture(Texture texture) {
        this.current_material.mainTexture = texture;
    }

    public void updateScale(Vector3 newScale, float new_y) {
        this.transform.localScale = newScale;
        //this.transform.Translate(Vector3.up*new_y);
    }

}
