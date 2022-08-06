using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelButton : MonoBehaviour
{
    public void OpenNextLevel()
    {
        if(SceneManager.sceneCountInBuildSettings > SceneManager.GetActiveScene().buildIndex)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
