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

'Arreglar el cache que funciona como el culo

Imports System.Text.RegularExpressions
Imports System.Drawing
Imports System.Math

Public Class Tools

#Region "Enums"

    Private Enum GuiImage
        Button
        Scroll
        TextBox
        Dot
    End Enum

    Private Enum ConvertType
        Obj
        Veh
        None
    End Enum

    Private Enum ImageTypes
        MapIcon
        Skin
        Sprite
        Vehicle
        Weapon
    End Enum

#End Region

#Region "Structures"

    Public Structure Area
        Public minX As Integer
        Public minY As Integer
        Public maxX As Integer
        Public maxY As Integer
        Public xForward As Boolean
        Public yForward As Boolean
        Public Lock As Boolean
    End Structure

    Public Structure ObjectInfo
        Public Model As Integer
        Public X As Single
        Public Y As Single
        Public Z As Single
        Public rX As Single
        Public rY As Single
        Public rZ As Single
        Public Interior As Integer
    End Structure

    Public Structure VehicleInfo
        Public Model As Integer
        Public X As Single
        Public Y As Single
        Public Z As Single
        Public R As Single
        Public Color1 As Integer
        Public Color2 As Integer
    End Structure

    Public Structure CacheImage
        Public ID As Integer
        Public Img As Bitmap
    End Structure

#End Region

#Region "Me"

    Private Sub Tools_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        OFD.Filter = "Script & map files|*.pwn;*.inc;*.map|Pawn files (.pwn)|*.pwn|Include files (.inc)|*.inc|Map files (.map)|*.map|All files| *.*"

        ComboBox1.Items.Add("DIALOG_STYLE_MSGBOX")
        ComboBox1.Items.Add("DIALOG_STYLE_INPUT")
        ComboBox1.Items.Add("DIALOG_STYLE_LIST")
        ComboBox1.Items.Add("DIALOG_STYLE_PASSWORD")
        ComboBox1.SelectedIndex = 0

        ComboBox3.Items.Add("SA:MP")
        ComboBox3.Items.Add("MTA Race")
        ComboBox3.Items.Add("MTA 1.1")
        ComboBox3.Items.Add("Incognito's Streamer Plugin (ObjectInfo Only)")
        ComboBox3.Items.Add("YSI DynamicObject (ObjectInfo Only)")
        ComboBox3.Items.Add("xStreamer (ObjectInfo Only)")
        ComboBox3.Items.Add("MidoStream ObjectInfo (ObjectInfo Only)")
        ComboBox3.Items.Add("Doble-O ObjectInfo (ObjectInfo Only)")
        ComboBox3.Items.Add("Fallout's Object Streamer (ObjectInfo Only)")
        ComboBox3.SelectedIndex = 2
        ComboBox4.Items.Add("SA:MP")
        ComboBox4.Items.Add("Incognito's Streamer Plugin (ObjectInfo Only)")
        ComboBox4.Items.Add("YSI DynamicObject (ObjectInfo Only)")
        ComboBox4.Items.Add("xStreamer (ObjectInfo Only)")
        ComboBox4.Items.Add("MidoStream ObjectInfo (ObjectInfo Only)")
        ComboBox4.Items.Add("Doble-O ObjectInfo (ObjectInfo Only)")
        ComboBox4.Items.Add("Fallout's Object Streamer (ObjectInfo Only)")
        ComboBox4.SelectedIndex = 0

        PictureBox1.Image = GetImageFromGui(GuiImage.Button)
        PictureBox3.Image = GetImageFromGui(GuiImage.Scroll)
        PictureBox2.Image = GetImageFromGui(GuiImage.Button)
        PictureBox4.Image = GetImageFromGui(GuiImage.TextBox)
        PictureBox5.Image = GetImageFromGui(GuiImage.Dot)
        RichTextBox2.ForeColor = Color.FromArgb(255, 160, 160, 160)

        PictureBox6.Image = My.Resources.Map
        Panel6.BackColor = Settings.C_Area.Hex
        LoadResources(True)
    End Sub

#End Region

#Region "Tabs"

#Region "Teleports"

#Region "Texts Restrictions"

    Private Sub textbox39_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox39.KeyPress
        If Not IsNumeric(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) AndAlso e.KeyChar <> "." AndAlso e.KeyChar <> "-" AndAlso Asc(e.KeyChar) <> 8 Then e.Handled = True
        If e.KeyChar = "." And TextBox39.Text.LastIndexOf(".") <> -1 Then e.Handled = True
    End Sub

    Private Sub textbox38_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox38.KeyPress
        If Not IsNumeric(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) AndAlso e.KeyChar <> "." AndAlso e.KeyChar <> "-" AndAlso Asc(e.KeyChar) <> 8 Then e.Handled = True
        If e.KeyChar = "." And TextBox38.Text.LastIndexOf(".") <> -1 Then e.Handled = True
    End Sub

    Private Sub textbox34_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox34.KeyPress
        If Not IsNumeric(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) AndAlso e.KeyChar <> "." AndAlso e.KeyChar <> "-" AndAlso Asc(e.KeyChar) <> 8 Then e.Handled = True
        If e.KeyChar = "." And TextBox34.Text.LastIndexOf(".") <> -1 Then e.Handled = True
    End Sub

    Private Sub TextBox6_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox35.KeyPress
        If Not IsNumeric(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) AndAlso Asc(e.KeyChar) <> 8 Then e.Handled = True
    End Sub

    Private Sub textbox36_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox36.KeyPress
        If Not IsNumeric(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) AndAlso Asc(e.KeyChar) <> 8 Then e.Handled = True
    End Sub

    Private Sub textbox37_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox37.KeyPress
        If Not IsNumeric(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) AndAlso e.KeyChar <> "." AndAlso Asc(e.KeyChar) <> 8 Then e.Handled = True
        If e.KeyChar = "." AndAlso TextBox37.Text.LastIndexOf(".") <> -1 Then e.Handled = True
    End Sub

    Private Sub textbox39_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox39.TextChanged
        TextBox39.Text = Regex.Replace(TextBox39.Text, BadChars, "")
    End Sub

    Private Sub textbox38_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox38.TextChanged
        TextBox38.Text = Regex.Replace(TextBox38.Text, BadChars, "")
    End Sub

    Private Sub textbox34_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox34.TextChanged
        TextBox34.Text = Regex.Replace(TextBox34.Text, BadChars, "")
    End Sub

    Private Sub TextBox6_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox35.TextChanged
        TextBox35.Text = Regex.Replace(TextBox35.Text, BadChars, "")
    End Sub

    Private Sub textbox36_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox36.TextChanged
        TextBox36.Text = Regex.Replace(TextBox36.Text, BadChars, "")
    End Sub

    Private Sub textbox37_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox37.TextChanged
        TextBox37.Text = Regex.Replace(TextBox37.Text, BadChars, "")
    End Sub

#End Region

#Region "Message(Optional&Help)"

    Private Sub Button8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button8.Click
        gSender = CC.Msg
        eColor.TrackBar4.Enabled = True
        eColor.TextBox4.Enabled = True
        eColor.Show()
        eColor.Focus()
        eColor.Panel1.BackColor = Settings.C_Msg.Hex
    End Sub

    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button7.Click
        gSender = CC.Help
        eColor.TrackBar4.Enabled = True
        eColor.TextBox4.Enabled = True
        eColor.Show()
        eColor.Focus()
        eColor.Panel1.BackColor = Settings.C_Help.Hex
    End Sub

#End Region

#Region "Visual"

    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked = True Then
            Panel4.BackColor = Settings.C_Msg.Hex
            Panel4.Visible = True
            Button2.Visible = True
            Label3.Visible = True
            TextBox33.Visible = True
        Else
            Panel4.Visible = False
            Button2.Visible = False
            Label3.Visible = False
            TextBox33.Visible = False
        End If
    End Sub

    Private Sub RadioButton6_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton6.CheckedChanged
        If RadioButton6.Checked = True Then
            Panel3.BackColor = Settings.C_Help.Hex
            Panel3.Visible = True
            Button7.Visible = True
            Label22.Visible = True
            Label23.Visible = True
            TextBox32.Visible = True
        Else
            Panel3.Visible = False
            Button7.Visible = False
            Label22.Visible = False
            Label23.Visible = False
            TextBox32.Visible = False
        End If
    End Sub

#End Region

#Region "Generate Code"

    Private Sub Button9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button9.Click
        TextBox40.Clear()
        If Not TextBox31.Text.Length > 0 Then
            Select Case Settings.Language
                Case Languages.English
                    MsgBox("You must enter a name for the command.", MsgBoxStyle.Critical, "Error")
                Case Languages.Español
                    MsgBox("Debes ingresar un nombre para el comando.", MsgBoxStyle.Critical, "Error")
                Case Languages.Portuguêse
                    MsgBox("Você deve digitar um nome para o comando.", MsgBoxStyle.Critical, "Error")
                Case Else
                    MsgBox("Du musst einen Namen für den Befehl eingeben.", MsgBoxStyle.Critical, "Fehler")
            End Select
            Exit Sub
        End If
        If CheckBox1.Checked Then
            If Not TextBox33.Text.Length > 0 Then
                Select Case Settings.Language
                    Case Languages.English
                        MsgBox("You must enter a message to send.", MsgBoxStyle.Critical, "Error")
                    Case Languages.Español
                        MsgBox("Debes ingresar un mensaje para enviar.", MsgBoxStyle.Critical, "Error")
                    Case Languages.Portuguêse
                        MsgBox("Você deve digitar uma mensagem para enviar.", MsgBoxStyle.Critical, "Error")
                    Case Else
                        MsgBox("Du musst eine Nachricht zum senden eingeben.", MsgBoxStyle.Critical, "Fehler")
                End Select
                TextBox33.Focus()
                Exit Sub
            End If
        End If
        If Not TextBox39.Text.Length > 0 Then
            Select Case Settings.Language
                Case Languages.English
                    MsgBox("You must enter a X coordinate.", MsgBoxStyle.Critical, "Error")
                Case Languages.Español
                    MsgBox("Debes ingresar una coordenada X.", MsgBoxStyle.Critical, "Error")
                Case Languages.Portuguêse
                    MsgBox("Você deve digitar uma coordenada X.", MsgBoxStyle.Critical, "Error")
                Case Else
                    MsgBox("Du musst eine X-Koordinate eingeben.", MsgBoxStyle.Critical, "Fehler")
            End Select
            Exit Sub
        End If
        If Not TextBox38.Text.Length > 0 Then
            Select Case Settings.Language
                Case Languages.English
                    MsgBox("You must enter a Y coordinate.", MsgBoxStyle.Critical, "Error")
                Case Languages.Español
                    MsgBox("Debes ingresar una coordenada Y.", MsgBoxStyle.Critical, "Error")
                Case Languages.Portuguêse
                    MsgBox("Você deve digitar uma coordenada Y.", MsgBoxStyle.Critical, "Error")
                Case Else
                    MsgBox("Du musst eine Y-Koordinate eingeben.", MsgBoxStyle.Critical, "Fehler")
            End Select
            Exit Sub
        End If
        If Not TextBox34.Text.Length > 0 Then
            Select Case Settings.Language
                Case Languages.English
                    MsgBox("You must enter a Z coordinate.", MsgBoxStyle.Critical, "Error")
                Case Languages.Español
                    MsgBox("Debes ingresar una coordenada Z.", MsgBoxStyle.Critical, "Error")
                Case Languages.Portuguêse
                    MsgBox("Você deve digitar uma coordenada Z.", MsgBoxStyle.Critical, "Error")
                Case Else
                    MsgBox("Du musst eine Z-Koordinate eingeben.", MsgBoxStyle.Critical, "Fehler")
            End Select
            Exit Sub
        End If
        If Not TextBox35.Text.Length > 0 Then
            Select Case Settings.Language
                Case Languages.English
                    MsgBox("You must enter a world ID.", MsgBoxStyle.Critical, "Error")
                Case Languages.Español
                    MsgBox("Debes ingresar el id de un mundo", MsgBoxStyle.Critical, "Error")
                Case Languages.Portuguêse
                    MsgBox("Você deve digitar um ID mundo.", MsgBoxStyle.Critical, "Error")
                Case Else
                    MsgBox("Du musst eine Welt ID eingeben.", MsgBoxStyle.Critical, "Fehler")
            End Select
            Exit Sub
        End If
        If Not TextBox36.Text.Length > 0 Then
            Select Case Settings.Language
                Case Languages.English
                    MsgBox("You must enter an interior ID.", MsgBoxStyle.Critical, "Error")
                Case Languages.Español
                    MsgBox("Debes ingresar el id de un interior.", MsgBoxStyle.Critical, "Error")
                Case Languages.Portuguêse
                    MsgBox("Você deve digitar um ID interior.", MsgBoxStyle.Critical, "Error")
                Case Else
                    MsgBox("Du musst eine Interior ID eingeben.", MsgBoxStyle.Critical, "Fehler")
            End Select
            Exit Sub
        End If
        If Not TextBox37.Text.Length > 0 Then
            Select Case Settings.Language
                Case Languages.English
                    MsgBox("You must enter an angle.", MsgBoxStyle.Critical, "Error")
                Case Languages.Español
                    MsgBox("Debes ingresar un angulo.", MsgBoxStyle.Critical, "Error")
                Case Languages.Portuguêse
                    MsgBox("Você deve digitar um ângulo.", MsgBoxStyle.Critical, "Error")
                Case Else
                    MsgBox("Du musst einen Winkel angeben.", MsgBoxStyle.Critical, "Fehler")
            End Select
            Exit Sub
        End If
        If RadioButton9.Checked Then
            If RadioButton11.Checked Then
                TextBox40.Text = "if(!strcmp(cmdtext, ""/" & TextBox31.Text & """, true)){" & vbNewLine & _
                vbTab & "SetPlayerPos(playerid, " & TextBox39.Text & ", " & TextBox38.Text & ", " & TextBox34.Text & ");" & vbNewLine & _
                vbTab & "SetPlayerFacingAngle(playerid, " & TextBox37.Text & ");" & vbNewLine & _
                vbTab & "SetPlayerInterior(playerid, " & TextBox36.Text & ");" & vbNewLine & _
                vbTab & "SetPlayerVirtualWorld(playerid, " & TextBox35.Text & ");" & vbNewLine
                If CheckBox1.Checked Then
                    TextBox40.Text += vbTab & "return SendClientMessage(playerid, " & Settings.C_Msg.Name & ", """ & TextBox33.Text & """);" & vbNewLine & _
                    "}"
                Else
                    TextBox40.Text += vbTab & "return 1;" & vbNewLine & _
                    "}"
                End If
            Else
                TextBox40.Text = "if(!strcmp(cmdtext, ""/" & TextBox31.Text & """, true)){" & vbNewLine & _
                vbTab & "if(!IsPlayerInAnyVehicle(playerid)){" & vbNewLine & _
                vbTab & vbTab & "SetPlayerPos(playerid, " & TextBox39.Text & ", " & TextBox38.Text & ", " & TextBox34.Text & ");" & vbNewLine & _
                vbTab & vbTab & "SetPlayerFacingAngle(playerid, " & TextBox37.Text & ");" & vbNewLine & _
                vbTab & vbTab & "SetPlayerInterior(playerid, " & TextBox36.Text & ");" & vbNewLine & _
                vbTab & vbTab & "SetPlayerVirtualWorld(playerid, " & TextBox35.Text & ");" & vbNewLine & _
                vbTab & "}" & vbNewLine & _
                vbTab & "else{" & vbNewLine & _
                vbTab & vbTab & "new veh = GetPlayerVehicleID(playerid);" & vbNewLine & _
                vbTab & vbTab & "SetVehiclePos(veh, " & TextBox39.Text & ", " & TextBox38.Text & ", " & TextBox34.Text & ");" & vbNewLine & _
                vbTab & vbTab & "SetVehicleZAngle(veh, " & TextBox37.Text & ");" & vbNewLine & _
                vbTab & vbTab & "LinkVehicleToInterior(veh, " & TextBox36.Text & ");" & vbNewLine & _
                vbTab & vbTab & "SetVehicleVirtualWorld(veh, " & TextBox35.Text & ");" & vbNewLine & _
                vbTab & vbTab & "PutPlayerInVehicle(playerid, veh, 0);" & vbNewLine & _
                vbTab & "}" & vbNewLine
                If CheckBox1.Checked Then
                    TextBox40.Text += vbTab & "return SendClientMessage(playerid, " & Settings.C_Msg.Name & ", """ & TextBox33.Text & """);" & vbNewLine & _
                    "}"
                Else
                    TextBox40.Text += vbTab & "return 1;" & vbNewLine & _
                    "}"
                End If
            End If
        ElseIf RadioButton8.Checked Then
            If RadioButton11.Checked Then
                TextBox40.Text = "dcmd(" & TextBox31.Text & ", " & TextBox31.Text.Length & ", " & "cmdtext);" & vbNewLine & vbNewLine & _
                "dcmd_" & TextBox31.Text & "(playerid, params[])" & vbNewLine & _
                "{" & vbNewLine & _
                vbTab & "#pragma unused params" & vbNewLine & _
                vbTab & "SetPlayerPos(playerid, " & TextBox39.Text & ", " & TextBox38.Text & ", " & TextBox34.Text & ");" & vbNewLine & _
                vbTab & "SetPlayerFacingAngle(playerid, " & TextBox37.Text & ");" & vbNewLine & _
                vbTab & "SetPlayerInterior(playerid, " & TextBox36.Text & ");" & vbNewLine & _
                vbTab & "SetPlayerVirtualWorld(playerid, " & TextBox35.Text & ");" & vbNewLine
                If CheckBox1.Checked Then
                    TextBox40.Text += vbTab & "return SendClientMessage(playerid, " & Settings.C_Msg.Name & ", """ & TextBox33.Text & """);" & vbNewLine & _
                    "}"
                Else
                    TextBox40.Text += vbTab & "return 1;" & vbNewLine & _
                    "}"
                End If
            Else
                TextBox40.Text = "dcmd(" & TextBox31.Text & ", " & TextBox31.Text.Length & ", " & "cmdtext);" & vbNewLine & vbNewLine & _
                "dcmd_" & TextBox31.Text & "(playerid, params[])" & vbNewLine & _
                "{" & vbNewLine & _
                vbTab & "#pragma unused params" & vbNewLine & _
                vbTab & "if(!IsPlayerInAnyVehicle(playerid)){" & vbNewLine & _
                vbTab & vbTab & "SetPlayerPos(playerid, " & TextBox39.Text & ", " & TextBox38.Text & ", " & TextBox34.Text & ");" & vbNewLine & _
                vbTab & vbTab & "SetPlayerFacingAngle(playerid, " & TextBox37.Text & ");" & vbNewLine & _
                vbTab & vbTab & "SetPlayerInterior(playerid, " & TextBox36.Text & ");" & vbNewLine & _
                vbTab & vbTab & "SetPlayerVirtualWorld(playerid, " & TextBox35.Text & ");" & vbNewLine & _
                vbTab & "}" & vbNewLine & _
                vbTab & "else{" & vbNewLine & _
                vbTab & vbTab & "new veh = GetPlayerVehicleID(playerid);" & vbNewLine & _
                vbTab & vbTab & "SetVehiclePos(veh, " & TextBox39.Text & ", " & TextBox38.Text & ", " & TextBox34.Text & ");" & vbNewLine & _
                vbTab & vbTab & "SetVehicleZAngle(veh, " & TextBox37.Text & ");" & vbNewLine & _
                vbTab & vbTab & "LinkVehicleToInterior(veh, " & TextBox36.Text & ");" & vbNewLine & _
                vbTab & vbTab & "SetVehicleVirtualWorld(veh, " & TextBox35.Text & ");" & vbNewLine & _
                vbTab & vbTab & "PutPlayerInVehicle(playerid, veh, 0);" & vbNewLine & _
                vbTab & "}" & vbNewLine
                If CheckBox1.Checked Then
                    TextBox40.Text += vbTab & "return SendClientMessage(playerid, " & Settings.C_Msg.Name & ", """ & TextBox33.Text & """);" & vbNewLine & _
                    "}"
                Else
                    TextBox40.Text += vbTab & "return 1;" & vbNewLine & _
                    "}"
                End If
            End If
        ElseIf RadioButton7.Checked Then
            If RadioButton11.Checked Then
                TextBox40.Text = "CMD:" & TextBox31.Text & "(playerid, params[])" & vbNewLine & _
                "{" & vbNewLine & _
                vbTab & "SetPlayerPos(playerid, " & TextBox39.Text & ", " & TextBox38.Text & ", " & TextBox34.Text & ");" & vbNewLine & _
                vbTab & "SetPlayerFacingAngle(playerid, " & TextBox37.Text & ");" & vbNewLine & _
                vbTab & "SetPlayerInterior(playerid, " & TextBox36.Text & ");" & vbNewLine & _
                vbTab & "SetPlayerVirtualWorld(playerid, " & TextBox35.Text & ");" & vbNewLine
                If CheckBox1.Checked Then
                    TextBox40.Text += vbTab & "return SendClientMessage(playerid, " & Settings.C_Msg.Name & ", """ & TextBox33.Text & """);" & vbNewLine & _
                    "}"
                Else
                    TextBox40.Text += vbTab & "return 1;" & vbNewLine & _
                    "}"
                End If
            Else
                TextBox40.Text = "CMD:" & TextBox31.Text & "(playerid, params[])" & vbNewLine & _
                "{" & vbNewLine & _
                vbTab & "if(!IsPlayerInAnyVehicle(playerid)){" & vbNewLine & _
                vbTab & vbTab & "SetPlayerPos(playerid, " & TextBox39.Text & ", " & TextBox38.Text & ", " & TextBox34.Text & ");" & vbNewLine & _
                vbTab & vbTab & "SetPlayerFacingAngle(playerid, " & TextBox37.Text & ");" & vbNewLine & _
                vbTab & vbTab & "SetPlayerInterior(playerid, " & TextBox36.Text & ");" & vbNewLine & _
                vbTab & vbTab & "SetPlayerVirtualWorld(playerid, " & TextBox35.Text & ");" & vbNewLine & _
                vbTab & "}" & vbNewLine & _
                vbTab & "else{" & vbNewLine & _
                vbTab & vbTab & "new veh = GetPlayerVehicleID(playerid);" & vbNewLine & _
                vbTab & vbTab & "SetVehiclePos(veh, " & TextBox39.Text & ", " & TextBox38.Text & ", " & TextBox34.Text & ");" & vbNewLine & _
                vbTab & vbTab & "SetVehicleZAngle(veh, " & TextBox37.Text & ");" & vbNewLine & _
                vbTab & vbTab & "LinkVehicleToInterior(veh, " & TextBox36.Text & ");" & vbNewLine & _
                vbTab & vbTab & "SetVehicleVirtualWorld(veh, " & TextBox35.Text & ");" & vbNewLine & _
                vbTab & vbTab & "PutPlayerInVehicle(playerid, veh, 0);" & vbNewLine & _
                vbTab & "}" & vbNewLine
                If CheckBox1.Checked Then
                    TextBox40.Text += vbTab & "return SendClientMessage(playerid, " & Settings.C_Msg.Name & ", """ & TextBox33.Text & """);" & vbNewLine & _
                    "}"
                Else
                    TextBox40.Text += vbTab & "return 1;" & vbNewLine & _
                    "}"
                End If
            End If
        ElseIf RadioButton6.Checked Then
            If Not TextBox32.Text.Length > 0 Then
                Select Case Settings.Language
                    Case Languages.English
                        MsgBox("You must enter a help message to send.", MsgBoxStyle.Critical, "Error")
                    Case Languages.Español
                        MsgBox("Debes ingresar un mensaje de ayuda para enviar.", MsgBoxStyle.Critical, "Error")
                    Case Languages.Portuguêse
                        MsgBox("YVocê deve digitar uma mensagem de ajuda para enviar.", MsgBoxStyle.Critical, "Error")
                    Case Else
                        MsgBox("Du musst eine Hilfsnachricht zum senden angeben.", MsgBoxStyle.Critical, "Fehler")
                End Select
                TextBox33.Focus()
                Exit Sub
            End If
            If RadioButton11.Checked Then
                TextBox40.Text = "YCMD:" & TextBox31.Text & "(playerid, params[], help)" & vbNewLine & _
                "{" & vbNewLine & _
                vbTab & "if(help) return SendClientMessage(playerid, " & Settings.C_Help.Name & ", """ & TextBox32.Text & """);" & vbNewLine & _
                vbTab & "SetPlayerPos(playerid, " & TextBox39.Text & ", " & TextBox38.Text & ", " & TextBox34.Text & ");" & vbNewLine & _
                vbTab & "SetPlayerFacingAngle(playerid, " & TextBox37.Text & ");" & vbNewLine & _
                vbTab & "SetPlayerInterior(playerid, " & TextBox36.Text & ");" & vbNewLine & _
                vbTab & "SetPlayerVirtualWorld(playerid, " & TextBox35.Text & ");" & vbNewLine
                If CheckBox1.Checked Then
                    TextBox40.Text += vbTab & "return SendClientMessage(playerid, " & Settings.C_Msg.Name & ", """ & TextBox33.Text & """);" & vbNewLine & _
                    "}"
                Else
                    TextBox40.Text += vbTab & "return 1;" & vbNewLine & _
                    "}"
                End If
            Else
                TextBox40.Text = "YCMD:" & TextBox31.Text & "(playerid, params[], help)" & vbNewLine & _
                "{" & vbNewLine & _
                vbTab & "If(help) return SendClientMessage(playerid, " & Settings.C_Help.Name & ", """ & TextBox32.Text & """);" & vbNewLine & _
                vbTab & "if(!IsPlayerInAnyVehicle(playerid)){" & vbNewLine & _
                vbTab & vbTab & "SetPlayerPos(playerid, " & TextBox39.Text & ", " & TextBox38.Text & ", " & TextBox34.Text & ");" & vbNewLine & _
                vbTab & vbTab & "SetPlayerFacingAngle(playerid, " & TextBox37.Text & ");" & vbNewLine & _
                vbTab & vbTab & "SetPlayerInterior(playerid, " & TextBox36.Text & ");" & vbNewLine & _
                vbTab & vbTab & "SetPlayerVirtualWorld(playerid, " & TextBox35.Text & ");" & vbNewLine & _
                vbTab & "}" & vbNewLine & _
                vbTab & "else{" & vbNewLine & _
                vbTab & vbTab & "new veh = GetPlayerVehicleID(playerid);" & vbNewLine & _
                vbTab & vbTab & "SetVehiclePos(veh, " & TextBox39.Text & ", " & TextBox38.Text & ", " & TextBox34.Text & ");" & vbNewLine & _
                vbTab & vbTab & "SetVehicleZAngle(veh, " & TextBox37.Text & ");" & vbNewLine & _
                vbTab & vbTab & "LinkVehicleToInterior(veh, " & TextBox36.Text & ");" & vbNewLine & _
                vbTab & vbTab & "SetVehicleVirtualWorld(veh, " & TextBox35.Text & ");" & vbNewLine & _
                vbTab & vbTab & "PutPlayerInVehicle(playerid, veh, 0);" & vbNewLine & _
                vbTab & "}" & vbNewLine
                If CheckBox1.Checked Then
                    TextBox40.Text += vbTab & "return SendClientMessage(playerid, " & Settings.C_Msg.Name & ", """ & TextBox33.Text & """);" & vbNewLine & _
                    "}"
                Else
                    TextBox40.Text += vbTab & "return 1;" & vbNewLine & _
                    "}"
                End If
            End If
        End If
    End Sub

#End Region

#Region "Export"

    Private Sub Button10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button10.Click
        If TextBox40.Text.Length > 0 Then
            Instances(Main.TabControl1.SelectedIndex).SyntaxHandle.Selection.Text = TextBox40.Text
        Else
            TextBox40.Clear()
            If Not TextBox31.Text.Length > 0 Then
                Select Case Settings.Language
                    Case Languages.English
                        MsgBox("You must enter a name for the command.", MsgBoxStyle.Critical, "Error")
                    Case Languages.Español
                        MsgBox("Debes ingresar un nombre para el comando.", MsgBoxStyle.Critical, "Error")
                    Case Languages.Portuguêse
                        MsgBox("Você deve digitar um nome para o comando.", MsgBoxStyle.Critical, "Error")
                    Case Else
                        MsgBox("Du musst einen Namen für den Befehl eingeben.", MsgBoxStyle.Critical, "Fehler")
                End Select
                Exit Sub
            End If
            If CheckBox1.Checked Then
                If Not TextBox33.Text.Length > 0 Then
                    Select Case Settings.Language
                        Case Languages.English
                            MsgBox("You must enter a message to send.", MsgBoxStyle.Critical, "Error")
                        Case Languages.Español
                            MsgBox("Debes ingresar un mensaje para enviar.", MsgBoxStyle.Critical, "Error")
                        Case Languages.Portuguêse
                            MsgBox("Você deve digitar uma mensagem para enviar.", MsgBoxStyle.Critical, "Error")
                        Case Else
                            MsgBox("Du musst eine Nachricht zum senden eingeben.", MsgBoxStyle.Critical, "Fehler")
                    End Select
                    TextBox33.Focus()
                    Exit Sub
                End If
            End If
            If Not TextBox39.Text.Length > 0 Then
                Select Case Settings.Language
                    Case Languages.English
                        MsgBox("You must enter a X coordinate.", MsgBoxStyle.Critical, "Error")
                    Case Languages.Español
                        MsgBox("Debes ingresar una coordenada X.", MsgBoxStyle.Critical, "Error")
                    Case Languages.Portuguêse
                        MsgBox("Você deve digitar uma coordenada X.", MsgBoxStyle.Critical, "Error")
                    Case Else
                        MsgBox("Du musst eine X-Koordinate eingeben.", MsgBoxStyle.Critical, "Fehler")
                End Select
                Exit Sub
            End If
            If Not TextBox38.Text.Length > 0 Then
                Select Case Settings.Language
                    Case Languages.English
                        MsgBox("You must enter a Y coordinate.", MsgBoxStyle.Critical, "Error")
                    Case Languages.Español
                        MsgBox("Debes ingresar una coordenada Y.", MsgBoxStyle.Critical, "Error")
                    Case Languages.Portuguêse
                        MsgBox("Você deve digitar uma coordenada Y.", MsgBoxStyle.Critical, "Error")
                    Case Else
                        MsgBox("Du musst eine Y-Koordinate eingeben.", MsgBoxStyle.Critical, "Fehler")
                End Select
                Exit Sub
            End If
            If Not TextBox34.Text.Length > 0 Then
                Select Case Settings.Language
                    Case Languages.English
                        MsgBox("You must enter a Z coordinate.", MsgBoxStyle.Critical, "Error")
                    Case Languages.Español
                        MsgBox("Debes ingresar una coordenada Z.", MsgBoxStyle.Critical, "Error")
                    Case Languages.Portuguêse
                        MsgBox("Você deve digitar uma coordenada Z.", MsgBoxStyle.Critical, "Error")
                    Case Else
                        MsgBox("Du musst eine Z-Koordinate eingeben.", MsgBoxStyle.Critical, "Fehler")
                End Select
                Exit Sub
            End If
            If Not TextBox35.Text.Length > 0 Then
                Select Case Settings.Language
                    Case Languages.English
                        MsgBox("You must enter a world ID.", MsgBoxStyle.Critical, "Error")
                    Case Languages.Español
                        MsgBox("Debes ingresar el id de un mundo", MsgBoxStyle.Critical, "Error")
                    Case Languages.Portuguêse
                        MsgBox("Você deve digitar um ID mundo.", MsgBoxStyle.Critical, "Error")
                    Case Else
                        MsgBox("Du musst eine Welt ID eingeben.", MsgBoxStyle.Critical, "Fehler")
                End Select
                Exit Sub
            End If
            If Not TextBox36.Text.Length > 0 Then
                Select Case Settings.Language
                    Case Languages.English
                        MsgBox("You must enter an interior ID.", MsgBoxStyle.Critical, "Error")
                    Case Languages.Español
                        MsgBox("Debes ingresar el id de un interior.", MsgBoxStyle.Critical, "Error")
                    Case Languages.Portuguêse
                        MsgBox("Você deve digitar um ID interior.", MsgBoxStyle.Critical, "Error")
                    Case Else
                        MsgBox("Du musst eine Interior ID eingeben.", MsgBoxStyle.Critical, "Fehler")
                End Select
                Exit Sub
            End If
            If Not TextBox37.Text.Length > 0 Then
                Select Case Settings.Language
                    Case Languages.English
                        MsgBox("You must enter an angle.", MsgBoxStyle.Critical, "Error")
                    Case Languages.Español
                        MsgBox("Debes ingresar un angulo.", MsgBoxStyle.Critical, "Error")
                    Case Languages.Portuguêse
                        MsgBox("Você deve digitar um ângulo.", MsgBoxStyle.Critical, "Error")
                    Case Else
                        MsgBox("Du musst einen Winkel angeben.", MsgBoxStyle.Critical, "Fehler")
                End Select
                Exit Sub
            End If
            If RadioButton9.Checked Then
                If RadioButton11.Checked Then
                    TextBox40.Text = "if(!strcmp(cmdtext, ""/" & TextBox31.Text & """, true)){" & vbNewLine & _
                    vbTab & "SetPlayerPos(playerid, " & TextBox39.Text & ", " & TextBox38.Text & ", " & TextBox34.Text & ");" & vbNewLine & _
                    vbTab & "SetPlayerFacingAngle(playerid, " & TextBox37.Text & ");" & vbNewLine & _
                    vbTab & "SetPlayerInterior(playerid, " & TextBox36.Text & ");" & vbNewLine & _
                    vbTab & "SetPlayerVirtualWorld(playerid, " & TextBox35.Text & ");" & vbNewLine
                    If CheckBox1.Checked Then
                        TextBox40.Text += vbTab & "return SendClientMessage(playerid, " & Settings.C_Msg.Name & ", """ & TextBox33.Text & """);" & vbNewLine & _
                        "}"
                    Else
                        TextBox40.Text += vbTab & "return 1;" & vbNewLine & _
                        "}"
                    End If
                Else
                    TextBox40.Text = "if(!strcmp(cmdtext, ""/" & TextBox31.Text & """, true)){" & vbNewLine & _
                    vbTab & "if(!IsPlayerInAnyVehicle(playerid)){" & vbNewLine & _
                    vbTab & vbTab & "SetPlayerPos(playerid, " & TextBox39.Text & ", " & TextBox38.Text & ", " & TextBox34.Text & ");" & vbNewLine & _
                    vbTab & vbTab & "SetPlayerFacingAngle(playerid, " & TextBox37.Text & ");" & vbNewLine & _
                    vbTab & vbTab & "SetPlayerInterior(playerid, " & TextBox36.Text & ");" & vbNewLine & _
                    vbTab & vbTab & "SetPlayerVirtualWorld(playerid, " & TextBox35.Text & ");" & vbNewLine & _
                    vbTab & "}" & vbNewLine & _
                    vbTab & "else{" & vbNewLine & _
                    vbTab & vbTab & "new veh = GetPlayerVehicleID(playerid);" & vbNewLine & _
                    vbTab & vbTab & "SetVehiclePos(veh, " & TextBox39.Text & ", " & TextBox38.Text & ", " & TextBox34.Text & ");" & vbNewLine & _
                    vbTab & vbTab & "SetVehicleZAngle(veh, " & TextBox37.Text & ");" & vbNewLine & _
                    vbTab & vbTab & "LinkVehicleToInterior(veh, " & TextBox36.Text & ");" & vbNewLine & _
                    vbTab & vbTab & "SetVehicleVirtualWorld(veh, " & TextBox35.Text & ");" & vbNewLine & _
                    vbTab & vbTab & "PutPlayerInVehicle(playerid, veh, 0);" & vbNewLine & _
                    vbTab & "}" & vbNewLine
                    If CheckBox1.Checked Then
                        TextBox40.Text += vbTab & "return SendClientMessage(playerid, " & Settings.C_Msg.Name & ", """ & TextBox33.Text & """);" & vbNewLine & _
                        "}"
                    Else
                        TextBox40.Text += vbTab & "return 1;" & vbNewLine & _
                        "}"
                    End If
                End If
            ElseIf RadioButton8.Checked Then
                If RadioButton11.Checked Then
                    TextBox40.Text = "dcmd(" & TextBox31.Text & ", " & TextBox31.Text.Length & ", " & "cmdtext);" & vbNewLine & vbNewLine & _
                    "dcmd_" & TextBox31.Text & "(playerid, params[])" & vbNewLine & _
                    "{" & vbNewLine & _
                    vbTab & "#pragma unused params" & vbNewLine & _
                    vbTab & "SetPlayerPos(playerid, " & TextBox39.Text & ", " & TextBox38.Text & ", " & TextBox34.Text & ");" & vbNewLine & _
                    vbTab & "SetPlayerFacingAngle(playerid, " & TextBox37.Text & ");" & vbNewLine & _
                    vbTab & "SetPlayerInterior(playerid, " & TextBox36.Text & ");" & vbNewLine & _
                    vbTab & "SetPlayerVirtualWorld(playerid, " & TextBox35.Text & ");" & vbNewLine
                    If CheckBox1.Checked Then
                        TextBox40.Text += vbTab & "return SendClientMessage(playerid, " & Settings.C_Msg.Name & ", """ & TextBox33.Text & """);" & vbNewLine & _
                        "}"
                    Else
                        TextBox40.Text += vbTab & "return 1;" & vbNewLine & _
                        "}"
                    End If
                Else
                    TextBox40.Text = "dcmd(" & TextBox31.Text & ", " & TextBox31.Text.Length & ", " & "cmdtext);" & vbNewLine & vbNewLine & _
                    "dcmd_" & TextBox31.Text & "(playerid, params[])" & vbNewLine & _
                    "{" & vbNewLine & _
                    vbTab & "#pragma unused params" & vbNewLine & _
                    vbTab & "if(!IsPlayerInAnyVehicle(playerid)){" & vbNewLine & _
                    vbTab & vbTab & "SetPlayerPos(playerid, " & TextBox39.Text & ", " & TextBox38.Text & ", " & TextBox34.Text & ");" & vbNewLine & _
                    vbTab & vbTab & "SetPlayerFacingAngle(playerid, " & TextBox37.Text & ");" & vbNewLine & _
                    vbTab & vbTab & "SetPlayerInterior(playerid, " & TextBox36.Text & ");" & vbNewLine & _
                    vbTab & vbTab & "SetPlayerVirtualWorld(playerid, " & TextBox35.Text & ");" & vbNewLine & _
                    vbTab & "}" & vbNewLine & _
                    vbTab & "else{" & vbNewLine & _
                    vbTab & vbTab & "new veh = GetPlayerVehicleID(playerid);" & vbNewLine & _
                    vbTab & vbTab & "SetVehiclePos(veh, " & TextBox39.Text & ", " & TextBox38.Text & ", " & TextBox34.Text & ");" & vbNewLine & _
                    vbTab & vbTab & "SetVehicleZAngle(veh, " & TextBox37.Text & ");" & vbNewLine & _
                    vbTab & vbTab & "LinkVehicleToInterior(veh, " & TextBox36.Text & ");" & vbNewLine & _
                    vbTab & vbTab & "SetVehicleVirtualWorld(veh, " & TextBox35.Text & ");" & vbNewLine & _
                    vbTab & vbTab & "PutPlayerInVehicle(playerid, veh, 0);" & vbNewLine & _
                    vbTab & "}" & vbNewLine
                    If CheckBox1.Checked Then
                        TextBox40.Text += vbTab & "return SendClientMessage(playerid, " & Settings.C_Msg.Name & ", """ & TextBox33.Text & """);" & vbNewLine & _
                        "}"
                    Else
                        TextBox40.Text += vbTab & "return 1;" & vbNewLine & _
                        "}"
                    End If
                End If
            ElseIf RadioButton7.Checked Then
                If RadioButton11.Checked Then
                    TextBox40.Text = "CMD:" & TextBox31.Text & "(playerid, params[])" & vbNewLine & _
                    "{" & vbNewLine & _
                    vbTab & "SetPlayerPos(playerid, " & TextBox39.Text & ", " & TextBox38.Text & ", " & TextBox34.Text & ");" & vbNewLine & _
                    vbTab & "SetPlayerFacingAngle(playerid, " & TextBox37.Text & ");" & vbNewLine & _
                    vbTab & "SetPlayerInterior(playerid, " & TextBox36.Text & ");" & vbNewLine & _
                    vbTab & "SetPlayerVirtualWorld(playerid, " & TextBox35.Text & ");" & vbNewLine
                    If CheckBox1.Checked Then
                        TextBox40.Text += vbTab & "return SendClientMessage(playerid, " & Settings.C_Msg.Name & ", """ & TextBox33.Text & """);" & vbNewLine & _
                        "}"
                    Else
                        TextBox40.Text += vbTab & "return 1;" & vbNewLine & _
                        "}"
                    End If
                Else
                    TextBox40.Text = "CMD:" & TextBox31.Text & "(playerid, params[])" & vbNewLine & _
                    "{" & vbNewLine & _
                    vbTab & "if(!IsPlayerInAnyVehicle(playerid)){" & vbNewLine & _
                    vbTab & vbTab & "SetPlayerPos(playerid, " & TextBox39.Text & ", " & TextBox38.Text & ", " & TextBox34.Text & ");" & vbNewLine & _
                    vbTab & vbTab & "SetPlayerFacingAngle(playerid, " & TextBox37.Text & ");" & vbNewLine & _
                    vbTab & vbTab & "SetPlayerInterior(playerid, " & TextBox36.Text & ");" & vbNewLine & _
                    vbTab & vbTab & "SetPlayerVirtualWorld(playerid, " & TextBox35.Text & ");" & vbNewLine & _
                    vbTab & "}" & vbNewLine & _
                    vbTab & "else{" & vbNewLine & _
                    vbTab & vbTab & "new veh = GetPlayerVehicleID(playerid);" & vbNewLine & _
                    vbTab & vbTab & "SetVehiclePos(veh, " & TextBox39.Text & ", " & TextBox38.Text & ", " & TextBox34.Text & ");" & vbNewLine & _
                    vbTab & vbTab & "SetVehicleZAngle(veh, " & TextBox37.Text & ");" & vbNewLine & _
                    vbTab & vbTab & "LinkVehicleToInterior(veh, " & TextBox36.Text & ");" & vbNewLine & _
                    vbTab & vbTab & "SetVehicleVirtualWorld(veh, " & TextBox35.Text & ");" & vbNewLine & _
                    vbTab & vbTab & "PutPlayerInVehicle(playerid, veh, 0);" & vbNewLine & _
                    vbTab & "}" & vbNewLine
                    If CheckBox1.Checked Then
                        TextBox40.Text += vbTab & "return SendClientMessage(playerid, " & Settings.C_Msg.Name & ", """ & TextBox33.Text & """);" & vbNewLine & _
                        "}"
                    Else
                        TextBox40.Text += vbTab & "return 1;" & vbNewLine & _
                        "}"
                    End If
                End If
            ElseIf RadioButton6.Checked Then
                If Not TextBox32.Text.Length Then
                    Select Case Settings.Language
                        Case Languages.English
                            MsgBox("You must enter a help message to send.", MsgBoxStyle.Critical, "Error")
                        Case Languages.Español
                            MsgBox("Debes ingresar un mensaje de ayuda para enviar.", MsgBoxStyle.Critical, "Error")
                        Case Languages.Portuguêse
                            MsgBox("YVocê deve digitar uma mensagem de ajuda para enviar.", MsgBoxStyle.Critical, "Error")
                        Case Else
                            MsgBox("Du musst eine Hilfsnachricht zum senden angeben.", MsgBoxStyle.Critical, "Fehler")
                    End Select
                    TextBox33.Focus()
                    Exit Sub
                End If
                If RadioButton11.Checked Then
                    TextBox40.Text = "YCMD:" & TextBox31.Text & "(playerid, params[], help)" & vbNewLine & _
                    "{" & vbNewLine & _
                    vbTab & "if(help) return SendClientMessage(playerid, " & Settings.C_Help.Name & ", """ & TextBox32.Text & """);" & vbNewLine & _
                    vbTab & "SetPlayerPos(playerid, " & TextBox39.Text & ", " & TextBox38.Text & ", " & TextBox34.Text & ");" & vbNewLine & _
                    vbTab & "SetPlayerFacingAngle(playerid, " & TextBox37.Text & ");" & vbNewLine & _
                    vbTab & "SetPlayerInterior(playerid, " & TextBox36.Text & ");" & vbNewLine & _
                    vbTab & "SetPlayerVirtualWorld(playerid, " & TextBox35.Text & ");" & vbNewLine
                    If CheckBox1.Checked Then
                        TextBox40.Text += vbTab & "return SendClientMessage(playerid, " & Settings.C_Msg.Name & ", """ & TextBox33.Text & """);" & vbNewLine & _
                        "}"
                    Else
                        TextBox40.Text += vbTab & "return 1;" & vbNewLine & _
                        "}"
                    End If
                Else
                    TextBox40.Text = "YCMD:" & TextBox31.Text & "(playerid, params[], help)" & vbNewLine & _
                    "{" & vbNewLine & _
                    vbTab & "If(help) return SendClientMessage(playerid, " & Settings.C_Help.Name & ", """ & TextBox32.Text & """);" & vbNewLine & _
                    vbTab & "if(!IsPlayerInAnyVehicle(playerid)){" & vbNewLine & _
                    vbTab & vbTab & "SetPlayerPos(playerid, " & TextBox39.Text & ", " & TextBox38.Text & ", " & TextBox34.Text & ");" & vbNewLine & _
                    vbTab & vbTab & "SetPlayerFacingAngle(playerid, " & TextBox37.Text & ");" & vbNewLine & _
                    vbTab & vbTab & "SetPlayerInterior(playerid, " & TextBox36.Text & ");" & vbNewLine & _
                    vbTab & vbTab & "SetPlayerVirtualWorld(playerid, " & TextBox35.Text & ");" & vbNewLine & _
                    vbTab & "}" & vbNewLine & _
                    vbTab & "else{" & vbNewLine & _
                    vbTab & vbTab & "new veh = GetPlayerVehicleID(playerid);" & vbNewLine & _
                    vbTab & vbTab & "SetVehiclePos(veh, " & TextBox39.Text & ", " & TextBox38.Text & ", " & TextBox34.Text & ");" & vbNewLine & _
                    vbTab & vbTab & "SetVehicleZAngle(veh, " & TextBox37.Text & ");" & vbNewLine & _
                    vbTab & vbTab & "LinkVehicleToInterior(veh, " & TextBox36.Text & ");" & vbNewLine & _
                    vbTab & vbTab & "SetVehicleVirtualWorld(veh, " & TextBox35.Text & ");" & vbNewLine & _
                    vbTab & vbTab & "PutPlayerInVehicle(playerid, veh, 0);" & vbNewLine & _
                    vbTab & "}" & vbNewLine
                    If CheckBox1.Checked Then
                        TextBox40.Text += vbTab & "return SendClientMessage(playerid, " & Settings.C_Msg.Name & ", """ & TextBox33.Text & """);" & vbNewLine & _
                        "}"
                    Else
                        TextBox40.Text += vbTab & "return 1;" & vbNewLine & _
                        "}"
                    End If
                End If
            End If
            Instances(Main.TabControl1.SelectedIndex).SyntaxHandle.Selection.Text = TextBox40.Text
            Me.Hide()
        End If
    End Sub

#End Region

#End Region

#Region "Dialogs"

#Region "Help"

    Private Sub TextBox3_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox3.GotFocus
        Select Case Settings.Language
            Case Languages.English
                If TextBox3.Text.IndexOf("PRESS ENTER TO CREATE A JUMP LINE ON THE TEXT") <> -1 Then
                    TextBox3.Clear()
                    TextBox3.ForeColor = Color.Black
                End If
            Case Languages.Español
                If TextBox3.Text.IndexOf("PRESIONA ENTER PARA CREAR UN SALTO DE LINEA EN EL TEXTO") <> -1 Then
                    TextBox3.Clear()
                    TextBox3.ForeColor = Color.Black
                End If
            Case Languages.Portuguêse
                If TextBox3.Text.IndexOf("PRESSIONE ENTER PARA CRIAR UM SALTO DE LINHA NO TEXTO") <> -1 Then
                    TextBox3.Clear()
                    TextBox3.ForeColor = Color.Black
                End If
            Case Else
                If TextBox3.Text.IndexOf("DRÜCKE ENTER, UM EINE LEERZEILE ZU ERSTELLEN, ODER") <> -1 Then
                    TextBox3.Clear()
                    TextBox3.ForeColor = Color.Black
                End If
        End Select
    End Sub

    Private Sub TextBox3_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox3.LostFocus
        If TextBox3.Text.Length = 0 Then
            Select Case Settings.Language
                Case Languages.English
                    TextBox40.Text = vbNewLine & vbNewLine & "PRESS ENTER TO CREATE A JUMP LINE ON THE TEXT OR" & vbNewLine & "TAB TO CREATE A TABULATION"
                    TextBox40.ForeColor = Color.Gray
                Case Languages.Español
                    TextBox40.Text = vbNewLine & vbNewLine & "PRESIONA ENTER PARA CREAR UN SALTO DE LINEA EN EL TEXTO O" & vbNewLine & "TAB PARA CREAR UNA TABULACIÓN"
                    TextBox40.ForeColor = Color.Gray
                Case Languages.Portuguêse
                    TextBox40.Text = vbNewLine & vbNewLine & "PRESSIONE ENTER PARA CRIAR UM SALTO DE LINHA NO TEXTO OU" & vbNewLine & "TAB PARA CRIAR UMA TABULAÇÃO"
                    TextBox40.ForeColor = Color.Gray
                Case Else
                    TextBox40.Text = vbNewLine & vbNewLine & "DRÜCKE ENTER, UM EINE LEERZEILE ZU ERSTELLEN, ODER" & vbNewLine & "TAB, UM EINE EINRÜCKUNG ZU ERSTELLEN"
                    TextBox40.ForeColor = Color.Gray
            End Select
        End If
    End Sub

    Private Sub TextBox3_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox3.KeyPress
        If e.KeyChar = Chr(Keys.Tab) Then
            TextBox3.SelectedText = "\t"
            e.Handled = True
        ElseIf e.KeyChar = Chr(Keys.Enter) Then
            TextBox3.SelectedText = "\n"
            e.Handled = True
        End If
    End Sub

#End Region

#Region "Generate"

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Instances(Main.TabControl1.SelectedIndex).SyntaxHandle.Selection.Text = "ShowPlayerDialog(" & TextBox6.Text & ", " & TextBox1.Text & ", " & ComboBox1.Text & ", """ & TextBox2.Text & """, """ & TextBox3.Text & """, """ & TextBox4.Text & """, """ & TextBox5.Text & """);"
        Me.Hide()
    End Sub

#End Region

#Region "Previewer"

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        UpdatePreviewer()
        UpdatePreviewer()
    End Sub

    Private Sub TextBox2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox2.TextChanged
        UpdatePreviewer()
    End Sub

    Private Sub TextBox4_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox4.TextChanged
        UpdatePreviewer()
    End Sub

    Private Sub TextBox5_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox5.TextChanged
        UpdatePreviewer()
    End Sub

    Private Sub TextBox3_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox3.TextChanged
        UpdatePreviewer()
    End Sub

#End Region

#Region "Functions/Subs"

    Private Function GetImageFromGui(ByVal type As GuiImage) As Image
        Dim bmp As Bitmap, g As Graphics, selection As Rectangle
        Select Case type
            Case GuiImage.Button
                selection = New Rectangle(2, 3, 174, 63)
                bmp = New Bitmap(selection.Width, selection.Height)
                g = Graphics.FromImage(bmp)
                g.DrawImage(My.Resources.sampgui, 0, 0, selection, GraphicsUnit.Pixel)
                g.Dispose()
            Case GuiImage.TextBox
                selection = New Rectangle(7, 83, 320, 50)
                bmp = New Bitmap(selection.Width, selection.Height)
                g = Graphics.FromImage(bmp)
                g.DrawImage(My.Resources.sampgui, 0, 0, selection, GraphicsUnit.Pixel)
                g.Dispose()
            Case GuiImage.Scroll
                Dim scroll(2) As Image, line(2) As Image
                selection = New Rectangle(243, 123, 29, 33)
                scroll(0) = New Bitmap(selection.Width, selection.Height)
                g = Graphics.FromImage(scroll(0))
                g.DrawImage(My.Resources.sampgui, 0, 0, selection, GraphicsUnit.Pixel)
                g.Dispose()
                selection = New Rectangle(243, 149.5, 29, 33)
                scroll(1) = New Bitmap(selection.Width, selection.Height)
                g = Graphics.FromImage(scroll(1))
                g.DrawImage(My.Resources.sampgui, 0, 0, selection, GraphicsUnit.Pixel)
                g.Dispose()
                selection = New Rectangle(243, 141, 3, 17)
                line(0) = New Bitmap(selection.Width, selection.Height)
                g = Graphics.FromImage(line(0))
                g.DrawImage(My.Resources.sampgui, 0, 0, selection, GraphicsUnit.Pixel)
                g.Dispose()
                selection = New Rectangle(262, 141, 3, 17)
                line(1) = New Bitmap(selection.Width, selection.Height)
                g = Graphics.FromImage(line(1))
                g.DrawImage(My.Resources.sampgui, 0, 0, selection, GraphicsUnit.Pixel)
                g.Dispose()
                bmp = New Bitmap(29, 202)
                g = Graphics.FromImage(bmp)
                g.DrawImage(scroll(0), New Point(0, 0))
                g.DrawImage(scroll(1), New Point(0, scroll(0).Height + 136))
                g.DrawImage(line(0), New Point(0, scroll(0).Height))
                g.DrawImage(line(0), New Point(0, scroll(0).Height + line(0).Height))
                g.DrawImage(line(0), New Point(0, scroll(0).Height + line(0).Height * 2))
                g.DrawImage(line(0), New Point(0, scroll(0).Height + line(0).Height * 3))
                g.DrawImage(line(0), New Point(0, scroll(0).Height + line(0).Height * 4))
                g.DrawImage(line(0), New Point(0, scroll(0).Height + line(0).Height * 5))
                g.DrawImage(line(0), New Point(0, scroll(0).Height + line(0).Height * 6))
                g.DrawImage(line(0), New Point(0, scroll(0).Height + line(0).Height * 7))
                g.DrawImage(line(1), New Point(scroll(0).Width - 3.51, scroll(0).Height))
                g.DrawImage(line(1), New Point(scroll(0).Width - 4, scroll(0).Height + line(1).Height))
                g.DrawImage(line(1), New Point(scroll(0).Width - 4, scroll(0).Height + line(1).Height * 2))
                g.DrawImage(line(1), New Point(scroll(0).Width - 4, scroll(0).Height + line(1).Height * 3))
                g.DrawImage(line(1), New Point(scroll(0).Width - 4, scroll(0).Height + line(1).Height * 4))
                g.DrawImage(line(1), New Point(scroll(0).Width - 4, scroll(0).Height + line(1).Height * 5))
                g.DrawImage(line(1), New Point(scroll(0).Width - 4, scroll(0).Height + line(1).Height * 6))
                g.DrawImage(line(1), New Point(scroll(0).Width - 4, scroll(0).Height + line(1).Height * 7))
                g.DrawLine(Pens.White, scroll(0).Width - 1, scroll(0).Height, scroll(0).Width - 1, scroll(0).Height + 136)
                g.Dispose()
            Case GuiImage.Dot
                Dim dot As Image
                selection = New Rectangle(56, 56, 30, 31)
                dot = New Bitmap(selection.Width, selection.Height)
                g = Graphics.FromImage(dot)
                g.DrawImage(My.Resources.sampgui, 0, 0, selection, GraphicsUnit.Pixel)
                g.Dispose()
                bmp = New Bitmap(456, 31)
                g = Graphics.FromImage(bmp)
                g.DrawImage(dot, New Point(0, 0))
                g.DrawImage(dot, New Point(dot.Width + 8, 0))
                g.DrawImage(dot, New Point((dot.Width + 8) * 2, 0))
                g.DrawImage(dot, New Point((dot.Width + 8) * 3, 0))
                g.DrawImage(dot, New Point((dot.Width + 8) * 4, 0))
                g.DrawImage(dot, New Point((dot.Width + 8) * 5, 0))
                g.DrawImage(dot, New Point((dot.Width + 8) * 6, 0))
                g.DrawImage(dot, New Point((dot.Width + 8) * 7, 0))
                g.DrawImage(dot, New Point((dot.Width + 8) * 8, 0))
                g.DrawImage(dot, New Point((dot.Width + 8) * 9, 0))
                g.DrawImage(dot, New Point((dot.Width + 8) * 10, 0))
                g.DrawImage(dot, New Point((dot.Width + 8) * 11, 0))
                g.Dispose()
            Case Else
                bmp = New Bitmap(1, 1)
        End Select
        Return bmp
    End Function

    Public Function GetCharCountFromLongestLine(ByVal Text As RichTextBox) As Integer
        Dim count As Integer
        For Each line In Text.Lines
            If count < line.Length Then
                count = line.Length
            End If
        Next
        Return count
    End Function

    Public Function ProcessText(ByRef Text As String, ByVal TypeList As Boolean) As String
        Dim str As String = vbNullString, tmp As String()
        tmp = Split(Text, "\t")
        For i = LBound(tmp) To UBound(tmp)
            str += tmp(i) & vbTab
        Next
        tmp = Split(str, "\n")
        str = vbNullString
        For i = LBound(tmp) To UBound(tmp)
            If i < UBound(tmp) Then
                If TypeList = True Then
                    str += tmp(i) & "{FFFFFF}" & vbNewLine
                Else
                    str += tmp(i) & "{A9C4E4}" & vbNewLine
                End If
            Else
                str += tmp(i)
            End If
        Next
        Return str
    End Function

    Public Sub ProcessColor(ByRef Rich As RichTextBox)
        Rich.Text = Rich.Text.Replace("}", "}|/\| ")
        Dim istart As Integer, iend As Integer, ccColor As String = vbNullString
        With Rich
            istart = .Text.IndexOf("{")
            If istart = -1 Then
                Exit Sub
            Else
                iend = .Text.IndexOf("}", istart)
            End If
            While istart > -1 And iend > -1
                .SelectionStart = istart + 1
                .SelectionLength = iend - istart - 1
                If IsHex(.SelectedText) = True Then
                    ccColor = .SelectedText
                Else
                    Dim defined As Boolean = False
                    For Each col As PawnColor In Instances(Main.TabControl1.SelectedIndex).ACLists.eColors
                        If col.Name = .SelectedText Then
                            defined = True
                            ccColor = cColor(col.Hex.R, col.Hex.G, col.Hex.B)
                            .Text = .Text.Replace(.SelectedText, ccColor)
                            If istart < iend Then
                                istart = .Text.IndexOf("{")
                            Else
                                istart = .Text.IndexOf("{", istart + 1)
                            End If
                            If istart = -1 Then
                                Exit Sub
                            Else
                                iend = .Text.IndexOf("}", istart)
                            End If
                            .SelectionStart = istart + 1
                            .SelectionLength = iend - istart - 1
                            Exit For
                        End If
                    Next
                    If Not defined Then
                        .Text = .Text.Replace("}|/\| ", "}")
                        Select Case Settings.Language
                            Case Languages.English
                                MsgBox("The color must be in RGB Hex code or be a embedded defined color.", MsgBoxStyle.Critical, "Error")
                            Case Languages.Español
                                MsgBox("El color debe estar en formato Hex RGB o ser un color definido como embebido.", MsgBoxStyle.Critical, "Error")
                            Case Languages.Portuguêse
                                MsgBox("A cor deve estar em RGB código Hex ou ser uma cor definida embutido.", MsgBoxStyle.Critical, "Error")
                            Case Else
                                MsgBox("Die Farbe muss in RGB Hex-Code werden oder ein eingebettetes definierten Farbe.", MsgBoxStyle.Critical, "Error")
                        End Select
                        Exit Sub
                    End If
                End If
                .SelectionStart = iend
                If .Text.IndexOf("\n", iend) > -1 Then
                    .SelectionLength = .Text.IndexOf(Keys.Enter.ToString, iend) - iend
                ElseIf .Text.IndexOf("{", iend) > -1 Then
                    .SelectionLength = .Text.IndexOf("{", iend) - iend
                Else
                    .SelectionLength = .Text.Length - iend
                End If
                .SelectionColor = Color.FromArgb(Integer.Parse(ccColor, Globalization.NumberStyles.HexNumber))
                .SelectionStart = istart
                .SelectionLength = iend - istart + 6
                .ReadOnly = False
                .SelectedText = ""
                .ReadOnly = True
                If iend < .Text.Length Then
                    istart = .Text.IndexOf("{", istart + 1)
                    iend = .Text.IndexOf("}", iend + 1)
                Else
                    Exit While
                End If
            End While
        End With
    End Sub

    Private Sub UpdatePreviewer()
        RichTextBox1.Clear()
        RichTextBox2.Clear()
        If ComboBox1.SelectedIndex = 2 Then
            RichTextBox1.Text = ProcessText(TextBox3.Text, True)
        Else
            RichTextBox1.Text = ProcessText(TextBox3.Text, False)
        End If
        RichTextBox2.Text = TextBox2.Text
        ProcessColor(RichTextBox1)
        ProcessColor(RichTextBox2)
        TextBox19.Text = TextBox4.Text
        TextBox18.Text = TextBox5.Text
        Dim chars As Integer, linecount As Integer
        chars = GetCharCountFromLongestLine(RichTextBox1)
        linecount = RichTextBox1.Lines.Length
        Select Case ComboBox1.Text
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
                Panel2.Size = New Point(350 + 2.3 * chars, 109 + 11 * linecount)
                If TextBox5.Text = "" Then
                    TextBox18.Visible = False
                    PictureBox2.Visible = False
                    TextBox19.Location = New Point(Panel2.Width / 2 - 50, Panel2.Height - 40)
                    PictureBox1.Location = New Point(Panel2.Width / 2 - 55.5, Panel2.Height - 44)
                Else
                    TextBox18.Visible = True
                    TextBox18.Location = New Point(Panel2.Width * (2 / 3) - TextBox18.Width / 2, Panel2.Height - 40)
                    PictureBox2.Location = New Point(Panel2.Width * (2 / 3) - TextBox18.Width / 2 - 5, Panel2.Height - 44)
                    PictureBox2.Visible = True
                    TextBox19.Location = New Point(Panel2.Width / 3 - TextBox19.Width / 2, Panel2.Height - 40)
                    PictureBox1.Location = New Point(Panel2.Width / 3 - TextBox19.Width / 2 - 5, Panel2.Height - 44)
                End If
                RichTextBox1.BorderStyle = BorderStyle.None
                RichTextBox1.Font = New Font("Arial Rounded MT Bold", 9, FontStyle.Regular)
                RichTextBox1.ForeColor = Color.FromArgb(255, 169, 196, 228)
                RichTextBox1.Location = New Point(35, 27)
                RichTextBox1.Size = New Point(Panel2.Width - 70, Panel2.Height - 78)
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
                Panel2.Size = New Point(350 + 2.3 * chars, 151 + 11 * linecount)
                If TextBox5.Text = "" Then
                    TextBox18.Visible = False
                    PictureBox2.Visible = False
                    TextBox19.Location = New Point(Panel2.Width / 2 - 50, Panel2.Height - 37)
                    PictureBox1.Location = New Point(Panel2.Width / 2 - 55.5, Panel2.Height - 41)
                Else
                    TextBox18.Visible = True
                    TextBox18.Location = New Point(Panel2.Width * (2 / 3) - TextBox18.Width / 2, Panel2.Height - 40)
                    PictureBox2.Location = New Point(Panel2.Width * (2 / 3) - TextBox18.Width / 2 - 5, Panel2.Height - 44)
                    PictureBox2.Visible = True
                    TextBox19.Location = New Point(Panel2.Width / 3 - TextBox19.Width / 2, Panel2.Height - 40)
                    PictureBox1.Location = New Point(Panel2.Width / 3 - TextBox19.Width / 2 - 5, Panel2.Height - 44)
                End If
                PictureBox4.Location = New Point(12, 63 + 11 * linecount)
                PictureBox4.Size = New Point(Panel2.Width - 24, 32)
                RichTextBox1.BorderStyle = BorderStyle.None
                RichTextBox1.Font = New Font("Arial Rounded MT Bold", 9, FontStyle.Regular)
                RichTextBox1.ForeColor = Color.FromArgb(255, 169, 196, 228)
                RichTextBox1.Size = New Point(Panel2.Width - 70, Panel2.Height - 78)
                RichTextBox1.Location = New Point(35 - RichTextBox1.Size.Width / 10, 27 - RichTextBox1.Size.Height / 10)
            Case "DIALOG_STYLE_LIST"
                If chars > 80 Then
                    chars = 80
                ElseIf chars < 6 Then
                    chars = 0
                End If
                PictureBox3.Visible = True
                PictureBox4.Visible = False
                PictureBox5.Visible = False
                If TextBox5.Text = "" Then
                    TextBox18.Visible = False
                    PictureBox2.Visible = False
                    TextBox19.Location = New Point(Panel2.Width / 2 - 50, 245)
                    PictureBox1.Location = New Point(Panel2.Width / 2 - 55.5, 241)
                Else
                    TextBox18.Visible = True
                    TextBox18.Location = New Point(Panel2.Width * (2 / 3) - TextBox18.Width / 2, 245)
                    PictureBox2.Location = New Point(Panel2.Width * (2 / 3) - TextBox18.Width / 2 - 5, 241)
                    PictureBox2.Visible = True
                    TextBox19.Location = New Point(Panel2.Width / 3 - TextBox19.Width / 2, 245)
                    PictureBox1.Location = New Point(Panel2.Width / 3 - TextBox19.Width / 2 - 5, 241)
                End If
                Panel2.Size = New Point(350 + 2.3 * chars, 275)
                RichTextBox2.Location = New Point(-1, 0)
                RichTextBox2.Size = New Point(Panel2.Width, 23)
                RichTextBox1.BorderStyle = BorderStyle.Fixed3D
                RichTextBox1.Font = New Font("Arial Rounded MT Bold", 11, FontStyle.Regular)
                RichTextBox1.ForeColor = Color.White
                RichTextBox1.Location = New Point(2, 27)
                RichTextBox1.Size = New Point(Panel2.Width - 4, Panel2.Height - 73)
                PictureBox3.Location = New Point(Panel2.Width - 24, 28)
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
                Panel2.Size = New Point(350 + 2.3 * chars, 151 + 11 * linecount)
                If TextBox5.Text = "" Then
                    TextBox18.Visible = False
                    PictureBox2.Visible = False
                    TextBox19.Location = New Point(Panel2.Width / 2 - 50, Panel2.Height - 37)
                    PictureBox1.Location = New Point(Panel2.Width / 2 - 55.5, Panel2.Height - 41)
                Else
                    TextBox18.Visible = True
                    TextBox18.Location = New Point(Panel2.Width * (2 / 3) - TextBox18.Width / 2, Panel2.Height - 40)
                    PictureBox2.Location = New Point(Panel2.Width * (2 / 3) - TextBox18.Width / 2 - 5, Panel2.Height - 44)
                    PictureBox2.Visible = True
                    TextBox19.Location = New Point(Panel2.Width / 3 - TextBox19.Width / 2, Panel2.Height - 40)
                    PictureBox1.Location = New Point(Panel2.Width / 3 - TextBox19.Width / 2 - 5, Panel2.Height - 44)
                End If
                PictureBox4.Location = New Point(12, 63 + 11 * linecount)
                PictureBox4.Size = New Point(Panel2.Width - 24, 32)
                PictureBox5.Location = New Point(25, 75 + 11 * linecount)
                RichTextBox1.BorderStyle = BorderStyle.None
                RichTextBox1.Font = New Font("Arial Rounded MT Bold", 9, FontStyle.Regular)
                RichTextBox1.ForeColor = Color.FromArgb(255, 169, 196, 228)
                RichTextBox1.Location = New Point(35, 27)
                RichTextBox1.Size = New Point(Panel2.Width - 70, Panel2.Height - 78)
        End Select
    End Sub

#End Region

#Region "Extra"

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        gSender = CC.Dialog
        eColor.Show()
    End Sub

#End Region

#End Region

#Region "Color Picker"

#Region "Arrays"

    Dim ColorSender As Integer = 0

#End Region

#Region "Export"

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Instances(Main.TabControl1.SelectedIndex).SyntaxHandle.Selection.Text = TextBox21.Text
        Me.Hide()
    End Sub

#End Region

#Region "Text Restrictions"

    Private Sub textbox7_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox7.KeyPress
        If Not IsNumeric(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) AndAlso Asc(e.KeyChar) <> 8 Then e.Handled = True
    End Sub

    Private Sub textbox8_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox8.KeyPress
        If Not IsNumeric(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) AndAlso Asc(e.KeyChar) <> 8 Then e.Handled = True
    End Sub

    Private Sub textbox9_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox9.KeyPress
        If Not IsNumeric(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) AndAlso Asc(e.KeyChar) <> 8 Then e.Handled = True
    End Sub

    Private Sub textbox10_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox10.TextAlignChanged
        If Not IsNumeric(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) AndAlso Asc(e.KeyChar) <> 8 Then e.Handled = True
    End Sub

    Private Sub textbox11_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox11.KeyPress
        If Not IsNumeric(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) AndAlso e.KeyChar <> "," AndAlso Asc(e.KeyChar) <> 8 Then e.Handled = True
        If e.KeyChar = "," AndAlso TextBox11.Text.IndexOf(",") > -1 Then e.Handled = True
    End Sub

    Private Sub textbox12_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox12.KeyPress
        If Not IsNumeric(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) AndAlso e.KeyChar <> "," AndAlso Asc(e.KeyChar) <> 8 Then e.Handled = True
        If e.KeyChar = "," AndAlso TextBox12.Text.IndexOf(",") > -1 Then e.Handled = True
    End Sub

    Private Sub textbox13_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox13.KeyPress
        If Not IsNumeric(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) AndAlso e.KeyChar <> "," AndAlso Asc(e.KeyChar) <> 8 Then e.Handled = True
        If e.KeyChar = "," AndAlso TextBox13.Text.IndexOf(",") > -1 Then e.Handled = True
    End Sub

    Private Sub textbox14_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox14.KeyPress
        If Not IsNumeric(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) AndAlso e.KeyChar <> "," AndAlso Asc(e.KeyChar) <> 8 Then e.Handled = True
        If e.KeyChar = "," AndAlso TextBox14.Text.IndexOf(",") > -1 Then e.Handled = True
    End Sub

    Private Sub textbox15_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox15.KeyPress
        If Not IsNumeric(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) AndAlso Asc(e.KeyChar) <> 8 Then e.Handled = True
    End Sub

    Private Sub textbox16_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox16.KeyPress
        If Not IsNumeric(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) AndAlso e.KeyChar <> "," AndAlso Asc(e.KeyChar) <> 8 Then e.Handled = True
        If e.KeyChar = "," AndAlso TextBox16.Text.IndexOf(",") > -1 Then e.Handled = True
    End Sub

    Private Sub textbox17_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox17.KeyPress
        If Not IsNumeric(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) AndAlso e.KeyChar <> "," AndAlso Asc(e.KeyChar) <> 8 Then e.Handled = True
        If e.KeyChar = "," AndAlso TextBox17.Text.IndexOf(",") > -1 Then e.Handled = True
    End Sub

#End Region

#Region "Generating Color"

#Region "RGB"

#Region "Tracks"

    Private Sub TrackBar1_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBar1.Scroll
        If Not ColorSender Then
            Dim nCMYK As CMYK = RGB2CMYK(TrackBar1.Value, TrackBar2.Value, TrackBar3.Value)
            Dim nHSL As HSL = RGB2HSL(TrackBar1.Value, TrackBar2.Value, TrackBar3.Value)
            If Not CheckBox6.Checked Then
                TextBox21.TextAlign = HorizontalAlignment.Center
                If CheckBox5.Checked Then
                    TextBox21.Text = cColor(TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel5.BackColor)
                Else
                    TextBox21.Text = cColor(TrackBar4.Value, TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel5.BackColor)
                End If
            Else
                If TextBox21.Text.Length > 0 Then
                    TextBox21.TextAlign = HorizontalAlignment.Left
                    If CheckBox5.Checked Then
                        TextBox21.Text = "#define " & TextBox20.Text & cColor(TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel5.BackColor)
                    Else
                        TextBox21.Text = "#define " & TextBox20.Text & "(" & cColor(TrackBar4.Value, TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel5.BackColor) & ")"
                    End If
                Else
                    CheckBox6.Checked = False
                    TextBox21.TextAlign = HorizontalAlignment.Center
                    If CheckBox5.Checked Then
                        TextBox21.Text = cColor(TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel5.BackColor)
                    Else
                        TextBox21.Text = cColor(TrackBar4.Value, TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel5.BackColor)
                    End If
                End If
            End If
            TextBox7.Text = TrackBar1.Value
            TrackBar5.Value = nCMYK.Cyan * 1000
            TextBox11.Text = nCMYK.Cyan
            TrackBar6.Value = nCMYK.Magenta * 1000
            TextBox12.Text = nCMYK.Magenta
            TrackBar7.Value = nCMYK.Yellow * 1000
            TextBox13.Text = nCMYK.Yellow
            TrackBar8.Value = nCMYK.Black * 1000
            TextBox14.Text = nCMYK.Black
            TrackBar9.Value = nHSL.Hue
            TextBox15.Text = nHSL.Hue
            TrackBar10.Value = nHSL.Saturation * 1000
            TextBox16.Text = nHSL.Saturation
            TrackBar11.Value = nHSL.Luminance * 1000
            TextBox17.Text = nHSL.Luminance
        End If
    End Sub

    Private Sub TrackBar2_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBar2.Scroll
        If Not ColorSender Then
            Dim nCMYK As CMYK = RGB2CMYK(TrackBar1.Value, TrackBar2.Value, TrackBar3.Value)
            Dim nHSL As HSL = RGB2HSL(TrackBar1.Value, TrackBar2.Value, TrackBar3.Value)
            If Not CheckBox6.Checked = False Then
                TextBox21.TextAlign = HorizontalAlignment.Center
                If CheckBox5.Checked Then
                    TextBox21.Text = cColor(TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel5.BackColor)
                Else
                    TextBox21.Text = cColor(TrackBar4.Value, TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel5.BackColor)
                End If
            Else
                If TextBox21.Text.Length > 0 Then
                    TextBox21.TextAlign = HorizontalAlignment.Left
                    If CheckBox5.Checked Then
                        TextBox21.Text = "#define " & TextBox20.Text & cColor(TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel5.BackColor)
                    Else
                        TextBox21.Text = "#define " & TextBox20.Text & "(" & cColor(TrackBar4.Value, TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel5.BackColor) & ")"
                    End If
                Else
                    CheckBox6.Checked = False
                    TextBox21.TextAlign = HorizontalAlignment.Center
                    If CheckBox5.Checked Then
                        TextBox21.Text = cColor(TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel5.BackColor)
                    Else
                        TextBox21.Text = cColor(TrackBar4.Value, TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel5.BackColor)
                    End If
                End If
            End If
            TextBox8.Text = TrackBar2.Value
            TrackBar5.Value = nCMYK.Cyan * 1000
            TextBox11.Text = nCMYK.Cyan
            TrackBar6.Value = nCMYK.Magenta * 1000
            TextBox12.Text = nCMYK.Magenta
            TrackBar7.Value = nCMYK.Yellow * 1000
            TextBox13.Text = nCMYK.Yellow
            TrackBar8.Value = nCMYK.Black * 1000
            TextBox14.Text = nCMYK.Black
            TrackBar9.Value = nHSL.Hue
            TextBox15.Text = nHSL.Hue
            TrackBar10.Value = nHSL.Saturation * 1000
            TextBox16.Text = nHSL.Saturation
            TrackBar11.Value = nHSL.Luminance * 1000
            TextBox17.Text = nHSL.Luminance
        End If
    End Sub

    Private Sub TrackBar3_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBar3.Scroll
        If Not ColorSender Then
            Dim nCMYK As CMYK = RGB2CMYK(TrackBar1.Value, TrackBar2.Value, TrackBar3.Value)
            Dim nHSL As HSL = RGB2HSL(TrackBar1.Value, TrackBar2.Value, TrackBar3.Value)
            If Not CheckBox6.Checked Then
                TextBox21.TextAlign = HorizontalAlignment.Center
                If CheckBox5.Checked Then
                    TextBox21.Text = cColor(TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel5.BackColor)
                Else
                    TextBox21.Text = cColor(TrackBar4.Value, TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel5.BackColor)
                End If
            Else
                If TextBox21.Text.Length > 0 Then
                    TextBox21.TextAlign = HorizontalAlignment.Left
                    If CheckBox5.Checked Then
                        TextBox21.Text = "#define " & TextBox20.Text & cColor(TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel5.BackColor)
                    Else
                        TextBox21.Text = "#define " & TextBox20.Text & "(" & cColor(TrackBar4.Value, TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel5.BackColor) & ")"
                    End If
                Else
                    CheckBox6.Checked = False
                    TextBox21.TextAlign = HorizontalAlignment.Center
                    If CheckBox5.Checked Then
                        TextBox21.Text = cColor(TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel5.BackColor)
                    Else
                        TextBox21.Text = cColor(TrackBar4.Value, TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel5.BackColor)
                    End If
                End If
            End If
            TextBox9.Text = TrackBar3.Value
            TrackBar5.Value = nCMYK.Cyan * 1000
            TextBox11.Text = nCMYK.Cyan
            TrackBar6.Value = nCMYK.Magenta * 1000
            TextBox12.Text = nCMYK.Magenta
            TrackBar7.Value = nCMYK.Yellow * 1000
            TextBox13.Text = nCMYK.Yellow
            TrackBar8.Value = nCMYK.Black * 1000
            TextBox14.Text = nCMYK.Black
            TrackBar9.Value = nHSL.Hue
            TextBox15.Text = nHSL.Hue
            TrackBar10.Value = nHSL.Saturation * 1000
            TextBox16.Text = nHSL.Saturation
            TrackBar11.Value = nHSL.Luminance * 1000
            TextBox17.Text = nHSL.Luminance
        End If
    End Sub

#End Region

#Region "Box"

    Private Sub textbox7_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox7.TextChanged
        If Not ColorSender Then
            TextBox7.Text = Regex.Replace(TextBox7.Text, BadChars, "")
            If Val(TextBox7.Text) <> TrackBar1.Value Then
                If Val(TextBox7.Text) > 255 Then TextBox7.Text = 255
                If Val(TextBox7.Text) < 0 Then TextBox7.Text = 0
                TrackBar1.Value = Val(TextBox7.Text)
                If Not CheckBox6.Checked Then
                    TextBox21.TextAlign = HorizontalAlignment.Center
                    If CheckBox5.Checked Then
                        TextBox21.Text = cColor(TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel5.BackColor)
                    Else
                        TextBox21.Text = cColor(TrackBar4.Value, TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel5.BackColor)
                    End If
                Else
                    If TextBox21.Text.Length > 0 Then
                        TextBox21.TextAlign = HorizontalAlignment.Left
                        If CheckBox5.Checked Then
                            TextBox21.Text = "#define " & TextBox20.Text & cColor(TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel5.BackColor)
                        Else
                            TextBox21.Text = "#define " & TextBox20.Text & "(" & cColor(TrackBar4.Value, TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel5.BackColor) & ")"
                        End If
                    Else
                        CheckBox6.Checked = False
                        TextBox21.TextAlign = HorizontalAlignment.Center
                        If CheckBox5.Checked Then
                            TextBox21.Text = cColor(TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel5.BackColor)
                        Else
                            TextBox21.Text = cColor(TrackBar4.Value, TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel5.BackColor)
                        End If
                    End If
                End If
                Dim nCMYK As CMYK = RGB2CMYK(TrackBar1.Value, TrackBar2.Value, TrackBar3.Value)
                Dim nHSL As HSL = RGB2HSL(TrackBar1.Value, TrackBar2.Value, TrackBar3.Value)
                TrackBar5.Value = nCMYK.Cyan * 1000
                TextBox11.Text = nCMYK.Cyan
                TrackBar6.Value = nCMYK.Magenta * 1000
                TextBox12.Text = nCMYK.Magenta
                TrackBar7.Value = nCMYK.Yellow * 1000
                TextBox13.Text = nCMYK.Yellow
                TrackBar8.Value = nCMYK.Black * 1000
                TextBox14.Text = nCMYK.Black
                TrackBar9.Value = nHSL.Hue
                TextBox15.Text = nHSL.Hue
                TrackBar10.Value = nHSL.Saturation * 1000
                TextBox16.Text = nHSL.Saturation
                TrackBar11.Value = nHSL.Luminance * 1000
                TextBox17.Text = nHSL.Luminance
            End If
        End If
    End Sub

    Private Sub textbox8_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox8.TextChanged
        If Not ColorSender Then
            TextBox8.Text = Regex.Replace(TextBox8.Text, BadChars, "")
            If Val(TextBox8.Text) <> TrackBar2.Value Then
                If Val(TextBox8.Text) > 255 Then TextBox8.Text = 255
                If Val(TextBox8.Text) < 0 Then TextBox8.Text = 0
                TrackBar2.Value = Val(TextBox8.Text)
                If Not CheckBox6.Checked Then
                    TextBox21.TextAlign = HorizontalAlignment.Center
                    If CheckBox5.Checked Then
                        TextBox21.Text = cColor(TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel5.BackColor)
                    Else
                        TextBox21.Text = cColor(TrackBar4.Value, TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel5.BackColor)
                    End If
                Else
                    If TextBox21.Text.Length > 0 Then
                        TextBox21.TextAlign = HorizontalAlignment.Left
                        If CheckBox5.Checked Then
                            TextBox21.Text = "#define " & TextBox20.Text & cColor(TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel5.BackColor)
                        Else
                            TextBox21.Text = "#define " & TextBox20.Text & "(" & cColor(TrackBar4.Value, TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel5.BackColor) & ")"
                        End If
                    Else
                        CheckBox6.Checked = False
                        TextBox21.TextAlign = HorizontalAlignment.Center
                        If CheckBox5.Checked Then
                            TextBox21.Text = cColor(TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel5.BackColor)
                        Else
                            TextBox21.Text = cColor(TrackBar4.Value, TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel5.BackColor)
                        End If
                    End If
                End If
                Dim nCMYK As CMYK = RGB2CMYK(TrackBar1.Value, TrackBar2.Value, TrackBar3.Value)
                Dim nHSL As HSL = RGB2HSL(TrackBar1.Value, TrackBar2.Value, TrackBar3.Value)
                TrackBar5.Value = nCMYK.Cyan * 1000
                TextBox11.Text = nCMYK.Cyan
                TrackBar6.Value = nCMYK.Magenta * 1000
                TextBox12.Text = nCMYK.Magenta
                TrackBar7.Value = nCMYK.Yellow * 1000
                TextBox13.Text = nCMYK.Yellow
                TrackBar8.Value = nCMYK.Black * 1000
                TextBox14.Text = nCMYK.Black
                TrackBar9.Value = nHSL.Hue
                TextBox15.Text = nHSL.Hue
                TrackBar10.Value = nHSL.Saturation * 1000
                TextBox16.Text = nHSL.Saturation
                TrackBar11.Value = nHSL.Luminance * 1000
                TextBox17.Text = nHSL.Luminance
            End If
        End If
    End Sub

    Private Sub textbox9_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox9.TextChanged
        If Not ColorSender Then
            TextBox9.Text = Regex.Replace(TextBox9.Text, BadChars, "")
            If Val(TextBox9.Text) <> TrackBar3.Value Then
                If Val(TextBox9.Text) > 255 Then TextBox9.Text = 255
                If Val(TextBox9.Text) < 0 Then TextBox9.Text = 0
                TrackBar3.Value = Val(TextBox9.Text)
                If Not CheckBox6.Checked Then
                    TextBox21.TextAlign = HorizontalAlignment.Center
                    If CheckBox5.Checked Then
                        TextBox21.Text = cColor(TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel5.BackColor)
                    Else
                        TextBox21.Text = cColor(TrackBar4.Value, TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel5.BackColor)
                    End If
                Else
                    If TextBox21.Text.Length > 0 Then
                        TextBox21.TextAlign = HorizontalAlignment.Left
                        If CheckBox5.Checked Then
                            TextBox21.Text = "#define " & TextBox20.Text & cColor(TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel5.BackColor)
                        Else
                            TextBox21.Text = "#define " & TextBox20.Text & "(" & cColor(TrackBar4.Value, TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel5.BackColor) & ")"
                        End If
                    Else
                        CheckBox6.Checked = False
                        TextBox21.TextAlign = HorizontalAlignment.Center
                        If CheckBox5.Checked Then
                            TextBox21.Text = cColor(TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel5.BackColor)
                        Else
                            TextBox21.Text = cColor(TrackBar4.Value, TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel5.BackColor)
                        End If
                    End If
                End If
                Dim nCMYK As CMYK = RGB2CMYK(TrackBar1.Value, TrackBar2.Value, TrackBar3.Value)
                Dim nHSL As HSL = RGB2HSL(TrackBar1.Value, TrackBar2.Value, TrackBar3.Value)
                TrackBar5.Value = nCMYK.Cyan * 1000
                TextBox11.Text = nCMYK.Cyan
                TrackBar6.Value = nCMYK.Magenta * 1000
                TextBox12.Text = nCMYK.Magenta
                TrackBar7.Value = nCMYK.Yellow * 1000
                TextBox13.Text = nCMYK.Yellow
                TrackBar8.Value = nCMYK.Black * 1000
                TextBox14.Text = nCMYK.Black
                TrackBar9.Value = nHSL.Hue
                TextBox15.Text = nHSL.Hue
                TrackBar10.Value = nHSL.Saturation * 1000
                TextBox16.Text = nHSL.Saturation
                TrackBar11.Value = nHSL.Luminance * 1000
                TextBox17.Text = nHSL.Luminance
            End If
        End If
    End Sub

#End Region

#End Region

#Region "CMYK"

#Region "Tracks"

    Private Sub TrackBar5_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBar5.Scroll
        TextBox11.Text = TrackBar5.Value / 1000
        If ColorSender = 1 Then
            Dim nRGB As RGB = CMYK2RGB(TextBox11.Text, TextBox12.Text, TextBox13.Text, TextBox14.Text)
            Dim nHSL As HSL = RGB2HSL(nRGB.Red, nRGB.Green, nRGB.Blue)
            If Not CheckBox6.Checked Then
                TextBox21.TextAlign = HorizontalAlignment.Center
                If CheckBox5.Checked Then
                    TextBox21.Text = cColor(nRGB.Red, nRGB.Green, nRGB.Blue, Panel5.BackColor)
                Else
                    TextBox21.Text = cColor(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue, Panel5.BackColor)
                End If
            Else
                If TextBox21.Text.Length > 0 Then
                    TextBox21.TextAlign = HorizontalAlignment.Left
                    If CheckBox5.Checked Then
                        TextBox21.Text = "#define " & TextBox20.Text & " " & cColor(nRGB.Red, nRGB.Green, nRGB.Blue, Panel5.BackColor)
                    Else
                        TextBox21.Text = "#define " & TextBox20.Text & " (" & cColor(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue) & ")"
                    End If
                Else
                    CheckBox6.Checked = False
                    TextBox21.TextAlign = HorizontalAlignment.Center
                    If CheckBox5.Checked Then
                        TextBox21.Text = cColor(nRGB.Red, nRGB.Green, nRGB.Blue, Panel5.BackColor)
                    Else
                        TextBox21.Text = cColor(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue, Panel5.BackColor)
                    End If
                End If
            End If
            TrackBar1.Value = nRGB.Red
            TextBox7.Text = nRGB.Red
            TrackBar2.Value = nRGB.Green
            TextBox8.Text = nRGB.Green
            TrackBar3.Value = nRGB.Blue
            TextBox9.Text = nRGB.Blue
            TrackBar9.Value = nHSL.Hue
            TextBox15.Text = nHSL.Hue
            TrackBar10.Value = nHSL.Saturation * 1000
            TextBox16.Text = nHSL.Saturation
            TrackBar11.Value = nHSL.Luminance * 1000
            TextBox17.Text = nHSL.Luminance
        End If
    End Sub

    Private Sub TrackBar6_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBar6.Scroll
        TextBox12.Text = TrackBar6.Value / 1000
        If ColorSender = 1 Then
            Dim nRGB As RGB = CMYK2RGB(TextBox11.Text, TextBox12.Text, TextBox13.Text, TextBox14.Text)
            Dim nHSL As HSL = RGB2HSL(nRGB.Red, nRGB.Green, nRGB.Blue)
            If Not CheckBox6.Checked Then
                TextBox21.TextAlign = HorizontalAlignment.Center
                If CheckBox5.Checked Then
                    TextBox21.Text = cColor(nRGB.Red, nRGB.Green, nRGB.Blue, Panel5.BackColor)
                Else
                    TextBox21.Text = cColor(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue, Panel5.BackColor)
                End If
            Else
                If TextBox21.Text.Length > 0 Then
                    TextBox21.TextAlign = HorizontalAlignment.Left
                    If CheckBox5.Checked Then
                        TextBox21.Text = "#define " & TextBox20.Text & " " & cColor(nRGB.Red, nRGB.Green, nRGB.Blue, Panel5.BackColor)
                    Else
                        TextBox21.Text = "#define " & TextBox20.Text & " (" & cColor(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue, Panel5.BackColor) & ")"
                    End If
                Else
                    CheckBox6.Checked = False
                    TextBox21.TextAlign = HorizontalAlignment.Center
                    If CheckBox5.Checked Then
                        TextBox21.Text = cColor(nRGB.Red, nRGB.Green, nRGB.Blue, Panel5.BackColor)
                    Else
                        TextBox21.Text = cColor(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue, Panel5.BackColor)
                    End If
                End If
            End If
            TrackBar1.Value = nRGB.Red
            TextBox7.Text = nRGB.Red
            TrackBar2.Value = nRGB.Green
            TextBox8.Text = nRGB.Green
            TrackBar3.Value = nRGB.Blue
            TextBox9.Text = nRGB.Blue
            TrackBar9.Value = nHSL.Hue
            TextBox15.Text = nHSL.Hue
            TrackBar10.Value = nHSL.Saturation * 1000
            TextBox16.Text = nHSL.Saturation
            TrackBar11.Value = nHSL.Luminance * 1000
            TextBox17.Text = nHSL.Luminance
        End If
    End Sub

    Private Sub TrackBar7_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBar7.Scroll
        TextBox13.Text = TrackBar7.Value / 1000
        If ColorSender = 1 Then
            Dim nRGB As RGB = CMYK2RGB(TextBox11.Text, TextBox12.Text, TextBox13.Text, TextBox14.Text)
            Dim nHSL As HSL = RGB2HSL(nRGB.Red, nRGB.Green, nRGB.Blue)
            If Not CheckBox6.Checked Then
                TextBox21.TextAlign = HorizontalAlignment.Center
                If CheckBox5.Checked Then
                    TextBox21.Text = cColor(nRGB.Red, nRGB.Green, nRGB.Blue, Panel5.BackColor)
                Else
                    TextBox21.Text = cColor(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue, Panel5.BackColor)
                End If
            Else
                If TextBox21.Text.Length > 0 Then
                    TextBox21.TextAlign = HorizontalAlignment.Left
                    If CheckBox5.Checked Then
                        TextBox21.Text = "#define " & TextBox20.Text & " " & cColor(nRGB.Red, nRGB.Green, nRGB.Blue, Panel5.BackColor)
                    Else
                        TextBox21.Text = "#define " & TextBox20.Text & " (" & cColor(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue, Panel5.BackColor) & ")"
                    End If
                Else
                    CheckBox6.Checked = False
                    TextBox21.TextAlign = HorizontalAlignment.Center
                    If CheckBox5.Checked Then
                        TextBox21.Text = cColor(nRGB.Red, nRGB.Green, nRGB.Blue, Panel5.BackColor)
                    Else
                        TextBox21.Text = cColor(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue, Panel5.BackColor)
                    End If
                End If
            End If
            TrackBar1.Value = nRGB.Red
            TextBox7.Text = nRGB.Red
            TrackBar2.Value = nRGB.Green
            TextBox8.Text = nRGB.Green
            TrackBar3.Value = nRGB.Blue
            TextBox9.Text = nRGB.Blue
            TrackBar9.Value = nHSL.Hue
            TextBox15.Text = nHSL.Hue
            TrackBar10.Value = nHSL.Saturation * 1000
            TextBox16.Text = nHSL.Saturation
            TrackBar11.Value = nHSL.Luminance * 1000
            TextBox17.Text = nHSL.Luminance
        End If
    End Sub

    Private Sub TrackBar8_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBar8.Scroll
        TextBox14.Text = TrackBar8.Value / 1000
        If ColorSender = 1 Then
            Dim nRGB As RGB = CMYK2RGB(TextBox11.Text, TextBox12.Text, TextBox13.Text, TextBox14.Text)
            Dim nHSL As HSL = RGB2HSL(nRGB.Red, nRGB.Green, nRGB.Blue)
            If Not CheckBox6.Checked Then
                TextBox21.TextAlign = HorizontalAlignment.Center
                If CheckBox5.Checked Then
                    TextBox21.Text = cColor(nRGB.Red, nRGB.Green, nRGB.Blue, Panel5.BackColor)
                Else
                    TextBox21.Text = cColor(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue, Panel5.BackColor)
                End If
            Else
                If TextBox21.Text.Length > 0 Then
                    TextBox21.TextAlign = HorizontalAlignment.Left
                    If CheckBox5.Checked Then
                        TextBox21.Text = "#define " & TextBox20.Text & " " & cColor(nRGB.Red, nRGB.Green, nRGB.Blue, Panel5.BackColor)
                    Else
                        TextBox21.Text = "#define " & TextBox20.Text & " (" & cColor(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue, Panel5.BackColor) & ")"
                    End If
                Else
                    CheckBox6.Checked = False
                    TextBox21.TextAlign = HorizontalAlignment.Center
                    If CheckBox5.Checked Then
                        TextBox21.Text = cColor(nRGB.Red, nRGB.Green, nRGB.Blue, Panel5.BackColor)
                    Else
                        TextBox21.Text = cColor(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue, Panel5.BackColor)
                    End If
                End If
            End If
            TrackBar1.Value = nRGB.Red
            TextBox7.Text = nRGB.Red
            TrackBar2.Value = nRGB.Green
            TextBox8.Text = nRGB.Green
            TrackBar3.Value = nRGB.Blue
            TextBox9.Text = nRGB.Blue
            TrackBar9.Value = nHSL.Hue
            TextBox15.Text = nHSL.Hue
            TrackBar10.Value = nHSL.Saturation * 1000
            TextBox16.Text = nHSL.Saturation
            TrackBar11.Value = nHSL.Luminance * 1000
            TextBox17.Text = nHSL.Luminance
        End If
    End Sub

#End Region

#Region "Box"

    Private Sub textbox11_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox11.TextChanged
        If ColorSender = 1 Then
            TextBox11.Text = Regex.Replace(TextBox11.Text, BadChars2, "")
            If Round(TextBox11.Text * 1000) <> TrackBar5.Value Then
                TextBox11.Text = CSng(TextBox11.Text)
                If TextBox11.Text > 1 Then TextBox11.Text = 1
                If TextBox11.Text < 0 Then TextBox11.Text = 0
                TrackBar5.Value = TextBox11.Text * 1000
                Dim nRGB As RGB = CMYK2RGB(TrackBar5.Value / 1000, TrackBar6.Value / 1000, TrackBar7.Value / 1000, TrackBar8.Value / 1000)
                Dim nHSL As HSL = RGB2HSL(nRGB.Red, nRGB.Green, nRGB.Blue)
                If Not CheckBox6.Checked Then
                    TextBox21.TextAlign = HorizontalAlignment.Center
                    If CheckBox5.Checked Then
                        TextBox21.Text = cColor(nRGB.Red, nRGB.Green, nRGB.Blue, Panel5.BackColor)
                    Else
                        TextBox21.Text = cColor(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue, Panel5.BackColor)
                    End If
                Else
                    If TextBox21.Text.Length > 0 Then
                        TextBox21.TextAlign = HorizontalAlignment.Left
                        If CheckBox5.Checked Then
                            TextBox21.Text = "#define " & TextBox20.Text & " " & cColor(nRGB.Red, nRGB.Green, nRGB.Blue, Panel5.BackColor)
                        Else
                            TextBox21.Text = "#define " & TextBox20.Text & " (" & cColor(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue, Panel5.BackColor) & ")"
                        End If
                    Else
                        CheckBox6.Checked = False
                        TextBox21.TextAlign = HorizontalAlignment.Center
                        If CheckBox5.Checked Then
                            TextBox21.Text = cColor(nRGB.Red, nRGB.Green, nRGB.Blue, Panel5.BackColor)
                        Else
                            TextBox21.Text = cColor(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue, Panel5.BackColor)
                        End If
                    End If
                End If
                TrackBar1.Value = nRGB.Red
                TextBox7.Text = nRGB.Red
                TrackBar2.Value = nRGB.Green
                TextBox8.Text = nRGB.Green
                TrackBar3.Value = nRGB.Blue
                TextBox9.Text = nRGB.Blue
                TrackBar9.Value = nHSL.Hue
                TextBox15.Text = nHSL.Hue
                TrackBar10.Value = nHSL.Saturation * 1000
                TextBox16.Text = nHSL.Saturation
                TrackBar11.Value = nHSL.Luminance * 1000
                TextBox17.Text = nHSL.Luminance
            End If
        End If
    End Sub

    Private Sub textbox12_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox12.TextChanged
        If ColorSender = 1 Then
            TextBox12.Text = Regex.Replace(TextBox12.Text, BadChars2, "")
            If Round(TextBox12.Text * 1000) <> TrackBar6.Value Then
                TextBox12.Text = CSng(TextBox12.Text)
                If TextBox12.Text > 1 Then TextBox12.Text = 1
                If TextBox12.Text < 0 Then TextBox12.Text = 0
                TrackBar6.Value = TextBox12.Text * 1000
                Dim nRGB As RGB = CMYK2RGB(TrackBar5.Value / 1000, TrackBar6.Value / 1000, TrackBar7.Value / 1000, TrackBar8.Value / 1000)
                Dim nHSL As HSL = RGB2HSL(nRGB.Red, nRGB.Green, nRGB.Blue)
                If Not CheckBox6.Checked Then
                    TextBox21.TextAlign = HorizontalAlignment.Center
                    If CheckBox5.Checked Then
                        TextBox21.Text = cColor(nRGB.Red, nRGB.Green, nRGB.Blue, Panel5.BackColor)
                    Else
                        TextBox21.Text = cColor(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue, Panel5.BackColor)
                    End If
                Else
                    If TextBox21.Text.Length > 0 Then
                        TextBox21.TextAlign = HorizontalAlignment.Left
                        If CheckBox5.Checked Then
                            TextBox21.Text = "#define " & TextBox20.Text & " " & cColor(nRGB.Red, nRGB.Green, nRGB.Blue, Panel5.BackColor)
                        Else
                            TextBox21.Text = "#define " & TextBox20.Text & " (" & cColor(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue, Panel5.BackColor) & ")"
                        End If
                    Else
                        CheckBox6.Checked = False
                        TextBox21.TextAlign = HorizontalAlignment.Center
                        If CheckBox5.Checked Then
                            TextBox21.Text = cColor(nRGB.Red, nRGB.Green, nRGB.Blue, Panel5.BackColor)
                        Else
                            TextBox21.Text = cColor(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue, Panel5.BackColor)
                        End If
                    End If
                End If
                TrackBar1.Value = nRGB.Red
                TextBox7.Text = nRGB.Red
                TrackBar2.Value = nRGB.Green
                TextBox8.Text = nRGB.Green
                TrackBar3.Value = nRGB.Blue
                TextBox9.Text = nRGB.Blue
                TrackBar9.Value = nHSL.Hue
                TextBox15.Text = nHSL.Hue
                TrackBar10.Value = nHSL.Saturation * 1000
                TextBox16.Text = nHSL.Saturation
                TrackBar11.Value = nHSL.Luminance * 1000
                TextBox17.Text = nHSL.Luminance
            End If
        End If
    End Sub

    Private Sub textbox13_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox13.TextChanged
        If ColorSender = 1 Then
            TextBox13.Text = Regex.Replace(TextBox13.Text, BadChars2, "")
            If Round(TextBox13.Text * 1000) <> TrackBar7.Value Then
                TextBox13.Text = CSng(TextBox13.Text)
                If TextBox13.Text > 1 Then TextBox13.Text = 1
                If TextBox13.Text < 0 Then TextBox13.Text = 0
                TrackBar7.Value = TextBox13.Text * 1000
                Dim nRGB As RGB = CMYK2RGB(TrackBar5.Value / 1000, TrackBar6.Value / 1000, TrackBar7.Value / 1000, TrackBar8.Value / 1000)
                Dim nHSL As HSL = RGB2HSL(nRGB.Red, nRGB.Green, nRGB.Blue)
                If Not CheckBox6.Checked Then
                    TextBox21.TextAlign = HorizontalAlignment.Center
                    If CheckBox5.Checked Then
                        TextBox21.Text = cColor(nRGB.Red, nRGB.Green, nRGB.Blue, Panel5.BackColor)
                    Else
                        TextBox21.Text = cColor(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue, Panel5.BackColor)
                    End If
                Else
                    If TextBox21.Text.Length > 0 Then
                        TextBox21.TextAlign = HorizontalAlignment.Left
                        If CheckBox5.Checked Then
                            TextBox21.Text = "#define " & TextBox20.Text & " " & cColor(nRGB.Red, nRGB.Green, nRGB.Blue, Panel5.BackColor)
                        Else
                            TextBox21.Text = "#define " & TextBox20.Text & " (" & cColor(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue, Panel5.BackColor) & ")"
                        End If
                    Else
                        CheckBox6.Checked = False
                        TextBox21.TextAlign = HorizontalAlignment.Center
                        If CheckBox5.Checked Then
                            TextBox21.Text = cColor(nRGB.Red, nRGB.Green, nRGB.Blue, Panel5.BackColor)
                        Else
                            TextBox21.Text = cColor(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue, Panel5.BackColor)
                        End If
                    End If
                End If
                TrackBar1.Value = nRGB.Red
                TextBox7.Text = nRGB.Red
                TrackBar2.Value = nRGB.Green
                TextBox8.Text = nRGB.Green
                TrackBar3.Value = nRGB.Blue
                TextBox9.Text = nRGB.Blue
                TrackBar9.Value = nHSL.Hue
                TextBox15.Text = nHSL.Hue
                TrackBar10.Value = nHSL.Saturation * 1000
                TextBox16.Text = nHSL.Saturation
                TrackBar11.Value = nHSL.Luminance * 1000
                TextBox17.Text = nHSL.Luminance
            End If
        End If
    End Sub

    Private Sub textbox14_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox14.TextChanged
        If ColorSender = 1 Then
            TextBox14.Text = Regex.Replace(TextBox14.Text, BadChars2, "")
            If Round(TextBox14.Text * 1000) <> TrackBar8.Value Then
                TextBox14.Text = CSng(TextBox14.Text)
                If TextBox14.Text > 1 Then TextBox14.Text = 1
                If TextBox14.Text < 0 Then TextBox14.Text = 0
                TrackBar8.Value = TextBox14.Text * 1000
                Dim nRGB As RGB = CMYK2RGB(TrackBar5.Value / 1000, TrackBar6.Value / 1000, TrackBar7.Value / 1000, TrackBar8.Value / 1000)
                Dim nHSL As HSL = RGB2HSL(nRGB.Red, nRGB.Green, nRGB.Blue)
                If Not CheckBox6.Checked Then
                    TextBox21.TextAlign = HorizontalAlignment.Center
                    If CheckBox5.Checked Then
                        TextBox21.Text = cColor(nRGB.Red, nRGB.Green, nRGB.Blue, Panel5.BackColor)
                    Else
                        TextBox21.Text = cColor(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue, Panel5.BackColor)
                    End If
                Else
                    If TextBox21.Text.Length > 0 Then
                        TextBox21.TextAlign = HorizontalAlignment.Left
                        If CheckBox5.Checked Then
                            TextBox21.Text = "#define " & TextBox20.Text & " " & cColor(nRGB.Red, nRGB.Green, nRGB.Blue, Panel5.BackColor)
                        Else
                            TextBox21.Text = "#define " & TextBox20.Text & " (" & cColor(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue, Panel5.BackColor) & ")"
                        End If
                    Else
                        CheckBox6.Checked = False
                        TextBox21.TextAlign = HorizontalAlignment.Center
                        If CheckBox5.Checked Then
                            TextBox21.Text = cColor(nRGB.Red, nRGB.Green, nRGB.Blue, Panel5.BackColor)
                        Else
                            TextBox21.Text = cColor(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue, Panel5.BackColor)
                        End If
                    End If
                End If
                TrackBar1.Value = nRGB.Red
                TextBox7.Text = nRGB.Red
                TrackBar2.Value = nRGB.Green
                TextBox8.Text = nRGB.Green
                TrackBar3.Value = nRGB.Blue
                TextBox9.Text = nRGB.Blue
                TrackBar9.Value = nHSL.Hue
                TextBox15.Text = nHSL.Hue
                TrackBar10.Value = nHSL.Saturation * 1000
                TextBox16.Text = nHSL.Saturation
                TrackBar11.Value = nHSL.Luminance * 1000
                TextBox17.Text = nHSL.Luminance
            End If
        End If
    End Sub

#End Region

#End Region

#Region "HSL"

#Region "Tracks"

    Private Sub TrackBar9_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBar9.Scroll
        If ColorSender = 2 Then
            TextBox15.Text = TrackBar9.Value
            Dim nRGB As RGB = HSL2RGB(TrackBar9.Value, TextBox16.Text, TextBox17.Text)
            Dim nCMYK As CMYK = RGB2CMYK(nRGB.Red, nRGB.Green, nRGB.Blue)
            If Not CheckBox6.Checked Then
                TextBox21.TextAlign = HorizontalAlignment.Center
                If CheckBox5.Checked Then
                    TextBox21.Text = cColor(nRGB.Red, nRGB.Green, nRGB.Blue, Panel5.BackColor)
                Else
                    TextBox21.Text = cColor(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue, Panel5.BackColor)
                End If
            Else
                If TextBox21.Text.Length > 0 Then
                    TextBox21.TextAlign = HorizontalAlignment.Left
                    If CheckBox5.Checked Then
                        TextBox21.Text = "#define " & TextBox20.Text & " " & cColor(nRGB.Red, nRGB.Green, nRGB.Blue, Panel5.BackColor)
                    Else
                        TextBox21.Text = "#define " & TextBox20.Text & " (" & cColor(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue) & ")"
                    End If
                Else
                    CheckBox6.Checked = False
                    TextBox21.TextAlign = HorizontalAlignment.Center
                    If CheckBox5.Checked Then
                        TextBox21.Text = cColor(nRGB.Red, nRGB.Green, nRGB.Blue, Panel5.BackColor)
                    Else
                        TextBox21.Text = cColor(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue, Panel5.BackColor)
                    End If
                End If
            End If
            TrackBar1.Value = nRGB.Red
            TextBox7.Text = nRGB.Red
            TrackBar2.Value = nRGB.Green
            TextBox8.Text = nRGB.Green
            TrackBar3.Value = nRGB.Blue
            TextBox9.Text = nRGB.Blue
            TextBox7.Text = TrackBar1.Value
            TrackBar5.Value = nCMYK.Cyan * 1000
            TextBox11.Text = nCMYK.Cyan
            TrackBar6.Value = nCMYK.Magenta * 1000
            TextBox12.Text = nCMYK.Magenta
            TrackBar7.Value = nCMYK.Yellow * 1000
            TextBox13.Text = nCMYK.Yellow
            TrackBar8.Value = nCMYK.Black * 1000
            TextBox14.Text = nCMYK.Black
        End If
    End Sub

    Private Sub TrackBar10_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBar10.Scroll
        TextBox16.Text = TrackBar10.Value / 1000
        If ColorSender = 2 Then
            Dim nRGB As RGB = HSL2RGB(TrackBar9.Value, TextBox16.Text, TextBox17.Text)
            Dim nCMYK As CMYK = RGB2CMYK(nRGB.Red, nRGB.Green, nRGB.Blue)
            Panel5.BackColor = Color.FromArgb(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue)
            If Not CheckBox6.Checked Then
                TextBox21.TextAlign = HorizontalAlignment.Center
                If CheckBox5.Checked Then
                    TextBox21.Text = cColor(nRGB.Red, nRGB.Green, nRGB.Blue, Panel5.BackColor)
                Else
                    TextBox21.Text = cColor(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue, Panel5.BackColor)
                End If
            Else
                If TextBox21.Text.Length > 0 Then
                    TextBox21.TextAlign = HorizontalAlignment.Left
                    If CheckBox5.Checked Then
                        TextBox21.Text = "#define " & TextBox20.Text & " " & cColor(nRGB.Red, nRGB.Green, nRGB.Blue, Panel5.BackColor)
                    Else
                        TextBox21.Text = "#define " & TextBox20.Text & " (" & cColor(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue) & ")"
                    End If
                Else
                    CheckBox6.Checked = False
                    TextBox21.TextAlign = HorizontalAlignment.Center
                    If CheckBox5.Checked Then
                        TextBox21.Text = cColor(nRGB.Red, nRGB.Green, nRGB.Blue, Panel5.BackColor)
                    Else
                        TextBox21.Text = cColor(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue, Panel5.BackColor)
                    End If
                End If
            End If
            TrackBar1.Value = nRGB.Red
            TextBox7.Text = nRGB.Red
            TrackBar2.Value = nRGB.Green
            TextBox8.Text = nRGB.Green
            TrackBar3.Value = nRGB.Blue
            TextBox9.Text = nRGB.Blue
            TextBox7.Text = TrackBar1.Value
            TrackBar5.Value = nCMYK.Cyan * 1000
            TextBox11.Text = nCMYK.Cyan
            TrackBar6.Value = nCMYK.Magenta * 1000
            TextBox12.Text = nCMYK.Magenta
            TrackBar7.Value = nCMYK.Yellow * 1000
            TextBox13.Text = nCMYK.Yellow
            TrackBar8.Value = nCMYK.Black * 1000
            TextBox14.Text = nCMYK.Black
        End If
    End Sub

    Private Sub TrackBar11_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBar11.Scroll
        TextBox17.Text = TrackBar11.Value / 1000
        If ColorSender = 2 Then
            Dim nRGB As RGB = HSL2RGB(TrackBar9.Value, TextBox16.Text, TextBox17.Text)
            Dim nCMYK As CMYK = RGB2CMYK(nRGB.Red, nRGB.Green, nRGB.Blue)
            Panel5.BackColor = Color.FromArgb(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue)
            If Not CheckBox6.Checked Then
                TextBox21.TextAlign = HorizontalAlignment.Center
                If CheckBox5.Checked Then
                    TextBox21.Text = cColor(nRGB.Red, nRGB.Green, nRGB.Blue, Panel5.BackColor)
                Else
                    TextBox21.Text = cColor(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue, Panel5.BackColor)
                End If
            Else
                If TextBox21.Text.Length > 0 Then
                    TextBox21.TextAlign = HorizontalAlignment.Left
                    If CheckBox5.Checked Then
                        TextBox21.Text = "#define " & TextBox20.Text & " " & cColor(nRGB.Red, nRGB.Green, nRGB.Blue, Panel5.BackColor)
                    Else
                        TextBox21.Text = "#define " & TextBox20.Text & " (" & cColor(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue) & ")"
                    End If
                Else
                    CheckBox6.Checked = False
                    TextBox21.TextAlign = HorizontalAlignment.Center
                    If CheckBox5.Checked Then
                        TextBox21.Text = cColor(nRGB.Red, nRGB.Green, nRGB.Blue, Panel5.BackColor)
                    Else
                        TextBox21.Text = cColor(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue, Panel5.BackColor)
                    End If
                End If
            End If
            TrackBar1.Value = nRGB.Red
            TextBox7.Text = nRGB.Red
            TrackBar2.Value = nRGB.Green
            TextBox8.Text = nRGB.Green
            TrackBar3.Value = nRGB.Blue
            TextBox9.Text = nRGB.Blue
            TextBox7.Text = TrackBar1.Value
            TrackBar5.Value = nCMYK.Cyan * 1000
            TextBox11.Text = nCMYK.Cyan
            TrackBar6.Value = nCMYK.Magenta * 1000
            TextBox12.Text = nCMYK.Magenta
            TrackBar7.Value = nCMYK.Yellow * 1000
            TextBox13.Text = nCMYK.Yellow
            TrackBar8.Value = nCMYK.Black * 1000
            TextBox14.Text = nCMYK.Black
        End If
    End Sub

#End Region

#Region "Box"

    Private Sub textbox15_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox15.TextChanged
        If ColorSender = 2 Then
            TextBox15.Text = Regex.Replace(TextBox15.Text, BadChars2, "")
            If TextBox15.Text <> TrackBar9.Value Then
                If TextBox15.Text > 360 Then TextBox15.Text = 360
                If TextBox15.Text < 0 Then TextBox15.Text = 0
                TrackBar9.Value = TextBox15.Text
                Dim nRGB As RGB = HSL2RGB(TrackBar9.Value, TextBox16.Text, TextBox17.Text)
                Dim nCMYK As CMYK = RGB2CMYK(nRGB.Red, nRGB.Green, nRGB.Blue)
                Panel5.BackColor = Color.FromArgb(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue)
                If Not CheckBox6.Checked Then
                    TextBox21.TextAlign = HorizontalAlignment.Center
                    If CheckBox5.Checked Then
                        TextBox21.Text = cColor(nRGB.Red, nRGB.Green, nRGB.Blue, Panel5.BackColor)
                    Else
                        TextBox21.Text = cColor(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue, Panel5.BackColor)
                    End If
                Else
                    If TextBox21.Text.Length > 0 Then
                        TextBox21.TextAlign = HorizontalAlignment.Left
                        If CheckBox5.Checked Then
                            TextBox21.Text = "#define " & TextBox20.Text & " " & cColor(nRGB.Red, nRGB.Green, nRGB.Blue, Panel5.BackColor)
                        Else
                            TextBox21.Text = "#define " & TextBox20.Text & " (" & cColor(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue) & ")"
                        End If
                    Else
                        CheckBox6.Checked = False
                        TextBox21.TextAlign = HorizontalAlignment.Center
                        If CheckBox5.Checked Then
                            TextBox21.Text = cColor(nRGB.Red, nRGB.Green, nRGB.Blue, Panel5.BackColor)
                        Else
                            TextBox21.Text = cColor(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue, Panel5.BackColor)
                        End If
                    End If
                End If
                TrackBar1.Value = nRGB.Red
                TextBox7.Text = nRGB.Red
                TrackBar2.Value = nRGB.Green
                TextBox8.Text = nRGB.Green
                TrackBar3.Value = nRGB.Blue
                TextBox9.Text = nRGB.Blue
                TextBox7.Text = TrackBar1.Value
                TrackBar5.Value = nCMYK.Cyan * 1000
                TextBox11.Text = nCMYK.Cyan
                TrackBar6.Value = nCMYK.Magenta * 1000
                TextBox12.Text = nCMYK.Magenta
                TrackBar7.Value = nCMYK.Yellow * 1000
                TextBox13.Text = nCMYK.Yellow
                TrackBar8.Value = nCMYK.Black * 1000
                TextBox14.Text = nCMYK.Black
            End If
        End If
    End Sub

    Private Sub textbox16_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox16.TextChanged
        If ColorSender = 2 Then
            TextBox16.Text = Regex.Replace(TextBox16.Text, BadChars2, "")
            If Round(TextBox16.Text * 1000) <> TrackBar10.Value Then
                TextBox16.Text = CSng(TextBox16.Text)
                If TextBox16.Text > 1 Then TextBox16.Text = 1
                If TextBox16.Text < 0 Then TextBox16.Text = 0
                TrackBar10.Value = TextBox16.Text * 1000
                Dim nRGB As RGB = HSL2RGB(TrackBar9.Value, TextBox16.Text, TextBox17.Text)
                Dim nCMYK As CMYK = RGB2CMYK(nRGB.Red, nRGB.Green, nRGB.Blue)
                Panel5.BackColor = Color.FromArgb(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue)
                If Not CheckBox6.Checked Then
                    TextBox21.TextAlign = HorizontalAlignment.Center
                    If CheckBox5.Checked Then
                        TextBox21.Text = cColor(nRGB.Red, nRGB.Green, nRGB.Blue, Panel5.BackColor)
                    Else
                        TextBox21.Text = cColor(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue, Panel5.BackColor)
                    End If
                Else
                    If TextBox21.Text.Length > 0 Then
                        TextBox21.TextAlign = HorizontalAlignment.Left
                        If CheckBox5.Checked Then
                            TextBox21.Text = "#define " & TextBox20.Text & " " & cColor(nRGB.Red, nRGB.Green, nRGB.Blue, Panel5.BackColor)
                        Else
                            TextBox21.Text = "#define " & TextBox20.Text & " (" & cColor(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue) & ")"
                        End If
                    Else
                        CheckBox6.Checked = False
                        TextBox21.TextAlign = HorizontalAlignment.Center
                        If CheckBox5.Checked Then
                            TextBox21.Text = cColor(nRGB.Red, nRGB.Green, nRGB.Blue, Panel5.BackColor)
                        Else
                            TextBox21.Text = cColor(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue, Panel5.BackColor)
                        End If
                    End If
                End If
                TrackBar1.Value = nRGB.Red
                TextBox7.Text = nRGB.Red
                TrackBar2.Value = nRGB.Green
                TextBox8.Text = nRGB.Green
                TrackBar3.Value = nRGB.Blue
                TextBox9.Text = nRGB.Blue
                TextBox7.Text = TrackBar1.Value
                TrackBar5.Value = nCMYK.Cyan * 1000
                TextBox11.Text = nCMYK.Cyan
                TrackBar6.Value = nCMYK.Magenta * 1000
                TextBox12.Text = nCMYK.Magenta
                TrackBar7.Value = nCMYK.Yellow * 1000
                TextBox13.Text = nCMYK.Yellow
                TrackBar8.Value = nCMYK.Black * 1000
                TextBox14.Text = nCMYK.Black
            End If
        End If
    End Sub

    Private Sub textbox17_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox17.TextChanged
        If ColorSender = 2 Then
            TextBox17.Text = Regex.Replace(TextBox17.Text, BadChars2, "")
            If Round(TextBox17.Text * 1000) <> TrackBar11.Value Then
                TextBox17.Text = CSng(TextBox17.Text)
                If TextBox17.Text > 1 Then TextBox17.Text = 1
                If TextBox17.Text < 0 Then TextBox17.Text = 0
                TrackBar11.Value = TextBox17.Text * 1000
                Dim nRGB As RGB = HSL2RGB(TrackBar9.Value, TextBox16.Text, TextBox17.Text)
                Dim nCMYK As CMYK = RGB2CMYK(nRGB.Red, nRGB.Green, nRGB.Blue)
                Panel5.BackColor = Color.FromArgb(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue)
                If Not CheckBox6.Checked Then
                    TextBox21.TextAlign = HorizontalAlignment.Center
                    If CheckBox5.Checked Then
                        TextBox21.Text = cColor(nRGB.Red, nRGB.Green, nRGB.Blue, Panel5.BackColor)
                    Else
                        TextBox21.Text = cColor(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue, Panel5.BackColor)
                    End If
                Else
                    If TextBox21.Text.Length > 0 Then
                        TextBox21.TextAlign = HorizontalAlignment.Left
                        If CheckBox5.Checked Then
                            TextBox21.Text = "#define " & TextBox20.Text & " " & cColor(nRGB.Red, nRGB.Green, nRGB.Blue, Panel5.BackColor)
                        Else
                            TextBox21.Text = "#define " & TextBox20.Text & " (" & cColor(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue) & ")"
                        End If
                    Else
                        CheckBox6.Checked = False
                        TextBox21.TextAlign = HorizontalAlignment.Center
                        If CheckBox5.Checked Then
                            TextBox21.Text = cColor(nRGB.Red, nRGB.Green, nRGB.Blue, Panel5.BackColor)
                        Else
                            TextBox21.Text = cColor(TrackBar4.Value, nRGB.Red, nRGB.Green, nRGB.Blue, Panel5.BackColor)
                        End If
                    End If
                End If
                TrackBar1.Value = nRGB.Red
                TextBox7.Text = nRGB.Red
                TrackBar2.Value = nRGB.Green
                TextBox8.Text = nRGB.Green
                TrackBar3.Value = nRGB.Blue
                TextBox9.Text = nRGB.Blue
                TextBox7.Text = TrackBar1.Value
                TrackBar5.Value = nCMYK.Cyan * 1000
                TextBox11.Text = nCMYK.Cyan
                TrackBar6.Value = nCMYK.Magenta * 1000
                TextBox12.Text = nCMYK.Magenta
                TrackBar7.Value = nCMYK.Yellow * 1000
                TextBox13.Text = nCMYK.Yellow
                TrackBar8.Value = nCMYK.Black * 1000
                TextBox14.Text = nCMYK.Black
            End If
        End If
    End Sub

#End Region

#End Region

#Region "Alpha"

#Region "Track"

    Private Sub TrackBar4_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBar4.Scroll
        If Not CheckBox6.Checked Then
            TextBox21.TextAlign = HorizontalAlignment.Center
            If CheckBox5.Checked Then
                TextBox21.Text = cColor(TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel5.BackColor)
            Else
                TextBox21.Text = cColor(TrackBar4.Value, TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel5.BackColor)
            End If
        Else
            If TextBox21.Text.Length > 0 Then
                TextBox21.TextAlign = HorizontalAlignment.Left
                If CheckBox5.Checked Then
                    TextBox21.Text = "#define " & TextBox20.Text & cColor(TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel5.BackColor)
                Else
                    TextBox21.Text = "#define " & TextBox20.Text & "(" & cColor(TrackBar4.Value, TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel5.BackColor) & ")"
                End If
            Else
                CheckBox6.Checked = False
                TextBox21.TextAlign = HorizontalAlignment.Center
                If CheckBox5.Checked Then
                    TextBox21.Text = cColor(TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel5.BackColor)
                Else
                    TextBox21.Text = cColor(TrackBar4.Value, TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel5.BackColor)
                End If
            End If
        End If
        TextBox10.Text = TrackBar4.Value
    End Sub

#End Region

#Region "Box"

    Private Sub textbox10_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox10.TextAlignChanged
        TextBox10.Text = Regex.Replace(TextBox10.Text, BadChars, "")
        If Val(TextBox10.Text) <> TrackBar4.Value Then
            If Val(TextBox10.Text) > 255 Then TextBox10.Text = 255
            If Val(TextBox10.Text) < 0 Then TextBox10.Text = 0
            TrackBar4.Value = Val(TextBox10.Text)
            If Not CheckBox6.Checked Then
                TextBox21.TextAlign = HorizontalAlignment.Center
                If CheckBox5.Checked Then
                    TextBox21.Text = cColor(TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel5.BackColor)
                Else
                    TextBox21.Text = cColor(TrackBar4.Value, TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel5.BackColor)
                End If
            Else
                If TextBox21.Text.Length > 0 Then
                    TextBox21.TextAlign = HorizontalAlignment.Left
                    If CheckBox5.Checked Then
                        TextBox21.Text = "#define " & TextBox20.Text & cColor(TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel5.BackColor)
                    Else
                        TextBox21.Text = "#define " & TextBox20.Text & "(" & cColor(TrackBar4.Value, TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel5.BackColor) & ")"
                    End If
                Else
                    CheckBox6.Checked = False
                    TextBox21.TextAlign = HorizontalAlignment.Center
                    If CheckBox5.Checked Then
                        TextBox21.Text = cColor(TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel5.BackColor)
                    Else
                        TextBox21.Text = cColor(TrackBar4.Value, TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel5.BackColor)
                    End If
                End If
            End If
        End If
    End Sub

#End Region

#End Region

#End Region

#Region "Extra"

    Private Sub TextBox20_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox20.TextChanged
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

    Private Sub CheckBox5_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox5.CheckedChanged
        If CheckBox5.Checked Then
            Label11.Enabled = False
            TrackBar4.Enabled = False
            TextBox10.Enabled = False
        Else
            Label11.Enabled = True
            TrackBar4.Enabled = True
            TextBox10.Enabled = True
        End If
        Select Case TabControl2.SelectedIndex
            Case 0
                TrackBar1_Scroll(sender, e)
            Case 1
                TrackBar5_Scroll(sender, e)
            Case 2
                TrackBar9_Scroll(sender, e)
        End Select
    End Sub

#End Region

#End Region

#Region "Areas"

#Region "Arrays"

    Dim Selection As Area, aCount As Integer, Areas As String, pAreas As String

#End Region

#Region "Drawing"

    Private Sub PictureBox6_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PictureBox6.MouseDown
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
                    PictureBox6.Image = My.Resources.Map
                End If
        End Select
    End Sub

    Private Sub PictureBox6_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PictureBox6.MouseMove
        If Selection.Lock = True Then
            With Selection
                If e.Location.X < 0 Then
                    .minX = 0
                ElseIf e.Location.Y < 0 Then
                    .minY = 0
                ElseIf PictureBox6.Width < e.Location.X Then
                    .maxX = PictureBox6.Width - 1
                    If PictureBox6.Height > e.Location.Y Then
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
                ElseIf PictureBox6.Height < e.Location.Y Then
                    .maxY = PictureBox6.Height - 1
                    If PictureBox6.Width > e.Location.X Then
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
                TextBox22.Text = Round(6000 / TrackBar14.Value * .minX - 3000, 6)
                TextBox23.Text = Round(6000 / TrackBar14.Value * .minY - 3000, 6)
                If .maxX = PictureBox6.Width - 1 Then
                    TextBox24.Text = 3000
                Else
                    TextBox24.Text = Round(6000 / TrackBar14.Value * .maxX - 3000, 6)
                End If
                If .maxY = PictureBox6.Height - 1 Then
                    TextBox25.Text = 3000
                Else
                    TextBox25.Text = Round(6000 / TrackBar14.Value * .maxY - 3000, 6)
                End If
                TextBox22.Text = TextBox22.Text.Replace(",", ".")
                TextBox23.Text = TextBox23.Text.Replace(",", ".")
                TextBox24.Text = TextBox24.Text.Replace(",", ".")
                TextBox25.Text = TextBox25.Text.Replace(",", ".")
                PictureBox6.Refresh()
                PictureBox6.CreateGraphics.DrawRectangle(New Pen(Settings.C_Area.Hex), New Rectangle(.minX, .minY, .maxX - .minX, .maxY - .minY))
                PictureBox6.CreateGraphics.Dispose()
            End With
        End If
    End Sub

    Private Sub PictureBox6_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PictureBox6.MouseUp
        If Selection.Lock = True Then
            Dim g As Graphics, tmp As Single
            g = Graphics.FromImage(PictureBox6.Image)
            tmp = 2048 / TrackBar14.Value
            If CheckBox8.Checked Then
                If RadioButton18.Checked Then
                    Areas += String.Format("Area[{4}] = " & Settings.AreaCreateOutput & vbNewLine & Settings.AreaShowOutput & vbNewLine, TextBox22.Text, TextBox23.Text, TextBox24.Text, TextBox25.Text, aCount, "Area[" & aCount & "]", Settings.C_Area.Name)
                Else
                    Areas += String.Format(Settings.BoundsOutput & vbNewLine, TextBox22.Text, TextBox24.Text, TextBox23.Text, TextBox25.Text)
                End If
                pAreas += Selection.minX & "|,|" & Selection.minY & "|,|" & Selection.maxX & "|,|" & Selection.maxY & "|;|"
                aCount += 1
            End If
            If Not CheckBox10.Checked Then
                g.DrawRectangle(New Pen(Settings.C_Area.Hex), Selection.minX * tmp, Selection.minY * tmp, (Selection.maxX - Selection.minX) * tmp, (Selection.maxY - Selection.minY) * tmp)
            Else
                g.FillRectangle(New SolidBrush(Settings.C_Area.Hex), Selection.minX * tmp, Selection.minY * tmp, (Selection.maxX - Selection.minX) * tmp, (Selection.maxY - Selection.minY) * tmp)
            End If
            g.Dispose()
            PictureBox6.Refresh()
            Selection.Lock = False
        End If
    End Sub

#End Region

#Region "Buttons"

    Private Sub Button15_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button15.Click
        If CheckBox8.Checked = False Then
            If TextBox22.Text.Length = 0 OrElse TextBox23.Text.Length = 0 OrElse TextBox24.Text.Length = 0 OrElse TextBox25.Text.Length = 0 Then
                Select Case Settings.Language
                    Case Languages.English
                        MsgBox("You must select an area first.", MsgBoxStyle.Critical, "Error")
                    Case Languages.Español
                        MsgBox("Debes seleccionar un area primero.", MsgBoxStyle.Critical, "Error")
                    Case Languages.Portuguêse
                        MsgBox("Você deve selecionar uma área em primeiro lugar.", MsgBoxStyle.Critical, "Error")
                    Case Else
                        MsgBox("Du musst zuerst ein Gebiet auswählen.", MsgBoxStyle.Critical, "Error")
                End Select
                Exit Sub
            End If
            Try
                If RadioButton18.Checked Then
                    Instances(Main.TabControl1.SelectedIndex).SyntaxHandle.Selection.Text = "new Area;" & vbNewLine
                    Instances(Main.TabControl1.SelectedIndex).SyntaxHandle.Selection.Text = String.Format("Area = " & Settings.AreaCreateOutput & vbNewLine & Settings.AreaShowOutput, TextBox22.Text, TextBox23.Text, TextBox24.Text, TextBox25.Text, "Area", Settings.C_Area.Name) & vbNewLine
                Else
                    Instances(Main.TabControl1.SelectedIndex).SyntaxHandle.Selection.Text = String.Format(Settings.BoundsOutput & vbNewLine, TextBox22.Text, TextBox24.Text, TextBox23.Text, TextBox25.Text)
                End If
            Catch ex As Exception
                Select Case Settings.Language
                    Case Languages.English
                        MsgBox("Wrong format, please check Settings tab.", MsgBoxStyle.Critical, "Error")
                    Case Languages.Español
                        MsgBox("Error de formato, por favor chequear la pestaña de configuración.", MsgBoxStyle.Critical, "Error")
                    Case Languages.Portuguêse
                        MsgBox("Formato errado, por favor verifique guia de configuração.", MsgBoxStyle.Critical, "Error")
                    Case Else
                        MsgBox("Falsches Format, überprüfen Sie bitte config-Register.", MsgBoxStyle.Critical, "Error")
                End Select
                Exit Sub
            End Try
        Else
            If Areas Is Nothing OrElse Areas.Length = 0 Then
                Select Case Settings.Language
                    Case Languages.English
                        MsgBox("You must select an area first.", MsgBoxStyle.Critical, "Error")
                    Case Languages.Español
                        MsgBox("Debes seleccionar un area primero.", MsgBoxStyle.Critical, "Error")
                    Case Languages.Portuguêse
                        MsgBox("Você deve selecionar uma área em primeiro lugar.", MsgBoxStyle.Critical, "Error")
                    Case Else
                        MsgBox("Du musst zuerst ein Gebiet auswählen.", MsgBoxStyle.Critical, "Error")
                End Select
                Exit Sub
            End If
            If RadioButton18.Checked Then
                Instances(Main.TabControl1.SelectedIndex).SyntaxHandle.Selection.Text = "new Area[" & aCount & "];" & vbNewLine
            End If
            Instances(Main.TabControl1.SelectedIndex).SyntaxHandle.Selection.Text = Areas
        End If
        Me.Hide()
    End Sub

    Private Sub Button16_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button16.Click
        PictureBox6.Image = My.Resources.Map
        TextBox22.Clear()
        TextBox23.Clear()
        TextBox24.Clear()
        TextBox25.Clear()
        Areas = ""
        pAreas = ""
        aCount = 0
    End Sub

    Private Sub Button17_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button17.Click
        PictureBox6.Image = My.Resources.Map
        TextBox22.Clear()
        TextBox23.Clear()
        TextBox24.Clear()
        TextBox25.Clear()
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
        eColor.Panel1.BackColor = Settings.C_Area.Hex
        eColor.TextBox1.Text = Settings.C_Area.Hex.R
        eColor.TrackBar1.Value = Settings.C_Area.Hex.R
        eColor.TextBox2.Text = Settings.C_Area.Hex.G
        eColor.TrackBar2.Value = Settings.C_Area.Hex.G
        eColor.TextBox3.Text = Settings.C_Area.Hex.B
        eColor.TrackBar3.Value = Settings.C_Area.Hex.B
        eColor.TextBox4.Text = Settings.C_Area.Hex.A
        eColor.TrackBar4.Value = Settings.C_Area.Hex.A
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
        PictureBox6.Size = New Point(TrackBar14.Value, TrackBar14.Value)
        PictureBox6.Location = New Point(0, 0)
        TrackBar12.Value = TrackBar12.Minimum
        TrackBar13.Value = TrackBar13.Maximum
    End Sub

    Private Sub TrackBar12_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBar12.Scroll
        PictureBox6.Location = New Point(TrackBar12.Value * -1, TrackBar13.Value)
    End Sub

    Private Sub TrackBar13_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBar13.Scroll
        PictureBox6.Location = New Point(TrackBar12.Value * -1, TrackBar13.Value)
    End Sub

#End Region

#Region "Extra"

    Private Sub CheckBox8_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox8.CheckedChanged
        If CheckBox8.Checked Then
            Button17.Visible = True
            If TextBox22.Text.Length > 0 Then
                If RadioButton18.Checked Then
                    Areas += String.Format("Area[{4}] = " & Settings.AreaCreateOutput & vbNewLine & Settings.AreaShowOutput & vbNewLine, TextBox22.Text, TextBox23.Text, TextBox24.Text, TextBox25.Text, aCount, "Area[" & aCount & "]", Settings.C_Area.Name)
                Else
                    Areas = String.Format(Settings.BoundsOutput & vbNewLine, TextBox22.Text, TextBox24.Text, TextBox23.Text, TextBox25.Text)
                End If
                pAreas += Selection.minX & "|,|" & Selection.minY & "|,|" & Selection.maxX & "|,|" & Selection.maxY & "|;|"
                aCount += 1
            End If
        Else
            Button17.Visible = False
            Areas = ""
            pAreas = ""
            PictureBox6.Image = My.Resources.Map
            TextBox22.Clear()
            TextBox23.Clear()
            TextBox24.Clear()
            TextBox25.Clear()
        End If
        Settings.A_MSelect = CheckBox8.Checked
    End Sub

    Private Sub CheckBox10_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox10.CheckedChanged
        Dim res As Single = 2048 / TrackBar14.Value
        If CheckBox10.Checked Then
            If CheckBox8.Checked Then
                If Not Areas Is Nothing AndAlso Areas.Length > 0 Then
                    PictureBox6.Image = My.Resources.Map
                    Dim tmp As String(), line As String(), g As Graphics
                    tmp = Split(pAreas, "|;|")
                    g = Graphics.FromImage(PictureBox6.Image)
                    For i = 0 To UBound(tmp) - 1
                        line = Split(tmp(i), "|,|")
                        g.FillRectangle(New SolidBrush(Settings.C_Area.Hex), CInt(line(0)) * res, CInt(line(1)) * res, (CInt(line(2)) - CInt(line(0))) * res, (CInt(line(3)) - CInt(line(1))) * res)
                    Next
                    g.Dispose()
                    PictureBox6.Refresh()
                End If
            Else
                PictureBox6.Image = My.Resources.Map
                Dim g As Graphics = Graphics.FromImage(PictureBox6.Image)
                g.FillRectangle(New SolidBrush(Settings.C_Area.Hex), Selection.minX * res, Selection.minY * res, (Selection.maxX - Selection.minX) * res, (Selection.maxY - Selection.minY) * res)
                g.Dispose()
                PictureBox6.Refresh()
            End If
        Else
            If CheckBox8.Checked = True Then
                If Not Areas Is Nothing AndAlso Areas.Length > 1 Then
                    PictureBox6.Image = My.Resources.Map
                    Dim tmp As String(), line As String(), g As Graphics
                    tmp = Split(pAreas, "|;|")
                    g = Graphics.FromImage(PictureBox6.Image)
                    For i = 0 To UBound(tmp) - 1
                        line = Split(tmp(i), "|,|")
                        g.DrawRectangle(New Pen(Settings.C_Area.Hex), CInt(line(0)) * res, CInt(line(1)) * res, (CInt(line(2)) - CInt(line(0))) * res, (CInt(line(3)) - CInt(line(1))) * res)
                    Next
                    g.Dispose()
                    PictureBox6.Refresh()
                End If
            Else
                PictureBox6.Image = My.Resources.Map
                Dim g As Graphics = Graphics.FromImage(PictureBox6.Image)
                g.DrawRectangle(New Pen(Settings.C_Area.Hex), Selection.minX * res, Selection.minY * res, (Selection.maxX - Selection.minX) * res, (Selection.maxY - Selection.minY) * res)
                g.Dispose()
                PictureBox6.Refresh()
            End If
        End If
        Settings.A_Fill = CheckBox10.Checked
    End Sub

    Private Sub RadioButton18_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton18.CheckedChanged
        If CheckBox8.Checked Then
            If Not Areas Is Nothing AndAlso Areas.Length > 0 Then
                If RadioButton18.Checked Then
                    Areas = ""
                    Dim tmp As String(), line As String()
                    tmp = Split(pAreas, "|;|")
                    For i = 0 To UBound(tmp) - 1
                        line = Split(tmp(i), "|,|")
                        Areas += String.Format("Area[{4}] = " & Settings.AreaCreateOutput & vbNewLine & Settings.AreaShowOutput & vbNewLine, Round(6000 / TrackBar14.Value * CInt(line(0)) - 3000, 6), Round(6000 / TrackBar14.Value * CInt(line(1)) - 3000, 6), Round(6000 / TrackBar14.Value * CInt(line(2)) - 3000, 6), Round(6000 / TrackBar14.Value * CInt(line(3)) - 3000, 6), aCount, "Area[" & aCount & "]", Settings.C_Area.Name)
                    Next
                End If
            End If
        End If
    End Sub

    Private Sub RadioButton19_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton19.CheckedChanged
        If CheckBox8.Checked Then
            If Not Areas Is Nothing AndAlso Areas.Length > 0 Then
                If RadioButton19.Checked Then
                    Areas = ""
                    Dim tmp As String(), line As String()
                    tmp = Split(pAreas, "|;|")
                    For i = 0 To UBound(tmp) - 1
                        line = Split(tmp(i), "|,|")
                        Areas += String.Format(Settings.BoundsOutput & vbNewLine, Round(6000 / TrackBar14.Value * CInt(line(0)) - 3000, 6), Round(6000 / TrackBar14.Value * CInt(line(2)) - 3000, 6), Round(6000 / TrackBar14.Value * CInt(line(1)) - 3000, 6), Round(6000 / TrackBar14.Value * CInt(line(3)) - 3000, 6))
                    Next
                End If
            End If
        End If
    End Sub

#End Region

#End Region

#Region "Converter"

#Region "Text Restrictions"

    Private Sub TextBox43_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Not IsNumeric(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) AndAlso Asc(e.KeyChar) <> 8 Then e.Handled = True
    End Sub

    Private Sub TextBox43_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        TextBox43.Text = Regex.Replace(TextBox43.Text, BadChars, "")
    End Sub

#End Region

#Region "Generate Code"

    Private Sub Button14_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button14.Click
        Try
            If RichTextBox3.Text.Length = 0 Then
                Select Case Settings.Language
                    Case Languages.English
                        MsgBox("You must add some objects and/or vehicles to convert.", MsgBoxStyle.Critical, "Error")
                    Case Languages.Español
                        MsgBox("Debes ingresar objetos y/o vehiculos para convertir.", MsgBoxStyle.Critical, "Error")
                    Case Languages.Portuguêse
                        MsgBox("Você deve inserir objetos e / ou veículos para converter.", MsgBoxStyle.Critical, "Error")
                    Case Else
                        MsgBox("Sie müssen Objekte und / oder Fahrzeuge zu konvertieren.", MsgBoxStyle.Critical, "Error")
                End Select
                Exit Sub
            End If
            TextBox26.Clear()
            If ComboBox3.Text = ComboBox4.Text Then
                If CheckBox9.Checked = False Then
                    TextBox26.Text = RichTextBox3.Text
                    Exit Sub
                End If
            End If
            Dim pos() As String, _
                ObjOutput As String, ObjOutPut2 As String = vbNullString, VehOutput As String = vbNullString, _
                oCount As Integer, vCount As Integer, ObjArray As Boolean, VehArray As Boolean
            If TextBox41.Text.Length > 0 Then
                ObjArray = True
                Select Case ComboBox4.SelectedIndex
                    Case 0
                        ObjOutput = TextBox41.Text & "[{0}] = CreateObject({1}, {2}, {3}, {4}, {5}, {6}, {7});" & vbNewLine
                    Case 1
                        If CheckBox3.Checked Then
                            ObjOutput = TextBox41.Text & "[{0}] = CreateDynamicObject({1}, {2}, {3}, {4}, {5}, {6}, {7}, .interiorid = {8});" & vbNewLine
                            ObjOutPut2 = TextBox41.Text & "[{0}] = CreateDynamicObject({1}, {2}, {3}, {4}, {5}, {6}, {7});" & vbNewLine
                        Else
                            ObjOutput = TextBox41.Text & "[{0}] = CreateDynamicObject({1}, {2}, {3}, {4}, {5}, {6}, {7});" & vbNewLine
                        End If
                    Case 2
                        ObjOutput = TextBox41.Text & "[{0}] = CreateDynamicObject({1}, {2}, {3}, {4}, {5}, {6}, {7});" & vbNewLine
                    Case 3
                        ObjOutput = TextBox41.Text & "[{0}] = CreateStreamedObject({1}, {2}, {3}, {4}, {5}, {6}, {7});" & vbNewLine
                    Case 4
                        ObjOutput = TextBox41.Text & "[{0}] = CreateStreamObject({1}, {2}, {3}, {4}, {5}, {6}, {7}, 250.0);" & vbNewLine
                    Case 5
                        ObjOutput = TextBox41.Text & "[{0}] = CreateStreamObject({1}, {2}, {3}, {4}, {5}, {6}, {7}, 250.0);" & vbNewLine
                    Case 6
                        ObjOutput = TextBox41.Text & "[{0}] = F_CreateObject({1}, {2}, {3}, {4}, {5}, {6}, {7});" & vbNewLine
                    Case Else
                        ObjOutput = TextBox41.Text & "[{0}] = CreateObject({1}, {2}, {3}, {4}, {5}, {6}, {7});" & vbNewLine
                End Select
            Else
                ObjArray = False
                Select Case ComboBox4.SelectedIndex
                    Case 0
                        ObjOutput = "CreateObject({0}, {1}, {2}, {3}, {4}, {5}, {6});" & vbNewLine
                    Case 1
                        If CheckBox3.Checked Then
                            ObjOutput = "CreateDynamicObject({0}, {1}, {2}, {3}, {4}, {5}, {6}, .interiorid = {7});" & vbNewLine
                            ObjOutPut2 = "CreateDynamicObject({0}, {1}, {2}, {3}, {4}, {5}, {6});" & vbNewLine
                        Else
                            ObjOutput = "CreateDynamicObject({0}, {1}, {2}, {3}, {4}, {5}, {6});" & vbNewLine
                        End If
                    Case 2
                        ObjOutput = "CreateDynamicObject({0}, {1}, {2}, {3}, {4}, {5}, {6});" & vbNewLine
                    Case 3
                        ObjOutput = "CreateStreamedObject({0}, {1}, {2}, {3}, {4}, {5}, {6});" & vbNewLine
                    Case 4
                        ObjOutput = "CreateStreamObject({0}, {1}, {2}, {3}, {4}, {5}, {6}, 250.0);" & vbNewLine
                    Case 5
                        ObjOutput = "CreateStreamObject({0}, {1}, {2}, {3}, {4}, {5}, {6}, 250.0);" & vbNewLine
                    Case 6
                        ObjOutput = "F_CreateObject({0}, {1}, {2}, {3}, {4}, {5}, {6});" & vbNewLine
                    Case Else
                        ObjOutput = "CreateObject({0}, {1}, {2}, {3}, {4}, {5}, {6});" & vbNewLine
                End Select
            End If
            If Not CheckBox4.Checked Then
                If TextBox42.Text.Length > 0 Then
                    VehArray = True
                    If CheckBox2.Checked = True Then
                        VehOutput = TextBox42.Text & "[{0}] = AddStaticVehicleEx({1}, {2}, {3}, {4}, {5}, {6}, {7}, {8});" & vbNewLine
                    Else
                        VehOutput = TextBox42.Text & "[{0}] = AddStaticVehicle({1}, {2}, {3}, {4}, {5}, {6}, {7});" & vbNewLine
                    End If
                Else
                    VehArray = False
                    If CheckBox2.Checked = True Then
                        VehOutput = "AddStaticVehicleEx({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7});" & vbNewLine
                    Else
                        VehOutput = "AddStaticVehicle({0}, {1}, {2}, {3}, {4}, {5}, {6});" & vbNewLine
                    End If
                End If
            End If
            Select Case ComboBox3.SelectedIndex
                Case 0 'SA-MP
                    For Each line In RichTextBox3.Lines
                        If line.IndexOf("CreateObject(") > -1 Then
                            pos = Split(Trim(Mid(Mid(line, line.IndexOf("(") + 2, line.Length - line.IndexOf("(")), 1, line.Length - 3)), ",")
                            Dim i As Integer = UBound(pos)
                            pos(i) = Mid(pos(i), 1, pos(i).IndexOf(")"))
                            If ObjArray Then
                                TextBox26.Text += String.Format(ObjOutput, oCount, pos(0), Convert.ToDecimal(pos(1)), Convert.ToDecimal(pos(2)), Convert.ToDecimal(pos(3)), Convert.ToDecimal(pos(4)), Convert.ToDecimal(pos(5)), Convert.ToDecimal(pos(6)))
                            Else
                                TextBox26.Text += String.Format(ObjOutput, pos(0), Convert.ToDecimal(pos(1)), Convert.ToDecimal(pos(2)), Convert.ToDecimal(pos(3)), Convert.ToDecimal(pos(4)), Convert.ToDecimal(pos(5)), Convert.ToDecimal(pos(6)))
                            End If
                            oCount += 1
                        ElseIf line.IndexOf("AddStaticVehicle(") > -1 AndAlso Not CheckBox4.Checked Then
                            pos = Split(Trim(Mid(Mid(line, line.IndexOf("(") + 2, line.Length - line.IndexOf("(")), 1, line.Length - 3)), ",")
                            Dim i As Integer = UBound(pos)
                            pos(i) = Mid(pos(i), 1, pos(i).IndexOf(")"))
                            If VehArray Then
                                TextBox26.Text += String.Format("{0}[{1}] = AddStaticVehicle({2}, {3}, {4}, {5}, {6}, {7}, {8});" & vbNewLine, vCount, pos(0), Convert.ToDecimal(pos(1)), Convert.ToDecimal(pos(2)), Convert.ToDecimal(pos(3)), Convert.ToDecimal(pos(4)), pos(5), pos(6))
                            Else
                                TextBox26.Text += String.Format("AddStaticVehicle({0}, {1}, {2}, {3}, {4}, {5}, {6});" & vbNewLine, pos(0), Convert.ToDecimal(pos(1)), Convert.ToDecimal(pos(2)), Convert.ToDecimal(pos(3)), Convert.ToDecimal(pos(4)), pos(5), pos(6))
                            End If
                            vCount += 1
                        ElseIf line.IndexOf("AddStaticVehicleEx(") > -1 AndAlso Not CheckBox4.Checked Then
                            pos = Split(Trim(Mid(Mid(line, line.IndexOf("(") + 2, line.Length - line.IndexOf("(")), 1, line.Length - 3)), ",")
                            Dim i As Integer = UBound(pos)
                            pos(i) = Mid(pos(i), 1, pos(i).IndexOf(")"))
                            If VehArray Then
                                TextBox26.Text += String.Format("{0}[{1}] = AddStaticVehicleEx({2}, {3}, {4}, {5}, {6}, {7}, {8}, {9});" & vbNewLine, TextBox42.Text, vCount, pos(0), Convert.ToDecimal(pos(1)), Convert.ToDecimal(pos(2)), Convert.ToDecimal(pos(3)), Convert.ToDecimal(pos(4)), pos(5), pos(6), pos(7))
                            Else
                                TextBox26.Text += String.Format("AddStaticVehicleEx({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7});" & vbNewLine, pos(0), Convert.ToDecimal(pos(1)), Convert.ToDecimal(pos(2)), Convert.ToDecimal(pos(3)), Convert.ToDecimal(pos(4)), pos(5), pos(6), pos(7))
                            End If
                            vCount += 1
                        End If

                    Next
                Case 1 'MTA Race
                    Try
                        Dim Writer As New StreamWriter(My.Application.Info.DirectoryPath & "\Convert.tmp")
                        Writer.Write(RichTextBox3.Text)
                        Writer.Close()
                        Dim Reader As Xml.XmlTextReader, Obj As ObjectInfo, Veh As VehicleInfo, type As ConvertType
                        Reader = New Xml.XmlTextReader(My.Application.Info.DirectoryPath & "\Convert.tmp")
                        Reader.WhitespaceHandling = Xml.WhitespaceHandling.None
                        Do While Reader.Read()
                            Select Case Reader.NodeType
                                Case Xml.XmlNodeType.Element
                                    Select Case Reader.Name
                                        Case "object"
                                            type = ConvertType.Obj
                                        Case "spawnpoint"
                                            If CheckBox4.Checked = False Then
                                                type = ConvertType.Veh
                                            Else
                                                type = ConvertType.None
                                            End If
                                        Case "position"
                                            Reader.Read()
                                            If type = ConvertType.Obj Then
                                                pos = Split(Reader.Value)
                                                Obj.X = Convert.ToDecimal(pos(0), New Globalization.CultureInfo("en-US"))
                                                Obj.Y = Convert.ToDecimal(pos(1), New Globalization.CultureInfo("en-US"))
                                                Obj.Z = Convert.ToDecimal(pos(2), New Globalization.CultureInfo("en-US"))
                                            ElseIf type = ConvertType.Veh Then
                                                pos = Split(Reader.Value)
                                                Veh.X = Convert.ToDecimal(pos(0), New Globalization.CultureInfo("en-US"))
                                                Veh.Y = Convert.ToDecimal(pos(1), New Globalization.CultureInfo("en-US"))
                                                Veh.Z = Convert.ToDecimal(pos(2), New Globalization.CultureInfo("en-US"))
                                            End If
                                        Case "rotation"
                                            Reader.Read()
                                            If type = ConvertType.Obj Then
                                                pos = Split(Reader.Value)
                                                Obj.rX = Convert.ToDecimal(pos(0), New Globalization.CultureInfo("en-US"))
                                                Obj.rY = Convert.ToDecimal(pos(1), New Globalization.CultureInfo("en-US"))
                                                Obj.rZ = Convert.ToDecimal(pos(2), New Globalization.CultureInfo("en-US"))
                                            ElseIf type = ConvertType.Veh Then
                                                Veh.R = Convert.ToDecimal(Reader.Value, New Globalization.CultureInfo("en-US"))
                                            End If
                                        Case "model"
                                            Reader.Read()
                                            Obj.Model = Reader.Value
                                            If CheckBox9.Checked Then
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
                                        Case "vehicle"
                                            Reader.Read()
                                            Veh.Model = Reader.Value
                                    End Select
                                Case Xml.XmlNodeType.EndElement
                                    Select Case Reader.Name
                                        Case "object"
                                            If ObjArray Then
                                                TextBox26.Text += String.Format(New Globalization.CultureInfo("en-US"), ObjOutput, oCount, Obj.Model, Obj.X, Obj.Y, Obj.Z, Obj.rX, Obj.rY, Obj.rZ)
                                            Else
                                                TextBox26.Text += String.Format(New Globalization.CultureInfo("en-US"), ObjOutput, Obj.Model, Obj.X, Obj.Y, Obj.Z, Obj.rX, Obj.rY, Obj.rZ)
                                            End If
                                            oCount += 1
                                            type = ConvertType.None
                                        Case "spawnpoint"
                                            If CheckBox4.Checked = False Then
                                                If VehArray Then
                                                    If CheckBox2.Checked Then
                                                        TextBox26.Text += String.Format(New Globalization.CultureInfo("en-US"), VehOutput, vCount, Veh.Model, Veh.X, Veh.Y, Veh.Z, Veh.R, Veh.Color1, Veh.Color2, TextBox43.Text)
                                                    Else
                                                        TextBox26.Text += String.Format(New Globalization.CultureInfo("en-US"), VehOutput, vCount, Veh.Model, Veh.X, Veh.Y, Veh.Z, Veh.R, Veh.Color1, Veh.Color2)
                                                    End If
                                                Else
                                                    If CheckBox2.Checked Then
                                                        TextBox26.Text += String.Format(New Globalization.CultureInfo("en-US"), VehOutput, Veh.Model, Veh.X, Veh.Y, Veh.Z, Veh.R, Veh.Color1, Veh.Color2, TextBox43.Text)
                                                    Else
                                                        TextBox26.Text += String.Format(New Globalization.CultureInfo("en-US"), VehOutput, Veh.Model, Veh.X, Veh.Y, Veh.Z, Veh.R, Veh.Color1, Veh.Color2)
                                                    End If
                                                End If
                                                vCount += 1
                                                type = ConvertType.None
                                            End If
                                    End Select
                            End Select
                        Loop
                        Reader.Close()
                        File.Delete(My.Application.Info.DirectoryPath & "\Convert.tmp")
                    Catch ex As Exception
                        File.Delete(My.Application.Info.DirectoryPath & "\Convert.tmp")
                        MsgBox(ex.Message.ToString)
                    End Try
                Case 2 'MTA 1.1
                    Dim Obj As ObjectInfo, Veh As VehicleInfo
                    For Each Line In RichTextBox3.Lines
                        If Line.IndexOf("<object") > -1 Then
                            Obj.Model = Mid(Line, Line.IndexOf("model=") + 8, Line.IndexOf("""", Line.IndexOf("model=")) - Line.IndexOf("model=") - 3)
                            If CheckBox9.Checked Then
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
                            Obj.X = Round(Convert.ToDecimal(Mid(Line, Line.IndexOf("posX=") + 7, If(Line.IndexOf(""" ", Line.IndexOf("posX=")) - Line.IndexOf("posX=") - 6 > 0, Line.IndexOf(""" ", Line.IndexOf("posX=")) - Line.IndexOf("posX=") - 6, 1)), New Globalization.CultureInfo("en-US")), 6)
                            Obj.Y = Round(Convert.ToDecimal(Mid(Line, Line.IndexOf("posY=") + 7, If(Line.IndexOf(""" ", Line.IndexOf("posY=")) - Line.IndexOf("posY=") - 6 > 0, Line.IndexOf(""" ", Line.IndexOf("posY=")) - Line.IndexOf("posY=") - 6, 1)), New Globalization.CultureInfo("en-US")), 6)
                            Obj.Z = Round(Convert.ToDecimal(Mid(Line, Line.IndexOf("posZ=") + 7, If(Line.IndexOf(""" ", Line.IndexOf("posZ=")) - Line.IndexOf("posZ=") - 6 > 0, Line.IndexOf(""" ", Line.IndexOf("posZ=")) - Line.IndexOf("posZ=") - 6, 1)), New Globalization.CultureInfo("en-US")), 6)
                            Obj.rX = Round(Convert.ToDecimal(Mid(Line, Line.IndexOf("rotX=") + 7, If(Line.IndexOf(""" ", Line.IndexOf("rotX=")) - Line.IndexOf("rotX=") - 6 > 0, Line.IndexOf(""" ", Line.IndexOf("rotX=")) - Line.IndexOf("rotX=") - 6, 1)), New Globalization.CultureInfo("en-US")), 6)
                            Obj.rY = Round(Convert.ToDecimal(Mid(Line, Line.IndexOf("rotY=") + 7, If(Line.IndexOf(""" ", Line.IndexOf("rotY=")) - Line.IndexOf("rotY=") - 6 > 0, Line.IndexOf(""" ", Line.IndexOf("rotY=")) - Line.IndexOf("rotY=") - 6, 1)), New Globalization.CultureInfo("en-US")), 6)
                            Obj.rZ = Round(Convert.ToDecimal(Mid(Line, Line.IndexOf("rotZ=") + 7, If(Line.IndexOf("""", Line.IndexOf("rotZ=""")) - Line.IndexOf("rotZ=""") - 6 > 0, Line.IndexOf("""", Line.IndexOf("rotZ=""")) - Line.IndexOf("rotZ=""") - 6, 1)), New Globalization.CultureInfo("en-US")), 6)
                            If CheckBox3.Checked Then
                                Try
                                    Obj.Interior = Mid(Line, Line.IndexOf("interior=") + 11, If(Line.IndexOf("""", Line.IndexOf("interior=""")) - Line.IndexOf("interior=""") - 6 > 0, Line.IndexOf("""", Line.IndexOf("interior=""")) - Line.IndexOf("interior=""") - 6, 1))
                                Catch ex As Exception
                                    Obj.Interior = -1
                                End Try
                            End If
                            If ObjArray Then
                                If CheckBox3.Checked AndAlso Obj.Interior <> -1 Then
                                    TextBox26.Text += String.Format(New Globalization.CultureInfo("en-US"), ObjOutput, oCount, Obj.Model, Obj.X, Obj.Y, Obj.Z, Obj.rX, Obj.rY, Obj.rZ, Obj.Interior)
                                Else
                                    TextBox26.Text += String.Format(New Globalization.CultureInfo("en-US"), ObjOutPut2, oCount, Obj.Model, Obj.X, Obj.Y, Obj.Z, Obj.rX, Obj.rY, Obj.rZ)
                                End If
                            Else
                                If CheckBox3.Checked AndAlso Obj.Interior <> -1 Then
                                    TextBox26.Text += String.Format(New Globalization.CultureInfo("en-US"), ObjOutput, Obj.Model, Obj.X, Obj.Y, Obj.Z, Obj.rX, Obj.rY, Obj.rZ, Obj.Interior)
                                Else
                                    TextBox26.Text += String.Format(New Globalization.CultureInfo("en-US"), ObjOutPut2, Obj.Model, Obj.X, Obj.Y, Obj.Z, Obj.rX, Obj.rY, Obj.rZ)
                                End If
                            End If
                            oCount += 1
                        ElseIf Line.IndexOf("<vehicle") > -1 AndAlso Not CheckBox4.Checked Then
                            Veh.Model = Mid(Line, Line.IndexOf("model=") + 8, Line.IndexOf("""", Line.IndexOf("model=")) - Line.IndexOf("model=") - 3)
                            Veh.X = Round(Convert.ToDecimal(Mid(Line, Line.IndexOf("posX=") + 7, If(Line.IndexOf(""" ", Line.IndexOf("posX=")) - Line.IndexOf("posX=") - 6 > 0, Line.IndexOf(""" ", Line.IndexOf("posX=")) - Line.IndexOf("posX=") - 6, 1)), New Globalization.CultureInfo("en-US")), 6)
                            Veh.Y = Round(Convert.ToDecimal(Mid(Line, Line.IndexOf("posY=") + 7, If(Line.IndexOf(""" ", Line.IndexOf("posY=")) - Line.IndexOf("posY=") - 6 > 0, Line.IndexOf(""" ", Line.IndexOf("posY=")) - Line.IndexOf("posY=") - 6, 1)), New Globalization.CultureInfo("en-US")), 6)
                            Veh.Z = Round(Convert.ToDecimal(Mid(Line, Line.IndexOf("posZ=") + 7, If(Line.IndexOf(""" ", Line.IndexOf("posZ=")) - Line.IndexOf("posZ=") - 6 > 0, Line.IndexOf(""" ", Line.IndexOf("posZ=")) - Line.IndexOf("posZ=") - 6, 1)), New Globalization.CultureInfo("en-US")), 6)
                            Veh.R = Round(Convert.ToDecimal(Mid(Line, Line.IndexOf("rotZ=") + 7, If(Line.IndexOf(""" ", Line.IndexOf("rotZ=")) - Line.IndexOf("rotZ=") - 6 > 0, Line.IndexOf(""" ", Line.IndexOf("rotZ=")) - Line.IndexOf("rotZ=") - 6, 1)), New Globalization.CultureInfo("en-US")), 6)
                            If Line.IndexOf("color=") > -1 Then
                                pos = Split(Mid(Line, Line.IndexOf("color=") + 8, Line.IndexOf("""", Line.IndexOf("color=")) - Line.IndexOf("color=") - 1), ",")
                                Veh.Color1 = pos(0)
                                Veh.Color2 = pos(1)
                            Else
                                Veh.Color1 = 0
                                Veh.Color2 = 0
                            End If
                            If VehArray Then
                                If CheckBox2.Checked Then
                                    TextBox26.Text += String.Format(New Globalization.CultureInfo("en-US"), VehOutput, vCount, Veh.Model, Veh.X, Veh.Y, Veh.Z, Veh.R, Veh.Color1, Veh.Color2, TextBox43.Text)
                                Else
                                    TextBox26.Text += String.Format(New Globalization.CultureInfo("en-US"), VehOutput, vCount, Veh.Model, Veh.X, Veh.Y, Veh.Z, Veh.R, Veh.Color1, Veh.Color2)
                                End If
                            Else
                                If CheckBox2.Checked Then
                                    TextBox26.Text += String.Format(New Globalization.CultureInfo("en-US"), VehOutput, Veh.Model, Veh.X, Veh.Y, Veh.Z, Veh.R, Veh.Color1, Veh.Color2, TextBox43.Text)
                                Else
                                    TextBox26.Text += String.Format(New Globalization.CultureInfo("en-US"), VehOutput, Veh.Model, Veh.X, Veh.Y, Veh.Z, Veh.R, Veh.Color1, Veh.Color2)
                                End If
                            End If
                            vCount += 1
                        End If
                    Next
                Case 3, 4 'Incognito's Streamer Plugin & YSI DynamicObject
                    For Each Line In RichTextBox3.Lines
                        If Line.IndexOf("CreateDynamicObject(") > -1 Then
                            pos = Split(Trim(Mid(Mid(Line, Line.IndexOf("(") + 2, Line.Length - Line.IndexOf("(")), 1, Line.Length - 3)), ",")
                            Dim i As Integer = UBound(pos)
                            pos(i) = Mid(pos(i), 1, pos(i).IndexOf(")"))
                            If CheckBox9.Checked Then
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
                            If ObjArray Then
                                TextBox26.Text += String.Format(New Globalization.CultureInfo("en-US"), ObjOutput, oCount, pos(0), Convert.ToDecimal(pos(1)), Convert.ToDecimal(pos(2)), Convert.ToDecimal(pos(3)), Convert.ToDecimal(pos(4)), Convert.ToDecimal(pos(5)), Convert.ToDecimal(pos(6)))
                            Else
                                TextBox26.Text += String.Format(New Globalization.CultureInfo("en-US"), ObjOutput, pos(0), Convert.ToDecimal(pos(1)), Convert.ToDecimal(pos(2)), Convert.ToDecimal(pos(3)), Convert.ToDecimal(pos(4)), Convert.ToDecimal(pos(5)), Convert.ToDecimal(pos(6)))
                            End If
                            oCount += 1
                        End If
                    Next
                Case 5 'xStreamer
                    For Each Line In RichTextBox3.Lines
                        If Line.IndexOf("CreateStreamedObject(") > -1 Then
                            pos = Split(Trim(Mid(Mid(Line, Line.IndexOf("(") + 2, Line.Length - Line.IndexOf("(")), 1, Line.Length - 3)), ",")
                            Dim i As Integer = UBound(pos)
                            pos(i) = Mid(pos(i), 1, pos(i).IndexOf(")"))
                            If CheckBox9.Checked Then
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
                            If ObjArray Then
                                TextBox26.Text += String.Format(New Globalization.CultureInfo("en-US"), ObjOutput, oCount, pos(0), Convert.ToDecimal(pos(1)), Convert.ToDecimal(pos(2)), Convert.ToDecimal(pos(3)), Convert.ToDecimal(pos(4)), Convert.ToDecimal(pos(5)), Convert.ToDecimal(pos(6)))
                            Else
                                TextBox26.Text += String.Format(New Globalization.CultureInfo("en-US"), ObjOutput, pos(0), Convert.ToDecimal(pos(1)), Convert.ToDecimal(pos(2)), Convert.ToDecimal(pos(3)), Convert.ToDecimal(pos(4)), Convert.ToDecimal(pos(5)), Convert.ToDecimal(pos(6)))
                            End If
                            oCount += 1
                        End If
                    Next
                Case 6, 7 'MidoStream ObjectInfo & Doble-O ObjectInfo
                    For Each Line In RichTextBox3.Lines
                        If Line.IndexOf("CreateStreamObject(") > -1 Then
                            pos = Split(Trim(Mid(Mid(Line, Line.IndexOf("(") + 2, Line.Length - Line.IndexOf("(")), 1, Line.Length - 3)), ",")
                            Dim i As Integer = UBound(pos)
                            pos(i) = Mid(pos(i), 1, pos(i).IndexOf(")"))
                            If CheckBox9.Checked Then
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
                            If ObjArray Then
                                TextBox26.Text += String.Format(New Globalization.CultureInfo("en-US"), ObjOutput, oCount, pos(0), Convert.ToDecimal(pos(1)), Convert.ToDecimal(pos(2)), Convert.ToDecimal(pos(3)), Convert.ToDecimal(pos(4)), Convert.ToDecimal(pos(5)), Convert.ToDecimal(pos(6)))
                            Else
                                TextBox26.Text += String.Format(New Globalization.CultureInfo("en-US"), ObjOutput, pos(0), Convert.ToDecimal(pos(1)), Convert.ToDecimal(pos(2)), Convert.ToDecimal(pos(3)), Convert.ToDecimal(pos(4)), Convert.ToDecimal(pos(5)), Convert.ToDecimal(pos(6)))
                            End If
                            oCount += 1
                        End If
                    Next
                Case 8 'Fallout's Object Streamer
                    For Each Line In RichTextBox3.Lines
                        If Line.IndexOf("F_CreateObject(") > -1 Then
                            pos = Split(Trim(Mid(Mid(Line, Line.IndexOf("(") + 2, Line.Length - Line.IndexOf("(")), 1, Line.Length - 3)), ",")
                            Dim i As Integer = UBound(pos)
                            pos(i) = Mid(pos(i), 1, pos(i).IndexOf(")"))
                            If CheckBox9.Checked Then
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
                            If ObjArray Then
                                TextBox26.Text += String.Format(New Globalization.CultureInfo("en-US"), ObjOutput, oCount, pos(0), Convert.ToDecimal(pos(1)), Convert.ToDecimal(pos(2)), Convert.ToDecimal(pos(3)), Convert.ToDecimal(pos(4)), Convert.ToDecimal(pos(5)), Convert.ToDecimal(pos(6)))
                            Else
                                TextBox26.Text += String.Format(New Globalization.CultureInfo("en-US"), ObjOutput, pos(0), Convert.ToDecimal(pos(1)), Convert.ToDecimal(pos(2)), Convert.ToDecimal(pos(3)), Convert.ToDecimal(pos(4)), Convert.ToDecimal(pos(5)), Convert.ToDecimal(pos(6)))
                            End If
                            oCount += 1
                        End If
                    Next
            End Select
            Select Case Settings.Language
                Case Languages.English
                    MsgBox(String.Format("Converted Objects: {0}  -  Converted Vehicles: {1}", oCount, vCount), MsgBoxStyle.OkOnly, "Object converter")
                Case Languages.Español
                    MsgBox(String.Format("Objetos convertidos: {0}  -  Vehiculos convertidos: {1}", oCount, vCount), MsgBoxStyle.OkOnly, "Conversor de objetos")
                Case Languages.Portuguêse
                    MsgBox(String.Format("Objetos convertidos: {0}", oCount), MsgBoxStyle.OkOnly, "Object Converter")
                Case Else
                    MsgBox(String.Format("Konvertierte Objekte: {0}", oCount), MsgBoxStyle.OkOnly, "Object converter")
            End Select
        Catch ex As Exception
            TextBox26.Clear()
                Select Settings.Language
                Case Languages.English
                    MsgBox("Conversion Stoped due to an error.", MsgBoxStyle.Critical, "Error")
                Case Languages.Español
                    MsgBox("Conversion parada debido a un error.", MsgBoxStyle.Critical, "Error")
                Case Languages.Portuguêse
                    MsgBox("conversão parou devido a um erro.", MsgBoxStyle.Critical, "Erro")
                Case Else
                    MsgBox("Conversion gestoppt aufgrund eines Fehlers.", MsgBoxStyle.Critical, "Error")
            End Select
        End Try
    End Sub

#End Region

#Region "From File"

    Private Sub Button20_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button20.Click
        With OFD
            .InitialDirectory = Settings.oPath
            .Multiselect = False
            If .ShowDialog = Windows.Forms.DialogResult.Cancel Then Exit Sub
            Dim Reader As New IO.StreamReader(.FileName)
            Settings.oPath = Mid(.FileName, 1, .FileName.LastIndexOf("\"))
            RichTextBox3.Text = Reader.ReadToEnd()
            Reader.Close()
        End With
    End Sub

#End Region

#Region "Export"

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        Instances(Main.TabControl1.SelectedIndex).SyntaxHandle.Selection.Text = TextBox29.Text & vbNewLine
    End Sub

#End Region

#Region "Visual"

    Private Sub CheckBox2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox2.CheckedChanged
        If CheckBox2.Checked Then
            TextBox43.Enabled = True
        Else
            TextBox43.Enabled = False
        End If
    End Sub

    Private Sub Button11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button11.Click
        If Panel8.Visible Then
            Panel8.Visible = False
        Else
            Panel8.Visible = True
        End If
    End Sub

    Private Sub Button11_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Button11.MouseMove
        If Not Panel8.Visible Then
            Panel8.Visible = True
        End If
    End Sub

    Private Sub TabPage4_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles TabPage4.MouseMove
        If Panel8.Visible Then
            Panel8.Visible = False
        End If
    End Sub

    Private Sub RichTextBox3_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles RichTextBox3.MouseMove
        If Panel8.Visible Then
            Panel8.Visible = False
        End If
    End Sub

    Private Sub Button20_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Button20.MouseMove
        If Panel8.Visible Then
            Panel8.Visible = False
        End If
    End Sub

#End Region

#End Region

#Region "Info"

#Region "Tabs"

#Region "Skins"

#Region "Display"

    Private Sub TreeView1_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles TreeView1.AfterSelect
        If IsNumeric(TreeView1.SelectedNode.Text) Then
            Dim tmp As Boolean
            If Settings.Images OrElse Settings.URL_Skin.Length > 0 Then
                tmp = True
            Else
                tmp = False
            End If
            If Not tmp Then
                Try
                    PictureBox7.Image = LoadImageFromURL(String.Format(Settings.URL_Skin, TreeView1.SelectedNode.Text))
                Catch ex As Exception
                    PictureBox7.Image = My.Resources.N_A
                End Try
            End If
            For Each Skin In Skins
                If TreeView1.SelectedNode.Text = Skin.ID Then
                    If tmp Then
                        Try
                            PictureBox7.Image = GetImageFromResource(ImageTypes.Skin, Skin.ID)
                        Catch ex As Exception
                            PictureBox7.Image = My.Resources.N_A
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

#Region "Export"

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        If RadioButton1.Checked Then
            Instances(Main.TabControl1.SelectedIndex).SyntaxHandle.Selection.Text = "AddPlayerClass(" & TreeView1.SelectedNode.Text & ", 0.0, 0.0, 0.0, 0.0, 0, 0, 0, 0, 0, 0);" & vbNewLine
        ElseIf RadioButton2.Checked Then
            Instances(Main.TabControl1.SelectedIndex).SyntaxHandle.Selection.Text = "SetPlayerSkin(" & TextBox27.Text & ", " & TreeView1.SelectedNode.Text & ");" & vbNewLine
        Else
            Try
                Instances(Main.TabControl1.SelectedIndex).SyntaxHandle.Selection.Text = String.Format(TextBox28.Text & vbNewLine, TextBox27.Text, TreeView1.SelectedNode.Text)
            Catch ex As Exception
                Instances(Main.TabControl1.SelectedIndex).SyntaxHandle.Selection.Text = "SetPlayerSkin(" & TextBox27.Text & ", " & TreeView1.SelectedNode.Text & ");" & vbNewLine
            End Try
        End If
        Me.Hide()
    End Sub

#End Region

#Region "Extra"

    Private Sub RadioButton2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton2.CheckedChanged
        If RadioButton2.Checked Then
            TextBox27.Enabled = True
        Else
            TextBox27.Enabled = False
        End If
    End Sub

    Private Sub RadioButton3_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton3.CheckedChanged
        If RadioButton3.Checked Then
            TextBox27.Enabled = True
            TextBox28.Enabled = True
        Else
            TextBox27.Enabled = False
            TextBox28.Enabled = False
        End If
    End Sub

#End Region

#End Region

#Region "Vehicles"

#Region "Arrays"

    Dim VehSender As Boolean = False, VehPos As Integer

#End Region

#Region "Text Restrictions"

    Private Sub TextBox52_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox52.KeyPress
        If Not IsNumeric(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) AndAlso Asc(e.KeyChar) <> 8 Then e.Handled = True
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
        If Not VehSender Then
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
        If Not VehSender Then
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
        If Not VehSender Then
            VehSender = True
            Dim pos As Integer
            pos = GetScrollPos(TreeView2.Handle, SBS_VERT) + 3
            SetScrollPos(TreeView3.Handle, SBS_VERT, pos, True)
            PostMessageA(TreeView3.Handle, WM_VSCROLL, SB_THUMBPOSITION + &H10000 * pos, Nothing)
        End If
        VehSender = False
    End Sub

    Private Sub TreeView3_AfterCollapse(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles TreeView3.AfterCollapse
        If Not VehSender Then
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
        If Not VehSender Then
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
        If Not VehSender Then
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
            If Settings.Images OrElse Settings.URL_Veh.Length > 0 Then
                tmp = True
            Else
                tmp = False
            End If
            If Not tmp Then
                Try
                    PictureBox8.Image = LoadImageFromURL(String.Format(Settings.URL_Veh, TreeView2.SelectedNode.Text))
                Catch ex As Exception
                    PictureBox8.Image = My.Resources.N_A
                End Try
            End If
            For Each vehicle In Vehicles
                If vehicle.ID = TreeView2.SelectedNode.Text Then
                    If tmp = True Then
                        Try
                            PictureBox8.Image = GetImageFromResource(ImageTypes.Vehicle, vehicle.ID - 400)
                        Catch ex As Exception
                            PictureBox8.Image = My.Resources.N_A
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
                If Settings.Images OrElse Settings.URL_Veh.Length > 0 Then
                    tmp = True
                Else
                    tmp = False
                End If
                If Not tmp Then
                    Try
                        PictureBox8.Image = LoadImageFromURL(String.Format(Settings.URL_Veh, TreeView2.SelectedNode.Text))
                    Catch ex As Exception
                        PictureBox8.Image = My.Resources.N_A
                    End Try
                End If
                For Each vehicle In Vehicles
                    If vehicle.Name = TreeView3.SelectedNode.Text Then
                        If tmp Then
                            Try
                                PictureBox8.Image = GetImageFromResource(ImageTypes.Vehicle, vehicle.ID - 400)
                            Catch ex As Exception
                                PictureBox8.Image = My.Resources.N_A
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
        If TextBox52.Text.Length > 0 AndAlso TextBox52.Text <> "0" Then
            If TextBox52.Text > 611 OrElse TextBox52.Text < 400 Then
                Select Case Settings.Language
                    Case Languages.English
                        MsgBox("Invalid model ID.", MsgBoxStyle.Critical, "Error")
                    Case Languages.Español
                        MsgBox("Modelo inválido.", MsgBoxStyle.Critical, "Error")
                    Case Languages.Portuguêse
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
            If Not found Then
                Select Case Settings.Language
                    Case Languages.English
                        MsgBox("Vehicle not found.", MsgBoxStyle.Critical, "Error")
                    Case Languages.Español
                        MsgBox("Vehiculo no encontrado.", MsgBoxStyle.Critical, "Error")
                    Case Languages.Portuguêse
                        MsgBox("Veículo não encontrado.", MsgBoxStyle.Critical, "Error")
                    Case Else
                        MsgBox("Fahrzeug nicht gefunden.", MsgBoxStyle.Critical, "Error")
                End Select
                Exit Sub
            End If
            TreeView3.Select()
        Else
            If TextBox52.Text.Length > 0 Then
                Select Case Settings.Language
                    Case Languages.English
                        MsgBox("Invalid model name.", MsgBoxStyle.Critical, "Error")
                    Case Languages.Español
                        MsgBox("Modelo inválido.", MsgBoxStyle.Critical, "Error")
                    Case Languages.Portuguêse
                        MsgBox("Modelo inválido.", MsgBoxStyle.Critical, "Error")
                    Case Else
                        MsgBox("Ungültiger Modellname.", MsgBoxStyle.Critical, "Error")
                End Select
                Exit Sub
            End If
            Dim found As Boolean, count As Integer
            found = False
            For Each vehicle In Vehicles
                If vehicle.Name.IndexOf(TextBox53.Text) > -1 Then
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
            If Not found Then
                VehPos = 0
                For Each vehicle In Vehicles
                    If vehicle.Name.IndexOf(TextBox53.Text) > -1 Then
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
                If Not found Then
                    Select Case Settings.Language
                        Case Languages.English
                            MsgBox("Vehicle not found.", MsgBoxStyle.Critical, "Error")
                        Case Languages.Español
                            MsgBox("Vehiculo no encontrado.", MsgBoxStyle.Critical, "Error")
                        Case Languages.Portuguêse
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

#Region "Export"

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        If RadioButton5.Checked Then
            Instances(Main.TabControl1.SelectedIndex).SyntaxHandle.Selection.Text = "PlayerPlaySound(" & TextBox29.Text & ", " & TextBox57.Text & ");" & vbNewLine
        Else
            Try
                Instances(Main.TabControl1.SelectedIndex).SyntaxHandle.Selection.Text = String.Format(TextBox30.Text & vbNewLine, TextBox29.Text, TextBox57.Text)
            Catch ex As Exception
                Instances(Main.TabControl1.SelectedIndex).SyntaxHandle.Selection.Text = "PlayerPlaySound(" & TextBox29.Text & ", " & TextBox57.Text & ");" & vbNewLine
            End Try
        End If
    End Sub

#End Region

#Region "Extra"

    Private Sub RadioButton4_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton4.CheckedChanged
        If RadioButton4.Checked Then
            TextBox30.Enabled = True
        Else
            TextBox30.Enabled = False
        End If
    End Sub

#End Region

#End Region

#Region "Weapons"

#Region "Display"

    Private Sub TreeView6_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles TreeView6.AfterSelect
        Dim tmp As Boolean
        If Settings.Images OrElse Settings.URL_Weap.Length > 0 Then
            tmp = True
        Else
            tmp = False
        End If
        If Not tmp Then
            Try
                PictureBox9.Image = LoadImageFromURL(String.Format(Settings.URL_Weap, TreeView7.SelectedNode.Text))
            Catch ex As Exception
                PictureBox9.Image = My.Resources.N_A
            End Try
        End If
        For Each weapon As Weap In Weapons
            If weapon.Name = TreeView6.SelectedNode.Text Then
                If tmp Then
                    Try
                        PictureBox9.Image = GetImageFromResource(ImageTypes.Weapon, weapon.ID)
                    Catch ex As Exception
                        PictureBox9.Image = My.Resources.N_A
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

#Region "Text Restrictions"

    Private Sub TextBox45_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox45.KeyPress
        If Not IsNumeric(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) AndAlso Asc(e.KeyChar) <> 8 Then e.Handled = True
    End Sub

    Private Sub TextBox45_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox45.TextChanged
        TextBox45.Text = Regex.Replace(TextBox45.Text, BadChars, "")
    End Sub

#End Region

#Region "Export"

    Private Sub Button13_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button13.Click
        If RadioButton12.Checked Then
            Instances(Main.TabControl1.SelectedIndex).SyntaxHandle.Selection.Text = "GivePlayerWeapon(" & TextBox44.Text & ", " & TextBox74.Text & ", " & TextBox45.Text & ");" & vbNewLine
        Else
            Try
                Instances(Main.TabControl1.SelectedIndex).SyntaxHandle.Selection.Text = String.Format(TextBox46.Text & vbNewLine, TextBox44.Text, TextBox74.Text, TextBox45.Text)
            Catch ex As Exception
                Instances(Main.TabControl1.SelectedIndex).SyntaxHandle.Selection.Text = "GivePlayerWeapon(" & TextBox44.Text & ", " & TextBox74.Text & ", " & TextBox45.Text & ");" & vbNewLine
            End Try
        End If
    End Sub

#End Region

#End Region

#Region "Map Icons"

#Region "Display"

    Private Sub TreeView7_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles TreeView7.AfterSelect
        Dim tmp As Boolean
        If Settings.Images OrElse Settings.URL_Map.Length > 0 Then
            tmp = True
        Else
            tmp = False
        End If
        If Not tmp Then
            Try
                PictureBox10.Image = LoadImageFromURL(String.Format(Settings.URL_Map, TreeView7.SelectedNode.Text))
            Catch ex As Exception
                PictureBox10.Image = My.Resources.N_A
            End Try
        End If
        For Each map In Maps
            If map.ID = TreeView7.SelectedNode.Text Then
                If tmp Then
                    Try
                        PictureBox10.Image = GetImageFromResource(ImageTypes.MapIcon, map.ID)
                    Catch ex As Exception
                        PictureBox10.Image = My.Resources.N_A
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

#Region "Arrays"

    Dim SprPos As Integer

#End Region

#Region "Display"

    Private Sub TreeView8_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles TreeView8.AfterSelect
        Dim tmp As Boolean
        If Settings.Images OrElse Settings.URL_Map.Length > 0 Then
            tmp = True
        Else
            tmp = False
        End If
        If Not tmp Then
            Try
                PictureBox11.Image = LoadImageFromURL(String.Format(Settings.URL_Sprite, TreeView7.SelectedNode.Text))
            Catch ex As Exception
                PictureBox11.Image = My.Resources.N_A
            End Try
        End If
        Dim count As Integer
        For i = 0 To Sprites.Length - 1
            If TreeView8.SelectedNode.Text = Sprites(i).Name Then
                If tmp Then
                    Try
                        'PictureBox11.Image = Image.FromFile(My.Application.Info.DirectoryPath & "\Resources\Sprite_" & count & ".bmp")
                        PictureBox11.Image = GetImageFromResource(ImageTypes.Sprite, i)
                    Catch ex As Exception
                        PictureBox11.Image = My.Resources.N_A
                    End Try
                End If
                TextBox88.Text = Sprites(i).Name
                TextBox89.Text = Sprites(i).Path
                TextBox90.Text = Sprites(i).File
                TextBox91.Text = Sprites(i).Size
            End If
            count += 1
        Next
    End Sub

#End Region

#Region "Find"

    Private Sub Button19_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button19.Click
        If Not TextBox94.Text.Length > 0 Then
            Select Case Settings.Language
                Case Languages.English
                    MsgBox("Invalid name.", MsgBoxStyle.Critical, "Error")
                Case Languages.Español
                    MsgBox("Nombre inválido.", MsgBoxStyle.Critical, "Error")
                Case Languages.Portuguêse
                    MsgBox("Nombre inválido.", MsgBoxStyle.Critical, "Error")
                Case Else
                    MsgBox("ungültigen Namen.", MsgBoxStyle.Critical, "Error")
            End Select
            Exit Sub
        End If
        Dim found As Boolean, count As Integer
        found = False
        For Each sprite In Sprites
            If sprite.Name.IndexOf(TextBox94.Text) > -1 Then
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
        If Not found Then
            SprPos = 0
            For Each sprite In Sprites
                If sprite.Name.IndexOf(TextBox94.Text) > -1 Then
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
            If Not found Then
                Select Case Settings.Language
                    Case Languages.English
                        MsgBox("Sprite not found.", MsgBoxStyle.Critical, "Error")
                    Case Languages.Español
                        MsgBox("Sprite no encontrado.", MsgBoxStyle.Critical, "Error")
                    Case Languages.Portuguêse
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

#Region "Functions"

    Private Function GetImageFromResource(ByVal type As ImageTypes, ByVal index As Integer) As Image
        Dim img As Bitmap, g As Graphics, height As Integer
        Select Case type
            Case ImageTypes.MapIcon
                If index < 6 Then
                    height = 0
                Else
                    height = Floor(index / 6)
                End If
                img = New Bitmap(16, 16)
                g = Graphics.FromImage(img)
                g.DrawImage(My.Resources.Map_Icons, 0, 0, New Rectangle((index Mod 6) * 16, height * 16, 16, 16), GraphicsUnit.Pixel)
                g.Dispose()
            Case ImageTypes.Skin
                If index < 15 Then
                    height = 0
                Else
                    height = Floor(index / 15)
                End If
                img = New Bitmap(150, 260)
                g = Graphics.FromImage(img)
                g.DrawImage(My.Resources.Skins, 0, 0, New Rectangle((index Mod 15) * 150, height * 260, 150, 260), GraphicsUnit.Pixel)
                g.Dispose()
            Case ImageTypes.Sprite
                If index < 32 Then
                    height = 0
                Else
                    height = Floor(index / 32)
                End If
                img = New Bitmap(64, 64)
                g = Graphics.FromImage(img)
                g.DrawImage(My.Resources.Sprites, 0, 0, New Rectangle((index Mod 32) * 64, height * 64, 64, 64), GraphicsUnit.Pixel)
                g.Dispose()
            Case ImageTypes.Vehicle
                If index <= 10 Then
                    height = 0
                Else
                    height = Floor(index / 10)
                End If
                img = New Bitmap(204, 125)
                g = Graphics.FromImage(img)
                g.DrawImage(My.Resources.Vehicles, 0, 0, New Rectangle((index Mod 10) * 204, height * 125, 204, 125), GraphicsUnit.Pixel)
                g.Dispose()
            Case Else
                If index < 10 Then
                    height = 0
                Else
                    height = Floor(index / 10)
                End If
                img = New Bitmap(64, 64)
                g = Graphics.FromImage(img)
                g.DrawImage(My.Resources.Weapons, 0, 0, New Rectangle((index Mod 10) * 64, height * 64, 64, 64), GraphicsUnit.Pixel)
                g.Dispose()
        End Select
        Return img
    End Function

#End Region

#End Region

#End Region

End Class