using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkControll : MonoBehaviour
{
    // Start is called before the first frame update
    private GameManager gameManager;
    private void Awake() {
        gameManager=FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (gameManager.gameStarted)
        {
            transform.position = new Vector3(-53.14f, 10.86f+(float)Math.Sin(Time.time*2.5f)*0.4f, -49.81f);
            transform.Rotate(Vector3.up, 50f*Time.fixedDeltaTime,Space.World);

        }


    }
}
