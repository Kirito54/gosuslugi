window.checkAccordionInit = function () {
    const acc = document.querySelector('.e-accordion');
    if (acc && acc.ej2_instances) acc.ej2_instances[0].refresh();
};
