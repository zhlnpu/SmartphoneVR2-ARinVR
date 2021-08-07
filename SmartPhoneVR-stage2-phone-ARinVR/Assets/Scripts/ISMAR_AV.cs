using BasicConfigs;
using ServerCommunication;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


/*this is a mixed scenario including:
1. virtual buton
2. 2D-3D content transfer
3. Augmented virtuality
4. VR phone
5. text input

*/
public class ISMAR_AV : MonoBehaviour
{




    [Header("AV Settings")]
    public GameObject rawImage;
    static WebCamTexture cam;


    public bool setRotation = true;
    public bool setTranslation = false;
    public bool setScale = false;

    private Touch oldTouch1;  //上次触摸点1(手指1)  
    private Touch oldTouch2;  //上次触摸点2(手指2)  


    Data_Position3f m_data_3f_rotate = new Data_Position3f(CLIENT_NAME.CN_ANDROID_1, DATA_NAME.DT_ASYMETRIC_ROTATE);

    Data_Position3f m_data_3f_translate = new Data_Position3f(CLIENT_NAME.CN_ANDROID_1, DATA_NAME.DT_ASYMETRIC_TRANSLATE);

    Data_Position3f m_data_3f_scale = new Data_Position3f(CLIENT_NAME.CN_ANDROID_1, DATA_NAME.DT_ASYMETRIC_SCALE);

    public GameObject canvasRight;
    public GameObject canvasLeft;


    //  public GameObject m_planImage;
    // Start is called before the first frame update
    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;

        BackgroundCommunication.instance.OnProcessingData += ProcessingData;


        if (cam == null)
        {
            cam = new WebCamTexture();
        }


        //Color[] c = mTexture.GetPixels(0, 0, 200, 200);
        //Texture2D m2Texture = new Texture2D(200, 200);
        //m2Texture.SetPixels(c);
        //m2Texture.Apply();
        //gameObject.GetComponent<MeshRenderer>().material.mainTexture = m2Texture;

        if (!cam.isPlaying)
            cam.Play();



        rawImage.GetComponent<RawImage>().material.mainTexture = cam;


        
        Vector2  imageSize=       rawImage.GetComponent<RectTransform>().rect.size;

        if(imageSize.x/imageSize.y>(1.0f*cam.width/cam.height))
        {
            rawImage.GetComponent<RectTransform>()  .sizeDelta =new Vector2(imageSize.x,   imageSize.x*cam.height/cam.width    );
        }
        else
        {
            rawImage.GetComponent<RectTransform>().sizeDelta = new Vector2( imageSize.y * cam.width / cam.height,imageSize.y);


        }


//        print(cam.width);

    }

    public float fps = 0;
  

    private void Update()
    {
        fps = 1.0f / Time.deltaTime;

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





    public void ProcessingData(DATA_NAME dataName, byte[] data)
    {
        switch (dataName)
        {
            case DATA_NAME.DT_MODEL_NUMBER:
                Debug.Log("DT_MODEL_NUMBER");

                Data_ModelNumber data_leftOrRight = new Data_ModelNumber(data);

                if (data_leftOrRight.value == 0)
                {
                    OnClickRightHand();
                }
                else
                {
                    if (data_leftOrRight.value == 1)
                    {
                        OnClickLeftHand();
                    }
                }

                break;


            default:
                Debug.LogError("Unknown datatype");
                break;
        }
    }

    private void OnClickLeftHand()
    {
        Screen.orientation = ScreenOrientation.LandscapeRight;
        canvasLeft.SetActive(true);
        canvasRight.SetActive(false);

        rawImage.GetComponent<RectTransform>().Rotate(0, 0, 180);


    }

    private void OnClickRightHand()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;


        canvasRight.SetActive(true);
        canvasLeft.SetActive(false);
        rawImage.GetComponent<RectTransform>().Rotate(0, 0,180);



    }

    private void OnGUI()
    {

    //    GUI.Label(new Rect(500, 500, 300, 300), fps.ToString());


        //if (Input.touchCount > 0)
        //{
        //    GUI.Label(new Rect(10, 10, 300, 300), Input.GetTouch(0).position.x.ToString() + "  " + Input.GetTouch(0).position.x.ToString(), mustyule);

        //}

    }



    private void OnClickBack()
    {
        //BackgroundCommunication.instance.m_currentScene = Scenes.MAIN_MENU;
        // SceneManager.LoadScene("Scene_mainMenu");
        // Destroy(gameObject);

        AppExit();

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

    void AppExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif        
    }
}

