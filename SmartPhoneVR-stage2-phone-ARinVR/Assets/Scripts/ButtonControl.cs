using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonControl : MonoBehaviour
{
    public float rotSpeed = 20;
    public float transSpeed = 20;

    public GameObject cube;

    public bool setRotation = true;
    public bool setTranslation = false;

    public float slider_value = 0;

    public GameObject cameraObj;

    public Vector3 camera_x;
    public Vector3 camera_y;
    public Vector3 camera_z;

    public Vector3 translateValue;

    public float rotX;
    public float rotY;

    public int pointNum = 0;


    bool fingerCountChanged = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    void Update()
    {
     //    pointNum = Input.touchCount;
        // if ( pointNum> 0)
        // {
        // Debug.Log(Input.touchCount);
        // }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //user pressed back key
            Application.Quit();
        }

        if (Input.GetKeyDown("space"))
        {
            print("space key was pressed");
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.Menu))
        {
            //user pressed menu key
            Application.Quit();
        }
        //  OnMouseDrag();
        Debug.Log(Input.touchCount);

    }

    void OnMouseDrag()
    {
        Debug.Log(Input.touchCount);



        if (pointNum == Input.touchCount)
        {
          //  fingerCountChanged = false;


        }
        else
        {
            pointNum = Input.touchCount;
            return;
        }

//            fingerCountChanged = false;


        //for 1 finger point
        if (pointNum == 1)
        {
            print(pointNum);
            if (setTranslation)
            {
                PhoneScreenTranslate_XY();
            }
            if (setRotation)
            {
                PhoneScreenRotate_XY();
            }
        }
        //for 2 finger point
        else if (pointNum == 2)
        {
            if (setTranslation)
            {
                PhoneScreenTranslate_Z();
            }
            if (setRotation)
            {
                PhoneScreenRotate_Z();
            }
        }
    }



    /// <summary>
    /// touch screen translation of object
    /// </summary>
    /// <returns></returns>
    void PhoneScreenTranslate_XY()
    {
        rotX = Input.GetAxis("Mouse X") * transSpeed;
        rotY = Input.GetAxis("Mouse Y") * transSpeed;

        var matrix = cameraObj.transform.localToWorldMatrix;

        camera_x = new Vector3(matrix[0, 0], matrix[1, 0], matrix[2, 0]);
        camera_y = new Vector3(matrix[0, 1], matrix[1, 1], matrix[2, 1]);
        camera_z = new Vector3(matrix[0, 2], matrix[1, 2], matrix[2, 2]);

        translateValue = camera_x * rotX + camera_y * rotY;
        cube.transform.Translate(new Vector3(translateValue.x, translateValue.y, translateValue.z), Space.World);
    }

    void PhoneScreenTranslate_Z()
    {
        rotX = Input.GetAxis("Mouse X") * transSpeed;
        rotY = Input.GetAxis("Mouse Y") * transSpeed;

        var matrix = cameraObj.transform.localToWorldMatrix;

        camera_x = new Vector3(matrix[0, 0], matrix[1, 0], matrix[2, 0]);
        camera_y = new Vector3(matrix[0, 1], matrix[1, 1], matrix[2, 1]);
        camera_z = new Vector3(matrix[0, 2], matrix[1, 2], matrix[2, 2]);

        translateValue =  camera_z * rotY;
        cube.transform.Translate(new Vector3(translateValue.x, translateValue.y, translateValue.z), Space.World);
    }



    /// <summary>
    /// touch screen rotation of object
    /// </summary>
    /// <returns></returns>
    void PhoneScreenRotate_XY()
    {
        rotX = Input.GetAxis("Mouse X") * rotSpeed;
        rotY = Input.GetAxis("Mouse Y") * rotSpeed;


        var matrix = cameraObj.transform.localToWorldMatrix;

        camera_x = new Vector3(matrix[0, 0], matrix[1, 0], matrix[2, 0]);
        camera_y = new Vector3(matrix[0, 1], matrix[1, 1], matrix[2, 1]);
        camera_z = new Vector3(matrix[0, 2], matrix[1, 2], matrix[2, 2]);

        cube.transform.Rotate(camera_y, -rotX, Space.World);
        cube.transform.Rotate(camera_x, rotY, Space.World);
    }


    void PhoneScreenRotate_Z()
    {
        rotX = Input.GetAxis("Mouse X") * rotSpeed;
        rotY = Input.GetAxis("Mouse Y") * rotSpeed;


        var matrix = cameraObj.transform.localToWorldMatrix;

        camera_x = new Vector3(matrix[0, 0], matrix[1, 0], matrix[2, 0]);
        camera_y = new Vector3(matrix[0, 1], matrix[1, 1], matrix[2, 1]);
        camera_z = new Vector3(matrix[0, 2], matrix[1, 2], matrix[2, 2]);

       // cube.transform.Rotate(camera_y, -rotX, Space.World);
        cube.transform.Rotate(camera_z, rotY, Space.World);
    }







    public GUIStyle myStyle = new GUIStyle();
     int buttonHeight = 100;
     float buttonWidth =Screen.width*0.45f;
    float buttonEdge = Screen.width * 0.025f;



    private void OnGUI()
    {

        GUI.Label(new Rect(100, 100, 200, 200), Input.touchCount.ToString(), myStyle);


        //myStyle.fontSize = 50;
        GUI.Label(new Rect(10, 10, 200, 200), pointNum.ToString(), myStyle);

        if (GUI.Button(new Rect(buttonEdge, Screen.height- buttonHeight-buttonEdge, buttonWidth, buttonHeight), "Translation",myStyle))
        {
            setTranslation = true;
            setRotation = false;
        }

        if (GUI.Button(new Rect(Screen.width/2+ buttonEdge, Screen.height - buttonHeight-buttonEdge, buttonWidth, buttonHeight), "Rotation", myStyle))

        {
            setTranslation = false;
            setRotation = true;
        }

        slider_value = GUI.HorizontalSlider(new Rect(25, 100, 150, 20), slider_value, -1, 1);
        if (Input.GetMouseButtonUp(0))
        {
            slider_value = 0;
        }
    }




}