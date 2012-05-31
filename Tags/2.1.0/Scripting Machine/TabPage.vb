Imports System.ComponentModel
Imports System.Collections
Imports System.Diagnostics
Imports System.Windows.Forms
Imports System.Drawing.Drawing2D
Imports System.Drawing

Namespace TabPageEx

    ''' <summary>
    ''' Summary description for TabPage.
    ''' </summary>
    ''' 
    Public Class TabPageEx

        Inherits System.Windows.Forms.TabPage

        ''' <summary>
        ''' Required designer variable.
        ''' </summary>
        Private components As System.ComponentModel.Container = Nothing

        Public Sub New(ByVal container As System.ComponentModel.IContainer)
            container.Add(Me)

            InitializeComponent()
        End Sub

        Public Sub New()
            InitializeComponent()
        End Sub

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
        ''' 
        Private Sub InitializeComponent()
            components = New System.ComponentModel.Container()
        End Sub

        Public Overrides Property Text() As String
            Get
                Return MyBase.Text & "                                    "
            End Get
            Set(ByVal value As String)
                MyBase.Text = value
            End Set
        End Property

        Private ctxtMenu As ContextMenu = Nothing

        Public Property Menu() As ContextMenu
            Get
                Return Me.ctxtMenu
            End Get
            Set(ByVal value As ContextMenu)

                Me.ctxtMenu = value
            End Set
        End Property

#End Region

    End Class

End Namespace
