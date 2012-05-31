Public NotInheritable Class Splash

    Private Sub Splash_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If My.Application.Info.Title <> "" Then
            ApplicationTitle.Text = My.Application.Info.Title
        Else
            ApplicationTitle.Text = System.IO.Path.GetFileNameWithoutExtension(My.Application.Info.AssemblyName)
        End If
        Version.Text = String.Format("Version: {0}", My.Application.Info.Version.ToString)
        Copyright.Text = My.Application.Info.Copyright
        Label1.CheckForIllegalCrossThreadCalls = False
        ProgressBar1.CheckForIllegalCrossThreadCalls = False
    End Sub

End Class
