
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;

namespace GeniusCrate.Utility
{
    public class TimedRewardUI : MonoBehaviour
    {
        public GameObject timedRewardPanel;


        [Header("Panel Reward")]
        public Button buttonClaim;
        public TMP_Text textInfo;

        [Header("Panel Reward Message")]
        public GameObject panelReward;
        public TMP_Text textReward;
        public Button buttonCloseReward;
        public Image imageRewardMessage;

        /* [Header("Panel Available Rewards")]
         public GameObject panelAvailableRewards;  
         public GameObject rewardPrefab;           
         public Button buttonCloseAvailable;       
         public GridLayoutGroup rewardsGroup;      
         public ScrollRect scrollRect;      */

        private TimedRewards timedRewards;

        // private List<TimedRewardUI> rewardsUI = new List<TimedRewardUI>();

        void Awake()
        {
            timedRewardPanel.SetActive(false);
            timedRewards = TimedRewards.GetInstance(0);
        }

        void Start()
        {
            // InitializeAvailableRewardsUI();
            buttonClaim.interactable = false;
            /*    panelAvailableRewards.SetActive(false);

                buttonCloseAvailable.onClick.AddListener(() =>
                {
                    panelAvailableRewards.SetActive(false);
                    buttonClaim.interactable = true;
                });*/

            buttonClaim.onClick.AddListener(() =>
            {
                buttonClaim.interactable = false;
                if (timedRewards.rewards.Count == 1)
                {
                    ClaimReward(0);


                }
                else if (timedRewards.isRewardRandom)
                {
                    ClaimReward(UnityEngine.Random.Range(0, timedRewards.rewards.Count));
                }
                else
                {
                    //panelAvailableRewards.SetActive(true);
                }
            });

            buttonCloseReward.onClick.AddListener(() =>
            {
                //  panelAvailableRewards.SetActive(false);
                panelReward.SetActive(false);
            });

        }

        void OnEnable()
        {
            TimedRewards.onCanClaim += OnCanClaim;
            TimedRewards.onInitialize += OnInitialize;
        }

        void OnDisable()
        {
            if (timedRewards != null)
            {
                TimedRewards.onCanClaim -= OnCanClaim;
                TimedRewards.onInitialize -= OnInitialize;
            }
        }

        private void UpdateTextInfo()
        {
            if (timedRewards.timer.TotalSeconds > 0)
                textInfo.text = timedRewards.GetFormattedTime();
        }
        /*
                private void InitializeAvailableRewardsUI()
                {
                    if (timedRewards.rewards.Count > 1)
                    {
                        for (int i = 0; i < timedRewards.rewards.Count; i++)
                        {
                            var reward = timedRewards.GetReward(i);

                            GameObject dailyRewardGo = GameObject.Instantiate(rewardPrefab) as GameObject;

                            TimedRewardUI rewardUI = dailyRewardGo.GetComponent<TimedRewardUI>();

                            rewardUI.index = 0;

                            rewardUI.transform.SetParent(rewardsGroup.transform);
                            dailyRewardGo.transform.localScale = Vector2.one;

                            rewardUI.button.onClick.AddListener(OnClickReward(i));

                            rewardUI.reward = reward;
                            rewardUI.Initialize();

                            rewardsUI.Add(rewardUI);
                        }
                    }
                }
                private UnityEngine.Events.UnityAction OnClickReward(int index)
                {
                    return () =>
                    {
                        panelAvailableRewards.SetActive(false);
                        ClaimReward(index);
                    };
                }
        */
        private void ClaimReward(int index)
        {
            timedRewards.ClaimReward(index);

            panelReward.SetActive(true);

            var reward = timedRewards.GetReward(index);
            var unit = reward.rewardName;
            var rewardQt = reward.reward;
            imageRewardMessage.sprite = reward.icon;

            if (rewardQt > 0)
                textReward.text = string.Format("{0} {1}", reward.reward, unit);
            else
                textReward.text = string.Format("{0}", unit);
            reward.GrandReward();
        }

        private void OnCanClaim()
        {
            buttonClaim.interactable = true;
            textInfo.text = "Reward Ready!";
            if (!buttonClaim.gameObject.activeInHierarchy)
                buttonClaim.gameObject.SetActive(true);
        }

        private void OnInitialize(bool error, string errorMessage)
        {
            if (!error)
            {
                //timedRewardPanel.gameObject.SetActive(true);
                StartCoroutine(TickTime());
            }
        }

        private IEnumerator TickTime()
        {
            for (; ; )
            {
                timedRewards.TickTime();
                UpdateTextInfo();
                yield return null;
            }
        }
    }
}