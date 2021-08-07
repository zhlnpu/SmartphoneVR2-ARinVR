using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Net.Sockets;
using System;

public class CalibrationOculusRS : MonoBehaviour
{
    public GameObject centerEyeAnchor;


    public GameObject obj_ControllerBall;
    public GameObject obj_imageBall;

    public List<Vector3> controllerPointsList;
    public List<Vector3> RSPointsList;

    public int totalCalibrationPoints = 10;

    private TcpClient mySocket;
    private NetworkStream theStream;
    private BinaryWriter theWriter;
    private BinaryReader theReader;
    // private String Host = "192.168.191.1";	

    //1.1 change to IP of server
    public String Host = "127.0.0.1";
    public Int32 Port = 27015;
    public static int DEFAULT_BUFLEN = 128;

    public bool socketReady = false;
    C_VuforiaTrackingData vuforiaData = new C_VuforiaTrackingData(1);


    //  public GameObject ImageTargetCube;



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
            C_CommandBase cmd = new C_CommandBase(CLIENT_NAME_2.COMMAND_CONNECTION);

            SendToServer(cmd.ToByteArray());
        }

        else
        {
            Application.Quit();
        }


        vuforiaData.ToByteArray();
        SendToServer(vuforiaData.ret);

    }

    public double fps = 0;

    // Update is called once per frame
    void Update()
    {
        byte[] data = readSocket();
        if (data != null)
            ProcessingData(data);

        //  float revised_y =(float)( (0.205 - transform.localPosition.z) / 0.095 * 0.025);      

        //minorRevisio.transform.localPosition=new Vector3(0.0263f, revised_y   ,-0.0119f);

        fps = 1.0 / Time.deltaTime;
    }

    void OnApplicationQuit()
    {
        closeSocket();
    }
    public void ProcessingData(byte[] data)
    {
        vuforiaData.DecomposeData(data);

        transform.localPosition = new Vector3(vuforiaData.value[0], vuforiaData.value[1], vuforiaData.value[2]);
        transform.localRotation = new Quaternion(vuforiaData.value[3], vuforiaData.value[4], vuforiaData.value[5], vuforiaData.value[6]);


        vuforiaData.ToByteArray();
        SendToServer(data);

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
    // Start is called before the first frame update
    
    // Update is called once per frame

    private void OnGUI()
    {
        //  obj_objLeftControllerToEye.transform.position = obj_controllerCube.transform.position;
        //  obj_objLeftControllerToEye.transform.rotation = obj_controllerCube.transform.rotation;



        if (GUI.Button(new Rect(10, 10, 200, 30), "Record this point"))
        {
            //  if (controllerPointsList.Count >= totalCalibrationPoints)
            {
                RSPointsList.Add( transform.localPosition);

                //   GetRelativePosition

                Debug.Log("    obj_imageBall " + obj_imageBall.transform.localPosition.x);
                Debug.Log("    centerEyeAnchor " + centerEyeAnchor.transform.localPosition.x);
                Debug.Log("    obj_ControllerBall " + obj_ControllerBall.transform.localPosition.x);

                controllerPointsList.Add(GetRelativePosition(centerEyeAnchor.transform, obj_ControllerBall.transform.position));
                // controllerPointsList.Add( obj_ControllerBall.transform.position);

                FileStream fs1 = new FileStream("data.txt", FileMode.Create, FileAccess.Write);//创建写入文件          
                StreamWriter sw1 = new StreamWriter(fs1);
                for (int i = 0; i < RSPointsList.Count; i++)
                {
                    sw1.WriteLine(RSPointsList[i].x.ToString() + " " + RSPointsList[i].y.ToString() + " " + RSPointsList[i].z.ToString());
                }
                sw1.Close();
                fs1.Close();


                FileStream fs2 = new FileStream("model.txt", FileMode.Create, FileAccess.Write);//创建写入文件          
                StreamWriter sw2 = new StreamWriter(fs2);
                for (int i = 0; i < controllerPointsList.Count; i++)
                {
                    sw2.WriteLine(controllerPointsList[i].x.ToString() + " " + controllerPointsList[i].y.ToString() + " " + controllerPointsList[i].z.ToString());
                }
                sw2.Close();
                fs2.Close();

                FileStream fs3 = new FileStream("distance.txt", FileMode.Create, FileAccess.Write);//创建写入文件          
                StreamWriter sw3 = new StreamWriter(fs3);
                for (int i = 0; i < controllerPointsList.Count; i++)
                {
                    sw3.WriteLine(Vector3.Distance(RSPointsList[i], controllerPointsList[i]).ToString());

                }
                sw3.Close();
                fs3.Close();
            }
        }
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

public enum CLIENT_NAME_2
{
    COMMAND_CONNECTION = 0,
    CN_PC_1 = 101,
    CN_PC_2 = 102,
    CN_PC_3 = 103,
    CN_PC_4 = 104,
    CN_PC_5 = 105,

    CN_UNITY_1 = 201,
    CN_UNITY_2 = 202,
    CN_UNITY_3 = 203,
    CN_UNITY_4 = 204,
    CN_UNITY_5 = 205,

    CN_ANDROID_1 = 301,
    CN_ANDROID_2 = 302,
    CN_ANDROID_3 = 303,
    CN_ANDROID_4 = 304,
    CN_ANDROID_5 = 305,

    CN_HOLOLENS_1 = 401,
    CN_HOLOLENS_2 = 402,
    CN_HOLOLENS_3 = 403,
    CN_HOLOLENS_4 = 404,
    CN_HOLOLENS_5 = 405,
};

public enum DATA_NAME_2
{
    DN_TEST = 0,
    DT_ARDUINO = 1,
    DT_POWER = 2,
    DT_CHAOS = 3,
};


//kun
public enum DATA_TYPE_2
{
    DT_POINT = 0,
    DT_POINT3 = 1,
};


public class C_CommandBase
{
    public CLIENT_NAME_2 whom;
    //      public CLIENT_NAME_2 clientType;
    //   public DATA_TYPE_2 dataType;

    public virtual byte[] ToByteArray()
    {
        byte[] _commandType = BitConverter.GetBytes((Int32)whom);
        // byte[] _dataType = BitConverter.GetBytes((Int32)dataType);
        //byte[] _val = BitConverter.GetBytes((Int32)value);

        byte[] ret = new byte[_commandType.Length /*+ _dataType.Length*/];

        _commandType.CopyTo(ret, 0);
        //   _dataType.CopyTo(ret, _commandType.Length);
        //      r  _val.CopyTo(ret, _base.Length + _cmd.Length);

        return ret;
    }
    public C_CommandBase(CLIENT_NAME_2 _commandType)
    {
        whom = _commandType;
        // dataType = (DATA_TYPE_2)_dataType;
    }


    public C_CommandBase(byte[] data)
    {
        whom = (CLIENT_NAME_2)BitConverter.ToInt32(data, 0);
        //   dataType = (DATA_TYPE_2)BitConverter.ToInt32(_data, sizeof(Int32));

        //dataType = (DataName)BitConverter.ToInt32(data, byte_size());
    }

}

public class BaseDatatype : C_CommandBase
{
    public DATA_NAME_2 dataName;
    public DATA_TYPE_2 dataType;

    public int length = sizeof(CLIENT_NAME_2) + sizeof(DATA_NAME_2) + sizeof(DATA_TYPE_2);


    public BaseDatatype(CLIENT_NAME_2 __whom, DATA_NAME_2 __dataName, DATA_TYPE_2 __dataType) : base(__whom)
    {
        dataName = __dataName;
        dataType = __dataType;
    }

    public BaseDatatype(byte[] data) : base(data)
    {
        int pos = sizeof(CLIENT_NAME_2);
        int len = sizeof(DATA_NAME_2);
        dataName = (DATA_NAME_2)BitConverter.ToInt32(data, pos);
        pos += len;

        len = sizeof(DATA_TYPE_2);
        dataType = (DATA_TYPE_2)BitConverter.ToInt32(data, pos);
        pos += len;

    }

    public override byte[] ToByteArray()
    {
        byte[] byte_whom = base.ToByteArray();
        byte[] byte_dataName = BitConverter.GetBytes((int)dataName);
        byte[] byte_dataType = BitConverter.GetBytes((int)dataType);

        byte[] ret = new byte[byte_whom.Length + byte_dataName.Length + byte_dataType.Length];

        byte_whom.CopyTo(ret, 0);
        byte_dataName.CopyTo(ret, byte_whom.Length);
        byte_dataType.CopyTo(ret, byte_whom.Length + byte_dataName.Length);

        return ret;
    }
}

public class C_VuforiaTrackingData
{
    public byte[] ret = new byte[4 + sizeof(float) * 7];

    int prefix = 0;
    //add extra codes here
    public float[] value = new float[7];
    public void ToByteArray()
    {
        for (int i = 0; i < 7; i++)
        {
            (BitConverter.GetBytes(value[i])).CopyTo(ret, 4 + i * 4);
        }

    }

    public C_VuforiaTrackingData(int _prefix)
    {
        prefix = _prefix;

        byte[] _commandType = BitConverter.GetBytes((Int32)prefix);
        _commandType.CopyTo(ret, 0);


        for (int i = 0; i < 7; i++)
        {
            value[i] = 0;

        }

    }

    public C_VuforiaTrackingData(byte[] data)
    {
        for (int i = 0; i < 7; i++)
        {
            value[i] = BitConverter.ToSingle(data, 4 + i * 4);
        }
    }

    public void DecomposeData(byte[] data)
    {
        for (int i = 0; i < 7; i++)
        {
            value[i] = BitConverter.ToSingle(data, 4 + i * 4);
        }
    }

}



public class Converter
{
    public static byte[] ConvertToByte(float[] data)
    {

        byte[] ret = new byte[data.Length * sizeof(float)];
        for (int i = 0; i < data.Length; i++)
        {
            (BitConverter.GetBytes((float)data[i])).CopyTo(ret, i * sizeof(float));
        }
        return ret;
    }
    public static float[] ConvertToFloatArray(byte[] data, int index, int length)
    {
        float[] result = new float[length];
        for (int i = 0; i < length; i++)
        {
            result[i] = BitConverter.ToSingle(data, index + i * sizeof(float));
        }
        return result;
    }
    public static byte[] ConvertToByte(double[] data)
    {

        byte[] ret = new byte[data.Length * sizeof(double)];
        for (int i = 0; i < data.Length; i++)
        {
            (BitConverter.GetBytes((double)data[i])).CopyTo(ret, i * sizeof(double));
        }
        return ret;
    }
    public static double[] ConvertToDoubleArray(byte[] data, int index, int length)
    {
        double[] result = new double[length];
        for (int i = 0; i < length; i++)
        {
            result[i] = BitConverter.ToDouble(data, index + i * sizeof(double));
        }
        return result;
    }
    public static byte[] ConvertToByte(int[] data)
    {
        byte[] ret = new byte[data.Length * sizeof(Int32)];
        for (int i = 0; i < data.Length; i++)
        {
            (BitConverter.GetBytes((int)data[i])).CopyTo(ret, i * sizeof(Int32));
        }
        return ret;
    }
    public static int[] ConvertToIntArray(byte[] data, int index, int length)
    {
        int[] result = new int[length];
        for (int i = 0; i < length; i++)
        {
            result[i] = BitConverter.ToInt32(data, index + i * sizeof(Int32));
        }
        return result;
    }
}