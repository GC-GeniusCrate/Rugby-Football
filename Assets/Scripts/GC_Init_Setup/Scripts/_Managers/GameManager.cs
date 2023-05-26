using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
namespace GeniusCrate.Utility
{
    [DisallowMultipleComponent]

    public class GameManager : MonoBehaviourSingletonPersistent<GameManager>
    {
        private int score;
        public int _Score
        {
            get
            {
                return score;
            }
            set
            {
                score = value;
                if (score > HighScore)
                {
                    HighScore = score;
                }

            }
        }

        public bool mTutorial
        {
            get
            {
                return PlayerPrefs.GetInt("IsInTutorial", 0) == 0;
            }
            set
            {
                PlayerPrefs.SetInt("IsInTutorial", value ? 0 : 1);
            }
        }

        public string mSelectedCharacterKey
        {
            get
            {
                return PlayerPrefs.GetString("SelectedCharacter", "Man");
            }
            set
            {
                PlayerPrefs.SetString("SelectedCharacter", value);
                OnCharacterChange?.Invoke(value);
            }
        }

        public float scoreMultiplier = 1f;

        public int HighScore;

        public int _InGameCurrency;
        public int _IAPCurrency;

        public bool isGameStarted;
        public bool PauseGame;

        public WorldType _GameWorldType;

        public float distance;

        public static Action OnGameOver;
        public static Action OnGameStart;
        public static Action OnGameReEnter;
        public static Action BackToMainMenu;
        public static Action OnSummonEnemy;


        public static Action<string> OnCharacterChange;




        public static Action<int, int> OnGameCurrencyChange;
        public static Action<int> OnGameScoreChange;

        [SerializeField] int restartableWithAdCount = 1;

        public float gameTime;
        [SerializeField] float startTimer = 3;
        bool timerStart;

        public CharacterController player;

        private void OnEnable()
        {
            MenuManager.OnStartButtonTrigged += StartGame;
            HighScore = PlayerPrefs.GetInt("HighScore", 0);
        }
        public virtual void StartGame()
        {
            startTimer = 3;
            timerStart = true;
        }

        public void SetTimer()
        {
            startTimer = 3;
            timerStart = true;
        }
        void ReEnterGame()
        {
            isGameStarted = true;
            OnGameReEnter?.Invoke();
        }
        public void SummonEnemies()
        {
            OnSummonEnemy?.Invoke();
        }
        public virtual void GameOver()
        {
            OnGameOver?.Invoke();
            PlayerPrefs.SetInt("HighScore", HighScore);

            //isGameStarted = false;
            startTimer = 3;
            gameTime = 0;

            if (restartableWithAdCount <= 0)
            {
                isGameStarted = false;
                OnBackToMenu();
            }
            else
            {
                PopUp.Instance.RequestPopUp(new PopUpRequest("GameOver", "Restart Game?", (result) =>
                {
                    if (result)
                    {
                        bool isRewarded = false;
                        //AdManager.Instance.InitializeRewardedAds((a, b, c) =>
                        //{
                        //    ReEnterGame();
                        //    isRewarded = true;
                        //    restartableWithAdCount--;
                        //}, (a, b) =>
                        // {
                        //     if (isRewarded) return;
                        //     OnBackToMenu();
                        //     isGameStarted = false;
                        // });
                        //AdManager.Instance.ShowRewardedAd();
                    }
                    else
                    {
                        OnBackToMenu();
                        isGameStarted = false;
                    }
                }, PopUpType.Feedback, false));
            }
        }

        void OnBackToMenu()
        {
            Invoke(nameof(BackToMain), 1f);
        }
        void BackToMain()
        {
            BackToMainMenu?.Invoke();
            restartableWithAdCount = 1;
            //PlayfabManager.Instance.SendLeaderBoard("Top", HighScore);  //Leaderboard
        }
        public virtual void AddScore(int value)
        {
            _Score += Mathf.RoundToInt(value * scoreMultiplier);
            OnGameScoreChange?.Invoke(_Score);
            AchievementManager.OnAchevement?.Invoke(1, value);

        }

        public virtual void AddIAPCurrency(int value)
        {
            _IAPCurrency += value;
            OnGameCurrencyChange?.Invoke(_InGameCurrency, _IAPCurrency);

        }
        public virtual void AddInGameCurrency(int value)
        {
            _InGameCurrency += value;
            //PlayfabManager.Instance.AddVirtualCurrency("GC", value);
            //OnGameCurrencyChange?.Invoke(_InGameCurrency, _IAPCurrency);

        }
        public void UpdateAllCurrency(int _IAP, int IGC)
        {
            _IAPCurrency = _IAP;
            _InGameCurrency = IGC;
            OnGameCurrencyChange?.Invoke(_InGameCurrency, _IAPCurrency);
        }
        public int _Coin;

        public static Action OnCoinValueChange;


        private void Start()
        {
            HighScore = PlayerPrefs.GetInt("HighScore", 0);
        }
        private void Update()
        {
            if (isGameStarted) gameTime += Time.deltaTime;
            if (timerStart) startTimer -= Time.deltaTime;
            if (startTimer < 0 && timerStart)
            {
                startTimer = 0;
                isGameStarted = true;
                timerStart = false;
                OnGameStart?.Invoke();
            }

        }
        public void UpdateCoin(int coin)
        {
            _Coin += coin;
            OnCoinValueChange?.Invoke();
        }

        private void OnDestroy()
        {
            MenuManager.OnStartButtonTrigged -= StartGame;
        }
    }
}
public enum WorldType
{
    StrightWorld,
    TurnWorld
}