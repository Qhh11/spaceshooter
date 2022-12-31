using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

[RequireComponent(typeof(LineRenderer))]
public class RopeBehaviour : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Transform point0, point1;
    public GameObject spring;
    public float maxLength = 10f;
    
    [Range(1, 100)]
    public int numPoints = 50;
    private Vector3[] positions;
    private SpringJoint _spring;
    private bool calculateAnchor = true;
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        positions = new Vector3[numPoints];
        lineRenderer.positionCount = numPoints;
        _spring = spring.GetComponent<SpringJoint>();
    }

    private void Update()
    {
        DrawCurve();
        MakeSpringAnchorPoint(point0.position, point1.position);
        if (Input.GetKeyDown(KeyCode.O))
        {
            Debug.Log("Hey");
            StartCoroutine(Bounce());
        }
    }

    private void DrawCurve()
    {
        for (int i = 1; i <= numPoints; i++)
        {
            float t = i / (float) numPoints;
            positions[i - 1] = CalculateQuadraticBezierPoint(t, point0.position, spring.transform.position, point1.position);
        }
        lineRenderer.SetPositions(positions);
    }

    private Vector3 CalculateQuadraticBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        // B(t) = (1-t)2P0 + 2(1-t)tP1 + t2P2 , 0 < t < 1
        
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        
        Vector3 point = uu * p0;
        point += 2 * u * t * p1;
        point += tt * p2;
        
        return point;
    }

    private void MakeSpringAnchorPoint(Vector3 p0, Vector3 p1)
    {
        if (!calculateAnchor) return;
        Vector3 pos = (p0 + p1) / 2;
        spring.transform.position = new Vector3(pos.x,spring.transform.position.y,spring.transform.position.z);
        pos.y -= maxLength - Vector3.Distance(p0, p1);
        _spring.connectedAnchor = pos;
    }

    IEnumerator Bounce()
    {
        calculateAnchor = false;
        _spring.connectedAnchor += new Vector3(0, 10, 0);
        yield return new WaitForSeconds(.1f);
        _spring.connectedAnchor -= new Vector3(0, 10, 0);
        calculateAnchor = true;
    }
}
