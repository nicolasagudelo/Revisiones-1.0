Public Class Form2
    Dim respuesta As Global.System.Int32
    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TextBox1.Text = ""
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Form1.TextBoxContraseña.Text = TextBox1.Text
        Form1.TextBoxRespuestaForm2.Text = "1"
        Me.Close()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Close()
    End Sub
End Class