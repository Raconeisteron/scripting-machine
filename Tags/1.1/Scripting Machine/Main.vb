'Copyright (C) 2011  The_Chaoz
'
'    This program is free software: you can redistribute it and/or modify
'    it under the terms of the GNU General Public License as published by
'    the Free Software Foundation, either version 3 of the License, or
'    (at your option) any later version.
'
'    This program is distributed in the hope that it will be useful,
'    but WITHOUT ANY WARRANTY; without even the implied warranty of
'    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
'    GNU General Public License for more details.
'
'    You should have received a copy of the GNU General Public License
'    along with this program.  If not, see <http://www.gnu.org/licenses/>.

Imports System.Text.RegularExpressions
Imports System.Drawing
Imports System.Math
Public Class Main

    ''' <summary>
    ''' Changelog
    ''' </summary>
    ''' Changelog:
    ''' Added CMYK and HSL color support
    ''' Added multiple area selection with exporting format
    ''' Added color selecting for areas
    ''' Added 0.3d RC 4-2 MoveObject support
    ''' Added 0.3d RC 5 Skins
    ''' Added a cmd proccessor selection for Gates
    ''' Added Deutsch support
    ''' Added 0.3d RC 5-3 Sprites
    ''' Improved usage of memory
    ''' Fixed dialog preview color bug
    ''' Added zoom to map
    ''' Added a color picker for the dialog
    ''' Added Load Objects from file
    ''' Fixed a color bug with the dialogs previewer
    ''' Fixed some script bugs
    ''' Added zoom feature to areas selecting
    ''' Added 0.3d RC 7 IsObjectMoving support
    ''' Added SetPlayerWorldBounds support
    ''' Added Find Next function to Find 'Button'
    ''' Improved the time-load of the software

#Region "Arrays"

    Dim VehSender As Boolean = False, aCount As Integer, Areas As String, pAreas As String, _
        ColorSender As Integer = 0, VehPos As Integer, SprPos As Integer

#End Region

#Region "Main"

    Private Sub Main_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        SaveConfig()
    End Sub

    Private Sub Main_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        eColor.Close()
        Previewer.Close()
        AreaE.Close()
        LoadConfig()
        FillSkins()
        FillVehicles()
        FillSounds()
        FillAnims()
        FillWeapons()
        FillMapIcons()
        FillSprites()
        PictureBox3.Image = My.Resources.Map
        TrackBar12.Enabled = False
        TrackBar13.Enabled = False
        Panel6.BackColor = Config.C_Area.Hex

        Selection.Lock = False
        For i = 0 To 23
            Select Case i
                Case 0 To 5, 8, 11 To 15, 19, 22, 23
                    ComboBox1.Items.Add(i)
            End Select
        Next
        ComboBox1.SelectedIndex = 0
        ComboBox2.Items.Add("DIALOG_STYLE_MSGBOX")
        ComboBox2.Items.Add("DIALOG_STYLE_INPUT")
        ComboBox2.Items.Add("DIALOG_STYLE_LIST")
        ComboBox2.Items.Add("DIALOG_STYLE_PASSWORD")
        ComboBox2.SelectedIndex = 0

        ComboBox3.Items.Add("SA:MP Objects")
        ComboBox3.Items.Add("MTA Race Objects")
        ComboBox3.Items.Add("MTA 1.1 Objects")
        ComboBox3.Items.Add("Incognito's Streamer Plugin")
        ComboBox3.Items.Add("YSI DynamicObject")
        ComboBox3.Items.Add("xStreamer")
        ComboBox3.Items.Add("MidoStream Objects")
        ComboBox3.Items.Add("Doble-O Objects")
        ComboBox3.Items.Add("Fallout's Object Streamer")
        ComboBox3.SelectedIndex = 2
        ComboBox4.Items.Add("SA:MP Objects")
        ComboBox4.Items.Add("Incognito's Streamer Plugin")
        ComboBox4.Items.Add("YSI DynamicObject")
        ComboBox4.Items.Add("xStreamer")
        ComboBox4.Items.Add("MidoStream Objects")
        ComboBox4.Items.Add("Doble-O Objects")
        ComboBox4.Items.Add("Fallout's Object Streamer")
        ComboBox4.SelectedIndex = 0

        If CheckBox1.Checked = True Then
            Panel1.Visible = True
            Button2.Visible = True
            Label3.Visible = True
            TextBox2.Visible = True
        Else
            Panel1.Visible = False
            Button2.Visible = False
            Label3.Visible = False
            TextBox2.Visible = False
        End If

        If RadioButton6.Checked = True Then
            Panel2.BackColor = Config.C_Help.Hex
            Panel2.Visible = True
            Button3.Visible = True
            Label21.Visible = True
            Label4.Visible = True
            TextBox10.Visible = True
        Else
            Panel2.Visible = False
            Button3.Visible = False
            Label21.Visible = False
            Label4.Visible = False
            TextBox10.Visible = False
        End If

        If CheckBox2.Checked = True Then
            Panel3.Visible = True
            Button5.Visible = True
            Panel3.BackColor = Config.C_Open.Hex
            Label25.Visible = True
            TextBox25.Visible = True
        Else
            Panel3.Visible = False
            Button5.Visible = False
            Label25.Visible = False
            TextBox25.Visible = False
        End If

        If CheckBox3.Checked = True Then
            Panel4.Visible = True
            Panel4.BackColor = Config.C_Close.Hex
            Button6.Visible = True
            Label26.Visible = True
            TextBox26.Visible = True
        Else
            Panel4.Visible = False
            Panel4.BackColor = Config.C_Close.Hex
            Button6.Visible = False
            Label26.Visible = False
            TextBox26.Visible = False
        End If

        If RadioButton8.Checked = True Then
            Label23.Visible = False
            TextBox23.Visible = False
            LinkLabel2.Top = 48
            TextBox24.Top = 45
            Label29.Visible = True
            TextBox28.Visible = True
        Else
            Label23.Visible = True
            TextBox23.Visible = True
            LinkLabel2.Top = 74
            TextBox24.Top = 71
            Label29.Visible = False
            TextBox28.Visible = False
        End If

        If RadioButton9.Checked = True Then
            Label22.Visible = False
            TextBox22.Visible = False
            Label23.Visible = False
            TextBox23.Visible = False
            LinkLabel2.Top = 22
            TextBox24.Top = 19
            Label29.Visible = True
            TextBox28.Visible = True
        Else
            Label22.Visible = True
            TextBox22.Visible = True
            Label23.Visible = True
            TextBox23.Visible = True
            LinkLabel2.Top = 74
            TextBox24.Top = 71
            Label29.Visible = False
            TextBox28.Visible = False
        End If

        If RadioButton10.Checked = True Then
            Select Case Config.Idioma
                Case Lang.English
                    Label22.Text = "Command (both):"
                Case Lang.Español, Lang.Portugues
                    Label22.Text = "Cmd (ambos):"
                Case Else
                    Label22.Text = "Befehl (beides):"
            End Select
            Label23.Visible = False
            TextBox23.Visible = False
            LinkLabel2.Top = 48
            TextBox24.Top = 45
        Else
            Select Case Config.Idioma
                Case Lang.English
                    Label22.Text = "Command (open):"
                Case Lang.Español
                    Label22.Text = "Comando (abrir):"
                Case Lang.Portugues
                    Label22.Text = "Comando (aberta):"
                Case Else
                    Label22.Text = "Befehl (öffnen):"
            End Select
            Label23.Visible = True
            TextBox23.Visible = True
            LinkLabel2.Top = 74
            TextBox24.Top = 71
        End If

        If CheckBox6.Checked = True Then
            TextBox48.Visible = True
        Else
            TextBox48.Visible = False
        End If

        If CheckBox7.Checked = True Then
            GroupBox13.Enabled = False
            GroupBox14.Enabled = False
            GroupBox17.Enabled = False
        Else
            GroupBox13.Enabled = True
            GroupBox14.Enabled = True
            GroupBox17.Enabled = True
        End If

        If CheckBox8.Checked = True Then
            Button17.Visible = True
        Else
            Button17.Visible = False
        End If
    End Sub

    Private Sub Main_Move(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Move
        AreaE.Location = New Point(Me.Left, Me.Top + Me.Height)
    End Sub

#End Region

#Region "Tabs"

#Region "Teleports"

#Region "Texts Restrictions"

    Private Sub TextBox3_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox3.KeyPress
        If Not IsNumeric(e.KeyChar) And Not Char.IsControl(e.KeyChar) And e.KeyChar <> "." And e.KeyChar <> "-" And Asc(e.KeyChar) <> 8 Then e.Handled = True
        If e.KeyChar = "." And TextBox3.Text.IndexOf(".") <> -1 Then e.Handled = True
    End Sub

    Private Sub TextBox4_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox4.KeyPress
        If Not IsNumeric(e.KeyChar) And Not Char.IsControl(e.KeyChar) And e.KeyChar <> "." And e.KeyChar <> "-" And Asc(e.KeyChar) <> 8 Then e.Handled = True
        If e.KeyChar = "." And TextBox4.Text.IndexOf(".") <> -1 Then e.Handled = True
    End Sub

    Private Sub TextBox5_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox5.KeyPress
        If Not IsNumeric(e.KeyChar) And Not Char.IsControl(e.KeyChar) And e.KeyChar <> "." And e.KeyChar <> "-" And Asc(e.KeyChar) <> 8 Then e.Handled = True
        If e.KeyChar = "." And TextBox5.Text.IndexOf(".") <> -1 Then e.Handled = True
    End Sub

    Private Sub TextBox6_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox6.KeyPress
        If Not IsNumeric(e.KeyChar) And Not Char.IsControl(e.KeyChar) And Asc(e.KeyChar) <> 8 Then e.Handled = True
    End Sub

    Private Sub TextBox7_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox7.KeyPress
        If Not IsNumeric(e.KeyChar) And Not Char.IsControl(e.KeyChar) And Asc(e.KeyChar) <> 8 Then e.Handled = True
    End Sub

    Private Sub TextBox8_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox8.KeyPress
        If Not IsNumeric(e.KeyChar) And Not Char.IsControl(e.KeyChar) And e.KeyChar <> "." And Asc(e.KeyChar) <> 8 Then e.Handled = True
        If e.KeyChar = "." And TextBox8.Text.IndexOf(".") <> -1 Then e.Handled = True
    End Sub

    Private Sub TextBox3_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox3.TextChanged
        TextBox3.Text = Regex.Replace(TextBox3.Text, BadChars, "")
    End Sub

    Private Sub TextBox4_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox4.TextChanged
        TextBox4.Text = Regex.Replace(TextBox4.Text, BadChars, "")
    End Sub

    Private Sub TextBox5_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox5.TextChanged
        TextBox5.Text = Regex.Replace(TextBox5.Text, BadChars, "")
    End Sub

    Private Sub TextBox6_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox6.TextChanged
        TextBox6.Text = Regex.Replace(TextBox6.Text, BadChars, "")
    End Sub

    Private Sub TextBox7_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox7.TextChanged
        TextBox7.Text = Regex.Replace(TextBox7.Text, BadChars, "")
    End Sub

    Private Sub TextBox8_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox8.TextChanged
        TextBox8.Text = Regex.Replace(TextBox8.Text, BadChars, "")
    End Sub

#End Region

#Region "Message(Optional&Help)"

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        gSender = CC.Msg
        eColor.TrackBar4.Enabled = True
        eColor.TextBox4.Enabled = True
        eColor.Show()
        eColor.Focus()
        eColor.Panel1.BackColor = Config.C_Msg.Hex
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        gSender = CC.Help
        eColor.TrackBar4.Enabled = True
        eColor.TextBox4.Enabled = True
        eColor.Show()
        eColor.Focus()
        eColor.Panel1.BackColor = Config.C_Help.Hex
    End Sub

#End Region

#Region "Visual"

    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked = True Then
            Panel1.BackColor = Config.C_Msg.Hex
            Panel1.Visible = True
            Button2.Visible = True
            Label3.Visible = True
            TextBox2.Visible = True
        Else
            Panel1.Visible = False
            Button2.Visible = False
            Label3.Visible = False
            TextBox2.Visible = False
        End If
    End Sub

    Private Sub RadioButton6_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton6.CheckedChanged
        If RadioButton6.Checked = True Then
            Panel2.BackColor = Config.C_Help.Hex
            Panel2.Visible = True
            Button3.Visible = True
            Label21.Visible = True
            Label4.Visible = True
            TextBox10.Visible = True
        Else
            Panel2.Visible = False
            Button3.Visible = False
            Label21.Visible = False
            Label4.Visible = False
            TextBox10.Visible = False
        End If
    End Sub

#End Region

#Region "Generate Code"

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        TextBox1.Clear()
        If TextBox9.Text.Length = 0 Then
            Select Case Config.Idioma
                Case Lang.English
                    MsgBox("You must enter a name for the command.", MsgBoxStyle.Critical, "Error")
                Case Lang.Español
                    MsgBox("Debes ingresar un nombre para el comando.", MsgBoxStyle.Critical, "Error")
                Case Lang.Portugues
                    MsgBox("Você deve digitar um nome para o comando.", MsgBoxStyle.Critical, "Error")
                Case Else
                    MsgBox("Du musst einen Namen für den Befehl eingeben.", MsgBoxStyle.Critical, "Fehler")
            End Select
            Exit Sub
        End If
        If CheckBox1.Checked = True Then
            If TextBox2.Text.Length = 0 Then
                Select Case Config.Idioma
                    Case Lang.English
                        MsgBox("You must enter a message to send.", MsgBoxStyle.Critical, "Error")
                    Case Lang.Español
                        MsgBox("Debes ingresar un mensaje para enviar.", MsgBoxStyle.Critical, "Error")
                    Case Lang.Portugues
                        MsgBox("Você deve digitar uma mensagem para enviar.", MsgBoxStyle.Critical, "Error")
                    Case Else
                        MsgBox("Du musst eine Nachricht zum senden eingeben.", MsgBoxStyle.Critical, "Fehler")
                End Select
                TextBox2.Focus()
                Exit Sub
            End If
        End If
        If TextBox3.Text.Length = 0 Then
            Select Case Config.Idioma
                Case Lang.English
                    MsgBox("You must enter a X coordinate.", MsgBoxStyle.Critical, "Error")
                Case Lang.Español
                    MsgBox("Debes ingresar una coordenada X.", MsgBoxStyle.Critical, "Error")
                Case Lang.Portugues
                    MsgBox("Você deve digitar uma coordenada X.", MsgBoxStyle.Critical, "Error")
                Case Else
                    MsgBox("Du musst eine X-Koordinate eingeben.", MsgBoxStyle.Critical, "Fehler")
            End Select
            Exit Sub
        End If
        If TextBox4.Text.Length = 0 Then
            Select Case Config.Idioma
                Case Lang.English
                    MsgBox("You must enter a Y coordinate.", MsgBoxStyle.Critical, "Error")
                Case Lang.Español
                    MsgBox("Debes ingresar una coordenada Y.", MsgBoxStyle.Critical, "Error")
                Case Lang.Portugues
                    MsgBox("Você deve digitar uma coordenada Y.", MsgBoxStyle.Critical, "Error")
                Case Else
                    MsgBox("Du musst eine Y-Koordinate eingeben.", MsgBoxStyle.Critical, "Fehler")
            End Select
            Exit Sub
        End If
        If TextBox5.Text.Length = 0 Then
            Select Case Config.Idioma
                Case Lang.English
                    MsgBox("You must enter a Z coordinate.", MsgBoxStyle.Critical, "Error")
                Case Lang.Español
                    MsgBox("Debes ingresar una coordenada Z.", MsgBoxStyle.Critical, "Error")
                Case Lang.Portugues
                    MsgBox("Você deve digitar uma coordenada Z.", MsgBoxStyle.Critical, "Error")
                Case Else
                    MsgBox("Du musst eine Z-Koordinate eingeben.", MsgBoxStyle.Critical, "Fehler")
            End Select
            Exit Sub
        End If
        If TextBox6.Text.Length = 0 Then
            Select Case Config.Idioma
                Case Lang.English
                    MsgBox("You must enter a world ID.", MsgBoxStyle.Critical, "Error")
                Case Lang.Español
                    MsgBox("Debes ingresar el id de un mundo", MsgBoxStyle.Critical, "Error")
                Case Lang.Portugues
                    MsgBox("Você deve digitar um ID mundo.", MsgBoxStyle.Critical, "Error")
                Case Else
                    MsgBox("Du musst eine Welt ID eingeben.", MsgBoxStyle.Critical, "Fehler")
            End Select
            Exit Sub
        End If
        If TextBox7.Text.Length = 0 Then
            Select Case Config.Idioma
                Case Lang.English
                    MsgBox("You must enter an interior ID.", MsgBoxStyle.Critical, "Error")
                Case Lang.Español
                    MsgBox("Debes ingresar el id de un interior.", MsgBoxStyle.Critical, "Error")
                Case Lang.Portugues
                    MsgBox("Você deve digitar um ID interior.", MsgBoxStyle.Critical, "Error")
                Case Else
                    MsgBox("Du musst eine Interior ID eingeben.", MsgBoxStyle.Critical, "Fehler")
            End Select
            Exit Sub
        End If
        If TextBox8.Text.Length = 0 Then
            Select Case Config.Idioma
                Case Lang.English
                    MsgBox("You must enter an angle.", MsgBoxStyle.Critical, "Error")
                Case Lang.Español
                    MsgBox("Debes ingresar un angulo.", MsgBoxStyle.Critical, "Error")
                Case Lang.Portugues
                    MsgBox("Você deve digitar um ângulo.", MsgBoxStyle.Critical, "Error")
                Case Else
                    MsgBox("Du musst einen Winkel angeben.", MsgBoxStyle.Critical, "Fehler")
            End Select
            Exit Sub
        End If
        If RadioButton3.Checked = True Then
            If RadioButton1.Checked = True Then
                TextBox1.Text = "if(!strcmp(cmdtext, ""/" & TextBox9.Text & """, true)){" & vbNewLine & _
                vbTab & "SetPlayerPos(playerid, " & TextBox3.Text & ", " & TextBox4.Text & ", " & TextBox5.Text & ");" & vbNewLine & _
                vbTab & "SetPlayerFacingAngle(playerid, " & TextBox8.Text & ");" & vbNewLine & _
                vbTab & "SetPlayerInterior(playerid, " & TextBox7.Text & ");" & vbNewLine & _
                vbTab & "SetPlayerVirtualWorld(playerid, " & TextBox6.Text & ");" & vbNewLine
                If CheckBox1.Checked = True Then
                    TextBox1.Text += vbTab & "return SendClientMessage(playerid, " & Config.C_Msg.Name & ", """ & TextBox2.Text & """);" & vbNewLine & _
                    "}"
                Else
                    TextBox1.Text += vbTab & "return 1;" & vbNewLine & _
                    "}"
                End If
            Else
                TextBox1.Text = "if(!strcmp(cmdtext, ""/" & TextBox9.Text & """, true)){" & vbNewLine & _
                vbTab & "if(!IsPlayerInAnyVehicle(playerid)){" & vbNewLine & _
                vbTab & vbTab & "SetPlayerPos(playerid, " & TextBox3.Text & ", " & TextBox4.Text & ", " & TextBox5.Text & ");" & vbNewLine & _
                vbTab & vbTab & "SetPlayerFacingAngle(playerid, " & TextBox8.Text & ");" & vbNewLine & _
                vbTab & vbTab & "SetPlayerInterior(playerid, " & TextBox7.Text & ");" & vbNewLine & _
                vbTab & vbTab & "SetPlayerVirtualWorld(playerid, " & TextBox6.Text & ");" & vbNewLine & _
                vbTab & "}" & vbNewLine & _
                vbTab & "else{" & vbNewLine & _
                vbTab & vbTab & "new veh = GetPlayerVehicleID(playerid);" & vbNewLine & _
                vbTab & vbTab & "SetVehiclePos(veh, " & TextBox3.Text & ", " & TextBox4.Text & ", " & TextBox5.Text & ");" & vbNewLine & _
                vbTab & vbTab & "SetVehicleZAngle(veh, " & TextBox8.Text & ");" & vbNewLine & _
                vbTab & vbTab & "LinkVehicleToInterior(veh, " & TextBox7.Text & ");" & vbNewLine & _
                vbTab & vbTab & "SetVehicleVirtualWorld(veh, " & TextBox6.Text & ");" & vbNewLine & _
                vbTab & vbTab & "PutPlayerInVehicle(playerid, veh, 0);" & vbNewLine & _
                vbTab & "}" & vbNewLine
                If CheckBox1.Checked = True Then
                    TextBox1.Text += vbTab & "return SendClientMessage(playerid, " & Config.C_Msg.Name & ", """ & TextBox2.Text & """);" & vbNewLine & _
                    "}"
                Else
                    TextBox1.Text += vbTab & "return 1;" & vbNewLine & _
                    "}"
                End If
            End If
        ElseIf RadioButton4.Checked = True Then
            If RadioButton1.Checked = True Then
                TextBox1.Text = "dcmd(" & TextBox9.Text & ", " & TextBox9.Text.Length & ", " & "cmdtext);" & vbNewLine & vbNewLine & _
                "dcmd_" & TextBox9.Text & "(playerid, params[])" & vbNewLine & _
                "{" & vbNewLine & _
                vbTab & "#pragma unused params" & vbNewLine & _
                vbTab & "SetPlayerPos(playerid, " & TextBox3.Text & ", " & TextBox4.Text & ", " & TextBox5.Text & ");" & vbNewLine & _
                vbTab & "SetPlayerFacingAngle(playerid, " & TextBox8.Text & ");" & vbNewLine & _
                vbTab & "SetPlayerInterior(playerid, " & TextBox7.Text & ");" & vbNewLine & _
                vbTab & "SetPlayerVirtualWorld(playerid, " & TextBox6.Text & ");" & vbNewLine
                If CheckBox1.Checked = True Then
                    TextBox1.Text += vbTab & "return SendClientMessage(playerid, " & Config.C_Msg.Name & ", """ & TextBox2.Text & """);" & vbNewLine & _
                    "}"
                Else
                    TextBox1.Text += vbTab & "return 1;" & vbNewLine & _
                    "}"
                End If
            Else
                TextBox1.Text = "dcmd(" & TextBox9.Text & ", " & TextBox9.Text.Length & ", " & "cmdtext);" & vbNewLine & vbNewLine & _
                "dcmd_" & TextBox9.Text & "(playerid, params[])" & vbNewLine & _
                "{" & vbNewLine & _
                vbTab & "#pragma unused params" & vbNewLine & _
                vbTab & "if(!IsPlayerInAnyVehicle(playerid)){" & vbNewLine & _
                vbTab & vbTab & "SetPlayerPos(playerid, " & TextBox3.Text & ", " & TextBox4.Text & ", " & TextBox5.Text & ");" & vbNewLine & _
                vbTab & vbTab & "SetPlayerFacingAngle(playerid, " & TextBox8.Text & ");" & vbNewLine & _
                vbTab & vbTab & "SetPlayerInterior(playerid, " & TextBox7.Text & ");" & vbNewLine & _
                vbTab & vbTab & "SetPlayerVirtualWorld(playerid, " & TextBox6.Text & ");" & vbNewLine & _
                vbTab & "}" & vbNewLine & _
                vbTab & "else{" & vbNewLine & _
                vbTab & vbTab & "new veh = GetPlayerVehicleID(playerid);" & vbNewLine & _
                vbTab & vbTab & "SetVehiclePos(veh, " & TextBox3.Text & ", " & TextBox4.Text & ", " & TextBox5.Text & ");" & vbNewLine & _
                vbTab & vbTab & "SetVehicleZAngle(veh, " & TextBox8.Text & ");" & vbNewLine & _
                vbTab & vbTab & "LinkVehicleToInterior(veh, " & TextBox7.Text & ");" & vbNewLine & _
                vbTab & vbTab & "SetVehicleVirtualWorld(veh, " & TextBox6.Text & ");" & vbNewLine & _
                vbTab & vbTab & "PutPlayerInVehicle(playerid, veh, 0);" & vbNewLine & _
                vbTab & "}" & vbNewLine
                If CheckBox1.Checked = True Then
                    TextBox1.Text += vbTab & "return SendClientMessage(playerid, " & Config.C_Msg.Name & ", """ & TextBox2.Text & """);" & vbNewLine & _
                    "}"
                Else
                    TextBox1.Text += vbTab & "return 1;" & vbNewLine & _
                    "}"
                End If
            End If
        ElseIf RadioButton5.Checked = True Then
            If RadioButton1.Checked = True Then
                TextBox1.Text = "CMD:" & TextBox9.Text & "(playerid, params[])" & vbNewLine & _
                "{" & vbNewLine & _
                vbTab & "SetPlayerPos(playerid, " & TextBox3.Text & ", " & TextBox4.Text & ", " & TextBox5.Text & ");" & vbNewLine & _
                vbTab & "SetPlayerFacingAngle(playerid, " & TextBox8.Text & ");" & vbNewLine & _
                vbTab & "SetPlayerInterior(playerid, " & TextBox7.Text & ");" & vbNewLine & _
                vbTab & "SetPlayerVirtualWorld(playerid, " & TextBox6.Text & ");" & vbNewLine
                If CheckBox1.Checked = True Then
                    TextBox1.Text += vbTab & "return SendClientMessage(playerid, " & Config.C_Msg.Name & ", """ & TextBox2.Text & """);" & vbNewLine & _
                    "}"
                Else
                    TextBox1.Text += vbTab & "return 1;" & vbNewLine & _
                    "}"
                End If
            Else
                TextBox1.Text = "CMD:" & TextBox9.Text & "(playerid, params[])" & vbNewLine & _
                "{" & vbNewLine & _
                vbTab & "if(!IsPlayerInAnyVehicle(playerid)){" & vbNewLine & _
                vbTab & vbTab & "SetPlayerPos(playerid, " & TextBox3.Text & ", " & TextBox4.Text & ", " & TextBox5.Text & ");" & vbNewLine & _
                vbTab & vbTab & "SetPlayerFacingAngle(playerid, " & TextBox8.Text & ");" & vbNewLine & _
                vbTab & vbTab & "SetPlayerInterior(playerid, " & TextBox7.Text & ");" & vbNewLine & _
                vbTab & vbTab & "SetPlayerVirtualWorld(playerid, " & TextBox6.Text & ");" & vbNewLine & _
                vbTab & "}" & vbNewLine & _
                vbTab & "else{" & vbNewLine & _
                vbTab & vbTab & "new veh = GetPlayerVehicleID(playerid);" & vbNewLine & _
                vbTab & vbTab & "SetVehiclePos(veh, " & TextBox3.Text & ", " & TextBox4.Text & ", " & TextBox5.Text & ");" & vbNewLine & _
                vbTab & vbTab & "SetVehicleZAngle(veh, " & TextBox8.Text & ");" & vbNewLine & _
                vbTab & vbTab & "LinkVehicleToInterior(veh, " & TextBox7.Text & ");" & vbNewLine & _
                vbTab & vbTab & "SetVehicleVirtualWorld(veh, " & TextBox6.Text & ");" & vbNewLine & _
                vbTab & vbTab & "PutPlayerInVehicle(playerid, veh, 0);" & vbNewLine & _
                vbTab & "}" & vbNewLine
                If CheckBox1.Checked = True Then
                    TextBox1.Text += vbTab & "return SendClientMessage(playerid, " & Config.C_Msg.Name & ", """ & TextBox2.Text & """);" & vbNewLine & _
                    "}"
                Else
                    TextBox1.Text += vbTab & "return 1;" & vbNewLine & _
                    "}"
                End If
            End If
        ElseIf RadioButton6.Checked = True Then
            If TextBox10.Text.Length = 0 Then
                Select Case Config.Idioma
                    Case Lang.English
                        MsgBox("You must enter a help message to send.", MsgBoxStyle.Critical, "Error")
                    Case Lang.Español
                        MsgBox("Debes ingresar un mensaje de ayuda para enviar.", MsgBoxStyle.Critical, "Error")
                    Case Lang.Portugues
                        MsgBox("YVocê deve digitar uma mensagem de ajuda para enviar.", MsgBoxStyle.Critical, "Error")
                    Case Else
                        MsgBox("Du musst eine Hilfsnachricht zum senden angeben.", MsgBoxStyle.Critical, "Fehler")
                End Select
                TextBox2.Focus()
                Exit Sub
            End If
            If RadioButton1.Checked = True Then
                TextBox1.Text = "YCMD:" & TextBox9.Text & "(playerid, params[], help)" & vbNewLine & _
                "{" & vbNewLine & _
                vbTab & "if(help) return SendClientMessage(playerid, " & Config.C_Help.Name & ", """ & TextBox10.Text & """);" & vbNewLine & _
                vbTab & "SetPlayerPos(playerid, " & TextBox3.Text & ", " & TextBox4.Text & ", " & TextBox5.Text & ");" & vbNewLine & _
                vbTab & "SetPlayerFacingAngle(playerid, " & TextBox8.Text & ");" & vbNewLine & _
                vbTab & "SetPlayerInterior(playerid, " & TextBox7.Text & ");" & vbNewLine & _
                vbTab & "SetPlayerVirtualWorld(playerid, " & TextBox6.Text & ");" & vbNewLine
                If CheckBox1.Checked = True Then
                    TextBox1.Text += vbTab & "return SendClientMessage(playerid, " & Config.C_Msg.Name & ", """ & TextBox2.Text & """);" & vbNewLine & _
                    "}"
                Else
                    TextBox1.Text += vbTab & "return 1;" & vbNewLine & _
                    "}"
                End If
            Else
                TextBox1.Text = "YCMD:" & TextBox9.Text & "(playerid, params[], help)" & vbNewLine & _
                "{" & vbNewLine & _
                vbTab & "If(help) return SendClientMessage(playerid, " & Config.C_Help.Name & ", """ & TextBox10.Text & """);" & vbNewLine & _
                vbTab & "if(!IsPlayerInAnyVehicle(playerid)){" & vbNewLine & _
                vbTab & vbTab & "SetPlayerPos(playerid, " & TextBox3.Text & ", " & TextBox4.Text & ", " & TextBox5.Text & ");" & vbNewLine & _
                vbTab & vbTab & "SetPlayerFacingAngle(playerid, " & TextBox8.Text & ");" & vbNewLine & _
                vbTab & vbTab & "SetPlayerInterior(playerid, " & TextBox7.Text & ");" & vbNewLine & _
                vbTab & vbTab & "SetPlayerVirtualWorld(playerid, " & TextBox6.Text & ");" & vbNewLine & _
                vbTab & "}" & vbNewLine & _
                vbTab & "else{" & vbNewLine & _
                vbTab & vbTab & "new veh = GetPlayerVehicleID(playerid);" & vbNewLine & _
                vbTab & vbTab & "SetVehiclePos(veh, " & TextBox3.Text & ", " & TextBox4.Text & ", " & TextBox5.Text & ");" & vbNewLine & _
                vbTab & vbTab & "SetVehicleZAngle(veh, " & TextBox8.Text & ");" & vbNewLine & _
                vbTab & vbTab & "LinkVehicleToInterior(veh, " & TextBox7.Text & ");" & vbNewLine & _
                vbTab & vbTab & "SetVehicleVirtualWorld(veh, " & TextBox6.Text & ");" & vbNewLine & _
                vbTab & vbTab & "PutPlayerInVehicle(playerid, veh, 0);" & vbNewLine & _
                vbTab & "}" & vbNewLine
                If CheckBox1.Checked = True Then
                    TextBox1.Text += vbTab & "return SendClientMessage(playerid, " & Config.C_Msg.Name & ", """ & TextBox2.Text & """);" & vbNewLine & _
                    "}"
                Else
                    TextBox1.Text += vbTab & "return 1;" & vbNewLine & _
                    "}"
                End If
            End If
        End If
    End Sub

#End Region

#Region "Extra"

    Private Sub TabPage1_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles TabPage1.Leave
        CheckBox1.Checked = False
        RadioButton1.Checked = True
        RadioButton3.Checked = True
        TextBox1.Clear()
        TextBox2.Clear()
        TextBox9.Clear()
        TextBox10.Clear()
        TextBox3.Text = "0.0"
        TextBox4.Text = "0.0"
        TextBox5.Text = "0.0"
        TextBox8.Text = "0.0"
        TextBox7.Text = "0"
        TextBox6.Text = "0"
    End Sub

#End Region

#End Region

#Region "Gates"

#Region "Texts Restrictions"

    Private Sub TextBox12_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox12.KeyPress
        If Not IsNumeric(e.KeyChar) And Not Char.IsControl(e.KeyChar) And e.KeyChar <> "." And e.KeyChar <> "-" And Asc(e.KeyChar) <> 8 Then e.Handled = True
        If e.KeyChar = "." And TextBox12.Text.IndexOf(".") <> -1 Then e.Handled = True
    End Sub

    Private Sub TextBox13_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox13.KeyPress
        If Not IsNumeric(e.KeyChar) And Not Char.IsControl(e.KeyChar) And e.KeyChar <> "." And e.KeyChar <> "-" And Asc(e.KeyChar) <> 8 Then e.Handled = True
        If e.KeyChar = "." And TextBox13.Text.IndexOf(".") <> -1 Then e.Handled = True
    End Sub

    Private Sub TextBox14_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox14.KeyPress
        If Not IsNumeric(e.KeyChar) And Not Char.IsControl(e.KeyChar) And e.KeyChar <> "." And Asc(e.KeyChar) <> 8 Then e.Handled = True
        If e.KeyChar = "." And TextBox14.Text.IndexOf(".") <> -1 Then e.Handled = True
    End Sub

    Private Sub TextBox15_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox15.KeyPress
        If Not IsNumeric(e.KeyChar) And Not Char.IsControl(e.KeyChar) And e.KeyChar <> "." And e.KeyChar <> "-" And Asc(e.KeyChar) <> 8 Then e.Handled = True
        If e.KeyChar = "." And TextBox15.Text.IndexOf(".") <> -1 Then e.Handled = True
    End Sub

    Private Sub TextBox18_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox18.KeyPress
        If Not IsNumeric(e.KeyChar) And Not Char.IsControl(e.KeyChar) And e.KeyChar <> "." And Asc(e.KeyChar) <> 8 Then e.Handled = True
        If e.KeyChar = "." And TextBox18.Text.IndexOf(".") <> -1 Then e.Handled = True
    End Sub

    Private Sub TextBox19_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox19.KeyPress
        If Not IsNumeric(e.KeyChar) And Not Char.IsControl(e.KeyChar) And e.KeyChar <> "." And e.KeyChar <> "-" And Asc(e.KeyChar) <> 8 Then e.Handled = True
        If e.KeyChar = "." And TextBox19.Text.IndexOf(".") <> -1 Then e.Handled = True
    End Sub

    Private Sub TextBox20_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox20.KeyPress
        If Not IsNumeric(e.KeyChar) And Not Char.IsControl(e.KeyChar) And e.KeyChar <> "." And e.KeyChar <> "-" And Asc(e.KeyChar) <> 8 Then e.Handled = True
        If e.KeyChar = "." And TextBox20.Text.IndexOf(".") <> -1 Then e.Handled = True
    End Sub

    Private Sub TextBox21_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox21.KeyPress
        If Not IsNumeric(e.KeyChar) And Not Char.IsControl(e.KeyChar) And e.KeyChar <> "." And e.KeyChar <> "-" And Asc(e.KeyChar) <> 8 Then e.Handled = True
        If e.KeyChar = "." And TextBox21.Text.IndexOf(".") <> -1 Then e.Handled = True
    End Sub

    Private Sub TextBox24_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox24.KeyPress
        If Not IsNumeric(e.KeyChar) And Not Char.IsControl(e.KeyChar) And Asc(e.KeyChar) <> 8 Then e.Handled = True
    End Sub

    Private Sub TextBox27_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox27.KeyPress
        If Not IsNumeric(e.KeyChar) And Not Char.IsControl(e.KeyChar) And e.KeyChar <> "." And Asc(e.KeyChar) <> 8 Then e.Handled = True
        If e.KeyChar = "." And TextBox27.Text.IndexOf(".") <> -1 Then e.Handled = True
    End Sub

    Private Sub TextBox28_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox28.KeyPress
        If Not IsNumeric(e.KeyChar) And Not Char.IsControl(e.KeyChar) And Asc(e.KeyChar) <> 8 Then e.Handled = True
    End Sub

    Private Sub TextBox77_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox77.KeyPress
        If Not IsNumeric(e.KeyChar) And Not Char.IsControl(e.KeyChar) And e.KeyChar <> "." And e.KeyChar <> "-" And Asc(e.KeyChar) <> 8 Then e.Handled = True
        If e.KeyChar = "." And TextBox77.Text.IndexOf(".") <> -1 Then e.Handled = True
    End Sub

    Private Sub TextBox78_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox78.KeyPress
        If Not IsNumeric(e.KeyChar) And Not Char.IsControl(e.KeyChar) And e.KeyChar <> "." And e.KeyChar <> "-" And Asc(e.KeyChar) <> 8 Then e.Handled = True
        If e.KeyChar = "." And TextBox78.Text.IndexOf(".") <> -1 Then e.Handled = True
    End Sub

    Private Sub TextBox79_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox79.KeyPress
        If Not IsNumeric(e.KeyChar) And Not Char.IsControl(e.KeyChar) And e.KeyChar <> "." And e.KeyChar <> "-" And Asc(e.KeyChar) <> 8 Then e.Handled = True
        If e.KeyChar = "." And TextBox79.Text.IndexOf(".") <> -1 Then e.Handled = True
    End Sub

    Private Sub TextBox17_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox17.KeyPress
        If Not IsNumeric(e.KeyChar) And Not Char.IsControl(e.KeyChar) And e.KeyChar <> "." And e.KeyChar <> "-" And Asc(e.KeyChar) <> 8 Then e.Handled = True
        If e.KeyChar = "." And TextBox17.Text.IndexOf(".") <> -1 Then e.Handled = True
    End Sub

    Private Sub TextBox80_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox80.KeyPress
        If Not IsNumeric(e.KeyChar) And Not Char.IsControl(e.KeyChar) And e.KeyChar <> "." And e.KeyChar <> "-" And Asc(e.KeyChar) <> 8 Then e.Handled = True
        If e.KeyChar = "." And TextBox80.Text.IndexOf(".") <> -1 Then e.Handled = True
    End Sub

    Private Sub TextBox81_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox81.KeyPress
        If Not IsNumeric(e.KeyChar) And Not Char.IsControl(e.KeyChar) And e.KeyChar <> "." And e.KeyChar <> "-" And Asc(e.KeyChar) <> 8 Then e.Handled = True
        If e.KeyChar = "." And TextBox81.Text.IndexOf(".") <> -1 Then e.Handled = True
    End Sub

    Private Sub TextBox12_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox12.TextChanged
        TextBox12.Text = Regex.Replace(TextBox12.Text, BadChars, "")
    End Sub

    Private Sub TextBox13_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox13.TextChanged
        TextBox13.Text = Regex.Replace(TextBox13.Text, BadChars, "")
    End Sub

    Private Sub TextBox14_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox14.TextChanged
        TextBox14.Text = Regex.Replace(TextBox14.Text, BadChars, "")
    End Sub

    Private Sub TextBox15_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox15.TextChanged
        TextBox15.Text = Regex.Replace(TextBox15.Text, BadChars, "")
    End Sub

    Private Sub TextBox18_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox18.TextChanged
        TextBox18.Text = Regex.Replace(TextBox18.Text, BadChars, "")
    End Sub

    Private Sub TextBox19_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox19.TextChanged
        TextBox19.Text = Regex.Replace(TextBox19.Text, BadChars, "")
    End Sub

    Private Sub TextBox20_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox20.TextChanged
        TextBox20.Text = Regex.Replace(TextBox20.Text, BadChars, "")
    End Sub

    Private Sub TextBox21_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox21.TextChanged
        TextBox21.Text = Regex.Replace(TextBox21.Text, BadChars, "")
    End Sub

    Private Sub TextBox24_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox24.TextChanged
        TextBox24.Text = Regex.Replace(TextBox24.Text, BadChars, "")
    End Sub

    Private Sub TextBox27_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox27.TextChanged
        TextBox27.Text = Regex.Replace(TextBox27.Text, BadChars, "")
    End Sub

    Private Sub TextBox28_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox28.TextChanged
        TextBox27.Text = Regex.Replace(TextBox27.Text, BadChars, "")
    End Sub

    Private Sub TextBox77_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox77.TextChanged
        TextBox77.Text = Regex.Replace(TextBox77.Text, BadChars, "")
    End Sub

    Private Sub TextBox78_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox78.TextChanged
        TextBox78.Text = Regex.Replace(TextBox78.Text, BadChars, "")
    End Sub

    Private Sub TextBox79_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox79.TextChanged
        TextBox79.Text = Regex.Replace(TextBox79.Text, BadChars, "")
    End Sub

    Private Sub TextBox17_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox17.TextChanged
        TextBox17.Text = Regex.Replace(TextBox17.Text, BadChars, "")
    End Sub

    Private Sub TextBox80_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox80.TextChanged
        TextBox80.Text = Regex.Replace(TextBox80.Text, BadChars, "")
    End Sub

    Private Sub TextBox81_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox81.TextChanged
        TextBox81.Text = Regex.Replace(TextBox81.Text, BadChars, "")
    End Sub

#End Region

#Region "Messages"

    Private Sub CheckBox2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox2.CheckedChanged
        If CheckBox2.Checked = True Then
            Panel3.Visible = True
            Button5.Visible = True
            Panel3.BackColor = Config.C_Open.Hex
            Label25.Visible = True
            TextBox25.Visible = True
        Else
            Panel3.Visible = False
            Button5.Visible = False
            Label25.Visible = False
            TextBox25.Visible = False
        End If
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        gSender = CC.Gate_Open
        eColor.TrackBar4.Enabled = True
        eColor.TextBox4.Enabled = True
        eColor.Show()
        eColor.Focus()
        eColor.Panel1.BackColor = Config.C_Open.Hex
    End Sub

    Private Sub CheckBox3_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox3.CheckedChanged
        If CheckBox3.Checked = True Then
            Panel4.Visible = True
            Panel4.BackColor = Config.C_Close.Hex
            Button6.Visible = True
            Label26.Visible = True
            TextBox26.Visible = True
        Else
            Panel4.Visible = False
            Panel4.BackColor = Config.C_Close.Hex
            Button6.Visible = False
            Label26.Visible = False
            TextBox26.Visible = False
        End If
    End Sub

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        gSender = CC.Gate_Close
        eColor.TrackBar4.Enabled = True
        eColor.TextBox4.Enabled = True
        eColor.Show()
        eColor.Focus()
        eColor.Panel1.BackColor = Config.C_Close.Hex
    End Sub

#End Region

#Region "Visual"

    Private Sub RadioButton7_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton7.CheckedChanged
        GroupBox15.Enabled = True
    End Sub

    Private Sub RadioButton10_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton10.CheckedChanged
        If RadioButton10.Checked = True Then
            Select Case Config.Idioma
                Case Lang.English
                    Label22.Text = "Command (both):"
                Case Lang.Español, Lang.Portugues
                    Label22.Text = "Cmd (ambos):"
                Case Else
                    Label22.Text = "Befehl (beides):"
            End Select
            Label23.Visible = False
            TextBox23.Visible = False
            LinkLabel2.Top = 48
            TextBox24.Top = 45
            GroupBox15.Enabled = True
        Else
            Select Case Config.Idioma
                Case Lang.English
                    Label22.Text = "Command (open):"
                Case Lang.Español
                    Label22.Text = "Comando (abrir):"
                Case Lang.Portugues
                    Label22.Text = "Comando (aberta):"
                Case Else
                    Label22.Text = "Befehl (öffnen):"
            End Select
            Label23.Visible = True
            TextBox23.Visible = True
            LinkLabel2.Top = 74
            TextBox24.Top = 71
        End If
    End Sub

    Private Sub RadioButton8_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton8.CheckedChanged
        If RadioButton8.Checked = True Then
            Label23.Visible = False
            TextBox23.Visible = False
            LinkLabel2.Top = 48
            TextBox24.Top = 45
            Label29.Visible = True
            TextBox28.Visible = True
            GroupBox15.Enabled = True
        Else
            Label23.Visible = True
            TextBox23.Visible = True
            LinkLabel2.Top = 74
            TextBox24.Top = 71
            Label29.Visible = False
            TextBox28.Visible = False
        End If
    End Sub

    Private Sub RadioButton9_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton9.CheckedChanged
        If RadioButton9.Checked = True Then
            Label22.Visible = False
            TextBox22.Visible = False
            Label23.Visible = False
            TextBox23.Visible = False
            LinkLabel2.Top = 22
            TextBox24.Top = 19
            Label29.Visible = True
            TextBox28.Visible = True
            GroupBox15.Enabled = False
        Else
            Label22.Visible = True
            TextBox22.Visible = True
            Label23.Visible = True
            TextBox23.Visible = True
            LinkLabel2.Top = 74
            TextBox24.Top = 71
            Label29.Visible = False
            TextBox28.Visible = False
        End If
    End Sub

#End Region

#Region "Help"

    Private Sub LinkLabel2_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel2.LinkClicked
        Process.Start("http://wiki.sa-mp.com/wiki/Objects")
    End Sub

#End Region

#Region "Generate Code"

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        TextBox11.Clear()
        If TextBox24.Text.Length = 0 Then
            Select Case Config.Idioma
                Case Lang.English
                    MsgBox("You must enter an object id.", MsgBoxStyle.Critical, "Error")
                Case Lang.Español
                    MsgBox("Debes ingresar el id de un objeto.", MsgBoxStyle.Critical, "Error")
                Case Lang.Portugues
                    MsgBox("Você deve digitar um ID de objeto.", MsgBoxStyle.Critical, "Error")
                Case Else
                    MsgBox("Du musst eine Objekt ID eingeben.", MsgBoxStyle.Critical, "Fehler")
            End Select
            Exit Sub
        End If
        If CheckBox2.Checked = True Then
            If TextBox25.Text.Length = 0 Then
                Select Case Config.Idioma
                    Case Lang.English
                        MsgBox("You must enter a message to send when the door opens.", MsgBoxStyle.Critical, "Error")
                    Case Lang.Español
                        MsgBox("Debes ingresar un mensaje para enviar cuando la puerta se abre.", MsgBoxStyle.Critical, "Error")
                    Case Lang.Portugues
                        MsgBox("Você deve digitar uma mensagem para enviar, quando a porta se abre.", MsgBoxStyle.Critical, "Error")
                    Case Else
                        MsgBox("Du musst eine Nachricht eingeben, die beim Öffnen der Tür erscheint.", MsgBoxStyle.Critical, "Fehler")
                End Select
                Exit Sub
            End If
        End If
        If CheckBox3.Checked = True Then
            If TextBox26.Text.Length = 0 Then
                Select Case Config.Idioma
                    Case Lang.English
                        MsgBox("You must enter a message to send when the door closes.", MsgBoxStyle.Critical, "Error")
                    Case Lang.Español
                        MsgBox("Debes ingresar un mensaje para enviar cuando la puerta se cierra.", MsgBoxStyle.Critical, "Error")
                    Case Lang.Portugues
                        MsgBox("Você deve digitar uma mensagem para enviar, quando a porta se fecha.", MsgBoxStyle.Critical, "Error")
                    Case Else
                        MsgBox("Du musst eine Nachricht eingeben, die beim Schließen der Tür erscheint.", MsgBoxStyle.Critical, "Fehler")
                End Select
                Exit Sub
            End If
        End If
        If TextBox15.Text.Length = 0 Then
            Select Case Config.Idioma
                Case Lang.English
                    MsgBox("You must enter a X coordinate.", MsgBoxStyle.Critical, "Error")
                Case Lang.Español
                    MsgBox("Debes ingresar una coordenada X.", MsgBoxStyle.Critical, "Error")
                Case Lang.Portugues
                    MsgBox("Você deve digitar uma coordenada X.", MsgBoxStyle.Critical, "Error")
                Case Else
                    MsgBox("Du musst eine X-Koordinate eingeben.", MsgBoxStyle.Critical, "Fehler")
            End Select
            Exit Sub
        End If
        If TextBox12.Text.Length = 0 Then
            Select Case Config.Idioma
                Case Lang.English
                    MsgBox("You must enter a Y coordinate.", MsgBoxStyle.Critical, "Error")
                Case Lang.Español
                    MsgBox("Debes ingresar una coordenada Y.", MsgBoxStyle.Critical, "Error")
                Case Lang.Portugues
                    MsgBox("Você deve digitar uma coordenada Y.", MsgBoxStyle.Critical, "Error")
                Case Else
                    MsgBox("Du musst eine Y-Koordinate eingeben.", MsgBoxStyle.Critical, "Fehler")
            End Select
            Exit Sub
        End If
        If TextBox13.Text.Length = 0 Then
            Select Case Config.Idioma
                Case Lang.English
                    MsgBox("You must enter a Z coordinate.", MsgBoxStyle.Critical, "Error")
                Case Lang.Español
                    MsgBox("Debes ingresar una coordenada Z.", MsgBoxStyle.Critical, "Error")
                Case Lang.Portugues
                    MsgBox("Você deve digitar uma coordenada Z.", MsgBoxStyle.Critical, "Error")
                Case Else
                    MsgBox("Du musst eine Z-Koordinate eingeben.", MsgBoxStyle.Critical, "Fehler")
            End Select
            Exit Sub
        End If
        If TextBox14.Text.Length = 0 Then
            Select Case Config.Idioma
                Case Lang.English
                    MsgBox("You must enter a radio to open the door.", MsgBoxStyle.Critical, "Error")
                Case Lang.Español
                    MsgBox("Debes ingresar un radio para abrir la puerta.", MsgBoxStyle.Critical, "Error")
                Case Lang.Portugues
                    MsgBox("Você deve digitar um rádio para abrir a porta.", MsgBoxStyle.Critical, "Error")
                Case Else
                    MsgBox("Du musst eine Rate für die zu öffnende Tür eingeben.", MsgBoxStyle.Critical, "Fehler")
            End Select
            Exit Sub
        End If
        If TextBox21.Text.Length = 0 Then
            Select Case Config.Idioma
                Case Lang.English
                    MsgBox("You must enter a X coordinate.", MsgBoxStyle.Critical, "Error")
                Case Lang.Español
                    MsgBox("Debes ingresar una coordenada X.", MsgBoxStyle.Critical, "Error")
                Case Lang.Portugues
                    MsgBox("Você deve digitar uma coordenada X.", MsgBoxStyle.Critical, "Error")
                Case Else
                    MsgBox("Du musst eine X-Koordinate eingeben.", MsgBoxStyle.Critical, "Fehler")
            End Select
            Exit Sub
        End If
        If TextBox20.Text.Length = 0 Then
            Select Case Config.Idioma
                Case Lang.English
                    MsgBox("You must enter a Y coordinate.", MsgBoxStyle.Critical, "Error")
                Case Lang.Español
                    MsgBox("Debes ingresar una coordenada Y.", MsgBoxStyle.Critical, "Error")
                Case Lang.Portugues
                    MsgBox("Você deve digitar uma coordenada Y.", MsgBoxStyle.Critical, "Error")
                Case Else
                    MsgBox("Du musst eine Y-Koordinate eingeben.", MsgBoxStyle.Critical, "Fehler")
            End Select
            Exit Sub
        End If
        If TextBox19.Text.Length = 0 Then
            Select Case Config.Idioma
                Case Lang.English
                    MsgBox("You must enter a Z coordinate.", MsgBoxStyle.Critical, "Error")
                Case Lang.Español
                    MsgBox("Debes ingresar una coordenada Z.", MsgBoxStyle.Critical, "Error")
                Case Lang.Portugues
                    MsgBox("Você deve digitar uma coordenada Z.", MsgBoxStyle.Critical, "Error")
                Case Else
                    MsgBox("Du musst eine Z-Koordinate eingeben.", MsgBoxStyle.Critical, "Fehler")
            End Select
            Exit Sub
        End If
        If TextBox18.Text.Length = 0 Then
            Select Case Config.Idioma
                Case Lang.English
                    MsgBox("You must enter a radio to lock the door.", MsgBoxStyle.Critical, "Error")
                Case Lang.Español
                    MsgBox("Debes ingresar un radio para cerrar la puerta.", MsgBoxStyle.Critical, "Error")
                Case Lang.Portugues
                    MsgBox("Você deve digitar um rádio para trancar a porta.", MsgBoxStyle.Critical, "Error")
                Case Else
                    MsgBox("Du musst eine Rate für die zu schließende Tür eingeben.", MsgBoxStyle.Critical, "Fehler")
            End Select
            Exit Sub
        End If
        If TextBox79.Text.Length = 0 Then
            Select Case Config.Idioma
                Case Lang.English
                    MsgBox("You must enter a X rotation for the open door.", MsgBoxStyle.Critical, "Error")
                Case Lang.Español
                    MsgBox("Debes ingresar una rotacion X para la puerta abierta", MsgBoxStyle.Critical, "Error")
                Case Lang.Portugues
                    MsgBox("Você deve digitar uma rotação X para a porta aberta.", MsgBoxStyle.Critical, "Error")
                Case Else
                    MsgBox("Du musst eine X-Rotation für die offene Tür eingeben.", MsgBoxStyle.Critical, "Fehler")
            End Select
            Exit Sub
        End If
        If TextBox78.Text.Length = 0 Then
            Select Case Config.Idioma
                Case Lang.English
                    MsgBox("You must enter a Y rotation for the open door.", MsgBoxStyle.Critical, "Error")
                Case Lang.Español
                    MsgBox("Debes ingresar una rotacion Y para la puerta abierta", MsgBoxStyle.Critical, "Error")
                Case Lang.Portugues
                    MsgBox("Você deve digitar uma rotação Y para a porta aberta.", MsgBoxStyle.Critical, "Error")
                Case Else
                    MsgBox("Du musst eine Y-Rotation für die offene Tür eingeben.", MsgBoxStyle.Critical, "Fehler")
            End Select
            Exit Sub
        End If
        If TextBox77.Text.Length = 0 Then
            Select Case Config.Idioma
                Case Lang.English
                    MsgBox("You must enter a Z rotation for the open door.", MsgBoxStyle.Critical, "Error")
                Case Lang.Español
                    MsgBox("Debes ingresar una rotacion Z para la puerta abierta", MsgBoxStyle.Critical, "Error")
                Case Lang.Portugues
                    MsgBox("Você deve digitar uma rotação Z para a porta aberta.", MsgBoxStyle.Critical, "Error")
                Case Else
                    MsgBox("Du musst eine Z-Rotation für die offene Tür eingeben.", MsgBoxStyle.Critical, "Fehler")
            End Select
            Exit Sub
        End If
        If TextBox81.Text.Length = 0 Then
            Select Case Config.Idioma
                Case Lang.English
                    MsgBox("You must enter a X rotation for the closed door.", MsgBoxStyle.Critical, "Error")
                Case Lang.Español
                    MsgBox("Debes ingresar una rotacion X para la puerta cerrada", MsgBoxStyle.Critical, "Error")
                Case Lang.Portugues
                    MsgBox("Você deve digitar uma rotação X para a porta fechada.", MsgBoxStyle.Critical, "Error")
                Case Else
                    MsgBox("Du musst eine X-Rotation für die geschlossene Tür eingeben", MsgBoxStyle.Critical, "Fehler")
            End Select
            Exit Sub
        End If
        If TextBox80.Text.Length = 0 Then
            Select Case Config.Idioma
                Case Lang.English
                    MsgBox("You must enter a Y rotation for the closed door.", MsgBoxStyle.Critical, "Error")
                Case Lang.Español
                    MsgBox("Debes ingresar una rotacion Y para la puerta cerrada", MsgBoxStyle.Critical, "Error")
                Case Lang.Portugues
                    MsgBox("Você deve digitar uma rotação Y para a porta fechada.", MsgBoxStyle.Critical, "Error")
                Case Else
                    MsgBox("Du musst eine Y-Rotation für die geschlossene Tür eingeben", MsgBoxStyle.Critical, "Fehler")
            End Select
            Exit Sub
        End If
        If TextBox17.Text.Length = 0 Then
            Select Case Config.Idioma
                Case Lang.English
                    MsgBox("You must enter a Z rotation for the closed door.", MsgBoxStyle.Critical, "Error")
                Case Lang.Español
                    MsgBox("Debes ingresar una rotacion Z para la puerta cerrada", MsgBoxStyle.Critical, "Error")
                Case Lang.Portugues
                    MsgBox("Você deve digitar uma rotação Z para a porta fechada.", MsgBoxStyle.Critical, "Error")
                Case Else
                    MsgBox("Du musst eine Z-Rotation für die geschlossene Tür eingeben", MsgBoxStyle.Critical, "Fehler")
            End Select
            Exit Sub
        End If
        If TextBox22.Text = TextBox23.Text And RadioButton7.Checked = True And TextBox22.Text.Length > 0 Then
            RadioButton10.Checked = True
        End If
        If RadioButton7.Checked = True Then
            If TextBox22.Text.Length = 0 Then
                Select Case Config.Idioma
                    Case Lang.English
                        MsgBox("You must enter the name of the command to open the door.", MsgBoxStyle.Critical, "Error")
                    Case Lang.Español
                        MsgBox("Debes ingresar el nombre del comando para abrir la puerta.", MsgBoxStyle.Critical, "Error")
                    Case Lang.Portugues
                        MsgBox("Você deve digitar o nome do comando para abrir a porta.", MsgBoxStyle.Critical, "Error")
                    Case Else
                        MsgBox("Du musst einen Befehl zum Öffnen der Tür angeben.", MsgBoxStyle.Critical, "Fehler")
                End Select
                Exit Sub
            End If
            If TextBox23.Text.Length = 0 Then
                Select Case Config.Idioma
                    Case Lang.English
                        MsgBox("You must enter the name of the command to close the door.", MsgBoxStyle.Critical, "Error")
                    Case Lang.Español
                        MsgBox("Debes ingresar el nombre del comando para cerrar la puerta.", MsgBoxStyle.Critical, "Error")
                    Case Lang.Portugues
                        MsgBox("Você deve digitar o nome do comando para trancar a porta.", MsgBoxStyle.Critical, "Error")
                    Case Else
                        MsgBox("Du musst einen Befehl zum Schließen der Tür angeben.", MsgBoxStyle.Critical, "Fehler")
                End Select
                Exit Sub
            End If
            TextBox11.Text = "#include <a_samp>" & vbNewLine & _
                "new Gate;" & vbNewLine & vbNewLine
            If CheckBox4.Checked = True Then
                TextBox11.Text += "public OnFilterScriptInit()" & vbNewLine
            Else
                TextBox11.Text += "public OnGameModeInit()" & vbNewLine
            End If
            TextBox11.Text += "{" & vbNewLine & _
                vbTab & "Gate = CreateObject(" & TextBox24.Text & ", " & TextBox21.Text & ", " & TextBox20.Text & ", " & TextBox19.Text & ", " & TextBox81.Text & ", " & TextBox80.Text & ", " & TextBox17.Text & ", 100.0);" & vbNewLine & _
                vbTab & "return 1;" & vbNewLine & _
                "}" & vbNewLine & vbNewLine
            If RadioButton13.Checked = True Then
                TextBox11.Text += "public OnPlayerCommandText(playerid, cmdtext[])" & vbNewLine & _
                "{" & vbNewLine & _
                vbTab & "if(!strcmp(cmdtext, ""/" & TextBox22.Text & """, true)){" & vbNewLine & _
                vbTab & vbTab & "if(IsPlayerInRangeOfPoint(playerid, " & TextBox14.Text & ", " & TextBox21.Text & ", " & TextBox20.Text & ", " & TextBox19.Text & ")){" & vbNewLine & _
                vbTab & vbTab & vbTab & "MoveObject(Gate, " & TextBox15.Text & ", " & TextBox12.Text & ", " & TextBox13.Text & ", " & TextBox27.Text & ", " & TextBox79.Text & ", " & TextBox78.Text & ", " & TextBox77.Text & ");" & vbNewLine
                If CheckBox2.Checked = True Then
                    TextBox11.Text += vbTab & vbTab & vbTab & "return SendClientMessage(playerid, " & Config.C_Open.Name & ", """ & TextBox25.Text & """);" & vbNewLine
                Else
                    TextBox11.Text += vbTab & vbTab & vbTab & "return 1;" & vbNewLine
                End If
                TextBox11.Text += vbTab & vbTab & "}" & vbNewLine & _
                vbTab & "}" & vbNewLine & _
                vbTab & "else if(strcmp(cmdtext, ""/" & TextBox23.Text & """, true)){" & vbNewLine & _
                vbTab & vbTab & "if(IsPlayerInRangeOfPoint(playerid, " & TextBox18.Text & ", " & TextBox21.Text & ", " & TextBox20.Text & ", " & TextBox19.Text & ")){" & vbNewLine & _
                vbTab & vbTab & vbTab & "MoveObject(Gate, " & TextBox21.Text & ", " & TextBox20.Text & ", " & TextBox19.Text & ", " & TextBox27.Text & ", " & TextBox81.Text & ", " & TextBox80.Text & ", " & TextBox17.Text & ");" & vbNewLine
                If CheckBox2.Checked = True Then
                    TextBox11.Text += vbTab & vbTab & vbTab & "return SendClientMessage(playerid, " & Config.C_Close.Name & ", """ & TextBox26.Text & """);" & vbNewLine
                Else
                    TextBox11.Text += vbTab & vbTab & vbTab & "return 1;" & vbNewLine
                End If
                TextBox11.Text += vbTab & vbTab & "}" & vbNewLine & _
                vbTab & "}" & vbNewLine & _
                vbTab & "return 0; " & vbNewLine & _
                "}"
            ElseIf RadioButton12.Checked = True Then
                TextBox11.Text += "public OnPlayerCommandText(playerid, cmdtext[])" & vbNewLine & _
                "{" & vbNewLine & _
                vbTab & "dcmd(" & TextBox22.Text & ", " & TextBox22.Text.Length & ", cmdtext);" & vbNewLine & _
                vbTab & "dcmd(" & TextBox23.Text & ", " & TextBox23.Text.Length & ", cmdtext);" & vbNewLine & _
                vbTab & "return 0;" & vbNewLine & _
                "}" & vbNewLine & vbNewLine & _
                "dcmd_" & TextBox22.Text & "(playerid, params[])" & vbNewLine & _
                "{" & vbNewLine & _
                vbTab & "#pragma unused params" & vbNewLine & _
                vbTab & "if(IsPlayerInRangeOfPoint(playerid, " & TextBox14.Text & ", " & TextBox21.Text & ", " & TextBox20.Text & ", " & TextBox19.Text & ")){" & vbNewLine & _
                vbTab & vbTab & "MoveObject(Gate, " & TextBox15.Text & ", " & TextBox12.Text & ", " & TextBox13.Text & ", " & TextBox27.Text & ", " & TextBox79.Text & ", " & TextBox78.Text & ", " & TextBox77.Text & ");" & vbNewLine
                If CheckBox2.Checked = True Then
                    TextBox11.Text += vbTab & vbTab & "return SendClientMessage(playerid, " & Config.C_Open.Name & ", """ & TextBox25.Text & """);" & vbNewLine
                Else
                    TextBox11.Text += vbTab & vbTab & "return 1;" & vbNewLine
                End If
                TextBox11.Text += vbTab & "}" & vbNewLine & _
                "}" & vbNewLine & vbNewLine & _
                "dcmd_" & TextBox23.Text & "(playerid, params[])" & vbNewLine & _
                "{" & vbNewLine & _
                vbTab & "#pragma unused params" & vbNewLine & _
                vbTab & "if(IsPlayerInRangeOfPoint(playerid, " & TextBox18.Text & ", " & TextBox21.Text & ", " & TextBox20.Text & ", " & TextBox19.Text & ")){" & vbNewLine & _
                vbTab & vbTab & "MoveObject(Gate, " & TextBox21.Text & ", " & TextBox20.Text & ", " & TextBox19.Text & ", " & TextBox27.Text & ", " & TextBox81.Text & ", " & TextBox80.Text & ", " & TextBox17.Text & ");" & vbNewLine
                If CheckBox2.Checked = True Then
                    TextBox11.Text += vbTab & vbTab & "return SendClientMessage(playerid, " & Config.C_Close.Name & ", """ & TextBox26.Text & """);" & vbNewLine
                Else
                    TextBox11.Text += vbTab & vbTab & "return 1;" & vbNewLine
                End If
                TextBox11.Text += vbTab & "}" & vbNewLine & _
                "}" & vbNewLine & vbNewLine
            ElseIf RadioButton11.Checked = True Then
                TextBox11.Text += "CMD:" & TextBox22.Text & "(playerid, params[])" & vbNewLine & _
                "{" & vbNewLine & _
                vbTab & "if(IsPlayerInRangeOfPoint(playerid, " & TextBox14.Text & ", " & TextBox21.Text & ", " & TextBox20.Text & ", " & TextBox19.Text & ")){" & vbNewLine & _
                vbTab & vbTab & "MoveObject(Gate, " & TextBox15.Text & ", " & TextBox12.Text & ", " & TextBox13.Text & ", " & TextBox27.Text & ", " & TextBox79.Text & ", " & TextBox78.Text & ", " & TextBox77.Text & ");" & vbNewLine
                If CheckBox2.Checked = True Then
                    TextBox11.Text += vbTab & vbTab & "return SendClientMessage(playerid, " & Config.C_Open.Name & ", """ & TextBox25.Text & """);" & vbNewLine
                Else
                    TextBox11.Text += vbTab & vbTab & "return 1;" & vbNewLine
                End If
                TextBox11.Text += vbTab & "}" & vbNewLine & _
                "}" & vbNewLine & vbNewLine & _
                "CMD:" & TextBox23.Text & "(playerid, params[])" & vbNewLine & _
                "{" & vbNewLine & _
                vbTab & "if(IsPlayerInRangeOfPoint(playerid, " & TextBox18.Text & ", " & TextBox21.Text & ", " & TextBox20.Text & ", " & TextBox19.Text & ")){" & vbNewLine & _
                vbTab & vbTab & "MoveObject(Gate, " & TextBox21.Text & ", " & TextBox20.Text & ", " & TextBox19.Text & ", " & TextBox27.Text & ", " & TextBox81.Text & ", " & TextBox80.Text & ", " & TextBox17.Text & ");" & vbNewLine
                If CheckBox2.Checked = True Then
                    TextBox11.Text += vbTab & vbTab & "return SendClientMessage(playerid, " & Config.C_Close.Name & ", """ & TextBox26.Text & """);" & vbNewLine
                Else
                    TextBox11.Text += vbTab & vbTab & "return 1;" & vbNewLine
                End If
                TextBox11.Text += vbTab & "}" & vbNewLine & _
                "}" & vbNewLine & vbNewLine
            End If
        ElseIf RadioButton8.Checked = True Then
            If TextBox22.Text.Length = 0 Then
                Select Case Config.Idioma
                    Case Lang.English
                        MsgBox("You must enter the name of the command to open the door.", MsgBoxStyle.Critical, "Error")
                    Case Lang.Español
                        MsgBox("Debes ingresar el nombre del comando para abrir la puerta.", MsgBoxStyle.Critical, "Error")
                    Case Lang.Portugues
                        MsgBox("Você deve digitar o nome do comando para abrir a porta.", MsgBoxStyle.Critical, "Error")
                    Case Else
                        MsgBox("Du musst einen Befehl zum Öffnen der Tür angeben.", MsgBoxStyle.Critical, "Fehler")
                End Select
                Exit Sub
            End If
            If TextBox28.Text.Length = 0 Then
                Select Case Config.Idioma
                    Case Lang.English
                        MsgBox("You must enter a valid time to close the door.", MsgBoxStyle.Critical, "Error")
                    Case Lang.Español
                        MsgBox("Debes ingresar un tiempo valido para cerrar la puerta.", MsgBoxStyle.Critical, "Error")
                    Case Lang.Portugues
                        MsgBox("Você deve inserir um tempo válido para fechar a porta.", MsgBoxStyle.Critical, "Error")
                    Case Else
                        MsgBox("Du musst eine gültige Zeit zum Öffnen der Tür angeben.", MsgBoxStyle.Critical, "Fehler")
                End Select
                Exit Sub
            End If
            TextBox11.Text = "#include <a_samp>" & vbNewLine & _
                "new Gate;" & vbNewLine & vbNewLine
            If CheckBox4.Checked = True Then
                TextBox11.Text += "public OnFilterScriptInit()" & vbNewLine
            Else
                TextBox11.Text += "public OnGameModeInit()" & vbNewLine
            End If
            TextBox11.Text += "{" & vbNewLine & _
            vbTab & "Gate = CreateObject(" & TextBox24.Text & ", " & TextBox21.Text & ", " & TextBox20.Text & ", " & TextBox19.Text & ", " & TextBox81.Text & ", " & TextBox80.Text & ", " & TextBox17.Text & ", 100.0);" & vbNewLine & _
            vbTab & "return 1;" & vbNewLine & _
            "}" & vbNewLine & vbNewLine
            If RadioButton13.Checked = True Then
                TextBox11.Text += "public OnPlayerCommandText(playerid, cmdtext[])" & vbNewLine & _
                "{" & vbNewLine & _
                vbTab & "if(!strcmp(cmdtext, ""/" & TextBox22.Text & """, true)){" & vbNewLine & _
                vbTab & vbTab & "if(IsPlayerInRangeOfPoint(playerid, " & TextBox14.Text & ", " & TextBox21.Text & ", " & TextBox20.Text & ", " & TextBox19.Text & ")){" & vbNewLine & _
                vbTab & vbTab & vbTab & "SetTimerEx(""CloseGate"", " & TextBox28.Text & ", false, ""i"", playerid);" & vbNewLine & _
                vbTab & vbTab & vbTab & "MoveObject(Gate, " & TextBox15.Text & ", " & TextBox12.Text & ", " & TextBox13.Text & ", " & TextBox27.Text & ", " & TextBox79.Text & ", " & TextBox78.Text & ", " & TextBox77.Text & ");" & vbNewLine
                If CheckBox2.Checked = True Then
                    TextBox11.Text += vbTab & vbTab & vbTab & "return SendClientMessage(playerid, " & Config.C_Open.Name & ", """ & TextBox25.Text & """);" & vbNewLine
                Else
                    TextBox11.Text += vbTab & vbTab & vbTab & "return 1;" & vbNewLine
                End If
                TextBox11.Text += vbTab & vbTab & "}" & vbNewLine & _
                vbTab & "}" & vbNewLine & _
                vbTab & "return 0;" & vbNewLine & _
                "}" & vbNewLine & vbNewLine
            ElseIf RadioButton12.Checked = True Then
                TextBox11.Text += "public OnPlayerCommandText(playerid, cmdtext[])" & vbNewLine & _
                "{" & vbNewLine & _
                vbTab & "dcmd(" & TextBox22.Text & ", " & TextBox22.Text.Length & ", cmdtext);" & vbNewLine & _
                vbTab & "return 0;" & vbNewLine & _
                "}" & vbNewLine & vbNewLine & _
                "dcmd_" & TextBox22.Text & "(playerid, params[])" & vbNewLine & _
                "{" & vbNewLine & _
                vbTab & "#pragma unused params" & vbNewLine & _
                vbTab & "if(IsPlayerInRangeOfPoint(playerid, " & TextBox14.Text & ", " & TextBox21.Text & ", " & TextBox20.Text & ", " & TextBox19.Text & ")){" & vbNewLine & _
                vbTab & vbTab & "SetTimerEx(""CloseGate"", " & TextBox28.Text & ", false, ""i"", playerid);" & vbNewLine & _
                vbTab & vbTab & "MoveObject(Gate, " & TextBox15.Text & ", " & TextBox12.Text & ", " & TextBox13.Text & ", " & TextBox27.Text & ", " & TextBox79.Text & ", " & TextBox78.Text & ", " & TextBox77.Text & ");" & vbNewLine
                If CheckBox2.Checked = True Then
                    TextBox11.Text += vbTab & vbTab & "return SendClientMessage(playerid, " & Config.C_Open.Name & ", """ & TextBox25.Text & """);" & vbNewLine
                Else
                    TextBox11.Text += vbTab & vbTab & "return 1;" & vbNewLine
                End If
                TextBox11.Text += vbTab & "}" & vbNewLine & _
                "}" & vbNewLine & vbNewLine
            ElseIf RadioButton11.Checked = True Then
                TextBox11.Text += "CMD:" & TextBox22.Text & "(playerid, params[])" & vbNewLine & _
                "{" & vbNewLine & _
                vbTab & "if(IsPlayerInRangeOfPoint(playerid, " & TextBox14.Text & ", " & TextBox21.Text & ", " & TextBox20.Text & ", " & TextBox19.Text & ")){" & vbNewLine & _
                vbTab & vbTab & "SetTimerEx(""CloseGate"", " & TextBox28.Text & ", false, ""i"", playerid);" & vbNewLine & _
                vbTab & vbTab & "MoveObject(Gate, " & TextBox15.Text & ", " & TextBox12.Text & ", " & TextBox13.Text & ", " & TextBox27.Text & ", " & TextBox79.Text & ", " & TextBox78.Text & ", " & TextBox77.Text & ");" & vbNewLine
                If CheckBox2.Checked = True Then
                    TextBox11.Text += vbTab & vbTab & "return SendClientMessage(playerid, " & Config.C_Open.Name & ", """ & TextBox25.Text & """);" & vbNewLine
                Else
                    TextBox11.Text += vbTab & vbTab & "return 1;" & vbNewLine
                End If
                TextBox11.Text += vbTab & "}" & vbNewLine & _
                "}" & vbNewLine & vbNewLine
            End If
            TextBox11.Text += "forward CloseGate(playerid);" & vbNewLine & _
                "public CloseGate(playerid)" & vbNewLine & _
                "{" & vbNewLine & _
                vbTab & "MoveObject(Gate, " & TextBox21.Text & ", " & TextBox20.Text & ", " & TextBox19.Text & ", " & TextBox27.Text & ", " & TextBox81.Text & ", " & TextBox80.Text & ", " & TextBox17.Text & ");" & vbNewLine
            If CheckBox2.Checked = True Then TextBox11.Text += vbTab & "SendClientMessage(playerid, " & Config.C_Close.Name & ", """ & TextBox26.Text & """);" & vbNewLine
            TextBox11.Text += "}"
        ElseIf RadioButton9.Checked = True Then
            If TextBox28.Text.Length = 0 Then
                Select Case Config.Idioma
                    Case Lang.English
                        MsgBox("You must enter a valid time to open and close the door.", MsgBoxStyle.Critical, "Error")
                    Case Lang.Español
                        MsgBox("Debes ingresar un tiempo valido para abrir y cerrar la puerta.", MsgBoxStyle.Critical, "Error")
                    Case Lang.Portugues
                        MsgBox("Você deve inserir um tempo válido para abrir e fechar a porta.", MsgBoxStyle.Critical, "Error")
                    Case Else
                        MsgBox("Du musst eine gültige Zeit zum Öffnen und Schließen der Tür angeben.", MsgBoxStyle.Critical, "Fehler")
                End Select
                Exit Sub
            End If
            TextBox11.Text = "#include <a_samp>" & vbNewLine & _
                "new Gate, bool:GateClosed = true;" & vbNewLine & vbNewLine
            If CheckBox4.Checked = True Then
                TextBox11.Text += "public OnFilterScriptInit()" & vbNewLine
            Else
                TextBox11.Text += "public OnGameModeInit()" & vbNewLine
            End If
            TextBox11.Text += "{" & vbNewLine & _
            vbTab & "Gate = CreateObject(" & TextBox24.Text & ", " & TextBox21.Text & ", " & TextBox20.Text & ", " & TextBox19.Text & ", " & TextBox81.Text & ", " & TextBox80.Text & ", " & TextBox17.Text & ", 100.0);" & vbNewLine & _
            vbTab & "SetTimer(""GateCheck"", " & TextBox28.Text & ", true);" & vbNewLine & _
            vbTab & "return 1;" & vbNewLine & _
            "}" & vbNewLine & vbNewLine & _
            "forward GateCheck();" & vbNewLine & _
            "public GateCheck()" & vbNewLine & _
            "{" & vbNewLine & _
            vbTab & "if(!IsObjectMoving(Gate)){" & vbNewLine & _
            vbTab & vbTab & "for(new i; i<GetMaxPlayers(); i++){" & vbNewLine & _
            vbTab & vbTab & vbTab & "if(IsPlayerConnected(i && IsPlayerInRangeOfPoint(i, " & TextBox14.Text & ", " & TextBox21.Text & ", " & TextBox20.Text & ", " & TextBox19.Text & ")){" & vbNewLine & _
            vbTab & vbTab & vbTab & vbTab & "if(GateClosed){" & vbNewLine & _
            vbTab & vbTab & vbTab & vbTab & vbTab & "GateClosed = false;" & vbNewLine & _
            vbTab & vbTab & vbTab & vbTab & vbTab & "MoveObject(Gate, " & TextBox15.Text & ", " & TextBox12.Text & ", " & TextBox13.Text & ", " & TextBox27.Text & ", " & TextBox79.Text & ", " & TextBox78.Text & ", " & TextBox77.Text & ");" & vbNewLine
            If CheckBox2.Checked = True Then TextBox11.Text += vbTab & vbTab & vbTab & vbTab & vbTab & "SendClientMessage(i, " & Config.C_Open.Name & ", """ & TextBox25.Text & """);" & vbNewLine
            TextBox11.Text += vbTab & vbTab & vbTab & vbTab & "}" & vbNewLine & _
            vbTab & vbTab & vbTab & vbTab & "else{" & vbNewLine & _
            vbTab & vbTab & vbTab & vbTab & vbTab & "GateClosed = true;" & vbNewLine & _
            vbTab & vbTab & vbTab & vbTab & vbTab & "MoveObject(Gate, " & TextBox21.Text & ", " & TextBox20.Text & ", " & TextBox19.Text & ", " & TextBox27.Text & ", " & TextBox81.Text & ", " & TextBox80.Text & ", " & TextBox17.Text & ");" & vbNewLine
            If CheckBox2.Checked = True Then TextBox11.Text += vbTab & vbTab & vbTab & vbTab & vbTab & "SendClientMessage(i, " & Config.C_Close.Name & ", """ & TextBox26.Text & """);" & vbNewLine
            TextBox11.Text += vbTab & vbTab & vbTab & "}" & vbNewLine & _
            vbTab & vbTab & vbTab & "}" & vbNewLine & _
            vbTab & vbTab & "}" & vbNewLine & _
            vbTab & "}" & vbNewLine & _
            "}"
        ElseIf RadioButton10.Checked = True Then
            If TextBox22.Text.Length = 0 Then
                Select Case Config.Idioma
                    Case Lang.English
                        MsgBox("You must enter the name of the command to open and close the door.", MsgBoxStyle.Critical, "Error")
                    Case Lang.Español
                        MsgBox("Debes ingresar el nombre del comando para abrir y cerrar la puerta.", MsgBoxStyle.Critical, "Error")
                    Case Lang.Portugues
                        MsgBox("Você deve digitar o nome do comando para abrir e fechar a porta.", MsgBoxStyle.Critical, "Error")
                    Case Else
                        MsgBox("Du musst einen Befehl zum Öffnen und Schließen der Tür angeben.", MsgBoxStyle.Critical, "Fehler")
                End Select
                Exit Sub
            End If
            TextBox11.Text = "#include <a_samp>" & vbNewLine & _
                "new Gate, bool:GateClosed = true;" & vbNewLine & vbNewLine
            If CheckBox4.Checked = True Then
                TextBox11.Text += "public OnFilterScriptInit()" & vbNewLine
            Else
                TextBox11.Text += "public OnGameModeInit()" & vbNewLine
            End If
            TextBox11.Text += "{" & vbNewLine & _
            vbTab & "Gate = CreateObject(" & TextBox24.Text & ", " & TextBox21.Text & ", " & TextBox20.Text & ", " & TextBox19.Text & ", " & TextBox81.Text & ", " & TextBox80.Text & ", " & TextBox17.Text & ", 100.0);" & vbNewLine & _
            vbTab & "return 1;" & vbNewLine & _
            "}" & vbNewLine & vbNewLine
            If RadioButton13.Checked = True Then
                TextBox11.Text += "public OnPlayerCommandText(playerid, cmdtext[])" & vbNewLine & _
                "{" & vbNewLine & _
                vbTab & "if(!strcmp(cmdtext, ""/" & TextBox22.Text & """, true)){" & vbNewLine & _
                vbTab & vbTab & "if(GateClosed){" & vbNewLine & _
                vbTab & vbTab & vbTab & "if(IsPlayerInRangeOfPoint(playerid, " & TextBox14.Text & ", " & TextBox21.Text & ", " & TextBox20.Text & ", " & TextBox19.Text & ")){" & vbNewLine & _
                vbTab & vbTab & vbTab & vbTab & "GateClosed = false;" & vbNewLine & _
                vbTab & vbTab & vbTab & vbTab & "MoveObject(Gate, " & TextBox15.Text & ", " & TextBox12.Text & ", " & TextBox13.Text & ", " & TextBox27.Text & ", " & TextBox79.Text & ", " & TextBox78.Text & ", " & TextBox77.Text & ");" & vbNewLine
                If CheckBox2.Checked = True Then
                    TextBox11.Text += vbTab & vbTab & vbTab & vbTab & "return SendClientMessage(playerid, " & Config.C_Open.Name & ", """ & TextBox25.Text & """);" & vbNewLine
                Else
                    TextBox11.Text += vbTab & vbTab & vbTab & vbTab & "return 1;" & vbNewLine
                End If
                TextBox11.Text += vbTab & vbTab & vbTab & "}" & vbNewLine & _
                vbTab & vbTab & "}" & vbNewLine & _
                vbTab & vbTab & "else{" & vbNewLine & _
                vbTab & vbTab & vbTab & "if(IsPlayerInRangeOfPoint(playerid, " & TextBox18.Text & ", " & TextBox21.Text & ", " & TextBox20.Text & ", " & TextBox19.Text & ")){" & vbNewLine & _
                vbTab & vbTab & vbTab & vbTab & "GateClosed = true;" & vbNewLine & _
                vbTab & vbTab & vbTab & vbTab & "MoveObject(Gate, " & TextBox21.Text & ", " & TextBox20.Text & ", " & TextBox19.Text & ", " & TextBox27.Text & ", " & TextBox81.Text & ", " & TextBox80.Text & ", " & TextBox17.Text & ");" & vbNewLine
                If CheckBox2.Checked = True Then
                    TextBox11.Text += vbTab & vbTab & vbTab & vbTab & "return SendClientMessage(playerid, " & Config.C_Close.Name & ", """ & TextBox26.Text & """);" & vbNewLine
                Else
                    TextBox11.Text += vbTab & vbTab & vbTab & vbTab & "return 1;" & vbNewLine
                End If
                TextBox11.Text += vbTab & vbTab & vbTab & "}" & vbNewLine & _
                 vbTab & vbTab & "}" & vbNewLine & _
                vbTab & "}" & vbNewLine & _
                vbTab & "return 0;" & vbNewLine & _
                "}"
            ElseIf RadioButton12.Checked = True Then
                TextBox11.Text += "public OnPlayerCommandText(playerid, cmdtext[])" & vbNewLine & _
                "{" & vbNewLine & _
                vbTab & "dcmd(" & TextBox22.Text & ", " & TextBox22.Text.Length & ", cmdtext);" & vbNewLine & _
                vbTab & "return 0;" & vbNewLine & _
                "}" & vbNewLine & vbNewLine & _
                "dcmd_" & TextBox22.Text & "(playerid, params[])" & vbNewLine & _
                "{" & vbNewLine & _
                vbTab & "#pragma unused params" & vbNewLine & _
                vbTab & "if(GateClosed){" & vbNewLine & _
                vbTab & vbTab & "if(IsPlayerInRangeOfPoint(playerid, " & TextBox14.Text & ", " & TextBox21.Text & ", " & TextBox20.Text & ", " & TextBox19.Text & ")){" & vbNewLine & _
                vbTab & vbTab & vbTab & "GateClosed = false;" & vbNewLine & _
                vbTab & vbTab & vbTab & "MoveObject(Gate, " & TextBox15.Text & ", " & TextBox12.Text & ", " & TextBox13.Text & ", " & TextBox27.Text & ", " & TextBox79.Text & ", " & TextBox78.Text & ", " & TextBox77.Text & ");" & vbNewLine
                If CheckBox2.Checked = True Then
                    TextBox11.Text += vbTab & vbTab & vbTab & "return SendClientMessage(playerid, " & Config.C_Open.Name & ", """ & TextBox25.Text & """);" & vbNewLine
                Else
                    TextBox11.Text += vbTab & vbTab & vbTab & "return 1;" & vbNewLine
                End If
                TextBox11.Text += vbTab & vbTab & "}" & vbNewLine & _
                vbTab & "}" & vbNewLine & _
                vbTab & "else{" & vbNewLine & _
                vbTab & vbTab & "if(IsPlayerInRangeOfPoint(playerid, " & TextBox18.Text & ", " & TextBox21.Text & ", " & TextBox20.Text & ", " & TextBox19.Text & ")){" & vbNewLine & _
                vbTab & vbTab & vbTab & "GateClosed = true;" & vbNewLine & _
                vbTab & vbTab & vbTab & "MoveObject(Gate, " & TextBox21.Text & ", " & TextBox20.Text & ", " & TextBox19.Text & ", " & TextBox27.Text & ", " & TextBox81.Text & ", " & TextBox80.Text & ", " & TextBox17.Text & ");" & vbNewLine
                If CheckBox2.Checked = True Then
                    TextBox11.Text += vbTab & vbTab & vbTab & "return SendClientMessage(playerid, " & Config.C_Close.Name & ", """ & TextBox26.Text & """);" & vbNewLine
                Else
                    TextBox11.Text += vbTab & vbTab & vbTab & "return 1;" & vbNewLine
                End If
                TextBox11.Text += vbTab & vbTab & "}" & vbNewLine & _
                vbTab & "}" & vbNewLine & _
                "}"
            ElseIf RadioButton11.Checked = True Then
                TextBox11.Text += "CMD:" & TextBox22.Text & "(playerid, params[])" & vbNewLine & _
                "{" & vbNewLine & _
                vbTab & "if(GateClosed){" & vbNewLine & _
                vbTab & vbTab & "if(IsPlayerInRangeOfPoint(playerid, " & TextBox14.Text & ", " & TextBox21.Text & ", " & TextBox20.Text & ", " & TextBox19.Text & ")){" & vbNewLine & _
                vbTab & vbTab & vbTab & "GateClosed = false;" & vbNewLine & _
                vbTab & vbTab & vbTab & "MoveObject(Gate, " & TextBox15.Text & ", " & TextBox12.Text & ", " & TextBox13.Text & ", " & TextBox27.Text & ", " & TextBox79.Text & ", " & TextBox78.Text & ", " & TextBox77.Text & ");" & vbNewLine
                If CheckBox2.Checked = True Then
                    TextBox11.Text += vbTab & vbTab & vbTab & "return SendClientMessage(playerid, " & Config.C_Open.Name & ", """ & TextBox25.Text & """);" & vbNewLine
                Else
                    TextBox11.Text += vbTab & vbTab & vbTab & "return 1;" & vbNewLine
                End If
                TextBox11.Text += vbTab & vbTab & "}" & vbNewLine & _
                vbTab & "}" & vbNewLine & _
                vbTab & "else{" & vbNewLine & _
                vbTab & vbTab & "if(IsPlayerInRangeOfPoint(playerid, " & TextBox18.Text & ", " & TextBox21.Text & ", " & TextBox20.Text & ", " & TextBox19.Text & ")){" & vbNewLine & _
                vbTab & vbTab & vbTab & "GateClosed = true;" & vbNewLine & _
                vbTab & vbTab & vbTab & "MoveObject(Gate, " & TextBox21.Text & ", " & TextBox20.Text & ", " & TextBox19.Text & ", " & TextBox27.Text & ", " & TextBox81.Text & ", " & TextBox80.Text & ", " & TextBox17.Text & ");" & vbNewLine
                If CheckBox2.Checked = True Then
                    TextBox11.Text += vbTab & vbTab & vbTab & "return SendClientMessage(playerid, " & Config.C_Close.Name & ", """ & TextBox26.Text & """);" & vbNewLine
                Else
                    TextBox11.Text += vbTab & vbTab & vbTab & "return 1;" & vbNewLine
                End If
                TextBox11.Text += vbTab & vbTab & "}" & vbNewLine & _
                vbTab & "}" & vbNewLine & _
                "}"
            End If
        End If
    End Sub

#End Region

#Region "Extra"

    Private Sub TabPage2_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles TabPage2.Leave
        CheckBox7.Checked = True
        TextBox11.Clear()
        TextBox22.Clear()
        TextBox23.Clear()
        TextBox24.Clear()
        TextBox25.Clear()
        TextBox26.Clear()
        TextBox12.Text = "0.0"
        TextBox13.Text = "0.0"
        TextBox14.Text = "15.0"
        TextBox15.Text = "0.0"
        TextBox18.Text = "15.0"
        TextBox19.Text = "0.0"
        TextBox20.Text = "0.0"
        TextBox21.Text = "0.0"
        TextBox27.Text = "3.0"
        TextBox28.Text = "7000"
    End Sub

#End Region

#End Region

#Region "Puckups"

#Region "Texts Restrictions"

    Private Sub TextBox29_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox29.KeyPress
        If Not IsNumeric(e.KeyChar) And Not Char.IsControl(e.KeyChar) And Asc(e.KeyChar) <> 8 Then e.Handled = True
    End Sub

    Private Sub TextBox31_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox31.KeyPress
        If Not IsNumeric(e.KeyChar) And Not Char.IsControl(e.KeyChar) And e.KeyChar <> "." And e.KeyChar <> "-" And Asc(e.KeyChar) <> 8 Then e.Handled = True
        If e.KeyChar = "." And TextBox31.Text.IndexOf(".") <> -1 Then e.Handled = True
    End Sub

    Private Sub TextBox32_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox32.KeyPress
        If Not IsNumeric(e.KeyChar) And Not Char.IsControl(e.KeyChar) And Asc(e.KeyChar) <> 8 Then e.Handled = True
    End Sub

    Private Sub TextBox35_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox35.KeyPress
        If Not IsNumeric(e.KeyChar) And Not Char.IsControl(e.KeyChar) And e.KeyChar <> "." And e.KeyChar <> "-" And Asc(e.KeyChar) <> 8 Then e.Handled = True
        If e.KeyChar = "." And TextBox35.Text.IndexOf(".") <> -1 Then e.Handled = True
    End Sub

    Private Sub TextBox36_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox36.KeyPress
        If Not IsNumeric(e.KeyChar) And Not Char.IsControl(e.KeyChar) And e.KeyChar <> "." And e.KeyChar <> "-" And Asc(e.KeyChar) <> 8 Then e.Handled = True
        If e.KeyChar = "." And TextBox36.Text.IndexOf(".") <> -1 Then e.Handled = True
    End Sub

    Private Sub TextBox29_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox29.TextChanged
        TextBox29.Text = Regex.Replace(TextBox29.Text, BadChars, "")
    End Sub

    Private Sub TextBox31_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox31.TextChanged
        TextBox31.Text = Regex.Replace(TextBox31.Text, BadChars, "")
    End Sub

    Private Sub TextBox32_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox32.TextChanged
        TextBox32.Text = Regex.Replace(TextBox32.Text, BadChars, "")
    End Sub

    Private Sub TextBox35_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox35.TextChanged
        TextBox35.Text = Regex.Replace(TextBox35.Text, BadChars, "")
    End Sub

    Private Sub TextBox36_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox36.TextChanged
        TextBox36.Text = Regex.Replace(TextBox36.Text, BadChars, "")
    End Sub

#End Region

#Region "Help"

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        Select Case ComboBox1.SelectedItem
            Case 0
                Select Case Config.Idioma
                    Case Lang.English
                        TextBox34.Text = "The pickup does not display."
                    Case Lang.Español
                        TextBox34.Text = "El pickup no es visible."
                    Case Lang.Portugues
                        TextBox34.Text = "A pickup não será exibido."
                    Case Else
                        TextBox34.Text = "Das Pickup wird nicht angezeigt."
                End Select
            Case 1
                Select Case Config.Idioma
                    Case Lang.English
                        TextBox34.Text = "Not pickupable, exists all the time (Suitable for completely scripted pickups using OnPlayerPickUpPickup)."
                    Case Lang.Español
                        TextBox34.Text = "No se puede recoger, existe todo el tiempo (Adecuado para pickups totalmente scripteados mediante OnPlayerPickUpPickup)."
                    Case Lang.Portugues
                        TextBox34.Text = "Você não pode escolher, lá o tempo todo (Adequado para a plena roteiro de pickups OnPlayerPickUpPickup)."
                    Case Else
                        TextBox34.Text = "Nicht einsammelbar, existiert dauerhaft (praktisch zur Verwendung in OnPlayerPickUpPickup)"
                End Select
            Case 2
                Select Case Config.Idioma
                    Case Lang.English
                        TextBox34.Text = "Pickupable, respawns after some time."
                    Case Lang.Español
                        TextBox34.Text = "Se puede recoger, aparece luego de un tiempo."
                    Case Lang.Portugues
                        TextBox34.Text = "Você pode pegar, aparece depois de um tempo."
                    Case Else
                        TextBox34.Text = "Einsammelbar, spawnt nach einiger Zeit neu."
                End Select
            Case 3, 15, 22
                Select Case Config.Idioma
                    Case Lang.English
                        TextBox34.Text = "Pickupable, respawns after death."
                    Case Lang.Español
                        TextBox34.Text = "Se puede recoger, aparece luego al morir el jugador."
                    Case Lang.Portugues
                        TextBox34.Text = "Você pode pegar, aparece após a morte do jogador."
                    Case Else
                        TextBox34.Text = "Einsammelbar, spawnt nach dem Tod neu."
                End Select
            Case 4, 5
                Select Case Config.Idioma
                    Case Lang.English
                        TextBox34.Text = "Disappears shortly after created (perhaps for weapon drops?)."
                    Case Lang.Español
                        TextBox34.Text = "Desaparece poco tiempo despues de ser creado (tal vez para arrojar armas?)."
                    Case Lang.Portugues
                        TextBox34.Text = "Embora logo depois de ser criado (talvez para lançar armas?)."
                    Case Else
                        TextBox34.Text = "Verschwindet kurz nach der Erstellung (evtl. für Waffen?)"
                End Select
            Case 8
                Select Case Config.Idioma
                    Case Lang.English
                        TextBox34.Text = "Pickupable, but has no effect. Disappears automatically."
                    Case Lang.Español
                        TextBox34.Text = "Se puede recoger pero no tiene efecto. Desaparece automaticamente."
                    Case Lang.Portugues
                        TextBox34.Text = "Você pode pegar, mas não tem efeito. Desaparece automaticamente."
                    Case Else
                        TextBox34.Text = "Einsammelbar, hat aber keinen Effekt und verschwindet von allein."
                End Select
            Case 11, 12
                Select Case Config.Idioma
                    Case Lang.English
                        TextBox34.Text = "Blows up a few seconds after being created (bombs?)."
                    Case Lang.Español
                        TextBox34.Text = "Explota unos segundos despues de ser creado (Bombas?)."
                    Case Lang.Portugues
                        TextBox34.Text = "Explode alguns segundos depois de ter sido criado (bombas?)."
                    Case Else
                        TextBox34.Text = "Explodiert ein paar Sekunden nach Erstellung (Bomben?)"
                End Select
            Case 13
                Select Case Config.Idioma
                    Case Lang.English
                        TextBox34.Text = "Slowly decends to the ground."
                    Case Lang.Español
                        TextBox34.Text = "Desciende lentamente al piso."
                    Case Lang.Portugues
                        TextBox34.Text = "Descer lentamente para o chão."
                    Case Else
                        TextBox34.Text = "Sinkt langam zum Boden."
                End Select
            Case 14
                Select Case Config.Idioma
                    Case Lang.English
                        TextBox34.Text = "Pickupable, but only when in a vehicle."
                    Case Lang.Español
                        TextBox34.Text = "Se puede recoger, pero solo con un vehiculo."
                    Case Lang.Portugues
                        TextBox34.Text = "Você pode pegar, mas apenas com um veículo."
                    Case Else
                        TextBox34.Text = "Einsammelbar, aber nur in Fahrzeugen."
                End Select
            Case 19
                Select Case Config.Idioma
                    Case Lang.English
                        TextBox34.Text = "Pickupable, but has no effect (information icons?)."
                    Case Lang.Español
                        TextBox34.Text = "Se puede recoger, pero no tiene efecto (iconos de informacion?)."
                    Case Lang.Portugues
                        TextBox34.Text = "Você pode pegar, mas não tem efeito (ícones de informação?)."
                    Case Else
                        TextBox34.Text = "Einsammelbar, aber ohne Effekt (Info-Symbole?)"
                End Select
            Case 23
                Select Case Config.Idioma
                    Case Lang.English
                        TextBox34.Text = "Pickupable, but doesn't disappear on pickup."
                    Case Lang.Español
                        TextBox34.Text = "Se puede recoger, pero no desaparece."
                    Case Lang.Portugues
                        TextBox34.Text = "Você pode pegar, mas não desaparece."
                    Case Else
                        TextBox34.Text = "Einsammelbar, verschwindet dann aber nicht."
                End Select
        End Select
    End Sub

    Private Sub LinkLabel1_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Process.Start("http://wiki.sa-mp.com/wiki/Pickup_help")
    End Sub

#End Region

#Region "Generate Code"

    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button7.Click
        TextBox33.Clear()
        If TextBox30.Text.Length = 0 Then
            Select Case Config.Idioma
                Case Lang.English
                    MsgBox("You must enter the variable name.", MsgBoxStyle.Critical, "Error")
                Case Lang.Español
                    MsgBox("Debes ingresar el nombre para la variable.", MsgBoxStyle.Critical, "Error")
                Case Lang.Portugues
                    MsgBox("Você deve digitar o nome da variável.", MsgBoxStyle.Critical, "Error")
                Case Else
                    MsgBox("Du musst den Namen der Variable angeben.", MsgBoxStyle.Critical, "Fehler")
            End Select
            Exit Sub
        End If
        If TextBox29.Text.Length = 0 Then
            Select Case Config.Idioma
                Case Lang.English
                    MsgBox("You must enter a pickup model.", MsgBoxStyle.Critical, "Error")
                Case Lang.Español
                    MsgBox("Debes ingresar un modelo de pickup.", MsgBoxStyle.Critical, "Error")
                Case Lang.Portugues
                    MsgBox("Você deve digitar um modelo pickup.", MsgBoxStyle.Critical, "Error")
                Case Else
                    MsgBox("Du musst eine Pickup Model ID eingeben.", MsgBoxStyle.Critical, "Fehler")
            End Select
            Exit Sub
        End If
        If TextBox36.Text.Length = 0 Then
            Select Case Config.Idioma
                Case Lang.English
                    MsgBox("You must enter a X coordinate.", MsgBoxStyle.Critical, "Error")
                Case Lang.Español
                    MsgBox("Debes ingresar una coordenada X.", MsgBoxStyle.Critical, "Error")
                Case Lang.Portugues
                    MsgBox("Você deve digitar uma coordenada X.", MsgBoxStyle.Critical, "Error")
                Case Else
                    MsgBox("Du musst eine X-Koordinate eingeben.", MsgBoxStyle.Critical, "Fehler")
            End Select
            Exit Sub
        End If
        If TextBox35.Text.Length = 0 Then
            Select Case Config.Idioma
                Case Lang.English
                    MsgBox("You must enter a Y coordinate.", MsgBoxStyle.Critical, "Error")
                Case Lang.Español
                    MsgBox("Debes ingresar una coordenada Y.", MsgBoxStyle.Critical, "Error")
                Case Lang.Portugues
                    MsgBox("Você deve digitar uma coordenada Y.", MsgBoxStyle.Critical, "Error")
                Case Else
                    MsgBox("Du musst eine Y-Koordinate eingeben.", MsgBoxStyle.Critical, "Fehler")
            End Select
            Exit Sub
        End If
        If TextBox31.Text.Length = 0 Then
            Select Case Config.Idioma
                Case Lang.English
                    MsgBox("You must enter a Z coordinate.", MsgBoxStyle.Critical, "Error")
                Case Lang.Español
                    MsgBox("Debes ingresar una coordenada Z.", MsgBoxStyle.Critical, "Error")
                Case Lang.Portugues
                    MsgBox("Você deve digitar uma coordenada Z.", MsgBoxStyle.Critical, "Error")
                Case Else
                    MsgBox("Du musst eine Z-Koordinate eingeben.", MsgBoxStyle.Critical, "Fehler")
            End Select
            Exit Sub
        End If
        If TextBox32.Text.Length = 0 Then
            Select Case Config.Idioma
                Case Lang.English
                    MsgBox("You must enter a world ID.", MsgBoxStyle.Critical, "Error")
                Case Lang.Español
                    MsgBox("Debes ingresar el id de un mundo", MsgBoxStyle.Critical, "Error")
                Case Lang.Portugues
                    MsgBox("Você deve digitar um ID mundo.", MsgBoxStyle.Critical, "Error")
                Case Else
                    MsgBox("Du musst eine Welt ID eingeben.", MsgBoxStyle.Critical, "Fehler")
            End Select
            Exit Sub
        End If
        TextBox33.Text = "#include <a_samp>" & vbNewLine & _
            "new " & TextBox30.Text & ";" & vbNewLine & vbNewLine
        If CheckBox5.Checked = True Then
            TextBox33.Text += "public OnFilterSciptInit()" & vbNewLine
        Else
            TextBox33.Text += "public OnGameModeInit()" & vbNewLine
        End If
        TextBox33.Text += "{" & vbNewLine & _
        vbTab & TextBox30.Text & " = CreatePickup(" & TextBox29.Text & ", " & ComboBox1.Text & ", " & TextBox36.Text & ", " & TextBox35.Text & ", " & TextBox31.Text & ", " & TextBox32.Text & ");" & vbNewLine & _
        vbTab & "return 1;" & vbNewLine & _
        "}" & vbNewLine & vbNewLine & _
        "public OnPlayerPickUpPickup(playerid, pickupid)" & vbNewLine & _
        "{" & vbNewLine & _
        vbTab & "if(pickupid == " & TextBox30.Text & "){" & vbNewLine & _
        vbTab & vbTab & "//Do something here" & vbNewLine & _
        vbTab & "}" & vbNewLine & _
        vbTab & "return 1;" & vbNewLine & _
        "}"
    End Sub

#End Region

#Region "Extra"

    Private Sub TabPage3_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles TabPage3.Leave
        TextBox29.Clear()
        TextBox30.Clear()
        TextBox33.Clear()
        TextBox31.Text = "0.0"
        TextBox35.Text = "0.0"
        TextBox36.Text = "0.0"
        TextBox32.Text = "-1"
        ComboBox1.SelectedIndex = 0
        CheckBox5.Checked = False
    End Sub

#End Region

#End Region

#Region "Dialogs"

#Region "Help"

    Private Sub TextBox40_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox40.GotFocus
        Select Case Config.Idioma
            Case Lang.English
                If TextBox40.Text.IndexOf("PRESS ENTER TO CREATE A JUMP LINE ON THE TEXT") <> -1 Then
                    TextBox40.Clear()
                    TextBox40.ForeColor = Color.Black
                End If
            Case Lang.Español
                If TextBox40.Text.IndexOf("PRESIONA ENTER PARA CREAR UN SALTO DE LINEA EN EL TEXTO") <> -1 Then
                    TextBox40.Clear()
                    TextBox40.ForeColor = Color.Black
                End If
            Case Lang.Portugues
                If TextBox40.Text.IndexOf("PRESSIONE ENTER PARA CRIAR UM SALTO DE LINHA NO TEXTO") <> -1 Then
                    TextBox40.Clear()
                    TextBox40.ForeColor = Color.Black
                End If
            Case Else
                If TextBox40.Text.IndexOf("DRÜCKE ENTER, UM EINE LEERZEILE ZU ERSTELLEN, ODER") <> -1 Then
                    TextBox40.Clear()
                    TextBox40.ForeColor = Color.Black
                End If
        End Select
    End Sub

    Private Sub TextBox40_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox40.LostFocus
        If TextBox40.Text.Length = 0 Then
            Select Case Config.Idioma
                Case Lang.English
                    TextBox40.Text = vbNewLine & vbNewLine & "PRESS ENTER TO CREATE A JUMP LINE ON THE TEXT OR" & vbNewLine & "TAB TO CREATE A TABULATION"
                    TextBox40.ForeColor = Color.Gray
                Case Lang.Español
                    TextBox40.Text = vbNewLine & vbNewLine & "PRESIONA ENTER PARA CREAR UN SALTO DE LINEA EN EL TEXTO O" & vbNewLine & "TAB PARA CREAR UNA TABULACIÓN"
                    TextBox40.ForeColor = Color.Gray
                Case Lang.Portugues
                    TextBox40.Text = vbNewLine & vbNewLine & "PRESSIONE ENTER PARA CRIAR UM SALTO DE LINHA NO TEXTO OU" & vbNewLine & "TAB PARA CRIAR UMA TABULAÇÃO"
                    TextBox40.ForeColor = Color.Gray
                Case Else
                    TextBox40.Text = vbNewLine & vbNewLine & "DRÜCKE ENTER, UM EINE LEERZEILE ZU ERSTELLEN, ODER" & vbNewLine & "TAB, UM EINE EINRÜCKUNG ZU ERSTELLEN"
                    TextBox40.ForeColor = Color.Gray
            End Select
        End If
    End Sub

    Private Sub TextBox40_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox40.KeyPress
        If Asc(e.KeyChar) = 9 Then
            TextBox40.SelectedText = "\t"
            e.Handled = True
        ElseIf Asc(e.KeyChar) = 13 Then
            TextBox40.SelectedText = "\n"
            e.Handled = True
        End If
    End Sub

#End Region

#Region "Preview"

    Private Sub Button9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button9.Click
        Previewer.RichTextBox1.Clear()
        Previewer.RichTextBox2.Clear()
        If ComboBox2.SelectedIndex = 2 Then
            Previewer.RichTextBox1.Text = ProcessText(TextBox40.Text, True)
        Else
            Previewer.RichTextBox1.Text = ProcessText(TextBox40.Text, False)
        End If
        Previewer.RichTextBox2.Text = TextBox39.Text
        ProcessColor(Previewer.RichTextBox1)
        ProcessColor(Previewer.RichTextBox2)
        Previewer.TextBox3.Text = TextBox41.Text
        Previewer.TextBox2.Text = TextBox42.Text
        Previewer.Show()
        Previewer.Top = Me.Top + 100
        Previewer.Left = Me.Left + 200
        Previewer.TopMost = True
    End Sub

#End Region

#Region "Generate Code"

    Private Sub Button8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button8.Click
        Dim DID As String
        TextBox37.Clear()
        If TextBox38.Text.Length = 0 Then
            Select Case Config.Idioma
                Case Lang.English
                    MsgBox("You must enter a id/name for the dialogue.", MsgBoxStyle.Critical, "Error")
                Case Lang.Español
                    MsgBox("Debes ingresar un id/nombre para el dialogo.", MsgBoxStyle.Critical, "Error")
                Case Lang.Portugues
                    MsgBox("Você deve digitar um id/nome para o diálogo.", MsgBoxStyle.Critical, "Error")
                Case Else
                    MsgBox("Du musst eine ID/einen Namen für den Dialog angeben.", MsgBoxStyle.Critical, "Fehler")
            End Select
            Exit Sub
        End If
        If TextBox39.Text.Length = 0 Then
            Select Case Config.Idioma
                Case Lang.English
                    MsgBox("You must enter a title for the dialog.", MsgBoxStyle.Critical, "Error")
                Case Lang.Español
                    MsgBox("Debes ingresar un titulo para el dialogo.", MsgBoxStyle.Critical, "Error")
                Case Lang.Portugues
                    MsgBox("Você deve digitar um título para o diálogo.", MsgBoxStyle.Critical, "Error")
                Case Else
                    MsgBox("Du musst einen Titel für den Dialog eingeben.", MsgBoxStyle.Critical, "Fehler")
            End Select
            Exit Sub
        End If
        If TextBox41.Text.Length = 0 Then
            Select Case Config.Idioma
                Case Lang.English
                    MsgBox("You must enter text for the button 1.", MsgBoxStyle.Critical, "Error")
                Case Lang.Español
                    MsgBox("Debes entrar un texto para el boton 1.", MsgBoxStyle.Critical, "Error")
                Case Lang.Portugues
                    MsgBox("Inserir o texto para o botão 1.", MsgBoxStyle.Critical, "Error")
                Case Else
                    MsgBox("Du musst einen Text für Knopf 1 angeben.", MsgBoxStyle.Critical, "Fehler")
            End Select
            Exit Sub
        End If
        If TextBox40.Text.Length = 0 Then
            Select Case Config.Idioma
                Case Lang.English
                    MsgBox("You must enter a text for the dialog.", MsgBoxStyle.Critical, "Error")
                Case Lang.Español
                    MsgBox("Debes ingresar un texto para el dialogo.", MsgBoxStyle.Critical, "Error")
                Case Lang.Portugues
                    MsgBox("Você deve digitar um texto para o diálogo.", MsgBoxStyle.Critical, "Error")
                Case Else
                    MsgBox("Du musst einen Text für den Dialog eingeben.", MsgBoxStyle.Critical, "Fehler")
            End Select
            Exit Sub
        End If
        If TextBox43.Text.Length = 0 Then
            Select Case Config.Idioma
                Case Lang.English
                    MsgBox("You must enter the id of the player to be shown the dialog.", MsgBoxStyle.Critical, "Error")
                Case Lang.Español
                    MsgBox("Debes ingresar el id del jugador al que se le mostrara el dialogo.", MsgBoxStyle.Critical, "Error")
                Case Lang.Portugues
                    MsgBox("Você deve digitar o ID do jogador a ser mostrado o diálogo.", MsgBoxStyle.Critical, "Error")
                Case Else
                    MsgBox("Du musst die ID des Spielers angeben, dem der Dialog gezeigt werden soll.", MsgBoxStyle.Critical, "Fehler")
            End Select
            Exit Sub
        End If
        If Not IsNumeric(TextBox38.Text) Then
            Dim R As New Random
            TextBox37.Text = "#define DIALOG_" & TextBox38.Text & "   " & R.Next(0, 32767) & vbNewLine & vbNewLine
            DID = "DIALOG_" & TextBox38.Text
        Else
            If TextBox38.Text > 32767 Then
                DID = 32767
            Else
                DID = TextBox38.Text
            End If
        End If
        TextBox37.Text += "ShowPlayerDialog(" & TextBox43.Text & ", " & DID & ", " & ComboBox2.Text & ", """ & TextBox39.Text & """, """ & TextBox40.Text & """, """ & TextBox41.Text & """, """ & TextBox42.Text & """);" & vbNewLine & vbNewLine & _
        "public OnDialogResponse(playerid, dialogid, response, listitem, inputtext[])" & vbNewLine & _
        "{" & vbNewLine & _
        vbTab & "if(dialogid == " & DID & "){" & vbNewLine
        Select Case ComboBox2.SelectedIndex
            Case 0
                If TextBox42.Text.Length > 0 Then
                    TextBox37.Text += vbTab & vbTab & "if(response){" & vbNewLine & _
                    vbTab & vbTab & vbTab & "//The player has pressed """ & TextBox41.Text & """." & vbNewLine & _
                    vbTab & vbTab & "}" & vbNewLine & _
                    vbTab & vbTab & "else{" & vbNewLine & _
                    vbTab & vbTab & vbTab & "//The player has pressed """ & TextBox42.Text & """." & vbNewLine & _
                    vbTab & vbTab & "}" & vbNewLine
                Else
                    TextBox37.Text += vbTab & vbTab & "//The player has pressed """ & TextBox41.Text & """(because it's de only avaliable button.)" & vbNewLine
                End If
            Case 1
                If TextBox42.Text.Length > 0 Then
                    TextBox37.Text += vbTab & vbTab & "if(response){" & vbNewLine & _
                    vbTab & vbTab & vbTab & "//The player has pressed """ & TextBox41.Text & """." & vbNewLine & _
                    vbTab & vbTab & vbTab & "}" & vbNewLine & _
                    vbTab & vbTab & "else{" & vbNewLine & _
                    vbTab & vbTab & vbTab & "//The player has pressed """ & TextBox42.Text & """." & vbNewLine & _
                    vbTab & vbTab & "}" & vbNewLine
                Else
                    TextBox37.Text += vbTab & vbTab & "//The player has pressed """ & TextBox41.Text & """(because it's de only avaliable button.)"
                End If
            Case 2
                Dim count = Counter(TextBox40.Text, "\n")
                If TextBox42.Text.Length > 0 Then
                    TextBox37.Text += vbTab & vbTab & "if(response){" & vbNewLine & _
                     vbTab & vbTab & vbTab & "switch(listitem)){" & vbNewLine
                    For i = 0 To count
                        TextBox37.Text += vbTab & vbTab & vbTab & vbTab & "case " & i & ":" & vbNewLine & _
                        vbTab & vbTab & vbTab & vbTab & "{" & vbNewLine & _
                        vbTab & vbTab & vbTab & vbTab & vbTab & "//Selected Item: """ & GetLine(TextBox40.Text, i) & """" & vbNewLine & _
                        vbTab & vbTab & vbTab & vbTab & "}" & vbNewLine
                    Next
                    TextBox37.Text += vbTab & vbTab & vbTab & "}" & vbNewLine & _
                        vbTab & vbTab & "}" & vbNewLine & _
                        vbTab & vbTab & "else{" & vbNewLine & _
                        vbTab & vbTab & vbTab & "//The player has pressed """ & TextBox41.Text & """." & vbNewLine & _
                        vbTab & vbTab & vbTab & "switch(listitem{" & vbNewLine
                    For i = 0 To count
                        TextBox37.Text += vbTab & vbTab & vbTab & vbTab & "case " & i & ":" & vbNewLine & _
                        vbTab & vbTab & vbTab & vbTab & "{" & vbNewLine & _
                        vbTab & vbTab & vbTab & vbTab & vbTab & "//Selected Item: """ & GetLine(TextBox40.Text, i) & """" & vbNewLine & _
                        vbTab & vbTab & vbTab & vbTab & "}" & vbNewLine
                    Next
                    TextBox37.Text += vbTab & vbTab & vbTab & "}" & vbNewLine & _
                        vbTab & vbTab & "}" & vbNewLine
                Else
                    TextBox37.Text += vbTab & vbTab & "switch(listitem)){" & vbNewLine
                    For i = 0 To count
                        TextBox37.Text += vbTab & vbTab & vbTab & "case " & i & ":" & vbNewLine & _
                        vbTab & vbTab & vbTab & "{" & vbNewLine & _
                        vbTab & vbTab & vbTab & vbTab & "//Selected Item: """ & GetLine(TextBox40.Text, i) & """" & vbNewLine & _
                        vbTab & vbTab & vbTab & "}" & vbNewLine
                    Next
                    TextBox37.Text += vbTab & vbTab & "}" & vbNewLine
                End If
            Case 3
                If TextBox42.Text.Length > 0 Then
                    TextBox37.Text += vbTab & vbTab & "if(response){" & vbNewLine & _
                    vbTab & vbTab & vbTab & "//The player has pressed """ & TextBox41.Text & """." & vbNewLine & _
                    vbTab & vbTab & "}" & vbNewLine & _
                    vbTab & "else{" & vbNewLine & _
                    vbTab & vbTab & vbTab & "//The player has pressed """ & TextBox42.Text & """." & vbNewLine & _
                    vbTab & vbTab & "}" & vbNewLine
                Else
                    TextBox37.Text += vbTab & vbTab & "//The player has pressed """ & TextBox41.Text & """(because it's de only avaliable button.)"
                End If
        End Select
        TextBox37.Text += vbTab & "}" & vbNewLine & "    return 0;" & vbNewLine & "}"
    End Sub

#End Region

#Region "Extra"

    Private Sub TabPage4_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles TabPage4.Leave
        TextBox38.Clear()
        TextBox39.Clear()
        Select Case Config.Idioma
            Case Lang.English
                TextBox40.Text = vbNewLine & vbNewLine & "PRESS ENTER TO CREATE A JUMP LINE ON THE TEXT OR" & vbNewLine & "TAB TO CREATE A TABULATION"
            Case Lang.Español
                TextBox40.Text = vbNewLine & vbNewLine & "PRESIONA ENTER PARA CREAR UN SALTO DE LINEA EN EL TEXTO O" & vbNewLine & "TAB PARA CREAR UNA TABULACIÓN"
            Case Lang.Portugues
                TextBox40.Text = vbNewLine & vbNewLine & "PRESSIONE ENTER PARA CRIAR UM SALTO DE LINHA NO TEXTO OU" & vbNewLine & "TAB PARA CRIAR UMA TABULAÇÃO"
            Case Else
                TextBox40.Text = vbNewLine & vbNewLine & "DRÜCKE ENTER, UM EINE LEERZEILE ZU ERSTELLEN, ODER" & vbNewLine & "TAB, UM EINE EINRÜCKUNG ZU ERSTELLEN"
        End Select
        TextBox40.ForeColor = Color.Gray
        TextBox41.Clear()
        TextBox42.Clear()
        TextBox43.Text = "playerid"
    End Sub

    Private Sub Button22_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button22.Click
        Select Case Config.Idioma
            Case Lang.English
                If TextBox40.Text.IndexOf("PRESS ENTER TO CREATE A JUMP LINE ON THE TEXT") <> -1 Then
                    TextBox40.Clear()
                    TextBox40.ForeColor = Color.Black
                End If
            Case Lang.Español
                If TextBox40.Text.IndexOf("PRESIONA ENTER PARA CREAR UN SALTO DE LINEA EN EL TEXTO") <> -1 Then
                    TextBox40.Clear()
                    TextBox40.ForeColor = Color.Black
                End If
            Case Lang.Portugues
                If TextBox40.Text.IndexOf("PRESSIONE ENTER PARA CRIAR UM SALTO DE LINHA NO TEXTO") <> -1 Then
                    TextBox40.Clear()
                    TextBox40.ForeColor = Color.Black
                End If
            Case Else
                If TextBox40.Text.IndexOf("DRÜCKE ENTER, UM EINE LEERZEILE ZU ERSTELLEN, ODER") <> -1 Then
                    TextBox40.Clear()
                    TextBox40.ForeColor = Color.Black
                End If
        End Select
        gSender = CC.Dialog
        eColor.TrackBar4.Enabled = False
        eColor.TextBox4.Enabled = False
        eColor.Show()
        eColor.Focus()
    End Sub

#End Region

#End Region

#Region "Color Picker"

#Region "Texts Restrictions"

    Private Sub TextBox44_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox44.KeyPress
        If Not IsNumeric(e.KeyChar) And Not Char.IsControl(e.KeyChar) And Asc(e.KeyChar) <> 8 Then e.Handled = True
    End Sub

    Private Sub TextBox45_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox45.KeyPress
        If Not IsNumeric(e.KeyChar) And Not Char.IsControl(e.KeyChar) And Asc(e.KeyChar) <> 8 Then e.Handled = True
    End Sub

    Private Sub TextBox46_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox46.KeyPress
        If Not IsNumeric(e.KeyChar) And Not Char.IsControl(e.KeyChar) And Asc(e.KeyChar) <> 8 Then e.Handled = True
    End Sub

    Private Sub TextBox47_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox47.TextAlignChanged
        If Not IsNumeric(e.KeyChar) And Not Char.IsControl(e.KeyChar) And Asc(e.KeyChar) <> 8 Then e.Handled = True
    End Sub

    Private Sub TextBox82_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox82.KeyPress
        If Not IsNumeric(e.KeyChar) And Not Char.IsControl(e.KeyChar) And e.KeyChar <> "," And Asc(e.KeyChar) <> 8 Then e.Handled = True
        If e.KeyChar = "," And TextBox82.Text.IndexOf(",") >= 0 Then e.Handled = True
    End Sub

    Private Sub TextBox83_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox83.KeyPress
        If Not IsNumeric(e.KeyChar) And Not Char.IsControl(e.KeyChar) And e.KeyChar <> "," And Asc(e.KeyChar) <> 8 Then e.Handled = True
        If e.KeyChar = "," And TextBox83.Text.IndexOf(",") >= 0 Then e.Handled = True
    End Sub

    Private Sub TextBox84_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox84.KeyPress
        If Not IsNumeric(e.KeyChar) And Not Char.IsControl(e.KeyChar) And e.KeyChar <> "," And Asc(e.KeyChar) <> 8 Then e.Handled = True
        If e.KeyChar = "," And TextBox84.Text.IndexOf(",") >= 0 Then e.Handled = True
    End Sub

    Private Sub TextBox85_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox85.KeyPress
        If Not IsNumeric(e.KeyChar) And Not Char.IsControl(e.KeyChar) And e.KeyChar <> "," And Asc(e.KeyChar) <> 8 Then e.Handled = True
        If e.KeyChar = "," And TextBox85.Text.IndexOf(",") >= 0 Then e.Handled = True
    End Sub

    Private Sub TextBox16_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox16.KeyPress
        If Not IsNumeric(e.KeyChar) And Not Char.IsControl(e.KeyChar) And Asc(e.KeyChar) <> 8 Then e.Handled = True
    End Sub

    Private Sub TextBox86_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox86.KeyPress
        If Not IsNumeric(e.KeyChar) And Not Char.IsControl(e.KeyChar) And e.KeyChar <> "," And Asc(e.KeyChar) <> 8 Then e.Handled = True
        If e.KeyChar = "," And TextBox86.Text.IndexOf(",") >= 0 Then e.Handled = True
    End Sub

    Private Sub TextBox87_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox87.KeyPress
        If Not IsNumeric(e.KeyChar) And Not Char.IsControl(e.KeyChar) And e.KeyChar <> "," And Asc(e.KeyChar) <> 8 Then e.Handled = True
        If e.KeyChar = "," And TextBox87.Text.IndexOf(",") >= 0 Then e.Handled = True
    End Sub

#End Region

#Region "Color"

#Region "RGB"

#Region "Tracks"

    Private Sub TrackBar1_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBar1.Scroll
        If ColorSender = 0 Then
            Dim nCMYK As CMYK = RGB2CMYK(TrackBar1.Value, TrackBar2.Value, TrackBar3.Value)
            Dim nHSL As HSL = RGB2HSL(TrackBar1.Value, TrackBar2.Value, TrackBar3.Value)
            If CheckBox6.Checked = False Then
                TextBox49.TextAlign = HorizontalAlignment.Center
                TextBox49.Text = cColor(TrackBar4.Value, TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel5.BackColor)
            Else
                If TextBox48.Text.Length > 0 Then
                    TextBox49.TextAlign = HorizontalAlignment.Left
                    TextBox49.Text = "#define " & TextBox48.Text & " " & cColor(TrackBar4.Value, TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel5.BackColor)
                Else
                    CheckBox6.Checked = False
                    TextBox49.TextAlign = HorizontalAlignment.Center
                    TextBox49.Text = cColor(TrackBar4.Value, TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel5.BackColor)
                End If
            End If
            TextBox44.Text = TrackBar1.Value
            TrackBar5.Value = nCMYK.Cyan * 1000
            TextBox82.Text = nCMYK.Cyan
            TrackBar6.Value = nCMYK.Magenta * 1000
            TextBox83.Text = nCMYK.Magenta
            TrackBar7.Value = nCMYK.Yellow * 1000
            TextBox84.Text = nCMYK.Yellow
            TrackBar8.Value = nCMYK.Black * 1000
            TextBox85.Text = nCMYK.Black
            TrackBar9.Value = nHSL.Hue
            TextBox16.Text = nHSL.Hue
            TrackBar10.Value = nHSL.Saturation * 1000
            TextBox86.Text = nHSL.Saturation
            TrackBar11.Value = nHSL.Luminance * 1000
            TextBox87.Text = nHSL.Luminance
        End If
    End Sub

    Private Sub TrackBar2_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBar2.Scroll
        If ColorSender = 0 Then
            Dim nCMYK As CMYK = RGB2CMYK(TrackBar1.Value, TrackBar2.Value, TrackBar3.Value)
            Dim nHSL As HSL = RGB2HSL(TrackBar1.Value, TrackBar2.Value, TrackBar3.Value)
            If CheckBox6.Checked = False Then
                TextBox49.TextAlign = HorizontalAlignment.Center
                TextBox49.Text = cColor(TrackBar4.Value, TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel5.BackColor)
            Else
                If TextBox48.Text.Length > 0 Then
                    TextBox49.TextAlign = HorizontalAlignment.Left
                    TextBox49.Text = "#define " & TextBox48.Text & " " & cColor(TrackBar4.Value, TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel5.BackColor)
                Else
                    CheckBox6.Checked = False
                    TextBox49.TextAlign = HorizontalAlignment.Center
                    TextBox49.Text = cColor(TrackBar4.Value, TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel5.BackColor)
                End If
            End If
            TextBox45.Text = TrackBar2.Value
            TrackBar5.Value = nCMYK.Cyan * 1000
            TextBox82.Text = nCMYK.Cyan
            TrackBar6.Value = nCMYK.Magenta * 1000
            TextBox83.Text = nCMYK.Magenta
            TrackBar7.Value = nCMYK.Yellow * 1000
            TextBox84.Text = nCMYK.Yellow
            TrackBar8.Value = nCMYK.Black * 1000
            TextBox85.Text = nCMYK.Black
            TrackBar9.Value = nHSL.Hue
            TextBox16.Text = nHSL.Hue
            TrackBar10.Value = nHSL.Saturation * 1000
            TextBox86.Text = nHSL.Saturation
            TrackBar11.Value = nHSL.Luminance * 1000
            TextBox87.Text = nHSL.Luminance
        End If
    End Sub

    Private Sub TrackBar3_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBar3.Scroll
        If ColorSender = 0 Then
            Dim nCMYK As CMYK = RGB2CMYK(TrackBar1.Value, TrackBar2.Value, TrackBar3.Value)
            Dim nHSL As HSL = RGB2HSL(TrackBar1.Value, TrackBar2.Value, TrackBar3.Value)
            If CheckBox6.Checked = False Then
                TextBox49.TextAlign = HorizontalAlignment.Center
                TextBox49.Text = cColor(TrackBar4.Value, TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel5.BackColor)
            Else
                If TextBox48.Text.Length > 0 Then
                    TextBox49.TextAlign = HorizontalAlignment.Left
                    TextBox49.Text = "#define " & TextBox48.Text & " " & cColor(TrackBar4.Value, TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel5.BackColor)
                Else
                    CheckBox6.Checked = False
                    TextBox49.TextAlign = HorizontalAlignment.Center
                    TextBox49.Text = cColor(TrackBar4.Value, TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel5.BackColor)
                End If
            End If
            TextBox46.Text = TrackBar3.Value
            TrackBar5.Value = nCMYK.Cyan * 1000
            TextBox82.Text = nCMYK.Cyan
            TrackBar6.Value = nCMYK.Magenta * 1000
            TextBox83.Text = nCMYK.Magenta
            TrackBar7.Value = nCMYK.Yellow * 1000
            TextBox84.Text = nCMYK.Yellow
            TrackBar8.Value = nCMYK.Black * 1000
            TextBox85.Text = nCMYK.Black
            TrackBar9.Value = nHSL.Hue
            TextBox16.Text = nHSL.Hue
            TrackBar10.Value = nHSL.Saturation * 1000
            TextBox86.Text = nHSL.Saturation
            TrackBar11.Value = nHSL.Luminance * 1000
            TextBox87.Text = nHSL.Luminance
        End If
    End Sub

#End Region

#Region "Box"

    Private Sub TextBox44_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox44.TextChanged
        If ColorSender = 0 Then
            TextBox44.Text = Regex.Replace(TextBox44.Text, BadChars, "")
            If Val(TextBox44.Text) <> TrackBar1.Value Then
                If Val(TextBox44.Text) > 255 Then TextBox44.Text = 255
                If Val(TextBox44.Text) < 0 Then TextBox44.Text = 0
                TrackBar1.Value = Val(TextBox44.Text)
                If CheckBox6.Checked = False Then
                    TextBox49.TextAlign = HorizontalAlignment.Center
                    TextBox49.Text = cColor(TrackBar4.Value, TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel5.BackColor)
                Else
                    If TextBox48.Text.Length > 0 Then
                        TextBox49.TextAlign = HorizontalAlignment.Left
                        TextBox49.Text = "#define " & TextBox48.Text & " " & cColor(TrackBar4.Value, TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel5.BackColor)
                    Else
                        CheckBox6.Checked = False
                        TextBox49.TextAlign = HorizontalAlignment.Center
                        TextBox49.Text = cColor(TrackBar4.Value, TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel5.BackColor)
                    End If
                End If
                Dim nCMYK As CMYK = RGB2CMYK(TrackBar1.Value, TrackBar2.Value, TrackBar3.Value)
                Dim nHSL As HSL = RGB2HSL(TrackBar1.Value, TrackBar2.Value, TrackBar3.Value)
                TrackBar5.Value = nCMYK.Cyan * 1000
                TextBox82.Text = nCMYK.Cyan
                TrackBar6.Value = nCMYK.Magenta * 1000
                TextBox83.Text = nCMYK.Magenta
                TrackBar7.Value = nCMYK.Yellow * 1000
                TextBox84.Text = nCMYK.Yellow
                TrackBar8.Value = nCMYK.Black * 1000
                TextBox85.Text = nCMYK.Black
                TrackBar9.Value = nHSL.Hue
                TextBox16.Text = nHSL.Hue
                TrackBar10.Value = nHSL.Saturation * 1000
                TextBox86.Text = nHSL.Saturation
                TrackBar11.Value = nHSL.Luminance * 1000
                TextBox87.Text = nHSL.Luminance
            End If
        End If
    End Sub

    Private Sub TextBox45_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox45.TextChanged
        If ColorSender = 0 Then
            TextBox45.Text = Regex.Replace(TextBox45.Text, BadChars, "")
            If Val(TextBox45.Text) <> TrackBar2.Value Then
                If Val(TextBox45.Text) > 255 Then TextBox45.Text = 255
                If Val(TextBox45.Text) < 0 Then TextBox45.Text = 0
                TrackBar2.Value = Val(TextBox45.Text)
                If CheckBox6.Checked = False Then
                    TextBox49.TextAlign = HorizontalAlignment.Center
                    TextBox49.Text = cColor(TrackBar4.Value, TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel5.BackColor)
                Else
                    If TextBox48.Text.Length > 0 Then
                        TextBox49.TextAlign = HorizontalAlignment.Left
                        TextBox49.Text = "#define " & TextBox48.Text & " " & cColor(TrackBar4.Value, TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel5.BackColor)
                    Else
                        CheckBox6.Checked = False
                        TextBox49.TextAlign = HorizontalAlignment.Center
                        TextBox49.Text = cColor(TrackBar4.Value, TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel5.BackColor)
                    End If
                End If
                Dim nCMYK As CMYK = RGB2CMYK(TrackBar1.Value, TrackBar2.Value, TrackBar3.Value)
                Dim nHSL As HSL = RGB2HSL(TrackBar1.Value, TrackBar2.Value, TrackBar3.Value)
                TrackBar5.Value = nCMYK.Cyan * 1000
                TextBox82.Text = nCMYK.Cyan
                TrackBar6.Value = nCMYK.Magenta * 1000
                TextBox83.Text = nCMYK.Magenta
                TrackBar7.Value = nCMYK.Yellow * 1000
                TextBox84.Text = nCMYK.Yellow
                TrackBar8.Value = nCMYK.Black * 1000
                TextBox85.Text = nCMYK.Black
                TrackBar9.Value = nHSL.Hue
                TextBox16.Text = nHSL.Hue
                TrackBar10.Value = nHSL.Saturation * 1000
                TextBox86.Text = nHSL.Saturation
                TrackBar11.Value = nHSL.Luminance * 1000
                TextBox87.Text = nHSL.Luminance
            End If
        End If
    End Sub

    Private Sub TextBox46_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox46.TextChanged
        If ColorSender = 0 Then
            TextBox46.Text = Regex.Replace(TextBox46.Text, BadChars, "")
            If Val(TextBox46.Text) <> TrackBar3.Value Then
                If Val(TextBox46.Text) > 255 Then TextBox46.Text = 255
                If Val(TextBox46.Text) < 0 Then TextBox46.Text = 0
                TrackBar3.Value = Val(TextBox46.Text)
                If CheckBox6.Checked = False Then
                    TextBox49.TextAlign = HorizontalAlignment.Center
                    TextBox49.Text = cColor(TrackBar4.Value, TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel5.BackColor)
                Else
                    If TextBox48.Text.Length > 0 Then
                        TextBox49.TextAlign = HorizontalAlignment.Left
                        TextBox49.Text = "#define " & TextBox48.Text & " " & cColor(TrackBar4.Value, TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel5.BackColor)
                    Else
                        CheckBox6.Checked = False
                        TextBox49.TextAlign = HorizontalAlignment.Center
                        TextBox49.Text = cColor(TrackBar4.Value, TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel5.BackColor)
                    End If
                End If
                Dim nCMYK As CMYK = RGB2CMYK(TrackBar1.Value, TrackBar2.Value, TrackBar3.Value)
                Dim nHSL As HSL = RGB2HSL(TrackBar1.Value, TrackBar2.Value, TrackBar3.Value)
                TrackBar5.Value = nCMYK.Cyan * 1000
                TextBox82.Text = nCMYK.Cyan
                TrackBar6.Value = nCMYK.Magenta * 1000
                TextBox83.Text = nCMYK.Magenta
                TrackBar7.Value = nCMYK.Yellow * 1000
                TextBox84.Text = nCMYK.Yellow
                TrackBar8.Value = nCMYK.Black * 1000
                TextBox85.Text = nCMYK.Black
                TrackBar9.Value = nHSL.Hue
                TextBox16.Text = nHSL.Hue
                TrackBar10.Value = nHSL.Saturation * 1000
                TextBox86.Text = nHSL.Saturation
                TrackBar11.Value = nHSL.Luminance * 1000
                TextBox87.Text = nHSL.Luminance
            End If
        End If
    End Sub

#End Region

#End Region

#Region "CMYK"

#Region "Tracks"

    Private Sub TrackBar5_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBar5.Scroll
        TextBox82.Text = TrackBar5.Value / 1000
        If ColorSender = 1 Then
            Dim nRGB As RGB = CMYK2RGB(TextBox82.Text, TextBox83.Text, TextBox84.Text, TextBox85.Text)
            Dim nHSL As HSL = RGB2HSL(nRGB.Red, nRGB.Green, nRGB.Blue)
            Panel5.BackColor = Color.FromArgb(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue)
            If CheckBox6.Checked = False Then
                TextBox49.TextAlign = HorizontalAlignment.Center
                TextBox49.Text = cColor(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue)
            Else
                If TextBox48.Text.Length > 0 Then
                    TextBox49.TextAlign = HorizontalAlignment.Left
                    TextBox49.Text = "#define " & TextBox48.Text & " " & cColor(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue)
                Else
                    CheckBox6.Checked = False
                    TextBox49.TextAlign = HorizontalAlignment.Center
                    TextBox49.Text = cColor(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue)
                End If
            End If
            TrackBar1.Value = nRGB.Red
            TextBox44.Text = nRGB.Red
            TrackBar2.Value = nRGB.Green
            TextBox45.Text = nRGB.Green
            TrackBar3.Value = nRGB.Blue
            TextBox46.Text = nRGB.Blue
            TrackBar9.Value = nHSL.Hue
            TextBox16.Text = nHSL.Hue
            TrackBar10.Value = nHSL.Saturation * 1000
            TextBox86.Text = nHSL.Saturation
            TrackBar11.Value = nHSL.Luminance * 1000
            TextBox87.Text = nHSL.Luminance
        End If
    End Sub

    Private Sub TrackBar6_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBar6.Scroll
        TextBox83.Text = TrackBar6.Value / 1000
        If ColorSender = 1 Then
            Dim nRGB As RGB = CMYK2RGB(TextBox82.Text, TextBox83.Text, TextBox84.Text, TextBox85.Text)
            Dim nHSL As HSL = RGB2HSL(nRGB.Red, nRGB.Green, nRGB.Blue)
            Panel5.BackColor = Color.FromArgb(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue)
            If CheckBox6.Checked = False Then
                TextBox49.TextAlign = HorizontalAlignment.Center
                TextBox49.Text = cColor(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue)
            Else
                If TextBox48.Text.Length > 0 Then
                    TextBox49.TextAlign = HorizontalAlignment.Left
                    TextBox49.Text = "#define " & TextBox48.Text & " " & cColor(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue)
                Else
                    CheckBox6.Checked = False
                    TextBox49.TextAlign = HorizontalAlignment.Center
                    TextBox49.Text = cColor(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue)
                End If
            End If
            TrackBar1.Value = nRGB.Red
            TextBox44.Text = nRGB.Red
            TrackBar2.Value = nRGB.Green
            TextBox45.Text = nRGB.Green
            TrackBar3.Value = nRGB.Blue
            TextBox46.Text = nRGB.Blue
            TrackBar9.Value = nHSL.Hue
            TextBox16.Text = nHSL.Hue
            TrackBar10.Value = nHSL.Saturation * 1000
            TextBox86.Text = nHSL.Saturation
            TrackBar11.Value = nHSL.Luminance * 1000
            TextBox87.Text = nHSL.Luminance
        End If
    End Sub

    Private Sub TrackBar7_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBar7.Scroll
        TextBox84.Text = TrackBar7.Value / 1000
        If ColorSender = 1 Then
            Dim nRGB As RGB = CMYK2RGB(TextBox82.Text, TextBox83.Text, TextBox84.Text, TextBox85.Text)
            Dim nHSL As HSL = RGB2HSL(nRGB.Red, nRGB.Green, nRGB.Blue)
            Panel5.BackColor = Color.FromArgb(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue)
            If CheckBox6.Checked = False Then
                TextBox49.TextAlign = HorizontalAlignment.Center
                TextBox49.Text = cColor(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue)
            Else
                If TextBox48.Text.Length > 0 Then
                    TextBox49.TextAlign = HorizontalAlignment.Left
                    TextBox49.Text = "#define " & TextBox48.Text & " " & cColor(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue)
                Else
                    CheckBox6.Checked = False
                    TextBox49.TextAlign = HorizontalAlignment.Center
                    TextBox49.Text = cColor(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue)
                End If
            End If
            TrackBar1.Value = nRGB.Red
            TextBox44.Text = nRGB.Red
            TrackBar2.Value = nRGB.Green
            TextBox45.Text = nRGB.Green
            TrackBar3.Value = nRGB.Blue
            TextBox46.Text = nRGB.Blue
            TrackBar9.Value = nHSL.Hue
            TextBox16.Text = nHSL.Hue
            TrackBar10.Value = nHSL.Saturation * 1000
            TextBox86.Text = nHSL.Saturation
            TrackBar11.Value = nHSL.Luminance * 1000
            TextBox87.Text = nHSL.Luminance
        End If
    End Sub

    Private Sub TrackBar8_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBar8.Scroll
        TextBox85.Text = TrackBar8.Value / 1000
        If ColorSender = 1 Then
            Dim nRGB As RGB = CMYK2RGB(TextBox82.Text, TextBox83.Text, TextBox84.Text, TextBox85.Text)
            Dim nHSL As HSL = RGB2HSL(nRGB.Red, nRGB.Green, nRGB.Blue)
            Panel5.BackColor = Color.FromArgb(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue)
            If CheckBox6.Checked = False Then
                TextBox49.TextAlign = HorizontalAlignment.Center
                TextBox49.Text = cColor(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue)
            Else
                If TextBox48.Text.Length > 0 Then
                    TextBox49.TextAlign = HorizontalAlignment.Left
                    TextBox49.Text = "#define " & TextBox48.Text & " " & cColor(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue)
                Else
                    CheckBox6.Checked = False
                    TextBox49.TextAlign = HorizontalAlignment.Center
                    TextBox49.Text = cColor(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue)
                End If
            End If
            TrackBar1.Value = nRGB.Red
            TextBox44.Text = nRGB.Red
            TrackBar2.Value = nRGB.Green
            TextBox45.Text = nRGB.Green
            TrackBar3.Value = nRGB.Blue
            TextBox46.Text = nRGB.Blue
            TrackBar9.Value = nHSL.Hue
            TextBox16.Text = nHSL.Hue
            TrackBar10.Value = nHSL.Saturation * 1000
            TextBox86.Text = nHSL.Saturation
            TrackBar11.Value = nHSL.Luminance * 1000
            TextBox87.Text = nHSL.Luminance
        End If
    End Sub

#End Region

#Region "Box"

    Private Sub TextBox82_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox82.TextChanged
        If ColorSender = 1 Then
            TextBox82.Text = Regex.Replace(TextBox82.Text, BadChars2, "")
            If Round(TextBox82.Text * 1000) <> TrackBar5.Value Then
                TextBox82.Text = CSng(TextBox82.Text)
                If TextBox82.Text > 1 Then TextBox82.Text = 1
                If TextBox82.Text < 0 Then TextBox82.Text = 0
                TrackBar5.Value = TextBox82.Text * 1000
                Dim nRGB As RGB = CMYK2RGB(TrackBar5.Value / 1000, TrackBar6.Value / 1000, TrackBar7.Value / 1000, TrackBar8.Value / 1000)
                Dim nHSL As HSL = RGB2HSL(nRGB.Red, nRGB.Green, nRGB.Blue)
                Panel5.BackColor = Color.FromArgb(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue)
                If CheckBox6.Checked = False Then
                    TextBox49.TextAlign = HorizontalAlignment.Center
                    TextBox49.Text = cColor(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue)
                Else
                    If TextBox48.Text.Length > 0 Then
                        TextBox49.TextAlign = HorizontalAlignment.Left
                        TextBox49.Text = "#define " & TextBox48.Text & " " & cColor(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue)
                    Else
                        CheckBox6.Checked = False
                        TextBox49.TextAlign = HorizontalAlignment.Center
                        TextBox49.Text = cColor(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue)
                    End If
                End If
                TrackBar1.Value = nRGB.Red
                TextBox44.Text = nRGB.Red
                TrackBar2.Value = nRGB.Green
                TextBox45.Text = nRGB.Green
                TrackBar3.Value = nRGB.Blue
                TextBox46.Text = nRGB.Blue
                TrackBar9.Value = nHSL.Hue
                TextBox16.Text = nHSL.Hue
                TrackBar10.Value = nHSL.Saturation * 1000
                TextBox86.Text = nHSL.Saturation
                TrackBar11.Value = nHSL.Luminance * 1000
                TextBox87.Text = nHSL.Luminance
            End If
        End If
    End Sub

    Private Sub TextBox83_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox83.TextChanged
        If ColorSender = 1 Then
            TextBox83.Text = Regex.Replace(TextBox83.Text, BadChars2, "")
            If Round(TextBox83.Text * 1000) <> TrackBar6.Value Then
                TextBox83.Text = CSng(TextBox83.Text)
                If TextBox83.Text > 1 Then TextBox83.Text = 1
                If TextBox83.Text < 0 Then TextBox83.Text = 0
                TrackBar6.Value = TextBox83.Text * 1000
                Dim nRGB As RGB = CMYK2RGB(TrackBar5.Value / 1000, TrackBar6.Value / 1000, TrackBar7.Value / 1000, TrackBar8.Value / 1000)
                Dim nHSL As HSL = RGB2HSL(nRGB.Red, nRGB.Green, nRGB.Blue)
                Panel5.BackColor = Color.FromArgb(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue)
                If CheckBox6.Checked = False Then
                    TextBox49.TextAlign = HorizontalAlignment.Center
                    TextBox49.Text = cColor(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue)
                Else
                    If TextBox48.Text.Length > 0 Then
                        TextBox49.TextAlign = HorizontalAlignment.Left
                        TextBox49.Text = "#define " & TextBox48.Text & " " & cColor(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue)
                    Else
                        CheckBox6.Checked = False
                        TextBox49.TextAlign = HorizontalAlignment.Center
                        TextBox49.Text = cColor(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue)
                    End If
                End If
                TrackBar1.Value = nRGB.Red
                TextBox44.Text = nRGB.Red
                TrackBar2.Value = nRGB.Green
                TextBox45.Text = nRGB.Green
                TrackBar3.Value = nRGB.Blue
                TextBox46.Text = nRGB.Blue
                TrackBar9.Value = nHSL.Hue
                TextBox16.Text = nHSL.Hue
                TrackBar10.Value = nHSL.Saturation * 1000
                TextBox86.Text = nHSL.Saturation
                TrackBar11.Value = nHSL.Luminance * 1000
                TextBox87.Text = nHSL.Luminance
            End If
        End If
    End Sub

    Private Sub TextBox84_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox84.TextChanged
        If ColorSender = 1 Then
            TextBox84.Text = Regex.Replace(TextBox84.Text, BadChars2, "")
            If Round(TextBox84.Text * 1000) <> TrackBar7.Value Then
                TextBox84.Text = CSng(TextBox84.Text)
                If TextBox84.Text > 1 Then TextBox84.Text = 1
                If TextBox84.Text < 0 Then TextBox84.Text = 0
                TrackBar7.Value = TextBox84.Text * 1000
                Dim nRGB As RGB = CMYK2RGB(TrackBar5.Value / 1000, TrackBar6.Value / 1000, TrackBar7.Value / 1000, TrackBar8.Value / 1000)
                Dim nHSL As HSL = RGB2HSL(nRGB.Red, nRGB.Green, nRGB.Blue)
                Panel5.BackColor = Color.FromArgb(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue)
                If CheckBox6.Checked = False Then
                    TextBox49.TextAlign = HorizontalAlignment.Center
                    TextBox49.Text = cColor(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue)
                Else
                    If TextBox48.Text.Length > 0 Then
                        TextBox49.TextAlign = HorizontalAlignment.Left
                        TextBox49.Text = "#define " & TextBox48.Text & " " & cColor(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue)
                    Else
                        CheckBox6.Checked = False
                        TextBox49.TextAlign = HorizontalAlignment.Center
                        TextBox49.Text = cColor(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue)
                    End If
                End If
                TrackBar1.Value = nRGB.Red
                TextBox44.Text = nRGB.Red
                TrackBar2.Value = nRGB.Green
                TextBox45.Text = nRGB.Green
                TrackBar3.Value = nRGB.Blue
                TextBox46.Text = nRGB.Blue
                TrackBar9.Value = nHSL.Hue
                TextBox16.Text = nHSL.Hue
                TrackBar10.Value = nHSL.Saturation * 1000
                TextBox86.Text = nHSL.Saturation
                TrackBar11.Value = nHSL.Luminance * 1000
                TextBox87.Text = nHSL.Luminance
            End If
        End If
    End Sub

    Private Sub TextBox85_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox85.TextChanged
        If ColorSender = 1 Then
            TextBox85.Text = Regex.Replace(TextBox85.Text, BadChars2, "")
            If Round(TextBox85.Text * 1000) <> TrackBar8.Value Then
                TextBox85.Text = CSng(TextBox85.Text)
                If TextBox85.Text > 1 Then TextBox85.Text = 1
                If TextBox85.Text < 0 Then TextBox85.Text = 0
                TrackBar8.Value = TextBox85.Text * 1000
                Dim nRGB As RGB = CMYK2RGB(TrackBar5.Value / 1000, TrackBar6.Value / 1000, TrackBar7.Value / 1000, TrackBar8.Value / 1000)
                Dim nHSL As HSL = RGB2HSL(nRGB.Red, nRGB.Green, nRGB.Blue)
                Panel5.BackColor = Color.FromArgb(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue)
                If CheckBox6.Checked = False Then
                    TextBox49.TextAlign = HorizontalAlignment.Center
                    TextBox49.Text = cColor(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue)
                Else
                    If TextBox48.Text.Length > 0 Then
                        TextBox49.TextAlign = HorizontalAlignment.Left
                        TextBox49.Text = "#define " & TextBox48.Text & " " & cColor(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue)
                    Else
                        CheckBox6.Checked = False
                        TextBox49.TextAlign = HorizontalAlignment.Center
                        TextBox49.Text = cColor(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue)
                    End If
                End If
                TrackBar1.Value = nRGB.Red
                TextBox44.Text = nRGB.Red
                TrackBar2.Value = nRGB.Green
                TextBox45.Text = nRGB.Green
                TrackBar3.Value = nRGB.Blue
                TextBox46.Text = nRGB.Blue
                TrackBar9.Value = nHSL.Hue
                TextBox16.Text = nHSL.Hue
                TrackBar10.Value = nHSL.Saturation * 1000
                TextBox86.Text = nHSL.Saturation
                TrackBar11.Value = nHSL.Luminance * 1000
                TextBox87.Text = nHSL.Luminance
            End If
        End If
    End Sub

#End Region

#End Region

#Region "HSL"

#Region "Tracks"

    Private Sub TrackBar9_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBar9.Scroll
        If ColorSender = 2 Then
            TextBox16.Text = TrackBar9.Value
            Dim nRGB As RGB = HSL2RGB(TrackBar9.Value, TextBox86.Text, TextBox87.Text)
            Dim nCMYK As CMYK = RGB2CMYK(nRGB.Red, nRGB.Green, nRGB.Blue)
            Panel5.BackColor = Color.FromArgb(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue)
            If CheckBox6.Checked = False Then
                TextBox49.TextAlign = HorizontalAlignment.Center
                TextBox49.Text = cColor(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue)
            Else
                If TextBox48.Text.Length > 0 Then
                    TextBox49.TextAlign = HorizontalAlignment.Left
                    TextBox49.Text = "#define " & TextBox48.Text & " " & cColor(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue)
                Else
                    CheckBox6.Checked = False
                    TextBox49.TextAlign = HorizontalAlignment.Center
                    TextBox49.Text = cColor(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue)
                End If
            End If
            TrackBar1.Value = nRGB.Red
            TextBox44.Text = nRGB.Red
            TrackBar2.Value = nRGB.Green
            TextBox45.Text = nRGB.Green
            TrackBar3.Value = nRGB.Blue
            TextBox46.Text = nRGB.Blue
            TextBox44.Text = TrackBar1.Value
            TrackBar5.Value = nCMYK.Cyan * 1000
            TextBox82.Text = nCMYK.Cyan
            TrackBar6.Value = nCMYK.Magenta * 1000
            TextBox83.Text = nCMYK.Magenta
            TrackBar7.Value = nCMYK.Yellow * 1000
            TextBox84.Text = nCMYK.Yellow
            TrackBar8.Value = nCMYK.Black * 1000
            TextBox85.Text = nCMYK.Black
        End If
    End Sub

    Private Sub TrackBar10_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBar10.Scroll
        TextBox86.Text = TrackBar10.Value / 1000
        If ColorSender = 2 Then
            Dim nRGB As RGB = HSL2RGB(TrackBar9.Value, TextBox86.Text, TextBox87.Text)
            Dim nCMYK As CMYK = RGB2CMYK(nRGB.Red, nRGB.Green, nRGB.Blue)
            Panel5.BackColor = Color.FromArgb(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue)
            If CheckBox6.Checked = False Then
                TextBox49.TextAlign = HorizontalAlignment.Center
                TextBox49.Text = cColor(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue)
            Else
                If TextBox48.Text.Length > 0 Then
                    TextBox49.TextAlign = HorizontalAlignment.Left
                    TextBox49.Text = "#define " & TextBox48.Text & " " & cColor(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue)
                Else
                    CheckBox6.Checked = False
                    TextBox49.TextAlign = HorizontalAlignment.Center
                    TextBox49.Text = cColor(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue)
                End If
            End If
            TrackBar1.Value = nRGB.Red
            TextBox44.Text = nRGB.Red
            TrackBar2.Value = nRGB.Green
            TextBox45.Text = nRGB.Green
            TrackBar3.Value = nRGB.Blue
            TextBox46.Text = nRGB.Blue
            TextBox44.Text = TrackBar1.Value
            TrackBar5.Value = nCMYK.Cyan * 1000
            TextBox82.Text = nCMYK.Cyan
            TrackBar6.Value = nCMYK.Magenta * 1000
            TextBox83.Text = nCMYK.Magenta
            TrackBar7.Value = nCMYK.Yellow * 1000
            TextBox84.Text = nCMYK.Yellow
            TrackBar8.Value = nCMYK.Black * 1000
            TextBox85.Text = nCMYK.Black
        End If
    End Sub

    Private Sub TrackBar11_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBar11.Scroll
        TextBox87.Text = TrackBar11.Value / 1000
        If ColorSender = 2 Then
            Dim nRGB As RGB = HSL2RGB(TrackBar9.Value, TextBox86.Text, TextBox87.Text)
            Dim nCMYK As CMYK = RGB2CMYK(nRGB.Red, nRGB.Green, nRGB.Blue)
            Panel5.BackColor = Color.FromArgb(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue)
            If CheckBox6.Checked = False Then
                TextBox49.TextAlign = HorizontalAlignment.Center
                TextBox49.Text = cColor(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue)
            Else
                If TextBox48.Text.Length > 0 Then
                    TextBox49.TextAlign = HorizontalAlignment.Left
                    TextBox49.Text = "#define " & TextBox48.Text & " " & cColor(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue)
                Else
                    CheckBox6.Checked = False
                    TextBox49.TextAlign = HorizontalAlignment.Center
                    TextBox49.Text = cColor(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue)
                End If
            End If
            TrackBar1.Value = nRGB.Red
            TextBox44.Text = nRGB.Red
            TrackBar2.Value = nRGB.Green
            TextBox45.Text = nRGB.Green
            TrackBar3.Value = nRGB.Blue
            TextBox46.Text = nRGB.Blue
            TextBox44.Text = TrackBar1.Value
            TrackBar5.Value = nCMYK.Cyan * 1000
            TextBox82.Text = nCMYK.Cyan
            TrackBar6.Value = nCMYK.Magenta * 1000
            TextBox83.Text = nCMYK.Magenta
            TrackBar7.Value = nCMYK.Yellow * 1000
            TextBox84.Text = nCMYK.Yellow
            TrackBar8.Value = nCMYK.Black * 1000
            TextBox85.Text = nCMYK.Black
        End If
    End Sub

#End Region

#Region "Box"

    Private Sub TextBox16_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox16.TextChanged
        If ColorSender = 2 Then
            TextBox16.Text = Regex.Replace(TextBox16.Text, BadChars2, "")
            If TextBox16.Text <> TrackBar9.Value Then
                If TextBox16.Text > 360 Then TextBox16.Text = 360
                If TextBox16.Text < 0 Then TextBox16.Text = 0
                TrackBar9.Value = TextBox16.Text
                Dim nRGB As RGB = HSL2RGB(TrackBar9.Value, TextBox86.Text, TextBox87.Text)
                Dim nCMYK As CMYK = RGB2CMYK(nRGB.Red, nRGB.Green, nRGB.Blue)
                Panel5.BackColor = Color.FromArgb(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue)
                If CheckBox6.Checked = False Then
                    TextBox49.TextAlign = HorizontalAlignment.Center
                    TextBox49.Text = cColor(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue)
                Else
                    If TextBox48.Text.Length > 0 Then
                        TextBox49.TextAlign = HorizontalAlignment.Left
                        TextBox49.Text = "#define " & TextBox48.Text & " " & cColor(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue)
                    Else
                        CheckBox6.Checked = False
                        TextBox49.TextAlign = HorizontalAlignment.Center
                        TextBox49.Text = cColor(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue)
                    End If
                End If
                TrackBar1.Value = nRGB.Red
                TextBox44.Text = nRGB.Red
                TrackBar2.Value = nRGB.Green
                TextBox45.Text = nRGB.Green
                TrackBar3.Value = nRGB.Blue
                TextBox46.Text = nRGB.Blue
                TextBox44.Text = TrackBar1.Value
                TrackBar5.Value = nCMYK.Cyan * 1000
                TextBox82.Text = nCMYK.Cyan
                TrackBar6.Value = nCMYK.Magenta * 1000
                TextBox83.Text = nCMYK.Magenta
                TrackBar7.Value = nCMYK.Yellow * 1000
                TextBox84.Text = nCMYK.Yellow
                TrackBar8.Value = nCMYK.Black * 1000
                TextBox85.Text = nCMYK.Black
            End If
        End If
    End Sub

    Private Sub TextBox86_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox86.TextChanged
        If ColorSender = 2 Then
            TextBox86.Text = Regex.Replace(TextBox86.Text, BadChars2, "")
            If Round(TextBox86.Text * 1000) <> TrackBar10.Value Then
                TextBox86.Text = CSng(TextBox86.Text)
                If TextBox86.Text > 1 Then TextBox86.Text = 1
                If TextBox86.Text < 0 Then TextBox86.Text = 0
                TrackBar10.Value = TextBox86.Text * 1000
                Dim nRGB As RGB = HSL2RGB(TrackBar9.Value, TextBox86.Text, TextBox87.Text)
                Dim nCMYK As CMYK = RGB2CMYK(nRGB.Red, nRGB.Green, nRGB.Blue)
                Panel5.BackColor = Color.FromArgb(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue)
                If CheckBox6.Checked = False Then
                    TextBox49.TextAlign = HorizontalAlignment.Center
                    TextBox49.Text = cColor(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue)
                Else
                    If TextBox48.Text.Length > 0 Then
                        TextBox49.TextAlign = HorizontalAlignment.Left
                        TextBox49.Text = "#define " & TextBox48.Text & " " & cColor(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue)
                    Else
                        CheckBox6.Checked = False
                        TextBox49.TextAlign = HorizontalAlignment.Center
                        TextBox49.Text = cColor(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue)
                    End If
                End If
                TrackBar1.Value = nRGB.Red
                TextBox44.Text = nRGB.Red
                TrackBar2.Value = nRGB.Green
                TextBox45.Text = nRGB.Green
                TrackBar3.Value = nRGB.Blue
                TextBox46.Text = nRGB.Blue
                TextBox44.Text = TrackBar1.Value
                TrackBar5.Value = nCMYK.Cyan * 1000
                TextBox82.Text = nCMYK.Cyan
                TrackBar6.Value = nCMYK.Magenta * 1000
                TextBox83.Text = nCMYK.Magenta
                TrackBar7.Value = nCMYK.Yellow * 1000
                TextBox84.Text = nCMYK.Yellow
                TrackBar8.Value = nCMYK.Black * 1000
                TextBox85.Text = nCMYK.Black
            End If
        End If
    End Sub

    Private Sub TextBox87_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox87.TextChanged
        If ColorSender = 2 Then
            TextBox87.Text = Regex.Replace(TextBox87.Text, BadChars2, "")
            If Round(TextBox87.Text * 1000) <> TrackBar11.Value Then
                TextBox87.Text = CSng(TextBox87.Text)
                If TextBox87.Text > 1 Then TextBox87.Text = 1
                If TextBox87.Text < 0 Then TextBox87.Text = 0
                TrackBar11.Value = TextBox87.Text * 1000
                Dim nRGB As RGB = HSL2RGB(TrackBar9.Value, TextBox86.Text, TextBox87.Text)
                Dim nCMYK As CMYK = RGB2CMYK(nRGB.Red, nRGB.Green, nRGB.Blue)
                Panel5.BackColor = Color.FromArgb(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue)
                If CheckBox6.Checked = False Then
                    TextBox49.TextAlign = HorizontalAlignment.Center
                    TextBox49.Text = cColor(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue)
                Else
                    If TextBox48.Text.Length > 0 Then
                        TextBox49.TextAlign = HorizontalAlignment.Left
                        TextBox49.Text = "#define " & TextBox48.Text & " " & cColor(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue)
                    Else
                        CheckBox6.Checked = False
                        TextBox49.TextAlign = HorizontalAlignment.Center
                        TextBox49.Text = cColor(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue)
                    End If
                End If
                TrackBar1.Value = nRGB.Red
                TextBox44.Text = nRGB.Red
                TrackBar2.Value = nRGB.Green
                TextBox45.Text = nRGB.Green
                TrackBar3.Value = nRGB.Blue
                TextBox46.Text = nRGB.Blue
                TextBox44.Text = TrackBar1.Value
                TrackBar5.Value = nCMYK.Cyan * 1000
                TextBox82.Text = nCMYK.Cyan
                TrackBar6.Value = nCMYK.Magenta * 1000
                TextBox83.Text = nCMYK.Magenta
                TrackBar7.Value = nCMYK.Yellow * 1000
                TextBox84.Text = nCMYK.Yellow
                TrackBar8.Value = nCMYK.Black * 1000
                TextBox85.Text = nCMYK.Black
            End If
        End If
    End Sub

#End Region

#End Region

#Region "Alpha"

#Region "Track"

    Private Sub TrackBar4_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBar4.Scroll
        If CheckBox6.Checked = False Then
            TextBox49.TextAlign = HorizontalAlignment.Center
            TextBox49.Text = cColor(TrackBar4.Value, TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel5.BackColor)
        Else
            If TextBox48.Text.Length > 0 Then
                TextBox49.TextAlign = HorizontalAlignment.Left
                TextBox49.Text = "#define " & TextBox48.Text & " " & cColor(TrackBar4.Value, TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel5.BackColor)
            Else
                CheckBox6.Checked = False
                TextBox49.TextAlign = HorizontalAlignment.Center
                TextBox49.Text = cColor(TrackBar4.Value, TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel5.BackColor)
            End If
        End If
        TextBox47.Text = TrackBar4.Value
    End Sub

#End Region

#Region "Box"

    Private Sub TextBox47_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox47.TextAlignChanged
        TextBox47.Text = Regex.Replace(TextBox47.Text, BadChars, "")
        If Val(TextBox47.Text) <> TrackBar4.Value Then
            If Val(TextBox47.Text) > 255 Then TextBox47.Text = 255
            If Val(TextBox47.Text) < 0 Then TextBox47.Text = 0
            TrackBar4.Value = Val(TextBox47.Text)
            If CheckBox6.Checked = False Then
                TextBox49.TextAlign = HorizontalAlignment.Center
                TextBox49.Text = cColor(TrackBar4.Value, TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel5.BackColor)
            Else
                If TextBox48.Text.Length > 0 Then
                    TextBox49.TextAlign = HorizontalAlignment.Left
                    TextBox49.Text = "#define " & TextBox48.Text & " " & cColor(TrackBar4.Value, TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel5.BackColor)
                Else
                    CheckBox6.Checked = False
                    TextBox49.TextAlign = HorizontalAlignment.Center
                    TextBox49.Text = cColor(TrackBar4.Value, TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel5.BackColor)
                End If
            End If
        End If
    End Sub

#End Region

#End Region

#End Region

#Region "Visual"

    Private Sub CheckBox6_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox6.CheckedChanged
        If CheckBox6.Checked = True Then
            TextBox48.Visible = True
        Else
            TextBox48.Visible = False
        End If
    End Sub

#End Region

#Region "Exta"

    Private Sub TextBox48_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox48.TextChanged
        TrackBar1_Scroll(sender, e)
    End Sub

    Private Sub TabPage16_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles TabPage16.Enter
        ColorSender = 0
    End Sub

    Private Sub TabPage17_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles TabPage17.Enter
        ColorSender = 1
    End Sub

    Private Sub TabPage18_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles TabPage18.Enter
        ColorSender = 2
    End Sub

#End Region

#End Region

#Region "Area"

#Region "Drawing"

    Private Sub PictureBox3_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PictureBox3.MouseDown
        Select Case e.Button
            Case Windows.Forms.MouseButtons.Left
                With Selection
                    .xForward = True
                    .yForward = True
                    .minX = e.Location.X
                    .minY = e.Location.Y
                    .Lock = True
                End With
                If CheckBox8.Checked = False Then
                    PictureBox3.Image = My.Resources.Map
                End If
        End Select
    End Sub

    Private Sub PictureBox3_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PictureBox3.MouseMove
        If Selection.Lock = True Then
            With Selection
                If e.Location.X < 0 Then
                    .minX = 0
                ElseIf e.Location.Y < 0 Then
                    .minY = 0
                ElseIf PictureBox3.Width < e.Location.X Then
                    .maxX = PictureBox3.Width - 1
                    If PictureBox3.Height > e.Location.Y Then
                        If e.Location.Y > .minY Then
                            If .yForward = True Then
                                .maxY = e.Location.Y
                            Else
                                If e.Location.Y < .maxY Then
                                    .minY = e.Location.Y
                                Else
                                    .minY = .maxY
                                    .maxY = e.Location.Y
                                    .yForward = True
                                End If
                            End If
                        Else
                            If .yForward = True Then
                                .maxY = .minY
                                .minY = e.Location.Y
                                .yForward = False
                            Else
                                If .minY <= .maxY Then
                                    .minY = e.Location.Y
                                Else
                                    .maxY = e.Location.Y
                                End If
                            End If
                        End If
                    End If
                ElseIf PictureBox3.Height < e.Location.Y Then
                    .maxY = PictureBox3.Height - 1
                    If PictureBox3.Width > e.Location.X Then
                        If e.Location.X > .minX Then
                            If .xForward = True Then
                                .maxX = e.Location.X
                            Else
                                If e.Location.X < .maxX Then
                                    .minX = e.Location.X
                                Else
                                    .minX = .maxX
                                    .maxX = e.Location.X
                                    .xForward = True
                                End If
                            End If
                        Else
                            If .xForward = True Then
                                .maxX = .minX
                                .minX = e.Location.X
                                .xForward = False
                            Else
                                If .minX <= .maxX Then
                                    .minX = e.Location.X
                                Else
                                    .maxX = e.Location.X
                                End If
                            End If
                        End If
                    End If
                Else
                    If e.Location.X > .minX Then
                        If .xForward = True Then
                            .maxX = e.Location.X
                        Else
                            If e.Location.X < .maxX Then
                                .minX = e.Location.X
                            Else
                                .minX = .maxX
                                .maxX = e.Location.X
                                .xForward = True
                            End If
                        End If
                    Else
                        If .xForward = True Then
                            .maxX = .minX
                            .minX = e.Location.X
                            .xForward = False
                        Else
                            If .minX <= .maxX Then
                                .minX = e.Location.X
                            Else
                                .maxX = e.Location.X
                            End If
                        End If
                    End If
                    If e.Location.Y > .minY Then
                        If .yForward = True Then
                            .maxY = e.Location.Y
                        Else
                            If e.Location.Y < .maxY Then
                                .minY = e.Location.Y
                            Else
                                .minY = .maxY
                                .maxY = e.Location.Y
                                .yForward = True
                            End If
                        End If
                    Else
                        If .yForward = True Then
                            .maxY = .minY
                            .minY = e.Location.Y
                            .yForward = False
                        Else
                            If .minY <= .maxY Then
                                .minY = e.Location.Y
                            Else
                                .maxY = e.Location.Y
                            End If
                        End If
                    End If
                End If
                TextBox59.Text = Round(6000 / TrackBar14.Value * .minX - 3000, 6)
                TextBox60.Text = Round(6000 / TrackBar14.Value * .minY - 3000, 6)
                If .maxX = PictureBox3.Width - 1 Then
                    TextBox61.Text = 3000
                Else
                    TextBox61.Text = Round(6000 / TrackBar14.Value * .maxX - 3000, 6)
                End If
                If .maxY = PictureBox3.Height - 1 Then
                    TextBox62.Text = 3000
                Else
                    TextBox62.Text = Round(6000 / TrackBar14.Value * .maxY - 3000, 6)
                End If
                TextBox59.Text = TextBox59.Text.Replace(",", ".")
                TextBox60.Text = TextBox60.Text.Replace(",", ".")
                TextBox61.Text = TextBox61.Text.Replace(",", ".")
                TextBox62.Text = TextBox62.Text.Replace(",", ".")
                PictureBox3.Refresh()
                PictureBox3.CreateGraphics.DrawRectangle(New Pen(Config.C_Area.Hex), New Rectangle(.minX, .minY, .maxX - .minX, .maxY - .minY))
                PictureBox3.CreateGraphics.Dispose()
            End With
        End If
    End Sub

    Private Sub PictureBox3_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PictureBox3.MouseUp
        If Selection.Lock = True Then
            Dim g As Graphics, tmp As Single
            g = Graphics.FromImage(PictureBox3.Image)
            tmp = 2048 / TrackBar14.Value
            If CheckBox8.Checked = True Then
                If RadioButton18.Checked = True Then
                    Areas += String.Format("Area[{4}] = " & Config.ZoneCreate & vbNewLine & Config.ZoneShow & vbNewLine, TextBox59.Text, TextBox60.Text, TextBox61.Text, TextBox62.Text, aCount, "Area[" & aCount & "]", Config.C_Area.Name)
                Else
                    Areas += String.Format(Config.Bounds & vbNewLine, TextBox59.Text, TextBox61.Text, TextBox60.Text, TextBox62.Text)
                End If
                pAreas += Selection.minX & "|,|" & Selection.minY & "|,|" & Selection.maxX & "|,|" & Selection.maxY & "|;|"
                aCount += 1
            End If
                If CheckBox10.Checked = False Then
                    g.DrawRectangle(New Pen(Config.C_Area.Hex), Selection.minX * tmp, Selection.minY * tmp, (Selection.maxX - Selection.minX) * tmp, (Selection.maxY - Selection.minY) * tmp)
                Else
                    g.FillRectangle(New SolidBrush(Config.C_Area.Hex), Selection.minX * tmp, Selection.minY * tmp, (Selection.maxX - Selection.minX) * tmp, (Selection.maxY - Selection.minY) * tmp)
                End If
                g.Dispose()
                PictureBox3.Refresh()
                Selection.Lock = False
            End If
    End Sub

#End Region

#Region "Buttons"

    Private Sub Button15_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button15.Click
        AreaE.Show()
        AreaE.TextBox1.Clear()
        AreaE.Location = New Point(Me.Left, Me.Top + Me.Height)
        If CheckBox8.Checked = False Then
            If TextBox59.Text.Length = 0 Or TextBox60.Text.Length = 0 Or TextBox61.Text.Length = 0 Or TextBox62.Text.Length = 0 Then
                Select Case Config.Idioma
                    Case Lang.English
                        MsgBox("You must select an area first.", MsgBoxStyle.Critical, "Error")
                    Case Lang.Español
                        MsgBox("Debes seleccionar un area primero.", MsgBoxStyle.Critical, "Error")
                    Case Lang.Portugues
                        MsgBox("Você deve selecionar uma área em primeiro lugar.", MsgBoxStyle.Critical, "Error")
                    Case Else
                        MsgBox("Du musst zuerst ein Gebiet auswählen.", MsgBoxStyle.Critical, "Error")
                End Select
                AreaE.Hide()
                Exit Sub
            End If
            Try
                If RadioButton18.Checked = True Then
                    AreaE.TextBox1.Text = "new Area;" & vbNewLine
                    AreaE.TextBox1.Text += String.Format("Area = " & Config.ZoneCreate & vbNewLine & Config.ZoneShow, TextBox59.Text, TextBox60.Text, TextBox61.Text, TextBox62.Text, "Area", Config.C_Area.Name) & vbNewLine
                Else
                    AreaE.TextBox1.Text = String.Format(Config.Bounds & vbNewLine, TextBox59.Text, TextBox61.Text, TextBox60.Text, TextBox62.Text)
                End If
            Catch ex As Exception
                AreaE.Hide()
                Select Case Config.Idioma
                    Case Lang.English
                        MsgBox("Wrong format, please check Settings tab.", MsgBoxStyle.Critical, "Error")
                    Case Lang.Español
                        MsgBox("Error de formato, por favor chequear la pestaña de configuración.", MsgBoxStyle.Critical, "Error")
                    Case Lang.Portugues
                        MsgBox("Formato errado, por favor verifique guia de configuração.", MsgBoxStyle.Critical, "Error")
                    Case Else
                        MsgBox("Falsches Format, überprüfen Sie bitte config-Register.", MsgBoxStyle.Critical, "Error")
                End Select
                Exit Sub
            End Try
        Else
            If Areas Is Nothing OrElse Areas.Length = 0 Then
                Select Case Config.Idioma
                    Case Lang.English
                        MsgBox("You must select an area first.", MsgBoxStyle.Critical, "Error")
                    Case Lang.Español
                        MsgBox("Debes seleccionar un area primero.", MsgBoxStyle.Critical, "Error")
                    Case Lang.Portugues
                        MsgBox("Você deve selecionar uma área em primeiro lugar.", MsgBoxStyle.Critical, "Error")
                    Case Else
                        MsgBox("Du musst zuerst ein Gebiet auswählen.", MsgBoxStyle.Critical, "Error")
                End Select
                AreaE.Hide()
                Exit Sub
            End If
            If RadioButton18.Checked = True Then
                AreaE.TextBox1.Text = "new Area[" & aCount & "];" & vbNewLine
            End If
            AreaE.TextBox1.Text += Areas
        End If
    End Sub

    Private Sub Button16_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button16.Click
        PictureBox3.Image = My.Resources.Map
        AreaE.TextBox1.Clear()
        TextBox59.Clear()
        TextBox60.Clear()
        TextBox61.Clear()
        TextBox62.Clear()
        Areas = ""
        pAreas = ""
        aCount = 0
    End Sub

    Private Sub Button17_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button17.Click
        PictureBox3.Image = My.Resources.Map
        AreaE.TextBox1.Clear()
        TextBox59.Clear()
        TextBox60.Clear()
        TextBox61.Clear()
        TextBox62.Clear()
        Areas = ""
        pAreas = ""
        aCount = 0
    End Sub

    Private Sub Button18_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button18.Click
        gSender = CC.Area
        eColor.TrackBar4.Enabled = True
        eColor.TextBox4.Enabled = True
        eColor.Show()
        eColor.Focus()
        eColor.Panel1.BackColor = Config.C_Area.Hex
        eColor.TextBox1.Text = Config.C_Area.Hex.R
        eColor.TrackBar1.Value = Config.C_Area.Hex.R
        eColor.TextBox2.Text = Config.C_Area.Hex.G
        eColor.TrackBar2.Value = Config.C_Area.Hex.G
        eColor.TextBox3.Text = Config.C_Area.Hex.B
        eColor.TrackBar3.Value = Config.C_Area.Hex.B
        eColor.TextBox4.Text = Config.C_Area.Hex.A
        eColor.TrackBar4.Value = Config.C_Area.Hex.A
    End Sub

#End Region

#Region "Visual"

    Private Sub CheckBox8_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox8.CheckedChanged
        If CheckBox8.Checked = True Then
            Button17.Visible = True
            If TextBox59.Text.Length > 0 Then
                If RadioButton18.Checked = True Then
                    Areas += String.Format("Area[{4}] = " & Config.ZoneCreate & vbNewLine & Config.ZoneShow & vbNewLine, TextBox59.Text, TextBox60.Text, TextBox61.Text, TextBox62.Text, aCount, "Area[" & aCount & "]", Config.C_Area.Name)
                Else
                    Areas = String.Format(Config.Bounds & vbNewLine, TextBox59.Text, TextBox61.Text, TextBox60.Text, TextBox62.Text)
                End If
                pAreas += Selection.minX & "|,|" & Selection.minY & "|,|" & Selection.maxX & "|,|" & Selection.maxY & "|;|"
                aCount += 1
            End If
        Else
            Button17.Visible = False
            Areas = ""
            pAreas = ""
            PictureBox3.Image = My.Resources.Map
            TextBox59.Clear()
            TextBox60.Clear()
            TextBox61.Clear()
            TextBox62.Clear()
        End If
        Config.A_MSelect = CheckBox8.Checked
    End Sub

    Private Sub CheckBox10_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox10.CheckedChanged
        Dim res As Single = 2048 / TrackBar14.Value
        If CheckBox10.Checked = True Then
            If CheckBox8.Checked = True Then
                If Not Areas Is Nothing AndAlso Areas.Length > 1 Then
                    PictureBox3.Image = My.Resources.Map
                    Dim tmp As String(), line As String(), g As Graphics
                    tmp = Split(pAreas, "|;|")
                    g = Graphics.FromImage(PictureBox3.Image)
                    For i = 0 To UBound(tmp) - 1
                        line = Split(tmp(i), "|,|")
                        g.FillRectangle(New SolidBrush(Config.C_Area.Hex), CInt(line(0)) * res, CInt(line(1)) * res, (CInt(line(2)) - CInt(line(0))) * res, (CInt(line(3)) - CInt(line(1))) * res)
                    Next
                    g.Dispose()
                    PictureBox3.Refresh()
                End If
            Else
                PictureBox3.Image = My.Resources.Map
                Dim g As Graphics = Graphics.FromImage(PictureBox3.Image)
                g.FillRectangle(New SolidBrush(Config.C_Area.Hex), Selection.minX * res, Selection.minY * res, (Selection.maxX - Selection.minX) * res, (Selection.maxY - Selection.minY) * res)
                g.Dispose()
                PictureBox3.Refresh()
            End If
        Else
            If CheckBox8.Checked = True Then
                If Not Areas Is Nothing AndAlso Areas.Length > 1 Then
                    PictureBox3.Image = My.Resources.Map
                    Dim tmp As String(), line As String(), g As Graphics
                    tmp = Split(pAreas, "|;|")
                    g = Graphics.FromImage(PictureBox3.Image)
                    For i = 0 To UBound(tmp) - 1
                        line = Split(tmp(i), "|,|")
                        g.DrawRectangle(New Pen(Config.C_Area.Hex), CInt(line(0)) * res, CInt(line(1)) * res, (CInt(line(2)) - CInt(line(0))) * res, (CInt(line(3)) - CInt(line(1))) * res)
                    Next
                    g.Dispose()
                    PictureBox3.Refresh()
                End If
            Else
                PictureBox3.Image = My.Resources.Map
                Dim g As Graphics = Graphics.FromImage(PictureBox3.Image)
                g.DrawRectangle(New Pen(Config.C_Area.Hex), Selection.minX*res, Selection.minY*res, (Selection.maxX - Selection.minX)*res, (Selection.maxY - Selection.minY)*res)
                g.Dispose()
                PictureBox3.Refresh()
            End If
        End If
        Config.A_Fill = CheckBox10.Checked
    End Sub

    Private Sub TabPage13_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TabPage13.Click
        AreaE.Hide()
    End Sub

    Private Sub PictureBox3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox3.Click
        AreaE.Hide()
    End Sub

    Private Sub RadioButton18_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton18.CheckedChanged
        If CheckBox8.Checked = True Then
            If Not Areas Is Nothing AndAlso Areas.Length > 0 Then
                If RadioButton18.Checked = True Then
                    Areas = ""
                    Dim tmp As String(), line As String()
                    tmp = Split(pAreas, "|;|")
                    For i = 0 To UBound(tmp) - 1
                        line = Split(tmp(i), "|,|")
                        Areas += String.Format("Area[{4}] = " & Config.ZoneCreate & vbNewLine & Config.ZoneShow & vbNewLine, Round(6000 / TrackBar14.Value * CInt(line(0)) - 3000, 6), Round(6000 / TrackBar14.Value * CInt(line(1)) - 3000, 6), Round(6000 / TrackBar14.Value * CInt(line(2)) - 3000, 6), Round(6000 / TrackBar14.Value * CInt(line(3)) - 3000, 6), aCount, "Area[" & aCount & "]", Config.C_Area.Name)
                    Next
                End If
            End If
        End If
    End Sub

    Private Sub RadioButton19_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton19.CheckedChanged
        If CheckBox8.Checked = True Then
            If Not Areas Is Nothing AndAlso Areas.Length > 0 Then
                If RadioButton19.Checked = True Then
                    Areas = ""
                    Dim tmp As String(), line As String()
                    tmp = Split(pAreas, "|;|")
                    For i = 0 To UBound(tmp) - 1
                        line = Split(tmp(i), "|,|")
                        Areas += String.Format(Config.Bounds & vbNewLine, Round(6000 / TrackBar14.Value * CInt(line(0)) - 3000, 6), Round(6000 / TrackBar14.Value * CInt(line(2)) - 3000, 6), Round(6000 / TrackBar14.Value * CInt(line(1)) - 3000, 6), Round(6000 / TrackBar14.Value * CInt(line(3)) - 3000, 6))
                    Next
                End If
            End If
        End If
    End Sub

#End Region

#Region "Zoom"

    Private Sub TrackBar14_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBar14.Scroll
        If TrackBar14.Value = 461 Then
            TrackBar12.Enabled = False
            TrackBar13.Enabled = False
        Else
            TrackBar12.Enabled = True
            TrackBar13.Enabled = True
            TrackBar12.Maximum = TrackBar14.Value - 461
            TrackBar13.Minimum = (TrackBar14.Value - 461) * -1
        End If
        PictureBox3.Size = New Point(TrackBar14.Value, TrackBar14.Value)
        PictureBox3.Location = New Point(0, 0)
        TrackBar12.Value = TrackBar12.Minimum
        TrackBar13.Value = TrackBar13.Maximum
    End Sub

    Private Sub TrackBar12_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBar12.Scroll
        PictureBox3.Location = New Point(TrackBar12.Value * -1, TrackBar13.Value)
    End Sub

    Private Sub TrackBar13_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBar13.Scroll
        PictureBox3.Location = New Point(TrackBar12.Value * -1, TrackBar13.Value)
    End Sub

#End Region

#End Region

#Region "Object Converter"

#Region "Generate Code"

    Private Sub Button14_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button14.Click
        On Error GoTo Fail
        If RichTextBox1.Text.Length = 0 Then
            Select Case Config.Idioma
                Case Lang.English
                    MsgBox("You must add some objects to convert.", MsgBoxStyle.Critical, "Error")
                Case Lang.Español
                    MsgBox("Debes ingresar objetos para convertir.", MsgBoxStyle.Critical, "Error")
                Case Lang.Portugues
                    MsgBox("Você deve adicionar alguns objetos para converter.", MsgBoxStyle.Critical, "Error")
                Case Else
                    MsgBox("Du musst Objekte zum konvertieren auswählen.", MsgBoxStyle.Critical, "Error")
            End Select
            Exit Sub
        End If
        TextBox58.Clear()
        If ComboBox3.SelectedIndex = ComboBox4.SelectedIndex Then
            If CheckBox9.Checked = False Then
                TextBox58.Text = RichTextBox1.Text
                Exit Sub
            End If
        End If
        Dim pos() As String, Output As String, count As Integer
        Select Case ComboBox4.SelectedIndex
            Case 0
                Output = "CreateObject({0}, {1}, {2}, {3}, {4}, {5}, {6});" & vbNewLine
            Case 1
                Output = "CreateDynamicObject({0}, {1}, {2}, {3}, {4}, {5}, {6});" & vbNewLine
            Case 2
                Output = "CreateDynamicObject({0}, {1}, {2}, {3}, {4}, {5}, {6});" & vbNewLine
            Case 3
                Output = "CreateStreamedObject({0}, {1}, {2}, {3}, {4}, {5}, {6});" & vbNewLine
            Case 4
                Output = "CreateStreamObject({0}, {1}, {2}, {3}, {4}, {5}, {6}, 250.0);" & vbNewLine
            Case 5
                Output = "CreateStreamObject({0}, {1}, {2}, {3}, {4}, {5}, {6}, 250.0);" & vbNewLine
            Case 6
                Output = "F_CreateObject({0}, {1}, {2}, {3}, {4}, {5}, {6});" & vbNewLine
            Case Else
                Output = "CreateObject({0}, {1}, {2}, {3}, {4}, {5}, {6});" & vbNewLine
        End Select
        Select Case ComboBox3.SelectedIndex
            Case 0
                For Each line In RichTextBox1.Lines
                    If line.IndexOf("CreateObject(") >= 0 Then
                        pos = Split(Trim(Mid(Mid(line, line.IndexOf("(") + 2, line.Length - line.IndexOf("(")), 1, line.Length - 3)), ",")
                        Dim i As Integer = UBound(pos)
                        pos(i) = Mid(pos(i), 1, pos(i).IndexOf(")"))
                        TextBox58.Text += String.Format(Output, pos(0), Convert.ToDecimal(pos(1)), Convert.ToDecimal(pos(2)), Convert.ToDecimal(pos(3)), Convert.ToDecimal(pos(4)), Convert.ToDecimal(pos(5)), Convert.ToDecimal(pos(6)))
                        count += 1
                    End If
                Next
            Case 1
                Dim Obj As ObjectS, lock As Boolean = False
                For Each Line In RichTextBox1.Lines
                    If Line.IndexOf("<position>") >= 0 Then
                        pos = Split(Mid(Mid(Line, Line.IndexOf(">") + 2, Line.Length - Line.IndexOf(">")), 1, Line.Length - 25))
                        Obj.X = Convert.ToDecimal(pos(0), New Globalization.CultureInfo("en-US"))
                        Obj.Y = Convert.ToDecimal(pos(1), New Globalization.CultureInfo("en-US"))
                        Obj.Z = Convert.ToDecimal(pos(2), New Globalization.CultureInfo("en-US"))
                    ElseIf Line.IndexOf("<rotation>") >= 0 Then
                        pos = Split(Mid(Mid(Line, Line.IndexOf(">") + 2, Line.Length - Line.IndexOf(">")), 1, Line.Length - 25))
                        Obj.rX = Convert.ToDecimal(pos(0), New Globalization.CultureInfo("en-US"))
                        Obj.rY = Convert.ToDecimal(pos(1), New Globalization.CultureInfo("en-US"))
                        Obj.rZ = Convert.ToDecimal(pos(2), New Globalization.CultureInfo("en-US"))
                    ElseIf Line.IndexOf("<model>") >= 0 Then
                        Obj.Model = Mid(Mid(Line, Line.IndexOf(">") + 2, Line.Length - Line.IndexOf(">")), 1, Line.Length - 19)
                        If CheckBox9.Checked = True Then
                            Select Case Obj.Model
                                Case 14383 To 14483
                                    Obj.Model += 4248
                                Case 14770 To 14856
                                    Obj.Model += 4063
                                Case 14858 To 14871
                                    Obj.Model += 4062
                                Case 18000 To 18036
                                    Obj.Model += 934
                                Case 18038 To 18101
                                    Obj.Model += 933
                                Case 14872 To 14883
                                    Obj.Model += 4163
                                Case 14885 To 14891
                                    Obj.Model += 4162
                                Case 13590 To 13667
                                    Obj.Model += 5142
                                Case 14500 To 14522
                                    Obj.Model += 4310
                                Case 12835 To 12944
                                    Obj.Model += 6219
                                Case 16000 To 16143
                                    Obj.Model += 3164
                                Case 14892
                                    Obj.Model += 5009
                            End Select
                        End If
                        lock = True
                    End If
                    If lock = True Then
                        TextBox58.Text += String.Format(Output, Obj.Model, Obj.X, Obj.Y, Obj.Z, Obj.rX, Obj.rY, Obj.rZ)
                        count += 1
                        lock = False
                    End If
                Next
            Case 2
                Dim Obj As ObjectS, lock As Boolean = False
                For Each Line In RichTextBox1.Lines
                    If Line.IndexOf("<object") >= 0 Then
                        Obj.Model = Val(Mid(Line, Line.IndexOf("model=") + 8, Line.IndexOf("""", Line.IndexOf("model=")) - Line.IndexOf("model=") - 1))
                        If CheckBox9.Checked = True Then
                            Select Case Obj.Model
                                Case 14383 To 14483
                                    Obj.Model += 4248
                                Case 14770 To 14856
                                    Obj.Model += 4063
                                Case 14858 To 14871
                                    Obj.Model += 4062
                                Case 18000 To 18036
                                    Obj.Model += 934
                                Case 18038 To 18101
                                    Obj.Model += 933
                                Case 14872 To 14883
                                    Obj.Model += 4163
                                Case 14885 To 14891
                                    Obj.Model += 4162
                                Case 13590 To 13667
                                    Obj.Model += 5142
                                Case 14500 To 14522
                                    Obj.Model += 4310
                                Case 12835 To 12944
                                    Obj.Model += 6219
                                Case 16000 To 16143
                                    Obj.Model += 3164
                                Case 14892
                                    Obj.Model += 5009
                            End Select
                        End If
                        Obj.X = Round(Convert.ToDecimal(Mid(Line, Line.IndexOf("posX=") + 7, Line.IndexOf(""" ", Line.IndexOf("posX=")) - Line.IndexOf("posX=") - 6), New Globalization.CultureInfo("en-US")), 6)
                        Obj.Y = Round(Convert.ToDecimal(Mid(Line, Line.IndexOf("posY=") + 7, Line.IndexOf(""" ", Line.IndexOf("posY=")) - Line.IndexOf("posY=") - 6), New Globalization.CultureInfo("en-US")), 6)
                        Obj.Z = Round(Convert.ToDecimal(Mid(Line, Line.IndexOf("posZ=") + 7, Line.IndexOf(""" ", Line.IndexOf("posZ=")) - Line.IndexOf("posZ=") - 6), New Globalization.CultureInfo("en-US")), 6)
                        Obj.rX = Round(Convert.ToDecimal(Mid(Line, Line.IndexOf("rotX=") + 7, Line.IndexOf(""" ", Line.IndexOf("rotX=")) - Line.IndexOf("rotX=") - 6), New Globalization.CultureInfo("en-US")), 6)
                        Obj.rY = Round(Convert.ToDecimal(Mid(Line, Line.IndexOf("rotY=") + 7, Line.IndexOf(""" ", Line.IndexOf("rotY=")) - Line.IndexOf("rotY=") - 6), New Globalization.CultureInfo("en-US")), 6)
                        Obj.rZ = Round(Convert.ToDecimal(Mid(Line, Line.IndexOf("rotZ=") + 7, Line.IndexOf(""" ", Line.IndexOf("rotZ=")) - Line.IndexOf("rotZ=") - 6), New Globalization.CultureInfo("en-US")), 6)
                        TextBox58.Text += String.Format(Output, Obj.Model, Obj.X, Obj.Y, Obj.Z, Obj.rX, Obj.rY, Obj.rZ)
                        count += 1
                    End If
                Next
            Case 3, 4
                For Each Line In RichTextBox1.Lines
                    If Line.IndexOf("CreateDynamicObject(") >= 0 Then
                        pos = Split(Trim(Mid(Mid(Line, Line.IndexOf("(") + 2, Line.Length - Line.IndexOf("(")), 1, Line.Length - 3)), ",")
                        Dim i As Integer = UBound(pos)
                        pos(i) = Mid(pos(i), 1, pos(i).IndexOf(")"))
                        If CheckBox9.Checked = True Then
                            Select Case pos(0)
                                Case 14383 To 14483
                                    pos(0) += 4248
                                Case 14770 To 14856
                                    pos(0) += 4063
                                Case 14858 To 14871
                                    pos(0) += 4062
                                Case 18000 To 18036
                                    pos(0) += 934
                                Case 18038 To 18101
                                    pos(0) += 933
                                Case 14872 To 14883
                                    pos(0) += 4163
                                Case 14885 To 14891
                                    pos(0) += 4162
                                Case 13590 To 13667
                                    pos(0) += 5142
                                Case 14500 To 14522
                                    pos(0) += 4310
                                Case 12835 To 12944
                                    pos(0) += 6219
                                Case 16000 To 16143
                                    pos(0) += 3164
                                Case 14892
                                    pos(0) += 5009
                            End Select
                        End If
                        TextBox58.Text += String.Format(Output, pos(0), Convert.ToDecimal(pos(1)), Convert.ToDecimal(pos(2)), Convert.ToDecimal(pos(3)), Convert.ToDecimal(pos(4)), Convert.ToDecimal(pos(5)), Convert.ToDecimal(pos(6)))
                        count += 1
                    End If
                Next
            Case 5
                For Each Line In RichTextBox1.Lines
                    If Line.IndexOf("CreateStreamedObject(") >= 0 Then
                        pos = Split(Trim(Mid(Mid(Line, Line.IndexOf("(") + 2, Line.Length - Line.IndexOf("(")), 1, Line.Length - 3)), ",")
                        Dim i As Integer = UBound(pos)
                        pos(i) = Mid(pos(i), 1, pos(i).IndexOf(")"))
                        If CheckBox9.Checked = True Then
                            Select Case pos(0)
                                Case 14383 To 14483
                                    pos(0) += 4248
                                Case 14770 To 14856
                                    pos(0) += 4063
                                Case 14858 To 14871
                                    pos(0) += 4062
                                Case 18000 To 18036
                                    pos(0) += 934
                                Case 18038 To 18101
                                    pos(0) += 933
                                Case 14872 To 14883
                                    pos(0) += 4163
                                Case 14885 To 14891
                                    pos(0) += 4162
                                Case 13590 To 13667
                                    pos(0) += 5142
                                Case 14500 To 14522
                                    pos(0) += 4310
                                Case 12835 To 12944
                                    pos(0) += 6219
                                Case 16000 To 16143
                                    pos(0) += 3164
                                Case 14892
                                    pos(0) += 5009
                            End Select
                        End If
                        TextBox58.Text += String.Format(Output, pos(0), Convert.ToDecimal(pos(1)), Convert.ToDecimal(pos(2)), Convert.ToDecimal(pos(3)), Convert.ToDecimal(pos(4)), Convert.ToDecimal(pos(5)), Convert.ToDecimal(pos(6)))
                        count += 1
                    End If
                Next
            Case 6, 7
                For Each Line In RichTextBox1.Lines
                    If Line.IndexOf("CreateStreamObject(") >= 0 Then
                        pos = Split(Trim(Mid(Mid(Line, Line.IndexOf("(") + 2, Line.Length - Line.IndexOf("(")), 1, Line.Length - 3)), ",")
                        Dim i As Integer = UBound(pos)
                        pos(i) = Mid(pos(i), 1, pos(i).IndexOf(")"))
                        If CheckBox9.Checked = True Then
                            Select Case pos(0)
                                Case 14383 To 14483
                                    pos(0) += 4248
                                Case 14770 To 14856
                                    pos(0) += 4063
                                Case 14858 To 14871
                                    pos(0) += 4062
                                Case 18000 To 18036
                                    pos(0) += 934
                                Case 18038 To 18101
                                    pos(0) += 933
                                Case 14872 To 14883
                                    pos(0) += 4163
                                Case 14885 To 14891
                                    pos(0) += 4162
                                Case 13590 To 13667
                                    pos(0) += 5142
                                Case 14500 To 14522
                                    pos(0) += 4310
                                Case 12835 To 12944
                                    pos(0) += 6219
                                Case 16000 To 16143
                                    pos(0) += 3164
                                Case 14892
                                    pos(0) += 5009
                            End Select
                        End If
                        TextBox58.Text += String.Format(Output, pos(0), Convert.ToDecimal(pos(1)), Convert.ToDecimal(pos(2)), Convert.ToDecimal(pos(3)), Convert.ToDecimal(pos(4)), Convert.ToDecimal(pos(5)), Convert.ToDecimal(pos(6)))
                        count += 1
                    End If
                Next
            Case 8
                For Each Line In RichTextBox1.Lines
                    If Line.IndexOf("F_CreateObject(") >= 0 Then
                        pos = Split(Trim(Mid(Mid(Line, Line.IndexOf("(") + 2, Line.Length - Line.IndexOf("(")), 1, Line.Length - 3)), ",")
                        Dim i As Integer = UBound(pos)
                        pos(i) = Mid(pos(i), 1, pos(i).IndexOf(")"))
                        If CheckBox9.Checked = True Then
                            Select Case pos(0)
                                Case 14383 To 14483
                                    pos(0) += 4248
                                Case 14770 To 14856
                                    pos(0) += 4063
                                Case 14858 To 14871
                                    pos(0) += 4062
                                Case 18000 To 18036
                                    pos(0) += 934
                                Case 18038 To 18101
                                    pos(0) += 933
                                Case 14872 To 14883
                                    pos(0) += 4163
                                Case 14885 To 14891
                                    pos(0) += 4162
                                Case 13590 To 13667
                                    pos(0) += 5142
                                Case 14500 To 14522
                                    pos(0) += 4310
                                Case 12835 To 12944
                                    pos(0) += 6219
                                Case 16000 To 16143
                                    pos(0) += 3164
                                Case 14892
                                    pos(0) += 5009
                            End Select
                        End If
                        TextBox58.Text += String.Format(Output, pos(0), Convert.ToDecimal(pos(1)), Convert.ToDecimal(pos(2)), Convert.ToDecimal(pos(3)), Convert.ToDecimal(pos(4)), Convert.ToDecimal(pos(5)), Convert.ToDecimal(pos(6)))
                        count += 1
                    End If
                Next
        End Select
        Select Case Config.Idioma
            Case Lang.English
                MsgBox(String.Format("Converted objects: {0}", count), MsgBoxStyle.OkOnly, "Object converter")
            Case Lang.Español
                MsgBox(String.Format("Objetos convertidos: {0}", count), MsgBoxStyle.OkOnly, "Conversor de objetos")
            Case Lang.Portugues
                MsgBox(String.Format("Objetos convertidos: {0}", count), MsgBoxStyle.OkOnly, "Object Converter")
            Case Else
                MsgBox(String.Format("Konvertierte Objekte: {0}", count), MsgBoxStyle.OkOnly, "Object converter")
        End Select
        Exit Sub
Fail:
        TextBox58.Clear()
        Exit Sub
    End Sub

#End Region

#Region "From File"

    Private Sub Button20_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button20.Click
        OFD.InitialDirectory = Config.oPath
        OFD.Multiselect = False
        If OFD.ShowDialog = Windows.Forms.DialogResult.Cancel Then Exit Sub
        Dim Reader As New IO.StreamReader(OFD.FileName)
        Config.oPath = Mid(OFD.FileName, 1, OFD.FileName.LastIndexOf("\"))
        RichTextBox1.Text = Reader.ReadToEnd()
        Reader.Close()
    End Sub

#End Region

#End Region

#Region "Info"

#Region "Skins"

#Region "Display"

    Private Sub TreeView1_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles TreeView1.AfterSelect
        If IsNumeric(TreeView1.SelectedNode.Text) Then
            Dim tmp As Boolean
            If Config.Images = True Or Config.URL_Skin = "" Then
                tmp = True
            Else
                tmp = False
            End If
            If tmp = False Then
                Try
                    PictureBox1.Image = LoadImageFromURL(String.Format(Config.URL_Skin, TreeView1.SelectedNode.Text))
                Catch ex As Exception
                    PictureBox1.Image = My.Resources.N_A
                End Try
            End If
            For Each Skin In Skins
                If TreeView1.SelectedNode.Text = Skin.ID Then
                    If tmp = True Then
                        Try
                            PictureBox1.Image = Image.FromFile(My.Application.Info.DirectoryPath & "\Resources\Skin_" & Skin.ID & ".png")
                        Catch ex As Exception
                            PictureBox1.Image = My.Resources.N_A
                        End Try
                    End If
                    TextBox69.Text = Skin.Name
                    TextBox70.Text = Skin.Gender.ToString
                    TextBox71.Text = Skin.Gang.ToString
                    Exit For
                End If
            Next
        End If
    End Sub

#End Region

#End Region

#Region "Vehicles"

#Region "Text Restrictions"

    Private Sub TextBox52_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox52.KeyPress
        If Not IsNumeric(e.KeyChar) And Not Char.IsControl(e.KeyChar) And Asc(e.KeyChar) <> 8 Then e.Handled = True
        TextBox53.Clear()
    End Sub

    Private Sub TextBox52_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox52.TextChanged
        TextBox52.Text = Regex.Replace(TextBox52.Text, BadChars, "")
        TextBox53.Clear()
    End Sub

    Private Sub TextBox53_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox53.KeyPress
        TextBox52.Text = "0"
    End Sub

    Private Sub TextBox53_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox53.TextChanged
        TextBox52.Text = "0"
    End Sub

#End Region

#Region "Visual"

    Private Sub TreeView2_AfterCollapse(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles TreeView2.AfterCollapse
        If VehSender = False Then
            VehSender = True
            Select Case e.Node.Text
                Case "Airplanes"
                    TreeView3.Nodes(0).Collapse()
                Case "Helicopters"
                    TreeView3.Nodes(1).Collapse()
                Case "Bikes"
                    TreeView3.Nodes(2).Collapse()
                Case "Convertibles"
                    TreeView3.Nodes(3).Collapse()
                Case "Industrial"
                    TreeView3.Nodes(4).Collapse()
                Case "Lowriders"
                    TreeView3.Nodes(5).Collapse()
                Case "Off Road"
                    TreeView3.Nodes(6).Collapse()
                Case "Public Service"
                    TreeView3.Nodes(7).Collapse()
                Case "Saloons"
                    TreeView3.Nodes(8).Collapse()
                Case "Sport"
                    TreeView3.Nodes(9).Collapse()
                Case "Station Wagons"
                    TreeView3.Nodes(10).Collapse()
                Case "Boats"
                    TreeView3.Nodes(11).Collapse()
                Case "Trailers"
                    TreeView3.Nodes(12).Collapse()
                Case "Unique"
                    TreeView3.Nodes(13).Collapse()
                Case "RC"
                    TreeView3.Nodes(14).Collapse()
                Case "All"
                    TreeView3.Nodes(15).Collapse()
            End Select
            VehSender = False
        End If
    End Sub

    Private Sub TreeView2_AfterExpand(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles TreeView2.AfterExpand
        If VehSender = False Then
            VehSender = True
            Select Case e.Node.Text
                Case "Airplanes"
                    TreeView3.Nodes(0).Expand()
                Case "Helicopters"
                    TreeView3.Nodes(1).Expand()
                Case "Bikes"
                    TreeView3.Nodes(2).Expand()
                Case "Convertibles"
                    TreeView3.Nodes(3).Expand()
                Case "Industrial"
                    TreeView3.Nodes(4).Expand()
                Case "Lowriders"
                    TreeView3.Nodes(5).Expand()
                Case "Off Road"
                    TreeView3.Nodes(6).Expand()
                Case "Public Service"
                    TreeView3.Nodes(7).Expand()
                Case "Saloons"
                    TreeView3.Nodes(8).Expand()
                Case "Sport"
                    TreeView3.Nodes(9).Expand()
                Case "Station Wagons"
                    TreeView3.Nodes(10).Expand()
                Case "Boats"
                    TreeView3.Nodes(11).Expand()
                Case "Trailers"
                    TreeView3.Nodes(12).Expand()
                Case "Unique"
                    TreeView3.Nodes(13).Expand()
                Case "RC"
                    TreeView3.Nodes(14).Expand()
                Case "All"
                    TreeView3.Nodes(15).Expand()
            End Select
            VehSender = False
        End If
    End Sub

    Private Sub TreeView2_MouseWheel(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles TreeView2.MouseWheel
        If VehSender = False Then
            VehSender = True
            Dim pos As Integer
            pos = GetScrollPos(TreeView2.Handle, SBS_VERT) + 3
            SetScrollPos(TreeView3.Handle, SBS_VERT, pos, True)
            PostMessageA(TreeView3.Handle, WM_VSCROLL, SB_THUMBPOSITION + &H10000 * pos, Nothing)
        End If
        VehSender = False
    End Sub

    Private Sub TreeView3_AfterCollapse(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles TreeView3.AfterCollapse
        If VehSender = False Then
            VehSender = True
            Select Case e.Node.Text
                Case "Airplanes"
                    TreeView2.Nodes(0).Collapse()
                Case "Helicopters"
                    TreeView2.Nodes(1).Collapse()
                Case "Bikes"
                    TreeView2.Nodes(2).Collapse()
                Case "Convertibles"
                    TreeView2.Nodes(3).Collapse()
                Case "Industrial"
                    TreeView2.Nodes(4).Collapse()
                Case "Lowriders"
                    TreeView2.Nodes(5).Collapse()
                Case "Off Road"
                    TreeView2.Nodes(6).Collapse()
                Case "Public Service"
                    TreeView2.Nodes(7).Collapse()
                Case "Saloons"
                    TreeView2.Nodes(8).Collapse()
                Case "Sport"
                    TreeView2.Nodes(9).Collapse()
                Case "Station Wagons"
                    TreeView2.Nodes(10).Collapse()
                Case "Boats"
                    TreeView2.Nodes(11).Collapse()
                Case "Trailers"
                    TreeView2.Nodes(12).Collapse()
                Case "Unique"
                    TreeView2.Nodes(13).Collapse()
                Case "RC"
                    TreeView2.Nodes(14).Collapse()
                Case "All"
                    TreeView2.Nodes(15).Collapse()
            End Select
            VehSender = False
        End If
    End Sub

    Private Sub TreeView3_AfterExpand(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles TreeView3.AfterExpand
        If VehSender = False Then
            VehSender = True
            Select Case e.Node.Text
                Case "Airplanes"
                    TreeView2.Nodes(0).Expand()
                Case "Helicopters"
                    TreeView2.Nodes(1).Expand()
                Case "Bikes"
                    TreeView2.Nodes(2).Expand()
                Case "Convertibles"
                    TreeView2.Nodes(3).Expand()
                Case "Industrial"
                    TreeView2.Nodes(4).Expand()
                Case "Lowriders"
                    TreeView2.Nodes(5).Expand()
                Case "Off Road"
                    TreeView2.Nodes(6).Expand()
                Case "Public Service"
                    TreeView2.Nodes(7).Expand()
                Case "Saloons"
                    TreeView2.Nodes(8).Expand()
                Case "Sport"
                    TreeView2.Nodes(9).Expand()
                Case "Station Wagons"
                    TreeView2.Nodes(10).Expand()
                Case "Boats"
                    TreeView2.Nodes(11).Expand()
                Case "Trailers"
                    TreeView2.Nodes(12).Expand()
                Case "Unique"
                    TreeView2.Nodes(13).Expand()
                Case "RC"
                    TreeView2.Nodes(14).Expand()
                Case "All"
                    TreeView2.Nodes(15).Expand()
            End Select
            VehSender = False
        End If
    End Sub

    Private Sub TreeView3_MouseWheel(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles TreeView3.MouseWheel
        If VehSender = False Then
            VehSender = True
            Dim pos As Integer
            pos = GetScrollPos(TreeView3.Handle, SBS_VERT) + 3
            SetScrollPos(TreeView2.Handle, SBS_VERT, pos, True)
            PostMessageA(TreeView2.Handle, WM_VSCROLL, SB_THUMBPOSITION + &H10000 * pos, Nothing)
        End If
        VehSender = False
    End Sub

#End Region

#Region "Display"

    Private Sub TreeView2_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles TreeView2.AfterSelect
        If IsNumeric(TreeView2.SelectedNode.Text) Then
            Dim tmp As Boolean
            If Config.Images = True Or Config.URL_Veh = "" Then : tmp = True
            Else : tmp = False
            End If
            If tmp = False Then
                Try
                    PictureBox2.Image = LoadImageFromURL(String.Format(Config.URL_Veh, TreeView2.SelectedNode.Text))
                Catch ex As Exception
                    PictureBox1.Image = My.Resources.N_A
                End Try
            End If
            For Each vehicle In Vehicles
                If vehicle.ID = TreeView2.SelectedNode.Text Then
                    If tmp = True Then
                        Try
                            Dim stmp As String = My.Application.Info.DirectoryPath.ToString.Replace("\", "/") & "/Resources/Vehicle_" & vehicle.ID & ".bmp"
                            PictureBox2.Image = Image.FromFile(stmp)
                        Catch ex As Exception
                            PictureBox1.Image = My.Resources.N_A
                        End Try
                    End If
                    TextBox54.Text = vehicle.ID
                    TextBox55.Text = vehicle.Name
                    TextBox56.Text = vehicle.Type.ToString
                    Exit For
                End If
            Next
        End If
    End Sub

    Private Sub TreeView3_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles TreeView3.AfterSelect
        Select Case TreeView3.SelectedNode.Text
            Case "Airplanes", "Helicopters", "Bikes", "Convertibles", "Industrial", "Lowriders", "Off Road", "Public Service", "Saloons", "Sport", "Station Wagons", "Boats""Trailers", "Unique", "RC", "All"
            Case Else
                Dim tmp As Boolean
                If Config.Images = True Or Config.URL_Veh = "" Then : tmp = True
                Else : tmp = False
                End If
                If tmp = False Then
                    Try
                        PictureBox2.Image = LoadImageFromURL(String.Format(Config.URL_Veh, TreeView2.SelectedNode.Text))
                    Catch ex As Exception
                        PictureBox1.Image = My.Resources.N_A
                    End Try
                End If
                For Each vehicle In Vehicles
                    If vehicle.Name = TreeView3.SelectedNode.Text Then
                        If tmp = True Then
                            Try
                                PictureBox2.Image = Image.FromFile(My.Application.Info.DirectoryPath & "/Resources/Vehicle_" & vehicle.ID & ".bmp")
                            Catch ex As Exception
                                PictureBox1.Image = My.Resources.N_A
                            End Try
                        End If
                        TextBox54.Text = vehicle.ID
                        TextBox55.Text = vehicle.Name
                        TextBox56.Text = vehicle.Type.ToString
                        Exit For
                    End If
                Next
        End Select
    End Sub

#End Region

#Region "Find"

    Private Sub Button12_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button12.Click
        If TextBox52.Text.Length > 0 And TextBox52.Text <> "0" Then
            If TextBox52.Text > 611 Or TextBox52.Text < 400 Then
                Select Case Config.Idioma
                    Case Lang.English
                        MsgBox("Invalid model ID.", MsgBoxStyle.Critical, "Error")
                    Case Lang.Español
                        MsgBox("Modelo inválido.", MsgBoxStyle.Critical, "Error")
                    Case Lang.Portugues
                        MsgBox("Modelo inválido.", MsgBoxStyle.Critical, "Error")
                    Case Else
                        MsgBox("Ungültige ModelID.", MsgBoxStyle.Critical, "Error")
                End Select
                Exit Sub
            End If
            Dim found As Boolean = False
            For Each vehicle In Vehicles
                If TextBox52.Text = vehicle.ID Then
                    Select Case vehicle.Type
                        Case VehicleType.Airplane
                            TreeView3.SelectedNode = GetNodeItem(TreeView3.Nodes(0).Nodes, vehicle.Name)
                        Case VehicleType.Helicopter
                            TreeView3.SelectedNode = GetNodeItem(TreeView3.Nodes(1).Nodes, vehicle.Name)
                        Case VehicleType.Bike
                            TreeView3.SelectedNode = GetNodeItem(TreeView3.Nodes(2).Nodes, vehicle.Name)
                        Case VehicleType.Convertible
                            TreeView3.SelectedNode = GetNodeItem(TreeView3.Nodes(3).Nodes, vehicle.Name)
                        Case VehicleType.Industrial
                            TreeView3.SelectedNode = GetNodeItem(TreeView3.Nodes(4).Nodes, vehicle.Name)
                        Case VehicleType.Lowriders
                            TreeView3.SelectedNode = GetNodeItem(TreeView3.Nodes(5).Nodes, vehicle.Name)
                        Case VehicleType.Off_Road
                            TreeView3.SelectedNode = GetNodeItem(TreeView3.Nodes(6).Nodes, vehicle.Name)
                        Case VehicleType.Public_Service
                            TreeView3.SelectedNode = GetNodeItem(TreeView3.Nodes(7).Nodes, vehicle.Name)
                        Case VehicleType.Saloon
                            TreeView3.SelectedNode = GetNodeItem(TreeView3.Nodes(8).Nodes, vehicle.Name)
                        Case VehicleType.Sport
                            TreeView3.SelectedNode = GetNodeItem(TreeView3.Nodes(9).Nodes, vehicle.Name)
                        Case VehicleType.Station_Wagon
                            TreeView3.SelectedNode = GetNodeItem(TreeView3.Nodes(10).Nodes, vehicle.Name)
                        Case VehicleType.Boat
                            TreeView3.SelectedNode = GetNodeItem(TreeView3.Nodes(11).Nodes, vehicle.Name)
                        Case VehicleType.Trailer
                            TreeView3.SelectedNode = GetNodeItem(TreeView3.Nodes(12).Nodes, vehicle.Name)
                        Case VehicleType.Unique
                            TreeView3.SelectedNode = GetNodeItem(TreeView3.Nodes(13).Nodes, vehicle.Name)
                        Case VehicleType.RC
                            TreeView3.SelectedNode = GetNodeItem(TreeView3.Nodes(14).Nodes, vehicle.Name)
                    End Select
                    found = True
                    Exit For
                End If
            Next
            If found = False Then
                Select Case Config.Idioma
                    Case Lang.English
                        MsgBox("Vehicle not found.", MsgBoxStyle.Critical, "Error")
                    Case Lang.Español
                        MsgBox("Vehiculo no encontrado.", MsgBoxStyle.Critical, "Error")
                    Case Lang.Portugues
                        MsgBox("Veículo não encontrado.", MsgBoxStyle.Critical, "Error")
                    Case Else
                        MsgBox("Fahrzeug nicht gefunden.", MsgBoxStyle.Critical, "Error")
                End Select
                Exit Sub
            End If
            TreeView3.Select()
        Else
            If TextBox52.Text.Length = 0 Then
                Select Case Config.Idioma
                    Case Lang.English
                        MsgBox("Invalid model name.", MsgBoxStyle.Critical, "Error")
                    Case Lang.Español
                        MsgBox("Modelo inválido.", MsgBoxStyle.Critical, "Error")
                    Case Lang.Portugues
                        MsgBox("Modelo inválido.", MsgBoxStyle.Critical, "Error")
                    Case Else
                        MsgBox("Ungültiger Modellname.", MsgBoxStyle.Critical, "Error")
                End Select
                Exit Sub
            End If
            Dim found As Boolean, count As Integer
            found = False
            For Each vehicle In Vehicles
                If vehicle.Name.IndexOf(TextBox53.Text) >= 0 Then
                    If count = VehPos Then
                        Select Case vehicle.Type
                            Case VehicleType.Airplane
                                TreeView3.SelectedNode = GetNodeItem(TreeView3.Nodes(0).Nodes, vehicle.Name)
                            Case VehicleType.Helicopter
                                TreeView3.SelectedNode = GetNodeItem(TreeView3.Nodes(1).Nodes, vehicle.Name)
                            Case VehicleType.Bike
                                TreeView3.SelectedNode = GetNodeItem(TreeView3.Nodes(2).Nodes, vehicle.Name)
                            Case VehicleType.Convertible
                                TreeView3.SelectedNode = GetNodeItem(TreeView3.Nodes(3).Nodes, vehicle.Name)
                            Case VehicleType.Industrial
                                TreeView3.SelectedNode = GetNodeItem(TreeView3.Nodes(4).Nodes, vehicle.Name)
                            Case VehicleType.Lowriders
                                TreeView3.SelectedNode = GetNodeItem(TreeView3.Nodes(5).Nodes, vehicle.Name)
                            Case VehicleType.Off_Road
                                TreeView3.SelectedNode = GetNodeItem(TreeView3.Nodes(6).Nodes, vehicle.Name)
                            Case VehicleType.Public_Service
                                TreeView3.SelectedNode = GetNodeItem(TreeView3.Nodes(7).Nodes, vehicle.Name)
                            Case VehicleType.Saloon
                                TreeView3.SelectedNode = GetNodeItem(TreeView3.Nodes(8).Nodes, vehicle.Name)
                            Case VehicleType.Sport
                                TreeView3.SelectedNode = GetNodeItem(TreeView3.Nodes(9).Nodes, vehicle.Name)
                            Case VehicleType.Station_Wagon
                                TreeView3.SelectedNode = GetNodeItem(TreeView3.Nodes(10).Nodes, vehicle.Name)
                            Case VehicleType.Boat
                                TreeView3.SelectedNode = GetNodeItem(TreeView3.Nodes(11).Nodes, vehicle.Name)
                            Case VehicleType.Trailer
                                TreeView3.SelectedNode = GetNodeItem(TreeView3.Nodes(12).Nodes, vehicle.Name)
                            Case VehicleType.Unique
                                TreeView3.SelectedNode = GetNodeItem(TreeView3.Nodes(13).Nodes, vehicle.Name)
                            Case VehicleType.RC
                                TreeView3.SelectedNode = GetNodeItem(TreeView3.Nodes(14).Nodes, vehicle.Name)
                        End Select
                        found = True
                        VehPos += 1
                        Exit For
                    Else
                        count += 1
                    End If
                End If
            Next
            If found = False Then
                VehPos = 0
                For Each vehicle In Vehicles
                    If vehicle.Name.IndexOf(TextBox53.Text) >= 0 Then
                        Select Case vehicle.Type
                            Case VehicleType.Airplane
                                TreeView3.SelectedNode = GetNodeItem(TreeView3.Nodes(0).Nodes, vehicle.Name)
                            Case VehicleType.Helicopter
                                TreeView3.SelectedNode = GetNodeItem(TreeView3.Nodes(1).Nodes, vehicle.Name)
                            Case VehicleType.Bike
                                TreeView3.SelectedNode = GetNodeItem(TreeView3.Nodes(2).Nodes, vehicle.Name)
                            Case VehicleType.Convertible
                                TreeView3.SelectedNode = GetNodeItem(TreeView3.Nodes(3).Nodes, vehicle.Name)
                            Case VehicleType.Industrial
                                TreeView3.SelectedNode = GetNodeItem(TreeView3.Nodes(4).Nodes, vehicle.Name)
                            Case VehicleType.Lowriders
                                TreeView3.SelectedNode = GetNodeItem(TreeView3.Nodes(5).Nodes, vehicle.Name)
                            Case VehicleType.Off_Road
                                TreeView3.SelectedNode = GetNodeItem(TreeView3.Nodes(6).Nodes, vehicle.Name)
                            Case VehicleType.Public_Service
                                TreeView3.SelectedNode = GetNodeItem(TreeView3.Nodes(7).Nodes, vehicle.Name)
                            Case VehicleType.Saloon
                                TreeView3.SelectedNode = GetNodeItem(TreeView3.Nodes(8).Nodes, vehicle.Name)
                            Case VehicleType.Sport
                                TreeView3.SelectedNode = GetNodeItem(TreeView3.Nodes(9).Nodes, vehicle.Name)
                            Case VehicleType.Station_Wagon
                                TreeView3.SelectedNode = GetNodeItem(TreeView3.Nodes(10).Nodes, vehicle.Name)
                            Case VehicleType.Boat
                                TreeView3.SelectedNode = GetNodeItem(TreeView3.Nodes(11).Nodes, vehicle.Name)
                            Case VehicleType.Trailer
                                TreeView3.SelectedNode = GetNodeItem(TreeView3.Nodes(12).Nodes, vehicle.Name)
                            Case VehicleType.Unique
                                TreeView3.SelectedNode = GetNodeItem(TreeView3.Nodes(13).Nodes, vehicle.Name)
                            Case VehicleType.RC
                                TreeView3.SelectedNode = GetNodeItem(TreeView3.Nodes(14).Nodes, vehicle.Name)
                        End Select
                        found = True
                        VehPos += 1
                        Exit For
                    End If
                Next
                If found = False Then
                    Select Case Config.Idioma
                        Case Lang.English
                            MsgBox("Vehicle not found.", MsgBoxStyle.Critical, "Error")
                        Case Lang.Español
                            MsgBox("Vehiculo no encontrado.", MsgBoxStyle.Critical, "Error")
                        Case Lang.Portugues
                            MsgBox("Veículo não encontrado.", MsgBoxStyle.Critical, "Error")
                        Case Else
                            MsgBox("Vehicle nicht gefunden.", MsgBoxStyle.Critical, "Error")
                    End Select
                    Exit Sub
                End If
            End If
        End If
        TreeView3.Select()
    End Sub

#End Region

#End Region

#Region "Sounds"

#Region "Node Selection"

    Private Sub TreeView4_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles TreeView4.AfterSelect
        TextBox57.Text = Sounds(TreeView4.SelectedNode.Text)
    End Sub

#End Region

#End Region

#Region "Weapons"

#Region "Display"

    Private Sub TreeView6_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles TreeView6.AfterSelect
        Dim tmp As Boolean
        If Config.Images = True Or Config.URL_Weap = "" Then
            tmp = True
        Else
            tmp = False
        End If
        If tmp = False Then
            Try
                PictureBox5.Image = LoadImageFromURL(String.Format(Config.URL_Weap, TreeView7.SelectedNode.Text))
            Catch ex As Exception
                PictureBox1.Image = My.Resources.N_A
            End Try
        End If
        For Each weapon As Weap In Weapons
            If weapon.Name = TreeView6.SelectedNode.Text Then
                If tmp = True Then
                    Try
                        PictureBox4.Image = Image.FromFile(My.Application.Info.DirectoryPath & "\Resources\Weapon_" & weapon.ID & ".png")
                    Catch ex As Exception
                        PictureBox1.Image = My.Resources.N_A
                    End Try
                End If
                TextBox64.Text = weapon.ID
                TextBox65.Text = weapon.Name
                TextBox66.Text = weapon.Slot
                TextBox67.Text = weapon.Type.ToString
                TextBox74.Text = weapon.Def
            End If
        Next
    End Sub

#End Region

#End Region

#Region "Map Icons"

#Region "Display"

    Private Sub TreeView7_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles TreeView7.AfterSelect
        Dim tmp As Boolean
        If Config.Images = True Or Config.URL_Map = "" Then
            tmp = True
        Else
            tmp = False
        End If
        If tmp = False Then
            Try
                PictureBox5.Image = LoadImageFromURL(String.Format(Config.URL_Map, TreeView7.SelectedNode.Text))
            Catch ex As Exception
                PictureBox1.Image = My.Resources.N_A
            End Try
        End If
        For Each map In Maps
            If map.ID = TreeView7.SelectedNode.Text Then
                If tmp = True Then
                    Try
                        PictureBox5.Image = Image.FromFile(My.Application.Info.DirectoryPath & "\Resources\MapIcon_" & map.ID & ".gif")
                    Catch ex As Exception
                        PictureBox1.Image = My.Resources.N_A
                    End Try
                End If
                TextBox72.Text = map.Name
                TextBox73.Text = map.ID
                Exit For
            End If
        Next
    End Sub

#End Region

#End Region

#Region "Sprites"

#Region "Display"

    Private Sub TreeView8_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles TreeView8.AfterSelect
        Dim tmp As Boolean
        If Config.Images = True Or Config.URL_Map = "" Then
            tmp = True
        Else
            tmp = False
        End If
        If tmp = False Then
            Try
                PictureBox6.Image = LoadImageFromURL(String.Format(Config.URL_Sprite, TreeView7.SelectedNode.Text))
            Catch ex As Exception
                PictureBox1.Image = My.Resources.N_A
            End Try
        End If
        Dim count As Integer
        For Each sprite In Sprites
            If TreeView8.SelectedNode.Text = sprite.Name Then
                If tmp = True Then
                    Try
                        PictureBox6.Image = Image.FromFile(My.Application.Info.DirectoryPath & "\Resources\Sprite_" & count & ".bmp")
                    Catch ex As Exception
                        PictureBox1.Image = My.Resources.N_A
                    End Try
                End If
                TextBox88.Text = sprite.Name
                TextBox89.Text = sprite.Path
                TextBox90.Text = sprite.File
                TextBox91.Text = sprite.Size
            End If
            count += 1
        Next
    End Sub

#End Region

#Region "Find"

    Private Sub Button19_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button19.Click
        If TextBox94.Text.Length = 0 Then
            Select Case Config.Idioma
                Case Lang.English
                    MsgBox("Invalid name.", MsgBoxStyle.Critical, "Error")
                Case Lang.Español
                    MsgBox("Nombre inválido.", MsgBoxStyle.Critical, "Error")
                Case Lang.Portugues
                    MsgBox("Nombre inválido.", MsgBoxStyle.Critical, "Error")
                Case Else
                    MsgBox("ungültigen Namen.", MsgBoxStyle.Critical, "Error")
            End Select
            Exit Sub
        End If
        Dim found As Boolean, count As Integer
        found = False
        For Each sprite In Sprites
            If sprite.Name.IndexOf(TextBox94.Text) >= 0 Then
                If count = SprPos Then
                    Select Case sprite.File
                        Case "samaps"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(0).Nodes, sprite.Name)
                        Case "LD_BUM"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(0).Nodes, sprite.Name)
                        Case "intro1"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(1).Nodes, sprite.Name)
                        Case "intro2"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(2).Nodes, sprite.Name)
                        Case "INTRO3"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(3).Nodes, sprite.Name)
                        Case "intro4"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(4).Nodes, sprite.Name)
                        Case "LD_BEAT"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(5).Nodes, sprite.Name)
                        Case "LD_CARD"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(6).Nodes, sprite.Name)
                        Case "LD_CHAT"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(7).Nodes, sprite.Name)
                        Case "LD_DRV"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(8).Nodes, sprite.Name)
                        Case "LD_DUAL"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(9).Nodes, sprite.Name)
                        Case "ld_grav"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(10).Nodes, sprite.Name)
                        Case "LD_NONE"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(11).Nodes, sprite.Name)
                        Case "OTB"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(12).Nodes, sprite.Name)
                        Case "OTB2"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(13).Nodes, sprite.Name)
                        Case "LD_PLAN"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(14).Nodes, sprite.Name)
                        Case "LD_POKE"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(15).Nodes, sprite.Name)
                        Case "LD_POOL"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(16).Nodes, sprite.Name)
                        Case "LD_RACE"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(17).Nodes, sprite.Name)
                        Case "LD_RACE1"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(18).Nodes, sprite.Name)
                        Case "LD_RACE2"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(19).Nodes, sprite.Name)
                        Case "LD_RACE3"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(20).Nodes, sprite.Name)
                        Case "LD_RACE4"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(21).Nodes, sprite.Name)
                        Case "LD_RACE5"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(22).Nodes, sprite.Name)
                        Case "LD_ROUL"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(23).Nodes, sprite.Name)
                        Case "ld_shtr"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(24).Nodes, sprite.Name)
                        Case "LD_SLOT"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(25).Nodes, sprite.Name)
                        Case "LD_SPAC"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(26).Nodes, sprite.Name)
                        Case "LD_TATT"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(27).Nodes, sprite.Name)
                        Case "load0uk"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(28).Nodes, sprite.Name)
                        Case "loadsc0"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(29).Nodes, sprite.Name)
                        Case "loadsc1"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(30).Nodes, sprite.Name)
                        Case "loadsc2"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(31).Nodes, sprite.Name)
                        Case "loadsc3"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(32).Nodes, sprite.Name)
                        Case "loadsc4"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(33).Nodes, sprite.Name)
                        Case "loadsc5"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(34).Nodes, sprite.Name)
                        Case "loadsc6"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(35).Nodes, sprite.Name)
                        Case "loadsc7"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(36).Nodes, sprite.Name)
                        Case "loadsc8"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(37).Nodes, sprite.Name)
                        Case "loadsc9"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(38).Nodes, sprite.Name)
                        Case "loadsc10"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(39).Nodes, sprite.Name)
                        Case "loadsc11"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(40).Nodes, sprite.Name)
                        Case "loadsc12"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(41).Nodes, sprite.Name)
                        Case "loadsc13"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(42).Nodes, sprite.Name)
                        Case "loadsc14"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(43).Nodes, sprite.Name)
                        Case "LOADSCS"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(44).Nodes, sprite.Name)
                        Case "LOADSUK"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(45).Nodes, sprite.Name)
                        Case "outro"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(46).Nodes, sprite.Name)
                        Case "splash1"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(47).Nodes, sprite.Name)
                        Case "splash2"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(48).Nodes, sprite.Name)
                        Case "splash3"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(49).Nodes, sprite.Name)
                    End Select
                    found = True
                    SprPos += 1
                    Exit For
                Else
                    count += 1
                End If
            End If
        Next
        If found = False Then
            SprPos = 0
            For Each sprite In Sprites
                If sprite.Name.IndexOf(TextBox94.Text) >= 0 Then
                    Select Case sprite.File
                        Case "samaps"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(0).Nodes, sprite.Name)
                        Case "LD_BUM"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(0).Nodes, sprite.Name)
                        Case "intro1"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(1).Nodes, sprite.Name)
                        Case "intro2"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(2).Nodes, sprite.Name)
                        Case "INTRO3"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(3).Nodes, sprite.Name)
                        Case "intro4"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(4).Nodes, sprite.Name)
                        Case "LD_BEAT"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(5).Nodes, sprite.Name)
                        Case "LD_CARD"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(6).Nodes, sprite.Name)
                        Case "LD_CHAT"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(7).Nodes, sprite.Name)
                        Case "LD_DRV"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(8).Nodes, sprite.Name)
                        Case "LD_DUAL"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(9).Nodes, sprite.Name)
                        Case "ld_grav"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(10).Nodes, sprite.Name)
                        Case "LD_NONE"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(11).Nodes, sprite.Name)
                        Case "OTB"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(12).Nodes, sprite.Name)
                        Case "OTB2"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(13).Nodes, sprite.Name)
                        Case "LD_PLAN"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(14).Nodes, sprite.Name)
                        Case "LD_POKE"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(15).Nodes, sprite.Name)
                        Case "LD_POOL"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(16).Nodes, sprite.Name)
                        Case "LD_RACE"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(17).Nodes, sprite.Name)
                        Case "LD_RACE1"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(18).Nodes, sprite.Name)
                        Case "LD_RACE2"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(19).Nodes, sprite.Name)
                        Case "LD_RACE3"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(20).Nodes, sprite.Name)
                        Case "LD_RACE4"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(21).Nodes, sprite.Name)
                        Case "LD_RACE5"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(22).Nodes, sprite.Name)
                        Case "LD_ROUL"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(23).Nodes, sprite.Name)
                        Case "ld_shtr"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(24).Nodes, sprite.Name)
                        Case "LD_SLOT"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(25).Nodes, sprite.Name)
                        Case "LD_SPAC"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(26).Nodes, sprite.Name)
                        Case "LD_TATT"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(27).Nodes, sprite.Name)
                        Case "load0uk"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(28).Nodes, sprite.Name)
                        Case "loadsc0"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(29).Nodes, sprite.Name)
                        Case "loadsc1"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(30).Nodes, sprite.Name)
                        Case "loadsc2"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(31).Nodes, sprite.Name)
                        Case "loadsc3"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(32).Nodes, sprite.Name)
                        Case "loadsc4"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(33).Nodes, sprite.Name)
                        Case "loadsc5"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(34).Nodes, sprite.Name)
                        Case "loadsc6"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(35).Nodes, sprite.Name)
                        Case "loadsc7"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(36).Nodes, sprite.Name)
                        Case "loadsc8"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(37).Nodes, sprite.Name)
                        Case "loadsc9"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(38).Nodes, sprite.Name)
                        Case "loadsc10"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(39).Nodes, sprite.Name)
                        Case "loadsc11"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(40).Nodes, sprite.Name)
                        Case "loadsc12"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(41).Nodes, sprite.Name)
                        Case "loadsc13"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(42).Nodes, sprite.Name)
                        Case "loadsc14"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(43).Nodes, sprite.Name)
                        Case "LOADSCS"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(44).Nodes, sprite.Name)
                        Case "LOADSUK"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(45).Nodes, sprite.Name)
                        Case "outro"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(46).Nodes, sprite.Name)
                        Case "splash1"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(47).Nodes, sprite.Name)
                        Case "splash2"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(48).Nodes, sprite.Name)
                        Case "splash3"
                            TreeView8.SelectedNode = GetNodeItem(TreeView8.Nodes(1).Nodes(49).Nodes, sprite.Name)
                    End Select
                    found = True
                    SprPos += 1
                    Exit For
                End If
            Next
            If found = False Then
                Select Case Config.Idioma
                    Case Lang.English
                        MsgBox("Sprite not found.", MsgBoxStyle.Critical, "Error")
                    Case Lang.Español
                        MsgBox("Sprite no encontrado.", MsgBoxStyle.Critical, "Error")
                    Case Lang.Portugues
                        MsgBox("Sprite não encontrado.", MsgBoxStyle.Critical, "Error")
                    Case Else
                        MsgBox("Sprite nicht gefunden.", MsgBoxStyle.Critical, "Error")
                End Select
                Exit Sub
            End If
        End If
        TreeView8.Select()
    End Sub

#End Region

#End Region

#End Region

#Region "Config"

#Region "Visual"

    Private Sub CheckBox7_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox7.CheckedChanged
        If CheckBox7.Checked = True Then
            GroupBox11.Enabled = False
            GroupBox13.Enabled = False
            GroupBox14.Enabled = False
            GroupBox17.Enabled = False
            GroupBox16.Enabled = False
        Else
            GroupBox11.Enabled = True
            GroupBox13.Enabled = True
            GroupBox14.Enabled = True
            GroupBox17.Enabled = True
            GroupBox16.Enabled = True
        End If
    End Sub

#End Region

#Region "Languaje"

    Private Sub RadioButton14_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton14.CheckedChanged
        If RadioButton14.Checked = True Then
            ChangeLang(Lang.English)
        End If
    End Sub

    Private Sub RadioButton15_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton15.CheckedChanged
        If RadioButton15.Checked = True Then
            ChangeLang(Lang.Español)
        End If
    End Sub

    Private Sub RadioButton16_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton16.CheckedChanged
        If RadioButton16.Checked = True Then
            ChangeLang(Lang.Portugues)
        End If
    End Sub

    Private Sub RadioButton17_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton17.CheckedChanged
        If RadioButton17.Checked = True Then
            ChangeLang(Lang.Deutsch)
        End If
    End Sub

#End Region

#Region "Buttons"

    Private Sub Button10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button10.Click
        ChangeLang(Config.Idioma)
        TextBox50.Text = Config.URL_Skin
        TextBox51.Text = Config.URL_Veh
        TextBox68.Text = Config.URL_Weap
        TextBox75.Text = Config.URL_Map
        TextBox92.Text = Config.URL_Sprite
        Select Case Config.Idioma
            Case Lang.English
                RadioButton14.Checked = True
            Case Lang.Español
                RadioButton15.Checked = True
            Case Lang.Portugues
                RadioButton16.Checked = True
            Case Lang.Deutsch
                RadioButton17.Checked = True
        End Select
        CheckBox7.Checked = Config.Images
        TextBox63.Text = Config.ZoneCreate
        TextBox76.Text = Config.ZoneShow
        TextBox93.Text = Config.Bounds
    End Sub

    Private Sub Button11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button11.Click
        If RadioButton14.Checked = True Then
            Config.Idioma = Lang.English
        ElseIf RadioButton15.Checked = True Then
            Config.Idioma = Lang.Español
        ElseIf RadioButton16.Checked = True Then
            Config.Idioma = Lang.Portugues
        Else
            Config.Idioma = Lang.Deutsch
        End If
        ChangeLang(Config.Idioma)
        If CheckBox7.Checked = True Then
            Config.Images = True
        Else
            Config.Images = False
            Config.URL_Skin = TextBox50.Text
            Config.URL_Veh = TextBox51.Text
            Config.URL_Weap = TextBox68.Text
            Config.URL_Map = TextBox75.Text
            Config.URL_Sprite = TextBox92.Text
        End If
        Config.ZoneCreate = TextBox63.Text
        Config.ZoneShow = TextBox76.Text
        Config.Bounds = TextBox93.Text
    End Sub

    Private Sub Button13_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button13.Click
        CheckBox7.Checked = True
        RadioButton14.Checked = True
        TextBox63.Text = "GangZoneCreate({0}, {1}, {2}, {3});"
        TextBox50.Text = "http://www.gamerxserver.com/images/Skins/Thumbnails/{0}.jpg"
        TextBox51.Text = ""
        TextBox68.Text = ""
        TextBox75.Text = ""
        TextBox76.Text = "GangZoneShowForAll({5}, {6});"
        TextBox92.Text = ""
        TextBox93.Text = "SetPlayerWorldBounds(playerid, {0}, {1}, {2}, {3});"
    End Sub

#End Region

#Region "Extra"

    Private Sub TabPage12_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles TabPage12.Leave
        ChangeLang(Config.Idioma)
        TextBox50.Text = Config.URL_Skin
        TextBox51.Text = Config.URL_Veh
        TextBox68.Text = Config.URL_Weap
        TextBox75.Text = Config.URL_Map
        Select Case Config.Idioma
            Case Lang.English
                RadioButton14.Checked = True
            Case Lang.Español
                RadioButton15.Checked = True
            Case Lang.Portugues
                RadioButton16.Checked = True
        End Select
        CheckBox7.Checked = Config.Images
        TextBox63.Text = Config.ZoneCreate
    End Sub

#End Region

#End Region

#End Region

#Region "Menu"

#Region "File"

    Private Sub CloseToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CloseToolStripMenuItem.Click
        Application.Exit()
    End Sub

#End Region

#Region "Other"

    Private Sub MainPageToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MainPageToolStripMenuItem.Click
        Process.Start("http://wiki.sa-mp.com")
    End Sub

    Private Sub SearchForToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SearchForToolStripMenuItem.Click
        Srch.Show()
        Srch.Top = Me.Top + 50
        Srch.Left = Me.Left + 20
    End Sub

    Private Sub WebPageToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles WebPageToolStripMenuItem.Click
        Diagnostics.Process.Start("http://sa-mp.com")
    End Sub

    Private Sub ForumToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ForumToolStripMenuItem.Click
        Diagnostics.Process.Start("http://forum.sa-mp.com")
    End Sub

    Private Sub CreditsToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CreditsToolStripMenuItem1.Click
        Creds.Show()
        Creds.Top = Me.Top + 130
        Creds.Left = Me.Left + 100
    End Sub

#End Region

#End Region

End Class
