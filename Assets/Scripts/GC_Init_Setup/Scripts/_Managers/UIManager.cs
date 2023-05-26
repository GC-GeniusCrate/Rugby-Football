using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;

namespace GeniusCrate.Utility
{
    [DisallowMultipleComponent]
    public class UIManager : MonoBehaviour
    {
        [SerializeField] protected TMP_Text ScoreText;
        [SerializeField] protected TMP_Text IAPCurrencyText;
        [SerializeField] protected TMP_Text InGameCurrencyText;


        [SerializeField] GameObject menuUi;
        [SerializeField] GameObject gameUi;

        [SerializeField] List<GameObject> uIPriorities;
        public static System.Action OnLanguageLocalizerInitialized;

        private void OnEnable()
        {
            GameManager.OnCoinValueChange += UpdateCoinInUI;

            GameManager.OnGameCurrencyChange += OnHUDValueChange;
            GameManager.OnGameScoreChange += OnScoreChange;

            MenuManager.OnStartButtonTrigged += GameStart;
            GameManager.BackToMainMenu += BackToMainMenu;

            StartCoroutine(SetUpLanguage());
        }

        private void Start()
        {
            UpdateCoinInUI();
        }
        public void OnRestartButtonPress()
        {
            GameManager.Instance.isGameStarted = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
       
        public void OnQuitButtonPress()
        {
            Application.Quit();
        }

        private void OnDisable()
        {
            GameManager.OnCoinValueChange -= UpdateCoinInUI;

            GameManager.OnGameCurrencyChange -= OnHUDValueChange;
            GameManager.OnGameScoreChange -= OnScoreChange;

            MenuManager.OnStartButtonTrigged -= GameStart;
            GameManager.BackToMainMenu -= BackToMainMenu;

        }
        private void GameStart()
        {
            menuUi.SetActive(false);
            gameUi.SetActive(true);
        }

        void BackToMainMenu()
        {
            menuUi.SetActive(true);
            gameUi.SetActive(false);
        }
        void UpdateCoinInUI()
        {

            // coinText.text = GameManager.Instance._Coin.ToString("N0");
        }
    

        IEnumerator SetUpLanguage()
        {
            yield return LocalizationSettings.InitializationOperation;
            OnLanguageLocalizerInitialized?.Invoke();
        }

        public virtual void OnHUDValueChange(int InGameCurrency, int IAPCurrency)
        {
            IAPCurrencyText.text = IAPCurrency.ToString();
            InGameCurrencyText.text = InGameCurrency.ToString();
        }
        public virtual void OnScoreChange(int score)
        {
            ScoreText.text = score.ToString();
        }
        private void OnValidate()
        {
            int i = 0;
            foreach (var item in uIPriorities)
            {
                item.transform.SetSiblingIndex(i);
                i++;
            }
        }
    }
}
