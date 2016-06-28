using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Hex))]
[RequireComponent(typeof(Collider))]
public class BuildingManager : MonoBehaviour {
    public Dictionary<Hex.Direction, GameObject> roads;
    public Dictionary<Hex.Direction, Vector3> roadPositions;
    public GameObject roadPrefab;
    public Board board;
    private Vector3 size;
    private Hex hex;

    // Use this for initialization
    void Start () {
        size = this.GetComponent<Collider>().bounds.size;
        hex = this.GetComponent<Hex>();

        roadPositions = new Dictionary<Hex.Direction, Vector3>();
        roads = new Dictionary<Hex.Direction, GameObject>();

        foreach(Hex.Direction direction in directionArray) {
            roadPositions.Add(direction, directionToLocalPosition(direction, roadPrefab));
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    private static Hex.Direction[] directionArray = new Hex.Direction[] { Hex.Direction.NORTHEAST, Hex.Direction.NORTHWEST, Hex.Direction.WEST, Hex.Direction.SOUTHWEST, Hex.Direction.SOUTHEAST, Hex.Direction.EAST };
    public Hex.Direction angleToDirection(int angle) {
        if(angle < 0) angle = 360 + angle;
        int start = 30;
        for(int i = 0; i < directionArray.Length; i++) {
            if(angle > start && angle <= start + 60) return directionArray[i];
            start += 60;
        }
        return Hex.Direction.EAST;
    }

    public float directionToRotateAngle(Hex.Direction direction) {
        switch(direction) {
            case Hex.Direction.EAST:
            case Hex.Direction.WEST:
                return 0;

            case Hex.Direction.NORTHEAST:
            case Hex.Direction.SOUTHWEST:
                return -60;

            case Hex.Direction.SOUTHEAST:
            case Hex.Direction.NORTHWEST:
                return 60;
            default:
                Debug.Log("ERROR INVALID DIRECTION");
                return 0;
        }
    }

    private Vector3 directionToLocalPosition(Hex.Direction direction, GameObject roadInstance) {
        Vector3 roadSize = roadInstance.GetComponent<Renderer>().bounds.size;
        switch(direction) {
            case Hex.Direction.EAST:
                return new Vector3((size.x / 2) - (roadInstance.GetComponent<Renderer>().bounds.size.y / 2), roadSize.y);
            case Hex.Direction.WEST:
                return new Vector3((size.x / 2) - (roadInstance.GetComponent<Renderer>().bounds.size.y / 2), roadSize.y) +
                    this.transform.InverseTransformPoint(board.gridCoordstoWorldCoords(hex.xCoord - 1, hex.yCoord));
            case Hex.Direction.NORTHEAST:
                return new Vector3((size.x / 4), roadSize.y, (size.z / 4) + (roadInstance.GetComponent<Renderer>().bounds.size.z / 4));
            case Hex.Direction.NORTHWEST:
                return new Vector3((size.x / 4), roadSize.y, (-size.z / 4) - (roadInstance.GetComponent<Renderer>().bounds.size.z / 4)) +
                    this.transform.InverseTransformPoint(board.gridCoordstoWorldCoords(hex.xCoord, hex.yCoord + 1));
            case Hex.Direction.SOUTHEAST:
                return new Vector3((size.x / 4), roadSize.y, (-size.z / 4) - (roadInstance.GetComponent<Renderer>().bounds.size.z / 4));
            case Hex.Direction.SOUTHWEST:
                return
                    new Vector3((size.x / 4), roadSize.y, (size.z / 4) + (roadInstance.GetComponent<Renderer>().bounds.size.z / 4)) +
                    this.transform.InverseTransformPoint(board.gridCoordstoWorldCoords(hex.xCoord - 1, hex.yCoord - 1));
            default:
                Debug.LogError("ERROR Unknown direction: " + direction);
                return new Vector3();
        }
    }

    public Vector3 directionToGlobalPosition(Hex.Direction direction) {
        Vector3 result;
        roadPositions.TryGetValue(direction, out result);
        return this.transform.TransformPoint(result);
    }

    public void addRoad(Hex.Direction direction, GameObject roadInstance) {
        GameObject newRoad = Instantiate(roadInstance);
        newRoad.transform.parent = this.transform;
        newRoad.transform.localScale = new Vector3(1, 1, 1);

        Renderer renderer = newRoad.GetComponent<Renderer>();
        Color color = renderer.material.color;
        color.a = 1;
        renderer.material.color = color;

        roads.Add(direction, roadInstance);
        Destroy(roadInstance);
        roadInstance = null;
    }
}
