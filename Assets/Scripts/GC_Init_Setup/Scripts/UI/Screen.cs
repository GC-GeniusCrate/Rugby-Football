using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace GeniusCrate.Utility
{
    public class Screen : MonoBehaviour
    {
        [SerializeField]
        protected TMP_Text ScreenTitleText;
        [SerializeField]
        protected GameObject ScreenObject;
        [SerializeField]
        protected Button closeButton;


        public virtual void Awake()
        {
            closeButton.onClick.AddListener(CloseScreen);
        }

        public virtual void InitScreen()
        {
            ScreenObject.SetActive(true);

        }
        public virtual void CloseScreen()
        {
            ScreenObject.SetActive(false);
        }
    }
}