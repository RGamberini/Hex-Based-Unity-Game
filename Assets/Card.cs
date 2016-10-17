using UnityEngine;
using System.Collections;

public class Card : MonoBehaviour {
    public int scaleFactor = 2;
    public Hand hand;
    public int index;
    private Vector3 cardSize;
	// Use this for initialization
	void Start () {
        this.transform.parent = hand.gameObject.transform;
        this.transform.localRotation = new Quaternion();
        this.transform.Rotate(new Vector3(180, 90));

        cardSize = this.GetComponent<Renderer>().bounds.size;

        Vector3 newPosition = getPosition();
        this.transform.localPosition = newPosition;
    }
	
	// Update is called once per frame
	void Update () {
    }

    private Vector3 getPosition() {
        return hand.getPosition(index) + new Vector3(0, cardSize.y / 2);
    }

    void OnMouseEnter() {
        this.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
        this.transform.localPosition += new Vector3(0, cardSize.y / 2);
    }

    void OnMouseExit() {
        this.transform.localScale = new Vector3(1, 1, 1);
        this.transform.localPosition = getPosition();
    }
}
