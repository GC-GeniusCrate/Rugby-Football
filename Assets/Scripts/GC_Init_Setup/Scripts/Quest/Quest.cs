using UnityEngine;
namespace GeniusCrate.Utility
{
    public class Quest : ScriptableObject
    {
        public int mQuestID;
        public string mQuestTitle;
        public string mQuestDescription;


        public virtual void OnAchievedTheQuest(int _amount)
        {

        }
    }
}
