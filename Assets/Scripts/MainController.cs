using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class MainController : MonoBehaviour
{
    public static UnityEvent OnStartGame = new UnityEvent();

    [SerializeField] private PlayerController _player;

    private bool _isGamePlayed;

    private bool _isTouch;
    
    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            if (!_isGamePlayed)
            {
                StartGame();
            }
            else
            {
                if (_player.IsAiming)
                {
                    _player.Shot();
                }
            }
        }

#elif UNITY_ANDROID
         if (Input.touchCount > 0 && !_isTouch)
        {
            _isTouch = true;
            if (!_isGamePlayed)
            {
                StartGame();
            }
            else
            {
                if (_player.IsAiming)
                {
                    _player.Shot();
                }
            }
        }
        if(Input.touchCount == 0 && _isTouch)
        {
            _isTouch = false;
        }
#endif
    }
    public void StartGame()
    {
        OnStartGame.Invoke();
        _isGamePlayed = true;
    }

    public void EndGame()
    {
        _isGamePlayed = false;
    }
}
