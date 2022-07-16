using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(GridPosition))]
public class SyncGridPosition : MonoBehaviour
{
    public enum Mode
    {
        Lerp,
    }

    public Mode mode;
    public float speed;
    public bool isDie;
    public List<GameObject> dieFaces;
    public Transform diePivot;
    public Transform dieParent;

    [SerializeField]
    private bool updateInEditMode;

    private GridPosition gridPosition;
    private float remainingTime;
    private Vector2Int prevGridPosition;
    private Vector3 fromDir;
    private Vector3 to;
    private Vector3 diePivotPosition;
    private int diePivotAxis;
    private float diePivotToAngle;

    public bool Done => remainingTime == 0f;

    void Awake()
    {
        gridPosition = GetComponent<GridPosition>();
    }

    void Start()
    {
        remainingTime = 0f;
        prevGridPosition = gridPosition.Position;
        this.transform.position = new Vector3(gridPosition.Position.x, 0, gridPosition.Position.y);
    }

    void Update()
    {
        if (gridPosition.Position != prevGridPosition)
        {
            if (!updateInEditMode && !Application.isPlaying)
            {
                prevGridPosition = gridPosition.Position;
                this.transform.position = new Vector3(gridPosition.Position.x, 0, gridPosition.Position.y);
                remainingTime = 0f;
            }

            if (Vector2Int.Distance(gridPosition.Position, prevGridPosition) == 1f)
            {
                var dv = gridPosition.Position - prevGridPosition;

                // position lerp
                prevGridPosition = gridPosition.Position;
                to = new Vector3(gridPosition.Position.x, 0, gridPosition.Position.y);
                fromDir = (this.transform.position - to).normalized;
                var remDist = Vector3.Distance(this.transform.position, to);
                remainingTime = remDist / speed;

                // die pivot
                if (isDie)
                {
                    diePivotAxis = dv.x == 0 ? 0 : 2;
                    diePivotToAngle = dv.x == -1 || dv.y == 1 ? 90f : -90f;
                    diePivotPosition = to - new Vector3(dv.x, 0, dv.y) * 0.5f + new Vector3(0, -0.5f, 0);

                    diePivot.transform.position = diePivotPosition;
                    diePivot.transform.eulerAngles = new Vector3(0, 0, 0);

                    foreach (var xf in dieFaces)
                    {
                        xf.transform.SetParent(diePivot, true);
                    }
                }
            }
            else
            {
                // teleport
                prevGridPosition = gridPosition.Position;
                remainingTime = 0f;
                to = new Vector3(gridPosition.Position.x, 0, gridPosition.Position.y);
                this.transform.position = to;
            }
        }

        if (remainingTime > 0f)
        {
            remainingTime -= Time.deltaTime;

            if (remainingTime < 0f)
            {
                remainingTime = 0f;
                this.transform.position = to;

                if (isDie)
                {
                    diePivot.transform.position = diePivotPosition;

                    var euler = diePivot.transform.localEulerAngles;
                    euler[diePivotAxis] = diePivotToAngle;
                    diePivot.transform.localEulerAngles = euler;

                    foreach (var xf in dieFaces)
                    {
                        xf.transform.SetParent(dieParent, true);
                    }
                }
            }
            else
            {
                this.transform.position = to + fromDir * remainingTime * speed;

                if (isDie)
                {
                    diePivot.transform.position = diePivotPosition;

                    var euler = diePivot.transform.localEulerAngles;
                    euler[diePivotAxis] = diePivotToAngle * (1f - remainingTime * speed);
                    diePivot.transform.localEulerAngles = euler;
                }
            }
        }
    }
}
