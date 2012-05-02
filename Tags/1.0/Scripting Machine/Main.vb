Imports System.Text.RegularExpressions
Imports System.Drawing
Imports System.Math
Public Class Main

#Region "Arrays"

    Dim VehSender As Boolean = False
    Dim Selection As Area
    Dim Areas As String

#End Region

#Region "Main"

    Private Sub Main_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        SaveConfig()
    End Sub

    Private Sub Main_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        eColor.Close()
        Previewer.Close()
        LoadConfig()
        FillSkins()
        FillVehicles()
        FillSounds()
        FillAnims()
        FillWeapons()
        FillMapIcons()
        PictureBox3.Image = ScaleImage(My.Resources.Map, 76, ScaleType.Down)

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
            Label22.Text = "Command (both):"
            Label23.Visible = False
            TextBox23.Visible = False
            LinkLabel2.Top = 48
            TextBox24.Top = 45
        Else
            Label22.Text = "Command (open):"
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

#End Region

#Region "Tabs"

#Region "Teleports"

#Region "Texts Restrictions"

    Private Sub TextBox3_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox3.KeyPress
        If Not IsNumeric(e.KeyChar) And Not Char.IsControl(e.KeyChar) And e.KeyChar <> "." And Asc(e.KeyChar) <> 8 Then e.Handled = True
        If e.KeyChar = "." And TextBox3.Text.IndexOf(".") <> -1 Then e.Handled = True
    End Sub

    Private Sub TextBox4_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox4.KeyPress
        If Not IsNumeric(e.KeyChar) And Not Char.IsControl(e.KeyChar) And e.KeyChar <> "." And Asc(e.KeyChar) <> 8 Then e.Handled = True
        If e.KeyChar = "." And TextBox4.Text.IndexOf(".") <> -1 Then e.Handled = True
    End Sub

    Private Sub TextBox5_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox5.KeyPress
        If Not IsNumeric(e.KeyChar) And Not Char.IsControl(e.KeyChar) And e.KeyChar <> "." And Asc(e.KeyChar) <> 8 Then e.Handled = True
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
        eColor.Show()
        eColor.Focus()
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        gSender = CC.Help
        eColor.Show()
        eColor.Focus()
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
            End Select
            Exit Sub
        End If
        If RadioButton3.Checked = True Then
            If RadioButton1.Checked = True Then
                TextBox1.Text = "if(!strcmp(cmdtext, ""/" & TextBox9.Text & """, true)){" & vbNewLine & _
                "   SetPlayerPos(playerid, " & TextBox3.Text & ", " & TextBox4.Text & ", " & TextBox5.Text & ");" & vbNewLine & _
                "   SetPlayerFacingAngle(playerid, " & TextBox8.Text & ");" & vbNewLine & _
                "   SetPlayerInterior(playerid, " & TextBox7.Text & ");" & vbNewLine & _
                "   SetPlayerVirtualWorld(playerid, " & TextBox6.Text & ");" & vbNewLine
                If CheckBox1.Checked = True Then
                    TextBox1.Text += "   return SendClientMessage(playerid, " & Config.C_Msg.Name & ", """ & TextBox2.Text & """);" & vbNewLine & _
                    "}"
                Else
                    TextBox1.Text += "   return 1;" & vbNewLine & _
                    "}"
                End If
            Else
                TextBox1.Text = "if(!strcmp(cmdtext, ""/" & TextBox9.Text & """, true)){" & vbNewLine & _
                "   if(!IsPlayerInAnyVehicle(playerid)){" & vbNewLine & _
                "       SetPlayerPos(playerid, " & TextBox3.Text & ", " & TextBox4.Text & ", " & TextBox5.Text & ");" & vbNewLine & _
                "       SetPlayerFacingAngle(playerid, " & TextBox8.Text & ");" & vbNewLine & _
                "       SetPlayerInterior(playerid, " & TextBox7.Text & ");" & vbNewLine & _
                "       SetPlayerVirtualWorld(playerid, " & TextBox6.Text & ");" & vbNewLine & _
                "   }" & vbNewLine & _
                "   else{" & vbNewLine & _
                "       new veh = GetPlayerVehicleID(playerid);" & vbNewLine & _
                "       SetVehiclePos(veh, " & TextBox3.Text & ", " & TextBox4.Text & ", " & TextBox5.Text & ");" & vbNewLine & _
                "       SetVehicleZAngle(veh, " & TextBox8.Text & ");" & vbNewLine & _
                "       LinkVehicleToInterior(veh, " & TextBox7.Text & ");" & vbNewLine & _
                "       SetVehicleVirtualWorld(veh, " & TextBox6.Text & ");" & vbNewLine & _
                "       PutPlayerInVehicle(playerid, veh, 0);" & vbNewLine & _
                "   }" & vbNewLine
                If CheckBox1.Checked = True Then
                    TextBox1.Text += "   return SendClientMessage(playerid, " & Config.C_Msg.Name & ", """ & TextBox2.Text & """);" & vbNewLine & _
                    "}"
                Else
                    TextBox1.Text += "   return 1;" & vbNewLine & _
                    "}"
                End If
            End If
        ElseIf RadioButton4.Checked = True Then
            If RadioButton1.Checked = True Then
                TextBox1.Text = "dcmd(" & TextBox9.Text & ", " & TextBox9.Text.Length & ", " & "cmdtext);" & vbNewLine & vbNewLine & _
                "dcmd_" & TextBox9.Text & "(playerid, params[])" & vbNewLine & _
                "{" & vbNewLine & _
                "   #pragma unused params" & vbNewLine & _
                "   SetPlayerPos(playerid, " & TextBox3.Text & ", " & TextBox4.Text & ", " & TextBox5.Text & ");" & vbNewLine & _
                "   SetPlayerFacingAngle(playerid, " & TextBox8.Text & ");" & vbNewLine & _
                "   SetPlayerInterior(playerid, " & TextBox7.Text & ");" & vbNewLine & _
                "   SetPlayerVirtualWorld(playerid, " & TextBox6.Text & ");" & vbNewLine
                If CheckBox1.Checked = True Then
                    TextBox1.Text += "   return SendClientMessage(playerid, " & Config.C_Msg.Name & ", """ & TextBox2.Text & """);" & vbNewLine & _
                    "}"
                Else
                    TextBox1.Text += "   return 1;" & vbNewLine & _
                    "}"
                End If
            Else
                TextBox1.Text = "dcmd(" & TextBox9.Text & ", " & TextBox9.Text.Length & ", " & "cmdtext);" & vbNewLine & vbNewLine & _
                "dcmd_" & TextBox9.Text & "(playerid, params[])" & vbNewLine & _
                "{" & vbNewLine & _
                "   #pragma unused params" & vbNewLine & _
                "   if(!IsPlayerInAnyVehicle(playerid)){" & vbNewLine & _
                "       SetPlayerPos(playerid, " & TextBox3.Text & ", " & TextBox4.Text & ", " & TextBox5.Text & ");" & vbNewLine & _
                "       SetPlayerFacingAngle(playerid, " & TextBox8.Text & ");" & vbNewLine & _
                "       SetPlayerInterior(playerid, " & TextBox7.Text & ");" & vbNewLine & _
                "       SetPlayerVirtualWorld(playerid, " & TextBox6.Text & ");" & vbNewLine & _
                "   }" & vbNewLine & _
                "   else{" & vbNewLine & _
                "       new veh = GetPlayerVehicleID(playerid);" & vbNewLine & _
                "       SetVehiclePos(veh, " & TextBox3.Text & ", " & TextBox4.Text & ", " & TextBox5.Text & ");" & vbNewLine & _
                "       SetVehicleZAngle(veh, " & TextBox8.Text & ");" & vbNewLine & _
                "       LinkVehicleToInterior(veh, " & TextBox7.Text & ");" & vbNewLine & _
                "       SetVehicleVirtualWorld(veh, " & TextBox6.Text & ");" & vbNewLine & _
                "       PutPlayerInVehicle(playerid, veh, 0);" & vbNewLine & _
                "   }" & vbNewLine
                If CheckBox1.Checked = True Then
                    TextBox1.Text += "   return SendClientMessage(playerid, " & Config.C_Msg.Name & ", """ & TextBox2.Text & """);" & vbNewLine & _
                    "}"
                Else
                    TextBox1.Text += "   return 1;" & vbNewLine & _
                    "}"
                End If
            End If
        ElseIf RadioButton5.Checked = True Then
            If RadioButton1.Checked = True Then
                TextBox1.Text = "CMD:" & TextBox9.Text & "(playerid, params[])" & vbNewLine & _
                "{" & vbNewLine & _
                "   SetPlayerPos(playerid, " & TextBox3.Text & ", " & TextBox4.Text & ", " & TextBox5.Text & ");" & vbNewLine & _
                "   SetPlayerFacingAngle(playerid, " & TextBox8.Text & ");" & vbNewLine & _
                "   SetPlayerInterior(playerid, " & TextBox7.Text & ");" & vbNewLine & _
                "   SetPlayerVirtualWorld(playerid, " & TextBox6.Text & ");" & vbNewLine
                If CheckBox1.Checked = True Then
                    TextBox1.Text += "   return SendClientMessage(playerid, " & Config.C_Msg.Name & ", """ & TextBox2.Text & """);" & vbNewLine & _
                    "}"
                Else
                    TextBox1.Text += "   return 1;" & vbNewLine & _
                    "}"
                End If
            Else
                TextBox1.Text = "CMD:" & TextBox9.Text & "(playerid, params[])" & vbNewLine & _
                "{" & vbNewLine & _
                "   if(!IsPlayerInAnyVehicle(playerid)){" & vbNewLine & _
                "       SetPlayerPos(playerid, " & TextBox3.Text & ", " & TextBox4.Text & ", " & TextBox5.Text & ");" & vbNewLine & _
                "       SetPlayerFacingAngle(playerid, " & TextBox8.Text & ");" & vbNewLine & _
                "       SetPlayerInterior(playerid, " & TextBox7.Text & ");" & vbNewLine & _
                "       SetPlayerVirtualWorld(playerid, " & TextBox6.Text & ");" & vbNewLine & _
                "   }" & vbNewLine & _
                "   else{" & vbNewLine & _
                "       new veh = GetPlayerVehicleID(playerid);" & vbNewLine & _
                "       SetVehiclePos(veh, " & TextBox3.Text & ", " & TextBox4.Text & ", " & TextBox5.Text & ");" & vbNewLine & _
                "       SetVehicleZAngle(veh, " & TextBox8.Text & ");" & vbNewLine & _
                "       LinkVehicleToInterior(veh, " & TextBox7.Text & ");" & vbNewLine & _
                "       SetVehicleVirtualWorld(veh, " & TextBox6.Text & ");" & vbNewLine & _
                "       PutPlayerInVehicle(playerid, veh, 0);" & vbNewLine & _
                "   }" & vbNewLine
                If CheckBox1.Checked = True Then
                    TextBox1.Text += "   return SendClientMessage(playerid, " & Config.C_Msg.Name & ", """ & TextBox2.Text & """);" & vbNewLine & _
                    "}"
                Else
                    TextBox1.Text += "   return 1;" & vbNewLine & _
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
                End Select
                TextBox2.Focus()
                Exit Sub
                Exit Sub
            End If
            If RadioButton1.Checked = True Then
                TextBox1.Text = "YCMD:" & TextBox9.Text & "(playerid, params[], help)" & vbNewLine & _
                "{" & vbNewLine & _
                "   If(help) return SendClientMessage(playerid, " & Config.C_Help.Name & ", """ & TextBox10.Text & """);" & vbNewLine & _
                "   if(!IsPlayerInAnyVehicle(playerid)){" & vbNewLine & _
                "       SetPlayerPos(playerid, " & TextBox3.Text & ", " & TextBox4.Text & ", " & TextBox5.Text & ");" & vbNewLine & _
                "       SetPlayerFacingAngle(playerid, " & TextBox8.Text & ");" & vbNewLine & _
                "       SetPlayerInterior(playerid, " & TextBox7.Text & ");" & vbNewLine & _
                "       SetPlayerVirtualWorld(playerid, " & TextBox6.Text & ");" & vbNewLine & _
                "   }" & vbNewLine & _
                "   else{" & vbNewLine & _
                "       new veh = GetPlayerVehicleID(playerid);" & vbNewLine & _
                "       SetVehiclePos(veh, " & TextBox3.Text & ", " & TextBox4.Text & ", " & TextBox5.Text & ");" & vbNewLine & _
                "       SetVehicleZAngle(veh, " & TextBox8.Text & ");" & vbNewLine & _
                "       LinkVehicleToInterior(veh, " & TextBox7.Text & ");" & vbNewLine & _
                "       SetVehicleVirtualWorld(veh, " & TextBox6.Text & ");" & vbNewLine & _
                "       PutPlayerInVehicle(playerid, veh, 0);" & vbNewLine & _
                "   }" & vbNewLine
                If CheckBox1.Checked = True Then
                    TextBox1.Text += "   return SendClientMessage(playerid, " & Config.C_Msg.Name & ", """ & TextBox2.Text & """);" & vbNewLine & _
                    "}"
                Else
                    TextBox1.Text += "   return 1;" & vbNewLine & _
                    "}"
                End If
            Else
                TextBox1.Text = "YCMD:" & TextBox9.Text & "(playerid, params[], help)" & vbNewLine & _
                "{" & vbNewLine & _
                "   If(help) return SendClientMessage(playerid, " & Config.C_Help.Name & ", """ & TextBox10.Text & """);" & vbNewLine & _
                "   if(!IsPlayerInAnyVehicle(playerid)){" & vbNewLine & _
                "       SetPlayerPos(playerid, " & TextBox3.Text & ", " & TextBox4.Text & ", " & TextBox5.Text & ");" & vbNewLine & _
                "       SetPlayerFacingAngle(playerid, " & TextBox8.Text & ");" & vbNewLine & _
                "       SetPlayerInterior(playerid, " & TextBox7.Text & ");" & vbNewLine & _
                "       SetPlayerVirtualWorld(playerid, " & TextBox6.Text & ");" & vbNewLine & _
                "   }" & vbNewLine & _
                "   else{" & vbNewLine & _
                "       new veh = GetPlayerVehicleID(playerid);" & vbNewLine & _
                "       SetVehiclePos(veh, " & TextBox3.Text & ", " & TextBox4.Text & ", " & TextBox5.Text & ");" & vbNewLine & _
                "       SetVehicleZAngle(veh, " & TextBox8.Text & ");" & vbNewLine & _
                "       LinkVehicleToInterior(veh, " & TextBox7.Text & ");" & vbNewLine & _
                "       SetVehicleVirtualWorld(veh, " & TextBox6.Text & ");" & vbNewLine & _
                "       PutPlayerInVehicle(playerid, veh, 0);" & vbNewLine & _
                "   }" & vbNewLine
                If CheckBox1.Checked = True Then
                    TextBox1.Text += "   return SendClientMessage(playerid, " & Config.C_Msg.Name & ", """ & TextBox2.Text & """);" & vbNewLine & _
                    "}"
                Else
                    TextBox1.Text += "   return 1;" & vbNewLine & _
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
        If Not IsNumeric(e.KeyChar) And Not Char.IsControl(e.KeyChar) And e.KeyChar <> "." And Asc(e.KeyChar) <> 8 Then e.Handled = True
        If e.KeyChar = "." And TextBox12.Text.IndexOf(".") <> -1 Then e.Handled = True
    End Sub

    Private Sub TextBox13_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox13.KeyPress
        If Not IsNumeric(e.KeyChar) And Not Char.IsControl(e.KeyChar) And e.KeyChar <> "." And Asc(e.KeyChar) <> 8 Then e.Handled = True
        If e.KeyChar = "." And TextBox13.Text.IndexOf(".") <> -1 Then e.Handled = True
    End Sub

    Private Sub TextBox14_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox14.KeyPress
        If Not IsNumeric(e.KeyChar) And Not Char.IsControl(e.KeyChar) And e.KeyChar <> "." And Asc(e.KeyChar) <> 8 Then e.Handled = True
        If e.KeyChar = "." And TextBox14.Text.IndexOf(".") <> -1 Then e.Handled = True
    End Sub

    Private Sub TextBox15_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox15.KeyPress
        If Not IsNumeric(e.KeyChar) And Not Char.IsControl(e.KeyChar) And e.KeyChar <> "." And Asc(e.KeyChar) <> 8 Then e.Handled = True
        If e.KeyChar = "." And TextBox15.Text.IndexOf(".") <> -1 Then e.Handled = True
    End Sub

    Private Sub TextBox16_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox16.KeyPress
        If Not IsNumeric(e.KeyChar) And Not Char.IsControl(e.KeyChar) And e.KeyChar <> "." And Asc(e.KeyChar) <> 8 Then e.Handled = True
        If e.KeyChar = "." And TextBox16.Text.IndexOf(".") <> -1 Then e.Handled = True
    End Sub

    Private Sub TextBox17_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox17.KeyPress
        If Not IsNumeric(e.KeyChar) And Not Char.IsControl(e.KeyChar) And e.KeyChar <> "." And Asc(e.KeyChar) <> 8 Then e.Handled = True
        If e.KeyChar = "." And TextBox17.Text.IndexOf(".") <> -1 Then e.Handled = True
    End Sub

    Private Sub TextBox18_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox18.KeyPress
        If Not IsNumeric(e.KeyChar) And Not Char.IsControl(e.KeyChar) And e.KeyChar <> "." And Asc(e.KeyChar) <> 8 Then e.Handled = True
        If e.KeyChar = "." And TextBox18.Text.IndexOf(".") <> -1 Then e.Handled = True
    End Sub

    Private Sub TextBox19_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox19.KeyPress
        If Not IsNumeric(e.KeyChar) And Not Char.IsControl(e.KeyChar) And e.KeyChar <> "." And Asc(e.KeyChar) <> 8 Then e.Handled = True
        If e.KeyChar = "." And TextBox19.Text.IndexOf(".") <> -1 Then e.Handled = True
    End Sub

    Private Sub TextBox20_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox20.KeyPress
        If Not IsNumeric(e.KeyChar) And Not Char.IsControl(e.KeyChar) And e.KeyChar <> "." And Asc(e.KeyChar) <> 8 Then e.Handled = True
        If e.KeyChar = "." And TextBox20.Text.IndexOf(".") <> -1 Then e.Handled = True
    End Sub

    Private Sub TextBox21_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox21.KeyPress
        If Not IsNumeric(e.KeyChar) And Not Char.IsControl(e.KeyChar) And e.KeyChar <> "." And Asc(e.KeyChar) <> 8 Then e.Handled = True
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

    Private Sub TextBox16_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox16.TextChanged
        TextBox16.Text = Regex.Replace(TextBox16.Text, BadChars, "")
    End Sub

    Private Sub TextBox17_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox17.TextChanged
        TextBox17.Text = Regex.Replace(TextBox17.Text, BadChars, "")
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
        eColor.Show()
        eColor.Focus()
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
        eColor.Show()
        eColor.Focus()
    End Sub

#End Region

#Region "Visual"

    Private Sub RadioButton10_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton10.CheckedChanged
        If RadioButton10.Checked = True Then
            Select Case Config.Idioma
                Case Lang.English
                    Label22.Text = "Command (both):"
                Case Else
                    Label22.Text = "Cmd (ambos):"
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
                Case Else
                    Label22.Text = "Comando (aberta):"
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
            End Select
            Exit Sub
        End If
        If TextBox16.Text.Length = 0 Then
            Select Case Config.Idioma
                Case Lang.English
                    MsgBox("You must enter a radius of rotation for opening.", MsgBoxStyle.Critical, "Error")
                Case Lang.Español
                    MsgBox("Debes ingresar un radio de rotacion para la apertura.", MsgBoxStyle.Critical, "Error")
                Case Lang.Portugues
                    MsgBox("Você deve entrar com um raio de rotação para a abertura.", MsgBoxStyle.Critical, "Error")
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
            End Select
            Exit Sub
        End If
        If TextBox17.Text.Length = 0 Then
            Select Case Config.Idioma
                Case Lang.English
                    MsgBox("You must enter a radio to close the door.", MsgBoxStyle.Critical, "Error")
                Case Lang.Español
                    MsgBox("Debes ingresar un radio para cerrar la puerta.", MsgBoxStyle.Critical, "Error")
                Case Lang.Portugues
                    MsgBox("Você deve digitar um rádio para trancar a porta.", MsgBoxStyle.Critical, "Error")
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
                End Select
                Exit Sub
            End If
            TextBox11.Text = "new Gate;" & vbNewLine & vbNewLine
            If CheckBox4.Checked = True Then
                TextBox11.Text += "public OnFilterScriptInit()" & vbNewLine
            Else
                TextBox11.Text += "public OnGameModeInit()" & vbNewLine
            End If
            TextBox11.Text += "{" & vbNewLine & _
            "   Gate = CreateObject(" & TextBox24.Text & ", " & TextBox21.Text & ", " & TextBox20.Text & ", " & TextBox19.Text & ", 0.0, 0.0, " & TextBox17.Text & ", 100.0);" & vbNewLine & _
            "   return 1;" & vbNewLine & _
            "}" & vbNewLine & vbNewLine & _
            "public OnPlayerCommandText(playerid, cmdtext[])" & vbNewLine & _
            "{" & vbNewLine & _
            "   if(!strcmp(cmdtext, ""/" & TextBox22.Text & """, true)){" & vbNewLine & _
            "       if(IsPlayerInRangeOfPoint(playerid, " & TextBox14.Text & ", " & TextBox21.Text & ", " & TextBox20.Text & ", " & TextBox19.Text & ")){" & vbNewLine & _
            "           MoveObject(Gate, " & TextBox15.Text & ", " & TextBox12.Text & ", " & TextBox13.Text & ", " & TextBox27.Text & ");" & vbNewLine & _
            "           SetObjectRot(Gate, 0.0, 0.0, " & TextBox16.Text & ");" & vbNewLine
            If CheckBox2.Checked = True Then
                TextBox11.Text += "           return SendClientMessage(playerid, " & Config.C_Open.Name & ", """ & TextBox25.Text & """);" & vbNewLine
            Else
                TextBox11.Text += "           return 1;" & vbNewLine
            End If
            TextBox11.Text += "       }" & vbNewLine & _
            "   }" & vbNewLine & _
            "   else if(strcmp(cmdtext, ""/" & TextBox23.Text & """, true)){" & vbNewLine & _
            "       if(IsPlayerInRangeOfPoint(playerid, " & TextBox18.Text & ", " & TextBox21.Text & ", " & TextBox20.Text & ", " & TextBox19.Text & ")){" & vbNewLine & _
            "           MoveObject(Gate, " & TextBox21.Text & ", " & TextBox20.Text & ", " & TextBox19.Text & ", " & TextBox27.Text & ");" & vbNewLine & _
            "           SetObjectRot(Gate, 0.0, 0.0, " & TextBox17.Text & ");" & vbNewLine
            If CheckBox2.Checked = True Then
                TextBox11.Text += "           return SendClientMessage(playerid, " & Config.C_Close.Name & ", """ & TextBox26.Text & """);" & vbNewLine
            Else
                TextBox11.Text += "           return 1;" & vbNewLine
            End If
            TextBox11.Text += "       }" & vbNewLine & _
            "   }" & vbNewLine & _
            "   return 0; " & vbNewLine & _
            "}"
        ElseIf RadioButton8.Checked = True Then
            If TextBox22.Text.Length = 0 Then
                Select Case Config.Idioma
                    Case Lang.English
                        MsgBox("You must enter the name of the command to open the door.", MsgBoxStyle.Critical, "Error")
                    Case Lang.Español
                        MsgBox("Debes ingresar el nombre del comando para abrir la puerta.", MsgBoxStyle.Critical, "Error")
                    Case Lang.Portugues
                        MsgBox("Você deve digitar o nome do comando para abrir a porta.", MsgBoxStyle.Critical, "Error")
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
                End Select
                Exit Sub
            End If
            TextBox11.Text = "new Gate;" & vbNewLine & vbNewLine
            If CheckBox4.Checked = True Then
                TextBox11.Text += "public OnFilterScriptInit()" & vbNewLine
            Else
                TextBox11.Text += "public OnGameModeInit()" & vbNewLine
            End If
            TextBox11.Text += "{" & vbNewLine & _
            "   Gate = CreateObject(" & TextBox24.Text & ", " & TextBox21.Text & ", " & TextBox20.Text & ", " & TextBox19.Text & ", 0.0, 0.0, " & TextBox17.Text & ", 100.0);" & vbNewLine & _
            "   return 1;" & vbNewLine & _
            "}" & vbNewLine & vbNewLine & _
            "public OnPlayerCommandText(playerid, cmdtext[])" & vbNewLine & _
            "{" & vbNewLine & _
            "   if(!strcmp(cmdtext, ""/" & TextBox22.Text & """, true)){" & vbNewLine & _
            "       if(IsPlayerInRangeOfPoint(playerid, " & TextBox14.Text & ", " & TextBox21.Text & ", " & TextBox20.Text & ", " & TextBox19.Text & ")){" & vbNewLine & _
            "           SetTimerEx(""CloseGate"", " & TextBox28.Text & ", false, ""i"", playerid);" & vbNewLine & _
            "           MoveObject(Gate, " & TextBox15.Text & ", " & TextBox12.Text & ", " & TextBox13.Text & ", " & TextBox27.Text & ");" & vbNewLine & _
            "           SetObjectRot(Gate, 0.0, 0.0, " & TextBox16.Text & ");" & vbNewLine
            If CheckBox2.Checked = True Then
                TextBox11.Text += "           return SendClientMessage(playerid, " & Config.C_Open.Name & ", """ & TextBox25.Text & """);" & vbNewLine
            Else
                TextBox11.Text += "           return 1;" & vbNewLine
            End If
            TextBox11.Text += "       }" & vbNewLine & _
            "   }" & vbNewLine & _
            "   return 0;" & vbNewLine & _
            "}" & vbNewLine & vbNewLine & _
            "forward CloseGate(playerid);" & vbNewLine & _
            "public CloseGate(playerid)" & vbNewLine & _
            "{" & vbNewLine & _
            "   MoveObject(Gate, " & TextBox21.Text & ", " & TextBox20.Text & ", " & TextBox19.Text & ", " & TextBox27.Text & ");" & vbNewLine & _
            "   SetObjectRot(Gate, 0.0, 0.0, " & TextBox17.Text & ");" & vbNewLine
            If CheckBox2.Checked = True Then TextBox11.Text += "   return SendClientMessage(playerid, " & Config.C_Close.Name & ", """ & TextBox26.Text & """);" & vbNewLine
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
                End Select
                Exit Sub
            End If
            TextBox11.Text = "new Gate, bool:GateClose, bool:GateMoving;" & vbNewLine & vbNewLine
            If CheckBox4.Checked = True Then
                TextBox11.Text += "public OnFilterScriptInit()" & vbNewLine
            Else
                TextBox11.Text += "public OnGameModeInit()" & vbNewLine
            End If
            TextBox11.Text += "{" & vbNewLine & _
            "   Gate = CreateObject(" & TextBox24.Text & ", " & TextBox21.Text & ", " & TextBox20.Text & ", " & TextBox19.Text & ", 0.0, 0.0, " & TextBox17.Text & ", 100.0);" & vbNewLine & _
            "   SetTimer(""GateCheck"", " & TextBox28.Text & ", true);" & vbNewLine & _
            "   return 1;" & vbNewLine & _
            "}" & vbNewLine & vbNewLine & _
            "forward GateCheck();" & vbNewLine & _
            "public GateCheck()" & vbNewLine & _
            "{" & vbNewLine & _
            "   if(!GateMoving)){" & vbNewLine & _
            "       for(new i; i<GetMaxPlayers(); i++){" & vbNewLine & _
            "           if(IsPlayerConnected(i && IsPlayerInRangeOfPoint(i, " & TextBox14.Text & ", " & TextBox21.Text & ", " & TextBox20.Text & ", " & TextBox19.Text & ")){" & vbNewLine & _
            "              if(GateClose){" & vbNewLine & _
            "                   GateClose = false;" & vbNewLine & _
            "                   GateMoving = true;" & vbNewLine & _
            "                   MoveObject(Gate, " & TextBox15.Text & ", " & TextBox12.Text & ", " & TextBox13.Text & ", " & TextBox27.Text & ");" & vbNewLine & _
            "                   SetObjectRot(Gate, 0.0, 0.0, " & TextBox16.Text & ");" & vbNewLine
            If CheckBox2.Checked = True Then TextBox11.Text += "                   SendClientMessage(i, " & Config.C_Open.Name & ", """ & TextBox25.Text & """);" & vbNewLine
            TextBox11.Text += "               }" & vbNewLine & _
            "               else{" & vbNewLine & _
            "                   GateClose = true;" & vbNewLine & _
            "                   GateMoving = true;" & vbNewLine & _
            "                   MoveObject(Gate, " & TextBox21.Text & ", " & TextBox20.Text & ", " & TextBox19.Text & ", " & TextBox27.Text & ");" & vbNewLine & _
            "                   SetObjectRot(Gate, 0.0, 0.0, " & TextBox17.Text & ");" & vbNewLine
            If CheckBox2.Checked = True Then TextBox11.Text += "                   SendClientMessage(i, " & Config.C_Close.Name & ", """ & TextBox26.Text & """);" & vbNewLine
            TextBox11.Text += "               }" & vbNewLine & _
            "           }" & vbNewLine & _
            "       }" & vbNewLine & _
            "   }" & vbNewLine & _
            "   else{" & vbNewLine & _
            "       new Float:P[3];" & vbNewLine & _
            "       GetObjectPos(Gate, P[0], P[1], P[2]);" & vbNewLine & _
            "       if(!floatcmp(P[0], " & TextBox21.Text & ") && !floatcmp(P[1], " & TextBox20.Text & ") && !floatcmp(P[2], " & TextBox19.Text & ")" & vbNewLine & _
            "           GateMoving = false;" & vbNewLine & _
            "   }" & vbNewLine & _
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
                End Select
                Exit Sub
            End If
            TextBox11.Text = "new Gate, bool:GateClose;" & vbNewLine & vbNewLine
            If CheckBox4.Checked = True Then
                TextBox11.Text += "public OnFilterScriptInit()" & vbNewLine
            Else
                TextBox11.Text += "public OnGameModeInit()" & vbNewLine
            End If
            TextBox11.Text += "{" & vbNewLine & _
            "   Gate = CreateObject(" & TextBox24.Text & ", " & TextBox21.Text & ", " & TextBox20.Text & ", " & TextBox19.Text & ", 0.0, 0.0, " & TextBox17.Text & ", 100.0);" & vbNewLine & _
            "   return 1;" & vbNewLine & _
            "}" & vbNewLine & vbNewLine & _
            "public OnPlayerCommandText(playerid, cmdtext[])" & vbNewLine & _
            "{" & vbNewLine & _
            "   if(!strcmp(cmdtext, ""/" & TextBox22.Text & """, true)){" & vbNewLine & _
            "       if(GateClose){" & vbNewLine & _
            "          if(IsPlayerInRangeOfPoint(playerid, " & TextBox14.Text & ", " & TextBox21.Text & ", " & TextBox20.Text & ", " & TextBox19.Text & ")){" & vbNewLine & _
            "               GateClose = false;" & vbNewLine & _
            "               MoveObject(Gate, " & TextBox15.Text & ", " & TextBox12.Text & ", " & TextBox13.Text & ", " & TextBox27.Text & ");" & vbNewLine & _
            "               SetObjectRot(Gate, 0.0, 0.0, " & TextBox16.Text & ");" & vbNewLine
            If CheckBox2.Checked = True Then
                TextBox11.Text += "               return SendClientMessage(playerid, " & Config.C_Open.Name & ", """ & TextBox25.Text & """);" & vbNewLine
            Else
                TextBox11.Text += "               return 1;" & vbNewLine
            End If
            TextBox11.Text += "          }" & vbNewLine & _
            "       }" & vbNewLine & _
            "       else{" & vbNewLine & _
            "           if(IsPlayerInRangeOfPoint(playerid, " & TextBox18.Text & ", " & TextBox21.Text & ", " & TextBox20.Text & ", " & TextBox19.Text & ")){" & vbNewLine & _
            "               GateClose = true;" & vbNewLine & _
            "               MoveObject(Gate, " & TextBox21.Text & ", " & TextBox20.Text & ", " & TextBox19.Text & ", " & TextBox27.Text & ");" & vbNewLine & _
            "               SetObjectRot(Gate, 0.0, 0.0, " & TextBox17.Text & ");" & vbNewLine
            If CheckBox2.Checked = True Then
                TextBox11.Text += "               return SendClientMessage(playerid, " & Config.C_Close.Name & ", """ & TextBox26.Text & """);" & vbNewLine
            Else
                TextBox11.Text += "               return 1;" & vbNewLine
            End If
            TextBox11.Text += "           }" & vbNewLine & _
             "       }" & vbNewLine & _
            "   }" & vbNewLine & _
            "   return 0;" & vbNewLine & _
            "}"
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
        TextBox16.Text = "0.0"
        TextBox17.Text = "0.0"
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
        If Not IsNumeric(e.KeyChar) And Not Char.IsControl(e.KeyChar) And e.KeyChar <> "." And Asc(e.KeyChar) <> 8 Then e.Handled = True
        If e.KeyChar = "." And TextBox31.Text.IndexOf(".") <> -1 Then e.Handled = True
    End Sub

    Private Sub TextBox32_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox32.KeyPress
        If Not IsNumeric(e.KeyChar) And Not Char.IsControl(e.KeyChar) And Asc(e.KeyChar) <> 8 Then e.Handled = True
    End Sub

    Private Sub TextBox35_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox35.KeyPress
        If Not IsNumeric(e.KeyChar) And Not Char.IsControl(e.KeyChar) And e.KeyChar <> "." And Asc(e.KeyChar) <> 8 Then e.Handled = True
        If e.KeyChar = "." And TextBox35.Text.IndexOf(".") <> -1 Then e.Handled = True
    End Sub

    Private Sub TextBox36_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox36.KeyPress
        If Not IsNumeric(e.KeyChar) And Not Char.IsControl(e.KeyChar) And e.KeyChar <> "." And Asc(e.KeyChar) <> 8 Then e.Handled = True
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
                    Case Else
                        TextBox34.Text = "A pickup não será exibido."
                End Select
            Case 1
                Select Case Config.Idioma
                    Case Lang.English
                        TextBox34.Text = "Not pickupable, exists all the time (Suitable for completely scripted pickups using OnPlayerPickUpPickup)."
                    Case Lang.Español
                        TextBox34.Text = "No se puede recoger, existe todo el tiempo (Adecuado para pickups totalmente scripteados mediante OnPlayerPickUpPickup)."
                    Case Else
                        TextBox34.Text = "Você não pode escolher, lá o tempo todo (Adequado para a plena roteiro de pickups OnPlayerPickUpPickup)."
                End Select
            Case 2
                Select Case Config.Idioma
                    Case Lang.English
                        TextBox34.Text = "Pickupable, respawns after some time."
                    Case Lang.Español
                        TextBox34.Text = "Se puede recoger, aparece luego de un tiempo."
                    Case Else
                        TextBox34.Text = "Você pode pegar, aparece depois de um tempo."
                End Select
            Case 3
                Select Case Config.Idioma
                    Case Lang.English
                        TextBox34.Text = "Pickupable, respawns after death."
                    Case Lang.Español
                        TextBox34.Text = "Se puede recoger, aparece luego al morir el jugador."
                    Case Else
                        TextBox34.Text = "Você pode pegar, aparece após a morte do jogador."
                End Select
            Case 4
                Select Case Config.Idioma
                    Case Lang.English
                        TextBox34.Text = "Disappears shortly after created (perhaps for weapon drops?)."
                    Case Lang.Español
                        TextBox34.Text = "Desaparece poco tiempo despues de ser creado (tal vez para arrojar armas?)."
                    Case Else
                        TextBox34.Text = "Embora logo depois de ser criado (talvez para lançar armas?)."
                End Select
            Case 5
                Select Case Config.Idioma
                    Case Lang.English
                        TextBox34.Text = "Disappears shortly after created (perhaps for weapon drops?)."
                    Case Lang.Español
                        TextBox34.Text = "Desaparece poco tiempo despues de ser creado (tal vez para arrojar armas?)."
                    Case Else
                        TextBox34.Text = "Embora logo depois de ser criado (talvez para lançar armas?)."
                End Select
            Case 8
                Select Case Config.Idioma
                    Case Lang.English
                        TextBox34.Text = "Pickupable, but has no effect. Disappears automatically."
                    Case Lang.Español
                        TextBox34.Text = "Se puede recoger pero no tiene efecto. Desaparece automaticamente."
                    Case Else
                        TextBox34.Text = "Você pode pegar, mas não tem efeito. Desaparece automaticamente."
                End Select
            Case 11
                Select Case Config.Idioma
                    Case Lang.English
                        TextBox34.Text = "Blows up a few seconds after being created (bombs?)."
                    Case Lang.Español
                        TextBox34.Text = "Explota unos segundos despues de ser creado (Bombas?)."
                    Case Else
                        TextBox34.Text = "Explode alguns segundos depois de ter sido criado (bombas?)."
                End Select
            Case 12
                Select Case Config.Idioma
                    Case Lang.English
                        TextBox34.Text = "Blows up a few seconds after being created (bombs?)."
                    Case Lang.Español
                        TextBox34.Text = "Explota unos segundos despues de ser creado (Bombas?)."
                    Case Else
                        TextBox34.Text = "Explode alguns segundos depois de ter sido criado (bombas?)."
                End Select
            Case 13
                Select Case Config.Idioma
                    Case Lang.English
                        TextBox34.Text = "Slowly decends to the ground."
                    Case Lang.Español
                        TextBox34.Text = "Desciende lentamente al piso."
                    Case Else
                        TextBox34.Text = "Descer lentamente para o chão."
                End Select
            Case 14
                Select Case Config.Idioma
                    Case Lang.English
                        TextBox34.Text = "Pickupable, but only when in a vehicle."
                    Case Lang.Español
                        TextBox34.Text = "Se puede recoger, pero solo con un vehiculo."
                    Case Else
                        TextBox34.Text = "Você pode pegar, mas apenas com um veículo."
                End Select
            Case 15
                Select Case Config.Idioma
                    Case Lang.English
                        TextBox34.Text = "Pickupable, respawns after death."
                    Case Lang.Español
                        TextBox34.Text = "Se puede recoger, aparece luego al morir el jugador."
                    Case Else
                        TextBox34.Text = "Você pode pegar, aparece após a morte do jogador."
                End Select
            Case 19
                Select Case Config.Idioma
                    Case Lang.English
                        TextBox34.Text = "Pickupable, but has no effect (information icons?)."
                    Case Lang.Español
                        TextBox34.Text = "Se puede recoger, pero no tiene efecto (iconos de informacion?)."
                    Case Else
                        TextBox34.Text = "Você pode pegar, mas não tem efeito (ícones de informação?)."
                End Select
            Case 22
                Select Case Config.Idioma
                    Case Lang.English
                        TextBox34.Text = "Pickupable, respawns after death."
                    Case Lang.Español
                        TextBox34.Text = "Se puede recoger, aparece luego al morir el jugador."
                    Case Else
                        TextBox34.Text = "Você pode pegar, aparece após a morte do jogador."
                End Select
            Case 23
                Select Case Config.Idioma
                    Case Lang.English
                        TextBox34.Text = "Pickupable, but doesn't disappear on pickup."
                    Case Lang.Español
                        TextBox34.Text = "Se puede recoger, pero no desaparece."
                    Case Else
                        TextBox34.Text = "Você pode pegar, mas não desaparece."
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
            End Select
            Exit Sub
        End If
        TextBox33.Text = "new " & TextBox30.Text & ";" & vbNewLine & vbNewLine
        If CheckBox5.Checked = True Then
            TextBox33.Text += "public OnFilterSciptInit()" & vbNewLine
        Else
            TextBox33.Text += "public OnGameModeInit()" & vbNewLine
        End If
        TextBox33.Text += "{" & vbNewLine & _
        "   " & TextBox30.Text & " = CreatePickup(" & TextBox29.Text & ", " & ComboBox1.Text & ", " & TextBox36.Text & ", " & TextBox35.Text & ", " & TextBox31.Text & ", " & TextBox32.Text & ");" & vbNewLine & _
        "   return 1;" & vbNewLine & _
        "}" & vbNewLine & vbNewLine & _
        "public OnPlayerPickUpPickup(playerid, pickupid)" & vbNewLine & _
        "{" & vbNewLine & _
        "   if(pickupid == " & TextBox30.Text & "){" & vbNewLine & _
        "       //Do something here" & vbNewLine & _
        "   }" & vbNewLine & _
        "   return 1;" & vbNewLine & _
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
            Case Else
                If TextBox40.Text.IndexOf("PRESSIONE ENTER PARA CRIAR UM SALTO DE LINHA NO TEXTO") <> -1 Then
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
                Case Else
                    TextBox40.Text = vbNewLine & vbNewLine & "PRESSIONE ENTER PARA CRIAR UM SALTO DE LINHA NO TEXTO OU" & vbNewLine & "TAB PARA CRIAR UMA TABULAÇÃO"
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
        Previewer.RichTextBox1.Text = ProcessText(TextBox40.Text)
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
            End Select
            Exit Sub
        End If
        If TextBox41.Text.Length = 0 Then
            Select Case Config.Idioma
                Case Lang.English
                    MsgBox("You enter text for the button 1.", MsgBoxStyle.Critical, "Error")
                Case Lang.Español
                    MsgBox("Debes entrar un texto para el boton 1.", MsgBoxStyle.Critical, "Error")
                Case Lang.Portugues
                    MsgBox("Inserir o texto para o botão 1.", MsgBoxStyle.Critical, "Error")
            End Select
            Exit Sub
        End If
        If TextBox40.Text.Length = 0 Or TextBox40.Text.IndexOf("PRESS ENTER TO CREATE A JUMP LINE ON THE TEXT") <> -1 Then
            Select Case Config.Idioma
                Case Lang.English
                    MsgBox("You must enter a text for the dialog.", MsgBoxStyle.Critical, "Error")
                Case Lang.Español
                    MsgBox("Debes ingresar un texto para el dialogo.", MsgBoxStyle.Critical, "Error")
                Case Lang.Portugues
                    MsgBox("Você deve digitar um texto para o diálogo.", MsgBoxStyle.Critical, "Error")
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
        "{" & vbNewLine & "   if(dialogid == " & DID & "){" & vbNewLine
        Select Case ComboBox2.SelectedIndex
            Case 0
                If TextBox42.Text.Length > 0 Then
                    TextBox37.Text += "      if(response){" & vbNewLine & _
                    "      }" & vbNewLine & _
                    "         //The player has pressed """ & TextBox41.Text & """." & vbNewLine & _
                    "      }" & vbNewLine & _
                    "      else{" & vbNewLine & _
                    "         //The player has pressed """ & TextBox42.Text & """." & vbNewLine & _
                    "      }" & vbNewLine
                Else
                    TextBox37.Text += "      //The player has pressed """ & TextBox41.Text & """(because it's de only avaliable button.)" & vbNewLine
                End If
            Case 1
                If TextBox42.Text.Length > 0 Then
                    TextBox37.Text += "      if(response){" & vbNewLine & _
                    "         //The player has pressed """ & TextBox41.Text & """." & vbNewLine & _
                    "      }" & vbNewLine & _
                    "      else{" & vbNewLine & _
                    "         //The player has pressed """ & TextBox42.Text & """." & vbNewLine & _
                    "      }" & vbNewLine
                Else
                    TextBox37.Text += "      //The player has pressed """ & TextBox41.Text & """(because it's de only avaliable button.)"
                End If
            Case 2
                Dim count = Counter(TextBox40.Text, "\n")
                If TextBox42.Text.Length > 0 Then
                    TextBox37.Text += "      if(response){" & vbNewLine & _
                     "         switch(listitem)){" & vbNewLine
                    For i = 0 To count
                        TextBox37.Text += "         case " & i & ":" & vbNewLine & _
                        "         {" & vbNewLine & _
                        "            //Selected Item: """ & GetLine(TextBox40.Text, i) & """" & vbNewLine & _
                        "         }" & vbNewLine
                    Next
                    TextBox37.Text += "      }" & vbNewLine & _
                        "      else{" & vbNewLine & _
                        "         //The player has pressed """ & TextBox41.Text & """." & vbNewLine & _
                        "         switch(listitem{" & vbNewLine
                    For i = 0 To count
                        TextBox37.Text += "         case " & i & ":" & vbNewLine & _
                        "         {" & vbNewLine & _
                        "            //Selected Item: """ & GetLine(TextBox40.Text, i) & """" & vbNewLine & _
                        "         }" & vbNewLine
                    Next
                    TextBox37.Text += "      }" & vbNewLine
                Else
                    TextBox37.Text += "      switch(listitem)){" & vbNewLine
                    For i = 0 To count
                        TextBox37.Text += "         case " & i & ":" & vbNewLine & _
                        "         {" & vbNewLine & _
                        "            //Selected Item: """ & GetLine(TextBox40.Text, i) & """" & vbNewLine & _
                        "         }" & vbNewLine
                    Next
                    TextBox37.Text += "      }" & vbNewLine
                End If
            Case 3
                If TextBox42.Text.Length > 0 Then
                    TextBox37.Text += "      if(response){" & vbNewLine & _
                    "         //The player has pressed """ & TextBox41.Text & """." & vbNewLine & _
                    "      }" & vbNewLine & _
                    "      else{" & vbNewLine & _
                    "         //The player has pressed """ & TextBox42.Text & """." & vbNewLine & _
                    "      }" & vbNewLine
                Else
                    TextBox37.Text += "      //The player has pressed """ & TextBox41.Text & """(because it's de only avaliable button.)"
                End If
        End Select
        TextBox37.Text += "   }" & vbNewLine & "    return 0;" & vbNewLine & "}"
    End Sub

#End Region

#Region "Extra"

    Private Sub TabPage4_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles TabPage4.Leave
        TextBox38.Clear()
        TextBox39.Clear()
        TextBox40.Text = vbNewLine & vbNewLine & "PRESS ENTER TO CREATE A JUMP LINE ON THE TEXT OR" & vbNewLine & "TAB TO CREATE A TABULATION"
        TextBox40.ForeColor = Color.Gray
        TextBox41.Clear()
        TextBox42.Clear()
        TextBox43.Text = "playerid"
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

    Private Sub TextBox47_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox47.KeyPress
        If Not IsNumeric(e.KeyChar) And Not Char.IsControl(e.KeyChar) And Asc(e.KeyChar) <> 8 Then e.Handled = True
    End Sub

#End Region

#Region "Color"

#Region "Tracks"

    Private Sub TrackBar1_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBar1.Scroll
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
    End Sub

    Private Sub TrackBar2_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBar2.Scroll
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
    End Sub

    Private Sub TrackBar3_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBar3.Scroll
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
    End Sub

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

    Private Sub TextBox44_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox44.TextChanged
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
        End If
    End Sub

    Private Sub TextBox45_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox45.TextChanged
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
        End If
    End Sub

    Private Sub TextBox46_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox46.TextChanged
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
        End If
    End Sub

    Private Sub TextBox47_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox47.TextChanged
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
                    PictureBox3.Image = ScaleImage(My.Resources.Map, 76, ScaleType.Down)
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
                            .maxY = e.Location.Y
                        Else
                            .maxY = .minY
                            .minY = e.Location.Y
                        End If
                    End If
                ElseIf PictureBox3.Height < e.Location.Y Then
                    .maxY = PictureBox3.Height - 1
                    If PictureBox3.Width > e.Location.X Then
                        If e.Location.X > .minX Then
                            .maxX = e.Location.X
                        Else
                            .maxX = .minX
                            .minX = e.Location.X
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
                TextBox59.Text = Round(MAPXY * .minX - 3000, 6)
                TextBox60.Text = Round(MAPXY * .minY - 3000, 6)
                If .maxX = PictureBox3.Width - 1 Then
                    TextBox61.Text = 3000
                Else
                    TextBox61.Text = Round((MAPXY * .maxX) - 3000, 6)
                End If
                If .maxY = PictureBox3.Height - 1 Then
                    TextBox62.Text = 3000
                Else
                    TextBox62.Text = Round((MAPXY * .maxY) - 3000, 6)
                End If
                TextBox59.Text = Regex.Replace(TextBox59.Text, ",", ".")
                TextBox60.Text = Regex.Replace(TextBox60.Text, ",", ".")
                TextBox61.Text = Regex.Replace(TextBox61.Text, ",", ".")
                TextBox62.Text = Regex.Replace(TextBox62.Text, ",", ".")
                PictureBox3.Refresh()
                PictureBox3.CreateGraphics.DrawRectangle(Pens.Red, New Rectangle(.minX, .minY, .maxX - .minX, .maxY - .minY))
                PictureBox3.CreateGraphics.Dispose()
            End With
        End If
    End Sub

    Private Sub PictureBox3_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PictureBox3.MouseUp
        If Selection.Lock = True Then
            Dim g As Graphics = Graphics.FromImage(PictureBox3.Image)
            If CheckBox8.Checked = True Then
                Areas += String.Format(Config.Clip, TextBox59.Text, TextBox60.Text, TextBox61.Text, TextBox62.Text) & vbNewLine
            End If
            If CheckBox10.Checked = False Then
                g.DrawRectangle(Pens.Red, Selection.minX, Selection.minY, Selection.maxX - Selection.minX, Selection.maxY - Selection.minY)
            Else
                g.FillRectangle(New SolidBrush(Color.FromArgb(166, 255, 0, 0)), Selection.minX, Selection.minY, Selection.maxX - Selection.minX, Selection.maxY - Selection.minY)
            End If
            g.Dispose()
            PictureBox3.Refresh()
            Selection.Lock = False
        End If
    End Sub

#End Region

#Region "Buttons"

    Private Sub Button15_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button15.Click
        If CheckBox8.Checked = False Then
            My.Computer.Clipboard.SetText(String.Format(Config.Clip, TextBox59.Text, TextBox60.Text, TextBox61.Text, TextBox62.Text, TextDataFormat.Text))
        Else
            My.Computer.Clipboard.SetText(Areas)
        End If
    End Sub

    Private Sub Button16_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button16.Click
        PictureBox3.Image = ScaleImage(My.Resources.Map, 76, ScaleType.Down)
        TextBox59.Clear()
        TextBox60.Clear()
        TextBox61.Clear()
        TextBox62.Clear()
        Areas = ""
    End Sub

    Private Sub Button17_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button17.Click
        Areas = ""
        PictureBox3.Image = ScaleImage(My.Resources.Map, 76, ScaleType.Down)
        TextBox59.Clear()
        TextBox60.Clear()
        TextBox61.Clear()
        TextBox62.Clear()
    End Sub

#End Region

#Region "Visual"

    Private Sub CheckBox8_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox8.CheckedChanged
        If CheckBox8.Checked = True Then
            Button17.Visible = True
            If TextBox59.Text.Length > 0 Then
                Areas = String.Format(Config.Clip, TextBox59.Text, TextBox60.Text, TextBox61.Text, TextBox62.Text) & vbNewLine
            End If
        Else
            Button17.Visible = False
            Areas = ""
            PictureBox3.Image = ScaleImage(My.Resources.Map, 76, ScaleType.Down)
            TextBox59.Clear()
            TextBox60.Clear()
            TextBox61.Clear()
            TextBox62.Clear()
        End If
    End Sub

    Private Sub CheckBox10_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox10.CheckedChanged
        If CheckBox10.Checked = True Then
            If CheckBox8.Checked = True Then
                If Not Areas Is Nothing Then
                    If Areas.Length > 1 Then
                        PictureBox3.Image = ScaleImage(My.Resources.Map, 76, ScaleType.Down)
                        Dim tmp As String(), line As String(), g As Graphics
                        tmp = Split(Areas, ";")
                        g = Graphics.FromImage(PictureBox3.Image)
                        For i = 0 To UBound(tmp) - 1
                            tmp(i) = Mid(tmp(i), tmp(i).IndexOf("(") + 2, tmp(i).Length - (tmp(i).IndexOf("(") + 2))
                            line = Split(tmp(i), ", ")
                            g.FillRectangle(New SolidBrush(Color.FromArgb(166, 255, 0, 0)), Convert.ToSingle((Convert.ToDecimal(line(0), New Globalization.CultureInfo("en-US")) + 3000) / MAPXY), Convert.ToSingle((Convert.ToDecimal(line(1), New Globalization.CultureInfo("en-US")) + 3000) / MAPXY), Convert.ToSingle(((Convert.ToDecimal(line(2), New Globalization.CultureInfo("en-US")) + 3000) - (Convert.ToDecimal(line(0), New Globalization.CultureInfo("en-US")) + 3000)) / MAPXY), Convert.ToSingle(((Convert.ToDecimal(line(3), New Globalization.CultureInfo("en-US")) + 3000) - (Convert.ToDecimal(line(1), New Globalization.CultureInfo("en-US")) + 3000)) / MAPXY))
                        Next
                        g.Dispose()
                        PictureBox3.Refresh()
                    End If
                End If
            Else
                PictureBox3.Image = ScaleImage(My.Resources.Map, 76, ScaleType.Down)
                Dim g As Graphics = Graphics.FromImage(PictureBox3.Image)
                g.FillRectangle(New SolidBrush(Color.FromArgb(166, 255, 0, 0)), Selection.minX, Selection.minY, Selection.maxX - Selection.minX, Selection.maxY - Selection.minY)
                g.Dispose()
                PictureBox3.Refresh()
            End If
        Else
            If CheckBox8.Checked = True Then
                If Areas.Length > 1 Then
                    PictureBox3.Image = ScaleImage(My.Resources.Map, 76, ScaleType.Down)
                    Dim tmp As String(), line As String(), g As Graphics
                    tmp = Split(Areas, ";")
                    g = Graphics.FromImage(PictureBox3.Image)
                    For i = 0 To UBound(tmp) - 1
                        tmp(i) = Mid(tmp(i), tmp(i).IndexOf("(") + 2, tmp(i).Length - (tmp(i).IndexOf("(") + 2))
                        line = Split(tmp(i), ", ")
                        g.DrawRectangle(Pens.Red, Convert.ToSingle((Convert.ToDecimal(line(0), New Globalization.CultureInfo("en-US")) + 3000) / MAPXY), Convert.ToSingle((Convert.ToDecimal(line(1), New Globalization.CultureInfo("en-US")) + 3000) / MAPXY), Convert.ToSingle(((Convert.ToDecimal(line(2), New Globalization.CultureInfo("en-US")) + 3000) - (Convert.ToDecimal(line(0), New Globalization.CultureInfo("en-US")) + 3000)) / MAPXY), Convert.ToSingle(((Convert.ToDecimal(line(3), New Globalization.CultureInfo("en-US")) + 3000) - (Convert.ToDecimal(line(1), New Globalization.CultureInfo("en-US")) + 3000)) / MAPXY))
                    Next
                    g.Dispose()
                    PictureBox3.Refresh()
                End If
            Else
                PictureBox3.Image = ScaleImage(My.Resources.Map, 76, ScaleType.Down)
                Dim g As Graphics = Graphics.FromImage(PictureBox3.Image)
                g.DrawRectangle(Pens.Red, Selection.minX, Selection.minY, Selection.maxX - Selection.minX, Selection.maxY - Selection.minY)
                g.Dispose()
                PictureBox3.Refresh()
            End If
        End If
    End Sub

#End Region

#End Region

#Region "Object Converter"

#Region "Generate Code"

    Private Sub Button14_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button14.Click
        If RichTextBox1.Text.Length = 0 Then
            Select Case Config.Idioma
                Case Lang.English
                    MsgBox("You must add some objects to convert.", MsgBoxStyle.Critical, "Error")
                Case Lang.Español
                    MsgBox("Debes ingresar objetos para convertir.", MsgBoxStyle.Critical, "Error")
                Case Lang.Portugues
                    MsgBox("Você deve adicionar alguns objetos para converter.", MsgBoxStyle.Critical, "Error")
            End Select
            Exit Sub
        End If
        TextBox58.Clear()
        If ComboBox3.SelectedIndex = ComboBox4.SelectedIndex Then
            If CheckBox9.Checked = False Then
                TextBox75.Text = RichTextBox1.Text
                Exit Sub
            End If
        End If
        Dim pos() As String, Output As String, count As Integer
        Select Case ComboBox4.SelectedIndex
            Case 0
                Output = "CreateObject({0}, {1}, {2}, {3}, {4}, {5}, {6});"
            Case 1
                Output = "CreateDynamicObject({0}, {1}, {2}, {3}, {4}, {5}, {6});"
            Case 2
                Output = "CreateDynamicObject({0}, {1}, {2}, {3}, {4}, {5}, {6});"
            Case 3
                Output = "CreateStreamedObject({0}, {1}, {2}, {3}, {4}, {5}, {6});"
            Case 4
                Output = "CreateStreamObject({0}, {1}, {2}, {3}, {4}, {5}, {6}, 250);"
            Case 5
                Output = "CreateStreamObject({0}, {1}, {2}, {3}, {4}, {5}, {6}, 250);"
            Case 6
                Output = "F_CreateObject({0}, {1}, {2}, {3}, {4}, {5}, {6});"
            Case Else
                Output = "CreateObject({0}, {1}, {2}, {3}, {4}, {5}, {6});"
        End Select
        Select Case ComboBox3.SelectedIndex
            Case 0
                For Each line In RichTextBox1.Lines
                    If line.IndexOf("CreateObject(") >= 0 Then
                        pos = Split(Trim(Mid(Mid(line, line.IndexOf("(") + 2, line.Length - line.IndexOf("(")), 1, line.Length - 3)), ",")
                        TextBox58.Text += String.Format(Output, pos(0), Convert.ToDecimal(pos(1)), Convert.ToDecimal(pos(2)), Convert.ToDecimal(pos(3)), Convert.ToDecimal(pos(4)), Convert.ToDecimal(pos(5)), Convert.ToDecimal(pos(6)))
                        count += 1
                    End If
                Next
            Case 1
                Dim Obj As ObjectS, lock As Boolean = False
                For Each Line In RichTextBox1.Lines
                    If Line.IndexOf("<position>") >= 0 Then
                        pos = Split(Mid(Mid(Line, Line.IndexOf(">") + 2, Line.Length - Line.IndexOf(">")), 1, Line.Length - 25))
                        Obj.X = Convert.ToDecimal(pos(0))
                        Obj.Y = Convert.ToDecimal(pos(1))
                        Obj.Z = Convert.ToDecimal(pos(2))
                    ElseIf Line.IndexOf("<rotation>") >= 0 Then
                        pos = Split(Mid(Mid(Line, Line.IndexOf(">") + 2, Line.Length - Line.IndexOf(">")), 1, Line.Length - 25))
                        Obj.rX = Convert.ToDecimal(pos(0))
                        Obj.rY = Convert.ToDecimal(pos(1))
                        Obj.rZ = Convert.ToDecimal(pos(2))
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
                        TextBox58.Text += String.Format(Output, Obj.Model, Obj.X, Obj.Y, Obj.Z, Obj.rX, Obj.rY, Obj.rZ) & vbNewLine
                        count += 1
                        lock = False
                    End If
                Next
            Case 2
                Dim Obj As ObjectS, lock As Boolean = False
                For Each Line In RichTextBox1.Lines
                    If Line.IndexOf("<object") >= 0 Then
                        Obj.Model = Val(Mid(Line, Line.IndexOf("model=") + 8, Line.IndexOf("""", Line.IndexOf("model=")) - Line.IndexOf("model=") - 2))
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
                        Obj.rZ = Round(Convert.ToDecimal(Mid(Line, Line.IndexOf("rotZ=") + 7, Line.IndexOf(""">", Line.IndexOf("rotZ=")) - Line.IndexOf("rotZ=") - 6), New Globalization.CultureInfo("en-US")), 6)
                        TextBox58.Text += String.Format(New Globalization.CultureInfo("en-US"), Output, Obj.Model, Obj.X, Obj.Y, Obj.Z, Obj.rX, Obj.rY, Obj.rZ)
                        count += 1
                    End If
                Next
            Case 3, 4
                For Each Line In RichTextBox1.Lines
                    If Line.IndexOf("CreateDynamicObject(") >= 0 Then
                        pos = Split(Trim(Mid(Mid(Line, Line.IndexOf("(") + 2, Line.Length - Line.IndexOf("(")), 1, Line.Length - 3)), ",")
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
        End Select
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
                PictureBox1.Image = LoadImageFromURL(String.Format(Config.URL_Skin, TreeView1.SelectedNode.Text))
            End If
            For Each Skin In Skins
                If TreeView1.SelectedNode.Text = Skin.ID Then
                    If tmp = True Then PictureBox1.Image = Skin.Img
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
                PictureBox2.Image = LoadImageFromURL(String.Format(Config.URL_Veh, TreeView2.SelectedNode.Text))
            End If
            For Each vehicle In Vehicles
                If vehicle.ID = TreeView2.SelectedNode.Text Then
                    If tmp = True Then PictureBox2.Image = vehicle.Img
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
                    PictureBox2.Image = LoadImageFromURL(String.Format(Config.URL_Veh, TreeView2.SelectedNode.Text))
                End If
                For Each vehicle In Vehicles
                    If vehicle.Name = TreeView3.SelectedNode.Text Then
                        If tmp = True Then PictureBox2.Image = vehicle.Img
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
                End Select
                Exit Sub
            End If
            Dim count As Integer
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
                    Exit For
                Else
                    count += 1
                    If count = Vehicles.Length Then
                        Select Case Config.Idioma
                            Case Lang.English
                                MsgBox("Vehicle not found", MsgBoxStyle.Critical, "Error")
                            Case Lang.Español
                                MsgBox("Vehiculo no encontrado", MsgBoxStyle.Critical, "Error")
                            Case Else
                                MsgBox("Veículo não encontrado", MsgBoxStyle.Critical, "Error")
                        End Select
                        Exit Sub
                    End If
                End If
            Next
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
                End Select
                Exit Sub
            End If
            Dim found As Boolean = False
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
                    Exit For
                End If
            Next
            If found = False Then
                Select Case Config.Idioma
                    Case Lang.English
                        MsgBox("Vehicle not found", MsgBoxStyle.Critical, "Error")
                    Case Lang.Español
                        MsgBox("Vehiculo no encontrado", MsgBoxStyle.Critical, "Error")
                    Case Else
                        MsgBox("Veículo não encontrado", MsgBoxStyle.Critical, "Error")
                End Select
                Exit Sub
            End If
            TreeView3.Select()
        End If
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
            PictureBox5.Image = LoadImageFromURL(String.Format(Config.URL_Weap, TreeView7.SelectedNode.Text))
        End If
        For Each weapon As Weap In Weapons
            If weapon.Name = TreeView6.SelectedNode.Text Then
                If tmp = True Then PictureBox4.Image = weapon.Img
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
            PictureBox5.Image = LoadImageFromURL(String.Format(Config.URL_Map, TreeView7.SelectedNode.Text))
        End If
        For Each map In Maps
            If map.ID = TreeView7.SelectedNode.Text Then
                If tmp = True Then PictureBox5.Image = map.Img
                TextBox72.Text = map.Name
                TextBox73.Text = map.ID
                Exit For
            End If
        Next
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
        Else
            GroupBox11.Enabled = True
            GroupBox13.Enabled = True
            GroupBox14.Enabled = True
            GroupBox17.Enabled = True
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

#End Region

#Region "Buttons"

    Private Sub Button10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button10.Click
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
        TextBox63.Text = Config.Clip
    End Sub

    Private Sub Button11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button11.Click
        If RadioButton14.Checked = True Then
            Config.Idioma = Lang.English
        ElseIf RadioButton15.Checked = True Then
            Config.Idioma = Lang.Español
        Else
            Config.Idioma = Lang.Portugues
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
        End If
        Config.Clip = TextBox63.Text
    End Sub

    Private Sub Button13_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button13.Click
        CheckBox7.Checked = True
        RadioButton14.Checked = True
        TextBox63.Text = "{0}, {0}, {0}, {0}"
        TextBox50.Text = "http://www.gamerxserver.com/images/Skins/Thumbnails/{0}.jpg"
        TextBox51.Text = ""
        TextBox68.Text = ""
        TextBox75.Text = ""
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
        TextBox63.Text = Config.Clip
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
        Creds.Top = Me.Top + 100
        Creds.Left = Me.Left + 200
    End Sub

#End Region

#End Region

End Class
