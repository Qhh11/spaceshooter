using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class heart : MonoBehaviour
{
  
    protected Camera cam;
    [SerializeField] protected float itemSpeed = 1f;
    [SerializeField] private Waypoints waypoints;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float distance = 0.1f;
    private Transform currentWaypoint;
    private bool hit = false;
   



    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        SetPosition();
        waypoints = GetComponent<Waypoints>();
    }

    // Update is called once per frame
    void Update()
    {
        var itemSpeedNormalized = itemSpeed * Time.deltaTime;
        transform.Translate(itemSpeedNormalized * Vector3.right, Space.Self);
        if (hit)
        {
            transform.position =
                Vector3.MoveTowards(transform.position, currentWaypoint.position, moveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, currentWaypoint.position) < distance)
            {
                currentWaypoint = waypoints.GetNextWaypoint(currentWaypoint);
            }
        }
    }


    public void SetPosition()
    {
        if (cam is null || cam.IsDestroyed()) return;

        transform.position = new Vector3(-14f, 9.53f, 8.13f);

    }

    private void SetHeart()
    {
        currentWaypoint = waypoints.getStart();
        Vector3 newPosition = currentWaypoint.position; 
        transform.position =
            Vector3.MoveTowards(transform.position, newPosition, 5f * Time.deltaTime);
        currentWaypoint = waypoints.GetNextWaypoint(currentWaypoint);
    }

    private void OnTriggerEnter(Collider other)
    {
        Asteroid enemy = other.GetComponent<Asteroid>();
        if (enemy != null && !enemy.fallingDownVal)
        {
           enemy.PlayExplosion();
           enemy.OnRespawnRandomize();
           SetHeart();
           hit = true;
           Debug.Log("Sweet Heart HIT.");
        }
    }
}
