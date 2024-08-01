using System;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.UI;

namespace MultiFPS.UI
{
    public class UIPanelConnect : UIPanelBase
    {
        [SerializeField] private Button btnReturn;


        private void Start()
        {
            btnReturn.onClick.AddListener(OnClickReturn);
        }

        private void OnClickReturn ()
        {
            lobbyPanelManager.UIPanelLobby.Show(true);
            Show(false);
        }
    }
}