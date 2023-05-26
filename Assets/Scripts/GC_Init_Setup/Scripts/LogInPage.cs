using PlayFab.ClientModels;
using UnityEngine.UI;
using UnityEngine;

namespace GeniusCrate.Utility
{
    public class LogInPage : MonoBehaviour
    {

        [SerializeField] Button _guestLogin;
        [SerializeField] Button _facebookLogin;
        private void OnEnable() => PlayfabManager.OnloginSuccess += OnPlayerLogIn;
        private void Start()
        {
            _facebookLogin.onClick.AddListener(() => PlayfabManager.Instance.LogInWithFacebook());
            _guestLogin.onClick.AddListener(() => PlayfabManager.Instance.LogInAsGuest());
        }

        private void OnPlayerLogIn(LoginResult result)
        {
            this.gameObject.SetActive(false);
        }
    }
}