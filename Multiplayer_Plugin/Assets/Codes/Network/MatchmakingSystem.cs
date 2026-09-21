using System;
using Unity.Netcode;
using UnityEngine;

/// <summary>
/// Matchmaking system handles games state changes during a multiplayer session to be used by UI elements by clients later in game.
/// </summary>
[AddComponentMenu("JKGames/Multiplayer Plugin/MatchmakingSystem")]
[RequireComponent(typeof(NetworkObject))]
public class MatchmakingSystem : NetworkBehaviour
{
    /// <summary>
    /// An enum for tracking in game state 
    /// </summary>
    public enum GameState
    {
        None,
        InLobby,
        InGame,
        GameOver
    }
    
    public static MatchmakingSystem Instance { private set; get; }
    
    private NetworkObject networkObject;
    private GameState gameState;

    private void Awake()
    {
        // Singleton >>>
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            Instance = this;
        }
        // <<< Singleton
        
        networkObject = GetComponent<NetworkObject>();

        if (networkObject)
        {
            networkObject.DestroyWithScene = false;
        }
    }

    private void Start()
    {
        gameState = GameState.None;
    }

    public GameState GetGameState()
    {
        return gameState;
    }
    
}
