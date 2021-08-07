using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BasicConfigs;
using UnityEngine.SceneManagement;
using ServerCommunication;

public class SpatialInputAndroid : MonoBehaviour
{
    Data_Position3f m_data_3f = new Data_Position3f(CLIENT_NAME.CN_ANDROID_1, DATA_NAME.DT_SPATIAL_INPUT_POINT);

    Data_Position3f m_data_3f_start = new Data_Position3f(CLIENT_NAME.CN_ANDROID_1, DATA_NAME.DT_SPATIAL_INPUT_POINT_START);

    Data_Position3f m_data_3f_end = new Data_Position3f(CLIENT_NAME.CN_ANDROID_1, DATA_NAME.DT_SPATIAL_INPUT_POINT_END);

    //public   BackgroundCommunication m_TCPIP_Base;
    private bool m_waitForReply=false;



   // int startNum = 0;
   // int endnum = 0;

   //public  GUIStyle style;


    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;

        BackgroundCommunication.instance  .OnProcessingData += ProcessingData;

    }

    // Update is called once per frame
    void Update()
    {


        //int = 0时，获取鼠标左键
        //int = 1时，获取鼠标右键
        //int = 2时，获取鼠标中键
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("GetMouseButtonDown");
            BackgroundCommunication.instance.SendToServer(m_data_3f_start.ToByteArray());
        }
        else if (Input.GetMouseButton(0))
        {
            //  Debug.Log("x: " + Input.mousePosition.x.ToString() + " y: " + Input.mousePosition.y.ToString() + " z: " + Input.mousePosition.z.ToString());
            m_data_3f.SetValue(Input.mousePosition);
            BackgroundCommunication.instance.SendToServer(m_data_3f.ToByteArray());
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("GetMouseButtonUp");
            BackgroundCommunication.instance.SendToServer(m_data_3f_end.ToByteArray());

            m_waitForReply = true;
        }






        //if (Input.touchCount == 1)
        //{
        //    if (Input.touches[0].phase == TouchPhase.Began)
        //    {
        //        BackgroundCommunication.instance.SendToServer(m_data_3f_start.ToByteArray());
        //      //  startNum++;
        //    }
                       
        //    if (Input.touches[0].phase == TouchPhase.Moved)
        //    {

        //        float s01 = Input.GetAxis("Mouse X");    //手指水平移动的距离
        //        float s02 = Input.GetAxis("Mouse Y");    //手指垂直移动的距离
        //        m_data_3f.SetValue(s01, 0, s02);
        //        BackgroundCommunication.instance.SendToServer(m_data_3f.ToByteArray());

        //    }

        //    if (Input.touches[0].phase == TouchPhase.Ended)
        //    {
        //        //  BackgroundCommunication.instance.SendToServer(m_data_3f_end.ToByteArray());
        //        //endnum++;

        //        m_waitForReply = true;

        //    }

        //}


        if (m_waitForReply)
        {
            BackgroundCommunication.instance.SendToServer(m_data_3f_end.ToByteArray());
        }




        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnClickBack();
        }
    }

    public void ProcessingData(DATA_NAME dataName, byte[] data)
    {
        switch (dataName)
        {
            case DATA_NAME.DT_SPATIAL_INPUT_POINT_START:
                Debug.Log("DT_SPATIAL_INPUT_POINT_START");

                break;

            case DATA_NAME.DT_SPATIAL_INPUT_POINT_END:

           //     Debug.Log("DT_SPATIAL_INPUT_POINT_END");
           

                    m_waitForReply = false;

                
                break;


            case DATA_NAME.DT_SPATIAL_INPUT_POINT:

                break;



            default:
                Debug.LogError("Unknown datatype");
                break;
        }
    }


    //private void OnGUI()
    //{
    //    GUI.Label(new Rect(300, 300, 200, 200), startNum.ToString(), style);

    //    GUI.Label(new Rect(300, 600, 200, 200), startNum.ToString(), style);

    //}



    private void OnClickBack()
    {
        BackgroundCommunication.instance.m_currentScene = Scenes.MAIN_MENU;
        SceneManager.LoadScene("Scene_mainMenu");
        Destroy(gameObject);
    }
}
