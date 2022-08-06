using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenLevel : MonoBehaviour
{
    [SerializeField] private int _levelCount;

    public void Open()
    {
        SceneManager.LoadScene(_levelCount);
    }
}
