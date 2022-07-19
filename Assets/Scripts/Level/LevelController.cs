using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public static LevelController _levelController;
    [SerializeField] private List<Transform> _destinationPoints;
    [SerializeField] private List<Transform> _enemyDestinationPoints;

    private void Awake()
    {
        if(_levelController == null)
        {
            _levelController = this;
        }
    }

    public List<Transform> GetDestinationPoints()
    {
        return _destinationPoints;
    }

    public List<Transform> GetEnemyDestinationPoints()
    {
        return _enemyDestinationPoints;
    }

    public bool IsLastPoint()
    {
        return PlayerController._player.CheckedPointCount == _destinationPoints.Count ? true : false;
    }
}
