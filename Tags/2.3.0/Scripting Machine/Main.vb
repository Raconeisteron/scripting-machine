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

Public Class Main

#Region "Me"

    Private Sub Main_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Me.Visible = False
            AddHandler AppDomain.CurrentDomain.UnhandledException, AddressOf OnUnhandledException
            AddHandler My.Application.StartupNextInstance, AddressOf OnStartupNextInstance
            Tools.Owner = Me
            MultiF.Owner = Me
            Options.Owner = Me
            Srch.Owner = Me
            eColor.Owner = Me
            RegisterHotKey(Me.Handle, 9303, MOD_CONTROL, Keys.Space)
            SFD.Filter = "Script files|*.pwn;*.inc|Pawn files (.pwn)|*.pwn|Include files (.inc)|*.inc|All files| *.*"
            SFD.DefaultExt = "pwn"
            OFD.Filter = "Script files|*.pwn;*.inc|Pawn files (.pwn)|*.pwn|Include files (.inc)|*.inc|All files| *.*"
            LoadResources()
            Dim Reader As StreamReader
            If Directory.Exists(My.Application.Info.DirectoryPath & "\TMP") Then
                If Directory.GetFiles(My.Application.Info.DirectoryPath & "\TMP").Length > 0 Then
                    Dim result As MsgBoxResult
                    Select Case Settings.Language
                        Case Languages.English
                            result = MsgBox("Unsaved files from the last time the program was used were detected. Do you want to recover them?", MsgBoxStyle.YesNo, "Alert")
                        Case Languages.Español
                            result = MsgBox("Archivos no guardados desde la ultima sesion del programa fueron encontrados. ¿Quieres recuperarlos?", MsgBoxStyle.YesNo, "Alerta")
                        Case Languages.Portuguêse
                            result = MsgBox("Arquivos não salvos da última vez que o programa foi utilizado foram detectados. Você deseja recuperá-los?", MsgBoxStyle.YesNo, "Alerta")
                        Case Else
                            result = MsgBox("Nicht gespeicherte Dateien aus der letzten Zeit wurde das Programm eingesetzt wurden nachgewiesen. Wollen Sie sie wieder?", MsgBoxStyle.YesNo, "Alarm")
                    End Select
                    If result = vbYes Then
                        Dim name As String
                        For Each File In Directory.GetFiles(My.Application.Info.DirectoryPath & "\TMP")
                            name = Mid(File, File.LastIndexOf("\") + 2, File.LastIndexOf(".") - File.LastIndexOf("\") - 1)
                            Instances.Add(New Instance(name, Instances.Count))
                            TabControl1.SelectedTab = Instances(GetInstanceByName(name, Instances.Count - 1)).TabHandle
                            TabControl1.Select()
                            Reader = New StreamReader(File, System.Text.Encoding.GetEncoding(28591))
                            Instances(GetInstanceByName(name, Instances.Count - 1)).SyntaxHandle.Text = Reader.ReadToEnd()
                            Reader.Close()
                            With Instances(GetInstanceByName(name, Instances.Count - 1))
                                .Path = .SyntaxHandle.Lines(0).Text
                                .Saved = False
                                .SyntaxHandle.Lines(0).Text = ""
                            End With
                        Next
                        Directory.Delete(My.Application.Info.DirectoryPath & "\TMP", True)
                        If My.Application.CommandLineArgs Is Nothing Then Exit Sub
                    Else
                        Directory.Delete(My.Application.Info.DirectoryPath & "\TMP", True)
                    End If
                Else
                    Directory.Delete(My.Application.Info.DirectoryPath & "\TMP", True)
                End If
            End If
            Try
                Dim name As String
                name = Mid(My.Application.CommandLineArgs(0), My.Application.CommandLineArgs(0).LastIndexOf("\") + 2, My.Application.CommandLineArgs(0).LastIndexOf(".") - My.Application.CommandLineArgs(0).LastIndexOf("\") - 1)
                Instances.Add(New Instance(name, Instances.Count))
                Reader = New StreamReader(My.Application.CommandLineArgs(0), System.Text.Encoding.GetEncoding(28591))
                With Instances(GetInstanceByName(name, Instances.Count - 1))
                    .SyntaxHandle.Text = Reader.ReadToEnd()
                    .Path = My.Application.CommandLineArgs(0)
                    .Saved = True
                    .Ext = Mid(My.Application.CommandLineArgs(0), My.Application.CommandLineArgs(0).LastIndexOf(".") + 2, My.Application.CommandLineArgs(0).Length - My.Application.CommandLineArgs(0).LastIndexOf(".") - 1)
                End With
                Reader.Close()
            Catch ex As Exception
                If File.Exists(My.Application.Info.DirectoryPath & "\Scripts\new.pwn") Then
                    Instances.Add(New Instance("new script", Instances.Count))
                    Reader = New StreamReader(My.Application.Info.DirectoryPath & "\Scripts\new.pwn", System.Text.Encoding.GetEncoding(28591))
                    With Instances(GetInstanceByName("new script", Instances.Count - 1))
                        .SyntaxHandle.Text = Reader.ReadToEnd()
                        .Path = My.Application.Info.DirectoryPath & "\Scripts\new.pwn"
                    End With
                    Reader.Close()
                End If
            End Try
            Dim cMenu As New ContextMenu, i(2) As MenuItem
            i(0) = New MenuItem("Copy selected row")
            i(1) = New MenuItem("Copy all")
            i(2) = New MenuItem("Clear")
            For p = 0 To 2
                AddHandler i(p).Click, AddressOf ContextMenuHandler
                cMenu.MenuItems.Add(i(p))
            Next
            ListView1.ContextMenu = cMenu
            Me.Visible = True
        Catch ex As Exception
            Splash.Label1.Invoke(sLabel, New Object() {"Error", Splash})
        End Try
    End Sub

    Private Sub Main_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        RemoveHandler AppDomain.CurrentDomain.UnhandledException, AddressOf OnUnhandledException
        RemoveHandler My.Application.StartupNextInstance, AddressOf OnStartupNextInstance
        UnregisterHotKey(Me.Handle, 9303)
        SaveConfig()
        For Each Inst In Instances
            If Inst.Saved = False Then
                Dim Res As MsgBoxResult
                Select Case Settings.Language
                    Case Languages.English
                        Res = MsgBox("Do you want to save changes from """ & Inst.Name & """?", MsgBoxStyle.YesNoCancel, "Closing")
                    Case Languages.Español
                        Res = MsgBox("¿Quieres guardar los cambios de """ & Inst.Name & """?", MsgBoxStyle.YesNoCancel, "Closing")
                    Case Languages.Portuguêse
                        Res = MsgBox("Você deseja salvar as alterações de """ & Inst.Name & """?", MsgBoxStyle.YesNoCancel, "Closing")
                    Case Else
                        Res = MsgBox("Wollen Sie Änderungen von """ & Inst.Name & """ zu retten?""", MsgBoxStyle.YesNoCancel, "Closing")
                End Select
                Select Case Res
                    Case MsgBoxResult.No
                        Continue For
                    Case MsgBoxResult.Yes
                        With Inst
                            If Not .Path Is Nothing AndAlso .Path.Length > 0 Then
                                Dim Writer As New StreamWriter(.Path, False, System.Text.Encoding.GetEncoding(28591))
                                Writer.Write(.SyntaxHandle.Text)
                                Writer.Close()
                                .Saved = True
                            Else
                                SFD.InitialDirectory = Settings.DefaultPath
                                SFD.ShowDialog()
                                If Not SFD.FileName Is Nothing AndAlso SFD.FileName.Length > 0 Then
                                    Dim Writer As New StreamWriter(SFD.FileName, False, System.Text.Encoding.GetEncoding(28591))
                                    Writer.Write(.SyntaxHandle.Text)
                                    Writer.Close()
                                    .Saved = True
                                    .Path = SFD.FileName
                                    .Name = Mid(SFD.FileName, SFD.FileName.LastIndexOf("\") + 2, SFD.FileName.LastIndexOf(".") - SFD.FileName.LastIndexOf("\") - 1)
                                End If
                            End If
                        End With
                    Case MsgBoxResult.Cancel
                        e.Cancel = True
                End Select
            End If
        Next
    End Sub

    Private Sub Main_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        On Error Resume Next
        Dim Header As Boolean() = New Boolean() {True, True, True}
        For Each item As ListViewItem In Instances(TabControl1.SelectedIndex).Errors
            ListView1.Items.Add(item)
            If Not Header(0) AndAlso ListView1.Columns(1).Text.Length <= item.SubItems(1).Text.Length Then Header(0) = False
            If Not Header(1) AndAlso ListView1.Columns(2).Text.Length <= item.SubItems(2).Text.Length Then Header(1) = False
            If Not Header(2) AndAlso ListView1.Columns(3).Text.Length <= item.SubItems(3).Text.Length Then Header(2) = False
        Next
        With ListView1
            .Columns(0).Width = 25
            .Columns(1).Width = If(Header(0), -2, -1)
            .Columns(2).Width = If(Header(1), -2, -1)
            .Columns(3).Width = If(Header(2), -2, -1)
            .Columns(4).Width = -2
        End With
    End Sub

#End Region

#Region "App"

    Private Sub OnUnhandledException(ByVal sender As Object, ByVal e As UnhandledExceptionEventArgs)
        If Not Directory.Exists(My.Application.Info.DirectoryPath & "\Temp") Then Directory.CreateDirectory(My.Application.Info.DirectoryPath & "\Temp")
        Dim Writer As StreamWriter, count As Integer
        For Each item In Instances
            If item.Saved = False Then
                Writer = New StreamWriter(My.Application.Info.DirectoryPath & "\Temp\" & item.Name & ".pwn", False, System.Text.Encoding.GetEncoding(28591))
                Writer.Write(item.Path & vbNewLine & item.SyntaxHandle.Text)
                Writer.Close()
                count += 1
            End If
        Next
        If count = 0 Then Directory.Delete(My.Application.Info.ProductName & "\Temp")
    End Sub

    Private Sub OnStartupNextInstance(ByVal sender As Object, ByVal e As Microsoft.VisualBasic.ApplicationServices.StartupNextInstanceEventArgs)
        For Each a As String In e.CommandLine
            Try
                Dim name As String, index As Integer
                name = Mid(a, a.LastIndexOf("\") + 2, a.LastIndexOf(".") - a.LastIndexOf("\") - 1)
                Instances.Add(New Instance(name, Instances.Count))
                index = GetInstanceByName(name, Instances.Count - 1)
                Dim Reader = New StreamReader(a, System.Text.Encoding.GetEncoding(28591))
                With Instances(index)
                    .SyntaxHandle.Text = Reader.ReadToEnd()
                    .Path = a
                    .Saved = True
                    .Ext = Mid(a, a.LastIndexOf(".") + 2, a.Length - a.LastIndexOf(".") - 1)
                End With
                TabControl1.SelectedIndex = index
                Reader.Close()
                Me.WindowState = FormWindowState.Maximized
                Me.Focus()
            Catch ex As Exception

            End Try
        Next
    End Sub

    Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
        If m.Msg = WM_HOTKEY Then
            If m.WParam.ToInt32 = 9303 AndAlso Not Instances(TabControl1.SelectedIndex).InfoText.Visible Then
                Dim func As PawnFunction = GetFunctionByName(Instances(TabControl1.SelectedIndex).ACLists.Functions, Instances(TabControl1.SelectedIndex).CurrentFunction)
                If Instances(TabControl1.SelectedIndex).ACLists.Functions.Contains(func) Then
                    With Instances(TabControl1.SelectedIndex)
                        .InfoText.Clear()
                        Dim istart As Integer, iend As Integer
                        For Each param As String In .ACLists.Functions(.ACLists.Functions.IndexOf(func)).Params
                            If Not .ACLists.Functions(.ACLists.Functions.IndexOf(func)).Params(UBound(.ACLists.Functions(.ACLists.Functions.IndexOf(func)).Params)) = param Then
                                If Array.IndexOf(.ACLists.Functions(.ACLists.Functions.IndexOf(func)).Params, param) = .CurrentParamIndex Then
                                    istart = .InfoText.Text.Length
                                    iend = istart + Len(param + ", ")
                                End If
                                .InfoText.Text += param & ", "
                            Else
                                If Array.IndexOf(.ACLists.Functions(.ACLists.Functions.IndexOf(func)).Params, param) = .CurrentParamIndex Then
                                    istart = .InfoText.Text.Length
                                    iend = istart + Len(param)
                                End If
                                .InfoText.Text += param
                            End If
                        Next
                        .InfoText.Width = .InfoText.Text.Length * 6.5
                        .InfoText.SelectionStart = istart
                        .InfoText.SelectionLength = iend - istart
                        .InfoText.SelectionFont = New Font(.InfoText.Font.FontFamily, 10)
                        .InfoText.SelectionColor = Color.Blue
                        .InfoText.Location = New Point(Instances(TabControl1.SelectedIndex).SyntaxHandle.PointXFromPosition(Instances(TabControl1.SelectedIndex).SyntaxHandle.CurrentPos), Instances(TabControl1.SelectedIndex).SyntaxHandle.PointYFromPosition(Instances(TabControl1.SelectedIndex).SyntaxHandle.CurrentPos) + 20)
                        .ShowingInfoText = True
                    End With
                End If
            End If
        End If
        MyBase.WndProc(m)
    End Sub

#End Region

#Region "Menu"

#Region "Menu"

#Region "File"

#Region "New"

    Private Sub GameModeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GamemodeToolStripMenuItem.Click
        Dim count As Integer, name As String
        For Each item In Instances
            If item.Name.StartsWith("new script") Then
                count += 1
            End If
        Next
        If count Then
            name = "new script" & count
        Else
            name = "new script"
        End If
        Instances.Add(New Instance(name, Instances.Count))
        If File.Exists(My.Application.Info.DirectoryPath & "\Scripts\gamemode.pwn") Then
            Dim Reader As New StreamReader(My.Application.Info.DirectoryPath & "\Scripts\gamemode.pwn", System.Text.Encoding.GetEncoding(28591))
            With Instances(GetInstanceByName(name, Instances.Count - 1))
                .SyntaxHandle.Text = Reader.ReadToEnd()
                .Path = My.Application.Info.DirectoryPath & "\Scripts\gamemode.pwn"
                TabControl1.SelectedTab = .TabHandle
            End With
            Reader.Close()
        Else
            Select Case Settings.Language
                Case Languages.English
                    MsgBox("File could not be found.", MsgBoxStyle.Critical, "Error")
                Case Languages.Español
                    MsgBox("Archivo no encontrado.", MsgBoxStyle.Critical, "Error")
                Case Languages.Portuguêse
                    MsgBox("Arquivo não pôde ser encontrado.", MsgBoxStyle.Critical, "Error")
                Case Else
                    MsgBox("Datei konnte nicht gefunden werden.", MsgBoxStyle.Critical, "Error")
            End Select
        End If
    End Sub

    Private Sub FilterscriptToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FilterscriptToolStripMenuItem.Click
        Dim count As Integer, name As String
        For Each item In Instances
            If item.Name.IndexOf("new script") > -1 Then
                count += 1
            End If
        Next
        If count = 0 Then
            name = "new script"
        Else
            name = "new script" & count
        End If
        Instances.Add(New Instance(name, Instances.Count))
        If File.Exists(My.Application.Info.DirectoryPath & "\Scripts\filterscript.pwn") Then
            Dim Reader As New StreamReader(My.Application.Info.DirectoryPath & "\Scripts\filterscript.pwn", System.Text.Encoding.GetEncoding(28591))
            With Instances(GetInstanceByName(name, Instances.Count - 1))
                .SyntaxHandle.Text = Reader.ReadToEnd()
                .Path = My.Application.Info.DirectoryPath & "\Scripts\filterscript.pwn"
                TabControl1.SelectedTab = .TabHandle
            End With
            Reader.Close()
        Else
            Select Case Settings.Language
                Case Languages.English
                    MsgBox("File could not be found.", MsgBoxStyle.Critical, "Error")
                Case Languages.Español
                    MsgBox("Archivo no encontrado.", MsgBoxStyle.Critical, "Error")
                Case Languages.Portuguêse
                    MsgBox("Arquivo não pôde ser encontrado.", MsgBoxStyle.Critical, "Error")
                Case Else
                    MsgBox("Datei konnte nicht gefunden werden.", MsgBoxStyle.Critical, "Error")
            End Select
        End If
    End Sub

    Private Sub NewScriptToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewScriptToolStripMenuItem.Click
        Dim count As Integer, name As String
        For Each item In Instances
            If item.Name.IndexOf("new script") > -1 Then
                count += 1
            End If
        Next

        If count = 0 Then
            name = "new script"
        Else
            name = "new script" & count
        End If
        Instances.Add(New Instance(name, Instances.Count))
        If File.Exists(My.Application.Info.DirectoryPath & "\Scripts\new.pwn") Then
            Dim Reader As New StreamReader(My.Application.Info.DirectoryPath & "\Scripts\new.pwn", System.Text.Encoding.GetEncoding(28591))
            With Instances(GetInstanceByName(name, Instances.Count - 1))
                .SyntaxHandle.Text = Reader.ReadToEnd()
                .Path = My.Application.Info.DirectoryPath & "\Scripts\new.pwn"
                TabControl1.SelectedTab = .TabHandle
            End With
            Reader.Close()
        Else
            Select Case Settings.Language
                Case Languages.English
                    MsgBox("File could not be found.", MsgBoxStyle.Critical, "Error")
                Case Languages.Español
                    MsgBox("Archivo no encontrado.", MsgBoxStyle.Critical, "Error")
                Case Languages.Portuguêse
                    MsgBox("Arquivo não pôde ser encontrado.", MsgBoxStyle.Critical, "Error")
                Case Else
                    MsgBox("Datei konnte nicht gefunden werden.", MsgBoxStyle.Critical, "Error")
            End Select
        End If
    End Sub

    Private Sub EmptyDocumentToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EmptyDocumentToolStripMenuItem.Click
        Dim count As Integer, name As String
        For Each item In Instances
            If item.Name.IndexOf("new script") > -1 Then
                count += 1
            End If
        Next

        If count = 0 Then
            name = "new script"
        Else
            name = "new script" & count
        End If
        Instances.Add(New Instance(name, Instances.Count))
        With Instances(GetInstanceByName(name, Instances.Count - 1))
            TabControl1.SelectedTab = .TabHandle
            .Path = "{Empty Doc}"
        End With
    End Sub

#End Region

    Private Sub OpenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenToolStripMenuItem.Click
        OFD.InitialDirectory = Settings.DefaultPath
        If OFD.ShowDialog() = Windows.Forms.DialogResult.Cancel Then Exit Sub
        If Not OFD.FileName Is Nothing AndAlso OFD.FileName.Length > 0 Then
            If File.Exists(OFD.FileName) Then
                Dim name As String
                name = Mid(OFD.FileName, OFD.FileName.LastIndexOf("\") + 2, OFD.FileName.LastIndexOf(".") - OFD.FileName.LastIndexOf("\") - 1)
                Instances.Add(New Instance(name, Instances.Count))
                With Instances(GetInstanceByName(name, Instances.Count - 1))
                    Dim Reader As StreamReader
                    If OFD.FileName.EndsWith(".pwn") OrElse OFD.FileName.EndsWith(".inc") Then
                        Reader = New StreamReader(OFD.FileName, System.Text.Encoding.GetEncoding(28591), True)
                    Else
                        Reader = New StreamReader(OFD.FileName, System.Text.Encoding.UTF8, True)
                    End If
                    .SyntaxHandle.Text = Reader.ReadToEnd()
                    Reader.Close()
                    .Path = OFD.FileName
                    TabControl1.SelectedTab = .TabHandle
                End With
            End If
        End If
    End Sub

    Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        With Instances(TabControl1.SelectedIndex)
            If Not .Path Is Nothing AndAlso .Path.Length > 0 Then
                Dim Writer As New StreamWriter(.Path, False, System.Text.Encoding.GetEncoding(28591))
                Writer.Write(.SyntaxHandle.Text)
                Writer.Close()
                .Saved = True
            Else
                SFD.InitialDirectory = Settings.DefaultPath
                If SFD.ShowDialog() = Windows.Forms.DialogResult.Cancel Then Exit Sub
                If Not SFD.FileName Is Nothing AndAlso SFD.FileName.Length > 0 Then
                    Dim Writer As New StreamWriter(SFD.FileName, False, System.Text.Encoding.GetEncoding(28591))
                    Writer.Write(.SyntaxHandle.Text)
                    Writer.Close()
                    .Saved = True
                    .Path = SFD.FileName
                    .Name = Mid(SFD.FileName, SFD.FileName.LastIndexOf("\") + 2, SFD.FileName.LastIndexOf(".") - SFD.FileName.LastIndexOf("\") - 1)
                End If
            End If
        End With
    End Sub

    Private Sub SaveAllToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveAllToolStripMenuItem.Click
        For Each item As Instance In Instances
            With item
                If Not .Path Is Nothing AndAlso .Path.Length > 0 Then
                    Dim Writer As New StreamWriter(.Path, False, System.Text.Encoding.GetEncoding(28591))
                    Writer.Write(.SyntaxHandle.Text)
                    Writer.Close()
                    .Saved = True
                Else
                    SFD.InitialDirectory = Settings.DefaultPath
                    If SFD.ShowDialog() = Windows.Forms.DialogResult.Cancel Then Exit Sub
                    If Not SFD.FileName Is Nothing AndAlso SFD.FileName.Length > 0 Then
                        Dim Writer As New StreamWriter(SFD.FileName, False, System.Text.Encoding.GetEncoding(28591))
                        Writer.Write(.SyntaxHandle.Text)
                        Writer.Close()
                        .Saved = True
                        .Path = SFD.FileName
                        .Name = Mid(SFD.FileName, SFD.FileName.LastIndexOf("\") + 2, SFD.FileName.LastIndexOf(".") - SFD.FileName.LastIndexOf("\") - 1)
                    End If
                End If
            End With
        Next
    End Sub

    Private Sub SaveAsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveAsToolStripMenuItem.Click
        SFD.InitialDirectory = Settings.DefaultPath
        If SFD.ShowDialog() = Windows.Forms.DialogResult.Cancel Then Exit Sub
        If Not SFD.FileName Is Nothing AndAlso SFD.FileName.Length > 0 Then
            Dim Writer As New StreamWriter(SFD.FileName, False, System.Text.Encoding.GetEncoding(28591))
            With Instances(TabControl1.SelectedIndex)
                Writer.Write(.SyntaxHandle.Text)
                Writer.Close()
                .Saved = True
                .Path = SFD.FileName
                .Name = Mid(SFD.FileName, SFD.FileName.LastIndexOf("\") + 2, SFD.FileName.LastIndexOf(".") - SFD.FileName.LastIndexOf("\") - 1)
            End With
        End If
    End Sub

    Private Sub CloseToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CloseToolStripMenuItem.Click
        For Each Inst In Instances
            If Inst.Saved = False Then
                Dim Res As MsgBoxResult
                Select Case Settings.Language
                    Case Languages.English
                        Res = MsgBox("Do you want to save " & Inst.Name & "?", MsgBoxStyle.YesNoCancel, "Closing")
                    Case Languages.Español
                        Res = MsgBox("¿Quieres guardar el archivo """ & Inst.Name & """?", MsgBoxStyle.YesNo, "Closing")
                    Case Languages.Portuguêse
                        Res = MsgBox("Quer salvar o arquivo """ & Inst.Name & """?", MsgBoxStyle.YesNo, "Closing")
                    Case Else
                        Res = MsgBox("Wollen Sie die Datei speichern """ & Inst.Name & """?", MsgBoxStyle.YesNo, "Closing")
                End Select
                Select Case Res
                    Case MsgBoxResult.No
                        Continue For
                    Case MsgBoxResult.Yes
                        If Not Inst.Path Is Nothing AndAlso Inst.Path.Length > 0 Then
                            Dim Writer As New StreamWriter(SFD.FileName, False, System.Text.Encoding.GetEncoding(28591))
                            Writer.Write(Inst.SyntaxHandle.Text)
                            Writer.Close()
                            Inst.Saved = True
                        Else
                            SFD.InitialDirectory = Settings.DefaultPath
                            If SFD.ShowDialog() = Windows.Forms.DialogResult.Cancel Then Exit Sub
                            Dim Writer As New StreamWriter(SFD.FileName, False, System.Text.Encoding.GetEncoding(28591))
                            Writer.Write(Inst.SyntaxHandle.Text)
                            Writer.Close()
                            Inst.Saved = True
                            Inst.Path = SFD.FileName
                        End If
                    Case MsgBoxResult.Cancel
                        Exit Sub
                End Select
            End If
        Next
        Application.Exit()
    End Sub

#End Region

#Region "Edit"

    Private Sub UndoToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UndoToolStripMenuItem.Click
        Instances(TabControl1.SelectedIndex).SyntaxHandle.UndoRedo.Undo()
    End Sub

    Private Sub RedoToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RedoToolStripMenuItem.Click
        Instances(TabControl1.SelectedIndex).SyntaxHandle.UndoRedo.Redo()
    End Sub

    Private Sub CopyToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CopyToolStripMenuItem.Click
        On Error Resume Next
        Instances(TabControl1.SelectedIndex).SyntaxHandle.Clipboard.Copy()
    End Sub

    Private Sub CutToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CutToolStripMenuItem.Click
        On Error Resume Next
        Instances(TabControl1.SelectedIndex).SyntaxHandle.Clipboard.Cut()
    End Sub

    Private Sub PasteToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PasteToolStripMenuItem.Click
        On Error Resume Next
        Instances(TabControl1.SelectedIndex).SyntaxHandle.Clipboard.Paste()
    End Sub

    Private Sub FindToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FindToolStripMenuItem.Click
        Instances(TabControl1.SelectedIndex).SyntaxHandle.FindReplace.ShowFind()
    End Sub

    Private Sub FindNextToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FindNextToolStripMenuItem.Click
        Instances(TabControl1.SelectedIndex).SyntaxHandle.FindReplace.Window.FindNext()
    End Sub

    Private Sub FindPrevToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FindPrevToolStripMenuItem.Click
        Instances(TabControl1.SelectedIndex).SyntaxHandle.FindReplace.Window.FindPrevious()
    End Sub

    Private Sub ReplaceToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReplaceToolStripMenuItem.Click
        Instances(TabControl1.SelectedIndex).SyntaxHandle.FindReplace.ShowReplace()
    End Sub

    Private Sub GotoLineToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GotoLineToolStripMenuItem.Click
        Instances(TabControl1.SelectedIndex).SyntaxHandle.GoTo.ShowGoToDialog()
    End Sub

    Private Sub SelectAllToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SelectAllToolStripMenuItem.Click
        Instances(TabControl1.SelectedIndex).SyntaxHandle.Selection.SelectAll()
    End Sub

#End Region

#Region "Build"

    Private Sub BuildToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BuildToolStripMenuItem1.Click
        With Instances(TabControl1.SelectedIndex)
            If Not .Path Is Nothing AndAlso .Path.Length > 0 Then
                If .Ext <> ".inc" Then
                    Dim Writer As New StreamWriter(.Path, False, System.Text.Encoding.GetEncoding(28591))
                    Writer.Write(.SyntaxHandle.Text)
                    Writer.Close()
                    .Saved = True
                Else
                    Select Case Settings.Language
                        Case Languages.English
                            MsgBox("You can't compile a "".inc"" file", MsgBoxStyle.Critical, "Error")
                        Case Languages.Español
                            MsgBox("No puedes compilar un archivo "".inc""", MsgBoxStyle.Critical, "Error")
                        Case Languages.Portuguêse
                            MsgBox("Você não pode compilar um "".Inc"" file", MsgBoxStyle.Critical, "Error")
                        Case Else
                            MsgBox("Man kann nicht kompilieren ""Inc""-Datei", MsgBoxStyle.Critical, "Error")
                    End Select
                    Exit Sub
                End If
            Else
                SFD.InitialDirectory = Settings.DefaultPath
                If SFD.ShowDialog() = Windows.Forms.DialogResult.Cancel Then Exit Sub
                If Not SFD.FileName Is Nothing AndAlso SFD.FileName.Length > 0 Then
                    Dim Writer As New StreamWriter(SFD.FileName, False, System.Text.Encoding.GetEncoding(28591))
                    Writer.Write(Instances(TabControl1.SelectedIndex).SyntaxHandle.Text)
                    Writer.Close()
                    .Saved = True
                    .Path = SFD.FileName
                    .Name = Mid(SFD.FileName, SFD.FileName.LastIndexOf("\") + 2, SFD.FileName.LastIndexOf(".") - SFD.FileName.LastIndexOf("\") - 1)
                    If .Ext = ".inc" Then
                        Select Case Settings.Language
                            Case Languages.English
                                MsgBox("You can't compile a "".inc"" file", MsgBoxStyle.Critical, "Error")
                            Case Languages.Español
                                MsgBox("No puedes compilar un archivo "".inc""", MsgBoxStyle.Critical, "Error")
                            Case Languages.Portuguêse
                                MsgBox("Você não pode compilar um "".Inc"" file", MsgBoxStyle.Critical, "Error")
                            Case Else
                                MsgBox("Man kann nicht kompilieren ""Inc""-Datei", MsgBoxStyle.Critical, "Error")
                        End Select
                        Exit Sub
                    End If
                Else
                    Exit Sub
                End If
            End If
        End With
        Dim P As New Process(), out As String, err As String
        With P
            With .StartInfo
                .UseShellExecute = False
                .RedirectStandardOutput = True
                .RedirectStandardError = True
                .CreateNoWindow = True
                .FileName = Settings.CompPath
                .Arguments = """" & Instances(TabControl1.SelectedIndex).Path & """" & If(Settings.CompArgs.StartsWith(" "), Settings.CompArgs, " " & Settings.CompArgs) & " -; -("
            End With
            .Start()
            out = .StandardOutput.ReadToEnd
            err = .StandardError.ReadToEnd
            .WaitForExit()
            .Close()
        End With
        For Each pr As Process In Process.GetProcesses
            If pr Is P Then pr.Kill()
        Next
        Dim tmpf As String = Mid(Instances(TabControl1.SelectedIndex).Path, 1, Instances(TabControl1.SelectedIndex).Path.LastIndexOf("\")) & ".xml"
        If File.Exists(tmpf) Then File.Delete(tmpf)
        Dim errs As String(), tmp As String()
        errs = Split(err, vbNewLine)
        With Instances(TabControl1.SelectedIndex)
            .Errors.Clear()
            For Each er As String In errs
                If er.Length > 0 Then
                    Try
                        tmp = Split(er, " : ")
                        .Errors.Add(New ListViewItem(New String() {"", Trim(System.Text.RegularExpressions.Regex.Replace(Mid(tmp(1), 1, tmp(1).IndexOf(":")), "[A-z]", "")), Mid(tmp(0), tmp(0).LastIndexOf("\") + 2, tmp(0).LastIndexOf(".") - tmp(0).LastIndexOf("\") - 1), Regex.Match(tmp(0), "\([\d\s-]+\)").Value.Replace("(", "").Replace(")", ""), Mid(tmp(1), tmp(1).IndexOf(":") + 3, tmp(1).Length - tmp(1).IndexOf(":") - 1)}, If(tmp(1).IndexOf("error") > -1, 0, 1)))
                    Catch ex As Exception

                    End Try
                End If
            Next
            If Settings.OETab Then
                TextBox1.Text = "Output from """ & .Name & """ finished at: " & If(Date.Now.Hour < 10, "0" & Date.Now.Hour, Date.Now.Hour) & ":" & If(Date.Now.Minute < 10, "0" & Date.Now.Minute, Date.Now.Minute) & ":" & Date.Now.Second & vbNewLine & out
                TextBox1.SelectionStart = TextBox1.Text.Length
                TextBox1.SelectionLength = 0
                ListView1.Items.Clear()
                Dim Header As Boolean() = New Boolean() {True, True, True}
                For Each item As ListViewItem In .Errors
                    ListView1.Items.Add(item)
                    If Not Header(0) AndAlso ListView1.Columns(1).Text.Length <= item.SubItems(1).Text.Length Then Header(0) = False
                    If Not Header(1) AndAlso ListView1.Columns(2).Text.Length <= item.SubItems(2).Text.Length Then Header(1) = False
                    If Not Header(2) AndAlso ListView1.Columns(3).Text.Length <= item.SubItems(3).Text.Length Then Header(2) = False
                Next
                If err.Length Then
                    TabControl3.SelectedIndex = 0
                Else
                    TabControl3.SelectedIndex = 1
                End If
                With ListView1
                    .Columns(0).Width = 25
                    .Columns(1).Width = If(Header(0), -2, -1)
                    .Columns(2).Width = If(Header(1), -2, -1)
                    .Columns(3).Width = If(Header(2), -2, -1)
                    .Columns(4).Width = -2
                End With
            End If
        End With
    End Sub

#End Region

#Region "Tools"

    Private Sub TeleportsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TeleportsToolStripMenuItem.Click
        Instances(TabControl1.SelectedIndex).SyntaxHandle.Invoke(Instances(TabControl1.SelectedIndex).DataUpdaterEx, New Object() {Instance.UpdateType.Colors, 0, Instances(TabControl1.SelectedIndex).SyntaxHandle.Lines.Count})
        Tools.Show()
        Tools.TabControl1.SelectedTab = Tools.TabPage6
    End Sub

    Private Sub GatesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GatesToolStripMenuItem.Click
        Instances(TabControl1.SelectedIndex).SyntaxHandle.Invoke(Instances(TabControl1.SelectedIndex).DataUpdaterEx, New Object() {Instance.UpdateType.Colors, 0, Instances(TabControl1.SelectedIndex).SyntaxHandle.Lines.Count})
        Tools.Show()
        Tools.TabControl1.SelectedTab = Tools.TabPage12
    End Sub

    Private Sub DialogsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DialogsToolStripMenuItem.Click
        Instances(TabControl1.SelectedIndex).SyntaxHandle.Invoke(Instances(TabControl1.SelectedIndex).DataUpdaterEx, New Object() {Instance.UpdateType.Colors, 0, Instances(TabControl1.SelectedIndex).SyntaxHandle.Lines.Count})
        Tools.Show()
        Tools.TabControl1.SelectedTab = Tools.TabPage1
    End Sub

    Private Sub AreasToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AreasToolStripMenuItem.Click
        Instances(TabControl1.SelectedIndex).SyntaxHandle.Invoke(Instances(TabControl1.SelectedIndex).DataUpdaterEx, New Object() {Instance.UpdateType.Colors, 0, Instances(TabControl1.SelectedIndex).SyntaxHandle.Lines.Count})
        Tools.Show()
        Tools.TabControl1.SelectedTab = Tools.TabPage3
    End Sub

    Private Sub ColorPickerToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ColorPickerToolStripMenuItem.Click
        Instances(TabControl1.SelectedIndex).SyntaxHandle.Invoke(Instances(TabControl1.SelectedIndex).DataUpdaterEx, New Object() {Instance.UpdateType.Colors, 0, Instances(TabControl1.SelectedIndex).SyntaxHandle.Lines.Count})
        Tools.Show()
        Tools.TabControl1.SelectedTab = Tools.TabPage2
    End Sub

    Private Sub ConverterToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ConverterToolStripMenuItem.Click
        Instances(TabControl1.SelectedIndex).SyntaxHandle.Invoke(Instances(TabControl1.SelectedIndex).DataUpdaterEx, New Object() {Instance.UpdateType.Colors, 0, Instances(TabControl1.SelectedIndex).SyntaxHandle.Lines.Count})
        Tools.Show()
        Tools.TabControl1.SelectedTab = Tools.TabPage4
    End Sub

    Private Sub AnimsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AnimsToolStripMenuItem.Click
        Instances(TabControl1.SelectedIndex).SyntaxHandle.Invoke(Instances(TabControl1.SelectedIndex).DataUpdaterEx, New Object() {Instance.UpdateType.Colors, 0, Instances(TabControl1.SelectedIndex).SyntaxHandle.Lines.Count})
        Tools.Show()
        Tools.TabControl1.SelectedTab = Tools.TabPage5
        Tools.TabControl3.SelectedTab = Tools.TabPage10
    End Sub

    Private Sub MapIconsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MapIconsToolStripMenuItem.Click
        Instances(TabControl1.SelectedIndex).SyntaxHandle.Invoke(Instances(TabControl1.SelectedIndex).DataUpdaterEx, New Object() {Instance.UpdateType.Colors, 0, Instances(TabControl1.SelectedIndex).SyntaxHandle.Lines.Count})
        Tools.Show()
        Tools.TabControl1.SelectedTab = Tools.TabPage5
        Tools.TabControl3.SelectedTab = Tools.TabPage14
    End Sub

    Private Sub SkinsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SkinsToolStripMenuItem.Click
        Instances(TabControl1.SelectedIndex).SyntaxHandle.Invoke(Instances(TabControl1.SelectedIndex).DataUpdaterEx, New Object() {Instance.UpdateType.Colors, 0, Instances(TabControl1.SelectedIndex).SyntaxHandle.Lines.Count})
        Tools.Show()
        Tools.TabControl1.SelectedTab = Tools.TabPage5
        Tools.TabControl3.SelectedTab = Tools.TabPage7
    End Sub

    Private Sub SoundsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SoundsToolStripMenuItem.Click
        Instances(TabControl1.SelectedIndex).SyntaxHandle.Invoke(Instances(TabControl1.SelectedIndex).DataUpdaterEx, New Object() {Instance.UpdateType.Colors, 0, Instances(TabControl1.SelectedIndex).SyntaxHandle.Lines.Count})
        Tools.Show()
        Tools.TabControl1.SelectedTab = Tools.TabPage5
        Tools.TabControl3.SelectedTab = Tools.TabPage9
    End Sub

    Private Sub SpritesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SpritesToolStripMenuItem.Click
        Instances(TabControl1.SelectedIndex).SyntaxHandle.Invoke(Instances(TabControl1.SelectedIndex).DataUpdaterEx, New Object() {Instance.UpdateType.Colors, 0, Instances(TabControl1.SelectedIndex).SyntaxHandle.Lines.Count})
        Tools.Show()
        Tools.TabControl1.SelectedTab = Tools.TabPage5
        Tools.TabControl3.SelectedTab = Tools.TabPage19
    End Sub

    Private Sub VehiclesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles VehiclesToolStripMenuItem.Click
        Instances(TabControl1.SelectedIndex).SyntaxHandle.Invoke(Instances(TabControl1.SelectedIndex).DataUpdaterEx, New Object() {Instance.UpdateType.Colors, 0, Instances(TabControl1.SelectedIndex).SyntaxHandle.Lines.Count})
        Tools.Show()
        Tools.TabControl1.SelectedTab = Tools.TabPage5
        Tools.TabControl3.SelectedTab = Tools.TabPage8
    End Sub

    Private Sub WeaponsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles WeaponsToolStripMenuItem.Click
        Instances(TabControl1.SelectedIndex).SyntaxHandle.Invoke(Instances(TabControl1.SelectedIndex).DataUpdaterEx, New Object() {Instance.UpdateType.Colors, 0, Instances(TabControl1.SelectedIndex).SyntaxHandle.Lines.Count})
        Tools.Show()
        Tools.TabControl1.SelectedTab = Tools.TabPage5
        Tools.TabControl3.SelectedTab = Tools.TabPage11
    End Sub

#End Region

#Region "Options"

    Private Sub EditorOptionsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EditorOptionsToolStripMenuItem.Click
        Options.Visible = True
    End Sub

#End Region

#Region "Help"

#Region "SA-MP"

    Private Sub WebpageToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles WebPageToolStripMenuItem.Click
        Process.Start("http://sa-mp.com")
    End Sub

    Private Sub ForumToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ForumToolStripMenuItem.Click
        Process.Start("http://forum.sa-mp.com")
    End Sub

#Region "Wiki"

    Private Sub MainPageToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MainPageToolStripMenuItem.Click
        Process.Start("http://wiki.sa-mp.com")
    End Sub

    Private Sub SearchForToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SearchForToolStripMenuItem.Click
        Srch.Show()
    End Sub

#End Region

#End Region

    Private Sub CreditsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CreditsToolStripMenuItem.Click
        On Error Resume Next
        tSender = MsgT.Credits
        With MultiF
            .Show()
            .Size = New Size(532, 335)
            .Location = New Point(Me.Location.X + 130, Me.Location.Y + 100)
            .Opacity = 70
        End With
    End Sub

    Private Sub ReportBugToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReportBugToolStripMenuItem.Click
        Process.Start("https://accounts.google.com/ServiceLogin?service=code&ltmpl=phosting&continue=http%3A%2F%2Fcode.google.com%2Fp%2Fscripting-machine%2Fissues%2Fentry")
    End Sub

    Private Sub ContactToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ContactToolStripMenuItem.Click
        On Error Resume Next
        tSender = MsgT.Contact
        With MultiF
            .Show()
            .Size = New Size(532, 335)
            .Location = New Point(Me.Location.X + 130, Me.Location.Y + 100)
            .Opacity = 70
        End With
    End Sub

#End Region

#End Region

#Region "ToolBar"

    ''' <summary>
    ''' New
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ToolStripButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton1.Click
        Dim count As Integer, name As String
        For Each item In Instances
            If item.Name.IndexOf("new script") > -1 Then
                count += 1
            End If
        Next

        If count = 0 Then
            name = "new script"
        Else
            name = "new script" & count
        End If
        Instances.Add(New Instance(name, Instances.Count))
        If File.Exists(My.Application.Info.DirectoryPath & "\Scripts\new.pwn") Then
            Dim Reader As New StreamReader(My.Application.Info.DirectoryPath & "\Scripts\new.pwn", System.Text.Encoding.GetEncoding(28591))
            With Instances(GetInstanceByName(name, Instances.Count - 1))
                .SyntaxHandle.Text = Reader.ReadToEnd()
                .Path = My.Application.Info.DirectoryPath & "\Scripts\new.pwn"
                TabControl1.SelectedTab = .TabHandle
            End With
            Reader.Close()
        Else
            Select Case Settings.Language
                Case Languages.English
                    MsgBox("File could not be found.", MsgBoxStyle.Critical, "Error")
                Case Languages.Español
                    MsgBox("Archivo no encontrado.", MsgBoxStyle.Critical, "Error")
                Case Languages.Portuguêse
                    MsgBox("Arquivo não pôde ser encontrado.", MsgBoxStyle.Critical, "Error")
                Case Else
                    MsgBox("Datei konnte nicht gefunden werden.", MsgBoxStyle.Critical, "Error")
            End Select
        End If
    End Sub

    ''' <summary>
    ''' Open
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ToolStripButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton2.Click
        OFD.InitialDirectory = Settings.DefaultPath
        If OFD.ShowDialog() = Windows.Forms.DialogResult.Cancel Then Exit Sub
        If Not OFD.FileName Is Nothing AndAlso OFD.FileName.Length > 0 Then
            If File.Exists(OFD.FileName) Then
                Dim name As String
                name = Mid(OFD.FileName, OFD.FileName.LastIndexOf("\") + 2, OFD.FileName.LastIndexOf(".") - OFD.FileName.LastIndexOf("\") - 1)
                Instances.Add(New Instance(name, Instances.Count))
                With Instances(GetInstanceByName(name, Instances.Count - 1))
                    TabControl1.SelectedTab = .TabHandle
                    Dim Reader As StreamReader
                    If OFD.FileName.EndsWith(".pwn") OrElse OFD.FileName.EndsWith(".inc") Then
                        Reader = New StreamReader(OFD.FileName, System.Text.Encoding.GetEncoding(28591), True)
                    Else
                        Reader = New StreamReader(OFD.FileName, System.Text.Encoding.UTF8, True)
                    End If
                    .SyntaxHandle.Text = Reader.ReadToEnd()
                    Reader.Close()
                    .Path = OFD.FileName
                End With
            End If
        End If
    End Sub

    ''' <summary>
    ''' Save
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ToolStripButton3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton3.Click
        With Instances(TabControl1.SelectedIndex)
            If Not .Path Is Nothing AndAlso .Path.Length > 0 Then
                Dim Writer As New StreamWriter(.Path, False, System.Text.Encoding.GetEncoding(28591))
                Writer.Write(.SyntaxHandle.Text)
                Writer.Close()
                .Saved = True
            Else
                SFD.InitialDirectory = Settings.DefaultPath
                If SFD.ShowDialog() = Windows.Forms.DialogResult.Cancel Then Exit Sub
                If Not SFD.FileName Is Nothing AndAlso SFD.FileName.Length > 0 Then
                    Dim Writer As New StreamWriter(SFD.FileName, False, System.Text.Encoding.GetEncoding(28591))
                    Writer.Write(.SyntaxHandle.Text)
                    Writer.Close()
                    .Saved = True
                    .Path = SFD.FileName
                    .Name = Mid(SFD.FileName, SFD.FileName.LastIndexOf("\") + 2, SFD.FileName.LastIndexOf(".") - SFD.FileName.LastIndexOf("\") - 1)
                End If
            End If
        End With
    End Sub

    ''' <summary>
    ''' Copy
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ToolStripButton4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton4.Click
        On Error Resume Next
        Instances(TabControl1.SelectedIndex).SyntaxHandle.Clipboard.Copy()
    End Sub

    ''' <summary>
    ''' Cut
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ToolStripButton5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton5.Click
        On Error Resume Next
        Instances(TabControl1.SelectedIndex).SyntaxHandle.Clipboard.Cut()
    End Sub

    ''' <summary>
    ''' Paste
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ToolStripButton6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton6.Click
        On Error Resume Next
        Instances(TabControl1.SelectedIndex).SyntaxHandle.Clipboard.Paste()
    End Sub

    ''' <summary>
    ''' Undo
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ToolStripButton7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton7.Click
        Instances(TabControl1.SelectedIndex).SyntaxHandle.UndoRedo.Undo()
    End Sub

    ''' <summary>
    ''' Redo
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ToolStripButton8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton8.Click
        Instances(TabControl1.SelectedIndex).SyntaxHandle.UndoRedo.Redo()
    End Sub

    ''' <summary>
    ''' Find
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ToolStripButton9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton9.Click
        Instances(TabControl1.SelectedIndex).SyntaxHandle.FindReplace.ShowFind()
    End Sub

    ''' <summary>
    ''' Goto line
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ToolStripButton10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton10.Click
        Instances(TabControl1.SelectedIndex).SyntaxHandle.GoTo.ShowGoToDialog()
    End Sub

    ''' <summary>
    ''' Compile
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ToolStripButton11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton11.Click
        With Instances(TabControl1.SelectedIndex)
            If Not .Path Is Nothing AndAlso .Path.Length > 0 Then
                If .Ext <> ".inc" Then
                    Dim Writer As New StreamWriter(.Path, False, System.Text.Encoding.GetEncoding(28591))
                    Writer.Write(.SyntaxHandle.Text)
                    Writer.Close()
                    .Saved = True
                Else
                    Select Case Settings.Language
                        Case Languages.English
                            MsgBox("You can't compile a "".inc"" file", MsgBoxStyle.Critical, "Error")
                        Case Languages.Español
                            MsgBox("No puedes compilar un archivo "".inc""", MsgBoxStyle.Critical, "Error")
                        Case Languages.Portuguêse
                            MsgBox("Você não pode compilar um "".Inc"" file", MsgBoxStyle.Critical, "Error")
                        Case Else
                            MsgBox("Man kann nicht kompilieren ""Inc""-Datei", MsgBoxStyle.Critical, "Error")
                    End Select
                    Exit Sub
                End If
            Else
                SFD.InitialDirectory = Settings.DefaultPath
                If SFD.ShowDialog() = Windows.Forms.DialogResult.Cancel Then Exit Sub
                If Not SFD.FileName Is Nothing AndAlso SFD.FileName.Length > 0 Then
                    Dim Writer As New StreamWriter(SFD.FileName, False, System.Text.Encoding.GetEncoding(28591))
                    Writer.Write(Instances(TabControl1.SelectedIndex).SyntaxHandle.Text)
                    Writer.Close()
                    .Saved = True
                    .Path = SFD.FileName
                    .Name = Mid(SFD.FileName, SFD.FileName.LastIndexOf("\") + 2, SFD.FileName.LastIndexOf(".") - SFD.FileName.LastIndexOf("\") - 1)
                    If .Ext = ".inc" Then
                        Select Case Settings.Language
                            Case Languages.English
                                MsgBox("You can't compile a "".inc"" file", MsgBoxStyle.Critical, "Error")
                            Case Languages.Español
                                MsgBox("No puedes compilar un archivo "".inc""", MsgBoxStyle.Critical, "Error")
                            Case Languages.Portuguêse
                                MsgBox("Você não pode compilar um "".Inc"" file", MsgBoxStyle.Critical, "Error")
                            Case Else
                                MsgBox("Man kann nicht kompilieren ""Inc""-Datei", MsgBoxStyle.Critical, "Error")
                        End Select
                        Exit Sub
                    End If
                Else
                    Exit Sub
                End If
            End If
        End With
        Dim P As New Process(), out As String, err As String
        With P
            With .StartInfo
                .UseShellExecute = False
                .RedirectStandardOutput = True
                .RedirectStandardError = True
                .CreateNoWindow = True
                .FileName = Settings.CompPath
                .Arguments = """" & Instances(TabControl1.SelectedIndex).Path & """" & If(Settings.CompArgs.StartsWith(" "), Settings.CompArgs, " " & Settings.CompArgs) & " -; -("
            End With
            .Start()
            out = .StandardOutput.ReadToEnd
            err = .StandardError.ReadToEnd
            .WaitForExit()
            .Close()
        End With
        For Each pr As Process In Process.GetProcesses
            If pr Is P Then pr.Kill()
        Next
        Dim tmpf As String = Mid(Instances(TabControl1.SelectedIndex).Path, 1, Instances(TabControl1.SelectedIndex).Path.LastIndexOf("\")) & ".xml"
        If File.Exists(tmpf) Then File.Delete(tmpf)
        Dim errs As String(), tmp As String()
        errs = Split(err, vbNewLine)
        With Instances(TabControl1.SelectedIndex)
            .Errors.Clear()
            For Each er As String In errs
                If er.Length > 0 Then
                    Try
                        tmp = Split(er, " : ")
                        .Errors.Add(New ListViewItem(New String() {"", Trim(System.Text.RegularExpressions.Regex.Replace(Mid(tmp(1), 1, tmp(1).IndexOf(":")), "[A-z]", "")), Mid(tmp(0), tmp(0).LastIndexOf("\") + 2, tmp(0).LastIndexOf(".") - tmp(0).LastIndexOf("\") - 1), Regex.Match(tmp(0), "\([\d\s-]+\)").Value.Replace("(", "").Replace(")", ""), Mid(tmp(1), tmp(1).IndexOf(":") + 3, tmp(1).Length - tmp(1).IndexOf(":") - 1)}, If(tmp(1).IndexOf("error") > -1, 0, 1)))
                    Catch ex As Exception
                    End Try
                End If
            Next
            If Settings.OETab Then
                TextBox1.Text = "Output from """ & .Name & """ finished at: " & If(Date.Now.Hour < 10, "0" & Date.Now.Hour, Date.Now.Hour) & ":" & If(Date.Now.Minute < 10, "0" & Date.Now.Minute, Date.Now.Minute) & ":" & Date.Now.Second & vbNewLine & out
                TextBox1.SelectionStart = TextBox1.Text.Length
                TextBox1.SelectionLength = 0
                ListView1.Items.Clear()
                Dim Header As Boolean() = New Boolean() {True, True, True}
                For Each item As ListViewItem In .Errors
                    ListView1.Items.Add(item)
                    If Not Header(0) AndAlso ListView1.Columns(1).Text.Length <= item.SubItems(1).Text.Length Then Header(0) = False
                    If Not Header(1) AndAlso ListView1.Columns(2).Text.Length <= item.SubItems(2).Text.Length Then Header(1) = False
                    If Not Header(2) AndAlso ListView1.Columns(3).Text.Length <= item.SubItems(3).Text.Length Then Header(2) = False
                Next
                If err.Length Then
                    TabControl3.SelectedIndex = 0
                Else
                    TabControl3.SelectedIndex = 1
                End If
                With ListView1
                    .Columns(0).Width = 25
                    .Columns(1).Width = If(Header(0), -2, -1)
                    .Columns(2).Width = If(Header(1), -2, -1)
                    .Columns(3).Width = If(Header(2), -2, -1)
                    .Columns(4).Width = -2
                End With
            End If
        End With
    End Sub

#End Region

#End Region

#Region "Inclue Lists"

    ''' <summary>
    ''' Add current instance's text the selected function
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub TreeView1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles TreeView1.DoubleClick
        With Instances(TabControl1.SelectedIndex)
            For Each func As PawnFunction In AllFunctions
                If func.Name = TreeView1.SelectedNode.Text Then
                    .SyntaxHandle.Selection.Text = func.Name & "("
                    .SyntaxHandle.Focus()
                End If
            Next
            Try
                If Not TreeView1.SelectedNode.IsExpanded Then
                    TreeView1.SelectedNode.Expand()
                    .SyntaxHandle.Focus()
                Else
                    .SyntaxHandle.Selection.Text = "#include <" & Mid(TreeView1.SelectedNode.Text, 1, TreeView1.SelectedNode.Text.Length - 1) & ">"
                    .SyntaxHandle.Focus()
                End If
            Catch ex As Exception
                .SyntaxHandle.Selection.Text = "#include <" & Mid(TreeView1.SelectedNode.Text, 1, TreeView1.SelectedNode.Text.Length - 1) & ">"
                .SyntaxHandle.Focus()
            End Try
        End With
    End Sub

    ''' <summary>
    ''' Add current instance's text the selected function
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub TreeView2_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles TreeView2.DoubleClick
        If TreeView2.SelectedNode.Text = "Current:" Then Exit Sub
        With Instances(TabControl1.SelectedIndex)
            For Each func As PawnFunction In .ACLists.Functions
                If func.Name = TreeView2.SelectedNode.Text Then
                    .SyntaxHandle.Selection.Text = func.Name & "("
                    .SyntaxHandle.Focus()
                    Exit Sub
                End If
            Next
            Try
                If Not TreeView1.SelectedNode.IsExpanded Then
                    TreeView1.SelectedNode.Expand()
                    .SyntaxHandle.Focus()
                Else
                    .SyntaxHandle.Selection.Text = "#include <" & Mid(TreeView2.SelectedNode.Text, 1, TreeView2.SelectedNode.Text.Length - 1) & ">"
                    .SyntaxHandle.Focus()
                End If
            Catch ex As Exception
                .SyntaxHandle.Selection.Text = "#include <" & Mid(TreeView2.SelectedNode.Text, 1, TreeView2.SelectedNode.Text.Length - 1) & ">"
                .SyntaxHandle.Focus()
            End Try
        End With
    End Sub

    ''' <summary>
    ''' Refresh current instance's include list
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Instances(TabControl1.SelectedIndex).SyntaxHandle.Invoke(Instances(TabControl1.SelectedIndex).DataUpdater)
    End Sub

    ''' <summary>
    ''' Refresh main include list
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        LoadIncludes(True)
    End Sub

    ''' <summary>
    ''' Refresh current instance's include list
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub TabControl2_Selected(ByVal sender As Object, ByVal e As System.Windows.Forms.TabControlEventArgs) Handles TabControl2.Selected
        If TabControl2.SelectedTab Is TabPage2 Then Instances(TabControl1.SelectedIndex).SyntaxHandle.Invoke(Instances(TabControl1.SelectedIndex).DataUpdater)
        If TabControl2.SelectedTab Is TabPage1 Then
            With Instances(TabControl1.SelectedIndex)
                If .CurrentFunction = "SetPlayerSkin" Then
                    PictureBox1.Enabled = True
                    ComboBox1.Enabled = True
                    Button3.Enabled = True
                    If PictureBox1.Image Is My.Resources.N_A Then ComboBox1.SelectedIndex = 0
                Else
                    PictureBox1.Image = My.Resources.N_A
                    PictureBox1.Enabled = False
                    ComboBox1.Enabled = False
                    Button3.Enabled = False
                End If
                Select Case Settings.Language
                    Case Languages.English
                        Label2.Text = "Document lines: " & .SyntaxHandle.Lines.Count
                    Case Languages.Español
                        Label2.Text = "Lineas del documento: " & .SyntaxHandle.Lines.Count
                    Case Languages.Portuguêse
                        Label2.Text = "Linhas de documento: " & .SyntaxHandle.Lines.Count
                    Case Else
                        Label2.Text = "Document Linien: " & .SyntaxHandle.Lines.Count
                End Select
            End With
        End If
    End Sub

#End Region

#Region "Instances"

    Private Sub TabPage1_ControlAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.ControlEventArgs) Handles TabPage1.ControlAdded
        On Error Resume Next
        If Not TabControl1.SelectedTab Is Nothing Then
            With Instances(TabControl1.SelectedIndex)
                If .Saved Then
                    ToolStripButton3.Enabled = False
                    SaveToolStripMenuItem.Enabled = False
                Else
                    ToolStripButton3.Enabled = True
                    SaveToolStripMenuItem.Enabled = True
                End If
                If .SyntaxHandle.UndoRedo.CanUndo Then
                    UndoToolStripMenuItem.Enabled = True
                    ToolStripButton7.Enabled = True
                Else
                    UndoToolStripMenuItem.Enabled = False
                    ToolStripButton7.Enabled = False
                End If
                If .SyntaxHandle.UndoRedo.CanRedo Then
                    RedoToolStripMenuItem.Enabled = True
                    ToolStripButton8.Enabled = True
                Else
                    RedoToolStripMenuItem.Enabled = False
                    ToolStripButton8.Enabled = False
                End If
            End With
        End If
    End Sub

    ''' <summary>
    ''' Destroy selected instance
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub TabControl1_ControlRemoved(ByVal sender As Object, ByVal e As System.Windows.Forms.ControlEventArgs) Handles TabPage1.ControlRemoved
        If Instances.Count = 0 Then
            Instances.Add(New Instance("new script", Instances.Count))
            If File.Exists(My.Application.Info.DirectoryPath & "\Scripts\new.pwn") Then
                Dim Reader As New StreamReader(My.Application.Info.DirectoryPath & "\Scripts\new.pwn", System.Text.Encoding.GetEncoding(28591))
                Instances(0).SyntaxHandle.Text = Reader.ReadToEnd()
                Reader.Close()
            End If
        Else
            For Each Item In Instances
                If TabControl1.Controls.Contains(Item.TabHandle) = False Then
                    Instances.Remove(Item)
                    Exit For
                End If
            Next
        End If
    End Sub

    ''' <summary>
    ''' Destroy selected instance
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub TabControl1_OnClose(ByVal sender As Object, ByVal e As TabControlEx.CloseEventArgs) Handles TabControl1.OnClose
        Dim index As Integer = GetInstanceByName(Trim(TabControl1.TabPages(e.TabIndex).Text), Instances.Count - 1)
        Instances(index).destroy()
        Instances.RemoveAt(index)
        TabControl1.Controls.RemoveAt(e.TabIndex)
        If Instances.Count = 0 Then
            Instances.Add(New Instance("new script", Instances.Count))
            If File.Exists(My.Application.Info.DirectoryPath & "\Scripts\new.pwn") Then
                Dim Reader As New StreamReader(My.Application.Info.DirectoryPath & "\Scripts\new.pwn", System.Text.Encoding.GetEncoding(28591))
                With Instances(GetInstanceByName("new script", Instances.Count - 1))
                    .SyntaxHandle.Text = Reader.ReadToEnd()
                    .Path = My.Application.Info.DirectoryPath & "\Scripts\new.pwn"
                End With
                Reader.Close()
            End If
        End If
        With Instances(TabControl1.SelectedIndex)
            If .Errors.Count AndAlso .Errors(0).Text <> "" Then
                Dim Header As Boolean() = New Boolean() {True, True, True}
                For Each item As ListViewItem In .Errors
                    ListView1.Items.Add(item)
                    If Not Header(0) AndAlso ListView1.Columns(1).Text.Length <= item.SubItems(1).Text.Length Then Header(0) = False
                    If Not Header(1) AndAlso ListView1.Columns(2).Text.Length <= item.SubItems(2).Text.Length Then Header(1) = False
                    If Not Header(2) AndAlso ListView1.Columns(3).Text.Length <= item.SubItems(3).Text.Length Then Header(2) = False
                Next
                With ListView1
                    .Columns(0).Width = 25
                    .Columns(1).Width = If(Header(0), -2, -1)
                    .Columns(2).Width = If(Header(1), -2, -1)
                    .Columns(3).Width = If(Header(2), -2, -1)
                    .Columns(4).Width = -2
                End With
            Else
                With ListView1
                    .Items.Clear()
                    .Columns(0).Width = 25
                    .Columns(1).Width = -2
                    .Columns(2).Width = -2
                    .Columns(3).Width = -2
                    .Columns(4).Width = -2
                End With
            End If
        End With
    End Sub

    ''' <summary>
    ''' Update controls using current instance's info
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub TabControl1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TabControl1.SelectedIndexChanged
        On Error Resume Next
        With Instances(TabControl1.SelectedIndex)
            If .Saved Then
                ToolStripButton3.Enabled = False
                SaveAsToolStripMenuItem.Enabled = False
            Else
                ToolStripButton3.Enabled = True
                SaveAsToolStripMenuItem.Enabled = True
            End If
            If .SyntaxHandle.UndoRedo.CanUndo Then
                UndoToolStripMenuItem.Enabled = True
                ToolStripButton7.Enabled = True
            Else
                UndoToolStripMenuItem.Enabled = False
                ToolStripButton7.Enabled = False
            End If
            If .SyntaxHandle.UndoRedo.CanRedo Then
                RedoToolStripMenuItem.Enabled = True
                ToolStripButton8.Enabled = True
            Else
                RedoToolStripMenuItem.Enabled = False
                ToolStripButton8.Enabled = False
            End If
            If .Errors.Count Then
                Dim Header As Boolean() = New Boolean() {True, True, True}
                ListView1.Items.Clear()
                For Each item As ListViewItem In .Errors
                    ListView1.Items.Add(item)
                    If Not Header(0) AndAlso ListView1.Columns(1).Text.Length <= item.SubItems(1).Text.Length Then Header(0) = False
                    If Not Header(1) AndAlso ListView1.Columns(2).Text.Length <= item.SubItems(2).Text.Length Then Header(1) = False
                    If Not Header(2) AndAlso ListView1.Columns(3).Text.Length <= item.SubItems(3).Text.Length Then Header(2) = False
                Next
                With ListView1
                    .Columns(0).Width = 25
                    .Columns(1).Width = If(Header(0), -2, -1)
                    .Columns(2).Width = If(Header(1), -2, -1)
                    .Columns(3).Width = If(Header(2), -2, -1)
                    .Columns(4).Width = -2
                End With
            Else
                With ListView1
                    .Items.Clear()
                    .Columns(0).Width = 25
                    .Columns(1).Width = -2
                    .Columns(2).Width = -2
                    .Columns(3).Width = -2
                    .Columns(4).Width = -2
                End With
            End If
            If Not .Saved Then .SyntaxHandle.Invoke(.DataUpdater)
        End With
    End Sub

#End Region

#Region "Extra (Tab)"

    Private Sub ComboBox1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles ComboBox1.KeyPress
        If Asc(e.KeyChar) = Keys.Enter Then
            Dim tmp As Boolean
            If Settings.Images Or Settings.URL_Skin.Length Then
                tmp = True
            Else
                tmp = False
            End If
            If tmp = False Then
                Try
                    PictureBox1.Image = LoadImageFromURL(String.Format(Settings.URL_Skin, ComboBox1.Text))
                Catch ex As Exception
                    PictureBox1.Image = My.Resources.N_A
                End Try
            End If
            For Each Skin In Skins
                If ComboBox1.Text = Skin.ID Then
                    If tmp = True Then
                        Try
                            PictureBox1.Image = Image.FromFile(My.Application.Info.DirectoryPath & "\Resources\Skin_" & Skin.ID & ".png")
                        Catch ex As Exception
                            PictureBox1.Image = My.Resources.N_A
                        End Try
                    End If
                    Exit For
                End If
            Next
        End If
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        Dim tmp As Boolean
        If Settings.Images OrElse Settings.URL_Skin <> "" Then
            tmp = True
        Else
            tmp = False
        End If
        If tmp = False Then
            Try
                PictureBox1.Image = LoadImageFromURL(String.Format(Settings.URL_Skin, ComboBox1.Text))
            Catch ex As Exception
                PictureBox1.Image = My.Resources.N_A
            End Try
        End If
        For Each Skin In Skins
            If ComboBox1.Text = Skin.ID Then
                If tmp = True Then
                    Try
                        PictureBox1.Image = Image.FromFile(My.Application.Info.DirectoryPath & "\Resources\Skin_" & Skin.ID & ".png")
                    Catch ex As Exception
                        PictureBox1.Image = My.Resources.N_A
                    End Try
                End If
                Exit For
            End If
        Next
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        With Instances(TabControl1.SelectedIndex)
            .SyntaxHandle.Selection.Text = ComboBox1.Text & ");"
            .SyntaxHandle.Focus()
            If .ShowingInfoText AndAlso .CurrentFunction = "" Then .ShowingInfoText = False
        End With
    End Sub

#End Region

#Region "Errors List"

    Private Sub ListView1_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ListView1.MouseClick
        If e.Button = Windows.Forms.MouseButtons.Right Then

        End If
    End Sub

    Private Sub ListView1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListView1.SelectedIndexChanged
        If Not ListView1.FocusedItem Is Nothing AndAlso ListView1.FocusedItem.SubItems(3).Text <> "" Then
            Dim line As String, index = -1
            line = ListView1.FocusedItem.SubItems(3).Text
            For i = 0 To Instances.Count - 1
                If Instances(i).Name = ListView1.FocusedItem.SubItems(2).Text Then
                    index = i
                    Exit For
                End If
            Next
            If index <> -1 Then
                With Instances(index)
                    TabControl1.SelectedIndex = index
                    TabControl1.Select()
                    With .SyntaxHandle
                        If IsNumeric(line) Then
                            line -= 1
                            .GoTo.Line(line)
                            .Selection.Start = .Lines(line).SelectionStartPosition
                            .Selection.Length = .Lines(line).Text.Length
                        ElseIf line.IndexOf("--") > -1 Then
                            Dim i As Integer, tmp As String = vbNullString
                            While line(i) <> " "
                                tmp += line(i)
                                i += 1
                            End While
                            tmp -= 1
                            .GoTo.Line(tmp)
                            .Selection.Start = .Lines(tmp).SelectionStartPosition
                            .Selection.Length = .Lines(tmp).Text.Length
                        End If
                    End With
                End With
            Else
                If File.Exists(My.Application.Info.DirectoryPath & "\Include\" & ListView1.FocusedItem.SubItems(2).Text & ".inc") Then
                    Instances.Add(New Instance(ListView1.FocusedItem.SubItems(2).Text, Instances.Count))
                    With Instances(GetInstanceByName(ListView1.FocusedItem.SubItems(2).Text, Instances.Count - 1))
                        .Path = My.Application.Info.DirectoryPath & "\Include\" & .Name & ".inc"
                        TabControl1.SelectedTab = .TabHandle
                        Dim Reader As New StreamReader(.Path, System.Text.Encoding.GetEncoding(28591), True)
                        .SyntaxHandle.Text = Reader.ReadToEnd()
                        Reader.Close()
                        .SyntaxHandle.Focus()
                        If IsNumeric(line) Then
                            line -= 1
                            .SyntaxHandle.GoTo.Line(line)
                            .SyntaxHandle.Selection.Start = .SyntaxHandle.Lines(line).SelectionStartPosition
                            .SyntaxHandle.Selection.Length = .SyntaxHandle.Lines(line).Text.Length
                        ElseIf line.IndexOf("--") > -1 Then
                            Dim i As Integer, tmp As String = vbNullString
                            While line(i) <> " "
                                tmp += line(i)
                                i += 1
                            End While
                            tmp -= 1
                            .SyntaxHandle.GoTo.Line(tmp)
                            .SyntaxHandle.Selection.Start = .SyntaxHandle.Lines(tmp).SelectionStartPosition
                            .SyntaxHandle.Selection.Length = .SyntaxHandle.Lines(tmp).Text.Length
                        End If
                    End With
                End If
            End If
        End If
    End Sub

    Private Sub ContextMenuHandler(ByVal Sender As Object, ByVal e As EventArgs)
        On Error Resume Next
        Dim mI As MenuItem = DirectCast(Sender, MenuItem)
        Select Case mI.Text
            Case "Copy selected row"
                If Not ListView1.FocusedItem Is Nothing Then
                    Dim tmp As String
                    tmp = ListView1.FocusedItem.SubItems(1).Text & vbTab & ListView1.FocusedItem.SubItems(2).Text & vbTab & ListView1.FocusedItem.SubItems(3).Text & vbTab & """" & ListView1.FocusedItem.SubItems(4).Text & """"
                    Clipboard.SetText(tmp)
                End If
            Case "Copy all"
                Dim tmp As String = vbNullString
                For Each L As ListViewItem In Instances(TabControl1.SelectedIndex).Errors
                    tmp += L.SubItems(1).Text & vbTab & L.SubItems(2).Text & vbTab & L.SubItems(3).Text & vbTab & """" & L.SubItems(4).Text & """" & vbNewLine
                Next
                Clipboard.SetText(tmp)
            Case "Clear"
                Instances(TabControl1.SelectedIndex).Errors.Clear()
                ListView1.Items.Clear()
        End Select
    End Sub

#End Region

#Region "Drag & Drop (Files)"

    Private Sub MenuStrip1_DragEnter(sender As Object, e As System.Windows.Forms.DragEventArgs) Handles MenuStrip1.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then e.Effect = DragDropEffects.All
    End Sub

    Private Sub ToolStrip1_DragEnter(sender As Object, e As System.Windows.Forms.DragEventArgs) Handles ToolStrip1.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then e.Effect = DragDropEffects.All
    End Sub

    Private Sub TabControl1_DragEnter(sender As Object, e As System.Windows.Forms.DragEventArgs) Handles TabControl1.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then e.Effect = DragDropEffects.All
    End Sub

    Private Sub MenuStrip1_DragDrop(sender As Object, e As System.Windows.Forms.DragEventArgs) Handles MenuStrip1.DragDrop
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            For Each path As String In e.Data.GetData(DataFormats.FileDrop)
                Dim name As String = Mid(path, path.LastIndexOf("\") + 2, path.LastIndexOf(".") - path.LastIndexOf("\") - 1)
                Instances.Add(New Instance(name, Instances.Count))
                With Instances(GetInstanceByName(name, Instances.Count - 1))
                    TabControl1.SelectedTab = .TabHandle
                    Dim Reader As StreamReader
                    If path.EndsWith(".pwn") OrElse path.EndsWith(".inc") Then
                        Reader = New StreamReader(path, System.Text.Encoding.GetEncoding(28591), True)
                    Else
                        Reader = New StreamReader(path, System.Text.Encoding.UTF8, True)
                    End If
                    .SyntaxHandle.Text = Reader.ReadToEnd()
                    Reader.Close()
                    .Path = OFD.FileName
                End With
            Next
        End If
    End Sub

    Private Sub ToolStrip1_DragDrop(sender As Object, e As System.Windows.Forms.DragEventArgs) Handles ToolStrip1.DragDrop
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            For Each path As String In e.Data.GetData(DataFormats.FileDrop)
                Dim name As String = Mid(path, path.LastIndexOf("\") + 2, path.LastIndexOf(".") - path.LastIndexOf("\") - 1)
                Instances.Add(New Instance(name, Instances.Count))
                With Instances(GetInstanceByName(name, Instances.Count - 1))
                    TabControl1.SelectedTab = .TabHandle
                    Dim Reader As StreamReader
                    If path.EndsWith(".pwn") OrElse path.EndsWith(".inc") Then
                        Reader = New StreamReader(path, System.Text.Encoding.GetEncoding(28591), True)
                    Else
                        Reader = New StreamReader(path, System.Text.Encoding.UTF8, True)
                    End If
                    .SyntaxHandle.Text = Reader.ReadToEnd()
                    Reader.Close()
                    .Path = OFD.FileName
                End With
            Next
        End If
    End Sub

    Private Sub TabControl1_DragDrop(sender As Object, e As System.Windows.Forms.DragEventArgs) Handles TabControl1.DragDrop
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            For Each path As String In e.Data.GetData(DataFormats.FileDrop)
                Dim name As String = Mid(path, path.LastIndexOf("\") + 2, path.LastIndexOf(".") - path.LastIndexOf("\") - 1)
                Instances.Add(New Instance(name, Instances.Count))
                With Instances(GetInstanceByName(name, Instances.Count - 1))
                    TabControl1.SelectedTab = .TabHandle
                    Dim Reader As StreamReader
                    If path.EndsWith(".pwn") OrElse path.EndsWith(".inc") Then
                        Reader = New StreamReader(path, System.Text.Encoding.GetEncoding(28591), True)
                    Else
                        Reader = New StreamReader(path, System.Text.Encoding.UTF8, True)
                    End If
                    .SyntaxHandle.Text = Reader.ReadToEnd()
                    Reader.Close()
                    .Path = OFD.FileName
                End With
            Next
        End If
    End Sub

#End Region

End Class