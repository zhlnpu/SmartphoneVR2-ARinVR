//using UnityEngine;
//using ServerCommunication;


//public class TCPCompoument : MonoBehaviour
//{
//    //variables
//    private TCPConnection myTCP;
//    //private ModelingManager     myModelingManager;

//    //   public EmotionFeddback emotionalData = new EmotionFeddback();
//    public GameObject emptyCamera;

//    Data_Transform data_transform = new Data_Transform(CLIENT_NAME.CN_PC_1, DATA_NAME.EMPTYCAMERA);


//    void Start()
//    {

//        myTCP = gameObject.GetComponent<TCPConnection>();

//        //kun
//        //  C_KUN_POINT kun_point = new C_KUN_POINT((int)CLIENT_NAME.CN_UNITY_1, (int)DATA_TYPE.DT_POINT, 1.111, 2.222, 3.333);
//        //   myTCP.SendToServer(kun_point.ToByteArray());

//    }
//    void Update()
//    {
//        //keep checking the server for messages, if a message is received from server, it gets logged in the Debug console (see function below)
//        if (myTCP.socketReady == true)
//        {
//            //Vector3 rotation = emptyCamera.transform.rotation.eulerAngles;
//            //data_transform.rotation[0] = rotation.x;
//            //data_transform.rotation[1] = rotation.y;
//            //data_transform.rotation[2] = rotation.z;

        
//            //Vector3 translation = emptyCamera.transform.position;
//            //data_transform.translation[0] = translation.x;
//            //data_transform.translation[1] = translation.y;
//            //data_transform.translation[2] = translation.z;

//            //myTCP.SendToServer(data_transform.ToByteArray());
//        }
//    }

//    //void OnGUI()
//    //{
//    //    // if connection has not been made, display button to connect



//    //    if (GUI.Button(new Rect(10, 10, 100, 100), "send test message"))
//    //    {
//    //        Data_TEST test = new Data_TEST(CLIENT_NAME.CN_PC_1, DATA_NAME.DN_TEST);  

//    //        test.value[0] = 1.1f;
//    //        test.value[1] = 2.1f;
//    //        test.value[2] = 3.1f;


//    //        /*    C_Test test = new C_Test(CLIENT_NAME.CN_UNITY_1, (int)DATA_TYPE.DT_ARDUINO, 1);*/
//    //        myTCP.SendToServer(test.ToByteArray());

//    //    }


//    //}

  

//    /*
//        public void SetData(C_CommandBase data)
//        {
//            switch(data.dataType)
//            {
//                case DATA_TYPE.DT_ARDUINO:
//                    emotionalData.power = (C_Power)data;

//                    break;
//                    case DATA_TYPE.DT_POWER:
//                    emotionalData.arduino = (C_Arduino)data;

//                    break;
//                    case DATA_TYPE.DT_CHAOS:


//                    break;
//            }
        


//        }*/


//}


