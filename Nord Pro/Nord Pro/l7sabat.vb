Imports System.Data.SqlClient

Public Class l7sabat
    Dim ds As New DataSet()
    Dim dat As SqlDataAdapter
    Dim cnx As New SqlConnection("Data Source=DESKTOP-MFI4B2P;Initial Catalog=Nordpro;Integrated Security=True")

    Private Sub l7sabat_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            cnx.Open()
            ' You can perform database-related operations here if needed.
        Catch ex As Exception
            MsgBox("Erreur de connexion : " & ex.Message)
            Me.Close()
        End Try

        Try
            ' Create the SqlDataAdapter and populate the DataSet with a parameterized query
            dat = New SqlDataAdapter("SELECT Compt.Nom,Compt.Numtel AS [telephon] FROM Compt ", cnx)
            dat.SelectCommand.Parameters.Add("@search", SqlDbType.VarChar).Value = "%" & TextBox1.Text & "%"
            dat.Fill(ds, "cmpt")

            Dim dal As New SqlDataAdapter("SELECT SUM(montant) AS total FROM payement", cnx)
            Dim dsTotal As New DataSet()
            dal.Fill(dsTotal, "ttl")

            If dsTotal.Tables("ttl").Rows.Count > 0 Then
                Dim total As Object = dsTotal.Tables("ttl").Rows(0)("total")

                If Not IsDBNull(total) Then
                    Label4.Text = total.ToString()
                Else
                    Label4.Text = "0" ' or any default value
                End If
            Else
                Label4.Text = "0" ' or any default value
            End If
            Dim dalFacture As New SqlDataAdapter("SELECT SUM(Montant) AS total FROM Facture", cnx)
            Dim dsTotalFacture As New DataSet()
            dalFacture.Fill(dsTotalFacture, "py")

            If dsTotalFacture.Tables("py").Rows.Count > 0 Then
                Dim totalFacture As Object = dsTotalFacture.Tables("py").Rows(0)("total")

                If Not IsDBNull(totalFacture) Then
                    Label6.Text = totalFacture.ToString()
                Else
                    Label6.Text = "0" ' or any default value
                End If
            Else
                Label6.Text = "0" ' or any default value
            End If
            ' Convert Label4.Text and Label6.Text to numbers
            Dim montantPayement As Decimal
            Dim montantFacture As Decimal

            If Decimal.TryParse(Label4.Text, montantPayement) AndAlso Decimal.TryParse(Label6.Text, montantFacture) Then
                ' Perform the subtraction
                Dim difference As Decimal = montantPayement - montantFacture

                ' Update Label8.Text with the result
                Label8.Text = difference.ToString()
            Else
                ' Handle invalid input (e.g., non-numeric text)
                Label8.Text = "Invalid input"
            End If

            ' Set up the DataGridView
            DataGridView1.DataSource = ds.Tables("cmpt")
        Catch ex As Exception
            MsgBox("Error: " & ex.Message)
        End Try
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        Dim dv As New DataView
        dv.Table = ds.Tables("cmpt")
        dv.RowFilter = "Nom LIKE '" & TextBox1.Text & "%'"
        DataGridView1.DataSource = dv
    End Sub

   

    ' Create a DataAdapter for the table you want to update


    ' Update the database with changes from the DataSet




    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim selectedRowIndex As Integer = DataGridView1.CurrentCell.RowIndex

        Dim selectedName As String = DataGridView1.Rows(selectedRowIndex).Cells(0).Value.ToString()

        Dim d As New credit(selectedName)
        d.Show()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim selectedRowIndex As Integer = DataGridView1.CurrentCell.RowIndex

        Dim selectedName As String = DataGridView1.Rows(selectedRowIndex).Cells(0).Value.ToString()

        Dim d As New detaillescl(selectedName)
        d.Show()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim selectedRowIndex As Integer = DataGridView1.CurrentCell.RowIndex

        Dim selectedName As String = DataGridView1.Rows(selectedRowIndex).Cells(0).Value.ToString()
        Dim py As New dofo3at(selectedName)
        py.Show()
    End Sub

   Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Try
            ' Check if the SqlConnection is initialized
            If cnx Is Nothing Then
                MessageBox.Show("Connection is not properly initialized.")
                Return
            End If

            ' Create DataAdapters for each table
            Dim daFacture As New SqlDataAdapter("SELECT * FROM Facture", cnx)
            Dim daPayement As New SqlDataAdapter("SELECT * FROM payement", cnx)
            Dim daPersonne As New SqlDataAdapter("SELECT * FROM personne", cnx)
            Dim daCompt As New SqlDataAdapter("SELECT * FROM Compt", cnx)

            ' Fill DataSets for each table
            Dim dsFacture As New DataSet()
            Dim dsPayement As New DataSet()
            Dim dsPersonne As New DataSet()
            Dim dsCompt As New DataSet()

            daFacture.Fill(dsFacture, "Facture")
            daPayement.Fill(dsPayement, "payement")
            daPersonne.Fill(dsPersonne, "personne")
            daCompt.Fill(dsCompt, "Compt")

            ' Get the selected row index
            Dim rowIndex As Integer = DataGridView1.CurrentRow.Index

            If rowIndex <> -1 Then
                ' Check if the DataRow exists in each table
                If rowIndex < dsFacture.Tables("Facture").Rows.Count Then
                    dsFacture.Tables("Facture").Rows(rowIndex).Delete()
                End If

                If rowIndex < dsPayement.Tables("payement").Rows.Count Then
                    dsPayement.Tables("payement").Rows(rowIndex).Delete()
                End If

                If rowIndex < dsPersonne.Tables("personne").Rows.Count Then
                    dsPersonne.Tables("personne").Rows(rowIndex).Delete()
                End If

                If rowIndex < dsCompt.Tables("Compt").Rows.Count Then
                    dsCompt.Tables("Compt").Rows(rowIndex).Delete()
                End If

                ' Update each table separately
                daFacture.Update(dsFacture, "Facture")
                daPayement.Update(dsPayement, "payement")
                daPersonne.Update(dsPersonne, "personne")
                daCompt.Update(dsCompt, "Compt")

                MessageBox.Show("Row deleted successfully.")
            Else
                MessageBox.Show("No row selected to delete.")
            End If
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try
    End Sub

  
End Class

