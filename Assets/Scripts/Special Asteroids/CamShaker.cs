using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CamShaker : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            StartCoroutine(ShakeFadeout(1,.4f));
        }
    }

    // Start is called before the first frame update
    public IEnumerator ShakeSteady(float duration, float magnitude)
    {
        Vector3 originalPos = transform.localPosition;

        float elapsed = 0.0f;

        while (elapsed <= duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x, y, originalPos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }
    }
    
    public IEnumerator ShakeFadeout(float duration, float magnitude) {
        Vector3 originalPos = transform.localPosition;
        
        float elapsed = 0.0f;

        float fadeout = elapsed / duration;

        while (elapsed <= duration)
        {
            float x = Random.Range(-1f+fadeout, 1f-fadeout) * magnitude + originalPos.x;
            float y = Random.Range(-1f+fadeout, 1f-fadeout) * magnitude + originalPos.y;

            transform.localPosition = new Vector3(x,y,originalPos.z);

            elapsed += Time.deltaTime;
            fadeout = elapsed / duration;

            yield return null;
        }

        transform.position = originalPos;

    }
}
