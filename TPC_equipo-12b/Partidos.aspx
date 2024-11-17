<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="Partidos.aspx.cs" Inherits="TPC_equipo_12b.Partidos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="d-flex justify-content-between align-items-center m-4">
        <h1 class="display-6 text-primary fw-bold text-uppercase pb-2 mb-0">Partidos</h1>
        <asp:Button ID="btnAbrirModal" runat="server" Text="Crear Partido" CssClass="btn btn-primary" OnClientClick="openCreateModal(); return false;" />
    </div>

    <%--MODAL CREAR PARTIDO--%>
    <div class="modal fade" id="creatModal" tabindex="-1" aria-labelledby="modalFormularioPartidoLabel" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" ID="lblFormularioTitulo" id="modalFormularioPartidoLabel">Crear Partido</h5>
                    <button type="button" class="btn-close" aria-label="Close" onclick="closeCreateModal()"></button>
                </div>
                <div class="modal-body">
                    <asp:Panel ID="panel1" runat="server" CssClass="wi mb-4">
                        <asp:HiddenField ID="hfPartidoId" runat="server" />
                        <div class="mb-3">
                            <label for="ddlLiga" class="form-label">Liga</label>
                            <asp:DropDownList ID="ddlLiga" runat="server" CssClass="form-select">
                            </asp:DropDownList>
                        </div>

                        <div class="mb-3">
                            <label for="ddlJugador1" class="form-label">Jugador 1</label>
                            <asp:DropDownList ID="ddlJugador1" runat="server" CssClass="form-select">
                            </asp:DropDownList>
                        </div>

                        <div class="mb-3">
                            <label for="txtPuntosJugador1" class="form-label">Puntos de Jugador 1</label>
                            <asp:TextBox ID="txtPuntosJugador1" runat="server" CssClass="form-control" placeholder="Puntos de Jugador 1"></asp:TextBox>
                        </div>

                        <div class="mb-3">
                            <label for="ddlJugador2" class="form-label">Jugador 2</label>
                            <asp:DropDownList ID="ddlJugador2" runat="server" CssClass="form-select">
                             </asp:DropDownList>
                        </div>

                        <div class="mb-3">
                            <label for="txtPuntosJugador2" class="form-label">Puntos de Jugador 2</label>
                            <asp:TextBox ID="txtPuntosJugador2" runat="server" CssClass="form-control" placeholder="Puntos de Jugador 2"></asp:TextBox>
                        </div>
                    </asp:Panel>
                </div>
                <div class="modal-footer">
                     <button type="button" class="btn btn-secondary" onclick="closeCreateModal()">Cancelar</button>
                    <asp:Button ID="Button1" runat="server" Text="Guardar" CssClass="btn btn-primary" OnClick="btnGuardarPartido_Click" />
                </div>
            </div>
        </div>
    </div>

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

                    </asp:Panel>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" onclick="closeUpdateModal()">Cancelar</button>
                    <asp:Button ID="btnSaveEditPartido" runat="server" Text="Guardar Cambios" CssClass="btn btn-primary" OnClick="btnUpdatePartido_Click" />
                </div>
            </div>
        </div>
    </div>

    <%--LISTAR TODOS LOS PARTIDOS--%>
    <div class="mx-3">
        <div class="row row-cols-1 row-cols-md-3 g-2">
            <asp:Repeater ID="rptPartidos" runat="server">
                <ItemTemplate>
                    <div class="col"> 
                        <div class="card" style="width: 100%;">
                            <div class="card-body">
                                <h5 class="card-title">Liga: <%# Eval("NombreLiga") %></h5>
                                <div class="d-flex align-items-center justify-content-between" style="width: 100%;">
                                    <span style='<%# Convert.ToInt32(Eval("Jugador1Id")) == Convert.ToInt32(Eval("GanadorId")) ? "color: green;" : "color: red;" %>'>Jugador 1: <%# Eval("Jugador1Nombre") %></span>
                                    <span style='<%# Convert.ToInt32(Eval("Jugador1Id")) == Convert.ToInt32(Eval("GanadorId")) ? "color: green;" : "color: red;" %>'> <%# Eval("PuntosJugador1") %></span>
                                </div>
                                <div class="d-flex align-items-center justify-content-between" style="width: 100%;">
                                    <span style='<%# Convert.ToInt32(Eval("Jugador2Id")) == Convert.ToInt32(Eval("GanadorId")) ? "color: green;" : "color: red;" %>'>Jugador 2: <%# Eval("Jugador2Nombre") %></span>
                                    <span style='<%# Convert.ToInt32(Eval("Jugador2Id")) == Convert.ToInt32(Eval("GanadorId")) ? "color: green;" : "color: red;" %>'><%# Eval("PuntosJugador2") %></span>
                                </div>
                                <div class="d-flex justify-content-end mt-3 gap-2">
                                    <button type="button" class="btn btn-secondary" onclick="setPartidoIdAndOpenEditModal('<%# Eval("Id") %>', '<%# Eval("Jugador1Id") %>', '<%# Eval("Jugador2Id") %>', '<%# Eval("Jugador1Nombre") %>', '<%# Eval("Jugador2Nombre") %>', '<%# Eval("GanadorId") %>')">Editar</button>
                                    <asp:Button ID="Button3" runat="server" Text="Eliminar" CssClass="btn btn-danger" OnClientClick="setPartidoIdAndOpenModal(this, 'delete'); return false;" data-ligaid='<%# Eval("Id") %>' />
                                </div>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>

    <%--MODAL BORRAR PARTIDO--%>
    <div id="deleteModal" class="modal fade" tabindex="-1" aria-labelledby="modalLabel" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="modalDeleteLabel">Eliminar Partido</h5>
                    <button type="button" class="btn-close" aria-label="Close" onclick="closeDeleteModal()"></button>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" onclick="closeDeleteModal()">Cancelar</button>
                    <asp:Button ID="Button2" runat="server" CssClass="btn btn-primary" Text="Eliminar" OnClientClick="closeDeleteModal();" OnClick="deletePartido_Click" />
                </div>
            </div>
        </div>
    </div>

    <asp:HiddenField ID="hiddenPartidoId" runat="server" />
    <asp:HiddenField ID="hiddenMessage" runat="server" />
    <asp:HiddenField ID="hiddenMessageType" runat="server" />

    <script>
        function openCreateModal() {
            var modal = document.getElementById("creatModal");
            modal.style.display = "block";
            modal.classList.add("show");
        }

        function closeCreateModal() {
            var modal = document.getElementById("creatModal");
            modal.style.display = "none";
            modal.classList.remove("show");
        }

        function openDeleteModal() {
            var modal = document.getElementById("deleteModal");
            modal.style.display = "block";
            modal.classList.add("show");
        }

        function closeDeleteModal() {
            var modal = document.getElementById("deleteModal");
            modal.style.display = "none";
            modal.classList.remove("show");
        }

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

        function setPartidoIdAndOpenModal(button, action) {
            var ligaId = button.getAttribute("data-ligaid");
            document.getElementById('<%= hiddenPartidoId.ClientID %>').value = ligaId;

            if (action === "update") {
                  openUpdateModal();
            } else {
                   openDeleteModal();
            }
        }

        function setPartidoIdAndOpenEditModal(id, J1Id, J2Id, puntosJugador1, puntosJugador2, ganadorId) {
            //var ligaId = button.getAttribute("data-ligaid");
            document.getElementById('<%= hiddenPartidoId.ClientID %>').value = id;
            document.getElementById('<%= hiddenUpdateJ1Id.ClientID %>').value = J1Id;
            document.getElementById('<%= hiddenUpdateJ2Id.ClientID %>').value = J2Id;
            document.getElementById('<%= hiddenUpdateGanadorId.ClientID %>').value = ganadorId;

            document.getElementById('nombreJugador1Update').textContent = puntosJugador1;
            document.getElementById('nombreJugador2Update').textContent = puntosJugador2;

            openUpdateModal();
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
