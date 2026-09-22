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
        private NetworkList<PlayerSessionData> sessionPlayers = new NetworkList<PlayerSessionData>();
        
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
        
        private void AddPlayer(ulong clientId)
        {
            sessionPlayers.Add(new PlayerSessionData 
            { 
                clientId = clientId, 
                displayName = $"Player {clientId}",
                isReady = false
            });
        }

        private void RemovePlayer(ulong clientId)
        {
            for (int i = 0; i < sessionPlayers.Count; i++)
            {
                if (sessionPlayers[i].clientId == clientId)
                {
                    sessionPlayers.RemoveAt(i);
                    break;
                }
            }
        }
    }
}