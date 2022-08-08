using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flyingPlatfirm : MonoBehaviour
{
    [SerializeField] private Transform _upperPoint, _lowwerPoint;
    [SerializeField] private float _speed;

    private Vector3 _startPoint;
    private Coroutine _coroutine;

    private Enemy _enemy;
    private BoxCollider _collider;

    private void Start()
    {
        _startPoint = transform.localPosition;
        _collider = gameObject.GetComponent<BoxCollider>();
    }

    public void GoToPoint(Enemy enemy = null)
    {
        
            _collider.enabled = false;
        
        LevelController._levelController.DisablePlatform();
        _coroutine = StartCoroutine(Moving(true));
        _enemy = enemy;
    }

    public void StopMoving()
    {
        StopCoroutine(_coroutine);
        StartCoroutine(Moving(false));
    }

    private IEnumerator Moving(bool isStart)
    {
        if (isStart)
        {
            int dir = Random.Range(0, 2);
            bool isUp = dir == 0 ? true : false;
            Vector3 target = isUp ? _upperPoint.localPosition : _lowwerPoint.localPosition;

            while (true)
            {
                Vector3 direction = (target - transform.localPosition).normalized;
                float distance = Vector3.Distance(transform.localPosition, target);
                if (distance >= 0.1f)
                {
                    transform.Translate(direction * _speed * Time.deltaTime);
                    
                }
                else
                {
                    isUp = !isUp;
                    target = isUp ? _upperPoint.localPosition : _lowwerPoint.localPosition;
                }

                yield return new WaitForEndOfFrame();
            }
        }
        else
        {
            if (_enemy != null)
            {
                _enemy.IsOnPlatform = false;
                if (_enemy.GetEnemyType() == EnemyController.EnemyType.Boss)
                {
                    _enemy.MoveToNextPoint();
                }
            }
            while (true)
            {
                Vector3 direction = (_startPoint - transform.localPosition).normalized;
                float distance = Vector3.Distance(transform.localPosition, _startPoint);
                if (distance >= 0.03f)
                {
                    transform.Translate(direction * _speed * 1.5f * Time.deltaTime);
                   
                }
                else
                {
                    if(_enemy!= null )
                    {
                       
                    }
                    else
                    {
                        
                        PlayerController._player.IsOnPlatform = false;
                        
                         PlayerController._player.MoveToNextPoint(false);
                        
                    }
                    yield break;
                }

                yield return new WaitForEndOfFrame();
            }
        }

    }
}
