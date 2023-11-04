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
                if(!PlayerIsConnected(player))
                    connectedPlayers.Add(player);
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
        public UnityEvent onGameReady;
        public UnityEvent onPlayerGained;

        private PlayerInputManager manager;
        private bool readyToInvoke = false;
        private void Awake()
        {
            if (clearConnectedPlayers) ConnectedPlayerHolder.Instance.Reset();

            manager = this.GetComponent<PlayerInputManager>();
            manager.notificationBehavior = PlayerNotifications.InvokeCSharpEvents;
            manager.onPlayerJoined += onPlayerJoined;
            manager.joinBehavior = PlayerJoinBehavior.JoinPlayersManually;

            // join all players that were connected in previous scenes.
            foreach (var player in ConnectedPlayerHolder.Instance.connectedPlayers)
            {
                manager.JoinPlayer(player.playerIndex, pairWithDevice: player.targetDevice);
            }

            // allow for individual scene testing.
            
            if (ConnectedPlayerHolder.Instance.connectedPlayers.Count < minimumPlayerCount)
                manager.joinBehavior = PlayerJoinBehavior.JoinPlayersWhenButtonIsPressed;
            else {
                readyToInvoke = true;
            }
        }

        private void Start()
        {
            if(readyToInvoke)
                onGameReady?.Invoke();
        }

        private void onPlayerJoined(PlayerInput input)
        {
            if (input == null) return; //just in case.
            AddPlayerToSingleton(input);
            onPlayerGained?.Invoke();
            if(manager.joinBehavior == PlayerJoinBehavior.JoinPlayersWhenButtonIsPressed &&
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

