using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestinationPoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!LevelController._levelController.IsLastPoint())
            {
                other.GetComponent<PlayerController>().LookAtNextEnemy();
            }
        }
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy= other.GetComponent<Enemy>();
            if(enemy.GetEnemyType() == EnemyController.EnemyType.Boss)
            {
                
                enemy.IsOnPoint = true;
            }
        }
    }
}
