using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController _player;

    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private GameObject _bullet;
    [SerializeField] private GameObject _line;
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private Transform _headTransform;
    [SerializeField] private Animator _playerAnimator;
    [SerializeField] private Transform _headBone;
    [SerializeField] private GameObject _cactusInAss;
    [SerializeField] private GameObject _firework;
    [SerializeField] private GameObject _flash;
    [SerializeField] private GameObject _pumpkin;
    [SerializeField] private Rigidbody _regdollRb;
    [SerializeField] private GameObject _gun;
    [SerializeField] private Transform _gunBulletSpawn;

    public int CheckedPointCount { get; private set; }
    public int KilledEnemyCount { get; set; }
    private List<Transform> _destinationPoints;

    public bool IsAiming { get; private set; }
    public flyingPlatfirm FlyingPlatform { get; set; }
    public bool IsOnPlatform { get; set; }
    public bool IsDead { get; private set; }

    public Swing Swing { get; set; }
    public Transform SwingPosition { get; set; }
    public bool IsOnSwing { get; set; }

    private Vector3 _currentPosition;
    

    private void Awake()
    {
        if(_player == null)
        {
            _player = this;
        }
        else
        {
            Destroy(gameObject);
        }
        KilledEnemyCount = 0;
    }

    private void Start()
    {
        _destinationPoints = LevelController._levelController.GetDestinationPoints();
        StopAiming();
        MainController.OnStartGame.AddListener(Aiming);
    }

    private void Update()
    {
        if (IsOnPlatform)
        {
            MoveWithPlatform();
        }
        if (IsOnSwing)
        {
            MoveWithSwing();
        }
    }

    private void MoveWithPlatform()
    {
        _currentPosition = FlyingPlatform.gameObject.transform.position + new Vector3(0f, FlyingPlatform.transform.localScale.y / 4, 0f);
        transform.position = FlyingPlatform.gameObject.transform.position + new Vector3(0f, FlyingPlatform.transform.localScale.y / 4, 0f);
    }

    private void MoveWithSwing()
    {
        transform.position = SwingPosition.position + new Vector3(0f, SwingPosition.transform.localScale.y / 4, 0f);
    }

    public void MoveToNextPoint(bool isBoss)
    {
        if (isBoss)
        {
            MoveToNext();
        }
        else
        {
            Invoke("MoveToNext", 0.5f);
        }
    }

    private void MoveToNext()
    {
        if (Swing != null)
        {
            IsOnSwing = false;
        }
        ChangeMovementAnimation();
        _agent.isStopped = false;
        if (_destinationPoints.Count > CheckedPointCount)
        {
            if (IsAiming)
            {

                StopAiming();
            }
            if (_destinationPoints[KilledEnemyCount] != null)
            {
                _agent.destination = _destinationPoints[KilledEnemyCount].position;
                CheckedPointCount++;
            }

        }
    }

    public void HitOnPlayer(Bullet.BulletType bulletType, Vector3 direction, float force)   /// Гравець програв
    {
        if (bulletType == Bullet.BulletType.Beehive)
        {
            OnDeatAnimation(bulletType);
           
        }
        else
        {
            StartCoroutine(OnRegdoll(direction, force));
        }

        IsDead = true;
        MainController._main.EndGame();
        StopAiming();
    }

    public void OffAnimator()
    {
        _playerAnimator.enabled = false;
    }

    public IEnumerator OnRegdoll(Vector3 direction, float force)
    {
        OffAnimator();
        yield return new WaitForEndOfFrame();
        _regdollRb.isKinematic = false;
        _regdollRb.AddForce(direction * force, ForceMode.Impulse);
    }

    public void Shot()
    {
        Vector3 vector;
        Vector3 spawnPosition;
        if (LevelController._levelController.GetLevelType() == LevelController.LevelType.FlyingPlatforms)
        {
            vector = _line.GetComponent<AimLine>().GetVector().normalized;
            spawnPosition = _line.GetComponent<AimLine>().GetBulletSpawnPosition();
            float difference = _currentPosition.y - gameObject.transform.position.y;
            spawnPosition = spawnPosition + new Vector3(0, difference, 0);
        } 
        else
        {
             vector = _line.GetComponent<AimLine>().GetVector().normalized;
            spawnPosition = _line.GetComponent<AimLine>().GetBulletSpawnPosition();
        }
        Bullet b = PlayerInfo._playerInfo._bullet.GetComponent<Bullet>();
        if (b.GetBulletType() == Bullet.BulletType.Gun)
        {
            _gun.SetActive(true);
            EnableGunAnimation();
           

            StartCoroutine(ShotFromGun(vector, spawnPosition));
        }
        else
        {
           
            GameObject bullet = Instantiate(PlayerInfo._playerInfo._bullet, spawnPosition, Quaternion.identity);
            bullet.GetComponent<Rigidbody>().AddForce(vector * _bulletSpeed, ForceMode.Impulse);
        }
        if (FlyingPlatform != null)
        {
            FlyingPlatform.StopMoving();
        }
       
    }

    private IEnumerator ShotFromGun(Vector3 direction, Vector3 spawn)
    {
        StopAiming();
        yield return new WaitForSeconds(0.5f);
        GameObject bullet = Instantiate(PlayerInfo._playerInfo._bullet, spawn, Quaternion.identity);

        bullet.GetComponent<Rigidbody>().AddForce(direction * _bulletSpeed, ForceMode.Impulse);
        _playerAnimator.SetBool("IsGun", false);
        _playerAnimator.applyRootMotion = false;
        yield return new WaitForSeconds(0.3f);
        _gun.SetActive(false);

    }

    public Transform GetHeadPosition()
    {
        return _headTransform;
    }

    public void Aiming()
    {
        IsAiming = true;
        _line.SetActive(true);
    }

    public void StopAiming()
    {
        IsAiming = false;
        _line.SetActive(false);
    }
    public void EnableGunAnimation()
    {
        _playerAnimator.applyRootMotion = true;
        _playerAnimator.SetBool("IsGun", true);
    }

    public void LookAtNextEnemy()
    {
        _agent.isStopped = true;
        if (EnemyController._enemyController.GetCurrentEnemy() != null)
        {
            Transform enemyPosition = EnemyController._enemyController.GetCurrentEnemy().transform;
            if (LevelController._levelController.GetLevelType() != LevelController.LevelType.Train && LevelController._levelController.GetLevelType() != LevelController.LevelType.FlyingPlatforms && LevelController._levelController.GetLevelType() != LevelController.LevelType.SwingPlatform)
            {
                StartCoroutine(LookAtEnemy(enemyPosition));
            }
            if(LevelController._levelController.GetLevelType() == LevelController.LevelType.SwingPlatform)
            {
                transform.rotation = Quaternion.Euler(Vector3.zero);
            }
           
            Aiming();
        }
    }

    private IEnumerator LookAtEnemy(Transform enemy)
    {
        Quaternion a = transform.rotation;
        Vector3 pos = new Vector3(enemy.position.x, transform.position.y, enemy.position.z);
        Vector3 heading = enemy.position - transform.position;
        float distance = heading.magnitude;

        Vector3 direction = heading / distance;

        float yAngle = Vector3.SignedAngle(transform.forward, direction, Vector3.up);
        Quaternion b = Quaternion.Euler(0, -180*CheckedPointCount, 0);

        float t = 0;
        while (true)
        {
            if (t > 1+_rotationSpeed)
            {
                yield break;
            }
            transform.rotation = Quaternion.Lerp(a, b, t);
            t += _rotationSpeed;
            yield return new WaitForEndOfFrame();

        }
    }

    public void ChangeMovementAnimation()
    {
        _playerAnimator.SetBool("IsMove", !_playerAnimator.GetBool("IsMove"));
    }

    public void SetBoneforBullet(GameObject bullet)
    {
        bullet.transform.parent = _headBone;
    }

    public void OnDeatAnimation(Bullet.BulletType bulletType)
    {
        
        _playerAnimator.applyRootMotion = true;
        switch (bulletType)
        {
            case Bullet.BulletType.BoxingGlove:
                _playerAnimator.SetBool("IsDeath", true);
                break;
            case Bullet.BulletType.ButcherKnife:
                _playerAnimator.SetBool("IsKnife", true);
                break;
            case Bullet.BulletType.Cactus:
                _playerAnimator.SetBool("IsCactus", true);
                break;
            case Bullet.BulletType.Cake:
                _playerAnimator.SetBool("IsCake", true);
                break;
            case Bullet.BulletType.BlueBottle:
                _playerAnimator.SetBool("IsBlueBottle", true);
                break;
            case Bullet.BulletType.GreenBottle:
                _playerAnimator.SetBool("IsGreenBottle", true);
                break;
            case Bullet.BulletType.Firework:
                _playerAnimator.SetBool("IsFirework", true);
                break;
            case Bullet.BulletType.Banana:
                _playerAnimator.SetBool("IsBanana", true);
                break;
            case Bullet.BulletType.Onion:
                _playerAnimator.SetBool("IsOnion", true);
                break;
            case Bullet.BulletType.Flash:
                _playerAnimator.SetBool("IsFlash", true);
                break;
            case Bullet.BulletType.Pumpkin:
                _playerAnimator.SetBool("IsPumpkin", true);
                break;
            case Bullet.BulletType.Beehive:
                _playerAnimator.applyRootMotion = false;
                _playerAnimator.SetBool("IsBeehive", true);
                break;
        }
    }

    public Transform GetHeadBone()
    {
        return _headBone;
    }

    public void EnableCactusInAss()
    {
        _cactusInAss.SetActive(true);
    }

    public void EnableFirework()
    {
        _firework.SetActive(true);
    }

    public void EnableFlash()
    {
        _flash.SetActive(true);
    }

    public void EnablePumpkin()
    {
        _pumpkin.SetActive(true);
    }
}
