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
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float distance = 0.2f;
    private Transform currentWaypoint;
    private bool hit = false;
    private int count = 0;
    private bool animated = false;



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
        if (!hit)
        {
            transform.Translate(itemSpeedNormalized * Vector3.right, Space.Self);
        }
        else
        {
            hit = true;
            animated = true;
            if (count == 7)
            {
                Debug.Log("is over");
            }
            transform.position =
                    Vector3.MoveTowards(transform.position, currentWaypoint.position, moveSpeed * Time.deltaTime);
                Debug.Log("Distance is " + Vector3.Distance(transform.position, currentWaypoint.position));
                if (Vector3.Distance(transform.position, currentWaypoint.position) < distance)
                {
                    transform.position = currentWaypoint.position;
                    currentWaypoint = waypoints.GetNextWaypoint(currentWaypoint);
                    count++;
                }
        }
    }


    public void SetPosition()
    {
        if (cam is null || cam.IsDestroyed()) return;

        transform.position = new Vector3(-14f, 7.88f, 0f);

    }

    private void SetHeart()
    {
        currentWaypoint = waypoints.getStart();
        transform.position = currentWaypoint.position;
        currentWaypoint = waypoints.GetNextWaypoint(currentWaypoint);
    }

    private void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null && !enemy.fallingDownVal && !animated)
        {
           enemy.PlayExplosion();
           enemy.SetSpeedAndPosition();
           SetHeart();
           hit = true;
           Debug.Log("Sweet Heart HIT.");
        }
    }
}
