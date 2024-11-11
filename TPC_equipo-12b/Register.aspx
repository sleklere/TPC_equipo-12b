<%@ Page Title="Registrarse" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="TPC_equipo_12b.Register" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="d-flex justify-content-center align-items-center" style="height: calc(100vh - 56px);">
        <div class="w-25">
            <h2 class="text-center">Registrarse</h2>
            <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>

            <asp:TextBox ID="txtNombre" runat="server" placeholder="Nombre" CssClass="form-control"></asp:TextBox><br />
            <asp:TextBox ID="txtApellido" runat="server" placeholder="Apellido" CssClass="form-control"></asp:TextBox><br />
            <asp:TextBox ID="txtUsername" runat="server" placeholder="Username" CssClass="form-control"></asp:TextBox><br />
            <asp:TextBox ID="txtEmail" runat="server" placeholder="Email" CssClass="form-control"></asp:TextBox><br />
            <asp:TextBox ID="txtPassword" runat="server" placeholder="Contraseña" TextMode="Password" CssClass="form-control"></asp:TextBox><br />

            <asp:Button ID="btnCrear" runat="server" Text="Crear" CssClass="btn btn-primary w-100" OnClick="btnCrear_Click" />
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