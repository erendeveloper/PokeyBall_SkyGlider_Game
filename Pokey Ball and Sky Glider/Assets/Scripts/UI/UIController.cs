using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Added on Main Camera
public class UIController : MonoBehaviour
{
    public GameObject gameOverMenu;

    public Text fpsValueText;

    private void Update()
    {
        showFps();
    }
    public void OpenGameOver()
    {
        gameOverMenu.SetActive(true);
    }
    private void showFps()
    {
        fpsValueText.text = (1 / Time.deltaTime).ToString();
    }
}
