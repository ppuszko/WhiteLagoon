
$(document).ready(function () {
    loadCustomerAndBookingLineChart();
});

function loadCustomerAndBookingLineChart() {
    $(".chart-spinner").show();

    $.ajax({
        url: "/Dashboard/GetMemberAndBookingLineChartData",
        type: 'GET',
        dataType: 'json',
        success: function (data) {

            loadLineChart("newMembersAndBookingsLineChart", data)

            

            $(".chart-spinner").hide();
        }
    })
}

function loadLineChart(id, data) {
    var chartColors = getChartColorsArray(id);

    var options = {
        series: data.series,
        colors: chartColors,
        chart: {
            type: 'line',
            height: 350
        },
        stroke: {
            curve: 'smooth',
            width: 2
        },
        markers: {
            size: 4,
            strokeWidth: 0,
            hover: {
                size: 7
            }
        },
        xaxis: {
            categories: data.categories,
            labels: {
                style: {
                    colors: "#ddd"
                }
            }
        },
        yaxis: {
            labels: {
                style: {
                    colors: "#fff"
                },
            }
        },
        Legend: {
            labels: {
                colors: "#fff"
            },
        }
    };

    var chart = new ApexCharts(document.querySelector("#" + id), options);
    chart.render();
}