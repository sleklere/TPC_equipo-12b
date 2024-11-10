<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="TPC_equipo_12b.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="d-flex justify-content-between align-items-center m-4">
        <h1 class="display-6 text-primary fw-bold text-uppercase border-bottom pb-2 mb-0">Gestión de Partidos</h1>
    </div>

    <asp:Panel ID="panelFormularioPartido" runat="server" CssClass="mb-4">
        <h3>
            <asp:Label ID="lblFormularioTitulo" runat="server" Text="Crear Partido"></asp:Label></h3>

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

        <div class="mb-3">
            <label for="txtResultado" class="form-label">Resultado General</label>
            <asp:TextBox ID="txtResultado" runat="server" CssClass="form-control" placeholder="Resultado general del partido (opcional)"></asp:TextBox>
        </div>

        <asp:Button ID="btnGuardarPartido" runat="server" Text="Guardar" CssClass="btn btn-primary" OnClick="btnGuardarPartido_Click" />
        <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn btn-secondary" OnClick="btnCancelar_Click" />
    </asp:Panel>

    <h3>Partidos</h3>
    <asp:Repeater ID="rptPartidos" runat="server">
        <ItemTemplate>
            <div class="card" style="width: 20rem;">
                <div class="card-body">
                    <h5 class="card-title">Liga amigos 2024</h5>
<%--                    <p class="card-text">With supporting text below as a natural lead-in to additional content.</p>--%>
                    <div class="d-flex align-items-center justify-content-between" style="width: 100%;">
                        <span><%# Eval("Jugador1Nombre") %></span>
                        <span><%# Eval("PuntosJugador1") %></span>
                    </div>
                    <div class="d-flex align-items-center justify-content-between" style="width: 100%;">
                        <span><%# Eval("Jugador2Nombre") %></span>
                        <span><%# Eval("PuntosJugador2") %></span>
                    </div>
                    <a href="#" class="btn btn-primary">Go somewhere</a>
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>

    <asp:HiddenField ID="hiddenMessage" runat="server" />

    <script>
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
