 			Dim query As String = " Select table_name from Information_Schema.Tables where table_type = 'base table' and table_schema = 'revision_bd'"
            Dim cmd As New MySqlCommand(query, conn)
            Dim sqlAdap As New MySqlDataAdapter(cmd)
            Dim dtRecord As New DataTable
            sqlAdap.Fill(dtRecord)
            ComboBox1.DataSource = dtRecord
            ComboBox1.DisplayMember = "TABLE_NAME"
            ComboBox1.ValueMember = "TABLE_NAME"