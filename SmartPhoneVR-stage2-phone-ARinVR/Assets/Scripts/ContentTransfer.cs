using BasicConfigs;
using ServerCommunication;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ContentTransfer : MonoBehaviour
{
    GUIStyle mustyule = new GUIStyle();


    public GameObject[] m_images;
    GameObject m_currentImage;

    public GameObject m_VRSpace;



  public      bool imagetouched=false;
   // public Vector3 mousepos;

    Vector2 m_initialPos;
    int index = -1;

    private void Update()
    {
    //    mousepos = Input.mousePosition;

        // if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)

        //Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
        //   movableObj.transform.Translate(touchDeltaPosition.x * speed, 0f, touchDeltaPosition.y * speed);

        //    Input.mousePosition

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
                if (TouchImage(Input.mousePosition.x, Input.mousePosition.y, m_images[i]))
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
            if(DragedIntoVRSpace( m_currentImage))
            {
                Debug.Log("into VR");
                                Data_ModelNumber modelNumber = new Data_ModelNumber(BackgroundCommunication.instance.m_thisClientName, DATA_NAME.DT_MODEL_NUMBER, index);
                                BackgroundCommunication.instance.SendToServer(modelNumber.ToByteArray());
            }
            m_currentImage.GetComponent<RectTransform>().anchoredPosition = m_initialPos;
            imagetouched = false;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnClickBack();
        }
    }

    private bool DragedIntoVRSpace(GameObject currentImage)
    {
        return TouchImage(currentImage.GetComponent<RectTransform>().anchoredPosition, m_VRSpace);
    }

    Vector2 offset;
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
        //mustyule.fontSize = 20;

        //if (Input.touchCount > 0)
        //{
        //    GUI.Label(new Rect(10, 10, 300, 300), Input.GetTouch(0).position.x.ToString() + "  " + Input.GetTouch(0).position.x.ToString(), mustyule);

        //}

    }



    private void OnClickBack()
    {
        BackgroundCommunication.instance.m_currentScene = Scenes.MAIN_MENU;
        SceneManager.LoadScene("Scene_mainMenu");
        Destroy(gameObject);
    }
}
