'use strict';

const SCRIPTS_NOT_AVAILABLE = 'jQuery and/or select2 are not available! Make sure you\'re connected to the Internet or insert the scripts into the DOM.';

const SELECT2_EVENTS = {
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

enum EventTypes {
    change,
    changeSelect2,
    closing,
    close,
    opening,
    open,
    selecting,
    select,
    unselecting,
    unselect,
    clearing,
    clear,
}

export function setup(options?: { script?: string, style?: string }) {
    let { script, style } = options || {};

    window['jQuery'] ||
        document.write(
            '<script src="https://cdn.jsdelivr.net/npm/jquery@3.7.1/dist/jquery.min.js"><\/script>'
        );
    if (!window['jQuery'].fn.select2) {
        script || (script = 'https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/js/select2.min.js');
        document.write(`<script src="${script}"><\/script>`);

        const id = 'app-cms-select2-css-3b9cfbe7d2594223ab8c6e376c9a4dca';

        if (!document.getElementById(id)) {
            const link = document.createElement('link');
            link.setAttribute('id', id);
            link.rel = 'stylesheet';
            link.href = style || 'https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/css/select2.min.css';
            document.head.appendChild(link);
        }
    }
}

export function init(options: { selector: string, dotNet?: any, dotNetMethod?: string }) {

    // make sure jQuery and select2 are available
    window['jQuery'] && window['jQuery'].fn.select2 || setup();

    // if it's still not there, quit!
    if (!window['jQuery'] || !window['jQuery'].fn.select2)
        throw new Error(SCRIPTS_NOT_AVAILABLE);

    const jQuery = window['jQuery'];

    jQuery(() => {
        const { selector, dotNet, dotNetMethod } = options;

        jQuery(selector).select2();

        if (dotNet && dotNetMethod) {
            // wire up event handlers so that we may notify the calling .NET component
            jQuery(selector).on(SELECT2_EVENTS.select, e => {

                const { disabled, selected, id, text, title } = e.params.data;
                const data = { id, text, title, selected, disabled, eventType: EventTypes.select };

                // notify
                dotNet.invokeMethodAsync(dotNetMethod, data);

            }).on(SELECT2_EVENTS.unselect, e => {

                const { disabled, selected, id, text, title } = e.params.data;
                const data = { id, text, title, selected, disabled, eventType: EventTypes.unselect };
                dotNet.invokeMethodAsync(dotNetMethod, data);

            });
        }
    });
}