
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace GeniusCrate.Utility
{
    public class TimedRewards : DailyRewardsCore<TimedRewards>
    {
        public DateTime lastRewardTime;
        public TimeSpan timer;
        public float maxTime = 3600f;
        public bool isRewardRandom = false;

        public List<Reward> rewards;

        public static Action onCanClaim;

        public static Action<int> onClaimPrize;

        private bool canClaim;

        private const string TIMED_REWARDS_TIME = "TimedRewardsTime";
        private const string FMT = "O";

        void Start()
        {
            StartCoroutine(InitializeTimer());
        }

        private IEnumerator InitializeTimer()
        {
            yield return StartCoroutine(base.InitializeDate());

            if (base.isErrorConnect)
            {
                onInitialize?.Invoke(true, base.errorMessage);
            }
            else
            {
                LoadTimerData();

                onInitialize?.Invoke(false, "");

                CheckCanClaim();
            }
        }

        void LoadTimerData()
        {
            string lastRewardTimeStr = PlayerPrefs.GetString(GetTimedRewardsTimeKey());

            if (!string.IsNullOrEmpty(lastRewardTimeStr))
            {
                lastRewardTime = DateTime.ParseExact(lastRewardTimeStr, FMT, CultureInfo.InvariantCulture);
                timer = (lastRewardTime - now).Add(TimeSpan.FromSeconds(maxTime));
            }
            else
            {
                timer = TimeSpan.FromSeconds(maxTime);
            }
        }

        protected override void OnApplicationPause(bool pauseStatus)
        {
            base.OnApplicationPause(pauseStatus);
            LoadTimerData();
            CheckCanClaim();
        }

        public override void TickTime()
        {
            base.TickTime();
            if (isInitialized)
            {
                if (!canClaim)
                {
                    timer = timer.Subtract(TimeSpan.FromSeconds(Time.unscaledDeltaTime));
                    CheckCanClaim();
                }
            }
        }

        public void CheckCanClaim()
        {
            if (timer.TotalSeconds <= 0)
            {
                canClaim = true;
                onCanClaim?.Invoke();
            }
            else
            {
                PlayerPrefs.SetString(GetTimedRewardsTimeKey(), now.Add(timer - TimeSpan.FromSeconds(maxTime)).ToString(FMT));
            }
        }

        private string GetTimedRewardsTimeKey()
        {
            if (instanceId == 0)
                return TIMED_REWARDS_TIME;

            return string.Format("{0}_{1}", TIMED_REWARDS_TIME, instanceId);
        }
        public void ClaimReward(int rewardIdx)
        {
            PlayerPrefs.SetString(GetTimedRewardsTimeKey(), now.Add(timer - TimeSpan.FromSeconds(maxTime)).ToString(FMT));
            timer = TimeSpan.FromSeconds(maxTime);

            canClaim = false;

            onClaimPrize?.Invoke(rewardIdx);
        }

        public string GetFormattedTime()
        {
            if (timer.Days > 0)
                return string.Format("{0:D2} days {1:D2}:{2:D2}:{0:D3}", timer.Days, timer.Hours, timer.Minutes, timer.Seconds);
            else
                return string.Format("{0:D2}:{1:D2}:{2:D2}", timer.Hours, timer.Minutes, timer.Seconds);
        }

        public void Reset()
        {
            PlayerPrefs.DeleteKey(GetTimedRewardsTimeKey());
            canClaim = true;
            timer = TimeSpan.FromSeconds(0);
            onCanClaim?.Invoke();
        }
        public Reward GetReward(int index)
        {
            return rewards[index];
        }
    }
}