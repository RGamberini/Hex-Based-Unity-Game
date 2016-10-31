using UnityEngine;
using System.Collections;
using System;

public class Road : Buildable {
    public GameObject roadPrefab;
    private Vector3 roadSize;
    // Use this for initialization
    void Start () {
        base.Start();
        this.startRotation = 30;
        roadSize = roadPrefab.GetComponent<MeshFilter>().sharedMesh.bounds.size;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    protected override void addToBoard() {
        board.getHex(xCoord, yCoord).buildingManager.roads.Add(direction, this);

        Hex adjacentHex = board.getHex(xCoord + (int) direction.directionVector().x, yCoord + (int) direction.directionVector().y);
        if(adjacentHex != null) adjacentHex.buildingManager.roads.Add(direction.oppositeDirection(), this);
    }

    public override Vector3 directionToLocalPosition(Direction direction) {
        float xRadius = hexSize.x/2f, zRadius = hexSize.z * (3f/8f);
        switch(direction) {
            //Work out the vector for half the directions and for the opposite direction just grab it from the adjacent hex
            case Direction.EAST:
                return new Vector3(xRadius, 1);
            case Direction.WEST:
                return new Vector3(-xRadius, 1);

            case Direction.NORTHEAST:
                return new Vector3(xRadius / 2, 1, zRadius);
            case Direction.NORTHWEST:
                return new Vector3(-xRadius / 2, 1, zRadius);
            
            case Direction.SOUTHEAST:
                return new Vector3(xRadius / 2, 1, -zRadius);

            case Direction.SOUTHWEST:
                return new Vector3(-xRadius / 2, 1, -zRadius);
            default:
                Debug.LogError("ERROR Unknown direction: " + direction);
                return new Vector3();
        }
    }

    public override float directionToRotateAngle(Direction direction) {
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
}
