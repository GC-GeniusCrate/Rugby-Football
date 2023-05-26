using GeniusCrate.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GeniusCrate
{[RequireComponent(typeof(Button))]
	public class ButtonClick : MonoBehaviour
	{

        Button button;

        #region Public Fields
        #endregion

        #region Unity Methods
        private void Start()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(() =>
            {
                AudioManager.Instance.PlaySoundOfType(SoundEffectType.ButtonClick);
            });
        }
        #endregion

        #region Private Methods
        #endregion

    }
}
