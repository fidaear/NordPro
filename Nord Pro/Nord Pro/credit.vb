Imports System.Data.SqlClient

Public Class credit
    Dim cnx As SqlConnection
    Dim dat As SqlDataAdapter
    Dim ds As New DataSet

    ' Constructor that accepts the selected name as a parameter
    Public Sub New(ByVal selectedName As String)
        InitializeComponent()
        cnx = New SqlConnection("Data Source=DESKTOP-MFI4B2P;Initial Catalog=Nordpro;Integrated Security=True")

        ' Modify your SQL query to filter by the selected name
        Dim query As String = "SELECT Compt.Nom FROM Compt WHERE Compt.Nom = @SelectedName"
        dat = New SqlDataAdapter(query, cnx)
        dat.SelectCommand.Parameters.AddWithValue("@SelectedName", selectedName)

        dat.Fill(ds, "datagrid")

        ' Display the selected name
        Label4.Text = selectedName

       
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ' Rest of your code for loading data and updating the database
        dat = New SqlDataAdapter("SELECT * FROM Facture where Nom = '" & Label4.Text & "'", cnx)
        dat.Fill(ds, "facture")

        ' Assuming Label4.Text contains the value you want to search for
        Dim searchValue As String = Label4.Text

        ' Use the Select method to find the matching DataRow
        Dim l As DataRow
        l = ds.Tables("facture").NewRow


            ' Set the values for the columns in the DataRow
        l(0) = Label4.Text
        l(1) = TextBox1.Text
        l(2) = DateTimePicker1.Value ' Replace "ColumnName2" with the actual column name
        l(3) = TextBox2.Text ' Replace "ColumnName3" with the actual column name
        ds.Tables("facture").Rows.Add(l)
            ' Update the database
            Dim cmd As New SqlCommandBuilder(dat)
            dat.Update(ds, "facture")
            MsgBox("le credit a été ajouté")


    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim rv As New l7sabat
        rv.Show()
    End Sub

    Private Sub credit_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class


