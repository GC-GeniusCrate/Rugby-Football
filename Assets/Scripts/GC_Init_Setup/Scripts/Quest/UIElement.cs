
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace GeniusCrate.Utility
{
    public class UIElement : MonoBehaviour
    {
        [SerializeField] protected Image icon;
        [SerializeField] protected TMP_Text title;
        [SerializeField] protected TMP_Text description;
        [SerializeField] protected Image progressHandle;
        [SerializeField] protected TMP_Text progressText;
        [SerializeField] protected Button collectBtn;
        protected int elementId;
    }

}
