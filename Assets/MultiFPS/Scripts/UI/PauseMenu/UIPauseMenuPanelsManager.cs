using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace MultiFPS.UI
{
    public class UIPauseMenuPanelsManager : MonoBehaviour
    {
        [System.Serializable]
        public class Panel
        {
            public GameObject PanelOverlay;
            public Button ShowButton;
            public Button[] HideButtons;
            public bool isHideInitialize = true;

            public void Initialize()
            {
                ShowButton.onClick.AddListener(ShowPanel);

                foreach (var button in HideButtons)
                {
                    button.onClick.AddListener(HidePanel);
                }
                
                if(isHideInitialize)
                {
                    HidePanel();
                }
            }
            void ShowPanel()
            {
                PanelOverlay.SetActive(true);
            }
            public void HidePanel()
            {
                PanelOverlay.SetActive(false);
            }
        }

        public Panel[] panels;

        private void Awake()
        {
            for (int i = 0; i < panels.Length; i++)
            {
                panels[i].ShowButton.onClick.AddListener(HideAllPanels);
                panels[i].Initialize();
            }
        }
        void HideAllPanels()
        {
            for (int i = 0; i < panels.Length; i++)
            {
                panels[i].HidePanel();
            }
        }
    }
}