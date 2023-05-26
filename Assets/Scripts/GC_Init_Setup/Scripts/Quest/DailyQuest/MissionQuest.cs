using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace GeniusCrate.Utility
{
    [CreateAssetMenu(fileName = "Mission Quest", menuName = "GeniusCrate/Mission Quest")]

    public class MissionQuest : Quest
    {
        public Sprite icon;
        public int countToAchive;
        [HideInInspector] public int countAchieved;
        [HideInInspector] public bool isCompleted;
        public Reward reward;
        [HideInInspector] public bool IsRewardGranded;

        private void OnEnable()
        {
            countAchieved = PlayerPrefs.GetInt("Mission" + mQuestID + "Achieved", 0);
            isCompleted = PlayerPrefs.GetInt("Mission" + mQuestID + "Iscompleted", 0) == 1;
            IsRewardGranded = PlayerPrefs.GetInt("Mission" + mQuestID + "IsrewardGranded", 0) == 1;
        }
        public override void OnAchievedTheQuest(int _amount)
        {
            if (isCompleted) return;
            if (countAchieved < countToAchive)
            {
                countAchieved = Mathf.Clamp(countAchieved + _amount, countAchieved, countToAchive);
                if (countAchieved >= countToAchive)
                {
                    isCompleted = true;
                    PlayerPrefs.SetInt("Mission" + mQuestID + "Iscompleted", isCompleted ? 1 : 0);

                }
            }
        }
        public void SaveAchievement()
        {
            PlayerPrefs.SetInt("Mission" + mQuestID + "IsrewardGranded", IsRewardGranded ? 1 : 0);
            PlayerPrefs.SetInt("Mission" + mQuestID + "Iscompleted", isCompleted ? 1 : 0);
            PlayerPrefs.SetInt("Mission" + mQuestID + "Achieved", countAchieved);
        }

    }
}
