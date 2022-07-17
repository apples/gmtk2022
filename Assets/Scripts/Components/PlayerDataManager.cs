using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager Singleton { get; set; }

    public int CurrentLevelReached { get; set; }

    void Start()
    {
        if (Singleton != null)
        {
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(this.gameObject);
        Singleton = this;
    }
}
