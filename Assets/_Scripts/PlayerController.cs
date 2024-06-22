using System;
using Rog_Card;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Button moveButton; // UI Button to trigger movement
    public MapManager mapManager;

    public event EventHandler<PlayerMoveEventArgs> OnPlayerMovementEvent;

    void Start()
    {
        moveButton.onClick.AddListener(() => OnMoveButtonClick());
    }

    // Method to be called by the button
    public void OnMoveButtonClick()
    {
        Debug.Log("Run Event");
        mapManager.OnPlayerMove(1, 1);
    }
}
