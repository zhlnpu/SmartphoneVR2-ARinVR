using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ServerCommunication;
using System;

public class ARinVR : MonoBehaviour
{
    //  public GameObject[] m_images;
    //  GameObject m_currentImage;

    public BackgroundCommunication m_TCPIP_Base;

    //  public GameObject obj_phone;

    [Header("Button remapping Settings")]
    public GameObject lRStartPoint;

    LineRenderer lRend2;

    bool objSelected = false;

    // bool triggerPressed = false;

    public GameObject obj_grabble;
    public GameObject obj_grabble_parent;

   // public GameObject obj_VRCamera;

    public float fps = 0;
  //  bool gripEnabled = false;
  //  bool joystickEnabled = false;

    [Header("Content transfer Settings")]
  //  public GameObject[] m_images;
  //  GameObject m_currentImage;

    public GameObject obj_phone;
    
   // public GameObject obj_ContentTransferParent;



   // [Header("AV Settings")]
  //  public GameObject m_phoneBody;
  //  public GameObject m_virtualCamera;



    //declare a buffer to store objects
    // GameObject  m_grabble_objects;

    public enum CURRENT_SCENR
    {
        CONTENT_TRANSFER,
        BUTTON_REMAPPING,
        AV,
        TEXT,
        PHONE_ONLY,
    }

    public CURRENT_SCENR m_currentScene = CURRENT_SCENR.BUTTON_REMAPPING;
    public bool touchActive;
    private bool hit;
    void Awake()
    {

    }

    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;

        // Instantiate(m_images[0], obj_phone.transform.position, obj_phone.transform.rotation).SetActive(true);
        if (m_TCPIP_Base == null)
        {
            try
            {
                m_TCPIP_Base = GetComponent<BackgroundCommunication>();
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



        lRend2 = lRStartPoint.AddComponent<LineRenderer>();
        lRend2.startWidth = 0.001f;
        lRend2.endWidth= 0.001f;
    }

 //   public GameObject m_objCameraView;


    public GameObject gameObjHit;
    void Update()
    {

   
        //render a ray
        Ray ray = new Ray(lRStartPoint.transform.position, lRStartPoint.transform.forward);
        RaycastHit hitInfo;

        //   Debug.DrawLine(ray.origin,new  Vector3 (5,5,5));//划出射线，只有在scene视图中才能看到

        lRend2.positionCount = 2;//设置顶点数 
        lRend2.SetPosition(0, lRStartPoint.transform.position);//设置顶点位置
        //Vector3 lREndPos = new Vector3(1080f, 540f, 0f);
        //lRend2.SetPosition(1, lREndPos);//设置顶点位置


        //check if hits a target
       
        if (Physics.Raycast(ray, out hitInfo))
        {
            //  obj_pointer.transform.position = hitInfo.point;
            //  Debug.DrawLine(ray.origin, hitInfo.point);//划出射线，只有在scene视图中才能看到
            gameObjHit = hitInfo.collider.gameObject;
            Debug.Log(gameObjHit.name);

            //   Debug.Log("click object name is " + gameObj.name);
            if (gameObjHit != null)//当射线碰撞目标为boot类型的物品 ，执行拾取操作
            {
               //Vector3 tempHitPos = hitInfo.point;
                lRend2.SetPosition(1, hitInfo.point);//设置顶点位置

                if (gameObjHit.name.StartsWith("SportCar20"))
                {
                    gameObjHit = gameObjHit.transform.parent.gameObject;

                    objSelected = true;

                    if (gameObjHit.name.StartsWith("SportCar20_Brake"))
                    {
                        gameObjHit = gameObjHit.transform.parent.parent.gameObject;
                    }
                    if (gameObjHit.name=="SportCar20")
                    {
                        //gameObjHit = gameObjHit.transform.parent.gameObject;
                        gameObjHit = null;
                        //禁止移动车体
                        objSelected = false;
                    }

                }
                else
                {
                    objSelected = false;
                }
            }
            else
            {
                objSelected = false;
                lRend2.SetPosition(1, lRStartPoint.transform.forward * 1000);//设置顶点位置
                //lRend2.SetPosition(1, lREndPos);//设置顶点位置
            }
        }
        else
        {
            objSelected = false;
            lRend2.SetPosition(1, lRStartPoint.transform.forward * 1000);//设置顶点位置
            //lRend2.SetPosition(1, lREndPos);//设置顶点位置
            //gameObjHit = null;
        }

        if (Input.touchCount != 0)
        {
            touchActive = true;
        } else
        {
            touchActive = false;
        }


        if (objSelected && touchActive)
        {
            if (lRStartPoint.transform.childCount == 0)
            {
                obj_grabble = gameObjHit;
            }
            m_phoneInputEnabled = true;

        }
        else
        {
            if(m_phoneInputEnabled   )
            {
                if (!touchActive)
                {
                    m_phoneInputEnabled = false;
                }
            }


        }
     //   Debug.Log(m_phoneInputEnabled);


        if (m_phoneInputEnabled)
        {
            if (lRStartPoint.transform.childCount == 0)
            { 
                obj_grabble.transform.SetParent(lRStartPoint.transform);
                lRend2.SetPosition(1, hitInfo.point);
                // obj_asymetric_operation = obj_grabble;
            }

        }
        else
        {
            if (obj_grabble != null)
            {
            obj_grabble.transform.SetParent(obj_grabble_parent.transform);
                //obj_grabble = null;
                gameObjHit = null;
                //lRend2.SetPosition(1, lREndPos);
            }

        }





        //send pos to PC
        Data_ARINVR data = new Data_ARINVR(CLIENT_NAME.CN_ANDROID_1, DATA_NAME.DT_ARINVR);
        //不发送 手机位置，发送射线首末位置
        data.SetData(lRend2.GetPosition(0), lRend2.GetPosition(1)   , m_objs);

        m_TCPIP_Base.SendToServer(data.ToByteArray());





    }

    public GameObject m_ARCamera;
    public GameObject[] m_objs;



    public Vector3 projectedVector2;




    Data_VirtualButton virtualButtonData = new Data_VirtualButton(CLIENT_NAME.CN_ANDROID_1, DATA_NAME.DT_VITUAL_BUTTON);
    private bool m_phoneInputEnabled = false;




    public void ProcessingData(DATA_NAME dataName, byte[] data)
    {
        switch (dataName)
        {
            //add other processings here!
            // case DATA_NAME.DT_SPATIAL_INPUT_POINT:
            //      Data_Position3f touchpoint = new Data_Position3f(data);

            case DATA_NAME.DT_SCENE3:

                Data_ModelNumber sceneID = new Data_ModelNumber(data);

                m_currentScene = (CURRENT_SCENR)sceneID.value;







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

    //private Vector3 GetRelativePosition(Transform origin, Vector3 position)
    //{
    //    Vector3 distance = position - origin.position;
    //    Vector3 relativePosition = Vector3.zero;
    //    relativePosition.x = Vector3.Dot(distance, origin.right.normalized);
    //    relativePosition.y = Vector3.Dot(distance, origin.up.normalized);
    //    relativePosition.z = Vector3.Dot(distance, origin.forward.normalized);
    //    return relativePosition;
    //}


}