using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.Gaming;
using UnityEngine.UI;

public class GazeAwareTest : MonoBehaviour
{
    public GazeAware gazeAware;
    public Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (gazeAware.HasGazeFocus)
			{
				Debug.Log("dd");
                slider.value+=0.01f;
			}
        
    }
}
