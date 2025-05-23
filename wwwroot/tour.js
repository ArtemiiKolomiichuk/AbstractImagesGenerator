﻿var tour;
export function startTour() {
    localStorage.removeItem("tour_current_step");
    localStorage.removeItem("tour_end");
    const actionlessTemplate = `<div class='popover tour tour-tour-0'>
            <div class= 'arrow' ></div>
            <div class='tour-header'>
                <h3 class='popover-title'></h3>
                <button class='btn btn-sm btn-default tour-end' data-role='end'>X</button>
            </div>
            <div class='popover-content'></div>
          </div>`;
    const nextTemplate = `<div class='popover tour tour-tour-0'>
        <div class='arrow' ></div>
        <div class='tour-header'>
            <h3 class='popover-title'></h3>
            <button class='btn btn-sm btn-default tour-end' data-role='end'>X</button>
        </div>
        <div class='popover-content'></div>
        <div class='popover-navigation'>
                <button id="tour-next" class='bttn' data-role='next'>Next »</button>
            </div>
    </div>`;

    tour = new Tour({
        template: actionlessTemplate,
        steps: [
            {
                element: "#final-blending-change-type",
                title: "Final Blending",
                content: "Blendings merge it's sublayers into an image. Click here to select type of the final blending.",
                placement: "auto left",
                backdrop: true,
                onShown: function (tour) {
                    $('#final-blending-change-type').one('click', function () {
                        $('#final-blending-change-type').off('click');
                        setTimeout(() => {
                            tour.goTo(1);
                        }, 50)
                    });
                }
            },
            {
                element: ".modal-item:eq(2)",
                title: "Blending Type",
                content: "There are different blending types available. Each one has different parameters. Click here to select blending that generates " +
                    "zones in pattern that looks like waves.",
                reflex: true,
                placement: "auto left",
                backdrop: true
            },
            {
                element: "#final-blending-add-layer",
                title: "Adding Sublayers",
                content: "Click here to add layer to the final blending.",
                reflex: true,
                placement: "auto left",
                backdrop: true,
                onNext: function (tour) {
                    setTimeout(() => {
                        tour.goTo(3);
                    }, 300)
                }
            },
            {
                element: ".modal-content:eq(0)",
                title: "Blendings & Drawings",
                template: nextTemplate,
                reflex: true,
                content: "There are different options for sublayers. You can add blendings or drawings. Unlike blendings, drawings don't have sublayers. Click 'next'.",
                placement: "auto left",
                backdrop: true,
            },

            {
                element: ".tooltip-ico:eq(0)",
                title: "Tooltip",
                template: nextTemplate,
                content: "If you're unsure about what these algorithms do, you can always hover over the tooltip icon for more information. Click 'next'.",
                placement: "auto left",
                backdrop: true,
            },

            {
                element: ".modal-item:eq(12)",
                title: "Adding Drawings",
                content: "Click on 'Pattern replicator' to add new drawing that repeats a pattern based on specified parameters.",
                reflex: true,
                placement: "auto right",
                backdrop: true,
            },
            {
                element: ".setting-row:eq(0)",
                title: "Parameters of blending 2",
                template: nextTemplate,
                content: "There are two types of blending parameters: those that affect the blending algorithm itself, and those that are applied to each individual sublayer. The parameters shown here influence the blending algorithm—specifically, they control the number of waves generated for later distribution.",
                reflex: true,
                placement: "auto left",
                backdrop: true,
            },
            {
                element: ".setting-row:eq(5)",
                title: "Parameters of blending 2",
                template: nextTemplate,
                content: "The parameters shown here are used by the blending to determine how sublayers are processed. Specifically in this case, they define the weights assigned to algorithm, which are then used to distribute zones accordingly.",
                reflex: true,
                placement: "auto left",
                backdrop: true,

            },
            {
                element: ".settings:eq(0)",
                title: "Parameters of layers",
                template: nextTemplate,
                content: "Each layer has its own parameters that define how it is generated. These are primarily controlled by sliders, but occasionally you may encounter combo boxes and checkboxes as well.",
                reflex: true,
                placement: "auto left",
                backdrop: true,
            },
            {
                element: ".icon-button:eq(7)",
                title: "Parameters of layers - Color",
                content: "One of most important parameters is color. If you click here you can add new random color to algorithm.",
                reflex: true,
                placement: "auto left",
                backdrop: true,
            },
            {
                element: ".icon-button:eq(7)",
                title: "Parameters of layers - Color",
                content: "If the maximum number of colors hasn't been reached, you'll still see the button to add more colors. Click to add another color.",
                reflex: true,
                placement: "auto left",
                backdrop: true,
            },
            {
                element: ".icon-button:eq(8)",
                title: "Parameters of layers - Color",
                content: "Click here to randomize the colors you've already added.",
                reflex: true,
                placement: "auto left",
                backdrop: true,
            },
            {
                element: ".icon-button:eq(7)",
                title: "Parameters of layers - Color",
                content: "If you want to add a specific color, you can do that as well. Let’s add one more color.",
                reflex: true,
                placement: "auto left",
                backdrop: true,
                onNext: function (tour) {
                    const checkModal = () => {
                        const modal = document.querySelectorAll(".color-value")[2];
                        if (modal && modal.offsetParent !== null) {
                            tour.goTo(13);
                        } else {
                            setTimeout(checkModal, 200);
                        }
                    };
                    checkModal();
                }
            },
            {
                element: ".color-value:eq(2)",
                title: "Parameters of layers - Color",
                content: "Now click on color icon to change it",
                reflex: true,
                placement: "auto left",
                backdrop: true,
                onNext: function (tour) {
                    const checkModal = () => {
                        const modal = document.querySelectorAll(".color-modal-content")[0];
                        if (modal && modal.offsetParent !== null) {
                            tour.goTo(14);
                        } else {
                            setTimeout(checkModal, 200);
                        }
                    };
                    checkModal();
                }
            },
            {
                element: ".color-modal-content:eq(0)",
                title: "Parameters of layers - Color",
                template: nextTemplate,
                content: "Here you can change the selected color. If you're above the minimum number of colors required by the algorithm, you can also delete this color. Click 'next'.",
                reflex: false,
                placement: "auto left",
                backdrop: true,
            },

            {
                element: ".drag-handle:eq(1)",
                title: "Parameters of layers - Color",
                template: nextTemplate,
                content: "You can also rearrange the colors if needed. Click 'next'.",
                reflex: false,
                placement: "auto left",
                backdrop: true,
            },

            {
                element: ".setting-row-title:eq(1)",
                title: "Parameters of layers",
                template: nextTemplate,
                content: "If you're unsure what a specific layer parameter does, you can always hover over its name to see a tooltip with more information.",
                reflex: false,
                placement: "auto left",
                backdrop: true,
            },

            {
                element: ".icon-button:eq(4)",
                title: "Layer ability to hide",
                content: "If you done working with layer you can hide it.",
                reflex: true,
                placement: "auto left",
                backdrop: true,
            },

            {
                element: ".icon-button:eq(3)",
                title: "Rename Layer",
                content: "You have ability to rename layer.",
                reflex: true,
                placement: "auto left",
                backdrop: true,
            },

            {
                element: ".editing-title:eq(0)",
                title: "Rename Layer",
                content: "You have ability to rename layer.",
                reflex: true,
                placement: "auto left",
                backdrop: true,
            },
            {
                element: ".icon-button:eq(4)",
                title: "Copy layer",
                content: "You can duplicate layer with all the parameters.",
                reflex: true,
                placement: "auto left",
                backdrop: true,
                onNext: function (tour) {
                    const checkModal = () => {
                        const modal = document.querySelectorAll(".icon-button")[10];
                        if (modal && modal.offsetParent !== null) {
                            tour.goTo(21);
                        } else {
                            setTimeout(checkModal, 200);
                        }
                    };
                    checkModal();
                }
            },

            {
                element: ".icon-button:eq(10)",
                title: "Delete layer",
                content: "If you added a layer by mistake, you can always delete it.",
                reflex: true,
                placement: "auto left",
                backdrop: true,
            },

            {
                element: ".icon-button:eq(1)",
                title: "Add more blendings and layers",
                content: "Lets add more blendings and layers.",
                reflex: true,
                placement: "auto left",
                backdrop: true,
                onNext: function (tour) {
                    setTimeout(() => {
                        tour.goTo(23);
                    }, 300)
                }
            },

            {
                element: ".modal-item:eq(0)",
                title: "Add more blendings and layers",
                content: "Lets add Transparency blending.",
                reflex: true,
                placement: "auto right",
                backdrop: true,
            },

            {
                element: ".layer-card:eq(1)",
                title: "Layers and blendings",
                template: nextTemplate,
                content: "As you can see layers that generate images are blue. Click 'next'.",
                reflex: true,
                placement: "auto right",
                backdrop: true,
            },

            {
                element: ".layer-card:eq(2)",
                title: "Layers and blendings",
                template: nextTemplate,
                content: "As you can see layers that blend images are red. Click 'next'.",
                reflex: true,
                placement: "auto right",
                backdrop: true,
            },

            {
                element: ".icon-button:eq(9)",
                title: "Adding more sublayers",
                content: "Lets add sublayer in newly added Transparency blending.",
                reflex: true,
                placement: "auto left",
                backdrop: true,
                onNext: function (tour) {
                    setTimeout(() => {
                        tour.goTo(27);
                    }, 800)
                }
            },

            {
                element: ".modal-item:eq(6)",
                title: "Adding more sublayers",
                content: "Lets add Cosmic flow.",
                reflex: true,
                placement: "auto right",
                backdrop: true,
                onNext: function (tour) {
                    setTimeout(() => {
                        tour.goTo(28);
                    }, 300)
                }
            },

            {
                element: ".icon-button:eq(17)",
                title: "Randomise colors",
                content: "Lets randomise colors in cosmic flow.",
                reflex: true,
                placement: "auto right",
                backdrop: true,
            },

            {
                element: ".icon-button:eq(13)",
                title: "Hide obstructing layer",
                content: "Lets hide Cosmic flow layer for now.",
                reflex: true,
                placement: "auto left",
                backdrop: true,
            },

            {
                element: ".icon-button:eq(1)",
                title: "Adding more sublayers",
                content: "Lets add one more sublayer but in our main blending.",
                reflex: true,
                placement: "auto left",
                backdrop: true,
                onNext: function (tour) {
                    setTimeout(() => {
                        tour.goTo(31);
                    }, 300)
                }
            },

            {
                element: ".modal-item:eq(10)",
                title: "Adding more sublayers",
                content: "Lets add Celular pattern.",
                reflex: true,
                placement: "auto right",
                backdrop: true,
            },

            {
                element: ".drag-handle:eq(4)",
                title: "Rearrange layer",
                template: nextTemplate,
                content: "You can rearrange layers using this handle.",
                reflex: true,
                placement: "auto left",
                backdrop: true,
            },

            {
                element: ".layer-card:eq(0)",
                title: "Rearrange layer",
                template: nextTemplate,
                content: "Drag in Celular pattern layer in Transparency blending under Cosmic flow and click 'next'.",
                reflex: false,
                placement: "auto left",
                backdrop: true,
            },

            {
                element: ".selector:eq(0)",
                title: "Select size",
                content: "There you can select size of generated image",
                reflex: true,
                placement: "auto left",
                backdrop: true,
            },

            {
                element: ".selector:eq(0)",
                title: "Select size",
                content: "Select the size you want",
                reflex: true,
                placement: "auto left",
                backdrop: true,
            },

            {
                element: ".bttn:eq(0)",
                title: "Generate image",
                content: "Lets generate our image",
                reflex: true,
                placement: "auto left",
                backdrop: true,
                onNext: function (tour) {
                    setTimeout(() => {
                        tour.goTo(37);
                    }, 300)
                }
            },

            {
                element: ".download-container:eq(0)",
                title: "Other actions",
                template: nextTemplate,
                content: "You can download, like and share your image",
                reflex: true,
                placement: "auto left",
                backdrop: true,
            },

            {
                element: ".icon-button:eq(1)",
                title: "Lock seed",
                content: "After an image is generated, locks appear on the layers. These can be activated to lock the seed based on what was generated layer. If you activate the lock on a blending layer, all locks in its sublayers will also be locked.",
                reflex: true,
                placement: "auto left",
                backdrop: true,
            },

            {
                element: ".bttn:eq(0)",
                title: "That's all!",
                content: "That's all! Now you can try regenerating the image with locks enabled and then without to see the difference in results.",
                placement: "auto left",
                reflex:true,
                backdrop: true,
            },
        ]
    });
    tour.init();
    setTimeout(() => {
        tour.start(true);
    }, 200);
};

export function stopTour() {
    if (tour) {
        tour.end();
        tour = null;
    }
}