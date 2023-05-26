
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;

namespace GeniusCrate.Utility
{

    public class DailyRewardsUI : MonoBehaviour
    {
        public GameObject DailyRewardPanel;
        public DailyRewardUIElement dailyRewardPrefab;
        [SerializeField] Button buttonShowDailyReward;

        [Header("Panel Reward Message")]
        public GameObject panelReward;
        public TMP_Text textReward;
        public Button buttonCloseReward;
        public Image imageReward;

        [Header("Panel Reward")]
        public Button buttonClaim;
        public Button buttonClose;
        public TMP_Text textTimeDue;
        [SerializeField] Transform content;

        private bool readyToClaim;
        private List<DailyRewardUIElement> dailyRewardsUI = new List<DailyRewardUIElement>();

        private DailyRewards dailyRewards;

        void Awake()
        {
            DailyRewardPanel.gameObject.SetActive(false);
            dailyRewards = DailyRewards.GetInstance(0);
        }

        void Start()
        {
            InitializeDailyRewardsUI();

            buttonClose.gameObject.SetActive(false);

            buttonClaim.onClick.AddListener(() =>
            {
                dailyRewards.ClaimPrize();
                readyToClaim = false;
                UpdateUI();
            });

            buttonCloseReward.onClick.AddListener(() =>
            {
                var keepOpen = dailyRewards.keepOpen;
                panelReward.SetActive(false);
                DailyRewardPanel.gameObject.SetActive(keepOpen);
            });

            buttonClose.onClick.AddListener(() =>
            {
                DailyRewardPanel.gameObject.SetActive(false);
            });
            UpdateUI();
            buttonShowDailyReward.onClick.AddListener(() => { DailyRewardPanel.gameObject.SetActive(true); });
        }

        void OnEnable()
        {
            DailyRewards.onClaimPrize += OnClaimPrize;
            DailyRewards.onInitialize += OnInitialize;
        }

        void OnDisable()
        {
            if (dailyRewards != null)
            {
                DailyRewards.onClaimPrize -= OnClaimPrize;
                DailyRewards.onInitialize -= OnInitialize;
            }
        }
        private void InitializeDailyRewardsUI()
        {
            for (int i = 0; i < dailyRewards.rewards.Count; i++)
            {
                int day = i + 1;
                var reward = dailyRewards.GetReward(day);

                DailyRewardUIElement dailyRewardUI = Instantiate(dailyRewardPrefab, content);
                dailyRewardUI.day = day;
                dailyRewardUI.reward = reward;
                dailyRewardUI.Initialize();

                dailyRewardsUI.Add(dailyRewardUI);
            }
        }

        public void UpdateUI()
        {
            dailyRewards.CheckRewards();

            bool isRewardAvailableNow = false;

            var lastReward = dailyRewards.lastReward;
            var availableReward = dailyRewards.availableReward;

            foreach (var dailyRewardUI in dailyRewardsUI)
            {
                var day = dailyRewardUI.day;

                if (day == availableReward)
                {
                    dailyRewardUI.state = DailyRewardUIElement.DailyRewardState.UNCLAIMED_AVAILABLE;

                    isRewardAvailableNow = true;
                }
                else if (day <= lastReward)
                {
                    dailyRewardUI.state = DailyRewardUIElement.DailyRewardState.CLAIMED;
                }
                else
                {
                    dailyRewardUI.state = DailyRewardUIElement.DailyRewardState.UNCLAIMED_UNAVAILABLE;
                }

                dailyRewardUI.Refresh();
            }

            buttonClaim.gameObject.SetActive(isRewardAvailableNow);
            buttonClose.gameObject.SetActive(!isRewardAvailableNow);
            if (isRewardAvailableNow)
            {
                textTimeDue.text = "You can claim your reward!";
            }
            readyToClaim = isRewardAvailableNow;
        }
        private void CheckTimeDifference()
        {
            if (!readyToClaim)
            {
                TimeSpan difference = dailyRewards.GetTimeDifference();

                if (difference.TotalSeconds <= 0)
                {
                    readyToClaim = true;
                    UpdateUI();
                    return;
                }

                string formattedTs = dailyRewards.GetFormattedTime(difference);

                textTimeDue.text = string.Format("Come back in {0}", formattedTs);
            }
        }
        private void OnClaimPrize(int day)
        {
            panelReward.SetActive(true);

            var reward = dailyRewards.GetReward(day);
            var unit = reward.rewardName;
            var rewardQt = reward.reward;
            imageReward.sprite = reward.icon;
            if (rewardQt > 0)
            {
                textReward.text = string.Format("You got {0} {1}!", reward.reward, unit);
            }
            else
            {
                textReward.text = string.Format("You got {0}!", unit);
            }
            reward.GrandReward();
        }

        private void OnInitialize(bool error, string errorMessage)
        {
            if (!error)
            {
                var showWhenNotAvailable = dailyRewards.keepOpen;
                var isRewardAvailable = dailyRewards.availableReward > 0;

                UpdateUI();
                DailyRewardPanel.gameObject.SetActive(showWhenNotAvailable || (!showWhenNotAvailable && isRewardAvailable));
                CheckTimeDifference();

                StartCoroutine(TickTime());
            }
        }

        private IEnumerator TickTime()
        {
            for (; ; )
            {
                dailyRewards.TickTime();
                CheckTimeDifference();
                yield return null;
            }
        }
    }
}