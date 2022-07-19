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

    private NavMeshAgent _agent;
    private List<Transform> _destinationPoints;

    public bool IsOnPoint { get; set; }

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _destinationPoints = LevelController._levelController.GetEnemyDestinationPoints();
        Bullet.OnMiss.AddListener(Shot);
        IsOnPoint = false;
    }

    private void Update()
    {
        if (IsOnPoint)
        {
            transform.LookAt(PlayerController._player.gameObject.transform.position, Vector3.up);
            transform.localRotation = Quaternion.Euler(0, transform.localRotation.y, 0);
        }
    }


    public void ShotOnEnemy()
    {
        if (_type != EnemyController.EnemyType.Boss)
        {
            PlayerController._player.MoveToNextPoint();
            EnemyController._enemyController.ShotInEnemy();

            gameObject.SetActive(false);
        }
        else
        {
            MoveToNextPoint();
            PlayerController._player.MoveToNextPoint();
        }
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
        IsOnPoint = false;
        _agent.isStopped = false;
       
        _agent.destination = _destinationPoints[PlayerController._player.CheckedPointCount +1].position;
    }

    public EnemyController.EnemyType GetEnemyType()
    {
        return _type;
    }

}
