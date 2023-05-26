using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace GeniusCrate.Utility
{
    [DisallowMultipleComponent]

    public class MissionManager : Screen
    {
        public List<MissionQuest> mMissions = new List<MissionQuest>();
        public List<MissionQuest> mDailyMissions = new List<MissionQuest>();

        [HideInInspector] public List<MissionElement> mDailyMissionElements = new List<MissionElement>();
        public Transform content;
        public Transform contentInGame;
        public MissionElement mMissionElement;
        bool isPopulated;


        public static Action<int, int> OnMissionTrigger;

        [Header("Mission Settings")]
        [SerializeField] int mMissionCountPerDay;
        [HideInInspector] public List<int> MissionIndex = new List<int>();
        int PreviousDay;


        private void OnEnable()
        {
            OnMissionTrigger += OnAchieved;
            MenuManager.OnMissionButtonTrigger += InitScreen;
        }

        public virtual void OnAchieved(int _achievementID, int _achievementAmount)
        {
            foreach (var achievement in mDailyMissions)
            {
                if (achievement.mQuestID == _achievementID)
                {
                    achievement.OnAchievedTheQuest(_achievementAmount);
                    achievement.SaveAchievement();
                    UpdateAchievementUiElement(_achievementID);
                }
            }
        }

        private void OnDisable()
        {
            OnMissionTrigger -= OnAchieved;
            MenuManager.OnMissionButtonTrigger -= InitScreen;

        }
        private void Start()
        {
            PreviousDay = PlayerPrefs.GetInt("PeviousDay", DateTime.Now.Day);

            if (DateTime.Now.Day != PreviousDay || !PlayerPrefs.HasKey("PeviousDay"))
                PopulateRandomDailyMissions();
            else
                PopulatePreviousMission();
            if (!isPopulated)
            {
                PopulateAchievementElements(content);
                PopulateAchievementElements(contentInGame);

            }
        }

        public void TriggerAchievement(int id)
        {
            OnMissionTrigger?.Invoke(id, 3);
        }

        public override void InitScreen()
        {
            base.InitScreen(); 
        }

        void PopulatePreviousMission()
        {
            mDailyMissions.Clear();
            for (int i = 0; i < mMissionCountPerDay; i++)
            {
                int _index = PlayerPrefs.GetInt("Dailymission" + i);
                mDailyMissions.Add(mMissions[_index]);
            }
        }
        void PopulateRandomDailyMissions()
        {
            for (int i = 0; i < mMissionCountPerDay; i++)
            {
                int _random = UnityEngine.Random.Range(0, mMissions.Count);
                while (MissionIndex.Contains(_random))
                {
                    _random = UnityEngine.Random.Range(0, mMissions.Count);
                   
                }
                MissionIndex.Add(_random);
                mDailyMissions.Add(mMissions[_random]);
                PlayerPrefs.SetInt("Dailymission" + i, _random);
            }
        }

        public virtual void PopulateAchievementElements(Transform content)
        {
            foreach (var achievement in mDailyMissions)
            {
                MissionElement element = Instantiate(mMissionElement, content);
                element.mMissionManager = this;
                element.PopulateElement(achievement);
                mDailyMissionElements.Add(element);
                element.gameObject.SetActive(true);
            }
            isPopulated = true;
        }
        public void UpdateAchievementUiElement(int _achievementID)
        {
            int i = 0;
            foreach (var achievement in mDailyMissions)
            {
                if (achievement.mQuestID == _achievementID)
                {
                    achievement.SaveAchievement();
                    mDailyMissionElements[i].PopulateElement(achievement);
                    mDailyMissionElements[i+mMissionCountPerDay].PopulateElement(achievement);
                }
                i++;
            }
        }
        private void OnApplicationQuit()
        {
            PlayerPrefs.SetInt("PeviousDay", DateTime.Now.Day);

        }

    }
}
