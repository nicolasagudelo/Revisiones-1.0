Imports MySql.Data.MySqlClient
Public Class Form1
    Dim conn As New MySqlConnection
    Public Sub connect()
        Dim DatabaseName As String = "bd_revision"
        Dim server As String = "localhost"
        Dim userName As String = "root"
        Dim password As String = "dm900494665"
        If Not conn Is Nothing Then conn.Close()
        conn.ConnectionString = String.Format("server={0}; user id={1}; password={2}; database={3}; pooling=false; Charset = utf8;", server, userName, password, DatabaseName)
        Try
            conn.Open()
            Console.WriteLine("conectandose a la base de datos")

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        conn.Close()

        'Llenando combobox utilizado en la pestaña de administrador para elegir la tabla a mostrar


        ComboBox1.Items.Add("Analistas")
        ComboBox1.Items.Add("Pruebas")
        ComboBox1.Items.Add("Relacion Analistas Pruebas")
        ComboBox1.Items.Add("Muestras Asignadas")
        ComboBox1.Items.Add("Muestras no Asignadas")
        ComboBox1.Items.Add("Bandejas Asignadas")
        ComboBox1.Items.Add("Bandejas no Asignadas")
        ComboBox1.Items.Add("Historial de Muestras")
        ComboBox1.Items.Add("Historial de Bandejas")

        ComboBox1.SelectedItem = "Analistas"


    End Sub

    Private Sub ComboBox1_SelectedValueChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedValueChanged
        GroupBox1.Visible = False
        cargar()
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        connect()

    End Sub

    Public Sub cargar()
        Dim Nombre_Tabla As String = ComboBox1.Text.ToString()
        Dim query As String

        If Nombre_Tabla = "Analistas" Then

            query = "SELECT * FROM analistas
                    Order by Nombre"

        ElseIf Nombre_Tabla = "Pruebas" Then

            query = "SELECT * FROM pruebas
                    Order by Nombre"

        ElseIf Nombre_Tabla = "Relacion Analistas Pruebas" Then

            query = "SELECT analistas.Nombre as 'Analista', pruebas.Nombre as 'Prueba' FROM ANALISTAS, rel_prue_analistas, pruebas
                    where analistas.AnalistNo = rel_prue_analistas.AnalistNo and pruebas.PrueNo = rel_prue_analistas.PrueNo
                    order by Analistas.Nombre"

        ElseIf Nombre_Tabla = "Muestras Asignadas" Then

            query = "SELECT rev_muestras.Muestra_No as 'Muestra',pruebas.Nombre as 'Prueba', rev_muestras.Valor_In, rev_muestras.Valor_C1, rev_muestras.Estado, rev_muestras.Valor_C2, rev_muestras.Pasa, analistas.Nombre as 'Revisa'
                    FROM analistas inner join rev_muestras on analistas.AnalistNo = rev_muestras.AnalistNo inner join pruebas on rev_muestras.PrueNo = pruebas.PrueNo
                    where rev_muestras.Estado in ('Revisado','Pendiente'); "

        ElseIf Nombre_Tabla = "Muestras no Asignadas" Then

            query = "SELECT rev_muestras.Muestra_No as 'Muestra',pruebas.Nombre as 'Prueba', rev_muestras.Valor_In, rev_muestras.Valor_C1, rev_muestras.Estado, rev_muestras.Valor_C2, rev_muestras.Pasa
                    FROM rev_muestras inner join pruebas on rev_muestras.PrueNo = pruebas.PrueNo
                    where rev_muestras.Estado in ('Revisado','Pendiente') and rev_muestras.AnalistNo is NULL; "

        ElseIf Nombre_Tabla = "Bandejas Asignadas" Then

            query = "SELECT rev_bandejas.Bandeja_No as 'Bandeja', pruebas.Nombre as 'Prueba', rev_bandejas.Comentario, rev_bandejas.Comentario_Revision, rev_bandejas.Estado, rev_bandejas.Comentario_Revision_2, rev_bandejas.Pasa, analistas.Nombre as 'Revisa'
                    FROM analistas inner join rev_bandejas on analistas.AnalistNo = rev_bandejas.AnalistNo inner join pruebas on rev_bandejas.PrueNo = pruebas.PrueNo
                    where rev_bandejas.Estado in ('Revisado','Pendiente');"

        ElseIf Nombre_Tabla = "Bandejas no Asignadas" Then

            query = "SELECT rev_bandejas.Bandeja_No as 'Bandeja', pruebas.Nombre as 'Prueba', rev_bandejas.Comentario, rev_bandejas.Comentario_Revision, rev_bandejas.Estado, rev_bandejas.Comentario_Revision_2, rev_bandejas.Pasa
                    FROM rev_bandejas inner join pruebas on rev_bandejas.PrueNo = pruebas.PrueNo
                    where rev_bandejas.Estado in ('Revisado','Pendiente') and rev_bandejas.AnalistNo is null;"

        ElseIf Nombre_Tabla = "Historial de Muestras" Then

            query = "SELECT rev_muestras.Muestra_No as 'Numero de muestra',pruebas.Nombre as 'Prueba', rev_muestras.Valor_In, rev_muestras.Valor_C1, rev_muestras.Estado, rev_muestras.Valor_C2, rev_muestras.Pasa, analistas.Nombre as 'Revisa'
                    FROM analistas inner join rev_muestras on analistas.AnalistNo = rev_muestras.AnalistNo inner join pruebas on rev_muestras.PrueNo = pruebas.PrueNo
                    where rev_muestras.Estado in ('Finalizado');"

        ElseIf Nombre_Tabla = "Historial de Bandejas" Then

            query = "SELECT rev_bandejas.Bandeja_No as 'Bandeja', pruebas.Nombre as 'Prueba', rev_bandejas.Comentario, rev_bandejas.Comentario_Revision, rev_bandejas.Estado, rev_bandejas.Comentario_Revision_2, rev_bandejas.Pasa, analistas.Nombre as 'Revisa'
                    FROM analistas inner join rev_bandejas on analistas.AnalistNo = rev_bandejas.AnalistNo inner join pruebas on rev_bandejas.PrueNo = pruebas.PrueNo
                    where rev_bandejas.Estado in ('Finalizado');"

            'Descomentar esto si se altera la linea de codigo donde ComboBox1.SelectedItem = "Analistas" o si se cambia el estilo del combobox1
            'Else
            'MsgBox("No ha seleccionado ninguna tabla", False, "Error")
            'Exit Sub
        End If

        Dim cmd As New MySqlCommand(query, conn)
        Dim reader As MySqlDataReader

        Label22.Text = Nombre_Tabla

        Try

            conn.Open()
            Console.WriteLine("conectandose a la base de datos")

            reader = cmd.ExecuteReader()

            Dim table As New DataTable
            table.Load(reader)
            DataGridView1.DataSource = table
            DataGridView1.ReadOnly = True
            DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect

            reader.Close()
            conn.Close()
        Catch ex As MySqlException
            MsgBox(ex.Message)
            conn.Close()
        End Try

        If Nombre_Tabla = "Analistas" Or Nombre_Tabla = "Pruebas" Then

            DataGridView1.Columns(0).Visible = False
            GroupBox2.Visible = True
            Button2.Enabled = True
            Button3.Enabled = True
            Button4.Enabled = True



        ElseIf Nombre_Tabla = "Relacion Analistas Pruebas" Then

            GroupBox2.Visible = True
            Button2.Enabled = True
            Button3.Enabled = True
            Button4.Enabled = True
            LlenarComboBox2()
            LlenarComboBox3()

        ElseIf Nombre_Tabla = "Muestras Asignadas" Then



        ElseIf Nombre_Tabla = "Muestras no Asignadas" Then



        ElseIf Nombre_Tabla = "Bandejas Asignadas" Then



        ElseIf Nombre_Tabla = "Bandejas no Asignadas" Then



        ElseIf Nombre_Tabla = "Historial de Muestras" Then



        ElseIf Nombre_Tabla = "Historial de Bandejas" Then

        End If
    End Sub



    Dim Pest_actual As Integer = 0

    Private Sub TabControl1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TabControl1.SelectedIndexChanged
        If TabControl1.SelectedTab Is TabPage1 Then
            'MsgBox("Se encuentra en la pestaña de Administrador")

            Pest_actual = 1
            Label24.Visible = False
            Label17.Visible = False
            Label18.Visible = False
            Label19.Visible = False
        ElseIf TabControl1.SelectedTab Is TabPage2 Then
            'MsgBox("Se encuentra en la pestaña de Muestras")

            Pest_actual = 2
            Label24.Visible = False
            Label17.Visible = False
            Label18.Visible = False
            Label19.Visible = False
        ElseIf TabControl1.SelectedTab Is TabPage3 Then
            'MsgBox("Se encuentra en la pestaña de Bandejas")

            Pest_actual = 3
            Label24.Visible = False
            Label17.Visible = False
            Label18.Visible = False
            Label19.Visible = False
        ElseIf TabControl1.SelectedTab Is TabPage4 Then
            Label24.Visible = True
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        If Label22.Text = "Analistas" Then
            Label1.Text = "Nombre del analista"
            Label2.Text = "Contraseña del analista"
            TextBox2.PasswordChar = "*"
            TextBox1.Text = ""
            TextBox2.Text = ""
            Panel1.Visible = False
            GroupBox1.Visible = True
            TextBox3.Visible = False
            TextBox6.Visible = False
            TextBox1.Visible = True
            TextBox2.Visible = True
            ComboBox2.Visible = False
            ComboBox3.Visible = False
            ComboBox8.Visible = False
            ComboBox9.Visible = False
            Button8.Visible = False
            Button5.Visible = True
        ElseIf Label22.Text = "Pruebas" Then
            Label1.Text = "Nombre de la Prueba"
            Label2.Text = "Descripción de la Prueba"
            TextBox2.PasswordChar = ""
            TextBox1.Text = ""
            TextBox2.Text = ""
            GroupBox1.Visible = True
            TextBox3.Visible = False
            TextBox6.Visible = False
            TextBox1.Visible = True
            TextBox2.Visible = True
            ComboBox2.Visible = False
            ComboBox3.Visible = False
            ComboBox8.Visible = False
            ComboBox9.Visible = False
            Button8.Visible = False
            Button5.Visible = True
        ElseIf Label22.Text = "Relacion Analistas Pruebas" Then
            Label1.Text = "Nombre del Analista"
            Label2.Text = "Nombre de la Prueba"
            GroupBox1.Visible = True
            Panel1.Visible = False
            TextBox3.Visible = False
            TextBox6.Visible = False
            TextBox1.Visible = False
            TextBox2.Visible = False
            ComboBox2.Visible = False
            ComboBox8.Visible = True
            Button8.Visible = False
            Button5.Visible = True
            LlenarComboBox8()
            ComboBox3.Visible = False
            ComboBox9.Visible = True
            LlenarComboBox9()
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click

        If Label22.Text = "Analistas" Then
            Label1.Text = "Nombre del analista"
            Label2.Text = "Contraseña del analista"
            TextBox2.PasswordChar = "*"
            TextBox1.Text = ""
            TextBox2.Text = ""
            GroupBox1.Visible = True
            Panel1.Visible = False
            Button5.Visible = False
            Button8.Visible = True
            TextBox3.Visible = True
            TextBox6.Visible = True
            ComboBox2.Visible = False
            ComboBox3.Visible = False
            ComboBox8.Visible = False
            ComboBox9.Visible = False
            TextBox1.Visible = False
            TextBox2.Visible = False
            DataGridView1_SelectedIndexChanged(sender, e)

        ElseIf Label22.Text = "Pruebas" Then
            Label1.Text = "Nombre de la Prueba"
            Label2.Text = "Descripción de la Prueba"
            TextBox2.PasswordChar = ""
            TextBox1.Text = ""
            TextBox2.Text = ""
            GroupBox1.Visible = True
            Panel1.Visible = False
            Button5.Visible = False
            Button8.Visible = True
            TextBox3.Visible = True
            TextBox6.Visible = True
            ComboBox2.Visible = False
            ComboBox3.Visible = False
            ComboBox8.Visible = False
            ComboBox9.Visible = False
            TextBox1.Visible = False
            TextBox2.Visible = False
            DataGridView1_SelectedIndexChanged(sender, e)
        ElseIf Label22.Text = "Relacion Analistas Pruebas" Then
            Panel1.Visible = True
            Label6.Visible = True
            Label3.Visible = True
            Label1.Text = "Valor Actual del analista"
            Label6.Text = "Nuevo Valor del analista"
            Label2.Text = "Valor Actual de la Prueba"
            Label3.Text = "Nuevo Valor de la Prueba"
            GroupBox1.Visible = True
            TextBox3.Visible = False
            TextBox6.Visible = False
            TextBox1.Visible = False
            TextBox2.Visible = False
            ComboBox8.Visible = False
            ComboBox9.Visible = False
            ComboBox5.Visible = True
            LlenarComboBox5()
            ComboBox4.Visible = True
            LlenarComboBox4()
            Button8.Visible = True
            Button5.Visible = False
            ComboBox2.Visible = True
            LlenarComboBox2()
            ComboBox2.Enabled = False
            ComboBox3.Visible = True
            LlenarComboBox3()
            ComboBox3.Enabled = False
            DataGridView1_SelectedIndexChanged(sender, e)

        End If
    End Sub


    Public Sub LlenarComboBox2()
        Dim query As String = " Select AnalistNo, Nombre from analistas
                                Where AnalistNo <> 1
                                Order by Nombre"
        Dim cmd As New MySqlCommand(query, conn)
        Dim sqlAdap As New MySqlDataAdapter(cmd)
        Dim dtRecord As New DataTable
        sqlAdap.Fill(dtRecord)
        ComboBox2.DataSource = dtRecord
        ComboBox2.DisplayMember = "Nombre"
        ComboBox2.ValueMember = "AnalistNo"
    End Sub

    Public Sub LlenarComboBox3()
        Dim query As String = " Select PrueNo, Nombre from Pruebas
                                Order by Nombre"
        Dim cmd As New MySqlCommand(query, conn)
        Dim sqlAdap As New MySqlDataAdapter(cmd)
        Dim dtRecord As New DataTable
        sqlAdap.Fill(dtRecord)
        ComboBox3.DataSource = dtRecord
        ComboBox3.DisplayMember = "Nombre"
        ComboBox3.ValueMember = "PrueNo"
    End Sub

    Public Sub LlenarComboBox4()
        Dim query As String = " Select PrueNo, Nombre from Pruebas
                                Order by Nombre"
        Dim cmd As New MySqlCommand(query, conn)
        Dim sqlAdap As New MySqlDataAdapter(cmd)
        Dim dtRecord As New DataTable
        sqlAdap.Fill(dtRecord)
        ComboBox4.DataSource = dtRecord
        ComboBox4.DisplayMember = "Nombre"
        ComboBox4.ValueMember = "PrueNo"
    End Sub

    Public Sub LlenarComboBox5()
        Dim query As String = " Select AnalistNo, Nombre from analistas
                                Where AnalistNo <> 1
                                Order by Nombre"
        Dim cmd As New MySqlCommand(query, conn)
        Dim sqlAdap As New MySqlDataAdapter(cmd)
        Dim dtRecord As New DataTable
        sqlAdap.Fill(dtRecord)
        ComboBox5.DataSource = dtRecord
        ComboBox5.DisplayMember = "Nombre"
        ComboBox5.ValueMember = "AnalistNo"
    End Sub

    Public Sub LlenarComboBox8()
        Dim query As String = " Select AnalistNo, Nombre from analistas
                                Where AnalistNo <> 1
                                Order by Nombre"
        Dim cmd As New MySqlCommand(query, conn)
        Dim sqlAdap As New MySqlDataAdapter(cmd)
        Dim dtRecord As New DataTable
        sqlAdap.Fill(dtRecord)
        ComboBox8.DataSource = dtRecord
        ComboBox8.DisplayMember = "Nombre"
        ComboBox8.ValueMember = "AnalistNo"
    End Sub

    Public Sub LlenarComboBox9()
        Dim query As String = " Select PrueNo, Nombre from Pruebas
                                Order by Nombre"
        Dim cmd As New MySqlCommand(query, conn)
        Dim sqlAdap As New MySqlDataAdapter(cmd)
        Dim dtRecord As New DataTable
        sqlAdap.Fill(dtRecord)
        ComboBox9.DataSource = dtRecord
        ComboBox9.DisplayMember = "Nombre"
        ComboBox9.ValueMember = "PrueNo"
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Dim Tabla_Actual As String = Label22.Text.ToString()
        If Tabla_Actual = "Analistas" Then
            Dim Filas_total As Integer = DataGridView1.RowCount
            Dim nombre As String = TextBox1.Text.ToString
            Dim Contraseña As String = TextBox2.Text.ToString
            If Contraseña = "" Then
                Contraseña = "123"
            End If
            If nombre = "" Then
                MsgBox("El nombre no puede estar vacio", False, "Error")
                Exit Sub
            End If
            Try
                conn.Open()
                Dim cmd As New MySqlCommand(String.Format("INSERT INTO analistas VALUES ('" & Filas_total & "','" & nombre & "','" & Contraseña & "')"), conn)
                MsgBox("Registro agregado satisfactoriamente", False, "Registro agregado")
                cmd.ExecuteNonQuery()
            Catch ex As MySqlException
                MsgBox(ex.Message)
                conn.Close()
            End Try
        ElseIf Tabla_Actual = "Pruebas" Then
            Dim Filas_total As Integer = DataGridView1.RowCount
            Dim nombre As String = TextBox1.Text.ToString
            Dim Descripcion As String = TextBox2.Text.ToString
            If nombre = "" Then
                MsgBox("El nombre no puede estar vacio", False, "Error")
                Exit Sub
            End If
            Try
                conn.Open()
                Dim cmd As New MySqlCommand(String.Format("INSERT INTO pruebas VALUES ('" & Filas_total & "','" & nombre & "','" & Descripcion & "')"), conn)
                MsgBox("Registro agregado satisfactoriamente", False, "Registro agregado")
                cmd.ExecuteNonQuery()
            Catch ex As MySqlException
                MsgBox(ex.Message)
                conn.Close()
            End Try
        ElseIf Tabla_Actual = "Relacion Analistas Pruebas" Then
            Dim Nombre As Integer = ComboBox8.SelectedValue
            Dim Prueba As Integer = ComboBox9.SelectedValue

            'Activar esto solo si se cambia el estilo de los Combobox 2 y 3 
            'If Nombre = 0 Then
            'MsgBox("El nombre seleccionado no es valido", False, "Error")
            'Exit Sub
            'ElseIf Prueba = 0 Then
            'MsgBox("La prueba seleccionada no es valida", False, "Error")
            'End If

            Try
                conn.Open()
                Dim cmd As New MySqlCommand(String.Format("INSERT INTO rel_prue_analistas VALUES ('" & Nombre & "', '" & Prueba & "');"), conn)
                cmd.ExecuteNonQuery()
                MsgBox("Registro agregado satisfactoriamente", False, "Registro agregado")
            Catch ex As MySqlException
                MsgBox(ex.Message, False, "Error")
                conn.Close()
            End Try

        End If
        conn.Close()
        cargar()
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        Dim reader As MySqlDataReader
        Dim Fila_actual_obj As Object = (DataGridView1.CurrentRow)
        If IsNothing(Fila_actual_obj) Then
            MsgBox("Seleccione una fila a modificar")
            Exit Sub
        End If

        Dim Tabla_Actual As String = Label22.Text
        If Tabla_Actual = "Analistas" Then
            Dim Fila_actual As Integer = (DataGridView1.CurrentRow.Index)
            Dim llave As Integer = DataGridView1(0, (Fila_actual)).Value
            Dim nombre As String = TextBox3.Text.ToString
            Dim Contraseña As String = TextBox6.Text.ToString
            Try
                conn.Open()
                Dim query As String = "UPDATE analistas set Nombre = '" & nombre & "', Contraseña = '" & Contraseña & "' where AnalistNo = " & llave
                Dim cmd As New MySqlCommand(query, conn)
                reader = cmd.ExecuteReader
                MsgBox("Registro modificado satisfactoriamente", False, "Registro Modificado")
                conn.Close()
            Catch ex As MySqlException
                MsgBox(ex.Message)
                conn.Close()
            End Try
        ElseIf Tabla_Actual = "Pruebas" Then
            Dim Fila_actual As Integer = (DataGridView1.CurrentRow.Index)
            Dim llave As Integer = DataGridView1(0, (Fila_actual)).Value
            Dim nombre As String = TextBox3.Text.ToString
            Dim Descripcion As String = TextBox6.Text.ToString
            Try
                conn.Open()
                Dim query As String = "UPDATE pruebas set Nombre = '" & nombre & "', Descripcion = '" & Descripcion & "' where PrueNo = " & llave
                Dim cmd As New MySqlCommand(query, conn)
                reader = cmd.ExecuteReader
                MsgBox("Registro modificado satisfactoriamente", False, "Registro Modificado")
                conn.Close()
            Catch ex As MySqlException
                MsgBox(ex.Message)
                conn.Close()
            End Try
        ElseIf Tabla_Actual = "Relacion Analistas Pruebas" Then
            Dim Nombre_actual As Integer = ComboBox2.SelectedValue
            Dim Prueba_actual As Integer = ComboBox3.SelectedValue
            Dim Nombre_Nuevo As Integer = ComboBox5.SelectedValue
            Dim Prueba_Nueva As Integer = ComboBox4.SelectedValue
            Try
                conn.Open()
                Dim query As String = "UPDATE rel_prue_analistas SET AnalistNo ='" & Nombre_Nuevo & "', PrueNo ='" & Prueba_Nueva & "' WHERE AnalistNo ='" & Nombre_actual & "' and PrueNo ='" & Prueba_actual & "';"
                Dim cmd As New MySqlCommand(query, conn)
                reader = cmd.ExecuteReader
                MsgBox("Registro modificado satisfactoriamente", False, "Registro Modificado")
                conn.Close()
            Catch ex As Exception
                MsgBox(ex.Message)
                conn.Close()
            End Try
        End If
        cargar()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If MessageBox.Show("Desea eliminar el registro seleccionado ?", "Eliminar", MessageBoxButtons.YesNo) = DialogResult.Yes Then
            Dim reader As MySqlDataReader
            Dim Fila_actual_obj As Object = (DataGridView1.CurrentRow)
            If IsNothing(Fila_actual_obj) Then
                MsgBox("Seleccione una fila a eliminar")
                Exit Sub
            End If

            Dim Tabla_Actual As String = Label22.Text
            If Tabla_Actual = "Analistas" Then
                Dim Fila_actual As Integer = (DataGridView1.CurrentRow.Index)
                Dim llave As Integer = DataGridView1(0, (Fila_actual)).Value
                DataGridView1_SelectedIndexChanged(sender, e)
                Try
                    conn.Open()
                    Dim query As String = "DELETE FROM analistas where AnalistNo = " & llave
                    Dim cmd As New MySqlCommand(query, conn)
                    reader = cmd.ExecuteReader
                    MsgBox("Registro eliminado satisfactoriamente", False, "Registro Eliminado")
                    conn.Close()
                Catch ex As MySqlException
                    MsgBox(ex.Message)
                    conn.Close()
                End Try
            ElseIf Tabla_Actual = "Pruebas" Then
                Dim Fila_actual As Integer = (DataGridView1.CurrentRow.Index)
                Dim llave As Integer = DataGridView1(0, (Fila_actual)).Value
                DataGridView1_SelectedIndexChanged(sender, e)
                Try
                    conn.Open()
                    Dim query As String = "DELETE FROM pruebas where PrueNo = " & llave
                    Dim cmd As New MySqlCommand(query, conn)
                    reader = cmd.ExecuteReader
                    MsgBox("Registro eliminado satisfactoriamente", False, "Registro Eliminado")
                    conn.Close()
                Catch ex As MySqlException
                    MsgBox(ex.Message)
                    conn.Close()
                End Try
            ElseIf Tabla_Actual = "Relacion Analistas Pruebas" Then

                DataGridView1_SelectedIndexChanged(sender, e)

                Dim nombre As Integer = ComboBox2.SelectedValue
                Dim prueba As Integer = ComboBox3.SelectedValue

                Try
                        conn.Open()
                        Dim query As String = "DELETE FROM rel_prue_analistas WHERE AnalistNo = '" & nombre & "' and PrueNo ='" & prueba & "';"
                        Dim cmd As New MySqlCommand(query, conn)
                        reader = cmd.ExecuteReader
                        MsgBox("Registro eliminado satisfactoriamente", False, "Registro Eliminado")
                        conn.Close()
                    Catch ex As MySqlException
                        MsgBox(ex.Message)
                        conn.Close()
                    End Try

            End If
            cargar()
        End If
    End Sub

    Private Sub DataGridView1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridView1.SelectionChanged
        Try
            Dim fila_actual As Integer = (DataGridView1.CurrentRow.Index)
            'Dim nombre_columna As String = DataGridView1.Columns(0).Name

            Dim nom_tabla As String = Label22.Text


            If nom_tabla = "Analistas" Or nom_tabla = "Pruebas" Then
                TextBox3.Text = DataGridView1(1, (fila_actual)).Value
                TextBox6.Text = DataGridView1(2, (fila_actual)).Value
            ElseIf nom_tabla = "Relacion Analistas Pruebas" Then
                ComboBox2.Text = DataGridView1(0, (fila_actual)).Value
                ComboBox3.Text = DataGridView1(1, (fila_actual)).Value
                ComboBox5.Text = ComboBox2.Text
                ComboBox4.Text = ComboBox3.Text
            End If
        Catch ex As Exception
        End Try
    End Sub


End Class
