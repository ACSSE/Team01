<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/master.Master" CodeBehind="stats.aspx.cs" Inherits="IceBreak.stats" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>IceBreak | Statistics</title>
    <script>
        getUsersIcebreakCount = function(data)
        {
            //if (data != null)
                alert(data);
        }

        $(document).ready(function ()
        {
            /*$.ajax({
                type: "GET",
                async:false,
                url: "http://icebreak.azurewebsites.net/IBUserRequestService.svc/getUsersIcebreakCount",
                contentType: "text/plain; charset=utf-8",//"application/json; charset=utf-8",
                //data: data,
                dataType: "jsonp",
                jsonpCallback: "getUsersIcebreakCount",
                crossDomain: true,
                success: function (data)
                {
                    alert("S:"+data);
                }
            });*/
            var ctx = document.getElementById("myChart");
            //document.writeln(ctx == null)
            var myChart = new Chart(ctx, {
                type: 'line',
                data: {
                    labels: ["Tevin Moodley", "Aaron Sekati", "Austin Naidoo", "Casper Ndlovu", "Bruce Wayne", "Jarrod Hill"],
                    datasets: [{
                        label: 'Number of IceBreak Requests Made',
                        data: [12, 19, 3, 5, 2, 7],
                        backgroundColor: [
                            'rgba(255, 99, 132, 0.2)',
                            'rgba(54, 162, 235, 0.2)',
                            'rgba(255, 206, 86, 0.2)',
                            'rgba(75, 192, 192, 0.2)',
                            'rgba(153, 102, 255, 0.2)',
                            'rgba(255, 159, 64, 0.2)'
                        ],
                        borderColor: [
                            'rgba(255,99,132,1)',
                            'rgba(54, 162, 235, 1)',
                            'rgba(255, 206, 86, 1)',
                            'rgba(75, 192, 192, 1)',
                            'rgba(153, 102, 255, 1)',
                            'rgba(255, 159, 64, 1)'
                        ],
                        borderWidth: 1
                    }]
                },
                options: {
                    scales: {
                        yAxes: [{
                            ticks: {
                                beginAtZero: true
                            }
                        }]
                    }
                }
            });
            //
            ctx = document.getElementById("myChart2");
            var myChart2 = new Chart(ctx, {
                type: 'bar',
                data: {
                    labels: ["Tevin Moodley", "Aaron Sekati", "Austin Naidoo", "Casper Ndlovu", "Bruce Wayne", "Jarrod Hill"],
                    datasets: [{
                        label: 'Number of IceBreak Events Attended',
                        data: [15, 19, 30, 15, 12, 7],
                        backgroundColor: [
                            'rgba(255, 99, 132, 0.2)',
                            'rgba(54, 162, 235, 0.2)',
                            'rgba(255, 206, 86, 0.2)',
                            'rgba(75, 192, 192, 0.2)',
                            'rgba(153, 102, 255, 0.2)',
                            'rgba(255, 159, 64, 0.2)'
                        ],
                        borderColor: [
                            'rgba(255,99,132,1)',
                            'rgba(54, 162, 235, 1)',
                            'rgba(255, 206, 86, 1)',
                            'rgba(75, 192, 192, 1)',
                            'rgba(153, 102, 255, 1)',
                            'rgba(255, 159, 64, 1)'
                        ],
                        borderWidth: 1
                    }]
                },
                options: {
                    scales: {
                        yAxes: [{
                            ticks: {
                                beginAtZero: true
                            }
                        }]
                    }
                }
            });
            //
            ctx = document.getElementById("myChart3");
            var myChart3 = new Chart(ctx, {
            type: 'line',
            data: {
                labels: ["Tevin Moodley", "Aaron Sekati", "Austin Naidoo", "Casper Ndlovu", "Bruce Wayne", "Jarrod Hill"],
                datasets: [{
                    label: 'Number of IceBreak Requests Accepted.',
                    data: [90, 70, 30, 50, 20, 111],
                    backgroundColor: [
                        'rgba(255, 99, 132, 0.2)',
                        'rgba(54, 162, 235, 0.2)',
                        'rgba(255, 206, 86, 0.2)',
                        'rgba(75, 192, 192, 0.2)',
                        'rgba(153, 102, 255, 0.2)',
                        'rgba(255, 159, 64, 0.2)'
                    ],
                    borderColor: [
                        'rgba(255,99,132,1)',
                        'rgba(54, 162, 235, 1)',
                        'rgba(255, 206, 86, 1)',
                        'rgba(75, 192, 192, 1)',
                        'rgba(153, 102, 255, 1)',
                        'rgba(255, 159, 64, 1)'
                    ],
                    borderWidth: 1
                }]
            },
            options: {
                scales: {
                        yAxes: [{
                            ticks: {
                                beginAtZero: true
                            }
                        }]
                }
            }
        });
        });
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="width:500px;height:500px;margin:auto;">
        <canvas id="myChart" width="600" height="400"></canvas>
    </div>
    <div style="width:500px;height:500px;margin:auto;">
        <canvas id="myChart2" width="600" height="400"></canvas>
    </div>
    <div style="width:500px;height:500px;margin:auto;">
        <canvas id="myChart3" width="600" height="400"></canvas>
    </div>
</asp:Content>