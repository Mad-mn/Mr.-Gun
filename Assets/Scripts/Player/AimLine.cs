using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimLine : MonoBehaviour
{
    [SerializeField] private Transform _origin, _vector;

    private Animation _animation;
    private Vector3 _playerStartPosition;

    

    private IEnumerator Start()
    {
        yield return null;
        _animation = GetComponent<Animation>();
        if(LevelController._levelController.GetLevelType() == LevelController.LevelType.FlyingPlatforms || LevelController._levelController.GetLevelType() == LevelController.LevelType.SwingPlatform)
        {
            _animation.clip = _animation.GetClip("Aiming_Flying_platform");
        }
        else
        {
            _animation.clip = _animation.GetClip("Aiming_aimLine");
        }
            _playerStartPosition = PlayerController._player.transform.position;
       
        
    }

    private void OnEnable()
    {

        _animation = GetComponent<Animation>();
        if (LevelController._levelController.GetLevelType() == LevelController.LevelType.FlyingPlatforms || LevelController._levelController.GetLevelType() == LevelController.LevelType.SwingPlatform)
        {
            _animation.clip = _animation.GetClip("Aiming_Flying_platform");
        }
        else
        {
            _animation.clip = _animation.GetClip("Aiming_aimLine");
        }
        _animation.Play();
    }
    public Vector3 GetVector()
    {
        return _vector.position - _origin.position;
    }

    public Vector3 GetBulletSpawnPosition()
    {
      
        return _origin.position;
    }
}
