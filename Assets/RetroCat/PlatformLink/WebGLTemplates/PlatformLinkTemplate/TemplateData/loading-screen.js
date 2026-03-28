(function () {
  function getMarkup() {
    return [
      '<div id="loading-page" class="loading-screen" data-node-id="3408:21099">',
      '  <div class="loading-screen__content">',
      '    <img class="loading-screen__cat" src="TemplateData/loading-cat.png" alt="RETRO CAT">',
      '    <p class="loading-screen__text">Загрузка...</p>',
      '    <div class="loading-screen__progress">',
      '      <div class="loading-screen__progress-track">',
      '        <div id="progress-bar-fill" class="loading-screen__progress-fill"></div>',
      '      </div>',
      '    </div>',
      '    <p class="loading-screen__powered">Powered by "RETRO CAT"</p>',
      '  </div>',
      '</div>'
    ].join("");
  }

  function create(overlayElement) {
    if (!overlayElement) {
      return null;
    }

    overlayElement.innerHTML = getMarkup();

    const progressBarFill = overlayElement.querySelector("#progress-bar-fill");

    return {
      progressBarFill,
      setProgress(progress) {
        if (!progressBarFill) {
          return;
        }

        const normalizedProgress = Math.max(0, Math.min(1, Number(progress) || 0));
        progressBarFill.style.width = `${normalizedProgress * 100}%`;
      },
      hide() {
        overlayElement.style.display = "none";
      }
    };
  }

  window.RetroLoadingScreen = {
    create
  };
})();
