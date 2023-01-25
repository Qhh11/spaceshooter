using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ExplosionEarth : MonoBehaviour {

    public GameObject[] AllEffect;
    int i;
    GameObject MakedObject;

    void Start()
    {
        i = 1;
        MakedObject = Instantiate(AllEffect[i - 1], AllEffect[i - 1].transform.position, Quaternion.identity) as GameObject;
    }

    void Update()
    {
        
    }

}