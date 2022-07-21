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
    [SerializeField] private Animator _enemyAnimator;

    //[SerializeField] private float _bossHp;
    //[SerializeField] private string _bossName;

    private NavMeshAgent _agent;
    private List<Transform> _destinationPoints;

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

    public void ShotOnEnemy(float damage)
    {
        if (_type != EnemyController.EnemyType.Boss)
        {
            PlayerController._player.MoveToNextPoint();
            EnemyController._enemyController.ShotInEnemy();

            Destroy(gameObject);
        }
        else
        {
            CanvasController._canvasController.ChangeBossHp(damage);
            BossHp -= damage;
            
            if (BossHp <= 0)
            {
                KillTheBoss();
            }
            else
            {
                MoveToNextPoint();
                PlayerController._player.MoveToNextPoint();
            }
        }
    }

    public void KillTheBoss()
    {
        Destroy(gameObject);
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


        GameObject bullet = Instantiate(_bullet, _bulletSpawnPosition.position, Quaternion.identity);
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
        _enemyAnimator.SetBool("IsMove", !_enemyAnimator.GetBool("IsMove"));
    }

}
