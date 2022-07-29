using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public static PlayerInfo _playerInfo;

    [SerializeField] private GameObject _defaultBullet;

    private int _coins;

    public int BestScore { get; set; }
    public GameObject _bullet { get; set; }

    private void Awake()
    {
        if(_playerInfo == null)
        {
            _playerInfo = this;
        }
    }

    private void Start()
    {
        _bullet = _defaultBullet;
    }


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
