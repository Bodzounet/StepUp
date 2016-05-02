using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class ProceduralLevelGeneration : MonoBehaviour
{

    public GameObject[] platforms;
    public int maxDifficulty;
    public GameObject camera;
    public GameObject firstPlatform;


    private float difficulyLevel;
    private float steps;
    private const float STEP_SIZE = 15;
    private const float DIFFICULTY_SETP = 1.0f;
    private GameObject instPlatform;

    private List<GameObject>[] platformsDifficulty; //[ [,] platformsDificulty;
    // Use this for initialization
    void Start()
    {
        difficulyLevel = 0;
        steps = -5;
        platformsDifficulty = new List<GameObject>[maxDifficulty + 1];
        for (int i = 0; i < maxDifficulty + 1; i++)
        {
            platformsDifficulty[i] = new List<GameObject>();
        }
      
        for (int i = 0; i < platforms.Length; i++)
        {
            if (platforms[i] != null)
            {
                int difTmp = platforms[i].GetComponent<Difficulty>().difficulty;
               
                platformsDifficulty[difTmp].Add(platforms[i]);
                print(i);
            }
        }
     //   UnityEngine.Random.seed = unchecked(DateTime.Now.Ticks.GetHashCode());
    }

    // Update is called once per frame
    void Update()
    {
        
        if (camera != null)
        {
            if (steps < 0)
            {
                steps += firstPlatform.GetComponent<BoxCollider2D>().size.y / 2;
            }
            else if (camera.transform.position.y >= steps - 15)
            {
                if (difficulyLevel < maxDifficulty)
                    difficulyLevel += DIFFICULTY_SETP;
                print("Difficulty level:" + difficulyLevel);

                List<GameObject> lstPlatform = platformsDifficulty[(int)Math.Truncate(difficulyLevel)];
                if (instPlatform != null)
                    steps += instPlatform.GetComponent<BoxCollider2D>().size.y / 2 + 2;

                instPlatform = lstPlatform[UnityEngine.Random.Range(0, lstPlatform.Count)];

                steps += instPlatform.GetComponent<BoxCollider2D>().size.y / 2;
                float offset = instPlatform.GetComponent<BoxCollider2D>().offset.y;

                Instantiate(instPlatform, new Vector3(0, (steps) - offset), new Quaternion(0, 0, 0, 0));
            }
        }
        else
        {
            print("YA WANNA HAVE A BAD TOM!?!");
        }
    }

}
