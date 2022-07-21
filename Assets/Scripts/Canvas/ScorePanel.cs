using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScorePanel : MonoBehaviour
{
    [SerializeField] private Text _textfield;

    private void OnEnable()
    {
        SetScore(0);
    }

    public void SetScore(int score)
    {
        _textfield.text = "Score: "+ score;
    }
}
