<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="LigaDetalle.aspx.cs" Inherits="TPC_equipo_12b.LigaDetalle" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>Detalles de la Liga</h1>
    <h3>Jugadores</h3>
    <div class="table-responsive">
        <table class="table table-striped">
            <thead>
                <tr>
                    <th scope="col">#</th>
                    <th scope="col">Nombre</th>
                    <th scope="col">Apellido</th>
                    <th scope="col">Username</th>
                </tr>
            </thead>
            <tbody>
                <% 
                    int index = 1;
                    foreach (Dominio.Jugador jugador in ListaJugadores)
                    {
                %>
                <tr>
                    <th scope="row"><%: index++ %></th>
                    <td><%: jugador.Nombre %></td>
                    <td><%: jugador.Apellido %></td>
                    <td><%: jugador.Username %></td>
                </tr>
                <% } %>
            </tbody>
        </table>
    </div>
</asp:Content>
