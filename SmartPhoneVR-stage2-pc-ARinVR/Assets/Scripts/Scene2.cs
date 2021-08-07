using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using ServerCommunication;
using TMPro;

public class Scene2 : MonoBehaviour
{
    public TCPIP_Base m_TCPIP_Base;

    public GameObject obj_phone;

    Vector3[] path;
    private float time = 0;
    List<Vector3> pos = new List<Vector3>();
    List<List<Vector3>> poses = new List<List<Vector3>>();

    public List<LineRenderer> lineRenderers;
    public GameObject controller;

    LineRenderer m_controllerRay;


    public GameObject m_text;


    GameObject gameObjHit;
    void Awake()
    {
        path = pos.ToArray();//转成数组
        m_currentMaterial = new Material(Shader.Find("Sprites/Default"));
    }

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


        path = pos.ToArray();//转成数组

        m_controllerRay = controller.AddComponent<LineRenderer>();

        m_controllerRay.SetWidth(0.002f, 0.002f);

    }

    void Update()
    {

        time += Time.deltaTime;
        if (time > 0.01)//每0.1秒绘制一次
        {
            time = 0;
            // pos.Add(transform.position);//添加当前坐标进链表
            // path = pos.ToArray();//转成数组

            if (poses.Count == 0)
            {
                //     line.enabled = false;
            }
            else
            {
                for (int i = 0; i < poses.Count; i++)
                {

                    lineRenderers[i].enabled = true;

                    lineRenderers[i].positionCount = poses[i].Count;//设置顶点数      
                    lineRenderers[i].SetPositions(poses[i].ToArray());//设置顶点位置
                }

            }
        }





        Ray ray = new Ray(controller.transform.position, controller.transform.forward);
        RaycastHit hitInfo;

        //   Debug.DrawLine(ray.origin,new  Vector3 (5,5,5));//划出射线，只有在scene视图中才能看到

        m_controllerRay.positionCount = 2;//设置顶点数 
        m_controllerRay.SetPosition(0, controller.transform.position);//设置顶点位置


        //check if hits a target
        if (Physics.Raycast(ray, out hitInfo))
        {
            //  obj_pointer.transform.position = hitInfo.point;
            //  Debug.DrawLine(ray.origin, hitInfo.point);//划出射线，只有在scene视图中才能看到
            gameObjHit = hitInfo.collider.gameObject;
            //   Debug.Log("click object name is " + gameObj.name);

            if (gameObjHit != null)
            {
                m_controllerRay.SetPosition(1, hitInfo.point);//设置顶点位置

                if (gameObjHit.transform.parent.gameObject.name == "Palette")//当射线碰撞目标为boot类型的物品 ，执行拾取操作
                {
                    objSelected = true;
                }
            }
            else
            {
                objSelected = false;
                //   m_controllerRay.SetPosition(1, controller.transform.forward * 1000);//设置顶点位置
            }
        }
        else
        {
            m_controllerRay.SetPosition(1, controller.transform.forward * 1000);//设置顶点位置
        }
        if (objSelected)
        {
            bool currentTriggerState = OVRInput.Get(OVRInput.RawButton.RIndexTrigger);

            if (!currentTriggerState && lastTriggerState)
            {

                //press trigger event
                m_currentMaterial = gameObjHit.GetComponent<MeshRenderer>().material;
            }
            lastTriggerState = currentTriggerState;
        }




        //  Debug.Log( OVRInput.Get(OVRInput.RawButton.A));

        if (OVRInput.GetDown(OVRInput.RawButton.A))
        {
            if (m_lineWidth > 0.001)
                m_lineWidth -= 0.001f;

        }

        if (OVRInput.GetDown(OVRInput.RawButton.B))
        {
            if (m_lineWidth < 0.01)
                m_lineWidth += 0.001f;
        }


        EnableJoystick();
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
    }
    public GameObject obj_VRCamera;
    public GameObject m_objCameraView;


    private Material m_currentMaterial;

    public float fps = 0;
    private bool objSelected;
    private bool lastTriggerState = false;
    private float m_lineWidth = 0.002f;

    Data_CharByte phoneInput = new Data_CharByte(CLIENT_NAME.CN_ANDROID_1,DATA_NAME.DT_PHONE_INPUT);


    public void ProcessingData(DATA_NAME dataName, byte[] data)
    {
        switch (dataName)
        {
            //add other processings here!
            // case DATA_NAME.DT_SPATIAL_INPUT_POINT:
            //      Data_Position3f touchpoint = new Data_Position3f(data);


            case DATA_NAME.DT_SPATIAL_INPUT_POINT_START:
                Debug.Log("DT_SPATIAL_INPUT_POINT_START");
                //    if (pos.Count > 0)
                {
                    pos = new List<Vector3>();

                    poses.Add(pos);

                    GameObject newLine2 = new GameObject("Line");

                    LineRenderer lRend2 = newLine2.AddComponent<LineRenderer>();
                    lRend2.SetWidth(m_lineWidth, m_lineWidth);
                    lRend2.material = m_currentMaterial;

                    lineRenderers.Add(lRend2);



                }
                break;

            case DATA_NAME.DT_SPATIAL_INPUT_POINT_END:

                Debug.Log("DT_SPATIAL_INPUT_POINT_END");

                break;


            case DATA_NAME.DT_SPATIAL_INPUT_POINT:
                Data_Position3f touchpoint = new Data_Position3f(data);

                //                    pos.Add(new Vector3(touchpoint.value[0] / 1000.0f, touchpoint.value[1] / 1000.0f, touchpoint.value[2] / 1000.0f));

                float x = (540 - touchpoint.value[1]) / 540 * 0.035f;
                float z = (touchpoint.value[0] - 1120) / 540 * 0.035f;

                pos.Add(obj_phone.transform.TransformPoint(new Vector3(x, 0.005f, z)));

                //                    pos.Add(GetRelativePosition(obj_phone.transform, new Vector3(x,0,z)));



                fps = 1.0f / Time.deltaTime;
                break;




            case DATA_NAME.DT_PHONE_INPUT:

                 phoneInput = new Data_CharByte(data);

                print(phoneInput.value[0].ToString());
                m_text.GetComponent<TextMeshPro>().text =new string( phoneInput.value);


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