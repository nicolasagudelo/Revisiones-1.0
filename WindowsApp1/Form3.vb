Public Class FormCambiarContraseña
    Private Sub FormCambiarContraseña_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TxBxContraseñaAnterior.Text = ""
        TxBxNuevaContraseña.Text = ""
        TxBxNuevaContraseña2.Text = ""
    End Sub

    Private Sub BtnAceptar_Click(sender As Object, e As EventArgs) Handles BtnAceptar.Click

        If TxBxContraseñaAnterior.TextLength < 3 Or TxBxNuevaContraseña.TextLength < 3 Or TxBxNuevaContraseña2.TextLength < 3 Then
            MsgBox("Las contraseñas deben tener minimo 3 caracteres", False, "Error")
        Else
            MainForm.TxBxContraseñaAnterior.Text = TxBxContraseñaAnterior.Text
            MainForm.TxBxContraseñaNueva.Text = TxBxNuevaContraseña.Text
            MainForm.TextBoxContraseña.Text = TxBxNuevaContraseña2.Text
            MainForm.TxBxRespuestaForm3.Text = "1"
            Me.Close()
        End If

    End Sub

    Private Sub BtnCancelar_Click(sender As Object, e As EventArgs) Handles BtnCancelar.Click
        Me.Close()
    End Sub
End Class