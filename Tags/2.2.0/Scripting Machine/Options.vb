'Copyright (C) 2011-2012  The_Chaoz
'
'   This file is part of Scripting Machine.
'   Scripting Machine is free software: you can redistribute it and/or modify
'   it under the terms of the GNU General Public License as published by
'   the Free Software Foundation, either version 3 of the License, or
'   (at your option) any later version.
'
'   Scripting Machine is distributed in the hope that it will be useful,
'   but WITHOUT ANY WARRANTY; without even the implied warranty of
'   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
'   GNU General Public License for more details.
'
'   You should have received a copy of the GNU General Public License
'   along with Scripting Machine.  If not, see <http://www.gnu.org/licenses/>.

Public Class Options

#Region "Buttons"

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim key As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.ClassesRoot
        Dim tFont As Font
        Try
            Dim pStyle As FontStyle
            If CheckBox1.Checked Then pStyle = FontStyle.Bold
            If CheckBox2.Checked Then pStyle += FontStyle.Italic
            tFont = New Font(New FontFamily(ComboBox1.Text), ComboBox2.Text, pStyle, GraphicsUnit.Pixel)
        Catch ex As Exception
            Select Case Settings.Language
                Case Languages.English
                    MsgBox("Invalid font family or size", MsgBoxStyle.Critical, "Error")
                Case Languages.Español
                    MsgBox("Fuente o tamaño de letra invalidos", MsgBoxStyle.Critical, "Error")
                Case Languages.Portuguêse
                    MsgBox("Fonte Tamanho da fonte ou inválido", MsgBoxStyle.Critical, "Error")
                Case Else
                    MsgBox("Schriftgröße verändern Schriftgröße oder ungültig", MsgBoxStyle.Critical, "Fehler")
            End Select
            Exit Sub
        End Try
        With Settings
            If RadioButton1.Checked Then
                .Language = Languages.English
            ElseIf RadioButton2.Checked Then
                .Language = Languages.Español
            ElseIf RadioButton3.Checked Then
                .Language = Languages.Portuguêse
            Else
                .Language = Languages.Deutsch
            End If
            ChangeLang(.Language)
            .cFont = tFont
            .AreaCreateOutput = TextBox1.Text
            .AreaShowOutput = TextBox2.Text
            .BoundsOutput = TextBox3.Text
            If CheckBox4.Checked Then
                .Assoc = True
                If Not key.OpenSubKey(".pwn") Is Nothing Then key.DeleteSubKeyTree(".pwn")
                key.CreateSubKey(".pwn").SetValue("", ".pwn", Microsoft.Win32.RegistryValueKind.String)
                key.CreateSubKey(".pwn\shell\open\command").SetValue("", Application.ExecutablePath & " ""%l"" ", Microsoft.Win32.RegistryValueKind.String)
                If Not key.OpenSubKey(".inc") Is Nothing Then key.DeleteSubKeyTree(".inc")
                key.CreateSubKey(".inc").SetValue("", ".inc", Microsoft.Win32.RegistryValueKind.String)
                key.CreateSubKey(".inc\shell\open\command").SetValue("", Application.ExecutablePath & " ""%l"" ", Microsoft.Win32.RegistryValueKind.String)
            Else
                .Assoc = False
                If Not key.OpenSubKey(".pwn") Is Nothing Then key.DeleteSubKeyTree(".pwn")
                If Not key.OpenSubKey(".inc") Is Nothing Then key.DeleteSubKeyTree(".inc")
            End If
            If RadioButton5.Checked Then
                .Images = Imgs.iDefault
            ElseIf RadioButton6.Checked Then
                .Images = Imgs.iFolder
            Else
                .Images = Imgs.iURL
            End If
            .URL_Skin = TextBox4.Text
            .URL_Veh = TextBox5.Text
            .URL_Weap = TextBox6.Text
            .URL_Map = TextBox7.Text
            .URL_Sprite = TextBox8.Text
            .iTabs = CheckBox5.Checked
            Main.TabControl2.Visible = .iTabs
            If CheckBox6.Checked Then
                .CompDefPath = True
                .CompPath = My.Application.Info.DirectoryPath & "\pawncc.exe"
            Else
                If TextBox9.Text.Length > 0 Then
                    .CompDefPath = False
                    .CompPath = TextBox9.Text
                Else
                    CheckBox6.Checked = False
                    .CompDefPath = True
                    .CompPath = My.Application.Info.DirectoryPath & "\pawncc.exe"
                End If
            End If
            .CompArgs = TextBox10.Text
            If CheckBox7.Checked Then
                .ToolBar = True
                Main.ToolStrip1.Visible = True
            Else
                .ToolBar = False
                Main.ToolStrip1.Visible = False
            End If
            If CheckBox9.Checked Then
                .OETab = True
                Main.TabControl3.Visible = True
            Else
                .OETab = False
                Main.TabControl3.Visible = False
            End If
            .aSelect = CheckBox8.Checked

            With .H_Numbers
                .BackColor = Panel2.BackColor
                .ForeColor = Panel1.BackColor
                .Bold = CheckBox10.Checked
                .Italic = CheckBox11.Checked
            End With
            With .H_String
                .BackColor = Panel3.BackColor
                .ForeColor = Panel4.BackColor
                .Bold = CheckBox13.Checked
                .Italic = CheckBox12.Checked
            End With
            With .H_String2
                .BackColor = Panel5.BackColor
                .ForeColor = Panel6.BackColor
                .Bold = CheckBox15.Checked
                .Italic = CheckBox14.Checked
            End With
            With .H_Operator
                .BackColor = Panel7.BackColor
                .ForeColor = Panel8.BackColor
                .Bold = CheckBox17.Checked
                .Italic = CheckBox16.Checked
            End With
            With .H_Chars
                .BackColor = Panel9.BackColor
                .ForeColor = Panel10.BackColor
                .Bold = CheckBox19.Checked
                .Italic = CheckBox18.Checked
            End With
            With .H_Class
                .BackColor = Panel11.BackColor
                .ForeColor = Panel12.BackColor
                .Bold = CheckBox21.Checked
                .Italic = CheckBox20.Checked
            End With
            With .H_Comment
                .BackColor = Panel14.BackColor
                .ForeColor = Panel13.BackColor
                .Bold = CheckBox22.Checked
                .Italic = CheckBox3.Checked
            End With
            .BackColor = Panel15.BackColor

            If RadioButton8.Checked Then
                .Enc = System.Text.Encoding.UTF8
            ElseIf RadioButton9.Checked Then
                .Enc = System.Text.Encoding.BigEndianUnicode
            ElseIf RadioButton10.Checked Then
                .Enc = System.Text.Encoding.ASCII
            Else
                .Enc = System.Text.Encoding.Unicode
            End If

            For Each inst As Instance In Instances
                inst.Font = .cFont
                inst.SyntaxHandle.Encoding = .Enc
                inst.SyntaxHandle.BackColor = .BackColor
                With .H_Numbers
                    inst.SyntaxHandle.Styles("NUMBER").BackColor = .BackColor
                    inst.SyntaxHandle.Styles("NUMBER").ForeColor = .ForeColor
                    inst.SyntaxHandle.Styles("NUMBER").Bold = .Bold
                    inst.SyntaxHandle.Styles("NUMBER").Italic = .Italic
                End With
                With .H_String
                    inst.SyntaxHandle.Styles("STRING").BackColor = .BackColor
                    inst.SyntaxHandle.Styles("STRING").ForeColor = .ForeColor
                    inst.SyntaxHandle.Styles("STRING").Bold = .Bold
                    inst.SyntaxHandle.Styles("STRING").Italic = .Italic
                End With
                With .H_String2
                    inst.SyntaxHandle.Styles("STRINGEOL").BackColor = .BackColor
                    inst.SyntaxHandle.Styles("STRINGEOL").ForeColor = .ForeColor
                    inst.SyntaxHandle.Styles("STRINGEOL").Bold = .Bold
                    inst.SyntaxHandle.Styles("STRINGEOL").Italic = .Italic
                End With
                With .H_Operator
                    inst.SyntaxHandle.Styles("OPERATOR").BackColor = .BackColor
                    inst.SyntaxHandle.Styles("OPERATOR").ForeColor = .ForeColor
                    inst.SyntaxHandle.Styles("OPERATOR").Bold = .Bold
                    inst.SyntaxHandle.Styles("OPERATOR").Italic = .Italic
                End With
                With .H_Chars
                    inst.SyntaxHandle.Styles("CHARACTER").BackColor = .BackColor
                    inst.SyntaxHandle.Styles("CHARACTER").ForeColor = .ForeColor
                    inst.SyntaxHandle.Styles("CHARACTER").Bold = .Bold
                    inst.SyntaxHandle.Styles("CHARACTER").Italic = .Italic
                End With
                With .H_Class
                    inst.SyntaxHandle.Styles("GLOBALCLASS").BackColor = .BackColor
                    inst.SyntaxHandle.Styles("GLOBALCLASS").ForeColor = .ForeColor
                    inst.SyntaxHandle.Styles("GLOBALCLASS").Font = inst.Font
                    inst.SyntaxHandle.Styles("GLOBALCLASS").Bold = .Bold
                    inst.SyntaxHandle.Styles("GLOBALCLASS").Italic = .Italic
                End With
                With .H_Comment
                    inst.SyntaxHandle.Styles("COMMENT").BackColor = .BackColor
                    inst.SyntaxHandle.Styles("COMMENT").ForeColor = .ForeColor
                    inst.SyntaxHandle.Styles("COMMENT").Bold = .Bold
                    inst.SyntaxHandle.Styles("COMMENT").Italic = .Italic
                    inst.SyntaxHandle.Styles("COMMENTLINE").BackColor = .BackColor
                    inst.SyntaxHandle.Styles("COMMENTLINE").ForeColor = .ForeColor
                    inst.SyntaxHandle.Styles("COMMENTLINE").Bold = .Bold
                    inst.SyntaxHandle.Styles("COMMENTLINE").Italic = .Italic
                    inst.SyntaxHandle.Styles("COMMENTDOC").BackColor = .BackColor
                    inst.SyntaxHandle.Styles("COMMENTDOC").ForeColor = .ForeColor
                    inst.SyntaxHandle.Styles("COMMENTDOC").Bold = .Bold
                    inst.SyntaxHandle.Styles("COMMENTDOC").Italic = .Italic
                End With
                inst.SyntaxHandle.Lexing.Colorize()
            Next
        End With
        Me.Hide()
        Me.Owner.Refresh()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        With Settings
            Select Case .Language
                Case Languages.English
                    RadioButton1.Checked = True
                Case Languages.Español
                    RadioButton2.Checked = True
                Case Languages.Portuguêse
                    RadioButton3.Checked = True
                Case Else
                    RadioButton4.Checked = True
            End Select
            ChangeLang(.Language)
            CheckBox1.Checked = .cFont.Bold
            CheckBox2.Checked = .cFont.Italic
            If RadioButton5.Checked Then
                .Images = Imgs.iDefault
            ElseIf RadioButton6.Checked Then
                .Images = Imgs.iFolder
            Else
                .Images = Imgs.iURL
            End If
            CheckBox4.Checked = .Assoc
            CheckBox7.Checked = .ToolBar
            CheckBox8.Checked = .aSelect
            CheckBox9.Checked = .OETab
            Select Case .Images
                Case Imgs.iDefault
                    RadioButton5.Checked = True
                Case Imgs.iFolder
                    RadioButton6.Checked = True
                Case Else
                    RadioButton7.Checked = True
            End Select
            TextBox1.Text = .AreaCreateOutput
            TextBox2.Text = .AreaShowOutput
            TextBox3.Text = .BoundsOutput
            TextBox4.Text = .URL_Skin
            TextBox5.Text = .URL_Veh
            TextBox6.Text = .URL_Weap
            TextBox7.Text = .URL_Map
            TextBox8.Text = .URL_Sprite
            If .CompDefPath Then
                CheckBox6.Checked = True
            Else
                CheckBox6.Checked = False
                TextBox9.Text = .CompPath
            End If
            TextBox10.Text = .CompArgs
            With .H_Numbers
                Panel2.BackColor = .BackColor
                Panel1.BackColor = .ForeColor
                CheckBox10.Checked = .Bold
                CheckBox11.Checked = .Italic
            End With
            With .H_String
                Panel3.BackColor = .BackColor
                Panel4.BackColor = .ForeColor
                CheckBox13.Checked = .Bold
                CheckBox12.Checked = .Italic
            End With
            With .H_String2
                Panel5.BackColor = .BackColor
                Panel6.BackColor = .ForeColor
                CheckBox15.Checked = .Bold
                CheckBox14.Checked = .Italic
            End With
            With .H_Operator
                Panel7.BackColor = .BackColor
                Panel8.BackColor = .ForeColor
                CheckBox17.Checked = .Bold
                CheckBox16.Checked = .Italic
            End With
            With .H_Chars
                Panel9.BackColor = .BackColor
                Panel10.BackColor = .ForeColor
                CheckBox19.Checked = .Bold
                CheckBox18.Checked = .Italic
            End With
            With .H_Class
                Panel11.BackColor = .BackColor
                Panel12.BackColor = .ForeColor
                CheckBox21.Checked = .Bold
                CheckBox20.Checked = .Italic
            End With
            With .H_Comment
                Panel13.BackColor = .ForeColor
                Panel14.BackColor = .BackColor
                CheckBox22.Checked = .Bold
                CheckBox3.Checked = .Italic
            End With
            If .Enc Is System.Text.Encoding.UTF8 Then
                RadioButton8.Checked = True
            ElseIf .Enc Is System.Text.Encoding.BigEndianUnicode Then
                RadioButton9.Checked = True
            ElseIf .Enc Is System.Text.Encoding.ASCII Then
                RadioButton10.Checked = True
            Else
                RadioButton11.Checked = True
            End If
            Panel15.BackColor = .BackColor
        End With
        Me.Hide()
        Me.Owner.Refresh()
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        RadioButton1.Checked = True
        CheckBox1.Checked = False
        CheckBox2.Checked = False
        RadioButton5.Checked = True
        CheckBox4.Checked = True
        CheckBox5.Checked = True
        CheckBox7.Checked = True
        CheckBox8.Checked = False
        CheckBox9.Checked = True
        ComboBox2.SelectedItem = 4
        ComboBox1.SelectedItem = 0
        TextBox1.Text = "GangZoneCreate({0}, {1}, {2}, {3});"
        TextBox2.Text = "GangZoneShowForAll({5}, {6});"
        TextBox3.Text = "SetPlayerWorldBounds(playerid, {0}, {1}, {2}, {3});"
        RadioButton1.Checked = True
        TextBox4.Text = Settings.URL_Skin
        TextBox5.Clear()
        TextBox6.Clear()
        TextBox7.Clear()
        TextBox8.Clear()
        TextBox9.Clear()
        CheckBox6.Checked = True
        TextBox10.Text = "-r"
        RadioButton8.Checked = True

        Panel1.BackColor = Color.FromArgb(255, 27, 124, 143)
        Panel2.BackColor = Color.White
        CheckBox10.Checked = False
        CheckBox11.Checked = False
        Panel4.BackColor = Color.FromArgb(255, 92, 32, 153)
        Panel3.BackColor = Color.White
        CheckBox13.Checked = False
        CheckBox12.Checked = False
        Panel6.BackColor = Color.FromArgb(255, 237, 21, 180)
        Panel5.BackColor = Color.FromArgb(255, 180, 240, 227)
        CheckBox15.Checked = False
        CheckBox14.Checked = True
        Panel8.BackColor = Color.FromArgb(255, 17, 112, 72)
        Panel7.BackColor = Color.White
        CheckBox17.Checked = False
        CheckBox16.Checked = False
        Panel10.BackColor = Color.FromArgb(255, 17, 95, 112)
        Panel9.BackColor = Color.White
        CheckBox19.Checked = False
        CheckBox18.Checked = False
        Panel12.BackColor = Color.FromArgb(255, 232, 19, 204)
        Panel11.BackColor = Color.White
        CheckBox21.Checked = False
        CheckBox20.Checked = False
        Panel13.BackColor = Color.FromArgb(255, 0, 160, 0)
        Panel14.BackColor = Color.White
        CheckBox22.Checked = False
        CheckBox3.Checked = False
        Panel15.BackColor = Color.White
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        With OFD
            If Settings.CompDefPath Then
                .InitialDirectory = Settings.DefaultPath
            Else
                .InitialDirectory = My.Application.Info.DirectoryPath
            End If
            .Filter = "Executable Files|*.exe"
            .ShowDialog()
            If .FileName.Length > 0 Then TextBox9.Text = .FileName
        End With
    End Sub

#End Region

#Region "Customize"

    Private Sub Panel1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Panel1.Click
        gSender = CC.H_NF
        With eColor
            .Show()
            .Panel1.BackColor = Panel1.BackColor
            .TrackBar1.Value = Panel1.BackColor.R
            .TrackBar2.Value = Panel1.BackColor.G
            .TrackBar3.Value = Panel1.BackColor.B
            .TrackBar4.Value = Panel1.BackColor.A
        End With
    End Sub

    Private Sub Panel2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Panel2.Click
        gSender = CC.H_NB
        With eColor
            .Show()
            .Panel1.BackColor = Panel2.BackColor
            .TrackBar1.Value = Panel2.BackColor.R
            .TrackBar2.Value = Panel2.BackColor.G
            .TrackBar3.Value = Panel2.BackColor.B
            .TrackBar4.Value = Panel2.BackColor.A
        End With
    End Sub

    Private Sub Panel3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Panel3.Click
        gSender = CC.H_SB
        With eColor
            .Show()
            .Panel1.BackColor = Panel3.BackColor
            .TrackBar1.Value = Panel3.BackColor.R
            .TrackBar2.Value = Panel3.BackColor.G
            .TrackBar3.Value = Panel3.BackColor.B
            .TrackBar4.Value = Panel3.BackColor.A
        End With
    End Sub

    Private Sub Panel4_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Panel4.Click
        gSender = CC.H_SF
        With eColor
            .Show()
            .Panel1.BackColor = Panel4.BackColor
            .TrackBar1.Value = Panel4.BackColor.R
            .TrackBar2.Value = Panel4.BackColor.G
            .TrackBar3.Value = Panel4.BackColor.B
            .TrackBar4.Value = Panel4.BackColor.A
        End With
    End Sub

    Private Sub Panel5_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Panel5.Click
        gSender = CC.H_S2B
        With eColor
            .Show()
            .Panel1.BackColor = Panel5.BackColor
            .TrackBar1.Value = Panel5.BackColor.R
            .TrackBar2.Value = Panel5.BackColor.G
            .TrackBar3.Value = Panel5.BackColor.B
            .TrackBar4.Value = Panel5.BackColor.A
        End With
    End Sub

    Private Sub Panel6_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Panel6.Click
        gSender = CC.H_S2F
        With eColor
            .Show()
            .Panel1.BackColor = Panel6.BackColor
            .TrackBar1.Value = Panel6.BackColor.R
            .TrackBar2.Value = Panel6.BackColor.G
            .TrackBar3.Value = Panel6.BackColor.B
            .TrackBar4.Value = Panel6.BackColor.A
        End With
    End Sub

    Private Sub Panel7_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Panel7.Click
        gSender = CC.H_OB
        With eColor
            .Show()
            .Panel1.BackColor = Panel7.BackColor
            .TrackBar1.Value = Panel7.BackColor.R
            .TrackBar2.Value = Panel7.BackColor.G
            .TrackBar3.Value = Panel7.BackColor.B
            .TrackBar4.Value = Panel7.BackColor.A
        End With
    End Sub

    Private Sub Panel8_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Panel8.Click
        gSender = CC.H_OF
        With eColor
            .Show()
            .Panel1.BackColor = Panel8.BackColor
            .TrackBar1.Value = Panel8.BackColor.R
            .TrackBar2.Value = Panel8.BackColor.G
            .TrackBar3.Value = Panel8.BackColor.B
            .TrackBar4.Value = Panel8.BackColor.A
        End With
    End Sub

    Private Sub Panel9_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Panel9.Click
        gSender = CC.H_CHB
        With eColor
            .Show()
            .Panel1.BackColor = Panel9.BackColor
            .TrackBar1.Value = Panel9.BackColor.R
            .TrackBar2.Value = Panel9.BackColor.G
            .TrackBar3.Value = Panel9.BackColor.B
            .TrackBar4.Value = Panel9.BackColor.A
        End With
    End Sub

    Private Sub Panel10_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Panel10.Click
        gSender = CC.H_CHF
        With eColor
            .Show()
            .Panel1.BackColor = Panel10.BackColor
            .TrackBar1.Value = Panel10.BackColor.R
            .TrackBar2.Value = Panel10.BackColor.G
            .TrackBar3.Value = Panel10.BackColor.B
            .TrackBar4.Value = Panel10.BackColor.A
        End With
    End Sub

    Private Sub Panel11_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Panel11.Click
        gSender = CC.H_CLB
        With eColor
            .Show()
            .Panel1.BackColor = Panel11.BackColor
            .TrackBar1.Value = Panel11.BackColor.R
            .TrackBar2.Value = Panel11.BackColor.G
            .TrackBar3.Value = Panel11.BackColor.B
            .TrackBar4.Value = Panel11.BackColor.A
        End With
    End Sub

    Private Sub Panel12_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Panel12.Click
        gSender = CC.H_CLF
        eColor.Show()
        eColor.Panel1.BackColor = Panel12.BackColor
        eColor.TrackBar1.Value = Panel12.BackColor.R
        eColor.TrackBar2.Value = Panel12.BackColor.G
        eColor.TrackBar3.Value = Panel12.BackColor.B
        eColor.TrackBar4.Value = Panel12.BackColor.A
    End Sub

    Private Sub Panel13_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Panel13.Click
        gSender = CC.H_CMF
        With eColor
            .Show()
            .Panel1.BackColor = Panel13.BackColor
            .TrackBar1.Value = Panel13.BackColor.R
            .TrackBar2.Value = Panel13.BackColor.G
            .TrackBar3.Value = Panel13.BackColor.B
            .TrackBar4.Value = Panel13.BackColor.A
        End With
    End Sub

    Private Sub Panel14_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Panel14.Click
        gSender = CC.H_CMB
        With eColor
            .Show()
            .Panel1.BackColor = Panel14.BackColor
            .TrackBar1.Value = Panel14.BackColor.R
            .TrackBar2.Value = Panel14.BackColor.G
            .TrackBar3.Value = Panel14.BackColor.B
            .TrackBar4.Value = Panel14.BackColor.A
        End With
    End Sub

    Private Sub Panel15_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Panel15.Click
        gSender = CC.Back
        With eColor
            .Show()
            .Panel1.BackColor = Panel15.BackColor
            .TrackBar1.Value = Panel15.BackColor.R
            .TrackBar2.Value = Panel15.BackColor.G
            .TrackBar3.Value = Panel15.BackColor.B
            .TrackBar4.Value = Panel15.BackColor.A
        End With
    End Sub

#End Region

#Region "Visual"

    Private Sub RadioButton6_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton6.CheckedChanged
        If RadioButton6.Checked OrElse RadioButton7.Checked Then
            Label3.Enabled = True
            Label4.Enabled = True
            Label5.Enabled = True
            Label6.Enabled = True
            Label7.Enabled = True
            TextBox4.Enabled = True
            TextBox5.Enabled = True
            TextBox6.Enabled = True
            TextBox7.Enabled = True
            TextBox8.Enabled = True
        Else
            Label3.Enabled = False
            Label4.Enabled = False
            Label5.Enabled = False
            Label6.Enabled = False
            Label7.Enabled = False
            TextBox4.Enabled = False
            TextBox5.Enabled = False
            TextBox6.Enabled = False
            TextBox7.Enabled = False
            TextBox8.Enabled = False
        End If
    End Sub

    Private Sub RadioButton7_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton7.CheckedChanged
        If RadioButton5.Checked OrElse RadioButton6.Checked Then
            Label3.Enabled = True
            Label4.Enabled = True
            Label5.Enabled = True
            Label6.Enabled = True
            Label7.Enabled = True
            TextBox4.Enabled = False
            TextBox5.Enabled = False
            TextBox6.Enabled = False
            TextBox7.Enabled = False
            TextBox8.Enabled = False
        Else
            Label3.Enabled = False
            Label4.Enabled = False
            Label5.Enabled = False
            Label6.Enabled = False
            Label7.Enabled = False
            TextBox4.Enabled = True
            TextBox5.Enabled = True
            TextBox6.Enabled = True
            TextBox7.Enabled = True
            TextBox8.Enabled = True
        End If
    End Sub

    Private Sub CheckBox6_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox6.CheckedChanged
        If CheckBox6.Checked Then
            TextBox9.Clear()
            TextBox9.Enabled = False
            Button4.Enabled = False
        Else
            TextBox9.Enabled = True
            TextBox9.Text = Settings.CompPath
            Button4.Enabled = True
        End If
    End Sub

#End Region

#Region "Languajes"

    Private Sub RadioButton1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton1.CheckedChanged
        If RadioButton1.Checked Then ChangeLang(Languages.English)
    End Sub

    Private Sub RadioButton2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton2.CheckedChanged
        If RadioButton2.Checked Then ChangeLang(Languages.Español)
    End Sub

    Private Sub RadioButton3_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton3.CheckedChanged
        If RadioButton3.Checked Then ChangeLang(Languages.Portuguêse)
    End Sub

    Private Sub RadioButton4_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton4.CheckedChanged
        If RadioButton4.Checked Then ChangeLang(Languages.Deutsch)
    End Sub

#End Region

End Class