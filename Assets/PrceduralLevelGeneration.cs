using UnityEngine;
using System.Collections;

public class PrceduralLevelGeneration : MonoBehaviour {

    public GameObject[] platforms;
    public GameObject camera;

    private float steps;
    private const float STEP_SIZE = 15;

    // Use this for initialization
    void Start() {
        steps = -20;
        Random.seed = (int)(Time.time);
    }

    // Update is called once per frame
    void Update()
    {
        if (camera.transform.position.y >= steps * STEP_SIZE)
        {
            steps++;
            Instantiate(platforms[Random.Range(0, platforms.Length)], new Vector3(0, (steps + 3) * STEP_SIZE), new Quaternion(0,0,0,0));
        }
    }

}
