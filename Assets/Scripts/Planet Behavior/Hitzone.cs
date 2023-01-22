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
        var enemy = other.GetComponent<Enemy>();
        
        if (enemy != null)
        {
            float damage = enemy.getEnemyScale() * (enemy.getEnemySpeed()/2);
            healthBar.DamageTaken(damage);
            return;
        }
        
        //Cosinus Asteroid
        var cosine = other.GetComponent<CosineAsteroid>();

        if (cosine != null)
        {
            float damage = cosine.getEnemyScale() * (cosine.getEnemySpeed()/2)+5f;
            healthBar.DamageTaken(damage);
            return;
        }
    }
}
