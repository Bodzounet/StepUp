using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class HowToRand : MonoBehaviour {
    public GameObject   input;
    public GameObject   items;
    private float       time;

    // Use this for initialization
    void Start ()
    {
	
	}

    void OnEnable()
    {
        if (!GameConfig.Instance.HowtoEnable)
        {
            time = 1000.0f;
            return;
        }

            time = .0f;

        if (Random.value >= 0.5f)
        {
            input.SetActive(true);
            items.SetActive(false);
        }
        else
        {
            input.SetActive(false);
            items.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update ()
    {
        if (Input.GetButtonDown("A1") && time > .0f)
            time = 1000.0f;
        else if (Input.GetButtonDown("B1"))
        {
            GameConfig.Instance.HowtoEnable = false;
            time = 1000.0f;
        }

        if (time > 2.0f)
            SceneManager.LoadScene("Level");
        time += Time.deltaTime;
    }
}
