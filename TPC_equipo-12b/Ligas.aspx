<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="Ligas.aspx.cs" Inherits="TPC_equipo_12b.Ligas" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server" />--%>

    <div class="d-flex justify-content-between align-items-center m-4">
        <h1 class="display-6 text-primary fw-bold text-uppercase border-bottom pb-2 mb-0">Ligas</h1>
        <asp:Button ID="btnCrearLiga" runat="server" Text="Crear Liga" CssClass="btn btn-primary" OnClick="btnCrearLiga_Click" />
    </div>

    <div class="row row-cols-1 row-cols-md-2 g-4 mx-4">
        <asp:Repeater ID="rptLigas" runat="server">
            <ItemTemplate>
                <div class="col">
                    <div class="card shadow-sm">
                        <div class="card-body">
                            <h5 class="card-title text-uppercase font-weight-bold"><%# Eval("Nombre") %></h5>
                            <p class="card-text text-muted"><%# ((List<Dominio.Jugador>)Eval("Jugadores")).Count %> jugadores</p>
                            <div class="d-flex gap-2">
                                <a href='<%# "LigaDetalle.aspx?id=" + Eval("Id") %>' class="btn btn-outline-primary">Ver</a>
                                <asp:Button ID="btnEditarLiga" runat="server" Text="Editar" CssClass="btn btn-secondary btn-sm"
                                    CommandName="Editar" CommandArgument='<%# Eval("Id") %>' OnCommand="btnEditarLiga_Command" />
                    <asp:Button ID="btnDeleteLiga" runat="server" Text="Eliminar" CssClass="btn btn-danger" OnClientClick="setLigaIdAndOpenModal(this, 'delete'); return false;" data-ligaid='<%# Eval("Id") %>' />
                            </div>
                        </div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>

    <div id="modalPanel" class="modal fade" tabindex="-1" aria-labelledby="modalLabel" aria-hidden="true">

        <%-- diferenciar entre creacion y edicion --%>
        <asp:HiddenField ID="hfLigaId" runat="server" />

        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="modalLabel">
                        <asp:Label ID="lblModalTitle" runat="server" Text="Crear Nueva Liga"></asp:Label>
                    </h5>
                    <button type="button" class="btn-close" aria-label="Close" onclick="closeModal()"></button>
                </div>
                <div class="modal-body">
                    <asp:TextBox ID="txtLigaNombre" runat="server" CssClass="form-control mb-3" placeholder="Nombre de la liga"></asp:TextBox>

                    <asp:UpdatePanel ID="UpdatePanelBuscarJugador" runat="server">
                        <ContentTemplate>
                            <div class="input-group mb-3">
                                <asp:TextBox ID="txtCodigoJugador" runat="server" CssClass="form-control" placeholder="Código del jugador"></asp:TextBox>
                                <asp:Button ID="btnBuscarJugador" runat="server" CssClass="btn btn-outline-secondary" Text="Agregar" OnClick="btnBuscarJugador_Click" />
                            </div>
                            <asp:Label ID="lblMensajeBusqueda" runat="server" CssClass="text-info" Visible="false"></asp:Label>
                            <div class="mt-3">
                                <h6 class="fw-bold">Jugadores Agregados:</h6>
                                <asp:Repeater ID="rptJugadoresAgregados" runat="server" OnItemCommand="rptJugadoresAgregados_ItemCommand">
                                    <ItemTemplate>
                                        <div class="list-group-item d-flex justify-content-between align-items-center">
                                            <span><%# Eval("Username") %></span>
                                            <asp:Button ID="btnEliminar" runat="server" CommandName="Eliminar" CommandArgument='<%# Eval("Id") %>' Text="Eliminar" CssClass="btn btn-danger btn-sm" />
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                        </ContentTemplate>
                        <%-- actualizacion parcial (prevenir postback completo) --%>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnBuscarJugador" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" onclick="closeModal()">Cancelar</button>
                    <asp:Button ID="btnSaveLiga" runat="server" CssClass="btn btn-primary" Text="Guardar" OnClick="btnSaveLiga_Click" />
                </div>
            </div>
        </div>
    </div>

      <div id="deleteModal" class="modal fade" tabindex="-1" aria-labelledby="modalLabel" aria-hidden="true">
     <div class="modal-dialog modal-dialog-centered">
         <div class="modal-content">
             <div class="modal-header">
                 <h5 class="modal-title" id="modalDeleteLabel">Eliminar Liga</h5>
                 <button type="button" class="btn-close" aria-label="Close" onclick="closeUpdateModal()"></button>
             </div>
             <div class="modal-footer">
                 <button type="button" class="btn btn-secondary" onclick="closeDeleteModal()">Cancelar</button>
                 <asp:Button ID="Button2" runat="server" CssClass="btn btn-primary" Text="Eliminar" OnClientClick="closeDeleteModal();" OnClick="deleteLiga_Click" />
             </div>
             </div>
         </div>
     </div>

    <asp:HiddenField ID="hiddenLigaId" runat="server" />
    <asp:HiddenField ID="hiddenMessage" runat="server" />

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

        function setLigaIdAndOpenModal(button, action) {
            var ligaId = button.getAttribute("data-ligaid");
            document.getElementById('<%= hiddenLigaId.ClientID %>').value = ligaId;

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
