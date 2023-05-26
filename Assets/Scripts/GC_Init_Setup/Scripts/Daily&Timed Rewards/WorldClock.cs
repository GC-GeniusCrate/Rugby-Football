
using UnityEngine;
using System;
using System.Collections;
using System.Globalization;

namespace GeniusCrate.Utility
{
    [Serializable]
    public class WorldClock
    {
        public string currentDateTime;
        public string utcOffset;
        public string dayOfTheWeek;
        public string timeZoneName;
        public double currentFileTime;
        public string ordinalDate;
        public string serviceResponse;
    }
    public static class WorldClockBuilder
    {
        public static string errorMessage = String.Empty;
        private static int connectrionRetries;
        public static WorldClock instance;
        public static DateTime worldClockDate;
        public static State currentState;

        public enum State
        {
            NotInitialized,
            Initializing, Initialized, FailedToInitialize
        };

        public static IEnumerator InitializeWorldClock(string worldClockUrl, int maxRetries, string worldClockFMT)
        {
            currentState = State.Initializing;
            string result = String.Empty;
            while (connectrionRetries < maxRetries)
            {
                WWW www = new WWW(worldClockUrl);
                while (!www.isDone)
                    yield return null;

                if (!string.IsNullOrEmpty(www.error))
                {
                    connectrionRetries++;
                    Debug.LogError("Error Loading World Clock. Retrying connection " + connectrionRetries);
                    errorMessage = www.error;
                }
                else
                {
                    result = www.text;
                    break;
                }
            }
            if (!string.IsNullOrEmpty(result))
            {
                var clockJson = result;
                var worldClock = JsonUtility.FromJson<WorldClock>(clockJson);
                instance = worldClock;
                var dateTimeStr = worldClock.currentDateTime;

                worldClockDate = DateTime.ParseExact(dateTimeStr, worldClockFMT, CultureInfo.InvariantCulture);
                worldClockDate = worldClockDate.AddSeconds(DateTime.Now.Second);

                var time = string.Format("{0:D4}/{1:D2}/{2:D2} {3:D2}:{4:D2}:{5:D2}", worldClockDate.Year, worldClockDate.Month, worldClockDate.Day, worldClockDate.Hour, worldClockDate.Minute, worldClockDate.Second);
                currentState = State.Initialized;
            }
            else
            {
                Debug.LogError("Error Loading World Clock:" + errorMessage);
                currentState = State.FailedToInitialize;
            }
        }
    }
}