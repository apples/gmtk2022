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

    [SerializeField]
    private bool updateInEditMode;

    private GridPosition gridPosition;
    private float remainingTime;
    private Vector2Int prevGridPosition;
    private Vector3 fromDir;
    private Vector3 to;

    public bool Done => remainingTime == 0f;

    void Awake()
    {
        gridPosition = GetComponent<GridPosition>();
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
                prevGridPosition = gridPosition.Position;
                to = new Vector3(gridPosition.Position.x, 0, gridPosition.Position.y);
                fromDir = (this.transform.position - to).normalized;
                var remDist = Vector3.Distance(this.transform.position, to);
                remainingTime = remDist / speed;
            }
            else
            {
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
            }
            else
            {
                this.transform.position = to + fromDir * remainingTime * speed;
            }
        }
    }
}
