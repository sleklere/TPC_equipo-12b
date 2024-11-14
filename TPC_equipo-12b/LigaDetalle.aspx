<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="LigaDetalle.aspx.cs" Inherits="TPC_equipo_12b.LigaDetalle" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="d-flex justify-content-start gap-4 align-items-center m-4">
    <h1 class="display-6 text-primary fw-bold text-uppercase pb-2 mb-0"><%= LigaData.Nombre %></h1>
    <div class="input-group mb-3" style="max-width: 300px;">
        <asp:TextBox ID="txtCodigoLiga" runat="server" CssClass="form-control" ReadOnly="true" Text=''></asp:TextBox>
        <button type="button" class="btn btn-outline-secondary" onclick="copiarCodigo()">Copiar</button>
    </div>
    </div>

    <h3>Jugadores</h3>
    <div class="table-responsive">
        <table class="table table-striped">
            <thead>
                <tr>
                    <th scope="col">#</th>
                    <th scope="col">Nombre</th>
                    <th scope="col">Apellido</th>
                    <th scope="col">Username</th>
                </tr>
            </thead>
            <tbody>
                <% 
                    int index = 1;
                    foreach (Dominio.Jugador jugador in ListaJugadores)
                    {
                %>
                <tr>
                    <th scope="row"><%: index++ %></th>
                    <td><%: jugador.Nombre %></td>
                    <td><%: jugador.Apellido %></td>
                    <td><%: jugador.Username %></td>
                </tr>
                <% } %>
            </tbody>
        </table>
    </div>

    <script>
        function copiarCodigo() {
            var codigoTextBox = document.getElementById('<%= txtCodigoLiga.ClientID %>');

            codigoTextBox.select();

            document.execCommand("copy");

            showSuccessMessage("Código copiado");
        }
    </script>
</asp:Content>
