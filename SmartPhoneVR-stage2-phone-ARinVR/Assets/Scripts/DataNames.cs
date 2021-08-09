using System;
using System.Collections.Generic;
using UnityEngine;

namespace ServerCommunication
{
    public enum CLIENT_NAME
    {
        COMMAND_CONNECTION = 0,
        CN_PC_1 = 101,
        CN_PC_2 = 102,
        CN_PC_3 = 103,
        CN_PC_4 = 104,
        CN_PC_5 = 105,

        //CN_UNITY_1 = 201,
        //CN_UNITY_2 = 202,
        //CN_UNITY_3 = 203,
        //CN_UNITY_4 = 204,
        //CN_UNITY_5 = 205,

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

    public enum DATA_NAME
    {
        DN_TEST,
        DT_ROTATE,
        EMPTYCAMERA,
        DT_SPATIAL_INPUT_POINT,
        DT_SPATIAL_INPUT_POINT_START,
        DT_SPATIAL_INPUT_POINT_END,
        DT_ASYMETRIC_ROTATE,
        DT_ASYMETRIC_TRANSLATE,
        DT_ASYMETRIC_SCALE,

        DT_MODEL_NUMBER,


        DT_VITUAL_BUTTON,

        DT_SCENE1,
        DT_SCENE3,

        DT_ONSCREEN_INPUT_SELECT_OBJ,
        DT_PHONE_INPUT,

        DT_ARINVR,

    }


    //     //kun
    //     public enum DATA_TYPE
    //     {
    //         DT_POINT = 0,
    //         DT_POINT3 = 1,
    //     };


    // 
    // 
    //     public class EmotionFeddback
    //     {
    //         public Dictionary<string, bool> dic = new Dictionary<string, bool>();
    // 
    //         public C_Power power;
    //         public C_Arduino arduino;
    // 
    //         public EmotionFeddback()
    //         {
    //             dic.Add("Received power", false);
    //             dic.Add("Received arduino", false);
    //         }
    //     }

    public class C_CommandBase_Whom
    {
        public CLIENT_NAME whom;
        //      public CLIENT_NAME clientType;
        //   public DATA_TYPE dataType;

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
        public C_CommandBase_Whom(CLIENT_NAME _commandType)
        {
            whom = _commandType;
            // dataType = (DATA_TYPE)_dataType;
        }


        public C_CommandBase_Whom(byte[] data)
        {
            whom = (CLIENT_NAME)BitConverter.ToInt32(data, 0);
            //   dataType = (DATA_TYPE)BitConverter.ToInt32(_data, sizeof(Int32));

            //dataType = (DataName)BitConverter.ToInt32(data, byte_size());
        }

    }

    public class C_CommandBase_Whom_Dataname : C_CommandBase_Whom
    {
        public DATA_NAME dataName;
        // public DATA_TYPE dataType;

        // public int length = sizeof(CLIENT_NAME) + sizeof(DATA_NAME) + sizeof(DATA_TYPE);
        public int length = sizeof(CLIENT_NAME) + sizeof(DATA_NAME);


        //  public BaseDatatype(CLIENT_NAME __whom, DATA_NAME __dataName, DATA_TYPE __dataType) : base(__whom)
        public C_CommandBase_Whom_Dataname(CLIENT_NAME __whom, DATA_NAME __dataName) : base(__whom)
        {
            dataName = __dataName;
            //   dataType = __dataType;
        }

        public C_CommandBase_Whom_Dataname(byte[] data) : base(data)
        {
            int pos = sizeof(CLIENT_NAME);
            int len = sizeof(DATA_NAME);
            dataName = (DATA_NAME)BitConverter.ToInt32(data, pos);
            pos += len;

            //             len = sizeof(DATA_TYPE);
            //             dataType = (DATA_TYPE)BitConverter.ToInt32(data, pos);
            //             pos += len;

        }

        public override byte[] ToByteArray()
        {
            byte[] byte_whom = base.ToByteArray();
            byte[] byte_dataName = BitConverter.GetBytes((int)dataName);
            // byte[] byte_dataType = BitConverter.GetBytes((int)dataType);

            //   byte[] ret = new byte[byte_whom.Length + byte_dataName.Length + byte_dataType.Length];
            byte[] ret = new byte[byte_whom.Length + byte_dataName.Length];

            byte_whom.CopyTo(ret, 0);
            byte_dataName.CopyTo(ret, byte_whom.Length);
            //  byte_dataType.CopyTo(ret, byte_whom.Length + byte_dataName.Length);

            return ret;
        }
    }


    public class Data_TEST : C_CommandBase_Whom_Dataname
    {
        //add extra codes here
        public float[] value = new float[3];
        public override byte[] ToByteArray()
        {
            byte[] byte_base = base.ToByteArray();
            byte[] byte_value = Converter.ConvertToByte(value);

            byte[] ret = new byte[byte_base.Length + byte_value.Length];

            byte_base.CopyTo(ret, 0);
            byte_value.CopyTo(ret, byte_base.Length);

            return ret;
        }

        public Data_TEST(byte[] data) : base(data)
        {
            value = Converter.ConvertToFloatArray(data, base.length, value.Length);
        }

        //       public Data_rotation(CLIENT_NAME __whom, DATA_NAME __dataName, DATA_TYPE __dataType) : base(__whom, __dataName, __dataType)
        public Data_TEST(CLIENT_NAME __whom, DATA_NAME __dataName) : base(__whom, __dataName)
        {

        }

    }

    public class Data_CharByte : C_CommandBase_Whom_Dataname
    {
        //add extra codes here
        public char[] value = new char[100];
        public override byte[] ToByteArray()
        {
            byte[] byte_base = base.ToByteArray();
            byte[] byte_value = Converter.ConvertToByte(value);

            byte[] ret = new byte[byte_base.Length + byte_value.Length];

            byte_base.CopyTo(ret, 0);
            byte_value.CopyTo(ret, byte_base.Length);

            return ret;
        }

        public Data_CharByte(byte[] data) : base(data)
        {
            value = Converter.ConvertTocharArray(data, base.length, value.Length);
        }

        //       public Data_rotation(CLIENT_NAME __whom, DATA_NAME __dataName, DATA_TYPE __dataType) : base(__whom, __dataName, __dataType)
        public Data_CharByte(CLIENT_NAME __whom, DATA_NAME __dataName) : base(__whom, __dataName)
        {

        }

    }

    public class Data_Position3f : C_CommandBase_Whom_Dataname
    {
        //add extra codes here
        public float[] value = new float[3];
        public override byte[] ToByteArray()
        {
            byte[] byte_base = base.ToByteArray();
            byte[] byte_value = Converter.ConvertToByte(value);

            byte[] ret = new byte[byte_base.Length + byte_value.Length];

            byte_base.CopyTo(ret, 0);
            byte_value.CopyTo(ret, byte_base.Length);

            return ret;
        }

        public Data_Position3f(byte[] data) : base(data)
        {
            value = Converter.ConvertToFloatArray(data, base.length, value.Length);
        }

        //       public Data_rotation(CLIENT_NAME __whom, DATA_NAME __dataName, DATA_TYPE __dataType) : base(__whom, __dataName, __dataType)
        public Data_Position3f(CLIENT_NAME __whom, DATA_NAME __dataName) : base(__whom, __dataName)
        {

        }

        public Data_Position3f(CLIENT_NAME __whom, DATA_NAME __dataName, Vector3 point) : base(__whom, __dataName)
        {
            value[0] = point.x;
            value[1] = point.y;
            value[2] = point.z;
        }

        public void SetValue(Vector3 point)
        {
            value[0] = point.x;
            value[1] = point.y;
            value[2] = point.z;

        }
    }




    public class Data_ModelNumber : C_CommandBase_Whom_Dataname
    {
        //add extra codes here
        public int value = -1;
        public override byte[] ToByteArray()
        {
            byte[] byte_base = base.ToByteArray();
            byte[] byte_value = Converter.ConvertToByte(value);

            byte[] ret = new byte[byte_base.Length + byte_value.Length];

            byte_base.CopyTo(ret, 0);
            byte_value.CopyTo(ret, byte_base.Length);

            return ret;
        }

        public Data_ModelNumber(byte[] data) : base(data)
        {
            value = Converter.ConvertToInt(data, base.length);
        }

        //       public Data_rotation(CLIENT_NAME __whom, DATA_NAME __dataName, DATA_TYPE __dataType) : base(__whom, __dataName, __dataType)
        public Data_ModelNumber(CLIENT_NAME __whom, DATA_NAME __dataName) : base(__whom, __dataName)
        {

        }

        public Data_ModelNumber(CLIENT_NAME __whom, DATA_NAME __dataName, int _value) : base(__whom, __dataName)
        {
            value = _value;
        }

        public void SetValue(int _value)
        {
            value = _value;
        }
    }


















    public class Data_Transform : C_CommandBase_Whom_Dataname
    {
        //add extra codes here
        public float[] translation = new float[3];
        public float[] rotation = new float[3];
        public override byte[] ToByteArray()
        {
            byte[] byte_base = base.ToByteArray();
            byte[] byte_translation = Converter.ConvertToByte(translation);
            byte[] byte_rotation = Converter.ConvertToByte(rotation);

            byte[] ret = new byte[byte_base.Length + byte_translation.Length + byte_rotation.Length];

            byte_base.CopyTo(ret, 0);
            byte_translation.CopyTo(ret, byte_base.Length);
            byte_rotation.CopyTo(ret, byte_base.Length + byte_translation.Length);

            return ret;
        }

        public Data_Transform(byte[] data) : base(data)
        {
            translation = Converter.ConvertToFloatArray(data, base.length, translation.Length);
            rotation = Converter.ConvertToFloatArray(data, base.length + sizeof(float) * translation.Length, rotation.Length);

        }

        //       public Data_rotation(CLIENT_NAME __whom, DATA_NAME __dataName, DATA_TYPE __dataType) : base(__whom, __dataName, __dataType)
        public Data_Transform(CLIENT_NAME __whom, DATA_NAME __dataName) : base(__whom, __dataName)
        {

        }
    }



    public class Data_VirtualButton : C_CommandBase_Whom_Dataname
    {
        public bool joystickEnabled = false;
        public float[] joystickDirection = new float[2];
        public float[] joystickMagnitude = new float[1];
        public bool gripEnabled = false;

        public override byte[] ToByteArray()
        {
            byte[] byte_base = base.ToByteArray();
            byte[] byte_joystickEnabled = Converter.ConvertToByte(joystickEnabled);
            byte[] byte_joystickDirection = Converter.ConvertToByte(joystickDirection);
            byte[] byte_joystickMagnitude = Converter.ConvertToByte(joystickMagnitude);
            byte[] byte_gripEnabled = Converter.ConvertToByte(gripEnabled);

            byte[] ret = new byte[byte_base.Length + byte_joystickEnabled.Length + byte_joystickDirection.Length + byte_joystickMagnitude.Length + byte_gripEnabled.Length];

            byte_base.CopyTo(ret, 0);
            byte_joystickEnabled.CopyTo(ret, byte_base.Length);
            byte_joystickDirection.CopyTo(ret, byte_base.Length + byte_joystickEnabled.Length);
            byte_joystickMagnitude.CopyTo(ret, byte_base.Length + byte_joystickEnabled.Length + byte_joystickDirection.Length);
            byte_gripEnabled.CopyTo(ret, byte_base.Length + byte_joystickEnabled.Length + byte_joystickDirection.Length + byte_joystickMagnitude.Length);

            return ret;
        }

        public Data_VirtualButton(byte[] data) : base(data)
        {
            joystickEnabled = Converter.ConvertToBool(data, base.length);
            joystickDirection = Converter.ConvertToFloatArray(data, base.length + sizeof(bool), joystickDirection.Length);
            joystickMagnitude = Converter.ConvertToFloatArray(data, base.length + sizeof(bool) + sizeof(float) * joystickDirection.Length, joystickMagnitude.Length);
            gripEnabled = Converter.ConvertToBool(data, base.length + sizeof(bool) + sizeof(float) * joystickDirection.Length + sizeof(float) * joystickMagnitude.Length);

        }

        //       public Data_rotation(CLIENT_NAME __whom, DATA_NAME __dataName, DATA_TYPE __dataType) : base(__whom, __dataName, __dataType)
        public Data_VirtualButton(CLIENT_NAME __whom, DATA_NAME __dataName) : base(__whom, __dataName)
        {

        }
    }





    public class Data_ARINVR : C_CommandBase_Whom_Dataname
    {
        //add extra codes here
        //相机与码的相对位置
        //所有物体相对于码的位置，7个物体
        //共计8个位置旋转参数

        public float[] pos = new float[56];

        public override byte[] ToByteArray()
        {
            byte[] byte_base = base.ToByteArray();
            byte[] byte_pos = Converter.ConvertToByte(pos);

            byte[] ret = new byte[byte_base.Length + byte_pos.Length];

            byte_base.CopyTo(ret, 0);
            byte_pos.CopyTo(ret, byte_base.Length);

            return ret;
        }

        public Data_ARINVR(byte[] data) : base(data)
        {
            pos = Converter.ConvertToFloatArray(data, base.length, pos.Length);

        }

        //       public Data_rotation(CLIENT_NAME __whom, DATA_NAME __dataName, DATA_TYPE __dataType) : base(__whom, __dataName, __dataType)
        public Data_ARINVR(CLIENT_NAME __whom, DATA_NAME __dataName) : base(__whom, __dataName)
        {

        }

     public   void SetData(Transform cam, GameObject [] objs)
        {

            pos[0] = cam.position.x;
            pos[1] = cam.position.y;
            pos[2] = cam.position.z;
            pos[3] = cam.rotation.x;
            pos[4] = cam.rotation.y;
            pos[5] = cam.rotation.z;
            pos[6] = cam.rotation.w;

            for (int i = 1; i <= objs.Length; i++)
            {
                pos[i * 7 + 0] = objs[i-1].transform.position.x;
                pos[i * 7 + 1] = objs[i-1].transform.position.y;
                pos[i * 7 + 2] = objs[i-1].transform.position.z;
                pos[i * 7 + 3] = objs[i-1].transform.rotation.x;
                pos[i * 7 + 4] = objs[i-1].transform.rotation.y;
                pos[i * 7 + 5] = objs[i-1].transform.rotation.z;
                pos[i * 7 + 6] = objs[i-1].transform.rotation.w;
            }
        }



        public void SetData(Vector3 start, Vector3 end, GameObject[] objs)
        {

            pos[0] = start.x;
            pos[1] = start.y;
            pos[2] = start.z;
            pos[3] = end.x;
            pos[4] = end.y;
            pos[5] = end.z;

            for (int i = 1; i <= objs.Length; i++)
            {
                pos[i * 7 + 0] = objs[i - 1].transform.position.x;
                pos[i * 7 + 1] = objs[i - 1].transform.position.y;
                pos[i * 7 + 2] = objs[i - 1].transform.position.z;
                pos[i * 7 + 3] = objs[i - 1].transform.rotation.x;
                pos[i * 7 + 4] = objs[i - 1].transform.rotation.y;
                pos[i * 7 + 5] = objs[i - 1].transform.rotation.z;
                pos[i * 7 + 6] = objs[i - 1].transform.rotation.w;
            }
        }
    }





















    public class Converter
    {
        public static byte[] ConvertToByte(char[] data)
        {

            byte[] ret = new byte[data.Length * sizeof(char)];
            for (int i = 0; i < data.Length; i++)
            {
                (BitConverter.GetBytes((char)data[i])).CopyTo(ret, i * sizeof(char));
            }
            return ret;
        }

        public static char[] ConvertTocharArray(byte[] data, int index, int length)
        {
            char[] result = new char[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = BitConverter.ToChar(data, index + i * sizeof(char));
            }
            return result;
        }

        public static byte[] ConvertToByte(bool data)
        {
            byte[] ret = new byte[sizeof(bool)];

            (BitConverter.GetBytes((bool)data)).CopyTo(ret, 0);

            return ret;
        }

        public static bool ConvertToBool(byte[] data, int index)
        {
            return BitConverter.ToBoolean(data, index);
        }

        public static byte[] ConvertToByte(int data)
        {
            byte[] ret = new byte[sizeof(Int32)];

            (BitConverter.GetBytes((int)data)).CopyTo(ret, 0);

            return ret;
        }

        public static int ConvertToInt(byte[] data, int index)
        {

            return BitConverter.ToInt32(data, index);

        }

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
}


