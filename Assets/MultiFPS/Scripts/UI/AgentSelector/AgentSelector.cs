using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Mirror;
using MultiFPS.Gameplay.Gamemodes;
using MultiFPS.Room;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

namespace  MultiFPS.UI
{
    public class AgentSelector : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] private Button btnLockIn;
        [SerializeField] private Button btnStart;
        [SerializeField] private ItemAgent itemAgentPrefab;
        [SerializeField] private Transform itemParent;
        [SerializeField] private TextMeshProUGUI txtMap;
        
        private CustomNetworkRoomPlayer customNetworkRoomPlayer;

        [Space(20)] 
        [SerializeField] private ItemPlayerAgent[] teamA;
        [SerializeField] private ItemPlayerAgent[] teamB;

        public List<ItemPlayerAgent> playerSlots;
        
        
        private List<ItemAgent> _itemAgents;
        private Dictionary<int, GameObject> _agentPools = new Dictionary<int, GameObject>();
        private int _selectedID = -1;

        public static AgentSelector Instance;


        private void Awake()
        {
            Instance = this;
        }

        private void OnEnable()
        {
            ClientFrontend.ShowCursor(true);
        }

        private void OnDisable()
        {
            ClientFrontend.ShowCursor(false);
        }


        private void Start()
        {
            btnLockIn.onClick.AddListener(OnClickLockIn);
            btnStart.onClick.AddListener(OnClickStart);
            InitAgentList(ClientInterfaceManager.Instance.agentContainers);
        }

        #region APIs
        public void InitAgentList(AgentContainer[] agents)
        {
            _itemAgents = new List<ItemAgent>();
            foreach (var agent in agents)
            {
                ItemAgent item = Instantiate(itemAgentPrefab, itemParent);
                _itemAgents.Add(item);
                item.Setup(agent.SpawnID, _selectedID, agent.icon);
            }
        }

        public void SetupRoom(CustomNetworkRoomPlayer customNetworkRoomPlayer, RoomProperties room)
        {
            this.customNetworkRoomPlayer = customNetworkRoomPlayer;
            txtMap.text = room.P_Map;
            
            playerSlots = new List<ItemPlayerAgent>();
            int numOfPlayerEachTeam = room.P_MaxPlayers / 2;
            
            if (numOfPlayerEachTeam >= teamA.Length || numOfPlayerEachTeam >= teamB.Length)
            {
                Debug.LogError($"Failed to setup players: {nameof(numOfPlayerEachTeam)} is out of range");
                return;
            }
            
            for (int i = 0; i < teamA.Length; i++)
            {
                teamA[i].gameObject.SetActive(i < numOfPlayerEachTeam);
                
                if(teamA[i].gameObject.activeSelf)
                    playerSlots.Add(teamA[i]);
            }
            
            for (int i = 0; i < teamB.Length; i++)
            {
                teamB[i].gameObject.SetActive(i < numOfPlayerEachTeam);
                
                if(teamA[i].gameObject.activeSelf)
                    playerSlots.Add(teamB[i]);
            }
        }
        
        public void SelectAgent(int selectID)
        {
            // Reload local layout
            if(_selectedID == selectID)
                return;
            
            _selectedID = selectID;
            foreach (var item in _itemAgents)
            {
                item.ReloadLayout(_selectedID);
            }

            customNetworkRoomPlayer.SetPlayerAgentID(agentID: selectID);
            
            SpawnAgent();

        }

        public void ShowStartButton()
        {
            btnLockIn.gameObject.SetActive(false);
            btnStart.gameObject.SetActive(true);
        }
        
        #endregion

        #region private methods
        private void OnClickLockIn()
        {
            customNetworkRoomPlayer.CmdChangeReadyState(true);
        }

        private void OnClickStart()
        {
            
        }

        private void SpawnAgent()
        {
            AgentContainer agent = ClientInterfaceManager.Instance.GetAgent(_selectedID);

            if (agent != null)
            {
                foreach (var currentAgent in _agentPools.Values)
                {
                    currentAgent.SetActive(false);
                }


                if (_agentPools.ContainsKey(_selectedID))
                {
                    _agentPools[_selectedID].SetActive(true);
                }
                else
                {
                    var newAgent = Instantiate(agent.MenuPrefab);
                    _agentPools.Add(_selectedID, newAgent);
                }
            }
            else
            {
                Debug.LogError($"Failed to spawn agent: agent with id {_selectedID} not found");
            }
        }
        #endregion
    }
}
