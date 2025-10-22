using PlatformLink;
using UnityEngine;
using UnityEngine.UI;

namespace RetroCat.PlatformLink.Runtime.Source.Common.Modules.Purchases
{
    [RequireComponent(typeof(PurchaseInvoker))]
    public class PurchaseButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private string _id;
        
        private PurchaseInvoker _purchaseInvoker;

        private void Awake()
        {
            _purchaseInvoker = gameObject.GetComponent<PurchaseInvoker>();
        }

        private void OnEnable()
        {
            PLink.Purchases.PurchaseStarted += OnPurchaseStarted;
            PLink.Purchases.Purchased += OnPurchased;
            PLink.Purchases.PurchaseFailed += OnPurchaseFailed;

            _button.onClick.AddListener(OnPurchaseClicked);
        }

        private void OnDisable()
        {
            PLink.Purchases.PurchaseStarted -= OnPurchaseStarted;
            PLink.Purchases.Purchased -= OnPurchased;
            PLink.Purchases.PurchaseFailed -= OnPurchaseFailed;

            _button.onClick.RemoveListener(OnPurchaseClicked);
        }

        private void OnPurchaseClicked()
        {
            _purchaseInvoker.InitiatePurchase(_id);
        }

        private void OnPurchased(Purchase purchase)
        {
            _button.interactable = true;
        }

        private void OnPurchaseFailed(string error)
        {
            _button.interactable = true;
        }

        private void OnPurchaseStarted()
        {
            _button.interactable = false;
        }
    }
}