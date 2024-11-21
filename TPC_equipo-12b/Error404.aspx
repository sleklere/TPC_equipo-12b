<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="Error404.aspx.cs" Inherits="TPC_equipo_12b.Error404" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <div class="container d-flex justify-content-center align-items-center" style="min-height: calc(100vh - 60px);">
        <div class="row w-100 text-center">
            <div class="col-12">
                <h1 class="display-1 text-danger">404</h1>
                <h2 class="mb-4">¡Pagina no encontrada!</h2>
                <p class="lead">Lo sentimos, esta pagina no existe.</p>
                <a href="/Default.aspx" class="btn btn-primary btn-lg">Volver al inicio</a>
            </div>
        </div>
    </div>
</asp:Content>
