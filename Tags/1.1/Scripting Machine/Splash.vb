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

Public NotInheritable Class Splash

    Private Sub Splash_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If My.Application.Info.Title <> "" Then
            ApplicationTitle.Text = My.Application.Info.Title
        Else
            ApplicationTitle.Text = System.IO.Path.GetFileNameWithoutExtension(My.Application.Info.AssemblyName)
        End If
        Version.Text = String.Format("Version: {0}.{1}.{2}", My.Application.Info.Version.Major.ToString, My.Application.Info.Version.Minor, My.Application.Info.Version.MajorRevision)
        Copyright.Text = My.Application.Info.Copyright
        Control.CheckForIllegalCrossThreadCalls = False
        Control.CheckForIllegalCrossThreadCalls = False
    End Sub

End Class
