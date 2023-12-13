Imports System.Data.SqlClient

Public Class Form1
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            Dim cnx As New SqlConnection("Data Source=DESKTOP-MFI4B2P;Initial Catalog=Nordpro;Integrated Security=True")
            cnx.Open()
            ' You can perform database-related operations here if needed.
        Catch ex As Exception
            MsgBox("erreur de connection")
            End
        End Try
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim sec As New donne_de__personne
        sec.Show()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim thr As New l7sabat
        thr.Show()
    End Sub
End Class
