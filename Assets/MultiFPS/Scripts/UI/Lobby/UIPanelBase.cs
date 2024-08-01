using System;
using System.Net.NetworkInformation;
using UnityEngine;

namespace MultiFPS.UI
{
    public class UIPanelBase : MonoBehaviour
    {
        [SerializeField] protected LobbyPanelManagager lobbyPanelManager;
        
        [SerializeField] private bool _isShow = false;

        private void OnEnable()
        {
            _isShow = true;
        }

        private void OnDisable()
        {
            _isShow = false;
        }

        public virtual void Show (bool isShow)
        {
            if (_isShow)
            {
                if (isShow)
                {
                    // Show --request_show--> Do nothing
                }
                else
                {
                    // Show --request_hide--> 
                    gameObject.SetActive(false);
                }
            }
            else
            {
                if (isShow)
                {
                    // Hide --request_show-->
                    gameObject.SetActive(true);
                }
                else
                {
                    // Hide --request_hide-->  Do nothing
                }
            }
        }
    }
}