<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="Versus.aspx.cs" Inherits="TPC_equipo_12b.Versus" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="d-flex justify-content-between align-items-center m-4">
        <h1 class="display-6 text-primary fw-bold text-uppercase border-bottom pb-2 mb-0">Jugador vs Jugador</h1>
    </div>

    <div class="d-flex justify-content-center" style="gap: 20px; margin-top: 20px;">

        <%-- jugador 1 --%>
        <div class="mb-3">
            <label for="ddlJugador1" class="form-label">Jugador 1</label>
            <asp:DropDownList ID="ddlJugador1" runat="server" CssClass="form-select" OnSelectedIndexChanged="ddlJugador1_SelectedIndexChanged" AutoPostBack="true">
            </asp:DropDownList>
        </div>
        <div class="card text-center mb-4" style="width: 400px;">
            <img src="https://via.placeholder.com/150" alt="Foto de Perfil" class="card-img-top rounded-circle mt-3" style="width: 100px; height: 100px; object-fit: cover; margin: 0 auto;">
            <div class="card-body">
                <h5 class="card-title fw-bold">
                    <asp:Label ID="lblJugador1Nombre" runat="server" />
                </h5>

                <!-- Estadísticas del Jugador -->
                <p class="mb-1"><strong>W/L Histórico:</strong> 53-22</p>
                <p class="mb-1"><strong>Torneos Ganados:</strong> 3</p>
                <p class="mb-1"><strong>Mejor Resultado Torneo:</strong> Final</p>
                <p class="mb-1"><strong>Partidos Jugados Recientemente:</strong> 10</p>
                <p class="mb-1"><strong>Porcentaje de Victorias:</strong> 70%</p>
                <p class="mb-1"><strong>Racha Actual:</strong> 5 victorias</p>
            </div>
        </div>
        <%-- ganados/perdidos --%>
        <div class="card border-light shadow-sm p-3 mb-4" style="max-width: 300px;">
            <div class="card-body text-center">
                <div class="d-flex justify-content-around align-items-center mt-3">
                    <div>
                        <span class="d-block fs-1 fw-bold text-success">           
                            <asp:Label ID="lblJugador1Victorias" runat="server" />
                        </span>
                        <%--<span class="text-muted">Ganados</span>--%>
                    </div>
                    <div>
                        <span class="d-block fs-1 fw-bold">-</span>
                        <%--<span class="text-muted">Ganados</span>--%>
                    </div>
                    <div>
                        <span class="d-block fs-1 fw-bold text-danger">
                            <asp:Label ID="lblJugador2Victorias" runat="server" />
                        </span>
                        <%--<span class="text-muted">Perdidos</span>--%>
                    </div>
                </div>
            </div>
        </div>

        <%-- jugador 2 --%>
        <div class="mb-3">
            <label for="ddlJugador2" class="form-label">Jugador 2</label>
            <asp:DropDownList ID="ddlJugador2" runat="server" CssClass="form-select" OnSelectedIndexChanged="ddlJugador2_SelectedIndexChanged" AutoPostBack="true">
            </asp:DropDownList>
        </div>
        <div class="card text-center mb-4" style="width: 400px;">
            <img src="https://via.placeholder.com/150" alt="Foto de Perfil" class="card-img-top rounded-circle mt-3" style="width: 100px; height: 100px; object-fit: cover; margin: 0 auto;">
            <div class="card-body">
                <h5 class="card-title fw-bold">
                    <asp:Label ID="lblJugador2Nombre" runat="server" />
                </h5>
            </div>
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
