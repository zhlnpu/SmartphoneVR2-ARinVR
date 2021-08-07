using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ServerCommunication;
using System;

public class Scene3 : MonoBehaviour
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
  public   GameObject obj_grabble_parent;

    public GameObject obj_VRCamera;

    public float fps = 0;
    bool gripEnabled = false;
    bool joystickEnabled = false;

    [Header("Content transfer Settings")]
    public GameObject[] m_images;
    GameObject m_currentImage;

    public GameObject obj_phone;

    public GameObject obj_ContentTransferParent;



    [Header("AV Settings")]
    public GameObject m_phoneBody;
    public GameObject m_virtualCamera;



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



        lRend2 = controller.AddComponent<LineRenderer>();
        lRend2.SetWidth(0.002f, 0.002f);
    }

    public GameObject m_objCameraView;


    GameObject gameObjHit;
    void Update()
    {
        switch (m_currentScene)
        {
            //UI settings
            case CURRENT_SCENR.CONTENT_TRANSFER:
                {
             
                }
                break;
            case CURRENT_SCENR.BUTTON_REMAPPING:

                break;
            case CURRENT_SCENR.AV:
                lRend2.SetPosition(0, new Vector3(0, 0, 0));
                lRend2.SetPosition(1, new Vector3(0, 0, 0));
                break;
            case CURRENT_SCENR.TEXT:
                break;
            case CURRENT_SCENR.PHONE_ONLY:
                break;
            default:
                break;
        }

        //reactions
        switch (m_currentScene)
        {
            case CURRENT_SCENR.CONTENT_TRANSFER:
                {
                    EnableObjectRaySelection();

                    EnableJoystick();

                }
                break;
            case CURRENT_SCENR.BUTTON_REMAPPING:
                {
                    EnableObjectRaySelection();

                    EnableJoystick();                  
                }
                break;
            case CURRENT_SCENR.AV:
                EnableObjectRaySelection();


                break;
            case CURRENT_SCENR.TEXT:
                break;
            case CURRENT_SCENR.PHONE_ONLY:
                break;
            default:
                break;
        }            
    }

    private void EnableJoystick()
    {
        Vector2 controllerStick = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);

        if (Mathf.Abs(controllerStick.x) > 0.01 || Mathf.Abs(controllerStick.y) > 0.01)
        {
            // obj_VRCamera.transform.Translate(controllerStick.x / 100, 0, controllerStick.y / 100, m_objCameraView.transform);

            Vector3 projectedVector = m_objCameraView.transform.forward * controllerStick.y / 100 + m_objCameraView.transform.right * controllerStick.x / 100;


            obj_VRCamera.transform.Translate(projectedVector.x, 0, projectedVector.z);

        }
        else
        {
            if (joystickEnabled)
            {
                Vector2 phoneStick = new Vector2(virtualButtonData.joystickDirection[0], virtualButtonData.joystickDirection[1]);
                if (Mathf.Abs(phoneStick.x) > 0.01 || Mathf.Abs(phoneStick.y) > 0.01)
                {
                    //   obj_VRCamera.transform.Translate(phoneStick.x / 100, 0, phoneStick.y / 100, m_objCameraView.transform);

                    projectedVector2 = m_objCameraView.transform.forward * phoneStick.y / 30 + m_objCameraView.transform.right * phoneStick.x / 30;

                    obj_VRCamera.transform.Translate(projectedVector2.x, 0, projectedVector2.z);

                }

                joystickEnabled = false;
            }
        }
    }

    private void EnableObjectRaySelection()
    {
                //render a ray
        Ray ray = new Ray(controller.transform.position, controller.transform.forward);
        RaycastHit hitInfo;

        //   Debug.DrawLine(ray.origin,new  Vector3 (5,5,5));//划出射线，只有在scene视图中才能看到

        lRend2.positionCount = 2;//设置顶点数 
        lRend2.SetPosition(0, controller.transform.position);//设置顶点位置


        //check if hits a target
        if (Physics.Raycast(ray, out hitInfo))
        {
            //  obj_pointer.transform.position = hitInfo.point;
            //  Debug.DrawLine(ray.origin, hitInfo.point);//划出射线，只有在scene视图中才能看到
            gameObjHit = hitInfo.collider.gameObject;


            //   Debug.Log(gameObjHit.name);

            //   Debug.Log("click object name is " + gameObj.name);
            if (gameObjHit != null)//当射线碰撞目标为boot类型的物品 ，执行拾取操作
            {
                lRend2.SetPosition(1, hitInfo.point);//设置顶点位置


                if (gameObjHit.name.StartsWith("WPN") || gameObjHit.name.StartsWith("Pea"))
                {
                    gameObjHit = gameObjHit.transform.parent.gameObject;

                    objSelected = true;

                }
                else
                if (gameObjHit.name == "SportCar20_Body_Col")
                {
                    gameObjHit = gameObjHit.transform.parent.parent.gameObject;
                    objSelected = true;
                }
                else
                {
                    if (gameObjHit.name.StartsWith("RedC"))
                    {

                        objSelected = true;
                        //    Debug.Log("  redc");
                    }
                    else
                    {
                        objSelected = false;
                    }
                }
            }
            else
            {
                objSelected = false;
                lRend2.SetPosition(1, controller.transform.forward * 1000);//设置顶点位置
            }
        }
        else
        {
            objSelected = false;
            lRend2.SetPosition(1, controller.transform.forward * 1000);//设置顶点位置
        }



        if (objSelected && OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger))
        {
            obj_grabble = gameObjHit;
            m_controllerInputEnabled = true;
        }

        if (OVRInput.GetUp(OVRInput.RawButton.RIndexTrigger))
        {
            m_controllerInputEnabled = false;


        }

        if (m_controllerInputEnabled)
        {
            obj_grabble.transform.SetParent(controller.transform);
            // obj_asymetric_operation = obj_grabble;
        }
        else
        {
            if (obj_grabble != null)
            {
                obj_grabble.transform.SetParent(obj_grabble_parent.transform);
                obj_grabble = null;
            }
        }
    }

    public Vector3 projectedVector2;




    Data_VirtualButton virtualButtonData = new Data_VirtualButton(CLIENT_NAME.CN_ANDROID_1, DATA_NAME.DT_VITUAL_BUTTON);
    private bool m_controllerInputEnabled=false;




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

            case DATA_NAME.DT_VITUAL_BUTTON:
                Debug.Log("DT_VITUAL_BUTTON");

                virtualButtonData = new Data_VirtualButton(data);


             //   Debug.Log(virtualButtonData.joystickEnabled);
             //   Debug.Log(virtualButtonData.joystickDirection[0].ToString() + "   " + virtualButtonData.joystickDirection[1].ToString());
             //   Debug.Log(virtualButtonData.joystickMagnitude[0]);
             //   Debug.Log(virtualButtonData.gripEnabled);


                joystickEnabled = virtualButtonData.joystickEnabled;
                gripEnabled = virtualButtonData.gripEnabled;

                break;

            case DATA_NAME.DT_MODEL_NUMBER:
                Debug.Log("DT_MODEL_NUMBER");

                Data_ModelNumber modelNumber = new Data_ModelNumber(data);

                Instantiate(m_images[modelNumber.value], obj_phone.transform.position, m_images[modelNumber.value].transform.rotation, obj_grabble_parent.transform).SetActive(true);

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