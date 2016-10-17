using UnityEngine;
using System.Collections;

public class Die : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public int getNumber() {
        float upThreshold = 0.99f;     //You can modify this, slightly, if you want to allow slight rotations when the die has stopped

        float dotFwd = Vector3.Dot(transform.forward, Vector3.up);
        if(dotFwd >= upThreshold) return 5;
        if(dotFwd <= -(upThreshold)) return 6; 

        float dotRight = Vector3.Dot(transform.right, Vector3.up);
        if(dotRight >= upThreshold) return 2;
        if(dotRight <= -(upThreshold)) return 1;

        float dotUp = Vector3.Dot(transform.up, Vector3.up);
        if(dotUp >= upThreshold) return 4;
        if(dotUp <= -(upThreshold)) return 3;

        return 0;
    }
}
