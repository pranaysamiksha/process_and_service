<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="dashborad.aspx.cs" Inherits="process_webservice.dashborad" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!--start page wrapper -->
    <div class="page-wrapper">
        <div class="page-content">
            <!--breadcrumb-->
            <div class="page-breadcrumb d-none d-sm-flex align-items-center mb-3">
                <div class="breadcrumb-title pe-3"></div>
                <div class="ps-3">
                    <nav aria-label="breadcrumb">
                        <ol class="breadcrumb mb-0 p-0">
                            <li class="breadcrumb-item"><a href="javascript:;"><i class="bx bx-home-alt"></i></a>
                            </li>
                            <li class="breadcrumb-item active" aria-current="page">Dashboard</li>
                        </ol>
                    </nav>
                </div>
                <div class="ms-auto">
                </div>
            </div>


            <div class="row">

                <div class="col-12 col-lg-12 d-flex">

                    <div class="card radius-10 w-100">

                        <div class="card-header">

                            <div class="d-flex align-items-center">

                                <div>

                                    <h6 class="mb-0">Total Employees working Hours</h6>

                                </div>

                                <div class="dropdown ms-auto">

                                    <a class="dropdown-toggle dropdown-toggle-nocaret" href="#" data-bs-toggle="dropdown"><i class='bx bx-dots-horizontal-rounded font-22 text-option'></i>

                                    </a>



                                </div>

                            </div>

                        </div>

                        <div class="card-body">

                            <div class="d-flex align-items-center ms-auto font-13 gap-2 mb-3">

                                <span class="border px-1 rounded cursor-pointer"><i class="bx bxs-circle me-1" style="color: #14abef"></i>Total Employees working Hours</span>

                                <%--                                <span class="border px-1 rounded cursor-pointer"><i class="bx bxs-circle me-1" style="color: #ffc107"></i>Principal Approved Student</span>

                                <span class="border px-1 rounded cursor-pointer"><i class="bx bxs-circle me-1" style="color: green"></i>Admin Approved Student</span>--%>
                            </div>

                            <div class="chart-container-1">

                                <canvas id="chart1"></canvas>

                            </div>

                        </div>

                       <!-- <div class="row row-cols-1 row-cols-md-3 row-cols-xl-3 g-0 row-group text-center border-top">

                            <div class="col">

                                <div class="p-3">

                                    <h5 class="mb-0">100</h5>

                                    <small class="mb-0">Total Users<span> <i class="bx bx-up-arrow-alt align-middle"></i></span></small>

                                </div>

                            </div>

                            <div class="col">

                                <div class="p-3">

                                    <h5 class="mb-0">100</h5>

                                    <small class="mb-0">Total Working Hours <span><i class="bx bx-up-arrow-alt align-middle"></i></span></small>

                                </div>

                            </div>

                            <div class="col">

                                <div class="p-3">

                                    <h5 class="mb-0">100</h5>

                                    <small class="mb-0">Total Admin Approved<span> <i class="bx bx-up-arrow-alt align-middle"></i></span></small>

                                </div>

                            </div>

                        </div> -->
                    </div>

                </div>
            </div>

        </div>
    </div>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.2.1/jquery.min.js"></script>

    <script>


        $(function () {
            "use strict";
            var myarray_calss = [];
            var classname = "<%=strusername %>";
            var array = classname.split(",");
            $.each(array, function (i) {
                if (array[i] != "") {
                    myarray_calss.push(array[i]);
                }
            });

            var myarray_taotalaap = [];

            var taotalaaplication = "<%=totalwork%>";
            var array1 = taotalaaplication.split(",");

            $.each(
                array1, function (i) {
                    if (array1[i] != "") {
                        myarray_taotalaap.push(array1[i]);
                    }
                });


            // chart 1

            var ctx = document.getElementById("chart1").getContext('2d');

            var gradientStroke1 = ctx.createLinearGradient(0, 0, 0, 300);
            gradientStroke1.addColorStop(0, '#6078ea');
            gradientStroke1.addColorStop(1, '#17c5ea');



            var myChart = new Chart(ctx, {
                type: 'bar',
                data: {
                    labels: myarray_calss,
                    datasets: [{
                        label: 'Total Employees working Hours',
                        data: myarray_taotalaap,
                        borderColor: gradientStroke1,
                        backgroundColor: gradientStroke1,
                        hoverBackgroundColor: gradientStroke1,
                        pointRadius: 0,
                        fill: false,
                        borderRadius: 20,
                        borderWidth: 0
                    }]
                },

                options: {
                    maintainAspectRatio: false,
                    barPercentage: 0.5,
                    categoryPercentage: 0.8,
                    plugins: {
                        legend: {
                            display: false,
                        }
                    },
                    scales: {
                        y: {
                            beginAtZero: true
                        }
                    }
                }
            });







        });



    </script>

    <!--end page wrapper -->
</asp:Content>
