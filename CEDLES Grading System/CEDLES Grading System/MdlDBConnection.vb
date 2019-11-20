Imports System.Data.Sql
Imports System.Data.SqlClient
Module MdlDBConnection
    Public con As New SqlConnection("Data Source=TAN-PC\MSSQLSERVER1;Initial Catalog=SchoolDatabase;Integrated Security=True")
    Public cmd As SqlCommand
    Public dr As SqlClient.SqlDataReader
End Module
