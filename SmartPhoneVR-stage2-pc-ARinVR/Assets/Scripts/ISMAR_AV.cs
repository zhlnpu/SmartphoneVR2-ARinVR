using ServerCommunication;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ISMAR_AV : MonoBehaviour
{
    public TCPIP_Base m_TCPIP_Base;


    public GameObject obj_grabble;
    public GameObject obj_grabbleTarget;

    public GameObject obj_grabble_parent;
    RaycastHit hitInfo;

    public GameObject m_RightController;
    public GameObject m_LeftController;

  //  public GameObject m_PhoneDevice;

    public bool m_SwitchHand = false;


   // LineRenderer lRend2;

    bool objSelected = false;

    GameObject thisController;

    // public GameObject obj_asymetric_operation;
    Vector3 rot1;
    Vector3 rot2;


    public GameObject m_finishTestAligned;


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



        OnClickRightHand();

    }

    private void Update()
    {

        Ray ray = new Ray(thisController.transform.position, thisController.transform.forward);
        RaycastHit hitInfo;

        //   Debug.DrawLine(ray.origin,new  Vector3 (5,5,5));//划出射线，只有在scene视图中才能看到

        thisController.GetComponent<LineRenderer>().positionCount = 2;//设置顶点数 

        thisController.GetComponent<LineRenderer>().SetPosition(0, thisController.transform.position);//设置顶点位置

        if (Physics.Raycast(ray, out hitInfo))
        {
            thisController.GetComponent<LineRenderer>().SetPosition(1, hitInfo.point);//设置顶点位置
                                                                                      //  obj_pointer.transform.position = hitInfo.point;

            //  Debug.DrawLine(ray.origin, hitInfo.point);//划出射线，只有在scene视图中才能看到
            GameObject gameObj = hitInfo.collider.gameObject;
            //  Debug.Log("click object name is " + gameObj.name);





            if (gameObj.name == "SportCar20_Body_Col")//当射线碰撞目标为boot类型的物品 ，执行拾取操作
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
            thisController.GetComponent<LineRenderer>().SetPosition(1, thisController.transform.forward * 1000);//设置顶点位置

        }




        if (objSelected && OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger))
        {
            m_phoneInputEnabled = true;
        }

        if (OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger))
        {
            m_phoneInputEnabled = false;
            // print("  m_phoneInputEnabled: false   ");
        }





        if (objSelected && OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
        {
            m_controllerInputEnabled = true;
        }
        else
        {
            if (!OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
            {
                m_controllerInputEnabled = false;

            }
        }

        if (m_phoneInputEnabled && OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
        {
            m_controllerInputEnabled = true;

        }




        if (m_controllerInputEnabled)
        {
            obj_grabble.transform.SetParent(thisController.transform);
            // obj_asymetric_operation = obj_grabble;
        }
        else
        {
            obj_grabble.transform.SetParent(obj_grabble_parent.transform);
            // obj_asymetric_operation = null;
        }



        if (WellAlighed(obj_grabble.transform, obj_grabbleTarget.transform))
        {
           // Debug.LogError("aligned");

            m_finishTestAligned.SetActive(true);




        }
    }


    


    bool WellAlighed(Transform origin, Transform targer)
    {
        bool result = false;
       //           print("position " + Vector3.Distance(origin.position, targer.position).ToString());
      //  print("localScale " + Vector3.Distance(origin.localScale, targer.localScale).ToString());

      //  print("eulerAngles " + Vector3.Distance(origin.rotation.eulerAngles, targer.rotation.eulerAngles).ToString());

   



        if (Vector3.Distance(origin.position, targer.position) < 0.01)
        {

            if (Vector3.Distance(origin.localScale, targer.localScale) < 0.02)
            {
                rot1 = origin.rotation.eulerAngles;
                rot2 = targer.rotation.eulerAngles;

                if (rot1.x > 180)
                    rot1.x -= 360;
                if (rot1.y > 180)
                    rot1.y -= 360;
                if (rot1.z > 180)
                    rot1.z -= 360;



                if (Vector3.Distance(rot1, rot2) < 0.8)
                {
                    result = true;
                }
            }
        }
        return result;
    }



    public GameObject cam;

    bool m_phoneInputEnabled = false;

    bool m_controllerInputEnabled = false;


    public void ProcessingData(DATA_NAME dataName, byte[] data)
    {
        switch (dataName)
        {
            case DATA_NAME.DT_ASYMETRIC_ROTATE:
                Data_Position3f deltaPos = new Data_Position3f(data);
                Debug.Log("DT_ASYMETRIC_ROTATE");

                if (m_phoneInputEnabled)
                {

                    //obj_grabble.transform.Rotate(Vector3.down * deltaPos.value[0] / 10, Space.Self);
                    //obj_grabble.transform.Rotate(Vector3.right * deltaPos.value[1] / 10, Space.Self);
                    //obj_grabble.transform.Rotate(Vector3.forward * deltaPos.value[2] / 10, Space.Self);



                   obj_grabble.transform.RotateAround(obj_grabble.transform.position,  cam.transform.right ,   deltaPos.value[1]/10 );
                    obj_grabble.transform.RotateAround(obj_grabble.transform.position, cam.transform.up, -deltaPos.value[0] / 10);
                    obj_grabble.transform.RotateAround(obj_grabble.transform.position, cam.transform.forward, deltaPos.value[2] / 10);

                   // obj_grabble.transform.Rotate(Vector3.right * deltaPos.value[1] / 10, Space.Self);
                 //   obj_grabble.transform.Rotate(Vector3.forward * deltaPos.value[2] / 10, Space.Self);


                }
                break;

            case DATA_NAME.DT_ASYMETRIC_TRANSLATE:
                Data_Position3f deltaTrans = new Data_Position3f(data);
                if (m_phoneInputEnabled)
                {
                    obj_grabble.transform.Translate(new Vector3(deltaTrans.value[0] / 1080 * 0.7f, deltaTrans.value[1] / 1080 * 0.7f, -deltaTrans.value[2] / 1080 * 0.7f), cam.transform);
                    // obj_asymetric_operation.transform.Translate(cam.transform.up * deltaTrans.value[1] / 1080 * 0.7f, cam.transform);
                    // obj_asymetric_operation.transform.Translate(cam.transform.right  * deltaTrans.value[2] / 1080 * 0.7f, cam.transform);
                }
                break;

            case DATA_NAME.DT_ASYMETRIC_SCALE:
                Data_Position3f deltaScale = new Data_Position3f(data);
                if (m_phoneInputEnabled)
                {
                    Debug.Log(deltaScale.value[0]);

                    Vector3 localScale = obj_grabble.transform.localScale;
                    Vector3 scale = new Vector3(localScale.x * (1 + deltaScale.value[0] / 1080),
                                  localScale.y * (1 + deltaScale.value[0] / 1080),
                                  localScale.z * (1 + deltaScale.value[0] / 1080));
                    //最小缩放到 0.3 倍  
                    if (scale.x > 0.03f && scale.x < 10)
                    {
                        obj_grabble.transform.localScale = scale;
                    }
                }
                break;

            default:
                Debug.LogError("Unknown datatype");
                break;
        }
    }


    private void OnGUI()
    {

        if (GUI.Button(new Rect(0, 400, 100, 100), "Right Hand"))
        {
            OnClickRightHand();
        }

        if (GUI.Button(new Rect(0, 500, 100, 100), "Left Hand"))
        {
            OnClickLeftHand();
        }


    }





    void OnClickRightHand()
    {
        if (thisController != m_RightController)
        {
            //right
            m_RightController.AddComponent<LineRenderer>();
            m_RightController.GetComponent<LineRenderer>().SetWidth(0.002f, 0.002f);

            if (m_LeftController.GetComponent<LineRenderer>() != null)
            {
                Destroy(m_LeftController.GetComponent<LineRenderer>());
            }

            thisController = m_RightController;


            //     m_PhoneDevice.transform.SetParent(thisController.transform);
            m_LeftController.SetActive(false);
            m_RightController.SetActive(true);

            Data_ModelNumber data_leftOrRight = new Data_ModelNumber(m_TCPIP_Base.m_thisClientName, DATA_NAME.DT_MODEL_NUMBER, 0);
            m_TCPIP_Base.SendToServer(data_leftOrRight.ToByteArray());
        }
    }


    void OnClickLeftHand()
    {
        if (thisController != m_LeftController)
        {
            //left
            m_LeftController.AddComponent<LineRenderer>();
            m_LeftController.GetComponent<LineRenderer>().SetWidth(0.002f, 0.002f);


            if (m_RightController.GetComponent<LineRenderer>() != null)
            {
                Destroy(m_RightController.GetComponent<LineRenderer>());
            }

            thisController = m_LeftController;
            //   m_PhoneDevice.transform.SetParent(thisController.transform);

            m_RightController.SetActive(false);
            m_LeftController.SetActive(true);


            Data_ModelNumber data_leftOrRight = new Data_ModelNumber(m_TCPIP_Base.m_thisClientName, DATA_NAME.DT_MODEL_NUMBER, 1);
            m_TCPIP_Base.SendToServer(data_leftOrRight.ToByteArray());
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
