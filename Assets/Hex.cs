using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (Renderer))]
[RequireComponent (typeof (Collider))]
public class Hex : MonoBehaviour {
    public enum Direction {
        EAST, NORTHEAST, NORTHWEST, WEST, SOUTHWEST, SOUTHEAST
    }
    public Board board;
    public int xCoord, yCoord;
    public Dictionary<Direction, GameObject> roads;
    private static Color liveColor = new Color(76 / 255f, 175 / 255f, 80 / 255f);
    private new Collider collider;
    private Vector3 size;

    // Use this for initialization
    void Start () {
        TextMesh coordinates = GetComponentInChildren<TextMesh>();
        coordinates.text = xCoord + " " + yCoord;
        collider = GetComponent<Collider>();

        size = collider.bounds.size;
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //void OnMouseDown() {
    //    Renderer renderer = GetComponent<Renderer>();
    //    renderer.material.color = liveColor;
    //}

    //void OnMouseEnter() {
    //    roadInstance = Instantiate(roadPrefab);
    //    roadInstance.transform.parent = this.transform;
    //    roadInstance.transform.localScale = new Vector3(1, 1, 1);
    //}

    void OnMouseOver() {
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //RaycastHit hit;
        //if(collider.Raycast(ray, out hit, 100.0F)) {
        //    Vector3 localHit = hit.point;

        //    localHit = this.transform.InverseTransformPoint(localHit);

        //    float angleFromCenter = Mathf.Atan2(localHit.z, localHit.x) * 180 / Mathf.PI;
        //    Direction direction = angleToDirection((int) Mathf.Round(angleFromCenter));

        //    Vector3 transformPosition = this.transform.TransformPoint(
        //        directionToLocalPosition(direction));
        //    roadInstance.transform.position = transformPosition;

        //    float angleToRotate = directionToRotateAngle(direction);
        //    roadInstance.transform.eulerAngles = new Vector3(0, angleToRotate);
        //}
    }

    private static Direction[] directionArray = new Direction[] { Direction.NORTHEAST, Direction.NORTHWEST, Direction.WEST, Direction.SOUTHWEST, Direction.SOUTHEAST, Direction.EAST};
    public Direction angleToDirection(int angle) {
        if(angle < 0) angle = 360 + angle;
        int start = 30;
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
                return 0;

            case Direction.NORTHEAST:
            case Direction.SOUTHWEST:
                return -60;

            case Direction.SOUTHEAST:
            case Direction.NORTHWEST:
                return 60;
            default:
                Debug.Log("ERROR INVALID DIRECTION");
                return 0;
        }
    }

    public Vector3 directionToLocalPosition(Direction direction, GameObject roadInstance) {
        Vector3 roadSize = roadInstance.GetComponent<Renderer>().bounds.size;
        switch(direction) {
            case Direction.EAST:
                return new Vector3((size.x / 2) - (roadInstance.GetComponent<Renderer>().bounds.size.x / 2), roadSize.y);
            case Direction.WEST:
                return new Vector3((size.x / 2) - (roadInstance.GetComponent<Renderer>().bounds.size.x / 2), roadSize.y) +
                    this.transform.InverseTransformPoint(board.gridCoordstoWorldCoords(xCoord - 1, yCoord));
            case Direction.NORTHEAST:
                return new Vector3((size.x / 4), roadSize.y, (size.z / 4) + (roadInstance.GetComponent<Renderer>().bounds.size.z / 4));
            case Direction.NORTHWEST:
                return new Vector3((size.x / 4), roadSize.y, (-size.z / 4) - (roadInstance.GetComponent<Renderer>().bounds.size.z / 4)) +
                    this.transform.InverseTransformPoint(board.gridCoordstoWorldCoords(xCoord, yCoord + 1));
            case Direction.SOUTHEAST:
                return new Vector3((size.x / 4), roadSize.y, (-size.z / 4) - (roadInstance.GetComponent<Renderer>().bounds.size.z / 4));
            case Direction.SOUTHWEST:
                return
                    new Vector3((size.x / 4), roadSize.y, (size.z / 4) + (roadInstance.GetComponent<Renderer>().bounds.size.z / 4)) +
                    this.transform.InverseTransformPoint(board.gridCoordstoWorldCoords(xCoord - 1, yCoord - 1));
            default:
                Debug.LogError("ERROR Unknown direction: " + direction);
                return new Vector3();
        }

    }
}
