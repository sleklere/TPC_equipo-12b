<%@ Page Title="Iniciar Sesión" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="TPC_equipo_12b.Login" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="d-flex justify-content-center align-items-center" style="height: calc(100vh - 56px);">
        <div class="w-25">
            <h2 class="text-center">Iniciar sesión</h2>
            <asp:Label ID="lblMessage" runat="server" ForeColor="Red" CssClass="d-block text-center mb-3"></asp:Label>

            <asp:TextBox ID="txtEmail" runat="server" placeholder="Email" CssClass="form-control mb-3"></asp:TextBox>
            <asp:TextBox ID="txtPassword" runat="server" placeholder="Contraseña" TextMode="Password" CssClass="form-control mb-3"></asp:TextBox>

            <asp:Button ID="btnCrear" runat="server" Text="Iniciar sesión" CssClass="btn btn-primary w-100" OnClick="btnLogin_Click" />
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