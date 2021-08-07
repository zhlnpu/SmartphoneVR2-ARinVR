using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixEnvironment : MonoBehaviour
{

    public bool fix=false;

    public GameObject m_leftController;
    public GameObject m_tables;
    public GameObject m_environment;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(fix)
        {
            m_tables.transform.SetParent(m_environment.transform);    
            m_tables.isStatic = true;

         foreach (Transform tmep in m_tables.transform)
            {
                tmep.gameObject.isStatic = true;
            }
        }
        else
        {
            m_tables.transform.SetParent(m_leftController.transform);
        }
       



    }
}
