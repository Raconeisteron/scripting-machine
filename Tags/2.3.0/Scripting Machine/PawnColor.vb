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

Public Structure PawnColor

#Region "Arrays"

    Private _name As String, _
        _hex As Color, _
        _line As Integer, _
        _exist As Boolean

#End Region

#Region "Properties"

    Public Property Name As String
        Get
            Name = _name
        End Get
        Set(ByVal value As String)
            _name = value
        End Set
    End Property

    Public Property Hex As Color
        Get
            Hex = _hex
        End Get
        Set(ByVal value As Color)
            _hex = value
        End Set
    End Property

    Public Property Line As Integer
        Get
            Line = _line
        End Get
        Set(ByVal value As Integer)
            _line = value
        End Set
    End Property

    Public Property Exist As Boolean
        Get
            Exist = _exist
        End Get
        Set(ByVal value As Boolean)
            _exist = value
        End Set
    End Property

#End Region

#Region "Operators"

    Public Shared Operator =(ByVal color1 As PawnColor, ByVal color2 As PawnColor) As Boolean
        Return color1._hex = color2._hex AndAlso color1._name = color2._name AndAlso color1._exist = color2._exist AndAlso color1._line = color2._line
    End Operator

    Public Shared Operator <>(ByVal color1 As PawnColor, ByVal color2 As PawnColor) As Boolean
        Return color1._hex <> color2._hex OrElse color1._name <> color2._name OrElse color1._exist <> color2._exist OrElse color1._line <> color2._line
    End Operator

#End Region

#Region "Methods"

    Public Overrides Function Equals(ByVal obj As Object) As Boolean
        If (obj Is Nothing) Or (Me.GetType() IsNot obj.GetType()) Then Return False
        Return (Me = CType(obj, PawnColor))
    End Function

    Public Overrides Function GetHashCode() As Integer
        Return CInt(_name.GetHashCode ^ _hex.GetHashCode() ^ _exist.GetHashCode() ^ _line.GetHashCode())
    End Function

    Public Sub New(ByVal Name As String, ByVal Hex As Color, ByVal Line As Integer)
        _name = Name
        _hex = Hex
        _line = Line
        _exist = True
    End Sub

#End Region

End Structure
