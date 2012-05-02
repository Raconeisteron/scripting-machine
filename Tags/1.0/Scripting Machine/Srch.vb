Public Class Srch

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Diagnostics.Process.Start("http://wiki.sa-mp.com/wiki/Special:Search?search=" & TextBox1.Text & "&go=Go")
        Me.Close()
    End Sub

    Private Sub TextBox1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox1.KeyPress
        If Asc(e.KeyChar) = 13 Then
            Diagnostics.Process.Start("http://wiki.sa-mp.com/wiki/Special:Search?search=" & TextBox1.Text & "&go=Go")
            Me.Close()
        End If
    End Sub

End Class