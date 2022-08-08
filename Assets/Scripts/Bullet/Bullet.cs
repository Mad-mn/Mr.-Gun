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
    [SerializeField] private Collider _bulletCollider;
    [SerializeField] private GameObject _poisinCloud;
    [SerializeField] private Animation _bulletAnimation;
    [SerializeField] private float _bulletForce;
    [SerializeField] private GameObject _granadeParticle;

    private bool _isActive = true;
    private Rigidbody _rb;

    public bool IsEnemyBullet { get; set; }

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (_isActive)
        {
            LookAtVelocity();
        }
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
                if (_type == BulletType.Cactus)
                {
                    enemy.CactusInAss();
                }
                if(_type == BulletType.Firework)
                {
                    enemy.OnFirework();
                }
                if(_type == BulletType.Flash)
                {
                    enemy.EnableFlash();
                }
                if(_type == BulletType.Pumpkin)
                {
                    enemy.EnablePumpkin();
                }
                CanvasController._canvasController.AddCoins(_headShotCoinPrice);
                enemy.ShotOnEnemy(_headShotDamage, _type, transform.forward, _bulletForce);
                GameSessionController._sessionController.ShotOnEnemy(true);
                SetBoneForBullet(collision.gameObject, true, true);
                CreatePoisonCloud(enemy.GetHeadBone());
                
            }
            else if (collision.gameObject.CompareTag("Enemy") && !IsEnemyBullet)
            {
                Enemy enemy = collision.transform.GetComponentInParent<Enemy>();
                if (_type == BulletType.Cactus)
                {
                    enemy.CactusInAss();
                }
                if (_type == BulletType.Firework)
                {
                    enemy.OnFirework();
                }
                if (_type == BulletType.Flash)
                {
                    enemy.EnableFlash();
                }
                if (_type == BulletType.Pumpkin)
                {
                    enemy.EnablePumpkin();
                }
                enemy.ShotOnEnemy(_bodyShotDamage, _type, transform.forward, _bulletForce);
                GameSessionController._sessionController.ShotOnEnemy(false);
                SetBoneForBullet(collision.gameObject, true, false);
                CreatePoisonCloud(enemy.GetHeadBone());
               
            }
            else
            {
                OnMiss.Invoke();
            }
            if (_type == BulletType.ButcherKnife || _type == BulletType.Cake || _type == BulletType.Cactus)
            {
                _rb.isKinematic = true;
                          
            }
            if (_type == BulletType.Granade)
            {
                GameObject granade = Instantiate(_granadeParticle, transform.position, Quaternion.identity);
                Destroy(granade, 1f);
            }

            if (_type == BulletType.Firework || _type == BulletType.GreenBottle || _type == BulletType.Cactus
                || _type == BulletType.Banana || _type == BulletType.Onion || _type == BulletType.Flash
                || _type == BulletType.Pumpkin || _type == BulletType.Granade || _type == BulletType.Beehive)
            {
                Destroy(gameObject);
            }
            if (_bulletAnimation != null)
            {
                _bulletAnimation.Stop();
            }
            if(LevelController._levelController.GetLevelType() == LevelController.LevelType.FlyingPlatforms)
            {
                LevelController._levelController.EnablebPlatform();
            }
           
        }
        if(_isActive && IsEnemyBullet && collision.gameObject.CompareTag("Player"))
        {
            _isActive = false;
            if (_type == BulletType.Cactus)
            {
                PlayerController._player.EnableCactusInAss();
            }
            if (_type == BulletType.Firework)
            {
                PlayerController._player.EnableFirework();
            }
            if (_type == BulletType.Flash)
            {
                PlayerController._player.EnableFlash();
            }
            if(_type == BulletType.Pumpkin)
            {
                PlayerController._player.EnablePumpkin();
            }
            PlayerController._player.HitOnPlayer(_type, transform.forward, _bulletForce);
            SetBoneForBullet(collision.gameObject, false, true);
            CreatePoisonCloud(PlayerController._player.GetHeadBone());
            if (_type == BulletType.ButcherKnife || _type == BulletType.Cake)
            {
                _rb.isKinematic = true;
               
            }
            if (_type == BulletType.Granade)
            {
                GameObject granade = Instantiate(_granadeParticle, transform.position, Quaternion.identity);
                Destroy(granade, 1f);
            }

            if (_type == BulletType.Firework || _type == BulletType.GreenBottle || _type == BulletType.Cactus
                || _type == BulletType.Banana || _type == BulletType.Onion || _type == BulletType.Flash
                || _type == BulletType.Pumpkin || _type == BulletType.Granade || _type == BulletType.Beehive)
            {
                Destroy(gameObject);
            }
            if (_bulletAnimation != null)
            {
                _bulletAnimation.Stop();
            }
        }
    }

    public void SetBoneForBullet(GameObject parent, bool isEnemy, bool isHeadshot)
    {
        if(_type == BulletType.ButcherKnife || _type == BulletType.Cake)
        {
            _bulletCollider.enabled = false;
            _rb.isKinematic = true;
            if (isEnemy)
            {
                Enemy enemy;
                if (isHeadshot)
                {
                    enemy = parent.transform.GetComponentInParent<Enemy>();
                }
                else
                {
                    enemy = parent.GetComponentInParent<Enemy>();
                }
                enemy.SetBoneForBullet(gameObject, isHeadshot);
            }
            else
            {
                PlayerController._player.SetBoneforBullet(gameObject);
            }
        }
        
    }

    public void CreatePoisonCloud(Transform parent)
    {
        if (_type == BulletType.GreenBottle)
        {
            GameObject poison = Instantiate(_poisinCloud, parent);
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

    public BulletType GetBulletType()
    {
        return _type;
    }

    public enum BulletType { BoxingGlove, ButcherKnife, Cake, Cactus, BlueBottle, GreenBottle, Firework, Banana, Onion, Flash, Pumpkin, Beehive, Granade, Brick, Gun}
}
