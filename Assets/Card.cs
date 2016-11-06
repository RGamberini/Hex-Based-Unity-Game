using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Card : MonoBehaviour {
    public int scaleFactor = 2;
    public int index;
    public Texture rockTexture, brickTexture, sheepTexture, wheatTexture, woodTexture;
    private Vector3 cardSize;

    public HexType hexType {
        set
        {
            Material mainMaterial = this.GetComponent<Renderer>().material;
            switch (value) {
                case HexType.MOUNTAIN:
                    mainMaterial.mainTexture = rockTexture;
                    break;
                case HexType.PASTURE:
                    mainMaterial.mainTexture = sheepTexture;
                    break;
                case HexType.FIELD:
                    mainMaterial.mainTexture = wheatTexture;
                    break;
                case HexType.FOREST:
                    mainMaterial.mainTexture = woodTexture;
                    break;
                case HexType.HILLS:
                    mainMaterial.mainTexture = brickTexture;
                    break;
            }
        }
    }
	// Use this for initialization
	void Start () {
        this.transform.localRotation = new Quaternion();
        this.transform.Rotate(new Vector3(180, 90));
        cardSize = this.GetComponent<Renderer>().bounds.size;
    }
	
	// Update is called once per frame
	void Update () {
    }

    void OnMouseEnter() {
        this.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
        this.transform.localPosition += new Vector3(0, cardSize.y / 2);
    }

    void OnMouseExit() {
        this.transform.localScale = new Vector3(1, 1, 1);
        this.transform.localPosition -= new Vector3(0, cardSize.y / 2);
    }
}
