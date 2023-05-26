
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using GeniusCrate.Utility;

public class GameFramework : EditorWindow
{

    Texture2D HeaderTexture;
    Texture2D MenuScreenTexture;
    Texture2D ShopScreenTexture;
    Texture2D SettingsScreenTexture;
    Texture2D ManagerScreenTexture;

    Rect HeaderRect;
    Rect MenuRect;
    Rect ShopRect;
    Rect SettingsRect;
    Rect ManagerRect;

    GUISkin skin;
    UIManager UIManager;
    ShopManager ShopManager;
    SettingsManager SettingsManager;
    GameManager GameManager;
    AudioManager AudioManager;
    TimedRewards timedReward;
    DailyRewards dailyRewards;
   // ADManager adManager;
    SocialShare socialShare;
    PopUp popUpScreen;
    MissionManager missionManager;
    AchievementManager achievementManager;
    TimedRewardUI timedRewardScreen;
    DailyRewardsUI dailyRewardScreen;
    AnalyticsManager AnalyticsManager;
    LeaderBoardManager leaderboardManager;
    LogInPage LoginPage;
    PlayfabManager PlayFabManager;


    Transform IAPButtonParent;
    Transform SpecialButtonParent;

    [MenuItem("GeniusCrate/Game Framework")]
    static void OpenMenuEditor()
    {
        GameFramework window = (GameFramework)GetWindow(typeof(GameFramework));
        window.minSize = new Vector2(600, 300);
        window.Show();
    }

    private void OnEnable()
    {
        Init_Texture();
        skin = Resources.Load<GUISkin>("TitleSkin");

        UIManager = FindObjectOfType<UIManager>();
        ShopManager = FindObjectOfType<ShopManager>();
        GameManager = FindObjectOfType<GameManager>();
        AudioManager = FindObjectOfType<AudioManager>();
        SettingsManager = FindObjectOfType<SettingsManager>();
        timedReward = FindObjectOfType<TimedRewards>();
        dailyRewards = FindObjectOfType<DailyRewards>();
       // adManager = FindObjectOfType<ADManager>();
        socialShare = FindObjectOfType<SocialShare>();
        popUpScreen = FindObjectOfType<PopUp>();
        missionManager = FindObjectOfType<MissionManager>();
        achievementManager = FindObjectOfType<AchievementManager>();
        timedRewardScreen = FindObjectOfType<TimedRewardUI>();
        dailyRewardScreen = FindObjectOfType<DailyRewardsUI>();
        leaderboardManager = FindObjectOfType<LeaderBoardManager>();
        LoginPage= FindObjectOfType<LogInPage>();
        PlayFabManager = FindObjectOfType<PlayfabManager>();

        AnalyticsManager = FindObjectOfType<AnalyticsManager>();

        if (ShopManager)
        {/*
            IAPButtonParent = GameObject.Find("IAP-Content").transform;
            SpecialButtonParent = GameObject.Find("SpecialOffers-Content").transform;*/
        }
    }
    void Init_Texture()
    {
        HeaderTexture = new Texture2D(1, 1);
        HeaderTexture.SetPixel(0, 0, new Color(.15f, .15f, .15f));
        HeaderTexture.Apply();
        MenuScreenTexture = new Texture2D(1, 1);
        MenuScreenTexture.SetPixel(0, 0, new Color(.218f, .218f, .218f));
        MenuScreenTexture.Apply();
        ShopScreenTexture = new Texture2D(1, 1);
        ShopScreenTexture.SetPixel(0, 0, new Color(.2f, .2f, .2f));
        ShopScreenTexture.Apply();
        SettingsScreenTexture = new Texture2D(1, 1);
        SettingsScreenTexture.SetPixel(0, 0, new Color(.218f, .218f, .218f));
        SettingsScreenTexture.Apply();
        ManagerScreenTexture = new Texture2D(1, 1);
        ManagerScreenTexture.SetPixel(0, 0, new Color(.3f, .3f, .3f));
        ManagerScreenTexture.Apply();
    }
    private void OnGUI()
    {
        DrawLayout();
        DrawHeader();
        DrawMenuScreen();
        DrawSettingsScreen();
        DrawShopScreen();
        DrawManagerScreen();
    }
    void DrawLayout()
    {
        HeaderRect.x = 0;
        HeaderRect.y = 0;
        HeaderRect.width = UnityEngine.Screen.width;
        HeaderRect.height = 60;
        GUI.DrawTexture(HeaderRect, HeaderTexture);

        MenuRect.x = 0;
        MenuRect.y = 60 + 200;
        MenuRect.width = UnityEngine.Screen.width / 4;
        MenuRect.height = UnityEngine.Screen.height - 60 - 200;
        GUI.DrawTexture(MenuRect, MenuScreenTexture);

        ShopRect.x = UnityEngine.Screen.width / 4;
        ShopRect.y = 60 + 200;
        ShopRect.width = UnityEngine.Screen.width / 4;
        ShopRect.height = UnityEngine.Screen.height - 60 - 200;
        GUI.DrawTexture(ShopRect, ShopScreenTexture);

        SettingsRect.x = UnityEngine.Screen.width / 4 * 2;
        SettingsRect.y = 60 + 200;
        SettingsRect.width = UnityEngine.Screen.width / 4;
        SettingsRect.height = UnityEngine.Screen.height - 60 - 200;
        GUI.DrawTexture(SettingsRect, SettingsScreenTexture);

        ManagerRect.x = 0;
        ManagerRect.y = 60;
        ManagerRect.width = UnityEngine.Screen.width / 2f;
        ManagerRect.height = 200;
        GUI.DrawTexture(ManagerRect, ManagerScreenTexture);
    }

    void DrawHeader()
    {
        GUILayout.BeginArea(HeaderRect);
        skin.GetStyle("Title").contentOffset = new Vector2(-UnityEngine.Screen.width / 8, 0);
        GUI.Label(HeaderRect, "GeniusCrate - Game Framework", skin.GetStyle("Title"));
        skin.GetStyle("SubTitle").contentOffset = new Vector2(-UnityEngine.Screen.width / 8, 0);
        GUI.Label(HeaderRect, "Sceen Creation", skin.GetStyle("SubTitle"));
        GUILayout.EndArea();
    }


    void DrawShopScreen()
    {
        GUILayout.BeginArea(ShopRect);
        GUILayout.Label("Shop Screen", skin.GetStyle("ScreenTitle"));
        GUILayout.Space(20);
        if (GUILayout.Button("Create Shop Screen", GUILayout.Height(30)))
        {
            Canvas canvas = CheckForCanvas();
            if (canvas != null)
            {
                if (UIManager == null)
                    UIManager = canvas.gameObject.AddComponent<UIManager>();
                //  var mainMenu = FindObjectOfType<ShopManagerGC>();
                if (ShopManager) Debug.LogError("<color=red>Error:</color> <color=yellow>Already Have A Shop Screen!</color>");
                else
                {
                    GameObject Screen = Resources.Load<GameObject>("Shop/ShopScreen");
                    GameObject ShopScreen = Instantiate(Screen, canvas.transform);
                    ShopScreen.name = "ShopScreen";

                    ShopManager = ShopScreen.GetComponent<ShopManager>();
                    if (ShopManager)
                    {
                        IAPButtonParent = GameObject.Find("IAP-Content").transform;
                        SpecialButtonParent = GameObject.Find("SpecialOffers-Content").transform;
                    }
                    // ShopManager.gameObject.SetActive(false);
                }
            }
        }
        GUILayout.Space(20);
        if (GUILayout.Button("Create IAP Button", GUILayout.Height(30)))
        {
            ShopManager = FindObjectOfType<ShopManager>();
            if (!ShopManager) Debug.LogError("<color=red>Error:</color> <color=yellow>Create Shop Screen First To Create IAP Button!</color>");
            else
            {
                GameObject button = Resources.Load<GameObject>("Shop/IAPElement");
                GameObject IAPButton = Instantiate(button, IAPButtonParent);
                IAPButton.name = "IAPButton";
            }
        }
        GUILayout.Space(20);
        if (GUILayout.Button("Create IAP Special Button", GUILayout.Height(30)))
        {
            ShopManager = FindObjectOfType<ShopManager>();
            if (!ShopManager) Debug.LogError("<color=red>Error:</color> <color=yellow>Create Shop Screen First To Create IAP Button!</color>");
            else
            {
                GameObject button = Resources.Load<GameObject>("Shop/Special_IAP");
                GameObject IAPButton = Instantiate(button, SpecialButtonParent);
                IAPButton.name = "Special_IAP";
            }
        }
        GUILayout.Space(20);

        EditorGUILayout.HelpBox("Create The Shop Screen Element", MessageType.Info);

        GUILayout.EndArea();
    }
    void DrawSettingsScreen()
    {

        GUILayout.BeginArea(SettingsRect);
        GUILayout.Label("Settings Screen", skin.GetStyle("ScreenTitle"));
        GUILayout.Space(20);

        if (GUILayout.Button("Create Menu Screen", GUILayout.Height(30)))
        {

            Canvas canvas = CheckForCanvas();
            if (canvas != null)
            {
                if (UIManager == null)
                    UIManager = canvas.gameObject.AddComponent<UIManager>();
                var settingsScreen = FindObjectOfType<SettingsManager>();
                if (settingsScreen != null) Debug.LogError("<color=red>Error:</color> <color=yellow>Already Have A Settings Screen!</color>");
                else
                {
                    GameObject Screen = Resources.Load<GameObject>("Settings/SettingsScreen");
                    GameObject ShopScreen = Instantiate(Screen, canvas.transform);
                    ShopScreen.name = "SettingsScreen";
                    SettingsManager = ShopScreen.GetComponent<SettingsManager>();
                    //SettingsManager.gameObject.SetActive(false);
                }
            }
        }
        GUILayout.Space(20);

        EditorGUILayout.HelpBox("Create The Setting Screen Element", MessageType.Info);
        GUILayout.EndArea();
    }
    void DrawMenuScreen()
    {
        GUILayout.BeginArea(MenuRect);
        skin.GetStyle("ScreenTitle").contentOffset = new Vector2(UnityEngine.Screen.width / 16, 0);
        GUILayout.Label("Menu Screen", skin.GetStyle("ScreenTitle"));
        GUILayout.Space(20);

        if (GUILayout.Button("Create Menu Screen", GUILayout.Height(25)))
        {
            var canvas = FindObjectOfType<Canvas>();
            if (canvas == null) Debug.LogError("<color=red>Error:</color> <color=yellow>Create a UI Canvas First!</color>");
            else
            {
                var mainMenu = FindObjectOfType<MenuManager>();
                if (mainMenu != null) Debug.LogError("<color=red>Error:</color> <color=yellow>Already Have A Main Menu!</color>");
                else
                {
                    if (UIManager == null)
                        UIManager = canvas.gameObject.AddComponent<UIManager>();
                    canvas.GetComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                    canvas.GetComponent<CanvasScaler>().referenceResolution = new Vector2(1080, 1920);
                    GameObject menuScreen = Resources.Load<GameObject>("MainMenu/MainMenu");
                    GameObject menu = Instantiate(menuScreen, canvas.transform);
                    menu.name = "MainMenu";
                    menu.transform.SetAsFirstSibling();
                }
            }
        }
        GUILayout.Space(20);

        EditorGUILayout.HelpBox("Create The Menu Screen", MessageType.Info);
        GUILayout.EndArea();
    }
    void DrawManagerScreen()
    {

        GUILayout.BeginArea(ManagerRect);
        GUILayout.Label("Create: ", skin.GetStyle("ScreenTitle"));
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("GameManager", GUILayout.Height(25)))
        {
            if (this.GameManager) Debug.LogError("<color=red>Error:</color> <color=yellow>Already Have A GameManager!</color>");
            else
            {
                GameObject GameManager = new GameObject("GameManager");
                this.GameManager = GameManager.AddComponent<GameManager>();
            }

        }

        if (GUILayout.Button("AudioManager", GUILayout.Height(25)))
        {
            if (this.AudioManager) Debug.LogError("<color=red>Error:</color> <color=yellow>Already Have A AudioManager!</color>");
            else
            {
                GameObject AudioManager = new GameObject("AudioManager");
                this.AudioManager = AudioManager.AddComponent<AudioManager>();
            }
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("DailyReward", GUILayout.Height(25)))
        {
            if (this.dailyRewards) Debug.LogError("<color=red>Error:</color> <color=yellow>Already Have A Daily Reward Manager!</color>");
            else
            {
                GameObject DailyRewards = new GameObject("DailyRewards");
                this.dailyRewards = DailyRewards.AddComponent<DailyRewards>();
            }
            Canvas canvas = CheckForCanvas();
            if (this.dailyRewardScreen) Debug.LogError("<color=red>Error:</color> <color=yellow>Already Have A Daily Reward !</color>");
            else
            {
                GameObject Screen = Resources.Load<GameObject>("Reward/DailyRewardPanel");
                GameObject s_timedReward = Instantiate(Screen, canvas.transform);
                s_timedReward.name = "DailyReward";
                this.dailyRewardScreen = s_timedReward.GetComponent<DailyRewardsUI>();
            }
        }
        if (GUILayout.Button("TimedReward", GUILayout.Height(25)))
        {
            if (this.timedReward) Debug.LogError("<color=red>Error:</color> <color=yellow>Already Have A Time Reward Manager!</color>");
            else
            {
                GameObject timedReward = new GameObject("TimedReward");
                this.timedReward = timedReward.AddComponent<TimedRewards>();
            }
            Canvas canvas = CheckForCanvas();
            if (this.timedRewardScreen) Debug.LogError("<color=red>Error:</color> <color=yellow>Already Have A Time Reward !</color>");
            else
            {
                GameObject Screen = Resources.Load<GameObject>("Reward/TimedRewardPanel");
                GameObject s_timedReward = Instantiate(Screen, canvas.transform);
                s_timedReward.name = "TimedReward";
                this.timedRewardScreen = s_timedReward.GetComponent<TimedRewardUI>();
            }
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        //if (GUILayout.Button("Unity AdManager", GUILayout.Height(25)))
        //{
        //    if (this.adManager) Debug.LogError("<color=red>Error:</color> <color=yellow>Already Have A UNITY AD Manager!</color>");
        //    else
        //    {
        //        GameObject AdManager = new GameObject("UNITY_AdManager");
        //        this.adManager = AdManager.AddComponent<ADManager>();
        //    }
        //}
        if (GUILayout.Button("Social Share", GUILayout.Height(25)))
        {
            if (this.socialShare) Debug.LogError("<color=red>Error:</color> <color=yellow>Already Have A SocialShare Manager!</color>");
            else
            {
                GameObject socialShare = new GameObject("SocialShare");
                this.socialShare = socialShare.AddComponent<SocialShare>();
            }
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Missions Screen", GUILayout.Height(25)))
        {
            Canvas canvas = CheckForCanvas();
            if (this.missionManager) Debug.LogError("<color=red>Error:</color> <color=yellow>Already Have A Mission Manager!</color>");
            else
            {
                GameObject Screen = Resources.Load<GameObject>("Quest/MissionManager");
                GameObject missionManager = Instantiate(Screen, canvas.transform);
                missionManager.name = "MissionManager";
                this.missionManager = missionManager.GetComponent<MissionManager>();
            }
        }
        if (GUILayout.Button("Achievement Screen", GUILayout.Height(25)))
        {
            Canvas canvas = CheckForCanvas();
            if (this.achievementManager) Debug.LogError("<color=red>Error:</color> <color=yellow>Already Have A Achievement Manager!</color>");
            else
            {
                GameObject Screen = Resources.Load<GameObject>("Quest/AchievementManager");
                GameObject AchievementManager = Instantiate(Screen, canvas.transform);
                AchievementManager.name = "AchievementManager";
                this.achievementManager = AchievementManager.GetComponent<AchievementManager>();
            }
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Popup", GUILayout.Height(25)))
        {
            Canvas canvas = CheckForCanvas();
            if (this.popUpScreen) Debug.LogError("<color=red>Error:</color> <color=yellow>Already Have A Pop Up!</color>");
            else
            {
                GameObject Screen = Resources.Load<GameObject>("PopUp");
                GameObject popUpManager = Instantiate(Screen, canvas.transform);
                popUpManager.name = "PopUP";
                this.popUpScreen = popUpManager.GetComponent<PopUp>();
            }
        }
        if (GUILayout.Button("Analytics Manager", GUILayout.Height(25)))
        {
            if (this.AnalyticsManager) Debug.LogError("<color=red>Error:</color> <color=yellow>Already Have A Analytics Manager!</color>");
            else
            {
                GameObject AnalyticsManager = new GameObject("AnalyticsManager");
                this.AnalyticsManager = AnalyticsManager.AddComponent<AnalyticsManager>();
            }
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("LeaderBoard", GUILayout.Height(25)))
        {
            Canvas canvas = CheckForCanvas();
            if (this.leaderboardManager) Debug.LogError("<color=red>Error:</color> <color=yellow>Already Have A leader Board!</color>");
            else
            {
                GameObject Screen = Resources.Load<GameObject>("LeaderboardScreen");
                GameObject leaderBoard = Instantiate(Screen, canvas.transform);
                leaderBoard.name = "LeaderBoardManager";
                this.leaderboardManager = leaderBoard.GetComponent<LeaderBoardManager>();
            }
        }
        if (GUILayout.Button("PlayFab Manager", GUILayout.Height(25)))
        {
            if (this.PlayFabManager) Debug.LogError("<color=red>Error:</color> <color=yellow>Already Have A PlayFab Manager!</color>");
            else
            {
                GameObject PlayFabManager = new GameObject("PlayFabManager");
                this.PlayFabManager = PlayFabManager.AddComponent<PlayfabManager>();
            }
        }
        if (GUILayout.Button("LogIn Page", GUILayout.Height(25)))
        {
            Canvas canvas = CheckForCanvas();
            if (this.LoginPage) Debug.LogError("<color=red>Error:</color> <color=yellow>Already Have A LoginPage!</color>");
            else
            {
                GameObject Screen = Resources.Load<GameObject>("LoginPage");
                GameObject LogInPage = Instantiate(Screen, canvas.transform);
                LogInPage.name = "LogInPage";
                this.LoginPage = LogInPage.GetComponent<LogInPage>();
            }
        }
        EditorGUILayout.EndHorizontal();

        GUILayout.EndArea();

    }
    Canvas CheckForCanvas()
    {
        var canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("<color=red>Error:</color> <color=yellow>Create a UI Canvas First!</color>");
            return null;
        }
        else
            return canvas;
    }
}

