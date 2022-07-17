using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Singleton { get; set; }

    public GameObject playerPrefab;
    public GameObject tileGridPrefab;
    public VoidEvent onExitReached;

    public string gameOverSceneName;

    private int level = 0;

    private TileGrid currentTileGrid;
    private PlayerController currentPlayer;

    private bool isGameOver;
    private float gameOverTimer;

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

    void Update()
    {
        if (isGameOver)
        {
            gameOverTimer -= Time.deltaTime;
            if (gameOverTimer <= 0)
            {
                SceneManager.LoadScene(gameOverSceneName);
            }
        }
    }

    private void CreateNextLevel()
    {
        ++level;
        PlayerDataManager.Singleton.CurrentLevelReached = level;

        currentTileGrid = Instantiate(tileGridPrefab).GetComponent<TileGrid>();

        currentTileGrid.Generate();

        currentPlayer.GetComponent<GridPosition>().Position = currentTileGrid.PlayerSpawnLocation;
    }

    private void onExitReached_onTrigger()
    {
        DestroyCurrentLevel();
        CreateNextLevel();
    }

    private void DestroyCurrentLevel()
    {
        Destroy(currentTileGrid.gameObject);
        currentTileGrid = null;
    }

    public void StartGameOverTimer()
    {
        isGameOver = true;
        gameOverTimer = 2f;
    }
}
