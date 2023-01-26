
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
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float distance = 0.2f;
    [SerializeField] public AudioSource sound;
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
        if (!hit && !animated)
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
                GameManager gameManager = GameManager.Instance;
                StartCoroutine(GameManager.Instance.HealBothPlayers());
                StartCoroutine(Rotate(2f));
                hit = false;
                return;
            }
            transform.position =
                    Vector3.MoveTowards(transform.position, currentWaypoint.position, moveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, currentWaypoint.position) < distance)
                {
                    transform.position = currentWaypoint.position;
                    currentWaypoint = waypoints.GetNextWaypoint(currentWaypoint);
                    count++;
                }
        }
    }

    IEnumerator Rotate(float duration) {
        transform.position =
            Vector3.MoveTowards(transform.position, new Vector3(1, -1000, 0), moveSpeed * Time.deltaTime);
        transform.localScale += new Vector3(0.02f, 0.02f, 0.02f);
        Quaternion startRot = transform.rotation;
        float t = 0.0f;
        while ( t  < duration )
        {
            t += Time.deltaTime;
            transform.rotation = startRot * Quaternion.AngleAxis(t / duration * 1080f, Vector3.up); 
            yield return null;
        }
        transform.rotation = startRot;
        Destroy(gameObject);
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
        Asteroid enemy = other.GetComponent<Asteroid>();
        if (enemy != null && !enemy.fallingDown && !animated)
        {
            sound.Play();
           enemy.PlayExplosion();
           enemy.OnRespawnRandomize();
           SetHeart();
           hit = true;
           Debug.Log("Sweet Heart HIT.");
        }
    }
}