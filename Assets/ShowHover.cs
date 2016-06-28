using UnityEngine;
using System.Collections;

public class ShowHover : MonoBehaviour {
    public GameObject roadPrefab;
    private GameObject roadInstance;

	// Use this for initialization
	void Start () {
        roadInstance = Instantiate(roadPrefab);
        roadInstance.transform.parent = this.transform;
        roadInstance.transform.localScale = new Vector3(1, 1, 1);
    }

    IEnumerator updateRoadInstance(Vector3 position, float angle) {
        yield return new WaitForSeconds(2);
        roadInstance.transform.localPosition = position;
        Debug.Log(position);
        roadInstance.transform.eulerAngles = new Vector3(0, angle);
    }

    // Update is called once per frame
    void Update () {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, 100.0f)) {
            if(hit.collider.name.Contains("Hexagon")) {
                Hex hex = hit.collider.gameObject.GetComponent<Hex>();
                Vector3 localHit = hit.point;

                localHit = hex.transform.InverseTransformPoint(localHit);

                float angleFromCenter = Mathf.Atan2(localHit.z, localHit.x) * 180 / Mathf.PI;
                Hex.Direction direction = hex.angleToDirection((int) Mathf.Round(angleFromCenter));

                Vector3 transformPosition = hex.transform.TransformPoint(
                    hex.directionToLocalPosition(direction, roadInstance));

                float angleToRotate = hex.directionToRotateAngle(direction);

                StartCoroutine(updateRoadInstance(transformPosition, angleToRotate));
            }
        }
    }
}
