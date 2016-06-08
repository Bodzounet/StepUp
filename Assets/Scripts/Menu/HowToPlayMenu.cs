using UnityEngine;
using System.Collections;

public class HowToPlayMenu : MonoBehaviour {

    public GameObject InputHowto;
    public GameObject ItemsHowto;

    // Use this for initialization
    void Start () {
        InputHowto.SetActive(true);
        ItemsHowto.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetButtonDown("B1"))
        {
            if (InputHowto.activeInHierarchy)
            {
                InputHowto.SetActive(false);
                ItemsHowto.SetActive(true);
            }
            else
            {
                InputHowto.SetActive(true);
                ItemsHowto.SetActive(false);
            }
        }
	}
}
