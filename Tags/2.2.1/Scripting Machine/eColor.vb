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

Imports System.Text.RegularExpressions

Public Class eColor

#Region "Arrays"

    Dim tmp As String

#End Region

#Region "Texts Restrictions"

    Private Sub TextBox1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox1.KeyPress
        If Not IsNumeric(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) AndAlso Asc(e.KeyChar) <> 8 Then e.Handled = True
    End Sub

    Private Sub TextBox2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox2.KeyPress
        If Not IsNumeric(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) AndAlso Asc(e.KeyChar) <> 8 Then e.Handled = True
    End Sub

    Private Sub TextBox3_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox3.KeyPress
        If Not IsNumeric(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) AndAlso Asc(e.KeyChar) <> 8 Then e.Handled = True
    End Sub

    Private Sub TextBox4_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox4.KeyPress
        If Not IsNumeric(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) AndAlso Asc(e.KeyChar) <> 8 Then e.Handled = True
    End Sub

#End Region

#Region "Generate Color"

#Region "Tracks"

    Private Sub TrackBar1_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBar1.Scroll
        If gSender = CC.Dialog Then
            tmp = cColor(TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel1.BackColor)
        Else
            tmp = cColor(TrackBar4.Value, TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel1.BackColor)
        End If
        TextBox1.Text = TrackBar1.Value
    End Sub

    Private Sub TrackBar2_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBar2.Scroll
        If gSender = CC.Dialog Then
            tmp = cColor(TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel1.BackColor)
        Else
            tmp = cColor(TrackBar4.Value, TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel1.BackColor)
        End If
        TextBox2.Text = TrackBar2.Value
    End Sub

    Private Sub TrackBar3_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBar3.Scroll
        If gSender = CC.Dialog Then
            tmp = cColor(TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel1.BackColor)
        Else
            tmp = cColor(TrackBar4.Value, TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel1.BackColor)
        End If
        TextBox3.Text = TrackBar3.Value
    End Sub

    Private Sub TrackBar4_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBar4.Scroll
        If gSender = CC.Dialog Then
            tmp = cColor(TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel1.BackColor)
        Else
            tmp = cColor(TrackBar4.Value, TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel1.BackColor)
        End If
        TextBox4.Text = TrackBar4.Value
    End Sub

    Private Sub TrackBar1_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TrackBar1.ValueChanged
        TextBox1.Text = TrackBar1.Value
    End Sub

    Private Sub TrackBar2_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TrackBar2.ValueChanged
        TextBox2.Text = TrackBar2.Value
    End Sub

    Private Sub TrackBar3_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TrackBar3.ValueChanged
        TextBox3.Text = TrackBar3.Value
    End Sub

    Private Sub TrackBar4_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TrackBar4.ValueChanged
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
            If gSender = CC.Dialog Then
                tmp = cColor(TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel1.BackColor)
            Else
                tmp = cColor(TrackBar4.Value, TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel1.BackColor)
            End If
        End If
    End Sub

    Private Sub TextBox2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox2.TextChanged
        TextBox2.Text = Regex.Replace(TextBox2.Text, BadChars, "")
        If Val(TextBox2.Text) <> TrackBar1.Value Then
            If Val(TextBox2.Text) > 255 Then TextBox2.Text = 255
            If Val(TextBox2.Text) < 0 Then TextBox2.Text = 0
            TrackBar2.Value = Val(TextBox2.Text)
            If gSender = CC.Dialog Then
                tmp = cColor(TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel1.BackColor)
            Else
                tmp = cColor(TrackBar4.Value, TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel1.BackColor)
            End If
        End If
    End Sub

    Private Sub TextBox3_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox3.TextChanged
        TextBox3.Text = Regex.Replace(TextBox3.Text, BadChars, "")
        If Val(TextBox3.Text) <> TrackBar1.Value Then
            If Val(TextBox3.Text) > 255 Then TextBox3.Text = 255
            If Val(TextBox3.Text) < 0 Then TextBox3.Text = 0
            TrackBar3.Value = Val(TextBox3.Text)
            If gSender = CC.Dialog Then
                tmp = cColor(TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel1.BackColor)
            Else
                tmp = cColor(TrackBar4.Value, TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel1.BackColor)
            End If
        End If
    End Sub

    Private Sub TextBox4_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox4.TextChanged
        TextBox4.Text = Regex.Replace(TextBox4.Text, BadChars, "")
        If Val(TextBox4.Text) <> TrackBar4.Value Then
            If Val(TextBox4.Text) > 255 Then TextBox4.Text = 255
            If Val(TextBox4.Text) < 0 Then TextBox4.Text = 0
            TrackBar4.Value = Val(TextBox4.Text)
            If gSender = CC.Dialog Then
                tmp = cColor(TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel1.BackColor)
            Else
                tmp = cColor(TrackBar4.Value, TrackBar1.Value, TrackBar2.Value, TrackBar3.Value, Panel1.BackColor)
            End If
        End If
    End Sub

#End Region

#End Region

#Region "Accept/Cancel"

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Select Case gSender
            Case CC.Dialog
                Tools.TextBox3.SelectedText = tmp
                Tools.Panel1.BackColor = Panel1.BackColor
            Case CC.Area
                Tools.Panel6.BackColor = Panel1.BackColor
                Settings.C_Area.Hex = Panel1.BackColor
                Settings.C_Area.Name = tmp
            Case CC.Msg
                Tools.Panel4.BackColor = Panel1.BackColor
                Settings.C_Msg.Hex = Panel1.BackColor
                Settings.C_Msg.Name = tmp
            Case CC.Help
                Tools.Panel3.BackColor = Panel1.BackColor
                Settings.C_Help.Hex = Panel1.BackColor
                Settings.C_Help.Name = tmp
            Case CC.H_NF
                Options.Panel1.BackColor = Panel1.BackColor
            Case CC.H_NB
                Options.Panel2.BackColor = Panel1.BackColor
            Case CC.H_SF
                If TrackBar4.Value = 0 Then TrackBar4.Value = 1
                Options.Panel4.BackColor = Panel1.BackColor
            Case CC.H_SB
                If TrackBar4.Value = 0 Then TrackBar4.Value = 1
                Options.Panel3.BackColor = Panel1.BackColor
            Case CC.H_S2F
                If TrackBar4.Value = 0 Then TrackBar4.Value = 1
                Options.Panel6.BackColor = Panel1.BackColor
            Case CC.H_S2B
                If TrackBar4.Value = 0 Then TrackBar4.Value = 1
                Options.Panel5.BackColor = Panel1.BackColor
            Case CC.H_OF
                If TrackBar4.Value = 0 Then TrackBar4.Value = 1
                Options.Panel8.BackColor = Panel1.BackColor
            Case CC.H_OB
                If TrackBar4.Value = 0 Then TrackBar4.Value = 1
                Options.Panel7.BackColor = Panel1.BackColor
            Case CC.H_CHF
                If TrackBar4.Value = 0 Then TrackBar4.Value = 1
                Options.Panel10.BackColor = Panel1.BackColor
            Case CC.H_CHB
                If TrackBar4.Value = 0 Then TrackBar4.Value = 1
                Options.Panel9.BackColor = Panel1.BackColor
            Case CC.H_CLF
                If TrackBar4.Value = 0 Then TrackBar4.Value = 1
                Options.Panel12.BackColor = Panel1.BackColor
            Case CC.H_CLB
                If TrackBar4.Value = 0 Then TrackBar4.Value = 1
                Options.Panel11.BackColor = Panel1.BackColor
            Case CC.H_CMB
                If TrackBar4.Value = 0 Then TrackBar4.Value = 1
                Options.Panel14.BackColor = Panel1.BackColor
            Case CC.H_CMF
                If TrackBar4.Value = 0 Then TrackBar4.Value = 1
                Options.Panel13.BackColor = Panel1.BackColor
            Case CC.G_Open
                Tools.Panel10.BackColor = Panel1.BackColor
                Settings.G_Open_Color.Hex = Panel1.BackColor
                Settings.G_Open_Color.Name = tmp
            Case CC.G_Close
                Tools.Panel9.BackColor = Panel1.BackColor
                Settings.G_Close_Color.Hex = Panel1.BackColor
                Settings.G_Close_Color.Name = tmp
            Case CC.Back
                If TrackBar4.Value = 0 Then TrackBar4.Value = 1
                Options.Panel15.BackColor = Panel1.BackColor
        End Select
        Me.Hide()
        Me.Owner.Refresh()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.Hide()
        Me.Owner.Refresh()
    End Sub

#End Region

#Region "defined colors"

    Private Sub eColor_VisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.VisibleChanged
        If gSender = CC.Dialog Then
            TrackBar4.Enabled = False
            TextBox4.Enabled = False
            Label4.Enabled = False
            With Instances(Main.TabControl1.SelectedIndex).ACLists
                If .eColors.Count Then
                    ComboBox1.Enabled = True
                    ComboBox1.Items.Clear()
                    For Each col As PawnColor In .eColors
                        ComboBox1.Items.Add(col.Name)
                    Next
                Else
                    ComboBox1.Enabled = False
                End If
            End With
        Else
            TrackBar4.Enabled = True
            TextBox4.Enabled = True
            Label4.Enabled = True
            With Instances(Main.TabControl1.SelectedIndex).ACLists
                If .Colors.Count Then
                    ComboBox1.Enabled = True
                    ComboBox1.Items.Clear()
                    For Each col As PawnColor In .Colors
                        ComboBox1.Items.Add(col.Name)
                    Next
                Else
                    ComboBox1.Enabled = False
                End If
            End With
        End If
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        If gSender = CC.Dialog Then
            With Instances(Main.TabControl1.SelectedIndex).ACLists
                For Each col As PawnColor In .eColors
                    If col.Name = ComboBox1.Text Then
                        TrackBar1.Value = col.Hex.R
                        TrackBar2.Value = col.Hex.G
                        TrackBar3.Value = col.Hex.B
                        TrackBar4.Value = 255
                        Panel1.BackColor = col.Hex
                        Exit For
                    End If
                Next
            End With
        Else
            With Instances(Main.TabControl1.SelectedIndex).ACLists
                For Each col As PawnColor In .Colors
                    If col.Name = ComboBox1.Text Then
                        TrackBar1.Value = col.Hex.R
                        TrackBar2.Value = col.Hex.G
                        TrackBar3.Value = col.Hex.B
                        TrackBar4.Value = col.Hex.A
                        Panel1.BackColor = col.Hex
                        Exit For
                    End If
                Next
            End With
        End If
    End Sub

#End Region

End Class