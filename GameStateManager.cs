using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameStateManager : MonoBehaviour
{
    public enum GameState
    {
        Loading,
        InPlay,
        Paused,
        Victory,
        Defeat,
    }
    public MoveState currentGameState { get; private set; }
    public static Action<GameState> OnGameStateChanged;

    public void SetNewGameState(GameState newGameState)
    {
        if (newGameState == currentGameState) return;

        OnGameStateChanged?.Invoke(newGameState);
        currentGameState = newGameState;

        // Optionally I would add a switch statement here if any adjustments need to be made based on the new state from this script
    }
}
