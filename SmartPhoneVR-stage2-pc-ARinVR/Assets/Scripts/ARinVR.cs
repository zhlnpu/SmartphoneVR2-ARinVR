using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ServerCommunication;
using System;

public class ARinVR : MonoBehaviour
{
    //  public GameObject[] m_images;
    //  GameObject m_currentImage;

    public TCPIP_Base m_TCPIP_Base;

    //  public GameObject obj_phone;

    [Header("Button remapping Settings")]
    public GameObject controller;

    LineRenderer lRend2;

    bool objSelected = false;

    // bool triggerPressed = false;

    GameObject obj_grabble;
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



        lRend2 = m_ARCamera.AddComponent<LineRenderer>();
        lRend2.SetWidth(0.002f, 0.002f);
    }

 //   public GameObject m_objCameraView;


    GameObject gameObjHit;
    void Update()
    {

   
        ////render a ray
        //Ray ray = new Ray(controller.transform.position, controller.transform.forward);
        //RaycastHit hitInfo;

        ////   Debug.DrawLine(ray.origin,new  Vector3 (5,5,5));//划出射线，只有在scene视图中才能看到

        //lRend2.positionCount = 2;//设置顶点数 
        //lRend2.SetPosition(0, controller.transform.position);//设置顶点位置


        ////check if hits a target
        //if (Physics.Raycast(ray, out hitInfo))
        //{
        //    //  obj_pointer.transform.position = hitInfo.point;
        //    //  Debug.DrawLine(ray.origin, hitInfo.point);//划出射线，只有在scene视图中才能看到
        //    gameObjHit = hitInfo.collider.gameObject;


        //       Debug.Log(gameObjHit.name);

        //    //   Debug.Log("click object name is " + gameObj.name);
        //    if (gameObjHit != null)//当射线碰撞目标为boot类型的物品 ，执行拾取操作
        //    {
        //        lRend2.SetPosition(1, hitInfo.point);//设置顶点位置

        //        if (gameObjHit.name.StartsWith("SportCar20"))
        //        {
        //            gameObjHit = gameObjHit.transform.parent.gameObject;

        //            objSelected = true;

        //            if (gameObjHit.name.StartsWith("SportCar20_Brake"))
        //            {
        //                gameObjHit = gameObjHit.transform.parent.parent.gameObject;
        //            }
        //            if (gameObjHit.name=="SportCar20")
        //            {
        //                gameObjHit = gameObjHit.transform.parent.gameObject;





        //                //禁止移动车体
        //                objSelected = false;






        //            }

        //        }
        //        else
        //        {
        //            objSelected = false;
        //        }
        //    }
        //    else
        //    {
        //        objSelected = false;
        //        lRend2.SetPosition(1, controller.transform.forward * 1000);//设置顶点位置
        //    }
        //}
        //else
        //{
        //    objSelected = false;
        //    lRend2.SetPosition(1, controller.transform.forward * 1000);//设置顶点位置
        //}



        //if (objSelected && OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger))
        //{
        //    obj_grabble = gameObjHit;
        //    m_controllerInputEnabled = true;
        //}

        //if (OVRInput.GetUp(OVRInput.RawButton.RIndexTrigger))
        //{
        //    m_controllerInputEnabled = false;


        //}

        //if (m_controllerInputEnabled)
        //{
        //    obj_grabble.transform.SetParent(controller.transform);
        //    // obj_asymetric_operation = obj_grabble;
        //}
        //else
        //{
        //    if (obj_grabble != null)
        //    {
        //        obj_grabble.transform.SetParent(obj_grabble_parent.transform);
        //        obj_grabble = null;
        //    }
        //}



        m_plan_alone.transform.position = m_plan_controller.transform.position;
        m_plan_alone.transform.rotation = m_plan_controller.transform.rotation;


    }

    public GameObject m_plan_controller;
    public GameObject m_plan_alone;




    public Vector3 projectedVector2;




    Data_VirtualButton virtualButtonData = new Data_VirtualButton(CLIENT_NAME.CN_ANDROID_1, DATA_NAME.DT_VITUAL_BUTTON);
    private bool m_controllerInputEnabled = false;



    public GameObject m_ARCamera;
    public GameObject[] m_objs;

    public GameObject m_lineStart;
    public GameObject m_lineEnd;




    public void ProcessingData(DATA_NAME dataName, byte[] data)
    {
        switch (dataName)
        {
            //add other processings here!
            // case DATA_NAME.DT_SPATIAL_INPUT_POINT:
            //      Data_Position3f touchpoint = new Data_Position3f(data);

            case DATA_NAME.DT_ARINVR:

                Data_ARINVR result = new Data_ARINVR(data);



//                m_ARCamera.transform.localPosition = new Vector3(result.pos[0], result.pos[1], result.pos[2]);

//                m_ARCamera.transform.localRotation = new Quaternion(result.pos[3], result.pos[4], result.pos[5], result.pos[6]);





            //    lRend2.positionCount = 2;//设置顶点数 

                m_lineStart.transform.localPosition = new Vector3(result.pos[0], result.pos[1], result.pos[2]);
                m_lineEnd.transform.localPosition = new Vector3(result.pos[3], result.pos[4], result.pos[5]);

            //    lRend2.SetPosition(0, m_lineStart.transform.position);//设置顶点位置
            //    lRend2.SetPosition(1, m_lineEnd.transform.position );//设置顶点位置

                for (int i = 0; i < m_objs.Length; i++)
                {
                    m_objs[i].transform.localPosition = new Vector3(result.pos[7 * (i + 1)], result.pos[7 * (i + 1) + 1], result.pos[7 * (i + 1) + 2]);

                    m_objs[i].transform.localRotation = new Quaternion(result.pos[7 * (i + 1) + 3], result.pos[7 * (i + 1) + 4], result.pos[7 * (i + 1) + 5], result.pos[7 * (i + 1) + 6]);


                }









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