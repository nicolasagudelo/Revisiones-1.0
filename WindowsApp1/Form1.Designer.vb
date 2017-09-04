<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class MainForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim DataGridViewCellStyle19 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle20 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle21 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle22 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle23 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle24 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me.BtnNuevoRegistro = New System.Windows.Forms.Button()
        Me.BtnModificarRegistro = New System.Windows.Forms.Button()
        Me.DGVAdmin = New System.Windows.Forms.DataGridView()
        Me.TxtBxFiltroRegistro = New System.Windows.Forms.TextBox()
        Me.BtnEliminarRegistro = New System.Windows.Forms.Button()
        Me.CbBxTablas = New System.Windows.Forms.ComboBox()
        Me.LabelTituloFiltroRegistro = New System.Windows.Forms.Label()
        Me.GroupBoxControlesTablas = New System.Windows.Forms.GroupBox()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPageRevisionesXPrueba = New System.Windows.Forms.TabPage()
        Me.TabPageMuestras = New System.Windows.Forms.TabPage()
        Me.GroupBoxMuestras = New System.Windows.Forms.GroupBox()
        Me.LabelValorC2Muestra = New System.Windows.Forms.Label()
        Me.LabelValorC1Muestra = New System.Windows.Forms.Label()
        Me.TxBxValorC2Muestra = New System.Windows.Forms.TextBox()
        Me.BtnAsignarMuestras = New System.Windows.Forms.Button()
        Me.TxBxValorC1Muestra = New System.Windows.Forms.TextBox()
        Me.LabelMuestras = New System.Windows.Forms.Label()
        Me.DGVMuestras = New System.Windows.Forms.DataGridView()
        Me.TabPageBandejas = New System.Windows.Forms.TabPage()
        Me.GroupBoxBandejas = New System.Windows.Forms.GroupBox()
        Me.LabelComentario1Bandeja = New System.Windows.Forms.Label()
        Me.LabelComentario2Bandeja = New System.Windows.Forms.Label()
        Me.TxBxComentario1Bandeja = New System.Windows.Forms.TextBox()
        Me.BtnAsignarBandejas = New System.Windows.Forms.Button()
        Me.TxBxComentario2Bandeja = New System.Windows.Forms.TextBox()
        Me.LabelBandejas = New System.Windows.Forms.Label()
        Me.DGVBandejas = New System.Windows.Forms.DataGridView()
        Me.TabPageAdmin = New System.Windows.Forms.TabPage()
        Me.GroupBoxAdmin = New System.Windows.Forms.GroupBox()
        Me.PanelAdmin = New System.Windows.Forms.Panel()
        Me.ComboBox5 = New System.Windows.Forms.ComboBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.ComboBox4 = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.ComboBox9 = New System.Windows.Forms.ComboBox()
        Me.ComboBox8 = New System.Windows.Forms.ComboBox()
        Me.ComboBox3 = New System.Windows.Forms.ComboBox()
        Me.ComboBox2 = New System.Windows.Forms.ComboBox()
        Me.TextBox6 = New System.Windows.Forms.TextBox()
        Me.TextBox3 = New System.Windows.Forms.TextBox()
        Me.BtnModificar = New System.Windows.Forms.Button()
        Me.ButtonAgregar = New System.Windows.Forms.Button()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.LabelAdmin = New System.Windows.Forms.Label()
        Me.CmbBxAnalistas = New System.Windows.Forms.ComboBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.LabelTimerVerificacion = New System.Windows.Forms.Label()
        Me.LabelTimerAsignacion = New System.Windows.Forms.Label()
        Me.BtnConectarAdmin = New System.Windows.Forms.Button()
        Me.BtnDesconectar = New System.Windows.Forms.Button()
        Me.LabelTimerTotal = New System.Windows.Forms.Label()
        Me.BtnConectar = New System.Windows.Forms.Button()
        Me.BtnFiltroPrueba = New System.Windows.Forms.Button()
        Me.CmbBxFiltroPrueba = New System.Windows.Forms.ComboBox()
        Me.PrintDocument1 = New System.Drawing.Printing.PrintDocument()
        Me.LabelRevisionesPrueba = New System.Windows.Forms.Label()
        Me.TextBoxContraseña = New System.Windows.Forms.TextBox()
        Me.TextBoxRespuestaForm2 = New System.Windows.Forms.TextBox()
        Me.Timer2 = New System.Windows.Forms.Timer(Me.components)
        Me.BtnRecargar = New System.Windows.Forms.Button()
        Me.btnPreviewPrint = New System.Windows.Forms.Button()
        Me.BtnSi = New System.Windows.Forms.Button()
        Me.BtnNo = New System.Windows.Forms.Button()
        CType(Me.DGVAdmin, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBoxControlesTablas.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.TabPageMuestras.SuspendLayout()
        Me.GroupBoxMuestras.SuspendLayout()
        CType(Me.DGVMuestras, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPageBandejas.SuspendLayout()
        Me.GroupBoxBandejas.SuspendLayout()
        CType(Me.DGVBandejas, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPageAdmin.SuspendLayout()
        Me.GroupBoxAdmin.SuspendLayout()
        Me.PanelAdmin.SuspendLayout()
        Me.SuspendLayout()
        '
        'BtnNuevoRegistro
        '
        Me.BtnNuevoRegistro.Enabled = False
        Me.BtnNuevoRegistro.Location = New System.Drawing.Point(11, 17)
        Me.BtnNuevoRegistro.Name = "BtnNuevoRegistro"
        Me.BtnNuevoRegistro.Size = New System.Drawing.Size(89, 39)
        Me.BtnNuevoRegistro.TabIndex = 1
        Me.BtnNuevoRegistro.Text = "Nuevo Registro"
        Me.BtnNuevoRegistro.UseVisualStyleBackColor = True
        '
        'BtnModificarRegistro
        '
        Me.BtnModificarRegistro.Enabled = False
        Me.BtnModificarRegistro.Location = New System.Drawing.Point(106, 17)
        Me.BtnModificarRegistro.Name = "BtnModificarRegistro"
        Me.BtnModificarRegistro.Size = New System.Drawing.Size(89, 39)
        Me.BtnModificarRegistro.TabIndex = 2
        Me.BtnModificarRegistro.Text = "Modificar Registro"
        Me.BtnModificarRegistro.UseVisualStyleBackColor = True
        '
        'DGVAdmin
        '
        DataGridViewCellStyle19.ForeColor = System.Drawing.SystemColors.WindowText
        Me.DGVAdmin.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle19
        Me.DGVAdmin.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DGVAdmin.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.DGVAdmin.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells
        Me.DGVAdmin.BackgroundColor = System.Drawing.Color.Azure
        Me.DGVAdmin.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.DGVAdmin.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.[Single]
        DataGridViewCellStyle20.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle20.BackColor = System.Drawing.SystemColors.ActiveBorder
        DataGridViewCellStyle20.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte), True)
        DataGridViewCellStyle20.ForeColor = System.Drawing.Color.Coral
        DataGridViewCellStyle20.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle20.SelectionForeColor = System.Drawing.SystemColors.ControlDark
        DataGridViewCellStyle20.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DGVAdmin.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle20
        Me.DGVAdmin.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DGVAdmin.Cursor = System.Windows.Forms.Cursors.Hand
        Me.DGVAdmin.GridColor = System.Drawing.Color.DarkRed
        Me.DGVAdmin.ImeMode = System.Windows.Forms.ImeMode.[On]
        Me.DGVAdmin.Location = New System.Drawing.Point(16, 30)
        Me.DGVAdmin.Name = "DGVAdmin"
        Me.DGVAdmin.Size = New System.Drawing.Size(1023, 299)
        Me.DGVAdmin.TabIndex = 3
        '
        'TxtBxFiltroRegistro
        '
        Me.TxtBxFiltroRegistro.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.TxtBxFiltroRegistro.Location = New System.Drawing.Point(1121, 478)
        Me.TxtBxFiltroRegistro.Name = "TxtBxFiltroRegistro"
        Me.TxtBxFiltroRegistro.Size = New System.Drawing.Size(132, 20)
        Me.TxtBxFiltroRegistro.TabIndex = 25
        Me.TxtBxFiltroRegistro.Visible = False
        '
        'BtnEliminarRegistro
        '
        Me.BtnEliminarRegistro.Enabled = False
        Me.BtnEliminarRegistro.Location = New System.Drawing.Point(201, 17)
        Me.BtnEliminarRegistro.Name = "BtnEliminarRegistro"
        Me.BtnEliminarRegistro.Size = New System.Drawing.Size(78, 39)
        Me.BtnEliminarRegistro.TabIndex = 3
        Me.BtnEliminarRegistro.Text = "Eliminar Registro"
        Me.BtnEliminarRegistro.UseVisualStyleBackColor = True
        '
        'CbBxTablas
        '
        Me.CbBxTablas.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.CbBxTablas.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.CbBxTablas.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CbBxTablas.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.CbBxTablas.FormattingEnabled = True
        Me.CbBxTablas.Location = New System.Drawing.Point(16, 384)
        Me.CbBxTablas.Name = "CbBxTablas"
        Me.CbBxTablas.Size = New System.Drawing.Size(184, 21)
        Me.CbBxTablas.TabIndex = 9
        '
        'LabelTituloFiltroRegistro
        '
        Me.LabelTituloFiltroRegistro.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.LabelTituloFiltroRegistro.AutoSize = True
        Me.LabelTituloFiltroRegistro.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelTituloFiltroRegistro.Location = New System.Drawing.Point(1118, 454)
        Me.LabelTituloFiltroRegistro.Name = "LabelTituloFiltroRegistro"
        Me.LabelTituloFiltroRegistro.Size = New System.Drawing.Size(0, 15)
        Me.LabelTituloFiltroRegistro.TabIndex = 13
        Me.LabelTituloFiltroRegistro.Visible = False
        '
        'GroupBoxControlesTablas
        '
        Me.GroupBoxControlesTablas.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.GroupBoxControlesTablas.Controls.Add(Me.BtnEliminarRegistro)
        Me.GroupBoxControlesTablas.Controls.Add(Me.BtnModificarRegistro)
        Me.GroupBoxControlesTablas.Controls.Add(Me.BtnNuevoRegistro)
        Me.GroupBoxControlesTablas.Location = New System.Drawing.Point(225, 373)
        Me.GroupBoxControlesTablas.Name = "GroupBoxControlesTablas"
        Me.GroupBoxControlesTablas.Size = New System.Drawing.Size(285, 68)
        Me.GroupBoxControlesTablas.TabIndex = 19
        Me.GroupBoxControlesTablas.TabStop = False
        Me.GroupBoxControlesTablas.Visible = False
        '
        'Timer1
        '
        Me.Timer1.Enabled = True
        Me.Timer1.Interval = 300000
        '
        'TabControl1
        '
        Me.TabControl1.Alignment = System.Windows.Forms.TabAlignment.Left
        Me.TabControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TabControl1.Controls.Add(Me.TabPageRevisionesXPrueba)
        Me.TabControl1.Controls.Add(Me.TabPageMuestras)
        Me.TabControl1.Controls.Add(Me.TabPageBandejas)
        Me.TabControl1.Controls.Add(Me.TabPageAdmin)
        Me.TabControl1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte), True)
        Me.TabControl1.Location = New System.Drawing.Point(16, 30)
        Me.TabControl1.Margin = New System.Windows.Forms.Padding(1)
        Me.TabControl1.Multiline = True
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(1077, 464)
        Me.TabControl1.TabIndex = 20
        '
        'TabPageRevisionesXPrueba
        '
        Me.TabPageRevisionesXPrueba.BackColor = System.Drawing.Color.Azure
        Me.TabPageRevisionesXPrueba.Location = New System.Drawing.Point(25, 4)
        Me.TabPageRevisionesXPrueba.Name = "TabPageRevisionesXPrueba"
        Me.TabPageRevisionesXPrueba.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPageRevisionesXPrueba.Size = New System.Drawing.Size(1048, 456)
        Me.TabPageRevisionesXPrueba.TabIndex = 3
        Me.TabPageRevisionesXPrueba.Text = "Revisiones X prueba"
        '
        'TabPageMuestras
        '
        Me.TabPageMuestras.BackColor = System.Drawing.Color.Azure
        Me.TabPageMuestras.Controls.Add(Me.GroupBoxMuestras)
        Me.TabPageMuestras.Controls.Add(Me.LabelMuestras)
        Me.TabPageMuestras.Controls.Add(Me.DGVMuestras)
        Me.TabPageMuestras.Location = New System.Drawing.Point(25, 4)
        Me.TabPageMuestras.Name = "TabPageMuestras"
        Me.TabPageMuestras.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPageMuestras.Size = New System.Drawing.Size(1048, 456)
        Me.TabPageMuestras.TabIndex = 1
        Me.TabPageMuestras.Text = "Revisión Muestras"
        '
        'GroupBoxMuestras
        '
        Me.GroupBoxMuestras.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.GroupBoxMuestras.Controls.Add(Me.LabelValorC2Muestra)
        Me.GroupBoxMuestras.Controls.Add(Me.LabelValorC1Muestra)
        Me.GroupBoxMuestras.Controls.Add(Me.TxBxValorC2Muestra)
        Me.GroupBoxMuestras.Controls.Add(Me.BtnAsignarMuestras)
        Me.GroupBoxMuestras.Controls.Add(Me.TxBxValorC1Muestra)
        Me.GroupBoxMuestras.Location = New System.Drawing.Point(383, 345)
        Me.GroupBoxMuestras.Name = "GroupBoxMuestras"
        Me.GroupBoxMuestras.Size = New System.Drawing.Size(461, 96)
        Me.GroupBoxMuestras.TabIndex = 37
        Me.GroupBoxMuestras.TabStop = False
        Me.GroupBoxMuestras.Visible = False
        '
        'LabelValorC2Muestra
        '
        Me.LabelValorC2Muestra.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.LabelValorC2Muestra.AutoSize = True
        Me.LabelValorC2Muestra.Location = New System.Drawing.Point(162, 27)
        Me.LabelValorC2Muestra.Name = "LabelValorC2Muestra"
        Me.LabelValorC2Muestra.Size = New System.Drawing.Size(50, 13)
        Me.LabelValorC2Muestra.TabIndex = 27
        Me.LabelValorC2Muestra.Text = "Valor_C2"
        Me.LabelValorC2Muestra.Visible = False
        '
        'LabelValorC1Muestra
        '
        Me.LabelValorC1Muestra.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.LabelValorC1Muestra.AutoSize = True
        Me.LabelValorC1Muestra.Location = New System.Drawing.Point(17, 27)
        Me.LabelValorC1Muestra.Name = "LabelValorC1Muestra"
        Me.LabelValorC1Muestra.Size = New System.Drawing.Size(50, 13)
        Me.LabelValorC1Muestra.TabIndex = 26
        Me.LabelValorC1Muestra.Text = "Valor_C1"
        Me.LabelValorC1Muestra.Visible = False
        '
        'TxBxValorC2Muestra
        '
        Me.TxBxValorC2Muestra.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.TxBxValorC2Muestra.Location = New System.Drawing.Point(165, 52)
        Me.TxBxValorC2Muestra.Name = "TxBxValorC2Muestra"
        Me.TxBxValorC2Muestra.Size = New System.Drawing.Size(110, 20)
        Me.TxBxValorC2Muestra.TabIndex = 20
        Me.TxBxValorC2Muestra.Visible = False
        '
        'BtnAsignarMuestras
        '
        Me.BtnAsignarMuestras.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.BtnAsignarMuestras.Enabled = False
        Me.BtnAsignarMuestras.Location = New System.Drawing.Point(323, 27)
        Me.BtnAsignarMuestras.Name = "BtnAsignarMuestras"
        Me.BtnAsignarMuestras.Size = New System.Drawing.Size(110, 45)
        Me.BtnAsignarMuestras.TabIndex = 21
        Me.BtnAsignarMuestras.Text = "Asignar e ingresar datos"
        Me.BtnAsignarMuestras.UseVisualStyleBackColor = True
        Me.BtnAsignarMuestras.Visible = False
        '
        'TxBxValorC1Muestra
        '
        Me.TxBxValorC1Muestra.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.TxBxValorC1Muestra.Location = New System.Drawing.Point(20, 52)
        Me.TxBxValorC1Muestra.Name = "TxBxValorC1Muestra"
        Me.TxBxValorC1Muestra.Size = New System.Drawing.Size(110, 20)
        Me.TxBxValorC1Muestra.TabIndex = 19
        Me.TxBxValorC1Muestra.Visible = False
        '
        'LabelMuestras
        '
        Me.LabelMuestras.AutoSize = True
        Me.LabelMuestras.Font = New System.Drawing.Font("Cambria", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelMuestras.ForeColor = System.Drawing.SystemColors.MenuHighlight
        Me.LabelMuestras.Location = New System.Drawing.Point(31, 8)
        Me.LabelMuestras.Name = "LabelMuestras"
        Me.LabelMuestras.Size = New System.Drawing.Size(77, 19)
        Me.LabelMuestras.TabIndex = 36
        Me.LabelMuestras.Text = "Muestras"
        '
        'DGVMuestras
        '
        DataGridViewCellStyle21.ForeColor = System.Drawing.SystemColors.WindowText
        Me.DGVMuestras.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle21
        Me.DGVMuestras.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DGVMuestras.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.DGVMuestras.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells
        Me.DGVMuestras.BackgroundColor = System.Drawing.Color.Azure
        Me.DGVMuestras.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.DGVMuestras.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.[Single]
        DataGridViewCellStyle22.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle22.BackColor = System.Drawing.SystemColors.ActiveBorder
        DataGridViewCellStyle22.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte), True)
        DataGridViewCellStyle22.ForeColor = System.Drawing.Color.Coral
        DataGridViewCellStyle22.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle22.SelectionForeColor = System.Drawing.SystemColors.ControlDark
        DataGridViewCellStyle22.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DGVMuestras.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle22
        Me.DGVMuestras.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DGVMuestras.Cursor = System.Windows.Forms.Cursors.Hand
        Me.DGVMuestras.GridColor = System.Drawing.Color.DarkRed
        Me.DGVMuestras.ImeMode = System.Windows.Forms.ImeMode.[On]
        Me.DGVMuestras.Location = New System.Drawing.Point(16, 30)
        Me.DGVMuestras.Name = "DGVMuestras"
        Me.DGVMuestras.Size = New System.Drawing.Size(1011, 309)
        Me.DGVMuestras.TabIndex = 4
        '
        'TabPageBandejas
        '
        Me.TabPageBandejas.BackColor = System.Drawing.Color.Azure
        Me.TabPageBandejas.Controls.Add(Me.GroupBoxBandejas)
        Me.TabPageBandejas.Controls.Add(Me.LabelBandejas)
        Me.TabPageBandejas.Controls.Add(Me.DGVBandejas)
        Me.TabPageBandejas.Location = New System.Drawing.Point(25, 4)
        Me.TabPageBandejas.Name = "TabPageBandejas"
        Me.TabPageBandejas.Size = New System.Drawing.Size(1048, 456)
        Me.TabPageBandejas.TabIndex = 2
        Me.TabPageBandejas.Text = "Revisión Bandejas"
        '
        'GroupBoxBandejas
        '
        Me.GroupBoxBandejas.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.GroupBoxBandejas.Controls.Add(Me.LabelComentario1Bandeja)
        Me.GroupBoxBandejas.Controls.Add(Me.LabelComentario2Bandeja)
        Me.GroupBoxBandejas.Controls.Add(Me.TxBxComentario1Bandeja)
        Me.GroupBoxBandejas.Controls.Add(Me.BtnAsignarBandejas)
        Me.GroupBoxBandejas.Controls.Add(Me.TxBxComentario2Bandeja)
        Me.GroupBoxBandejas.Location = New System.Drawing.Point(383, 345)
        Me.GroupBoxBandejas.Name = "GroupBoxBandejas"
        Me.GroupBoxBandejas.Size = New System.Drawing.Size(461, 96)
        Me.GroupBoxBandejas.TabIndex = 36
        Me.GroupBoxBandejas.TabStop = False
        Me.GroupBoxBandejas.Visible = False
        '
        'LabelComentario1Bandeja
        '
        Me.LabelComentario1Bandeja.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.LabelComentario1Bandeja.AutoSize = True
        Me.LabelComentario1Bandeja.Location = New System.Drawing.Point(17, 27)
        Me.LabelComentario1Bandeja.Name = "LabelComentario1Bandeja"
        Me.LabelComentario1Bandeja.Size = New System.Drawing.Size(107, 13)
        Me.LabelComentario1Bandeja.TabIndex = 34
        Me.LabelComentario1Bandeja.Text = "Comentario_Revisión"
        Me.LabelComentario1Bandeja.Visible = False
        '
        'LabelComentario2Bandeja
        '
        Me.LabelComentario2Bandeja.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.LabelComentario2Bandeja.AutoSize = True
        Me.LabelComentario2Bandeja.Location = New System.Drawing.Point(162, 27)
        Me.LabelComentario2Bandeja.Name = "LabelComentario2Bandeja"
        Me.LabelComentario2Bandeja.Size = New System.Drawing.Size(72, 13)
        Me.LabelComentario2Bandeja.TabIndex = 33
        Me.LabelComentario2Bandeja.Text = "Comentario_2"
        Me.LabelComentario2Bandeja.Visible = False
        '
        'TxBxComentario1Bandeja
        '
        Me.TxBxComentario1Bandeja.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.TxBxComentario1Bandeja.Location = New System.Drawing.Point(20, 52)
        Me.TxBxComentario1Bandeja.Name = "TxBxComentario1Bandeja"
        Me.TxBxComentario1Bandeja.Size = New System.Drawing.Size(110, 20)
        Me.TxBxComentario1Bandeja.TabIndex = 19
        '
        'BtnAsignarBandejas
        '
        Me.BtnAsignarBandejas.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.BtnAsignarBandejas.Enabled = False
        Me.BtnAsignarBandejas.Location = New System.Drawing.Point(323, 27)
        Me.BtnAsignarBandejas.Name = "BtnAsignarBandejas"
        Me.BtnAsignarBandejas.Size = New System.Drawing.Size(110, 45)
        Me.BtnAsignarBandejas.TabIndex = 21
        Me.BtnAsignarBandejas.Text = "Asignar e ingresar datos"
        Me.BtnAsignarBandejas.UseVisualStyleBackColor = True
        '
        'TxBxComentario2Bandeja
        '
        Me.TxBxComentario2Bandeja.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.TxBxComentario2Bandeja.Location = New System.Drawing.Point(165, 52)
        Me.TxBxComentario2Bandeja.Name = "TxBxComentario2Bandeja"
        Me.TxBxComentario2Bandeja.Size = New System.Drawing.Size(110, 20)
        Me.TxBxComentario2Bandeja.TabIndex = 20
        '
        'LabelBandejas
        '
        Me.LabelBandejas.AutoSize = True
        Me.LabelBandejas.Font = New System.Drawing.Font("Cambria", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelBandejas.ForeColor = System.Drawing.SystemColors.MenuHighlight
        Me.LabelBandejas.Location = New System.Drawing.Point(35, 9)
        Me.LabelBandejas.Name = "LabelBandejas"
        Me.LabelBandejas.Size = New System.Drawing.Size(77, 19)
        Me.LabelBandejas.TabIndex = 35
        Me.LabelBandejas.Text = "Bandejas"
        '
        'DGVBandejas
        '
        DataGridViewCellStyle23.ForeColor = System.Drawing.SystemColors.WindowText
        Me.DGVBandejas.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle23
        Me.DGVBandejas.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DGVBandejas.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.DGVBandejas.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells
        Me.DGVBandejas.BackgroundColor = System.Drawing.Color.Azure
        Me.DGVBandejas.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.DGVBandejas.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.[Single]
        DataGridViewCellStyle24.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle24.BackColor = System.Drawing.SystemColors.ActiveBorder
        DataGridViewCellStyle24.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte), True)
        DataGridViewCellStyle24.ForeColor = System.Drawing.Color.Coral
        DataGridViewCellStyle24.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle24.SelectionForeColor = System.Drawing.SystemColors.ControlDark
        DataGridViewCellStyle24.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DGVBandejas.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle24
        Me.DGVBandejas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DGVBandejas.Cursor = System.Windows.Forms.Cursors.Hand
        Me.DGVBandejas.GridColor = System.Drawing.Color.DarkRed
        Me.DGVBandejas.ImeMode = System.Windows.Forms.ImeMode.[On]
        Me.DGVBandejas.Location = New System.Drawing.Point(13, 31)
        Me.DGVBandejas.Name = "DGVBandejas"
        Me.DGVBandejas.Size = New System.Drawing.Size(1018, 316)
        Me.DGVBandejas.TabIndex = 4
        '
        'TabPageAdmin
        '
        Me.TabPageAdmin.AccessibleRole = System.Windows.Forms.AccessibleRole.Window
        Me.TabPageAdmin.BackColor = System.Drawing.Color.Azure
        Me.TabPageAdmin.Controls.Add(Me.GroupBoxAdmin)
        Me.TabPageAdmin.Controls.Add(Me.LabelAdmin)
        Me.TabPageAdmin.Controls.Add(Me.DGVAdmin)
        Me.TabPageAdmin.Controls.Add(Me.GroupBoxControlesTablas)
        Me.TabPageAdmin.Controls.Add(Me.CbBxTablas)
        Me.TabPageAdmin.Location = New System.Drawing.Point(25, 4)
        Me.TabPageAdmin.Name = "TabPageAdmin"
        Me.TabPageAdmin.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPageAdmin.Size = New System.Drawing.Size(1048, 456)
        Me.TabPageAdmin.TabIndex = 0
        Me.TabPageAdmin.Text = "Administrador"
        '
        'GroupBoxAdmin
        '
        Me.GroupBoxAdmin.Controls.Add(Me.BtnSi)
        Me.GroupBoxAdmin.Controls.Add(Me.BtnNo)
        Me.GroupBoxAdmin.Controls.Add(Me.PanelAdmin)
        Me.GroupBoxAdmin.Controls.Add(Me.ComboBox9)
        Me.GroupBoxAdmin.Controls.Add(Me.ComboBox8)
        Me.GroupBoxAdmin.Controls.Add(Me.ComboBox3)
        Me.GroupBoxAdmin.Controls.Add(Me.ComboBox2)
        Me.GroupBoxAdmin.Controls.Add(Me.TextBox6)
        Me.GroupBoxAdmin.Controls.Add(Me.TextBox3)
        Me.GroupBoxAdmin.Controls.Add(Me.BtnModificar)
        Me.GroupBoxAdmin.Controls.Add(Me.ButtonAgregar)
        Me.GroupBoxAdmin.Controls.Add(Me.TextBox2)
        Me.GroupBoxAdmin.Controls.Add(Me.Label2)
        Me.GroupBoxAdmin.Controls.Add(Me.TextBox1)
        Me.GroupBoxAdmin.Controls.Add(Me.Label1)
        Me.GroupBoxAdmin.Location = New System.Drawing.Point(542, 326)
        Me.GroupBoxAdmin.Name = "GroupBoxAdmin"
        Me.GroupBoxAdmin.Size = New System.Drawing.Size(487, 115)
        Me.GroupBoxAdmin.TabIndex = 37
        Me.GroupBoxAdmin.TabStop = False
        Me.GroupBoxAdmin.Visible = False
        '
        'PanelAdmin
        '
        Me.PanelAdmin.Controls.Add(Me.ComboBox5)
        Me.PanelAdmin.Controls.Add(Me.Label6)
        Me.PanelAdmin.Controls.Add(Me.ComboBox4)
        Me.PanelAdmin.Controls.Add(Me.Label3)
        Me.PanelAdmin.Location = New System.Drawing.Point(149, 11)
        Me.PanelAdmin.Name = "PanelAdmin"
        Me.PanelAdmin.Size = New System.Drawing.Size(200, 98)
        Me.PanelAdmin.TabIndex = 14
        '
        'ComboBox5
        '
        Me.ComboBox5.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox5.FormattingEnabled = True
        Me.ComboBox5.Location = New System.Drawing.Point(28, 26)
        Me.ComboBox5.Name = "ComboBox5"
        Me.ComboBox5.Size = New System.Drawing.Size(121, 21)
        Me.ComboBox5.TabIndex = 13
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(25, 5)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(39, 13)
        Me.Label6.TabIndex = 12
        Me.Label6.Text = "Label6"
        '
        'ComboBox4
        '
        Me.ComboBox4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox4.FormattingEnabled = True
        Me.ComboBox4.Location = New System.Drawing.Point(28, 76)
        Me.ComboBox4.Name = "ComboBox4"
        Me.ComboBox4.Size = New System.Drawing.Size(121, 21)
        Me.ComboBox4.TabIndex = 11
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(25, 55)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(39, 13)
        Me.Label3.TabIndex = 10
        Me.Label3.Text = "Label3"
        '
        'ComboBox9
        '
        Me.ComboBox9.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox9.FormattingEnabled = True
        Me.ComboBox9.Location = New System.Drawing.Point(9, 86)
        Me.ComboBox9.Name = "ComboBox9"
        Me.ComboBox9.Size = New System.Drawing.Size(121, 21)
        Me.ComboBox9.TabIndex = 16
        '
        'ComboBox8
        '
        Me.ComboBox8.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox8.FormattingEnabled = True
        Me.ComboBox8.Location = New System.Drawing.Point(9, 37)
        Me.ComboBox8.Name = "ComboBox8"
        Me.ComboBox8.Size = New System.Drawing.Size(121, 21)
        Me.ComboBox8.TabIndex = 15
        '
        'ComboBox3
        '
        Me.ComboBox3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox3.FormattingEnabled = True
        Me.ComboBox3.Location = New System.Drawing.Point(9, 86)
        Me.ComboBox3.Name = "ComboBox3"
        Me.ComboBox3.Size = New System.Drawing.Size(121, 21)
        Me.ComboBox3.TabIndex = 9
        '
        'ComboBox2
        '
        Me.ComboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox2.FormattingEnabled = True
        Me.ComboBox2.Location = New System.Drawing.Point(9, 37)
        Me.ComboBox2.Name = "ComboBox2"
        Me.ComboBox2.Size = New System.Drawing.Size(121, 21)
        Me.ComboBox2.TabIndex = 8
        '
        'TextBox6
        '
        Me.TextBox6.Location = New System.Drawing.Point(9, 87)
        Me.TextBox6.Name = "TextBox6"
        Me.TextBox6.Size = New System.Drawing.Size(100, 20)
        Me.TextBox6.TabIndex = 7
        '
        'TextBox3
        '
        Me.TextBox3.Location = New System.Drawing.Point(9, 37)
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.Size = New System.Drawing.Size(100, 20)
        Me.TextBox3.TabIndex = 6
        '
        'BtnModificar
        '
        Me.BtnModificar.Location = New System.Drawing.Point(396, 70)
        Me.BtnModificar.Name = "BtnModificar"
        Me.BtnModificar.Size = New System.Drawing.Size(85, 39)
        Me.BtnModificar.TabIndex = 5
        Me.BtnModificar.Text = "Modificar"
        Me.BtnModificar.UseVisualStyleBackColor = True
        '
        'ButtonAgregar
        '
        Me.ButtonAgregar.Location = New System.Drawing.Point(396, 70)
        Me.ButtonAgregar.Name = "ButtonAgregar"
        Me.ButtonAgregar.Size = New System.Drawing.Size(85, 39)
        Me.ButtonAgregar.TabIndex = 4
        Me.ButtonAgregar.Text = "Agregar"
        Me.ButtonAgregar.UseVisualStyleBackColor = True
        '
        'TextBox2
        '
        Me.TextBox2.Location = New System.Drawing.Point(9, 87)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.TextBox2.Size = New System.Drawing.Size(100, 20)
        Me.TextBox2.TabIndex = 3
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(6, 66)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(39, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Label2"
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(9, 37)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(100, 20)
        Me.TextBox1.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 17)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(39, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Label1"
        '
        'LabelAdmin
        '
        Me.LabelAdmin.AutoSize = True
        Me.LabelAdmin.Font = New System.Drawing.Font("Cambria", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelAdmin.ForeColor = System.Drawing.SystemColors.MenuHighlight
        Me.LabelAdmin.Location = New System.Drawing.Point(38, -1)
        Me.LabelAdmin.Name = "LabelAdmin"
        Me.LabelAdmin.Size = New System.Drawing.Size(118, 19)
        Me.LabelAdmin.TabIndex = 36
        Me.LabelAdmin.Text = "Administrador"
        '
        'CmbBxAnalistas
        '
        Me.CmbBxAnalistas.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.CmbBxAnalistas.Enabled = False
        Me.CmbBxAnalistas.FormattingEnabled = True
        Me.CmbBxAnalistas.Location = New System.Drawing.Point(1121, 310)
        Me.CmbBxAnalistas.Name = "CmbBxAnalistas"
        Me.CmbBxAnalistas.Size = New System.Drawing.Size(132, 21)
        Me.CmbBxAnalistas.TabIndex = 35
        '
        'Label5
        '
        Me.Label5.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(1118, 273)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(67, 18)
        Me.Label5.TabIndex = 22
        Me.Label5.Text = "Analista"
        Me.Label5.Visible = False
        '
        'LabelTimerVerificacion
        '
        Me.LabelTimerVerificacion.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.LabelTimerVerificacion.AutoSize = True
        Me.LabelTimerVerificacion.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelTimerVerificacion.ForeColor = System.Drawing.Color.Blue
        Me.LabelTimerVerificacion.Location = New System.Drawing.Point(392, 520)
        Me.LabelTimerVerificacion.Name = "LabelTimerVerificacion"
        Me.LabelTimerVerificacion.Size = New System.Drawing.Size(17, 18)
        Me.LabelTimerVerificacion.TabIndex = 28
        Me.LabelTimerVerificacion.Text = "2"
        Me.LabelTimerVerificacion.Visible = False
        '
        'LabelTimerAsignacion
        '
        Me.LabelTimerAsignacion.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.LabelTimerAsignacion.AutoSize = True
        Me.LabelTimerAsignacion.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelTimerAsignacion.ForeColor = System.Drawing.Color.Blue
        Me.LabelTimerAsignacion.Location = New System.Drawing.Point(48, 520)
        Me.LabelTimerAsignacion.Name = "LabelTimerAsignacion"
        Me.LabelTimerAsignacion.Size = New System.Drawing.Size(17, 18)
        Me.LabelTimerAsignacion.TabIndex = 35
        Me.LabelTimerAsignacion.Text = "1"
        Me.LabelTimerAsignacion.Visible = False
        '
        'BtnConectarAdmin
        '
        Me.BtnConectarAdmin.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.BtnConectarAdmin.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnConectarAdmin.Location = New System.Drawing.Point(1121, 391)
        Me.BtnConectarAdmin.Name = "BtnConectarAdmin"
        Me.BtnConectarAdmin.Size = New System.Drawing.Size(132, 33)
        Me.BtnConectarAdmin.TabIndex = 24
        Me.BtnConectarAdmin.Text = "Administrador"
        Me.BtnConectarAdmin.UseVisualStyleBackColor = True
        '
        'BtnDesconectar
        '
        Me.BtnDesconectar.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.BtnDesconectar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnDesconectar.ForeColor = System.Drawing.Color.Turquoise
        Me.BtnDesconectar.Image = CType(resources.GetObject("BtnDesconectar.Image"), System.Drawing.Image)
        Me.BtnDesconectar.Location = New System.Drawing.Point(1191, 45)
        Me.BtnDesconectar.Name = "BtnDesconectar"
        Me.BtnDesconectar.Size = New System.Drawing.Size(62, 48)
        Me.BtnDesconectar.TabIndex = 21
        Me.BtnDesconectar.UseVisualStyleBackColor = True
        '
        'LabelTimerTotal
        '
        Me.LabelTimerTotal.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.LabelTimerTotal.AutoSize = True
        Me.LabelTimerTotal.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelTimerTotal.ForeColor = System.Drawing.Color.Blue
        Me.LabelTimerTotal.Location = New System.Drawing.Point(731, 520)
        Me.LabelTimerTotal.Name = "LabelTimerTotal"
        Me.LabelTimerTotal.Size = New System.Drawing.Size(17, 18)
        Me.LabelTimerTotal.TabIndex = 22
        Me.LabelTimerTotal.Text = "3"
        Me.LabelTimerTotal.Visible = False
        '
        'BtnConectar
        '
        Me.BtnConectar.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.BtnConectar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnConectar.Location = New System.Drawing.Point(1121, 337)
        Me.BtnConectar.Name = "BtnConectar"
        Me.BtnConectar.Size = New System.Drawing.Size(132, 28)
        Me.BtnConectar.TabIndex = 36
        Me.BtnConectar.Text = "Conectar"
        Me.BtnConectar.UseVisualStyleBackColor = True
        '
        'BtnFiltroPrueba
        '
        Me.BtnFiltroPrueba.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.BtnFiltroPrueba.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnFiltroPrueba.Location = New System.Drawing.Point(1121, 144)
        Me.BtnFiltroPrueba.Name = "BtnFiltroPrueba"
        Me.BtnFiltroPrueba.Size = New System.Drawing.Size(132, 30)
        Me.BtnFiltroPrueba.TabIndex = 37
        Me.BtnFiltroPrueba.Text = "Filtrar por Prueba"
        Me.BtnFiltroPrueba.UseVisualStyleBackColor = True
        Me.BtnFiltroPrueba.Visible = False
        '
        'CmbBxFiltroPrueba
        '
        Me.CmbBxFiltroPrueba.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.CmbBxFiltroPrueba.FormattingEnabled = True
        Me.CmbBxFiltroPrueba.Location = New System.Drawing.Point(1121, 113)
        Me.CmbBxFiltroPrueba.Name = "CmbBxFiltroPrueba"
        Me.CmbBxFiltroPrueba.Size = New System.Drawing.Size(132, 21)
        Me.CmbBxFiltroPrueba.TabIndex = 38
        Me.CmbBxFiltroPrueba.Visible = False
        '
        'PrintDocument1
        '
        '
        'LabelRevisionesPrueba
        '
        Me.LabelRevisionesPrueba.AutoSize = True
        Me.LabelRevisionesPrueba.Font = New System.Drawing.Font("Cambria", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelRevisionesPrueba.ForeColor = System.Drawing.SystemColors.MenuHighlight
        Me.LabelRevisionesPrueba.Location = New System.Drawing.Point(62, 1)
        Me.LabelRevisionesPrueba.Name = "LabelRevisionesPrueba"
        Me.LabelRevisionesPrueba.Size = New System.Drawing.Size(308, 22)
        Me.LabelRevisionesPrueba.TabIndex = 39
        Me.LabelRevisionesPrueba.Text = "Cantidad de revisiones por prueba"
        '
        'TextBoxContraseña
        '
        Me.TextBoxContraseña.Location = New System.Drawing.Point(1114, 5)
        Me.TextBoxContraseña.Name = "TextBoxContraseña"
        Me.TextBoxContraseña.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.TextBoxContraseña.Size = New System.Drawing.Size(100, 20)
        Me.TextBoxContraseña.TabIndex = 40
        Me.TextBoxContraseña.Visible = False
        '
        'TextBoxRespuestaForm2
        '
        Me.TextBoxRespuestaForm2.Location = New System.Drawing.Point(1230, 5)
        Me.TextBoxRespuestaForm2.Name = "TextBoxRespuestaForm2"
        Me.TextBoxRespuestaForm2.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.TextBoxRespuestaForm2.Size = New System.Drawing.Size(23, 20)
        Me.TextBoxRespuestaForm2.TabIndex = 41
        Me.TextBoxRespuestaForm2.Visible = False
        '
        'Timer2
        '
        '
        'BtnRecargar
        '
        Me.BtnRecargar.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.BtnRecargar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnRecargar.ForeColor = System.Drawing.Color.Turquoise
        Me.BtnRecargar.Image = CType(resources.GetObject("BtnRecargar.Image"), System.Drawing.Image)
        Me.BtnRecargar.Location = New System.Drawing.Point(1121, 45)
        Me.BtnRecargar.Name = "BtnRecargar"
        Me.BtnRecargar.Size = New System.Drawing.Size(62, 48)
        Me.BtnRecargar.TabIndex = 42
        Me.BtnRecargar.UseVisualStyleBackColor = True
        '
        'btnPreviewPrint
        '
        Me.btnPreviewPrint.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.btnPreviewPrint.Image = CType(resources.GetObject("btnPreviewPrint.Image"), System.Drawing.Image)
        Me.btnPreviewPrint.Location = New System.Drawing.Point(1152, 180)
        Me.btnPreviewPrint.Name = "btnPreviewPrint"
        Me.btnPreviewPrint.Size = New System.Drawing.Size(75, 55)
        Me.btnPreviewPrint.TabIndex = 43
        Me.btnPreviewPrint.UseVisualStyleBackColor = True
        '
        'BtnSi
        '
        Me.BtnSi.Location = New System.Drawing.Point(396, 25)
        Me.BtnSi.Name = "BtnSi"
        Me.BtnSi.Size = New System.Drawing.Size(85, 39)
        Me.BtnSi.TabIndex = 17
        Me.BtnSi.Text = "Si"
        Me.BtnSi.UseVisualStyleBackColor = True
        Me.BtnSi.Visible = False
        '
        'BtnNo
        '
        Me.BtnNo.Location = New System.Drawing.Point(396, 70)
        Me.BtnNo.Name = "BtnNo"
        Me.BtnNo.Size = New System.Drawing.Size(85, 39)
        Me.BtnNo.TabIndex = 18
        Me.BtnNo.Text = "No"
        Me.BtnNo.UseVisualStyleBackColor = True
        Me.BtnNo.Visible = False
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.BackColor = System.Drawing.Color.Azure
        Me.ClientSize = New System.Drawing.Size(1287, 573)
        Me.Controls.Add(Me.btnPreviewPrint)
        Me.Controls.Add(Me.BtnRecargar)
        Me.Controls.Add(Me.TextBoxRespuestaForm2)
        Me.Controls.Add(Me.LabelRevisionesPrueba)
        Me.Controls.Add(Me.CmbBxFiltroPrueba)
        Me.Controls.Add(Me.BtnFiltroPrueba)
        Me.Controls.Add(Me.BtnConectar)
        Me.Controls.Add(Me.LabelTimerVerificacion)
        Me.Controls.Add(Me.LabelTimerAsignacion)
        Me.Controls.Add(Me.LabelTimerTotal)
        Me.Controls.Add(Me.CmbBxAnalistas)
        Me.Controls.Add(Me.BtnDesconectar)
        Me.Controls.Add(Me.BtnConectarAdmin)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.TxtBxFiltroRegistro)
        Me.Controls.Add(Me.LabelTituloFiltroRegistro)
        Me.Controls.Add(Me.TextBoxContraseña)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "MainForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Revisiones 1.1"
        CType(Me.DGVAdmin, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBoxControlesTablas.ResumeLayout(False)
        Me.TabControl1.ResumeLayout(False)
        Me.TabPageMuestras.ResumeLayout(False)
        Me.TabPageMuestras.PerformLayout()
        Me.GroupBoxMuestras.ResumeLayout(False)
        Me.GroupBoxMuestras.PerformLayout()
        CType(Me.DGVMuestras, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPageBandejas.ResumeLayout(False)
        Me.TabPageBandejas.PerformLayout()
        Me.GroupBoxBandejas.ResumeLayout(False)
        Me.GroupBoxBandejas.PerformLayout()
        CType(Me.DGVBandejas, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPageAdmin.ResumeLayout(False)
        Me.TabPageAdmin.PerformLayout()
        Me.GroupBoxAdmin.ResumeLayout(False)
        Me.GroupBoxAdmin.PerformLayout()
        Me.PanelAdmin.ResumeLayout(False)
        Me.PanelAdmin.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents BtnNuevoRegistro As System.Windows.Forms.Button
    Friend WithEvents BtnModificarRegistro As System.Windows.Forms.Button
    Friend WithEvents DGVAdmin As System.Windows.Forms.DataGridView
    Friend WithEvents TxtBxFiltroRegistro As System.Windows.Forms.TextBox
    Friend WithEvents BtnEliminarRegistro As System.Windows.Forms.Button
    Friend WithEvents CbBxTablas As System.Windows.Forms.ComboBox
    Friend WithEvents LabelTituloFiltroRegistro As System.Windows.Forms.Label
    Friend WithEvents GroupBoxControlesTablas As System.Windows.Forms.GroupBox
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPageAdmin As System.Windows.Forms.TabPage
    Friend WithEvents TabPageMuestras As System.Windows.Forms.TabPage
    Friend WithEvents BtnAsignarMuestras As System.Windows.Forms.Button
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents TxBxValorC1Muestra As System.Windows.Forms.TextBox
    Friend WithEvents TabPageBandejas As System.Windows.Forms.TabPage
    Friend WithEvents DGVMuestras As System.Windows.Forms.DataGridView
    Friend WithEvents DGVBandejas As System.Windows.Forms.DataGridView
    Friend WithEvents BtnConectarAdmin As System.Windows.Forms.Button
    Friend WithEvents CmbBxAnalistas As System.Windows.Forms.ComboBox
    Friend WithEvents LabelValorC2Muestra As System.Windows.Forms.Label
    Friend WithEvents LabelValorC1Muestra As System.Windows.Forms.Label
    Friend WithEvents TxBxValorC2Muestra As System.Windows.Forms.TextBox
    Friend WithEvents BtnDesconectar As System.Windows.Forms.Button
    Friend WithEvents LabelComentario1Bandeja As System.Windows.Forms.Label
    Friend WithEvents LabelComentario2Bandeja As System.Windows.Forms.Label
    Friend WithEvents TxBxComentario1Bandeja As System.Windows.Forms.TextBox
    Friend WithEvents BtnAsignarBandejas As System.Windows.Forms.Button
    Friend WithEvents TxBxComentario2Bandeja As System.Windows.Forms.TextBox
    Friend WithEvents LabelTimerVerificacion As System.Windows.Forms.Label
    Friend WithEvents LabelTimerAsignacion As System.Windows.Forms.Label
    Friend WithEvents LabelTimerTotal As System.Windows.Forms.Label
    Friend WithEvents LabelMuestras As System.Windows.Forms.Label
    Friend WithEvents LabelBandejas As System.Windows.Forms.Label
    Friend WithEvents LabelAdmin As System.Windows.Forms.Label
    Friend WithEvents GroupBoxMuestras As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBoxBandejas As System.Windows.Forms.GroupBox
    Friend WithEvents BtnConectar As System.Windows.Forms.Button
    Friend WithEvents BtnFiltroPrueba As System.Windows.Forms.Button
    Friend WithEvents CmbBxFiltroPrueba As System.Windows.Forms.ComboBox
    Friend WithEvents PrintDocument1 As System.Drawing.Printing.PrintDocument
    Friend WithEvents TabPageRevisionesXPrueba As System.Windows.Forms.TabPage
    Friend WithEvents LabelRevisionesPrueba As System.Windows.Forms.Label
    Friend WithEvents GroupBoxAdmin As GroupBox
    Friend WithEvents ButtonAgregar As Button
    Friend WithEvents TextBox2 As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents BtnModificar As Button
    Friend WithEvents TextBox6 As TextBox
    Friend WithEvents ComboBox3 As ComboBox
    Friend WithEvents ComboBox2 As ComboBox
    Friend WithEvents ComboBox4 As ComboBox
    Friend WithEvents Label3 As Label
    Friend WithEvents ComboBox5 As ComboBox
    Friend WithEvents Label6 As Label
    Friend WithEvents ComboBox9 As ComboBox
    Friend WithEvents ComboBox8 As ComboBox
    Friend WithEvents PanelAdmin As Panel
    Friend WithEvents TextBoxContraseña As TextBox
    Friend WithEvents TextBoxRespuestaForm2 As TextBox
    Friend WithEvents TextBox3 As TextBox
    Friend WithEvents Timer2 As Timer
    Friend WithEvents BtnRecargar As Button
    Friend WithEvents btnPreviewPrint As Button
    Friend WithEvents BtnSi As Button
    Friend WithEvents BtnNo As Button
End Class