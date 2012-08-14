Public Class CustomUserPublics

#Region "Enums"

    Public Enum Macro_Type
        Macro_Type_Function
        Macro_Type_Definition
    End Enum

#End Region

#Region "Arrays"

    Private _regex As String, _
        _type As Macro_Type, _
        _paramscount As Integer

#End Region

#Region "Properties"

    Public Property Regex As String
        Get
            Regex = _regex
        End Get
        Set(value As String)
            _regex = value
        End Set
    End Property

    Public Property Type As Macro_Type
        Get
            Type = _type
        End Get
        Set(value As Macro_Type)
            _type = value
        End Set
    End Property

    Public Property ParamsCount As Integer
        Get
            ParamsCount = _paramscount
        End Get
        Set(value As Integer)
            _paramscount = value
        End Set
    End Property

#End Region

#Region "Operators"

    Public Shared Operator =(ByVal uPublic1 As CustomUserPublics, uPublic2 As CustomUserPublics) As Boolean
        Return uPublic1.Regex = uPublic2.Regex AndAlso uPublic1.Type = uPublic2.Type AndAlso uPublic1.ParamsCount = uPublic2.ParamsCount
    End Operator

    Public Shared Operator <>(ByVal uPublic1 As CustomUserPublics, uPublic2 As CustomUserPublics) As Boolean
        Return uPublic1.Regex <> uPublic2.Regex OrElse uPublic1.Type <> uPublic2.Type OrElse uPublic1.ParamsCount <> uPublic2.ParamsCount
    End Operator

#End Region

#Region "Methods"

    Public Overrides Function Equals(ByVal obj As Object) As Boolean
        If (obj Is Nothing) Or (Me.GetType() IsNot obj.GetType()) Then Return False
        Return (Me = CType(obj, CustomUserPublics))
    End Function

    Public Overrides Function GetHashCode() As Integer
        Return CInt(_regex.GetHashCode ^ _type.GetHashCode() ^ _paramscount.GetHashCode())
    End Function

    Public Sub New(ByVal regex As String, ByVal type As Macro_Type, Optional ByVal params As Integer = -1)
        _regex = regex
        _type = type
        _paramscount = params
    End Sub

#End Region

End Class
