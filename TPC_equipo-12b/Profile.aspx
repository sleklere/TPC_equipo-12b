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
         <div class="container my-4">
        <h2 class="mb-4">Últimos 10 Partidos</h2>
        
        <asp:Repeater ID="PartidosRepeater" runat="server">
            <ItemTemplate>
                <div class="card mb-3">
                    <div class="card-body">
                        <div class="d-flex justify-content-between">
                            <h5 class="card-title">Partido - <%# Eval("Fecha") %></h5>
                            <asp:Label runat="server" 
                                Text='<%# Eval("EsGanador").ToString() == "True" ? "Ganado" : "Perdido" %>' Style='<%# Eval("EsGanador").ToString() == "True" ? "display:flex;align-items:center;padding-left:5px;padding-right:5px;color:white;background-color:green;border-radius:6px;font-weight:600;" : "display:flex;align-items:center;padding-left:5px;padding-right:5px;color:white;background-color:red;border-radius:6px;font-weight:600;" %>'>
                            </asp:Label>
                        </div>
                        <p class="card-text">
                            <strong>Jugador 1:</strong> <%# Eval("NombreJugador1") %> <%# Eval("ApellidoJugador1") %> - 
                            <%# Eval("PuntosJugador1") %> puntos
                        </p>
                        <p class="card-text">
                            <strong>Jugador 2:</strong> <%# Eval("NombreJugador2") %> <%# Eval("ApellidoJugador2") %> - 
                            <%# Eval("PuntosJugador2") %> puntos
                        </p>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
    </div>
</asp:Content>