using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenWeaponProggres : MonoBehaviour
{
    [SerializeField] private Slider _proggresSlider;
    [SerializeField] private int _levelProggresStep;
    [SerializeField] private float _sliderSpeed;
    [SerializeField] private GameObject _nextButton;

    private void OnEnable()
    {
        _proggresSlider.value = WeaponController._weaponController.GetBulletProggres();
        StartCoroutine(IncrementProggres());
    }

    private IEnumerator IncrementProggres()
    {
        int target = (int)_proggresSlider.value + _levelProggresStep;
        float step = (0.02f * _levelProggresStep) / _sliderSpeed;
        if (target >= 100)
        {
            WeaponController._weaponController.OpenNextWeapon();
            WeaponController._weaponController.SetBulletProggres(0);
        }
        else
        {
            WeaponController._weaponController.SetBulletProggres(target);
        }

        while (_proggresSlider.value <= target )
        {
            _proggresSlider.value += step;
            yield return new WaitForFixedUpdate();
            if(_proggresSlider.value == _proggresSlider.maxValue)
            {
                break;
            }
        }
      
        
        _nextButton.SetActive(true);
    }
}
