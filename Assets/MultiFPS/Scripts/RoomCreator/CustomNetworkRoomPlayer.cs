using System;
using Mirror;
using MultiFPS.Gameplay.Gamemodes;
using MultiFPS.UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MultiFPS.Room
{
    public class CustomNetworkRoomPlayer : NetworkRoomPlayer
    {
        [SyncVar]
        public RoomProperties RoomProperties;
        
        [SyncVar(hook = nameof(HookPlayerNameChanged))]
        public string UserName;

        [SyncVar(hook = nameof(HookPlayerAgentIDChanged))]
        public int AgentID;

        [SyncVar] 
        public int Team;

        public bool BOT = false;

        public override void OnStartClient()
        {
            if(isLocalPlayer)
            {
                AgentSelector.Instance.SetupRoom(this, RoomProperties);
            }
            
            if(!BOT)
            {
                SetPlayerName(UserSettings.UserNickname);
            }
            
            SetPlayerAgentID();
        }

        public override void OnClientEnterRoom()
        {
            if (isLocalPlayer)
            {
                UpdateDisplay();
            }

            if (BOT)
            {
                SetPlayerName($"BOT {Random.Range(1, 9999)}");
                SetPlayerAgentID(ClientInterfaceManager.Instance.GetRandomAgent().SpawnID);
                SetReadyToBegin(true);
            }
        }

        public override void OnClientExitRoom()
        {
            // Debug.Log($"Log: OnClientExitRoom {SceneManager.GetActiveScene().path}");
        }
        
        public void SetPlayerAgentID(int agentID = -1)
        {
            if (isServer)
            {
                AgentID = agentID;
            }
            else
            {
                CmdPlayerAgentIDChanged(agentID);
            }
        }
        
        public void SetPlayerName(string name = "")
        {
            if (isServer)
            {
                UserName = name;
            }
            else
            {
                CmdPlayerNameChanged(name);
            }
        }

        public void SetAsBOT()
        {
            BOT = true;
        }

        #region Sync hooks
        public void HookPlayerAgentIDChanged(int oldAgentID, int newAgentID)
        { 
            UpdateDisplay();
        }
        
        public void HookPlayerNameChanged(string oldName, string newName)
        {
            this.gameObject.name = UserName;
            UpdateDisplay();
        }

        public override void IndexChanged(int oldIndex, int newIndex)
        {
            base.IndexChanged(oldIndex, newIndex);
            CustomNetworkManager.Instance.RecalculateRoomPlayerTeam();
            UpdateDisplay();
        }

        #endregion

        
        #region Commands
        [Command]
        public void CmdPlayerNameChanged(string newName)
        {
            UserName = newName;
            UpdateDisplay();
        }

        [Command]
        public void CmdPlayerAgentIDChanged(int newAgentID)
        {
            AgentID = newAgentID;
            UpdateDisplay();
        }


        [Command]
        public void CmdIndexChanged(int oldIndex, int newIndex)
        {
            CustomNetworkManager.Instance.SwapPlayerIndex(oldIndex, newIndex);
        }

        #endregion
        #region private methods
        private void UpdateDisplay()
        {
            foreach (CustomNetworkRoomPlayer player in CustomNetworkManager.Instance.roomSlots)
            {
                if(player == null)
                    continue;
                
                try
                {
                    AgentSelector.Instance.playerSlots[player.index]
                        .SetPlayer(player);
                    
                    AgentSelector.Instance.playerSlots[player.index]
                        .SetPlayerName(player.UserName, player.isLocalPlayer);
                
                    AgentSelector.Instance.playerSlots[player.index]
                        .SetAgent(ClientInterfaceManager.Instance.GetAgent(player.AgentID),
                            player.readyToBegin);
                    
                    Debug.Log($"Successful update display");
                }
                catch (Exception e)
                {
                    Debug.LogWarning($"Failed to update display: {e}");
                }

            }
        }
        #endregion
    }
}