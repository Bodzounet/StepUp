using UnityEngine;
using System.Collections;

public class RemoveOnPass : MonoBehaviour {

    public GameObject[] toRemove;
	// Use this for initialization
	void Start () {
	
	}
	
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            for (int i = 0; i < toRemove.Length; i++)
            {
                if (toRemove[i] != null)
                    Object.Destroy(toRemove[i]);
            }
        }
    }
}
