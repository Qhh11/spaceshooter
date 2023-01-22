using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    public Transform getStart()
    {
        return transform.GetChild(0);
    }

    private void OnDrawGizmos()
    {
        foreach (Transform t in transform)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(t.position, 0.1f);
        }
        
        Gizmos.color = Color.red;
        for (int i = 0;  i< transform.childCount -1; i++)
        {
            Gizmos.DrawLine(transform.GetChild(i).position, transform.GetChild(i + 1).position);
        }
    }
    public Transform GetNextWaypoint(Transform curr)
    {
      
        if (curr.GetSiblingIndex() == transform.childCount -1)
        {
            return null;
        }
        return transform.GetChild(curr.GetSiblingIndex() + 1);
    }
}
