using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UIMenuAgent : MonoBehaviour
{
    public string gameplaySceneName;

    private PlayerControls controls;

    void Awake()
    {
        controls = new PlayerControls();
        controls.PlayerDice.Accept.performed += this.Accept;
    }

    private void Accept(InputAction.CallbackContext obj)
    {
        SceneManager.LoadScene(gameplaySceneName);
    }

    void OnEnable()
    {
        controls.PlayerDice.Enable();
    }

    void OnDisable()
    {
        controls.PlayerDice.Disable();
    }
}
