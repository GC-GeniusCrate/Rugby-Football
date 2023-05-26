using UnityEngine.UI;
using UnityEngine;
using System;
using System.Collections.Generic;
namespace GeniusCrate.Utility
{


    [CreateAssetMenu(fileName = "Achievement Quest", menuName = "GeniusCrate/Achievement Quest")]

    public class AchievementQuest : Quest
    {
        public static Action<string> OnAchievementCompleted;

        public int currentLevel;
        public List<AchievementLevel> achievementLevels = new List<AchievementLevel>();


        private void OnEnable()
        {
            currentLevel = PlayerPrefs.GetInt("Achievement" + mQuestID + "Level", 0);

            if (currentLevel < achievementLevels.Count)
            {
                achievementLevels[currentLevel].countAchieved = PlayerPrefs.GetInt("Achievement" + mQuestID + "Achieved", 0);
                achievementLevels[currentLevel].isCompleted = PlayerPrefs.GetInt("Achievement" + mQuestID + "Iscompleted" + currentLevel, 0) == 1;
            }

        }
        public override void OnAchievedTheQuest(int _amount)
        {
            if (currentLevel >= achievementLevels.Count) return;
            achievementLevels[currentLevel].IncrementTheCount(_amount);
            if (achievementLevels[currentLevel].isCompleted)
            {
                OnAchievementCompleted?.Invoke(mQuestID + "_" + currentLevel);

            }
        }


        public void SaveAchievement()
        {
            PlayerPrefs.SetInt("Achievement" + mQuestID + "Level", currentLevel);
            if (currentLevel >= achievementLevels.Count) return;
            PlayerPrefs.SetInt("Achievement" + mQuestID + "Achieved", achievementLevels[currentLevel].countAchieved);
            PlayerPrefs.SetInt("Achievement" + mQuestID + "Iscompleted" + currentLevel, achievementLevels[currentLevel].isCompleted ? 1 : 0);

        }
    }


    [Serializable]
    public class AchievementLevel
    {
        public Sprite icon;
        public int countToAchive;
        public Reward reward;

        [HideInInspector]
        public int countAchieved;
        [HideInInspector]
        public bool isCompleted;


        public void IncrementTheCount(int amount)
        {
            if (isCompleted) return;
            if (countAchieved < countToAchive)
            {
                countAchieved = Mathf.Clamp(countAchieved + amount, countAchieved, countToAchive);
                if (countAchieved >= countToAchive)
                {
                    isCompleted = true;
                }

            }
        }

    }
}
