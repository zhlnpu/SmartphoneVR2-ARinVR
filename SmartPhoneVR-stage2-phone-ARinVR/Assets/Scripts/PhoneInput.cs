using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ServerCommunication;

public class PhoneInput : MonoBehaviour
{

    public GameObject textInputField;

    Data_CharByte phoneInput = new Data_CharByte(CLIENT_NAME.CN_ANDROID_1, DATA_NAME.DT_PHONE_INPUT);



    // Start is called before the first frame update
    void Start()
    {

        textInputField. transform.GetComponent<InputField>().onValueChanged.AddListener(Changed_Value);
    }

    private void Changed_Value(string arg0)
    {


        //  phoneInput.value = arg0.ToCharArray();
        phoneInput.SetValue(arg0);

        BackgroundCommunication.instance.SendToServer(phoneInput.ToByteArray());


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
