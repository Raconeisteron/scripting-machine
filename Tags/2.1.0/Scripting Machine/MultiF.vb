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

Public Class MultiF

#Region "Load"

    Private Sub Creds_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If tSender = MsgT.Credits Then
            Label1.Text = "Credits:" & vbNewLine & _
                "   **Programing:" & vbNewLine & _
                "       ***The_Chaoz" & vbNewLine & _
                "   **Elements:" & vbNewLine & _
                "       ***Tabcontrol Class(main): vijayaprasen" & vbNewLine & _
                "       ***Scintilla: Neil Hodgson" & vbNewLine & _
                "       ***ScintillaNET: Garrett Serack" & vbNewLine & _
                "   **Resources:" & vbNewLine & _
                "       ***Vehicle Images: Peter" & vbNewLine & _
                "       ***Weapon Images: thegtaplace.com" & vbNewLine & _
                "       ***Pawn compiler: Spookie" & vbNewLine & _
                "   **Codes:" & vbNewLine & _
                "       ***Area's code convertion: Zamaroht" & vbNewLine & _
                "       ***Color conversions: Guillaume Leparmentier" & vbNewLine & _
                "   **Beta Testing:" & vbNewLine & _
                "       ***NightWar" & vbNewLine & _
                "       ***Jovanny" & vbNewLine & _
                "   **SA:MP team, without them this program won't exist." & vbNewLine & _
                "   **Every one that contribute to the SA:MP wikipedia."
        Else
            Label1.Text = "Tutorial:" & vbNewLine & tText
        End If
    End Sub

#End Region

#Region "Closing"

    Private Sub Creds_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LostFocus
        Me.Close()
    End Sub

    Private Sub Creds_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Click
        Me.Close()
    End Sub

    Private Sub Label1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Label1.Click
        Me.Close()
    End Sub

    Private Sub Label2_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.Close()
    End Sub

#End Region

End Class