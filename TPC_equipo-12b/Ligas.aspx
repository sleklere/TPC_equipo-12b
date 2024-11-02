<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="Ligas.aspx.cs" Inherits="TPC_equipo_12b.Ligas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <h1>LIGAS</h1>
    <% 
        foreach (Dominio.Liga liga in ListaLigas)
        {
    %>
    <div class="card w-50">
        <div class="card-body">
            <h5 class="card-title"><%:liga.Nombre%></h5>
            <p class="card-text">X jugadores</p>
            <a href="<%: "LigaDetalle.aspx?id=" + liga.Id %>" class="btn btn-primary">Ver</a>
            <%-- <a href="#" class="btn btn-primary">Ver</a>--%>
        </div>
    </div>
    <% } %>
</asp:Content>
