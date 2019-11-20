Imports System.Data.Sql
Imports System.Data.SqlClient
Imports System.Net.Mail
Module MdlLogin
    Public user As String
    Public gname As String
    Public sname As String
    Public mode As String
    Public password As String
    Public attempts As Integer = 0
    Public Sub loginaccess()
        attempts = attempts + 1
        Dim dr As SqlClient.SqlDataReader
        Dim cmd As New SqlClient.SqlCommand("select * from TblUsers where Username COLLATE Latin1_General_CS_AS= '" & FrmLogin.username.Text & "' and Password COLLATE Latin1_General_CS_AS = '" & FrmLogin.password.Text & "'", con)
        con.Open()
        dr = cmd.ExecuteReader
        If dr.Read() Then
            user = dr.GetValue(2).ToString()
            gname = dr.GetValue(1).ToString()
            sname = dr.GetValue(0).ToString()
            con.Close()
            MsgBox("Successful Login!")
            attempts = 0
            FrmLogin.Hide()
            FrmHome.Show()
            FrmLogin.username.Clear()
            FrmLogin.password.Clear()
        ElseIf attempts < 5 Then
            con.Close()
            MsgBox("Invalid Username or Password", MsgBoxStyle.Exclamation, "Error")
            FrmLogin.username.Clear()
            FrmLogin.password.Clear()
            FrmLogin.username.Focus()
        Else
            con.Open()
            Dim cmd1 As New SqlClient.SqlCommand("select * from TblUsers where Username COLLATE Latin1_General_CS_AS= '" & FrmLogin.username.Text & "'", con)
            dr = cmd1.ExecuteReader

            If dr.Read Then
                forgotpassword()
            Else
                MsgBox("Access denied, Unknown User Detected", MsgBoxStyle.Exclamation, "Error")
                FrmLogin.username.ReadOnly = True
                FrmLogin.password.ReadOnly = True
            End If
            con.Close()
        End If
        con.Close()
    End Sub
    Public Sub forgotpassword()
        con.Open()
        Dim cmd1 As New SqlClient.SqlCommand("select * from Accounts where Username COLLATE Latin1_General_CS_AS= '" & FrmLogin.username.Text & "'", con)
        dr = cmd1.ExecuteReader

        If dr.Read Then
            Dim newpass As String
            Dim Letters As New List(Of Integer)
            'add ASCII codes for numbers
            For i As Integer = 48 To 57
                Letters.Add(i)
            Next
            'lowercase letters
            For i As Integer = 97 To 122
                Letters.Add(i)
            Next
            'uppercase letters
            For i As Integer = 65 To 90
                Letters.Add(i)
            Next
            'select 8 random integers from number of items in Letters
            'then convert those random integers to characters and
            'add each to a string and display in Textbox
            Dim Rnd As New Random
            Dim SB As New System.Text.StringBuilder
            Dim Temp As Integer
            For count As Integer = 1 To 8
                Temp = Rnd.Next(0, Letters.Count)
                SB.Append(Chr(Letters(Temp)))
            Next

            newpass = SB.ToString
            Try
                Dim SmtpServer As New SmtpClient()
                Dim mail As New MailMessage()
                SmtpServer.EnableSsl = True
                SmtpServer.Credentials = New _
            Net.NetworkCredential("decsinquiry@gmail.com", "inquiry@decs")
                SmtpServer.Port = 587
                SmtpServer.Host = "smtp.gmail.com"
                mail = New MailMessage()
                mail.From = New MailAddress("YOURusername@gmail.com")
                mail.To.Add("nathan_19942001@yahoo.com")
                mail.Subject = "Test Mail"
                mail.Body = "New password for user " & newpass
                SmtpServer.Send(mail)
                MsgBox("mail send")

            Catch ex As Exception
                MsgBox(ex.ToString)
            End Try
        Else
            MsgBox("Invalid User", MsgBoxStyle.Exclamation, "Error")
            FrmLogin.username.ReadOnly = True
            FrmLogin.password.ReadOnly = True
        End If
        con.Close()


    End Sub
End Module
