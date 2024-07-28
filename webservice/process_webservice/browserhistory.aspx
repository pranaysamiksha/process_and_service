<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="browserhistory.aspx.cs" Inherits="process_webservice.browserhistory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .gridheader {
            background: #008cff;
            padding: 4px;
            color: white;
        }
    </style>
    <link rel="stylesheet" href="https://rawgit.com/LeshikJanz/libraries/master/Bootstrap/baguetteBox.min.css">
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
                            <li class="breadcrumb-item active" aria-current="page">Browser History</li>
                        </ol>
                    </nav>
                </div>
                <div class="ms-auto">
                </div>
            </div>


            <div class="row">
                <div class="col-xl-12 mx-auto">

                    <div class="card">
                        <div class="card-body p-4">
                            <h5 class="mb-4">Browser History</h5>


                            <div class="row g-3">
                                <div class="col-md-6">
                                    <label for="input1" class="form-label">Date</label>

                                    <asp:TextBox ID="txtFromDate" runat="server" autocomplete="off" CssClass="form-control" placeholder="Date"></asp:TextBox>

                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtFromDate"
                                        ErrorMessage="Please Enter Date" Display="None" ValidationGroup="company_master"></asp:RequiredFieldValidator>


                                </div>

                                <%-- <div class="col-md-6">
                                    <label for="input1" class="form-label">To Date</label>
                                    <asp:TextBox ID="txtToDate" runat="server" class="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtToDate"
                                        ErrorMessage="Please Enter To Date" Display="None" ValidationGroup="company_master"></asp:RequiredFieldValidator>

                                </div>--%>
                                <div class="col-md-6">
                                    <label for="input1" class="form-label">Select User</label>
                                    <asp:DropDownList ID="ddladmin_login_id" runat="server" CssClass="form-control">
                                    </asp:DropDownList>


                                </div>

                                <div class="col-md-12">
                                    <div class="d-md-flex d-grid align-items-center gap-3">
                                        <asp:Button ID="btnsubmit" ValidationGroup="company_master" runat="server" class="btn btn-primary px-4" Text="Submit" OnClick="btnsubmit_Click" />
                                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="company_master"
                                            ShowMessageBox="true" ShowSummary="false" />

                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </div>

            <!--end breadcrumb-->

            <h6 class="mb-0 text-uppercase">DataTable Import</h6>
            <hr />
            <div class="card">
                <div class="table-responsive">
                    <asp:DataGrid ID="dgCountries" GridLines="Vertical" BorderColor="lightgray" Width="100%"
                        ItemStyle-ForeColor="Black" ShowFooter="false" runat="server" Style="border-collapse: collapse;"
                        PageSize="100" AutoGenerateColumns="false" AllowPaging="true"
                        OnPageIndexChanged="dgCountries_PageIndexChanged">
                        <ItemStyle BorderColor="lightgray" BorderWidth="1px" />
                        <PagerStyle Mode="NumericPages" Position="Bottom" HorizontalAlign="Left" />
                        <HeaderStyle BorderColor="lightgray" />
                        <FooterStyle BorderColor="lightgray" />
                        <Columns>

                            <asp:TemplateColumn>
                                <HeaderTemplate>
                                    <div style="padding: 0px 0px 0px 05px; text-align: left;">
                                        Browser Name

                                    </div>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <div style="padding: 0px 0px 0px 05px; text-align: left;">
                                        <%#Eval("browser_name")%>
                                    </div>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <div id="dvFooter" runat="server" style="padding: 5px;">
                                    </div>
                                </FooterTemplate>
                                <HeaderStyle CssClass="gridheader" />
                                <ItemStyle HorizontalAlign="Center" />
                                <FooterStyle HorizontalAlign="Center" />
                            </asp:TemplateColumn>

                            <asp:TemplateColumn>
                                <HeaderTemplate>
                                    <div style="padding: 0px 0px 0px 05px; text-align: left;">
                                        Title
                                    </div>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <div style="padding: 0px 0px 0px 05px; text-align: left;">
                                        <%#Eval("title")%>
                                    </div>
                                </ItemTemplate>
                                <HeaderStyle CssClass="gridheader" />
                                <ItemStyle HorizontalAlign="Center" />
                                <FooterStyle HorizontalAlign="Center" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderTemplate>
                                    <div style="padding: 0px 0px 0px 05px; text-align: left;">
                                        Last visit time
                                    </div>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <div style="padding: 0px 0px 0px 05px; text-align: left;">
                                        <%#Eval("last_visit_time")%>
                                    </div>
                                </ItemTemplate>
                                <HeaderStyle CssClass="gridheader" />
                                <ItemStyle HorizontalAlign="Center" />
                                <FooterStyle HorizontalAlign="Center" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderTemplate>
                                    <div style="padding: 0px 0px 0px 05px; text-align: left;">
                                        URL
                                    </div>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <div style="padding: 0px 0px 0px 05px; text-align: left;">
                                        <%#Eval("url")%>
                                    </div>
                                </ItemTemplate>
                                <HeaderStyle CssClass="gridheader" />
                                <ItemStyle HorizontalAlign="Center" Width="200px" />
                                <FooterStyle HorizontalAlign="Center" />
                            </asp:TemplateColumn>

                        </Columns>
                    </asp:DataGrid>
                </div>
            </div>
        </div>
    </div>

    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.2.1/jquery.min.js"></script>

    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.0.8/popper.min.js"></script>

    <script src="https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/4.0.0-beta/js/bootstrap.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/baguettebox.js/1.8.1/baguetteBox.min.js"></script>
    <script>
        baguetteBox.run('.tz-gallery');
    </script>

    <link rel="stylesheet" href="https://code.jquery.com/ui/1.13.3/themes/base/jquery-ui.css">
    <link rel="stylesheet" href="/resources/demos/style.css">

    <script src="https://code.jquery.com/ui/1.13.3/jquery-ui.js"></script>
    <script>
        $(function () {
            $("#ContentPlaceHolder1_txtFromDate").datepicker({ dateFormat: 'yy-mm-dd' });
        });
    </script>


    <!--end page wrapper -->
</asp:Content>

