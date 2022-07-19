using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    public static CanvasController _canvasController;

    [SerializeField] private GameObject _mainmenuPanel, _gamePanel;

    private void Start()
    {
        MainController.OnStartGame.AddListener(StartGame);
    }

    public void StartGame()
    {
        _mainmenuPanel.SetActive(false);
        _gamePanel.SetActive(true);
    }
}
