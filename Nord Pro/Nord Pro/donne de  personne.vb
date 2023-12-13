Imports System.Data.SqlClient

Public Class donne_de__personne
    Dim ds As New DataSet()
    Dim dat, dat2 As New SqlDataAdapter()
    Dim cnx As New SqlConnection("Data Source=DESKTOP-MFI4B2P;Initial Catalog=Nordpro;Integrated Security=True")

    Private Sub donne_de__personne_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            cnx.Open()
            ' You can perform database-related operations here if needed.
        Catch ex As Exception
            MsgBox("Erreur de connexion : " & ex.Message)
            Me.Close()
        End Try
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            dat = New SqlDataAdapter("SELECT * FROM personne", cnx)
            dat2 = New SqlDataAdapter("SELECT Nom, Numtel FROM Compt", cnx)

            dat.Fill(ds, "nv_client")
            dat2.Fill(ds, "cmpt")
            Dim newRow1 As DataRow = ds.Tables("nv_client").NewRow()
            Dim newRow2 As DataRow = ds.Tables("cmpt").NewRow()


            newRow1("Nom") = TextBox1.Text ' Assuming the column names match
            newRow1("Numtel") = TextBox2.Text ' Assuming the column names match
            newRow1("Dateouv") = DateTimePicker1.Value

            newRow2("Nom") = TextBox1.Text ' Assuming the column names match
            newRow2("Numtel") = TextBox2.Text ' Assuming the column names match

            ds.Tables("nv_client").Rows.Add(newRow1)
            ds.Tables("cmpt").Rows.Add(newRow2)

            Dim cmd As New SqlCommandBuilder(dat)
            Dim cmd2 As New SqlCommandBuilder(dat2)
            dat.Update(ds, "nv_client")
            dat2.Update(ds, "cmpt")

            MsgBox("Le client a été ajouté avec succès.")
        Catch ex As Exception
            MsgBox("Erreur lors de l'ajout du client : " & ex.Message)
        End Try
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim th As New Form1
        th.Show()
    End Sub
End Class
