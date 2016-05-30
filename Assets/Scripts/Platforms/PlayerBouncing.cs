using UnityEngine;
using System.Collections;

public class PlayerBouncing : MonoBehaviour {

    private Animator animator;

	// Use this for initialization
	void Start () {
        animator = this.GetComponent<Animator>();
	}

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            animator.Play("bouncing_platform");
            SoundManager.PlaySound("BoioioioioioiJumpingPlatform");
        }
    }
}
