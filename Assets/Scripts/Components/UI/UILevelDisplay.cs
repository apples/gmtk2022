using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UILevelDisplay : MonoBehaviour
{
    public string prefix = "Level";

    private TMP_Text text;

    private int lastLevel;

    void Start()
    {
        text = GetComponent<TMP_Text>();
        text.text = $"{prefix} {PlayerDataManager.Singleton.CurrentLevelReached}";
        lastLevel = PlayerDataManager.Singleton.CurrentLevelReached;
    }

    void Update()
    {
        if (lastLevel != PlayerDataManager.Singleton.CurrentLevelReached)
        {
            lastLevel = PlayerDataManager.Singleton.CurrentLevelReached;
            text.text = $"{prefix} {lastLevel}";
        }
    }
}
