<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="Versus.aspx.cs" Inherits="TPC_equipo_12b.Versus" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="d-flex justify-content-between align-items-center m-4">
        <h1 class="display-6 text-primary fw-bold text-uppercase pb-2 mb-0">Jugador vs Jugador</h1>
    </div>

    <div class="d-flex justify-content-center" style="gap: 20px; margin-top: 20px;">

        <%-- jugador 1 --%>
        <div class="d-flex flex-column">
            <div class="mb-3">
                <%--<label for="ddlJugador1" class="form-label">Jugador 1</label>--%>
                <asp:DropDownList ID="ddlJugador1" runat="server" CssClass="form-select" OnSelectedIndexChanged="ddlJugador1_SelectedIndexChanged" AutoPostBack="true">
                </asp:DropDownList>
            </div>
            <div class="card text-center mb-4 versus-card-jugador" style="width: 400px;">
                <img src="https://via.placeholder.com/150" alt="Foto de Perfil" class="card-img-top rounded-circle mt-3" style="width: 200px; height: 200px; object-fit: cover; margin: 0 auto;">
                <div class="card-body">
                    <%--                <h5 class="card-title fw-bold">
                    <asp:Label ID="lblJugador1Nombre" runat="server" />
                </h5>--%>

                    <p class="mb-1">
                        <strong>W/L Histórico:</strong>
                        <asp:Label ID="lblVDHistoricoJ1" runat="server" ForeColor="Black" />
                    </p>
                    <%--<p class="mb-1"><strong>Torneos Ganados:</strong> 3</p>--%>
                    <%--<p class="mb-1"><strong>Mejor Resultado Torneo:</strong> Final</p>--%>
                    <p class="mb-1">
                        <strong>Total Partidos Jugados:</strong>
                        <asp:Label ID="lblJugador1TotalPartidos" runat="server" ForeColor="Black" />
                    </p>
                    <p class="mb-1">
                        <strong>Porcentaje de Victorias:</strong>
                        <asp:Label ID="porcentajeVictoriasJ1" runat="server" ForeColor="Black" />
                    </p>
                    <p class="mb-1"><strong>Racha Actual:</strong> 5 victorias</p>
                </div>
            </div>
        </div>
        <%-- ganados/perdidos --%>
        <div class="card border-light shadow-sm p-3 mb-4" style="max-width: 300px; max-height: 150px;">
            <div class="card-body text-center d-flex align-items-center justify-content-center">
                <div class="d-flex justify-content-around align-items-center">
                    <div>
                        <span class="d-block fs-1 fw-bold">
                            <asp:Label ID="lblJugador1Victorias" runat="server" />
                        </span>
                        <%--<span class="text-muted">Ganados</span>--%>
                    </div>
                    <div>
                        <span class="d-block fs-1 fw-bold">-</span>
                        <%--<span class="text-muted">Ganados</span>--%>
                    </div>
                    <div>
                        <span class="d-block fs-1 fw-bold">
                            <asp:Label ID="lblJugador2Victorias" runat="server" />
                        </span>
                        <%--<span class="text-muted">Perdidos</span>--%>
                    </div>
                </div>
            </div>
        </div>

        <%-- jugador 2 --%>
        <div class="d-flex flex-column">
            <div class="mb-3">
                <%--<label for="ddlJugador2" class="form-label">Jugador 2</label>--%>
                <asp:DropDownList ID="ddlJugador2" runat="server" CssClass="form-select" OnSelectedIndexChanged="ddlJugador2_SelectedIndexChanged" AutoPostBack="true">
                </asp:DropDownList>
            </div>
            <div class="card text-center mb-4 versus-card-jugador" style="width: 400px;">
                <img src="https://via.placeholder.com/150" alt="Foto de Perfil" class="card-img-top rounded-circle mt-3" style="width: 200px; height: 200px; object-fit: cover; margin: 0 auto;">
                <div class="card-body">
                    <%--                <h5 class="card-title fw-bold">
                    <asp:Label ID="lblJugador2Nombre" runat="server" />
                </h5>--%>
                    <p class="mb-1">
                        <strong>W/L Histórico:</strong>
                        <asp:Label ID="lblVDHistoricoJ2" runat="server" ForeColor="Black" />
                    </p>
                    <%--<p class="mb-1"><strong>Torneos Ganados:</strong> 3</p>--%>
                    <%--<p class="mb-1"><strong>Mejor Resultado Torneo:</strong> Final</p>--%>
                    <p class="mb-1">
                        <strong>Total Partidos Jugados:</strong>
                        <asp:Label ID="lblJugador2TotalPartidos" runat="server" ForeColor="Black" />
                    </p>
                    <p class="mb-1">
                        <strong>Porcentaje de Victorias:</strong>
                        <asp:Label ID="porcentajeVictoriasJ2" runat="server" ForeColor="Black" />
                    </p>
                    <p class="mb-1"><strong>Racha Actual:</strong> 5 victorias</p>
                </div>
            </div>
        </div>
    </div>


    <h4 class="" style="margin: 0 24px; padding: 0 24px; font-size: 32px; font-weight: 600;">Partidos</h4>
    <div class="m-24 p-24" style="margin: 24px; padding: 24px;">
        <asp:Repeater ID="rptPartidos" runat="server">
            <ItemTemplate>
                <div class="d-flex align-items-center justify-content-between py-2 mb-2 border-bottom">
                    <!-- Liga -->
                    <span class="text-muted fw-bold" style="width: 20%;">Liga amigos 2024</span>
                    <!-- Jugador 1 -->
<%--                    <div class="d-flex align-items-center">
                        <span class="fw-bold mr-2"><%# Eval("Jugador1Nombre") %></span>
                    </div>--%>
                    <%-- Ganador --%>
                    <div class="d-flex align-items-center">
                        <span class="badge bg-primary rounded-pill fs-6"><%# Eval("GanadorNombre") %></span>
                    </div>
                    <div class="d-flex align-items-center" style="gap: 8px;">
                        <span class="badge bg-secondary rounded-pill fs-6"><%# Eval("PuntosJugador1") %></span>
                        <span class="badge bg-secondary rounded-pill fs-6"><%# Eval("PuntosJugador2") %></span>
                    </div>
                    <!-- Jugador 2 -->
<%--                    <div class="d-flex align-items-center">
                        <span class="fw-bold ml-2"><%# Eval("Jugador2Nombre") %></span>
                    </div>--%>
                    <!-- Detalles -->
                    <a href="#" class="btn btn-outline-primary btn-sm">Detalles</a>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>

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
