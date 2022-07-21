using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    public static LevelController _levelController;
    [SerializeField] private List<Transform> _destinationPoints;
    [SerializeField] private List<Transform> _enemyDestinationPoints;
    [SerializeField] private int _enemyCountInFirstLevel;
    [SerializeField] private float _firstLevelBossHp;

    private int _levelId;

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

    public int GetLevelId()
    {
        return SceneManager.GetActiveScene().buildIndex +1 ;
    }

    public int GetSingleEnemyCount()
    {
        return _enemyCountInFirstLevel + GetLevelId();
    }

    public float GetBossHp()
    {
        return _firstLevelBossHp + GetLevelId();
    }

    public string GetBossName()
    {
        return "Bob";
    }
}
