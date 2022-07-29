using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelProggressBar : MonoBehaviour
{
    [SerializeField] private Image _fillImage;

    private float _levelStep;

    private void Start()
    {
        _levelStep = 1f /(3f + (float)LevelController._levelController.GetLevelId());
        _fillImage.fillAmount = 0;
    }

    public void Fill()
    {
        if(_fillImage.fillAmount < 1)
        {
            _fillImage.fillAmount += _levelStep;
        }
        if(_fillImage.fillAmount == 1)
        {
            gameObject.SetActive(false);
        }
    }
}
