﻿using UnityEngine;
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
    private static Color liveColor = new Color(76 / 255f, 175 / 255f, 80 / 255f);

    // Use this for initialization
    void Start () {
        TextMesh coordinates = GetComponentInChildren<TextMesh>();
        coordinates.text = xCoord + " " + yCoord;
        buildingManager = GetComponent<BuildingManager>();

        GetComponent<Transform>().localScale = new Vector3(1.05f, 1.05f, 1.05f);
	}

    public override void OnStartClient() {
        Board parentObject = ClientScene.FindLocalObject(parentNetId).GetComponent<Board>();
        transform.SetParent(parentObject.transform);

        if(parentObject.board == null) parentObject.populateBoardArray();
        parentObject.registerHex(xCoord, yCoord, this);
    }

    // Update is called once per frame
    void Update () {
	
	}
}
