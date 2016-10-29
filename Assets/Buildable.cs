using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public abstract class Buildable : NetworkBehaviour {
    public string identifer;
    [SyncVar]
    public bool preview = true;
    [SyncVar]
    public int xCoord, yCoord;
    [SyncVar]
    public Direction direction;

    public GameObject hexPrefab;
    protected Vector3 hexSize;

	// Use this for initialization
	protected void Start ()
	{
	    hexSize = hexPrefab.GetComponent<MeshFilter>().sharedMesh.bounds.size;

        Color color = this.GetComponent<Renderer>().material.color;
        if(preview) color.a = .76f;
        else {
            color.a = 1;
            addToBoard(GameObject.Find("Board").GetComponent<Board>());
        }
        this.GetComponent<Renderer>().material.color = color;
    }

    // Update is called once per frame
    void Update () {
	
	}

    protected abstract void addToBoard(Board board);
    public abstract float directionToRotateAngle(Direction direction);
    protected abstract Vector3 directionToLocalPosition(Direction direction);

    protected int startRotation;
    public static Direction[] directionArray = new Direction[] { Direction.NORTHEAST, Direction.NORTHWEST, Direction.WEST, Direction.SOUTHWEST, Direction.SOUTHEAST, Direction.EAST };
    public Direction angleToDirection(int angle) {
        if(angle < 0) angle = 360 + angle;
        foreach (Direction dir in directionArray) {
            if(angle > startRotation && angle <= startRotation + 60) return dir;
            startRotation += 60;
        }
        return Direction.EAST;
    }

    public Vector3 directionToGlobalPosition(Direction dir) {       
        return this.transform.TransformPoint(
            directionToLocalPosition(dir));
    }
}
