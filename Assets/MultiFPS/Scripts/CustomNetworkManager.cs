using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using kcp2k;
using Mirror.SimpleWeb;
using System.Text.RegularExpressions;
using MultiFPS.Gameplay;
using MultiFPS.Gameplay.Gamemodes;
using MultiFPS.Room;
using MultiFPS.Scripts.UI;
using MultiFPS.UI;
using Sirenix.OdinInspector;
using Random = UnityEngine.Random;
using Utils = Mirror.Utils;

namespace MultiFPS
{
    public class CustomNetworkManager : NetworkRoomManager
    {
        public static CustomNetworkManager Instance;

        //this event exist to send late players data about gamemode and equipment of other players.
        public delegate void NewPlayerJoinedTheGame(NetworkConnectionToClient conn);
        public NewPlayerJoinedTheGame OnNewPlayerConnected { get; set; }

        public delegate void PlayerDisconnected(NetworkConnectionToClient conn);
        public PlayerDisconnected OnPlayerDisconnected { get; set; }

        public CustomNetworkRoomPlayer LocalPlayer
        {
            get
            {
                foreach (CustomNetworkRoomPlayer player in roomSlots)
                {
                    if (player.isLocalPlayer)
                    {
                        return player;
                    }
                }

                return null;
            }
        }

        //Toggle Mirror's GUI for hosting game and connecting 
        bool _guiSet;

        public override void Awake()
        {
            base.Awake();

            autoStartServerBuild = false;

            if (Instance)
            {
                Debug.LogError("Fatal error, two instances of Custom Network Manager");
                Destroy(Instance.gameObject);
            }

            Instance = this;
        }

        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            DespawnOneBOTSlotIfFull();
            RecalculateRoomPlayerIndices();
            base.OnServerAddPlayer(conn);
            OnNewPlayerConnected?.Invoke(conn);
        }

        private void DespawnOneBOTSlotIfFull()
        {
            if (roomSlots.Count >= RoomSetup.Properties.P_MaxPlayers)
            {
                foreach (CustomNetworkRoomPlayer player in roomSlots)
                {
                    if (player.BOT)
                    {
                        NetworkServer.Destroy(player.gameObject);
                        break;
                    }
                }
            }
        }

        public override void OnRoomStartServer()
        {
            base.OnRoomStartServer();
            FillEmptySlotWithBOTs();
        }

        public void FillEmptySlotWithBOTs()
        {
            int neededBots = RoomSetup.Properties.P_MaxPlayers - roomSlots.Count;
            
            for (int i = 0; i < neededBots; i++)
            {
                var newRoomGameObject = Instantiate(roomPlayerPrefab.gameObject, Vector3.zero, Quaternion.identity);
                CustomNetworkRoomPlayer roomPlayer = newRoomGameObject.GetComponent<CustomNetworkRoomPlayer>();
                roomPlayer.SetAsBOT();
                
                // roomPlayer.UserName = $"BOT {Random.Range(1, 9999)}";
                //
                // roomPlayer.AgentID = ClientInterfaceManager.Instance.GetRandomAgent().SpawnID;
                //
                // roomPlayer.SetAsBOT();
                // roomPlayer.SetReadyToBegin(true);
                //
                // if(roomPlayer.isLocalPlayer)
                // {
                //     roomPlayer.gameObject.name = roomPlayer.UserName;
                // }
                
                NetworkServer.Spawn(roomPlayer.gameObject);
            }
        }

        public override void OnServerDisconnect(NetworkConnectionToClient conn)
        {
            OnPlayerDisconnected?.Invoke(conn);
            base.OnServerDisconnect(conn);
        }

        public override void Update()
        {
            base.Update();

            if (Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                _guiSet = !_guiSet;

                GetComponent<NetworkManagerHUD>().enabled = _guiSet;
                Cursor.visible = _guiSet;
                if (_guiSet)
                {
                    Cursor.lockState = CursorLockMode.Confined;
                }
                else
                {
                    Cursor.lockState = CursorLockMode.Locked;
                }
            }
        }


        public override void ServerChangeScene(string newSceneName)
        {
            UILoading.Instance?.Show(true);
            base.ServerChangeScene(newSceneName);
            StartCoroutine(IELoadScene(loadingSceneAsync, () => UILoading.Instance?.Show(false)));
        }
        
        private IEnumerator IELoadScene(AsyncOperation asyncLoad, Action callback = null)
        {
            while (!asyncLoad.isDone)
            {
                UILoading.Instance.UpdateProgress(asyncLoad.progress);
                yield return null;
            }
            
            callback?.Invoke();
        }
        
        
        
        public override bool OnRoomServerSceneLoadedForPlayer(NetworkConnectionToClient conn, GameObject roomPlayer, GameObject gamePlayer)
        {
            // var playerInstance = gamePlayer.GetComponent<PlayerInstance>();
            // var netRoomPlayer = roomPlayer.GetComponent<CustomNetworkRoomPlayer>();
            //
            // playerInstance.PlayerInfo.AgentID = netRoomPlayer.AgentID;
            // playerInstance.PlayerInfo.Username = netRoomPlayer.UserName;
            //
            //
            return true;
        }
        
        public override GameObject OnRoomServerCreateGamePlayer(NetworkConnectionToClient conn, GameObject roomPlayer)
        {
            
            // get start position from base class
            Transform startPos = GetStartPosition();
            var gamePlayer = startPos != null
                ? Instantiate(playerPrefab, startPos.position, startPos.rotation)
                : Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);

            PlayerInstance playerInstance = gamePlayer.GetComponent<PlayerInstance>();
            var netRoomPlayer = roomPlayer.GetComponent<CustomNetworkRoomPlayer>();
            
            playerInstance.PlayerInfo.AgentID = netRoomPlayer.AgentID;
            playerInstance.PlayerInfo.Username = netRoomPlayer.UserName;
            playerInstance.SetTeam(netRoomPlayer.Team);
            
            return playerInstance.gameObject;
        }
        
        public override GameObject OnRoomServerCreateRoomPlayer(NetworkConnectionToClient conn)
        {
            CustomNetworkRoomPlayer roomPlayer = Instantiate(roomPlayerPrefab, Vector3.zero, Quaternion.identity)
                .GetComponent<CustomNetworkRoomPlayer>();

            roomPlayer.RoomProperties = RoomSetup.Properties;
            
            GameplayScene = RoomSetup.Properties.P_Map;
            
            return roomPlayer.gameObject;
        }

        public void SwapPlayerIndex(int oldIndex, int newIndex)
        {
            try
            {
                var temp = roomSlots[oldIndex];
                roomSlots[oldIndex] = roomSlots[newIndex];
                roomSlots[newIndex] = temp;
            
                RecalculateRoomPlayerIndices();
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to swap player position: {e}");
                throw;
            }
        }
        
        [Server]
        public void RecalculateRoomPlayerTeam()
        {
            foreach (CustomNetworkRoomPlayer player in roomSlots)
            {
                player.Team = player.index < (RoomSetup.Properties.P_MaxPlayers / 2) ? 0 : 1;
            }
        }
        

        public void ConnectToTheGame()
        {
            StartClient();
        }

        public void SetAddressAndPort(string address, string port)
        {
            if (string.IsNullOrEmpty(address) || string.IsNullOrEmpty(port)) return;

            networkAddress = address;

            ushort uport = (ushort)System.Convert.ToInt32(port);

            KcpTransport kcpTransport = GetComponent<KcpTransport>();

            if (kcpTransport == transport)
            {
                kcpTransport.Port = uport;
                return;
            }

            SimpleWebTransport simpleWebTransport = GetComponent<SimpleWebTransport>();

            if (simpleWebTransport == transport) 
            {
                simpleWebTransport.port = uport;
                return;
            }
        }
    }

}