<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Carrito.aspx.cs" Inherits="PAD_TFI.Carrito" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

        <asp:Panel ID="PagarPanel" runat="server" BackColor="#E4E4E4" Height="124px">
            <h2 style="font-family: 'Arial Black'; font-size: x-large; font-weight: bold; font-style: oblique; color: #339933; vertical-align: middle; text-align: center; ">Confirmarción Exitosa</h2>
            <asp:Label ID="Label6" runat="server" Width="938px"></asp:Label>
            <asp:Button ID="pagarBTN" runat="server" Text="Pagar" BackColor="#99CCFF" BorderColor="#6699FF" BorderStyle="Solid" BorderWidth="5px" Font-Bold="True" Font-Size="14pt" Height="38px" Width="184px" ForeColor="White" />

        </asp:Panel>

    <div class="carrito-container" id="carrito">
        <asp:Table runat="server" ID="ProductsTable" CellPadding="4" BorderStyle="Solid" BorderWidth="2px" CellSpacing="2" BackColor="Silver" BorderColor="#666666" HorizontalAlign="Left" Width="1171px">

        </asp:Table>
    </div>
    <div class="comprador-info" id="comprador">
        <asp:Panel ID="Panel1" runat="server" BackColor="#E4E4E4">
            <br />
            <h2 style="font-family: 'Arial Black'; font-size: large; font-weight: bold; font-style: normal; color: #CC9900; vertical-align: middle; text-align: center; text-decoration: underline;">Datos del Comprador</h2>
            <asp:Label ID="nombreLB" runat="server" Text="Nombre: " Font-Bold="True" Font-Size="16pt"></asp:Label>
            <asp:TextBox ID="nombreTB" runat="server" Width="291px" ></asp:TextBox>
            <asp:Label ID="Label1" runat="server" Width="40"></asp:Label>
            <asp:Label ID="apellidoLB" runat="server" Font-Bold="True" Font-Size="16pt" Text="  Apellido: "></asp:Label>
            <asp:TextBox ID="apellidoTB" runat="server" Width="399px"></asp:TextBox>
            <br />
            <asp:Label ID="dniLB" runat="server" Text="DNI: " Font-Bold="True" Font-Size="16pt"></asp:Label>
            <asp:TextBox ID="dniTB" runat="server" Width="291px" MaxLength="8" ></asp:TextBox>
            <asp:Label ID="Label3" runat="server" Width="93px"></asp:Label>
            <asp:Label ID="telefonoLB" runat="server" Font-Bold="True" Font-Size="16pt" Text="Telefono: "></asp:Label>
            <asp:TextBox ID="telefonoTB" runat="server" Width="399px"></asp:TextBox>
            <br />
            <asp:Label ID="correoLB" runat="server" Font-Bold="True" Font-Size="16pt" Text="Correo: "></asp:Label>
            <asp:TextBox ID="correoTB" runat="server" Width="399px"></asp:TextBox>
            <br />
            <h2 style="font-family: 'Arial Black'; font-size: large; font-weight: bold; font-style: normal; color: #003399; vertical-align: middle; text-align: center; text-decoration: underline;">Datos de Envío</h2>
            <asp:Label ID="calleLB" runat="server" Text="Calle: " Font-Bold="True" Font-Size="16pt"></asp:Label>
            <asp:TextBox ID="calleTB" runat="server" Width="291px" ></asp:TextBox>
            <asp:Label ID="Label4" runat="server" Width="85px"></asp:Label>
            <asp:Label ID="alturaLB" runat="server" Font-Bold="True" Font-Size="16pt" Text="Altura: "></asp:Label>
            <asp:TextBox ID="alturaTB" runat="server" Width="399px"></asp:TextBox>
            <br />
            <asp:Label ID="pisoLB" runat="server" Text="Piso: " Font-Bold="True" Font-Size="16pt"></asp:Label>
            <asp:TextBox ID="pisoTB" runat="server" Width="291px" MaxLength="8" ></asp:TextBox>
            <asp:Label ID="Label7" runat="server" Width="93px"></asp:Label>
            <asp:Label ID="dptoLB" runat="server" Font-Bold="True" Font-Size="16pt" Text="Departamento: "></asp:Label>
            <asp:TextBox ID="dptoTB" runat="server" Width="399px"></asp:TextBox>
            <p>
            <asp:Label ID="provinciaLB" runat="server" Text="Provincia: " Font-Bold="True" Font-Size="16pt"></asp:Label>
            <asp:DropDownList ID="provinciaSL" runat="server" AutoPostBack="true" OnSelectedIndexChanged="provinciaSL_SelectedIndexChanged" Font-Size="12pt">
            </asp:DropDownList>
            <asp:Label ID="Label2" runat="server" Width="93px"></asp:Label>
            <asp:Label ID="localidadLB" runat="server" Font-Bold="True" Font-Size="16pt" Text="Localidad: "></asp:Label>
            <asp:DropDownList ID="localidadSL" runat="server" AutoPostBack="true" OnSelectedIndexChanged="localidadSL_SelectedIndexChanged" Font-Size="12pt" Width="190px">
                </asp:DropDownList>
            </p>
            
            <br />
            <br />
        </asp:Panel>
        <asp:Panel ID="ConfirmacionPanel" runat="server" BackColor="#E4E4E4" Height="60">
            <asp:Label ID="Label5" runat="server" Width="883px"></asp:Label>
            <asp:Button ID="confirmacionBTN" runat="server" Text="Confirmar Compra" BackColor="#009900" BorderColor="#006600" BorderStyle="Solid" BorderWidth="5px" Font-Bold="True" Font-Size="14pt" Height="38px" Width="239px" OnClick="confirmacionBTN_Click" />

        </asp:Panel>
    </div>
</asp:Content>
