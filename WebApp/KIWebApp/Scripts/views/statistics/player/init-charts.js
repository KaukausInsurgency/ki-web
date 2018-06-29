$(document).ready(function () {
    var dayMonthYearFormat = '%e %b %Y';
    var hourMinuteSecondsFormat = '%H:%M:%S';

    var gaugesTooltipRender = function(sel) {
        var chart = $(sel).highcharts(),
        point = chart.series[0].points[0];
        point.onMouseOver(); // Show the hover marker
        chart.tooltip.refresh(point); // Show the tooltip
        chart.tooltip.hide = function () { console.log() };
    }

    var responsivePie = {
        rules: [{
            condition: {
                maxWidth: 196
            },
            chartOptions: {
                title: {
                    style: {
                        fontSize: '1.2em'
                    },
                },
            }
        }]
    }

    var responsiveGauges = {
        rules: [{
            condition: { minWidth: 136 },
            chartOptions: {
                title: {
                    style: {
                        fontSize: '1.5em'
                    },
                    y: 165
                },

                tooltip: {
                    enabled: true,
                    style: {
                        fontSize: '0.8em'
                    },
                    
                    pointFormat: '{series.name}<br><span style="font-size:1.7em; color: {point.color}; font-weight: bold">{point.y}%</span>',
                    positioner: function (labelWidth) {
                        return {
                            x: (this.chart.chartWidth - labelWidth) / 2,
                            y: (this.chart.plotHeight / 2) - 25
                        };
                    },
                    
                },

                pane: {
                    size: 130,
                },

                plotOptions: {
                    solidgauge: {
                        borderWidth: '16px',
                    }
                },
            }
        }, {
            condition: {
                maxWidth: 135
            },
            chartOptions: {
                title: {
                    style: {
                        fontSize: '1.2em'
                    },
                    y: 165
                },

                tooltip: {
                    enabled: true,
                    style: {
                        fontSize: '0.8em'
                    },
                   
                    pointFormat: '{series.name}<br><span style="font-size:1.5em; color: {point.color}; font-weight: bold">{point.y}%</span>',
                    positioner: function (labelWidth) {
                        return {
                            x: (this.chart.chartWidth - labelWidth) / 2,
                            y: (this.chart.plotHeight / 2) - 25
                        };
                    },    
                },

                pane: {
                    size: 100,
                },

                plotOptions: {
                    solidgauge: {
                        borderWidth: '12px',
                    }
                },
            }
        }]
    }

    var responsiveLineGraphs = {
        rules: [{
            condition: {
                maxWidth: 500
            },
            chartOptions: {
                title: {
                    style: {
                        fontSize: '1.3em'
                    }
                },
                legend: {
                    layout: 'horizontal',
                    align: 'center',
                    verticalAlign: 'bottom',
                    itemStyle: {
                        fontSize: '7px'
                    }
                },
                xAxis: {
                    title: {
                        style: {
                            fontSize: '10px'
                        }
                    }
                },
                yAxis: {
                    title: {
                        style: {
                            fontSize: '10px'
                        }
                    }
                },
                rangeSelector: {
                    buttonTheme: {
                        width: 30,
                        style: {
                            fontSize: '10px'
                        }
                    },
                    selected: 2
                },
            }
        }]
    }


    // workaround to graphical glitches in highcharts line graphs that add negative fill color when mousing over
    // this appears to only happen in chrome with certain GPUs
    Highcharts.wrap(Highcharts.Series.prototype, 'drawGraph', function (proceed) {
        var lineWidth;
    
        proceed.call(this);
        if (this.graph) {
          lineWidth = this.graph.attr('stroke-width');
          if (
                /Chrome/.test(navigator.userAgent) &&
              lineWidth >= 2 &&
              lineWidth <= 6 &&
              this.graph.attr('stroke-linecap') === 'round'
          ) {
                this.graph.attr('stroke-linecap', 'square');
          }
        }
    });


    Highcharts.chart('hc-last-session', {

        chart: {
            //backgroundColor: null
        },

        title: {
            text: 'Last Session'
        },

        yAxis: {
            title: {
                text: 'Count'
            },
            allowDecimals: false
        },

        xAxis: {
            title: {
                text: 'Time (HH:MM:SS)'
            },
            type: 'datetime',

            dateTimeLabelFormats: {
                day: null,
                week: null,
                month: null,
                year: null,
                hour: '%H:%M',
                minute: '%H:%M:%S'
            }
        },

        tooltip: {
            dateTimeLabelFormats: {
                hour: '%H:%M',
                minute: '%H:%M:%S'
            },
            formatter: function () {
                return this.y + '<br/>' + Highcharts.dateFormat('%H:%M:%S', this.x);
            }
        },

        legend: {
            layout: 'vertical',
            align: 'right',
            verticalAlign: 'middle',
            itemStyle: {
                fontSize: '10px'
            }
        },

        plotOptions: {
            series: {
                label: {
                    connectorAllowed: false
                },
                pointStart: 0,
                dataLabels: {
                    enabled: false,
                    formatter: function () {
                        return Highcharts.dateFormat('%H:%M:%S', this.x);
                    }
                },
            }
        },

        responsive: responsiveLineGraphs,

        series: model.LastSessionSeries,

        exporting: { enabled: false },
        credits: {
            enabled: false
        },

    });




    Highcharts.chart('hc-last-5-sessions', {
        chart: {
            type: 'bar',
            //backgroundColor: null
        },
        title: {
            text: 'Last 5 Sessions'
        },
        xAxis: {
            categories: ['5', '4', '3', '2', '1'],
            title: {
                text: null
            }
        },
        yAxis: {
            min: 0,
            title: {
                text: 'Activity',
                align: 'high'
            },
            labels: {
                overflow: 'justify'
            },
            allowDecimals: false
        },
        plotOptions: {
            bar: {
                dataLabels: {
                    enabled: true
                }
            }
        },
        legend: {
            layout: 'vertical',
            align: 'right',
            verticalAlign: 'top',
            x: -40,
            y: 80,
            floating: true,
            borderWidth: 1,
            backgroundColor: ((Highcharts.theme && Highcharts.theme.legendBackgroundColor) || '#FFFFFF'),
            shadow: true
        },
        credits: {
            enabled: false
        },
        exporting: { enabled: false },
        series: model.LastXSessionsEventsSeries       
    });






    Highcharts.chart('hc-airframes-pie', {
        chart: {
            margin: [0, 0, 30, 0],
            spacingTop: 0,
            spacingBottom: 0,
            spacingLeft: 0,
            spacingRight: 0,
            plotBackgroundColor: null,
            plotBorderWidth: null,
            plotShadow: false,
            type: 'pie',
            backgroundColor: null
        },
        title: {
            text: 'Top 5 Airframes',
        },
        tooltip: {
            pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
        },
        plotOptions: {
            pie: {
                size: '75%',
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: {
                    enabled: false
                },
                showInLegend: true,
                borderWidth: 0
            }
        },
        exporting: { enabled: false },
        credits: {
            enabled: false
        },
        responsive: responsivePie,
        series: [{
            name: 'Airframe',
            colorByPoint: true,
            data: model.TopAirframesSeries
        }]
    });






    Highcharts.chart('hc-score-pie', {
        chart: {
            margin: [0, 0, 30, 0],
            spacingTop: 0,
            spacingBottom: 0,
            spacingLeft: 0,
            spacingRight: 0,
            plotBackgroundColor: null,
            plotBorderWidth: null,
            plotShadow: false,
            type: 'pie',
            backgroundColor: null
        },
        title: {
            text: 'Score',
        },
        tooltip: {
            pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
        },
        plotOptions: {
            pie: {
                size: '75%',
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: {
                    enabled: false
                },
                showInLegend: true,
                borderWidth: 0
            }
        },
        exporting: { enabled: false },
        credits: {
            enabled: false
        },
        responsive: responsivePie,
        series: [{
            name: 'Score',
            colorByPoint: true,
            data: [{
                name: 'Kills',
                y: model.Kills
            }, {
                name: 'Resupplies',
                y: model.DepotResupplies
            }, {
                name: 'Transport',
                y: model.TransportDismounts
            }, {
                name: 'Slingload',
                y: model.SlingLoadUnhooks
            }]
        }]
    });






    Highcharts.chart('hc-sortie-success', {

        chart: {
            margin: [0, 0, 50, 0],
            type: 'solidgauge',
            height: 190,
            backgroundColor: 'rgba(0,0,0,0)',
            spacingTop: 0,
            spacingBottom: 0,
            spacingLeft: 0,
            spacingRight: 0,
            backgroundColor: null
        },

        title: {
            text: 'Sortie',
            style: {
                fontSize: '1.8em'
            },
            margin: 1,
            y: 165
        },

        tooltip: {
            borderWidth: 0,
            backgroundColor: 'none',
            shadow: false,
            style: {
                fontSize: '0.8em'
            },
            pointFormat: '{series.name}<br><span style="font-size:1.7em; color: {point.color}; font-weight: bold">{point.y}%</span>',
            positioner: function (labelWidth) {
                return {
                    x: (this.chart.chartWidth - labelWidth) / 2,
                    y: (this.chart.plotHeight / 2) - 25
                };
            },
            enabled: true
        },

        pane: {
            startAngle: 0,
            endAngle: 360,
            size: 130,
            background: {
                outerRadius: '87%',
                innerRadius: '63%',
                backgroundColor: Highcharts.Color(Highcharts.getOptions().colors[3])
                    .setOpacity(0.3)
                    .get(),
                borderWidth: 0
            }
        },

        yAxis: {
            min: 0,
            max: 100,
            lineWidth: 0,
            tickPositions: []
        },

        plotOptions: {
            solidgauge: {
                borderWidth: '16px',
                dataLabels: {
                    enabled: false
                },
                linecap: 'square',
                stickyTracking: false,
                rounded: true
            },
        },

        exporting: { enabled: false },

        credits: {
            enabled: false
        },

        responsive: responsiveGauges,

        series: [{
            name: 'Success',
            borderColor: Highcharts.getOptions().colors[3],
            data: [{
                color: Highcharts.getOptions().colors[3],
                radius: '75%',
                innerRadius: '75%',
                y: model.SortieSuccessRatio
            }]
        }],
    });




    Highcharts.chart('hc-sling-success', {

        chart: {
            margin: [0, 0, 50, 0],
            type: 'solidgauge',
            height: 190,
            backgroundColor: 'rgba(0,0,0,0)',
            spacingTop: 0,
            spacingBottom: 0,
            spacingLeft: 0,
            spacingRight: 0,
            backgroundColor: null
        },

        title: {
            text: 'Slingload',
            style: {
                fontSize: '1.8em'
            },
            margin: 1,
            y: 165
        },

        tooltip: {
            borderWidth: 0,
            backgroundColor: 'none',
            shadow: false,
            style: {
                fontSize: '0.8em'
            },
            pointFormat: '{series.name}<br><span style="font-size:1.7em; color: {point.color}; font-weight: bold">{point.y}%</span>',
            positioner: function (labelWidth) {
                return {
                    x: (this.chart.chartWidth - labelWidth) / 2,
                    y: (this.chart.plotHeight / 2) - 25
                };
            },
            enabled: true
        },

        pane: {
            startAngle: 0,
            endAngle: 360,
            size: 130,
            background: {
                outerRadius: '87%',
                innerRadius: '63%',
                backgroundColor: Highcharts.Color(Highcharts.getOptions().colors[2])
                    .setOpacity(0.3)
                    .get(),
                borderWidth: 0
            }
        },

        yAxis: {
            min: 0,
            max: 100,
            lineWidth: 0,
            tickPositions: []
        },

        plotOptions: {
            solidgauge: {
                borderWidth: '16px',
                dataLabels: {
                    enabled: false
                },
                linecap: 'square',
                stickyTracking: false,
                rounded: true
            }
        },

        exporting: { enabled: false },

        credits: {
            enabled: false
        },

        responsive: responsiveGauges,

        series: [{
            name: 'Success',
            borderColor: Highcharts.getOptions().colors[2],
            data: [{
                color: Highcharts.getOptions().colors[2],
                radius: '75%',
                innerRadius: '75%',
                y: model.SlingLoadSuccessRatio
            }]
        }]
    });

    


    Highcharts.chart('hc-transport-success', {

        chart: {
            margin: [0, 0, 50, 0],
            type: 'solidgauge',
            height: 190,
            backgroundColor: 'rgba(0,0,0,0)',
            spacingTop: 0,
            spacingBottom: 0,
            spacingLeft: 0,
            spacingRight: 0,
            backgroundColor: null
        },

        title: {
            text: 'Transport',
            style: {
                fontSize: '1.8em'
            },
            margin: 1,
            y: 165
        },

        tooltip: {
            borderWidth: 0,
            backgroundColor: 'none',
            shadow: false,
            style: {
                fontSize: '0.8em'
            },
            pointFormat: '{series.name}<br><span style="font-size:1.7em; color: {point.color}; font-weight: bold">{point.y}%</span>',
            positioner: function (labelWidth) {
                return {
                    x: (this.chart.chartWidth - labelWidth) / 2,
                    y: (this.chart.plotHeight / 2) - 25
                };
            },
            enabled: true
        },

        pane: {
            startAngle: 0,
            endAngle: 360,
            size: 130,
            background: {
                outerRadius: '87%',
                innerRadius: '63%',
                backgroundColor: Highcharts.Color(Highcharts.getOptions().colors[0])
                    .setOpacity(0.3)
                    .get(),
                borderWidth: 0
            }
        },

        yAxis: {
            min: 0,
            max: 100,
            lineWidth: 0,
            tickPositions: []
        },

        plotOptions: {
            solidgauge: {
                borderWidth: '16px',
                dataLabels: {
                    enabled: false
                },
                linecap: 'square',
                stickyTracking: false,
                rounded: true
            }
        },

        exporting: { enabled: false },

        credits: {
            enabled: false
        },

        responsive: responsiveGauges,

        series: [{
            name: 'Success',
            borderColor: Highcharts.getOptions().colors[0],
            data: [{
                color: Highcharts.getOptions().colors[0],
                radius: '75%',
                innerRadius: '75%',
                y: model.TransportSuccessRatio
            }]
        }]
    });

    gaugesTooltipRender('#hc-sortie-success');
    gaugesTooltipRender('#hc-sling-success');
    gaugesTooltipRender('#hc-transport-success');






    Highcharts.chart('hc-online-activity', {

        chart: {
            //backgroundColor: null,
            type: 'line'
        },

        title: {
            text: 'Online Time'
        },

        rangeSelector: {
            enabled: true,
            allButtonsEnabled: true,
            buttons: [{
                type: 'week',
                count: 1,
                text: 'Week',
            }, {
                type: 'month',
                count: 1,
                text: 'Month',
            }, {
                type: 'year',
                count: 1,
                text: 'Year'
            }, {
                type: 'all',
                text: 'All'
            }],
            buttonTheme: {
                width: 60
            },
            selected: 2
        },

        yAxis: {
            title: {
                text: 'Online'
            },
            type: 'datetime',
            allowDecimals: true,
            dateTimeLabelFormats: {
                day: null,
                week: null,
                month: null,
                year: null,
                hour: '%H:%M',
                minute: hourMinuteSecondsFormat
            }
        },

        xAxis: {
            title: {
                text: 'Time'
            },
            type: 'datetime',
            dateTimeLabelFormats: {
                day: dayMonthYearFormat
            }
        },

        tooltip: {
            dateTimeLabelFormats: {
                hour: '%H:%M',
                minute: hourMinuteSecondsFormat,
                minTickInterval: 3600 * 1000
            },
            formatter: function () {
                return Highcharts.dateFormat(hourMinuteSecondsFormat, this.y) + '<br/>' + Highcharts.dateFormat(dayMonthYearFormat, this.x);
            }
        },

        legend: {
            layout: 'vertical',
            align: 'right',
            verticalAlign: 'middle'
        },

        plotOptions: {
            series: {
                label: {
                    connectorAllowed: false
                },
                pointStart: 0,
                dataLabels: {
                    enabled: false,
                    formatter: function () {
                        return Highcharts.dateFormat('%H:%M:%S', this.x);
                    }
                },
            },
        },

        series: [
            {
                showInLegend: false,
                name: "Time",
                data: model.OnlineActivity.Series
            }
        ],

        responsive: responsiveLineGraphs,
        exporting: { enabled: false },
        credits: {
            enabled: false
        },

    });














    Highcharts.chart('hc-total-sorties', {

        chart: {
            //backgroundColor: null,
            type: 'area'
        },

        title: {
            text: 'Sorties'
        },

        rangeSelector: {
            enabled: true,
            allButtonsEnabled: true,
            buttons: [{
                type: 'week',
                count: 1,
                text: 'Week',
            }, {
                type: 'month',
                count: 1,
                text: 'Month',
            }, {
                type: 'year',
                count: 1,
                text: 'Year'
            }, {
                type: 'all',
                text: 'All'
            }],
            buttonTheme: {
                width: 60
            },
            selected: 2
        },

        yAxis: {
            title: {
                text: 'Sorties'
            },
            allowDecimals: false,
        },

        xAxis: {
            title: {
                text: 'Time'
            },
            type: 'datetime',
            dateTimeLabelFormats: {
                day: dayMonthYearFormat
            }
        },

        tooltip: {
            dateTimeLabelFormats: {
                hour: '%H:%M',
                minute: hourMinuteSecondsFormat,
                minTickInterval: 3600 * 1000
            },
            formatter: function () {
                return this.series.name + '<br/>' +
                    Highcharts.dateFormat(dayMonthYearFormat, this.x) + '<br/>' +
                    'Sorties: ' + this.y + '<br/>' +
                    'Kills: ' + this.point.kills + '<br/>' +
                    'Deaths: ' + this.point.deaths;
            }
        },

        legend: {
            layout: 'vertical',
            align: 'right',
            verticalAlign: 'middle'
        },

        plotOptions: {
            series: {
                label: {
                    connectorAllowed: false
                },
                pointStart: 0,
                dataLabels: {
                    enabled: false,
                    formatter: function () {
                        return Highcharts.dateFormat('%H:%M:%S', this.x);
                    }
                },
            },
        },

        series: model.SortiesOverTime,

        responsive: responsiveLineGraphs,
        exporting: { enabled: false },
        credits: {
            enabled: false
        },

    });










    Highcharts.chart('hc-score-over-time', {

        chart: {
            //backgroundColor: null,
            type: 'line'
        },

        title: {
            text: 'Score'
        },

        rangeSelector: {
            enabled: true,
            allButtonsEnabled: true,
            buttons: [{
                type: 'week',
                count: 1,
                text: 'Week',
            }, {
                type: 'month',
                count: 1,
                text: 'Month',
            }, {
                type: 'year',
                count: 1,
                text: 'Year'
            }, {
                type: 'all',
                text: 'All'
            }],
            buttonTheme: {
                width: 60
            },
            selected: 2
        },

        yAxis: {
            title: {
                text: 'Count'
            },
            allowDecimals: false,
        },

        xAxis: {
            title: {
                text: 'Time'
            },
            type: 'datetime',
            dateTimeLabelFormats: {
                day: dayMonthYearFormat
            }
        },

        tooltip: {
            dateTimeLabelFormats: {
                hour: '%H:%M',
                minute: hourMinuteSecondsFormat,
                minTickInterval: 3600 * 1000
            },
            formatter: function () {
                return Highcharts.dateFormat(dayMonthYearFormat, this.x) + '<br/>' +
                    'Count: ' + this.y
            }
        },

        legend: {
            layout: 'vertical',
            align: 'right',
            verticalAlign: 'middle'
        },

        plotOptions: {
            series: {
                label: {
                    connectorAllowed: false
                },
                pointStart: 0,
                dataLabels: {
                    enabled: false,
                    formatter: function () {
                        return Highcharts.dateFormat('%H:%M:%S', this.x);
                    }
                },
            },
        },

        series: model.ScoreOverTime,

        responsive: responsiveLineGraphs,
        exporting: { enabled: false },
        credits: {
            enabled: false
        },

    });


    // special callback function that checks if a circe gauge graph has hit a responsive breakpoint
    // currently when the breakpoint on these charts is hit - the tooltip content disappears
    // this is because highcharts calls the hide() function on the tooltip
    // we need to force show the tooltip again after highcharts has finished redrawing the chart
    // because we cant control if our handler is the last to be called 
    // (we need highcharts to finish rendering first)
    // we fire off the re-render function in an async function that waits 1 second before rendering
    function sleep(duration) {
        return new Promise((resolve) => setTimeout(resolve, duration));
    }

    async function delayRenderGaugeTooltip($el) {
        await sleep(1000);
        gaugesTooltipRender($el);  
    }

    $(window).on('resize', function(){
        $('#hc-sortie-success, #hc-sling-success, #hc-transport-success').each(function() {
            var $this = $(this);

            // if this div has not had it's breakpoint hit yet, rerender the tooltip content
            if ($this.width() <= 135 && !$this.hasClass('js-breakpoint')) {
                $this.addClass('js-breakpoint');
                delayRenderGaugeTooltip($this);
                
            }
            else if ($this.width() > 135 && $this.hasClass('js-breakpoint')) {
                $this.removeClass('js-breakpoint');
                delayRenderGaugeTooltip($this);
            }
        })
    });
    
});