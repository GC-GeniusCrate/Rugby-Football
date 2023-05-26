using GeniusCrate.Utility;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Button))]
[DisallowMultipleComponent]
public class ShareButton : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => { SocialShare.Instance.Share("", "", ""); });
    }
}
