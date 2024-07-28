<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="manage_users.aspx.cs" Inherits="process_webservice.manage_users" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .gridheader {
            background: #008cff;
            padding: 4px;
            color: white;
        }
    </style>

    <script language="javascript" type="text/javascript">
        function ____add(Id, name, email, limit, DisplayType) {

            document.getElementById("<%=hfuser_masterID.ClientID %>").value = Id;
                document.getElementById("<%=txtname.ClientID %>").value = name;
                document.getElementById("<%=txtemail.ClientID %>").value = email;
                document.getElementById("<%=txtlimit.ClientID %>").value = limit;
        }

    </script>
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
                            <li class="breadcrumb-item active" aria-current="page">Manage Users</li>
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
                            <h5 class="mb-4">Manage Users</h5>


                            <div class="row g-3">
                                <div class="col-md-6">
                                    <label for="input1" class="form-label">Name</label>
                                    <asp:TextBox ID="txtname" runat="server" class="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtname"
                                        ErrorMessage="Please Enter Name" Display="None" ValidationGroup="company_master"></asp:RequiredFieldValidator>


                                </div>
                                <div class="col-md-6">
                                    <label for="input1" class="form-label">Email</label>
                                    <asp:TextBox ID="txtemail" runat="server" class="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtemail"
                                        ErrorMessage="Please Enter Email" Display="None" ValidationGroup="company_master"></asp:RequiredFieldValidator>

                                </div>
                                <div class="col-md-6">
                                    <label for="input1" class="form-label">Limit in ML Seconds</label>
                                    <asp:TextBox ID="txtlimit" runat="server" class="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtlimit"
                                        ErrorMessage="Please Enter Limit" Display="None" ValidationGroup="company_master"></asp:RequiredFieldValidator>


                                </div>
                                <div class="col-md-12">
                                    <div class="d-md-flex d-grid align-items-center gap-3">
                                        <asp:Button ID="btnsubmit" ValidationGroup="company_master" runat="server" class="btn btn-primary px-4" Text="Submit" OnClick="btnsubmit_Click" />
                                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="company_master"
                                            ShowMessageBox="true" ShowSummary="false" />
                                        <asp:HiddenField ID="hfuser_masterID" Value="0" runat="server" />
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
                <div class="card-body">
                    <div class="table-responsive">
                        <asp:DataGrid ID="dgCountries" GridLines="Vertical" BorderColor="lightgray" Width="100%"
                            ItemStyle-ForeColor="Black" ShowFooter="false" runat="server" Style="border-collapse: collapse;"
                            PageSize="10" AutoGenerateColumns="false" AllowPaging="true" OnItemDataBound="dgCountries_ItemDataBound"
                            OnPageIndexChanged="dgCountries_PageIndexChanged">
                            <ItemStyle BorderColor="lightgray" BorderWidth="1px" />
                            <PagerStyle Mode="NumericPages" Position="Bottom" HorizontalAlign="Left" />
                            <HeaderStyle BorderColor="lightgray" />
                            <FooterStyle BorderColor="lightgray" />
                            <Columns>
                                <asp:TemplateColumn>
                                    <HeaderTemplate>
                                        <div style="padding: 0px 0px 0px 05px; text-align: left;">
                                            Action
                                        </div>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div class="dvFL">
                                            <asp:HiddenField ID="hfuser_masterID" runat="server" Value='<%#Eval("user_masterID") %>' />
                                            <asp:HiddenField ID="hfname" runat="server" Value='<%#Eval("name") %>' />
                                            <asp:HiddenField ID="hfemail" runat="server" Value='<%#Eval("email") %>' />
                                            <asp:HiddenField ID="hflimit" runat="server" Value='<%#Eval("limit") %>' />
                                            <a id="aEdit" runat="server" style="color: Blue;"><i class="fa fa-pencil" style="font-size: 16px; color: gray;"></i></a><a id="ancDelete" runat="server" style="color: Red; color: #EC6767;">
                                                <i class="fa fa-trash-o" style="font-size: 16px;"></i></a>
                                        </div>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="gridheader" />
                                    <ItemStyle HorizontalAlign="Left" />
                                    <FooterStyle HorizontalAlign="Left" />
                                </asp:TemplateColumn>
                                <asp:TemplateColumn>
                                    <HeaderTemplate>
                                        <div style="padding: 0px 0px 0px 05px; text-align: left;">
                                            UserID
                                        </div>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div style="padding: 0px 0px 0px 05px; text-align: left;">
                                            <%#Eval("user_masterID")%>
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
                                            Name
                                        </div>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div style="padding: 0px 0px 0px 05px; text-align: left;">
                                            <%#Eval("name")%>
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
                                            Email
                                        </div>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div style="padding: 0px 0px 0px 05px; text-align: left;">
                                            <%#Eval("email")%>
                                        </div>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="gridheader" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    <FooterStyle HorizontalAlign="Center" />
                                </asp:TemplateColumn>
                                <asp:TemplateColumn>
                                    <HeaderTemplate>
                                        <div style="padding: 0px 0px 0px 05px; text-align: left;">
                                            Limit
                                        </div>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div style="padding: 0px 0px 0px 05px; text-align: left;">
                                            <%#Eval("limit")%>
                                        </div>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="gridheader" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    <FooterStyle HorizontalAlign="Center" />
                                </asp:TemplateColumn>


                            </Columns>
                        </asp:DataGrid>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.2.1/jquery.min.js"></script>
    <!--end page wrapper -->
</asp:Content>
