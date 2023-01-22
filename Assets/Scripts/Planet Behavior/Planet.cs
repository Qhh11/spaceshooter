using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Planet : MonoBehaviour
{
    public float rotationSpeedY;
    
    //for game over -> right now not working
    public Image transitionScreen;
    public float transitionDuration;
    public Camera cam;
    //public GameObject explosionPrefab;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, rotationSpeedY, 0));
    }

    //falls Game over: Destroy Planet !! NOT WORKING
    public void DestroyItself()
    {
        //StartCoroutine(DestructionPlanet());
    }

    //!!! not finished/ not working
    private IEnumerator DestructionPlanet()
    {
        //Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        StartCoroutine(cam.GetComponent<CamShaker>().ShakeFadeout(2, .3f));
        yield return new WaitForSeconds(transitionDuration);
        
        SceneManager.LoadScene("LoseScene");
    }
}
