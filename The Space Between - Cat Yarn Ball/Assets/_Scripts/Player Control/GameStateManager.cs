using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : SingletonMonoBehaviour<GameStateManager>
{
    public GameObject EndCanvas;

    public GameState gameState;

    public void StartEndGame()
    {
        EndCanvas.SetActive(true);

        gameState = GameState.EndingGame;

        // Fade out music
    }
}

public enum GameState
{
    PauseScreen,
    Playing,

    EndingGame,
    GameEnded
}
