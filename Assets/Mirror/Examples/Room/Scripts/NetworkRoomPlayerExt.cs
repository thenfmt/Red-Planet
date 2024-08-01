using UnityEngine;

namespace Mirror.Examples.NetworkRoom
{
    [AddComponentMenu("")]
    public class NetworkRoomPlayerExt : NetworkRoomPlayer
    {
        public override void OnStartClient()
        {
            //Debug.Log($"OnStartClient {gameObject}");
        }

        public override void OnClientEnterRoom()
        {
            Debug.LogError($"OnClientEnterRoom {this.gameObject.name}_{isLocalPlayer}");
        }

        public override void OnClientExitRoom()
        {
            //Debug.Log($"OnClientExitRoom {SceneManager.GetActiveScene().path}");
        }

        public override void IndexChanged(int oldIndex, int newIndex)
        {
            //Debug.Log($"IndexChanged {newIndex}");
        }

        public override void ReadyStateChanged(bool oldReadyState, bool newReadyState)
        {
            //Debug.Log($"ReadyStateChanged {newReadyState}");
        }

        public override void OnGUI()
        {
            base.OnGUI();
        }

        public override void DrawPlayerReadyState()
        {
            
            GUILayout.BeginArea(new Rect(20f + (index * 100), 200f, 90f, 130f));

            GUILayout.Label($"Player [{index + 1}]");

            if (readyToBegin)
                GUILayout.Label("Ready");
            else
                GUILayout.Label("Not Ready");


            GUILayout.Label($"AgentID: {AgentID}");
            
            
            if (((isServer && index > 0) || isServerOnly) && GUILayout.Button("REMOVE"))
            {
                // This button only shows on the Host for all players other than the Host
                // Host and Players can't remove themselves (stop the client instead)
                // Host can kick a Player this way.
                GetComponent<NetworkIdentity>().connectionToClient.Disconnect();
            }

            GUILayout.EndArea();
        }

        public override void DrawPlayerReadyButton()
        {
            if (NetworkClient.active && isLocalPlayer)
            {
                GUILayout.BeginArea(new Rect(20f, 300f, 120f, 20f));

                // if (readyToBegin)
                // {
                //     if (GUILayout.Button("Cancel"))
                //         CmdChangeReadyState(false);
                // }
                // else
                // {
                //     if (GUILayout.Button("Ready"))
                //         CmdChangeReadyState(true);
                // }


                if (GUILayout.Button("Change Agent")) 
                    CmdChangeAgent(AgentID+1);

                GUILayout.EndArea();
            }
        }
        
        

        #region thenfmt
        public void AgentChanged(int oldID, int newID)
        {
            Debug.Log($"AgentChanged {newID}");
        }
        
        [SyncVar(hook = nameof(AgentChanged))]
        private int AgentID;
        [Command]
        public void CmdChangeAgent(int id)
        {
            AgentID = id;
            NetworkRoomManager room = NetworkManager.singleton as NetworkRoomManager;
            if (room != null)
            {
                
            }
        }
        #endregion
    }
}
