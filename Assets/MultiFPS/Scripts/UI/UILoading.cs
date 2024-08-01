using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MultiFPS.Scripts.UI
{
    public class UILoading : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI txtProgress;
        [SerializeField] private GameObject content;
        [SerializeField] private Image imgFill; 
        
        public static UILoading Instance { get; set; }

        private void Awake () 
        { 
            if (Instance != null && Instance != this) 
            { 
                Destroy(this); 
            } 
            else 
            { 
                DontDestroyOnLoad(this);
                Instance = this; 
            } 
        }

        public void Show (bool isShow)
        {
            content.SetActive(isShow);
            ResetUI();
        }
        
        public void Show (bool isShow, string message)
        {
            content.SetActive(isShow);
            ResetUI();
        }

        public void UpdateProgress (float percent)
        {
            txtProgress.text = $"{(int)(percent * 100)}%";  
            DOTween.To(x => imgFill.fillAmount = x, imgFill.fillAmount, percent, 0.25f);
        }

        private void ResetUI()
        {
            txtProgress.text = "0%";
            imgFill.fillAmount = 0;
        }
    }
}