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
public class Scene3 : MonoBehaviour
{

    GUIStyle mustyule = new GUIStyle();


    [Header("Canvas Settings")]
    public GameObject m_canvasVirtualButton;
    public GameObject m_canvasContentTransfer;
    public GameObject m_canvasAV;



    [Header("Button remapping Settings")]
    public GameObject m_smallCircle;

    public GameObject m_largeCircle;

    public GameObject m_gripButton;

    Vector2 offset;

    public bool m_buttonTouched = false;
    // public Vector3 mousepos;

    Vector2 m_initialPosSmallCircle;

    Vector2 uiPos;

    // 0~1, explains the eccentricity
    float joystickMag = 0;


    [Header("Content transfer Settings")]
    public GameObject[] m_images;
    GameObject m_currentImage;

    public GameObject m_VRSpace;
    public bool imagetouched = false;
    // public Vector3 mousepos;
    Vector2 m_initialPos;
    int index = -1;

    [Header("AV Settings")]
    public GameObject rawImage;
    static WebCamTexture cam;


    public enum CURRENT_SCENR
    {
        CONTENT_TRANSFER,
        BUTTON_REMAPPING,
        AV,
        TEXT,
        PHONE_ONLY,

    }

    public CURRENT_SCENR m_currentScene = CURRENT_SCENR.BUTTON_REMAPPING;



    // Start is called before the first frame update
    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;

        m_initialPosSmallCircle = m_smallCircle.GetComponent<RectTransform>().anchoredPosition;

        if (cam == null)
        {
            cam = new WebCamTexture();
        }

        rawImage.GetComponent<RawImage>().material.mainTexture = cam;



    }

public    float fps = 0;
    bool lastButtonTouched = false;
    bool lastGripButtonPressed = false;
    bool sendGripButtonInfo = false;

    private void Update()
    {
        fps = 1.0f / Time.deltaTime;

        switch (m_currentScene)
        {
            case CURRENT_SCENR.CONTENT_TRANSFER:
                m_canvasVirtualButton.SetActive(false);
                m_canvasContentTransfer.SetActive(true);
                m_canvasAV.SetActive(false);

                if (cam.isPlaying)
                    cam.Stop();

                break;
            case CURRENT_SCENR.BUTTON_REMAPPING:
                m_canvasVirtualButton.SetActive(true);
                m_canvasContentTransfer.SetActive(false);
                m_canvasAV.SetActive(false);
                if (cam.isPlaying)
                    cam.Stop();

                break;
            case CURRENT_SCENR.AV:
                m_canvasVirtualButton.SetActive(false);
                m_canvasContentTransfer.SetActive(false);
                m_canvasAV.SetActive(true);

                if (!cam.isPlaying)
                    cam.Play();

                break;
            case CURRENT_SCENR.TEXT:
                break;
            case CURRENT_SCENR.PHONE_ONLY:
                break;
            default:
                break;
        }



        switch (m_currentScene)
        {
            case CURRENT_SCENR.CONTENT_TRANSFER:
                {
                    if (imagetouched)
                    {
                        //    Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);//屏幕坐标转换世界坐标
                        //   Vector2 uiPos = canvas.transform.InverseTransformPoint(worldPos);
                        m_currentImage.GetComponent<RectTransform>().anchoredPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y) + offset;
                    }

                    if (Input.GetMouseButtonDown(0))
                    {
                        for (int i = 0; i < m_images.Length; i++)
                        {
                            if (TouchImageRect(Input.mousePosition.x, Input.mousePosition.y, m_images[i]))
                            {
                                imagetouched = true;
                                m_currentImage = m_images[i];
                                m_initialPos = m_currentImage.GetComponent<RectTransform>().anchoredPosition;
                                offset = new Vector2(m_initialPos.x - Input.mousePosition.x, m_initialPos.y - Input.mousePosition.y);
                                index = i;
                                Debug.Log("index= " + index.ToString());
                                break;
                            }
                        }
                    }

                    //release
                    if (imagetouched && Input.GetMouseButtonUp(0))
                    {
                        if (DragedIntoVRSpace(m_currentImage))
                        {
                            Debug.Log("into VR");
                            Data_ModelNumber modelNumber = new Data_ModelNumber(BackgroundCommunication.instance.m_thisClientName, DATA_NAME.DT_MODEL_NUMBER, index);
                            BackgroundCommunication.instance.SendToServer(modelNumber.ToByteArray());
                        }
                        m_currentImage.GetComponent<RectTransform>().anchoredPosition = m_initialPos;
                        imagetouched = false;
                    }







                }
                break;
            case CURRENT_SCENR.BUTTON_REMAPPING:
                {
                    lastButtonTouched = m_buttonTouched;
                    if (m_buttonTouched)
                    {
                        uiPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y) + offset;

                        if (TouchImageCircle(uiPos, m_largeCircle))
                        {
                            m_smallCircle.GetComponent<RectTransform>().anchoredPosition = uiPos;
                            joystickMag = Vector2.Distance(uiPos, m_initialPosSmallCircle) / m_largeCircle.GetComponent<RectTransform>().sizeDelta.x * 2;
                            if (joystickMag > 1)
                                joystickMag = 1;
                        }
                        else
                        {
                            m_smallCircle.GetComponent<RectTransform>().anchoredPosition = m_initialPosSmallCircle + (uiPos - m_initialPosSmallCircle).normalized * (m_largeCircle.GetComponent<RectTransform>().sizeDelta.x / 2);
                            joystickMag = 1;
                        }
                    }

                    if (Input.GetMouseButtonDown(0))
                    {
                        if (TouchImageCircle(Input.mousePosition.x, Input.mousePosition.y, m_smallCircle))
                        {
                            m_buttonTouched = true;
                            offset = new Vector2(m_initialPosSmallCircle.x - Input.mousePosition.x, m_initialPosSmallCircle.y - Input.mousePosition.y);
                        }
                    }

                    //release
                    if (m_buttonTouched && Input.GetMouseButtonUp(0))
                    {
                        m_smallCircle.GetComponent<RectTransform>().anchoredPosition = m_initialPosSmallCircle;
                        m_buttonTouched = false;
                        uiPos = m_initialPosSmallCircle;
                    }
                    //  if(imagetouched || m_gripButton.GetComponent<HoldButtonDetection>().isPointDown)


               

                    if (m_gripButton.GetComponent<HoldButtonDetection>().isPointDown)
                    {
                        if (!lastGripButtonPressed)
                        {
                            sendGripButtonInfo = true;
                        }
                        lastGripButtonPressed = true;
                    }
                    else
                    {
                        if (lastGripButtonPressed)
                        {
                            sendGripButtonInfo = true;
                        }
                        lastGripButtonPressed = false;
                    }



                    if (m_buttonTouched  || lastButtonTouched  || sendGripButtonInfo)
                    {

                        Data_VirtualButton data = new Data_VirtualButton(BackgroundCommunication.instance.m_thisClientName, DATA_NAME.DT_VITUAL_BUTTON);

                        data.joystickEnabled = m_buttonTouched;
                        data.joystickDirection[0] = (uiPos - m_initialPosSmallCircle).normalized.x * joystickMag;
                        data.joystickDirection[1] = (uiPos - m_initialPosSmallCircle).normalized.y * joystickMag;
                        data.joystickMagnitude[0] = joystickMag;
                        data.gripEnabled = m_gripButton.GetComponent<HoldButtonDetection>().isPointDown;

                        BackgroundCommunication.instance.SendToServer(data.ToByteArray());


                        sendGripButtonInfo = false;
                    }



                }
                break;
            case CURRENT_SCENR.AV:
                {




                }
                break;
            case CURRENT_SCENR.TEXT:
                {



                }
                break;
            case CURRENT_SCENR.PHONE_ONLY:
                {


                }
                break;
            default:
                break;
        }






        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnClickBack();
        }
    }



    private bool DragedIntoVRSpace(GameObject currentImage)
    {
        return TouchImageRect(currentImage.GetComponent<RectTransform>().anchoredPosition, m_VRSpace);
    }




    private bool TouchImageRect(Vector2 vec, GameObject image1)
    {
        return TouchImageRect(vec.x, vec.y, image1);
    }


    private bool TouchImageRect(float x, float y, GameObject image1)
    {

        Vector2 anchor = image1.GetComponent<RectTransform>().anchoredPosition;
        Vector2 size = image1.GetComponent<RectTransform>().sizeDelta;

        if (Mathf.Abs(anchor.x - x) < size.x / 2 && Mathf.Abs(anchor.y - y) < size.y / 2)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    private bool TouchImageCircle(Vector2 vec, GameObject image1)
    {
        return TouchImageCircle(vec.x, vec.y, image1);
    }
    private bool TouchImageCircle(float x, float y, GameObject image1)
    {

        Vector2 anchor = image1.GetComponent<RectTransform>().anchoredPosition;
        Vector2 size = image1.GetComponent<RectTransform>().sizeDelta;

        if (Mathf.Sqrt(Mathf.Pow(anchor.x - x, 2) + Mathf.Pow(anchor.y - y, 2)) < size.y / 2)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    private void OnGUI()
    {
        mustyule.fontSize = 20;

        if (Input.touchCount > 0)
        {
            GUI.Label(new Rect(10, 10, 300, 300), Input.GetTouch(0).position.x.ToString() + "  " + Input.GetTouch(0).position.x.ToString(), mustyule);

        }

    }



    private void OnClickBack()
    {
        //BackgroundCommunication.instance.m_currentScene = Scenes.MAIN_MENU;
        // SceneManager.LoadScene("Scene_mainMenu");
        // Destroy(gameObject);

        AppExit();

    }


    public void OnClickVirtaulButtonRemapping()
    {
        m_currentScene = CURRENT_SCENR.BUTTON_REMAPPING;


        Data_ModelNumber sceneID = new Data_ModelNumber(BackgroundCommunication.instance.m_thisClientName, DATA_NAME.DT_SCENE3, (int)m_currentScene);
        BackgroundCommunication.instance.SendToServer(sceneID.ToByteArray());
    }

    public void OnClickContentTransfer()
    {
        m_currentScene = CURRENT_SCENR.CONTENT_TRANSFER;
        Data_ModelNumber sceneID = new Data_ModelNumber(BackgroundCommunication.instance.m_thisClientName, DATA_NAME.DT_SCENE3, (int)m_currentScene);
        BackgroundCommunication.instance.SendToServer(sceneID.ToByteArray());
    }
    public void OnClickAugmentedVirtuality()
    {
        m_currentScene = CURRENT_SCENR.AV;
        Data_ModelNumber sceneID = new Data_ModelNumber(BackgroundCommunication.instance.m_thisClientName, DATA_NAME.DT_SCENE3, (int)m_currentScene);
        BackgroundCommunication.instance.SendToServer(sceneID.ToByteArray());
    }


    public void OnClickPhoneVR()
    {
        Data_ModelNumber sceneID = new Data_ModelNumber(BackgroundCommunication.instance.m_thisClientName, DATA_NAME.DT_SCENE3, (int)m_currentScene);
        BackgroundCommunication.instance.SendToServer(sceneID.ToByteArray());

        AppExit();

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
