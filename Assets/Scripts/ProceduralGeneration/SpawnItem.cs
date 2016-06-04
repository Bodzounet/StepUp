using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class SpawnItem : MonoBehaviour {

    public GameObject item;
    public bool isSpawnPlatform = false;

	// Use this for initialization
	void Start () {

        if (item != null)
            if (UnityEngine.Random.Range(0, 100) < 25 || isSpawnPlatform)
                Instantiate(item, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y + 0.75f, 0), new Quaternion(0,0,0,0));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
