using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Bullet : MonoBehaviour
{
    public static UnityEvent OnMiss = new UnityEvent();

    [SerializeField] private int _headShotCoinPrice;
    [SerializeField] private float _headShotDamage, _bodyShotDamage;
    [SerializeField] private BulletType _type;

    private bool _isActive = true;
    private Rigidbody _rb;

    public bool IsEnemyBullet { get; set; }

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        LookAtVelocity();
    }

    private void LookAtVelocity()
    {
        if (_rb.velocity != Vector3.zero)
        {
            transform.forward = _rb.velocity.normalized;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isActive && !IsEnemyBullet && !collision.gameObject.CompareTag("Player"))
        {
            _isActive = false;
            if (collision.gameObject.CompareTag("EnemyHead") && !IsEnemyBullet)
            {
                Enemy enemy = collision.transform.GetComponentInParent<Enemy>();
                CanvasController._canvasController.AddCoins(_headShotCoinPrice);
                enemy.ShotOnEnemy(_headShotDamage);
                GameSessionController._sessionController.ShotOnEnemy(true);
            }
            else if (collision.gameObject.CompareTag("Enemy") && !IsEnemyBullet)
            {
                Enemy enemy = collision.transform.GetComponent<Enemy>();

                enemy.ShotOnEnemy(_bodyShotDamage);
                GameSessionController._sessionController.ShotOnEnemy(false);
            }
            else
            {
                OnMiss.Invoke();
            }
            if (_type == BulletType.ButcherKnife || _type == BulletType.Cake || _type == BulletType.Cactus)
            {
                _rb.isKinematic = true;
            }

        }
        if(_isActive && IsEnemyBullet && collision.gameObject.CompareTag("Player"))
        {
            _isActive = false;
            PlayerController._player.HitOnPlayer();
            if (_type == BulletType.ButcherKnife || _type == BulletType.Cake || _type == BulletType.Cactus)
            {
                _rb.isKinematic = true;
                Destroy(gameObject, 0.5f);
            }
        }

        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isActive && other.CompareTag("ExitBulletZone"))
        {
            _isActive = false;
            OnMiss.Invoke();
        }
    }

    public bool GetActive()
    {
        return _isActive;
    }

    public enum BulletType { BoxingGlove, ButcherKnife, Cake, Cactus}
}
