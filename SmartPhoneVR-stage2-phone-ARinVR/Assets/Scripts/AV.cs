using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.UI.AspectRatioFitter;

using BasicConfigs;
using System;

public class AV : MonoBehaviour
{
    static WebCamTexture cam;
    public GameObject rawImage;


    public GUIStyle fontStyle = new GUIStyle();




    private void Start()
    {
     //   Screen.orientation = ScreenOrientation.LandscapeLeft;


        if (cam == null)
        {
            cam = new WebCamTexture();
        }

        rawImage.GetComponent<RawImage>().material.mainTexture = cam;
        if (!cam.isPlaying)
            cam.Play();
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnClickBack();
        }
    }
   private void OnClickBack()
    {
        BackgroundCommunication.instance.m_currentScene = Scenes.MAIN_MENU;

        SceneManager.LoadScene("Scene_mainMenu");
        Destroy(gameObject);
    }


    //private void OnGUI()
    //{    
    //    fontStyle.alignment = TextAnchor.MiddleCenter;
    //        fontStyle.fontSize = (int) (Screen.width / 20);


    //    int m_buttonWidth =(int) (Screen.width * 0.15f);
    //    int m_buttonHeight =(int)( Screen.height * 0.15f);


    //    if (GUI.Button(new Rect(Screen.width/2 - m_buttonWidth/2, Screen.height - m_buttonHeight, m_buttonWidth, m_buttonHeight), "Back", fontStyle))
    //    {

    //        OnClickBack();


    //    }
    //}

 
}