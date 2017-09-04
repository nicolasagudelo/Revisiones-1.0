<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormCambiarContraseña
    Inherits System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento
    'Se puede modificar usando el Diseñador de Windows Forms.  
    'No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormCambiarContraseña))
        Me.LabelContraseñaAnterior = New System.Windows.Forms.Label()
        Me.LabelNuevaContraseña = New System.Windows.Forms.Label()
        Me.LabelNuevaContraseña2 = New System.Windows.Forms.Label()
        Me.TxBxContraseñaAnterior = New System.Windows.Forms.TextBox()
        Me.TxBxNuevaContraseña = New System.Windows.Forms.TextBox()
        Me.TxBxNuevaContraseña2 = New System.Windows.Forms.TextBox()
        Me.BtnAceptar = New System.Windows.Forms.Button()
        Me.BtnCancelar = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'LabelContraseñaAnterior
        '
        Me.LabelContraseñaAnterior.AutoSize = True
        Me.LabelContraseñaAnterior.Location = New System.Drawing.Point(31, 29)
        Me.LabelContraseñaAnterior.Name = "LabelContraseñaAnterior"
        Me.LabelContraseñaAnterior.Size = New System.Drawing.Size(142, 13)
        Me.LabelContraseñaAnterior.TabIndex = 0
        Me.LabelContraseñaAnterior.Text = "Digite su contraseña anterior"
        '
        'LabelNuevaContraseña
        '
        Me.LabelNuevaContraseña.AutoSize = True
        Me.LabelNuevaContraseña.Location = New System.Drawing.Point(31, 101)
        Me.LabelNuevaContraseña.Name = "LabelNuevaContraseña"
        Me.LabelNuevaContraseña.Size = New System.Drawing.Size(140, 13)
        Me.LabelNuevaContraseña.TabIndex = 1
        Me.LabelNuevaContraseña.Text = "Digite su nueva contraseña "
        '
        'LabelNuevaContraseña2
        '
        Me.LabelNuevaContraseña2.AutoSize = True
        Me.LabelNuevaContraseña2.Location = New System.Drawing.Point(31, 169)
        Me.LabelNuevaContraseña2.Name = "LabelNuevaContraseña2"
        Me.LabelNuevaContraseña2.Size = New System.Drawing.Size(199, 13)
        Me.LabelNuevaContraseña2.TabIndex = 2
        Me.LabelNuevaContraseña2.Text = "Digite nuevamente su nueva contraseña"
        '
        'TxBxContraseñaAnterior
        '
        Me.TxBxContraseñaAnterior.Location = New System.Drawing.Point(34, 45)
        Me.TxBxContraseñaAnterior.MaxLength = 30
        Me.TxBxContraseñaAnterior.Name = "TxBxContraseñaAnterior"
        Me.TxBxContraseñaAnterior.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.TxBxContraseñaAnterior.Size = New System.Drawing.Size(234, 20)
        Me.TxBxContraseñaAnterior.TabIndex = 3
        '
        'TxBxNuevaContraseña
        '
        Me.TxBxNuevaContraseña.Location = New System.Drawing.Point(34, 117)
        Me.TxBxNuevaContraseña.MaxLength = 30
        Me.TxBxNuevaContraseña.Name = "TxBxNuevaContraseña"
        Me.TxBxNuevaContraseña.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.TxBxNuevaContraseña.Size = New System.Drawing.Size(234, 20)
        Me.TxBxNuevaContraseña.TabIndex = 4
        '
        'TxBxNuevaContraseña2
        '
        Me.TxBxNuevaContraseña2.Location = New System.Drawing.Point(34, 185)
        Me.TxBxNuevaContraseña2.MaxLength = 30
        Me.TxBxNuevaContraseña2.Name = "TxBxNuevaContraseña2"
        Me.TxBxNuevaContraseña2.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.TxBxNuevaContraseña2.Size = New System.Drawing.Size(234, 20)
        Me.TxBxNuevaContraseña2.TabIndex = 5
        '
        'BtnAceptar
        '
        Me.BtnAceptar.Location = New System.Drawing.Point(46, 230)
        Me.BtnAceptar.Name = "BtnAceptar"
        Me.BtnAceptar.Size = New System.Drawing.Size(75, 38)
        Me.BtnAceptar.TabIndex = 6
        Me.BtnAceptar.Text = "Aceptar"
        Me.BtnAceptar.UseVisualStyleBackColor = True
        '
        'BtnCancelar
        '
        Me.BtnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.BtnCancelar.Location = New System.Drawing.Point(159, 230)
        Me.BtnCancelar.Name = "BtnCancelar"
        Me.BtnCancelar.Size = New System.Drawing.Size(75, 38)
        Me.BtnCancelar.TabIndex = 7
        Me.BtnCancelar.Text = "Cancelar"
        Me.BtnCancelar.UseVisualStyleBackColor = True
        '
        'FormCambiarContraseña
        '
        Me.AcceptButton = Me.BtnAceptar
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.BtnCancelar
        Me.ClientSize = New System.Drawing.Size(299, 290)
        Me.Controls.Add(Me.BtnCancelar)
        Me.Controls.Add(Me.BtnAceptar)
        Me.Controls.Add(Me.TxBxNuevaContraseña2)
        Me.Controls.Add(Me.TxBxNuevaContraseña)
        Me.Controls.Add(Me.TxBxContraseñaAnterior)
        Me.Controls.Add(Me.LabelNuevaContraseña2)
        Me.Controls.Add(Me.LabelNuevaContraseña)
        Me.Controls.Add(Me.LabelContraseñaAnterior)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "FormCambiarContraseña"
        Me.Text = "Cambiar Contraseña"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents LabelContraseñaAnterior As Label
    Friend WithEvents LabelNuevaContraseña As Label
    Friend WithEvents LabelNuevaContraseña2 As Label
    Friend WithEvents TxBxContraseñaAnterior As TextBox
    Friend WithEvents TxBxNuevaContraseña As TextBox
    Friend WithEvents TxBxNuevaContraseña2 As TextBox
    Friend WithEvents BtnAceptar As Button
    Friend WithEvents BtnCancelar As Button
End Class
