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

Imports ScintillaNet

Public Class Instance

#Region "Arrays"

#Region "Private"

    Dim _Saved As Boolean
    Dim _Name As String
    Dim _Path As String
    Dim _Created As Boolean
    Dim _Ext As String
    Dim _Rate As Integer
    Dim _Font As Font = Settings.cFont
    Dim _ShowingInfoText As Boolean
    Dim wait As Boolean
    Dim first As Boolean
    Dim WithEvents Tim As New Timers.Timer(1000)
    Dim MarginUpdater As New uMargin(AddressOf UpdateMargin)

#End Region

#Region "Public"

    Public ACLists As AutoCompleteLists
    Public Errors As New List(Of ListViewItem)
    Public DataUpdater As New uData(AddressOf UpdateData)
    Public DataUpdaterEx As New uDataEx(AddressOf UpdateDataEx)

#End Region

#End Region

#Region "Enums"

    Public Enum UpdateType
        Includes
        Functions_Callbacks
        Colors
        Other
    End Enum

#End Region

#Region "Strutctures"

    Public Structure AutoCompleteLists
        Public Functions As List(Of PawnFunction)
        Public Callbacks As List(Of PawnFunction)
        Public Colors As List(Of PawnColor)
        Public eColors As List(Of PawnColor)
        Public Files As List(Of String)
        Public Dbs As List(Of String)
        Public DbRes As List(Of String)
        Public Menus As List(Of String)
        Public Texts As List(Of String)
        Public Texts2 As List(Of String)
        Public Floats As List(Of String)
    End Structure

#End Region

#Region "Components"

    Friend WithEvents TabHandle As TabPageEx.TabPageEx
    Friend WithEvents SyntaxHandle As Scintilla
    Friend WithEvents InfoText As RichTextBox

#End Region

#Region "Delegates"

#Region "Private"

    Private Delegate Function AddTreeNode(ByVal Name As String, ByVal Node As TreeNode) As TreeNode
    Private Delegate Function AddFirstTreeNode(ByVal Name As String, ByVal Tree As TreeView) As TreeNode
    Private Delegate Sub ClearTree(ByVal Tree As TreeView)
    Private Delegate Sub uMargin()
    Private Delegate Function SelectedLines(ByVal tControl As Scintilla) As ScintillaNet.LinesCollection
    Private Delegate Sub SetVisible(ByVal tControl As Control, ByVal value As Boolean)

#End Region

#Region "Public"

    Public Delegate Sub uDataEx(ByVal Type As UpdateType, ByVal startline As Integer, ByVal endline As Integer)
    Public Delegate Sub uData()

#End Region

#End Region

#Region "Properties"

    Public Property Saved As Boolean
        Get
            Saved = _Saved
        End Get
        Set(ByVal value As Boolean)
            _Saved = value
            Select Case Settings.Language
                Case Languages.English
                    Main.Label3.Text = "Document status: " & If(value = True, "saved", "not saved")
                Case Languages.Español
                    Main.Label3.Text = "Estado del documento: " & If(value = True, "guardado", "no guardado")
                Case Languages.Portuguêse
                    Main.Label3.Text = "Estado do Documento: " & If(value = True, "salvo", "não salvos")
                Case Else
                    Main.Label3.Text = "Document Status:" & If(value = True, "gerettet", "nicht gespeichert")
            End Select
            If value Then
                TabHandle.Text = _Name
            Else
                TabHandle.Text = _Name & " *"
            End If
        End Set
    End Property

    Public Property Name As String
        Get
            Name = _Name
        End Get
        Set(ByVal value As String)
            _Name = value
            TabHandle.Text = _Name
        End Set
    End Property

    Public Property Path As String
        Get
            Path = _Path
        End Get
        Set(ByVal value As String)
            _Path = value
            If _Ext Is Nothing OrElse _Ext.Length = 0 Then
                _Ext = Mid(_Path, _Path.LastIndexOf(".") + 2, _Path.Length - _Path.LastIndexOf(".") - 1)
            End If
        End Set
    End Property

    Public ReadOnly Property Created As Boolean
        Get
            Created = _Created
        End Get
    End Property

    Public ReadOnly Property CurrentFunction As String
        Get
            CurrentFunction = GetCurrentFunction(False, True)
        End Get
    End Property

    Public ReadOnly Property CurrentParamIndex As Integer
        Get
            CurrentParamIndex = GetCurrentParamIndex()
        End Get
    End Property

    Public Property Ext As String
        Get
            Ext = _Ext
        End Get
        Set(ByVal value As String)
            _Ext = value
        End Set
    End Property

    Public Property Font As Font
        Get
            Font = _Font
        End Get
        Set(ByVal value As Font)
            _Font = value
            SyntaxHandle.Font = value
            SyntaxHandle.Styles.Default.Font = value
            SyntaxHandle.Styles(0).Font = value
            SyntaxHandle.Styles(1).Font = value
            SyntaxHandle.Styles(2).Font = value
            SyntaxHandle.Styles(3).Font = value
            SyntaxHandle.Styles(4).Font = value
            SyntaxHandle.Styles(5).Font = value
            SyntaxHandle.Styles(6).Font = value
            SyntaxHandle.Styles(7).Font = value
            SyntaxHandle.Styles(8).Font = value
            SyntaxHandle.Styles(9).Font = value
            SyntaxHandle.Styles(10).Font = value
            SyntaxHandle.Styles(11).Font = value
            SyntaxHandle.Styles(12).Font = value
            SyntaxHandle.Styles(14).Font = value
            SyntaxHandle.Styles(19).Font = value
            SyntaxHandle.Styles(32).Font = value
            SyntaxHandle.Styles(ScintillaNet.StylesCommon.LineNumber).Font = value
            SyntaxHandle.Styles(ScintillaNet.StylesCommon.BraceBad).Font = value
            SyntaxHandle.Styles(ScintillaNet.StylesCommon.BraceLight).Font = value
            SyntaxHandle.Styles(ScintillaNet.StylesCommon.CallTip).Font = value
            SyntaxHandle.Styles(ScintillaNet.StylesCommon.ControlChar).Font = value
            SyntaxHandle.Styles(ScintillaNet.StylesCommon.Default).Font = value
            SyntaxHandle.Styles(ScintillaNet.StylesCommon.IndentGuide).Font = value
            SyntaxHandle.Styles(ScintillaNet.StylesCommon.LastPredefined).Font = value
            SyntaxHandle.Styles(ScintillaNet.StylesCommon.Max).Font = value
        End Set
    End Property

    Public Property ShowingInfoText As Boolean
        Get
            ShowingInfoText = _ShowingInfoText
        End Get
        Set(ByVal value As Boolean)
            _ShowingInfoText = value
            If InfoText.InvokeRequired Then
                Static VisibleDelegate As New SetVisible(AddressOf SetControlVisible)
                VisibleDelegate.Invoke(InfoText, value)
            Else
                InfoText.Visible = value
            End If
        End Set
    End Property

#End Region

#Region "Methods"

    Public Sub New(ByVal name As String, Optional ByVal iwait As Boolean = True)
        _Name = name
        TabHandle = New TabPageEx.TabPageEx()
        SyntaxHandle = New Scintilla()
        InfoText = New RichTextBox()
        With ACLists
            .Functions = New List(Of PawnFunction)
            .Callbacks = New List(Of PawnFunction)
            .Colors = New List(Of PawnColor)
            .eColors = New List(Of PawnColor)
            .Files = New List(Of String)
            .Dbs = New List(Of String)
            .DbRes = New List(Of String)
            .Menus = New List(Of String)
            .Texts = New List(Of String)
            .Texts2 = New List(Of String)
            .Floats = New List(Of String)
        End With
        With SyntaxHandle
            name = "SyntaxHandle" & If(Instances.Count = 0, "", Instances.Count)
            .Font = Settings.cFont
            .Dock = DockStyle.Fill
            .LineWrap.Mode = WrapMode.None
            .ConfigurationManager.Language = "pawn"
            .IsBraceMatching = True
            .AcceptsTab = True
            .MatchBraces = True
            .UndoRedo.IsUndoEnabled = False
            .Encoding = System.Text.Encoding.UTF8
            With .Margins
                .Margin0.Width = 18
                .Margin0.AutoToggleMarkerNumber = True
                .Margin1.IsFoldMargin = True
                .Margin1.IsClickable = True
                .Margin1.Width = 12
            End With
            With .AutoComplete
                .AutoHide = True
                .SingleLineAccept = True
                .IsCaseSensitive = True
                .DropRestOfWord = True
            End With
            With .Folding
                .IsEnabled = True
                .UseCompactFolding = True
                .MarkerScheme = FoldMarkerScheme.BoxPlusMinus
            End With
        End With
        With InfoText
            .Text = ""
            .BorderStyle = BorderStyle.None
            .Font = New Font(.Font.FontFamily, 10, FontStyle.Regular)
            .Enabled = False
            .Multiline = False
            .Visible = False
            .AutoSize = True
        End With
        With TabHandle
            .Text = name
            .Menu = New ContextMenu
            With .Menu.MenuItems
                .Add("Save", AddressOf SaveMenuItem_Click)
                .Add("Save As...", AddressOf SaveAsMenuItem_Click)
            End With
            With .Controls
                .Add(SyntaxHandle)
                .Add(InfoText)
            End With
        End With
        SetParent(SyntaxHandle.Handle, TabHandle.Handle)
        SetParent(InfoText.Handle, TabHandle.Handle)
        InfoText.BringToFront()
        Main.TabControl1.Controls.Add(TabHandle)
        If iwait = True Then
            wait = True
            first = True
        End If
        _Created = True
        Saved = True
    End Sub

    Public Sub destroy()
        If Not _Saved Then
            Dim result As MsgBoxResult
            Select Case Settings.Language
                Case Languages.English
                    result = MsgBox(String.Format("The file ""{0}"" has been changed. Do you want to save that changes?", _Name), vbYesNoCancel, "Alert")
                Case Languages.Español
                    result = MsgBox(String.Format("El archivo ""{0}"" ha sido cambiado. Deseas guardar esos cambios?", _Name), vbYesNoCancel, "Alerta")
                Case Languages.Portuguêse
                    result = MsgBox(String.Format("O arquivo""{0}""foi alterado. Você quer salvar o que muda?", _Name), vbYesNoCancel, "Alerta")
                Case Else
                    result = MsgBox(String.Format("Die Datei""{0}""geändert wurde. Wollen Sie, dass Änderungen speichern?", _Name), vbYesNoCancel, "Alert")
            End Select
            If result = vbYes Then
                If Not _Path Is Nothing AndAlso _Path.Length > 0 Then
                    Dim Writer As New StreamWriter(_Path)
                    Writer.Write(SyntaxHandle.Text)
                    Writer.Close()
                Else
                    With Main.SFD
                        .InitialDirectory = Settings.DefaultPath
                        .ShowDialog()
                        If Not .FileName Is Nothing AndAlso .FileName.Length > 0 Then
                            Dim Writer As New StreamWriter(Main.SFD.FileName)
                            Writer.Write(SyntaxHandle.Text)
                            Writer.Close()
                        End If
                    End With
                End If
            ElseIf result = vbCancel Then
                Exit Sub
            End If
        End If
        Tim.Enabled = False
        _Created = False
    End Sub

#End Region

#Region "Functions"

    Public Function GetLineByText(ByVal Text As String) As Integer
        For Each L As ScintillaNet.Line In SyntaxHandle.Lines
            If L.Text.IndexOf(Text) > -1 Then Return L.Number
        Next
        Return 0
    End Function

#End Region

#Region "Internal"

#Region "Events"

    Private Sub SyntaxHandle_AutoCompleteAccepted(ByVal sender As Object, ByVal e As ScintillaNet.AutoCompleteAcceptedEventArgs) Handles SyntaxHandle.AutoCompleteAccepted
        If SyntaxHandle.AutoComplete.List Is Lists.PreCompiler Then SyntaxHandle.Selection.Text = " "
        If _ShowingInfoText Then
            With InfoText
                .Clear()
                Dim istart As Integer, iend As Integer, func As PawnFunction
                func = GetFunctionByName(ACLists.Functions, GetCurrentFunction())
                If func.Name.Length > 0 Then
                    For Each param As String In ACLists.Functions(ACLists.Functions.IndexOf(func)).Params
                        If Not ACLists.Functions(ACLists.Functions.IndexOf(func)).Params(UBound(ACLists.Functions(ACLists.Functions.IndexOf(func)).Params)) = param Then
                            If Array.IndexOf(ACLists.Functions(ACLists.Functions.IndexOf(func)).Params, param) = GetCurrentParamIndex() Then
                                istart = .Text.Length
                                iend = istart + Len(param + ", ")
                            End If
                            .Text += param & ", "
                        Else
                            If Array.IndexOf(ACLists.Functions(ACLists.Functions.IndexOf(func)).Params, param) = GetCurrentParamIndex() Then
                                istart = .Text.Length
                                iend = istart + Len(param)
                            End If
                            .Text += param
                        End If
                    Next
                    .SelectionStart = istart
                    .SelectionLength = iend - istart
                    .SelectionFont = New Font(.Font.FontFamily, 10)
                    .SelectionColor = Color.Blue
                    If SyntaxHandle.PointXFromPosition(SyntaxHandle.CurrentPos) + .Size.Width < SyntaxHandle.Width - 20 Then
                        .Location = New Point(SyntaxHandle.PointXFromPosition(SyntaxHandle.CurrentPos), SyntaxHandle.PointYFromPosition(SyntaxHandle.CurrentPos) + 20)
                    Else
                        .Location = New Point(SyntaxHandle.Width - .Size.Width, SyntaxHandle.PointYFromPosition(SyntaxHandle.CurrentPos) + 20)
                    End If
                    .Visible = True
                End If
            End With
        End If
    End Sub

    Private Sub SyntaxHandle_CharAdded(ByVal sender As Object, ByVal e As ScintillaNet.CharAddedEventArgs) Handles SyntaxHandle.CharAdded
        Select Case e.Ch
            Case "#"
                Dim CommentedChar As Boolean, pos As Integer
                CommentedChar = False
                pos = GetLineCursorPosition()
                For i = 0 To SyntaxHandle.Lines.Current.Number
                    If SyntaxHandle.Lines(i).Text.IndexOf("/*") > -1 AndAlso SyntaxHandle.Lines(i).Text.IndexOf("*/") = -1 Then
                        CommentedChar = True
                    ElseIf CommentedChar AndAlso SyntaxHandle.Lines(i).Length > 0 AndAlso SyntaxHandle.Lines(i).Text.IndexOf("*/") > -1 Then
                        CommentedChar = False
                    End If
                Next
                If CommentedChar AndAlso SyntaxHandle.Lines.Current.Text.Length > 0 AndAlso SyntaxHandle.Lines.Current.Text.IndexOf("*/") > -1 AndAlso SyntaxHandle.Lines.Current.Text.IndexOf("*/") < pos Then
                    CommentedChar = False
                End If
                If SyntaxHandle.Lines.Current.Text.Length > 0 AndAlso (SyntaxHandle.Lines.Current.Text.IndexOf("//") > -1 AndAlso SyntaxHandle.Lines.Current.Text.IndexOf("//") < pos) Then
                    CommentedChar = True
                End If
                If CommentedChar Then Exit Sub
                With SyntaxHandle.AutoComplete
                    .List.Clear()
                    For Each i In Lists.PreCompiler
                        .List.Add(i)
                    Next
                    .Show()
                    If _ShowingInfoText Then InfoText.Visible = False
                End With
            Case "("
                Dim CommentedChar As Boolean, pos As Integer
                CommentedChar = False
                pos = GetLineCursorPosition()
                For i = 0 To SyntaxHandle.Lines.Current.Number
                    If SyntaxHandle.Lines(i).Text.IndexOf("/*") > -1 AndAlso SyntaxHandle.Lines(i).Text.IndexOf("*/") = -1 Then
                        CommentedChar = True
                    ElseIf CommentedChar AndAlso SyntaxHandle.Lines(i).Length > 0 AndAlso SyntaxHandle.Lines(i).Text.IndexOf("*/") > -1 Then
                        CommentedChar = False
                    End If
                Next
                If CommentedChar AndAlso SyntaxHandle.Lines.Current.Text.Length > 0 AndAlso SyntaxHandle.Lines.Current.Text.IndexOf("*/") > -1 AndAlso SyntaxHandle.Lines.Current.Text.IndexOf("*/") < pos Then
                    CommentedChar = False
                End If
                If SyntaxHandle.Lines.Current.Text.Length > 0 AndAlso (SyntaxHandle.Lines.Current.Text.IndexOf("//") > -1 AndAlso SyntaxHandle.Lines.Current.Text.IndexOf("//") < pos) Then
                    CommentedChar = True
                End If
                If CommentedChar Then Exit Sub
                Dim func As PawnFunction = GetFunctionByName(ACLists.Functions, GetCurrentFunction(False, True))
                If TrueContainsFunction(ACLists.Functions, func) Then
                    If GetCurrentParamIndex() < func.Params.Length Then
                        If func.Params(0).IndexOf("color") > -1 AndAlso ACLists.Colors.Count Then
                            Select Case func.Name
                                Case Not "AddStaticVehicle", "AddStaticVehicleEx", "CreateVehicle", "ChangeVehicleColor"
                                    With SyntaxHandle.AutoComplete
                                        .List.Clear()
                                        For Each c In ACLists.Colors
                                            If Not .List.Contains(c.Name) Then
                                                .List.Add(c.Name)
                                            End If
                                        Next
                                        If ACLists.Colors.Count = 1 Then .List.Add("-")
                                        If _ShowingInfoText Then InfoText.Visible = False
                                        .Show()
                                    End With
                            End Select
                        ElseIf func.Params(0).IndexOf("DB:") > -1 AndAlso ACLists.Dbs.Count > 0 Then
                            With SyntaxHandle.AutoComplete
                                .List.Clear()
                                For Each d In ACLists.Dbs
                                    .List.Add(d)
                                Next
                                If ACLists.Dbs.Count = 1 Then .List.Add("-")
                                If _ShowingInfoText Then InfoText.Visible = False
                                .Show()
                            End With
                        ElseIf func.Params(0).IndexOf("DBResult:") > -1 AndAlso ACLists.DbRes.Count > 0 Then
                            With SyntaxHandle.AutoComplete
                                .List.Clear()
                                For Each d In ACLists.DbRes
                                    .List.Add(d)
                                Next
                                If ACLists.DbRes.Count = 1 Then .List.Add("-")
                                If _ShowingInfoText Then InfoText.Visible = False
                                .Show()
                            End With
                        ElseIf func.Params(0).IndexOf("Menu:") > -1 AndAlso ACLists.Menus.Count > 0 Then
                            With SyntaxHandle.AutoComplete
                                .List.Clear()
                                For Each m In ACLists.Menus
                                    .List.Add(m)
                                Next
                                If ACLists.Menus.Count = 1 Then .List.Add("-")
                                If _ShowingInfoText Then InfoText.Visible = False
                                .Show()
                            End With
                        ElseIf func.Params(0).IndexOf("Text:") > -1 AndAlso ACLists.Texts.Count > 0 Then
                            With SyntaxHandle.AutoComplete
                                .List.Clear()
                                For Each t In ACLists.Texts
                                    .List.Add(t)
                                Next
                                If ACLists.Texts.Count = 1 Then .List.Add("-")
                                If _ShowingInfoText Then InfoText.Visible = False
                                .Show()
                            End With
                        ElseIf func.Params(0).IndexOf("Text3D:") > -1 AndAlso ACLists.Texts2.Count > 0 Then
                            With SyntaxHandle.AutoComplete
                                .List.Clear()
                                For Each t In ACLists.Texts2
                                    .List.Add(t)
                                Next
                                If ACLists.Texts2.Count = 1 Then .List.Add("-")
                                If _ShowingInfoText Then InfoText.Visible = False
                                .Show()
                            End With
                        ElseIf func.Params(0).IndexOf("Float:") > -1 AndAlso ACLists.Floats.Count > 0 Then
                            With SyntaxHandle.AutoComplete
                                .List.Clear()
                                For Each f In ACLists.Floats
                                    .List.Add(f)
                                Next
                                If ACLists.Floats.Count = 1 Then .List.Add("-")
                                If _ShowingInfoText Then InfoText.Visible = False
                                .Show()
                            End With
                        Else
                            Select Case Trim(func.Params(0))
                                Case "weapon", "Weapon"
                                    With SyntaxHandle.AutoComplete
                                        .List.Clear()
                                        For Each w As Weap In Weapons
                                            .List.Add(w.Def)
                                        Next
                                        If _ShowingInfoText Then InfoText.Visible = False
                                        .Show()
                                    End With
                                Case "style", "Style"
                                    Select Case GetCurrentFunction()
                                        Case "SetPlayerFightingStyle"
                                            With SyntaxHandle.AutoComplete
                                                .List.Clear()
                                                For Each i In Lists.FightingTypes
                                                    .List.Add(i)
                                                Next
                                                If _ShowingInfoText Then InfoText.Visible = False
                                                .Show()
                                            End With
                                        Case "GameTextForAll", "GameTextForPlayer"
                                            With SyntaxHandle.AutoComplete
                                                .List.Clear()
                                                For i = 0 To 6
                                                    .List.Add(i)
                                                Next
                                                If _ShowingInfoText Then InfoText.Visible = False
                                                .Show()
                                            End With
                                        Case "ShowPlayerDialog"
                                            With SyntaxHandle.AutoComplete
                                                .List.Clear()
                                                For Each i In Lists.DialogTypes
                                                    .List.Add(i)
                                                Next
                                                If _ShowingInfoText Then InfoText.Visible = False
                                                .Show()
                                            End With
                                    End Select
                                Case "callback", "Callback"
                                    With SyntaxHandle.AutoComplete
                                        .List.Clear()
                                        For Each i In ACLists.Callbacks
                                            .List.Add(i.Name)
                                        Next
                                        If _ShowingInfoText Then InfoText.Visible = False
                                        .Show()
                                    End With
                                Case "mode", "Mode"
                                    Select Case func.Name
                                        Case "PlayerSpectatePlayer", "PlayerSpectateVehicle"
                                            With SyntaxHandle.AutoComplete
                                                .List.Clear()
                                                For Each i In Lists.SpecTypes
                                                    .List.Add(i)
                                                Next
                                                If _ShowingInfoText Then InfoText.Visible = False
                                                .Show()
                                            End With
                                        Case "ShowPlayerMarkers"
                                            With SyntaxHandle.AutoComplete
                                                .List.Clear()
                                                For Each i In Lists.MarkerTypes
                                                    .List.Add(i)
                                                Next
                                                If _ShowingInfoText Then InfoText.Visible = False
                                                .Show()
                                            End With
                                        Case "fopen"
                                            With SyntaxHandle.AutoComplete
                                                .List.Clear()
                                                For Each i In Lists.FileTypes
                                                    .List.Add(i)
                                                Next
                                                If _ShowingInfoText Then InfoText.Visible = False
                                                .Show()
                                            End With
                                    End Select
                                Case "actionid"
                                    If func.Name = "SetPlayerSpecialAction" Then
                                        With SyntaxHandle.AutoComplete
                                            .List.Clear()
                                            For Each i In Lists.ActionTypes
                                                .List.Add(i)
                                            Next
                                            If _ShowingInfoText Then InfoText.Visible = False
                                            .Show()
                                        End With
                                    End If
                                Case "recordtype"
                                    If func.Name = "StartRecordingPlayerData" Then
                                        With SyntaxHandle.AutoComplete
                                            .List.Clear()
                                            For Each i In Lists.RecordingTypes
                                                .List.Add(i)
                                            Next
                                            If _ShowingInfoText Then InfoText.Visible = False
                                            .Show()
                                        End With
                                    End If
                                Case "method"
                                    If func.Name = "floatround" Then
                                        With SyntaxHandle.AutoComplete
                                            .List.Clear()
                                            For Each i In Lists.RoundTypes
                                                .List.Add(i)
                                            Next
                                            If _ShowingInfoText Then InfoText.Visible = False
                                            .Show()
                                        End With
                                    End If
                                Case "anglemode"
                                    If func.Name = "floatsin" Then
                                        With SyntaxHandle.AutoComplete
                                            .List.Clear()
                                            For Each i In Lists.AngleTypes
                                                .List.Add(i)
                                            Next
                                            If _ShowingInfoText Then InfoText.Visible = False
                                            .Show()
                                        End With
                                    End If
                                Case "whence"
                                    If func.Name = "fseek" Then
                                        With SyntaxHandle.AutoComplete
                                            .List.Clear()
                                            For Each i In Lists.WhenceTypes
                                                .List.Add(i)
                                            Next
                                            If _ShowingInfoText Then InfoText.Visible = False
                                            .Show()
                                        End With
                                    End If
                                Case Else
                                    With InfoText
                                        If _ShowingInfoText Then
                                            .Clear()
                                            If func.Params.Length = 0 Then InfoText.Visible = False
                                            Dim istart As Integer, iend As Integer
                                            For Each param As String In ACLists.Functions(ACLists.Functions.IndexOf(func)).Params
                                                If Not ACLists.Functions(ACLists.Functions.IndexOf(func)).Params(UBound(ACLists.Functions(ACLists.Functions.IndexOf(func)).Params)) = param Then
                                                    If Array.IndexOf(ACLists.Functions(ACLists.Functions.IndexOf(func)).Params, param) = 0 Then
                                                        istart = .Text.Length
                                                        iend = istart + Len(param + ", ")
                                                    End If
                                                    .Text += param & ", "
                                                Else
                                                    If Array.IndexOf(ACLists.Functions(ACLists.Functions.IndexOf(func)).Params, param) = 0 Then
                                                        istart = .Text.Length
                                                        iend = istart + Len(param)
                                                    End If
                                                    .Text += param
                                                End If
                                            Next
                                            .SelectionStart = istart
                                            .SelectionLength = iend - istart
                                            .SelectionFont = New Font(.Font.FontFamily, 10)
                                            .SelectionColor = Color.Blue
                                            .Location = New Point(SyntaxHandle.PointXFromPosition(SyntaxHandle.CurrentPos), SyntaxHandle.PointYFromPosition(SyntaxHandle.CurrentPos) + 20)
                                        Else
                                            Tim.Enabled = True
                                        End If
                                    End With
                            End Select
                        End If
                    End If
                ElseIf TrueContainsFunction(ACLists.Callbacks, func) Then
                    Dim tmp As String = vbNullString
                    For Each item In ACLists.Callbacks(ACLists.Callbacks.IndexOf(func)).Params
                        If tmp.Length > 0 Then
                            tmp += ", " & item
                        Else
                            tmp = item
                        End If
                    Next
                    SyntaxHandle.Selection.Text = tmp & ")"
                End If
            Case ","
                Dim CommentedChar As Boolean, pos As Integer
                CommentedChar = False
                pos = GetLineCursorPosition()
                For i = 0 To SyntaxHandle.Lines.Current.Number
                    If SyntaxHandle.Lines(i).Text.IndexOf("/*") > -1 AndAlso SyntaxHandle.Lines(i).Text.IndexOf("*/") = -1 Then
                        CommentedChar = True
                    ElseIf CommentedChar AndAlso SyntaxHandle.Lines(i).Length > 0 AndAlso SyntaxHandle.Lines(i).Text.IndexOf("*/") > -1 Then
                        CommentedChar = False
                    End If
                Next
                If CommentedChar AndAlso SyntaxHandle.Lines.Current.Text.Length > 0 AndAlso SyntaxHandle.Lines.Current.Text.IndexOf("*/") > -1 AndAlso SyntaxHandle.Lines.Current.Text.IndexOf("*/") < pos Then
                    CommentedChar = False
                End If
                If SyntaxHandle.Lines.Current.Text.Length > 0 AndAlso (SyntaxHandle.Lines.Current.Text.IndexOf("//") > -1 AndAlso SyntaxHandle.Lines.Current.Text.IndexOf("//") < pos) Then
                    CommentedChar = True
                End If
                If CommentedChar Then Exit Sub
                Dim func As PawnFunction = GetFunctionByName(ACLists.Functions, GetCurrentFunction(False, True))
                Dim index As Integer = GetCurrentParamIndex()
                If Settings.aSelect AndAlso func.Name = "SetPlayerSkin" AndAlso index = 1 Then
                    With Main
                        If Not .TabControl2.SelectedTab Is .TabPage1 Then
                            .TabControl2.SelectedTab = .TabPage1
                            .ComboBox1.SelectedIndex = 1
                            .ComboBox1.SelectedIndex = 0
                        Else
                            .PictureBox1.Enabled = True
                            .ComboBox1.Enabled = True
                            .Button3.Enabled = True
                            If .PictureBox1.Image Is My.Resources.N_A Then .ComboBox1.SelectedIndex = 0
                        End If
                    End With
                    Exit Sub
                End If
                If TrueContainsFunction(ACLists.Functions, func) Then
                    If index > -1 AndAlso index < func.Params.Length Then
                        If func.Params(index).IndexOf("color") > -1 AndAlso ACLists.Colors.Count Then
                            Select Case func.Name
                                Case "AddStaticVehicle", "AddStaticVehicleEx", "CreateVehicle", "ChangeVehicleColor"
                                Case Else
                                    With SyntaxHandle.AutoComplete
                                        .List.Clear()
                                        For Each c In ACLists.Colors
                                            If Not .List.Contains(c.Name) Then
                                                .List.Add(c.Name)
                                            End If
                                        Next
                                        If ACLists.Colors.Count = 1 Then .List.Add("-")
                                        .Show()
                                        If _ShowingInfoText Then InfoText.Visible = False
                                    End With
                            End Select
                        ElseIf func.Params(index).IndexOf("DB:") > -1 AndAlso ACLists.Dbs.Count > 0 Then
                            With SyntaxHandle.AutoComplete
                                .List.Clear()
                                For Each d In ACLists.Dbs
                                    .List.Add(d)
                                Next
                                If ACLists.Dbs.Count = 1 Then .List.Add("-")
                                If _ShowingInfoText Then InfoText.Visible = False
                                .Show()
                            End With
                        ElseIf func.Params(index).IndexOf("DBResult:") > -1 AndAlso ACLists.DbRes.Count > 0 Then
                            With SyntaxHandle.AutoComplete
                                .List.Clear()
                                For Each d In ACLists.DbRes
                                    .List.Add(d)
                                Next
                                If ACLists.DbRes.Count = 1 Then .List.Add("-")
                                If _ShowingInfoText Then InfoText.Visible = False
                                .Show()
                            End With
                        ElseIf func.Params(index).IndexOf("Menu:") > -1 AndAlso ACLists.Menus.Count > 0 Then
                            With SyntaxHandle.AutoComplete
                                .List.Clear()
                                For Each m In ACLists.Menus
                                    .List.Add(m)
                                Next
                                If ACLists.Menus.Count = 1 Then .List.Add("-")
                                If _ShowingInfoText Then InfoText.Visible = False
                                .Show()
                            End With
                        ElseIf func.Params(index).IndexOf("Text:") > -1 AndAlso ACLists.Texts.Count > 0 Then
                            With SyntaxHandle.AutoComplete
                                .List.Clear()
                                For Each t In ACLists.Texts
                                    .List.Add(t)
                                Next
                                If ACLists.Texts.Count = 1 Then .List.Add("-")
                                If _ShowingInfoText Then InfoText.Visible = False
                                .Show()
                            End With
                        ElseIf func.Params(index).IndexOf("Text3D:") > -1 AndAlso ACLists.Texts2.Count > 0 Then
                            With SyntaxHandle.AutoComplete
                                .List.Clear()
                                For Each t In ACLists.Texts2
                                    .List.Add(t)
                                Next
                                If ACLists.Texts2.Count = 1 Then .List.Add("-")
                                If _ShowingInfoText Then InfoText.Visible = False
                                .Show()
                            End With
                        ElseIf func.Params(index).IndexOf("Float:") > -1 AndAlso ACLists.Floats.Count > 0 Then
                            With SyntaxHandle.AutoComplete
                                .List.Clear()
                                For Each f In ACLists.Floats
                                    .List.Add(f)
                                Next
                                If ACLists.Floats.Count = 1 Then .List.Add("-")
                                If _ShowingInfoText Then InfoText.Visible = False
                                .Show()
                            End With
                        Else
                            Select Case Trim(func.Params(index))
                                Case "weapon", "Weapon", "weaponid", "Weaponid", "weaponID", "WeaponID", "weaponId", "WeaponId"
                                    With SyntaxHandle.AutoComplete
                                        .List.Clear()
                                        For Each w As Weap In Weapons
                                            .List.Add(w.Def)
                                        Next
                                        If _ShowingInfoText Then InfoText.Visible = False
                                        .Show()
                                    End With
                                Case "style", "Style"
                                    Select Case func.Name
                                        Case "SetPlayerFightingStyle"
                                            With SyntaxHandle.AutoComplete
                                                .List.Clear()
                                                For Each i In Lists.FightingTypes
                                                    .List.Add(i)
                                                Next
                                                If _ShowingInfoText Then InfoText.Visible = False
                                                .Show()
                                            End With
                                        Case "GameTextForAll", "GameTextForPlayer"
                                            With SyntaxHandle.AutoComplete
                                                .List.Clear()
                                                For i = 0 To 6
                                                    .List.Add(i)
                                                Next
                                                If _ShowingInfoText Then InfoText.Visible = False
                                                .Show()
                                            End With
                                        Case "ShowPlayerDialog"
                                            With SyntaxHandle.AutoComplete
                                                .List.Clear()
                                                For Each i In Lists.DialogTypes
                                                    .List.Add(i)
                                                Next
                                                If _ShowingInfoText Then InfoText.Visible = False
                                                .Show()
                                            End With
                                    End Select
                                Case "callback", "Callback"
                                    With SyntaxHandle.AutoComplete
                                        .List.Clear()
                                        For Each i In ACLists.Callbacks
                                            .List.Add(i.Name)
                                        Next
                                        If _ShowingInfoText Then InfoText.Visible = False
                                        .Show()
                                    End With
                                Case "mode", "Mode"
                                    Select Case func.Name
                                        Case "PlayerSpectatePlayer", "PlayerSpectateVehicle"
                                            With SyntaxHandle.AutoComplete
                                                .List.Clear()
                                                For Each i In Lists.SpecTypes
                                                    .List.Add(i)
                                                Next
                                                If _ShowingInfoText Then InfoText.Visible = False
                                                .Show()
                                            End With
                                        Case "ShowPlayerMarkers"
                                            With SyntaxHandle.AutoComplete
                                                .List.Clear()
                                                For Each i In Lists.MarkerTypes
                                                    .List.Add(i)
                                                Next
                                                If _ShowingInfoText Then InfoText.Visible = False
                                                .Show()
                                            End With
                                        Case "fopen"
                                            With SyntaxHandle.AutoComplete
                                                .List.Clear()
                                                For Each i In Lists.FileTypes
                                                    .List.Add(i)
                                                Next
                                                If _ShowingInfoText Then InfoText.Visible = False
                                                .Show()
                                            End With
                                    End Select
                                Case "actionid"
                                    If func.Name = "SetPlayerSpecialAction" Then
                                        With SyntaxHandle.AutoComplete
                                            .List.Clear()
                                            For Each i In Lists.ActionTypes
                                                .List.Add(i)
                                            Next
                                            If _ShowingInfoText Then InfoText.Visible = False
                                            .Show()
                                        End With
                                    End If
                                Case "recordtype"
                                    If func.Name = "StartRecordingPlayerData" Then
                                        With SyntaxHandle.AutoComplete
                                            .List.Clear()
                                            For Each i In Lists.RecordingTypes
                                                .List.Add(i)
                                            Next
                                            If _ShowingInfoText Then InfoText.Visible = False
                                            .Show()
                                        End With
                                    End If
                                Case "method"
                                    If func.Name = "floatround" Then
                                        With SyntaxHandle.AutoComplete
                                            .List.Clear()
                                            For Each i In Lists.RoundTypes
                                                .List.Add(i)
                                            Next
                                            If _ShowingInfoText Then InfoText.Visible = False
                                            .Show()
                                        End With
                                    End If
                                Case "anglemode"
                                    If func.Name = "floatsin" Then
                                        With SyntaxHandle.AutoComplete
                                            .List.Clear()
                                            For Each i In Lists.AngleTypes
                                                .List.Add(i)
                                            Next
                                            If _ShowingInfoText Then InfoText.Visible = False
                                            .Show()
                                        End With
                                    End If
                                Case "whence"
                                    If func.Name = "fseek" Then
                                        With SyntaxHandle.AutoComplete
                                            .List.Clear()
                                            For Each i In Lists.WhenceTypes
                                                .List.Add(i)
                                            Next
                                            If _ShowingInfoText Then InfoText.Visible = False
                                            .Show()
                                        End With
                                    End If
                                Case Else
                                    With InfoText
                                        If _ShowingInfoText Then
                                            .Clear()
                                            Dim istart As Integer, iend As Integer
                                            For Each param As String In ACLists.Functions(ACLists.Functions.IndexOf(func)).Params
                                                If Not ACLists.Functions(ACLists.Functions.IndexOf(func)).Params(UBound(ACLists.Functions(ACLists.Functions.IndexOf(func)).Params)) = param Then
                                                    If Array.IndexOf(ACLists.Functions(ACLists.Functions.IndexOf(func)).Params, param) = index Then
                                                        istart = .Text.Length
                                                        iend = istart + Len(param + ", ")
                                                    End If
                                                    .Text += param & ", "
                                                Else
                                                    If Array.IndexOf(ACLists.Functions(ACLists.Functions.IndexOf(func)).Params, param) = index Then
                                                        istart = .Text.Length
                                                        iend = istart + Len(param)
                                                    End If
                                                    .Text += param
                                                End If
                                            Next
                                            .SelectionStart = istart
                                            .SelectionLength = iend - istart
                                            .SelectionFont = New Font(.Font.FontFamily, 10)
                                            .SelectionColor = Color.Blue
                                            If SyntaxHandle.PointXFromPosition(SyntaxHandle.CurrentPos) + .Size.Width < SyntaxHandle.Width - 20 Then
                                                .Location = New Point(SyntaxHandle.PointXFromPosition(SyntaxHandle.CurrentPos), SyntaxHandle.PointYFromPosition(SyntaxHandle.CurrentPos) + 20)
                                            Else
                                                .Location = New Point(SyntaxHandle.Width - .Size.Width, SyntaxHandle.PointYFromPosition(SyntaxHandle.CurrentPos) + 20)
                                            End If
                                        Else
                                            Tim.Enabled = True
                                        End If
                                    End With
                            End Select
                        End If
                    Else
                        index = UBound(func.Params)
                        If func.Params(index).IndexOf("...") > -1 Then
                            With InfoText
                                If _ShowingInfoText Then
                                    .Clear()
                                    Dim istart As Integer, iend As Integer
                                    For Each param As String In ACLists.Functions(ACLists.Functions.IndexOf(func)).Params
                                        If Not ACLists.Functions(ACLists.Functions.IndexOf(func)).Params(UBound(ACLists.Functions(ACLists.Functions.IndexOf(func)).Params)) = param Then
                                            If Array.IndexOf(ACLists.Functions(ACLists.Functions.IndexOf(func)).Params, param) = index Then
                                                istart = .Text.Length
                                                iend = istart + Len(param + ", ")
                                            End If
                                            .Text += param & ", "
                                        Else
                                            If Array.IndexOf(ACLists.Functions(ACLists.Functions.IndexOf(func)).Params, param) = index Then
                                                istart = .Text.Length
                                                iend = istart + Len(param)
                                            End If
                                            .Text += param
                                        End If
                                    Next
                                    .SelectionStart = istart
                                    .SelectionLength = iend - istart
                                    .SelectionFont = New Font(.Font.FontFamily, 10)
                                    .SelectionColor = Color.Blue
                                    If SyntaxHandle.PointXFromPosition(SyntaxHandle.CurrentPos) + .Size.Width < SyntaxHandle.Width - 20 Then
                                        .Location = New Point(SyntaxHandle.PointXFromPosition(SyntaxHandle.CurrentPos), SyntaxHandle.PointYFromPosition(SyntaxHandle.CurrentPos) + 20)
                                    Else
                                        .Location = New Point(SyntaxHandle.Width - .Size.Width, SyntaxHandle.PointYFromPosition(SyntaxHandle.CurrentPos) + 20)
                                    End If
                                End If
                            End With
                        Else
                            With InfoText
                                If _ShowingInfoText Then
                                    .Clear()
                                    For Each param As String In ACLists.Functions(ACLists.Functions.IndexOf(func)).Params
                                        If Not ACLists.Functions(ACLists.Functions.IndexOf(func)).Params(UBound(ACLists.Functions(ACLists.Functions.IndexOf(func)).Params)) = param Then
                                            .Text += param & ", "
                                        Else
                                            .Text += param
                                        End If
                                    Next
                                    If SyntaxHandle.PointXFromPosition(SyntaxHandle.CurrentPos) + .Size.Width < SyntaxHandle.Width - 20 Then
                                        .Location = New Point(SyntaxHandle.PointXFromPosition(SyntaxHandle.CurrentPos), SyntaxHandle.PointYFromPosition(SyntaxHandle.CurrentPos) + 20)
                                    Else
                                        .Location = New Point(SyntaxHandle.Width - .Size.Width, SyntaxHandle.PointYFromPosition(SyntaxHandle.CurrentPos) + 20)
                                    End If
                                End If
                            End With
                        End If
                    End If
                ElseIf TrueContainsFunction(ACLists.Callbacks, func) Then
                    Dim tmp As String = vbNullString
                    For Each item In ACLists.Callbacks(ACLists.Callbacks.IndexOf(func)).Params
                        If tmp.Length > 0 Then
                            tmp += ", " & item
                        Else
                            tmp = item
                        End If
                    Next
                    SyntaxHandle.Selection.Text = tmp & ")"
                End If
            Case ")"
                Dim CommentedChar As Boolean, pos As Integer
                CommentedChar = False
                pos = GetLineCursorPosition()
                For i = 0 To SyntaxHandle.Lines.Current.Number
                    If SyntaxHandle.Lines(i).Text.IndexOf("/*") > -1 AndAlso SyntaxHandle.Lines(i).Text.IndexOf("*/") = -1 Then
                        CommentedChar = True
                    ElseIf CommentedChar AndAlso SyntaxHandle.Lines(i).Length > 0 AndAlso SyntaxHandle.Lines(i).Text.IndexOf("*/") > -1 Then
                        CommentedChar = False
                    End If
                Next
                If CommentedChar AndAlso SyntaxHandle.Lines.Current.Text.Length > 0 AndAlso SyntaxHandle.Lines.Current.Text.IndexOf("*/") > -1 AndAlso SyntaxHandle.Lines.Current.Text.IndexOf("*/") < pos Then
                    CommentedChar = False
                End If
                If SyntaxHandle.Lines.Current.Text.Length > 0 AndAlso (SyntaxHandle.Lines.Current.Text.IndexOf("//") > -1 AndAlso SyntaxHandle.Lines.Current.Text.IndexOf("//") < pos) Then
                    CommentedChar = True
                End If
                If CommentedChar Then Exit Sub
                Dim func As PawnFunction = GetFunctionByName(ACLists.Functions, GetCurrentFunction(False, True))
                If func.Name.Length = 0 Then : ShowingInfoText = False
                Else
                    With InfoText
                        If _ShowingInfoText Then
                            .Clear()
                            Dim istart As Integer, iend As Integer
                            For Each param As String In ACLists.Functions(ACLists.Functions.IndexOf(func)).Params
                                If Not ACLists.Functions(ACLists.Functions.IndexOf(func)).Params(UBound(ACLists.Functions(ACLists.Functions.IndexOf(func)).Params)) = param Then
                                    If Array.IndexOf(ACLists.Functions(ACLists.Functions.IndexOf(func)).Params, param) = GetCurrentParamIndex() Then
                                        istart = .Text.Length
                                        iend = istart + Len(param + ", ")
                                    End If
                                    .Text += param & ", "
                                Else
                                    If Array.IndexOf(ACLists.Functions(ACLists.Functions.IndexOf(func)).Params, param) = GetCurrentParamIndex() Then
                                        istart = .Text.Length
                                        iend = istart + Len(param)
                                    End If
                                    .Text += param
                                End If
                            Next
                            .SelectionStart = istart
                            .SelectionLength = iend - istart
                            .SelectionFont = New Font(.Font.FontFamily, 10)
                            .SelectionColor = Color.Blue
                            If SyntaxHandle.PointXFromPosition(SyntaxHandle.CurrentPos) + .Size.Width < SyntaxHandle.Width - 20 Then
                                .Location = New Point(SyntaxHandle.PointXFromPosition(SyntaxHandle.CurrentPos), SyntaxHandle.PointYFromPosition(SyntaxHandle.CurrentPos) + 20)
                            Else
                                .Location = New Point(SyntaxHandle.Width - .Size.Width, SyntaxHandle.PointYFromPosition(SyntaxHandle.CurrentPos) + 20)
                            End If
                        End If
                    End With
                End If
            Case "{"
                Dim CommentedChar As Boolean, pos As Integer
                CommentedChar = False
                pos = GetLineCursorPosition()
                For i = 0 To SyntaxHandle.Lines.Current.Number
                    If SyntaxHandle.Lines(i).Text.IndexOf("/*") > -1 AndAlso SyntaxHandle.Lines(i).Text.IndexOf("*/") = -1 Then
                        CommentedChar = True
                    ElseIf CommentedChar AndAlso SyntaxHandle.Lines(i).Length > 0 AndAlso SyntaxHandle.Lines(i).Text.IndexOf("*/") > -1 Then
                        CommentedChar = False
                    End If
                Next
                If CommentedChar AndAlso SyntaxHandle.Lines.Current.Text.Length > 0 AndAlso SyntaxHandle.Lines.Current.Text.IndexOf("*/") > -1 AndAlso SyntaxHandle.Lines.Current.Text.IndexOf("*/") < pos Then
                    CommentedChar = False
                End If
                If SyntaxHandle.Lines.Current.Text.Length > 0 AndAlso (SyntaxHandle.Lines.Current.Text.IndexOf("//") > -1 AndAlso SyntaxHandle.Lines.Current.Text.IndexOf("//") < pos) Then
                    CommentedChar = True
                End If
                If CommentedChar Then Exit Sub
                If ACLists.eColors.Count Then
                    With SyntaxHandle
                        Dim CursorPos As Integer = GetLineCursorPosition()
                        If .Lines.Current.Text.IndexOf("""") < CursorPos AndAlso .Text.IndexOf("""", .Lines.Current.Text.IndexOf("""") + 1) > CursorPos Then
                            With .AutoComplete
                                .List.Clear()
                                For Each col As PawnColor In ACLists.eColors
                                    .List.Add(col.Name)
                                Next
                                If ACLists.eColors.Count = 1 Then .List.Add("-")
                                If _ShowingInfoText Then InfoText.Visible = False
                                .Show()
                            End With
                        End If
                    End With
                End If
        End Select
    End Sub

    Private Sub SyntaxHandle_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles SyntaxHandle.KeyUp
        On Error Resume Next
        Select Case e.KeyData
            Case Keys.Oemcomma
                If e.Shift Then
                    If SyntaxHandle.Lines.Current.Text.IndexOf("#in") > -1 Then
                        SyntaxHandle.Invoke(DataUpdaterEx, New Object() {UpdateType.Includes, If(SyntaxHandle.Lines.Current.Number > 2, SyntaxHandle.Lines.Current.Number - 2, SyntaxHandle.Lines.Current.Number), If(SyntaxHandle.Lines.Current.Number + 1 < SyntaxHandle.Lines.Count, If(SyntaxHandle.Lines.Current.Number + 2 < SyntaxHandle.Lines.Count, SyntaxHandle.Lines.Current.Number + 2, SyntaxHandle.Lines.Current.Number + 1), SyntaxHandle.Lines.Current.Number)})
                    ElseIf (SyntaxHandle.Lines.Current.Text.IndexOf("stock") > -1 Or SyntaxHandle.Lines.Current.Text.IndexOf("public") > -1) AndAlso SyntaxHandle.Lines.Current.Text.IndexOf("(") > -1 AndAlso SyntaxHandle.Lines.Current.Text.IndexOf(")") > -1 Then
                        SyntaxHandle.Invoke(DataUpdaterEx, New Object() {UpdateType.Functions_Callbacks, If(SyntaxHandle.Lines.Current.Number > 2, SyntaxHandle.Lines.Current.Number - 2, SyntaxHandle.Lines.Current.Number), If(SyntaxHandle.Lines.Current.Number + 1 < SyntaxHandle.Lines.Count, If(SyntaxHandle.Lines.Current.Number + 2 < SyntaxHandle.Lines.Count, SyntaxHandle.Lines.Current.Number + 2, SyntaxHandle.Lines.Current.Number + 1), SyntaxHandle.Lines.Current.Number)})
                    ElseIf SyntaxHandle.Lines.Current.Text.IndexOf("#de") > -1 Then
                        SyntaxHandle.Invoke(DataUpdaterEx, New Object() {UpdateType.Colors, If(SyntaxHandle.Lines.Current.Number > 2, SyntaxHandle.Lines.Current.Number - 2, SyntaxHandle.Lines.Current.Number), If(SyntaxHandle.Lines.Current.Number + 1 < SyntaxHandle.Lines.Count, If(SyntaxHandle.Lines.Current.Number + 2 < SyntaxHandle.Lines.Count, SyntaxHandle.Lines.Current.Number + 2, SyntaxHandle.Lines.Current.Number + 1), SyntaxHandle.Lines.Current.Number)})
                    ElseIf SyntaxHandle.Lines.Current.Text.IndexOf("new") > -1 Then
                        SyntaxHandle.Invoke(DataUpdaterEx, New Object() {UpdateType.Other, If(SyntaxHandle.Lines.Current.Number > 2, SyntaxHandle.Lines.Current.Number - 2, SyntaxHandle.Lines.Current.Number), If(SyntaxHandle.Lines.Current.Number + 1 < SyntaxHandle.Lines.Count, If(SyntaxHandle.Lines.Current.Number + 2 < SyntaxHandle.Lines.Count, SyntaxHandle.Lines.Current.Number + 2, SyntaxHandle.Lines.Current.Number + 1), SyntaxHandle.Lines.Current.Number)})
                    End If
                End If
            Case Keys.Delete, Keys.Back
                If _ShowingInfoText Then
                    If GetCurrentFunction() = "" Then : ShowingInfoText = False
                    Else
                        With InfoText
                            .Clear()
                            Dim istart As Integer, iend As Integer, func As PawnFunction, index As Integer
                            func = GetFunctionByName(ACLists.Functions, GetCurrentFunction(True, True))
                            index = If(SyntaxHandle.Lines.Current.Text(GetLineCursorPosition(False) - 1) = ",", GetCurrentParamIndex(True, True), GetCurrentParamIndex(False, True))
                            If TrueContainsFunction(ACLists.Functions, func) Then
                                If index > -1 AndAlso index < func.Params.Length Then
                                    For Each param As String In ACLists.Functions(ACLists.Functions.IndexOf(func)).Params
                                        If Not ACLists.Functions(ACLists.Functions.IndexOf(func)).Params(UBound(ACLists.Functions(ACLists.Functions.IndexOf(func)).Params)) = param Then
                                            If Array.IndexOf(ACLists.Functions(ACLists.Functions.IndexOf(func)).Params, param) = index Then
                                                istart = .Text.Length
                                                iend = istart + Len(param + ", ")
                                            End If
                                            .Text += param & ", "
                                        Else
                                            If Array.IndexOf(ACLists.Functions(ACLists.Functions.IndexOf(func)).Params, param) = index Then
                                                istart = .Text.Length
                                                iend = istart + Len(param)
                                            End If
                                            .Text += param
                                        End If
                                    Next
                                    .SelectionStart = istart
                                    .SelectionLength = iend - istart
                                    .SelectionFont = New Font(.Font.FontFamily, 10)
                                    .SelectionColor = Color.Blue
                                    If SyntaxHandle.PointXFromPosition(SyntaxHandle.CurrentPos) + .Size.Width < SyntaxHandle.Width - 20 Then
                                        .Location = New Point(SyntaxHandle.PointXFromPosition(SyntaxHandle.CurrentPos), SyntaxHandle.PointYFromPosition(SyntaxHandle.CurrentPos) + 20)
                                    Else
                                        .Location = New Point(SyntaxHandle.Width - .Size.Width, SyntaxHandle.PointYFromPosition(SyntaxHandle.CurrentPos) + 20)
                                    End If
                                Else
                                    index = UBound(func.Params)
                                    If func.Params(index).IndexOf("...") > -1 Then
                                        With InfoText
                                            If _ShowingInfoText Then
                                                .Clear()
                                                For Each param As String In ACLists.Functions(ACLists.Functions.IndexOf(func)).Params
                                                    If Not ACLists.Functions(ACLists.Functions.IndexOf(func)).Params(UBound(ACLists.Functions(ACLists.Functions.IndexOf(func)).Params)) = param Then
                                                        If Array.IndexOf(ACLists.Functions(ACLists.Functions.IndexOf(func)).Params, param) = index Then
                                                            istart = .Text.Length
                                                            iend = istart + Len(param + ", ")
                                                        End If
                                                        .Text += param & ", "
                                                    Else
                                                        If Array.IndexOf(ACLists.Functions(ACLists.Functions.IndexOf(func)).Params, param) = index Then
                                                            istart = .Text.Length
                                                            iend = istart + Len(param)
                                                        End If
                                                        .Text += param
                                                    End If
                                                Next
                                                .SelectionStart = istart
                                                .SelectionLength = iend - istart
                                                .SelectionFont = New Font(.Font.FontFamily, 10)
                                                .SelectionColor = Color.Blue
                                                If SyntaxHandle.PointXFromPosition(SyntaxHandle.CurrentPos) + .Size.Width < SyntaxHandle.Width - 20 Then
                                                    .Location = New Point(SyntaxHandle.PointXFromPosition(SyntaxHandle.CurrentPos), SyntaxHandle.PointYFromPosition(SyntaxHandle.CurrentPos) + 20)
                                                Else
                                                    .Location = New Point(SyntaxHandle.Width - .Size.Width, SyntaxHandle.PointYFromPosition(SyntaxHandle.CurrentPos) + 20)
                                                End If
                                            End If
                                        End With
                                    Else
                                        With InfoText
                                            If _ShowingInfoText Then
                                                .Clear()
                                                For Each param As String In ACLists.Functions(ACLists.Functions.IndexOf(func)).Params
                                                    If Not ACLists.Functions(ACLists.Functions.IndexOf(func)).Params(UBound(ACLists.Functions(ACLists.Functions.IndexOf(func)).Params)) = param Then
                                                        .Text += param & ", "
                                                    Else
                                                        .Text += param
                                                    End If
                                                Next
                                                If SyntaxHandle.PointXFromPosition(SyntaxHandle.CurrentPos) + .Size.Width < SyntaxHandle.Width - 20 Then
                                                    .Location = New Point(SyntaxHandle.PointXFromPosition(SyntaxHandle.CurrentPos), SyntaxHandle.PointYFromPosition(SyntaxHandle.CurrentPos) + 20)
                                                Else
                                                    .Location = New Point(SyntaxHandle.Width - .Size.Width, SyntaxHandle.PointYFromPosition(SyntaxHandle.CurrentPos) + 20)
                                                End If
                                            End If
                                        End With
                                    End If
                                End If
                            Else
                                ShowingInfoText = False
                            End If
                        End With
                    End If
                End If
                If SyntaxHandle.Lines.Current.Text.IndexOf("#in") > -1 Then
                    SyntaxHandle.Invoke(DataUpdaterEx, New Object() {UpdateType.Includes, If(SyntaxHandle.Lines.Current.Number > 2, SyntaxHandle.Lines.Current.Number - 2, SyntaxHandle.Lines.Current.Number), If(SyntaxHandle.Lines.Current.Number + 1 < SyntaxHandle.Lines.Count, If(SyntaxHandle.Lines.Current.Number + 2 < SyntaxHandle.Lines.Count, SyntaxHandle.Lines.Current.Number + 2, SyntaxHandle.Lines.Current.Number + 1), SyntaxHandle.Lines.Current.Number)})
                ElseIf (SyntaxHandle.Lines.Current.Text.IndexOf("stock") > -1 Or SyntaxHandle.Lines.Current.Text.IndexOf("public") > -1) AndAlso SyntaxHandle.Lines.Current.Text.IndexOf("(") > -1 AndAlso SyntaxHandle.Lines.Current.Text.IndexOf(")") > -1 Then
                    SyntaxHandle.Invoke(DataUpdaterEx, New Object() {UpdateType.Functions_Callbacks, If(SyntaxHandle.Lines.Current.Number > 2, SyntaxHandle.Lines.Current.Number - 2, SyntaxHandle.Lines.Current.Number), If(SyntaxHandle.Lines.Current.Number + 1 < SyntaxHandle.Lines.Count, If(SyntaxHandle.Lines.Current.Number + 2 < SyntaxHandle.Lines.Count, SyntaxHandle.Lines.Current.Number + 2, SyntaxHandle.Lines.Current.Number + 1), SyntaxHandle.Lines.Current.Number)})
                ElseIf SyntaxHandle.Lines.Current.Text.IndexOf("#de") > -1 Then
                    SyntaxHandle.Invoke(DataUpdaterEx, New Object() {UpdateType.Colors, If(SyntaxHandle.Lines.Current.Number > 2, SyntaxHandle.Lines.Current.Number - 2, SyntaxHandle.Lines.Current.Number), If(SyntaxHandle.Lines.Current.Number + 1 < SyntaxHandle.Lines.Count, If(SyntaxHandle.Lines.Current.Number + 2 < SyntaxHandle.Lines.Count, SyntaxHandle.Lines.Current.Number + 2, SyntaxHandle.Lines.Current.Number + 1), SyntaxHandle.Lines.Current.Number)})
                ElseIf SyntaxHandle.Lines.Current.Text.IndexOf("new") > -1 Then
                    SyntaxHandle.Invoke(DataUpdaterEx, New Object() {UpdateType.Other, If(SyntaxHandle.Lines.Current.Number > 2, SyntaxHandle.Lines.Current.Number - 2, SyntaxHandle.Lines.Current.Number), If(SyntaxHandle.Lines.Current.Number + 1 < SyntaxHandle.Lines.Count, If(SyntaxHandle.Lines.Current.Number + 2 < SyntaxHandle.Lines.Count, SyntaxHandle.Lines.Current.Number + 2, SyntaxHandle.Lines.Current.Number + 1), SyntaxHandle.Lines.Current.Number)})
                End If
            Case Keys.Enter, Keys.Down
                If SyntaxHandle.Lines(SyntaxHandle.Lines.Current.Number - 1).Text.IndexOf("#in") > -1 Then
                    SyntaxHandle.Invoke(DataUpdaterEx, New Object() {UpdateType.Includes, If(SyntaxHandle.Lines.Current.Number > 2, SyntaxHandle.Lines.Current.Number - 2, SyntaxHandle.Lines.Current.Number), If(SyntaxHandle.Lines.Current.Number + 1 < SyntaxHandle.Lines.Count, If(SyntaxHandle.Lines.Current.Number + 2 < SyntaxHandle.Lines.Count, SyntaxHandle.Lines.Current.Number + 2, SyntaxHandle.Lines.Current.Number + 1), SyntaxHandle.Lines.Current.Number)})
                ElseIf (SyntaxHandle.Lines(SyntaxHandle.Lines.Current.Number - 1).Text.IndexOf("stock") > -1 Or SyntaxHandle.Lines(SyntaxHandle.Lines.Current.Number - 1).Text.IndexOf("public") > -1) AndAlso SyntaxHandle.Lines(SyntaxHandle.Lines.Current.Number - 1).Text.IndexOf("(") > -1 AndAlso SyntaxHandle.Lines(SyntaxHandle.Lines.Current.Number - 1).Text.IndexOf(")") > -1 Then
                    SyntaxHandle.Invoke(DataUpdaterEx, New Object() {UpdateType.Functions_Callbacks, If(SyntaxHandle.Lines.Current.Number > 2, SyntaxHandle.Lines.Current.Number - 2, SyntaxHandle.Lines.Current.Number), If(SyntaxHandle.Lines.Current.Number + 1 < SyntaxHandle.Lines.Count, If(SyntaxHandle.Lines.Current.Number + 2 < SyntaxHandle.Lines.Count, SyntaxHandle.Lines.Current.Number + 2, SyntaxHandle.Lines.Current.Number + 1), SyntaxHandle.Lines.Current.Number)})
                ElseIf SyntaxHandle.Lines(SyntaxHandle.Lines.Current.Number - 1).Text.IndexOf("#de") > -1 Then
                    SyntaxHandle.Invoke(DataUpdaterEx, New Object() {UpdateType.Colors, If(SyntaxHandle.Lines.Current.Number > 2, SyntaxHandle.Lines.Current.Number - 2, SyntaxHandle.Lines.Current.Number), If(SyntaxHandle.Lines.Current.Number + 1 < SyntaxHandle.Lines.Count, If(SyntaxHandle.Lines.Current.Number + 2 < SyntaxHandle.Lines.Count, SyntaxHandle.Lines.Current.Number + 2, SyntaxHandle.Lines.Current.Number + 1), SyntaxHandle.Lines.Current.Number)})
                ElseIf SyntaxHandle.Lines(SyntaxHandle.Lines.Current.Number - 1).Text.IndexOf("new") > -1 Then
                    SyntaxHandle.Invoke(DataUpdaterEx, New Object() {UpdateType.Other, If(SyntaxHandle.Lines.Current.Number > 2, SyntaxHandle.Lines.Current.Number - 2, SyntaxHandle.Lines.Current.Number), If(SyntaxHandle.Lines.Current.Number + 1 < SyntaxHandle.Lines.Count, If(SyntaxHandle.Lines.Current.Number + 2 < SyntaxHandle.Lines.Count, SyntaxHandle.Lines.Current.Number + 2, SyntaxHandle.Lines.Current.Number + 1), SyntaxHandle.Lines.Current.Number)})
                End If
                SyntaxHandle.Invoke(MarginUpdater)
            Case Keys.Up
                If SyntaxHandle.Lines(SyntaxHandle.Lines.Current.Number + 1).Text.IndexOf("#in") > -1 Then
                    SyntaxHandle.Invoke(DataUpdaterEx, New Object() {UpdateType.Includes, If(SyntaxHandle.Lines.Current.Number > 2, SyntaxHandle.Lines.Current.Number - 2, SyntaxHandle.Lines.Current.Number), If(SyntaxHandle.Lines.Current.Number + 1 < SyntaxHandle.Lines.Count, If(SyntaxHandle.Lines.Current.Number + 2 < SyntaxHandle.Lines.Count, SyntaxHandle.Lines.Current.Number + 2, SyntaxHandle.Lines.Current.Number + 1), SyntaxHandle.Lines.Current.Number)})
                ElseIf (SyntaxHandle.Lines(SyntaxHandle.Lines.Current.Number + 1).Text.IndexOf("stock") > -1 Or SyntaxHandle.Lines(SyntaxHandle.Lines.Current.Number + 1).Text.IndexOf("public") > -1) AndAlso SyntaxHandle.Lines(SyntaxHandle.Lines.Current.Number + 1).Text.IndexOf("(") > -1 AndAlso SyntaxHandle.Lines(SyntaxHandle.Lines.Current.Number + 1).Text.IndexOf(")") > -1 Then
                    SyntaxHandle.Invoke(DataUpdaterEx, New Object() {UpdateType.Functions_Callbacks, If(SyntaxHandle.Lines.Current.Number > 2, SyntaxHandle.Lines.Current.Number - 2, SyntaxHandle.Lines.Current.Number), If(SyntaxHandle.Lines.Current.Number + 1 < SyntaxHandle.Lines.Count, If(SyntaxHandle.Lines.Current.Number + 2 < SyntaxHandle.Lines.Count, SyntaxHandle.Lines.Current.Number + 2, SyntaxHandle.Lines.Current.Number + 1), SyntaxHandle.Lines.Current.Number)})
                ElseIf SyntaxHandle.Lines(SyntaxHandle.Lines.Current.Number + 1).Text.IndexOf("#de") > -1 Then
                    SyntaxHandle.Invoke(DataUpdaterEx, New Object() {UpdateType.Colors, If(SyntaxHandle.Lines.Current.Number > 2, SyntaxHandle.Lines.Current.Number - 2, SyntaxHandle.Lines.Current.Number), If(SyntaxHandle.Lines.Current.Number + 1 < SyntaxHandle.Lines.Count, If(SyntaxHandle.Lines.Current.Number + 2 < SyntaxHandle.Lines.Count, SyntaxHandle.Lines.Current.Number + 2, SyntaxHandle.Lines.Current.Number + 1), SyntaxHandle.Lines.Current.Number)})
                ElseIf SyntaxHandle.Lines(SyntaxHandle.Lines.Current.Number + 1).Text.IndexOf("new") > -1 Then
                    SyntaxHandle.Invoke(DataUpdaterEx, New Object() {UpdateType.Other, If(SyntaxHandle.Lines.Current.Number > 2, SyntaxHandle.Lines.Current.Number - 2, SyntaxHandle.Lines.Current.Number), If(SyntaxHandle.Lines.Current.Number + 1 < SyntaxHandle.Lines.Count, If(SyntaxHandle.Lines.Current.Number + 2 < SyntaxHandle.Lines.Count, SyntaxHandle.Lines.Current.Number + 2, SyntaxHandle.Lines.Current.Number + 1), SyntaxHandle.Lines.Current.Number)})
                End If
            Case Keys.Right, Keys.Left
                If _ShowingInfoText Then
                    If GetCurrentFunction() = "" Then : ShowingInfoText = False
                    Else
                        With InfoText
                            .Clear()
                            Dim istart As Integer, iend As Integer, func As PawnFunction, index As Integer
                            func = GetFunctionByName(ACLists.Functions, GetCurrentFunction(True, True))
                            index = If(SyntaxHandle.Lines.Current.Text(GetLineCursorPosition() - 1) = ",", GetCurrentParamIndex(True, True), GetCurrentParamIndex(False, True))
                            If TrueContainsFunction(ACLists.Functions, func) Then
                                If index > -1 AndAlso index < func.Params.Length Then
                                    For Each param As String In ACLists.Functions(ACLists.Functions.IndexOf(func)).Params
                                        If Not ACLists.Functions(ACLists.Functions.IndexOf(func)).Params(UBound(ACLists.Functions(ACLists.Functions.IndexOf(func)).Params)) = param Then
                                            If Array.IndexOf(ACLists.Functions(ACLists.Functions.IndexOf(func)).Params, param) = index Then
                                                istart = .Text.Length
                                                iend = istart + Len(param + ", ")
                                            End If
                                            .Text += param & ", "
                                        Else
                                            If Array.IndexOf(ACLists.Functions(ACLists.Functions.IndexOf(func)).Params, param) = index Then
                                                istart = .Text.Length
                                                iend = istart + Len(param)
                                            End If
                                            .Text += param
                                        End If
                                    Next
                                    .SelectionStart = istart
                                    .SelectionLength = iend - istart
                                    .SelectionFont = New Font(.Font.FontFamily, 10)
                                    .SelectionColor = Color.Blue
                                    If SyntaxHandle.PointXFromPosition(SyntaxHandle.CurrentPos) + .Size.Width < SyntaxHandle.Width - 20 Then
                                        .Location = New Point(SyntaxHandle.PointXFromPosition(SyntaxHandle.CurrentPos), SyntaxHandle.PointYFromPosition(SyntaxHandle.CurrentPos) + 20)
                                    Else
                                        .Location = New Point(SyntaxHandle.Width - .Size.Width, SyntaxHandle.PointYFromPosition(SyntaxHandle.CurrentPos) + 20)
                                    End If
                                Else
                                    index = UBound(func.Params)
                                    If func.Params(index).IndexOf("...") > -1 Then
                                        With InfoText
                                            If _ShowingInfoText Then
                                                .Clear()
                                                For Each param As String In ACLists.Functions(ACLists.Functions.IndexOf(func)).Params
                                                    If Not ACLists.Functions(ACLists.Functions.IndexOf(func)).Params(UBound(ACLists.Functions(ACLists.Functions.IndexOf(func)).Params)) = param Then
                                                        If Array.IndexOf(ACLists.Functions(ACLists.Functions.IndexOf(func)).Params, param) = index Then
                                                            istart = .Text.Length
                                                            iend = istart + Len(param + ", ")
                                                        End If
                                                        .Text += param & ", "
                                                    Else
                                                        If Array.IndexOf(ACLists.Functions(ACLists.Functions.IndexOf(func)).Params, param) = index Then
                                                            istart = .Text.Length
                                                            iend = istart + Len(param)
                                                        End If
                                                        .Text += param
                                                    End If
                                                Next
                                                .SelectionStart = istart
                                                .SelectionLength = iend - istart
                                                .SelectionFont = New Font(.Font.FontFamily, 10)
                                                .SelectionColor = Color.Blue
                                                If SyntaxHandle.PointXFromPosition(SyntaxHandle.CurrentPos) + .Size.Width < SyntaxHandle.Width - 20 Then
                                                    .Location = New Point(SyntaxHandle.PointXFromPosition(SyntaxHandle.CurrentPos), SyntaxHandle.PointYFromPosition(SyntaxHandle.CurrentPos) + 20)
                                                Else
                                                    .Location = New Point(SyntaxHandle.Width - .Size.Width, SyntaxHandle.PointYFromPosition(SyntaxHandle.CurrentPos) + 20)
                                                End If
                                            End If
                                        End With
                                    Else
                                        With InfoText
                                            If _ShowingInfoText Then
                                                .Clear()
                                                For Each param As String In ACLists.Functions(ACLists.Functions.IndexOf(func)).Params
                                                    If Not ACLists.Functions(ACLists.Functions.IndexOf(func)).Params(UBound(ACLists.Functions(ACLists.Functions.IndexOf(func)).Params)) = param Then
                                                        .Text += param & ", "
                                                    Else
                                                        .Text += param
                                                    End If
                                                Next
                                                If SyntaxHandle.PointXFromPosition(SyntaxHandle.CurrentPos) + .Size.Width < SyntaxHandle.Width - 20 Then
                                                    .Location = New Point(SyntaxHandle.PointXFromPosition(SyntaxHandle.CurrentPos), SyntaxHandle.PointYFromPosition(SyntaxHandle.CurrentPos) + 20)
                                                Else
                                                    .Location = New Point(SyntaxHandle.Width - .Size.Width, SyntaxHandle.PointYFromPosition(SyntaxHandle.CurrentPos) + 20)
                                                End If
                                            End If
                                        End With
                                    End If
                                End If
                            Else
                                ShowingInfoText = False
                            End If
                        End With
                    End If
                End If
                If SyntaxHandle.Lines.Current.Text.IndexOf("#in") > -1 Then
                    SyntaxHandle.Invoke(DataUpdaterEx, New Object() {UpdateType.Includes, If(SyntaxHandle.Lines.Current.Number > 2, SyntaxHandle.Lines.Current.Number - 2, SyntaxHandle.Lines.Current.Number), If(SyntaxHandle.Lines.Current.Number + 1 < SyntaxHandle.Lines.Count, If(SyntaxHandle.Lines.Current.Number + 2 < SyntaxHandle.Lines.Count, SyntaxHandle.Lines.Current.Number + 2, SyntaxHandle.Lines.Current.Number + 1), SyntaxHandle.Lines.Current.Number)})
                ElseIf (SyntaxHandle.Lines.Current.Text.IndexOf("stock") > -1 Or SyntaxHandle.Lines.Current.Text.IndexOf("public") > -1) AndAlso SyntaxHandle.Lines.Current.Text.IndexOf("(") > -1 AndAlso SyntaxHandle.Lines.Current.Text.IndexOf(")") > -1 Then
                    SyntaxHandle.Invoke(DataUpdaterEx, New Object() {UpdateType.Functions_Callbacks, If(SyntaxHandle.Lines.Current.Number > 2, SyntaxHandle.Lines.Current.Number - 2, SyntaxHandle.Lines.Current.Number), If(SyntaxHandle.Lines.Current.Number + 1 < SyntaxHandle.Lines.Count, If(SyntaxHandle.Lines.Current.Number + 15 < SyntaxHandle.Lines.Count, SyntaxHandle.Lines.Current.Number + 15, SyntaxHandle.Lines.Current.Number + 1), SyntaxHandle.Lines.Current.Number)})
                ElseIf SyntaxHandle.Lines.Current.Text.IndexOf("#de") > -1 Then
                    SyntaxHandle.Invoke(DataUpdaterEx, New Object() {UpdateType.Colors, If(SyntaxHandle.Lines.Current.Number > 2, SyntaxHandle.Lines.Current.Number - 2, SyntaxHandle.Lines.Current.Number), If(SyntaxHandle.Lines.Current.Number + 1 < SyntaxHandle.Lines.Count, If(SyntaxHandle.Lines.Current.Number + 2 < SyntaxHandle.Lines.Count, SyntaxHandle.Lines.Current.Number + 2, SyntaxHandle.Lines.Current.Number + 1), SyntaxHandle.Lines.Current.Number)})
                ElseIf SyntaxHandle.Lines.Current.Text.IndexOf("new") > -1 Then
                    SyntaxHandle.Invoke(DataUpdaterEx, New Object() {UpdateType.Other, If(SyntaxHandle.Lines.Current.Number > 2, SyntaxHandle.Lines.Current.Number - 2, SyntaxHandle.Lines.Current.Number), If(SyntaxHandle.Lines.Current.Number + 1 < SyntaxHandle.Lines.Count, If(SyntaxHandle.Lines.Current.Number + 2 < SyntaxHandle.Lines.Count, SyntaxHandle.Lines.Current.Number + 2, SyntaxHandle.Lines.Current.Number + 1), SyntaxHandle.Lines.Current.Number)})
                End If
            Case Keys.Escape
                If _ShowingInfoText Then ShowingInfoText = False
        End Select
    End Sub

    Private Sub SyntaxHandle_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles SyntaxHandle.KeyPress
        If Tim.Enabled Then Tim.Enabled = False
        Select Case Settings.Language
            Case Languages.English
                Main.Label2.Text = "Document lines: " & SyntaxHandle.Lines.Count
            Case Languages.Español
                Main.Label2.Text = "Lineas del documento: " & SyntaxHandle.Lines.Count
            Case Languages.Portuguêse
                Main.Label2.Text = "Linhas de documento: " & SyntaxHandle.Lines.Count
            Case Else
                Main.Label2.Text = "Document Linien: " & SyntaxHandle.Lines.Count
        End Select
    End Sub

    Private Sub SyntaxHandle_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles SyntaxHandle.SelectionChanged
        With Main.Label1
            If SyntaxHandle.Selection.Length > 0 Then
                Select Case Settings.Language
                    Case Languages.English
                        .Text = "Selection length: " & SyntaxHandle.Selection.Length
                    Case Languages.Español
                        .Text = "Largo de selección: " & SyntaxHandle.Selection.Length
                    Case Languages.Portuguêse
                        .Text = "Longo de seleção: " & SyntaxHandle.Selection.Length
                    Case Else
                        .Text = "Länge der Auswahl: " & SyntaxHandle.Selection.Length
                End Select
                If Settings.aSelect Then
                    Main.TabControl2.SelectedTab = Main.TabPage1
                    SyntaxHandle.Focus()
                End If
            Else
                If GetCurrentFunction() = "" AndAlso _ShowingInfoText Then ShowingInfoText = False
                Select Case Settings.Language
                    Case Languages.English
                        .Text = "Selection length: 0"
                    Case Languages.Español
                        .Text = "Largo de selección: 0"
                    Case Languages.Portuguêse
                        .Text = "Longo de seleção: 0"
                    Case Else
                        .Text = "Länge der Auswahl: 0"
                End Select
            End If
        End With
        With Main.Label2
            Select Case Settings.Language
                Case Languages.English
                    .Text = "Document lines: " & SyntaxHandle.Lines.Count
                Case Languages.Español
                    .Text = "Lineas del documento: " & SyntaxHandle.Lines.Count
                Case Languages.Portuguêse
                    .Text = "Linhas de documento: " & SyntaxHandle.Lines.Count
                Case Else
                    .Text = "Document Linien: " & SyntaxHandle.Lines.Count
            End Select
        End With
    End Sub

    Private Sub SyntaxHandle_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles SyntaxHandle.TextChanged
        If wait And first Then
            Saved = True
            first = False
            SyntaxHandle.Invoke(MarginUpdater)
            SyntaxHandle.Invoke(DataUpdater)
        Else
            If SyntaxHandle.UndoRedo.IsUndoEnabled = False Then
                SyntaxHandle.UndoRedo.IsUndoEnabled = True
            End If
            Saved = False
        End If
    End Sub

    Private Sub SaveMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If Not _Path Is Nothing AndAlso _Path.Length > 0 Then
            Dim Writer As New StreamWriter(_Path)
            Writer.Write(SyntaxHandle.Text)
            Writer.Close()
        Else
            With Main.SFD
                .InitialDirectory = Settings.DefaultPath
                .ShowDialog()
                If Not .FileName Is Nothing AndAlso .FileName.Length > 0 Then
                    Dim Writer As New StreamWriter(Main.SFD.FileName)
                    Writer.Write(SyntaxHandle.Text)
                    Writer.Close()
                    _Ext = Mid(.FileName, .FileName.LastIndexOf(".") + 2, .FileName.Length - .FileName.LastIndexOf(".") - 1)
                    _Path = .FileName
                    Saved = True
                End If
            End With
        End If
    End Sub

    Private Sub SaveAsMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        With Main.SFD
            .InitialDirectory = Settings.DefaultPath
            .ShowDialog()
            If Not .FileName Is Nothing AndAlso .FileName.Length > 0 Then
                Dim Writer As New StreamWriter(Main.SFD.FileName)
                Writer.Write(SyntaxHandle.Text)
                Writer.Close()
                _Ext = Mid(.FileName, .FileName.LastIndexOf(".") + 2, .FileName.Length - .FileName.LastIndexOf(".") - 1)
                _Path = .FileName
                Saved = True
            End If
        End With
    End Sub

    Private Sub Tim_Tick(ByVal sender As Object, ByVal e As Timers.ElapsedEventArgs) Handles Tim.Elapsed
        Dim hWnd As IntPtr = FindWindow(vbNullString, "Scripting Machine")
        PostMessageA(hWnd, WM_HOTKEY, 9303, vbNull)
        Tim.Enabled = False
    End Sub

#End Region

#Region "Functions"

    Private Function GetCurrentFunction(Optional ByVal remove As Boolean = False, Optional ByVal must As Boolean = False) As String
        Static func As String, lastcall As Long
        Dim calrest As Long = GetTickCount() - lastcall
        If lastcall = 0 OrElse (calrest) > 3000 Then
            func = StrReverse(Mid(SyntaxHandle.Lines.Current.Text.Replace(vbCrLf, "").Replace(vbTab, ""), 1, SyntaxHandle.Lines.Current.Text.Length))
            If (func.StartsWith(";") OrElse func.StartsWith("//")) AndAlso func.IndexOf("""") = -1 Then
                lastcall = GetTickCount()
                Return ""
            End If
            Dim tmp As Integer = func.IndexOf("""")
            If tmp > -1 Then
                tmp = func.IndexOf("""", tmp + 1)
                If tmp > -1 Then
                    While tmp > -1
                        func = func.Remove(func.IndexOf(""""), tmp + 1)
                        tmp = func.IndexOf("""")
                        If tmp > -1 Then
                            tmp = func.IndexOf("""", tmp)
                        Else
                            Exit While
                        End If
                    End While
                Else
                    func = func.Remove(0, func.IndexOf("""") + 1)
                End If
            End If
            If func.StartsWith("(") AndAlso func.IndexOf(",") > -1 Then func = func.Remove(func.IndexOf(","), func.Length - func.IndexOf(","))
            If remove AndAlso func.StartsWith("(") Then func = func.Remove(0, 1)
            While func.IndexOf("(") > -1 AndAlso func.IndexOf(")") > -1
                If func.IndexOf("(", func.IndexOf("(") + 1) > -1 Then
                    tmp = func.IndexOf(")(")
                    If tmp = -1 Then
                        func = func.Remove(func.LastIndexOf(")"), func.IndexOf("(", func.LastIndexOf(")") + 1) - func.LastIndexOf(")") + 1)
                    Else
                        If func.IndexOf("(", tmp + 1) > -1 Then
                            func = func.Remove(tmp, func.IndexOf("(", tmp + 2) - tmp)
                        Else
                            func = func.Remove(func.IndexOf(")"), func.Length - func.IndexOf(")"))
                        End If
                    End If
                Else
                    If func.IndexOf(")(") = -1 Then
                        func = func.Remove(func.LastIndexOf(")"), func.IndexOf("(") + 1)
                    Else
                        func = func.Remove(func.IndexOf(")"), func.Length - func.IndexOf(")"))
                    End If
                End If
            End While
            Dim pos As Integer = func.IndexOf("(")
            If pos > -1 Then
                If func.IndexOf(",") = -1 Then
                    If func.IndexOf("(", pos + 1) > -1 Then
                        func = func.Remove(func.LastIndexOf("("), func.Length - func.LastIndexOf("("))
                    Else
                        func = func.Remove(0, pos)
                    End If
                Else
                    If func.IndexOf("(", pos + 1) = -1 Then
                        func = func.Remove(0, func.IndexOf("("))
                        If func.StartsWith("(") Then
                            func = func.Remove(0, 1)
                        ElseIf must Then
                            Return ""
                        End If
                    Else
                        While func.IndexOf("(", pos + 1) > -1
                            func = func.Remove(0, func.IndexOf("("))
                            If func.IndexOf(",") > -1 Then
                                func = func.Remove(func.IndexOf(","), func.IndexOf("(", func.IndexOf("(") + 1) - func.IndexOf(","))
                            End If
                            func = func.Remove(func.IndexOf("(", func.IndexOf("(") + 1), func.Length - func.IndexOf("(", func.IndexOf("(") + 1))
                            pos = func.IndexOf("(")
                        End While
                    End If
                End If
            ElseIf must Then
                Return ""
            End If
            lastcall = GetTickCount()
            Return Trim(StrReverse(func.Replace("(", "").Replace(",", "")))
        Else
            Return Trim(StrReverse(func.Replace("(", "")))
        End If
    End Function

    Private Function GetCurrentParamIndex(Optional ByVal fix As Boolean = False, Optional ByVal remove As Boolean = False) As Integer
        If GetCurrentFunction(True) = "" Then Return -1
        Static index As Integer, lastcall As Long
        If lastcall = 0 OrElse (GetTickCount() - lastcall) > 500 Then
            Try
                Dim tmp(1) As String, pos(1) As Integer
                tmp(1) = Mid(SyntaxHandle.Lines.Current.Text.Replace(vbCrLf, "").Replace(vbTab, ""), 1, SyntaxHandle.Lines.Current.Text.Length)
                pos(0) = GetLineCursorPosition()
                tmp(1) = tmp(1).Remove(pos(0), tmp(1).Length - pos(0))
                tmp(0) = StrReverse(Mid(SyntaxHandle.Lines.Current.Text.Replace(vbCrLf, "").Replace(vbTab, ""), 1, SyntaxHandle.Lines.Current.Text.Length))
                While tmp(0).IndexOf("(") > -1 AndAlso tmp(0).IndexOf(")") > -1
                    pos(0) = tmp(0).IndexOf(")")
                    pos(1) = tmp(0).IndexOf(",", tmp(0).IndexOf("(", tmp(0).IndexOf(")")))
                    tmp(0) = tmp(0).Remove(pos(0), pos(1) - pos(0))
                End While
                If tmp(0).IndexOf(",") = -1 Then Return 0
                If remove AndAlso tmp(0).StartsWith("(") Then tmp(0) = tmp(0).Remove(0, 1)
                If tmp(0).IndexOf("(", tmp(0).IndexOf("(") + 1) > -1 Then
                    pos(0) = tmp(0).IndexOf(",", tmp(0).IndexOf("("))
                    tmp(0) = tmp(0).Remove(pos(0), tmp(0).Length - pos(0))
                End If
                tmp(0) = Trim(StrReverse(tmp(0)))
                index = CountEqualCharsFromString(tmp(0), ",", tmp(0).IndexOf(tmp(1)))
                lastcall = GetTickCount()
                Return If(fix, index - 1, index)
            Catch ex As Exception
                Return -1
            End Try
        Else
            Return If(fix, index - 1, index)
        End If
    End Function

    Private Function GetLineCursorPosition(Optional ByVal fixed As Boolean = False) As Integer
        Return If(fixed, SyntaxHandle.Selection.Start - SyntaxHandle.Lines.Current.IndentPosition + 1, SyntaxHandle.Selection.Start - SyntaxHandle.Lines.Current.IndentPosition)
    End Function

#End Region

#Region "Delegates Subs/Functions"

    Private Function AddFirstNode(ByVal Name As String, ByVal Tree As TreeView) As TreeNode
        Return Tree.Nodes.Add(Name)
    End Function

    Private Function AddNode(ByVal Name As String, ByVal Node As TreeNode) As TreeNode
        Return Node.Nodes.Add(Name)
    End Function

    Private Sub ClearTreeNodes(ByVal Tree As TreeView)
        Tree.Nodes.Clear()
    End Sub

    Private Sub UpdateMargin()
        With SyntaxHandle
            Select Case .Lines.Count
                Case 0 To 99
                    .Margins.Margin0.Width = 18
                Case 100 To 999
                    .Margins.Margin0.Width = 28
                Case 1000 To 9999
                    .Margins.Margin0.Width = 38
                Case 10000 To 99999
                    .Margins.Margin0.Width = 48
                Case 100000 To 999999
                    .Margins.Margin0.Width = 58
                Case Else
                    .Margins.Margin0.Width = 68
            End Select
        End With
    End Sub

    Private Function GetLineCollection(ByVal tControl As Scintilla) As ScintillaNet.LinesCollection
        Return tControl.Lines
    End Function

    Private Sub SetControlVisible(ByVal tControl As Control, ByVal value As Boolean)
        tControl.Visible = value
    End Sub

#End Region

#End Region

#Region "Delegates Subs/Functions"

    Public Sub UpdateDataEx(ByVal Type As UpdateType, ByVal startline As Integer, ByVal endline As Integer)
        On Error Resume Next
        Static LastUpdate As Long = -1
        If LastUpdate <> -1 AndAlso (GetTickCount() - LastUpdate) < 30000 Then Exit Sub
        If ACLists.Functions.Count = 0 Then
            SyntaxHandle.Invoke(DataUpdater)
            LastUpdate = GetTickCount()
            Exit Sub
        End If
        Dim CommentedLine As Boolean, CommentedSection As Boolean, tmp As String = vbNullString
        Select Case Type
            Case UpdateType.Includes
                For Each func As PawnFunction In ACLists.Functions
                    If func.Line = -1 Then func.Exist = False
                Next
                For Each clbk As PawnFunction In ACLists.Callbacks
                    If clbk.Line = -1 Then clbk.Exist = False
                Next
                For Each Line As ScintillaNet.Line In SyntaxHandle.Lines
                    If Line.Number > endline Then Exit For
                    If Line.Text.Length = 0 OrElse Line.Number < startline OrElse Line.Text = "{" OrElse Line.Text = "}" OrElse Line.Text = ";" Then Continue For
                    If Line.Text.IndexOf("//") > -1 Then
                        CommentedLine = True
                    ElseIf Line.Text.IndexOf("/*") > -1 AndAlso Line.Text.IndexOf("*/") = -1 Then
                        CommentedSection = True
                    ElseIf Line.Text.IndexOf("*/") > -1 Then
                        CommentedSection = False
                    End If
                    If CommentedLine OrElse CommentedSection Then
                        CommentedLine = False
                        Continue For
                    End If
                    If Line.Text.IndexOf("#include") > -1 Then
                        Dim file As String
                        If Line.Text.IndexOf("<") > -1 Then
                            file = Mid(Line.Text, Line.Text.IndexOf("<") + 2, Line.Text.IndexOf(">") - Line.Text.IndexOf("<") - 1)
                            file += ".inc"
                        Else
                            file = Mid(Line.Text, Line.Text.IndexOf("""") + 2, Line.Text.IndexOf("""") - Line.Text.IndexOf("""", Line.Text.IndexOf("""")))
                            If file.IndexOf(".inc") = -1 Then file += ".inc"
                        End If
                        If IO.File.Exists(My.Application.Info.DirectoryPath & "\Include\" & file) Then
                            Dim fLine As String, Reader As New StreamReader(My.Application.Info.DirectoryPath & "\Include\" & file)
                            fLine = Reader.ReadLine()
                            Do Until fLine Is Nothing
                                If fLine.Length = 0 OrElse fLine = "{" OrElse fLine = "}" OrElse fLine = ";" Then
                                    fLine = Reader.ReadLine()
                                    Continue Do
                                ElseIf fLine.StartsWith("//") Then
                                    CommentedLine = True
                                ElseIf fLine.IndexOf("/*") > -1 AndAlso fLine.IndexOf("*/") = -1 Then
                                    CommentedSection = True
                                ElseIf fLine.IndexOf("*/") > -1 Then
                                    CommentedSection = False
                                End If
                                If CommentedLine Or CommentedSection Then
                                    CommentedLine = False
                                    fLine = Reader.ReadLine()
                                    Continue Do
                                End If
                                Dim pos As Integer = fLine.IndexOf("native")
                                If pos = -1 Then
                                    pos = fLine.IndexOf("stock")
                                    If pos = -1 Then pos = fLine.IndexOf("public")
                                End If
                                If fLine.IndexOf("#include") > -1 Then
                                    Dim file2 As String, cNode2 As New TreeNode()
                                    If fLine.IndexOf("<") > -1 Then
                                        file2 = Mid(fLine, fLine.IndexOf("<") + 2, fLine.IndexOf(">") - fLine.IndexOf("<") - 1)
                                        file2 += ".inc"
                                    Else
                                        file2 = Mid(fLine, fLine.IndexOf("""") + 2, fLine.IndexOf("""") - fLine.IndexOf("""", fLine.IndexOf("""")))
                                        If file2.IndexOf(".inc") = -1 Then file2 += ".inc"
                                    End If
                                    If IO.File.Exists(My.Application.Info.DirectoryPath & "\Include\" & file2) Then
                                        Dim Reader2 As New StreamReader(My.Application.Info.DirectoryPath & "\Include\" & file2)
                                        fLine = Reader2.ReadLine()
                                        Do Until fLine Is Nothing
                                            If fLine.Length = 0 OrElse fLine = "{" OrElse fLine = "}" OrElse fLine = ";" Then
                                                fLine = Reader2.ReadLine()
                                                Continue Do
                                            ElseIf fLine.StartsWith("//") Then
                                                CommentedLine = True
                                            ElseIf fLine.IndexOf("/*") > -1 AndAlso fLine.IndexOf("*/") = -1 Then
                                                CommentedSection = True
                                            ElseIf fLine.IndexOf("*/") > -1 Then
                                                CommentedSection = False
                                            End If
                                            If CommentedLine Or CommentedSection Then
                                                CommentedLine = False
                                                fLine = Reader2.ReadLine()
                                                Continue Do
                                            End If
                                            pos = fLine.IndexOf("native")
                                            If pos = -1 Then
                                                pos = fLine.IndexOf("stock")
                                                If pos = -1 Then pos = fLine.IndexOf("public")
                                            End If
                                            If pos > -1 AndAlso fLine.IndexOf("(") > -1 AndAlso fLine.IndexOf(")") > -1 AndAlso fLine.IndexOf("operator") = -1 Then
                                                Dim params As New List(Of String)
                                                params.AddRange(Split(Trim(Mid(fLine, fLine.IndexOf("(") + 2, fLine.IndexOf(")") - fLine.IndexOf("(") - 1)), ","))
                                                For i = 0 To params.Count - 1
                                                    If i > 0 AndAlso params(i).Length > 0 AndAlso params(i).IndexOf("...") > -1 Then
                                                        params(i - 1) += "," & params(i)
                                                        params.RemoveAt(i)
                                                        Continue For
                                                    End If
                                                Next
                                                Dim func As PawnFunction = New PawnFunction(Trim(Mid(fLine, fLine.IndexOf(" ", pos) + 2, fLine.IndexOf("(") - fLine.IndexOf(" ", pos) - 1)), file2.Replace(".inc", ":"), -1, params.ToArray)
                                                If Not TrueContainsFunction(ACLists.Functions, func, True) AndAlso Not TrueContainsFunction(ACLists.Callbacks, func) Then ACLists.Functions.Add(func)
                                            ElseIf fLine.IndexOf("forward") > -1 AndAlso fLine.IndexOf("(") > -1 AndAlso fLine.IndexOf(")") > -1 Then
                                                Dim func As PawnFunction = New PawnFunction(Trim(Mid(fLine, fLine.IndexOf(" ") + 1, fLine.IndexOf("(") - fLine.IndexOf(" "))), file2.Replace(".inc", ":"), -1, Split(Trim(Mid(fLine, fLine.IndexOf("(") + 2, fLine.IndexOf(")") - fLine.IndexOf("(") - 1)), ","))
                                                If Not TrueContainsFunction(ACLists.Callbacks, func, True) Then ACLists.Callbacks.Add(func)
                                            End If
                                            fLine = Reader2.ReadLine()
                                        Loop
                                        Reader2.Close()
                                    Else
                                        Errors.Clear()
                                        Errors.Add(New ListViewItem(New String() {"", "100", Name, line.Number + 1, "cannot read from file: """ & file2 & """"}, 0))
                                    End If
                                ElseIf fLine.IndexOf("#tryinclude") > -1 Then
                                    Dim file2 As String
                                    If fLine.IndexOf("<") > -1 Then
                                        file2 = Mid(fLine, fLine.IndexOf("<") + 2, fLine.IndexOf(">") - fLine.IndexOf("<") - 1)
                                        file2 += ".inc"
                                    Else
                                        file2 = Mid(fLine, fLine.IndexOf("""") + 2, fLine.IndexOf("""") - fLine.IndexOf("""", fLine.IndexOf("""")))
                                        If file2.IndexOf(".inc") = -1 Then file2 += ".inc"
                                    End If
                                    If IO.File.Exists(My.Application.Info.DirectoryPath & "\Include\" & file2) Then
                                        Dim Reader2 As New StreamReader(My.Application.Info.DirectoryPath & "\Include\" & file2)
                                        fLine = Reader2.ReadLine()
                                        Do Until fLine Is Nothing
                                            If fLine.Length = 0 OrElse fLine = "{" OrElse fLine = "}" OrElse fLine = ";" Then
                                                fLine = Reader2.ReadLine()
                                                Continue Do
                                            ElseIf fLine.StartsWith("//") Then
                                                CommentedLine = True
                                            ElseIf fLine.IndexOf("/*") > -1 AndAlso fLine.IndexOf("*/") = -1 Then
                                                CommentedSection = True
                                            ElseIf fLine.IndexOf("*/") > -1 Then
                                                CommentedSection = False
                                            End If
                                            If CommentedLine Or CommentedSection Then
                                                CommentedLine = False
                                                fLine = Reader2.ReadLine()
                                                Continue Do
                                            End If
                                            pos = fLine.IndexOf("native")
                                            If pos = -1 Then
                                                pos = fLine.IndexOf("stock")
                                                If pos = -1 Then pos = fLine.IndexOf("public")
                                            End If
                                            If pos > -1 AndAlso fLine.IndexOf("(") > -1 AndAlso fLine.IndexOf(")") > -1 AndAlso fLine.IndexOf("operator") = -1 Then
                                                Dim params As New List(Of String)
                                                params.AddRange(Split(Trim(Mid(fLine, fLine.IndexOf("(") + 2, fLine.IndexOf(")") - fLine.IndexOf("(") - 1)), ","))
                                                For i = 0 To params.Count - 1
                                                    If i > 0 AndAlso params(i).Length > 0 AndAlso params(i).IndexOf("...") > -1 Then
                                                        params(i - 1) += "," & params(i)
                                                        params.RemoveAt(i)
                                                        Continue For
                                                    End If
                                                Next
                                                Dim func As PawnFunction = New PawnFunction(Trim(Mid(fLine, fLine.IndexOf(" ", pos) + 2, fLine.IndexOf("(") - fLine.IndexOf(" ", pos) - 1)), file2.Replace(".inc", ":"), -1, params.ToArray)
                                                If Not TrueContainsFunction(ACLists.Functions, func, True) AndAlso Not TrueContainsFunction(ACLists.Callbacks, func) Then ACLists.Functions.Add(func)
                                            ElseIf fLine.IndexOf("forward") > -1 AndAlso fLine.IndexOf("(") > -1 AndAlso fLine.IndexOf(")") > -1 Then
                                                Dim func As PawnFunction = New PawnFunction(Trim(Mid(fLine, fLine.IndexOf(" ") + 1, fLine.IndexOf("(") - fLine.IndexOf(" "))), file2.Replace(".inc", ":"), -1, Split(Trim(Mid(fLine, fLine.IndexOf("(") + 2, fLine.IndexOf(")") - fLine.IndexOf("(") - 1)), ","))
                                                If Not TrueContainsFunction(ACLists.Callbacks, func, True) Then ACLists.Callbacks.Add(func)
                                            End If
                                            fLine = Reader2.ReadLine()
                                        Loop
                                        Reader2.Close()
                                    Else
                                        Errors.Clear()
                                        Errors.Add(New ListViewItem(New String() {"", "100", Name, line.Number + 1, "cannot read from file: """ & file2 & """"}, 1))
                                    End If
                                ElseIf pos > -1 AndAlso fLine.IndexOf("(") > -1 AndAlso fLine.IndexOf(")") > -1 AndAlso fLine.IndexOf("operator") = -1 Then
                                    Dim params As New List(Of String)
                                    params.AddRange(Split(Trim(Mid(fLine, fLine.IndexOf("(") + 2, fLine.IndexOf(")") - fLine.IndexOf("(") - 1)), ","))
                                    For i = 0 To params.Count - 1
                                        If i > 0 AndAlso params(i).Length > 0 AndAlso params(i).IndexOf("...") > -1 Then
                                            params(i - 1) += "," & params(i)
                                            params.RemoveAt(i)
                                            Continue For
                                        End If
                                    Next
                                    Dim func As PawnFunction = New PawnFunction(Trim(Mid(fLine, fLine.IndexOf(" ", pos) + 2, fLine.IndexOf("(") - fLine.IndexOf(" ", pos) - 1)), file.Replace(".inc", ":"), -1, params.ToArray)
                                    If Not TrueContainsFunction(ACLists.Functions, func, True) AndAlso Not TrueContainsFunction(ACLists.Callbacks, func) Then ACLists.Functions.Add(func)
                                ElseIf fLine.IndexOf("forward") > -1 AndAlso fLine.IndexOf("(") > -1 AndAlso fLine.IndexOf(")") > -1 Then
                                    Dim func As PawnFunction = New PawnFunction(Trim(Mid(fLine, fLine.IndexOf(" ") + 1, fLine.IndexOf("(") - fLine.IndexOf(" "))), file.Replace(".inc", ":"), -1, Split(Trim(Mid(fLine, fLine.IndexOf("(") + 2, fLine.IndexOf(")") - fLine.IndexOf("(") - 1)), ","))
                                    If Not TrueContainsFunction(ACLists.Callbacks, func, True) Then ACLists.Callbacks.Add(func)
                                End If
                                fLine = Reader.ReadLine()
                            Loop
                            Reader.Close()
                        Else
                            Errors.Clear()
                            Errors.Add(New ListViewItem(New String() {"", "100", Name, line.Number + 1, "cannot read from file: """ & file & """"}, 0))
                        End If
                    ElseIf Line.Text.IndexOf("#tryinclude") > -1 Then
                        Dim file As String
                        If Line.Text.IndexOf("<") > -1 Then
                            file = Mid(Line.Text, Line.Text.IndexOf("<") + 2, Line.Text.IndexOf(">") - Line.Text.IndexOf("<") - 1)
                            file += ".inc"
                        Else
                            file = Mid(Line.Text, Line.Text.IndexOf("""") + 2, Line.Text.IndexOf("""") - Line.Text.IndexOf("""", Line.Text.IndexOf("""")))
                            If file.IndexOf(".inc") = -1 Then file += ".inc"
                        End If
                        If IO.File.Exists(My.Application.Info.DirectoryPath & "\Include\" & file) Then
                            Dim fLine As String, Reader As New StreamReader(My.Application.Info.DirectoryPath & "\Include\" & file)
                            fLine = Reader.ReadLine()
                            Do Until fLine Is Nothing
                                If fLine.Length = 0 OrElse fLine = "{" OrElse fLine = "}" OrElse fLine = ";" Then
                                    fLine = Reader.ReadLine()
                                    Continue Do
                                ElseIf fLine.StartsWith("//") Then
                                    CommentedLine = True
                                ElseIf fLine.IndexOf("/*") > -1 AndAlso fLine.IndexOf("*/") = -1 Then
                                    CommentedSection = True
                                ElseIf fLine.IndexOf("*/") > -1 Then
                                    CommentedSection = False
                                End If
                                If CommentedLine Or CommentedSection Then
                                    CommentedLine = False
                                    fLine = Reader.ReadLine()
                                    Continue Do
                                End If
                                Dim pos As Integer = fLine.IndexOf("native")
                                If pos = -1 Then
                                    pos = fLine.IndexOf("stock")
                                    If pos = -1 Then pos = fLine.IndexOf("public")
                                End If
                                If fLine.IndexOf("#include") > -1 Then
                                    Dim file2 As String, cNode2 As New TreeNode()
                                    If fLine.IndexOf("<") > -1 Then
                                        file2 = Mid(fLine, fLine.IndexOf("<") + 2, fLine.IndexOf(">") - fLine.IndexOf("<") - 1)
                                        file2 += ".inc"
                                    Else
                                        file2 = Mid(fLine, fLine.IndexOf("""") + 2, fLine.IndexOf("""") - fLine.IndexOf("""", fLine.IndexOf("""")))
                                        If file2.IndexOf(".inc") = -1 Then file2 += ".inc"
                                    End If
                                    If IO.File.Exists(My.Application.Info.DirectoryPath & "\Include\" & file2) Then
                                        Dim Reader2 As New StreamReader(My.Application.Info.DirectoryPath & "\Include\" & file2)
                                        fLine = Reader2.ReadLine()
                                        Do Until fLine Is Nothing
                                            If fLine.Length = 0 OrElse fLine = "{" OrElse fLine = "}" OrElse fLine = ";" Then
                                                fLine = Reader2.ReadLine()
                                                Continue Do
                                            ElseIf fLine.StartsWith("//") Then
                                                CommentedLine = True
                                            ElseIf fLine.IndexOf("/*") > -1 AndAlso fLine.IndexOf("*/") = -1 Then
                                                CommentedSection = True
                                            ElseIf fLine.IndexOf("*/") > -1 Then
                                                CommentedSection = False
                                            End If
                                            If CommentedLine Or CommentedSection Then
                                                CommentedLine = False
                                                fLine = Reader2.ReadLine()
                                                Continue Do
                                            End If
                                            pos = fLine.IndexOf("native")
                                            If pos = -1 Then
                                                pos = fLine.IndexOf("stock")
                                                If pos = -1 Then pos = fLine.IndexOf("public")
                                            End If
                                            If pos > -1 AndAlso fLine.IndexOf("(") > -1 AndAlso fLine.IndexOf(")") > -1 AndAlso fLine.IndexOf("operator") = -1 Then
                                                Dim params As New List(Of String)
                                                params.AddRange(Split(Trim(Mid(fLine, fLine.IndexOf("(") + 2, fLine.IndexOf(")") - fLine.IndexOf("(") - 1)), ","))
                                                For i = 0 To params.Count - 1
                                                    If i > 0 AndAlso params(i).Length > 0 AndAlso params(i).IndexOf("...") > -1 Then
                                                        params(i - 1) += "," & params(i)
                                                        params.RemoveAt(i)
                                                        Continue For
                                                    End If
                                                Next
                                                Dim func As PawnFunction = New PawnFunction(Trim(Mid(fLine, fLine.IndexOf(" ", pos) + 2, fLine.IndexOf("(") - fLine.IndexOf(" ", pos) - 1)), file2.Replace(".inc", ":"), -1, params.ToArray)
                                                If Not TrueContainsFunction(ACLists.Functions, func, True) AndAlso Not TrueContainsFunction(ACLists.Callbacks, func) Then ACLists.Functions.Add(func)
                                            ElseIf fLine.IndexOf("forward") > -1 AndAlso fLine.IndexOf("(") > -1 AndAlso fLine.IndexOf(")") > -1 Then
                                                Dim func As PawnFunction = New PawnFunction(Trim(Mid(fLine, fLine.IndexOf(" ") + 1, fLine.IndexOf("(") - fLine.IndexOf(" "))), file2.Replace(".inc", ":"), -1, Split(Trim(Mid(fLine, fLine.IndexOf("(") + 2, fLine.IndexOf(")") - fLine.IndexOf("(") - 1)), ","))
                                                If Not TrueContainsFunction(ACLists.Callbacks, func, True) Then ACLists.Callbacks.Add(func)
                                            End If
                                            fLine = Reader2.ReadLine()
                                        Loop
                                        Reader2.Close()
                                    Else
                                        Errors.Clear()
                                        Errors.Add(New ListViewItem(New String() {"", "100", Name, line.Number + 1, "cannot read from file: """ & file2 & """"}, 0))
                                    End If
                                ElseIf fLine.IndexOf("#tryinclude") > -1 Then
                                    Dim file2 As String
                                    If fLine.IndexOf("<") > -1 Then
                                        file2 = Mid(fLine, fLine.IndexOf("<") + 2, fLine.IndexOf(">") - fLine.IndexOf("<") - 1)
                                        file2 += ".inc"
                                    Else
                                        file2 = Mid(fLine, fLine.IndexOf("""") + 2, fLine.IndexOf("""") - fLine.IndexOf("""", fLine.IndexOf("""")))
                                        If file2.IndexOf(".inc") = -1 Then file2 += ".inc"
                                    End If
                                    If IO.File.Exists(My.Application.Info.DirectoryPath & "\Include\" & file2) Then
                                        Dim Reader2 As New StreamReader(My.Application.Info.DirectoryPath & "\Include\" & file2)
                                        fLine = Reader2.ReadLine()
                                        Do Until fLine Is Nothing
                                            If fLine.Length = 0 OrElse fLine = "{" OrElse fLine = "}" OrElse fLine = ";" Then
                                                fLine = Reader2.ReadLine()
                                                Continue Do
                                            ElseIf fLine.StartsWith("//") Then
                                                CommentedLine = True
                                            ElseIf fLine.IndexOf("/*") > -1 AndAlso fLine.IndexOf("*/") = -1 Then
                                                CommentedSection = True
                                            ElseIf fLine.IndexOf("*/") > -1 Then
                                                CommentedSection = False
                                            End If
                                            If CommentedLine Or CommentedSection Then
                                                CommentedLine = False
                                                fLine = Reader2.ReadLine()
                                                Continue Do
                                            End If
                                            pos = fLine.IndexOf("native")
                                            If pos = -1 Then
                                                pos = fLine.IndexOf("stock")
                                                If pos = -1 Then pos = fLine.IndexOf("public")
                                            End If
                                            If pos > -1 AndAlso fLine.IndexOf("(") > -1 AndAlso fLine.IndexOf(")") > -1 AndAlso fLine.IndexOf("operator") = -1 Then
                                                Dim params As New List(Of String)
                                                params.AddRange(Split(Trim(Mid(fLine, fLine.IndexOf("(") + 2, fLine.IndexOf(")") - fLine.IndexOf("(") - 1)), ","))
                                                For i = 0 To params.Count - 1
                                                    If i > 0 AndAlso params(i).Length > 0 AndAlso params(i).IndexOf("...") > -1 Then
                                                        params(i - 1) += "," & params(i)
                                                        params.RemoveAt(i)
                                                        Continue For
                                                    End If
                                                Next
                                                Dim func As PawnFunction = New PawnFunction(Trim(Mid(fLine, fLine.IndexOf(" ", pos) + 2, fLine.IndexOf("(") - fLine.IndexOf(" ", pos) - 1)), file2.Replace(".inc", ":"), -1, params.ToArray)
                                                If Not TrueContainsFunction(ACLists.Functions, func, True) AndAlso Not TrueContainsFunction(ACLists.Callbacks, func) Then ACLists.Functions.Add(func)
                                            ElseIf fLine.IndexOf("forward") > -1 AndAlso fLine.IndexOf("(") > -1 AndAlso fLine.IndexOf(")") > -1 Then
                                                Dim func As PawnFunction = New PawnFunction(Trim(Mid(fLine, fLine.IndexOf(" ") + 1, fLine.IndexOf("(") - fLine.IndexOf(" "))), file2.Replace(".inc", ":"), -1, Split(Trim(Mid(fLine, fLine.IndexOf("(") + 2, fLine.IndexOf(")") - fLine.IndexOf("(") - 1)), ","))
                                                If Not TrueContainsFunction(ACLists.Callbacks, func, True) Then ACLists.Callbacks.Add(func)
                                            End If
                                            fLine = Reader2.ReadLine()
                                        Loop
                                        Reader2.Close()
                                    Else
                                        Errors.Clear()
                                        Errors.Add(New ListViewItem(New String() {"", "100", Name, line.Number + 1, "cannot read from file: """ & file2 & """"}, 1))
                                    End If
                                ElseIf pos > -1 AndAlso fLine.IndexOf("(") > -1 AndAlso fLine.IndexOf(")") > -1 AndAlso fLine.IndexOf("operator") = -1 Then
                                    Dim params As New List(Of String)
                                    params.AddRange(Split(Trim(Mid(fLine, fLine.IndexOf("(") + 2, fLine.IndexOf(")") - fLine.IndexOf("(") - 1)), ","))
                                    For i = 0 To params.Count - 1
                                        If i > 0 AndAlso params(i).Length > 0 AndAlso params(i).IndexOf("...") > -1 Then
                                            params(i - 1) += "," & params(i)
                                            params.RemoveAt(i)
                                            Continue For
                                        End If
                                    Next
                                    Dim func As PawnFunction = New PawnFunction(Trim(Mid(fLine, fLine.IndexOf(" ", pos) + 2, fLine.IndexOf("(") - fLine.IndexOf(" ", pos) - 1)), file.Replace(".inc", ":"), -1, params.ToArray)
                                    If Not TrueContainsFunction(ACLists.Functions, func, True) AndAlso Not TrueContainsFunction(ACLists.Callbacks, func) Then ACLists.Functions.Add(func)
                                ElseIf fLine.IndexOf("forward") > -1 AndAlso fLine.IndexOf("(") > -1 AndAlso fLine.IndexOf(")") > -1 Then
                                    Dim func As PawnFunction = New PawnFunction(Trim(Mid(fLine, fLine.IndexOf(" ") + 1, fLine.IndexOf("(") - fLine.IndexOf(" "))), file.Replace(".inc", ":"), -1, Split(Trim(Mid(fLine, fLine.IndexOf("(") + 2, fLine.IndexOf(")") - fLine.IndexOf("(") - 1)), ","))
                                    If Not TrueContainsFunction(ACLists.Callbacks, func, True) Then ACLists.Callbacks.Add(func)
                                End If
                                fLine = Reader.ReadLine()
                            Loop
                            Reader.Close()
                        Else
                            Errors.Clear()
                            Errors.Add(New ListViewItem(New String() {"", "100", Name, line.Number + 1, "cannot read from file: """ & file & """"}, 1))
                        End If
                    End If
                Next
                For Each func As PawnFunction In ACLists.Functions
                    If Not func.Exist Then ACLists.Functions.Remove(func)
                Next
                For Each clbk As PawnFunction In ACLists.Callbacks
                    If Not clbk.Exist Then ACLists.Callbacks.Remove(clbk)
                Next
            Case UpdateType.Functions_Callbacks
                For Each func As PawnFunction In ACLists.Functions
                    If func.Line >= startline AndAlso func.Line < endline Then func.Exist = False
                Next
                For Each clbk As PawnFunction In ACLists.Callbacks
                    If clbk.Line >= startline AndAlso clbk.Line < endline Then clbk.Exist = False
                Next
                For Each Line As ScintillaNet.Line In SyntaxHandle.Lines
                    If Line.Number > endline Then Exit For
                    If Line.Text.Length = 0 OrElse Line.Number < startline OrElse Line.Text = "{" OrElse Line.Text = "}" OrElse Line.Text = ";" Then Continue For
                    If Line.Text.IndexOf("//") > -1 Then
                        CommentedLine = True
                    ElseIf Line.Text.IndexOf("/*") > -1 AndAlso Line.Text.IndexOf("*/") = -1 Then
                        CommentedSection = True
                    ElseIf Line.Text.IndexOf("*/") > -1 Then
                        CommentedSection = False
                    End If
                    If CommentedLine OrElse CommentedSection Then
                        CommentedLine = False
                        Continue For
                    End If
                    Dim pos As Integer = Line.Text.IndexOf("native")
                    If pos = -1 Then
                        pos = Line.Text.IndexOf("stock")
                        If pos = -1 Then pos = Line.Text.IndexOf("public")
                    End If
                    If pos > -1 AndAlso Line.Text.IndexOf("(") > -1 AndAlso Line.Text.IndexOf(")") > -1 AndAlso Line.Text.IndexOf("operator") = -1 Then
                        Dim params As New List(Of String)
                        params.AddRange(Split(Trim(Mid(Line.Text, Line.Text.IndexOf("(") + 2, Line.Text.IndexOf(")") - Line.Text.IndexOf("(") - 1)), ","))
                        For i = 0 To params.Count - 1
                            If i > 0 AndAlso params(i).Length > 0 AndAlso params(i).IndexOf("...") > -1 Then
                                params(i - 1) += "," & params(i)
                                params.RemoveAt(i)
                                Continue For
                            End If
                        Next
                        Dim func As PawnFunction = New PawnFunction(Trim(Mid(Line.Text, Line.Text.IndexOf(" ", pos) + 2, Line.Text.IndexOf("(") - Line.Text.IndexOf(" ", pos) - 1)), Name.Replace(".inc", ":"), -1, params.ToArray)
                        If Not TrueContainsFunction(ACLists.Functions, func, True) AndAlso Not TrueContainsFunction(ACLists.Callbacks, func) Then ACLists.Functions.Add(func)
                    ElseIf Line.Text.IndexOf("forward") > -1 AndAlso Line.Text.IndexOf("(") > -1 AndAlso Line.Text.IndexOf(")") > -1 Then
                        Dim func As PawnFunction = New PawnFunction(Trim(Mid(Line.Text, Line.Text.IndexOf(" ") + 1, Line.Text.IndexOf("(") - Line.Text.IndexOf(" "))), Name.Replace(".inc", ":"), -1, Split(Trim(Mid(Line.Text, Line.Text.IndexOf("(") + 2, Line.Text.IndexOf(")") - Line.Text.IndexOf("(") - 1)), ","))
                        If Not TrueContainsFunction(ACLists.Callbacks, func, True) Then ACLists.Callbacks.Add(func)
                    End If
                Next
                For Each func As PawnFunction In ACLists.Functions
                    If Not func.Exist Then ACLists.Functions.Remove(func)
                Next
                For Each clbk As PawnFunction In ACLists.Callbacks
                    If Not clbk.Exist Then ACLists.Callbacks.Remove(clbk)
                Next
            Case UpdateType.Colors
                For Each col As PawnColor In ACLists.Colors
                    If col.Line >= startline AndAlso col.Line < endline Then col.Exist = False
                Next
                For Each col As PawnColor In ACLists.eColors
                    If col.Line >= startline AndAlso col.Line < endline Then col.Exist = False
                Next
                For Each Line As ScintillaNet.Line In SyntaxHandle.Lines
                    If Line.Number > endline Then Exit For
                    If Line.Text.Length = 0 OrElse Line.Number < startline OrElse Line.Text = "{" OrElse Line.Text = "}" OrElse Line.Text = ";" Then Continue For
                    If Line.Text.IndexOf("//") > -1 Then
                        CommentedLine = True
                    ElseIf Line.Text.IndexOf("/*") > -1 AndAlso Line.Text.IndexOf("*/") = -1 Then
                        CommentedSection = True
                    ElseIf Line.Text.IndexOf("*/") > -1 Then
                        CommentedSection = False
                    End If
                    If CommentedLine OrElse CommentedSection Then
                        CommentedLine = False
                        Continue For
                    End If
                    If Line.Text.IndexOf("#define") > -1 Then
                        If Line.Text.IndexOf("0x") > -1 Then
                            Dim col As New PawnColor(), value As String
                            If Line.Text.IndexOf("(") > -1 AndAlso Line.Text.IndexOf(")") > -1 Then
                                value = Trim(Mid(Line.Text, Line.Text.IndexOf("(") + 4, 8))
                                If IsHex(value) Then
                                    col = New PawnColor(Trim(Mid(Line.Text, Line.Text.IndexOf("#define") + 9, If(Line.Text.IndexOf(" ", Line.Text.IndexOf(" ") + 2) > 0, Line.Text.IndexOf(" ", Line.Text.IndexOf(" ") + 2) - Line.Text.IndexOf(" ") - 1, Line.Text.IndexOf(vbTab.ToString()) - Line.Text.IndexOf(" ") - 1))), Color.FromArgb(Integer.Parse(Mid(value, 7, 2), Globalization.NumberStyles.HexNumber), Integer.Parse(Mid(value, 1, 2), Globalization.NumberStyles.HexNumber), Integer.Parse(Mid(value, 3, 2), Globalization.NumberStyles.HexNumber), Integer.Parse(Mid(value, 5, 2), Globalization.NumberStyles.HexNumber)), Line.Number)
                                    If Not TrueContainsColor(ACLists.Colors, col, True) Then
                                        ACLists.Colors.Add(col)
                                    End If
                                End If
                            Else
                                value = Trim(Mid(Line.Text, Line.Text.IndexOf("0x") + 3, 8))
                                If IsHex(value) Then
                                    col = New PawnColor(Trim(Mid(Line.Text, Line.Text.IndexOf("#define") + 9, If(Line.Text.IndexOf(" ", Line.Text.IndexOf(" ") + 2) > 0, Line.Text.IndexOf(" ", Line.Text.IndexOf(" ") + 2) - Line.Text.IndexOf(" ") - 1, Line.Text.IndexOf(vbTab.ToString()) - Line.Text.IndexOf(" ") - 1))), Color.FromArgb(Integer.Parse(Mid(value, 7, 2), Globalization.NumberStyles.HexNumber), Integer.Parse(Mid(value, 1, 2), Globalization.NumberStyles.HexNumber), Integer.Parse(Mid(value, 3, 2), Globalization.NumberStyles.HexNumber), Integer.Parse(Mid(value, 5, 2), Globalization.NumberStyles.HexNumber)), Line.Number)
                                    If Not TrueContainsColor(ACLists.Colors, col, True) Then ACLists.Colors.Add(col)
                                End If
                            End If
                        Else
                            If Line.Text.IndexOf("""") > -1 AndAlso Line.Text.IndexOf("""", Line.Text.IndexOf("""") + 1) > -1 Then
                                Dim value As String
                                If Line.Text.IndexOf("{") > -1 AndAlso Line.Text.IndexOf("}") > -1 Then
                                    value = Trim(Mid(Line.Text, Line.Text.IndexOf("{") + 2, 6))
                                Else
                                    value = Trim(Mid(Line.Text, Line.Text.IndexOf("""") + 2, 6))
                                End If
                                If IsHex(value) Then
                                    Dim col As New PawnColor(Trim(Mid(Line.Text, Line.Text.IndexOf("#define") + 9, If(Line.Text.IndexOf(" ", Line.Text.IndexOf(" ") + 2) > 0, Line.Text.IndexOf(" ", Line.Text.IndexOf(" ") + 2) - Line.Text.IndexOf(" ") - 1, Line.Text.IndexOf(vbTab.ToString()) - Line.Text.IndexOf(" ") - 1))), Color.FromArgb(255, Integer.Parse(Mid(value, 1, 2), Globalization.NumberStyles.HexNumber), Integer.Parse(Mid(value, 3, 2), Globalization.NumberStyles.HexNumber), Integer.Parse(Mid(value, 5, 2), Globalization.NumberStyles.HexNumber)), Line.Number)
                                    If col.Name.Length > 0 AndAlso Not TrueContainsColor(ACLists.eColors, col, True) Then ACLists.eColors.Add(col)
                                End If
                            End If
                        End If
                    End If
                Next
                For Each col As PawnColor In ACLists.Colors
                    If Not col.Exist Then ACLists.Colors.Remove(col)
                Next
                For Each col As PawnColor In ACLists.eColors
                    If Not col.Exist Then ACLists.eColors.Remove(col)
                Next
            Case Else
                For Each Line As ScintillaNet.Line In SyntaxHandle.Lines
                    If Line.Number > endline Then Exit For
                    If Line.Text.Length = 0 OrElse Line.Number < startline OrElse Line.Text = "{" OrElse Line.Text = "}" OrElse Line.Text = ";" Then Continue For
                    If Line.Text.IndexOf("//") > -1 Then
                        CommentedLine = True
                    ElseIf Line.Text.IndexOf("/*") > -1 AndAlso Line.Text.IndexOf("*/") = -1 Then
                        CommentedSection = True
                    ElseIf Line.Text.IndexOf("*/") > -1 Then
                        CommentedSection = False
                    End If
                    If CommentedLine OrElse CommentedSection Then
                        CommentedLine = False
                        Continue For
                    End If
                    If Line.Text.IndexOf("Menu:") > -1 AndAlso Line.Text.IndexOf("(") = -1 AndAlso Line.Text.IndexOf(")") = -1 Then
                        If Line.Text.IndexOf(",") = -1 Then
                            If Line.Text.IndexOf(";") = -1 Then
                                tmp = Trim(Mid(Line.Text, Line.Text.IndexOf("Menu:") + 6, Line.Text.Length - Line.Text.IndexOf("Menu:") - 5))
                            Else
                                tmp = Trim(Mid(Line.Text, Line.Text.IndexOf("Menu:") + 6, Line.Text.IndexOf(";") - Line.Text.IndexOf("Menu:") - 5))
                            End If
                            If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Menus.Contains(tmp) Then ACLists.Floats.Add(tmp)
                        Else
                            Dim fround As Boolean = True, oldpos As Integer, pos As Integer = Line.Text.IndexOf(",")
                            While pos > -1
                                If fround Then
                                    tmp = Trim(Mid(Line.Text, Line.Text.IndexOf("Menu:") + 6, pos - Line.Text.IndexOf("Menu:") - 5))
                                    fround = False
                                Else
                                    tmp = Trim(Mid(Line.Text, Line.Text.IndexOf("Menu:", oldpos) + 6, pos - Line.Text.IndexOf("Menu:", oldpos) - 5))
                                End If
                                oldpos = pos
                                pos = Line.Text.IndexOf(",", pos + 1)
                                If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Menus.Contains(tmp) Then ACLists.Floats.Add(tmp)
                            End While
                            If Line.Text.IndexOf(";") > -1 Then
                                pos = Line.Text.LastIndexOf("Menu:")
                                tmp = Trim(Mid(Line.Text, pos + 6, Line.Text.IndexOf(";") - pos - 5))
                                If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Menus.Contains(tmp) Then ACLists.Floats.Add(tmp)
                            End If
                        End If
                    ElseIf Line.Text.IndexOf("Text:") > -1 AndAlso Line.Text.IndexOf("(") = -1 AndAlso Line.Text.IndexOf(")") = -1 Then
                        If Line.Text.IndexOf(",") = -1 Then
                            If Line.Text.IndexOf(";") = -1 Then
                                tmp = Trim(Mid(Line.Text, Line.Text.IndexOf("Text:") + 6, Line.Text.Length - Line.Text.IndexOf("Text:") - 5))
                            Else
                                tmp = Trim(Mid(Line.Text, Line.Text.IndexOf("Text:") + 6, Line.Text.IndexOf(";") - Line.Text.IndexOf("Text:") - 5))
                            End If
                            If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Texts.Contains(tmp) Then ACLists.Floats.Add(tmp)
                        Else
                            Dim fround As Boolean = True, oldpos As Integer, pos As Integer = Line.Text.IndexOf(",")
                            While pos > -1
                                If fround Then
                                    tmp = Trim(Mid(Line.Text, Line.Text.IndexOf("Text:") + 6, pos - Line.Text.IndexOf("Text:") - 5))
                                    fround = False
                                Else
                                    tmp = Trim(Mid(Line.Text, Line.Text.IndexOf("Text:", oldpos) + 6, pos - Line.Text.IndexOf("Text:", oldpos) - 5))
                                End If
                                oldpos = pos
                                pos = Line.Text.IndexOf(",", pos + 1)
                                If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Texts.Contains(tmp) Then ACLists.Floats.Add(tmp)
                            End While
                            If Line.Text.IndexOf(";") > -1 Then
                                pos = Line.Text.LastIndexOf("Text:")
                                tmp = Trim(Mid(Line.Text, pos + 6, Line.Text.IndexOf(";") - pos - 5))
                                If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Texts.Contains(tmp) Then ACLists.Floats.Add(tmp)
                            End If
                        End If
                    ElseIf Line.Text.IndexOf("Text3D:") > -1 AndAlso Line.Text.IndexOf("(") = -1 AndAlso Line.Text.IndexOf(")") = -1 Then
                        If Line.Text.IndexOf(",") = -1 Then
                            If Line.Text.IndexOf(";") = -1 Then
                                tmp = Trim(Mid(Line.Text, Line.Text.IndexOf("Text3D:") + 8, Line.Text.Length - Line.Text.IndexOf("Text3D:") - 7))
                            Else
                                tmp = Trim(Mid(Line.Text, Line.Text.IndexOf("Text3D:") + 8, Line.Text.IndexOf(";") - Line.Text.IndexOf("Text3D:") - 7))
                            End If
                            If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Texts2.Contains(tmp) Then ACLists.Floats.Add(tmp)
                        Else
                            Dim fround As Boolean = True, oldpos As Integer, pos As Integer = Line.Text.IndexOf(",")
                            While pos > -1
                                If fround Then
                                    tmp = Trim(Mid(Line.Text, Line.Text.IndexOf("Text3D:") + 8, pos - Line.Text.IndexOf("Text3D:") - 7))
                                    fround = False
                                Else
                                    tmp = Trim(Mid(Line.Text, Line.Text.IndexOf("Text3D:", oldpos) + 8, pos - Line.Text.IndexOf("Text3D:", oldpos) - 7))
                                End If
                                oldpos = pos
                                pos = Line.Text.IndexOf(",", pos + 1)
                                If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Texts2.Contains(tmp) Then ACLists.Floats.Add(tmp)
                            End While
                            If Line.Text.IndexOf(";") > -1 Then
                                pos = Line.Text.LastIndexOf("Text3D:")
                                tmp = Trim(Mid(Line.Text, pos + 8, Line.Text.IndexOf(";") - pos - 7))
                                If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Texts2.Contains(tmp) Then ACLists.Floats.Add(tmp)
                            End If
                        End If
                    ElseIf Line.Text.IndexOf("Float:") > -1 AndAlso Line.Text.IndexOf("(") = -1 AndAlso Line.Text.IndexOf(")") = -1 Then
                        If Line.Text.IndexOf(",") = -1 Then
                            If Line.Text.IndexOf(";") = -1 Then
                                tmp = Trim(Mid(Line.Text, Line.Text.IndexOf("Float:") + 7, Line.Text.Length - Line.Text.IndexOf("Float:") - 6))
                            Else
                                tmp = Trim(Mid(Line.Text, Line.Text.IndexOf("Float:") + 7, Line.Text.IndexOf(";") - Line.Text.IndexOf("Float:") - 6))
                            End If
                            If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Floats.Contains(tmp) Then ACLists.Floats.Add(tmp)
                        Else
                            Dim fround As Boolean = True, oldpos As Integer, pos As Integer = Line.Text.IndexOf(",")
                            While pos > -1
                                If fround Then
                                    tmp = Trim(Mid(Line.Text, Line.Text.IndexOf("Float:") + 7, pos - Line.Text.IndexOf("Float:") - 6))
                                    fround = False
                                Else
                                    tmp = Trim(Mid(Line.Text, Line.Text.IndexOf("Float:", oldpos) + 7, pos - Line.Text.IndexOf("Float:", oldpos) - 6))
                                End If
                                oldpos = pos
                                pos = Line.Text.IndexOf(",", pos + 1)
                                If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Floats.Contains(tmp) Then ACLists.Floats.Add(tmp)
                            End While
                            If Line.Text.IndexOf(";") > -1 Then
                                pos = Line.Text.LastIndexOf("Float:")
                                tmp = Trim(Mid(Line.Text, pos + 7, Line.Text.IndexOf(";") - pos - 6))
                                If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Floats.Contains(tmp) Then ACLists.Floats.Add(tmp)
                            End If
                        End If
                    ElseIf Line.Text.IndexOf("DB:") > -1 AndAlso Line.Text.IndexOf("(") = -1 AndAlso Line.Text.IndexOf(")") = -1 Then
                        If Line.Text.IndexOf(",") = -1 Then
                            If Line.Text.IndexOf(";") = -1 Then
                                tmp = Trim(Mid(Line.Text, Line.Text.IndexOf("DB:") + 3, Line.Text.Length - Line.Text.IndexOf("DB:") - 2))
                            Else
                                tmp = Trim(Mid(Line.Text, Line.Text.IndexOf("DB:") + 3, Line.Text.IndexOf(";") - Line.Text.IndexOf("DB:") - 2))
                            End If
                            If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Dbs.Contains(tmp) Then ACLists.Floats.Add(tmp)
                        Else
                            Dim fround As Boolean = True, oldpos As Integer, pos As Integer = Line.Text.IndexOf(",")
                            While pos > -1
                                If fround Then
                                    tmp = Trim(Mid(Line.Text, Line.Text.IndexOf("DB:") + 3, pos - Line.Text.IndexOf("DB:") - 2))
                                    fround = False
                                Else
                                    tmp = Trim(Mid(Line.Text, Line.Text.IndexOf("DB:", oldpos) + 3, pos - Line.Text.IndexOf("DB:", oldpos) - 2))
                                End If
                                oldpos = pos
                                pos = Line.Text.IndexOf(",", pos + 1)
                                If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Dbs.Contains(tmp) Then ACLists.Floats.Add(tmp)
                            End While
                            If Line.Text.IndexOf(";") > -1 Then
                                pos = Line.Text.LastIndexOf("DB:")
                                tmp = Trim(Mid(Line.Text, pos + 3, Line.Text.IndexOf(";") - pos - 2))
                                If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Dbs.Contains(tmp) Then ACLists.Floats.Add(tmp)
                            End If
                        End If
                    ElseIf Line.Text.IndexOf("DBResult:") > -1 AndAlso Line.Text.IndexOf("(") = -1 AndAlso Line.Text.IndexOf(")") = -1 Then
                        If Line.Text.IndexOf(",") = -1 Then
                            If Line.Text.IndexOf(";") = -1 Then
                                tmp = Trim(Mid(Line.Text, Line.Text.IndexOf("DBResult:") + 10, Line.Text.Length - Line.Text.IndexOf("DBResult:") - 8))
                            Else
                                tmp = Trim(Mid(Line.Text, Line.Text.IndexOf("DBResult:") + 10, Line.Text.IndexOf(";") - Line.Text.IndexOf("DBResult:") - 8))
                            End If
                            If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.DbRes.Contains(tmp) Then ACLists.Floats.Add(tmp)
                        Else
                            Dim fround As Boolean = True, oldpos As Integer, pos As Integer = Line.Text.IndexOf(",")
                            While pos > -1
                                If fround Then
                                    tmp = Trim(Mid(Line.Text, Line.Text.IndexOf("DBResult:") + 10, pos - Line.Text.IndexOf("DBResult:") - 8))
                                    fround = False
                                Else
                                    tmp = Trim(Mid(Line.Text, Line.Text.IndexOf("DBResult:", oldpos) + 10, pos - Line.Text.IndexOf("DBResult:", oldpos) - 8))
                                End If
                                oldpos = pos
                                pos = Line.Text.IndexOf(",", pos + 1)
                                If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.DbRes.Contains(tmp) Then ACLists.Floats.Add(tmp)
                            End While
                            If Line.Text.IndexOf(";") > -1 Then
                                pos = Line.Text.LastIndexOf("DBResult:")
                                tmp = Trim(Mid(Line.Text, pos + 10, Line.Text.IndexOf(";") - pos - 8))
                                If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.DbRes.Contains(tmp) Then ACLists.Floats.Add(tmp)
                            End If
                        End If
                    ElseIf Line.Text.IndexOf("File:") > -1 AndAlso Line.Text.IndexOf("(") = -1 AndAlso Line.Text.IndexOf(")") = -1 Then
                        If Line.Text.IndexOf(",") = -1 Then
                            If Line.Text.IndexOf(";") = -1 Then
                                tmp = Trim(Mid(Line.Text, Line.Text.IndexOf("File:") + 6, Line.Text.Length - Line.Text.IndexOf("File:") - 5))
                            Else
                                tmp = Trim(Mid(Line.Text, Line.Text.IndexOf("File:") + 6, Line.Text.IndexOf(";") - Line.Text.IndexOf("File:") - 5))
                            End If
                            If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Files.Contains(tmp) Then ACLists.Floats.Add(tmp)
                        Else
                            Dim fround As Boolean = True, oldpos As Integer, pos As Integer = Line.Text.IndexOf(",")
                            While pos > -1
                                If fround Then
                                    tmp = Trim(Mid(Line.Text, Line.Text.IndexOf("File:") + 6, pos - Line.Text.IndexOf("File:") - 5))
                                    fround = False
                                Else
                                    tmp = Trim(Mid(Line.Text, Line.Text.IndexOf("File:", oldpos) + 6, pos - Line.Text.IndexOf("File:", oldpos) - 5))
                                End If
                                oldpos = pos
                                pos = Line.Text.IndexOf(",", pos + 1)
                                If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Files.Contains(tmp) Then ACLists.Floats.Add(tmp)
                            End While
                            If Line.Text.IndexOf(";") > -1 Then
                                pos = Line.Text.LastIndexOf("File:")
                                tmp = Trim(Mid(Line.Text, pos + 6, Line.Text.IndexOf(";") - pos - 5))
                                If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Files.Contains(tmp) Then ACLists.Floats.Add(tmp)
                            End If
                        End If
                    End If
                Next
        End Select
        LastUpdate = GetTickCount()
        Dim tmpstring As String = vbNullString
        For Each item As PawnFunction In ACLists.Functions
            tmpstring += item.Name & " "
        Next
        SyntaxHandle.Lexing.Keywords(3) = tmpstring
        SyntaxHandle.Lexing.Colorize()
    End Sub

    Private Sub UpdateData()
        On Error Resume Next
        Static lastcall As Long = -1
        If lastcall <> -1 AndAlso GetTickCount() - lastcall < 30000 Then Exit Sub
        Static TreeDelegate As New AddTreeNode(AddressOf AddNode), FirstTreeDelegate As New AddFirstTreeNode(AddressOf AddFirstNode), _
                ClearDelegate As New ClearTree(AddressOf ClearTreeNodes), LinesDelegate As New SelectedLines(AddressOf GetLineCollection)
        Dim CommentedLine As Boolean, CommentedSection As Boolean, tmp As String = vbNullString
        With ACLists
            .Colors.Clear()
            .eColors.Clear()
            .DbRes.Clear()
            .Dbs.Clear()
            .Files.Clear()
            .Floats.Clear()
            .Functions.Clear()
            .Menus.Clear()
            .Texts.Clear()
            .Texts2.Clear()
        End With
        For Each line As ScintillaNet.Line In If(SyntaxHandle.InvokeRequired, SyntaxHandle.Invoke(LinesDelegate, New Object() {SyntaxHandle}), SyntaxHandle.Lines)
            If line.Text.Length = 0 OrElse line.Text = "{" OrElse line.Text = "}" OrElse line.Text = ";" Then Continue For
            If line.Text.IndexOf("//") > -1 Then
                CommentedLine = True
            ElseIf line.Text.IndexOf("/*") > -1 AndAlso line.Text.IndexOf("*/") = -1 Then
                CommentedSection = True
            ElseIf line.Text.IndexOf("*/") > -1 Then
                CommentedSection = False
            End If
            If CommentedLine Or CommentedSection Then
                CommentedLine = False
                Continue For
            End If
            Dim spos As Integer = line.Text.IndexOf("native")
            If spos = -1 Then
                spos = line.Text.IndexOf("stock")
                If spos = -1 Then spos = line.Text.IndexOf("public")
            End If
            If line.Text.IndexOf("#include") > -1 Then
                Dim file As String
                If line.Text.IndexOf("<") > -1 Then
                    file = Mid(line.Text, line.Text.IndexOf("<") + 2, line.Text.IndexOf(">") - line.Text.IndexOf("<") - 1)
                    file += ".inc"
                Else
                    file = Mid(line.Text, line.Text.IndexOf("""") + 2, line.Text.IndexOf("""") - line.Text.IndexOf("""", line.Text.IndexOf("""")))
                    If file.IndexOf(".inc") = -1 Then file += ".inc"
                End If
                If IO.File.Exists(My.Application.Info.DirectoryPath & "\Include\" & file) Then
                    Dim fLine As String, Reader As New StreamReader(My.Application.Info.DirectoryPath & "\Include\" & file)
                    fLine = Reader.ReadLine()
                    Do Until fLine Is Nothing
                        If fLine.Length = 0 OrElse fLine = "{" OrElse fLine = "}" OrElse fLine = ";" Then
                            fLine = Reader.ReadLine()
                            Continue Do
                        ElseIf fLine.StartsWith("//") Then
                            CommentedLine = True
                        ElseIf fLine.IndexOf("/*") > -1 AndAlso fLine.IndexOf("*/") = -1 Then
                            CommentedSection = True
                        ElseIf fLine.IndexOf("*/") > -1 Then
                            CommentedSection = False
                        End If
                        If CommentedLine Or CommentedSection Then
                            CommentedLine = False
                            fLine = Reader.ReadLine()
                            Continue Do
                        End If
                        spos = fLine.IndexOf("native")
                        If spos = -1 Then
                            spos = fLine.IndexOf("stock")
                            If spos = -1 Then spos = fLine.IndexOf("public")
                        End If
                        If fLine.IndexOf("#include") > -1 Then
                            Dim file2 As String, cNode2 As New TreeNode()
                            If fLine.IndexOf("<") > -1 Then
                                file2 = Mid(fLine, fLine.IndexOf("<") + 2, fLine.IndexOf(">") - fLine.IndexOf("<") - 1)
                                file2 += ".inc"
                            Else
                                file2 = Mid(fLine, fLine.IndexOf("""") + 2, fLine.IndexOf("""") - fLine.IndexOf("""", fLine.IndexOf("""")))
                                If file2.IndexOf(".inc") = -1 Then file2 += ".inc"
                            End If
                            Dim count As Integer
                            If IO.File.Exists(My.Application.Info.DirectoryPath & "\Include\" & file2) Then
                                Dim Reader2 As New StreamReader(My.Application.Info.DirectoryPath & "\Include\" & file2)
                                fLine = Reader2.ReadLine()
                                Do Until fLine Is Nothing
                                    If fLine.Length = 0 OrElse fLine = "{" OrElse fLine = "}" OrElse fLine = ";" Then
                                        count += 1
                                        fLine = Reader2.ReadLine()
                                        Continue Do
                                    ElseIf fLine.StartsWith("//") Then
                                        CommentedLine = True
                                    ElseIf fLine.IndexOf("/*") > -1 AndAlso fLine.IndexOf("*/") = -1 Then
                                        CommentedSection = True
                                    ElseIf fLine.IndexOf("*/") > -1 Then
                                        CommentedSection = False
                                    End If
                                    If CommentedLine Or CommentedSection Then
                                        CommentedLine = False
                                        count += 1
                                        fLine = Reader2.ReadLine()
                                        Continue Do
                                    End If
                                    spos = fLine.IndexOf("native")
                                    If spos = -1 Then
                                        spos = fLine.IndexOf("stock")
                                        If spos = -1 Then spos = fLine.IndexOf("public")
                                    End If
                                    If spos > -1 AndAlso fLine.IndexOf("(") > -1 AndAlso fLine.IndexOf(")") > -1 AndAlso fLine.IndexOf("operator") = -1 Then
                                        Dim params As New List(Of String)
                                        params.AddRange(Split(Trim(Mid(fLine, fLine.IndexOf("(") + 2, fLine.IndexOf(")") - fLine.IndexOf("(") - 1)), ","))
                                        For i = 0 To params.Count - 1
                                            If i > 0 AndAlso params(i).Length > 0 AndAlso params(i).IndexOf("...") > -1 Then
                                                params(i - 1) += "," & params(i)
                                                params.RemoveAt(i)
                                                Continue For
                                            End If
                                        Next
                                        Dim func As PawnFunction = New PawnFunction(Trim(Mid(fLine, fLine.IndexOf(" ", spos) + 2, fLine.IndexOf("(") - fLine.IndexOf(" ", spos) - 1)).Replace("Float:", "").Replace("bool:", ""), file2.Replace(".inc", ":"), -1, params.ToArray)
                                        If Not TrueContainsFunction(ACLists.Functions, func, True) AndAlso Not TrueContainsFunction(ACLists.Callbacks, func) Then ACLists.Functions.Add(func)
                                    ElseIf fLine.IndexOf("forward") > -1 AndAlso fLine.IndexOf("(") > -1 AndAlso fLine.IndexOf(")") > -1 Then
                                        Dim func As PawnFunction = New PawnFunction(Trim(Mid(fLine, fLine.IndexOf(" ") + 1, fLine.IndexOf("(") - fLine.IndexOf(" "))), file2.Replace(".inc", ":"), -1, Split(Trim(Mid(fLine, fLine.IndexOf("(") + 2, fLine.IndexOf(")") - fLine.IndexOf("(") - 1)), ","))
                                        If Not TrueContainsFunction(ACLists.Callbacks, func, True) Then ACLists.Callbacks.Add(func)
                                    ElseIf fLine.IndexOf("#define") > -1 Then
                                        If fLine.IndexOf("0x") > -1 Then
                                            Dim col As PawnColor, value As String
                                            If fLine.IndexOf("(") > -1 AndAlso fLine.IndexOf(")") > -1 Then
                                                value = Trim(Mid(fLine, fLine.IndexOf("(") + 4, 8))
                                                If IsHex(value) Then
                                                    col = New PawnColor(Trim(Mid(fLine, fLine.IndexOf("#define") + 9, If(fLine.IndexOf(" ", fLine.IndexOf(" ") + 2) > 0, fLine.IndexOf(" ", fLine.IndexOf(" ") + 2) - fLine.IndexOf(" ") - 1, fLine.IndexOf(vbTab.ToString()) - fLine.IndexOf(" ") - 1))), Color.FromArgb(Integer.Parse(Mid(value, 7, 2), Globalization.NumberStyles.HexNumber), Integer.Parse(Mid(value, 1, 2), Globalization.NumberStyles.HexNumber), Integer.Parse(Mid(value, 3, 2), Globalization.NumberStyles.HexNumber), Integer.Parse(Mid(value, 5, 2), Globalization.NumberStyles.HexNumber)), -1)
                                                    If Not ACLists.Colors.Contains(col) Then ACLists.Colors.Add(col)
                                                End If
                                            Else
                                                value = Trim(Mid(fLine, fLine.IndexOf("0x") + 3, 8))
                                                If IsHex(value) Then
                                                    col = New PawnColor(Trim(Mid(fLine, fLine.IndexOf("#define") + 9, If(fLine.IndexOf(" ", fLine.IndexOf(" ") + 2) > 0, fLine.IndexOf(" ", fLine.IndexOf(" ") + 2) - fLine.IndexOf(" ") - 1, fLine.IndexOf(vbTab.ToString()) - fLine.IndexOf(" ") - 1))), Color.FromArgb(Integer.Parse(Mid(value, 7, 2), Globalization.NumberStyles.HexNumber), Integer.Parse(Mid(value, 1, 2), Globalization.NumberStyles.HexNumber), Integer.Parse(Mid(value, 3, 2), Globalization.NumberStyles.HexNumber), Integer.Parse(Mid(value, 5, 2), Globalization.NumberStyles.HexNumber)), -1)
                                                    If Not ACLists.Colors.Contains(col) Then ACLists.Colors.Add(col)
                                                End If
                                            End If
                                        Else
                                            If fLine.IndexOf("""") > -1 AndAlso fLine.IndexOf("""", fLine.IndexOf("""") + 1) > -1 Then
                                                Dim value As String
                                                If fLine.IndexOf("{") > -1 AndAlso fLine.IndexOf("}") > -1 Then
                                                    value = Trim(Mid(fLine, fLine.IndexOf("{") + 2, 6))
                                                Else
                                                    value = Trim(Mid(fLine, fLine.IndexOf("""") + 2, 6))
                                                End If
                                                If IsHex(value) Then
                                                    Dim col As New PawnColor(Trim(Mid(fLine, fLine.IndexOf("#define") + 9, If(fLine.IndexOf(" ", fLine.IndexOf(" ") + 2) > 0, fLine.IndexOf(" ", fLine.IndexOf(" ") + 2) - fLine.IndexOf(" ") - 1, fLine.IndexOf(vbTab.ToString()) - fLine.IndexOf(" ") - 1))), Color.FromArgb(255, Integer.Parse(Mid(value, 1, 2), Globalization.NumberStyles.HexNumber), Integer.Parse(Mid(value, 3, 2), Globalization.NumberStyles.HexNumber), Integer.Parse(Mid(value, 5, 2), Globalization.NumberStyles.HexNumber)), -1)
                                                    If col.Name.Length > 0 AndAlso Not ACLists.eColors.Contains(col) Then ACLists.eColors.Add(col)
                                                End If
                                            End If
                                        End If
                                    ElseIf fLine.IndexOf("Menu:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                                        If fLine.IndexOf(",") = -1 Then
                                            If fLine.IndexOf(";") = -1 Then
                                                tmp = Trim(Mid(fLine, fLine.IndexOf("Menu:") + 6, fLine.Length - fLine.IndexOf("Menu:") - 5))
                                            Else
                                                tmp = Trim(Mid(fLine, fLine.IndexOf("Menu:") + 6, fLine.IndexOf(";") - fLine.IndexOf("Menu:") - 5))
                                            End If
                                            If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Menus.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                        Else
                                            Dim fround As Boolean = True, oldpos As Integer, pos As Integer = fLine.IndexOf(",")
                                            While pos > -1
                                                If fround Then
                                                    tmp = Trim(Mid(fLine, fLine.IndexOf("Menu:") + 6, pos - fLine.IndexOf("Menu:") - 5))
                                                    fround = False
                                                Else
                                                    tmp = Trim(Mid(fLine, fLine.IndexOf("Menu:", oldpos) + 6, pos - fLine.IndexOf("Menu:", oldpos) - 5))
                                                End If
                                                oldpos = pos
                                                pos = fLine.IndexOf(",", pos + 1)
                                                If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Menus.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                            End While
                                            If fLine.IndexOf(";") > -1 Then
                                                pos = fLine.LastIndexOf("Menu:")
                                                tmp = Trim(Mid(fLine, pos + 6, fLine.IndexOf(";") - pos - 5))
                                                If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Menus.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                            End If
                                        End If
                                    ElseIf fLine.IndexOf("Text:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                                        If fLine.IndexOf(",") = -1 Then
                                            If fLine.IndexOf(";") = -1 Then
                                                tmp = Trim(Mid(fLine, fLine.IndexOf("Text:") + 6, fLine.Length - fLine.IndexOf("Text:") - 5))
                                            Else
                                                tmp = Trim(Mid(fLine, fLine.IndexOf("Text:") + 6, fLine.IndexOf(";") - fLine.IndexOf("Text:") - 5))
                                            End If
                                            If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Texts.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                        Else
                                            Dim fround As Boolean = True, oldpos As Integer, pos As Integer = fLine.IndexOf(",")
                                            While pos > -1
                                                If fround Then
                                                    tmp = Trim(Mid(fLine, fLine.IndexOf("Text:") + 6, pos - fLine.IndexOf("Text:") - 5))
                                                    fround = False
                                                Else
                                                    tmp = Trim(Mid(fLine, fLine.IndexOf("Text:", oldpos) + 6, pos - fLine.IndexOf("Text:", oldpos) - 5))
                                                End If
                                                oldpos = pos
                                                pos = fLine.IndexOf(",", pos + 1)
                                                If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Texts.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                            End While
                                            If fLine.IndexOf(";") > -1 Then
                                                pos = fLine.LastIndexOf("Text:")
                                                tmp = Trim(Mid(fLine, pos + 6, fLine.IndexOf(";") - pos - 5))
                                                If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Texts.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                            End If
                                        End If
                                    ElseIf fLine.IndexOf("Text3D:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                                        If fLine.IndexOf(",") = -1 Then
                                            If fLine.IndexOf(";") = -1 Then
                                                tmp = Trim(Mid(fLine, fLine.IndexOf("Text3D:") + 8, fLine.Length - fLine.IndexOf("Text3D:") - 7))
                                            Else
                                                tmp = Trim(Mid(fLine, fLine.IndexOf("Text3D:") + 8, fLine.IndexOf(";") - fLine.IndexOf("Text3D:") - 7))
                                            End If
                                            If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Texts2.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                        Else
                                            Dim fround As Boolean = True, oldpos As Integer, pos As Integer = fLine.IndexOf(",")
                                            While pos > -1
                                                If fround Then
                                                    tmp = Trim(Mid(fLine, fLine.IndexOf("Text3D:") + 8, pos - fLine.IndexOf("Text3D:") - 7))
                                                    fround = False
                                                Else
                                                    tmp = Trim(Mid(fLine, fLine.IndexOf("Text3D:", oldpos) + 8, pos - fLine.IndexOf("Text3D:", oldpos) - 7))
                                                End If
                                                oldpos = pos
                                                pos = fLine.IndexOf(",", pos + 1)
                                                If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Texts2.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                            End While
                                            If fLine.IndexOf(";") > -1 Then
                                                pos = fLine.LastIndexOf("Text3D:")
                                                tmp = Trim(Mid(fLine, pos + 8, fLine.IndexOf(";") - pos - 7))
                                                If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Texts2.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                            End If
                                        End If
                                    ElseIf fLine.IndexOf("Float:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                                        If fLine.IndexOf(",") = -1 Then
                                            If fLine.IndexOf(";") = -1 Then
                                                tmp = Trim(Mid(fLine, fLine.IndexOf("Float:") + 7, fLine.Length - fLine.IndexOf("Float:") - 6))
                                            Else
                                                tmp = Trim(Mid(fLine, fLine.IndexOf("Float:") + 7, fLine.IndexOf(";") - fLine.IndexOf("Float:") - 6))
                                            End If
                                            If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Floats.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                        Else
                                            Dim fround As Boolean = True, oldpos As Integer, pos As Integer = fLine.IndexOf(",")
                                            While pos > -1
                                                If fround Then
                                                    tmp = Trim(Mid(fLine, fLine.IndexOf("Float:") + 7, pos - fLine.IndexOf("Float:") - 6))
                                                    fround = False
                                                Else
                                                    tmp = Trim(Mid(fLine, fLine.IndexOf("Float:", oldpos) + 7, pos - fLine.IndexOf("Float:", oldpos) - 6))
                                                End If
                                                oldpos = pos
                                                pos = fLine.IndexOf(",", pos + 1)
                                                If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Floats.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                            End While
                                            If fLine.IndexOf(";") > -1 Then
                                                pos = fLine.LastIndexOf("Float:")
                                                tmp = Trim(Mid(fLine, pos + 7, fLine.IndexOf(";") - pos - 6))
                                                If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Floats.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                            End If
                                        End If
                                    ElseIf fLine.IndexOf("DB:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                                        If fLine.IndexOf(",") = -1 Then
                                            If fLine.IndexOf(";") = -1 Then
                                                tmp = Trim(Mid(fLine, fLine.IndexOf("DB:") + 3, fLine.Length - fLine.IndexOf("DB:") - 2))
                                            Else
                                                tmp = Trim(Mid(fLine, fLine.IndexOf("DB:") + 3, fLine.IndexOf(";") - fLine.IndexOf("DB:") - 2))
                                            End If
                                            If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Dbs.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                        Else
                                            Dim fround As Boolean = True, oldpos As Integer, pos As Integer = fLine.IndexOf(",")
                                            While pos > -1
                                                If fround Then
                                                    tmp = Trim(Mid(fLine, fLine.IndexOf("DB:") + 3, pos - fLine.IndexOf("DB:") - 2))
                                                    fround = False
                                                Else
                                                    tmp = Trim(Mid(fLine, fLine.IndexOf("DB:", oldpos) + 3, pos - fLine.IndexOf("DB:", oldpos) - 2))
                                                End If
                                                oldpos = pos
                                                pos = fLine.IndexOf(",", pos + 1)
                                                If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Dbs.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                            End While
                                            If fLine.IndexOf(";") > -1 Then
                                                pos = fLine.LastIndexOf("DB:")
                                                tmp = Trim(Mid(fLine, pos + 3, fLine.IndexOf(";") - pos - 2))
                                                If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Dbs.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                            End If
                                        End If
                                    ElseIf fLine.IndexOf("DBResult:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                                        If fLine.IndexOf(",") = -1 Then
                                            If fLine.IndexOf(";") = -1 Then
                                                tmp = Trim(Mid(fLine, fLine.IndexOf("DBResult:") + 10, fLine.Length - fLine.IndexOf("DBResult:") - 8))
                                            Else
                                                tmp = Trim(Mid(fLine, fLine.IndexOf("DBResult:") + 10, fLine.IndexOf(";") - fLine.IndexOf("DBResult:") - 8))
                                            End If
                                            If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.DbRes.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                        Else
                                            Dim fround As Boolean = True, oldpos As Integer, pos As Integer = fLine.IndexOf(",")
                                            While pos > -1
                                                If fround Then
                                                    tmp = Trim(Mid(fLine, fLine.IndexOf("DBResult:") + 10, pos - fLine.IndexOf("DBResult:") - 8))
                                                    fround = False
                                                Else
                                                    tmp = Trim(Mid(fLine, fLine.IndexOf("DBResult:", oldpos) + 10, pos - fLine.IndexOf("DBResult:", oldpos) - 8))
                                                End If
                                                oldpos = pos
                                                pos = fLine.IndexOf(",", pos + 1)
                                                If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.DbRes.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                            End While
                                            If fLine.IndexOf(";") > -1 Then
                                                pos = fLine.LastIndexOf("DBResult:")
                                                tmp = Trim(Mid(fLine, pos + 10, fLine.IndexOf(";") - pos - 8))
                                                If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.DbRes.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                            End If
                                        End If
                                    ElseIf fLine.IndexOf("File:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                                        If fLine.IndexOf(",") = -1 Then
                                            If fLine.IndexOf(";") = -1 Then
                                                tmp = Trim(Mid(fLine, fLine.IndexOf("File:") + 6, fLine.Length - fLine.IndexOf("File:") - 5))
                                            Else
                                                tmp = Trim(Mid(fLine, fLine.IndexOf("File:") + 6, fLine.IndexOf(";") - fLine.IndexOf("File:") - 5))
                                            End If
                                            If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Files.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                        Else
                                            Dim fround As Boolean = True, oldpos As Integer, pos As Integer = fLine.IndexOf(",")
                                            While pos > -1
                                                If fround Then
                                                    tmp = Trim(Mid(fLine, fLine.IndexOf("File:") + 6, pos - fLine.IndexOf("File:") - 5))
                                                    fround = False
                                                Else
                                                    tmp = Trim(Mid(fLine, fLine.IndexOf("File:", oldpos) + 6, pos - fLine.IndexOf("File:", oldpos) - 5))
                                                End If
                                                oldpos = pos
                                                pos = fLine.IndexOf(",", pos + 1)
                                                If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Files.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                            End While
                                            If fLine.IndexOf(";") > -1 Then
                                                pos = fLine.LastIndexOf("File:")
                                                tmp = Trim(Mid(fLine, pos + 6, fLine.IndexOf(";") - pos - 5))
                                                If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Files.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                            End If
                                        End If
                                    End If
                                    count += 1
                                    fLine = Reader2.ReadLine()
                                Loop
                                Reader2.Close()
                            Else
                                Errors.Clear()
                                Errors.Add(New ListViewItem(New String() {"", "100", Name, count, "cannot read from file: """ & file2 & """"}, 0))
                            End If
                        ElseIf fLine.IndexOf("#tryinclude") > -1 Then
                            Dim file2 As String
                            If fLine.IndexOf("<") > -1 Then
                                file2 = Mid(fLine, fLine.IndexOf("<") + 2, fLine.IndexOf(">") - fLine.IndexOf("<") - 1)
                                file2 += ".inc"
                            Else
                                file2 = Mid(fLine, fLine.IndexOf("""") + 2, fLine.IndexOf("""") - fLine.IndexOf("""", fLine.IndexOf("""")))
                                If file2.IndexOf(".inc") = -1 Then file2 += ".inc"
                            End If
                            Dim count As Integer
                            If IO.File.Exists(My.Application.Info.DirectoryPath & "\Include\" & file2) Then
                                Dim Reader2 As New StreamReader(My.Application.Info.DirectoryPath & "\Include\" & file2)
                                fLine = Reader2.ReadLine()
                                Do Until fLine Is Nothing
                                    If fLine.Length = 0 OrElse fLine = "{" OrElse fLine = "}" OrElse fLine = ";" Then
                                        count += 1
                                        fLine = Reader2.ReadLine()
                                        Continue Do
                                    ElseIf fLine.StartsWith("//") Then
                                        CommentedLine = True
                                    ElseIf fLine.IndexOf("/*") > -1 AndAlso fLine.IndexOf("*/") = -1 Then
                                        CommentedSection = True
                                    ElseIf fLine.IndexOf("*/") > -1 Then
                                        CommentedSection = False
                                    End If
                                    If CommentedLine Or CommentedSection Then
                                        CommentedLine = False
                                        count += 1
                                        fLine = Reader2.ReadLine()
                                        Continue Do
                                    End If
                                    spos = fLine.IndexOf("native")
                                    If spos = -1 Then
                                        spos = fLine.IndexOf("stock")
                                        If spos = -1 Then spos = fLine.IndexOf("public")
                                    End If
                                    If spos > -1 AndAlso fLine.IndexOf("(") > -1 AndAlso fLine.IndexOf(")") > -1 AndAlso fLine.IndexOf("operator") = -1 Then
                                        Dim params As New List(Of String)
                                        params.AddRange(Split(Trim(Mid(fLine, fLine.IndexOf("(") + 2, fLine.IndexOf(")") - fLine.IndexOf("(") - 1)), ","))
                                        For i = 0 To params.Count - 1
                                            If i > 0 AndAlso params(i).Length > 0 AndAlso params(i).IndexOf("...") > -1 Then
                                                params(i - 1) += "," & params(i)
                                                params.RemoveAt(i)
                                                Continue For
                                            End If
                                        Next
                                        Dim func As PawnFunction = New PawnFunction(Trim(Mid(fLine, fLine.IndexOf(" ", spos) + 2, fLine.IndexOf("(") - fLine.IndexOf(" ", spos) - 1)).Replace("Float:", "").Replace("bool:", ""), file2.Replace(".inc", ":"), -1, params.ToArray)
                                        If Not TrueContainsFunction(ACLists.Functions, func, True) AndAlso Not TrueContainsFunction(ACLists.Callbacks, func) Then ACLists.Functions.Add(func)
                                    ElseIf fLine.IndexOf("forward") > -1 AndAlso fLine.IndexOf("(") > -1 AndAlso fLine.IndexOf(")") > -1 Then
                                        Dim func As PawnFunction = New PawnFunction(Trim(Mid(fLine, fLine.IndexOf(" ") + 1, fLine.IndexOf("(") - fLine.IndexOf(" "))), file2.Replace(".inc", ":"), -1, Split(Trim(Mid(fLine, fLine.IndexOf("(") + 2, fLine.IndexOf(")") - fLine.IndexOf("(") - 1)), ","))
                                        If Not TrueContainsFunction(ACLists.Callbacks, func, True) Then ACLists.Callbacks.Add(func)
                                    ElseIf fLine.IndexOf("#define") > -1 Then
                                        If fLine.IndexOf("0x") > -1 Then
                                            Dim col As PawnColor, value As String
                                            If fLine.IndexOf("(") > -1 AndAlso fLine.IndexOf(")") > -1 Then
                                                value = Trim(Mid(fLine, fLine.IndexOf("(") + 4, 8))
                                                If IsHex(value) Then
                                                    col = New PawnColor(Trim(Mid(fLine, fLine.IndexOf("#define") + 9, If(fLine.IndexOf(" ", fLine.IndexOf(" ") + 2) > 0, fLine.IndexOf(" ", fLine.IndexOf(" ") + 2) - fLine.IndexOf(" ") - 1, fLine.IndexOf(vbTab.ToString()) - fLine.IndexOf(" ") - 1))), Color.FromArgb(Integer.Parse(Mid(value, 7, 2), Globalization.NumberStyles.HexNumber), Integer.Parse(Mid(value, 1, 2), Globalization.NumberStyles.HexNumber), Integer.Parse(Mid(value, 3, 2), Globalization.NumberStyles.HexNumber), Integer.Parse(Mid(value, 5, 2), Globalization.NumberStyles.HexNumber)), -1)
                                                    If Not ACLists.Colors.Contains(col) Then ACLists.Colors.Add(col)
                                                End If
                                            Else
                                                value = Trim(Mid(fLine, fLine.IndexOf("0x") + 3, 8))
                                                If IsHex(value) Then
                                                    col = New PawnColor(Trim(Mid(fLine, fLine.IndexOf("#define") + 9, If(fLine.IndexOf(" ", fLine.IndexOf(" ") + 2) > 0, fLine.IndexOf(" ", fLine.IndexOf(" ") + 2) - fLine.IndexOf(" ") - 1, fLine.IndexOf(vbTab.ToString()) - fLine.IndexOf(" ") - 1))), Color.FromArgb(Integer.Parse(Mid(value, 7, 2), Globalization.NumberStyles.HexNumber), Integer.Parse(Mid(value, 1, 2), Globalization.NumberStyles.HexNumber), Integer.Parse(Mid(value, 3, 2), Globalization.NumberStyles.HexNumber), Integer.Parse(Mid(value, 5, 2), Globalization.NumberStyles.HexNumber)), -1)
                                                    If Not ACLists.Colors.Contains(col) Then ACLists.Colors.Add(col)
                                                End If
                                            End If
                                        Else
                                            If fLine.IndexOf("""") > -1 AndAlso fLine.IndexOf("""", fLine.IndexOf("""") + 1) > -1 Then
                                                Dim value As String
                                                If fLine.IndexOf("{") > -1 AndAlso fLine.IndexOf("}") > -1 Then
                                                    value = Trim(Mid(fLine, fLine.IndexOf("{") + 2, 6))
                                                Else
                                                    If fLine.IndexOf("{") > -1 AndAlso fLine.IndexOf("}") > -1 Then
                                                        value = Trim(Mid(fLine, fLine.IndexOf("{") + 2, 6))
                                                    Else
                                                        value = Trim(Mid(fLine, fLine.IndexOf("""") + 2, 6))
                                                    End If
                                                End If
                                                If IsHex(value) Then
                                                    Dim col As New PawnColor(Trim(Mid(fLine, fLine.IndexOf("#define") + 9, If(fLine.IndexOf(" ", fLine.IndexOf(" ") + 2) > 0, fLine.IndexOf(" ", fLine.IndexOf(" ") + 2) - fLine.IndexOf(" ") - 1, fLine.IndexOf(vbTab.ToString()) - fLine.IndexOf(" ") - 1))), Color.FromArgb(255, Integer.Parse(Mid(value, 1, 2), Globalization.NumberStyles.HexNumber), Integer.Parse(Mid(value, 3, 2), Globalization.NumberStyles.HexNumber), Integer.Parse(Mid(value, 5, 2), Globalization.NumberStyles.HexNumber)), -1)
                                                    If col.Name.Length > 0 AndAlso Not ACLists.eColors.Contains(col) Then ACLists.eColors.Add(col)
                                                End If
                                            End If
                                        End If
                                    ElseIf fLine.IndexOf("Menu:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                                        If fLine.IndexOf(",") = -1 Then
                                            If fLine.IndexOf(";") = -1 Then
                                                tmp = Trim(Mid(fLine, fLine.IndexOf("Menu:") + 6, fLine.Length - fLine.IndexOf("Menu:") - 5))
                                            Else
                                                tmp = Trim(Mid(fLine, fLine.IndexOf("Menu:") + 6, fLine.IndexOf(";") - fLine.IndexOf("Menu:") - 5))
                                            End If
                                            If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Menus.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                        Else
                                            Dim fround As Boolean = True, oldpos As Integer, pos As Integer = fLine.IndexOf(",")
                                            While pos > -1
                                                If fround Then
                                                    tmp = Trim(Mid(fLine, fLine.IndexOf("Menu:") + 6, pos - fLine.IndexOf("Menu:") - 5))
                                                    fround = False
                                                Else
                                                    tmp = Trim(Mid(fLine, fLine.IndexOf("Menu:", oldpos) + 6, pos - fLine.IndexOf("Menu:", oldpos) - 5))
                                                End If
                                                oldpos = pos
                                                pos = fLine.IndexOf(",", pos + 1)
                                                If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Menus.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                            End While
                                            If fLine.IndexOf(";") > -1 Then
                                                pos = fLine.LastIndexOf("Menu:")
                                                tmp = Trim(Mid(fLine, pos + 6, fLine.IndexOf(";") - pos - 5))
                                                If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Menus.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                            End If
                                        End If
                                    ElseIf fLine.IndexOf("Text:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                                        If fLine.IndexOf(",") = -1 Then
                                            If fLine.IndexOf(";") = -1 Then
                                                tmp = Trim(Mid(fLine, fLine.IndexOf("Text:") + 6, fLine.Length - fLine.IndexOf("Text:") - 5))
                                            Else
                                                tmp = Trim(Mid(fLine, fLine.IndexOf("Text:") + 6, fLine.IndexOf(";") - fLine.IndexOf("Text:") - 5))
                                            End If
                                            If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Texts.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                        Else
                                            Dim fround As Boolean = True, oldpos As Integer, pos As Integer = fLine.IndexOf(",")
                                            While pos > -1
                                                If fround Then
                                                    tmp = Trim(Mid(fLine, fLine.IndexOf("Text:") + 6, pos - fLine.IndexOf("Text:") - 5))
                                                    fround = False
                                                Else
                                                    tmp = Trim(Mid(fLine, fLine.IndexOf("Text:", oldpos) + 6, pos - fLine.IndexOf("Text:", oldpos) - 5))
                                                End If
                                                oldpos = pos
                                                pos = fLine.IndexOf(",", pos + 1)
                                                If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Texts.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                            End While
                                            If fLine.IndexOf(";") > -1 Then
                                                pos = fLine.LastIndexOf("Text:")
                                                tmp = Trim(Mid(fLine, pos + 6, fLine.IndexOf(";") - pos - 5))
                                                If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Texts.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                            End If
                                        End If
                                    ElseIf fLine.IndexOf("Text3D:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                                        If fLine.IndexOf(",") = -1 Then
                                            If fLine.IndexOf(";") = -1 Then
                                                tmp = Trim(Mid(fLine, fLine.IndexOf("Text3D:") + 8, fLine.Length - fLine.IndexOf("Text3D:") - 7))
                                            Else
                                                tmp = Trim(Mid(fLine, fLine.IndexOf("Text3D:") + 8, fLine.IndexOf(";") - fLine.IndexOf("Text3D:") - 7))
                                            End If
                                            If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Texts2.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                        Else
                                            Dim fround As Boolean = True, oldpos As Integer, pos As Integer = fLine.IndexOf(",")
                                            While pos > -1
                                                If fround Then
                                                    tmp = Trim(Mid(fLine, fLine.IndexOf("Text3D:") + 8, pos - fLine.IndexOf("Text3D:") - 7))
                                                    fround = False
                                                Else
                                                    tmp = Trim(Mid(fLine, fLine.IndexOf("Text3D:", oldpos) + 8, pos - fLine.IndexOf("Text3D:", oldpos) - 7))
                                                End If
                                                oldpos = pos
                                                pos = fLine.IndexOf(",", pos + 1)
                                                If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Texts2.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                            End While
                                            If fLine.IndexOf(";") > -1 Then
                                                pos = fLine.LastIndexOf("Text3D:")
                                                tmp = Trim(Mid(fLine, pos + 8, fLine.IndexOf(";") - pos - 7))
                                                If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Texts2.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                            End If
                                        End If
                                    ElseIf fLine.IndexOf("Float:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                                        If fLine.IndexOf(",") = -1 Then
                                            If fLine.IndexOf(";") = -1 Then
                                                tmp = Trim(Mid(fLine, fLine.IndexOf("Float:") + 7, fLine.Length - fLine.IndexOf("Float:") - 6))
                                            Else
                                                tmp = Trim(Mid(fLine, fLine.IndexOf("Float:") + 7, fLine.IndexOf(";") - fLine.IndexOf("Float:") - 6))
                                            End If
                                            If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Floats.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                        Else
                                            Dim fround As Boolean = True, oldpos As Integer, pos As Integer = fLine.IndexOf(",")
                                            While pos > -1
                                                If fround Then
                                                    tmp = Trim(Mid(fLine, fLine.IndexOf("Float:") + 7, pos - fLine.IndexOf("Float:") - 6))
                                                    fround = False
                                                Else
                                                    tmp = Trim(Mid(fLine, fLine.IndexOf("Float:", oldpos) + 7, pos - fLine.IndexOf("Float:", oldpos) - 6))
                                                End If
                                                oldpos = pos
                                                pos = fLine.IndexOf(",", pos + 1)
                                                If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Floats.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                            End While
                                            If fLine.IndexOf(";") > -1 Then
                                                pos = fLine.LastIndexOf("Float:")
                                                tmp = Trim(Mid(fLine, pos + 7, fLine.IndexOf(";") - pos - 6))
                                                If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Floats.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                            End If
                                        End If
                                    ElseIf fLine.IndexOf("DB:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                                        If fLine.IndexOf(",") = -1 Then
                                            If fLine.IndexOf(";") = -1 Then
                                                tmp = Trim(Mid(fLine, fLine.IndexOf("DB:") + 3, fLine.Length - fLine.IndexOf("DB:") - 2))
                                            Else
                                                tmp = Trim(Mid(fLine, fLine.IndexOf("DB:") + 3, fLine.IndexOf(";") - fLine.IndexOf("DB:") - 2))
                                            End If
                                            If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Dbs.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                        Else
                                            Dim fround As Boolean = True, oldpos As Integer, pos As Integer = fLine.IndexOf(",")
                                            While pos > -1
                                                If fround Then
                                                    tmp = Trim(Mid(fLine, fLine.IndexOf("DB:") + 3, pos - fLine.IndexOf("DB:") - 2))
                                                    fround = False
                                                Else
                                                    tmp = Trim(Mid(fLine, fLine.IndexOf("DB:", oldpos) + 3, pos - fLine.IndexOf("DB:", oldpos) - 2))
                                                End If
                                                oldpos = pos
                                                pos = fLine.IndexOf(",", pos + 1)
                                                If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Dbs.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                            End While
                                            If fLine.IndexOf(";") > -1 Then
                                                pos = fLine.LastIndexOf("DB:")
                                                tmp = Trim(Mid(fLine, pos + 3, fLine.IndexOf(";") - pos - 2))
                                                If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Dbs.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                            End If
                                        End If
                                    ElseIf fLine.IndexOf("DBResult:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                                        If fLine.IndexOf(",") = -1 Then
                                            If fLine.IndexOf(";") = -1 Then
                                                tmp = Trim(Mid(fLine, fLine.IndexOf("DBResult:") + 10, fLine.Length - fLine.IndexOf("DBResult:") - 8))
                                            Else
                                                tmp = Trim(Mid(fLine, fLine.IndexOf("DBResult:") + 10, fLine.IndexOf(";") - fLine.IndexOf("DBResult:") - 8))
                                            End If
                                            If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.DbRes.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                        Else
                                            Dim fround As Boolean = True, oldpos As Integer, pos As Integer = fLine.IndexOf(",")
                                            While pos > -1
                                                If fround Then
                                                    tmp = Trim(Mid(fLine, fLine.IndexOf("DBResult:") + 10, pos - fLine.IndexOf("DBResult:") - 8))
                                                    fround = False
                                                Else
                                                    tmp = Trim(Mid(fLine, fLine.IndexOf("DBResult:", oldpos) + 10, pos - fLine.IndexOf("DBResult:", oldpos) - 8))
                                                End If
                                                oldpos = pos
                                                pos = fLine.IndexOf(",", pos + 1)
                                                If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.DbRes.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                            End While
                                            If fLine.IndexOf(";") > -1 Then
                                                pos = fLine.LastIndexOf("DBResult:")
                                                tmp = Trim(Mid(fLine, pos + 10, fLine.IndexOf(";") - pos - 8))
                                                If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.DbRes.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                            End If
                                        End If
                                    ElseIf fLine.IndexOf("File:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                                        If fLine.IndexOf(",") = -1 Then
                                            If fLine.IndexOf(";") = -1 Then
                                                tmp = Trim(Mid(fLine, fLine.IndexOf("File:") + 6, fLine.Length - fLine.IndexOf("File:") - 5))
                                            Else
                                                tmp = Trim(Mid(fLine, fLine.IndexOf("File:") + 6, fLine.IndexOf(";") - fLine.IndexOf("File:") - 5))
                                            End If
                                            If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Files.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                        Else
                                            Dim fround As Boolean = True, oldpos As Integer, pos As Integer = fLine.IndexOf(",")
                                            While pos > -1
                                                If fround Then
                                                    tmp = Trim(Mid(fLine, fLine.IndexOf("File:") + 6, pos - fLine.IndexOf("File:") - 5))
                                                    fround = False
                                                Else
                                                    tmp = Trim(Mid(fLine, fLine.IndexOf("File:", oldpos) + 6, pos - fLine.IndexOf("File:", oldpos) - 5))
                                                End If
                                                oldpos = pos
                                                pos = fLine.IndexOf(",", pos + 1)
                                                If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Files.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                            End While
                                            If fLine.IndexOf(";") > -1 Then
                                                pos = fLine.LastIndexOf("File:")
                                                tmp = Trim(Mid(fLine, pos + 6, fLine.IndexOf(";") - pos - 5))
                                                If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Files.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                            End If
                                        End If
                                    End If
                                    count += 1
                                    fLine = Reader2.ReadLine()
                                Loop
                                Reader2.Close()
                            Else
                                Errors.Clear()
                                Errors.Add(New ListViewItem(New String() {"", "100", Name, count, "cannot read from file: """ & file2 & """"}, 1))
                            End If
                        ElseIf spos > -1 AndAlso fLine.IndexOf("(") > -1 AndAlso fLine.IndexOf(")") > -1 AndAlso fLine.IndexOf("operator") = -1 Then
                            Dim params As New List(Of String)
                            params.AddRange(Split(Trim(Mid(fLine, fLine.IndexOf("(") + 2, fLine.IndexOf(")") - fLine.IndexOf("(") - 1)), ","))
                            For i = 0 To params.Count - 1
                                If i > 0 AndAlso params(i).Length > 0 AndAlso params(i).IndexOf("...") > -1 Then
                                    params(i - 1) += "," & params(i)
                                    params.RemoveAt(i)
                                    Continue For
                                End If
                            Next
                            Dim func As PawnFunction = New PawnFunction(Trim(Mid(fLine, fLine.IndexOf(" ", spos) + 2, fLine.IndexOf("(") - fLine.IndexOf(" ", spos) - 1)).Replace("Float:", "").Replace("bool:", ""), file.Replace(".inc", ":"), -1, params.ToArray)
                            If Not TrueContainsFunction(ACLists.Functions, func, True) AndAlso Not TrueContainsFunction(ACLists.Callbacks, func) Then ACLists.Functions.Add(func)
                        ElseIf fLine.IndexOf("forward") > -1 AndAlso fLine.IndexOf("(") > -1 AndAlso fLine.IndexOf(")") > -1 Then
                            Dim func As PawnFunction = New PawnFunction(Trim(Mid(fLine, fLine.IndexOf(" ") + 1, fLine.IndexOf("(") - fLine.IndexOf(" "))), file.Replace(".inc", ":"), -1, Split(Trim(Mid(fLine, fLine.IndexOf("(") + 2, fLine.IndexOf(")") - fLine.IndexOf("(") - 1)), ","))
                            If Not TrueContainsFunction(ACLists.Callbacks, func, True) Then ACLists.Callbacks.Add(func)
                        ElseIf fLine.IndexOf("#define") > -1 Then
                            If fLine.IndexOf("0x") > -1 Then
                                Dim col As PawnColor, value As String
                                If fLine.IndexOf("(") > -1 AndAlso fLine.IndexOf(")") > -1 Then
                                    value = Trim(Mid(fLine, fLine.IndexOf("(") + 4, 8))
                                    If IsHex(value) Then
                                        col = New PawnColor(Trim(Mid(fLine, fLine.IndexOf("#define") + 9, If(fLine.IndexOf(" ", fLine.IndexOf(" ") + 2) > 0, fLine.IndexOf(" ", fLine.IndexOf(" ") + 2) - fLine.IndexOf(" ") - 1, fLine.IndexOf(vbTab.ToString()) - fLine.IndexOf(" ") - 1))), Color.FromArgb(Integer.Parse(Mid(value, 7, 2), Globalization.NumberStyles.HexNumber), Integer.Parse(Mid(value, 1, 2), Globalization.NumberStyles.HexNumber), Integer.Parse(Mid(value, 3, 2), Globalization.NumberStyles.HexNumber), Integer.Parse(Mid(value, 5, 2), Globalization.NumberStyles.HexNumber)), -1)
                                        If Not ACLists.Colors.Contains(col) Then ACLists.Colors.Add(col)
                                    End If
                                Else
                                    value = Trim(Mid(fLine, fLine.IndexOf("0x") + 3, 8))
                                    If IsHex(value) Then
                                        col = New PawnColor(Trim(Mid(fLine, fLine.IndexOf("#define") + 9, If(fLine.IndexOf(" ", fLine.IndexOf(" ") + 2) > 0, fLine.IndexOf(" ", fLine.IndexOf(" ") + 2) - fLine.IndexOf(" ") - 1, fLine.IndexOf(vbTab.ToString()) - fLine.IndexOf(" ") - 1))), Color.FromArgb(Integer.Parse(Mid(value, 7, 2), Globalization.NumberStyles.HexNumber), Integer.Parse(Mid(value, 1, 2), Globalization.NumberStyles.HexNumber), Integer.Parse(Mid(value, 3, 2), Globalization.NumberStyles.HexNumber), Integer.Parse(Mid(value, 5, 2), Globalization.NumberStyles.HexNumber)), -1)
                                        If Not ACLists.Colors.Contains(col) Then ACLists.Colors.Add(col)
                                    End If
                                End If
                            Else
                                If fLine.IndexOf("""") > -1 AndAlso fLine.IndexOf("""", fLine.IndexOf("""") + 1) > -1 Then
                                    Dim value As String
                                    If fLine.IndexOf("{") > -1 AndAlso fLine.IndexOf("}") > -1 Then
                                        value = Trim(Mid(fLine, fLine.IndexOf("{") + 2, 6))
                                    Else
                                        If fLine.IndexOf("{") > -1 AndAlso fLine.IndexOf("}") > -1 Then
                                            value = Trim(Mid(fLine, fLine.IndexOf("{") + 2, 6))
                                        Else
                                            value = Trim(Mid(fLine, fLine.IndexOf("""") + 2, 6))
                                        End If
                                    End If
                                    If IsHex(value) Then
                                        Dim col As New PawnColor(Trim(Mid(fLine, fLine.IndexOf("#define") + 9, If(fLine.IndexOf(" ", fLine.IndexOf(" ") + 2) > 0, fLine.IndexOf(" ", fLine.IndexOf(" ") + 2) - fLine.IndexOf(" ") - 1, fLine.IndexOf(vbTab.ToString()) - fLine.IndexOf(" ") - 1))), Color.FromArgb(255, Integer.Parse(Mid(value, 1, 2), Globalization.NumberStyles.HexNumber), Integer.Parse(Mid(value, 3, 2), Globalization.NumberStyles.HexNumber), Integer.Parse(Mid(value, 5, 2), Globalization.NumberStyles.HexNumber)), -1)
                                        If col.Name.Length > 0 AndAlso Not ACLists.eColors.Contains(col) Then ACLists.eColors.Add(col)
                                    End If
                                End If
                            End If
                        ElseIf fLine.IndexOf("Menu:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                            If fLine.IndexOf(",") = -1 Then
                                If fLine.IndexOf(";") = -1 Then
                                    tmp = Trim(Mid(fLine, fLine.IndexOf("Menu:") + 6, fLine.Length - fLine.IndexOf("Menu:") - 5))
                                Else
                                    tmp = Trim(Mid(fLine, fLine.IndexOf("Menu:") + 6, fLine.IndexOf(";") - fLine.IndexOf("Menu:") - 5))
                                End If
                                If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Menus.Contains(tmp) Then ACLists.Floats.Add(tmp)
                            Else
                                Dim fround As Boolean = True, oldpos As Integer, pos As Integer = fLine.IndexOf(",")
                                While pos > -1
                                    If fround Then
                                        tmp = Trim(Mid(fLine, fLine.IndexOf("Menu:") + 6, pos - fLine.IndexOf("Menu:") - 5))
                                        fround = False
                                    Else
                                        tmp = Trim(Mid(fLine, fLine.IndexOf("Menu:", oldpos) + 6, pos - fLine.IndexOf("Menu:", oldpos) - 5))
                                    End If
                                    oldpos = pos
                                    pos = fLine.IndexOf(",", pos + 1)
                                    If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Menus.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                End While
                                If fLine.IndexOf(";") > -1 Then
                                    pos = fLine.LastIndexOf("Menu:")
                                    tmp = Trim(Mid(fLine, pos + 6, fLine.IndexOf(";") - pos - 5))
                                    If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Menus.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                End If
                            End If
                        ElseIf fLine.IndexOf("Text:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                            If fLine.IndexOf(",") = -1 Then
                                If fLine.IndexOf(";") = -1 Then
                                    tmp = Trim(Mid(fLine, fLine.IndexOf("Text:") + 6, fLine.Length - fLine.IndexOf("Text:") - 5))
                                Else
                                    tmp = Trim(Mid(fLine, fLine.IndexOf("Text:") + 6, fLine.IndexOf(";") - fLine.IndexOf("Text:") - 5))
                                End If
                                If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Texts.Contains(tmp) Then ACLists.Floats.Add(tmp)
                            Else
                                Dim fround As Boolean = True, oldpos As Integer, pos As Integer = fLine.IndexOf(",")
                                While pos > -1
                                    If fround Then
                                        tmp = Trim(Mid(fLine, fLine.IndexOf("Text:") + 6, pos - fLine.IndexOf("Text:") - 5))
                                        fround = False
                                    Else
                                        tmp = Trim(Mid(fLine, fLine.IndexOf("Text:", oldpos) + 6, pos - fLine.IndexOf("Text:", oldpos) - 5))
                                    End If
                                    oldpos = pos
                                    pos = fLine.IndexOf(",", pos + 1)
                                    If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Texts.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                End While
                                If fLine.IndexOf(";") > -1 Then
                                    pos = fLine.LastIndexOf("Text:")
                                    tmp = Trim(Mid(fLine, pos + 6, fLine.IndexOf(";") - pos - 5))
                                    If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Texts.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                End If
                            End If
                        ElseIf fLine.IndexOf("Text3D:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                            If fLine.IndexOf(",") = -1 Then
                                If fLine.IndexOf(";") = -1 Then
                                    tmp = Trim(Mid(fLine, fLine.IndexOf("Text3D:") + 8, fLine.Length - fLine.IndexOf("Text3D:") - 7))
                                Else
                                    tmp = Trim(Mid(fLine, fLine.IndexOf("Text3D:") + 8, fLine.IndexOf(";") - fLine.IndexOf("Text3D:") - 7))
                                End If
                                If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Texts2.Contains(tmp) Then ACLists.Floats.Add(tmp)
                            Else
                                Dim fround As Boolean = True, oldpos As Integer, pos As Integer = fLine.IndexOf(",")
                                While pos > -1
                                    If fround Then
                                        tmp = Trim(Mid(fLine, fLine.IndexOf("Text3D:") + 8, pos - fLine.IndexOf("Text3D:") - 7))
                                        fround = False
                                    Else
                                        tmp = Trim(Mid(fLine, fLine.IndexOf("Text3D:", oldpos) + 8, pos - fLine.IndexOf("Text3D:", oldpos) - 7))
                                    End If
                                    oldpos = pos
                                    pos = fLine.IndexOf(",", pos + 1)
                                    If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Texts2.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                End While
                                If fLine.IndexOf(";") > -1 Then
                                    pos = fLine.LastIndexOf("Text3D:")
                                    tmp = Trim(Mid(fLine, pos + 8, fLine.IndexOf(";") - pos - 7))
                                    If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Texts2.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                End If
                            End If
                        ElseIf fLine.IndexOf("Float:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                            If fLine.IndexOf(",") = -1 Then
                                If fLine.IndexOf(";") = -1 Then
                                    tmp = Trim(Mid(fLine, fLine.IndexOf("Float:") + 7, fLine.Length - fLine.IndexOf("Float:") - 6))
                                Else
                                    tmp = Trim(Mid(fLine, fLine.IndexOf("Float:") + 7, fLine.IndexOf(";") - fLine.IndexOf("Float:") - 6))
                                End If
                                If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Floats.Contains(tmp) Then ACLists.Floats.Add(tmp)
                            Else
                                Dim fround As Boolean = True, oldpos As Integer, pos As Integer = fLine.IndexOf(",")
                                While pos > -1
                                    If fround Then
                                        tmp = Trim(Mid(fLine, fLine.IndexOf("Float:") + 7, pos - fLine.IndexOf("Float:") - 6))
                                        fround = False
                                    Else
                                        tmp = Trim(Mid(fLine, fLine.IndexOf("Float:", oldpos) + 7, pos - fLine.IndexOf("Float:", oldpos) - 6))
                                    End If
                                    oldpos = pos
                                    pos = fLine.IndexOf(",", pos + 1)
                                    If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Floats.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                End While
                                If fLine.IndexOf(";") > -1 Then
                                    pos = fLine.LastIndexOf("Float:")
                                    tmp = Trim(Mid(fLine, pos + 7, fLine.IndexOf(";") - pos - 6))
                                    If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Floats.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                End If
                            End If
                        ElseIf fLine.IndexOf("DB:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                            If fLine.IndexOf(",") = -1 Then
                                If fLine.IndexOf(";") = -1 Then
                                    tmp = Trim(Mid(fLine, fLine.IndexOf("DB:") + 3, fLine.Length - fLine.IndexOf("DB:") - 2))
                                Else
                                    tmp = Trim(Mid(fLine, fLine.IndexOf("DB:") + 3, fLine.IndexOf(";") - fLine.IndexOf("DB:") - 2))
                                End If
                                If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Dbs.Contains(tmp) Then ACLists.Floats.Add(tmp)
                            Else
                                Dim fround As Boolean = True, oldpos As Integer, pos As Integer = fLine.IndexOf(",")
                                While pos > -1
                                    If fround Then
                                        tmp = Trim(Mid(fLine, fLine.IndexOf("DB:") + 3, pos - fLine.IndexOf("DB:") - 2))
                                        fround = False
                                    Else
                                        tmp = Trim(Mid(fLine, fLine.IndexOf("DB:", oldpos) + 3, pos - fLine.IndexOf("DB:", oldpos) - 2))
                                    End If
                                    oldpos = pos
                                    pos = fLine.IndexOf(",", pos + 1)
                                    If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Dbs.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                End While
                                If fLine.IndexOf(";") > -1 Then
                                    pos = fLine.LastIndexOf("DB:")
                                    tmp = Trim(Mid(fLine, pos + 3, fLine.IndexOf(";") - pos - 2))
                                    If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Dbs.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                End If
                            End If
                        ElseIf fLine.IndexOf("DBResult:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                            If fLine.IndexOf(",") = -1 Then
                                If fLine.IndexOf(";") = -1 Then
                                    tmp = Trim(Mid(fLine, fLine.IndexOf("DBResult:") + 10, fLine.Length - fLine.IndexOf("DBResult:") - 8))
                                Else
                                    tmp = Trim(Mid(fLine, fLine.IndexOf("DBResult:") + 10, fLine.IndexOf(";") - fLine.IndexOf("DBResult:") - 8))
                                End If
                                If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.DbRes.Contains(tmp) Then ACLists.Floats.Add(tmp)
                            Else
                                Dim fround As Boolean = True, oldpos As Integer, pos As Integer = fLine.IndexOf(",")
                                While pos > -1
                                    If fround Then
                                        tmp = Trim(Mid(fLine, fLine.IndexOf("DBResult:") + 10, pos - fLine.IndexOf("DBResult:") - 8))
                                        fround = False
                                    Else
                                        tmp = Trim(Mid(fLine, fLine.IndexOf("DBResult:", oldpos) + 10, pos - fLine.IndexOf("DBResult:", oldpos) - 8))
                                    End If
                                    oldpos = pos
                                    pos = fLine.IndexOf(",", pos + 1)
                                    If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.DbRes.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                End While
                                If fLine.IndexOf(";") > -1 Then
                                    pos = fLine.LastIndexOf("DBResult:")
                                    tmp = Trim(Mid(fLine, pos + 10, fLine.IndexOf(";") - pos - 8))
                                    If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.DbRes.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                End If
                            End If
                        ElseIf fLine.IndexOf("File:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                            If fLine.IndexOf(",") = -1 Then
                                If fLine.IndexOf(";") = -1 Then
                                    tmp = Trim(Mid(fLine, fLine.IndexOf("File:") + 6, fLine.Length - fLine.IndexOf("File:") - 5))
                                Else
                                    tmp = Trim(Mid(fLine, fLine.IndexOf("File:") + 6, fLine.IndexOf(";") - fLine.IndexOf("File:") - 5))
                                End If
                                If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Files.Contains(tmp) Then ACLists.Floats.Add(tmp)
                            Else
                                Dim fround As Boolean = True, oldpos As Integer, pos As Integer = fLine.IndexOf(",")
                                While pos > -1
                                    If fround Then
                                        tmp = Trim(Mid(fLine, fLine.IndexOf("File:") + 6, pos - fLine.IndexOf("File:") - 5))
                                        fround = False
                                    Else
                                        tmp = Trim(Mid(fLine, fLine.IndexOf("File:", oldpos) + 6, pos - fLine.IndexOf("File:", oldpos) - 5))
                                    End If
                                    oldpos = pos
                                    pos = fLine.IndexOf(",", pos + 1)
                                    If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Files.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                End While
                                If fLine.IndexOf(";") > -1 Then
                                    pos = fLine.LastIndexOf("File:")
                                    tmp = Trim(Mid(fLine, pos + 6, fLine.IndexOf(";") - pos - 5))
                                    If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Files.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                End If
                            End If
                        End If
                        fLine = Reader.ReadLine()
                    Loop
                    Reader.Close()
                Else
                    Errors.Clear()
                    Errors.Add(New ListViewItem(New String() {"", "100", Name, line.Number + 1, "cannot read from file: """ & file & """"}, 0))
                End If
            ElseIf line.Text.IndexOf("#tryinclude") > -1 Then
                Dim file As String
                If line.Text.IndexOf("<") > -1 Then
                    file = Mid(line.Text, line.Text.IndexOf("<") + 2, line.Text.IndexOf(">") - line.Text.IndexOf("<") - 1)
                    file += ".inc"
                Else
                    file = Mid(line.Text, line.Text.IndexOf("""") + 2, line.Text.IndexOf("""") - line.Text.IndexOf("""", line.Text.IndexOf("""")))
                    If file.IndexOf(".inc") = -1 Then file += ".inc"
                End If
                If IO.File.Exists(My.Application.Info.DirectoryPath & "\Include\" & file) Then
                    Dim fLine As String, Reader As New StreamReader(My.Application.Info.DirectoryPath & "\Include\" & file)
                    fLine = Reader.ReadLine()
                    Do Until fLine Is Nothing
                        If fLine.Length = 0 OrElse fLine = "{" OrElse fLine = "}" OrElse fLine = ";" Then
                            fLine = Reader.ReadLine()
                            Continue Do
                        ElseIf fLine.StartsWith("//") Then
                            CommentedLine = True
                        ElseIf fLine.IndexOf("/*") > -1 AndAlso fLine.IndexOf("*/") = -1 Then
                            CommentedSection = True
                        ElseIf fLine.IndexOf("*/") > -1 Then
                            CommentedSection = False
                        End If
                        If CommentedLine Or CommentedSection Then
                            CommentedLine = False
                            fLine = Reader.ReadLine()
                            Continue Do
                        End If
                        spos = fLine.IndexOf("native")
                        If spos = -1 Then
                            spos = fLine.IndexOf("stock")
                            If spos = -1 Then spos = fLine.IndexOf("public")
                        End If
                        If fLine.IndexOf("#include") > -1 Then
                            Dim file2 As String, cNode2 As New TreeNode()
                            If fLine.IndexOf("<") > -1 Then
                                file2 = Mid(fLine, fLine.IndexOf("<") + 2, fLine.IndexOf(">") - fLine.IndexOf("<") - 1)
                                file2 += ".inc"
                            Else
                                file2 = Mid(fLine, fLine.IndexOf("""") + 2, fLine.IndexOf("""") - fLine.IndexOf("""", fLine.IndexOf("""")))
                                If file2.IndexOf(".inc") = -1 Then file2 += ".inc"
                            End If
                            Dim count As Integer
                            If IO.File.Exists(My.Application.Info.DirectoryPath & "\Include\" & file2) Then
                                Dim Reader2 As New StreamReader(My.Application.Info.DirectoryPath & "\Include\" & file2)
                                fLine = Reader2.ReadLine()
                                Do Until fLine Is Nothing
                                    If fLine.Length = 0 OrElse fLine = "{" OrElse fLine = "}" OrElse fLine = ";" Then
                                        count += 1
                                        fLine = Reader2.ReadLine()
                                        Continue Do
                                    ElseIf fLine.StartsWith("//") Then
                                        CommentedLine = True
                                    ElseIf fLine.IndexOf("/*") > -1 AndAlso fLine.IndexOf("*/") = -1 Then
                                        CommentedSection = True
                                    ElseIf fLine.IndexOf("*/") > -1 Then
                                        CommentedSection = False
                                    End If
                                    If CommentedLine Or CommentedSection Then
                                        CommentedLine = False
                                        count += 1
                                        fLine = Reader2.ReadLine()
                                        Continue Do
                                    End If
                                    spos = fLine.IndexOf("native")
                                    If spos = -1 Then
                                        spos = fLine.IndexOf("stock")
                                        If spos = -1 Then spos = fLine.IndexOf("public")
                                    End If
                                    If spos > -1 AndAlso fLine.IndexOf("(") > -1 AndAlso fLine.IndexOf(")") > -1 AndAlso fLine.IndexOf("operator") = -1 Then
                                        Dim params As New List(Of String)
                                        params.AddRange(Split(Trim(Mid(fLine, fLine.IndexOf("(") + 2, fLine.IndexOf(")") - fLine.IndexOf("(") - 1)), ","))
                                        For i = 0 To params.Count - 1
                                            If i > 0 AndAlso params(i).Length > 0 AndAlso params(i).IndexOf("...") > -1 Then
                                                params(i - 1) += "," & params(i)
                                                params.RemoveAt(i)
                                                Continue For
                                            End If
                                        Next
                                        Dim func As PawnFunction = New PawnFunction(Trim(Mid(fLine, fLine.IndexOf(" ", spos) + 2, fLine.IndexOf("(") - fLine.IndexOf(" ", spos) - 1)).Replace("Float:", "").Replace("bool:", ""), file2.Replace(".inc", ":"), -1, params.ToArray)
                                        If Not TrueContainsFunction(ACLists.Functions, func, True) AndAlso Not TrueContainsFunction(ACLists.Callbacks, func) Then ACLists.Functions.Add(func)
                                    ElseIf fLine.IndexOf("forward") > -1 AndAlso fLine.IndexOf("(") > -1 AndAlso fLine.IndexOf(")") > -1 Then
                                        Dim func As PawnFunction = New PawnFunction(Trim(Mid(fLine, fLine.IndexOf(" ") + 1, fLine.IndexOf("(") - fLine.IndexOf(" "))), file2.Replace(".inc", ":"), -1, Split(Trim(Mid(fLine, fLine.IndexOf("(") + 2, fLine.IndexOf(")") - fLine.IndexOf("(") - 1)), ","))
                                        If Not TrueContainsFunction(ACLists.Callbacks, func, True) Then ACLists.Callbacks.Add(func)
                                    ElseIf fLine.IndexOf("#define") > -1 Then
                                        If fLine.IndexOf("0x") > -1 Then
                                            Dim col As PawnColor, value As String
                                            If fLine.IndexOf("(") > -1 AndAlso fLine.IndexOf(")") > -1 Then
                                                value = Trim(Mid(fLine, fLine.IndexOf("(") + 4, 8))
                                                If IsHex(value) Then
                                                    col = New PawnColor(Trim(Mid(fLine, fLine.IndexOf("#define") + 9, If(fLine.IndexOf(" ", fLine.IndexOf(" ") + 2) > 0, fLine.IndexOf(" ", fLine.IndexOf(" ") + 2) - fLine.IndexOf(" ") - 1, fLine.IndexOf(vbTab.ToString()) - fLine.IndexOf(" ") - 1))), Color.FromArgb(Integer.Parse(Mid(value, 7, 2), Globalization.NumberStyles.HexNumber), Integer.Parse(Mid(value, 1, 2), Globalization.NumberStyles.HexNumber), Integer.Parse(Mid(value, 3, 2), Globalization.NumberStyles.HexNumber), Integer.Parse(Mid(value, 5, 2), Globalization.NumberStyles.HexNumber)), -1)
                                                    If Not ACLists.Colors.Contains(col) Then ACLists.Colors.Add(col)
                                                End If
                                            Else
                                                value = Trim(Mid(fLine, fLine.IndexOf("0x") + 3, 8))
                                                If IsHex(value) Then
                                                    col = New PawnColor(Trim(Mid(fLine, fLine.IndexOf("#define") + 9, If(fLine.IndexOf(" ", fLine.IndexOf(" ") + 2) > 0, fLine.IndexOf(" ", fLine.IndexOf(" ") + 2) - fLine.IndexOf(" ") - 1, fLine.IndexOf(vbTab.ToString()) - fLine.IndexOf(" ") - 1))), Color.FromArgb(Integer.Parse(Mid(value, 7, 2), Globalization.NumberStyles.HexNumber), Integer.Parse(Mid(value, 1, 2), Globalization.NumberStyles.HexNumber), Integer.Parse(Mid(value, 3, 2), Globalization.NumberStyles.HexNumber), Integer.Parse(Mid(value, 5, 2), Globalization.NumberStyles.HexNumber)), -1)
                                                    If Not ACLists.Colors.Contains(col) Then ACLists.Colors.Add(col)
                                                End If
                                            End If
                                        Else
                                            If fLine.IndexOf("""") > -1 AndAlso fLine.IndexOf("""", fLine.IndexOf("""") + 1) > -1 Then
                                                Dim value As String
                                                If fLine.IndexOf("{") > -1 AndAlso fLine.IndexOf("}") > -1 Then
                                                    value = Trim(Mid(fLine, fLine.IndexOf("{") + 2, 6))
                                                Else
                                                    value = Trim(Mid(fLine, fLine.IndexOf("""") + 2, 6))
                                                End If
                                                If IsHex(value) Then
                                                    Dim col As New PawnColor(Trim(Mid(fLine, fLine.IndexOf("#define") + 9, If(fLine.IndexOf(" ", fLine.IndexOf(" ") + 2) > 0, fLine.IndexOf(" ", fLine.IndexOf(" ") + 2) - fLine.IndexOf(" ") - 1, fLine.IndexOf(vbTab.ToString()) - fLine.IndexOf(" ") - 1))), Color.FromArgb(255, Integer.Parse(Mid(value, 1, 2), Globalization.NumberStyles.HexNumber), Integer.Parse(Mid(value, 3, 2), Globalization.NumberStyles.HexNumber), Integer.Parse(Mid(value, 5, 2), Globalization.NumberStyles.HexNumber)), -1)
                                                    If col.Name.Length > 0 AndAlso Not ACLists.eColors.Contains(col) Then ACLists.eColors.Add(col)
                                                End If
                                            End If
                                        End If
                                    ElseIf fLine.IndexOf("Menu:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                                        If fLine.IndexOf(",") = -1 Then
                                            If fLine.IndexOf(";") = -1 Then
                                                tmp = Trim(Mid(fLine, fLine.IndexOf("Menu:") + 6, fLine.Length - fLine.IndexOf("Menu:") - 5))
                                            Else
                                                tmp = Trim(Mid(fLine, fLine.IndexOf("Menu:") + 6, fLine.IndexOf(";") - fLine.IndexOf("Menu:") - 5))
                                            End If
                                            If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Menus.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                        Else
                                            Dim fround As Boolean = True, oldpos As Integer, pos As Integer = fLine.IndexOf(",")
                                            While pos > -1
                                                If fround Then
                                                    tmp = Trim(Mid(fLine, fLine.IndexOf("Menu:") + 6, pos - fLine.IndexOf("Menu:") - 5))
                                                    fround = False
                                                Else
                                                    tmp = Trim(Mid(fLine, fLine.IndexOf("Menu:", oldpos) + 6, pos - fLine.IndexOf("Menu:", oldpos) - 5))
                                                End If
                                                oldpos = pos
                                                pos = fLine.IndexOf(",", pos + 1)
                                                If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Menus.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                            End While
                                            If fLine.IndexOf(";") > -1 Then
                                                pos = fLine.LastIndexOf("Menu:")
                                                tmp = Trim(Mid(fLine, pos + 6, fLine.IndexOf(";") - pos - 5))
                                                If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Menus.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                            End If
                                        End If
                                    ElseIf fLine.IndexOf("Text:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                                        If fLine.IndexOf(",") = -1 Then
                                            If fLine.IndexOf(";") = -1 Then
                                                tmp = Trim(Mid(fLine, fLine.IndexOf("Text:") + 6, fLine.Length - fLine.IndexOf("Text:") - 5))
                                            Else
                                                tmp = Trim(Mid(fLine, fLine.IndexOf("Text:") + 6, fLine.IndexOf(";") - fLine.IndexOf("Text:") - 5))
                                            End If
                                            If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Texts.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                        Else
                                            Dim fround As Boolean = True, oldpos As Integer, pos As Integer = fLine.IndexOf(",")
                                            While pos > -1
                                                If fround Then
                                                    tmp = Trim(Mid(fLine, fLine.IndexOf("Text:") + 6, pos - fLine.IndexOf("Text:") - 5))
                                                    fround = False
                                                Else
                                                    tmp = Trim(Mid(fLine, fLine.IndexOf("Text:", oldpos) + 6, pos - fLine.IndexOf("Text:", oldpos) - 5))
                                                End If
                                                oldpos = pos
                                                pos = fLine.IndexOf(",", pos + 1)
                                                If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Texts.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                            End While
                                            If fLine.IndexOf(";") > -1 Then
                                                pos = fLine.LastIndexOf("Text:")
                                                tmp = Trim(Mid(fLine, pos + 6, fLine.IndexOf(";") - pos - 5))
                                                If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Texts.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                            End If
                                        End If
                                    ElseIf fLine.IndexOf("Text3D:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                                        If fLine.IndexOf(",") = -1 Then
                                            If fLine.IndexOf(";") = -1 Then
                                                tmp = Trim(Mid(fLine, fLine.IndexOf("Text3D:") + 8, fLine.Length - fLine.IndexOf("Text3D:") - 7))
                                            Else
                                                tmp = Trim(Mid(fLine, fLine.IndexOf("Text3D:") + 8, fLine.IndexOf(";") - fLine.IndexOf("Text3D:") - 7))
                                            End If
                                            If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Texts2.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                        Else
                                            Dim fround As Boolean = True, oldpos As Integer, pos As Integer = fLine.IndexOf(",")
                                            While pos > -1
                                                If fround Then
                                                    tmp = Trim(Mid(fLine, fLine.IndexOf("Text3D:") + 8, pos - fLine.IndexOf("Text3D:") - 7))
                                                    fround = False
                                                Else
                                                    tmp = Trim(Mid(fLine, fLine.IndexOf("Text3D:", oldpos) + 8, pos - fLine.IndexOf("Text3D:", oldpos) - 7))
                                                End If
                                                oldpos = pos
                                                pos = fLine.IndexOf(",", pos + 1)
                                                If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Texts2.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                            End While
                                            If fLine.IndexOf(";") > -1 Then
                                                pos = fLine.LastIndexOf("Text3D:")
                                                tmp = Trim(Mid(fLine, pos + 8, fLine.IndexOf(";") - pos - 7))
                                                If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Texts2.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                            End If
                                        End If
                                    ElseIf fLine.IndexOf("Float:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                                        If fLine.IndexOf(",") = -1 Then
                                            If fLine.IndexOf(";") = -1 Then
                                                tmp = Trim(Mid(fLine, fLine.IndexOf("Float:") + 7, fLine.Length - fLine.IndexOf("Float:") - 6))
                                            Else
                                                tmp = Trim(Mid(fLine, fLine.IndexOf("Float:") + 7, fLine.IndexOf(";") - fLine.IndexOf("Float:") - 6))
                                            End If
                                            If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Floats.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                        Else
                                            Dim fround As Boolean = True, oldpos As Integer, pos As Integer = fLine.IndexOf(",")
                                            While pos > -1
                                                If fround Then
                                                    tmp = Trim(Mid(fLine, fLine.IndexOf("Float:") + 7, pos - fLine.IndexOf("Float:") - 6))
                                                    fround = False
                                                Else
                                                    tmp = Trim(Mid(fLine, fLine.IndexOf("Float:", oldpos) + 7, pos - fLine.IndexOf("Float:", oldpos) - 6))
                                                End If
                                                oldpos = pos
                                                pos = fLine.IndexOf(",", pos + 1)
                                                If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Floats.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                            End While
                                            If fLine.IndexOf(";") > -1 Then
                                                pos = fLine.LastIndexOf("Float:")
                                                tmp = Trim(Mid(fLine, pos + 7, fLine.IndexOf(";") - pos - 6))
                                                If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Floats.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                            End If
                                        End If
                                    ElseIf fLine.IndexOf("DB:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                                        If fLine.IndexOf(",") = -1 Then
                                            If fLine.IndexOf(";") = -1 Then
                                                tmp = Trim(Mid(fLine, fLine.IndexOf("DB:") + 3, fLine.Length - fLine.IndexOf("DB:") - 2))
                                            Else
                                                tmp = Trim(Mid(fLine, fLine.IndexOf("DB:") + 3, fLine.IndexOf(";") - fLine.IndexOf("DB:") - 2))
                                            End If
                                            If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Dbs.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                        Else
                                            Dim fround As Boolean = True, oldpos As Integer, pos As Integer = fLine.IndexOf(",")
                                            While pos > -1
                                                If fround Then
                                                    tmp = Trim(Mid(fLine, fLine.IndexOf("DB:") + 3, pos - fLine.IndexOf("DB:") - 2))
                                                    fround = False
                                                Else
                                                    tmp = Trim(Mid(fLine, fLine.IndexOf("DB:", oldpos) + 3, pos - fLine.IndexOf("DB:", oldpos) - 2))
                                                End If
                                                oldpos = pos
                                                pos = fLine.IndexOf(",", pos + 1)
                                                If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Dbs.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                            End While
                                            If fLine.IndexOf(";") > -1 Then
                                                pos = fLine.LastIndexOf("DB:")
                                                tmp = Trim(Mid(fLine, pos + 3, fLine.IndexOf(";") - pos - 2))
                                                If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Dbs.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                            End If
                                        End If
                                    ElseIf fLine.IndexOf("DBResult:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                                        If fLine.IndexOf(",") = -1 Then
                                            If fLine.IndexOf(";") = -1 Then
                                                tmp = Trim(Mid(fLine, fLine.IndexOf("DBResult:") + 10, fLine.Length - fLine.IndexOf("DBResult:") - 8))
                                            Else
                                                tmp = Trim(Mid(fLine, fLine.IndexOf("DBResult:") + 10, fLine.IndexOf(";") - fLine.IndexOf("DBResult:") - 8))
                                            End If
                                            If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.DbRes.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                        Else
                                            Dim fround As Boolean = True, oldpos As Integer, pos As Integer = fLine.IndexOf(",")
                                            While pos > -1
                                                If fround Then
                                                    tmp = Trim(Mid(fLine, fLine.IndexOf("DBResult:") + 10, pos - fLine.IndexOf("DBResult:") - 8))
                                                    fround = False
                                                Else
                                                    tmp = Trim(Mid(fLine, fLine.IndexOf("DBResult:", oldpos) + 10, pos - fLine.IndexOf("DBResult:", oldpos) - 8))
                                                End If
                                                oldpos = pos
                                                pos = fLine.IndexOf(",", pos + 1)
                                                If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.DbRes.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                            End While
                                            If fLine.IndexOf(";") > -1 Then
                                                pos = fLine.LastIndexOf("DBResult:")
                                                tmp = Trim(Mid(fLine, pos + 10, fLine.IndexOf(";") - pos - 8))
                                                If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.DbRes.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                            End If
                                        End If
                                    ElseIf fLine.IndexOf("File:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                                        If fLine.IndexOf(",") = -1 Then
                                            If fLine.IndexOf(";") = -1 Then
                                                tmp = Trim(Mid(fLine, fLine.IndexOf("File:") + 6, fLine.Length - fLine.IndexOf("File:") - 5))
                                            Else
                                                tmp = Trim(Mid(fLine, fLine.IndexOf("File:") + 6, fLine.IndexOf(";") - fLine.IndexOf("File:") - 5))
                                            End If
                                            If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Files.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                        Else
                                            Dim fround As Boolean = True, oldpos As Integer, pos As Integer = fLine.IndexOf(",")
                                            While pos > -1
                                                If fround Then
                                                    tmp = Trim(Mid(fLine, fLine.IndexOf("File:") + 6, pos - fLine.IndexOf("File:") - 5))
                                                    fround = False
                                                Else
                                                    tmp = Trim(Mid(fLine, fLine.IndexOf("File:", oldpos) + 6, pos - fLine.IndexOf("File:", oldpos) - 5))
                                                End If
                                                oldpos = pos
                                                pos = fLine.IndexOf(",", pos + 1)
                                                If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Files.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                            End While
                                            If fLine.IndexOf(";") > -1 Then
                                                pos = fLine.LastIndexOf("File:")
                                                tmp = Trim(Mid(fLine, pos + 6, fLine.IndexOf(";") - pos - 5))
                                                If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Files.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                            End If
                                        End If
                                    End If
                                    count += 1
                                    fLine = Reader2.ReadLine()
                                Loop
                                Reader2.Close()
                            Else
                                Errors.Clear()
                                Errors.Add(New ListViewItem(New String() {"", "100", Name, count, "cannot read from file: """ & file2 & """"}, 0))
                            End If
                        ElseIf fLine.IndexOf("#tryinclude") > -1 Then
                            Dim file2 As String
                            If fLine.IndexOf("<") > -1 Then
                                file2 = Mid(fLine, fLine.IndexOf("<") + 2, fLine.IndexOf(">") - fLine.IndexOf("<") - 1)
                                file2 += ".inc"
                            Else
                                file2 = Mid(fLine, fLine.IndexOf("""") + 2, fLine.IndexOf("""") - fLine.IndexOf("""", fLine.IndexOf("""")))
                                If file2.IndexOf(".inc") = -1 Then file2 += ".inc"
                            End If
                            Dim count As Integer
                            If IO.File.Exists(My.Application.Info.DirectoryPath & "\Include\" & file2) Then
                                Dim Reader2 As New StreamReader(My.Application.Info.DirectoryPath & "\Include\" & file2)
                                fLine = Reader2.ReadLine()
                                Do Until fLine Is Nothing
                                    If fLine.Length = 0 OrElse fLine = "{" OrElse fLine = "}" OrElse fLine = ";" Then
                                        count += 1
                                        fLine = Reader2.ReadLine()
                                        Continue Do
                                    ElseIf fLine.StartsWith("//") Then
                                        CommentedLine = True
                                    ElseIf fLine.IndexOf("/*") > -1 AndAlso fLine.IndexOf("*/") = -1 Then
                                        CommentedSection = True
                                    ElseIf fLine.IndexOf("*/") > -1 Then
                                        CommentedSection = False
                                    End If
                                    If CommentedLine Or CommentedSection Then
                                        CommentedLine = False
                                        count += 1
                                        fLine = Reader2.ReadLine()
                                        Continue Do
                                    End If
                                    spos = fLine.IndexOf("native")
                                    If spos = -1 Then
                                        spos = fLine.IndexOf("stock")
                                        If spos = -1 Then spos = fLine.IndexOf("public")
                                    End If
                                    If spos > -1 AndAlso fLine.IndexOf("(") > -1 AndAlso fLine.IndexOf(")") > -1 AndAlso fLine.IndexOf("operator") = -1 Then
                                        Dim params As New List(Of String)
                                        params.AddRange(Split(Trim(Mid(fLine, fLine.IndexOf("(") + 2, fLine.IndexOf(")") - fLine.IndexOf("(") - 1)), ","))
                                        For i = 0 To params.Count - 1
                                            If i > 0 AndAlso params(i).Length > 0 AndAlso params(i).IndexOf("...") > -1 Then
                                                params(i - 1) += "," & params(i)
                                                params.RemoveAt(i)
                                                Continue For
                                            End If
                                        Next
                                        Dim func As PawnFunction = New PawnFunction(Trim(Mid(fLine, fLine.IndexOf(" ", spos) + 2, fLine.IndexOf("(") - fLine.IndexOf(" ", spos) - 1)).Replace("Float:", "").Replace("bool:", ""), file2.Replace(".inc", ":"), -1, params.ToArray)
                                        If Not TrueContainsFunction(ACLists.Functions, func, True) AndAlso Not TrueContainsFunction(ACLists.Callbacks, func) Then ACLists.Functions.Add(func)
                                    ElseIf fLine.IndexOf("forward") > -1 AndAlso fLine.IndexOf("(") > -1 AndAlso fLine.IndexOf(")") > -1 Then
                                        Dim func As PawnFunction = New PawnFunction(Trim(Mid(fLine, fLine.IndexOf(" ") + 1, fLine.IndexOf("(") - fLine.IndexOf(" "))), file2.Replace(".inc", ":"), -1, Split(Trim(Mid(fLine, fLine.IndexOf("(") + 2, fLine.IndexOf(")") - fLine.IndexOf("(") - 1)), ","))
                                        If Not TrueContainsFunction(ACLists.Callbacks, func, True) Then ACLists.Callbacks.Add(func)
                                    ElseIf fLine.IndexOf("#define") > -1 Then
                                        If fLine.IndexOf("0x") > -1 Then
                                            Dim col As PawnColor, value As String
                                            If fLine.IndexOf("(") > -1 AndAlso fLine.IndexOf(")") > -1 Then
                                                value = Trim(Mid(fLine, fLine.IndexOf("(") + 4, 8))
                                                If IsHex(value) Then
                                                    col = New PawnColor(Trim(Mid(fLine, fLine.IndexOf("#define") + 9, If(fLine.IndexOf(" ", fLine.IndexOf(" ") + 2) > 0, fLine.IndexOf(" ", fLine.IndexOf(" ") + 2) - fLine.IndexOf(" ") - 1, fLine.IndexOf(vbTab.ToString()) - fLine.IndexOf(" ") - 1))), Color.FromArgb(Integer.Parse(Mid(value, 7, 2), Globalization.NumberStyles.HexNumber), Integer.Parse(Mid(value, 1, 2), Globalization.NumberStyles.HexNumber), Integer.Parse(Mid(value, 3, 2), Globalization.NumberStyles.HexNumber), Integer.Parse(Mid(value, 5, 2), Globalization.NumberStyles.HexNumber)), -1)
                                                    If Not ACLists.Colors.Contains(col) Then ACLists.Colors.Add(col)
                                                End If
                                            Else
                                                value = Trim(Mid(fLine, fLine.IndexOf("0x") + 3, 8))
                                                If IsHex(value) Then
                                                    col = New PawnColor(Trim(Mid(fLine, fLine.IndexOf("#define") + 9, If(fLine.IndexOf(" ", fLine.IndexOf(" ") + 2) > 0, fLine.IndexOf(" ", fLine.IndexOf(" ") + 2) - fLine.IndexOf(" ") - 1, fLine.IndexOf(vbTab.ToString()) - fLine.IndexOf(" ") - 1))), Color.FromArgb(Integer.Parse(Mid(value, 7, 2), Globalization.NumberStyles.HexNumber), Integer.Parse(Mid(value, 1, 2), Globalization.NumberStyles.HexNumber), Integer.Parse(Mid(value, 3, 2), Globalization.NumberStyles.HexNumber), Integer.Parse(Mid(value, 5, 2), Globalization.NumberStyles.HexNumber)), -1)
                                                    If Not ACLists.Colors.Contains(col) Then ACLists.Colors.Add(col)
                                                End If
                                            End If
                                        Else
                                            If fLine.IndexOf("""") > -1 AndAlso fLine.IndexOf("""", fLine.IndexOf("""") + 1) > -1 Then
                                                Dim value As String
                                                If fLine.IndexOf("{") > -1 AndAlso fLine.IndexOf("}") > -1 Then
                                                    value = Trim(Mid(fLine, fLine.IndexOf("{") + 2, 6))
                                                Else
                                                    If fLine.IndexOf("{") > -1 AndAlso fLine.IndexOf("}") > -1 Then
                                                        value = Trim(Mid(fLine, fLine.IndexOf("{") + 2, 6))
                                                    Else
                                                        value = Trim(Mid(fLine, fLine.IndexOf("""") + 2, 6))
                                                    End If
                                                End If
                                                If IsHex(value) Then
                                                    Dim col As New PawnColor(Trim(Mid(fLine, fLine.IndexOf("#define") + 9, If(fLine.IndexOf(" ", fLine.IndexOf(" ") + 2) > 0, fLine.IndexOf(" ", fLine.IndexOf(" ") + 2) - fLine.IndexOf(" ") - 1, fLine.IndexOf(vbTab.ToString()) - fLine.IndexOf(" ") - 1))), Color.FromArgb(255, Integer.Parse(Mid(value, 1, 2), Globalization.NumberStyles.HexNumber), Integer.Parse(Mid(value, 3, 2), Globalization.NumberStyles.HexNumber), Integer.Parse(Mid(value, 5, 2), Globalization.NumberStyles.HexNumber)), -1)
                                                    If col.Name.Length > 0 AndAlso Not ACLists.eColors.Contains(col) Then ACLists.eColors.Add(col)
                                                End If
                                            End If
                                        End If
                                    ElseIf fLine.IndexOf("Menu:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                                        If fLine.IndexOf(",") = -1 Then
                                            If fLine.IndexOf(";") = -1 Then
                                                tmp = Trim(Mid(fLine, fLine.IndexOf("Menu:") + 6, fLine.Length - fLine.IndexOf("Menu:") - 5))
                                            Else
                                                tmp = Trim(Mid(fLine, fLine.IndexOf("Menu:") + 6, fLine.IndexOf(";") - fLine.IndexOf("Menu:") - 5))
                                            End If
                                            If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Menus.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                        Else
                                            Dim fround As Boolean = True, oldpos As Integer, pos As Integer = fLine.IndexOf(",")
                                            While pos > -1
                                                If fround Then
                                                    tmp = Trim(Mid(fLine, fLine.IndexOf("Menu:") + 6, pos - fLine.IndexOf("Menu:") - 5))
                                                    fround = False
                                                Else
                                                    tmp = Trim(Mid(fLine, fLine.IndexOf("Menu:", oldpos) + 6, pos - fLine.IndexOf("Menu:", oldpos) - 5))
                                                End If
                                                oldpos = pos
                                                pos = fLine.IndexOf(",", pos + 1)
                                                If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Menus.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                            End While
                                            If fLine.IndexOf(";") > -1 Then
                                                pos = fLine.LastIndexOf("Menu:")
                                                tmp = Trim(Mid(fLine, pos + 6, fLine.IndexOf(";") - pos - 5))
                                                If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Menus.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                            End If
                                        End If
                                    ElseIf fLine.IndexOf("Text:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                                        If fLine.IndexOf(",") = -1 Then
                                            If fLine.IndexOf(";") = -1 Then
                                                tmp = Trim(Mid(fLine, fLine.IndexOf("Text:") + 6, fLine.Length - fLine.IndexOf("Text:") - 5))
                                            Else
                                                tmp = Trim(Mid(fLine, fLine.IndexOf("Text:") + 6, fLine.IndexOf(";") - fLine.IndexOf("Text:") - 5))
                                            End If
                                            If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Texts.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                        Else
                                            Dim fround As Boolean = True, oldpos As Integer, pos As Integer = fLine.IndexOf(",")
                                            While pos > -1
                                                If fround Then
                                                    tmp = Trim(Mid(fLine, fLine.IndexOf("Text:") + 6, pos - fLine.IndexOf("Text:") - 5))
                                                    fround = False
                                                Else
                                                    tmp = Trim(Mid(fLine, fLine.IndexOf("Text:", oldpos) + 6, pos - fLine.IndexOf("Text:", oldpos) - 5))
                                                End If
                                                oldpos = pos
                                                pos = fLine.IndexOf(",", pos + 1)
                                                If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Texts.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                            End While
                                            If fLine.IndexOf(";") > -1 Then
                                                pos = fLine.LastIndexOf("Text:")
                                                tmp = Trim(Mid(fLine, pos + 6, fLine.IndexOf(";") - pos - 5))
                                                If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Texts.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                            End If
                                        End If
                                    ElseIf fLine.IndexOf("Text3D:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                                        If fLine.IndexOf(",") = -1 Then
                                            If fLine.IndexOf(";") = -1 Then
                                                tmp = Trim(Mid(fLine, fLine.IndexOf("Text3D:") + 8, fLine.Length - fLine.IndexOf("Text3D:") - 7))
                                            Else
                                                tmp = Trim(Mid(fLine, fLine.IndexOf("Text3D:") + 8, fLine.IndexOf(";") - fLine.IndexOf("Text3D:") - 7))
                                            End If
                                            If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Texts2.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                        Else
                                            Dim fround As Boolean = True, oldpos As Integer, pos As Integer = fLine.IndexOf(",")
                                            While pos > -1
                                                If fround Then
                                                    tmp = Trim(Mid(fLine, fLine.IndexOf("Text3D:") + 8, pos - fLine.IndexOf("Text3D:") - 7))
                                                    fround = False
                                                Else
                                                    tmp = Trim(Mid(fLine, fLine.IndexOf("Text3D:", oldpos) + 8, pos - fLine.IndexOf("Text3D:", oldpos) - 7))
                                                End If
                                                oldpos = pos
                                                pos = fLine.IndexOf(",", pos + 1)
                                                If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Texts2.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                            End While
                                            If fLine.IndexOf(";") > -1 Then
                                                pos = fLine.LastIndexOf("Text3D:")
                                                tmp = Trim(Mid(fLine, pos + 8, fLine.IndexOf(";") - pos - 7))
                                                If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Texts2.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                            End If
                                        End If
                                    ElseIf fLine.IndexOf("Float:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                                        If fLine.IndexOf(",") = -1 Then
                                            If fLine.IndexOf(";") = -1 Then
                                                tmp = Trim(Mid(fLine, fLine.IndexOf("Float:") + 7, fLine.Length - fLine.IndexOf("Float:") - 6))
                                            Else
                                                tmp = Trim(Mid(fLine, fLine.IndexOf("Float:") + 7, fLine.IndexOf(";") - fLine.IndexOf("Float:") - 6))
                                            End If
                                            If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Floats.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                        Else
                                            Dim fround As Boolean = True, oldpos As Integer, pos As Integer = fLine.IndexOf(",")
                                            While pos > -1
                                                If fround Then
                                                    tmp = Trim(Mid(fLine, fLine.IndexOf("Float:") + 7, pos - fLine.IndexOf("Float:") - 6))
                                                    fround = False
                                                Else
                                                    tmp = Trim(Mid(fLine, fLine.IndexOf("Float:", oldpos) + 7, pos - fLine.IndexOf("Float:", oldpos) - 6))
                                                End If
                                                oldpos = pos
                                                pos = fLine.IndexOf(",", pos + 1)
                                                If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Floats.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                            End While
                                            If fLine.IndexOf(";") > -1 Then
                                                pos = fLine.LastIndexOf("Float:")
                                                tmp = Trim(Mid(fLine, pos + 7, fLine.IndexOf(";") - pos - 6))
                                                If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Floats.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                            End If
                                        End If
                                    ElseIf fLine.IndexOf("DB:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                                        If fLine.IndexOf(",") = -1 Then
                                            If fLine.IndexOf(";") = -1 Then
                                                tmp = Trim(Mid(fLine, fLine.IndexOf("DB:") + 3, fLine.Length - fLine.IndexOf("DB:") - 2))
                                            Else
                                                tmp = Trim(Mid(fLine, fLine.IndexOf("DB:") + 3, fLine.IndexOf(";") - fLine.IndexOf("DB:") - 2))
                                            End If
                                            If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Dbs.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                        Else
                                            Dim fround As Boolean = True, oldpos As Integer, pos As Integer = fLine.IndexOf(",")
                                            While pos > -1
                                                If fround Then
                                                    tmp = Trim(Mid(fLine, fLine.IndexOf("DB:") + 3, pos - fLine.IndexOf("DB:") - 2))
                                                    fround = False
                                                Else
                                                    tmp = Trim(Mid(fLine, fLine.IndexOf("DB:", oldpos) + 3, pos - fLine.IndexOf("DB:", oldpos) - 2))
                                                End If
                                                oldpos = pos
                                                pos = fLine.IndexOf(",", pos + 1)
                                                If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Dbs.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                            End While
                                            If fLine.IndexOf(";") > -1 Then
                                                pos = fLine.LastIndexOf("DB:")
                                                tmp = Trim(Mid(fLine, pos + 3, fLine.IndexOf(";") - pos - 2))
                                                If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Dbs.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                            End If
                                        End If
                                    ElseIf fLine.IndexOf("DBResult:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                                        If fLine.IndexOf(",") = -1 Then
                                            If fLine.IndexOf(";") = -1 Then
                                                tmp = Trim(Mid(fLine, fLine.IndexOf("DBResult:") + 10, fLine.Length - fLine.IndexOf("DBResult:") - 8))
                                            Else
                                                tmp = Trim(Mid(fLine, fLine.IndexOf("DBResult:") + 10, fLine.IndexOf(";") - fLine.IndexOf("DBResult:") - 8))
                                            End If
                                            If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.DbRes.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                        Else
                                            Dim fround As Boolean = True, oldpos As Integer, pos As Integer = fLine.IndexOf(",")
                                            While pos > -1
                                                If fround Then
                                                    tmp = Trim(Mid(fLine, fLine.IndexOf("DBResult:") + 10, pos - fLine.IndexOf("DBResult:") - 8))
                                                    fround = False
                                                Else
                                                    tmp = Trim(Mid(fLine, fLine.IndexOf("DBResult:", oldpos) + 10, pos - fLine.IndexOf("DBResult:", oldpos) - 8))
                                                End If
                                                oldpos = pos
                                                pos = fLine.IndexOf(",", pos + 1)
                                                If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.DbRes.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                            End While
                                            If fLine.IndexOf(";") > -1 Then
                                                pos = fLine.LastIndexOf("DBResult:")
                                                tmp = Trim(Mid(fLine, pos + 10, fLine.IndexOf(";") - pos - 8))
                                                If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.DbRes.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                            End If
                                        End If
                                    ElseIf fLine.IndexOf("File:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                                        If fLine.IndexOf(",") = -1 Then
                                            If fLine.IndexOf(";") = -1 Then
                                                tmp = Trim(Mid(fLine, fLine.IndexOf("File:") + 6, fLine.Length - fLine.IndexOf("File:") - 5))
                                            Else
                                                tmp = Trim(Mid(fLine, fLine.IndexOf("File:") + 6, fLine.IndexOf(";") - fLine.IndexOf("File:") - 5))
                                            End If
                                            If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Files.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                        Else
                                            Dim fround As Boolean = True, oldpos As Integer, pos As Integer = fLine.IndexOf(",")
                                            While pos > -1
                                                If fround Then
                                                    tmp = Trim(Mid(fLine, fLine.IndexOf("File:") + 6, pos - fLine.IndexOf("File:") - 5))
                                                    fround = False
                                                Else
                                                    tmp = Trim(Mid(fLine, fLine.IndexOf("File:", oldpos) + 6, pos - fLine.IndexOf("File:", oldpos) - 5))
                                                End If
                                                oldpos = pos
                                                pos = fLine.IndexOf(",", pos + 1)
                                                If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Files.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                            End While
                                            If fLine.IndexOf(";") > -1 Then
                                                pos = fLine.LastIndexOf("File:")
                                                tmp = Trim(Mid(fLine, pos + 6, fLine.IndexOf(";") - pos - 5))
                                                If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Files.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                            End If
                                        End If
                                    End If
                                    count += 1
                                    fLine = Reader2.ReadLine()
                                Loop
                                Reader2.Close()
                            Else
                                Errors.Clear()
                                Errors.Add(New ListViewItem(New String() {"", "100", Name, count, "cannot read from file: """ & file2 & """"}, 1))
                            End If
                        ElseIf spos > -1 AndAlso fLine.IndexOf("(") > -1 AndAlso fLine.IndexOf(")") > -1 AndAlso fLine.IndexOf("operator") = -1 Then
                            Dim params As New List(Of String)
                            params.AddRange(Split(Trim(Mid(fLine, fLine.IndexOf("(") + 2, fLine.IndexOf(")") - fLine.IndexOf("(") - 1)), ","))
                            For i = 0 To params.Count - 1
                                If i > 0 AndAlso params(i).Length > 0 AndAlso params(i).IndexOf("...") > -1 Then
                                    params(i - 1) += "," & params(i)
                                    params.RemoveAt(i)
                                    Continue For
                                End If
                            Next
                            Dim func As PawnFunction = New PawnFunction(Trim(Mid(fLine, fLine.IndexOf(" ", spos) + 2, fLine.IndexOf("(") - fLine.IndexOf(" ", spos) - 1)).Replace("Float:", "").Replace("bool:", ""), file.Replace(".inc", ":"), -1, params.ToArray)
                            If Not TrueContainsFunction(ACLists.Functions, func, True) AndAlso Not TrueContainsFunction(ACLists.Callbacks, func) Then ACLists.Functions.Add(func)
                        ElseIf fLine.IndexOf("forward") > -1 AndAlso fLine.IndexOf("(") > -1 AndAlso fLine.IndexOf(")") > -1 Then
                            Dim func As PawnFunction = New PawnFunction(Trim(Mid(fLine, fLine.IndexOf(" ") + 1, fLine.IndexOf("(") - fLine.IndexOf(" "))), file.Replace(".inc", ":"), -1, Split(Trim(Mid(fLine, fLine.IndexOf("(") + 2, fLine.IndexOf(")") - fLine.IndexOf("(") - 1)), ","))
                            If Not TrueContainsFunction(ACLists.Callbacks, func, True) Then ACLists.Callbacks.Add(func)
                        ElseIf fLine.IndexOf("#define") > -1 Then
                            If fLine.IndexOf("0x") > -1 Then
                                Dim col As PawnColor, value As String
                                If fLine.IndexOf("(") > -1 AndAlso fLine.IndexOf(")") > -1 Then
                                    value = Trim(Mid(fLine, fLine.IndexOf("(") + 4, 8))
                                    If IsHex(value) Then
                                        col = New PawnColor(Trim(Mid(fLine, fLine.IndexOf("#define") + 9, If(fLine.IndexOf(" ", fLine.IndexOf(" ") + 2) > 0, fLine.IndexOf(" ", fLine.IndexOf(" ") + 2) - fLine.IndexOf(" ") - 1, fLine.IndexOf(vbTab.ToString()) - fLine.IndexOf(" ") - 1))), Color.FromArgb(Integer.Parse(Mid(value, 7, 2), Globalization.NumberStyles.HexNumber), Integer.Parse(Mid(value, 1, 2), Globalization.NumberStyles.HexNumber), Integer.Parse(Mid(value, 3, 2), Globalization.NumberStyles.HexNumber), Integer.Parse(Mid(value, 5, 2), Globalization.NumberStyles.HexNumber)), -1)
                                        If Not ACLists.Colors.Contains(col) Then ACLists.Colors.Add(col)
                                    End If
                                Else
                                    value = Trim(Mid(fLine, fLine.IndexOf("0x") + 3, 8))
                                    If IsHex(value) Then
                                        col = New PawnColor(Trim(Mid(fLine, fLine.IndexOf("#define") + 9, If(fLine.IndexOf(" ", fLine.IndexOf(" ") + 2) > 0, fLine.IndexOf(" ", fLine.IndexOf(" ") + 2) - fLine.IndexOf(" ") - 1, fLine.IndexOf(vbTab.ToString()) - fLine.IndexOf(" ") - 1))), Color.FromArgb(Integer.Parse(Mid(value, 7, 2), Globalization.NumberStyles.HexNumber), Integer.Parse(Mid(value, 1, 2), Globalization.NumberStyles.HexNumber), Integer.Parse(Mid(value, 3, 2), Globalization.NumberStyles.HexNumber), Integer.Parse(Mid(value, 5, 2), Globalization.NumberStyles.HexNumber)), -1)
                                        If Not ACLists.Colors.Contains(col) Then ACLists.Colors.Add(col)
                                    End If
                                End If
                            Else
                                If fLine.IndexOf("""") > -1 AndAlso fLine.IndexOf("""", fLine.IndexOf("""") + 1) > -1 Then
                                    Dim value As String
                                    If fLine.IndexOf("{") > -1 AndAlso fLine.IndexOf("}") > -1 Then
                                        value = Trim(Mid(fLine, fLine.IndexOf("{") + 2, 6))
                                    Else
                                        If fLine.IndexOf("{") > -1 AndAlso fLine.IndexOf("}") > -1 Then
                                            value = Trim(Mid(fLine, fLine.IndexOf("{") + 2, 6))
                                        Else
                                            value = Trim(Mid(fLine, fLine.IndexOf("""") + 2, 6))
                                        End If
                                    End If
                                    If IsHex(value) Then
                                        Dim col As New PawnColor(Trim(Mid(fLine, fLine.IndexOf("#define") + 9, If(fLine.IndexOf(" ", fLine.IndexOf(" ") + 2) > 0, fLine.IndexOf(" ", fLine.IndexOf(" ") + 2) - fLine.IndexOf(" ") - 1, fLine.IndexOf(vbTab.ToString()) - fLine.IndexOf(" ") - 1))), Color.FromArgb(255, Integer.Parse(Mid(value, 1, 2), Globalization.NumberStyles.HexNumber), Integer.Parse(Mid(value, 3, 2), Globalization.NumberStyles.HexNumber), Integer.Parse(Mid(value, 5, 2), Globalization.NumberStyles.HexNumber)), -1)
                                        If col.Name.Length > 0 AndAlso Not ACLists.eColors.Contains(col) Then ACLists.eColors.Add(col)
                                    End If
                                End If
                            End If
                        ElseIf fLine.IndexOf("Menu:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                            If fLine.IndexOf(",") = -1 Then
                                If fLine.IndexOf(";") = -1 Then
                                    tmp = Trim(Mid(fLine, fLine.IndexOf("Menu:") + 6, fLine.Length - fLine.IndexOf("Menu:") - 5))
                                Else
                                    tmp = Trim(Mid(fLine, fLine.IndexOf("Menu:") + 6, fLine.IndexOf(";") - fLine.IndexOf("Menu:") - 5))
                                End If
                                If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Menus.Contains(tmp) Then ACLists.Floats.Add(tmp)
                            Else
                                Dim fround As Boolean = True, oldpos As Integer, pos As Integer = fLine.IndexOf(",")
                                While pos > -1
                                    If fround Then
                                        tmp = Trim(Mid(fLine, fLine.IndexOf("Menu:") + 6, pos - fLine.IndexOf("Menu:") - 5))
                                        fround = False
                                    Else
                                        tmp = Trim(Mid(fLine, fLine.IndexOf("Menu:", oldpos) + 6, pos - fLine.IndexOf("Menu:", oldpos) - 5))
                                    End If
                                    oldpos = pos
                                    pos = fLine.IndexOf(",", pos + 1)
                                    If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Menus.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                End While
                                If fLine.IndexOf(";") > -1 Then
                                    pos = fLine.LastIndexOf("Menu:")
                                    tmp = Trim(Mid(fLine, pos + 6, fLine.IndexOf(";") - pos - 5))
                                    If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Menus.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                End If
                            End If
                        ElseIf fLine.IndexOf("Text:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                            If fLine.IndexOf(",") = -1 Then
                                If fLine.IndexOf(";") = -1 Then
                                    tmp = Trim(Mid(fLine, fLine.IndexOf("Text:") + 6, fLine.Length - fLine.IndexOf("Text:") - 5))
                                Else
                                    tmp = Trim(Mid(fLine, fLine.IndexOf("Text:") + 6, fLine.IndexOf(";") - fLine.IndexOf("Text:") - 5))
                                End If
                                If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Texts.Contains(tmp) Then ACLists.Floats.Add(tmp)
                            Else
                                Dim fround As Boolean = True, oldpos As Integer, pos As Integer = fLine.IndexOf(",")
                                While pos > -1
                                    If fround Then
                                        tmp = Trim(Mid(fLine, fLine.IndexOf("Text:") + 6, pos - fLine.IndexOf("Text:") - 5))
                                        fround = False
                                    Else
                                        tmp = Trim(Mid(fLine, fLine.IndexOf("Text:", oldpos) + 6, pos - fLine.IndexOf("Text:", oldpos) - 5))
                                    End If
                                    oldpos = pos
                                    pos = fLine.IndexOf(",", pos + 1)
                                    If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Texts.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                End While
                                If fLine.IndexOf(";") > -1 Then
                                    pos = fLine.LastIndexOf("Text:")
                                    tmp = Trim(Mid(fLine, pos + 6, fLine.IndexOf(";") - pos - 5))
                                    If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Texts.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                End If
                            End If
                        ElseIf fLine.IndexOf("Text3D:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                            If fLine.IndexOf(",") = -1 Then
                                If fLine.IndexOf(";") = -1 Then
                                    tmp = Trim(Mid(fLine, fLine.IndexOf("Text3D:") + 8, fLine.Length - fLine.IndexOf("Text3D:") - 7))
                                Else
                                    tmp = Trim(Mid(fLine, fLine.IndexOf("Text3D:") + 8, fLine.IndexOf(";") - fLine.IndexOf("Text3D:") - 7))
                                End If
                                If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Texts2.Contains(tmp) Then ACLists.Floats.Add(tmp)
                            Else
                                Dim fround As Boolean = True, oldpos As Integer, pos As Integer = fLine.IndexOf(",")
                                While pos > -1
                                    If fround Then
                                        tmp = Trim(Mid(fLine, fLine.IndexOf("Text3D:") + 8, pos - fLine.IndexOf("Text3D:") - 7))
                                        fround = False
                                    Else
                                        tmp = Trim(Mid(fLine, fLine.IndexOf("Text3D:", oldpos) + 8, pos - fLine.IndexOf("Text3D:", oldpos) - 7))
                                    End If
                                    oldpos = pos
                                    pos = fLine.IndexOf(",", pos + 1)
                                    If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Texts2.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                End While
                                If fLine.IndexOf(";") > -1 Then
                                    pos = fLine.LastIndexOf("Text3D:")
                                    tmp = Trim(Mid(fLine, pos + 8, fLine.IndexOf(";") - pos - 7))
                                    If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Texts2.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                End If
                            End If
                        ElseIf fLine.IndexOf("Float:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                            If fLine.IndexOf(",") = -1 Then
                                If fLine.IndexOf(";") = -1 Then
                                    tmp = Trim(Mid(fLine, fLine.IndexOf("Float:") + 7, fLine.Length - fLine.IndexOf("Float:") - 6))
                                Else
                                    tmp = Trim(Mid(fLine, fLine.IndexOf("Float:") + 7, fLine.IndexOf(";") - fLine.IndexOf("Float:") - 6))
                                End If
                                If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Floats.Contains(tmp) Then ACLists.Floats.Add(tmp)
                            Else
                                Dim fround As Boolean = True, oldpos As Integer, pos As Integer = fLine.IndexOf(",")
                                While pos > -1
                                    If fround Then
                                        tmp = Trim(Mid(fLine, fLine.IndexOf("Float:") + 7, pos - fLine.IndexOf("Float:") - 6))
                                        fround = False
                                    Else
                                        tmp = Trim(Mid(fLine, fLine.IndexOf("Float:", oldpos) + 7, pos - fLine.IndexOf("Float:", oldpos) - 6))
                                    End If
                                    oldpos = pos
                                    pos = fLine.IndexOf(",", pos + 1)
                                    If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Floats.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                End While
                                If fLine.IndexOf(";") > -1 Then
                                    pos = fLine.LastIndexOf("Float:")
                                    tmp = Trim(Mid(fLine, pos + 7, fLine.IndexOf(";") - pos - 6))
                                    If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Floats.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                End If
                            End If
                        ElseIf fLine.IndexOf("DB:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                            If fLine.IndexOf(",") = -1 Then
                                If fLine.IndexOf(";") = -1 Then
                                    tmp = Trim(Mid(fLine, fLine.IndexOf("DB:") + 3, fLine.Length - fLine.IndexOf("DB:") - 2))
                                Else
                                    tmp = Trim(Mid(fLine, fLine.IndexOf("DB:") + 3, fLine.IndexOf(";") - fLine.IndexOf("DB:") - 2))
                                End If
                                If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Dbs.Contains(tmp) Then ACLists.Floats.Add(tmp)
                            Else
                                Dim fround As Boolean = True, oldpos As Integer, pos As Integer = fLine.IndexOf(",")
                                While pos > -1
                                    If fround Then
                                        tmp = Trim(Mid(fLine, fLine.IndexOf("DB:") + 3, pos - fLine.IndexOf("DB:") - 2))
                                        fround = False
                                    Else
                                        tmp = Trim(Mid(fLine, fLine.IndexOf("DB:", oldpos) + 3, pos - fLine.IndexOf("DB:", oldpos) - 2))
                                    End If
                                    oldpos = pos
                                    pos = fLine.IndexOf(",", pos + 1)
                                    If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Dbs.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                End While
                                If fLine.IndexOf(";") > -1 Then
                                    pos = fLine.LastIndexOf("DB:")
                                    tmp = Trim(Mid(fLine, pos + 3, fLine.IndexOf(";") - pos - 2))
                                    If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Dbs.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                End If
                            End If
                        ElseIf fLine.IndexOf("DBResult:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                            If fLine.IndexOf(",") = -1 Then
                                If fLine.IndexOf(";") = -1 Then
                                    tmp = Trim(Mid(fLine, fLine.IndexOf("DBResult:") + 10, fLine.Length - fLine.IndexOf("DBResult:") - 8))
                                Else
                                    tmp = Trim(Mid(fLine, fLine.IndexOf("DBResult:") + 10, fLine.IndexOf(";") - fLine.IndexOf("DBResult:") - 8))
                                End If
                                If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.DbRes.Contains(tmp) Then ACLists.Floats.Add(tmp)
                            Else
                                Dim fround As Boolean = True, oldpos As Integer, pos As Integer = fLine.IndexOf(",")
                                While pos > -1
                                    If fround Then
                                        tmp = Trim(Mid(fLine, fLine.IndexOf("DBResult:") + 10, pos - fLine.IndexOf("DBResult:") - 8))
                                        fround = False
                                    Else
                                        tmp = Trim(Mid(fLine, fLine.IndexOf("DBResult:", oldpos) + 10, pos - fLine.IndexOf("DBResult:", oldpos) - 8))
                                    End If
                                    oldpos = pos
                                    pos = fLine.IndexOf(",", pos + 1)
                                    If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.DbRes.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                End While
                                If fLine.IndexOf(";") > -1 Then
                                    pos = fLine.LastIndexOf("DBResult:")
                                    tmp = Trim(Mid(fLine, pos + 10, fLine.IndexOf(";") - pos - 8))
                                    If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.DbRes.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                End If
                            End If
                        ElseIf fLine.IndexOf("File:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                            If fLine.IndexOf(",") = -1 Then
                                If fLine.IndexOf(";") = -1 Then
                                    tmp = Trim(Mid(fLine, fLine.IndexOf("File:") + 6, fLine.Length - fLine.IndexOf("File:") - 5))
                                Else
                                    tmp = Trim(Mid(fLine, fLine.IndexOf("File:") + 6, fLine.IndexOf(";") - fLine.IndexOf("File:") - 5))
                                End If
                                If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Files.Contains(tmp) Then ACLists.Floats.Add(tmp)
                            Else
                                Dim fround As Boolean = True, oldpos As Integer, pos As Integer = fLine.IndexOf(",")
                                While pos > -1
                                    If fround Then
                                        tmp = Trim(Mid(fLine, fLine.IndexOf("File:") + 6, pos - fLine.IndexOf("File:") - 5))
                                        fround = False
                                    Else
                                        tmp = Trim(Mid(fLine, fLine.IndexOf("File:", oldpos) + 6, pos - fLine.IndexOf("File:", oldpos) - 5))
                                    End If
                                    oldpos = pos
                                    pos = fLine.IndexOf(",", pos + 1)
                                    If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Files.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                End While
                                If fLine.IndexOf(";") > -1 Then
                                    pos = fLine.LastIndexOf("File:")
                                    tmp = Trim(Mid(fLine, pos + 6, fLine.IndexOf(";") - pos - 5))
                                    If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Files.Contains(tmp) Then ACLists.Floats.Add(tmp)
                                End If
                            End If
                        End If
                        fLine = Reader.ReadLine()
                    Loop
                    Reader.Close()
                Else
                    Errors.Clear()
                    Errors.Add(New ListViewItem(New String() {"", "100", Name, line.Number + 1, "cannot read from file: """ & file & """"}, 1))
                End If
            ElseIf spos > -1 AndAlso line.Text.IndexOf("(") > -1 AndAlso line.Text.IndexOf(")") > -1 AndAlso line.Text.IndexOf("operator") = -1 Then
                Dim params As New List(Of String)
                params.AddRange(Split(Trim(Mid(line.Text, line.Text.IndexOf("(") + 2, line.Text.IndexOf(")") - line.Text.IndexOf("(") - 1)), ","))
                For i = 0 To params.Count - 1
                    If i > 0 AndAlso params(i).Length > 0 AndAlso params(i).IndexOf("...") > -1 Then
                        params(i - 1) += "," & params(i)
                        params.RemoveAt(i)
                        Continue For
                    End If
                Next
                Dim func As PawnFunction = New PawnFunction(Trim(Mid(line.Text, line.Text.IndexOf(" ", spos) + 2, line.Text.IndexOf("(") - line.Text.IndexOf(" ", spos) - 1)), Name.Replace(".inc", ":"), -1, params.ToArray)
                If Not TrueContainsFunction(ACLists.Functions, func, True) AndAlso Not TrueContainsFunction(ACLists.Callbacks, func) Then ACLists.Functions.Add(func)
            ElseIf line.Text.IndexOf("forward") > -1 AndAlso line.Text.IndexOf("(") > -1 AndAlso line.Text.IndexOf(")") > -1 Then
                Dim func As PawnFunction = New PawnFunction(Trim(Mid(line.Text, line.Text.IndexOf(" ") + 1, line.Text.IndexOf("(") - line.Text.IndexOf(" "))), Name.Replace(".inc", ":"), -1, Split(Trim(Mid(line.Text, line.Text.IndexOf("(") + 2, line.Text.IndexOf(")") - line.Text.IndexOf("(") - 1)), ","))
                If Not TrueContainsFunction(ACLists.Callbacks, func, True) Then ACLists.Callbacks.Add(func)
            ElseIf line.Text.IndexOf("#define") > -1 Then
                If line.Text.IndexOf("0x") > -1 Then
                    Dim col As PawnColor, value As String
                    If line.Text.IndexOf("(") > -1 AndAlso line.Text.IndexOf(")") > -1 Then
                        value = Trim(Mid(line.Text, line.Text.IndexOf("(") + 4, 8))
                        If IsHex(value) Then
                            col = New PawnColor(Trim(Mid(line.Text, line.Text.IndexOf("#define") + 9, If(line.Text.IndexOf(" ", line.Text.IndexOf(" ") + 2) > 0, line.Text.IndexOf(" ", line.Text.IndexOf(" ") + 2) - line.Text.IndexOf(" ") - 1, line.Text.IndexOf(vbTab.ToString()) - line.Text.IndexOf(" ") - 1))), Color.FromArgb(Integer.Parse(Mid(value, 7, 2), Globalization.NumberStyles.HexNumber), Integer.Parse(Mid(value, 1, 2), Globalization.NumberStyles.HexNumber), Integer.Parse(Mid(value, 3, 2), Globalization.NumberStyles.HexNumber), Integer.Parse(Mid(value, 5, 2), Globalization.NumberStyles.HexNumber)), line.Number)
                            If Not ACLists.Colors.Contains(col) Then ACLists.Colors.Add(col)
                        End If
                    Else
                        value = Trim(Mid(line.Text, line.Text.IndexOf("0x") + 3, 8))
                        If IsHex(value) Then
                            col = New PawnColor(Trim(Mid(line.Text, line.Text.IndexOf("#define") + 9, If(line.Text.IndexOf(" ", line.Text.IndexOf(" ") + 2) > 0, line.Text.IndexOf(" ", line.Text.IndexOf(" ") + 2) - line.Text.IndexOf(" ") - 1, line.Text.IndexOf(vbTab.ToString()) - line.Text.IndexOf(" ") - 1))), Color.FromArgb(Integer.Parse(Mid(value, 7, 2), Globalization.NumberStyles.HexNumber), Integer.Parse(Mid(value, 1, 2), Globalization.NumberStyles.HexNumber), Integer.Parse(Mid(value, 3, 2), Globalization.NumberStyles.HexNumber), Integer.Parse(Mid(value, 5, 2), Globalization.NumberStyles.HexNumber)), line.Number)
                            If Not ACLists.Colors.Contains(col) Then ACLists.Colors.Add(col)
                        End If
                    End If
                Else
                    If line.Text.IndexOf("""") > -1 AndAlso line.Text.IndexOf("""", line.Text.IndexOf("""") + 1) > -1 Then
                        Dim value As String
                        If line.Text.IndexOf("{") > -1 AndAlso line.Text.IndexOf("}") > -1 Then
                            value = Trim(Mid(line.Text, line.Text.IndexOf("{") + 2, 6))
                        Else
                            value = Trim(Mid(line.Text, line.Text.IndexOf("""") + 2, 6))
                        End If
                        If IsHex(value) Then
                            Dim col As New PawnColor(Trim(Mid(line.Text, line.Text.IndexOf("#define") + 9, If(line.Text.IndexOf(" ", line.Text.IndexOf(" ") + 2) > 0, line.Text.IndexOf(" ", line.Text.IndexOf(" ") + 2) - line.Text.IndexOf(" ") - 1, line.Text.IndexOf(vbTab.ToString()) - line.Text.IndexOf(" ") - 1))), Color.FromArgb(255, Integer.Parse(Mid(value, 1, 2), Globalization.NumberStyles.HexNumber), Integer.Parse(Mid(value, 3, 2), Globalization.NumberStyles.HexNumber), Integer.Parse(Mid(value, 5, 2), Globalization.NumberStyles.HexNumber)), -1)
                            If col.Name.Length > 0 AndAlso Not ACLists.eColors.Contains(col) Then ACLists.eColors.Add(col)
                        End If
                    End If
                End If
            ElseIf line.Text.IndexOf("Menu:") > -1 AndAlso line.Text.IndexOf("(") = -1 AndAlso line.Text.IndexOf(")") = -1 Then
                If line.Text.IndexOf(",") = -1 Then
                    If line.Text.IndexOf(";") = -1 Then
                        tmp = Trim(Mid(line.Text, line.Text.IndexOf("Menu:") + 6, line.Text.Length - line.Text.IndexOf("Menu:") - 5))
                    Else
                        tmp = Trim(Mid(line.Text, line.Text.IndexOf("Menu:") + 6, line.Text.IndexOf(";") - line.Text.IndexOf("Menu:") - 5))
                    End If
                    If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Menus.Contains(tmp) Then ACLists.Floats.Add(tmp)
                Else
                    Dim fround As Boolean = True, oldpos As Integer, pos As Integer = line.Text.IndexOf(",")
                    While pos > -1
                        If fround Then
                            tmp = Trim(Mid(line.Text, line.Text.IndexOf("Menu:") + 6, pos - line.Text.IndexOf("Menu:") - 5))
                            fround = False
                        Else
                            tmp = Trim(Mid(line.Text, line.Text.IndexOf("Menu:", oldpos) + 6, pos - line.Text.IndexOf("Menu:", oldpos) - 5))
                        End If
                        oldpos = pos
                        pos = line.Text.IndexOf(",", pos + 1)
                        If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Menus.Contains(tmp) Then ACLists.Floats.Add(tmp)
                    End While
                    If line.Text.IndexOf(";") > -1 Then
                        pos = line.Text.LastIndexOf("Menu:")
                        tmp = Trim(Mid(line.Text, pos + 6, line.Text.IndexOf(";") - pos - 5))
                        If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Menus.Contains(tmp) Then ACLists.Floats.Add(tmp)
                    End If
                End If
            ElseIf line.Text.IndexOf("Text:") > -1 AndAlso line.Text.IndexOf("(") = -1 AndAlso line.Text.IndexOf(")") = -1 Then
                If line.Text.IndexOf(",") = -1 Then
                    If line.Text.IndexOf(";") = -1 Then
                        tmp = Trim(Mid(line.Text, line.Text.IndexOf("Text:") + 6, line.Text.Length - line.Text.IndexOf("Text:") - 5))
                    Else
                        tmp = Trim(Mid(line.Text, line.Text.IndexOf("Text:") + 6, line.Text.IndexOf(";") - line.Text.IndexOf("Text:") - 5))
                    End If
                    If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Texts.Contains(tmp) Then ACLists.Floats.Add(tmp)
                Else
                    Dim fround As Boolean = True, oldpos As Integer, pos As Integer = line.Text.IndexOf(",")
                    While pos > -1
                        If fround Then
                            tmp = Trim(Mid(line.Text, line.Text.IndexOf("Text:") + 6, pos - line.Text.IndexOf("Text:") - 5))
                            fround = False
                        Else
                            tmp = Trim(Mid(line.Text, line.Text.IndexOf("Text:", oldpos) + 6, pos - line.Text.IndexOf("Text:", oldpos) - 5))
                        End If
                        oldpos = pos
                        pos = line.Text.IndexOf(",", pos + 1)
                        If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Texts.Contains(tmp) Then ACLists.Floats.Add(tmp)
                    End While
                    If line.Text.IndexOf(";") > -1 Then
                        pos = line.Text.LastIndexOf("Text:")
                        tmp = Trim(Mid(line.Text, pos + 6, line.Text.IndexOf(";") - pos - 5))
                        If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Texts.Contains(tmp) Then ACLists.Floats.Add(tmp)
                    End If
                End If
            ElseIf line.Text.IndexOf("Text3D:") > -1 AndAlso line.Text.IndexOf("(") = -1 AndAlso line.Text.IndexOf(")") = -1 Then
                If line.Text.IndexOf(",") = -1 Then
                    If line.Text.IndexOf(";") = -1 Then
                        tmp = Trim(Mid(line.Text, line.Text.IndexOf("Text3D:") + 8, line.Text.Length - line.Text.IndexOf("Text3D:") - 7))
                    Else
                        tmp = Trim(Mid(line.Text, line.Text.IndexOf("Text3D:") + 8, line.Text.IndexOf(";") - line.Text.IndexOf("Text3D:") - 7))
                    End If
                    If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Texts2.Contains(tmp) Then ACLists.Floats.Add(tmp)
                Else
                    Dim fround As Boolean = True, oldpos As Integer, pos As Integer = line.Text.IndexOf(",")
                    While pos > -1
                        If fround Then
                            tmp = Trim(Mid(line.Text, line.Text.IndexOf("Text3D:") + 8, pos - line.Text.IndexOf("Text3D:") - 7))
                            fround = False
                        Else
                            tmp = Trim(Mid(line.Text, line.Text.IndexOf("Text3D:", oldpos) + 8, pos - line.Text.IndexOf("Text3D:", oldpos) - 7))
                        End If
                        oldpos = pos
                        pos = line.Text.IndexOf(",", pos + 1)
                        If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Texts2.Contains(tmp) Then ACLists.Floats.Add(tmp)
                    End While
                    If line.Text.IndexOf(";") > -1 Then
                        pos = line.Text.LastIndexOf("Text3D:")
                        tmp = Trim(Mid(line.Text, pos + 8, line.Text.IndexOf(";") - pos - 7))
                        If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Texts2.Contains(tmp) Then ACLists.Floats.Add(tmp)
                    End If
                End If
            ElseIf line.Text.IndexOf("Float:") > -1 AndAlso line.Text.IndexOf("(") = -1 AndAlso line.Text.IndexOf(")") = -1 Then
                If line.Text.IndexOf(",") = -1 Then
                    If line.Text.IndexOf(";") = -1 Then
                        tmp = Trim(Mid(line.Text, line.Text.IndexOf("Float:") + 7, line.Text.Length - line.Text.IndexOf("Float:") - 6))
                    Else
                        tmp = Trim(Mid(line.Text, line.Text.IndexOf("Float:") + 7, line.Text.IndexOf(";") - line.Text.IndexOf("Float:") - 6))
                    End If
                    If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Floats.Contains(tmp) Then ACLists.Floats.Add(tmp)
                Else
                    Dim fround As Boolean = True, oldpos As Integer, pos As Integer = line.Text.IndexOf(",")
                    While pos > -1
                        If fround Then
                            tmp = Trim(Mid(line.Text, line.Text.IndexOf("Float:") + 7, pos - line.Text.IndexOf("Float:") - 6))
                            fround = False
                        Else
                            tmp = Trim(Mid(line.Text, line.Text.IndexOf("Float:", oldpos) + 7, pos - line.Text.IndexOf("Float:", oldpos) - 6))
                        End If
                        oldpos = pos
                        pos = line.Text.IndexOf(",", pos + 1)
                        If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Floats.Contains(tmp) Then ACLists.Floats.Add(tmp)
                    End While
                    If line.Text.IndexOf(";") > -1 Then
                        pos = line.Text.LastIndexOf("Float:")
                        tmp = Trim(Mid(line.Text, pos + 7, line.Text.IndexOf(";") - pos - 6))
                        If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Floats.Contains(tmp) Then ACLists.Floats.Add(tmp)
                    End If
                End If
            ElseIf line.Text.IndexOf("DB:") > -1 AndAlso line.Text.IndexOf("(") = -1 AndAlso line.Text.IndexOf(")") = -1 Then
                If line.Text.IndexOf(",") = -1 Then
                    If line.Text.IndexOf(";") = -1 Then
                        tmp = Trim(Mid(line.Text, line.Text.IndexOf("DB:") + 3, line.Text.Length - line.Text.IndexOf("DB:") - 2))
                    Else
                        tmp = Trim(Mid(line.Text, line.Text.IndexOf("DB:") + 3, line.Text.IndexOf(";") - line.Text.IndexOf("DB:") - 2))
                    End If
                    If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Dbs.Contains(tmp) Then ACLists.Floats.Add(tmp)
                Else
                    Dim fround As Boolean = True, oldpos As Integer, pos As Integer = line.Text.IndexOf(",")
                    While pos > -1
                        If fround Then
                            tmp = Trim(Mid(line.Text, line.Text.IndexOf("DB:") + 3, pos - line.Text.IndexOf("DB:") - 2))
                            fround = False
                        Else
                            tmp = Trim(Mid(line.Text, line.Text.IndexOf("DB:", oldpos) + 3, pos - line.Text.IndexOf("DB:", oldpos) - 2))
                        End If
                        oldpos = pos
                        pos = line.Text.IndexOf(",", pos + 1)
                        If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Dbs.Contains(tmp) Then ACLists.Floats.Add(tmp)
                    End While
                    If line.Text.IndexOf(";") > -1 Then
                        pos = line.Text.LastIndexOf("DB:")
                        tmp = Trim(Mid(line.Text, pos + 3, line.Text.IndexOf(";") - pos - 2))
                        If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Dbs.Contains(tmp) Then ACLists.Floats.Add(tmp)
                    End If
                End If
            ElseIf line.Text.IndexOf("DBResult:") > -1 AndAlso line.Text.IndexOf("(") = -1 AndAlso line.Text.IndexOf(")") = -1 Then
                If line.Text.IndexOf(",") = -1 Then
                    If line.Text.IndexOf(";") = -1 Then
                        tmp = Trim(Mid(line.Text, line.Text.IndexOf("DBResult:") + 10, line.Text.Length - line.Text.IndexOf("DBResult:") - 8))
                    Else
                        tmp = Trim(Mid(line.Text, line.Text.IndexOf("DBResult:") + 10, line.Text.IndexOf(";") - line.Text.IndexOf("DBResult:") - 8))
                    End If
                    If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.DbRes.Contains(tmp) Then ACLists.Floats.Add(tmp)
                Else
                    Dim fround As Boolean = True, oldpos As Integer, pos As Integer = line.Text.IndexOf(",")
                    While pos > -1
                        If fround Then
                            tmp = Trim(Mid(line.Text, line.Text.IndexOf("DBResult:") + 10, pos - line.Text.IndexOf("DBResult:") - 8))
                            fround = False
                        Else
                            tmp = Trim(Mid(line.Text, line.Text.IndexOf("DBResult:", oldpos) + 10, pos - line.Text.IndexOf("DBResult:", oldpos) - 8))
                        End If
                        oldpos = pos
                        pos = line.Text.IndexOf(",", pos + 1)
                        If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.DbRes.Contains(tmp) Then ACLists.Floats.Add(tmp)
                    End While
                    If line.Text.IndexOf(";") > -1 Then
                        pos = line.Text.LastIndexOf("DBResult:")
                        tmp = Trim(Mid(line.Text, pos + 10, line.Text.IndexOf(";") - pos - 8))
                        If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.DbRes.Contains(tmp) Then ACLists.Floats.Add(tmp)
                    End If
                End If
            ElseIf line.Text.IndexOf("File:") > -1 AndAlso line.Text.IndexOf("(") = -1 AndAlso line.Text.IndexOf(")") = -1 Then
                If line.Text.IndexOf(",") = -1 Then
                    If line.Text.IndexOf(";") = -1 Then
                        tmp = Trim(Mid(line.Text, line.Text.IndexOf("File:") + 6, line.Text.Length - line.Text.IndexOf("File:") - 5))
                    Else
                        tmp = Trim(Mid(line.Text, line.Text.IndexOf("File:") + 6, line.Text.IndexOf(";") - line.Text.IndexOf("File:") - 5))
                    End If
                    If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Files.Contains(tmp) Then ACLists.Floats.Add(tmp)
                Else
                    Dim fround As Boolean = True, oldpos As Integer, pos As Integer = line.Text.IndexOf(",")
                    While pos > -1
                        If fround Then
                            tmp = Trim(Mid(line.Text, line.Text.IndexOf("File:") + 6, pos - line.Text.IndexOf("File:") - 5))
                            fround = False
                        Else
                            tmp = Trim(Mid(line.Text, line.Text.IndexOf("File:", oldpos) + 6, pos - line.Text.IndexOf("File:", oldpos) - 5))
                        End If
                        oldpos = pos
                        pos = line.Text.IndexOf(",", pos + 1)
                        If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Files.Contains(tmp) Then ACLists.Floats.Add(tmp)
                    End While
                    If line.Text.IndexOf(";") > -1 Then
                        pos = line.Text.LastIndexOf("File:")
                        tmp = Trim(Mid(line.Text, pos + 6, line.Text.IndexOf(";") - pos - 5))
                        If tmp.Length > 0 AndAlso tmp <> " " AndAlso Not Char.IsSymbol(tmp) AndAlso Not ACLists.Files.Contains(tmp) Then ACLists.Floats.Add(tmp)
                    End If
                End If
            End If
        Next
        Dim tmpstring As String = vbNullString
        For Each item As PawnFunction In ACLists.Functions
            tmpstring += item.Name & " "
        Next
        SyntaxHandle.Lexing.Keywords(3) = tmpstring
        SyntaxHandle.Lexing.Colorize()
        For Each item In ACLists.Floats
            If item = "cellmin" Then ACLists.Floats.Remove(item)
        Next
        If Errors.Count Then
            Main.ListView1.Items.Clear()
            Dim Header As Boolean() = New Boolean() {True, True, True}
            For Each item As ListViewItem In Errors
                Main.ListView1.Items.Add(item)
                If Not Header(0) AndAlso Main.ListView1.Columns(1).Text.Length <= item.SubItems(1).Text.Length Then Header(0) = False
                If Not Header(1) AndAlso Main.ListView1.Columns(2).Text.Length <= item.SubItems(2).Text.Length Then Header(1) = False
                If Not Header(2) AndAlso Main.ListView1.Columns(3).Text.Length <= item.SubItems(3).Text.Length Then Header(2) = False
            Next
            With Main.ListView1
                .Columns(0).Width = 25
                .Columns(1).Width = If(Header(0), -2, -1)
                .Columns(2).Width = If(Header(1), -2, -1)
                .Columns(3).Width = If(Header(2), -2, -1)
                .Columns(4).Width = -2
            End With
            With Main.ListView1
                .Columns(0).Width = 25
                .Columns(1).Width = If(Header(0), -2, -1)
                .Columns(2).Width = If(Header(1), -2, -1)
                .Columns(3).Width = If(Header(2), -2, -1)
                .Columns(4).Width = -2
            End With
        End If
        With Main.TreeView2
            If .InvokeRequired Then
                Dim cNode As New TreeNode()
                ClearDelegate.Invoke(Main.TreeView2)
                For Each func As PawnFunction In ACLists.Functions
                    If Not TrueNodeContains(Main.TreeView2.Nodes, func.Include) Then cNode = FirstTreeDelegate.Invoke(func.Include, Main.TreeView2)
                    TreeDelegate.Invoke(func.Name, cNode)
                Next
            Else
                Dim IncCount As Integer = -1
                .Nodes.Clear()
                For Each func As PawnFunction In ACLists.Functions
                    If Not TrueNodeContains(Main.TreeView2.Nodes, func.Include) Then
                        .Nodes.Add(func.Include)
                        IncCount += 1
                    End If
                    .Nodes(IncCount).Nodes.Add(func.Name)
                Next
            End If
        End With
        lastcall = GetTickCount()
    End Sub

#End Region

End Class
