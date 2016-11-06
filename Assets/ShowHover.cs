using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ShowHover: NetworkBehaviour {
    public GameObject[] buildables;
    public GameObject buildablePrefab;
    public Player player;

    public IEnumerator call;

    [HideInInspector]
    public GameObject buildableInstance;
    private GameObject buildableContainer;
    private Hex currentHex;
    private Buildable currentBuildable;
    public bool active = false;
    public Direction currentDirection;

	// Use this for initialization
	void Start () {
        buildableContainer = GameObject.Find("Buildables");
	    player = this.GetComponent<Player>();
	}

    // Update is called once per frame
    void Update () {
        if(!isLocalPlayer) return;
        if(!active) return;
        // First shoot a ray out from where the mouse is
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        // If that ray hits something
        if(Physics.Raycast(ray, out hit, 100.0f)) {
            // And it's a hexagon
            if(hit.collider.GetComponent<Hex>() != null) {
                // RoadInstance is null after the mouse is moved off the board or is pointed in a spot that has a road already.
                if(buildableInstance == null) {
                    buildableInstance = Instantiate(buildablePrefab);
                    buildableInstance.transform.parent = this.transform;
                    buildableInstance.transform.localScale = new Vector3(1, 1, 1);

                    currentBuildable = buildableInstance.GetComponent<Buildable>();
                }
                
                // Grab the hex we hit
                currentHex = hit.collider.gameObject.GetComponent<Hex>();
                currentBuildable.xCoord = currentHex.xCoord;
                currentBuildable.yCoord = currentHex.yCoord;
                // And where we hit it
                Vector3 localHit = hit.point;
                // In world space
                localHit = currentHex.transform.InverseTransformPoint(localHit);

                // Yay triginometry
                // Work the angle out from where we hit in relation to the origin
                float angleFromCenter = Mathf.Atan2(localHit.z, localHit.x) * 180 / Mathf.PI;
                // Now translate that angle into a direction
                currentDirection = currentBuildable.angleToDirection((int) Mathf.Round(angleFromCenter));
                
                // If there is already a road in that spot destroy the instance
                if(currentBuildable.canBuild(currentHex, currentDirection)) {
                    Destroy(buildableInstance);
                    buildableInstance = null;
                } else {
                    // Other wise translate that direction into a Position + Angle and apply them to the instance
                    Vector3 transformPosition = currentBuildable.directionToLocalPosition(currentDirection);
                    float angleToRotate = currentBuildable.directionToRotateAngle(currentDirection);
                    transformPosition += currentHex.transform.position;

                    buildableInstance.transform.position = transformPosition;
                    buildableInstance.transform.eulerAngles = new Vector3(0, angleToRotate);
                }
            }
        } else {
            // If it's not a hexagon destroy the instance
            Destroy(buildableInstance);
            buildableInstance = null;
            currentHex = null;
        }

        if (!Input.GetMouseButtonDown(0) || buildableInstance == null) return;
        CmdSpawnRoad(
            currentHex.xCoord,
            currentHex.yCoord,
            player.playerID,
            currentDirection,
            buildableInstance.transform.localPosition,
            buildableInstance.transform.localRotation);

        Destroy(buildableInstance);
        buildableInstance = null;
        Debug.Log("Calling");
        CmdCallback();
    }

    [Command]
    void CmdCallback() {
        call.MoveNext();
    }

    [Command]
    void CmdSpawnRoad(int xCoord, int yCoord, int playerIndex, Direction direction, Vector3 roadPosition, Quaternion roadRotation) {
        GameObject newRoad = Instantiate(
            buildablePrefab, 
            roadPosition, 
            roadRotation) as GameObject;
        newRoad.transform.localScale = new Vector3(1, 1, 1);
        newRoad.transform.parent = buildableContainer.transform;

        Buildable roadAsRoad = newRoad.GetComponent<Buildable>();
        roadAsRoad.preview = false;
        roadAsRoad.xCoord = xCoord;
        roadAsRoad.yCoord = yCoord;
        roadAsRoad.playerIndex = playerIndex;
        roadAsRoad.direction = direction;

        NetworkServer.Spawn(newRoad);
    }
}
