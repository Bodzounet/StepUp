using UnityEngine;
using System.Collections;

public class DeathManager : MonoBehaviour 
{
    public delegate void LifeNumberChange(int newLifeNumber);
    public delegate void Death();
    public delegate void Win(GameObject go);

    public event LifeNumberChange OnLifeNumberChange;
    public event Death OnDeath;
    public event Win OnWin;

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

    private void Die()
    {
        this.gameObject.SetActive(false);
        if (OnDeath != null)
            OnDeath();
    }

    private void Respawn()
    {
        var camPos = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();

        transform.position = new Vector3(-5.0f, camPos.position.y + 4.0f, 0);

        int layermask = 1 << LayerMask.NameToLayer("Platform");

        for (int i = -5; i <= 5; i++)
        {
            if (Physics.Raycast(transform.position, -transform.up, Mathf.Infinity, layermask))
                return;
            transform.position = new Vector3(i, camPos.position.y + 4.0f, 0);
        }
    }

    public void Wins()
    {
        if (OnWin != null)
            OnWin(this.gameObject);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "KillZone")
        {
            if (Lifes == 0)
                Die();
            else
                Respawn();

            Lifes--;
        }
    }
}
