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
                <asp:DropDownList ID="ddlJugador1" runat="server" CssClass="form-select" OnSelectedIndexChanged="ddlJugador1_SelectedIndexChanged" AutoPostBack="true">
                </asp:DropDownList>
            </div>
            <div class="card text-center mb-4 versus-card-jugador" style="width: 400px;">
                <div class="rounded-circle d-flex justify-content-center align-items-center text-white mx-auto mt-3"
                    style="background-color: #228be6; width: 200px; height: 200px; font-size: 5rem;">
                        <asp:Label ID="inicialJugador1" runat="server" ForeColor="white" />
                </div>

                <div class="card-body">

                    <p class="mb-1">
                        <strong>W/L Histórico:</strong>
                        <asp:Label ID="lblVDHistoricoJ1" runat="server" ForeColor="Black" />
                    </p>
                    <p class="mb-1">
                        <strong>Total Partidos Jugados:</strong>
                        <asp:Label ID="lblJugador1TotalPartidos" runat="server" ForeColor="Black" />
                    </p>
                    <p class="mb-1">
                        <strong>Porcentaje de Victorias:</strong>
                        <asp:Label ID="porcentajeVictoriasJ1" runat="server" ForeColor="Black" />
                    </p>
                    <p class="mb-1">
                        <strong>Racha Actual:</strong>
                        <asp:Label ID="lblRachaJ1" runat="server" ForeColor="Black" />
                    </p>
                    <p class="mb-1">
                        <strong>Torneos Ganados:</strong>
                        <asp:Label ID="lblTorneosJ1" runat="server" ForeColor="Black" />
                    </p>
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
                <div class="rounded-circle d-flex justify-content-center align-items-center text-white mx-auto mt-3"
                    style="background-color: #228be6; width: 200px; height: 200px; font-size: 5rem;">
                        <asp:Label ID="inicialJugador2" runat="server" ForeColor="white" />
                </div>
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
                    <p class="mb-1">
                        <strong>Racha Actual:</strong>
                        <asp:Label ID="lblRachaJ2" runat="server" ForeColor="Black" />
                    </p>
                    <p class="mb-1">
                        <strong>Torneos Ganados:</strong>
                        <asp:Label ID="lblTorneosJ2" runat="server" ForeColor="Black" />
                    </p>
                </div>
            </div>
        </div>
    </div>

    <div class="mx-3 my-5">
        <div class="d-flex justify-content-between align-items-center m-4">
            <h3>Partidos</h3>
        </div>
        <div class="row row-cols-1 row-cols-md-3 g-2">
            <asp:Repeater ID="rptPartidos" runat="server">
                <ItemTemplate>
                    <div class="col">
                        <div class="card" style="width: 100%;">
                            <div class="card-body">
                                <div class="d-flex justify-content-between">
                                    <h5 class="card-title">
                                        <%# !string.IsNullOrEmpty(Eval("NombreLiga") as string) ? "Liga: " + Eval("NombreLiga") : "Torneo: " + Eval("NombreTorneo") %>
                                    </h5>
                                    <h5 class="card-title"><%# Convert.ToDateTime(Eval("Fecha")).ToString("dd/MM/yyyy") %></h5>
                                </div>
                                <div class="d-flex align-items-center justify-content-between" style="width: 100%;">
                                    <span style='<%# Convert.ToInt32(Eval("Jugador1Id")) == Convert.ToInt32(Eval("GanadorId")) ? "color: green;": "color: red;" %>'>Jugador 1: <%# Eval("Jugador1Nombre") %></span>
                                    <span style='<%# Convert.ToInt32(Eval("Jugador1Id")) == Convert.ToInt32(Eval("GanadorId")) ? "color: green;": "color: red;" %>'><%# Eval("PuntosJugador1") %></span>
                                </div>
                                <div class="d-flex align-items-center justify-content-between" style="width: 100%;">
                                    <span style='<%# Convert.ToInt32(Eval("Jugador2Id")) == Convert.ToInt32(Eval("GanadorId")) ? "color: green;": "color: red;" %>'>Jugador 2: <%# Eval("Jugador2Nombre") %></span>
                                    <span style='<%# Convert.ToInt32(Eval("Jugador2Id")) == Convert.ToInt32(Eval("GanadorId")) ? "color: green;": "color: red;" %>'><%# Eval("PuntosJugador2") %></span>
                                </div>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
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
