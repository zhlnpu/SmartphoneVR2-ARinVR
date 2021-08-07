
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using ServerCommunication;
using BasicConfigs;
using UnityEngine.SceneManagement;

public class ProgressiveOperation : MonoBehaviour
{
    GUIStyle mustyule = new GUIStyle();
    
    
    public bool setRotation = true;
    public bool setTranslation = false;
    public bool setScale = false;

    private Touch oldTouch1;  //上次触摸点1(手指1)  
    private Touch oldTouch2;  //上次触摸点2(手指2)  


    Data_Position3f m_data_3f_rotate = new Data_Position3f(CLIENT_NAME.CN_ANDROID_1, DATA_NAME.DT_ASYMETRIC_ROTATE);

    Data_Position3f m_data_3f_translate = new Data_Position3f(CLIENT_NAME.CN_ANDROID_1, DATA_NAME.DT_ASYMETRIC_TRANSLATE);

    Data_Position3f m_data_3f_scale = new Data_Position3f(CLIENT_NAME.CN_ANDROID_1, DATA_NAME.DT_ASYMETRIC_SCALE);

    // Start is called before the first frame update
    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        mustyule.fontSize = 80;
    }

    string render = "";


    string ShowResult(float[] data)
    {
        return data[0].ToString() + " " + data[1].ToString() + " " + data[2].ToString();
    }

    void Update()
    {



        if (setRotation)
        {
            //单点触摸， 水平上下旋转  
            if (1 == Input.touchCount)
            {
                Touch touch = Input.GetTouch(0);
                Vector2 deltaPos = touch.deltaPosition;

                m_data_3f_rotate.value[0] = deltaPos.x;
                m_data_3f_rotate.value[1] = deltaPos.y;
                m_data_3f_rotate.value[2] = 0;

                //unit is pixel !!!
                BackgroundCommunication.instance.SendToServer(m_data_3f_rotate.ToByteArray());
                //data_trans = ShowResult(m_data_3f_rotate.value);
            }

            if (2 == Input.touchCount)
            {
                //多点触摸, 放大缩小  
                Touch newTouch1 = Input.GetTouch(0);
                Touch newTouch2 = Input.GetTouch(1);

                //第2点刚开始接触屏幕, 只记录，不做处理  
                if (newTouch2.phase == TouchPhase.Began)
                {
                    oldTouch2 = newTouch2;
                    oldTouch1 = newTouch1;
                    return;
                }

                //计算老的两点距离和新的两点间距离，变大要放大模型，变小要缩放模型  
                float oldDistance = Vector2.Distance(oldTouch1.position, oldTouch2.position);
                float newDistance = Vector2.Distance(newTouch1.position, newTouch2.position);

                //两个距离之差，为正表示放大手势， 为负表示缩小手势  
                float offset = newDistance - oldDistance;

                //放大因子， 一个像素按 0.01倍来算(100可调整)  
                float scaleFactor = offset;

                m_data_3f_rotate.value[0] = 0;
                m_data_3f_rotate.value[1] = 0;
                m_data_3f_rotate.value[2] = scaleFactor;

                BackgroundCommunication.instance.SendToServer(m_data_3f_rotate.ToByteArray());
                //data_trans = ShowResult(m_data_3f_rotate.value);


                //Vector3 localScale = transform.localScale;
                //Vector3 scale = new Vector3(localScale.x + scaleFactor,
                //              localScale.y + scaleFactor,
                //              localScale.z + scaleFactor);
                ////最小缩放到 0.3 倍  
                //if (scale.x > 0.3f && scale.y > 0.3f && scale.z > 0.3f)
                //{
                //    transform.localScale = scale;
                //}
                //记住最新的触摸点，下次使用  
                oldTouch1 = newTouch1;
                oldTouch2 = newTouch2;
            }
        }




        if (setTranslation)
        {
            //单点触摸， 水平上下旋转  
            if (1 == Input.touchCount)
            {
                Touch touch = Input.GetTouch(0);
                Vector2 deltaPos = touch.deltaPosition;

                m_data_3f_translate.value[0] = deltaPos.x;
                m_data_3f_translate.value[1] = deltaPos.y;
                m_data_3f_translate.value[2] = 0;

                BackgroundCommunication.instance.SendToServer(m_data_3f_translate.ToByteArray());
                //  data_trans = ShowResult(m_data_3f_rotate.value);


                //  transform.Translate(Vector3.right * deltaPos.x / 1080 * 0.7f, Space.World);
                // transform.Translate(Vector3.up * deltaPos.y / 1080 * 0.7f, Space.World);
            }

            if (2 == Input.touchCount)
            {

                //多点触摸, 放大缩小  
                Touch newTouch1 = Input.GetTouch(0);
                Touch newTouch2 = Input.GetTouch(1);

                //第2点刚开始接触屏幕, 只记录，不做处理  
                if (newTouch2.phase == TouchPhase.Began)
                {
                    oldTouch2 = newTouch2;
                    oldTouch1 = newTouch1;
                    return;
                }

                //计算老的两点距离和新的两点间距离，变大要放大模型，变小要缩放模型  
                float oldDistance = Vector2.Distance(oldTouch1.position, oldTouch2.position);
                float newDistance = Vector2.Distance(newTouch1.position, newTouch2.position);

                //两个距离之差，为正表示放大手势， 为负表示缩小手势  
                float offset = newDistance - oldDistance;

                //放大因子， 一个像素按 0.01倍来算(100可调整)  
                float scaleFactor = offset;

                m_data_3f_translate.value[0] = 0;
                m_data_3f_translate.value[1] = 0;
                m_data_3f_translate.value[2] = scaleFactor;

                BackgroundCommunication.instance.SendToServer(m_data_3f_translate.ToByteArray());
                //data_trans = ShowResult(m_data_3f_rotate.value);
                oldTouch1 = newTouch1;
                oldTouch2 = newTouch2;
            }
        }

        if (setScale)
        {
            if (2 == Input.touchCount)
            {
                //多点触摸, 放大缩小  
                Touch newTouch1 = Input.GetTouch(0);
                Touch newTouch2 = Input.GetTouch(1);

                //第2点刚开始接触屏幕, 只记录，不做处理  
                if (newTouch2.phase == TouchPhase.Began)
                {
                    oldTouch2 = newTouch2;
                    oldTouch1 = newTouch1;
                    return;
                }

                //计算老的两点距离和新的两点间距离，变大要放大模型，变小要缩放模型  
                float oldDistance = Vector2.Distance(oldTouch1.position, oldTouch2.position);
                float newDistance = Vector2.Distance(newTouch1.position, newTouch2.position);

                //两个距离之差，为正表示放大手势， 为负表示缩小手势  
                float offset = newDistance - oldDistance;

                //放大因子， 一个像素按 0.01倍来算(100可调整)  
                float scaleFactor = offset;

                m_data_3f_scale.value[0] = scaleFactor;
                m_data_3f_scale.value[1] = scaleFactor;
                m_data_3f_scale.value[2] = scaleFactor;

                BackgroundCommunication.instance.SendToServer(m_data_3f_scale.ToByteArray());
                //   data_scale = ShowResult(m_data_3f_scale.value);

                oldTouch1 = newTouch1;
                oldTouch2 = newTouch2;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnClickBack();
        }

    }

    //  public GameObject obj_asymetric_operation;

    //  public GameObject cam;
    //private string data_trans;
    //private string data_rot;
    //private string data_scale;

    private void OnGUI()
    {
        //    if (GUI.Button(new Rect(300, 100, 100, 100), "test"))
        //    {
        //        m_data_3f_translate.value[0] = 0.1f;
        //        m_data_3f_translate.value[1] = 0.1f;
        //        m_data_3f_translate.value[2] = 0;


        //        obj_asymetric_operation.transform.Translate(Vector3.right * m_data_3f_translate.value[0] , cam.transform);
        //        obj_asymetric_operation.transform.Translate(Vector3.up * m_data_3f_translate.value[1] , cam.transform);

        //    }



        //    GUI.Label(new Rect(400, 200, 500, 100), data_trans, mustyule);
        //    GUI.Label(new Rect(400, 400, 500, 100), data_rot, mustyule);
        //  GUI.Label(new Rect(400, 600, 500, 100), data_scale, mustyule);





        //if (GUI.Button(new Rect(300, 100, 100, 100), "test"))
        //{

        //    OnClickBack();
        //}

    }

    public void OnClick_Rotation()
    {
        setRotation = true;
        setTranslation = false;
        setScale = false;
    }
    public void OnClick_Translation()
    {
        setRotation = false;
        setTranslation = true;
        setScale = false;

    }
    public void OnClick_Scale()
    {
        setRotation = false;
        setTranslation = false;
        setScale = true;

    }



    private void OnClickBack()
    {
        BackgroundCommunication.instance.m_currentScene = Scenes.MAIN_MENU;
        SceneManager.LoadScene("Scene_mainMenu");
        Destroy(gameObject);
    }
}
