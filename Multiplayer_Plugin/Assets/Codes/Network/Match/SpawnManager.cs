using System;
using Unity.Netcode;
using UnityEngine;

namespace Codes.Network.Match
{
    /// <summary>
    /// A class for handling spawning players before and in game, it never destroys.
    /// </summary>
    [AddComponentMenu("JKGames/Multiplayer Plugin/SpawnManager")]
    [RequireComponent(typeof(NetworkObject))]
    
    public class SpawnManager : NetworkBehaviour
    {
        private NetworkManager networkManager;
        private NetworkObject networkObject;

        // -- Singleton --
        public static SpawnManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
        }
        //  --   --  --  --
        
        private void Start()
        {
            networkManager = NetworkManager.Singleton;
            networkObject = GetComponent<NetworkObject>();
            
            networkObject.DontDestroyWithOwner = true;
            if (networkManager == null)
            {
                networkManager = FindAnyObjectByType<NetworkManager>();
            }
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
        }

        /// <summary>
        /// Spawns a network object related to clientID with default of the host or server.
        /// </summary>
        /// <param name="objectPrefab">Prefab of the network object to spawn</param>
        /// <param name="position">Position of the spawned object</param>
        /// <param name="rotation">Rotation of the spawned object</param>
        /// <param name="clientID">Owner of the object</param>
        /// <param name="destroyWithScene">Should be destroyed on scene changes</param>
        /// <param name="isPlayerObject">Is a player object or not</param>
        /// <returns>If successfully instantiated returns the spawned object, else returns null</returns>
        public NetworkObject SpawnObject(NetworkObject objectPrefab, Vector3 position, Quaternion rotation,
            ulong clientID = 0, bool destroyWithScene = true, bool isPlayerObject = false)
        {
            if (IsServer)
            {
                return networkManager.SpawnManager.InstantiateAndSpawn(objectPrefab, clientID, destroyWithScene, isPlayerObject,
                    position:position, rotation:rotation);
            }
            
            return null;
        }
    }   
}
