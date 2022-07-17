using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Singleton { get; set; }

    public GameObject playerPrefab;
    public GameObject tileGridPrefab;
    public VoidEvent onExitReached;

    private int level = 0;

    private TileGrid currentTileGrid;
    private PlayerController currentPlayer;

    void OnEnable()
    {
        onExitReached.onTrigger += this.onExitReached_onTrigger;
        Singleton = this;
    }

    void OnDisable()
    {
        onExitReached.onTrigger -= this.onExitReached_onTrigger;
        Singleton = null;
    }

    void Start()
    {
        currentPlayer = Instantiate(playerPrefab).GetComponent<PlayerController>();

        CreateNextLevel();
    }

    private void CreateNextLevel()
    {
        currentTileGrid = Instantiate(tileGridPrefab).GetComponent<TileGrid>();

        currentTileGrid.Generate();

        currentPlayer.GetComponent<GridPosition>().Position = currentTileGrid.PlayerSpawnLocation;
    }

    private void onExitReached_onTrigger()
    {
        DestroyCurrentLevel();
        ++level;
        CreateNextLevel();
    }

    private void DestroyCurrentLevel()
    {
        Destroy(currentTileGrid.gameObject);
        currentTileGrid = null;
    }

    public void StartGameOverTimer()
    {
        throw new NotImplementedException();
    }
}
