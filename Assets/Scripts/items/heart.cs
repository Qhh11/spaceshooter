using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class heart : MonoBehaviour
{
    protected Camera cam;
    [SerializeField] protected float itemSpeed = 1f;
    
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        SetPosition();
    }

    // Update is called once per frame
    void Update()
    {
        var itemSpeedNormalized = itemSpeed * Time.deltaTime;
        transform.Translate(itemSpeedNormalized*Vector3.right,Space.Self);
    }
    
    protected void RandomizeData()
    {
      
    }
    
    public void SetPosition()
    {
        if (cam is null || cam.IsDestroyed()) return;
      
        transform.position = new Vector3(-14f, 9.53f,8.13f);
    
    }

}
