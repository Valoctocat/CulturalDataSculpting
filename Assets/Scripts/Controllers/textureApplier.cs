using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureApplier : MonoBehaviour
{
    //private intstances
    private Material current_material;

    // Start is called before the first frame update
    void Awake()
    {
        this.current_material = GetComponent<Renderer>().materials[1];
        //this.current_material.color = new Color(1,1,1,1);

    }
    public void SetBackground() {
      this.current_material.color = new Color(0.8f,0.8f,0.8f,1.0f);
    }
    public void applyTexture(Texture texture) {
        this.current_material.mainTexture = texture;
    }
}
