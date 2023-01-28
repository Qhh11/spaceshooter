using System.Collections;
using System.Collections.Generic;
using UnityEngine;



using UnityEngine.SceneManagement;

public class notReload : MonoBehaviour
{ 
    private static notReload instance;

    void Awake()
    {
        if (instance != null )

        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            }
    }


}
