using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.IO;
using System;

using ServerCommunication;

public class TCPConnection : MonoBehaviour
{
    private TcpClient mySocket;
    private NetworkStream theStream;
    private BinaryWriter theWriter;
    private BinaryReader theReader;
    // private String Host = "192.168.191.1";	

    //1.1 change to IP of server
    public String Host = "127.0.0.1";
    public Int32 Port = 27015;
    public static int DEFAULT_BUFLEN = 4096;

    public bool socketReady = false;


    //定义所有涉及到的数据结构，接受解析时直接存储，
    //同时设置Get()函数，返回到主程序TCPCompoument里面

    //  private Dictionary<DATA_TYPE, bool> dict = new Dictionary<DATA_TYPE, bool>();


    public GameObject obj_phone;
    public GameObject cube;

    // Use this for initialization

    public GameObject obj_asymetric_operation;
    public GameObject obj_camera_center;




    private LineRenderer line;
    Vector3[] path;
    private float time = 0;
    public List<Vector3> pos = new List<Vector3>();
    public List<List<Vector3>> poses = new List<List<Vector3>>();

    public List<LineRenderer> lineRenderers;




    void Start()
    {
        Application.runInBackground = true;
        Debug.Log("Attempting to connect..");
        setupSocket();

        if (socketReady == true)
        {
            Debug.Log("socket connected!");
            // tell server type of the client

            C_CommandBase_Whom cmd = new C_CommandBase_Whom(CLIENT_NAME.COMMAND_CONNECTION);

            SendToServer(cmd.ToByteArray());
        }

        else
        {
            Application.Quit();
        }


        path = pos.ToArray();//转成数组




    }

    // Update is called once per frame
    void Update()
    {
        if (socketReady)
        {
            byte[] data = readSocket();
            if (data != null)
                ProcessingData(data);
        }


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
    }


    void OnApplicationQuit()
    {
        closeSocket();

    }


    public float fps = 0;

    public void ProcessingData(byte[] data)
    {
        //  C_CommandBase command = new C_CommandBase(data);
        C_CommandBase_Whom_Dataname baseType = new C_CommandBase_Whom_Dataname(data);


        if (baseType.whom == CLIENT_NAME.CN_ANDROID_1)
        {
            switch (baseType.dataName)
            {
                case DATA_NAME.DN_TEST:
                    Data_TEST test = new Data_TEST(data);
                    test.whom = CLIENT_NAME.CN_PC_1;
                    test.value[0]++;
                    print(test.value[0].ToString());
                    SendToServer(test.ToByteArray());


                    break;

                case DATA_NAME.DT_ROTATE:
                    Data_Transform point = new Data_Transform(data);
                    cube.transform.position = new Vector3(point.translation[0], point.translation[1], point.translation[2]);
                    cube.transform.rotation = Quaternion.Euler(point.rotation[0], point.rotation[1], point.rotation[2]);

                    //  Debug.Log(point.value[0].ToString() + point.value[1].ToString() + point.value[2].ToString());
                    //  C_KUN_POINT kun_point = new C_KUN_POINT((int)CLIENT_NAME.CN_UNITY_1, (int)DATA_TYPE.DT_POINT, 1.111, 2.222, 3.333);
                    //   SendToServer(kun_point.ToByteArray());

                    break;
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

                case DATA_NAME.DT_ASYMETRIC_ROTATE:
                    Data_Position3f deltaPos = new Data_Position3f(data);
                    Debug.Log("DT_ASYMETRIC_ROTATE");

                    if (obj_asymetric_operation != null)
                    {

                        obj_asymetric_operation.transform.Rotate(Vector3.down * deltaPos.value[0] / 5, Space.World);
                        obj_asymetric_operation.transform.Rotate(Vector3.right * deltaPos.value[1] / 5, Space.World);
                    }
                    break;

                case DATA_NAME.DT_ASYMETRIC_TRANSLATE:
                    Data_Position3f deltaTrans = new Data_Position3f(data);
                    if (obj_asymetric_operation != null)
                    {
                        obj_asymetric_operation.transform.Translate(Vector3.right * deltaTrans.value[0] / 1080 * 0.7f, Space.Self);
                        obj_asymetric_operation.transform.Translate(Vector3.up * deltaTrans.value[0] / 1080 * 0.7f, Space.Self);
                    }
                    break;

                case DATA_NAME.DT_ASYMETRIC_SCALE:
                    Data_Position3f deltaScale = new Data_Position3f(data);
                    if (obj_asymetric_operation != null)
                    {
                        Vector3 localScale = obj_asymetric_operation.transform.localScale;
                        Vector3 scale = new Vector3(localScale.x + deltaScale.value[0],
                                      localScale.y + deltaScale.value[0],
                                      localScale.z + deltaScale.value[0]);
                        //最小缩放到 0.3 倍  
                        if (scale.x > 0.3f && scale.y > 0.3f && scale.z > 0.3f)
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


        else
            Debug.Log("Unknown data");

    }


    // 	public void SetDic( Dictionary<DATA_TYPE, bool> dic)
    // 	{
    // 		this.dict = dic;
    // 	}

    //     public Dictionary<DATA_TYPE, bool> GetDic()
    // 	{
    // 		return dict;
    // 	}




    private Vector3 GetRelativePosition(Transform origin, Vector3 position)
    {
        Vector3 distance = position - origin.position;
        Vector3 relativePosition = Vector3.zero;
        relativePosition.x = Vector3.Dot(distance, origin.right.normalized);
        relativePosition.y = Vector3.Dot(distance, origin.up.normalized);
        relativePosition.z = Vector3.Dot(distance, origin.forward.normalized);
        return relativePosition;
    }


    public void setupSocket()
    {
        try
        {
            mySocket = new TcpClient(Host, Port);
            theStream = mySocket.GetStream();
            theWriter = new BinaryWriter(theStream);
            theReader = new BinaryReader(theStream);
            socketReady = true;
        }
        catch (Exception e)
        {
            Debug.Log("Socket error:" + e);
        }
    }

    public void SendToServer(byte[] data)
    {
        if (!socketReady)
            return;

        theWriter.Write(data, 0, data.Length);
        theWriter.Flush();
    }

    public byte[] readSocket()
    {
        if (!socketReady)
            return null;

        if (theStream.DataAvailable)
        {
            byte[] myReadBuffer = new byte[DEFAULT_BUFLEN];
            theStream.Read(myReadBuffer, 0, myReadBuffer.Length);
            return myReadBuffer;
        }

        return null;
    }

    public void closeSocket()
    {
        if (!socketReady)
            return;
        theWriter.Close();
        theReader.Close();
        mySocket.Close();
        socketReady = false;
    }

    //     public void maintainConnection()
    //     {
    //         if (!theStream.CanRead)
    //         {
    //             setupSocket();
    //         }
    //     }



}