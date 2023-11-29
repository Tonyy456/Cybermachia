using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.Events;

namespace Tony
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(PlayerInputManager))]
    public class PlayerConnector : MonoBehaviour
    {
        #region PRIVATE_CLASSES
        public class ConnectedPlayer
        {
            public ConnectedPlayer()
            { }
            // add important elements that need contained.
            public InputDevice targetDevice { get; set; } = null;
            public int playerIndex { get; set; } = -1;
        }
        private class ConnectedPlayerHolder
        {
            //Singleton Junk
            private static ConnectedPlayerHolder instance = null;
            private static readonly object padlock = new object();
            ConnectedPlayerHolder() { }
            public static ConnectedPlayerHolder Instance
            {
                get
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new ConnectedPlayerHolder();
                        }
                        return instance;
                    }
                }
            }

            //Instance junk
            public List<ConnectedPlayer> connectedPlayers { get; set; } = new List<ConnectedPlayer>();
            public void AddPlayer(ConnectedPlayer player)
            {
                if (!PlayerIsConnected(player))
                    connectedPlayers.Add(player);
            }
            public void RemovePlayer(PlayerInput input)
            {
                if (input.devices.Count == 0) return;
                var match = connectedPlayers.Find(x => x.targetDevice == input.devices[0]);
                Debug.Log("Attempted to remove player by index: " + input.playerIndex);
                if (match != null)
                {
                    Debug.Log("success!");
                    connectedPlayers.Remove(match);
                }
            }
            public void Reset()
            {
                connectedPlayers = new List<ConnectedPlayer>();
            }
            public bool PlayerIsConnected(ConnectedPlayer player)
            {
                return
                    (
                    connectedPlayers.Find(x => x.targetDevice.deviceId == player.targetDevice.deviceId || x.playerIndex == player.playerIndex)
                    ) != null;
            }

        }
        #endregion

        [SerializeField] private int minimumPlayerCount = 0;
        [SerializeField] private bool clearConnectedPlayers = false;
        [SerializeField] private Transform playerParent;
        [SerializeField] private bool InitPlayerScene = false; 
        public UnityEvent onGameReady;
        public UnityEvent onPlayerGained;
        public UnityEvent onPlayerRemoved;

        private PlayerInputManager manager;
        private bool readyToInvoke = false;

        public static int numPlayers
        {
            get
            {
                return ConnectedPlayerHolder.Instance.connectedPlayers.Count;
            }
        }
        private void Awake()
        {
            Init();
        }

        public void Init()
        {
            if (clearConnectedPlayers) ConnectedPlayerHolder.Instance.Reset();
            manager = this.GetComponent<PlayerInputManager>();
            manager.notificationBehavior = PlayerNotifications.InvokeCSharpEvents;
            manager.onPlayerJoined += onPlayerJoined;
            manager.joinBehavior = PlayerJoinBehavior.JoinPlayersManually;
            manager.onPlayerLeft += (PlayerInput p) => {
                onPlayerRemoved?.Invoke();
            };

            // join all players that were connected in previous scenes.
            foreach (var player in ConnectedPlayerHolder.Instance.connectedPlayers)
            {
                manager.JoinPlayer(player.playerIndex, pairWithDevice: player.targetDevice);
            }

            // allow for individual scene testing.
            if (ConnectedPlayerHolder.Instance.connectedPlayers.Count < minimumPlayerCount)
                manager.joinBehavior = PlayerJoinBehavior.JoinPlayersWhenJoinActionIsTriggered;
            else
            {
                readyToInvoke = true;
            }
        }

        public void RemovePlayer(PlayerInput input)
        {
            ConnectedPlayerHolder.Instance.RemovePlayer(input);
        }

        public void Reset()
        {
            ConnectedPlayerHolder.Instance.Reset();
            foreach(var input in GameObject.FindObjectsOfType<PlayerInput>(true))
            {
                GameObject.Destroy(input.gameObject);
            }
        }

        private void Start()
        {
            if(readyToInvoke)
                onGameReady?.Invoke();
        }

        private void Refresh()
        {
            ConnectedPlayerHolder.Instance.Reset();
            foreach (var input in GameObject.FindObjectsOfType<PlayerInput>())
            {
                AddPlayerToSingleton(input);
            }
        }

        private void onPlayerJoined(PlayerInput input)
        {
            if (input == null) return; //just in case.
            AddPlayerToSingleton(input);
            onPlayerGained?.Invoke();
            if(manager.joinBehavior == PlayerJoinBehavior.JoinPlayersWhenJoinActionIsTriggered &&
                ConnectedPlayerHolder.Instance.connectedPlayers.Count >= minimumPlayerCount)
            {
                onGameReady?.Invoke();
            }
            if (playerParent != null) input.gameObject.transform.SetParent(playerParent);
            IPlayerConnectedHandler[] devices = Object.FindObjectsOfType<IPlayerConnectedHandler>();
            foreach (var i in devices) i.ConnectPlayer(input);
        }

        private void AddPlayerToSingleton(PlayerInput input)
        {
            ConnectedPlayer player = new ConnectedPlayer();
            player.playerIndex = input.playerIndex;
            player.targetDevice = input.devices.Count > 0 ? input.devices[0] : null;
            ConnectedPlayerHolder.Instance.AddPlayer(player);
        }
    }
}

