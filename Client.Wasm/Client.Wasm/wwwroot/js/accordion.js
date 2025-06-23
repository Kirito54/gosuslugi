window.checkAccordionInit = function () {
    console.log('Running checkAccordionInit...');
    if (typeof ej !== "undefined" && ej.base && typeof ej.base.enableRipple === "function") {
        ej.base.enableRipple(true);
    }

    const element = document.querySelector(".e-accordion");
    if (element && !element.ej2_instances) {
        new ej.navigations.Accordion({}, element);
    }
};
