window.isBottomPart = (y) => {
    console.log(y, window.innerHeight / 2);
    return y > window.innerHeight / 2;
};

window.fitsToTheRight = (x, size) => {
    return x + size < window.innerWidth;
}

window.copyToClipboard = (text) => {
    navigator.clipboard.writeText(text).then(function () {
        alert("URL copied to clipboard!");
    })
    .catch(function (error) {
        alert(error);
    });
};

//TEST
function showTooltip(wrapperId, targetId) {
    const wrapper = document.getElementById(wrapperId);
    const targetElement = document.getElementById(targetId);

    if (!wrapper || !targetElement) {
        console.error('Invalid wrapperId or targetId provided.');
        return;
    }

    const tooltipText = wrapper.querySelector('.tooltip-text');

    if (!tooltipText) {
        console.error('Tooltip text element not found in the wrapper.');
        return;
    }

    const rect = targetElement.getBoundingClientRect();

    // Position the tooltip
    tooltipText.style.position = 'fixed';
    tooltipText.style.top = `${rect.top - tooltipText.offsetHeight}px`;
    tooltipText.style.left = `${rect.left + (rect.width / 2)}px`;
    tooltipText.style.visibility = 'visible';

    // Hide the tooltip on any action
    const hideOnAction = () => {
        tooltipText.style.visibility = 'hidden';
        window.removeEventListener('scroll', hideOnAction);
        window.removeEventListener('click', hideOnAction);
        window.removeEventListener('keydown', hideOnAction);
        document.removeEventListener('scroll', hideOnAction, true); // Capture inner scrolls
    };

    window.addEventListener('scroll', hideOnAction);
    window.addEventListener('click', hideOnAction);
    window.addEventListener('keydown', hideOnAction);
    document.addEventListener('scroll', hideOnAction, true); // Capture inner scrolls
}

function hideTooltip(wrapperId) {
    const wrapper = document.getElementById(wrapperId);
    const tooltipText = wrapper.querySelector('.tooltip-text');
    if (tooltipText) {
        tooltipText.style.visibility = 'hidden';
    }
}