using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BasicConfigs;
using UnityEngine.SceneManagement;
using ServerCommunication;

public class Scene2 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;

    }

    // Update is called once per frame
    void Update()
    {
        //int = 0时，获取鼠标左键
        //int = 1时，获取鼠标右键
        //int = 2时，获取鼠标中键
        if (Input.GetMouseButtonDown(0))
        {
        //    Debug.Log("GetMouseButtonDown");
            BackgroundCommunication.instance.SendToServer(m_data_3f_start.ToByteArray());

        }
        else

        if (Input.GetMouseButton(0))
        {
          //  Debug.Log("x: " + Input.mousePosition.x.ToString() + " y: " + Input.mousePosition.y.ToString() + " z: " + Input.mousePosition.z.ToString());

            m_data_3f.SetValue(Input.mousePosition);

            BackgroundCommunication.instance.SendToServer(m_data_3f.ToByteArray());

        }
        else
        if (Input.GetMouseButtonUp(0))
        {
        //    Debug.Log("GetMouseButtonUp");
            BackgroundCommunication.instance.SendToServer(m_data_3f_end.ToByteArray());

        }


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnClickBack();
        }


    }
    //dewe
    private void OnClickBack()
    {
        //BackgroundCommunication.instance.m_currentScene = Scenes.MAIN_MENU;
        // SceneManager.LoadScene("Scene_mainMenu");
        // Destroy(gameObject);

        AppExit();

    }


    public GUIStyle style;

    Data_Position3f m_data_3f = new Data_Position3f(CLIENT_NAME.CN_ANDROID_1, DATA_NAME.DT_SPATIAL_INPUT_POINT);
    
    Data_Position3f m_data_3f_start = new Data_Position3f(CLIENT_NAME.CN_ANDROID_1, DATA_NAME.DT_SPATIAL_INPUT_POINT_START);

    Data_Position3f m_data_3f_end = new Data_Position3f(CLIENT_NAME.CN_ANDROID_1, DATA_NAME.DT_SPATIAL_INPUT_POINT_END);


    void AppExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif        
    }

}
