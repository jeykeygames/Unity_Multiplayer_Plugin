using System;
using Unity.Netcode;
using UnityEngine;

namespace Codes.Network.Player
{
    /// <summary>
    /// A class that contains all players that are joined the current session and is handled by server or host.
    /// </summary>
    [AddComponentMenu("JKGames/Multiplayer Plugin/PlayerSessionManager")]
    [RequireComponent(typeof(NetworkObject))]
    public class PlayerSessionManager : NetworkBehaviour
    {
        // Setting these values allow you to control the min and max players in lobby.
        public int maxPlayers = 10;
        public int minPlayers = 1;
        
        private NetworkList<PlayerSessionData> sessionPlayers = new NetworkList<PlayerSessionData>();

        /* // -- Singleton --
        public static PlayerSessionManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }
        //  --  --  --  -- */

        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                NetworkManager.Singleton.OnClientConnectedCallback += AddPlayer;
                NetworkManager.Singleton.OnClientDisconnectCallback += RemovePlayer;
            }
        }

        public override void OnNetworkDespawn()
        {
            if (IsServer)
            {
                NetworkManager.Singleton.OnClientConnectedCallback -= AddPlayer;
                NetworkManager.Singleton.OnClientDisconnectCallback -= RemovePlayer;
            }
        }
        
        /// <summary>
        /// Adds a player to the player's list that are currently in lobby.
        /// </summary>
        /// <param name="clientId">Player to add</param>
        private void AddPlayer(ulong clientId)
        {
            if (sessionPlayers.Count >= maxPlayers)
            {
                return;
            }
            sessionPlayers.Add(new PlayerSessionData 
            { 
                clientId = clientId, 
                displayName = $"Player {clientId}",
                isReady = false
            });
        }

        /// <summary>
        /// Removes a player from the player's list that are currently in lobby.
        /// </summary>
        /// <param name="clientId">Player to remove</param>
        private void RemovePlayer(ulong clientId)
        {
            for (int i = 0; i < sessionPlayers.Count; i++)
            {
                if (sessionPlayers[i].clientId == clientId)
                {
                    sessionPlayers.RemoveAt(i);
                    return;
                }
            }
        }

        /* /// <summary>
        /// Gives the number of players in session.
        /// </summary>
        /// <returns>Number of players currently in session or lobby</returns>
        [Rpc()]
        public int RequestNumberOfPlayersInSession()
        {
            return sessionPlayers.Count;
        } */
    }
}