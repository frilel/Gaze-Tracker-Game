using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public bool gameStarted=true;
    public GameObject gameOverPanel;
    public Text gameOverText;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState=CursorLockMode.None;
        Cursor.visible=true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartGame()
    {
        gameStarted=true;
    }
    public void GameOver(int result)//0 wrong, 1 right
    {
        gameOverPanel.SetActive(true);
        gameStarted=false;
        if(result==0)
        {
            gameOverText.text="You killed the wrong guy!";
        }
        else
        {
            gameOverText.text="You killed the right guy, mission complete";
        }
        

    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
