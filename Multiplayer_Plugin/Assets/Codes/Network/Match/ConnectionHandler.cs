using System;
using Unity.Netcode;
using UnityEngine;

namespace Codes.Network.Match
{
    /// <summary>
    /// A class for handling client connections. 
    /// </summary>
    [AddComponentMenu("JKGames/Multiplayer Plugin/ConnectionHandler")]
    [RequireComponent(typeof(NetworkObject))]
    public class ConnectionHandler : NetworkBehaviour
    {
        public bool canJoinDuringSession = false;

        private NetworkManager networkManager;
        private NetworkObject networkObject;
        
        private void Awake()
        {
            networkManager = NetworkManager.Singleton;
            networkObject = GetComponent<NetworkObject>();
            
            networkObject.DontDestroyWithOwner = true;
            if (networkManager == null)
            {
                networkManager = FindAnyObjectByType<NetworkManager>();
            }
        }

        // -- Connection --
        private void ConnectionApprovalCallback(NetworkManager.ConnectionApprovalRequest arg1, NetworkManager.ConnectionApprovalResponse arg2)
        {
            if (MatchmakingSystem.Instance.GetGameState() == GameState.InLobby && !canJoinDuringSession)
            {
                string message = "Connection disapproved : The session is ended or in progress!";
                arg2.Approved = false;
                arg2.CreatePlayerObject = false;
                arg2.Reason = message;
                Debug.LogWarning(message);
            }
            else
            {
                arg2.Approved = true;
                arg2.CreatePlayerObject = true;
            }
        }
        
        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                networkManager.ConnectionApprovalCallback += ConnectionApprovalCallback;
            }
        }

        public override void OnNetworkDespawn()
        {
            if (IsServer)
            {
                networkManager.ConnectionApprovalCallback -= ConnectionApprovalCallback;
            }
        }
    }
}