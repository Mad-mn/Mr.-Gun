using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletButton : MonoBehaviour
{
    [SerializeField] private GameObject _bullet;

    public void SetBullet()
    {
        PlayerInfo._playerInfo._bullet = _bullet;
    }
}
