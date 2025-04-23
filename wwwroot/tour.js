var tour;
export function startTour() {
    localStorage.removeItem("tour_current_step");
    localStorage.removeItem("tour_end");
    tour = new Tour({
        template: `<div class='popover tour tour-tour-0'>
            <div class= 'arrow' ></div>
            <h3 class='popover-title'></h3>
            <div class='popover-content'></div>
            <div class='popover-navigation'>
                <button class='btn btn-default' data-role='prev'>« Prev</button>
                <button id="tour-next" class='btn btn-default' data-role='next'>Next »</button>
                <button class='btn btn-sm btn-default' data-role='end'>End tour</button>
            </div>
          </div>`,
        steps: [
            {
                element: "#final-blending-change-type",
                title: "Final Blending",
                content: "Blendings merge it's sublayers into an image. Click here to select type of the final blending.",
                placement: "auto left",
                backdrop: true,
                template: `<div class='popover tour'>
                    <div class= 'arrow' ></div>
                      <h3 class='popover-title'></h3>
                      <div class='popover-content'></div>
                      <div class='popover-navigation'>
                        <button class='btn btn-default' data-role='end'>End tour</button>
                      </div>
                    </div >`,
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
                content: "There are different options for sublayers. You can add blendings or drawings. Unlike blendings, drawings don't have sublayers. Click 'next'.",
                placement: "auto left",
                backdrop: true,
            },
            {
                element: ".modal-item:eq(11)",
                title: "Adding Drawings",
                content: "Click on 'Pattern replicator' to add new drawing that repeats a pattern based on specified parameters.",
                reflex: true,
                placement: "auto right",
                backdrop: true,
            }
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