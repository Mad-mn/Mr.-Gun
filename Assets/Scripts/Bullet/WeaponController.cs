using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public static WeaponController _weaponController;

    [SerializeField] private List<GameObject> _bullets;

    private Bullet.BulletType _currentBullet;
    private List<Bullet> _openedBullets;
    private List<Bullet> _closedBullets;

    private int _openBulletProggres;

    private void Awake()
    {
        if(_weaponController == null)
        {
            _weaponController = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this);
        InitializeClosedBulletsList();
    }


    private void InitializeClosedBulletsList()
    {
        _closedBullets = new List<Bullet>();
        _openedBullets = new List<Bullet>();
        if (_closedBullets.Count == 0)
        {
            foreach (GameObject obj in _bullets)
            {
                _closedBullets.Add(obj.GetComponent<Bullet>());
            }
            _currentBullet = _closedBullets[0].GetBulletType();
            _openedBullets.Add(_closedBullets[0]);
            _closedBullets.RemoveAt(0);
        }
    }

    public int GetBulletProggres()
    {
        return _openBulletProggres;
    }

    public void SetBulletProggres(int value)
    {
        if(value >=0 && value < 100)
        {
            _openBulletProggres = value;
        }
    }

    public void OpenNextWeapon()
    {
        _openedBullets.Add(_closedBullets[0]);
        _closedBullets.RemoveAt(0);
    }

    public GameObject GetCurrentBullet()
    {
        foreach(GameObject obj in _bullets)
        {
            if(obj.GetComponent<Bullet>().GetBulletType() == _currentBullet)
            {
                return obj;
            }
        }
        return null;
    }

    public void SetCurrentBullet(Bullet.BulletType type)
    {
        _currentBullet = type;
    }

    public bool IsBulletOppened(Bullet.BulletType type)
    {
        foreach(Bullet b in _openedBullets)
        {
            if(b.GetBulletType() == type)
            {
                return true;
            }
        }
        return false;
    }
}
