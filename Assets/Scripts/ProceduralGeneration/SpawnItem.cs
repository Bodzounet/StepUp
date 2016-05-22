using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class SpawnItem : MonoBehaviour {

    public GameObject[] items;

	// Use this for initialization
	void Start () {

        if (items.Length != 0)
            Instantiate(items[UnityEngine.Random.Range(0, items.Length)], new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y + 0.75f, 0), new Quaternion(0,0,0,0));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
