(function () {
  const MODE = {
    PC: "pc",
    MOBILE: "mobile"
  };

  const DESIGN = {
    [MODE.PC]: { width: 1440, height: 1024 },
    [MODE.MOBILE]: { width: 375, height: 812 }
  };

  const MOBILE_MAX_WIDTH = 812;
  const MOBILE_PORTRAIT_ASPECT = 1;
  const MOBILE_FORCE_WIDTH = 480;

  let currentOverlayElement = null;
  let currentRootElement = null;
  let resizeBound = false;

  function getMarkup() {
    return [
      '<div id="loading-page" class="loading-screen loading-screen--pc" data-node-id="3408:21099">',
      '  <div class="loading-screen__content">',
      '    <img class="loading-screen__cat" src="TemplateData/loading-cat.png" alt="RETRO CAT">',
      '    <p class="loading-screen__text">Загрузка...</p>',
      '    <div class="loading-screen__progress">',
      '      <div class="loading-screen__progress-track">',
      '        <div id="progress-bar-light" class="loading-screen__progress-light"></div>',
      '        <div id="progress-bar-fill" class="loading-screen__progress-fill"></div>',
      '      </div>',
      '    </div>',
      '    <p class="loading-screen__powered">Powered by "RETRO CAT"</p>',
      '  </div>',
      '</div>'
    ].join("");
  }

  function getMode(width, height) {
    const aspect = height > 0 ? width / height : 1;
    return width <= MOBILE_FORCE_WIDTH || (width <= MOBILE_MAX_WIDTH && aspect <= MOBILE_PORTRAIT_ASPECT)
      ? MODE.MOBILE
      : MODE.PC;
  }

  function applyLayout() {
    if (!currentOverlayElement || !currentRootElement) {
      return;
    }

    const rect = currentOverlayElement.getBoundingClientRect();
    const mode = getMode(rect.width, rect.height);
    const design = DESIGN[mode];

    currentRootElement.classList.toggle("loading-screen--mobile", mode === MODE.MOBILE);
    currentRootElement.classList.toggle("loading-screen--pc", mode === MODE.PC);

    const scaleX = design.width > 0 ? rect.width / design.width : 1;
    const scaleY = design.height > 0 ? rect.height / design.height : 1;

    currentRootElement.style.setProperty("--pl-scale-x", `${scaleX}`);
    currentRootElement.style.setProperty("--pl-scale-y", `${scaleY}`);
  }

  function onResize() {
    applyLayout();
  }

  function close() {
    if (!currentOverlayElement) {
      return;
    }

    currentOverlayElement.style.display = "none";
    currentOverlayElement.innerHTML = "";
    currentOverlayElement = null;
    currentRootElement = null;
  }

  function create(overlayElement) {
    if (!overlayElement) {
      return null;
    }

    currentOverlayElement = overlayElement;
    overlayElement.style.display = "";
    overlayElement.innerHTML = getMarkup();

    currentRootElement = overlayElement.querySelector("#loading-page");
    const progressBarLight = overlayElement.querySelector("#progress-bar-light");
    const progressBarFill = overlayElement.querySelector("#progress-bar-fill");

    applyLayout();

    if (!resizeBound) {
      window.addEventListener("resize", onResize);
      resizeBound = true;
    }

    return {
      progressBarFill,
      setProgress(progress) {
        if (!progressBarFill) {
          return;
        }

        const normalizedProgress = Math.max(0, Math.min(1, Number(progress) || 0));
        const width = `${normalizedProgress * 100}%`;
        progressBarFill.style.width = width;

        if (progressBarLight) {
          progressBarLight.style.width = width;
        }
      },
      hide: close
    };
  }

  window.RetroLoadingScreen = {
    create,
    close,
    refreshLayout: applyLayout
  };

  window.closeLoadingScreen = close;
})();
