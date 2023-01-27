using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitzone : MonoBehaviour
{
    public HealthBar healthBar;

    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = cam.ViewportToWorldPoint(new Vector3(0.5f, -0.1f, -cam.transform.position.z));
    }

    //Asteroid kollidiert mit dem Planeten
    private void OnTriggerEnter(Collider other)
    {
        if (healthBar.getCurrentHP() <= 0) return;
        
        //Cosinus Asteroid
        var cosine = other.GetComponent<CosineAsteroid>();

        if (cosine != null)
        {
            float damage = cosine.GetEnemyScale()*2 * (cosine.GetEnemySpeed()/2)+5;
            healthBar.DamageTaken(damage);
            cosine.OnRespawnRandomize();
            return;
        }

        //regulärer Asteroid
        var enemy = other.GetComponent<Asteroid>();
        
        if (enemy != null)
        {
            float damage = enemy.GetEnemyScale()*2 * (enemy.GetEnemySpeed()/2);
            healthBar.DamageTaken(damage);
            enemy.OnRespawnRandomize();
            return;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 0, 0.1f);
        Gizmos.DrawCube(transform.position, new Vector3(38, 1, 13));
    }
}
