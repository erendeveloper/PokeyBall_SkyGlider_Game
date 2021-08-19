using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Added on Main Camera
public class GameManager : MonoBehaviour
{
    //Access other script
    private UIController uiControllerScript;

    private void Start()
    {
        Application.targetFrameRate = 300;
        uiControllerScript = this.GetComponent<UIController>();
    }
    public void GameOver()
    {
        uiControllerScript.OpenGameOver();
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

}
