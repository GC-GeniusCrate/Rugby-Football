using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace GeniusCrate.Utility
{
    public class ShopManager : Screen
    {
        [SerializeField] TMP_Text InGameCurrencyText;
        [SerializeField] TMP_Text IAPCurrencyText;

        [Header("Store details as per playfab")]
        [SerializeField] string catalogVersion;
        [SerializeField] GameObject storeItemPrefab;
        [SerializeField] Transform contentParent;
        public void OnEnable()
        {
            MenuManager.OnShopButtonTrigger += InitScreen;
        }
        public void OnDisable()
        {
            MenuManager.OnShopButtonTrigger -= InitScreen;
        }

        public override void InitScreen()
        {
            base.InitScreen();
            /* PlayfabManager.Instance.GetStoreItems(storeId, result =>
             {
                 foreach (var item in result.Store)
                 {
                    Debug.Log( item.ItemId);
                    Debug.Log( item.VirtualCurrencyPrices["RM"]);
                 }
             });*/
            int i = 0;
            foreach (Transform child in contentParent)
            {
                if (i >= 1)
                {
                    Destroy(child.gameObject);
                }
                i++;
            }
            PlayfabManager.Instance.GetCatalog(catalogVersion, result =>
            {
                foreach (var item in result.Catalog)
                {
                    GameObject shopItem = Instantiate(storeItemPrefab, contentParent);
                    PlayfabManager.Instance.DownloadAndShowImage(item.ItemImageUrl, shopItem.transform.GetChild(0).GetComponent<RawImage>());
                    shopItem.transform.GetChild(1).GetComponent<TMP_Text>().text = $"{item.DisplayName}";
                    shopItem.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() =>
                    {
                        PlayfabManager.Instance.PurchaceItem(catalogVersion, item.ItemId, (int)item.VirtualCurrencyPrices["GC"], "GC", result => PlayfabManager.Instance.GetVirtualCurrency());
                        PlayfabManager.Instance.GetInventory(result =>
                        {
                            foreach (var item in result.Inventory)
                            {
                                Debug.Log(item.DisplayName);
                                Debug.Log(item.RemainingUses);
                            }

                        });
                    });
                    shopItem.transform.GetChild(2).GetChild(0).GetComponent<TMP_Text>().text = $"{item.VirtualCurrencyPrices["GC"]}";
                }
            });
        }

    }
}

