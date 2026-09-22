using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

namespace Codes.Network.UI
{
    /// <summary>
    /// This class handles lobby ui and the logic behind network manager for hosting, joining or starting as server. 
    /// </summary>
    [AddComponentMenu("JKGames/Multiplayer Plugin/LobbyManager")]
    public class LobbyManager : MonoBehaviour
    {
        [Header("Lobby UI Elements")]

        private NetworkManager networkManager;

        private void Start()
        {
            networkManager = NetworkManager.Singleton;
            
            networkManager.OnClientConnectedCallback += HandleClientConnected;
            networkManager.OnClientDisconnectCallback += HandleClientDisconnect;
        }

        public void StartAsHost()
        {
            networkManager.StartHost();
            EnterLobbyUI(isHost: true);
        }

        public void StartAsServer()
        {
            networkManager.StartServer();
            EnterLobbyUI(isHost: true);
        }

        public void StartAsClient()
        {
            networkManager.StartClient();
            EnterLobbyUI(isHost: false);
        }

        private void EnterLobbyUI(bool isHost)
        {
            
            UpdatePlayerCount();
        }

        private void HandleClientConnected(ulong clientId)
        {
            if (networkManager.IsServer)
            {
                UpdatePlayerCount();
            }
        }

        private void HandleClientDisconnect(ulong clientId)
        {
            if (networkManager.IsServer)
            {
                UpdatePlayerCount();
            }
            else if (clientId == networkManager.LocalClientId)
            {
                networkManager.Shutdown();
            }
        }

        private void UpdatePlayerCount()
        {
            if (networkManager.IsServer)
            {
                int count = networkManager.ConnectedClients.Count;
                //
            }
        }

        public void OnHostStartGameClicked()
        {
            if (networkManager.IsServer)
            {
                networkManager.SceneManager.LoadScene("SCENE_NAME", UnityEngine.SceneManagement.LoadSceneMode.Single);
            }
        }

        private void OnDestroy()
        {
            if (networkManager != null)
            {
                networkManager.OnClientConnectedCallback -= HandleClientConnected;
                networkManager.OnClientDisconnectCallback -= HandleClientDisconnect;
            }
        }
    }
}