using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitzone : MonoBehaviour
{
    public HealthBar healthBar;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Asteroid kollidiert mit dem Planeten
    private void OnTriggerEnter(Collider other)
    {
        if (healthBar.getCurrentHP() <= 0) return;

        //regulärer Asteroid
        var enemy = other.GetComponent<Asteroid>();
        
        if (enemy != null)
        {
            float damage = enemy.GetEnemyScale() * (enemy.GetEnemySpeed()/2)+5f;
            healthBar.DamageTaken(damage);
            return;
        }
        
        //Cosinus Asteroid
        var cosine = other.GetComponent<CosineAsteroid>();

        if (cosine != null)
        {
            float damage = cosine.GetEnemyScale() * (cosine.GetEnemySpeed()/2)+10f;
            healthBar.DamageTaken(damage);
            return;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 0, 0.1f);
        Gizmos.DrawCube(transform.position, new Vector3(38, 1, 13));
    }
}
