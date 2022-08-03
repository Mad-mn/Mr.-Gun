using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swing : MonoBehaviour
{
    [SerializeField] private float _angle;
    [SerializeField] private float _speed;
    [SerializeField] private Transform _platformPosition;

    private Coroutine _coroutine;

    public void StartRotation()
    {
        _coroutine = StartCoroutine(Rotation());
    }
    

    private IEnumerator Rotation()
    {
        Quaternion lowwer =Quaternion.Euler(-_angle, transform.localRotation.y, transform.localRotation.z);
        Quaternion upper = Quaternion.Euler(_angle, transform.localRotation.y, transform.localRotation.z);
        float t = 0.5f;
        float step = (0.02f * (_angle + _angle)) / _speed;
        while (true)
        {
            transform.rotation = Quaternion.Lerp(lowwer, upper, t);
            t += step;
            yield return new WaitForFixedUpdate();
            if(t >= 1 || t <= 0)
            {
                step *= -1;
            }
        }
    }

    public Transform GetPlatformPosition()
    {
        return _platformPosition;
    }
}
