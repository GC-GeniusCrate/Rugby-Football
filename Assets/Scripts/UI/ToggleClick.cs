using GeniusCrate.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GeniusCrate
{
    [RequireComponent(typeof(Toggle))]
    public class ToggleClick : MonoBehaviour
    {
        Toggle toggle;
        #region Public Fields
        #endregion

        #region Unity Methods
        private void Start()
        {
            toggle = GetComponent<Toggle>();
            toggle.onValueChanged.AddListener((value) => { AudioManager.Instance.PlaySoundOfType(SoundEffectType.Toggle); });
        }
        #endregion

        #region Private Methods
        #endregion

    }
}
