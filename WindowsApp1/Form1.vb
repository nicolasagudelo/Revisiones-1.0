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

        conteo_muestras()
    End Sub

    Private Sub conteo_muestras()
        Dim numero_de_pruebas As Integer

        Try
            conn.Open()
            Dim cmd As New MySqlCommand(String.Format("Select count(PrueNo) FROM pruebas;"), conn)
            numero_de_pruebas = Convert.ToString(cmd.ExecuteScalar())
            conn.Close()
        Catch ex As Exception
            MsgBox(ex.Message, False, "Error")
            conn.Close()
            Exit Sub
        End Try

        TabPage4.Controls.Clear()

        Dim X1, Y1 As Integer
        Dim r As Integer = 15
        Dim m As Integer = 0
        Dim cantidad_pendiente, cantidad_pendiente_bandejas As Integer
        Dim LabelArray(numero_de_pruebas) As Label
        Dim TextBoxArray(numero_de_pruebas) As TextBox
        Dim j As Integer = 0
        Dim j1 As Integer = j
        Dim nombre_prueba As String
        For i = 1 To numero_de_pruebas
            r = r + 10
            X1 = 30 + m
            Y1 = (20 + (j1 - 1) * 30) + r
            LabelArray(i) = New Label
            LabelArray(i).Location = New Point(X1, Y1)
            Try
                conn.Open()
                Dim cmd As New MySqlCommand(String.Format("select Nombre from pruebas where prueno =" & i & ";"), conn)
                nombre_prueba = Convert.ToString(cmd.ExecuteScalar())
                conn.Close()
            Catch ex As Exception
                MsgBox(ex.Message, False, "Error")
                conn.Close()
                Exit Sub
            End Try
            conn.Close()
            LabelArray(i).Text = nombre_prueba
            LabelArray(i).Width = 60
            LabelArray(i).Height = 15
            LabelArray(i).ForeColor = Color.Black
            LabelArray(i).Visible = True
            LabelArray(i).Parent = Me.TabPage4
            Try
                conn.Open()
                Dim cmd As New MySqlCommand(String.Format("Select Cuenta_Pruebas.c
                                                        FROM(
                                                        select pruebas.Nombre, pruebas.PrueNo, count(*) as c FROM rev_muestras inner join pruebas on rev_muestras.PrueNo = pruebas.PrueNo
                                                        WHERE pruebas.PrueNo = " & i & " and Estado in ('Pendiente','Revisado')
                                                        )Cuenta_Pruebas;"), conn)
                cantidad_pendiente = Convert.ToString(cmd.ExecuteScalar())
                conn.Close()
            Catch ex As Exception
                MsgBox(ex.Message, False, "Error")
                conn.Close()
                Exit Sub
            End Try
            conn.Close()
            Try
                conn.Open()
                Dim cmd As New MySqlCommand(String.Format("Select Cuenta_Pruebas.c
                                                        FROM(
                                                        select pruebas.Nombre, pruebas.PrueNo, count(*)*40 as c FROM rev_bandejas inner join pruebas on rev_bandejas.PrueNo = pruebas.PrueNo
                                                        WHERE pruebas.PrueNo = " & i & " and Estado in ('Pendiente','Revisado')
                                                        )Cuenta_Pruebas;"), conn)
                cantidad_pendiente_bandejas = Convert.ToString(cmd.ExecuteScalar())
                conn.Close()
            Catch ex As Exception
                MsgBox(ex.Message, False, "Error")
                conn.Close()
                Exit Sub
            End Try
            conn.Close()
            cantidad_pendiente = cantidad_pendiente + cantidad_pendiente_bandejas
            r = r + 20
            X1 = 30 + m
            Y1 = (20 + (j1 - 1) * 30) + r
            TextBoxArray(i) = New TextBox
            TextBoxArray(i).Location = New Point(X1, Y1)
            TextBoxArray(i).Width = 110
            TextBoxArray(i).Height = 15
            TextBoxArray(i).Name = "Textboxs" & i
            TextBoxArray(i).Text = ""
            TextBoxArray(i).Text = cantidad_pendiente.ToString
            TextBoxArray(i).Visible = True
            TextBoxArray(i).ReadOnly = True
            TextBoxArray(i).Parent = Me.TabPage4

            j = j + 1
            j1 = j1 + 1
            If i Mod 7 = 0 Then
                m = m + 170
                r = 15
                j1 = 0
            End If
        Next

    End Sub

    Private Sub ComboBox1_SelectedValueChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedValueChanged
        GroupBox1.Visible = False
        cargar()
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        connect()
        cargar_analistas()
        TabControl1.TabPages.Remove(TabPage1)

    End Sub

    Private Sub cargar_analistas()
        ComboBox6.Items.Clear()
        ComboBox6.Enabled = True
        Dim query As String = " Select AnalistNo, Nombre from analistas
                                where AnalistNo <> 1;"
        Dim cmd As New MySqlCommand(query, conn)
        Dim sqlAdap As New MySqlDataAdapter(cmd)
        Dim dtRecord As New DataTable
        sqlAdap.Fill(dtRecord)
        ComboBox6.DataSource = dtRecord
        ComboBox6.DisplayMember = "Nombre"
        ComboBox6.ValueMember = "AnalistNo"
        ComboBox6.Text = ""
        ComboBox6.SelectedValue = 0
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

            query = "SELECT Muestra_ID, rev_muestras.Muestra_No as 'Muestra',pruebas.Nombre as 'Prueba', rev_muestras.Valor_In, rev_muestras.Valor_C1, rev_muestras.Estado, rev_muestras.Valor_C2, rev_muestras.Pasa, analistas.Nombre as 'Revisa'
                    FROM analistas inner join rev_muestras on analistas.AnalistNo = rev_muestras.AnalistNo inner join pruebas on rev_muestras.PrueNo = pruebas.PrueNo
                    where rev_muestras.Estado in ('Revisado','Pendiente'); "

        ElseIf Nombre_Tabla = "Muestras no Asignadas" Then

            query = "SELECT Muestra_ID, rev_muestras.Muestra_No as 'Muestra',pruebas.Nombre as 'Prueba', rev_muestras.Valor_In, rev_muestras.Valor_C1, rev_muestras.Estado, rev_muestras.Valor_C2, rev_muestras.Pasa
                    FROM rev_muestras inner join pruebas on rev_muestras.PrueNo = pruebas.PrueNo
                    where rev_muestras.Estado in ('Revisado','Pendiente') and rev_muestras.AnalistNo is NULL; "

        ElseIf Nombre_Tabla = "Bandejas Asignadas" Then

            query = "SELECT Bandeja_ID, rev_bandejas.Bandeja_No as 'Bandeja', pruebas.Nombre as 'Prueba', rev_bandejas.Comentario, rev_bandejas.Comentario_Revision, rev_bandejas.Estado, rev_bandejas.Comentario_Revision_2, rev_bandejas.Pasa, analistas.Nombre as 'Revisa'
                    FROM analistas inner join rev_bandejas on analistas.AnalistNo = rev_bandejas.AnalistNo inner join pruebas on rev_bandejas.PrueNo = pruebas.PrueNo
                    where rev_bandejas.Estado in ('Revisado','Pendiente');"

        ElseIf Nombre_Tabla = "Bandejas no Asignadas" Then

            query = "SELECT Bandeja_ID, rev_bandejas.Bandeja_No as 'Bandeja', pruebas.Nombre as 'Prueba', rev_bandejas.Comentario, rev_bandejas.Comentario_Revision, rev_bandejas.Estado, rev_bandejas.Comentario_Revision_2, rev_bandejas.Pasa
                    FROM rev_bandejas inner join pruebas on rev_bandejas.PrueNo = pruebas.PrueNo
                    where rev_bandejas.Estado in ('Revisado','Pendiente') and rev_bandejas.AnalistNo is null;"

        ElseIf Nombre_Tabla = "Historial de Muestras" Then

            query = "SELECT Muestra_ID, rev_muestras.Muestra_No as 'Numero de muestra',pruebas.Nombre as 'Prueba', rev_muestras.Valor_In, rev_muestras.Valor_C1, rev_muestras.Estado, rev_muestras.Valor_C2, rev_muestras.Pasa, analistas.Nombre as 'Revisa'
                    FROM analistas inner join rev_muestras on analistas.AnalistNo = rev_muestras.AnalistNo inner join pruebas on rev_muestras.PrueNo = pruebas.PrueNo
                    where rev_muestras.Estado in ('Finalizado');"

        ElseIf Nombre_Tabla = "Historial de Bandejas" Then

            query = "SELECT Bandeja_ID, rev_bandejas.Bandeja_No as 'Bandeja', pruebas.Nombre as 'Prueba', rev_bandejas.Comentario, rev_bandejas.Comentario_Revision, rev_bandejas.Estado, rev_bandejas.Comentario_Revision_2, rev_bandejas.Pasa, analistas.Nombre as 'Revisa'
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
            Button2.Visible = True
            Button3.Visible = True
            Button4.Visible = True


        ElseIf Nombre_Tabla = "Relacion Analistas Pruebas" Then

            GroupBox2.Visible = True
            Button2.Enabled = True
            Button3.Enabled = True
            Button4.Enabled = True
            Button2.Visible = True
            Button3.Visible = True
            Button4.Visible = True
            LlenarComboBox2()
            LlenarComboBox3()

        ElseIf Nombre_Tabla = "Muestras Asignadas" Then

            DataGridView1.Columns(0).Visible = False
            GroupBox1.Visible = True
            TextBox1.Visible = False
            TextBox2.Visible = False
            ComboBox8.Visible = False
            ComboBox9.Visible = False
            Label1.Visible = False
            Label2.Visible = False
            Button8.Visible = False
            ComboBox2.Visible = False
            ComboBox3.Visible = False
            TextBox3.Visible = False
            TextBox6.Visible = False
            Panel1.Visible = True
            Label3.Visible = False
            ComboBox4.Visible = False
            GroupBox2.Visible = False
            Button5.Text = "Aceptar"
            Button5.Visible = True
            Label6.Text = "Pasa"
            Label6.Visible = True
            ComboBox5.DataSource = Nothing
            ComboBox5.Items.Clear()
            ComboBox5.Items.Add("Si")
            ComboBox5.Items.Add("No")
            ComboBox5.Visible = True

        ElseIf Nombre_Tabla = "Muestras no Asignadas" Then

                DataGridView1.Columns(0).Visible = False
                GroupBox2.Visible = True
                Button2.Visible = True
                Button3.Visible = False
                Button4.Visible = False
                Button2.Enabled = True
                Button3.Enabled = False
                Button4.Enabled = False

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
            Label1.Visible = True
            Label2.Visible = True
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
            Button5.Text = "Agregar"
            Button5.Visible = True
        ElseIf Label22.Text = "Pruebas" Then
            Label1.Text = "Nombre de la Prueba"
            Label2.Text = "Descripción de la Prueba"
            Label1.Visible = True
            Label2.Visible = True
            TextBox2.PasswordChar = ""
            TextBox1.Text = ""
            TextBox2.Text = ""
            GroupBox1.Visible = True
            Panel1.Visible = False
            TextBox3.Visible = False
            TextBox6.Visible = False
            TextBox1.Visible = True
            TextBox2.Visible = True
            ComboBox2.Visible = False
            ComboBox3.Visible = False
            ComboBox8.Visible = False
            ComboBox9.Visible = False
            Button8.Visible = False
            Button5.Text = "Agregar"
            Button5.Visible = True
        ElseIf Label22.Text = "Relacion Analistas Pruebas" Then
            Label1.Text = "Nombre del Analista"
            Label2.Text = "Nombre de la Prueba"
            Label1.Visible = True
            Label2.Visible = True
            GroupBox1.Visible = True
            Panel1.Visible = False
            TextBox3.Visible = False
            TextBox6.Visible = False
            TextBox1.Visible = False
            TextBox2.Visible = False
            ComboBox2.Visible = False
            ComboBox8.Visible = True
            Button8.Visible = False
            Button5.Text = "Agregar"
            Button5.Visible = True
            LlenarComboBox8()
            ComboBox3.Visible = False
            ComboBox9.Visible = True
            LlenarComboBox9()
        ElseIf Label22.Text = "Muestras no Asignadas" Then
            Label1.Text = "Numero de Muestra"
            Label2.Text = "Valor Ingresado"
            Label1.Visible = True
            Label2.Visible = True
            TextBox2.PasswordChar = ""
            TextBox1.Text = ""
            TextBox2.Text = ""
            Button5.Text = "Crear Muestra"
            GroupBox1.Visible = True
            Button8.Visible = False
            Button5.Visible = True
            TextBox3.Visible = False
            TextBox6.Visible = False
            TextBox1.Visible = True
            TextBox2.Visible = True
            ComboBox2.Visible = False
            ComboBox3.Visible = False
            ComboBox8.Visible = False
            ComboBox9.Visible = False
            LlenarComboBox4()
            ComboBox4.Visible = True
            Panel1.Visible = True
            Label6.Visible = False
            ComboBox5.Visible = False
            Label3.Text = "Prueba"
            Label3.Visible = True

        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click

        If Label22.Text = "Analistas" Then
            Label1.Text = "Nombre del analista"
            Label2.Text = "Contraseña del analista"
            Label1.Visible = True
            Label2.Visible = True
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
            Label1.Visible = True
            Label2.Visible = True
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
            Label1.Visible = True
            Label2.Visible = True
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

        ElseIf Tabla_Actual = "Muestras Asignadas" Then
            Dim t_terminado As String
            Dim t_asignacion_2 As String
            Dim pasa As String = ComboBox5.Text
            Dim valor_c2_existe As Boolean
            If TextBox3.Text = Nothing Then
                valor_c2_existe = False
            Else
                valor_c2_existe = True
            End If
            Dim muestra_id As String = TextBox6.Text
            If pasa = Nothing Then
                MsgBox("No ha seleccionado ningun valor en el campo 'Pasa'", False, "Error")
            ElseIf pasa = "Si" Then
                t_terminado = String.Format(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                Try
                    conn.Open()
                    Dim cmd As New MySqlCommand(String.Format("UPDATE rev_muestras SET Tiempo_T ='" & t_terminado & "', Estado ='Finalizado', Pasa ='Si' WHERE Muestra_ID ='" & muestra_id & "';"), conn)
                    cmd.ExecuteNonQuery()
                    MsgBox("Registro modificado satisfactoriamente", False, "Registro modificado")
                Catch ex As MySqlException
                    MsgBox(ex.Message, False, "Error")
                    conn.Close()
                End Try
                conn.Close()
            ElseIf pasa = "No" And Not (valor_c2_existe) Then
                t_asignacion_2 = String.Format(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                Try
                    conn.Open()
                    Dim cmd As New MySqlCommand(String.Format("UPDATE rev_muestras SET Tiempo_A2 ='" & t_asignacion_2 & "', Estado = 'Pendiente', Pasa ='No' WHERE `Muestra_ID`='" & muestra_id & "';"), conn)
                    cmd.ExecuteNonQuery()
                    MsgBox("Registro modificado satisfactoriamente", False, "Registro modificado")
                Catch ex As MySqlException
                    MsgBox(ex.Message, False, "Error")
                    conn.Close()
                End Try
                conn.Close()
            ElseIf pasa = "No" And valor_c2_existe Then
                t_terminado = String.Format(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                Try
                    conn.Open()
                    Dim cmd As New MySqlCommand(String.Format("UPDATE rev_muestras SET `Tiempo_T`='" & t_terminado & "', Estado= 'Finalizado' WHERE Muestra_ID ='" & muestra_id & "';"), conn)
                    cmd.ExecuteNonQuery()
                    MsgBox("Registro modificado satisfactoriamente", False, "Registro modificado")
                Catch ex As MySqlException
                    MsgBox(ex.Message, False, "Error")
                    conn.Close()
                End Try
                conn.Close()
            End If
            cargar()

        ElseIf Tabla_Actual = "Muestras no Asignadas" Then
            Dim llave As String
            Dim Muestra_No As String = TextBox1.Text.ToString
            Dim Valor_In As String = TextBox2.Text.ToString
            Dim prueba As Integer = ComboBox4.SelectedValue

            Try
                conn.Open()
                Dim cmd As New MySqlCommand(String.Format("SELECT MAX(Muestra_ID)+1 FROM rev_muestras;"), conn)
                llave = Convert.ToString(cmd.ExecuteScalar())
                conn.Close()
            Catch ex As Exception
                MsgBox(ex.Message, False, "Error")
                conn.Close()
                Exit Sub
            End Try

            Dim t_creacion As String = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")

            Try
                conn.Open()
                Dim cmd As New MySqlCommand(String.Format("INSERT INTO rev_muestras (`Muestra_ID`, `Muestra_No`, `PrueNo`, `Valor_In`, `Tiempo_C`, `Estado`) VALUES ('" & llave & "', '" & Muestra_No & "', '" & prueba & "', '" & Valor_In & "', '" & t_creacion & "', 'Pendiente');"), conn)
                cmd.ExecuteNonQuery()
                MsgBox("Muestra Creada Satisfactoriamente", False, "Muestra Creada")
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
            ElseIf nom_tabla = "Muestras Asignadas" Then
                If DataGridView1(5, Fila_actual).Value.ToString() = "Revisado" Then
                    ComboBox5.Enabled = True
                    Button5.Enabled = True
                    TextBox3.Text = DataGridView1(6, fila_actual).Value.ToString()
                    TextBox6.Text = DataGridView1(0, fila_actual).Value.ToString()
                    TextBox3.Visible = False
                    TextBox6.Visible = False
                Else
                    ComboBox5.Enabled = False
                    Button5.Enabled = False
                    TextBox6.Text = DataGridView1(0, fila_actual).Value.ToString()
                    TextBox6.Visible = False
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub

    Dim password As String

    Private Sub Button15_Click(sender As Object, e As EventArgs) Handles Button15.Click
        DataGridView2.Visible = True
        DataGridView3.Visible = True
        Dim usuario As Integer = ComboBox6.SelectedValue
        Dim usuario_string As String = ComboBox6.Text
        TextBoxContraseña.Text = ""
        TextBoxRespuestaForm2.Text = ""
        If usuario = 0 Then
            MsgBox("Seleccione un analista", False, "Error")
            Exit Sub
        End If
        Form2.ShowDialog()
        Dim respuestaform2 As String = TextBoxRespuestaForm2.Text
        If respuestaform2 = "1" Then
            Dim password As String = TextBoxContraseña.Text
            Dim bd_password As String
            Try
                conn.Open()
                Dim cmd As New MySqlCommand(String.Format("Select contraseña FROM analistas
                                                       Where AnalistNo = " & usuario & ";"), conn)
                bd_password = Convert.ToString(cmd.ExecuteScalar())
                conn.Close()
            Catch ex As Exception
                MsgBox(ex.Message, False, "Error")
                conn.Close()
                Exit Sub
            End Try

            If password = bd_password Then
                MsgBox("Bienvenido " & usuario_string & "", False, "Log-In")
                cargar_muestras(usuario)
                cargar_bandejas(usuario)
                ComboBox6.Enabled = False
                Button15.Enabled = False
                Button11.Enabled = False
            Else
                MsgBox("Constraseña Incorrecta")
                Exit Sub
            End If
        Else
            Exit Sub
        End If

    End Sub

    Private Sub cargar_muestras(usuario)

        GroupBox4.Visible = True
        Button7.Enabled = True
        Button7.Visible = True


        Dim reader As MySqlDataReader

        Try
            conn.Open()
            Dim query As String = "Select Muestra_ID, Muestra_No, pruebas.Nombre as Prueba, Valor_In, Valor_C1, Estado, Valor_C2, Pasa, analistas.Nombre as Revisa
                                    from(
                                    Select Muestra_ID, Muestra_No, Prueba, Valor_In, Valor_C1, Estado, Valor_C2, Pasa, Revisa 
                                    from(
                                    SELECT Muestra_ID, Muestra_No, pruebas.PrueNo as Prueba, Valor_In, Valor_C1, Estado, Valor_C2, Pasa, analistas.AnalistNo as Revisa
                                    FROM pruebas inner join rev_muestras on pruebas.PrueNo = rev_muestras.PrueNo left join analistas on rev_muestras.AnalistNo = analistas.AnalistNo
                                    where (rev_muestras.AnalistNo = " & usuario & " or rev_muestras.AnalistNo is Null) and Estado in ('Revisado','Pendiente')
                                    )a 
                                    inner join 
                                    (Select * from rel_prue_analistas
                                    where AnalistNo = " & usuario & ")b
                                    on a.Prueba = b.PrueNo)c
                                    inner join pruebas on pruebas.PrueNo = c.Prueba left join analistas on analistas.AnalistNo = c.Revisa;"
            Dim cmd As New MySqlCommand(query, conn)
            Console.WriteLine("Cargando Muestras del analista")

            reader = cmd.ExecuteReader()

            Dim table As New DataTable
            table.Load(reader)
            DataGridView2.DataSource = table
            DataGridView2.ReadOnly = True
            DataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            DataGridView2.Columns(0).Visible = False

            reader.Close()
            conn.Close()
        Catch ex As MySqlException
            MsgBox(ex.Message)
            conn.Close()
        End Try
        If DataGridView2(0, 0).Value Is Nothing Then
            MsgBox("Usted no cuenta con revisiones de muestras pendientes o posibles", False, "Info. Muestras")
        End If
    End Sub

    Private Sub cargar_bandejas(usuario)
        Dim reader As MySqlDataReader

        Try
            conn.Open()
            Dim query As String = "Select Bandeja_ID, Bandeja_No, pruebas.Nombre as Prueba, Comentario, Comentario_Revision, Estado, Comentario_Revision_2, Pasa, analistas.AnalistNo as Revisa
                                    from(
                                    Select Bandeja_ID, Bandeja_No, Prueba, Comentario, Comentario_Revision, Estado, Comentario_Revision_2, Pasa, Revisa 
                                    from(
                                    SELECT Bandeja_ID, Bandeja_No, pruebas.PrueNo as Prueba, Comentario, Comentario_Revision, Estado, Comentario_Revision_2, Pasa, analistas.AnalistNo as Revisa
                                    FROM pruebas inner join rev_bandejas on pruebas.PrueNo = rev_bandejas.PrueNo left join analistas on rev_bandejas.AnalistNo = analistas.AnalistNo
                                    where (rev_bandejas.AnalistNo = " & usuario & " or rev_bandejas.AnalistNo is Null) and Estado in ('Revisado','Pendiente')
                                    )a 
                                    inner join 
                                    (Select * from rel_prue_analistas
                                    where AnalistNo = " & usuario & ")b
                                    on a.Prueba = b.PrueNo)c
                                    inner join pruebas on pruebas.PrueNo = c.Prueba left join analistas on analistas.AnalistNo = c.Revisa;"
            Dim cmd As New MySqlCommand(query, conn)
            Console.WriteLine("Cargando Bandejas del analista")

            reader = cmd.ExecuteReader()

            Dim table As New DataTable
            table.Load(reader)
            DataGridView3.DataSource = table
            DataGridView3.ReadOnly = True
            DataGridView3.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            DataGridView3.Columns(0).Visible = False

            reader.Close()
            conn.Close()
        Catch ex As MySqlException
            MsgBox(ex.Message)
            conn.Close()
        End Try
        If DataGridView3(0, 0).Value Is Nothing Then
            MsgBox("Usted no cuenta con revisiones de bandejas pendientes o posibles", False, "Info. Bandejas")
        End If
    End Sub

    Dim flag As Byte

    Private Sub DataGridView2_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridView2.SelectionChanged
        Label17.Text = ""
        Label18.Text = ""
        Label19.Text = ""
        Try
            Dim fila_actual As Integer = (DataGridView2.CurrentRow.Index)
            If IsDBNull(DataGridView2(0, fila_actual).Value) Then
                Button7.Enabled = False
                ComboBox6.Text = ""
                Exit Sub
            End If
            If fila_actual = (DataGridView2.RowCount - 1) Then
                flag = 0
                Label17.Visible = False
                Label18.Visible = False
                Label19.Visible = False
                Button7.Visible = False
            Else
                flag = 1
                Label17.Visible = True
                Label18.Visible = True
                Label19.Visible = True
                Button7.Visible = True
            End If
            If IsDBNull(DataGridView2(8, fila_actual).Value) Then
                TextBox5.Visible = False
                TextBox9.Visible = False
                Label12.Visible = False
                Label13.Visible = False
                Button7.Visible = True
                Button7.Enabled = True
            Else
                Label12.Visible = True
                TextBox5.Visible = True
                TextBox5.Text = ""
                'Label13.Visible = True
                'TextBox9.Visible = True
                TextBox9.Text = ""
                Button7.Enabled = False
                If DataGridView2(5, fila_actual).Value = "Finalizado" Then
                    TextBox5.Text = DataGridView2(4, fila_actual).Value
                    TextBox5.Visible = True
                    TextBox5.Enabled = False
                    If IsDBNull(DataGridView2(6, fila_actual).Value) Then
                        TextBox9.Text = ""
                    Else
                        TextBox9.Text = DataGridView2(6, fila_actual).Value
                    End If
                    TextBox9.Visible = True
                    TextBox9.Enabled = False
                    Label12.Visible = True
                    Label13.Visible = True
                ElseIf DataGridView2(5, fila_actual).Value = "Revisado" Or DataGridView2(5, fila_actual).Value = "Pendiente" Then
                    If IsDBNull(DataGridView2(4, fila_actual).Value) Then
                        Button7.Visible = True
                        Button7.Enabled = True
                    Else
                        TextBox5.Text = DataGridView2(4, fila_actual).Value
                        TextBox5.Visible = True
                        TextBox5.Enabled = False
                        'TextBox9.Visible = True
                        Label12.Visible = True
                    End If
                    If IsDBNull(DataGridView2(6, fila_actual).Value) Then
                        If TextBox5.Text <> "" Then
                            TextBox5.Text = DataGridView2(4, fila_actual).Value
                            TextBox5.Visible = True
                        End If
                    Else
                        TextBox9.Text = DataGridView2(6, fila_actual).Value
                        TextBox9.Visible = True
                        TextBox9.Enabled = False
                        'TextBox9.Visible = True
                        Label13.Visible = True
                    End If
                    If IsDBNull(DataGridView2(4, fila_actual).Value) And DataGridView2(8, fila_actual).Value <> "" Then
                        TextBox5.Enabled = True
                        TextBox5.Visible = True
                        Label12.Visible = True
                        Button7.Visible = True
                        Button7.Enabled = True
                    End If
                    If IsDBNull(DataGridView2(7, fila_actual).Value) Then

                    ElseIf IsDBNull(DataGridView2(6, fila_actual).Value) And DataGridView2(7, fila_actual).Value = "No" Then
                        TextBox5.Visible = True
                        'TextBox9.Visible = True
                        Label12.Visible = True
                        TextBox9.Visible = True
                        TextBox9.Enabled = True
                        'Label12.Visible = True
                        Label13.Visible = True
                        Button7.Enabled = True
                        Button7.Visible = True
                    End If

                End If

            End If
        Catch ex As Exception
        End Try

    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        Dim t_asignacion As String
        Dim t_revision As String
        If IsDBNull(DataGridView2(0, 0).Value) Then
            MsgBox("No existen Muestras para revisar actualmente")
            Exit Sub
        End If
        Dim fila_actual As Integer = (DataGridView2.CurrentRow.Index)
        Dim analista As Integer = ComboBox6.SelectedValue
        If IsDBNull(DataGridView2(8, fila_actual).Value) Then
            t_asignacion = String.Format(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            Dim reader As MySqlDataReader
            Try
                conn.Open()
                Dim query As String = "UPDATE rev_muestras SET Tiempo_A = '" & t_asignacion & "', AnalistNo= '" & analista & "' WHERE Muestra_ID='" & DataGridView2(0, fila_actual).Value.ToString() & "';"
                Dim cmd As New MySqlCommand(query, conn)
                reader = cmd.ExecuteReader
                conn.Close()
            Catch ex As Exception
                MsgBox(ex.Message)
                conn.Close()
            End Try
            cargar_muestras(analista)
        ElseIf IsDBNull(DataGridView2(4, fila_actual).Value) And TextBox5.Text <> "" Then
            t_revision = String.Format(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            Dim reader As MySqlDataReader
            Try
                conn.Open()
                Dim query = "UPDATE rev_muestras SET Valor_C1='" & TextBox5.Text & "', `Tiempo_V`='" & t_revision & "', Estado='Revisado' WHERE Muestra_ID='" & DataGridView2(0, fila_actual).Value.ToString() & "';"
                Dim cmd As New MySqlCommand(query, conn)
                reader = cmd.ExecuteReader
                conn.Close()
            Catch ex As Exception
                MsgBox(ex.Message)
                conn.Close()
            End Try
            cargar_muestras(analista)
        ElseIf Not (IsDBNull(DataGridView2(4, fila_actual).Value) And TextBox9.Text <> "") Then
            Dim t_revision_2 = String.Format(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            Dim reader As MySqlDataReader
            Try
                conn.Open()
                Dim query = "UPDATE rev_muestras SET Tiempo_V2 = '" & t_revision_2 & "', Estado ='Revisado', Valor_C2 ='" & TextBox9.Text & "' WHERE `Muestra_ID`='" & DataGridView2(0, fila_actual).Value.ToString() & "';"
                Dim cmd As New MySqlCommand(query, conn)
                reader = cmd.ExecuteReader
                conn.Close()
            Catch ex As Exception
                MsgBox(ex.Message)
                conn.Close()
            End Try
            cargar_muestras(analista)
        End If


    End Sub

    Private Sub Button12_Click(sender As Object, e As EventArgs) Handles Button12.Click
        ComboBox6.Enabled = True
        Button15.Enabled = True
        Button11.Enabled = True
        TabControl1.TabPages.Remove(TabPage1)
        ComboBox6.Text = ""
        MsgBox("Desconectado", False, "Log-Out")
        DataGridView2.Visible = False
        DataGridView3.Visible = False
        DataGridView2.DataSource = Nothing
        DataGridView2.Rows.Clear()
        DataGridView3.DataSource = Nothing
        DataGridView3.Rows.Clear()
        GroupBox4.Visible = False
        GroupBox5.Visible = False
    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        DataGridView2.Visible = True
        DataGridView3.Visible = True
        Dim usuario As Integer = 1
        TextBoxContraseña.Text = ""
        TextBoxRespuestaForm2.Text = ""
        If usuario = 0 Then
            MsgBox("Seleccione un analista", False, "Error")
            Exit Sub
        End If
        Form2.ShowDialog()
        Dim respuestaform2 As String = TextBoxRespuestaForm2.Text
        If respuestaform2 = "1" Then
            Dim password As String = TextBoxContraseña.Text
            Dim bd_password As String
            Try
                conn.Open()
                Dim cmd As New MySqlCommand(String.Format("Select contraseña FROM analistas
                                                       Where AnalistNo = " & usuario & ";"), conn)
                bd_password = Convert.ToString(cmd.ExecuteScalar())
                conn.Close()
            Catch ex As Exception
                MsgBox(ex.Message, False, "Error")
                conn.Close()
                Exit Sub
            End Try

            If password = bd_password Then
                MsgBox("Conectado, ahora cuenta con permisos de administrador", False, "Log-In")
                TabControl1.TabPages.Insert(3, TabPage1)
                Dim reader As MySqlDataReader
                Dim query As String
                Try
                    conn.Open()
                    query = "SELECT Muestra_ID, rev_muestras.Muestra_No as 'Muestra',pruebas.Nombre as 'Prueba', rev_muestras.Valor_In, rev_muestras.Valor_C1, rev_muestras.Estado, rev_muestras.Valor_C2, rev_muestras.Pasa, analistas.Nombre as 'Revisa'
                    FROM analistas inner join rev_muestras on analistas.AnalistNo = rev_muestras.AnalistNo inner join pruebas on rev_muestras.PrueNo = pruebas.PrueNo;"
                    Dim cmd As New MySqlCommand(query, conn)

                    reader = cmd.ExecuteReader()

                    Dim table As New DataTable
                    table.Load(reader)
                    DataGridView2.DataSource = table
                    DataGridView2.ReadOnly = True
                    DataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect

                    reader.Close()
                    conn.Close()
                Catch ex As MySqlException
                    MsgBox(ex.Message)
                    conn.Close()
                End Try

                Try
                    conn.Open()
                    query = "SELECT Bandeja_ID, rev_bandejas.Bandeja_No as 'Bandeja', pruebas.Nombre as 'Prueba', rev_bandejas.Comentario, rev_bandejas.Comentario_Revision, rev_bandejas.Estado, rev_bandejas.Comentario_Revision_2, rev_bandejas.Pasa, analistas.Nombre as 'Revisa'
                    FROM analistas inner join rev_bandejas on analistas.AnalistNo = rev_bandejas.AnalistNo inner join pruebas on rev_bandejas.PrueNo = pruebas.PrueNo;"
                    Dim cmd As New MySqlCommand(query, conn)

                    reader = cmd.ExecuteReader()

                    Dim table As New DataTable
                    table.Load(reader)
                    DataGridView3.DataSource = table
                    DataGridView3.ReadOnly = True
                    DataGridView3.SelectionMode = DataGridViewSelectionMode.FullRowSelect

                    reader.Close()
                    conn.Close()
                Catch ex As MySqlException
                    MsgBox(ex.Message)
                    conn.Close()
                End Try

                ComboBox6.Enabled = False
                Button15.Enabled = False
                Button11.Enabled = False
            Else
                MsgBox("Constraseña Incorrecta", False, "Error")
                Exit Sub
            End If
        Else
            Exit Sub
        End If
    End Sub
End Class