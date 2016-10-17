using UnityEngine;
using System.Collections;

public class Token : MonoBehaviour {
    public Texture[] textures;
    //[HideInInspector]
    public int number;
	// Use this for initialization
	void Start () {
        if(number >= textures.Length || number < 0) number = 0;

        Renderer renderer = this.GetComponent<Renderer>();
        renderer.material.mainTexture = textures[number];
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
