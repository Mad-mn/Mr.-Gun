using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPrefab : MonoBehaviour
{
    [SerializeField] private Transform _headBone, _bodyBone;
    [SerializeField] private GameObject _cactusInAss;
    [SerializeField] private GameObject _pumpkin;

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
}
