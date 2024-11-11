<%@ Page Title="Perfil" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="TPC_equipo_12b.Profile" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-5">
        <div class="row justify-content-center">
            <div class="col-md-6">
                <div class="card shadow-sm">
                    <div class="card-header bg-primary text-white text-center">
                        <h3>Perfil de Usuario</h3>
                    </div>
                    <div class="card-body">
                        <% 
                            var jugador = Session["Jugador"] as Dominio.Jugador; 
                            if (jugador != null) 
                            { 
                        %>
                            <p><strong>Nombre:</strong> <%= jugador.Nombre %></p>
                            <p><strong>Apellido:</strong> <%= jugador.Apellido %></p>
                            <p><strong>Nombre de Usuario:</strong> <%= jugador.Username %></p>
                            <p><strong>Email:</strong> <%= jugador.Email %></p>
                        <% 
                            } 
                            else 
                            { 
                        %>
                            <p class="text-danger">No se encontró información del usuario.</p>
                        <% } %>
                    </div>
                    <div class="card-footer d-flex justify-content-between">
                        <asp:Button ID="btnEditar" runat="server" Text="Editar Perfil" CssClass="btn btn-warning" OnClick="btnEditar_Click" />
                        <asp:Button ID="btnCerrarSesion" runat="server" Text="Cerrar Sesión" CssClass="btn btn-danger" OnClick="btnCerrarSesion_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>