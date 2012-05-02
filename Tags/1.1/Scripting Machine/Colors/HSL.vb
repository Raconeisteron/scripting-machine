Public Structure HSL

#Region "Arrays"

    Dim _hue As Single
    Dim _sat As Single
    Dim _lum As Single

#End Region

#Region "Propertys"

    Public Property Hue() As Single
        Get
            Hue = _hue
        End Get
        Set(ByVal value As Single)
            If value > 360 Then
                _hue = 360
            Else
                If value < 0 Then
                    _hue = 0
                Else
                    _hue = value
                End If
            End If
        End Set
    End Property

    Public Property Saturation() As Single
        Get
            Saturation = _sat
        End Get
        Set(ByVal value As Single)
            If value > 1 Then
                _sat = 1
            Else
                If value < 0 Then
                    _sat = 0
                Else
                    _sat = value
                End If
            End If
        End Set
    End Property

    Public Property Luminance() As Single
        Get
            Luminance = _lum
        End Get
        Set(ByVal value As Single)
            If value > 1 Then
                _lum = 1
            Else
                If value < 0 Then
                    _lum = 0
                Else
                    _lum = value
                End If
            End If
        End Set
    End Property

#End Region

#Region "Operatos"

    Public Shared Operator =(ByVal color1 As HSL, ByVal color2 As HSL) As Boolean
        Return color1.Hue = color2.Hue AndAlso color1.Saturation = color2.Saturation AndAlso color1.Luminance = color2.Luminance
    End Operator

    Public Shared Operator <>(ByVal color1 As HSL, ByVal color2 As HSL) As Boolean
        Return color1.Hue <> color2.Hue OrElse color1.Saturation <> color2.Saturation OrElse color1.Luminance <> color2.Luminance
    End Operator

#End Region

#Region "Methods"

    Public Overrides Function Equals(ByVal obj As Object) As Boolean
        If (obj Is Nothing) Or (Me.GetType() IsNot obj.GetType()) Then Return False
        Return (Me = CType(obj, HSL))
    End Function

    Public Overrides Function GetHashCode() As Integer
        Return Hue.GetHashCode() ^ Saturation.GetHashCode() ^ Luminance.GetHashCode()
    End Function

#End Region

    Public Sub New(ByVal H As Single, ByVal S As Single, ByVal L As Single)
        Hue = H
        Saturation = S
        Luminance = L
    End Sub

End Structure