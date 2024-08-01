using System;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

namespace FPS
{
    public class UIRoomFPS : MonoBehaviour
    {
        [SerializeField] private NetworkManager networkManager;

        [SerializeField] private Button btnHost;


        private void Start()
        {
            btnHost.onClick.AddListener(OnClickHost);
        }

        private void OnClickHost()
        {
            networkManager.StartHost();
        }
    }
}