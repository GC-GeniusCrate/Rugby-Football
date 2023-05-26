using UnityEngine.UI;
using UnityEngine;
using System;
using UnityEngine.Localization.Settings;
using System.Collections.Generic;
using TMPro;
namespace GeniusCrate.Utility
{
    public class SettingsManager : Screen
    {
        [SerializeField] Toggle SoundToggle;
        [SerializeField] Toggle MusicToggle;
        [SerializeField] Toggle NotificationToggle;
        [SerializeField] TMP_Dropdown LanguageDropDown;
        [SerializeField] Button facebookConnect;

        public static Action<bool> OnMusicValueChange;
        public static Action<bool> OnSoundValueChange;
        public static Action<bool> OnNotificationValueChange;

        public static Action OnFacebookConnectTrigger;

        [SerializeField] GameObject LocalizationPopUp;
        public void OnEnable()
        {
            MenuManager.OnSettingsButtonTrigger += InitScreen;
            UIManager.OnLanguageLocalizerInitialized += SetUpLanguageScreen;
            PlayfabManager.OnFaceBookLinked += CheckFaceBookConnected;

        }
        public void OnDisable()
        {
            MenuManager.OnSettingsButtonTrigger -= InitScreen;
            UIManager.OnLanguageLocalizerInitialized -= SetUpLanguageScreen;
            PlayfabManager.OnFaceBookLinked -= CheckFaceBookConnected;

        }
        public override void InitScreen()
        {
            base.InitScreen();
            CheckFaceBookConnected();
        }

        private void CheckFaceBookConnected()
        {
            facebookConnect.interactable = !PlayfabManager.Instance.isFaceBookConnected;
            if (!PlayfabManager.Instance.isFaceBookConnected) facebookConnect.GetComponentInChildren<TMP_Text>().text = "Connect";
            else facebookConnect.GetComponentInChildren<TMP_Text>().text = "Connected";
        }

        private void Start()
        {
            SoundToggle.isOn = PlayerPrefs.GetInt("SoundEffectState", 1) == 1;
            SoundToggle.onValueChanged.AddListener((isOn) =>
            {
                OnSoundValueChange?.Invoke(isOn);
            });

            MusicToggle.isOn = PlayerPrefs.GetInt("MusicState", 1) == 1;
            MusicToggle.onValueChanged.AddListener((isOn) =>
            {
                OnMusicValueChange?.Invoke(isOn);
            });

            NotificationToggle.isOn = PlayerPrefs.GetInt("NotificationState", 1) == 1;
            if (NotificationToggle != null)
            {
                NotificationToggle.onValueChanged.AddListener((isOn) =>
                {
                    OnNotificationValueChange?.Invoke(isOn);
                });
            }
        }
        public void SetUpLanguageScreen()
        {
            if (PlayerPrefs.HasKey("LanguageIndex"))
            {
                SetLanguage(PlayerPrefs.GetInt("LanguageIndex"));
            }
            var options = new List<TMP_Dropdown.OptionData>();
            int selected = 0;
            for (int i = 0; i < LocalizationSettings.AvailableLocales.Locales.Count; ++i)
            {
                var locale = LocalizationSettings.AvailableLocales.Locales[i];
                if (LocalizationSettings.SelectedLocale == locale)
                    selected = i;
                options.Add(new TMP_Dropdown.OptionData(locale.name.Split(' ')[0]));
            }
            LanguageDropDown.options = options;

            LanguageDropDown.value = selected;
            LanguageDropDown.onValueChanged.AddListener(SetLanguage);
        }
        public virtual void OnFacebookConnectButton()
        {
            OnFacebookConnectTrigger?.Invoke();
        }

        public void OpenURL(string url)
        {
            Application.OpenURL(url);
        }
        public void SetLanguage(int index)
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
            PlayerPrefs.SetInt("LanguageIndex", index);
        }
        public virtual void OnLanguageButton()
        {
            LocalizationPopUp.SetActive(true);
        }
        public virtual void OnLanguageCloseButton()
        {
            LocalizationPopUp.SetActive(false);

        }
    }
}
