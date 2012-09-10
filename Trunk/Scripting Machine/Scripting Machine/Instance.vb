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
Imports System.Text.RegularExpressions

Public Class Instance

#Region "Arrays"

#Region "Private"

    Private _Saved As Boolean, _
        _Name As String, _
        _Path As String, _
        _Created As Boolean, _
        _Ext As String, _
        _Rate As Integer, _
        _Font As Font = Settings.cFont, _
        _index As Integer, _
        wait As Boolean, _
        first As Boolean, _
        MarginUpdater As New uMargin(AddressOf UpdateMargin),
        justreloaded As Boolean, _
        DataFileUpdater As New uFileData(AddressOf UpdateFileData)
    Private WithEvents Tim As New Timers.Timer(1000)

#End Region

#Region "Public"

    Public ACLists As AutoCompleteLists, _
        Errors As New List(Of ListViewItem), _
        OutPut As String, _
        DataUpdater As New uData(AddressOf UpdateData), _
        DataUpdaterEx As New uDataEx(AddressOf UpdateDataEx)

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
        Public Functions As List(Of PawnFunction), _
            Callbacks As List(Of PawnFunction), _
            Colors As List(Of PawnColor), _
            eColors As List(Of PawnColor), _
            Files As List(Of String), _
            Dbs As List(Of String), _
            DbRes As List(Of String), _
            Menus As List(Of String), _
            Texts As List(Of String), _
            Texts2 As List(Of String), _
            Floats As List(Of String), _
            UserDefinedPublics As List(Of CustomUserPublics)
    End Structure

#End Region

#Region "Components"

    Friend WithEvents TabHandle As TabPageEx.TabPageEx, _
        SyntaxHandle As Scintilla

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
    Public Delegate Sub uFileData()

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
                Main.SaveToolStripMenuItem.Enabled = False
                Main.ToolStripButton3.Enabled = False
            Else
                TabHandle.Text = _Name & " *"
                Main.SaveToolStripMenuItem.Enabled = True
                Main.ToolStripButton3.Enabled = True
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
            CurrentFunction = GetCurrentFunction(GetLineCursorPosition(True), False, True)
        End Get
    End Property

    Public ReadOnly Property CurrentParamIndex As Integer
        Get
            CurrentParamIndex = GetCurrentParamIndex()
        End Get
    End Property

    Public ReadOnly Property Index As Integer
        Get
            Index = _index
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

#End Region

#Region "Methods"

    Public Sub New(ByVal name As String, ByVal index As Integer, Optional ByVal iwait As Boolean = True)
        _Name = name
        _index = index
        TabHandle = New TabPageEx.TabPageEx()
        SyntaxHandle = New Scintilla()
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
            .UserDefinedPublics = New List(Of CustomUserPublics)
        End With
        With SyntaxHandle
            If index <> 0 Then name = "SyntaxHandle_" & index
            .Font = Settings.cFont
            .Dock = DockStyle.Fill
            .LineWrap.Mode = WrapMode.None
            .ConfigurationManager.Language = "pawn"
            .IsBraceMatching = True
            .AcceptsTab = True
            .UndoRedo.IsUndoEnabled = False
            .Encoding = Settings.Enc
            .Indentation.TabWidth = 4
            .BackColor = Settings.BackColor
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
            Dim inverted As Color = Color.FromArgb(255 - Settings.BackColor.A, 255 - Settings.BackColor.R, 255 - Settings.BackColor.G, 255 - Settings.BackColor.B)
            .BackColor = Settings.BackColor
            .Caret.Color = inverted
            If Settings.All Then
                For i = 0 To 19
                    Select Case i
                        Case 1 To 7, 9, 10, 12, 15, 17 To 19
                        Case Else
                            .Styles(i).ForeColor = inverted
                    End Select
                    .Styles(i).BackColor = Settings.BackColor
                Next
                .Styles("NUMBER").ForeColor = Settings.H_Numbers.ForeColor
                .Styles("NUMBER").Bold = Settings.H_Numbers.Bold
                .Styles("NUMBER").Italic = Settings.H_Numbers.Italic
                .Styles("STRING").ForeColor = Settings.H_String.ForeColor
                .Styles("STRING").Bold = Settings.H_String.Bold
                .Styles("STRING").Italic = Settings.H_String.Italic
                .Styles("STRINGEOL").ForeColor = Settings.H_String2.ForeColor
                .Styles("STRINGEOL").Bold = Settings.H_String2.Bold
                .Styles("STRINGEOL").Italic = Settings.H_String2.Italic
                .Styles("OPERATOR").ForeColor = Settings.H_Operator.ForeColor
                .Styles("OPERATOR").Bold = Settings.H_Operator.Bold
                .Styles("OPERATOR").Italic = Settings.H_Operator.Italic
                .Styles("CHARACTER").ForeColor = Settings.H_Chars.ForeColor
                .Styles("CHARACTER").Bold = Settings.H_Chars.Bold
                .Styles("CHARACTER").Italic = Settings.H_Chars.Italic
                .Styles("GLOBALCLASS").ForeColor = Settings.H_Class.ForeColor
                .Styles("GLOBALCLASS").Font = Font
                .Styles("GLOBALCLASS").Bold = Settings.H_Class.Bold
                .Styles("GLOBALCLASS").Italic = Settings.H_Class.Italic
                .Styles("PREPROCESSOR").ForeColor = Settings.H_Preproc.ForeColor
                .Styles("PREPROCESSOR").Font = Font
                .Styles("PREPROCESSOR").Bold = Settings.H_Preproc.Bold
                .Styles("PREPROCESSOR").Italic = Settings.H_Preproc.Italic
                .Styles("COMMENT").ForeColor = Settings.H_Comment.ForeColor
                .Styles("COMMENT").Bold = Settings.H_Comment.Bold
                .Styles("COMMENT").Italic = Settings.H_Comment.Italic
                .Styles("COMMENTLINE").ForeColor = Settings.H_Comment.ForeColor
                .Styles("COMMENTLINE").Bold = Settings.H_Comment.Bold
                .Styles("COMMENTLINE").Italic = Settings.H_Comment.Italic
                .Styles("COMMENTDOC").ForeColor = Settings.H_Comment.ForeColor
                .Styles("COMMENTDOC").Bold = Settings.H_Comment.Bold
                .Styles("COMMENTDOC").Italic = Settings.H_Comment.Italic
                .Styles("COMMENTLINEDOC").ForeColor = Settings.H_Comment.ForeColor
                .Styles("COMMENTLINEDOC").Bold = Settings.H_Comment.Bold
                .Styles("COMMENTLINEDOC").Italic = Settings.H_Comment.Italic
                .Styles("COMMENTDOCKEYWORD").ForeColor = Settings.H_Comment.ForeColor
                .Styles("COMMENTDOCKEYWORD").Bold = Settings.H_Comment.Bold
                .Styles("COMMENTDOCKEYWORD").Italic = Settings.H_Comment.Italic
                .Styles("COMMENTDOCKEYWORDERROR").ForeColor = Settings.H_Comment.ForeColor
                .Styles("COMMENTDOCKEYWORDERROR").Bold = Settings.H_Comment.Bold
                .Styles("COMMENTDOCKEYWORDERROR").Italic = Settings.H_Comment.Italic
                .Lexing.Colorize()
            Else
                For i = 0 To 19
                    Select Case i
                        Case 1 To 7, 9, 10, 12, 15, 17 To 19
                        Case Else
                            .Styles(i).ForeColor = inverted
                    End Select
                    .Styles(i).BackColor = .BackColor
                Next
                .Styles("NUMBER").BackColor = Settings.H_Numbers.BackColor
                .Styles("NUMBER").ForeColor = Settings.H_Numbers.ForeColor
                .Styles("NUMBER").Bold = Settings.H_Numbers.Bold
                .Styles("NUMBER").Italic = Settings.H_Numbers.Italic
                .Styles("STRING").BackColor = Settings.H_String.BackColor
                .Styles("STRING").ForeColor = Settings.H_String.ForeColor
                .Styles("STRING").Bold = Settings.H_String.Bold
                .Styles("STRING").Italic = Settings.H_String.Italic
                .Styles("STRINGEOL").BackColor = Settings.H_String2.BackColor
                .Styles("STRINGEOL").ForeColor = Settings.H_String2.ForeColor
                .Styles("STRINGEOL").Bold = Settings.H_String2.Bold
                .Styles("STRINGEOL").Italic = Settings.H_String2.Italic
                .Styles("OPERATOR").BackColor = Settings.H_Operator.BackColor
                .Styles("OPERATOR").ForeColor = Settings.H_Operator.ForeColor
                .Styles("OPERATOR").Bold = Settings.H_Operator.Bold
                .Styles("OPERATOR").Italic = Settings.H_Operator.Italic
                .Styles("CHARACTER").BackColor = Settings.H_Chars.BackColor
                .Styles("CHARACTER").ForeColor = Settings.H_Chars.ForeColor
                .Styles("CHARACTER").Bold = Settings.H_Chars.Bold
                .Styles("CHARACTER").Italic = Settings.H_Chars.Italic
                .Styles("GLOBALCLASS").BackColor = Settings.H_Class.BackColor
                .Styles("GLOBALCLASS").ForeColor = Settings.H_Class.ForeColor
                .Styles("GLOBALCLASS").Font = Font
                .Styles("GLOBALCLASS").Bold = Settings.H_Class.Bold
                .Styles("GLOBALCLASS").Italic = Settings.H_Class.Italic
                .Styles("PREPROCESSOR").BackColor = Settings.H_Preproc.BackColor
                .Styles("PREPROCESSOR").ForeColor = Settings.H_Preproc.ForeColor
                .Styles("PREPROCESSOR").Font = Font
                .Styles("PREPROCESSOR").Bold = Settings.H_Preproc.Bold
                .Styles("PREPROCESSOR").Italic = Settings.H_Preproc.Italic
                .Styles("COMMENT").BackColor = Settings.H_Comment.BackColor
                .Styles("COMMENT").ForeColor = Settings.H_Comment.ForeColor
                .Styles("COMMENT").Bold = Settings.H_Comment.Bold
                .Styles("COMMENT").Italic = Settings.H_Comment.Italic
                .Styles("COMMENTLINE").BackColor = Settings.H_Comment.BackColor
                .Styles("COMMENTLINE").ForeColor = Settings.H_Comment.ForeColor
                .Styles("COMMENTLINE").Bold = Settings.H_Comment.Bold
                .Styles("COMMENTLINE").Italic = Settings.H_Comment.Italic
                .Styles("COMMENTDOC").BackColor = Settings.H_Comment.BackColor
                .Styles("COMMENTDOC").ForeColor = Settings.H_Comment.ForeColor
                .Styles("COMMENTDOC").Bold = Settings.H_Comment.Bold
                .Styles("COMMENTDOC").Italic = Settings.H_Comment.Italic
                .Styles("COMMENTLINEDOC").BackColor = Settings.H_Comment.BackColor
                .Styles("COMMENTLINEDOC").ForeColor = Settings.H_Comment.ForeColor
                .Styles("COMMENTLINEDOC").Bold = Settings.H_Comment.Bold
                .Styles("COMMENTLINEDOC").Italic = Settings.H_Comment.Italic
                .Styles("COMMENTDOCKEYWORD").BackColor = Settings.H_Comment.BackColor
                .Styles("COMMENTDOCKEYWORD").ForeColor = Settings.H_Comment.ForeColor
                .Styles("COMMENTDOCKEYWORD").Bold = Settings.H_Comment.Bold
                .Styles("COMMENTDOCKEYWORD").Italic = Settings.H_Comment.Italic
                .Styles("COMMENTDOCKEYWORDERROR").BackColor = Settings.H_Comment.BackColor
                .Styles("COMMENTDOCKEYWORDERROR").ForeColor = Settings.H_Comment.ForeColor
                .Styles("COMMENTDOCKEYWORDERROR").Bold = Settings.H_Comment.Bold
                .Styles("COMMENTDOCKEYWORDERROR").Italic = Settings.H_Comment.Italic
                .Lexing.Colorize()
            End If
        End With
        With TabHandle
            .Text = name
            .Menu = New ContextMenu
            With .Menu.MenuItems
                .Add("Save", AddressOf SaveMenuItem_Click)
                .Add("Save As...", AddressOf SaveAsMenuItem_Click)
                .Add("Reload File", AddressOf ReloadFileMenuItem_Click)
            End With
            .Controls.Add(SyntaxHandle)
        End With
        SetParent(SyntaxHandle.Handle, TabHandle.Handle)
        Main.TabControl1.Controls.Add(TabHandle)
        If iwait Then
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
        On Error Resume Next
        If SyntaxHandle.AutoComplete.List Is Lists.PreCompiler Then SyntaxHandle.Selection.Text = " "
        If SyntaxHandle.CallTip.IsActive Then
            With SyntaxHandle.CallTip
                .Cancel()
                Dim func As PawnFunction, istart As Integer, iend As Integer, tmp As String = vbNullString
                func = GetFunctionByName(ACLists.Functions, GetCurrentFunction(GetLineCursorPosition(True)))
                If func.Name.Length > 0 Then
                    For Each param As String In ACLists.Functions(ACLists.Functions.IndexOf(func)).Params
                        If Not ACLists.Functions(ACLists.Functions.IndexOf(func)).Params(UBound(ACLists.Functions(ACLists.Functions.IndexOf(func)).Params)) = param Then
                            If Array.IndexOf(ACLists.Functions(ACLists.Functions.IndexOf(func)).Params, param) = GetCurrentParamIndex() Then
                                istart = tmp.Length
                                iend = istart + Len(param + ", ")
                            End If
                            tmp += param & ", "
                        Else
                            If Array.IndexOf(ACLists.Functions(ACLists.Functions.IndexOf(func)).Params, param) = GetCurrentParamIndex() Then
                                istart = tmp.Length
                                iend = istart + Len(param)
                            End If
                            tmp += param
                        End If
                    Next
                    .HighlightTextColor = Color.Blue
                    .Show(tmp, SyntaxHandle.CurrentPos, istart, iend)
                End If
            End With
        End If
    End Sub

    Private Sub SyntaxHandle_CharAdded(ByVal sender As Object, ByVal e As ScintillaNet.CharAddedEventArgs) Handles SyntaxHandle.CharAdded
        On Error Resume Next
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
                    If SyntaxHandle.CallTip.IsActive Then SyntaxHandle.CallTip.Cancel()
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
                Dim func As PawnFunction = GetFunctionByName(ACLists.Functions, GetCurrentFunction(GetLineCursorPosition(True), False, True))
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
                                        If SyntaxHandle.CallTip.IsActive Then SyntaxHandle.CallTip.Cancel()
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
                                If SyntaxHandle.CallTip.IsActive Then SyntaxHandle.CallTip.Cancel()
                                .Show()
                            End With
                        ElseIf func.Params(0).IndexOf("DBResult:") > -1 AndAlso ACLists.DbRes.Count > 0 Then
                            With SyntaxHandle.AutoComplete
                                .List.Clear()
                                For Each d In ACLists.DbRes
                                    .List.Add(d)
                                Next
                                If ACLists.DbRes.Count = 1 Then .List.Add("-")
                                If SyntaxHandle.CallTip.IsActive Then SyntaxHandle.CallTip.Cancel()
                                .Show()
                            End With
                        ElseIf func.Params(0).IndexOf("Menu:") > -1 AndAlso ACLists.Menus.Count > 0 Then
                            With SyntaxHandle.AutoComplete
                                .List.Clear()
                                For Each m In ACLists.Menus
                                    .List.Add(m)
                                Next
                                If ACLists.Menus.Count = 1 Then .List.Add("-")
                                If SyntaxHandle.CallTip.IsActive Then SyntaxHandle.CallTip.Cancel()
                                .Show()
                            End With
                        ElseIf func.Params(0).IndexOf("Text:") > -1 AndAlso ACLists.Texts.Count > 0 Then
                            With SyntaxHandle.AutoComplete
                                .List.Clear()
                                For Each t In ACLists.Texts
                                    .List.Add(t)
                                Next
                                If ACLists.Texts.Count = 1 Then .List.Add("-")
                                If SyntaxHandle.CallTip.IsActive Then SyntaxHandle.CallTip.Cancel()
                                .Show()
                            End With
                        ElseIf func.Params(0).IndexOf("Text3D:") > -1 AndAlso ACLists.Texts2.Count > 0 Then
                            With SyntaxHandle.AutoComplete
                                .List.Clear()
                                For Each t In ACLists.Texts2
                                    .List.Add(t)
                                Next
                                If ACLists.Texts2.Count = 1 Then .List.Add("-")
                                If SyntaxHandle.CallTip.IsActive Then SyntaxHandle.CallTip.Cancel()
                                .Show()
                            End With
                        ElseIf func.Params(0).IndexOf("Float:") > -1 AndAlso ACLists.Floats.Count > 0 Then
                            With SyntaxHandle.AutoComplete
                                .List.Clear()
                                For Each f In ACLists.Floats
                                    .List.Add(f)
                                Next
                                If ACLists.Floats.Count = 1 Then .List.Add("-")
                                If SyntaxHandle.CallTip.IsActive Then SyntaxHandle.CallTip.Cancel()
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
                                        If SyntaxHandle.CallTip.IsActive Then SyntaxHandle.CallTip.Cancel()
                                        .Show()
                                    End With
                                Case "style", "Style"
                                    Select Case GetCurrentFunction(GetLineCursorPosition(True))
                                        Case "SetPlayerFightingStyle"
                                            With SyntaxHandle.AutoComplete
                                                .List.Clear()
                                                For Each i In Lists.FightingTypes
                                                    .List.Add(i)
                                                Next
                                                If SyntaxHandle.CallTip.IsActive Then SyntaxHandle.CallTip.Cancel()
                                                .Show()
                                            End With
                                        Case "GameTextForAll", "GameTextForPlayer"
                                            With SyntaxHandle.AutoComplete
                                                .List.Clear()
                                                For i = 0 To 6
                                                    .List.Add(i)
                                                Next
                                                If SyntaxHandle.CallTip.IsActive Then SyntaxHandle.CallTip.Cancel()
                                                .Show()
                                            End With
                                        Case "ShowPlayerDialog"
                                            With SyntaxHandle.AutoComplete
                                                .List.Clear()
                                                For Each i In Lists.DialogTypes
                                                    .List.Add(i)
                                                Next
                                                If SyntaxHandle.CallTip.IsActive Then SyntaxHandle.CallTip.Cancel()
                                                .Show()
                                            End With
                                    End Select
                                Case "callback", "Callback"
                                    With SyntaxHandle.AutoComplete
                                        .List.Clear()
                                        For Each i In ACLists.Callbacks
                                            .List.Add(i.Name)
                                        Next
                                        If SyntaxHandle.CallTip.IsActive Then SyntaxHandle.CallTip.Cancel()
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
                                                If SyntaxHandle.CallTip.IsActive Then SyntaxHandle.CallTip.Cancel()
                                                .Show()
                                            End With
                                        Case "ShowPlayerMarkers"
                                            With SyntaxHandle.AutoComplete
                                                .List.Clear()
                                                For Each i In Lists.MarkerTypes
                                                    .List.Add(i)
                                                Next
                                                If SyntaxHandle.CallTip.IsActive Then SyntaxHandle.CallTip.Cancel()
                                                .Show()
                                            End With
                                        Case "fopen"
                                            With SyntaxHandle.AutoComplete
                                                .List.Clear()
                                                For Each i In Lists.FileTypes
                                                    .List.Add(i)
                                                Next
                                                If SyntaxHandle.CallTip.IsActive Then SyntaxHandle.CallTip.Cancel()
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
                                            If SyntaxHandle.CallTip.IsActive Then SyntaxHandle.CallTip.Cancel()
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
                                            If SyntaxHandle.CallTip.IsActive Then SyntaxHandle.CallTip.Cancel()
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
                                            If SyntaxHandle.CallTip.IsActive Then SyntaxHandle.CallTip.Cancel()
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
                                            If SyntaxHandle.CallTip.IsActive Then SyntaxHandle.CallTip.Cancel()
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
                                            If SyntaxHandle.CallTip.IsActive Then SyntaxHandle.CallTip.Cancel()
                                            .Show()
                                        End With
                                    End If
                                Case Else
                                    With SyntaxHandle.CallTip
                                        If .IsActive Then
                                            .Cancel()
                                            Dim istart As Integer, iend As Integer, tmp As String = vbNullString
                                            For Each param As String In ACLists.Functions(ACLists.Functions.IndexOf(func)).Params
                                                If Not ACLists.Functions(ACLists.Functions.IndexOf(func)).Params(UBound(ACLists.Functions(ACLists.Functions.IndexOf(func)).Params)) = param Then
                                                    If Array.IndexOf(ACLists.Functions(ACLists.Functions.IndexOf(func)).Params, param) = 0 Then
                                                        istart = tmp.Length
                                                        iend = istart + Len(param + ", ")
                                                    End If
                                                    tmp += param & ", "
                                                Else
                                                    If Array.IndexOf(ACLists.Functions(ACLists.Functions.IndexOf(func)).Params, param) = 0 Then
                                                        istart = tmp.Length
                                                        iend = istart + Len(param)
                                                    End If
                                                    tmp += param
                                                End If
                                            Next
                                            .HighlightTextColor = Color.Blue
                                            .Show(tmp, SyntaxHandle.CurrentPos, istart, iend)
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
                Dim func As PawnFunction = GetFunctionByName(ACLists.Functions, GetCurrentFunction(GetLineCursorPosition(True), False, True))
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
                                        If SyntaxHandle.CallTip.IsActive Then SyntaxHandle.CallTip.Cancel()
                                    End With
                            End Select
                        ElseIf func.Params(index).IndexOf("DB:") > -1 AndAlso ACLists.Dbs.Count > 0 Then
                            With SyntaxHandle.AutoComplete
                                .List.Clear()
                                For Each d In ACLists.Dbs
                                    .List.Add(d)
                                Next
                                If ACLists.Dbs.Count = 1 Then .List.Add("-")
                                If SyntaxHandle.CallTip.IsActive Then SyntaxHandle.CallTip.Cancel()
                                .Show()
                            End With
                        ElseIf func.Params(index).IndexOf("DBResult:") > -1 AndAlso ACLists.DbRes.Count > 0 Then
                            With SyntaxHandle.AutoComplete
                                .List.Clear()
                                For Each d In ACLists.DbRes
                                    .List.Add(d)
                                Next
                                If ACLists.DbRes.Count = 1 Then .List.Add("-")
                                If SyntaxHandle.CallTip.IsActive Then SyntaxHandle.CallTip.Cancel()
                                .Show()
                            End With
                        ElseIf func.Params(index).IndexOf("Menu:") > -1 AndAlso ACLists.Menus.Count > 0 Then
                            With SyntaxHandle.AutoComplete
                                .List.Clear()
                                For Each m In ACLists.Menus
                                    .List.Add(m)
                                Next
                                If ACLists.Menus.Count = 1 Then .List.Add("-")
                                If SyntaxHandle.CallTip.IsActive Then SyntaxHandle.CallTip.Cancel()
                                .Show()
                            End With
                        ElseIf func.Params(index).IndexOf("Text:") > -1 AndAlso ACLists.Texts.Count > 0 Then
                            With SyntaxHandle.AutoComplete
                                .List.Clear()
                                For Each t In ACLists.Texts
                                    .List.Add(t)
                                Next
                                If ACLists.Texts.Count = 1 Then .List.Add("-")
                                If SyntaxHandle.CallTip.IsActive Then SyntaxHandle.CallTip.Cancel()
                                .Show()
                            End With
                        ElseIf func.Params(index).IndexOf("Text3D:") > -1 AndAlso ACLists.Texts2.Count > 0 Then
                            With SyntaxHandle.AutoComplete
                                .List.Clear()
                                For Each t In ACLists.Texts2
                                    .List.Add(t)
                                Next
                                If ACLists.Texts2.Count = 1 Then .List.Add("-")
                                If SyntaxHandle.CallTip.IsActive Then SyntaxHandle.CallTip.Cancel()
                                .Show()
                            End With
                        ElseIf func.Params(index).IndexOf("Float:") > -1 AndAlso ACLists.Floats.Count > 0 Then
                            With SyntaxHandle.AutoComplete
                                .List.Clear()
                                For Each f In ACLists.Floats
                                    .List.Add(f)
                                Next
                                If ACLists.Floats.Count = 1 Then .List.Add("-")
                                If SyntaxHandle.CallTip.IsActive Then SyntaxHandle.CallTip.Cancel()
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
                                        If SyntaxHandle.CallTip.IsActive Then SyntaxHandle.CallTip.Cancel()
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
                                                If SyntaxHandle.CallTip.IsActive Then SyntaxHandle.CallTip.Cancel()
                                                .Show()
                                            End With
                                        Case "GameTextForAll", "GameTextForPlayer"
                                            With SyntaxHandle.AutoComplete
                                                .List.Clear()
                                                For i = 0 To 6
                                                    .List.Add(i)
                                                Next
                                                If SyntaxHandle.CallTip.IsActive Then SyntaxHandle.CallTip.Cancel()
                                                .Show()
                                            End With
                                        Case "ShowPlayerDialog"
                                            With SyntaxHandle.AutoComplete
                                                .List.Clear()
                                                For Each i In Lists.DialogTypes
                                                    .List.Add(i)
                                                Next
                                                If SyntaxHandle.CallTip.IsActive Then SyntaxHandle.CallTip.Cancel()
                                                .Show()
                                            End With
                                    End Select
                                Case "callback", "Callback"
                                    With SyntaxHandle.AutoComplete
                                        .List.Clear()
                                        For Each i In ACLists.Callbacks
                                            .List.Add(i.Name)
                                        Next
                                        If SyntaxHandle.CallTip.IsActive Then SyntaxHandle.CallTip.Cancel()
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
                                                If SyntaxHandle.CallTip.IsActive Then SyntaxHandle.CallTip.Cancel()
                                                .Show()
                                            End With
                                        Case "ShowPlayerMarkers"
                                            With SyntaxHandle.AutoComplete
                                                .List.Clear()
                                                For Each i In Lists.MarkerTypes
                                                    .List.Add(i)
                                                Next
                                                If SyntaxHandle.CallTip.IsActive Then SyntaxHandle.CallTip.Cancel()
                                                .Show()
                                            End With
                                        Case "fopen"
                                            With SyntaxHandle.AutoComplete
                                                .List.Clear()
                                                For Each i In Lists.FileTypes
                                                    .List.Add(i)
                                                Next
                                                If SyntaxHandle.CallTip.IsActive Then SyntaxHandle.CallTip.Cancel()
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
                                            If SyntaxHandle.CallTip.IsActive Then SyntaxHandle.CallTip.Cancel()
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
                                            If SyntaxHandle.CallTip.IsActive Then SyntaxHandle.CallTip.Cancel()
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
                                            If SyntaxHandle.CallTip.IsActive Then SyntaxHandle.CallTip.Cancel()
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
                                            If SyntaxHandle.CallTip.IsActive Then SyntaxHandle.CallTip.Cancel()
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
                                            If SyntaxHandle.CallTip.IsActive Then SyntaxHandle.CallTip.Cancel()
                                            .Show()
                                        End With
                                    End If
                                Case Else
                                    With SyntaxHandle.CallTip
                                        If .IsActive Then
                                            .Cancel()
                                            Dim istart As Integer, iend As Integer, tmp As String = vbNullString
                                            For Each param As String In ACLists.Functions(ACLists.Functions.IndexOf(func)).Params
                                                If Not ACLists.Functions(ACLists.Functions.IndexOf(func)).Params(UBound(ACLists.Functions(ACLists.Functions.IndexOf(func)).Params)) = param Then
                                                    If Array.IndexOf(ACLists.Functions(ACLists.Functions.IndexOf(func)).Params, param) = index Then
                                                        istart = tmp.Length
                                                        iend = istart + Len(param + ", ")
                                                    End If
                                                    tmp += param & ", "
                                                Else
                                                    If Array.IndexOf(ACLists.Functions(ACLists.Functions.IndexOf(func)).Params, param) = index Then
                                                        istart = tmp.Length
                                                        iend = istart + Len(param)
                                                    End If
                                                    tmp += param
                                                End If
                                            Next
                                            .HighlightTextColor = Color.Blue
                                            .Show(tmp, SyntaxHandle.CurrentPos, istart, iend)
                                        Else
                                            Tim.Enabled = True
                                        End If
                                    End With
                            End Select
                        End If
                    Else
                        index = UBound(func.Params)
                        If func.Params(index).IndexOf("...") > -1 Then
                            With SyntaxHandle.CallTip
                                If .IsActive Then
                                    .Cancel()
                                    Dim istart As Integer, iend As Integer, tmp As String = vbNullString
                                    For Each param As String In ACLists.Functions(ACLists.Functions.IndexOf(func)).Params
                                        If Not ACLists.Functions(ACLists.Functions.IndexOf(func)).Params(UBound(ACLists.Functions(ACLists.Functions.IndexOf(func)).Params)) = param Then
                                            If Array.IndexOf(ACLists.Functions(ACLists.Functions.IndexOf(func)).Params, param) = index Then
                                                istart = tmp.Length
                                                iend = istart + Len(param + ", ")
                                            End If
                                            tmp += param & ", "
                                        Else
                                            If Array.IndexOf(ACLists.Functions(ACLists.Functions.IndexOf(func)).Params, param) = index Then
                                                istart = tmp.Length
                                                iend = istart + Len(param)
                                            End If
                                            tmp += param
                                        End If
                                    Next
                                    .HighlightTextColor = Color.Blue
                                    .Show(tmp, SyntaxHandle.CurrentPos, istart, iend)
                                End If
                            End With
                        Else
                            With SyntaxHandle.CallTip
                                If .IsActive Then
                                    .Cancel()
                                    Dim tmp As String = vbNullString
                                    For Each param As String In ACLists.Functions(ACLists.Functions.IndexOf(func)).Params
                                        If Not ACLists.Functions(ACLists.Functions.IndexOf(func)).Params(UBound(ACLists.Functions(ACLists.Functions.IndexOf(func)).Params)) = param Then
                                            tmp += param & ", "
                                        Else
                                            tmp += param
                                        End If
                                    Next
                                    .Show(tmp, SyntaxHandle.CurrentPos)
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
                Dim func As PawnFunction = GetFunctionByName(ACLists.Functions, GetCurrentFunction(GetLineCursorPosition(True), False, True))
                If Trim(func.Name) = "" AndAlso SyntaxHandle.CallTip.IsActive Then
                    SyntaxHandle.CallTip.Cancel()
                Else
                    With SyntaxHandle.CallTip
                        If .IsActive Then
                            .Cancel()
                            Dim istart As Integer, iend As Integer, tmp As String = vbNullString
                            For Each param As String In ACLists.Functions(ACLists.Functions.IndexOf(func)).Params
                                If Not ACLists.Functions(ACLists.Functions.IndexOf(func)).Params(UBound(ACLists.Functions(ACLists.Functions.IndexOf(func)).Params)) = param Then
                                    If Array.IndexOf(ACLists.Functions(ACLists.Functions.IndexOf(func)).Params, param) = GetCurrentParamIndex() Then
                                        istart = tmp.Length
                                        iend = istart + Len(param + ", ")
                                    End If
                                    tmp += param & ", "
                                Else
                                    If Array.IndexOf(ACLists.Functions(ACLists.Functions.IndexOf(func)).Params, param) = GetCurrentParamIndex() Then
                                        istart = tmp.Length
                                        iend = istart + Len(param)
                                    End If
                                    tmp += param
                                End If
                            Next
                            .HighlightTextColor = Color.Blue
                            .Show(tmp, SyntaxHandle.CurrentPos, istart, iend)
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
                                If SyntaxHandle.CallTip.IsActive Then SyntaxHandle.CallTip.Cancel()
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
                    If SyntaxHandle.Lines.Current.Text.IndexOf("#inc") > -1 Then
                        SyntaxHandle.Invoke(DataUpdaterEx, New Object() {UpdateType.Includes, If(SyntaxHandle.Lines.Current.Number > 2, SyntaxHandle.Lines.Current.Number - 2, SyntaxHandle.Lines.Current.Number), If(SyntaxHandle.Lines.Current.Number + 1 < SyntaxHandle.Lines.Count, If(SyntaxHandle.Lines.Current.Number + 2 < SyntaxHandle.Lines.Count, SyntaxHandle.Lines.Current.Number + 2, SyntaxHandle.Lines.Current.Number + 1), SyntaxHandle.Lines.Current.Number)})
                    ElseIf (SyntaxHandle.Lines.Current.Text.IndexOf("stock") > -1 Or SyntaxHandle.Lines.Current.Text.IndexOf("public") > -1) AndAlso SyntaxHandle.Lines.Current.Text.IndexOf("(") > -1 AndAlso SyntaxHandle.Lines.Current.Text.IndexOf(")") > -1 Then
                        SyntaxHandle.Invoke(DataUpdaterEx, New Object() {UpdateType.Functions_Callbacks, If(SyntaxHandle.Lines.Current.Number > 2, SyntaxHandle.Lines.Current.Number - 2, SyntaxHandle.Lines.Current.Number), If(SyntaxHandle.Lines.Current.Number + 1 < SyntaxHandle.Lines.Count, If(SyntaxHandle.Lines.Current.Number + 2 < SyntaxHandle.Lines.Count, SyntaxHandle.Lines.Current.Number + 2, SyntaxHandle.Lines.Current.Number + 1), SyntaxHandle.Lines.Current.Number)})
                    ElseIf SyntaxHandle.Lines.Current.Text.IndexOf("#def") > -1 Then
                        SyntaxHandle.Invoke(DataUpdaterEx, New Object() {UpdateType.Colors, If(SyntaxHandle.Lines.Current.Number > 2, SyntaxHandle.Lines.Current.Number - 2, SyntaxHandle.Lines.Current.Number), If(SyntaxHandle.Lines.Current.Number + 1 < SyntaxHandle.Lines.Count, If(SyntaxHandle.Lines.Current.Number + 2 < SyntaxHandle.Lines.Count, SyntaxHandle.Lines.Current.Number + 2, SyntaxHandle.Lines.Current.Number + 1), SyntaxHandle.Lines.Current.Number)})
                    ElseIf SyntaxHandle.Lines.Current.Text.IndexOf("new") > -1 Then
                        SyntaxHandle.Invoke(DataUpdaterEx, New Object() {UpdateType.Other, If(SyntaxHandle.Lines.Current.Number > 2, SyntaxHandle.Lines.Current.Number - 2, SyntaxHandle.Lines.Current.Number), If(SyntaxHandle.Lines.Current.Number + 1 < SyntaxHandle.Lines.Count, If(SyntaxHandle.Lines.Current.Number + 2 < SyntaxHandle.Lines.Count, SyntaxHandle.Lines.Current.Number + 2, SyntaxHandle.Lines.Current.Number + 1), SyntaxHandle.Lines.Current.Number)})
                    End If
                End If
            Case Keys.Delete, Keys.Back
                If SyntaxHandle.CallTip.IsActive Then
                    If GetCurrentFunction(GetLineCursorPosition(True)) = "" AndAlso SyntaxHandle.CallTip.IsActive Then
                        SyntaxHandle.CallTip.Cancel()
                    Else
                        With SyntaxHandle.CallTip
                            .Cancel()
                            Dim istart As Integer, iend As Integer, tmp As String = vbNullString, func As PawnFunction, index As Integer
                            func = GetFunctionByName(ACLists.Functions, GetCurrentFunction(GetLineCursorPosition(True), True, True))
                            index = If(SyntaxHandle.Lines.Current.Text(GetLineCursorPosition(False) - 1) = ",", GetCurrentParamIndex(True, True), GetCurrentParamIndex(False, True))
                            If TrueContainsFunction(ACLists.Functions, func) Then
                                If index > -1 AndAlso index < func.Params.Length Then
                                    For Each param As String In ACLists.Functions(ACLists.Functions.IndexOf(func)).Params
                                        If Not ACLists.Functions(ACLists.Functions.IndexOf(func)).Params(UBound(ACLists.Functions(ACLists.Functions.IndexOf(func)).Params)) = param Then
                                            If Array.IndexOf(ACLists.Functions(ACLists.Functions.IndexOf(func)).Params, param) = index Then
                                                istart = tmp.Length
                                                iend = istart + Len(param + ", ")
                                            End If
                                            tmp += param & ", "
                                        Else
                                            If Array.IndexOf(ACLists.Functions(ACLists.Functions.IndexOf(func)).Params, param) = index Then
                                                istart = tmp.Length
                                                iend = istart + Len(param)
                                            End If
                                            tmp += param
                                        End If
                                    Next
                                    .HighlightTextColor = Color.Blue
                                    .Show(tmp, SyntaxHandle.CurrentPos, istart, iend)
                                Else
                                    index = UBound(func.Params)
                                    If func.Params(index).IndexOf("...") > -1 Then
                                        With SyntaxHandle.CallTip
                                            If .IsActive Then
                                                .Cancel()
                                                For Each param As String In ACLists.Functions(ACLists.Functions.IndexOf(func)).Params
                                                    If Not ACLists.Functions(ACLists.Functions.IndexOf(func)).Params(UBound(ACLists.Functions(ACLists.Functions.IndexOf(func)).Params)) = param Then
                                                        If Array.IndexOf(ACLists.Functions(ACLists.Functions.IndexOf(func)).Params, param) = index Then
                                                            istart = tmp.Length
                                                            iend = istart + Len(param + ", ")
                                                        End If
                                                        tmp += param & ", "
                                                    Else
                                                        If Array.IndexOf(ACLists.Functions(ACLists.Functions.IndexOf(func)).Params, param) = index Then
                                                            istart = tmp.Length
                                                            iend = istart + Len(param)
                                                        End If
                                                        tmp += param
                                                    End If
                                                Next
                                                .HighlightTextColor = Color.Blue
                                                .Show(tmp, SyntaxHandle.CurrentPos, istart, iend)
                                            End If
                                        End With
                                    Else
                                        With SyntaxHandle.CallTip
                                            If .IsActive Then
                                                .Cancel()
                                                For Each param As String In ACLists.Functions(ACLists.Functions.IndexOf(func)).Params
                                                    If Not ACLists.Functions(ACLists.Functions.IndexOf(func)).Params(UBound(ACLists.Functions(ACLists.Functions.IndexOf(func)).Params)) = param Then
                                                        tmp += param & ", "
                                                    Else
                                                        tmp += param
                                                    End If
                                                Next
                                                .Show(tmp, SyntaxHandle.CurrentPos)
                                            End If
                                        End With
                                    End If
                                End If
                            Else
                                SyntaxHandle.CallTip.Cancel()
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
                If SyntaxHandle.CallTip.IsActive Then
                    If GetCurrentFunction(GetLineCursorPosition(True)) = "" AndAlso SyntaxHandle.CallTip.IsActive Then
                        SyntaxHandle.CallTip.Cancel()
                    Else
                        With SyntaxHandle.CallTip
                            .Cancel()
                            Dim istart As Integer, iend As Integer, tmp As String = vbNullString, func As PawnFunction, index As Integer
                            func = GetFunctionByName(ACLists.Functions, GetCurrentFunction(GetLineCursorPosition(True), True, True))
                            index = If(SyntaxHandle.Lines.Current.Text(GetLineCursorPosition() - 1) = ",", GetCurrentParamIndex(True, True), GetCurrentParamIndex(False, True))
                            If TrueContainsFunction(ACLists.Functions, func) Then
                                If index > -1 AndAlso index < func.Params.Length Then
                                    For Each param As String In ACLists.Functions(ACLists.Functions.IndexOf(func)).Params
                                        If Not ACLists.Functions(ACLists.Functions.IndexOf(func)).Params(UBound(ACLists.Functions(ACLists.Functions.IndexOf(func)).Params)) = param Then
                                            If Array.IndexOf(ACLists.Functions(ACLists.Functions.IndexOf(func)).Params, param) = index Then
                                                istart = tmp.Length
                                                iend = istart + Len(param + ", ")
                                            End If
                                            tmp += param & ", "
                                        Else
                                            If Array.IndexOf(ACLists.Functions(ACLists.Functions.IndexOf(func)).Params, param) = index Then
                                                istart = tmp.Length
                                                iend = istart + Len(param)
                                            End If
                                            tmp += param
                                        End If
                                    Next
                                    .HighlightTextColor = Color.Blue
                                    .Show(tmp, SyntaxHandle.CurrentPos, istart, iend)
                                Else
                                    index = UBound(func.Params)
                                    If func.Params(index).IndexOf("...") > -1 Then
                                        With SyntaxHandle.CallTip
                                            If .IsActive Then
                                                .Cancel()
                                                For Each param As String In ACLists.Functions(ACLists.Functions.IndexOf(func)).Params
                                                    If Not ACLists.Functions(ACLists.Functions.IndexOf(func)).Params(UBound(ACLists.Functions(ACLists.Functions.IndexOf(func)).Params)) = param Then
                                                        If Array.IndexOf(ACLists.Functions(ACLists.Functions.IndexOf(func)).Params, param) = index Then
                                                            istart = tmp.Length
                                                            iend = istart + Len(param + ", ")
                                                        End If
                                                        tmp += param & ", "
                                                    Else
                                                        If Array.IndexOf(ACLists.Functions(ACLists.Functions.IndexOf(func)).Params, param) = index Then
                                                            istart = tmp.Length
                                                            iend = istart + Len(param)
                                                        End If
                                                        tmp += param
                                                    End If
                                                Next
                                                .HighlightTextColor = Color.Blue
                                                .Show(tmp, SyntaxHandle.CurrentPos, istart, iend)
                                            End If
                                        End With
                                    Else
                                        With SyntaxHandle.CallTip
                                            If .IsActive Then
                                                .Cancel()
                                                For Each param As String In ACLists.Functions(ACLists.Functions.IndexOf(func)).Params
                                                    If Not ACLists.Functions(ACLists.Functions.IndexOf(func)).Params(UBound(ACLists.Functions(ACLists.Functions.IndexOf(func)).Params)) = param Then
                                                        tmp += param & ", "
                                                    Else
                                                        tmp += param
                                                    End If
                                                Next
                                                .HighlightTextColor = Color.Blue
                                                .Show(tmp, SyntaxHandle.CurrentPos)
                                            End If
                                        End With
                                    End If
                                End If
                            Else
                                SyntaxHandle.CallTip.Cancel()
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
                If SyntaxHandle.CallTip.IsActive Then SyntaxHandle.CallTip.Cancel()
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
                If GetCurrentFunction(GetLineCursorPosition(True)) = "" AndAlso SyntaxHandle.CallTip.IsActive Then SyntaxHandle.CallTip.Cancel()
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
        On Error Resume Next
        If SyntaxHandle.Lines.Count = 1 AndAlso SyntaxHandle.Text.Length = 0 Then SyntaxHandle.Text = vbNullString
        With SyntaxHandle
            If wait AndAlso first Then
                Saved = True
                first = False
                wait = False
                .Invoke(MarginUpdater)
                If _Path Is Nothing OrElse _Path.Length = 0 OrElse Not File.Exists(_Path) Then
                    .Invoke(DataUpdater)
                Else
                    If _Saved Then
                        .Invoke(DataFileUpdater)
                    Else
                        Dim tmp As String = My.Application.Info.DirectoryPath & "\tmp_" & _Name & ".tmp"
                        If File.Exists(tmp) Then File.Delete(tmp)
                        Dim Writer As New StreamWriter(tmp)
                        Writer.Write(.Text)
                        Writer.Close()
                        .Invoke(DataFileUpdater)
                        File.Delete(tmp)
                    End If
                End If
                .UndoRedo.IsUndoEnabled = True
                For Each Line As ScintillaNet.Line In .Lines
                    If Line.IsFoldPoint AndAlso Line.FoldExpanded Then Line.ToggleFoldExpanded()
                Next
            Else
                If .UndoRedo.CanUndo Then
                    Main.ToolStripButton7.Enabled = True
                    Main.UndoToolStripMenuItem.Enabled = True
                Else
                    Main.ToolStripButton7.Enabled = False
                    Main.UndoToolStripMenuItem.Enabled = False
                End If
                If .UndoRedo.CanRedo Then
                    Main.ToolStripButton8.Enabled = True
                    Main.RedoToolStripMenuItem.Enabled = True
                Else
                    Main.ToolStripButton8.Enabled = False
                    Main.RedoToolStripMenuItem.Enabled = False
                End If
                Saved = If(justreloaded, True, False)
            End If
        End With
    End Sub

    Private Sub SaveMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        On Error Resume Next
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

    Private Sub ReloadFileMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        On Error Resume Next
        If Not _Saved Then
            Dim Res As MsgBoxResult
            Select Case Settings.Language
                Case Languages.English
                    Res = MsgBox("Do you want to save changes from """ & _Name & """?", MsgBoxStyle.YesNoCancel, "Closing")
                Case Languages.Español
                    Res = MsgBox("¿Quieres guardar los cambios de """ & _Name & """?", MsgBoxStyle.YesNoCancel, "Closing")
                Case Languages.Portuguêse
                    Res = MsgBox("Você deseja salvar as alterações de """ & _Name & """?", MsgBoxStyle.YesNoCancel, "Closing")
                Case Else
                    Res = MsgBox("Wollen Sie Änderungen von """ & _Name & """ zu retten?""", MsgBoxStyle.YesNoCancel, "Closing")
            End Select
            Select Case Res
                Case MsgBoxResult.Yes
                    If Not _Path Is Nothing AndAlso _Path.Length > 0 Then
                        Dim Writer As New StreamWriter(_Path, False, System.Text.Encoding.GetEncoding(28591))
                        Writer.Write(SyntaxHandle.Text)
                        Writer.Close()
                        _Saved = True
                    Else
                        Main.SFD.InitialDirectory = Settings.DefaultPath
                        Main.SFD.ShowDialog()
                        If Not Main.SFD.FileName Is Nothing AndAlso Main.SFD.FileName.Length > 0 Then
                            Dim Writer As New StreamWriter(Main.SFD.FileName, False, System.Text.Encoding.GetEncoding(28591))
                            Writer.Write(SyntaxHandle.Text)
                            Writer.Close()
                            _Saved = True
                            _Path = Main.SFD.FileName
                            _Name = Mid(Main.SFD.FileName, Main.SFD.FileName.LastIndexOf("\") + 2, Main.SFD.FileName.LastIndexOf(".") - Main.SFD.FileName.LastIndexOf("\") - 1)
                        End If
                    End If
                Case MsgBoxResult.Cancel
                    Exit Sub
            End Select
            If _Path <> "{Empty Doc}" Then
                Dim Reader As New StreamReader(_Path, System.Text.Encoding.GetEncoding(28591), True)
                SyntaxHandle.Text = Reader.ReadToEnd()
                Reader.Close()
            Else
                SyntaxHandle.Text = ""
            End If
        Else
            If _Path <> "{Empty Doc}" Then
                Dim Reader As New StreamReader(_Path, System.Text.Encoding.GetEncoding(28591), True)
                With SyntaxHandle
                    .Text = Reader.ReadToEnd()
                    .Invoke(MarginUpdater)
                    .Invoke(DataUpdater)
                    .UndoRedo.IsUndoEnabled = True
                    For Each Line As ScintillaNet.Line In .Lines
                        If Line.IsFoldPoint AndAlso Line.FoldExpanded Then Line.ToggleFoldExpanded()
                    Next
                End With
                Reader.Close()
            Else
                SyntaxHandle.Text = ""
            End If
        End If
        justreloaded = True
    End Sub

    Private Sub Tim_Tick(ByVal sender As Object, ByVal e As Timers.ElapsedEventArgs) Handles Tim.Elapsed
        Dim hWnd As IntPtr = FindWindow(vbNullString, "Scripting Machine")
        PostMessageA(hWnd, WM_HOTKEY, 9303, vbNull)
        Tim.Enabled = False
    End Sub

#End Region

#Region "Functions"

    Private Function GetCurrentFunction(ByVal StartPos As Integer, Optional ByVal remove As Boolean = False, Optional ByVal must As Boolean = False) As String
        On Error Resume Next
        Static func As String, lastcall As Long
        Dim calrest As Long = GetTickCount() - lastcall
        If lastcall = 0 OrElse (calrest) > 3000 Then
            func = StrReverse(Mid(SyntaxHandle.Lines.Current.Text.Replace(vbCrLf, "").Replace(vbTab, ""), 1, SyntaxHandle.Lines.Current.Text.Length))
            Dim lenght As Integer
            If StartPos = -1 OrElse func.Length - StartPos < 1 OrElse func.Length - StartPos > func.Length Then : lenght = 0
            Else : lenght = func.Length - StartPos
            End If
            func = func.Remove(0, lenght)
            If func.StartsWith(";") OrElse ((func.EndsWith("#") OrElse func.EndsWith("//")) AndAlso func.IndexOf("""") = -1) Then
                lastcall = GetTickCount()
                Return ""
            End If
            Dim tpos As Integer = func.IndexOf("""")
            If tpos > -1 Then
                tpos = func.IndexOf("""", tpos + 1)
                If tpos > -1 Then
                    If func(tpos - 1) = "\" Then
                        If func.IndexOf("""", tpos + 1) = -1 Then
                            func = func.Remove(0, tpos + 1)
                        Else
                            tpos = func.IndexOf("""", tpos + 1)
                            While tpos > -1
                                If func(tpos - 1) = "\" Then tpos = func.IndexOf("""", tpos + 1)
                                func = func.Remove(func.IndexOf(""""), tpos - func.IndexOf("""") + 1)
                                tpos = func.IndexOf("""")
                                If tpos > -1 OrElse func(tpos - 1) = "\" Then
                                    tpos = func.IndexOf("""", tpos + 1)
                                Else
                                    Exit While
                                End If
                            End While
                        End If
                    Else
                        While tpos > -1
                            func = func.Remove(func.IndexOf(""""), tpos - func.IndexOf("""") + 1)
                            tpos = func.IndexOf("""")
                            If tpos > -1 Then
                                tpos = func.IndexOf("""", tpos + 1)
                            Else
                                Exit While
                            End If
                        End While
                    End If
                Else
                    func = func.Remove(0, func.IndexOf("""") + 1)
                End If
            End If
            If func.IndexOf(";") > -1 Then
                lastcall = GetTickCount()
                Return ""
            End If
            If func.StartsWith("(") AndAlso func.IndexOf(",") > -1 Then func = func.Remove(func.IndexOf(","), func.Length - func.IndexOf(","))
            If remove AndAlso func.StartsWith("(") Then func = func.Remove(0, 1)
            While func.IndexOf("(") > -1 AndAlso func.IndexOf(")") > -1
                If func.IndexOf("(", func.IndexOf("(") + 1) > -1 Then
                    tpos = func.IndexOf(")(")
                    If tpos = -1 Then
                        func = func.Remove(func.LastIndexOf(")"), func.IndexOf("(", func.LastIndexOf(")") + 1) - func.LastIndexOf(")") + 1)
                    Else
                        If func.IndexOf("(", tpos + 1) > -1 Then
                            func = func.Remove(tpos, func.IndexOf("(", tpos + 2) - tpos)
                        Else
                            func = func.Remove(func.IndexOf(")"), func.Length - func.IndexOf(")"))
                        End If
                    End If
                Else
                    If func.IndexOf(")(") = -1 Then
                        If func.EndsWith("fi") Then
                            func = func.Remove(func.LastIndexOf(")"))
                        Else
                            func = func.Remove(func.LastIndexOf(")"), func.IndexOf("(") - func.LastIndexOf(")") + 1)
                        End If
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
                            lastcall = GetTickCount()
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
                lastcall = GetTickCount()
                Return ""
            End If
            lastcall = GetTickCount()
            Return Trim(StrReverse(func.Replace("(", "").Replace(",", "")))
        Else
            Return Trim(StrReverse(func.Replace("(", "")))
        End If
    End Function

    Private Function GetCurrentParamIndex(Optional ByVal fix As Boolean = False, Optional ByVal remove As Boolean = False) As Integer
            If GetCurrentFunction(GetLineCursorPosition(True), True) = "" Then Return -1
            Static index As Integer, lastcall As Long
            If lastcall = 0 OrElse (GetTickCount() - lastcall) > 500 Then
                Try
                    Dim tmp(1) As String, pos(1) As Integer
                    tmp(1) = Mid(SyntaxHandle.Lines.Current.Text.Replace(vbCrLf, "").Replace(vbTab, ""), 1, SyntaxHandle.Lines.Current.Text.Length)
                    pos(0) = GetLineCursorPosition()
                    tmp(1) = tmp(1).Remove(pos(0), tmp(1).Length - pos(0))
                    tmp(0) = StrReverse(Mid(SyntaxHandle.Lines.Current.Text.Replace(vbCrLf, "").Replace(vbTab, ""), 1, SyntaxHandle.Lines.Current.Text.Length))
                    Dim Ms As MatchCollection = Regex.Matches(tmp(0), "\(.*\)")
                    For Each M As Match In Ms
                        tmp(0) = tmp(0).Remove(M.Index, M.Length)
                    Next
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

#Region "Subs"

    Private Sub ParseCode()
        On Error Resume Next
        ACLists.UserDefinedPublics.Clear()
        Dim CommentedLine As Boolean, CommentedSection As Boolean
        For Each Line As ScintillaNet.Line In SyntaxHandle.Lines
            If Line.Text.Length = 0 OrElse Line.Text = "{" OrElse Line.Text = "}" OrElse Line.Text = ";" Then
                Continue For
            ElseIf Line.Text.StartsWith("//") Then
                CommentedLine = True
            ElseIf Line.Text = "/*" OrElse Line.Text = " /*" Then
                CommentedSection = True
                Continue For
            ElseIf Line.Text = "*/" OrElse Line.Text = " */" Then
                CommentedSection = False
                Continue For
            ElseIf Line.Text.IndexOf("/*") > -1 AndAlso Line.Text.IndexOf("*/") = -1 Then
                CommentedSection = True
            ElseIf Line.Text.IndexOf("*/") > -1 Then
                CommentedSection = False
            End If
            If CommentedLine Or CommentedSection Then
                CommentedLine = False
                Continue For
            End If
            If Line.Text.IndexOf("#include") > -1 Then
                Dim file As String, path As String
                If Line.Text.IndexOf("<") > -1 Then
                    file = Mid(Line.Text, Line.Text.IndexOf("<") + 2, Line.Text.IndexOf(">") - Line.Text.IndexOf("<") - 1)
                    path = My.Application.Info.DirectoryPath & "\Include\" & file & ".inc"
                ElseIf Line.Text.IndexOf("""") > -1 Then
                    If Line.Text.IndexOf("..") = -1 Then
                        file = Mid(Line.Text, Line.Text.IndexOf("""") + 2, Line.Text.LastIndexOf("""") - Line.Text.IndexOf("""") - 1)
                        path = My.Application.Info.DirectoryPath & "\Include\" & file & If(file.IndexOf(".inc") = -1, ".inc", "")
                    Else
                        file = Mid(Line.Text, Line.Text.IndexOf("..") + 3, Line.Text.LastIndexOf("""") - Line.Text.IndexOf("""") - 1).Replace("/", "\")
                        path = Directory.GetParent(My.Application.Info.DirectoryPath).FullName & file & If(file.IndexOf(".inc") = -1, ".inc", "")
                    End If
                Else
                    Continue For
                End If
                If IO.File.Exists(path) Then
                    Dim fLine As String, Reader As New StreamReader(path)
                    fLine = Reader.ReadLine()
                    Dim CommentedLine2 As Boolean, CommentedSection2 As Boolean
                    Do Until fLine Is Nothing
                        If fLine.Length = 0 OrElse fLine = "{" OrElse fLine = "}" OrElse fLine = ";" Then
                            fLine = Reader.ReadLine()
                            Continue Do
                        ElseIf fLine.StartsWith("//") Then
                            CommentedLine2 = True
                        ElseIf fLine = "/*" OrElse fLine = " /*" OrElse fLine = " /*" Then
                            CommentedSection2 = True
                            fLine = Reader.ReadLine()
                            Continue Do
                        ElseIf fLine = "*/" OrElse fLine = " */" OrElse fLine = " */" Then
                            CommentedSection2 = False
                            fLine = Reader.ReadLine()
                            Continue Do
                        ElseIf fLine.IndexOf("/*") > -1 AndAlso fLine.IndexOf("*/") = -1 Then
                            CommentedSection2 = True
                        ElseIf fLine.IndexOf("*/") > -1 Then
                            CommentedSection2 = False
                        End If
                        If CommentedLine2 Or CommentedSection2 Then
                            CommentedLine2 = False
                            fLine = Reader.ReadLine()
                            Continue Do
                        End If
                        If fLine.IndexOf("#include") > -1 Then
                            Dim file2 As String, path2 As String, cNode2 As New TreeNode()
                            If fLine.IndexOf("<") > -1 Then
                                file2 = Mid(fLine, fLine.IndexOf("<") + 2, fLine.IndexOf(">") - fLine.IndexOf("<") - 1)
                                path2 = My.Application.Info.DirectoryPath & "\Include\" & file2 & ".inc"
                            ElseIf fLine.IndexOf("""") > -1 Then
                                If fLine.IndexOf("..") = -1 Then
                                    file2 = Mid(fLine, fLine.IndexOf("""") + 2, fLine.LastIndexOf("""") - fLine.IndexOf("""") - 1)
                                    path2 = My.Application.Info.DirectoryPath & "\Include\" & file2 & If(file2.IndexOf(".inc") = -1, ".inc", "")
                                Else
                                    file2 = Mid(fLine, fLine.IndexOf("..") + 3, fLine.LastIndexOf("""") - fLine.IndexOf("""") - 1).Replace("/", "\")
                                    path2 = Directory.GetParent(My.Application.Info.DirectoryPath).FullName & file2 & If(file2.IndexOf(".inc") = -1, ".inc", "")
                                End If
                            Else
                                fLine = Reader.ReadLine()
                                Continue Do
                            End If
                            If IO.File.Exists(path2) Then
                                Dim Reader2 As New StreamReader(path2)
                                fLine = Reader2.ReadLine()
                                Dim CommentedLine3 As Boolean, CommentedSection3 As Boolean
                                Do Until fLine Is Nothing
                                    If fLine.Length = 0 OrElse fLine = "{" OrElse fLine = "}" OrElse fLine = ";" Then
                                        fLine = Reader2.ReadLine()
                                        Continue Do
                                    ElseIf fLine.StartsWith("//") Then
                                        CommentedLine3 = True
                                    ElseIf fLine = "/*" OrElse fLine = " /*" Then
                                        CommentedSection3 = True
                                        fLine = Reader2.ReadLine()
                                        Continue Do
                                    ElseIf fLine = "*/" OrElse fLine = " */" Then
                                        CommentedSection3 = False
                                        fLine = Reader2.ReadLine()
                                        Continue Do
                                    ElseIf fLine.IndexOf("/*") > -1 AndAlso fLine.IndexOf("*/") = -1 Then
                                        CommentedSection3 = True
                                    ElseIf fLine.IndexOf("*/") > -1 Then
                                        CommentedSection3 = False
                                    End If
                                    If CommentedLine3 Or CommentedSection3 Then
                                        CommentedLine3 = False
                                        fLine = Reader2.ReadLine()
                                        Continue Do
                                    End If
                                    If fLine.IndexOf("#define") > -1 AndAlso fLine.IndexOf("public") > -1 OrElse fLine.IndexOf("forward") > -1 Then
                                        Dim M As Match
                                        M = Regex.Match(fLine, "#define [^\s]+[\s]public[\s]?")
                                        If M.Success Then
                                            Dim tmp2 As New CustomUserPublics(Regex.Replace(M.Value.Remove(0, 8), "public[\s]?", ""), CustomUserPublics.Macro_Type.Macro_Type_Definition)
                                            If tmp2.Regex.Length > 0 AndAlso Not TrueContainsuPublic(ACLists.UserDefinedPublics, tmp2) Then ACLists.UserDefinedPublics.Add(tmp2)
                                        Else
                                            M = Regex.Match(fLine, "#define [^\s]+")
                                            If M.Success Then
                                                Dim tmp2 As New CustomUserPublics(Regex.Unescape(Regex.Replace(M.Value.Remove(0, 8), "%[0-9]", ".+")), CustomUserPublics.Macro_Type.Macro_Type_Function, Regex.Matches(M.Value, "%[0-9]").Count)
                                                If tmp2.Regex.Length > 0 AndAlso Not TrueContainsuPublic(ACLists.UserDefinedPublics, tmp2) Then ACLists.UserDefinedPublics.Add(tmp2)
                                            End If
                                        End If
                                    End If
                                    fLine = Reader2.ReadLine()
                                Loop
                            End If
                        ElseIf fLine.IndexOf("#tryinclude") > -1 Then
                            Dim file2 As String, path2 As String, cNode2 As New TreeNode()
                            If fLine.IndexOf("<") > -1 Then
                                file2 = Mid(fLine, fLine.IndexOf("<") + 2, fLine.IndexOf(">") - fLine.IndexOf("<") - 1)
                                path2 = My.Application.Info.DirectoryPath & "\Include\" & file2 & ".inc"
                            Else
                                If fLine.IndexOf("..") = -1 Then
                                    file2 = Mid(fLine, fLine.IndexOf("""") + 2, fLine.LastIndexOf("""") - fLine.IndexOf("""") - 1)
                                    path2 = My.Application.Info.DirectoryPath & "\Include\" & file2 & If(file2.IndexOf(".inc") = -1, ".inc", "")
                                Else
                                    file2 = Mid(fLine, fLine.IndexOf("..") + 3, fLine.LastIndexOf("""") - fLine.IndexOf("""") - 1).Replace("/", "\")
                                    path2 = Directory.GetParent(My.Application.Info.DirectoryPath).FullName & file2 & If(file2.IndexOf(".inc") = -1, ".inc", "")
                                End If
                            End If
                            If IO.File.Exists(path2) Then
                                Dim Reader2 As New StreamReader(path2)
                                fLine = Reader2.ReadLine()
                                Dim CommentedLine3 As Boolean, CommentedSection3 As Boolean
                                Do Until fLine Is Nothing
                                    If fLine.Length = 0 OrElse fLine = "{" OrElse fLine = "}" OrElse fLine = ";" Then
                                        fLine = Reader2.ReadLine()
                                        Continue Do
                                    ElseIf fLine.StartsWith("//") Then
                                        CommentedLine3 = True
                                    ElseIf fLine = "/*" OrElse fLine = " /*" Then
                                        CommentedSection3 = True
                                        fLine = Reader2.ReadLine()
                                        Continue Do
                                    ElseIf fLine = "*/" OrElse fLine = " */" Then
                                        CommentedSection3 = False
                                        fLine = Reader2.ReadLine()
                                        Continue Do
                                    ElseIf fLine.IndexOf("/*") > -1 AndAlso fLine.IndexOf("*/") = -1 Then
                                        CommentedSection3 = True
                                    ElseIf fLine.IndexOf("*/") > -1 Then
                                        CommentedSection3 = False
                                    End If
                                    If CommentedLine3 Or CommentedSection3 Then
                                        CommentedLine3 = False
                                        fLine = Reader2.ReadLine()
                                        Continue Do
                                    End If
                                    If fLine.IndexOf("#define") > -1 AndAlso fLine.IndexOf("public") > -1 OrElse fLine.IndexOf("forward") > -1 Then
                                        Dim M As Match
                                        M = Regex.Match(fLine, "#define [^\s]+[\s]public[\s]?")
                                        If M.Success Then
                                            Dim tmp2 As New CustomUserPublics(Regex.Replace(M.Value.Remove(0, 8), "public[\s]?", ""), CustomUserPublics.Macro_Type.Macro_Type_Definition)
                                            If tmp2.Regex.Length > 0 AndAlso Not TrueContainsuPublic(ACLists.UserDefinedPublics, tmp2) Then ACLists.UserDefinedPublics.Add(tmp2)
                                        Else
                                            M = Regex.Match(fLine, "#define [^\s]+")
                                            If M.Success Then
                                                Dim tmp2 As New CustomUserPublics(Regex.Unescape(Regex.Replace(M.Value.Remove(0, 8), "%[0-9]", ".+")), CustomUserPublics.Macro_Type.Macro_Type_Function, Regex.Matches(M.Value, "%[0-9]").Count)
                                                If tmp2.Regex.Length > 0 AndAlso Not TrueContainsuPublic(ACLists.UserDefinedPublics, tmp2) Then ACLists.UserDefinedPublics.Add(tmp2)
                                            End If
                                        End If
                                    End If
                                    fLine = Reader2.ReadLine()
                                Loop
                            End If
                        ElseIf fLine.IndexOf("#define") > -1 AndAlso fLine.IndexOf("public") > -1 OrElse fLine.IndexOf("forward") > -1 Then
                            Dim M As Match = Regex.Match(fLine, "#define [^\s]+[\s]public[\s]?")
                            If M.Success Then
                                Dim tmp2 As New CustomUserPublics(Regex.Replace(M.Value.Remove(0, 8), "public[\s]?", ""), CustomUserPublics.Macro_Type.Macro_Type_Definition)
                                If tmp2.Regex.Length > 0 AndAlso Not TrueContainsuPublic(ACLists.UserDefinedPublics, tmp2) Then ACLists.UserDefinedPublics.Add(tmp2)
                            Else
                                M = Regex.Match(fLine, "#define [^\s]+")
                                If M.Success Then
                                    Dim tmp2 As New CustomUserPublics(Regex.Unescape(Regex.Replace(M.Value.Remove(0, 8), "%[0-9]", ".+")), CustomUserPublics.Macro_Type.Macro_Type_Function, Regex.Matches(M.Value, "%[0-9]").Count)
                                    If tmp2.Regex.Length > 0 AndAlso Not TrueContainsuPublic(ACLists.UserDefinedPublics, tmp2) Then ACLists.UserDefinedPublics.Add(tmp2)
                                End If
                            End If
                        End If
                        fLine = Reader.ReadLine()
                    Loop
                End If
            ElseIf Line.Text.IndexOf("#tryinclude") > -1 Then
                Dim file As String, path As String
                If Line.Text.IndexOf("<") > -1 Then
                    file = Mid(Line.Text, Line.Text.IndexOf("<") + 2, Line.Text.IndexOf(">") - Line.Text.IndexOf("<") - 1)
                    path = My.Application.Info.DirectoryPath & "\Include\" & file & ".inc"
                Else
                    If Line.Text.IndexOf("..") = -1 Then
                        file = Mid(Line.Text, Line.Text.IndexOf("""") + 2, Line.Text.LastIndexOf("""") - Line.Text.IndexOf("""") - 1)
                        path = My.Application.Info.DirectoryPath & "\Include\" & file & If(file.IndexOf(".inc") = -1, ".inc", "")
                    Else
                        file = Mid(Line.Text, Line.Text.IndexOf("..") + 3, Line.Text.LastIndexOf("""") - Line.Text.IndexOf("""") - 1).Replace("/", "\")
                        path = Directory.GetParent(My.Application.Info.DirectoryPath).FullName & file & If(file.IndexOf(".inc") = -1, ".inc", "")
                    End If
                End If
                If IO.File.Exists(path) Then
                    Dim fLine As String, Reader As New StreamReader(path)
                    fLine = Reader.ReadLine()
                    Dim CommentedLine2 As Boolean, CommentedSection2 As Boolean
                    Do Until fLine Is Nothing
                        If fLine.Length = 0 OrElse fLine = "{" OrElse fLine = "}" OrElse fLine = ";" Then
                            fLine = Reader.ReadLine()
                            Continue Do
                        ElseIf fLine.StartsWith("//") Then
                            CommentedLine2 = True
                        ElseIf fLine = "/*" OrElse fLine = " /*" Then
                            CommentedSection2 = True
                            fLine = Reader.ReadLine()
                            Continue Do
                        ElseIf fLine = "*/" OrElse fLine = " */" Then
                            CommentedSection2 = False
                            fLine = Reader.ReadLine()
                            Continue Do
                        ElseIf fLine.IndexOf("/*") > -1 AndAlso fLine.IndexOf("*/") = -1 Then
                            CommentedSection2 = True
                        ElseIf fLine.IndexOf("*/") > -1 Then
                            CommentedSection2 = False
                        End If
                        If CommentedLine2 Or CommentedSection2 Then
                            CommentedLine2 = False
                            fLine = Reader.ReadLine()
                            Continue Do
                        End If
                        If fLine.IndexOf("#include") > -1 Then
                            Dim file2 As String, path2 As String, cNode2 As New TreeNode()
                            If fLine.IndexOf("<") > -1 Then
                                file2 = Mid(fLine, fLine.IndexOf("<") + 2, fLine.IndexOf(">") - fLine.IndexOf("<") - 1)
                                path2 = My.Application.Info.DirectoryPath & "\Include\" & file2 & ".inc"
                            ElseIf fLine.IndexOf("""") > -1 Then
                                If fLine.IndexOf("..") = -1 Then
                                    file2 = Mid(fLine, fLine.IndexOf("""") + 2, fLine.LastIndexOf("""") - fLine.IndexOf("""") - 1)
                                    path2 = My.Application.Info.DirectoryPath & "\Include\" & file2 & If(file2.IndexOf(".inc") = -1, ".inc", "")
                                Else
                                    file2 = Mid(fLine, fLine.IndexOf("..") + 3, fLine.LastIndexOf("""") - fLine.IndexOf("""") - 1).Replace("/", "\")
                                    path2 = Directory.GetParent(My.Application.Info.DirectoryPath).FullName & file2 & If(file2.IndexOf(".inc") = -1, ".inc", "")
                                End If
                            Else
                                fLine = Reader.ReadLine()
                                Continue Do
                            End If
                            If IO.File.Exists(path2) Then
                                Dim Reader2 As New StreamReader(path2)
                                fLine = Reader2.ReadLine()
                                Dim CommentedLine3 As Boolean, CommentedSection3 As Boolean
                                Do Until fLine Is Nothing
                                    If fLine.Length = 0 OrElse fLine = "{" OrElse fLine = "}" OrElse fLine = ";" Then
                                        fLine = Reader.ReadLine()
                                        Continue Do
                                    ElseIf fLine.StartsWith("//") Then
                                        CommentedLine3 = True
                                    ElseIf fLine = "/*" OrElse fLine = " /*" Then
                                        CommentedSection3 = True
                                        fLine = Reader.ReadLine()
                                        Continue Do
                                    ElseIf fLine = "*/" OrElse fLine = " */" Then
                                        CommentedSection3 = False
                                        fLine = Reader.ReadLine()
                                        Continue Do
                                    ElseIf fLine.IndexOf("/*") > -1 AndAlso fLine.IndexOf("*/") = -1 Then
                                        CommentedSection3 = True
                                    ElseIf fLine.IndexOf("*/") > -1 Then
                                        CommentedSection3 = False
                                    End If
                                    If CommentedLine3 Or CommentedSection3 Then
                                        CommentedLine3 = False
                                        fLine = Reader.ReadLine()
                                        Continue Do
                                    End If
                                    If fLine.IndexOf("#define") > -1 AndAlso fLine.IndexOf("public") > -1 OrElse fLine.IndexOf("forward") > -1 Then
                                        Dim M As Match
                                        M = Regex.Match(fLine, "#define [^\s]+[\s]public[\s]?")
                                        If M.Success Then
                                            Dim tmp2 As New CustomUserPublics(Regex.Replace(M.Value.Remove(0, 8), "public[\s]?", ""), CustomUserPublics.Macro_Type.Macro_Type_Definition)
                                            If tmp2.Regex.Length > 0 AndAlso Not TrueContainsuPublic(ACLists.UserDefinedPublics, tmp2) Then ACLists.UserDefinedPublics.Add(tmp2)
                                        Else
                                            M = Regex.Match(fLine, "#define [^\s]+")
                                            If M.Success Then
                                                Dim tmp2 As New CustomUserPublics(Regex.Unescape(Regex.Replace(M.Value.Remove(0, 8), "%[0-9]", ".+")), CustomUserPublics.Macro_Type.Macro_Type_Function, Regex.Matches(M.Value, "%[0-9]").Count)
                                                If tmp2.Regex.Length > 0 AndAlso Not TrueContainsuPublic(ACLists.UserDefinedPublics, tmp2) Then ACLists.UserDefinedPublics.Add(tmp2)
                                            End If
                                        End If
                                    End If
                                    fLine = Reader.ReadLine()
                                Loop
                            End If
                        ElseIf fLine.IndexOf("#tryinclude") > -1 Then
                            Dim file2 As String, path2 As String, cNode2 As New TreeNode()
                            If fLine.IndexOf("<") > -1 Then
                                file2 = Mid(fLine, fLine.IndexOf("<") + 2, fLine.IndexOf(">") - fLine.IndexOf("<") - 1)
                                path2 = My.Application.Info.DirectoryPath & "\Include\" & file2 & ".inc"
                            Else
                                If fLine.IndexOf("..") = -1 Then
                                    file2 = Mid(fLine, fLine.IndexOf("""") + 2, fLine.LastIndexOf("""") - fLine.IndexOf("""") - 1)
                                    path2 = My.Application.Info.DirectoryPath & "\Include\" & file2 & If(file2.IndexOf(".inc") = -1, ".inc", "")
                                Else
                                    file2 = Mid(fLine, fLine.IndexOf("..") + 3, fLine.LastIndexOf("""") - fLine.IndexOf("""") - 1).Replace("/", "\")
                                    path2 = Directory.GetParent(My.Application.Info.DirectoryPath).FullName & file2 & If(file2.IndexOf(".inc") = -1, ".inc", "")
                                End If
                            End If
                            If IO.File.Exists(path2) Then
                                Dim Reader2 As New StreamReader(path2)
                                fLine = Reader2.ReadLine()
                                Dim CommentedLine3 As Boolean, CommentedSection3 As Boolean
                                Do Until fLine Is Nothing
                                    If fLine.Length = 0 OrElse fLine = "{" OrElse fLine = "}" OrElse fLine = ";" Then
                                        fLine = Reader.ReadLine()
                                        Continue Do
                                    ElseIf fLine.StartsWith("//") Then
                                        CommentedLine3 = True
                                    ElseIf fLine = "/*" OrElse fLine = " /*" Then
                                        CommentedSection = True
                                        fLine = Reader.ReadLine()
                                        Continue Do
                                    ElseIf fLine = "*/" OrElse fLine = " */" Then
                                        CommentedSection3 = False
                                        fLine = Reader.ReadLine()
                                        Continue Do
                                    ElseIf fLine.IndexOf("/*") > -1 AndAlso fLine.IndexOf("*/") = -1 Then
                                        CommentedSection3 = True
                                    ElseIf fLine.IndexOf("*/") > -1 Then
                                        CommentedSection3 = False
                                    End If
                                    If CommentedLine3 Or CommentedSection3 Then
                                        CommentedLine3 = False
                                        fLine = Reader.ReadLine()
                                        Continue Do
                                    End If
                                    If fLine.IndexOf("#define") > -1 AndAlso fLine.IndexOf("public") > -1 OrElse fLine.IndexOf("forward") > -1 Then
                                        Dim M As Match
                                        M = Regex.Match(fLine, "#define [^\s]+[\s]public[\s]?")
                                        If M.Success Then
                                            Dim tmp2 As New CustomUserPublics(Regex.Replace(M.Value.Remove(0, 8), "public[\s]?", ""), CustomUserPublics.Macro_Type.Macro_Type_Definition)
                                            If tmp2.Regex.Length > 0 AndAlso Not TrueContainsuPublic(ACLists.UserDefinedPublics, tmp2) Then ACLists.UserDefinedPublics.Add(tmp2)
                                        Else
                                            M = Regex.Match(fLine, "#define [^\s]+")
                                            If M.Success Then
                                                Dim tmp2 As New CustomUserPublics(Regex.Unescape(Regex.Replace(M.Value.Remove(0, 8), "%[0-9]", ".+")), CustomUserPublics.Macro_Type.Macro_Type_Function, Regex.Matches(M.Value, "%[0-9]").Count)
                                                If tmp2.Regex.Length > 0 AndAlso Not TrueContainsuPublic(ACLists.UserDefinedPublics, tmp2) Then ACLists.UserDefinedPublics.Add(tmp2)
                                            End If
                                        End If
                                    End If
                                    fLine = Reader.ReadLine()
                                Loop
                            End If
                        ElseIf fLine.IndexOf("#define") > -1 AndAlso fLine.IndexOf("public") > -1 OrElse fLine.IndexOf("forward") > -1 Then
                            Dim M As Match
                            M = Regex.Match(fLine, "#define [^\s]+[\s]public[\s]?")
                            If M.Success Then
                                Dim tmp2 As New CustomUserPublics(Regex.Replace(M.Value.Remove(0, 8), "public[\s]?", ""), CustomUserPublics.Macro_Type.Macro_Type_Definition)
                                If tmp2.Regex.Length > 0 AndAlso Not TrueContainsuPublic(ACLists.UserDefinedPublics, tmp2) Then ACLists.UserDefinedPublics.Add(tmp2)
                            Else
                                M = Regex.Match(fLine, "#define [^\s]+")
                                If M.Success Then
                                    Dim tmp2 As New CustomUserPublics(Regex.Unescape(Regex.Replace(M.Value.Remove(0, 8), "%[0-9]", ".+")), CustomUserPublics.Macro_Type.Macro_Type_Function, Regex.Matches(M.Value, "%[0-9]").Count)
                                    If tmp2.Regex.Length > 0 AndAlso Not TrueContainsuPublic(ACLists.UserDefinedPublics, tmp2) Then ACLists.UserDefinedPublics.Add(tmp2)
                                End If
                            End If
                        End If
                        fLine = Reader.ReadLine()
                    Loop
                End If
            ElseIf Line.Text.IndexOf("#define") > -1 AndAlso Line.Text.IndexOf("public") > -1 OrElse Line.Text.IndexOf("forward") > -1 Then
                Dim M As Match
                M = Regex.Match(Line.Text, "#define [^\s]+[\s]public[\s]?")
                If M.Success Then
                    Dim tmp2 As New CustomUserPublics(Regex.Replace(M.Value.Remove(0, 8), "public[\s]?", ""), CustomUserPublics.Macro_Type.Macro_Type_Definition)
                    If tmp2.Regex.Length > 0 AndAlso Not TrueContainsuPublic(ACLists.UserDefinedPublics, tmp2) Then ACLists.UserDefinedPublics.Add(tmp2)
                Else
                    M = Regex.Match(Line.Text, "#define [^\s]+")
                    If M.Success Then
                        Dim tmp2 As New CustomUserPublics(Regex.Unescape(Regex.Replace(M.Value.Remove(0, 8), "%[0-9]", ".+")), CustomUserPublics.Macro_Type.Macro_Type_Function, Regex.Matches(M.Value, "%[0-9]").Count)
                        If tmp2.Regex.Length > 0 AndAlso Not TrueContainsuPublic(ACLists.UserDefinedPublics, tmp2) Then ACLists.UserDefinedPublics.Add(tmp2)
                    End If
                End If
            End If
        Next
    End Sub

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

    Private Sub Colorize()
        With Settings
            Dim inverted As Color = Color.FromArgb(255 - .BackColor.A, 255 - .BackColor.R, 255 - .BackColor.G, 255 - .BackColor.B)
            If .All Then
                SyntaxHandle.BackColor = .BackColor
                SyntaxHandle.Caret.Color = inverted
                For i = 0 To 19
                    Select Case i
                        Case 1 To 7, 9, 10, 12, 15, 17 To 19
                        Case Else
                            SyntaxHandle.Styles(i).ForeColor = inverted
                    End Select
                    SyntaxHandle.Styles(i).BackColor = .BackColor
                Next
                With .H_Numbers
                    SyntaxHandle.Styles("NUMBER").ForeColor = .ForeColor
                    SyntaxHandle.Styles("NUMBER").Bold = .Bold
                    SyntaxHandle.Styles("NUMBER").Italic = .Italic
                End With
                With .H_String
                    SyntaxHandle.Styles("STRING").ForeColor = .ForeColor
                    SyntaxHandle.Styles("STRING").Bold = .Bold
                    SyntaxHandle.Styles("STRING").Italic = .Italic
                End With
                With .H_String2
                    SyntaxHandle.Styles("STRINGEOL").ForeColor = .ForeColor
                    SyntaxHandle.Styles("STRINGEOL").Bold = .Bold
                    SyntaxHandle.Styles("STRINGEOL").Italic = .Italic
                End With
                With .H_Operator
                    SyntaxHandle.Styles("OPERATOR").ForeColor = .ForeColor
                    SyntaxHandle.Styles("OPERATOR").Bold = .Bold
                    SyntaxHandle.Styles("OPERATOR").Italic = .Italic
                End With
                With .H_Chars
                    SyntaxHandle.Styles("CHARACTER").ForeColor = .ForeColor
                    SyntaxHandle.Styles("CHARACTER").Bold = .Bold
                    SyntaxHandle.Styles("CHARACTER").Italic = .Italic
                End With
                With .H_Class
                    SyntaxHandle.Styles("GLOBALCLASS").ForeColor = .ForeColor
                    SyntaxHandle.Styles("GLOBALCLASS").Font = Font
                    SyntaxHandle.Styles("GLOBALCLASS").Bold = .Bold
                    SyntaxHandle.Styles("GLOBALCLASS").Italic = .Italic
                End With
                With .H_Preproc
                    SyntaxHandle.Styles("PREPROCESSOR").ForeColor = .ForeColor
                    SyntaxHandle.Styles("PREPROCESSOR").Font = Font
                    SyntaxHandle.Styles("PREPROCESSOR").Bold = .Bold
                    SyntaxHandle.Styles("PREPROCESSOR").Italic = .Italic
                End With
                With .H_Comment
                    SyntaxHandle.Styles("COMMENT").ForeColor = .ForeColor
                    SyntaxHandle.Styles("COMMENT").Bold = .Bold
                    SyntaxHandle.Styles("COMMENT").Italic = .Italic
                    SyntaxHandle.Styles("COMMENTLINE").ForeColor = .ForeColor
                    SyntaxHandle.Styles("COMMENTLINE").Bold = .Bold
                    SyntaxHandle.Styles("COMMENTLINE").Italic = .Italic
                    SyntaxHandle.Styles("COMMENTDOC").ForeColor = .ForeColor
                    SyntaxHandle.Styles("COMMENTDOC").Bold = .Bold
                    SyntaxHandle.Styles("COMMENTDOC").Italic = .Italic
                    SyntaxHandle.Styles("COMMENTLINEDOC").ForeColor = .ForeColor
                    SyntaxHandle.Styles("COMMENTLINEDOC").Bold = .Bold
                    SyntaxHandle.Styles("COMMENTLINEDOC").Italic = .Italic
                    SyntaxHandle.Styles("COMMENTDOCKEYWORD").ForeColor = .ForeColor
                    SyntaxHandle.Styles("COMMENTDOCKEYWORD").Bold = .Bold
                    SyntaxHandle.Styles("COMMENTDOCKEYWORD").Italic = .Italic
                    SyntaxHandle.Styles("COMMENTDOCKEYWORDERROR").ForeColor = .ForeColor
                    SyntaxHandle.Styles("COMMENTDOCKEYWORDERROR").Bold = .Bold
                    SyntaxHandle.Styles("COMMENTDOCKEYWORDERROR").Italic = .Italic
                End With
                SyntaxHandle.Lexing.Colorize()
            Else
                Font = .cFont
                SyntaxHandle.Encoding = .Enc
                SyntaxHandle.BackColor = .BackColor
                SyntaxHandle.Caret.Color = inverted
                For i = 0 To 19
                    Select Case i
                        Case 1 To 7, 9, 10, 12, 15, 17 To 19
                        Case Else
                            SyntaxHandle.Styles(i).ForeColor = inverted
                    End Select
                    SyntaxHandle.Styles(i).BackColor = .BackColor
                Next
                With .H_Numbers
                    SyntaxHandle.Styles("NUMBER").BackColor = .BackColor
                    SyntaxHandle.Styles("NUMBER").ForeColor = .ForeColor
                    SyntaxHandle.Styles("NUMBER").Bold = .Bold
                    SyntaxHandle.Styles("NUMBER").Italic = .Italic
                End With
                With .H_String
                    SyntaxHandle.Styles("STRING").BackColor = .BackColor
                    SyntaxHandle.Styles("STRING").ForeColor = .ForeColor
                    SyntaxHandle.Styles("STRING").Bold = .Bold
                    SyntaxHandle.Styles("STRING").Italic = .Italic
                End With
                With .H_String2
                    SyntaxHandle.Styles("STRINGEOL").BackColor = .BackColor
                    SyntaxHandle.Styles("STRINGEOL").ForeColor = .ForeColor
                    SyntaxHandle.Styles("STRINGEOL").Bold = .Bold
                    SyntaxHandle.Styles("STRINGEOL").Italic = .Italic
                End With
                With .H_Operator
                    SyntaxHandle.Styles("OPERATOR").BackColor = .BackColor
                    SyntaxHandle.Styles("OPERATOR").ForeColor = .ForeColor
                    SyntaxHandle.Styles("OPERATOR").Bold = .Bold
                    SyntaxHandle.Styles("OPERATOR").Italic = .Italic
                End With
                With .H_Chars
                    SyntaxHandle.Styles("CHARACTER").BackColor = .BackColor
                    SyntaxHandle.Styles("CHARACTER").ForeColor = .ForeColor
                    SyntaxHandle.Styles("CHARACTER").Bold = .Bold
                    SyntaxHandle.Styles("CHARACTER").Italic = .Italic
                End With
                With .H_Class
                    SyntaxHandle.Styles("GLOBALCLASS").BackColor = .BackColor
                    SyntaxHandle.Styles("GLOBALCLASS").ForeColor = .ForeColor
                    SyntaxHandle.Styles("GLOBALCLASS").Font = Font
                    SyntaxHandle.Styles("GLOBALCLASS").Bold = .Bold
                    SyntaxHandle.Styles("GLOBALCLASS").Italic = .Italic
                End With
                With .H_Preproc
                    SyntaxHandle.Styles("PREPROCESSOR").BackColor = .BackColor
                    SyntaxHandle.Styles("PREPROCESSOR").ForeColor = .ForeColor
                    SyntaxHandle.Styles("PREPROCESSOR").Font = Font
                    SyntaxHandle.Styles("PREPROCESSOR").Bold = .Bold
                    SyntaxHandle.Styles("PREPROCESSOR").Italic = .Italic
                End With
                With .H_Comment
                    SyntaxHandle.Styles("COMMENT").BackColor = .BackColor
                    SyntaxHandle.Styles("COMMENT").ForeColor = .ForeColor
                    SyntaxHandle.Styles("COMMENT").Bold = .Bold
                    SyntaxHandle.Styles("COMMENT").Italic = .Italic
                    SyntaxHandle.Styles("COMMENTLINE").BackColor = .BackColor
                    SyntaxHandle.Styles("COMMENTLINE").ForeColor = .ForeColor
                    SyntaxHandle.Styles("COMMENTLINE").Bold = .Bold
                    SyntaxHandle.Styles("COMMENTLINE").Italic = .Italic
                    SyntaxHandle.Styles("COMMENTDOC").BackColor = .BackColor
                    SyntaxHandle.Styles("COMMENTDOC").ForeColor = .ForeColor
                    SyntaxHandle.Styles("COMMENTDOC").Bold = .Bold
                    SyntaxHandle.Styles("COMMENTDOC").Italic = .Italic
                    SyntaxHandle.Styles("COMMENTLINEDOC").BackColor = .BackColor
                    SyntaxHandle.Styles("COMMENTLINEDOC").ForeColor = .ForeColor
                    SyntaxHandle.Styles("COMMENTLINEDOC").Bold = .Bold
                    SyntaxHandle.Styles("COMMENTLINEDOC").Italic = .Italic
                    SyntaxHandle.Styles("COMMENTDOCKEYWORD").BackColor = .BackColor
                    SyntaxHandle.Styles("COMMENTDOCKEYWORD").ForeColor = .ForeColor
                    SyntaxHandle.Styles("COMMENTDOCKEYWORD").Bold = .Bold
                    SyntaxHandle.Styles("COMMENTDOCKEYWORD").Italic = .Italic
                    SyntaxHandle.Styles("COMMENTDOCKEYWORDERROR").BackColor = .BackColor
                    SyntaxHandle.Styles("COMMENTDOCKEYWORDERROR").ForeColor = .ForeColor
                    SyntaxHandle.Styles("COMMENTDOCKEYWORDERROR").Bold = .Bold
                    SyntaxHandle.Styles("COMMENTDOCKEYWORDERROR").Italic = .Italic
                End With
                SyntaxHandle.Lexing.Colorize()
            End If
        End With
    End Sub

    Private Sub UpdateFileData()
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
        Errors.Clear()
        ParseCode()
        Dim mLine As String, linecounter As Integer, MainReader As New StreamReader(_Path)
        mLine = MainReader.ReadLine()
        While Not mLine Is Nothing
            linecounter += 1
            If mLine.Length = 0 OrElse mLine = "{" OrElse mLine = "}" OrElse mLine = ";" Then
                mLine = MainReader.ReadLine()
                Continue While
            ElseIf mLine.StartsWith("//") Then
                CommentedLine = True
            ElseIf mLine = "/*" Then
                CommentedSection = True
                mLine = MainReader.ReadLine()
                Continue While
            ElseIf mLine = "*/" Then
                CommentedSection = False
                mLine = MainReader.ReadLine()
                Continue While
            ElseIf mLine.IndexOf("/*") > -1 AndAlso mLine.IndexOf("*/") = -1 Then
                CommentedSection = True
            ElseIf mLine.IndexOf("*/") > -1 Then
                CommentedSection = False
            End If
            If CommentedLine Or CommentedSection Then
                CommentedLine = False
                mLine = MainReader.ReadLine()
                Continue While
            End If
            Dim spos As Integer = mLine.IndexOf("native")
            If spos = -1 Then
                spos = mLine.IndexOf("stock")
                If spos = -1 Then spos = mLine.IndexOf("public")
            End If
            If mLine.IndexOf("#include") > -1 Then
                Dim file As String, path As String
                If mLine.IndexOf("<") > -1 Then
                    file = Mid(mLine, mLine.IndexOf("<") + 2, mLine.IndexOf(">") - mLine.IndexOf("<") - 1)
                    path = My.Application.Info.DirectoryPath & "\Include\" & file & ".inc"
                ElseIf mLine.IndexOf("""") > -1 Then
                    If mLine.IndexOf("..") = -1 Then
                        file = Mid(mLine, mLine.IndexOf("""") + 2, mLine.LastIndexOf("""") - mLine.IndexOf("""") - 1)
                        path = My.Application.Info.DirectoryPath & "\Include\" & file & If(file.IndexOf(".inc") = -1, ".inc", "")
                    Else
                        file = Mid(mLine, mLine.IndexOf("..") + 3, mLine.LastIndexOf("""") - mLine.IndexOf("""") - 1).Replace("/", "\")
                        path = Directory.GetParent(My.Application.Info.DirectoryPath).FullName & file & If(file.IndexOf(".inc") = -1, ".inc", "")
                    End If
                Else
                    mLine = MainReader.ReadLine()
                    Continue While
                End If
                If IO.File.Exists(path) Then
                    Dim fLine As String, Reader As New StreamReader(path)
                    fLine = Reader.ReadLine()
                    Dim CommentedLine2 As Boolean, CommentedSection2 As Boolean
                    Do Until fLine Is Nothing
                        If fLine.Length = 0 OrElse fLine = "{" OrElse fLine = "}" OrElse fLine = ";" Then
                            fLine = Reader.ReadLine()
                            Continue Do
                        ElseIf fLine.StartsWith("//") Then
                            CommentedLine2 = True
                        ElseIf fLine = "/*" OrElse fLine = " /*" Then
                            CommentedSection2 = True
                            fLine = Reader.ReadLine()
                            Continue Do
                        ElseIf fLine = "*/" OrElse fLine = " */" Then
                            CommentedSection2 = False
                            fLine = Reader.ReadLine()
                            Continue Do
                        ElseIf fLine.IndexOf("/*") > -1 AndAlso fLine.IndexOf("*/") = -1 Then
                            CommentedSection2 = True
                        ElseIf fLine.IndexOf("*/") > -1 Then
                            CommentedSection2 = False
                        End If
                        If CommentedLine2 Or CommentedSection2 Then
                            CommentedLine2 = False
                            fLine = Reader.ReadLine()
                            Continue Do
                        End If
                        spos = fLine.IndexOf("native")
                        If spos = -1 Then
                            spos = fLine.IndexOf("stock")
                            If spos = -1 Then spos = fLine.IndexOf("public")
                        End If
                        If fLine.IndexOf("#include") > -1 Then
                            Dim file2 As String, path2 As String, cNode2 As New TreeNode()
                            If fLine.IndexOf("<") > -1 Then
                                file2 = Mid(fLine, fLine.IndexOf("<") + 2, fLine.IndexOf(">") - fLine.IndexOf("<") - 1)
                                path2 = My.Application.Info.DirectoryPath & "\Include\" & file2 & ".inc"
                            ElseIf fLine.IndexOf("""") > -1 Then
                                If fLine.IndexOf("..") = -1 Then
                                    file2 = Mid(fLine, fLine.IndexOf("""") + 2, fLine.LastIndexOf("""") - fLine.IndexOf("""") - 1)
                                    path2 = My.Application.Info.DirectoryPath & "\Include\" & file2 & If(file2.IndexOf(".inc") = -1, ".inc", "")
                                Else
                                    file2 = Mid(fLine, fLine.IndexOf("..") + 3, fLine.LastIndexOf("""") - fLine.IndexOf("""") - 1).Replace("/", "\")
                                    path2 = Directory.GetParent(My.Application.Info.DirectoryPath).FullName & file2 & If(file2.IndexOf(".inc") = -1, ".inc", "")
                                End If
                            Else
                                fLine = Reader.ReadLine()
                                Continue Do
                            End If
                            Dim count As Integer
                            If IO.File.Exists(path2) Then
                                Dim Reader2 As New StreamReader(path2)
                                fLine = Reader2.ReadLine()
                                Dim CommentedLine3 As Boolean, CommentedSection3 As Boolean
                                Do Until fLine Is Nothing
                                    If fLine.Length = 0 OrElse fLine = "{" OrElse fLine = "}" OrElse fLine = ";" Then
                                        count += 1
                                        fLine = Reader2.ReadLine()
                                        Continue Do
                                    ElseIf fLine.StartsWith("//") Then
                                        CommentedLine3 = True
                                    ElseIf fLine = "/*" OrElse fLine = " /*" Then
                                        CommentedSection3 = True
                                        count += 1
                                        fLine = Reader2.ReadLine()
                                        Continue Do
                                    ElseIf fLine = "*/" OrElse fLine = " */" Then
                                        CommentedSection3 = False
                                        count += 1
                                        fLine = Reader2.ReadLine()
                                        Continue Do
                                    ElseIf fLine.IndexOf("/*") > -1 AndAlso fLine.IndexOf("*/") = -1 Then
                                        CommentedSection3 = True
                                    ElseIf fLine.IndexOf("*/") > -1 Then
                                        CommentedSection3 = False
                                    End If
                                    If CommentedLine3 Or CommentedSection3 Then
                                        CommentedLine3 = False
                                        count += 1
                                        fLine = Reader2.ReadLine()
                                        Continue Do
                                    End If
                                    spos = fLine.IndexOf("native")
                                    If spos = -1 Then
                                        spos = fLine.IndexOf("stock")
                                        If spos = -1 Then
                                            spos = fLine.IndexOf("public")
                                        End If
                                    End If
                                    If spos > -1 AndAlso (fLine.EndsWith(";") AndAlso fLine.StartsWith("native ")) AndAlso fLine.IndexOf("(") > -1 AndAlso fLine.IndexOf(")") > -1 AndAlso fLine.IndexOf("operator") = -1 Then
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
                                        Dim value As String, Ms As MatchCollection = Regex.Matches(fLine, "Menu:[^,;\s]+")
                                        For Each M As Match In Ms
                                            value = Regex.Replace(M.Value, "(Menu:|[,;\s])", "")
                                            If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Menus.Contains(value) Then ACLists.Menus.Add(value)
                                        Next
                                    ElseIf fLine.IndexOf("Text:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                                        Dim value As String, Ms As MatchCollection = Regex.Matches(fLine, "Text:[^,;\s]+")
                                        For Each M As Match In Ms
                                            value = Regex.Replace(M.Value, "(Text:|[,;\s])", "")
                                            If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Texts.Contains(value) Then ACLists.Texts.Add(value)
                                        Next
                                    ElseIf fLine.IndexOf("Text3D:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                                        Dim value As String, Ms As MatchCollection = Regex.Matches(fLine, "Text3D:[^,;\s]+")
                                        For Each M As Match In Ms
                                            value = Regex.Replace(M.Value, "(Text3D:|[,;\s])", "")
                                            If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Texts2.Contains(value) Then ACLists.Texts2.Add(value)
                                        Next
                                    ElseIf fLine.IndexOf("Float:") > -1 AndAlso fLine.IndexOf("cellmin") = -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                                        Dim value As String, Ms As MatchCollection = Regex.Matches(fLine, "Float:[^,;\s]+")
                                        For Each M As Match In Ms
                                            value = Regex.Replace(M.Value, "(Float:|[,;\s])", "")
                                            If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Floats.Contains(value) Then ACLists.Floats.Add(value)
                                        Next
                                    ElseIf fLine.IndexOf("DB:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                                        Dim value As String, Ms As MatchCollection = Regex.Matches(fLine, "DB:[^,;\s]+")
                                        For Each M As Match In Ms
                                            value = Regex.Replace(M.Value, "(DB:|[,;\s])", "")
                                            If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Dbs.Contains(value) Then ACLists.Dbs.Add(value)
                                        Next
                                    ElseIf fLine.IndexOf("DBResult:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                                        Dim value As String, Ms As MatchCollection = Regex.Matches(fLine, "DBResult:[^,;\s]+")
                                        For Each M As Match In Ms
                                            value = Regex.Replace(M.Value, "(DBResult:|[,;\s])", "")
                                            If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.DbRes.Contains(value) Then ACLists.DbRes.Add(value)
                                        Next
                                    ElseIf fLine.IndexOf("File:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                                        Dim value As String, Ms As MatchCollection = Regex.Matches(fLine, "File:[^,;\s]+")
                                        For Each M As Match In Ms
                                            value = Regex.Replace(M.Value, "(File:|[,;\s])", "")
                                            If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Files.Contains(value) Then ACLists.Files.Add(value)
                                        Next
                                    Else
                                        Dim tDef As String, name As String, params As String(), func As PawnFunction
                                        For Each def As CustomUserPublics In ACLists.UserDefinedPublics
                                            Dim M As Match = Regex.Match(fLine, def.Regex)
                                            If M.Success Then
                                                tDef = def.Regex
                                                tmp = M.Value.Remove(0, tDef.IndexOf(".+"))
                                                tDef = tDef.Remove(0, tDef.IndexOf(".+"))
                                                name = Regex.Match(tmp, Regex.Escape(Mid(tDef, 1, tDef.IndexOf(".+", 1))).Replace("\.", ".").Replace("\+", "+")).Value
                                                tDef = tDef.Remove(0, 2)
                                                tmp = tmp.Replace(name, "")
                                                name = name.Remove(name.Length - 1, 1)
                                                params = Regex.Split(Mid(tmp, 1, tmp.Length - 2), "[\s]?,[\s]?")
                                                func = New PawnFunction(name, _Name.Replace(".inc", ":"), linecounter, params)
                                                If Not TrueContainsFunction(ACLists.Functions, func) Then ACLists.Functions.Add(func)
                                            End If
                                        Next
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
                            Dim file2 As String, path2 As String
                            If fLine.IndexOf("<") > -1 Then
                                file2 = Mid(fLine, fLine.IndexOf("<") + 2, fLine.IndexOf(">") - fLine.IndexOf("<") - 1)
                                path2 = My.Application.Info.DirectoryPath & "\Include\" & file2 & ".inc"
                            Else
                                If fLine.IndexOf("..") = -1 Then
                                    file2 = Mid(fLine, fLine.IndexOf("""") + 2, fLine.LastIndexOf("""") - fLine.IndexOf("""") - 1)
                                    path2 = My.Application.Info.DirectoryPath & "\Include\" & file2 & If(file2.IndexOf(".inc") = -1, ".inc", "")
                                Else
                                    file2 = Mid(fLine, fLine.IndexOf("..") + 3, fLine.LastIndexOf("""") - fLine.IndexOf("""") - 1).Replace("/", "\")
                                    path2 = Directory.GetParent(My.Application.Info.DirectoryPath).FullName & file2 & If(file2.IndexOf(".inc") = -1, ".inc", "")
                                End If
                            End If
                            Dim count As Integer
                            If IO.File.Exists(path2) Then
                                Dim Reader2 As New StreamReader(path2)
                                fLine = Reader2.ReadLine()
                                Dim CommentedLine3 As Boolean, CommentedSection3 As Boolean
                                Do Until fLine Is Nothing
                                    If fLine.Length = 0 OrElse fLine = "{" OrElse fLine = "}" OrElse fLine = ";" Then
                                        count += 1
                                        fLine = Reader2.ReadLine()
                                        Continue Do
                                    ElseIf fLine.StartsWith("//") Then
                                        CommentedLine3 = True
                                    ElseIf fLine = "/*" OrElse fLine = " /*" Then
                                        CommentedSection3 = True
                                        count += 1
                                        fLine = Reader2.ReadLine()
                                        Continue Do
                                    ElseIf fLine = "*/" OrElse fLine = " */" Then
                                        CommentedSection3 = False
                                        count += 1
                                        fLine = Reader2.ReadLine()
                                        Continue Do
                                    ElseIf fLine.IndexOf("/*") > -1 AndAlso fLine.IndexOf("*/") = -1 Then
                                        CommentedSection3 = True
                                    ElseIf fLine.IndexOf("*/") > -1 Then
                                        CommentedSection3 = False
                                    End If
                                    If CommentedLine3 Or CommentedSection3 Then
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
                                    If spos > -1 AndAlso (fLine.EndsWith(";") AndAlso fLine.StartsWith("native ")) AndAlso fLine.IndexOf("(") > -1 AndAlso fLine.IndexOf(")") > -1 AndAlso fLine.IndexOf("operator") = -1 Then
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
                                        Dim value As String, Ms As MatchCollection = Regex.Matches(fLine, "Menu:[^,;\s]+")
                                        For Each M As Match In Ms
                                            value = Regex.Replace(M.Value, "(Menu:|[,;\s])", "")
                                            If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Menus.Contains(value) Then ACLists.Menus.Add(value)
                                        Next
                                    ElseIf fLine.IndexOf("Text:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                                        Dim value As String, Ms As MatchCollection = Regex.Matches(fLine, "Text:[^,;\s]+")
                                        For Each M As Match In Ms
                                            value = Regex.Replace(M.Value, "(Text:|[,;\s])", "")
                                            If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Texts.Contains(value) Then ACLists.Texts.Add(value)
                                        Next
                                    ElseIf fLine.IndexOf("Text3D:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                                        Dim value As String, Ms As MatchCollection = Regex.Matches(fLine, "Text3D:[^,;\s]+")
                                        For Each M As Match In Ms
                                            value = Regex.Replace(M.Value, "(Text3D:|[,;\s])", "")
                                            If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Texts2.Contains(value) Then ACLists.Texts2.Add(value)
                                        Next
                                    ElseIf fLine.IndexOf("Float:") > -1 AndAlso fLine.IndexOf("cellmin") = -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                                        Dim value As String, Ms As MatchCollection = Regex.Matches(fLine, "Float:[^,;\s]+")
                                        For Each M As Match In Ms
                                            value = Regex.Replace(M.Value, "(Float:|[,;\s])", "")
                                            If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Floats.Contains(value) Then ACLists.Floats.Add(value)
                                        Next
                                    ElseIf fLine.IndexOf("DB:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                                        Dim value As String, Ms As MatchCollection = Regex.Matches(fLine, "DB:[^,;\s]+")
                                        For Each M As Match In Ms
                                            value = Regex.Replace(M.Value, "(DB:|[,;\s])", "")
                                            If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Dbs.Contains(value) Then ACLists.Dbs.Add(value)
                                        Next
                                    ElseIf fLine.IndexOf("DBResult:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                                        Dim value As String, Ms As MatchCollection = Regex.Matches(fLine, "DBResult:[^,;\s]+")
                                        For Each M As Match In Ms
                                            value = Regex.Replace(M.Value, "(DBResult:|[,;\s])", "")
                                            If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.DbRes.Contains(value) Then ACLists.DbRes.Add(value)
                                        Next
                                    ElseIf fLine.IndexOf("File:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                                        Dim value As String, Ms As MatchCollection = Regex.Matches(fLine, "File:[^,;\s]+")
                                        For Each M As Match In Ms
                                            value = Regex.Replace(M.Value, "(File:|[,;\s])", "")
                                            If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Files.Contains(value) Then ACLists.Files.Add(value)
                                        Next
                                    Else
                                        Dim tDef As String, name As String, params As String(), func As PawnFunction
                                        For Each def As CustomUserPublics In ACLists.UserDefinedPublics
                                            Dim M As Match = Regex.Match(fLine, def.Regex)
                                            If M.Success Then
                                                tDef = def.Regex
                                                tmp = M.Value.Remove(0, tDef.IndexOf(".+"))
                                                tDef = tDef.Remove(0, tDef.IndexOf(".+"))
                                                name = Regex.Match(tmp, Regex.Escape(Mid(tDef, 1, tDef.IndexOf(".+", 1))).Replace("\.", ".").Replace("\+", "+")).Value
                                                tDef = tDef.Remove(0, 2)
                                                tmp = tmp.Replace(name, "")
                                                name = name.Remove(name.Length - 1, 1)
                                                params = Regex.Split(Mid(tmp, 1, tmp.Length - 2), "[\s]?,[\s]?")
                                                func = New PawnFunction(name, _Name.Replace(".inc", ":"), linecounter, params)
                                                If Not TrueContainsFunction(ACLists.Functions, func) Then ACLists.Functions.Add(func)
                                            End If
                                        Next
                                    End If
                                    count += 1
                                    fLine = Reader2.ReadLine()
                                Loop
                                Reader2.Close()
                            Else
                                Errors.Clear()
                                Errors.Add(New ListViewItem(New String() {"", "100", Name, count, "cannot read from file: """ & file2 & """"}, 1))
                            End If
                        ElseIf spos > -1 AndAlso (fLine.EndsWith(";") AndAlso fLine.StartsWith("native ")) AndAlso fLine.IndexOf("(") > -1 AndAlso fLine.IndexOf(")") > -1 AndAlso fLine.IndexOf("operator") = -1 Then
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
                            Dim value As String, Ms As MatchCollection = Regex.Matches(fLine, "Menu:[^,;\s]+")
                            For Each M As Match In Ms
                                value = Regex.Replace(M.Value, "(Menu:|[,;\s])", "")
                                If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Menus.Contains(value) Then ACLists.Menus.Add(value)
                            Next
                        ElseIf fLine.IndexOf("Text:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                            Dim value As String, Ms As MatchCollection = Regex.Matches(fLine, "Text:[^,;\s]+")
                            For Each M As Match In Ms
                                value = Regex.Replace(M.Value, "(Text:|[,;\s])", "")
                                If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Texts.Contains(value) Then ACLists.Texts.Add(value)
                            Next
                        ElseIf fLine.IndexOf("Text3D:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                            Dim value As String, Ms As MatchCollection = Regex.Matches(fLine, "Text3D:[^,;\s]+")
                            For Each M As Match In Ms
                                value = Regex.Replace(M.Value, "(Text3D:|[,;\s])", "")
                                If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Texts2.Contains(value) Then ACLists.Texts2.Add(value)
                            Next
                        ElseIf fLine.IndexOf("Float:") > -1 AndAlso fLine.IndexOf("cellmin") = -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                            Dim value As String, Ms As MatchCollection = Regex.Matches(fLine, "Float:[^,;\s]+")
                            For Each M As Match In Ms
                                value = Regex.Replace(M.Value, "(Float:|[,;\s])", "")
                                If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Floats.Contains(value) Then ACLists.Floats.Add(value)
                            Next
                        ElseIf fLine.IndexOf("DB:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                            Dim value As String, Ms As MatchCollection = Regex.Matches(fLine, "DB:[^,;\s]+")
                            For Each M As Match In Ms
                                value = Regex.Replace(M.Value, "(DB:|[,;\s])", "")
                                If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Dbs.Contains(value) Then ACLists.Dbs.Add(value)
                            Next
                        ElseIf fLine.IndexOf("DBResult:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                            Dim value As String, Ms As MatchCollection = Regex.Matches(fLine, "DBResult:[^,;\s]+")
                            For Each M As Match In Ms
                                value = Regex.Replace(M.Value, "(DBResult:|[,;\s])", "")
                                If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.DbRes.Contains(value) Then ACLists.DbRes.Add(value)
                            Next
                        ElseIf fLine.IndexOf("File:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                            Dim value As String, Ms As MatchCollection = Regex.Matches(fLine, "File:[^,;\s]+")
                            For Each M As Match In Ms
                                value = Regex.Replace(M.Value, "(File:|[,;\s])", "")
                                If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Files.Contains(value) Then ACLists.Files.Add(value)
                            Next
                        Else
                            Dim tDef As String, name As String, params As String(), func As PawnFunction
                            For Each def As CustomUserPublics In ACLists.UserDefinedPublics
                                Dim M As Match = Regex.Match(fLine, def.Regex)
                                If M.Success Then
                                    tDef = def.Regex
                                    tmp = M.Value.Remove(0, tDef.IndexOf(".+"))
                                    tDef = tDef.Remove(0, tDef.IndexOf(".+"))
                                    name = Regex.Match(tmp, Regex.Escape(Mid(tDef, 1, tDef.IndexOf(".+", 1))).Replace("\.", ".").Replace("\+", "+")).Value
                                    tDef = tDef.Remove(0, 2)
                                    tmp = tmp.Replace(name, "")
                                    name = name.Remove(name.Length - 1, 1)
                                    params = Regex.Split(Mid(tmp, 1, tmp.Length - 2), "[\s]?,[\s]?")
                                    func = New PawnFunction(name, _Name.Replace(".inc", ":"), linecounter, params)
                                    If Not TrueContainsFunction(ACLists.Functions, func) Then ACLists.Functions.Add(func)
                                End If
                            Next
                        End If
                        fLine = Reader.ReadLine()
                    Loop
                    Reader.Close()
                Else
                    Errors.Clear()
                    Errors.Add(New ListViewItem(New String() {"", "100", Name, linecounter + 1, "cannot read from file: """ & file & """"}, 0))
                End If
            ElseIf mLine.IndexOf("#tryinclude") > -1 Then
                Dim file As String, path As String
                If mLine.IndexOf("<") > -1 Then
                    file = Mid(mLine, mLine.IndexOf("<") + 2, mLine.IndexOf(">") - mLine.IndexOf("<") - 1)
                    path = My.Application.Info.DirectoryPath & "\Include\" & file & ".inc"
                Else
                    If mLine.IndexOf("..") = -1 Then
                        file = Mid(mLine, mLine.IndexOf("""") + 2, mLine.LastIndexOf("""") - mLine.IndexOf("""") - 1)
                        path = My.Application.Info.DirectoryPath & "\Include\" & file & If(file.IndexOf(".inc") = -1, ".inc", "")
                    Else
                        file = Mid(mLine, mLine.IndexOf("..") + 3, mLine.LastIndexOf("""") - mLine.IndexOf("""") - 1).Replace("/", "\")
                        path = Directory.GetParent(My.Application.Info.DirectoryPath).FullName & file & If(file.IndexOf(".inc") = -1, ".inc", "")
                    End If
                End If
                If IO.File.Exists(path) Then
                    Dim fLine As String, Reader As New StreamReader(path)
                    fLine = Reader.ReadLine()
                    Dim CommentedLine2 As Boolean, CommentedSection2 As Boolean
                    Do Until fLine Is Nothing
                        If fLine.Length = 0 OrElse fLine = "{" OrElse fLine = "}" OrElse fLine = ";" Then
                            fLine = Reader.ReadLine()
                            Continue Do
                        ElseIf fLine.StartsWith("//") Then
                            CommentedLine2 = True
                        ElseIf fLine = "/*" OrElse fLine = " /*" Then
                            CommentedSection2 = True
                            fLine = Reader.ReadLine()
                            Continue Do
                        ElseIf fLine = "*/" OrElse fLine = " */" Then
                            CommentedSection2 = False
                            fLine = Reader.ReadLine()
                            Continue Do
                        ElseIf fLine.IndexOf("/*") > -1 AndAlso fLine.IndexOf("*/") = -1 Then
                            CommentedSection2 = True
                        ElseIf fLine.IndexOf("*/") > -1 Then
                            CommentedSection2 = False
                        End If
                        If CommentedLine2 Or CommentedSection2 Then
                            CommentedLine2 = False
                            fLine = Reader.ReadLine()
                            Continue Do
                        End If
                        spos = fLine.IndexOf("native")
                        If spos = -1 Then
                            spos = fLine.IndexOf("stock")
                            If spos = -1 Then spos = fLine.IndexOf("public")
                        End If
                        If fLine.IndexOf("#include") > -1 Then
                            Dim file2 As String, path2 As String, cNode2 As New TreeNode()
                            If fLine.IndexOf("<") > -1 Then
                                file2 = Mid(fLine, fLine.IndexOf("<") + 2, fLine.IndexOf(">") - fLine.IndexOf("<") - 1)
                                path2 = My.Application.Info.DirectoryPath & "\Include\" & file2 & ".inc"
                            ElseIf fLine.IndexOf("""") > -1 Then
                                If fLine.IndexOf("..") = -1 Then
                                    file2 = Mid(fLine, fLine.IndexOf("""") + 2, fLine.LastIndexOf("""") - fLine.IndexOf("""") - 1)
                                    path2 = My.Application.Info.DirectoryPath & "\Include\" & file2 & If(file2.IndexOf(".inc") = -1, ".inc", "")
                                Else
                                    file2 = Mid(fLine, fLine.IndexOf("..") + 3, fLine.LastIndexOf("""") - fLine.IndexOf("""") - 1).Replace("/", "\")
                                    path2 = Directory.GetParent(My.Application.Info.DirectoryPath).FullName & file2 & If(file2.IndexOf(".inc") = -1, ".inc", "")
                                End If
                            Else
                                fLine = Reader.ReadLine()
                                Continue Do
                            End If
                            Dim count As Integer
                            If IO.File.Exists(path2) Then
                                Dim Reader2 As New StreamReader(path2)
                                fLine = Reader2.ReadLine()
                                Dim CommentedLine3 As Boolean, CommentedSection3 As Boolean
                                Do Until fLine Is Nothing
                                    If fLine.Length = 0 OrElse fLine = "{" OrElse fLine = "}" OrElse fLine = ";" Then
                                        count += 1
                                        fLine = Reader2.ReadLine()
                                        Continue Do
                                    ElseIf fLine.StartsWith("//") Then
                                        CommentedLine3 = True
                                    ElseIf fLine = "/*" OrElse fLine = " /*" Then
                                        CommentedSection3 = True
                                        count += 1
                                        fLine = Reader2.ReadLine()
                                        Continue Do
                                    ElseIf fLine = "*/" OrElse fLine = " */" Then
                                        CommentedSection3 = False
                                        count += 1
                                        fLine = Reader2.ReadLine()
                                        Continue Do
                                    ElseIf fLine.IndexOf("/*") > -1 AndAlso fLine.IndexOf("*/") = -1 Then
                                        CommentedSection3 = True
                                    ElseIf fLine.IndexOf("*/") > -1 Then
                                        CommentedSection3 = False
                                    End If
                                    If CommentedLine3 Or CommentedSection3 Then
                                        CommentedLine3 = False
                                        count += 1
                                        fLine = Reader2.ReadLine()
                                        Continue Do
                                    End If
                                    spos = fLine.IndexOf("native")
                                    If spos = -1 Then
                                        spos = fLine.IndexOf("stock")
                                        If spos = -1 Then spos = fLine.IndexOf("public")
                                    End If
                                    If spos > -1 AndAlso (fLine.EndsWith(";") AndAlso fLine.StartsWith("native ")) AndAlso fLine.IndexOf("(") > -1 AndAlso fLine.IndexOf(")") > -1 AndAlso fLine.IndexOf("operator") = -1 Then
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
                                        Dim value As String, Ms As MatchCollection = Regex.Matches(fLine, "Menu:[^,;\s]+")
                                        For Each M As Match In Ms
                                            value = Regex.Replace(M.Value, "(Menu:|[,;\s])", "")
                                            If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Menus.Contains(value) Then ACLists.Menus.Add(value)
                                        Next
                                    ElseIf fLine.IndexOf("Text:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                                        Dim value As String, Ms As MatchCollection = Regex.Matches(fLine, "Text:[^,;\s]+")
                                        For Each M As Match In Ms
                                            value = Regex.Replace(M.Value, "(Text:|[,;\s])", "")
                                            If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Texts.Contains(value) Then ACLists.Texts.Add(value)
                                        Next
                                    ElseIf fLine.IndexOf("Text3D:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                                        Dim value As String, Ms As MatchCollection = Regex.Matches(fLine, "Text3D:[^,;\s]+")
                                        For Each M As Match In Ms
                                            value = Regex.Replace(M.Value, "(Text3D:|[,;\s])", "")
                                            If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Texts2.Contains(value) Then ACLists.Texts2.Add(value)
                                        Next
                                    ElseIf fLine.IndexOf("Float:") > -1 AndAlso fLine.IndexOf("cellmin") = -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                                        Dim value As String, Ms As MatchCollection = Regex.Matches(fLine, "Float:[^,;\s]+")
                                        For Each M As Match In Ms
                                            value = Regex.Replace(M.Value, "(Float:|[,;\s])", "")
                                            If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Floats.Contains(value) Then ACLists.Floats.Add(value)
                                        Next
                                    ElseIf fLine.IndexOf("DB:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                                        Dim value As String, Ms As MatchCollection = Regex.Matches(fLine, "DB:[^,;\s]+")
                                        For Each M As Match In Ms
                                            value = Regex.Replace(M.Value, "(DB:|[,;\s])", "")
                                            If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Dbs.Contains(value) Then ACLists.Dbs.Add(value)
                                        Next
                                    ElseIf fLine.IndexOf("DBResult:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                                        Dim value As String, Ms As MatchCollection = Regex.Matches(fLine, "DBResult:[^,;\s]+")
                                        For Each M As Match In Ms
                                            value = Regex.Replace(M.Value, "(DBResult:|[,;\s])", "")
                                            If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.DbRes.Contains(value) Then ACLists.DbRes.Add(value)
                                        Next
                                    ElseIf fLine.IndexOf("File:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                                        Dim value As String, Ms As MatchCollection = Regex.Matches(fLine, "File:[^,;\s]+")
                                        For Each M As Match In Ms
                                            value = Regex.Replace(M.Value, "(File:|[,;\s])", "")
                                            If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Files.Contains(value) Then ACLists.Files.Add(value)
                                        Next
                                    Else
                                        Dim tDef As String, name As String, params As String(), func As PawnFunction
                                        For Each def As CustomUserPublics In ACLists.UserDefinedPublics
                                            Dim M As Match = Regex.Match(fLine, def.Regex)
                                            If M.Success Then
                                                tDef = def.Regex
                                                tmp = M.Value.Remove(0, tDef.IndexOf(".+"))
                                                tDef = tDef.Remove(0, tDef.IndexOf(".+"))
                                                name = Regex.Match(tmp, Regex.Escape(Mid(tDef, 1, tDef.IndexOf(".+", 1))).Replace("\.", ".").Replace("\+", "+")).Value
                                                tDef = tDef.Remove(0, 2)
                                                tmp = tmp.Replace(name, "")
                                                name = name.Remove(name.Length - 1, 1)
                                                params = Regex.Split(Mid(tmp, 1, tmp.Length - 2), "[\s]?,[\s]?")
                                                func = New PawnFunction(name, _Name.Replace(".inc", ":"), linecounter, params)
                                                If Not TrueContainsFunction(ACLists.Functions, func) Then ACLists.Functions.Add(func)
                                            End If
                                        Next
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
                            Dim file2 As String, path2 As String, cNode2 As New TreeNode()
                            If fLine.IndexOf("<") > -1 Then
                                file2 = Mid(fLine, fLine.IndexOf("<") + 2, fLine.IndexOf(">") - fLine.IndexOf("<") - 1)
                                path2 = My.Application.Info.DirectoryPath & "\Include\" & file2 & ".inc"
                            Else
                                If fLine.IndexOf("..") = -1 Then
                                    file2 = Mid(fLine, fLine.IndexOf("""") + 2, fLine.LastIndexOf("""") - fLine.IndexOf("""") - 1)
                                    path2 = My.Application.Info.DirectoryPath & "\Include\" & file2 & If(file.IndexOf(".inc") = -1, ".inc", "")
                                Else
                                    file2 = Mid(fLine, fLine.IndexOf("..") + 3, fLine.LastIndexOf("""") - fLine.IndexOf("""") - 1).Replace("/", "\")
                                    path2 = Directory.GetParent(My.Application.Info.DirectoryPath).FullName & file2 & If(file2.IndexOf(".inc") = -1, ".inc", "")
                                End If
                            End If
                            Dim count As Integer
                            If IO.File.Exists(path2) Then
                                Dim Reader2 As New StreamReader(path2)
                                fLine = Reader2.ReadLine()
                                Dim CommentedLine3 As Boolean, CommentedSection3 As Boolean
                                Do Until fLine Is Nothing
                                    If fLine.Length = 0 OrElse fLine = "{" OrElse fLine = "}" OrElse fLine = ";" Then
                                        count += 1
                                        fLine = Reader2.ReadLine()
                                        Continue Do
                                    ElseIf fLine.StartsWith("//") Then
                                        CommentedLine3 = True
                                    ElseIf fLine = "/*" OrElse fLine = " /*" Then
                                        CommentedSection3 = True
                                        count += 1
                                        fLine = Reader2.ReadLine()
                                        Continue Do
                                    ElseIf fLine = "*/" OrElse fLine = " */" Then
                                        CommentedSection3 = False
                                        count += 1
                                        fLine = Reader2.ReadLine()
                                        Continue Do
                                    ElseIf fLine.IndexOf("/*") > -1 AndAlso fLine.IndexOf("*/") = -1 Then
                                        CommentedSection3 = True
                                    ElseIf fLine.IndexOf("*/") > -1 Then
                                        CommentedSection3 = False
                                    End If
                                    If CommentedLine3 Or CommentedSection3 Then
                                        CommentedLine3 = False
                                        count += 1
                                        fLine = Reader2.ReadLine()
                                        Continue Do
                                    End If
                                    spos = fLine.IndexOf("native")
                                    If spos = -1 Then
                                        spos = fLine.IndexOf("stock")
                                        If spos = -1 Then spos = fLine.IndexOf("public")
                                    End If
                                    If spos > -1 AndAlso (fLine.EndsWith(";") AndAlso fLine.StartsWith("native ")) AndAlso fLine.IndexOf("(") > -1 AndAlso fLine.IndexOf(")") > -1 AndAlso fLine.IndexOf("operator") = -1 Then
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
                                        Dim value As String, Ms As MatchCollection = Regex.Matches(fLine, "Menu:[^,;\s]+")
                                        For Each M As Match In Ms
                                            value = Regex.Replace(M.Value, "(Menu:|[,;\s])", "")
                                            If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Menus.Contains(value) Then ACLists.Menus.Add(value)
                                        Next
                                    ElseIf fLine.IndexOf("Text:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                                        Dim value As String, Ms As MatchCollection = Regex.Matches(fLine, "Text:[^,;\s]+")
                                        For Each M As Match In Ms
                                            value = Regex.Replace(M.Value, "(Text:|[,;\s])", "")
                                            If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Texts.Contains(value) Then ACLists.Texts.Add(value)
                                        Next
                                    ElseIf fLine.IndexOf("Text3D:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                                        Dim value As String, Ms As MatchCollection = Regex.Matches(fLine, "Text3D:[^,;\s]+")
                                        For Each M As Match In Ms
                                            value = Regex.Replace(M.Value, "(Text3D:|[,;\s])", "")
                                            If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Texts2.Contains(value) Then ACLists.Texts2.Add(value)
                                        Next
                                    ElseIf fLine.IndexOf("Float:") > -1 AndAlso fLine.IndexOf("cellmin") = -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                                        Dim value As String, Ms As MatchCollection = Regex.Matches(fLine, "Float:[^,;\s]+")
                                        For Each M As Match In Ms
                                            value = Regex.Replace(M.Value, "(Float:|[,;\s])", "")
                                            If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Floats.Contains(value) Then ACLists.Floats.Add(value)
                                        Next
                                    ElseIf fLine.IndexOf("DB:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                                        Dim value As String, Ms As MatchCollection = Regex.Matches(fLine, "DB:[^,;\s]+")
                                        For Each M As Match In Ms
                                            value = Regex.Replace(M.Value, "(DB:|[,;\s])", "")
                                            If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Dbs.Contains(value) Then ACLists.Dbs.Add(value)
                                        Next
                                    ElseIf fLine.IndexOf("DBResult:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                                        Dim value As String, Ms As MatchCollection = Regex.Matches(fLine, "DBResult:[^,;\s]+")
                                        For Each M As Match In Ms
                                            value = Regex.Replace(M.Value, "(DBResult:|[,;\s])", "")
                                            If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.DbRes.Contains(value) Then ACLists.DbRes.Add(value)
                                        Next
                                    ElseIf fLine.IndexOf("File:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                                        Dim value As String, Ms As MatchCollection = Regex.Matches(fLine, "File:[^,;\s]+")
                                        For Each M As Match In Ms
                                            value = Regex.Replace(M.Value, "(File:|[,;\s])", "")
                                            If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Files.Contains(value) Then ACLists.Files.Add(value)
                                        Next
                                    Else
                                        Dim tDef As String, name As String, params As String(), func As PawnFunction
                                        For Each def As CustomUserPublics In ACLists.UserDefinedPublics
                                            Dim M As Match = Regex.Match(fLine, def.Regex)
                                            If M.Success Then
                                                tDef = def.Regex
                                                tmp = M.Value.Remove(0, tDef.IndexOf(".+"))
                                                tDef = tDef.Remove(0, tDef.IndexOf(".+"))
                                                name = Regex.Match(tmp, Regex.Escape(Mid(tDef, 1, tDef.IndexOf(".+", 1))).Replace("\.", ".").Replace("\+", "+")).Value
                                                tDef = tDef.Remove(0, 2)
                                                tmp = tmp.Replace(name, "")
                                                name = name.Remove(name.Length - 1, 1)
                                                params = Regex.Split(Mid(tmp, 1, tmp.Length - 2), "[\s]?,[\s]?")
                                                func = New PawnFunction(name, _Name.Replace(".inc", ":"), linecounter, params)
                                                If Not TrueContainsFunction(ACLists.Functions, func) Then ACLists.Functions.Add(func)
                                            End If
                                        Next
                                    End If
                                    count += 1
                                    fLine = Reader2.ReadLine()
                                Loop
                                Reader2.Close()
                            Else
                                Errors.Clear()
                                Errors.Add(New ListViewItem(New String() {"", "100", Name, count, "cannot read from file: """ & file2 & """"}, 1))
                            End If
                        ElseIf spos > -1 AndAlso (fLine.EndsWith(";") AndAlso fLine.StartsWith("native ")) AndAlso fLine.IndexOf("(") > -1 AndAlso fLine.IndexOf(")") > -1 AndAlso fLine.IndexOf("operator") = -1 Then
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
                            Dim value As String, Ms As MatchCollection = Regex.Matches(fLine, "Menu:[^,;\s]+")
                            For Each M As Match In Ms
                                value = Regex.Replace(M.Value, "(Menu:|[,;\s])", "")
                                If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Menus.Contains(value) Then ACLists.Menus.Add(value)
                            Next
                        ElseIf fLine.IndexOf("Text:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                            Dim value As String, Ms As MatchCollection = Regex.Matches(fLine, "Text:[^,;\s]+")
                            For Each M As Match In Ms
                                value = Regex.Replace(M.Value, "(Text:|[,;\s])", "")
                                If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Texts.Contains(value) Then ACLists.Texts.Add(value)
                            Next
                        ElseIf fLine.IndexOf("Text3D:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                            Dim value As String, Ms As MatchCollection = Regex.Matches(fLine, "Text3D:[^,;\s]+")
                            For Each M As Match In Ms
                                value = Regex.Replace(M.Value, "(Text3D:|[,;\s])", "")
                                If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Texts2.Contains(value) Then ACLists.Texts2.Add(value)
                            Next
                        ElseIf fLine.IndexOf("Float:") > -1 AndAlso fLine.IndexOf("cellmin") = -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                            Dim value As String, Ms As MatchCollection = Regex.Matches(fLine, "Float:[^,;\s]+")
                            For Each M As Match In Ms
                                value = Regex.Replace(M.Value, "(Float:|[,;\s])", "")
                                If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Floats.Contains(value) Then ACLists.Floats.Add(value)
                            Next
                        ElseIf fLine.IndexOf("DB:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                            Dim value As String, Ms As MatchCollection = Regex.Matches(fLine, "DB:[^,;\s]+")
                            For Each M As Match In Ms
                                value = Regex.Replace(M.Value, "(DB:|[,;\s])", "")
                                If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Dbs.Contains(value) Then ACLists.Dbs.Add(value)
                            Next
                        ElseIf fLine.IndexOf("DBResult:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                            Dim value As String, Ms As MatchCollection = Regex.Matches(fLine, "DBResult:[^,;\s]+")
                            For Each M As Match In Ms
                                value = Regex.Replace(M.Value, "(DBResult:|[,;\s])", "")
                                If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.DbRes.Contains(value) Then ACLists.DbRes.Add(value)
                            Next
                        ElseIf fLine.IndexOf("File:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                            Dim value As String, Ms As MatchCollection = Regex.Matches(fLine, "File:[^,;\s]+")
                            For Each M As Match In Ms
                                value = Regex.Replace(M.Value, "(File:|[,;\s])", "")
                                If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Files.Contains(value) Then ACLists.Files.Add(value)
                            Next
                        Else
                            Dim tDef As String, name As String, params As String(), func As PawnFunction
                            For Each def As CustomUserPublics In ACLists.UserDefinedPublics
                                Dim M As Match = Regex.Match(fLine, def.Regex)
                                If M.Success Then
                                    tDef = def.Regex
                                    tmp = M.Value.Remove(0, tDef.IndexOf(".+"))
                                    tDef = tDef.Remove(0, tDef.IndexOf(".+"))
                                    name = Regex.Match(tmp, Regex.Escape(Mid(tDef, 1, tDef.IndexOf(".+", 1))).Replace("\.", ".").Replace("\+", "+")).Value
                                    tDef = tDef.Remove(0, 2)
                                    tmp = tmp.Replace(name, "")
                                    name = name.Remove(name.Length - 1, 1)
                                    params = Regex.Split(Mid(tmp, 1, tmp.Length - 2), "[\s]?,[\s]?")
                                    func = New PawnFunction(name, _Name.Replace(".inc", ":"), linecounter, params)
                                    If Not TrueContainsFunction(ACLists.Functions, func) Then ACLists.Functions.Add(func)
                                End If
                            Next
                        End If
                        fLine = Reader.ReadLine()
                    Loop
                    Reader.Close()
                Else
                    Errors.Clear()
                    Errors.Add(New ListViewItem(New String() {"", "100", Name, linecounter + 1, "cannot read from file: """ & file & """"}, 1))
                End If
            ElseIf spos > -1 AndAlso (mLine.EndsWith(";") AndAlso mLine.StartsWith("native ")) AndAlso mLine.IndexOf("(") > -1 AndAlso mLine.IndexOf(")") > -1 AndAlso mLine.IndexOf("operator") = -1 AndAlso mLine.IndexOf("#define") = -1 Then
                Dim params As New List(Of String)
                params.AddRange(Split(Trim(Mid(mLine, mLine.IndexOf("(") + 2, mLine.IndexOf(")") - mLine.IndexOf("(") - 1)), ","))
                For i = 0 To params.Count - 1
                    If i > 0 AndAlso params(i).Length > 0 AndAlso params(i).IndexOf("...") > -1 Then
                        params(i - 1) += "," & params(i)
                        params.RemoveAt(i)
                        Continue For
                    End If
                Next
                Dim func As PawnFunction = New PawnFunction(Trim(Mid(mLine, mLine.IndexOf(" ", spos) + 2, mLine.IndexOf("(") - mLine.IndexOf(" ", spos) - 1)), _Name.Replace(".inc", ":"), -1, params.ToArray)
                If Not TrueContainsFunction(ACLists.Functions, func, True) AndAlso Not TrueContainsFunction(ACLists.Callbacks, func) Then ACLists.Functions.Add(func)
            ElseIf mLine.IndexOf("forward") > -1 AndAlso mLine.IndexOf("#define") = -1 AndAlso mLine.IndexOf("(") > -1 AndAlso mLine.IndexOf(")") > -1 Then
                Dim func As PawnFunction = New PawnFunction(Trim(Mid(mLine, mLine.IndexOf(" ") + 1, mLine.IndexOf("(") - mLine.IndexOf(" "))), _Name.Replace(".inc", ":"), -1, Split(Trim(Mid(mLine, mLine.IndexOf("(") + 2, mLine.IndexOf(")") - mLine.IndexOf("(") - 1)), ","))
                If Not TrueContainsFunction(ACLists.Callbacks, func, True) Then ACLists.Callbacks.Add(func)
            ElseIf mLine.IndexOf("#define") > -1 Then
                If mLine.IndexOf("0x") > -1 Then
                    Dim col As PawnColor, value As String
                    If mLine.IndexOf("(") > -1 AndAlso mLine.IndexOf(")") > -1 Then
                        value = Trim(Mid(mLine, mLine.IndexOf("(") + 4, 8))
                        If IsHex(value) Then
                            col = New PawnColor(Trim(Mid(mLine, mLine.IndexOf("#define") + 9, If(mLine.IndexOf(" ", mLine.IndexOf(" ") + 2) > 0, mLine.IndexOf(" ", mLine.IndexOf(" ") + 2) - mLine.IndexOf(" ") - 1, mLine.IndexOf(vbTab.ToString()) - mLine.IndexOf(" ") - 1))), Color.FromArgb(Integer.Parse(Mid(value, 7, 2), Globalization.NumberStyles.HexNumber), Integer.Parse(Mid(value, 1, 2), Globalization.NumberStyles.HexNumber), Integer.Parse(Mid(value, 3, 2), Globalization.NumberStyles.HexNumber), Integer.Parse(Mid(value, 5, 2), Globalization.NumberStyles.HexNumber)), linecounter)
                            If Not ACLists.Colors.Contains(col) Then ACLists.Colors.Add(col)
                        End If
                    Else
                        value = Trim(Mid(mLine, mLine.IndexOf("0x") + 3, 8))
                        If IsHex(value) Then
                            col = New PawnColor(Trim(Mid(mLine, mLine.IndexOf("#define") + 9, If(mLine.IndexOf(" ", mLine.IndexOf(" ") + 2) > 0, mLine.IndexOf(" ", mLine.IndexOf(" ") + 2) - mLine.IndexOf(" ") - 1, mLine.IndexOf(vbTab.ToString()) - mLine.IndexOf(" ") - 1))), Color.FromArgb(Integer.Parse(Mid(value, 7, 2), Globalization.NumberStyles.HexNumber), Integer.Parse(Mid(value, 1, 2), Globalization.NumberStyles.HexNumber), Integer.Parse(Mid(value, 3, 2), Globalization.NumberStyles.HexNumber), Integer.Parse(Mid(value, 5, 2), Globalization.NumberStyles.HexNumber)), linecounter)
                            If Not ACLists.Colors.Contains(col) Then ACLists.Colors.Add(col)
                        End If
                    End If
                Else
                    If mLine.IndexOf("""") > -1 AndAlso mLine.IndexOf("""", mLine.IndexOf("""") + 1) > -1 Then
                        Dim value As String
                        If mLine.IndexOf("{") > -1 AndAlso mLine.IndexOf("}") > -1 Then
                            value = Trim(Mid(mLine, mLine.IndexOf("{") + 2, 6))
                        Else
                            value = Trim(Mid(mLine, mLine.IndexOf("""") + 2, 6))
                        End If
                        If IsHex(value) Then
                            Dim col As New PawnColor(Trim(Mid(mLine, mLine.IndexOf("#define") + 9, If(mLine.IndexOf(" ", mLine.IndexOf(" ") + 2) > 0, mLine.IndexOf(" ", mLine.IndexOf(" ") + 2) - mLine.IndexOf(" ") - 1, mLine.IndexOf(vbTab.ToString()) - mLine.IndexOf(" ") - 1))), Color.FromArgb(255, Integer.Parse(Mid(value, 1, 2), Globalization.NumberStyles.HexNumber), Integer.Parse(Mid(value, 3, 2), Globalization.NumberStyles.HexNumber), Integer.Parse(Mid(value, 5, 2), Globalization.NumberStyles.HexNumber)), -1)
                            If col.Name.Length > 0 AndAlso Not ACLists.eColors.Contains(col) Then ACLists.eColors.Add(col)
                        End If
                    End If
                End If
            ElseIf mLine.IndexOf("Menu:") > -1 AndAlso mLine.IndexOf("(") = -1 AndAlso mLine.IndexOf(")") = -1 Then
                Dim value As String, Ms As MatchCollection = Regex.Matches(mLine, "Menu:[^,;\s]+")
                For Each M As Match In Ms
                    value = Regex.Replace(M.Value, "(Menu:|[,;\s])", "")
                    If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Menus.Contains(value) Then ACLists.Menus.Add(value)
                Next
            ElseIf mLine.IndexOf("Text:") > -1 AndAlso mLine.IndexOf("(") = -1 AndAlso mLine.IndexOf(")") = -1 Then
                Dim value As String, Ms As MatchCollection = Regex.Matches(mLine, "Text:[^,;\s]+")
                For Each M As Match In Ms
                    value = Regex.Replace(M.Value, "(Text:|[,;\s])", "")
                    If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Texts.Contains(value) Then ACLists.Texts.Add(value)
                Next
            ElseIf mLine.IndexOf("Text3D:") > -1 AndAlso mLine.IndexOf("(") = -1 AndAlso mLine.IndexOf(")") = -1 Then
                Dim value As String, Ms As MatchCollection = Regex.Matches(mLine, "Text3D:[^,;\s]+")
                For Each M As Match In Ms
                    value = Regex.Replace(M.Value, "(Text3D:|[,;\s])", "")
                    If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Texts2.Contains(value) Then ACLists.Texts2.Add(value)
                Next
            ElseIf mLine.IndexOf("Float:") > -1 AndAlso mLine.IndexOf("cellmin") = -1 AndAlso mLine.IndexOf("(") = -1 AndAlso mLine.IndexOf(")") = -1 Then
                Dim value As String, Ms As MatchCollection = Regex.Matches(mLine, "Float:[^,;\s]+")
                For Each M As Match In Ms
                    value = Regex.Replace(M.Value, "(Float:|[,;\s])", "")
                    If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Floats.Contains(value) Then ACLists.Floats.Add(value)
                Next
            ElseIf mLine.IndexOf("DB:") > -1 AndAlso mLine.IndexOf("(") = -1 AndAlso mLine.IndexOf(")") = -1 Then
                Dim value As String, Ms As MatchCollection = Regex.Matches(mLine, "DB:[^,;\s]+")
                For Each M As Match In Ms
                    value = Regex.Replace(M.Value, "(DB:|[,;\s])", "")
                    If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Dbs.Contains(value) Then ACLists.Dbs.Add(value)
                Next
            ElseIf mLine.IndexOf("DBResult:") > -1 AndAlso mLine.IndexOf("(") = -1 AndAlso mLine.IndexOf(")") = -1 Then
                Dim value As String, Ms As MatchCollection = Regex.Matches(mLine, "DBResult:[^,;\s]+")
                For Each M As Match In Ms
                    value = Regex.Replace(M.Value, "(DBResult:|[,;\s])", "")
                    If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.DbRes.Contains(value) Then ACLists.DbRes.Add(value)
                Next
            ElseIf mLine.IndexOf("File:") > -1 AndAlso mLine.IndexOf("(") = -1 AndAlso mLine.IndexOf(")") = -1 Then
                Dim value As String, Ms As MatchCollection = Regex.Matches(mLine, "File:[^,;\s]+")
                For Each M As Match In Ms
                    value = Regex.Replace(M.Value, "(File:|[,;\s])", "")
                    If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Files.Contains(value) Then ACLists.Files.Add(value)
                Next
            Else
                If ACLists.UserDefinedPublics.Count > 0 Then
                    Dim tDef As String, name As String, params As String(), func As PawnFunction
                    For Each def As CustomUserPublics In ACLists.UserDefinedPublics
                        Dim M As Match = Regex.Match(mLine, def.Regex)
                        If M.Success Then
                            tDef = def.Regex
                            tmp = M.Value.Remove(0, tDef.IndexOf(".+"))
                            tDef = tDef.Remove(0, tDef.IndexOf(".+"))
                            name = Regex.Match(tmp, Regex.Escape(Mid(tDef, 1, tDef.IndexOf(".+", 1))).Replace("\.", ".").Replace("\+", "+")).Value
                            tDef = tDef.Remove(0, 2)
                            tmp = tmp.Replace(name, "")
                            name = name.Remove(name.Length - 1, 1)
                            params = Regex.Split(Mid(tmp, 1, tmp.Length - 2), "[\s]?,[\s]?")
                            func = New PawnFunction(name, _Name.Replace(".inc", ":"), linecounter, params)
                            If Not TrueContainsFunction(ACLists.Functions, func) Then ACLists.Functions.Add(func)
                        End If
                    Next
                End If
            End If
            mLine = MainReader.ReadLine()
        End While
        Dim tmpstring As String = vbNullString
        For Each item As PawnFunction In ACLists.Functions
            tmpstring += item.Name & " "
        Next
        SyntaxHandle.Lexing.Keywords(3) = tmpstring
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
        Colorize()
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
                    If Line.Text.Length = 0 OrElse Line.Text = "{" OrElse Line.Text = "}" OrElse Line.Text = ";" Then
                        Continue For
                    ElseIf Line.Text.StartsWith("//") Then
                        CommentedLine = True
                        Continue For
                    ElseIf Line.Text = "/*" OrElse Line.Text = " /*" Then
                        CommentedSection = True
                        Continue For
                    ElseIf Line.Text = "*/" OrElse Line.Text = " */" Then
                        CommentedSection = False
                        Continue For
                    ElseIf Line.Text.IndexOf("/*") > -1 AndAlso Line.Text.IndexOf("*/") = -1 Then
                        CommentedSection = True
                    ElseIf Line.Text.IndexOf("*/") > -1 Then
                        CommentedSection = False
                    End If
                    If CommentedLine Or CommentedSection Then
                        CommentedLine = False
                        Continue For
                    End If
                    If Line.Text.IndexOf("#include") > -1 Then
                        Dim file As String, path As String
                        If Line.Text.IndexOf("<") > -1 Then
                            file = Mid(Line.Text, Line.Text.IndexOf("<") + 2, Line.Text.IndexOf(">") - Line.Text.IndexOf("<") - 1)
                            path = My.Application.Info.DirectoryPath & "\Include\" & file & ".inc"
                        ElseIf Line.Text.IndexOf("""") > -1 Then
                            If Line.Text.IndexOf("..") = -1 Then
                                file = Mid(Line.Text, Line.Text.IndexOf("""") + 2, Line.Text.LastIndexOf("""") - Line.Text.IndexOf("""") - 1)
                                path = My.Application.Info.DirectoryPath & "\Include\" & file & If(file.IndexOf(".inc") = -1, ".inc", "")
                            Else
                                file = Mid(Line.Text, Line.Text.IndexOf("..") + 3, Line.Text.LastIndexOf("""") - Line.Text.IndexOf("""") - 1).Replace("/", "\")
                                path = Directory.GetParent(My.Application.Info.DirectoryPath).FullName & file & If(file.IndexOf(".inc") = -1, ".inc", "")
                            End If
                        Else
                            Continue For
                        End If
                        If IO.File.Exists(path) Then
                            Dim fLine As String, Reader As New StreamReader(path)
                            fLine = Reader.ReadLine()
                            Dim CommentedLine2 As Boolean, CommentedSection2 As Boolean
                            Do Until fLine Is Nothing
                                If fLine.Length = 0 OrElse fLine = "{" OrElse fLine = "}" OrElse fLine = ";" Then
                                    fLine = Reader.ReadLine()
                                    Continue Do
                                ElseIf fLine.StartsWith("//") Then
                                    CommentedLine2 = True
                                ElseIf fLine = "/*" OrElse fLine = " /*" Then
                                    CommentedSection2 = True
                                    fLine = Reader.ReadLine()
                                    Continue Do
                                ElseIf fLine = "*/" OrElse fLine = " */" Then
                                    CommentedSection2 = False
                                    fLine = Reader.ReadLine()
                                    Continue Do
                                ElseIf fLine.IndexOf("/*") > -1 AndAlso fLine.IndexOf("*/") = -1 Then
                                    CommentedSection2 = True
                                ElseIf fLine.IndexOf("*/") > -1 Then
                                    CommentedSection2 = False
                                End If
                                If CommentedLine2 Or CommentedSection2 Then
                                    CommentedLine2 = False
                                    fLine = Reader.ReadLine()
                                    Continue Do
                                End If
                                Dim pos As Integer = fLine.IndexOf("native")
                                If pos = -1 Then
                                    pos = fLine.IndexOf("stock")
                                    If pos = -1 Then pos = fLine.IndexOf("public")
                                End If
                                If fLine.IndexOf("#include") > -1 Then
                                    Dim file2 As String, path2 As String, cNode2 As New TreeNode()
                                    If fLine.IndexOf("<") > -1 Then
                                        file2 = Mid(fLine, fLine.IndexOf("<") + 2, fLine.IndexOf(">") - fLine.IndexOf("<") - 1)
                                        path2 = My.Application.Info.DirectoryPath & "\Include\" & file2 & ".inc"
                                    ElseIf fLine.IndexOf("""") > -1 Then
                                        If fLine.IndexOf("..") = -1 Then
                                            file2 = Mid(fLine, fLine.IndexOf("""") + 2, fLine.LastIndexOf("""") - fLine.IndexOf("""") - 1)
                                            path2 = My.Application.Info.DirectoryPath & "\Include\" & file2 & If(file2.IndexOf(".inc") = -1, ".inc", "")
                                        Else
                                            file2 = Mid(fLine, fLine.IndexOf("..") + 3, fLine.LastIndexOf("""") - fLine.IndexOf("""") - 1).Replace("/", "\")
                                            path2 = Directory.GetParent(My.Application.Info.DirectoryPath).FullName & file2 & If(file2.IndexOf(".inc") = -1, ".inc", "")
                                        End If
                                    Else
                                        fLine = Reader.ReadLine()
                                        Continue Do
                                    End If
                                    Dim count As Integer
                                    If IO.File.Exists(path2) Then
                                        Dim Reader2 As New StreamReader(path2)
                                        fLine = Reader2.ReadLine()
                                        Dim CommentedLine3 As Boolean, CommentedSection3 As Boolean
                                        Do Until fLine Is Nothing
                                            If fLine.Length = 0 OrElse fLine = "{" OrElse fLine = "}" OrElse fLine = ";" Then
                                                count += 1
                                                fLine = Reader2.ReadLine()
                                                Continue Do
                                            ElseIf fLine.StartsWith("//") Then
                                                CommentedLine3 = True
                                            ElseIf fLine = "/*" OrElse fLine = " /*" Then
                                                CommentedSection3 = True
                                                count += 1
                                                fLine = Reader2.ReadLine()
                                                Continue Do
                                            ElseIf fLine = "*/" OrElse fLine = " */" Then
                                                CommentedSection3 = False
                                                count += 1
                                                fLine = Reader2.ReadLine()
                                                Continue Do
                                            ElseIf fLine.IndexOf("/*") > -1 AndAlso fLine.IndexOf("*/") = -1 Then
                                                CommentedSection3 = True
                                            ElseIf fLine.IndexOf("*/") > -1 Then
                                                CommentedSection3 = False
                                            End If
                                            If CommentedLine3 Or CommentedSection3 Then
                                                CommentedLine3 = False
                                                count += 1
                                                fLine = Reader2.ReadLine()
                                                Continue Do
                                            End If
                                            pos = fLine.IndexOf("native")
                                            If pos = -1 Then
                                                pos = fLine.IndexOf("stock")
                                                If pos = -1 Then pos = fLine.IndexOf("public")
                                            End If
                                            If pos > -1 AndAlso (fLine.EndsWith(";") AndAlso fLine.StartsWith("native ")) AndAlso fLine.IndexOf("(") > -1 AndAlso fLine.IndexOf(")") > -1 AndAlso fLine.IndexOf("operator") = -1 Then
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
                                            Else
                                                Dim tDef As String, name As String, params As String(), func As PawnFunction
                                                For Each def As CustomUserPublics In ACLists.UserDefinedPublics
                                                    Dim M As Match = Regex.Match(fLine, def.Regex)
                                                    If M.Success Then
                                                        tDef = def.Regex
                                                        tmp = M.Value.Remove(0, tDef.IndexOf(".+"))
                                                        tDef = tDef.Remove(0, tDef.IndexOf(".+"))
                                                        name = Regex.Match(tmp, Regex.Escape(Mid(tDef, 1, tDef.IndexOf(".+", 1))).Replace("\.", ".").Replace("\+", "+")).Value
                                                        tDef = tDef.Remove(0, 2)
                                                        tmp = tmp.Replace(name, "")
                                                        name = name.Remove(name.Length - 1, 1)
                                                        params = Regex.Split(Mid(tmp, 1, tmp.Length - 2), "[\s]?,[\s]?")
                                                        func = New PawnFunction(name, _Name.Replace(".inc", ":"), Line.Number, params)
                                                        If Not TrueContainsFunction(ACLists.Functions, func, True) Then ACLists.Functions.Add(func)
                                                    End If
                                                Next
                                            End If
                                            fLine = Reader2.ReadLine()
                                        Loop
                                        Reader2.Close()
                                    Else
                                        Errors.Clear()
                                        Errors.Add(New ListViewItem(New String() {"", "100", Name, Line.Number + 1, "cannot read from file: """ & file2 & """"}, 0))
                                    End If
                                ElseIf fLine.IndexOf("#tryinclude") > -1 Then
                                    Dim file2 As String, path2 As String, cNode2 As New TreeNode()
                                    If fLine.IndexOf("<") > -1 Then
                                        file2 = Mid(fLine, fLine.IndexOf("<") + 2, fLine.IndexOf(">") - fLine.IndexOf("<") - 1)
                                        path2 = My.Application.Info.DirectoryPath & "\Include\" & file2 & ".inc"
                                    Else
                                        If fLine.IndexOf("..") = -1 Then
                                            file2 = Mid(fLine, fLine.IndexOf("""") + 2, fLine.LastIndexOf("""") - fLine.IndexOf("""") - 1)
                                            path2 = My.Application.Info.DirectoryPath & "\Include\" & file2 & If(file2.IndexOf(".inc") = -1, ".inc", "")
                                        Else
                                            file2 = Mid(fLine, fLine.IndexOf("..") + 3, fLine.LastIndexOf("""") - fLine.IndexOf("""") - 1).Replace("/", "\")
                                            path2 = Directory.GetParent(My.Application.Info.DirectoryPath).FullName & file2 & If(file2.IndexOf(".inc") = -1, ".inc", "")
                                        End If
                                    End If
                                    Dim count As Integer
                                    If IO.File.Exists(path2) Then
                                        Dim Reader2 As New StreamReader(path2)
                                        fLine = Reader2.ReadLine()
                                        Dim CommentedLine3 As Boolean, CommentedSection3 As Boolean
                                        Do Until fLine Is Nothing
                                            If fLine.Length = 0 OrElse fLine = "{" OrElse fLine = "}" OrElse fLine = ";" Then
                                                count += 1
                                                fLine = Reader2.ReadLine()
                                                Continue Do
                                            ElseIf fLine.StartsWith("//") Then
                                                CommentedLine3 = True
                                            ElseIf fLine = "/*" OrElse fLine = " /*" Then
                                                CommentedSection3 = True
                                                count += 1
                                                fLine = Reader2.ReadLine()
                                                Continue Do
                                            ElseIf fLine = "*/" OrElse fLine = " */" Then
                                                count += 1
                                                CommentedSection3 = False
                                                fLine = Reader2.ReadLine()
                                                Continue Do
                                            ElseIf fLine.IndexOf("/*") > -1 AndAlso fLine.IndexOf("*/") = -1 Then
                                                CommentedSection3 = True
                                            ElseIf fLine.IndexOf("*/") > -1 Then
                                                CommentedSection3 = False
                                            End If
                                            If CommentedLine3 Or CommentedSection3 Then
                                                CommentedLine3 = False
                                                count += 1
                                                fLine = Reader2.ReadLine()
                                                Continue Do
                                            End If
                                            pos = fLine.IndexOf("native")
                                            If pos = -1 Then
                                                pos = fLine.IndexOf("stock")
                                                If pos = -1 Then pos = fLine.IndexOf("public")
                                            End If
                                            If pos > -1 AndAlso (fLine.EndsWith(";") AndAlso fLine.StartsWith("native ")) AndAlso fLine.IndexOf("(") > -1 AndAlso fLine.IndexOf(")") > -1 AndAlso fLine.IndexOf("operator") = -1 Then
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
                                            Else
                                                Dim tDef As String, name As String, params As String(), func As PawnFunction
                                                For Each def As CustomUserPublics In ACLists.UserDefinedPublics
                                                    Dim M As Match = Regex.Match(fLine, def.Regex)
                                                    If M.Success Then
                                                        tDef = def.Regex
                                                        tmp = M.Value.Remove(0, tDef.IndexOf(".+"))
                                                        tDef = tDef.Remove(0, tDef.IndexOf(".+"))
                                                        name = Regex.Match(tmp, Regex.Escape(Mid(tDef, 1, tDef.IndexOf(".+", 1))).Replace("\.", ".").Replace("\+", "+")).Value
                                                        tDef = tDef.Remove(0, 2)
                                                        tmp = tmp.Replace(name, "")
                                                        name = name.Remove(name.Length - 1, 1)
                                                        params = Regex.Split(Mid(tmp, 1, tmp.Length - 2), "[\s]?,[\s]?")
                                                        func = New PawnFunction(name, _Name.Replace(".inc", ":"), Line.Number, params)
                                                        If Not TrueContainsFunction(ACLists.Functions, func, True) Then ACLists.Functions.Add(func)
                                                    End If
                                                Next
                                            End If
                                            fLine = Reader2.ReadLine()
                                        Loop
                                        Reader2.Close()
                                    Else
                                        Errors.Clear()
                                        Errors.Add(New ListViewItem(New String() {"", "100", Name, Line.Number + 1, "cannot read from file: """ & file2 & """"}, 1))
                                    End If
                                ElseIf pos > -1 AndAlso (fLine.EndsWith(";") AndAlso fLine.StartsWith("native ")) AndAlso fLine.IndexOf("(") > -1 AndAlso fLine.IndexOf(")") > -1 AndAlso fLine.IndexOf("operator") = -1 Then
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
                                Else
                                    Dim tDef As String, name As String, params As String(), func As PawnFunction
                                    For Each def As CustomUserPublics In ACLists.UserDefinedPublics
                                        Dim M As Match = Regex.Match(fLine, def.Regex)
                                        If M.Success Then
                                            tDef = def.Regex
                                            tmp = M.Value.Remove(0, tDef.IndexOf(".+"))
                                            tDef = tDef.Remove(0, tDef.IndexOf(".+"))
                                            name = Regex.Match(tmp, Regex.Escape(Mid(tDef, 1, tDef.IndexOf(".+", 1))).Replace("\.", ".").Replace("\+", "+")).Value
                                            tDef = tDef.Remove(0, 2)
                                            tmp = tmp.Replace(name, "")
                                            name = name.Remove(name.Length - 1, 1)
                                            params = Regex.Split(Mid(tmp, 1, tmp.Length - 2), "[\s]?,[\s]?")
                                            func = New PawnFunction(name, _Name.Replace(".inc", ":"), Line.Number, params)
                                            If Not TrueContainsFunction(ACLists.Functions, func, True) Then ACLists.Functions.Add(func)
                                        End If
                                    Next
                                End If
                                fLine = Reader.ReadLine()
                            Loop
                            Reader.Close()
                        Else
                            Errors.Clear()
                            Errors.Add(New ListViewItem(New String() {"", "100", Name, Line.Number + 1, "cannot read from file: """ & file & """"}, 0))
                        End If
                    ElseIf Line.Text.IndexOf("#tryinclude") > -1 Then
                        Dim file As String, path As String
                        If Line.Text.IndexOf("<") > -1 Then
                            file = Mid(Line.Text, Line.Text.IndexOf("<") + 2, Line.Text.IndexOf(">") - Line.Text.IndexOf("<") - 1)
                            path = My.Application.Info.DirectoryPath & "\Include\" & file & ".inc"
                        Else
                            If Line.Text.IndexOf("..") = -1 Then
                                file = Mid(Line.Text, Line.Text.IndexOf("""") + 2, Line.Text.LastIndexOf("""") - Line.Text.IndexOf("""") - 1)
                                path = My.Application.Info.DirectoryPath & "\Include\" & file & If(file.IndexOf(".inc") = -1, ".inc", "")
                            Else
                                file = Mid(Line.Text, Line.Text.IndexOf("..") + 3, Line.Text.LastIndexOf("""") - Line.Text.IndexOf("""") - 1).Replace("/", "\")
                                path = Directory.GetParent(My.Application.Info.DirectoryPath).FullName & file & If(file.IndexOf(".inc") = -1, ".inc", "")
                            End If
                        End If
                        If IO.File.Exists(path) Then
                            Dim fLine As String, Reader As New StreamReader(path)
                            fLine = Reader.ReadLine()
                            Dim CommentedLine2 As Boolean, CommentedSection2 As Boolean
                            Do Until fLine Is Nothing
                                If fLine.Length = 0 OrElse fLine = "{" OrElse fLine = "}" OrElse fLine = ";" Then
                                    fLine = Reader.ReadLine()
                                    Continue Do
                                ElseIf fLine.StartsWith("//") Then
                                    CommentedLine2 = True
                                ElseIf fLine = "/*" OrElse fLine = " /*" Then
                                    CommentedSection2 = True
                                    fLine = Reader.ReadLine()
                                    Continue Do
                                ElseIf fLine = "*/" OrElse fLine = " */" Then
                                    CommentedSection2 = False
                                    fLine = Reader.ReadLine()
                                    Continue Do
                                ElseIf fLine.IndexOf("/*") > -1 AndAlso fLine.IndexOf("*/") = -1 Then
                                    CommentedSection2 = True
                                ElseIf fLine.IndexOf("*/") > -1 Then
                                    CommentedSection2 = False
                                End If
                                If CommentedLine2 Or CommentedSection2 Then
                                    CommentedLine2 = False
                                    fLine = Reader.ReadLine()
                                    Continue Do
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
                                    Dim file2 As String, path2 As String, cNode2 As New TreeNode()
                                    If fLine.IndexOf("<") > -1 Then
                                        file2 = Mid(fLine, fLine.IndexOf("<") + 2, fLine.IndexOf(">") - fLine.IndexOf("<") - 1)
                                        path2 = My.Application.Info.DirectoryPath & "\Include\" & file2 & ".inc"
                                    ElseIf fLine.IndexOf("""") > -1 Then
                                        If fLine.IndexOf("..") = -1 Then
                                            file2 = Mid(fLine, fLine.IndexOf("""") + 2, fLine.LastIndexOf("""") - fLine.IndexOf("""") - 1)
                                            path2 = My.Application.Info.DirectoryPath & "\Include\" & file2 & If(file2.IndexOf(".inc") = -1, ".inc", "")
                                        Else
                                            file2 = Mid(fLine, fLine.IndexOf("..") + 3, fLine.LastIndexOf("""") - fLine.IndexOf("""") - 1).Replace("/", "\")
                                            path2 = Directory.GetParent(My.Application.Info.DirectoryPath).FullName & file2 & If(file2.IndexOf(".inc") = -1, ".inc", "")
                                        End If
                                    Else
                                        fLine = Reader.ReadLine
                                        Continue Do
                                    End If
                                    If IO.File.Exists(path2) Then
                                        Dim Reader2 As New StreamReader(path2)
                                        fLine = Reader2.ReadLine()
                                        Dim CommentedLine3 As Boolean, CommentedSection3 As Boolean
                                        Do Until fLine Is Nothing
                                            If fLine.Length = 0 OrElse fLine = "{" OrElse fLine = "}" OrElse fLine = ";" Then
                                                fLine = Reader2.ReadLine()
                                                Continue Do
                                            ElseIf fLine.StartsWith("//") Then
                                                CommentedLine3 = True
                                            ElseIf fLine = "/*" OrElse fLine = " /*" Then
                                                CommentedSection3 = True
                                                fLine = Reader2.ReadLine()
                                                Continue Do
                                            ElseIf fLine = "*/" OrElse fLine = " */" Then
                                                CommentedSection3 = False
                                                fLine = Reader2.ReadLine()
                                                Continue Do
                                            ElseIf fLine.IndexOf("/*") > -1 AndAlso fLine.IndexOf("*/") = -1 Then
                                                CommentedSection3 = True
                                            ElseIf fLine.IndexOf("*/") > -1 Then
                                                CommentedSection3 = False
                                            End If
                                            If CommentedLine3 Or CommentedSection3 Then
                                                CommentedLine3 = False
                                                fLine = Reader2.ReadLine()
                                                Continue Do
                                            End If
                                            pos = fLine.IndexOf("native")
                                            If pos = -1 Then
                                                pos = fLine.IndexOf("stock")
                                                If pos = -1 Then pos = fLine.IndexOf("public")
                                            End If
                                            If pos > -1 AndAlso (fLine.EndsWith(";") AndAlso fLine.StartsWith("native ")) AndAlso fLine.IndexOf("(") > -1 AndAlso fLine.IndexOf(")") > -1 AndAlso fLine.IndexOf("operator") = -1 Then
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
                                            Else
                                                Dim tDef As String, name As String, params As String(), func As PawnFunction
                                                For Each def As CustomUserPublics In ACLists.UserDefinedPublics
                                                    Dim M As Match = Regex.Match(fLine, def.Regex)
                                                    If M.Success Then
                                                        tDef = def.Regex
                                                        tmp = M.Value.Remove(0, tDef.IndexOf(".+"))
                                                        tDef = tDef.Remove(0, tDef.IndexOf(".+"))
                                                        name = Regex.Match(tmp, Regex.Escape(Mid(tDef, 1, tDef.IndexOf(".+", 1))).Replace("\.", ".").Replace("\+", "+")).Value
                                                        tDef = tDef.Remove(0, 2)
                                                        tmp = tmp.Replace(name, "")
                                                        name = name.Remove(name.Length - 1, 1)
                                                        params = Regex.Split(Mid(tmp, 1, tmp.Length - 2), "[\s]?,[\s]?")
                                                        func = New PawnFunction(name, _Name.Replace(".inc", ":"), Line.Number, params)
                                                        If Not TrueContainsFunction(ACLists.Functions, func, True) Then ACLists.Functions.Add(func)
                                                    End If
                                                Next
                                            End If
                                            fLine = Reader2.ReadLine()
                                        Loop
                                        Reader2.Close()
                                    Else
                                        Errors.Clear()
                                        Errors.Add(New ListViewItem(New String() {"", "100", Name, Line.Number + 1, "cannot read from file: """ & file2 & """"}, 0))
                                    End If
                                ElseIf fLine.IndexOf("#tryinclude") > -1 Then
                                    Dim file2 As String, path2 As String
                                    If fLine.IndexOf("<") > -1 Then
                                        file2 = Mid(fLine, fLine.IndexOf("<") + 2, fLine.IndexOf(">") - fLine.IndexOf("<") - 1)
                                        path2 = My.Application.Info.DirectoryPath & "\Include\" & file2 & ".inc"
                                    Else
                                        If fLine.IndexOf("..") = -1 Then
                                            file2 = Mid(fLine, fLine.IndexOf("""") + 2, fLine.LastIndexOf("""") - fLine.IndexOf("""") - 1)
                                            path2 = My.Application.Info.DirectoryPath & "\Include\" & file2 & If(file2.IndexOf(".inc") = -1, ".inc", "")
                                        Else
                                            file2 = Mid(fLine, fLine.IndexOf("..") + 3, fLine.LastIndexOf("""") - fLine.IndexOf("""") - 1).Replace("/", "\")
                                            path2 = Directory.GetParent(My.Application.Info.DirectoryPath).FullName & file2 & If(file2.IndexOf(".inc") = -1, ".inc", "")
                                        End If
                                    End If
                                    If IO.File.Exists(path2) Then
                                        Dim Reader2 As New StreamReader(path2)
                                        fLine = Reader2.ReadLine()
                                        Dim CommentedLine3 As Boolean, CommentedSection3 As Boolean
                                        Do Until fLine Is Nothing
                                            If fLine.Length = 0 OrElse fLine = "{" OrElse fLine = "}" OrElse fLine = ";" Then
                                                fLine = Reader2.ReadLine()
                                                Continue Do
                                            ElseIf fLine.StartsWith("//") Then
                                                CommentedLine3 = True
                                            ElseIf fLine = "/*" OrElse fLine = " /*" Then
                                                CommentedSection3 = True
                                                fLine = Reader2.ReadLine()
                                                Continue Do
                                            ElseIf fLine = "*/" OrElse fLine = " */" Then
                                                CommentedSection3 = False
                                                fLine = Reader2.ReadLine()
                                                Continue Do
                                            ElseIf fLine.IndexOf("/*") > -1 AndAlso fLine.IndexOf("*/") = -1 Then
                                                CommentedSection3 = True
                                            ElseIf fLine.IndexOf("*/") > -1 Then
                                                CommentedSection3 = False
                                            End If
                                            If CommentedLine3 Or CommentedSection3 Then
                                                CommentedLine3 = False
                                                fLine = Reader2.ReadLine()
                                                Continue Do
                                            End If
                                            pos = fLine.IndexOf("native")
                                            If pos = -1 Then
                                                pos = fLine.IndexOf("stock")
                                                If pos = -1 Then pos = fLine.IndexOf("public")
                                            End If
                                            If pos > -1 AndAlso (fLine.EndsWith(";") AndAlso fLine.StartsWith("native ")) AndAlso fLine.IndexOf("(") > -1 AndAlso fLine.IndexOf(")") > -1 AndAlso fLine.IndexOf("operator") = -1 Then
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
                                            Else
                                                Dim tDef As String, name As String, params As String(), func As PawnFunction
                                                For Each def As CustomUserPublics In ACLists.UserDefinedPublics
                                                    Dim M As Match = Regex.Match(fLine, def.Regex)
                                                    If M.Success Then
                                                        tDef = def.Regex
                                                        tmp = M.Value.Remove(0, tDef.IndexOf(".+"))
                                                        tDef = tDef.Remove(0, tDef.IndexOf(".+"))
                                                        name = Regex.Match(tmp, Regex.Escape(Mid(tDef, 1, tDef.IndexOf(".+", 1))).Replace("\.", ".").Replace("\+", "+")).Value
                                                        tDef = tDef.Remove(0, 2)
                                                        tmp = tmp.Replace(name, "")
                                                        name = name.Remove(name.Length - 1, 1)
                                                        params = Regex.Split(Mid(tmp, 1, tmp.Length - 2), "[\s]?,[\s]?")
                                                        func = New PawnFunction(name, _Name.Replace(".inc", ":"), Line.Number, params)
                                                        If Not TrueContainsFunction(ACLists.Functions, func, True) Then ACLists.Functions.Add(func)
                                                    End If
                                                Next
                                            End If
                                            fLine = Reader2.ReadLine()
                                        Loop
                                        Reader2.Close()
                                    Else
                                        Errors.Clear()
                                        Errors.Add(New ListViewItem(New String() {"", "100", Name, Line.Number + 1, "cannot read from file: """ & file2 & """"}, 1))
                                    End If
                                ElseIf pos > -1 AndAlso (fLine.EndsWith(";") AndAlso fLine.StartsWith("native ")) AndAlso fLine.IndexOf("(") > -1 AndAlso fLine.IndexOf(")") > -1 AndAlso fLine.IndexOf("operator") = -1 Then
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
                                Else
                                    Dim tDef As String, name As String, params As String(), func As PawnFunction
                                    For Each def As CustomUserPublics In ACLists.UserDefinedPublics
                                        Dim M As Match = Regex.Match(fLine, def.Regex)
                                        If M.Success Then
                                            tDef = def.Regex
                                            tmp = M.Value.Remove(0, tDef.IndexOf(".+"))
                                            tDef = tDef.Remove(0, tDef.IndexOf(".+"))
                                            name = Regex.Match(tmp, Regex.Escape(Mid(tDef, 1, tDef.IndexOf(".+", 1))).Replace("\.", ".").Replace("\+", "+")).Value
                                            tDef = tDef.Remove(0, 2)
                                            tmp = tmp.Replace(name, "")
                                            name = name.Remove(name.Length - 1, 1)
                                            params = Regex.Split(Mid(tmp, 1, tmp.Length - 2), "[\s]?,[\s]?")
                                            func = New PawnFunction(name, _Name.Replace(".inc", ":"), Line.Number, params)
                                            If Not TrueContainsFunction(ACLists.Functions, func, True) Then ACLists.Functions.Add(func)
                                        End If
                                    Next
                                End If
                                fLine = Reader.ReadLine()
                            Loop
                            Reader.Close()
                        Else
                            Errors.Clear()
                            Errors.Add(New ListViewItem(New String() {"", "100", Name, Line.Number + 1, "cannot read from file: """ & file & """"}, 1))
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
                    If Line.Text.Length = 0 OrElse Line.Text = "{" OrElse Line.Text = "}" OrElse Line.Text = ";" Then
                        Continue For
                    ElseIf Line.Text.StartsWith("//") Then
                        CommentedLine = True
                    ElseIf Line.Text = "/*" OrElse Line.Text = " /*" Then
                        CommentedSection = True
                        Continue For
                    ElseIf Line.Text = "*/" OrElse Line.Text = " */" Then
                        CommentedSection = False
                        Continue For
                    ElseIf Line.Text.IndexOf("/*") > -1 AndAlso Line.Text.IndexOf("*/") = -1 Then
                        CommentedSection = True
                    ElseIf Line.Text.IndexOf("*/") > -1 Then
                        CommentedSection = False
                    End If
                    If CommentedLine Or CommentedSection Then
                        CommentedLine = False
                        Continue For
                    End If
                    Dim pos As Integer = Line.Text.IndexOf("native")
                    If pos = -1 Then
                        pos = Line.Text.IndexOf("stock")
                        If pos = -1 Then pos = Line.Text.IndexOf("public")
                    End If
                    If pos > -1 AndAlso (Line.Text.EndsWith(";") AndAlso Line.Text.StartsWith("native ")) AndAlso Line.Text.IndexOf("(") > -1 AndAlso Line.Text.IndexOf(")") > -1 AndAlso Line.Text.IndexOf("operator") = -1 Then
                        Dim params As New List(Of String)
                        params.AddRange(Split(Trim(Mid(Line.Text, Line.Text.IndexOf("(") + 2, Line.Text.IndexOf(")") - Line.Text.IndexOf("(") - 1)), ","))
                        For i = 0 To params.Count - 1
                            If i > 0 AndAlso params(i).Length > 0 AndAlso params(i).IndexOf("...") > -1 Then
                                params(i - 1) += "," & params(i)
                                params.RemoveAt(i)
                                Continue For
                            End If
                        Next
                        Dim func As PawnFunction = New PawnFunction(Trim(Mid(Line.Text, Line.Text.IndexOf(" ", pos) + 2, Line.Text.IndexOf("(") - Line.Text.IndexOf(" ", pos) - 1)), _Name.Replace(".inc", ":"), -1, params.ToArray)
                        If Not TrueContainsFunction(ACLists.Functions, func, True) AndAlso Not TrueContainsFunction(ACLists.Callbacks, func) Then ACLists.Functions.Add(func)
                    ElseIf Line.Text.IndexOf("forward") > -1 AndAlso Line.Text.IndexOf("(") > -1 AndAlso Line.Text.IndexOf(")") > -1 Then
                        Dim func As PawnFunction = New PawnFunction(Trim(Mid(Line.Text, Line.Text.IndexOf(" ") + 1, Line.Text.IndexOf("(") - Line.Text.IndexOf(" "))), _Name.Replace(".inc", ":"), -1, Split(Trim(Mid(Line.Text, Line.Text.IndexOf("(") + 2, Line.Text.IndexOf(")") - Line.Text.IndexOf("(") - 1)), ","))
                        If Not TrueContainsFunction(ACLists.Callbacks, func, True) Then ACLists.Callbacks.Add(func)
                    Else
                        Dim tDef As String, name As String, params As String(), func As PawnFunction
                        For Each def As CustomUserPublics In ACLists.UserDefinedPublics
                            Dim M As Match = Regex.Match(Line.Text, def.Regex)
                            If M.Success Then
                                tDef = def.Regex
                                tmp = M.Value.Remove(0, tDef.IndexOf(".+"))
                                tDef = tDef.Remove(0, tDef.IndexOf(".+"))
                                name = Regex.Match(tmp, Regex.Escape(Mid(tDef, 1, tDef.IndexOf(".+", 1))).Replace("\.", ".").Replace("\+", "+")).Value
                                tDef = tDef.Remove(0, 2)
                                tmp = tmp.Replace(name, "")
                                name = name.Remove(name.Length - 1, 1)
                                params = Regex.Split(Mid(tmp, 1, tmp.Length - 2), "[\s]?,[\s]?")
                                func = New PawnFunction(name, _Name.Replace(".inc", ":"), Line.Number, params)
                                If Not TrueContainsFunction(ACLists.Functions, func, True) Then ACLists.Functions.Add(func)
                            End If
                        Next
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
                    If Line.Text.Length = 0 OrElse Line.Text = "{" OrElse Line.Text = "}" OrElse Line.Text = ";" Then
                        Continue For
                    ElseIf Line.Text.StartsWith("//") Then
                        CommentedLine = True
                    ElseIf Line.Text = "/*" OrElse Line.Text = " /*" Then
                        CommentedSection = True
                        Continue For
                    ElseIf Line.Text = "*/" OrElse Line.Text = " */" Then
                        CommentedSection = False
                        Continue For
                    ElseIf Line.Text.IndexOf("/*") > -1 AndAlso Line.Text.IndexOf("*/") = -1 Then
                        CommentedSection = True
                    ElseIf Line.Text.IndexOf("*/") > -1 Then
                        CommentedSection = False
                    End If
                    If CommentedLine Or CommentedSection Then
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
                    If Line.Text.Length = 0 OrElse Line.Text = "{" OrElse Line.Text = "}" OrElse Line.Text = ";" Then
                        Continue For
                    ElseIf Line.Text.StartsWith("//") Then
                        CommentedLine = True
                    ElseIf Line.Text = "/*" OrElse Line.Text = " /*" Then
                        CommentedSection = True
                        Continue For
                    ElseIf Line.Text = "*/" OrElse Line.Text = " */" Then
                        CommentedSection = False
                        Continue For
                    ElseIf Line.Text.IndexOf("/*") > -1 AndAlso Line.Text.IndexOf("*/") = -1 Then
                        CommentedSection = True
                    ElseIf Line.Text.IndexOf("*/") > -1 Then
                        CommentedSection = False
                    End If
                    If CommentedLine Or CommentedSection Then
                        CommentedLine = False
                        Continue For
                    End If
                    If Line.Text.IndexOf("Menu:") > -1 AndAlso Line.Text.IndexOf("(") = -1 AndAlso Line.Text.IndexOf(")") = -1 Then
                        Dim value As String, Ms As MatchCollection = Regex.Matches(Line.Text, "Menu:[^,;\s]+")
                        For Each M As Match In Ms
                            value = Regex.Replace(M.Value, "(Menu:|[,;\s])", "")
                            If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Menus.Contains(value) Then ACLists.Menus.Add(value)
                        Next
                    ElseIf Line.Text.IndexOf("Text:") > -1 AndAlso Line.Text.IndexOf("(") = -1 AndAlso Line.Text.IndexOf(")") = -1 Then
                        Dim value As String, Ms As MatchCollection = Regex.Matches(Line.Text, "Text:[^,;\s]+")
                        For Each M As Match In Ms
                            value = Regex.Replace(M.Value, "(Text:|[,;\s])", "")
                            If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Texts.Contains(value) Then ACLists.Texts.Add(value)
                        Next
                    ElseIf Line.Text.IndexOf("Text3D:") > -1 AndAlso Line.Text.IndexOf("(") = -1 AndAlso Line.Text.IndexOf(")") = -1 Then
                        Dim value As String, Ms As MatchCollection = Regex.Matches(Line.Text, "Text3D:[^,;\s]+")
                        For Each M As Match In Ms
                            value = Regex.Replace(M.Value, "(Text3D:|[,;\s])", "")
                            If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Texts2.Contains(value) Then ACLists.Texts2.Add(value)
                        Next
                    ElseIf Line.Text.IndexOf("Float:") > -1 AndAlso Line.Text.IndexOf("cellmin") = -1 AndAlso Line.Text.IndexOf("(") = -1 AndAlso Line.Text.IndexOf(")") = -1 Then
                        Dim value As String, Ms As MatchCollection = Regex.Matches(Line.Text, "Float:[^,;\s]+")
                        For Each M As Match In Ms
                            value = Regex.Replace(M.Value, "(Float:|[,;\s])", "")
                            If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Floats.Contains(value) Then ACLists.Floats.Add(value)
                        Next
                    ElseIf Line.Text.IndexOf("DB:") > -1 AndAlso Line.Text.IndexOf("(") = -1 AndAlso Line.Text.IndexOf(")") = -1 Then
                        Dim value As String, Ms As MatchCollection = Regex.Matches(Line.Text, "DB:[^,;\s]+")
                        For Each M As Match In Ms
                            value = Regex.Replace(M.Value, "(DB:|[,;\s])", "")
                            If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Dbs.Contains(value) Then ACLists.Dbs.Add(value)
                        Next
                    ElseIf Line.Text.IndexOf("DBResult:") > -1 AndAlso Line.Text.IndexOf("(") = -1 AndAlso Line.Text.IndexOf(")") = -1 Then
                        Dim value As String, Ms As MatchCollection = Regex.Matches(Line.Text, "DBResult:[^,;\s]+")
                        For Each M As Match In Ms
                            value = Regex.Replace(M.Value, "(DBResult:|[,;\s])", "")
                            If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.DbRes.Contains(value) Then ACLists.DbRes.Add(value)
                        Next
                    ElseIf Line.Text.IndexOf("File:") > -1 AndAlso Line.Text.IndexOf("(") = -1 AndAlso Line.Text.IndexOf(")") = -1 Then
                        Dim value As String, Ms As MatchCollection = Regex.Matches(Line.Text, "File:[^,;\s]+")
                        For Each M As Match In Ms
                            value = Regex.Replace(M.Value, "(File:|[,;\s])", "")
                            If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Files.Contains(value) Then ACLists.Files.Add(value)
                        Next
                    End If
                Next
        End Select
        LastUpdate = GetTickCount()
        Dim tmpstring As String = vbNullString
        For Each item As PawnFunction In ACLists.Functions
            tmpstring += item.Name & " "
        Next
        SyntaxHandle.Lexing.Keywords(3) = tmpstring
        Colorize()
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
        Errors.Clear()
        ParseCode()
        For Each line As ScintillaNet.Line In If(SyntaxHandle.InvokeRequired, SyntaxHandle.Invoke(LinesDelegate, New Object() {SyntaxHandle}), SyntaxHandle.Lines)
            If line.Text.Length = 0 OrElse line.Text = "{" OrElse line.Text = "}" OrElse line.Text = ";" Then
                Continue For
            ElseIf line.Text.StartsWith("//") Then
                CommentedLine = True
            ElseIf line.Text = "/*" Then
                CommentedSection = True
                Continue For
            ElseIf line.Text = "*/" Then
                CommentedSection = False
                Continue For
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
                Dim file As String, path As String
                If line.Text.IndexOf("<") > -1 Then
                    file = Mid(line.Text, line.Text.IndexOf("<") + 2, line.Text.IndexOf(">") - line.Text.IndexOf("<") - 1)
                    path = My.Application.Info.DirectoryPath & "\Include\" & file & ".inc"
                ElseIf line.Text.IndexOf("""") > -1 Then
                    If line.Text.IndexOf("..") = -1 Then
                        file = Mid(line.Text, line.Text.IndexOf("""") + 2, line.Text.LastIndexOf("""") - line.Text.IndexOf("""") - 1)
                        path = My.Application.Info.DirectoryPath & "\Include\" & file & If(file.IndexOf(".inc") = -1, ".inc", "")
                    Else
                        file = Mid(line.Text, line.Text.IndexOf("..") + 3, line.Text.LastIndexOf("""") - line.Text.IndexOf("""") - 1).Replace("/", "\")
                        path = Directory.GetParent(My.Application.Info.DirectoryPath).FullName & file & If(file.IndexOf(".inc") = -1, ".inc", "")
                    End If
                Else
                    Continue For
                End If
                If IO.File.Exists(path) Then
                    Dim fLine As String, Reader As New StreamReader(path)
                    fLine = Reader.ReadLine()
                    Dim CommentedLine2 As Boolean, CommentedSection2 As Boolean
                    Do Until fLine Is Nothing
                        If fLine.Length = 0 OrElse fLine = "{" OrElse fLine = "}" OrElse fLine = ";" Then
                            fLine = Reader.ReadLine()
                            Continue Do
                        ElseIf fLine.StartsWith("//") Then
                            CommentedLine2 = True
                        ElseIf fLine = "/*" OrElse fLine = " /*" Then
                            CommentedSection2 = True
                            fLine = Reader.ReadLine()
                            Continue Do
                        ElseIf fLine = "*/" OrElse fLine = " */" Then
                            CommentedSection2 = False
                            fLine = Reader.ReadLine()
                            Continue Do
                        ElseIf fLine.IndexOf("/*") > -1 AndAlso fLine.IndexOf("*/") = -1 Then
                            CommentedSection2 = True
                        ElseIf fLine.IndexOf("*/") > -1 Then
                            CommentedSection2 = False
                        End If
                        If CommentedLine2 Or CommentedSection2 Then
                            CommentedLine2 = False
                            fLine = Reader.ReadLine()
                            Continue Do
                        End If
                        spos = fLine.IndexOf("native")
                        If spos = -1 Then
                            spos = fLine.IndexOf("stock")
                            If spos = -1 Then spos = fLine.IndexOf("public")
                        End If
                        If fLine.IndexOf("#include") > -1 Then
                            Dim file2 As String, path2 As String, cNode2 As New TreeNode()
                            If fLine.IndexOf("<") > -1 Then
                                file2 = Mid(fLine, fLine.IndexOf("<") + 2, fLine.IndexOf(">") - fLine.IndexOf("<") - 1)
                                path2 = My.Application.Info.DirectoryPath & "\Include\" & file2 & ".inc"
                            ElseIf fLine.IndexOf("""") > -1 Then
                                If fLine.IndexOf("..") = -1 Then
                                    file2 = Mid(fLine, fLine.IndexOf("""") + 2, fLine.LastIndexOf("""") - fLine.IndexOf("""") - 1)
                                    path2 = My.Application.Info.DirectoryPath & "\Include\" & file2 & If(file2.IndexOf(".inc") = -1, ".inc", "")
                                Else
                                    file2 = Mid(fLine, fLine.IndexOf("..") + 3, fLine.LastIndexOf("""") - fLine.IndexOf("""") - 1).Replace("/", "\")
                                    path2 = Directory.GetParent(My.Application.Info.DirectoryPath).FullName & file2 & If(file2.IndexOf(".inc") = -1, ".inc", "")
                                End If
                            Else
                                fLine = Reader.ReadLine()
                                Continue Do
                            End If
                            Dim count As Integer
                            If IO.File.Exists(path2) Then
                                Dim Reader2 As New StreamReader(path2)
                                fLine = Reader2.ReadLine()
                                Dim CommentedLine3 As Boolean, CommentedSection3 As Boolean
                                Do Until fLine Is Nothing
                                    If fLine.Length = 0 OrElse fLine = "{" OrElse fLine = "}" OrElse fLine = ";" Then
                                        count += 1
                                        fLine = Reader2.ReadLine()
                                        Continue Do
                                    ElseIf fLine.StartsWith("//") Then
                                        CommentedLine3 = True
                                    ElseIf fLine = "/*" OrElse fLine = " /*" Then
                                        CommentedSection3 = True
                                        count += 1
                                        fLine = Reader2.ReadLine()
                                        Continue Do
                                    ElseIf fLine = "*/" OrElse fLine = " */" Then
                                        CommentedSection3 = False
                                        count += 1
                                        fLine = Reader2.ReadLine()
                                        Continue Do
                                    ElseIf fLine.IndexOf("/*") > -1 AndAlso fLine.IndexOf("*/") = -1 Then
                                        CommentedSection3 = True
                                    ElseIf fLine.IndexOf("*/") > -1 Then
                                        CommentedSection3 = False
                                    End If
                                    If CommentedLine3 Or CommentedSection3 Then
                                        CommentedLine3 = False
                                        count += 1
                                        fLine = Reader2.ReadLine()
                                        Continue Do
                                    End If
                                    spos = fLine.IndexOf("native")
                                    If spos = -1 Then
                                        spos = fLine.IndexOf("stock")
                                        If spos = -1 Then
                                            spos = fLine.IndexOf("public")
                                        End If
                                    End If
                                    If spos > -1 AndAlso (fLine.EndsWith(";") AndAlso fLine.StartsWith("native ")) AndAlso fLine.IndexOf("(") > -1 AndAlso fLine.IndexOf(")") > -1 AndAlso fLine.IndexOf("operator") = -1 Then
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
                                        Dim value As String, Ms As MatchCollection = Regex.Matches(fLine, "Menu:[^,;\s]+")
                                        For Each M As Match In Ms
                                            value = Regex.Replace(M.Value, "(Menu:|[,;\s])", "")
                                            If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Menus.Contains(value) Then ACLists.Menus.Add(value)
                                        Next
                                    ElseIf fLine.IndexOf("Text:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                                        Dim value As String, Ms As MatchCollection = Regex.Matches(fLine, "Text:[^,;\s]+")
                                        For Each M As Match In Ms
                                            value = Regex.Replace(M.Value, "(Text:|[,;\s])", "")
                                            If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Texts.Contains(value) Then ACLists.Texts.Add(value)
                                        Next
                                    ElseIf fLine.IndexOf("Text3D:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                                        Dim value As String, Ms As MatchCollection = Regex.Matches(fLine, "Text3D:[^,;\s]+")
                                        For Each M As Match In Ms
                                            value = Regex.Replace(M.Value, "(Text3D:|[,;\s])", "")
                                            If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Texts2.Contains(value) Then ACLists.Texts2.Add(value)
                                        Next
                                    ElseIf fLine.IndexOf("Float:") > -1 AndAlso fLine.IndexOf("cellmin") = -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                                        Dim value As String, Ms As MatchCollection = Regex.Matches(fLine, "Float:[^,;\s]+")
                                        For Each M As Match In Ms
                                            value = Regex.Replace(M.Value, "(Float:|[,;\s])", "")
                                            If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Floats.Contains(value) Then ACLists.Floats.Add(value)
                                        Next
                                    ElseIf fLine.IndexOf("DB:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                                        Dim value As String, Ms As MatchCollection = Regex.Matches(fLine, "DB:[^,;\s]+")
                                        For Each M As Match In Ms
                                            value = Regex.Replace(M.Value, "(DB:|[,;\s])", "")
                                            If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Dbs.Contains(value) Then ACLists.Dbs.Add(value)
                                        Next
                                    ElseIf fLine.IndexOf("DBResult:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                                        Dim value As String, Ms As MatchCollection = Regex.Matches(fLine, "DBResult:[^,;\s]+")
                                        For Each M As Match In Ms
                                            value = Regex.Replace(M.Value, "(DBResult:|[,;\s])", "")
                                            If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.DbRes.Contains(value) Then ACLists.DbRes.Add(value)
                                        Next
                                    ElseIf fLine.IndexOf("File:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                                        Dim value As String, Ms As MatchCollection = Regex.Matches(fLine, "File:[^,;\s]+")
                                        For Each M As Match In Ms
                                            value = Regex.Replace(M.Value, "(File:|[,;\s])", "")
                                            If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Files.Contains(value) Then ACLists.Files.Add(value)
                                        Next
                                    Else
                                        Dim tDef As String, name As String, params As String(), func As PawnFunction
                                        For Each def As CustomUserPublics In ACLists.UserDefinedPublics
                                            Dim M As Match = Regex.Match(fLine, def.Regex)
                                            If M.Success Then
                                                tDef = def.Regex
                                                tmp = M.Value.Remove(0, tDef.IndexOf(".+"))
                                                tDef = tDef.Remove(0, tDef.IndexOf(".+"))
                                                name = Regex.Match(tmp, Regex.Escape(Mid(tDef, 1, tDef.IndexOf(".+", 1))).Replace("\.", ".").Replace("\+", "+")).Value
                                                tDef = tDef.Remove(0, 2)
                                                tmp = tmp.Replace(name, "")
                                                name = name.Remove(name.Length - 1, 1)
                                                params = Regex.Split(Mid(tmp, 1, tmp.Length - 2), "[\s]?,[\s]?")
                                                func = New PawnFunction(name, _Name.Replace(".inc", ":"), line.Number, params)
                                                If Not TrueContainsFunction(ACLists.Functions, func) Then ACLists.Functions.Add(func)
                                            End If
                                        Next
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
                            Dim file2 As String, path2 As String
                            If fLine.IndexOf("<") > -1 Then
                                file2 = Mid(fLine, fLine.IndexOf("<") + 2, fLine.IndexOf(">") - fLine.IndexOf("<") - 1)
                                path2 = My.Application.Info.DirectoryPath & "\Include\" & file2 & ".inc"
                            Else
                                If fLine.IndexOf("..") = -1 Then
                                    file2 = Mid(fLine, fLine.IndexOf("""") + 2, fLine.LastIndexOf("""") - fLine.IndexOf("""") - 1)
                                    path2 = My.Application.Info.DirectoryPath & "\Include\" & file2 & If(file2.IndexOf(".inc") = -1, ".inc", "")
                                Else
                                    file2 = Mid(fLine, fLine.IndexOf("..") + 3, fLine.LastIndexOf("""") - fLine.IndexOf("""") - 1).Replace("/", "\")
                                    path2 = Directory.GetParent(My.Application.Info.DirectoryPath).FullName & file2 & If(file2.IndexOf(".inc") = -1, ".inc", "")
                                End If
                            End If
                            Dim count As Integer
                            If IO.File.Exists(path2) Then
                                Dim Reader2 As New StreamReader(path2)
                                fLine = Reader2.ReadLine()
                                Dim CommentedLine3 As Boolean, CommentedSection3 As Boolean
                                Do Until fLine Is Nothing
                                    If fLine.Length = 0 OrElse fLine = "{" OrElse fLine = "}" OrElse fLine = ";" Then
                                        count += 1
                                        fLine = Reader2.ReadLine()
                                        Continue Do
                                    ElseIf fLine.StartsWith("//") Then
                                        CommentedLine3 = True
                                    ElseIf fLine = "/*" OrElse fLine = " /*" Then
                                        CommentedSection3 = True
                                        count += 1
                                        fLine = Reader2.ReadLine()
                                        Continue Do
                                    ElseIf fLine = "*/" OrElse fLine = " */" Then
                                        CommentedSection3 = False
                                        count += 1
                                        fLine = Reader2.ReadLine()
                                        Continue Do
                                    ElseIf fLine.IndexOf("/*") > -1 AndAlso fLine.IndexOf("*/") = -1 Then
                                        CommentedSection3 = True
                                    ElseIf fLine.IndexOf("*/") > -1 Then
                                        CommentedSection3 = False
                                    End If
                                    If CommentedLine3 Or CommentedSection3 Then
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
                                    If spos > -1 AndAlso (fLine.EndsWith(";") AndAlso fLine.StartsWith("native ")) AndAlso fLine.IndexOf("(") > -1 AndAlso fLine.IndexOf(")") > -1 AndAlso fLine.IndexOf("operator") = -1 Then
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
                                        Dim value As String, Ms As MatchCollection = Regex.Matches(fLine, "Menu:[^,;\s]+")
                                        For Each M As Match In Ms
                                            value = Regex.Replace(M.Value, "(Menu:|[,;\s])", "")
                                            If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Menus.Contains(value) Then ACLists.Menus.Add(value)
                                        Next
                                    ElseIf fLine.IndexOf("Text:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                                        Dim value As String, Ms As MatchCollection = Regex.Matches(fLine, "Text:[^,;\s]+")
                                        For Each M As Match In Ms
                                            value = Regex.Replace(M.Value, "(Text:|[,;\s])", "")
                                            If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Texts.Contains(value) Then ACLists.Texts.Add(value)
                                        Next
                                    ElseIf fLine.IndexOf("Text3D:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                                        Dim value As String, Ms As MatchCollection = Regex.Matches(fLine, "Text3D:[^,;\s]+")
                                        For Each M As Match In Ms
                                            value = Regex.Replace(M.Value, "(Text3D:|[,;\s])", "")
                                            If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Texts2.Contains(value) Then ACLists.Texts2.Add(value)
                                        Next
                                    ElseIf fLine.IndexOf("Float:") > -1 AndAlso fLine.IndexOf("cellmin") = -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                                        Dim value As String, Ms As MatchCollection = Regex.Matches(fLine, "Float:[^,;\s]+")
                                        For Each M As Match In Ms
                                            value = Regex.Replace(M.Value, "(Float:|[,;\s])", "")
                                            If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Floats.Contains(value) Then ACLists.Floats.Add(value)
                                        Next
                                    ElseIf fLine.IndexOf("DB:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                                        Dim value As String, Ms As MatchCollection = Regex.Matches(fLine, "DB:[^,;\s]+")
                                        For Each M As Match In Ms
                                            value = Regex.Replace(M.Value, "(DB:|[,;\s])", "")
                                            If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Dbs.Contains(value) Then ACLists.Dbs.Add(value)
                                        Next
                                    ElseIf fLine.IndexOf("DBResult:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                                        Dim value As String, Ms As MatchCollection = Regex.Matches(fLine, "DBResult:[^,;\s]+")
                                        For Each M As Match In Ms
                                            value = Regex.Replace(M.Value, "(DBResult:|[,;\s])", "")
                                            If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.DbRes.Contains(value) Then ACLists.DbRes.Add(value)
                                        Next
                                    ElseIf fLine.IndexOf("File:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                                        Dim value As String, Ms As MatchCollection = Regex.Matches(fLine, "File:[^,;\s]+")
                                        For Each M As Match In Ms
                                            value = Regex.Replace(M.Value, "(File:|[,;\s])", "")
                                            If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Files.Contains(value) Then ACLists.Files.Add(value)
                                        Next
                                    Else
                                        Dim tDef As String, name As String, params As String(), func As PawnFunction
                                        For Each def As CustomUserPublics In ACLists.UserDefinedPublics
                                            Dim M As Match = Regex.Match(fLine, def.Regex)
                                            If M.Success Then
                                                tDef = def.Regex
                                                tmp = M.Value.Remove(0, tDef.IndexOf(".+"))
                                                tDef = tDef.Remove(0, tDef.IndexOf(".+"))
                                                name = Regex.Match(tmp, Regex.Escape(Mid(tDef, 1, tDef.IndexOf(".+", 1))).Replace("\.", ".").Replace("\+", "+")).Value
                                                tDef = tDef.Remove(0, 2)
                                                tmp = tmp.Replace(name, "")
                                                name = name.Remove(name.Length - 1, 1)
                                                params = Regex.Split(Mid(tmp, 1, tmp.Length - 2), "[\s]?,[\s]?")
                                                func = New PawnFunction(name, _Name.Replace(".inc", ":"), line.Number, params)
                                                If Not TrueContainsFunction(ACLists.Functions, func) Then ACLists.Functions.Add(func)
                                            End If
                                        Next
                                    End If
                                    count += 1
                                    fLine = Reader2.ReadLine()
                                Loop
                                Reader2.Close()
                            Else
                                Errors.Clear()
                                Errors.Add(New ListViewItem(New String() {"", "100", Name, count, "cannot read from file: """ & file2 & """"}, 1))
                            End If
                        ElseIf spos > -1 AndAlso (fLine.EndsWith(";") AndAlso fLine.StartsWith("native ")) AndAlso fLine.IndexOf("(") > -1 AndAlso fLine.IndexOf(")") > -1 AndAlso fLine.IndexOf("operator") = -1 Then
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
                            Dim value As String, Ms As MatchCollection = Regex.Matches(fLine, "Menu:[^,;\s]+")
                            For Each M As Match In Ms
                                value = Regex.Replace(M.Value, "(Menu:|[,;\s])", "")
                                If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Menus.Contains(value) Then ACLists.Menus.Add(value)
                            Next
                        ElseIf fLine.IndexOf("Text:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                            Dim value As String, Ms As MatchCollection = Regex.Matches(fLine, "Text:[^,;\s]+")
                            For Each M As Match In Ms
                                value = Regex.Replace(M.Value, "(Text:|[,;\s])", "")
                                If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Texts.Contains(value) Then ACLists.Texts.Add(value)
                            Next
                        ElseIf fLine.IndexOf("Text3D:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                            Dim value As String, Ms As MatchCollection = Regex.Matches(fLine, "Text3D:[^,;\s]+")
                            For Each M As Match In Ms
                                value = Regex.Replace(M.Value, "(Text3D:|[,;\s])", "")
                                If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Texts2.Contains(value) Then ACLists.Texts2.Add(value)
                            Next
                        ElseIf fLine.IndexOf("Float:") > -1 AndAlso fLine.IndexOf("cellmin") = -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                            Dim value As String, Ms As MatchCollection = Regex.Matches(fLine, "Float:[^,;\s]+")
                            For Each M As Match In Ms
                                value = Regex.Replace(M.Value, "(Float:|[,;\s])", "")
                                If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Floats.Contains(value) Then ACLists.Floats.Add(value)
                            Next
                        ElseIf fLine.IndexOf("DB:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                            Dim value As String, Ms As MatchCollection = Regex.Matches(fLine, "DB:[^,;\s]+")
                            For Each M As Match In Ms
                                value = Regex.Replace(M.Value, "(DB:|[,;\s])", "")
                                If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Dbs.Contains(value) Then ACLists.Dbs.Add(value)
                            Next
                        ElseIf fLine.IndexOf("DBResult:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                            Dim value As String, Ms As MatchCollection = Regex.Matches(fLine, "DBResult:[^,;\s]+")
                            For Each M As Match In Ms
                                value = Regex.Replace(M.Value, "(DBResult:|[,;\s])", "")
                                If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.DbRes.Contains(value) Then ACLists.DbRes.Add(value)
                            Next
                        ElseIf fLine.IndexOf("File:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                            Dim value As String, Ms As MatchCollection = Regex.Matches(fLine, "File:[^,;\s]+")
                            For Each M As Match In Ms
                                value = Regex.Replace(M.Value, "(File:|[,;\s])", "")
                                If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Files.Contains(value) Then ACLists.Files.Add(value)
                            Next
                        Else
                            Dim tDef As String, name As String, params As String(), func As PawnFunction
                            For Each def As CustomUserPublics In ACLists.UserDefinedPublics
                                Dim M As Match = Regex.Match(fLine, def.Regex)
                                If M.Success Then
                                    tDef = def.Regex
                                    tmp = M.Value.Remove(0, tDef.IndexOf(".+"))
                                    tDef = tDef.Remove(0, tDef.IndexOf(".+"))
                                    name = Regex.Match(tmp, Regex.Escape(Mid(tDef, 1, tDef.IndexOf(".+", 1))).Replace("\.", ".").Replace("\+", "+")).Value
                                    tDef = tDef.Remove(0, 2)
                                    tmp = tmp.Replace(name, "")
                                    name = name.Remove(name.Length - 1, 1)
                                    params = Regex.Split(Mid(tmp, 1, tmp.Length - 2), "[\s]?,[\s]?")
                                    func = New PawnFunction(name, _Name.Replace(".inc", ":"), line.Number, params)
                                    If Not TrueContainsFunction(ACLists.Functions, func) Then ACLists.Functions.Add(func)
                                End If
                            Next
                        End If
                        fLine = Reader.ReadLine()
                    Loop
                    Reader.Close()
                Else
                    Errors.Clear()
                    Errors.Add(New ListViewItem(New String() {"", "100", Name, line.Number + 1, "cannot read from file: """ & file & """"}, 0))
                End If
            ElseIf line.Text.IndexOf("#tryinclude") > -1 Then
                Dim file As String, path As String
                If line.Text.IndexOf("<") > -1 Then
                    file = Mid(line.Text, line.Text.IndexOf("<") + 2, line.Text.IndexOf(">") - line.Text.IndexOf("<") - 1)
                    path = My.Application.Info.DirectoryPath & "\Include\" & file & ".inc"
                Else
                    If line.Text.IndexOf("..") = -1 Then
                        file = Mid(line.Text, line.Text.IndexOf("""") + 2, line.Text.LastIndexOf("""") - line.Text.IndexOf("""") - 1)
                        path = My.Application.Info.DirectoryPath & "\Include\" & file & If(file.IndexOf(".inc") = -1, ".inc", "")
                    Else
                        file = Mid(line.Text, line.Text.IndexOf("..") + 3, line.Text.LastIndexOf("""") - line.Text.IndexOf("""") - 1).Replace("/", "\")
                        path = Directory.GetParent(My.Application.Info.DirectoryPath).FullName & file & If(file.IndexOf(".inc") = -1, ".inc", "")
                    End If
                End If
                If IO.File.Exists(path) Then
                    Dim fLine As String, Reader As New StreamReader(path)
                    fLine = Reader.ReadLine()
                    Dim CommentedLine2 As Boolean, CommentedSection2 As Boolean
                    Do Until fLine Is Nothing
                        If fLine.Length = 0 OrElse fLine = "{" OrElse fLine = "}" OrElse fLine = ";" Then
                            fLine = Reader.ReadLine()
                            Continue Do
                        ElseIf fLine.StartsWith("//") Then
                            CommentedLine2 = True
                        ElseIf fLine = "/*" OrElse fLine = " /*" Then
                            CommentedSection2 = True
                            fLine = Reader.ReadLine()
                            Continue Do
                        ElseIf fLine = "*/" OrElse fLine = " */" Then
                            CommentedSection2 = False
                            fLine = Reader.ReadLine()
                            Continue Do
                        ElseIf fLine.IndexOf("/*") > -1 AndAlso fLine.IndexOf("*/") = -1 Then
                            CommentedSection2 = True
                        ElseIf fLine.IndexOf("*/") > -1 Then
                            CommentedSection2 = False
                        End If
                        If CommentedLine2 Or CommentedSection2 Then
                            CommentedLine2 = False
                            fLine = Reader.ReadLine()
                            Continue Do
                        End If
                        spos = fLine.IndexOf("native")
                        If spos = -1 Then
                            spos = fLine.IndexOf("stock")
                            If spos = -1 Then spos = fLine.IndexOf("public")
                        End If
                        If fLine.IndexOf("#include") > -1 Then
                            Dim file2 As String, path2 As String, cNode2 As New TreeNode()
                            If fLine.IndexOf("<") > -1 Then
                                file2 = Mid(fLine, fLine.IndexOf("<") + 2, fLine.IndexOf(">") - fLine.IndexOf("<") - 1)
                                path2 = My.Application.Info.DirectoryPath & "\Include\" & file2 & ".inc"
                            ElseIf fLine.IndexOf("""") > -1 Then
                                If fLine.IndexOf("..") = -1 Then
                                    file2 = Mid(fLine, fLine.IndexOf("""") + 2, fLine.LastIndexOf("""") - fLine.IndexOf("""") - 1)
                                    path2 = My.Application.Info.DirectoryPath & "\Include\" & file2 & If(file2.IndexOf(".inc") = -1, ".inc", "")
                                Else
                                    file2 = Mid(fLine, fLine.IndexOf("..") + 3, fLine.LastIndexOf("""") - fLine.IndexOf("""") - 1).Replace("/", "\")
                                    path2 = Directory.GetParent(My.Application.Info.DirectoryPath).FullName & file2 & If(file2.IndexOf(".inc") = -1, ".inc", "")
                                End If
                            Else
                                fLine = Reader.ReadLine()
                                Continue Do
                            End If
                            Dim count As Integer
                            If IO.File.Exists(path2) Then
                                Dim Reader2 As New StreamReader(path2)
                                fLine = Reader2.ReadLine()
                                Dim CommentedLine3 As Boolean, CommentedSection3 As Boolean
                                Do Until fLine Is Nothing
                                    If fLine.Length = 0 OrElse fLine = "{" OrElse fLine = "}" OrElse fLine = ";" Then
                                        count += 1
                                        fLine = Reader2.ReadLine()
                                        Continue Do
                                    ElseIf fLine.StartsWith("//") Then
                                        CommentedLine3 = True
                                    ElseIf fLine = "/*" OrElse fLine = " /*" Then
                                        CommentedSection3 = True
                                        count += 1
                                        fLine = Reader2.ReadLine()
                                        Continue Do
                                    ElseIf fLine = "*/" OrElse fLine = " */" Then
                                        CommentedSection3 = False
                                        count += 1
                                        fLine = Reader2.ReadLine()
                                        Continue Do
                                    ElseIf fLine.IndexOf("/*") > -1 AndAlso fLine.IndexOf("*/") = -1 Then
                                        CommentedSection3 = True
                                    ElseIf fLine.IndexOf("*/") > -1 Then
                                        CommentedSection3 = False
                                    End If
                                    If CommentedLine3 Or CommentedSection3 Then
                                        CommentedLine3 = False
                                        count += 1
                                        fLine = Reader2.ReadLine()
                                        Continue Do
                                    End If
                                    spos = fLine.IndexOf("native")
                                    If spos = -1 Then
                                        spos = fLine.IndexOf("stock")
                                        If spos = -1 Then spos = fLine.IndexOf("public")
                                    End If
                                    If spos > -1 AndAlso (fLine.EndsWith(";") AndAlso fLine.StartsWith("native ")) AndAlso fLine.IndexOf("(") > -1 AndAlso fLine.IndexOf(")") > -1 AndAlso fLine.IndexOf("operator") = -1 Then
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
                                        Dim value As String, Ms As MatchCollection = Regex.Matches(fLine, "Menu:[^,;\s]+")
                                        For Each M As Match In Ms
                                            value = Regex.Replace(M.Value, "(Menu:|[,;\s])", "")
                                            If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Menus.Contains(value) Then ACLists.Menus.Add(value)
                                        Next
                                    ElseIf fLine.IndexOf("Text:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                                        Dim value As String, Ms As MatchCollection = Regex.Matches(fLine, "Text:[^,;\s]+")
                                        For Each M As Match In Ms
                                            value = Regex.Replace(M.Value, "(Text:|[,;\s])", "")
                                            If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Texts.Contains(value) Then ACLists.Texts.Add(value)
                                        Next
                                    ElseIf fLine.IndexOf("Text3D:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                                        Dim value As String, Ms As MatchCollection = Regex.Matches(fLine, "Text3D:[^,;\s]+")
                                        For Each M As Match In Ms
                                            value = Regex.Replace(M.Value, "(Text3D:|[,;\s])", "")
                                            If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Texts2.Contains(value) Then ACLists.Texts2.Add(value)
                                        Next
                                    ElseIf fLine.IndexOf("Float:") > -1 AndAlso fLine.IndexOf("cellmin") = -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                                        Dim value As String, Ms As MatchCollection = Regex.Matches(fLine, "Float:[^,;\s]+")
                                        For Each M As Match In Ms
                                            value = Regex.Replace(M.Value, "(Float:|[,;\s])", "")
                                            If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Floats.Contains(value) Then ACLists.Floats.Add(value)
                                        Next
                                    ElseIf fLine.IndexOf("DB:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                                        Dim value As String, Ms As MatchCollection = Regex.Matches(fLine, "DB:[^,;\s]+")
                                        For Each M As Match In Ms
                                            value = Regex.Replace(M.Value, "(DB:|[,;\s])", "")
                                            If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Dbs.Contains(value) Then ACLists.Dbs.Add(value)
                                        Next
                                    ElseIf fLine.IndexOf("DBResult:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                                        Dim value As String, Ms As MatchCollection = Regex.Matches(fLine, "DBResult:[^,;\s]+")
                                        For Each M As Match In Ms
                                            value = Regex.Replace(M.Value, "(DBResult:|[,;\s])", "")
                                            If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.DbRes.Contains(value) Then ACLists.DbRes.Add(value)
                                        Next
                                    ElseIf fLine.IndexOf("File:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                                        Dim value As String, Ms As MatchCollection = Regex.Matches(fLine, "File:[^,;\s]+")
                                        For Each M As Match In Ms
                                            value = Regex.Replace(M.Value, "(File:|[,;\s])", "")
                                            If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Files.Contains(value) Then ACLists.Files.Add(value)
                                        Next
                                    Else
                                        Dim tDef As String, name As String, params As String(), func As PawnFunction
                                        For Each def As CustomUserPublics In ACLists.UserDefinedPublics
                                            Dim M As Match = Regex.Match(fLine, def.Regex)
                                            If M.Success Then
                                                tDef = def.Regex
                                                tmp = M.Value.Remove(0, tDef.IndexOf(".+"))
                                                tDef = tDef.Remove(0, tDef.IndexOf(".+"))
                                                name = Regex.Match(tmp, Regex.Escape(Mid(tDef, 1, tDef.IndexOf(".+", 1))).Replace("\.", ".").Replace("\+", "+")).Value
                                                tDef = tDef.Remove(0, 2)
                                                tmp = tmp.Replace(name, "")
                                                name = name.Remove(name.Length - 1, 1)
                                                params = Regex.Split(Mid(tmp, 1, tmp.Length - 2), "[\s]?,[\s]?")
                                                func = New PawnFunction(name, _Name.Replace(".inc", ":"), line.Number, params)
                                                If Not TrueContainsFunction(ACLists.Functions, func) Then ACLists.Functions.Add(func)
                                            End If
                                        Next
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
                            Dim file2 As String, path2 As String, cNode2 As New TreeNode()
                            If fLine.IndexOf("<") > -1 Then
                                file2 = Mid(fLine, fLine.IndexOf("<") + 2, fLine.IndexOf(">") - fLine.IndexOf("<") - 1)
                                path2 = My.Application.Info.DirectoryPath & "\Include\" & file2 & ".inc"
                            Else
                                If fLine.IndexOf("..") = -1 Then
                                    file2 = Mid(fLine, fLine.IndexOf("""") + 2, fLine.LastIndexOf("""") - fLine.IndexOf("""") - 1)
                                    path2 = My.Application.Info.DirectoryPath & "\Include\" & file2 & If(file.IndexOf(".inc") = -1, ".inc", "")
                                Else
                                    file2 = Mid(fLine, fLine.IndexOf("..") + 3, fLine.LastIndexOf("""") - fLine.IndexOf("""") - 1).Replace("/", "\")
                                    path2 = Directory.GetParent(My.Application.Info.DirectoryPath).FullName & file2 & If(file2.IndexOf(".inc") = -1, ".inc", "")
                                End If
                            End If
                            Dim count As Integer
                            If IO.File.Exists(path2) Then
                                Dim Reader2 As New StreamReader(path2)
                                fLine = Reader2.ReadLine()
                                Dim CommentedLine3 As Boolean, CommentedSection3 As Boolean
                                Do Until fLine Is Nothing
                                    If fLine.Length = 0 OrElse fLine = "{" OrElse fLine = "}" OrElse fLine = ";" Then
                                        count += 1
                                        fLine = Reader2.ReadLine()
                                        Continue Do
                                    ElseIf fLine.StartsWith("//") Then
                                        CommentedLine3 = True
                                    ElseIf fLine = "/*" OrElse fLine = " /*" Then
                                        CommentedSection3 = True
                                        count += 1
                                        fLine = Reader2.ReadLine()
                                        Continue Do
                                    ElseIf fLine = "*/" OrElse fLine = " */" Then
                                        CommentedSection3 = False
                                        count += 1
                                        fLine = Reader2.ReadLine()
                                        Continue Do
                                    ElseIf fLine.IndexOf("/*") > -1 AndAlso fLine.IndexOf("*/") = -1 Then
                                        CommentedSection3 = True
                                    ElseIf fLine.IndexOf("*/") > -1 Then
                                        CommentedSection3 = False
                                    End If
                                    If CommentedLine3 Or CommentedSection3 Then
                                        CommentedLine3 = False
                                        count += 1
                                        fLine = Reader2.ReadLine()
                                        Continue Do
                                    End If
                                    spos = fLine.IndexOf("native")
                                    If spos = -1 Then
                                        spos = fLine.IndexOf("stock")
                                        If spos = -1 Then spos = fLine.IndexOf("public")
                                    End If
                                    If spos > -1 AndAlso (fLine.EndsWith(";") AndAlso fLine.StartsWith("native ")) AndAlso fLine.IndexOf("(") > -1 AndAlso fLine.IndexOf(")") > -1 AndAlso fLine.IndexOf("operator") = -1 Then
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
                                        Dim value As String, Ms As MatchCollection = Regex.Matches(fLine, "Menu:[^,;\s]+")
                                        For Each M As Match In Ms
                                            value = Regex.Replace(M.Value, "(Menu:|[,;\s])", "")
                                            If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Menus.Contains(value) Then ACLists.Menus.Add(value)
                                        Next
                                    ElseIf fLine.IndexOf("Text:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                                        Dim value As String, Ms As MatchCollection = Regex.Matches(fLine, "Text:[^,;\s]+")
                                        For Each M As Match In Ms
                                            value = Regex.Replace(M.Value, "(Text:|[,;\s])", "")
                                            If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Texts.Contains(value) Then ACLists.Texts.Add(value)
                                        Next
                                    ElseIf fLine.IndexOf("Text3D:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                                        Dim value As String, Ms As MatchCollection = Regex.Matches(fLine, "Text3D:[^,;\s]+")
                                        For Each M As Match In Ms
                                            value = Regex.Replace(M.Value, "(Text3D:|[,;\s])", "")
                                            If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Texts2.Contains(value) Then ACLists.Texts2.Add(value)
                                        Next
                                    ElseIf fLine.IndexOf("Float:") > -1 AndAlso fLine.IndexOf("cellmin") = -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                                        Dim value As String, Ms As MatchCollection = Regex.Matches(fLine, "Float:[^,;\s]+")
                                        For Each M As Match In Ms
                                            value = Regex.Replace(M.Value, "(Float:|[,;\s])", "")
                                            If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Floats.Contains(value) Then ACLists.Floats.Add(value)
                                        Next
                                    ElseIf fLine.IndexOf("DB:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                                        Dim value As String, Ms As MatchCollection = Regex.Matches(fLine, "DB:[^,;\s]+")
                                        For Each M As Match In Ms
                                            value = Regex.Replace(M.Value, "(DB:|[,;\s])", "")
                                            If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Dbs.Contains(value) Then ACLists.Dbs.Add(value)
                                        Next
                                    ElseIf fLine.IndexOf("DBResult:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                                        Dim value As String, Ms As MatchCollection = Regex.Matches(fLine, "DBResult:[^,;\s]+")
                                        For Each M As Match In Ms
                                            value = Regex.Replace(M.Value, "(DBResult:|[,;\s])", "")
                                            If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.DbRes.Contains(value) Then ACLists.DbRes.Add(value)
                                        Next
                                    ElseIf fLine.IndexOf("File:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                                        Dim value As String, Ms As MatchCollection = Regex.Matches(fLine, "File:[^,;\s]+")
                                        For Each M As Match In Ms
                                            value = Regex.Replace(M.Value, "(File:|[,;\s])", "")
                                            If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Files.Contains(value) Then ACLists.Files.Add(value)
                                        Next
                                    Else
                                        Dim tDef As String, name As String, params As String(), func As PawnFunction
                                        For Each def As CustomUserPublics In ACLists.UserDefinedPublics
                                            Dim M As Match = Regex.Match(fLine, def.Regex)
                                            If M.Success Then
                                                tDef = def.Regex
                                                tmp = M.Value.Remove(0, tDef.IndexOf(".+"))
                                                tDef = tDef.Remove(0, tDef.IndexOf(".+"))
                                                name = Regex.Match(tmp, Regex.Escape(Mid(tDef, 1, tDef.IndexOf(".+", 1))).Replace("\.", ".").Replace("\+", "+")).Value
                                                tDef = tDef.Remove(0, 2)
                                                tmp = tmp.Replace(name, "")
                                                name = name.Remove(name.Length - 1, 1)
                                                params = Regex.Split(Mid(tmp, 1, tmp.Length - 2), "[\s]?,[\s]?")
                                                func = New PawnFunction(name, _Name.Replace(".inc", ":"), line.Number, params)
                                                If Not TrueContainsFunction(ACLists.Functions, func) Then ACLists.Functions.Add(func)
                                            End If
                                        Next
                                    End If
                                    count += 1
                                    fLine = Reader2.ReadLine()
                                Loop
                                Reader2.Close()
                            Else
                                Errors.Clear()
                                Errors.Add(New ListViewItem(New String() {"", "100", Name, count, "cannot read from file: """ & file2 & """"}, 1))
                            End If
                        ElseIf spos > -1 AndAlso (fLine.EndsWith(";") AndAlso fLine.StartsWith("native ")) AndAlso fLine.IndexOf("(") > -1 AndAlso fLine.IndexOf(")") > -1 AndAlso fLine.IndexOf("operator") = -1 Then
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
                            Dim value As String, Ms As MatchCollection = Regex.Matches(fLine, "Menu:[^,;\s]+")
                            For Each M As Match In Ms
                                value = Regex.Replace(M.Value, "(Menu:|[,;\s])", "")
                                If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Menus.Contains(value) Then ACLists.Menus.Add(value)
                            Next
                        ElseIf fLine.IndexOf("Text:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                            Dim value As String, Ms As MatchCollection = Regex.Matches(fLine, "Text:[^,;\s]+")
                            For Each M As Match In Ms
                                value = Regex.Replace(M.Value, "(Text:|[,;\s])", "")
                                If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Texts.Contains(value) Then ACLists.Texts.Add(value)
                            Next
                        ElseIf fLine.IndexOf("Text3D:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                            Dim value As String, Ms As MatchCollection = Regex.Matches(fLine, "Text3D:[^,;\s]+")
                            For Each M As Match In Ms
                                value = Regex.Replace(M.Value, "(Text3D:|[,;\s])", "")
                                If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Texts2.Contains(value) Then ACLists.Texts2.Add(value)
                            Next
                        ElseIf fLine.IndexOf("Float:") > -1 AndAlso fLine.IndexOf("cellmin") = -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                            Dim value As String, Ms As MatchCollection = Regex.Matches(fLine, "Float:[^,;\s]+")
                            For Each M As Match In Ms
                                value = Regex.Replace(M.Value, "(Float:|[,;\s])", "")
                                If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Floats.Contains(value) Then ACLists.Floats.Add(value)
                            Next
                        ElseIf fLine.IndexOf("DB:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                            Dim value As String, Ms As MatchCollection = Regex.Matches(fLine, "DB:[^,;\s]+")
                            For Each M As Match In Ms
                                value = Regex.Replace(M.Value, "(DB:|[,;\s])", "")
                                If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Dbs.Contains(value) Then ACLists.Dbs.Add(value)
                            Next
                        ElseIf fLine.IndexOf("DBResult:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                            Dim value As String, Ms As MatchCollection = Regex.Matches(fLine, "DBResult:[^,;\s]+")
                            For Each M As Match In Ms
                                value = Regex.Replace(M.Value, "(DBResult:|[,;\s])", "")
                                If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.DbRes.Contains(value) Then ACLists.DbRes.Add(value)
                            Next
                        ElseIf fLine.IndexOf("File:") > -1 AndAlso fLine.IndexOf("(") = -1 AndAlso fLine.IndexOf(")") = -1 Then
                            Dim value As String, Ms As MatchCollection = Regex.Matches(fLine, "File:[^,;\s]+")
                            For Each M As Match In Ms
                                value = Regex.Replace(M.Value, "(File:|[,;\s])", "")
                                If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Files.Contains(value) Then ACLists.Files.Add(value)
                            Next
                        Else
                            Dim tDef As String, name As String, params As String(), func As PawnFunction
                            For Each def As CustomUserPublics In ACLists.UserDefinedPublics
                                Dim M As Match = Regex.Match(fLine, def.Regex)
                                If M.Success Then
                                    tDef = def.Regex
                                    tmp = M.Value.Remove(0, tDef.IndexOf(".+"))
                                    tDef = tDef.Remove(0, tDef.IndexOf(".+"))
                                    name = Regex.Match(tmp, Regex.Escape(Mid(tDef, 1, tDef.IndexOf(".+", 1))).Replace("\.", ".").Replace("\+", "+")).Value
                                    tDef = tDef.Remove(0, 2)
                                    tmp = tmp.Replace(name, "")
                                    name = name.Remove(name.Length - 1, 1)
                                    params = Regex.Split(Mid(tmp, 1, tmp.Length - 2), "[\s]?,[\s]?")
                                    func = New PawnFunction(name, _Name.Replace(".inc", ":"), line.Number, params)
                                    If Not TrueContainsFunction(ACLists.Functions, func) Then ACLists.Functions.Add(func)
                                End If
                            Next
                        End If
                        fLine = Reader.ReadLine()
                    Loop
                    Reader.Close()
                Else
                    Errors.Clear()
                    Errors.Add(New ListViewItem(New String() {"", "100", Name, line.Number + 1, "cannot read from file: """ & file & """"}, 1))
                End If
            ElseIf spos > -1 AndAlso (line.Text.EndsWith(";") AndAlso line.Text.StartsWith("native ")) AndAlso line.Text.IndexOf("(") > -1 AndAlso line.Text.IndexOf(")") > -1 AndAlso line.Text.IndexOf("operator") = -1 AndAlso line.Text.IndexOf("#define") = -1 Then
                Dim params As New List(Of String)
                params.AddRange(Split(Trim(Mid(line.Text, line.Text.IndexOf("(") + 2, line.Text.IndexOf(")") - line.Text.IndexOf("(") - 1)), ","))
                For i = 0 To params.Count - 1
                    If i > 0 AndAlso params(i).Length > 0 AndAlso params(i).IndexOf("...") > -1 Then
                        params(i - 1) += "," & params(i)
                        params.RemoveAt(i)
                        Continue For
                    End If
                Next
                Dim func As PawnFunction = New PawnFunction(Trim(Mid(line.Text, line.Text.IndexOf(" ", spos) + 2, line.Text.IndexOf("(") - line.Text.IndexOf(" ", spos) - 1)), _Name.Replace(".inc", ":"), -1, params.ToArray)
                If Not TrueContainsFunction(ACLists.Functions, func, True) AndAlso Not TrueContainsFunction(ACLists.Callbacks, func) Then ACLists.Functions.Add(func)
            ElseIf line.Text.IndexOf("forward") > -1 AndAlso line.Text.IndexOf("#define") = -1 AndAlso line.Text.IndexOf("(") > -1 AndAlso line.Text.IndexOf(")") > -1 Then
                Dim func As PawnFunction = New PawnFunction(Trim(Mid(line.Text, line.Text.IndexOf(" ") + 1, line.Text.IndexOf("(") - line.Text.IndexOf(" "))), _Name.Replace(".inc", ":"), -1, Split(Trim(Mid(line.Text, line.Text.IndexOf("(") + 2, line.Text.IndexOf(")") - line.Text.IndexOf("(") - 1)), ","))
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
                Dim value As String, Ms As MatchCollection = Regex.Matches(line.Text, "Menu:[^,;\s]+")
                For Each M As Match In Ms
                    value = Regex.Replace(M.Value, "(Menu:|[,;\s])", "")
                    If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Menus.Contains(value) Then ACLists.Menus.Add(value)
                Next
            ElseIf line.Text.IndexOf("Text:") > -1 AndAlso line.Text.IndexOf("(") = -1 AndAlso line.Text.IndexOf(")") = -1 Then
                Dim value As String, Ms As MatchCollection = Regex.Matches(line.Text, "Text:[^,;\s]+")
                For Each M As Match In Ms
                    value = Regex.Replace(M.Value, "(Text:|[,;\s])", "")
                    If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Texts.Contains(value) Then ACLists.Texts.Add(value)
                Next
            ElseIf line.Text.IndexOf("Text3D:") > -1 AndAlso line.Text.IndexOf("(") = -1 AndAlso line.Text.IndexOf(")") = -1 Then
                Dim value As String, Ms As MatchCollection = Regex.Matches(line.Text, "Text3D:[^,;\s]+")
                For Each M As Match In Ms
                    value = Regex.Replace(M.Value, "(Text3D:|[,;\s])", "")
                    If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Texts2.Contains(value) Then ACLists.Texts2.Add(value)
                Next
            ElseIf line.Text.IndexOf("Float:") > -1 AndAlso line.Text.IndexOf("cellmin") = -1 AndAlso line.Text.IndexOf("(") = -1 AndAlso line.Text.IndexOf(")") = -1 Then
                Dim value As String, Ms As MatchCollection = Regex.Matches(line.Text, "Float:[^,;\s]+")
                For Each M As Match In Ms
                    value = Regex.Replace(M.Value, "(Float:|[,;\s])", "")
                    If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Floats.Contains(value) Then ACLists.Floats.Add(value)
                Next
            ElseIf line.Text.IndexOf("DB:") > -1 AndAlso line.Text.IndexOf("(") = -1 AndAlso line.Text.IndexOf(")") = -1 Then
                Dim value As String, Ms As MatchCollection = Regex.Matches(line.Text, "DB:[^,;\s]+")
                For Each M As Match In Ms
                    value = Regex.Replace(M.Value, "(DB:|[,;\s])", "")
                    If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Dbs.Contains(value) Then ACLists.Dbs.Add(value)
                Next
            ElseIf line.Text.IndexOf("DBResult:") > -1 AndAlso line.Text.IndexOf("(") = -1 AndAlso line.Text.IndexOf(")") = -1 Then
                Dim value As String, Ms As MatchCollection = Regex.Matches(line.Text, "DBResult:[^,;\s]+")
                For Each M As Match In Ms
                    value = Regex.Replace(M.Value, "(DBResult:|[,;\s])", "")
                    If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.DbRes.Contains(value) Then ACLists.DbRes.Add(value)
                Next
            ElseIf line.Text.IndexOf("File:") > -1 AndAlso line.Text.IndexOf("(") = -1 AndAlso line.Text.IndexOf(")") = -1 Then
                Dim value As String, Ms As MatchCollection = Regex.Matches(line.Text, "File:[^,;\s]+")
                For Each M As Match In Ms
                    value = Regex.Replace(M.Value, "(File:|[,;\s])", "")
                    If value.Length > 0 AndAlso value <> " " AndAlso Not Char.IsSymbol(value) AndAlso Not ACLists.Files.Contains(value) Then ACLists.Files.Add(value)
                Next
            Else
                If ACLists.UserDefinedPublics.Count > 0 Then
                    Dim tDef As String, name As String, params As String(), func As PawnFunction
                    For Each def As CustomUserPublics In ACLists.UserDefinedPublics
                        Dim M As Match = Regex.Match(line.Text, def.Regex)
                        If M.Success Then
                            tDef = def.Regex
                            tmp = M.Value.Remove(0, tDef.IndexOf(".+"))
                            tDef = tDef.Remove(0, tDef.IndexOf(".+"))
                            name = Regex.Match(tmp, Regex.Escape(Mid(tDef, 1, tDef.IndexOf(".+", 1))).Replace("\.", ".").Replace("\+", "+")).Value
                            tDef = tDef.Remove(0, 2)
                            tmp = tmp.Replace(name, "")
                            name = name.Remove(name.Length - 1, 1)
                            params = Regex.Split(Mid(tmp, 1, tmp.Length - 2), "[\s]?,[\s]?")
                            func = New PawnFunction(name, _Name.Replace(".inc", ":"), line.Number, params)
                            If Not TrueContainsFunction(ACLists.Functions, func) Then ACLists.Functions.Add(func)
                        End If
                    Next
                End If
            End If
        Next
        Dim tmpstring As String = vbNullString
        For Each item As PawnFunction In ACLists.Functions
            tmpstring += item.Name & " "
        Next
        SyntaxHandle.Lexing.Keywords(3) = tmpstring
        Colorize()
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
