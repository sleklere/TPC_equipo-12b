<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="Torneos.aspx.cs" Inherits="TPC_equipo_12b.Torneos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="d-flex justify-content-between align-items-center m-4">
        <h1 class="display-6 text-primary fw-bold text-uppercase border-bottom pb-2 mb-0">Torneos</h1>
        <div class="d-flex justify-content-end gap-4 align-items-center">
            <asp:Button ID="btnCrearTorneo" runat="server" Text="Crear Torneo" CssClass="btn btn-primary" OnClick="btnCrearTorneo_Click" />
        </div>
    </div>

    <asp:Label ID="lblSinTorneos" runat="server" Visible="false" CssClass="m-4">
        No hay torneos disponibles.
    </asp:Label>

    <div class="row row-cols-1 row-cols-md-2 g-4 mx-4">
        <asp:Repeater ID="rptTorneos" runat="server">
            <ItemTemplate>
                <div class="col">
                    <div class="card shadow-sm">
                        <div class="card-body">
                            <div class="d-flex align-items-center justify-content-between">
                                <h5 class="card-title text-uppercase font-weight-bold"><%# Eval("Nombre") %></h5>
                                <div class="d-flex justify-content-start align-items-center">
                                    <div class='<%# (int)Eval("GanadorId") > 0 ? "badge bg-success text-white px-3 py-2" : "badge bg-secondary text-white px-3 py-2" %>'>
                                        <%# (int)Eval("GanadorId") > 0 ? "Finalizado" : "En proceso" %>
                                    </div>
                                </div>
                            </div>

                            <p class="card-text text-muted"><%# ((List<Dominio.Jugador>)Eval("Jugadores")).Count %> jugadores</p>
                            <div class="d-flex gap-2">
                                <a href='<%# "/TorneoDetalle.aspx?id=" + Eval("Id") %>' class="btn btn-outline-primary">Ver</a>
                                <asp:Button ID="btnEditarTorneo" runat="server" Text="Editar Nombre" CssClass="btn btn-secondary btn-sm"
                                    CommandName="Editar" CommandArgument='<%# Eval("Id") %>' OnCommand="btnEditarTorneo_Command" />
                                <asp:Button ID="btnDeleteTorneo" runat="server" Text="Eliminar" CssClass="btn btn-danger" OnClientClick="setTorneoIdAndOpenModal(this, 'delete'); return false;" data-torneoid='<%# Eval("Id") %>' />
                            </div>
                        </div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>

    <div id="modalPanel" class="modal fade" tabindex="-1" aria-labelledby="modalLabel" aria-hidden="true">
        <%-- diferenciar entre creacion y edicion --%>
        <asp:HiddenField ID="hfTorneoId" runat="server" />

        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="modalLabel">
                        <asp:Label ID="lblModalTitle" runat="server" Text="Crear Nuevo Torneo"></asp:Label>
                    </h5>
                    <button type="button" class="btn-close" aria-label="Close" onclick="closeModal()"></button>
                </div>
                <div class="modal-body">
                    <asp:TextBox ID="txtTorneoNombre" runat="server" CssClass="form-control mb-3" placeholder="Nombre del torneo"></asp:TextBox>
                    <div id="divLiga" class="mb-3" runat="server" visible="false">
                        <label for="ddlLiga" class="form-label">Liga</label>
                        <asp:DropDownList ID="ddlLiga" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlLiga_SelectedIndexChanged">
                        </asp:DropDownList>
                    </div>
                    <asp:UpdatePanel ID="UpdatePanelTorneo" runat="server">
                        <ContentTemplate>
                            <asp:Repeater ID="rptJugadores" runat="server">
                                <ItemTemplate>
                                    <div class="form-check">
                                        <input class="form-check-input" type="checkbox" id="Checkbox1" value='<%# Eval("Id") %>' runat="server" />
                                        <label class="form-check-label" for="chkJugador<%# Eval("Id") %>'"><%# Eval("Username") %></label>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlLiga" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>

                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" onclick="closeModal()">Cancelar</button>
                    <asp:Button ID="btnSaveTorneo" runat="server" CssClass="btn btn-primary" Text="Guardar" OnClick="btnSaveTorneo_Click" />
                </div>
            </div>
        </div>
    </div>

    <div id="deleteModal" class="modal fade" tabindex="-1" aria-labelledby="modalLabel" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="modalDeleteLabel">Eliminar Torneo</h5>
                    <button type="button" class="btn-close" aria-label="Close" onclick="closeDeleteModal()"></button>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" onclick="closeDeleteModal()">Cancelar</button>
                    <asp:Button ID="Button2" runat="server" CssClass="btn btn-primary" Text="Eliminar" OnClientClick="closeDeleteModal();" OnClick="deleteTorneo_Click" />
                </div>
            </div>
        </div>
    </div>

    <asp:HiddenField ID="hiddenMessage" runat="server" />
    <asp:HiddenField ID="hiddenMessageType" runat="server" />
    <asp:HiddenField ID="hiddenTorneoId" runat="server" />

    <script>

        function openModal() {
            var modal = document.getElementById("modalPanel");
            modal.style.display = "block";
            modal.classList.add("show");
        }

        function closeModal() {
            var modal = document.getElementById("modalPanel");
            modal.style.display = "none";
            modal.classList.remove("show");
        }

        function closeJoinModal() {
            var modal = document.getElementById("joinModal");
            modal.style.display = "none";
            modal.classList.remove("show");
        }

        function setTorneoIdAndOpenModal(button, action) {
            var ligaId = button.getAttribute("data-torneoid");
            document.getElementById('<%= hiddenTorneoId.ClientID %>').value = ligaId;

            if (action === "update") {
                openUpdateModal();
            } else {
                openDeleteModal();
            }
        }

        function openUpdateModal() {
            var modal = document.getElementById("modalPanel");
            modal.style.display = "block";
            modal.classList.add("show");
        }

        function closeUpdateModal() {
            var modal = document.getElementById("modalPanel");
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
