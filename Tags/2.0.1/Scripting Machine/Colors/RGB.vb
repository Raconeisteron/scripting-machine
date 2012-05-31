Public Structure RGB

#Region "Arrays"

    Dim _red As Integer
    Dim _green As Integer
    Dim _blue As Integer

#End Region

#Region "Propertys"

    Public Property Red() As Integer
        Get
            Red = _red
        End Get
        Set(ByVal value As Integer)
            If value > 255 Then
                _red = 255
            Else
                If value < 0 Then
                    _red = 0
                Else
                    _red = value
                End If
            End If
        End Set
    End Property

    Public Property Green() As Integer
        Get
            Green = _green
        End Get
        Set(ByVal value As Integer)
            If value > 255 Then
                _green = 255
            Else
                If value < 0 Then
                    _green = 0
                Else
                    _green = value
                End If
            End If
        End Set
    End Property

    Public Property Blue() As Integer
        Get
            Blue = _blue
        End Get
        Set(ByVal value As Integer)
            If value > 255 Then
                _blue = 255
            Else
                If value < 0 Then
                    _blue = 0
                Else
                    _blue = value
                End If
            End If
        End Set
    End Property

#End Region

#Region "Operators"

    Public Shared Operator =(ByVal color1 As RGB, ByVal color2 As RGB) As Boolean
        Return color1.Red = color2.Red AndAlso color1.Green = color2.Green AndAlso color1.Blue = color2.Blue
    End Operator

    Public Shared Operator <>(ByVal color1 As RGB, ByVal color2 As RGB) As Boolean
        Return color1.Red <> color2.Red OrElse color1.Green <> color2.Green OrElse color1.Blue <> color2.Blue
    End Operator

#End Region

#Region "Methods"

    Public Overrides Function Equals(ByVal obj As Object) As Boolean
        If (obj Is Nothing) Or (Me.GetType() IsNot obj.GetType()) Then Return False
        Return (Me = CType(obj, RGB))
    End Function

    Public Overrides Function GetHashCode() As Integer
        Return CInt(Red.GetHashCode() ^ Green.GetHashCode() ^ Blue.GetHashCode())
    End Function

    Public Sub New(ByVal R As Integer, ByVal G As Integer, ByVal B As Integer)
        Red = R
        Green = G
        Blue = B
    End Sub

#End Region

End Structure
