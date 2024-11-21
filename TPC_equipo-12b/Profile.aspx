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
                        <asp:Button ID="btnEditar" runat="server" Text="Editar Perfil" CssClass="btn btn-warning" OnClick="btnEditarTraerDatos_Click"/>
                        <asp:Button ID="btnCerrarSesion" runat="server" Text="Cerrar Sesión" CssClass="btn btn-danger" OnClick="btnCerrarSesion_Click" />
                    </div>
                </div>
            </div>
        </div>
         <div class="container my-4">
        <h2 class="mb-4">Últimos 10 Partidos</h2>

        
        <%--MODAL EDITAR PERFIL--%>
        <div class="modal fade" id="updateModal" tabindex="-1" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="modalFormularioPartidoLabel">Editar Perfil</h5>
                        <button type="button" class="btn-close" aria-label="Close" onclick="closeUpdateModal()"></button>
                    </div>
                    <div class="modal-body">
                        <asp:Panel ID="panelEditPartido" runat="server" CssClass="wi mb-4">

                            <div class="mb-3">
                                <label for="txtNombre" class="form-label">Nombre:</label>
                                <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>

                            <div class="mb-3">
                                <label for="txtApellido" class="form-label">Apellido:</label>
                                <asp:TextBox ID="txtApellido" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>

                            <div class="mb-3">
                                <label for="txtUsername" class="form-label">Username:</label>
                                <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>

                            <div class="mb-3">
                                <label for="txtEmail" class="form-label">Email:</label>
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>

                        </asp:Panel>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" onclick="closeUpdateModal()">Cancelar</button>
                        <asp:Button ID="btnSaveEditPartido" runat="server" Text="Guardar Cambios" CssClass="btn btn-primary" OnClick="btnUpdatePerfil_Click" />
                    </div>
                </div>
            </div>
        </div>


        <%--LISTADO ULTIMOS 10 PARTIDOS--%>
        <asp:Repeater ID="PartidosRepeater" runat="server">
            <ItemTemplate>
                <div class="card mb-3">
                    <div class="card-body">
                        <div class="d-flex justify-content-between">
                            <h5 class="card-title">Liga: <%# Eval("NombreLiga") %> - <%# Convert.ToDateTime(Eval("Fecha")).ToString("dd/MM/yyyy") %></h5>
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

    <asp:HiddenField ID="hiddenMessage" runat="server" />
    <asp:HiddenField ID="hiddenMessageType" runat="server" />

    <script>
        function openUpdateModal() {
            var modal = document.getElementById("updateModal");
            modal.style.display = "block";
            modal.classList.add("show");
        }

        function closeUpdateModal() {
            var modal = document.getElementById("updateModal");
            modal.style.display = "none";
            modal.classList.remove("show");
        }

        window.onload = function () {
            var message = document.getElementById('<%= hiddenMessage.ClientID %>').value;
            if (message) {
                if (message.includes("correctamente")) {
                    showSuccessMessage(message);
                } else {
                    showErrorMessage(message);
                }
                document.getElementById('<%= hiddenMessage.ClientID %>').value = '';
            }
        };
    </script>
</asp:Content>