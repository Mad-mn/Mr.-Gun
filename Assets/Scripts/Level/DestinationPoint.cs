using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestinationPoint : MonoBehaviour
{
    public bool IsActiveToEnemy { get; set; }

    private void Start()
    {
        IsActiveToEnemy = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!LevelController._levelController.IsLastPoint())
            {
                other.GetComponent<PlayerController>().LookAtNextEnemy();
                other.GetComponent<PlayerController>().ChangeMovementAnimation();
            }
        }
        if (other.CompareTag("Enemy") && IsActiveToEnemy)
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy.GetEnemyType() == EnemyController.EnemyType.Boss)
            {

                enemy.ChangeMovementAnimation();
                enemy.IsOnPoint = true;
            }
        }
    }
}
