using ServerCommunication;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene1 : MonoBehaviour
{
    public TCPIP_Base m_TCPIP_Base;
    public enum CURRENT_SCENR
    {
        BIMANUAL_OPERATION,
        ONSCRREN_INPUT,
    }

    [Header("Operation")]
    public GameObject obj_grabble;
    public GameObject obj_grabble_parent;
    RaycastHit hitInfo;

    public GameObject controller;

    LineRenderer lRend2;

    bool objSelected = false;


    GameObject obj_asymetric_operation;

    //define the ralative space for object movement
    public GameObject cam;


    public GameObject m_objRayStartPoint;


    CURRENT_SCENR m_currentScene = CURRENT_SCENR.BIMANUAL_OPERATION;

    public GameObject obj_blueBackground;
    public GameObject obj_virtualCamera;


    bool m_phoneInputEnabled = false;

    bool m_controllerInputEnabled = false;



    public GameObject m_finishAlignedModel1;
    public GameObject m_finishAlignedModel2;

    bool m_objAlignedCkeck1 = false;
    bool m_objAlignedCkeck2 = false;

    public GameObject m_finishAlignedText11;
    public GameObject m_finishAlignedText2;


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

        lRend2 = controller.AddComponent<LineRenderer>();
        lRend2.SetWidth(0.002f, 0.002f);
    }

    //OVRcamerarig
    public GameObject obj_VRCamera;

    private void Update()
    {

        Vector2 controllerStick = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);

        if (Mathf.Abs(controllerStick.x) > 0.01 || Mathf.Abs(controllerStick.y) > 0.01)
        {
            //            obj_VRCamera.transform.Translate(controllerStick.x / 100, 0, controllerStick.y / 100, cam.transform);

            Vector3 projectedVector = cam.transform.forward * controllerStick.y / 100 + cam.transform.right * controllerStick.x / 100;

            obj_VRCamera.transform.Translate(projectedVector.x, 0, projectedVector.z);
        }
        //  print(cam.transform.forward);

        //UI settings
        switch (m_currentScene)
        {
            case CURRENT_SCENR.BIMANUAL_OPERATION:
                // render phone screen
                // close virtual camera
                obj_blueBackground.SetActive(true);
                obj_virtualCamera.SetActive(false);

                break;
            case CURRENT_SCENR.ONSCRREN_INPUT:
                // stop screen mirroring
                // open virtual camera
                obj_blueBackground.SetActive(false);
                obj_virtualCamera.SetActive(true);

                break;
            default:
                break;
        }

        //functions
        switch (m_currentScene)
        {
            case CURRENT_SCENR.BIMANUAL_OPERATION:
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
                        if (gameObj.name == "SportCar20_Body_Col")//当射线碰撞目标为boot类型的物品 ，执行拾取操作
                        {
                            obj_grabble = gameObj.transform.parent.parent.gameObject;
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
                        //if (!OVRInput.Get(OVRInput.RawButton.RIndexTrigger))
                        //    objSelected = false;

                        lRend2.SetPosition(1, controller.transform.forward * 1000);//设置顶点位置

                        if (objSelected && m_phoneInputEnabled)
                        {
                            objSelected = true;
                        }
                        else
                        {
                            objSelected = false;

                        }
                        //  obj_pointer.transform.position = controller.transform.forward * 1000;
                    }

                    if (objSelected && OVRInput.GetDown(OVRInput.RawButton.RHandTrigger))
                    {
                        m_phoneInputEnabled = true;


                    }

                    if (OVRInput.GetUp(OVRInput.RawButton.RHandTrigger))
                    {
                        m_phoneInputEnabled = false;
                        // print("  m_phoneInputEnabled: false   ");
                    }

                    if (objSelected && OVRInput.Get(OVRInput.RawButton.RIndexTrigger))
                    {
                        m_controllerInputEnabled = true;
                    }
                    else
                    {
                        if (!OVRInput.Get(OVRInput.RawButton.RIndexTrigger))
                        {
                            m_controllerInputEnabled = false;

                        }
                    }

                    if (m_phoneInputEnabled && OVRInput.Get(OVRInput.RawButton.RIndexTrigger))
                    {
                        m_controllerInputEnabled = true;

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
                            // obj_asymetric_operation = null;
                        }
                    }


                }
                break;

            case CURRENT_SCENR.ONSCRREN_INPUT:
                {

                    Ray ray = new Ray(m_objRayStartPoint.transform.position, m_objRayStartPoint.transform.forward);
                    RaycastHit hitInfo;

                    //   Debug.DrawLine(ray.origin,new  Vector3 (5,5,5));//划出射线，只有在scene视图中才能看到

                    lRend2.positionCount = 2;//设置顶点数 

                    lRend2.SetPosition(0, m_objRayStartPoint.transform.position);//设置顶点位置

                    if (Physics.Raycast(ray, out hitInfo))
                    {
                        lRend2.SetPosition(1, hitInfo.point);//设置顶点位置
                                                             //  obj_pointer.transform.position = hitInfo.point;

                        //  Debug.DrawLine(ray.origin, hitInfo.point);//划出射线，只有在scene视图中才能看到
                        GameObject gameObj = hitInfo.collider.gameObject;
                        //   Debug.Log("click object name is " + gameObj.name);
                        if (gameObj.name == "SportCar20_Body_Col")//当射线碰撞目标为boot类型的物品 ，执行拾取操作
                        {
                            obj_grabble = gameObj.transform.parent.parent.gameObject;
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
                        //if (!OVRInput.Get(OVRInput.RawButton.RIndexTrigger))
                        //    objSelected = false;

                        lRend2.SetPosition(1, m_objRayStartPoint.transform.forward * 1000);//设置顶点位置

                        if (objSelected && m_phoneInputEnabled)
                        {
                            objSelected = true;
                        }
                        else
                        {
                            objSelected = false;

                        }
                        //  obj_pointer.transform.position = controller.transform.forward * 1000;
                    }

                    if (objSelected && OVRInput.GetDown(OVRInput.RawButton.RHandTrigger))
                    {
                        m_phoneInputEnabled = true;


                    }

                    if (OVRInput.GetUp(OVRInput.RawButton.RHandTrigger))
                    {
                        m_phoneInputEnabled = false;
                        // print("  m_phoneInputEnabled: false   ");
                    }

                    if (objSelected && OVRInput.Get(OVRInput.RawButton.RIndexTrigger))
                    {
                        m_controllerInputEnabled = true;
                    }
                    else
                    {
                        if (!OVRInput.Get(OVRInput.RawButton.RIndexTrigger))
                        {
                            m_controllerInputEnabled = false;

                        }
                    }

                    if (m_phoneInputEnabled && OVRInput.Get(OVRInput.RawButton.RIndexTrigger))
                    {
                        m_controllerInputEnabled = true;

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
                            // obj_asymetric_operation = null;
                        }
                    }


                }

                break;
            default:
                break;
        }





        //check is the models are aligned    
        if (!m_objAlignedCkeck1)
        {
            if (obj_grabble != null)
            {
                if (WellAlighed(obj_grabble.transform, m_finishAlignedModel1.transform))
                {
                    // Debug.LogError("aligned");

                    m_objAlignedCkeck1 = true;
                    m_finishAlignedText11.SetActive(true);

                }
            }

        }

        if (!m_objAlignedCkeck2)
        {
            if (obj_grabble != null)
            {
                if (WellAlighed(obj_grabble.transform, m_finishAlignedModel2.transform))
                {
                    // Debug.LogError("aligned");

                    m_objAlignedCkeck2 = true;
                    m_finishAlignedText2.SetActive(true);

                }
            }

        }

    }





    bool WellAlighed(Transform origin, Transform targer)
    {
        bool result = false;
        if (Vector3.Distance(origin.position, targer.position) < 0.01)
        {
            if (Vector3.Distance(origin.localScale, targer.localScale) < 0.02)
            {
                Vector3 rot1 = origin.rotation.eulerAngles;
                Vector3 rot2 = targer.rotation.eulerAngles;

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














public void ProcessingData(DATA_NAME dataName, byte[] data)
    {
        switch (dataName)
        {
            case DATA_NAME.DT_SCENE1:

                Data_ModelNumber sceneID = new Data_ModelNumber(data);

                m_currentScene = (CURRENT_SCENR)sceneID.value;

                break;


            case DATA_NAME.DT_ASYMETRIC_ROTATE:
                Data_Position3f deltaPos = new Data_Position3f(data);
                Debug.Log("DT_ASYMETRIC_ROTATE");

                if (m_phoneInputEnabled)
                {
                    obj_grabble.transform.RotateAround(obj_grabble.transform.position, cam.transform.up, -deltaPos.value[0] / 10);
                    obj_grabble.transform.RotateAround(obj_grabble.transform.position, cam.transform.right, deltaPos.value[1] / 10);
                    obj_grabble.transform.RotateAround(obj_grabble.transform.position, cam.transform.forward, deltaPos.value[2] / 10);
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
