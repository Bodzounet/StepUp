using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;

public class UI_StunBar : MonoBehaviour 
{
    public RectTransform Bottom;
    public RectTransform Up;

    public RectTransform bar;

    private Image img;

    Controller tmp;

    void Start()
    {
        tmp = GameObject.FindObjectsOfType<Controller>().Single(x => x.playerNumber == this.GetComponentInParent<UI_PlayerInfo>().playerIdToManage);
        tmp.OnStunPercentageChange += OnStunBarValueChange;
        img = bar.GetComponent<Image>();

        ResetBar();
    }

    public void OnStunBarValueChange(float newValue)
    {
        bar.transform.localPosition = Vector3.Lerp(Bottom.localPosition, Up.localPosition, newValue);
        img.color = Color.Lerp(Color.green, Color.red, newValue);
    }

    public void ResetBar()
    {
        bar.transform.localPosition = Bottom.localPosition;
        img.color = Color.green;
    }

    //public void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.V))
    //        tmp.Stun(1, 0.05f);
    //}
}
