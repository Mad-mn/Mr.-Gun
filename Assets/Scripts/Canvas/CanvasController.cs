using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    public static CanvasController _canvasController;

    [SerializeField] private GameObject _mainmenuPanel, _gamePanel;
    [SerializeField] private GameObject _addCoinsPanel;
    [SerializeField] private ScorePanel _scorePanel;
    [SerializeField] private GameObject _addScorePanel;
    [SerializeField] private GameObject _bossHpPanel;
    [SerializeField] private GameObject _losePanel;
    [SerializeField] private GameObject _levelProggressBar;
    [SerializeField] private GameObject _weaponProggresPanel;

    private BossHpBar _bossHpBar;
    private LevelProggressBar _lPB;
    

    private void Awake()
    {
        if(_canvasController == null)
        {
            _canvasController = this;
        }
    }

    private void Start()
    {
        MainController.OnStartGame.AddListener(StartGame);
        MainController.OnLoseGame.AddListener(EnebleLosePanel);
        _bossHpBar = _bossHpPanel.GetComponent<BossHpBar>();
        _lPB = _levelProggressBar.GetComponent<LevelProggressBar>();
    }

    public void StartGame()
    {
        _mainmenuPanel.SetActive(false);
        _gamePanel.SetActive(true);
    }

    public void AddCoins(int count)
    {
        _addCoinsPanel.SetActive(true);
        _addCoinsPanel.GetComponent<AddCoinPanel>().SetCointTxt(count);
        PlayerInfo._playerInfo.AddCoins(count);
    }

    public void SetScore(int score, int howManyScore)
    {
        _addScorePanel.SetActive(true);
        _scorePanel.SetScore(score);
        _addScorePanel.GetComponent<AddScorePanel>().SetScoreTxt(howManyScore);
    }

    public void SetBossInfo(float hp, string name)
    {
        _bossHpPanel.SetActive(true);
        _bossHpBar.SetBossHp(hp);
        _bossHpBar.SetBossName(name);
    }

    public void ChangeBossHp(float damage)
    {
        _bossHpBar.DecrementBossHp(damage);
    }

    public void EnebleLosePanel()
    {
        _levelProggressBar.SetActive(false);
        _losePanel.SetActive(true);
    }

    public void FillLevelProggress()
    {
        
        _lPB.Fill();
        if(GameSessionController._sessionController.LastSingleEnemy == 0)
        {
            _levelProggressBar.SetActive(false);
        }
    }

    public void OpenWeaponProggresPanel()
    {
        _weaponProggresPanel.SetActive(true);
    }
}
