using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestinationPoint : MonoBehaviour
{
    public bool IsActiveToEnemy { get; set; }

    [SerializeField] private TypePoint _type;

    private void Start()
    {
        IsActiveToEnemy = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (_type == TypePoint.Player)
        {
            if (other.CompareTag("Player"))
            {
                if (!LevelController._levelController.IsLastPoint())
                {
                    other.GetComponentInParent<PlayerController>().LookAtNextEnemy();
                    other.GetComponentInParent<PlayerController>().ChangeMovementAnimation();
                    Destroy(gameObject);
                }
            }
        }
        if (_type == TypePoint.Enemy)
        {
            if (other.CompareTag("Enemy") && IsActiveToEnemy)
            {
                Enemy enemy = other.GetComponentInParent<Enemy>();
                if (enemy.GetEnemyType() == EnemyController.EnemyType.Boss)
                {

                    enemy.ChangeMovementAnimation();
                    enemy.IsOnPoint = true;
                    Destroy(gameObject);
                }
            }
        }
    }

    public enum TypePoint { Player, Enemy}
}
