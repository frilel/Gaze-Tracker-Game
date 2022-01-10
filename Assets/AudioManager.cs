using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource backgroundMusic;
    public AudioSource infoPop;
    public AudioSource newspaper;
    public AudioSource park;
    public AudioSource shoot;
    public AudioSource walking;
    public AudioSource watchout;
    public AudioSource lockAudio;
    public AudioSource drawGun;
    public AudioSource victory;
    public AudioSource failed;
    public AudioSource slowMotion;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SlowMoAudio()
    {
        /*foreach(AudioSource audioSource in GetComponents<AudioSource>()) 
        {
            audioSource.pitch=Time.timeScale;
        }*/
        if (Time.timeScale != 1)
        {
            foreach (AudioSource audioSource in GetComponents<AudioSource>())
            {
                if (audioSource != slowMotion&&audioSource!=shoot)
                {
                    audioSource.pitch = 0.85f;
                    audioSource.volume=1;
                }

            }
        }
        else
        {
            foreach (AudioSource audioSource in GetComponents<AudioSource>())
            {

                audioSource.pitch = 1f;
            }
        }
    }
}
