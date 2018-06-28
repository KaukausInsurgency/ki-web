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


    var responsiveBarGraphs = {
        rules: [{
            condition: {
                maxWidth: 420
            },
            chartOptions: {
                title: {
                    style: {
                        fontSize: '1.3em'
                    }
                },
                legend: {
                    enabled: false
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


    Highcharts.chart('hc-last-sortie', {

        chart: {
            //backgroundColor: null
        },

        title: {
            text: 'Last Sortie'
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

        series: [
            {
                "name": "TAKEOFF",
                "data": [
                    {
                        "x": 443000,
                        "y": 1
                    }
                ],
                "_colorIndex": 0,
                "_symbolIndex": 0
            },
            {
                "name": "SHOT",
                "data": [
                    {
                        "x": 779000,
                        "y": 1
                    },
                    {
                        "x": 922000,
                        "y": 2
                    },
                    {
                        "x": 987000,
                        "y": 3
                    },
                    {
                        "x": 1030000,
                        "y": 4
                    },
                    {
                        "x": 1096000,
                        "y": 5
                    },
                    {
                        "x": 1487000,
                        "y": 6
                    },
                    {
                        "x": 1532000,
                        "y": 7
                    },
                    {
                        "x": 2077000,
                        "y": 8
                    },
                    {
                        "x": 2108000,
                        "y": 9
                    },
                    {
                        "x": 2122000,
                        "y": 10
                    },
                    {
                        "x": 2122000,
                        "y": 11
                    },
                    {
                        "x": 2138000,
                        "y": 12
                    },
                    {
                        "x": 2156000,
                        "y": 13
                    },
                    {
                        "x": 2156000,
                        "y": 14
                    },
                    {
                        "x": 2156000,
                        "y": 15
                    },
                    {
                        "x": 2156000,
                        "y": 16
                    },
                    {
                        "x": 2156000,
                        "y": 17
                    },
                    {
                        "x": 2156000,
                        "y": 18
                    },
                    {
                        "x": 2156000,
                        "y": 19
                    },
                    {
                        "x": 2156000,
                        "y": 20
                    },
                    {
                        "x": 2156000,
                        "y": 21
                    },
                    {
                        "x": 2156000,
                        "y": 22
                    },
                    {
                        "x": 2156000,
                        "y": 23
                    },
                    {
                        "x": 2156000,
                        "y": 24
                    },
                    {
                        "x": 2156000,
                        "y": 25
                    },
                    {
                        "x": 2156000,
                        "y": 26
                    },
                    {
                        "x": 2156000,
                        "y": 27
                    },
                    {
                        "x": 2156000,
                        "y": 28
                    },
                    {
                        "x": 2156000,
                        "y": 29
                    },
                    {
                        "x": 2156000,
                        "y": 30
                    },
                    {
                        "x": 2156000,
                        "y": 31
                    },
                    {
                        "x": 2156000,
                        "y": 32
                    }
                ],
                "_colorIndex": 1,
                "_symbolIndex": 1
            },
            {
                "name": "HIT",
                "data": [
                    {
                        "x": 789000,
                        "y": 1
                    },
                    {
                        "x": 936000,
                        "y": 2
                    },
                    {
                        "x": 1044000,
                        "y": 3
                    },
                    {
                        "x": 1044000,
                        "y": 4
                    },
                    {
                        "x": 1108000,
                        "y": 5
                    },
                    {
                        "x": 1500000,
                        "y": 6
                    },
                    {
                        "x": 1500000,
                        "y": 7
                    },
                    {
                        "x": 1546000,
                        "y": 8
                    },
                    {
                        "x": 2081000,
                        "y": 9
                    },
                    {
                        "x": 2081000,
                        "y": 10
                    },
                    {
                        "x": 2112000,
                        "y": 11
                    },
                    {
                        "x": 2112000,
                        "y": 12
                    },
                    {
                        "x": 2158000,
                        "y": 13
                    },
                    {
                        "x": 2158000,
                        "y": 14
                    },
                    {
                        "x": 2158000,
                        "y": 15
                    },
                    {
                        "x": 2158000,
                        "y": 16
                    },
                    {
                        "x": 2158000,
                        "y": 17
                    },
                    {
                        "x": 2158000,
                        "y": 18
                    },
                    {
                        "x": 2158000,
                        "y": 19
                    },
                    {
                        "x": 2158000,
                        "y": 20
                    },
                    {
                        "x": 2158000,
                        "y": 21
                    },
                    {
                        "x": 2158000,
                        "y": 22
                    }
                ],
                "_colorIndex": 2,
                "_symbolIndex": 2
            },
            {
                "name": "KILL",
                "data": [
                    {
                        "x": 807000,
                        "y": 1
                    },
                    {
                        "x": 936000,
                        "y": 2
                    },
                    {
                        "x": 1046000,
                        "y": 3
                    },
                    {
                        "x": 1111000,
                        "y": 4
                    },
                    {
                        "x": 1503000,
                        "y": 5
                    },
                    {
                        "x": 1548000,
                        "y": 6
                    }
                ],
                "_colorIndex": 3,
                "_symbolIndex": 3
            },
            {
                "name": "LAND",
                "data": [
                    {
                        "x": 2332000,
                        "y": 1
                    }
                ],
                "_colorIndex": 4,
                "_symbolIndex": 4
            },
        ],

        exporting: { enabled: false },
        credits: {
            enabled: false
        },

    });




    Highcharts.chart('hc-last-5-sorties', {
        chart: {
            type: 'bar',
            //backgroundColor: null
        },
        title: {
            text: 'Last 5 Sorties'
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
        series: [
            {
                "name": "KILL",
                "data": [
                    0,
                    4,
                    2,
                    0,
                    6
                ],
            },
            {
                "name": "SHOTS FIRED",
                "data": [
                    1,
                    9,
                    2,
                    4,
                    4
                ],
            },
            {
                "name": "HITS",
                "data": [
                    1,
                    5,
                    3,
                    0,
                    8
                ],
            }
        ]
    });




    Highcharts.chart('hc-kills-by-type-bar', {
        chart: {
            type: 'column',
        },
        title: {
            text: 'Kills by Type'
        },
        subtitle: {
            text: 'Click the columns to drilldown'
        },
        legend: {
            enabled: false
        },
        xAxis: {
            type: 'category'
        },
        yAxis: {
            title: {
                text: 'Kills',
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
        
        credits: {
            enabled: false
        },
        exporting: { enabled: false },

        series: [
            {
                "name": "Forces",
                "colorByPoint": true,
                "data": [
                    {
                        "name": "Ground",
                        "y": 30,
                        "drilldown": "Ground"
                    },
                    {
                        "name": "Air",
                        "y": 3,
                        "drilldown": "Air"
                    },
                    {
                        "name": "Helicopter",
                        "y": 3,
                        "drilldown": "Helicopter"
                    },
                    {
                        "name": "Ship",
                        "y": 1,
                        "drilldown": "Ship"
                    }
                ],
            }
        ],

        drilldown: {
            activeAxisLabelStyle: {
                textDecoration: 'none',
                fontStyle: 'italic',
                color: '#f1f1f1',
                fill: '#f1f1f1'
            },
            activeDataLabelStyle: {
                textDecoration: 'none',
                fontStyle: 'italic',
                color: '#f1f1f1',
                fill: '#f1f1f1'
            },
            series: [
                {
                    name: "Ground",
                    id: "Ground",
                    data: [
                        {
                            name: "Vehicle",
                            y: 10,
                            drilldown: "Vehicle"
                        },
                        {
                            name: "Air Defense",
                            y: 5,
                            drilldown: "AirDefense"
                        },
                        {
                            name: "Infantry",
                            y: 10,
                            drilldown: "Infantry"
                        },                   
                        {
                            name: "Artillery",
                            y: 5,
                            drilldown: "Artillery"
                        },                             
                    ]
                },

                // drilldowns
                {
                    id: "Vehicle",
                    data: [
                        ["TANK", 5],
                        ["APC", 2],
                        ["IFV", 1],
                        ["CAR", 1],
                        ["TRUCK", 1]
                    ]
                },
                {
                    id: "AirDefense",
                    data: [
                        ["EWR", 1],
                        ["AAA", 1],
                        ["SAM", 2],
                        ["SAM_CC", 1],
                        ["SAM_RADAR", 0]
                    ]
                },
                {
                    id: "Infantry",
                    data: [
                        ["Infantry", 7],
                        ["MANPADS", 3]
                    ]
                },
                {
                    id: "Artillery",
                    data: [
                        ["ARTILLERY", 3],
                        ["MRLS", 2]
                    ]
                },




                {
                    "name": "Air",
                    "id": "Air",
                    "data": [
                        ["Fighter", 1],
                        ["Striker", 1],
                        ["Bomber", 0],
                        ["Transport", 1],
                    ]
                },
                {
                    "name": "Helicopter",
                    "id": "Helicopter",
                    "data": [
                        ["Transport", 2],
                        ["Attack", 1]
                    ]
                },
                {
                    "name": "Ship",
                    "id": "Ship",
                    "data": [
                        ["Carrier", 0],
                        ["Vessel", 1]
                    ]
                }
            ]
        }
    });


    Highcharts.chart('hc-deaths-by-type-bar', {
        chart: {
            type: 'column',
        },
        title: {
            text: 'Deaths by Type'
        },
        subtitle: {
            text: 'Click the columns to drilldown'
        },
        legend: {
            enabled: false
        },
        xAxis: {
            type: 'category'
        },
        yAxis: {
            title: {
                text: 'Deaths',
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
        
        credits: {
            enabled: false
        },
        exporting: { enabled: false },

        series: [
            {
                "name": "Forces",
                "colorByPoint": true,
                "data": [
                    {
                        "name": "Ground",
                        "y": 5,
                        "drilldown": "Ground"
                    },
                    {
                        "name": "Air",
                        "y": 3,
                        "drilldown": "Air"
                    },
                    {
                        "name": "Helicopter",
                        "y": 2,
                        "drilldown": "Helicopter"
                    },
                    {
                        "name": "Ship",
                        "y": 1,
                        "drilldown": "Ship"
                    }
                ],
            }
        ],

        drilldown: {
            activeAxisLabelStyle: {
                textDecoration: 'none',
                fontStyle: 'italic',
                color: '#f1f1f1',
                fill: '#f1f1f1'
            },
            activeDataLabelStyle: {
                textDecoration: 'none',
                fontStyle: 'italic',
                color: '#f1f1f1',
                fill: '#f1f1f1'
            },
            series: [
                {
                    name: "Ground",
                    id: "Ground",
                    data: [
                        {
                            name: "Vehicle",
                            y: 1,
                            drilldown: "Vehicle"
                        },
                        {
                            name: "Air Defense",
                            y: 4,
                            drilldown: "AirDefense"
                        },
                        {
                            name: "Infantry",
                            y: 1,
                            drilldown: "Infantry"
                        },                   
                        {
                            name: "Artillery",
                            y: 0,
                            drilldown: "Artillery"
                        },                             
                    ]
                },

                // drilldowns
                {
                    id: "Vehicle",
                    data: [
                        ["TANK", 0],
                        ["APC", 0],
                        ["IFV", 1],
                        ["CAR", 0],
                        ["TRUCK", 0]
                    ]
                },
                {
                    id: "AirDefense",
                    data: [
                        ["EWR", 0],
                        ["AAA", 2],
                        ["SAM", 2],
                        ["SAM_CC", 0],
                        ["SAM_RADAR", 0]
                    ]
                },
                {
                    id: "Infantry",
                    data: [
                        ["Infantry", 0],
                        ["MANPADS", 1]
                    ]
                },
                {
                    id: "Artillery",
                    data: [
                        ["ARTILLERY", 0],
                        ["MRLS", 0]
                    ]
                },




                {
                    "name": "Air",
                    "id": "Air",
                    "data": [
                        ["Fighter", 1],
                        ["Striker", 1],
                        ["Bomber", 0],
                        ["Transport", 0],
                    ]
                },
                {
                    "name": "Helicopter",
                    "id": "Helicopter",
                    "data": [
                        ["Transport", 0],
                        ["Attack", 2]
                    ]
                },
                {
                    "name": "Ship",
                    "id": "Ship",
                    "data": [
                        ["Carrier", 1],
                        ["Vessel", 0]
                    ]
                }
            ]
        }
    });

    Highcharts.chart('hc-kills-by-unit-top-10-bar', {
        chart: {
            type: 'column',
        },
        title: {
            text: 'Prey'
        },
        subtitle: {
            text: 'Top 10 Most Killed Units'
        },
        legend: {
            enabled: false
        },
        xAxis: {
            type: 'category'
        },
        yAxis: {
            title: {
                text: 'Kills',
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
        responsive: responsiveBarGraphs,
        credits: {
            enabled: false
        },
        exporting: { enabled: false },

        series: [
            {
                "name": "Kills",
                "colorByPoint": true,
                "data": [
                    {
                        "name": "M4 Soldier",
                        "y": 24,
                    },
                    {
                        "name": "BTR-80",
                        "y": 15,
                    },
                    {
                        "name": "AK Infantry",
                        "y": 10,
                    },
                    {
                        "name": "T-72",
                        "y": 8,
                    },
                    {
                        "name": "SA-11",
                        "y": 8,
                    },
                    {
                        "name": "Igla manpad INS",
                        "y": 5,
                    },
                    {
                        "name": "KAMAZ Truck",
                        "y": 4,
                    },
                    {
                        "name": "T-55",
                        "y": 3,
                    },
                    {
                        "name": "Soldier RPG",
                        "y": 1,
                    },
                    {
                        "name": "Paratrooper RPG-16",
                        "y": 1,
                    }
                ],
            }
        ],
    });

    Highcharts.chart('hc-deaths-by-unit-top-10-bar', {
        chart: {
            type: 'column',
        },
        title: {
            text: 'Threats'
        },
        subtitle: {
            text: 'Top 10 Most Deaths By'
        },
        legend: {
            enabled: false
        },
        xAxis: {
            type: 'category'
        },
        yAxis: {
            title: {
                text: 'Deaths',
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
        responsive: responsiveBarGraphs,
        credits: {
            enabled: false
        },
        exporting: { enabled: false },

        series: [
            {
                "name": "Deaths",
                "colorByPoint": true,
                "data": [
                    {
                        "name": "SA-18 Igla-S manpad",
                        "y": 12,
                    },
                    {
                        "name": "Mig-29S",
                        "y": 9,
                    },
                    {
                        "name": "SA-11",
                        "y": 9,
                    },
                    {
                        "name": "SA-8 Osa",
                        "y": 8,
                    },
                    {
                        "name": "SA-10 Tor",
                        "y": 7,
                    },
                    {
                        "name": "Igla manpad INS",
                        "y": 5,
                    },
                    {
                        "name": "SA-6",
                        "y": 4,
                    },
                    {
                        "name": "Afghanski",
                        "y": 3,
                    },
                    {
                        "name": "Soldier RPG",
                        "y": 1,
                    },
                    {
                        "name": "Paratrooper RPG-16",
                        "y": 0,
                    }
                ],
            }
        ],
    });



    Highcharts.chart('hc-missile-accuracy', {

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
            text: 'Missile',
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
            name: 'Accuracy',
            borderColor: Highcharts.getOptions().colors[3],
            data: [{
                color: Highcharts.getOptions().colors[3],
                radius: '75%',
                innerRadius: '75%',
                y: 60
            }]
        }],
    });

    Highcharts.chart('hc-rocket-accuracy', {

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
            text: 'Rockets',
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
                backgroundColor: Highcharts.Color(Highcharts.getOptions().colors[4])
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
            name: 'Accuracy',
            borderColor: Highcharts.getOptions().colors[4],
            data: [{
                color: Highcharts.getOptions().colors[4],
                radius: '75%',
                innerRadius: '75%',
                y: 10
            }]
        }],
    });

    Highcharts.chart('hc-bomb-accuracy', {

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
            text: 'Bomb',
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
            },
        },

        exporting: { enabled: false },

        credits: {
            enabled: false
        },

        responsive: responsiveGauges,

        series: [{
            name: 'Accuracy',
            borderColor: Highcharts.getOptions().colors[2],
            data: [{
                color: Highcharts.getOptions().colors[2],
                radius: '75%',
                innerRadius: '75%',
                y: 8
            }]
        }],
    });

    Highcharts.chart('hc-gun-accuracy', {

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
            text: 'Gun',
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
                backgroundColor: Highcharts.Color(Highcharts.getOptions().colors[5])
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
            name: 'Accuracy',
            borderColor: Highcharts.getOptions().colors[5],
            data: [{
                color: Highcharts.getOptions().colors[5],
                radius: '75%',
                innerRadius: '75%',
                y: 17
            }]
        }],
    });

    gaugesTooltipRender('#hc-missile-accuracy');
    gaugesTooltipRender('#hc-rocket-accuracy');
    gaugesTooltipRender('#hc-bomb-accuracy');
    gaugesTooltipRender('#hc-gun-accuracy');


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
        $('#hc-missile-accuracy, #hc-rocket-accuracy, #hc-bomb-accuracy, #hc-gun-accuracy').each(function() {
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