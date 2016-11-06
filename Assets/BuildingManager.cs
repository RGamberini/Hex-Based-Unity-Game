using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

[RequireComponent(typeof(Hex))]
[RequireComponent(typeof(Collider))]
public class BuildingManager : NetworkBehaviour {
    public Dictionary<Direction, Buildable> edges;
    public Dictionary<Direction, Buildable> points;
    public Dictionary<Direction, Vector3> roadPositions;

    // Use this for initialization
    void Start() {
        roadPositions = new Dictionary<Direction, Vector3>();
        edges = new Dictionary<Direction, Buildable>();
        points = new Dictionary<Direction, Buildable>();
    }

    // Update is called once per frame
    void Update() {

    }
}
