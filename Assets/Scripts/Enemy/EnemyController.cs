using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public static EnemyController _enemyController;

    [SerializeField] private List<GameObject> _enemies;
    [SerializeField] private GameObject _enemyPrefab;

    public int EnemyChechedCount { get; set; }

    private List<Transform> _enemySpawnPoints;

    private void Awake()
    {
        if(_enemyController == null)
        {
            _enemyController = this;
        }
    }

    private void Start()
    {
        MainController.OnStartGame.AddListener(OnNextEnemy);
        MainController.OnStartGame.AddListener(SpawnEnemy);
        _enemySpawnPoints = LevelController._levelController.GetEnemySpawnPoints();

    }

    public List<GameObject> GetEnemyList()
    {
        return _enemies;
    }

    public void ShotInEnemy()
    {
        if(LevelController._levelController.GetLevelType() == LevelController.LevelType.Simple)
        {
            Invoke("SpawnEnemy", 1f);
        }
        else
        {
            SpawnEnemy();
        }
        
        LevelController._levelController.EnableStairs(EnemyChechedCount +1);

        EnemyChechedCount++;
        //OnNextEnemy();
    }

    private void OnNextEnemy()
    {
        if (EnemyChechedCount < _enemies.Count)
        {
            _enemies[EnemyChechedCount].SetActive(true);
        }
    }

    public GameObject GetCurrentEnemy()
    {
        return _enemies[EnemyChechedCount];
    }

    public void SpawnEnemy()
    {
       
        Vector3 spawnPoint;
       
        spawnPoint = _enemySpawnPoints[PlayerController._player.KilledEnemyCount].position;

        //GameObject point = _enemySpawnPoints[PlayerController._player.CheckedPointCount].gameObject;
        //Destroy(point);
        GameObject enemyObj = Instantiate(_enemyPrefab, spawnPoint, Quaternion.identity);
        
        if (_enemies.Count == LevelController._levelController.GetSingleEnemyCount())  /// Створюємо боса
        {
            Enemy enemy = enemyObj.GetComponent<Enemy>();
            enemy.SetEnemyType(EnemyType.Boss);
            enemy.BossName = LevelController._levelController.GetBossName();
            enemy.BossHp = LevelController._levelController.GetBossHp();
        }
        
        _enemies.Add(enemyObj);

       
    }

    public enum EnemyType { Single, Boss}

}
