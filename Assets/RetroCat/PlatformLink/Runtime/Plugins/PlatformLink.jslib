mergeInto(LibraryManager.library, {
    jslib_convertString: function (data) {
        let bufferSize = lengthBytesUTF8(data) + 1;
        let buffer = _malloc(bufferSize);
        stringToUTF8(data, buffer, bufferSize);
        return buffer;
    },

    jslib_initializePlugin: function() {
        initializePlugin();
    },

    jslib_loadRemoteConfig: function() {
        loadRemoteConfig();
    },

    jslib_showInterstitialAd: function() {
        showInterstitialAd();
    },
    
    jslib_showRewardedAd: function() {
        showRewardedAd();
    },

    jslib_saveToPlatform: function(key, stringData) {
        saveToPlatform(UTF8ToString(key), UTF8ToString(stringData));
    },

    jslib_loadFromPlatform: function(key) {
        loadFromPlatform(UTF8ToString(key));
    },

    jslib_saveToLocalStorage: function(key, data) {
        saveToLocalStorage(UTF8ToString(key), UTF8ToString(data));
    },

    jslib_loadFromLocalStorage: function(key) {
        return _jslib_convertString(loadFromLocalStorage(UTF8ToString(key)));
    },

    jslib_getLanguage: function() {
        return _jslib_convertString(getLanguage());
    },

    jslib_getAppId: function() {
        return _jslib_convertString(getAppId());
    },

    jslib_getDeviceType: function() {
        return _jslib_convertString(getDeviceInfo());
    },

    jslib_sendGameReadyMessage: function() {
        sendGameReadyMessage();
    },

    jslib_isPlayerAuthorized: function() {
        return isPlayerAuthorized() ? 1 : 0;
    },

    jslib_openAuthDialog: function() {
        openAuthDialog();
    },

    jslib_openLink: function(url) {
        openLink(UTF8ToString(url));
    },

    jslib_getAllGames: function() {
        getAllGames();
    },
    
    jslib_purchase: function(id) {
        purchase(UTF8ToString(id));
    },

    jslib_consumePurchase: function(token) {
        consumePurchase(UTF8ToString(token));
    },

    jslib_getPurchases: function() {
        getPurchases();
    },
    
    jslib_getCatalog: function() {
        getCatalog();
    },
    
    jslib_getProduct: function(id) {
        getProduct(UTF8ToString(id));
    },
    
    jslib_setLeaderboardScore: function(leaderboardId, score) {
        setLeaderboardScore(UTF8ToString(leaderboardId), score);
    },

    jslib_getLeaderboardPlayerEntry: function(leaderboardId) {
        getLeaderboardPlayerEntry(UTF8ToString(leaderboardId));
    },

    jslib_getLeaderboardEntries: function(leaderboardId, includeUser, quantityAround, quantityTop) {
        getLeaderboardEntries(UTF8ToString(leaderboardId), includeUser, quantityAround, quantityTop);
    },

    jslib_isNativeShareAvailable: function() {
        return isNativeShareAvailable() ? 1 : 0;
    },

    jslib_showNativeShare: function(payload) {
        showNativeShare(UTF8ToString(payload));
    },

    jslib_isVibrationSupported: function() {
        return isVibrationSupported() ? 1 : 0;
    },

    jslib_vibrate: function(durationMs) {
        vibrate(durationMs);
    },

    jslib_vibratePattern: function(patternCsv) {
        vibratePattern(UTF8ToString(patternCsv));
    },

    jslib_copyToClipboard: function(text) {
        copyToClipboard(UTF8ToString(text));
    }
});
