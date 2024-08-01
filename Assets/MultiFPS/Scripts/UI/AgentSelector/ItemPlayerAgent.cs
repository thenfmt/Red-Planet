using System;
using System.Collections;
using MultiFPS.Room;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MultiFPS.UI
{
    public class ItemPlayerAgent : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] private Image iconAgentSelected;
        [SerializeField] private TextMeshProUGUI txtPlayerName;
        [SerializeField] private TextMeshProUGUI txtAgentName;
        [SerializeField] private Button btnSwapPosition;

        [Space, Header("Config")] 
        [SerializeField] private Color localPlayerNameColor;
        [SerializeField] private Color otherPlayerNameColor;

        private AgentContainer _agentSeleted = null;
        private CustomNetworkRoomPlayer _player;


        private void Start()
        {
            btnSwapPosition.onClick.AddListener(OnClickSwapPosition);
        }
        
        public void SetPlayer(CustomNetworkRoomPlayer player)
        {
            _player = player;
        }

        public void SetPlayerName(string playerName, bool isLocalPlayer)
        {
            txtPlayerName.text = playerName;
            StartCoroutine(IERebuildLayout());
            txtPlayerName.color = isLocalPlayer ? localPlayerNameColor : otherPlayerNameColor;
        }

        private IEnumerator IERebuildLayout()
        {
            yield return new WaitForEndOfFrame();
            gameObject.SetActive(!gameObject.activeSelf);
            gameObject.SetActive(!gameObject.activeSelf);
        }
        
        public void SetAgent(AgentContainer agent, bool isLockIn = false)
        {
            _agentSeleted = agent;
            txtAgentName.text = isLockIn ? agent.AgentName : "Picking...";

            if (_agentSeleted != null)
            {
                iconAgentSelected.gameObject.SetActive(true);
                iconAgentSelected.sprite = agent.icon;
            }
            else
            {
                iconAgentSelected.gameObject.SetActive(false);
            }
        }

        private void OnClickSwapPosition()
        {
            var localPlayer = CustomNetworkManager.Instance.LocalPlayer;
            if (localPlayer && _player)
            {
                if (localPlayer.isServer)
                {
                    CustomNetworkManager.Instance.SwapPlayerIndex(localPlayer.index, _player.index);
                }
                else
                {
                    localPlayer.CmdIndexChanged(localPlayer.index, _player.index);
                }
            }
        }
    }
}