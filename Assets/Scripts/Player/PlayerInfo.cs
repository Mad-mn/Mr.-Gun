using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public static PlayerInfo _playerInfo;

    private void Awake()
    {
        if(_playerInfo == null)
        {
            _playerInfo = this;
        }
    }

    private int _coins;
    
    public int BestScore { get; set; }

    public void AddCoins(int count)
    {
        _coins += count;
    }

    public int GetCointsCount()
    {
        return _coins;
    }

    public void DecrementCoins(int count)
    {
        _coins -= count;
    }
}
