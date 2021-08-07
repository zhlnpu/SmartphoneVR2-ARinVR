using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.IO;
using System;

using ServerCommunication;

public class TCPIP_Base : MonoBehaviour
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

    public CLIENT_NAME m_thisClientName;
    public CLIENT_NAME  m_processDataFrom;

    public Action<DATA_NAME ,byte []> OnProcessingData;

    public bool m_testCommunication;


    void Start()
    {
        if(m_thisClientName==CLIENT_NAME.COMMAND_CONNECTION  || m_processDataFrom == CLIENT_NAME.COMMAND_CONNECTION)
        {
            Debug.LogError("PLease set this client name");
            ExitApp();
            return;
        }

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
            Debug.LogError("Cannot setup socket connection!");

            ExitApp();
            return;
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

    int m_testMax = 10;


    // Update is called once per frame
    void Update()
    {
        if(m_testCommunication)
        {
            m_testMax = 10;
            Data_TEST test = new Data_TEST( m_thisClientName, DATA_NAME.DN_TEST);
            test.whom = m_thisClientName;          
                SendToServer(test.ToByteArray());
            m_testCommunication = false;
        }

        if (socketReady)
        {
            byte[] data = readSocket();
            if (data != null)
            {
                C_CommandBase_Whom_Dataname baseType = new C_CommandBase_Whom_Dataname(data);

                if (baseType.whom == m_thisClientName)
                {
                    Debug.LogError("The same client name!");
                    ExitApp();
                    return;
                }

                    if (baseType.whom == m_processDataFrom)
                {
                    if (baseType.dataName == DATA_NAME.DN_TEST)
                    {
                        Data_TEST test = new Data_TEST(data);
                        test.whom = m_thisClientName;
                        Debug.Log("Received test data: " + test.value[0].ToString());
                        if (test.value[0] < m_testMax)
                        {
                            test.value[0]++;
                            SendToServer(test.ToByteArray());
                        }
                    }
                    else
                    {
                        OnProcessingData(baseType.dataName, data);
                    }
                }
            }
        }
    }

    void OnApplicationQuit()
    {
        closeSocket();
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