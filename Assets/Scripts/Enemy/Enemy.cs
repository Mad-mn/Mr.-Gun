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

    //[SerializeField] private float _bossHp;
    //[SerializeField] private string _bossName;

    private NavMeshAgent _agent;
    private List<Transform> _destinationPoints;
    private EnemyPrefab _enemyPrefab;
    private Animator _enemyAnimator;

    private Transform _headBone, _bodyBone;
    private string _idleName;
    private bool _isRun;

    public bool IsOnPoint { get; set; }

    public float BossHp { get; set; }
    public string BossName { get; set; }

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
    }

    
    private void Update()
    {
        if (IsOnPoint)
        {
            Vector3 LookAtPosition = new Vector3(PlayerController._player.gameObject.transform.position.x, transform.position.y, PlayerController._player.gameObject.transform.position.z);
            transform.LookAt(LookAtPosition, Vector3.up);
            //transform.rotation = Quaternion.Euler(0, transform.rotation.y, 0);
        }
    }

    public void CreateRandomEnemy()
    {
        GameObject enem = Instantiate(_enemyPrefabs[Random.Range(0, _enemyPrefabs.Count)], transform);
        _enemyAnimator = enem.GetComponent<Animator>();
        _enemyPrefab = enem.GetComponent<EnemyPrefab>();
        _headBone = _enemyPrefab.GetHeadBone();
        _bodyBone = _enemyPrefab.GetBodyBone();
        _enemyAnimator.SetBool(_idleName, true);
       
    }

    public void ShotOnEnemy(float damage, Bullet.BulletType bulletType)
    {
        if (_type != EnemyController.EnemyType.Boss)
        {
            PlayerController._player.MoveToNextPoint(false);
            EnemyController._enemyController.ShotInEnemy();
            OnDeathAnimatioon(bulletType);
            Destroy(gameObject, 1f);
            GameSessionController._sessionController.LastSingleEnemy--;
            CanvasController._canvasController.FillLevelProggress();
        }
        else
        {
            CanvasController._canvasController.ChangeBossHp(damage);
            BossHp -= damage;
            
            if (BossHp <= 0)
            {
                KillTheBoss(bulletType);
            }
            else
            {
                    LevelController._levelController.NextStairCount++;
                    LevelController._levelController.OnStairs();
                    MoveToNextPoint();

                PlayerController._player.MoveToNextPoint(true);
            }
        }
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

    public void KillTheBoss(Bullet.BulletType bulletType)
    {
        OnDeathAnimatioon(bulletType);
        Destroy(gameObject, 1f);
        GameSessionController._sessionController.KillTheBoss();
    }

    

    public void Shot()
    {
        Vector3 fromTo = PlayerController._player.GetHeadPosition().position - transform.position;
        Vector3 fromToXZ = new Vector3(fromTo.x, 0, fromTo.z);

        float x = fromToXZ.magnitude;
        float y = fromTo.y;

        float angleInRad = _shotAngle * Mathf.PI / 180;

        float v2 = (Physics.gravity.y * x * x) /( 2 * (y - Mathf.Tan(angleInRad) * x) * Mathf.Pow(Mathf.Cos(angleInRad), 2));
        float v = Mathf.Sqrt(Mathf.Abs(v2));


        GameObject bullet = Instantiate(PlayerInfo._playerInfo._bullet, _bulletSpawnPosition.position, Quaternion.identity);
        Vector3 shotDirection = (PlayerController._player.GetHeadPosition().position - transform.position).normalized;
        bullet.GetComponent<Rigidbody>().AddForce(shotDirection * v, ForceMode.Impulse);
        bullet.GetComponent<Bullet>().IsEnemyBullet = true;
    }

    public void MoveToNextPoint()
    {
        ChangeMovementAnimation();
        IsOnPoint = false;
        _agent.isStopped = false;
        
        _agent.destination = _destinationPoints[PlayerController._player.CheckedPointCount +1].position;
       
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
    }

    public void EnableFlash()
    {
        _flash.SetActive(true);
    }

    public void EnablePumpkin()
    {
        _enemyPrefab.EnablePumpkin();
    }
    
}
