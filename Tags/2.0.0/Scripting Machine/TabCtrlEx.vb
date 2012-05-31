Imports System.Collections
Imports System.ComponentModel
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Data
Imports System.Windows.Forms
Imports System.IO
Imports System.Reflection
Namespace TabControlEx
    ''' <summary>
    ''' Summary description for UserControl1.
    ''' </summary>
    ''' 
    Public Class TabCtlEx

        Inherits System.Windows.Forms.TabControl

        ''' <summary>
        ''' Required designer variable.
        ''' </summary>
        Private components As System.ComponentModel.Container = Nothing

        Public Delegate Sub OnHeaderCloseDelegate(ByVal sender As Object, ByVal e As CloseEventArgs)
        Public Event OnClose As OnHeaderCloseDelegate

        Public Sub New()
            ' This call is required by the Windows.Forms Form Designer.
            InitializeComponent()


            ' TODO: Add any initialization after the InitComponent call

            Me.TabStop = False
        End Sub

        Private m_confirmOnClose As Boolean = True

        Public Property ConfirmOnClose() As Boolean
            Get
                Return Me.m_confirmOnClose
            End Get
            Set(ByVal value As Boolean)
                Me.m_confirmOnClose = value
            End Set
        End Property

        ''' <summary>
        ''' Clean up any resources being used.
        ''' </summary>
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing Then
                If components IsNot Nothing Then
                    components.Dispose()
                End If
            End If
            MyBase.Dispose(disposing)
        End Sub


#Region "Component Designer generated code"

        ''' <summary>
        ''' Required method for Designer support - do not modify 
        ''' the contents of this method with the code editor.
        ''' </summary>
        Private Sub InitializeComponent()
            components = New System.ComponentModel.Container()
            SetStyle(System.Windows.Forms.ControlStyles.DoubleBuffer, True)
            Me.TabStop = False
            Me.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed
            Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CByte(0))
            Me.ItemSize = New System.Drawing.Size(230, 24)
            'this.Controls.Add(this.btnClose); 
        End Sub

#End Region


        Private Function GetContentFromResource(ByVal filename As String) As Stream
            Dim asm As Assembly = Assembly.GetExecutingAssembly()
            Dim stream As Stream = asm.GetManifestResourceStream("MyControlLibrary." & filename)
            Return stream

        End Function

        Protected Overrides Sub OnDrawItem(ByVal e As DrawItemEventArgs)

            If e.Bounds <> RectangleF.Empty Then
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias

                Dim tabTextArea As RectangleF = RectangleF.Empty

                For nIndex As Integer = 0 To Me.TabCount - 1
                    If nIndex <> Me.SelectedIndex Then
                        tabTextArea = CType(Me.GetTabRect(nIndex), RectangleF)
                        Dim _Path As New GraphicsPath()
                        _Path.AddRectangle(tabTextArea)
                        Using _Brush As New LinearGradientBrush(tabTextArea, SystemColors.Control, SystemColors.ControlLight, LinearGradientMode.Vertical)
                            Dim _ColorBlend As New ColorBlend(3)
                            _ColorBlend.Colors = New Color() {SystemColors.ControlLightLight, Color.FromArgb(255, SystemColors.ControlLight), SystemColors.ControlDark, SystemColors.ControlLightLight}

                            _ColorBlend.Positions = New Single() {0.0F, 0.4F, 0.5F, 1.0F}
                            _Brush.InterpolationColors = _ColorBlend

                            e.Graphics.FillPath(_Brush, _Path)
                            Using pen As New Pen(SystemColors.ActiveBorder)
                                e.Graphics.DrawPath(pen, _Path)
                            End Using


                            _ColorBlend.Colors = New Color() {SystemColors.ActiveBorder, SystemColors.ActiveBorder, SystemColors.ActiveBorder, SystemColors.ActiveBorder}

                            _ColorBlend.Positions = New Single() {0.0F, 0.4F, 0.5F, 1.0F}
                            _Brush.InterpolationColors = _ColorBlend
                            e.Graphics.FillRectangle(_Brush, tabTextArea.X + tabTextArea.Width - 22, 4, tabTextArea.Height - 3, tabTextArea.Height - 5)
                            e.Graphics.DrawRectangle(Pens.White, tabTextArea.X + tabTextArea.Width - 20, 6, tabTextArea.Height - 8, tabTextArea.Height - 9)
                            Using pen As New Pen(Color.White, 2)
                                e.Graphics.DrawLine(pen, tabTextArea.X + tabTextArea.Width - 16, 9, tabTextArea.X + tabTextArea.Width - 7, 17)
                                e.Graphics.DrawLine(pen, tabTextArea.X + tabTextArea.Width - 16, 17, tabTextArea.X + tabTextArea.Width - 7, 9)
                            End Using
                            If CanDrawMenuButton(nIndex) Then
                                _ColorBlend.Positions = New Single() {0.0F, 0.4F, 0.5F, 1.0F}
                                _Brush.InterpolationColors = _ColorBlend
                                _ColorBlend.Positions = New Single() {0.0F, 0.4F, 0.5F, 1.0F}
                                ' assign the color blend to the pathgradientbrush
                                _Brush.InterpolationColors = _ColorBlend

                                e.Graphics.FillRectangle(_Brush, tabTextArea.X + tabTextArea.Width - 43, 4, tabTextArea.Height - 3, tabTextArea.Height - 5)
                                ' e.Graphics.DrawRectangle(SystemPens.GradientInactiveCaption, tabTextArea.X + tabTextArea.Width - 37, 7, 13, 13);
                                e.Graphics.DrawRectangle(New Pen(Color.White), tabTextArea.X + tabTextArea.Width - 41, 6, tabTextArea.Height - 7, tabTextArea.Height - 9)
                                Using pen As New Pen(Color.White, 2)
                                    e.Graphics.DrawLine(pen, tabTextArea.X + tabTextArea.Width - 36, 11, tabTextArea.X + tabTextArea.Width - 33, 16)
                                    e.Graphics.DrawLine(pen, tabTextArea.X + tabTextArea.Width - 33, 16, tabTextArea.X + tabTextArea.Width - 30, 11)
                                End Using
                            End If
                        End Using

                        _Path.Dispose()
                    Else

                        tabTextArea = CType(Me.GetTabRect(nIndex), RectangleF)
                        Dim _Path As New GraphicsPath()
                        _Path.AddRectangle(tabTextArea)
                        Using _Brush As New LinearGradientBrush(tabTextArea, SystemColors.Control, SystemColors.ControlLight, LinearGradientMode.Vertical)
                            Dim _ColorBlend As New ColorBlend(3)
                            _ColorBlend.Colors = New Color() {SystemColors.ControlLightLight, Color.FromArgb(255, SystemColors.Control), SystemColors.ControlLight, SystemColors.Control}
                            _ColorBlend.Positions = New Single() {0.0F, 0.4F, 0.5F, 1.0F}
                            _Brush.InterpolationColors = _ColorBlend
                            e.Graphics.FillPath(_Brush, _Path)
                            Using pen As New Pen(SystemColors.ActiveBorder)
                                e.Graphics.DrawPath(pen, _Path)
                            End Using
                            'Drawing Close Button
                            _ColorBlend.Colors = New Color() {Color.FromArgb(255, 231, 164, 152), Color.FromArgb(255, 231, 164, 152), Color.FromArgb(255, 197, 98, 79), Color.FromArgb(255, 197, 98, 79)}
                            _Brush.InterpolationColors = _ColorBlend
                            e.Graphics.FillRectangle(_Brush, tabTextArea.X + tabTextArea.Width - 22, 4, tabTextArea.Height - 3, tabTextArea.Height - 5)
                            e.Graphics.DrawRectangle(Pens.White, tabTextArea.X + tabTextArea.Width - 20, 6, tabTextArea.Height - 8, tabTextArea.Height - 9)
                            Using pen As New Pen(Color.White, 2)
                                e.Graphics.DrawLine(pen, tabTextArea.X + tabTextArea.Width - 16, 9, tabTextArea.X + tabTextArea.Width - 7, 17)
                                e.Graphics.DrawLine(pen, tabTextArea.X + tabTextArea.Width - 16, 17, tabTextArea.X + tabTextArea.Width - 7, 9)
                            End Using
                            If CanDrawMenuButton(nIndex) Then
                                'Drawing menu button
                                _ColorBlend.Colors = New Color() {SystemColors.ControlLightLight, Color.FromArgb(255, SystemColors.ControlLight), SystemColors.ControlDark, SystemColors.ControlLightLight}
                                _ColorBlend.Positions = New Single() {0.0F, 0.4F, 0.5F, 1.0F}
                                _Brush.InterpolationColors = _ColorBlend
                                _ColorBlend.Colors = New Color() {Color.FromArgb(255, 170, 213, 243), Color.FromArgb(255, 170, 213, 243), Color.FromArgb(255, 44, 137, 191), Color.FromArgb(255, 44, 137, 191)}
                                _Brush.InterpolationColors = _ColorBlend
                                e.Graphics.FillRectangle(_Brush, tabTextArea.X + tabTextArea.Width - 43, 4, tabTextArea.Height - 3, tabTextArea.Height - 5)
                                e.Graphics.DrawRectangle(Pens.White, tabTextArea.X + tabTextArea.Width - 41, 6, tabTextArea.Height - 7, tabTextArea.Height - 9)
                                Using pen As New Pen(Color.White, 2)
                                    e.Graphics.DrawLine(pen, tabTextArea.X + tabTextArea.Width - 36, 11, tabTextArea.X + tabTextArea.Width - 33, 16)
                                    e.Graphics.DrawLine(pen, tabTextArea.X + tabTextArea.Width - 33, 16, tabTextArea.X + tabTextArea.Width - 30, 11)
                                End Using
                            End If
                        End Using
                        _Path.Dispose()
                    End If
                    Dim str As String = Me.TabPages(nIndex).Text
                    Dim stringFormat As New StringFormat()
                    stringFormat.Alignment = StringAlignment.Center


                    e.Graphics.DrawString(str, Me.Font, New SolidBrush(Me.TabPages(nIndex).ForeColor), tabTextArea, stringFormat)
                Next
            End If

        End Sub

        Private Function CanDrawMenuButton(ByVal nIndex As Integer) As Boolean
            If DirectCast(Me.TabPages(nIndex), TabPageEx.TabPageEx).Menu IsNot Nothing Then
                Return True
            End If
            Return False
        End Function

        Protected Overrides Sub OnMouseLeave(ByVal e As EventArgs)
            Dim g As Graphics = CreateGraphics()
            g.SmoothingMode = SmoothingMode.AntiAlias
            Dim tabTextArea As RectangleF = RectangleF.Empty
            For nIndex As Integer = 0 To Me.TabCount - 1
                If nIndex <> Me.SelectedIndex Then
                    tabTextArea = CType(Me.GetTabRect(nIndex), RectangleF)
                    Dim _Path As New GraphicsPath()
                    _Path.AddRectangle(tabTextArea)
                    Using _Brush As New LinearGradientBrush(tabTextArea, SystemColors.Control, SystemColors.ControlLight, LinearGradientMode.Vertical)
                        Dim _ColorBlend As New ColorBlend(3)

                        _ColorBlend.Colors = New Color() {SystemColors.ActiveBorder, SystemColors.ActiveBorder, SystemColors.ActiveBorder, SystemColors.ActiveBorder}

                        _ColorBlend.Positions = New Single() {0.0F, 0.4F, 0.5F, 1.0F}
                        _Brush.InterpolationColors = _ColorBlend
                        g.FillRectangle(_Brush, tabTextArea.X + tabTextArea.Width - 22, 4, tabTextArea.Height - 2, tabTextArea.Height - 5)
                        g.DrawRectangle(Pens.White, tabTextArea.X + tabTextArea.Width - 20, 6, tabTextArea.Height - 8, tabTextArea.Height - 9)
                        Using pen As New Pen(Color.White, 2)
                            g.DrawLine(pen, tabTextArea.X + tabTextArea.Width - 16, 9, tabTextArea.X + tabTextArea.Width - 7, 17)
                            g.DrawLine(pen, tabTextArea.X + tabTextArea.Width - 16, 17, tabTextArea.X + tabTextArea.Width - 7, 9)
                        End Using
                        If CanDrawMenuButton(nIndex) Then
                            _ColorBlend.Positions = New Single() {0.0F, 0.4F, 0.5F, 1.0F}
                            ' assign the color blend to the pathgradientbrush
                            _Brush.InterpolationColors = _ColorBlend

                            g.FillRectangle(_Brush, tabTextArea.X + tabTextArea.Width - 43, 4, tabTextArea.Height - 3, tabTextArea.Height - 5)
                            ' e.Graphics.DrawRectangle(SystemPens.GradientInactiveCaption, tabTextArea.X + tabTextArea.Width - 37, 7, 13, 13);
                            g.DrawRectangle(New Pen(Color.White), tabTextArea.X + tabTextArea.Width - 41, 6, tabTextArea.Height - 7, tabTextArea.Height - 9)
                            Using pen As New Pen(Color.White, 2)
                                g.DrawLine(pen, tabTextArea.X + tabTextArea.Width - 36, 11, tabTextArea.X + tabTextArea.Width - 33, 16)
                                g.DrawLine(pen, tabTextArea.X + tabTextArea.Width - 33, 16, tabTextArea.X + tabTextArea.Width - 30, 11)
                            End Using
                        End If
                    End Using

                    _Path.Dispose()
                Else

                    tabTextArea = CType(Me.GetTabRect(nIndex), RectangleF)
                    Dim _Path As New GraphicsPath()
                    _Path.AddRectangle(tabTextArea)
                    Using _Brush As New LinearGradientBrush(tabTextArea, SystemColors.Control, SystemColors.ControlLight, LinearGradientMode.Vertical)
                        Dim _ColorBlend As New ColorBlend(3)
                        _ColorBlend.Positions = New Single() {0.0F, 0.4F, 0.5F, 1.0F}

                        _ColorBlend.Colors = New Color() {Color.FromArgb(255, 231, 164, 152), Color.FromArgb(255, 231, 164, 152), Color.FromArgb(255, 197, 98, 79), Color.FromArgb(255, 197, 98, 79)}
                        _Brush.InterpolationColors = _ColorBlend
                        g.FillRectangle(_Brush, tabTextArea.X + tabTextArea.Width - 22, 4, tabTextArea.Height - 3, tabTextArea.Height - 5)
                        g.DrawRectangle(Pens.White, tabTextArea.X + tabTextArea.Width - 20, 6, tabTextArea.Height - 8, tabTextArea.Height - 9)
                        Using pen As New Pen(Color.White, 2)
                            g.DrawLine(pen, tabTextArea.X + tabTextArea.Width - 16, 9, tabTextArea.X + tabTextArea.Width - 7, 17)
                            g.DrawLine(pen, tabTextArea.X + tabTextArea.Width - 16, 17, tabTextArea.X + tabTextArea.Width - 7, 9)
                        End Using
                        If CanDrawMenuButton(nIndex) Then
                            'Drawing menu button
                            _ColorBlend.Colors = New Color() {SystemColors.ControlLightLight, Color.FromArgb(255, SystemColors.ControlLight), SystemColors.ControlDark, SystemColors.ControlLightLight}
                            _ColorBlend.Positions = New Single() {0.0F, 0.4F, 0.5F, 1.0F}
                            _Brush.InterpolationColors = _ColorBlend
                            _ColorBlend.Colors = New Color() {Color.FromArgb(255, 170, 213, 243), Color.FromArgb(255, 170, 213, 243), Color.FromArgb(255, 44, 137, 191), Color.FromArgb(255, 44, 137, 191)}
                            _Brush.InterpolationColors = _ColorBlend
                            g.FillRectangle(_Brush, tabTextArea.X + tabTextArea.Width - 43, 4, tabTextArea.Height - 3, tabTextArea.Height - 5)
                            g.DrawRectangle(Pens.White, tabTextArea.X + tabTextArea.Width - 41, 6, tabTextArea.Height - 7, tabTextArea.Height - 9)
                            Using pen As New Pen(Color.White, 2)
                                g.DrawLine(pen, tabTextArea.X + tabTextArea.Width - 36, 11, tabTextArea.X + tabTextArea.Width - 33, 16)
                                g.DrawLine(pen, tabTextArea.X + tabTextArea.Width - 33, 16, tabTextArea.X + tabTextArea.Width - 30, 11)
                            End Using
                        End If
                    End Using
                    _Path.Dispose()

                End If
            Next

            g.Dispose()


        End Sub

        Protected Overrides Sub OnMouseMove(ByVal e As MouseEventArgs)

            If Not DesignMode Then
                Dim g As Graphics = CreateGraphics()
                g.SmoothingMode = SmoothingMode.AntiAlias
                For nIndex As Integer = 0 To Me.TabCount - 1
                    Dim tabTextArea As RectangleF = CType(Me.GetTabRect(nIndex), RectangleF)
                    tabTextArea = New RectangleF(tabTextArea.X + tabTextArea.Width - 22, 4, tabTextArea.Height - 3, tabTextArea.Height - 5)

                    Dim pt As New Point(e.X, e.Y)
                    If tabTextArea.Contains(pt) Then
                        Using _Brush As New LinearGradientBrush(tabTextArea, SystemColors.Control, SystemColors.ControlLight, LinearGradientMode.Vertical)
                            Dim _ColorBlend As New ColorBlend(3)
                            _ColorBlend.Colors = New Color() {Color.FromArgb(255, 252, 193, 183), Color.FromArgb(255, 252, 193, 183), Color.FromArgb(255, 210, 35, 2), Color.FromArgb(255, 210, 35, 2)}
                            _ColorBlend.Positions = New Single() {0.0F, 0.4F, 0.5F, 1.0F}
                            _Brush.InterpolationColors = _ColorBlend

                            g.FillRectangle(_Brush, tabTextArea)
                            g.DrawRectangle(Pens.White, tabTextArea.X + 2, 6, tabTextArea.Height - 3, tabTextArea.Height - 4)
                            Using pen As New Pen(Color.White, 2)
                                g.DrawLine(pen, tabTextArea.X + 6, 9, tabTextArea.X + 15, 17)
                                g.DrawLine(pen, tabTextArea.X + 6, 17, tabTextArea.X + 15, 9)
                            End Using
                        End Using
                    Else
                        If nIndex <> SelectedIndex Then
                            Using _Brush As New LinearGradientBrush(tabTextArea, SystemColors.Control, SystemColors.ControlLight, LinearGradientMode.Vertical)
                                Dim _ColorBlend As New ColorBlend(3)
                                _ColorBlend.Colors = New Color() {SystemColors.ActiveBorder, SystemColors.ActiveBorder, SystemColors.ActiveBorder, SystemColors.ActiveBorder}
                                _ColorBlend.Positions = New Single() {0.0F, 0.4F, 0.5F, 1.0F}
                                _Brush.InterpolationColors = _ColorBlend

                                g.FillRectangle(_Brush, tabTextArea)
                                g.DrawRectangle(Pens.White, tabTextArea.X + 2, 6, tabTextArea.Height - 3, tabTextArea.Height - 4)
                                Using pen As New Pen(Color.White, 2)
                                    g.DrawLine(pen, tabTextArea.X + 6, 9, tabTextArea.X + 15, 17)
                                    g.DrawLine(pen, tabTextArea.X + 6, 17, tabTextArea.X + 15, 9)
                                End Using
                            End Using
                        End If
                    End If
                    If CanDrawMenuButton(nIndex) Then
                        Dim tabMenuArea As RectangleF = CType(Me.GetTabRect(nIndex), RectangleF)
                        tabMenuArea = New RectangleF(tabMenuArea.X + tabMenuArea.Width - 43, 4, tabMenuArea.Height - 3, tabMenuArea.Height - 5)
                        pt = New Point(e.X, e.Y)
                        If tabMenuArea.Contains(pt) Then
                            Using _Brush As New LinearGradientBrush(tabMenuArea, SystemColors.Control, SystemColors.ControlLight, LinearGradientMode.Vertical)
                                Dim _ColorBlend As New ColorBlend(3)
                                _ColorBlend.Colors = New Color() {Color.FromArgb(255, 170, 213, 255), Color.FromArgb(255, 170, 213, 255), Color.FromArgb(255, 44, 157, 250), Color.FromArgb(255, 44, 157, 250)}
                                _ColorBlend.Positions = New Single() {0.0F, 0.4F, 0.5F, 1.0F}
                                _Brush.InterpolationColors = _ColorBlend

                                g.FillRectangle(_Brush, tabMenuArea)
                                g.DrawRectangle(Pens.White, tabMenuArea.X + 2, 6, tabMenuArea.Height - 2, tabMenuArea.Height - 4)
                                Using pen As New Pen(Color.White, 2)
                                    g.DrawLine(pen, tabMenuArea.X + 7, 11, tabMenuArea.X + 10, 16)
                                    g.DrawLine(pen, tabMenuArea.X + 10, 16, tabMenuArea.X + 13, 11)
                                End Using
                            End Using
                        Else
                            If nIndex <> SelectedIndex Then
                                Using _Brush As New LinearGradientBrush(tabMenuArea, SystemColors.Control, SystemColors.ControlLight, LinearGradientMode.Vertical)
                                    Dim _ColorBlend As New ColorBlend(3)
                                    _ColorBlend.Colors = New Color() {SystemColors.ActiveBorder, SystemColors.ActiveBorder, SystemColors.ActiveBorder, SystemColors.ActiveBorder}
                                    _ColorBlend.Positions = New Single() {0.0F, 0.4F, 0.5F, 1.0F}
                                    _Brush.InterpolationColors = _ColorBlend

                                    g.FillRectangle(_Brush, tabMenuArea)
                                    g.DrawRectangle(Pens.White, tabMenuArea.X + 2, 6, tabMenuArea.Height - 2, tabMenuArea.Height - 4)
                                    Using pen As New Pen(Color.White, 2)
                                        g.DrawLine(pen, tabMenuArea.X + 7, 11, tabMenuArea.X + 10, 16)
                                        g.DrawLine(pen, tabMenuArea.X + 10, 16, tabMenuArea.X + 13, 11)
                                    End Using
                                End Using
                            End If
                        End If

                    End If
                Next
                g.Dispose()
            End If
            'Reorder tab
            If (e.Button = Windows.Forms.MouseButtons.Left) AndAlso (Me._SourceTabPage IsNot Nothing) Then
                Dim currTabPage As System.Windows.Forms.TabPage = GetTabPageFromXY(e.X, e.Y)
                If (currTabPage IsNot Nothing) AndAlso (Not currTabPage.Equals(Me._SourceTabPage)) Then
                    Dim currRect As Drawing.Rectangle = MyBase.GetTabRect(MyBase.TabPages.IndexOf(currTabPage))
                    If (MyBase.TabPages.IndexOf(currTabPage) < MyBase.TabPages.IndexOf(Me._SourceTabPage)) Then
                        MyBase.TabPages.Remove(Me._SourceTabPage)
                        MyBase.TabPages.Insert(MyBase.TabPages.IndexOf(currTabPage), Me._SourceTabPage)
                        MyBase.SelectedTab = Me._SourceTabPage
                    ElseIf (MyBase.TabPages.IndexOf(currTabPage) > MyBase.TabPages.IndexOf(Me._SourceTabPage)) Then
                        MyBase.TabPages.Remove(Me._SourceTabPage)
                        MyBase.TabPages.Insert(MyBase.TabPages.IndexOf(currTabPage) + 1, Me._SourceTabPage)
                        MyBase.SelectedTab = Me._SourceTabPage
                    End If
                End If
            End If
        End Sub

        Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
            If Not DesignMode Then
                Dim tabTextArea As RectangleF = CType(Me.GetTabRect(SelectedIndex), RectangleF)
                tabTextArea = InlineAssignHelper(tabTextArea, New RectangleF(tabTextArea.X + tabTextArea.Width - 22, 4, tabTextArea.Height - 3, tabTextArea.Height - 5))
                Dim pt As New Point(e.X, e.Y)
                If tabTextArea.Contains(pt) Then
                    If m_confirmOnClose Then
                        If MessageBox.Show("You are about to close " & Me.TabPages(SelectedIndex).Text.TrimEnd() & " tab. Are you sure you want to continue?", "Confirm close", MessageBoxButtons.YesNo) = DialogResult.No Then
                            Return
                        End If
                    End If
                    'Fire Event to Client
                    RaiseEvent OnClose(Me, New CloseEventArgs(SelectedIndex))
                End If
                If CanDrawMenuButton(SelectedIndex) Then
                    Dim tabMenuArea As RectangleF = CType(Me.GetTabRect(SelectedIndex), RectangleF)
                    tabMenuArea = New RectangleF(tabMenuArea.X + tabMenuArea.Width - 43, 4, tabMenuArea.Height - 3, tabMenuArea.Height - 5)
                    pt = New Point(e.X, e.Y)
                    If tabMenuArea.Contains(pt) Then
                        If DirectCast(Me.TabPages(SelectedIndex), TabPageEx.TabPageEx).Menu IsNot Nothing Then
                            DirectCast(Me.TabPages(SelectedIndex), TabPageEx.TabPageEx).Menu.Show(Me, New Point(CInt(Math.Truncate(tabMenuArea.X)), CInt(Math.Truncate(tabMenuArea.Y + tabMenuArea.Height))))
                        End If
                    End If
                End If
            End If
            'Reorder tab
            If (e.Button = Windows.Forms.MouseButtons.Left) AndAlso (MyBase.SelectedTab IsNot Nothing) AndAlso (Not MyBase.GetTabRect(MyBase.SelectedIndex).IsEmpty) Then
                Me._SourceTabPage = MyBase.SelectedTab
            End If
            MyBase.OnMouseDown(e)
        End Sub

        Private Shared Function InlineAssignHelper(Of T)(ByRef target As T, ByVal value As T) As T
            target = value
            Return value
        End Function

#Region "Reordering tabs"

        Private _SourceTabPage As System.Windows.Forms.TabPage = Nothing

        Private Function GetTabPageFromXY(ByVal x As Integer, ByVal y As Integer) As TabPage
            For i As Integer = 0 To MyBase.TabPages.Count - 1
                If MyBase.GetTabRect(i).Contains(x, y) Then
                    Return MyBase.TabPages(i)
                End If
            Next
            Return Nothing
        End Function

#End Region

    End Class

    Public Class CloseEventArgs

        Inherits EventArgs
        Private nTabIndex As Integer = -1

        Public Sub New(ByVal nTabIndex As Integer)
            Me.nTabIndex = nTabIndex
        End Sub

        ''' <summary>
        ''' Get/Set the tab index value where the close button is clicked
        ''' </summary>
        Public Property TabIndex() As Integer
            Get
                Return Me.nTabIndex
            End Get
            Set(ByVal value As Integer)
                Me.nTabIndex = value
            End Set
        End Property

    End Class

End Namespace
