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

Public Class Creds

#Region "Load"

    Private Sub Creds_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Label2.Text = _
        "   **SA:MP team, without them" & vbNewLine & "       this program will no exist, " & vbNewLine & "       and won't have any sence." & vbNewLine & _
        "   **Every one that contribute" & vbNewLine & "       to the SA:MP wikipedia." & vbNewLine & _
        "   **Peter, vehicle images." & vbNewLine & _
        "   **thegtaplace.com, weapon images." & vbNewLine & _
        "   **Zamaroht, area's code convertion." & vbNewLine & _
        "   **Guillaume Leparmentier, color conversions." & vbNewLine & _
        "   **Meta, Deutsch translation." & vbNewLine & _
        "   **NightWar, very productive conversations" & vbNewLine & _
        "       and beta testing." & vbNewLine & _
        "   **Edugta, beta testing." & vbNewLine & _
        "   **Adoniiz, beta testing." & vbNewLine & _
        "   **The_Chaoz, programing."
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

    Private Sub Label2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Label2.Click
        Me.Close()
    End Sub

#End Region

End Class