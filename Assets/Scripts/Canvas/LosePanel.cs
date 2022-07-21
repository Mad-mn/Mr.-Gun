using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LosePanel : MonoBehaviour
{
    [SerializeField] private Text _scoreTextField, _levelTextField;

    private void Start()
    {
        _levelTextField.text = "LEVEL " + LevelController._levelController.GetLevelId().ToString();
        _scoreTextField.text = PlayerInfo._playerInfo.BestScore.ToString();
    }
}
