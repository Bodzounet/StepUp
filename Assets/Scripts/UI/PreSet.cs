using UnityEngine;
using System.Collections;

public class PreSet : MonoBehaviour 
{
    void Awake()
    {
        GameConfig.Instance.Players.Add(1);
        GameConfig.Instance.Players.Add(2);
    }
}
