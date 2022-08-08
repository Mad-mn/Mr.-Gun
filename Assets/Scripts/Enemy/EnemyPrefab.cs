using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPrefab : MonoBehaviour
{
    [SerializeField] private Transform _headBone, _bodyBone;
    [SerializeField] private GameObject _cactusInAss;
    [SerializeField] private GameObject _pumpkin;
    [SerializeField] private Rigidbody _regdollRb;
    [SerializeField] private GameObject _gun;

    public Transform GetHeadBone()
    {
        return _headBone;
    }

    public Transform GetBodyBone()
    {
        return _bodyBone;
    }

   public void EnableCactus()
    {
        _cactusInAss.SetActive(true);
    }

    public void EnablePumpkin()
    {
        _pumpkin.SetActive(true);
    }

    public Rigidbody GetRegdollRigidbody()
    {
        return _regdollRb;
    }

    public GameObject GetGun()
    {
        return _gun;
    }
}
