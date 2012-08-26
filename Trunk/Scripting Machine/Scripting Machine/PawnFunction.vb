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

Public Structure PawnFunction

#Region "Arrays"

    Private _name As String, _
        _params As String(), _
        _inc As String, _
        _Line As Integer, _
        _Exist As Boolean

#End Region

#Region "Properties"

    Public Property Name As String
        Get
            Name = Trim(_name) & " "
        End Get
        Set(ByVal value As String)
            _name = value
        End Set
    End Property

    Public Property Params As String()
        Get
            Params = _params
        End Get
        Set(ByVal value As String())
            _params = value
        End Set
    End Property

    Public Property Include As String
        Get
            Include = _inc
        End Get
        Set(ByVal value As String)
            _inc = value
        End Set
    End Property

    Public Property Line As Integer
        Get
            Line = _Line
        End Get
        Set(ByVal value As Integer)
            _Line = value
        End Set
    End Property

    Public Property Exist As Boolean
        Get
            Exist = _Exist
        End Get
        Set(ByVal value As Boolean)
            _Exist = value
        End Set
    End Property

#End Region

#Region "Operators"

    Public Shared Operator =(ByVal func1 As PawnFunction, ByVal func2 As PawnFunction) As Boolean
        Return func1._name = func2._name AndAlso func1._params Is func2._params AndAlso func1._inc = func2._inc AndAlso func1._Line = func1._Line
    End Operator

    Public Shared Operator <>(ByVal func1 As PawnFunction, ByVal func2 As PawnFunction) As Boolean
        Return func1._name <> func2._name OrElse Not func1._params Is func2._params OrElse func1._inc <> func2._inc OrElse func1._Line <> func1._Line
    End Operator

#End Region

#Region "Methods"

    Public Overrides Function Equals(ByVal obj As Object) As Boolean
        If (obj Is Nothing) Or (Me.GetType() IsNot obj.GetType()) Then Return False
        Return (Me = CType(obj, PawnFunction))
    End Function

    Public Overrides Function GetHashCode() As Integer
        Return CInt(_name.GetHashCode() ^ _params.GetHashCode ^ _inc.GetHashCode ^ _Line.GetHashCode)
    End Function

    Public Sub New(ByVal name As String, ByVal include As String, ByVal line As Integer, ByVal ParamArray params As String())
        _name = name
        _params = params
        _inc = include
        _Line = line
        _Exist = True
    End Sub

#End Region

End Structure
