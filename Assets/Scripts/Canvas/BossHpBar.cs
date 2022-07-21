using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHpBar : MonoBehaviour
{
    [SerializeField] private Text _bossNameTextField;
    [SerializeField] private Image _hpBarImg;

    private float _bossHp;

    public void SetBossHp(float hp)
    {
        _bossHp = hp;
    }
    
    public void SetBossName(string name)
    {
        _bossNameTextField.text = name;
    }

    public void DecrementBossHp(float damage)
    {
        
        float damageValue =(float) (damage / _bossHp);
       
        _hpBarImg.fillAmount -= damageValue;
    }
}
