Imports System.Security.Cryptography


Public Class Form1

    Private Const PWD_ALLOWED_CHARS As String = _
   "abcdefghijkmnopqrstuvwxyz" & _
   "ABCDEFGHJKLMNOPQRSTUVWXYZ" & _
   "0123456789!@#$%^&*(){}[]|\~`<>?"

    Private Sub Error_Handler(ByVal except As Exception, Optional ByVal message As String = "")
        Try
            MsgBox("The following error was trapped: (" & message & ") " & vbCrLf & except.ToString, MsgBoxStyle.Critical, "Critical Error")
        Catch ex As Exception
            MsgBox("The Error Handler failed due to: " & vbCrLf & ex.ToString, MsgBoxStyle.Critical, "Critical Error")
        End Try
    End Sub

    Private Function GenerateStrongPassword( _
       ByVal PasswordLength As Integer) As String
        ' Convert the string containing allowed
        ' characters into an array of bytes. If you 
        ' were going to call this code often, you 
        ' might consider making these variables
        ' static:
        Dim encoder As New System.Text.ASCIIEncoding
        Dim abytAllowedChars() As Byte = _
         encoder.GetBytes(PWD_ALLOWED_CHARS)

        ' Use the RNGCryptoServiceProvider to get a 
        ' crytographically strong set of random bytes.
        Dim abytRand(PasswordLength) As Byte
        Dim rng As RNGCryptoServiceProvider = _
           New RNGCryptoServiceProvider
        rng.GetBytes(abytRand)

        ' Calculate the number of allowed characters.
        Dim intChars As Integer = abytAllowedChars.Length

        ' Build password.
        Dim abytOutput(PasswordLength) As Byte
        For intLoop As Integer = 0 To PasswordLength - 1
            abytOutput(intLoop) = _
               abytAllowedChars(abytRand(intLoop) Mod intChars)
        Next
        Return encoder.GetString(abytOutput)
    End Function

    Private Sub GenerateValues()
        Try
            If RadioButton1.Checked = True Then
                TextBox1.Text = GenerateStrongPassword(5)
            End If
            If RadioButton2.Checked = True Then
                TextBox1.Text = GenerateStrongPassword(13)
            End If
            If RadioButton3.Checked = True Then
                If IsNumeric(TextBox3.Text) = True Then
                    TextBox1.Text = GenerateStrongPassword(Integer.Parse(TextBox3.Text))
                End If
            End If

            Dim result As String = ""
            Dim chr As Char

            For Each chr In TextBox1.Text.ToCharArray
                result = result & (Hex(Asc(chr)))
            Next
            TextBox2.Tag = result
            If RadioButton1.Checked = True Then
                If result.Length > 10 Then
                    TextBox2.Text = result.Substring(0, 10)
                Else
                    TextBox2.Text = result
                End If
            End If
            If RadioButton2.Checked = True Then
                If result.Length > 26 Then
                    TextBox2.Text = result.Substring(0, 26)
                Else
                    TextBox2.Text = result
                End If
            End If
            If RadioButton3.Checked = True Then
                TextBox2.Text = result
            End If
            TextBox2.Text = TextBox2.Text.ToLower()
            counter1.Text = TextBox1.Text.Length
            counter2.Text = TextBox2.Text.Length
        Catch ex As Exception
            Error_Handler(ex, "GenerateValues")
        End Try
    End Sub



    Private Sub RadioButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton1.CheckedChanged, RadioButton2.CheckedChanged, RadioButton3.CheckedChanged
        Try
            GenerateValues()
        Catch ex As Exception
            Error_Handler(ex, "InputCapture")
        End Try
    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GenerateValues()
    End Sub

    Private Sub GenerateButtonClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Try
            GenerateValues()
        Catch ex As Exception
            Error_Handler(ex, "GenerateButtonClick")
        End Try
    End Sub
End Class

