using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSessionController : MonoBehaviour
{
    public static GameSessionController _sessionController;

    [SerializeField] private int _bodyScoreHit, _headScoreHit;

    public int Score { get; private set; }

    private void Awake()
    {
        if(_sessionController == null)
        {
            _sessionController = this;
        }
    }

    private void Start()
    {
        MainController.OnLoseGame.AddListener(CheckBestScore);
    }

    public void ShotOnEnemy(bool isHead)
    {
        int score;
        if (isHead)
        {
            Score += _headScoreHit;
            score = _headScoreHit;
        }
        else
        {
            Score += _bodyScoreHit;
            score = _bodyScoreHit;
        }
        CanvasController._canvasController.SetScore(Score, score);
    }

    public void CheckBestScore()
    {
        if(Score > PlayerInfo._playerInfo.BestScore)
        {
            PlayerInfo._playerInfo.BestScore = Score;
        }
    }

}
