using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ServerCommunication;


public class ContentTransfer : MonoBehaviour
{
    public GameObject[] m_images;
    GameObject m_currentImage;

    public TCPIP_Base m_TCPIP_Base;

    public GameObject obj_phone;



    void Awake()
    {

    }

    void Start()
    {
       // Instantiate(m_images[0], obj_phone.transform.position, obj_phone.transform.rotation).SetActive(true);


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

    void Update()
    {

        

    }
    public float fps = 0;

    public void ProcessingData(DATA_NAME dataName, byte[] data)
    {
        switch (dataName)
        {
            //add other processings here!
            // case DATA_NAME.DT_SPATIAL_INPUT_POINT:
            //      Data_Position3f touchpoint = new Data_Position3f(data);


            case DATA_NAME.DT_MODEL_NUMBER:
                Debug.Log("DT_MODEL_NUMBER");

                Data_ModelNumber modelNumber = new Data_ModelNumber(data);


                Instantiate(m_images[modelNumber.value], obj_phone.transform.position, obj_phone.transform.rotation).SetActive(true);



                break;     
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

    private Vector3 GetRelativePosition(Transform origin, Vector3 position)
    {
        Vector3 distance = position - origin.position;
        Vector3 relativePosition = Vector3.zero;
        relativePosition.x = Vector3.Dot(distance, origin.right.normalized);
        relativePosition.y = Vector3.Dot(distance, origin.up.normalized);
        relativePosition.z = Vector3.Dot(distance, origin.forward.normalized);
        return relativePosition;
    }


}