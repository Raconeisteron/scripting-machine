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
        Dim key As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.ClassesRoot, pStyle As Drawing.FontStyle
        Dim tFont As Font
        Try
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
            If CheckBox1.Checked Then pStyle = FontStyle.Bold
            If CheckBox2.Checked Then pStyle += FontStyle.Italic
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
            For Each inst As Instance In Instances
                inst.Font = .cFont
            Next
        End With
        Me.Hide()
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
            CheckBox3.Checked = .Images
            CheckBox4.Checked = .Assoc
            CheckBox7.Checked = .ToolBar
            CheckBox8.Checked = .aSelect
            CheckBox9.Checked = .OETab
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
        End With
        Me.Hide()
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        RadioButton1.Checked = True
        CheckBox1.Checked = False
        CheckBox2.Checked = False
        CheckBox3.Checked = True
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

#Region "Visual"

    Private Sub CheckBox3_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox3.CheckedChanged
        If Not CheckBox3.Checked Then
            TextBox4.Enabled = True
            TextBox5.Enabled = True
            TextBox6.Enabled = True
            TextBox7.Enabled = True
            TextBox8.Enabled = True
        Else
            TextBox4.Enabled = False
            TextBox5.Enabled = False
            TextBox6.Enabled = False
            TextBox7.Enabled = False
            TextBox8.Enabled = False
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