using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieAttackSwing : MonoBehaviour
{
    public float speed;
    public List<GameObject> dieFaces;
    public Transform diePivot;
    public Transform dieParent;

    private float remainingTime;
    private Vector3 diePivotPosition;
    private int diePivotAxis;
    private float diePivotToAngle;

    public bool Done => remainingTime == 0f;

    void Start()
    {
        remainingTime = 0f;
    }

    void Update()
    {
        if (remainingTime > 0f)
        {
            remainingTime -= Time.deltaTime * speed;

            if (remainingTime < 0f)
            {
                remainingTime = 0f;

                var euler = diePivot.transform.localEulerAngles;
                euler[diePivotAxis] = 0f;
                diePivot.transform.localEulerAngles = euler;

                foreach (var xf in dieFaces)
                {
                    xf.transform.SetParent(dieParent, true);
                }
            }
            else
            {
                var euler = diePivot.transform.localEulerAngles;
                euler[diePivotAxis] = Mathf.PingPong(remainingTime * 2f, 1f) * diePivotToAngle;
                diePivot.transform.localEulerAngles = euler;
            }
        }
    }

    public void Play(Vector2Int dv)
    {
        remainingTime = 1f;

        diePivotAxis = dv.x == 0 ? 0 : 2;
        diePivotToAngle = dv.x == -1 || dv.y == 1 ? 45f : -45f;
        diePivotPosition = this.transform.position + new Vector3(dv.x, -1f, dv.y) * 0.5f;

        diePivot.transform.position = diePivotPosition;
        diePivot.transform.eulerAngles = new Vector3(0, 0, 0);

        foreach (var xf in dieFaces)
        {
            xf.transform.SetParent(diePivot, true);
        }
    }
}
