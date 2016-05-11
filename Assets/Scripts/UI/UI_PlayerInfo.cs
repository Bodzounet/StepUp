using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class UI_PlayerInfo : MonoBehaviour 
{
    public Image playerSprite;
    public Image itemSprite;
    public GameObject[] lifes;

    public int playerIdToManage;

    private GameObject _managedPlayer;

    public void ResetPlayerInfo()
    {
        _managedPlayer = GameObject.FindObjectsOfType<Controller>().Where(x => x.playerNumber == playerIdToManage).Single().gameObject;

        _managedPlayer.GetComponent<Items.Inventory>().onItemIsSet += OnItemChange;
        _managedPlayer.GetComponent<DeathManager>().OnLifeNumberChange += OnLifeChange;

        SpriteRenderer sr = _managedPlayer.GetComponent<SpriteRenderer>();
        playerSprite.sprite = sr.sprite;
        playerSprite.color = sr.color;

        OnItemChange(_managedPlayer.GetComponent<Items.Inventory>().Item);
    }

    void OnLifeChange(int remainingLifes)
    {
        for (int i = 0; i < lifes.Length; i++)
        {
            lifes[lifes.Length - i - 1].SetActive(i < remainingLifes);
        }
    }

    void OnItemChange(GameObject go)
    {
        if (go == null)
        {

        }
        else
        {
            itemSprite.sprite = go.GetComponent<SpriteRenderer>().sprite;
        }
    }
}
