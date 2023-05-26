using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
namespace GeniusCrate.Utility
{
    public class PopUp : MonoBehaviourSingleton<PopUp>
    {
        [SerializeField] Image picture;
        [SerializeField] protected TMP_Text title;
        [SerializeField] protected TMP_Text content;
        [SerializeField] Button okayButton;
        [SerializeField] Button cancelButton;
        Queue<PopUpRequest> popUpRequestQueue = new Queue<PopUpRequest>();
        PopUpRequest currentPopUpRequest;
        [SerializeField] bool ShowNextPopup = true;
        [SerializeField] protected float popUpDelayTime;
        [SerializeField] float infoTime = 1;

        void UpdateTitle(string translatedValue)
        {
            title.text = translatedValue;
        }
        void UpdateContent(string translatedValue)
        {
            content.text = translatedValue;
        }
        public void RequestPopUp(PopUpRequest request)
        {
            popUpRequestQueue.Enqueue(request);

        }
        public void Update()
        {
            if (ShowNextPopup && popUpRequestQueue.Count > 0)
            {
                Invoke(nameof(ShowPopUp), popUpDelayTime);
                ShowNextPopup = false;
            }
        }

        protected virtual void ShowPopUp()
        {
            if (popUpRequestQueue.Count <= 0) return;

            okayButton.gameObject.SetActive(true);
            cancelButton.gameObject.SetActive(true);

            currentPopUpRequest = popUpRequestQueue.Dequeue();

            if (currentPopUpRequest.withMultiLanguage)
            {
                currentPopUpRequest.title.LocalizeString(title);    //Localizing With Extention Method
                currentPopUpRequest.content.LocalizeString(content);
            }
            else
            {
                title.text = currentPopUpRequest.title;
                content.text = currentPopUpRequest.content;
            }

            this.transform.GetChild(0).gameObject.SetActive(true);

            if (currentPopUpRequest.type == PopUpType.Info)
            {
                Invoke(nameof(HideInfoPopUp), infoTime);
                okayButton.gameObject.SetActive(false);
                cancelButton.gameObject.SetActive(false);
            }
            else if (currentPopUpRequest.type == PopUpType.SingleAnswer) cancelButton.gameObject.SetActive(false);
        }
        public virtual void OnButtonPress(bool on)
        {
            if (currentPopUpRequest == null) return;
            currentPopUpRequest.callback?.Invoke(on);
            this.transform.GetChild(0).gameObject.SetActive(false);
            ShowNextPopup = true;
        }

        void HideInfoPopUp()
        {
            this.transform.GetChild(0).gameObject.SetActive(false);
            ShowNextPopup = true;
            currentPopUpRequest.callback?.Invoke(true);
        }
    }
    [Serializable]
    public class PopUpRequest
    {
        public string title;
        public string content;
        public Action<bool> callback = null;
        public bool withMultiLanguage;
        public PopUpType type;
        public PopUpRequest(string _title, string _content, Action<bool> _callback, PopUpType _type = PopUpType.Feedback, bool _withMultiLanguage = true)
        {
            title = _title;
            content = _content;
            callback = _callback;
            type = _type;
            withMultiLanguage = _withMultiLanguage;
        }
    }

    public enum PopUpType
    {
        Info,
        Feedback,
        SingleAnswer
    }
}