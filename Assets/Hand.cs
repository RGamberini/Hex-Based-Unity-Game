using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(Camera))]
public class Hand : MonoBehaviour {
    public GameObject cardPrefab;
    public List<Card> cards = new List<Card>();
    private Vector3 cardSize;

    private new Camera camera;
	// Use this for initialization
	void Start () {
        createCard();
        createCard();
        cardSize = cardPrefab.GetComponent<Renderer>().bounds.size;
        camera = this.gameObject.GetComponent<Camera>();
	}

    private void createCard() {
        Card card = Instantiate(cardPrefab).GetComponent<Card>();
        //if (cardSize == Vector3.zero) cardSize = card.GetComponent<Renderer>().bounds.size;
        card.hand = this;
        card.index = cards.Count;
        cards.Add(card);
    }

    public Vector3 getPosition(int index) {
        Vector3 middle = this.gameObject.transform.InverseTransformPoint(
    camera.ViewportToWorldPoint(new Vector3(.5f, 0, 1)));
        return middle += new Vector3(
            (cardSize.z * index) - ((cardSize.z) / 2), 0);
    }
	// Update is called once per frame
	void Update () {
	
	}
}
