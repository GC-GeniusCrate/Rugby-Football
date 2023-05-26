using System;
using UnityEngine;

namespace GeniusCrate.Utility
{
    public class MenuManager : MonoBehaviour
    {
        public static Action OnStartButtonTrigged;
        public static Action OnShopButtonTrigger;
        public static Action OnSettingsButtonTrigger;
        public static Action OnOptionButtonTrigger;
        public static Action OnMissionButtonTrigger;
        public static Action OnAchievementButtonTrigger;
        public static Action OnLeaderBoardButtonTrigger;
        public static Action OnCharacterSelectionTrigger;
        public virtual void OnStartButtonPress()
        {
            OnStartButtonTrigged?.Invoke();
        }
        public virtual void OnShopButtonPress()
        {
            OnShopButtonTrigger?.Invoke();
        }
        public virtual void OnOptionButtonPress()
        {
            OnOptionButtonTrigger?.Invoke();
        }
        public virtual void OnSettingsButtonPress()
        {
            OnSettingsButtonTrigger?.Invoke();
        }
        public virtual void OnMissionButtonPress()
        {
            OnMissionButtonTrigger?.Invoke();
        }
        public virtual void OnAchievementButtonPress()
        {
            OnAchievementButtonTrigger?.Invoke();
        }
        public virtual void OnQuitButtonPress()
        {
            Application.Quit();
        }
        public void OnLeaderBoardButtonPress()
        {
            OnLeaderBoardButtonTrigger?.Invoke();
        }
        public void OnCharacterSelectionButtonPress()
        {
            OnCharacterSelectionTrigger?.Invoke();
        }
    }
}
