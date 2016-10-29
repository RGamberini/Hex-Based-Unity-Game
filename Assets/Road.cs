﻿using UnityEngine;
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

    protected override void addToBoard(Board board) {
        board.getHex(xCoord, yCoord).buildingManager.roads.Add(direction, this);

        Hex adjacentHex = board.getHex(xCoord + (int) direction.directionVector().x, yCoord + (int) direction.directionVector().y);
        if(adjacentHex != null) adjacentHex.buildingManager.roads.Add(direction.oppositeDirection(), this);
    }

    protected override Vector3 directionToLocalPosition(Direction direction) {
        switch(direction) {
            //Work out the vector for half the directions and for the opposite direction just grab it from the adjacent hex
            case Direction.EAST:
                return new Vector3((hexSize.x / 2) - (roadSize.y / 2), roadSize.y);

            case Direction.NORTHEAST:
                return new Vector3((hexSize.x / 4), roadSize.y, (hexSize.z / 4) + (roadSize.z / 4));

            case Direction.SOUTHEAST:
                return new Vector3((hexSize.x / 4), roadSize.y, (-hexSize.z / 4) - (roadSize.z / 4));
            case Direction.WEST:
            case Direction.NORTHWEST:
            case Direction.SOUTHWEST:
                return new Vector3();
//                Vector2 directionVector = direction.directionVector();
//                return directionToLocalPosition(direction.oppositeDirection(), roadInstance) +
//                    this.transform.InverseTransformPoint(
//                        board.gridCoordstoWorldCoords(
//                            hex.xCoord + (int) directionVector.x, hex.yCoord + (int) directionVector.y));

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
