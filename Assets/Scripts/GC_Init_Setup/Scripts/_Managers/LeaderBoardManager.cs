using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;

namespace GeniusCrate.Utility
{
    public class LeaderBoardManager : Screen
    {
        [Header("LeaderBoard Details: ")]
        [SerializeField] List<Leaderboard> leaderboard;
        [SerializeField] GameObject _leaderBoardElementPrefab;
        private void OnEnable()
        {
            MenuManager.OnLeaderBoardButtonTrigger += InitScreen;
        }
        private void OnDisable()
        {
            MenuManager.OnLeaderBoardButtonTrigger -= InitScreen;
        }
        public override void CloseScreen()
        {
            base.CloseScreen();
            HideBoards();
        }
        public override void InitScreen()
        {
            base.InitScreen();
            SetBoardActive(0);

            foreach (Leaderboard board in leaderboard)
            {
                PlayfabManager.Instance.GetLeaderBoard(board._name, board._maxcount, (result) =>
                {
                    foreach (Transform item in board._contentTransform)
                    {
                        Destroy(item.gameObject);
                    }
                    foreach (var item in result.Leaderboard)
                    {
                        Transform element = Instantiate(_leaderBoardElementPrefab, board._contentTransform).transform;
                        element.GetChild(0).GetComponent<TMPro.TMP_Text>().text = (item.Position + 1).ToString();
                        element.GetChild(1).GetComponent<TMPro.TMP_Text>().text = (item.DisplayName).ToString();
                        element.GetChild(2).GetComponent<TMPro.TMP_Text>().text = (item.StatValue).ToString();
                    }
                });
            }
        }

        void HideBoards()
        {
            foreach (Leaderboard board in leaderboard)
            {
                board.boardUI.SetActive(false);
            }
        }
        public void SetBoardActive(int index)
        {
            ScreenTitleText.text = leaderboard[index]._name;
            HideBoards();
            leaderboard[index].boardUI.SetActive(true);
        }

    }
    [System.Serializable]
    public class Leaderboard
    {
        public string _name;
        public int _maxcount;
        public Transform _contentTransform;
        public GameObject boardUI;
    }
}