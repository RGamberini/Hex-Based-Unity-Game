using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ShowHover : NetworkBehaviour {
    public GameObject[] buildables;
    public GameObject roadPrefab;
    [HideInInspector]
    public GameObject roadInstance;
    private Board board;
    private GameObject roadContainer;
    private Hex currentHex;
    public Direction currentDirection;

	// Use this for initialization
	void Start () {
        board = GameObject.Find("Board").GetComponent<Board>();
        roadContainer = GameObject.Find("Roads");
    }

    // Update is called once per frame
    void Update () {
        if(!isLocalPlayer) return;
        // First shoot a ray out from where the mouse is
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        // If that ray hits something
        if(Physics.Raycast(ray, out hit, 100.0f)) {
            // And it's a hexagon
            if(hit.collider.name.Contains("Hexagon")) {
                // RoadInstance is null after the mouse is moved off the board or is pointed in a spot that has a road already.
                if(roadInstance == null) {
                    roadInstance = Instantiate(roadPrefab);
                    roadInstance.transform.parent = this.transform;
                    roadInstance.transform.localScale = new Vector3(1, 1, 1);
                }
                
                // Grab the hex we hit
                currentHex = hit.collider.gameObject.GetComponent<Hex>();
                // And where we hit it
                Vector3 localHit = hit.point;
                // In world space
                localHit = currentHex.transform.InverseTransformPoint(localHit);

                // Yay triginometry
                // Work the angle out from where we hit in relation to the origin
                float angleFromCenter = Mathf.Atan2(localHit.z, localHit.x) * 180 / Mathf.PI;
                // Now translate that angle into a direction
                currentDirection = currentHex.buildingManager.angleToDirection((int) Mathf.Round(angleFromCenter));
                
                // If there is already a road in that spot destroy the instance
                if(currentHex.buildingManager.roads.ContainsKey(currentDirection)) {
                    Destroy(roadInstance);
                    roadInstance = null;
                } else {
                    // Other wise translate that direction into a Position + Angle and apply them to the instance
                    Vector3 transformPosition = currentHex.buildingManager.directionToGlobalPosition(currentDirection);
                    float angleToRotate = currentHex.buildingManager.directionToRotateAngle(currentDirection);

                    roadInstance.transform.localPosition = transformPosition;
                    roadInstance.transform.eulerAngles = new Vector3(0, angleToRotate);
                }
            }
        } else {
            // If it's not a hexagon destroy the instance
            Destroy(roadInstance);
            roadInstance = null;
            currentHex = null;
        }

        if (Input.GetMouseButtonDown(0) && roadInstance != null) {
            Debug.Log("Spawning road");
            CmdSpawnRoad(
                currentHex.xCoord, 
                currentHex.yCoord,
                currentDirection,
                roadInstance.transform.localPosition,
                roadInstance.transform.localRotation);

            Destroy(roadInstance);
            roadInstance = null;
        }
    }

    [Command]
    void CmdSpawnRoad(int xCoord, int yCoord, Direction direction, Vector3 roadPosition, Quaternion roadRotation) {
        GameObject newRoad = Instantiate(
            roadPrefab, 
            roadPosition, 
            roadRotation) as GameObject;
        newRoad.transform.localScale = new Vector3(1, 1, 1);
        newRoad.transform.parent = roadContainer.transform;

        Buildable roadAsRoad = newRoad.GetComponent<Buildable>();
        roadAsRoad.preview = false;
        roadAsRoad.xCoord = xCoord;
        roadAsRoad.yCoord = yCoord;
        roadAsRoad.direction = direction;

        NetworkServer.Spawn(newRoad);
    }
}
