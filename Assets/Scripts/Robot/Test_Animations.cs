using UnityEngine;
using System.Collections;

public class Test_Animations : MonoBehaviour
{
    Animator _anim;

	void Start ()
    {
        _anim = this.GetComponent<Animator>();
	}
	
	void Update ()
    {
	    if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _anim.Play("Idle");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _anim.Play("Jump");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _anim.Play("Air Jump");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            _anim.Play("Move");
        }
    }
}
