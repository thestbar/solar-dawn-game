using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCircles : MonoBehaviour
{
    private float angle = 0f;

    [Range(0f,1f)]
    public float angleStep;

    private GameObject obj1;
    private GameObject obj2;
    private GameObject obj3;
    private LineRenderer lr1;
    private LineRenderer lr2;
    private LineRenderer lr3;
    public float innerRadius = 1.0f;
    public float middleRadius = 2.0f;
    public float outerRadius = 3.0f;

    [Range(0.0f,0.5f)]
    public float lineStartWidth;

    [Range(0.0f, 0.5f)]
    public float lineEndWidth;

    public Material lineRendererMaterial;
    public float maxDistanceFromEarth = 80.0f;
    public float targetCirclesMetallicChangeSpeed = 5f;

    void Start()
    {
        obj1 = GameObject.Find("InnerCircle");
        obj2 = GameObject.Find("MiddleCircle");
        obj3 = GameObject.Find("OuterCircle");
        lr1 = obj1.AddComponent<LineRenderer>();
        lr2 = obj2.AddComponent<LineRenderer>();
        lr3 = obj3.AddComponent<LineRenderer>();
    }

    void Update()
    {
        DrawCirclesOnTarget();
        lineRendererMaterial.SetFloat("_Metallic", Mathf.Sin(angle));
        angle += Time.deltaTime * angleStep * targetCirclesMetallicChangeSpeed;
        if (angle > Mathf.PI) angle = 0f;
    }

    void DrawCirclesOnTarget()
    {
        DrawPolygon(1000, innerRadius, gameObject.transform.position, lineStartWidth, lineEndWidth, lr1);
        DrawPolygon(1000, middleRadius, gameObject.transform.position, lineStartWidth, lineEndWidth, lr2);
        DrawPolygon(1000, outerRadius, gameObject.transform.position, lineStartWidth, lineEndWidth, lr3);
    }

    void DrawPolygon(int vertexNumber, float radius, Vector3 centrePos, float startWidth, float endWidth, LineRenderer lr)
    {
        lr.material = lineRendererMaterial;
        lr.generateLightingData = true;
        lr.startWidth = startWidth;
        lr.endWidth = endWidth;
        lr.loop = true;
        float angle = 2 * Mathf.PI / vertexNumber;
        lr.positionCount = vertexNumber;

        for (int i = 0; i < vertexNumber; i++)
        {
            Matrix4x4 rotationMatrix = new Matrix4x4(new Vector4(Mathf.Cos(angle * i), Mathf.Sin(angle * i), 0, 0),
                                                     new Vector4(-1 * Mathf.Sin(angle * i), Mathf.Cos(angle * i), 0, 0),
                                                     new Vector4(0, 0, 1, 0),
                                                     new Vector4(0, 0, 0, 1));
            Vector3 initialRelativePosition = new Vector3(0, radius, 0);
            lr.SetPosition(i, centrePos + rotationMatrix.MultiplyPoint(initialRelativePosition));
        }
    }

    public float GetMaxDistanceFromEarth()
    {
        return maxDistanceFromEarth;
    }
}
