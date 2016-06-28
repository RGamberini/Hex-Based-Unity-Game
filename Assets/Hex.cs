using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (Renderer))]
[RequireComponent (typeof (Collider))]
[RequireComponent (typeof (BuildingManager))]
public class Hex : MonoBehaviour {
    public enum Direction {
        EAST, NORTHEAST, NORTHWEST, WEST, SOUTHWEST, SOUTHEAST
    }
    [HideInInspector]
    public BuildingManager buildingManager;
    public int xCoord, yCoord;
    private static Color liveColor = new Color(76 / 255f, 175 / 255f, 80 / 255f);

    // Use this for initialization
    void Start () {
        TextMesh coordinates = GetComponentInChildren<TextMesh>();
        coordinates.text = xCoord + " " + yCoord;
        buildingManager = GetComponent<BuildingManager>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
