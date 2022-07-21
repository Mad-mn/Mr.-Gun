using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinsPanel : MonoBehaviour
{
    [SerializeField] private Text _textField;

    private void Update()
    {
        _textField.text = PlayerInfo._playerInfo.GetCointsCount().ToString();
    }
}
