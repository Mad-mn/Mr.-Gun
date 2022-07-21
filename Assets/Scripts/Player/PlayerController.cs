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

    public int CheckedPointCount { get; private set; }
    private List<Transform> _destinationPoints;

    public bool IsAiming { get; private set; }

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

    }

    private void Start()
    {
        _destinationPoints = LevelController._levelController.GetDestinationPoints();
        StopAiming();
        MainController.OnStartGame.AddListener(Aiming);
    }

    public void MoveToNextPoint()
    {
        ChangeMovementAnimation();
        _agent.isStopped = false;
        if (_destinationPoints.Count > CheckedPointCount)
        {
            if (IsAiming)
            {
                
                StopAiming();
            }
            _agent.destination = _destinationPoints[CheckedPointCount].position;
            CheckedPointCount++;
           
        }
    }

    public void HitOnPlayer()   /// Гравець програв
    {
        MainController._main.EndGame();
        StopAiming();
    }

    public void Shot()
    {
        Vector3 vector = _line.GetComponent<AimLine>().GetVector().normalized;
        Vector3 spawnPosition = _line.GetComponent<AimLine>().GetBulletSpawnPosition();
        //StopAiming();
        GameObject bullet = Instantiate(_bullet, spawnPosition, Quaternion.identity);

        bullet.GetComponent<Rigidbody>().AddForce(vector * _bulletSpeed, ForceMode.Impulse);
        
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
    
    public void LookAtNextEnemy()
    {
        _agent.isStopped = true;
        if (EnemyController._enemyController.GetCurrentEnemy() != null)
        {
            Transform enemyPosition = EnemyController._enemyController.GetCurrentEnemy().transform;

            StartCoroutine(LookAtEnemy(enemyPosition));
            //transform.Rotate(new Vector3(transform.rotation.x, 180 * CheckedPointCount, transform.rotation.z));
            Aiming();
        }
    }

    private IEnumerator LookAtEnemy(Transform enemy)
    {
        //for (int i = 0; i < 4; i++)
        //{
        //    transform.LookAt(enemy, Vector3.up);
        //    yield return new WaitForEndOfFrame();
        //}
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

    
}
