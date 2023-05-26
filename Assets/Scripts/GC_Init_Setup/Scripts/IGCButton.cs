using UnityEngine.UI;
using UnityEngine;

namespace GeniusCrate.Utility
{
    [RequireComponent(typeof(Button))]
    [DisallowMultipleComponent]
    public class IGCButton : MonoBehaviour
    {
        [SerializeField] int cost;
        [SerializeField] string currencyKey = "GC";

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(BuyItem);
        }
        void BuyItem()
        {
            PlayfabManager.Instance.SubtractVirtualCurrency(currencyKey, cost);
        }
    }
}