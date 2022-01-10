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
    private AudioManager audioManager;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        audioManager = FindObjectOfType<AudioManager>();
    }
    public void EnemySuspect()
    {
        if (!enemeySuspected)
        {
            enemiesAI[0].SwitchAnimSuspect(0);
            enemiesAI[1].SwitchAnimSuspect(0);
            enemiesAI[2].SwitchAnimSuspect(1);
            enemeySuspected = true;
            if (!audioManager.watchout.isPlaying)
            {
                audioManager.watchout.Play();
            }
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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
        }
        if (SceneManager.GetActiveScene().buildIndex == 0 && Input.GetKeyDown(KeyCode.Q))
        {
            SceneManager.LoadScene(1);
        }
    }
    public void StartGame()
    {
        gameStarted = true;
    }
    public IEnumerator GameOver(int result)//0 wrong, 1 right, 2 spotted
    {
        if (result == 1)
        {
            enemiesAI[2].GetComponentInChildren<Animator>().speed = 0;
        }

        gameStarted = false;
        slowMoVolume.SetActive(false);
        yield return new WaitForSeconds(1);
        gameOverPanel.SetActive(true);

        if (result == 0)
        {
            gameOverTitle.text = "Mission Failed!";
            gameOverText.text = "You killed the wrong guy!";
            if (!audioManager.failed.isPlaying)
            {
                audioManager.failed.Play();
            }
        }
        else if (result == 1)
        {
            gameOverTitle.text = "Mission Success!";
            gameOverText.text = "Target Eliminated!";
            if (!audioManager.victory.isPlaying)
            {
                audioManager.victory.Play();
            }
        }
        else
        {
            deathVolume.SetActive(true);
            gameOverTitle.text = "Mission Failed!";
            gameOverText.text = "You have been spotted and got shot!";
            if (!audioManager.failed.isPlaying)
            {
                audioManager.failed.Play();
            }
        }
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void Retry()
    {
        Time.timeScale = 1;
        audioManager.SlowMoAudio();
        if (SceneManager.GetActiveScene().buildIndex != 3)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            SceneManager.LoadScene(1);
        }

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