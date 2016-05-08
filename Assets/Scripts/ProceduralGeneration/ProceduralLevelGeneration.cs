using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class ProceduralLevelGeneration : MonoBehaviour
{

    public GameObject[] platforms;
    public GameObject[] specialPlatforms;
    public int maxDifficulty;
    public GameObject arbiter;
    public GameObject bottomTrigger;
    public GameObject firstPlatform;


    private float difficulyLevel;
    private float steps;
    private const float STEP_SIZE = 15;
    private const float DIFFICULTY_SETP = 0.1f;
    private GameObject instPlatform;
    private List<GameObject> platformInstantiated;


    private List<GameObject>[] platformsDifficulty; //[ [,] platformsDificulty;
    // Use this for initialization
    void Start()
    {
        difficulyLevel = 0;
        steps = -5;
        platformInstantiated = new List<GameObject>();

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
            }
        }
     //   UnityEngine.Random.seed = unchecked(DateTime.Now.Ticks.GetHashCode());
    }

    // Update is called once per frame
    void Update()
    {
        
        if (arbiter != null)
        {
            if (steps < 0)
            {
                steps += firstPlatform.GetComponent<BoxCollider2D>().size.y / 2 + 6;
                platformInstantiated.Add(firstPlatform);
            }
            else if (arbiter.transform.position.y >= steps - 15)
            {
                if (difficulyLevel < maxDifficulty)
                    difficulyLevel += DIFFICULTY_SETP;

                List<GameObject> lstPlatform = platformsDifficulty[(int)Math.Truncate(difficulyLevel)];
                if (instPlatform != null)
                    steps += instPlatform.GetComponent<BoxCollider2D>().size.y / 2 + 3;

                instPlatform = lstPlatform[UnityEngine.Random.Range(0, lstPlatform.Count)];

                steps += instPlatform.GetComponent<BoxCollider2D>().size.y / 2;
                float offset = instPlatform.GetComponent<BoxCollider2D>().offset.y;

                platformInstantiated.Add((GameObject) Instantiate(instPlatform, new Vector3(0, (steps) - offset), new Quaternion(0, 0, 0, 0)));

                int platformType = UnityEngine.Random.Range(0, specialPlatforms.Length * 5);
                if (platformType < specialPlatforms.Length)
                {
                    int side = UnityEngine.Random.Range(0, 3);
                    platformInstantiated.Add((GameObject)Instantiate(specialPlatforms[platformType], new Vector3(side == 0 ? -10 : 10, (steps) - offset + side), new Quaternion(0, 0, 0, 0)));
                }

                // Destruction of platform when under bottomTrigger of the arbiter
                GameObject platformTmp = platformInstantiated[0];
                if (platformTmp.transform.position.y < bottomTrigger.transform.position.y)
                {
                    platformInstantiated.Remove(platformTmp);
                    Destroy(platformTmp);
                }
            }
        }
        else
        {
            print("No arbiter instance set in level generation");
        }
    }

}
