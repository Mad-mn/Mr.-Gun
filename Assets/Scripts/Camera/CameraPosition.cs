using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPosition : MonoBehaviour
{
    [SerializeField] private Transform _player;

    private float _xOffset;
    private float _yOffset;
    private float _zOffset;
    private LevelController.LevelType _leveltype;

    private void Start()
    {
        _xOffset = transform.position.x - _player.position.x;
        _yOffset = transform.position.y - _player.position.y;
        _zOffset = transform.position.z - _player.position.z;

        _leveltype = LevelController._levelController.GetLevelType();
    }

    private void Update()
    {
        if (_leveltype == LevelController.LevelType.Simple)
        {
            transform.position = new Vector3(_player.position.x + _xOffset, _player.position.y + _yOffset, transform.position.z);
        } else if(_leveltype == LevelController.LevelType.FlyingPlatforms || _leveltype == LevelController.LevelType.SwingPlatform)
        {
            transform.position = new Vector3(_player.position.x + _xOffset,transform.position.y, _player.position.z + _zOffset);
        }
    }
}
