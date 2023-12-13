Imports System.Data.SqlClient

Public Class dofo3at
    Dim cnx As SqlConnection
    Dim dat As SqlDataAdapter
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
        Label7.Text = selectedName


    End Sub
   
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        dat = New SqlDataAdapter("select * from payement where Nom= '" & Label7.Text & "'", cnx)
        dat.Fill(ds, "pay")
        Dim l As DataRow
        l = ds.Tables("pay").NewRow
        l(0) = Label7.Text
        If CheckBox1.Checked Then
            l(1) = CheckBox1.Text
        ElseIf CheckBox1.Checked = True And CheckBox2.Checked = True Then
            l(1) = CheckBox1.Text & CheckBox2.Text
        Else
            l(1) = CheckBox2.Text
        End If


        l(2) = TextBox1.Text
        l(3) = TextBox2.Text
        l(4) = DateTimePicker1.Value
        ds.Tables("pay").Rows.Add(l)
        Dim cmd As New SqlCommandBuilder(dat)
        dat.Update(ds, "pay")
        MsgBox("le payment et ajouter succesivement et vous ecrire " & TextBox1.Text & "pour le client " & Label7.Text)
    End Sub
    Private Sub dofo3at_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim dav As New SqlDataAdapter("select mtpay, montant, numcheq, date from payement where Nom='" & Label7.Text & "'", cnx)
        dav.Fill(ds, "dataview")
        DataGridView1.DataSource = ds.Tables("dataview")

        Dim das As New SqlDataAdapter("select sum(montant) as total from payement where Nom ='" & Label7.Text & "'", cnx)
        das.Fill(ds, "su")

        ' Check if the dataset table has rows (data) before accessing it
        If ds.Tables("su").Rows.Count > 0 Then
            ' Access the first row of the dataset table and the "total" column
            Dim totalSum As Object = ds.Tables("su").Rows(0)("total")

            ' Check if the value is not DBNull before setting it as the label text
            If Not IsDBNull(totalSum) Then
                Label10.Text = totalSum.ToString()
            Else
                Label10.Text = "0" ' or any default value
            End If
        Else
            Label10.Text = "0" ' or any default value
        End If
        

    End Sub


    
   
    Private Sub DateTimePicker2_ValueChanged(sender As Object, e As EventArgs) Handles DateTimePicker2.ValueChanged
        Dim selectedDate As Date = DateTimePicker2.Value

        Dim dv As New DataView
        dv.Table = ds.Tables("dataview")

        ' Format the date using the desired format (e.g., yyyy-MM-dd)
        Dim formattedDate As String = selectedDate.ToString("yyyy-MM-dd")

        ' Apply the date filter
        dv.RowFilter = "date >= '" & formattedDate & " 00:00:00' AND date <= '" & formattedDate & " 23:59:59'"

        DataGridView1.DataSource = dv
    End Sub

End Class