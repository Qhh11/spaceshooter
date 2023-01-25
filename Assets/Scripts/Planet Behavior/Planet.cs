using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Planet : MonoBehaviour
{
    public float rotationSpeedY;
    
    //for game over -> right now not working
    public float transitionDuration;
    public Camera cam;
    public GameObject explosionPrefab;

    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Renderer>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, rotationSpeedY, 0));
    }

    //falls Game over: Destroy Planet !! NOT WORKING
    public IEnumerator DestroyItself()
    {
        Instantiate(explosionPrefab, new Vector3(0f, -10f, -3f), Quaternion.identity);
        this.GetComponent<Renderer>().enabled = false;
        
        StartCoroutine(cam.GetComponent<CamShaker>().ShakeFadeout(3, .3f));
        yield return new WaitForSeconds(transitionDuration);

        //muss dann den end game screen laden
        //SceneManager.LoadScene("LoseScene");
    }
}
