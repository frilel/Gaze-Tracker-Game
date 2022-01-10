using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    private PlayerMovement playerMovement;
    public GameObject slowMo;
    private AudioManager audioManager;
    // Start is called before the first frame update
    void Start()
    {
        playerMovement=FindObjectOfType<PlayerMovement>();
        audioManager=FindObjectOfType<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ShootPlayer()
    {
        playerMovement.GetShot();
        slowMo.SetActive(false);
        Time.timeScale=1;
        audioManager.SlowMoAudio();
        if(!audioManager.shoot.isPlaying)
        {
            audioManager.shoot.Play();
        }
    }
}
