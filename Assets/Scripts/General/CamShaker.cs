using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CamShaker : MonoBehaviour
{

    private bool isShaking = false;
    public static CamShaker Instance = null;
    private void Update()
    {
        
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    public IEnumerator ShakeSteady(float duration, float magnitude)
    {
        if (isShaking) yield break;
        isShaking = true;
        Vector3 originalPos = transform.localPosition;

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude + originalPos.x;
            float y = Random.Range(-1f, 1f) * magnitude + originalPos.y;

            transform.localPosition = new Vector3(x, y, originalPos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }
        transform.localPosition = originalPos;
        isShaking = false;
    }
    
    public IEnumerator ShakeFadeout(float duration, float magnitude) {
        if (isShaking) yield break;
        isShaking = true;
        Vector3 originalPos = transform.localPosition;

        float elapsed = 0.0f;

        float fadeout = elapsed / duration;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f+fadeout, 1f-fadeout) * magnitude + originalPos.x;
            float y = Random.Range(-1f+fadeout, 1f-fadeout) * magnitude + originalPos.y;

            transform.localPosition = new Vector3(x,y,originalPos.z);

            elapsed += Time.deltaTime;
            fadeout = elapsed / duration;
            yield return null;
        }
        
        transform.localPosition = originalPos;
        isShaking = false;

    }
}
