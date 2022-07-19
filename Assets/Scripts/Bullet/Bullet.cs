using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Bullet : MonoBehaviour
{
    public static UnityEvent OnMiss = new UnityEvent();

    private bool _isActive = true;

    public bool IsEnemyBullet { get; set; }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player") && !IsEnemyBullet)
        {
            if (_isActive)
            {
                Enemy enemy = collision.transform.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.ShotOnEnemy();
                }
                else
                {
                    OnMiss.Invoke();
                }
                _isActive = false;
            }
        }
        //Destroy(gameObject, 0.01f);
    }

    
}
