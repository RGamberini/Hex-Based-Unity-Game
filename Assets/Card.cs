using UnityEngine;
using System.Collections;

public class Card : MonoBehaviour {
    public int scaleFactor = 2;
    public int index;
    private Vector3 cardSize;
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
