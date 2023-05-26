using UnityEngine.Analytics;
using UnityEngine;
using System.Collections.Generic;

namespace GeniusCrate.Utility
{
    public class AnalyticsManager : MonoBehaviour
    {
        private void OnEnable()
        {
            GameManager.OnGameOver += OnGameOver;
            GameManager.OnGameStart += OnGameStart;
            MenuManager.OnShopButtonTrigger += OnShopOpened;

            AchievementQuest.OnAchievementCompleted += AchievementComplete;

            //ADManager.OnRewardedAdCompleted += OnRewardedAdCompleted;
            //ADManager.OnRewardedAdStarted += OnShowRewardedAd;
            //ADManager.OnRewardedAdSkip += OnRewardedAdSkiped;

            CrossPromotionButton.OnCrossPromoButtonClick += OnCrossPromoClick;
            SocialShare.OnSocialShare += OnSocialShare;
        }

        private void OnDisable()
        {
            GameManager.OnGameOver -= OnGameOver;
            GameManager.OnGameStart -= OnGameStart;

            MenuManager.OnShopButtonTrigger -= OnShopOpened;
            AchievementQuest.OnAchievementCompleted -= AchievementComplete;

            //ADManager.OnRewardedAdCompleted -= OnRewardedAdCompleted;
            //ADManager.OnRewardedAdStarted -= OnShowRewardedAd;
            //ADManager.OnRewardedAdSkip -= OnRewardedAdSkiped;

            CrossPromotionButton.OnCrossPromoButtonClick -= OnCrossPromoClick;
            SocialShare.OnSocialShare -= OnSocialShare;

        }


        void OnGameOver()
        {
            
            AnalyticsEvent.GameOver(null, new Dictionary<string, object> {
            { "IAPCurrency", GameManager.Instance._IAPCurrency },
            { "score", GameManager.Instance._Score },
            { "InGameCurrency",  GameManager.Instance._InGameCurrency },

        });
        }

        void OnGameStart()
        {
            AnalyticsEvent.GameStart(new Dictionary<string, object> {
            { "IAPCurrency", GameManager.Instance._IAPCurrency },
            { "score", GameManager.Instance._Score },
            { "InGameCurrency",  GameManager.Instance._InGameCurrency },

        });
        }
        void OnSocialShare(string result, string targetName)
        {
            Analytics.CustomEvent("social_share_click", new Dictionary<string, object>
            {
              { "result",result},
              { "targetName",targetName },
             });
        }

        void OnCrossPromoClick(int id)
        {
            Analytics.CustomEvent("cross_promotion_click", new Dictionary<string, object>
            {
              { "PromoID", id.ToString()},
              { "timer", Time.realtimeSinceStartup },
             });
        }
        void OnShopOpened()
        {
            AnalyticsEvent.StoreOpened(StoreType.Soft);
            Debug.LogError("Shop Analytics");
        }
        void OnTutoralComplete()
        {
            AnalyticsEvent.TutorialComplete();
        }
        void OnTutorialSkip()
        {
            AnalyticsEvent.TutorialSkip();
        }
        void OnTutorialStart()
        {
            AnalyticsEvent.TutorialStart();

        }
        void OnStoreItemClicked(string itemID)
        {
            AnalyticsEvent.StoreItemClick(StoreType.Soft, itemID);
        }
        void OnIAPTransactions(string context, float price, string itemId)
        {
            AnalyticsEvent.IAPTransaction(context, price, itemId);
        }
        void OnShowRewardedAd(string placementId)
        {
            AnalyticsEvent.AdStart(true, AdvertisingNetwork.UnityAds, placementId);
        }
        void OnRewardedAdCompleted(string placementId)
        {
            AnalyticsEvent.AdComplete(true, AdvertisingNetwork.UnityAds, placementId);
        }
        void OnRewardedAdSkiped(string placementId)
        {
            AnalyticsEvent.AdSkip(true, AdvertisingNetwork.UnityAds, placementId);
        }
        void AchievementComplete(string id)
        {
            AnalyticsEvent.AchievementUnlocked(id, new Dictionary<string, object> {
            { "IAPCurrency", GameManager.Instance._IAPCurrency },
            { "score", GameManager.Instance._Score },
            { "InGameCurrency",  GameManager.Instance._InGameCurrency },
        });
        }

        protected void OnApplicationQuit()
        {
            Analytics.CustomEvent("user_end_session", new Dictionary<string, object>
            {
              { "force_exit", GameManager.Instance.isGameStarted},
              { "timer", Time.realtimeSinceStartup }
             });
        }
    }
}

