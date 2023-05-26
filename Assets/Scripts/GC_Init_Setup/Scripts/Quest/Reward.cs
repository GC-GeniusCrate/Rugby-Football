using UnityEngine;
namespace GeniusCrate.Utility
{
    [System.Serializable]
   // [CreateAssetMenu(fileName = "Reward", menuName = "GeniusCrate/Reward")]
    public class Reward 
    {
        public string rewardName;
        public int reward;
        public Sprite icon;

        public virtual void GrandReward()
        {
            GameManager.Instance.AddInGameCurrency(reward);
        }
    }
}

