using System.Collections;
using System.Collections.Generic;
using UnityEngine;

  

public abstract class Singleton<T> where T : class
{    
    public static T m_instance;
    public static T instance
    {
        get
        {
            if(m_instance == null)
            {
                m_instance = System.Activator.CreateInstance(typeof(T),true) as T;
            }
            return m_instance;
        }

    } 
   
}

public abstract class SingletonMono<T>: MonoBehaviour where T: SingletonMono<T>
{    
    private static T m_instance;
    public static T instance
    {
        get
        {         
            return m_instance;
        }

    }

    private void Awake() {
       
        if (m_instance == null)
        {
            m_instance = (T) this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
     
}