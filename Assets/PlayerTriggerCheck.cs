using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTriggerCheck : MonoBehaviour
{
    public Text infoText;
    private string infoOriginalText;
    public GameObject infoPanel;
    public GameObject buttonHint;
    private PlayerMovement playerMovement;
    public GameObject newsPaper;
    public GameObject gunButton;
    private GameManager gameManager;
    public GameObject shootButton;
    public GameObject slowMo;
    public GameObject handPos;
    public GameObject redSlider;
    // Start is called before the first frame update
    void Start()
    {
        playerMovement = transform.parent.GetComponent<PlayerMovement>();
        gameManager=FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            infoPanel.SetActive(!infoPanel.activeSelf);
        }
        if (buttonHint.activeSelf && Input.GetKeyDown(KeyCode.F))
        {
            playerMovement.SitOnBench();
            buttonHint.SetActive(false);
            newsPaper.SetActive(true);
            infoPanel.SetActive(true);
            gunButton.SetActive(true);
            infoText.text = "The mayor is an old man in black coat, gray trousers and gray shirt. I shouldn't gaze at them. I shall kill the target once I recognize him.";
        }
        if(gunButton.activeSelf&&Input.GetKeyDown(KeyCode.Q))
        {

            handPos.SetActive(true);
            gameManager.EnemySuspect();
            slowMo.SetActive(true);
            shootButton.SetActive(true);
            gunButton.SetActive(false);
            newsPaper.GetComponent<Rigidbody>().isKinematic=false;
            playerMovement.DrawGun();
            Time.timeScale=0.4f;
            
        }
        if(shootButton.activeSelf&&Input.GetKeyDown(KeyCode.S)&&gameManager.gameStarted)
        {
            playerMovement.Shoot();
            Time.timeScale=1;

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PoliceTrigger"))
        {
            infoPanel.SetActive(true);
            infoOriginalText = infoText.text;
            infoText.text = "There are police ahead. I should better go the other way.";
        }
        else if (other.CompareTag("FoundTargetTrigger"))
        {
            infoPanel.SetActive(true);
            infoOriginalText = infoText.text;
            infoText.text = "I've found the target group. But I should't look at them, or else they would get suspicious.I shall sit down on the bench.";
            redSlider.SetActive(true);
        }
        else if (other.CompareTag("FoundBench"))
        {
            buttonHint.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PoliceTrigger") || other.CompareTag("FoundTargetTrigger"))
        {
            infoPanel.SetActive(false);
            infoText.text = infoOriginalText;
            //infoText.text = "There are police ahead. I should better go the other way.";
        }
        else if (other.CompareTag("FoundBench"))
        {
            buttonHint.SetActive(false);

        }
    }

}
