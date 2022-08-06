using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletButton : MonoBehaviour
{
    [SerializeField] private Bullet.BulletType _type;
    [SerializeField] private GameObject _lockImage;

    private bool _isOpen;

    private void Start()
    {
        if (WeaponController._weaponController.IsBulletOppened(_type))
        {
            _lockImage.SetActive(false);
            _isOpen = true;
        }
        else
        {
            _lockImage.SetActive(true);
            _isOpen = false;
        }
    }

    public void SetBullet()
    {
        // PlayerInfo._playerInfo._bullet = _bullet;
        if (_isOpen)
        {
            WeaponController._weaponController.SetCurrentBullet(_type);
        }
    }
}
