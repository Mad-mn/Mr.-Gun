using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    public static LevelController _levelController;
    [SerializeField] private List<Transform> _destinationPoints;
    [SerializeField] private List<Transform> _enemyDestinationPoints;
    [SerializeField] private List<Transform> _enemySpawnPoint;
    [SerializeField] private int _enemyCountInFirstLevel;
    [SerializeField] private float _firstLevelBossHp;
    [SerializeField] private List <GameObject> _stairBlocks;
    [SerializeField] private LevelType _type;

    [SerializeField] private List<GameObject> _platforms;

    private int _levelId;
    public int NextStairCount { get; set; }

    private void Awake()
    {
        if(_levelController == null)
        {
            _levelController = this;
        }
        foreach(Transform tr in _destinationPoints)
        {
            tr.gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        MainController.OnStartGame.AddListener(EnablePlayerdestinationPoints);
    }

    public List<Transform> GetDestinationPoints()
    {
        return _destinationPoints;
    }

    public List<Transform> GetEnemyDestinationPoints()
    {
        return _enemyDestinationPoints;
    }

    public List<Transform> GetEnemySpawnPoints()
    {
        return _enemySpawnPoint;
    }

    public void EnablePlayerdestinationPoints()
    {
        foreach (Transform tr in _destinationPoints)
        {
            tr.gameObject.SetActive(true);
        }
    }

    public bool IsLastPoint()
    {
        return PlayerController._player.CheckedPointCount == _destinationPoints.Count ? true : false;
    }

    public int GetLevelId()
    {
        return SceneManager.GetActiveScene().buildIndex + 1 ;
    }

    public int GetSingleEnemyCount()
    {
        return _enemyCountInFirstLevel;
    }

    public float GetBossHp()
    {
        return _firstLevelBossHp;
    }

    public string GetBossName()
    {
        return "Bob";
    }

    public void EnableStairs(int count)
    {
        NextStairCount = count;
        Invoke("OnStairs", 1f);
    }

    public void OnStairs()
    {
        if (_type == LevelType.Simple)
        {
            _stairBlocks[NextStairCount].SetActive(true);
        }
    }

    public void EnablebPlatform()
    {
        if (_platforms[PlayerController._player.KilledEnemyCount - 1])
        {
            _platforms[PlayerController._player.KilledEnemyCount - 1].SetActive(true);
        }
    }

    public void DisablePlatform()
    {
        _platforms[PlayerController._player.KilledEnemyCount].SetActive(false);
    }

    public LevelType GetLevelType()
    {
        return _type;
    }

    public enum LevelType { Simple, SpiralTower, Train, FlyingPlatforms, SwingPlatform}
}
