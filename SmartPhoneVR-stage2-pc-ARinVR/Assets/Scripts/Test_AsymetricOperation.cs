using ServerCommunication;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_AsymetricOperation : MonoBehaviour
{
    public TCPIP_Base m_TCPIP_Base;


    public GameObject obj_grabble;
    public GameObject obj_grabble_parent;
    RaycastHit hitInfo;

    public GameObject controller;

    LineRenderer lRend2;

    bool objSelected = false;


    public GameObject obj_asymetric_operation;


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
    private void Update()
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



        if (objSelected && OVRInput.Get(OVRInput.RawButton.RIndexTrigger))
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


    public GameObject cam;



    public void ProcessingData(DATA_NAME dataName, byte[] data)
    {
        switch (dataName)
        {
            case DATA_NAME.DT_ASYMETRIC_ROTATE:
                Data_Position3f deltaPos = new Data_Position3f(data);
                Debug.Log("DT_ASYMETRIC_ROTATE");

                if (obj_asymetric_operation != null)
                {

                    obj_asymetric_operation.transform.Rotate(Vector3.down * deltaPos.value[0] / 10, Space.Self);
                    obj_asymetric_operation.transform.Rotate(Vector3.right * deltaPos.value[1] / 10, Space.Self);
                    obj_asymetric_operation.transform.Rotate(Vector3. forward* deltaPos.value[2] / 10, Space.Self);


                }
                break;

            case DATA_NAME.DT_ASYMETRIC_TRANSLATE:
                Data_Position3f deltaTrans = new Data_Position3f(data);
                if (obj_asymetric_operation != null)
                {
                    obj_asymetric_operation.transform.Translate(new Vector3( deltaTrans.value[0] / 1080 * 0.7f, deltaTrans.value[1] / 1080 * 0.7f, -deltaTrans.value[2] / 1080 * 0.7f), cam.transform);
                   // obj_asymetric_operation.transform.Translate(cam.transform.up * deltaTrans.value[1] / 1080 * 0.7f, cam.transform);
                   // obj_asymetric_operation.transform.Translate(cam.transform.right  * deltaTrans.value[2] / 1080 * 0.7f, cam.transform);
                }
                break;

            case DATA_NAME.DT_ASYMETRIC_SCALE:
                Data_Position3f deltaScale = new Data_Position3f(data);
                if (obj_asymetric_operation != null)
                {
                    Debug.Log(deltaScale.value[0]);

                    Vector3 localScale = obj_asymetric_operation.transform.localScale;
                    Vector3 scale = new Vector3(localScale.x * (1+ deltaScale.value[0]/1080),
                                  localScale.y * (1 + deltaScale.value[0]/1080),
                                  localScale.z * (1 + deltaScale.value[0]/1080));
                    //最小缩放到 0.3 倍  
                    if (scale.x > 0.03f  && scale.x < 10  )
                    {
                        obj_asymetric_operation.transform.localScale = scale;
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
