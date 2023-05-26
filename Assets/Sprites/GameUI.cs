using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using GeniusCrate.Utility;
using System;

public class GameUI : MonoBehaviour
{
    [SerializeField] TMP_Text scoreTxt;
    [SerializeField] TMP_Text coinTxt;
    [SerializeField] TMP_Text highScoreTxt;
    [SerializeField] TMP_Text distanceTxt;

    [SerializeField] PowerUpUIElement powerUpElement;
    [SerializeField] Transform powerUpUIContent;

    [SerializeField] GameObject resumeScreen;
    [SerializeField] GameObject TimerScreen;
    int coin = 0;

    private void OnEnable()
    {
        Coin.OnCollectingCoin += UpdateCoin;
        Consumable.OnConsumablePicked += OnConsumablePicked;
        coin = 0;
        GameManager.BackToMainMenu += ResetValues;
    }

    private void ResetValues()
    {
        scoreTxt.text = 0.ToString();
        coinTxt.text = 0.ToString();
        highScoreTxt.text = (GameManager.Instance.HighScore).ToString();
        distanceTxt.text = "0 M";
        GameManager.Instance.AddInGameCurrency(coin);
        coin = 0;

    }

    private void OnDisable()
    {
        Coin.OnCollectingCoin -= UpdateCoin;
        Consumable.OnConsumablePicked -= OnConsumablePicked;
        GameManager.BackToMainMenu += ResetValues;


    }
    private void Update()
    {
        scoreTxt.text = GameManager.Instance._Score.ToString("D10");
        distanceTxt.text = GameManager.Instance.distance.ToString("F2")+" M";
        highScoreTxt.text = (GameManager.Instance.HighScore- GameManager.Instance._Score).ToString();

        if (!GameManager.Instance.isGameStarted) return;
      
    }

    public void OnPauseButtonPress()
    {
        //GameManager.Instance.isGameStarted = false;
        GameManager.Instance.PauseGame = true;
        resumeScreen.SetActive(true);
        Time.timeScale = 0;
    }
    public void ResumeButtonPress()
    {
        //GameManager.Instance.isGameStarted = true;
        GameManager.Instance.PauseGame = false;
        GameManager.Instance.SetTimer();
        resumeScreen.SetActive(false);
        Time.timeScale = 1;


    }

    public void QuitButtonPress()
    {
        Application.Quit();
    }
    void OnConsumablePicked(Sprite icon, float duration,ConsumableType type)
    {
        Instantiate(powerUpElement, powerUpUIContent).Init(icon, duration,type);
    }
    void UpdateCoin()
    {
        coin++;
        coinTxt.text = coin.ToString();
    }
}
