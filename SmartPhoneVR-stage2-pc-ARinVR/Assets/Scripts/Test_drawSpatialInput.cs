using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using ServerCommunication;

public class Test_drawSpatialInput : MonoBehaviour
{
    public TCPIP_Base m_TCPIP_Base;

    public GameObject obj_phone;

    Vector3[] path;
    private float time = 0;
     List<Vector3> pos = new List<Vector3>();
     List<List<Vector3>> poses = new List<List<Vector3>>();

    public List<LineRenderer> lineRenderers;


    void Awake()
    {
 
                path = pos.ToArray();//转成数组
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
   lineRenderers[i].positionCount = poses[i].Count;//设置顶点数      
                    lineRenderers[i].SetPositions(poses[i].ToArray());//设置顶点位置

                    lineRenderers[i].enabled = true;

                 
                }

            }
        }

    }
    public float fps = 0;

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
                    lRend2.SetWidth(0.002f, 0.002f);

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