namespace GeniusCrate.Utility
{
    public class MissionElement : UIElement
    {
        public MissionManager mMissionManager;
        public void PopulateElement(MissionQuest quest)
        {
            icon.sprite = quest.icon;
            title.text = quest.mQuestTitle;
            description.text = quest.mQuestDescription;
            progressHandle.fillAmount = (float)quest.countAchieved / (float)quest.countToAchive;
            progressText.text = quest.countAchieved + "/" + quest.countToAchive; ;

            collectBtn.interactable = quest.isCompleted;
            elementId = quest.mQuestID;
            if (quest.IsRewardGranded)
            {
                collectBtn.gameObject.SetActive(false);
                return;
            }
            if (quest.isCompleted)
            {
                collectBtn.onClick.AddListener(() =>
                {
                    quest.reward.GrandReward();
                    quest.IsRewardGranded = true;
                    mMissionManager.UpdateAchievementUiElement(elementId);
                });
            }
        }

    }
}

