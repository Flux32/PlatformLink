using System.Collections;
using PlatformLink;
using UnityEngine;
using UnityEngine.Events;

namespace RetroCat.PlatformLink.Runtime.Source.Common.Modules.Purchases
{
    public class ProductPriceLoader : MonoBehaviour
    {
        [SerializeField] private string _productId;
        [SerializeField] private bool _loadOnEnable;
    
        [SerializeField] private UnityEvent OnLoadStarted;
        [SerializeField] private UnityEvent<Texture2D> OnIconLoadFinished;
        [SerializeField] private UnityEvent OnIconLoadError;
        [SerializeField] private UnityEvent<string> OnPriceLoadFinished;

        private Coroutine _coroutine;
    
        private void OnEnable()
        {
            if (_loadOnEnable)
            {
                if (_coroutine != null)
                    return;
            
                StartCoroutine(LoadProductPrice());
            }
        }

        private void OnDisable()
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
            }
        }

        private IEnumerator LoadProductPrice()
        {
            OnLoadStarted?.Invoke();

#if UNITY_EDITOR
            yield return new WaitForSeconds(1f);
#endif
            
            bool isLoaded = false;
            CatalogProduct loadedProduct = null;
        
            PLink.Purchases.GetProduct(_productId, (success, product) =>
            {
                isLoaded = success;
                loadedProduct = product;
            });
        
            yield return new WaitUntil(() => isLoaded && loadedProduct != null);
        
            isLoaded = false;
            Texture2D loadedCurrencyIcon = null;
        
            loadedProduct.PriceCurrencyIcon.LoadTexture((success, currencyIcon ) =>
            {
                isLoaded = true;
                loadedCurrencyIcon = currencyIcon;
            });
        
            yield return new WaitUntil(() => isLoaded);

            if (loadedCurrencyIcon == null)
            {
                OnIconLoadError?.Invoke();
            }
            else
            {
                OnIconLoadFinished?.Invoke(loadedCurrencyIcon);
            }
            
            OnPriceLoadFinished?.Invoke(loadedProduct.PriceValue);
        }
    }
}
