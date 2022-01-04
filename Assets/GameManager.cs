using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public bool gameStarted = true;
    public GameObject gameOverPanel;
    public GameObject startGamePanel;
    public Text gameOverText;
    public Text gameOverTitle;
    public GameObject deathVolume;
    public NPC_Controller[] enemiesAI;
    public Slider redSlider;
    private bool enemeySuspected = false;
    public GameObject slowMoVolume;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void EnemySuspect()
    {
        if (!enemeySuspected)
        {
            enemiesAI[0].SwitchAnimSuspect(0);
            enemiesAI[1].SwitchAnimSuspect(0);
            enemiesAI[2].SwitchAnimSuspect(1);
            enemeySuspected = true;
        }

    }

    // Update is called once per frame
    void Update()
    {
        MenuControll();
        if (redSlider.value >= 1)
        {
            EnemySuspect();
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
        }
    }
    public void StartGame()
    {
        gameStarted = true;
    }
    public IEnumerator GameOver(int result)//0 wrong, 1 right, 2 spotted
    {
        enemiesAI[2].GetComponentInChildren<Animator>().speed = 0;
        gameStarted = false;
        slowMoVolume.SetActive(false);
        yield return new WaitForSeconds(1);
        gameOverPanel.SetActive(true);

        if (result == 0)
        {
            gameOverTitle.text = "Mission Failed!";
            gameOverText.text = "You killed the wrong guy!";
        }
        else if (result == 1)
        {
            gameOverTitle.text = "Mission Success!";
            gameOverText.text = "Target Eliminated!";
        }
        else
        {
            deathVolume.SetActive(true);
            gameOverTitle.text = "Mission Failed!";
            gameOverText.text = "You have been spotted and got shot!";
        }
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void Retry()
    {
        Time.timeScale=1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private void MenuControll()
    {
        if (startGamePanel.activeSelf && Input.GetKeyDown(KeyCode.S))
        {
            startGamePanel.SetActive(false);
            StartGame();
        }
        if (gameOverPanel.activeSelf && Input.GetKeyDown(KeyCode.R))
        {
            Retry();
        }
        if (gameOverPanel.activeSelf && Input.GetKeyDown(KeyCode.Q))
        {
            QuitGame();
        }
    }
}