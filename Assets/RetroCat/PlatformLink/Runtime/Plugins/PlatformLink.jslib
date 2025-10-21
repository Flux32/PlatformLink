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

    jslib_getDeviceType: function() {
        return _jslib_convertString(getDeviceInfo());
    },

    jslib_sendGameReadyMessage: function() {
        sendGameReadyMessage();
    },
    
    jslib_purchase: function(id) {
        purchase(UTF8ToString(id));
    },
    
    jslib_setLeaderboardScore: function(leaderboardId, score) {
        setLeaderboardScore(leaderboardId, score);
    }
});
