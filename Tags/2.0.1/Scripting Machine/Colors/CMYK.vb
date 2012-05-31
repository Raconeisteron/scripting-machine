Public Structure CMYK

#Region "Arrays"

    Dim _cyan As Single
    Dim _mag As Single
    Dim _yell As Single
    Dim _black As Single

#End Region

#Region "Propertys"

    Public Property Cyan() As Single
        Get
            Cyan = _cyan
        End Get
        Set(ByVal value As Single)
            If value > 1 Then
                _cyan = 1
            Else
                If value < 0 Then
                    _cyan = 0
                Else
                    _cyan = value
                End If
            End If
        End Set
    End Property

    Public Property Magenta() As Single
        Get
            Magenta = _mag
        End Get
        Set(ByVal value As Single)
            If value > 1 Then
                _mag = 1
            Else
                If value < 0 Then
                    _mag = 0
                Else
                    _mag = value
                End If
            End If
        End Set
    End Property

    Public Property Yellow() As Single
        Get
            Yellow = _yell
        End Get
        Set(ByVal value As Single)
            If value > 1 Then
                _yell = 1
            Else
                If value < 0 Then
                    _yell = 0
                Else
                    _yell = value
                End If
            End If
        End Set
    End Property

    Public Property Black() As Single
        Get
            Black = _black
        End Get
        Set(ByVal value As Single)
            If value > 1 Then
                _black = 1
            Else
                If value < 0 Then
                    _black = 0
                Else
                    _black = value
                End If
            End If
        End Set
    End Property

#End Region

#Region "Operators"

    Public Shared Operator =(ByVal color1 As CMYK, ByVal color2 As CMYK) As Boolean
        Return color1.Cyan = color2.Cyan AndAlso color1.Magenta = color2.Magenta AndAlso color1.Yellow = color2.Yellow AndAlso color1.Black = color2.Black
    End Operator

    Public Shared Operator <>(ByVal color1 As CMYK, ByVal color2 As CMYK) As Boolean
        Return color1.Cyan <> color2.Cyan OrElse color1.Magenta <> color2.Magenta OrElse color1.Yellow <> color2.Yellow OrElse color1.Black <> color2.Black
    End Operator

#End Region

#Region "Methods"

    Public Overrides Function Equals(ByVal obj As Object) As Boolean
        If (obj Is Nothing) Or (Me.GetType() IsNot obj.GetType()) Then Return False
        Return (Me = CType(obj, CMYK))
    End Function

    Public Overrides Function GetHashCode() As Integer
        Return Cyan.GetHashCode() ^ Magenta.GetHashCode() ^ Yellow.GetHashCode() ^ Black.GetHashCode()
    End Function

    Public Sub New(ByVal C As Single, ByVal M As Single, ByVal Y As Single, ByVal K As Single)
        Cyan = C
        Magenta = M
        Yellow = Y
        Black = K
    End Sub

#End Region

End Structure
