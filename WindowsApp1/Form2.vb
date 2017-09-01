Public Class FormContraseña
    Dim respuesta As Global.System.Int32
    Private Sub FormContraseña_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TxBxContraseña.Text = ""
    End Sub

    Private Sub BtnAceptar_Click(sender As Object, e As EventArgs) Handles BtnAceptar.Click
        MainForm.TextBoxContraseña.Text = TxBxContraseña.Text
        MainForm.TextBoxRespuestaForm2.Text = "1"
        Me.Close()
    End Sub

    Private Sub BtnCancelar_Click(sender As Object, e As EventArgs) Handles BtnCancelar.Click
        Me.Close()
    End Sub
End Class