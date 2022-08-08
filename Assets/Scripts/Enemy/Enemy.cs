using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyController.EnemyType _type;
    [SerializeField] private Transform _bulletSpawnPosition;
    [SerializeField] private GameObject _bullet;
    [SerializeField] private float _shotAngle;
    
    [SerializeField] private List<GameObject> _enemyPrefabs;
    [SerializeField] private GameObject _firework;
    [SerializeField] private GameObject _flash;
    [SerializeField] private float _bulletForce;
    [SerializeField] private float _bulletSpeed;

    //[SerializeField] private float _bossHp;
    //[SerializeField] private string _bossName;

    private NavMeshAgent _agent;
    private List<Transform> _destinationPoints;
    private EnemyPrefab _enemyPrefab;
    private Animator _enemyAnimator;

    private Transform _headBone, _bodyBone;
    private Rigidbody _regdollRb;
    private string _idleName;
    private bool _isRun;
    private bool isActive = true;
    private GameObject _gun;

    public bool IsOnPoint { get; set; }

    public float BossHp { get; set; }
    public string BossName { get; set; }

    public flyingPlatfirm FlyingPlatform { get; set; }
    public bool IsOnPlatform { get; set; }

    public Swing Swing { get; set; }
    public Transform SwingPosition { get; set; }
    public bool IsOnSwing { get; set; }

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _destinationPoints = LevelController._levelController.GetEnemyDestinationPoints();
        Bullet.OnMiss.AddListener(Shot);
        IsOnPoint = true;
        if (_type == EnemyController.EnemyType.Boss)
        {
            CanvasController._canvasController.SetBossInfo(BossHp, BossName);
        }
        
        int rnd = Random.Range(1, 5);
        _idleName = "IsIdle" + rnd;
        CreateRandomEnemy();
        _gun = _enemyPrefab.GetGun();
        _enemyAnimator.SetBool(_idleName, true);
        MoveToFirstPoint();
    }

    
    private void Update()
    {
        if (IsOnPoint)
        {
            Vector3 LookAtPosition = new Vector3(PlayerController._player.gameObject.transform.position.x, transform.position.y, PlayerController._player.gameObject.transform.position.z);
            transform.LookAt(LookAtPosition, Vector3.up);
            //transform.rotation = Quaternion.Euler(0, transform.rotation.y, 0);
        }
        if (IsOnPlatform)
        {
            MoveWithPlatform();
        }
        if (IsOnSwing)
        {
            MoveWithSwing();
        }
    }

    private void MoveWithSwing()
    {
        transform.position = SwingPosition.position + new Vector3(0f, SwingPosition.transform.localScale.y / 4, 0f);
    }

    public void CreateRandomEnemy()
    {
        GameObject enem = Instantiate(_enemyPrefabs[Random.Range(0, _enemyPrefabs.Count)], transform);
        _enemyAnimator = enem.GetComponent<Animator>();
        _enemyPrefab = enem.GetComponent<EnemyPrefab>();
        _headBone = _enemyPrefab.GetHeadBone();
        _bodyBone = _enemyPrefab.GetBodyBone();
        _enemyAnimator.SetBool(_idleName, true);
        _regdollRb = _enemyPrefab.GetRegdollRigidbody();
       
    }

   
    private void MoveWithPlatform()
    {
        transform.position = FlyingPlatform.gameObject.transform.position + new Vector3(0f, FlyingPlatform.transform.localScale.y/4, 0f);
    }

    public void ShotOnEnemy(float damage, Bullet.BulletType bulletType, Vector3 direction, float force)
    {
        if (Swing != null)
        {
            IsOnSwing = false;
        }
        if (_type != EnemyController.EnemyType.Boss)
        {
            if (LevelController._levelController.GetLevelType() != LevelController.LevelType.FlyingPlatforms)
            {
                PlayerController._player.MoveToNextPoint(false);
            }
            else
            {
                FlyingPlatform.StopMoving();
            }
            PlayerController._player.KilledEnemyCount++;
            EnemyController._enemyController.ShotInEnemy();
            if(bulletType == Bullet.BulletType.Beehive)
            {
                OnDeathAnimatioon(bulletType);
                Destroy(gameObject, 1f);
            }
            else
            {
                StartCoroutine(OnRegdoll(direction, force));
            }
           
            GameSessionController._sessionController.LastSingleEnemy--;
            CanvasController._canvasController.FillLevelProggress();
            isActive = false;
        }
        else
        {
            CanvasController._canvasController.ChangeBossHp(damage);
            BossHp -= damage;

            PlayerController._player.KilledEnemyCount++;
            if (BossHp <= 0)
            {
                KillTheBoss(bulletType, direction, force);
                if (FlyingPlatform != null)
                {
                    FlyingPlatform.StopMoving();
                }
            }
            else
            {
                LevelController._levelController.NextStairCount++;
                LevelController._levelController.OnStairs();
                if (LevelController._levelController.GetLevelType() != LevelController.LevelType.FlyingPlatforms)
                {
                    MoveToNextPoint();
                    PlayerController._player.MoveToNextPoint(true);
                }
                else
                {
                    if (FlyingPlatform != null)
                    {
                        FlyingPlatform.StopMoving();
                    }
                }
               
            }
        }
    }

    public void OffAnimator()
    {
        _enemyAnimator.enabled = false;
    }

    public IEnumerator OnRegdoll(Vector3 direction, float force)
    {
        _agent.enabled = false;
        OffAnimator();
        yield return new WaitForEndOfFrame();
        _regdollRb.isKinematic = false;
        direction = direction + Vector3.right;
        _regdollRb.AddForce(direction * force, ForceMode.Impulse);
    }

    public void SetBoneForBullet(GameObject bullet, bool isHeadShot)
    {
        if (isHeadShot)
        {
            bullet.transform.parent = _headBone;
        }
        else
        {
            bullet.transform.parent = _bodyBone;
        }
    }

    public void KillTheBoss(Bullet.BulletType bulletType, Vector3 direction, float force)
    {
        GameSessionController._sessionController.KillTheBoss();
        isActive = false;
        if (bulletType == Bullet.BulletType.Beehive)
        {
            OnDeathAnimatioon(bulletType);
            Destroy(gameObject, 1f);
        }
        else
        {
            StartCoroutine(OnRegdoll(direction, force));
        }
       
        
    }

    

    public void Shot()
    {
        if (isActive)
        {
            Vector3 fromTo = PlayerController._player.GetHeadPosition().position - transform.position;
            Vector3 fromToXZ = new Vector3(fromTo.x, 0, fromTo.z);

            float x = fromToXZ.magnitude;
            float y = fromTo.y;

            float angleInRad = _shotAngle * Mathf.PI / 180;

            float v2 = (Physics.gravity.y * x * x) / (2 * (y - Mathf.Tan(angleInRad) * x) * Mathf.Pow(Mathf.Cos(angleInRad), 2));
            float v = Mathf.Sqrt(Mathf.Abs(v2));

            Vector3 shotDirection = (PlayerController._player.GetHeadPosition().position - transform.position).normalized;

            Bullet b = PlayerInfo._playerInfo._bullet.GetComponent<Bullet>();
            if (b.GetBulletType() == Bullet.BulletType.Gun)
            {
                _gun.SetActive(true);
                EnableGunAnimation();


                StartCoroutine(ShotFromGun(shotDirection, _bulletSpawnPosition.position, v));
            }
            else
            {
                GameObject bullet = Instantiate(WeaponController._weaponController.GetCurrentBullet(), _bulletSpawnPosition.position, Quaternion.identity);

                bullet.GetComponent<Rigidbody>().AddForce(shotDirection * _bulletSpeed, ForceMode.Impulse);
                bullet.GetComponent<Bullet>().IsEnemyBullet = true;
            }
        }
    }

    private IEnumerator ShotFromGun(Vector3 direction, Vector3 spawn, float bulletSpeed)
    {

        yield return new WaitForSeconds(0.5f);
        GameObject bullet = Instantiate(PlayerInfo._playerInfo._bullet, spawn, Quaternion.identity);

        bullet.GetComponent<Rigidbody>().AddForce(direction * bulletSpeed, ForceMode.Impulse);
        bullet.GetComponent<Bullet>().IsEnemyBullet = true;
        _enemyAnimator.SetBool("IsGun", false);
        
        _enemyAnimator.applyRootMotion = false;
        yield return new WaitForSeconds(0.3f);
        _enemyAnimator.SetBool(_idleName, true);
        _gun.SetActive(false);

    }

    public void EnableGunAnimation()
    {
        _enemyAnimator.applyRootMotion = true;
        _enemyAnimator.SetBool(_idleName, false);
        _enemyAnimator.SetBool("IsGun", true);
        
    }
    public void MoveToFirstPoint()
    {
        ChangeMovementAnimation();
        IsOnPoint = false;
        _agent.isStopped = false;

        _agent.destination = _destinationPoints[PlayerController._player.KilledEnemyCount].position;
    }

    public void MoveToNextPoint()
    {
        if (Swing != null)
        {
            IsOnSwing = false;
        }
        ChangeMovementAnimation();
        IsOnPoint = false;
        if (_agent.enabled)
        {
            _agent.isStopped = false;

            _agent.destination = _destinationPoints[PlayerController._player.KilledEnemyCount].position;
        }
    }

    public EnemyController.EnemyType GetEnemyType()
    {
        return _type;
    }

    public void SetEnemyType(EnemyController.EnemyType type)
    {
        _type = type;
    }

    public void ChangeMovementAnimation()
    {
        _enemyAnimator.SetBool(_idleName, !_enemyAnimator.GetBool(_idleName));
        _enemyAnimator.SetBool("IsMove", !_enemyAnimator.GetBool("IsMove"));
    }

    public void OnDeathAnimatioon(Bullet.BulletType bulletType)
    {
        _enemyAnimator.applyRootMotion = true;
        switch (bulletType)
        {
            case Bullet.BulletType.BoxingGlove:
                _enemyAnimator.SetBool("IsDeath", true);
                break;
            case Bullet.BulletType.ButcherKnife:
                _enemyAnimator.SetBool("IsKnife", true);
                break;
            case Bullet.BulletType.Cactus:
                _enemyAnimator.SetBool("IsCactus", true);
                break;
            case Bullet.BulletType.Cake:
                _enemyAnimator.SetBool("IsCake", true);
                break;
            case Bullet.BulletType.BlueBottle:
                _enemyAnimator.SetBool("IsBlueBottle", true);
                break;
            case Bullet.BulletType.GreenBottle:
                _enemyAnimator.SetBool("IsGreenBottle", true);
                break;
            case Bullet.BulletType.Firework:
                _enemyAnimator.SetBool("IsFirework", true);
                break;
            case Bullet.BulletType.Banana:
                _enemyAnimator.SetBool("IsBanana", true);
                break;
            case Bullet.BulletType.Onion:
                _enemyAnimator.SetBool("IsOnion", true);
                break;
            case Bullet.BulletType.Flash:
                _enemyAnimator.SetBool("IsFlash", true);
                break;
            case Bullet.BulletType.Pumpkin:
                _enemyAnimator.SetBool("IsPumpkin", true);
                break;
            case Bullet.BulletType.Beehive:
                _enemyAnimator.applyRootMotion = false;
                _enemyAnimator.SetBool("IsBeehive", true);
                break;
        }
       
    }

    public Transform GetHeadBone()
    {
        return _headBone;
    }

    public void CactusInAss()
    {
        _enemyPrefab.EnableCactus();
    }

    public void OnFirework()
    {
        _firework.SetActive(true);
        StartCoroutine(DisableGameObject(_firework));
    }

    public void EnableFlash()
    {
        _flash.SetActive(true);
        StartCoroutine(DisableGameObject(_flash));
    }

    private IEnumerator DisableGameObject(GameObject obj)
    {
        yield return new WaitForSeconds(1f);
        obj.SetActive(false);
    }
   

    public void EnablePumpkin()
    {
        _enemyPrefab.EnablePumpkin();
    }
    
}
