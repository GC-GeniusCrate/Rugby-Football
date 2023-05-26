using System;
using System.Collections.Generic;
using UnityEngine;

namespace GeniusCrate.Utility
{
    public class AchievementManager : Screen
    {
        public List<AchievementQuest> achievementQuests = new List<AchievementQuest>();
        public List<AchevementElement> achievementUIElement = new List<AchevementElement>();
        public AchevementElement achievementElementTemp;
        public Transform content;
        bool isPopulated;

        public static Action<int, int> OnAchevement;// Trigger This action for Quest Increment With Achievement Quest Id and Amound Achieved;
        private void OnEnable()
        {
            OnAchevement += OnAchieved;
            MenuManager.OnAchievementButtonTrigger += InitScreen;
        }
        private void OnDisable()
        {
            OnAchevement -= OnAchieved;
            MenuManager.OnAchievementButtonTrigger -= InitScreen;

        }
        //test
        public void TriggerAchievement(int id)
        {
            OnAchevement?.Invoke(id, 3);
        }


        public override void InitScreen()
        {
            base.InitScreen();
           
        }

        private void Start()
        {
            if (!isPopulated)
                PopulateAchievementElements();
        }
        public virtual void PopulateAchievementElements()
        {
            foreach (var achievement in achievementQuests)
            {
                AchevementElement element = Instantiate(achievementElementTemp, content);
                element.mAchievementManager = this;
                element.PopulateElement(achievement);
                achievementUIElement.Add(element);
                element.gameObject.SetActive(true);
            }
            isPopulated = true;
        }

        public void UpdateAchievementUiElement(int _achievementID)
        {
            int i = 0;
            foreach (var achievement in achievementQuests)
            {
                if (achievement.mQuestID == _achievementID)
                {
                    achievement.SaveAchievement();
                    if (achievementUIElement.Count > 0)
                    {
                        achievementUIElement[i].PopulateElement(achievement);
                    }
                }
                i++;
            }
        }

        public virtual void OnAchieved(int _achievementID, int _achievementAmount)
        {
            foreach (var achievement in achievementQuests)
            {
                if (achievement.mQuestID == _achievementID && achievement.currentLevel < achievement.achievementLevels.Count)
                {
                    achievement.OnAchievedTheQuest(_achievementAmount);
                    achievement.SaveAchievement();
                    UpdateAchievementUiElement(_achievementID);
                }
            }
        }
    }
}
