using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

[RequireComponent (typeof (Renderer))]
[RequireComponent (typeof (Collider))]
[RequireComponent (typeof (BuildingManager))]
public class Hex : NetworkBehaviour {
    [HideInInspector]
    public BuildingManager buildingManager;
    [SyncVar]
    public int xCoord, yCoord;
    [SyncVar]
    public NetworkInstanceId parentNetId;
    [SyncVar]
    public int diceRoll;
    [SyncVar]
    public HexType hexType;

    // Use this for initialization
    void Start () {
        Token token = GetComponentInChildren<Token>();
        token.number = diceRoll;
        buildingManager = GetComponent<BuildingManager>();

        GetComponent<Transform>().localScale = new Vector3(1.05f, 1.05f, 1.05f);
        GetComponent<Renderer>().material.color = hexType.color();

        if (this.hexType == HexType.DESERT) {
            Destroy(token.gameObject);
        }
	}

    public override void OnStartClient() {
        Board parentObject = ClientScene.FindLocalObject(parentNetId).GetComponent<Board>();
        transform.SetParent(parentObject.transform);

        if(parentObject.board == null) parentObject.populateBoardArray();
        parentObject.registerHex(xCoord, yCoord, this);
        this.gameObject.name = "X: " + xCoord + " Y: " + yCoord;
    }

    // Update is called once per frame
    void Update () {
	
	}
}
