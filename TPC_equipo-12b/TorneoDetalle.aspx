<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="TorneoDetalle.aspx.cs" Inherits="TPC_equipo_12b.TorneoDetalle" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <div class="d-flex justify-content-start gap-4 align-items-center m-4">
        <h1 class="display-6 text-primary fw-bold text-uppercase pb-2 mb-0"><%= TorneoData.Nombre %></h1>
    </div>

    <%--LISTAR TODOS LOS PARTIDOS POR RONDA--%>
    <%--    <div class="mx-3 my-5">
        <div class="d-flex justify-content-between align-items-center m-4">
            <h3>Partidos</h3>
            <asp:Button ID="btnAbrirModal" runat="server" Text="Crear Partido" CssClass="btn btn-primary" OnClientClick="openCreateModal(); return false;" />
        </div>
        <div class="row row-cols-1 row-cols-md-3 g-2">
            <asp:Repeater ID="rptPartidos" runat="server">
                <ItemTemplate>
                    <div class="col">
                        <div class="card" style="width: 100%;">
                            <div class="card-body">
                                <h5 class="card-title">Liga: <%# Eval("NombreLiga") %></h5>
                                <div class="d-flex align-items-center justify-content-between" style="width: 100%;">
                                    <span style='<%# Convert.ToInt32(Eval("Jugador1Id")) == Convert.ToInt32(Eval("GanadorId")) ? "color: green;": "color: red;" %>'>Jugador 1: <%# Eval("Jugador1Nombre") %></span>
                                    <span style='<%# Convert.ToInt32(Eval("Jugador1Id")) == Convert.ToInt32(Eval("GanadorId")) ? "color: green;": "color: red;" %>'><%# Eval("PuntosJugador1") %></span>
                                </div>
                                <div class="d-flex align-items-center justify-content-between" style="width: 100%;">
                                    <span style='<%# Convert.ToInt32(Eval("Jugador2Id")) == Convert.ToInt32(Eval("GanadorId")) ? "color: green;": "color: red;" %>'>Jugador 2: <%# Eval("Jugador2Nombre") %></span>
                                    <span style='<%# Convert.ToInt32(Eval("Jugador2Id")) == Convert.ToInt32(Eval("GanadorId")) ? "color: green;": "color: red;" %>'><%# Eval("PuntosJugador2") %></span>
                                </div>
                                <div class="d-flex justify-content-end mt-3 gap-2">
                                    <button type="button" class="btn btn-secondary" onclick="setPartidoIdAndOpenEditModal('<%# Eval("Id") %>', '<%# Eval("Jugador1Id") %>', '<%# Eval("Jugador2Id") %>', '<%# Eval("Jugador1Nombre") %>', '<%# Eval("Jugador2Nombre") %>', '<%# Eval("PuntosJugador1") %>', '<%# Eval("PuntosJugador2") %>','<%# Eval("GanadorId") %>', '<%# Eval("TipoPartidoId") %>')">Editar</button>
                                    <asp:Button ID="Button3" runat="server" Text="Eliminar" CssClass="btn btn-danger" OnClientClick="setPartidoIdAndOpenModal(this, 'delete'); return false;" data-ligaid='<%# Eval("Id") %>' />
                                </div>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>--%>


    <div class="container">
        <asp:Repeater ID="rptRondas" runat="server" OnItemDataBound="rptRondas_ItemDataBound">
            <ItemTemplate>
                <div class="mb-4">
                    <h4 class="text-primary">Ronda: <%# Eval("Ronda.Nombre") %></h4>
                    <div class="row row-cols-1 row-cols-md-3 g-2">
                        <asp:Repeater ID="rptPartidos" runat="server">
                            <ItemTemplate>
                                <div class="col">
                                    <div class="card" style="width: 100%;">
                                        <div class="card-body">
                                            <div class="d-flex align-items-center justify-content-between" style="width: 100%;">
                                                <span style='<%# Convert.ToInt32(Eval("Jugador1Id")) == Convert.ToInt32(Eval("GanadorId")) ? "color: green;": "color: black;" %>'>Jugador 1: <%# Eval("NombreJugador1") %></span>
                                                <span style='<%# Convert.ToInt32(Eval("Jugador1Id")) == Convert.ToInt32(Eval("GanadorId")) ? "color: green;": "color: black;" %>'><%# Eval("PuntosJugador1") %></span>
                                            </div>
                                            <div class="d-flex align-items-center justify-content-between" style="width: 100%;">
                                                <span style='<%# Convert.ToInt32(Eval("Jugador2Id")) == Convert.ToInt32(Eval("GanadorId")) ? "color: green;": "color: black;" %>'>Jugador 2: <%# Eval("NombreJugador2") %></span>
                                                <span style='<%# Convert.ToInt32(Eval("Jugador2Id")) == Convert.ToInt32(Eval("GanadorId")) ? "color: green;": "color: black;" %>'><%# Eval("PuntosJugador2") %></span>
                                            </div>
                                            <div class="d-flex justify-content-end mt-3 gap-2">
                                                <button type="button" class="btn btn-secondary"
                                                    onclick="setPartidoIdAndOpenEditModal('<%# Eval("PartidoId") %>', '<%# Eval("Jugador1Id") %>', '<%# Eval("Jugador2Id") %>', 
                                                    '<%# Eval("NombreJugador1") %>', '<%# Eval("NombreJugador2") %>', '<%# Eval("PuntosJugador1") %>', '<%# Eval("PuntosJugador2") %>',
                                                    '<%# Eval("GanadorId") %>', '<%# Eval("TipoPartidoId") %>')">
                                                    Editar</button>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
    <asp:Button ID="btnNextRound" runat="server" Text="Avanzar de ronda" CssClass="btn btn-primary" OnClick="btnNextRound_Click" />



    <%--MODAL EDITAR PARTIDO--%>
    <div class="modal fade" id="updateModal" tabindex="-1" aria-labelledby="modalFormularioPartidoLabel" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="modalFormularioPartidoLabel">Editar Partido</h5>
                    <button type="button" class="btn-close" aria-label="Close" onclick="closeUpdateModal()"></button>
                </div>
                <div class="modal-body">
                    <asp:Panel ID="panelEditPartido" runat="server" CssClass="wi mb-4">
                        <asp:HiddenField ID="hfEditPartidoId" runat="server" />

                        <div class="mb-3">
                            <label for="txtPuntosJugador1" class="form-label">Puntos de Jugador 1 (<span id="nombreJugador1Update"></span>):</label>
                            <asp:TextBox ID="txtEditPuntosJugador1" runat="server" CssClass="form-control" placeholder="Puntos de Jugador 1"></asp:TextBox>
                        </div>

                        <div class="mb-3">
                            <label for="txtPuntosJugador2" class="form-label">Puntos de Jugador 2 (<span id="nombreJugador2Update"></span>):</label>
                            <asp:TextBox ID="txtEditPuntosJugador2" runat="server" CssClass="form-control" placeholder="Puntos de Jugador 2"></asp:TextBox>
                        </div>

                        <asp:HiddenField ID="hiddenUpdateJ1Id" runat="server" />
                        <asp:HiddenField ID="hiddenUpdateJ2Id" runat="server" />
                        <asp:HiddenField ID="hiddenUpdateGanadorId" runat="server" />
                        <asp:HiddenField ID="hiddenTipoPartido" runat="server" />

                    </asp:Panel>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" onclick="closeUpdateModal()">Cancelar</button>
                    <asp:Button ID="btnSaveEditPartido" runat="server" Text="Guardar Cambios" CssClass="btn btn-primary" OnClick="btnUpdatePartido_Click" />
                </div>
            </div>
        </div>
    </div>




    <asp:HiddenField ID="hiddenMessage" runat="server" />
    <asp:HiddenField ID="hiddenMessageType" runat="server" />
    <asp:HiddenField ID="hiddenPartidoId" runat="server" />

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

        function setPartidoIdAndOpenEditModal(id, J1Id, J2Id, nombreJugador1, nombreJugador2, puntosJugador1, puntosJugador2, ganadorId, tipoPartidoId) {
            document.getElementById('<%= hiddenPartidoId.ClientID %>').value = id;
            document.getElementById('<%= hiddenUpdateJ1Id.ClientID %>').value = J1Id;
            document.getElementById('<%= hiddenUpdateJ2Id.ClientID %>').value = J2Id;
            document.getElementById('<%= hiddenUpdateGanadorId.ClientID %>').value = ganadorId;
            document.getElementById('<%= hiddenTipoPartido.ClientID %>').value = tipoPartidoId;

            document.getElementById('nombreJugador1Update').textContent = nombreJugador1;
            document.getElementById('nombreJugador2Update').textContent = nombreJugador2;

            document.getElementById('<%= txtEditPuntosJugador1.ClientID %>').value = puntosJugador1;
            document.getElementById('<%= txtEditPuntosJugador2.ClientID %>').value = puntosJugador2;

            openUpdateModal();
        }


        window.onload = function () {
            var message = document.getElementById('<%= hiddenMessage.ClientID %>').value;
            var messageType = document.getElementById('<%= hiddenMessageType.ClientID %>').value;

            if (message) {
                if (messageType === "success") {
                    showSuccessMessage(message);
                } else if (messageType === "error") {
                    showErrorMessage(message);
                }
                document.getElementById('<%= hiddenMessage.ClientID %>').value = '';
                document.getElementById('<%= hiddenMessageType.ClientID %>').value = '';
            }
        };
    </script>

</asp:Content>
