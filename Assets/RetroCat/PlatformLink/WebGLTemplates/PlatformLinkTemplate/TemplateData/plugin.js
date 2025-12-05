let player;

function initializePlugin()
{
  YaGames
    .init()
    .then(ysdk => {
    console.log('Yandex SDK initialized');
    window.ysdk = ysdk;
    initializePlayer();
});
}

function initializePlayer() {
  return ysdk.getPlayer({ scopes: false }).then(_player => {
    player = _player;
    const auth = (typeof player.isAuthorized === 'function') ? player.isAuthorized() : false;
    console.log("Yandex SDK player initialized: [authorized = " + auth + "]");
    console.log("PlatformLink initialized");
    sendMessageToUnity('fjs_platformLinkInitialized');
  });
}

function sendMessageToUnity(message, value = undefined) {
  myGameInstance.SendMessage('#!_platform_link_#!', message, value);
}

function getLanguage() {
  return ysdk.environment.i18n.lang;
}

function sendGameReadyMessage() {
  ysdk.features.LoadingAPI?.ready();
}

function showInterstitialAd() {
  ysdk.adv.showFullscreenAdv({
      callbacks: {
          onOpen: () => {
              console.log('Interstetial opened.');
              sendMessageToUnity('fjs_onInterstetialAdOpened');
          },
          onClose: (wasShown) => {
              console.log('Interstetial closed.');
              sendMessageToUnity('fjs_onInterstetialAdClosed');
          }, 
          onError: (error) => {
              console.log('Error while open video ad:', error);
              sendMessageToUnity('fjs_onInterstetialAdError'); // TODO: Add error message
          },
          onoffline: () => {
              console.log('The interstitial is not open because the user is Offline.');
              sendMessageToUnity('fjs_onInterstetialAdError');
          }
      }
  })
}

function showRewardedAd() {
  ysdk.adv.showRewardedVideo({
      callbacks: {
          onOpen: () => {
              console.log('Rewarded ad opened.');
              sendMessageToUnity('fjs_onRewardedAdOpened');
          },
          onRewarded: () => {
              console.log('Rewarded!');
              sendMessageToUnity('fjs_onRewarded');
          },
          onClose: () => {
              console.log('Rewarded ad closed.');
              sendMessageToUnity('fjs_onRewardedAdClosed');
          }, 
          onError: (error) => {
              console.log('Error while open video ad:', error);
              sendMessageToUnity('fjs_onRewardedAdError');
          }
      }
  })
}

function purchase(id) {
  ysdk.payments.purchase({ id: id }).then(purchase => {
    console.log('Purchase success');
    sendMessageToUnity('fjs_onPurchaseSuccess');
  }).catch(error => {
    console.log('Purchase error:', error);
    sendMessageToUnity('fjs_onPurchaseError');
  });
}

function getCatalog() {
  if (!ysdk || !ysdk.payments || !ysdk.payments.getCatalog) {
    console.warn('Yandex SDK payments.getCatalog is not available');
    sendMessageToUnity('fjs_onGetCatalogFailed');
    return;
  }

  ysdk.payments.getCatalog()
    .then(products => {
      try {
        const mapped = (products || []).map(p => ({
          id: p.id || '',
          title: p.title || '',
          description: p.description || '',
          iconUrl: p.imageURI || '',
          currencyIconUrl: (typeof p.getPriceCurrencyImage === 'function') ? p.getPriceCurrencyImage('small') : '',
          price: p.price || '',
          priceValue: p.priceValue || '',
          priceCurrencyCode: p.priceCurrencyCode || ''
        }));
        const payload = JSON.stringify({ items: mapped });
        sendMessageToUnity('fjs_onGetCatalogSuccess', payload);
      } catch (e) {
        console.error('Error serializing catalog:', e);
        sendMessageToUnity('fjs_onGetCatalogFailed');
      }
    })
    .catch(error => {
      console.log('getCatalog error:', error);
      sendMessageToUnity('fjs_onGetCatalogFailed');
    });
}

function getProduct(id) {
  if (!ysdk || !ysdk.payments || !ysdk.payments.getCatalog) {
    console.warn('Yandex SDK payments.getCatalog is not available');
    sendMessageToUnity('fjs_onGetProductFailed');
    return;
  }

  ysdk.payments.getCatalog()
    .then(products => {
      const product = (products || []).find(p => p.id === id);
      if (!product) {
        sendMessageToUnity('fjs_onGetProductFailed');
        return;
      }

      try {
        const mapped = {
          id: product.id || '',
          title: product.title || '',
          description: product.description || '',
          iconUrl: product.imageURI || '',
          currencyIconUrl: (typeof product.getPriceCurrencyImage === 'function') ? product.getPriceCurrencyImage('small') : '',
          price: product.price || '',
          priceValue: product.priceValue || '',
          priceCurrencyCode: product.priceCurrencyCode || ''
        };
        const payload = JSON.stringify(mapped);
        sendMessageToUnity('fjs_onGetProductSuccess', payload);
      } catch (e) {
        console.error('Error serializing product:', e);
        sendMessageToUnity('fjs_onGetProductFailed');
      }
    })
    .catch(error => {
      console.log('getProduct error:', error);
      sendMessageToUnity('fjs_onGetProductFailed');
    });
}

function getPurchases() {
  if (!ysdk || !ysdk.payments || !ysdk.payments.getPurchases) {
    console.warn('Yandex SDK payments.getPurchases is not available');
    sendMessageToUnity('fjs_onGetPurchasesFailed');
    return;
  }

  ysdk.payments.getPurchases()
    .then(purchases => {
      try {
        const mapped = (purchases || []).map(p => ({
          productID: p.productID || '',
          purchaseToken: p.purchaseToken || '',
          developerPayload: p.developerPayload || ''
        }));
        const payload = JSON.stringify({ items: mapped });
        sendMessageToUnity('fjs_onGetPurchasesSuccess', payload);
      } catch (e) {
        console.error('Error serializing purchases:', e);
        sendMessageToUnity('fjs_onGetPurchasesFailed');
      }
    })
    .catch(error => {
      console.log('getPurchases error:', error);
      sendMessageToUnity('fjs_onGetPurchasesFailed');
    });
}

function consumePurchase(token) {
  if (!ysdk || !ysdk.payments || !ysdk.payments.consumePurchase) {
    console.warn('Yandex SDK payments.consumePurchase is not available');
    sendMessageToUnity('fjs_onConsumePurchaseFailed', token || '');
    return;
  }

  ysdk.payments.consumePurchase(token)
    .then(() => {
      sendMessageToUnity('fjs_onConsumePurchaseSuccess', token || '');
    })
    .catch(error => {
      console.log('consumePurchase error:', error);
      sendMessageToUnity('fjs_onConsumePurchaseFailed', token || '');
    });
}

function saveToPlatform(key, data)
{
  let object = {
    [key]: data
  }

  player.setData(object).then(() => {
    console.log('Data saved: ');
    console.log(object);
    sendMessageToUnity('fjs_onSaveDataSuccess');
  });
}

function loadFromPlatform(key) {
    console.log('key: ' + key); //TODO: не тот ключ

    player.getData([key]).then(data => {
        console.log('object: ');
        console.log(data);
        console.log('value: ');
        console.log(data[key]);

        player.getData([key]).then(data => {
            if (data[key]) {
                sendMessageToUnity('fjs_onLoadDataSuccess', data[key]);
                console.log('loaded');
            }
            else {
                console.log('loaded null');
                sendMessageToUnity('fjs_onLoadDataSuccess', "");
            }
        });
    });
}

function saveToLocalStorage(key, data) {
  try {
    localStorage.setItem(key, data);
  }
  catch (error) {
    console.error('Save to local storage error: ', error.message);
  }
}

function loadFromLocalStorage(key)
{
  return localStorage.getItem(key);
}

function getDeviceInfo()
{
  return ysdk.deviceInfo.type;
}

function setLeaderboardScore(leaderboardId, score)
{
  ysdk.leaderboards.setScore(leaderboardId, score);
}

function getLeaderboardPlayerEntry(leaderboardId) {
  if (!ysdk || !ysdk.leaderboards || !ysdk.leaderboards.getPlayerEntry) {
    console.warn('Yandex SDK leaderboards.getPlayerEntry is not available');
    sendMessageToUnity('fjs_onGetPlayerEntryFailed');
    return;
  }

  ysdk.leaderboards.getPlayerEntry(leaderboardId)
    .then(res => {
      try {
        const player = res.player || {};
        const scope = player.scopePermissions || {};
        const payload = {
          score: res.score || 0,
          extraData: res.extraData || '',
          rank: res.rank || 0,
          formattedScore: res.formattedScore || '',
          player: {
            lang: player.lang || '',
            publicName: player.publicName || '',
            uniqueID: player.uniqueID || '',
            avatarUrl: (typeof player.getAvatarSrc === 'function') ? player.getAvatarSrc('medium') : '',
            scopePermissions: {
              avatar: scope.avatar || '',
              public_name: scope.public_name || ''
            }
          }
        };

        const json = JSON.stringify(payload);
        sendMessageToUnity('fjs_onGetPlayerEntrySuccess', json);
      } catch (e) {
        console.error('Error serializing player entry:', e);
        sendMessageToUnity('fjs_onGetPlayerEntryFailed');
      }
    })
    .catch(err => {
      console.log('getPlayerEntry error:', err);
      if (err && err.code === 'LEADERBOARD_PLAYER_NOT_PRESENT') {
        sendMessageToUnity('fjs_onGetPlayerEntryNotPresent');
      } else {
        sendMessageToUnity('fjs_onGetPlayerEntryFailed');
      }
    });
}

function getLeaderboardEntries(leaderboardId, includeUser, quantityAround, quantityTop) {
  if (!ysdk || !ysdk.leaderboards || !ysdk.leaderboards.getEntries) {
    console.warn('Yandex SDK leaderboards.getEntries is not available');
    sendMessageToUnity('fjs_onGetLeaderboardEntriesFailed');
    return;
  }

  const options = {};
  if (includeUser) {
    options.includeUser = true;
  }
  if (quantityAround > 0) {
    options.quantityAround = quantityAround;
  }
  if (quantityTop > 0) {
    options.quantityTop = quantityTop;
  }

  ysdk.leaderboards.getEntries(leaderboardId, options)
    .then(res => {
      try {
        const entries = (res.entries || []).map(e => {
          const player = e.player || {};
          const scope = player.scopePermissions || {};
          return {
            score: e.score || 0,
            extraData: e.extraData || '',
            rank: e.rank || 0,
            formattedScore: e.formattedScore || '',
            player: {
              lang: player.lang || '',
              publicName: player.publicName || '',
              uniqueID: player.uniqueID || '',
              avatarUrl: (typeof player.getAvatarSrc === 'function') ? player.getAvatarSrc('medium') : '',
              scopePermissions: {
                avatar: scope.avatar || '',
                public_name: scope.public_name || ''
              }
            }
          };
        });

        const ranges = (res.ranges || []).map(r => ({
          start: r.start || 0,
          size: r.size || 0
        }));

        const payload = {
          leaderboardId: (res.leaderboard && (res.leaderboard.id || res.leaderboard.name)) || '',
          userRank: res.userRank || 0,
          ranges: ranges,
          entries: entries
        };

        const json = JSON.stringify(payload);
        sendMessageToUnity('fjs_onGetLeaderboardEntriesSuccess', json);
      } catch (e) {
        console.error('Error serializing leaderboard entries:', e);
        sendMessageToUnity('fjs_onGetLeaderboardEntriesFailed');
      }
    })
    .catch(err => {
      console.log('getEntries error:', err);
      sendMessageToUnity('fjs_onGetLeaderboardEntriesFailed');
    });
}
