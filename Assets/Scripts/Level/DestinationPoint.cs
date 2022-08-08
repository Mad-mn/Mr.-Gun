using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestinationPoint : MonoBehaviour
{
    public bool IsActiveToEnemy { get; set; }
    private bool _isPlayerActive;

    [SerializeField] private flyingPlatfirm _flyingPlatform;
    [SerializeField] private TypePoint _type;

    [SerializeField] private Swing _swing;

    private void Start()
    {
        IsActiveToEnemy = true;
        _isPlayerActive = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (_type == TypePoint.Player )
        {
            if (other.CompareTag("Player") && _isPlayerActive)
            {
                _isPlayerActive = false;
                if (!LevelController._levelController.IsLastPoint())
                {
                    if (LevelController._levelController.GetLevelType() == LevelController.LevelType.FlyingPlatforms )
                    {
                        if(_flyingPlatform != null)
                        {
                            _flyingPlatform.GoToPoint();
                            PlayerController._player.FlyingPlatform = _flyingPlatform;
                            PlayerController._player.IsOnPlatform = true;
                        }
                    }else if( LevelController._levelController.GetLevelType() == LevelController.LevelType.SwingPlatform)
                    {
                        if(_swing != null)
                        {
                            _swing.StartRotation();
                            PlayerController._player.Swing = _swing;
                            PlayerController._player.SwingPosition = _swing.GetPlatformPosition();
                            PlayerController._player.IsOnSwing = true;
                        }
                    }
                    if (PlayerController._player.KilledEnemyCount > 0)
                    {
                        other.GetComponentInParent<PlayerController>().LookAtNextEnemy();
                        other.GetComponentInParent<PlayerController>().ChangeMovementAnimation();
                    }
                    Destroy(gameObject);
                }
            }
        }
        if (_type == TypePoint.Enemy)
        {
            if (other.CompareTag("Enemy") && IsActiveToEnemy)
            {
                IsActiveToEnemy = false;
                Enemy enemy = other.GetComponentInParent<Enemy>();
                if (LevelController._levelController.GetLevelType() == LevelController.LevelType.FlyingPlatforms)
                {
                    if (_flyingPlatform != null)
                    {
                        _flyingPlatform.GoToPoint(enemy);
                        enemy.FlyingPlatform = _flyingPlatform;
                        enemy.IsOnPlatform = true;
                    }
                }
                else if (LevelController._levelController.GetLevelType() == LevelController.LevelType.SwingPlatform)
                {
                    if (_swing != null)
                    {
                        _swing.StartRotation();
                        enemy.Swing = _swing;
                        enemy.SwingPosition = _swing.GetPlatformPosition();
                        enemy.IsOnSwing = true;
                    }
                }
                enemy.ChangeMovementAnimation();
                enemy.IsOnPoint = true;
                Destroy(gameObject);
                
            }
        }
    }

    public enum TypePoint { Player, Enemy}
}
