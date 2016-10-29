using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

[RequireComponent(typeof(Hex))]
[RequireComponent(typeof(Collider))]
public class BuildingManager : NetworkBehaviour {
    public Dictionary<Direction, Buildable> roads;
    public Dictionary<Direction, Vector3> roadPositions;
    private GameObject roadContainer;
    public GameObject roadPrefab;
    private Board board;
    private Vector3 size;
    private Vector3 roadSize;
    private Hex hex;

    // Use this for initialization
    void Start() {
        board = GameObject.Find("Board").GetComponent<Board>();
        roadContainer = GameObject.Find("Roads");

        size = this.GetComponent<Collider>().bounds.size;
        hex = this.GetComponent<Hex>();

        roadPositions = new Dictionary<Direction, Vector3>();
        roads = new Dictionary<Direction, Buildable>();
        Bounds roadBounds = roadPrefab.GetComponent<MeshFilter>().sharedMesh.bounds;
        roadBounds = new Bounds(roadBounds.center, roadBounds.size);
        roadSize = roadBounds.size;

        foreach(Direction direction in directionArray) {
            roadPositions.Add(direction, directionToLocalPosition(direction));
        }
    }

    // Update is called once per frame
    void Update() {

    }

    protected int start;
    public static Direction[] directionArray = new Direction[] { Direction.NORTHEAST, Direction.NORTHWEST, Direction.WEST, Direction.SOUTHWEST, Direction.SOUTHEAST, Direction.EAST };
    public Direction angleToDirection(int angle) {
        if(angle < 0) angle = 360 + angle;
        int start = 0;
        for(int i = 0; i < directionArray.Length; i++) {
            if(angle > start && angle <= start + 60) return directionArray[i];
            start += 60;
        }
        return Direction.EAST;
    }

    public float directionToRotateAngle(Direction direction) {
        switch(direction) {
            case Direction.EAST:
            case Direction.WEST:
                return 30;

            case Direction.NORTHEAST:
            case Direction.SOUTHWEST:
                return -30;

            case Direction.SOUTHEAST:
            case Direction.NORTHWEST:
                return 90;
            default:
                Debug.Log("ERROR INVALID DIRECTION");
                return 0;
        }
    }

    //private Vector3 directionToLocalPosition(Direction direction, GameObject roadInstance) {
    //    Vector3 roadSize = roadInstance.GetComponent<Renderer>().bounds.size;
    //    switch(direction) {
    //        //Work out the vector for half the directions and for the opposite direction just grab it from the adjacent hex
    //        case Direction.EAST:
    //            return new Vector3((size.x / 2) - (roadSize.y / 2), roadSize.y);

    //        case Direction.NORTHEAST:
    //            return new Vector3((size.x / 4), roadSize.y, (size.z / 4) + (roadSize.z / 4));

    //        case Direction.SOUTHEAST:
    //            return new Vector3((size.x / 4), roadSize.y, (-size.z / 4) - (roadSize.z / 4));
    //        case Direction.WEST:
    //        case Direction.NORTHWEST:
    //        case Direction.SOUTHWEST:
    //            Vector2 directionVector = direction.directionVector();
    //            return directionToLocalPosition(direction.oppositeDirection(), roadInstance) +
    //                this.transform.InverseTransformPoint(
    //                    board.gridCoordstoWorldCoords(
    //                        hex.xCoord + (int) directionVector.x, hex.yCoord + (int) directionVector.y));

    //        default:
    //            Debug.LogError("ERROR Unknown direction: " + direction);
    //            return new Vector3();
    //    }
    //}

    private Vector3 directionToLocalPosition(Direction direction) {
        switch(direction) {
            //Work out the vector for half the directions and for the opposite direction just grab it from the adjacent hex
            case Direction.EAST:
                //return new Vector3((size.x / 2) - (roadSize.y / 2), roadSize.y);
                return new Vector3((size.x / 2) - (roadSize.x / 2), roadSize.y, (-size.z / 4) + (roadSize.z / 4));

            case Direction.NORTHEAST:
                return new Vector3((size.x / 2) - (roadSize.x / 2), roadSize.y, (size.z / 4) - (roadSize.z / 4));

            case Direction.SOUTHEAST:
                return new Vector3(0, roadSize.y, -1 * ((size.z / 2) - (roadSize.z / 2)));
            case Direction.NORTHWEST:
                return new Vector3(0, roadSize.y, (size.z / 2) - (roadSize.z / 2));
            case Direction.WEST:
                return new Vector3((-size.x / 2) + (roadSize.x / 2), roadSize.y, (size.z / 4) - (roadSize.z / 4));
            case Direction.SOUTHWEST:
                return new Vector3((-size.x / 2) + (roadSize.x / 2), roadSize.y, (-size.z / 4) + (roadSize.z / 4));

            default:
                Debug.LogError("ERROR Unknown direction: " + direction);
                return new Vector3();
        }
    }

    public Vector3 directionToGlobalPosition(Direction direction) {
        Vector3 result;
        roadPositions.TryGetValue(direction, out result);
        return this.transform.TransformPoint(result);
    }
}
