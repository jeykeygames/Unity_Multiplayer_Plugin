using System;
using System.Collections.Generic;
using Codes.Network.Player;
using TMPro;
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
        [SerializeField]
        private GameObject menuUIPrefab;
        [SerializeField]
        private GameObject playerInfoUIPrefab;
        [SerializeField]
        private TextMeshProUGUI playerNumberText;
        [SerializeField]
        private GameObject loadingUIPrefab;
        [SerializeField]
        private GameObject lobbyUIPrefab;
        [SerializeField]
        private GameObject multiplayerUIPrefab;
        private List<GameObject> playerInfos = new List<GameObject>();
            
        private NetworkManager networkManager;
        private PlayerSessionManager playerSessionManager;

        private void Start()
        {
            networkManager = NetworkManager.Singleton;
            playerSessionManager = FindAnyObjectByType<PlayerSessionManager>();

            if (playerSessionManager == null)
            {
                Debug.LogWarning("No PlayerSessionManager found!");
            }
            
            networkManager.OnClientConnectedCallback += HandleClientConnected;
            networkManager.OnClientDisconnectCallback += HandleClientDisconnect;
            networkManager.OnConnectionEvent += OnConnectionEvent;
        }

        public void StartAsHost()
        {
            networkManager.StartHost();
            if (networkManager.IsHost)
            {
                Debug.Log("Started as host.");
            }
            EnterLobbyUI(true);
        }

        public void StartAsServer()
        {
            networkManager.StartServer();
            if (networkManager.IsServer)
            {
                Debug.Log("Started as server.");
            }
            EnterLobbyUI(true);
        }

        public void StartAsClient()
        {
            networkManager.StartClient();
            if (networkManager.IsClient)
            {
                Debug.Log("Started as client.");
            }
            EnterLobbyUI(false);
        }

        public void StopNetworkManager()
        {
            networkManager.Shutdown();
        }

        private void EnterLobbyUI(bool isHost)
        {
            SetUIVisibility(multiplayerUIPrefab, false);
            SetUIVisibility(lobbyUIPrefab, true);
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
                OnClientConnectionFailed();
            }
        }
        
        private void OnConnectionEvent(NetworkManager arg1, ConnectionEventData arg2)
        {
            switch (arg2.EventType)
            {
                case ConnectionEvent.PeerConnected:
                    ConnectedToPeer();
                    Debug.Log("Peer Connected.");
                    break;
                case ConnectionEvent.PeerDisconnected:
                    Debug.LogWarning(arg1.DisconnectReason);
                    break;
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

        // -- UI Functions --
        
        private void SetUIVisibility(GameObject ui, bool visible)
        {
            if (ui)
            {
                ui.SetActive(visible);
            }
        }
        
        private void ConnectedToPeer()
        {
            SetUIVisibility(loadingUIPrefab, false);
            SetUIVisibility(lobbyUIPrefab, true);
        }
        
        private void UpdatePlayerCount(string playerName = "Player")
        {
            int count = networkManager.ConnectedClients.Count;
            if (playerNumberText)
            {
                playerNumberText.text = $"Number of players in lobby : {count}";
            }
        }
        
        private void OnClientConnectionFailed()
        {
            SetUIVisibility(loadingUIPrefab, false);
            SetUIVisibility(menuUIPrefab, true);
            Debug.LogWarning("Failed to connect to the server!");
        }
    }
}