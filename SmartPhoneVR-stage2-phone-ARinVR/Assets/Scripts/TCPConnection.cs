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


    public GameObject emptyCamera;


    //定义所有涉及到的数据结构，接受解析时直接存储，
    //同时设置Get()函数，返回到主程序TCPCompoument里面

    // private Dictionary<DATA_TYPE, bool> dict = new Dictionary<DATA_TYPE, bool>();


    // Use this for initialization
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



        //   dict.Add(DATA_TYPE.DT_ARDUINO, false);
        /*     dict.Add(DATA_TYPE.DT_ARDUINO, false);
             dict.Add(DATA_TYPE.DT_ARDUINO, false);
             dict.Add(DATA_TYPE.DT_ARDUINO, false);*/


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
    }


    void OnApplicationQuit()
    {
        closeSocket();

    }

    public void ProcessingData(byte[] data)
    {
        //  C_CommandBase command = new C_CommandBase(data);
        C_CommandBase_Whom_Dataname baseType = new C_CommandBase_Whom_Dataname(data);


        if (baseType.whom == CLIENT_NAME.CN_PC_1)
        {
            switch (baseType.dataName)
            {
                case DATA_NAME.DN_TEST:
                    Data_TEST test = new Data_TEST(data);
                    test.whom = CLIENT_NAME.CN_ANDROID_1;

                    test.value[0]++;
                    print(test.value[0].ToString());

                    SendToServer(test.ToByteArray());
                    break;

                case DATA_NAME.EMPTYCAMERA:
                    Data_Transform emptycamera = new Data_Transform(data);

                    emptyCamera.transform.position = new Vector3(emptycamera.translation[0], emptycamera.translation[1], emptycamera.translation[2]);

                    emptyCamera.transform.rotation = Quaternion.Euler(emptycamera.rotation[0], emptycamera.rotation[1], emptycamera.rotation[2]);
                    //  Debug.Log(point.value[0].ToString() + point.value[1].ToString() + point.value[2].ToString());

                    //  C_KUN_POINT kun_point = new C_KUN_POINT((int)CLIENT_NAME.CN_UNITY_1, (int)DATA_TYPE.DT_POINT, 1.111, 2.222, 3.333);
                    //   SendToServer(kun_point.ToByteArray());
                    break;
                default:
                    Debug.LogError("Unknown datatype");
                    break;
            }
        }


        else
            Debug.LogError("Unknown data");

    }


    // 	public void SetDic( Dictionary<DATA_TYPE, bool> dic)
    // 	{
    // 		this.dict = dic;
    // 	}

    //     public Dictionary<DATA_TYPE, bool> GetDic()
    // 	{
    // 		return dict;
    // 	}







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