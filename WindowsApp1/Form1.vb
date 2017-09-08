'Programador: Nicolas Alberto Agudelo Herrera

Imports MySql.Data.MySqlClient
Imports System.Configuration
Public Class MainForm
    Implements IMessageFilter
    Dim conn As New MySqlConnection

    'Main
    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Timer2.Start()
        Connect()
        Cargar_analistas()
        LlenarComboBox7()
        CmbBxFiltroPrueba.Text = Nothing
        TabControl1.TabPages.Remove(TabPageAdmin)

    End Sub

    'Conexion a la base de datos
    Public Sub Connect()

        'Dim DatabaseName As String = "bd_revision"
        'Dim server As String = "localhost"
        'Dim userName As String = "root"
        'Dim password As String = "dm900494665"
        'If Not conn Is Nothing Then conn.Close()
        'conn.ConnectionString = String.Format("server={0}; user id={1}; password={2}; database={3}; pooling=false; Charset = utf8;", server, userName, password, DatabaseName)

        conn.ConnectionString = ConfigurationManager.ConnectionStrings("cs").ConnectionString
        Try
            conn.Open()
            Console.WriteLine("conectandose a la base de datos")

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        conn.Close()

        'Llenando combobox utilizado en la pestaña de administrador para elegir la tabla a mostrar


        CbBxTablas.Items.Add("Analistas")
        CbBxTablas.Items.Add("Pruebas")
        CbBxTablas.Items.Add("Relacion Analistas Pruebas")
        CbBxTablas.Items.Add("Muestras Asignadas")
        CbBxTablas.Items.Add("Muestras no Asignadas")
        CbBxTablas.Items.Add("Bandejas Asignadas")
        CbBxTablas.Items.Add("Bandejas no Asignadas")
        CbBxTablas.Items.Add("Historial de Muestras")
        CbBxTablas.Items.Add("Historial de Bandejas")

        CbBxTablas.SelectedItem = "Analistas"

        Conteo_muestras()
    End Sub

    'Cuenta de muestras por revisar en el sistema
    Private Sub Conteo_muestras()
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

        TabPageRevisionesXPrueba.Controls.Clear()

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
            LabelArray(i) = New Label With {
                .Location = New Point(X1, Y1)
            }
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
            LabelArray(i).Parent = Me.TabPageRevisionesXPrueba
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
            TextBoxArray(i) = New TextBox With {
                .Location = New Point(X1, Y1),
                .Width = 110,
                .Height = 15,
                .Name = "Textboxs" & i,
                .Text = ""
            }
            TextBoxArray(i).Text = cantidad_pendiente.ToString
            TextBoxArray(i).Visible = True
            TextBoxArray(i).ReadOnly = True
            TextBoxArray(i).Parent = Me.TabPageRevisionesXPrueba

            j = j + 1
            j1 = j1 + 1
            If i Mod 7 = 0 Then
                m = m + 170
                r = 15
                j1 = 0
            End If
        Next

    End Sub

    'Control para cargar las tablas cuando se seleccionan en la pestañañ de Admin
    Private Sub CbBxTablas_SelectedValueChanged(sender As Object, e As EventArgs) Handles CbBxTablas.SelectedValueChanged
        GroupBoxAdmin.Visible = False
        Cargar()
    End Sub

    'Cargar los nombres de los analistas en el CmbBxAnalistas
    Private Sub Cargar_analistas()
        CmbBxAnalistas.Items.Clear()
        CmbBxAnalistas.Enabled = True
        Dim query As String = " Select AnalistNo, Nombre from analistas
                                where AnalistNo <> 1;"
        Dim cmd As New MySqlCommand(query, conn)
        Dim sqlAdap As New MySqlDataAdapter(cmd)
        Dim dtRecord As New DataTable
        sqlAdap.Fill(dtRecord)
        CmbBxAnalistas.DataSource = dtRecord
        CmbBxAnalistas.DisplayMember = "Nombre"
        CmbBxAnalistas.ValueMember = "AnalistNo"
        CmbBxAnalistas.Text = ""
        CmbBxAnalistas.SelectedValue = 0
    End Sub

    'Carga los contenidos de los DGVMuestras y DGVBandejas teniendo en cuenta los filtros que se estan usando
    Public Sub Cargar()
        Dim Nombre_Tabla As String = CbBxTablas.Text.ToString()
        Dim query As String
        Dim filtro As String = TxtBxFiltroRegistro.Text
        Dim filtro2 As String = CmbBxFiltroPrueba.Text

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

            If filtro = "" And filtro2 = "" Then
                query = "SELECT Muestra_ID, rev_muestras.Muestra_No as 'Muestra',pruebas.Nombre as 'Prueba', rev_muestras.Valor_In, rev_muestras.Valor_C1, rev_muestras.Estado, rev_muestras.Valor_C2, rev_muestras.Pasa, analistas.Nombre as 'Revisa', Tiempo_C, Tiempo_A, Tiempo_V, Tiempo_A2, Tiempo_V2, Tiempo_T
                    FROM analistas inner join rev_muestras on analistas.AnalistNo = rev_muestras.AnalistNo inner join pruebas on rev_muestras.PrueNo = pruebas.PrueNo
                    where rev_muestras.Estado in ('Revisado','Pendiente'); "
            ElseIf (Not filtro = "") And filtro2 = "" Then
                query = "SELECT Muestra_ID, rev_muestras.Muestra_No as 'Muestra',pruebas.Nombre as 'Prueba', rev_muestras.Valor_In, rev_muestras.Valor_C1, rev_muestras.Estado, rev_muestras.Valor_C2, rev_muestras.Pasa, analistas.Nombre as 'Revisa', Tiempo_C, Tiempo_A, Tiempo_V, Tiempo_A2, Tiempo_V2, Tiempo_T
                    FROM analistas inner join rev_muestras on analistas.AnalistNo = rev_muestras.AnalistNo inner join pruebas on rev_muestras.PrueNo = pruebas.PrueNo
                    where Muestra_No like '" & filtro & "%' and rev_muestras.Estado in ('Revisado','Pendiente'); "
            ElseIf filtro = "" And (Not filtro2 = "") Then
                query = "SELECT Muestra_ID, rev_muestras.Muestra_No as 'Muestra',pruebas.Nombre as 'Prueba', rev_muestras.Valor_In, rev_muestras.Valor_C1, rev_muestras.Estado, rev_muestras.Valor_C2, rev_muestras.Pasa, analistas.Nombre as 'Revisa', Tiempo_C, Tiempo_A, Tiempo_V, Tiempo_A2, Tiempo_V2, Tiempo_T
                    FROM analistas inner join rev_muestras on analistas.AnalistNo = rev_muestras.AnalistNo inner join pruebas on rev_muestras.PrueNo = pruebas.PrueNo
                    where pruebas.Nombre = '" & filtro2 & "' and rev_muestras.Estado in ('Revisado','Pendiente'); "
            ElseIf (Not filtro = "") And (Not filtro2 = "") Then
                query = "SELECT Muestra_ID, rev_muestras.Muestra_No as 'Muestra',pruebas.Nombre as 'Prueba', rev_muestras.Valor_In, rev_muestras.Valor_C1, rev_muestras.Estado, rev_muestras.Valor_C2, rev_muestras.Pasa, analistas.Nombre as 'Revisa', Tiempo_C, Tiempo_A, Tiempo_V, Tiempo_A2, Tiempo_V2, Tiempo_T
                    FROM analistas inner join rev_muestras on analistas.AnalistNo = rev_muestras.AnalistNo inner join pruebas on rev_muestras.PrueNo = pruebas.PrueNo
                    where pruebas.Nombre = '" & filtro2 & "' and Muestra_No like '" & filtro & "%' and rev_muestras.Estado in ('Revisado','Pendiente'); "
            End If

        ElseIf Nombre_Tabla = "Muestras no Asignadas" Then

            If filtro = "" And filtro2 = "" Then

                query = "SELECT Muestra_ID, rev_muestras.Muestra_No as 'Muestra',pruebas.Nombre as 'Prueba', rev_muestras.Valor_In, rev_muestras.Valor_C1, rev_muestras.Estado, rev_muestras.Valor_C2, rev_muestras.Pasa, Tiempo_C, Tiempo_A, Tiempo_V, Tiempo_A2, Tiempo_V2, Tiempo_T
                    FROM rev_muestras inner join pruebas on rev_muestras.PrueNo = pruebas.PrueNo
                    where rev_muestras.Estado in ('Revisado','Pendiente') and rev_muestras.AnalistNo is NULL; "

            ElseIf (Not filtro = "") And filtro2 = "" Then

                query = "SELECT Muestra_ID, rev_muestras.Muestra_No as 'Muestra',pruebas.Nombre as 'Prueba', rev_muestras.Valor_In, rev_muestras.Valor_C1, rev_muestras.Estado, rev_muestras.Valor_C2, rev_muestras.Pasa, Tiempo_C, Tiempo_A, Tiempo_V, Tiempo_A2, Tiempo_V2, Tiempo_T
                    FROM rev_muestras inner join pruebas on rev_muestras.PrueNo = pruebas.PrueNo
                    where Muestra_No like '" & filtro & "%' and rev_muestras.Estado in ('Revisado','Pendiente') and rev_muestras.AnalistNo is NULL; "

            ElseIf filtro = "" And (Not filtro2 = "") Then

                query = "SELECT Muestra_ID, rev_muestras.Muestra_No as 'Muestra',pruebas.Nombre as 'Prueba', rev_muestras.Valor_In, rev_muestras.Valor_C1, rev_muestras.Estado, rev_muestras.Valor_C2, rev_muestras.Pasa, Tiempo_C, Tiempo_A, Tiempo_V, Tiempo_A2, Tiempo_V2, Tiempo_T
                    FROM rev_muestras inner join pruebas on rev_muestras.PrueNo = pruebas.PrueNo
                    where pruebas.Nombre = '" & filtro2 & "' and rev_muestras.Estado in ('Revisado','Pendiente') and rev_muestras.AnalistNo is NULL; "

            ElseIf (Not filtro = "") And (Not filtro2 = "") Then

                query = "SELECT Muestra_ID, rev_muestras.Muestra_No as 'Muestra',pruebas.Nombre as 'Prueba', rev_muestras.Valor_In, rev_muestras.Valor_C1, rev_muestras.Estado, rev_muestras.Valor_C2, rev_muestras.Pasa, Tiempo_C, Tiempo_A, Tiempo_V, Tiempo_A2, Tiempo_V2, Tiempo_T
                    FROM rev_muestras inner join pruebas on rev_muestras.PrueNo = pruebas.PrueNo
                    where pruebas.Nombre = '" & filtro2 & "' and Muestra_No like '" & filtro & "%' and rev_muestras.Estado in ('Revisado','Pendiente') and rev_muestras.AnalistNo is NULL; "

            End If

        ElseIf Nombre_Tabla = "Bandejas Asignadas" Then

            If filtro = "" And filtro2 = "" Then

                query = "SELECT Bandeja_ID, rev_bandejas.Bandeja_No as 'Bandeja', pruebas.Nombre as 'Prueba', rev_bandejas.Comentario, rev_bandejas.Comentario_Revision, rev_bandejas.Estado, rev_bandejas.Comentario_Revision_2, rev_bandejas.Pasa, analistas.Nombre as 'Revisa', Tiempo_C, Tiempo_A, Tiempo_V, Tiempo_A2, Tiempo_V2, Tiempo_T
                    FROM analistas inner join rev_bandejas on analistas.AnalistNo = rev_bandejas.AnalistNo inner join pruebas on rev_bandejas.PrueNo = pruebas.PrueNo
                    where rev_bandejas.Estado in ('Revisado','Pendiente');"

            ElseIf (Not filtro = "") And filtro2 = "" Then

                query = "SELECT Bandeja_ID, rev_bandejas.Bandeja_No as 'Bandeja', pruebas.Nombre as 'Prueba', rev_bandejas.Comentario, rev_bandejas.Comentario_Revision, rev_bandejas.Estado, rev_bandejas.Comentario_Revision_2, rev_bandejas.Pasa, analistas.Nombre as 'Revisa', Tiempo_C, Tiempo_A, Tiempo_V, Tiempo_A2, Tiempo_V2, Tiempo_T
                    FROM analistas inner join rev_bandejas on analistas.AnalistNo = rev_bandejas.AnalistNo inner join pruebas on rev_bandejas.PrueNo = pruebas.PrueNo
                    where Bandeja_No like '" & filtro & "%' and rev_bandejas.Estado in ('Revisado','Pendiente');"

            ElseIf filtro = "" And (Not filtro2 = "") Then

                query = "SELECT Bandeja_ID, rev_bandejas.Bandeja_No as 'Bandeja', pruebas.Nombre as 'Prueba', rev_bandejas.Comentario, rev_bandejas.Comentario_Revision, rev_bandejas.Estado, rev_bandejas.Comentario_Revision_2, rev_bandejas.Pasa, analistas.Nombre as 'Revisa', Tiempo_C, Tiempo_A, Tiempo_V, Tiempo_A2, Tiempo_V2, Tiempo_T
                    FROM analistas inner join rev_bandejas on analistas.AnalistNo = rev_bandejas.AnalistNo inner join pruebas on rev_bandejas.PrueNo = pruebas.PrueNo
                    where pruebas.Nombre = '" & filtro2 & "' and rev_bandejas.Estado in ('Revisado','Pendiente');"

            ElseIf (Not filtro = "") And (Not filtro2 = "") Then

                query = "SELECT Bandeja_ID, rev_bandejas.Bandeja_No as 'Bandeja', pruebas.Nombre as 'Prueba', rev_bandejas.Comentario, rev_bandejas.Comentario_Revision, rev_bandejas.Estado, rev_bandejas.Comentario_Revision_2, rev_bandejas.Pasa, analistas.Nombre as 'Revisa', Tiempo_C, Tiempo_A, Tiempo_V, Tiempo_A2, Tiempo_V2, Tiempo_T
                    FROM analistas inner join rev_bandejas on analistas.AnalistNo = rev_bandejas.AnalistNo inner join pruebas on rev_bandejas.PrueNo = pruebas.PrueNo
                    where pruebas.Nombre = '" & filtro2 & "' and Bandeja_No like '" & filtro & "%' and rev_bandejas.Estado in ('Revisado','Pendiente');"

            End If

        ElseIf Nombre_Tabla = "Bandejas no Asignadas" Then

            If filtro = "" And filtro2 = "" Then

                query = "SELECT Bandeja_ID, rev_bandejas.Bandeja_No as 'Bandeja', pruebas.Nombre as 'Prueba', rev_bandejas.Comentario, rev_bandejas.Comentario_Revision, rev_bandejas.Estado, rev_bandejas.Comentario_Revision_2, rev_bandejas.Pasa, Tiempo_C, Tiempo_A, Tiempo_V, Tiempo_A2, Tiempo_V2, Tiempo_T
                    FROM rev_bandejas inner join pruebas on rev_bandejas.PrueNo = pruebas.PrueNo
                    where rev_bandejas.Estado in ('Revisado','Pendiente') and rev_bandejas.AnalistNo is null;"

            ElseIf (Not filtro = "") And filtro2 = "" Then

                query = "SELECT Bandeja_ID, rev_bandejas.Bandeja_No as 'Bandeja', pruebas.Nombre as 'Prueba', rev_bandejas.Comentario, rev_bandejas.Comentario_Revision, rev_bandejas.Estado, rev_bandejas.Comentario_Revision_2, rev_bandejas.Pasa, Tiempo_C, Tiempo_A, Tiempo_V, Tiempo_A2, Tiempo_V2, Tiempo_T
                    FROM rev_bandejas inner join pruebas on rev_bandejas.PrueNo = pruebas.PrueNo
                    where Bandeja_No like '" & filtro & "%' and rev_bandejas.Estado in ('Revisado','Pendiente') and rev_bandejas.AnalistNo is null;"

            ElseIf filtro = "" And (Not filtro2 = "") Then

                query = "SELECT Bandeja_ID, rev_bandejas.Bandeja_No as 'Bandeja', pruebas.Nombre as 'Prueba', rev_bandejas.Comentario, rev_bandejas.Comentario_Revision, rev_bandejas.Estado, rev_bandejas.Comentario_Revision_2, rev_bandejas.Pasa, Tiempo_C, Tiempo_A, Tiempo_V, Tiempo_A2, Tiempo_V2, Tiempo_T
                    FROM rev_bandejas inner join pruebas on rev_bandejas.PrueNo = pruebas.PrueNo
                    where pruebas.Nombre = '" & filtro2 & "' and rev_bandejas.Estado in ('Revisado','Pendiente')and rev_bandejas.AnalistNo is null;"

            ElseIf (Not filtro = "") And (Not filtro2 = "") Then

                query = "SELECT Bandeja_ID, rev_bandejas.Bandeja_No as 'Bandeja', pruebas.Nombre as 'Prueba', rev_bandejas.Comentario, rev_bandejas.Comentario_Revision, rev_bandejas.Estado, rev_bandejas.Comentario_Revision_2, rev_bandejas.Pasa, Tiempo_C, Tiempo_A, Tiempo_V, Tiempo_A2, Tiempo_V2, Tiempo_T
                    FROM rev_bandejas inner join pruebas on rev_bandejas.PrueNo = pruebas.PrueNo
                    where pruebas.Nombre = '" & filtro2 & "' and Bandeja_No like '" & filtro & "%' and rev_bandejas.Estado in ('Revisado','Pendiente') and rev_bandejas.AnalistNo is null;"

            End If

        ElseIf Nombre_Tabla = "Historial de Muestras" Then

            If filtro = "" And filtro2 = "" Then

                query = "SELECT Muestra_ID, rev_muestras.Muestra_No as 'Numero de muestra',pruebas.Nombre as 'Prueba', rev_muestras.Valor_In, rev_muestras.Valor_C1, rev_muestras.Estado, rev_muestras.Valor_C2, rev_muestras.Pasa, analistas.Nombre as 'Revisa', Tiempo_C, Tiempo_A, Tiempo_V, Tiempo_A2, Tiempo_V2, Tiempo_T
                    FROM analistas inner join rev_muestras on analistas.AnalistNo = rev_muestras.AnalistNo inner join pruebas on rev_muestras.PrueNo = pruebas.PrueNo
                    where rev_muestras.Estado in ('Finalizado');"

            ElseIf (Not filtro = "") And filtro2 = "" Then

                query = "SELECT Muestra_ID, rev_muestras.Muestra_No as 'Numero de muestra',pruebas.Nombre as 'Prueba', rev_muestras.Valor_In, rev_muestras.Valor_C1, rev_muestras.Estado, rev_muestras.Valor_C2, rev_muestras.Pasa, analistas.Nombre as 'Revisa', Tiempo_C, Tiempo_A, Tiempo_V, Tiempo_A2, Tiempo_V2, Tiempo_T
                    FROM analistas inner join rev_muestras on analistas.AnalistNo = rev_muestras.AnalistNo inner join pruebas on rev_muestras.PrueNo = pruebas.PrueNo
                    where Muestra_No like '" & filtro & "%' and rev_muestras.Estado in ('Finalizado'); "

            ElseIf filtro = "" And (Not filtro2 = "") Then
                query = "SELECT Muestra_ID, rev_muestras.Muestra_No as 'Numero de muestra',pruebas.Nombre as 'Prueba', rev_muestras.Valor_In, rev_muestras.Valor_C1, rev_muestras.Estado, rev_muestras.Valor_C2, rev_muestras.Pasa, analistas.Nombre as 'Revisa', Tiempo_C, Tiempo_A, Tiempo_V, Tiempo_A2, Tiempo_V2, Tiempo_T
                    FROM analistas inner join rev_muestras on analistas.AnalistNo = rev_muestras.AnalistNo inner join pruebas on rev_muestras.PrueNo = pruebas.PrueNo
                    where pruebas.Nombre = '" & filtro2 & "' and rev_muestras.Estado in ('Finalizado'); "
            ElseIf (Not filtro = "") And (Not filtro2 = "") Then
                query = "SELECT Muestra_ID, rev_muestras.Muestra_No as 'Numero de muestra',pruebas.Nombre as 'Prueba', rev_muestras.Valor_In, rev_muestras.Valor_C1, rev_muestras.Estado, rev_muestras.Valor_C2, rev_muestras.Pasa, analistas.Nombre as 'Revisa', Tiempo_C, Tiempo_A, Tiempo_V, Tiempo_A2, Tiempo_V2, Tiempo_T
                    FROM analistas inner join rev_muestras on analistas.AnalistNo = rev_muestras.AnalistNo inner join pruebas on rev_muestras.PrueNo = pruebas.PrueNo
                    where pruebas.Nombre = '" & filtro2 & "' and Muestra_No like '" & filtro & "%' and rev_muestras.Estado in ('Finalizado'); "
            End If

        ElseIf Nombre_Tabla = "Historial de Bandejas" Then

            If filtro = "" And filtro2 = "" Then

                query = "SELECT Bandeja_ID, rev_bandejas.Bandeja_No as 'Bandeja', pruebas.Nombre as 'Prueba', rev_bandejas.Comentario, rev_bandejas.Comentario_Revision, rev_bandejas.Estado, rev_bandejas.Comentario_Revision_2, rev_bandejas.Pasa, analistas.Nombre as 'Revisa', Tiempo_C, Tiempo_A, Tiempo_V, Tiempo_A2, Tiempo_V2, Tiempo_T
                    FROM analistas inner join rev_bandejas on analistas.AnalistNo = rev_bandejas.AnalistNo inner join pruebas on rev_bandejas.PrueNo = pruebas.PrueNo
                    where rev_bandejas.Estado in ('Finalizado');"

            ElseIf (Not filtro = "") And filtro2 = "" Then

                query = "SELECT Bandeja_ID, rev_bandejas.Bandeja_No as 'Bandeja', pruebas.Nombre as 'Prueba', rev_bandejas.Comentario, rev_bandejas.Comentario_Revision, rev_bandejas.Estado, rev_bandejas.Comentario_Revision_2, rev_bandejas.Pasa, analistas.Nombre as 'Revisa', Tiempo_C, Tiempo_A, Tiempo_V, Tiempo_A2, Tiempo_V2, Tiempo_T
                    FROM analistas inner join rev_bandejas on analistas.AnalistNo = rev_bandejas.AnalistNo inner join pruebas on rev_bandejas.PrueNo = pruebas.PrueNo
                    where Bandeja_No like '" & filtro & "%' and rev_bandejas.Estado in ('Finalizado');"

            ElseIf filtro = "" And (Not filtro2 = "") Then

                query = "SELECT Bandeja_ID, rev_bandejas.Bandeja_No as 'Bandeja', pruebas.Nombre as 'Prueba', rev_bandejas.Comentario, rev_bandejas.Comentario_Revision, rev_bandejas.Estado, rev_bandejas.Comentario_Revision_2, rev_bandejas.Pasa, analistas.Nombre as 'Revisa', Tiempo_C, Tiempo_A, Tiempo_V, Tiempo_A2, Tiempo_V2, Tiempo_T
                    FROM analistas inner join rev_bandejas on analistas.AnalistNo = rev_bandejas.AnalistNo inner join pruebas on rev_bandejas.PrueNo = pruebas.PrueNo
                    where pruebas.Nombre = '" & filtro2 & "' and rev_bandejas.Estado in ('Finalizado');"

            ElseIf (Not filtro = "") And (Not filtro2 = "") Then

                query = "SELECT Bandeja_ID, rev_bandejas.Bandeja_No as 'Bandeja', pruebas.Nombre as 'Prueba', rev_bandejas.Comentario, rev_bandejas.Comentario_Revision, rev_bandejas.Estado, rev_bandejas.Comentario_Revision_2, rev_bandejas.Pasa, analistas.Nombre as 'Revisa', Tiempo_C, Tiempo_A, Tiempo_V, Tiempo_A2, Tiempo_V2, Tiempo_T
                    FROM analistas inner join rev_bandejas on analistas.AnalistNo = rev_bandejas.AnalistNo inner join pruebas on rev_bandejas.PrueNo = pruebas.PrueNo
                    where pruebas.Nombre = '" & filtro2 & "' and Bandeja_No like '" & filtro & "%' and rev_bandejas.Estado in ('Finalizado');"

            End If

            'Descomentar esto si se altera la linea de codigo donde CbBxTablas.SelectedItem = "Analistas" o si se cambia el estilo del CbBxTablas
            'Else
            'MsgBox("No ha seleccionado ninguna tabla", False, "Error")
            'Exit Sub
        End If

        Dim cmd As New MySqlCommand(query, conn)
        Dim reader As MySqlDataReader

        LabelAdmin.Text = Nombre_Tabla

        Try

            conn.Open()
            Console.WriteLine("conectandose a la base de datos")

            reader = cmd.ExecuteReader()

            Dim table As New DataTable
            table.Load(reader)
            DGVAdmin.DataSource = table
            DGVAdmin.ReadOnly = True
            DGVAdmin.AllowUserToResizeColumns = True
            DGVAdmin.SelectionMode = DataGridViewSelectionMode.FullRowSelect

            reader.Close()
            conn.Close()
        Catch ex As MySqlException
            MsgBox(ex.Message)
            conn.Close()
        End Try

        If Nombre_Tabla = "Analistas" Or Nombre_Tabla = "Pruebas" Then

            DGVAdmin.Columns(0).Visible = False
            GroupBoxControlesTablas.Visible = True
            BtnNuevoRegistro.Enabled = True
            BtnModificarRegistro.Enabled = True
            BtnEliminarRegistro.Enabled = True
            BtnNuevoRegistro.Visible = True
            BtnModificarRegistro.Visible = True
            BtnEliminarRegistro.Visible = True
            BtnSi.Visible = False
            BtnNo.Visible = False
            GroupBoxAdmin.Text = Nothing


        ElseIf Nombre_Tabla = "Relacion Analistas Pruebas" Then

            GroupBoxControlesTablas.Visible = True
            BtnNuevoRegistro.Enabled = True
            BtnModificarRegistro.Enabled = True
            BtnEliminarRegistro.Enabled = True
            BtnNuevoRegistro.Visible = True
            BtnModificarRegistro.Visible = True
            BtnEliminarRegistro.Visible = True
            BtnSi.Visible = False
            BtnNo.Visible = False
            GroupBoxAdmin.Text = Nothing
            LlenarComboBox2()
            LlenarComboBox3()

        ElseIf Nombre_Tabla = "Muestras Asignadas" Or Nombre_Tabla = "Bandejas Asignadas" Then

            DGVAdmin.Columns(0).Visible = False
            DGVAdmin.Columns(9).Visible = False
            DGVAdmin.Columns(10).Visible = False
            DGVAdmin.Columns(11).Visible = False
            DGVAdmin.Columns(12).Visible = False
            DGVAdmin.Columns(13).Visible = False
            DGVAdmin.Columns(14).Visible = False
            LabelTimerAsignacion.Visible = False
            LabelTimerVerificacion.Visible = False
            LabelTimerTotal.Visible = False
            GroupBoxAdmin.Visible = True
            TextBox1.Visible = False
            TextBox2.Visible = False
            ComboBox8.Visible = False
            ComboBox9.Visible = False
            Label1.Visible = False
            Label2.Visible = False
            BtnModificar.Visible = False
            ComboBox2.Visible = False
            ComboBox3.Visible = False
            TextBox3.Visible = False
            TextBox6.Visible = False
            PanelAdmin.Visible = True
            Label3.Visible = False
            ComboBox4.Visible = False
            GroupBoxControlesTablas.Visible = False
            ButtonAgregar.Visible = False
            GroupBoxAdmin.Text = "Pasa"
            ComboBox5.Visible = False
            BtnSi.Visible = True
            BtnNo.Visible = True
            Label6.Visible = False

        ElseIf Nombre_Tabla = "Muestras no Asignadas" Or Nombre_Tabla = "Bandejas no Asignadas" Then

            DGVAdmin.Columns(0).Visible = False
            DGVAdmin.Columns(8).Visible = False
            DGVAdmin.Columns(9).Visible = False
            DGVAdmin.Columns(10).Visible = False
            DGVAdmin.Columns(11).Visible = False
            DGVAdmin.Columns(12).Visible = False
            DGVAdmin.Columns(13).Visible = False
            BtnSi.Visible = False
            BtnNo.Visible = False
            GroupBoxAdmin.Text = Nothing
            GroupBoxControlesTablas.Visible = True
            BtnNuevoRegistro.Visible = True
            BtnModificarRegistro.Visible = False
            BtnEliminarRegistro.Visible = False
            BtnNuevoRegistro.Enabled = True
            BtnModificarRegistro.Enabled = False
            BtnEliminarRegistro.Enabled = False
            LabelTimerAsignacion.Visible = False
            LabelTimerVerificacion.Visible = False
            LabelTimerTotal.Visible = False

        ElseIf Nombre_Tabla = "Historial de Muestras" Or Nombre_Tabla = "Historial de Bandejas" Then

            BtnSi.Visible = False
            BtnNo.Visible = False
            GroupBoxAdmin.Text = Nothing
            GroupBoxControlesTablas.Visible = False
            DGVAdmin.Columns(0).Visible = False
            DGVAdmin.Columns(9).Visible = False
            DGVAdmin.Columns(10).Visible = False
            DGVAdmin.Columns(11).Visible = False
            DGVAdmin.Columns(12).Visible = False
            DGVAdmin.Columns(13).Visible = False
            DGVAdmin.Columns(14).Visible = False

        End If
    End Sub

    Dim Pest_actual As Integer = 0

    'Control que revisa en que pestaña se encuentra el usuario
    Private Sub TabControl1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TabControl1.SelectedIndexChanged
        If TabControl1.SelectedTab Is TabPageAdmin Then

            Pest_actual = 1
            LabelRevisionesPrueba.Visible = False
            LabelTimerAsignacion.Visible = False
            LabelTimerVerificacion.Visible = False
            LabelTimerTotal.Visible = False
        ElseIf TabControl1.SelectedTab Is TabPageMuestras Then

            Pest_actual = 2
            LabelRevisionesPrueba.Visible = False
            LabelTimerAsignacion.Visible = False
            LabelTimerVerificacion.Visible = False
            LabelTimerTotal.Visible = False
        ElseIf TabControl1.SelectedTab Is TabPageBandejas Then

            Pest_actual = 3
            LabelRevisionesPrueba.Visible = False
            LabelTimerAsignacion.Visible = False
            LabelTimerVerificacion.Visible = False
            LabelTimerTotal.Visible = False
        ElseIf TabControl1.SelectedTab Is TabPageRevisionesXPrueba Then
            Pest_actual = 0
            LabelRevisionesPrueba.Visible = True
        End If
    End Sub

    'Boton para crear nuevos registros de analistas pruebas muestras bandejas.
    Private Sub BtnNuevoRegistro_Click(sender As Object, e As EventArgs) Handles BtnNuevoRegistro.Click

        If LabelAdmin.Text = "Analistas" Then
            Label1.Text = "Nombre del analista"
            Label2.Text = "Contraseña del analista"
            Label1.Visible = True
            Label2.Visible = True
            TextBox2.PasswordChar = "*"
            TextBox1.Text = ""
            TextBox2.Text = ""
            PanelAdmin.Visible = False
            GroupBoxAdmin.Visible = True
            TextBox3.Visible = False
            TextBox6.Visible = False
            TextBox1.Visible = True
            TextBox2.Visible = True
            ComboBox2.Visible = False
            ComboBox3.Visible = False
            ComboBox8.Visible = False
            ComboBox9.Visible = False
            BtnModificar.Visible = False
            ButtonAgregar.Text = "Agregar"
            ButtonAgregar.Visible = True
            ButtonAgregar.Enabled = True
        ElseIf LabelAdmin.Text = "Pruebas" Then
            Label1.Text = "Nombre de la Prueba"
            Label2.Text = "Descripción de la Prueba"
            Label1.Visible = True
            Label2.Visible = True
            TextBox2.PasswordChar = ""
            TextBox1.Text = ""
            TextBox2.Text = ""
            GroupBoxAdmin.Visible = True
            PanelAdmin.Visible = False
            TextBox3.Visible = False
            TextBox6.Visible = False
            TextBox1.Visible = True
            TextBox2.Visible = True
            ComboBox2.Visible = False
            ComboBox3.Visible = False
            ComboBox8.Visible = False
            ComboBox9.Visible = False
            BtnModificar.Visible = False
            ButtonAgregar.Text = "Agregar"
            ButtonAgregar.Visible = True
            ButtonAgregar.Enabled = True
        ElseIf LabelAdmin.Text = "Relacion Analistas Pruebas" Then
            Label1.Text = "Nombre del Analista"
            Label2.Text = "Nombre de la Prueba"
            Label1.Visible = True
            Label2.Visible = True
            GroupBoxAdmin.Visible = True
            PanelAdmin.Visible = False
            TextBox3.Visible = False
            TextBox6.Visible = False
            TextBox1.Visible = False
            TextBox2.Visible = False
            ComboBox2.Visible = False
            ComboBox8.Visible = True
            BtnModificar.Visible = False
            ButtonAgregar.Text = "Agregar"
            ButtonAgregar.Visible = True
            ButtonAgregar.Enabled = True
            LlenarComboBox8()
            ComboBox3.Visible = False
            ComboBox9.Visible = True
            LlenarComboBox9()
        ElseIf LabelAdmin.Text = "Muestras no Asignadas" Then
            Label1.Text = "Numero de Muestra"
            Label2.Text = "Valor Ingresado"
            Label1.Visible = True
            Label2.Visible = True
            TextBox2.PasswordChar = ""
            TextBox1.Text = ""
            TextBox2.Text = ""
            ButtonAgregar.Text = "Crear Muestra"
            GroupBoxAdmin.Visible = True
            BtnModificar.Visible = False
            ButtonAgregar.Visible = True
            ButtonAgregar.Enabled = True
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
            PanelAdmin.Visible = True
            Label6.Visible = False
            ComboBox5.Visible = False
            Label3.Text = "Prueba"
            Label3.Visible = True

        ElseIf LabelAdmin.Text = "Bandejas no Asignadas" Then

            Label1.Text = "Numero de Bandeja"
            Label2.Text = "Comentario del Admin"
            Label1.Visible = True
            Label2.Visible = True
            TextBox2.PasswordChar = ""
            TextBox1.Text = ""
            TextBox2.Text = ""
            ButtonAgregar.Text = "Crear Bandeja"
            GroupBoxAdmin.Visible = True
            BtnModificar.Visible = False
            ButtonAgregar.Visible = True
            ButtonAgregar.Enabled = True
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
            PanelAdmin.Visible = True
            Label6.Visible = False
            ComboBox5.Visible = False
            Label3.Text = "Prueba"
            Label3.Visible = True

        End If
    End Sub

    'Boton para modificar los registros de analistas pruebas muestras bandejas
    Private Sub BtnModificarRegistro_Click(sender As Object, e As EventArgs) Handles BtnModificarRegistro.Click

        If LabelAdmin.Text = "Analistas" Then
            Label1.Text = "Nombre del analista"
            Label2.Text = "Contraseña del analista"
            Label1.Visible = True
            Label2.Visible = True
            TextBox2.PasswordChar = "*"
            TextBox1.Text = ""
            TextBox2.Text = ""
            GroupBoxAdmin.Visible = True
            PanelAdmin.Visible = False
            ButtonAgregar.Visible = False
            BtnModificar.Visible = True
            TextBox3.Visible = True
            TextBox6.Visible = True
            ComboBox2.Visible = False
            ComboBox3.Visible = False
            ComboBox8.Visible = False
            ComboBox9.Visible = False
            TextBox1.Visible = False
            TextBox2.Visible = False
            DGVAdmin_SelectedIndexChanged(sender, e)

        ElseIf LabelAdmin.Text = "Pruebas" Then
            Label1.Text = "Nombre de la Prueba"
            Label2.Text = "Descripción de la Prueba"
            Label1.Visible = True
            Label2.Visible = True
            TextBox2.PasswordChar = ""
            TextBox1.Text = ""
            TextBox2.Text = ""
            GroupBoxAdmin.Visible = True
            PanelAdmin.Visible = False
            ButtonAgregar.Visible = False
            BtnModificar.Visible = True
            TextBox3.Visible = True
            TextBox6.Visible = True
            ComboBox2.Visible = False
            ComboBox3.Visible = False
            ComboBox8.Visible = False
            ComboBox9.Visible = False
            TextBox1.Visible = False
            TextBox2.Visible = False
            DGVAdmin_SelectedIndexChanged(sender, e)
        ElseIf LabelAdmin.Text = "Relacion Analistas Pruebas" Then
            PanelAdmin.Visible = True
            Label6.Visible = True
            Label3.Visible = True
            Label1.Text = "Valor Actual del analista"
            Label6.Text = "Nuevo Valor del analista"
            Label2.Text = "Valor Actual de la Prueba"
            Label3.Text = "Nuevo Valor de la Prueba"
            Label1.Visible = True
            Label2.Visible = True
            GroupBoxAdmin.Visible = True
            TextBox3.Visible = False
            TextBox6.Visible = False
            TextBox1.Visible = False
            TextBox2.Visible = False
            ComboBox8.Visible = False
            ComboBox9.Visible = False
            ComboBox5.Visible = True
            ComboBox5.Enabled = True
            LlenarComboBox5()
            ComboBox4.Visible = True
            LlenarComboBox4()
            BtnModificar.Visible = True
            ButtonAgregar.Visible = False
            ComboBox2.Visible = True
            LlenarComboBox2()
            ComboBox2.Enabled = False
            ComboBox3.Visible = True
            LlenarComboBox3()
            ComboBox3.Enabled = False
            DGVAdmin_SelectedIndexChanged(sender, e)

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

    Public Sub LlenarComboBox7()
        Dim query As String = " Select PrueNo, Nombre from Pruebas
                                Order by Nombre"
        Dim cmd As New MySqlCommand(query, conn)
        Dim sqlAdap As New MySqlDataAdapter(cmd)
        Dim dtRecord As New DataTable
        sqlAdap.Fill(dtRecord)
        CmbBxFiltroPrueba.DataSource = dtRecord
        CmbBxFiltroPrueba.DisplayMember = "Nombre"
        CmbBxFiltroPrueba.ValueMember = "PrueNo"
    End Sub

    Private Sub BtnAgregar_Click(sender As Object, e As EventArgs) Handles ButtonAgregar.Click
        Dim Tabla_Actual As String = LabelAdmin.Text.ToString()
        If Tabla_Actual = "Analistas" Then
            Dim Filas_total As Integer = DGVAdmin.RowCount
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
            Dim Filas_total As Integer = DGVAdmin.RowCount
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

            Try
                conn.Open()
                Dim cmd As New MySqlCommand(String.Format("INSERT INTO rel_prue_analistas VALUES ('" & Nombre & "', '" & Prueba & "');"), conn)
                cmd.ExecuteNonQuery()
                MsgBox("Registro agregado satisfactoriamente", False, "Registro agregado")
            Catch ex As MySqlException
                MsgBox(ex.Message, False, "Error")
                conn.Close()
            End Try

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

            Dim t_creacion As String

            Try
                conn.Open()
                Dim cmd As New MySqlCommand(String.Format("SELECT NOW();"), conn)
                Dim fecha_servidor As DateTime = cmd.ExecuteScalar()
                t_creacion = fecha_servidor.ToString("yyyy-MM-dd HH:mm:ss")
                conn.Close()
            Catch ex As Exception
                msgbox(ex.message, False, "No se puede obtener la fecha de la base de datos")
                conn.Close()
                Exit Sub
            End Try

            If llave = Nothing Then
                llave = "1"
            End If

            Try
                conn.Open()
                Dim cmd As New MySqlCommand(String.Format("INSERT INTO rev_muestras (`Muestra_ID`, `Muestra_No`, `PrueNo`, `Valor_In`, `Tiempo_C`, `Estado`) VALUES ('" & llave & "', '" & Muestra_No & "', '" & prueba & "', '" & Valor_In & "', '" & t_creacion & "', 'Pendiente');"), conn)
                cmd.ExecuteNonQuery()
                MsgBox("Muestra Creada Satisfactoriamente", False, "Muestra Creada")
            Catch ex As MySqlException
                MsgBox(ex.Message, False, "Error")
                conn.Close()
                Exit Sub
            End Try
            conn.Close()
            Cargar_MuestrasBandejas_Admin()

        ElseIf Tabla_Actual = "Bandejas no Asignadas" Then

            Dim llave As String
            Dim Bandeja_No As String = TextBox1.Text.ToString
            Dim Comentario_admin As String = TextBox2.Text.ToString
            Dim prueba As Integer = ComboBox4.SelectedValue

            Try
                conn.Open()
                Dim cmd As New MySqlCommand(String.Format("SELECT MAX(Bandeja_ID)+1 FROM rev_bandejas;"), conn)
                llave = Convert.ToString(cmd.ExecuteScalar())
                conn.Close()
            Catch ex As Exception
                MsgBox(ex.Message, False, "Error")
                conn.Close()
                Exit Sub
            End Try

            Dim t_creacion As String

            Try
                conn.Open()
                Dim cmd As New MySqlCommand(String.Format("SELECT NOW();"), conn)
                Dim fecha_servidor As DateTime = cmd.ExecuteScalar()
                t_creacion = fecha_servidor.ToString("yyyy-MM-dd HH:mm:ss")
                conn.Close()
            Catch ex As Exception
                MsgBox(ex.Message, False, "No se puede obtener la fecha de la base de datos")
                conn.Close()
                Exit Sub
            End Try

            If llave = Nothing Then
                llave = "1"
            End If

            Try
                conn.Open()
                Dim cmd As New MySqlCommand(String.Format("INSERT INTO rev_bandejas (`Bandeja_ID`, `Bandeja_No`, `PrueNo`, `Comentario`, `Tiempo_C`, `Estado`) VALUES ('" & llave & "', '" & Bandeja_No & "', '" & prueba & "', '" & Comentario_admin & "', '" & t_creacion & "', 'Pendiente');"), conn)
                cmd.ExecuteNonQuery()
                MsgBox("Bandeja Creada Satisfactoriamente", False, "Bandeja Creada")
            Catch ex As MySqlException
                MsgBox(ex.Message, False, "Error")
                conn.Close()
                Exit Sub
            End Try
            conn.Close()
            Cargar_MuestrasBandejas_Admin()
        End If
        conn.Close()
        Cargar()
    End Sub

    Private Sub BtnModificar_Click(sender As Object, e As EventArgs) Handles BtnModificar.Click
        Dim reader As MySqlDataReader
        Dim Fila_actual_obj As Object = (DGVAdmin.CurrentRow)
        If IsNothing(Fila_actual_obj) Then
            MsgBox("Seleccione una fila a modificar")
            Exit Sub
        End If

        Dim Tabla_Actual As String = LabelAdmin.Text
        If Tabla_Actual = "Analistas" Then
            Dim Fila_actual As Integer = (DGVAdmin.CurrentRow.Index)
            Dim llave As Integer = DGVAdmin(0, (Fila_actual)).Value
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
            Dim Fila_actual As Integer = (DGVAdmin.CurrentRow.Index)
            Dim llave As Integer = DGVAdmin(0, (Fila_actual)).Value
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
        Cargar()
    End Sub

    Private Sub BtnEliminarRegistro_Click(sender As Object, e As EventArgs) Handles BtnEliminarRegistro.Click
        If MessageBox.Show("Desea eliminar el registro seleccionado ?", "Eliminar", MessageBoxButtons.YesNo) = DialogResult.Yes Then
            Dim reader As MySqlDataReader
            Dim Fila_actual_obj As Object = (DGVAdmin.CurrentRow)
            If IsNothing(Fila_actual_obj) Then
                MsgBox("Seleccione una fila a eliminar")
                Exit Sub
            End If

            Dim Tabla_Actual As String = LabelAdmin.Text
            If Tabla_Actual = "Analistas" Then
                Dim Fila_actual As Integer = (DGVAdmin.CurrentRow.Index)
                Dim llave As Integer = DGVAdmin(0, (Fila_actual)).Value
                DGVAdmin_SelectedIndexChanged(sender, e)
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
                Dim Fila_actual As Integer = (DGVAdmin.CurrentRow.Index)
                Dim llave As Integer = DGVAdmin(0, (Fila_actual)).Value
                DGVAdmin_SelectedIndexChanged(sender, e)
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

                DGVAdmin_SelectedIndexChanged(sender, e)

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
            Cargar()
        End If
    End Sub

    Private Sub DGVAdmin_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DGVAdmin.SelectionChanged
        Try
            Dim fila_actual As Integer = (DGVAdmin.CurrentRow.Index)

            Dim nom_tabla As String = LabelAdmin.Text


            If nom_tabla = "Analistas" Or nom_tabla = "Pruebas" Then
                TextBox3.Text = DGVAdmin(1, (fila_actual)).Value
                TextBox6.Text = DGVAdmin(2, (fila_actual)).Value
            ElseIf nom_tabla = "Relacion Analistas Pruebas" Then
                ComboBox2.Text = DGVAdmin(0, (fila_actual)).Value
                ComboBox3.Text = DGVAdmin(1, (fila_actual)).Value
                ComboBox5.Text = ComboBox2.Text
                ComboBox4.Text = ComboBox3.Text
            ElseIf nom_tabla = "Muestras Asignadas" Or nom_tabla = "Bandejas Asignadas" Then
                If DGVAdmin(5, fila_actual).Value.ToString() = "Revisado" Then
                    BtnSi.Enabled = True
                    BtnNo.Enabled = True
                    TextBox3.Text = DGVAdmin(6, fila_actual).Value.ToString()
                    TextBox6.Text = DGVAdmin(0, fila_actual).Value.ToString()
                    TextBox3.Visible = False
                    TextBox6.Visible = False
                Else
                    BtnSi.Enabled = False
                    BtnNo.Enabled = False
                    TextBox6.Text = DGVAdmin(0, fila_actual).Value.ToString()
                    TextBox6.Visible = False
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub

    Dim password As String

    Private Sub BtnConectar_Click(sender As Object, e As EventArgs) Handles BtnConectar.Click
        DGVMuestras.Visible = True
        DGVBandejas.Visible = True
        Dim usuario As Integer = CmbBxAnalistas.SelectedValue
        Dim usuario_string As String = CmbBxAnalistas.Text
        TextBoxContraseña.Text = ""
        TextBoxRespuestaForm2.Text = ""
        If usuario = 0 Then
            MsgBox("Seleccione un analista", False, "Error")
            Exit Sub
        End If
        FormContraseña.ShowDialog()
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
                Cargar_muestras(usuario)
                Cargar_bandejas(usuario)
                conectado = 1
                LabelCambiarContraseña.Visible = True
                CmbBxAnalistas.Enabled = False
                BtnConectar.Enabled = False
                BtnConectarAdmin.Enabled = False
                TxtBxFiltroRegistro.Visible = True
                LabelTituloFiltroRegistro.Visible = True
                DGVMuestras.Rows(0).Selected = True
                DGVBandejas.Rows(0).Selected = True
            Else
                MsgBox("Constraseña Incorrecta")
                Exit Sub
            End If
        Else
            Exit Sub
        End If

    End Sub

    Private Sub Cargar_muestras(usuario)
        CmbBxFiltroPrueba.Visible = True
        BtnFiltroPrueba.Visible = True
        GroupBoxMuestras.Visible = True
        BtnAsignarMuestras.Enabled = True
        BtnAsignarMuestras.Visible = True
        Dim filtro As String = TxtBxFiltroRegistro.Text 'filtro numero de registro
        Dim filtro2 As String = CmbBxFiltroPrueba.Text 'filtro prueba


        Dim reader As MySqlDataReader
        If filtro = "" And filtro2 = "" Then
            Try
                conn.Open()
                Dim query As String = "Select Muestra_ID, Muestra_No, pruebas.Nombre as Prueba, Valor_In, Valor_C1, Estado, Valor_C2, Pasa, analistas.Nombre as Revisa, Tiempo_C, Tiempo_A, Tiempo_V, Tiempo_A2, Tiempo_V2, Tiempo_T
                                    from(
                                    Select Muestra_ID, Muestra_No, Prueba, Valor_In, Valor_C1, Estado, Valor_C2, Pasa, Revisa, Tiempo_C, Tiempo_A, Tiempo_V, Tiempo_A2, Tiempo_V2, Tiempo_T
                                    from(
                                    SELECT Muestra_ID, Muestra_No, pruebas.PrueNo as Prueba, Valor_In, Valor_C1, Estado, Valor_C2, Pasa, analistas.AnalistNo as Revisa, rev_muestras.Tiempo_C, Tiempo_A, Tiempo_V, Tiempo_A2, Tiempo_V2, Tiempo_T
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
                DGVMuestras.DataSource = table
                DGVMuestras.ReadOnly = True
                DGVMuestras.SelectionMode = DataGridViewSelectionMode.FullRowSelect
                DGVMuestras.Columns(0).Visible = False
                DGVMuestras.Columns(9).Visible = False
                DGVMuestras.Columns(10).Visible = False
                DGVMuestras.Columns(11).Visible = False
                DGVMuestras.Columns(12).Visible = False
                DGVMuestras.Columns(13).Visible = False
                DGVMuestras.Columns(14).Visible = False

                reader.Close()
                conn.Close()
            Catch ex As MySqlException
                MsgBox(ex.Message)
                conn.Close()
            End Try
            If DGVMuestras(0, 0).Value Is Nothing Then
                MsgBox("Usted no cuenta con revisiones de muestras pendientes o posibles", False, "Info. Muestras")
            End If
        ElseIf filtro2 = "" And Not (filtro = "") Then
            Try
                conn.Open()
                Dim query As String = "Select Muestra_ID, Muestra_No, pruebas.Nombre as Prueba, Valor_In, Valor_C1, Estado, Valor_C2, Pasa, analistas.Nombre as Revisa, Tiempo_C, Tiempo_A, Tiempo_V, Tiempo_A2, Tiempo_V2, Tiempo_T
                                    from(
                                    Select Muestra_ID, Muestra_No, Prueba, Valor_In, Valor_C1, Estado, Valor_C2, Pasa, Revisa, Tiempo_C, Tiempo_A, Tiempo_V, Tiempo_A2, Tiempo_V2, Tiempo_T
                                    from(
                                    SELECT Muestra_ID, Muestra_No, pruebas.PrueNo as Prueba, Valor_In, Valor_C1, Estado, Valor_C2, Pasa, analistas.AnalistNo as Revisa, rev_muestras.Tiempo_C, Tiempo_A, Tiempo_V, Tiempo_A2, Tiempo_V2, Tiempo_T
                                    FROM pruebas inner join rev_muestras on pruebas.PrueNo = rev_muestras.PrueNo left join analistas on rev_muestras.AnalistNo = analistas.AnalistNo
                                    where (rev_muestras.AnalistNo = " & usuario & " or rev_muestras.AnalistNo is Null) and Estado in ('Revisado','Pendiente')
                                    )a 
                                    inner join 
                                    (Select * from rel_prue_analistas
                                    where AnalistNo = " & usuario & ")b
                                    on a.Prueba = b.PrueNo)c
                                    inner join pruebas on pruebas.PrueNo = c.Prueba left join analistas on analistas.AnalistNo = c.Revisa
                                    where Muestra_No like '" & filtro & "%';"
                Dim cmd As New MySqlCommand(query, conn)
                Console.WriteLine("Cargando Muestras del analista")

                reader = cmd.ExecuteReader()

                Dim table As New DataTable
                table.Load(reader)
                DGVMuestras.DataSource = table
                DGVMuestras.ReadOnly = True
                DGVMuestras.SelectionMode = DataGridViewSelectionMode.FullRowSelect
                DGVMuestras.Columns(0).Visible = False
                DGVMuestras.Columns(9).Visible = False
                DGVMuestras.Columns(10).Visible = False
                DGVMuestras.Columns(11).Visible = False
                DGVMuestras.Columns(12).Visible = False
                DGVMuestras.Columns(13).Visible = False
                DGVMuestras.Columns(14).Visible = False

                reader.Close()
                conn.Close()
            Catch ex As MySqlException
                MsgBox(ex.Message)
                conn.Close()
            End Try
        ElseIf filtro = "" And Not (filtro2 = "") Then
            Try
                conn.Open()
                Dim query As String = "Select Muestra_ID, Muestra_No, pruebas.Nombre as Prueba, Valor_In, Valor_C1, Estado, Valor_C2, Pasa, analistas.Nombre as Revisa, Tiempo_C, Tiempo_A, Tiempo_V, Tiempo_A2, Tiempo_V2, Tiempo_T
                                    from(
                                    Select Muestra_ID, Muestra_No, Prueba, Valor_In, Valor_C1, Estado, Valor_C2, Pasa, Revisa, Tiempo_C, Tiempo_A, Tiempo_V, Tiempo_A2, Tiempo_V2, Tiempo_T
                                    from(
                                    SELECT Muestra_ID, Muestra_No, pruebas.PrueNo as Prueba, Valor_In, Valor_C1, Estado, Valor_C2, Pasa, analistas.AnalistNo as Revisa, rev_muestras.Tiempo_C, Tiempo_A, Tiempo_V, Tiempo_A2, Tiempo_V2, Tiempo_T
                                    FROM pruebas inner join rev_muestras on pruebas.PrueNo = rev_muestras.PrueNo left join analistas on rev_muestras.AnalistNo = analistas.AnalistNo
                                    where (rev_muestras.AnalistNo = " & usuario & " or rev_muestras.AnalistNo is Null) and Estado in ('Revisado','Pendiente')
                                    )a 
                                    inner join 
                                    (Select * from rel_prue_analistas
                                    where AnalistNo = " & usuario & ")b
                                    on a.Prueba = b.PrueNo)c
                                    inner join pruebas on pruebas.PrueNo = c.Prueba left join analistas on analistas.AnalistNo = c.Revisa
                                    where pruebas.Nombre = '" & filtro2 & "';"
                Dim cmd As New MySqlCommand(query, conn)
                Console.WriteLine("Cargando Muestras del analista")

                reader = cmd.ExecuteReader()

                Dim table As New DataTable
                table.Load(reader)
                DGVMuestras.DataSource = table
                DGVMuestras.ReadOnly = True
                DGVMuestras.SelectionMode = DataGridViewSelectionMode.FullRowSelect
                DGVMuestras.Columns(0).Visible = False
                DGVMuestras.Columns(9).Visible = False
                DGVMuestras.Columns(10).Visible = False
                DGVMuestras.Columns(11).Visible = False
                DGVMuestras.Columns(12).Visible = False
                DGVMuestras.Columns(13).Visible = False
                DGVMuestras.Columns(14).Visible = False

                reader.Close()
                conn.Close()
            Catch ex As MySqlException
                MsgBox(ex.Message)
                conn.Close()
            End Try
        ElseIf (Not filtro = "") And (Not filtro2 = "") Then
            Try
                conn.Open()
                Dim query As String = "Select Muestra_ID, Muestra_No, pruebas.Nombre as Prueba, Valor_In, Valor_C1, Estado, Valor_C2, Pasa, analistas.Nombre as Revisa, Tiempo_C, Tiempo_A, Tiempo_V, Tiempo_A2, Tiempo_V2, Tiempo_T
                                    from(
                                    Select Muestra_ID, Muestra_No, Prueba, Valor_In, Valor_C1, Estado, Valor_C2, Pasa, Revisa, Tiempo_C, Tiempo_A, Tiempo_V, Tiempo_A2, Tiempo_V2, Tiempo_T
                                    from(
                                    SELECT Muestra_ID, Muestra_No, pruebas.PrueNo as Prueba, Valor_In, Valor_C1, Estado, Valor_C2, Pasa, analistas.AnalistNo as Revisa, rev_muestras.Tiempo_C, Tiempo_A, Tiempo_V, Tiempo_A2, Tiempo_V2, Tiempo_T
                                    FROM pruebas inner join rev_muestras on pruebas.PrueNo = rev_muestras.PrueNo left join analistas on rev_muestras.AnalistNo = analistas.AnalistNo
                                    where (rev_muestras.AnalistNo = " & usuario & " or rev_muestras.AnalistNo is Null) and Estado in ('Revisado','Pendiente')
                                    )a 
                                    inner join 
                                    (Select * from rel_prue_analistas
                                    where AnalistNo = " & usuario & ")b
                                    on a.Prueba = b.PrueNo)c
                                    inner join pruebas on pruebas.PrueNo = c.Prueba left join analistas on analistas.AnalistNo = c.Revisa
                                    where Muestra_no like '" & filtro & "%' and pruebas.Nombre = '" & filtro2 & "';"
                Dim cmd As New MySqlCommand(query, conn)
                Console.WriteLine("Cargando Muestras del analista")

                reader = cmd.ExecuteReader()

                Dim table As New DataTable
                table.Load(reader)
                DGVMuestras.DataSource = table
                DGVMuestras.ReadOnly = True
                DGVMuestras.SelectionMode = DataGridViewSelectionMode.FullRowSelect
                DGVMuestras.Columns(0).Visible = False
                DGVMuestras.Columns(9).Visible = False
                DGVMuestras.Columns(10).Visible = False
                DGVMuestras.Columns(11).Visible = False
                DGVMuestras.Columns(12).Visible = False
                DGVMuestras.Columns(13).Visible = False
                DGVMuestras.Columns(14).Visible = False

                reader.Close()
                conn.Close()
            Catch ex As MySqlException
                MsgBox(ex.Message)
                conn.Close()
            End Try
        End If
    End Sub

    Private Sub Cargar_bandejas(usuario)
        CmbBxFiltroPrueba.Visible = True
        BtnFiltroPrueba.Visible = True
        GroupBoxBandejas.Visible = True
        BtnAsignarBandejas.Enabled = True
        BtnAsignarBandejas.Visible = True
        Dim filtro As String = TxtBxFiltroRegistro.Text
        Dim filtro2 As String = CmbBxFiltroPrueba.Text

        Dim reader As MySqlDataReader
        If filtro = "" And filtro2 = "" Then
            Try
                conn.Open()
                Dim query As String = "Select Bandeja_ID, Bandeja_No, pruebas.Nombre as Prueba, Comentario, Comentario_Revision, Estado, Comentario_Revision_2, Pasa, analistas.Nombre as Revisa, Tiempo_C, Tiempo_A, Tiempo_V, Tiempo_A2, Tiempo_V2, Tiempo_T
                                    from(
                                    Select Bandeja_ID, Bandeja_No, Prueba, Comentario, Comentario_Revision, Estado, Comentario_Revision_2, Pasa, Revisa , Tiempo_C, Tiempo_A, Tiempo_V, Tiempo_A2, Tiempo_V2, Tiempo_T
                                    from(
                                    SELECT Bandeja_ID, Bandeja_No, pruebas.PrueNo as Prueba, Comentario, Comentario_Revision, Estado, Comentario_Revision_2, Pasa, analistas.AnalistNo as Revisa, Tiempo_C, Tiempo_A, Tiempo_V, Tiempo_A2, Tiempo_V2, Tiempo_T
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
                DGVBandejas.DataSource = table
                DGVBandejas.ReadOnly = True
                DGVBandejas.SelectionMode = DataGridViewSelectionMode.FullRowSelect
                DGVBandejas.Columns(0).Visible = False
                DGVBandejas.Columns(9).Visible = False
                DGVBandejas.Columns(10).Visible = False
                DGVBandejas.Columns(11).Visible = False
                DGVBandejas.Columns(12).Visible = False
                DGVBandejas.Columns(13).Visible = False
                DGVBandejas.Columns(14).Visible = False

                reader.Close()
                conn.Close()
            Catch ex As MySqlException
                MsgBox(ex.Message)
                conn.Close()
            End Try
            If DGVBandejas(0, 0).Value Is Nothing Then
                MsgBox("Usted no cuenta con revisiones de bandejas pendientes o posibles", False, "Info. Bandejas")
            End If
        ElseIf filtro2 = "" And Not (filtro = "") Then
            Try
                conn.Open()
                Dim query As String = "Select Bandeja_ID, Bandeja_No, pruebas.Nombre as Prueba, Comentario, Comentario_Revision, Estado, Comentario_Revision_2, Pasa, analistas.Nombre as Revisa, Tiempo_C, Tiempo_A, Tiempo_V, Tiempo_A2, Tiempo_V2, Tiempo_T
                                    from(
                                    Select Bandeja_ID, Bandeja_No, Prueba, Comentario, Comentario_Revision, Estado, Comentario_Revision_2, Pasa, Revisa , Tiempo_C, Tiempo_A, Tiempo_V, Tiempo_A2, Tiempo_V2, Tiempo_T
                                    from(
                                    SELECT Bandeja_ID, Bandeja_No, pruebas.PrueNo as Prueba, Comentario, Comentario_Revision, Estado, Comentario_Revision_2, Pasa, analistas.AnalistNo as Revisa, Tiempo_C, Tiempo_A, Tiempo_V, Tiempo_A2, Tiempo_V2, Tiempo_T
                                    FROM pruebas inner join rev_bandejas on pruebas.PrueNo = rev_bandejas.PrueNo left join analistas on rev_bandejas.AnalistNo = analistas.AnalistNo
                                    where (rev_bandejas.AnalistNo = " & usuario & " or rev_bandejas.AnalistNo is Null) and Estado in ('Revisado','Pendiente')
                                    )a 
                                    inner join 
                                    (Select * from rel_prue_analistas
                                    where AnalistNo = " & usuario & ")b
                                    on a.Prueba = b.PrueNo)c
                                    inner join pruebas on pruebas.PrueNo = c.Prueba left join analistas on analistas.AnalistNo = c.Revisa
                                    where Bandeja_No like '" & filtro & "%';;"
                Dim cmd As New MySqlCommand(query, conn)
                Console.WriteLine("Cargando Bandejas del analista")

                reader = cmd.ExecuteReader()

                Dim table As New DataTable
                table.Load(reader)
                DGVBandejas.DataSource = table
                DGVBandejas.ReadOnly = True
                DGVBandejas.SelectionMode = DataGridViewSelectionMode.FullRowSelect
                DGVBandejas.Columns(0).Visible = False
                DGVBandejas.Columns(9).Visible = False
                DGVBandejas.Columns(10).Visible = False
                DGVBandejas.Columns(11).Visible = False
                DGVBandejas.Columns(12).Visible = False
                DGVBandejas.Columns(13).Visible = False
                DGVBandejas.Columns(14).Visible = False

                reader.Close()
                conn.Close()
            Catch ex As MySqlException
                MsgBox(ex.Message)
                conn.Close()
            End Try
        ElseIf filtro = "" And Not (filtro2 = "") Then
            Try
                conn.Open()
                Dim query As String = "Select Bandeja_ID, Bandeja_No, pruebas.Nombre as Prueba, Comentario, Comentario_Revision, Estado, Comentario_Revision_2, Pasa, analistas.Nombre as Revisa, Tiempo_C, Tiempo_A, Tiempo_V, Tiempo_A2, Tiempo_V2, Tiempo_T
                                    from(
                                    Select Bandeja_ID, Bandeja_No, Prueba, Comentario, Comentario_Revision, Estado, Comentario_Revision_2, Pasa, Revisa , Tiempo_C, Tiempo_A, Tiempo_V, Tiempo_A2, Tiempo_V2, Tiempo_T
                                    from(
                                    SELECT Bandeja_ID, Bandeja_No, pruebas.PrueNo as Prueba, Comentario, Comentario_Revision, Estado, Comentario_Revision_2, Pasa, analistas.AnalistNo as Revisa, Tiempo_C, Tiempo_A, Tiempo_V, Tiempo_A2, Tiempo_V2, Tiempo_T
                                    FROM pruebas inner join rev_bandejas on pruebas.PrueNo = rev_bandejas.PrueNo left join analistas on rev_bandejas.AnalistNo = analistas.AnalistNo
                                    where (rev_bandejas.AnalistNo = " & usuario & " or rev_bandejas.AnalistNo is Null) and Estado in ('Revisado','Pendiente')
                                    )a 
                                    inner join 
                                    (Select * from rel_prue_analistas
                                    where AnalistNo = " & usuario & ")b
                                    on a.Prueba = b.PrueNo)c
                                    inner join pruebas on pruebas.PrueNo = c.Prueba left join analistas on analistas.AnalistNo = c.Revisa
                                    where pruebas.Nombre = '" & filtro2 & "';;"
                Dim cmd As New MySqlCommand(query, conn)
                Console.WriteLine("Cargando Bandejas del analista")

                reader = cmd.ExecuteReader()

                Dim table As New DataTable
                table.Load(reader)
                DGVBandejas.DataSource = table
                DGVBandejas.ReadOnly = True
                DGVBandejas.SelectionMode = DataGridViewSelectionMode.FullRowSelect
                DGVBandejas.Columns(0).Visible = False
                DGVBandejas.Columns(9).Visible = False
                DGVBandejas.Columns(10).Visible = False
                DGVBandejas.Columns(11).Visible = False
                DGVBandejas.Columns(12).Visible = False
                DGVBandejas.Columns(13).Visible = False
                DGVBandejas.Columns(14).Visible = False

                reader.Close()
                conn.Close()
            Catch ex As Exception
                MsgBox(ex.Message)
                conn.Close()
            End Try
        ElseIf (Not filtro = "") And (Not filtro2 = "") Then
            Try
                conn.Open()
                Dim query As String = "Select Bandeja_ID, Bandeja_No, pruebas.Nombre as Prueba, Comentario, Comentario_Revision, Estado, Comentario_Revision_2, Pasa, analistas.Nombre as Revisa, Tiempo_C, Tiempo_A, Tiempo_V, Tiempo_A2, Tiempo_V2, Tiempo_T
                                    from(
                                    Select Bandeja_ID, Bandeja_No, Prueba, Comentario, Comentario_Revision, Estado, Comentario_Revision_2, Pasa, Revisa , Tiempo_C, Tiempo_A, Tiempo_V, Tiempo_A2, Tiempo_V2, Tiempo_T
                                    from(
                                    SELECT Bandeja_ID, Bandeja_No, pruebas.PrueNo as Prueba, Comentario, Comentario_Revision, Estado, Comentario_Revision_2, Pasa, analistas.AnalistNo as Revisa, Tiempo_C, Tiempo_A, Tiempo_V, Tiempo_A2, Tiempo_V2, Tiempo_T
                                    FROM pruebas inner join rev_bandejas on pruebas.PrueNo = rev_bandejas.PrueNo left join analistas on rev_bandejas.AnalistNo = analistas.AnalistNo
                                    where (rev_bandejas.AnalistNo = " & usuario & " or rev_bandejas.AnalistNo is Null) and Estado in ('Revisado','Pendiente')
                                    )a 
                                    inner join 
                                    (Select * from rel_prue_analistas
                                    where AnalistNo = " & usuario & ")b
                                    on a.Prueba = b.PrueNo)c
                                    inner join pruebas on pruebas.PrueNo = c.Prueba left join analistas on analistas.AnalistNo = c.Revisa
                                    where Bandeja_No like '" & filtro & "%' and pruebas.Nombre = '" & filtro2 & "';"
                Dim cmd As New MySqlCommand(query, conn)
                Console.WriteLine("Cargando Bandejas del analista")

                reader = cmd.ExecuteReader()

                Dim table As New DataTable
                table.Load(reader)
                DGVBandejas.DataSource = table
                DGVBandejas.ReadOnly = True
                DGVBandejas.SelectionMode = DataGridViewSelectionMode.FullRowSelect
                DGVBandejas.Columns(0).Visible = False
                DGVBandejas.Columns(9).Visible = False
                DGVBandejas.Columns(10).Visible = False
                DGVBandejas.Columns(11).Visible = False
                DGVBandejas.Columns(12).Visible = False
                DGVBandejas.Columns(13).Visible = False
                DGVBandejas.Columns(14).Visible = False

                reader.Close()
                conn.Close()
            Catch ex As Exception
                MsgBox(ex.Message)
                conn.Close()
            End Try
        End If

    End Sub

    Dim flag As Byte

    Private Sub DGVMuestras_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DGVMuestras.SelectionChanged
        LabelTimerAsignacion.Text = ""
        LabelTimerVerificacion.Text = ""
        LabelTimerTotal.Text = ""
        Try
            Dim fila_actual As Integer = (DGVMuestras.CurrentRow.Index)
            If IsDBNull(DGVMuestras(0, fila_actual).Value) Then
                BtnAsignarMuestras.Enabled = False
                CmbBxAnalistas.Text = ""
                Exit Sub
            End If
            If fila_actual = (DGVMuestras.RowCount - 1) Then
                flag = 0
                LabelTimerAsignacion.Visible = False
                LabelTimerVerificacion.Visible = False
                LabelTimerTotal.Visible = False
                BtnAsignarMuestras.Visible = False
            Else
                flag = 1
                LabelTimerAsignacion.Visible = True
                LabelTimerVerificacion.Visible = True
                LabelTimerTotal.Visible = True
                BtnAsignarMuestras.Visible = True
            End If
            If IsDBNull(DGVMuestras(8, fila_actual).Value) Then
                TxBxValorC1Muestra.Visible = False
                TxBxValorC2Muestra.Visible = False
                LabelValorC1Muestra.Visible = False
                LabelValorC2Muestra.Visible = False
                BtnAsignarMuestras.Visible = True
                BtnAsignarMuestras.Enabled = True
            Else
                LabelValorC1Muestra.Visible = True
                TxBxValorC1Muestra.Visible = True
                TxBxValorC1Muestra.Text = ""
                'LabelValorC2Muestra.Visible = True
                'TxBxValorC2Muestra.Visible = True
                TxBxValorC2Muestra.Text = ""
                BtnAsignarMuestras.Enabled = False
                If DGVMuestras(5, fila_actual).Value = "Finalizado" Then
                    TxBxValorC1Muestra.Text = DGVMuestras(4, fila_actual).Value
                    TxBxValorC1Muestra.Visible = True
                    TxBxValorC1Muestra.Enabled = False
                    If IsDBNull(DGVMuestras(6, fila_actual).Value) Then
                        TxBxValorC2Muestra.Text = ""
                    Else
                        TxBxValorC2Muestra.Text = DGVMuestras(6, fila_actual).Value
                    End If
                    TxBxValorC2Muestra.Visible = True
                    TxBxValorC2Muestra.Enabled = False
                    LabelValorC1Muestra.Visible = True
                    LabelValorC2Muestra.Visible = True
                ElseIf DGVMuestras(5, fila_actual).Value = "Revisado" Or DGVMuestras(5, fila_actual).Value = "Pendiente" Then
                    If IsDBNull(DGVMuestras(4, fila_actual).Value) Then
                        BtnAsignarMuestras.Visible = True
                        BtnAsignarMuestras.Enabled = True
                    Else
                        TxBxValorC1Muestra.Text = DGVMuestras(4, fila_actual).Value
                        TxBxValorC1Muestra.Visible = True
                        TxBxValorC1Muestra.Enabled = False
                        'TxBxValorC2Muestra.Visible = True
                        LabelValorC1Muestra.Visible = True
                    End If
                    If IsDBNull(DGVMuestras(6, fila_actual).Value) Then
                        If TxBxValorC1Muestra.Text <> "" Then
                            TxBxValorC1Muestra.Text = DGVMuestras(4, fila_actual).Value
                            TxBxValorC1Muestra.Visible = True
                        End If
                    Else
                        TxBxValorC2Muestra.Text = DGVMuestras(6, fila_actual).Value
                        TxBxValorC2Muestra.Visible = True
                        TxBxValorC2Muestra.Enabled = False
                        'TxBxValorC2Muestra.Visible = True
                        LabelValorC2Muestra.Visible = True
                    End If
                    If IsDBNull(DGVMuestras(4, fila_actual).Value) And DGVMuestras(8, fila_actual).Value <> "" Then
                        TxBxValorC1Muestra.Enabled = True
                        TxBxValorC1Muestra.Visible = True
                        LabelValorC1Muestra.Visible = True
                        BtnAsignarMuestras.Visible = True
                        BtnAsignarMuestras.Enabled = True
                    End If
                    If IsDBNull(DGVMuestras(7, fila_actual).Value) Then

                    ElseIf IsDBNull(DGVMuestras(6, fila_actual).Value) And DGVMuestras(7, fila_actual).Value = "No" Then
                        TxBxValorC1Muestra.Visible = True
                        'TxBxValorC2Muestra.Visible = True
                        LabelValorC1Muestra.Visible = True
                        TxBxValorC2Muestra.Visible = True
                        TxBxValorC2Muestra.Enabled = True
                        'LabelValorC1Muestra.Visible = True
                        LabelValorC2Muestra.Visible = True
                        BtnAsignarMuestras.Enabled = True
                        BtnAsignarMuestras.Visible = True
                    End If

                End If

            End If
        Catch ex As Exception
        End Try

    End Sub

    Private Sub BtnAsignarMuestras_Click(sender As Object, e As EventArgs) Handles BtnAsignarMuestras.Click
        Dim t_asignacion As String
        Dim t_revision As String
        If IsDBNull(DGVMuestras(0, 0).Value) Then
            MsgBox("No existen Muestras para revisar actualmente")
            Exit Sub
        End If

        Try
            Dim fila = DGVMuestras.CurrentRow.Index
        Catch ex As Exception
            MsgBox("Seleccione una fila", False, "Error")
            Exit Sub
        End Try

        Dim fila_actual As Integer = (DGVMuestras.CurrentRow.Index)


        Dim analista As Integer = CmbBxAnalistas.SelectedValue
        If IsDBNull(DGVMuestras(8, fila_actual).Value) Then

            Try
                conn.Open()
                Dim cmd As New MySqlCommand(String.Format("SELECT NOW();"), conn)
                Dim fecha_DB As DateTime = cmd.ExecuteScalar()
                t_asignacion = fecha_DB.ToString("yyyy-MM-dd HH:mm:ss")
                conn.Close()
            Catch ex As Exception
                MsgBox(ex.Message, False, "No se puede obtener la fecha de la base de datos")
                conn.Close()
                Exit Sub
            End Try

            Dim reader As MySqlDataReader
            Try
                conn.Open()
                Dim query As String = "UPDATE rev_muestras SET Tiempo_A = '" & t_asignacion & "', AnalistNo= '" & analista & "' WHERE Muestra_ID='" & DGVMuestras(0, fila_actual).Value.ToString() & "';"
                Dim cmd As New MySqlCommand(query, conn)
                reader = cmd.ExecuteReader
                conn.Close()
            Catch ex As Exception
                MsgBox(ex.Message)
                conn.Close()
            End Try
            Cargar_muestras(analista)
        ElseIf IsDBNull(DGVMuestras(4, fila_actual).Value) And TxBxValorC1Muestra.Text <> "" Then
            Try
                conn.Open()
                Dim cmd As New MySqlCommand(String.Format("SELECT NOW();"), conn)
                Dim fecha_DB As DateTime = cmd.ExecuteScalar()
                t_revision = fecha_DB.ToString("yyyy-MM-dd HH:mm:ss")
                conn.Close()
            Catch ex As Exception
                MsgBox(ex.Message, False, "No se puede obtener la fecha de la base de datos")
                conn.Close()
                Exit Sub
            End Try
            Dim reader As MySqlDataReader
            Try
                conn.Open()
                Dim query = "UPDATE rev_muestras SET Valor_C1='" & TxBxValorC1Muestra.Text & "', `Tiempo_V`='" & t_revision & "', Estado='Revisado' WHERE Muestra_ID='" & DGVMuestras(0, fila_actual).Value.ToString() & "';"
                Dim cmd As New MySqlCommand(query, conn)
                reader = cmd.ExecuteReader
                conn.Close()
            Catch ex As Exception
                MsgBox(ex.Message)
                conn.Close()
            End Try
            Cargar_muestras(analista)
        ElseIf Not (IsDBNull(DGVMuestras(4, fila_actual).Value) And TxBxValorC2Muestra.Text <> "") Then
            Dim t_revision_2 As String

            Try
                conn.Open()
                Dim cmd As New MySqlCommand(String.Format("SELECT NOW();"), conn)
                Dim fecha_DB As DateTime = cmd.ExecuteScalar()
                t_revision_2 = fecha_DB.ToString("yyyy-MM-dd HH:mm:ss")
                conn.Close()
            Catch ex As Exception
                MsgBox(ex.Message, False, "No se puede obtener la fecha de la base de datos")
                conn.Close()
                Exit Sub
            End Try

            Dim reader As MySqlDataReader
            Try
                conn.Open()
                Dim query = "UPDATE rev_muestras SET Tiempo_V2 = '" & t_revision_2 & "', Estado ='Revisado', Valor_C2 ='" & TxBxValorC2Muestra.Text & "', Pasa = NULL WHERE Muestra_ID='" & DGVMuestras(0, fila_actual).Value.ToString() & "';"
                Dim cmd As New MySqlCommand(query, conn)
                reader = cmd.ExecuteReader
                conn.Close()
            Catch ex As Exception
                MsgBox(ex.Message)
                conn.Close()
            End Try
            Cargar_muestras(analista)
        End If
    End Sub

    Private Sub BtnAsignarBandejas_Click(sender As Object, e As EventArgs) Handles BtnAsignarBandejas.Click
        Dim t_asignacion As String
        Dim t_revision As String
        If IsDBNull(DGVBandejas(0, 0).Value) Then
            MsgBox("No existen Bandejas para revisar actualmente")
            Exit Sub
        End If

        Try
            Dim fila = DGVBandejas.CurrentRow.Index
        Catch ex As Exception
            MsgBox("Seleccione una fila", False, "Error")
            Exit Sub
        End Try

        Dim fila_actual As Integer = (DGVBandejas.CurrentRow.Index)
        Dim analista As Integer = CmbBxAnalistas.SelectedValue
        If IsDBNull(DGVBandejas(8, fila_actual).Value) Then

            Try
                conn.Open()
                Dim cmd As New MySqlCommand(String.Format("SELECT NOW();"), conn)
                Dim fecha_DB As DateTime = cmd.ExecuteScalar()
                t_asignacion = fecha_DB.ToString("yyyy-MM-dd HH:mm:ss")
                conn.Close()
            Catch ex As Exception
                MsgBox(ex.Message, False, "No se puede obtener la fecha de la base de datos")
                conn.Close()
                Exit Sub
            End Try

            Dim reader As MySqlDataReader

            Try
                conn.Open()
                Dim query As String = "UPDATE rev_bandejas SET Tiempo_A='" & t_asignacion & "', AnalistNo ='" & analista & "' WHERE Bandeja_ID = '" & DGVBandejas(0, fila_actual).Value.ToString() & "';"
                Dim cmd As New MySqlCommand(query, conn)
                reader = cmd.ExecuteReader
                conn.Close()
            Catch ex As Exception
                MsgBox(ex.Message)
                conn.Close()
            End Try
            Cargar_bandejas(analista)
        ElseIf IsDBNull(DGVBandejas(4, fila_actual).Value) And TxBxComentario1Bandeja.Text <> "" Then
            Try
                conn.Open()
                Dim cmd As New MySqlCommand(String.Format("SELECT NOW();"), conn)
                Dim fecha_DB As DateTime = cmd.ExecuteScalar()
                t_revision = fecha_DB.ToString("yyyy-MM-dd HH:mm:ss")
                conn.Close()
            Catch ex As Exception
                MsgBox(ex.Message, False, "No se puede obtener la fecha de la base de datos")
                conn.Close()
                Exit Sub
            End Try
            Dim reader As MySqlDataReader
            Try
                conn.Open()
                Dim query = "UPDATE rev_bandejas SET Comentario_Revision = '" & TxBxComentario1Bandeja.Text & "', Tiempo_V='" & t_revision & "', Estado ='Revisado' WHERE Bandeja_ID ='" & DGVBandejas(0, fila_actual).Value.ToString() & "';"
                Dim cmd As New MySqlCommand(query, conn)
                reader = cmd.ExecuteReader
                conn.Close()
            Catch ex As Exception
                MsgBox(ex.Message)
                conn.Close()
            End Try
            Cargar_bandejas(analista)
        ElseIf Not (IsDBNull(DGVMuestras(4, fila_actual).Value) And TxBxComentario2Bandeja.Text <> "") Then
            Dim t_revision_2 As String

            Try
                conn.Open()
                Dim cmd As New MySqlCommand(String.Format("SELECT NOW();"), conn)
                Dim fecha_DB As DateTime = cmd.ExecuteScalar()
                t_revision_2 = fecha_DB.ToString("yyyy-MM-dd HH:mm:ss")
                conn.Close()
            Catch ex As Exception
                MsgBox(ex.Message, False, "No se puede obtener la fecha de la base de datos")
                conn.Close()
                Exit Sub
            End Try

            Dim reader As MySqlDataReader
            Try
                conn.Open()
                Dim query = "UPDATE rev_bandejas SET Tiempo_V2 = '" & t_revision_2 & "', Estado = 'Revisado', Comentario_Revision_2 = '" & TxBxComentario2Bandeja.Text & "', Pasa = NULL WHERE Bandeja_ID = '" & DGVBandejas(0, fila_actual).Value.ToString() & "';"
                Dim cmd As New MySqlCommand(query, conn)
                reader = cmd.ExecuteReader
                conn.Close()
            Catch ex As Exception
                MsgBox(ex.Message)
                conn.Close()
            End Try
            Cargar_bandejas(analista)
        End If
    End Sub

    Private Sub BtnDesconectar_Click(sender As Object, e As EventArgs) Handles BtnDesconectar.Click
        If conectado = 1 Then
            CmbBxAnalistas.Enabled = True
            TxtBxFiltroRegistro.Text = Nothing
            TxtBxFiltroRegistro.Visible = False
            CmbBxFiltroPrueba.Text = Nothing
            CmbBxFiltroPrueba.Visible = False
            BtnFiltroPrueba.Visible = False
            LabelTituloFiltroRegistro.Visible = False
            BtnConectar.Enabled = True
            BtnConectarAdmin.Enabled = True
            TabControl1.TabPages.Remove(TabPageAdmin)
            CmbBxAnalistas.Text = " "
            MsgBox("Desconectado", False, "Log-Out")
            conectado = 0
            LabelCambiarContraseña.Visible = False
            DGVMuestras.Visible = False
            DGVBandejas.Visible = False
            DGVMuestras.DataSource = Nothing
            DGVMuestras.Rows.Clear()
            DGVBandejas.DataSource = Nothing
            DGVBandejas.Rows.Clear()
            GroupBoxMuestras.Visible = False
            GroupBoxBandejas.Visible = False
        Else
            MsgBox("No hay sesión activa", False, "Error")
        End If
    End Sub
    Dim conectado As Integer = 0
    Private Sub BtnConectarAdmin_Click(sender As Object, e As EventArgs) Handles BtnConectarAdmin.Click
        DGVMuestras.Visible = True
        DGVBandejas.Visible = True
        Dim usuario As Integer = 1
        TextBoxContraseña.Text = ""
        TextBoxRespuestaForm2.Text = ""
        If usuario = 0 Then
            MsgBox("Seleccione un analista", False, "Error")
            Exit Sub
        End If
        FormContraseña.ShowDialog()
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
                conectado = 1
                LabelCambiarContraseña.Visible = True
                TabControl1.TabPages.Insert(3, TabPageAdmin)
                Cargar_MuestrasBandejas_Admin()
                CmbBxAnalistas.Text = " "
                CmbBxAnalistas.Enabled = False
                BtnConectar.Enabled = False
                BtnConectarAdmin.Enabled = False
                LabelTituloFiltroRegistro.Visible = True
                TxtBxFiltroRegistro.Visible = True
                CmbBxFiltroPrueba.Visible = True
                BtnFiltroPrueba.Visible = True
            Else
                MsgBox("Constraseña Incorrecta", False, "Error")
                Exit Sub
            End If
        Else
            Exit Sub
        End If
    End Sub

    Private Sub Cargar_MuestrasBandejas_Admin()
        Dim reader As MySqlDataReader
        Dim query As String

        Dim filtro As String = TxtBxFiltroRegistro.Text
        Dim filtro2 As String = CmbBxFiltroPrueba.Text

        If filtro = "" And filtro2 = "" Then
            Try
                conn.Open()
                query = "SELECT Muestra_ID, rev_muestras.Muestra_No as 'Muestra',pruebas.Nombre as 'Prueba', rev_muestras.Valor_In, rev_muestras.Valor_C1, rev_muestras.Estado, rev_muestras.Valor_C2, rev_muestras.Pasa, analistas.Nombre as 'Revisa', Tiempo_C, Tiempo_A, Tiempo_V, Tiempo_A2, Tiempo_V2, Tiempo_T
                    FROM rev_muestras left join analistas on analistas.AnalistNo = rev_muestras.AnalistNo inner join pruebas on rev_muestras.PrueNo = pruebas.PrueNo;"
                'Where rev_muestras.Estado in ('Revisado','Pendiente');"
                Dim cmd As New MySqlCommand(query, conn)

                reader = cmd.ExecuteReader()

                Dim table As New DataTable
                table.Load(reader)
                DGVMuestras.DataSource = table
                DGVMuestras.ReadOnly = True
                DGVMuestras.SelectionMode = DataGridViewSelectionMode.FullRowSelect
                DGVMuestras.Columns(0).Visible = False
                DGVMuestras.Columns(9).Visible = False
                DGVMuestras.Columns(10).Visible = False
                DGVMuestras.Columns(11).Visible = False
                DGVMuestras.Columns(12).Visible = False
                DGVMuestras.Columns(13).Visible = False
                DGVMuestras.Columns(14).Visible = False

                reader.Close()
                conn.Close()
            Catch ex As MySqlException
                MsgBox(ex.Message)
                conn.Close()
            End Try

            Try
                conn.Open()
                query = "SELECT Bandeja_ID, rev_bandejas.Bandeja_No as 'Bandeja', pruebas.Nombre as 'Prueba', rev_bandejas.Comentario, rev_bandejas.Comentario_Revision, rev_bandejas.Estado, rev_bandejas.Comentario_Revision_2, rev_bandejas.Pasa, analistas.Nombre as 'Revisa' , Tiempo_C, Tiempo_A, Tiempo_V, Tiempo_A2, Tiempo_V2, Tiempo_T
                    FROM rev_bandejas left join analistas on analistas.AnalistNo = rev_bandejas.AnalistNo inner join pruebas on rev_bandejas.PrueNo = pruebas.PrueNo;"
                'Where rev_bandejas.Estado in ('Revisado','Pendiente');"
                Dim cmd As New MySqlCommand(query, conn)

                reader = cmd.ExecuteReader()

                Dim table As New DataTable
                table.Load(reader)
                DGVBandejas.DataSource = table
                DGVBandejas.ReadOnly = True
                DGVBandejas.SelectionMode = DataGridViewSelectionMode.FullRowSelect
                DGVBandejas.Columns(0).Visible = False
                DGVBandejas.Columns(9).Visible = False
                DGVBandejas.Columns(10).Visible = False
                DGVBandejas.Columns(11).Visible = False
                DGVBandejas.Columns(12).Visible = False
                DGVBandejas.Columns(13).Visible = False
                DGVBandejas.Columns(14).Visible = False

                reader.Close()
                conn.Close()
            Catch ex As MySqlException
                MsgBox(ex.Message)
                conn.Close()
            End Try

        ElseIf filtro2 = "" And Not (filtro = "") Then
            Try
                conn.Open()
                query = "SELECT Muestra_ID, rev_muestras.Muestra_No as 'Muestra',pruebas.Nombre as 'Prueba', rev_muestras.Valor_In, rev_muestras.Valor_C1, rev_muestras.Estado, rev_muestras.Valor_C2, rev_muestras.Pasa, analistas.Nombre as 'Revisa', Tiempo_C, Tiempo_A, Tiempo_V, Tiempo_A2, Tiempo_V2, Tiempo_T
                    FROM rev_muestras left join analistas on analistas.AnalistNo = rev_muestras.AnalistNo inner join pruebas on rev_muestras.PrueNo = pruebas.PrueNo
                    where Muestra_No like '" & filtro & "%';"
                Dim cmd As New MySqlCommand(query, conn)
                Console.WriteLine("Cargando Muestras del analista")

                reader = cmd.ExecuteReader()

                Dim table As New DataTable
                table.Load(reader)
                DGVMuestras.DataSource = table
                DGVMuestras.ReadOnly = True
                DGVMuestras.SelectionMode = DataGridViewSelectionMode.FullRowSelect
                DGVMuestras.Columns(0).Visible = False
                DGVMuestras.Columns(9).Visible = False
                DGVMuestras.Columns(10).Visible = False
                DGVMuestras.Columns(11).Visible = False
                DGVMuestras.Columns(12).Visible = False
                DGVMuestras.Columns(13).Visible = False
                DGVMuestras.Columns(14).Visible = False

                reader.Close()
                conn.Close()
            Catch ex As MySqlException
                MsgBox(ex.Message)
                conn.Close()
            End Try

            Try
                conn.Open()
                query = "SELECT Bandeja_ID, rev_bandejas.Bandeja_No as 'Bandeja', pruebas.Nombre as 'Prueba', rev_bandejas.Comentario, rev_bandejas.Comentario_Revision, rev_bandejas.Estado, rev_bandejas.Comentario_Revision_2, rev_bandejas.Pasa, analistas.Nombre as 'Revisa' , Tiempo_C, Tiempo_A, Tiempo_V, Tiempo_A2, Tiempo_V2, Tiempo_T
                    FROM rev_bandejas left join analistas on analistas.AnalistNo = rev_bandejas.AnalistNo inner join pruebas on rev_bandejas.PrueNo = pruebas.PrueNo
                    Where Bandeja_No like '" & filtro & "%';"
                'Where rev_bandejas.Estado in ('Revisado','Pendiente');"
                Dim cmd As New MySqlCommand(query, conn)

                reader = cmd.ExecuteReader()

                Dim table As New DataTable
                table.Load(reader)
                DGVBandejas.DataSource = table
                DGVBandejas.ReadOnly = True
                DGVBandejas.SelectionMode = DataGridViewSelectionMode.FullRowSelect
                DGVBandejas.Columns(0).Visible = False
                DGVBandejas.Columns(9).Visible = False
                DGVBandejas.Columns(10).Visible = False
                DGVBandejas.Columns(11).Visible = False
                DGVBandejas.Columns(12).Visible = False
                DGVBandejas.Columns(13).Visible = False
                DGVBandejas.Columns(14).Visible = False

                reader.Close()
                conn.Close()
            Catch ex As MySqlException
                MsgBox(ex.Message)
                conn.Close()
            End Try

        ElseIf filtro = "" And Not (filtro2 = "") Then
            Try
                conn.Open()
                query = "SELECT Muestra_ID, rev_muestras.Muestra_No as 'Muestra',pruebas.Nombre as 'Prueba', rev_muestras.Valor_In, rev_muestras.Valor_C1, rev_muestras.Estado, rev_muestras.Valor_C2, rev_muestras.Pasa, analistas.Nombre as 'Revisa', Tiempo_C, Tiempo_A, Tiempo_V, Tiempo_A2, Tiempo_V2, Tiempo_T
                    FROM rev_muestras left join analistas on analistas.AnalistNo = rev_muestras.AnalistNo inner join pruebas on rev_muestras.PrueNo = pruebas.PrueNo
                    where pruebas.Nombre = '" & filtro2 & "';"
                Dim cmd As New MySqlCommand(query, conn)
                Console.WriteLine("Cargando Muestras del analista")

                reader = cmd.ExecuteReader()

                Dim table As New DataTable
                table.Load(reader)
                DGVMuestras.DataSource = table
                DGVMuestras.ReadOnly = True
                DGVMuestras.SelectionMode = DataGridViewSelectionMode.FullRowSelect
                DGVMuestras.Columns(0).Visible = False
                DGVMuestras.Columns(9).Visible = False
                DGVMuestras.Columns(10).Visible = False
                DGVMuestras.Columns(11).Visible = False
                DGVMuestras.Columns(12).Visible = False
                DGVMuestras.Columns(13).Visible = False
                DGVMuestras.Columns(14).Visible = False

                reader.Close()
                conn.Close()
            Catch ex As MySqlException
                MsgBox(ex.Message)
                conn.Close()
            End Try

            Try
                conn.Open()
                query = "SELECT Bandeja_ID, rev_bandejas.Bandeja_No as 'Bandeja', pruebas.Nombre as 'Prueba', rev_bandejas.Comentario, rev_bandejas.Comentario_Revision, rev_bandejas.Estado, rev_bandejas.Comentario_Revision_2, rev_bandejas.Pasa, analistas.Nombre as 'Revisa' , Tiempo_C, Tiempo_A, Tiempo_V, Tiempo_A2, Tiempo_V2, Tiempo_T
                    FROM rev_bandejas left join analistas on analistas.AnalistNo = rev_bandejas.AnalistNo inner join pruebas on rev_bandejas.PrueNo = pruebas.PrueNo
                    Where pruebas.Nombre = '" & filtro2 & "';"
                'Where rev_bandejas.Estado in ('Revisado','Pendiente');"
                Dim cmd As New MySqlCommand(query, conn)

                reader = cmd.ExecuteReader()

                Dim table As New DataTable
                table.Load(reader)
                DGVBandejas.DataSource = table
                DGVBandejas.ReadOnly = True
                DGVBandejas.SelectionMode = DataGridViewSelectionMode.FullRowSelect
                DGVBandejas.Columns(0).Visible = False
                DGVBandejas.Columns(9).Visible = False
                DGVBandejas.Columns(10).Visible = False
                DGVBandejas.Columns(11).Visible = False
                DGVBandejas.Columns(12).Visible = False
                DGVBandejas.Columns(13).Visible = False
                DGVBandejas.Columns(14).Visible = False

                reader.Close()
                conn.Close()
            Catch ex As MySqlException
                MsgBox(ex.Message)
                conn.Close()
            End Try


        ElseIf (Not filtro = "") And (Not filtro2 = "") Then
            Try
                conn.Open()
                query = "SELECT Muestra_ID, rev_muestras.Muestra_No as 'Muestra',pruebas.Nombre as 'Prueba', rev_muestras.Valor_In, rev_muestras.Valor_C1, rev_muestras.Estado, rev_muestras.Valor_C2, rev_muestras.Pasa, analistas.Nombre as 'Revisa', Tiempo_C, Tiempo_A, Tiempo_V, Tiempo_A2, Tiempo_V2, Tiempo_T
                    FROM rev_muestras left join analistas on analistas.AnalistNo = rev_muestras.AnalistNo inner join pruebas on rev_muestras.PrueNo = pruebas.PrueNo
                    where Muestra_No like '" & filtro & "%' and pruebas.Nombre = '" & filtro2 & "';"
                Dim cmd As New MySqlCommand(query, conn)
                Console.WriteLine("Cargando Muestras del analista")

                reader = cmd.ExecuteReader()

                Dim table As New DataTable
                table.Load(reader)
                DGVMuestras.DataSource = table
                DGVMuestras.ReadOnly = True
                DGVMuestras.SelectionMode = DataGridViewSelectionMode.FullRowSelect
                DGVMuestras.Columns(0).Visible = False
                DGVMuestras.Columns(9).Visible = False
                DGVMuestras.Columns(10).Visible = False
                DGVMuestras.Columns(11).Visible = False
                DGVMuestras.Columns(12).Visible = False
                DGVMuestras.Columns(13).Visible = False
                DGVMuestras.Columns(14).Visible = False

                reader.Close()
                conn.Close()
            Catch ex As MySqlException
                MsgBox(ex.Message)
                conn.Close()
            End Try

            Try
                conn.Open()
                query = "SELECT Bandeja_ID, rev_bandejas.Bandeja_No as 'Bandeja', pruebas.Nombre as 'Prueba', rev_bandejas.Comentario, rev_bandejas.Comentario_Revision, rev_bandejas.Estado, rev_bandejas.Comentario_Revision_2, rev_bandejas.Pasa, analistas.Nombre as 'Revisa' , Tiempo_C, Tiempo_A, Tiempo_V, Tiempo_A2, Tiempo_V2, Tiempo_T
                    FROM rev_bandejas left join analistas on analistas.AnalistNo = rev_bandejas.AnalistNo inner join pruebas on rev_bandejas.PrueNo = pruebas.PrueNo
                    where Bandeja_No like '" & filtro & "%' and pruebas.Nombre = '" & filtro2 & "';"
                'Where rev_bandejas.Estado in ('Revisado','Pendiente');"
                Dim cmd As New MySqlCommand(query, conn)

                reader = cmd.ExecuteReader()

                Dim table As New DataTable
                table.Load(reader)
                DGVBandejas.DataSource = table
                DGVBandejas.ReadOnly = True
                DGVBandejas.SelectionMode = DataGridViewSelectionMode.FullRowSelect
                DGVBandejas.Columns(0).Visible = False
                DGVBandejas.Columns(9).Visible = False
                DGVBandejas.Columns(10).Visible = False
                DGVBandejas.Columns(11).Visible = False
                DGVBandejas.Columns(12).Visible = False
                DGVBandejas.Columns(13).Visible = False
                DGVBandejas.Columns(14).Visible = False

                reader.Close()
                conn.Close()
            Catch ex As MySqlException
                MsgBox(ex.Message)
                conn.Close()
            End Try

        End If


    End Sub

    Dim flag1 As Byte

    Private Sub DGVBandejas_SelectionChanged(sender As Object, e As EventArgs) Handles DGVBandejas.SelectionChanged
        TxBxComentario1Bandeja.Visible = False
        TxBxComentario2Bandeja.Visible = False
        LabelComentario1Bandeja.Visible = False
        LabelComentario2Bandeja.Visible = False
        LabelTimerAsignacion.Text = ""
        LabelTimerVerificacion.Text = ""
        LabelTimerTotal.Text = ""

        Try
            Dim fila_actual As Integer = (DGVBandejas.CurrentRow.Index)
            If IsDBNull(DGVBandejas(0, fila_actual).Value) Then
                BtnAsignarBandejas.Enabled = False
                Exit Sub
            End If
            If fila_actual = (DGVBandejas.RowCount - 1) Then
                flag1 = 0
                BtnAsignarBandejas.Visible = False
                LabelTimerAsignacion.Visible = False
                LabelTimerVerificacion.Visible = False
                LabelTimerTotal.Visible = False
            Else
                flag1 = 1
                BtnAsignarBandejas.Visible = True
                LabelTimerAsignacion.Visible = True
                LabelTimerVerificacion.Visible = True
                LabelTimerTotal.Visible = True
            End If
            If IsDBNull(DGVBandejas(8, fila_actual).Value) Then
                TxBxComentario1Bandeja.Visible = False
                TxBxComentario2Bandeja.Visible = False
                LabelComentario1Bandeja.Visible = False
                LabelComentario2Bandeja.Visible = False
                BtnAsignarBandejas.Visible = True
                BtnAsignarBandejas.Enabled = True
            Else
                LabelComentario1Bandeja.Visible = True
                TxBxComentario1Bandeja.Visible = True
                TxBxComentario1Bandeja.Text = ""
                'LabelComentario2Bandeja.Visible = True
                'TxBxComentario2Bandeja.Visible = True
                TxBxComentario2Bandeja.Text = ""
                BtnAsignarBandejas.Enabled = False
                If DGVBandejas(5, fila_actual).Value = "Finalizado" Then
                    TxBxComentario1Bandeja.Text = DGVBandejas(4, fila_actual).Value
                    TxBxComentario1Bandeja.Visible = True
                    TxBxComentario1Bandeja.Enabled = False
                    If IsDBNull(DGVBandejas(6, fila_actual).Value) Then
                        TxBxComentario2Bandeja.Text = ""
                    Else
                        TxBxComentario2Bandeja.Text = DGVBandejas(6, fila_actual).Value
                    End If
                    TxBxComentario2Bandeja.Visible = True
                    TxBxComentario2Bandeja.Enabled = False
                    'TxBxValorC2Muestra.Visible = True
                    LabelComentario1Bandeja.Visible = True
                    LabelComentario2Bandeja.Visible = True
                ElseIf DGVBandejas(5, fila_actual).Value = "Revisado" Or DGVBandejas(5, fila_actual).Value = "Pendiente" Then
                    If IsDBNull(DGVBandejas(4, fila_actual).Value) Then
                        BtnAsignarBandejas.Visible = True
                        BtnAsignarBandejas.Enabled = True
                    Else
                        TxBxComentario1Bandeja.Text = DGVBandejas(4, fila_actual).Value
                        TxBxComentario1Bandeja.Visible = True
                        TxBxComentario1Bandeja.Enabled = False
                        LabelComentario1Bandeja.Visible = True
                    End If
                    If IsDBNull(DGVBandejas(6, fila_actual).Value) Then
                        If TxBxComentario1Bandeja.Text <> "" Then
                            TxBxComentario1Bandeja.Text = DGVBandejas(4, fila_actual).Value
                            TxBxComentario1Bandeja.Visible = True
                        End If
                    Else
                        TxBxComentario2Bandeja.Text = DGVBandejas(6, fila_actual).Value
                        TxBxComentario2Bandeja.Visible = True
                        TxBxComentario2Bandeja.Enabled = False
                        LabelValorC2Muestra.Visible = True
                    End If
                    If IsDBNull(DGVBandejas(4, fila_actual).Value) And DGVBandejas(8, fila_actual).Value <> "" Then
                        TxBxComentario1Bandeja.Enabled = True
                        TxBxComentario1Bandeja.Visible = True
                        LabelComentario1Bandeja.Visible = True
                        BtnAsignarBandejas.Visible = True
                        BtnAsignarBandejas.Enabled = True
                    End If
                    If IsDBNull(DGVBandejas(7, fila_actual).Value) Then
                        'BtnAsignarBandejas.Enabled = False
                    ElseIf IsDBNull(DGVBandejas(6, fila_actual).Value) And DGVBandejas(7, fila_actual).Value = "No" Then
                        TxBxComentario1Bandeja.Visible = True
                        LabelComentario1Bandeja.Visible = True
                        TxBxComentario2Bandeja.Visible = True
                        TxBxComentario2Bandeja.Enabled = True
                        LabelComentario2Bandeja.Visible = True
                        BtnAsignarBandejas.Enabled = True
                        BtnAsignarBandejas.Visible = True
                    End If

                End If

            End If
        Catch ex As Exception
            'MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub TxtBxFiltroRegistro_TextChanged(sender As Object, e As EventArgs) Handles TxtBxFiltroRegistro.TextChanged

        Dim usuario As String

        If CmbBxAnalistas.Text = "" Then
            usuario = 1
        Else
            usuario = CmbBxAnalistas.SelectedValue
        End If

        Select Case Pest_actual
            Case 1
                Cargar()

            Case 2
                If usuario > 1 Then
                    Cargar_muestras(usuario)
                Else
                    Cargar_MuestrasBandejas_Admin()
                End If
                Exit Sub
            Case 3
                If usuario > 1 Then
                    Cargar_bandejas(usuario)
                Else
                    Cargar_MuestrasBandejas_Admin()
                End If
                Exit Sub
        End Select

    End Sub

    Private Sub BtnFiltroPrueba_Click(sender As Object, e As EventArgs) Handles BtnFiltroPrueba.Click

        Dim usuario As String

        If CmbBxAnalistas.Text = " " Then
            usuario = 1
        Else
            usuario = CmbBxAnalistas.SelectedValue
        End If

        Select Case Pest_actual
            Case 1
                Cargar()

            Case 2
                If usuario > 1 Then
                    Cargar_muestras(usuario)
                Else
                    Cargar_MuestrasBandejas_Admin()
                End If
                Exit Sub
            Case 3
                If usuario > 1 Then
                    Cargar_bandejas(usuario)
                Else
                    Cargar_MuestrasBandejas_Admin()
                End If
                Exit Sub
        End Select
    End Sub

    Private Sub BtnRecargar_Click(sender As Object, e As EventArgs) Handles BtnRecargar.Click

        If conectado = 1 Then
            CmbBxFiltroPrueba.Text = Nothing
            TxtBxFiltroRegistro.Text = Nothing
            Dim usuario As Integer
            If CmbBxAnalistas.Text = " " Then
                usuario = 1
            Else
                usuario = CmbBxAnalistas.SelectedValue
            End If
            If usuario > 1 Then
                Cargar_muestras(usuario)
                Cargar_bandejas(usuario)
            Else
                Cargar_MuestrasBandejas_Admin()
                Cargar()
            End If
        Else MsgBox("No hay sesion activa", False, "Error")
        End If

    End Sub

    'Controles para el timer de inactividad

    Public Sub New()
        InitializeComponent()
        Application.AddMessageFilter(Me)
        Timer1.Enabled = True
    End Sub

    Public Function PreFilterMessage(ByRef m As Message) As Boolean Implements IMessageFilter.PreFilterMessage
        'Reinicio del timer cuando se detecta uso del teclado o mouse
        If (m.Msg >= &H100 And m.Msg <= &H109) Or (m.Msg >= &H200 And m.Msg <= &H20E) Then
            Timer1.Stop()
            Timer1.Start()
        End If
        Return Nothing
    End Function

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        If conectado = 1 Then
            Timer1.Stop()
            MsgBox("5 minutos de inactividad cerrando sesion activa", False, "Alerta por inactividad")
            flag = 0
            flag1 = 0
            BtnDesconectar.PerformClick()
            Conteo_muestras()
        End If
    End Sub

    'Controles para los timers de Asignacion, Verificacion y Total

    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
        Dim fecha_actual As String = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        Dim fecha_actual_datetime As DateTime = DateTime.ParseExact(fecha_actual, "yyyy-MM-dd HH:mm:ss", Nothing)

        Select Case Pest_actual
            Case 0
                LabelTimerAsignacion.Visible = False
                LabelTimerVerificacion.Visible = False
                LabelTimerTotal.Visible = False
                LabelTituloFiltroRegistro.Visible = False
                TxtBxFiltroRegistro.Visible = False
                CmbBxFiltroPrueba.Visible = False
                BtnFiltroPrueba.Visible = False
                Exit Sub
            Case 1 'Pestaña Administrador
                Dim tabla_actual = LabelAdmin.Text
                Select Case tabla_actual
                    Case "Muestras Asignadas"
                        LabelTituloFiltroRegistro.Text = "Buscar No de Muestra"
                        LabelTituloFiltroRegistro.Visible = True
                        TxtBxFiltroRegistro.Visible = True
                        CmbBxFiltroPrueba.Visible = True
                        BtnFiltroPrueba.Visible = True
                        Try

                            Dim fila_actual As Integer = (DGVAdmin.CurrentRow.Index)

                            Dim at As Integer = DGVAdmin.RowCount
                            If fila_actual = at Then
                                Exit Sub
                            End If
                            If IsDBNull(DGVAdmin(14, fila_actual).Value) Then
                                Dim t_total As String
                                Dim fecha_creacion As String
                                Dim fecha_creacion_datetime As DateTime
                                fecha_creacion = DGVAdmin(9, fila_actual).Value.ToString
                                fecha_creacion_datetime = Convert.ToDateTime(fecha_creacion)
                                t_total = (fecha_actual_datetime - fecha_creacion_datetime).ToString
                                LabelTimerTotal.ForeColor = Color.Red
                                LabelTimerTotal.Visible = True
                                LabelTimerTotal.Text = "Tiempo Total: " & t_total
                            Else
                                Dim t_total As String
                                Dim fecha_terminado As String
                                Dim fecha_terminado_datetime As DateTime
                                Dim fecha_creacion As String
                                Dim fecha_creacion_datetime As DateTime
                                fecha_creacion = DGVAdmin(9, fila_actual).Value.ToString
                                fecha_creacion_datetime = Convert.ToDateTime(fecha_creacion)
                                fecha_terminado = DGVAdmin(14, fila_actual).Value.ToString
                                fecha_terminado_datetime = Convert.ToDateTime(fecha_terminado)
                                t_total = (fecha_terminado_datetime - fecha_creacion_datetime).ToString
                                LabelTimerTotal.ForeColor = Color.Green
                                LabelTimerTotal.Text = "Tiempo Total: " & t_total
                                LabelTimerTotal.Visible = True
                            End If

                            If IsDBNull(DGVAdmin(10, fila_actual).Value) Then
                                Dim fecha_creacion As String
                                Dim fecha_creacion_datetime As DateTime
                                fecha_creacion = DGVAdmin(9, fila_actual).Value.ToString
                                fecha_creacion_datetime = Convert.ToDateTime(fecha_creacion)
                                LabelTimerAsignacion.ForeColor = Color.Red
                                LabelTimerAsignacion.Text = "Tiempo Asignacion: " & (fecha_actual_datetime - fecha_creacion_datetime).ToString()
                                LabelTimerAsignacion.Visible = True
                            Else
                                Dim fecha_asignacion As String
                                Dim fecha_asignacion_datetime As DateTime
                                Dim fecha_creacion As String
                                Dim fecha_creacion_datetime As DateTime
                                fecha_asignacion = DGVAdmin(10, fila_actual).Value.ToString()
                                fecha_asignacion_datetime = Convert.ToDateTime(fecha_asignacion)
                                fecha_creacion = DGVAdmin(9, fila_actual).Value.ToString
                                fecha_creacion_datetime = Convert.ToDateTime(fecha_creacion)
                                LabelTimerAsignacion.ForeColor = Color.Green
                                LabelTimerAsignacion.Text = "Tiempo Asignacion: " & (fecha_asignacion_datetime - fecha_creacion_datetime).ToString()
                                LabelTimerAsignacion.Visible = True
                            End If

                            If IsDBNull(DGVAdmin(11, fila_actual).Value) Then
                                Dim fecha_asignacion As String = DGVAdmin(10, fila_actual).Value.ToString()
                                Dim fecha_asignacion_datetime As DateTime = Convert.ToDateTime(fecha_asignacion)
                                Dim t_verificacion As String = (fecha_actual_datetime - fecha_asignacion_datetime).ToString()
                                LabelTimerVerificacion.ForeColor = Color.Red
                                LabelTimerVerificacion.Text = "Tiempo Verificacion: " & t_verificacion
                                LabelTimerVerificacion.Visible = True
                            ElseIf (Not IsDBNull(DGVAdmin(11, fila_actual).Value)) And IsDBNull(DGVAdmin(12, fila_actual).Value) And IsDBNull(DGVAdmin(13, fila_actual).Value) And IsDBNull(DGVAdmin(7, fila_actual).Value) Then
                                Dim fecha_verificacion_string As String = DGVAdmin(11, fila_actual).Value.ToString()
                                Dim fecha_verificacion_datetime As DateTime = Convert.ToDateTime(fecha_verificacion_string)
                                Dim fecha_asignacion As String = DGVAdmin(10, fila_actual).Value.ToString()
                                Dim fecha_asignacion_datetime As DateTime = Convert.ToDateTime(fecha_asignacion)
                                LabelTimerVerificacion.ForeColor = Color.DarkGoldenrod
                                LabelTimerVerificacion.Text = "Tiempo Verificacion: " & (fecha_verificacion_datetime - fecha_asignacion_datetime).ToString()
                                LabelTimerVerificacion.Visible = True
                            ElseIf (Not IsDBNull(DGVAdmin(11, fila_actual).Value)) And IsDBNull(DGVAdmin(12, fila_actual).Value) And IsDBNull(DGVAdmin(13, fila_actual).Value) And (DGVAdmin(7, fila_actual).Value.ToString() = "Si") Then
                                Dim fecha_verificacion_string As String = DGVAdmin(11, fila_actual).Value.ToString()
                                Dim fecha_verificacion_datetime As DateTime = Convert.ToDateTime(fecha_verificacion_string)
                                Dim fecha_asignacion As String = DGVAdmin(10, fila_actual).Value.ToString()
                                Dim fecha_asignacion_datetime As DateTime = Convert.ToDateTime(fecha_asignacion)
                                LabelTimerVerificacion.ForeColor = Color.Green
                                LabelTimerVerificacion.Text = "Tiempo Verificacion: " & (fecha_verificacion_datetime - fecha_asignacion_datetime).ToString()
                                LabelTimerVerificacion.Visible = True
                            ElseIf (Not IsDBNull(DGVAdmin(11, fila_actual).Value)) And (Not IsDBNull(DGVAdmin(12, fila_actual).Value)) And IsDBNull(DGVAdmin(13, fila_actual).Value) And (DGVAdmin(7, fila_actual).Value.ToString() = "No") Then
                                Dim fecha_verificacion1_string As String = DGVAdmin(11, fila_actual).Value.ToString()
                                Dim fecha_verificacion1_datetime As DateTime = Convert.ToDateTime(fecha_verificacion1_string)
                                Dim fecha_asignacion1 As String = DGVAdmin(10, fila_actual).Value.ToString()
                                Dim fecha_asignacion1_datetime As DateTime = Convert.ToDateTime(fecha_asignacion1)
                                Dim fecha_asignacion2 As String = DGVAdmin(12, fila_actual).Value.ToString()
                                Dim fecha_asignacion2_datetime As DateTime = Convert.ToDateTime(fecha_asignacion2)
                                LabelTimerVerificacion.ForeColor = Color.Red
                                LabelTimerVerificacion.Text = "Tiempo Verificacion: " & ((fecha_verificacion1_datetime - fecha_asignacion1_datetime) + (fecha_actual_datetime - fecha_asignacion2_datetime)).ToString()
                                LabelTimerVerificacion.Visible = True
                            ElseIf (Not IsDBNull(DGVAdmin(11, fila_actual).Value)) And (Not IsDBNull(DGVAdmin(12, fila_actual).Value)) And (Not IsDBNull(DGVAdmin(13, fila_actual).Value)) And IsDBNull(DGVAdmin(7, fila_actual).Value) Then
                                Dim fecha_verificacion1_string As String = DGVAdmin(11, fila_actual).Value.ToString()
                                Dim fecha_verificacion1_datetime As DateTime = Convert.ToDateTime(fecha_verificacion1_string)
                                Dim fecha_verificacion2 As String = DGVAdmin(13, fila_actual).Value.ToString()
                                Dim fecha_verificacion2_datetime As DateTime = Convert.ToDateTime(fecha_verificacion2)
                                Dim fecha_asignacion1 As String = DGVAdmin(10, fila_actual).Value.ToString()
                                Dim fecha_asignacion1_datetime As DateTime = Convert.ToDateTime(fecha_asignacion1)
                                Dim fecha_asignacion2 As String = DGVAdmin(12, fila_actual).Value.ToString()
                                Dim fecha_asignacion2_datetime As DateTime = Convert.ToDateTime(fecha_asignacion2)
                                LabelTimerVerificacion.ForeColor = Color.DarkGoldenrod
                                LabelTimerVerificacion.Text = "Tiempo Verificacion: " & ((fecha_verificacion1_datetime - fecha_asignacion1_datetime) + (fecha_verificacion2_datetime - fecha_asignacion2_datetime)).ToString()
                                LabelTimerVerificacion.Visible = True
                            ElseIf (Not IsDBNull(DGVAdmin(11, fila_actual).Value)) And (Not IsDBNull(DGVAdmin(12, fila_actual).Value)) And (Not IsDBNull(DGVAdmin(13, fila_actual).Value)) And (Not IsDBNull(DGVAdmin(7, fila_actual).Value)) Then
                                Dim fecha_verificacion1_string As String = DGVAdmin(11, fila_actual).Value.ToString()
                                Dim fecha_verificacion1_datetime As DateTime = Convert.ToDateTime(fecha_verificacion1_string)
                                Dim fecha_verificacion2 As String = DGVAdmin(13, fila_actual).Value.ToString()
                                Dim fecha_verificacion2_datetime As DateTime = Convert.ToDateTime(fecha_verificacion2)
                                Dim fecha_asignacion1 As String = DGVAdmin(10, fila_actual).Value.ToString()
                                Dim fecha_asignacion1_datetime As DateTime = Convert.ToDateTime(fecha_asignacion1)
                                Dim fecha_asignacion2 As String = DGVAdmin(12, fila_actual).Value.ToString()
                                Dim fecha_asignacion2_datetime As DateTime = Convert.ToDateTime(fecha_asignacion2)
                                LabelTimerVerificacion.ForeColor = Color.Green
                                LabelTimerVerificacion.Text = "Tiempo Verificacion: " & ((fecha_verificacion1_datetime - fecha_asignacion1_datetime) + (fecha_verificacion2_datetime - fecha_asignacion2_datetime)).ToString()
                                LabelTimerVerificacion.Visible = True
                            End If
                        Catch ex As Exception
                        End Try
                        Exit Sub
                    Case "Muestras no Asignadas"
                        LabelTituloFiltroRegistro.Text = "Buscar No de Muestra"
                        LabelTituloFiltroRegistro.Visible = True
                        TxtBxFiltroRegistro.Visible = True
                        CmbBxFiltroPrueba.Visible = True
                        BtnFiltroPrueba.Visible = True
                        Try

                            Dim fila_actual As Integer = (DGVAdmin.CurrentRow.Index)

                            Dim at As Integer = DGVAdmin.RowCount
                            If fila_actual = at Then
                                Exit Sub
                            End If
                            If IsDBNull(DGVAdmin(9, fila_actual).Value) Then
                                Dim fecha_creacion As String
                                Dim fecha_creacion_datetime As DateTime
                                fecha_creacion = DGVAdmin(8, fila_actual).Value.ToString
                                fecha_creacion_datetime = Convert.ToDateTime(fecha_creacion)
                                LabelTimerAsignacion.ForeColor = Color.Red
                                LabelTimerAsignacion.Text = "Tiempo Asignacion: " & (fecha_actual_datetime - fecha_creacion_datetime).ToString()
                                LabelTimerAsignacion.Visible = True
                            Else
                                Dim fecha_asignacion As String
                                Dim fecha_asignacion_datetime As DateTime
                                Dim fecha_creacion As String
                                Dim fecha_creacion_datetime As DateTime
                                fecha_asignacion = DGVAdmin(9, fila_actual).Value.ToString()
                                fecha_asignacion_datetime = Convert.ToDateTime(fecha_asignacion)
                                fecha_creacion = DGVAdmin(8, fila_actual).Value.ToString
                                fecha_creacion_datetime = Convert.ToDateTime(fecha_creacion)
                                LabelTimerAsignacion.ForeColor = Color.Green
                                LabelTimerAsignacion.Text = "Tiempo Asignacion: " & (fecha_asignacion_datetime - fecha_creacion_datetime).ToString()
                                LabelTimerAsignacion.Visible = True
                            End If
                        Catch ex As Exception
                        End Try
                        Exit Sub
                    Case "Bandejas Asignadas"
                        LabelTituloFiltroRegistro.Text = "Buscar No de Bandeja"
                        LabelTituloFiltroRegistro.Visible = True
                        TxtBxFiltroRegistro.Visible = True
                        CmbBxFiltroPrueba.Visible = True
                        BtnFiltroPrueba.Visible = True
                        Try

                            Dim fila_actual As Integer = (DGVAdmin.CurrentRow.Index)

                            Dim at As Integer = DGVAdmin.RowCount
                            If fila_actual = at Then
                                Exit Sub
                            End If
                            If IsDBNull(DGVAdmin(14, fila_actual).Value) Then
                                Dim t_total As String
                                Dim fecha_creacion As String
                                Dim fecha_creacion_datetime As DateTime
                                fecha_creacion = DGVAdmin(9, fila_actual).Value.ToString
                                fecha_creacion_datetime = Convert.ToDateTime(fecha_creacion)
                                t_total = (fecha_actual_datetime - fecha_creacion_datetime).ToString
                                LabelTimerTotal.ForeColor = Color.Red
                                LabelTimerTotal.Visible = True
                                LabelTimerTotal.Text = "Tiempo Total: " & t_total
                            Else
                                Dim t_total As String
                                Dim fecha_terminado As String
                                Dim fecha_terminado_datetime As DateTime
                                Dim fecha_creacion As String
                                Dim fecha_creacion_datetime As DateTime
                                fecha_creacion = DGVAdmin(9, fila_actual).Value.ToString
                                fecha_creacion_datetime = Convert.ToDateTime(fecha_creacion)
                                fecha_terminado = DGVAdmin(14, fila_actual).Value.ToString
                                fecha_terminado_datetime = Convert.ToDateTime(fecha_terminado)
                                t_total = (fecha_terminado_datetime - fecha_creacion_datetime).ToString
                                LabelTimerTotal.ForeColor = Color.Green
                                LabelTimerTotal.Text = "Tiempo Total: " & t_total
                                LabelTimerTotal.Visible = True
                            End If

                            If IsDBNull(DGVAdmin(10, fila_actual).Value) Then
                                Dim fecha_creacion As String
                                Dim fecha_creacion_datetime As DateTime
                                fecha_creacion = DGVAdmin(9, fila_actual).Value.ToString
                                fecha_creacion_datetime = Convert.ToDateTime(fecha_creacion)
                                LabelTimerAsignacion.ForeColor = Color.Red
                                LabelTimerAsignacion.Text = "Tiempo Asignacion: " & (fecha_actual_datetime - fecha_creacion_datetime).ToString()
                                LabelTimerAsignacion.Visible = True
                            Else
                                Dim fecha_asignacion As String
                                Dim fecha_asignacion_datetime As DateTime
                                Dim fecha_creacion As String
                                Dim fecha_creacion_datetime As DateTime
                                fecha_asignacion = DGVAdmin(10, fila_actual).Value.ToString()
                                fecha_asignacion_datetime = Convert.ToDateTime(fecha_asignacion)
                                fecha_creacion = DGVAdmin(9, fila_actual).Value.ToString
                                fecha_creacion_datetime = Convert.ToDateTime(fecha_creacion)
                                LabelTimerAsignacion.ForeColor = Color.Green
                                LabelTimerAsignacion.Text = "Tiempo Asignacion: " & (fecha_asignacion_datetime - fecha_creacion_datetime).ToString()
                                LabelTimerAsignacion.Visible = True
                            End If

                            If IsDBNull(DGVAdmin(11, fila_actual).Value) Then
                                Dim fecha_asignacion As String = DGVAdmin(10, fila_actual).Value.ToString()
                                Dim fecha_asignacion_datetime As DateTime = Convert.ToDateTime(fecha_asignacion)
                                Dim t_verificacion As String = (fecha_actual_datetime - fecha_asignacion_datetime).ToString()
                                LabelTimerVerificacion.ForeColor = Color.Red
                                LabelTimerVerificacion.Text = "Tiempo Verificacion: " & t_verificacion
                                LabelTimerVerificacion.Visible = True
                            ElseIf (Not IsDBNull(DGVAdmin(11, fila_actual).Value)) And IsDBNull(DGVAdmin(12, fila_actual).Value) And IsDBNull(DGVAdmin(13, fila_actual).Value) And IsDBNull(DGVAdmin(7, fila_actual).Value) Then
                                Dim fecha_verificacion_string As String = DGVAdmin(11, fila_actual).Value.ToString()
                                Dim fecha_verificacion_datetime As DateTime = Convert.ToDateTime(fecha_verificacion_string)
                                Dim fecha_asignacion As String = DGVAdmin(10, fila_actual).Value.ToString()
                                Dim fecha_asignacion_datetime As DateTime = Convert.ToDateTime(fecha_asignacion)
                                LabelTimerVerificacion.ForeColor = Color.DarkGoldenrod
                                LabelTimerVerificacion.Text = "Tiempo Verificacion: " & (fecha_verificacion_datetime - fecha_asignacion_datetime).ToString()
                                LabelTimerVerificacion.Visible = True
                            ElseIf (Not IsDBNull(DGVAdmin(11, fila_actual).Value)) And IsDBNull(DGVAdmin(12, fila_actual).Value) And IsDBNull(DGVAdmin(13, fila_actual).Value) And (DGVAdmin(7, fila_actual).Value.ToString() = "Si") Then
                                Dim fecha_verificacion_string As String = DGVAdmin(11, fila_actual).Value.ToString()
                                Dim fecha_verificacion_datetime As DateTime = Convert.ToDateTime(fecha_verificacion_string)
                                Dim fecha_asignacion As String = DGVAdmin(10, fila_actual).Value.ToString()
                                Dim fecha_asignacion_datetime As DateTime = Convert.ToDateTime(fecha_asignacion)
                                LabelTimerVerificacion.ForeColor = Color.Green
                                LabelTimerVerificacion.Text = "Tiempo Verificacion: " & (fecha_verificacion_datetime - fecha_asignacion_datetime).ToString()
                                LabelTimerVerificacion.Visible = True
                            ElseIf (Not IsDBNull(DGVAdmin(11, fila_actual).Value)) And (Not IsDBNull(DGVAdmin(12, fila_actual).Value)) And IsDBNull(DGVAdmin(13, fila_actual).Value) And (DGVAdmin(7, fila_actual).Value.ToString() = "No") Then
                                Dim fecha_verificacion1_string As String = DGVAdmin(11, fila_actual).Value.ToString()
                                Dim fecha_verificacion1_datetime As DateTime = Convert.ToDateTime(fecha_verificacion1_string)
                                Dim fecha_asignacion1 As String = DGVAdmin(10, fila_actual).Value.ToString()
                                Dim fecha_asignacion1_datetime As DateTime = Convert.ToDateTime(fecha_asignacion1)
                                Dim fecha_asignacion2 As String = DGVAdmin(12, fila_actual).Value.ToString()
                                Dim fecha_asignacion2_datetime As DateTime = Convert.ToDateTime(fecha_asignacion2)
                                LabelTimerVerificacion.ForeColor = Color.Red
                                LabelTimerVerificacion.Text = "Tiempo Verificacion: " & ((fecha_verificacion1_datetime - fecha_asignacion1_datetime) + (fecha_actual_datetime - fecha_asignacion2_datetime)).ToString()
                                LabelTimerVerificacion.Visible = True
                            ElseIf (Not IsDBNull(DGVAdmin(11, fila_actual).Value)) And (Not IsDBNull(DGVAdmin(12, fila_actual).Value)) And (Not IsDBNull(DGVAdmin(13, fila_actual).Value)) And IsDBNull(DGVAdmin(7, fila_actual).Value) Then
                                Dim fecha_verificacion1_string As String = DGVAdmin(11, fila_actual).Value.ToString()
                                Dim fecha_verificacion1_datetime As DateTime = Convert.ToDateTime(fecha_verificacion1_string)
                                Dim fecha_verificacion2 As String = DGVAdmin(13, fila_actual).Value.ToString()
                                Dim fecha_verificacion2_datetime As DateTime = Convert.ToDateTime(fecha_verificacion2)
                                Dim fecha_asignacion1 As String = DGVAdmin(10, fila_actual).Value.ToString()
                                Dim fecha_asignacion1_datetime As DateTime = Convert.ToDateTime(fecha_asignacion1)
                                Dim fecha_asignacion2 As String = DGVAdmin(12, fila_actual).Value.ToString()
                                Dim fecha_asignacion2_datetime As DateTime = Convert.ToDateTime(fecha_asignacion2)
                                LabelTimerVerificacion.ForeColor = Color.DarkGoldenrod
                                LabelTimerVerificacion.Text = "Tiempo Verificacion: " & ((fecha_verificacion1_datetime - fecha_asignacion1_datetime) + (fecha_verificacion2_datetime - fecha_asignacion2_datetime)).ToString()
                                LabelTimerVerificacion.Visible = True
                            ElseIf (Not IsDBNull(DGVAdmin(11, fila_actual).Value)) And (Not IsDBNull(DGVAdmin(12, fila_actual).Value)) And (Not IsDBNull(DGVAdmin(13, fila_actual).Value)) And (Not IsDBNull(DGVAdmin(7, fila_actual).Value)) Then
                                Dim fecha_verificacion1_string As String = DGVAdmin(11, fila_actual).Value.ToString()
                                Dim fecha_verificacion1_datetime As DateTime = Convert.ToDateTime(fecha_verificacion1_string)
                                Dim fecha_verificacion2 As String = DGVAdmin(13, fila_actual).Value.ToString()
                                Dim fecha_verificacion2_datetime As DateTime = Convert.ToDateTime(fecha_verificacion2)
                                Dim fecha_asignacion1 As String = DGVAdmin(10, fila_actual).Value.ToString()
                                Dim fecha_asignacion1_datetime As DateTime = Convert.ToDateTime(fecha_asignacion1)
                                Dim fecha_asignacion2 As String = DGVAdmin(12, fila_actual).Value.ToString()
                                Dim fecha_asignacion2_datetime As DateTime = Convert.ToDateTime(fecha_asignacion2)
                                LabelTimerVerificacion.ForeColor = Color.Green
                                LabelTimerVerificacion.Text = "Tiempo Verificacion: " & ((fecha_verificacion1_datetime - fecha_asignacion1_datetime) + (fecha_verificacion2_datetime - fecha_asignacion2_datetime)).ToString()
                                LabelTimerVerificacion.Visible = True
                            End If
                        Catch ex As Exception
                        End Try
                        Exit Sub
                    Case "Bandejas no Asignadas"
                        LabelTituloFiltroRegistro.Text = "Buscar No de Bandeja"
                        LabelTituloFiltroRegistro.Visible = True
                        TxtBxFiltroRegistro.Visible = True
                        CmbBxFiltroPrueba.Visible = True
                        BtnFiltroPrueba.Visible = True
                        Try

                            Dim fila_actual As Integer = (DGVAdmin.CurrentRow.Index)

                            Dim at As Integer = DGVAdmin.RowCount
                            If fila_actual = at Then
                                Exit Sub
                            End If
                            If IsDBNull(DGVAdmin(9, fila_actual).Value) Then
                                Dim fecha_creacion As String
                                Dim fecha_creacion_datetime As DateTime
                                fecha_creacion = DGVAdmin(8, fila_actual).Value.ToString
                                fecha_creacion_datetime = Convert.ToDateTime(fecha_creacion)
                                LabelTimerAsignacion.ForeColor = Color.Red
                                LabelTimerAsignacion.Text = "Tiempo Asignacion: " & (fecha_actual_datetime - fecha_creacion_datetime).ToString()
                                LabelTimerAsignacion.Visible = True
                            Else
                                Dim fecha_asignacion As String
                                Dim fecha_asignacion_datetime As DateTime
                                Dim fecha_creacion As String
                                Dim fecha_creacion_datetime As DateTime
                                fecha_asignacion = DGVAdmin(9, fila_actual).Value.ToString()
                                fecha_asignacion_datetime = Convert.ToDateTime(fecha_asignacion)
                                fecha_creacion = DGVAdmin(8, fila_actual).Value.ToString
                                fecha_creacion_datetime = Convert.ToDateTime(fecha_creacion)
                                LabelTimerAsignacion.ForeColor = Color.Green
                                LabelTimerAsignacion.Text = "Tiempo Asignacion: " & (fecha_asignacion_datetime - fecha_creacion_datetime).ToString()
                                LabelTimerAsignacion.Visible = True
                            End If
                        Catch ex As Exception
                        End Try
                        Exit Sub
                    Case "Historial de Muestras"
                        LabelTituloFiltroRegistro.Text = "Buscar No de Muestra"
                        LabelTituloFiltroRegistro.Visible = True
                        TxtBxFiltroRegistro.Visible = True
                        CmbBxFiltroPrueba.Visible = True
                        BtnFiltroPrueba.Visible = True
                        Try
                            Dim fila_actual As Integer = (DGVAdmin.CurrentRow.Index)

                            Dim at As Integer = DGVAdmin.RowCount
                            If fila_actual = at Then
                                Exit Sub
                            End If
                            If IsDBNull(DGVAdmin(14, fila_actual).Value) Then
                                Dim t_total As String
                                Dim fecha_creacion As String
                                Dim fecha_creacion_datetime As DateTime
                                fecha_creacion = DGVAdmin(9, fila_actual).Value.ToString
                                fecha_creacion_datetime = Convert.ToDateTime(fecha_creacion)
                                t_total = (fecha_actual_datetime - fecha_creacion_datetime).ToString
                                LabelTimerTotal.ForeColor = Color.Red
                                LabelTimerTotal.Visible = True
                                LabelTimerTotal.Text = "Tiempo Total: " & t_total
                            Else
                                Dim t_total As String
                                Dim fecha_terminado As String
                                Dim fecha_terminado_datetime As DateTime
                                Dim fecha_creacion As String
                                Dim fecha_creacion_datetime As DateTime
                                fecha_creacion = DGVAdmin(9, fila_actual).Value.ToString
                                fecha_creacion_datetime = Convert.ToDateTime(fecha_creacion)
                                fecha_terminado = DGVAdmin(14, fila_actual).Value.ToString
                                fecha_terminado_datetime = Convert.ToDateTime(fecha_terminado)
                                t_total = (fecha_terminado_datetime - fecha_creacion_datetime).ToString
                                LabelTimerTotal.ForeColor = Color.Green
                                LabelTimerTotal.Text = "Tiempo Total: " & t_total
                                LabelTimerTotal.Visible = True
                            End If

                            If IsDBNull(DGVAdmin(10, fila_actual).Value) Then
                                Dim fecha_creacion As String
                                Dim fecha_creacion_datetime As DateTime
                                fecha_creacion = DGVAdmin(9, fila_actual).Value.ToString
                                fecha_creacion_datetime = Convert.ToDateTime(fecha_creacion)
                                LabelTimerAsignacion.ForeColor = Color.Red
                                LabelTimerAsignacion.Text = "Tiempo Asignacion: " & (fecha_actual_datetime - fecha_creacion_datetime).ToString()
                                LabelTimerAsignacion.Visible = True
                            Else
                                Dim fecha_asignacion As String
                                Dim fecha_asignacion_datetime As DateTime
                                Dim fecha_creacion As String
                                Dim fecha_creacion_datetime As DateTime
                                fecha_asignacion = DGVAdmin(10, fila_actual).Value.ToString()
                                fecha_asignacion_datetime = Convert.ToDateTime(fecha_asignacion)
                                fecha_creacion = DGVAdmin(9, fila_actual).Value.ToString
                                fecha_creacion_datetime = Convert.ToDateTime(fecha_creacion)
                                LabelTimerAsignacion.ForeColor = Color.Green
                                LabelTimerAsignacion.Text = "Tiempo Asignacion: " & (fecha_asignacion_datetime - fecha_creacion_datetime).ToString()
                                LabelTimerAsignacion.Visible = True
                            End If

                            If IsDBNull(DGVAdmin(11, fila_actual).Value) Then
                                Dim fecha_asignacion As String = DGVAdmin(10, fila_actual).Value.ToString()
                                Dim fecha_asignacion_datetime As DateTime = Convert.ToDateTime(fecha_asignacion)
                                Dim t_verificacion As String = (fecha_actual_datetime - fecha_asignacion_datetime).ToString()
                                LabelTimerVerificacion.ForeColor = Color.Red
                                LabelTimerVerificacion.Text = "Tiempo Verificacion: " & t_verificacion
                                LabelTimerVerificacion.Visible = True
                            ElseIf (Not IsDBNull(DGVAdmin(11, fila_actual).Value)) And IsDBNull(DGVAdmin(12, fila_actual).Value) And IsDBNull(DGVAdmin(13, fila_actual).Value) And IsDBNull(DGVAdmin(7, fila_actual).Value) Then
                                Dim fecha_verificacion_string As String = DGVAdmin(11, fila_actual).Value.ToString()
                                Dim fecha_verificacion_datetime As DateTime = Convert.ToDateTime(fecha_verificacion_string)
                                Dim fecha_asignacion As String = DGVAdmin(10, fila_actual).Value.ToString()
                                Dim fecha_asignacion_datetime As DateTime = Convert.ToDateTime(fecha_asignacion)
                                LabelTimerVerificacion.ForeColor = Color.DarkGoldenrod
                                LabelTimerVerificacion.Text = "Tiempo Verificacion: " & (fecha_verificacion_datetime - fecha_asignacion_datetime).ToString()
                                LabelTimerVerificacion.Visible = True
                            ElseIf (Not IsDBNull(DGVAdmin(11, fila_actual).Value)) And IsDBNull(DGVAdmin(12, fila_actual).Value) And IsDBNull(DGVAdmin(13, fila_actual).Value) And (DGVAdmin(7, fila_actual).Value.ToString() = "Si") Then
                                Dim fecha_verificacion_string As String = DGVAdmin(11, fila_actual).Value.ToString()
                                Dim fecha_verificacion_datetime As DateTime = Convert.ToDateTime(fecha_verificacion_string)
                                Dim fecha_asignacion As String = DGVAdmin(10, fila_actual).Value.ToString()
                                Dim fecha_asignacion_datetime As DateTime = Convert.ToDateTime(fecha_asignacion)
                                LabelTimerVerificacion.ForeColor = Color.Green
                                LabelTimerVerificacion.Text = "Tiempo Verificacion: " & (fecha_verificacion_datetime - fecha_asignacion_datetime).ToString()
                                LabelTimerVerificacion.Visible = True
                            ElseIf (Not IsDBNull(DGVAdmin(11, fila_actual).Value)) And (Not IsDBNull(DGVAdmin(12, fila_actual).Value)) And IsDBNull(DGVAdmin(13, fila_actual).Value) And (DGVAdmin(7, fila_actual).Value.ToString() = "No") Then
                                Dim fecha_verificacion1_string As String = DGVAdmin(11, fila_actual).Value.ToString()
                                Dim fecha_verificacion1_datetime As DateTime = Convert.ToDateTime(fecha_verificacion1_string)
                                Dim fecha_asignacion1 As String = DGVAdmin(10, fila_actual).Value.ToString()
                                Dim fecha_asignacion1_datetime As DateTime = Convert.ToDateTime(fecha_asignacion1)
                                Dim fecha_asignacion2 As String = DGVAdmin(12, fila_actual).Value.ToString()
                                Dim fecha_asignacion2_datetime As DateTime = Convert.ToDateTime(fecha_asignacion2)
                                LabelTimerVerificacion.ForeColor = Color.Red
                                LabelTimerVerificacion.Text = "Tiempo Verificacion: " & ((fecha_verificacion1_datetime - fecha_asignacion1_datetime) + (fecha_actual_datetime - fecha_asignacion2_datetime)).ToString()
                                LabelTimerVerificacion.Visible = True
                            ElseIf (Not IsDBNull(DGVAdmin(11, fila_actual).Value)) And (Not IsDBNull(DGVAdmin(12, fila_actual).Value)) And (Not IsDBNull(DGVAdmin(13, fila_actual).Value)) And IsDBNull(DGVAdmin(7, fila_actual).Value) Then
                                Dim fecha_verificacion1_string As String = DGVAdmin(11, fila_actual).Value.ToString()
                                Dim fecha_verificacion1_datetime As DateTime = Convert.ToDateTime(fecha_verificacion1_string)
                                Dim fecha_verificacion2 As String = DGVAdmin(13, fila_actual).Value.ToString()
                                Dim fecha_verificacion2_datetime As DateTime = Convert.ToDateTime(fecha_verificacion2)
                                Dim fecha_asignacion1 As String = DGVAdmin(10, fila_actual).Value.ToString()
                                Dim fecha_asignacion1_datetime As DateTime = Convert.ToDateTime(fecha_asignacion1)
                                Dim fecha_asignacion2 As String = DGVAdmin(12, fila_actual).Value.ToString()
                                Dim fecha_asignacion2_datetime As DateTime = Convert.ToDateTime(fecha_asignacion2)
                                LabelTimerVerificacion.ForeColor = Color.DarkGoldenrod
                                LabelTimerVerificacion.Text = "Tiempo Verificacion: " & ((fecha_verificacion1_datetime - fecha_asignacion1_datetime) + (fecha_verificacion2_datetime - fecha_asignacion2_datetime)).ToString()
                                LabelTimerVerificacion.Visible = True
                            ElseIf (Not IsDBNull(DGVAdmin(11, fila_actual).Value)) And (Not IsDBNull(DGVAdmin(12, fila_actual).Value)) And (Not IsDBNull(DGVAdmin(13, fila_actual).Value)) And (Not IsDBNull(DGVAdmin(7, fila_actual).Value)) Then
                                Dim fecha_verificacion1_string As String = DGVAdmin(11, fila_actual).Value.ToString()
                                Dim fecha_verificacion1_datetime As DateTime = Convert.ToDateTime(fecha_verificacion1_string)
                                Dim fecha_verificacion2 As String = DGVAdmin(13, fila_actual).Value.ToString()
                                Dim fecha_verificacion2_datetime As DateTime = Convert.ToDateTime(fecha_verificacion2)
                                Dim fecha_asignacion1 As String = DGVAdmin(10, fila_actual).Value.ToString()
                                Dim fecha_asignacion1_datetime As DateTime = Convert.ToDateTime(fecha_asignacion1)
                                Dim fecha_asignacion2 As String = DGVAdmin(12, fila_actual).Value.ToString()
                                Dim fecha_asignacion2_datetime As DateTime = Convert.ToDateTime(fecha_asignacion2)
                                LabelTimerVerificacion.ForeColor = Color.Green
                                LabelTimerVerificacion.Text = "Tiempo Verificacion: " & ((fecha_verificacion1_datetime - fecha_asignacion1_datetime) + (fecha_verificacion2_datetime - fecha_asignacion2_datetime)).ToString()
                                LabelTimerVerificacion.Visible = True
                            End If
                        Catch ex As Exception
                        End Try
                        Exit Sub
                    Case "Historial de Bandejas"
                        LabelTituloFiltroRegistro.Text = "Buscar No de Bandeja"
                        LabelTituloFiltroRegistro.Visible = True
                        TxtBxFiltroRegistro.Visible = True
                        CmbBxFiltroPrueba.Visible = True
                        BtnFiltroPrueba.Visible = True
                        Try
                            Dim fila_actual As Integer = (DGVAdmin.CurrentRow.Index)

                            Dim at As Integer = DGVAdmin.RowCount
                            If fila_actual = at Then
                                Exit Sub
                            End If
                            If IsDBNull(DGVAdmin(14, fila_actual).Value) Then
                                Dim t_total As String
                                Dim fecha_creacion As String
                                Dim fecha_creacion_datetime As DateTime
                                fecha_creacion = DGVAdmin(9, fila_actual).Value.ToString
                                fecha_creacion_datetime = Convert.ToDateTime(fecha_creacion)
                                t_total = (fecha_actual_datetime - fecha_creacion_datetime).ToString
                                LabelTimerTotal.ForeColor = Color.Red
                                LabelTimerTotal.Visible = True
                                LabelTimerTotal.Text = "Tiempo Total: " & t_total
                            Else
                                Dim t_total As String
                                Dim fecha_terminado As String
                                Dim fecha_terminado_datetime As DateTime
                                Dim fecha_creacion As String
                                Dim fecha_creacion_datetime As DateTime
                                fecha_creacion = DGVAdmin(9, fila_actual).Value.ToString
                                fecha_creacion_datetime = Convert.ToDateTime(fecha_creacion)
                                fecha_terminado = DGVAdmin(14, fila_actual).Value.ToString
                                fecha_terminado_datetime = Convert.ToDateTime(fecha_terminado)
                                t_total = (fecha_terminado_datetime - fecha_creacion_datetime).ToString
                                LabelTimerTotal.ForeColor = Color.Green
                                LabelTimerTotal.Text = "Tiempo Total: " & t_total
                                LabelTimerTotal.Visible = True
                            End If

                            If IsDBNull(DGVAdmin(10, fila_actual).Value) Then
                                Dim fecha_creacion As String
                                Dim fecha_creacion_datetime As DateTime
                                fecha_creacion = DGVAdmin(9, fila_actual).Value.ToString
                                fecha_creacion_datetime = Convert.ToDateTime(fecha_creacion)
                                LabelTimerAsignacion.ForeColor = Color.Red
                                LabelTimerAsignacion.Text = "Tiempo Asignacion: " & (fecha_actual_datetime - fecha_creacion_datetime).ToString()
                                LabelTimerAsignacion.Visible = True
                            Else
                                Dim fecha_asignacion As String
                                Dim fecha_asignacion_datetime As DateTime
                                Dim fecha_creacion As String
                                Dim fecha_creacion_datetime As DateTime
                                fecha_asignacion = DGVAdmin(10, fila_actual).Value.ToString()
                                fecha_asignacion_datetime = Convert.ToDateTime(fecha_asignacion)
                                fecha_creacion = DGVAdmin(9, fila_actual).Value.ToString
                                fecha_creacion_datetime = Convert.ToDateTime(fecha_creacion)
                                LabelTimerAsignacion.ForeColor = Color.Green
                                LabelTimerAsignacion.Text = "Tiempo Asignacion: " & (fecha_asignacion_datetime - fecha_creacion_datetime).ToString()
                                LabelTimerAsignacion.Visible = True
                            End If

                            If IsDBNull(DGVAdmin(11, fila_actual).Value) Then
                                Dim fecha_asignacion As String = DGVAdmin(10, fila_actual).Value.ToString()
                                Dim fecha_asignacion_datetime As DateTime = Convert.ToDateTime(fecha_asignacion)
                                Dim t_verificacion As String = (fecha_actual_datetime - fecha_asignacion_datetime).ToString()
                                LabelTimerVerificacion.ForeColor = Color.Red
                                LabelTimerVerificacion.Text = "Tiempo Verificacion: " & t_verificacion
                                LabelTimerVerificacion.Visible = True
                            ElseIf (Not IsDBNull(DGVAdmin(11, fila_actual).Value)) And IsDBNull(DGVAdmin(12, fila_actual).Value) And IsDBNull(DGVAdmin(13, fila_actual).Value) And IsDBNull(DGVAdmin(7, fila_actual).Value) Then
                                Dim fecha_verificacion_string As String = DGVAdmin(11, fila_actual).Value.ToString()
                                Dim fecha_verificacion_datetime As DateTime = Convert.ToDateTime(fecha_verificacion_string)
                                Dim fecha_asignacion As String = DGVAdmin(10, fila_actual).Value.ToString()
                                Dim fecha_asignacion_datetime As DateTime = Convert.ToDateTime(fecha_asignacion)
                                LabelTimerVerificacion.ForeColor = Color.DarkGoldenrod
                                LabelTimerVerificacion.Text = "Tiempo Verificacion: " & (fecha_verificacion_datetime - fecha_asignacion_datetime).ToString()
                                LabelTimerVerificacion.Visible = True
                            ElseIf (Not IsDBNull(DGVAdmin(11, fila_actual).Value)) And IsDBNull(DGVAdmin(12, fila_actual).Value) And IsDBNull(DGVAdmin(13, fila_actual).Value) And (DGVAdmin(7, fila_actual).Value.ToString() = "Si") Then
                                Dim fecha_verificacion_string As String = DGVAdmin(11, fila_actual).Value.ToString()
                                Dim fecha_verificacion_datetime As DateTime = Convert.ToDateTime(fecha_verificacion_string)
                                Dim fecha_asignacion As String = DGVAdmin(10, fila_actual).Value.ToString()
                                Dim fecha_asignacion_datetime As DateTime = Convert.ToDateTime(fecha_asignacion)
                                LabelTimerVerificacion.ForeColor = Color.Green
                                LabelTimerVerificacion.Text = "Tiempo Verificacion: " & (fecha_verificacion_datetime - fecha_asignacion_datetime).ToString()
                                LabelTimerVerificacion.Visible = True
                            ElseIf (Not IsDBNull(DGVAdmin(11, fila_actual).Value)) And (Not IsDBNull(DGVAdmin(12, fila_actual).Value)) And IsDBNull(DGVAdmin(13, fila_actual).Value) And (DGVAdmin(7, fila_actual).Value.ToString() = "No") Then
                                Dim fecha_verificacion1_string As String = DGVAdmin(11, fila_actual).Value.ToString()
                                Dim fecha_verificacion1_datetime As DateTime = Convert.ToDateTime(fecha_verificacion1_string)
                                Dim fecha_asignacion1 As String = DGVAdmin(10, fila_actual).Value.ToString()
                                Dim fecha_asignacion1_datetime As DateTime = Convert.ToDateTime(fecha_asignacion1)
                                Dim fecha_asignacion2 As String = DGVAdmin(12, fila_actual).Value.ToString()
                                Dim fecha_asignacion2_datetime As DateTime = Convert.ToDateTime(fecha_asignacion2)
                                LabelTimerVerificacion.ForeColor = Color.Red
                                LabelTimerVerificacion.Text = "Tiempo Verificacion: " & ((fecha_verificacion1_datetime - fecha_asignacion1_datetime) + (fecha_actual_datetime - fecha_asignacion2_datetime)).ToString()
                                LabelTimerVerificacion.Visible = True
                            ElseIf (Not IsDBNull(DGVAdmin(11, fila_actual).Value)) And (Not IsDBNull(DGVAdmin(12, fila_actual).Value)) And (Not IsDBNull(DGVAdmin(13, fila_actual).Value)) And IsDBNull(DGVAdmin(7, fila_actual).Value) Then
                                Dim fecha_verificacion1_string As String = DGVAdmin(11, fila_actual).Value.ToString()
                                Dim fecha_verificacion1_datetime As DateTime = Convert.ToDateTime(fecha_verificacion1_string)
                                Dim fecha_verificacion2 As String = DGVAdmin(13, fila_actual).Value.ToString()
                                Dim fecha_verificacion2_datetime As DateTime = Convert.ToDateTime(fecha_verificacion2)
                                Dim fecha_asignacion1 As String = DGVAdmin(10, fila_actual).Value.ToString()
                                Dim fecha_asignacion1_datetime As DateTime = Convert.ToDateTime(fecha_asignacion1)
                                Dim fecha_asignacion2 As String = DGVAdmin(12, fila_actual).Value.ToString()
                                Dim fecha_asignacion2_datetime As DateTime = Convert.ToDateTime(fecha_asignacion2)
                                LabelTimerVerificacion.ForeColor = Color.DarkGoldenrod
                                LabelTimerVerificacion.Text = "Tiempo Verificacion: " & ((fecha_verificacion1_datetime - fecha_asignacion1_datetime) + (fecha_verificacion2_datetime - fecha_asignacion2_datetime)).ToString()
                                LabelTimerVerificacion.Visible = True
                            ElseIf (Not IsDBNull(DGVAdmin(11, fila_actual).Value)) And (Not IsDBNull(DGVAdmin(12, fila_actual).Value)) And (Not IsDBNull(DGVAdmin(13, fila_actual).Value)) And (Not IsDBNull(DGVAdmin(7, fila_actual).Value)) Then
                                Dim fecha_verificacion1_string As String = DGVAdmin(11, fila_actual).Value.ToString()
                                Dim fecha_verificacion1_datetime As DateTime = Convert.ToDateTime(fecha_verificacion1_string)
                                Dim fecha_verificacion2 As String = DGVAdmin(13, fila_actual).Value.ToString()
                                Dim fecha_verificacion2_datetime As DateTime = Convert.ToDateTime(fecha_verificacion2)
                                Dim fecha_asignacion1 As String = DGVAdmin(10, fila_actual).Value.ToString()
                                Dim fecha_asignacion1_datetime As DateTime = Convert.ToDateTime(fecha_asignacion1)
                                Dim fecha_asignacion2 As String = DGVAdmin(12, fila_actual).Value.ToString()
                                Dim fecha_asignacion2_datetime As DateTime = Convert.ToDateTime(fecha_asignacion2)
                                LabelTimerVerificacion.ForeColor = Color.Green
                                LabelTimerVerificacion.Text = "Tiempo Verificacion: " & ((fecha_verificacion1_datetime - fecha_asignacion1_datetime) + (fecha_verificacion2_datetime - fecha_asignacion2_datetime)).ToString()
                                LabelTimerVerificacion.Visible = True
                            End If
                        Catch ex As Exception
                        End Try
                        Exit Sub
                    Case Else
                        LabelTimerAsignacion.Visible = False
                        LabelTimerVerificacion.Visible = False
                        LabelTimerTotal.Visible = False
                        TxtBxFiltroRegistro.Visible = False
                        LabelTituloFiltroRegistro.Visible = False
                        CmbBxFiltroPrueba.Visible = False
                        BtnFiltroPrueba.Visible = False
                        Exit Sub
                End Select
                Exit Sub
            Case 2 'pestaña muestras
                Try
                    If flag = 1 Then
                        LabelTimerAsignacion.Visible = True
                        LabelTimerVerificacion.Visible = True
                        LabelTimerTotal.Visible = True
                        If conectado = 1 Then
                            LabelTituloFiltroRegistro.Text = "Buscar No de Muestra"
                            LabelTituloFiltroRegistro.Visible = True
                            TxtBxFiltroRegistro.Visible = True
                            CmbBxFiltroPrueba.Visible = True
                            BtnFiltroPrueba.Visible = True
                        Else
                            LabelTituloFiltroRegistro.Visible = False
                            TxtBxFiltroRegistro.Visible = False
                            CmbBxFiltroPrueba.Visible = False
                            BtnFiltroPrueba.Visible = False
                        End If

                        Dim fila_actual As Integer = (DGVMuestras.CurrentRow.Index)

                        Dim at As Integer = DGVMuestras.RowCount
                        If fila_actual = at Then
                            Exit Sub
                        End If
                        If IsDBNull(DGVMuestras(14, fila_actual).Value) Then
                            Dim t_total As String
                            Dim fecha_creacion As String
                            Dim fecha_creacion_datetime As DateTime
                            fecha_creacion = DGVMuestras(9, fila_actual).Value.ToString
                            fecha_creacion_datetime = Convert.ToDateTime(fecha_creacion)
                            t_total = (fecha_actual_datetime - fecha_creacion_datetime).ToString
                            LabelTimerTotal.ForeColor = Color.Red
                            LabelTimerTotal.Visible = True
                            LabelTimerTotal.Text = "Tiempo Total: " & t_total
                        Else
                            Dim t_total As String
                            Dim fecha_terminado As String
                            Dim fecha_terminado_datetime As DateTime
                            Dim fecha_creacion As String
                            Dim fecha_creacion_datetime As DateTime
                            fecha_creacion = DGVMuestras(9, fila_actual).Value.ToString
                            fecha_creacion_datetime = Convert.ToDateTime(fecha_creacion)
                            fecha_terminado = DGVMuestras(14, fila_actual).Value.ToString
                            fecha_terminado_datetime = Convert.ToDateTime(fecha_terminado)
                            t_total = (fecha_terminado_datetime - fecha_creacion_datetime).ToString
                            LabelTimerTotal.ForeColor = Color.Green
                            LabelTimerTotal.Text = "Tiempo Total: " & t_total
                            LabelTimerTotal.Visible = True
                        End If

                        If IsDBNull(DGVMuestras(10, fila_actual).Value) Then
                            Dim fecha_creacion As String
                            Dim fecha_creacion_datetime As DateTime
                            fecha_creacion = DGVMuestras(9, fila_actual).Value.ToString
                            fecha_creacion_datetime = Convert.ToDateTime(fecha_creacion)
                            LabelTimerAsignacion.ForeColor = Color.Red
                            LabelTimerAsignacion.Text = "Tiempo Asignacion: " & (fecha_actual_datetime - fecha_creacion_datetime).ToString()
                        Else
                            Dim fecha_asignacion As String
                            Dim fecha_asignacion_datetime As DateTime
                            Dim fecha_creacion As String
                            Dim fecha_creacion_datetime As DateTime
                            fecha_asignacion = DGVMuestras(10, fila_actual).Value.ToString()
                            fecha_asignacion_datetime = Convert.ToDateTime(fecha_asignacion)
                            fecha_creacion = DGVMuestras(9, fila_actual).Value.ToString
                            fecha_creacion_datetime = Convert.ToDateTime(fecha_creacion)
                            LabelTimerAsignacion.ForeColor = Color.Green
                            LabelTimerAsignacion.Text = "Tiempo Asignacion: " & (fecha_asignacion_datetime - fecha_creacion_datetime).ToString()
                        End If

                        If IsDBNull(DGVMuestras(11, fila_actual).Value) Then
                            Dim fecha_asignacion As String = DGVMuestras(10, fila_actual).Value.ToString()
                            Dim fecha_asignacion_datetime As DateTime = Convert.ToDateTime(fecha_asignacion)
                            Dim t_verificacion As String = (fecha_actual_datetime - fecha_asignacion_datetime).ToString()
                            LabelTimerVerificacion.ForeColor = Color.Red
                            LabelTimerVerificacion.Text = "Tiempo Verificacion: " & t_verificacion
                            LabelTimerVerificacion.Visible = True
                        ElseIf (Not IsDBNull(DGVMuestras(11, fila_actual).Value)) And IsDBNull(DGVMuestras(12, fila_actual).Value) And IsDBNull(DGVMuestras(13, fila_actual).Value) And IsDBNull(DGVMuestras(7, fila_actual).Value) Then
                            Dim fecha_verificacion_string As String = DGVMuestras(11, fila_actual).Value.ToString()
                            Dim fecha_verificacion_datetime As DateTime = Convert.ToDateTime(fecha_verificacion_string)
                            Dim fecha_asignacion As String = DGVMuestras(10, fila_actual).Value.ToString()
                            Dim fecha_asignacion_datetime As DateTime = Convert.ToDateTime(fecha_asignacion)
                            LabelTimerVerificacion.ForeColor = Color.DarkGoldenrod
                            LabelTimerVerificacion.Text = "Tiempo Verificacion: " & (fecha_verificacion_datetime - fecha_asignacion_datetime).ToString()
                            LabelTimerVerificacion.Visible = True
                        ElseIf (Not IsDBNull(DGVMuestras(11, fila_actual).Value)) And IsDBNull(DGVMuestras(12, fila_actual).Value) And IsDBNull(DGVMuestras(13, fila_actual).Value) And (DGVMuestras(7, fila_actual).Value.ToString() = "Si") Then
                            Dim fecha_verificacion_string As String = DGVMuestras(11, fila_actual).Value.ToString()
                            Dim fecha_verificacion_datetime As DateTime = Convert.ToDateTime(fecha_verificacion_string)
                            Dim fecha_asignacion As String = DGVMuestras(10, fila_actual).Value.ToString()
                            Dim fecha_asignacion_datetime As DateTime = Convert.ToDateTime(fecha_asignacion)
                            LabelTimerVerificacion.ForeColor = Color.Green
                            LabelTimerVerificacion.Text = "Tiempo Verificacion: " & (fecha_verificacion_datetime - fecha_asignacion_datetime).ToString()
                            LabelTimerVerificacion.Visible = True
                        ElseIf (Not IsDBNull(DGVMuestras(11, fila_actual).Value)) And (Not IsDBNull(DGVMuestras(12, fila_actual).Value)) And IsDBNull(DGVMuestras(13, fila_actual).Value) And (DGVMuestras(7, fila_actual).Value.ToString() = "No") Then
                            Dim fecha_verificacion1_string As String = DGVMuestras(11, fila_actual).Value.ToString()
                            Dim fecha_verificacion1_datetime As DateTime = Convert.ToDateTime(fecha_verificacion1_string)
                            Dim fecha_asignacion1 As String = DGVMuestras(10, fila_actual).Value.ToString()
                            Dim fecha_asignacion1_datetime As DateTime = Convert.ToDateTime(fecha_asignacion1)
                            Dim fecha_asignacion2 As String = DGVMuestras(12, fila_actual).Value.ToString()
                            Dim fecha_asignacion2_datetime As DateTime = Convert.ToDateTime(fecha_asignacion2)
                            LabelTimerVerificacion.ForeColor = Color.Red
                            LabelTimerVerificacion.Text = "Tiempo Verificacion: " & ((fecha_verificacion1_datetime - fecha_asignacion1_datetime) + (fecha_actual_datetime - fecha_asignacion2_datetime)).ToString()
                            LabelTimerVerificacion.Visible = True
                        ElseIf (Not IsDBNull(DGVMuestras(11, fila_actual).Value)) And (Not IsDBNull(DGVMuestras(12, fila_actual).Value)) And (Not IsDBNull(DGVMuestras(13, fila_actual).Value)) And IsDBNull(DGVMuestras(7, fila_actual).Value) Then
                            Dim fecha_verificacion1_string As String = DGVMuestras(11, fila_actual).Value.ToString()
                            Dim fecha_verificacion1_datetime As DateTime = Convert.ToDateTime(fecha_verificacion1_string)
                            Dim fecha_verificacion2 As String = DGVMuestras(13, fila_actual).Value.ToString()
                            Dim fecha_verificacion2_datetime As DateTime = Convert.ToDateTime(fecha_verificacion2)
                            Dim fecha_asignacion1 As String = DGVMuestras(10, fila_actual).Value.ToString()
                            Dim fecha_asignacion1_datetime As DateTime = Convert.ToDateTime(fecha_asignacion1)
                            Dim fecha_asignacion2 As String = DGVMuestras(12, fila_actual).Value.ToString()
                            Dim fecha_asignacion2_datetime As DateTime = Convert.ToDateTime(fecha_asignacion2)
                            LabelTimerVerificacion.ForeColor = Color.DarkGoldenrod
                            LabelTimerVerificacion.Text = "Tiempo Verificacion: " & ((fecha_verificacion1_datetime - fecha_asignacion1_datetime) + (fecha_verificacion2_datetime - fecha_asignacion2_datetime)).ToString()
                            LabelTimerVerificacion.Visible = True
                        ElseIf (Not IsDBNull(DGVMuestras(11, fila_actual).Value)) And (Not IsDBNull(DGVMuestras(12, fila_actual).Value)) And (Not IsDBNull(DGVMuestras(13, fila_actual).Value)) And (Not IsDBNull(DGVMuestras(7, fila_actual).Value)) Then
                            Dim fecha_verificacion1_string As String = DGVMuestras(11, fila_actual).Value.ToString()
                            Dim fecha_verificacion1_datetime As DateTime = Convert.ToDateTime(fecha_verificacion1_string)
                            Dim fecha_verificacion2 As String = DGVMuestras(13, fila_actual).Value.ToString()
                            Dim fecha_verificacion2_datetime As DateTime = Convert.ToDateTime(fecha_verificacion2)
                            Dim fecha_asignacion1 As String = DGVMuestras(10, fila_actual).Value.ToString()
                            Dim fecha_asignacion1_datetime As DateTime = Convert.ToDateTime(fecha_asignacion1)
                            Dim fecha_asignacion2 As String = DGVMuestras(12, fila_actual).Value.ToString()
                            Dim fecha_asignacion2_datetime As DateTime = Convert.ToDateTime(fecha_asignacion2)
                            LabelTimerVerificacion.ForeColor = Color.Green
                            LabelTimerVerificacion.Text = "Tiempo Verificacion: " & ((fecha_verificacion1_datetime - fecha_asignacion1_datetime) + (fecha_verificacion2_datetime - fecha_asignacion2_datetime)).ToString()
                            LabelTimerVerificacion.Visible = True
                        End If
                    End If
                Catch ex As Exception
                End Try
            Case 3 'Pestaña Bandejas
                Try
                    If flag1 = 1 Then
                        LabelTimerAsignacion.Visible = True
                        LabelTimerVerificacion.Visible = True
                        LabelTimerTotal.Visible = True
                        If conectado = 1 Then
                            LabelTituloFiltroRegistro.Text = "Buscar No de Bandeja"
                            LabelTituloFiltroRegistro.Visible = True
                            TxtBxFiltroRegistro.Visible = True
                            CmbBxFiltroPrueba.Visible = True
                            BtnFiltroPrueba.Visible = True
                        Else
                            LabelTituloFiltroRegistro.Visible = False
                            TxtBxFiltroRegistro.Visible = False
                            CmbBxFiltroPrueba.Visible = False
                            BtnFiltroPrueba.Visible = False
                        End If

                        Dim fila_actual As Integer = (DGVBandejas.CurrentRow.Index)

                        Dim at As Integer = DGVBandejas.RowCount
                        If fila_actual = at Then
                            Exit Sub
                        End If
                        If IsDBNull(DGVBandejas(14, fila_actual).Value) Then
                            Dim t_total As String
                            Dim fecha_creacion As String
                            Dim fecha_creacion_datetime As DateTime
                            fecha_creacion = DGVBandejas(9, fila_actual).Value.ToString
                            fecha_creacion_datetime = Convert.ToDateTime(fecha_creacion)
                            t_total = (fecha_actual_datetime - fecha_creacion_datetime).ToString
                            LabelTimerTotal.ForeColor = Color.Red
                            LabelTimerTotal.Visible = True
                            LabelTimerTotal.Text = "Tiempo Total: " & t_total
                        Else
                            Dim t_total As String
                            Dim fecha_terminado As String
                            Dim fecha_terminado_datetime As DateTime
                            Dim fecha_creacion As String
                            Dim fecha_creacion_datetime As DateTime
                            fecha_creacion = DGVBandejas(9, fila_actual).Value.ToString
                            fecha_creacion_datetime = Convert.ToDateTime(fecha_creacion)
                            fecha_terminado = DGVBandejas(14, fila_actual).Value.ToString
                            fecha_terminado_datetime = Convert.ToDateTime(fecha_terminado)
                            t_total = (fecha_terminado_datetime - fecha_creacion_datetime).ToString
                            LabelTimerTotal.ForeColor = Color.Green
                            LabelTimerTotal.Text = "Tiempo Total: " & t_total
                            LabelTimerTotal.Visible = True
                        End If

                        If IsDBNull(DGVBandejas(10, fila_actual).Value) Then
                            Dim fecha_creacion As String
                            Dim fecha_creacion_datetime As DateTime
                            fecha_creacion = DGVBandejas(9, fila_actual).Value.ToString
                            fecha_creacion_datetime = Convert.ToDateTime(fecha_creacion)
                            LabelTimerAsignacion.ForeColor = Color.Red
                            LabelTimerAsignacion.Text = "Tiempo Asignacion: " & (fecha_actual_datetime - fecha_creacion_datetime).ToString()
                        Else
                            Dim fecha_asignacion As String
                            Dim fecha_asignacion_datetime As DateTime
                            Dim fecha_creacion As String
                            Dim fecha_creacion_datetime As DateTime
                            fecha_asignacion = DGVBandejas(10, fila_actual).Value.ToString()
                            fecha_asignacion_datetime = Convert.ToDateTime(fecha_asignacion)
                            fecha_creacion = DGVBandejas(9, fila_actual).Value.ToString
                            fecha_creacion_datetime = Convert.ToDateTime(fecha_creacion)
                            LabelTimerAsignacion.ForeColor = Color.Green
                            LabelTimerAsignacion.Text = "Tiempo Asignacion: " & (fecha_asignacion_datetime - fecha_creacion_datetime).ToString()
                        End If

                        If IsDBNull(DGVBandejas(11, fila_actual).Value) Then
                            Dim fecha_asignacion As String = DGVBandejas(10, fila_actual).Value.ToString()
                            Dim fecha_asignacion_datetime As DateTime = Convert.ToDateTime(fecha_asignacion)
                            Dim t_verificacion As String = (fecha_actual_datetime - fecha_asignacion_datetime).ToString()
                            LabelTimerVerificacion.ForeColor = Color.Red
                            LabelTimerVerificacion.Text = "Tiempo Verificacion: " & t_verificacion
                            LabelTimerVerificacion.Visible = True
                        ElseIf (Not IsDBNull(DGVBandejas(11, fila_actual).Value)) And IsDBNull(DGVBandejas(12, fila_actual).Value) And IsDBNull(DGVBandejas(13, fila_actual).Value) And IsDBNull(DGVBandejas(7, fila_actual).Value) Then
                            Dim fecha_verificacion_string As String = DGVBandejas(11, fila_actual).Value.ToString()
                            Dim fecha_verificacion_datetime As DateTime = Convert.ToDateTime(fecha_verificacion_string)
                            Dim fecha_asignacion As String = DGVBandejas(10, fila_actual).Value.ToString()
                            Dim fecha_asignacion_datetime As DateTime = Convert.ToDateTime(fecha_asignacion)
                            LabelTimerVerificacion.ForeColor = Color.DarkGoldenrod
                            LabelTimerVerificacion.Text = "Tiempo Verificacion: " & (fecha_verificacion_datetime - fecha_asignacion_datetime).ToString()
                            LabelTimerVerificacion.Visible = True
                        ElseIf (Not IsDBNull(DGVBandejas(11, fila_actual).Value)) And IsDBNull(DGVBandejas(12, fila_actual).Value) And IsDBNull(DGVBandejas(13, fila_actual).Value) And (DGVBandejas(7, fila_actual).Value.ToString() = "Si") Then
                            Dim fecha_verificacion_string As String = DGVBandejas(11, fila_actual).Value.ToString()
                            Dim fecha_verificacion_datetime As DateTime = Convert.ToDateTime(fecha_verificacion_string)
                            Dim fecha_asignacion As String = DGVBandejas(10, fila_actual).Value.ToString()
                            Dim fecha_asignacion_datetime As DateTime = Convert.ToDateTime(fecha_asignacion)
                            LabelTimerVerificacion.ForeColor = Color.Green
                            LabelTimerVerificacion.Text = "Tiempo Verificacion: " & (fecha_verificacion_datetime - fecha_asignacion_datetime).ToString()
                            LabelTimerVerificacion.Visible = True
                        ElseIf (Not IsDBNull(DGVBandejas(11, fila_actual).Value)) And (Not IsDBNull(DGVBandejas(12, fila_actual).Value)) And IsDBNull(DGVBandejas(13, fila_actual).Value) And (DGVBandejas(7, fila_actual).Value.ToString() = "No") Then
                            Dim fecha_verificacion1_string As String = DGVBandejas(11, fila_actual).Value.ToString()
                            Dim fecha_verificacion1_datetime As DateTime = Convert.ToDateTime(fecha_verificacion1_string)
                            Dim fecha_asignacion1 As String = DGVBandejas(10, fila_actual).Value.ToString()
                            Dim fecha_asignacion1_datetime As DateTime = Convert.ToDateTime(fecha_asignacion1)
                            Dim fecha_asignacion2 As String = DGVBandejas(12, fila_actual).Value.ToString()
                            Dim fecha_asignacion2_datetime As DateTime = Convert.ToDateTime(fecha_asignacion2)
                            LabelTimerVerificacion.ForeColor = Color.Red
                            LabelTimerVerificacion.Text = "Tiempo Verificacion: " & ((fecha_verificacion1_datetime - fecha_asignacion1_datetime) + (fecha_actual_datetime - fecha_asignacion2_datetime)).ToString()
                            LabelTimerVerificacion.Visible = True
                        ElseIf (Not IsDBNull(DGVBandejas(11, fila_actual).Value)) And (Not IsDBNull(DGVBandejas(12, fila_actual).Value)) And (Not IsDBNull(DGVBandejas(13, fila_actual).Value)) And IsDBNull(DGVBandejas(7, fila_actual).Value) Then
                            Dim fecha_verificacion1_string As String = DGVBandejas(11, fila_actual).Value.ToString()
                            Dim fecha_verificacion1_datetime As DateTime = Convert.ToDateTime(fecha_verificacion1_string)
                            Dim fecha_verificacion2 As String = DGVBandejas(13, fila_actual).Value.ToString()
                            Dim fecha_verificacion2_datetime As DateTime = Convert.ToDateTime(fecha_verificacion2)
                            Dim fecha_asignacion1 As String = DGVBandejas(10, fila_actual).Value.ToString()
                            Dim fecha_asignacion1_datetime As DateTime = Convert.ToDateTime(fecha_asignacion1)
                            Dim fecha_asignacion2 As String = DGVBandejas(12, fila_actual).Value.ToString()
                            Dim fecha_asignacion2_datetime As DateTime = Convert.ToDateTime(fecha_asignacion2)
                            LabelTimerVerificacion.ForeColor = Color.DarkGoldenrod
                            LabelTimerVerificacion.Text = "Tiempo Verificacion: " & ((fecha_verificacion1_datetime - fecha_asignacion1_datetime) + (fecha_verificacion2_datetime - fecha_asignacion2_datetime)).ToString()
                            LabelTimerVerificacion.Visible = True
                        ElseIf (Not IsDBNull(DGVBandejas(11, fila_actual).Value)) And (Not IsDBNull(DGVBandejas(12, fila_actual).Value)) And (Not IsDBNull(DGVBandejas(13, fila_actual).Value)) And (Not IsDBNull(DGVBandejas(7, fila_actual).Value)) Then
                            Dim fecha_verificacion1_string As String = DGVBandejas(11, fila_actual).Value.ToString()
                            Dim fecha_verificacion1_datetime As DateTime = Convert.ToDateTime(fecha_verificacion1_string)
                            Dim fecha_verificacion2 As String = DGVBandejas(13, fila_actual).Value.ToString()
                            Dim fecha_verificacion2_datetime As DateTime = Convert.ToDateTime(fecha_verificacion2)
                            Dim fecha_asignacion1 As String = DGVBandejas(10, fila_actual).Value.ToString()
                            Dim fecha_asignacion1_datetime As DateTime = Convert.ToDateTime(fecha_asignacion1)
                            Dim fecha_asignacion2 As String = DGVBandejas(12, fila_actual).Value.ToString()
                            Dim fecha_asignacion2_datetime As DateTime = Convert.ToDateTime(fecha_asignacion2)
                            LabelTimerVerificacion.ForeColor = Color.Green
                            LabelTimerVerificacion.Text = "Tiempo Verificacion: " & ((fecha_verificacion1_datetime - fecha_asignacion1_datetime) + (fecha_verificacion2_datetime - fecha_asignacion2_datetime)).ToString()
                            LabelTimerVerificacion.Visible = True
                        End If
                    End If
                Catch ex As Exception
                End Try
        End Select


    End Sub

    'Logica para la Impresion

    Private Structure pageDetails
        Dim columns As Integer
        Dim rows As Integer
        Dim startCol As Integer
        Dim startRow As Integer
    End Structure

    Private pages As Dictionary(Of Integer, pageDetails)

    Dim maxPagesWide As Integer
    Dim maxPagesTall As Integer

    Private Sub btnPreviewPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPreviewPrint.Click
        Try
            Dim pod As New PrintDialog
            pod.Document = PrintDocument1
            pod.ShowDialog()
            Dim ppd As New PrintPreviewDialog
            ppd.WindowState = FormWindowState.Maximized
            ppd.Document = PrintDocument1
            ppd.ShowDialog()
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub

    Private Sub PrintDocument1_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument1.BeginPrint
        Select Case Pest_actual
            Case 1
                PrintDocument1.OriginAtMargins = True
                PrintDocument1.DefaultPageSettings.Margins = New Drawing.Printing.Margins(0, 0, 0, 0)

                pages = New Dictionary(Of Integer, pageDetails)
                Dim maxHeight As Integer = CInt(PrintDocument1.DefaultPageSettings.Bounds.Height)
                Dim maxWidth As Integer = CInt(PrintDocument1.DefaultPageSettings.Bounds.Width)
                'Dim maxWidth As Integer = CInt(PrintDocument1.DefaultPageSettings.PrintableArea.Width) - 40
                'Dim maxHeight As Integer = CInt(PrintDocument1.DefaultPageSettings.PrintableArea.Height) - 40 + Label1.Height

                Dim pageCounter As Integer = 0
                pages.Add(pageCounter, New pageDetails)

                Dim columnCounter As Integer = 0

                Dim columnSum As Integer = DGVAdmin.RowHeadersWidth

                Dim tabla_actual = LabelAdmin.Text

                If tabla_actual = "Analistas" Then
                    For c As Integer = 1 To DGVAdmin.Columns.Count - 1
                        If columnSum + DGVAdmin.Columns(c).Width < maxWidth Then
                            columnSum += DGVAdmin.Columns(c).Width
                            columnCounter += 1
                        Else
                            pages(pageCounter) = New pageDetails With {.columns = columnCounter, .rows = 0, .startCol = pages(pageCounter).startCol}
                            columnSum = DGVAdmin.RowHeadersWidth + DGVAdmin.Columns(c).Width
                            columnCounter = 1
                            pageCounter += 1
                            pages.Add(pageCounter, New pageDetails With {.startCol = c})
                        End If
                        If c = DGVAdmin.Columns.Count - 1 Then
                            If pages(pageCounter).columns = 0 Then
                                pages(pageCounter) = New pageDetails With {.columns = columnCounter, .rows = 0, .startCol = pages(pageCounter).startCol}
                            End If
                        End If
                    Next

                    maxPagesWide = pages.Keys.Max + 1

                    pageCounter = 0

                    Dim rowCounter As Integer = 0

                    Dim rowSum As Integer = DGVAdmin.ColumnHeadersHeight

                    For r As Integer = 0 To DGVAdmin.Rows.Count - 2
                        If rowSum + DGVAdmin.Rows(r).Height < maxHeight Then
                            rowSum += DGVAdmin.Rows(r).Height
                            rowCounter += 1
                        Else
                            pages(pageCounter) = New pageDetails With {.columns = pages(pageCounter).columns, .rows = rowCounter, .startCol = pages(pageCounter).startCol, .startRow = pages(pageCounter).startRow}
                            For x As Integer = 1 To maxPagesWide - 1
                                pages(pageCounter + x) = New pageDetails With {.columns = pages(pageCounter + x).columns, .rows = rowCounter, .startCol = pages(pageCounter + x).startCol, .startRow = pages(pageCounter).startRow}
                            Next

                            pageCounter += maxPagesWide
                            For x As Integer = 0 To maxPagesWide - 1
                                pages.Add(pageCounter + x, New pageDetails With {.columns = pages(x).columns, .rows = 0, .startCol = pages(x).startCol, .startRow = r})
                            Next

                            rowSum = DGVAdmin.ColumnHeadersHeight + DGVAdmin.Rows(r).Height
                            rowCounter = 1
                        End If
                        If r = DGVAdmin.Rows.Count - 2 Then
                            For x As Integer = 0 To maxPagesWide - 1
                                If pages(pageCounter + x).rows = 0 Then
                                    pages(pageCounter + x) = New pageDetails With {.columns = pages(pageCounter + x).columns, .rows = rowCounter, .startCol = pages(pageCounter + x).startCol, .startRow = pages(pageCounter + x).startRow}
                                End If
                            Next
                        End If
                    Next

                    maxPagesTall = pages.Count \ maxPagesWide

                ElseIf tabla_actual = "Pruebas" Then
                    For c As Integer = 1 To DGVAdmin.Columns.Count - 1
                        If columnSum + DGVAdmin.Columns(c).Width < maxWidth Then
                            columnSum += DGVAdmin.Columns(c).Width
                            columnCounter += 1
                        Else
                            pages(pageCounter) = New pageDetails With {.columns = columnCounter, .rows = 0, .startCol = pages(pageCounter).startCol}
                            columnSum = DGVAdmin.RowHeadersWidth + DGVAdmin.Columns(c).Width
                            columnCounter = 1
                            pageCounter += 1
                            pages.Add(pageCounter, New pageDetails With {.startCol = c})
                        End If
                        If c = DGVAdmin.Columns.Count - 1 Then
                            If pages(pageCounter).columns = 0 Then
                                pages(pageCounter) = New pageDetails With {.columns = columnCounter, .rows = 0, .startCol = pages(pageCounter).startCol}
                            End If
                        End If
                    Next
                    maxPagesWide = pages.Keys.Max + 1

                    pageCounter = 0

                    Dim rowCounter As Integer = 0

                    Dim rowSum As Integer = DGVAdmin.ColumnHeadersHeight

                    For r As Integer = 0 To DGVAdmin.Rows.Count - 2
                        If rowSum + DGVAdmin.Rows(r).Height < maxHeight Then
                            rowSum += DGVAdmin.Rows(r).Height
                            rowCounter += 1
                        Else
                            pages(pageCounter) = New pageDetails With {.columns = pages(pageCounter).columns, .rows = rowCounter, .startCol = pages(pageCounter).startCol, .startRow = pages(pageCounter).startRow}
                            For x As Integer = 1 To maxPagesWide - 1
                                pages(pageCounter + x) = New pageDetails With {.columns = pages(pageCounter + x).columns, .rows = rowCounter, .startCol = pages(pageCounter + x).startCol, .startRow = pages(pageCounter).startRow}
                            Next

                            pageCounter += maxPagesWide
                            For x As Integer = 0 To maxPagesWide - 1
                                pages.Add(pageCounter + x, New pageDetails With {.columns = pages(x).columns, .rows = 0, .startCol = pages(x).startCol, .startRow = r})
                            Next

                            rowSum = DGVAdmin.ColumnHeadersHeight + DGVAdmin.Rows(r).Height
                            rowCounter = 1
                        End If
                        If r = DGVAdmin.Rows.Count - 2 Then
                            For x As Integer = 0 To maxPagesWide - 1
                                If pages(pageCounter + x).rows = 0 Then
                                    pages(pageCounter + x) = New pageDetails With {.columns = pages(pageCounter + x).columns, .rows = rowCounter, .startCol = pages(pageCounter + x).startCol, .startRow = pages(pageCounter + x).startRow}
                                End If
                            Next
                        End If
                    Next

                    maxPagesTall = pages.Count \ maxPagesWide
                ElseIf tabla_actual = "Relacion Analistas Pruebas" Then
                    For c As Integer = 0 To DGVAdmin.Columns.Count - 1
                        If columnSum + DGVAdmin.Columns(c).Width < maxWidth Then
                            columnSum += DGVAdmin.Columns(c).Width
                            columnCounter += 1
                        Else
                            pages(pageCounter) = New pageDetails With {.columns = columnCounter, .rows = 0, .startCol = pages(pageCounter).startCol}
                            columnSum = DGVAdmin.RowHeadersWidth + DGVAdmin.Columns(c).Width
                            columnCounter = 1
                            pageCounter += 1
                            pages.Add(pageCounter, New pageDetails With {.startCol = c})
                        End If
                        If c = DGVAdmin.Columns.Count - 1 Then
                            If pages(pageCounter).columns = 0 Then
                                pages(pageCounter) = New pageDetails With {.columns = columnCounter, .rows = 0, .startCol = pages(pageCounter).startCol}
                            End If
                        End If
                    Next
                    maxPagesWide = pages.Keys.Max + 1

                    pageCounter = 0

                    Dim rowCounter As Integer = 0

                    Dim rowSum As Integer = DGVAdmin.ColumnHeadersHeight

                    For r As Integer = 0 To DGVAdmin.Rows.Count - 2
                        If rowSum + DGVAdmin.Rows(r).Height < maxHeight Then
                            rowSum += DGVAdmin.Rows(r).Height
                            rowCounter += 1
                        Else
                            pages(pageCounter) = New pageDetails With {.columns = pages(pageCounter).columns, .rows = rowCounter, .startCol = pages(pageCounter).startCol, .startRow = pages(pageCounter).startRow}
                            For x As Integer = 1 To maxPagesWide - 1
                                pages(pageCounter + x) = New pageDetails With {.columns = pages(pageCounter + x).columns, .rows = rowCounter, .startCol = pages(pageCounter + x).startCol, .startRow = pages(pageCounter).startRow}
                            Next

                            pageCounter += maxPagesWide
                            For x As Integer = 0 To maxPagesWide - 1
                                pages.Add(pageCounter + x, New pageDetails With {.columns = pages(x).columns, .rows = 0, .startCol = pages(x).startCol, .startRow = r})
                            Next

                            rowSum = DGVAdmin.ColumnHeadersHeight + DGVAdmin.Rows(r).Height
                            rowCounter = 1
                        End If
                        If r = DGVAdmin.Rows.Count - 2 Then
                            For x As Integer = 0 To maxPagesWide - 1
                                If pages(pageCounter + x).rows = 0 Then
                                    pages(pageCounter + x) = New pageDetails With {.columns = pages(pageCounter + x).columns, .rows = rowCounter, .startCol = pages(pageCounter + x).startCol, .startRow = pages(pageCounter + x).startRow}
                                End If
                            Next
                        End If
                    Next

                    maxPagesTall = pages.Count \ maxPagesWide
                ElseIf tabla_actual = "Muestras Asignadas" Then
                    For c As Integer = 0 To 8
                        If columnSum + DGVAdmin.Columns(c).Width < maxWidth Then
                            columnSum += DGVAdmin.Columns(c).Width
                            columnCounter += 1
                        Else
                            pages(pageCounter) = New pageDetails With {.columns = columnCounter, .rows = 0, .startCol = pages(pageCounter).startCol}
                            columnSum = DGVAdmin.RowHeadersWidth + DGVAdmin.Columns(c).Width
                            columnCounter = 1
                            pageCounter += 1
                            pages.Add(pageCounter, New pageDetails With {.startCol = c})
                        End If
                        If c = 8 Then
                            If pages(pageCounter).columns = 0 Then
                                pages(pageCounter) = New pageDetails With {.columns = columnCounter, .rows = 0, .startCol = pages(pageCounter).startCol}
                            End If
                        End If
                    Next
                    maxPagesWide = pages.Keys.Max + 1

                    pageCounter = 0

                    Dim rowCounter As Integer = 0

                    Dim rowSum As Integer = DGVAdmin.ColumnHeadersHeight

                    For r As Integer = 0 To DGVAdmin.Rows.Count - 2
                        If rowSum + DGVAdmin.Rows(r).Height < maxHeight Then
                            rowSum += DGVAdmin.Rows(r).Height
                            rowCounter += 1
                        Else
                            pages(pageCounter) = New pageDetails With {.columns = pages(pageCounter).columns, .rows = rowCounter, .startCol = pages(pageCounter).startCol, .startRow = pages(pageCounter).startRow}
                            For x As Integer = 1 To maxPagesWide - 1
                                pages(pageCounter + x) = New pageDetails With {.columns = pages(pageCounter + x).columns, .rows = rowCounter, .startCol = pages(pageCounter + x).startCol, .startRow = pages(pageCounter).startRow}
                            Next

                            pageCounter += maxPagesWide
                            For x As Integer = 0 To maxPagesWide - 1
                                pages.Add(pageCounter + x, New pageDetails With {.columns = pages(x).columns, .rows = 0, .startCol = pages(x).startCol, .startRow = r})
                            Next

                            rowSum = DGVAdmin.ColumnHeadersHeight + DGVAdmin.Rows(r).Height
                            rowCounter = 1
                        End If
                        If r = DGVAdmin.Rows.Count - 2 Then
                            For x As Integer = 0 To maxPagesWide - 1
                                If pages(pageCounter + x).rows = 0 Then
                                    pages(pageCounter + x) = New pageDetails With {.columns = pages(pageCounter + x).columns, .rows = rowCounter, .startCol = pages(pageCounter + x).startCol, .startRow = pages(pageCounter + x).startRow}
                                End If
                            Next
                        End If
                    Next

                    maxPagesTall = pages.Count \ maxPagesWide
                ElseIf tabla_actual = "Muestras no Asignadas" Then
                    For c As Integer = 0 To 7
                        If columnSum + DGVAdmin.Columns(c).Width < maxWidth Then
                            columnSum += DGVAdmin.Columns(c).Width
                            columnCounter += 1
                        Else
                            pages(pageCounter) = New pageDetails With {.columns = columnCounter, .rows = 0, .startCol = pages(pageCounter).startCol}
                            columnSum = DGVAdmin.RowHeadersWidth + DGVAdmin.Columns(c).Width
                            columnCounter = 1
                            pageCounter += 1
                            pages.Add(pageCounter, New pageDetails With {.startCol = c})
                        End If
                        If c = 7 Then
                            If pages(pageCounter).columns = 0 Then
                                pages(pageCounter) = New pageDetails With {.columns = columnCounter, .rows = 0, .startCol = pages(pageCounter).startCol}
                            End If
                        End If
                    Next
                    maxPagesWide = pages.Keys.Max + 1

                    pageCounter = 0

                    Dim rowCounter As Integer = 0

                    Dim rowSum As Integer = DGVAdmin.ColumnHeadersHeight

                    For r As Integer = 0 To DGVAdmin.Rows.Count - 2
                        If rowSum + DGVAdmin.Rows(r).Height < maxHeight Then
                            rowSum += DGVAdmin.Rows(r).Height
                            rowCounter += 1
                        Else
                            pages(pageCounter) = New pageDetails With {.columns = pages(pageCounter).columns, .rows = rowCounter, .startCol = pages(pageCounter).startCol, .startRow = pages(pageCounter).startRow}
                            For x As Integer = 1 To maxPagesWide - 1
                                pages(pageCounter + x) = New pageDetails With {.columns = pages(pageCounter + x).columns, .rows = rowCounter, .startCol = pages(pageCounter + x).startCol, .startRow = pages(pageCounter).startRow}
                            Next

                            pageCounter += maxPagesWide
                            For x As Integer = 0 To maxPagesWide - 1
                                pages.Add(pageCounter + x, New pageDetails With {.columns = pages(x).columns, .rows = 0, .startCol = pages(x).startCol, .startRow = r})
                            Next

                            rowSum = DGVAdmin.ColumnHeadersHeight + DGVAdmin.Rows(r).Height
                            rowCounter = 1
                        End If
                        If r = DGVAdmin.Rows.Count - 2 Then
                            For x As Integer = 0 To maxPagesWide - 1
                                If pages(pageCounter + x).rows = 0 Then
                                    pages(pageCounter + x) = New pageDetails With {.columns = pages(pageCounter + x).columns, .rows = rowCounter, .startCol = pages(pageCounter + x).startCol, .startRow = pages(pageCounter + x).startRow}
                                End If
                            Next
                        End If
                    Next

                    maxPagesTall = pages.Count \ maxPagesWide
                ElseIf tabla_actual = "Bandejas Asignadas" Then
                    For c As Integer = 0 To 8
                        If columnSum + DGVAdmin.Columns(c).Width < maxWidth Then
                            columnSum += DGVAdmin.Columns(c).Width
                            columnCounter += 1
                        Else
                            pages(pageCounter) = New pageDetails With {.columns = columnCounter, .rows = 0, .startCol = pages(pageCounter).startCol}
                            columnSum = DGVAdmin.RowHeadersWidth + DGVAdmin.Columns(c).Width
                            columnCounter = 1
                            pageCounter += 1
                            pages.Add(pageCounter, New pageDetails With {.startCol = c})
                        End If
                        If c = 8 Then
                            If pages(pageCounter).columns = 0 Then
                                pages(pageCounter) = New pageDetails With {.columns = columnCounter, .rows = 0, .startCol = pages(pageCounter).startCol}
                            End If
                        End If
                    Next
                    maxPagesWide = pages.Keys.Max + 1

                    pageCounter = 0

                    Dim rowCounter As Integer = 0

                    Dim rowSum As Integer = DGVAdmin.ColumnHeadersHeight

                    For r As Integer = 0 To DGVAdmin.Rows.Count - 2
                        If rowSum + DGVAdmin.Rows(r).Height < maxHeight Then
                            rowSum += DGVAdmin.Rows(r).Height
                            rowCounter += 1
                        Else
                            pages(pageCounter) = New pageDetails With {.columns = pages(pageCounter).columns, .rows = rowCounter, .startCol = pages(pageCounter).startCol, .startRow = pages(pageCounter).startRow}
                            For x As Integer = 1 To maxPagesWide - 1
                                pages(pageCounter + x) = New pageDetails With {.columns = pages(pageCounter + x).columns, .rows = rowCounter, .startCol = pages(pageCounter + x).startCol, .startRow = pages(pageCounter).startRow}
                            Next

                            pageCounter += maxPagesWide
                            For x As Integer = 0 To maxPagesWide - 1
                                pages.Add(pageCounter + x, New pageDetails With {.columns = pages(x).columns, .rows = 0, .startCol = pages(x).startCol, .startRow = r})
                            Next

                            rowSum = DGVAdmin.ColumnHeadersHeight + DGVAdmin.Rows(r).Height
                            rowCounter = 1
                        End If
                        If r = DGVAdmin.Rows.Count - 2 Then
                            For x As Integer = 0 To maxPagesWide - 1
                                If pages(pageCounter + x).rows = 0 Then
                                    pages(pageCounter + x) = New pageDetails With {.columns = pages(pageCounter + x).columns, .rows = rowCounter, .startCol = pages(pageCounter + x).startCol, .startRow = pages(pageCounter + x).startRow}
                                End If
                            Next
                        End If
                    Next

                    maxPagesTall = pages.Count \ maxPagesWide
                ElseIf tabla_actual = "Bandejas no Asignadas" Then
                    For c As Integer = 0 To 7
                        If columnSum + DGVAdmin.Columns(c).Width < maxWidth Then
                            columnSum += DGVAdmin.Columns(c).Width
                            columnCounter += 1
                        Else
                            pages(pageCounter) = New pageDetails With {.columns = columnCounter, .rows = 0, .startCol = pages(pageCounter).startCol}
                            columnSum = DGVAdmin.RowHeadersWidth + DGVAdmin.Columns(c).Width
                            columnCounter = 1
                            pageCounter += 1
                            pages.Add(pageCounter, New pageDetails With {.startCol = c})
                        End If
                        If c = 7 Then
                            If pages(pageCounter).columns = 0 Then
                                pages(pageCounter) = New pageDetails With {.columns = columnCounter, .rows = 0, .startCol = pages(pageCounter).startCol}
                            End If
                        End If
                    Next
                    maxPagesWide = pages.Keys.Max + 1

                    pageCounter = 0

                    Dim rowCounter As Integer = 0

                    Dim rowSum As Integer = DGVAdmin.ColumnHeadersHeight

                    For r As Integer = 0 To DGVAdmin.Rows.Count - 2
                        If rowSum + DGVAdmin.Rows(r).Height < maxHeight Then
                            rowSum += DGVAdmin.Rows(r).Height
                            rowCounter += 1
                        Else
                            pages(pageCounter) = New pageDetails With {.columns = pages(pageCounter).columns, .rows = rowCounter, .startCol = pages(pageCounter).startCol, .startRow = pages(pageCounter).startRow}
                            For x As Integer = 1 To maxPagesWide - 1
                                pages(pageCounter + x) = New pageDetails With {.columns = pages(pageCounter + x).columns, .rows = rowCounter, .startCol = pages(pageCounter + x).startCol, .startRow = pages(pageCounter).startRow}
                            Next

                            pageCounter += maxPagesWide
                            For x As Integer = 0 To maxPagesWide - 1
                                pages.Add(pageCounter + x, New pageDetails With {.columns = pages(x).columns, .rows = 0, .startCol = pages(x).startCol, .startRow = r})
                            Next

                            rowSum = DGVAdmin.ColumnHeadersHeight + DGVAdmin.Rows(r).Height
                            rowCounter = 1
                        End If
                        If r = DGVAdmin.Rows.Count - 2 Then
                            For x As Integer = 0 To maxPagesWide - 1
                                If pages(pageCounter + x).rows = 0 Then
                                    pages(pageCounter + x) = New pageDetails With {.columns = pages(pageCounter + x).columns, .rows = rowCounter, .startCol = pages(pageCounter + x).startCol, .startRow = pages(pageCounter + x).startRow}
                                End If
                            Next
                        End If
                    Next

                    maxPagesTall = pages.Count \ maxPagesWide
                ElseIf tabla_actual = "Historial de Muestras" Then

                    For c As Integer = 0 To 8
                        If columnSum + DGVAdmin.Columns(c).Width < maxWidth Then
                            columnSum += DGVAdmin.Columns(c).Width
                            columnCounter += 1
                        Else
                            pages(pageCounter) = New pageDetails With {.columns = columnCounter, .rows = 0, .startCol = pages(pageCounter).startCol}
                            columnSum = DGVAdmin.RowHeadersWidth + DGVAdmin.Columns(c).Width
                            columnCounter = 1
                            pageCounter += 1
                            pages.Add(pageCounter, New pageDetails With {.startCol = c})
                        End If
                        If c = 8 Then
                            If pages(pageCounter).columns = 0 Then
                                pages(pageCounter) = New pageDetails With {.columns = columnCounter, .rows = 0, .startCol = pages(pageCounter).startCol}
                            End If
                        End If
                    Next
                    maxPagesWide = pages.Keys.Max + 1

                    pageCounter = 0

                    Dim rowCounter As Integer = 0

                    Dim rowSum As Integer = DGVAdmin.ColumnHeadersHeight

                    For r As Integer = 0 To DGVAdmin.Rows.Count - 2
                        If rowSum + DGVAdmin.Rows(r).Height < maxHeight Then
                            rowSum += DGVAdmin.Rows(r).Height
                            rowCounter += 1
                        Else
                            pages(pageCounter) = New pageDetails With {.columns = pages(pageCounter).columns, .rows = rowCounter, .startCol = pages(pageCounter).startCol, .startRow = pages(pageCounter).startRow}
                            For x As Integer = 1 To maxPagesWide - 1
                                pages(pageCounter + x) = New pageDetails With {.columns = pages(pageCounter + x).columns, .rows = rowCounter, .startCol = pages(pageCounter + x).startCol, .startRow = pages(pageCounter).startRow}
                            Next

                            pageCounter += maxPagesWide
                            For x As Integer = 0 To maxPagesWide - 1
                                pages.Add(pageCounter + x, New pageDetails With {.columns = pages(x).columns, .rows = 0, .startCol = pages(x).startCol, .startRow = r})
                            Next

                            rowSum = DGVAdmin.ColumnHeadersHeight + DGVAdmin.Rows(r).Height
                            rowCounter = 1
                        End If
                        If r = DGVAdmin.Rows.Count - 2 Then
                            For x As Integer = 0 To maxPagesWide - 1
                                If pages(pageCounter + x).rows = 0 Then
                                    pages(pageCounter + x) = New pageDetails With {.columns = pages(pageCounter + x).columns, .rows = rowCounter, .startCol = pages(pageCounter + x).startCol, .startRow = pages(pageCounter + x).startRow}
                                End If
                            Next
                        End If
                    Next

                    maxPagesTall = pages.Count \ maxPagesWide
                ElseIf tabla_actual = "Historial de Bandejas" Then

                    For c As Integer = 0 To 8
                        If columnSum + DGVAdmin.Columns(c).Width < maxWidth Then
                            columnSum += DGVAdmin.Columns(c).Width
                            columnCounter += 1
                        Else
                            pages(pageCounter) = New pageDetails With {.columns = columnCounter, .rows = 0, .startCol = pages(pageCounter).startCol}
                            columnSum = DGVAdmin.RowHeadersWidth + DGVAdmin.Columns(c).Width
                            columnCounter = 1
                            pageCounter += 1
                            pages.Add(pageCounter, New pageDetails With {.startCol = c})
                        End If
                        If c = 8 Then
                            If pages(pageCounter).columns = 0 Then
                                pages(pageCounter) = New pageDetails With {.columns = columnCounter, .rows = 0, .startCol = pages(pageCounter).startCol}
                            End If
                        End If
                    Next
                    maxPagesWide = pages.Keys.Max + 1

                    pageCounter = 0

                    Dim rowCounter As Integer = 0

                    Dim rowSum As Integer = DGVAdmin.ColumnHeadersHeight

                    For r As Integer = 0 To DGVAdmin.Rows.Count - 2
                        If rowSum + DGVAdmin.Rows(r).Height < maxHeight Then
                            rowSum += DGVAdmin.Rows(r).Height
                            rowCounter += 1
                        Else
                            pages(pageCounter) = New pageDetails With {.columns = pages(pageCounter).columns, .rows = rowCounter, .startCol = pages(pageCounter).startCol, .startRow = pages(pageCounter).startRow}
                            For x As Integer = 1 To maxPagesWide - 1
                                pages(pageCounter + x) = New pageDetails With {.columns = pages(pageCounter + x).columns, .rows = rowCounter, .startCol = pages(pageCounter + x).startCol, .startRow = pages(pageCounter).startRow}
                            Next

                            pageCounter += maxPagesWide
                            For x As Integer = 0 To maxPagesWide - 1
                                pages.Add(pageCounter + x, New pageDetails With {.columns = pages(x).columns, .rows = 0, .startCol = pages(x).startCol, .startRow = r})
                            Next

                            rowSum = DGVAdmin.ColumnHeadersHeight + DGVAdmin.Rows(r).Height
                            rowCounter = 1
                        End If
                        If r = DGVAdmin.Rows.Count - 2 Then
                            For x As Integer = 0 To maxPagesWide - 1
                                If pages(pageCounter + x).rows = 0 Then
                                    pages(pageCounter + x) = New pageDetails With {.columns = pages(pageCounter + x).columns, .rows = rowCounter, .startCol = pages(pageCounter + x).startCol, .startRow = pages(pageCounter + x).startRow}
                                End If
                            Next
                        End If
                    Next

                    maxPagesTall = pages.Count \ maxPagesWide
                End If


                Exit Sub

            Case 2
                PrintDocument1.OriginAtMargins = True
                PrintDocument1.DefaultPageSettings.Margins = New Drawing.Printing.Margins(0, 0, 0, 0)

                pages = New Dictionary(Of Integer, pageDetails)

                Dim maxWidth As Integer = CInt(PrintDocument1.DefaultPageSettings.PrintableArea.Width) - 40
                Dim maxHeight As Integer = CInt(PrintDocument1.DefaultPageSettings.PrintableArea.Height) - 40 + Label1.Height

                Dim pageCounter As Integer = 0
                pages.Add(pageCounter, New pageDetails)

                Dim columnCounter As Integer = 0

                Dim columnSum As Integer = DGVMuestras.RowHeadersWidth

                For c As Integer = 0 To 3
                    If columnSum + DGVMuestras.Columns(c).Width < maxWidth Then
                        columnSum += DGVMuestras.Columns(c).Width
                        columnCounter += 1
                    Else
                        pages(pageCounter) = New pageDetails With {.columns = columnCounter, .rows = 0, .startCol = pages(pageCounter).startCol}
                        columnSum = DGVMuestras.RowHeadersWidth + DGVMuestras.Columns(c).Width
                        columnCounter = 1
                        pageCounter += 1
                        pages.Add(pageCounter, New pageDetails With {.startCol = c})
                    End If
                    If c = 3 Then
                        If pages(pageCounter).columns = 0 Then
                            pages(pageCounter) = New pageDetails With {.columns = columnCounter, .rows = 0, .startCol = pages(pageCounter).startCol}
                        End If
                    End If
                Next
                maxPagesWide = pages.Keys.Max + 1

                pageCounter = 0

                Dim rowCounter As Integer = 0

                Dim rowSum As Integer = DGVMuestras.ColumnHeadersHeight

                For r As Integer = 0 To DGVMuestras.Rows.Count - 2
                    If rowSum + DGVMuestras.Rows(r).Height < maxHeight Then
                        rowSum += DGVMuestras.Rows(r).Height
                        rowCounter += 1
                    Else
                        pages(pageCounter) = New pageDetails With {.columns = pages(pageCounter).columns, .rows = rowCounter, .startCol = pages(pageCounter).startCol, .startRow = pages(pageCounter).startRow}
                        For x As Integer = 1 To maxPagesWide - 1
                            pages(pageCounter + x) = New pageDetails With {.columns = pages(pageCounter + x).columns, .rows = rowCounter, .startCol = pages(pageCounter + x).startCol, .startRow = pages(pageCounter).startRow}
                        Next

                        pageCounter += maxPagesWide
                        For x As Integer = 0 To maxPagesWide - 1
                            pages.Add(pageCounter + x, New pageDetails With {.columns = pages(x).columns, .rows = 0, .startCol = pages(x).startCol, .startRow = r})
                        Next

                        rowSum = DGVMuestras.ColumnHeadersHeight + DGVMuestras.Rows(r).Height
                        rowCounter = 1
                    End If
                    If r = DGVMuestras.Rows.Count - 2 Then
                        For x As Integer = 0 To maxPagesWide - 1
                            If pages(pageCounter + x).rows = 0 Then
                                pages(pageCounter + x) = New pageDetails With {.columns = pages(pageCounter + x).columns, .rows = rowCounter, .startCol = pages(pageCounter + x).startCol, .startRow = pages(pageCounter + x).startRow}
                            End If
                        Next
                    End If
                Next

                maxPagesTall = pages.Count \ maxPagesWide
                Exit Sub

            Case 3
                PrintDocument1.OriginAtMargins = True
                PrintDocument1.DefaultPageSettings.Margins = New Drawing.Printing.Margins(0, 0, 0, 0)

                pages = New Dictionary(Of Integer, pageDetails)

                Dim maxWidth As Integer = CInt(PrintDocument1.DefaultPageSettings.PrintableArea.Width) - 40
                Dim maxHeight As Integer = CInt(PrintDocument1.DefaultPageSettings.PrintableArea.Height) - 40 + Label1.Height

                Dim pageCounter As Integer = 0
                pages.Add(pageCounter, New pageDetails)

                Dim columnCounter As Integer = 0

                Dim columnSum As Integer = DGVBandejas.RowHeadersWidth

                For c As Integer = 0 To 3
                    If columnSum + DGVBandejas.Columns(c).Width < maxWidth Then
                        columnSum += DGVBandejas.Columns(c).Width
                        columnCounter += 1
                    Else
                        pages(pageCounter) = New pageDetails With {.columns = columnCounter, .rows = 0, .startCol = pages(pageCounter).startCol}
                        columnSum = DGVBandejas.RowHeadersWidth + DGVBandejas.Columns(c).Width
                        columnCounter = 1
                        pageCounter += 1
                        pages.Add(pageCounter, New pageDetails With {.startCol = c})
                    End If
                    If c = 3 Then
                        If pages(pageCounter).columns = 0 Then
                            pages(pageCounter) = New pageDetails With {.columns = columnCounter, .rows = 0, .startCol = pages(pageCounter).startCol}
                        End If
                    End If
                Next
                maxPagesWide = pages.Keys.Max + 1

                pageCounter = 0

                Dim rowCounter As Integer = 0

                Dim rowSum As Integer = DGVBandejas.ColumnHeadersHeight

                For r As Integer = 0 To DGVBandejas.Rows.Count - 2
                    If rowSum + DGVBandejas.Rows(r).Height < maxHeight Then
                        rowSum += DGVBandejas.Rows(r).Height
                        rowCounter += 1
                    Else
                        pages(pageCounter) = New pageDetails With {.columns = pages(pageCounter).columns, .rows = rowCounter, .startCol = pages(pageCounter).startCol, .startRow = pages(pageCounter).startRow}
                        For x As Integer = 1 To maxPagesWide - 1
                            pages(pageCounter + x) = New pageDetails With {.columns = pages(pageCounter + x).columns, .rows = rowCounter, .startCol = pages(pageCounter + x).startCol, .startRow = pages(pageCounter).startRow}
                        Next

                        pageCounter += maxPagesWide
                        For x As Integer = 0 To maxPagesWide - 1
                            pages.Add(pageCounter + x, New pageDetails With {.columns = pages(x).columns, .rows = 0, .startCol = pages(x).startCol, .startRow = r})
                        Next

                        rowSum = DGVBandejas.ColumnHeadersHeight + DGVBandejas.Rows(r).Height
                        rowCounter = 1
                    End If
                    If r = DGVBandejas.Rows.Count - 2 Then
                        For x As Integer = 0 To maxPagesWide - 1
                            If pages(pageCounter + x).rows = 0 Then
                                pages(pageCounter + x) = New pageDetails With {.columns = pages(pageCounter + x).columns, .rows = rowCounter, .startCol = pages(pageCounter + x).startCol, .startRow = pages(pageCounter + x).startRow}
                            End If
                        Next
                    End If
                Next

                maxPagesTall = pages.Count \ maxPagesWide
                Exit Sub
            Case Else
                Exit Sub

        End Select

    End Sub

    Private Sub PrintDocument1_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage

        Select Case Pest_actual
            Case 1
                Dim rect As New Rectangle(20, 20, CInt(PrintDocument1.DefaultPageSettings.Bounds.Width), Label1.Height)
                Dim sf As New StringFormat
                sf.Alignment = StringAlignment.Center
                sf.LineAlignment = StringAlignment.Center
                Label1.Text = LabelAdmin.Text
                e.Graphics.DrawString(Label1.Text, Label1.Font, Brushes.Black, rect, sf)

                sf.Alignment = StringAlignment.Near

                Dim startX As Integer = 50
                Dim startY As Integer = rect.Bottom

                Static startPage As Integer = 0

                For p As Integer = startPage To pages.Count - 1
                    PrintDocument1.DefaultPageSettings.Landscape = True
                    Dim cell As New Rectangle(startX, startY, DGVAdmin.RowHeadersWidth, DGVAdmin.ColumnHeadersHeight)
                    e.Graphics.FillRectangle(New SolidBrush(SystemColors.ControlLight), cell)
                    e.Graphics.DrawRectangle(Pens.Black, cell)

                    startY += DGVAdmin.ColumnHeadersHeight

                    For r As Integer = pages(p).startRow To pages(p).startRow + pages(p).rows - 1
                        cell = New Rectangle(startX, startY, DGVAdmin.RowHeadersWidth, DGVAdmin.Rows(r).Height)
                        e.Graphics.FillRectangle(New SolidBrush(SystemColors.ControlLight), cell)
                        e.Graphics.DrawRectangle(Pens.Black, cell)
                        'e.Graphics.DrawString(DGVAdmin.Rows(r).HeaderCell.Value.ToString, DGVAdmin.Font, Brushes.Black, cell, sf)
                        startY += DGVAdmin.Rows(r).Height
                    Next

                    startX += cell.Width
                    startY = rect.Bottom

                    For c As Integer = pages(p).startCol To pages(p).startCol + pages(p).columns - 1
                        cell = New Rectangle(startX, startY, DGVAdmin.Columns(c).Width, DGVAdmin.ColumnHeadersHeight)
                        e.Graphics.FillRectangle(New SolidBrush(SystemColors.ControlLight), cell)
                        e.Graphics.DrawRectangle(Pens.Black, cell)
                        e.Graphics.DrawString(DGVAdmin.Columns(c).HeaderCell.Value.ToString, DGVAdmin.Font, Brushes.Black, cell, sf)
                        startX += DGVAdmin.Columns(c).Width
                    Next

                    startY = rect.Bottom + DGVAdmin.ColumnHeadersHeight

                    For r As Integer = pages(p).startRow To pages(p).startRow + pages(p).rows - 1
                        startX = 50 + DGVAdmin.RowHeadersWidth
                        For c As Integer = pages(p).startCol To pages(p).startCol + pages(p).columns - 1
                            cell = New Rectangle(startX, startY, DGVAdmin.Columns(c).Width, DGVAdmin.Rows(r).Height)
                            e.Graphics.DrawRectangle(Pens.Black, cell)
                            e.Graphics.DrawString(DGVAdmin(c, r).Value.ToString, DGVAdmin.Font, Brushes.Black, cell, sf)
                            startX += DGVAdmin.Columns(c).Width
                        Next
                        startY += DGVAdmin.Rows(r).Height
                    Next

                    If p <> pages.Count - 1 Then
                        startPage = p + 1
                        e.HasMorePages = True
                        Return
                    Else
                        startPage = 0
                    End If

                Next
                Exit Sub
            Case 2
                Dim rect As New Rectangle(20, 20, CInt(PrintDocument1.DefaultPageSettings.Bounds.Width), Label1.Height)
                Dim sf As New StringFormat
                sf.Alignment = StringAlignment.Center
                sf.LineAlignment = StringAlignment.Center
                Label1.Text = LabelMuestras.Text

                e.Graphics.DrawString(Label1.Text, Label1.Font, Brushes.Black, rect, sf)

                sf.Alignment = StringAlignment.Near

                Dim startX As Integer = 50
                Dim startY As Integer = rect.Bottom

                Static startpage2 As Integer = 0

                For p As Integer = startpage2 To pages.Count - 1
                    Dim cell As New Rectangle(startX, startY, DGVMuestras.RowHeadersWidth, DGVMuestras.ColumnHeadersHeight)
                    e.Graphics.FillRectangle(New SolidBrush(SystemColors.ControlLight), cell)
                    e.Graphics.DrawRectangle(Pens.Black, cell)

                    startY += DGVMuestras.ColumnHeadersHeight

                    For r As Integer = pages(p).startRow To pages(p).startRow + pages(p).rows - 1
                        cell = New Rectangle(startX, startY, DGVMuestras.RowHeadersWidth, DGVMuestras.Rows(r).Height)
                        e.Graphics.FillRectangle(New SolidBrush(SystemColors.ControlLight), cell)
                        e.Graphics.DrawRectangle(Pens.Black, cell)
                        'e.Graphics.DrawString(DGVMuestras.Rows(r).HeaderCell.Value.ToString, DGVMuestras.Font, Brushes.Black, cell, sf)
                        startY += DGVMuestras.Rows(r).Height
                    Next

                    startX += cell.Width
                    startY = rect.Bottom

                    For c As Integer = pages(p).startCol To pages(p).startCol + pages(p).columns - 1
                        cell = New Rectangle(startX, startY, DGVMuestras.Columns(c).Width, DGVMuestras.ColumnHeadersHeight)
                        e.Graphics.FillRectangle(New SolidBrush(SystemColors.ControlLight), cell)
                        e.Graphics.DrawRectangle(Pens.Black, cell)
                        e.Graphics.DrawString(DGVMuestras.Columns(c).HeaderCell.Value.ToString, DGVMuestras.Font, Brushes.Black, cell, sf)
                        startX += DGVMuestras.Columns(c).Width
                    Next

                    startY = rect.Bottom + DGVMuestras.ColumnHeadersHeight

                    For r As Integer = pages(p).startRow To pages(p).startRow + pages(p).rows - 1
                        startX = 50 + DGVMuestras.RowHeadersWidth
                        For c As Integer = pages(p).startCol To pages(p).startCol + pages(p).columns - 1
                            cell = New Rectangle(startX, startY, DGVMuestras.Columns(c).Width, DGVMuestras.Rows(r).Height)
                            e.Graphics.DrawRectangle(Pens.Black, cell)
                            e.Graphics.DrawString(DGVMuestras(c, r).Value.ToString, DGVMuestras.Font, Brushes.Black, cell, sf)
                            startX += DGVMuestras.Columns(c).Width
                        Next
                        startY += DGVMuestras.Rows(r).Height
                    Next

                    If p <> pages.Count - 1 Then
                        startpage2 = p + 1
                        e.HasMorePages = True
                        Return
                    Else
                        startpage2 = 0
                    End If

                Next
                Exit Sub
            Case 3
                Dim rect As New Rectangle(20, 20, CInt(PrintDocument1.DefaultPageSettings.Bounds.Width), Label1.Height)
                Dim sf As New StringFormat
                sf.Alignment = StringAlignment.Center
                sf.LineAlignment = StringAlignment.Center
                Label1.Text = LabelBandejas.Text
                e.Graphics.DrawString(Label1.Text, Label1.Font, Brushes.Black, rect, sf)

                sf.Alignment = StringAlignment.Near

                Dim startX As Integer = 50
                Dim startY As Integer = rect.Bottom

                Static startpage3 As Integer = 0

                For p As Integer = startpage3 To pages.Count - 1
                    Dim cell As New Rectangle(startX, startY, DGVBandejas.RowHeadersWidth, DGVBandejas.ColumnHeadersHeight)
                    e.Graphics.FillRectangle(New SolidBrush(SystemColors.ControlLight), cell)
                    e.Graphics.DrawRectangle(Pens.Black, cell)

                    startY += DGVBandejas.ColumnHeadersHeight

                    For r As Integer = pages(p).startRow To pages(p).startRow + pages(p).rows - 1
                        cell = New Rectangle(startX, startY, DGVBandejas.RowHeadersWidth, DGVBandejas.Rows(r).Height)
                        e.Graphics.FillRectangle(New SolidBrush(SystemColors.ControlLight), cell)
                        e.Graphics.DrawRectangle(Pens.Black, cell)
                        'e.Graphics.DrawString(DGVBandejas.Rows(r).HeaderCell.Value.ToString, DGVBandejas.Font, Brushes.Black, cell, sf)
                        startY += DGVBandejas.Rows(r).Height
                    Next

                    startX += cell.Width
                    startY = rect.Bottom

                    For c As Integer = pages(p).startCol To pages(p).startCol + pages(p).columns - 1
                        cell = New Rectangle(startX, startY, DGVBandejas.Columns(c).Width, DGVBandejas.ColumnHeadersHeight)
                        e.Graphics.FillRectangle(New SolidBrush(SystemColors.ControlLight), cell)
                        e.Graphics.DrawRectangle(Pens.Black, cell)
                        e.Graphics.DrawString(DGVBandejas.Columns(c).HeaderCell.Value.ToString, DGVBandejas.Font, Brushes.Black, cell, sf)
                        startX += DGVBandejas.Columns(c).Width
                    Next

                    startY = rect.Bottom + DGVBandejas.ColumnHeadersHeight

                    For r As Integer = pages(p).startRow To pages(p).startRow + pages(p).rows - 1
                        startX = 50 + DGVBandejas.RowHeadersWidth
                        For c As Integer = pages(p).startCol To pages(p).startCol + pages(p).columns - 1
                            cell = New Rectangle(startX, startY, DGVBandejas.Columns(c).Width, DGVBandejas.Rows(r).Height)
                            e.Graphics.DrawRectangle(Pens.Black, cell)
                            e.Graphics.DrawString(DGVBandejas(c, r).Value.ToString, DGVBandejas.Font, Brushes.Black, cell, sf)
                            startX += DGVBandejas.Columns(c).Width
                        Next
                        startY += DGVBandejas.Rows(r).Height
                    Next

                    If p <> pages.Count - 1 Then
                        startpage3 = p + 1
                        e.HasMorePages = True
                        Return
                    Else
                        startpage3 = 0
                    End If

                Next
                Exit Sub
            Case Else
                Exit Sub
        End Select


    End Sub

    Private Sub BtnSi_Click(sender As Object, e As EventArgs) Handles BtnSi.Click
        Dim Tabla_Actual As String = LabelAdmin.Text

        If Tabla_Actual = "Muestras Asignadas" Then
            Dim t_terminado As String
            Dim muestra_id As String = TextBox6.Text

            Try
                conn.Open()
                Dim cmd As New MySqlCommand(String.Format("SELECT NOW();"), conn)
                Dim fecha_DB As DateTime = cmd.ExecuteScalar()
                t_terminado = fecha_DB.ToString("yyyy-MM-dd HH:mm:ss")
                conn.Close()
            Catch ex As Exception
                MsgBox(ex.Message, False, "No se puede obtener la fecha de la base de datos")
                conn.Close()
                Exit Sub
            End Try
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
            Cargar_MuestrasBandejas_Admin()

        ElseIf Tabla_Actual = "Bandejas Asignadas" Then
            Dim t_terminado As String
            Dim bandeja_id As String = TextBox6.Text

            Try
                conn.Open()
                Dim cmd As New MySqlCommand(String.Format("SELECT NOW();"), conn)
                Dim fecha_DB As DateTime = cmd.ExecuteScalar()
                t_terminado = fecha_DB.ToString("yyyy-MM-dd HH:mm:ss")
                conn.Close()
            Catch ex As Exception
                MsgBox(ex.Message, False, "No se puede obtener la fecha de la base de datos")
                conn.Close()
                Exit Sub
            End Try
            Try
                conn.Open()
                Dim cmd As New MySqlCommand(String.Format("UPDATE rev_bandejas SET Tiempo_T = '" & t_terminado & "', Estado = 'Finalizado', Pasa = 'Si' WHERE Bandeja_ID='" & bandeja_id & "';"), conn)
                cmd.ExecuteNonQuery()
                MsgBox("Registro modificado satisfactoriamente", False, "Registro modificado")
            Catch ex As MySqlException
                MsgBox(ex.Message, False, "Error")
                conn.Close()
            End Try
            conn.Close()
            Cargar_MuestrasBandejas_Admin()

        End If
        Cargar()
    End Sub

    Private Sub BtnNo_Click(sender As Object, e As EventArgs) Handles BtnNo.Click
        Dim Tabla_Actual As String = LabelAdmin.Text

        If Tabla_Actual = "Muestras Asignadas" Then

            Dim t_terminado As String
            Dim t_asignacion_2 As String
            Dim valor_c2_existe As Boolean
            If TextBox3.Text = Nothing Then
                valor_c2_existe = False
            Else
                valor_c2_existe = True
            End If
            Dim muestra_id As String = TextBox6.Text

            Try
                conn.Open()
                Dim cmd As New MySqlCommand(String.Format("SELECT NOW();"), conn)
                Dim fecha_DB As DateTime = cmd.ExecuteScalar()
                t_terminado = fecha_DB.ToString("yyyy-MM-dd HH:mm:ss")
                conn.Close()
            Catch ex As Exception
                MsgBox(ex.Message, False, "No se puede obtener la fecha de la base de datos")
                conn.Close()
                Exit Sub
            End Try

            If Not (valor_c2_existe) Then
                Try
                    conn.Open()
                    Dim cmd As New MySqlCommand(String.Format("SELECT NOW();"), conn)
                    Dim fecha_DB As DateTime = cmd.ExecuteScalar()
                    t_asignacion_2 = fecha_DB.ToString("yyyy-MM-dd HH:mm:ss")
                    conn.Close()
                Catch ex As Exception
                    MsgBox(ex.Message, False, "No se puede obtener la fecha de la base de datos")
                    conn.Close()
                    Exit Sub
                End Try
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
                Cargar_MuestrasBandejas_Admin()
            ElseIf valor_c2_existe Then

                Try
                    conn.Open()
                    Dim cmd As New MySqlCommand(String.Format("SELECT NOW();"), conn)
                    Dim fecha_DB As DateTime = cmd.ExecuteScalar()
                    t_terminado = fecha_DB.ToString("yyyy-MM-dd HH:mm:ss")
                    conn.Close()
                Catch ex As Exception
                    MsgBox(ex.Message, False, "No se puede obtener la fecha de la base de datos")
                    conn.Close()
                    Exit Sub
                End Try

                Try
                    conn.Open()
                    Dim cmd As New MySqlCommand(String.Format("UPDATE rev_muestras SET `Tiempo_T`='" & t_terminado & "', Estado= 'Finalizado', Pasa = 'No' WHERE Muestra_ID ='" & muestra_id & "';"), conn)
                    cmd.ExecuteNonQuery()
                    MsgBox("Registro modificado satisfactoriamente", False, "Registro modificado")
                Catch ex As MySqlException
                    MsgBox(ex.Message, False, "Error")
                    conn.Close()
                End Try
                conn.Close()
                Cargar_MuestrasBandejas_Admin()
            End If
            Cargar()
        ElseIf Tabla_Actual = "Bandejas Asignadas" Then

            Dim t_terminado As String
            Dim t_asignacion_2 As String
            Dim valor_c2_existe As Boolean
            If TextBox3.Text = Nothing Then
                valor_c2_existe = False
            Else
                valor_c2_existe = True
            End If
            Dim bandeja_id As String = TextBox6.Text

            Try
                conn.Open()
                Dim cmd As New MySqlCommand(String.Format("SELECT NOW();"), conn)
                Dim fecha_DB As DateTime = cmd.ExecuteScalar()
                t_terminado = fecha_DB.ToString("yyyy-MM-dd HH:mm:ss")
                conn.Close()
            Catch ex As Exception
                MsgBox(ex.Message, False, "No se puede obtener la fecha de la base de datos")
                conn.Close()
                Exit Sub
            End Try

            If Not (valor_c2_existe) Then
                Try
                    conn.Open()
                    Dim cmd As New MySqlCommand(String.Format("SELECT NOW();"), conn)
                    Dim fecha_DB As DateTime = cmd.ExecuteScalar()
                    t_asignacion_2 = fecha_DB.ToString("yyyy-MM-dd HH:mm:ss")
                    conn.Close()
                Catch ex As Exception
                    MsgBox(ex.Message, False, "No se puede obtener la fecha de la base de datos")
                    conn.Close()
                    Exit Sub
                End Try
                Try
                    conn.Open()
                    Dim cmd As New MySqlCommand(String.Format("UPDATE rev_bandejas SET Tiempo_A2='" & t_asignacion_2 & "', Estado ='Pendiente', Pasa = 'No' WHERE Bandeja_ID ='" & bandeja_id & "';"), conn)
                    cmd.ExecuteNonQuery()
                    MsgBox("Registro modificado satisfactoriamente", False, "Registro modificado")
                Catch ex As MySqlException
                    MsgBox(ex.Message, False, "Error")
                    conn.Close()
                End Try
                conn.Close()
                Cargar_MuestrasBandejas_Admin()
            ElseIf valor_c2_existe Then
                Try
                    conn.Open()
                    Dim cmd As New MySqlCommand(String.Format("SELECT NOW();"), conn)
                    Dim fecha_DB As DateTime = cmd.ExecuteScalar()
                    t_terminado = fecha_DB.ToString("yyyy-MM-dd HH:mm:ss")
                    conn.Close()
                Catch ex As Exception
                    MsgBox(ex.Message, False, "No se puede obtener la fecha de la base de datos")
                    conn.Close()
                    Exit Sub
                End Try

                Try
                    conn.Open()
                    Dim cmd As New MySqlCommand(String.Format("UPDATE rev_bandejas SET Tiempo_T ='" & t_terminado & "', Estado= 'Finalizado', Pasa = 'No' WHERE Bandeja_ID ='" & bandeja_id & "';"), conn)
                    cmd.ExecuteNonQuery()
                    MsgBox("Registro modificado satisfactoriamente", False, "Registro modificado")
                Catch ex As MySqlException
                    MsgBox(ex.Message, False, "Error")
                    conn.Close()
                End Try
                conn.Close()
            Cargar_MuestrasBandejas_Admin()
        End If
        Cargar()
        End If
    End Sub

    Private Sub LabelCambiarContraseña_Click(sender As Object, e As EventArgs) Handles LabelCambiarContraseña.Click

        If CmbBxAnalistas.Text = " " Then
            Dim usuario As Integer = 1
            Dim reader As MySqlDataReader
            TxBxRespuestaForm3.Text = ""
            TxBxContraseñaAnterior.Text = ""
            TxBxContraseñaNueva.Text = ""
            TextBoxContraseña.Text = ""
            FormCambiarContraseña.ShowDialog()
            Dim respuestaform3 As String = TxBxRespuestaForm3.Text
            If respuestaform3 = "1" Then
                Dim contraseña_anterior As String = TxBxContraseñaAnterior.Text
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
                If contraseña_anterior = bd_password Then
                    Dim contraseña_nueva As String = TxBxContraseñaNueva.Text
                    Dim contraseña_nueva2 As String = TextBoxContraseña.Text
                    If contraseña_nueva = contraseña_nueva2 Then
                        Try
                            conn.Open()
                            Dim query As String = "UPDATE analistas set Contraseña = '" & contraseña_nueva & "' where AnalistNo = " & usuario
                            Dim cmd As New MySqlCommand(query, conn)
                            reader = cmd.ExecuteReader
                            MsgBox("Contraseña Actualizada", False, "Contraseña Actualizada")
                            conn.Close()
                        Catch ex As MySqlException
                            MsgBox(ex.Message)
                            conn.Close()
                        End Try
                    Else
                        MsgBox("Ingreso dos contraseñas diferentes en los campos de contraseña nueva", False, "Error")
                    End If
                Else
                    MsgBox("La contraseña ingresada es erronea", False, "Error")
                End If
            Else
                Exit Sub
            End If

        Else
            Dim usuario As Integer = CmbBxAnalistas.SelectedValue
            Dim reader As MySqlDataReader
            TxBxRespuestaForm3.Text = ""
            TxBxContraseñaAnterior.Text = ""
            TxBxContraseñaNueva.Text = ""
            TextBoxContraseña.Text = ""
            FormCambiarContraseña.ShowDialog()
            Dim respuestaform3 As String = TxBxRespuestaForm3.Text

            If respuestaform3 = "1" Then
                Dim contraseña_anterior As String = TxBxContraseñaAnterior.Text
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
                If contraseña_anterior = bd_password Then
                    Dim contraseña_nueva As String = TxBxContraseñaNueva.Text
                    Dim contraseña_nueva2 As String = TextBoxContraseña.Text
                    If contraseña_nueva = contraseña_nueva2 Then
                        Try
                            conn.Open()
                            Dim query As String = "UPDATE analistas set Contraseña = '" & contraseña_nueva & "' where AnalistNo = " & usuario
                            Dim cmd As New MySqlCommand(query, conn)
                            reader = cmd.ExecuteReader
                            MsgBox("Contraseña Actualizada", False, "Contraseña Actualizada")
                            conn.Close()
                        Catch ex As MySqlException
                            MsgBox(ex.Message)
                            conn.Close()
                        End Try
                    Else
                        MsgBox("Ingreso dos contraseñas diferentes en los campos de contraseña nueva", False, "Error")
                    End If
                Else
                    MsgBox("La contraseña ingresada es erronea", False, "Error")
                End If
            Else
                Exit Sub
            End If
        End If
    End Sub

    Private Sub AgregarMuestrasTXT_Click(sender As Object, e As EventArgs) Handles AgregarMuestrasTXT.Click
        Dim AbrirMuestraTXT As New OpenFileDialog
        AbrirMuestraTXT.InitialDirectory = "C:\"
        AbrirMuestraTXT.RestoreDirectory = True
        AbrirMuestraTXT.ShowDialog()
        Dim filepath As String

        Try
            filepath = IO.Path.GetFullPath(AbrirMuestraTXT.FileName)
        Catch ex As Exception
            MsgBox(ex.Message, False, "Error")
            Exit Sub
        End Try

        Dim numero_de_lineas = IO.File.ReadAllLines(filepath).Length

        Dim sData() As String
        Dim numeromuestras, prueba, valor_in As New List(Of String)()

        Using sr As New IO.StreamReader(filepath)
            While Not sr.EndOfStream
                sData = sr.ReadLine().Split(";"c)

                numeromuestras.Add(sData(0).Trim())
                prueba.Add(sData(1).Trim())
                valor_in.Add(sData(2).Trim())
            End While
        End Using

        Dim prueNo As String
        Dim llave As String
        Dim t_creacion As String

        For i As Integer = 0 To numero_de_lineas - 1

            Try
                conn.Open()
                Dim cmd As New MySqlCommand(String.Format("Select PrueNo FROM pruebas WHERE Nombre ='" & prueba(i) & "';"), conn)
                prueNo = Convert.ToString(cmd.ExecuteScalar())
                conn.Close()
            Catch ex As Exception
                MsgBox(ex.Message, False, "Error")
                conn.Close()
                Exit Sub
            End Try

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

            Try
                conn.Open()
                Dim cmd As New MySqlCommand(String.Format("SELECT NOW();"), conn)
                Dim fecha_servidor As DateTime = cmd.ExecuteScalar()
                t_creacion = fecha_servidor.ToString("yyyy-MM-dd HH:mm:ss")
                conn.Close()
            Catch ex As Exception
                MsgBox(ex.Message, False, "No se puede obtener la fecha de la base de datos")
                conn.Close()
                Exit Sub
            End Try

            Try
                conn.Open()
                Dim cmd As New MySqlCommand(String.Format("INSERT INTO rev_muestras (Muestra_ID, Muestra_No, PrueNo, Valor_In, Tiempo_C, Estado) VALUES ('" & llave & "', '" & numeromuestras(i) & "', '" & prueNo & "', '" & valor_in(i) & "', '" & t_creacion & "', 'Pendiente');"),conn)
                cmd.ExecuteNonQuery()
                conn.Close()
            Catch ex As Exception
                MsgBox(ex.Message, False, "Error")
                conn.Close()
                Exit Sub
            End Try
        Next

    End Sub
End Class