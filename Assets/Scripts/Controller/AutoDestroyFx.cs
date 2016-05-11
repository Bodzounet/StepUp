using UnityEngine;
using System.Collections;

public class AutoDestroyFx : MonoBehaviour {

	public void AutoDestroy()
    {
        Destroy(this.gameObject);
    }
}
