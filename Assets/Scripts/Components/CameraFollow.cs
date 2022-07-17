using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform following;
    public float distance;
    public float smoothing = 10f;

    void Start()
    {
        this.transform.position = following.position - this.transform.forward * distance;
    }

    void Update()
    {
        var idealPosition = following.position - this.transform.forward * distance;

        this.transform.position = Damp(this.transform.position, idealPosition, smoothing, Time.deltaTime);
    }

    private Vector3 Damp(Vector3 a, Vector3 b, float lambda, float dt)
    {
        return Vector3.Lerp(a, b, 1 - Mathf.Exp(-lambda * dt));
    }
}
