namespace GeniusCrate.Utility
{
    public class AchevementElement : UIElement
    {

        public AchievementManager mAchievementManager;

        public void PopulateElement(AchievementQuest quest)
        {
            if (quest.currentLevel >= quest.achievementLevels.Count)
            {
                collectBtn.gameObject.SetActive(false);
                AchievementLevel achieve = quest.achievementLevels[quest.achievementLevels.Count - 1];
                icon.sprite = achieve.icon;
                title.text = quest.mQuestTitle;
                description.text = quest.mQuestDescription;
                progressHandle.fillAmount = (float)achieve.countAchieved / (float)achieve.countToAchive;
                progressText.text = achieve.countAchieved + "/" + achieve.countToAchive; ;
                return;
            }

            AchievementLevel achL = quest.achievementLevels[quest.currentLevel];
            icon.sprite = achL.icon;
            title.text = quest.mQuestTitle;
            description.text = quest.mQuestDescription;
            progressHandle.fillAmount = (float)achL.countAchieved / (float)achL.countToAchive;
            progressText.text = achL.countAchieved + "/" + achL.countToAchive; ;

            collectBtn.interactable = achL.isCompleted;

            collectBtn.onClick.RemoveAllListeners();
            elementId = quest.mQuestID;
            if (achL.isCompleted)
            {
                collectBtn.onClick.AddListener(() =>
                {
                    achL.reward.GrandReward();
                    quest.currentLevel++;
                    mAchievementManager.UpdateAchievementUiElement(elementId);
                });
            }
        }

    }
}

