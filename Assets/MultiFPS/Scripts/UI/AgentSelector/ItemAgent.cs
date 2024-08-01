using System;
using UnityEngine;
using UnityEngine.UI;

namespace MultiFPS.UI
{
    public class ItemAgent : MonoBehaviour
    {
        [Header("Refs")] 
        [SerializeField] private Button btnClick;
        [SerializeField] private Image imgBg;
        [SerializeField] private Image imgAgentIcon;
        
        [Space, Header("Data")]
        [SerializeField] private int id;
        [SerializeField] private Sprite bgNormal;
        [SerializeField] private Sprite bgSelect;

        private void Start()
        {
            btnClick.onClick.AddListener(OnClickSelect);
        }


        public void Setup(int id, int selectedID, Sprite agentIcon)
        {
            this.id = id;
            imgAgentIcon.sprite = agentIcon;
            
            ReloadLayout(selectedID);
        }

        public void ReloadLayout(int selectedID)
        {
            imgBg.sprite = id == selectedID ? bgSelect : bgNormal;
        }

        private void OnClickSelect()
        {
            try
            {
                AgentSelector.Instance.SelectAgent(id);
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to change agent: {e}");
                throw;
            }
        }
    }
}