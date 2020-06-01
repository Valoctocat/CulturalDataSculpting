using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureApplier : MonoBehaviour
{
    /*
      Apply a texture on the attached object
    */
    //private intstances
    private Material current_material;

    // Start is called before the first frame update
    void Start()
    {
        this.current_material = GetComponent<Renderer>().materials[1];

    }
    public void SetBackground() {
      this.current_material.color = new Color(0.8f,0.8f,0.8f,0.8f);
    }
    public void applyTexture(Texture texture) {
        if(texture != null) {
            this.current_material.mainTexture = texture;
        }
    }
}
