using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using System.Net.Sockets;
using System.IO;
using System;

using ServerCommunication;
using BasicConfigs;

public class LoadScene : MonoBehaviour
{
    public GUIStyle fontStyle = new GUIStyle();


    // Start is called before the first frame update
    void Start()
    {
        BackgroundCommunication.instance.m_currentScene = Scenes.MAIN_MENU;
        Screen.orientation = ScreenOrientation.AutoRotation;

        //  m_TCPIP_Base.OnProcessingData += ProcessingData;

    }

    public void ProcessingData(DATA_NAME dataName, byte[] data)
    {
        switch (dataName)
        {
            //add other processings here!
            // case DATA_NAME.DT_SPATIAL_INPUT_POINT:
            //      Data_Position3f touchpoint = new Data_Position3f(data);






            default:
                Debug.LogError("Unknown datatype");
                break;
        }
    }




    private void OnGUI()
    {

        fontStyle.alignment = TextAnchor.MiddleCenter;


        if (Screen.width < Screen.height)
        {
            fontStyle.fontSize = (int)(Screen.width / 20);

            int m_buttonWidth = (int)(Screen.width * 0.8f);
            int m_buttonHeight = (int)(Screen.height * 0.06f);
            int m_buttonLeft = (int)(Screen.width * 0.1f);
            int m_buttonTop = (int)(Screen.height * 0.08f);
            int m_buttonOffset = (int)(Screen.height * 0.01f);

            if (GUI.Button(new Rect(m_buttonLeft, m_buttonTop + 0 * (m_buttonOffset + m_buttonHeight), m_buttonWidth, m_buttonHeight), "Asymmetric bimanual operation", fontStyle))
            {
                OnClick_AsymetricOperation();
            }

            if (GUI.Button(new Rect(m_buttonLeft, m_buttonTop + 1 * (m_buttonOffset + m_buttonHeight), m_buttonWidth, m_buttonHeight), "Progressive operation", fontStyle))
            {
                OnClick_ProgressiveOperation();
            }
            if (GUI.Button(new Rect(m_buttonLeft, m_buttonTop + 2 * (m_buttonOffset + m_buttonHeight), m_buttonWidth, m_buttonHeight), "Augmented Virtuality", fontStyle))
            {
                OnClick_AugmentedVirtuality();
            }
            if (GUI.Button(new Rect(m_buttonLeft, m_buttonTop + 3 * (m_buttonOffset + m_buttonHeight), m_buttonWidth, m_buttonHeight), "Second virtual perspective", fontStyle))
            {
                OnClick_SecondPerpestive();
            }
            if (GUI.Button(new Rect(m_buttonLeft, m_buttonTop + 4 * (m_buttonOffset + m_buttonHeight), m_buttonWidth, m_buttonHeight), "Tangible spatial Input", fontStyle))
            {
                OnClick_SpatialInput();
            }
            if (GUI.Button(new Rect(m_buttonLeft, m_buttonTop + 5 * (m_buttonOffset + m_buttonHeight), m_buttonWidth, m_buttonHeight), "Tangible virtual buttons", fontStyle))
            {
                OnClick_VirtualButtons();
            }
            if (GUI.Button(new Rect(m_buttonLeft, m_buttonTop + 6 * (m_buttonOffset + m_buttonHeight), m_buttonWidth, m_buttonHeight), "Onscreen tangible input", fontStyle))
            {
                OnClick_OnscreenInput();
            }
            if (GUI.Button(new Rect(m_buttonLeft, m_buttonTop + 7 * (m_buttonOffset + m_buttonHeight), m_buttonWidth, m_buttonHeight), "Surface gestural input", fontStyle))
            {
                OnClick_GesturalInput();
            }
            if (GUI.Button(new Rect(m_buttonLeft, m_buttonTop + 8 * (m_buttonOffset + m_buttonHeight), m_buttonWidth, m_buttonHeight), "2D-3D content transfer", fontStyle))
            {
                OnClick_ContentTransfer();
            }
            if (GUI.Button(new Rect(m_buttonLeft, m_buttonTop + 9 * (m_buttonOffset + m_buttonHeight), m_buttonWidth, m_buttonHeight), "VR smartphone", fontStyle))
            {
                OnClick_VRphone();
            }
            if (GUI.Button(new Rect(m_buttonLeft, m_buttonTop + 10 * (m_buttonOffset + m_buttonHeight), m_buttonWidth, m_buttonHeight), "Calibration", fontStyle))
            {
                OnClick_Calibration();
            }
            if (GUI.Button(new Rect(m_buttonLeft, m_buttonTop + 11 * (m_buttonOffset + m_buttonHeight), m_buttonWidth, m_buttonHeight), "Exit", fontStyle))
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            }
        }

        else
        {
            fontStyle.fontSize = (int)(Screen.height / 20);

            int m_buttonWidth = (int)(Screen.width * 0.38f);
            int m_buttonHeight = (int)(Screen.height * 0.12f);
            int m_buttonLeft = (int)(Screen.width * 0.08f);
            int m_buttonTop = (int)(Screen.height * 0.04f);

            if (GUI.Button(GetRect(m_buttonWidth, m_buttonHeight, m_buttonLeft, m_buttonTop, 0), "Asymmetric bimanual operation", fontStyle))
            {
                OnClick_AsymetricOperation();
            }
            if (GUI.Button(GetRect(m_buttonWidth, m_buttonHeight, m_buttonLeft, m_buttonTop, 1), "Progressive operation", fontStyle))
            {
                OnClick_ProgressiveOperation();
            }
            if (GUI.Button(GetRect(m_buttonWidth, m_buttonHeight, m_buttonLeft, m_buttonTop, 2), "Augmented Virtuality", fontStyle))
            {
                OnClick_AugmentedVirtuality();
            }
            if (GUI.Button(GetRect(m_buttonWidth, m_buttonHeight, m_buttonLeft, m_buttonTop, 3), "Second virtual perspective", fontStyle))
            {
                OnClick_SecondPerpestive();
            }
            if (GUI.Button(GetRect(m_buttonWidth, m_buttonHeight, m_buttonLeft, m_buttonTop, 4), "Tangible spatial Input", fontStyle))
            {
                OnClick_SpatialInput();
            }
            if (GUI.Button(GetRect(m_buttonWidth, m_buttonHeight, m_buttonLeft, m_buttonTop, 5), "Tangible virtual buttons", fontStyle))
            {
                OnClick_VirtualButtons();
            }
            if (GUI.Button(GetRect(m_buttonWidth, m_buttonHeight, m_buttonLeft, m_buttonTop, 6), "Onscreen tangible input", fontStyle))
            {
                OnClick_OnscreenInput();
            }
            if (GUI.Button(GetRect(m_buttonWidth, m_buttonHeight, m_buttonLeft, m_buttonTop, 7), "Surface gestural input", fontStyle))
            {
                OnClick_GesturalInput();
            }
            if (GUI.Button(GetRect(m_buttonWidth, m_buttonHeight, m_buttonLeft, m_buttonTop, 8), "2D-3D content transfer", fontStyle))
            {
                OnClick_ContentTransfer();
            }
            if (GUI.Button(GetRect(m_buttonWidth, m_buttonHeight, m_buttonLeft, m_buttonTop, 9), "VR smartphone", fontStyle))
            {
                OnClick_VRphone();
            }
            if (GUI.Button(GetRect(m_buttonWidth, m_buttonHeight, m_buttonLeft, m_buttonTop, 10), "Calibration", fontStyle))
            {
                OnClick_Calibration();
            }
            if (GUI.Button(GetRect(m_buttonWidth, m_buttonHeight, m_buttonLeft, m_buttonTop, 11), "Exit", fontStyle))
            {
                AppExit();
            }
        }
    }

    private void OnClick_ProgressiveOperation()
    {
        SceneManager.LoadScene("Scene_ProgressiveOperation");
        BackgroundCommunication.instance.m_currentScene = Scenes.PROGRESSIVE_OPERATION;
    }

    private void OnClick_Calibration()
    {
        throw new NotImplementedException();
    }

    private void OnClick_VRphone()
    {
        AppExit();

    }

    private void OnClick_ContentTransfer()
    {
        SceneManager.LoadScene("Scene_ContentTransfer");
        BackgroundCommunication.instance.m_currentScene = Scenes.SECOND_PERPESTIVE;
    }

    private void OnClick_GesturalInput()
    {
        throw new NotImplementedException();
    }

    private void OnClick_OnscreenInput()
    {
        SceneManager.LoadScene("Scene_OnscreenInput");
        BackgroundCommunication.instance.m_currentScene = Scenes.SECOND_PERPESTIVE;
    }

    private void OnClick_VirtualButtons()
    {
        SceneManager.LoadScene("Scene_VirtualButton");
        BackgroundCommunication.instance.m_currentScene = Scenes.VIRTUAL_BUTTONS;
    }

    private void OnClick_SecondPerpestive()
    {
        SceneManager.LoadScene("Scene_SecondPerpestive");
        BackgroundCommunication.instance.m_currentScene = Scenes.SECOND_PERPESTIVE;
    }

    public void OnClick_AugmentedVirtuality()
    {
        SceneManager.LoadScene("Scene_AugmentedVirtuality");
        BackgroundCommunication.instance.m_currentScene = Scenes.AUGMENTED_VIRTUALITY;
    }

    public void OnClick_SpatialInput()
    {
        SceneManager.LoadScene("Scene_SpatialInput");
        BackgroundCommunication.instance.m_currentScene = Scenes.SPATIAL_INPUT;
    }

    public void OnClick_AsymetricOperation()
    {
        SceneManager.LoadScene("Scene_AsymetricOperation");
        BackgroundCommunication.instance.m_currentScene = Scenes.ASYMETRIC_OPERATION;
    }



    Rect GetRect(int m_buttonWidth, int m_buttonHeight, int m_buttonLeft, int m_buttonTop, int i)
    {
        return new Rect(m_buttonLeft + (i % 2) * (m_buttonLeft + m_buttonWidth), m_buttonTop + (i / 2) * (m_buttonTop + m_buttonHeight), m_buttonWidth, m_buttonHeight);
    }
    void AppExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif        
    }
}
