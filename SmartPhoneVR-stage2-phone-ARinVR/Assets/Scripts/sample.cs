using BasicConfigs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sample : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeRight;

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnClickBack();
        }
    }
    private void OnClickBack()
    {
        BackgroundCommunication.instance.m_currentScene = Scenes.MAIN_MENU;

        SceneManager.LoadScene("Scene_mainMenu");
        Destroy(gameObject);
    }
}
