using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Added on Main Camera
public class GameManager : MonoBehaviour
{
    //Access other script
    private UIController UIControllerScript;

    private void Start()
    {
        Application.targetFrameRate = 300;
        UIControllerScript = this.GetComponent<UIController>();
    }
    public void GameOver()
    {
        UIControllerScript.OpenGameOver();
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

}
