Imports System.Text.RegularExpressions

Public Class eColor

#Region "Arrays"

    Dim tmp As String

#End Region

#Region "Text Restrictions"

    Private Sub TextBox1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox1.KeyPress
        If Not IsNumeric(e.KeyChar) And Not Char.IsControl(e.KeyChar) And Asc(e.KeyChar) <> 8 Then e.Handled = True
    End Sub

    Private Sub TextBox2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox2.KeyPress
        If Not IsNumeric(e.KeyChar) And Not Char.IsControl(e.KeyChar) And Asc(e.KeyChar) <> 8 Then e.Handled = True
    End Sub

    Private Sub TextBox3_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox3.KeyPress
        If Not IsNumeric(e.KeyChar) And Not Char.IsControl(e.KeyChar) And Asc(e.KeyChar) <> 8 Then e.Handled = True
    End Sub

    Private Sub TextBox4_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox4.KeyPress
        If Not IsNumeric(e.KeyChar) And Not Char.IsControl(e.KeyChar) And Asc(e.KeyChar) <> 8 Then e.Handled = True
    End Sub

#End Region

#Region "Color"

#Region "Tracks"

    Private Sub TrackBar1_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBar1.Scroll
        tmp = cColor(TrackBar4.Value, TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel1.BackColor)
        TextBox1.Text = TrackBar1.Value
    End Sub

    Private Sub TrackBar2_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBar2.Scroll
        tmp = cColor(TrackBar4.Value, TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel1.BackColor)
        TextBox2.Text = TrackBar2.Value
    End Sub

    Private Sub TrackBar3_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBar3.Scroll
        tmp = cColor(TrackBar4.Value, TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel1.BackColor)
        TextBox3.Text = TrackBar3.Value
    End Sub

    Private Sub TrackBar4_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBar4.Scroll
        tmp = cColor(TrackBar4.Value, TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel1.BackColor)
        TextBox4.Text = TrackBar4.Value
    End Sub

#End Region

#Region "Box"

    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.TextChanged
        TextBox1.Text = Regex.Replace(TextBox1.Text, BadChars, "")
        If Val(TextBox1.Text) <> TrackBar1.Value Then
            If Val(TextBox1.Text) > 255 Then TextBox1.Text = 255
            If Val(TextBox1.Text) < 0 Then TextBox1.Text = 0
            TrackBar1.Value = Val(TextBox1.Text)
            tmp = cColor(TrackBar4.Value, TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel1.BackColor)
        End If
    End Sub

    Private Sub TextBox2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox2.TextChanged
        TextBox2.Text = Regex.Replace(TextBox2.Text, BadChars, "")
        If Val(TextBox2.Text) <> TrackBar2.Value Then
            If Val(TextBox2.Text) > 255 Then TextBox2.Text = 255
            If Val(TextBox2.Text) < 0 Then TextBox2.Text = 0
            TrackBar2.Value = Val(TextBox2.Text)
            tmp = cColor(TrackBar4.Value, TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel1.BackColor)
        End If
    End Sub

    Private Sub TextBox3_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox3.TextChanged
        TextBox3.Text = Regex.Replace(TextBox3.Text, BadChars, "")
        If Val(TextBox3.Text) <> TrackBar3.Value Then
            If Val(TextBox3.Text) > 255 Then TextBox3.Text = 255
            If Val(TextBox3.Text) < 0 Then TextBox3.Text = 0
            TrackBar3.Value = Val(TextBox3.Text)
            tmp = cColor(TrackBar4.Value, TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel1.BackColor)
        End If
    End Sub

    Private Sub TextBox4_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox4.TextChanged
        TextBox4.Text = Regex.Replace(TextBox4.Text, BadChars, "")
        If Val(TextBox4.Text) <> TrackBar4.Value Then
            If Val(TextBox4.Text) > 255 Then TextBox4.Text = 255
            If Val(TextBox4.Text) < 0 Then TextBox4.Text = 0
            TrackBar4.Value = Val(TextBox4.Text)
            tmp = cColor(TrackBar4.Value, TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel1.BackColor)
        End If
    End Sub

#End Region

#End Region

#Region "Accept/Cancel"

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.Close()
        Main.Focus()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Select Case gSender
            Case CC.Msg
                Config.C_Msg.Name = tmp
                Config.C_Msg.Hex = Panel1.BackColor
                Main.Panel1.BackColor = Config.C_Msg.Hex
            Case CC.Help
                Config.C_Help.Name = tmp
                Config.C_Help.Hex = Panel1.BackColor
                Main.Panel2.BackColor = Config.C_Help.Hex
            Case CC.Gate_Close
                Config.C_Close.Name = tmp
                Config.C_Close.Hex = Panel1.BackColor
                Main.Panel4.BackColor = Config.C_Close.Hex
            Case CC.Gate_Open
                Config.C_Open.Name = tmp
                Config.C_Open.Hex = Panel1.BackColor
                Main.Panel3.BackColor = Config.C_Open.Hex
        End Select
        Me.Close()
        Main.Focus()
    End Sub

#End Region

End Class