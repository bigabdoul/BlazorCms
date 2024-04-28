
export function setup(scriptSrc) {
    window.Promise ||
        document.write(
            '<script src="https://cdn.jsdelivr.net/npm/promise-polyfill@8/dist/polyfill.min.js"><\/script>'
        );
    window.Promise ||
        document.write(
            '<script src="https://cdn.jsdelivr.net/npm/eligrey-classlist-js-polyfill@1.2.20171210/classList.min.js"><\/script>'
        );
    window.Promise ||
        document.write(
            '<script src="https://cdn.jsdelivr.net/npm/findindex_polyfill_mdn"><\/script>'
        );
    if (!window.ApexCharts) {
        scriptSrc || (scriptSrc = 'https://cdn.jsdelivr.net/npm/apexcharts');
        document.write(`<script src="${scriptSrc}"><\/script>`);
    }
}

const existingCharts = {};

export function realtime(chartOptions) {
    window.Promise && window.ApexCharts || setup();

    if (!chartOptions) {
        console.error('Chart options are required.');
        return false;
    }

    const { selector } = chartOptions;
    const element = document.querySelector(selector);

    if (!element) {
        console.error(`No element found matching query selector '${selector}'`);
        return false;
    }

    const {
        data: optData, title, xaxisMin, xaxisMax, yaxisMin, yaxisMax,
        height, interval, type, curve, plotRange
    } = chartOptions;

    const ONE_DAY_TICKINTERVAL = 86400000 // 24h * 3600s/h * 1000ms/s
    const XAXISRANGE = ONE_DAY_TICKINTERVAL * (plotRange || 30);// 777600000

    const dataOriginal = optData || [];
    let dataCopy = [...dataOriginal];
    const initialDate = new Date(xaxisMin || 0).valueOf();
    const finalDate   = new Date(xaxisMax || 0).valueOf();
    
    const options = {
        series: [{
            data: dataCopy//.slice()
        }],
        chart: {
            //id: 'realtime',
            height,
            type,
            animations: {
                enabled: true,
                easing: 'linear',
                dynamicAnimation: {
                    speed: 1000
                }
            },
            toolbar: {
                show: false
            },
            zoom: {
                enabled: false
            }
        },
        dataLabels: {
            enabled: false
        },
        stroke: {
            curve: curve || 'smooth'
        },
        title: {
            text: title,
            align: 'left'
        },
        markers: {
            size: 0
        },
        xaxis: {
            type: 'datetime',
            range: XAXISRANGE,
        },
        yaxis: {
            max: yaxisMax
        },
        legend: {
            show: false
        },
    };

    let chart = existingCharts[selector];

    if (!chart) {
        chart = new ApexCharts(element, options);
        existingCharts[selector] = chart;
        chart.render();
        //console.log('Chart newly rendered with options', options);
    } else {
        chart.updateOptions(options);
        //console.log('Chart updated with options', options);
    }

    let lastDate = initialDate;

    if (interval && interval > 0) {
        window.setInterval(function () {
            getNewSeries(lastDate, {
                min: yaxisMin,
                max: yaxisMax
            })

            chart.updateSeries([{
                data: dataCopy
            }])
        }, interval);
    }

    return true;

    function getNewSeries(baseval, yrange) {
        if (finalDate && (baseval > finalDate)) {
            // reset data
            baseval = initialDate
            dataCopy = [...dataOriginal];
        }
        const newDate = baseval + ONE_DAY_TICKINTERVAL;
        lastDate = newDate;
        for (let i = 0; i < dataCopy.length - 10; i++) {
            // IMPORTANT
            // we reset the x and y of the data which is out of drawing area
            // to prevent memory leaks
            dataCopy[i].x = newDate - XAXISRANGE - ONE_DAY_TICKINTERVAL
            dataCopy[i].y = 0
        }
        dataCopy.push({
            x: newDate,
            y: Math.floor(Math.random() * (yrange.max - yrange.min + 1)) + yrange.min
        })
    }
}