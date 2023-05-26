using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;

namespace GeniusCrate.Utility
{
    public class CharacterSelection : Screen
    {

        [SerializeField] Button leftButton;
        [SerializeField] Button rightButton;

        [SerializeField] Button SelectButton;

        [SerializeField] Transform characterparentTransform;

       public List<GameObject> characters = new List<GameObject>();
       public List<string> characterKeys = new List<string>();

        int currntCharacterIndex;
        int selectedIndex;

        private void OnEnable()
        {
            MenuManager.OnCharacterSelectionTrigger += InitScreen;
        }
        private void OnDisable()
        {
            MenuManager.OnCharacterSelectionTrigger -= InitScreen;
        }
        private void Start()
        {
            Addressables.LoadResourceLocationsAsync("Characters").Completed += (a) =>
              {
                  int i = 0;
                  foreach (var item in a.Result)
                  {
                      
                      Addressables.InstantiateAsync(item.PrimaryKey, characterparentTransform).Completed += (character) =>
                        {
                            characters.Add(character.Result.gameObject);
                            characterKeys.Add(item.PrimaryKey);

                            character.Result.gameObject.GetComponent<CharacterController>().enabled = false;
                            if (item.PrimaryKey != GameManager.Instance.mSelectedCharacterKey)
                            {
                                character.Result.gameObject.SetActive(false);

                            }
                            else
                            {
                                currntCharacterIndex = i;
                            }

                            i++;
                            CheckButtons();

                        };
                  }

              };

            leftButton.onClick.AddListener(() => 
            {
                characters[currntCharacterIndex].SetActive(false);
                currntCharacterIndex--;
                characters[currntCharacterIndex].SetActive(true);
                CheckButtons();

            });

            rightButton.onClick.AddListener(() =>
            {
                characters[currntCharacterIndex].SetActive(false);
                currntCharacterIndex++;
                characters[currntCharacterIndex].SetActive(true);
                CheckButtons();

            });
            SelectButton.onClick.AddListener(() => 
            {
                SelectButton.interactable = false;
                SelectButton.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = "Selected";
                GameManager.Instance.mSelectedCharacterKey = characterKeys[currntCharacterIndex];
                selectedIndex = currntCharacterIndex;

            });
        }

        void CheckButtons()
        {
            if (currntCharacterIndex == 0)
            {
                leftButton.gameObject.SetActive(false);
                rightButton.gameObject.SetActive(true);
            }
            else if (currntCharacterIndex == characters.Count - 1)
            {
                leftButton.gameObject.SetActive(true);
                rightButton.gameObject.SetActive(false);
            }
            else
            {
                leftButton.gameObject.SetActive(true);
                rightButton.gameObject.SetActive(true);
            }
            if (characterKeys.Count <= 0) return;
            if (characterKeys[currntCharacterIndex] == GameManager.Instance.mSelectedCharacterKey)
            {
                SelectButton.interactable = false;
                SelectButton.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = "Selected";
                selectedIndex = currntCharacterIndex;

            }
            else
            {
                SelectButton.interactable = true;
                SelectButton.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = "Select";
            }
        }

        public override void CloseScreen()
        {
            base.CloseScreen();
            characters[currntCharacterIndex].SetActive(false);

            characters[selectedIndex].SetActive(true);
            currntCharacterIndex = selectedIndex;
            CheckButtons();
        }

    }
}