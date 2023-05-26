using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialUI : MonoBehaviour
{
   [SerializeField] TMP_Text tutorialText;
   [SerializeField] GameObject tutorialScreen;

    private void OnEnable()
    {
        CharacterController.ShowTutorialUI += ShowTutorialUI;
    }

    private void ShowTutorialUI(bool activate,string text)
    {
        tutorialText.text = text;
        tutorialScreen.SetActive(activate);
    }

    private void OnDisable()
    {
        CharacterController.ShowTutorialUI -= ShowTutorialUI;

    }
}
