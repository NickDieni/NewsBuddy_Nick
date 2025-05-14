window.addLongPressListener = (elementId, dotNetHelper) => {
    const element = document.getElementById(elementId);
    if (!element) return;

    let pressTimer;

    const start = () => {
        pressTimer = setTimeout(() => {
            dotNetHelper.invokeMethodAsync("OnLongPress");
        }, 800); // Long press threshold
    };

    const cancel = () => clearTimeout(pressTimer);

    element.addEventListener("mousedown", start);
    element.addEventListener("touchstart", start);
    element.addEventListener("mouseup", cancel);
    element.addEventListener("mouseleave", cancel);
    element.addEventListener("touchend", cancel);
};
