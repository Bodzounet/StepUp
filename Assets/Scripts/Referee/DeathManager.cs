using UnityEngine;
using System.Collections;

public class DeathManager : MonoBehaviour 
{
    public delegate void LifeNumberChange(int newLifeNumber);
    public delegate void Death();
    public delegate void Respawn();
    public delegate void Win(GameObject go);

    public event LifeNumberChange OnLifeNumberChange;
    public event Death OnDeath;
    public event Win OnWin;
    public event Respawn OnRespawn;

    private int _lifes = 3;
    public int Lifes
    {
        get { return _lifes; }
        set
        {
            _lifes = value; 
            if (OnLifeNumberChange != null)
            {
                OnLifeNumberChange(_lifes);
            }
        }
    }

    private void Start()
    {
        OnRespawn += this.GetComponent<Controller>().ResetController;
    }

    private void Die()
    {   
        this.gameObject.SetActive(false);
        if (OnDeath != null)
            OnDeath();
    }

    private void RespawnPlayer()
    {
        if (OnRespawn != null)
            OnRespawn();

        //var camPos = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();

        //transform.position = new Vector3(-5.0f, camPos.position.y + 4.0f, 0);

        //int layermask = 1 << LayerMask.NameToLayer("Platform");

        //for (int i = -5; i <= 5; i++)
        //{
        //    if (Physics.Raycast(transform.position, -transform.up, Mathf.Infinity, layermask))
        //        return;
        //    transform.position = new Vector3(i, camPos.position.y + 4.0f, 0);
        //}

        Transform respawnPos = GameObject.FindGameObjectWithTag("MainCamera").transform.GetChild(Random.Range(0, 2));
        respawnPos.SendMessage("WakeUp");
        transform.position = new Vector2(respawnPos.position.x, respawnPos.position.y) + Vector2.up * 0.5f;

        StartCoroutine(Co_SetPlayerInvulnerableAfterDeath());
    }

    IEnumerator Co_SetPlayerInvulnerableAfterDeath()
    {
        yield return new WaitForEndOfFrame();

        Items.Inventory inventory = this.GetComponent<Items.Inventory>();

        inventory.PickSpecificItem(GameObject.FindObjectOfType<Items.ItemManager>().PickSpecificItem("Shield"));
        inventory.UseItem();
        this.transform.GetComponentInChildren<Items.Shield>().ChangeDuration(1.5f);
    }

    public void Wins()
    {
        if (OnWin != null)
            OnWin(this.gameObject);
    }

    public void LoseLife()
    {
        Lifes--;

        if (Lifes == 0)
            Die();
        else
            RespawnPlayer();
        SoundManager.PlaySound("Ouilleouilleouilleh");
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "KillZone")
        {
            LoseLife();
        }
    }
}
