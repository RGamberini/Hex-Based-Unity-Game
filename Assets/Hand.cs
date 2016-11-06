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
        cardSize = cardPrefab.GetComponent<MeshFilter>().sharedMesh.bounds.size;
        camera = this.gameObject.GetComponent<Camera>();
	}

    public void createCard(HexType hexType) {
        Card card = Instantiate(cardPrefab).GetComponent<Card>();
        //if (cardSize == Vector3.zero) cardSize = card.GetComponent<Renderer>().bounds.size;
        card.index = cards.Count;
        card.transform.parent = this.transform;
        card.hexType = hexType;
        cards.Add(card);

        reposition();
    }

    public Vector3 getPosition(int index) {
        Vector3 middle = this.transform.InverseTransformPoint(
    camera.ViewportToWorldPoint(new Vector3(.5f, 0, 1)));
        middle += new Vector3(
            cardSize.z * (index - Mathf.Floor(cards.Count / 2f)),
            cardSize.y / 2, 
            -cardSize.y / 2);

        if (cards.Count % 2 == 0) middle += new Vector3(cardSize.z / 2, 0);
        return middle;
    }

    public void reposition() {
        int middleCard = Mathf.FloorToInt(cards.Count / 2f);
        for (int i = 0; i < cards.Count; i++) {
            Card repositionCard = cards[i];
            repositionCard.transform.localPosition = getPosition(i);
        }
    }

    // Update is called once per frame
	void Update () {
	
	}
}
