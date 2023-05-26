
using System;
using UnityEngine;
using System.Collections;

namespace GeniusCrate.Utility
{

    public abstract class DailyRewardsCore<T> : MonoBehaviour where T : DailyRewardsCore<T>
    {
        [HideInInspector] public int instanceId;
        [HideInInspector] public bool isSingleton = true;
        [HideInInspector] public string worldClockUrl = "http://worldclockapi.com/api/json/est/now";
        [HideInInspector] public string worldClockFMT = "yyyy-MM-dd'T'HH:mmzzz";

        [HideInInspector] public bool useWorldClockApi = true;
        [HideInInspector] public WorldClock worldClock;

        [HideInInspector] public string errorMessage;
        [HideInInspector] public bool isErrorConnect;
        public DateTime now;

        [HideInInspector] public int maxRetries = 3;
        public static Action<bool, string> onInitialize;

        protected bool isInitialized = false;


        public IEnumerator InitializeDate()
        {
            if (useWorldClockApi)
            {
                if (WorldClockBuilder.currentState == WorldClockBuilder.State.NotInitialized)
                {

                    yield return StartCoroutine(WorldClockBuilder.InitializeWorldClock(worldClockUrl, maxRetries, worldClockFMT));
                }
                else if (WorldClockBuilder.currentState == WorldClockBuilder.State.Initializing)
                {

                    while (WorldClockBuilder.currentState == WorldClockBuilder.State.Initializing)
                        yield return null;
                }

                if (WorldClockBuilder.currentState == WorldClockBuilder.State.Initialized)
                {
                    worldClock = WorldClockBuilder.instance;
                    now = WorldClockBuilder.worldClockDate;
                    isInitialized = true;
                }
                else if (WorldClockBuilder.currentState == WorldClockBuilder.State.FailedToInitialize)
                {
                    isErrorConnect = true;
                    errorMessage = WorldClockBuilder.errorMessage;
                }
            }
            else
            {
                now = DateTime.Now;
                isInitialized = true;
            }
        }

        public void RefreshTime()
        {
            if (useWorldClockApi)
                now = WorldClockBuilder.worldClockDate;
            else
                now = DateTime.Now;
        }

        public static T GetInstance(int id = 0)
        {
            var instances = FindObjectsOfType<T>();
            for (int i = 0; i < instances.Length; i++)
            {
                if ((instances[i]).instanceId == id)
                    return instances[i];
            }
            return null;
        }
        public virtual void TickTime()
        {
            if (!isInitialized)
                return;

            now = now.AddSeconds(Time.unscaledDeltaTime);

            if (useWorldClockApi)
                WorldClockBuilder.worldClockDate = now;
        }

        public string GetFormattedTime(TimeSpan span)
        {
            return string.Format("{0:D2}:{1:D2}:{2:D2}", span.Hours, span.Minutes, span.Seconds);
        }

        protected virtual void Awake()
        {
            if (isSingleton)
            {
                DontDestroyOnLoad(this.gameObject);

                var instanceCount = GetInstanceCount();

                if (instanceCount > 1)
                    Destroy(gameObject);
            }
        }
        private int GetInstanceCount()
        {
            var instances = FindObjectsOfType<T>();
            var count = 0;
            for (int i = 0; i < instances.Length; i++)
            {
                if ((instances[i]).instanceId == instanceId)
                    count++;
            }
            return count;
        }

        protected virtual void OnApplicationPause(bool pauseStatus)
        {
            if (!pauseStatus)
                RefreshTime();
        }
    }
}