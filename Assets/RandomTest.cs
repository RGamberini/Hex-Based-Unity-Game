using UnityEngine;
using System.Collections;

public class RandomTest : MonoBehaviour {
    public Die diePrefab;
    int[] randomValues = new int[10];

    // Use this for initialization
    void Start () {
        testPhysics();
    }
    private static Vector3[] DieRotations = {
        new Vector3(0, 0, 270), new Vector3(0, 0, 90), new Vector3(180, 0),
        new Vector3(), new Vector3(270, 0), new Vector3(90, 0)};

    private int rollDie (Vector3 toPosition) {
        Die[] dice = new Die[2];
        Vector3 randomness = new Vector3(1.5f, 0, 1.5f);
        int c = 0;
        for(int i = 1; i <= 1; i += 2) {
            Die die = Instantiate(diePrefab);
            //die.transform.localPosition = new Vector3(0, 2) + toPosition + (randomness * i);
            die.transform.localPosition = new Vector3(0, 2) + toPosition;
            die.transform.Rotate(DieRotations[Random.Range(0, DieRotations.Length)]);
            dice[c] = die;
            c++;

            //Rigidbody body = die.GetComponent<Rigidbody>();
            //body.AddForce((toPosition - die.transform.localPosition).normalized * 500 * body.mass);
            //body.AddTorque(new Vector3(Random.Range(-360, 360), Random.Range(-360, 360), Random.Range(-360, 360)));
        }

        return dice[0].getNumber();
    }

    private void testPhysics() {
        int previous = -1;
        int streak = 0;

        for(int i = 0; i < 1000; i++) {
            int randomValue = rollDie(new Vector3());
            randomValues[randomValue]++;
            if(randomValue == previous) {
                streak++;
                Debug.Log(streak);
            } else {
                streak = 0;
            }
            previous = randomValue;
        }

        using(System.IO.StreamWriter file =
           new System.IO.StreamWriter(@"C:\Users\Rudy\Rudy\random.txt")) {
            file.Write(randomValues[0]);
            for(int i = 1; i < randomValues.Length; i++) {
                file.Write(", " + randomValues[i]);
            }
        }
    }

    private void testRNG() {
        int[] randomValues = new int[10];
        int previous = -1;
        int streak = 0;
        for(int i = 0; i < 10000; i++) {
            int randomValue = Random.Range(0, 10);
            randomValues[randomValue]++;
            if(randomValue == previous) {
                streak++;
                Debug.Log(streak);
            } else {
                streak = 0;
            }
            previous = randomValue;
        }
        using(System.IO.StreamWriter file =
           new System.IO.StreamWriter(@"C:\Users\Rudy\Rudy\random.txt")) {
            file.Write(randomValues[0]);
            for(int i = 1; i < randomValues.Length; i++) {
                file.Write(", " + randomValues[i]);
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
