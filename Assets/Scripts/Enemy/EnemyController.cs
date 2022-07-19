using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public static EnemyController _enemyController;

    [SerializeField] private List<GameObject> _enemies;

    public int EnemyChechedCount { get; set; }

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
    }

    public List<GameObject> GetEnemyList()
    {
        return _enemies;
    }

    public void ShotInEnemy()
    {
        EnemyChechedCount++;
        OnNextEnemy();
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

    public enum EnemyType { Single, Boss}

}
