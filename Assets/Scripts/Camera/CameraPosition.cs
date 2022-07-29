using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPosition : MonoBehaviour
{
    [SerializeField] private Transform _player;

    private float _xOffset;
    private float _yOffset;
    private void Start()
    {
        _xOffset = transform.position.x - _player.position.x;
        _yOffset = transform.position.y - _player.position.y;
    }

    private void Update()
    {
       
        transform.position = new Vector3(_player.position.x + _xOffset, _player.position.y + _yOffset, transform.position.z);
    }
}
