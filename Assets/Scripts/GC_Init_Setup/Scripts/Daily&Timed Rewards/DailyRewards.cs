
using UnityEngine;
using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;

namespace GeniusCrate.Utility
{
    public class DailyRewards : DailyRewardsCore<DailyRewards>
    {
        public List<Reward> rewards;
        public DateTime lastRewardTime;
        [HideInInspector] public int availableReward;
        [HideInInspector] public int lastReward;
        public bool keepOpen = false;

        public static Action<int> onClaimPrize;

        private const string LAST_REWARD_TIME = "LastRewardTime";
        private const string LAST_REWARD = "LastReward";
        private const string DEBUG_TIME = "DebugTime";
        private const string FMT = "O";

        public TimeSpan debugTime;

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
                LoadDebugTime();
                CheckRewards();

                onInitialize?.Invoke(false, "");
            }
        }

        protected override void OnApplicationPause(bool pauseStatus)
        {
            base.OnApplicationPause(pauseStatus);
            CheckRewards();
        }

        public TimeSpan GetTimeDifference()
        {
            TimeSpan difference = (lastRewardTime - now);
            difference = difference.Subtract(debugTime);
            return difference.Add(new TimeSpan(0, 24, 0, 0));
        }

        private void LoadDebugTime()
        {
            int debugHours = PlayerPrefs.GetInt(GetDebugTimeKey(), 0);
            debugTime = new TimeSpan(debugHours, 0, 0);
        }

        public void CheckRewards()
        {
            string lastClaimedTimeStr = PlayerPrefs.GetString(GetLastRewardTimeKey());
            lastReward = PlayerPrefs.GetInt(GetLastRewardKey());

            if (!string.IsNullOrEmpty(lastClaimedTimeStr))
            {
                lastRewardTime = DateTime.ParseExact(lastClaimedTimeStr, FMT, CultureInfo.InvariantCulture);

                DateTime advancedTime = now.AddHours(debugTime.TotalHours);

                TimeSpan diff = advancedTime - lastRewardTime;

                int days = (int)(Math.Abs(diff.TotalHours) / 24);
                if (days == 0)
                {
                    availableReward = 0;
                    return;
                }

                if (days >= 1 && days < 2)
                {
                    if (lastReward == rewards.Count)
                    {
                        availableReward = 1;
                        lastReward = 0;
                        return;
                    }

                    availableReward = lastReward + 1;
                    return;
                }

                if (days >= 2)
                {
                    availableReward = 1;
                    lastReward = 0;

                }
            }
            else
            {
                availableReward = 1;
            }
        }
        public void ClaimPrize()
        {
            if (availableReward > 0)
            {
                onClaimPrize?.Invoke(availableReward);
                PlayerPrefs.SetInt(GetLastRewardKey(), availableReward);


                string lastClaimedStr = now.AddHours(debugTime.TotalHours).ToString(FMT);
                PlayerPrefs.SetString(GetLastRewardTimeKey(), lastClaimedStr);
                PlayerPrefs.SetInt(GetDebugTimeKey(), (int)debugTime.TotalHours);
            }
            else if (availableReward == 0)
            {
                Debug.LogError("Error! The player is trying to claim the same reward twice.");
            }

            CheckRewards();
        }

        private string GetLastRewardKey()
        {
            if (instanceId == 0)
                return LAST_REWARD;

            return string.Format("{0}_{1}", LAST_REWARD, instanceId);
        }

        private string GetLastRewardTimeKey()
        {
            if (instanceId == 0)
                return LAST_REWARD_TIME;

            return string.Format("{0}_{1}", LAST_REWARD_TIME, instanceId);
        }

        private string GetDebugTimeKey()
        {
            if (instanceId == 0)
                return DEBUG_TIME;

            return string.Format("{0}_{1}", DEBUG_TIME, instanceId);
        }

        public Reward GetReward(int day)
        {
            return rewards[day - 1];
        }
        public void Reset()
        {
            PlayerPrefs.DeleteKey(GetLastRewardKey());
            PlayerPrefs.DeleteKey(GetLastRewardTimeKey());
            PlayerPrefs.DeleteKey(GetDebugTimeKey());
        }
    }
}