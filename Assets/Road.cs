using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Road : NetworkBehaviour {
    [SyncVar]
    public bool preview = true;
    [SyncVar]
    public int xCoord, yCoord;
    [SyncVar]
    public Direction direction;

    private Board board;
	// Use this for initialization
	void Start () {
        Renderer renderer = this.GetComponent<Renderer>();
        Color color = renderer.material.color;
        if(preview) color.a = .76f;
        else {
            color.a = 1;
            board = GameObject.Find("Board").GetComponent<Board>();
            board.getHex(xCoord, yCoord).buildingManager.roads.Add(direction, this);

            Hex adjacentHex = board.getHex(xCoord + (int) direction.directionVector().x, yCoord + (int) direction.directionVector().y);
            if(adjacentHex != null) adjacentHex.buildingManager.roads.Add(direction.oppositeDirection(), this);
        }
        renderer.material.color = color;
    }

    // Update is called once per frame
    void Update () {
	
	}
}
