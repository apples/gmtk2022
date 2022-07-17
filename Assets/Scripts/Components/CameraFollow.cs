using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float distance;
    public float smoothing = 10f;
    public float teleportDistance = 10f;

    public GameObjectReference targetReference;

    void Start()
    {
        if (targetReference != null && targetReference.Current != null)
        {
            this.transform.position = targetReference.Current.transform.position - this.transform.forward * distance;
        }
    }

    void Update()
    {
        if (targetReference != null && targetReference.Current != null)
        {
            var idealPosition = targetReference.Current.transform.position - this.transform.forward * distance;

            if (Vector3.Distance(idealPosition, this.transform.position) > teleportDistance)
            {
                this.transform.position = idealPosition;
            }
            else
            {
                this.transform.position = Damp(this.transform.position, idealPosition, smoothing, Time.deltaTime);
            }
        }
    }

    private Vector3 Damp(Vector3 a, Vector3 b, float lambda, float dt)
    {
        return Vector3.Lerp(a, b, 1 - Mathf.Exp(-lambda * dt));
    }
}
