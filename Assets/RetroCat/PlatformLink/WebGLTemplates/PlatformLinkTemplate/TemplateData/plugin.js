YaGames
.init()
.then(ysdk => {
  console.log('Yandex SDK initialized');
  window.ysdk = ysdk;
  initializePlayer();

  sendGameReadyMessage(); //DOTO: Here?
});

let player;

function initializePlayer() {
  return ysdk.getPlayer({ scopes: false }).then(_player => {
    player = _player;
    console.log("Yandex SDK player initialized: [player mode = " + player.getMode() + "]");
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