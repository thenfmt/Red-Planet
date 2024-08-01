using System;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.UI;

namespace MultiFPS.UI
{
    public class UIPanelLobby : UIPanelBase
    {
        [SerializeField] private Button btnCreateRoom;
        [SerializeField] private Button btnConnect;
        // [SerializeField] private Button btnSetting;
        [SerializeField] private Button btnQuit;


        private void Start()
        {
            btnCreateRoom.onClick.AddListener(OnClickCreateRoom);
            btnConnect.onClick.AddListener(OnClickConnect);
            // btnSetting.onClick.AddListener(OnClickSetting);
            btnQuit.onClick.AddListener(OnClickQuit);
        }


        private void OnClickCreateRoom ()
        {
            lobbyPanelManager.UIPanelCreateRoom.Show(true);
            Show(false);
        }

        private void OnClickConnect()
        {
            
            lobbyPanelManager.UIPanelConnect.Show(true);
            Show(false);
        }

        private void OnClickSetting()
        {
            
        }

        private void OnClickQuit()
        {
            Application.Quit();
        }
    }
}