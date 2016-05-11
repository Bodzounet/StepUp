﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameConfig : Singleton<GameConfig>
{
    private List<int> _players = new List<int>();
    public List<int> Players
    {
        get { return _players; }
        set { _players = value; }
    }

    public void ResetGameConfig()
    {
        _players.Clear();
    }
}
