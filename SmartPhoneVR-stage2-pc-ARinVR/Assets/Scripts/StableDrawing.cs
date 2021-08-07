using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using ServerCommunication;



using System.IO; //输出误差txt
using Dest.Math;//数学库 Math library for unity 
using System.Diagnostics;
using System;

public class StableDrawing : MonoBehaviour
{
    public TCPIP_Base m_TCPIP_Base;

    GameObject obj_phone;


    Vector3[] path;
    private float time = 0;
    List<Vector3> m_lineRenderPoints = new List<Vector3>();
    List<List<Vector3>> poses = new List<List<Vector3>>();

    public List<LineRenderer> lineRenderers;
    //  public GameObject controller;

    List<GameObject> lineObjects = new List<GameObject>();


    LineRenderer m_controllerRay;


    public GameObject m_RightController;
    public GameObject m_LeftController;

    public GameObject m_RightController_Cylinder;
    public GameObject m_RightController_Ball;




    GameObject gameObjHit;







    public GameObject targetCircle; //@KJJ
    public GameObject targetSquare;  //KJJ

    public GameObject targetCircle1; //KJJ
    public GameObject targetSquare1;  //KJJ

    List<Vector3> pointslist;//笔画的信息
    List<float> timelist;//笔画的时间信息


    Circle3 tCircle;//KJJ
    Rectangle3 tSquare;//KJJ
    Circle3 tCircle1;//KJJ
    Rectangle3 tSquare1;//KJJ

    StreamWriter writer;  //KJJ
    StreamReader reader;  //KJJ
    List<string> allData;  //KJJ

    Stopwatch sw = new Stopwatch(); //计时器  //KJJ

    public GameObject m_lineColor;



    void Awake()
    {

        path = m_lineRenderPoints.ToArray();//转成数组


        m_currentMaterial = m_lineColor.GetComponent<MeshRenderer>().material;
        // m_currentMaterial = new Material(Shader.Find("Sprites/Default"));


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
                //   Debug.LogError("No baes tcpip class");
                ExitApp();
                return;
            }
        }
        if (m_TCPIP_Base == null)
        {
            //   Debug.LogError("PLease set base tcpip class");
            ExitApp();
            return;
        }

        m_TCPIP_Base.OnProcessingData += ProcessingData;


        path = m_lineRenderPoints.ToArray();//转成数组

        //   m_controllerRay = controller.AddComponent<LineRenderer>();

        //   m_controllerRay.SetWidth(0.002f, 0.002f);



        obj_phone =  GetChildWithName(m_RightController, "AlignmentOffset-PhoneV2"); ;

    }

    void Update()
    {


        /// 大范围笔画
        Vector3 center = targetCircle.transform.Find("center").transform.position; //KJJ
        Vector3 center1 = targetCircle.transform.Find("center1").transform.position; //KJJ
        Vector3 rad = targetCircle.transform.Find("edge").transform.position; //KJJ
        float radius = Vector3.Distance(tCircle.Center, rad);//KJJ
        Vector3 normal = (center - center1).normalized;//KJJ
        tCircle = new Circle3(center, normal, radius);//KJJ


        Vector3 vertex1 = targetSquare.transform.Find("Cube1").transform.position;//KJJ
        Vector3 vertex2 = targetSquare.transform.Find("Cube2").transform.position;//KJJ
        Vector3 vertex3 = targetSquare.transform.Find("Cube3").transform.position;//KJJ
        Vector3 vertex4 = targetSquare.transform.Find("Cube4").transform.position;//KJJ
        tSquare = Rectangle3.CreateFromCCWPoints(vertex1, vertex2, vertex3, vertex4);//KJJ 逆时针


        ///小范围笔画
        center = targetCircle1.transform.Find("center").transform.position; //KJJ
        center1 = targetCircle1.transform.Find("center1").transform.position; //KJJ
        rad = targetCircle1.transform.Find("edge").transform.position; //KJJ
        radius = Vector3.Distance(tCircle1.Center, rad);//KJJ
        normal = (center - center1).normalized;//KJJ
        tCircle1 = new Circle3(center, normal, radius);//KJJ
        //print(vertex1);
        //print(vertex2);
        //print(vertex3);
        //print(vertex4);
        vertex1 = targetSquare1.transform.Find("Cube1").transform.position;//KJJ
        vertex2 = targetSquare1.transform.Find("Cube2").transform.position;//KJJ
        vertex3 = targetSquare1.transform.Find("Cube3").transform.position;//KJJ
        vertex4 = targetSquare1.transform.Find("Cube4").transform.position;//KJJ
        tSquare1 = Rectangle3.CreateFromCCWPoints(vertex1, vertex2, vertex3, vertex4);//KJJ 逆时针







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


        //Ray ray = new Ray(controller.transform.position, controller.transform.forward);
        //RaycastHit hitInfo;

        ////   Debug.DrawLine(ray.origin,new  Vector3 (5,5,5));//划出射线，只有在scene视图中才能看到

        //m_controllerRay.positionCount = 2;//设置顶点数 
        //m_controllerRay.SetPosition(0, controller.transform.position);//设置顶点位置


        ////check if hits a target
        //if (Physics.Raycast(ray, out hitInfo))
        //{
        //    //  obj_pointer.transform.position = hitInfo.point;
        //    //  Debug.DrawLine(ray.origin, hitInfo.point);//划出射线，只有在scene视图中才能看到
        //    gameObjHit = hitInfo.collider.gameObject;
        //    //   Debug.Log("click object name is " + gameObj.name);

        //    if (gameObjHit != null)
        //    {
        //        m_controllerRay.SetPosition(1, hitInfo.point);//设置顶点位置

        //        if (gameObjHit.transform.parent.gameObject.name == "Palette")//当射线碰撞目标为boot类型的物品 ，执行拾取操作
        //        {
        //            objSelected = true;
        //        }
        //    }
        //    else
        //    {
        //        objSelected = false;
        //        //   m_controllerRay.SetPosition(1, controller.transform.forward * 1000);//设置顶点位置
        //    }
        //}
        //else
        //{
        //    m_controllerRay.SetPosition(1, controller.transform.forward * 1000);//设置顶点位置
        //}
        //if (objSelected)
        //{
        //    bool currentTriggerState = OVRInput.Get(OVRInput.RawButton.RIndexTrigger);

        //    if (!currentTriggerState && lastTriggerState)
        //    {

        //        //press trigger event
        //        m_currentMaterial = gameObjHit.GetComponent<MeshRenderer>().material;
        //    }
        //    lastTriggerState = currentTriggerState;
        //}




        //  Debug.Log( OVRInput.Get(OVRInput.RawButton.A));

        if (OVRInput.GetDown(OVRInput.RawButton.A))
        {
            if (m_lineWidth > 0.002)
                m_lineWidth -= 0.001f;

        }

        if (OVRInput.GetDown(OVRInput.RawButton.B))
        {
            if (m_lineWidth < 0.01)
                m_lineWidth += 0.001f;

        }



        if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger))
        {
            OnStartDrawing();
        }

        if (OVRInput.Get(OVRInput.RawButton.RIndexTrigger))
        {


         

            m_lineRenderPoints.Add(m_RightController_Ball.transform.position);

            ////                    pos.Add(GetRelativePosition(obj_phone.transform, new Vector3(x,0,z)));




            float stroketime = (float)sw.ElapsedMilliseconds / 1000f; //当前时间

            timelist.Add(stroketime);



        }

        if (OVRInput.GetUp(OVRInput.RawButton.RIndexTrigger))
        {
            OnEndDrawing();
        }


    }

    private Material m_currentMaterial;

    public float fps = 0;
    private bool objSelected;
    private bool lastTriggerState = false;
    private float m_lineWidth = 0.002f;

    bool touchActivated = false;


    public void ProcessingData(DATA_NAME dataName, byte[] data)
    {
        switch (dataName)
        {
            //add other processings here!
            // case DATA_NAME.DT_SPATIAL_INPUT_POINT:
            //      Data_Position3f touchpoint = new Data_Position3f(data);


            case DATA_NAME.DT_SPATIAL_INPUT_POINT_START:
                //  Debug.Log("DT_SPATIAL_INPUT_POINT_START");
                //    if (pos.Count > 0)

                OnStartDrawing();


                break;

            case DATA_NAME.DT_SPATIAL_INPUT_POINT_END:


                print("DT_SPATIAL_INPUT_POINT_END");

                Data_Position3f m_data_3f_end = new Data_Position3f(CLIENT_NAME.CN_ANDROID_2, DATA_NAME.DT_SPATIAL_INPUT_POINT_END);

                m_TCPIP_Base.SendToServer(m_data_3f_end.ToByteArray());


                OnEndDrawing();


             


                break;


            case DATA_NAME.DT_SPATIAL_INPUT_POINT:
                Data_Position3f touchpoint = new Data_Position3f(data);

                //                    pos.Add(new Vector3(touchpoint.value[0] / 1000.0f, touchpoint.value[1] / 1000.0f, touchpoint.value[2] / 1000.0f));

                float x;
                float z;
                if (m_RightController.activeSelf)
                {
                    x = (540 - touchpoint.value[1]) / 540 * 0.035f;
                    z = (touchpoint.value[0] - 1120) / 540 * 0.035f;
                }
                else
                {
                    x = (touchpoint.value[1] - 540) / 540 * 0.035f;
                    z = (1120 - touchpoint.value[0]) / 540 * 0.035f;
                }





             //   m_lineRenderPoints.Add(obj_phone.transform.TransformPoint(new Vector3(x, 0.005f, z)));
                m_lineRenderPoints.Add(obj_phone.transform.TransformPoint(new Vector3(x, 0, z)));

                //                    pos.Add(GetRelativePosition(obj_phone.transform, new Vector3(x,0,z)));




                float stroketime = (float)sw.ElapsedMilliseconds / 1000f; //当前时间

                timelist.Add(stroketime);


          //      fps = 1.0f / Time.deltaTime;
                break;



            default:
                //  Debug.LogError("Unknown datatype");
                break;
        }
    }

    private void OnEndDrawing()
    {
        if (touchActivated)
        {

            sw.Reset();//获取当前时间

            SaveData();
            touchActivated = false;
        }
    }

    private void OnStartDrawing()
    {
        {
            m_lineRenderPoints = new List<Vector3>();

            poses.Add(m_lineRenderPoints);

            GameObject newLine2 = new GameObject("Line");
            lineObjects.Add(newLine2);

            newLine2.transform.SetParent(gameObject.transform);

            LineRenderer lRend2 = newLine2.AddComponent<LineRenderer>();
            lRend2.SetWidth(m_lineWidth, m_lineWidth);
            lRend2.material = m_currentMaterial;

            lineRenderers.Add(lRend2);


            timelist = new List<float>(); //新建笔画采样点时间链表
            sw.Start(); //开始记录时间


            float stroketime1 = (float)sw.ElapsedMilliseconds / 1000f; //当前时间

            timelist.Add(stroketime1);

            touchActivated = true;



        }
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, 100, 100), "Circle Large"))
        {
            targetCircle.SetActive(true);
            targetCircle1.SetActive(false);
            targetSquare.SetActive(false);
            targetSquare1.SetActive(false);

        }

        if (GUI.Button(new Rect(0, 100, 100, 100), "Circle small"))
        {
            targetCircle.SetActive(false);
            targetCircle1.SetActive(true);
            targetSquare.SetActive(false);
            targetSquare1.SetActive(false);

        }


        if (GUI.Button(new Rect(0, 200, 100, 100), "square Large"))
        {
            targetCircle.SetActive(false);
            targetCircle1.SetActive(false);
            targetSquare.SetActive(true);
            targetSquare1.SetActive(false);

        }


        if (GUI.Button(new Rect(0, 300, 100, 100), "square small"))
        {
            targetCircle.SetActive(false);
            targetCircle1.SetActive(false);
            targetSquare.SetActive(false);
            targetSquare1.SetActive(true);

        }


        if (GUI.Button(new Rect(300, 0, 100, 100), "SAVE"))
        {
            SaveData();
        }


        if (GUI.Button(new Rect(200, 0, 100, 100), "Clear Lines"))
        {
            lineRenderers.Clear();
            poses.Clear();

            //            lineObjects.Clear();

            foreach (var temp in lineObjects)
                Destroy(temp);


        }


        if (GUI.Button(new Rect(100, 0, 100, 100), "Right Hand"))
        {
            OnClickRightHand();
        }

        if (GUI.Button(new Rect(100, 100, 100, 100), "Left Hand"))
        {
            OnClickLeftHand();
        }

        if (GUI.Button(new Rect(100, 200, 100, 100), "Controller"))
        {
            OnClickController();
        }
    }

    private void OnClickController()
    {
        m_LeftController.SetActive(false);
        m_RightController.SetActive(true);


        GetChildWithName(m_LeftController, "AlignmentOffset-PhoneV2").SetActive(false);
        GetChildWithName(m_RightController, "AlignmentOffset-PhoneV2").SetActive(false);


        m_RightController_Ball.SetActive(true);
        m_RightController_Cylinder.SetActive(true);

    }

    void OnClickRightHand()
    {
     

            m_LeftController.SetActive(false);
            m_RightController.SetActive(true);

        obj_phone = GetChildWithName(m_RightController, "AlignmentOffset-PhoneV2");

            GetChildWithName(m_LeftController, "AlignmentOffset-PhoneV2").SetActive(false);
            GetChildWithName(m_RightController, "AlignmentOffset-PhoneV2").SetActive(true);

            m_RightController_Ball.SetActive(false);
            m_RightController_Cylinder.SetActive(false);
       
    }


    void OnClickLeftHand()
    {

        m_LeftController.SetActive(true);
        m_RightController.SetActive(false);


        obj_phone = GetChildWithName(m_LeftController, "AlignmentOffset-PhoneV2");
        GetChildWithName(m_LeftController, "AlignmentOffset-PhoneV2").SetActive(true);
        GetChildWithName(m_RightController, "AlignmentOffset-PhoneV2").SetActive(false);

        m_RightController_Ball.SetActive(false);
        m_RightController_Cylinder.SetActive(false);
    }



    GameObject GetChildWithName(GameObject obj, string name)
    {
        Transform trans = obj.transform;
        Transform childTrans = trans.Find(name);
        if (childTrans != null)
        {
            return childTrans.gameObject;
        }
        else
        {
            return null;
        }
    }


    void SaveData()
    {

        // if (Input.GetKeyDown(KeyCode.S)) //KJJ 

        List<Vector3> ve3list = Medianfilter(m_lineRenderPoints, 6);//KJJ

        if (targetSquare.activeSelf == true)//KJJ 
        {
            float Accuracy = targetSquareAccuracy(ve3list, timelist, tSquare, "large");//KJJ 
        }
        else
        if (targetSquare1.activeSelf == true)//KJJ 
        {
            float Accuracy = targetSquareAccuracy(ve3list, timelist, tSquare1, "small");//KJJ 
        }
        else

        if (targetCircle.activeSelf == true)//KJJ 
        {
            float Accuracy = targetCircleAccuracy(ve3list, timelist, tCircle, "large"); //KJJ 
        }
        else
        if (targetCircle1.activeSelf == true)//KJJ 
        {
            float Accuracy = targetCircleAccuracy(ve3list, timelist, tCircle1, "small"); //KJJ 
        }
        else
        {

            UnityEngine.Debug.LogError(" Activate one model  ");
        }

    }


    void ExitApp()
    {
        //  Debug.LogError("Error Exit!");

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


    public float targetCircleAccuracy(List<Vector3> PointsList, List<float> timelist, Circle3 tCircle, string name)
    {

        FileInfo file = new FileInfo(Application.dataPath + "/../" + GetTime() + "CircleAccuracy_" + name + ".txt"); //存储到asset中，命名

        if (file.Exists)//存在txt，删除以前的内容
        {

            file.Delete();
            file.Refresh();//刷新对象的状态
        }

        WriteData("Type: Circle. Center: (" + tCircle.Center.x + "," + tCircle.Center.y + "," + tCircle.Center.z + "," + ")" + "  Radius: " + tCircle.Radius + "." + "  Area: " + tCircle.CalcArea(), file);



        //求得总的误差
        float sum = 0;//总误差
        for (int i = 0; i < PointsList.Count; i++)
        {
            Vector3 p = PointsList[i];
            float MinDis = Distance.Point3Circle3(ref p, ref tCircle, false);
            //  print("点" + i + " " + MinDis);

            sum = sum + MinDis;

            // print("MinDis  " + MinDis);
        }

        float AverageError = sum / PointsList.Count;  //平均误差

        WriteData("Average error:" + AverageError, file);

        allData = new List<string>();

        print("CircleAccuracy  " + AverageError);

        float time = timelist[timelist.Count - 1] - timelist[0];
        WriteData("drawing time(s):  " + time, file);
        WriteData("points number:  " + PointsList.Count, file);

        WriteData("points: x,y,z,t", file);

        for (int i = 0; i < PointsList.Count; i++)
        {
            WriteData((i + 1) + ": (" + PointsList[i].x + "," + PointsList[i].y + "," + PointsList[i].z + "," + timelist[i] + ")", file);

            allData = new List<string>();
        }

        return AverageError;

    }  //KJJ

    public float targetSquareAccuracy(List<Vector3> PointsList, List<float> timelist, Rectangle3 tSquare, string name)
    {

        print("targetSquareAccuracy");

        FileInfo file = new FileInfo(Application.dataPath + "/../" + GetTime() + "SquareAccuracy_" + name + ".txt"); //存储到asset中，命名

        if (file.Exists)//存在txt，删除以前的内容
        {
            file.Delete();

            file.Refresh();//刷新对象的状态
        }

        Vector3 v1, v2, v3, v4;
        tSquare.CalcVertices(out v1, out v2, out v3, out v4);

        WriteData("Type: Rectangle. vertex 1:(" + v1.x + "," + v1.y + "," + v1.z + ") " + "vertex 2: (" + v2.x + "," + v2.y + "," + v2.z + ") vertex 3: (" + v3.x + "," + v3.y + "," + v3.z + ")  vertex 4: (" + v4.x + "," + v4.y + "," + v4.z + ")" + "  Area: " + tCircle.CalcArea(), file);

        //总误差
        float sum = 0;//总误差
        for (int i = 0; i < PointsList.Count; i++)
        {
            Vector3 p = PointsList[i];
            float MinDis = Distance.Point3Rectangle3(ref p, ref tSquare);
            sum = sum + MinDis;
        }

        float AverageError = sum / PointsList.Count;  //平均误差

        WriteData("Average error:" + AverageError, file);

        allData = new List<string>();

        print("SquareAccuracy" + AverageError);

        float time = timelist[timelist.Count - 1] - timelist[0];
        WriteData("drawing time(s):  " + time, file);
        WriteData("points number:  " + PointsList.Count, file);

        WriteData("points: x,y,z,t", file);

        for (int i = 0; i < PointsList.Count; i++)
        {
            WriteData((i + 1) + ":(" + PointsList[i].x + "," + PointsList[i].y + "," + PointsList[i].z + "," + timelist[i] + ")", file);
            allData = new List<string>();
        }

        return AverageError;

    }  //KJJ

    string GetTime()
    {

        int hour;
        int minute;
        int second;
        int year;
        int month;
        int day;

        hour = DateTime.Now.Hour;
        minute = DateTime.Now.Minute;
        second = DateTime.Now.Second;
        year = DateTime.Now.Year;
        month = DateTime.Now.Month;
        day = DateTime.Now.Day;


        return string.Format("{0:D4}{1:D2}{2:D2}{3:D2}{4:D2}{5:D2}", year, month, day, hour, minute, second);


    }


    public void WriteData(string message, FileInfo file)
    {

        writer = file.AppendText();//创建一个StreamWriter，它向FileInfo的此实例表示的文件追加文本内容

        writer.WriteLine(message);
        writer.Flush();
        writer.Dispose();
        writer.Close();
    }  //KJJ

    private List<Vector3> Medianfilter(List<Vector3> Points_A, int w)  //输入list和窗口值，从(w+1)/2开始，到
    {
        Vector3[] Points = new Vector3[Points_A.Count];
        for (int i = 0; i < Points_A.Count; i++)
        {
            Points[i] = Points_A[i];
        }

        //List<Vector3> a_MF = new List<Vector3>();
        Vector3[] a_MF = new Vector3[Points_A.Count];
        Vector3[] window = new Vector3[w];

        Vector3[] result = new Vector3[Points_A.Count];
        //采样点数量小于w窗口值
        if (Points_A.Count < w / 2 + 1)
        {
            for (int j = 0; j < w; ++j)
                a_MF[j] = Points[j];
        }
        else
        {

            //首尾值处理
            for (int l = 0; l < Points_A.Count; l++)
            {
                if (l < w / 2 || l > Points_A.Count - w / 2 || l == Points_A.Count - w / 2)
                {
                    // a_MF.Add(Points[i]);

                    a_MF[l] = Points[l];
                }
                else
                {
                    //获得窗口数值
                    for (int i = w / 2; i < Points_A.Count - w / 2; ++i)
                    {

                        for (int j = 0; j < w; ++j)
                        {
                            window[j] = Points[i - w / 2 + j];
                        }

                        //窗口数值排序 x,y,z 分别排序   
                        for (int k = 0; k < w - 1; k++)//n个数的数列总共扫描n-1次
                        {
                            for (int j = 0; j < w - 1 - i; j++)//每一趟扫描到a[n-i-2]与a[n-i-1]比较为止结束
                            {
                                if (window[j].x > window[j + 1].x)//后一位数比前一位数小的话，就交换两个数的位置（升序）
                                {
                                    float t = window[j + 1].x;
                                    window[j + 1].x = window[j].x;
                                    window[j].x = t;
                                }
                                if (window[j].y > window[j + 1].y)//后一位数比前一位数小的话，就交换两个数的位置（升序）
                                {
                                    float t = window[j + 1].y;
                                    window[j + 1].y = window[j].y;
                                    window[j].y = t;
                                }
                                if (window[j].y > window[j + 1].y)//后一位数比前一位数小的话，就交换两个数的位置（升序）
                                {
                                    float t = window[j + 1].y;
                                    window[j + 1].y = window[j].y;
                                    window[j].y = t;
                                }
                            }
                        }

                        result[i].x = window[w / 2 + 1].x;
                        result[i].y = window[w / 2 + 1].y;
                        result[i].z = window[w / 2 + 1].z;
                        a_MF[i] = result[i];

                    }
                }
            }


        }
        List<Vector3> a_MF_A = new List<Vector3>();
        for (int i = 0; i < Points_A.Count; i++)
        {
            a_MF_A.Add(a_MF[i]);
        }

        return a_MF_A;
    }

}