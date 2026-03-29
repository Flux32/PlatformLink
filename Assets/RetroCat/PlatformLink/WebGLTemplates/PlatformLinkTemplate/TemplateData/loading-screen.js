(function () {
  const MODE = {
    PC: "pc",
    MOBILE: "mobile"
  };

  const DESIGN = {
    [MODE.PC]: { width: 1440, height: 1024, catTop: 259, catSize: 318.78, loadingTop: 606 },
    [MODE.MOBILE]: { width: 375, height: 812, catTop: 259, catSize: 204.01919555664062, loadingTop: 477 }
  };

  const MOBILE_MAX_WIDTH = 812;
  const MOBILE_PORTRAIT_ASPECT = 1;
  const MOBILE_FORCE_WIDTH = 480;

  let currentOverlayElement = null;
  let currentRootElement = null;
  let currentProgressBarFill = null;
  let currentProgressBarLight = null;
  let currentProgressFillClip = null;
  let currentProgress = 0;
  let resizeBound = false;

  function getMarkup() {
    return [
      '<div id="loading-page" class="loading-screen loading-screen--pc" data-node-id="3408:21099">',
      '  <div class="loading-screen__content">',
      '    <img class="loading-screen__cat" src="TemplateData/loading-cat.png" alt="RETRO CAT">',
      '    <p class="loading-screen__text">Загрузка...</p>',
      '    <div class="loading-screen__progress">',
      '      <div class="loading-screen__progress-track"></div>',
      '        <div id="progress-bar-light" class="loading-screen__progress-light"></div>',
      '      <div class="loading-screen__progress-fill-clip">',
      '        <div id="progress-bar-fill" class="loading-screen__progress-fill"></div>',
      '      </div>',
      '      </div>',
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

    const desiredCatTop = (design.catTop + (design.catSize * 0.5)) * scaleY - (design.catSize * 0.5);
    const maxCatTop = (design.loadingTop * scaleY) - design.catSize - 12;
    const catTopPx = Math.max(0, Math.min(desiredCatTop, maxCatTop));
    currentRootElement.style.setProperty("--pl-cat-top-px", `${catTopPx}`);

    applyProgress(currentProgress);
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
    currentProgressBarFill = null;
    currentProgressBarLight = null;
    currentProgressFillClip = null;
    currentProgress = 0;
  }

  function applyProgress(progress) {
    if (!currentProgressBarFill || !currentProgressFillClip) {
      return;
    }

    const normalizedProgress = Math.max(0, Math.min(1, Number(progress) || 0));
    currentProgress = normalizedProgress;

    const clipWidth = currentProgressFillClip.clientWidth;
    const fillWidthPx = clipWidth * normalizedProgress;
    const widthPx = `${fillWidthPx}px`;

    currentProgressBarFill.style.width = widthPx;

    if (currentProgressBarLight) {
      currentProgressBarLight.style.width = widthPx;
      currentProgressBarLight.style.opacity = normalizedProgress > 0 ? "1" : "0";
    }
  }

  function create(overlayElement) {
    if (!overlayElement) {
      return null;
    }

    currentOverlayElement = overlayElement;
    overlayElement.style.display = "";
    overlayElement.innerHTML = getMarkup();

    currentRootElement = overlayElement.querySelector("#loading-page");
    currentProgressBarLight = overlayElement.querySelector("#progress-bar-light");
    currentProgressFillClip = overlayElement.querySelector(".loading-screen__progress-fill-clip");
    currentProgressBarFill = overlayElement.querySelector("#progress-bar-fill");

    applyLayout();

    if (!resizeBound) {
      window.addEventListener("resize", onResize);
      resizeBound = true;
    }

    return {
      setProgress(progress) {
        applyProgress(progress);
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
