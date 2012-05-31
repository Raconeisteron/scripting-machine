
Public Class Previewer

#Region "Load"

    Private Sub Previewer_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        PictureBox3.Image = GetImageFromGui(GuiImage.Scroll)
        PictureBox1.Image = GetImageFromGui(GuiImage.Button)
        PictureBox2.Image = GetImageFromGui(GuiImage.Button)
        PictureBox4.Image = GetImageFromGui(GuiImage.TextBox)
        PictureBox5.Image = GetImageFromGui(GuiImage.Dot)
        RichTextBox2.ForeColor = Color.FromArgb(255, 160, 160, 160)
    End Sub

#End Region

#Region "Closing"

    Private Sub Previewer_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LostFocus
        Me.Hide()
    End Sub

    Private Sub Previewer_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Click
        Me.Hide()
    End Sub

    Private Sub RichTextBox1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RichTextBox1.Click
        Me.Hide()
    End Sub

    Private Sub RichTextBox2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RichTextBox2.Click
        Me.Hide()
    End Sub

    Private Sub PictureBox1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox1.Click
        Me.Hide()
    End Sub

    Private Sub PictureBox2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Hide()
    End Sub

    Private Sub PictureBox3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox3.Click
        Me.Hide()
    End Sub

    Private Sub PictureBox4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox4.Click
        Me.Hide()
    End Sub

    Private Sub PictureBox5_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles PictureBox5.Click
        Me.Hide()
    End Sub

    Private Sub TextBox2_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.Hide()
    End Sub

    Private Sub TextBox3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox3.Click
        Me.Hide()
    End Sub

#End Region

#Region "Style"

    Private Sub Previewer_VisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.VisibleChanged
        If Me.Visible = True Then
            Me.TopMost = False
            Dim chars As Integer, linecount As Integer
            chars = GetCharCountFromLongestLine(RichTextBox1)
            linecount = RichTextBox1.Lines.Length
            Select Case Main.ComboBox2.Text
                Case "DIALOG_STYLE_MSGBOX"
                    If chars > 80 Then
                        chars = 80
                    ElseIf chars < 32 Then
                        chars = 0
                    End If
                    If linecount = 1 Then linecount = 0
                    PictureBox3.Visible = False
                    PictureBox4.Visible = False
                    PictureBox5.Visible = False
                    Me.Size = New Point(350 + 2.3 * chars, 109 + 11 * linecount)
                    If Main.TextBox42.Text = "" Then
                        TextBox2.Visible = False
                        PictureBox2.Visible = False
                        TextBox3.Location = New Point(Me.Width / 2 - 50, Me.Height - 40)
                        PictureBox1.Location = New Point(Me.Width / 2 - 55.5, Me.Height - 44)
                    Else
                        TextBox2.Visible = True
                        TextBox2.Location = New Point(Me.Width * (2 / 3) - TextBox2.Width / 2, Me.Height - 40)
                        PictureBox2.Location = New Point(Me.Width * (2 / 3) - TextBox2.Width / 2 - 5, Me.Height - 44)
                        PictureBox2.Visible = True
                        TextBox3.Location = New Point(Me.Width / 3 - TextBox3.Width / 2, Me.Height - 40)
                        PictureBox1.Location = New Point(Me.Width / 3 - TextBox3.Width / 2 - 5, Me.Height - 44)
                    End If
                    RichTextBox1.BorderStyle = BorderStyle.None
                    RichTextBox1.Font = New Font("Arial Rounded MT Bold", 9, FontStyle.Regular)
                    RichTextBox1.ForeColor = Color.FromArgb(255, 169, 196, 228)
                    RichTextBox1.Location = New Point(35, 27)
                    RichTextBox1.Size = New Point(Me.Width - 70, Me.Height - 78)
                Case "DIALOG_STYLE_INPUT"
                    If chars > 80 Then
                        chars = 80
                    ElseIf chars < 32 Then
                        chars = 0
                    End If
                    If linecount = 1 Then linecount = 0
                    PictureBox3.Visible = False
                    PictureBox4.Visible = True
                    PictureBox5.Visible = False
                    Me.Size = New Point(350 + 2.3 * chars, 151 + 11 * linecount)
                    If Main.TextBox42.Text = "" Then
                        TextBox2.Visible = False
                        PictureBox2.Visible = False
                        TextBox3.Location = New Point(Me.Width / 2 - 50, Me.Height - 37)
                        PictureBox1.Location = New Point(Me.Width / 2 - 55.5, Me.Height - 41)
                    Else
                        TextBox2.Visible = True
                        TextBox2.Location = New Point(Me.Width * (2 / 3) - TextBox2.Width / 2, Me.Height - 40)
                        PictureBox2.Location = New Point(Me.Width * (2 / 3) - TextBox2.Width / 2 - 5, Me.Height - 44)
                        PictureBox2.Visible = True
                        TextBox3.Location = New Point(Me.Width / 3 - TextBox3.Width / 2, Me.Height - 40)
                        PictureBox1.Location = New Point(Me.Width / 3 - TextBox3.Width / 2 - 5, Me.Height - 44)
                    End If
                    PictureBox4.Location = New Point(12, 63 + 11 * linecount)
                    PictureBox4.Size = New Point(Me.Width - 24, 32)
                    RichTextBox1.BorderStyle = BorderStyle.None
                    RichTextBox1.Font = New Font("Arial Rounded MT Bold", 9, FontStyle.Regular)
                    RichTextBox1.ForeColor = Color.FromArgb(255, 169, 196, 228)
                    RichTextBox1.Location = New Point(35, 27)
                    RichTextBox1.Size = New Point(Me.Width - 70, Me.Height - 78)
                Case "DIALOG_STYLE_LIST"
                    If chars > 80 Then
                        chars = 80
                    ElseIf chars < 6 Then
                        chars = 0
                    End If
                    PictureBox3.Visible = True
                    PictureBox4.Visible = False
                    PictureBox5.Visible = False
                    If Main.TextBox42.Text = "" Then
                        TextBox2.Visible = False
                        PictureBox2.Visible = False
                        TextBox3.Location = New Point(Me.Width / 2 - 50, 245)
                        PictureBox1.Location = New Point(Me.Width / 2 - 55.5, 241)
                    Else
                        TextBox2.Visible = True
                        TextBox2.Location = New Point(Me.Width * (2 / 3) - TextBox2.Width / 2, 245)
                        PictureBox2.Location = New Point(Me.Width * (2 / 3) - TextBox2.Width / 2 - 5, 241)
                        PictureBox2.Visible = True
                        TextBox3.Location = New Point(Me.Width / 3 - TextBox3.Width / 2, 245)
                        PictureBox1.Location = New Point(Me.Width / 3 - TextBox3.Width / 2 - 5, 241)
                    End If
                    Me.Size = New Point(350 + 2.3 * chars, 275)
                    RichTextBox2.Location = New Point(-1, 0)
                    RichTextBox2.Size = New Point(Me.Width, 23)
                    RichTextBox1.BorderStyle = BorderStyle.Fixed3D
                    RichTextBox1.Font = New Font("Arial Rounded MT Bold", 11, FontStyle.Regular)
                    RichTextBox1.ForeColor = Color.White
                    RichTextBox1.Location = New Point(2, 27)
                    RichTextBox1.Size = New Point(Me.Width - 4, Me.Height - 73)
                    PictureBox3.Location = New Point(Me.Width - 24, 28)
                Case "DIALOG_STYLE_PASSWORD"
                    If chars > 80 Then
                        chars = 80
                    ElseIf chars < 32 Then
                        chars = 0
                    End If
                    If linecount = 1 Then linecount = 0
                    PictureBox3.Visible = False
                    PictureBox4.Visible = True
                    PictureBox5.Visible = True
                    Me.Size = New Point(350 + 2.3 * chars, 151 + 11 * linecount)
                    If Main.TextBox42.Text = "" Then
                        TextBox2.Visible = False
                        PictureBox2.Visible = False
                        TextBox3.Location = New Point(Me.Width / 2 - 50, Me.Height - 37)
                        PictureBox1.Location = New Point(Me.Width / 2 - 55.5, Me.Height - 41)
                    Else
                        TextBox2.Visible = True
                        TextBox2.Location = New Point(Me.Width * (2 / 3) - TextBox2.Width / 2, Me.Height - 40)
                        PictureBox2.Location = New Point(Me.Width * (2 / 3) - TextBox2.Width / 2 - 5, Me.Height - 44)
                        PictureBox2.Visible = True
                        TextBox3.Location = New Point(Me.Width / 3 - TextBox3.Width / 2, Me.Height - 40)
                        PictureBox1.Location = New Point(Me.Width / 3 - TextBox3.Width / 2 - 5, Me.Height - 44)
                    End If
                    PictureBox4.Location = New Point(12, 63 + 11 * linecount)
                    PictureBox4.Size = New Point(Me.Width - 24, 32)
                    PictureBox5.Location = New Point(25, 75 + 11 * linecount)
                    RichTextBox1.BorderStyle = BorderStyle.None
                    RichTextBox1.Font = New Font("Arial Rounded MT Bold", 9, FontStyle.Regular)
                    RichTextBox1.ForeColor = Color.FromArgb(255, 169, 196, 228)
                    RichTextBox1.Location = New Point(35, 27)
                    RichTextBox1.Size = New Point(Me.Width - 70, Me.Height - 78)
            End Select
        End If
        TextBox2.Focus()
    End Sub

#End Region

End Class