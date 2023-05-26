using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace GeniusCrate.Utility
{
    public class DailyRewardUIElement : MonoBehaviour
    {
        public bool showRewardName;

        [Header("UI Elements")]
        public TMP_Text textDay;                
        public TMP_Text textReward;             
        public Image imageRewardBackground; 
        public Image imageReward;           
        public Color colorClaim;            
        private Color colorUnclaimed;       

        [Header("Internal")]
        public int day;

        [HideInInspector]
        public Reward reward;

        public DailyRewardState state;

        public enum DailyRewardState
        {
            UNCLAIMED_AVAILABLE,
            UNCLAIMED_UNAVAILABLE,
            CLAIMED
        }

        void Awake()
        {
            colorUnclaimed = imageReward.color;
        }

        public void Initialize()
        {
            textDay.text = string.Format("Day {0}", day.ToString());
            if (reward.reward > 0)
            {
                if (showRewardName)
                {
                    textReward.text = reward.reward + " " + reward.rewardName;
                }
                else
                {
                    textReward.text = reward.reward.ToString();
                }
            }
            else
            {
                textReward.text = reward.rewardName.ToString();
            }
            imageReward.sprite = reward.icon;
        }

        // Refreshes the UI
        public void Refresh()
        {
            switch (state)
            {
                case DailyRewardState.UNCLAIMED_AVAILABLE:
                    imageRewardBackground.color = colorClaim;
                    break;
                case DailyRewardState.UNCLAIMED_UNAVAILABLE:
                    imageRewardBackground.color = colorUnclaimed;
                    break;
                case DailyRewardState.CLAIMED:
                    imageRewardBackground.color = colorClaim;
                    break;
            }
        }
    }
}