using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPosition : MonoBehaviour
{
    public TileGridReference tileGridReference;

    [SerializeField]
    private Vector2Int position;

    public Vector2Int Position {
        get => position;
        set
        {
            if (tileGridReference.Current != null)
            {
                tileGridReference.Current.MoveOccupant(this, position, value);
            }
            position = value;
        }
    }

    void Start()
    {
        if (tileGridReference.Current != null)
        {
            tileGridReference.Current.NewOccupant(this);
        }
    }

    void OnDestroy()
    {
        if (tileGridReference.Current != null)
        {
            tileGridReference.Current.RemoveOccupant(this);
        }
    }
}
