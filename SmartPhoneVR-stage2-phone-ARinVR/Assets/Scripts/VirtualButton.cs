using BasicConfigs;
using ServerCommunication;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VirtualButton : MonoBehaviour
{
    GUIStyle mustyule = new GUIStyle();

    public GameObject m_smallCircle;

    public GameObject m_largeCircle;

    public GameObject m_gripButton;

    Vector2 offset;

    public bool imagetouched = false;
    public Vector3 mousepos;

    Vector2 m_initialPos;

    Vector2 uiPos;

    // 0~1, explains the eccentricity
    float joystickMag = 0;

    // Start is called before the first frame update
    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;

        m_initialPos = m_smallCircle.GetComponent<RectTransform>().anchoredPosition;
    }


    private void Update()
    {
        if (imagetouched)
        {
            uiPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y) + offset;

            if (TouchImage(uiPos, m_largeCircle))
            {
                m_smallCircle.GetComponent<RectTransform>().anchoredPosition = uiPos;
                joystickMag = Vector2.Distance(uiPos, m_initialPos) / m_largeCircle.GetComponent<RectTransform>().sizeDelta.x * 2;

                if (joystickMag > 1)
                    joystickMag = 1;
            }
            else
            {
                m_smallCircle.GetComponent<RectTransform>().anchoredPosition = m_initialPos + (uiPos - m_initialPos).normalized * (m_largeCircle.GetComponent<RectTransform>().sizeDelta.x / 2 - 25);
                joystickMag = 1;
            }
        }


        if (Input.GetMouseButtonDown(0))
        {

            if (TouchImage(Input.mousePosition.x, Input.mousePosition.y, m_smallCircle))
            {
                imagetouched = true;
                offset = new Vector2(m_initialPos.x - Input.mousePosition.x, m_initialPos.y - Input.mousePosition.y);

                //  Debug.Log("index= " + index.ToString());                
            }

        }

        //release
        if (imagetouched && Input.GetMouseButtonUp(0))
        {
            m_smallCircle.GetComponent<RectTransform>().anchoredPosition = m_initialPos;
            imagetouched = false;
            uiPos = m_initialPos;
        }



        //  if(imagetouched || m_gripButton.GetComponent<HoldButtonDetection>().isPointDown)

        Data_VirtualButton data = new Data_VirtualButton(BackgroundCommunication.instance.m_thisClientName, DATA_NAME.DT_VITUAL_BUTTON);

        data.joystickEnabled = imagetouched;
        data.joystickDirection[0] = (uiPos - m_initialPos).normalized.x* joystickMag;
        data.joystickDirection[1] = (uiPos - m_initialPos).normalized.y* joystickMag;
        data.joystickMagnitude[0] = joystickMag;
        data.gripEnabled = m_gripButton.GetComponent<HoldButtonDetection>().isPointDown;

        BackgroundCommunication.instance.SendToServer(data.ToByteArray());




        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnClickBack();
        }
    }


    private bool TouchImage(Vector2 vec, GameObject image1)
    {
        return TouchImage(vec.x, vec.y, image1);
    }


    private bool TouchImage(float x, float y, GameObject image1)
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
        BackgroundCommunication.instance.m_currentScene = Scenes.MAIN_MENU;
        SceneManager.LoadScene("Scene_mainMenu");
        Destroy(gameObject);
    }
}
