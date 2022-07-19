using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimLine : MonoBehaviour
{
    [SerializeField] private Transform _origin, _vector;

    public Vector3 GetVector()
    {
        return _vector.position - _origin.position;
    }

    public Vector3 GetBulletSpawnPosition()
    {
        return _origin.position;
    }
}
