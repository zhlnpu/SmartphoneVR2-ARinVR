using ServerCommunication;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Test_TCPIP_App : MonoBehaviour
{
    public TCPIP_Base m_TCPIP_Base;



    // Start is called before the first frame update
    void Start()
    {
        if (m_TCPIP_Base == null)
        {
            try
            {
                m_TCPIP_Base = GetComponent<TCPIP_Base>();
            }
            catch (System.Exception)
            {
                Debug.LogError("No baes tcpip class");
                ExitApp();
                return;
            }
        }
        if (m_TCPIP_Base == null)
        { 
            Debug.LogError("PLease set base tcpip class");
            ExitApp();
            return;
        }

        m_TCPIP_Base.OnProcessingData += ProcessingData;


    }

    // Update is called once per frame
    void Update()
    {

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

    void ExitApp()
    {
        Debug.LogError("Error Exit!");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif

    }

}
