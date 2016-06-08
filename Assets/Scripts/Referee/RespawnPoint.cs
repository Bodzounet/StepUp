using UnityEngine;
using System.Collections;

public class RespawnPoint : MonoBehaviour {

    public GameObject platform;
    private GameObject newPlatform;

    public void WakeUp()
    {
        StopAllCoroutines();
        if (newPlatform != null)
            Destroy(newPlatform);
        StartCoroutine(Co_RespawnPlatform());
    }

    private IEnumerator Co_RespawnPlatform()
    {
        newPlatform = GameObject.Instantiate(platform, new Vector2(transform.position.x, transform.position.y), Quaternion.identity) as GameObject;
        SpriteRenderer sr = newPlatform.GetComponent<SpriteRenderer>();

        Color initialColor = sr.color;
        Color endColor = new Color(initialColor.r, initialColor.g, initialColor.b, 0);

        for (int i = 0; i < 20; i++)
        {
            sr.color = Color.Lerp(initialColor, endColor, i * 0.05f);
            yield return new WaitForSeconds(0.05f);
        }
        Destroy(newPlatform);
    }
}
