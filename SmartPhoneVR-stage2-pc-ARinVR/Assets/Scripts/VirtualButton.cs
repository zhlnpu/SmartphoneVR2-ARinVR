using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ServerCommunication;


public class VirtualButton : MonoBehaviour
{
  //  public GameObject[] m_images;
  //  GameObject m_currentImage;

    public TCPIP_Base m_TCPIP_Base;

  //  public GameObject obj_phone;


    public GameObject controller;

    LineRenderer lRend2;

    bool objSelected = false;
    public GameObject obj_grabble;
    public GameObject obj_grabble_parent;
    public GameObject obj_asymetric_operation;

    public GameObject obj_VRCamera;



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

    void Update()
    {


        Ray ray = new Ray(controller.transform.position, controller.transform.forward);
        RaycastHit hitInfo;

        //   Debug.DrawLine(ray.origin,new  Vector3 (5,5,5));//划出射线，只有在scene视图中才能看到

        lRend2.positionCount = 2;//设置顶点数 

        lRend2.SetPosition(0, controller.transform.position);//设置顶点位置

        if (Physics.Raycast(ray, out hitInfo))
        {
            lRend2.SetPosition(1, hitInfo.point);//设置顶点位置
                                                 //  obj_pointer.transform.position = hitInfo.point;

            //  Debug.DrawLine(ray.origin, hitInfo.point);//划出射线，只有在scene视图中才能看到
            GameObject gameObj = hitInfo.collider.gameObject;
            //   Debug.Log("click object name is " + gameObj.name);
            if (gameObj == obj_grabble)//当射线碰撞目标为boot类型的物品 ，执行拾取操作
            {
                objSelected = true;
                //  Debug.Log("pick up!");
            }
            else
            {
                objSelected = false;

            }
        }
        else
        {
            if (!OVRInput.Get(OVRInput.RawButton.RIndexTrigger))
                objSelected = false;

            lRend2.SetPosition(1, controller.transform.forward * 1000);//设置顶点位置
                                                                       //  obj_pointer.transform.position = controller.transform.forward * 1000;
        }



        if (objSelected )
        {
            if (OVRInput.Get(OVRInput.RawButton.RIndexTrigger) || gripEnabled)

            {
                // Debug.Log("Button Down!");
                obj_grabble.transform.SetParent(controller.transform);
                obj_asymetric_operation = obj_grabble;
            }
            else
            {
                obj_grabble.transform.SetParent(obj_grabble_parent.transform);
                obj_asymetric_operation = null;
            }

        }
        else
        {
            obj_grabble.transform.SetParent(obj_grabble_parent.transform);
            obj_asymetric_operation = null;
        }




        Vector2 controllerStick = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);

        if(Mathf.Abs( controllerStick.x)>0.01 ||  Mathf.Abs(controllerStick.y)>0.01)
        {
            obj_VRCamera.transform.Translate(controllerStick.x / 100, 0, controllerStick.y / 100);
        }
        else
        {
            Vector2 phoneStick = new Vector2(virtualButtonData.joystickDirection[0], virtualButtonData.joystickDirection[1]);
            if (Mathf.Abs(phoneStick.x) > 0.01 || Mathf.Abs(phoneStick.y) > 0.01)
            {
            obj_VRCamera.transform.Translate(phoneStick.x / 100, 0, phoneStick.y / 100);


            }
        }


//           



    }
    public float fps = 0;
    bool gripEnabled = false;
     bool joystickEnabled=false;


    Data_VirtualButton virtualButtonData = new Data_VirtualButton(CLIENT_NAME.CN_ANDROID_1, DATA_NAME.DT_VITUAL_BUTTON);

    public void ProcessingData(DATA_NAME dataName, byte[] data)
    {
        switch (dataName)
        {
            //add other processings here!
            // case DATA_NAME.DT_SPATIAL_INPUT_POINT:
            //      Data_Position3f touchpoint = new Data_Position3f(data);


            case DATA_NAME.DT_VITUAL_BUTTON:
                Debug.Log("DT_VITUAL_BUTTON");

                 virtualButtonData = new Data_VirtualButton(data);


                Debug.Log(virtualButtonData.joystickEnabled);
                Debug.Log(virtualButtonData.joystickDirection[0].ToString() + "   " + virtualButtonData.joystickDirection[1].ToString());
                Debug.Log(virtualButtonData.joystickMagnitude[0]);
                Debug.Log(virtualButtonData.gripEnabled);


                joystickEnabled = virtualButtonData.joystickEnabled;
                gripEnabled = virtualButtonData.gripEnabled;

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