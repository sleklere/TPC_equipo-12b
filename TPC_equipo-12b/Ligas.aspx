<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="Ligas.aspx.cs" Inherits="TPC_equipo_12b.Ligas" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="d-flex align-items-center gap-2 mt-2 mb-2">
        <h1>LIGAS</h1>
        <button type="button" class="btn btn-primary" onclick="openModal()">Crear Liga</button>
    </div>

    <asp:Repeater ID="rptLigas" runat="server">
        <ItemTemplate>
            <div class="card w-50">
                <div class="card-body">
                    <h5 class="card-title"><%# Eval("Nombre") %></h5>
                    <p class="card-text">X jugadores</p>
                    <a href='<%# "LigaDetalle.aspx?id=" + Eval("Id") %>' class="btn btn-primary">Ver</a>
                    <asp:Button ID="btnVerLiga" runat="server" Text="Modificar" OnClientClick="openUpdateModal(); return false;" OnClick="btnModalUpdate_Click" CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-primary" />
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>

    <div id="modalPanel" class="modal fade" tabindex="-1" aria-labelledby="modalLabel" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="modalLabel">Crear Nueva Liga</h5>
                    <button type="button" class="btn-close" aria-label="Close" onclick="closeModal()"></button>
                </div>
                <div class="modal-body">
                    <asp:TextBox ID="txtLigaNombre" runat="server" CssClass="form-control" placeholder="Nombre de la liga"></asp:TextBox>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" onclick="closeModal()">Cancelar</button>
                    <asp:Button ID="btnSaveLiga" runat="server" CssClass="btn btn-primary" Text="Guardar" OnClientClick="closeModal();" OnClick="btnSaveLiga_Click" />
                </div>
            </div>
        </div>
    </div>

     <div id="updateModal" class="modal fade" tabindex="-1" aria-labelledby="modalLabel" aria-hidden="true">
     <div class="modal-dialog modal-dialog-centered">
         <div class="modal-content">
             <div class="modal-header">
                 <h5 class="modal-title" id="modalUpdateLabel">Editar Liga</h5>
                 <button type="button" class="btn-close" aria-label="Close" onclick="closeUpdateModal()"></button>
             </div>
             <div class="modal-body">
                 <asp:TextBox ID="txtLigaNombreUpdate" runat="server" CssClass="form-control" placeholder="Nombre de la liga"></asp:TextBox>
             </div>
             <div class="modal-footer">
                 <button type="button" class="btn btn-secondary" onclick="closeModal()">Cancelar</button>
                 <asp:Button ID="Button1" runat="server" CssClass="btn btn-primary" Text="Guardar" OnClientClick="closeUpdateModal();" OnClick="updateLiga_Click" />
             </div>
             </div>
         </div>
     </div>

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
    </script>
</asp:Content>