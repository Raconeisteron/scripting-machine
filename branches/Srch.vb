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

Public Class Srch

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If Not TextBox1.Text.Length Then
            Select Case Settings.Language
                Case Languages.English
                    MsgBox("You must enter a text to search for.", MsgBoxStyle.Critical, "Error")
                Case Languages.Español
                    MsgBox("Debes ingresar un texto para buscar.", MsgBoxStyle.Critical, "Error")
                Case Languages.Portuguêse
                    MsgBox("Você deve digitar um texto a ser pesquisado.", MsgBoxStyle.Critical, "Error")
                Case Else
                    MsgBox("Du musst einen Text zum Suchen angeben.", MsgBoxStyle.Critical, "Fehler")
            End Select
            Exit Sub
        End If
        Process.Start("http://wiki.sa-mp.com/wiki/Special:Search?search=" & TextBox1.Text & "&go=Go")
        Me.Hide()
        Me.Owner.Refresh()
    End Sub

    Private Sub TextBox1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox1.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            Process.Start("http://wiki.sa-mp.com/wiki/Special:Search?search=" & TextBox1.Text & "&go=Go")
            Me.Hide()
            Me.Owner.Refresh()
        End If
    End Sub

End Class