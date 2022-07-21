using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddCoinPanel : MonoBehaviour
{
    [SerializeField] private Text _textField;

    public void OffPanel()
    {
        gameObject.SetActive(false);
    }

    public void SetCointTxt(int count)
    {
        _textField.text = "+" + count;
    }
}
