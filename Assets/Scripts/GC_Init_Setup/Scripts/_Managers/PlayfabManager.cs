using System.Collections.Generic;
using UnityEngine;
using PlayFab.ClientModels;
using PlayFab;
using Facebook.Unity;
using System;
using UnityEngine.Networking;
using System.Collections;

namespace GeniusCrate.Utility
{
    public class PlayfabManager : MonoBehaviourSingletonPersistent<PlayfabManager>
    {
        [Space(10)]
        [Header("PlayFab Info")]
        public string _playFabId;
        public string _sessionTicket;
        public string _displayName;
        public string _avatarURL;
        public bool isFaceBookConnected;

        public GetPlayerCombinedInfoRequestParams InfoRequestParameters;
        public static Action<PlayFab.ClientModels.LoginResult> OnloginSuccess;
        public static Action OnFaceBookLinked;

        [Space(10)]
        [Header("PlayerPrefs Keys")]
        [SerializeField] string playerCustemIdKey = "CustemID";
        [SerializeField] string authTypeKey = "AuthType";

        public string PlayerCustemId
        {
            get { return PlayerPrefs.GetString(playerCustemIdKey, ""); }
            set
            {
                string guid = string.IsNullOrEmpty(value) ? Guid.NewGuid().ToString() : value;
                PlayerPrefs.SetString(playerCustemIdKey, guid);
            }
        }

        private void Start()
        {
            if (!FB.IsInitialized) FB.Init(() => FB.ActivateApp());//initializing fb app
            Authenticate();// Auto Authrntication
            SettingsManager.OnFacebookConnectTrigger += LinkFacebookToPlayFab;
        }
        private void OnDisable()
        {
            SettingsManager.OnFacebookConnectTrigger -= LinkFacebookToPlayFab;

        }
        private void LinkFacebookToPlayFab()
        {
            FB.LogInWithReadPermissions(new List<string> { "public_profile", "email" }, Result =>
            {
                PlayFabClientAPI.LinkFacebookAccount(new LinkFacebookAccountRequest()
                {
                    AccessToken = AccessToken.CurrentAccessToken.TokenString
                },
                result =>
                {
                    isFaceBookConnected = true;
                    OnFaceBookLinked?.Invoke();
                }, OnError);

            });
        }

        private void OnHideUnity(bool isGameShown)
        {
            if (!isGameShown)
            {
                Time.timeScale = 0; //pause
            }
            else
            {
                Time.timeScale = 1; //resume
            }
        }
        private void OnError(PlayFabError error)
        {
            // Debug.LogError(error.ErrorMessage);
            Debug.LogError(error.Error);
            if (error.Error == PlayFabErrorCode.LinkedAccountAlreadyClaimed)
            {
                LogInWithPlayFab();
            }
        }



        #region Login
        void Authenticate()
        {
            if (!string.IsNullOrEmpty(PlayerCustemId))
            {
                PlayFabClientAPI.LoginWithCustomID(new LoginWithCustomIDRequest()
                {
                    TitleId = PlayFabSettings.TitleId,
                    CustomId = PlayerCustemId,
                    CreateAccount = true,
                    InfoRequestParameters = InfoRequestParameters
                }, (result) =>
                {
                    _playFabId = result.PlayFabId;
                    _sessionTicket = result.SessionTicket;
                    if (result.InfoResultPayload.PlayerProfile != null)
                    {
                        _displayName = result.InfoResultPayload.PlayerProfile.DisplayName;
                        _avatarURL = result.InfoResultPayload.PlayerProfile.AvatarUrl;
                    }
                    OnloginSuccess?.Invoke(result);
                    GetVirtualCurrency();//getting Currency

                    PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), result => isFaceBookConnected = result.AccountInfo.FacebookInfo != null, OnError);

                }, error => Debug.LogError(error.ErrorMessage));
            }
            else
            {
                LogInAsGuest();
            }
        }
        public void LogInAsGuest()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaClass up = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = up.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject contentResolver = currentActivity.Call<AndroidJavaObject>("getContentResolver");
        AndroidJavaClass secure = new AndroidJavaClass("android.provider.Settings$Secure");
        string android_id = secure.CallStatic<string>("getString", contentResolver, "android_id");

        PlayFabClientAPI.LoginWithAndroidDeviceID(new LoginWithAndroidDeviceIDRequest()
        {
            TitleId = PlayFabSettings.TitleId,
            AndroidDevice = SystemInfo.deviceModel,
            OS = SystemInfo.operatingSystem,
            CreateAccount = true,
            InfoRequestParameters = InfoRequestParameters,
            AndroidDeviceId = android_id

        }, OnSilentLoginSuccess, OnError);
#elif UNITY_IOS && !UNITY_EDITOR
            PlayFabClientAPI.LoginWithIOSDeviceID(new LoginWithIOSDeviceIDRequest()
            {
                DeviceId = SystemInfo.deviceUniqueIdentifier,
                CreateAccount = true
            }, OnSilentLoginSuccess, OnError);
#else
            Debug.Log("Logging as guest");
            PlayFabClientAPI.LoginWithCustomID(new LoginWithCustomIDRequest()
            {
                TitleId = PlayFabSettings.TitleId,
                CustomId = SystemInfo.deviceUniqueIdentifier,
                CreateAccount = true,
                InfoRequestParameters = InfoRequestParameters

            }, OnSilentLoginSuccess, OnError);
#endif
        }

        private void OnSilentLoginSuccess(PlayFab.ClientModels.LoginResult result)
        {
            _playFabId = result.PlayFabId;
            _sessionTicket = result.SessionTicket;
            if (result.InfoResultPayload.PlayerProfile != null)
            {
                _displayName = result.InfoResultPayload.PlayerProfile.DisplayName;
                _avatarURL = result.InfoResultPayload.PlayerProfile.AvatarUrl;
            }
            OnloginSuccess?.Invoke(result);
            GetVirtualCurrency();//getting Currency
            PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), result => isFaceBookConnected = result.AccountInfo.FacebookInfo != null, OnError);
        }
        public void LogInWithFacebook()
        {
            FB.LogInWithReadPermissions(new List<string> { "public_profile", "email" }, Result =>
            {
                LogInWithPlayFab();

            });
        }
        void LogInWithPlayFab()
        {
            PlayFabClientAPI.LoginWithFacebook(new LoginWithFacebookRequest
            {
                TitleId = PlayFabSettings.TitleId,
                AccessToken = AccessToken.CurrentAccessToken.TokenString,
                CreateAccount = true,
                InfoRequestParameters = InfoRequestParameters
            },
            OnPlayFabLoginSuccess, OnError
            );
        }

        private void OnPlayFabLoginSuccess(PlayFab.ClientModels.LoginResult result)
        {
            _playFabId = result.PlayFabId;
            _sessionTicket = result.SessionTicket;
            isFaceBookConnected = true;
            if (result.InfoResultPayload.PlayerProfile != null)
            {
                _displayName = result.InfoResultPayload.PlayerProfile.DisplayName;
                _avatarURL = result.InfoResultPayload.PlayerProfile.AvatarUrl;
            }

            PlayerCustemId = Guid.NewGuid().ToString();

            /* if (FB.IsLoggedIn) //Update Facebook Data to playfab
             {
                 FB.API("/me?fields=name", HttpMethod.GET, (result) => UpdateDisplayName(result.ResultDictionary["name"].ToString()));
                 string url = "http://graph.facebook.com/" + AccessToken.CurrentAccessToken.UserId + "/picture?type=square&width=300&height=300"; ;
                 // FB.API("me/picture?type=square&height=128&width=128", HttpMethod.GET, GetPicture);
                 UpdateDisplayPictureURL(url);
             }*/


            GetVirtualCurrency();//getting Currency

            PlayFabClientAPI.LinkCustomID(new LinkCustomIDRequest()
            {
                CustomId = PlayerCustemId,
                ForceLink = true
            }, null, null);
            OnloginSuccess?.Invoke(result);

        }

        #endregion

        #region Update Profile
        void UpdateDisplayName(string name)
        {
            PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest() { DisplayName = name }, (result) => _displayName = result.DisplayName, OnError);
        }
        void UpdateDisplayPictureURL(string url)
        {
            PlayFabClientAPI.UpdateAvatarUrl(new UpdateAvatarUrlRequest() { ImageUrl = url }, (result) => _avatarURL = url, OnError);
        }
        #endregion

        #region LeaderBoard
        public void SendLeaderBoard(string leaderboardName, int score)
        {
            PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest()
            {
                Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = leaderboardName,
                    Value = score
                }
            }
            }, (result) => Debug.Log("success"), OnError);
        }
        public void GetLeaderBoard(string leaderboardName, int maxResultCount, Action<GetLeaderboardResult> OnGettingLeaderboard, int _startPos = 0)
        {
            PlayFabClientAPI.GetLeaderboard(new GetLeaderboardRequest()
            {
                StatisticName = leaderboardName,
                StartPosition = _startPos,
                MaxResultsCount = maxResultCount
            }, OnGettingLeaderboard, OnError);
        }
        #endregion

        #region Virtual Currency
        public void GetVirtualCurrency()
        {
            PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), OnGettingUserInventory, OnError);
        }

        private void OnGettingUserInventory(GetUserInventoryResult result)
        {
            /* GameManager.Instance.AddIAPCurrency(result.VirtualCurrency["IC"]);
             GameManager.Instance.AddInGameCurrency(result.VirtualCurrency["GC"]);*/
            GameManager.Instance.UpdateAllCurrency(result.VirtualCurrency["IC"], result.VirtualCurrency["GC"]);
        }

        public void SubtractVirtualCurrency(string _key, int _amount)
        {
            if (GameManager.Instance._InGameCurrency > _amount)
            {
                PlayFabClientAPI.SubtractUserVirtualCurrency(new SubtractUserVirtualCurrencyRequest()
                {
                    VirtualCurrency = _key,
                    Amount = _amount
                }, OnSubtractVirtualCurrencySuccess, OnError);
            }
            else
                Debug.LogError("No enough Currency");
        }

        private void OnSubtractVirtualCurrencySuccess(ModifyUserVirtualCurrencyResult obj)
        {
            //Grand Something
            GetVirtualCurrency();
        }

        public void AddVirtualCurrency(string _key, int _amount)
        {
            PlayFabClientAPI.AddUserVirtualCurrency(new AddUserVirtualCurrencyRequest()
            {
                VirtualCurrency = _key,
                Amount = _amount
            }, OnAddVirtualCurrecySuccess, OnError);
        }

        private void OnAddVirtualCurrecySuccess(ModifyUserVirtualCurrencyResult obj)
        {
            GetVirtualCurrency();
        }
        #endregion

        public void GetStoreItems(string _storeId, Action<GetStoreItemsResult> storeResult)
        {
            PlayFabClientAPI.GetStoreItems(new GetStoreItemsRequest() { StoreId = _storeId }, result => storeResult?.Invoke(result), OnError);
        }

        public void GetCatalog(string _catalogVersion, Action<GetCatalogItemsResult> catalogResult)
        {
            PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest() { CatalogVersion = _catalogVersion }, result => catalogResult?.Invoke(result), OnError);
        }
        public void DownloadAndShowImage(string url, UnityEngine.UI.RawImage image)
        {
            StartCoroutine(DownloadImage(url, image));
        }
        IEnumerator DownloadImage(string MediaUrl, UnityEngine.UI.RawImage image)
        {
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError)
                Debug.Log(request.error);
            else
                image.texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
        }
        public void PurchaceItem(string _catalogVersion, string _itemId, int _price,string _virtualCurrency, Action<PurchaseItemResult> purchaseResult)
        {
            PlayFabClientAPI.PurchaseItem(new PurchaseItemRequest() { CatalogVersion = _catalogVersion, ItemId = _itemId, Price = _price, VirtualCurrency= _virtualCurrency }, result => purchaseResult?.Invoke(result), OnError);
        }


        public void GetInventory(Action<GetUserInventoryResult> _Result)
        {
            PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), result => _Result?.Invoke(result), OnError);
        }
    }

}
