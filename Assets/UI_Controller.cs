using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Tobii.Gaming;
public class UI_Controller : MonoBehaviour
{
    public Image crossHair;
    private float gazeX=0;
    private float gazeY=0;
    private Vector2 gazeData;
    private Vector2 lastGazeData;
    private float interpolation;
    private Camera mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera=Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCrossHair();
        CrossHairFixOnTarget();
    }
    void UpdateCrossHair()
    {
        gazeData=TobiiAPI.GetGazePoint().Screen;
        interpolation=(Mathf.Pow((Mathf.Min(Vector2.Distance(gazeData,lastGazeData),Screen.height)/Screen.height),0.5f))*0.6f;        
        //gazeX=gazeData.x
        //gazeX=Mathf.Lerp(gazeX,gazeData.x-Screen.width/2,0.5f);
        //gazeY=Mathf.Lerp(gazeY,gazeData.y-Screen.height/2,0.5f);
        gazeX=Mathf.Lerp(gazeX,gazeData.x,interpolation);
        gazeY=Mathf.Lerp(gazeY,gazeData.y,interpolation);
        crossHair.GetComponent<RectTransform>().position=new Vector2(gazeX,gazeY);
        lastGazeData=gazeData;
    }
    void CrossHairFixOnTarget()
    {
        if(TobiiAPI.GetFocusedObject())
        {
            crossHair.GetComponent<RectTransform>().position=mainCamera.WorldToScreenPoint(TobiiAPI.GetFocusedObject().transform.GetChild(1).position);
            crossHair.GetComponent<Image>().color=Color.red;
        }
        else
        {
            crossHair.GetComponent<Image>().color=Color.white;
        }
    }
}
