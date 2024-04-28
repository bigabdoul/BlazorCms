'use strict';

const SCRIPTS_NOT_AVAILABLE = 'jQuery and/or select2 are not available! Make sure you\'re connected to the Internet or insert the scripts into the DOM.';

const EVENT_TYPES = {
    change: 'change',
    change_select2: 'change.select2',
    closing: 'select2:closing',
    close: 'select2:close',
    opening: 'select2:opening',
    open: 'select2:open',
    selecting: 'select2:selecting',
    select: 'select2:select',
    unselecting: 'select2:unselecting',
    unselect: 'select2:unselect',
    clearing: 'select2:clearing',
    clear: 'select2:clear',
}

export function setup(scriptSrc, styleSrc) {
    window.jQuery ||
        document.write(
            '<script src="https://cdn.jsdelivr.net/npm/jquery@3.7.1/dist/jquery.min.js"><\/script>'
        );
    if (!window.jQuery.fn.select2) {
        scriptSrc || (scriptSrc = 'https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/js/select2.min.js');
        document.write(`<script src="${scriptSrc}"><\/script>`);

        const id = 'app-cms-select2-css-3b9cfbe7d2594223ab8c6e376c9a4dca';

        if (!document.getElementById(id)) {
            const link = document.createElement('link');
            link.setAttribute('id', id);
            link.rel = 'stylesheet';
            link.href = styleSrc || 'https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/css/select2.min.css';
            document.head.appendChild(link);
        }
    }
}

export function init(options) {

    // make sure jQuery and select2 are available
    window.jQuery && window.jQuery.fn.select2 || setup();

    // if it's still not there, quit!
    if (!window.jQuery || !window.jQuery.fn.select2)
        throw new Error(SCRIPTS_NOT_AVAILABLE);

    jQuery(() => {
        const { selector, dotNet, dotNetMethod } = options;

        jQuery(selector).select2();

        if (dotNet && dotNetMethod) {
            // wire up event handlers so that we may notify the calling .NET component
            jQuery(selector).on(EVENT_TYPES.select, e => {

                const { disabled, selected, id, text, title } = e.params.data;
                const data = { id, text, title, selected, disabled, eventType: EVENT_TYPES.select };
                dotNet.invokeMethodAsync(dotNetMethod, data);

            }).on(EVENT_TYPES.unselect, e => {

                const { disabled, selected, id, text, title } = e.params.data;
                const data = { id, text, title, selected, disabled, eventType: EVENT_TYPES.unselect };
                dotNet.invokeMethodAsync(dotNetMethod, data);

            });
        }
    });
}