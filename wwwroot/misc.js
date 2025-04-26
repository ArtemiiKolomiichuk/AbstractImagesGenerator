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

    tooltipText.style.position = 'fixed';
    tooltipText.style.top = `${rect.top - tooltipText.offsetHeight}px`;
    tooltipText.style.left = `${rect.left + (rect.width / 2)}px`;
    tooltipText.style.visibility = 'visible';

    const hideOnAction = () => {
        tooltipText.style.visibility = 'hidden';
        window.removeEventListener('scroll', hideOnAction);
        window.removeEventListener('click', hideOnAction);
        window.removeEventListener('keydown', hideOnAction);
        document.removeEventListener('scroll', hideOnAction, true); 
    };

    window.addEventListener('scroll', hideOnAction);
    window.addEventListener('click', hideOnAction);
    window.addEventListener('keydown', hideOnAction);
    document.addEventListener('scroll', hideOnAction, true); 
}

function hideTooltip(wrapperId) {
    const wrapper = document.getElementById(wrapperId);
    const tooltipText = wrapper.querySelector('.tooltip-text');
    if (tooltipText) {
        tooltipText.style.visibility = 'hidden';
    }
}

window.infiniteScrollHandler = {
    dotNetHelper: null,
    scrollHandler: null,

    initialize: function (dotNetHelper) {
        this.dotNetHelper = dotNetHelper;

        this.scrollHandler = this.handleScroll.bind(this);
        window.addEventListener('scroll', this.scrollHandler);
    },

    handleScroll: function () {
        const galleryElement = document.getElementById('gallery');
        if (galleryElement) {
            const galleryRect = galleryElement.getBoundingClientRect();
            if (galleryRect.bottom <= window.innerHeight + 200) {
                this.dotNetHelper.invokeMethodAsync('OnScrollToBottom');
            }
        }
    },

    dispose: function () {
        if (this.scrollHandler) {
            window.removeEventListener('scroll', this.scrollHandler);
            this.scrollHandler = null;
        }
        this.dotNetHelper = null;
    }
};