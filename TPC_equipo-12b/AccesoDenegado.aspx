<%@ Page Title="Acceso Denegado" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="AccesoDenegado.aspx.cs" Inherits="TPC_equipo_12b.AccesoDenegado" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="d-flex justify-content-center align-items-center" style="height: calc(100vh - 56px);">
        <div class="text-center">
            <div class="card shadow-sm">
                <div class="card-body">
                    <h2 class="text-danger">Acceso Denegado</h2>
                    <p>No tienes permiso para ver esta página. Por favor, inicia sesión para continuar.</p>
                    <a href="Login.aspx" class="btn btn-primary mt-3">Iniciar Sesión</a>
                </div>
            </div>
        </div>
    </div>
</asp:Content>