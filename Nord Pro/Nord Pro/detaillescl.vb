Imports System.Data.SqlClient

Public Class detaillescl
    Dim cnx As SqlConnection
    Dim dat, nm As SqlDataAdapter
    Dim ds As New DataSet

    Public Sub New(ByVal selectedName As String)
        InitializeComponent()
        cnx = New SqlConnection("Data Source=DESKTOP-MFI4B2P;Initial Catalog=Nordpro;Integrated Security=True")

        ' Modify your SQL query to filter by the selected name
        Dim query As String = "SELECT Compt.Nom FROM Compt WHERE Compt.Nom = @SelectedName"
        dat = New SqlDataAdapter(query, cnx)
        dat.SelectCommand.Parameters.AddWithValue("@SelectedName", selectedName)

        dat.Fill(ds, "datagrid")

        ' Display the selected name
        Label3.Text = selectedName
    End Sub

    Private Sub detaillescl_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Load the data for the DataGridView
        dat = New SqlDataAdapter("SELECT Numfacture, Datefact, Montant FROM Facture WHERE Nom = @Nom", cnx)
        dat.SelectCommand.Parameters.AddWithValue("@Nom", Label3.Text)
        dat.Fill(ds, "fct")
        DataGridView1.DataSource = ds.Tables("fct")

        ' Calculate and display the sum of Montant
        Dim sumAdapter As New SqlDataAdapter("SELECT SUM(Montant) AS TotalMontant FROM Facture WHERE Nom = @Nom", cnx)
        sumAdapter.SelectCommand.Parameters.AddWithValue("@Nom", Label3.Text)
        sumAdapter.Fill(ds, "ttl")

        ' Check if there are any rows with the sum (in case no records match the criteria)
        If ds.Tables("ttl").Rows.Count > 0 AndAlso Not IsDBNull(ds.Tables("ttl").Rows(0)("TotalMontant")) Then
            Label5.Text = ds.Tables("ttl").Rows(0)("TotalMontant").ToString()
        Else
            Label5.Text = "0" ' or any default value you prefer
        End If

        ' Set the DataGridView data source to the "fct" table
        DataGridView1.DataSource = ds.Tables("fct")

        ' Calculate the total sum of payement.montant
        Dim paySumAdapter As New SqlDataAdapter("SELECT SUM(montant) AS TotalMontant FROM payement WHERE Nom = @Nom", cnx)
        paySumAdapter.SelectCommand.Parameters.AddWithValue("@Nom", Label3.Text)
        paySumAdapter.Fill(ds, "su")

        ' Check if there are any rows with the sum (in case no records match the criteria)
        If ds.Tables("su").Rows.Count > 0 AndAlso Not IsDBNull(ds.Tables("su").Rows(0)("TotalMontant")) Then
            Label10.Text = ds.Tables("su").Rows(0)("TotalMontant").ToString()
        Else
            Label10.Text = "0" ' or any default value you prefer
        End If
        ' Query to get the total sum of Facture.Montant
        Dim creQuery As String = "SELECT SUM(Montant) AS TotalMontant FROM Facture WHERE Nom = @Nom"
        Dim creAdapter As New SqlDataAdapter(creQuery, cnx)
        creAdapter.SelectCommand.Parameters.AddWithValue("@Nom", Label3.Text)
        creAdapter.Fill(ds, "cre")

        ' Query to get the total sum of payement.montant
        Dim payQuery As String = "SELECT SUM(montant) AS TotalMontant FROM payement WHERE Nom = @Nom"
        Dim payAdapter As New SqlDataAdapter(payQuery, cnx)
        payAdapter.SelectCommand.Parameters.AddWithValue("@Nom", Label3.Text)
        payAdapter.Fill(ds, "pay")

        ' Calculate the difference
        Dim creValue As Decimal = If(ds.Tables("cre").Rows.Count > 0 AndAlso Not IsDBNull(ds.Tables("cre").Rows(0)("TotalMontant")), Convert.ToDecimal(ds.Tables("cre").Rows(0)("TotalMontant")), 0)
        Dim payValue As Decimal = If(ds.Tables("pay").Rows.Count > 0 AndAlso Not IsDBNull(ds.Tables("pay").Rows(0)("TotalMontant")), Convert.ToDecimal(ds.Tables("pay").Rows(0)("TotalMontant")), 0)

        ' Calculate the difference between cre and pay
        Dim restMontant As Decimal = creValue - payValue

        ' Display or use the calculated value as needed
        Label7.Text = restMontant.ToString()

    End Sub

    Private Sub Label5_Click(sender As Object, e As EventArgs) Handles Label5.Click

    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        ' Filter the data in the DataGridView based on user input
        Dim dv As New DataView
        dv.Table = ds.Tables("fct")
        dv.RowFilter = "Numfacture LIKE '" & TextBox1.Text & "%'"
        DataGridView1.DataSource = dv
    End Sub
    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        ' Check if a valid row is clicked (avoid headers)
        If e.RowIndex >= 0 Then
            ' Assuming your columns are in order: Numfacture, Datefact, Montant

            TextBox2.Text = DataGridView1.Rows(e.RowIndex).Cells("Numfacture").Value.ToString()
            TextBox4.Text = DataGridView1.Rows(e.RowIndex).Cells("Montant").Value.ToString()

            ' Assuming Datefact is a DateTime column
            DateTimePicker1.Value = Convert.ToDateTime(DataGridView1.Rows(e.RowIndex).Cells("Datefact").Value)
        End If
    End Sub


    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        ' Check if there is a selected row in the DataGridView
        If DataGridView1.SelectedRows.Count > 0 Then
            ' Get the selected row
            Dim selectedRow As DataGridViewRow = DataGridView1.SelectedRows(0)

            ' Get the value from the Numfacture column of the selected row
            Dim numFacture As String = selectedRow.Cells("Numfacture").Value.ToString()

            ' Delete the row from the dataset
            Dim rowToDelete As DataRow = ds.Tables("fct").Select("Numfacture = '" & numFacture & "'").FirstOrDefault()
            If rowToDelete IsNot Nothing Then
                rowToDelete.Delete()
            End If

            ' Update the database (perform the actual deletion)
            Dim deleteCommand As New SqlCommand("DELETE FROM Facture WHERE Nom = @Nom AND Numfacture = @Numfacture", cnx)
            deleteCommand.Parameters.AddWithValue("@Nom", Label3.Text)
            deleteCommand.Parameters.AddWithValue("@Numfacture", numFacture)

            ' Open the connection, execute the command, and close the connection
            cnx.Open()
            deleteCommand.ExecuteNonQuery()
            cnx.Close()

            ' Refresh the DataGridView
            DataGridView1.DataSource = ds.Tables("fct").DefaultView
        Else
            MessageBox.Show("Please select a row to delete.")
        End If
    End Sub


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ' Check if there is a selected row in the DataGridView
        If DataGridView1.SelectedRows.Count > 0 Then
            ' Get the selected row
            Dim selectedRow As DataGridViewRow = DataGridView1.SelectedRows(0)

            ' Get the value from the Numfacture column of the selected row
            Dim numFacture As String = selectedRow.Cells("Numfacture").Value.ToString()

            ' Update the row in the dataset
            Dim rowToUpdate As DataRow = ds.Tables("fct").Select("Numfacture = '" & numFacture & "'").FirstOrDefault()
            If rowToUpdate IsNot Nothing Then
                rowToUpdate("Datefact") = DateTimePicker1.Value
                rowToUpdate("Montant") = TextBox2.Text
            End If

            ' Update the database (perform the actual update)
            Dim updateCommand As New SqlCommand("UPDATE Facture SET Datefact = @Datefact, Montant = @Montant WHERE Nom = @Nom AND Numfacture = @Numfacture", cnx)
            updateCommand.Parameters.AddWithValue("@Nom", Label3.Text)
            updateCommand.Parameters.AddWithValue("@Numfacture", TextBox2.Text)
            updateCommand.Parameters.AddWithValue("@Datefact", DateTimePicker1.Value)
            updateCommand.Parameters.AddWithValue("@Montant", TextBox4.Text)

            ' Open the connection, execute the command, and close the connection
            cnx.Open()
            updateCommand.ExecuteNonQuery()
            cnx.Close()

            ' Refresh the DataGridView
            DataGridView1.DataSource = ds.Tables("fct").DefaultView

            MessageBox.Show("Data updated successfully.")
        Else
            MessageBox.Show("Please select a row to update.")
        End If
    End Sub





End Class
