using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelCountTxt : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Text>().text = "LEVEL " + LevelController._levelController.GetLevelId();
    }
}
