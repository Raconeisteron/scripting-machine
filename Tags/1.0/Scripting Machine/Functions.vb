Imports System.Text.RegularExpressions
Imports System.IO
Imports System.Net

Module Functions

#Region "APIs"

    Public Declare Function GetPrivateProfileString Lib "kernel32" Alias "GetPrivateProfileStringA" ( _
        ByVal lpApplicationName As String, ByVal lpKeyName As String, _
        ByVal lpDefault As String, ByVal lpReturnedString As String, _
        ByVal nSize As Integer, ByVal lpFileName As String) As Integer
    Public Declare Function WritePrivateProfileString Lib "kernel32" Alias "WritePrivateProfileStringA" ( _
        ByVal lpApplicationName As String, ByVal lpKeyName As String, _
        ByVal lpString As String, ByVal lpFileName As String) As Integer
    Public Declare Function GetScrollPos Lib "user32" Alias "GetScrollPos" ( _
        ByVal hwnd As Integer, ByVal nBar As Integer) As Integer
    Public Declare Function SetScrollPos Lib "user32" Alias "SetScrollPos" ( _
        ByVal hwnd As Integer, ByVal nBar As Integer, _
        ByVal nPos As Integer, ByVal bRedraw As Integer) As Integer
    Public Declare Function PostMessageA Lib "user32.dll" ( _
            ByVal hwnd As IntPtr, ByVal wMsg As Integer, _
            ByVal wParam As Integer, ByVal lParam As Integer) As Boolean
#End Region

#Region "Enums/Structures"

    Public Enum CC As Integer
        Gate_Open
        Gate_Close
        Help
        Msg
    End Enum

    Public Enum Lang As Integer
        English
        Español
        Portugues
    End Enum

    Public Enum ScaleType As Integer
        Down
        Up
    End Enum

    Public Enum GuiImage
        Button
        Scroll
        TextBox
        Dot
    End Enum

    Public Enum GenderType
        Male
        Female
    End Enum

    Public Enum GangType
        Civilian
        Police
        Grove
        Balla
        Vago
        Azteca
        RussianMafia
        ItalianMafia
        DaNang
        Triad
        Rifa
    End Enum

    Public Enum VehicleType As Integer
        Airplane
        Helicopter
        Bike
        Convertible
        Industrial
        Lowriders
        Off_Road
        Public_Service
        Saloon
        Sport
        Station_Wagon
        Boat
        Trailer
        Unique
        RC
    End Enum

    Public Enum WeaponType
        None
        Pistol
        SubMachine
        Shotgun
        SMG
        Assault
        Rifle
        Thrown
        Special
        White
        Other
    End Enum

    Public Structure Colors
        Public Name As String
        Public Hex As Color
    End Structure

    Public Structure Area
        Public minX As Integer
        Public minY As Integer
        Public maxX As Integer
        Public maxY As Integer
        Public xForward As Boolean
        Public yForward As Boolean
        Public Lock As Boolean
    End Structure

    Public Structure Skin
        Public ID As Integer
        Public Name As String
        Public Gender As GenderType
        Public Gang As GangType
        Public Img As Image
    End Structure

    Public Structure Veh
        Public ID As Integer
        Public Name As String
        Public Img As Image
        Public Type As VehicleType
    End Structure

    Public Structure Weap
        Public ID As Integer
        Public Name As String
        Public Def As String
        Public Slot As Integer
        Public Type As WeaponType
        Public Img As Image
    End Structure

    Public Structure Anim
        Public _Lib As String
        Public Name As String
    End Structure

    Public Structure mIcon
        Public ID As Integer
        Public Name As String
        Public Img As Image
    End Structure

    Public Structure Cfg
        Public C_Msg As Colors
        Public C_Help As Colors
        Public C_Open As Colors
        Public C_Close As Colors
        Public Idioma As Lang
        Public Images As Boolean
        Public URL_Skin As String
        Public URL_Veh As String
        Public URL_Weap As String
        Public URL_Map As String
        Public Clip As String
    End Structure

    Public Structure ObjectS
        Public Model As Integer
        Public X As Double
        Public Y As Double
        Public Z As Double
        Public rX As Double
        Public rY As Double
        Public rZ As Double
    End Structure

#End Region

#Region "Const"

    Public Const SBS_VERT = 1
    Public Const WM_VSCROLL = &H115
    Public Const SB_THUMBPOSITION = 4
    Public Const MAPXY = 6000 / 493

#End Region

#Region "Arrays"

#Region "Public"

    Public gSender As CC
    Public Const BadChars As String = "[^\d\.]+"

#End Region

#Region "Private"

    Dim Web As New WebClient

#End Region

#Region "Resources"

    Public Sounds As New Dictionary(Of String, Integer)
    Public Vehicles(211) As Veh
    Public Skins(283) As Skin
    Public Weapons(43) As Weap
    Public Anims(1724) As Anim
    Public Maps(63) As mIcon

#End Region

#Region "Config"

    Public Config As Cfg

#End Region

#End Region

#Region "Functions"

#Region "INI"

    Private Sub IniDelete(ByVal Path As String, ByVal key As String)
        Call WritePrivateProfileString("Config", key, vbNullString, Path)
    End Sub

    Private Function IniRead(ByVal Path As String, ByVal key As String, Optional ByVal def As String = "")
        Dim ret As Integer
        Dim sRetVal As String
        sRetVal = New String(vbNullChar, 256)
        ret = GetPrivateProfileString("Config", key, def, sRetVal, Len(sRetVal), Path)
        If ret = 0 Then
            Return def
        Else
            Return Left(sRetVal, ret)
        End If
    End Function

    Private Sub IniWrite(ByVal Path As String, ByVal key As String, ByVal Value As String)
        Call WritePrivateProfileString("Config", key, Value, Path)
    End Sub

#End Region

#Region "Color Picker"

    Public Function cColor(ByVal A As Integer, ByVal R As Integer, ByVal G As Integer, ByVal B As Integer, ByRef C As Color) As String
        Dim str As String = "0x"
        C = Color.FromArgb(A, R, G, B)
        If R < 15 Then : str += "0" & R.ToString("X")
        Else : str += R.ToString("X")
        End If
        If G < 15 Then : str += "0" & G.ToString("X")
        Else : str += G.ToString("X")
        End If
        If B < 15 Then : str += "0" & B.ToString("X")
        Else : str += B.ToString("X")
        End If
        If A < 15 Then : str += "0" & A.ToString("X")
        Else : str += A.ToString("X")
        End If
        Return str
    End Function

    Public Function cColor(ByVal A As Integer, ByVal R As Integer, ByVal G As Integer, ByVal B As Integer) As String
        Dim str As String = "0x"
        If R < 15 Then : str += "0" & R.ToString("X")
        Else : str += R.ToString("X")
        End If
        If G < 15 Then : str += "0" & G.ToString("X")
        Else : str += G.ToString("X")
        End If
        If B < 15 Then : str += "0" & B.ToString("X")
        Else : str += B.ToString("X")
        End If
        If A < 15 Then : str += "0" & A.ToString("X")
        Else : str += A.ToString("X")
        End If
        Return str
    End Function

#End Region

#Region "Config"

    Public Sub LoadConfig()
        Splash.Label1.Text = "Loading config..."
        Dim Path As String = My.Application.Info.DirectoryPath & "\Scripting Machine.cfg"
        With Config
            .C_Msg.Hex = Color.FromArgb(IniRead(Path, "MsgC", -1))
            .C_Msg.Name = cColor(.C_Msg.Hex.A, .C_Msg.Hex.R, .C_Msg.Hex.G, .C_Msg.Hex.B)
            .C_Help.Hex = Color.FromArgb(IniRead(Path, "HelpC", -1))
            .C_Help.Name = cColor(.C_Help.Hex.A, .C_Help.Hex.R, .C_Help.Hex.G, .C_Help.Hex.B)
            .C_Open.Hex = Color.FromArgb(IniRead(Path, "OpenC", -1))
            .C_Open.Name = cColor(.C_Open.Hex.A, .C_Open.Hex.R, .C_Open.Hex.G, .C_Open.Hex.B)
            .C_Close.Hex = Color.FromArgb(IniRead(Path, "CloseC", -1))
            .C_Close.Name = cColor(.C_Close.Hex.A, .C_Close.Hex.R, .C_Close.Hex.G, .C_Close.Hex.B)
            .Idioma = LanguajeFromInt(IniRead(Path, "Lang", 0))
            .Images = IntToBool(IniRead(Path, "dImg", 1))
            .URL_Skin = IniRead(Path, "sURL", "http://www.gamerxserver.com/images/Skins/Thumbnails/{0}.jpg")
            .URL_Veh = IniRead(Path, "vURL", "")
            .URL_Weap = IniRead(Path, "wURL", "")
            .URL_Map = IniRead(Path, "mURL", "")
            .Clip = IniRead(Path, "clpfmt", "GangZoneCreate({0}, {1}, {2}, {3});")
            ChangeLang(Config.Idioma)
            Select Case .Idioma
                Case Lang.English
                    Main.RadioButton14.Checked = True
                Case Lang.Español
                    Main.RadioButton15.Checked = True
                Case Else
                    Main.RadioButton16.Checked = True
            End Select
        End With
        With Main
            .CheckBox7.Checked = Config.Images
            .TextBox63.Text = Config.Clip
            .TextBox50.Text = Config.URL_Skin
            .TextBox51.Text = Config.URL_Veh
            .TextBox68.Text = Config.URL_Weap
            .TextBox75.Text = Config.URL_Map
        End With
        Splash.ProgressBar1.Value += 10
    End Sub

    Public Sub SaveConfig()
        Dim Path As String = My.Application.Info.DirectoryPath & "\Scripting Machine.cfg"
        With Config
            IniWrite(Path, "MsgC", .C_Msg.Hex.ToArgb())
            IniWrite(Path, "HelpC", .C_Help.Hex.ToArgb())
            IniWrite(Path, "OpenC", .C_Open.Hex.ToArgb())
            IniWrite(Path, "CloseC", .C_Close.Hex.ToArgb())
            IniWrite(Path, "Lang", LanguajeToInt(.Idioma))
            IniWrite(Path, "dImg", BoolToInt(.Images))
            IniWrite(Path, "clpfmt", .Clip)
            If .Images = True Then
                IniDelete(Path, "sURL")
                IniDelete(Path, "vURL")
                IniDelete(Path, "wURL")
                IniDelete(Path, "mURL")
            Else
                IniWrite(Path, "sURL", .URL_Skin)
                IniWrite(Path, "vURL", .URL_Veh)
                IniWrite(Path, "wURL", .URL_Weap)
                IniWrite(Path, "mURL", .URL_Map)
            End If
        End With
    End Sub

    Public Sub ChangeLang(ByVal Idioma As Lang)
        Select Case Idioma
            Case Lang.English
                With Main
                    .FileToolStripMenuItem.Text = "File"
                    .OtherToolStripMenuItem.Text = "Other"
                    .CloseToolStripMenuItem.Text = "Close"
                    .CreditsToolStripMenuItem1.Text = "Credits"
                    .WebPageToolStripMenuItem.Text = "Web Page"
                    .ForumToolStripMenuItem.Text = "Forum"
                    .MainPageToolStripMenuItem.Text = "Main Page"
                    .SearchForToolStripMenuItem.Text = "Search For"
                    .TabPage2.Text = "Gates"
                    .TabPage5.Text = "Color Picker"
                    .TabPage12.Text = "Settings"
                    .Label2.Text = "Command:"
                    .GroupBox1.Text = "Type"
                    .RadioButton1.Text = "Only Player"
                    .RadioButton2.Text = "Player and Vehicle"
                    .GroupBox4.Text = "Command Processor:"
                    .RadioButton3.Text = "None"
                    .Label1.Text = "Code:"
                    .Label10.Text = "Angle:"
                    .Label10.Left = 150
                    .Label8.Text = "World:"
                    .Label8.Left = 309
                    .CheckBox1.Text = "Send a message"
                    .Label3.Text = "Message:"
                    .Label21.Text = "Help message"
                    .Label4.Text = "Message:"
                    .Button1.Text = "Generate"
                    .GroupBox5.Text = "Type"
                    .RadioButton7.Text = "Command Gate (Diferent commands)"
                    .RadioButton10.Text = "Command Gate (Same command)"
                    .RadioButton8.Text = "Semi Automatic Gate (only open cmd)"
                    .RadioButton9.Text = "Full Automatic Gate"
                    .Label22.Text = "Command (open):"
                    .Label23.Text = "Command (close):"
                    .CheckBox4.Text = "This script is a filterscript"
                    .Label27.Text = "Code:"
                    .GroupBox7.Text = "Config (Open)"
                    .GroupBox8.Text = "Config (Close)"
                    .CheckBox2.Text = "Message (Open):"
                    .Label25.Text = "Message:"
                    .CheckBox3.Text = "Message (Close):"
                    .Label26.Text = "Message:"
                    .Label28.Text = "Speed:"
                    .Label28.Left = 236
                    .Label29.Text = "Close Time (miliseconds):"
                    .Label29.Left = 389
                    .Button4.Text = "Generate"
                    .LinkLabel1.Text = "Model:"
                    .Label32.Text = "World:"
                    .Label34.Text = "Type:"
                    .CheckBox5.Text = "This script is a filterscript"
                    .Button7.Text = "Generate"
                    .Label37.Text = "Dialog ID:"
                    .Label39.Text = "Title:"
                    .Label39.Left = 167
                    .Label38.Text = "Type:"
                    .Label40.Text = "Text:"
                    .Label41.Text = "Button 1:"
                    .Label42.Text = "Button 2:"
                    .Button9.Text = "Preview"
                    .TextBox40.Text = vbNewLine & vbNewLine & "PRESS ENTER TO CREATE A JUMP LINE ON THE TEXT OR" & vbNewLine & "TAB TO CREATE A TABULATION"
                    .CheckBox6.Text = "Define color"
                    .CheckBox8.Text = "Multiple Areas"
                    .CheckBox10.Text = "Fill Areas"
                    .Button17.Text = "Clear Areas"
                    .Button16.Text = "Clear"
                    .Button15.Text = "Copy with format"
                    .Label61.Text = "Objects:"
                    .CheckBox9.Text = "Fix Object ID"
                    .Button14.Text = "Convert"
                    .TabPage8.Text = "Vehicles"
                    .TabPage9.Text = "Sounds"
                    .TabPage11.Text = "Weapons"
                    .TabPage14.Text = "Map Icons"
                    .Label73.Text = "Name:"
                    .Label73.Left = 205
                    .Label74.Text = "Gender:"
                    .Label74.Left = 198
                    .Label75.Text = "Gang:"
                    .Label75.Left = 207
                    .Label51.Text = "Vehicle (ID):"
                    .Label55.Text = "Model ID:"
                    .Label55.Left = 203
                    .Label56.Text = "Name:"
                    .Label56.Left = 218
                    .Label57.Text = "Category:"
                    .Label57.Left = 204
                    .Label52.Text = "Vehicle (Name):"
                    .Label54.Text = "Name:"
                    .Label54.Left = 528
                    .Button12.Text = "Find"
                    .Label58.Text = "Sound:"
                    .Label59.Text = "Sound (ID):"
                    .Label67.Text = "Weapon:"
                    .Label69.Text = "Name:"
                    .Label69.Left = 246
                    .Label70.Text = "Slot:"
                    .Label70.Left = 256
                    .Label71.Text = "Type:"
                    .Label80.Text = "Define:"
                    .Label78.Text = "Map Icon:"
                    .Label76.Text = "Name:"
                    .Label76.Left = 235
                    .GroupBox10.Text = "Languaje"
                    .GroupBox12.Text = "Images"
                    .CheckBox7.Text = "Use default images"
                    .GroupBox14.Text = "Vehicles URL"
                    .GroupBox17.Text = "Weapons URL"
                    .GroupBox11.Text = "Map Icons URL"
                    .GroupBox18.Text = "Area"
                    .Label66.Text = "Clipboard format:"
                    .TextBox63.Left = 98
                    .TextBox63.Width = 545
                    .Button13.Text = "Default"
                    .Button11.Text = "Accept"
                    .Button10.Text = "Cancel"
                End With
                With Srch
                    .Label1.Text = "Search:"
                    .Button1.Text = "Go!"
                End With
            Case Lang.Español
                With Main
                    .FileToolStripMenuItem.Text = "Archivo"
                    .OtherToolStripMenuItem.Text = "Otros"
                    .CloseToolStripMenuItem.Text = "Cerrar"
                    .CreditsToolStripMenuItem1.Text = "Creditos"
                    .WebPageToolStripMenuItem.Text = "Pagina Web"
                    .ForumToolStripMenuItem.Text = "Foro"
                    .MainPageToolStripMenuItem.Text = "Pagina Principal"
                    .SearchForToolStripMenuItem.Text = "Buscar"
                    .TabPage2.Text = "Rejas"
                    .TabPage5.Text = "Color Mixer"
                    .TabPage12.Text = "Configuracion"
                    .Label2.Text = "Commando:"
                    .GroupBox1.Text = "Tipo"
                    .RadioButton1.Text = "Solo jugador"
                    .RadioButton2.Text = "Jugador y vehiculo"
                    .GroupBox4.Text = "Procesador de comandos:"
                    .RadioButton3.Text = "Ninguno"
                    .Label1.Text = "Codigo:"
                    .Label10.Text = "Angulo:"
                    .Label10.Left = 144
                    .Label8.Text = "Mundo:"
                    .Label8.Left = 304
                    .CheckBox1.Text = "Enviar un mensaje"
                    .Label3.Text = "Mensaje:"
                    .Label21.Text = "Mensaje de ayuda"
                    .Label4.Text = "Mensaje:"
                    .Button1.Text = "Generar"
                    .GroupBox5.Text = "Tipo"
                    .RadioButton7.Text = "Reja por comandos (diferentes cmds)"
                    .RadioButton10.Text = "Reja por comandos (solo un cmd)"
                    .RadioButton8.Text = "Reja semi automatica (cmd para abrir)"
                    .RadioButton9.Text = "Reja automatica"
                    .Label22.Text = "Comando (abrir):"
                    .Label23.Text = "Comando (cerrar):"
                    .CheckBox4.Text = "Este script es un filterscript"
                    .Label27.Text = "Codigo:"
                    .GroupBox7.Text = "Config (Abrir)"
                    .GroupBox8.Text = "Config (Cerrar)"
                    .CheckBox2.Text = "Message (Abrir):"
                    .Label25.Text = "Mensaje:"
                    .CheckBox3.Text = "Message (Cerrar):"
                    .Label26.Text = "Mensaje:"
                    .Label28.Text = "Velocidad:"
                    .Label28.Left = 220
                    .Label29.Text = "Tiempo de cerrado (miliseconds):"
                    .Label29.Left = 352
                    .Button4.Text = "Generar"
                    .LinkLabel1.Text = "Modelo:"
                    .Label32.Text = "Mundo:"
                    .Label34.Text = "Tipo:"
                    .CheckBox5.Text = "Este script es un filterscript"
                    .Button7.Text = "Generar"
                    .Label37.Text = "Dialogo ID:"
                    .Label39.Text = "Titulo:"
                    .Label39.Left = 161
                    .Label38.Text = "Tipo:"
                    .Label40.Text = "Texto:"
                    .Label41.Text = "Boton 1:"
                    .Label42.Text = "Boton 2:"
                    .Button9.Text = "Previsualizar"
                    .Button9.Text = "Generar"
                    .TextBox40.Text = vbNewLine & vbNewLine & "PRESIONA ENTER PARA CREAR UN SALTO DE LINEA EN EL TEXTO O" & vbNewLine & "TAB PARA CREAR UNA TABULACIÓN"
                    .CheckBox6.Text = "Define color"
                    .CheckBox8.Text = "Areas Multiples"
                    .CheckBox10.Text = "Llenar Areas"
                    .Button17.Text = "Limpiar Areas"
                    .Button16.Text = "Limpiar"
                    .Button15.Text = "Copiar con formato"
                    .Label61.Text = "Objetos:"
                    .CheckBox9.Text = "Arreglar las IDs"
                    .Button14.Text = "Convertir"
                    .TabPage8.Text = "Vehiculos"
                    .TabPage9.Text = "Sonidos"
                    .TabPage11.Text = "Armas"
                    .TabPage14.Text = "Iconos de Mapa"
                    .Label73.Text = "Nombre:"
                    .Label73.Left = 196
                    .Label74.Text = "Genero:"
                    .Label74.Left = 198
                    .Label75.Text = "Clan:"
                    .Label75.Left = 212
                    .Label51.Text = "Vehiculo (ID):"
                    .Label55.Text = "Modelo ID:"
                    .Label55.Left = 109
                    .Label56.Text = "Nombre:"
                    .Label56.Left = 209
                    .Label57.Text = "Categoria:"
                    .Label55.Left = 201
                    .Label52.Text = "Vehiculo (Nombre):"
                    .Label54.Text = "Nombre:"
                    .Label54.Left = 522
                    .Button12.Text = "Buscar"
                    .Label58.Text = "Sonido:"
                    .Label59.Text = "Sonido (ID):"
                    .Label67.Text = "Arma:"
                    .Label69.Text = "Nombre:"
                    .Label69.Left = 237
                    .Label70.Text = "Ranura:"
                    .Label70.Left = 239
                    .Label71.Text = "Tipo:"
                    .Label80.Text = "Define:"
                    .Label78.Text = "Icono de Mapa:"
                    .Label76.Text = "Nombre:"
                    .Label76.Left = 226
                    .GroupBox10.Text = "Idioma"
                    .GroupBox12.Text = "Imagenes"
                    .CheckBox7.Text = "Usar imagenes por defecto"
                    .GroupBox14.Text = "Vehiculos URL"
                    .GroupBox17.Text = "Armas URL"
                    .GroupBox11.Text = "Iconos de Mapa URL"
                    .GroupBox18.Text = "Area"
                    .Label66.Text = "Formato de clipboard:"
                    .TextBox63.Left = 121
                    .TextBox63.Width = 431
                    .Button13.Text = "Defecto"
                    .Button11.Text = "Aceptar"
                    .Button10.Text = "Cancelar"
                End With
                With Srch
                    .Label1.Text = "Buscar:"
                    .Button1.Text = "Ir!"
                End With
            Case Else
                With Main
                    .FileToolStripMenuItem.Text = "Arquivo"
                    .OtherToolStripMenuItem.Text = "Outros"
                    .CloseToolStripMenuItem.Text = "Fechar"
                    .CreditsToolStripMenuItem1.Text = "Creditos"
                    .WebPageToolStripMenuItem.Text = "Pagina Web"
                    .ForumToolStripMenuItem.Text = "Foro"
                    .MainPageToolStripMenuItem.Text = "Pagina Principal"
                    .SearchForToolStripMenuItem.Text = "Pesquisa"
                    .TabPage2.Text = "Fences"
                    .TabPage5.Text = "Color Mixer"
                    .TabPage12.Text = "Configuração"
                    .Label2.Text = "Comando:"
                    .GroupBox1.Text = "Tipo"
                    .RadioButton1.Text = "Unico jogador"
                    .RadioButton2.Text = "Jogador e veículo"
                    .GroupBox4.Text = "Procesador de comandos:"
                    .RadioButton3.Text = "Nenhum"
                    .Label1.Text = "Codigo:"
                    .Label10.Text = "Angulo:"
                    .Label10.Left = 144
                    .Label8.Text = "Mundo:"
                    .Label8.Left = 304
                    .CheckBox1.Text = "Enviar uma mensagem"
                    .Label3.Text = "Mensagem:"
                    .Label21.Text = "Mensagem de ajuda"
                    .Label4.Text = "Mensagem:"
                    .Button1.Text = "Gerar"
                    .GroupBox5.Text = "Tipo"
                    .RadioButton7.Text = "Cmd Fence (diferente cmds)"
                    .RadioButton10.Text = "Cmd Fence(cmd só)"
                    .RadioButton8.Text = "Fence semi automatica (cmd para abrir)"
                    .RadioButton9.Text = "Fence automática"
                    .Label22.Text = "Comando (abeto):"
                    .Label23.Text = "Comando (fechar):"
                    .CheckBox4.Text = "Este script é um filterscript"
                    .Label27.Text = "Codigo:"
                    .GroupBox7.Text = "Config (aberto)"
                    .GroupBox8.Text = "Config (fechar)"
                    .CheckBox2.Text = "Mensagem (aberto):"
                    .Label25.Text = "Mensagem:"
                    .CheckBox3.Text = "Mensagem (fechar):"
                    .Label26.Text = "Mensagem:"
                    .Label28.Text = "Velocidade:"
                    .Label28.Left = 220
                    .Label29.Text = "Tempo de fechamento (miliseconds):"
                    .Label29.Left = 348
                    .Button4.Text = "Gerar"
                    .LinkLabel1.Text = "Modelo:"
                    .Label32.Text = "Mundo:"
                    .Label34.Text = "Tipo:"
                    .CheckBox5.Text = "Este script é um filterscript"
                    .Button7.Text = "Gerar"
                    .Label37.Text = "Dialogo ID:"
                    .Label39.Text = "Titulo:"
                    .Label39.Left = 161
                    .Label38.Text = "Tipo:"
                    .Label40.Text = "Texto:"
                    .Label41.Text = "Botão 1:"
                    .Label42.Text = "Botão 2:"
                    .Button9.Text = "Visualização"
                    .Button9.Text = "Gerar"
                    .TextBox40.Text = vbNewLine & vbNewLine & "PRESSIONE ENTER PARA CRIAR UM SALTO DE LINHA NO TEXTO OU" & vbNewLine & "TAB PARA CRIAR UMA TABULAÇÃO"
                    .CheckBox6.Text = "Define color"
                    .CheckBox8.Text = "Areas Multiplas"
                    .CheckBox10.Text = "Preencha Areas"
                    .Button17.Text = "Limpar Areas"
                    .Button16.Text = "Limpo"
                    .Button15.Text = "Formato de cópia"
                    .Label61.Text = "Objetos:"
                    .CheckBox9.Text = "Fixar as IDs"
                    .Button14.Text = "Converter"
                    .TabPage8.Text = "Veiculos"
                    .TabPage9.Text = "Sons"
                    .TabPage11.Text = "Armas"
                    .TabPage14.Text = "Icones mapa"
                    .Label73.Text = "Nome:"
                    .Label73.Left = 196
                    .Label74.Text = "Gênero:"
                    .Label74.Left = 198
                    .Label75.Text = "Clã:"
                    .Label75.Left = 212
                    .Label51.Text = "Veiculo (ID):"
                    .Label55.Text = "Modelo ID:"
                    .Label55.Left = 109
                    .Label56.Text = "Nome:"
                    .Label56.Left = 209
                    .Label57.Text = "Categoria:"
                    .Label55.Left = 201
                    .Label52.Text = "Veiculo (Nome):"
                    .Label54.Text = "Nome:"
                    .Label54.Left = 522
                    .Button12.Text = "Pesquisar"
                    .Label58.Text = "Som:"
                    .Label59.Text = "Som (ID):"
                    .Label67.Text = "Arma:"
                    .Label69.Text = "Nome:"
                    .Label69.Left = 237
                    .Label70.Text = "ranhura"
                    .Label70.Left = 239
                    .Label71.Text = "Tipo:"
                    .Label80.Text = "Definir"
                    .Label78.Text = "icon Mapa:"
                    .Label76.Text = "Nome"
                    .Label76.Left = 226
                    .GroupBox10.Text = "Idioma"
                    .GroupBox12.Text = "Imagens"
                    .CheckBox7.Text = "default imagens"
                    .GroupBox14.Text = "URL Veículos"
                    .GroupBox17.Text = "URL Armas"
                    .GroupBox11.Text = "URL ícone do mapa"
                    .GroupBox18.Text = "Area"
                    .Label66.Text = "formato de prancheta"
                    .TextBox63.Left = 121
                    .TextBox63.Width = 431
                    .Button13.Text = "Default"
                    .Button11.Text = "OK"
                    .Button10.Text = "Cancelar"
                End With
                With Srch
                    .Label1.Text = "Pesquisar"
                    .Button1.Text = "Ir!"
                End With
        End Select
    End Sub

#End Region

#Region "Text"

    Public Function ProcessText(ByVal Text As String) As String
        Dim str As String = vbNullString, _
            tmp As String()
        tmp = Split(Text, "\t")
        For i = LBound(tmp) To UBound(tmp)
            str += tmp(i) & vbTab
        Next
        tmp = Split(str, "\n")
        str = vbNullString
        For i = LBound(tmp) To UBound(tmp)
            str += tmp(i) & vbNewLine
        Next
        Return str
    End Function

    Public Sub ProcessColor(ByVal Rich As RichTextBox)
        Dim istart As Integer, iend As Integer, cColor As String
        With Rich
            istart = .Text.IndexOf("{")
            If istart = -1 Then
                iend = .Text.IndexOf("}")
            Else
                iend = .Text.IndexOf("}", istart)
            End If
            While istart > -1 And iend > -1
                .SelectionStart = istart + 1
                .SelectionLength = iend - istart - 1
                cColor = .SelectedText
                .SelectionStart = iend
                If .Text.IndexOf("{", iend) > -1 Then
                    .SelectionLength = .Text.IndexOf("{", iend) - iend
                Else
                    .SelectionLength = .Text.Length - iend
                End If
                .SelectionColor = Color.FromArgb(Integer.Parse(cColor, Globalization.NumberStyles.HexNumber))
                .SelectionStart = istart
                .SelectionLength = iend - istart + 1
                .ReadOnly = False
                .SelectedText = ""
                .ReadOnly = True
                If iend < .Text.Length Then
                    istart = .Text.IndexOf("{", istart + 1)
                    iend = .Text.IndexOf("}", iend + 1)
                Else
                    Exit While
                End If
            End While
        End With
    End Sub

    Public Function Counter(ByVal Text As String, ByVal Patern As String) As Integer
        Dim istart As Integer, iend As Integer, count As Integer
        istart = Text.IndexOf(Patern)
        While istart >= 0
            count += 1
            iend = istart + 1
            istart = Text.IndexOf(Patern, iend)
        End While
        Return count
    End Function

    Public Function GetLine(ByVal Text As String, ByVal Line As Integer) As String
        Dim str As String, tmp As String()
        str = vbNullString
        If Text.IndexOf("\n") >= 0 Then
            tmp = Split(Text, "\n")
            For i = LBound(tmp) To UBound(tmp)
                If i = Line Then str = tmp(i)
            Next
        Else : str = Text
        End If
        Return str
    End Function

    Public Function GetCharCountFromLongestLine(ByVal Text As RichTextBox) As Integer
        Dim count As Integer
        For Each line In Text.Lines
            If count < line.Length Then
                count = line.Length
            End If
        Next
        Return count
    End Function

#End Region

#Region "Skins"

    Public Sub FillSkins()
        FillSkinsList()
        Main.TreeView1.Nodes.Add("Men")
        Main.TreeView1.Nodes.Add("Women")
        Main.TreeView1.Nodes.Add("Gang")
        Main.TreeView1.Nodes(2).Nodes.Add("Civil")
        Main.TreeView1.Nodes(2).Nodes.Add("Police")
        Main.TreeView1.Nodes(2).Nodes.Add("Grove")
        Main.TreeView1.Nodes(2).Nodes.Add("Balla")
        Main.TreeView1.Nodes(2).Nodes.Add("Vagos")
        Main.TreeView1.Nodes(2).Nodes.Add("Aztecas")
        Main.TreeView1.Nodes(2).Nodes.Add("Russian Mafia")
        Main.TreeView1.Nodes(2).Nodes.Add("Italian Mafia")
        Main.TreeView1.Nodes(2).Nodes.Add("SF Rifa")
        Main.TreeView1.Nodes(2).Nodes.Add("DaNang")
        Main.TreeView1.Nodes(2).Nodes.Add("Triad")
        Main.TreeView1.Nodes.Add("All")
        Dim count As Integer
        For Each Skin In Skins
            Select Case Skin.Gender
                Case GenderType.Male
                    Main.TreeView1.Nodes(0).Nodes.Add(Skin.ID)
                Case Else
                    Main.TreeView1.Nodes(1).Nodes.Add(Skin.ID)
            End Select
            Select Case Skin.Gang
                Case GangType.Civilian
                    Main.TreeView1.Nodes(2).Nodes(0).Nodes.Add(Skin.ID)
                Case GangType.Police
                    Main.TreeView1.Nodes(2).Nodes(1).Nodes.Add(Skin.ID)
                Case GangType.Grove
                    Main.TreeView1.Nodes(2).Nodes(2).Nodes.Add(Skin.ID)
                Case GangType.Balla
                    Main.TreeView1.Nodes(2).Nodes(3).Nodes.Add(Skin.ID)
                Case GangType.Vago
                    Main.TreeView1.Nodes(2).Nodes(4).Nodes.Add(Skin.ID)
                Case GangType.Azteca
                    Main.TreeView1.Nodes(2).Nodes(5).Nodes.Add(Skin.ID)
                Case GangType.RussianMafia
                    Main.TreeView1.Nodes(2).Nodes(6).Nodes.Add(Skin.ID)
                Case GangType.ItalianMafia
                    Main.TreeView1.Nodes(2).Nodes(7).Nodes.Add(Skin.ID)
                Case GangType.DaNang
                    Main.TreeView1.Nodes(2).Nodes(9).Nodes.Add(Skin.ID)
                Case GangType.Triad
                    Main.TreeView1.Nodes(2).Nodes(10).Nodes.Add(Skin.ID)
                Case GangType.Rifa
                    Main.TreeView1.Nodes(2).Nodes(8).Nodes.Add(Skin.ID)
            End Select
            Main.TreeView1.Nodes(3).Nodes.Add(Skin.ID)
            Splash.Label1.Text = String.Format("Loading skins ({0}/285)", count)
            Splash.ProgressBar1.Value += 1
            count += 1
        Next
    End Sub

    Private Sub FillSkinsList()
        Splash.Label1.Text = "Loading skins..."
        Skins(0).ID = 0
        Skins(0).Name = "Carl Johnson (CJ)"
        Skins(0).Gender = GenderType.Male
        Skins(0).Gang = GangType.Civilian
        Skins(0).Img = My.Resources.Skin_0
        Skins(1).ID = 1
        Skins(1).Name = "The Truth"
        Skins(1).Gender = GenderType.Male
        Skins(1).Gang = GangType.Civilian
        Skins(1).Img = My.Resources.Skin_1
        Skins(2).ID = 2
        Skins(2).Name = "Maccer"
        Skins(2).Gender = GenderType.Male
        Skins(2).Gang = GangType.Civilian
        Skins(2).Img = My.Resources.Skin_2
        Skins(3).ID = 7
        Skins(3).Name = "Taxi Driver/Train Driver"
        Skins(3).Gender = GenderType.Male
        Skins(3).Gang = GangType.Civilian
        Skins(3).Img = My.Resources.Skin_7
        Skins(4).ID = 9
        Skins(4).Name = "Normal Ped"
        Skins(4).Gender = GenderType.Female
        Skins(4).Gang = GangType.Civilian
        Skins(4).Img = My.Resources.Skin_9
        Skins(5).ID = 10
        Skins(5).Name = "Normal Ped"
        Skins(5).Gender = GenderType.Female
        Skins(5).Gang = GangType.Civilian
        Skins(5).Img = My.Resources.Skin_10
        Skins(6).ID = 11
        Skins(6).Name = "Casino croupier"
        Skins(6).Gender = GenderType.Female
        Skins(6).Gang = GangType.Civilian
        Skins(6).Img = My.Resources.Skin_11
        Skins(7).ID = 12
        Skins(7).Name = "Normal Ped"
        Skins(7).Gender = GenderType.Female
        Skins(7).Gang = GangType.Civilian
        Skins(7).Img = My.Resources.Skin_12
        Skins(8).ID = 13
        Skins(8).Name = "Normal Ped"
        Skins(8).Gender = GenderType.Female
        Skins(8).Gang = GangType.Civilian
        Skins(8).Img = My.Resources.Skin_13
        Skins(9).ID = 14
        Skins(9).Name = "Normal Ped"
        Skins(9).Gender = GenderType.Male
        Skins(9).Gang = GangType.Civilian
        Skins(9).Img = My.Resources.Skin_14
        Skins(10).ID = 15
        Skins(10).Name = "RS Haul Owner"
        Skins(10).Gender = GenderType.Male
        Skins(10).Gang = GangType.Civilian
        Skins(10).Img = My.Resources.Skin_15
        Skins(11).ID = 16
        Skins(11).Name = "Airport Ground Worker"
        Skins(11).Gender = GenderType.Male
        Skins(11).Gang = GangType.Civilian
        Skins(11).Img = My.Resources.Skin_16
        Skins(12).ID = 17
        Skins(12).Name = "Buisnessman"
        Skins(12).Gender = GenderType.Male
        Skins(12).Gang = GangType.Civilian
        Skins(12).Img = My.Resources.Skin_17
        Skins(13).ID = 18
        Skins(13).Name = "Beach Visitor"
        Skins(13).Gender = GenderType.Male
        Skins(13).Gang = GangType.Civilian
        Skins(13).Img = My.Resources.Skin_18
        Skins(14).ID = 19
        Skins(14).Name = "DJ"
        Skins(14).Gender = GenderType.Male
        Skins(14).Gang = GangType.Civilian
        Skins(14).Img = My.Resources.Skin_19
        Skins(15).ID = 20
        Skins(15).Name = "Madd Dogg's Manager"
        Skins(15).Gender = GenderType.Male
        Skins(15).Gang = GangType.Civilian
        Skins(15).Img = My.Resources.Skin_20
        Skins(16).ID = 21
        Skins(16).Name = "Normal Ped"
        Skins(16).Gender = GenderType.Male
        Skins(16).Gang = GangType.Civilian
        Skins(16).Img = My.Resources.Skin_21
        Skins(17).ID = 22
        Skins(17).Name = "Normal Ped"
        Skins(17).Gender = GenderType.Male
        Skins(17).Gang = GangType.Civilian
        Skins(17).Img = My.Resources.Skin_22
        Skins(18).ID = 23
        Skins(18).Name = "BMXer"
        Skins(18).Gender = GenderType.Male
        Skins(18).Gang = GangType.Civilian
        Skins(18).Img = My.Resources.Skin_23
        Skins(19).ID = 24
        Skins(19).Name = "Madd Dogg Bodyguard"
        Skins(19).Gender = GenderType.Male
        Skins(19).Gang = GangType.Civilian
        Skins(19).Img = My.Resources.Skin_24
        Skins(20).ID = 25
        Skins(20).Name = "Madd Dogg Bodyguard"
        Skins(20).Gender = GenderType.Male
        Skins(20).Gang = GangType.Civilian
        Skins(20).Img = My.Resources.Skin_25
        Skins(21).ID = 26
        Skins(21).Name = "Backpacker"
        Skins(21).Gender = GenderType.Male
        Skins(21).Gang = GangType.Civilian
        Skins(21).Img = My.Resources.Skin_26
        Skins(22).ID = 27
        Skins(22).Name = "Builder"
        Skins(22).Gender = GenderType.Male
        Skins(22).Gang = GangType.Civilian
        Skins(22).Img = My.Resources.Skin_27
        Skins(23).ID = 28
        Skins(23).Name = "Drug Dealer"
        Skins(23).Gender = GenderType.Male
        Skins(23).Gang = GangType.Civilian
        Skins(23).Img = My.Resources.Skin_28
        Skins(24).ID = 29
        Skins(24).Name = "Drug Dealer"
        Skins(24).Gender = GenderType.Male
        Skins(24).Gang = GangType.Civilian
        Skins(24).Img = My.Resources.Skin_29
        Skins(25).ID = 30
        Skins(25).Name = "Drug Dealer"
        Skins(25).Gender = GenderType.Male
        Skins(25).Gang = GangType.Civilian
        Skins(25).Img = My.Resources.Skin_30
        Skins(26).ID = 31
        Skins(26).Name = "Farm-Town inhabitant"
        Skins(26).Gender = GenderType.Female
        Skins(26).Gang = GangType.Civilian
        Skins(26).Img = My.Resources.Skin_31
        Skins(27).ID = 32
        Skins(27).Name = "Farm-Town inhabitant"
        Skins(27).Gender = GenderType.Male
        Skins(27).Gang = GangType.Civilian
        Skins(27).Img = My.Resources.Skin_32
        Skins(28).ID = 33
        Skins(28).Name = "Farm-Town inhabitant"
        Skins(28).Gender = GenderType.Male
        Skins(28).Gang = GangType.Civilian
        Skins(28).Img = My.Resources.Skin_33
        Skins(29).ID = 34
        Skins(29).Name = "Farm-Town inhabitant"
        Skins(29).Gender = GenderType.Male
        Skins(29).Gang = GangType.Civilian
        Skins(29).Img = My.Resources.Skin_34
        Skins(30).ID = 35
        Skins(30).Name = "Normal Ped"
        Skins(30).Gender = GenderType.Male
        Skins(30).Gang = GangType.Civilian
        Skins(30).Img = My.Resources.Skin_35
        Skins(31).ID = 36
        Skins(31).Name = "Golfer"
        Skins(31).Gender = GenderType.Male
        Skins(31).Gang = GangType.Civilian
        Skins(31).Img = My.Resources.Skin_36
        Skins(32).ID = 37
        Skins(32).Name = "Golfer"
        Skins(32).Gender = GenderType.Male
        Skins(32).Gang = GangType.Civilian
        Skins(32).Img = My.Resources.Skin_37
        Skins(33).ID = 38
        Skins(33).Name = "Normal Ped"
        Skins(33).Gender = GenderType.Male
        Skins(33).Gang = GangType.Civilian
        Skins(33).Img = My.Resources.Skin_38
        Skins(34).ID = 39
        Skins(34).Name = "Normal Ped"
        Skins(34).Gender = GenderType.Female
        Skins(34).Gang = GangType.Civilian
        Skins(34).Img = My.Resources.Skin_39
        Skins(35).ID = 40
        Skins(35).Name = "Normal Ped"
        Skins(35).Gender = GenderType.Female
        Skins(35).Gang = GangType.Civilian
        Skins(35).Img = My.Resources.Skin_40
        Skins(36).ID = 41
        Skins(36).Name = "Normal Ped"
        Skins(36).Gender = GenderType.Female
        Skins(36).Gang = GangType.Civilian
        Skins(36).Img = My.Resources.Skin_41
        Skins(37).ID = 43
        Skins(37).Name = "Normal Ped"
        Skins(37).Gender = GenderType.Male
        Skins(37).Gang = GangType.Civilian
        Skins(37).Img = My.Resources.Skin_43
        Skins(38).ID = 44
        Skins(38).Name = "Normal Ped"
        Skins(38).Gender = GenderType.Male
        Skins(38).Gang = GangType.Civilian
        Skins(38).Img = My.Resources.Skin_44
        Skins(39).ID = 45
        Skins(39).Name = "Beach Visitor"
        Skins(39).Gender = GenderType.Male
        Skins(39).Gang = GangType.Civilian
        Skins(39).Img = My.Resources.Skin_45
        Skins(40).ID = 46
        Skins(40).Name = "Normal Ped"
        Skins(40).Gender = GenderType.Male
        Skins(40).Gang = GangType.Civilian
        Skins(40).Img = My.Resources.Skin_46
        Skins(41).ID = 47
        Skins(41).Name = "Normal Ped"
        Skins(41).Gender = GenderType.Male
        Skins(41).Gang = GangType.Civilian
        Skins(41).Img = My.Resources.Skin_47
        Skins(42).ID = 48
        Skins(42).Name = "Normal Ped"
        Skins(42).Gender = GenderType.Male
        Skins(42).Gang = GangType.Civilian
        Skins(42).Img = My.Resources.Skin_48
        Skins(43).ID = 49
        Skins(43).Name = "Snakehead (Da Nang)"
        Skins(43).Gender = GenderType.Male
        Skins(43).Gang = GangType.Civilian
        Skins(43).Img = My.Resources.Skin_49
        Skins(44).ID = 50
        Skins(44).Name = "Mechanic"
        Skins(44).Gender = GenderType.Male
        Skins(44).Gang = GangType.Civilian
        Skins(44).Img = My.Resources.Skin_50
        Skins(45).ID = 51
        Skins(45).Name = "Mountain Biker"
        Skins(45).Gender = GenderType.Male
        Skins(45).Gang = GangType.Civilian
        Skins(45).Img = My.Resources.Skin_51
        Skins(46).ID = 52
        Skins(46).Name = "Mountain Biker"
        Skins(46).Gender = GenderType.Male
        Skins(46).Gang = GangType.Civilian
        Skins(46).Img = My.Resources.Skin_52
        Skins(47).ID = 54
        Skins(47).Name = "Normal Ped"
        Skins(47).Gender = GenderType.Female
        Skins(47).Gang = GangType.Civilian
        Skins(47).Img = My.Resources.Skin_54
        Skins(48).ID = 55
        Skins(48).Name = "Normal Ped"
        Skins(48).Gender = GenderType.Female
        Skins(48).Gang = GangType.Civilian
        Skins(48).Img = My.Resources.Skin_55
        Skins(49).ID = 56
        Skins(49).Name = "Normal Ped"
        Skins(49).Gender = GenderType.Female
        Skins(49).Gang = GangType.Civilian
        Skins(49).Img = My.Resources.Skin_56
        Skins(50).ID = 57
        Skins(50).Name = "Feds"
        Skins(50).Gender = GenderType.Male
        Skins(50).Gang = GangType.Civilian
        Skins(50).Img = My.Resources.Skin_57
        Skins(51).ID = 58
        Skins(51).Name = "Normal Ped"
        Skins(51).Gender = GenderType.Male
        Skins(51).Gang = GangType.Civilian
        Skins(51).Img = My.Resources.Skin_58
        Skins(52).ID = 59
        Skins(52).Name = "Normal Ped"
        Skins(52).Gender = GenderType.Male
        Skins(52).Gang = GangType.Civilian
        Skins(52).Img = My.Resources.Skin_59
        Skins(53).ID = 60
        Skins(53).Name = "Normal Ped"
        Skins(53).Gender = GenderType.Male
        Skins(53).Gang = GangType.Civilian
        Skins(53).Img = My.Resources.Skin_60
        Skins(54).ID = 61
        Skins(54).Name = "Pilot"
        Skins(54).Gender = GenderType.Male
        Skins(54).Gang = GangType.Civilian
        Skins(54).Img = My.Resources.Skin_61
        Skins(55).ID = 62
        Skins(55).Name = "Colonel Fuhrberger"
        Skins(55).Gender = GenderType.Male
        Skins(55).Gang = GangType.Civilian
        Skins(55).Img = My.Resources.Skin_62
        Skins(56).ID = 63
        Skins(56).Name = "Prostitute"
        Skins(56).Gender = GenderType.Female
        Skins(56).Gang = GangType.Civilian
        Skins(56).Img = My.Resources.Skin_63
        Skins(57).ID = 64
        Skins(57).Name = "Prostitute"
        Skins(57).Gender = GenderType.Female
        Skins(57).Gang = GangType.Civilian
        Skins(57).Img = My.Resources.Skin_64
        Skins(58).ID = 66
        Skins(58).Name = "Pool Player"
        Skins(58).Gender = GenderType.Male
        Skins(58).Gang = GangType.Civilian
        Skins(58).Img = My.Resources.Skin_66
        Skins(59).ID = 67
        Skins(59).Name = "Pool Player"
        Skins(59).Gender = GenderType.Male
        Skins(59).Gang = GangType.Civilian
        Skins(59).Img = My.Resources.Skin_67
        Skins(60).ID = 68
        Skins(60).Name = "Priest"
        Skins(60).Gender = GenderType.Male
        Skins(60).Gang = GangType.Civilian
        Skins(60).Img = My.Resources.Skin_68
        Skins(61).ID = 69
        Skins(61).Name = "Normal Ped"
        Skins(61).Gender = GenderType.Female
        Skins(61).Gang = GangType.Civilian
        Skins(61).Img = My.Resources.Skin_69
        Skins(62).ID = 70
        Skins(62).Name = "Scientist"
        Skins(62).Gender = GenderType.Male
        Skins(62).Gang = GangType.Civilian
        Skins(62).Img = My.Resources.Skin_70
        Skins(63).ID = 71
        Skins(63).Name = "Security Guard"
        Skins(63).Gender = GenderType.Male
        Skins(63).Gang = GangType.Civilian
        Skins(63).Img = My.Resources.Skin_71
        Skins(64).ID = 72
        Skins(64).Name = "Hippy"
        Skins(64).Gender = GenderType.Male
        Skins(64).Gang = GangType.Civilian
        Skins(64).Img = My.Resources.Skin_72
        Skins(65).ID = 73
        Skins(65).Name = "Hippy (Jethro)"
        Skins(65).Gender = GenderType.Male
        Skins(65).Gang = GangType.Civilian
        Skins(65).Img = My.Resources.Skin_73
        Skins(66).ID = 75
        Skins(66).Name = "Prostitute"
        Skins(66).Gender = GenderType.Female
        Skins(66).Gang = GangType.Civilian
        Skins(66).Img = My.Resources.Skin_75
        Skins(67).ID = 76
        Skins(67).Name = "Normal Ped"
        Skins(67).Gender = GenderType.Female
        Skins(67).Gang = GangType.Civilian
        Skins(67).Img = My.Resources.Skin_76
        Skins(68).ID = 77
        Skins(68).Name = "Homeless"
        Skins(68).Gender = GenderType.Female
        Skins(68).Gang = GangType.Civilian
        Skins(68).Img = My.Resources.Skin_77
        Skins(69).ID = 78
        Skins(69).Name = "Homeless"
        Skins(69).Gender = GenderType.Male
        Skins(69).Gang = GangType.Civilian
        Skins(69).Img = My.Resources.Skin_78
        Skins(70).ID = 79
        Skins(70).Name = "Homeless"
        Skins(70).Gender = GenderType.Male
        Skins(70).Gang = GangType.Civilian
        Skins(70).Img = My.Resources.Skin_79
        Skins(71).ID = 80
        Skins(71).Name = "Boxer"
        Skins(71).Gender = GenderType.Male
        Skins(71).Gang = GangType.Civilian
        Skins(71).Img = My.Resources.Skin_80
        Skins(72).ID = 81
        Skins(72).Name = "Boxer"
        Skins(72).Gender = GenderType.Male
        Skins(72).Gang = GangType.Civilian
        Skins(72).Img = My.Resources.Skin_81
        Skins(73).ID = 82
        Skins(73).Name = "Black Elvis"
        Skins(73).Gender = GenderType.Male
        Skins(73).Gang = GangType.Civilian
        Skins(73).Img = My.Resources.Skin_82
        Skins(74).ID = 83
        Skins(74).Name = "White Elvis"
        Skins(74).Gender = GenderType.Male
        Skins(74).Gang = GangType.Civilian
        Skins(74).Img = My.Resources.Skin_83
        Skins(75).ID = 84
        Skins(75).Name = "Blue Elvis"
        Skins(75).Gender = GenderType.Male
        Skins(75).Gang = GangType.Civilian
        Skins(75).Img = My.Resources.Skin_84
        Skins(76).ID = 85
        Skins(76).Name = "Prostitute"
        Skins(76).Gender = GenderType.Female
        Skins(76).Gang = GangType.Civilian
        Skins(76).Img = My.Resources.Skin_85
        Skins(77).ID = 87
        Skins(77).Name = "Stripper"
        Skins(77).Gender = GenderType.Female
        Skins(77).Gang = GangType.Civilian
        Skins(77).Img = My.Resources.Skin_87
        Skins(78).ID = 88
        Skins(78).Name = "Normal Ped"
        Skins(78).Gender = GenderType.Female
        Skins(78).Gang = GangType.Civilian
        Skins(78).Img = My.Resources.Skin_88
        Skins(79).ID = 89
        Skins(79).Name = "Normal Ped"
        Skins(79).Gender = GenderType.Female
        Skins(79).Gang = GangType.Civilian
        Skins(79).Img = My.Resources.Skin_89
        Skins(80).ID = 90
        Skins(80).Name = "Jogger"
        Skins(80).Gender = GenderType.Female
        Skins(80).Gang = GangType.Civilian
        Skins(80).Img = My.Resources.Skin_90
        Skins(81).ID = 92
        Skins(81).Name = "Rollerskater"
        Skins(81).Gender = GenderType.Female
        Skins(81).Gang = GangType.Civilian
        Skins(81).Img = My.Resources.Skin_92
        Skins(82).ID = 93
        Skins(82).Name = "Normal Ped"
        Skins(82).Gender = GenderType.Female
        Skins(82).Gang = GangType.Civilian
        Skins(82).Img = My.Resources.Skin_93
        Skins(83).ID = 94
        Skins(83).Name = "Normal Ped"
        Skins(83).Gender = GenderType.Male
        Skins(83).Gang = GangType.Civilian
        Skins(83).Img = My.Resources.Skin_94
        Skins(84).ID = 95
        Skins(84).Name = "Normal Ped"
        Skins(84).Gender = GenderType.Male
        Skins(84).Gang = GangType.Civilian
        Skins(84).Img = My.Resources.Skin_95
        Skins(85).ID = 96
        Skins(85).Name = "Jogger"
        Skins(85).Gender = GenderType.Male
        Skins(85).Gang = GangType.Civilian
        Skins(85).Img = My.Resources.Skin_96
        Skins(86).ID = 97
        Skins(86).Name = "Lifeguard"
        Skins(86).Gender = GenderType.Male
        Skins(86).Gang = GangType.Civilian
        Skins(86).Img = My.Resources.Skin_97
        Skins(87).ID = 98
        Skins(87).Name = "Normal Ped"
        Skins(87).Gender = GenderType.Male
        Skins(87).Gang = GangType.Civilian
        Skins(87).Img = My.Resources.Skin_98
        Skins(88).ID = 99
        Skins(88).Name = "Rollerskater"
        Skins(88).Gender = GenderType.Male
        Skins(88).Gang = GangType.Civilian
        Skins(88).Img = My.Resources.Skin_99
        Skins(89).ID = 100
        Skins(89).Name = "Biker"
        Skins(89).Gender = GenderType.Male
        Skins(89).Gang = GangType.Civilian
        Skins(89).Img = My.Resources.Skin_100
        Skins(90).ID = 101
        Skins(90).Name = "Normal Ped"
        Skins(90).Gender = GenderType.Male
        Skins(90).Gang = GangType.Civilian
        Skins(90).Img = My.Resources.Skin_101
        Skins(91).ID = 102
        Skins(91).Name = "Balla"
        Skins(91).Gender = GenderType.Male
        Skins(91).Gang = GangType.Balla
        Skins(91).Img = My.Resources.Skin_102
        Skins(92).ID = 103
        Skins(92).Name = "Balla"
        Skins(92).Gender = GenderType.Male
        Skins(92).Gang = GangType.Balla
        Skins(92).Img = My.Resources.Skin_103
        Skins(93).ID = 104
        Skins(93).Name = "Balla"
        Skins(93).Gender = GenderType.Male
        Skins(93).Gang = GangType.Balla
        Skins(93).Img = My.Resources.Skin_104
        Skins(94).ID = 105
        Skins(94).Name = "Grove Street Families"
        Skins(94).Gender = GenderType.Male
        Skins(94).Gang = GangType.Grove
        Skins(94).Img = My.Resources.Skin_105
        Skins(95).ID = 106
        Skins(95).Name = "Grove Street Families"
        Skins(95).Gender = GenderType.Male
        Skins(95).Gang = GangType.Grove
        Skins(95).Img = My.Resources.Skin_106
        Skins(96).ID = 107
        Skins(96).Name = "Grove Street Families"
        Skins(96).Gender = GenderType.Male
        Skins(96).Gang = GangType.Grove
        Skins(96).Img = My.Resources.Skin_107
        Skins(97).ID = 108
        Skins(97).Name = "Los Santos Vagos"
        Skins(97).Gender = GenderType.Male
        Skins(97).Gang = GangType.Vago
        Skins(97).Img = My.Resources.Skin_108
        Skins(98).ID = 109
        Skins(98).Name = "Los Santos Vagos"
        Skins(98).Gender = GenderType.Male
        Skins(98).Gang = GangType.Vago
        Skins(98).Img = My.Resources.Skin_109
        Skins(99).ID = 110
        Skins(99).Name = "Los Santos Vagos"
        Skins(99).Gender = GenderType.Male
        Skins(99).Gang = GangType.Vago
        Skins(99).Img = My.Resources.Skin_110
        Skins(100).ID = 111
        Skins(100).Name = "The Russian Mafia"
        Skins(100).Gender = GenderType.Male
        Skins(100).Gang = GangType.RussianMafia
        Skins(100).Img = My.Resources.Skin_111
        Skins(101).ID = 112
        Skins(101).Name = "The Russian Mafia"
        Skins(101).Gender = GenderType.Male
        Skins(101).Gang = GangType.RussianMafia
        Skins(101).Img = My.Resources.Skin_112
        Skins(102).ID = 113
        Skins(102).Name = "The Russian Mafia"
        Skins(102).Gender = GenderType.Male
        Skins(102).Gang = GangType.RussianMafia
        Skins(102).Img = My.Resources.Skin_113
        Skins(103).ID = 114
        Skins(103).Name = "Varios Los Aztecas"
        Skins(103).Gender = GenderType.Male
        Skins(103).Gang = GangType.Azteca
        Skins(103).Img = My.Resources.Skin_114
        Skins(104).ID = 115
        Skins(104).Name = "Varios Los Aztecas"
        Skins(104).Gender = GenderType.Male
        Skins(104).Gang = GangType.Azteca
        Skins(104).Img = My.Resources.Skin_115
        Skins(105).ID = 116
        Skins(105).Name = "Varios Los Aztecas"
        Skins(105).Gender = GenderType.Male
        Skins(105).Gang = GangType.Azteca
        Skins(105).Img = My.Resources.Skin_116
        Skins(106).ID = 117
        Skins(106).Name = "Traid"
        Skins(106).Gender = GenderType.Male
        Skins(106).Gang = GangType.Triad
        Skins(106).Img = My.Resources.Skin_117
        Skins(107).ID = 118
        Skins(107).Name = "Traid"
        Skins(107).Gender = GenderType.Male
        Skins(107).Gang = GangType.Triad
        Skins(107).Img = My.Resources.Skin_118
        Skins(108).ID = 120
        Skins(108).Name = "Traid"
        Skins(108).Gender = GenderType.Male
        Skins(108).Gang = GangType.Triad
        Skins(108).Img = My.Resources.Skin_120
        Skins(109).ID = 121
        Skins(109).Name = "Da Nang Boy"
        Skins(109).Gender = GenderType.Male
        Skins(109).Gang = GangType.DaNang
        Skins(109).Img = My.Resources.Skin_121
        Skins(110).ID = 122
        Skins(110).Name = "Da Nang Boy"
        Skins(110).Gender = GenderType.Male
        Skins(110).Gang = GangType.DaNang
        Skins(110).Img = My.Resources.Skin_122
        Skins(111).ID = 123
        Skins(111).Name = "Da Nang Boy"
        Skins(111).Gender = GenderType.Male
        Skins(111).Gang = GangType.DaNang
        Skins(111).Img = My.Resources.Skin_123
        Skins(112).ID = 124
        Skins(112).Name = "The Mafia"
        Skins(112).Gender = GenderType.Male
        Skins(112).Gang = GangType.ItalianMafia
        Skins(112).Img = My.Resources.Skin_124
        Skins(113).ID = 125
        Skins(113).Name = "The Mafia"
        Skins(113).Gender = GenderType.Male
        Skins(113).Gang = GangType.ItalianMafia
        Skins(113).Img = My.Resources.Skin_125
        Skins(114).ID = 126
        Skins(114).Name = "The Mafia"
        Skins(114).Gender = GenderType.Male
        Skins(114).Gang = GangType.ItalianMafia
        Skins(114).Img = My.Resources.Skin_126
        Skins(115).ID = 127
        Skins(115).Name = "The Mafia"
        Skins(115).Gender = GenderType.Male
        Skins(115).Gang = GangType.ItalianMafia
        Skins(115).Img = My.Resources.Skin_127
        Skins(116).ID = 128
        Skins(116).Name = "Farm Inhabitant"
        Skins(116).Gender = GenderType.Male
        Skins(116).Gang = GangType.Civilian
        Skins(116).Img = My.Resources.Skin_128
        Skins(117).ID = 129
        Skins(117).Name = "Farm Inhabitant"
        Skins(117).Gender = GenderType.Female
        Skins(117).Gang = GangType.Civilian
        Skins(117).Img = My.Resources.Skin_129
        Skins(118).ID = 130
        Skins(118).Name = "Farm Inhabitant"
        Skins(118).Gender = GenderType.Female
        Skins(118).Gang = GangType.Civilian
        Skins(118).Img = My.Resources.Skin_130
        Skins(119).ID = 131
        Skins(119).Name = "Farm Inhabitant"
        Skins(119).Gender = GenderType.Female
        Skins(119).Gang = GangType.Civilian
        Skins(119).Img = My.Resources.Skin_131
        Skins(120).ID = 132
        Skins(120).Name = "Farm Inhabitant"
        Skins(120).Gender = GenderType.Male
        Skins(120).Gang = GangType.Civilian
        Skins(120).Img = My.Resources.Skin_132
        Skins(121).ID = 133
        Skins(121).Name = "Farm Inhabitant"
        Skins(121).Gender = GenderType.Male
        Skins(121).Gang = GangType.Civilian
        Skins(121).Img = My.Resources.Skin_133
        Skins(122).ID = 134
        Skins(122).Name = "Homeless"
        Skins(122).Gender = GenderType.Male
        Skins(122).Gang = GangType.Civilian
        Skins(122).Img = My.Resources.Skin_134
        Skins(123).ID = 135
        Skins(123).Name = "Homeless"
        Skins(123).Gender = GenderType.Male
        Skins(123).Gang = GangType.Civilian
        Skins(123).Img = My.Resources.Skin_135
        Skins(124).ID = 136
        Skins(124).Name = "Normal Ped"
        Skins(124).Gender = GenderType.Male
        Skins(124).Gang = GangType.Civilian
        Skins(124).Img = My.Resources.Skin_136
        Skins(125).ID = 137
        Skins(125).Name = "Homeless"
        Skins(125).Gender = GenderType.Male
        Skins(125).Gang = GangType.Civilian
        Skins(125).Img = My.Resources.Skin_137
        Skins(126).ID = 138
        Skins(126).Name = "Beach Visitor"
        Skins(126).Gender = GenderType.Female
        Skins(126).Gang = GangType.Civilian
        Skins(126).Img = My.Resources.Skin_138
        Skins(127).ID = 139
        Skins(127).Name = "Beach Visitor"
        Skins(127).Gender = GenderType.Female
        Skins(127).Gang = GangType.Civilian
        Skins(127).Img = My.Resources.Skin_139
        Skins(128).ID = 140
        Skins(128).Name = "Beach Visitor"
        Skins(128).Gender = GenderType.Female
        Skins(128).Gang = GangType.Civilian
        Skins(128).Img = My.Resources.Skin_140
        Skins(129).ID = 141
        Skins(129).Name = "Buisnesswoman"
        Skins(129).Gender = GenderType.Female
        Skins(129).Gang = GangType.Civilian
        Skins(129).Img = My.Resources.Skin_141
        Skins(130).ID = 142
        Skins(130).Name = "Taxi Driver"
        Skins(130).Gender = GenderType.Male
        Skins(130).Gang = GangType.Civilian
        Skins(130).Img = My.Resources.Skin_142
        Skins(131).ID = 143
        Skins(131).Name = "Drug Maker"
        Skins(131).Gender = GenderType.Male
        Skins(131).Gang = GangType.Civilian
        Skins(131).Img = My.Resources.Skin_143
        Skins(132).ID = 144
        Skins(132).Name = "Drug Maker"
        Skins(132).Gender = GenderType.Male
        Skins(132).Gang = GangType.Civilian
        Skins(132).Img = My.Resources.Skin_144
        Skins(133).ID = 145
        Skins(133).Name = "Drug Maker"
        Skins(133).Gender = GenderType.Female
        Skins(133).Gang = GangType.Civilian
        Skins(133).Img = My.Resources.Skin_145
        Skins(134).ID = 146
        Skins(134).Name = "Drug Maker"
        Skins(134).Gender = GenderType.Male
        Skins(134).Gang = GangType.Civilian
        Skins(134).Img = My.Resources.Skin_146
        Skins(135).ID = 147
        Skins(135).Name = "Buisnessman"
        Skins(135).Gender = GenderType.Male
        Skins(135).Gang = GangType.Civilian
        Skins(135).Img = My.Resources.Skin_147
        Skins(136).ID = 148
        Skins(136).Name = "Buisnesswoman"
        Skins(136).Gender = GenderType.Female
        Skins(136).Gang = GangType.Civilian
        Skins(136).Img = My.Resources.Skin_148
        Skins(137).ID = 150
        Skins(137).Name = "Buisnesswoman"
        Skins(137).Gender = GenderType.Female
        Skins(137).Gang = GangType.Civilian
        Skins(137).Img = My.Resources.Skin_150
        Skins(138).ID = 151
        Skins(138).Name = "Normal Ped"
        Skins(138).Gender = GenderType.Female
        Skins(138).Gang = GangType.Civilian
        Skins(138).Img = My.Resources.Skin_151
        Skins(139).ID = 152
        Skins(139).Name = "Prostitute"
        Skins(139).Gender = GenderType.Female
        Skins(139).Gang = GangType.Civilian
        Skins(139).Img = My.Resources.Skin_152
        Skins(140).ID = 153
        Skins(140).Name = "Construction worker"
        Skins(140).Gender = GenderType.Male
        Skins(140).Gang = GangType.Civilian
        Skins(140).Img = My.Resources.Skin_153
        Skins(141).ID = 154
        Skins(141).Name = "Beach Visitor"
        Skins(141).Gender = GenderType.Male
        Skins(141).Gang = GangType.Civilian
        Skins(141).Img = My.Resources.Skin_154
        Skins(142).ID = 155
        Skins(142).Name = "Well Stacked Pizza"
        Skins(142).Gender = GenderType.Male
        Skins(142).Gang = GangType.Civilian
        Skins(142).Img = My.Resources.Skin_155
        Skins(143).ID = 156
        Skins(143).Name = "Barber"
        Skins(143).Gender = GenderType.Male
        Skins(143).Gang = GangType.Civilian
        Skins(143).Img = My.Resources.Skin_156
        Skins(144).ID = 157
        Skins(144).Name = "Hillbilly"
        Skins(144).Gender = GenderType.Female
        Skins(144).Gang = GangType.Civilian
        Skins(144).Img = My.Resources.Skin_157
        Skins(145).ID = 158
        Skins(145).Name = "Farmer"
        Skins(145).Gender = GenderType.Male
        Skins(145).Gang = GangType.Civilian
        Skins(145).Img = My.Resources.Skin_158
        Skins(146).ID = 159
        Skins(146).Name = "Hillbilly"
        Skins(146).Gender = GenderType.Male
        Skins(146).Gang = GangType.Civilian
        Skins(146).Img = My.Resources.Skin_159
        Skins(147).ID = 160
        Skins(147).Name = "Hillbilly"
        Skins(147).Gender = GenderType.Male
        Skins(147).Gang = GangType.Civilian
        Skins(147).Img = My.Resources.Skin_160
        Skins(148).ID = 161
        Skins(148).Name = "Farmer"
        Skins(148).Gender = GenderType.Male
        Skins(148).Gang = GangType.Civilian
        Skins(148).Img = My.Resources.Skin_161
        Skins(149).ID = 162
        Skins(149).Name = "Hillbilly"
        Skins(149).Gender = GenderType.Male
        Skins(149).Gang = GangType.Civilian
        Skins(149).Img = My.Resources.Skin_162
        Skins(150).ID = 163
        Skins(150).Name = "Bouncer"
        Skins(150).Gender = GenderType.Male
        Skins(150).Gang = GangType.Civilian
        Skins(150).Img = My.Resources.Skin_163
        Skins(151).ID = 164
        Skins(151).Name = "Bouncer"
        Skins(151).Gender = GenderType.Male
        Skins(151).Gang = GangType.Civilian
        Skins(151).Img = My.Resources.Skin_164
        Skins(152).ID = 165
        Skins(152).Name = "MIB agent"
        Skins(152).Gender = GenderType.Male
        Skins(152).Gang = GangType.Civilian
        Skins(152).Img = My.Resources.Skin_165
        Skins(153).ID = 166
        Skins(153).Name = "MIB agent"
        Skins(153).Gender = GenderType.Male
        Skins(153).Gang = GangType.Civilian
        Skins(153).Img = My.Resources.Skin_166
        Skins(154).ID = 167
        Skins(154).Name = "Cluckin' Bell"
        Skins(154).Gender = GenderType.Male
        Skins(154).Gang = GangType.Civilian
        Skins(154).Img = My.Resources.Skin_167
        Skins(155).ID = 168
        Skins(155).Name = "Food vendor"
        Skins(155).Gender = GenderType.Male
        Skins(155).Gang = GangType.Civilian
        Skins(155).Img = My.Resources.Skin_168
        Skins(156).ID = 169
        Skins(156).Name = "Normal Ped"
        Skins(156).Gender = GenderType.Female
        Skins(156).Gang = GangType.Civilian
        Skins(156).Img = My.Resources.Skin_169
        Skins(157).ID = 170
        Skins(157).Name = "Normal Ped"
        Skins(157).Gender = GenderType.Male
        Skins(157).Gang = GangType.Civilian
        Skins(157).Img = My.Resources.Skin_170
        Skins(158).ID = 171
        Skins(158).Name = "Casino Worker"
        Skins(158).Gender = GenderType.Male
        Skins(158).Gang = GangType.Civilian
        Skins(158).Img = My.Resources.Skin_171
        Skins(159).ID = 172
        Skins(159).Name = "Casino croupier"
        Skins(159).Gender = GenderType.Female
        Skins(159).Gang = GangType.Civilian
        Skins(159).Img = My.Resources.Skin_172
        Skins(160).ID = 173
        Skins(160).Name = "San Fierro Rifa"
        Skins(160).Gender = GenderType.Male
        Skins(160).Gang = GangType.Rifa
        Skins(160).Img = My.Resources.Skin_173
        Skins(161).ID = 174
        Skins(161).Name = "San Fierro Rifa"
        Skins(161).Gender = GenderType.Male
        Skins(161).Gang = GangType.Rifa
        Skins(161).Img = My.Resources.Skin_174
        Skins(162).ID = 175
        Skins(162).Name = "San Fierro Rifa"
        Skins(162).Gender = GenderType.Male
        Skins(162).Gang = GangType.Rifa
        Skins(162).Img = My.Resources.Skin_175
        Skins(163).ID = 176
        Skins(163).Name = "Barber"
        Skins(163).Gender = GenderType.Male
        Skins(163).Gang = GangType.Civilian
        Skins(163).Img = My.Resources.Skin_176
        Skins(164).ID = 177
        Skins(164).Name = "Barber"
        Skins(164).Gender = GenderType.Male
        Skins(164).Gang = GangType.Civilian
        Skins(164).Img = My.Resources.Skin_177
        Skins(165).ID = 178
        Skins(165).Name = "Whore"
        Skins(165).Gender = GenderType.Female
        Skins(165).Gang = GangType.Civilian
        Skins(165).Img = My.Resources.Skin_178
        Skins(166).ID = 179
        Skins(166).Name = "Ammu-Nation Salesmen"
        Skins(166).Gender = GenderType.Male
        Skins(166).Gang = GangType.Civilian
        Skins(166).Img = My.Resources.Skin_179
        Skins(167).ID = 180
        Skins(167).Name = "Tattoo Artist"
        Skins(167).Gender = GenderType.Male
        Skins(167).Gang = GangType.Civilian
        Skins(167).Img = My.Resources.Skin_180
        Skins(168).ID = 181
        Skins(168).Name = "Punker"
        Skins(168).Gender = GenderType.Male
        Skins(168).Gang = GangType.Civilian
        Skins(168).Img = My.Resources.Skin_181
        Skins(169).ID = 182
        Skins(169).Name = "Normal Ped"
        Skins(169).Gender = GenderType.Male
        Skins(169).Gang = GangType.Civilian
        Skins(169).Img = My.Resources.Skin_182
        Skins(170).ID = 183
        Skins(170).Name = "Normal Ped"
        Skins(170).Gender = GenderType.Male
        Skins(170).Gang = GangType.Civilian
        Skins(170).Img = My.Resources.Skin_183
        Skins(171).ID = 184
        Skins(171).Name = "Normal Ped"
        Skins(171).Gender = GenderType.Male
        Skins(171).Gang = GangType.Civilian
        Skins(171).Img = My.Resources.Skin_184
        Skins(172).ID = 185
        Skins(172).Name = "Normal Ped"
        Skins(172).Gender = GenderType.Male
        Skins(172).Gang = GangType.Civilian
        Skins(172).Img = My.Resources.Skin_185
        Skins(173).ID = 186
        Skins(173).Name = "Normal Ped"
        Skins(173).Gender = GenderType.Male
        Skins(173).Gang = GangType.Civilian
        Skins(173).Img = My.Resources.Skin_186
        Skins(174).ID = 187
        Skins(174).Name = "Buisnessman"
        Skins(174).Gender = GenderType.Male
        Skins(174).Gang = GangType.Civilian
        Skins(174).Img = My.Resources.Skin_187
        Skins(175).ID = 188
        Skins(175).Name = "Normal Ped"
        Skins(175).Gender = GenderType.Male
        Skins(175).Gang = GangType.Civilian
        Skins(175).Img = My.Resources.Skin_188
        Skins(176).ID = 189
        Skins(176).Name = "Valet"
        Skins(176).Gender = GenderType.Male
        Skins(176).Gang = GangType.Civilian
        Skins(176).Img = My.Resources.Skin_189
        Skins(177).ID = 190
        Skins(177).Name = "Barbara Schternvart"
        Skins(177).Gender = GenderType.Female
        Skins(177).Gang = GangType.Civilian
        Skins(177).Img = My.Resources.Skin_190
        Skins(178).ID = 191
        Skins(178).Name = "Helena Wankstein"
        Skins(178).Gender = GenderType.Female
        Skins(178).Gang = GangType.Civilian
        Skins(178).Img = My.Resources.Skin_191
        Skins(179).ID = 192
        Skins(179).Name = "Michelle Cannes"
        Skins(179).Gender = GenderType.Female
        Skins(179).Gang = GangType.Civilian
        Skins(179).Img = My.Resources.Skin_192
        Skins(180).ID = 193
        Skins(180).Name = "Katie Zhan"
        Skins(180).Gender = GenderType.Female
        Skins(180).Gang = GangType.Civilian
        Skins(180).Img = My.Resources.Skin_193
        Skins(181).ID = 194
        Skins(181).Name = "Millie Perkins"
        Skins(181).Gender = GenderType.Female
        Skins(181).Gang = GangType.Civilian
        Skins(181).Img = My.Resources.Skin_194
        Skins(182).ID = 195
        Skins(182).Name = "Denise Robinson"
        Skins(182).Gender = GenderType.Female
        Skins(182).Gang = GangType.Civilian
        Skins(182).Img = My.Resources.Skin_195
        Skins(183).ID = 196
        Skins(183).Name = "Farm-Town inhabitant"
        Skins(183).Gender = GenderType.Female
        Skins(183).Gang = GangType.Civilian
        Skins(183).Img = My.Resources.Skin_196
        Skins(184).ID = 197
        Skins(184).Name = "Farm-Town inhabitant"
        Skins(184).Gender = GenderType.Female
        Skins(184).Gang = GangType.Civilian
        Skins(184).Img = My.Resources.Skin_197
        Skins(185).ID = 198
        Skins(185).Name = "Farm-Town inhabitant"
        Skins(185).Gender = GenderType.Female
        Skins(185).Gang = GangType.Civilian
        Skins(185).Img = My.Resources.Skin_198
        Skins(186).ID = 199
        Skins(186).Name = "Farm-Town inhabitant"
        Skins(186).Gender = GenderType.Female
        Skins(186).Gang = GangType.Civilian
        Skins(186).Img = My.Resources.Skin_199
        Skins(187).ID = 200
        Skins(187).Name = "Farmer"
        Skins(187).Gender = GenderType.Male
        Skins(187).Gang = GangType.Civilian
        Skins(187).Img = My.Resources.Skin_200
        Skins(188).ID = 201
        Skins(188).Name = "Farmer"
        Skins(188).Gender = GenderType.Female
        Skins(188).Gang = GangType.Civilian
        Skins(188).Img = My.Resources.Skin_201
        Skins(189).ID = 202
        Skins(189).Name = "Farmer"
        Skins(189).Gender = GenderType.Male
        Skins(189).Gang = GangType.Civilian
        Skins(189).Img = My.Resources.Skin_202
        Skins(190).ID = 203
        Skins(190).Name = "Karate Teacher"
        Skins(190).Gender = GenderType.Male
        Skins(190).Gang = GangType.Civilian
        Skins(190).Img = My.Resources.Skin_203
        Skins(191).ID = 204
        Skins(191).Name = "Karate Teacher"
        Skins(191).Gender = GenderType.Male
        Skins(191).Gang = GangType.Civilian
        Skins(191).Img = My.Resources.Skin_204
        Skins(192).ID = 205
        Skins(192).Name = "Burger Shot Cashier"
        Skins(192).Gender = GenderType.Female
        Skins(192).Gang = GangType.Civilian
        Skins(192).Img = My.Resources.Skin_205
        Skins(193).ID = 206
        Skins(193).Name = "Normal Ped"
        Skins(193).Gender = GenderType.Male
        Skins(193).Gang = GangType.Civilian
        Skins(193).Img = My.Resources.Skin_206
        Skins(194).ID = 207
        Skins(194).Name = "Prostitute"
        Skins(194).Gender = GenderType.Female
        Skins(194).Gang = GangType.Civilian
        Skins(194).Img = My.Resources.Skin_207
        Skins(195).ID = 209
        Skins(195).Name = "Noodle stand guy"
        Skins(195).Gender = GenderType.Male
        Skins(195).Gang = GangType.Civilian
        Skins(195).Img = My.Resources.Skin_209
        Skins(196).ID = 210
        Skins(196).Name = "Boater"
        Skins(196).Gender = GenderType.Male
        Skins(196).Gang = GangType.Civilian
        Skins(196).Img = My.Resources.Skin_210
        Skins(197).ID = 211
        Skins(197).Name = "Clothes shop staff"
        Skins(197).Gender = GenderType.Female
        Skins(197).Gang = GangType.Civilian
        Skins(197).Img = My.Resources.Skin_211
        Skins(198).ID = 212
        Skins(198).Name = "Homeless"
        Skins(198).Gender = GenderType.Male
        Skins(198).Gang = GangType.Civilian
        Skins(198).Img = My.Resources.Skin_212
        Skins(199).ID = 213
        Skins(199).Name = "Weird old man"
        Skins(199).Gender = GenderType.Male
        Skins(199).Gang = GangType.Civilian
        Skins(199).Img = My.Resources.Skin_213
        Skins(200).ID = 214
        Skins(200).Name = "Waitress"
        Skins(200).Gender = GenderType.Female
        Skins(200).Gang = GangType.Civilian
        Skins(200).Img = My.Resources.Skin_214
        Skins(201).ID = 215
        Skins(201).Name = "Normal Ped"
        Skins(201).Gender = GenderType.Female
        Skins(201).Gang = GangType.Civilian
        Skins(201).Img = My.Resources.Skin_215
        Skins(202).ID = 216
        Skins(202).Name = "Normal Ped"
        Skins(202).Gender = GenderType.Female
        Skins(202).Gang = GangType.Civilian
        Skins(202).Img = My.Resources.Skin_216
        Skins(203).ID = 217
        Skins(203).Name = "Clothes shop staff"
        Skins(203).Gender = GenderType.Male
        Skins(203).Gang = GangType.Civilian
        Skins(203).Img = My.Resources.Skin_217
        Skins(204).ID = 218
        Skins(204).Name = "Normal Ped"
        Skins(204).Gender = GenderType.Female
        Skins(204).Gang = GangType.Civilian
        Skins(204).Img = My.Resources.Skin_218
        Skins(205).ID = 219
        Skins(205).Name = "Secretary"
        Skins(205).Gender = GenderType.Female
        Skins(205).Gang = GangType.Civilian
        Skins(205).Img = My.Resources.Skin_219
        Skins(206).ID = 220
        Skins(206).Name = "Cab Driver"
        Skins(206).Gender = GenderType.Male
        Skins(206).Gang = GangType.Civilian
        Skins(206).Img = My.Resources.Skin_220
        Skins(207).ID = 221
        Skins(207).Name = "Normal Ped"
        Skins(207).Gender = GenderType.Male
        Skins(207).Gang = GangType.Civilian
        Skins(207).Img = My.Resources.Skin_221
        Skins(208).ID = 222
        Skins(208).Name = "Normal Ped"
        Skins(208).Gender = GenderType.Male
        Skins(208).Gang = GangType.Civilian
        Skins(208).Img = My.Resources.Skin_222
        Skins(209).ID = 223
        Skins(209).Name = "Normal Ped"
        Skins(209).Gender = GenderType.Male
        Skins(209).Gang = GangType.Civilian
        Skins(209).Img = My.Resources.Skin_223
        Skins(210).ID = 224
        Skins(210).Name = "Sofori"
        Skins(210).Gender = GenderType.Female
        Skins(210).Gang = GangType.Civilian
        Skins(210).Img = My.Resources.Skin_224
        Skins(211).ID = 225
        Skins(211).Name = "Normal Ped"
        Skins(211).Gender = GenderType.Female
        Skins(211).Gang = GangType.Civilian
        Skins(211).Img = My.Resources.Skin_225
        Skins(212).ID = 226
        Skins(212).Name = "Normal Ped"
        Skins(212).Gender = GenderType.Female
        Skins(212).Gang = GangType.Civilian
        Skins(212).Img = My.Resources.Skin_226
        Skins(213).ID = 227
        Skins(213).Name = "Buisnessman"
        Skins(213).Gender = GenderType.Male
        Skins(213).Gang = GangType.Civilian
        Skins(213).Img = My.Resources.Skin_227
        Skins(214).ID = 228
        Skins(214).Name = "Normal Ped"
        Skins(214).Gender = GenderType.Male
        Skins(214).Gang = GangType.Civilian
        Skins(214).Img = My.Resources.Skin_228
        Skins(215).ID = 229
        Skins(215).Name = "Normal Ped"
        Skins(215).Gender = GenderType.Male
        Skins(215).Gang = GangType.Civilian
        Skins(215).Img = My.Resources.Skin_229
        Skins(216).ID = 230
        Skins(216).Name = "Homeless"
        Skins(216).Gender = GenderType.Male
        Skins(216).Gang = GangType.Civilian
        Skins(216).Img = My.Resources.Skin_230
        Skins(217).ID = 231
        Skins(217).Name = "Normal Ped"
        Skins(217).Gender = GenderType.Female
        Skins(217).Gang = GangType.Civilian
        Skins(217).Img = My.Resources.Skin_231
        Skins(218).ID = 232
        Skins(218).Name = "Normal Ped"
        Skins(218).Gender = GenderType.Female
        Skins(218).Gang = GangType.Civilian
        Skins(218).Img = My.Resources.Skin_232
        Skins(219).ID = 233
        Skins(219).Name = "Normal Ped"
        Skins(219).Gender = GenderType.Female
        Skins(219).Gang = GangType.Civilian
        Skins(219).Img = My.Resources.Skin_233
        Skins(220).ID = 234
        Skins(220).Name = "Cab driver"
        Skins(220).Gender = GenderType.Male
        Skins(220).Gang = GangType.Civilian
        Skins(220).Img = My.Resources.Skin_234
        Skins(221).ID = 235
        Skins(221).Name = "Normal Ped"
        Skins(221).Gender = GenderType.Male
        Skins(221).Gang = GangType.Civilian
        Skins(221).Img = My.Resources.Skin_235
        Skins(222).ID = 236
        Skins(222).Name = "Normal Ped"
        Skins(222).Gender = GenderType.Male
        Skins(222).Gang = GangType.Civilian
        Skins(222).Img = My.Resources.Skin_236
        Skins(223).ID = 237
        Skins(223).Name = "Prostitute"
        Skins(223).Gender = GenderType.Female
        Skins(223).Gang = GangType.Civilian
        Skins(223).Img = My.Resources.Skin_237
        Skins(224).ID = 238
        Skins(224).Name = "Prostitute"
        Skins(224).Gender = GenderType.Female
        Skins(224).Gang = GangType.Civilian
        Skins(224).Img = My.Resources.Skin_238
        Skins(225).ID = 239
        Skins(225).Name = "Homeless"
        Skins(225).Gender = GenderType.Male
        Skins(225).Gang = GangType.Civilian
        Skins(225).Img = My.Resources.Skin_239
        Skins(226).ID = 240
        Skins(226).Name = "The D.A"
        Skins(226).Gender = GenderType.Male
        Skins(226).Gang = GangType.Civilian
        Skins(226).Img = My.Resources.Skin_240
        Skins(227).ID = 241
        Skins(227).Name = "Afro-American"
        Skins(227).Gender = GenderType.Male
        Skins(227).Gang = GangType.Civilian
        Skins(227).Img = My.Resources.Skin_241
        Skins(228).ID = 242
        Skins(228).Name = "Mexican"
        Skins(228).Gender = GenderType.Male
        Skins(228).Gang = GangType.Civilian
        Skins(228).Img = My.Resources.Skin_242
        Skins(229).ID = 243
        Skins(229).Name = "Prostitute"
        Skins(229).Gender = GenderType.Female
        Skins(229).Gang = GangType.Civilian
        Skins(229).Img = My.Resources.Skin_243
        Skins(230).ID = 244
        Skins(230).Name = "Stripper"
        Skins(230).Gender = GenderType.Female
        Skins(230).Gang = GangType.Civilian
        Skins(230).Img = My.Resources.Skin_244
        Skins(231).ID = 245
        Skins(231).Name = "Prostitute"
        Skins(231).Gender = GenderType.Female
        Skins(231).Gang = GangType.Civilian
        Skins(231).Img = My.Resources.Skin_245
        Skins(232).ID = 246
        Skins(232).Name = "Stripper"
        Skins(232).Gender = GenderType.Female
        Skins(232).Gang = GangType.Civilian
        Skins(232).Img = My.Resources.Skin_246
        Skins(233).ID = 247
        Skins(233).Name = "Biker"
        Skins(233).Gender = GenderType.Male
        Skins(233).Gang = GangType.Civilian
        Skins(233).Img = My.Resources.Skin_247
        Skins(234).ID = 248
        Skins(234).Name = "Biker"
        Skins(234).Gender = GenderType.Male
        Skins(234).Gang = GangType.Civilian
        Skins(234).Img = My.Resources.Skin_248
        Skins(235).ID = 249
        Skins(235).Name = "Pimp"
        Skins(235).Gender = GenderType.Male
        Skins(235).Gang = GangType.Civilian
        Skins(235).Img = My.Resources.Skin_249
        Skins(236).ID = 250
        Skins(236).Name = "Normal Ped"
        Skins(236).Gender = GenderType.Male
        Skins(236).Gang = GangType.Civilian
        Skins(236).Img = My.Resources.Skin_250
        Skins(237).ID = 251
        Skins(237).Name = "Lifeguard"
        Skins(237).Gender = GenderType.Female
        Skins(237).Gang = GangType.Civilian
        Skins(237).Img = My.Resources.Skin_251
        Skins(238).ID = 252
        Skins(238).Name = "Naked Valet"
        Skins(238).Gender = GenderType.Male
        Skins(238).Gang = GangType.Civilian
        Skins(238).Img = My.Resources.Skin_252
        Skins(239).ID = 253
        Skins(239).Name = "Bus Driver"
        Skins(239).Gender = GenderType.Male
        Skins(239).Gang = GangType.Civilian
        Skins(239).Img = My.Resources.Skin_253
        Skins(240).ID = 254
        Skins(240).Name = "Drug Dealer"
        Skins(240).Gender = GenderType.Male
        Skins(240).Gang = GangType.Civilian
        Skins(240).Img = My.Resources.Skin_254
        Skins(241).ID = 255
        Skins(241).Name = "Chauffeur"
        Skins(241).Gender = GenderType.Male
        Skins(241).Gang = GangType.Civilian
        Skins(241).Img = My.Resources.Skin_255
        Skins(242).ID = 256
        Skins(242).Name = "Stripper"
        Skins(242).Gender = GenderType.Female
        Skins(242).Gang = GangType.Civilian
        Skins(242).Img = My.Resources.Skin_256
        Skins(243).ID = 257
        Skins(243).Name = "Stripper"
        Skins(243).Gender = GenderType.Female
        Skins(243).Gang = GangType.Civilian
        Skins(243).Img = My.Resources.Skin_257
        Skins(244).ID = 258
        Skins(244).Name = "Heckler"
        Skins(244).Gender = GenderType.Male
        Skins(244).Gang = GangType.Civilian
        Skins(244).Img = My.Resources.Skin_258
        Skins(245).ID = 259
        Skins(245).Name = "Heckler"
        Skins(245).Gender = GenderType.Male
        Skins(245).Gang = GangType.Civilian
        Skins(245).Img = My.Resources.Skin_259
        Skins(246).ID = 260
        Skins(246).Name = "Construction worker"
        Skins(246).Gender = GenderType.Male
        Skins(246).Gang = GangType.Civilian
        Skins(246).Img = My.Resources.Skin_260
        Skins(247).ID = 261
        Skins(247).Name = "Cab driver"
        Skins(247).Gender = GenderType.Male
        Skins(247).Gang = GangType.Civilian
        Skins(247).Img = My.Resources.Skin_261
        Skins(248).ID = 262
        Skins(248).Name = "Cab driver"
        Skins(248).Gender = GenderType.Male
        Skins(248).Gang = GangType.Civilian
        Skins(248).Img = My.Resources.Skin_262
        Skins(249).ID = 263
        Skins(249).Name = "Normal Ped"
        Skins(249).Gender = GenderType.Female
        Skins(249).Gang = GangType.Civilian
        Skins(249).Img = My.Resources.Skin_263
        Skins(250).ID = 264
        Skins(250).Name = "Clown"
        Skins(250).Gender = GenderType.Male
        Skins(250).Gang = GangType.Civilian
        Skins(250).Img = My.Resources.Skin_264
        Skins(251).ID = 265
        Skins(251).Name = "Tenpenny"
        Skins(251).Gender = GenderType.Male
        Skins(251).Gang = GangType.Police
        Skins(251).Img = My.Resources.Skin_265
        Skins(252).ID = 266
        Skins(252).Name = "Pulaski"
        Skins(252).Gender = GenderType.Male
        Skins(252).Gang = GangType.Police
        Skins(252).Img = My.Resources.Skin_266
        Skins(253).ID = 267
        Skins(253).Name = "Officer Frank Tenpenny (Crooked Cop)"
        Skins(253).Gender = GenderType.Male
        Skins(253).Gang = GangType.Police
        Skins(253).Img = My.Resources.Skin_267
        Skins(254).ID = 268
        Skins(254).Name = "Dwaine"
        Skins(254).Gender = GenderType.Male
        Skins(254).Gang = GangType.Civilian
        Skins(254).Img = My.Resources.Skin_268
        Skins(255).ID = 269
        Skins(255).Name = "Melvin Harris (Big Smoke)"
        Skins(255).Gender = GenderType.Male
        Skins(255).Gang = GangType.Civilian
        Skins(255).Img = My.Resources.Skin_269
        Skins(256).ID = 270
        Skins(256).Name = "Sean 'Sweet' Johnson"
        Skins(256).Gender = GenderType.Male
        Skins(256).Gang = GangType.Civilian
        Skins(256).Img = My.Resources.Skin_270
        Skins(257).ID = 271
        Skins(257).Name = "Lance 'Ryder' Wilson"
        Skins(257).Gender = GenderType.Male
        Skins(257).Gang = GangType.Civilian
        Skins(257).Img = My.Resources.Skin_271
        Skins(258).ID = 272
        Skins(258).Name = "Mafia Boss"
        Skins(258).Gender = GenderType.Male
        Skins(258).Gang = GangType.Civilian
        Skins(258).Img = My.Resources.Skin_272
        Skins(259).ID = 274
        Skins(259).Name = "Paramedic"
        Skins(259).Gender = GenderType.Male
        Skins(259).Gang = GangType.Civilian
        Skins(259).Img = My.Resources.Skin_274
        Skins(260).ID = 275
        Skins(260).Name = "Paramedic"
        Skins(260).Gender = GenderType.Male
        Skins(260).Gang = GangType.Civilian
        Skins(260).Img = My.Resources.Skin_275
        Skins(261).ID = 276
        Skins(261).Name = "Paramedic"
        Skins(261).Gender = GenderType.Male
        Skins(261).Gang = GangType.Civilian
        Skins(261).Img = My.Resources.Skin_276
        Skins(262).ID = 277
        Skins(262).Name = "Firefighter"
        Skins(262).Gender = GenderType.Male
        Skins(262).Gang = GangType.Civilian
        Skins(262).Img = My.Resources.Skin_277
        Skins(263).ID = 278
        Skins(263).Name = "Firefighter"
        Skins(263).Gender = GenderType.Male
        Skins(263).Gang = GangType.Civilian
        Skins(263).Img = My.Resources.Skin_278
        Skins(264).ID = 279
        Skins(264).Name = "Firefighter"
        Skins(264).Gender = GenderType.Male
        Skins(264).Gang = GangType.Civilian
        Skins(264).Img = My.Resources.Skin_279
        Skins(265).ID = 280
        Skins(265).Name = "Los Santos Police"
        Skins(265).Gender = GenderType.Male
        Skins(265).Gang = GangType.Police
        Skins(265).Img = My.Resources.Skin_280
        Skins(266).ID = 281
        Skins(266).Name = "San Fierro Police"
        Skins(266).Gender = GenderType.Male
        Skins(266).Gang = GangType.Police
        Skins(266).Img = My.Resources.Skin_281
        Skins(267).ID = 282
        Skins(267).Name = "Las Venturas Police"
        Skins(267).Gender = GenderType.Male
        Skins(267).Gang = GangType.Police
        Skins(267).Img = My.Resources.Skin_282
        Skins(268).ID = 283
        Skins(268).Name = "County Sheriff"
        Skins(268).Gender = GenderType.Male
        Skins(268).Gang = GangType.Police
        Skins(268).Img = My.Resources.Skin_283
        Skins(269).ID = 284
        Skins(269).Name = "Biker cop"
        Skins(269).Gender = GenderType.Male
        Skins(269).Gang = GangType.Police
        Skins(269).Img = My.Resources.Skin_284
        Skins(270).ID = 285
        Skins(270).Name = "S.W.A.T Special Forces"
        Skins(270).Gender = GenderType.Male
        Skins(270).Gang = GangType.Police
        Skins(270).Img = My.Resources.Skin_285
        Skins(271).ID = 286
        Skins(271).Name = "Federal Agents"
        Skins(271).Gender = GenderType.Male
        Skins(271).Gang = GangType.Police
        Skins(271).Img = My.Resources.Skin_286
        Skins(272).ID = 287
        Skins(272).Name = "San Andreas Army"
        Skins(272).Gender = GenderType.Male
        Skins(272).Gang = GangType.Police
        Skins(272).Img = My.Resources.Skin_287
        Skins(273).ID = 288
        Skins(273).Name = "Desert Sheriff"
        Skins(273).Gender = GenderType.Male
        Skins(273).Gang = GangType.Police
        Skins(273).Img = My.Resources.Skin_288
        Skins(274).ID = 290
        Skins(274).Name = "Ken Rosenberg"
        Skins(274).Gender = GenderType.Male
        Skins(274).Gang = GangType.Civilian
        Skins(274).Img = My.Resources.Skin_290
        Skins(275).ID = 291
        Skins(275).Name = "Kent Paul"
        Skins(275).Gender = GenderType.Male
        Skins(275).Gang = GangType.Civilian
        Skins(275).Img = My.Resources.Skin_291
        Skins(276).ID = 292
        Skins(276).Name = "Cesar Vialpando"
        Skins(276).Gender = GenderType.Male
        Skins(276).Gang = GangType.Civilian
        Skins(276).Img = My.Resources.Skin_292
        Skins(277).ID = 293
        Skins(277).Name = "Jeffrey 'OG Loc' Cross"
        Skins(277).Gender = GenderType.Male
        Skins(277).Gang = GangType.Civilian
        Skins(277).Img = My.Resources.Skin_293
        Skins(278).ID = 294
        Skins(278).Name = "Wu Zi Mu (Woozie)"
        Skins(278).Gender = GenderType.Male
        Skins(278).Gang = GangType.Civilian
        Skins(278).Img = My.Resources.Skin_294
        Skins(279).ID = 295
        Skins(279).Name = "Michael Toreno"
        Skins(279).Gender = GenderType.Male
        Skins(279).Gang = GangType.Civilian
        Skins(279).Img = My.Resources.Skin_295
        Skins(280).ID = 296
        Skins(280).Name = "Jizzy B."
        Skins(280).Gender = GenderType.Male
        Skins(280).Gang = GangType.Civilian
        Skins(280).Img = My.Resources.Skin_296
        Skins(281).ID = 297
        Skins(281).Name = "Madd Dogg"
        Skins(281).Gender = GenderType.Male
        Skins(281).Gang = GangType.Civilian
        Skins(281).Img = My.Resources.Skin_297
        Skins(282).ID = 298
        Skins(282).Name = "Catalina"
        Skins(282).Gender = GenderType.Female
        Skins(282).Gang = GangType.Civilian
        Skins(282).Img = My.Resources.Skin_298
        Skins(283).ID = 299
        Skins(283).Name = "Claude"
        Skins(283).Gender = GenderType.Male
        Skins(283).Gang = GangType.Civilian
        Skins(283).Img = My.Resources.Skin_299
    End Sub

#End Region

#Region "Vehicles"

    Public Sub FillVehicles()
        FillVehiclesList()
        Main.TreeView2.Nodes.Add("Airplanes")
        Main.TreeView2.Nodes.Add("Helicopters")
        Main.TreeView2.Nodes.Add("Bikes")
        Main.TreeView2.Nodes.Add("Convertibles")
        Main.TreeView2.Nodes.Add("Industrial")
        Main.TreeView2.Nodes.Add("Lowriders")
        Main.TreeView2.Nodes.Add("Off Road")
        Main.TreeView2.Nodes.Add("Public Service")
        Main.TreeView2.Nodes.Add("Saloons")
        Main.TreeView2.Nodes.Add("Sport")
        Main.TreeView2.Nodes.Add("Station Wagons")
        Main.TreeView2.Nodes.Add("Boats")
        Main.TreeView2.Nodes.Add("Trailers")
        Main.TreeView2.Nodes.Add("Unique")
        Main.TreeView2.Nodes.Add("RC")
        Main.TreeView2.Nodes.Add("All")

        Main.TreeView3.Nodes.Add("Airplanes")
        Main.TreeView3.Nodes.Add("Helicopters")
        Main.TreeView3.Nodes.Add("Bikes")
        Main.TreeView3.Nodes.Add("Convertibles")
        Main.TreeView3.Nodes.Add("Industrial")
        Main.TreeView3.Nodes.Add("Lowriders")
        Main.TreeView3.Nodes.Add("Off Road")
        Main.TreeView3.Nodes.Add("Public Service")
        Main.TreeView3.Nodes.Add("Saloons")
        Main.TreeView3.Nodes.Add("Sport")
        Main.TreeView3.Nodes.Add("Station Wagons")
        Main.TreeView3.Nodes.Add("Boats")
        Main.TreeView3.Nodes.Add("Trailers")
        Main.TreeView3.Nodes.Add("Unique")
        Main.TreeView3.Nodes.Add("RC")
        Main.TreeView3.Nodes.Add("All")
        Dim count As Integer
        For Each vehicle In Vehicles
            If vehicle.ID <> 0 Then
                Select Case vehicle.Type
                    Case VehicleType.Airplane
                        Main.TreeView2.Nodes(0).Nodes.Add(vehicle.ID)
                        Main.TreeView3.Nodes(0).Nodes.Add(vehicle.Name)
                    Case VehicleType.Helicopter
                        Main.TreeView2.Nodes(1).Nodes.Add(vehicle.ID)
                        Main.TreeView3.Nodes(1).Nodes.Add(vehicle.Name)
                    Case VehicleType.Bike
                        Main.TreeView2.Nodes(2).Nodes.Add(vehicle.ID)
                        Main.TreeView3.Nodes(2).Nodes.Add(vehicle.Name)
                    Case VehicleType.Convertible
                        Main.TreeView2.Nodes(3).Nodes.Add(vehicle.ID)
                        Main.TreeView3.Nodes(3).Nodes.Add(vehicle.Name)
                    Case VehicleType.Industrial
                        Main.TreeView2.Nodes(4).Nodes.Add(vehicle.ID)
                        Main.TreeView3.Nodes(4).Nodes.Add(vehicle.Name)
                    Case VehicleType.Lowriders
                        Main.TreeView2.Nodes(5).Nodes.Add(vehicle.ID)
                        Main.TreeView3.Nodes(5).Nodes.Add(vehicle.Name)
                    Case VehicleType.Off_Road
                        Main.TreeView2.Nodes(6).Nodes.Add(vehicle.ID)
                        Main.TreeView3.Nodes(6).Nodes.Add(vehicle.Name)
                    Case VehicleType.Public_Service
                        Main.TreeView2.Nodes(7).Nodes.Add(vehicle.ID)
                        Main.TreeView3.Nodes(7).Nodes.Add(vehicle.Name)
                    Case VehicleType.Saloon
                        Main.TreeView2.Nodes(8).Nodes.Add(vehicle.ID)
                        Main.TreeView3.Nodes(8).Nodes.Add(vehicle.Name)
                    Case VehicleType.Sport
                        Main.TreeView2.Nodes(9).Nodes.Add(vehicle.ID)
                        Main.TreeView3.Nodes(9).Nodes.Add(vehicle.Name)
                    Case VehicleType.Station_Wagon
                        Main.TreeView2.Nodes(10).Nodes.Add(vehicle.ID)
                        Main.TreeView3.Nodes(10).Nodes.Add(vehicle.Name)
                    Case VehicleType.Boat
                        Main.TreeView2.Nodes(11).Nodes.Add(vehicle.ID)
                        Main.TreeView3.Nodes(11).Nodes.Add(vehicle.Name)
                    Case VehicleType.Trailer
                        Main.TreeView2.Nodes(12).Nodes.Add(vehicle.ID)
                        Main.TreeView3.Nodes(12).Nodes.Add(vehicle.Name)
                    Case VehicleType.Unique
                        Main.TreeView2.Nodes(13).Nodes.Add(vehicle.ID)
                        Main.TreeView3.Nodes(13).Nodes.Add(vehicle.Name)
                    Case VehicleType.RC
                        Main.TreeView2.Nodes(14).Nodes.Add(vehicle.ID)
                        Main.TreeView3.Nodes(14).Nodes.Add(vehicle.Name)
                End Select
                Main.TreeView2.Nodes(15).Nodes.Add(vehicle.ID)
                Main.TreeView3.Nodes(15).Nodes.Add(vehicle.Name)
                Splash.Label1.Text = String.Format("Loading vehicles ({0}/211)", count)
                Splash.ProgressBar1.Value += 1
                count += 1
            End If
        Next
    End Sub

    Private Sub FillVehiclesList()
        Splash.Label1.Text = "Loading vehicles..."
        Vehicles(0).ID = 400
        Vehicles(0).Name = "Landstalker"
        Vehicles(0).Img = My.Resources.Vehicle_400
        Vehicles(0).Type = VehicleType.Off_Road
        Vehicles(1).ID = 401
        Vehicles(1).Name = "Bravura"
        Vehicles(1).Img = My.Resources.Vehicle_401
        Vehicles(1).Type = VehicleType.Saloon
        Vehicles(2).ID = 402
        Vehicles(2).Name = "Buffalo"
        Vehicles(2).Img = My.Resources.Vehicle_402
        Vehicles(2).Type = VehicleType.Sport
        Vehicles(3).ID = 403
        Vehicles(3).Name = "Linerunner"
        Vehicles(3).Img = My.Resources.Vehicle_403
        Vehicles(3).Type = VehicleType.Industrial
        Vehicles(4).ID = 404
        Vehicles(4).Name = "Perenniel"
        Vehicles(4).Img = My.Resources.Vehicle_404
        Vehicles(4).Type = VehicleType.Station_Wagon
        Vehicles(5).ID = 405
        Vehicles(5).Name = "Sentinel"
        Vehicles(5).Img = My.Resources.Vehicle_405
        Vehicles(5).Type = VehicleType.Saloon
        Vehicles(6).ID = 406
        Vehicles(6).Name = "Dumper"
        Vehicles(6).Img = My.Resources.Vehicle_406
        Vehicles(6).Type = VehicleType.Unique
        Vehicles(7).ID = 407
        Vehicles(7).Name = "Firetruck"
        Vehicles(7).Img = My.Resources.Vehicle_407
        Vehicles(7).Type = VehicleType.Public_Service
        Vehicles(8).ID = 408
        Vehicles(8).Name = "Trashmaster"
        Vehicles(8).Img = My.Resources.Vehicle_408
        Vehicles(8).Type = VehicleType.Industrial
        Vehicles(9).ID = 409
        Vehicles(9).Name = "Stretch"
        Vehicles(9).Img = My.Resources.Vehicle_409
        Vehicles(9).Type = VehicleType.Unique
        Vehicles(10).ID = 410
        Vehicles(10).Name = "Manana"
        Vehicles(10).Img = My.Resources.Vehicle_410
        Vehicles(10).Type = VehicleType.Saloon
        Vehicles(11).ID = 411
        Vehicles(11).Name = "Infernus"
        Vehicles(11).Img = My.Resources.Vehicle_411
        Vehicles(11).Type = VehicleType.Sport
        Vehicles(12).ID = 412
        Vehicles(12).Name = "Voodoo"
        Vehicles(12).Img = My.Resources.Vehicle_412
        Vehicles(12).Type = VehicleType.Lowriders
        Vehicles(13).ID = 413
        Vehicles(13).Name = "Pony"
        Vehicles(13).Img = My.Resources.Vehicle_413
        Vehicles(13).Type = VehicleType.Industrial
        Vehicles(14).ID = 414
        Vehicles(14).Name = "Mule"
        Vehicles(14).Img = My.Resources.Vehicle_414
        Vehicles(14).Type = VehicleType.Industrial
        Vehicles(15).ID = 415
        Vehicles(15).Name = "Cheetah"
        Vehicles(15).Img = My.Resources.Vehicle_415
        Vehicles(15).Type = VehicleType.Sport
        Vehicles(16).ID = 416
        Vehicles(16).Name = "Ambulance"
        Vehicles(16).Img = My.Resources.Vehicle_416
        Vehicles(16).Type = VehicleType.Public_Service
        Vehicles(17).ID = 417
        Vehicles(17).Name = "Leviathan"
        Vehicles(17).Img = My.Resources.Vehicle_417
        Vehicles(17).Type = VehicleType.Helicopter
        Vehicles(18).ID = 418
        Vehicles(18).Name = "Moonbeam"
        Vehicles(18).Img = My.Resources.Vehicle_418
        Vehicles(18).Type = VehicleType.Station_Wagon
        Vehicles(19).ID = 419
        Vehicles(19).Name = "Esperanto"
        Vehicles(19).Img = My.Resources.Vehicle_419
        Vehicles(19).Type = VehicleType.Saloon
        Vehicles(20).ID = 420
        Vehicles(20).Name = "Taxi"
        Vehicles(20).Img = My.Resources.Vehicle_420
        Vehicles(20).Type = VehicleType.Public_Service
        Vehicles(21).ID = 421
        Vehicles(21).Name = "Washington"
        Vehicles(21).Img = My.Resources.Vehicle_421
        Vehicles(21).Type = VehicleType.Saloon
        Vehicles(22).ID = 422
        Vehicles(22).Name = "Bobcat"
        Vehicles(22).Img = My.Resources.Vehicle_422
        Vehicles(22).Type = VehicleType.Industrial
        Vehicles(23).ID = 423
        Vehicles(23).Name = "Mr Whoopee"
        Vehicles(23).Img = My.Resources.Vehicle_423
        Vehicles(23).Type = VehicleType.Unique
        Vehicles(24).ID = 424
        Vehicles(24).Name = "BF Injection"
        Vehicles(24).Img = My.Resources.Vehicle_424
        Vehicles(24).Type = VehicleType.Off_Road
        Vehicles(25).ID = 425
        Vehicles(25).Name = "Hunter"
        Vehicles(25).Img = My.Resources.Vehicle_425
        Vehicles(25).Type = VehicleType.Helicopter
        Vehicles(26).ID = 426
        Vehicles(26).Name = "Premier"
        Vehicles(26).Img = My.Resources.Vehicle_426
        Vehicles(26).Type = VehicleType.Saloon
        Vehicles(27).ID = 427
        Vehicles(27).Name = "Enforcer"
        Vehicles(27).Img = My.Resources.Vehicle_427
        Vehicles(27).Type = VehicleType.Public_Service
        Vehicles(28).ID = 428
        Vehicles(28).Name = "Securicar"
        Vehicles(28).Img = My.Resources.Vehicle_428
        Vehicles(28).Type = VehicleType.Unique
        Vehicles(29).ID = 429
        Vehicles(29).Name = "Banshee"
        Vehicles(29).Img = My.Resources.Vehicle_429
        Vehicles(29).Type = VehicleType.Sport
        Vehicles(30).ID = 430
        Vehicles(30).Name = "Predator"
        Vehicles(30).Img = My.Resources.Vehicle_430
        Vehicles(30).Type = VehicleType.Boat
        Vehicles(31).ID = 431
        Vehicles(31).Name = "Bus"
        Vehicles(31).Img = My.Resources.Vehicle_431
        Vehicles(31).Type = VehicleType.Public_Service
        Vehicles(32).ID = 432
        Vehicles(32).Name = "Rhino"
        Vehicles(32).Img = My.Resources.Vehicle_432
        Vehicles(32).Type = VehicleType.Public_Service
        Vehicles(33).ID = 433
        Vehicles(33).Name = "Barracks"
        Vehicles(33).Img = My.Resources.Vehicle_433
        Vehicles(33).Type = VehicleType.Public_Service
        Vehicles(34).ID = 434
        Vehicles(34).Name = "Hotknife"
        Vehicles(34).Img = My.Resources.Vehicle_434
        Vehicles(34).Type = VehicleType.Unique
        Vehicles(35).ID = 435
        Vehicles(35).Name = "Article Trailer"
        Vehicles(35).Img = My.Resources.Vehicle_435
        Vehicles(35).Type = VehicleType.Trailer
        Vehicles(36).ID = 436
        Vehicles(36).Name = "Previon"
        Vehicles(36).Img = My.Resources.Vehicle_436
        Vehicles(36).Type = VehicleType.Saloon
        Vehicles(37).ID = 437
        Vehicles(37).Name = "Coach"
        Vehicles(37).Img = My.Resources.Vehicle_437
        Vehicles(37).Type = VehicleType.Public_Service
        Vehicles(38).ID = 438
        Vehicles(38).Name = "Cabbie"
        Vehicles(38).Img = My.Resources.Vehicle_438
        Vehicles(38).Type = VehicleType.Public_Service
        Vehicles(39).ID = 439
        Vehicles(39).Name = "Stallion"
        Vehicles(39).Img = My.Resources.Vehicle_439
        Vehicles(39).Type = VehicleType.Convertible
        Vehicles(40).ID = 440
        Vehicles(40).Name = "Rumpo"
        Vehicles(40).Img = My.Resources.Vehicle_440
        Vehicles(40).Type = VehicleType.Industrial
        Vehicles(41).ID = 441
        Vehicles(41).Name = "RC Bandit"
        Vehicles(41).Img = My.Resources.Vehicle_441
        Vehicles(41).Type = VehicleType.RC
        Vehicles(42).ID = 442
        Vehicles(42).Name = "Romero"
        Vehicles(42).Img = My.Resources.Vehicle_442
        Vehicles(42).Type = VehicleType.Unique
        Vehicles(43).ID = 443
        Vehicles(43).Name = "Packer"
        Vehicles(43).Img = My.Resources.Vehicle_443
        Vehicles(43).Type = VehicleType.Industrial
        Vehicles(44).ID = 444
        Vehicles(44).Name = "Monster"
        Vehicles(44).Img = My.Resources.Vehicle_444
        Vehicles(44).Type = VehicleType.Off_Road
        Vehicles(45).ID = 445
        Vehicles(45).Name = "Admiral"
        Vehicles(45).Img = My.Resources.Vehicle_445
        Vehicles(45).Type = VehicleType.Saloon
        Vehicles(46).ID = 446
        Vehicles(46).Name = "Squallo"
        Vehicles(46).Img = My.Resources.Vehicle_446
        Vehicles(46).Type = VehicleType.Boat
        Vehicles(47).ID = 447
        Vehicles(47).Name = "Seasparrow"
        Vehicles(47).Img = My.Resources.Vehicle_447
        Vehicles(47).Type = VehicleType.Helicopter
        Vehicles(48).ID = 448
        Vehicles(48).Name = "Pizzaboy"
        Vehicles(48).Img = My.Resources.Vehicle_448
        Vehicles(48).Type = VehicleType.Bike
        Vehicles(49).ID = 449
        Vehicles(49).Name = "Tram"
        Vehicles(49).Img = My.Resources.Vehicle_449
        Vehicles(49).Type = VehicleType.Unique
        Vehicles(50).ID = 450
        Vehicles(50).Name = "Article Trailer 2"
        Vehicles(50).Img = My.Resources.Vehicle_450
        Vehicles(50).Type = VehicleType.Trailer
        Vehicles(51).ID = 451
        Vehicles(51).Name = "Turismo"
        Vehicles(51).Img = My.Resources.Vehicle_451
        Vehicles(51).Type = VehicleType.Sport
        Vehicles(52).ID = 452
        Vehicles(52).Name = "Speeder"
        Vehicles(52).Img = My.Resources.Vehicle_452
        Vehicles(52).Type = VehicleType.Boat
        Vehicles(53).ID = 453
        Vehicles(53).Name = "Reefer"
        Vehicles(53).Img = My.Resources.Vehicle_453
        Vehicles(53).Type = VehicleType.Boat
        Vehicles(54).ID = 454
        Vehicles(54).Name = "Tropic"
        Vehicles(54).Img = My.Resources.Vehicle_454
        Vehicles(54).Type = VehicleType.Boat
        Vehicles(55).ID = 455
        Vehicles(55).Name = "Flatbed"
        Vehicles(55).Img = My.Resources.Vehicle_455
        Vehicles(55).Type = VehicleType.Industrial
        Vehicles(56).ID = 456
        Vehicles(56).Name = "Yankee"
        Vehicles(56).Img = My.Resources.Vehicle_456
        Vehicles(56).Type = VehicleType.Industrial
        Vehicles(57).ID = 457
        Vehicles(57).Name = "Caddy"
        Vehicles(57).Img = My.Resources.Vehicle_457
        Vehicles(57).Type = VehicleType.Unique
        Vehicles(58).ID = 458
        Vehicles(58).Name = "Solair"
        Vehicles(58).Img = My.Resources.Vehicle_458
        Vehicles(58).Type = VehicleType.Station_Wagon
        Vehicles(59).ID = 459
        Vehicles(59).Name = "Berkley's RC Van"
        Vehicles(59).Img = My.Resources.Vehicle_459
        Vehicles(59).Type = VehicleType.Industrial
        Vehicles(60).ID = 460
        Vehicles(60).Name = "Skimmer"
        Vehicles(60).Img = My.Resources.Vehicle_460
        Vehicles(60).Type = VehicleType.Airplane
        Vehicles(61).ID = 461
        Vehicles(61).Name = "PCJ-600"
        Vehicles(61).Img = My.Resources.Vehicle_461
        Vehicles(61).Type = VehicleType.Bike
        Vehicles(62).ID = 462
        Vehicles(62).Name = "Faggio"
        Vehicles(62).Img = My.Resources.Vehicle_462
        Vehicles(62).Type = VehicleType.Bike
        Vehicles(63).ID = 463
        Vehicles(63).Name = "Freeway"
        Vehicles(63).Img = My.Resources.Vehicle_463
        Vehicles(63).Type = VehicleType.Bike
        Vehicles(64).ID = 464
        Vehicles(64).Name = "RC Baron"
        Vehicles(64).Img = My.Resources.Vehicle_464
        Vehicles(64).Type = VehicleType.RC
        Vehicles(65).ID = 465
        Vehicles(65).Name = "RC Raider"
        Vehicles(65).Img = My.Resources.Vehicle_465
        Vehicles(65).Type = VehicleType.RC
        Vehicles(66).ID = 466
        Vehicles(66).Name = "Glendale"
        Vehicles(66).Img = My.Resources.Vehicle_466
        Vehicles(66).Type = VehicleType.Saloon
        Vehicles(67).ID = 467
        Vehicles(67).Name = "Oceanic"
        Vehicles(67).Img = My.Resources.Vehicle_467
        Vehicles(67).Type = VehicleType.Sport
        Vehicles(68).ID = 468
        Vehicles(68).Name = "Sanchez"
        Vehicles(68).Img = My.Resources.Vehicle_468
        Vehicles(68).Type = VehicleType.Bike
        Vehicles(69).ID = 469
        Vehicles(69).Name = "Sparrow"
        Vehicles(69).Img = My.Resources.Vehicle_469
        Vehicles(69).Type = VehicleType.Helicopter
        Vehicles(70).ID = 470
        Vehicles(70).Name = "Patriot"
        Vehicles(70).Img = My.Resources.Vehicle_470
        Vehicles(70).Type = VehicleType.Off_Road
        Vehicles(71).ID = 471
        Vehicles(71).Name = "Quad"
        Vehicles(71).Img = My.Resources.Vehicle_471
        Vehicles(71).Type = VehicleType.Bike
        Vehicles(72).ID = 472
        Vehicles(72).Name = "Coastguard"
        Vehicles(72).Img = My.Resources.Vehicle_472
        Vehicles(72).Type = VehicleType.Boat
        Vehicles(73).ID = 473
        Vehicles(73).Name = "Dinghy"
        Vehicles(73).Img = My.Resources.Vehicle_473
        Vehicles(73).Type = VehicleType.Boat
        Vehicles(74).ID = 474
        Vehicles(74).Name = "Hermes"
        Vehicles(74).Img = My.Resources.Vehicle_474
        Vehicles(74).Type = VehicleType.Sport
        Vehicles(75).ID = 475
        Vehicles(75).Name = "Sabre"
        Vehicles(75).Img = My.Resources.Vehicle_475
        Vehicles(75).Type = VehicleType.Sport
        Vehicles(76).ID = 476
        Vehicles(76).Name = "Rustler"
        Vehicles(76).Img = My.Resources.Vehicle_476
        Vehicles(76).Type = VehicleType.Airplane
        Vehicles(77).ID = 477
        Vehicles(77).Name = "ZR-350"
        Vehicles(77).Img = My.Resources.Vehicle_477
        Vehicles(77).Type = VehicleType.Sport
        Vehicles(78).ID = 478
        Vehicles(78).Name = "Walton"
        Vehicles(78).Img = My.Resources.Vehicle_478
        Vehicles(78).Type = VehicleType.Industrial
        Vehicles(79).ID = 479
        Vehicles(79).Name = "Regina"
        Vehicles(79).Img = My.Resources.Vehicle_479
        Vehicles(79).Type = VehicleType.Station_Wagon
        Vehicles(80).ID = 480
        Vehicles(80).Name = "Comet"
        Vehicles(80).Img = My.Resources.Vehicle_480
        Vehicles(80).Type = VehicleType.Convertible
        Vehicles(81).ID = 481
        Vehicles(81).Name = "BMX"
        Vehicles(81).Img = My.Resources.Vehicle_481
        Vehicles(81).Type = VehicleType.Bike
        Vehicles(82).ID = 482
        Vehicles(82).Name = "Burrito"
        Vehicles(82).Img = My.Resources.Vehicle_482
        Vehicles(82).Type = VehicleType.Industrial
        Vehicles(83).ID = 483
        Vehicles(83).Name = "Camper"
        Vehicles(83).Img = My.Resources.Vehicle_483
        Vehicles(83).Type = VehicleType.Unique
        Vehicles(84).ID = 484
        Vehicles(84).Name = "Marquis"
        Vehicles(84).Img = My.Resources.Vehicle_484
        Vehicles(84).Type = VehicleType.Boat
        Vehicles(85).ID = 485
        Vehicles(85).Name = "Baggage"
        Vehicles(85).Img = My.Resources.Vehicle_485
        Vehicles(85).Type = VehicleType.Unique
        Vehicles(86).ID = 486
        Vehicles(86).Name = "Dozer"
        Vehicles(86).Img = My.Resources.Vehicle_486
        Vehicles(86).Type = VehicleType.Unique
        Vehicles(87).ID = 487
        Vehicles(87).Name = "Maverick"
        Vehicles(87).Img = My.Resources.Vehicle_487
        Vehicles(87).Type = VehicleType.Helicopter
        Vehicles(88).ID = 488
        Vehicles(88).Name = "SAN News Maverick"
        Vehicles(88).Img = My.Resources.Vehicle_488
        Vehicles(88).Type = VehicleType.Helicopter
        Vehicles(89).ID = 489
        Vehicles(89).Name = "Rancher"
        Vehicles(89).Img = My.Resources.Vehicle_489
        Vehicles(89).Type = VehicleType.Off_Road
        Vehicles(90).ID = 490
        Vehicles(90).Name = "FBI Rancher"
        Vehicles(90).Img = My.Resources.Vehicle_490
        Vehicles(90).Type = VehicleType.Public_Service
        Vehicles(91).ID = 491
        Vehicles(91).Name = "Virgo"
        Vehicles(91).Img = My.Resources.Vehicle_491
        Vehicles(91).Type = VehicleType.Saloon
        Vehicles(92).ID = 492
        Vehicles(92).Name = "Greenwood"
        Vehicles(92).Img = My.Resources.Vehicle_492
        Vehicles(92).Type = VehicleType.Saloon
        Vehicles(93).ID = 493
        Vehicles(93).Name = "Jetmax"
        Vehicles(93).Img = My.Resources.Vehicle_493
        Vehicles(93).Type = VehicleType.Boat
        Vehicles(94).ID = 494
        Vehicles(94).Name = "Hotring Racer"
        Vehicles(94).Img = My.Resources.Vehicle_494
        Vehicles(94).Type = VehicleType.Sport
        Vehicles(95).ID = 495
        Vehicles(95).Name = "Sandking"
        Vehicles(95).Img = My.Resources.Vehicle_495
        Vehicles(95).Type = VehicleType.Off_Road
        Vehicles(96).ID = 496
        Vehicles(96).Name = "Blista Compact"
        Vehicles(96).Img = My.Resources.Vehicle_496
        Vehicles(96).Type = VehicleType.Sport
        Vehicles(97).ID = 497
        Vehicles(97).Name = "Police Maverick"
        Vehicles(97).Img = My.Resources.Vehicle_497
        Vehicles(97).Type = VehicleType.Helicopter
        Vehicles(98).ID = 498
        Vehicles(98).Name = "Boxville"
        Vehicles(98).Img = My.Resources.Vehicle_498
        Vehicles(98).Type = VehicleType.Industrial
        Vehicles(99).ID = 499
        Vehicles(99).Name = "Benson"
        Vehicles(99).Img = My.Resources.Vehicle_499
        Vehicles(99).Type = VehicleType.Industrial
        Vehicles(100).ID = 500
        Vehicles(100).Name = "Mesa"
        Vehicles(100).Img = My.Resources.Vehicle_500
        Vehicles(100).Type = VehicleType.Off_Road
        Vehicles(101).ID = 501
        Vehicles(101).Name = "RC Goblin"
        Vehicles(101).Img = My.Resources.Vehicle_501
        Vehicles(101).Type = VehicleType.RC
        Vehicles(102).ID = 502
        Vehicles(102).Name = "Hotring Racer"
        Vehicles(102).Img = My.Resources.Vehicle_502
        Vehicles(102).Type = VehicleType.Sport
        Vehicles(103).ID = 503
        Vehicles(103).Name = "Hotring Racer"
        Vehicles(103).Img = My.Resources.Vehicle_503
        Vehicles(103).Type = VehicleType.Sport
        Vehicles(104).ID = 504
        Vehicles(104).Name = "Bloodring Banger"
        Vehicles(104).Img = My.Resources.Vehicle_504
        Vehicles(104).Type = VehicleType.Saloon
        Vehicles(105).ID = 505
        Vehicles(105).Name = "Rancher"
        Vehicles(105).Img = My.Resources.Vehicle_505
        Vehicles(105).Type = VehicleType.Off_Road
        Vehicles(106).ID = 506
        Vehicles(106).Name = "Super GT"
        Vehicles(106).Img = My.Resources.Vehicle_506
        Vehicles(106).Type = VehicleType.Sport
        Vehicles(107).ID = 507
        Vehicles(107).Name = "Elegant"
        Vehicles(107).Img = My.Resources.Vehicle_507
        Vehicles(107).Type = VehicleType.Saloon
        Vehicles(108).ID = 508
        Vehicles(108).Name = "Journey"
        Vehicles(108).Img = My.Resources.Vehicle_508
        Vehicles(108).Type = VehicleType.Unique
        Vehicles(109).ID = 509
        Vehicles(109).Name = "Bike"
        Vehicles(109).Img = My.Resources.Vehicle_509
        Vehicles(109).Type = VehicleType.Bike
        Vehicles(110).ID = 510
        Vehicles(110).Name = "Mountain Bike"
        Vehicles(110).Img = My.Resources.Vehicle_510
        Vehicles(110).Type = VehicleType.Bike
        Vehicles(111).ID = 511
        Vehicles(111).Name = "Beagle"
        Vehicles(111).Img = My.Resources.Vehicle_511
        Vehicles(111).Type = VehicleType.Airplane
        Vehicles(112).ID = 512
        Vehicles(112).Name = "Cropduster"
        Vehicles(112).Img = My.Resources.Vehicle_512
        Vehicles(112).Type = VehicleType.Airplane
        Vehicles(113).ID = 513
        Vehicles(113).Name = "Stuntplane"
        Vehicles(113).Img = My.Resources.Vehicle_513
        Vehicles(113).Type = VehicleType.Airplane
        Vehicles(114).ID = 514
        Vehicles(114).Name = "Tanker"
        Vehicles(114).Img = My.Resources.Vehicle_514
        Vehicles(114).Type = VehicleType.Industrial
        Vehicles(115).ID = 515
        Vehicles(115).Name = "Roadtrain"
        Vehicles(115).Img = My.Resources.Vehicle_515
        Vehicles(115).Type = VehicleType.Industrial
        Vehicles(116).ID = 516
        Vehicles(116).Name = "Nebula"
        Vehicles(116).Img = My.Resources.Vehicle_516
        Vehicles(116).Type = VehicleType.Saloon
        Vehicles(117).ID = 517
        Vehicles(117).Name = "Majestic"
        Vehicles(117).Img = My.Resources.Vehicle_517
        Vehicles(117).Type = VehicleType.Saloon
        Vehicles(118).ID = 518
        Vehicles(118).Name = "Buccaneer"
        Vehicles(118).Img = My.Resources.Vehicle_518
        Vehicles(118).Type = VehicleType.Saloon
        Vehicles(119).ID = 519
        Vehicles(119).Name = "Shamal"
        Vehicles(119).Img = My.Resources.Vehicle_519
        Vehicles(119).Type = VehicleType.Airplane
        Vehicles(120).ID = 520
        Vehicles(120).Name = "Hydra"
        Vehicles(120).Img = My.Resources.Vehicle_520
        Vehicles(120).Type = VehicleType.Airplane
        Vehicles(121).ID = 521
        Vehicles(121).Name = "FCR-900"
        Vehicles(121).Img = My.Resources.Vehicle_521
        Vehicles(121).Type = VehicleType.Bike
        Vehicles(122).ID = 522
        Vehicles(122).Name = "NRG-500"
        Vehicles(122).Img = My.Resources.Vehicle_522
        Vehicles(122).Type = VehicleType.Bike
        Vehicles(123).ID = 523
        Vehicles(123).Name = "Cop Bike (HPV-1000)"
        Vehicles(123).Img = My.Resources.Vehicle_523
        Vehicles(123).Type = VehicleType.Bike
        Vehicles(124).ID = 524
        Vehicles(124).Name = "Cement Truck"
        Vehicles(124).Img = My.Resources.Vehicle_524
        Vehicles(124).Type = VehicleType.Industrial
        Vehicles(125).ID = 525
        Vehicles(125).Name = "Towtruck"
        Vehicles(125).Img = My.Resources.Vehicle_525
        Vehicles(125).Type = VehicleType.Unique
        Vehicles(126).ID = 526
        Vehicles(126).Name = "Fortune"
        Vehicles(126).Img = My.Resources.Vehicle_526
        Vehicles(126).Type = VehicleType.Saloon
        Vehicles(127).ID = 527
        Vehicles(127).Name = "Cadrona"
        Vehicles(127).Img = My.Resources.Vehicle_527
        Vehicles(127).Type = VehicleType.Saloon
        Vehicles(128).ID = 528
        Vehicles(128).Name = "FBI Truck"
        Vehicles(128).Img = My.Resources.Vehicle_528
        Vehicles(128).Type = VehicleType.Public_Service
        Vehicles(129).ID = 529
        Vehicles(129).Name = "Willard"
        Vehicles(129).Img = My.Resources.Vehicle_529
        Vehicles(129).Type = VehicleType.Saloon
        Vehicles(130).ID = 530
        Vehicles(130).Name = "Forklift"
        Vehicles(130).Img = My.Resources.Vehicle_530
        Vehicles(130).Type = VehicleType.Unique
        Vehicles(131).ID = 531
        Vehicles(131).Name = "Tractor"
        Vehicles(131).Img = My.Resources.Vehicle_531
        Vehicles(131).Type = VehicleType.Industrial
        Vehicles(132).ID = 532
        Vehicles(132).Name = "Combine Harvester"
        Vehicles(132).Img = My.Resources.Vehicle_532
        Vehicles(132).Type = VehicleType.Unique
        Vehicles(133).ID = 533
        Vehicles(133).Name = "Feltzer"
        Vehicles(133).Img = My.Resources.Vehicle_533
        Vehicles(133).Type = VehicleType.Convertible
        Vehicles(134).ID = 534
        Vehicles(134).Name = "Remington"
        Vehicles(134).Img = My.Resources.Vehicle_534
        Vehicles(134).Type = VehicleType.Lowriders
        Vehicles(135).ID = 535
        Vehicles(135).Name = "Slamvan"
        Vehicles(135).Img = My.Resources.Vehicle_535
        Vehicles(135).Type = VehicleType.Lowriders
        Vehicles(136).ID = 536
        Vehicles(136).Name = "Blade"
        Vehicles(136).Img = My.Resources.Vehicle_536
        Vehicles(136).Type = VehicleType.Lowriders
        Vehicles(137).ID = 537
        Vehicles(137).Name = "Freight (Train)"
        Vehicles(137).Img = My.Resources.Vehicle_537
        Vehicles(137).Type = VehicleType.Unique
        Vehicles(138).ID = 538
        Vehicles(138).Name = "Brownstreak (Train)"
        Vehicles(138).Img = My.Resources.Vehicle_538
        Vehicles(138).Type = VehicleType.Unique
        Vehicles(139).ID = 539
        Vehicles(139).Name = "Vortex"
        Vehicles(139).Img = My.Resources.Vehicle_539
        Vehicles(139).Type = VehicleType.Unique
        Vehicles(140).ID = 540
        Vehicles(140).Name = "Vincent"
        Vehicles(140).Img = My.Resources.Vehicle_540
        Vehicles(140).Type = VehicleType.Sport
        Vehicles(141).ID = 541
        Vehicles(141).Name = "Bullet"
        Vehicles(141).Img = My.Resources.Vehicle_541
        Vehicles(141).Type = VehicleType.Sport
        Vehicles(142).ID = 542
        Vehicles(142).Name = "Clover"
        Vehicles(142).Img = My.Resources.Vehicle_542
        Vehicles(142).Type = VehicleType.Saloon
        Vehicles(143).ID = 543
        Vehicles(143).Name = "Sadler"
        Vehicles(143).Img = My.Resources.Vehicle_543
        Vehicles(143).Type = VehicleType.Industrial
        Vehicles(144).ID = 544
        Vehicles(144).Name = "Firetruck LA"
        Vehicles(144).Img = My.Resources.Vehicle_544
        Vehicles(144).Type = VehicleType.Public_Service
        Vehicles(145).ID = 545
        Vehicles(145).Name = "Hustler"
        Vehicles(145).Img = My.Resources.Vehicle_545
        Vehicles(145).Type = VehicleType.Unique
        Vehicles(146).ID = 546
        Vehicles(146).Name = "Intruder"
        Vehicles(146).Img = My.Resources.Vehicle_546
        Vehicles(146).Type = VehicleType.Saloon
        Vehicles(147).ID = 547
        Vehicles(147).Name = "Primo"
        Vehicles(147).Img = My.Resources.Vehicle_547
        Vehicles(147).Type = VehicleType.Saloon
        Vehicles(148).ID = 548
        Vehicles(148).Name = "Cargobob"
        Vehicles(148).Img = My.Resources.Vehicle_548
        Vehicles(148).Type = VehicleType.Helicopter
        Vehicles(149).ID = 549
        Vehicles(149).Name = "Tampa"
        Vehicles(149).Img = My.Resources.Vehicle_549
        Vehicles(149).Type = VehicleType.Saloon
        Vehicles(150).ID = 550
        Vehicles(150).Name = "Sunrise"
        Vehicles(150).Img = My.Resources.Vehicle_550
        Vehicles(150).Type = VehicleType.Saloon
        Vehicles(151).ID = 551
        Vehicles(151).Name = "Merit"
        Vehicles(151).Img = My.Resources.Vehicle_551
        Vehicles(151).Type = VehicleType.Saloon
        Vehicles(152).ID = 552
        Vehicles(152).Name = "Utility Van"
        Vehicles(152).Img = My.Resources.Vehicle_552
        Vehicles(152).Type = VehicleType.Industrial
        Vehicles(153).ID = 553
        Vehicles(153).Name = "Nevada"
        Vehicles(153).Img = My.Resources.Vehicle_553
        Vehicles(153).Type = VehicleType.Airplane
        Vehicles(154).ID = 554
        Vehicles(154).Name = "Yosemite"
        Vehicles(154).Img = My.Resources.Vehicle_554
        Vehicles(154).Type = VehicleType.Industrial
        Vehicles(155).ID = 555
        Vehicles(155).Name = "Windsor"
        Vehicles(155).Img = My.Resources.Vehicle_555
        Vehicles(155).Type = VehicleType.Convertible
        Vehicles(156).ID = 556
        Vehicles(156).Name = "Monster A"
        Vehicles(156).Img = My.Resources.Vehicle_556
        Vehicles(156).Type = VehicleType.Off_Road
        Vehicles(157).ID = 557
        Vehicles(157).Name = "Monster B"
        Vehicles(157).Img = My.Resources.Vehicle_557
        Vehicles(157).Type = VehicleType.Off_Road
        Vehicles(158).ID = 558
        Vehicles(158).Name = "Uranus"
        Vehicles(158).Img = My.Resources.Vehicle_558
        Vehicles(158).Type = VehicleType.Sport
        Vehicles(159).ID = 559
        Vehicles(159).Name = "Jester"
        Vehicles(159).Img = My.Resources.Vehicle_559
        Vehicles(159).Type = VehicleType.Sport
        Vehicles(160).ID = 560
        Vehicles(160).Name = "Sultan"
        Vehicles(160).Img = My.Resources.Vehicle_560
        Vehicles(160).Type = VehicleType.Saloon
        Vehicles(161).ID = 561
        Vehicles(161).Name = "Stratum"
        Vehicles(161).Img = My.Resources.Vehicle_561
        Vehicles(161).Type = VehicleType.Station_Wagon
        Vehicles(162).ID = 562
        Vehicles(162).Name = "Elegy"
        Vehicles(162).Img = My.Resources.Vehicle_562
        Vehicles(162).Type = VehicleType.Saloon
        Vehicles(163).ID = 563
        Vehicles(163).Name = "Raindance"
        Vehicles(163).Img = My.Resources.Vehicle_563
        Vehicles(163).Type = VehicleType.Helicopter
        Vehicles(164).ID = 564
        Vehicles(164).Name = "RC Tiger"
        Vehicles(164).Img = My.Resources.Vehicle_564
        Vehicles(164).Type = VehicleType.RC
        Vehicles(165).ID = 565
        Vehicles(165).Name = "Flash"
        Vehicles(165).Img = My.Resources.Vehicle_565
        Vehicles(165).Type = VehicleType.Sport
        Vehicles(166).ID = 566
        Vehicles(166).Name = "Tahoma"
        Vehicles(166).Img = My.Resources.Vehicle_566
        Vehicles(166).Type = VehicleType.Lowriders
        Vehicles(167).ID = 567
        Vehicles(167).Name = "Savanna"
        Vehicles(167).Img = My.Resources.Vehicle_567
        Vehicles(167).Type = VehicleType.Lowriders
        Vehicles(168).ID = 568
        Vehicles(168).Name = "Bandito"
        Vehicles(168).Img = My.Resources.Vehicle_568
        Vehicles(168).Type = VehicleType.Off_Road
        Vehicles(169).ID = 569
        Vehicles(169).Name = "Freight Flat Trailer (Train)"
        Vehicles(169).Img = My.Resources.Vehicle_569
        Vehicles(169).Type = VehicleType.Trailer
        Vehicles(170).ID = 570
        Vehicles(170).Name = "Streak Trailer (Train)"
        Vehicles(170).Img = My.Resources.Vehicle_570
        Vehicles(170).Type = VehicleType.Trailer
        Vehicles(171).ID = 571
        Vehicles(171).Name = "Kart"
        Vehicles(171).Img = My.Resources.Vehicle_571
        Vehicles(171).Type = VehicleType.Unique
        Vehicles(172).ID = 572
        Vehicles(172).Name = "Mower"
        Vehicles(172).Img = My.Resources.Vehicle_572
        Vehicles(172).Type = VehicleType.Unique
        Vehicles(173).ID = 573
        Vehicles(173).Name = "Dune"
        Vehicles(173).Img = My.Resources.Vehicle_573
        Vehicles(173).Type = VehicleType.Off_Road
        Vehicles(174).ID = 574
        Vehicles(174).Name = "Sweeper"
        Vehicles(174).Img = My.Resources.Vehicle_574
        Vehicles(174).Type = VehicleType.Unique
        Vehicles(175).ID = 575
        Vehicles(175).Name = "Broadway"
        Vehicles(175).Img = My.Resources.Vehicle_575
        Vehicles(175).Type = VehicleType.Lowriders
        Vehicles(176).ID = 576
        Vehicles(176).Name = "Tornado"
        Vehicles(176).Img = My.Resources.Vehicle_576
        Vehicles(176).Type = VehicleType.Lowriders
        Vehicles(177).ID = 577
        Vehicles(177).Name = "AT-400"
        Vehicles(177).Img = My.Resources.Vehicle_577
        Vehicles(177).Type = VehicleType.Airplane
        Vehicles(178).ID = 578
        Vehicles(178).Name = "DFT-30"
        Vehicles(178).Img = My.Resources.Vehicle_578
        Vehicles(178).Type = VehicleType.Industrial
        Vehicles(179).ID = 579
        Vehicles(179).Name = "Huntley"
        Vehicles(179).Img = My.Resources.Vehicle_579
        Vehicles(179).Type = VehicleType.Off_Road
        Vehicles(180).ID = 580
        Vehicles(180).Name = "Stafford"
        Vehicles(180).Img = My.Resources.Vehicle_580
        Vehicles(180).Type = VehicleType.Saloon
        Vehicles(181).ID = 581
        Vehicles(181).Name = "BF-400"
        Vehicles(181).Img = My.Resources.Vehicle_581
        Vehicles(181).Type = VehicleType.Bike
        Vehicles(182).ID = 582
        Vehicles(182).Name = "Newsvan"
        Vehicles(182).Img = My.Resources.Vehicle_582
        Vehicles(182).Type = VehicleType.Industrial
        Vehicles(183).ID = 583
        Vehicles(183).Name = "Tug"
        Vehicles(183).Img = My.Resources.Vehicle_583
        Vehicles(183).Type = VehicleType.Unique
        Vehicles(184).ID = 584
        Vehicles(184).Name = "Petrol Tanker"
        Vehicles(184).Img = My.Resources.Vehicle_584
        Vehicles(184).Type = VehicleType.Trailer
        Vehicles(185).ID = 585
        Vehicles(185).Name = "Emperor"
        Vehicles(185).Img = My.Resources.Vehicle_585
        Vehicles(185).Type = VehicleType.Saloon
        Vehicles(186).ID = 586
        Vehicles(186).Name = "Wayfarer"
        Vehicles(186).Img = My.Resources.Vehicle_586
        Vehicles(186).Type = VehicleType.Bike
        Vehicles(187).ID = 587
        Vehicles(187).Name = "Euros"
        Vehicles(187).Img = My.Resources.Vehicle_587
        Vehicles(187).Type = VehicleType.Sport
        Vehicles(188).ID = 588
        Vehicles(188).Name = "Hotdog"
        Vehicles(188).Img = My.Resources.Vehicle_588
        Vehicles(188).Type = VehicleType.Unique
        Vehicles(189).ID = 589
        Vehicles(189).Name = "Club"
        Vehicles(189).Img = My.Resources.Vehicle_589
        Vehicles(189).Type = VehicleType.Sport
        Vehicles(190).ID = 590
        Vehicles(190).Name = "Freight Box Trailer (Train)"
        Vehicles(190).Img = My.Resources.Vehicle_590
        Vehicles(190).Type = VehicleType.Trailer
        Vehicles(191).ID = 591
        Vehicles(191).Name = "Article Trailer 3"
        Vehicles(191).Img = My.Resources.Vehicle_591
        Vehicles(191).Type = VehicleType.Trailer
        Vehicles(192).ID = 592
        Vehicles(192).Name = "Andromada"
        Vehicles(192).Img = My.Resources.Vehicle_592
        Vehicles(192).Type = VehicleType.Airplane
        Vehicles(193).ID = 593
        Vehicles(193).Name = "Dodo"
        Vehicles(193).Img = My.Resources.Vehicle_593
        Vehicles(193).Type = VehicleType.Airplane
        Vehicles(194).ID = 594
        Vehicles(194).Name = "RC Cam"
        Vehicles(194).Img = My.Resources.Vehicle_594
        Vehicles(194).Type = VehicleType.RC
        Vehicles(195).ID = 595
        Vehicles(195).Name = "Launch"
        Vehicles(195).Img = My.Resources.Vehicle_595
        Vehicles(195).Type = VehicleType.Boat
        Vehicles(196).ID = 596
        Vehicles(196).Name = "Police Car (LSPD)"
        Vehicles(196).Img = My.Resources.Vehicle_596
        Vehicles(196).Type = VehicleType.Public_Service
        Vehicles(197).ID = 597
        Vehicles(197).Name = "Police Car (SFPD)"
        Vehicles(197).Img = My.Resources.Vehicle_597
        Vehicles(197).Type = VehicleType.Public_Service
        Vehicles(198).ID = 598
        Vehicles(198).Name = "Police Car (LVPD)"
        Vehicles(198).Img = My.Resources.Vehicle_598
        Vehicles(198).Type = VehicleType.Public_Service
        Vehicles(199).ID = 599
        Vehicles(199).Name = "Police Ranger"
        Vehicles(199).Img = My.Resources.Vehicle_599
        Vehicles(199).Type = VehicleType.Public_Service
        Vehicles(200).ID = 600
        Vehicles(200).Name = "Picador"
        Vehicles(200).Img = My.Resources.Vehicle_600
        Vehicles(200).Type = VehicleType.Industrial
        Vehicles(201).ID = 601
        Vehicles(201).Name = "S.W.A.T."
        Vehicles(201).Img = My.Resources.Vehicle_601
        Vehicles(201).Type = VehicleType.Public_Service
        Vehicles(202).ID = 602
        Vehicles(202).Name = "Alpha"
        Vehicles(202).Img = My.Resources.Vehicle_602
        Vehicles(202).Type = VehicleType.Sport
        Vehicles(203).ID = 603
        Vehicles(203).Name = "Phoenix"
        Vehicles(203).Img = My.Resources.Vehicle_603
        Vehicles(203).Type = VehicleType.Sport
        Vehicles(204).ID = 604
        Vehicles(204).Name = "Glendale Shit"
        Vehicles(204).Img = My.Resources.Vehicle_604
        Vehicles(204).Type = VehicleType.Saloon
        Vehicles(205).ID = 605
        Vehicles(205).Name = "Sadler Shit"
        Vehicles(205).Img = My.Resources.Vehicle_605
        Vehicles(205).Type = VehicleType.Industrial
        Vehicles(206).ID = 606
        Vehicles(206).Name = "Baggage Trailer A"
        Vehicles(206).Img = My.Resources.Vehicle_606
        Vehicles(206).Type = VehicleType.Trailer
        Vehicles(207).ID = 607
        Vehicles(207).Name = "Baggage Trailer B"
        Vehicles(207).Img = My.Resources.Vehicle_607
        Vehicles(207).Type = VehicleType.Trailer
        Vehicles(208).ID = 608
        Vehicles(208).Name = "Tug Stairs Trailer"
        Vehicles(208).Img = My.Resources.Vehicle_608
        Vehicles(208).Type = VehicleType.Trailer
        Vehicles(209).ID = 609
        Vehicles(209).Name = "Boxburg"
        Vehicles(209).Img = My.Resources.Vehicle_609
        Vehicles(209).Type = VehicleType.Industrial
        Vehicles(210).ID = 610
        Vehicles(210).Name = "Farm Trailer"
        Vehicles(210).Img = My.Resources.Vehicle_610
        Vehicles(210).Type = VehicleType.Trailer
        Vehicles(211).ID = 611
        Vehicles(211).Name = "Utility Trailer"
        Vehicles(211).Img = My.Resources.Vehicle_611
        Vehicles(211).Type = VehicleType.Trailer
        Vehicles(0).ID = 400
        Vehicles(0).Name = "Landstalker"
        Vehicles(0).Img = My.Resources.Vehicle_400
        Vehicles(0).Type = VehicleType.Off_Road
        Vehicles(1).ID = 401
        Vehicles(1).Name = "Bravura"
        Vehicles(1).Img = My.Resources.Vehicle_401
        Vehicles(1).Type = VehicleType.Saloon
        Vehicles(2).ID = 402
        Vehicles(2).Name = "Buffalo"
        Vehicles(2).Img = My.Resources.Vehicle_402
        Vehicles(2).Type = VehicleType.Sport
        Vehicles(3).ID = 403
        Vehicles(3).Name = "Linerunner"
        Vehicles(3).Img = My.Resources.Vehicle_403
        Vehicles(3).Type = VehicleType.Industrial
        Vehicles(4).ID = 404
        Vehicles(4).Name = "Perenniel"
        Vehicles(4).Img = My.Resources.Vehicle_404
        Vehicles(4).Type = VehicleType.Station_Wagon
        Vehicles(5).ID = 405
        Vehicles(5).Name = "Sentinel"
        Vehicles(5).Img = My.Resources.Vehicle_405
        Vehicles(5).Type = VehicleType.Saloon
        Vehicles(6).ID = 406
        Vehicles(6).Name = "Dumper"
        Vehicles(6).Img = My.Resources.Vehicle_406
        Vehicles(6).Type = VehicleType.Unique
        Vehicles(7).ID = 407
        Vehicles(7).Name = "Firetruck"
        Vehicles(7).Img = My.Resources.Vehicle_407
        Vehicles(7).Type = VehicleType.Public_Service
        Vehicles(8).ID = 408
        Vehicles(8).Name = "Trashmaster"
        Vehicles(8).Img = My.Resources.Vehicle_408
        Vehicles(8).Type = VehicleType.Industrial
        Vehicles(9).ID = 409
        Vehicles(9).Name = "Stretch"
        Vehicles(9).Img = My.Resources.Vehicle_409
        Vehicles(9).Type = VehicleType.Unique
        Vehicles(10).ID = 410
        Vehicles(10).Name = "Manana"
        Vehicles(10).Img = My.Resources.Vehicle_410
        Vehicles(10).Type = VehicleType.Saloon
        Vehicles(11).ID = 411
        Vehicles(11).Name = "Infernus"
        Vehicles(11).Img = My.Resources.Vehicle_411
        Vehicles(11).Type = VehicleType.Sport
        Vehicles(12).ID = 412
        Vehicles(12).Name = "Voodoo"
        Vehicles(12).Img = My.Resources.Vehicle_412
        Vehicles(12).Type = VehicleType.Lowriders
        Vehicles(13).ID = 413
        Vehicles(13).Name = "Pony"
        Vehicles(13).Img = My.Resources.Vehicle_413
        Vehicles(13).Type = VehicleType.Industrial
        Vehicles(14).ID = 414
        Vehicles(14).Name = "Mule"
        Vehicles(14).Img = My.Resources.Vehicle_414
        Vehicles(14).Type = VehicleType.Industrial
        Vehicles(15).ID = 415
        Vehicles(15).Name = "Cheetah"
        Vehicles(15).Img = My.Resources.Vehicle_415
        Vehicles(15).Type = VehicleType.Sport
        Vehicles(16).ID = 416
        Vehicles(16).Name = "Ambulance"
        Vehicles(16).Img = My.Resources.Vehicle_416
        Vehicles(16).Type = VehicleType.Public_Service
        Vehicles(17).ID = 417
        Vehicles(17).Name = "Leviathan"
        Vehicles(17).Img = My.Resources.Vehicle_417
        Vehicles(17).Type = VehicleType.Helicopter
        Vehicles(18).ID = 418
        Vehicles(18).Name = "Moonbeam"
        Vehicles(18).Img = My.Resources.Vehicle_418
        Vehicles(18).Type = VehicleType.Station_Wagon
        Vehicles(19).ID = 419
        Vehicles(19).Name = "Esperanto"
        Vehicles(19).Img = My.Resources.Vehicle_419
        Vehicles(19).Type = VehicleType.Saloon
        Vehicles(20).ID = 420
        Vehicles(20).Name = "Taxi"
        Vehicles(20).Img = My.Resources.Vehicle_420
        Vehicles(20).Type = VehicleType.Public_Service
        Vehicles(21).ID = 421
        Vehicles(21).Name = "Washington"
        Vehicles(21).Img = My.Resources.Vehicle_421
        Vehicles(21).Type = VehicleType.Saloon
        Vehicles(22).ID = 422
        Vehicles(22).Name = "Bobcat"
        Vehicles(22).Img = My.Resources.Vehicle_422
        Vehicles(22).Type = VehicleType.Industrial
        Vehicles(23).ID = 423
        Vehicles(23).Name = "Mr Whoopee"
        Vehicles(23).Img = My.Resources.Vehicle_423
        Vehicles(23).Type = VehicleType.Unique
        Vehicles(24).ID = 424
        Vehicles(24).Name = "BF Injection"
        Vehicles(24).Img = My.Resources.Vehicle_424
        Vehicles(24).Type = VehicleType.Off_Road
        Vehicles(25).ID = 425
        Vehicles(25).Name = "Hunter"
        Vehicles(25).Img = My.Resources.Vehicle_425
        Vehicles(25).Type = VehicleType.Helicopter
        Vehicles(26).ID = 426
        Vehicles(26).Name = "Premier"
        Vehicles(26).Img = My.Resources.Vehicle_426
        Vehicles(26).Type = VehicleType.Saloon
        Vehicles(27).ID = 427
        Vehicles(27).Name = "Enforcer"
        Vehicles(27).Img = My.Resources.Vehicle_427
        Vehicles(27).Type = VehicleType.Public_Service
        Vehicles(28).ID = 428
        Vehicles(28).Name = "Securicar"
        Vehicles(28).Img = My.Resources.Vehicle_428
        Vehicles(28).Type = VehicleType.Unique
        Vehicles(29).ID = 429
        Vehicles(29).Name = "Banshee"
        Vehicles(29).Img = My.Resources.Vehicle_429
        Vehicles(29).Type = VehicleType.Sport
        Vehicles(30).ID = 430
        Vehicles(30).Name = "Predator"
        Vehicles(30).Img = My.Resources.Vehicle_430
        Vehicles(30).Type = VehicleType.Boat
        Vehicles(31).ID = 431
        Vehicles(31).Name = "Bus"
        Vehicles(31).Img = My.Resources.Vehicle_431
        Vehicles(31).Type = VehicleType.Public_Service
        Vehicles(32).ID = 432
        Vehicles(32).Name = "Rhino"
        Vehicles(32).Img = My.Resources.Vehicle_432
        Vehicles(32).Type = VehicleType.Public_Service
        Vehicles(33).ID = 433
        Vehicles(33).Name = "Barracks"
        Vehicles(33).Img = My.Resources.Vehicle_433
        Vehicles(33).Type = VehicleType.Public_Service
        Vehicles(34).ID = 434
        Vehicles(34).Name = "Hotknife"
        Vehicles(34).Img = My.Resources.Vehicle_434
        Vehicles(34).Type = VehicleType.Unique
        Vehicles(35).ID = 435
        Vehicles(35).Name = "Article Trailer"
        Vehicles(35).Img = My.Resources.Vehicle_435
        Vehicles(35).Type = VehicleType.Trailer
        Vehicles(36).ID = 436
        Vehicles(36).Name = "Previon"
        Vehicles(36).Img = My.Resources.Vehicle_436
        Vehicles(36).Type = VehicleType.Saloon
        Vehicles(37).ID = 437
        Vehicles(37).Name = "Coach"
        Vehicles(37).Img = My.Resources.Vehicle_437
        Vehicles(37).Type = VehicleType.Public_Service
        Vehicles(38).ID = 438
        Vehicles(38).Name = "Cabbie"
        Vehicles(38).Img = My.Resources.Vehicle_438
        Vehicles(38).Type = VehicleType.Public_Service
        Vehicles(39).ID = 439
        Vehicles(39).Name = "Stallion"
        Vehicles(39).Img = My.Resources.Vehicle_439
        Vehicles(39).Type = VehicleType.Convertible
        Vehicles(40).ID = 440
        Vehicles(40).Name = "Rumpo"
        Vehicles(40).Img = My.Resources.Vehicle_440
        Vehicles(40).Type = VehicleType.Industrial
        Vehicles(41).ID = 441
        Vehicles(41).Name = "RC Bandit"
        Vehicles(41).Img = My.Resources.Vehicle_441
        Vehicles(41).Type = VehicleType.RC
        Vehicles(42).ID = 442
        Vehicles(42).Name = "Romero"
        Vehicles(42).Img = My.Resources.Vehicle_442
        Vehicles(42).Type = VehicleType.Unique
        Vehicles(43).ID = 443
        Vehicles(43).Name = "Packer"
        Vehicles(43).Img = My.Resources.Vehicle_443
        Vehicles(43).Type = VehicleType.Industrial
        Vehicles(44).ID = 444
        Vehicles(44).Name = "Monster"
        Vehicles(44).Img = My.Resources.Vehicle_444
        Vehicles(44).Type = VehicleType.Off_Road
        Vehicles(45).ID = 445
        Vehicles(45).Name = "Admiral"
        Vehicles(45).Img = My.Resources.Vehicle_445
        Vehicles(45).Type = VehicleType.Saloon
        Vehicles(46).ID = 446
        Vehicles(46).Name = "Squallo"
        Vehicles(46).Img = My.Resources.Vehicle_446
        Vehicles(46).Type = VehicleType.Boat
        Vehicles(47).ID = 447
        Vehicles(47).Name = "Seasparrow"
        Vehicles(47).Img = My.Resources.Vehicle_447
        Vehicles(47).Type = VehicleType.Helicopter
        Vehicles(48).ID = 448
        Vehicles(48).Name = "Pizzaboy"
        Vehicles(48).Img = My.Resources.Vehicle_448
        Vehicles(48).Type = VehicleType.Bike
        Vehicles(49).ID = 449
        Vehicles(49).Name = "Tram"
        Vehicles(49).Img = My.Resources.Vehicle_449
        Vehicles(49).Type = VehicleType.Unique
        Vehicles(50).ID = 450
        Vehicles(50).Name = "Article Trailer 2"
        Vehicles(50).Img = My.Resources.Vehicle_450
        Vehicles(50).Type = VehicleType.Trailer
        Vehicles(51).ID = 451
        Vehicles(51).Name = "Turismo"
        Vehicles(51).Img = My.Resources.Vehicle_451
        Vehicles(51).Type = VehicleType.Sport
        Vehicles(52).ID = 452
        Vehicles(52).Name = "Speeder"
        Vehicles(52).Img = My.Resources.Vehicle_452
        Vehicles(52).Type = VehicleType.Boat
        Vehicles(53).ID = 453
        Vehicles(53).Name = "Reefer"
        Vehicles(53).Img = My.Resources.Vehicle_453
        Vehicles(53).Type = VehicleType.Boat
        Vehicles(54).ID = 454
        Vehicles(54).Name = "Tropic"
        Vehicles(54).Img = My.Resources.Vehicle_454
        Vehicles(54).Type = VehicleType.Boat
        Vehicles(55).ID = 455
        Vehicles(55).Name = "Flatbed"
        Vehicles(55).Img = My.Resources.Vehicle_455
        Vehicles(55).Type = VehicleType.Industrial
        Vehicles(56).ID = 456
        Vehicles(56).Name = "Yankee"
        Vehicles(56).Img = My.Resources.Vehicle_456
        Vehicles(56).Type = VehicleType.Industrial
        Vehicles(57).ID = 457
        Vehicles(57).Name = "Caddy"
        Vehicles(57).Img = My.Resources.Vehicle_457
        Vehicles(57).Type = VehicleType.Unique
        Vehicles(58).ID = 458
        Vehicles(58).Name = "Solair"
        Vehicles(58).Img = My.Resources.Vehicle_458
        Vehicles(58).Type = VehicleType.Station_Wagon
        Vehicles(59).ID = 459
        Vehicles(59).Name = "Berkley's RC Van"
        Vehicles(59).Img = My.Resources.Vehicle_459
        Vehicles(59).Type = VehicleType.Industrial
        Vehicles(60).ID = 460
        Vehicles(60).Name = "Skimmer"
        Vehicles(60).Img = My.Resources.Vehicle_460
        Vehicles(60).Type = VehicleType.Airplane
        Vehicles(61).ID = 461
        Vehicles(61).Name = "PCJ-600"
        Vehicles(61).Img = My.Resources.Vehicle_461
        Vehicles(61).Type = VehicleType.Bike
        Vehicles(62).ID = 462
        Vehicles(62).Name = "Faggio"
        Vehicles(62).Img = My.Resources.Vehicle_462
        Vehicles(62).Type = VehicleType.Bike
        Vehicles(63).ID = 463
        Vehicles(63).Name = "Freeway"
        Vehicles(63).Img = My.Resources.Vehicle_463
        Vehicles(63).Type = VehicleType.Bike
        Vehicles(64).ID = 464
        Vehicles(64).Name = "RC Baron"
        Vehicles(64).Img = My.Resources.Vehicle_464
        Vehicles(64).Type = VehicleType.RC
        Vehicles(65).ID = 465
        Vehicles(65).Name = "RC Raider"
        Vehicles(65).Img = My.Resources.Vehicle_465
        Vehicles(65).Type = VehicleType.RC
        Vehicles(66).ID = 466
        Vehicles(66).Name = "Glendale"
        Vehicles(66).Img = My.Resources.Vehicle_466
        Vehicles(66).Type = VehicleType.Saloon
        Vehicles(67).ID = 467
        Vehicles(67).Name = "Oceanic"
        Vehicles(67).Img = My.Resources.Vehicle_467
        Vehicles(67).Type = VehicleType.Sport
        Vehicles(68).ID = 468
        Vehicles(68).Name = "Sanchez"
        Vehicles(68).Img = My.Resources.Vehicle_468
        Vehicles(68).Type = VehicleType.Bike
        Vehicles(69).ID = 469
        Vehicles(69).Name = "Sparrow"
        Vehicles(69).Img = My.Resources.Vehicle_469
        Vehicles(69).Type = VehicleType.Helicopter
        Vehicles(70).ID = 470
        Vehicles(70).Name = "Patriot"
        Vehicles(70).Img = My.Resources.Vehicle_470
        Vehicles(70).Type = VehicleType.Off_Road
        Vehicles(71).ID = 471
        Vehicles(71).Name = "Quad"
        Vehicles(71).Img = My.Resources.Vehicle_471
        Vehicles(71).Type = VehicleType.Bike
        Vehicles(72).ID = 472
        Vehicles(72).Name = "Coastguard"
        Vehicles(72).Img = My.Resources.Vehicle_472
        Vehicles(72).Type = VehicleType.Boat
        Vehicles(73).ID = 473
        Vehicles(73).Name = "Dinghy"
        Vehicles(73).Img = My.Resources.Vehicle_473
        Vehicles(73).Type = VehicleType.Boat
        Vehicles(74).ID = 474
        Vehicles(74).Name = "Hermes"
        Vehicles(74).Img = My.Resources.Vehicle_474
        Vehicles(74).Type = VehicleType.Sport
        Vehicles(75).ID = 475
        Vehicles(75).Name = "Sabre"
        Vehicles(75).Img = My.Resources.Vehicle_475
        Vehicles(75).Type = VehicleType.Sport
        Vehicles(76).ID = 476
        Vehicles(76).Name = "Rustler"
        Vehicles(76).Img = My.Resources.Vehicle_476
        Vehicles(76).Type = VehicleType.Airplane
        Vehicles(77).ID = 477
        Vehicles(77).Name = "ZR-350"
        Vehicles(77).Img = My.Resources.Vehicle_477
        Vehicles(77).Type = VehicleType.Sport
        Vehicles(78).ID = 478
        Vehicles(78).Name = "Walton"
        Vehicles(78).Img = My.Resources.Vehicle_478
        Vehicles(78).Type = VehicleType.Industrial
        Vehicles(79).ID = 479
        Vehicles(79).Name = "Regina"
        Vehicles(79).Img = My.Resources.Vehicle_479
        Vehicles(79).Type = VehicleType.Station_Wagon
        Vehicles(80).ID = 480
        Vehicles(80).Name = "Comet"
        Vehicles(80).Img = My.Resources.Vehicle_480
        Vehicles(80).Type = VehicleType.Convertible
        Vehicles(81).ID = 481
        Vehicles(81).Name = "BMX"
        Vehicles(81).Img = My.Resources.Vehicle_481
        Vehicles(81).Type = VehicleType.Bike
        Vehicles(82).ID = 482
        Vehicles(82).Name = "Burrito"
        Vehicles(82).Img = My.Resources.Vehicle_482
        Vehicles(82).Type = VehicleType.Industrial
        Vehicles(83).ID = 483
        Vehicles(83).Name = "Camper"
        Vehicles(83).Img = My.Resources.Vehicle_483
        Vehicles(83).Type = VehicleType.Unique
        Vehicles(84).ID = 484
        Vehicles(84).Name = "Marquis"
        Vehicles(84).Img = My.Resources.Vehicle_484
        Vehicles(84).Type = VehicleType.Boat
        Vehicles(85).ID = 485
        Vehicles(85).Name = "Baggage"
        Vehicles(85).Img = My.Resources.Vehicle_485
        Vehicles(85).Type = VehicleType.Unique
        Vehicles(86).ID = 486
        Vehicles(86).Name = "Dozer"
        Vehicles(86).Img = My.Resources.Vehicle_486
        Vehicles(86).Type = VehicleType.Unique
        Vehicles(87).ID = 487
        Vehicles(87).Name = "Maverick"
        Vehicles(87).Img = My.Resources.Vehicle_487
        Vehicles(87).Type = VehicleType.Helicopter
        Vehicles(88).ID = 488
        Vehicles(88).Name = "SAN News Maverick"
        Vehicles(88).Img = My.Resources.Vehicle_488
        Vehicles(88).Type = VehicleType.Helicopter
        Vehicles(89).ID = 489
        Vehicles(89).Name = "Rancher"
        Vehicles(89).Img = My.Resources.Vehicle_489
        Vehicles(89).Type = VehicleType.Off_Road
        Vehicles(90).ID = 490
        Vehicles(90).Name = "FBI Rancher"
        Vehicles(90).Img = My.Resources.Vehicle_490
        Vehicles(90).Type = VehicleType.Public_Service
        Vehicles(91).ID = 491
        Vehicles(91).Name = "Virgo"
        Vehicles(91).Img = My.Resources.Vehicle_491
        Vehicles(91).Type = VehicleType.Saloon
        Vehicles(92).ID = 492
        Vehicles(92).Name = "Greenwood"
        Vehicles(92).Img = My.Resources.Vehicle_492
        Vehicles(92).Type = VehicleType.Saloon
        Vehicles(93).ID = 493
        Vehicles(93).Name = "Jetmax"
        Vehicles(93).Img = My.Resources.Vehicle_493
        Vehicles(93).Type = VehicleType.Boat
        Vehicles(94).ID = 494
        Vehicles(94).Name = "Hotring Racer"
        Vehicles(94).Img = My.Resources.Vehicle_494
        Vehicles(94).Type = VehicleType.Sport
        Vehicles(95).ID = 495
        Vehicles(95).Name = "Sandking"
        Vehicles(95).Img = My.Resources.Vehicle_495
        Vehicles(95).Type = VehicleType.Off_Road
        Vehicles(96).ID = 496
        Vehicles(96).Name = "Blista Compact"
        Vehicles(96).Img = My.Resources.Vehicle_496
        Vehicles(96).Type = VehicleType.Sport
        Vehicles(97).ID = 497
        Vehicles(97).Name = "Police Maverick"
        Vehicles(97).Img = My.Resources.Vehicle_497
        Vehicles(97).Type = VehicleType.Helicopter
        Vehicles(98).ID = 498
        Vehicles(98).Name = "Boxville"
        Vehicles(98).Img = My.Resources.Vehicle_498
        Vehicles(98).Type = VehicleType.Industrial
        Vehicles(99).ID = 499
        Vehicles(99).Name = "Benson"
        Vehicles(99).Img = My.Resources.Vehicle_499
        Vehicles(99).Type = VehicleType.Industrial
        Vehicles(100).ID = 500
        Vehicles(100).Name = "Mesa"
        Vehicles(100).Img = My.Resources.Vehicle_500
        Vehicles(100).Type = VehicleType.Off_Road
        Vehicles(101).ID = 501
        Vehicles(101).Name = "RC Goblin"
        Vehicles(101).Img = My.Resources.Vehicle_501
        Vehicles(101).Type = VehicleType.RC
        Vehicles(102).ID = 502
        Vehicles(102).Name = "Hotring Racer"
        Vehicles(102).Img = My.Resources.Vehicle_502
        Vehicles(102).Type = VehicleType.Sport
        Vehicles(103).ID = 503
        Vehicles(103).Name = "Hotring Racer"
        Vehicles(103).Img = My.Resources.Vehicle_503
        Vehicles(103).Type = VehicleType.Sport
        Vehicles(104).ID = 504
        Vehicles(104).Name = "Bloodring Banger"
        Vehicles(104).Img = My.Resources.Vehicle_504
        Vehicles(104).Type = VehicleType.Saloon
        Vehicles(105).ID = 505
        Vehicles(105).Name = "Rancher"
        Vehicles(105).Img = My.Resources.Vehicle_505
        Vehicles(105).Type = VehicleType.Off_Road
        Vehicles(106).ID = 506
        Vehicles(106).Name = "Super GT"
        Vehicles(106).Img = My.Resources.Vehicle_506
        Vehicles(106).Type = VehicleType.Sport
        Vehicles(107).ID = 507
        Vehicles(107).Name = "Elegant"
        Vehicles(107).Img = My.Resources.Vehicle_507
        Vehicles(107).Type = VehicleType.Saloon
        Vehicles(108).ID = 508
        Vehicles(108).Name = "Journey"
        Vehicles(108).Img = My.Resources.Vehicle_508
        Vehicles(108).Type = VehicleType.Unique
        Vehicles(109).ID = 509
        Vehicles(109).Name = "Bike"
        Vehicles(109).Img = My.Resources.Vehicle_509
        Vehicles(109).Type = VehicleType.Bike
        Vehicles(110).ID = 510
        Vehicles(110).Name = "Mountain Bike"
        Vehicles(110).Img = My.Resources.Vehicle_510
        Vehicles(110).Type = VehicleType.Bike
        Vehicles(111).ID = 511
        Vehicles(111).Name = "Beagle"
        Vehicles(111).Img = My.Resources.Vehicle_511
        Vehicles(111).Type = VehicleType.Airplane
        Vehicles(112).ID = 512
        Vehicles(112).Name = "Cropduster"
        Vehicles(112).Img = My.Resources.Vehicle_512
        Vehicles(112).Type = VehicleType.Airplane
        Vehicles(113).ID = 513
        Vehicles(113).Name = "Stuntplane"
        Vehicles(113).Img = My.Resources.Vehicle_513
        Vehicles(113).Type = VehicleType.Airplane
        Vehicles(114).ID = 514
        Vehicles(114).Name = "Tanker"
        Vehicles(114).Img = My.Resources.Vehicle_514
        Vehicles(114).Type = VehicleType.Industrial
        Vehicles(115).ID = 515
        Vehicles(115).Name = "Roadtrain"
        Vehicles(115).Img = My.Resources.Vehicle_515
        Vehicles(115).Type = VehicleType.Industrial
        Vehicles(116).ID = 516
        Vehicles(116).Name = "Nebula"
        Vehicles(116).Img = My.Resources.Vehicle_516
        Vehicles(116).Type = VehicleType.Saloon
        Vehicles(117).ID = 517
        Vehicles(117).Name = "Majestic"
        Vehicles(117).Img = My.Resources.Vehicle_517
        Vehicles(117).Type = VehicleType.Saloon
        Vehicles(118).ID = 518
        Vehicles(118).Name = "Buccaneer"
        Vehicles(118).Img = My.Resources.Vehicle_518
        Vehicles(118).Type = VehicleType.Saloon
        Vehicles(119).ID = 519
        Vehicles(119).Name = "Shamal"
        Vehicles(119).Img = My.Resources.Vehicle_519
        Vehicles(119).Type = VehicleType.Airplane
        Vehicles(120).ID = 520
        Vehicles(120).Name = "Hydra"
        Vehicles(120).Img = My.Resources.Vehicle_520
        Vehicles(120).Type = VehicleType.Airplane
        Vehicles(121).ID = 521
        Vehicles(121).Name = "FCR-900"
        Vehicles(121).Img = My.Resources.Vehicle_521
        Vehicles(121).Type = VehicleType.Bike
        Vehicles(122).ID = 522
        Vehicles(122).Name = "NRG-500"
        Vehicles(122).Img = My.Resources.Vehicle_522
        Vehicles(122).Type = VehicleType.Bike
        Vehicles(123).ID = 523
        Vehicles(123).Name = "Cop Bike (HPV-1000)"
        Vehicles(123).Img = My.Resources.Vehicle_523
        Vehicles(123).Type = VehicleType.Bike
        Vehicles(124).ID = 524
        Vehicles(124).Name = "Cement Truck"
        Vehicles(124).Img = My.Resources.Vehicle_524
        Vehicles(124).Type = VehicleType.Industrial
        Vehicles(125).ID = 525
        Vehicles(125).Name = "Towtruck"
        Vehicles(125).Img = My.Resources.Vehicle_525
        Vehicles(125).Type = VehicleType.Unique
        Vehicles(126).ID = 526
        Vehicles(126).Name = "Fortune"
        Vehicles(126).Img = My.Resources.Vehicle_526
        Vehicles(126).Type = VehicleType.Saloon
        Vehicles(127).ID = 527
        Vehicles(127).Name = "Cadrona"
        Vehicles(127).Img = My.Resources.Vehicle_527
        Vehicles(127).Type = VehicleType.Saloon
        Vehicles(128).ID = 528
        Vehicles(128).Name = "FBI Truck"
        Vehicles(128).Img = My.Resources.Vehicle_528
        Vehicles(128).Type = VehicleType.Public_Service
        Vehicles(129).ID = 529
        Vehicles(129).Name = "Willard"
        Vehicles(129).Img = My.Resources.Vehicle_529
        Vehicles(129).Type = VehicleType.Saloon
        Vehicles(130).ID = 530
        Vehicles(130).Name = "Forklift"
        Vehicles(130).Img = My.Resources.Vehicle_530
        Vehicles(130).Type = VehicleType.Unique
        Vehicles(131).ID = 531
        Vehicles(131).Name = "Tractor"
        Vehicles(131).Img = My.Resources.Vehicle_531
        Vehicles(131).Type = VehicleType.Industrial
        Vehicles(132).ID = 532
        Vehicles(132).Name = "Combine Harvester"
        Vehicles(132).Img = My.Resources.Vehicle_532
        Vehicles(132).Type = VehicleType.Unique
        Vehicles(133).ID = 533
        Vehicles(133).Name = "Feltzer"
        Vehicles(133).Img = My.Resources.Vehicle_533
        Vehicles(133).Type = VehicleType.Convertible
        Vehicles(134).ID = 534
        Vehicles(134).Name = "Remington"
        Vehicles(134).Img = My.Resources.Vehicle_534
        Vehicles(134).Type = VehicleType.Lowriders
        Vehicles(135).ID = 535
        Vehicles(135).Name = "Slamvan"
        Vehicles(135).Img = My.Resources.Vehicle_535
        Vehicles(135).Type = VehicleType.Lowriders
        Vehicles(136).ID = 536
        Vehicles(136).Name = "Blade"
        Vehicles(136).Img = My.Resources.Vehicle_536
        Vehicles(136).Type = VehicleType.Lowriders
        Vehicles(137).ID = 537
        Vehicles(137).Name = "Freight (Train)"
        Vehicles(137).Img = My.Resources.Vehicle_537
        Vehicles(137).Type = VehicleType.Unique
        Vehicles(138).ID = 538
        Vehicles(138).Name = "Brownstreak (Train)"
        Vehicles(138).Img = My.Resources.Vehicle_538
        Vehicles(138).Type = VehicleType.Unique
        Vehicles(139).ID = 539
        Vehicles(139).Name = "Vortex"
        Vehicles(139).Img = My.Resources.Vehicle_539
        Vehicles(139).Type = VehicleType.Unique
        Vehicles(140).ID = 540
        Vehicles(140).Name = "Vincent"
        Vehicles(140).Img = My.Resources.Vehicle_540
        Vehicles(140).Type = VehicleType.Sport
        Vehicles(141).ID = 541
        Vehicles(141).Name = "Bullet"
        Vehicles(141).Img = My.Resources.Vehicle_541
        Vehicles(141).Type = VehicleType.Sport
        Vehicles(142).ID = 542
        Vehicles(142).Name = "Clover"
        Vehicles(142).Img = My.Resources.Vehicle_542
        Vehicles(142).Type = VehicleType.Saloon
        Vehicles(143).ID = 543
        Vehicles(143).Name = "Sadler"
        Vehicles(143).Img = My.Resources.Vehicle_543
        Vehicles(143).Type = VehicleType.Industrial
        Vehicles(144).ID = 544
        Vehicles(144).Name = "Firetruck LA"
        Vehicles(144).Img = My.Resources.Vehicle_544
        Vehicles(144).Type = VehicleType.Public_Service
        Vehicles(145).ID = 545
        Vehicles(145).Name = "Hustler"
        Vehicles(145).Img = My.Resources.Vehicle_545
        Vehicles(145).Type = VehicleType.Unique
        Vehicles(146).ID = 546
        Vehicles(146).Name = "Intruder"
        Vehicles(146).Img = My.Resources.Vehicle_546
        Vehicles(146).Type = VehicleType.Saloon
        Vehicles(147).ID = 547
        Vehicles(147).Name = "Primo"
        Vehicles(147).Img = My.Resources.Vehicle_547
        Vehicles(147).Type = VehicleType.Saloon
        Vehicles(148).ID = 548
        Vehicles(148).Name = "Cargobob"
        Vehicles(148).Img = My.Resources.Vehicle_548
        Vehicles(148).Type = VehicleType.Helicopter
        Vehicles(149).ID = 549
        Vehicles(149).Name = "Tampa"
        Vehicles(149).Img = My.Resources.Vehicle_549
        Vehicles(149).Type = VehicleType.Saloon
        Vehicles(150).ID = 550
        Vehicles(150).Name = "Sunrise"
        Vehicles(150).Img = My.Resources.Vehicle_550
        Vehicles(150).Type = VehicleType.Saloon
        Vehicles(151).ID = 551
        Vehicles(151).Name = "Merit"
        Vehicles(151).Img = My.Resources.Vehicle_551
        Vehicles(151).Type = VehicleType.Saloon
        Vehicles(152).ID = 552
        Vehicles(152).Name = "Utility Van"
        Vehicles(152).Img = My.Resources.Vehicle_552
        Vehicles(152).Type = VehicleType.Industrial
        Vehicles(153).ID = 553
        Vehicles(153).Name = "Nevada"
        Vehicles(153).Img = My.Resources.Vehicle_553
        Vehicles(153).Type = VehicleType.Airplane
        Vehicles(154).ID = 554
        Vehicles(154).Name = "Yosemite"
        Vehicles(154).Img = My.Resources.Vehicle_554
        Vehicles(154).Type = VehicleType.Industrial
        Vehicles(155).ID = 555
        Vehicles(155).Name = "Windsor"
        Vehicles(155).Img = My.Resources.Vehicle_555
        Vehicles(155).Type = VehicleType.Convertible
        Vehicles(156).ID = 556
        Vehicles(156).Name = "Monster A"
        Vehicles(156).Img = My.Resources.Vehicle_556
        Vehicles(156).Type = VehicleType.Off_Road
        Vehicles(157).ID = 557
        Vehicles(157).Name = "Monster B"
        Vehicles(157).Img = My.Resources.Vehicle_557
        Vehicles(157).Type = VehicleType.Off_Road
        Vehicles(158).ID = 558
        Vehicles(158).Name = "Uranus"
        Vehicles(158).Img = My.Resources.Vehicle_558
        Vehicles(158).Type = VehicleType.Sport
        Vehicles(159).ID = 559
        Vehicles(159).Name = "Jester"
        Vehicles(159).Img = My.Resources.Vehicle_559
        Vehicles(159).Type = VehicleType.Sport
        Vehicles(160).ID = 560
        Vehicles(160).Name = "Sultan"
        Vehicles(160).Img = My.Resources.Vehicle_560
        Vehicles(160).Type = VehicleType.Saloon
        Vehicles(161).ID = 561
        Vehicles(161).Name = "Stratum"
        Vehicles(161).Img = My.Resources.Vehicle_561
        Vehicles(161).Type = VehicleType.Station_Wagon
        Vehicles(162).ID = 562
        Vehicles(162).Name = "Elegy"
        Vehicles(162).Img = My.Resources.Vehicle_562
        Vehicles(162).Type = VehicleType.Saloon
        Vehicles(163).ID = 563
        Vehicles(163).Name = "Raindance"
        Vehicles(163).Img = My.Resources.Vehicle_563
        Vehicles(163).Type = VehicleType.Helicopter
        Vehicles(164).ID = 564
        Vehicles(164).Name = "RC Tiger"
        Vehicles(164).Img = My.Resources.Vehicle_564
        Vehicles(164).Type = VehicleType.RC
        Vehicles(165).ID = 565
        Vehicles(165).Name = "Flash"
        Vehicles(165).Img = My.Resources.Vehicle_565
        Vehicles(165).Type = VehicleType.Sport
        Vehicles(166).ID = 566
        Vehicles(166).Name = "Tahoma"
        Vehicles(166).Img = My.Resources.Vehicle_566
        Vehicles(166).Type = VehicleType.Lowriders
        Vehicles(167).ID = 567
        Vehicles(167).Name = "Savanna"
        Vehicles(167).Img = My.Resources.Vehicle_567
        Vehicles(167).Type = VehicleType.Lowriders
        Vehicles(168).ID = 568
        Vehicles(168).Name = "Bandito"
        Vehicles(168).Img = My.Resources.Vehicle_568
        Vehicles(168).Type = VehicleType.Off_Road
        Vehicles(169).ID = 569
        Vehicles(169).Name = "Freight Flat Trailer (Train)"
        Vehicles(169).Img = My.Resources.Vehicle_569
        Vehicles(169).Type = VehicleType.Trailer
        Vehicles(170).ID = 570
        Vehicles(170).Name = "Streak Trailer (Train)"
        Vehicles(170).Img = My.Resources.Vehicle_570
        Vehicles(170).Type = VehicleType.Trailer
        Vehicles(171).ID = 571
        Vehicles(171).Name = "Kart"
        Vehicles(171).Img = My.Resources.Vehicle_571
        Vehicles(171).Type = VehicleType.Unique
        Vehicles(172).ID = 572
        Vehicles(172).Name = "Mower"
        Vehicles(172).Img = My.Resources.Vehicle_572
        Vehicles(172).Type = VehicleType.Unique
        Vehicles(173).ID = 573
        Vehicles(173).Name = "Dune"
        Vehicles(173).Img = My.Resources.Vehicle_573
        Vehicles(173).Type = VehicleType.Off_Road
        Vehicles(174).ID = 574
        Vehicles(174).Name = "Sweeper"
        Vehicles(174).Img = My.Resources.Vehicle_574
        Vehicles(174).Type = VehicleType.Unique
        Vehicles(175).ID = 575
        Vehicles(175).Name = "Broadway"
        Vehicles(175).Img = My.Resources.Vehicle_575
        Vehicles(175).Type = VehicleType.Lowriders
        Vehicles(176).ID = 576
        Vehicles(176).Name = "Tornado"
        Vehicles(176).Img = My.Resources.Vehicle_576
        Vehicles(176).Type = VehicleType.Lowriders
        Vehicles(177).ID = 577
        Vehicles(177).Name = "AT-400"
        Vehicles(177).Img = My.Resources.Vehicle_577
        Vehicles(177).Type = VehicleType.Airplane
        Vehicles(178).ID = 578
        Vehicles(178).Name = "DFT-30"
        Vehicles(178).Img = My.Resources.Vehicle_578
        Vehicles(178).Type = VehicleType.Industrial
        Vehicles(179).ID = 579
        Vehicles(179).Name = "Huntley"
        Vehicles(179).Img = My.Resources.Vehicle_579
        Vehicles(179).Type = VehicleType.Off_Road
        Vehicles(180).ID = 580
        Vehicles(180).Name = "Stafford"
        Vehicles(180).Img = My.Resources.Vehicle_580
        Vehicles(180).Type = VehicleType.Saloon
        Vehicles(181).ID = 581
        Vehicles(181).Name = "BF-400"
        Vehicles(181).Img = My.Resources.Vehicle_581
        Vehicles(181).Type = VehicleType.Bike
        Vehicles(182).ID = 582
        Vehicles(182).Name = "Newsvan"
        Vehicles(182).Img = My.Resources.Vehicle_582
        Vehicles(182).Type = VehicleType.Industrial
        Vehicles(183).ID = 583
        Vehicles(183).Name = "Tug"
        Vehicles(183).Img = My.Resources.Vehicle_583
        Vehicles(183).Type = VehicleType.Unique
        Vehicles(184).ID = 584
        Vehicles(184).Name = "Petrol Tanker"
        Vehicles(184).Img = My.Resources.Vehicle_584
        Vehicles(184).Type = VehicleType.Trailer
        Vehicles(185).ID = 585
        Vehicles(185).Name = "Emperor"
        Vehicles(185).Img = My.Resources.Vehicle_585
        Vehicles(185).Type = VehicleType.Saloon
        Vehicles(186).ID = 586
        Vehicles(186).Name = "Wayfarer"
        Vehicles(186).Img = My.Resources.Vehicle_586
        Vehicles(186).Type = VehicleType.Bike
        Vehicles(187).ID = 587
        Vehicles(187).Name = "Euros"
        Vehicles(187).Img = My.Resources.Vehicle_587
        Vehicles(187).Type = VehicleType.Sport
        Vehicles(188).ID = 588
        Vehicles(188).Name = "Hotdog"
        Vehicles(188).Img = My.Resources.Vehicle_588
        Vehicles(188).Type = VehicleType.Unique
        Vehicles(189).ID = 589
        Vehicles(189).Name = "Club"
        Vehicles(189).Img = My.Resources.Vehicle_589
        Vehicles(189).Type = VehicleType.Sport
        Vehicles(190).ID = 590
        Vehicles(190).Name = "Freight Box Trailer (Train)"
        Vehicles(190).Img = My.Resources.Vehicle_590
        Vehicles(190).Type = VehicleType.Trailer
        Vehicles(191).ID = 591
        Vehicles(191).Name = "Article Trailer 3"
        Vehicles(191).Img = My.Resources.Vehicle_591
        Vehicles(191).Type = VehicleType.Trailer
        Vehicles(192).ID = 592
        Vehicles(192).Name = "Andromada"
        Vehicles(192).Img = My.Resources.Vehicle_592
        Vehicles(192).Type = VehicleType.Airplane
        Vehicles(193).ID = 593
        Vehicles(193).Name = "Dodo"
        Vehicles(193).Img = My.Resources.Vehicle_593
        Vehicles(193).Type = VehicleType.Airplane
        Vehicles(194).ID = 594
        Vehicles(194).Name = "RC Cam"
        Vehicles(194).Img = My.Resources.Vehicle_594
        Vehicles(194).Type = VehicleType.RC
        Vehicles(195).ID = 595
        Vehicles(195).Name = "Launch"
        Vehicles(195).Img = My.Resources.Vehicle_595
        Vehicles(195).Type = VehicleType.Boat
        Vehicles(196).ID = 596
        Vehicles(196).Name = "Police Car (LSPD)"
        Vehicles(196).Img = My.Resources.Vehicle_596
        Vehicles(196).Type = VehicleType.Public_Service
        Vehicles(197).ID = 597
        Vehicles(197).Name = "Police Car (SFPD)"
        Vehicles(197).Img = My.Resources.Vehicle_597
        Vehicles(197).Type = VehicleType.Public_Service
        Vehicles(198).ID = 598
        Vehicles(198).Name = "Police Car (LVPD)"
        Vehicles(198).Img = My.Resources.Vehicle_598
        Vehicles(198).Type = VehicleType.Public_Service
        Vehicles(199).ID = 599
        Vehicles(199).Name = "Police Ranger"
        Vehicles(199).Img = My.Resources.Vehicle_599
        Vehicles(199).Type = VehicleType.Public_Service
        Vehicles(200).ID = 600
        Vehicles(200).Name = "Picador"
        Vehicles(200).Img = My.Resources.Vehicle_600
        Vehicles(200).Type = VehicleType.Industrial
        Vehicles(201).ID = 601
        Vehicles(201).Name = "S.W.A.T."
        Vehicles(201).Img = My.Resources.Vehicle_601
        Vehicles(201).Type = VehicleType.Public_Service
        Vehicles(202).ID = 602
        Vehicles(202).Name = "Alpha"
        Vehicles(202).Img = My.Resources.Vehicle_602
        Vehicles(202).Type = VehicleType.Sport
        Vehicles(203).ID = 603
        Vehicles(203).Name = "Phoenix"
        Vehicles(203).Img = My.Resources.Vehicle_603
        Vehicles(203).Type = VehicleType.Sport
        Vehicles(204).ID = 604
        Vehicles(204).Name = "Glendale Shit"
        Vehicles(204).Img = My.Resources.Vehicle_604
        Vehicles(204).Type = VehicleType.Saloon
        Vehicles(205).ID = 605
        Vehicles(205).Name = "Sadler Shit"
        Vehicles(205).Img = My.Resources.Vehicle_605
        Vehicles(205).Type = VehicleType.Industrial
        Vehicles(206).ID = 606
        Vehicles(206).Name = "Baggage Trailer A"
        Vehicles(206).Img = My.Resources.Vehicle_606
        Vehicles(206).Type = VehicleType.Trailer
        Vehicles(207).ID = 607
        Vehicles(207).Name = "Baggage Trailer B"
        Vehicles(207).Img = My.Resources.Vehicle_607
        Vehicles(207).Type = VehicleType.Trailer
        Vehicles(208).ID = 608
        Vehicles(208).Name = "Tug Stairs Trailer"
        Vehicles(208).Img = My.Resources.Vehicle_608
        Vehicles(208).Type = VehicleType.Trailer
        Vehicles(209).ID = 609
        Vehicles(209).Name = "Boxburg"
        Vehicles(209).Img = My.Resources.Vehicle_609
        Vehicles(209).Type = VehicleType.Industrial
        Vehicles(210).ID = 610
        Vehicles(210).Name = "Farm Trailer"
        Vehicles(210).Img = My.Resources.Vehicle_610
        Vehicles(210).Type = VehicleType.Trailer
        Vehicles(211).ID = 611
        Vehicles(211).Name = "Utility Trailer"
        Vehicles(211).Img = My.Resources.Vehicle_611
        Vehicles(211).Type = VehicleType.Trailer
    End Sub

#End Region

#Region "Sounds"

    Public Sub FillSounds()
        FillSoundsList()
        Dim count As Integer
        For Each value As String In Sounds.Keys
            Main.TreeView4.Nodes.Add(value)
            Splash.Label1.Text = String.Format("Loading sounds ({0}/7071)", count)
            Splash.ProgressBar1.Value += 1
            count += 1
        Next
    End Sub

    Private Sub FillSoundsList()
        Splash.Label1.Text = "Loading sounds..."
        Sounds.Add("SOUND_DISABLE_HELI_AUDIO", 1000)
        Sounds.Add("SOUND_ENABLE_HELI_AUDIO", 1001)
        Sounds.Add("SOUND_CEILING_VENT_LAND", 1002)
        Sounds.Add("SOUND_WAREHOUSE_DOOR_SLIDE_START", 1003)
        Sounds.Add("SOUND_WAREHOUSE_DOOR_SLIDE_STOP", 1004)
        Sounds.Add("SOUND_CLAXON_START", 1005)
        Sounds.Add("SOUND_CLAXON_STOP", 1006)
        Sounds.Add("SOUND_BLAST_DOOR_SLIDE_START", 1007)
        Sounds.Add("SOUND_BLAST_DOOR_SLIDE_STOP", 1008)
        Sounds.Add("SOUND_BONNET_DENT", 1009)
        Sounds.Add("SOUND_BASKETBALL_BOUNCE", 1010)
        Sounds.Add("SOUND_BASKETBALL_HIT_HOOP", 1011)
        Sounds.Add("SOUND_BASKETBALL_SCORE", 1012)
        Sounds.Add("SOUND_POOL_BREAK", 1013)
        Sounds.Add("SOUND_POOL_HIT_WHITE", 1014)
        Sounds.Add("SOUND_POOL_BALL_HIT_BALL", 1015)
        Sounds.Add("SOUND_POOL_HIT_CUSHION", 1016)
        Sounds.Add("SOUND_POOL_BALL_POT", 1017)
        Sounds.Add("SOUND_POOL_CHALK_CUE", 1018)
        Sounds.Add("SOUND_CRANE_ENTER", 1019)
        Sounds.Add("SOUND_CRANE_MOVE_START", 1020)
        Sounds.Add("SOUND_CRANE_MOVE_STOP", 1021)
        Sounds.Add("SOUND_CRANE_EXIT", 1022)
        Sounds.Add("SOUND_CRANE_SMASH_PORTACABIN", 1023)
        Sounds.Add("SOUND_CONTAINER_COLLISION", 1024)
        Sounds.Add("SOUND_VIDEO_POKER_PAYOUT", 1025)
        Sounds.Add("SOUND_VIDEO_POKER_BUTTON", 1026)
        Sounds.Add("SOUND_WHEEL_OF_FORTUNE_CLACKER", 1027)
        Sounds.Add("SOUND_KEYPAD_BEEP", 1028)
        Sounds.Add("SOUND_KEYPAD_PASS", 1029)
        Sounds.Add("SOUND_KEYPAD_FAIL", 1030)
        Sounds.Add("SOUND_SHOOTING_RANGE_TARGET_SHATTER", 1031)
        Sounds.Add("SOUND_SHOOTING_RANGE_TARGET_DROP", 1032)
        Sounds.Add("SOUND_SHOOTING_RANGE_TARGET_MOVE_START", 1033)
        Sounds.Add("SOUND_SHOOTING_RANGE_TARGET_MOVE_STOP", 1034)
        Sounds.Add("SOUND_SHUTTER_DOOR_START", 1035)
        Sounds.Add("SOUND_SHUTTER_DOOR_STOP", 1036)
        Sounds.Add("SOUND_FREEFALL_START", 1037)
        Sounds.Add("SOUND_FREEFALL_STOP", 1038)
        Sounds.Add("SOUND_PARACHUTE_OPEN", 1039)
        Sounds.Add("SOUND_PARACHUTE_COLLAPSE", 1040)
        Sounds.Add("SOUND_DUAL_SHOOT", 1041)
        Sounds.Add("SOUND_DUAL_THRUST", 1042)
        Sounds.Add("SOUND_DUAL_EXPLOSION_SHORT", 1043)
        Sounds.Add("SOUND_DUAL_EXPLOSION_LONG", 1044)
        Sounds.Add("SOUND_DUAL_MENU_SELECT", 1045)
        Sounds.Add("SOUND_DUAL_MENU_DESELECT", 1046)
        Sounds.Add("SOUND_DUAL_GAME_OVER", 1047)
        Sounds.Add("SOUND_DUAL_PICKUP_LIGHT", 1048)
        Sounds.Add("SOUND_DUAL_PICKUP_DARK", 1049)
        Sounds.Add("SOUND_DUAL_TOUCH_DARK", 1050)
        Sounds.Add("SOUND_DUAL_TOUCH_LIGHT", 1051)
        Sounds.Add("SOUND_AMMUNATION_BUY_WEAPON", 1052)
        Sounds.Add("SOUND_AMMUNATION_BUY_WEAPON_DENIED", 1053)
        Sounds.Add("SOUND_SHOP_BUY", 1054)
        Sounds.Add("SOUND_SHOP_BUY_DENIED", 1055)
        Sounds.Add("SOUND_RACE_321", 1056)
        Sounds.Add("SOUND_RACE_GO", 1057)
        Sounds.Add("SOUND_PART_MISSION_COMPLETE", 1058)
        Sounds.Add("SOUND_GOGO_PLAYER_FIRE", 1059)
        Sounds.Add("SOUND_GOGO_ENEMY_FIRE", 1060)
        Sounds.Add("SOUND_GOGO_EXPLOSION", 1061)
        Sounds.Add("SOUND_GOGO_TRACK_START", 1062)
        Sounds.Add("SOUND_GOGO_TRACK_STOP", 1063)
        Sounds.Add("SOUND_GOGO_SELECT", 1064)
        Sounds.Add("SOUND_GOGO_ACCEPT", 1065)
        Sounds.Add("SOUND_GOGO_DECLINE", 1066)
        Sounds.Add("SOUND_GOGO_GAME_OVER", 1067)
        Sounds.Add("SOUND_DUAL_TRACK_START", 1068)
        Sounds.Add("SOUND_DUAL_TRACK_STOP", 1069)
        Sounds.Add("SOUND_BEE_ZAP", 1070)
        Sounds.Add("SOUND_BEE_PICKUP", 1071)
        Sounds.Add("SOUND_BEE_DROP", 1072)
        Sounds.Add("SOUND_BEE_SELECT", 1073)
        Sounds.Add("SOUND_BEE_ACCEPT", 1074)
        Sounds.Add("SOUND_BEE_DECLINE", 1075)
        Sounds.Add("SOUND_BEE_TRACK_START", 1076)
        Sounds.Add("SOUND_BEE_TRACK_STOP", 1077)
        Sounds.Add("SOUND_BEE_GAME_OVER", 1078)
        Sounds.Add("SOUND_FREEZER_OPEN", 1079)
        Sounds.Add("SOUND_FREEZER_CLOSE", 1080)
        Sounds.Add("SOUND_MEAT_TRACK_START", 1081)
        Sounds.Add("SOUND_MEAT_TRACK_STOP", 1082)
        Sounds.Add("SOUND_ROULETTE_ADD_CASH", 1083)
        Sounds.Add("SOUND_ROULETTE_REMOVE_CASH", 1084)
        Sounds.Add("SOUND_ROULETTE_NO_CASH", 1085)
        Sounds.Add("SOUND_ROULETTE_SPIN", 1086)
        Sounds.Add("SOUND_BANDIT_INSERT_COIN", 1087)
        Sounds.Add("SOUND_BANDIT_WHEEL_STOP", 1088)
        Sounds.Add("SOUND_BANDIT_WHEEL_START", 1089)
        Sounds.Add("SOUND_BANDIT_PAYOUT", 1090)
        Sounds.Add("SOUND_OFFICE_FIRE_ALARM_START", 1091)
        Sounds.Add("SOUND_OFFICE_FIRE_ALARM_STOP", 1092)
        Sounds.Add("SOUND_OFFICE_FIRE_COUGHING_START", 1093)
        Sounds.Add("SOUND_OFFICE_FIRE_COUGHING_STOP", 1094)
        Sounds.Add("SOUND_BIKE_PACKER_CLUNK", 1095)
        Sounds.Add("SOUND_BIKE_GANG_WHEEL_SPIN", 1096)
        Sounds.Add("SOUND_AWARD_TRACK_START", 1097)
        Sounds.Add("SOUND_AWARD_TRACK_STOP", 1098)
        Sounds.Add("SOUND_MOLOTOV", 1099)
        Sounds.Add("SOUND_MESH_GATE_OPEN_START", 1100)
        Sounds.Add("SOUND_MESH_GATE_OPEN_STOP", 1101)
        Sounds.Add("SOUND_OGLOC_DOORBELL", 1102)
        Sounds.Add("SOUND_OGLOC_WINDOW_RATTLE_BANG", 1103)
        Sounds.Add("SOUND_STINGER_RELOAD", 1104)
        Sounds.Add("SOUND_HEAVY_DOOR_START", 1105)
        Sounds.Add("SOUND_HEAVY_DOOR_STOP", 1106)
        Sounds.Add("SOUND_SHOOT_CONTROLS", 1107)
        Sounds.Add("SOUND_CARGO_PLANE_DOOR_START", 1108)
        Sounds.Add("SOUND_CARGO_PLANE_DOOR_STOP", 1109)
        Sounds.Add("SOUND_DA_NANG_CONTAINER_OPEN", 1110)
        Sounds.Add("SOUND_DA_NANG_HEAVY_DOOR_OPEN", 1111)
        Sounds.Add("SOUND_DA_NANG_MUFFLED_REFUGEES", 1112)
        Sounds.Add("SOUND_GYM_BIKE_START", 1113)
        Sounds.Add("SOUND_GYM_BIKE_STOP", 1114)
        Sounds.Add("SOUND_GYM_BOXING_BELL", 1115)
        Sounds.Add("SOUND_GYM_INCREASE_DIFFICULTY", 1116)
        Sounds.Add("SOUND_GYM_REST_WEIGHTS", 1117)
        Sounds.Add("SOUND_GYM_RUNNING_MACHINE_START", 1118)
        Sounds.Add("SOUND_GYM_RUNNING_MACHINE_STOP", 1119)
        Sounds.Add("SOUND_OTB_BET_ZERO", 1120)
        Sounds.Add("SOUND_OTB_INCREASE_BET", 1121)
        Sounds.Add("SOUND_OTB_LOSE", 1122)
        Sounds.Add("SOUND_OTB_PLACE_BET", 1123)
        Sounds.Add("SOUND_OTB_WIN", 1124)
        Sounds.Add("SOUND_STINGER_FIRE", 1125)
        Sounds.Add("SOUND_HEAVY_GATE_START", 1126)
        Sounds.Add("SOUND_HEAVY_GATE_STOP", 1127)
        Sounds.Add("SOUND_VERTICAL_BIRD_LIFT_START", 1128)
        Sounds.Add("SOUND_VERTICAL_BIRD_LIFT_STOP", 1129)
        Sounds.Add("SOUND_PUNCH_PED", 1130)
        Sounds.Add("SOUND_AMMUNATION_GUN_COLLISION", 1131)
        Sounds.Add("SOUND_CAMERA_SHOT", 1132)
        Sounds.Add("SOUND_BUY_CAR_MOD", 1133)
        Sounds.Add("SOUND_BUY_CAR_RESPRAY", 1134)
        Sounds.Add("SOUND_BASEBALL_BAT_HIT_PED", 1135)
        Sounds.Add("SOUND_STAMP_PED", 1136)
        Sounds.Add("SOUND_CHECKPOINT_AMBER", 1137)
        Sounds.Add("SOUND_CHECKPOINT_GREEN", 1138)
        Sounds.Add("SOUND_CHECKPOINT_RED", 1139)
        Sounds.Add("SOUND_CAR_SMASH_CAR", 1140)
        Sounds.Add("SOUND_CAR_SMASH_GATE", 1141)
        Sounds.Add("SOUND_OTB_TRACK_START", 1142)
        Sounds.Add("SOUND_OTB_TRACK_STOP", 1143)
        Sounds.Add("SOUND_PED_HIT_WATER_SPLASH", 1144)
        Sounds.Add("SOUND_RESTAURANT_TRAY_COLLISION", 1145)
        Sounds.Add("SOUND_PICKUP_CRATE", 1146)
        Sounds.Add("SOUND_SWEETS_HORN", 1147)
        Sounds.Add("SOUND_MAGNET_VEHICLE_COLLISION", 1148)
        Sounds.Add("SOUND_PROPERTY_PURCHASED", 1149)
        Sounds.Add("SOUND_PICKUP_STANDARD", 1150)
        Sounds.Add("SOUND_MECHANIC_SLIDE_OUT", 1151)
        Sounds.Add("SOUND_MECHANIC_ATTACH_CAR_BOMB", 1152)
        Sounds.Add("SOUND_GARAGE_DOOR_START", 1153)
        Sounds.Add("SOUND_GARAGE_DOOR_STOP", 1154)
        Sounds.Add("SOUND_CAT2_SECURITY_ALARM", 1155)
        Sounds.Add("SOUND_CAT2_WOODEN_DOOR_BREACH", 1156)
        Sounds.Add("SOUND_MINITANK_FIRE", 1157)
        Sounds.Add("SOUND_OTB_NO_CASH", 1158)
        Sounds.Add("SOUND_EXPLOSION", 1159)
        Sounds.Add("SOUND_ROULETTE_BALL_BOUNCING", 1160)
        Sounds.Add("SOUND_VERTICAL_BIRD_ALARM_START", 1161)
        Sounds.Add("SOUND_VERTICAL_BIRD_ALARM_STOP", 1162)
        Sounds.Add("SOUND_PED_COLLAPSE", 1163)
        Sounds.Add("SOUND_AIR_HORN", 1164)
        Sounds.Add("SOUND_SHUTTER_DOOR_SLOW_START", 1165)
        Sounds.Add("SOUND_SHUTTER_DOOR_SLOW_STOP", 1166)
        Sounds.Add("SOUND_BEE_BUZZ", 1167)
        Sounds.Add("SOUND_RESTAURANT_CJ_EAT", 1168)
        Sounds.Add("SOUND_RESTAURANT_CJ_PUKE", 1169)
        Sounds.Add("SOUND_TEMPEST_PLAYER_SHOOT", 1170)
        Sounds.Add("SOUND_TEMPEST_ENEMY_SHOOT", 1171)
        Sounds.Add("SOUND_TEMPEST_EXPLOSION", 1172)
        Sounds.Add("SOUND_TEMPEST_PICKUP1", 1173)
        Sounds.Add("SOUND_TEMPEST_PICKUP2", 1174)
        Sounds.Add("SOUND_TEMPEST_PICKUP3", 1175)
        Sounds.Add("SOUND_TEMPEST_WARP", 1176)
        Sounds.Add("SOUND_TEMPEST_SHIELD_GLOW", 1177)
        Sounds.Add("SOUND_TEMPEST_GAME_OVER", 1178)
        Sounds.Add("SOUND_TEMPEST_HIGHLIGHT", 1179)
        Sounds.Add("SOUND_TEMPEST_SELECT", 1180)
        Sounds.Add("SOUND_TEMPEST_TRACK_START", 1181)
        Sounds.Add("SOUND_TEMPEST_TRACK_STOP", 1182)
        Sounds.Add("SOUND_DRIVING_AWARD_TRACK_START", 1183)
        Sounds.Add("SOUND_DRIVING_AWARD_TRACK_STOP", 1184)
        Sounds.Add("SOUND_BIKE_AWARD_TRACK_START", 1185)
        Sounds.Add("SOUND_BIKE_AWARD_TRACK_STOP", 1186)
        Sounds.Add("SOUND_PILOT_AWARD_TRACK_START", 1187)
        Sounds.Add("SOUND_PILOT_AWARD_TRACK_STOP", 1188)
        Sounds.Add("SOUND_PED_DEATH_CRUNCH", 1189)
        Sounds.Add("SOUND_BANK_VIDEO_POKER", 1800)
        Sounds.Add("SOUND_BANK_SHOOTING_RANGE", 1801)
        Sounds.Add("SOUND_BANK_POOL", 1802)
        Sounds.Add("SOUND_BANK_PARACHUTE", 1803)
        Sounds.Add("SOUND_BANK_KEYPAD", 1804)
        Sounds.Add("SOUND_BANK_WAREHOUSE_DOOR", 1805)
        Sounds.Add("SOUND_BANK_GOGO", 1806)
        Sounds.Add("SOUND_BANK_DUAL", 1807)
        Sounds.Add("SOUND_BANK_CRANE", 1808)
        Sounds.Add("SOUND_BANK_BLACK_PROJECT", 1809)
        Sounds.Add("SOUND_BANK_BEE", 1810)
        Sounds.Add("SOUND_BANK_BASKETBALL", 1811)
        Sounds.Add("SOUND_BANK_MEAT_BUSINESS", 1812)
        Sounds.Add("SOUND_BANK_ROULETTE", 1813)
        Sounds.Add("SOUND_BANK_BANDIT", 1814)
        Sounds.Add("SOUND_BANK_OFFICE_FIRE", 1815)
        Sounds.Add("SOUND_BANK_MESH_GATE", 1816)
        Sounds.Add("SOUND_BANK_OGLOC", 1817)
        Sounds.Add("SOUND_BANK_CARGO_PLANE", 1818)
        Sounds.Add("SOUND_BANK_DA_NANG", 1819)
        Sounds.Add("SOUND_BANK_GYM", 1820)
        Sounds.Add("SOUND_BANK_OTB", 1821)
        Sounds.Add("SOUND_BANK_STINGER", 1822)
        Sounds.Add("SOUND_BANK_UNCLE_SAM", 1823)
        Sounds.Add("SOUND_BANK_VERTICAL_BIRD", 1824)
        Sounds.Add("SOUND_BANK_MECHANIC", 1825)
        Sounds.Add("SOUND_BANK_CAT2_BANK", 1826)
        Sounds.Add("SOUND_BANK_AIR_HORN", 1827)
        Sounds.Add("SOUND_BANK_RESTAURANT", 1828)
        Sounds.Add("SOUND_BANK_TEMPEST", 1829)
        Sounds.Add("SOUND_ALDEAMALVADA", 2000)
        Sounds.Add("SOUND_ANGELPINE", 2001)
        Sounds.Add("SOUND_ARCODELOESTE", 2002)
        Sounds.Add("SOUND_AVISPACOUNTRYCLUB", 2003)
        Sounds.Add("SOUND_BACKOBEYOND", 2004)
        Sounds.Add("SOUND_BATTERYPOINT", 2005)
        Sounds.Add("SOUND_BAYSIDE", 2006)
        Sounds.Add("SOUND_BAYSIDEMARINA", 2007)
        Sounds.Add("SOUND_BAYSIDETUNNEL", 2008)
        Sounds.Add("SOUND_BEACONHILL", 2009)
        Sounds.Add("SOUND_BLACKFIELD", 2010)
        Sounds.Add("SOUND_BLACKFIELDCHAPEL", 2011)
        Sounds.Add("SOUND_BLACKFIELDINTERSECTION", 2012)
        Sounds.Add("SOUND_BLUBERRYACRES", 2013)
        Sounds.Add("SOUND_BLUEBERRY", 2014)
        Sounds.Add("SOUND_BONECOUNTY", 2015)
        Sounds.Add("SOUND_CALIGULASPALACE", 2016)
        Sounds.Add("SOUND_CALTONHEIGHTS", 2017)
        Sounds.Add("SOUND_CHINATOWN", 2018)
        Sounds.Add("SOUND_CITYHALL", 2019)
        Sounds.Add("SOUND_COME_A_LOT", 2020)
        Sounds.Add("SOUND_COMMERCE", 2021)
        Sounds.Add("SOUND_CONFERENCECENTRE", 2022)
        Sounds.Add("SOUND_CRANBERRYSTATION", 2023)
        Sounds.Add("SOUND_DILLIMORE", 2024)
        Sounds.Add("SOUND_DOHERTY", 2025)
        Sounds.Add("SOUND_DOWNTOWN", 2026)
        Sounds.Add("SOUND_DOWNTOWNLOSSANTOS", 2027)
        Sounds.Add("SOUND_EASRLOSSANTOS", 2028)
        Sounds.Add("SOUND_EASTBEACH", 2029)
        Sounds.Add("SOUND_EASTERBASIN", 2030)
        Sounds.Add("SOUND_EASTERBAYAIRPORT", 2031)
        Sounds.Add("SOUND_EASTERBAYBLUFFSCHEMICALPLANT", 2032)
        Sounds.Add("SOUND_EASTERTUNNEL", 2033)
        Sounds.Add("SOUND_ELCASTILLODELDIABLO", 2034)
        Sounds.Add("SOUND_ELCORONA", 2035)
        Sounds.Add("SOUND_ELQUEBRADOS", 2036)
        Sounds.Add("SOUND_ESPLANADEEAST", 2037)
        Sounds.Add("SOUND_ESPLANADENORTH", 2038)
        Sounds.Add("SOUND_FALLENTREE", 2039)
        Sounds.Add("SOUND_FALLOWBRIDGE", 2040)
        Sounds.Add("SOUND_FERNRIDGE", 2041)
        Sounds.Add("SOUND_FINANCIAL", 2042)
        Sounds.Add("SOUND_FISHERSLAGOON", 2043)
        Sounds.Add("SOUND_FLINTCOUNTY", 2044)
        Sounds.Add("SOUND_FLINTINTERSECTION", 2045)
        Sounds.Add("SOUND_FLINTRANGE", 2046)
        Sounds.Add("SOUND_FLINTWATER", 2047)
        Sounds.Add("SOUND_FORTCARSON", 2048)
        Sounds.Add("SOUND_FOSTERVALLEY", 2049)
        Sounds.Add("SOUND_FREDERICKBRIDGE", 2050)
        Sounds.Add("SOUND_GANTON", 2051)
        Sounds.Add("SOUND_GANTRBRIDGE", 2052)
        Sounds.Add("SOUND_GARCIA", 2053)
        Sounds.Add("SOUND_GARVERBRIDGE", 2054)
        Sounds.Add("SOUND_GLENPARK", 2055)
        Sounds.Add("SOUND_GREENGLASSCOLLEGE", 2056)
        Sounds.Add("SOUND_GREENPALMS", 2057)
        Sounds.Add("SOUND_HAMPTONBARNS", 2058)
        Sounds.Add("SOUND_HANKYPANKYPOINT", 2059)
        Sounds.Add("SOUND_HARRYGOLDPARKWAY", 2060)
        Sounds.Add("SOUND_HASHBERRY", 2061)
        Sounds.Add("SOUND_HILLTOPFARM", 2062)
        Sounds.Add("SOUND_HUNTERQUARRY", 2063)
        Sounds.Add("SOUND_IDLEWOOD", 2064)
        Sounds.Add("SOUND_JULIUSTHRUWAYEAST", 2065)
        Sounds.Add("SOUND_JULIUSTHRUWAYNORTH", 2066)
        Sounds.Add("SOUND_JULIUSTHRUWAYSOUTH", 2067)
        Sounds.Add("SOUND_JULIUSTHRUWAYWEST", 2068)
        Sounds.Add("SOUND_JUNIPERHILL", 2069)
        Sounds.Add("SOUND_JUNIPERHOLLOW", 2070)
        Sounds.Add("SOUND_KACCMILITARYFUELS", 2071)
        Sounds.Add("SOUND_KINCAIDBRIDEG", 2072)
        Sounds.Add("SOUND_KINGS", 2073)
        Sounds.Add("SOUND_LASBARRANCAS", 2074)
        Sounds.Add("SOUND_LASBRUJAS", 2075)
        Sounds.Add("SOUND_LASPAYASADAS", 2076)
        Sounds.Add("SOUND_LASTDIMEMOTEL", 2077)
        Sounds.Add("SOUND_LASVENTURAS", 2078)
        Sounds.Add("SOUND_LEAFYHOLLOW", 2079)
        Sounds.Add("SOUND_LILPROBEINN", 2080)
        Sounds.Add("SOUND_LINDENSIDE", 2081)
        Sounds.Add("SOUND_LINDENSTATION", 2082)
        Sounds.Add("SOUND_LITTLEMEXICO", 2083)
        Sounds.Add("SOUND_LOSCOLINAS", 2084)
        Sounds.Add("SOUND_LOSFLORES", 2085)
        Sounds.Add("SOUND_LOSSANTOS", 2086)
        Sounds.Add("SOUND_LOSSANTOSINLET", 2087)
        Sounds.Add("SOUND_LOSSANTOSINTERNATIONAL", 2088)
        Sounds.Add("SOUND_LOSSEPULCROS", 2089)
        Sounds.Add("SOUND_LOSVENTURASAIRPORT", 2090)
        Sounds.Add("SOUND_LVAFREIGHTDEPOT", 2091)
        Sounds.Add("SOUND_MARINA", 2092)
        Sounds.Add("SOUND_MARKET", 2093)
        Sounds.Add("SOUND_MARKETSTATION", 2094)
        Sounds.Add("SOUND_MARTINBRIDGE", 2095)
        Sounds.Add("SOUND_MISSIONARYHILL", 2096)
        Sounds.Add("SOUND_MONTGOMERY", 2097)
        Sounds.Add("SOUND_MONTGOMERYINTERSECTION", 2098)
        Sounds.Add("SOUND_MOUNTCHILLIAD", 2099)
        Sounds.Add("SOUND_MULHOLLAND", 2100)
        Sounds.Add("SOUND_MULLHOLLANDINTERSECTION", 2101)
        Sounds.Add("SOUND_NORTHSTARROCK", 2102)
        Sounds.Add("SOUND_OCEANDOCKS", 2103)
        Sounds.Add("SOUND_OCEANFLATS", 2104)
        Sounds.Add("SOUND_OCTANESPRINGS", 2105)
        Sounds.Add("SOUND_OLDVENTURASSTRIP", 2106)
        Sounds.Add("SOUND_OPENOCEAN", 2107)
        Sounds.Add("SOUND_PALLISADES", 2108)
        Sounds.Add("SOUND_PALOMINOCREEK", 2109)
        Sounds.Add("SOUND_PARADISO", 2110)
        Sounds.Add("SOUND_PILGRAMSCREEK", 2111)
        Sounds.Add("SOUND_PILSONINTERSECTIION", 2112)
        Sounds.Add("SOUND_PLAYADELSEVILLE", 2113)
        Sounds.Add("SOUND_PRICKLEPINE", 2114)
        Sounds.Add("SOUND_QUEENS", 2115)
        Sounds.Add("SOUND_RANDOLPHINDUSTRIALESTATE", 2116)
        Sounds.Add("SOUND_REDCOUNTY", 2117)
        Sounds.Add("SOUND_REDSANDSEAST", 2118)
        Sounds.Add("SOUND_REDSANDSWEST", 2119)
        Sounds.Add("SOUND_REGULARTOM", 2120)
        Sounds.Add("SOUND_RICHMAN", 2121)
        Sounds.Add("SOUND_ROCAESCALANTE", 2122)
        Sounds.Add("SOUND_ROCKSHOREEAST", 2123)
        Sounds.Add("SOUND_ROCKSHOREWEST", 2124)
        Sounds.Add("SOUND_RODEO", 2125)
        Sounds.Add("SOUND_ROYALECASINO", 2126)
        Sounds.Add("SOUND_SANANDREASSOUND", 2127)
        Sounds.Add("SOUND_SANFIERRO", 2128)
        Sounds.Add("SOUND_SANFIERROBAY", 2129)
        Sounds.Add("SOUND_SANTAFLORA", 2130)
        Sounds.Add("SOUND_SANTAMARIABEACH", 2131)
        Sounds.Add("SOUND_SHADYCREEKS", 2132)
        Sounds.Add("SOUND_SHERMANRESERVOIR", 2133)
        Sounds.Add("SOUND_SOBELLRAILYARDS", 2134)
        Sounds.Add("SOUND_SPINYBED", 2135)
        Sounds.Add("SOUND_STARFISHCASINO", 2136)
        Sounds.Add("SOUND_SUNNYSIDE", 2137)
        Sounds.Add("SOUND_TEMPLE", 2138)
        Sounds.Add("SOUND_THEBIGEARRADIOTELESCOPE", 2139)
        Sounds.Add("SOUND_THECAMELSTOE", 2140)
        Sounds.Add("SOUND_THECLOWNSPOCKET", 2141)
        Sounds.Add("SOUND_THEEMERALDISLE", 2142)
        Sounds.Add("SOUND_THEFARM", 2143)
        Sounds.Add("SOUND_THEFOURDRAGONSCASINO", 2144)
        Sounds.Add("SOUND_THEHIGHROLLER", 2145)
        Sounds.Add("SOUND_THEMAKOSPAN", 2146)
        Sounds.Add("SOUND_THEPANOPTICON", 2147)
        Sounds.Add("SOUND_THEPINKSWAN", 2148)
        Sounds.Add("SOUND_THEPIRATESINMENSPANTS", 2149)
        Sounds.Add("SOUND_THESHERMANDAM", 2150)
        Sounds.Add("SOUND_THEVISAGE", 2151)
        Sounds.Add("SOUND_TIERRA_ROBADA", 2152)
        Sounds.Add("SOUND_UNITYSTATION", 2153)
        Sounds.Add("SOUND_VALLEOCULTADO", 2154)
        Sounds.Add("SOUND_VERDANTBLUFFS", 2155)
        Sounds.Add("SOUND_VERDANTMEDOW", 2156)
        Sounds.Add("SOUND_VERONABEACH", 2157)
        Sounds.Add("SOUND_VINEWOOD", 2158)
        Sounds.Add("SOUND_WHETSTONE", 2159)
        Sounds.Add("SOUND_WHITEWOODESTATES", 2160)
        Sounds.Add("SOUND_WILLOWFIELD", 2161)
        Sounds.Add("SOUND_YELLOWBELLGOLFCOURSE", 2162)
        Sounds.Add("SOUND_YELLOWBELLSTATION", 2163)
        Sounds.Add("SOUND_BLACK", 2200)
        Sounds.Add("SOUND_BLUE", 2201)
        Sounds.Add("SOUND_BROWN", 2202)
        Sounds.Add("SOUND_COPPER", 2203)
        Sounds.Add("SOUND_CUSTOM", 2204)
        Sounds.Add("SOUND_CUSTOMISED", 2205)
        Sounds.Add("SOUND_DARK", 2206)
        Sounds.Add("SOUND_GOLD", 2207)
        Sounds.Add("SOUND_GREEN", 2208)
        Sounds.Add("SOUND_GREY", 2209)
        Sounds.Add("SOUND_LIGHT", 2210)
        Sounds.Add("SOUND_PINK", 2211)
        Sounds.Add("SOUND_RED", 2212)
        Sounds.Add("SOUND_SILVER", 2213)
        Sounds.Add("SOUND_WHITE", 2214)
        Sounds.Add("SOUND_CENTRAL", 2400)
        Sounds.Add("SOUND_EAST", 2401)
        Sounds.Add("SOUND_NORTH", 2402)
        Sounds.Add("SOUND_SOUTH", 2403)
        Sounds.Add("SOUND_WEST", 2404)
        Sounds.Add("SOUND_HEAD_TO_A_10", 2600)
        Sounds.Add("SOUND_IN_A_", 2601)
        Sounds.Add("SOUND_IN_WATER", 2602)
        Sounds.Add("SOUND_ON_A", 2603)
        Sounds.Add("SOUND_ON_FOOT", 2604)
        Sounds.Add("SOUND_RESPOND_TO_A_10", 2605)
        Sounds.Add("SOUND_SUSPECTINWATER", 2606)
        Sounds.Add("SOUND_SUSPECT_LAST_SEEN", 2607)
        Sounds.Add("SOUND_WEVE_GOT_A_10_", 2608)
        Sounds.Add("SOUND_17", 2800)
        Sounds.Add("SOUND_21", 2801)
        Sounds.Add("SOUND_24", 2802)
        Sounds.Add("SOUND_28", 2803)
        Sounds.Add("SOUND_34", 2804)
        Sounds.Add("SOUND_37", 2805)
        Sounds.Add("SOUND_7", 2806)
        Sounds.Add("SOUND_71", 2807)
        Sounds.Add("SOUND_81", 2808)
        Sounds.Add("SOUND_90", 2809)
        Sounds.Add("SOUND_91", 2810)
        Sounds.Add("SOUND_A10", 2811)
        Sounds.Add("SOUND_A10_2", 2812)
        Sounds.Add("SOUND_A10_3", 2813)
        Sounds.Add("SOUND_2DOOR", 3000)
        Sounds.Add("SOUND_4DOOR", 3001)
        Sounds.Add("SOUND_AMBULANCE", 3002)
        Sounds.Add("SOUND_ARTIC_CAB", 3003)
        Sounds.Add("SOUND_BEACH_BUGGY", 3004)
        Sounds.Add("SOUND_BIKE", 3005)
        Sounds.Add("SOUND_BOAT", 3006)
        Sounds.Add("SOUND_BUGGY", 3007)
        Sounds.Add("SOUND_BULL_DOZER", 3008)
        Sounds.Add("SOUND_BUS", 3009)
        Sounds.Add("SOUND_CAMPER_VAN", 3010)
        Sounds.Add("SOUND_COACH", 3011)
        Sounds.Add("SOUND_COMBINE_HARVESTER", 3012)
        Sounds.Add("SOUND_COMPACT", 3013)
        Sounds.Add("SOUND_CONVERTABLE", 3014)
        Sounds.Add("SOUND_COUPE", 3015)
        Sounds.Add("SOUND_CRUISER", 3016)
        Sounds.Add("SOUND_FIRE_TRUCK", 3017)
        Sounds.Add("SOUND_FORKLIFT", 3018)
        Sounds.Add("SOUND_FREIGHT_TRAIN", 3019)
        Sounds.Add("SOUND_GARBAGE_TRUCK", 3020)
        Sounds.Add("SOUND_GAS_TANKER", 3021)
        Sounds.Add("SOUND_GOLF_CART", 3022)
        Sounds.Add("SOUND_GO_KART", 3023)
        Sounds.Add("SOUND_HEARSE", 3024)
        Sounds.Add("SOUND_HELICOPTER", 3025)
        Sounds.Add("SOUND_HOVERCRAFT", 3026)
        Sounds.Add("SOUND_ICE_CREAM_VAN", 3027)
        Sounds.Add("SOUND_JEEP", 3028)
        Sounds.Add("SOUND_LAWNMOWER", 3029)
        Sounds.Add("SOUND_LIMO", 3030)
        Sounds.Add("SOUND_LOW_RIDER", 3031)
        Sounds.Add("SOUND_MOPED", 3032)
        Sounds.Add("SOUND_MOTORBIKE", 3033)
        Sounds.Add("SOUND_OFF_ROAD", 3034)
        Sounds.Add("SOUND_PEOPLE_CARRIER", 3035)
        Sounds.Add("SOUND_PICK_UP", 3036)
        Sounds.Add("SOUND_PLANE", 3037)
        Sounds.Add("SOUND_POLICE_CAR", 3038)
        Sounds.Add("SOUND_POLICE_VAN", 3039)
        Sounds.Add("SOUND_QUAD_BIKE", 3040)
        Sounds.Add("SOUND_RUBBER_DINGY", 3041)
        Sounds.Add("SOUND_SAND_BUGGY", 3042)
        Sounds.Add("SOUND_SEA_PLANE", 3043)
        Sounds.Add("SOUND_SNOWCAT", 3044)
        Sounds.Add("SOUND_SPEED_BOAT", 3045)
        Sounds.Add("SOUND_SPORT", 3046)
        Sounds.Add("SOUND_SPORTSCAR", 3047)
        Sounds.Add("SOUND_SPORTS_BIKE", 3048)
        Sounds.Add("SOUND_STATION_WAGON", 3049)
        Sounds.Add("SOUND_SUV", 3050)
        Sounds.Add("SOUND_TANK", 3051)
        Sounds.Add("SOUND_TAXI", 3052)
        Sounds.Add("SOUND_TRACTOR", 3053)
        Sounds.Add("SOUND_TRAIN", 3054)
        Sounds.Add("SOUND_TRAM", 3055)
        Sounds.Add("SOUND_TRUCK", 3056)
        Sounds.Add("SOUND_VAN", 3057)
        Sounds.Add("SOUND__AIR_HORN_L", 3200)
        Sounds.Add("SOUND__AIR_HORN_R", 3201)
        Sounds.Add("SOUND_AIRCONDITIONER", 3400)
        Sounds.Add("SOUND_OFFICE_FIRE_ALARM", 3401)
        Sounds.Add("SOUND_MOBILE_DIALING", 3600)
        Sounds.Add("SOUND_VIDEOTAPE_NOISE", 3800)
        Sounds.Add("SOUND_BALLA_1", 4000)
        Sounds.Add("SOUND_BALLA_2", 4001)
        Sounds.Add("SOUND__BANDIT_WHEEL_START", 4200)
        Sounds.Add("SOUND__BANDIT_PAYOUT", 4201)
        Sounds.Add("SOUND__BANDIT_WHEEL_STOP", 4202)
        Sounds.Add("SOUND__INSERT_COIN", 4203)
        Sounds.Add("SOUND_HAIRCUT", 4400)
        Sounds.Add("SOUND__BASKETBALL_BOUNCE_1", 4600)
        Sounds.Add("SOUND__BASKETBALL_BOUNCE_2", 4601)
        Sounds.Add("SOUND__BASKETBALL_BOUNCE_3", 4602)
        Sounds.Add("SOUND__BASKETBALL_HIT_HOOP", 4603)
        Sounds.Add("SOUND__BASKETBALL_SCORE", 4604)
        Sounds.Add("SOUND_BBOX_1", 4800)
        Sounds.Add("SOUND_BBOX_2", 4801)
        Sounds.Add("SOUND_BBOX_3", 4802)
        Sounds.Add("SOUND_BBOX_4", 4803)
        Sounds.Add("SOUND_BBOX_5", 4804)
        Sounds.Add("SOUND_BBOX_6", 4805)
        Sounds.Add("SOUND_BBOX_7", 4806)
        Sounds.Add("SOUND_BBOX_8", 4807)
        Sounds.Add("SOUND_BCS5_AA", 5000)
        Sounds.Add("SOUND_BCS5_AB", 5001)
        Sounds.Add("SOUND_BCS5_AC", 5002)
        Sounds.Add("SOUND_BCS5_AD", 5003)
        Sounds.Add("SOUND_BCS5_AE", 5004)
        Sounds.Add("SOUND_BCS5_AF", 5005)
        Sounds.Add("SOUND_BCS5_AG", 5006)
        Sounds.Add("SOUND_BCS5_AH", 5007)
        Sounds.Add("SOUND_BCS5_BA", 5008)
        Sounds.Add("SOUND_BCS5_BB", 5009)
        Sounds.Add("SOUND_BCS5_BC", 5010)
        Sounds.Add("SOUND_BCS5_BD", 5011)
        Sounds.Add("SOUND_BCS5_BE", 5012)
        Sounds.Add("SOUND_BCS5_BF", 5013)
        Sounds.Add("SOUND_BCS5_BG", 5014)
        Sounds.Add("SOUND_BB_GONE_BUZZ", 5200)
        Sounds.Add("SOUND_BB_GONE_ACCEPT", 5201)
        Sounds.Add("SOUND_BB_GONE_DROP", 5202)
        Sounds.Add("SOUND_BB_GONE_GAMEOVER", 5203)
        Sounds.Add("SOUND_BB_GONE_PICKUP", 5204)
        Sounds.Add("SOUND_BB_GONE_SELECT", 5205)
        Sounds.Add("SOUND_BB_GONE_ZAP", 5206)
        Sounds.Add("SOUND_B_BET_1", 5400)
        Sounds.Add("SOUND_B_BET_2", 5401)
        Sounds.Add("SOUND_B_LEND1", 5402)
        Sounds.Add("SOUND_B_LEND2", 5403)
        Sounds.Add("SOUND_B_LEND3", 5404)
        Sounds.Add("SOUND_B_NEM_1", 5405)
        Sounds.Add("SOUND_B_NEM_2", 5406)
        Sounds.Add("SOUND_B_NEM_3", 5407)
        Sounds.Add("SOUND_B_NMB_1", 5408)
        Sounds.Add("SOUND_B_NMB_2", 5409)
        Sounds.Add("SOUND_B_NMB_3", 5410)
        Sounds.Add("SOUND_B_NUM10", 5411)
        Sounds.Add("SOUND_B_NUM11", 5412)
        Sounds.Add("SOUND_B_NUM12", 5413)
        Sounds.Add("SOUND_B_NUM13", 5414)
        Sounds.Add("SOUND_B_NUM14", 5415)
        Sounds.Add("SOUND_B_NUM15", 5416)
        Sounds.Add("SOUND_B_NUM16", 5417)
        Sounds.Add("SOUND_B_NUM17", 5418)
        Sounds.Add("SOUND_B_NUM18", 5419)
        Sounds.Add("SOUND_B_NUM19", 5420)
        Sounds.Add("SOUND_B_NUM20", 5421)
        Sounds.Add("SOUND_B_NUM21", 5422)
        Sounds.Add("SOUND_B_NUM22", 5423)
        Sounds.Add("SOUND_B_NUM23", 5424)
        Sounds.Add("SOUND_B_NUM24", 5425)
        Sounds.Add("SOUND_B_NUM25", 5426)
        Sounds.Add("SOUND_B_NUM26", 5427)
        Sounds.Add("SOUND_B_NUM27", 5428)
        Sounds.Add("SOUND_B_NUM28", 5429)
        Sounds.Add("SOUND_B_NUM29", 5430)
        Sounds.Add("SOUND_B_NUM30", 5431)
        Sounds.Add("SOUND_B_NUM31", 5432)
        Sounds.Add("SOUND_B_NUM32", 5433)
        Sounds.Add("SOUND_B_NUM33", 5434)
        Sounds.Add("SOUND_B_NUM34", 5435)
        Sounds.Add("SOUND_B_NUM35", 5436)
        Sounds.Add("SOUND_B_NUM36", 5437)
        Sounds.Add("SOUND_B_NUM_0", 5438)
        Sounds.Add("SOUND_B_NUM_1", 5439)
        Sounds.Add("SOUND_B_NUM_2", 5440)
        Sounds.Add("SOUND_B_NUM_3", 5441)
        Sounds.Add("SOUND_B_NUM_4", 5442)
        Sounds.Add("SOUND_B_NUM_5", 5443)
        Sounds.Add("SOUND_B_NUM_6", 5444)
        Sounds.Add("SOUND_B_NUM_7", 5445)
        Sounds.Add("SOUND_B_NUM_8", 5446)
        Sounds.Add("SOUND_B_NUM_9", 5447)
        Sounds.Add("SOUND_B_PWIN1", 5448)
        Sounds.Add("SOUND_B_PWIN2", 5449)
        Sounds.Add("SOUND_B_PWIN3", 5450)
        Sounds.Add("SOUND_B_REG_1", 5451)
        Sounds.Add("SOUND_B_REG_2", 5452)
        Sounds.Add("SOUND_B_THX_1", 5453)
        Sounds.Add("SOUND_B_THX_2", 5454)
        Sounds.Add("SOUND_B_WEEL1", 5455)
        Sounds.Add("SOUND_B_WEEL2", 5456)
        Sounds.Add("SOUND_B_WEEL3", 5457)
        Sounds.Add("SOUND_B_WEEL4", 5458)
        Sounds.Add("SOUND_B_WEEL5", 5459)
        Sounds.Add("SOUND_B_WEEL6", 5460)
        Sounds.Add("SOUND_B_WEEL7", 5461)
        Sounds.Add("SOUND_B_WINS1", 5462)
        Sounds.Add("SOUND_B_WINS2", 5463)
        Sounds.Add("SOUND_B_WINS3", 5464)
        Sounds.Add("SOUND_BIKE_PACKAGE_THROW", 5600)
        Sounds.Add("SOUND_BIKE_PACKAGE_THROW2", 5601)
        Sounds.Add("SOUND_BIKE_PACKAGE_THROW3", 5602)
        Sounds.Add("SOUND_10_OR_20", 5800)
        Sounds.Add("SOUND_2_OR_12", 5801)
        Sounds.Add("SOUND_3_OR_13", 5802)
        Sounds.Add("SOUND_4_OR_14", 5803)
        Sounds.Add("SOUND_5_OR_15", 5804)
        Sounds.Add("SOUND_6_OR_16", 5805)
        Sounds.Add("SOUND_7_OR_17", 5806)
        Sounds.Add("SOUND_8_OR_18", 5807)
        Sounds.Add("SOUND_9_OR_19", 5808)
        Sounds.Add("SOUND_J_BET_1", 5809)
        Sounds.Add("SOUND_J_BET_2", 5810)
        Sounds.Add("SOUND_J_BJ_1", 5811)
        Sounds.Add("SOUND_J_BJ_2", 5812)
        Sounds.Add("SOUND_J_BUST1", 5813)
        Sounds.Add("SOUND_J_BUST2", 5814)
        Sounds.Add("SOUND_J_DRW_1", 5815)
        Sounds.Add("SOUND_J_DRW_2", 5816)
        Sounds.Add("SOUND_J_DW_1", 5817)
        Sounds.Add("SOUND_J_DW_2", 5818)
        Sounds.Add("SOUND_J_DW_3", 5819)
        Sounds.Add("SOUND_J_LEND1", 5820)
        Sounds.Add("SOUND_J_LEND2", 5821)
        Sounds.Add("SOUND_J_LEND3", 5822)
        Sounds.Add("SOUND_J_NEM_1", 5823)
        Sounds.Add("SOUND_J_NEM_2", 5824)
        Sounds.Add("SOUND_J_NEM_3", 5825)
        Sounds.Add("SOUND_J_NMB_1", 5826)
        Sounds.Add("SOUND_J_NMB_2", 5827)
        Sounds.Add("SOUND_J_NMB_3", 5828)
        Sounds.Add("SOUND_J_NUM10", 5829)
        Sounds.Add("SOUND_J_NUM11", 5830)
        Sounds.Add("SOUND_J_NUM12", 5831)
        Sounds.Add("SOUND_J_NUM13", 5832)
        Sounds.Add("SOUND_J_NUM14", 5833)
        Sounds.Add("SOUND_J_NUM15", 5834)
        Sounds.Add("SOUND_J_NUM16", 5835)
        Sounds.Add("SOUND_J_NUM17", 5836)
        Sounds.Add("SOUND_J_NUM18", 5837)
        Sounds.Add("SOUND_J_NUM19", 5838)
        Sounds.Add("SOUND_J_NUM20", 5839)
        Sounds.Add("SOUND_J_NUM21", 5840)
        Sounds.Add("SOUND_J_NUM_4", 5841)
        Sounds.Add("SOUND_J_NUM_5", 5842)
        Sounds.Add("SOUND_J_NUM_6", 5843)
        Sounds.Add("SOUND_J_NUM_7", 5844)
        Sounds.Add("SOUND_J_NUM_8", 5845)
        Sounds.Add("SOUND_J_NUM_9", 5846)
        Sounds.Add("SOUND_J_PWIN1", 5847)
        Sounds.Add("SOUND_J_PWIN2", 5848)
        Sounds.Add("SOUND_J_PWIN3", 5849)
        Sounds.Add("SOUND_J_REG_1", 5850)
        Sounds.Add("SOUND_J_REG_2", 5851)
        Sounds.Add("SOUND_J_THX_1", 5852)
        Sounds.Add("SOUND_J_THX_2", 5853)
        Sounds.Add("SOUND_J_WINS1", 5854)
        Sounds.Add("SOUND_J_WINS2", 5855)
        Sounds.Add("SOUND_J_WINS3", 5856)
        Sounds.Add("SOUND__BLAST_DOOR_SLIDE_LOOP_2", 6000)
        Sounds.Add("SOUND__CLAXON_LOOP", 6001)
        Sounds.Add("SOUND__HEAVY_DOOR", 6002)
        Sounds.Add("SOUND__SHOOT_CONTROLS", 6003)
        Sounds.Add("SOUND_GULL_SCREECH", 6200)
        Sounds.Add("SOUND_OMO_1", 6201)
        Sounds.Add("SOUND_OMO_2", 6202)
        Sounds.Add("SOUND_OMO_3", 6203)
        Sounds.Add("SOUND_OMO_4", 6204)
        Sounds.Add("SOUND_OMO_5", 6205)
        Sounds.Add("SOUND_DOOR_BUZZER", 6400)
        Sounds.Add("SOUND_LIFT_PING", 6401)
        Sounds.Add("SOUND_LIGHTS_POWER_DOWN", 6402)
        Sounds.Add("SOUND_ROOF_COLLAPSE", 6600)
        Sounds.Add("SOUND_WALL_COLLAPSE_BELOW", 6601)
        Sounds.Add("SOUND_WALL_COLLAPSE_NEARBY", 6602)
        Sounds.Add("SOUND_WOODEN_DOOR_BREACH", 6603)
        Sounds.Add("SOUND__CARGO_PLANE_DOOR_LOOP", 6800)
        Sounds.Add("SOUND__CARGO_PLANE_DOOR_START", 6801)
        Sounds.Add("SOUND__CARGO_PLANE_DOOR_STOP", 6802)
        Sounds.Add("SOUND_CAS1B00", 7000)
        Sounds.Add("SOUND_CAS1B01", 7001)
        Sounds.Add("SOUND_CAS1B02", 7002)
        Sounds.Add("SOUND_CAS1B03", 7003)
        Sounds.Add("SOUND_CAS1B04", 7004)
        Sounds.Add("SOUND_CAS1B05", 7005)
        Sounds.Add("SOUND_CAS1B06", 7006)
        Sounds.Add("SOUND_CAS1_AA", 7007)
        Sounds.Add("SOUND_CAS1_AB", 7008)
        Sounds.Add("SOUND_CAS1_AC", 7009)
        Sounds.Add("SOUND_CAS1_AD", 7010)
        Sounds.Add("SOUND_CAS1_AE", 7011)
        Sounds.Add("SOUND_CAS1_BA", 7012)
        Sounds.Add("SOUND_CAS1_BB", 7013)
        Sounds.Add("SOUND_CAS1_BC", 7014)
        Sounds.Add("SOUND_CAS1_BD", 7015)
        Sounds.Add("SOUND_CAS1_BE", 7016)
        Sounds.Add("SOUND_CAS1_CA", 7017)
        Sounds.Add("SOUND_CAS1_CB", 7018)
        Sounds.Add("SOUND_CAS1_CC", 7019)
        Sounds.Add("SOUND_CAS1_CD", 7020)
        Sounds.Add("SOUND_CAS1_CE", 7021)
        Sounds.Add("SOUND_CAS1_DA", 7022)
        Sounds.Add("SOUND_CAS1_DB", 7023)
        Sounds.Add("SOUND_CAS1_DC", 7024)
        Sounds.Add("SOUND_CAS1_DD", 7025)
        Sounds.Add("SOUND_CAS1_DE", 7026)
        Sounds.Add("SOUND_CAS1_EA", 7027)
        Sounds.Add("SOUND_CAS1_EB", 7028)
        Sounds.Add("SOUND_CAS1_EC", 7029)
        Sounds.Add("SOUND_CAS1_ED", 7030)
        Sounds.Add("SOUND_CAS1_EE", 7031)
        Sounds.Add("SOUND_CAS1_EF", 7032)
        Sounds.Add("SOUND_CAS1_EG", 7033)
        Sounds.Add("SOUND_CAS1_EH", 7034)
        Sounds.Add("SOUND_CAS1_EI", 7035)
        Sounds.Add("SOUND_CAS1_EJ", 7036)
        Sounds.Add("SOUND_CAS1_EK", 7037)
        Sounds.Add("SOUND_CAS1_EL", 7038)
        Sounds.Add("SOUND_CAS1_EM", 7039)
        Sounds.Add("SOUND_CAS1_EN", 7040)
        Sounds.Add("SOUND_CAS1_EO", 7041)
        Sounds.Add("SOUND_CAS1_EP", 7042)
        Sounds.Add("SOUND_CAS1_EQ", 7043)
        Sounds.Add("SOUND_CAS1_ES", 7044)
        Sounds.Add("SOUND_CAS1_ET", 7045)
        Sounds.Add("SOUND_CAS1_EU", 7046)
        Sounds.Add("SOUND_CAS1_EV", 7047)
        Sounds.Add("SOUND_CAS1_EW", 7048)
        Sounds.Add("SOUND_CAS1_EX", 7049)
        Sounds.Add("SOUND_CAS1_EY", 7050)
        Sounds.Add("SOUND_CAS1_EZ", 7051)
        Sounds.Add("SOUND_CAS1_FA", 7052)
        Sounds.Add("SOUND_CAS1_FB", 7053)
        Sounds.Add("SOUND_CAS1_FC", 7054)
        Sounds.Add("SOUND_CAS1_GA", 7055)
        Sounds.Add("SOUND_CAS1_GB", 7056)
        Sounds.Add("SOUND_CAS1_GC", 7057)
        Sounds.Add("SOUND_CAS1_GD", 7058)
        Sounds.Add("SOUND_CAS1_GE", 7059)
        Sounds.Add("SOUND_CAS1_HA", 7060)
        Sounds.Add("SOUND_CAS1_HB", 7061)
        Sounds.Add("SOUND_CAS1_HC", 7062)
        Sounds.Add("SOUND_CAS1_HD", 7063)
        Sounds.Add("SOUND_CAS1_HE", 7064)
        Sounds.Add("SOUND_CAS1_JA", 7065)
        Sounds.Add("SOUND_CAS1_JB", 7066)
        Sounds.Add("SOUND_CAS11AA", 7200)
        Sounds.Add("SOUND_CAS11AB", 7201)
        Sounds.Add("SOUND_CAS11AC", 7202)
        Sounds.Add("SOUND_CAS11AD", 7203)
        Sounds.Add("SOUND_CAS11BA", 7204)
        Sounds.Add("SOUND_CAS11BB", 7205)
        Sounds.Add("SOUND_CAS11BC", 7206)
        Sounds.Add("SOUND_CAS11CA", 7207)
        Sounds.Add("SOUND_CAS11DA", 7208)
        Sounds.Add("SOUND_CAS11DB", 7209)
        Sounds.Add("SOUND_CAS11EA", 7210)
        Sounds.Add("SOUND_CAS11EB", 7211)
        Sounds.Add("SOUND_CAS11EC", 7212)
        Sounds.Add("SOUND_CAS11ED", 7213)
        Sounds.Add("SOUND_CAS11EE", 7214)
        Sounds.Add("SOUND_CAS11EF", 7215)
        Sounds.Add("SOUND_CAS11EG", 7216)
        Sounds.Add("SOUND_CAS11FA", 7217)
        Sounds.Add("SOUND_CAS11GA", 7218)
        Sounds.Add("SOUND_CAS11GB", 7219)
        Sounds.Add("SOUND_CAS11GC", 7220)
        Sounds.Add("SOUND_CAS11HA", 7221)
        Sounds.Add("SOUND_CAS11HB", 7222)
        Sounds.Add("SOUND_CAS11HC", 7223)
        Sounds.Add("SOUND_CAS11HD", 7224)
        Sounds.Add("SOUND_CAS11HE", 7225)
        Sounds.Add("SOUND_CAS11HF", 7226)
        Sounds.Add("SOUND_CAS11_AA", 7227)
        Sounds.Add("SOUND_CAS11_AB", 7228)
        Sounds.Add("SOUND_CAS11_BA", 7229)
        Sounds.Add("SOUND_CAS11_DB", 7230)
        Sounds.Add("SOUND_CAS11_EA", 7231)
        Sounds.Add("SOUND_CAS11_EB", 7232)
        Sounds.Add("SOUND_CAS11_EC", 7233)
        Sounds.Add("SOUND_CAS11_ED", 7234)
        Sounds.Add("SOUND_CAS11_EE", 7235)
        Sounds.Add("SOUND_CAS11_EF", 7236)
        Sounds.Add("SOUND_CAS11_EG", 7237)
        Sounds.Add("SOUND_CAS11_GA", 7238)
        Sounds.Add("SOUND_CAS11_GB", 7239)
        Sounds.Add("SOUND_CAS2_AA", 7400)
        Sounds.Add("SOUND_CAS2_AB", 7401)
        Sounds.Add("SOUND_CAS2_AC", 7402)
        Sounds.Add("SOUND_CAS2_AD", 7403)
        Sounds.Add("SOUND_CAS2_AE", 7404)
        Sounds.Add("SOUND_CAS2_AF", 7405)
        Sounds.Add("SOUND_CAS2_AG", 7406)
        Sounds.Add("SOUND_CAS2_AH", 7407)
        Sounds.Add("SOUND_CAS2_AI", 7408)
        Sounds.Add("SOUND_CAS2_AJ", 7409)
        Sounds.Add("SOUND_CAS2_AK", 7410)
        Sounds.Add("SOUND_CAS2_AL", 7411)
        Sounds.Add("SOUND_CAS2_AM", 7412)
        Sounds.Add("SOUND_CAS2_AN", 7413)
        Sounds.Add("SOUND_CAS2_AO", 7414)
        Sounds.Add("SOUND_CAS2_BA", 7415)
        Sounds.Add("SOUND_CAS2_BB", 7416)
        Sounds.Add("SOUND_CAS2_BC", 7417)
        Sounds.Add("SOUND_CAS2_BD", 7418)
        Sounds.Add("SOUND_CAS2_BE", 7419)
        Sounds.Add("SOUND_CAS2_CA", 7420)
        Sounds.Add("SOUND_CAS2_CB", 7421)
        Sounds.Add("SOUND_CAS3_AA", 7600)
        Sounds.Add("SOUND_CAS3_AB", 7601)
        Sounds.Add("SOUND_CAS3_AC", 7602)
        Sounds.Add("SOUND_CAS3_AD", 7603)
        Sounds.Add("SOUND_CAS3_BA", 7604)
        Sounds.Add("SOUND_CAS3_BB", 7605)
        Sounds.Add("SOUND_CAS3_BC", 7606)
        Sounds.Add("SOUND_CAS3_BD", 7607)
        Sounds.Add("SOUND_CAS3_BE", 7608)
        Sounds.Add("SOUND_CAS3_CA", 7609)
        Sounds.Add("SOUND_CAS3_CB", 7610)
        Sounds.Add("SOUND_CAS3_CC", 7611)
        Sounds.Add("SOUND_CAS3_DA", 7612)
        Sounds.Add("SOUND_CAS4_BA", 7800)
        Sounds.Add("SOUND_CAS4_BB", 7801)
        Sounds.Add("SOUND_CAS4_CA", 7802)
        Sounds.Add("SOUND_CAS4_CB", 7803)
        Sounds.Add("SOUND_CAS4_CC", 7804)
        Sounds.Add("SOUND_CAS4_CD", 7805)
        Sounds.Add("SOUND_CAS4_CE", 7806)
        Sounds.Add("SOUND_CAS4_CF", 7807)
        Sounds.Add("SOUND_CAS4_CG", 7808)
        Sounds.Add("SOUND_CAS4_CH", 7809)
        Sounds.Add("SOUND_CAS4_CJ", 7810)
        Sounds.Add("SOUND_CAS4_CK", 7811)
        Sounds.Add("SOUND_CAS4_CL", 7812)
        Sounds.Add("SOUND_CAS4_DA", 7813)
        Sounds.Add("SOUND_CAS4_DB", 7814)
        Sounds.Add("SOUND_CAS4_DC", 7815)
        Sounds.Add("SOUND_CAS4_DD", 7816)
        Sounds.Add("SOUND_CAS4_DE", 7817)
        Sounds.Add("SOUND_CAS4_EA", 7818)
        Sounds.Add("SOUND_CAS4_EB", 7819)
        Sounds.Add("SOUND_CAS4_EC", 7820)
        Sounds.Add("SOUND_CAS4_ED", 7821)
        Sounds.Add("SOUND_CAS4_EE", 7822)
        Sounds.Add("SOUND_CAS4_EF", 7823)
        Sounds.Add("SOUND_CAS4_EG", 7824)
        Sounds.Add("SOUND_CAS4_EH", 7825)
        Sounds.Add("SOUND_CAS4_FA", 7826)
        Sounds.Add("SOUND_CAS4_FB", 7827)
        Sounds.Add("SOUND_CAS4_FC", 7828)
        Sounds.Add("SOUND_CAS4_FD", 7829)
        Sounds.Add("SOUND_CAS4_FE", 7830)
        Sounds.Add("SOUND_CAS4_FF", 7831)
        Sounds.Add("SOUND_CAS4_FG", 7832)
        Sounds.Add("SOUND_CAS4_FH", 7833)
        Sounds.Add("SOUND_CAS4_FJ", 7834)
        Sounds.Add("SOUND_CAS4_FK", 7835)
        Sounds.Add("SOUND_CAS4_FL", 7836)
        Sounds.Add("SOUND_CAS4_GA", 7837)
        Sounds.Add("SOUND_CAS4_GB", 7838)
        Sounds.Add("SOUND_CAS4_HA", 7839)
        Sounds.Add("SOUND_CAS4_HB", 7840)
        Sounds.Add("SOUND_CAS4_HC", 7841)
        Sounds.Add("SOUND_CAS4_HD", 7842)
        Sounds.Add("SOUND_CAS4_JA", 7843)
        Sounds.Add("SOUND_CAS4_JB", 7844)
        Sounds.Add("SOUND_CAS4_JC", 7845)
        Sounds.Add("SOUND_CAS4_JD", 7846)
        Sounds.Add("SOUND_CAS4_KA", 7847)
        Sounds.Add("SOUND_CAS4_KB", 7848)
        Sounds.Add("SOUND_CAS4_KC", 7849)
        Sounds.Add("SOUND_CAS4_KD", 7850)
        Sounds.Add("SOUND_CAS4_KE", 7851)
        Sounds.Add("SOUND_CAS4_KF", 7852)
        Sounds.Add("SOUND_CAS4_KG", 7853)
        Sounds.Add("SOUND_CAS4_KH", 7854)
        Sounds.Add("SOUND_CAS4_KJ", 7855)
        Sounds.Add("SOUND_CAS4_LA", 7856)
        Sounds.Add("SOUND_CAS4_LB", 7857)
        Sounds.Add("SOUND_CAS4_LC", 7858)
        Sounds.Add("SOUND_CAS4_LD", 7859)
        Sounds.Add("SOUND_CAS4_LE", 7860)
        Sounds.Add("SOUND_CAS4_MA", 7861)
        Sounds.Add("SOUND_CAS4_NA", 7862)
        Sounds.Add("SOUND_CAS4_NB", 7863)
        Sounds.Add("SOUND_CAS4_NC", 7864)
        Sounds.Add("SOUND_CAS4_ND", 7865)
        Sounds.Add("SOUND_CAS4_NE", 7866)
        Sounds.Add("SOUND_CAS4_NF", 7867)
        Sounds.Add("SOUND_CAS4_NG", 7868)
        Sounds.Add("SOUND_CAS4_NH", 7869)
        Sounds.Add("SOUND_CAS4_NJ", 7870)
        Sounds.Add("SOUND_CAS4_OA", 7871)
        Sounds.Add("SOUND_CAS4_OB", 7872)
        Sounds.Add("SOUND_CAS4_OC", 7873)
        Sounds.Add("SOUND_CAS4_OD", 7874)
        Sounds.Add("SOUND_CAS4_OE", 7875)
        Sounds.Add("SOUND_CAS4_OF", 7876)
        Sounds.Add("SOUND_CAS4_OG", 7877)
        Sounds.Add("SOUND_CAS4_OH", 7878)
        Sounds.Add("SOUND_CAS4_OJ", 7879)
        Sounds.Add("SOUND_CAS4_OK", 7880)
        Sounds.Add("SOUND_CAS4_OL", 7881)
        Sounds.Add("SOUND_CAS4_OM", 7882)
        Sounds.Add("SOUND_CAS4_PA", 7883)
        Sounds.Add("SOUND_CAS4_PB", 7884)
        Sounds.Add("SOUND_CAS4_PD", 7885)
        Sounds.Add("SOUND_CAS4_PE", 7886)
        Sounds.Add("SOUND_CAS4_PF", 7887)
        Sounds.Add("SOUND_CAS4_PG", 7888)
        Sounds.Add("SOUND_CAS4_PH", 7889)
        Sounds.Add("SOUND_CAS4_PJ", 7890)
        Sounds.Add("SOUND_CAS4_PK", 7891)
        Sounds.Add("SOUND_CAS4_QA", 7892)
        Sounds.Add("SOUND_CAS4_QB", 7893)
        Sounds.Add("SOUND_CAS4_RA", 7894)
        Sounds.Add("SOUND_CAS4_RB", 7895)
        Sounds.Add("SOUND_CAS4_RD", 7896)
        Sounds.Add("SOUND_CAS4_RE", 7897)
        Sounds.Add("SOUND_CAS4_SA", 7898)
        Sounds.Add("SOUND_CAS4_SB", 7899)
        Sounds.Add("SOUND_CAS4_TA", 7900)
        Sounds.Add("SOUND_CAS4_TB", 7901)
        Sounds.Add("SOUND_CAS4_TC", 7902)
        Sounds.Add("SOUND_CAS5_AA", 8000)
        Sounds.Add("SOUND_CAS5_AB", 8001)
        Sounds.Add("SOUND_CAS5_BA", 8002)
        Sounds.Add("SOUND_CAS5_BB", 8003)
        Sounds.Add("SOUND_CAS5_BC", 8004)
        Sounds.Add("SOUND_CAS5_BD", 8005)
        Sounds.Add("SOUND_CAS5_BE", 8006)
        Sounds.Add("SOUND_CAS5_CA", 8007)
        Sounds.Add("SOUND_CAS5_CB", 8008)
        Sounds.Add("SOUND_CAS5_CC", 8009)
        Sounds.Add("SOUND_CAS5_CD", 8010)
        Sounds.Add("SOUND_CAS5_CE", 8011)
        Sounds.Add("SOUND_CAS5_CF", 8012)
        Sounds.Add("SOUND_CAS5_CG", 8013)
        Sounds.Add("SOUND_CAS5_DA", 8014)
        Sounds.Add("SOUND_CAS5_DB", 8015)
        Sounds.Add("SOUND_CAS5_EA", 8016)
        Sounds.Add("SOUND_CAS5_EB", 8017)
        Sounds.Add("SOUND_CAS6_AA", 8200)
        Sounds.Add("SOUND_CAS6_AB", 8201)
        Sounds.Add("SOUND_CAS6_AC", 8202)
        Sounds.Add("SOUND_CAS6_AD", 8203)
        Sounds.Add("SOUND_CAS6_AE", 8204)
        Sounds.Add("SOUND_CAS6_AF", 8205)
        Sounds.Add("SOUND_CAS6_BA", 8206)
        Sounds.Add("SOUND_CAS6_BB", 8207)
        Sounds.Add("SOUND_CAS6_BC", 8208)
        Sounds.Add("SOUND_CAS6_BD", 8209)
        Sounds.Add("SOUND_CAS6_BE", 8210)
        Sounds.Add("SOUND_CAS6_BF", 8211)
        Sounds.Add("SOUND_CAS6_BH", 8212)
        Sounds.Add("SOUND_CAS6_BK", 8213)
        Sounds.Add("SOUND_CAS6_BL", 8214)
        Sounds.Add("SOUND_CAS6_BM", 8215)
        Sounds.Add("SOUND_CAS6_BN", 8216)
        Sounds.Add("SOUND_CAS6_CA", 8217)
        Sounds.Add("SOUND_CAS6_CB", 8218)
        Sounds.Add("SOUND_CAS6_DA", 8219)
        Sounds.Add("SOUND_CAS6_DB", 8220)
        Sounds.Add("SOUND_CAS6_EA", 8221)
        Sounds.Add("SOUND_CAS6_EB", 8222)
        Sounds.Add("SOUND_CAS6_EC", 8223)
        Sounds.Add("SOUND_CAS6_ED", 8224)
        Sounds.Add("SOUND_CAS6_EE", 8225)
        Sounds.Add("SOUND_CAS6_EF", 8226)
        Sounds.Add("SOUND_CAS6_EG", 8227)
        Sounds.Add("SOUND_CAS6_EH", 8228)
        Sounds.Add("SOUND_CAS6_EJ", 8229)
        Sounds.Add("SOUND_CAS6_EK", 8230)
        Sounds.Add("SOUND_CAS6_EL", 8231)
        Sounds.Add("SOUND_CAS6_EM", 8232)
        Sounds.Add("SOUND_CAS6_EN", 8233)
        Sounds.Add("SOUND_CAS6_EO", 8234)
        Sounds.Add("SOUND_CAS6_EP", 8235)
        Sounds.Add("SOUND_CAS6_EQ", 8236)
        Sounds.Add("SOUND_CAS6_ER", 8237)
        Sounds.Add("SOUND_CAS6_ES", 8238)
        Sounds.Add("SOUND_CAS6_ET", 8239)
        Sounds.Add("SOUND_CAS6_EU", 8240)
        Sounds.Add("SOUND_CAS6_FA", 8241)
        Sounds.Add("SOUND_CAS6_FC", 8242)
        Sounds.Add("SOUND_CAS6_FF", 8243)
        Sounds.Add("SOUND_CAS6_FK", 8244)
        Sounds.Add("SOUND_CAS6_FM", 8245)
        Sounds.Add("SOUND_CAS6_FN", 8246)
        Sounds.Add("SOUND_CAS6_FO", 8247)
        Sounds.Add("SOUND_CAS6_FP", 8248)
        Sounds.Add("SOUND_CAS6_GB", 8249)
        Sounds.Add("SOUND_CAS6_GH", 8250)
        Sounds.Add("SOUND_CAS6_GJ", 8251)
        Sounds.Add("SOUND_CAS6_GK", 8252)
        Sounds.Add("SOUND_CAS6_HA", 8253)
        Sounds.Add("SOUND_CAS6_HB", 8254)
        Sounds.Add("SOUND_CAS6_HC", 8255)
        Sounds.Add("SOUND_CAS6_HD", 8256)
        Sounds.Add("SOUND_CAS6_JA", 8257)
        Sounds.Add("SOUND_CAS6_JB", 8258)
        Sounds.Add("SOUND_CAS6_JC", 8259)
        Sounds.Add("SOUND_CAS6_JD", 8260)
        Sounds.Add("SOUND_CAS6_JE", 8261)
        Sounds.Add("SOUND_CAS6_JF", 8262)
        Sounds.Add("SOUND_CAS6_JG", 8263)
        Sounds.Add("SOUND_CAS6_JJ", 8264)
        Sounds.Add("SOUND_CAS6_JK", 8265)
        Sounds.Add("SOUND_CAS6_JL", 8266)
        Sounds.Add("SOUND_CAS6_JM", 8267)
        Sounds.Add("SOUND_CAS6_JN", 8268)
        Sounds.Add("SOUND_CAS6_JO", 8269)
        Sounds.Add("SOUND_CAS6_KA", 8270)
        Sounds.Add("SOUND_CAS6_KB", 8271)
        Sounds.Add("SOUND_CAS6_KC", 8272)
        Sounds.Add("SOUND_CAS6_KD", 8273)
        Sounds.Add("SOUND_CAS6_KE", 8274)
        Sounds.Add("SOUND_CAS6_LA", 8275)
        Sounds.Add("SOUND_CAS6_LB", 8276)
        Sounds.Add("SOUND_CAS6_LC", 8277)
        Sounds.Add("SOUND_CAS6_LD", 8278)
        Sounds.Add("SOUND_CAS9_AA", 8400)
        Sounds.Add("SOUND_CAS9_AB", 8401)
        Sounds.Add("SOUND_CAS9_AC", 8402)
        Sounds.Add("SOUND_CAS9_AD", 8403)
        Sounds.Add("SOUND_CAS9_BA", 8404)
        Sounds.Add("SOUND_CAS9_BB", 8405)
        Sounds.Add("SOUND_CAS9_BC", 8406)
        Sounds.Add("SOUND_CAS9_BD", 8407)
        Sounds.Add("SOUND_CAS9_CA", 8408)
        Sounds.Add("SOUND_CAS9_CB", 8409)
        Sounds.Add("SOUND_CAS9_CC", 8410)
        Sounds.Add("SOUND_CAS9_CD", 8411)
        Sounds.Add("SOUND_CAS9_DA", 8412)
        Sounds.Add("SOUND_CAT_AA", 8600)
        Sounds.Add("SOUND_CAT_AB", 8601)
        Sounds.Add("SOUND_CAT_AC", 8602)
        Sounds.Add("SOUND_CAT_AD", 8603)
        Sounds.Add("SOUND_CAT_BA", 8604)
        Sounds.Add("SOUND_CAT_BB", 8605)
        Sounds.Add("SOUND_CAT_BC", 8606)
        Sounds.Add("SOUND_CAT_BD", 8607)
        Sounds.Add("SOUND_CAT_BE", 8608)
        Sounds.Add("SOUND_CAT_BF", 8609)
        Sounds.Add("SOUND_CAT_BG", 8610)
        Sounds.Add("SOUND_CAT_BH", 8611)
        Sounds.Add("SOUND_CAT_BI", 8612)
        Sounds.Add("SOUND_CAT_BJ", 8613)
        Sounds.Add("SOUND_CAT_BK", 8614)
        Sounds.Add("SOUND_CAT_CA", 8615)
        Sounds.Add("SOUND_CAT_CB", 8616)
        Sounds.Add("SOUND_CAT_CC", 8617)
        Sounds.Add("SOUND_CAT_CD", 8618)
        Sounds.Add("SOUND_CAT_CE", 8619)
        Sounds.Add("SOUND_CAT_CF", 8620)
        Sounds.Add("SOUND_CAT_CG", 8621)
        Sounds.Add("SOUND_CAT_CH", 8622)
        Sounds.Add("SOUND_CAT_CI", 8623)
        Sounds.Add("SOUND_CAT_CJ", 8624)
        Sounds.Add("SOUND_CAT_CK", 8625)
        Sounds.Add("SOUND_CAT_DA", 8626)
        Sounds.Add("SOUND_CAT_DB", 8627)
        Sounds.Add("SOUND_CAT_DC", 8628)
        Sounds.Add("SOUND_CAT_DD", 8629)
        Sounds.Add("SOUND_CAT_DE", 8630)
        Sounds.Add("SOUND_CAT_DF", 8631)
        Sounds.Add("SOUND_CAT_DG", 8632)
        Sounds.Add("SOUND_CAT_DH", 8633)
        Sounds.Add("SOUND_CAT_DI", 8634)
        Sounds.Add("SOUND_CAT_DJ", 8635)
        Sounds.Add("SOUND_CAT_DK", 8636)
        Sounds.Add("SOUND_CAT_DL", 8637)
        Sounds.Add("SOUND_CAT_DM", 8638)
        Sounds.Add("SOUND_CAT_DN", 8639)
        Sounds.Add("SOUND_CAT_DO", 8640)
        Sounds.Add("SOUND_CAT_DP", 8641)
        Sounds.Add("SOUND_CAT_EA", 8642)
        Sounds.Add("SOUND_CAT_EB", 8643)
        Sounds.Add("SOUND_CAT_EC", 8644)
        Sounds.Add("SOUND_CAT_ED", 8645)
        Sounds.Add("SOUND_CAT_EE", 8646)
        Sounds.Add("SOUND_CAT_EF", 8647)
        Sounds.Add("SOUND_CAT_EG", 8648)
        Sounds.Add("SOUND_CAT_EH", 8649)
        Sounds.Add("SOUND_CAT_EI", 8650)
        Sounds.Add("SOUND_CAT_EJ", 8651)
        Sounds.Add("SOUND_CAT_EK", 8652)
        Sounds.Add("SOUND_CAT_EL", 8653)
        Sounds.Add("SOUND_CAT_EM", 8654)
        Sounds.Add("SOUND_CAT_EN", 8655)
        Sounds.Add("SOUND_CAT_EO", 8656)
        Sounds.Add("SOUND_CAT_FA", 8657)
        Sounds.Add("SOUND_CAT_FB", 8658)
        Sounds.Add("SOUND_CAT_FC", 8659)
        Sounds.Add("SOUND_CAT_FD", 8660)
        Sounds.Add("SOUND_CAT_FE", 8661)
        Sounds.Add("SOUND_CAT_FF", 8662)
        Sounds.Add("SOUND_CAT_FG", 8663)
        Sounds.Add("SOUND_CAT_FH", 8664)
        Sounds.Add("SOUND_CAT_FI", 8665)
        Sounds.Add("SOUND_CAT_FJ", 8666)
        Sounds.Add("SOUND_CAT_FK", 8667)
        Sounds.Add("SOUND_CAT_FL", 8668)
        Sounds.Add("SOUND_CAT_FM", 8669)
        Sounds.Add("SOUND_CAT_FN", 8670)
        Sounds.Add("SOUND_CAT_FO", 8671)
        Sounds.Add("SOUND_CAT_GA", 8672)
        Sounds.Add("SOUND_CAT_GB", 8673)
        Sounds.Add("SOUND_CAT_GC", 8674)
        Sounds.Add("SOUND_CAT_GD", 8675)
        Sounds.Add("SOUND_CAT_GE", 8676)
        Sounds.Add("SOUND_CAT_GF", 8677)
        Sounds.Add("SOUND_CAT_GG", 8678)
        Sounds.Add("SOUND_CAT_GH", 8679)
        Sounds.Add("SOUND_CAT_GI", 8680)
        Sounds.Add("SOUND_CAT_GJ", 8681)
        Sounds.Add("SOUND_CAT_GK", 8682)
        Sounds.Add("SOUND_CAT_GL", 8683)
        Sounds.Add("SOUND_CAT_GM", 8684)
        Sounds.Add("SOUND_CAT_GN", 8685)
        Sounds.Add("SOUND_CAT_HA", 8686)
        Sounds.Add("SOUND_CAT_HB", 8687)
        Sounds.Add("SOUND_CAT_HC", 8688)
        Sounds.Add("SOUND_CAT_HD", 8689)
        Sounds.Add("SOUND_CAT_HE", 8690)
        Sounds.Add("SOUND_CAT_HF", 8691)
        Sounds.Add("SOUND_CAT_KA", 8692)
        Sounds.Add("SOUND_CAT_KB", 8693)
        Sounds.Add("SOUND_CAT_KC", 8694)
        Sounds.Add("SOUND_CAT_KD", 8695)
        Sounds.Add("SOUND_CAT_KE", 8696)
        Sounds.Add("SOUND_CAT_KF", 8697)
        Sounds.Add("SOUND_CAT_KG", 8698)
        Sounds.Add("SOUND_CAT_KH", 8699)
        Sounds.Add("SOUND_CAT_KI", 8700)
        Sounds.Add("SOUND_CAT_KJ", 8701)
        Sounds.Add("SOUND_CAT_KK", 8702)
        Sounds.Add("SOUND_CAT_KL", 8703)
        Sounds.Add("SOUND_CAT_KM", 8704)
        Sounds.Add("SOUND_CAT_KN", 8705)
        Sounds.Add("SOUND_CAT_KO", 8706)
        Sounds.Add("SOUND_CAT_KP", 8707)
        Sounds.Add("SOUND_CAT_KQ", 8708)
        Sounds.Add("SOUND_CAT_KR", 8709)
        Sounds.Add("SOUND_CAT_KS", 8710)
        Sounds.Add("SOUND_CAT_KT", 8711)
        Sounds.Add("SOUND_CAT_LA", 8712)
        Sounds.Add("SOUND_CAT_LB", 8713)
        Sounds.Add("SOUND_CAT_MA", 8714)
        Sounds.Add("SOUND_CAT_MB", 8715)
        Sounds.Add("SOUND_CAT_MC", 8716)
        Sounds.Add("SOUND_CAT_NA", 8717)
        Sounds.Add("SOUND_CAT_NB", 8718)
        Sounds.Add("SOUND_CAT_NC", 8719)
        Sounds.Add("SOUND_CAT_ND", 8720)
        Sounds.Add("SOUND_CAT_NE", 8721)
        Sounds.Add("SOUND_CAT_NF", 8722)
        Sounds.Add("SOUND_CAT_NG", 8723)
        Sounds.Add("SOUND_CAT_NH", 8724)
        Sounds.Add("SOUND_CAT_OA", 8725)
        Sounds.Add("SOUND_CAT_OB", 8726)
        Sounds.Add("SOUND_CAT_OC", 8727)
        Sounds.Add("SOUND_CAT_OD", 8728)
        Sounds.Add("SOUND_CAT_OE", 8729)
        Sounds.Add("SOUND_CAT_OF", 8730)
        Sounds.Add("SOUND_CAT_OG", 8731)
        Sounds.Add("SOUND_CAT_OH", 8732)
        Sounds.Add("SOUND_CAT_PA", 8733)
        Sounds.Add("SOUND_CAT_PB", 8734)
        Sounds.Add("SOUND_CAT_PC", 8735)
        Sounds.Add("SOUND_CAT_PD", 8736)
        Sounds.Add("SOUND_CAT_PE", 8737)
        Sounds.Add("SOUND_CAT_PF", 8738)
        Sounds.Add("SOUND_CAT1_AA", 8800)
        Sounds.Add("SOUND_CAT1_AB", 8801)
        Sounds.Add("SOUND_CAT1_AC", 8802)
        Sounds.Add("SOUND_CAT1_AD", 8803)
        Sounds.Add("SOUND_CAT1_AE", 8804)
        Sounds.Add("SOUND_CAT1_AF", 8805)
        Sounds.Add("SOUND_CAT1_AG", 8806)
        Sounds.Add("SOUND_CAT1_AH", 8807)
        Sounds.Add("SOUND_CAT1_BA", 8808)
        Sounds.Add("SOUND_CAT1_BB", 8809)
        Sounds.Add("SOUND_CAT1_BC", 8810)
        Sounds.Add("SOUND_CAT1_CA", 8811)
        Sounds.Add("SOUND_CAT1_CB", 8812)
        Sounds.Add("SOUND_CAT1_CC", 8813)
        Sounds.Add("SOUND_CAT1_CD", 8814)
        Sounds.Add("SOUND_CAT1_CE", 8815)
        Sounds.Add("SOUND_CAT1_DA", 8816)
        Sounds.Add("SOUND_CAT1_DB", 8817)
        Sounds.Add("SOUND_CAT1_DC", 8818)
        Sounds.Add("SOUND_CAT1_DD", 8819)
        Sounds.Add("SOUND_CAT1_EA", 8820)
        Sounds.Add("SOUND_CAT1_EB", 8821)
        Sounds.Add("SOUND_CAT1_EC", 8822)
        Sounds.Add("SOUND_CAT1_ED", 8823)
        Sounds.Add("SOUND_CAT1_FA", 8824)
        Sounds.Add("SOUND_CAT1_FB", 8825)
        Sounds.Add("SOUND_CAT1_GA", 8826)
        Sounds.Add("SOUND_CAT1_GB", 8827)
        Sounds.Add("SOUND_CAT1_GC", 8828)
        Sounds.Add("SOUND_CAT1_GD", 8829)
        Sounds.Add("SOUND_CAT1_HA", 8830)
        Sounds.Add("SOUND_CAT1_HB", 8831)
        Sounds.Add("SOUND_CAT1_HC", 8832)
        Sounds.Add("SOUND_CAT1_HD", 8833)
        Sounds.Add("SOUND_CAT1_HE", 8834)
        Sounds.Add("SOUND_CAT1_IA", 8835)
        Sounds.Add("SOUND_CAT1_IB", 8836)
        Sounds.Add("SOUND_CAT1_IC", 8837)
        Sounds.Add("SOUND_CAT1_IE", 8838)
        Sounds.Add("SOUND_CAT1_IF", 8839)
        Sounds.Add("SOUND_CAT1_IG", 8840)
        Sounds.Add("SOUND_CAT2_AA", 9000)
        Sounds.Add("SOUND_CAT2_AB", 9001)
        Sounds.Add("SOUND_CAT2_AC", 9002)
        Sounds.Add("SOUND_CAT2_AF", 9003)
        Sounds.Add("SOUND_CAT2_AG", 9004)
        Sounds.Add("SOUND_CAT2_AH", 9005)
        Sounds.Add("SOUND_CAT2_AI", 9006)
        Sounds.Add("SOUND_CAT2_CA", 9007)
        Sounds.Add("SOUND_CAT2_CB", 9008)
        Sounds.Add("SOUND_CAT2_CC", 9009)
        Sounds.Add("SOUND_CAT2_DA", 9010)
        Sounds.Add("SOUND_CAT2_DB", 9011)
        Sounds.Add("SOUND_CAT2_DC", 9012)
        Sounds.Add("SOUND_CAT2_EA", 9013)
        Sounds.Add("SOUND_CAT2_EB", 9014)
        Sounds.Add("SOUND_CAT2_EC", 9015)
        Sounds.Add("SOUND_CAT2_FA", 9016)
        Sounds.Add("SOUND_CAT2_FB", 9017)
        Sounds.Add("SOUND_CAT2_FC", 9018)
        Sounds.Add("SOUND_CAT2_GA", 9019)
        Sounds.Add("SOUND_CAT2_GB", 9020)
        Sounds.Add("SOUND_CAT2_GC", 9021)
        Sounds.Add("SOUND_CAT2_HA", 9022)
        Sounds.Add("SOUND_CAT2_HB", 9023)
        Sounds.Add("SOUND_CAT2_HC", 9024)
        Sounds.Add("SOUND_CAT2_HE", 9025)
        Sounds.Add("SOUND_CAT2_HF", 9026)
        Sounds.Add("SOUND_CAT2_HG", 9027)
        Sounds.Add("SOUND_CAT2_HH", 9028)
        Sounds.Add("SOUND_CAT2_HI", 9029)
        Sounds.Add("SOUND_CAT2_HJ", 9030)
        Sounds.Add("SOUND_CAT2_JA", 9031)
        Sounds.Add("SOUND__CAT2_SECURITY_ALARM", 9200)
        Sounds.Add("SOUND__CAT2_WOODEN_DOOR_BREACH", 9201)
        Sounds.Add("SOUND_CAT3_AA", 9400)
        Sounds.Add("SOUND_CAT3_AB", 9401)
        Sounds.Add("SOUND_CAT3_AC", 9402)
        Sounds.Add("SOUND_CAT3_AD", 9403)
        Sounds.Add("SOUND_CAT3_AE", 9404)
        Sounds.Add("SOUND_CAT3_AF", 9405)
        Sounds.Add("SOUND_CAT3_AG", 9406)
        Sounds.Add("SOUND_CAT3_BA", 9407)
        Sounds.Add("SOUND_CAT3_BB", 9408)
        Sounds.Add("SOUND_CAT3_BC", 9409)
        Sounds.Add("SOUND_CAT3_BD", 9410)
        Sounds.Add("SOUND_CAT3_CA", 9411)
        Sounds.Add("SOUND_CAT3_CB", 9412)
        Sounds.Add("SOUND_CAT3_CC", 9413)
        Sounds.Add("SOUND_CAT3_CD", 9414)
        Sounds.Add("SOUND_CAT3_DA", 9415)
        Sounds.Add("SOUND_CAT3_DB", 9416)
        Sounds.Add("SOUND_CAT3_DC", 9417)
        Sounds.Add("SOUND_CAT3_DD", 9418)
        Sounds.Add("SOUND_CAT3_EA", 9419)
        Sounds.Add("SOUND_CAT3_EB", 9420)
        Sounds.Add("SOUND_CAT3_EC", 9421)
        Sounds.Add("SOUND_CAT3_ED", 9422)
        Sounds.Add("SOUND_CAT3_EE", 9423)
        Sounds.Add("SOUND_CAT3_EF", 9424)
        Sounds.Add("SOUND_CAT3_EG", 9425)
        Sounds.Add("SOUND_CAT3_FA", 9426)
        Sounds.Add("SOUND_CAT3_FB", 9427)
        Sounds.Add("SOUND_CAT3_FC", 9428)
        Sounds.Add("SOUND_CAT3_FD", 9429)
        Sounds.Add("SOUND_CAT3_FE", 9430)
        Sounds.Add("SOUND_CAT3_FF", 9431)
        Sounds.Add("SOUND_CAT3_GA", 9432)
        Sounds.Add("SOUND_CAT3_GB", 9433)
        Sounds.Add("SOUND_CAT3_GC", 9434)
        Sounds.Add("SOUND_CAT3_GD", 9435)
        Sounds.Add("SOUND_CAT3_GE", 9436)
        Sounds.Add("SOUND_CAT3_GF", 9437)
        Sounds.Add("SOUND_CAT3_GG", 9438)
        Sounds.Add("SOUND_CAT3_HA", 9439)
        Sounds.Add("SOUND_CAT3_HB", 9440)
        Sounds.Add("SOUND_CAT3_HC", 9441)
        Sounds.Add("SOUND_CAT3_JA", 9442)
        Sounds.Add("SOUND_CAT3_JB", 9443)
        Sounds.Add("SOUND_CAT3_JC", 9444)
        Sounds.Add("SOUND_CAT3_JD", 9445)
        Sounds.Add("SOUND_CAT3_JE", 9446)
        Sounds.Add("SOUND_CAT3_JF", 9447)
        Sounds.Add("SOUND_CAT3_JG", 9448)
        Sounds.Add("SOUND_CAT3_JH", 9449)
        Sounds.Add("SOUND_CAT3_JJ", 9450)
        Sounds.Add("SOUND_CAT3_JK", 9451)
        Sounds.Add("SOUND_CAT4_AA", 9600)
        Sounds.Add("SOUND_CAT4_AB", 9601)
        Sounds.Add("SOUND_CAT4_AC", 9602)
        Sounds.Add("SOUND_CAT4_AD", 9603)
        Sounds.Add("SOUND_CAT4_AE", 9604)
        Sounds.Add("SOUND_CAT4_AF", 9605)
        Sounds.Add("SOUND_CAT4_AG", 9606)
        Sounds.Add("SOUND_CAT4_AH", 9607)
        Sounds.Add("SOUND_CAT4_AI", 9608)
        Sounds.Add("SOUND_CAT4_AJ", 9609)
        Sounds.Add("SOUND_CAT4_BA", 9610)
        Sounds.Add("SOUND_CAT4_BB", 9611)
        Sounds.Add("SOUND_CAT4_BC", 9612)
        Sounds.Add("SOUND_CAT4_BD", 9613)
        Sounds.Add("SOUND_CAT4_BE", 9614)
        Sounds.Add("SOUND_CAT4_BF", 9615)
        Sounds.Add("SOUND_CAT4_BG", 9616)
        Sounds.Add("SOUND_CAT4_BH", 9617)
        Sounds.Add("SOUND_CAT4_BI", 9618)
        Sounds.Add("SOUND_CAT4_BJ", 9619)
        Sounds.Add("SOUND_CAT4_BK", 9620)
        Sounds.Add("SOUND_CAT4_CA", 9621)
        Sounds.Add("SOUND_CAT4_CB", 9622)
        Sounds.Add("SOUND_CAT4_CC", 9623)
        Sounds.Add("SOUND_CAT4_CD", 9624)
        Sounds.Add("SOUND_CAT4_CE", 9625)
        Sounds.Add("SOUND_CAT4_CF", 9626)
        Sounds.Add("SOUND_CAT4_CG", 9627)
        Sounds.Add("SOUND_CAT4_CH", 9628)
        Sounds.Add("SOUND_CAT4_DA", 9629)
        Sounds.Add("SOUND_CAT4_DB", 9630)
        Sounds.Add("SOUND_CAT4_DC", 9631)
        Sounds.Add("SOUND_CAT4_DD", 9632)
        Sounds.Add("SOUND_CAT4_DE", 9633)
        Sounds.Add("SOUND_CAT4_DF", 9634)
        Sounds.Add("SOUND_CAT4_DG", 9635)
        Sounds.Add("SOUND_CAT4_DH", 9636)
        Sounds.Add("SOUND_CAT4_DI", 9637)
        Sounds.Add("SOUND_CAT4_DJ", 9638)
        Sounds.Add("SOUND_CAT4_DK", 9639)
        Sounds.Add("SOUND_CAT4_DL", 9640)
        Sounds.Add("SOUND_CAT4_DM", 9641)
        Sounds.Add("SOUND_CAT4_EA", 9642)
        Sounds.Add("SOUND_CAT4_EB", 9643)
        Sounds.Add("SOUND_CAT4_EC", 9644)
        Sounds.Add("SOUND_CAT4_ED", 9645)
        Sounds.Add("SOUND_CAT4_FA", 9646)
        Sounds.Add("SOUND_CAT4_FB", 9647)
        Sounds.Add("SOUND_CAT4_FC", 9648)
        Sounds.Add("SOUND_CAT4_FD", 9649)
        Sounds.Add("SOUND_CAT4_GA", 9650)
        Sounds.Add("SOUND_CAT4_GB", 9651)
        Sounds.Add("SOUND_CAT4_HA", 9652)
        Sounds.Add("SOUND_CAT4_HB", 9653)
        Sounds.Add("SOUND_CAT4_HC", 9654)
        Sounds.Add("SOUND_CAT4_HD", 9655)
        Sounds.Add("SOUND_CAT4_HE", 9656)
        Sounds.Add("SOUND_CAT4_HF", 9657)
        Sounds.Add("SOUND_CAT4_HG", 9658)
        Sounds.Add("SOUND_CAT4_HH", 9659)
        Sounds.Add("SOUND_CAT4_HI", 9660)
        Sounds.Add("SOUND_CAT4_HJ", 9661)
        Sounds.Add("SOUND_CAT4_JA", 9662)
        Sounds.Add("SOUND_CAT4_JB", 9663)
        Sounds.Add("SOUND_CAT4_KA", 9664)
        Sounds.Add("SOUND_CAT4_KB", 9665)
        Sounds.Add("SOUND_CAT4_KC", 9666)
        Sounds.Add("SOUND_CAT4_KD", 9667)
        Sounds.Add("SOUND_CAT4_KE", 9668)
        Sounds.Add("SOUND_CAT4_KF", 9669)
        Sounds.Add("SOUND_CAT4_KG", 9670)
        Sounds.Add("SOUND_CAT4_LA", 9671)
        Sounds.Add("SOUND_CAT4_LB", 9672)
        Sounds.Add("SOUND_CAT4_LC", 9673)
        Sounds.Add("SOUND_CAT4_LD", 9674)
        Sounds.Add("SOUND_CAT4_LE", 9675)
        Sounds.Add("SOUND_CAT4_LF", 9676)
        Sounds.Add("SOUND_CATX_AA", 9800)
        Sounds.Add("SOUND_CATX_AB", 9801)
        Sounds.Add("SOUND_CATX_AC", 9802)
        Sounds.Add("SOUND_CATX_AD", 9803)
        Sounds.Add("SOUND_CATX_AE", 9804)
        Sounds.Add("SOUND_CATX_AF", 9805)
        Sounds.Add("SOUND_CATX_AG", 9806)
        Sounds.Add("SOUND_CATX_AH", 9807)
        Sounds.Add("SOUND_CATX_BA", 9808)
        Sounds.Add("SOUND_CATX_BB", 9809)
        Sounds.Add("SOUND_CATX_BC", 9810)
        Sounds.Add("SOUND_CATX_BD", 9811)
        Sounds.Add("SOUND_CATX_BE", 9812)
        Sounds.Add("SOUND_CATX_BF", 9813)
        Sounds.Add("SOUND_CATX_CA", 9814)
        Sounds.Add("SOUND_CATX_CB", 9815)
        Sounds.Add("SOUND_CATX_CC", 9816)
        Sounds.Add("SOUND_CATX_CD", 9817)
        Sounds.Add("SOUND_CATX_CE", 9818)
        Sounds.Add("SOUND_CATX_CF", 9819)
        Sounds.Add("SOUND_CATX_DA", 9820)
        Sounds.Add("SOUND_CATX_DB", 9821)
        Sounds.Add("SOUND_CATX_DC", 9822)
        Sounds.Add("SOUND_CATX_DD", 9823)
        Sounds.Add("SOUND_CATX_DE", 9824)
        Sounds.Add("SOUND_CATX_DF", 9825)
        Sounds.Add("SOUND_CATX_JA", 9826)
        Sounds.Add("SOUND_CATX_JB", 9827)
        Sounds.Add("SOUND_CATX_JC", 9828)
        Sounds.Add("SOUND_CATX_JD", 9829)
        Sounds.Add("SOUND_CATX_JE", 9830)
        Sounds.Add("SOUND_CATX_JF", 9831)
        Sounds.Add("SOUND_CATX_JG", 9832)
        Sounds.Add("SOUND_CATX_JH", 9833)
        Sounds.Add("SOUND_CATX_KA", 9834)
        Sounds.Add("SOUND_CATX_KB", 9835)
        Sounds.Add("SOUND_CATX_KC", 9836)
        Sounds.Add("SOUND_CATX_KD", 9837)
        Sounds.Add("SOUND_CATX_LA", 9838)
        Sounds.Add("SOUND_CATX_LB", 9839)
        Sounds.Add("SOUND_CATX_LC", 9840)
        Sounds.Add("SOUND_CATX_LD", 9841)
        Sounds.Add("SOUND_CATX_MA", 9842)
        Sounds.Add("SOUND_CATX_MB", 9843)
        Sounds.Add("SOUND_CATX_MC", 9844)
        Sounds.Add("SOUND_CATX_MD", 9845)
        Sounds.Add("SOUND_CATX_ME", 9846)
        Sounds.Add("SOUND_CATX_NA", 9847)
        Sounds.Add("SOUND_CATX_NB", 9848)
        Sounds.Add("SOUND_CATX_NC", 9849)
        Sounds.Add("SOUND_CATX_OA", 9850)
        Sounds.Add("SOUND_CATX_OB", 9851)
        Sounds.Add("SOUND_CATX_OC", 9852)
        Sounds.Add("SOUND_CATX_OD", 9853)
        Sounds.Add("SOUND_CATX_PA", 9854)
        Sounds.Add("SOUND_CATX_PB", 9855)
        Sounds.Add("SOUND_CATX_QA", 9856)
        Sounds.Add("SOUND_CATX_QB", 9857)
        Sounds.Add("SOUND_CATX_RA", 9858)
        Sounds.Add("SOUND_CATX_RB", 9859)
        Sounds.Add("SOUND_CATX_RC", 9860)
        Sounds.Add("SOUND_CATX_SA", 9861)
        Sounds.Add("SOUND_CATX_SB", 9862)
        Sounds.Add("SOUND_CATX_SC", 9863)
        Sounds.Add("SOUND_CATX_SD", 9864)
        Sounds.Add("SOUND_CATX_SE", 9865)
        Sounds.Add("SOUND_CATX_SF", 9866)
        Sounds.Add("SOUND_CATX_SG", 9867)
        Sounds.Add("SOUND_CATX_SH", 9868)
        Sounds.Add("SOUND_CATX_TA", 9869)
        Sounds.Add("SOUND_CATX_TB", 9870)
        Sounds.Add("SOUND_CATX_TC", 9871)
        Sounds.Add("SOUND_CATX_TD", 9872)
        Sounds.Add("SOUND_CATX_TE", 9873)
        Sounds.Add("SOUND_CATX_TF", 9874)
        Sounds.Add("SOUND_CATX_TG", 9875)
        Sounds.Add("SOUND_CATX_TH", 9876)
        Sounds.Add("SOUND_CATX_TJ", 9877)
        Sounds.Add("SOUND_CATX_TK", 9878)
        Sounds.Add("SOUND_CATX_TL", 9879)
        Sounds.Add("SOUND_CATX_TM", 9880)
        Sounds.Add("SOUND_CATX_TN", 9881)
        Sounds.Add("SOUND_CATX_TO", 9882)
        Sounds.Add("SOUND_CATX_TP", 9883)
        Sounds.Add("SOUND_CATX_TQ", 9884)
        Sounds.Add("SOUND_CATX_TR", 9885)
        Sounds.Add("SOUND_CATX_TS", 9886)
        Sounds.Add("SOUND_CATX_TT", 9887)
        Sounds.Add("SOUND_CATX_TU", 9888)
        Sounds.Add("SOUND_CATX_UA", 9889)
        Sounds.Add("SOUND_CATX_UB", 9890)
        Sounds.Add("SOUND_CATX_UC", 9891)
        Sounds.Add("SOUND_CATX_UD", 9892)
        Sounds.Add("SOUND_CATX_UE", 9893)
        Sounds.Add("SOUND_CATX_UF", 9894)
        Sounds.Add("SOUND_CATX_UG", 9895)
        Sounds.Add("SOUND_CATX_UH", 9896)
        Sounds.Add("SOUND_CATX_UJ", 9897)
        Sounds.Add("SOUND_CATX_UK", 9898)
        Sounds.Add("SOUND_CATX_VA", 9899)
        Sounds.Add("SOUND_CATX_VB", 9900)
        Sounds.Add("SOUND_CATX_VC", 9901)
        Sounds.Add("SOUND_CATX_VD", 9902)
        Sounds.Add("SOUND_CATX_VE", 9903)
        Sounds.Add("SOUND_CATX_VF", 9904)
        Sounds.Add("SOUND_CATX_VG", 9905)
        Sounds.Add("SOUND_CATX_VH", 9906)
        Sounds.Add("SOUND_CATX_VJ", 9907)
        Sounds.Add("SOUND_CATX_VK", 9908)
        Sounds.Add("SOUND_CATX_VL", 9909)
        Sounds.Add("SOUND_CATX_VM", 9910)
        Sounds.Add("SOUND_CATX_VN", 9911)
        Sounds.Add("SOUND_CATX_VO", 9912)
        Sounds.Add("SOUND_CATX_VP", 9913)
        Sounds.Add("SOUND_CATX_VQ", 9914)
        Sounds.Add("SOUND_CES1_AA", 10000)
        Sounds.Add("SOUND_CES1_AB", 10001)
        Sounds.Add("SOUND_CES1_AC", 10002)
        Sounds.Add("SOUND_CES1_AD", 10003)
        Sounds.Add("SOUND_CES1_BA", 10004)
        Sounds.Add("SOUND_CES1_BB", 10005)
        Sounds.Add("SOUND_CES1_BC", 10006)
        Sounds.Add("SOUND_CES1_BD", 10007)
        Sounds.Add("SOUND_CES1_CA", 10008)
        Sounds.Add("SOUND_CES1_CB", 10009)
        Sounds.Add("SOUND_CES1_CC", 10010)
        Sounds.Add("SOUND_CES1_CD", 10011)
        Sounds.Add("SOUND_CES1_CE", 10012)
        Sounds.Add("SOUND_CES1_CF", 10013)
        Sounds.Add("SOUND_CES1_DA", 10014)
        Sounds.Add("SOUND_CES1_DB", 10015)
        Sounds.Add("SOUND_CES2_AA", 10200)
        Sounds.Add("SOUND_CES2_AB", 10201)
        Sounds.Add("SOUND_CES2_AC", 10202)
        Sounds.Add("SOUND_CES2_AD", 10203)
        Sounds.Add("SOUND_CES2_AE", 10204)
        Sounds.Add("SOUND_CES2_AF", 10205)
        Sounds.Add("SOUND_CES2_AG", 10206)
        Sounds.Add("SOUND_CES2_ZA", 10207)
        Sounds.Add("SOUND_CES2_ZB", 10208)
        Sounds.Add("SOUND_CES2_ZC", 10209)
        Sounds.Add("SOUND_CES2_ZD", 10210)
        Sounds.Add("SOUND_CES2_ZE", 10211)
        Sounds.Add("SOUND_CES2_ZF", 10212)
        Sounds.Add("SOUND_CES2_ZG", 10213)
        Sounds.Add("SOUND_CES2_ZH", 10214)
        Sounds.Add("SOUND_CESX_AA", 10400)
        Sounds.Add("SOUND_CESX_AB", 10401)
        Sounds.Add("SOUND_CESX_AC", 10402)
        Sounds.Add("SOUND_CESX_AD", 10403)
        Sounds.Add("SOUND_CESX_AE", 10404)
        Sounds.Add("SOUND_CESX_AF", 10405)
        Sounds.Add("SOUND_CESX_BA", 10406)
        Sounds.Add("SOUND_CESX_BB", 10407)
        Sounds.Add("SOUND_CESX_BC", 10408)
        Sounds.Add("SOUND_CESX_BD", 10409)
        Sounds.Add("SOUND_CRA1_AA", 10600)
        Sounds.Add("SOUND_CRA1_AB", 10601)
        Sounds.Add("SOUND_CRA1_AC", 10602)
        Sounds.Add("SOUND_CRA1_AD", 10603)
        Sounds.Add("SOUND_CRA1_AE", 10604)
        Sounds.Add("SOUND_CRA1_AF", 10605)
        Sounds.Add("SOUND_CRA1_AG", 10606)
        Sounds.Add("SOUND_CRA1_AH", 10607)
        Sounds.Add("SOUND_CRA1_AI", 10608)
        Sounds.Add("SOUND_CRA1_AJ", 10609)
        Sounds.Add("SOUND_CRA1_BA", 10610)
        Sounds.Add("SOUND_CRA1_BB", 10611)
        Sounds.Add("SOUND_CRA1_BC", 10612)
        Sounds.Add("SOUND_CRA1_BD", 10613)
        Sounds.Add("SOUND_CRA1_BE", 10614)
        Sounds.Add("SOUND_CRA1_BF", 10615)
        Sounds.Add("SOUND_CRA1_BG", 10616)
        Sounds.Add("SOUND_CRA1_BH", 10617)
        Sounds.Add("SOUND_CRA1_BJ", 10618)
        Sounds.Add("SOUND_CRA1_BK", 10619)
        Sounds.Add("SOUND_CRA1_CA", 10620)
        Sounds.Add("SOUND_CRA1_CB", 10621)
        Sounds.Add("SOUND_CRA1_DA", 10622)
        Sounds.Add("SOUND_CRA1_DB", 10623)
        Sounds.Add("SOUND_CRA1_DC", 10624)
        Sounds.Add("SOUND_CRA1_DD", 10625)
        Sounds.Add("SOUND_CRA1_EA", 10626)
        Sounds.Add("SOUND_CRA1_EB", 10627)
        Sounds.Add("SOUND_CRA1_EC", 10628)
        Sounds.Add("SOUND_CRA1_ED", 10629)
        Sounds.Add("SOUND_CRA1_FA", 10630)
        Sounds.Add("SOUND_CRA1_FB", 10631)
        Sounds.Add("SOUND_CRA1_GA", 10632)
        Sounds.Add("SOUND_CRA1_GB", 10633)
        Sounds.Add("SOUND_CRA1_GC", 10634)
        Sounds.Add("SOUND_CRA1_HA", 10635)
        Sounds.Add("SOUND_CRA1_HB", 10636)
        Sounds.Add("SOUND_CRA1_HC", 10637)
        Sounds.Add("SOUND_CRA1_HD", 10638)
        Sounds.Add("SOUND_CRA1_HE", 10639)
        Sounds.Add("SOUND_CRA1_HF", 10640)
        Sounds.Add("SOUND_CRA1_HG", 10641)
        Sounds.Add("SOUND_CRA1_HH", 10642)
        Sounds.Add("SOUND_CRA1_HJ", 10643)
        Sounds.Add("SOUND_CRA1_HK", 10644)
        Sounds.Add("SOUND_CRA1_HL", 10645)
        Sounds.Add("SOUND_CRA1_JA", 10646)
        Sounds.Add("SOUND_CRA1_JB", 10647)
        Sounds.Add("SOUND_CRA1_JC", 10648)
        Sounds.Add("SOUND_CRA1_JD", 10649)
        Sounds.Add("SOUND_CRA1_KA", 10650)
        Sounds.Add("SOUND_CRA1_KB", 10651)
        Sounds.Add("SOUND_CRA1_KC", 10652)
        Sounds.Add("SOUND_CRA1_KD", 10653)
        Sounds.Add("SOUND_CRA1_KE", 10654)
        Sounds.Add("SOUND_CRA1_KF", 10655)
        Sounds.Add("SOUND_CRA1_KG", 10656)
        Sounds.Add("SOUND_CRA1_KH", 10657)
        Sounds.Add("SOUND_CRA1_KJ", 10658)
        Sounds.Add("SOUND_CRA1_LA", 10659)
        Sounds.Add("SOUND_CRA1_MA", 10660)
        Sounds.Add("SOUND_CRA1_MB", 10661)
        Sounds.Add("SOUND_CRA1_MC", 10662)
        Sounds.Add("SOUND_CRA1_MD", 10663)
        Sounds.Add("SOUND_CRA2_CA", 10800)
        Sounds.Add("SOUND_CRA2_CB", 10801)
        Sounds.Add("SOUND_CRA2_CC", 10802)
        Sounds.Add("SOUND_CRA2_CD", 10803)
        Sounds.Add("SOUND_CRA2_CE", 10804)
        Sounds.Add("SOUND_CRA2_CF", 10805)
        Sounds.Add("SOUND_CRA2_CG", 10806)
        Sounds.Add("SOUND_CRA2_CH", 10807)
        Sounds.Add("SOUND_CRA2_CI", 10808)
        Sounds.Add("SOUND_CRA2_CJ", 10809)
        Sounds.Add("SOUND_CRA2_CK", 10810)
        Sounds.Add("SOUND_CRA2_CL", 10811)
        Sounds.Add("SOUND_CRA2_CM", 10812)
        Sounds.Add("SOUND_CRA2_CN", 10813)
        Sounds.Add("SOUND_CRA2_CO", 10814)
        Sounds.Add("SOUND_CRA2_DA", 10815)
        Sounds.Add("SOUND_CRA2_DB", 10816)
        Sounds.Add("SOUND_CRA2_DC", 10817)
        Sounds.Add("SOUND_CRA2_DD", 10818)
        Sounds.Add("SOUND_CRA2_DE", 10819)
        Sounds.Add("SOUND_CRA2_DF", 10820)
        Sounds.Add("SOUND_CRA2_DG", 10821)
        Sounds.Add("SOUND_CRA2_DH", 10822)
        Sounds.Add("SOUND_CRA2_DI", 10823)
        Sounds.Add("SOUND_CRA2_DJ", 10824)
        Sounds.Add("SOUND_CRA2_DK", 10825)
        Sounds.Add("SOUND_CRA2_DL", 10826)
        Sounds.Add("SOUND_CRA2_DM", 10827)
        Sounds.Add("SOUND_CRA2_DN", 10828)
        Sounds.Add("SOUND_CRA2_DO", 10829)
        Sounds.Add("SOUND_CRA2_ZA", 10830)
        Sounds.Add("SOUND_CRA2_ZB", 10831)
        Sounds.Add("SOUND_CRA2_ZC", 10832)
        Sounds.Add("SOUND_CRA3_BA", 11000)
        Sounds.Add("SOUND_CRA3_CA", 11001)
        Sounds.Add("SOUND_CRA3_CB", 11002)
        Sounds.Add("SOUND_CRA3_DA", 11003)
        Sounds.Add("SOUND_CRA3_DB", 11004)
        Sounds.Add("SOUND_CRA3_DC", 11005)
        Sounds.Add("SOUND_CRA3_DD", 11006)
        Sounds.Add("SOUND_CRA3_DE", 11007)
        Sounds.Add("SOUND_CRA3_EA", 11008)
        Sounds.Add("SOUND_CRA3_EB", 11009)
        Sounds.Add("SOUND_CRA3_EC", 11010)
        Sounds.Add("SOUND_HATCH_LOCK", 11200)
        Sounds.Add("SOUND_DANCE_HIGH_01", 11400)
        Sounds.Add("SOUND_DANCE_HIGH_02", 11401)
        Sounds.Add("SOUND_DANCE_HIGH_03", 11402)
        Sounds.Add("SOUND_DANCE_HIGH_04", 11403)
        Sounds.Add("SOUND_DANCE_HIGH_05", 11404)
        Sounds.Add("SOUND_DANCE_HIGH_06", 11405)
        Sounds.Add("SOUND_DANCE_HIGH_07", 11406)
        Sounds.Add("SOUND_DANCE_HIGH_08", 11407)
        Sounds.Add("SOUND_DANCE_HIGH_09", 11408)
        Sounds.Add("SOUND_DANCE_HIGH_10", 11409)
        Sounds.Add("SOUND_DANCE_HIGH_11", 11410)
        Sounds.Add("SOUND_DANCE_HIGH_12", 11411)
        Sounds.Add("SOUND_DANCE_HIGH_13", 11412)
        Sounds.Add("SOUND_DANCE_HIGH_14", 11413)
        Sounds.Add("SOUND_DANCE_HIGH_15", 11414)
        Sounds.Add("SOUND_DANCE_HIGH_16", 11415)
        Sounds.Add("SOUND_DANCE_HIGH_17", 11416)
        Sounds.Add("SOUND_DANCE_HIGH_18", 11417)
        Sounds.Add("SOUND_DANCE_HIGH_19", 11418)
        Sounds.Add("SOUND_DANCE_HIGH_20", 11419)
        Sounds.Add("SOUND_DANCE_HIGH_21", 11420)
        Sounds.Add("SOUND_DANCE_HIGH_22", 11421)
        Sounds.Add("SOUND_DANCE_HIGH_23", 11422)
        Sounds.Add("SOUND_DANCE_HIGH_24", 11423)
        Sounds.Add("SOUND_DANCE_HIGH_25", 11424)
        Sounds.Add("SOUND_DANCE_HIGH_26", 11425)
        Sounds.Add("SOUND_DANCE_HIGH_27", 11426)
        Sounds.Add("SOUND_DANCE_HIGH_28", 11427)
        Sounds.Add("SOUND_DANCE_HIGH_29", 11428)
        Sounds.Add("SOUND_DANCE_HIGH_30", 11429)
        Sounds.Add("SOUND_DANCE_HIGH_31", 11430)
        Sounds.Add("SOUND_DANCE_HIGH_32", 11431)
        Sounds.Add("SOUND_DANCE_HIGH_33", 11432)
        Sounds.Add("SOUND_DANCE_HIGH_34", 11433)
        Sounds.Add("SOUND_DANCE_HIGH_35", 11434)
        Sounds.Add("SOUND_DANCE_HIGH_36", 11435)
        Sounds.Add("SOUND_DANCE_HIGH_37", 11436)
        Sounds.Add("SOUND_DANCE_HIGH_38", 11437)
        Sounds.Add("SOUND_DANCE_HIGH_39", 11438)
        Sounds.Add("SOUND_DANCE_HIGH_40", 11439)
        Sounds.Add("SOUND_DANCE_HIGH_41", 11440)
        Sounds.Add("SOUND_DANCE_HIGH_42", 11441)
        Sounds.Add("SOUND_DANCE_HIGH_43", 11442)
        Sounds.Add("SOUND_DANCE_HIGH_44", 11443)
        Sounds.Add("SOUND_DANCE_HIGH_45", 11444)
        Sounds.Add("SOUND_DANCE_HIGH_46", 11445)
        Sounds.Add("SOUND_DANCE_HIGH_47", 11446)
        Sounds.Add("SOUND_DANCE_HIGH_48", 11447)
        Sounds.Add("SOUND_DANCE_HIGH_49", 11448)
        Sounds.Add("SOUND_DANCE_HIGH_50", 11449)
        Sounds.Add("SOUND_DANCE_HIGH_51", 11450)
        Sounds.Add("SOUND_DANCE_HIGH_52", 11451)
        Sounds.Add("SOUND_DANCE_HIGH_53", 11452)
        Sounds.Add("SOUND_DANCE_HIGH_54", 11453)
        Sounds.Add("SOUND_DANCE_HIGH_55", 11454)
        Sounds.Add("SOUND_DANCE_HIGH_56", 11455)
        Sounds.Add("SOUND_DANCE_LOW_01", 11600)
        Sounds.Add("SOUND_DANCE_LOW_02", 11601)
        Sounds.Add("SOUND_DANCE_LOW_03", 11602)
        Sounds.Add("SOUND_DANCE_LOW_04", 11603)
        Sounds.Add("SOUND_DANCE_LOW_05", 11604)
        Sounds.Add("SOUND_DANCE_LOW_06", 11605)
        Sounds.Add("SOUND_DANCE_LOW_07", 11606)
        Sounds.Add("SOUND_DANCE_LOW_08", 11607)
        Sounds.Add("SOUND_DANCE_LOW_09", 11608)
        Sounds.Add("SOUND_DANCE_LOW_10", 11609)
        Sounds.Add("SOUND_DANCE_LOW_11", 11610)
        Sounds.Add("SOUND_DANCE_LOW_12", 11611)
        Sounds.Add("SOUND_DANCE_LOW_13", 11612)
        Sounds.Add("SOUND_DANCE_LOW_14", 11613)
        Sounds.Add("SOUND_DANCE_LOW_15", 11614)
        Sounds.Add("SOUND_DANCE_LOW_16", 11615)
        Sounds.Add("SOUND_DANCE_LOW_17", 11616)
        Sounds.Add("SOUND_DANCE_LOW_18", 11617)
        Sounds.Add("SOUND_DANCE_LOW_19", 11618)
        Sounds.Add("SOUND_DANCE_LOW_20", 11619)
        Sounds.Add("SOUND_DANCE_LOW_21", 11620)
        Sounds.Add("SOUND_DANCE_LOW_22", 11621)
        Sounds.Add("SOUND_DANCE_LOW_23", 11622)
        Sounds.Add("SOUND_DANCE_LOW_24", 11623)
        Sounds.Add("SOUND_DANCE_LOW_25", 11624)
        Sounds.Add("SOUND_DANCE_LOW_26", 11625)
        Sounds.Add("SOUND_DANCE_LOW_27", 11626)
        Sounds.Add("SOUND_DANCE_LOW_28", 11627)
        Sounds.Add("SOUND_DANCE_LOW_29", 11628)
        Sounds.Add("SOUND_DANCE_LOW_30", 11629)
        Sounds.Add("SOUND_DANCE_LOW_31", 11630)
        Sounds.Add("SOUND_DANCE_LOW_32", 11631)
        Sounds.Add("SOUND_DANCE_LOW_33", 11632)
        Sounds.Add("SOUND_DANCE_LOW_34", 11633)
        Sounds.Add("SOUND_DANCE_LOW_35", 11634)
        Sounds.Add("SOUND_DANCE_LOW_36", 11635)
        Sounds.Add("SOUND_DANCE_LOW_37", 11636)
        Sounds.Add("SOUND_DANCE_LOW_38", 11637)
        Sounds.Add("SOUND_DANCE_LOW_39", 11638)
        Sounds.Add("SOUND_DANCE_LOW_40", 11639)
        Sounds.Add("SOUND_DANCE_LOW_41", 11640)
        Sounds.Add("SOUND_DANCE_LOW_42", 11641)
        Sounds.Add("SOUND_DANCE_LOW_43", 11642)
        Sounds.Add("SOUND_DANCE_LOW_44", 11643)
        Sounds.Add("SOUND_DANCE_LOW_45", 11644)
        Sounds.Add("SOUND_DANCE_LOW_46", 11645)
        Sounds.Add("SOUND_DANCE_LOW_47", 11646)
        Sounds.Add("SOUND_DANCE_LOW_48", 11647)
        Sounds.Add("SOUND_DANCE_LOW_49", 11648)
        Sounds.Add("SOUND_DANCE_LOW_50", 11649)
        Sounds.Add("SOUND_DANCE_LOW_51", 11650)
        Sounds.Add("SOUND_DANCE_LOW_52", 11651)
        Sounds.Add("SOUND_DANCE_LOW_53", 11652)
        Sounds.Add("SOUND_DANCE_LOW_54", 11653)
        Sounds.Add("SOUND_DANCE_LOW_55", 11654)
        Sounds.Add("SOUND_DANCE_LOW_56", 11655)
        Sounds.Add("SOUND_DANCE_MED_01", 11800)
        Sounds.Add("SOUND_DANCE_MED_02", 11801)
        Sounds.Add("SOUND_DANCE_MED_03", 11802)
        Sounds.Add("SOUND_DANCE_MED_04", 11803)
        Sounds.Add("SOUND_DANCE_MED_05", 11804)
        Sounds.Add("SOUND_DANCE_MED_06", 11805)
        Sounds.Add("SOUND_DANCE_MED_07", 11806)
        Sounds.Add("SOUND_DANCE_MED_08", 11807)
        Sounds.Add("SOUND_DANCE_MED_09", 11808)
        Sounds.Add("SOUND_DANCE_MED_10", 11809)
        Sounds.Add("SOUND_DANCE_MED_11", 11810)
        Sounds.Add("SOUND_DANCE_MED_12", 11811)
        Sounds.Add("SOUND_DANCE_MED_13", 11812)
        Sounds.Add("SOUND_DANCE_MED_14", 11813)
        Sounds.Add("SOUND_DANCE_MED_15", 11814)
        Sounds.Add("SOUND_DANCE_MED_16", 11815)
        Sounds.Add("SOUND_DANCE_MED_17", 11816)
        Sounds.Add("SOUND_DANCE_MED_18", 11817)
        Sounds.Add("SOUND_DANCE_MED_19", 11818)
        Sounds.Add("SOUND_DANCE_MED_20", 11819)
        Sounds.Add("SOUND_DANCE_MED_21", 11820)
        Sounds.Add("SOUND_DANCE_MED_22", 11821)
        Sounds.Add("SOUND_DANCE_MED_23", 11822)
        Sounds.Add("SOUND_DANCE_MED_24", 11823)
        Sounds.Add("SOUND_DANCE_MED_25", 11824)
        Sounds.Add("SOUND_DANCE_MED_26", 11825)
        Sounds.Add("SOUND_DANCE_MED_27", 11826)
        Sounds.Add("SOUND_DANCE_MED_28", 11827)
        Sounds.Add("SOUND_DANCE_MED_29", 11828)
        Sounds.Add("SOUND_DANCE_MED_30", 11829)
        Sounds.Add("SOUND_DANCE_MED_31", 11830)
        Sounds.Add("SOUND_DANCE_MED_32", 11831)
        Sounds.Add("SOUND_DANCE_MED_33", 11832)
        Sounds.Add("SOUND_DANCE_MED_34", 11833)
        Sounds.Add("SOUND_DANCE_MED_35", 11834)
        Sounds.Add("SOUND_DANCE_MED_36", 11835)
        Sounds.Add("SOUND_DANCE_MED_37", 11836)
        Sounds.Add("SOUND_DANCE_MED_38", 11837)
        Sounds.Add("SOUND_DANCE_MED_39", 11838)
        Sounds.Add("SOUND_DANCE_MED_40", 11839)
        Sounds.Add("SOUND_DANCE_MED_41", 11840)
        Sounds.Add("SOUND_DANCE_MED_42", 11841)
        Sounds.Add("SOUND_DANCE_MED_43", 11842)
        Sounds.Add("SOUND_DANCE_MED_44", 11843)
        Sounds.Add("SOUND_DANCE_MED_45", 11844)
        Sounds.Add("SOUND_DANCE_MED_46", 11845)
        Sounds.Add("SOUND_DANCE_MED_47", 11846)
        Sounds.Add("SOUND_DANCE_MED_48", 11847)
        Sounds.Add("SOUND_DANCE_MED_49", 11848)
        Sounds.Add("SOUND_DANCE_MED_50", 11849)
        Sounds.Add("SOUND_DANCE_MED_51", 11850)
        Sounds.Add("SOUND_DANCE_MED_52", 11851)
        Sounds.Add("SOUND_DANCE_MED_53", 11852)
        Sounds.Add("SOUND_DANCE_MED_54", 11853)
        Sounds.Add("SOUND_DANCE_MED_55", 11854)
        Sounds.Add("SOUND_DANCE_MED_56", 11855)
        Sounds.Add("SOUND_DANCE_NOT_01", 12000)
        Sounds.Add("SOUND_DANCE_NOT_02", 12001)
        Sounds.Add("SOUND_DANCE_NOT_03", 12002)
        Sounds.Add("SOUND_DANCE_NOT_04", 12003)
        Sounds.Add("SOUND_DANCE_NOT_05", 12004)
        Sounds.Add("SOUND_DANCE_NOT_06", 12005)
        Sounds.Add("SOUND_DANCE_NOT_07", 12006)
        Sounds.Add("SOUND_DANCE_NOT_08", 12007)
        Sounds.Add("SOUND_DANCE_NOT_09", 12008)
        Sounds.Add("SOUND_DANCE_NOT_10", 12009)
        Sounds.Add("SOUND_DANCE_NOT_11", 12010)
        Sounds.Add("SOUND_DANCE_NOT_12", 12011)
        Sounds.Add("SOUND_DANCE_NOT_13", 12012)
        Sounds.Add("SOUND_DANCE_NOT_14", 12013)
        Sounds.Add("SOUND_DANCE_NOT_15", 12014)
        Sounds.Add("SOUND_DANCE_NOT_16", 12015)
        Sounds.Add("SOUND_DANCE_NOT_17", 12016)
        Sounds.Add("SOUND_DANCE_NOT_18", 12017)
        Sounds.Add("SOUND_DANCE_NOT_19", 12018)
        Sounds.Add("SOUND_DANCE_NOT_20", 12019)
        Sounds.Add("SOUND_DANCE_NOT_21", 12020)
        Sounds.Add("SOUND_DANCE_NOT_22", 12021)
        Sounds.Add("SOUND_DANCE_NOT_23", 12022)
        Sounds.Add("SOUND_DANCE_NOT_24", 12023)
        Sounds.Add("SOUND_DANCE_NOT_25", 12024)
        Sounds.Add("SOUND_DANCE_NOT_26", 12025)
        Sounds.Add("SOUND_DANCE_NOT_27", 12026)
        Sounds.Add("SOUND_DANCE_NOT_28", 12027)
        Sounds.Add("SOUND_DANCE_NOT_29", 12028)
        Sounds.Add("SOUND_DANCE_NOT_30", 12029)
        Sounds.Add("SOUND_DANCE_NOT_31", 12030)
        Sounds.Add("SOUND_DANCE_NOT_32", 12031)
        Sounds.Add("SOUND_DANCE_NOT_33", 12032)
        Sounds.Add("SOUND_DANCE_NOT_34", 12033)
        Sounds.Add("SOUND_DANCE_NOT_35", 12034)
        Sounds.Add("SOUND_DANCE_NOT_36", 12035)
        Sounds.Add("SOUND_DANCE_NOT_37", 12036)
        Sounds.Add("SOUND_DANCE_NOT_38", 12037)
        Sounds.Add("SOUND_DANCE_NOT_39", 12038)
        Sounds.Add("SOUND_DANCE_NOT_40", 12039)
        Sounds.Add("SOUND_DANCE_NOT_41", 12040)
        Sounds.Add("SOUND_DANCE_NOT_42", 12041)
        Sounds.Add("SOUND_DANCE_NOT_43", 12042)
        Sounds.Add("SOUND_DANCE_NOT_44", 12043)
        Sounds.Add("SOUND_DANCE_NOT_45", 12044)
        Sounds.Add("SOUND_DANCE_NOT_46", 12045)
        Sounds.Add("SOUND_DANCE_NOT_47", 12046)
        Sounds.Add("SOUND_DANCE_NOT_48", 12047)
        Sounds.Add("SOUND_DANCE_NOT_49", 12048)
        Sounds.Add("SOUND_DANCE_NOT_50", 12049)
        Sounds.Add("SOUND_DANCE_NOT_51", 12050)
        Sounds.Add("SOUND_DANCE_NOT_52", 12051)
        Sounds.Add("SOUND_DANCE_NOT_53", 12052)
        Sounds.Add("SOUND_DANCE_NOT_54", 12053)
        Sounds.Add("SOUND_DANCE_NOT_55", 12054)
        Sounds.Add("SOUND_DANCE_NOT_56", 12055)
        Sounds.Add("SOUND__DA_NANG_CONTAINER_OPEN", 12200)
        Sounds.Add("SOUND__DA_NANG_HEAVY_DOOR_OPEN", 12201)
        Sounds.Add("SOUND_DC2_AA", 12400)
        Sounds.Add("SOUND_DC2_AB", 12401)
        Sounds.Add("SOUND_DC2_AC", 12402)
        Sounds.Add("SOUND_DC2_AD", 12403)
        Sounds.Add("SOUND_DC2_AE", 12404)
        Sounds.Add("SOUND_DC2_AF", 12405)
        Sounds.Add("SOUND_DC2_AG", 12406)
        Sounds.Add("SOUND_DC2_AH", 12407)
        Sounds.Add("SOUND_DC2_AI", 12408)
        Sounds.Add("SOUND_DC2_AJ", 12409)
        Sounds.Add("SOUND_DC2_AK", 12410)
        Sounds.Add("SOUND_DC2_AL", 12411)
        Sounds.Add("SOUND_CEMENT_POUR", 12600)
        Sounds.Add("SOUND_SMASH_PORTACABIN", 12601)
        Sounds.Add("SOUND_SMASH_PORTACABIN2", 12602)
        Sounds.Add("SOUND_SMASH_PORTACABIN3", 12603)
        Sounds.Add("SOUND_SMASH_PORTACABIN4", 12604)
        Sounds.Add("SOUND_TOILET_FLUSH", 12605)
        Sounds.Add("SOUND_DES1_AA", 12800)
        Sounds.Add("SOUND_DES1_AB", 12801)
        Sounds.Add("SOUND_DES1_AC", 12802)
        Sounds.Add("SOUND_DES1_AD", 12803)
        Sounds.Add("SOUND_DES1_AE", 12804)
        Sounds.Add("SOUND_DES1_AF", 12805)
        Sounds.Add("SOUND_DES1_AG", 12806)
        Sounds.Add("SOUND_DES1_AH", 12807)
        Sounds.Add("SOUND_DES1_AJ", 12808)
        Sounds.Add("SOUND_DES1_AK", 12809)
        Sounds.Add("SOUND_DES1_AL", 12810)
        Sounds.Add("SOUND_DES1_AM", 12811)
        Sounds.Add("SOUND_DES1_AN", 12812)
        Sounds.Add("SOUND_DES1_AO", 12813)
        Sounds.Add("SOUND_DES1_AP", 12814)
        Sounds.Add("SOUND_DES1_AQ", 12815)
        Sounds.Add("SOUND_DES1_AR", 12816)
        Sounds.Add("SOUND_DES1_BA", 12817)
        Sounds.Add("SOUND_DES1_BB", 12818)
        Sounds.Add("SOUND_DES1_BC", 12819)
        Sounds.Add("SOUND_DES1_BD", 12820)
        Sounds.Add("SOUND_DES1_BE", 12821)
        Sounds.Add("SOUND_DES1_BF", 12822)
        Sounds.Add("SOUND_DES1_BG", 12823)
        Sounds.Add("SOUND_DES1_BH", 12824)
        Sounds.Add("SOUND_DES1_BJ", 12825)
        Sounds.Add("SOUND_DES1_CA", 12826)
        Sounds.Add("SOUND_DES1_CB", 12827)
        Sounds.Add("SOUND_DES1_CC", 12828)
        Sounds.Add("SOUND_DES1_CD", 12829)
        Sounds.Add("SOUND_DES1_CE", 12830)
        Sounds.Add("SOUND_DES1_CF", 12831)
        Sounds.Add("SOUND_DES1_CG", 12832)
        Sounds.Add("SOUND_DES2_AA", 13000)
        Sounds.Add("SOUND_DES2_AB", 13001)
        Sounds.Add("SOUND_DES2_AC", 13002)
        Sounds.Add("SOUND_DES2_AD", 13003)
        Sounds.Add("SOUND_DES2_AE", 13004)
        Sounds.Add("SOUND_DES2_AF", 13005)
        Sounds.Add("SOUND_DES2_BA", 13006)
        Sounds.Add("SOUND_DES2_BB", 13007)
        Sounds.Add("SOUND_DES2_BC", 13008)
        Sounds.Add("SOUND_DES2_CA", 13009)
        Sounds.Add("SOUND_DES2_CB", 13010)
        Sounds.Add("SOUND_DES2_CC", 13011)
        Sounds.Add("SOUND_DES2_DA", 13012)
        Sounds.Add("SOUND_DES2_DB", 13013)
        Sounds.Add("SOUND_DES2_DC", 13014)
        Sounds.Add("SOUND_DES2_EA", 13015)
        Sounds.Add("SOUND_DES2_EB", 13016)
        Sounds.Add("SOUND_DES2_EC", 13017)
        Sounds.Add("SOUND_DES2_ED", 13018)
        Sounds.Add("SOUND_DES2_EE", 13019)
        Sounds.Add("SOUND_DES2_EF", 13020)
        Sounds.Add("SOUND_DES2_FA", 13021)
        Sounds.Add("SOUND_DES2_FB", 13022)
        Sounds.Add("SOUND_DES2_FC", 13023)
        Sounds.Add("SOUND_DES2_GA", 13024)
        Sounds.Add("SOUND_DES2_GB", 13025)
        Sounds.Add("SOUND_DES2_GC", 13026)
        Sounds.Add("SOUND_DES2_HA", 13027)
        Sounds.Add("SOUND_DES2_HB", 13028)
        Sounds.Add("SOUND_DES2_HC", 13029)
        Sounds.Add("SOUND_DES2_JA", 13030)
        Sounds.Add("SOUND_DES2_JB", 13031)
        Sounds.Add("SOUND_DES2_JC", 13032)
        Sounds.Add("SOUND_DES2_KA", 13033)
        Sounds.Add("SOUND_DES2_KB", 13034)
        Sounds.Add("SOUND_DES2_KC", 13035)
        Sounds.Add("SOUND_DES2_KD", 13036)
        Sounds.Add("SOUND_DES2_KE", 13037)
        Sounds.Add("SOUND_DES2_LA", 13038)
        Sounds.Add("SOUND_DES3_AA", 13200)
        Sounds.Add("SOUND_DES3_AB", 13201)
        Sounds.Add("SOUND_DES3_AC", 13202)
        Sounds.Add("SOUND_DES3_AD", 13203)
        Sounds.Add("SOUND_DES3_AE", 13204)
        Sounds.Add("SOUND_DES3_BA", 13205)
        Sounds.Add("SOUND_DES3_BB", 13206)
        Sounds.Add("SOUND_DES3_BC", 13207)
        Sounds.Add("SOUND_DES3_BD", 13208)
        Sounds.Add("SOUND_DES3_BE", 13209)
        Sounds.Add("SOUND_DES3_BF", 13210)
        Sounds.Add("SOUND_DES3_BG", 13211)
        Sounds.Add("SOUND_DES3_BH", 13212)
        Sounds.Add("SOUND_DES3_CA", 13213)
        Sounds.Add("SOUND_DES3_CB", 13214)
        Sounds.Add("SOUND_DES3_CC", 13215)
        Sounds.Add("SOUND_DES3_CD", 13216)
        Sounds.Add("SOUND_DES6_AA", 13400)
        Sounds.Add("SOUND_DES6_AB", 13401)
        Sounds.Add("SOUND_DES6_AD", 13402)
        Sounds.Add("SOUND_DES6_AE", 13403)
        Sounds.Add("SOUND_DES6_AF", 13404)
        Sounds.Add("SOUND_DES6_AG", 13405)
        Sounds.Add("SOUND_DES6_AH", 13406)
        Sounds.Add("SOUND_DES6_AJ", 13407)
        Sounds.Add("SOUND_DES6_AK", 13408)
        Sounds.Add("SOUND_DES6_AM", 13409)
        Sounds.Add("SOUND_DES6_AN", 13410)
        Sounds.Add("SOUND_DES6_BA", 13411)
        Sounds.Add("SOUND_DES6_BB", 13412)
        Sounds.Add("SOUND_DES6_BC", 13413)
        Sounds.Add("SOUND_DES6_BD", 13414)
        Sounds.Add("SOUND_DES6_CA", 13415)
        Sounds.Add("SOUND_DES6_CB", 13416)
        Sounds.Add("SOUND_DES6_CC", 13417)
        Sounds.Add("SOUND_DES6_CD", 13418)
        Sounds.Add("SOUND_DES6_DA", 13419)
        Sounds.Add("SOUND_DES8_AA", 13600)
        Sounds.Add("SOUND_DES8_BA", 13601)
        Sounds.Add("SOUND_DES8_BB", 13602)
        Sounds.Add("SOUND_DES8_BC", 13603)
        Sounds.Add("SOUND_DES8_BD", 13604)
        Sounds.Add("SOUND_DES8_CA", 13605)
        Sounds.Add("SOUND_DES8_CB", 13606)
        Sounds.Add("SOUND_DES8_CC", 13607)
        Sounds.Add("SOUND_DES8_CD", 13608)
        Sounds.Add("SOUND_DES8_DA", 13609)
        Sounds.Add("SOUND_DES8_DB", 13610)
        Sounds.Add("SOUND_DES8_DC", 13611)
        Sounds.Add("SOUND_DES8_DD", 13612)
        Sounds.Add("SOUND_DES8_EA", 13613)
        Sounds.Add("SOUND_DES8_EB", 13614)
        Sounds.Add("SOUND_DES8_EC", 13615)
        Sounds.Add("SOUND_DES8_ED", 13616)
        Sounds.Add("SOUND_DES8_EE", 13617)
        Sounds.Add("SOUND_DES8_EF", 13618)
        Sounds.Add("SOUND_DES8_FA", 13619)
        Sounds.Add("SOUND_DES8_FB", 13620)
        Sounds.Add("SOUND_DES8_FC", 13621)
        Sounds.Add("SOUND_DES8_GA", 13622)
        Sounds.Add("SOUND_DES8_GB", 13623)
        Sounds.Add("SOUND_DES8_GC", 13624)
        Sounds.Add("SOUND_DES8_GD", 13625)
        Sounds.Add("SOUND_DES8_GE", 13626)
        Sounds.Add("SOUND_DES8_GF", 13627)
        Sounds.Add("SOUND_DES8_GG", 13628)
        Sounds.Add("SOUND_DES8_GH", 13629)
        Sounds.Add("SOUND_DES8_GI", 13630)
        Sounds.Add("SOUND_DES8_GJ", 13631)
        Sounds.Add("SOUND_DES8_GK", 13632)
        Sounds.Add("SOUND_DES8_GL", 13633)
        Sounds.Add("SOUND_DES8_GM", 13634)
        Sounds.Add("SOUND_DES8_GN", 13635)
        Sounds.Add("SOUND_DES8_HA", 13636)
        Sounds.Add("SOUND_DES8_HB", 13637)
        Sounds.Add("SOUND_DES8_JA", 13638)
        Sounds.Add("SOUND_DES8_JB", 13639)
        Sounds.Add("SOUND_DES8_JC", 13640)
        Sounds.Add("SOUND_DES8_KA", 13641)
        Sounds.Add("SOUND_DES8_LA", 13642)
        Sounds.Add("SOUND_DES8_LC", 13643)
        Sounds.Add("SOUND_DES8_MB", 13644)
        Sounds.Add("SOUND_DES8_MD", 13645)
        Sounds.Add("SOUND_DES8_ME", 13646)
        Sounds.Add("SOUND_DES9_AA", 13800)
        Sounds.Add("SOUND_DES9_AB", 13801)
        Sounds.Add("SOUND_DES9_BA", 13802)
        Sounds.Add("SOUND_DES9_BB", 13803)
        Sounds.Add("SOUND_DOGG_AA", 14000)
        Sounds.Add("SOUND_DOGG_AB", 14001)
        Sounds.Add("SOUND_DOGG_AC", 14002)
        Sounds.Add("SOUND_DOGG_AD", 14003)
        Sounds.Add("SOUND_DOGG_AE", 14004)
        Sounds.Add("SOUND_DOGG_AF", 14005)
        Sounds.Add("SOUND_DOGG_AG", 14006)
        Sounds.Add("SOUND_DOGG_AH", 14007)
        Sounds.Add("SOUND_DOGG_AI", 14008)
        Sounds.Add("SOUND_DOGG_AJ", 14009)
        Sounds.Add("SOUND_DOGG_BA", 14010)
        Sounds.Add("SOUND_DOGG_BB", 14011)
        Sounds.Add("SOUND_DOGG_BC", 14012)
        Sounds.Add("SOUND_DOGG_CA", 14013)
        Sounds.Add("SOUND_DOGG_CB", 14014)
        Sounds.Add("SOUND_DOGG_CC", 14015)
        Sounds.Add("SOUND_DOGG_CD", 14016)
        Sounds.Add("SOUND_DOGG_CE", 14017)
        Sounds.Add("SOUND_DOGG_CF", 14018)
        Sounds.Add("SOUND_DOGG_DA", 14019)
        Sounds.Add("SOUND_DOGG_DB", 14020)
        Sounds.Add("SOUND_DOGG_DC", 14021)
        Sounds.Add("SOUND_DOGG_DD", 14022)
        Sounds.Add("SOUND_DOGG_DE", 14023)
        Sounds.Add("SOUND_DOGG_DF", 14024)
        Sounds.Add("SOUND_DOGG_DG", 14025)
        Sounds.Add("SOUND_DOGG_DH", 14026)
        Sounds.Add("SOUND_DOGG_DI", 14027)
        Sounds.Add("SOUND_DOGG_DJ", 14028)
        Sounds.Add("SOUND_DOGG_EA", 14029)
        Sounds.Add("SOUND_DOGG_EB", 14030)
        Sounds.Add("SOUND_DOGG_EC", 14031)
        Sounds.Add("SOUND_DOGG_FA", 14032)
        Sounds.Add("SOUND_DOGG_FB", 14033)
        Sounds.Add("SOUND_DOGG_FC", 14034)
        Sounds.Add("SOUND_DOGG_FD", 14035)
        Sounds.Add("SOUND_DOGG_FE", 14036)
        Sounds.Add("SOUND_DOGG_FF", 14037)
        Sounds.Add("SOUND_DOGG_FG", 14038)
        Sounds.Add("SOUND_DOGG_GA", 14039)
        Sounds.Add("SOUND_DOGG_GB", 14040)
        Sounds.Add("SOUND_DOGG_GC", 14041)
        Sounds.Add("SOUND_PISSING", 14200)
        Sounds.Add("SOUND_THRUST", 14400)
        Sounds.Add("SOUND_EXPLODE_LONG", 14401)
        Sounds.Add("SOUND_EXPLODE_SHORT", 14402)
        Sounds.Add("SOUND_GAME_OVER", 14403)
        Sounds.Add("SOUND_MENU_DESELECT", 14404)
        Sounds.Add("SOUND_MENU_SELECT", 14405)
        Sounds.Add("SOUND_PICKUP_DARK", 14406)
        Sounds.Add("SOUND_PICKUP_LIGHT", 14407)
        Sounds.Add("SOUND_SHOOT", 14408)
        Sounds.Add("SOUND_TOUCH_DARK", 14409)
        Sounds.Add("SOUND_TOUCH_LIGHT", 14410)
        Sounds.Add("SOUND_SWAT_WALL_BREAK", 14600)
        Sounds.Add("SOUND_DETONATION_SIREN", 14800)
        Sounds.Add("SOUND_FAR2_AA", 15000)
        Sounds.Add("SOUND_FAR2_AB", 15001)
        Sounds.Add("SOUND_FAR2_AC", 15002)
        Sounds.Add("SOUND_FAR2_AD", 15003)
        Sounds.Add("SOUND_FAR2_AE", 15004)
        Sounds.Add("SOUND_FAR2_AF", 15005)
        Sounds.Add("SOUND_FAR2_AG", 15006)
        Sounds.Add("SOUND_FAR2_AH", 15007)
        Sounds.Add("SOUND_FAR2_AI", 15008)
        Sounds.Add("SOUND_FAR2_BA", 15009)
        Sounds.Add("SOUND_FAR2_BB", 15010)
        Sounds.Add("SOUND_FAR2_BC", 15011)
        Sounds.Add("SOUND_FAR2_BD", 15012)
        Sounds.Add("SOUND_FAR2_BE", 15013)
        Sounds.Add("SOUND_FAR2_BF", 15014)
        Sounds.Add("SOUND_FAR2_BG", 15015)
        Sounds.Add("SOUND_FAR2_BH", 15016)
        Sounds.Add("SOUND_FAR2_CA", 15017)
        Sounds.Add("SOUND_FAR2_CB", 15018)
        Sounds.Add("SOUND_FAR2_CC", 15019)
        Sounds.Add("SOUND_FAR2_CD", 15020)
        Sounds.Add("SOUND_FAR2_CE", 15021)
        Sounds.Add("SOUND_FAR2_CF", 15022)
        Sounds.Add("SOUND_FAR2_CG", 15023)
        Sounds.Add("SOUND_FAR2_CH", 15024)
        Sounds.Add("SOUND_FAR2_CI", 15025)
        Sounds.Add("SOUND_FAR2_ZA", 15026)
        Sounds.Add("SOUND_FAR2_ZB", 15027)
        Sounds.Add("SOUND_FAR2_ZC", 15028)
        Sounds.Add("SOUND_FAR3_CA", 15200)
        Sounds.Add("SOUND_FAR3_CB", 15201)
        Sounds.Add("SOUND_FAR3_CC", 15202)
        Sounds.Add("SOUND_FAR3_CD", 15203)
        Sounds.Add("SOUND_FAR3_CE", 15204)
        Sounds.Add("SOUND_FAR3_CF", 15205)
        Sounds.Add("SOUND_FAR3_CG", 15206)
        Sounds.Add("SOUND_FAR3_CH", 15207)
        Sounds.Add("SOUND_FAR3_CJ", 15208)
        Sounds.Add("SOUND_FAR3_CK", 15209)
        Sounds.Add("SOUND_FAR3_DA", 15210)
        Sounds.Add("SOUND_FAR3_DB", 15211)
        Sounds.Add("SOUND_FAR3_DC", 15212)
        Sounds.Add("SOUND_FAR3_DD", 15213)
        Sounds.Add("SOUND_FAR3_DE", 15214)
        Sounds.Add("SOUND_FAR3_EA", 15215)
        Sounds.Add("SOUND_FAR3_EB", 15216)
        Sounds.Add("SOUND_FAR3_EC", 15217)
        Sounds.Add("SOUND_FAR3_FA", 15218)
        Sounds.Add("SOUND_FAR3_FB", 15219)
        Sounds.Add("SOUND_FAR3_FC", 15220)
        Sounds.Add("SOUND_FAR3_FD", 15221)
        Sounds.Add("SOUND_FAR3_GA", 15222)
        Sounds.Add("SOUND_FAR3_GB", 15223)
        Sounds.Add("SOUND_FAR3_GC", 15224)
        Sounds.Add("SOUND_FAR3_HA", 15225)
        Sounds.Add("SOUND_FAR3_HB", 15226)
        Sounds.Add("SOUND_FAR3_HC", 15227)
        Sounds.Add("SOUND_FAR3_JA", 15228)
        Sounds.Add("SOUND_FAR3_JB", 15229)
        Sounds.Add("SOUND_FAR3_JC", 15230)
        Sounds.Add("SOUND_FAR3_JD", 15231)
        Sounds.Add("SOUND_FAR3_JE", 15232)
        Sounds.Add("SOUND_FAR3_JF", 15233)
        Sounds.Add("SOUND_FAR3_JG", 15234)
        Sounds.Add("SOUND_FAR3_KA", 15235)
        Sounds.Add("SOUND_FAR3_KB", 15236)
        Sounds.Add("SOUND_FAR3_KC", 15237)
        Sounds.Add("SOUND_FAR3_KD", 15238)
        Sounds.Add("SOUND_FAR3_LA", 15239)
        Sounds.Add("SOUND_FAR3_LB", 15240)
        Sounds.Add("SOUND_FAR3_LC", 15241)
        Sounds.Add("SOUND_FAR3_MA", 15242)
        Sounds.Add("SOUND_FAR3_MB", 15243)
        Sounds.Add("SOUND_FAR3_MC", 15244)
        Sounds.Add("SOUND_FAR3_MD", 15245)
        Sounds.Add("SOUND_FAR3_ME", 15246)
        Sounds.Add("SOUND_FAR3_MF", 15247)
        Sounds.Add("SOUND_FAR3_MG", 15248)
        Sounds.Add("SOUND_FAR3_MH", 15249)
        Sounds.Add("SOUND_FAR3_MJ", 15250)
        Sounds.Add("SOUND_FAR3_MK", 15251)
        Sounds.Add("SOUND_FAR3_ML", 15252)
        Sounds.Add("SOUND_FAR3_MM", 15253)
        Sounds.Add("SOUND_FAR3_MN", 15254)
        Sounds.Add("SOUND_FAR3_MO", 15255)
        Sounds.Add("SOUND_FAR3_MP", 15256)
        Sounds.Add("SOUND_FAR3_NA", 15257)
        Sounds.Add("SOUND_FAR3_NB", 15258)
        Sounds.Add("SOUND_FAR4_AA", 15400)
        Sounds.Add("SOUND_FAR4_AB", 15401)
        Sounds.Add("SOUND_FAR4_AC", 15402)
        Sounds.Add("SOUND_FAR4_AD", 15403)
        Sounds.Add("SOUND_FAR4_AE", 15404)
        Sounds.Add("SOUND_FAR4_AF", 15405)
        Sounds.Add("SOUND_FAR4_AG", 15406)
        Sounds.Add("SOUND_FAR4_AH", 15407)
        Sounds.Add("SOUND_FAR4_AJ", 15408)
        Sounds.Add("SOUND_FAR5_AA", 15600)
        Sounds.Add("SOUND_FAR5_AB", 15601)
        Sounds.Add("SOUND_FAR5_BA", 15602)
        Sounds.Add("SOUND_FAR5_BB", 15603)
        Sounds.Add("SOUND_FIN1_AA", 15800)
        Sounds.Add("SOUND_FIN1_AC", 15801)
        Sounds.Add("SOUND_FIN1_AD", 15802)
        Sounds.Add("SOUND_FIN1_AE", 15803)
        Sounds.Add("SOUND_FIN1_AH", 15804)
        Sounds.Add("SOUND_FIN1_AS", 15805)
        Sounds.Add("SOUND_FIN1_BC", 15806)
        Sounds.Add("SOUND_FIN1_BD", 15807)
        Sounds.Add("SOUND_FIN1_BE", 15808)
        Sounds.Add("SOUND_FIN1_BK", 15809)
        Sounds.Add("SOUND_FIN1_BL", 15810)
        Sounds.Add("SOUND_FIN1_BY", 15811)
        Sounds.Add("SOUND_FIN1_CB", 15812)
        Sounds.Add("SOUND_FIN1_CC", 15813)
        Sounds.Add("SOUND_FIN1_CD", 15814)
        Sounds.Add("SOUND_FIN1_CE", 15815)
        Sounds.Add("SOUND_FIN1_CI", 15816)
        Sounds.Add("SOUND_FIN1_CJ", 15817)
        Sounds.Add("SOUND_FIN1_CK", 15818)
        Sounds.Add("SOUND_FIN1_CL", 15819)
        Sounds.Add("SOUND_FIN1_CO", 15820)
        Sounds.Add("SOUND_FIN1_DA", 15821)
        Sounds.Add("SOUND_FIN1_DB", 15822)
        Sounds.Add("SOUND_FIN1_DC", 15823)
        Sounds.Add("SOUND_FIN1_EA", 15824)
        Sounds.Add("SOUND_FIN1_EB", 15825)
        Sounds.Add("SOUND_FIN1_GA", 15826)
        Sounds.Add("SOUND_FIN1_GB", 15827)
        Sounds.Add("SOUND_FIN1_GC", 15828)
        Sounds.Add("SOUND_FIN1_GD", 15829)
        Sounds.Add("SOUND_FIN1_GE", 15830)
        Sounds.Add("SOUND_FIN1_GF", 15831)
        Sounds.Add("SOUND_FIN1_GG", 15832)
        Sounds.Add("SOUND_FIN1_GH", 15833)
        Sounds.Add("SOUND_FIN1_GI", 15834)
        Sounds.Add("SOUND_FIN1_GJ", 15835)
        Sounds.Add("SOUND_FIN1_GK", 15836)
        Sounds.Add("SOUND_FIN1_GL", 15837)
        Sounds.Add("SOUND_FIN1_GM", 15838)
        Sounds.Add("SOUND_FIN1_GN", 15839)
        Sounds.Add("SOUND_FIN1_GO", 15840)
        Sounds.Add("SOUND_FIN1_GP", 15841)
        Sounds.Add("SOUND_FIN1_GQ", 15842)
        Sounds.Add("SOUND_FIN1_GR", 15843)
        Sounds.Add("SOUND_FIN1_GS", 15844)
        Sounds.Add("SOUND_FIN1_GT", 15845)
        Sounds.Add("SOUND_FIN1_GV", 15846)
        Sounds.Add("SOUND_FIN1_GW", 15847)
        Sounds.Add("SOUND_FIN1_HA", 15848)
        Sounds.Add("SOUND_FIN1_HB", 15849)
        Sounds.Add("SOUND_FIN1_HC", 15850)
        Sounds.Add("SOUND_FIN1_HD", 15851)
        Sounds.Add("SOUND_FIN1_HE", 15852)
        Sounds.Add("SOUND_FIN1_HF", 15853)
        Sounds.Add("SOUND_FIN1_HG", 15854)
        Sounds.Add("SOUND_FIN1_HH", 15855)
        Sounds.Add("SOUND_FIN1_HI", 15856)
        Sounds.Add("SOUND_FIN1_HJ", 15857)
        Sounds.Add("SOUND_FIN1_HK", 15858)
        Sounds.Add("SOUND_FIN1_HL", 15859)
        Sounds.Add("SOUND_FIN1_HM", 15860)
        Sounds.Add("SOUND_FIN1_JA", 15861)
        Sounds.Add("SOUND_FIN1_JB", 15862)
        Sounds.Add("SOUND_FIN1_JC", 15863)
        Sounds.Add("SOUND_FIN1_JD", 15864)
        Sounds.Add("SOUND_FIN1_JE", 15865)
        Sounds.Add("SOUND_FIN1_JF", 15866)
        Sounds.Add("SOUND_FIN1_JG", 15867)
        Sounds.Add("SOUND_FIN1_JH", 15868)
        Sounds.Add("SOUND_FIN1_JI", 15869)
        Sounds.Add("SOUND_FIN1_JJ", 15870)
        Sounds.Add("SOUND_FIN1_JK", 15871)
        Sounds.Add("SOUND_FIN1_JL", 15872)
        Sounds.Add("SOUND_FIN1_JM", 15873)
        Sounds.Add("SOUND_FIN1_JN", 15874)
        Sounds.Add("SOUND_FIN1_JO", 15875)
        Sounds.Add("SOUND_FIN1_KA", 15876)
        Sounds.Add("SOUND_FIN1_KB", 15877)
        Sounds.Add("SOUND_FIN1_KC", 15878)
        Sounds.Add("SOUND_FIN1_KE", 15879)
        Sounds.Add("SOUND_FIN1_KF", 15880)
        Sounds.Add("SOUND_FIN1_KG", 15881)
        Sounds.Add("SOUND_FIN1_KH", 15882)
        Sounds.Add("SOUND_FIN1_KI", 15883)
        Sounds.Add("SOUND_FIN1_KJ", 15884)
        Sounds.Add("SOUND_FIN1_KK", 15885)
        Sounds.Add("SOUND_FIN1_KL", 15886)
        Sounds.Add("SOUND_FIN1_KM", 15887)
        Sounds.Add("SOUND_FIN1_KO", 15888)
        Sounds.Add("SOUND_FIN1_KP", 15889)
        Sounds.Add("SOUND_FIN1_KQ", 15890)
        Sounds.Add("SOUND_FIN1_KR", 15891)
        Sounds.Add("SOUND_FIN1_KS", 15892)
        Sounds.Add("SOUND_FIN1_KT", 15893)
        Sounds.Add("SOUND_FIN1_KU", 15894)
        Sounds.Add("SOUND_FIN1_KV", 15895)
        Sounds.Add("SOUND_FIN1_KW", 15896)
        Sounds.Add("SOUND_FIN1_KX", 15897)
        Sounds.Add("SOUND_FIN1_KY", 15898)
        Sounds.Add("SOUND_FIN1_LA", 15899)
        Sounds.Add("SOUND_FIN1_LB", 15900)
        Sounds.Add("SOUND_FIN1_LC", 15901)
        Sounds.Add("SOUND_FIN1_LD", 15902)
        Sounds.Add("SOUND_FIN1_LE", 15903)
        Sounds.Add("SOUND_FIN1_LF", 15904)
        Sounds.Add("SOUND_FIN1_LG", 15905)
        Sounds.Add("SOUND_FIN1_LH", 15906)
        Sounds.Add("SOUND_FIN1_LI", 15907)
        Sounds.Add("SOUND_FIN1_LJ", 15908)
        Sounds.Add("SOUND_FIN1_LK", 15909)
        Sounds.Add("SOUND_FIN1_LL", 15910)
        Sounds.Add("SOUND_FIN1_LM", 15911)
        Sounds.Add("SOUND_FIN1_LN", 15912)
        Sounds.Add("SOUND_FIN1_LO", 15913)
        Sounds.Add("SOUND_FIN1_LP", 15914)
        Sounds.Add("SOUND_FIN1_LQ", 15915)
        Sounds.Add("SOUND_FIN1_LR", 15916)
        Sounds.Add("SOUND_FIN1_LS", 15917)
        Sounds.Add("SOUND_FIN1_LT", 15918)
        Sounds.Add("SOUND_FIN1_LU", 15919)
        Sounds.Add("SOUND_FIN1_LV", 15920)
        Sounds.Add("SOUND_FIN1_LW", 15921)
        Sounds.Add("SOUND_FIN1_LX", 15922)
        Sounds.Add("SOUND_FIN1_MA", 15923)
        Sounds.Add("SOUND_FIN1_MB", 15924)
        Sounds.Add("SOUND_FIN1_MC", 15925)
        Sounds.Add("SOUND_FIN1_MD", 15926)
        Sounds.Add("SOUND_FIN1_ME", 15927)
        Sounds.Add("SOUND_FIN1_MF", 15928)
        Sounds.Add("SOUND_FIN1_MG", 15929)
        Sounds.Add("SOUND_FIN1_MH", 15930)
        Sounds.Add("SOUND_FIN1_MI", 15931)
        Sounds.Add("SOUND_FIN1_MJ", 15932)
        Sounds.Add("SOUND_FIN1_MK", 15933)
        Sounds.Add("SOUND_FIN1_ML", 15934)
        Sounds.Add("SOUND_FIN1_MM", 15935)
        Sounds.Add("SOUND_FIN1_MN", 15936)
        Sounds.Add("SOUND_FIN1_MO", 15937)
        Sounds.Add("SOUND_FIN1_MP", 15938)
        Sounds.Add("SOUND_FIN1_MQ", 15939)
        Sounds.Add("SOUND_FIN1_MR", 15940)
        Sounds.Add("SOUND_FIN1_MS", 15941)
        Sounds.Add("SOUND_FIN1_MT", 15942)
        Sounds.Add("SOUND_FIN1_MU", 15943)
        Sounds.Add("SOUND_FIN1_MV", 15944)
        Sounds.Add("SOUND_FIN1_MW", 15945)
        Sounds.Add("SOUND_FIN1_MX", 15946)
        Sounds.Add("SOUND_FIN1_ZA", 15947)
        Sounds.Add("SOUND_FIN1_ZB", 15948)
        Sounds.Add("SOUND_FIN1_ZC", 15949)
        Sounds.Add("SOUND_FIN1_ZD", 15950)
        Sounds.Add("SOUND_FIN2_AA", 16000)
        Sounds.Add("SOUND_FIN2_AB", 16001)
        Sounds.Add("SOUND_FIN2_AC", 16002)
        Sounds.Add("SOUND_FIN2_AD", 16003)
        Sounds.Add("SOUND_FIN2_AE", 16004)
        Sounds.Add("SOUND_FIN2_AF", 16005)
        Sounds.Add("SOUND_FIN2_BA", 16006)
        Sounds.Add("SOUND_FIN2_BB", 16007)
        Sounds.Add("SOUND_FIN2_BC", 16008)
        Sounds.Add("SOUND_FIN2_BD", 16009)
        Sounds.Add("SOUND_FIN2_BE", 16010)
        Sounds.Add("SOUND_FIN2_BF", 16011)
        Sounds.Add("SOUND_FIN2_BG", 16012)
        Sounds.Add("SOUND_FIN2_BH", 16013)
        Sounds.Add("SOUND_FIN2_BJ", 16014)
        Sounds.Add("SOUND_FIN2_CA", 16015)
        Sounds.Add("SOUND_FIN2_CB", 16016)
        Sounds.Add("SOUND_FIN2_CC", 16017)
        Sounds.Add("SOUND_FIN2_CD", 16018)
        Sounds.Add("SOUND_PLANE_DOOR_KICK", 16200)
        Sounds.Add("SOUND_GAR1_AA", 16400)
        Sounds.Add("SOUND_GAR1_AB", 16401)
        Sounds.Add("SOUND_GAR1_AC", 16402)
        Sounds.Add("SOUND_GAR1_AD", 16403)
        Sounds.Add("SOUND_GAR1_AE", 16404)
        Sounds.Add("SOUND_GAR1_AF", 16405)
        Sounds.Add("SOUND_GAR1_AG", 16406)
        Sounds.Add("SOUND_GAR1_AH", 16407)
        Sounds.Add("SOUND_GAR1_AJ", 16408)
        Sounds.Add("SOUND_GAR1_BA", 16409)
        Sounds.Add("SOUND_GAR1_BB", 16410)
        Sounds.Add("SOUND_GAR1_BC", 16411)
        Sounds.Add("SOUND_GAR1_BD", 16412)
        Sounds.Add("SOUND_GAR1_BE", 16413)
        Sounds.Add("SOUND_GAR1_BF", 16414)
        Sounds.Add("SOUND_GAR1_BG", 16415)
        Sounds.Add("SOUND_GAR1_BH", 16416)
        Sounds.Add("SOUND_GAR1_CA", 16417)
        Sounds.Add("SOUND_GAR1_CB", 16418)
        Sounds.Add("SOUND_GAR1_CC", 16419)
        Sounds.Add("SOUND_GAR1_DA", 16420)
        Sounds.Add("SOUND_GAR1_DB", 16421)
        Sounds.Add("SOUND_GAR1_DC", 16422)
        Sounds.Add("SOUND_GAR1_DD", 16423)
        Sounds.Add("SOUND_GAR1_DE", 16424)
        Sounds.Add("SOUND_GAR1_DF", 16425)
        Sounds.Add("SOUND_GAR1_EA", 16426)
        Sounds.Add("SOUND_GAR1_EB", 16427)
        Sounds.Add("SOUND_GAR1_EC", 16428)
        Sounds.Add("SOUND_GAR1_ED", 16429)
        Sounds.Add("SOUND_GAR1_EE", 16430)
        Sounds.Add("SOUND_GAR1_FA", 16431)
        Sounds.Add("SOUND_GAR1_FB", 16432)
        Sounds.Add("SOUND_GAR1_FC", 16433)
        Sounds.Add("SOUND_GAR1_FD", 16434)
        Sounds.Add("SOUND_GAR1_FE", 16435)
        Sounds.Add("SOUND_GAR1_FF", 16436)
        Sounds.Add("SOUND_GAR1_FG", 16437)
        Sounds.Add("SOUND_GAR1_GA", 16438)
        Sounds.Add("SOUND_GAR1_GB", 16439)
        Sounds.Add("SOUND_GAR1_GC", 16440)
        Sounds.Add("SOUND_GAR1_GD", 16441)
        Sounds.Add("SOUND_GAR1_GE", 16442)
        Sounds.Add("SOUND_GAR1_GF", 16443)
        Sounds.Add("SOUND_GAR1_GG", 16444)
        Sounds.Add("SOUND_GAR1_GH", 16445)
        Sounds.Add("SOUND_GAR1_GI", 16446)
        Sounds.Add("SOUND_GAR1_GJ", 16447)
        Sounds.Add("SOUND_GAR1_HA", 16448)
        Sounds.Add("SOUND_GAR1_HB", 16449)
        Sounds.Add("SOUND_GAR1_HC", 16450)
        Sounds.Add("SOUND_GAR1_HD", 16451)
        Sounds.Add("SOUND_GAR1_HE", 16452)
        Sounds.Add("SOUND_GAR1_HF", 16453)
        Sounds.Add("SOUND_GAR1_HG", 16454)
        Sounds.Add("SOUND_GAR1_JA", 16455)
        Sounds.Add("SOUND_GAR1_JB", 16456)
        Sounds.Add("SOUND_GAR1_JC", 16457)
        Sounds.Add("SOUND_GAR1_JD", 16458)
        Sounds.Add("SOUND_GAR1_JE", 16459)
        Sounds.Add("SOUND_GAR1_JF", 16460)
        Sounds.Add("SOUND_GAR1_JG", 16461)
        Sounds.Add("SOUND_GAR1_JH", 16462)
        Sounds.Add("SOUND_GAR1_JJ", 16463)
        Sounds.Add("SOUND_GAR1_KA", 16464)
        Sounds.Add("SOUND_GAR1_KB", 16465)
        Sounds.Add("SOUND_GAR1_KC", 16466)
        Sounds.Add("SOUND_GAR1_KD", 16467)
        Sounds.Add("SOUND_GAR1_KE", 16468)
        Sounds.Add("SOUND_GAR1_LA", 16469)
        Sounds.Add("SOUND_GAR1_LB", 16470)
        Sounds.Add("SOUND_GAR1_LC", 16471)
        Sounds.Add("SOUND_GAR1_LD", 16472)
        Sounds.Add("SOUND_GAR1_MA", 16473)
        Sounds.Add("SOUND_GAR1_MB", 16474)
        Sounds.Add("SOUND_GAR1_MC", 16475)
        Sounds.Add("SOUND_GAR1_MD", 16476)
        Sounds.Add("SOUND_GAR1_ME", 16477)
        Sounds.Add("SOUND_GAR1_MF", 16478)
        Sounds.Add("SOUND_GAR1_MG", 16479)
        Sounds.Add("SOUND_GAR1_NA", 16480)
        Sounds.Add("SOUND_GAR1_NB", 16481)
        Sounds.Add("SOUND_GAR1_NC", 16482)
        Sounds.Add("SOUND_GAR1_ND", 16483)
        Sounds.Add("SOUND_GAR1_NE", 16484)
        Sounds.Add("SOUND_GAR1_NF", 16485)
        Sounds.Add("SOUND_GAR1_NG", 16486)
        Sounds.Add("SOUND_GAR1_NH", 16487)
        Sounds.Add("SOUND_GAR1_NJ", 16488)
        Sounds.Add("SOUND_GAR1_NK", 16489)
        Sounds.Add("SOUND_GAR1_OA", 16490)
        Sounds.Add("SOUND_GAR1_OB", 16491)
        Sounds.Add("SOUND_GAR1_OC", 16492)
        Sounds.Add("SOUND_GAR1_OD", 16493)
        Sounds.Add("SOUND_GAR1_OE", 16494)
        Sounds.Add("SOUND_GAR1_OF", 16495)
        Sounds.Add("SOUND_GAR1_PA", 16496)
        Sounds.Add("SOUND_GAR1_PB", 16497)
        Sounds.Add("SOUND_GAR1_PC", 16498)
        Sounds.Add("SOUND_GAR1_PD", 16499)
        Sounds.Add("SOUND_GAR1_PE", 16500)
        Sounds.Add("SOUND_GAR1_PF", 16501)
        Sounds.Add("SOUND_GAR1_PG", 16502)
        Sounds.Add("SOUND_GAR1_PH", 16503)
        Sounds.Add("SOUND_GAR1_PJ", 16504)
        Sounds.Add("SOUND_GAR2_AA", 16600)
        Sounds.Add("SOUND_GAR2_AB", 16601)
        Sounds.Add("SOUND_GAR2_AC", 16602)
        Sounds.Add("SOUND_GAR2_AD", 16603)
        Sounds.Add("SOUND_GAR2_AE", 16604)
        Sounds.Add("SOUND_GAR2_AF", 16605)
        Sounds.Add("SOUND_GAR2_AG", 16606)
        Sounds.Add("SOUND_GAR2_AH", 16607)
        Sounds.Add("SOUND_GAR2_AJ", 16608)
        Sounds.Add("SOUND_GAR2_AK", 16609)
        Sounds.Add("SOUND_GAR2_AL", 16610)
        Sounds.Add("SOUND_GAR2_AM", 16611)
        Sounds.Add("SOUND_GAR2_BA", 16612)
        Sounds.Add("SOUND_GAR2_BB", 16613)
        Sounds.Add("SOUND_GAR2_BC", 16614)
        Sounds.Add("SOUND_G_CLOS1", 16800)
        Sounds.Add("SOUND_G_CLOS2", 16801)
        Sounds.Add("SOUND_G_OPEN1", 16802)
        Sounds.Add("SOUND_G_OPEN2", 16803)
        Sounds.Add("SOUND__ACCEPT", 17000)
        Sounds.Add("SOUND__DECLINE", 17001)
        Sounds.Add("SOUND__ENEMYFIRE", 17002)
        Sounds.Add("SOUND__EXPLODE", 17003)
        Sounds.Add("SOUND__GAMEOVER", 17004)
        Sounds.Add("SOUND__PLAYERFIRE", 17005)
        Sounds.Add("SOUND__SELECT", 17006)
        Sounds.Add("SOUND_HEAVY_CRATE_LAND", 17200)
        Sounds.Add("SOUND_GRO1_AA", 17400)
        Sounds.Add("SOUND_GRO1_AB", 17401)
        Sounds.Add("SOUND_GRO1_AC", 17402)
        Sounds.Add("SOUND_GRO1_AD", 17403)
        Sounds.Add("SOUND_GRO1_AE", 17404)
        Sounds.Add("SOUND_GRO1_BA", 17405)
        Sounds.Add("SOUND_GRO1_BB", 17406)
        Sounds.Add("SOUND_GRO1_BC", 17407)
        Sounds.Add("SOUND_GRO1_BD", 17408)
        Sounds.Add("SOUND_GRO1_BE", 17409)
        Sounds.Add("SOUND_GRO1_CA", 17410)
        Sounds.Add("SOUND_GRO1_CB", 17411)
        Sounds.Add("SOUND_GRO1_CC", 17412)
        Sounds.Add("SOUND_GRO1_CD", 17413)
        Sounds.Add("SOUND_GRO1_CE", 17414)
        Sounds.Add("SOUND_GRO1_CF", 17415)
        Sounds.Add("SOUND_GRO1_CG", 17416)
        Sounds.Add("SOUND_GRO1_CH", 17417)
        Sounds.Add("SOUND_GRO1_CJ", 17418)
        Sounds.Add("SOUND_GRO1_CK", 17419)
        Sounds.Add("SOUND_GRO1_CL", 17420)
        Sounds.Add("SOUND_GRO1_CM", 17421)
        Sounds.Add("SOUND_GRO1_CN", 17422)
        Sounds.Add("SOUND_GRO1_CO", 17423)
        Sounds.Add("SOUND_GRO1_CP", 17424)
        Sounds.Add("SOUND_GRO1_CQ", 17425)
        Sounds.Add("SOUND_GRO1_CR", 17426)
        Sounds.Add("SOUND_GRO1_CS", 17427)
        Sounds.Add("SOUND_GRO1_DA", 17428)
        Sounds.Add("SOUND_GRO1_DB", 17429)
        Sounds.Add("SOUND_GRO1_EA", 17430)
        Sounds.Add("SOUND_GRO1_EB", 17431)
        Sounds.Add("SOUND_GRO1_FA", 17432)
        Sounds.Add("SOUND_GRO1_FB", 17433)
        Sounds.Add("SOUND_GRO1_FC", 17434)
        Sounds.Add("SOUND_GRO1_FD", 17435)
        Sounds.Add("SOUND_GRO1_FE", 17436)
        Sounds.Add("SOUND_GRO1_FF", 17437)
        Sounds.Add("SOUND_GRO1_FG", 17438)
        Sounds.Add("SOUND_GRO1_FH", 17439)
        Sounds.Add("SOUND_GRO1_FJ", 17440)
        Sounds.Add("SOUND_GRO1_GA", 17441)
        Sounds.Add("SOUND_GRO1_GB", 17442)
        Sounds.Add("SOUND_GRO1_HA", 17443)
        Sounds.Add("SOUND_GRO1_HB", 17444)
        Sounds.Add("SOUND_GRO1_JA", 17445)
        Sounds.Add("SOUND_GRO1_JB", 17446)
        Sounds.Add("SOUND_GRO1_JC", 17447)
        Sounds.Add("SOUND_GRO1_JD", 17448)
        Sounds.Add("SOUND_GRO1_JE", 17449)
        Sounds.Add("SOUND_GRO1_KA", 17450)
        Sounds.Add("SOUND_GRO1_KB", 17451)
        Sounds.Add("SOUND_GRO1_LA", 17452)
        Sounds.Add("SOUND_GRO1_LB", 17453)
        Sounds.Add("SOUND_GRO1_LC", 17454)
        Sounds.Add("SOUND_GRO1_LD", 17455)
        Sounds.Add("SOUND_GRO2_AA", 17600)
        Sounds.Add("SOUND_GRO2_AB", 17601)
        Sounds.Add("SOUND_GRO2_AC", 17602)
        Sounds.Add("SOUND_GRO2_AD", 17603)
        Sounds.Add("SOUND_GRO2_BA", 17604)
        Sounds.Add("SOUND_GRO2_BB", 17605)
        Sounds.Add("SOUND_GRO2_BC", 17606)
        Sounds.Add("SOUND_GRO2_CA", 17607)
        Sounds.Add("SOUND_GRO2_CB", 17608)
        Sounds.Add("SOUND_GRO2_CC", 17609)
        Sounds.Add("SOUND_GRO2_CE", 17610)
        Sounds.Add("SOUND_GRO2_DA", 17611)
        Sounds.Add("SOUND_GRO2_DB", 17612)
        Sounds.Add("SOUND_GRO2_DC", 17613)
        Sounds.Add("SOUND_GRO2_DD", 17614)
        Sounds.Add("SOUND_GRO2_EA", 17615)
        Sounds.Add("SOUND_GRO2_EB", 17616)
        Sounds.Add("SOUND_GRO2_FA", 17617)
        Sounds.Add("SOUND_GRO2_GA", 17618)
        Sounds.Add("SOUND_GRO2_GB", 17619)
        Sounds.Add("SOUND_GRO2_GC", 17620)
        Sounds.Add("SOUND_GRO2_GD", 17621)
        Sounds.Add("SOUND_GRO2_GE", 17622)
        Sounds.Add("SOUND__GYM_BIKE_LOOP", 17800)
        Sounds.Add("SOUND__RUNNING_MACHINE_LOOP", 17801)
        Sounds.Add("SOUND__GYM_BOXING_BELL", 17802)
        Sounds.Add("SOUND__GYM_INCREASE_DIFFICULTY", 17803)
        Sounds.Add("SOUND__GYM_PUNCH_BAG_1", 17804)
        Sounds.Add("SOUND__GYM_PUNCH_BAG_2", 17805)
        Sounds.Add("SOUND__GYM_PUNCH_BAG_3", 17806)
        Sounds.Add("SOUND__GYM_REST_WEIGHTS", 17807)
        Sounds.Add("SOUND_HE1_AA", 18000)
        Sounds.Add("SOUND_HE1_AB", 18001)
        Sounds.Add("SOUND_HE1_AC", 18002)
        Sounds.Add("SOUND_HE1_AD", 18003)
        Sounds.Add("SOUND_HE1_AE", 18004)
        Sounds.Add("SOUND_HE1_AF", 18005)
        Sounds.Add("SOUND_HE1_AG", 18006)
        Sounds.Add("SOUND_HE1_AH", 18007)
        Sounds.Add("SOUND_HE1_AI", 18008)
        Sounds.Add("SOUND_HE1_AJ", 18009)
        Sounds.Add("SOUND_HE1_AK", 18010)
        Sounds.Add("SOUND_HE1_AL", 18011)
        Sounds.Add("SOUND_HE1_XA", 18012)
        Sounds.Add("SOUND_HE1_XB", 18013)
        Sounds.Add("SOUND_HE1_ZA", 18014)
        Sounds.Add("SOUND_HE1_ZB", 18015)
        Sounds.Add("SOUND_HE1_ZC", 18016)
        Sounds.Add("SOUND_HE1_ZD", 18017)
        Sounds.Add("SOUND_HE1_ZE", 18018)
        Sounds.Add("SOUND_HE1_ZF", 18019)
        Sounds.Add("SOUND_HE1_ZG", 18020)
        Sounds.Add("SOUND_HE1_ZH", 18021)
        Sounds.Add("SOUND_HE1_ZJ", 18022)
        Sounds.Add("SOUND_HE1_ZK", 18023)
        Sounds.Add("SOUND_HE1_ZL", 18024)
        Sounds.Add("SOUND_HE1_ZM", 18025)
        Sounds.Add("SOUND_HE2_AA", 18200)
        Sounds.Add("SOUND_HE2_AB", 18201)
        Sounds.Add("SOUND_HE2_AC", 18202)
        Sounds.Add("SOUND_HE2_AD", 18203)
        Sounds.Add("SOUND_HE2_AE", 18204)
        Sounds.Add("SOUND_HE2_AF", 18205)
        Sounds.Add("SOUND_HE2_AG", 18206)
        Sounds.Add("SOUND_HE2_AH", 18207)
        Sounds.Add("SOUND_HE2_AJ", 18208)
        Sounds.Add("SOUND_HE2_AK", 18209)
        Sounds.Add("SOUND_HE2_AL", 18210)
        Sounds.Add("SOUND_HE2_BA", 18211)
        Sounds.Add("SOUND_HE2_BB", 18212)
        Sounds.Add("SOUND_HE2_CA", 18213)
        Sounds.Add("SOUND_HE2_CB", 18214)
        Sounds.Add("SOUND_HE2_DA", 18215)
        Sounds.Add("SOUND_HE2_DB", 18216)
        Sounds.Add("SOUND_HE3_AA", 18400)
        Sounds.Add("SOUND_HE3_AB", 18401)
        Sounds.Add("SOUND_HE3_BA", 18402)
        Sounds.Add("SOUND_HE3_BB", 18403)
        Sounds.Add("SOUND_HE3_CA", 18404)
        Sounds.Add("SOUND_HE3_CB", 18405)
        Sounds.Add("SOUND_HE3_CC", 18406)
        Sounds.Add("SOUND_HE3_CD", 18407)
        Sounds.Add("SOUND_HE3_CE", 18408)
        Sounds.Add("SOUND_HE3_CF", 18409)
        Sounds.Add("SOUND_HE3_CG", 18410)
        Sounds.Add("SOUND_HE3_CH", 18411)
        Sounds.Add("SOUND_HE3_DA", 18412)
        Sounds.Add("SOUND_HE3_DB", 18413)
        Sounds.Add("SOUND_HE3_DC", 18414)
        Sounds.Add("SOUND_HE3_DD", 18415)
        Sounds.Add("SOUND_HE3_DE", 18416)
        Sounds.Add("SOUND_HE3_DF", 18417)
        Sounds.Add("SOUND_HE3_DG", 18418)
        Sounds.Add("SOUND_HE3_EA", 18419)
        Sounds.Add("SOUND_HE3_EB", 18420)
        Sounds.Add("SOUND_HE3_EC", 18421)
        Sounds.Add("SOUND_HE3_FA", 18422)
        Sounds.Add("SOUND_HE3_FB", 18423)
        Sounds.Add("SOUND_HE3_FC", 18424)
        Sounds.Add("SOUND_HE3_FD", 18425)
        Sounds.Add("SOUND_HE3_FE", 18426)
        Sounds.Add("SOUND_HE3_FF", 18427)
        Sounds.Add("SOUND_HE3_FG", 18428)
        Sounds.Add("SOUND_HE3_GA", 18429)
        Sounds.Add("SOUND_HE3_GB", 18430)
        Sounds.Add("SOUND_HE3_GC", 18431)
        Sounds.Add("SOUND_HE3_GD", 18432)
        Sounds.Add("SOUND_HE3_GE", 18433)
        Sounds.Add("SOUND_HE3_HA", 18434)
        Sounds.Add("SOUND_HE4_AA", 18600)
        Sounds.Add("SOUND_HE4_AB", 18601)
        Sounds.Add("SOUND_HE4_AC", 18602)
        Sounds.Add("SOUND_HE4_BA", 18603)
        Sounds.Add("SOUND_HE4_BB", 18604)
        Sounds.Add("SOUND_HE4_BC", 18605)
        Sounds.Add("SOUND_HE4_BD", 18606)
        Sounds.Add("SOUND_HE4_BE", 18607)
        Sounds.Add("SOUND_HE4_BF", 18608)
        Sounds.Add("SOUND_HE4_BG", 18609)
        Sounds.Add("SOUND_HE4_BH", 18610)
        Sounds.Add("SOUND_HE4_BJ", 18611)
        Sounds.Add("SOUND_HE4_BK", 18612)
        Sounds.Add("SOUND_HE4_BL", 18613)
        Sounds.Add("SOUND_HE4_BM", 18614)
        Sounds.Add("SOUND_HE4_CA", 18615)
        Sounds.Add("SOUND_HE4_CB", 18616)
        Sounds.Add("SOUND_HE5_AA", 18800)
        Sounds.Add("SOUND_HE5_AB", 18801)
        Sounds.Add("SOUND_HE5_AC", 18802)
        Sounds.Add("SOUND_HE5_AD", 18803)
        Sounds.Add("SOUND_HE5_BA", 18804)
        Sounds.Add("SOUND_HE5_BB", 18805)
        Sounds.Add("SOUND_HE5_BC", 18806)
        Sounds.Add("SOUND_HE5_BD", 18807)
        Sounds.Add("SOUND_HE8_AA", 19000)
        Sounds.Add("SOUND_HE8_AB", 19001)
        Sounds.Add("SOUND_HE8_AC", 19002)
        Sounds.Add("SOUND_HE8_AD", 19003)
        Sounds.Add("SOUND_HE8_BA", 19004)
        Sounds.Add("SOUND_HE8_BB", 19005)
        Sounds.Add("SOUND_HE8_BC", 19006)
        Sounds.Add("SOUND_HE8_BD", 19007)
        Sounds.Add("SOUND_HE8_BE", 19008)
        Sounds.Add("SOUND_HE8_BF", 19009)
        Sounds.Add("SOUND_HE8_CA", 19010)
        Sounds.Add("SOUND_HE8_CB", 19011)
        Sounds.Add("SOUND_HE8_CC", 19012)
        Sounds.Add("SOUND_HE8_CD", 19013)
        Sounds.Add("SOUND_HE8_CE", 19014)
        Sounds.Add("SOUND_HE8_CF", 19015)
        Sounds.Add("SOUND_HE8_DA", 19016)
        Sounds.Add("SOUND_HE8_DB", 19017)
        Sounds.Add("SOUND_HE8_DC", 19018)
        Sounds.Add("SOUND_HE8_DD", 19019)
        Sounds.Add("SOUND_HE8_EA", 19020)
        Sounds.Add("SOUND_HE8_EB", 19021)
        Sounds.Add("SOUND_HE8_EC", 19022)
        Sounds.Add("SOUND_HE8_ED", 19023)
        Sounds.Add("SOUND_HE8_FA", 19024)
        Sounds.Add("SOUND_HE8_FB", 19025)
        Sounds.Add("SOUND_HE8_FC", 19026)
        Sounds.Add("SOUND_HE8_FD", 19027)
        Sounds.Add("SOUND_HE8_FE", 19028)
        Sounds.Add("SOUND_HE8_GA", 19029)
        Sounds.Add("SOUND_HE8_GB", 19030)
        Sounds.Add("SOUND_HE8_GC", 19031)
        Sounds.Add("SOUND_HE8_GD", 19032)
        Sounds.Add("SOUND_HE8_GE", 19033)
        Sounds.Add("SOUND_HE8_GF", 19034)
        Sounds.Add("SOUND_HE8_GG", 19035)
        Sounds.Add("SOUND_HE8_GH", 19036)
        Sounds.Add("SOUND_HE8_GJ", 19037)
        Sounds.Add("SOUND_HE8_HB", 19038)
        Sounds.Add("SOUND_HE8_JA", 19039)
        Sounds.Add("SOUND_HE8_JB", 19040)
        Sounds.Add("SOUND_HE8_JC", 19041)
        Sounds.Add("SOUND_HE8_KA", 19042)
        Sounds.Add("SOUND_HE8_KB", 19043)
        Sounds.Add("SOUND_HE8_KC", 19044)
        Sounds.Add("SOUND_HE8_KD", 19045)
        Sounds.Add("SOUND_HE8_KE", 19046)
        Sounds.Add("SOUND_HE8_KF", 19047)
        Sounds.Add("SOUND_HE8_KG", 19048)
        Sounds.Add("SOUND_HE8_KH", 19049)
        Sounds.Add("SOUND_HE8_LA", 19050)
        Sounds.Add("SOUND_HE8_LB", 19051)
        Sounds.Add("SOUND_HE8_LC", 19052)
        Sounds.Add("SOUND_HE8_LD", 19053)
        Sounds.Add("SOUND_HE8_LE", 19054)
        Sounds.Add("SOUND_HE8_LF", 19055)
        Sounds.Add("SOUND_HE8_MA", 19056)
        Sounds.Add("SOUND_HE8_MB", 19057)
        Sounds.Add("SOUND_HE8_MC", 19058)
        Sounds.Add("SOUND_HE8_NA", 19059)
        Sounds.Add("SOUND_HE8_NB", 19060)
        Sounds.Add("SOUND_HE8_NC", 19061)
        Sounds.Add("SOUND_HE8_ND", 19062)
        Sounds.Add("SOUND_HE8_NE", 19063)
        Sounds.Add("SOUND_HE8_OA", 19064)
        Sounds.Add("SOUND_HE8_OB", 19065)
        Sounds.Add("SOUND_HE8_OC", 19066)
        Sounds.Add("SOUND_HE8_OD", 19067)
        Sounds.Add("SOUND_HE8_OE", 19068)
        Sounds.Add("SOUND_HE8_OF", 19069)
        Sounds.Add("SOUND_HE8_OG", 19070)
        Sounds.Add("SOUND_HE8_OH", 19071)
        Sounds.Add("SOUND_HE8_OJ", 19072)
        Sounds.Add("SOUND_HE8_OK", 19073)
        Sounds.Add("SOUND_HE8_PA", 19074)
        Sounds.Add("SOUND_HE8_PB", 19075)
        Sounds.Add("SOUND_HE8_PC", 19076)
        Sounds.Add("SOUND_HE8_PD", 19077)
        Sounds.Add("SOUND_HE8_PE", 19078)
        Sounds.Add("SOUND_HE8_PF", 19079)
        Sounds.Add("SOUND_HE8_PG", 19080)
        Sounds.Add("SOUND_HE8_PH", 19081)
        Sounds.Add("SOUND_HE8_PJ", 19082)
        Sounds.Add("SOUND_HE8_QA", 19083)
        Sounds.Add("SOUND_HE8_QB", 19084)
        Sounds.Add("SOUND_HE8_QC", 19085)
        Sounds.Add("SOUND_HE8_RA", 19086)
        Sounds.Add("SOUND_HE8_RB", 19087)
        Sounds.Add("SOUND_HE8_RC", 19088)
        Sounds.Add("SOUND_HE8_RD", 19089)
        Sounds.Add("SOUND_HE8_RE", 19090)
        Sounds.Add("SOUND_HE8_SA", 19091)
        Sounds.Add("SOUND_HE8_SB", 19092)
        Sounds.Add("SOUND_HE8_TA", 19093)
        Sounds.Add("SOUND_HE8_TB", 19094)
        Sounds.Add("SOUND_HE8_TC", 19095)
        Sounds.Add("SOUND_HE8_TE", 19096)
        Sounds.Add("SOUND_HE8_TF", 19097)
        Sounds.Add("SOUND_HE8_TG", 19098)
        Sounds.Add("SOUND_HE8_TH", 19099)
        Sounds.Add("SOUND_HE8_TJ", 19100)
        Sounds.Add("SOUND_HE8_TK", 19101)
        Sounds.Add("SOUND_HE8_TL", 19102)
        Sounds.Add("SOUND_HE8_TM", 19103)
        Sounds.Add("SOUND_HE8_UA", 19104)
        Sounds.Add("SOUND_HE8_UB", 19105)
        Sounds.Add("SOUND_HE8_UC", 19106)
        Sounds.Add("SOUND_HE8_UD", 19107)
        Sounds.Add("SOUND_HE8_UE", 19108)
        Sounds.Add("SOUND_HE8_UF", 19109)
        Sounds.Add("SOUND_HE8_UG", 19110)
        Sounds.Add("SOUND_HE8_UH", 19111)
        Sounds.Add("SOUND_HE8_VA", 19112)
        Sounds.Add("SOUND_HE8_VB", 19113)
        Sounds.Add("SOUND_HE8_VC", 19114)
        Sounds.Add("SOUND_HE8_VD", 19115)
        Sounds.Add("SOUND_HE8_WA", 19116)
        Sounds.Add("SOUND_HE8_WB", 19117)
        Sounds.Add("SOUND_HE8_WC", 19118)
        Sounds.Add("SOUND_HE8_WD", 19119)
        Sounds.Add("SOUND_HE8_WE", 19120)
        Sounds.Add("SOUND_HE8_WF", 19121)
        Sounds.Add("SOUND_HE8_XA", 19122)
        Sounds.Add("SOUND_HE8_XB", 19123)
        Sounds.Add("SOUND_HE8_XC", 19124)
        Sounds.Add("SOUND_HE8_YA", 19125)
        Sounds.Add("SOUND_HE8_YB", 19126)
        Sounds.Add("SOUND_HE8_YC", 19127)
        Sounds.Add("SOUND_HE8_ZA", 19128)
        Sounds.Add("SOUND_HE8_ZB", 19129)
        Sounds.Add("SOUND_HE8_ZD", 19130)
        Sounds.Add("SOUND_HE8_ZE", 19131)
        Sounds.Add("SOUND_HE8_ZF", 19132)
        Sounds.Add("SOUND_HE8_ZG", 19133)
        Sounds.Add("SOUND_HE8_ZH", 19134)
        Sounds.Add("SOUND_HE8_ZJ", 19135)
        Sounds.Add("SOUND_HEIQ1", 19200)
        Sounds.Add("SOUND_HEIQ1N", 19201)
        Sounds.Add("SOUND_HEIQ1Y", 19202)
        Sounds.Add("SOUND_HEIQ2", 19203)
        Sounds.Add("SOUND_HEIQ2N", 19204)
        Sounds.Add("SOUND_HEIQ2Y", 19205)
        Sounds.Add("SOUND_HEIQ2YB", 19206)
        Sounds.Add("SOUND_HEIQ4", 19207)
        Sounds.Add("SOUND_HEIQ4B", 19208)
        Sounds.Add("SOUND_HEIQ4C", 19209)
        Sounds.Add("SOUND_HEIQ5", 19210)
        Sounds.Add("SOUND_HEIQ5B", 19211)
        Sounds.Add("SOUND_HEIQ5C", 19212)
        Sounds.Add("SOUND_HEIX1", 19213)
        Sounds.Add("SOUND_HEIX1B", 19214)
        Sounds.Add("SOUND_HEIX2", 19215)
        Sounds.Add("SOUND_HEIX2B", 19216)
        Sounds.Add("SOUND_HEIX2N", 19217)
        Sounds.Add("SOUND_HEIX2Y", 19218)
        Sounds.Add("SOUND_HEIX3", 19219)
        Sounds.Add("SOUND_TRUCK_SMASH_VEHICLE", 19400)
        Sounds.Add("SOUND_TRUCK_SMASH_VEHICLE_2", 19401)
        Sounds.Add("SOUND_TRUCK_SMASH_VEHICLE_3", 19402)
        Sounds.Add("SOUND_TRUCK_SMASH_VEHICLE_4", 19403)
        Sounds.Add("SOUND_ALARM_CLOCK", 19600)
        Sounds.Add("SOUND_SNORE", 19601)
        Sounds.Add("SOUND_SNORE2", 19602)
        Sounds.Add("SOUND_SNORE3", 19603)
        Sounds.Add("SOUND_SNORE4", 19604)
        Sounds.Add("SOUND_HOUSE_PARTY_BASS_LOOP", 19800)
        Sounds.Add("SOUND_INT1_AA", 20000)
        Sounds.Add("SOUND_INT1_AB", 20001)
        Sounds.Add("SOUND_INT1_AC", 20002)
        Sounds.Add("SOUND_INT1_AD", 20003)
        Sounds.Add("SOUND_INT1_AE", 20004)
        Sounds.Add("SOUND_INT1_AF", 20005)
        Sounds.Add("SOUND_INT1_AG", 20006)
        Sounds.Add("SOUND_INT1_AH", 20007)
        Sounds.Add("SOUND_INT1_AI", 20008)
        Sounds.Add("SOUND_INT1_AJ", 20009)
        Sounds.Add("SOUND_INT1_AK", 20010)
        Sounds.Add("SOUND_INT1_AM", 20011)
        Sounds.Add("SOUND_INT1_AN", 20012)
        Sounds.Add("SOUND_INT1_AO", 20013)
        Sounds.Add("SOUND_INT1_AP", 20014)
        Sounds.Add("SOUND_INT1_AQ", 20015)
        Sounds.Add("SOUND_INT1_AR", 20016)
        Sounds.Add("SOUND_INT1_BA", 20017)
        Sounds.Add("SOUND_INT1_BB", 20018)
        Sounds.Add("SOUND_INT1_BC", 20019)
        Sounds.Add("SOUND_INT1_BD", 20020)
        Sounds.Add("SOUND_INT1_BE", 20021)
        Sounds.Add("SOUND_INT1_BF", 20022)
        Sounds.Add("SOUND_INT1_BG", 20023)
        Sounds.Add("SOUND_INT1_BH", 20024)
        Sounds.Add("SOUND_INT1_BI", 20025)
        Sounds.Add("SOUND_INT1_BJ", 20026)
        Sounds.Add("SOUND_INT1_BK", 20027)
        Sounds.Add("SOUND_INT1_CA", 20028)
        Sounds.Add("SOUND_INT1_CB", 20029)
        Sounds.Add("SOUND_INT1_CC", 20030)
        Sounds.Add("SOUND_INT1_DA", 20031)
        Sounds.Add("SOUND_INT1_DB", 20032)
        Sounds.Add("SOUND_INT1_DC", 20033)
        Sounds.Add("SOUND_INT1_DD", 20034)
        Sounds.Add("SOUND_INT1_DE", 20035)
        Sounds.Add("SOUND_INT1_DF", 20036)
        Sounds.Add("SOUND_INT1_DG", 20037)
        Sounds.Add("SOUND_INT1_DH", 20038)
        Sounds.Add("SOUND_INT1_DI", 20039)
        Sounds.Add("SOUND_INT1_EA", 20040)
        Sounds.Add("SOUND_INT1_EB", 20041)
        Sounds.Add("SOUND_INT1_EC", 20042)
        Sounds.Add("SOUND_INT1_ED", 20043)
        Sounds.Add("SOUND_INT1_EE", 20044)
        Sounds.Add("SOUND_INT1_EF", 20045)
        Sounds.Add("SOUND_INT1_EG", 20046)
        Sounds.Add("SOUND_INT1_EH", 20047)
        Sounds.Add("SOUND_INT1_EI", 20048)
        Sounds.Add("SOUND_INT1_EJ", 20049)
        Sounds.Add("SOUND_INT1_FA", 20050)
        Sounds.Add("SOUND_INT1_FB", 20051)
        Sounds.Add("SOUND_INT1_FC", 20052)
        Sounds.Add("SOUND_INT1_FD", 20053)
        Sounds.Add("SOUND_INT1_FE", 20054)
        Sounds.Add("SOUND_INT1_FF", 20055)
        Sounds.Add("SOUND_INT1_FG", 20056)
        Sounds.Add("SOUND_INT1_FH", 20057)
        Sounds.Add("SOUND_INT1_FI", 20058)
        Sounds.Add("SOUND_INT1_GA", 20059)
        Sounds.Add("SOUND_INT1_GB", 20060)
        Sounds.Add("SOUND_INT1_GC", 20061)
        Sounds.Add("SOUND_INT1_GD", 20062)
        Sounds.Add("SOUND_INT1_GE", 20063)
        Sounds.Add("SOUND_INT1_GF", 20064)
        Sounds.Add("SOUND_INT1_GG", 20065)
        Sounds.Add("SOUND_INT1_GH", 20066)
        Sounds.Add("SOUND_INT1_GI", 20067)
        Sounds.Add("SOUND_INT1_GJ", 20068)
        Sounds.Add("SOUND_INT1_GK", 20069)
        Sounds.Add("SOUND_INT1_GL", 20070)
        Sounds.Add("SOUND_INT1_GM", 20071)
        Sounds.Add("SOUND_INT1_GN", 20072)
        Sounds.Add("SOUND_INT2_AA", 20200)
        Sounds.Add("SOUND_INT2_BA", 20201)
        Sounds.Add("SOUND_INT2_BB", 20202)
        Sounds.Add("SOUND_INT2_BC", 20203)
        Sounds.Add("SOUND_INT2_BD", 20204)
        Sounds.Add("SOUND_INT2_BE", 20205)
        Sounds.Add("SOUND_INT2_BF", 20206)
        Sounds.Add("SOUND_INT2_BG", 20207)
        Sounds.Add("SOUND_INT2_BH", 20208)
        Sounds.Add("SOUND_INT2_BJ", 20209)
        Sounds.Add("SOUND_INT2_BK", 20210)
        Sounds.Add("SOUND_INT2_CA", 20211)
        Sounds.Add("SOUND_INT2_CB", 20212)
        Sounds.Add("SOUND_INT2_CC", 20213)
        Sounds.Add("SOUND_INT2_CD", 20214)
        Sounds.Add("SOUND_INT2_CE", 20215)
        Sounds.Add("SOUND_INT2_CF", 20216)
        Sounds.Add("SOUND_INT2_CG", 20217)
        Sounds.Add("SOUND_INT2_DA", 20218)
        Sounds.Add("SOUND_INT2_DB", 20219)
        Sounds.Add("SOUND_INT2_DC", 20220)
        Sounds.Add("SOUND_INT2_EA", 20221)
        Sounds.Add("SOUND_INT2_FA", 20222)
        Sounds.Add("SOUND_INT2_FB", 20223)
        Sounds.Add("SOUND_INT2_FC", 20224)
        Sounds.Add("SOUND_INT2_FD", 20225)
        Sounds.Add("SOUND_INT2_GA", 20226)
        Sounds.Add("SOUND_INT2_GB", 20227)
        Sounds.Add("SOUND_INT2_GC", 20228)
        Sounds.Add("SOUND_INT2_GD", 20229)
        Sounds.Add("SOUND_INT2_GE", 20230)
        Sounds.Add("SOUND_INT2_GF", 20231)
        Sounds.Add("SOUND_INT2_HA", 20232)
        Sounds.Add("SOUND_INT2_HB", 20233)
        Sounds.Add("SOUND_INT2_IA", 20234)
        Sounds.Add("SOUND_INT2_IB", 20235)
        Sounds.Add("SOUND_INT2_JA", 20236)
        Sounds.Add("SOUND_INT2_JB", 20237)
        Sounds.Add("SOUND_INT2_KA", 20238)
        Sounds.Add("SOUND_INT2_KB", 20239)
        Sounds.Add("SOUND_INT2_LA", 20240)
        Sounds.Add("SOUND_INT2_LB", 20241)
        Sounds.Add("SOUND_INT2_LC", 20242)
        Sounds.Add("SOUND_INT2_MA", 20243)
        Sounds.Add("SOUND_INT2_MB", 20244)
        Sounds.Add("SOUND_INT2_NA", 20245)
        Sounds.Add("SOUND_INT2_NB", 20246)
        Sounds.Add("SOUND_INT2_NC", 20247)
        Sounds.Add("SOUND_INT2_ND", 20248)
        Sounds.Add("SOUND_JIZX_AA", 20400)
        Sounds.Add("SOUND_JIZX_AB", 20401)
        Sounds.Add("SOUND_JIZX_AC", 20402)
        Sounds.Add("SOUND_JIZX_AD", 20403)
        Sounds.Add("SOUND_JIZX_AE", 20404)
        Sounds.Add("SOUND_JIZX_AF", 20405)
        Sounds.Add("SOUND_JIZX_AG", 20406)
        Sounds.Add("SOUND_JIZX_AH", 20407)
        Sounds.Add("SOUND_JIZX_AI", 20408)
        Sounds.Add("SOUND_JIZX_AJ", 20409)
        Sounds.Add("SOUND_JIZX_AK", 20410)
        Sounds.Add("SOUND_JIZX_AL", 20411)
        Sounds.Add("SOUND_JIZX_AM", 20412)
        Sounds.Add("SOUND_JIZX_AN", 20413)
        Sounds.Add("SOUND_JIZX_AO", 20414)
        Sounds.Add("SOUND_JIZX_AP", 20415)
        Sounds.Add("SOUND_JIZX_BA", 20416)
        Sounds.Add("SOUND_JIZX_BB", 20417)
        Sounds.Add("SOUND_JIZX_BC", 20418)
        Sounds.Add("SOUND_JIZX_BD", 20419)
        Sounds.Add("SOUND_JIZX_BE", 20420)
        Sounds.Add("SOUND_JIZX_BF", 20421)
        Sounds.Add("SOUND_JIZX_BG", 20422)
        Sounds.Add("SOUND_JIZX_BH", 20423)
        Sounds.Add("SOUND_JIZX_BI", 20424)
        Sounds.Add("SOUND_CAR_PHONE_RING", 20600)
        Sounds.Add("SOUND_CLOTHES_DRESSING_WARDROBE", 20800)
        Sounds.Add("SOUND_DOORBELL", 20801)
        Sounds.Add("SOUND_DRESSING", 20802)
        Sounds.Add("SOUND_GIMP_SUIT", 20803)
        Sounds.Add("SOUND_PED_MOBRING", 20804)
        Sounds.Add("SOUND__KEYPAD_BEEP", 21000)
        Sounds.Add("SOUND__KEYPAD_FAIL", 21001)
        Sounds.Add("SOUND__KEYPAD_PASS", 21002)
        Sounds.Add("SOUND_KUNG_1", 21200)
        Sounds.Add("SOUND_KUNG_2", 21201)
        Sounds.Add("SOUND_KUNG_3", 21202)
        Sounds.Add("SOUND_KUNG_4", 21203)
        Sounds.Add("SOUND_KUNG_5", 21204)
        Sounds.Add("SOUND_KUNG_6", 21205)
        Sounds.Add("SOUND_KUNG_7", 21206)
        Sounds.Add("SOUND_KUNG_8", 21207)
        Sounds.Add("SOUND_BEXIT1", 21400)
        Sounds.Add("SOUND_BEXIT2", 21401)
        Sounds.Add("SOUND_DNCEF1", 21402)
        Sounds.Add("SOUND_DNCEF2", 21403)
        Sounds.Add("SOUND_DOFFE1", 21404)
        Sounds.Add("SOUND_DOFFE1N", 21405)
        Sounds.Add("SOUND_DOFFE1Y", 21406)
        Sounds.Add("SOUND_DOFFE2", 21407)
        Sounds.Add("SOUND_DOFFE2N", 21408)
        Sounds.Add("SOUND_DOFFE2Y", 21409)
        Sounds.Add("SOUND_DOFFE3", 21410)
        Sounds.Add("SOUND_DOFFE3N", 21411)
        Sounds.Add("SOUND_DOFFE3Y", 21412)
        Sounds.Add("SOUND_DOFFE4", 21413)
        Sounds.Add("SOUND_DOFFE4N", 21414)
        Sounds.Add("SOUND_DOFFE4Y", 21415)
        Sounds.Add("SOUND_DSTAR1", 21416)
        Sounds.Add("SOUND_DSTAR2", 21417)
        Sounds.Add("SOUND_GEXIT1", 21418)
        Sounds.Add("SOUND_GEXIT2", 21419)
        Sounds.Add("SOUND_HI1", 21420)
        Sounds.Add("SOUND_HI1N", 21421)
        Sounds.Add("SOUND_HI1Y", 21422)
        Sounds.Add("SOUND_HI2", 21423)
        Sounds.Add("SOUND_HI2N", 21424)
        Sounds.Add("SOUND_HI2Y", 21425)
        Sounds.Add("SOUND_HI3", 21426)
        Sounds.Add("SOUND_HI3N", 21427)
        Sounds.Add("SOUND_HI3Y", 21428)
        Sounds.Add("SOUND_HI4", 21429)
        Sounds.Add("SOUND_HI4N", 21430)
        Sounds.Add("SOUND_HI4Y", 21431)
        Sounds.Add("SOUND_HI5", 21432)
        Sounds.Add("SOUND_HI5N", 21433)
        Sounds.Add("SOUND_HI5Y", 21434)
        Sounds.Add("SOUND_HI6", 21435)
        Sounds.Add("SOUND_HI6N", 21436)
        Sounds.Add("SOUND_HI6Y", 21437)
        Sounds.Add("SOUND_HI7N", 21438)
        Sounds.Add("SOUND_HI7Y", 21439)
        Sounds.Add("SOUND_HI8N", 21440)
        Sounds.Add("SOUND_HI8Y", 21441)
        Sounds.Add("SOUND_VLATE", 21442)
        Sounds.Add("SOUND_VOFFE1", 21443)
        Sounds.Add("SOUND_VOFFE1N", 21444)
        Sounds.Add("SOUND_VOFFE1Y", 21445)
        Sounds.Add("SOUND_VOFFE2", 21446)
        Sounds.Add("SOUND_VOFFE2N", 21447)
        Sounds.Add("SOUND_VOFFE2Y", 21448)
        Sounds.Add("SOUND_VSURE1", 21449)
        Sounds.Add("SOUND_VSURE1N", 21450)
        Sounds.Add("SOUND_VSURE1Y", 21451)
        Sounds.Add("SOUND_VSURE2", 21452)
        Sounds.Add("SOUND_VSURE2N", 21453)
        Sounds.Add("SOUND_VSURE2Y", 21454)
        Sounds.Add("SOUND_VYES1", 21455)
        Sounds.Add("SOUND_VYES2", 21456)
        Sounds.Add("SOUND_LOC1_BA", 21600)
        Sounds.Add("SOUND_LOC1_BB", 21601)
        Sounds.Add("SOUND_LOC1_BC", 21602)
        Sounds.Add("SOUND_LOC1_BD", 21603)
        Sounds.Add("SOUND_LOC1_BE", 21604)
        Sounds.Add("SOUND_LOC1_BF", 21605)
        Sounds.Add("SOUND_LOC1_BG", 21606)
        Sounds.Add("SOUND_LOC1_BH", 21607)
        Sounds.Add("SOUND_LOC1_BJ", 21608)
        Sounds.Add("SOUND_LOC1_BK", 21609)
        Sounds.Add("SOUND_LOC1_BL", 21610)
        Sounds.Add("SOUND_LOC1_BM", 21611)
        Sounds.Add("SOUND_LOC1_BN", 21612)
        Sounds.Add("SOUND_LOC1_BO", 21613)
        Sounds.Add("SOUND_LOC1_BP", 21614)
        Sounds.Add("SOUND_LOC1_BQ", 21615)
        Sounds.Add("SOUND_LOC1_BR", 21616)
        Sounds.Add("SOUND_LOC1_BS", 21617)
        Sounds.Add("SOUND_LOC1_BT", 21618)
        Sounds.Add("SOUND_LOC1_BU", 21619)
        Sounds.Add("SOUND_LOC1_BV", 21620)
        Sounds.Add("SOUND_LOC1_CA", 21621)
        Sounds.Add("SOUND_LOC1_CB", 21622)
        Sounds.Add("SOUND_LOC1_CC", 21623)
        Sounds.Add("SOUND_LOC1_CD", 21624)
        Sounds.Add("SOUND_LOC1_CE", 21625)
        Sounds.Add("SOUND_LOC1_CF", 21626)
        Sounds.Add("SOUND_LOC1_CG", 21627)
        Sounds.Add("SOUND_LOC1_CH", 21628)
        Sounds.Add("SOUND_LOC1_CJ", 21629)
        Sounds.Add("SOUND_LOC1_CK", 21630)
        Sounds.Add("SOUND_LOC1_CL", 21631)
        Sounds.Add("SOUND_LOC1_CM", 21632)
        Sounds.Add("SOUND_LOC1_CN", 21633)
        Sounds.Add("SOUND_LOC1_CO", 21634)
        Sounds.Add("SOUND_LOC1_CP", 21635)
        Sounds.Add("SOUND_LOC1_CQ", 21636)
        Sounds.Add("SOUND_LOC1_CR", 21637)
        Sounds.Add("SOUND_LOC1_CS", 21638)
        Sounds.Add("SOUND_LOC1_CT", 21639)
        Sounds.Add("SOUND_LOC1_CU", 21640)
        Sounds.Add("SOUND_LOC1_CV", 21641)
        Sounds.Add("SOUND_LOC1_YA", 21642)
        Sounds.Add("SOUND_LOC1_YB", 21643)
        Sounds.Add("SOUND_LOC1_YC", 21644)
        Sounds.Add("SOUND_LOC1_YD", 21645)
        Sounds.Add("SOUND_LOC1_YE", 21646)
        Sounds.Add("SOUND_LOC1_YG", 21647)
        Sounds.Add("SOUND_LOC1_YH", 21648)
        Sounds.Add("SOUND_LOC1_YJ", 21649)
        Sounds.Add("SOUND_LOC1_YK", 21650)
        Sounds.Add("SOUND_LOC1_YO", 21651)
        Sounds.Add("SOUND_LOC1_YP", 21652)
        Sounds.Add("SOUND_LOC1_YQ", 21653)
        Sounds.Add("SOUND_LOC1_YR", 21654)
        Sounds.Add("SOUND_LOC1_YS", 21655)
        Sounds.Add("SOUND_LOC1_YT", 21656)
        Sounds.Add("SOUND_LOC1_ZE", 21657)
        Sounds.Add("SOUND_LOC1_ZF", 21658)
        Sounds.Add("SOUND_LOC1_ZG", 21659)
        Sounds.Add("SOUND_LOC1_ZH", 21660)
        Sounds.Add("SOUND_LOC1_ZN", 21661)
        Sounds.Add("SOUND_LOC1_ZO", 21662)
        Sounds.Add("SOUND_LOC1_ZP", 21663)
        Sounds.Add("SOUND_LOC1_ZQ", 21664)
        Sounds.Add("SOUND_LOC1_ZT", 21665)
        Sounds.Add("SOUND_LOC1_ZU", 21666)
        Sounds.Add("SOUND_LOC2_AA", 21800)
        Sounds.Add("SOUND_LOC2_AB", 21801)
        Sounds.Add("SOUND_LOC2_AC", 21802)
        Sounds.Add("SOUND_LOC2_BA", 21803)
        Sounds.Add("SOUND_LOC2_BB", 21804)
        Sounds.Add("SOUND_LOC2_CA", 21805)
        Sounds.Add("SOUND_LOC2_CB", 21806)
        Sounds.Add("SOUND_LOC2_CC", 21807)
        Sounds.Add("SOUND_LOC2_DC", 21808)
        Sounds.Add("SOUND_LOC3_AA", 22000)
        Sounds.Add("SOUND_LOC3_BA", 22001)
        Sounds.Add("SOUND_LOC3_BB", 22002)
        Sounds.Add("SOUND_LOC3_BC", 22003)
        Sounds.Add("SOUND_LOC3_BD", 22004)
        Sounds.Add("SOUND_LOC3_BE", 22005)
        Sounds.Add("SOUND_LOC3_CA", 22006)
        Sounds.Add("SOUND_LOC3_CB", 22007)
        Sounds.Add("SOUND_LOC3_DA", 22008)
        Sounds.Add("SOUND_LOC3_DB", 22009)
        Sounds.Add("SOUND_LOC3_DC", 22010)
        Sounds.Add("SOUND_LOC3_DD", 22011)
        Sounds.Add("SOUND_LOC3_EA", 22012)
        Sounds.Add("SOUND_LOC3_EB", 22013)
        Sounds.Add("SOUND_LOC3_EC", 22014)
        Sounds.Add("SOUND_LOC3_ED", 22015)
        Sounds.Add("SOUND_LOC3_EE", 22016)
        Sounds.Add("SOUND_LOC3_EF", 22017)
        Sounds.Add("SOUND_LOC3_EG", 22018)
        Sounds.Add("SOUND_LOC3_EH", 22019)
        Sounds.Add("SOUND_LOC3_EJ", 22020)
        Sounds.Add("SOUND_LOC3_EK", 22021)
        Sounds.Add("SOUND_LOC3_EL", 22022)
        Sounds.Add("SOUND_LOC3_EM", 22023)
        Sounds.Add("SOUND_LOC3_EN", 22024)
        Sounds.Add("SOUND_LOC3_EO", 22025)
        Sounds.Add("SOUND_LOC3_FA", 22026)
        Sounds.Add("SOUND_LOC3_FB", 22027)
        Sounds.Add("SOUND_LOC3_FC", 22028)
        Sounds.Add("SOUND_LOC3_FD", 22029)
        Sounds.Add("SOUND_LOC3_FE", 22030)
        Sounds.Add("SOUND_LOC3_FF", 22031)
        Sounds.Add("SOUND_LOC3_GA", 22032)
        Sounds.Add("SOUND_LOC3_GB", 22033)
        Sounds.Add("SOUND_LOC3_GC", 22034)
        Sounds.Add("SOUND_LOC3_GD", 22035)
        Sounds.Add("SOUND_LOC3_HA", 22036)
        Sounds.Add("SOUND_LOC3_HB", 22037)
        Sounds.Add("SOUND_LOC3_JA", 22038)
        Sounds.Add("SOUND_LOC3_JB", 22039)
        Sounds.Add("SOUND_LOC4_AA", 22200)
        Sounds.Add("SOUND_LOC4_AB", 22201)
        Sounds.Add("SOUND_LOC4_AC", 22202)
        Sounds.Add("SOUND_LOC4_AD", 22203)
        Sounds.Add("SOUND_LOC4_AE", 22204)
        Sounds.Add("SOUND_LOC4_AF", 22205)
        Sounds.Add("SOUND_LOC4_AG", 22206)
        Sounds.Add("SOUND_LOC4_AH", 22207)
        Sounds.Add("SOUND_LOC4_BA", 22208)
        Sounds.Add("SOUND_LOC4_BB", 22209)
        Sounds.Add("SOUND_LOC4_BC", 22210)
        Sounds.Add("SOUND_LOC4_BD", 22211)
        Sounds.Add("SOUND_LOC4_BE", 22212)
        Sounds.Add("SOUND_LOC4_BF", 22213)
        Sounds.Add("SOUND_LOC4_BG", 22214)
        Sounds.Add("SOUND_LOC4_CA", 22215)
        Sounds.Add("SOUND_LOC4_CB", 22216)
        Sounds.Add("SOUND_LOC4_CC", 22217)
        Sounds.Add("SOUND_LOCX_AA", 22400)
        Sounds.Add("SOUND_LOCX_AB", 22401)
        Sounds.Add("SOUND_LOCX_AC", 22402)
        Sounds.Add("SOUND_LOCX_AD", 22403)
        Sounds.Add("SOUND_LOCX_AE", 22404)
        Sounds.Add("SOUND_LOCX_AF", 22405)
        Sounds.Add("SOUND_LOCX_AG", 22406)
        Sounds.Add("SOUND_MOURNERS", 22600)
        Sounds.Add("SOUND_LOWR_AA", 22800)
        Sounds.Add("SOUND_LOWR_AB", 22801)
        Sounds.Add("SOUND_LOWR_AD", 22802)
        Sounds.Add("SOUND_LOWR_AE", 22803)
        Sounds.Add("SOUND_LOWR_AG", 22804)
        Sounds.Add("SOUND_LOWR_AH", 22805)
        Sounds.Add("SOUND_LOWR_AK", 22806)
        Sounds.Add("SOUND_LOWR_AL", 22807)
        Sounds.Add("SOUND_LOWR_BA", 22808)
        Sounds.Add("SOUND_LOWR_BB", 22809)
        Sounds.Add("SOUND_LOWR_BC", 22810)
        Sounds.Add("SOUND_LOWR_CA", 22811)
        Sounds.Add("SOUND_LOWR_CB", 22812)
        Sounds.Add("SOUND_LOWR_DA", 22813)
        Sounds.Add("SOUND_LOWR_DB", 22814)
        Sounds.Add("SOUND_LOWR_EA", 22815)
        Sounds.Add("SOUND_LOWR_EB", 22816)
        Sounds.Add("SOUND_LOWR_EC", 22817)
        Sounds.Add("SOUND_LOWR_ED", 22818)
        Sounds.Add("SOUND_LOWR_EE", 22819)
        Sounds.Add("SOUND_LOWR_EF", 22820)
        Sounds.Add("SOUND_LOWR_FA", 22821)
        Sounds.Add("SOUND_LOWR_FB", 22822)
        Sounds.Add("SOUND_LOWR_FC", 22823)
        Sounds.Add("SOUND_LOWR_FD", 22824)
        Sounds.Add("SOUND_LOWR_FE", 22825)
        Sounds.Add("SOUND_LOWR_FF", 22826)
        Sounds.Add("SOUND_LOWR_GA", 22827)
        Sounds.Add("SOUND_LOWR_GB", 22828)
        Sounds.Add("SOUND_LOWR_HA", 22829)
        Sounds.Add("SOUND_LOWR_HB", 22830)
        Sounds.Add("SOUND_LOWR_HC", 22831)
        Sounds.Add("SOUND_LOWR_HD", 22832)
        Sounds.Add("SOUND_LOWR_KA", 22833)
        Sounds.Add("SOUND_LOWR_KB", 22834)
        Sounds.Add("SOUND_LOWR_KC", 22835)
        Sounds.Add("SOUND_LOWR_KD", 22836)
        Sounds.Add("SOUND_LOWR_KE", 22837)
        Sounds.Add("SOUND_LOWR_KF", 22838)
        Sounds.Add("SOUND_LOWR_KG", 22839)
        Sounds.Add("SOUND_MOBRING", 23000)
        Sounds.Add("SOUND_MACX_AA", 23200)
        Sounds.Add("SOUND_MACX_AB", 23201)
        Sounds.Add("SOUND_MACX_AC", 23202)
        Sounds.Add("SOUND_MACX_AD", 23203)
        Sounds.Add("SOUND_MACX_AE", 23204)
        Sounds.Add("SOUND_MACX_AF", 23205)
        Sounds.Add("SOUND_MACX_AG", 23206)
        Sounds.Add("SOUND_MACX_AH", 23207)
        Sounds.Add("SOUND_MACX_AI", 23208)
        Sounds.Add("SOUND_MACX_AJ", 23209)
        Sounds.Add("SOUND_CRATE_LANDING", 23400)
        Sounds.Add("SOUND_VIDEO_GAME_LOOP", 23600)
        Sounds.Add("SOUND_MAN1_AA", 23800)
        Sounds.Add("SOUND_MAN1_AB", 23801)
        Sounds.Add("SOUND_MAN1_AC", 23802)
        Sounds.Add("SOUND_MAN1_AD", 23803)
        Sounds.Add("SOUND_MAN1_BA", 23804)
        Sounds.Add("SOUND_MAN1_BB", 23805)
        Sounds.Add("SOUND_MAN1_BC", 23806)
        Sounds.Add("SOUND_MAN1_BD", 23807)
        Sounds.Add("SOUND_MAN1_CA", 23808)
        Sounds.Add("SOUND_MAN1_CB", 23809)
        Sounds.Add("SOUND_MAN1_CC", 23810)
        Sounds.Add("SOUND_MAN1_DA", 23811)
        Sounds.Add("SOUND_MAN1_DB", 23812)
        Sounds.Add("SOUND_MAN1_DC", 23813)
        Sounds.Add("SOUND_MAN1_DD", 23814)
        Sounds.Add("SOUND_MAN1_DE", 23815)
        Sounds.Add("SOUND_MAN1_DF", 23816)
        Sounds.Add("SOUND_MAN1_DG", 23817)
        Sounds.Add("SOUND_MAN1_DH", 23818)
        Sounds.Add("SOUND_MAN1_DJ", 23819)
        Sounds.Add("SOUND_MAN1_EA", 23820)
        Sounds.Add("SOUND_MAN1_EB", 23821)
        Sounds.Add("SOUND_MAN1_FA", 23822)
        Sounds.Add("SOUND_MAN1_FB", 23823)
        Sounds.Add("SOUND_MAN1_FC", 23824)
        Sounds.Add("SOUND_MAN1_FD", 23825)
        Sounds.Add("SOUND_MAN1_FE", 23826)
        Sounds.Add("SOUND_MAN1_FF", 23827)
        Sounds.Add("SOUND_MAN1_FG", 23828)
        Sounds.Add("SOUND_MAN1_FH", 23829)
        Sounds.Add("SOUND_MAN1_FJ", 23830)
        Sounds.Add("SOUND_MAN1_FK", 23831)
        Sounds.Add("SOUND_MAN1_FL", 23832)
        Sounds.Add("SOUND_MAN1_FM", 23833)
        Sounds.Add("SOUND_MAN1_FN", 23834)
        Sounds.Add("SOUND_MAN1_FO", 23835)
        Sounds.Add("SOUND_MAN2_AA", 24000)
        Sounds.Add("SOUND_MAN2_AB", 24001)
        Sounds.Add("SOUND_MAN2_AC", 24002)
        Sounds.Add("SOUND_MAN2_AD", 24003)
        Sounds.Add("SOUND_MAN2_BA", 24004)
        Sounds.Add("SOUND_MAN2_BB", 24005)
        Sounds.Add("SOUND_MAN2_BC", 24006)
        Sounds.Add("SOUND_MAN2_BD", 24007)
        Sounds.Add("SOUND_MAN2_BE", 24008)
        Sounds.Add("SOUND_MAN2_BF", 24009)
        Sounds.Add("SOUND_MAN2_BG", 24010)
        Sounds.Add("SOUND_MAN2_BH", 24011)
        Sounds.Add("SOUND_MAN2_BJ", 24012)
        Sounds.Add("SOUND_MAN2_BK", 24013)
        Sounds.Add("SOUND_MAN2_BL", 24014)
        Sounds.Add("SOUND_MAN2_BM", 24015)
        Sounds.Add("SOUND_MAN2_BN", 24016)
        Sounds.Add("SOUND_MAN2_CA", 24017)
        Sounds.Add("SOUND_MAN2_DA", 24018)
        Sounds.Add("SOUND_MAN2_DB", 24019)
        Sounds.Add("SOUND_MAN2_DC", 24020)
        Sounds.Add("SOUND_MAN2_DD", 24021)
        Sounds.Add("SOUND_MAN2_EA", 24022)
        Sounds.Add("SOUND_MAN2_EB", 24023)
        Sounds.Add("SOUND_MAN2_EC", 24024)
        Sounds.Add("SOUND_MAN2_EE", 24025)
        Sounds.Add("SOUND_MAN2_EF", 24026)
        Sounds.Add("SOUND_MAN2_EG", 24027)
        Sounds.Add("SOUND_MAN2_EH", 24028)
        Sounds.Add("SOUND_MAN2_EJ", 24029)
        Sounds.Add("SOUND_MAN2_FA", 24030)
        Sounds.Add("SOUND_MAN2_FB", 24031)
        Sounds.Add("SOUND_MAN2_FC", 24032)
        Sounds.Add("SOUND_MAN2_FD", 24033)
        Sounds.Add("SOUND_MAN2_FE", 24034)
        Sounds.Add("SOUND_MAN2_FF", 24035)
        Sounds.Add("SOUND_MAN2_FG", 24036)
        Sounds.Add("SOUND_MAN2_FK", 24037)
        Sounds.Add("SOUND_MAN2_FL", 24038)
        Sounds.Add("SOUND_MAN2_GA", 24039)
        Sounds.Add("SOUND_MAN2_GB", 24040)
        Sounds.Add("SOUND_MAN2_GC", 24041)
        Sounds.Add("SOUND_MAN2_GD", 24042)
        Sounds.Add("SOUND_MAN2_GE", 24043)
        Sounds.Add("SOUND_MAN2_GF", 24044)
        Sounds.Add("SOUND_MAN2_GG", 24045)
        Sounds.Add("SOUND_MAN2_GH", 24046)
        Sounds.Add("SOUND_MAN2_HA", 24047)
        Sounds.Add("SOUND_MAN2_HB", 24048)
        Sounds.Add("SOUND_MAN2_HC", 24049)
        Sounds.Add("SOUND_MAN2_HD", 24050)
        Sounds.Add("SOUND_MAN2_HE", 24051)
        Sounds.Add("SOUND_MAN2_HF", 24052)
        Sounds.Add("SOUND_MAN2_JA", 24053)
        Sounds.Add("SOUND_MAN2_JC", 24054)
        Sounds.Add("SOUND_MAN2_JD", 24055)
        Sounds.Add("SOUND_MAN2_JE", 24056)
        Sounds.Add("SOUND_MAN2_JF", 24057)
        Sounds.Add("SOUND_MAN2_KA", 24058)
        Sounds.Add("SOUND_MAN2_KB", 24059)
        Sounds.Add("SOUND_MAN2_KC", 24060)
        Sounds.Add("SOUND_MAN2_KD", 24061)
        Sounds.Add("SOUND_MAN2_KE", 24062)
        Sounds.Add("SOUND_MAN2_KF", 24063)
        Sounds.Add("SOUND_MAN3_AA", 24200)
        Sounds.Add("SOUND_MAN3_AB", 24201)
        Sounds.Add("SOUND_MAN3_AC", 24202)
        Sounds.Add("SOUND_MAN3_AD", 24203)
        Sounds.Add("SOUND_MAN3_BA", 24204)
        Sounds.Add("SOUND_MAN3_BB", 24205)
        Sounds.Add("SOUND_MAN3_BC", 24206)
        Sounds.Add("SOUND_MAN3_BD", 24207)
        Sounds.Add("SOUND_MAN3_BE", 24208)
        Sounds.Add("SOUND_MAN3_CA", 24209)
        Sounds.Add("SOUND_MAN3_CB", 24210)
        Sounds.Add("SOUND_MAN3_CC", 24211)
        Sounds.Add("SOUND_MAN3_CD", 24212)
        Sounds.Add("SOUND_MAN3_CE", 24213)
        Sounds.Add("SOUND_MAN3_CF", 24214)
        Sounds.Add("SOUND_MAN3_CG", 24215)
        Sounds.Add("SOUND_MAN3_CH", 24216)
        Sounds.Add("SOUND_MAN3_CI", 24217)
        Sounds.Add("SOUND_MAN3_CJ", 24218)
        Sounds.Add("SOUND_MAN3_CK", 24219)
        Sounds.Add("SOUND_MAN5_AA", 24400)
        Sounds.Add("SOUND_MAN5_AB", 24401)
        Sounds.Add("SOUND_MAN5_AC", 24402)
        Sounds.Add("SOUND_MAN5_BA", 24403)
        Sounds.Add("SOUND_MAN5_BB", 24404)
        Sounds.Add("SOUND_MAN5_BC", 24405)
        Sounds.Add("SOUND_MAN5_BD", 24406)
        Sounds.Add("SOUND_MAN5_BE", 24407)
        Sounds.Add("SOUND_MAN5_BF", 24408)
        Sounds.Add("SOUND_MAN5_BG", 24409)
        Sounds.Add("SOUND_MAN5_BH", 24410)
        Sounds.Add("SOUND_MAN5_BJ", 24411)
        Sounds.Add("SOUND_MAN5_BK", 24412)
        Sounds.Add("SOUND_MAN5_BL", 24413)
        Sounds.Add("SOUND_MAN5_BM", 24414)
        Sounds.Add("SOUND_MAN5_BN", 24415)
        Sounds.Add("SOUND_MAN5_BO", 24416)
        Sounds.Add("SOUND_MAN5_CA", 24417)
        Sounds.Add("SOUND_MAN5_CB", 24418)
        Sounds.Add("SOUND_MAN5_DA", 24419)
        Sounds.Add("SOUND_MAN5_DB", 24420)
        Sounds.Add("SOUND_MAN5_DC", 24421)
        Sounds.Add("SOUND_MAN5_DD", 24422)
        Sounds.Add("SOUND_MAN5_EA", 24423)
        Sounds.Add("SOUND_MAN5_EB", 24424)
        Sounds.Add("SOUND_MAN5_FA", 24425)
        Sounds.Add("SOUND_MAN5_FB", 24426)
        Sounds.Add("SOUND_MAN5_FC", 24427)
        Sounds.Add("SOUND_MAN5_FD", 24428)
        Sounds.Add("SOUND_MAN5_FE", 24429)
        Sounds.Add("SOUND_MAN5_FF", 24430)
        Sounds.Add("SOUND_MAN5_FG", 24431)
        Sounds.Add("SOUND_MAN5_FH", 24432)
        Sounds.Add("SOUND_MAN5_FJ", 24433)
        Sounds.Add("SOUND_LOCK_CAR_DOORS", 24600)
        Sounds.Add("SOUND_MBARB1A", 24800)
        Sounds.Add("SOUND_MBARB1B", 24801)
        Sounds.Add("SOUND_MBARB1C", 24802)
        Sounds.Add("SOUND_MBARB1D", 24803)
        Sounds.Add("SOUND_MBARB1E", 24804)
        Sounds.Add("SOUND_MBARB1F", 24805)
        Sounds.Add("SOUND_MBARB1G", 24806)
        Sounds.Add("SOUND_MBARB1H", 24807)
        Sounds.Add("SOUND_MBARB1J", 24808)
        Sounds.Add("SOUND_MBARB1K", 24809)
        Sounds.Add("SOUND_MBARB1L", 24810)
        Sounds.Add("SOUND_MBARB1M", 24811)
        Sounds.Add("SOUND_MBARB1N", 24812)
        Sounds.Add("SOUND_MBARB1O", 24813)
        Sounds.Add("SOUND_MBARB1P", 24814)
        Sounds.Add("SOUND_MBARB1Q", 24815)
        Sounds.Add("SOUND_MBARB1R", 24816)
        Sounds.Add("SOUND_MBARB2A", 24817)
        Sounds.Add("SOUND_MBARB2D", 24818)
        Sounds.Add("SOUND_MBARB3A", 24819)
        Sounds.Add("SOUND_MBARB3B", 24820)
        Sounds.Add("SOUND_MBARB4A", 24821)
        Sounds.Add("SOUND_MBARB4D", 24822)
        Sounds.Add("SOUND_MBARB5A", 24823)
        Sounds.Add("SOUND_MBARB5D", 24824)
        Sounds.Add("SOUND_MBARB6A", 24825)
        Sounds.Add("SOUND_MBARB6B", 24826)
        Sounds.Add("SOUND_MBARB7D", 24827)
        Sounds.Add("SOUND_MBARB8D", 24828)
        Sounds.Add("SOUND_MBARB9D", 24829)
        Sounds.Add("SOUND_MCAT01A", 25000)
        Sounds.Add("SOUND_MCAT01B", 25001)
        Sounds.Add("SOUND_MCAT01C", 25002)
        Sounds.Add("SOUND_MCAT01D", 25003)
        Sounds.Add("SOUND_MCAT01E", 25004)
        Sounds.Add("SOUND_MCAT01F", 25005)
        Sounds.Add("SOUND_MCAT01G", 25006)
        Sounds.Add("SOUND_MCAT01H", 25007)
        Sounds.Add("SOUND_MCAT02A", 25008)
        Sounds.Add("SOUND_MCAT02B", 25009)
        Sounds.Add("SOUND_MCAT02C", 25010)
        Sounds.Add("SOUND_MCAT02D", 25011)
        Sounds.Add("SOUND_MCAT02E", 25012)
        Sounds.Add("SOUND_MCAT02F", 25013)
        Sounds.Add("SOUND_MCAT02G", 25014)
        Sounds.Add("SOUND_MCAT02H", 25015)
        Sounds.Add("SOUND_MCAT03A", 25016)
        Sounds.Add("SOUND_MCAT03B", 25017)
        Sounds.Add("SOUND_MCAT04A", 25018)
        Sounds.Add("SOUND_MCAT04B", 25019)
        Sounds.Add("SOUND_MCAT04C", 25020)
        Sounds.Add("SOUND_MCAT04D", 25021)
        Sounds.Add("SOUND_MCAT04E", 25022)
        Sounds.Add("SOUND_MCAT04F", 25023)
        Sounds.Add("SOUND_MCAT05A", 25024)
        Sounds.Add("SOUND_MCAT05B", 25025)
        Sounds.Add("SOUND_MCAT05C", 25026)
        Sounds.Add("SOUND_MCAT05D", 25027)
        Sounds.Add("SOUND_MCAT05E", 25028)
        Sounds.Add("SOUND_MCAT05F", 25029)
        Sounds.Add("SOUND_MCAT05G", 25030)
        Sounds.Add("SOUND_MCAT06A", 25031)
        Sounds.Add("SOUND_MCAT06B", 25032)
        Sounds.Add("SOUND_MCAT06C", 25033)
        Sounds.Add("SOUND_MCAT06D", 25034)
        Sounds.Add("SOUND_MCAT06E", 25035)
        Sounds.Add("SOUND_MCAT06F", 25036)
        Sounds.Add("SOUND_MCAT06G", 25037)
        Sounds.Add("SOUND_MCAT06H", 25038)
        Sounds.Add("SOUND_MCAT06J", 25039)
        Sounds.Add("SOUND_MCAT06K", 25040)
        Sounds.Add("SOUND_MCAT07A", 25041)
        Sounds.Add("SOUND_MCAT07B", 25042)
        Sounds.Add("SOUND_MCAT07C", 25043)
        Sounds.Add("SOUND_MCAT07D", 25044)
        Sounds.Add("SOUND_MCAT07E", 25045)
        Sounds.Add("SOUND_MCAT07F", 25046)
        Sounds.Add("SOUND_MCAT07G", 25047)
        Sounds.Add("SOUND_MCES01A", 25200)
        Sounds.Add("SOUND_MCES01B", 25201)
        Sounds.Add("SOUND_MCES01C", 25202)
        Sounds.Add("SOUND_MCES01D", 25203)
        Sounds.Add("SOUND_MCES01E", 25204)
        Sounds.Add("SOUND_MCES01F", 25205)
        Sounds.Add("SOUND_MCES01G", 25206)
        Sounds.Add("SOUND_MCES01H", 25207)
        Sounds.Add("SOUND_MCES01K", 25208)
        Sounds.Add("SOUND_MCES01L", 25209)
        Sounds.Add("SOUND_MCES01M", 25210)
        Sounds.Add("SOUND_MCES01N", 25211)
        Sounds.Add("SOUND_MCES01O", 25212)
        Sounds.Add("SOUND_MCES01P", 25213)
        Sounds.Add("SOUND_MCES02A", 25214)
        Sounds.Add("SOUND_MCES02B", 25215)
        Sounds.Add("SOUND_MCES02C", 25216)
        Sounds.Add("SOUND_MCES02D", 25217)
        Sounds.Add("SOUND_MCES02E", 25218)
        Sounds.Add("SOUND_MCES02F", 25219)
        Sounds.Add("SOUND_MCES02G", 25220)
        Sounds.Add("SOUND_MCES02H", 25221)
        Sounds.Add("SOUND_MCES02K", 25222)
        Sounds.Add("SOUND_MCES02L", 25223)
        Sounds.Add("SOUND_MCES02M", 25224)
        Sounds.Add("SOUND_MCES02N", 25225)
        Sounds.Add("SOUND_MCES02O", 25226)
        Sounds.Add("SOUND_MCES03A", 25227)
        Sounds.Add("SOUND_MCES03B", 25228)
        Sounds.Add("SOUND_MCES03C", 25229)
        Sounds.Add("SOUND_MCES03D", 25230)
        Sounds.Add("SOUND_MCES03E", 25231)
        Sounds.Add("SOUND_MCES03F", 25232)
        Sounds.Add("SOUND_MCES03G", 25233)
        Sounds.Add("SOUND_MCES03H", 25234)
        Sounds.Add("SOUND_MCES03J", 25235)
        Sounds.Add("SOUND_MCES03K", 25236)
        Sounds.Add("SOUND_MCES03L", 25237)
        Sounds.Add("SOUND_MCES03M", 25238)
        Sounds.Add("SOUND_MCES03N", 25239)
        Sounds.Add("SOUND_MCES04A", 25240)
        Sounds.Add("SOUND_MCES04B", 25241)
        Sounds.Add("SOUND_MCES04C", 25242)
        Sounds.Add("SOUND_MCES04D", 25243)
        Sounds.Add("SOUND_MCES04E", 25244)
        Sounds.Add("SOUND_MCES04F", 25245)
        Sounds.Add("SOUND_MCES04G", 25246)
        Sounds.Add("SOUND_MCES04H", 25247)
        Sounds.Add("SOUND_MCES04J", 25248)
        Sounds.Add("SOUND_MCES04L", 25249)
        Sounds.Add("SOUND_MCES04M", 25250)
        Sounds.Add("SOUND_MCES04N", 25251)
        Sounds.Add("SOUND_MCES04O", 25252)
        Sounds.Add("SOUND_MCES04P", 25253)
        Sounds.Add("SOUND_MCES04Q", 25254)
        Sounds.Add("SOUND_MCES04R", 25255)
        Sounds.Add("SOUND_MCES05A", 25256)
        Sounds.Add("SOUND_MCES05B", 25257)
        Sounds.Add("SOUND_MCES05C", 25258)
        Sounds.Add("SOUND_MCES05D", 25259)
        Sounds.Add("SOUND_MCES05E", 25260)
        Sounds.Add("SOUND_MCES05F", 25261)
        Sounds.Add("SOUND_MCES05G", 25262)
        Sounds.Add("SOUND_MCES05H", 25263)
        Sounds.Add("SOUND_MCES05J", 25264)
        Sounds.Add("SOUND_MCES05K", 25265)
        Sounds.Add("SOUND_MCES05L", 25266)
        Sounds.Add("SOUND_MCES05M", 25267)
        Sounds.Add("SOUND_MCES05N", 25268)
        Sounds.Add("SOUND_MCES05O", 25269)
        Sounds.Add("SOUND_MCES05P", 25270)
        Sounds.Add("SOUND_MCES05Q", 25271)
        Sounds.Add("SOUND_MCES05R", 25272)
        Sounds.Add("SOUND_MCES06A", 25273)
        Sounds.Add("SOUND_MCES06B", 25274)
        Sounds.Add("SOUND_MCES06C", 25275)
        Sounds.Add("SOUND_MCES06D", 25276)
        Sounds.Add("SOUND_MCES06E", 25277)
        Sounds.Add("SOUND_MCES06F", 25278)
        Sounds.Add("SOUND_MCES06G", 25279)
        Sounds.Add("SOUND_MCES07A", 25280)
        Sounds.Add("SOUND_MCES07B", 25281)
        Sounds.Add("SOUND_MCES07C", 25282)
        Sounds.Add("SOUND_MCES07D", 25283)
        Sounds.Add("SOUND_MCES08", 25284)
        Sounds.Add("SOUND_MCES08A", 25285)
        Sounds.Add("SOUND_MCES08B", 25286)
        Sounds.Add("SOUND_MCES08C", 25287)
        Sounds.Add("SOUND_MCES08D", 25288)
        Sounds.Add("SOUND_MCES08E", 25289)
        Sounds.Add("SOUND_MCES09A", 25290)
        Sounds.Add("SOUND_MCES09B", 25291)
        Sounds.Add("SOUND_MCES09C", 25292)
        Sounds.Add("SOUND_MCES09D", 25293)
        Sounds.Add("SOUND_MCES09E", 25294)
        Sounds.Add("SOUND_MCES09F", 25295)
        Sounds.Add("SOUND_MCES09G", 25296)
        Sounds.Add("SOUND_MCES09H", 25297)
        Sounds.Add("SOUND_MCES09K", 25298)
        Sounds.Add("SOUND_MCES09L", 25299)
        Sounds.Add("SOUND_MCES09M", 25300)
        Sounds.Add("SOUND_MCES09N", 25301)
        Sounds.Add("SOUND_MCES09O", 25302)
        Sounds.Add("SOUND_MCES09P", 25303)
        Sounds.Add("SOUND_MCES10A", 25304)
        Sounds.Add("SOUND_MCES10B", 25305)
        Sounds.Add("SOUND_MCES10C", 25306)
        Sounds.Add("SOUND_MCES10D", 25307)
        Sounds.Add("SOUND_MCES10E", 25308)
        Sounds.Add("SOUND_MCES10F", 25309)
        Sounds.Add("SOUND_MCES10G", 25310)
        Sounds.Add("SOUND_MCES10H", 25311)
        Sounds.Add("SOUND_MCES10J", 25312)
        Sounds.Add("SOUND_MCES10K", 25313)
        Sounds.Add("SOUND_MCES11A", 25314)
        Sounds.Add("SOUND_MCES11B", 25315)
        Sounds.Add("SOUND_MCES11C", 25316)
        Sounds.Add("SOUND_MCES11D", 25317)
        Sounds.Add("SOUND_MCES11E", 25318)
        Sounds.Add("SOUND_MCES11F", 25319)
        Sounds.Add("SOUND_MDEN_1A", 25400)
        Sounds.Add("SOUND_MDEN_1B", 25401)
        Sounds.Add("SOUND_MDEN_1C", 25402)
        Sounds.Add("SOUND_MDEN_1D", 25403)
        Sounds.Add("SOUND_MDEN_1E", 25404)
        Sounds.Add("SOUND_MDEN_1F", 25405)
        Sounds.Add("SOUND_MDEN_1G", 25406)
        Sounds.Add("SOUND_MDEN_1H", 25407)
        Sounds.Add("SOUND_MDEN_1J", 25408)
        Sounds.Add("SOUND_MDEN_1K", 25409)
        Sounds.Add("SOUND_MDEN_1L", 25410)
        Sounds.Add("SOUND_MDEN_2A", 25411)
        Sounds.Add("SOUND_MDEN_2D", 25412)
        Sounds.Add("SOUND_MDEN_3A", 25413)
        Sounds.Add("SOUND_MDEN_3B", 25414)
        Sounds.Add("SOUND_MDEN_4A", 25415)
        Sounds.Add("SOUND_MDEN_4D", 25416)
        Sounds.Add("SOUND_MDEN_5A", 25417)
        Sounds.Add("SOUND_MDEN_5B", 25418)
        Sounds.Add("SOUND_MDEN_6A", 25419)
        Sounds.Add("SOUND_MDEN_6D", 25420)
        Sounds.Add("SOUND_MDEN_7D", 25421)
        Sounds.Add("SOUND_MDEN_8D", 25422)
        Sounds.Add("SOUND_MDEN_9D", 25423)
        Sounds.Add("SOUND__MEAT_TRACK_LOOP", 25600)
        Sounds.Add("SOUND__FREEZER_CLOSE", 25601)
        Sounds.Add("SOUND__FREEZER_OPEN", 25602)
        Sounds.Add("SOUND__MEAT_TRACK_START", 25603)
        Sounds.Add("SOUND__MEAT_TRACK_STOP", 25604)
        Sounds.Add("SOUND__MECHANIC_ATTACH_CAR_BOMB", 25800)
        Sounds.Add("SOUND__MECHANIC_SLIDE_OUT", 25801)
        Sounds.Add("SOUND_MEC_B1", 26000)
        Sounds.Add("SOUND_MEC_B2", 26001)
        Sounds.Add("SOUND_MEC_D1", 26002)
        Sounds.Add("SOUND_MEC_D2", 26003)
        Sounds.Add("SOUND_MEC_D3", 26004)
        Sounds.Add("SOUND_MEC_D4", 26005)
        Sounds.Add("SOUND_MEC_D5", 26006)
        Sounds.Add("SOUND_MEC_HI", 26007)
        Sounds.Add("SOUND_MEC_N", 26008)
        Sounds.Add("SOUND_MEC_Y", 26009)
        Sounds.Add("SOUND_MHELD7D", 26200)
        Sounds.Add("SOUND_MHELD8D", 26201)
        Sounds.Add("SOUND_MHELD9D", 26202)
        Sounds.Add("SOUND_MHEL_1A", 26203)
        Sounds.Add("SOUND_MHEL_1B", 26204)
        Sounds.Add("SOUND_MHEL_1C", 26205)
        Sounds.Add("SOUND_MHEL_1D", 26206)
        Sounds.Add("SOUND_MHEL_1E", 26207)
        Sounds.Add("SOUND_MHEL_1F", 26208)
        Sounds.Add("SOUND_MHEL_1G", 26209)
        Sounds.Add("SOUND_MHEL_1H", 26210)
        Sounds.Add("SOUND_MHEL_1J", 26211)
        Sounds.Add("SOUND_MHEL_1K", 26212)
        Sounds.Add("SOUND_MHEL_1L", 26213)
        Sounds.Add("SOUND_MHEL_2A", 26214)
        Sounds.Add("SOUND_MHEL_2B", 26215)
        Sounds.Add("SOUND_MHEL_3A", 26216)
        Sounds.Add("SOUND_MHEL_3B", 26217)
        Sounds.Add("SOUND_MHEL_4A", 26218)
        Sounds.Add("SOUND_MHEL_4B", 26219)
        Sounds.Add("SOUND_MHEL_5A", 26220)
        Sounds.Add("SOUND_MHEL_5B", 26221)
        Sounds.Add("SOUND_MHEL_6A", 26222)
        Sounds.Add("SOUND_MHEL_6B", 26223)
        Sounds.Add("SOUND_MHRZ01A", 26400)
        Sounds.Add("SOUND_MHRZ01B", 26401)
        Sounds.Add("SOUND_MHRZ01C", 26402)
        Sounds.Add("SOUND_MHRZ01D", 26403)
        Sounds.Add("SOUND_MHRZ01E", 26404)
        Sounds.Add("SOUND_MHRZ01F", 26405)
        Sounds.Add("SOUND_MHRZ01G", 26406)
        Sounds.Add("SOUND_MHRZ01H", 26407)
        Sounds.Add("SOUND_MHRZ01J", 26408)
        Sounds.Add("SOUND_MHRZ01K", 26409)
        Sounds.Add("SOUND_MHRZ01L", 26410)
        Sounds.Add("SOUND_MHRZ01M", 26411)
        Sounds.Add("SOUND_MHRZ01N", 26412)
        Sounds.Add("SOUND_MJET_1A", 26600)
        Sounds.Add("SOUND_MJET_1B", 26601)
        Sounds.Add("SOUND_MJET_1C", 26602)
        Sounds.Add("SOUND_MJET_1D", 26603)
        Sounds.Add("SOUND_MJET_1E", 26604)
        Sounds.Add("SOUND_MJET_1F", 26605)
        Sounds.Add("SOUND_MJET_1G", 26606)
        Sounds.Add("SOUND_MJET_1H", 26607)
        Sounds.Add("SOUND_MJET_1J", 26608)
        Sounds.Add("SOUND_MJET_1K", 26609)
        Sounds.Add("SOUND_MJET_1L", 26610)
        Sounds.Add("SOUND_MJET_1M", 26611)
        Sounds.Add("SOUND_MJET_1N", 26612)
        Sounds.Add("SOUND_MJET_1O", 26613)
        Sounds.Add("SOUND_MJET_1P", 26614)
        Sounds.Add("SOUND_MJET_2A", 26615)
        Sounds.Add("SOUND_MJET_2B", 26616)
        Sounds.Add("SOUND_MJET_2C", 26617)
        Sounds.Add("SOUND_MJET_2D", 26618)
        Sounds.Add("SOUND_MJET_2E", 26619)
        Sounds.Add("SOUND_MJET_2F", 26620)
        Sounds.Add("SOUND_MJET_2G", 26621)
        Sounds.Add("SOUND_MJET_2H", 26622)
        Sounds.Add("SOUND_MJET_2J", 26623)
        Sounds.Add("SOUND_MJET_2K", 26624)
        Sounds.Add("SOUND_MJET_2L", 26625)
        Sounds.Add("SOUND_MJET_3A", 26626)
        Sounds.Add("SOUND_MJET_3B", 26627)
        Sounds.Add("SOUND_MJET_3C", 26628)
        Sounds.Add("SOUND_MJET_3D", 26629)
        Sounds.Add("SOUND_MJET_3E", 26630)
        Sounds.Add("SOUND_MJET_3F", 26631)
        Sounds.Add("SOUND_MJET_3G", 26632)
        Sounds.Add("SOUND_MJET_3H", 26633)
        Sounds.Add("SOUND_MJIZ01A", 26800)
        Sounds.Add("SOUND_MJIZ01B", 26801)
        Sounds.Add("SOUND_MJIZ01C", 26802)
        Sounds.Add("SOUND_MJIZ01D", 26803)
        Sounds.Add("SOUND_MJIZ01E", 26804)
        Sounds.Add("SOUND_MJIZ01F", 26805)
        Sounds.Add("SOUND_MJIZ01G", 26806)
        Sounds.Add("SOUND_MJIZ01H", 26807)
        Sounds.Add("SOUND_MJIZ02A", 26808)
        Sounds.Add("SOUND_MJIZ02B", 26809)
        Sounds.Add("SOUND_MJIZ02C", 26810)
        Sounds.Add("SOUND_MJIZ02D", 26811)
        Sounds.Add("SOUND_MKND01A", 27000)
        Sounds.Add("SOUND_MKND01B", 27001)
        Sounds.Add("SOUND_MKND01C", 27002)
        Sounds.Add("SOUND_MKND01D", 27003)
        Sounds.Add("SOUND_MKND01E", 27004)
        Sounds.Add("SOUND_MKND01F", 27005)
        Sounds.Add("SOUND_MKND01G", 27006)
        Sounds.Add("SOUND_MKND01H", 27007)
        Sounds.Add("SOUND_MKND01J", 27008)
        Sounds.Add("SOUND_MKND02A", 27009)
        Sounds.Add("SOUND_MKND02B", 27010)
        Sounds.Add("SOUND_MKND02C", 27011)
        Sounds.Add("SOUND_MKND02D", 27012)
        Sounds.Add("SOUND_MKND02E", 27013)
        Sounds.Add("SOUND_MKND02F", 27014)
        Sounds.Add("SOUND_MKND02G", 27015)
        Sounds.Add("SOUND_MKND02H", 27016)
        Sounds.Add("SOUND_MKND02J", 27017)
        Sounds.Add("SOUND_MKND02K", 27018)
        Sounds.Add("SOUND_MKP01A", 27200)
        Sounds.Add("SOUND_MKP01B", 27201)
        Sounds.Add("SOUND_MKP01C", 27202)
        Sounds.Add("SOUND_MKP01D", 27203)
        Sounds.Add("SOUND_MKP01E", 27204)
        Sounds.Add("SOUND_MKP01F", 27205)
        Sounds.Add("SOUND_MLOC01B", 27400)
        Sounds.Add("SOUND_MLOC01F", 27401)
        Sounds.Add("SOUND_MLOC02A", 27402)
        Sounds.Add("SOUND_MLOC02B", 27403)
        Sounds.Add("SOUND_MLOC02C", 27404)
        Sounds.Add("SOUND_MLOC02D", 27405)
        Sounds.Add("SOUND_MLOC02E", 27406)
        Sounds.Add("SOUND_MLOC02F", 27407)
        Sounds.Add("SOUND_MLOC02G", 27408)
        Sounds.Add("SOUND_MLOC02H", 27409)
        Sounds.Add("SOUND_MLOC02J", 27410)
        Sounds.Add("SOUND_MLOC02K", 27411)
        Sounds.Add("SOUND_MLOC03A", 27412)
        Sounds.Add("SOUND_MLOC03B", 27413)
        Sounds.Add("SOUND_MLOC03C", 27414)
        Sounds.Add("SOUND_MLOC03D", 27415)
        Sounds.Add("SOUND_MLOC03E", 27416)
        Sounds.Add("SOUND_MLOC03F", 27417)
        Sounds.Add("SOUND_MLOC03H", 27418)
        Sounds.Add("SOUND_MLOC04A", 27419)
        Sounds.Add("SOUND_MLOC04B", 27420)
        Sounds.Add("SOUND_MLOC04C", 27421)
        Sounds.Add("SOUND_MLOC04D", 27422)
        Sounds.Add("SOUND_MMICH1A", 27600)
        Sounds.Add("SOUND_MMICH1B", 27601)
        Sounds.Add("SOUND_MMICH1C", 27602)
        Sounds.Add("SOUND_MMICH1D", 27603)
        Sounds.Add("SOUND_MMICH1E", 27604)
        Sounds.Add("SOUND_MMICH1F", 27605)
        Sounds.Add("SOUND_MMICH1G", 27606)
        Sounds.Add("SOUND_MMICH1H", 27607)
        Sounds.Add("SOUND_MMICH1J", 27608)
        Sounds.Add("SOUND_MMICH2A", 27609)
        Sounds.Add("SOUND_MMICH2B", 27610)
        Sounds.Add("SOUND_MMICH3A", 27611)
        Sounds.Add("SOUND_MMICH3D", 27612)
        Sounds.Add("SOUND_MMICH4A", 27613)
        Sounds.Add("SOUND_MMICH4B", 27614)
        Sounds.Add("SOUND_MMICH5A", 27615)
        Sounds.Add("SOUND_MMICH5D", 27616)
        Sounds.Add("SOUND_MMICH6A", 27617)
        Sounds.Add("SOUND_MMICH6D", 27618)
        Sounds.Add("SOUND_MMICH7D", 27619)
        Sounds.Add("SOUND_MMICH8D", 27620)
        Sounds.Add("SOUND_MMICH9D", 27621)
        Sounds.Add("SOUND_MMIL10D", 27800)
        Sounds.Add("SOUND_MMILL1A", 27801)
        Sounds.Add("SOUND_MMILL1B", 27802)
        Sounds.Add("SOUND_MMILL1C", 27803)
        Sounds.Add("SOUND_MMILL1D", 27804)
        Sounds.Add("SOUND_MMILL1E", 27805)
        Sounds.Add("SOUND_MMILL1F", 27806)
        Sounds.Add("SOUND_MMILL1G", 27807)
        Sounds.Add("SOUND_MMILL1H", 27808)
        Sounds.Add("SOUND_MMILL1J", 27809)
        Sounds.Add("SOUND_MMILL1K", 27810)
        Sounds.Add("SOUND_MMILL2A", 27811)
        Sounds.Add("SOUND_MMILL2B", 27812)
        Sounds.Add("SOUND_MMILL2C", 27813)
        Sounds.Add("SOUND_MMILL2D", 27814)
        Sounds.Add("SOUND_MMILL2E", 27815)
        Sounds.Add("SOUND_MMILL2F", 27816)
        Sounds.Add("SOUND_MMILL2G", 27817)
        Sounds.Add("SOUND_MMILL2H", 27818)
        Sounds.Add("SOUND_MMILL2J", 27819)
        Sounds.Add("SOUND_MMILL2K", 27820)
        Sounds.Add("SOUND_MMILL2L", 27821)
        Sounds.Add("SOUND_MMILL3A", 27822)
        Sounds.Add("SOUND_MMILL3B", 27823)
        Sounds.Add("SOUND_MMILL4A", 27824)
        Sounds.Add("SOUND_MMILL4D", 27825)
        Sounds.Add("SOUND_MMILL5A", 27826)
        Sounds.Add("SOUND_MMILL5D", 27827)
        Sounds.Add("SOUND_MMILL6A", 27828)
        Sounds.Add("SOUND_MMILL6B", 27829)
        Sounds.Add("SOUND_MMILL7A", 27830)
        Sounds.Add("SOUND_MMILL7D", 27831)
        Sounds.Add("SOUND_MMILL8D", 27832)
        Sounds.Add("SOUND_MMILL9D", 27833)
        Sounds.Add("SOUND_REVERB_CAR_SCREECH", 28000)
        Sounds.Add("SOUND_MPUL01B", 28200)
        Sounds.Add("SOUND_MPUL01G", 28201)
        Sounds.Add("SOUND_MPUL01H", 28202)
        Sounds.Add("SOUND_MPUL01M", 28203)
        Sounds.Add("SOUND_MPUL01O", 28204)
        Sounds.Add("SOUND_MROS01B", 28400)
        Sounds.Add("SOUND_MROS01D", 28401)
        Sounds.Add("SOUND_MROS01E", 28402)
        Sounds.Add("SOUND_MROS01F", 28403)
        Sounds.Add("SOUND_MROS01H", 28404)
        Sounds.Add("SOUND_MROS01L", 28405)
        Sounds.Add("SOUND_MROS02A", 28406)
        Sounds.Add("SOUND_MROS02B", 28407)
        Sounds.Add("SOUND_MROS02C", 28408)
        Sounds.Add("SOUND_MROS02D", 28409)
        Sounds.Add("SOUND_MROS02E", 28410)
        Sounds.Add("SOUND_MROS02F", 28411)
        Sounds.Add("SOUND_MROS02G", 28412)
        Sounds.Add("SOUND_MROS03A", 28413)
        Sounds.Add("SOUND_MROS03B", 28414)
        Sounds.Add("SOUND_MROS03C", 28415)
        Sounds.Add("SOUND_MROS03D", 28416)
        Sounds.Add("SOUND_MROS03E", 28417)
        Sounds.Add("SOUND_MROS03F", 28418)
        Sounds.Add("SOUND_MROS03G", 28419)
        Sounds.Add("SOUND_MROS03H", 28420)
        Sounds.Add("SOUND_MROS03J", 28421)
        Sounds.Add("SOUND_MROS03K", 28422)
        Sounds.Add("SOUND_MROS03L", 28423)
        Sounds.Add("SOUND_MROS03N", 28424)
        Sounds.Add("SOUND_MROS03O", 28425)
        Sounds.Add("SOUND_MROS03P", 28426)
        Sounds.Add("SOUND_MROS03Q", 28427)
        Sounds.Add("SOUND_MSAL01A", 28600)
        Sounds.Add("SOUND_MSAL01B", 28601)
        Sounds.Add("SOUND_MSAL01C", 28602)
        Sounds.Add("SOUND_MSAL01D", 28603)
        Sounds.Add("SOUND_MSAL01E", 28604)
        Sounds.Add("SOUND_MSAL01F", 28605)
        Sounds.Add("SOUND_MSAL01G", 28606)
        Sounds.Add("SOUND_MSAL01H", 28607)
        Sounds.Add("SOUND_MSAL01J", 28608)
        Sounds.Add("SOUND_MSAL01K", 28609)
        Sounds.Add("SOUND_MSAL01M", 28610)
        Sounds.Add("SOUND_MSAL01N", 28611)
        Sounds.Add("SOUND_MSAL01O", 28612)
        Sounds.Add("SOUND_MSAL02A", 28613)
        Sounds.Add("SOUND_MSAL02B", 28614)
        Sounds.Add("SOUND_MSAL02C", 28615)
        Sounds.Add("SOUND_MSAL02D", 28616)
        Sounds.Add("SOUND_MSAL02E", 28617)
        Sounds.Add("SOUND_MSAL02F", 28618)
        Sounds.Add("SOUND_MSAL02G", 28619)
        Sounds.Add("SOUND_MSAL02H", 28620)
        Sounds.Add("SOUND_MSAL02J", 28621)
        Sounds.Add("SOUND_MSAL02K", 28622)
        Sounds.Add("SOUND_MSMK01A", 28800)
        Sounds.Add("SOUND_MSMK01B", 28801)
        Sounds.Add("SOUND_MSMK01C", 28802)
        Sounds.Add("SOUND_MSMK01D", 28803)
        Sounds.Add("SOUND_MSMK01E", 28804)
        Sounds.Add("SOUND_MSMK01F", 28805)
        Sounds.Add("SOUND_MSMK01G", 28806)
        Sounds.Add("SOUND_MSMK01H", 28807)
        Sounds.Add("SOUND_MSMK01J", 28808)
        Sounds.Add("SOUND_MSMK01K", 28809)
        Sounds.Add("SOUND_MSMK01L", 28810)
        Sounds.Add("SOUND_MSMK01M", 28811)
        Sounds.Add("SOUND_MSWE01A", 29000)
        Sounds.Add("SOUND_MSWE01B", 29001)
        Sounds.Add("SOUND_MSWE01C", 29002)
        Sounds.Add("SOUND_MSWE01D", 29003)
        Sounds.Add("SOUND_MSWE01E", 29004)
        Sounds.Add("SOUND_MSWE01F", 29005)
        Sounds.Add("SOUND_MSWE01G", 29006)
        Sounds.Add("SOUND_MSWE01H", 29007)
        Sounds.Add("SOUND_MSWE01J", 29008)
        Sounds.Add("SOUND_MSWE01K", 29009)
        Sounds.Add("SOUND_MSWE01L", 29010)
        Sounds.Add("SOUND_MSWE01M", 29011)
        Sounds.Add("SOUND_MSWE01N", 29012)
        Sounds.Add("SOUND_MSWE02A", 29013)
        Sounds.Add("SOUND_MSWE02B", 29014)
        Sounds.Add("SOUND_MSWE02C", 29015)
        Sounds.Add("SOUND_MSWE02D", 29016)
        Sounds.Add("SOUND_MSWE02E", 29017)
        Sounds.Add("SOUND_MSWE02F", 29018)
        Sounds.Add("SOUND_MSWE02G", 29019)
        Sounds.Add("SOUND_MSWE02H", 29020)
        Sounds.Add("SOUND_MSWE02J", 29021)
        Sounds.Add("SOUND_MSWE02K", 29022)
        Sounds.Add("SOUND_MSWE02L", 29023)
        Sounds.Add("SOUND_MSWE03A", 29024)
        Sounds.Add("SOUND_MSWE03B", 29025)
        Sounds.Add("SOUND_MSWE03C", 29026)
        Sounds.Add("SOUND_MSWE03D", 29027)
        Sounds.Add("SOUND_MSWE03E", 29028)
        Sounds.Add("SOUND_MSWE03F", 29029)
        Sounds.Add("SOUND_MSWE03G", 29030)
        Sounds.Add("SOUND_MSWE03H", 29031)
        Sounds.Add("SOUND_MSWE03J", 29032)
        Sounds.Add("SOUND_MSWE03K", 29033)
        Sounds.Add("SOUND_MSWE04A", 29034)
        Sounds.Add("SOUND_MSWE04B", 29035)
        Sounds.Add("SOUND_MSWE04C", 29036)
        Sounds.Add("SOUND_MSWE04D", 29037)
        Sounds.Add("SOUND_MSWE04E", 29038)
        Sounds.Add("SOUND_MSWE04F", 29039)
        Sounds.Add("SOUND_MSWE04G", 29040)
        Sounds.Add("SOUND_MSWE04H", 29041)
        Sounds.Add("SOUND_MSWE05A", 29042)
        Sounds.Add("SOUND_MSWE05B", 29043)
        Sounds.Add("SOUND_MSWE05C", 29044)
        Sounds.Add("SOUND_MSWE05D", 29045)
        Sounds.Add("SOUND_MSWE05E", 29046)
        Sounds.Add("SOUND_MSWE05F", 29047)
        Sounds.Add("SOUND_MSWE05G", 29048)
        Sounds.Add("SOUND_MSWE05H", 29049)
        Sounds.Add("SOUND_MSWE05J", 29050)
        Sounds.Add("SOUND_MSWE05K", 29051)
        Sounds.Add("SOUND_MSWE06A", 29052)
        Sounds.Add("SOUND_MSWE06B", 29053)
        Sounds.Add("SOUND_MSWE06C", 29054)
        Sounds.Add("SOUND_MSWE06D", 29055)
        Sounds.Add("SOUND_MSWE06E", 29056)
        Sounds.Add("SOUND_MSWE06F", 29057)
        Sounds.Add("SOUND_MSWE06G", 29058)
        Sounds.Add("SOUND_MSWE06H", 29059)
        Sounds.Add("SOUND_MSWE06J", 29060)
        Sounds.Add("SOUND_MSWE06K", 29061)
        Sounds.Add("SOUND_MSWE06L", 29062)
        Sounds.Add("SOUND_MSWE06M", 29063)
        Sounds.Add("SOUND_MSWE06N", 29064)
        Sounds.Add("SOUND_MSWE06O", 29065)
        Sounds.Add("SOUND_MSWE07A", 29066)
        Sounds.Add("SOUND_MSWE07B", 29067)
        Sounds.Add("SOUND_MSWE07C", 29068)
        Sounds.Add("SOUND_MSWE07D", 29069)
        Sounds.Add("SOUND_MSWE07E", 29070)
        Sounds.Add("SOUND_MSWE07F", 29071)
        Sounds.Add("SOUND_MSWE07G", 29072)
        Sounds.Add("SOUND_MSWE07H", 29073)
        Sounds.Add("SOUND_MSWE07J", 29074)
        Sounds.Add("SOUND_MSWE07K", 29075)
        Sounds.Add("SOUND_MSWE08A", 29076)
        Sounds.Add("SOUND_MSWE08B", 29077)
        Sounds.Add("SOUND_MSWE08C", 29078)
        Sounds.Add("SOUND_MSWE08D", 29079)
        Sounds.Add("SOUND_MSWE08E", 29080)
        Sounds.Add("SOUND_MSWE08F", 29081)
        Sounds.Add("SOUND_MSWE08G", 29082)
        Sounds.Add("SOUND_MSWE08H", 29083)
        Sounds.Add("SOUND_MSWE08J", 29084)
        Sounds.Add("SOUND_MSWE08K", 29085)
        Sounds.Add("SOUND_MSWE08L", 29086)
        Sounds.Add("SOUND_MSWE08M", 29087)
        Sounds.Add("SOUND_MSWE08N", 29088)
        Sounds.Add("SOUND_MSWE09A", 29089)
        Sounds.Add("SOUND_MSWE09B", 29090)
        Sounds.Add("SOUND_MSWE09C", 29091)
        Sounds.Add("SOUND_MSWE09E", 29092)
        Sounds.Add("SOUND_MSWE09F", 29093)
        Sounds.Add("SOUND_MSWE09G", 29094)
        Sounds.Add("SOUND_MSWE09H", 29095)
        Sounds.Add("SOUND_MSWE10A", 29096)
        Sounds.Add("SOUND_MSWE10B", 29097)
        Sounds.Add("SOUND_MSWE10C", 29098)
        Sounds.Add("SOUND_MSWE10D", 29099)
        Sounds.Add("SOUND_MSWE10E", 29100)
        Sounds.Add("SOUND_MSWE10F", 29101)
        Sounds.Add("SOUND_MSWE10G", 29102)
        Sounds.Add("SOUND_MSWE10H", 29103)
        Sounds.Add("SOUND_MSWE10J", 29104)
        Sounds.Add("SOUND_MSWE10K", 29105)
        Sounds.Add("SOUND_MSWE10L", 29106)
        Sounds.Add("SOUND_MSWE10N", 29107)
        Sounds.Add("SOUND_MSWE11A", 29108)
        Sounds.Add("SOUND_MSWE11B", 29109)
        Sounds.Add("SOUND_MSWE11C", 29110)
        Sounds.Add("SOUND_MSWE11D", 29111)
        Sounds.Add("SOUND_MSWE11E", 29112)
        Sounds.Add("SOUND_MSWE11F", 29113)
        Sounds.Add("SOUND_MSWE11G", 29114)
        Sounds.Add("SOUND_MSWE11H", 29115)
        Sounds.Add("SOUND_MSWE11J", 29116)
        Sounds.Add("SOUND_MSWE12A", 29117)
        Sounds.Add("SOUND_MSWE12B", 29118)
        Sounds.Add("SOUND_MSWE12C", 29119)
        Sounds.Add("SOUND_MSWE12D", 29120)
        Sounds.Add("SOUND_MSWE12E", 29121)
        Sounds.Add("SOUND_MSWE12F", 29122)
        Sounds.Add("SOUND_MSWE12G", 29123)
        Sounds.Add("SOUND_MSWE12H", 29124)
        Sounds.Add("SOUND_MSWE12J", 29125)
        Sounds.Add("SOUND_MSWE12K", 29126)
        Sounds.Add("SOUND_MSWE12L", 29127)
        Sounds.Add("SOUND_MSWE12M", 29128)
        Sounds.Add("SOUND_MSWE12N", 29129)
        Sounds.Add("SOUND_MSWE12O", 29130)
        Sounds.Add("SOUND_MSWE12P", 29131)
        Sounds.Add("SOUND_MSWE12Q", 29132)
        Sounds.Add("SOUND_MSWE12R", 29133)
        Sounds.Add("SOUND_MSWE12S", 29134)
        Sounds.Add("SOUND_MSWE12T", 29135)
        Sounds.Add("SOUND_MSWE12U", 29136)
        Sounds.Add("SOUND_MSWE13A", 29137)
        Sounds.Add("SOUND_MSWE13B", 29138)
        Sounds.Add("SOUND_MSWE13C", 29139)
        Sounds.Add("SOUND_MSWE13D", 29140)
        Sounds.Add("SOUND_MSWE13E", 29141)
        Sounds.Add("SOUND_MSWE13F", 29142)
        Sounds.Add("SOUND_MSWE13G", 29143)
        Sounds.Add("SOUND_MSWE14A", 29144)
        Sounds.Add("SOUND_MSWE14B", 29145)
        Sounds.Add("SOUND_MSWE14C", 29146)
        Sounds.Add("SOUND_MSWE14D", 29147)
        Sounds.Add("SOUND_MSWE14E", 29148)
        Sounds.Add("SOUND_MSWE14F", 29149)
        Sounds.Add("SOUND_MSWE14G", 29150)
        Sounds.Add("SOUND_MSWE14H", 29151)
        Sounds.Add("SOUND_MSWE14J", 29152)
        Sounds.Add("SOUND_MSWE14K", 29153)
        Sounds.Add("SOUND_MSWE14L", 29154)
        Sounds.Add("SOUND_MSWE14N", 29155)
        Sounds.Add("SOUND_MTEN01A", 29200)
        Sounds.Add("SOUND_MTEN01B", 29201)
        Sounds.Add("SOUND_MTEN01C", 29202)
        Sounds.Add("SOUND_MTEN01D", 29203)
        Sounds.Add("SOUND_MTEN01E", 29204)
        Sounds.Add("SOUND_MTEN01F", 29205)
        Sounds.Add("SOUND_MTEN01G", 29206)
        Sounds.Add("SOUND_MTEN01H", 29207)
        Sounds.Add("SOUND_MTEN01J", 29208)
        Sounds.Add("SOUND_MTEN02A", 29209)
        Sounds.Add("SOUND_MTEN02B", 29210)
        Sounds.Add("SOUND_MTEN02C", 29211)
        Sounds.Add("SOUND_MTEN02D", 29212)
        Sounds.Add("SOUND_MTEN02E", 29213)
        Sounds.Add("SOUND_MTEN02F", 29214)
        Sounds.Add("SOUND_MTEN03B", 29215)
        Sounds.Add("SOUND_MTEN03D", 29216)
        Sounds.Add("SOUND_MTEN03M", 29217)
        Sounds.Add("SOUND_MTG01A", 29400)
        Sounds.Add("SOUND_MTG01B", 29401)
        Sounds.Add("SOUND_MTG01C", 29402)
        Sounds.Add("SOUND_MTG01D", 29403)
        Sounds.Add("SOUND_MTG01E", 29404)
        Sounds.Add("SOUND_MTG01F", 29405)
        Sounds.Add("SOUND_MTG01G", 29406)
        Sounds.Add("SOUND_MTG01H", 29407)
        Sounds.Add("SOUND_MTG02A", 29408)
        Sounds.Add("SOUND_MTG02B", 29409)
        Sounds.Add("SOUND_MTG02C", 29410)
        Sounds.Add("SOUND_MTG02D", 29411)
        Sounds.Add("SOUND_MTG02E", 29412)
        Sounds.Add("SOUND_MTG02F", 29413)
        Sounds.Add("SOUND_MTOR01A", 29600)
        Sounds.Add("SOUND_MTOR01B", 29601)
        Sounds.Add("SOUND_MTOR01C", 29602)
        Sounds.Add("SOUND_MTOR01D", 29603)
        Sounds.Add("SOUND_MTOR01E", 29604)
        Sounds.Add("SOUND_MTOR01F", 29605)
        Sounds.Add("SOUND_MTOR01G", 29606)
        Sounds.Add("SOUND_MTOR01H", 29607)
        Sounds.Add("SOUND_MTOR01J", 29608)
        Sounds.Add("SOUND_MTOR01K", 29609)
        Sounds.Add("SOUND_MTOR01L", 29610)
        Sounds.Add("SOUND_MTOR02A", 29611)
        Sounds.Add("SOUND_MTOR02B", 29612)
        Sounds.Add("SOUND_MTOR02C", 29613)
        Sounds.Add("SOUND_MTOR02D", 29614)
        Sounds.Add("SOUND_MTOR02E", 29615)
        Sounds.Add("SOUND_MTOR03A", 29616)
        Sounds.Add("SOUND_MTOR03B", 29617)
        Sounds.Add("SOUND_MTOR04A", 29618)
        Sounds.Add("SOUND_MTOR04B", 29619)
        Sounds.Add("SOUND_MTOR04C", 29620)
        Sounds.Add("SOUND_MTOR04D", 29621)
        Sounds.Add("SOUND_MTOR04E", 29622)
        Sounds.Add("SOUND_MTOR04F", 29623)
        Sounds.Add("SOUND_MTOR04G", 29624)
        Sounds.Add("SOUND_MTOR04H", 29625)
        Sounds.Add("SOUND_MTOR04J", 29626)
        Sounds.Add("SOUND_MTOR04K", 29627)
        Sounds.Add("SOUND_MTOR04L", 29628)
        Sounds.Add("SOUND_MTOR04M", 29629)
        Sounds.Add("SOUND_MTOR05A", 29630)
        Sounds.Add("SOUND_MTOR05B", 29631)
        Sounds.Add("SOUND_MTOR05C", 29632)
        Sounds.Add("SOUND_MTOR05D", 29633)
        Sounds.Add("SOUND_MTOR05E", 29634)
        Sounds.Add("SOUND_MTOR05F", 29635)
        Sounds.Add("SOUND_MTOR05G", 29636)
        Sounds.Add("SOUND_MTOR05H", 29637)
        Sounds.Add("SOUND_MTOR05J", 29638)
        Sounds.Add("SOUND_MTOR06A", 29639)
        Sounds.Add("SOUND_MTOR06B", 29640)
        Sounds.Add("SOUND_MTOR06C", 29641)
        Sounds.Add("SOUND_MTOR06D", 29642)
        Sounds.Add("SOUND_MTOR06E", 29643)
        Sounds.Add("SOUND_MTOR06F", 29644)
        Sounds.Add("SOUND_MTOR06G", 29645)
        Sounds.Add("SOUND_MTOR06H", 29646)
        Sounds.Add("SOUND_MTOR06J", 29647)
        Sounds.Add("SOUND_MTOR06K", 29648)
        Sounds.Add("SOUND_MTOR06L", 29649)
        Sounds.Add("SOUND_MTOR06M", 29650)
        Sounds.Add("SOUND_MTOR06N", 29651)
        Sounds.Add("SOUND_MTOR07A", 29652)
        Sounds.Add("SOUND_MTOR07B", 29653)
        Sounds.Add("SOUND_MTOR07C", 29654)
        Sounds.Add("SOUND_MTOR07D", 29655)
        Sounds.Add("SOUND_MTOR07E", 29656)
        Sounds.Add("SOUND_MTOR07F", 29657)
        Sounds.Add("SOUND_MTOR07G", 29658)
        Sounds.Add("SOUND_MTOR07H", 29659)
        Sounds.Add("SOUND_MTOR07J", 29660)
        Sounds.Add("SOUND_MTOR07K", 29661)
        Sounds.Add("SOUND_MTOR07L", 29662)
        Sounds.Add("SOUND_MTOR07M", 29663)
        Sounds.Add("SOUND_MTOR07N", 29664)
        Sounds.Add("SOUND_MTOR07O", 29665)
        Sounds.Add("SOUND_MTRU01A", 29800)
        Sounds.Add("SOUND_MTRU01B", 29801)
        Sounds.Add("SOUND_MTRU01C", 29802)
        Sounds.Add("SOUND_MTRU01D", 29803)
        Sounds.Add("SOUND_MTRU01E", 29804)
        Sounds.Add("SOUND_MTRU01F", 29805)
        Sounds.Add("SOUND_MTRU01G", 29806)
        Sounds.Add("SOUND_MTRU01H", 29807)
        Sounds.Add("SOUND_MTRU01J", 29808)
        Sounds.Add("SOUND_MTRU01K", 29809)
        Sounds.Add("SOUND_MTRU01L", 29810)
        Sounds.Add("SOUND_MTRU01M", 29811)
        Sounds.Add("SOUND_MTRU01N", 29812)
        Sounds.Add("SOUND_MTRU01O", 29813)
        Sounds.Add("SOUND_MTRU01P", 29814)
        Sounds.Add("SOUND_MTRU02A", 29815)
        Sounds.Add("SOUND_MTRU02B", 29816)
        Sounds.Add("SOUND_MTRU02C", 29817)
        Sounds.Add("SOUND_MTRU02D", 29818)
        Sounds.Add("SOUND_MTRU02E", 29819)
        Sounds.Add("SOUND_MTRU02F", 29820)
        Sounds.Add("SOUND_MTRU03A", 29821)
        Sounds.Add("SOUND_MTRU03B", 29822)
        Sounds.Add("SOUND_MTRU03C", 29823)
        Sounds.Add("SOUND_MTRU03D", 29824)
        Sounds.Add("SOUND_MTRU03E", 29825)
        Sounds.Add("SOUND_MWUZ00A", 30000)
        Sounds.Add("SOUND_MWUZ00B", 30001)
        Sounds.Add("SOUND_MWUZ00C", 30002)
        Sounds.Add("SOUND_MWUZ00D", 30003)
        Sounds.Add("SOUND_MWUZ00E", 30004)
        Sounds.Add("SOUND_MWUZ00F", 30005)
        Sounds.Add("SOUND_MWUZ00G", 30006)
        Sounds.Add("SOUND_MWUZ00H", 30007)
        Sounds.Add("SOUND_MWUZ00J", 30008)
        Sounds.Add("SOUND_MWUZ01A", 30009)
        Sounds.Add("SOUND_MWUZ01B", 30010)
        Sounds.Add("SOUND_MWUZ01C", 30011)
        Sounds.Add("SOUND_MWUZ01D", 30012)
        Sounds.Add("SOUND_MWUZ01E", 30013)
        Sounds.Add("SOUND_MWUZ01F", 30014)
        Sounds.Add("SOUND_MWUZ01G", 30015)
        Sounds.Add("SOUND_MWUZ01H", 30016)
        Sounds.Add("SOUND_MWUZ01J", 30017)
        Sounds.Add("SOUND_MWUZ01K", 30018)
        Sounds.Add("SOUND_MWUZ02A", 30019)
        Sounds.Add("SOUND_MWUZ02B", 30020)
        Sounds.Add("SOUND_MWUZ02C", 30021)
        Sounds.Add("SOUND_MWUZ02D", 30022)
        Sounds.Add("SOUND_MWUZ02E", 30023)
        Sounds.Add("SOUND_MWUZ02F", 30024)
        Sounds.Add("SOUND_MWUZ02G", 30025)
        Sounds.Add("SOUND_MWUZ02H", 30026)
        Sounds.Add("SOUND_MWUZ02J", 30027)
        Sounds.Add("SOUND_MWUZ02K", 30028)
        Sounds.Add("SOUND_MWUZ03A", 30029)
        Sounds.Add("SOUND_MWUZ03B", 30030)
        Sounds.Add("SOUND_MWUZ03C", 30031)
        Sounds.Add("SOUND_MWUZ03D", 30032)
        Sounds.Add("SOUND_MWUZ03E", 30033)
        Sounds.Add("SOUND_MWUZ03F", 30034)
        Sounds.Add("SOUND_MWUZ03G", 30035)
        Sounds.Add("SOUND_MWUZ03H", 30036)
        Sounds.Add("SOUND_MWUZ03J", 30037)
        Sounds.Add("SOUND_MWUZ03K", 30038)
        Sounds.Add("SOUND_MWUZ03L", 30039)
        Sounds.Add("SOUND_MWUZ04A", 30040)
        Sounds.Add("SOUND_MWUZ04B", 30041)
        Sounds.Add("SOUND_MWUZ04C", 30042)
        Sounds.Add("SOUND_MWUZ04D", 30043)
        Sounds.Add("SOUND_MWUZ04E", 30044)
        Sounds.Add("SOUND_MWUZ04F", 30045)
        Sounds.Add("SOUND_MWUZ04G", 30046)
        Sounds.Add("SOUND_MWUZ04H", 30047)
        Sounds.Add("SOUND_MWUZ05A", 30048)
        Sounds.Add("SOUND_MWUZ05B", 30049)
        Sounds.Add("SOUND_MWUZ05C", 30050)
        Sounds.Add("SOUND_MWUZ05D", 30051)
        Sounds.Add("SOUND_MWUZ05E", 30052)
        Sounds.Add("SOUND_MWUZ05F", 30053)
        Sounds.Add("SOUND_MWUZ05G", 30054)
        Sounds.Add("SOUND_MWUZ05H", 30055)
        Sounds.Add("SOUND_MWUZ05J", 30056)
        Sounds.Add("SOUND_MWUZ06A", 30057)
        Sounds.Add("SOUND_MWUZ06B", 30058)
        Sounds.Add("SOUND_MWUZ06C", 30059)
        Sounds.Add("SOUND_MWUZ06D", 30060)
        Sounds.Add("SOUND_MWUZ06E", 30061)
        Sounds.Add("SOUND_MWUZ06F", 30062)
        Sounds.Add("SOUND_MWUZ06G", 30063)
        Sounds.Add("SOUND_MWUZ06H", 30064)
        Sounds.Add("SOUND_MWUZ06J", 30065)
        Sounds.Add("SOUND_MWUZ06K", 30066)
        Sounds.Add("SOUND_MWUZ06L", 30067)
        Sounds.Add("SOUND_MWUZ06M", 30068)
        Sounds.Add("SOUND_MWUZ06N", 30069)
        Sounds.Add("SOUND_MWUZ07A", 30070)
        Sounds.Add("SOUND_MWUZ07B", 30071)
        Sounds.Add("SOUND_MWUZ07C", 30072)
        Sounds.Add("SOUND_MWUZ07D", 30073)
        Sounds.Add("SOUND_MWUZ07E", 30074)
        Sounds.Add("SOUND_MWUZ08B", 30075)
        Sounds.Add("SOUND_MWUZ08C", 30076)
        Sounds.Add("SOUND_MWUZ08D", 30077)
        Sounds.Add("SOUND_MWUZ08F", 30078)
        Sounds.Add("SOUND_MWUZ09E", 30079)
        Sounds.Add("SOUND_MWUZ09F", 30080)
        Sounds.Add("SOUND_MWUZ09H", 30081)
        Sounds.Add("SOUND_MWUZ09J", 30082)
        Sounds.Add("SOUND_MZAHN1A", 30200)
        Sounds.Add("SOUND_MZAHN1B", 30201)
        Sounds.Add("SOUND_MZAHN1C", 30202)
        Sounds.Add("SOUND_MZAHN1D", 30203)
        Sounds.Add("SOUND_MZAHN1E", 30204)
        Sounds.Add("SOUND_MZAHN1F", 30205)
        Sounds.Add("SOUND_MZAHN1G", 30206)
        Sounds.Add("SOUND_MZAHN1H", 30207)
        Sounds.Add("SOUND_MZAHN1J", 30208)
        Sounds.Add("SOUND_MZAHN2A", 30209)
        Sounds.Add("SOUND_MZAHN2D", 30210)
        Sounds.Add("SOUND_MZAHN3A", 30211)
        Sounds.Add("SOUND_MZAHN3B", 30212)
        Sounds.Add("SOUND_MZAHN4A", 30213)
        Sounds.Add("SOUND_MZAHN4D", 30214)
        Sounds.Add("SOUND_MZAHN5A", 30215)
        Sounds.Add("SOUND_MZAHN5B", 30216)
        Sounds.Add("SOUND_MZAHN6A", 30217)
        Sounds.Add("SOUND_MZAHN6D", 30218)
        Sounds.Add("SOUND_MZAHN7D", 30219)
        Sounds.Add("SOUND_MZAHN8D", 30220)
        Sounds.Add("SOUND_MZAHN9D", 30221)
        Sounds.Add("SOUND_MZER01A", 30400)
        Sounds.Add("SOUND_MZER01B", 30401)
        Sounds.Add("SOUND_MZER01C", 30402)
        Sounds.Add("SOUND_MZER01D", 30403)
        Sounds.Add("SOUND_MZER01E", 30404)
        Sounds.Add("SOUND_MZER01F", 30405)
        Sounds.Add("SOUND_MZER01G", 30406)
        Sounds.Add("SOUND_MZER02A", 30407)
        Sounds.Add("SOUND_MZER02B", 30408)
        Sounds.Add("SOUND_MZER02C", 30409)
        Sounds.Add("SOUND_MZER02D", 30410)
        Sounds.Add("SOUND_MZER02E", 30411)
        Sounds.Add("SOUND_MZER02F", 30412)
        Sounds.Add("SOUND_MZER02G", 30413)
        Sounds.Add("SOUND_MZER02H", 30414)
        Sounds.Add("SOUND_MZER02J", 30415)
        Sounds.Add("SOUND_MZER02K", 30416)
        Sounds.Add("SOUND_RADAR_LEVEL_WARNING", 30600)
        Sounds.Add("SOUND_NULL_1A", 30800)
        Sounds.Add("SOUND_NULL_1B", 30801)
        Sounds.Add("SOUND_NULL_2A", 30802)
        Sounds.Add("SOUND_NULL_2B", 30803)
        Sounds.Add("SOUND__OGLOC_DOORBELL", 31000)
        Sounds.Add("SOUND__OGLOC_WINDOW_RATTLE_BANG", 31001)
        Sounds.Add("SOUND__BET_ZERO", 31200)
        Sounds.Add("SOUND__INCREASE_BET", 31201)
        Sounds.Add("SOUND__LOSE", 31202)
        Sounds.Add("SOUND__NO_CASH", 31203)
        Sounds.Add("SOUND__PLACEBET", 31204)
        Sounds.Add("SOUND__WIN", 31205)
        Sounds.Add("SOUND_PIMP_CUSTOMER_SEX", 31400)
        Sounds.Add("SOUND_POOL_B1", 31600)
        Sounds.Add("SOUND_POOL_B2", 31601)
        Sounds.Add("SOUND_POOL_B3", 31602)
        Sounds.Add("SOUND_POOL_B4", 31603)
        Sounds.Add("SOUND_POOL_B5", 31604)
        Sounds.Add("SOUND_POOL_B6", 31605)
        Sounds.Add("SOUND__POOL_BALL_HIT_BALL_1", 31800)
        Sounds.Add("SOUND__POOL_BALL_HIT_BALL_2", 31801)
        Sounds.Add("SOUND__POOL_BALL_HIT_BALL_3", 31802)
        Sounds.Add("SOUND__POOL_BALL_POT_1", 31803)
        Sounds.Add("SOUND__POOL_BALL_POT_2", 31804)
        Sounds.Add("SOUND__POOL_BALL_POT_3", 31805)
        Sounds.Add("SOUND__POOL_BREAK", 31806)
        Sounds.Add("SOUND__POOL_CHALK_CUE", 31807)
        Sounds.Add("SOUND__POOL_HIT_CUSHION_1", 31808)
        Sounds.Add("SOUND__POOL_HIT_CUSHION_2", 31809)
        Sounds.Add("SOUND__POOL_HIT_WHITE", 31810)
        Sounds.Add("SOUND_FIT_TYRE", 32000)
        Sounds.Add("SOUND__CJ_EAT", 32200)
        Sounds.Add("SOUND__CJ_PUKE", 32201)
        Sounds.Add("SOUND_CAR_SMASH_SIGN", 32400)
        Sounds.Add("SOUND_CEILING_VENT_OPEN", 32401)
        Sounds.Add("SOUND_HELI_SLASH_PED", 32402)
        Sounds.Add("SOUND_TRAILER_HOOKUP", 32600)
        Sounds.Add("SOUND_ROT1_AA", 32800)
        Sounds.Add("SOUND_ROT1_AB", 32801)
        Sounds.Add("SOUND_ROT1_BA", 32802)
        Sounds.Add("SOUND_ROT1_BB", 32803)
        Sounds.Add("SOUND_ROT1_BC", 32804)
        Sounds.Add("SOUND_ROT1_BD", 32805)
        Sounds.Add("SOUND_ROT1_BE", 32806)
        Sounds.Add("SOUND_ROT1_DA", 32807)
        Sounds.Add("SOUND_ROT1_DB", 32808)
        Sounds.Add("SOUND_ROT1_DC", 32809)
        Sounds.Add("SOUND_ROT1_EA", 32810)
        Sounds.Add("SOUND_ROT1_EB", 32811)
        Sounds.Add("SOUND_ROT1_EC", 32812)
        Sounds.Add("SOUND_ROT1_ED", 32813)
        Sounds.Add("SOUND_ROT1_EE", 32814)
        Sounds.Add("SOUND_ROT1_EF", 32815)
        Sounds.Add("SOUND_ROT1_EG", 32816)
        Sounds.Add("SOUND_ROT1_EH", 32817)
        Sounds.Add("SOUND_ROT1_EJ", 32818)
        Sounds.Add("SOUND_ROT1_EK", 32819)
        Sounds.Add("SOUND_ROT1_EL", 32820)
        Sounds.Add("SOUND_ROT1_EM", 32821)
        Sounds.Add("SOUND_ROT1_EN", 32822)
        Sounds.Add("SOUND_ROT1_FA", 32823)
        Sounds.Add("SOUND_ROT1_FB", 32824)
        Sounds.Add("SOUND_ROT1_FC", 32825)
        Sounds.Add("SOUND_ROT1_FD", 32826)
        Sounds.Add("SOUND_ROT1_FE", 32827)
        Sounds.Add("SOUND_ROT1_FF", 32828)
        Sounds.Add("SOUND_ROT1_FG", 32829)
        Sounds.Add("SOUND_ROT1_FH", 32830)
        Sounds.Add("SOUND_ROT1_GA", 32831)
        Sounds.Add("SOUND_ROT1_GB", 32832)
        Sounds.Add("SOUND_ROT1_GC", 32833)
        Sounds.Add("SOUND_ROT1_GD", 32834)
        Sounds.Add("SOUND_ROT1_GE", 32835)
        Sounds.Add("SOUND_ROT1_GG", 32836)
        Sounds.Add("SOUND_ROT1_GH", 32837)
        Sounds.Add("SOUND_ROT1_GJ", 32838)
        Sounds.Add("SOUND_ROT1_GK", 32839)
        Sounds.Add("SOUND_ROT1_GL", 32840)
        Sounds.Add("SOUND_ROT1_GM", 32841)
        Sounds.Add("SOUND_ROT1_HA", 32842)
        Sounds.Add("SOUND_ROT1_HB", 32843)
        Sounds.Add("SOUND_ROT1_HC", 32844)
        Sounds.Add("SOUND_ROT1_HD", 32845)
        Sounds.Add("SOUND_ROT1_HE", 32846)
        Sounds.Add("SOUND_ROT1_HF", 32847)
        Sounds.Add("SOUND_ROT2_AA", 33000)
        Sounds.Add("SOUND_ROT2_AB", 33001)
        Sounds.Add("SOUND_ROT2_AD", 33002)
        Sounds.Add("SOUND_ROT2_AE", 33003)
        Sounds.Add("SOUND_ROT2_BA", 33004)
        Sounds.Add("SOUND_ROT2_BB", 33005)
        Sounds.Add("SOUND_ROT2_BC", 33006)
        Sounds.Add("SOUND_ROT2_BD", 33007)
        Sounds.Add("SOUND_ROT2_CA", 33008)
        Sounds.Add("SOUND_ROT2_CB", 33009)
        Sounds.Add("SOUND_ROT2_CC", 33010)
        Sounds.Add("SOUND_ROT2_CD", 33011)
        Sounds.Add("SOUND_ROT2_CE", 33012)
        Sounds.Add("SOUND_ROT2_CF", 33013)
        Sounds.Add("SOUND_ROT2_CG", 33014)
        Sounds.Add("SOUND_ROT2_CH", 33015)
        Sounds.Add("SOUND_ROT2_CJ", 33016)
        Sounds.Add("SOUND_ROT2_CK", 33017)
        Sounds.Add("SOUND_ROT2_CL", 33018)
        Sounds.Add("SOUND_ROT2_CM", 33019)
        Sounds.Add("SOUND_ROT2_CN", 33020)
        Sounds.Add("SOUND_ROT2_CO", 33021)
        Sounds.Add("SOUND_ROT2_CP", 33022)
        Sounds.Add("SOUND_ROT2_CQ", 33023)
        Sounds.Add("SOUND_ROT2_DA", 33024)
        Sounds.Add("SOUND_ROT2_DB", 33025)
        Sounds.Add("SOUND_ROT2_DC", 33026)
        Sounds.Add("SOUND_ROT2_DD", 33027)
        Sounds.Add("SOUND_ROT2_DE", 33028)
        Sounds.Add("SOUND_ROT2_DF", 33029)
        Sounds.Add("SOUND_ROT2_DH", 33030)
        Sounds.Add("SOUND_ROT2_DJ", 33031)
        Sounds.Add("SOUND_ROT2_DK", 33032)
        Sounds.Add("SOUND_ROT2_DL", 33033)
        Sounds.Add("SOUND_ROT2_DM", 33034)
        Sounds.Add("SOUND_ROT2_EA", 33035)
        Sounds.Add("SOUND_ROT2_EB", 33036)
        Sounds.Add("SOUND_ROT2_EC", 33037)
        Sounds.Add("SOUND_ROT2_ED", 33038)
        Sounds.Add("SOUND_ROT2_EE", 33039)
        Sounds.Add("SOUND_ROT2_EF", 33040)
        Sounds.Add("SOUND_ROT2_EG", 33041)
        Sounds.Add("SOUND_ROT2_EH", 33042)
        Sounds.Add("SOUND_ROT2_EJ", 33043)
        Sounds.Add("SOUND_ROT2_EK", 33044)
        Sounds.Add("SOUND_ROT2_EL", 33045)
        Sounds.Add("SOUND_ROT2_EM", 33046)
        Sounds.Add("SOUND_ROT2_EN", 33047)
        Sounds.Add("SOUND_ROT2_EO", 33048)
        Sounds.Add("SOUND_ROT2_EP", 33049)
        Sounds.Add("SOUND_ROT2_EQ", 33050)
        Sounds.Add("SOUND_ROT2_ER", 33051)
        Sounds.Add("SOUND_ROT2_ES", 33052)
        Sounds.Add("SOUND_ROT2_ET", 33053)
        Sounds.Add("SOUND_ROT2_EU", 33054)
        Sounds.Add("SOUND_ROT2_EV", 33055)
        Sounds.Add("SOUND_ROT2_EW", 33056)
        Sounds.Add("SOUND_ROT2_EX", 33057)
        Sounds.Add("SOUND_ROT2_EY", 33058)
        Sounds.Add("SOUND_ROT2_FA", 33059)
        Sounds.Add("SOUND_ROT2_FB", 33060)
        Sounds.Add("SOUND_ROT2_FC", 33061)
        Sounds.Add("SOUND_ROT2_FD", 33062)
        Sounds.Add("SOUND_ROT2_FE", 33063)
        Sounds.Add("SOUND_ROT2_FG", 33064)
        Sounds.Add("SOUND_ROT2_FH", 33065)
        Sounds.Add("SOUND_ROT2_GA", 33066)
        Sounds.Add("SOUND_ROT2_GB", 33067)
        Sounds.Add("SOUND_ROT2_GC", 33068)
        Sounds.Add("SOUND_ROT2_GD", 33069)
        Sounds.Add("SOUND_ROT2_HA", 33070)
        Sounds.Add("SOUND_ROT2_HB", 33071)
        Sounds.Add("SOUND_ROT2_HC", 33072)
        Sounds.Add("SOUND_ROT2_JA", 33073)
        Sounds.Add("SOUND_ROT2_JB", 33074)
        Sounds.Add("SOUND_ROT2_KA", 33075)
        Sounds.Add("SOUND_ROT2_KB", 33076)
        Sounds.Add("SOUND_ROT2_KC", 33077)
        Sounds.Add("SOUND_ROT2_LA", 33078)
        Sounds.Add("SOUND_ROT2_LB", 33079)
        Sounds.Add("SOUND_ROT2_LC", 33080)
        Sounds.Add("SOUND_ROT2_MA", 33081)
        Sounds.Add("SOUND_ROT2_MB", 33082)
        Sounds.Add("SOUND_ROT2_MC", 33083)
        Sounds.Add("SOUND_ROT2_MD", 33084)
        Sounds.Add("SOUND_ROT2_ME", 33085)
        Sounds.Add("SOUND_ROT2_MF", 33086)
        Sounds.Add("SOUND_ROT2_MG", 33087)
        Sounds.Add("SOUND_ROT2_MH", 33088)
        Sounds.Add("SOUND_ROT4_AA", 33200)
        Sounds.Add("SOUND_ROT4_AB", 33201)
        Sounds.Add("SOUND_ROT4_AC", 33202)
        Sounds.Add("SOUND_ROT4_BA", 33203)
        Sounds.Add("SOUND_ROT4_BB", 33204)
        Sounds.Add("SOUND_ROT4_BC", 33205)
        Sounds.Add("SOUND_ROT4_BD", 33206)
        Sounds.Add("SOUND_ROT4_BE", 33207)
        Sounds.Add("SOUND_ROT4_BF", 33208)
        Sounds.Add("SOUND_ROT4_BG", 33209)
        Sounds.Add("SOUND_ROT4_BH", 33210)
        Sounds.Add("SOUND_ROT4_BJ", 33211)
        Sounds.Add("SOUND_ROT4_CA", 33212)
        Sounds.Add("SOUND_ROT4_CB", 33213)
        Sounds.Add("SOUND_ROT4_CC", 33214)
        Sounds.Add("SOUND_ROT4_DA", 33215)
        Sounds.Add("SOUND_ROT4_DB", 33216)
        Sounds.Add("SOUND_ROT4_EA", 33217)
        Sounds.Add("SOUND_ROT4_EB", 33218)
        Sounds.Add("SOUND_ROT4_EC", 33219)
        Sounds.Add("SOUND_ROT4_ED", 33220)
        Sounds.Add("SOUND_ROT4_FA", 33221)
        Sounds.Add("SOUND_ROT4_FB", 33222)
        Sounds.Add("SOUND_ROT4_FC", 33223)
        Sounds.Add("SOUND_ROT4_FD", 33224)
        Sounds.Add("SOUND_ROT4_GA", 33225)
        Sounds.Add("SOUND_ROT4_GB", 33226)
        Sounds.Add("SOUND_ROT4_HA", 33227)
        Sounds.Add("SOUND_ROT4_HB", 33228)
        Sounds.Add("SOUND_ROT4_HC", 33229)
        Sounds.Add("SOUND_ROT4_JB", 33230)
        Sounds.Add("SOUND_ROT4_JC", 33231)
        Sounds.Add("SOUND_ROT4_JD", 33232)
        Sounds.Add("SOUND_ROT4_KA", 33233)
        Sounds.Add("SOUND_ROT4_KB", 33234)
        Sounds.Add("SOUND_ROT4_KC", 33235)
        Sounds.Add("SOUND_ROT4_KD", 33236)
        Sounds.Add("SOUND_ROT4_KE", 33237)
        Sounds.Add("SOUND_ROT4_KF", 33238)
        Sounds.Add("SOUND_ROT4_KG", 33239)
        Sounds.Add("SOUND_ROT4_KH", 33240)
        Sounds.Add("SOUND_ROT4_KJ", 33241)
        Sounds.Add("SOUND_ROT4_LA", 33242)
        Sounds.Add("SOUND_ROT4_LB", 33243)
        Sounds.Add("SOUND_ROT4_MA", 33244)
        Sounds.Add("SOUND_ROT4_MB", 33245)
        Sounds.Add("SOUND_ROT4_MC", 33246)
        Sounds.Add("SOUND_ROT4_MD", 33247)
        Sounds.Add("SOUND_ROT4_ME", 33248)
        Sounds.Add("SOUND_ROT4_NA", 33249)
        Sounds.Add("SOUND_ROT4_NB", 33250)
        Sounds.Add("SOUND_ROT4_NC", 33251)
        Sounds.Add("SOUND_ROT4_ND", 33252)
        Sounds.Add("SOUND_ROT4_NE", 33253)
        Sounds.Add("SOUND_ROT4_NF", 33254)
        Sounds.Add("SOUND_ROT4_NG", 33255)
        Sounds.Add("SOUND_ROT4_NH", 33256)
        Sounds.Add("SOUND_ROT4_NJ", 33257)
        Sounds.Add("SOUND_ROT4_NK", 33258)
        Sounds.Add("SOUND_ROT4_NL", 33259)
        Sounds.Add("SOUND_ROT4_NM", 33260)
        Sounds.Add("SOUND_ROT4_NN", 33261)
        Sounds.Add("SOUND_ROT4_NO", 33262)
        Sounds.Add("SOUND_ROT4_NP", 33263)
        Sounds.Add("SOUND_ROT4_NQ", 33264)
        Sounds.Add("SOUND_ROT4_NR", 33265)
        Sounds.Add("SOUND_ROT4_NS", 33266)
        Sounds.Add("SOUND_ROT4_NT", 33267)
        Sounds.Add("SOUND_ROT4_NU", 33268)
        Sounds.Add("SOUND_ROT4_OA", 33269)
        Sounds.Add("SOUND_ROT4_OD", 33270)
        Sounds.Add("SOUND_ROT4_PA", 33271)
        Sounds.Add("SOUND_ROT4_PB", 33272)
        Sounds.Add("SOUND_ROT4_PC", 33273)
        Sounds.Add("SOUND_ROT4_QA", 33274)
        Sounds.Add("SOUND_ROT4_QB", 33275)
        Sounds.Add("SOUND_ROT4_QC", 33276)
        Sounds.Add("SOUND_ROT4_QD", 33277)
        Sounds.Add("SOUND_ROT4_QE", 33278)
        Sounds.Add("SOUND_ROT4_RA", 33279)
        Sounds.Add("SOUND_ROT4_RB", 33280)
        Sounds.Add("SOUND_ROT4_RC", 33281)
        Sounds.Add("SOUND_ROT4_RD", 33282)
        Sounds.Add("SOUND_ROT4_RE", 33283)
        Sounds.Add("SOUND_ROT4_SA", 33284)
        Sounds.Add("SOUND_ROT4_SB", 33285)
        Sounds.Add("SOUND_ROT4_TA", 33286)
        Sounds.Add("SOUND_ROT4_TB", 33287)
        Sounds.Add("SOUND_ROT4_TC", 33288)
        Sounds.Add("SOUND_ROT4_TD", 33289)
        Sounds.Add("SOUND_ROT4_TE", 33290)
        Sounds.Add("SOUND_ROT4_TF", 33291)
        Sounds.Add("SOUND_ROT4_TG", 33292)
        Sounds.Add("SOUND_ROT4_TH", 33293)
        Sounds.Add("SOUND_ROT4_TJ", 33294)
        Sounds.Add("SOUND_ROT4_TK", 33295)
        Sounds.Add("SOUND_ROT4_TL", 33296)
        Sounds.Add("SOUND_ROT4_TM", 33297)
        Sounds.Add("SOUND_ROT4_UA", 33298)
        Sounds.Add("SOUND_ROT4_UB", 33299)
        Sounds.Add("SOUND_ROT4_UC", 33300)
        Sounds.Add("SOUND_ROT4_UD", 33301)
        Sounds.Add("SOUND_ROT4_ZA", 33302)
        Sounds.Add("SOUND_ROT4_ZB", 33303)
        Sounds.Add("SOUND_ROT4_ZC", 33304)
        Sounds.Add("SOUND__ROULETTE_SPIN", 33400)
        Sounds.Add("SOUND__ROULETTE_BALL_BOUNCE1", 33401)
        Sounds.Add("SOUND__ROULETTE_BALL_BOUNCE2", 33402)
        Sounds.Add("SOUND__ROULETTE_BALL_BOUNCE3", 33403)
        Sounds.Add("SOUND_RYD1_AA", 33600)
        Sounds.Add("SOUND_RYD1_AB", 33601)
        Sounds.Add("SOUND_RYD1_AC", 33602)
        Sounds.Add("SOUND_RYD1_AD", 33603)
        Sounds.Add("SOUND_RYD1_AE", 33604)
        Sounds.Add("SOUND_RYD1_AF", 33605)
        Sounds.Add("SOUND_RYD1_AG", 33606)
        Sounds.Add("SOUND_RYD1_AH", 33607)
        Sounds.Add("SOUND_RYD1_AJ", 33608)
        Sounds.Add("SOUND_RYD1_BA", 33609)
        Sounds.Add("SOUND_RYD1_BB", 33610)
        Sounds.Add("SOUND_RYD1_BC", 33611)
        Sounds.Add("SOUND_RYD1_BD", 33612)
        Sounds.Add("SOUND_RYD1_BE", 33613)
        Sounds.Add("SOUND_RYD1_BF", 33614)
        Sounds.Add("SOUND_RYD1_CA", 33615)
        Sounds.Add("SOUND_RYD1_CB", 33616)
        Sounds.Add("SOUND_RYD1_CC", 33617)
        Sounds.Add("SOUND_RYD1_CD", 33618)
        Sounds.Add("SOUND_RYD1_CE", 33619)
        Sounds.Add("SOUND_RYD1_CF", 33620)
        Sounds.Add("SOUND_RYD1_DB", 33621)
        Sounds.Add("SOUND_RYD1_DC", 33622)
        Sounds.Add("SOUND_RYD1_DD", 33623)
        Sounds.Add("SOUND_RYD1_DE", 33624)
        Sounds.Add("SOUND_RYD1_DF", 33625)
        Sounds.Add("SOUND_RYD1_DG", 33626)
        Sounds.Add("SOUND_RYD1_DH", 33627)
        Sounds.Add("SOUND_RYD1_DJ", 33628)
        Sounds.Add("SOUND_RYD1_DK", 33629)
        Sounds.Add("SOUND_RYD1_DL", 33630)
        Sounds.Add("SOUND_RYD1_EA", 33631)
        Sounds.Add("SOUND_RYD1_EB", 33632)
        Sounds.Add("SOUND_RYD1_EC", 33633)
        Sounds.Add("SOUND_RYD1_ED", 33634)
        Sounds.Add("SOUND_RYD1_FA", 33635)
        Sounds.Add("SOUND_RYD1_FB", 33636)
        Sounds.Add("SOUND_RYD1_FC", 33637)
        Sounds.Add("SOUND_RYD1_GA", 33638)
        Sounds.Add("SOUND_RYD1_GB", 33639)
        Sounds.Add("SOUND_RYD1_GC", 33640)
        Sounds.Add("SOUND_RYD1_GD", 33641)
        Sounds.Add("SOUND_RYD1_GE", 33642)
        Sounds.Add("SOUND_RYD1_GF", 33643)
        Sounds.Add("SOUND_RYD1_GG", 33644)
        Sounds.Add("SOUND_RYD1_GH", 33645)
        Sounds.Add("SOUND_RYD1_GJ", 33646)
        Sounds.Add("SOUND_RYD1_GK", 33647)
        Sounds.Add("SOUND_RYD1_GL", 33648)
        Sounds.Add("SOUND_RYD1_HA", 33649)
        Sounds.Add("SOUND_RYD1_HB", 33650)
        Sounds.Add("SOUND_RYD1_JA", 33651)
        Sounds.Add("SOUND_RYD1_JB", 33652)
        Sounds.Add("SOUND_RYD1_KA", 33653)
        Sounds.Add("SOUND_RYD1_KB", 33654)
        Sounds.Add("SOUND_RYD1_KC", 33655)
        Sounds.Add("SOUND_RYD1_KD", 33656)
        Sounds.Add("SOUND_RYD1_KE", 33657)
        Sounds.Add("SOUND_RYD1_KF", 33658)
        Sounds.Add("SOUND_RYD1_KG", 33659)
        Sounds.Add("SOUND_RYD1_KH", 33660)
        Sounds.Add("SOUND_RYD1_LA", 33661)
        Sounds.Add("SOUND_RYD1_LB", 33662)
        Sounds.Add("SOUND_RYD1_LC", 33663)
        Sounds.Add("SOUND_RYD1_ZA", 33664)
        Sounds.Add("SOUND_RYD1_ZB", 33665)
        Sounds.Add("SOUND_RYD1_ZC", 33666)
        Sounds.Add("SOUND_RYD1_ZD", 33667)
        Sounds.Add("SOUND_RYD1_ZE", 33668)
        Sounds.Add("SOUND_RYD1_ZF", 33669)
        Sounds.Add("SOUND_RYD1_ZG", 33670)
        Sounds.Add("SOUND_RYD1_ZH", 33671)
        Sounds.Add("SOUND_RYD1_ZJ", 33672)
        Sounds.Add("SOUND_RYD1_ZK", 33673)
        Sounds.Add("SOUND_RYD1_ZL", 33674)
        Sounds.Add("SOUND_RYD1_ZM", 33675)
        Sounds.Add("SOUND_RYD1_ZN", 33676)
        Sounds.Add("SOUND_RYD2_AA", 33800)
        Sounds.Add("SOUND_RYD2_AB", 33801)
        Sounds.Add("SOUND_RYD2_BA", 33802)
        Sounds.Add("SOUND_RYD2_BB", 33803)
        Sounds.Add("SOUND_RYD2_BC", 33804)
        Sounds.Add("SOUND_RYD2_CA", 33805)
        Sounds.Add("SOUND_RYD2_CB", 33806)
        Sounds.Add("SOUND_RYD2_CC", 33807)
        Sounds.Add("SOUND_RYD2_CD", 33808)
        Sounds.Add("SOUND_RYD2_CE", 33809)
        Sounds.Add("SOUND_RYD2_CF", 33810)
        Sounds.Add("SOUND_RYD2_CG", 33811)
        Sounds.Add("SOUND_RYD2_CH", 33812)
        Sounds.Add("SOUND_RYD2_CJ", 33813)
        Sounds.Add("SOUND_RYD2_DA", 33814)
        Sounds.Add("SOUND_RYD2_DB", 33815)
        Sounds.Add("SOUND_RYD2_DC", 33816)
        Sounds.Add("SOUND_RYD2_DD", 33817)
        Sounds.Add("SOUND_RYD2_DE", 33818)
        Sounds.Add("SOUND_RYD2_DF", 33819)
        Sounds.Add("SOUND_RYD2_EA", 33820)
        Sounds.Add("SOUND_RYD2_EB", 33821)
        Sounds.Add("SOUND_RYD2_FA", 33822)
        Sounds.Add("SOUND_RYD2_GA", 33823)
        Sounds.Add("SOUND_RYD2_GB", 33824)
        Sounds.Add("SOUND_RYD2_HA", 33825)
        Sounds.Add("SOUND_RYD2_HB", 33826)
        Sounds.Add("SOUND_RYD2_JA", 33827)
        Sounds.Add("SOUND_RYD2_JB", 33828)
        Sounds.Add("SOUND_RYD2_JC", 33829)
        Sounds.Add("SOUND_RYD2_KA", 33830)
        Sounds.Add("SOUND_RYD2_KB", 33831)
        Sounds.Add("SOUND_RYD2_KC", 33832)
        Sounds.Add("SOUND_RYD2_LA", 33833)
        Sounds.Add("SOUND_RYD2_LB", 33834)
        Sounds.Add("SOUND_RYD2_LC", 33835)
        Sounds.Add("SOUND_RYD2_LD", 33836)
        Sounds.Add("SOUND_RYD2_LE", 33837)
        Sounds.Add("SOUND_RYD2_LF", 33838)
        Sounds.Add("SOUND_RYD2_LG", 33839)
        Sounds.Add("SOUND_RYD2_LH", 33840)
        Sounds.Add("SOUND_RYD2_LJ", 33841)
        Sounds.Add("SOUND_RYD2_MA", 33842)
        Sounds.Add("SOUND_RYD2_MB", 33843)
        Sounds.Add("SOUND_RYD2_MC", 33844)
        Sounds.Add("SOUND_RYD2_MD", 33845)
        Sounds.Add("SOUND_RYD2_ME", 33846)
        Sounds.Add("SOUND_RYD2_MF", 33847)
        Sounds.Add("SOUND_RYD2_NA", 33848)
        Sounds.Add("SOUND_RYD2_NB", 33849)
        Sounds.Add("SOUND_RYD2_NC", 33850)
        Sounds.Add("SOUND_RYD2_ND", 33851)
        Sounds.Add("SOUND_RYD2_OA", 33852)
        Sounds.Add("SOUND_RYD2_OB", 33853)
        Sounds.Add("SOUND_RYD2_PA", 33854)
        Sounds.Add("SOUND_RYD2_PB", 33855)
        Sounds.Add("SOUND_RYD2_PC", 33856)
        Sounds.Add("SOUND_RYD2_PD", 33857)
        Sounds.Add("SOUND_RYD2_PE", 33858)
        Sounds.Add("SOUND_RYD2_PF", 33859)
        Sounds.Add("SOUND_RYD2_PG", 33860)
        Sounds.Add("SOUND_RYD2_QA", 33861)
        Sounds.Add("SOUND_RYD2_QB", 33862)
        Sounds.Add("SOUND_RYD2_QC", 33863)
        Sounds.Add("SOUND_RYD2_RA", 33864)
        Sounds.Add("SOUND_RYD2_RB", 33865)
        Sounds.Add("SOUND_RYD2_SA", 33866)
        Sounds.Add("SOUND_RYD2_SB", 33867)
        Sounds.Add("SOUND_RYD2_SC", 33868)
        Sounds.Add("SOUND_RYD2_SD", 33869)
        Sounds.Add("SOUND_RYD2_SE", 33870)
        Sounds.Add("SOUND_RYD2_SF", 33871)
        Sounds.Add("SOUND_RYD2_SG", 33872)
        Sounds.Add("SOUND_RYD2_SH", 33873)
        Sounds.Add("SOUND_RYD2_SJ", 33874)
        Sounds.Add("SOUND_RYD2_SK", 33875)
        Sounds.Add("SOUND_RYD2_TA", 33876)
        Sounds.Add("SOUND_RYD2_TB", 33877)
        Sounds.Add("SOUND_RYD2_TC", 33878)
        Sounds.Add("SOUND_RYD2_TD", 33879)
        Sounds.Add("SOUND_RYD2_UA", 33880)
        Sounds.Add("SOUND_RYD2_UB", 33881)
        Sounds.Add("SOUND_RYD2_UC", 33882)
        Sounds.Add("SOUND_RYD2_VA", 33883)
        Sounds.Add("SOUND_RYD2_VB", 33884)
        Sounds.Add("SOUND_RYD2_VC", 33885)
        Sounds.Add("SOUND_RYD2_VD", 33886)
        Sounds.Add("SOUND_RYD2_VE", 33887)
        Sounds.Add("SOUND_RYD2_VF", 33888)
        Sounds.Add("SOUND_RYD2_VG", 33889)
        Sounds.Add("SOUND_RYD3_AA", 34000)
        Sounds.Add("SOUND_RYD3_AB", 34001)
        Sounds.Add("SOUND_RYD3_AC", 34002)
        Sounds.Add("SOUND_RYD3_AD", 34003)
        Sounds.Add("SOUND_RYD3_AE", 34004)
        Sounds.Add("SOUND_RYD3_BA", 34005)
        Sounds.Add("SOUND_RYD3_BB", 34006)
        Sounds.Add("SOUND_RYD3_BC", 34007)
        Sounds.Add("SOUND_RYD3_BD", 34008)
        Sounds.Add("SOUND_RYD3_BE", 34009)
        Sounds.Add("SOUND_RYD3_BF", 34010)
        Sounds.Add("SOUND_RYD3_BG", 34011)
        Sounds.Add("SOUND_RYD3_BH", 34012)
        Sounds.Add("SOUND_RYD3_BJ", 34013)
        Sounds.Add("SOUND_RYD3_CA", 34014)
        Sounds.Add("SOUND_RYD3_CB", 34015)
        Sounds.Add("SOUND_RYD3_CC", 34016)
        Sounds.Add("SOUND_RYD3_CD", 34017)
        Sounds.Add("SOUND_RYD3_DA", 34018)
        Sounds.Add("SOUND_RYD3_DB", 34019)
        Sounds.Add("SOUND_RYD3_DC", 34020)
        Sounds.Add("SOUND_RYD3_EA", 34021)
        Sounds.Add("SOUND_RYD3_FA", 34022)
        Sounds.Add("SOUND_RYD3_FB", 34023)
        Sounds.Add("SOUND_RYD3_GA", 34024)
        Sounds.Add("SOUND_RYD3_GB", 34025)
        Sounds.Add("SOUND_RYD3_GC", 34026)
        Sounds.Add("SOUND_RYD3_HA", 34027)
        Sounds.Add("SOUND_RYD3_HB", 34028)
        Sounds.Add("SOUND_RYD3_HC", 34029)
        Sounds.Add("SOUND_RYD3_HD", 34030)
        Sounds.Add("SOUND_RYD3_HE", 34031)
        Sounds.Add("SOUND_RYD3_HF", 34032)
        Sounds.Add("SOUND_RYD3_HG", 34033)
        Sounds.Add("SOUND_RYD3_HH", 34034)
        Sounds.Add("SOUND_RYD3_HJ", 34035)
        Sounds.Add("SOUND_RYD3_HK", 34036)
        Sounds.Add("SOUND_RYD3_HL", 34037)
        Sounds.Add("SOUND_RYD3_HM", 34038)
        Sounds.Add("SOUND_RYD3_HN", 34039)
        Sounds.Add("SOUND_RYD3_JA", 34040)
        Sounds.Add("SOUND_RYD3_JB", 34041)
        Sounds.Add("SOUND_RYD3_JC", 34042)
        Sounds.Add("SOUND_RYD3_JD", 34043)
        Sounds.Add("SOUND_RYD3_JE", 34044)
        Sounds.Add("SOUND_RYD3_JF", 34045)
        Sounds.Add("SOUND_RYD3_JG", 34046)
        Sounds.Add("SOUND_RYD3_KA", 34047)
        Sounds.Add("SOUND_RYD3_KB", 34048)
        Sounds.Add("SOUND_RYD3_LA", 34049)
        Sounds.Add("SOUND_RYD3_LB", 34050)
        Sounds.Add("SOUND_RYD3_LC", 34051)
        Sounds.Add("SOUND_RYD3_LD", 34052)
        Sounds.Add("SOUND_RYD3_LE", 34053)
        Sounds.Add("SOUND_RYD3_LF", 34054)
        Sounds.Add("SOUND_RYD3_LG", 34055)
        Sounds.Add("SOUND_RYD3_LH", 34056)
        Sounds.Add("SOUND_RYD3_MA", 34057)
        Sounds.Add("SOUND_RYD3_MB", 34058)
        Sounds.Add("SOUND_RYD3_MC", 34059)
        Sounds.Add("SOUND_RYD3_MD", 34060)
        Sounds.Add("SOUND_RYD3_ME", 34061)
        Sounds.Add("SOUND_RYD3_NA", 34062)
        Sounds.Add("SOUND_RYD3_NB", 34063)
        Sounds.Add("SOUND_RYD3_NC", 34064)
        Sounds.Add("SOUND_RYD3_OA", 34065)
        Sounds.Add("SOUND_RYD3_OB", 34066)
        Sounds.Add("SOUND_RYD3_OC", 34067)
        Sounds.Add("SOUND_RYDX_AA", 34200)
        Sounds.Add("SOUND_RYDX_AB", 34201)
        Sounds.Add("SOUND_RYDX_AC", 34202)
        Sounds.Add("SOUND_RYDX_AD", 34203)
        Sounds.Add("SOUND_RYDX_AE", 34204)
        Sounds.Add("SOUND_RYDX_AF", 34205)
        Sounds.Add("SOUND_RYDX_AG", 34206)
        Sounds.Add("SOUND_RYDX_AH", 34207)
        Sounds.Add("SOUND_RYDX_AI", 34208)
        Sounds.Add("SOUND_RYDX_AJ", 34209)
        Sounds.Add("SOUND_RYDX_AK", 34210)
        Sounds.Add("SOUND_RYDX_AL", 34211)
        Sounds.Add("SOUND_RYDX_AM", 34212)
        Sounds.Add("SOUND_RYDX_AN", 34213)
        Sounds.Add("SOUND_RYDX_AO", 34214)
        Sounds.Add("SOUND_RYDX_AP", 34215)
        Sounds.Add("SOUND_RYDX_AQ", 34216)
        Sounds.Add("SOUND_RYDX_AR", 34217)
        Sounds.Add("SOUND_RYDX_AS", 34218)
        Sounds.Add("SOUND_RYDX_AT", 34219)
        Sounds.Add("SOUND_RYDX_AU", 34220)
        Sounds.Add("SOUND_RYDX_BA", 34221)
        Sounds.Add("SOUND_RYDX_BB", 34222)
        Sounds.Add("SOUND_RYDX_BC", 34223)
        Sounds.Add("SOUND_RYDX_BD", 34224)
        Sounds.Add("SOUND_RYDX_BE", 34225)
        Sounds.Add("SOUND_RYDX_BF", 34226)
        Sounds.Add("SOUND_RYDX_BG", 34227)
        Sounds.Add("SOUND_RYDX_BH", 34228)
        Sounds.Add("SOUND_RYDX_BI", 34229)
        Sounds.Add("SOUND_RYDX_BJ", 34230)
        Sounds.Add("SOUND_RYDX_BK", 34231)
        Sounds.Add("SOUND_RYDX_BL", 34232)
        Sounds.Add("SOUND_RYDX_BM", 34233)
        Sounds.Add("SOUND_RYDX_BN", 34234)
        Sounds.Add("SOUND_RYDX_BO", 34235)
        Sounds.Add("SOUND_RYDX_BP", 34236)
        Sounds.Add("SOUND_RYDX_BQ", 34237)
        Sounds.Add("SOUND_RYDX_BR", 34238)
        Sounds.Add("SOUND_RYDX_BS", 34239)
        Sounds.Add("SOUND_RYDX_CA", 34240)
        Sounds.Add("SOUND_RYDX_CB", 34241)
        Sounds.Add("SOUND_RYDX_CC", 34242)
        Sounds.Add("SOUND_RYDX_CD", 34243)
        Sounds.Add("SOUND_RYDX_CE", 34244)
        Sounds.Add("SOUND_RYDX_CF", 34245)
        Sounds.Add("SOUND_RYDX_CG", 34246)
        Sounds.Add("SOUND_RYDX_CH", 34247)
        Sounds.Add("SOUND_RYDX_CI", 34248)
        Sounds.Add("SOUND_RYDX_CJ", 34249)
        Sounds.Add("SOUND_RYDX_CK", 34250)
        Sounds.Add("SOUND_RYDX_CL", 34251)
        Sounds.Add("SOUND_RYDX_CM", 34252)
        Sounds.Add("SOUND_RYDX_CN", 34253)
        Sounds.Add("SOUND_RYDX_CO", 34254)
        Sounds.Add("SOUND_RYDX_CP", 34255)
        Sounds.Add("SOUND_RYDX_CQ", 34256)
        Sounds.Add("SOUND_RYDX_CR", 34257)
        Sounds.Add("SOUND_RYDX_CS", 34258)
        Sounds.Add("SOUND_RYDX_CT", 34259)
        Sounds.Add("SOUND_RYDX_DA", 34260)
        Sounds.Add("SOUND_RYDX_DB", 34261)
        Sounds.Add("SOUND_RYDX_DC", 34262)
        Sounds.Add("SOUND_RYDX_DD", 34263)
        Sounds.Add("SOUND_RYDX_DE", 34264)
        Sounds.Add("SOUND_RYDX_DF", 34265)
        Sounds.Add("SOUND_RYDX_DG", 34266)
        Sounds.Add("SOUND_RYDX_DH", 34267)
        Sounds.Add("SOUND_RYDX_DI", 34268)
        Sounds.Add("SOUND_RYDX_DJ", 34269)
        Sounds.Add("SOUND_RYDX_DK", 34270)
        Sounds.Add("SOUND_RYDX_DL", 34271)
        Sounds.Add("SOUND_RYDX_DM", 34272)
        Sounds.Add("SOUND_RYDX_DN", 34273)
        Sounds.Add("SOUND_SCR1_AA", 34400)
        Sounds.Add("SOUND_SCR1_AB", 34401)
        Sounds.Add("SOUND_SCR1_AC", 34402)
        Sounds.Add("SOUND_SCR1_AD", 34403)
        Sounds.Add("SOUND_SCR1_AE", 34404)
        Sounds.Add("SOUND_SCR1_AF", 34405)
        Sounds.Add("SOUND_SCR1_AG", 34406)
        Sounds.Add("SOUND_SCR1_AH", 34407)
        Sounds.Add("SOUND_SCR1_AJ", 34408)
        Sounds.Add("SOUND_SCR1_AK", 34409)
        Sounds.Add("SOUND_SCR1_AL", 34410)
        Sounds.Add("SOUND_SCR1_AM", 34411)
        Sounds.Add("SOUND_SCR1_BA", 34412)
        Sounds.Add("SOUND_SCR1_BB", 34413)
        Sounds.Add("SOUND_SCR1_BC", 34414)
        Sounds.Add("SOUND_SCR1_BD", 34415)
        Sounds.Add("SOUND__SHOOTING_RANGE_TARGET_MOVE_LOOP", 34600)
        Sounds.Add("SOUND__SHOOTING_RANGE_TARGET_DROP", 34601)
        Sounds.Add("SOUND__SHOOTING_RANGE_TARGET_SHATTER_1", 34602)
        Sounds.Add("SOUND__SHOOTING_RANGE_TARGET_SHATTER_2", 34603)
        Sounds.Add("SOUND__SHOOTING_RANGE_TARGET_SHATTER_3", 34604)
        Sounds.Add("SOUND__SHOOTING_RANGE_TARGET_SHATTER_4", 34605)
        Sounds.Add("SOUND__SHOOTING_RANGE_TARGET_SHATTER_5", 34606)
        Sounds.Add("SOUND_SHRK10A", 34800)
        Sounds.Add("SOUND_SHRK10B", 34801)
        Sounds.Add("SOUND_SHRK11A", 34802)
        Sounds.Add("SOUND_SHRK11B", 34803)
        Sounds.Add("SOUND_SHRK12A", 34804)
        Sounds.Add("SOUND_SHRK12B", 34805)
        Sounds.Add("SOUND_SHRK13A", 34806)
        Sounds.Add("SOUND_SHRK13B", 34807)
        Sounds.Add("SOUND_SHRK14A", 34808)
        Sounds.Add("SOUND_SHRK14B", 34809)
        Sounds.Add("SOUND_SHRK15A", 34810)
        Sounds.Add("SOUND_SHRK15B", 34811)
        Sounds.Add("SOUND_SHRK16A", 34812)
        Sounds.Add("SOUND_SHRK16B", 34813)
        Sounds.Add("SOUND_SHRK_1A", 34814)
        Sounds.Add("SOUND_SHRK_1B", 34815)
        Sounds.Add("SOUND_SHRK_2A", 34816)
        Sounds.Add("SOUND_SHRK_2B", 34817)
        Sounds.Add("SOUND_SHRK_3A", 34818)
        Sounds.Add("SOUND_SHRK_3B", 34819)
        Sounds.Add("SOUND_SHRK_4A", 34820)
        Sounds.Add("SOUND_SHRK_4B", 34821)
        Sounds.Add("SOUND_SHRK_5A", 34822)
        Sounds.Add("SOUND_SHRK_5B", 34823)
        Sounds.Add("SOUND_SHRK_6A", 34824)
        Sounds.Add("SOUND_SHRK_6B", 34825)
        Sounds.Add("SOUND_SHRK_7A", 34826)
        Sounds.Add("SOUND_SHRK_7B", 34827)
        Sounds.Add("SOUND_SHRK_8A", 34828)
        Sounds.Add("SOUND_SHRK_8B", 34829)
        Sounds.Add("SOUND_SHRK_9A", 34830)
        Sounds.Add("SOUND_SHRK_9B", 34831)
        Sounds.Add("SOUND_SMO1_AA", 35000)
        Sounds.Add("SOUND_SMO1_AC", 35001)
        Sounds.Add("SOUND_SMO1_AD", 35002)
        Sounds.Add("SOUND_SMO1_AE", 35003)
        Sounds.Add("SOUND_SMO1_AF", 35004)
        Sounds.Add("SOUND_SMO1_AG", 35005)
        Sounds.Add("SOUND_SMO1_AH", 35006)
        Sounds.Add("SOUND_SMO1_AJ", 35007)
        Sounds.Add("SOUND_SMO1_BA", 35008)
        Sounds.Add("SOUND_SMO1_BB", 35009)
        Sounds.Add("SOUND_SMO1_BC", 35010)
        Sounds.Add("SOUND_SMO1_BD", 35011)
        Sounds.Add("SOUND_SMO1_BE", 35012)
        Sounds.Add("SOUND_SMO1_BF", 35013)
        Sounds.Add("SOUND_SMO1_BG", 35014)
        Sounds.Add("SOUND_SMO1_CA", 35015)
        Sounds.Add("SOUND_SMO1_CB", 35016)
        Sounds.Add("SOUND_SMO1_CC", 35017)
        Sounds.Add("SOUND_SMO1_CD", 35018)
        Sounds.Add("SOUND_SMO1_CE", 35019)
        Sounds.Add("SOUND_SMO1_CF", 35020)
        Sounds.Add("SOUND_SMO1_CG", 35021)
        Sounds.Add("SOUND_SMO1_CH", 35022)
        Sounds.Add("SOUND_SMO1_CJ", 35023)
        Sounds.Add("SOUND_SMO1_CK", 35024)
        Sounds.Add("SOUND_SMO1_CL", 35025)
        Sounds.Add("SOUND_SMO1_CM", 35026)
        Sounds.Add("SOUND_SMO1_CN", 35027)
        Sounds.Add("SOUND_SMO1_CO", 35028)
        Sounds.Add("SOUND_SMO1_CP", 35029)
        Sounds.Add("SOUND_SMO1_DA", 35030)
        Sounds.Add("SOUND_SMO1_DB", 35031)
        Sounds.Add("SOUND_SMO1_DC", 35032)
        Sounds.Add("SOUND_SMO1_EA", 35033)
        Sounds.Add("SOUND_SMO1_EB", 35034)
        Sounds.Add("SOUND_SMO1_FA", 35035)
        Sounds.Add("SOUND_SMO1_FB", 35036)
        Sounds.Add("SOUND_SMO1_FC", 35037)
        Sounds.Add("SOUND_SMO1_FD", 35038)
        Sounds.Add("SOUND_SMO1_FE", 35039)
        Sounds.Add("SOUND_SMO1_FF", 35040)
        Sounds.Add("SOUND_SMO1_FG", 35041)
        Sounds.Add("SOUND_SMO1_FH", 35042)
        Sounds.Add("SOUND_SMO1_FJ", 35043)
        Sounds.Add("SOUND_SMO1_FK", 35044)
        Sounds.Add("SOUND_SMO1_FL", 35045)
        Sounds.Add("SOUND_SMO1_FM", 35046)
        Sounds.Add("SOUND_SMO1_GA", 35047)
        Sounds.Add("SOUND_SMO1_GB", 35048)
        Sounds.Add("SOUND_SMO1_GC", 35049)
        Sounds.Add("SOUND_SMO1_GD", 35050)
        Sounds.Add("SOUND_SMO1_GE", 35051)
        Sounds.Add("SOUND_SMO1_GF", 35052)
        Sounds.Add("SOUND_SMO1_HA", 35053)
        Sounds.Add("SOUND_SMO1_HB", 35054)
        Sounds.Add("SOUND_SMO1_HC", 35055)
        Sounds.Add("SOUND_SMO1_HD", 35056)
        Sounds.Add("SOUND_SMO1_HE", 35057)
        Sounds.Add("SOUND_SMO1_HF", 35058)
        Sounds.Add("SOUND_SMO1_HG", 35059)
        Sounds.Add("SOUND_SMO1_HH", 35060)
        Sounds.Add("SOUND_SMO1_JA", 35061)
        Sounds.Add("SOUND_SMO1_JB", 35062)
        Sounds.Add("SOUND_SMO1_JC", 35063)
        Sounds.Add("SOUND_SMO1_JD", 35064)
        Sounds.Add("SOUND_SMO1_JG", 35065)
        Sounds.Add("SOUND_SMO1_JH", 35066)
        Sounds.Add("SOUND_SMO1_JJ", 35067)
        Sounds.Add("SOUND_SMO1_JK", 35068)
        Sounds.Add("SOUND_SMO1_JL", 35069)
        Sounds.Add("SOUND_SMO1_JM", 35070)
        Sounds.Add("SOUND_SMO1_KA", 35071)
        Sounds.Add("SOUND_SMO1_KB", 35072)
        Sounds.Add("SOUND_SMO1_KC", 35073)
        Sounds.Add("SOUND_SMO1_KD", 35074)
        Sounds.Add("SOUND_SMO1_KE", 35075)
        Sounds.Add("SOUND_SMO2B11", 35200)
        Sounds.Add("SOUND_SMO2_AA", 35201)
        Sounds.Add("SOUND_SMO2_AB", 35202)
        Sounds.Add("SOUND_SMO2_BA", 35203)
        Sounds.Add("SOUND_SMO2_BB", 35204)
        Sounds.Add("SOUND_SMO2_BC", 35205)
        Sounds.Add("SOUND_SMO2_BD", 35206)
        Sounds.Add("SOUND_SMO2_BE", 35207)
        Sounds.Add("SOUND_SMO2_BF", 35208)
        Sounds.Add("SOUND_SMO2_BG", 35209)
        Sounds.Add("SOUND_SMO2_BH", 35210)
        Sounds.Add("SOUND_SMO2_BJ", 35211)
        Sounds.Add("SOUND_SMO2_BK", 35212)
        Sounds.Add("SOUND_SMO2_BL", 35213)
        Sounds.Add("SOUND_SMO2_CA", 35214)
        Sounds.Add("SOUND_SMO2_CB", 35215)
        Sounds.Add("SOUND_SMO2_CC", 35216)
        Sounds.Add("SOUND_SMO2_DA", 35217)
        Sounds.Add("SOUND_SMO2_DB", 35218)
        Sounds.Add("SOUND_SMO2_DC", 35219)
        Sounds.Add("SOUND_SMO2_EA", 35220)
        Sounds.Add("SOUND_SMO2_FA", 35221)
        Sounds.Add("SOUND_SMO2_FB", 35222)
        Sounds.Add("SOUND_SMO2_FC", 35223)
        Sounds.Add("SOUND_SMO2_FD", 35224)
        Sounds.Add("SOUND_SMO2_GA", 35225)
        Sounds.Add("SOUND_SMO2_GB", 35226)
        Sounds.Add("SOUND_SMO2_GC", 35227)
        Sounds.Add("SOUND_SMO2_GD", 35228)
        Sounds.Add("SOUND_SMO2_GE", 35229)
        Sounds.Add("SOUND_SMO2_GF", 35230)
        Sounds.Add("SOUND_SMO2_GG", 35231)
        Sounds.Add("SOUND_SMO2_GH", 35232)
        Sounds.Add("SOUND_SMO2_GJ", 35233)
        Sounds.Add("SOUND_SMO2_GK", 35234)
        Sounds.Add("SOUND_SMO2_GL", 35235)
        Sounds.Add("SOUND_SMO2_HA", 35236)
        Sounds.Add("SOUND_SMO2_HB", 35237)
        Sounds.Add("SOUND_SMO2_HC", 35238)
        Sounds.Add("SOUND_SMO2_HD", 35239)
        Sounds.Add("SOUND_SMO2_HE", 35240)
        Sounds.Add("SOUND_SMO3_AA", 35400)
        Sounds.Add("SOUND_SMO3_AB", 35401)
        Sounds.Add("SOUND_SMO3_AC", 35402)
        Sounds.Add("SOUND_SMO3_AD", 35403)
        Sounds.Add("SOUND_SMO3_AE", 35404)
        Sounds.Add("SOUND_SMO3_AF", 35405)
        Sounds.Add("SOUND_SMO3_AG", 35406)
        Sounds.Add("SOUND_SMO3_AH", 35407)
        Sounds.Add("SOUND_SMO3_AJ", 35408)
        Sounds.Add("SOUND_SMO3_AK", 35409)
        Sounds.Add("SOUND_SMO3_AL", 35410)
        Sounds.Add("SOUND_SMO3_AM", 35411)
        Sounds.Add("SOUND_SMO3_BA", 35412)
        Sounds.Add("SOUND_SMO3_BB", 35413)
        Sounds.Add("SOUND_SMO3_BC", 35414)
        Sounds.Add("SOUND_SMO3_BD", 35415)
        Sounds.Add("SOUND_SMO3_BE", 35416)
        Sounds.Add("SOUND_SMO3_BF", 35417)
        Sounds.Add("SOUND_SMO3_BG", 35418)
        Sounds.Add("SOUND_SMO3_BH", 35419)
        Sounds.Add("SOUND_SMO3_CA", 35420)
        Sounds.Add("SOUND_SMO3_CB", 35421)
        Sounds.Add("SOUND_SMO3_CC", 35422)
        Sounds.Add("SOUND_SMO3_DA", 35423)
        Sounds.Add("SOUND_SMO3_DB", 35424)
        Sounds.Add("SOUND_SMO3_DC", 35425)
        Sounds.Add("SOUND_SMO3_DD", 35426)
        Sounds.Add("SOUND_SMO3_DE", 35427)
        Sounds.Add("SOUND_SMO3_EA", 35428)
        Sounds.Add("SOUND_SMO3_EB", 35429)
        Sounds.Add("SOUND_SMO3_EC", 35430)
        Sounds.Add("SOUND_SMO3_FA", 35431)
        Sounds.Add("SOUND_SMO3_FB", 35432)
        Sounds.Add("SOUND_SMO3_FC", 35433)
        Sounds.Add("SOUND_SMO3_GA", 35434)
        Sounds.Add("SOUND_SMO3_GB", 35435)
        Sounds.Add("SOUND_SMO3_GC", 35436)
        Sounds.Add("SOUND_SMO3_HA", 35437)
        Sounds.Add("SOUND_SMO3_HB", 35438)
        Sounds.Add("SOUND_SMO3_HC", 35439)
        Sounds.Add("SOUND_SMO3_JA", 35440)
        Sounds.Add("SOUND_SMO3_JB", 35441)
        Sounds.Add("SOUND_SMO3_JC", 35442)
        Sounds.Add("SOUND_SMO3_KA", 35443)
        Sounds.Add("SOUND_SMO3_KB", 35444)
        Sounds.Add("SOUND_SMO3_KC", 35445)
        Sounds.Add("SOUND_SMO3_LA", 35446)
        Sounds.Add("SOUND_SMO3_LB", 35447)
        Sounds.Add("SOUND_SMO3_LC", 35448)
        Sounds.Add("SOUND_SMO3_MA", 35449)
        Sounds.Add("SOUND_SMO3_MB", 35450)
        Sounds.Add("SOUND_SMO3_MC", 35451)
        Sounds.Add("SOUND_SMO3_NA", 35452)
        Sounds.Add("SOUND_SMO3_NB", 35453)
        Sounds.Add("SOUND_SMO3_NC", 35454)
        Sounds.Add("SOUND_SMO3_OA", 35455)
        Sounds.Add("SOUND_SMO3_OB", 35456)
        Sounds.Add("SOUND_SMO3_OC", 35457)
        Sounds.Add("SOUND_SMO3_PA", 35458)
        Sounds.Add("SOUND_SMO3_PB", 35459)
        Sounds.Add("SOUND_SMO3_PC", 35460)
        Sounds.Add("SOUND_SMO3_QA", 35461)
        Sounds.Add("SOUND_SMO3_QB", 35462)
        Sounds.Add("SOUND_SMO3_QC", 35463)
        Sounds.Add("SOUND_SMO3_QD", 35464)
        Sounds.Add("SOUND_SMO3_QE", 35465)
        Sounds.Add("SOUND_SMO3_QF", 35466)
        Sounds.Add("SOUND_SMO3_QG", 35467)
        Sounds.Add("SOUND_SMO3_QH", 35468)
        Sounds.Add("SOUND_SMO3_QJ", 35469)
        Sounds.Add("SOUND_SMO3_QK", 35470)
        Sounds.Add("SOUND_SMO3_QL", 35471)
        Sounds.Add("SOUND_SMO3_QM", 35472)
        Sounds.Add("SOUND_SMO3_QN", 35473)
        Sounds.Add("SOUND_SMO3_QO", 35474)
        Sounds.Add("SOUND_SMO3_QP", 35475)
        Sounds.Add("SOUND_SMO3_QR", 35476)
        Sounds.Add("SOUND_SMO3_RA", 35477)
        Sounds.Add("SOUND_SMO3_RB", 35478)
        Sounds.Add("SOUND_SMO3_RC", 35479)
        Sounds.Add("SOUND_SMO3_RD", 35480)
        Sounds.Add("SOUND_SMO3_RE", 35481)
        Sounds.Add("SOUND_SMO3_RF", 35482)
        Sounds.Add("SOUND_SMO3_RG", 35483)
        Sounds.Add("SOUND_SMO3_RH", 35484)
        Sounds.Add("SOUND_SMO3_SA", 35485)
        Sounds.Add("SOUND_SMO3_SB", 35486)
        Sounds.Add("SOUND_SMO3_SC", 35487)
        Sounds.Add("SOUND_SMO3_SD", 35488)
        Sounds.Add("SOUND_SMO4_AA", 35600)
        Sounds.Add("SOUND_SMO4_AB", 35601)
        Sounds.Add("SOUND_SMO4_AC", 35602)
        Sounds.Add("SOUND_SMO4_AD", 35603)
        Sounds.Add("SOUND_SMO4_AE", 35604)
        Sounds.Add("SOUND_SMO4_AF", 35605)
        Sounds.Add("SOUND_SMO4_AG", 35606)
        Sounds.Add("SOUND_SMO4_AH", 35607)
        Sounds.Add("SOUND_SMO4_AJ", 35608)
        Sounds.Add("SOUND_SMO4_AK", 35609)
        Sounds.Add("SOUND_SMO4_AL", 35610)
        Sounds.Add("SOUND_SMO4_AM", 35611)
        Sounds.Add("SOUND_SMO4_AN", 35612)
        Sounds.Add("SOUND_SMO4_AO", 35613)
        Sounds.Add("SOUND_SMO4_BA", 35614)
        Sounds.Add("SOUND_SMO4_BB", 35615)
        Sounds.Add("SOUND_SMO4_BC", 35616)
        Sounds.Add("SOUND_SMO4_BD", 35617)
        Sounds.Add("SOUND_SMO4_BE", 35618)
        Sounds.Add("SOUND_SMO4_BF", 35619)
        Sounds.Add("SOUND_SMO4_BG", 35620)
        Sounds.Add("SOUND_SMO4_BH", 35621)
        Sounds.Add("SOUND_SMO4_BJ", 35622)
        Sounds.Add("SOUND_SMO4_BK", 35623)
        Sounds.Add("SOUND_SMO4_BL", 35624)
        Sounds.Add("SOUND_SMO4_BM", 35625)
        Sounds.Add("SOUND_SMO4_BN", 35626)
        Sounds.Add("SOUND_SMO4_BO", 35627)
        Sounds.Add("SOUND_SMO4_BP", 35628)
        Sounds.Add("SOUND_SMO4_BQ", 35629)
        Sounds.Add("SOUND_SMO4_BR", 35630)
        Sounds.Add("SOUND_SMO4_CA", 35631)
        Sounds.Add("SOUND_SMO4_CB", 35632)
        Sounds.Add("SOUND_SMO4_CC", 35633)
        Sounds.Add("SOUND_SMO4_CD", 35634)
        Sounds.Add("SOUND_SMO4_CE", 35635)
        Sounds.Add("SOUND_SMO4_CF", 35636)
        Sounds.Add("SOUND_SMO4_DA", 35637)
        Sounds.Add("SOUND_SMO4_DB", 35638)
        Sounds.Add("SOUND_SMO4_DC", 35639)
        Sounds.Add("SOUND_SMO4_DE", 35640)
        Sounds.Add("SOUND_SMO4_DF", 35641)
        Sounds.Add("SOUND_SMO4_DG", 35642)
        Sounds.Add("SOUND_SMO4_DH", 35643)
        Sounds.Add("SOUND_SMO4_DJ", 35644)
        Sounds.Add("SOUND_SMO4_EA", 35645)
        Sounds.Add("SOUND_SMO4_EB", 35646)
        Sounds.Add("SOUND_SMO4_EC", 35647)
        Sounds.Add("SOUND_SMO4_ED", 35648)
        Sounds.Add("SOUND_SMO4_EE", 35649)
        Sounds.Add("SOUND_SMO4_EF", 35650)
        Sounds.Add("SOUND_SMO4_EG", 35651)
        Sounds.Add("SOUND_SMO4_EH", 35652)
        Sounds.Add("SOUND_SMO4_EJ", 35653)
        Sounds.Add("SOUND_SMO4_EK", 35654)
        Sounds.Add("SOUND_SMO4_EL", 35655)
        Sounds.Add("SOUND_SMO4_EM", 35656)
        Sounds.Add("SOUND_SMO4_EN", 35657)
        Sounds.Add("SOUND_SMO4_EO", 35658)
        Sounds.Add("SOUND_SMO4_EP", 35659)
        Sounds.Add("SOUND_SMO4_EQ", 35660)
        Sounds.Add("SOUND_SMO4_ER", 35661)
        Sounds.Add("SOUND_SMO4_ES", 35662)
        Sounds.Add("SOUND_SMO4_FA", 35663)
        Sounds.Add("SOUND_SMO4_FB", 35664)
        Sounds.Add("SOUND_SMO4_FC", 35665)
        Sounds.Add("SOUND_SMO4_FD", 35666)
        Sounds.Add("SOUND_SMO4_FE", 35667)
        Sounds.Add("SOUND_SMO4_FF", 35668)
        Sounds.Add("SOUND_SMO4_FG", 35669)
        Sounds.Add("SOUND_SMO4_FH", 35670)
        Sounds.Add("SOUND_SMO4_FJ", 35671)
        Sounds.Add("SOUND_SMO4_FK", 35672)
        Sounds.Add("SOUND_SMO4_FL", 35673)
        Sounds.Add("SOUND_SMO4_FM", 35674)
        Sounds.Add("SOUND_SMO4_GA", 35675)
        Sounds.Add("SOUND_SMO4_GB", 35676)
        Sounds.Add("SOUND_SMO4_GD", 35677)
        Sounds.Add("SOUND_SMO4_GE", 35678)
        Sounds.Add("SOUND_SMO4_GF", 35679)
        Sounds.Add("SOUND_SMO4_GG", 35680)
        Sounds.Add("SOUND_SMO4_GH", 35681)
        Sounds.Add("SOUND_SMO4_GJ", 35682)
        Sounds.Add("SOUND_SMO4_GK", 35683)
        Sounds.Add("SOUND_SMO4_GL", 35684)
        Sounds.Add("SOUND_SMO4_GM", 35685)
        Sounds.Add("SOUND_SMO4_GN", 35686)
        Sounds.Add("SOUND_SMO4_GO", 35687)
        Sounds.Add("SOUND_SMO4_GP", 35688)
        Sounds.Add("SOUND_SMO4_HA", 35689)
        Sounds.Add("SOUND_SMO4_HB", 35690)
        Sounds.Add("SOUND_SMO4_HC", 35691)
        Sounds.Add("SOUND_SMO4_HD", 35692)
        Sounds.Add("SOUND_SMO4_HE", 35693)
        Sounds.Add("SOUND_SMO4_HF", 35694)
        Sounds.Add("SOUND_SMO4_HH", 35695)
        Sounds.Add("SOUND_SMO4_HJ", 35696)
        Sounds.Add("SOUND_SMO4_HK", 35697)
        Sounds.Add("SOUND_SMO4_HL", 35698)
        Sounds.Add("SOUND_SMO4_HM", 35699)
        Sounds.Add("SOUND_SMO4_HN", 35700)
        Sounds.Add("SOUND_SMO4_HO", 35701)
        Sounds.Add("SOUND_SMO4_HP", 35702)
        Sounds.Add("SOUND_SMO4_HQ", 35703)
        Sounds.Add("SOUND_SMO4_HR", 35704)
        Sounds.Add("SOUND_SMO4_HS", 35705)
        Sounds.Add("SOUND_SMO4_HT", 35706)
        Sounds.Add("SOUND_SMO4_HU", 35707)
        Sounds.Add("SOUND_SMO4_HV", 35708)
        Sounds.Add("SOUND_SMO4_JA", 35709)
        Sounds.Add("SOUND_SMO4_JB", 35710)
        Sounds.Add("SOUND_SMO4_JC", 35711)
        Sounds.Add("SOUND_SMO4_JD", 35712)
        Sounds.Add("SOUND_SMO4_JE", 35713)
        Sounds.Add("SOUND_SMO4_JF", 35714)
        Sounds.Add("SOUND_SMO4_JG", 35715)
        Sounds.Add("SOUND_SMO4_JH", 35716)
        Sounds.Add("SOUND_SMO4_JJ", 35717)
        Sounds.Add("SOUND_SMO4_JK", 35718)
        Sounds.Add("SOUND_SMO4_JL", 35719)
        Sounds.Add("SOUND_SMO4_JM", 35720)
        Sounds.Add("SOUND_SMO4_JN", 35721)
        Sounds.Add("SOUND_SMO4_JO", 35722)
        Sounds.Add("SOUND_SMO4_JP", 35723)
        Sounds.Add("SOUND_SMO4_JQ", 35724)
        Sounds.Add("SOUND_SMO4_JR", 35725)
        Sounds.Add("SOUND_SMO4_JS", 35726)
        Sounds.Add("SOUND_SMO4_KA", 35727)
        Sounds.Add("SOUND_SMO4_KB", 35728)
        Sounds.Add("SOUND_SMO4_KC", 35729)
        Sounds.Add("SOUND_SMO4_KD", 35730)
        Sounds.Add("SOUND_SMO4_KE", 35731)
        Sounds.Add("SOUND_SMO4_KF", 35732)
        Sounds.Add("SOUND_SMO4_KG", 35733)
        Sounds.Add("SOUND_SMOX_AA", 35800)
        Sounds.Add("SOUND_SMOX_AB", 35801)
        Sounds.Add("SOUND_SMOX_AC", 35802)
        Sounds.Add("SOUND_SMOX_AD", 35803)
        Sounds.Add("SOUND_SMOX_AE", 35804)
        Sounds.Add("SOUND_SMOX_AF", 35805)
        Sounds.Add("SOUND_SMOX_AG", 35806)
        Sounds.Add("SOUND_SMOX_AH", 35807)
        Sounds.Add("SOUND_SMOX_AI", 35808)
        Sounds.Add("SOUND_SMOX_AJ", 35809)
        Sounds.Add("SOUND_SMOX_AK", 35810)
        Sounds.Add("SOUND_SMOX_AL", 35811)
        Sounds.Add("SOUND_SMOX_AM", 35812)
        Sounds.Add("SOUND_SMOX_AN", 35813)
        Sounds.Add("SOUND_SMOX_AO", 35814)
        Sounds.Add("SOUND_SMOX_AP", 35815)
        Sounds.Add("SOUND_SMOX_AQ", 35816)
        Sounds.Add("SOUND_SMOX_AR", 35817)
        Sounds.Add("SOUND_SMOX_AS", 35818)
        Sounds.Add("SOUND_SMOX_AT", 35819)
        Sounds.Add("SOUND_SMOX_AU", 35820)
        Sounds.Add("SOUND_SMOX_AV", 35821)
        Sounds.Add("SOUND_SMOX_AW", 35822)
        Sounds.Add("SOUND_SMOX_AX", 35823)
        Sounds.Add("SOUND_SMOX_AY", 35824)
        Sounds.Add("SOUND_SMOX_AZ", 35825)
        Sounds.Add("SOUND_SMOX_BA", 35826)
        Sounds.Add("SOUND_SMOX_BB", 35827)
        Sounds.Add("SOUND_SMOX_BC", 35828)
        Sounds.Add("SOUND_SMOX_BD", 35829)
        Sounds.Add("SOUND_SMOX_BE", 35830)
        Sounds.Add("SOUND_SMOX_BF", 35831)
        Sounds.Add("SOUND_SMOX_BG", 35832)
        Sounds.Add("SOUND_SMOX_BH", 35833)
        Sounds.Add("SOUND_SMOX_BI", 35834)
        Sounds.Add("SOUND_SMOX_BJ", 35835)
        Sounds.Add("SOUND_SMOX_BK", 35836)
        Sounds.Add("SOUND_SMOX_BL", 35837)
        Sounds.Add("SOUND_SMOX_BM", 35838)
        Sounds.Add("SOUND_SMOX_BN", 35839)
        Sounds.Add("SOUND_SMOX_BO", 35840)
        Sounds.Add("SOUND_SMOX_BP", 35841)
        Sounds.Add("SOUND_SMOX_BQ", 35842)
        Sounds.Add("SOUND_SMOX_BR", 35843)
        Sounds.Add("SOUND_SMOX_BS", 35844)
        Sounds.Add("SOUND_SMOX_BT", 35845)
        Sounds.Add("SOUND_SMOX_BU", 35846)
        Sounds.Add("SOUND_SMOX_BV", 35847)
        Sounds.Add("SOUND_SMOX_BW", 35848)
        Sounds.Add("SOUND_SMOX_BX", 35849)
        Sounds.Add("SOUND_SMOX_BY", 35850)
        Sounds.Add("SOUND_SMOX_BZ", 35851)
        Sounds.Add("SOUND_SMOX_CA", 35852)
        Sounds.Add("SOUND_SMOX_CB", 35853)
        Sounds.Add("SOUND_SMOX_CC", 35854)
        Sounds.Add("SOUND_SMOX_CD", 35855)
        Sounds.Add("SOUND_SMOX_CE", 35856)
        Sounds.Add("SOUND_SMOX_CF", 35857)
        Sounds.Add("SOUND_SMOX_CG", 35858)
        Sounds.Add("SOUND_SMOX_CH", 35859)
        Sounds.Add("SOUND_SMOX_CI", 35860)
        Sounds.Add("SOUND_SMOX_CJ", 35861)
        Sounds.Add("SOUND_SMOX_CK", 35862)
        Sounds.Add("SOUND_SMOX_CL", 35863)
        Sounds.Add("SOUND_SMOX_CM", 35864)
        Sounds.Add("SOUND_SMOX_CN", 35865)
        Sounds.Add("SOUND_SMOX_CO", 35866)
        Sounds.Add("SOUND_SMOX_CP", 35867)
        Sounds.Add("SOUND_SMOX_CQ", 35868)
        Sounds.Add("SOUND_SMOX_CR", 35869)
        Sounds.Add("SOUND_SMOX_CS", 35870)
        Sounds.Add("SOUND_SMOX_CT", 35871)
        Sounds.Add("SOUND_SMOX_CU", 35872)
        Sounds.Add("SOUND_SMOX_CV", 35873)
        Sounds.Add("SOUND_SMOX_CW", 35874)
        Sounds.Add("SOUND_SMOX_CX", 35875)
        Sounds.Add("SOUND_SMOX_CY", 35876)
        Sounds.Add("SOUND_SMOX_CZ", 35877)
        Sounds.Add("SOUND_SMOX_DA", 35878)
        Sounds.Add("SOUND_SMOX_DB", 35879)
        Sounds.Add("SOUND_SMOX_DC", 35880)
        Sounds.Add("SOUND_SMOX_DE", 35881)
        Sounds.Add("SOUND_SMOX_DF", 35882)
        Sounds.Add("SOUND_SMOX_DG", 35883)
        Sounds.Add("SOUND_SOLO_AA", 36000)
        Sounds.Add("SOUND_CROWD_AWWS", 36200)
        Sounds.Add("SOUND_CROWD_AWWS2", 36201)
        Sounds.Add("SOUND_CROWD_CHEERS", 36202)
        Sounds.Add("SOUND_CROWD_CHEERS2", 36203)
        Sounds.Add("SOUND_CROWD_CHEERS3", 36204)
        Sounds.Add("SOUND_CROWD_CHEERS_BIG", 36205)
        Sounds.Add("SOUND__STINGER_FIRE", 36400)
        Sounds.Add("SOUND__STINGER_RELOAD", 36401)
        Sounds.Add("SOUND_STL1_AA", 36600)
        Sounds.Add("SOUND_STL1_AB", 36601)
        Sounds.Add("SOUND_STL1_AC", 36602)
        Sounds.Add("SOUND_STL1_AD", 36603)
        Sounds.Add("SOUND_STL1_AE", 36604)
        Sounds.Add("SOUND_STL2_AA", 36800)
        Sounds.Add("SOUND_STL2_BA", 36801)
        Sounds.Add("SOUND_STL2_BB", 36802)
        Sounds.Add("SOUND_STL2_BC", 36803)
        Sounds.Add("SOUND_STL2_BD", 36804)
        Sounds.Add("SOUND_STL2_BE", 36805)
        Sounds.Add("SOUND_STL2_BF", 36806)
        Sounds.Add("SOUND_STL2_BG", 36807)
        Sounds.Add("SOUND_STL2_BH", 36808)
        Sounds.Add("SOUND_STL2_BJ", 36809)
        Sounds.Add("SOUND_STL2_BK", 36810)
        Sounds.Add("SOUND_STL2_BL", 36811)
        Sounds.Add("SOUND_STL2_BM", 36812)
        Sounds.Add("SOUND_STL2_CA", 36813)
        Sounds.Add("SOUND_STL2_CB", 36814)
        Sounds.Add("SOUND_STL2_CC", 36815)
        Sounds.Add("SOUND_STL2_CD", 36816)
        Sounds.Add("SOUND_STL2_CE", 36817)
        Sounds.Add("SOUND_STL2_CF", 36818)
        Sounds.Add("SOUND_STL2_CG", 36819)
        Sounds.Add("SOUND_STL2_DA", 36820)
        Sounds.Add("SOUND_STL2_DB", 36821)
        Sounds.Add("SOUND_STL2_DC", 36822)
        Sounds.Add("SOUND_STL2_DD", 36823)
        Sounds.Add("SOUND_STL2_DE", 36824)
        Sounds.Add("SOUND_STL2_DF", 36825)
        Sounds.Add("SOUND_STL2_DH", 36826)
        Sounds.Add("SOUND_STL2_DJ", 36827)
        Sounds.Add("SOUND_STL2_DK", 36828)
        Sounds.Add("SOUND_STL2_EA", 36829)
        Sounds.Add("SOUND_STL2_EB", 36830)
        Sounds.Add("SOUND_STL2_EC", 36831)
        Sounds.Add("SOUND_STL2_ED", 36832)
        Sounds.Add("SOUND_STL2_EE", 36833)
        Sounds.Add("SOUND_STL2_EF", 36834)
        Sounds.Add("SOUND_STL2_FA", 36835)
        Sounds.Add("SOUND_STL2_FB", 36836)
        Sounds.Add("SOUND_STL2_FC", 36837)
        Sounds.Add("SOUND_STL2_FD", 36838)
        Sounds.Add("SOUND_STL2_FE", 36839)
        Sounds.Add("SOUND_STL2_FF", 36840)
        Sounds.Add("SOUND_STL2_FG", 36841)
        Sounds.Add("SOUND_STL2_FH", 36842)
        Sounds.Add("SOUND_STL2_FJ", 36843)
        Sounds.Add("SOUND_STL2_FK", 36844)
        Sounds.Add("SOUND_STL2_FL", 36845)
        Sounds.Add("SOUND_STL2_FM", 36846)
        Sounds.Add("SOUND_STL2_FN", 36847)
        Sounds.Add("SOUND_STL2_FO", 36848)
        Sounds.Add("SOUND_STL2_FP", 36849)
        Sounds.Add("SOUND_STL2_FQ", 36850)
        Sounds.Add("SOUND_STL2_FR", 36851)
        Sounds.Add("SOUND_STL2_GA", 36852)
        Sounds.Add("SOUND_STL2_GB", 36853)
        Sounds.Add("SOUND_STL2_HA", 36854)
        Sounds.Add("SOUND_STL2_HB", 36855)
        Sounds.Add("SOUND_STL2_HC", 36856)
        Sounds.Add("SOUND_STL2_HD", 36857)
        Sounds.Add("SOUND_STL2_HE", 36858)
        Sounds.Add("SOUND_STL2_HF", 36859)
        Sounds.Add("SOUND_STL2_HG", 36860)
        Sounds.Add("SOUND_STL4_AA", 37000)
        Sounds.Add("SOUND_STL4_AB", 37001)
        Sounds.Add("SOUND_STL4_BA", 37002)
        Sounds.Add("SOUND_STL4_BB", 37003)
        Sounds.Add("SOUND_STL4_BC", 37004)
        Sounds.Add("SOUND_STL4_BD", 37005)
        Sounds.Add("SOUND_STL4_CA", 37006)
        Sounds.Add("SOUND_STL4_CB", 37007)
        Sounds.Add("SOUND_STL4_CC", 37008)
        Sounds.Add("SOUND_STL4_DA", 37009)
        Sounds.Add("SOUND_STL4_DB", 37010)
        Sounds.Add("SOUND_STL4_EA", 37011)
        Sounds.Add("SOUND_STL4_FA", 37012)
        Sounds.Add("SOUND_STL4_FB", 37013)
        Sounds.Add("SOUND_STL4_FC", 37014)
        Sounds.Add("SOUND_STL4_GA", 37015)
        Sounds.Add("SOUND_STL4_GB", 37016)
        Sounds.Add("SOUND_STL4_GC", 37017)
        Sounds.Add("SOUND_STL4_HA", 37018)
        Sounds.Add("SOUND_STL4_HB", 37019)
        Sounds.Add("SOUND_STL4_HC", 37020)
        Sounds.Add("SOUND_STL4_JA", 37021)
        Sounds.Add("SOUND_STL4_JB", 37022)
        Sounds.Add("SOUND_STL4_KA", 37023)
        Sounds.Add("SOUND_STL4_KB", 37024)
        Sounds.Add("SOUND_STL4_LA", 37025)
        Sounds.Add("SOUND_STL4_LB", 37026)
        Sounds.Add("SOUND_STL4_LC", 37027)
        Sounds.Add("SOUND_STL4_LD", 37028)
        Sounds.Add("SOUND_STL4_LE", 37029)
        Sounds.Add("SOUND_STL4_MA", 37030)
        Sounds.Add("SOUND_STL4_MB", 37031)
        Sounds.Add("SOUND_STL4_MC", 37032)
        Sounds.Add("SOUND_STL4_MD", 37033)
        Sounds.Add("SOUND_STL4_NA", 37034)
        Sounds.Add("SOUND_STL4_NB", 37035)
        Sounds.Add("SOUND_STL5_AA", 37200)
        Sounds.Add("SOUND_STL5_AB", 37201)
        Sounds.Add("SOUND_STL5_AC", 37202)
        Sounds.Add("SOUND_STL5_AD", 37203)
        Sounds.Add("SOUND_STL5_AE", 37204)
        Sounds.Add("SOUND_STL5_AF", 37205)
        Sounds.Add("SOUND_STL5_AG", 37206)
        Sounds.Add("SOUND_STL5_BA", 37207)
        Sounds.Add("SOUND_STL5_BB", 37208)
        Sounds.Add("SOUND_STL5_BC", 37209)
        Sounds.Add("SOUND_STL5_BD", 37210)
        Sounds.Add("SOUND_STL5_BE", 37211)
        Sounds.Add("SOUND_STL5_BF", 37212)
        Sounds.Add("SOUND_STL5_BG", 37213)
        Sounds.Add("SOUND_STL5_BL", 37214)
        Sounds.Add("SOUND_STL5_CA", 37215)
        Sounds.Add("SOUND_STL5_CF", 37216)
        Sounds.Add("SOUND_STL5_DA", 37217)
        Sounds.Add("SOUND_STL5_DB", 37218)
        Sounds.Add("SOUND_STL5_DC", 37219)
        Sounds.Add("SOUND_STL5_DD", 37220)
        Sounds.Add("SOUND_STL5_DE", 37221)
        Sounds.Add("SOUND_STL5_DF", 37222)
        Sounds.Add("SOUND_STL5_ED", 37223)
        Sounds.Add("SOUND_STL5_EE", 37224)
        Sounds.Add("SOUND_STL5_FA", 37225)
        Sounds.Add("SOUND_STL5_FC", 37226)
        Sounds.Add("SOUND_STL5_FD", 37227)
        Sounds.Add("SOUND_STL5_FE", 37228)
        Sounds.Add("SOUND_STL5_FF", 37229)
        Sounds.Add("SOUND_STL5_FG", 37230)
        Sounds.Add("SOUND_STL5_FH", 37231)
        Sounds.Add("SOUND_STL5_FJ", 37232)
        Sounds.Add("SOUND_STL5_FK", 37233)
        Sounds.Add("SOUND_STL5_FN", 37234)
        Sounds.Add("SOUND_STL5_FO", 37235)
        Sounds.Add("SOUND_STL5_FP", 37236)
        Sounds.Add("SOUND_STL5_FQ", 37237)
        Sounds.Add("SOUND_STL5_FR", 37238)
        Sounds.Add("SOUND_STL5_HA", 37239)
        Sounds.Add("SOUND_STL5_HB", 37240)
        Sounds.Add("SOUND_STL5_HC", 37241)
        Sounds.Add("SOUND_STL5_HD", 37242)
        Sounds.Add("SOUND_STL5_HE", 37243)
        Sounds.Add("SOUND_STL5_HF", 37244)
        Sounds.Add("SOUND_STL5_HG", 37245)
        Sounds.Add("SOUND_SWE1_AA", 37400)
        Sounds.Add("SOUND_SWE1_AB", 37401)
        Sounds.Add("SOUND_SWE1_AC", 37402)
        Sounds.Add("SOUND_SWE1_AD", 37403)
        Sounds.Add("SOUND_SWE1_AE", 37404)
        Sounds.Add("SOUND_SWE1_AF", 37405)
        Sounds.Add("SOUND_SWE1_AG", 37406)
        Sounds.Add("SOUND_SWE1_AH", 37407)
        Sounds.Add("SOUND_SWE1_AJ", 37408)
        Sounds.Add("SOUND_SWE1_AK", 37409)
        Sounds.Add("SOUND_SWE1_AL", 37410)
        Sounds.Add("SOUND_SWE1_AM", 37411)
        Sounds.Add("SOUND_SWE1_AN", 37412)
        Sounds.Add("SOUND_SWE1_AO", 37413)
        Sounds.Add("SOUND_SWE1_AP", 37414)
        Sounds.Add("SOUND_SWE1_AQ", 37415)
        Sounds.Add("SOUND_SWE1_AR", 37416)
        Sounds.Add("SOUND_SWE1_AS", 37417)
        Sounds.Add("SOUND_SWE1_AT", 37418)
        Sounds.Add("SOUND_SWE1_AU", 37419)
        Sounds.Add("SOUND_SWE1_AV", 37420)
        Sounds.Add("SOUND_SWE1_AW", 37421)
        Sounds.Add("SOUND_SWE1_AX", 37422)
        Sounds.Add("SOUND_SWE1_BA", 37423)
        Sounds.Add("SOUND_SWE1_BB", 37424)
        Sounds.Add("SOUND_SWE1_BC", 37425)
        Sounds.Add("SOUND_SWE1_BD", 37426)
        Sounds.Add("SOUND_SWE1_BE", 37427)
        Sounds.Add("SOUND_SWE1_BF", 37428)
        Sounds.Add("SOUND_SWE1_BG", 37429)
        Sounds.Add("SOUND_SWE1_BH", 37430)
        Sounds.Add("SOUND_SWE1_BJ", 37431)
        Sounds.Add("SOUND_SWE1_BK", 37432)
        Sounds.Add("SOUND_SWE1_BL", 37433)
        Sounds.Add("SOUND_SWE1_BM", 37434)
        Sounds.Add("SOUND_SWE1_BN", 37435)
        Sounds.Add("SOUND_SWE1_BO", 37436)
        Sounds.Add("SOUND_SWE1_BP", 37437)
        Sounds.Add("SOUND_SWE1_BQ", 37438)
        Sounds.Add("SOUND_SWE1_BR", 37439)
        Sounds.Add("SOUND_SWE1_BS", 37440)
        Sounds.Add("SOUND_SWE1_BT", 37441)
        Sounds.Add("SOUND_SWE1_BU", 37442)
        Sounds.Add("SOUND_SWE1_BX", 37443)
        Sounds.Add("SOUND_SWE1_CA", 37444)
        Sounds.Add("SOUND_SWE1_CB", 37445)
        Sounds.Add("SOUND_SWE1_SA", 37446)
        Sounds.Add("SOUND_SWE1_SB", 37447)
        Sounds.Add("SOUND_SWE1_TA", 37448)
        Sounds.Add("SOUND_SWE1_TB", 37449)
        Sounds.Add("SOUND_SWE1_TC", 37450)
        Sounds.Add("SOUND_SWE1_TD", 37451)
        Sounds.Add("SOUND_SWE1_TE", 37452)
        Sounds.Add("SOUND_SWE1_TF", 37453)
        Sounds.Add("SOUND_SWE1_UA", 37454)
        Sounds.Add("SOUND_SWE1_UB", 37455)
        Sounds.Add("SOUND_SWE1_UC", 37456)
        Sounds.Add("SOUND_SWE1_VA", 37457)
        Sounds.Add("SOUND_SWE1_VB", 37458)
        Sounds.Add("SOUND_SWE1_VC", 37459)
        Sounds.Add("SOUND_SWE1_WA", 37460)
        Sounds.Add("SOUND_SWE1_WB", 37461)
        Sounds.Add("SOUND_SWE1_WC", 37462)
        Sounds.Add("SOUND_SWE1_XA", 37463)
        Sounds.Add("SOUND_SWE1_XB", 37464)
        Sounds.Add("SOUND_SWE1_YA", 37465)
        Sounds.Add("SOUND_SWE1_YB", 37466)
        Sounds.Add("SOUND_SWE1_YC", 37467)
        Sounds.Add("SOUND_SWE1_YD", 37468)
        Sounds.Add("SOUND_SWE1_YE", 37469)
        Sounds.Add("SOUND_SWE1_YF", 37470)
        Sounds.Add("SOUND_SWE1_YG", 37471)
        Sounds.Add("SOUND_SWE1_YH", 37472)
        Sounds.Add("SOUND_SWE1_YJ", 37473)
        Sounds.Add("SOUND_SWE1_YK", 37474)
        Sounds.Add("SOUND_SWE1_YL", 37475)
        Sounds.Add("SOUND_SWE1_YM", 37476)
        Sounds.Add("SOUND_SWE1_YN", 37477)
        Sounds.Add("SOUND_SWE1_YO", 37478)
        Sounds.Add("SOUND_SWE1_YP", 37479)
        Sounds.Add("SOUND_SWE1_YQ", 37480)
        Sounds.Add("SOUND_SWE1_YR", 37481)
        Sounds.Add("SOUND_SWE1_YS", 37482)
        Sounds.Add("SOUND_SWE1_YT", 37483)
        Sounds.Add("SOUND_SWE1_YU", 37484)
        Sounds.Add("SOUND_SWE1_YV", 37485)
        Sounds.Add("SOUND_SWE1_YW", 37486)
        Sounds.Add("SOUND_SWE1_YX", 37487)
        Sounds.Add("SOUND_SWE1_YY", 37488)
        Sounds.Add("SOUND_SWE1_ZA", 37489)
        Sounds.Add("SOUND_SWE1_ZB", 37490)
        Sounds.Add("SOUND_SWE1_ZC", 37491)
        Sounds.Add("SOUND_SWE1_ZD", 37492)
        Sounds.Add("SOUND_SWE1_ZE", 37493)
        Sounds.Add("SOUND_SWE1_ZF", 37494)
        Sounds.Add("SOUND_SWE2_AA", 37600)
        Sounds.Add("SOUND_SWE2_BA", 37601)
        Sounds.Add("SOUND_SWE2_BB", 37602)
        Sounds.Add("SOUND_SWE2_BC", 37603)
        Sounds.Add("SOUND_SWE2_BD", 37604)
        Sounds.Add("SOUND_SWE2_BE", 37605)
        Sounds.Add("SOUND_SWE2_BF", 37606)
        Sounds.Add("SOUND_SWE2_BG", 37607)
        Sounds.Add("SOUND_SWE2_BH", 37608)
        Sounds.Add("SOUND_SWE2_BJ", 37609)
        Sounds.Add("SOUND_SWE2_BK", 37610)
        Sounds.Add("SOUND_SWE2_BL", 37611)
        Sounds.Add("SOUND_SWE2_BM", 37612)
        Sounds.Add("SOUND_SWE2_CA", 37613)
        Sounds.Add("SOUND_SWE2_CB", 37614)
        Sounds.Add("SOUND_SWE2_CC", 37615)
        Sounds.Add("SOUND_SWE2_DA", 37616)
        Sounds.Add("SOUND_SWE2_DB", 37617)
        Sounds.Add("SOUND_SWE2_DC", 37618)
        Sounds.Add("SOUND_SWE2_DD", 37619)
        Sounds.Add("SOUND_SWE2_EA", 37620)
        Sounds.Add("SOUND_SWE2_EB", 37621)
        Sounds.Add("SOUND_SWE2_EC", 37622)
        Sounds.Add("SOUND_SWE2_ED", 37623)
        Sounds.Add("SOUND_SWE2_EE", 37624)
        Sounds.Add("SOUND_SWE2_FA", 37625)
        Sounds.Add("SOUND_SWE2_FB", 37626)
        Sounds.Add("SOUND_SWE2_FC", 37627)
        Sounds.Add("SOUND_SWE2_FD", 37628)
        Sounds.Add("SOUND_SWE2_FE", 37629)
        Sounds.Add("SOUND_SWE2_GA", 37630)
        Sounds.Add("SOUND_SWE2_GB", 37631)
        Sounds.Add("SOUND_SWE2_GC", 37632)
        Sounds.Add("SOUND_SWE2_GD", 37633)
        Sounds.Add("SOUND_SWE2_GE", 37634)
        Sounds.Add("SOUND_SWE2_HA", 37635)
        Sounds.Add("SOUND_SWE2_HB", 37636)
        Sounds.Add("SOUND_SWE2_HC", 37637)
        Sounds.Add("SOUND_SWE2_HD", 37638)
        Sounds.Add("SOUND_SWE2_HE", 37639)
        Sounds.Add("SOUND_SWE2_HF", 37640)
        Sounds.Add("SOUND_SWE2_HG", 37641)
        Sounds.Add("SOUND_SWE2_JA", 37642)
        Sounds.Add("SOUND_SWE2_JB", 37643)
        Sounds.Add("SOUND_SWE2_JC", 37644)
        Sounds.Add("SOUND_SWE2_KA", 37645)
        Sounds.Add("SOUND_SWE2_KB", 37646)
        Sounds.Add("SOUND_SWE2_KC", 37647)
        Sounds.Add("SOUND_SWE2_LA", 37648)
        Sounds.Add("SOUND_SWE2_LB", 37649)
        Sounds.Add("SOUND_SWE2_LC", 37650)
        Sounds.Add("SOUND_SWE2_MA", 37651)
        Sounds.Add("SOUND_SWE2_MB", 37652)
        Sounds.Add("SOUND_SWE2_MC", 37653)
        Sounds.Add("SOUND_SWE2_MD", 37654)
        Sounds.Add("SOUND_SWE2_ME", 37655)
        Sounds.Add("SOUND_SWE2_MF", 37656)
        Sounds.Add("SOUND_SWE2_MG", 37657)
        Sounds.Add("SOUND_SWE2_MH", 37658)
        Sounds.Add("SOUND_SWE2_MJ", 37659)
        Sounds.Add("SOUND_SWE2_NA", 37660)
        Sounds.Add("SOUND_SWE2_NB", 37661)
        Sounds.Add("SOUND_SWE2_NC", 37662)
        Sounds.Add("SOUND_SWE2_ND", 37663)
        Sounds.Add("SOUND_SWE2_NE", 37664)
        Sounds.Add("SOUND_SWE2_NF", 37665)
        Sounds.Add("SOUND_SWE2_NG", 37666)
        Sounds.Add("SOUND_SWE2_NH", 37667)
        Sounds.Add("SOUND_SWE2_OA", 37668)
        Sounds.Add("SOUND_SWE2_OB", 37669)
        Sounds.Add("SOUND_SWE2_OC", 37670)
        Sounds.Add("SOUND_SWE2_OD", 37671)
        Sounds.Add("SOUND_SWE2_OE", 37672)
        Sounds.Add("SOUND_SWE2_OF", 37673)
        Sounds.Add("SOUND_SWE2_OG", 37674)
        Sounds.Add("SOUND_SWE2_OH", 37675)
        Sounds.Add("SOUND_SWE2_OJ", 37676)
        Sounds.Add("SOUND_SWE2_PA", 37677)
        Sounds.Add("SOUND_SWE2_PB", 37678)
        Sounds.Add("SOUND_SWE2_PC", 37679)
        Sounds.Add("SOUND_SWE2_PD", 37680)
        Sounds.Add("SOUND_SWE2_PE", 37681)
        Sounds.Add("SOUND_SWE3_AA", 37800)
        Sounds.Add("SOUND_SWE3_AB", 37801)
        Sounds.Add("SOUND_SWE3_AC", 37802)
        Sounds.Add("SOUND_SWE3_AD", 37803)
        Sounds.Add("SOUND_SWE3_AE", 37804)
        Sounds.Add("SOUND_SWE3_AF", 37805)
        Sounds.Add("SOUND_SWE3_AH", 37806)
        Sounds.Add("SOUND_SWE3_AJ", 37807)
        Sounds.Add("SOUND_SWE3_AK", 37808)
        Sounds.Add("SOUND_SWE3_BA", 37809)
        Sounds.Add("SOUND_SWE3_BB", 37810)
        Sounds.Add("SOUND_SWE3_BC", 37811)
        Sounds.Add("SOUND_SWE3_BD", 37812)
        Sounds.Add("SOUND_SWE3_BE", 37813)
        Sounds.Add("SOUND_SWE3_BF", 37814)
        Sounds.Add("SOUND_SWE3_BG", 37815)
        Sounds.Add("SOUND_SWE3_BH", 37816)
        Sounds.Add("SOUND_SWE3_BJ", 37817)
        Sounds.Add("SOUND_SWE3_BK", 37818)
        Sounds.Add("SOUND_SWE3_CA", 37819)
        Sounds.Add("SOUND_SWE3_CB", 37820)
        Sounds.Add("SOUND_SWE3_CC", 37821)
        Sounds.Add("SOUND_SWE3_CD", 37822)
        Sounds.Add("SOUND_SWE3_CE", 37823)
        Sounds.Add("SOUND_SWE3_CF", 37824)
        Sounds.Add("SOUND_SWE3_CG", 37825)
        Sounds.Add("SOUND_SWE3_CH", 37826)
        Sounds.Add("SOUND_SWE3_CJ", 37827)
        Sounds.Add("SOUND_SWE3_CK", 37828)
        Sounds.Add("SOUND_SWE3_DA", 37829)
        Sounds.Add("SOUND_SWE3_DB", 37830)
        Sounds.Add("SOUND_SWE3_DC", 37831)
        Sounds.Add("SOUND_SWE3_DD", 37832)
        Sounds.Add("SOUND_SWE3_DE", 37833)
        Sounds.Add("SOUND_SWE3_DF", 37834)
        Sounds.Add("SOUND_SWE3_DG", 37835)
        Sounds.Add("SOUND_SWE3_EA", 37836)
        Sounds.Add("SOUND_SWE3_EB", 37837)
        Sounds.Add("SOUND_SWE3_EC", 37838)
        Sounds.Add("SOUND_SWE3_ED", 37839)
        Sounds.Add("SOUND_SWE3_EE", 37840)
        Sounds.Add("SOUND_SWE3_EF", 37841)
        Sounds.Add("SOUND_SWE3_FA", 37842)
        Sounds.Add("SOUND_SWE3_FB", 37843)
        Sounds.Add("SOUND_SWE3_FC", 37844)
        Sounds.Add("SOUND_SWE3_FD", 37845)
        Sounds.Add("SOUND_SWE3_FE", 37846)
        Sounds.Add("SOUND_SWE3_FF", 37847)
        Sounds.Add("SOUND_SWE3_FG", 37848)
        Sounds.Add("SOUND_SWE3_FH", 37849)
        Sounds.Add("SOUND_SWE3_GA", 37850)
        Sounds.Add("SOUND_SWE3_GB", 37851)
        Sounds.Add("SOUND_SWE3_GC", 37852)
        Sounds.Add("SOUND_SWE3_GD", 37853)
        Sounds.Add("SOUND_SWE3_GE", 37854)
        Sounds.Add("SOUND_SWE3_GF", 37855)
        Sounds.Add("SOUND_SWE3_GG", 37856)
        Sounds.Add("SOUND_SWE3_GH", 37857)
        Sounds.Add("SOUND_SWE3_GJ", 37858)
        Sounds.Add("SOUND_SWE3_GK", 37859)
        Sounds.Add("SOUND_SWE3_GL", 37860)
        Sounds.Add("SOUND_SWE3_GM", 37861)
        Sounds.Add("SOUND_SWE3_GN", 37862)
        Sounds.Add("SOUND_SWE3_GO", 37863)
        Sounds.Add("SOUND_SWE3_HA", 37864)
        Sounds.Add("SOUND_SWE3_HB", 37865)
        Sounds.Add("SOUND_SWE3_HC", 37866)
        Sounds.Add("SOUND_SWE3_HD", 37867)
        Sounds.Add("SOUND_SWE3_HE", 37868)
        Sounds.Add("SOUND_SWE3_HF", 37869)
        Sounds.Add("SOUND_SWE3_HG", 37870)
        Sounds.Add("SOUND_SWE3_JA", 37871)
        Sounds.Add("SOUND_SWE3_JB", 37872)
        Sounds.Add("SOUND_SWE3_ZZ", 37873)
        Sounds.Add("SOUND_SWE4_AA", 38000)
        Sounds.Add("SOUND_SWE4_AB", 38001)
        Sounds.Add("SOUND_SWE4_AC", 38002)
        Sounds.Add("SOUND_SWE4_AD", 38003)
        Sounds.Add("SOUND_SWE4_AE", 38004)
        Sounds.Add("SOUND_SWE4_AF", 38005)
        Sounds.Add("SOUND_SWE4_AG", 38006)
        Sounds.Add("SOUND_SWE4_AH", 38007)
        Sounds.Add("SOUND_SWE4_AI", 38008)
        Sounds.Add("SOUND_SWE4_AJ", 38009)
        Sounds.Add("SOUND_SWE4_AK", 38010)
        Sounds.Add("SOUND_SWE4_BA", 38011)
        Sounds.Add("SOUND_SWE4_BB", 38012)
        Sounds.Add("SOUND_SWE4_BC", 38013)
        Sounds.Add("SOUND_SWE4_BD", 38014)
        Sounds.Add("SOUND_SWE4_BE", 38015)
        Sounds.Add("SOUND_SWE4_CA", 38016)
        Sounds.Add("SOUND_SWE4_CB", 38017)
        Sounds.Add("SOUND_SWE4_CC", 38018)
        Sounds.Add("SOUND_SWE4_DA", 38019)
        Sounds.Add("SOUND_SWE4_DB", 38020)
        Sounds.Add("SOUND_SWE4_DC", 38021)
        Sounds.Add("SOUND_SWE4_DD", 38022)
        Sounds.Add("SOUND_SWE4_DE", 38023)
        Sounds.Add("SOUND_SWE4_DF", 38024)
        Sounds.Add("SOUND_SWE4_EA", 38025)
        Sounds.Add("SOUND_SWE4_EB", 38026)
        Sounds.Add("SOUND_SWE4_EC", 38027)
        Sounds.Add("SOUND_SWE4_FA", 38028)
        Sounds.Add("SOUND_SWE4_FB", 38029)
        Sounds.Add("SOUND_SWE4_FC", 38030)
        Sounds.Add("SOUND_SWE4_FD", 38031)
        Sounds.Add("SOUND_SWE4_GA", 38032)
        Sounds.Add("SOUND_SWE4_GB", 38033)
        Sounds.Add("SOUND_SWE4_HA", 38034)
        Sounds.Add("SOUND_SWE4_HB", 38035)
        Sounds.Add("SOUND_SWE4_KA", 38036)
        Sounds.Add("SOUND_SWE4_KB", 38037)
        Sounds.Add("SOUND_SWE4_KC", 38038)
        Sounds.Add("SOUND_SWE4_KD", 38039)
        Sounds.Add("SOUND_SWE4_KE", 38040)
        Sounds.Add("SOUND_SWE4_KF", 38041)
        Sounds.Add("SOUND_SWE4_KG", 38042)
        Sounds.Add("SOUND_SWE4_KH", 38043)
        Sounds.Add("SOUND_SWE4_KI", 38044)
        Sounds.Add("SOUND_SWE4_LA", 38045)
        Sounds.Add("SOUND_SWE4_LB", 38046)
        Sounds.Add("SOUND_SWE4_LC", 38047)
        Sounds.Add("SOUND_SWE4_MA", 38048)
        Sounds.Add("SOUND_SWE4_MB", 38049)
        Sounds.Add("SOUND_SWE4_MC", 38050)
        Sounds.Add("SOUND_SWE4_MD", 38051)
        Sounds.Add("SOUND_SWE4_NA", 38052)
        Sounds.Add("SOUND_SWE4_NB", 38053)
        Sounds.Add("SOUND_SWE4_NC", 38054)
        Sounds.Add("SOUND_SWE4_OA", 38055)
        Sounds.Add("SOUND_SWE4_OB", 38056)
        Sounds.Add("SOUND_SWE4_OC", 38057)
        Sounds.Add("SOUND_SWE4_PA", 38058)
        Sounds.Add("SOUND_SWE4_PB", 38059)
        Sounds.Add("SOUND_SWE4_PC", 38060)
        Sounds.Add("SOUND_SWE5_AA", 38200)
        Sounds.Add("SOUND_SWE5_AB", 38201)
        Sounds.Add("SOUND_SWE5_AC", 38202)
        Sounds.Add("SOUND_SWE5_AD", 38203)
        Sounds.Add("SOUND_SWE5_AE", 38204)
        Sounds.Add("SOUND_SWE5_AF", 38205)
        Sounds.Add("SOUND_SWE5_BA", 38206)
        Sounds.Add("SOUND_SWE5_BB", 38207)
        Sounds.Add("SOUND_SWE5_BC", 38208)
        Sounds.Add("SOUND_SWE5_BD", 38209)
        Sounds.Add("SOUND_SWE5_BE", 38210)
        Sounds.Add("SOUND_SWE5_BF", 38211)
        Sounds.Add("SOUND_SWE5_BG", 38212)
        Sounds.Add("SOUND_SWE5_BH", 38213)
        Sounds.Add("SOUND_SWE5_BI", 38214)
        Sounds.Add("SOUND_SWE5_BJ", 38215)
        Sounds.Add("SOUND_SWE5_CA", 38216)
        Sounds.Add("SOUND_SWE5_CB", 38217)
        Sounds.Add("SOUND_SWE5_CC", 38218)
        Sounds.Add("SOUND_SWE5_DA", 38219)
        Sounds.Add("SOUND_SWE5_DB", 38220)
        Sounds.Add("SOUND_SWE5_EA", 38221)
        Sounds.Add("SOUND_SWE5_EB", 38222)
        Sounds.Add("SOUND_SWE5_EC", 38223)
        Sounds.Add("SOUND_SWE5_ED", 38224)
        Sounds.Add("SOUND_SWE5_EE", 38225)
        Sounds.Add("SOUND_SWE5_FA", 38226)
        Sounds.Add("SOUND_SWE5_FB", 38227)
        Sounds.Add("SOUND_SWE5_GA", 38228)
        Sounds.Add("SOUND_SWE5_GB", 38229)
        Sounds.Add("SOUND_SWE5_GC", 38230)
        Sounds.Add("SOUND_SWE5_GD", 38231)
        Sounds.Add("SOUND_SWE5_GE", 38232)
        Sounds.Add("SOUND_SWE5_GF", 38233)
        Sounds.Add("SOUND_SWE5_GG", 38234)
        Sounds.Add("SOUND_SWE5_GH", 38235)
        Sounds.Add("SOUND_SWE5_GJ", 38236)
        Sounds.Add("SOUND_SWE5_GK", 38237)
        Sounds.Add("SOUND_SWE5_GL", 38238)
        Sounds.Add("SOUND_SWE7_AA", 38400)
        Sounds.Add("SOUND_SWE7_AB", 38401)
        Sounds.Add("SOUND_SWE7_AC", 38402)
        Sounds.Add("SOUND_SWE7_AD", 38403)
        Sounds.Add("SOUND_SWE7_AE", 38404)
        Sounds.Add("SOUND_SWE7_AF", 38405)
        Sounds.Add("SOUND_SWE7_AG", 38406)
        Sounds.Add("SOUND_SWE7_AH", 38407)
        Sounds.Add("SOUND_SWE7_BA", 38408)
        Sounds.Add("SOUND_SWE7_BB", 38409)
        Sounds.Add("SOUND_SWE7_BC", 38410)
        Sounds.Add("SOUND_SWE7_BD", 38411)
        Sounds.Add("SOUND_SWE7_BE", 38412)
        Sounds.Add("SOUND_SWE7_CA", 38413)
        Sounds.Add("SOUND_SWE7_CB", 38414)
        Sounds.Add("SOUND_SWE7_CC", 38415)
        Sounds.Add("SOUND_SWE7_DA", 38416)
        Sounds.Add("SOUND_SWE7_DB", 38417)
        Sounds.Add("SOUND_SWE7_DC", 38418)
        Sounds.Add("SOUND_SWE7_DD", 38419)
        Sounds.Add("SOUND_SWE7_DE", 38420)
        Sounds.Add("SOUND_SWE7_EA", 38421)
        Sounds.Add("SOUND_SWE7_EB", 38422)
        Sounds.Add("SOUND_SWE7_EC", 38423)
        Sounds.Add("SOUND_SWE7_ED", 38424)
        Sounds.Add("SOUND_SWE7_EE", 38425)
        Sounds.Add("SOUND_SWE7_EF", 38426)
        Sounds.Add("SOUND_SWE7_EG", 38427)
        Sounds.Add("SOUND_SWE7_EH", 38428)
        Sounds.Add("SOUND_SWE7_EJ", 38429)
        Sounds.Add("SOUND_SWE7_EK", 38430)
        Sounds.Add("SOUND_SWE7_FA", 38431)
        Sounds.Add("SOUND_SWE7_FB", 38432)
        Sounds.Add("SOUND_SWE7_GA", 38433)
        Sounds.Add("SOUND_SWE7_GB", 38434)
        Sounds.Add("SOUND_SWE7_GC", 38435)
        Sounds.Add("SOUND_SWE7_HA", 38436)
        Sounds.Add("SOUND_SWE7_HB", 38437)
        Sounds.Add("SOUND_SWE7_HC", 38438)
        Sounds.Add("SOUND_SWE7_JA", 38439)
        Sounds.Add("SOUND_SWE7_JB", 38440)
        Sounds.Add("SOUND_SWE7_JC", 38441)
        Sounds.Add("SOUND_SWE7_KA", 38442)
        Sounds.Add("SOUND_SWE7_KB", 38443)
        Sounds.Add("SOUND_SWE7_KC", 38444)
        Sounds.Add("SOUND_SWE7_LA", 38445)
        Sounds.Add("SOUND_SWE7_LB", 38446)
        Sounds.Add("SOUND_SWE7_LC", 38447)
        Sounds.Add("SOUND_SWE7_MA", 38448)
        Sounds.Add("SOUND_SWE7_MB", 38449)
        Sounds.Add("SOUND_SWE7_MC", 38450)
        Sounds.Add("SOUND_SWE7_MD", 38451)
        Sounds.Add("SOUND_SWE7_NA", 38452)
        Sounds.Add("SOUND_SWE7_NB", 38453)
        Sounds.Add("SOUND_SWE7_OA", 38454)
        Sounds.Add("SOUND_SWE7_OB", 38455)
        Sounds.Add("SOUND_SWE7_PA", 38456)
        Sounds.Add("SOUND_SWE7_PB", 38457)
        Sounds.Add("SOUND_SWE7_PC", 38458)
        Sounds.Add("SOUND_SWE7_PD", 38459)
        Sounds.Add("SOUND_SWE7_QA", 38460)
        Sounds.Add("SOUND_SWE7_QB", 38461)
        Sounds.Add("SOUND_SWE7_QC", 38462)
        Sounds.Add("SOUND_SWE7_RA", 38463)
        Sounds.Add("SOUND_SWE7_RB", 38464)
        Sounds.Add("SOUND_SWE7_RC", 38465)
        Sounds.Add("SOUND_SWE7_SA", 38466)
        Sounds.Add("SOUND_SWE7_SB", 38467)
        Sounds.Add("SOUND_SWE7_SC", 38468)
        Sounds.Add("SOUND_SWE7_TA", 38469)
        Sounds.Add("SOUND_SWE7_TB", 38470)
        Sounds.Add("SOUND_SWE7_TC", 38471)
        Sounds.Add("SOUND_SWEX_AA", 38600)
        Sounds.Add("SOUND_SWEX_AB", 38601)
        Sounds.Add("SOUND_SWEX_AC", 38602)
        Sounds.Add("SOUND_SWEX_AD", 38603)
        Sounds.Add("SOUND_SWEX_AE", 38604)
        Sounds.Add("SOUND_SWEX_AF", 38605)
        Sounds.Add("SOUND_SWEX_AG", 38606)
        Sounds.Add("SOUND_SWEX_AH", 38607)
        Sounds.Add("SOUND_SWEX_AI", 38608)
        Sounds.Add("SOUND_SWEX_AJ", 38609)
        Sounds.Add("SOUND_SWEX_AK", 38610)
        Sounds.Add("SOUND_SWEX_AL", 38611)
        Sounds.Add("SOUND_SWEX_AM", 38612)
        Sounds.Add("SOUND_SWEX_AN", 38613)
        Sounds.Add("SOUND_SWEX_AO", 38614)
        Sounds.Add("SOUND_SWEX_AP", 38615)
        Sounds.Add("SOUND_SWEX_AQ", 38616)
        Sounds.Add("SOUND_SWEX_AR", 38617)
        Sounds.Add("SOUND_SWEX_AS", 38618)
        Sounds.Add("SOUND_SWEX_AT", 38619)
        Sounds.Add("SOUND_SWEX_AU", 38620)
        Sounds.Add("SOUND_SWEX_AV", 38621)
        Sounds.Add("SOUND_SWEX_AW", 38622)
        Sounds.Add("SOUND_SWEX_AX", 38623)
        Sounds.Add("SOUND_SWEX_AY", 38624)
        Sounds.Add("SOUND_SWEX_AZ", 38625)
        Sounds.Add("SOUND_SWEX_BA", 38626)
        Sounds.Add("SOUND_SWEX_BB", 38627)
        Sounds.Add("SOUND_SWEX_BC", 38628)
        Sounds.Add("SOUND_SWEX_BD", 38629)
        Sounds.Add("SOUND_SWEX_BE", 38630)
        Sounds.Add("SOUND_SWEX_BF", 38631)
        Sounds.Add("SOUND_SWEX_BG", 38632)
        Sounds.Add("SOUND_SWEX_BH", 38633)
        Sounds.Add("SOUND_SWEX_BI", 38634)
        Sounds.Add("SOUND_SWEX_BJ", 38635)
        Sounds.Add("SOUND_SWEX_BK", 38636)
        Sounds.Add("SOUND_SWEX_BL", 38637)
        Sounds.Add("SOUND_SWEX_BM", 38638)
        Sounds.Add("SOUND_SWEX_BN", 38639)
        Sounds.Add("SOUND_SWEX_BO", 38640)
        Sounds.Add("SOUND_SWEX_BP", 38641)
        Sounds.Add("SOUND_SWEX_BQ", 38642)
        Sounds.Add("SOUND_SWEX_BR", 38643)
        Sounds.Add("SOUND_SWEX_BS", 38644)
        Sounds.Add("SOUND_SYN1_AB", 38800)
        Sounds.Add("SOUND_SYN1_AC", 38801)
        Sounds.Add("SOUND_SYN1_AE", 38802)
        Sounds.Add("SOUND_SYN1_BA", 38803)
        Sounds.Add("SOUND_SYN1_BD", 38804)
        Sounds.Add("SOUND_SYN1_CB", 38805)
        Sounds.Add("SOUND_SYN1_EB", 38806)
        Sounds.Add("SOUND_SYN1_FB", 38807)
        Sounds.Add("SOUND_SYN1_FC", 38808)
        Sounds.Add("SOUND_SYN1_FD", 38809)
        Sounds.Add("SOUND_SYN1_IA", 38810)
        Sounds.Add("SOUND_SYN1_IB", 38811)
        Sounds.Add("SOUND_SYN1_ID", 38812)
        Sounds.Add("SOUND_SYN1_IF", 38813)
        Sounds.Add("SOUND_SYN1_JA", 38814)
        Sounds.Add("SOUND_SYN1_JB", 38815)
        Sounds.Add("SOUND_SYN1_JD", 38816)
        Sounds.Add("SOUND_SYN1_JE", 38817)
        Sounds.Add("SOUND_SYN1_JJ", 38818)
        Sounds.Add("SOUND_SYN1_JK", 38819)
        Sounds.Add("SOUND_SYN1_JN", 38820)
        Sounds.Add("SOUND_SYN1_JO", 38821)
        Sounds.Add("SOUND_SYN1_JP", 38822)
        Sounds.Add("SOUND_SYN1_JR", 38823)
        Sounds.Add("SOUND_SYN1_JS", 38824)
        Sounds.Add("SOUND_SYN1_JT", 38825)
        Sounds.Add("SOUND_SYN1_JU", 38826)
        Sounds.Add("SOUND_SYN1_JX", 38827)
        Sounds.Add("SOUND_SYN1_KB", 38828)
        Sounds.Add("SOUND_SYN1_LA", 38829)
        Sounds.Add("SOUND_SYN1_LC", 38830)
        Sounds.Add("SOUND_SYN1_YA", 38831)
        Sounds.Add("SOUND_SYN1_YB", 38832)
        Sounds.Add("SOUND_SYN1_YE", 38833)
        Sounds.Add("SOUND_SYN1_YF", 38834)
        Sounds.Add("SOUND_SYN1_YH", 38835)
        Sounds.Add("SOUND_SYN1_YJ", 38836)
        Sounds.Add("SOUND_SYN1_YK", 38837)
        Sounds.Add("SOUND_SYN1_ZC", 38838)
        Sounds.Add("SOUND_SYN1_ZD", 38839)
        Sounds.Add("SOUND_SYN1_ZE", 38840)
        Sounds.Add("SOUND_SYN1_ZG", 38841)
        Sounds.Add("SOUND_SYN1_ZK", 38842)
        Sounds.Add("SOUND_SYN1_ZL", 38843)
        Sounds.Add("SOUND_SYN1_ZM", 38844)
        Sounds.Add("SOUND_SYN1_ZN", 38845)
        Sounds.Add("SOUND_SYN1_ZP", 38846)
        Sounds.Add("SOUND_SYN1_ZQ", 38847)
        Sounds.Add("SOUND_SYN1_ZR", 38848)
        Sounds.Add("SOUND_SYN1_ZT", 38849)
        Sounds.Add("SOUND_SYN1_ZU", 38850)
        Sounds.Add("SOUND_SYN1_ZW", 38851)
        Sounds.Add("SOUND_SYN1_ZX", 38852)
        Sounds.Add("SOUND_SYN1_ZY", 38853)
        Sounds.Add("SOUND_SYN1_ZZ", 38854)
        Sounds.Add("SOUND_SYN2_AA", 39000)
        Sounds.Add("SOUND_SYN2_AB", 39001)
        Sounds.Add("SOUND_SYN2_AC", 39002)
        Sounds.Add("SOUND_SYN2_AD", 39003)
        Sounds.Add("SOUND_SYN2_BA", 39004)
        Sounds.Add("SOUND_SYN2_BB", 39005)
        Sounds.Add("SOUND_SYN2_BC", 39006)
        Sounds.Add("SOUND_SYN2_BD", 39007)
        Sounds.Add("SOUND_SYN2_BE", 39008)
        Sounds.Add("SOUND_SYN2_BF", 39009)
        Sounds.Add("SOUND_SYN2_CA", 39010)
        Sounds.Add("SOUND_SYN2_CB", 39011)
        Sounds.Add("SOUND_SYN2_CC", 39012)
        Sounds.Add("SOUND_SYN2_CD", 39013)
        Sounds.Add("SOUND_SYN2_CE", 39014)
        Sounds.Add("SOUND_SYN2_CF", 39015)
        Sounds.Add("SOUND_SYN2_CG", 39016)
        Sounds.Add("SOUND_SYN2_DA", 39017)
        Sounds.Add("SOUND_SYN2_DB", 39018)
        Sounds.Add("SOUND_SYN2_DC", 39019)
        Sounds.Add("SOUND_SYN2_DD", 39020)
        Sounds.Add("SOUND_SYN2_DE", 39021)
        Sounds.Add("SOUND_SYN2_DF", 39022)
        Sounds.Add("SOUND_SYN2_DG", 39023)
        Sounds.Add("SOUND_SYN2_EA", 39024)
        Sounds.Add("SOUND_SYN2_EB", 39025)
        Sounds.Add("SOUND_SYN2_EC", 39026)
        Sounds.Add("SOUND_SYN2_FA", 39027)
        Sounds.Add("SOUND_SYN2_FB", 39028)
        Sounds.Add("SOUND_SYN2_FC", 39029)
        Sounds.Add("SOUND_SYN2_FD", 39030)
        Sounds.Add("SOUND_SYN2_FE", 39031)
        Sounds.Add("SOUND_SYN2_FF", 39032)
        Sounds.Add("SOUND_SYN2_GA", 39033)
        Sounds.Add("SOUND_SYN2_GB", 39034)
        Sounds.Add("SOUND_SYN2_GC", 39035)
        Sounds.Add("SOUND_SYN2_GD", 39036)
        Sounds.Add("SOUND_SYN2_GE", 39037)
        Sounds.Add("SOUND_SYN2_GF", 39038)
        Sounds.Add("SOUND_SYN2_HA", 39039)
        Sounds.Add("SOUND_SYN2_HB", 39040)
        Sounds.Add("SOUND_SYN2_HC", 39041)
        Sounds.Add("SOUND_SYN2_HD", 39042)
        Sounds.Add("SOUND_SYN2_HE", 39043)
        Sounds.Add("SOUND_SYN2_HF", 39044)
        Sounds.Add("SOUND_SYN2_JA", 39045)
        Sounds.Add("SOUND_SYN2_JB", 39046)
        Sounds.Add("SOUND_SYN2_JC", 39047)
        Sounds.Add("SOUND_SYN2_JD", 39048)
        Sounds.Add("SOUND_SYN2_JE", 39049)
        Sounds.Add("SOUND_SYN2_JF", 39050)
        Sounds.Add("SOUND_SYN2_JG", 39051)
        Sounds.Add("SOUND_SYN2_JH", 39052)
        Sounds.Add("SOUND_SYN2_KA", 39053)
        Sounds.Add("SOUND_SYN2_KB", 39054)
        Sounds.Add("SOUND_SYN2_KC", 39055)
        Sounds.Add("SOUND_SYN2_KD", 39056)
        Sounds.Add("SOUND_SYN2_KE", 39057)
        Sounds.Add("SOUND_SYN2_LA", 39058)
        Sounds.Add("SOUND_SYN2_LB", 39059)
        Sounds.Add("SOUND_SYN2_LC", 39060)
        Sounds.Add("SOUND_SYN2_LD", 39061)
        Sounds.Add("SOUND_SYN2_LE", 39062)
        Sounds.Add("SOUND_SYN2_LF", 39063)
        Sounds.Add("SOUND_SYN2_LG", 39064)
        Sounds.Add("SOUND_SYN2_LH", 39065)
        Sounds.Add("SOUND_SYN2_LI", 39066)
        Sounds.Add("SOUND_SYN2_LJ", 39067)
        Sounds.Add("SOUND_SYN2_LK", 39068)
        Sounds.Add("SOUND_SYN2_LL", 39069)
        Sounds.Add("SOUND_SYN2_LM", 39070)
        Sounds.Add("SOUND_SYN2_LN", 39071)
        Sounds.Add("SOUND_SYN2_LO", 39072)
        Sounds.Add("SOUND_SYN2_LP", 39073)
        Sounds.Add("SOUND_SYN2_LQ", 39074)
        Sounds.Add("SOUND_SYN2_LR", 39075)
        Sounds.Add("SOUND_SYN2_LS", 39076)
        Sounds.Add("SOUND_SYN2_LT", 39077)
        Sounds.Add("SOUND_SYN2_LU", 39078)
        Sounds.Add("SOUND_SYN3_AA", 39200)
        Sounds.Add("SOUND_SYN3_AB", 39201)
        Sounds.Add("SOUND_SYN3_AC", 39202)
        Sounds.Add("SOUND_SYN3_AD", 39203)
        Sounds.Add("SOUND_SYN3_BA", 39204)
        Sounds.Add("SOUND_SYN3_BB", 39205)
        Sounds.Add("SOUND_SYN3_BC", 39206)
        Sounds.Add("SOUND_SYN3_BD", 39207)
        Sounds.Add("SOUND_SYN3_BE", 39208)
        Sounds.Add("SOUND_SYN3_BF", 39209)
        Sounds.Add("SOUND_SYN3_BG", 39210)
        Sounds.Add("SOUND_SYN3_BH", 39211)
        Sounds.Add("SOUND_SYN3_BI", 39212)
        Sounds.Add("SOUND_SYN3_CA", 39213)
        Sounds.Add("SOUND_SYN3_CB", 39214)
        Sounds.Add("SOUND_SYN3_CC", 39215)
        Sounds.Add("SOUND_SYN3_CD", 39216)
        Sounds.Add("SOUND_SYN3_CE", 39217)
        Sounds.Add("SOUND_SYN3_CF", 39218)
        Sounds.Add("SOUND_SYN3_CG", 39219)
        Sounds.Add("SOUND_SYN3_DC", 39220)
        Sounds.Add("SOUND_SYN3_EA", 39221)
        Sounds.Add("SOUND_SYN3_EB", 39222)
        Sounds.Add("SOUND_SYN3_HJ", 39223)
        Sounds.Add("SOUND_SYN4_AA", 39400)
        Sounds.Add("SOUND_SYN4_AB", 39401)
        Sounds.Add("SOUND_SYN4_AC", 39402)
        Sounds.Add("SOUND_SYN4_AD", 39403)
        Sounds.Add("SOUND_SYN4_AE", 39404)
        Sounds.Add("SOUND_SYN4_AF", 39405)
        Sounds.Add("SOUND_SYN4_AG", 39406)
        Sounds.Add("SOUND_SYN4_AH", 39407)
        Sounds.Add("SOUND_SYN4_AJ", 39408)
        Sounds.Add("SOUND_SYN4_AK", 39409)
        Sounds.Add("SOUND_SYN4_AL", 39410)
        Sounds.Add("SOUND_SYN4_BA", 39411)
        Sounds.Add("SOUND_SYN4_BB", 39412)
        Sounds.Add("SOUND_SYN4_BC", 39413)
        Sounds.Add("SOUND_SYN5_AA", 39600)
        Sounds.Add("SOUND_SYN5_BA", 39601)
        Sounds.Add("SOUND_SYN5_BB", 39602)
        Sounds.Add("SOUND_SYN5_BC", 39603)
        Sounds.Add("SOUND_SYN5_BD", 39604)
        Sounds.Add("SOUND_SYN5_BE", 39605)
        Sounds.Add("SOUND_SYN5_BF", 39606)
        Sounds.Add("SOUND_SYN5_BG", 39607)
        Sounds.Add("SOUND_SYN5_BH", 39608)
        Sounds.Add("SOUND_SYN5_BJ", 39609)
        Sounds.Add("SOUND_SYN5_BK", 39610)
        Sounds.Add("SOUND_SYN5_BL", 39611)
        Sounds.Add("SOUND_SYN5_BM", 39612)
        Sounds.Add("SOUND_SYN5_BN", 39613)
        Sounds.Add("SOUND_SYN5_CA", 39614)
        Sounds.Add("SOUND_SYN5_CB", 39615)
        Sounds.Add("SOUND_SYN5_CC", 39616)
        Sounds.Add("SOUND_SYN5_CD", 39617)
        Sounds.Add("SOUND_SYN5_CE", 39618)
        Sounds.Add("SOUND_SYN5_CF", 39619)
        Sounds.Add("SOUND_SYN5_DA", 39620)
        Sounds.Add("SOUND_SYN5_DB", 39621)
        Sounds.Add("SOUND_SYN5_DC", 39622)
        Sounds.Add("SOUND_SYN5_DD", 39623)
        Sounds.Add("SOUND_SYN5_DE", 39624)
        Sounds.Add("SOUND_SYN5_EA", 39625)
        Sounds.Add("SOUND_SYN5_EB", 39626)
        Sounds.Add("SOUND_SYN5_EC", 39627)
        Sounds.Add("SOUND_SYN5_ED", 39628)
        Sounds.Add("SOUND_SYN5_FA", 39629)
        Sounds.Add("SOUND_SYN5_FB", 39630)
        Sounds.Add("SOUND_SYN5_GA", 39631)
        Sounds.Add("SOUND_SYN5_GB", 39632)
        Sounds.Add("SOUND_SYN5_GC", 39633)
        Sounds.Add("SOUND_SYN5_GD", 39634)
        Sounds.Add("SOUND_SYN5_GE", 39635)
        Sounds.Add("SOUND_SYN5_GF", 39636)
        Sounds.Add("SOUND_SYN5_GG", 39637)
        Sounds.Add("SOUND_SYN5_GH", 39638)
        Sounds.Add("SOUND_SYN5_GJ", 39639)
        Sounds.Add("SOUND_SYN5_GK", 39640)
        Sounds.Add("SOUND_SYN5_GL", 39641)
        Sounds.Add("SOUND_SYN5_GM", 39642)
        Sounds.Add("SOUND_SYN5_GN", 39643)
        Sounds.Add("SOUND_SYN5_GO", 39644)
        Sounds.Add("SOUND_SYN5_GW", 39645)
        Sounds.Add("SOUND_SYN5_GX", 39646)
        Sounds.Add("SOUND_SYN5_GY", 39647)
        Sounds.Add("SOUND_SYN5_GZ", 39648)
        Sounds.Add("SOUND_SYN5_HA", 39649)
        Sounds.Add("SOUND_SYN5_HB", 39650)
        Sounds.Add("SOUND_SYN5_HC", 39651)
        Sounds.Add("SOUND_SYN5_HD", 39652)
        Sounds.Add("SOUND_SYN5_HE", 39653)
        Sounds.Add("SOUND_SYN5_HF", 39654)
        Sounds.Add("SOUND_SYN5_HG", 39655)
        Sounds.Add("SOUND_SYN5_HH", 39656)
        Sounds.Add("SOUND_SYN5_HJ", 39657)
        Sounds.Add("SOUND_SYN5_HK", 39658)
        Sounds.Add("SOUND_SYN5_HL", 39659)
        Sounds.Add("SOUND_SYN5_JA", 39660)
        Sounds.Add("SOUND_SYN5_JB", 39661)
        Sounds.Add("SOUND_SYN5_JC", 39662)
        Sounds.Add("SOUND_SYN5_KA", 39663)
        Sounds.Add("SOUND_SYN5_KB", 39664)
        Sounds.Add("SOUND_SYN5_KC", 39665)
        Sounds.Add("SOUND_SYN5_KD", 39666)
        Sounds.Add("SOUND_SYN5_KE", 39667)
        Sounds.Add("SOUND_SYN7_AA", 39800)
        Sounds.Add("SOUND_SYN7_AB", 39801)
        Sounds.Add("SOUND_SYN7_AC", 39802)
        Sounds.Add("SOUND_SYN7_AD", 39803)
        Sounds.Add("SOUND_SYN7_AE", 39804)
        Sounds.Add("SOUND_SYN7_BA", 39805)
        Sounds.Add("SOUND_SYN7_BB", 39806)
        Sounds.Add("SOUND_SYN7_BC", 39807)
        Sounds.Add("SOUND_SYN7_CA", 39808)
        Sounds.Add("SOUND_SYN7_CB", 39809)
        Sounds.Add("SOUND_SYN7_CC", 39810)
        Sounds.Add("SOUND_SYN7_DA", 39811)
        Sounds.Add("SOUND_SYN7_DB", 39812)
        Sounds.Add("SOUND_SYN7_DC", 39813)
        Sounds.Add("SOUND_SYN7_DD", 39814)
        Sounds.Add("SOUND_SYN7_DE", 39815)
        Sounds.Add("SOUND_TATTOO", 40000)
        Sounds.Add("SOUND_TBOX_AA", 40200)
        Sounds.Add("SOUND_TBOX_AB", 40201)
        Sounds.Add("SOUND_TBOX_AC", 40202)
        Sounds.Add("SOUND_TBOX_AD", 40203)
        Sounds.Add("SOUND_TBOX_AE", 40204)
        Sounds.Add("SOUND_TBOX_AF", 40205)
        Sounds.Add("SOUND_TBOX_AG", 40206)
        Sounds.Add("SOUND_TBOX_AH", 40207)
        Sounds.Add("SOUND_TBOX_AI", 40208)
        Sounds.Add("SOUND_TBOX_AJ", 40209)
        Sounds.Add("SOUND_TBOX_AK", 40210)
        Sounds.Add("SOUND_TBOX_AL", 40211)
        Sounds.Add("SOUND_TBOX_AM", 40212)
        Sounds.Add("SOUND_TBOX_AN", 40213)
        Sounds.Add("SOUND_TBOX_AO", 40214)
        Sounds.Add("SOUND_TBOX_AP", 40215)
        Sounds.Add("SOUND_TBOX_AQ", 40216)
        Sounds.Add("SOUND_TBOX_AR", 40217)
        Sounds.Add("SOUND_TBOX_AS", 40218)
        Sounds.Add("SOUND_TBOX_AT", 40219)
        Sounds.Add("SOUND_TBOX_AU", 40220)
        Sounds.Add("SOUND_TBOX_AV", 40221)
        Sounds.Add("SOUND_TBOX_AW", 40222)
        Sounds.Add("SOUND_TBOX_AX", 40223)
        Sounds.Add("SOUND_TBOX_AY", 40224)
        Sounds.Add("SOUND_TBOX_AZ", 40225)
        Sounds.Add("SOUND_TBOX_BA", 40226)
        Sounds.Add("SOUND_TBOX_BB", 40227)
        Sounds.Add("SOUND_TBOX_BC", 40228)
        Sounds.Add("SOUND_TBOX_BD", 40229)
        Sounds.Add("SOUND_TBOX_BE", 40230)
        Sounds.Add("SOUND_TBOX_BF", 40231)
        Sounds.Add("SOUND_TBOX_BG", 40232)
        Sounds.Add("SOUND_TBOX_BH", 40233)
        Sounds.Add("SOUND_TBOX_BI", 40234)
        Sounds.Add("SOUND_TBOX_BJ", 40235)
        Sounds.Add("SOUND_TBOX_BK", 40236)
        Sounds.Add("SOUND_TBOX_BL", 40237)
        Sounds.Add("SOUND_TBOX_BM", 40238)
        Sounds.Add("SOUND__TEMPEST_SHIELD_LOOP", 40400)
        Sounds.Add("SOUND__TEMPEST_ENEMYSHOOT", 40401)
        Sounds.Add("SOUND__TEMPEST_EXPLOSION", 40402)
        Sounds.Add("SOUND__TEMPEST_GAMEOVER", 40403)
        Sounds.Add("SOUND__TEMPEST_HIGHLIGHT", 40404)
        Sounds.Add("SOUND__TEMPEST_PICKUP", 40405)
        Sounds.Add("SOUND__TEMPEST_PLAYERSHOOT", 40406)
        Sounds.Add("SOUND__TEMPEST_SELECT", 40407)
        Sounds.Add("SOUND__TEMPEST_WARP", 40408)
        Sounds.Add("SOUND_GREEN_GOO_HUM", 40600)
        Sounds.Add("SOUND_TORX_AA", 40800)
        Sounds.Add("SOUND_TORX_AB", 40801)
        Sounds.Add("SOUND_TORX_AC", 40802)
        Sounds.Add("SOUND_TORX_AD", 40803)
        Sounds.Add("SOUND_TORX_AE", 40804)
        Sounds.Add("SOUND_TORX_AF", 40805)
        Sounds.Add("SOUND_TORX_AG", 40806)
        Sounds.Add("SOUND_TORX_AH", 40807)
        Sounds.Add("SOUND_TORX_AI", 40808)
        Sounds.Add("SOUND_TORX_AJ", 40809)
        Sounds.Add("SOUND_TORX_AK", 40810)
        Sounds.Add("SOUND_TORX_AL", 40811)
        Sounds.Add("SOUND_TORX_AM", 40812)
        Sounds.Add("SOUND_TORX_AN", 40813)
        Sounds.Add("SOUND_TORX_AO", 40814)
        Sounds.Add("SOUND_TORX_AP", 40815)
        Sounds.Add("SOUND_TORX_AQ", 40816)
        Sounds.Add("SOUND_TORX_AR", 40817)
        Sounds.Add("SOUND_TORX_AS", 40818)
        Sounds.Add("SOUND_TORX_AT", 40819)
        Sounds.Add("SOUND_TORX_AU", 40820)
        Sounds.Add("SOUND_TRU1_AA", 41000)
        Sounds.Add("SOUND_TRU1_AB", 41001)
        Sounds.Add("SOUND_TRU1_AC", 41002)
        Sounds.Add("SOUND_TRU1_AD", 41003)
        Sounds.Add("SOUND_TRU1_AE", 41004)
        Sounds.Add("SOUND_TRU1_AF", 41005)
        Sounds.Add("SOUND_TRU1_AG", 41006)
        Sounds.Add("SOUND_TRU1_AH", 41007)
        Sounds.Add("SOUND_TRU1_BA", 41008)
        Sounds.Add("SOUND_TRU1_BB", 41009)
        Sounds.Add("SOUND_TRU1_BC", 41010)
        Sounds.Add("SOUND_TRU1_BD", 41011)
        Sounds.Add("SOUND_TRU1_BE", 41012)
        Sounds.Add("SOUND_TRU1_BF", 41013)
        Sounds.Add("SOUND_TRU1_BG", 41014)
        Sounds.Add("SOUND_TRU1_BH", 41015)
        Sounds.Add("SOUND_TRU1_CA", 41016)
        Sounds.Add("SOUND_TRU1_CB", 41017)
        Sounds.Add("SOUND_TRU1_CC", 41018)
        Sounds.Add("SOUND_TRU1_CD", 41019)
        Sounds.Add("SOUND_TRU1_CE", 41020)
        Sounds.Add("SOUND_TRU1_CF", 41021)
        Sounds.Add("SOUND_TRU1_CG", 41022)
        Sounds.Add("SOUND_TRU1_CH", 41023)
        Sounds.Add("SOUND_TRU1_DA", 41024)
        Sounds.Add("SOUND_TRU1_DB", 41025)
        Sounds.Add("SOUND_TRU1_DC", 41026)
        Sounds.Add("SOUND_TRU1_DD", 41027)
        Sounds.Add("SOUND_TRU1_DE", 41028)
        Sounds.Add("SOUND_TRU1_DF", 41029)
        Sounds.Add("SOUND_TRU1_DG", 41030)
        Sounds.Add("SOUND_TRU1_DH", 41031)
        Sounds.Add("SOUND_TRU1_FA", 41032)
        Sounds.Add("SOUND_TRU1_FB", 41033)
        Sounds.Add("SOUND_TRU1_FC", 41034)
        Sounds.Add("SOUND_TRU1_FD", 41035)
        Sounds.Add("SOUND_TRU1_FE", 41036)
        Sounds.Add("SOUND_TRU1_FF", 41037)
        Sounds.Add("SOUND_TRU1_FG", 41038)
        Sounds.Add("SOUND_TRU1_FH", 41039)
        Sounds.Add("SOUND_TRU1_ZA", 41040)
        Sounds.Add("SOUND_TRU1_ZB", 41041)
        Sounds.Add("SOUND_TRU1_ZC", 41042)
        Sounds.Add("SOUND_TRU2_AA", 41200)
        Sounds.Add("SOUND_TRU2_AB", 41201)
        Sounds.Add("SOUND_TRU2_AC", 41202)
        Sounds.Add("SOUND_TRU2_AD", 41203)
        Sounds.Add("SOUND_TRU2_AE", 41204)
        Sounds.Add("SOUND_TRU2_BA", 41205)
        Sounds.Add("SOUND_TRU2_BB", 41206)
        Sounds.Add("SOUND_TRU2_BC", 41207)
        Sounds.Add("SOUND_TRU2_CA", 41208)
        Sounds.Add("SOUND_TRU2_CB", 41209)
        Sounds.Add("SOUND_TRU2_DA", 41210)
        Sounds.Add("SOUND_TRU2_DB", 41211)
        Sounds.Add("SOUND_TRU2_DC", 41212)
        Sounds.Add("SOUND_TRU2_DD", 41213)
        Sounds.Add("SOUND_TRU2_DE", 41214)
        Sounds.Add("SOUND_TRU2_DF", 41215)
        Sounds.Add("SOUND_TRU2_EA", 41216)
        Sounds.Add("SOUND_TRU2_EB", 41217)
        Sounds.Add("SOUND_TRU2_EC", 41218)
        Sounds.Add("SOUND_TRU2_ED", 41219)
        Sounds.Add("SOUND_TRU2_EF", 41220)
        Sounds.Add("SOUND_TRU2_EG", 41221)
        Sounds.Add("SOUND_TRU2_FA", 41222)
        Sounds.Add("SOUND_TRU2_FB", 41223)
        Sounds.Add("SOUND_TRU2_FC", 41224)
        Sounds.Add("SOUND_TRU2_FD", 41225)
        Sounds.Add("SOUND_TRU2_FE", 41226)
        Sounds.Add("SOUND_TRU2_FF", 41227)
        Sounds.Add("SOUND_TRU2_FG", 41228)
        Sounds.Add("SOUND_TRU2_FH", 41229)
        Sounds.Add("SOUND_TRU2_FJ", 41230)
        Sounds.Add("SOUND_TRU2_FK", 41231)
        Sounds.Add("SOUND_TRU2_GA", 41232)
        Sounds.Add("SOUND_TRU2_GB", 41233)
        Sounds.Add("SOUND_TRU2_GC", 41234)
        Sounds.Add("SOUND_TRU2_GE", 41235)
        Sounds.Add("SOUND_TRU2_GF", 41236)
        Sounds.Add("SOUND_TRU2_GG", 41237)
        Sounds.Add("SOUND_TRU2_GH", 41238)
        Sounds.Add("SOUND_TRU2_GJ", 41239)
        Sounds.Add("SOUND_TRU2_HA", 41240)
        Sounds.Add("SOUND_TRU2_HB", 41241)
        Sounds.Add("SOUND_TRU2_HC", 41242)
        Sounds.Add("SOUND_TRU2_HD", 41243)
        Sounds.Add("SOUND_TRU2_HE", 41244)
        Sounds.Add("SOUND_TRU2_HF", 41245)
        Sounds.Add("SOUND_TRU2_HG", 41246)
        Sounds.Add("SOUND_TRU2_HH", 41247)
        Sounds.Add("SOUND_TRU2_HJ", 41248)
        Sounds.Add("SOUND_TRU2_HK", 41249)
        Sounds.Add("SOUND_TRU2_HL", 41250)
        Sounds.Add("SOUND_TRU2_HM", 41251)
        Sounds.Add("SOUND_TRU2_HN", 41252)
        Sounds.Add("SOUND_TRU2_HO", 41253)
        Sounds.Add("SOUND_TRU2_JA", 41254)
        Sounds.Add("SOUND_TRU2_JB", 41255)
        Sounds.Add("SOUND_TRU2_KA", 41256)
        Sounds.Add("SOUND_TRU2_KB", 41257)
        Sounds.Add("SOUND_TRU2_KC", 41258)
        Sounds.Add("SOUND_TRU2_KD", 41259)
        Sounds.Add("SOUND_TRU2_LA", 41260)
        Sounds.Add("SOUND_TRU2_LB", 41261)
        Sounds.Add("SOUND_TRU2_LC", 41262)
        Sounds.Add("SOUND_TRU2_MA", 41263)
        Sounds.Add("SOUND_TRU2_MB", 41264)
        Sounds.Add("SOUND_TRU2_MC", 41265)
        Sounds.Add("SOUND_TRU2_NA", 41266)
        Sounds.Add("SOUND_TRU2_NB", 41267)
        Sounds.Add("SOUND_TRU2_OA", 41268)
        Sounds.Add("SOUND_TRU2_OC", 41269)
        Sounds.Add("SOUND_TRU2_OD", 41270)
        Sounds.Add("SOUND_TRU2_OE", 41271)
        Sounds.Add("SOUND_TRU2_OF", 41272)
        Sounds.Add("SOUND_TRUX_AA", 41400)
        Sounds.Add("SOUND_TRUX_AB", 41401)
        Sounds.Add("SOUND_TRUX_AC", 41402)
        Sounds.Add("SOUND_TRUX_AD", 41403)
        Sounds.Add("SOUND_TRUX_AE", 41404)
        Sounds.Add("SOUND_TRUX_AF", 41405)
        Sounds.Add("SOUND_TRUX_AG", 41406)
        Sounds.Add("SOUND_TRUX_AH", 41407)
        Sounds.Add("SOUND_TRUX_AI", 41408)
        Sounds.Add("SOUND_TRUX_AJ", 41409)
        Sounds.Add("SOUND_TRUX_AK", 41410)
        Sounds.Add("SOUND_TRUX_AL", 41411)
        Sounds.Add("SOUND_TRUX_AM", 41412)
        Sounds.Add("SOUND_TRUX_AN", 41413)
        Sounds.Add("SOUND_TRUX_AO", 41414)
        Sounds.Add("SOUND_TRUX_AP", 41415)
        Sounds.Add("SOUND_TRUX_AQ", 41416)
        Sounds.Add("SOUND_TRUX_AR", 41417)
        Sounds.Add("SOUND_TRUX_AS", 41418)
        Sounds.Add("SOUND_TRUX_AT", 41419)
        Sounds.Add("SOUND_TRUX_AU", 41420)
        Sounds.Add("SOUND_TRUX_AV", 41421)
        Sounds.Add("SOUND_TRUX_AW", 41422)
        Sounds.Add("SOUND_TRUX_AX", 41423)
        Sounds.Add("SOUND_TRUX_BA", 41424)
        Sounds.Add("SOUND_TRUX_BB", 41425)
        Sounds.Add("SOUND_TRUX_BC", 41426)
        Sounds.Add("SOUND_TRUX_BD", 41427)
        Sounds.Add("SOUND_TRUX_BE", 41428)
        Sounds.Add("SOUND_TRUX_BF", 41429)
        Sounds.Add("SOUND_TRUX_BG", 41430)
        Sounds.Add("SOUND_TRUX_BH", 41431)
        Sounds.Add("SOUND_TRUX_BI", 41432)
        Sounds.Add("SOUND__BLAST_DOOR_SLIDE_LOOP", 41600)
        Sounds.Add("SOUND__BLAST_DOOR_SLIDE_START", 41601)
        Sounds.Add("SOUND__BLAST_DOOR_SLIDE_STOP", 41602)
        Sounds.Add("SOUND__KEYPAD_BEEP2", 41603)
        Sounds.Add("SOUND__SHOOT_CONTROLS2", 41604)
        Sounds.Add("SOUND_SECURITY_ALARM", 41800)
        Sounds.Add("SOUND_VALDED1", 42000)
        Sounds.Add("SOUND_VALDED2", 42001)
        Sounds.Add("SOUND_VALDED3", 42002)
        Sounds.Add("SOUND_VALUND1", 42003)
        Sounds.Add("SOUND_VALUND2", 42004)
        Sounds.Add("SOUND_VANK_1", 42005)
        Sounds.Add("SOUND_VANK_2", 42006)
        Sounds.Add("SOUND_VANK_3", 42007)
        Sounds.Add("SOUND_VANK_4", 42008)
        Sounds.Add("SOUND_V_HEY_1", 42009)
        Sounds.Add("SOUND_V_HEY_2", 42010)
        Sounds.Add("SOUND_V_HEY_3", 42011)
        Sounds.Add("SOUND_VCR1_AA", 42200)
        Sounds.Add("SOUND_VCR1_AB", 42201)
        Sounds.Add("SOUND_VCR1_AC", 42202)
        Sounds.Add("SOUND_VCR1_AD", 42203)
        Sounds.Add("SOUND_VCR1_AE", 42204)
        Sounds.Add("SOUND_VCR1_AF", 42205)
        Sounds.Add("SOUND_VCR1_AG", 42206)
        Sounds.Add("SOUND_VCR1_AH", 42207)
        Sounds.Add("SOUND_VCR1_AI", 42208)
        Sounds.Add("SOUND_VCR2_AA", 42400)
        Sounds.Add("SOUND_VCR2_AB", 42401)
        Sounds.Add("SOUND_VCR2_AC", 42402)
        Sounds.Add("SOUND_VCR2_AD", 42403)
        Sounds.Add("SOUND_VCR2_AE", 42404)
        Sounds.Add("SOUND_VCR2_BA", 42405)
        Sounds.Add("SOUND_VCR2_BB", 42406)
        Sounds.Add("SOUND_VCR2_BC", 42407)
        Sounds.Add("SOUND_VCR2_BE", 42408)
        Sounds.Add("SOUND_VCR2_BF", 42409)
        Sounds.Add("SOUND_VCR2_BG", 42410)
        Sounds.Add("SOUND_VCR2_BH", 42411)
        Sounds.Add("SOUND_VCR2_BJ", 42412)
        Sounds.Add("SOUND_VCR2_CA", 42413)
        Sounds.Add("SOUND_VCR2_CB", 42414)
        Sounds.Add("SOUND_VCR2_CC", 42415)
        Sounds.Add("SOUND_VCR2_CD", 42416)
        Sounds.Add("SOUND_VCR2_CE", 42417)
        Sounds.Add("SOUND_VCR2_CF", 42418)
        Sounds.Add("SOUND_VCR2_CG", 42419)
        Sounds.Add("SOUND_VCR2_DA", 42420)
        Sounds.Add("SOUND_VCR2_DB", 42421)
        Sounds.Add("SOUND_VCR2_DC", 42422)
        Sounds.Add("SOUND_VCR2_DD", 42423)
        Sounds.Add("SOUND_VCR2_DE", 42424)
        Sounds.Add("SOUND_DRINKS_CAN", 42600)
        Sounds.Add("SOUND_VENDING_EAT", 42601)
        Sounds.Add("SOUND__LIFT_LOOP", 42800)
        Sounds.Add("SOUND__SECURITY_ALARM", 42801)
        Sounds.Add("SOUND__LIFT_START", 42802)
        Sounds.Add("SOUND__LIFT_STOP", 42803)
        Sounds.Add("SOUND__VIDEO_POKER_BUTTON", 43000)
        Sounds.Add("SOUND__VIDEO_POKER_PAYOUT", 43001)
        Sounds.Add("SOUND_VO_AA", 43200)
        Sounds.Add("SOUND_VO_AB", 43201)
        Sounds.Add("SOUND_VO_AC", 43202)
        Sounds.Add("SOUND_VO_AD", 43203)
        Sounds.Add("SOUND_VO_AE", 43204)
        Sounds.Add("SOUND_VO_AF", 43205)
        Sounds.Add("SOUND_VO_AG", 43206)
        Sounds.Add("SOUND_WBOX_1", 43400)
        Sounds.Add("SOUND_WBOX_2", 43401)
        Sounds.Add("SOUND_WBOX_3", 43402)
        Sounds.Add("SOUND_WBOX_4", 43403)
        Sounds.Add("SOUND_WBOX_5", 43404)
        Sounds.Add("SOUND_WBOX_6", 43405)
        Sounds.Add("SOUND_WBOX_7", 43406)
        Sounds.Add("SOUND_WBOX_8", 43407)
        Sounds.Add("SOUND_W_BET_1", 43600)
        Sounds.Add("SOUND_W_BET_2", 43601)
        Sounds.Add("SOUND_W_LEND1", 43602)
        Sounds.Add("SOUND_W_LEND2", 43603)
        Sounds.Add("SOUND_W_LEND3", 43604)
        Sounds.Add("SOUND_W_NEM_1", 43605)
        Sounds.Add("SOUND_W_NEM_2", 43606)
        Sounds.Add("SOUND_W_NEM_3", 43607)
        Sounds.Add("SOUND_W_NMB_1", 43608)
        Sounds.Add("SOUND_W_NMB_2", 43609)
        Sounds.Add("SOUND_W_NMB_3", 43610)
        Sounds.Add("SOUND_W_NUM10", 43611)
        Sounds.Add("SOUND_W_NUM11", 43612)
        Sounds.Add("SOUND_W_NUM12", 43613)
        Sounds.Add("SOUND_W_NUM13", 43614)
        Sounds.Add("SOUND_W_NUM14", 43615)
        Sounds.Add("SOUND_W_NUM15", 43616)
        Sounds.Add("SOUND_W_NUM16", 43617)
        Sounds.Add("SOUND_W_NUM17", 43618)
        Sounds.Add("SOUND_W_NUM18", 43619)
        Sounds.Add("SOUND_W_NUM19", 43620)
        Sounds.Add("SOUND_W_NUM20", 43621)
        Sounds.Add("SOUND_W_NUM21", 43622)
        Sounds.Add("SOUND_W_NUM22", 43623)
        Sounds.Add("SOUND_W_NUM23", 43624)
        Sounds.Add("SOUND_W_NUM24", 43625)
        Sounds.Add("SOUND_W_NUM25", 43626)
        Sounds.Add("SOUND_W_NUM26", 43627)
        Sounds.Add("SOUND_W_NUM27", 43628)
        Sounds.Add("SOUND_W_NUM28", 43629)
        Sounds.Add("SOUND_W_NUM29", 43630)
        Sounds.Add("SOUND_W_NUM30", 43631)
        Sounds.Add("SOUND_W_NUM31", 43632)
        Sounds.Add("SOUND_W_NUM32", 43633)
        Sounds.Add("SOUND_W_NUM33", 43634)
        Sounds.Add("SOUND_W_NUM34", 43635)
        Sounds.Add("SOUND_W_NUM35", 43636)
        Sounds.Add("SOUND_W_NUM36", 43637)
        Sounds.Add("SOUND_W_NUM_0", 43638)
        Sounds.Add("SOUND_W_NUM_1", 43639)
        Sounds.Add("SOUND_W_NUM_2", 43640)
        Sounds.Add("SOUND_W_NUM_3", 43641)
        Sounds.Add("SOUND_W_NUM_4", 43642)
        Sounds.Add("SOUND_W_NUM_5", 43643)
        Sounds.Add("SOUND_W_NUM_6", 43644)
        Sounds.Add("SOUND_W_NUM_7", 43645)
        Sounds.Add("SOUND_W_NUM_8", 43646)
        Sounds.Add("SOUND_W_NUM_9", 43647)
        Sounds.Add("SOUND_W_PWIN1", 43648)
        Sounds.Add("SOUND_W_PWIN2", 43649)
        Sounds.Add("SOUND_W_PWIN3", 43650)
        Sounds.Add("SOUND_W_REG_1", 43651)
        Sounds.Add("SOUND_W_REG_2", 43652)
        Sounds.Add("SOUND_W_THX_1", 43653)
        Sounds.Add("SOUND_W_THX_2", 43654)
        Sounds.Add("SOUND_W_WEEL1", 43655)
        Sounds.Add("SOUND_W_WEEL2", 43656)
        Sounds.Add("SOUND_W_WEEL3", 43657)
        Sounds.Add("SOUND_W_WEEL4", 43658)
        Sounds.Add("SOUND_W_WEEL5", 43659)
        Sounds.Add("SOUND_W_WEEL6", 43660)
        Sounds.Add("SOUND_W_WEEL7", 43661)
        Sounds.Add("SOUND_W_WINS1", 43662)
        Sounds.Add("SOUND_W_WINS2", 43663)
        Sounds.Add("SOUND_W_WINS3", 43664)
        Sounds.Add("SOUND_WUZ1_AA", 43800)
        Sounds.Add("SOUND_WUZ1_AB", 43801)
        Sounds.Add("SOUND_WUZ1_BA", 43802)
        Sounds.Add("SOUND_WUZ1_BB", 43803)
        Sounds.Add("SOUND_WUZ1_CA", 43804)
        Sounds.Add("SOUND_WUZ1_CB", 43805)
        Sounds.Add("SOUND_WUZ1_CC", 43806)
        Sounds.Add("SOUND_WUZ1_CD", 43807)
        Sounds.Add("SOUND_WUZ1_CE", 43808)
        Sounds.Add("SOUND_WUZ1_CG", 43809)
        Sounds.Add("SOUND_WUZ1_CH", 43810)
        Sounds.Add("SOUND_WUZ1_DA", 43811)
        Sounds.Add("SOUND_WUZ1_DB", 43812)
        Sounds.Add("SOUND_WUZ1_DC", 43813)
        Sounds.Add("SOUND_WUZ1_DD", 43814)
        Sounds.Add("SOUND_WUZ1_EA", 43815)
        Sounds.Add("SOUND_WUZ1_EB", 43816)
        Sounds.Add("SOUND_WUZ1_EC", 43817)
        Sounds.Add("SOUND_WUZ1_EE", 43818)
        Sounds.Add("SOUND_WUZ1_FA", 43819)
        Sounds.Add("SOUND_WUZ1_FB", 43820)
        Sounds.Add("SOUND_WUZ1_FC", 43821)
        Sounds.Add("SOUND_WUZ1_FD", 43822)
        Sounds.Add("SOUND_WUZ1_FE", 43823)
        Sounds.Add("SOUND_WUZ1_GA", 43824)
        Sounds.Add("SOUND_WUZ1_GB", 43825)
        Sounds.Add("SOUND_WUZ1_GC", 43826)
        Sounds.Add("SOUND_WUZ1_GD", 43827)
        Sounds.Add("SOUND_WUZ1_GE", 43828)
        Sounds.Add("SOUND_WUZ1_GF", 43829)
        Sounds.Add("SOUND_WUZ1_GG", 43830)
        Sounds.Add("SOUND_WUZ1_GH", 43831)
        Sounds.Add("SOUND_WUZ1_GJ", 43832)
        Sounds.Add("SOUND_WUZ1_GK", 43833)
        Sounds.Add("SOUND_WUZ1_GL", 43834)
        Sounds.Add("SOUND_WUZ1_GM", 43835)
        Sounds.Add("SOUND_WUZ1_HA", 43836)
        Sounds.Add("SOUND_WUZ1_HB", 43837)
        Sounds.Add("SOUND_WUZ1_HC", 43838)
        Sounds.Add("SOUND_WUZ1_JA", 43839)
        Sounds.Add("SOUND_WUZ1_JB", 43840)
        Sounds.Add("SOUND_WUZ1_KA", 43841)
        Sounds.Add("SOUND_WUZ1_KB", 43842)
        Sounds.Add("SOUND_WUZ1_KC", 43843)
        Sounds.Add("SOUND_WUZ1_KD", 43844)
        Sounds.Add("SOUND_WUZ1_LA", 43845)
        Sounds.Add("SOUND_WUZ1_LB", 43846)
        Sounds.Add("SOUND_WUZ1_LC", 43847)
        Sounds.Add("SOUND_WUZ1_LD", 43848)
        Sounds.Add("SOUND_WUZ1_MA", 43849)
        Sounds.Add("SOUND_WUZ1_MB", 43850)
        Sounds.Add("SOUND_WUZ1_MC", 43851)
        Sounds.Add("SOUND_WUZ1_MD", 43852)
        Sounds.Add("SOUND_WUZ1_ME", 43853)
        Sounds.Add("SOUND_WUZ1_MF", 43854)
        Sounds.Add("SOUND_WUZ1_MG", 43855)
        Sounds.Add("SOUND_WUZ1_MH", 43856)
        Sounds.Add("SOUND_WUZ1_MJ", 43857)
        Sounds.Add("SOUND_WUZ1_NA", 43858)
        Sounds.Add("SOUND_WUZ1_NB", 43859)
        Sounds.Add("SOUND_WUZ1_NC", 43860)
        Sounds.Add("SOUND_WUZ1_ND", 43861)
        Sounds.Add("SOUND_WUZ1_NE", 43862)
        Sounds.Add("SOUND_WUZ1_NF", 43863)
        Sounds.Add("SOUND_WUZ1_NG", 43864)
        Sounds.Add("SOUND_WUZ1_NH", 43865)
        Sounds.Add("SOUND_WUZ1_NJ", 43866)
        Sounds.Add("SOUND_WUZ1_NK", 43867)
        Sounds.Add("SOUND_WUZ1_NL", 43868)
        Sounds.Add("SOUND_WUZ1_NM", 43869)
        Sounds.Add("SOUND_WUZ1_NN", 43870)
        Sounds.Add("SOUND_WUZ1_NO", 43871)
        Sounds.Add("SOUND_WUZ1_NP", 43872)
        Sounds.Add("SOUND_WUZ1_NQ", 43873)
        Sounds.Add("SOUND_WUZ1_OA", 43874)
        Sounds.Add("SOUND_WUZ1_OB", 43875)
        Sounds.Add("SOUND_WUZ1_OC", 43876)
        Sounds.Add("SOUND_WUZ1_OD", 43877)
        Sounds.Add("SOUND_WUZ1_OE", 43878)
        Sounds.Add("SOUND_WUZ1_OF", 43879)
        Sounds.Add("SOUND_WUZ1_OG", 43880)
        Sounds.Add("SOUND_WUZ1_OH", 43881)
        Sounds.Add("SOUND_WUZ1_OJ", 43882)
        Sounds.Add("SOUND_WUZ1_OK", 43883)
        Sounds.Add("SOUND_WUZ1_OL", 43884)
        Sounds.Add("SOUND_WUZ1_OM", 43885)
        Sounds.Add("SOUND_WUZ1_ON", 43886)
        Sounds.Add("SOUND_WUZ1_OO", 43887)
        Sounds.Add("SOUND_WUZ1_OP", 43888)
        Sounds.Add("SOUND_WUZ1_OQ", 43889)
        Sounds.Add("SOUND_WUZ1_OR", 43890)
        Sounds.Add("SOUND_WUZ1_OS", 43891)
        Sounds.Add("SOUND_WUZ1_PA", 43892)
        Sounds.Add("SOUND_WUZ1_PB", 43893)
        Sounds.Add("SOUND_WUZ1_QA", 43894)
        Sounds.Add("SOUND_WUZ1_QB", 43895)
        Sounds.Add("SOUND_WUZ1_QC", 43896)
        Sounds.Add("SOUND_WUZ1_QD", 43897)
        Sounds.Add("SOUND_WUZ1_QE", 43898)
        Sounds.Add("SOUND_WUZ1_QF", 43899)
        Sounds.Add("SOUND_WUZ1_QG", 43900)
        Sounds.Add("SOUND_WUZ1_QH", 43901)
        Sounds.Add("SOUND_WUZ1_RA", 43902)
        Sounds.Add("SOUND_WUZ1_RB", 43903)
        Sounds.Add("SOUND_WUZ1_RC", 43904)
        Sounds.Add("SOUND_WUZ1_RD", 43905)
        Sounds.Add("SOUND_WUZ2_AA", 44000)
        Sounds.Add("SOUND_WUZ2_AB", 44001)
        Sounds.Add("SOUND_WUZ2_AC", 44002)
        Sounds.Add("SOUND_WUZ2_BA", 44003)
        Sounds.Add("SOUND_WUZ2_BB", 44004)
        Sounds.Add("SOUND_WUZ2_BC", 44005)
        Sounds.Add("SOUND_WUZ2_CA", 44006)
        Sounds.Add("SOUND_WUZ2_CB", 44007)
        Sounds.Add("SOUND_WUZ2_CC", 44008)
        Sounds.Add("SOUND_WUZ2_DA", 44009)
        Sounds.Add("SOUND_WUZ2_DB", 44010)
        Sounds.Add("SOUND_WUZ2_DC", 44011)
        Sounds.Add("SOUND_WUZ2_EA", 44012)
        Sounds.Add("SOUND_WUZ2_EB", 44013)
        Sounds.Add("SOUND_WUZ2_EC", 44014)
        Sounds.Add("SOUND_WUZ2_FA", 44015)
        Sounds.Add("SOUND_WUZ2_FB", 44016)
        Sounds.Add("SOUND_WUZ2_FC", 44017)
        Sounds.Add("SOUND_WUZ2_GA", 44018)
        Sounds.Add("SOUND_WUZ2_GB", 44019)
        Sounds.Add("SOUND_WUZ2_GC", 44020)
        Sounds.Add("SOUND_WUZ2_HA", 44021)
        Sounds.Add("SOUND_WUZ2_HB", 44022)
        Sounds.Add("SOUND_WUZ2_HC", 44023)
        Sounds.Add("SOUND_WUZ2_JA", 44024)
        Sounds.Add("SOUND_WUZ2_JB", 44025)
        Sounds.Add("SOUND_WUZ2_JC", 44026)
        Sounds.Add("SOUND_WUZ2_KA", 44027)
        Sounds.Add("SOUND_WUZ2_KB", 44028)
        Sounds.Add("SOUND_WUZ2_KC", 44029)
        Sounds.Add("SOUND_WUZ2_LA", 44030)
        Sounds.Add("SOUND_WUZ2_LB", 44031)
        Sounds.Add("SOUND_WUZ2_LC", 44032)
        Sounds.Add("SOUND_WUZ2_MA", 44033)
        Sounds.Add("SOUND_WUZ2_MB", 44034)
        Sounds.Add("SOUND_WUZ2_MC", 44035)
        Sounds.Add("SOUND_WUZ2_NA", 44036)
        Sounds.Add("SOUND_WUZ2_NB", 44037)
        Sounds.Add("SOUND_WUZ2_NC", 44038)
        Sounds.Add("SOUND_WUZ2_OA", 44039)
        Sounds.Add("SOUND_WUZ2_OB", 44040)
        Sounds.Add("SOUND_WUZ2_OC", 44041)
        Sounds.Add("SOUND_WUZ2_PA", 44042)
        Sounds.Add("SOUND_WUZ2_PB", 44043)
        Sounds.Add("SOUND_WUZ2_PC", 44044)
        Sounds.Add("SOUND_WUZ2_QA", 44045)
        Sounds.Add("SOUND_WUZ2_QB", 44046)
        Sounds.Add("SOUND_WUZ2_QC", 44047)
        Sounds.Add("SOUND_WUZ2_RA", 44048)
        Sounds.Add("SOUND_WUZ2_RB", 44049)
        Sounds.Add("SOUND_WUZ2_RC", 44050)
        Sounds.Add("SOUND_WUZ2_SA", 44051)
        Sounds.Add("SOUND_WUZ2_SB", 44052)
        Sounds.Add("SOUND_WUZ2_SC", 44053)
        Sounds.Add("SOUND_WUZ2_TA", 44054)
        Sounds.Add("SOUND_WUZ2_TB", 44055)
        Sounds.Add("SOUND_WUZ2_TC", 44056)
        Sounds.Add("SOUND_WUZ2_UA", 44057)
        Sounds.Add("SOUND_WUZ2_UB", 44058)
        Sounds.Add("SOUND_WUZ2_UC", 44059)
        Sounds.Add("SOUND_WUZ2_VA", 44060)
        Sounds.Add("SOUND_WUZ2_VB", 44061)
        Sounds.Add("SOUND_WUZ2_VC", 44062)
        Sounds.Add("SOUND_WUZ2_WA", 44063)
        Sounds.Add("SOUND_WUZ2_WB", 44064)
        Sounds.Add("SOUND_WUZ2_WC", 44065)
        Sounds.Add("SOUND_WUZ2_XA", 44066)
        Sounds.Add("SOUND_WUZ2_XB", 44067)
        Sounds.Add("SOUND_WUZ2_XC", 44068)
        Sounds.Add("SOUND_WUZ2_XD", 44069)
        Sounds.Add("SOUND_WUZ2_XE", 44070)
        Sounds.Add("SOUND_WUZ2_XF", 44071)
        Sounds.Add("SOUND_WUZ2_YA", 44072)
        Sounds.Add("SOUND_WUZ2_YB", 44073)
        Sounds.Add("SOUND_WUZ2_YC", 44074)
        Sounds.Add("SOUND_WUZ2_YD", 44075)
        Sounds.Add("SOUND_WUZ2_YE", 44076)
        Sounds.Add("SOUND_WUZ2_YF", 44077)
        Sounds.Add("SOUND_WUZ2_YG", 44078)
        Sounds.Add("SOUND_WUZ2_YH", 44079)
        Sounds.Add("SOUND_WUZ2_YJ", 44080)
        Sounds.Add("SOUND_WUZ2_YK", 44081)
        Sounds.Add("SOUND_WUZ2_YL", 44082)
        Sounds.Add("SOUND_WUZ2_ZA", 44083)
        Sounds.Add("SOUND_WUZ2_ZB", 44084)
        Sounds.Add("SOUND_WUZ2_ZC", 44085)
        Sounds.Add("SOUND_WUZ2_ZD", 44086)
        Sounds.Add("SOUND_WUZ2_ZE", 44087)
        Sounds.Add("SOUND_WUZ2_ZF", 44088)
        Sounds.Add("SOUND_WUZ2_ZG", 44089)
        Sounds.Add("SOUND_WUZ2_ZH", 44090)
        Sounds.Add("SOUND_WUZ2_ZJ", 44091)
        Sounds.Add("SOUND_WUZ2_ZK", 44092)
        Sounds.Add("SOUND_WUZ2_ZL", 44093)
        Sounds.Add("SOUND_WUZ2_ZM", 44094)
        Sounds.Add("SOUND_WUZ2_ZN", 44095)
        Sounds.Add("SOUND_WUZ2_ZO", 44096)
        Sounds.Add("SOUND_WUZ2_ZP", 44097)
        Sounds.Add("SOUND_WUZ2_ZQ", 44098)
        Sounds.Add("SOUND_WUZ2_ZR", 44099)
        Sounds.Add("SOUND_WUZ2_ZS", 44100)
        Sounds.Add("SOUND_WUZ2_ZT", 44101)
        Sounds.Add("SOUND_WUZ2_ZU", 44102)
        Sounds.Add("SOUND_WUZ2_ZV", 44103)
        Sounds.Add("SOUND_WUZ2_ZW", 44104)
        Sounds.Add("SOUND_WUZ2_ZX", 44105)
        Sounds.Add("SOUND_WUZ2_ZY", 44106)
        Sounds.Add("SOUND_WUZ2_ZZ", 44107)
        Sounds.Add("SOUND_WUZ4_AA", 44200)
        Sounds.Add("SOUND_WUZ4_AB", 44201)
        Sounds.Add("SOUND_WUZ4_BA", 44202)
        Sounds.Add("SOUND_WUZ4_CA", 44203)
        Sounds.Add("SOUND_WUZ4_CB", 44204)
        Sounds.Add("SOUND_WUZ4_CC", 44205)
        Sounds.Add("SOUND_WUZ4_DA", 44206)
        Sounds.Add("SOUND_WUZ4_EA", 44207)
        Sounds.Add("SOUND_WUZ4_EB", 44208)
        Sounds.Add("SOUND_WUZ4_FA", 44209)
        Sounds.Add("SOUND_WUZ4_FB", 44210)
        Sounds.Add("SOUND_WUZ4_GA", 44211)
        Sounds.Add("SOUND_WUZ4_GB", 44212)
        Sounds.Add("SOUND_WUZ4_GC", 44213)
        Sounds.Add("SOUND_WUZ4_HA", 44214)
        Sounds.Add("SOUND_WUZ4_JA", 44215)
        Sounds.Add("SOUND_WUZ4_JB", 44216)
        Sounds.Add("SOUND_WUZ4_JC", 44217)
        Sounds.Add("SOUND_WUZ4_JD", 44218)
        Sounds.Add("SOUND_WUZ4_JE", 44219)
        Sounds.Add("SOUND_WUZ4_JF", 44220)
        Sounds.Add("SOUND_WUZ4_KA", 44221)
        Sounds.Add("SOUND_WUZ4_KB", 44222)
        Sounds.Add("SOUND_WUZ4_KC", 44223)
        Sounds.Add("SOUND_WUZ4_KD", 44224)
        Sounds.Add("SOUND_WUZ4_KE", 44225)
        Sounds.Add("SOUND_WUZ4_LA", 44226)
        Sounds.Add("SOUND_WUZ4_LB", 44227)
        Sounds.Add("SOUND_WUZ4_LC", 44228)
        Sounds.Add("SOUND_WUZ4_LD", 44229)
        Sounds.Add("SOUND_WUZ4_LE", 44230)
        Sounds.Add("SOUND_WUZ4_MA", 44231)
        Sounds.Add("SOUND_WUZ4_MB", 44232)
        Sounds.Add("SOUND_WUZ4_MC", 44233)
        Sounds.Add("SOUND_WUZ4_MD", 44234)
        Sounds.Add("SOUND_WUZ4_ME", 44235)
        Sounds.Add("SOUND_WUZ4_MF", 44236)
        Sounds.Add("SOUND_WUZ4_MH", 44237)
        Sounds.Add("SOUND_WUZ4_MJ", 44238)
        Sounds.Add("SOUND_WUZ4_NA", 44239)
        Sounds.Add("SOUND_WUZ4_NB", 44240)
        Sounds.Add("SOUND_WUZ4_NC", 44241)
        Sounds.Add("SOUND_WUZ4_NE", 44242)
        Sounds.Add("SOUND_WUZ4_NF", 44243)
        Sounds.Add("SOUND_WUZ4_OA", 44244)
        Sounds.Add("SOUND_WUZ4_OB", 44245)
        Sounds.Add("SOUND_WUZ4_PA", 44246)
        Sounds.Add("SOUND_WUZ4_ZA", 44247)
        Sounds.Add("SOUND_WUZX_AA", 44400)
        Sounds.Add("SOUND_WUZX_AB", 44401)
        Sounds.Add("SOUND_WUZX_AC", 44402)
        Sounds.Add("SOUND_WUZX_AD", 44403)
        Sounds.Add("SOUND_WUZX_AE", 44404)
        Sounds.Add("SOUND_WUZX_AF", 44405)
        Sounds.Add("SOUND_WUZX_AG", 44406)
        Sounds.Add("SOUND_WUZX_AH", 44407)
        Sounds.Add("SOUND_WUZX_AI", 44408)
        Sounds.Add("SOUND_WUZX_AJ", 44409)
        Sounds.Add("SOUND_WUZX_AK", 44410)
        Sounds.Add("SOUND_WUZX_AL", 44411)
        Sounds.Add("SOUND_WUZX_AM", 44412)
        Sounds.Add("SOUND_WUZX_AN", 44413)
        Sounds.Add("SOUND_WUZX_AO", 44414)
        Sounds.Add("SOUND_WUZX_AP", 44415)
        Sounds.Add("SOUND_WUZX_AQ", 44416)
        Sounds.Add("SOUND_WUZX_AR", 44417)
        Sounds.Add("SOUND_WUZX_AS", 44418)
        Sounds.Add("SOUND_WUZX_AT", 44419)
        Sounds.Add("SOUND_WUZX_AU", 44420)
        Sounds.Add("SOUND_WUZX_AV", 44421)
        Sounds.Add("SOUND_WUZX_AW", 44422)
        Sounds.Add("SOUND_WUZX_AX", 44423)
        Sounds.Add("SOUND_WUZX_AY", 44424)
        Sounds.Add("SOUND_WUZX_AZ", 44425)
        Sounds.Add("SOUND_WUZX_BA", 44426)
        Sounds.Add("SOUND_WUZX_BB", 44427)
        Sounds.Add("SOUND_WUZX_BC", 44428)
        Sounds.Add("SOUND_WUZX_BD", 44429)
        Sounds.Add("SOUND_WUZX_BE", 44430)
        Sounds.Add("SOUND_WUZX_BF", 44431)
        Sounds.Add("SOUND_WUZX_BG", 44432)
        Sounds.Add("SOUND_WUZX_BH", 44433)
        Sounds.Add("SOUND_WUZX_BI", 44434)
        Sounds.Add("SOUND_WUZX_BJ", 44435)
        Sounds.Add("SOUND_WUZX_BK", 44436)
        Sounds.Add("SOUND_WUZX_BL", 44437)
        Sounds.Add("SOUND_WUZX_BM", 44438)
        Sounds.Add("SOUND_WUZX_BN", 44439)
        Sounds.Add("SOUND_WUZX_BO", 44440)
        Sounds.Add("SOUND_WUZX_BP", 44441)
        Sounds.Add("SOUND_WUZX_BQ", 44442)
        Sounds.Add("SOUND_WUZX_BR", 44443)
        Sounds.Add("SOUND_WUZX_BS", 44444)
        Sounds.Add("SOUND_ZER1_AA", 44600)
        Sounds.Add("SOUND_ZER1_AB", 44601)
        Sounds.Add("SOUND_ZER1_AC", 44602)
        Sounds.Add("SOUND_ZER1_AD", 44603)
        Sounds.Add("SOUND_ZER1_AE", 44604)
        Sounds.Add("SOUND_ZER1_BA", 44605)
        Sounds.Add("SOUND_ZER1_BB", 44606)
        Sounds.Add("SOUND_ZER1_BC", 44607)
        Sounds.Add("SOUND_ZER1_BD", 44608)
        Sounds.Add("SOUND_ZER1_BE", 44609)
        Sounds.Add("SOUND_ZER1_BF", 44610)
        Sounds.Add("SOUND_ZER1_BG", 44611)
        Sounds.Add("SOUND_ZER1_BH", 44612)
        Sounds.Add("SOUND_ZER1_BJ", 44613)
        Sounds.Add("SOUND_ZER1_BK", 44614)
        Sounds.Add("SOUND_ZER1_BL", 44615)
        Sounds.Add("SOUND_ZER1_BM", 44616)
        Sounds.Add("SOUND_ZER1_BN", 44617)
        Sounds.Add("SOUND_ZER1_CA", 44618)
        Sounds.Add("SOUND_ZER1_DA", 44619)
        Sounds.Add("SOUND_ZER1_DB", 44620)
        Sounds.Add("SOUND_ZER1_EA", 44621)
        Sounds.Add("SOUND_ZER1_EB", 44622)
        Sounds.Add("SOUND_ZER1_EC", 44623)
        Sounds.Add("SOUND_ZER1_FA", 44624)
        Sounds.Add("SOUND_ZER1_FB", 44625)
        Sounds.Add("SOUND_ZER1_FC", 44626)
        Sounds.Add("SOUND_ZER1_FD", 44627)
        Sounds.Add("SOUND_ZER1_FE", 44628)
        Sounds.Add("SOUND_ZER1_FF", 44629)
        Sounds.Add("SOUND_ZER1_FG", 44630)
        Sounds.Add("SOUND_ZER1_FH", 44631)
        Sounds.Add("SOUND_ZER2_AA", 44800)
        Sounds.Add("SOUND_ZER2_AB", 44801)
        Sounds.Add("SOUND_ZER2_AC", 44802)
        Sounds.Add("SOUND_ZER2_AD", 44803)
        Sounds.Add("SOUND_ZER2_BA", 44804)
        Sounds.Add("SOUND_ZER2_BB", 44805)
        Sounds.Add("SOUND_ZER2_CA", 44806)
        Sounds.Add("SOUND_ZER2_CB", 44807)
        Sounds.Add("SOUND_ZER2_CC", 44808)
        Sounds.Add("SOUND_ZER2_CD", 44809)
        Sounds.Add("SOUND_ZER2_CE", 44810)
        Sounds.Add("SOUND_ZER2_DA", 44811)
        Sounds.Add("SOUND_ZER2_DB", 44812)
        Sounds.Add("SOUND_ZER2_DC", 44813)
        Sounds.Add("SOUND_ZER2_DD", 44814)
        Sounds.Add("SOUND_ZER2_EA", 44815)
        Sounds.Add("SOUND_ZER2_EB", 44816)
        Sounds.Add("SOUND_ZER2_FA", 44817)
        Sounds.Add("SOUND_ZER2_FB", 44818)
        Sounds.Add("SOUND_ZER2_FC", 44819)
        Sounds.Add("SOUND_ZER2_FD", 44820)
        Sounds.Add("SOUND_ZER3_AA", 45000)
        Sounds.Add("SOUND_ZER3_AB", 45001)
        Sounds.Add("SOUND_ZER3_AC", 45002)
        Sounds.Add("SOUND_ZER3_AD", 45003)
        Sounds.Add("SOUND_ZER3_AE", 45004)
        Sounds.Add("SOUND_ZER3_AF", 45005)
        Sounds.Add("SOUND_ZER3_ZA", 45006)
        Sounds.Add("SOUND_ZER3_ZB", 45007)
        Sounds.Add("SOUND_ZER3_ZC", 45008)
        Sounds.Add("SOUND_ZER3_ZD", 45009)
        Sounds.Add("SOUND_ZER3_ZE", 45010)
        Sounds.Add("SOUND_ZER3_ZF", 45011)
        Sounds.Add("SOUND_ZER4_AA", 45200)
        Sounds.Add("SOUND_ZER4_AB", 45201)
        Sounds.Add("SOUND_ZER4_AC", 45202)
        Sounds.Add("SOUND_ZER4_AD", 45203)
        Sounds.Add("SOUND_ZER4_AE", 45204)
        Sounds.Add("SOUND_ZER4_AF", 45205)
        Sounds.Add("SOUND_ZER4_BA", 45206)
        Sounds.Add("SOUND_ZER4_BB", 45207)
        Sounds.Add("SOUND_ZER4_BC", 45208)
        Sounds.Add("SOUND_ZER4_CA", 45209)
        Sounds.Add("SOUND_ZER4_CB", 45210)
        Sounds.Add("SOUND_ZER4_CC", 45211)
        Sounds.Add("SOUND_ZER4_DA", 45212)
        Sounds.Add("SOUND_ZER4_DB", 45213)
        Sounds.Add("SOUND_ZER4_DC", 45214)
        Sounds.Add("SOUND_ZER4_DD", 45215)
        Sounds.Add("SOUND_ZER4_DE", 45216)
        Sounds.Add("SOUND_ZER4_EA", 45217)
        Sounds.Add("SOUND_ZER4_EB", 45218)
        Sounds.Add("SOUND_ZER4_FA", 45219)
        Sounds.Add("SOUND_ZER4_FB", 45220)
        Sounds.Add("SOUND_ZER4_FC", 45221)
        Sounds.Add("SOUND_ZER4_FD", 45222)
        Sounds.Add("SOUND_ZER4_FE", 45223)
        Sounds.Add("SOUND_ZER4_FF", 45224)
        Sounds.Add("SOUND_ZER4_GA", 45225)
        Sounds.Add("SOUND_ZER4_HA", 45226)
        Sounds.Add("SOUND_ZER4_HB", 45227)
        Sounds.Add("SOUND_ZER4_HC", 45228)
        Sounds.Add("SOUND_ZER4_JA", 45229)
        Sounds.Add("SOUND_ZER4_JB", 45230)
        Sounds.Add("SOUND_ZER4_JC", 45231)
        Sounds.Add("SOUND_ZER4_JD", 45232)
        Sounds.Add("SOUND_ZER4_JE", 45233)
        Sounds.Add("SOUND_ZER4_JF", 45234)
        Sounds.Add("SOUND_ZER4_KA", 45235)
        Sounds.Add("SOUND_ZER4_KB", 45236)
        Sounds.Add("SOUND_ZER4_KC", 45237)
        Sounds.Add("SOUND_ZER4_LA", 45238)
        Sounds.Add("SOUND_ZER4_LB", 45239)
        Sounds.Add("SOUND_ZER4_LC", 45240)
        Sounds.Add("SOUND_ZER4_LD", 45241)
        Sounds.Add("SOUND_ZER4_MA", 45242)
        Sounds.Add("SOUND_ZER4_MB", 45243)
        Sounds.Add("SOUND_ZER4_MC", 45244)
        Sounds.Add("SOUND_ZER4_MD", 45245)
        Sounds.Add("SOUND_ZER4_NA", 45246)
        Sounds.Add("SOUND_ZER4_NB", 45247)
        Sounds.Add("SOUND_ZER4_NC", 45248)
        Sounds.Add("SOUND_ZER4_OA", 45249)
        Sounds.Add("SOUND_ZER4_OB", 45250)
        Sounds.Add("SOUND_ZER4_OC", 45251)
        Sounds.Add("SOUND_ZER4_OD", 45252)
        Sounds.Add("SOUND_ZER4_OE", 45253)
        Sounds.Add("SOUND_ZER4_OF", 45254)
        Sounds.Add("SOUND_ZER4_OG", 45255)
        Sounds.Add("SOUND_BLIP_DETECTED", 45400)
    End Sub

#End Region

#Region "Anims"

    Public Sub FillAnims()
        On Error Resume Next
        FillAnimsList()
        Dim list As String, count As Integer
        list = ""
        For Each animation In Anims
            If Not (animation._Lib Is Nothing And animation.Name Is Nothing) Then
                If list.IndexOf(animation._Lib) = -1 Then
                    Main.TreeView5.Nodes.Add(animation._Lib).Nodes.Add(animation.Name)
                    list += animation._Lib & ", "
                Else
                    Dim tmp As String()
                    tmp = Split(list, ", ")
                    For i = 0 To UBound(tmp)
                        If animation._Lib = tmp(i) Then
                            Main.TreeView5.Nodes(i).Nodes.Add(animation.Name)
                        End If
                    Next
                End If
                Splash.Label1.Text = String.Format("Loading anims ({0}/1726)", count)
                Splash.ProgressBar1.Value += 1
                count += 1
            End If
        Next
    End Sub

    Private Sub FillAnimsList()
        Splash.Label1.Text = "Loading anims..."
        Anims(0)._Lib = "AIRPORT"
        Anims(0).Name = "thrw_barl_thrw"
        Anims(1)._Lib = "Attractors"
        Anims(1).Name = "Stepsit_in"
        Anims(2)._Lib = "Attractors"
        Anims(2).Name = "Stepsit_loop"
        Anims(3)._Lib = "Attractors"
        Anims(3).Name = "Stepsit_out"
        Anims(4)._Lib = "BAR"
        Anims(4).Name = "Barcustom_get"
        Anims(5)._Lib = "BAR"
        Anims(5).Name = "Barcustom_loop"
        Anims(6)._Lib = "BAR"
        Anims(6).Name = "Barcustom_order"
        Anims(7)._Lib = "BAR"
        Anims(7).Name = "BARman_idle"
        Anims(8)._Lib = "BAR"
        Anims(8).Name = "Barserve_bottle"
        Anims(9)._Lib = "BAR"
        Anims(9).Name = "Barserve_give"
        Anims(10)._Lib = "BAR"
        Anims(10).Name = "Barserve_glass"
        Anims(11)._Lib = "BAR"
        Anims(11).Name = "Barserve_in"
        Anims(12)._Lib = "BAR"
        Anims(12).Name = "Barserve_loop"
        Anims(13)._Lib = "BAR"
        Anims(13).Name = "Barserve_order"
        Anims(14)._Lib = "BAR"
        Anims(14).Name = "dnk_stndF_loop"
        Anims(15)._Lib = "BAR"
        Anims(15).Name = "dnk_stndM_loop"
        Anims(16)._Lib = "BASEBALL"
        Anims(16).Name = "Bat_1"
        Anims(17)._Lib = "BASEBALL"
        Anims(17).Name = "Bat_2"
        Anims(18)._Lib = "BASEBALL"
        Anims(18).Name = "Bat_3"
        Anims(19)._Lib = "BASEBALL"
        Anims(19).Name = "Bat_4"
        Anims(20)._Lib = "BASEBALL"
        Anims(20).Name = "Bat_block"
        Anims(21)._Lib = "BASEBALL"
        Anims(21).Name = "Bat_Hit_1"
        Anims(22)._Lib = "BASEBALL"
        Anims(22).Name = "Bat_Hit_2"
        Anims(23)._Lib = "BASEBALL"
        Anims(23).Name = "Bat_Hit_3"
        Anims(24)._Lib = "BASEBALL"
        Anims(24).Name = "Bat_IDLE"
        Anims(25)._Lib = "BASEBALL"
        Anims(25).Name = "Bat_M"
        Anims(26)._Lib = "BASEBALL"
        Anims(26).Name = "BAT_PART"
        Anims(27)._Lib = "BASEBALL"
        Anims(27).Name = "thx to MoroJr"
        Anims(28)._Lib = "BD_FIRE"
        Anims(28).Name = "BD_Fire1"
        Anims(29)._Lib = "BD_FIRE"
        Anims(29).Name = "BD_Fire2"
        Anims(30)._Lib = "BD_FIRE"
        Anims(30).Name = "BD_Fire3"
        Anims(31)._Lib = "BD_FIRE"
        Anims(31).Name = "BD_GF_Wave"
        Anims(32)._Lib = "BD_FIRE"
        Anims(32).Name = "BD_Panic_01"
        Anims(33)._Lib = "BD_FIRE"
        Anims(33).Name = "BD_Panic_02"
        Anims(34)._Lib = "BD_FIRE"
        Anims(34).Name = "BD_Panic_03"
        Anims(35)._Lib = "BD_FIRE"
        Anims(35).Name = "BD_Panic_04"
        Anims(36)._Lib = "BD_FIRE"
        Anims(36).Name = "BD_Panic_Loop"
        Anims(37)._Lib = "BD_FIRE"
        Anims(37).Name = "Grlfrd_Kiss_03"
        Anims(38)._Lib = "BD_FIRE"
        Anims(38).Name = "M_smklean_loop"
        Anims(39)._Lib = "BD_FIRE"
        Anims(39).Name = "Playa_Kiss_03"
        Anims(40)._Lib = "BD_FIRE"
        Anims(40).Name = "wash_up"
        Anims(41)._Lib = "BEACH"
        Anims(41).Name = "bather"
        Anims(42)._Lib = "BEACH"
        Anims(42).Name = "Lay_Bac_Loop"
        Anims(43)._Lib = "BEACH"
        Anims(43).Name = "ParkSit_M_loop"
        Anims(44)._Lib = "BEACH"
        Anims(44).Name = "ParkSit_W_loop"
        Anims(45)._Lib = "BEACH"
        Anims(45).Name = "SitnWait_loop_W"
        Anims(46)._Lib = "benchpress"
        Anims(46).Name = "gym_bp_celebrate"
        Anims(47)._Lib = "benchpress"
        Anims(47).Name = "gym_bp_down"
        Anims(48)._Lib = "benchpress"
        Anims(48).Name = "gym_bp_getoff"
        Anims(49)._Lib = "benchpress"
        Anims(49).Name = "gym_bp_geton"
        Anims(50)._Lib = "benchpress"
        Anims(50).Name = "gym_bp_up_A"
        Anims(51)._Lib = "benchpress"
        Anims(51).Name = "gym_bp_up_B"
        Anims(52)._Lib = "benchpress"
        Anims(52).Name = "gym_bp_up_smooth"
        Anims(53)._Lib = "BF_injection"
        Anims(53).Name = "BF_getin_LHS"
        Anims(54)._Lib = "BF_injection"
        Anims(54).Name = "BF_getin_RHS"
        Anims(55)._Lib = "BF_injection"
        Anims(55).Name = "BF_getout_LHS"
        Anims(56)._Lib = "BF_injection"
        Anims(56).Name = "BF_getout_RHS"
        Anims(57)._Lib = "BIKED"
        Anims(57).Name = "BIKEd_Back"
        Anims(58)._Lib = "BIKED"
        Anims(58).Name = "BIKEd_drivebyFT"
        Anims(59)._Lib = "BIKED"
        Anims(59).Name = "BIKEd_drivebyLHS"
        Anims(60)._Lib = "BIKED"
        Anims(60).Name = "BIKEd_drivebyRHS"
        Anims(61)._Lib = "BIKED"
        Anims(61).Name = "BIKEd_Fwd"
        Anims(62)._Lib = "BIKED"
        Anims(62).Name = "BIKEd_getoffBACK"
        Anims(63)._Lib = "BIKED"
        Anims(63).Name = "BIKEd_getoffLHS"
        Anims(64)._Lib = "BIKED"
        Anims(64).Name = "BIKEd_getoffRHS"
        Anims(65)._Lib = "BIKED"
        Anims(65).Name = "BIKEd_hit"
        Anims(66)._Lib = "BIKED"
        Anims(66).Name = "BIKEd_jumponL"
        Anims(67)._Lib = "BIKED"
        Anims(67).Name = "BIKEd_jumponR"
        Anims(68)._Lib = "BIKED"
        Anims(68).Name = "BIKEd_kick"
        Anims(69)._Lib = "BIKED"
        Anims(69).Name = "BIKEd_Left"
        Anims(70)._Lib = "BIKED"
        Anims(70).Name = "BIKEd_passenger"
        Anims(71)._Lib = "BIKED"
        Anims(71).Name = "BIKEd_pushes"
        Anims(72)._Lib = "BIKED"
        Anims(72).Name = "BIKEd_Ride"
        Anims(73)._Lib = "BIKED"
        Anims(73).Name = "BIKEd_Right"
        Anims(74)._Lib = "BIKED"
        Anims(74).Name = "BIKEd_shuffle"
        Anims(75)._Lib = "BIKED"
        Anims(75).Name = "BIKEd_Still"
        Anims(76)._Lib = "BIKEH"
        Anims(76).Name = "BIKEh_Back"
        Anims(77)._Lib = "BIKEH"
        Anims(77).Name = "BIKEh_drivebyFT"
        Anims(78)._Lib = "BIKEH"
        Anims(78).Name = "BIKEh_drivebyLHS"
        Anims(79)._Lib = "BIKEH"
        Anims(79).Name = "BIKEh_drivebyRHS"
        Anims(80)._Lib = "BIKEH"
        Anims(80).Name = "BIKEh_Fwd"
        Anims(81)._Lib = "BIKEH"
        Anims(81).Name = "BIKEh_getoffBACK"
        Anims(82)._Lib = "BIKEH"
        Anims(82).Name = "BIKEh_getoffLHS"
        Anims(83)._Lib = "BIKEH"
        Anims(83).Name = "BIKEh_getoffRHS"
        Anims(84)._Lib = "BIKEH"
        Anims(84).Name = "BIKEh_hit"
        Anims(85)._Lib = "BIKEH"
        Anims(85).Name = "BIKEh_jumponL"
        Anims(86)._Lib = "BIKEH"
        Anims(86).Name = "BIKEh_jumponR"
        Anims(87)._Lib = "BIKEH"
        Anims(87).Name = "BIKEh_kick"
        Anims(88)._Lib = "BIKEH"
        Anims(88).Name = "BIKEh_Left"
        Anims(89)._Lib = "BIKEH"
        Anims(89).Name = "BIKEh_passenger"
        Anims(90)._Lib = "BIKEH"
        Anims(90).Name = "BIKEh_pushes"
        Anims(91)._Lib = "BIKEH"
        Anims(91).Name = "BIKEh_Ride"
        Anims(92)._Lib = "BIKEH"
        Anims(92).Name = "BIKEh_Right"
        Anims(93)._Lib = "BIKEH"
        Anims(93).Name = "BIKEh_Still"
        Anims(94)._Lib = "BIKELEAP"
        Anims(94).Name = "bk_blnce_in"
        Anims(95)._Lib = "BIKELEAP"
        Anims(95).Name = "bk_blnce_out"
        Anims(96)._Lib = "BIKELEAP"
        Anims(96).Name = "bk_jmp"
        Anims(97)._Lib = "BIKELEAP"
        Anims(97).Name = "bk_rdy_in"
        Anims(98)._Lib = "BIKELEAP"
        Anims(98).Name = "bk_rdy_out"
        Anims(99)._Lib = "BIKELEAP"
        Anims(99).Name = "struggle_cesar"
        Anims(100)._Lib = "BIKELEAP"
        Anims(100).Name = "struggle_driver"
        Anims(101)._Lib = "BIKELEAP"
        Anims(101).Name = "truck_driver"
        Anims(102)._Lib = "BIKELEAP"
        Anims(102).Name = "truck_getin"
        Anims(103)._Lib = "BIKES"
        Anims(103).Name = "BIKEs_Back"
        Anims(104)._Lib = "BIKES"
        Anims(104).Name = "BIKEs_drivebyFT"
        Anims(105)._Lib = "BIKES"
        Anims(105).Name = "BIKEs_drivebyLHS"
        Anims(106)._Lib = "BIKES"
        Anims(106).Name = "BIKEs_drivebyRHS"
        Anims(107)._Lib = "BIKES"
        Anims(107).Name = "BIKEs_Fwd"
        Anims(108)._Lib = "BIKES"
        Anims(108).Name = "BIKEs_getoffBACK"
        Anims(109)._Lib = "BIKES"
        Anims(109).Name = "BIKEs_getoffLHS"
        Anims(110)._Lib = "BIKES"
        Anims(110).Name = "BIKEs_getoffRHS"
        Anims(111)._Lib = "BIKES"
        Anims(111).Name = "BIKEs_hit"
        Anims(112)._Lib = "BIKES"
        Anims(112).Name = "BIKEs_jumponL"
        Anims(113)._Lib = "BIKES"
        Anims(113).Name = "BIKEs_jumponR"
        Anims(114)._Lib = "BIKES"
        Anims(114).Name = "BIKEs_kick"
        Anims(115)._Lib = "BIKES"
        Anims(115).Name = "BIKEs_Left"
        Anims(116)._Lib = "BIKES"
        Anims(116).Name = "BIKEs_passenger"
        Anims(117)._Lib = "BIKES"
        Anims(117).Name = "BIKEs_pushes"
        Anims(118)._Lib = "BIKES"
        Anims(118).Name = "BIKEs_Ride"
        Anims(119)._Lib = "BIKES"
        Anims(119).Name = "BIKEs_Right"
        Anims(120)._Lib = "BIKES"
        Anims(120).Name = "BIKEs_Snatch_L"
        Anims(121)._Lib = "BIKES"
        Anims(121).Name = "BIKEs_Snatch_R"
        Anims(122)._Lib = "BIKES"
        Anims(122).Name = "BIKEs_Still"
        Anims(123)._Lib = "BIKEV"
        Anims(123).Name = "BIKEv_Back"
        Anims(124)._Lib = "BIKEV"
        Anims(124).Name = "BIKEv_drivebyFT"
        Anims(125)._Lib = "BIKEV"
        Anims(125).Name = "BIKEv_drivebyLHS"
        Anims(126)._Lib = "BIKEV"
        Anims(126).Name = "BIKEv_drivebyRHS"
        Anims(127)._Lib = "BIKEV"
        Anims(127).Name = "BIKEv_Fwd"
        Anims(128)._Lib = "BIKEV"
        Anims(128).Name = "BIKEv_getoffBACK"
        Anims(129)._Lib = "BIKEV"
        Anims(129).Name = "BIKEv_getoffLHS"
        Anims(130)._Lib = "BIKEV"
        Anims(130).Name = "BIKEv_getoffRHS"
        Anims(131)._Lib = "BIKEV"
        Anims(131).Name = "BIKEv_hit"
        Anims(132)._Lib = "BIKEV"
        Anims(132).Name = "BIKEv_jumponL"
        Anims(133)._Lib = "BIKEV"
        Anims(133).Name = "BIKEv_jumponR"
        Anims(134)._Lib = "BIKEV"
        Anims(134).Name = "BIKEv_kick"
        Anims(135)._Lib = "BIKEV"
        Anims(135).Name = "BIKEv_Left"
        Anims(136)._Lib = "BIKEV"
        Anims(136).Name = "BIKEv_passenger"
        Anims(137)._Lib = "BIKEV"
        Anims(137).Name = "BIKEv_pushes"
        Anims(138)._Lib = "BIKEV"
        Anims(138).Name = "BIKEv_Ride"
        Anims(139)._Lib = "BIKEV"
        Anims(139).Name = "BIKEv_Right"
        Anims(140)._Lib = "BIKEV"
        Anims(140).Name = "BIKEv_Still"
        Anims(141)._Lib = "BIKE_DBZ"
        Anims(141).Name = "Pass_Driveby_BWD"
        Anims(142)._Lib = "BIKE_DBZ"
        Anims(142).Name = "Pass_Driveby_FWD"
        Anims(143)._Lib = "BIKE_DBZ"
        Anims(143).Name = "Pass_Driveby_LHS"
        Anims(144)._Lib = "BIKE_DBZ"
        Anims(144).Name = "Pass_Driveby_RHS"
        Anims(145)._Lib = "BLOWJOBZ"
        Anims(145).Name = "BJ_COUCH_START_W"
        Anims(146)._Lib = "BLOWJOBZ"
        Anims(146).Name = "BJ_COUCH_LOOP_W"
        Anims(147)._Lib = "BLOWJOBZ"
        Anims(147).Name = "BJ_COUCH_END_W"
        Anims(148)._Lib = "BLOWJOBZ"
        Anims(148).Name = "BJ_COUCH_START_P"
        Anims(149)._Lib = "BLOWJOBZ"
        Anims(149).Name = "BJ_COUCH_LOOP_P"
        Anims(150)._Lib = "BLOWJOBZ"
        Anims(150).Name = "BJ_COUCH_END_P"
        Anims(151)._Lib = "BLOWJOBZ"
        Anims(151).Name = "BJ_STAND_START_W"
        Anims(152)._Lib = "BLOWJOBZ"
        Anims(152).Name = "BJ_STAND_LOOP_W"
        Anims(153)._Lib = "BLOWJOBZ"
        Anims(153).Name = "BJ_STAND_END_W"
        Anims(154)._Lib = "BLOWJOBZ"
        Anims(154).Name = "BJ_STAND_START_P"
        Anims(155)._Lib = "BLOWJOBZ"
        Anims(155).Name = "BJ_STAND_LOOP_P"
        Anims(156)._Lib = "BLOWJOBZ"
        Anims(156).Name = "BJ_STAND_END_P"
        Anims(157)._Lib = "BMX"
        Anims(157).Name = "BMX_back"
        Anims(158)._Lib = "BMX"
        Anims(158).Name = "BMX_bunnyhop"
        Anims(159)._Lib = "BMX"
        Anims(159).Name = "BMX_drivebyFT"
        Anims(160)._Lib = "BMX"
        Anims(160).Name = "BMX_driveby_LHS"
        Anims(161)._Lib = "BMX"
        Anims(161).Name = "BMX_driveby_RHS"
        Anims(162)._Lib = "BMX"
        Anims(162).Name = "BMX_fwd"
        Anims(163)._Lib = "BMX"
        Anims(163).Name = "BMX_getoffBACK"
        Anims(164)._Lib = "BMX"
        Anims(164).Name = "BMX_getoffLHS"
        Anims(165)._Lib = "BMX"
        Anims(165).Name = "BMX_getoffRHS"
        Anims(166)._Lib = "BMX"
        Anims(166).Name = "BMX_jumponL"
        Anims(167)._Lib = "BMX"
        Anims(167).Name = "BMX_jumponR"
        Anims(168)._Lib = "BMX"
        Anims(168).Name = "BMX_Left"
        Anims(169)._Lib = "BMX"
        Anims(169).Name = "BMX_pedal"
        Anims(170)._Lib = "BMX"
        Anims(170).Name = "BMX_pushes"
        Anims(171)._Lib = "BMX"
        Anims(171).Name = "BMX_Ride"
        Anims(172)._Lib = "BMX"
        Anims(172).Name = "BMX_Right"
        Anims(173)._Lib = "BMX"
        Anims(173).Name = "BMX_sprint"
        Anims(174)._Lib = "BMX"
        Anims(174).Name = "BMX_still"
        Anims(175)._Lib = "BOMBER"
        Anims(175).Name = "BOM_Plant"
        Anims(176)._Lib = "BOMBER"
        Anims(176).Name = "BOM_Plant_2Idle"
        Anims(177)._Lib = "BOMBER"
        Anims(177).Name = "BOM_Plant_Crouch_In"
        Anims(178)._Lib = "BOMBER"
        Anims(178).Name = "BOM_Plant_Crouch_Out"
        Anims(179)._Lib = "BOMBER"
        Anims(179).Name = "BOM_Plant_In"
        Anims(180)._Lib = "BOMBER"
        Anims(180).Name = "BOM_Plant_Loop"
        Anims(181)._Lib = "BOX"
        Anims(181).Name = "boxhipin"
        Anims(182)._Lib = "BOX"
        Anims(182).Name = "boxhipup"
        Anims(183)._Lib = "BOX"
        Anims(183).Name = "boxshdwn"
        Anims(184)._Lib = "BOX"
        Anims(184).Name = "boxshup"
        Anims(185)._Lib = "BOX"
        Anims(185).Name = "bxhipwlk"
        Anims(186)._Lib = "BOX"
        Anims(186).Name = "bxhwlki"
        Anims(187)._Lib = "BOX"
        Anims(187).Name = "bxshwlk"
        Anims(188)._Lib = "BOX"
        Anims(188).Name = "bxshwlki"
        Anims(189)._Lib = "BOX"
        Anims(189).Name = "bxwlko"
        Anims(190)._Lib = "BOX"
        Anims(190).Name = "catch_box"
        Anims(191)._Lib = "BSKTBALL"
        Anims(191).Name = "BBALL_def_jump_shot"
        Anims(192)._Lib = "BSKTBALL"
        Anims(192).Name = "BBALL_def_loop"
        Anims(193)._Lib = "BSKTBALL"
        Anims(193).Name = "BBALL_def_stepL"
        Anims(194)._Lib = "BSKTBALL"
        Anims(194).Name = "BBALL_def_stepR"
        Anims(195)._Lib = "BSKTBALL"
        Anims(195).Name = "BBALL_Dnk"
        Anims(196)._Lib = "BSKTBALL"
        Anims(196).Name = "BBALL_Dnk_Gli"
        Anims(197)._Lib = "BSKTBALL"
        Anims(197).Name = "BBALL_Dnk_Gli_O"
        Anims(198)._Lib = "BSKTBALL"
        Anims(198).Name = "BBALL_Dnk_Lnch"
        Anims(199)._Lib = "BSKTBALL"
        Anims(199).Name = "BBALL_Dnk_Lnch_O"
        Anims(200)._Lib = "BSKTBALL"
        Anims(200).Name = "BBALL_Dnk_Lnd"
        Anims(201)._Lib = "BSKTBALL"
        Anims(201).Name = "BBALL_Dnk_O"
        Anims(202)._Lib = "BSKTBALL"
        Anims(202).Name = "BBALL_idle"
        Anims(203)._Lib = "BSKTBALL"
        Anims(203).Name = "BBALL_idle2"
        Anims(204)._Lib = "BSKTBALL"
        Anims(204).Name = "BBALL_idle2_O"
        Anims(205)._Lib = "BSKTBALL"
        Anims(205).Name = "BBALL_idleloop"
        Anims(206)._Lib = "BSKTBALL"
        Anims(206).Name = "BBALL_idleloop_O"
        Anims(207)._Lib = "BSKTBALL"
        Anims(207).Name = "BBALL_idle_O"
        Anims(208)._Lib = "BSKTBALL"
        Anims(208).Name = "BBALL_Jump_Cancel"
        Anims(209)._Lib = "BSKTBALL"
        Anims(209).Name = "BBALL_Jump_Cancel_O"
        Anims(210)._Lib = "BSKTBALL"
        Anims(210).Name = "BBALL_Jump_End"
        Anims(211)._Lib = "BSKTBALL"
        Anims(211).Name = "BBALL_Jump_Shot"
        Anims(212)._Lib = "BSKTBALL"
        Anims(212).Name = "BBALL_Jump_Shot_O"
        Anims(213)._Lib = "BSKTBALL"
        Anims(213).Name = "BBALL_Net_Dnk_O"
        Anims(214)._Lib = "BSKTBALL"
        Anims(214).Name = "BBALL_pickup"
        Anims(215)._Lib = "BSKTBALL"
        Anims(215).Name = "BBALL_pickup_O"
        Anims(216)._Lib = "BSKTBALL"
        Anims(216).Name = "BBALL_react_miss"
        Anims(217)._Lib = "BSKTBALL"
        Anims(217).Name = "BBALL_react_score"
        Anims(218)._Lib = "BSKTBALL"
        Anims(218).Name = "BBALL_run"
        Anims(219)._Lib = "BSKTBALL"
        Anims(219).Name = "BBALL_run_O"
        Anims(220)._Lib = "BSKTBALL"
        Anims(220).Name = "BBALL_SkidStop_L"
        Anims(221)._Lib = "BSKTBALL"
        Anims(221).Name = "BBALL_SkidStop_L_O"
        Anims(222)._Lib = "BSKTBALL"
        Anims(222).Name = "BBALL_SkidStop_R"
        Anims(223)._Lib = "BSKTBALL"
        Anims(223).Name = "BBALL_SkidStop_R_O"
        Anims(224)._Lib = "BSKTBALL"
        Anims(224).Name = "BBALL_walk"
        Anims(225)._Lib = "BSKTBALL"
        Anims(225).Name = "BBALL_WalkStop_L"
        Anims(226)._Lib = "BSKTBALL"
        Anims(226).Name = "BBALL_WalkStop_L_O"
        Anims(227)._Lib = "BSKTBALL"
        Anims(227).Name = "BBALL_WalkStop_R"
        Anims(228)._Lib = "BSKTBALL"
        Anims(228).Name = "BBALL_WalkStop_R_O"
        Anims(229)._Lib = "BSKTBALL"
        Anims(229).Name = "BBALL_walk_O"
        Anims(230)._Lib = "BSKTBALL"
        Anims(230).Name = "BBALL_walk_start"
        Anims(231)._Lib = "BSKTBALL"
        Anims(231).Name = "BBALL_walk_start_O"
        Anims(232)._Lib = "BUDDY"
        Anims(232).Name = "buddy_crouchfire"
        Anims(233)._Lib = "BUDDY"
        Anims(233).Name = "buddy_crouchreload"
        Anims(234)._Lib = "BUDDY"
        Anims(234).Name = "buddy_fire"
        Anims(235)._Lib = "BUDDY"
        Anims(235).Name = "buddy_fire_poor"
        Anims(236)._Lib = "BUDDY"
        Anims(236).Name = "buddy_reload"
        Anims(237)._Lib = "BUS"
        Anims(237).Name = "BUS_close"
        Anims(238)._Lib = "BUS"
        Anims(238).Name = "BUS_getin_LHS"
        Anims(239)._Lib = "BUS"
        Anims(239).Name = "BUS_getin_RHS"
        Anims(240)._Lib = "BUS"
        Anims(240).Name = "BUS_getout_LHS"
        Anims(241)._Lib = "BUS"
        Anims(241).Name = "BUS_getout_RHS"
        Anims(242)._Lib = "BUS"
        Anims(242).Name = "BUS_jacked_LHS"
        Anims(243)._Lib = "BUS"
        Anims(243).Name = "BUS_open"
        Anims(244)._Lib = "BUS"
        Anims(244).Name = "BUS_open_RHS"
        Anims(245)._Lib = "BUS"
        Anims(245).Name = "BUS_pullout_LHS"
        Anims(246)._Lib = "CAMERA"
        Anims(246).Name = "camcrch_cmon"
        Anims(247)._Lib = "CAMERA"
        Anims(247).Name = "camcrch_idleloop"
        Anims(248)._Lib = "CAMERA"
        Anims(248).Name = "camcrch_stay"
        Anims(249)._Lib = "CAMERA"
        Anims(249).Name = "camcrch_to_camstnd"
        Anims(250)._Lib = "CAMERA"
        Anims(250).Name = "camstnd_cmon"
        Anims(251)._Lib = "CAMERA"
        Anims(251).Name = "camstnd_idleloop"
        Anims(252)._Lib = "CAMERA"
        Anims(252).Name = "camstnd_lkabt"
        Anims(253)._Lib = "CAMERA"
        Anims(253).Name = "camstnd_to_camcrch"
        Anims(254)._Lib = "CAMERA"
        Anims(254).Name = "piccrch_in"
        Anims(255)._Lib = "CAMERA"
        Anims(255).Name = "piccrch_out"
        Anims(256)._Lib = "CAMERA"
        Anims(256).Name = "piccrch_take"
        Anims(257)._Lib = "CAMERA"
        Anims(257).Name = "picstnd_in"
        Anims(258)._Lib = "CAMERA"
        Anims(258).Name = "picstnd_out"
        Anims(259)._Lib = "CAMERA"
        Anims(259).Name = "picstnd_take"
        Anims(260)._Lib = "CAR"
        Anims(260).Name = "Fixn_Car_Loop"
        Anims(261)._Lib = "CAR"
        Anims(261).Name = "Fixn_Car_Out"
        Anims(262)._Lib = "CAR"
        Anims(262).Name = "flag_drop"
        Anims(263)._Lib = "CAR"
        Anims(263).Name = "Sit_relaxed"
        Anims(264)._Lib = "CAR"
        Anims(264).Name = "Tap_hand"
        Anims(265)._Lib = "CAR"
        Anims(265).Name = "Tyd2car_bump"
        Anims(266)._Lib = "CAR"
        Anims(266).Name = "Tyd2car_high"
        Anims(267)._Lib = "CAR"
        Anims(267).Name = "Tyd2car_low"
        Anims(268)._Lib = "CAR"
        Anims(268).Name = "Tyd2car_med"
        Anims(269)._Lib = "CAR"
        Anims(269).Name = "Tyd2car_TurnL"
        Anims(270)._Lib = "CAR"
        Anims(270).Name = "Tyd2car_TurnR"
        Anims(271)._Lib = "CARRY"
        Anims(271).Name = "crry_prtial"
        Anims(272)._Lib = "CARRY"
        Anims(272).Name = "liftup"
        Anims(273)._Lib = "CARRY"
        Anims(273).Name = "liftup05"
        Anims(274)._Lib = "CARRY"
        Anims(274).Name = "liftup105"
        Anims(275)._Lib = "CARRY"
        Anims(275).Name = "putdwn"
        Anims(276)._Lib = "CARRY"
        Anims(276).Name = "putdwn05"
        Anims(277)._Lib = "CARRY"
        Anims(277).Name = "putdwn105"
        Anims(278)._Lib = "CAR_CHAT"
        Anims(278).Name = "carfone_in"
        Anims(279)._Lib = "CAR_CHAT"
        Anims(279).Name = "carfone_loopA"
        Anims(280)._Lib = "CAR_CHAT"
        Anims(280).Name = "carfone_loopA_to_B"
        Anims(281)._Lib = "CAR_CHAT"
        Anims(281).Name = "carfone_loopB"
        Anims(282)._Lib = "CAR_CHAT"
        Anims(282).Name = "carfone_loopB_to_A"
        Anims(283)._Lib = "CAR_CHAT"
        Anims(283).Name = "carfone_out"
        Anims(284)._Lib = "CAR_CHAT"
        Anims(284).Name = "CAR_Sc1_BL"
        Anims(285)._Lib = "CAR_CHAT"
        Anims(285).Name = "CAR_Sc1_BR"
        Anims(286)._Lib = "CAR_CHAT"
        Anims(286).Name = "CAR_Sc1_FL"
        Anims(287)._Lib = "CAR_CHAT"
        Anims(287).Name = "CAR_Sc1_FR"
        Anims(288)._Lib = "CAR_CHAT"
        Anims(288).Name = "CAR_Sc2_FL"
        Anims(289)._Lib = "CAR_CHAT"
        Anims(289).Name = "CAR_Sc3_BR"
        Anims(290)._Lib = "CAR_CHAT"
        Anims(290).Name = "CAR_Sc3_FL"
        Anims(291)._Lib = "CAR_CHAT"
        Anims(291).Name = "CAR_Sc3_FR"
        Anims(292)._Lib = "CAR_CHAT"
        Anims(292).Name = "CAR_Sc4_BL"
        Anims(293)._Lib = "CAR_CHAT"
        Anims(293).Name = "CAR_Sc4_BR"
        Anims(294)._Lib = "CAR_CHAT"
        Anims(294).Name = "CAR_Sc4_FL"
        Anims(295)._Lib = "CAR_CHAT"
        Anims(295).Name = "CAR_Sc4_FR"
        Anims(296)._Lib = "CAR_CHAT"
        Anims(296).Name = "car_talkm_in"
        Anims(297)._Lib = "CAR_CHAT"
        Anims(297).Name = "car_talkm_loop"
        Anims(298)._Lib = "CAR_CHAT"
        Anims(298).Name = "car_talkm_out"
        Anims(299)._Lib = "CASINO"
        Anims(299).Name = "cards_in"
        Anims(300)._Lib = "CASINO"
        Anims(300).Name = "cards_loop"
        Anims(301)._Lib = "CASINO"
        Anims(301).Name = "cards_lose"
        Anims(302)._Lib = "CASINO"
        Anims(302).Name = "cards_out"
        Anims(303)._Lib = "CASINO"
        Anims(303).Name = "cards_pick_01"
        Anims(304)._Lib = "CASINO"
        Anims(304).Name = "cards_pick_02"
        Anims(305)._Lib = "CASINO"
        Anims(305).Name = "cards_raise"
        Anims(306)._Lib = "CASINO"
        Anims(306).Name = "cards_win"
        Anims(307)._Lib = "CASINO"
        Anims(307).Name = "dealone"
        Anims(308)._Lib = "CASINO"
        Anims(308).Name = "manwinb"
        Anims(309)._Lib = "CASINO"
        Anims(309).Name = "manwind"
        Anims(310)._Lib = "CASINO"
        Anims(310).Name = "Roulette_bet"
        Anims(311)._Lib = "CASINO"
        Anims(311).Name = "Roulette_in"
        Anims(312)._Lib = "CASINO"
        Anims(312).Name = "Roulette_loop"
        Anims(313)._Lib = "CASINO"
        Anims(313).Name = "Roulette_lose"
        Anims(314)._Lib = "CASINO"
        Anims(314).Name = "Roulette_out"
        Anims(315)._Lib = "CASINO"
        Anims(315).Name = "Roulette_win"
        Anims(316)._Lib = "CASINO"
        Anims(316).Name = "Slot_bet_01"
        Anims(317)._Lib = "CASINO"
        Anims(317).Name = "Slot_bet_02"
        Anims(318)._Lib = "CASINO"
        Anims(318).Name = "Slot_in"
        Anims(319)._Lib = "CASINO"
        Anims(319).Name = "Slot_lose_out"
        Anims(320)._Lib = "CASINO"
        Anims(320).Name = "Slot_Plyr"
        Anims(321)._Lib = "CASINO"
        Anims(321).Name = "Slot_wait"
        Anims(322)._Lib = "CASINO"
        Anims(322).Name = "Slot_win_out"
        Anims(323)._Lib = "CASINO"
        Anims(323).Name = "wof"
        Anims(324)._Lib = "CHAINSAW"
        Anims(324).Name = "CSAW_1"
        Anims(325)._Lib = "CHAINSAW"
        Anims(325).Name = "CSAW_2"
        Anims(326)._Lib = "CHAINSAW"
        Anims(326).Name = "CSAW_3"
        Anims(327)._Lib = "CHAINSAW"
        Anims(327).Name = "CSAW_G"
        Anims(328)._Lib = "CHAINSAW"
        Anims(328).Name = "CSAW_Hit_1"
        Anims(329)._Lib = "CHAINSAW"
        Anims(329).Name = "CSAW_Hit_2"
        Anims(330)._Lib = "CHAINSAW"
        Anims(330).Name = "CSAW_Hit_3"
        Anims(331)._Lib = "CHAINSAW"
        Anims(331).Name = "csaw_part"
        Anims(332)._Lib = "CHAINSAW"
        Anims(332).Name = "IDLE_csaw"
        Anims(333)._Lib = "CHAINSAW"
        Anims(333).Name = "WEAPON_csaw"
        Anims(334)._Lib = "CHAINSAW"
        Anims(334).Name = "WEAPON_csawlo"
        Anims(335)._Lib = "CHOPPA"
        Anims(335).Name = "CHOPPA_back"
        Anims(336)._Lib = "CHOPPA"
        Anims(336).Name = "CHOPPA_bunnyhop"
        Anims(337)._Lib = "CHOPPA"
        Anims(337).Name = "CHOPPA_drivebyFT"
        Anims(338)._Lib = "CHOPPA"
        Anims(338).Name = "CHOPPA_driveby_LHS"
        Anims(339)._Lib = "CHOPPA"
        Anims(339).Name = "CHOPPA_driveby_RHS"
        Anims(340)._Lib = "CHOPPA"
        Anims(340).Name = "CHOPPA_fwd"
        Anims(341)._Lib = "CHOPPA"
        Anims(341).Name = "CHOPPA_getoffBACK"
        Anims(342)._Lib = "CHOPPA"
        Anims(342).Name = "CHOPPA_getoffLHS"
        Anims(343)._Lib = "CHOPPA"
        Anims(343).Name = "CHOPPA_getoffRHS"
        Anims(344)._Lib = "CHOPPA"
        Anims(344).Name = "CHOPPA_jumponL"
        Anims(345)._Lib = "CHOPPA"
        Anims(345).Name = "CHOPPA_jumponR"
        Anims(346)._Lib = "CHOPPA"
        Anims(346).Name = "CHOPPA_Left"
        Anims(347)._Lib = "CHOPPA"
        Anims(347).Name = "CHOPPA_pedal"
        Anims(348)._Lib = "CHOPPA"
        Anims(348).Name = "CHOPPA_Pushes"
        Anims(349)._Lib = "CHOPPA"
        Anims(349).Name = "CHOPPA_ride"
        Anims(350)._Lib = "CHOPPA"
        Anims(350).Name = "CHOPPA_Right"
        Anims(351)._Lib = "CHOPPA"
        Anims(351).Name = "CHOPPA_sprint"
        Anims(352)._Lib = "CHOPPA"
        Anims(352).Name = "CHOPPA_Still"
        Anims(353)._Lib = "CLOTHES"
        Anims(353).Name = "CLO_Buy"
        Anims(354)._Lib = "CLOTHES"
        Anims(354).Name = "CLO_In"
        Anims(355)._Lib = "CLOTHES"
        Anims(355).Name = "CLO_Out"
        Anims(356)._Lib = "CLOTHES"
        Anims(356).Name = "CLO_Pose_Hat"
        Anims(357)._Lib = "CLOTHES"
        Anims(357).Name = "CLO_Pose_In"
        Anims(358)._Lib = "CLOTHES"
        Anims(358).Name = "CLO_Pose_In_O"
        Anims(359)._Lib = "CLOTHES"
        Anims(359).Name = "CLO_Pose_Legs"
        Anims(360)._Lib = "CLOTHES"
        Anims(360).Name = "CLO_Pose_Loop"
        Anims(361)._Lib = "CLOTHES"
        Anims(361).Name = "CLO_Pose_Out"
        Anims(362)._Lib = "CLOTHES"
        Anims(362).Name = "CLO_Pose_Out_O"
        Anims(363)._Lib = "CLOTHES"
        Anims(363).Name = "CLO_Pose_Shoes"
        Anims(364)._Lib = "CLOTHES"
        Anims(364).Name = "CLO_Pose_Torso"
        Anims(365)._Lib = "CLOTHES"
        Anims(365).Name = "CLO_Pose_Watch"
        Anims(366)._Lib = "COACH"
        Anims(366).Name = "COACH_inL"
        Anims(367)._Lib = "COACH"
        Anims(367).Name = "COACH_inR"
        Anims(368)._Lib = "COACH"
        Anims(368).Name = "COACH_opnL"
        Anims(369)._Lib = "COACH"
        Anims(369).Name = "COACH_opnR"
        Anims(370)._Lib = "COACH"
        Anims(370).Name = "COACH_outL"
        Anims(371)._Lib = "COACH"
        Anims(371).Name = "COACH_outR"
        Anims(372)._Lib = "COLT45"
        Anims(372).Name = "2guns_crouchfire"
        Anims(373)._Lib = "COLT45"
        Anims(373).Name = "colt45_crouchfire"
        Anims(374)._Lib = "COLT45"
        Anims(374).Name = "colt45_crouchreload"
        Anims(375)._Lib = "COLT45"
        Anims(375).Name = "colt45_fire"
        Anims(376)._Lib = "COLT45"
        Anims(376).Name = "colt45_fire_2hands"
        Anims(377)._Lib = "COLT45"
        Anims(377).Name = "colt45_reload"
        Anims(378)._Lib = "COLT45"
        Anims(378).Name = "sawnoff_reload"
        Anims(379)._Lib = "COP_AMBIENT"
        Anims(379).Name = "Copbrowse_in"
        Anims(380)._Lib = "COP_AMBIENT"
        Anims(380).Name = "Copbrowse_loop"
        Anims(381)._Lib = "COP_AMBIENT"
        Anims(381).Name = "Copbrowse_nod"
        Anims(382)._Lib = "COP_AMBIENT"
        Anims(382).Name = "Copbrowse_out"
        Anims(383)._Lib = "COP_AMBIENT"
        Anims(383).Name = "Copbrowse_shake"
        Anims(384)._Lib = "COP_AMBIENT"
        Anims(384).Name = "Coplook_in"
        Anims(385)._Lib = "COP_AMBIENT"
        Anims(385).Name = "Coplook_loop"
        Anims(386)._Lib = "COP_AMBIENT"
        Anims(386).Name = "Coplook_nod"
        Anims(387)._Lib = "COP_AMBIENT"
        Anims(387).Name = "Coplook_out"
        Anims(388)._Lib = "COP_AMBIENT"
        Anims(388).Name = "Coplook_shake"
        Anims(389)._Lib = "COP_AMBIENT"
        Anims(389).Name = "Coplook_think"
        Anims(390)._Lib = "COP_AMBIENT"
        Anims(390).Name = "Coplook_watch"
        Anims(391)._Lib = "COP_DVBYZ"
        Anims(391).Name = "COP_Dvby_B"
        Anims(392)._Lib = "COP_DVBYZ"
        Anims(392).Name = "COP_Dvby_FT"
        Anims(393)._Lib = "COP_DVBYZ"
        Anims(393).Name = "COP_Dvby_L"
        Anims(394)._Lib = "COP_DVBYZ"
        Anims(394).Name = "COP_Dvby_R"
        Anims(395)._Lib = "CRACK"
        Anims(395).Name = "Bbalbat_Idle_01"
        Anims(396)._Lib = "CRACK"
        Anims(396).Name = "Bbalbat_Idle_02"
        Anims(397)._Lib = "CRACK"
        Anims(397).Name = "crckdeth1"
        Anims(398)._Lib = "CRACK"
        Anims(398).Name = "crckdeth2"
        Anims(399)._Lib = "CRACK"
        Anims(399).Name = "crckdeth3"
        Anims(400)._Lib = "CRACK"
        Anims(400).Name = "crckdeth4"
        Anims(401)._Lib = "CRACK"
        Anims(401).Name = "crckidle1"
        Anims(402)._Lib = "CRACK"
        Anims(402).Name = "crckidle2"
        Anims(403)._Lib = "CRACK"
        Anims(403).Name = "crckidle3"
        Anims(404)._Lib = "CRACK"
        Anims(404).Name = "crckidle4"
        Anims(405)._Lib = "CRIB"
        Anims(405).Name = "CRIB_Console_Loop"
        Anims(406)._Lib = "CRIB"
        Anims(406).Name = "CRIB_Use_Switch"
        Anims(407)._Lib = "CRIB"
        Anims(407).Name = "PED_Console_Loop"
        Anims(408)._Lib = "CRIB"
        Anims(408).Name = "PED_Console_Loose"
        Anims(409)._Lib = "CRIB"
        Anims(409).Name = "PED_Console_Win"
        Anims(410)._Lib = "DAM_JUMP"
        Anims(410).Name = "DAM_Dive_Loop"
        Anims(411)._Lib = "DAM_JUMP"
        Anims(411).Name = "DAM_Land"
        Anims(412)._Lib = "DAM_JUMP"
        Anims(412).Name = "DAM_Launch"
        Anims(413)._Lib = "DAM_JUMP"
        Anims(413).Name = "Jump_Roll"
        Anims(414)._Lib = "DAM_JUMP"
        Anims(414).Name = "SF_JumpWall"
        Anims(415)._Lib = "DANCING"
        Anims(415).Name = "bd_clap"
        Anims(416)._Lib = "DANCING"
        Anims(416).Name = "bd_clap1"
        Anims(417)._Lib = "DANCING"
        Anims(417).Name = "dance_loop"
        Anims(418)._Lib = "DANCING"
        Anims(418).Name = "DAN_Down_A"
        Anims(419)._Lib = "DANCING"
        Anims(419).Name = "DAN_Left_A"
        Anims(420)._Lib = "DANCING"
        Anims(420).Name = "DAN_Loop_A"
        Anims(421)._Lib = "DANCING"
        Anims(421).Name = "DAN_Right_A"
        Anims(422)._Lib = "DANCING"
        Anims(422).Name = "DAN_Up_A"
        Anims(423)._Lib = "DANCING"
        Anims(423).Name = "dnce_M_a"
        Anims(424)._Lib = "DANCING"
        Anims(424).Name = "dnce_M_b"
        Anims(425)._Lib = "DANCING"
        Anims(425).Name = "dnce_M_c"
        Anims(426)._Lib = "DANCING"
        Anims(426).Name = "dnce_M_d"
        Anims(427)._Lib = "DANCING"
        Anims(427).Name = "dnce_M_e"
        Anims(428)._Lib = "DEALER"
        Anims(428).Name = "DEALER_DEAL"
        Anims(429)._Lib = "DEALER"
        Anims(429).Name = "DEALER_IDLE"
        Anims(430)._Lib = "DEALER"
        Anims(430).Name = "DEALER_IDLE_01"
        Anims(431)._Lib = "DEALER"
        Anims(431).Name = "DEALER_IDLE_02"
        Anims(432)._Lib = "DEALER"
        Anims(432).Name = "DEALER_IDLE_03"
        Anims(433)._Lib = "DEALER"
        Anims(433).Name = "DRUGS_BUY"
        Anims(434)._Lib = "DEALER"
        Anims(434).Name = "shop_pay"
        Anims(435)._Lib = "DILDO"
        Anims(435).Name = "DILDO_1"
        Anims(436)._Lib = "DILDO"
        Anims(436).Name = "DILDO_2"
        Anims(437)._Lib = "DILDO"
        Anims(437).Name = "DILDO_3"
        Anims(438)._Lib = "DILDO"
        Anims(438).Name = "DILDO_block"
        Anims(439)._Lib = "DILDO"
        Anims(439).Name = "DILDO_G"
        Anims(440)._Lib = "DILDO"
        Anims(440).Name = "DILDO_Hit_1"
        Anims(441)._Lib = "DILDO"
        Anims(441).Name = "DILDO_Hit_2"
        Anims(442)._Lib = "DILDO"
        Anims(442).Name = "DILDO_Hit_3"
        Anims(443)._Lib = "DILDO"
        Anims(443).Name = "DILDO_IDLE"
        Anims(444)._Lib = "DODGE"
        Anims(444).Name = "Cover_Dive_01"
        Anims(445)._Lib = "DODGE"
        Anims(445).Name = "Cover_Dive_02"
        Anims(446)._Lib = "DODGE"
        Anims(446).Name = "Crushed"
        Anims(447)._Lib = "DODGE"
        Anims(447).Name = "Crush_Jump"
        Anims(448)._Lib = "DOZER"
        Anims(448).Name = "DOZER_Align_LHS"
        Anims(449)._Lib = "DOZER"
        Anims(449).Name = "DOZER_Align_RHS"
        Anims(450)._Lib = "DOZER"
        Anims(450).Name = "DOZER_getin_LHS"
        Anims(451)._Lib = "DOZER"
        Anims(451).Name = "DOZER_getin_RHS"
        Anims(452)._Lib = "DOZER"
        Anims(452).Name = "DOZER_getout_LHS"
        Anims(453)._Lib = "DOZER"
        Anims(453).Name = "DOZER_getout_RHS"
        Anims(454)._Lib = "DOZER"
        Anims(454).Name = "DOZER_Jacked_LHS"
        Anims(455)._Lib = "DOZER"
        Anims(455).Name = "DOZER_Jacked_RHS"
        Anims(456)._Lib = "DOZER"
        Anims(456).Name = "DOZER_pullout_LHS"
        Anims(457)._Lib = "DOZER"
        Anims(457).Name = "DOZER_pullout_RHS"
        Anims(458)._Lib = "DRIVEBYS"
        Anims(458).Name = "Gang_DrivebyLHS"
        Anims(459)._Lib = "DRIVEBYS"
        Anims(459).Name = "Gang_DrivebyLHS_Bwd"
        Anims(460)._Lib = "DRIVEBYS"
        Anims(460).Name = "Gang_DrivebyLHS_Fwd"
        Anims(461)._Lib = "DRIVEBYS"
        Anims(461).Name = "Gang_DrivebyRHS"
        Anims(462)._Lib = "DRIVEBYS"
        Anims(462).Name = "Gang_DrivebyRHS_Bwd"
        Anims(463)._Lib = "DRIVEBYS"
        Anims(463).Name = "Gang_DrivebyRHS_Fwd"
        Anims(464)._Lib = "DRIVEBYS"
        Anims(464).Name = "Gang_DrivebyTop_LHS"
        Anims(465)._Lib = "DRIVEBYS"
        Anims(465).Name = "Gang_DrivebyTop_RHS"
        Anims(466)._Lib = "FAT"
        Anims(466).Name = "FatIdle"
        Anims(467)._Lib = "FAT"
        Anims(467).Name = "FatIdle_armed"
        Anims(468)._Lib = "FAT"
        Anims(468).Name = "FatIdle_Csaw"
        Anims(469)._Lib = "FAT"
        Anims(469).Name = "FatIdle_Rocket"
        Anims(470)._Lib = "FAT"
        Anims(470).Name = "FatRun"
        Anims(471)._Lib = "FAT"
        Anims(471).Name = "FatRun_armed"
        Anims(472)._Lib = "FAT"
        Anims(472).Name = "FatRun_Csaw"
        Anims(473)._Lib = "FAT"
        Anims(473).Name = "FatRun_Rocket"
        Anims(474)._Lib = "FAT"
        Anims(474).Name = "FatSprint"
        Anims(475)._Lib = "FAT"
        Anims(475).Name = "FatWalk"
        Anims(476)._Lib = "FAT"
        Anims(476).Name = "FatWalkstart"
        Anims(477)._Lib = "FAT"
        Anims(477).Name = "FatWalkstart_Csaw"
        Anims(478)._Lib = "FAT"
        Anims(478).Name = "FatWalkSt_armed"
        Anims(479)._Lib = "FAT"
        Anims(479).Name = "FatWalkSt_Rocket"
        Anims(480)._Lib = "FAT"
        Anims(480).Name = "FatWalk_armed"
        Anims(481)._Lib = "FAT"
        Anims(481).Name = "FatWalk_Csaw"
        Anims(482)._Lib = "FAT"
        Anims(482).Name = "FatWalk_Rocket"
        Anims(483)._Lib = "FAT"
        Anims(483).Name = "IDLE_tired"
        Anims(484)._Lib = "FIGHT_B"
        Anims(484).Name = "FightB_1"
        Anims(485)._Lib = "FIGHT_B"
        Anims(485).Name = "FightB_2"
        Anims(486)._Lib = "FIGHT_B"
        Anims(486).Name = "FightB_3"
        Anims(487)._Lib = "FIGHT_B"
        Anims(487).Name = "FightB_block"
        Anims(488)._Lib = "FIGHT_B"
        Anims(488).Name = "FightB_G"
        Anims(489)._Lib = "FIGHT_B"
        Anims(489).Name = "FightB_IDLE"
        Anims(490)._Lib = "FIGHT_B"
        Anims(490).Name = "FightB_M"
        Anims(491)._Lib = "FIGHT_B"
        Anims(491).Name = "HitB_1"
        Anims(492)._Lib = "FIGHT_B"
        Anims(492).Name = "HitB_2"
        Anims(493)._Lib = "FIGHT_B"
        Anims(493).Name = "HitB_3"
        Anims(494)._Lib = "FIGHT_C"
        Anims(494).Name = "FightC_1"
        Anims(495)._Lib = "FIGHT_C"
        Anims(495).Name = "FightC_2"
        Anims(496)._Lib = "FIGHT_C"
        Anims(496).Name = "FightC_3"
        Anims(497)._Lib = "FIGHT_C"
        Anims(497).Name = "FightC_block"
        Anims(498)._Lib = "FIGHT_C"
        Anims(498).Name = "FightC_blocking"
        Anims(499)._Lib = "FIGHT_C"
        Anims(499).Name = "FightC_G"
        Anims(500)._Lib = "FIGHT_C"
        Anims(500).Name = "FightC_IDLE"
        Anims(501)._Lib = "FIGHT_C"
        Anims(501).Name = "FightC_M"
        Anims(502)._Lib = "FIGHT_C"
        Anims(502).Name = "FightC_Spar"
        Anims(503)._Lib = "FIGHT_C"
        Anims(503).Name = "HitC_1"
        Anims(504)._Lib = "FIGHT_C"
        Anims(504).Name = "HitC_2"
        Anims(505)._Lib = "FIGHT_C"
        Anims(505).Name = "HitC_3"
        Anims(506)._Lib = "FIGHT_D"
        Anims(506).Name = "FightD_1"
        Anims(507)._Lib = "FIGHT_D"
        Anims(507).Name = "FightD_2"
        Anims(508)._Lib = "FIGHT_D"
        Anims(508).Name = "FightD_3"
        Anims(509)._Lib = "FIGHT_D"
        Anims(509).Name = "FightD_block"
        Anims(510)._Lib = "FIGHT_D"
        Anims(510).Name = "FightD_G"
        Anims(511)._Lib = "FIGHT_D"
        Anims(511).Name = "FightD_IDLE"
        Anims(512)._Lib = "FIGHT_D"
        Anims(512).Name = "FightD_M"
        Anims(513)._Lib = "FIGHT_D"
        Anims(513).Name = "HitD_1"
        Anims(514)._Lib = "FIGHT_D"
        Anims(514).Name = "HitD_2"
        Anims(515)._Lib = "FIGHT_D"
        Anims(515).Name = "HitD_3"
        Anims(516)._Lib = "FIGHT_E"
        Anims(516).Name = "FightKick"
        Anims(517)._Lib = "FIGHT_E"
        Anims(517).Name = "FightKick_B"
        Anims(518)._Lib = "FIGHT_E"
        Anims(518).Name = "Hit_fightkick"
        Anims(519)._Lib = "FIGHT_E"
        Anims(519).Name = "Hit_fightkick_B"
        Anims(520)._Lib = "FINALE"
        Anims(520).Name = "FIN_Climb_In"
        Anims(521)._Lib = "FINALE"
        Anims(521).Name = "FIN_Cop1_ClimbOut2"
        Anims(522)._Lib = "FINALE"
        Anims(522).Name = "FIN_Cop1_Loop"
        Anims(523)._Lib = "FINALE"
        Anims(523).Name = "FIN_Cop1_Stomp"
        Anims(524)._Lib = "FINALE"
        Anims(524).Name = "FIN_Hang_L"
        Anims(525)._Lib = "FINALE"
        Anims(525).Name = "FIN_Hang_Loop"
        Anims(526)._Lib = "FINALE"
        Anims(526).Name = "FIN_Hang_R"
        Anims(527)._Lib = "FINALE"
        Anims(527).Name = "FIN_Hang_Slip"
        Anims(528)._Lib = "FINALE"
        Anims(528).Name = "FIN_Jump_On"
        Anims(529)._Lib = "FINALE"
        Anims(529).Name = "FIN_Land_Car"
        Anims(530)._Lib = "FINALE"
        Anims(530).Name = "FIN_Land_Die"
        Anims(531)._Lib = "FINALE"
        Anims(531).Name = "FIN_LegsUp"
        Anims(532)._Lib = "FINALE"
        Anims(532).Name = "FIN_LegsUp_L"
        Anims(533)._Lib = "FINALE"
        Anims(533).Name = "FIN_LegsUp_Loop"
        Anims(534)._Lib = "FINALE"
        Anims(534).Name = "FIN_LegsUp_R"
        Anims(535)._Lib = "FINALE"
        Anims(535).Name = "FIN_Let_Go"
        Anims(536)._Lib = "FINALE2"
        Anims(536).Name = "FIN_Cop1_ClimbOut"
        Anims(537)._Lib = "FINALE2"
        Anims(537).Name = "FIN_Cop1_Fall"
        Anims(538)._Lib = "FINALE2"
        Anims(538).Name = "FIN_Cop1_Loop"
        Anims(539)._Lib = "FINALE2"
        Anims(539).Name = "FIN_Cop1_Shot"
        Anims(540)._Lib = "FINALE2"
        Anims(540).Name = "FIN_Cop1_Swing"
        Anims(541)._Lib = "FINALE2"
        Anims(541).Name = "FIN_Cop2_ClimbOut"
        Anims(542)._Lib = "FINALE2"
        Anims(542).Name = "FIN_Switch_P"
        Anims(543)._Lib = "FINALE2"
        Anims(543).Name = "FIN_Switch_S"
        Anims(544)._Lib = "FLAME"
        Anims(544).Name = "FLAME_fire"
        Anims(545)._Lib = "Flowers"
        Anims(545).Name = "Flower_attack"
        Anims(546)._Lib = "Flowers"
        Anims(546).Name = "Flower_attack_M"
        Anims(547)._Lib = "Flowers"
        Anims(547).Name = "Flower_Hit"
        Anims(548)._Lib = "FOOD"
        Anims(548).Name = "EAT_Burger"
        Anims(549)._Lib = "FOOD"
        Anims(549).Name = "EAT_Chicken"
        Anims(550)._Lib = "FOOD"
        Anims(550).Name = "EAT_Pizza"
        Anims(551)._Lib = "FOOD"
        Anims(551).Name = "EAT_Vomit_P"
        Anims(552)._Lib = "FOOD"
        Anims(552).Name = "EAT_Vomit_SK"
        Anims(553)._Lib = "FOOD"
        Anims(553).Name = "FF_Dam_Bkw"
        Anims(554)._Lib = "FOOD"
        Anims(554).Name = "FF_Dam_Fwd"
        Anims(555)._Lib = "FOOD"
        Anims(555).Name = "FF_Dam_Left"
        Anims(556)._Lib = "FOOD"
        Anims(556).Name = "FF_Dam_Right"
        Anims(557)._Lib = "FOOD"
        Anims(557).Name = "FF_Die_Bkw"
        Anims(558)._Lib = "FOOD"
        Anims(558).Name = "FF_Die_Fwd"
        Anims(559)._Lib = "FOOD"
        Anims(559).Name = "FF_Die_Left"
        Anims(560)._Lib = "FOOD"
        Anims(560).Name = "FF_Die_Right"
        Anims(561)._Lib = "FOOD"
        Anims(561).Name = "FF_Sit_Eat1"
        Anims(562)._Lib = "FOOD"
        Anims(562).Name = "FF_Sit_Eat2"
        Anims(563)._Lib = "FOOD"
        Anims(563).Name = "FF_Sit_Eat3"
        Anims(564)._Lib = "FOOD"
        Anims(564).Name = "FF_Sit_In"
        Anims(565)._Lib = "FOOD"
        Anims(565).Name = "FF_Sit_In_L"
        Anims(566)._Lib = "FOOD"
        Anims(566).Name = "FF_Sit_In_R"
        Anims(567)._Lib = "FOOD"
        Anims(567).Name = "FF_Sit_Look"
        Anims(568)._Lib = "FOOD"
        Anims(568).Name = "FF_Sit_Loop"
        Anims(569)._Lib = "FOOD"
        Anims(569).Name = "FF_Sit_Out_180"
        Anims(570)._Lib = "FOOD"
        Anims(570).Name = "FF_Sit_Out_L_180"
        Anims(571)._Lib = "FOOD"
        Anims(571).Name = "FF_Sit_Out_R_180"
        Anims(572)._Lib = "FOOD"
        Anims(572).Name = "SHP_Thank"
        Anims(573)._Lib = "FOOD"
        Anims(573).Name = "SHP_Tray_In"
        Anims(574)._Lib = "FOOD"
        Anims(574).Name = "SHP_Tray_Lift"
        Anims(575)._Lib = "FOOD"
        Anims(575).Name = "SHP_Tray_Lift_In"
        Anims(576)._Lib = "FOOD"
        Anims(576).Name = "SHP_Tray_Lift_Loop"
        Anims(577)._Lib = "FOOD"
        Anims(577).Name = "SHP_Tray_Lift_Out"
        Anims(578)._Lib = "FOOD"
        Anims(578).Name = "SHP_Tray_Out"
        Anims(579)._Lib = "FOOD"
        Anims(579).Name = "SHP_Tray_Pose"
        Anims(580)._Lib = "FOOD"
        Anims(580).Name = "SHP_Tray_Return"
        Anims(581)._Lib = "Freeweights"
        Anims(581).Name = "gym_barbell"
        Anims(582)._Lib = "Freeweights"
        Anims(582).Name = "gym_free_A"
        Anims(583)._Lib = "Freeweights"
        Anims(583).Name = "gym_free_B"
        Anims(584)._Lib = "Freeweights"
        Anims(584).Name = "gym_free_celebrate"
        Anims(585)._Lib = "Freeweights"
        Anims(585).Name = "gym_free_down"
        Anims(586)._Lib = "Freeweights"
        Anims(586).Name = "gym_free_loop"
        Anims(587)._Lib = "Freeweights"
        Anims(587).Name = "gym_free_pickup"
        Anims(588)._Lib = "Freeweights"
        Anims(588).Name = "gym_free_putdown"
        Anims(589)._Lib = "Freeweights"
        Anims(589).Name = "gym_free_up_smooth"
        Anims(590)._Lib = "GANGS"
        Anims(590).Name = "DEALER_DEAL"
        Anims(591)._Lib = "GANGS"
        Anims(591).Name = "DEALER_IDLE"
        Anims(592)._Lib = "GANGS"
        Anims(592).Name = "drnkbr_prtl"
        Anims(593)._Lib = "GANGS"
        Anims(593).Name = "drnkbr_prtl_F"
        Anims(594)._Lib = "GANGS"
        Anims(594).Name = "DRUGS_BUY"
        Anims(595)._Lib = "GANGS"
        Anims(595).Name = "hndshkaa"
        Anims(596)._Lib = "GANGS"
        Anims(596).Name = "hndshkba"
        Anims(597)._Lib = "GANGS"
        Anims(597).Name = "hndshkca"
        Anims(598)._Lib = "GANGS"
        Anims(598).Name = "hndshkcb"
        Anims(599)._Lib = "GANGS"
        Anims(599).Name = "hndshkda"
        Anims(600)._Lib = "GANGS"
        Anims(600).Name = "hndshkea"
        Anims(601)._Lib = "GANGS"
        Anims(601).Name = "hndshkfa"
        Anims(602)._Lib = "GANGS"
        Anims(602).Name = "hndshkfa_swt"
        Anims(603)._Lib = "GANGS"
        Anims(603).Name = "Invite_No"
        Anims(604)._Lib = "GANGS"
        Anims(604).Name = "Invite_Yes"
        Anims(605)._Lib = "GANGS"
        Anims(605).Name = "leanIDLE"
        Anims(606)._Lib = "GANGS"
        Anims(606).Name = "leanIN"
        Anims(607)._Lib = "GANGS"
        Anims(607).Name = "leanOUT"
        Anims(608)._Lib = "GANGS"
        Anims(608).Name = "prtial_gngtlkA"
        Anims(609)._Lib = "GANGS"
        Anims(609).Name = "prtial_gngtlkB"
        Anims(610)._Lib = "GANGS"
        Anims(610).Name = "prtial_gngtlkC"
        Anims(611)._Lib = "GANGS"
        Anims(611).Name = "prtial_gngtlkD"
        Anims(612)._Lib = "GANGS"
        Anims(612).Name = "prtial_gngtlkE"
        Anims(613)._Lib = "GANGS"
        Anims(613).Name = "prtial_gngtlkF"
        Anims(614)._Lib = "GANGS"
        Anims(614).Name = "prtial_gngtlkG"
        Anims(615)._Lib = "GANGS"
        Anims(615).Name = "prtial_gngtlkH"
        Anims(616)._Lib = "GANGS"
        Anims(616).Name = "prtial_hndshk_01"
        Anims(617)._Lib = "GANGS"
        Anims(617).Name = "prtial_hndshk_biz_01"
        Anims(618)._Lib = "GANGS"
        Anims(618).Name = "shake_cara"
        Anims(619)._Lib = "GANGS"
        Anims(619).Name = "shake_carK"
        Anims(620)._Lib = "GANGS"
        Anims(620).Name = "shake_carSH"
        Anims(621)._Lib = "GANGS"
        Anims(621).Name = "smkcig_prtl"
        Anims(622)._Lib = "GANGS"
        Anims(622).Name = "smkcig_prtl_F"
        Anims(623)._Lib = "GHANDS"
        Anims(623).Name = "gsign1"
        Anims(624)._Lib = "GHANDS"
        Anims(624).Name = "gsign1LH"
        Anims(625)._Lib = "GHANDS"
        Anims(625).Name = "gsign2"
        Anims(626)._Lib = "GHANDS"
        Anims(626).Name = "gsign2LH"
        Anims(627)._Lib = "GHANDS"
        Anims(627).Name = "gsign3"
        Anims(628)._Lib = "GHANDS"
        Anims(628).Name = "gsign3LH"
        Anims(629)._Lib = "GHANDS"
        Anims(629).Name = "gsign4"
        Anims(630)._Lib = "GHANDS"
        Anims(630).Name = "gsign4LH"
        Anims(631)._Lib = "GHANDS"
        Anims(631).Name = "gsign5"
        Anims(632)._Lib = "GHANDS"
        Anims(632).Name = "gsign5LH"
        Anims(633)._Lib = "GHANDS"
        Anims(633).Name = "LHGsign1"
        Anims(634)._Lib = "GHANDS"
        Anims(634).Name = "LHGsign2"
        Anims(635)._Lib = "GHANDS"
        Anims(635).Name = "LHGsign3"
        Anims(636)._Lib = "GHANDS"
        Anims(636).Name = "LHGsign4"
        Anims(637)._Lib = "GHANDS"
        Anims(637).Name = "LHGsign5"
        Anims(638)._Lib = "GHANDS"
        Anims(638).Name = "RHGsign1"
        Anims(639)._Lib = "GHANDS"
        Anims(639).Name = "RHGsign2"
        Anims(640)._Lib = "GHANDS"
        Anims(640).Name = "RHGsign3"
        Anims(641)._Lib = "GHANDS"
        Anims(641).Name = "RHGsign4"
        Anims(642)._Lib = "GHANDS"
        Anims(642).Name = "RHGsign5"
        Anims(643)._Lib = "GHETTO_DB"
        Anims(643).Name = "GDB_Car2_PLY"
        Anims(644)._Lib = "GHETTO_DB"
        Anims(644).Name = "GDB_Car2_SMO"
        Anims(645)._Lib = "GHETTO_DB"
        Anims(645).Name = "GDB_Car2_SWE"
        Anims(646)._Lib = "GHETTO_DB"
        Anims(646).Name = "GDB_Car_PLY"
        Anims(647)._Lib = "GHETTO_DB"
        Anims(647).Name = "GDB_Car_RYD"
        Anims(648)._Lib = "GHETTO_DB"
        Anims(648).Name = "GDB_Car_SMO"
        Anims(649)._Lib = "GHETTO_DB"
        Anims(649).Name = "GDB_Car_SWE"
        Anims(650)._Lib = "goggles"
        Anims(650).Name = "goggles_put_on"
        Anims(651)._Lib = "GRAFFITI"
        Anims(651).Name = "graffiti_Chkout"
        Anims(652)._Lib = "GRAFFITI"
        Anims(652).Name = "spraycan_fire"
        Anims(653)._Lib = "GRAVEYARD"
        Anims(653).Name = "mrnF_loop"
        Anims(654)._Lib = "GRAVEYARD"
        Anims(654).Name = "mrnM_loop"
        Anims(655)._Lib = "GRAVEYARD"
        Anims(655).Name = "prst_loopa"
        Anims(656)._Lib = "GRENADE"
        Anims(656).Name = "WEAPON_start_throw"
        Anims(657)._Lib = "GRENADE"
        Anims(657).Name = "WEAPON_throw"
        Anims(658)._Lib = "GRENADE"
        Anims(658).Name = "WEAPON_throwu"
        Anims(659)._Lib = "GYMNASIUM"
        Anims(659).Name = "GYMshadowbox"
        Anims(660)._Lib = "GYMNASIUM"
        Anims(660).Name = "gym_bike_celebrate"
        Anims(661)._Lib = "GYMNASIUM"
        Anims(661).Name = "gym_bike_fast"
        Anims(662)._Lib = "GYMNASIUM"
        Anims(662).Name = "gym_bike_faster"
        Anims(663)._Lib = "GYMNASIUM"
        Anims(663).Name = "gym_bike_getoff"
        Anims(664)._Lib = "GYMNASIUM"
        Anims(664).Name = "gym_bike_geton"
        Anims(665)._Lib = "GYMNASIUM"
        Anims(665).Name = "gym_bike_pedal"
        Anims(666)._Lib = "GYMNASIUM"
        Anims(666).Name = "gym_bike_slow"
        Anims(667)._Lib = "GYMNASIUM"
        Anims(667).Name = "gym_bike_still"
        Anims(668)._Lib = "GYMNASIUM"
        Anims(668).Name = "gym_jog_falloff"
        Anims(669)._Lib = "GYMNASIUM"
        Anims(669).Name = "gym_shadowbox"
        Anims(670)._Lib = "GYMNASIUM"
        Anims(670).Name = "gym_tread_celebrate"
        Anims(671)._Lib = "GYMNASIUM"
        Anims(671).Name = "gym_tread_falloff"
        Anims(672)._Lib = "GYMNASIUM"
        Anims(672).Name = "gym_tread_getoff"
        Anims(673)._Lib = "GYMNASIUM"
        Anims(673).Name = "gym_tread_geton"
        Anims(674)._Lib = "GYMNASIUM"
        Anims(674).Name = "gym_tread_jog"
        Anims(675)._Lib = "GYMNASIUM"
        Anims(675).Name = "gym_tread_sprint"
        Anims(676)._Lib = "GYMNASIUM"
        Anims(676).Name = "gym_tread_tired"
        Anims(677)._Lib = "GYMNASIUM"
        Anims(677).Name = "gym_tread_walk"
        Anims(678)._Lib = "GYMNASIUM"
        Anims(678).Name = "gym_walk_falloff"
        Anims(679)._Lib = "GYMNASIUM"
        Anims(679).Name = "Pedals_fast"
        Anims(680)._Lib = "GYMNASIUM"
        Anims(680).Name = "Pedals_med"
        Anims(681)._Lib = "GYMNASIUM"
        Anims(681).Name = "Pedals_slow"
        Anims(682)._Lib = "GYMNASIUM"
        Anims(682).Name = "Pedals_still"
        Anims(683)._Lib = "HAIRCUTS"
        Anims(683).Name = "BRB_Beard_01"
        Anims(684)._Lib = "HAIRCUTS"
        Anims(684).Name = "BRB_Buy"
        Anims(685)._Lib = "HAIRCUTS"
        Anims(685).Name = "BRB_Cut"
        Anims(686)._Lib = "HAIRCUTS"
        Anims(686).Name = "BRB_Cut_In"
        Anims(687)._Lib = "HAIRCUTS"
        Anims(687).Name = "BRB_Cut_Out"
        Anims(688)._Lib = "HAIRCUTS"
        Anims(688).Name = "BRB_Hair_01"
        Anims(689)._Lib = "HAIRCUTS"
        Anims(689).Name = "BRB_Hair_02"
        Anims(690)._Lib = "HAIRCUTS"
        Anims(690).Name = "BRB_In"
        Anims(691)._Lib = "HAIRCUTS"
        Anims(691).Name = "BRB_Loop"
        Anims(692)._Lib = "HAIRCUTS"
        Anims(692).Name = "BRB_Out"
        Anims(693)._Lib = "HAIRCUTS"
        Anims(693).Name = "BRB_Sit_In"
        Anims(694)._Lib = "HAIRCUTS"
        Anims(694).Name = "BRB_Sit_Loop"
        Anims(695)._Lib = "HAIRCUTS"
        Anims(695).Name = "BRB_Sit_Out"
        Anims(696)._Lib = "HEIST9"
        Anims(696).Name = "CAS_G2_GasKO"
        Anims(697)._Lib = "HEIST9"
        Anims(697).Name = "swt_wllpk_L"
        Anims(698)._Lib = "HEIST9"
        Anims(698).Name = "swt_wllpk_L_back"
        Anims(699)._Lib = "HEIST9"
        Anims(699).Name = "swt_wllpk_R"
        Anims(700)._Lib = "HEIST9"
        Anims(700).Name = "swt_wllpk_R_back"
        Anims(701)._Lib = "HEIST9"
        Anims(701).Name = "swt_wllshoot_in_L"
        Anims(702)._Lib = "HEIST9"
        Anims(702).Name = "swt_wllshoot_in_R"
        Anims(703)._Lib = "HEIST9"
        Anims(703).Name = "swt_wllshoot_out_L"
        Anims(704)._Lib = "HEIST9"
        Anims(704).Name = "swt_wllshoot_out_R"
        Anims(705)._Lib = "HEIST9"
        Anims(705).Name = "Use_SwipeCard"
        Anims(706)._Lib = "INT_HOUSE"
        Anims(706).Name = "BED_In_L"
        Anims(707)._Lib = "INT_HOUSE"
        Anims(707).Name = "BED_In_R"
        Anims(708)._Lib = "INT_HOUSE"
        Anims(708).Name = "BED_Loop_L"
        Anims(709)._Lib = "INT_HOUSE"
        Anims(709).Name = "BED_Loop_R"
        Anims(710)._Lib = "INT_HOUSE"
        Anims(710).Name = "BED_Out_L"
        Anims(711)._Lib = "INT_HOUSE"
        Anims(711).Name = "BED_Out_R"
        Anims(712)._Lib = "INT_HOUSE"
        Anims(712).Name = "LOU_In"
        Anims(713)._Lib = "INT_HOUSE"
        Anims(713).Name = "LOU_Loop"
        Anims(714)._Lib = "INT_HOUSE"
        Anims(714).Name = "LOU_Out"
        Anims(715)._Lib = "INT_HOUSE"
        Anims(715).Name = "wash_up"
        Anims(716)._Lib = "INT_OFFICE"
        Anims(716).Name = "FF_Dam_Fwd"
        Anims(717)._Lib = "INT_OFFICE"
        Anims(717).Name = "OFF_Sit_2Idle_180"
        Anims(718)._Lib = "INT_OFFICE"
        Anims(718).Name = "OFF_Sit_Bored_Loop"
        Anims(719)._Lib = "INT_OFFICE"
        Anims(719).Name = "OFF_Sit_Crash"
        Anims(720)._Lib = "INT_OFFICE"
        Anims(720).Name = "OFF_Sit_Drink"
        Anims(721)._Lib = "INT_OFFICE"
        Anims(721).Name = "OFF_Sit_Idle_Loop"
        Anims(722)._Lib = "INT_OFFICE"
        Anims(722).Name = "OFF_Sit_In"
        Anims(723)._Lib = "INT_OFFICE"
        Anims(723).Name = "OFF_Sit_Read"
        Anims(724)._Lib = "INT_OFFICE"
        Anims(724).Name = "OFF_Sit_Type_Loop"
        Anims(725)._Lib = "INT_OFFICE"
        Anims(725).Name = "OFF_Sit_Watch"
        Anims(726)._Lib = "INT_SHOP"
        Anims(726).Name = "shop_cashier"
        Anims(727)._Lib = "INT_SHOP"
        Anims(727).Name = "shop_in"
        Anims(728)._Lib = "INT_SHOP"
        Anims(728).Name = "shop_lookA"
        Anims(729)._Lib = "INT_SHOP"
        Anims(729).Name = "shop_lookB"
        Anims(730)._Lib = "INT_SHOP"
        Anims(730).Name = "shop_loop"
        Anims(731)._Lib = "INT_SHOP"
        Anims(731).Name = "shop_out"
        Anims(732)._Lib = "INT_SHOP"
        Anims(732).Name = "shop_pay"
        Anims(733)._Lib = "INT_SHOP"
        Anims(733).Name = "shop_shelf"
        Anims(734)._Lib = "JST_BUISNESS"
        Anims(734).Name = "girl_01"
        Anims(735)._Lib = "JST_BUISNESS"
        Anims(735).Name = "girl_02"
        Anims(736)._Lib = "JST_BUISNESS"
        Anims(736).Name = "player_01"
        Anims(737)._Lib = "JST_BUISNESS"
        Anims(737).Name = "smoke_01"
        Anims(738)._Lib = "KART"
        Anims(738).Name = "KART_getin_LHS"
        Anims(739)._Lib = "KART"
        Anims(739).Name = "KART_getin_RHS"
        Anims(740)._Lib = "KART"
        Anims(740).Name = "KART_getout_LHS"
        Anims(741)._Lib = "KART"
        Anims(741).Name = "KART_getout_RHS"
        Anims(742)._Lib = "KISSING"
        Anims(742).Name = "BD_GF_Wave"
        Anims(743)._Lib = "KISSING"
        Anims(743).Name = "gfwave2"
        Anims(744)._Lib = "KISSING"
        Anims(744).Name = "GF_CarArgue_01"
        Anims(745)._Lib = "KISSING"
        Anims(745).Name = "GF_CarArgue_02"
        Anims(746)._Lib = "KISSING"
        Anims(746).Name = "GF_CarSpot"
        Anims(747)._Lib = "KISSING"
        Anims(747).Name = "GF_StreetArgue_01"
        Anims(748)._Lib = "KISSING"
        Anims(748).Name = "GF_StreetArgue_02"
        Anims(749)._Lib = "KISSING"
        Anims(749).Name = "gift_get"
        Anims(750)._Lib = "KISSING"
        Anims(750).Name = "gift_give"
        Anims(751)._Lib = "KISSING"
        Anims(751).Name = "Grlfrd_Kiss_01"
        Anims(752)._Lib = "KISSING"
        Anims(752).Name = "Grlfrd_Kiss_02"
        Anims(753)._Lib = "KISSING"
        Anims(753).Name = "Grlfrd_Kiss_03"
        Anims(754)._Lib = "KISSING"
        Anims(754).Name = "Playa_Kiss_01"
        Anims(755)._Lib = "KISSING"
        Anims(755).Name = "Playa_Kiss_02"
        Anims(756)._Lib = "KISSING"
        Anims(756).Name = "Playa_Kiss_03"
        Anims(757)._Lib = "KNIFE"
        Anims(757).Name = "KILL_Knife_Ped_Damage"
        Anims(758)._Lib = "KNIFE"
        Anims(758).Name = "KILL_Knife_Ped_Die"
        Anims(759)._Lib = "KNIFE"
        Anims(759).Name = "KILL_Knife_Player"
        Anims(760)._Lib = "KNIFE"
        Anims(760).Name = "KILL_Partial"
        Anims(761)._Lib = "KNIFE"
        Anims(761).Name = "knife_1"
        Anims(762)._Lib = "KNIFE"
        Anims(762).Name = "knife_2"
        Anims(763)._Lib = "KNIFE"
        Anims(763).Name = "knife_3"
        Anims(764)._Lib = "KNIFE"
        Anims(764).Name = "Knife_4"
        Anims(765)._Lib = "KNIFE"
        Anims(765).Name = "knife_block"
        Anims(766)._Lib = "KNIFE"
        Anims(766).Name = "Knife_G"
        Anims(767)._Lib = "KNIFE"
        Anims(767).Name = "knife_hit_1"
        Anims(768)._Lib = "KNIFE"
        Anims(768).Name = "knife_hit_2"
        Anims(769)._Lib = "KNIFE"
        Anims(769).Name = "knife_hit_3"
        Anims(770)._Lib = "KNIFE"
        Anims(770).Name = "knife_IDLE"
        Anims(771)._Lib = "KNIFE"
        Anims(771).Name = "knife_part"
        Anims(772)._Lib = "KNIFE"
        Anims(772).Name = "WEAPON_knifeidle"
        Anims(773)._Lib = "LAPDAN1"
        Anims(773).Name = "LAPDAN_D"
        Anims(774)._Lib = "LAPDAN1"
        Anims(774).Name = "LAPDAN_P"
        Anims(775)._Lib = "LAPDAN2"
        Anims(775).Name = "LAPDAN_D"
        Anims(776)._Lib = "LAPDAN2"
        Anims(776).Name = "LAPDAN_P"
        Anims(777)._Lib = "LAPDAN3"
        Anims(777).Name = "LAPDAN_D"
        Anims(778)._Lib = "LAPDAN3"
        Anims(778).Name = "LAPDAN_P"
        Anims(779)._Lib = "LOWRIDER"
        Anims(779).Name = "F_smklean_loop"
        Anims(780)._Lib = "LOWRIDER"
        Anims(780).Name = "lrgirl_bdbnce"
        Anims(781)._Lib = "LOWRIDER"
        Anims(781).Name = "lrgirl_hair"
        Anims(782)._Lib = "LOWRIDER"
        Anims(782).Name = "lrgirl_hurry"
        Anims(783)._Lib = "LOWRIDER"
        Anims(783).Name = "lrgirl_idleloop"
        Anims(784)._Lib = "LOWRIDER"
        Anims(784).Name = "lrgirl_idle_to_l0"
        Anims(785)._Lib = "LOWRIDER"
        Anims(785).Name = "lrgirl_l0_bnce"
        Anims(786)._Lib = "LOWRIDER"
        Anims(786).Name = "lrgirl_l0_loop"
        Anims(787)._Lib = "LOWRIDER"
        Anims(787).Name = "lrgirl_l0_to_l1"
        Anims(788)._Lib = "LOWRIDER"
        Anims(788).Name = "lrgirl_l12_to_l0"
        Anims(789)._Lib = "LOWRIDER"
        Anims(789).Name = "lrgirl_l1_bnce"
        Anims(790)._Lib = "LOWRIDER"
        Anims(790).Name = "lrgirl_l1_loop"
        Anims(791)._Lib = "LOWRIDER"
        Anims(791).Name = "lrgirl_l1_to_l2"
        Anims(792)._Lib = "LOWRIDER"
        Anims(792).Name = "lrgirl_l2_bnce"
        Anims(793)._Lib = "LOWRIDER"
        Anims(793).Name = "lrgirl_l2_loop"
        Anims(794)._Lib = "LOWRIDER"
        Anims(794).Name = "lrgirl_l2_to_l3"
        Anims(795)._Lib = "LOWRIDER"
        Anims(795).Name = "lrgirl_l345_to_l1"
        Anims(796)._Lib = "LOWRIDER"
        Anims(796).Name = "lrgirl_l3_bnce"
        Anims(797)._Lib = "LOWRIDER"
        Anims(797).Name = "lrgirl_l3_loop"
        Anims(798)._Lib = "LOWRIDER"
        Anims(798).Name = "lrgirl_l3_to_l4"
        Anims(799)._Lib = "LOWRIDER"
        Anims(799).Name = "lrgirl_l4_bnce"
        Anims(800)._Lib = "LOWRIDER"
        Anims(800).Name = "lrgirl_l4_loop"
        Anims(801)._Lib = "LOWRIDER"
        Anims(801).Name = "lrgirl_l4_to_l5"
        Anims(802)._Lib = "LOWRIDER"
        Anims(802).Name = "lrgirl_l5_bnce"
        Anims(803)._Lib = "LOWRIDER"
        Anims(803).Name = "lrgirl_l5_loop"
        Anims(804)._Lib = "LOWRIDER"
        Anims(804).Name = "M_smklean_loop"
        Anims(805)._Lib = "LOWRIDER"
        Anims(805).Name = "M_smkstnd_loop"
        Anims(806)._Lib = "LOWRIDER"
        Anims(806).Name = "prtial_gngtlkB"
        Anims(807)._Lib = "LOWRIDER"
        Anims(807).Name = "prtial_gngtlkC"
        Anims(808)._Lib = "LOWRIDER"
        Anims(808).Name = "prtial_gngtlkD"
        Anims(809)._Lib = "LOWRIDER"
        Anims(809).Name = "prtial_gngtlkE"
        Anims(810)._Lib = "LOWRIDER"
        Anims(810).Name = "prtial_gngtlkF"
        Anims(811)._Lib = "LOWRIDER"
        Anims(811).Name = "prtial_gngtlkG"
        Anims(812)._Lib = "LOWRIDER"
        Anims(812).Name = "prtial_gngtlkH"
        Anims(813)._Lib = "LOWRIDER"
        Anims(813).Name = "RAP_A_Loop"
        Anims(814)._Lib = "LOWRIDER"
        Anims(814).Name = "RAP_B_Loop"
        Anims(815)._Lib = "LOWRIDER"
        Anims(815).Name = "RAP_C_Loop"
        Anims(816)._Lib = "LOWRIDER"
        Anims(816).Name = "Sit_relaxed"
        Anims(817)._Lib = "LOWRIDER"
        Anims(817).Name = "Tap_hand"
        Anims(818)._Lib = "MD_CHASE"
        Anims(818).Name = "Carhit_Hangon"
        Anims(819)._Lib = "MD_CHASE"
        Anims(819).Name = "Carhit_Tumble"
        Anims(820)._Lib = "MD_CHASE"
        Anims(820).Name = "donutdrop"
        Anims(821)._Lib = "MD_CHASE"
        Anims(821).Name = "Fen_Choppa_L1"
        Anims(822)._Lib = "MD_CHASE"
        Anims(822).Name = "Fen_Choppa_L2"
        Anims(823)._Lib = "MD_CHASE"
        Anims(823).Name = "Fen_Choppa_L3"
        Anims(824)._Lib = "MD_CHASE"
        Anims(824).Name = "Fen_Choppa_R1"
        Anims(825)._Lib = "MD_CHASE"
        Anims(825).Name = "Fen_Choppa_R2"
        Anims(826)._Lib = "MD_CHASE"
        Anims(826).Name = "Fen_Choppa_R3"
        Anims(827)._Lib = "MD_CHASE"
        Anims(827).Name = "Hangon_Stun_loop"
        Anims(828)._Lib = "MD_CHASE"
        Anims(828).Name = "Hangon_Stun_Turn"
        Anims(829)._Lib = "MD_CHASE"
        Anims(829).Name = "MD_BIKE_2_HANG"
        Anims(830)._Lib = "MD_CHASE"
        Anims(830).Name = "MD_BIKE_Jmp_BL"
        Anims(831)._Lib = "MD_CHASE"
        Anims(831).Name = "MD_BIKE_Jmp_F"
        Anims(832)._Lib = "MD_CHASE"
        Anims(832).Name = "MD_BIKE_Lnd_BL"
        Anims(833)._Lib = "MD_CHASE"
        Anims(833).Name = "MD_BIKE_Lnd_Die_BL"
        Anims(834)._Lib = "MD_CHASE"
        Anims(834).Name = "MD_BIKE_Lnd_Die_F"
        Anims(835)._Lib = "MD_CHASE"
        Anims(835).Name = "MD_BIKE_Lnd_F"
        Anims(836)._Lib = "MD_CHASE"
        Anims(836).Name = "MD_BIKE_Lnd_Roll"
        Anims(837)._Lib = "MD_CHASE"
        Anims(837).Name = "MD_BIKE_Lnd_Roll_F"
        Anims(838)._Lib = "MD_CHASE"
        Anims(838).Name = "MD_BIKE_Punch"
        Anims(839)._Lib = "MD_CHASE"
        Anims(839).Name = "MD_BIKE_Punch_F"
        Anims(840)._Lib = "MD_CHASE"
        Anims(840).Name = "MD_BIKE_Shot_F"
        Anims(841)._Lib = "MD_CHASE"
        Anims(841).Name = "MD_HANG_Lnd_Roll"
        Anims(842)._Lib = "MD_CHASE"
        Anims(842).Name = "MD_HANG_Loop"
        Anims(843)._Lib = "MD_END"
        Anims(843).Name = "END_SC1_PLY"
        Anims(844)._Lib = "MD_END"
        Anims(844).Name = "END_SC1_RYD"
        Anims(845)._Lib = "MD_END"
        Anims(845).Name = "END_SC1_SMO"
        Anims(846)._Lib = "MD_END"
        Anims(846).Name = "END_SC1_SWE"
        Anims(847)._Lib = "MD_END"
        Anims(847).Name = "END_SC2_PLY"
        Anims(848)._Lib = "MD_END"
        Anims(848).Name = "END_SC2_RYD"
        Anims(849)._Lib = "MD_END"
        Anims(849).Name = "END_SC2_SMO"
        Anims(850)._Lib = "MD_END"
        Anims(850).Name = "END_SC2_SWE"
        Anims(851)._Lib = "MEDIC"
        Anims(851).Name = "CPR"
        Anims(852)._Lib = "MISC"
        Anims(852).Name = "bitchslap"
        Anims(853)._Lib = "MISC"
        Anims(853).Name = "BMX_celebrate"
        Anims(854)._Lib = "MISC"
        Anims(854).Name = "BMX_comeon"
        Anims(855)._Lib = "MISC"
        Anims(855).Name = "bmx_idleloop_01"
        Anims(856)._Lib = "MISC"
        Anims(856).Name = "bmx_idleloop_02"
        Anims(857)._Lib = "MISC"
        Anims(857).Name = "bmx_talkleft_in"
        Anims(858)._Lib = "MISC"
        Anims(858).Name = "bmx_talkleft_loop"
        Anims(859)._Lib = "MISC"
        Anims(859).Name = "bmx_talkleft_out"
        Anims(860)._Lib = "MISC"
        Anims(860).Name = "bmx_talkright_in"
        Anims(861)._Lib = "MISC"
        Anims(861).Name = "bmx_talkright_loop"
        Anims(862)._Lib = "MISC"
        Anims(862).Name = "bmx_talkright_out"
        Anims(863)._Lib = "MISC"
        Anims(863).Name = "bng_wndw"
        Anims(864)._Lib = "MISC"
        Anims(864).Name = "bng_wndw_02"
        Anims(865)._Lib = "MISC"
        Anims(865).Name = "Case_pickup"
        Anims(866)._Lib = "MISC"
        Anims(866).Name = "door_jet"
        Anims(867)._Lib = "MISC"
        Anims(867).Name = "GRAB_L"
        Anims(868)._Lib = "MISC"
        Anims(868).Name = "GRAB_R"
        Anims(869)._Lib = "MISC"
        Anims(869).Name = "Hiker_Pose"
        Anims(870)._Lib = "MISC"
        Anims(870).Name = "Hiker_Pose_L"
        Anims(871)._Lib = "MISC"
        Anims(871).Name = "Idle_Chat_02"
        Anims(872)._Lib = "MISC"
        Anims(872).Name = "KAT_Throw_K"
        Anims(873)._Lib = "MISC"
        Anims(873).Name = "KAT_Throw_O"
        Anims(874)._Lib = "MISC"
        Anims(874).Name = "KAT_Throw_P"
        Anims(875)._Lib = "MISC"
        Anims(875).Name = "PASS_Rifle_O"
        Anims(876)._Lib = "MISC"
        Anims(876).Name = "PASS_Rifle_Ped"
        Anims(877)._Lib = "MISC"
        Anims(877).Name = "PASS_Rifle_Ply"
        Anims(878)._Lib = "MISC"
        Anims(878).Name = "pickup_box"
        Anims(879)._Lib = "MISC"
        Anims(879).Name = "Plane_door"
        Anims(880)._Lib = "MISC"
        Anims(880).Name = "Plane_exit"
        Anims(881)._Lib = "MISC"
        Anims(881).Name = "Plane_hijack"
        Anims(882)._Lib = "MISC"
        Anims(882).Name = "Plunger_01"
        Anims(883)._Lib = "MISC"
        Anims(883).Name = "Plyrlean_loop"
        Anims(884)._Lib = "MISC"
        Anims(884).Name = "plyr_shkhead"
        Anims(885)._Lib = "MISC"
        Anims(885).Name = "Run_Dive"
        Anims(886)._Lib = "MISC"
        Anims(886).Name = "Scratchballs_01"
        Anims(887)._Lib = "MISC"
        Anims(887).Name = "SEAT_LR"
        Anims(888)._Lib = "MISC"
        Anims(888).Name = "Seat_talk_01"
        Anims(889)._Lib = "MISC"
        Anims(889).Name = "Seat_talk_02"
        Anims(890)._Lib = "MISC"
        Anims(890).Name = "SEAT_watch"
        Anims(891)._Lib = "MISC"
        Anims(891).Name = "smalplane_door"
        Anims(892)._Lib = "MISC"
        Anims(892).Name = "smlplane_door"
        Anims(893)._Lib = "MTB"
        Anims(893).Name = "MTB_back"
        Anims(894)._Lib = "MTB"
        Anims(894).Name = "MTB_bunnyhop"
        Anims(895)._Lib = "MTB"
        Anims(895).Name = "MTB_drivebyFT"
        Anims(896)._Lib = "MTB"
        Anims(896).Name = "MTB_driveby_LHS"
        Anims(897)._Lib = "MTB"
        Anims(897).Name = "MTB_driveby_RHS"
        Anims(898)._Lib = "MTB"
        Anims(898).Name = "MTB_fwd"
        Anims(899)._Lib = "MTB"
        Anims(899).Name = "MTB_getoffBACK"
        Anims(900)._Lib = "MTB"
        Anims(900).Name = "MTB_getoffLHS"
        Anims(901)._Lib = "MTB"
        Anims(901).Name = "MTB_getoffRHS"
        Anims(902)._Lib = "MTB"
        Anims(902).Name = "MTB_jumponL"
        Anims(903)._Lib = "MTB"
        Anims(903).Name = "MTB_jumponR"
        Anims(904)._Lib = "MTB"
        Anims(904).Name = "MTB_Left"
        Anims(905)._Lib = "MTB"
        Anims(905).Name = "MTB_pedal"
        Anims(906)._Lib = "MTB"
        Anims(906).Name = "MTB_pushes"
        Anims(907)._Lib = "MTB"
        Anims(907).Name = "MTB_Ride"
        Anims(908)._Lib = "MTB"
        Anims(908).Name = "MTB_Right"
        Anims(909)._Lib = "MTB"
        Anims(909).Name = "MTB_sprint"
        Anims(910)._Lib = "MTB"
        Anims(910).Name = "MTB_still"
        Anims(911)._Lib = "MUSCULAR"
        Anims(911).Name = "MscleWalkst_armed"
        Anims(912)._Lib = "MUSCULAR"
        Anims(912).Name = "MscleWalkst_Csaw"
        Anims(913)._Lib = "MUSCULAR"
        Anims(913).Name = "Mscle_rckt_run"
        Anims(914)._Lib = "MUSCULAR"
        Anims(914).Name = "Mscle_rckt_walkst"
        Anims(915)._Lib = "MUSCULAR"
        Anims(915).Name = "Mscle_run_Csaw"
        Anims(916)._Lib = "MUSCULAR"
        Anims(916).Name = "MuscleIdle"
        Anims(917)._Lib = "MUSCULAR"
        Anims(917).Name = "MuscleIdle_armed"
        Anims(918)._Lib = "MUSCULAR"
        Anims(918).Name = "MuscleIdle_Csaw"
        Anims(919)._Lib = "MUSCULAR"
        Anims(919).Name = "MuscleIdle_rocket"
        Anims(920)._Lib = "MUSCULAR"
        Anims(920).Name = "MuscleRun"
        Anims(921)._Lib = "MUSCULAR"
        Anims(921).Name = "MuscleRun_armed"
        Anims(922)._Lib = "MUSCULAR"
        Anims(922).Name = "MuscleSprint"
        Anims(923)._Lib = "MUSCULAR"
        Anims(923).Name = "MuscleWalk"
        Anims(924)._Lib = "MUSCULAR"
        Anims(924).Name = "MuscleWalkstart"
        Anims(925)._Lib = "MUSCULAR"
        Anims(925).Name = "MuscleWalk_armed"
        Anims(926)._Lib = "MUSCULAR"
        Anims(926).Name = "Musclewalk_Csaw"
        Anims(927)._Lib = "MUSCULAR"
        Anims(927).Name = "Musclewalk_rocket"
        Anims(928)._Lib = "NEVADA"
        Anims(928).Name = "NEVADA_getin"
        Anims(929)._Lib = "NEVADA"
        Anims(929).Name = "NEVADA_getout"
        Anims(930)._Lib = "ON_LOOKERS"
        Anims(930).Name = "lkaround_in"
        Anims(931)._Lib = "ON_LOOKERS"
        Anims(931).Name = "lkaround_loop"
        Anims(932)._Lib = "ON_LOOKERS"
        Anims(932).Name = "lkaround_out"
        Anims(933)._Lib = "ON_LOOKERS"
        Anims(933).Name = "lkup_in"
        Anims(934)._Lib = "ON_LOOKERS"
        Anims(934).Name = "lkup_loop"
        Anims(935)._Lib = "ON_LOOKERS"
        Anims(935).Name = "lkup_out"
        Anims(936)._Lib = "ON_LOOKERS"
        Anims(936).Name = "lkup_point"
        Anims(937)._Lib = "ON_LOOKERS"
        Anims(937).Name = "panic_cower"
        Anims(938)._Lib = "ON_LOOKERS"
        Anims(938).Name = "panic_hide"
        Anims(939)._Lib = "ON_LOOKERS"
        Anims(939).Name = "panic_in"
        Anims(940)._Lib = "ON_LOOKERS"
        Anims(940).Name = "panic_loop"
        Anims(941)._Lib = "ON_LOOKERS"
        Anims(941).Name = "panic_out"
        Anims(942)._Lib = "ON_LOOKERS"
        Anims(942).Name = "panic_point"
        Anims(943)._Lib = "ON_LOOKERS"
        Anims(943).Name = "panic_shout"
        Anims(944)._Lib = "ON_LOOKERS"
        Anims(944).Name = "Pointup_in"
        Anims(945)._Lib = "ON_LOOKERS"
        Anims(945).Name = "Pointup_loop"
        Anims(946)._Lib = "ON_LOOKERS"
        Anims(946).Name = "Pointup_out"
        Anims(947)._Lib = "ON_LOOKERS"
        Anims(947).Name = "Pointup_shout"
        Anims(948)._Lib = "ON_LOOKERS"
        Anims(948).Name = "point_in"
        Anims(949)._Lib = "ON_LOOKERS"
        Anims(949).Name = "point_loop"
        Anims(950)._Lib = "ON_LOOKERS"
        Anims(950).Name = "point_out"
        Anims(951)._Lib = "ON_LOOKERS"
        Anims(951).Name = "shout_01"
        Anims(952)._Lib = "ON_LOOKERS"
        Anims(952).Name = "shout_02"
        Anims(953)._Lib = "ON_LOOKERS"
        Anims(953).Name = "shout_in"
        Anims(954)._Lib = "ON_LOOKERS"
        Anims(954).Name = "shout_loop"
        Anims(955)._Lib = "ON_LOOKERS"
        Anims(955).Name = "shout_out"
        Anims(956)._Lib = "ON_LOOKERS"
        Anims(956).Name = "wave_in"
        Anims(957)._Lib = "ON_LOOKERS"
        Anims(957).Name = "wave_loop"
        Anims(958)._Lib = "ON_LOOKERS"
        Anims(958).Name = "wave_out"
        Anims(959)._Lib = "OTB"
        Anims(959).Name = "betslp_in"
        Anims(960)._Lib = "OTB"
        Anims(960).Name = "betslp_lkabt"
        Anims(961)._Lib = "OTB"
        Anims(961).Name = "betslp_loop"
        Anims(962)._Lib = "OTB"
        Anims(962).Name = "betslp_out"
        Anims(963)._Lib = "OTB"
        Anims(963).Name = "betslp_tnk"
        Anims(964)._Lib = "OTB"
        Anims(964).Name = "wtchrace_cmon"
        Anims(965)._Lib = "OTB"
        Anims(965).Name = "wtchrace_in"
        Anims(966)._Lib = "OTB"
        Anims(966).Name = "wtchrace_loop"
        Anims(967)._Lib = "OTB"
        Anims(967).Name = "wtchrace_lose"
        Anims(968)._Lib = "OTB"
        Anims(968).Name = "wtchrace_out"
        Anims(969)._Lib = "OTB"
        Anims(969).Name = "wtchrace_win"
        Anims(970)._Lib = "PARACHUTE"
        Anims(970).Name = "FALL_skyDive"
        Anims(971)._Lib = "PARACHUTE"
        Anims(971).Name = "FALL_SkyDive_Accel"
        Anims(972)._Lib = "PARACHUTE"
        Anims(972).Name = "FALL_skyDive_DIE"
        Anims(973)._Lib = "PARACHUTE"
        Anims(973).Name = "FALL_SkyDive_L"
        Anims(974)._Lib = "PARACHUTE"
        Anims(974).Name = "FALL_SkyDive_R"
        Anims(975)._Lib = "PARACHUTE"
        Anims(975).Name = "PARA_decel"
        Anims(976)._Lib = "PARACHUTE"
        Anims(976).Name = "PARA_decel_O"
        Anims(977)._Lib = "PARACHUTE"
        Anims(977).Name = "PARA_float"
        Anims(978)._Lib = "PARACHUTE"
        Anims(978).Name = "PARA_float_O"
        Anims(979)._Lib = "PARACHUTE"
        Anims(979).Name = "PARA_Land"
        Anims(980)._Lib = "PARACHUTE"
        Anims(980).Name = "PARA_Land_O"
        Anims(981)._Lib = "PARACHUTE"
        Anims(981).Name = "PARA_Land_Water"
        Anims(982)._Lib = "PARACHUTE"
        Anims(982).Name = "PARA_Land_Water_O"
        Anims(983)._Lib = "PARACHUTE"
        Anims(983).Name = "PARA_open"
        Anims(984)._Lib = "PARACHUTE"
        Anims(984).Name = "PARA_open_O"
        Anims(985)._Lib = "PARACHUTE"
        Anims(985).Name = "PARA_Rip_Land_O"
        Anims(986)._Lib = "PARACHUTE"
        Anims(986).Name = "PARA_Rip_Loop_O"
        Anims(987)._Lib = "PARACHUTE"
        Anims(987).Name = "PARA_Rip_O"
        Anims(988)._Lib = "PARACHUTE"
        Anims(988).Name = "PARA_steerL"
        Anims(989)._Lib = "PARACHUTE"
        Anims(989).Name = "PARA_steerL_O"
        Anims(990)._Lib = "PARACHUTE"
        Anims(990).Name = "PARA_steerR"
        Anims(991)._Lib = "PARACHUTE"
        Anims(991).Name = "PARA_steerR_O"
        Anims(992)._Lib = "PARK"
        Anims(992).Name = "Tai_Chi_in"
        Anims(993)._Lib = "PARK"
        Anims(993).Name = "Tai_Chi_Loop"
        Anims(994)._Lib = "PARK"
        Anims(994).Name = "Tai_Chi_Out"
        Anims(995)._Lib = "PAULNMAC"
        Anims(995).Name = "Piss_in"
        Anims(996)._Lib = "PAULNMAC"
        Anims(996).Name = "Piss_loop"
        Anims(997)._Lib = "PAULNMAC"
        Anims(997).Name = "Piss_out"
        Anims(998)._Lib = "PAULNMAC"
        Anims(998).Name = "PnM_Argue1_A"
        Anims(999)._Lib = "PAULNMAC"
        Anims(999).Name = "PnM_Argue1_B"
        Anims(1000)._Lib = "PAULNMAC"
        Anims(1000).Name = "PnM_Argue2_A"
        Anims(1001)._Lib = "PAULNMAC"
        Anims(1001).Name = "PnM_Argue2_B"
        Anims(1002)._Lib = "PAULNMAC"
        Anims(1002).Name = "PnM_Loop_A"
        Anims(1003)._Lib = "PAULNMAC"
        Anims(1003).Name = "PnM_Loop_B"
        Anims(1004)._Lib = "PAULNMAC"
        Anims(1004).Name = "wank_in"
        Anims(1005)._Lib = "PAULNMAC"
        Anims(1005).Name = "wank_loop"
        Anims(1006)._Lib = "PAULNMAC"
        Anims(1006).Name = "wank_out"
        Anims(1007)._Lib = "ped"
        Anims(1007).Name = "abseil"
        Anims(1008)._Lib = "ped"
        Anims(1008).Name = "ARRESTgun"
        Anims(1009)._Lib = "ped"
        Anims(1009).Name = "ATM"
        Anims(1010)._Lib = "ped"
        Anims(1010).Name = "BIKE_elbowL"
        Anims(1011)._Lib = "ped"
        Anims(1011).Name = "BIKE_elbowR"
        Anims(1012)._Lib = "ped"
        Anims(1012).Name = "BIKE_fallR"
        Anims(1013)._Lib = "ped"
        Anims(1013).Name = "BIKE_fall_off"
        Anims(1014)._Lib = "ped"
        Anims(1014).Name = "BIKE_pickupL"
        Anims(1015)._Lib = "ped"
        Anims(1015).Name = "BIKE_pickupR"
        Anims(1016)._Lib = "ped"
        Anims(1016).Name = "BIKE_pullupL"
        Anims(1017)._Lib = "ped"
        Anims(1017).Name = "BIKE_pullupR"
        Anims(1018)._Lib = "ped"
        Anims(1018).Name = "bomber"
        Anims(1019)._Lib = "ped"
        Anims(1019).Name = "CAR_alignHI_LHS"
        Anims(1020)._Lib = "ped"
        Anims(1020).Name = "CAR_alignHI_RHS"
        Anims(1021)._Lib = "ped"
        Anims(1021).Name = "CAR_align_LHS"
        Anims(1022)._Lib = "ped"
        Anims(1022).Name = "CAR_align_RHS"
        Anims(1023)._Lib = "ped"
        Anims(1023).Name = "CAR_closedoorL_LHS"
        Anims(1024)._Lib = "ped"
        Anims(1024).Name = "CAR_closedoorL_RHS"
        Anims(1025)._Lib = "ped"
        Anims(1025).Name = "CAR_closedoor_LHS"
        Anims(1026)._Lib = "ped"
        Anims(1026).Name = "CAR_closedoor_RHS"
        Anims(1027)._Lib = "ped"
        Anims(1027).Name = "CAR_close_LHS"
        Anims(1028)._Lib = "ped"
        Anims(1028).Name = "CAR_close_RHS"
        Anims(1029)._Lib = "ped"
        Anims(1029).Name = "CAR_crawloutRHS"
        Anims(1030)._Lib = "ped"
        Anims(1030).Name = "CAR_dead_LHS"
        Anims(1031)._Lib = "ped"
        Anims(1031).Name = "CAR_dead_RHS"
        Anims(1032)._Lib = "ped"
        Anims(1032).Name = "CAR_doorlocked_LHS"
        Anims(1033)._Lib = "ped"
        Anims(1033).Name = "CAR_doorlocked_RHS"
        Anims(1034)._Lib = "ped"
        Anims(1034).Name = "CAR_fallout_LHS"
        Anims(1035)._Lib = "ped"
        Anims(1035).Name = "CAR_fallout_RHS"
        Anims(1036)._Lib = "ped"
        Anims(1036).Name = "CAR_getinL_LHS"
        Anims(1037)._Lib = "ped"
        Anims(1037).Name = "CAR_getinL_RHS"
        Anims(1038)._Lib = "ped"
        Anims(1038).Name = "CAR_getin_LHS"
        Anims(1039)._Lib = "ped"
        Anims(1039).Name = "CAR_getin_RHS"
        Anims(1040)._Lib = "ped"
        Anims(1040).Name = "CAR_getoutL_LHS"
        Anims(1041)._Lib = "ped"
        Anims(1041).Name = "CAR_getoutL_RHS"
        Anims(1042)._Lib = "ped"
        Anims(1042).Name = "CAR_getout_LHS"
        Anims(1043)._Lib = "ped"
        Anims(1043).Name = "CAR_getout_RHS"
        Anims(1044)._Lib = "ped"
        Anims(1044).Name = "car_hookertalk"
        Anims(1045)._Lib = "ped"
        Anims(1045).Name = "CAR_jackedLHS"
        Anims(1046)._Lib = "ped"
        Anims(1046).Name = "CAR_jackedRHS"
        Anims(1047)._Lib = "ped"
        Anims(1047).Name = "CAR_jumpin_LHS"
        Anims(1048)._Lib = "ped"
        Anims(1048).Name = "CAR_LB"
        Anims(1049)._Lib = "ped"
        Anims(1049).Name = "CAR_LB_pro"
        Anims(1050)._Lib = "ped"
        Anims(1050).Name = "CAR_LB_weak"
        Anims(1051)._Lib = "ped"
        Anims(1051).Name = "CAR_LjackedLHS"
        Anims(1052)._Lib = "ped"
        Anims(1052).Name = "CAR_LjackedRHS"
        Anims(1053)._Lib = "ped"
        Anims(1053).Name = "CAR_Lshuffle_RHS"
        Anims(1054)._Lib = "ped"
        Anims(1054).Name = "CAR_Lsit"
        Anims(1055)._Lib = "ped"
        Anims(1055).Name = "CAR_open_LHS"
        Anims(1056)._Lib = "ped"
        Anims(1056).Name = "CAR_open_RHS"
        Anims(1057)._Lib = "ped"
        Anims(1057).Name = "CAR_pulloutL_LHS"
        Anims(1058)._Lib = "ped"
        Anims(1058).Name = "CAR_pulloutL_RHS"
        Anims(1059)._Lib = "ped"
        Anims(1059).Name = "CAR_pullout_LHS"
        Anims(1060)._Lib = "ped"
        Anims(1060).Name = "CAR_pullout_RHS"
        Anims(1061)._Lib = "ped"
        Anims(1061).Name = "CAR_Qjacked"
        Anims(1062)._Lib = "ped"
        Anims(1062).Name = "CAR_rolldoor"
        Anims(1063)._Lib = "ped"
        Anims(1063).Name = "CAR_rolldoorLO"
        Anims(1064)._Lib = "ped"
        Anims(1064).Name = "CAR_rollout_LHS"
        Anims(1065)._Lib = "ped"
        Anims(1065).Name = "CAR_rollout_RHS"
        Anims(1066)._Lib = "ped"
        Anims(1066).Name = "CAR_shuffle_RHS"
        Anims(1067)._Lib = "ped"
        Anims(1067).Name = "CAR_sit"
        Anims(1068)._Lib = "ped"
        Anims(1068).Name = "CAR_sitp"
        Anims(1069)._Lib = "ped"
        Anims(1069).Name = "CAR_sitpLO"
        Anims(1070)._Lib = "ped"
        Anims(1070).Name = "CAR_sit_pro"
        Anims(1071)._Lib = "ped"
        Anims(1071).Name = "CAR_sit_weak"
        Anims(1072)._Lib = "ped"
        Anims(1072).Name = "CAR_tune_radio"
        Anims(1073)._Lib = "ped"
        Anims(1073).Name = "CLIMB_idle"
        Anims(1074)._Lib = "ped"
        Anims(1074).Name = "CLIMB_jump"
        Anims(1075)._Lib = "ped"
        Anims(1075).Name = "CLIMB_jump2fall"
        Anims(1076)._Lib = "ped"
        Anims(1076).Name = "CLIMB_jump_B"
        Anims(1077)._Lib = "ped"
        Anims(1077).Name = "CLIMB_Pull"
        Anims(1078)._Lib = "ped"
        Anims(1078).Name = "CLIMB_Stand"
        Anims(1079)._Lib = "ped"
        Anims(1079).Name = "CLIMB_Stand_finish"
        Anims(1080)._Lib = "ped"
        Anims(1080).Name = "cower"
        Anims(1081)._Lib = "ped"
        Anims(1081).Name = "Crouch_Roll_L"
        Anims(1082)._Lib = "ped"
        Anims(1082).Name = "Crouch_Roll_R"
        Anims(1083)._Lib = "ped"
        Anims(1083).Name = "DAM_armL_frmBK"
        Anims(1084)._Lib = "ped"
        Anims(1084).Name = "DAM_armL_frmFT"
        Anims(1085)._Lib = "ped"
        Anims(1085).Name = "DAM_armL_frmLT"
        Anims(1086)._Lib = "ped"
        Anims(1086).Name = "DAM_armR_frmBK"
        Anims(1087)._Lib = "ped"
        Anims(1087).Name = "DAM_armR_frmFT"
        Anims(1088)._Lib = "ped"
        Anims(1088).Name = "DAM_armR_frmRT"
        Anims(1089)._Lib = "ped"
        Anims(1089).Name = "DAM_LegL_frmBK"
        Anims(1090)._Lib = "ped"
        Anims(1090).Name = "DAM_LegL_frmFT"
        Anims(1091)._Lib = "ped"
        Anims(1091).Name = "DAM_LegL_frmLT"
        Anims(1092)._Lib = "ped"
        Anims(1092).Name = "DAM_LegR_frmBK"
        Anims(1093)._Lib = "ped"
        Anims(1093).Name = "DAM_LegR_frmFT"
        Anims(1094)._Lib = "ped"
        Anims(1094).Name = "DAM_LegR_frmRT"
        Anims(1095)._Lib = "ped"
        Anims(1095).Name = "DAM_stomach_frmBK"
        Anims(1096)._Lib = "ped"
        Anims(1096).Name = "DAM_stomach_frmFT"
        Anims(1097)._Lib = "ped"
        Anims(1097).Name = "DAM_stomach_frmLT"
        Anims(1098)._Lib = "ped"
        Anims(1098).Name = "DAM_stomach_frmRT"
        Anims(1099)._Lib = "ped"
        Anims(1099).Name = "DOOR_LHinge_O"
        Anims(1100)._Lib = "ped"
        Anims(1100).Name = "DOOR_RHinge_O"
        Anims(1101)._Lib = "ped"
        Anims(1101).Name = "DrivebyL_L"
        Anims(1102)._Lib = "ped"
        Anims(1102).Name = "DrivebyL_R"
        Anims(1103)._Lib = "ped"
        Anims(1103).Name = "Driveby_L"
        Anims(1104)._Lib = "ped"
        Anims(1104).Name = "Driveby_R"
        Anims(1105)._Lib = "ped"
        Anims(1105).Name = "DRIVE_BOAT"
        Anims(1106)._Lib = "ped"
        Anims(1106).Name = "DRIVE_BOAT_back"
        Anims(1107)._Lib = "ped"
        Anims(1107).Name = "DRIVE_BOAT_L"
        Anims(1108)._Lib = "ped"
        Anims(1108).Name = "DRIVE_BOAT_R"
        Anims(1109)._Lib = "ped"
        Anims(1109).Name = "Drive_L"
        Anims(1110)._Lib = "ped"
        Anims(1110).Name = "Drive_LO_l"
        Anims(1111)._Lib = "ped"
        Anims(1111).Name = "Drive_LO_R"
        Anims(1112)._Lib = "ped"
        Anims(1112).Name = "Drive_L_pro"
        Anims(1113)._Lib = "ped"
        Anims(1113).Name = "Drive_L_pro_slow"
        Anims(1114)._Lib = "ped"
        Anims(1114).Name = "Drive_L_slow"
        Anims(1115)._Lib = "ped"
        Anims(1115).Name = "Drive_L_weak"
        Anims(1116)._Lib = "ped"
        Anims(1116).Name = "Drive_L_weak_slow"
        Anims(1117)._Lib = "ped"
        Anims(1117).Name = "Drive_R"
        Anims(1118)._Lib = "ped"
        Anims(1118).Name = "Drive_R_pro"
        Anims(1119)._Lib = "ped"
        Anims(1119).Name = "Drive_R_pro_slow"
        Anims(1120)._Lib = "ped"
        Anims(1120).Name = "Drive_R_slow"
        Anims(1121)._Lib = "ped"
        Anims(1121).Name = "Drive_R_weak"
        Anims(1122)._Lib = "ped"
        Anims(1122).Name = "Drive_R_weak_slow"
        Anims(1123)._Lib = "ped"
        Anims(1123).Name = "Drive_truck"
        Anims(1124)._Lib = "ped"
        Anims(1124).Name = "DRIVE_truck_back"
        Anims(1125)._Lib = "ped"
        Anims(1125).Name = "DRIVE_truck_L"
        Anims(1126)._Lib = "ped"
        Anims(1126).Name = "DRIVE_truck_R"
        Anims(1127)._Lib = "ped"
        Anims(1127).Name = "Drown"
        Anims(1128)._Lib = "ped"
        Anims(1128).Name = "DUCK_cower"
        Anims(1129)._Lib = "ped"
        Anims(1129).Name = "endchat_01"
        Anims(1130)._Lib = "ped"
        Anims(1130).Name = "endchat_02"
        Anims(1131)._Lib = "ped"
        Anims(1131).Name = "endchat_03"
        Anims(1132)._Lib = "ped"
        Anims(1132).Name = "EV_dive"
        Anims(1133)._Lib = "ped"
        Anims(1133).Name = "EV_step"
        Anims(1134)._Lib = "ped"
        Anims(1134).Name = "facanger"
        Anims(1135)._Lib = "ped"
        Anims(1135).Name = "facgum"
        Anims(1136)._Lib = "ped"
        Anims(1136).Name = "facsurp"
        Anims(1137)._Lib = "ped"
        Anims(1137).Name = "facsurpm"
        Anims(1138)._Lib = "ped"
        Anims(1138).Name = "factalk"
        Anims(1139)._Lib = "ped"
        Anims(1139).Name = "facurios"
        Anims(1140)._Lib = "ped"
        Anims(1140).Name = "FALL_back"
        Anims(1141)._Lib = "ped"
        Anims(1141).Name = "FALL_collapse"
        Anims(1142)._Lib = "ped"
        Anims(1142).Name = "FALL_fall"
        Anims(1143)._Lib = "ped"
        Anims(1143).Name = "FALL_front"
        Anims(1144)._Lib = "ped"
        Anims(1144).Name = "FALL_glide"
        Anims(1145)._Lib = "ped"
        Anims(1145).Name = "FALL_land"
        Anims(1146)._Lib = "ped"
        Anims(1146).Name = "FALL_skyDive"
        Anims(1147)._Lib = "ped"
        Anims(1147).Name = "Fight2Idle"
        Anims(1148)._Lib = "ped"
        Anims(1148).Name = "FightA_1"
        Anims(1149)._Lib = "ped"
        Anims(1149).Name = "FightA_2"
        Anims(1150)._Lib = "ped"
        Anims(1150).Name = "FightA_3"
        Anims(1151)._Lib = "ped"
        Anims(1151).Name = "FightA_block"
        Anims(1152)._Lib = "ped"
        Anims(1152).Name = "FightA_G"
        Anims(1153)._Lib = "ped"
        Anims(1153).Name = "FightA_M"
        Anims(1154)._Lib = "ped"
        Anims(1154).Name = "FIGHTIDLE"
        Anims(1155)._Lib = "ped"
        Anims(1155).Name = "FightShB"
        Anims(1156)._Lib = "ped"
        Anims(1156).Name = "FightShF"
        Anims(1157)._Lib = "ped"
        Anims(1157).Name = "FightSh_BWD"
        Anims(1158)._Lib = "ped"
        Anims(1158).Name = "FightSh_FWD"
        Anims(1159)._Lib = "ped"
        Anims(1159).Name = "FightSh_Left"
        Anims(1160)._Lib = "ped"
        Anims(1160).Name = "FightSh_Right"
        Anims(1161)._Lib = "ped"
        Anims(1161).Name = "flee_lkaround_01"
        Anims(1162)._Lib = "ped"
        Anims(1162).Name = "FLOOR_hit"
        Anims(1163)._Lib = "ped"
        Anims(1163).Name = "FLOOR_hit_f"
        Anims(1164)._Lib = "ped"
        Anims(1164).Name = "fucku"
        Anims(1165)._Lib = "ped"
        Anims(1165).Name = "gang_gunstand"
        Anims(1166)._Lib = "ped"
        Anims(1166).Name = "gas_cwr"
        Anims(1167)._Lib = "ped"
        Anims(1167).Name = "getup"
        Anims(1168)._Lib = "ped"
        Anims(1168).Name = "getup_front"
        Anims(1169)._Lib = "ped"
        Anims(1169).Name = "gum_eat"
        Anims(1170)._Lib = "ped"
        Anims(1170).Name = "GunCrouchBwd"
        Anims(1171)._Lib = "ped"
        Anims(1171).Name = "GunCrouchFwd"
        Anims(1172)._Lib = "ped"
        Anims(1172).Name = "GunMove_BWD"
        Anims(1173)._Lib = "ped"
        Anims(1173).Name = "GunMove_FWD"
        Anims(1174)._Lib = "ped"
        Anims(1174).Name = "GunMove_L"
        Anims(1175)._Lib = "ped"
        Anims(1175).Name = "GunMove_R"
        Anims(1176)._Lib = "ped"
        Anims(1176).Name = "Gun_2_IDLE"
        Anims(1177)._Lib = "ped"
        Anims(1177).Name = "GUN_BUTT"
        Anims(1178)._Lib = "ped"
        Anims(1178).Name = "GUN_BUTT_crouch"
        Anims(1179)._Lib = "ped"
        Anims(1179).Name = "Gun_stand"
        Anims(1180)._Lib = "ped"
        Anims(1180).Name = "handscower"
        Anims(1181)._Lib = "ped"
        Anims(1181).Name = "handsup"
        Anims(1182)._Lib = "ped"
        Anims(1182).Name = "HitA_1"
        Anims(1183)._Lib = "ped"
        Anims(1183).Name = "HitA_2"
        Anims(1184)._Lib = "ped"
        Anims(1184).Name = "HitA_3"
        Anims(1185)._Lib = "ped"
        Anims(1185).Name = "HIT_back"
        Anims(1186)._Lib = "ped"
        Anims(1186).Name = "HIT_behind"
        Anims(1187)._Lib = "ped"
        Anims(1187).Name = "HIT_front"
        Anims(1188)._Lib = "ped"
        Anims(1188).Name = "HIT_GUN_BUTT"
        Anims(1189)._Lib = "ped"
        Anims(1189).Name = "HIT_L"
        Anims(1190)._Lib = "ped"
        Anims(1190).Name = "HIT_R"
        Anims(1191)._Lib = "ped"
        Anims(1191).Name = "HIT_walk"
        Anims(1192)._Lib = "ped"
        Anims(1192).Name = "HIT_wall"
        Anims(1193)._Lib = "ped"
        Anims(1193).Name = "Idlestance_fat"
        Anims(1194)._Lib = "ped"
        Anims(1194).Name = "idlestance_old"
        Anims(1195)._Lib = "ped"
        Anims(1195).Name = "IDLE_armed"
        Anims(1196)._Lib = "ped"
        Anims(1196).Name = "IDLE_chat"
        Anims(1197)._Lib = "ped"
        Anims(1197).Name = "IDLE_csaw"
        Anims(1198)._Lib = "ped"
        Anims(1198).Name = "Idle_Gang1"
        Anims(1199)._Lib = "ped"
        Anims(1199).Name = "IDLE_HBHB"
        Anims(1200)._Lib = "ped"
        Anims(1200).Name = "IDLE_ROCKET"
        Anims(1201)._Lib = "ped"
        Anims(1201).Name = "IDLE_stance"
        Anims(1202)._Lib = "ped"
        Anims(1202).Name = "IDLE_taxi"
        Anims(1203)._Lib = "ped"
        Anims(1203).Name = "IDLE_tired"
        Anims(1204)._Lib = "ped"
        Anims(1204).Name = "Jetpack_Idle"
        Anims(1205)._Lib = "ped"
        Anims(1205).Name = "JOG_femaleA"
        Anims(1206)._Lib = "ped"
        Anims(1206).Name = "JOG_maleA"
        Anims(1207)._Lib = "ped"
        Anims(1207).Name = "JUMP_glide"
        Anims(1208)._Lib = "ped"
        Anims(1208).Name = "JUMP_land"
        Anims(1209)._Lib = "ped"
        Anims(1209).Name = "JUMP_launch"
        Anims(1210)._Lib = "ped"
        Anims(1210).Name = "JUMP_launch_R"
        Anims(1211)._Lib = "ped"
        Anims(1211).Name = "KART_drive"
        Anims(1212)._Lib = "ped"
        Anims(1212).Name = "KART_L"
        Anims(1213)._Lib = "ped"
        Anims(1213).Name = "KART_LB"
        Anims(1214)._Lib = "ped"
        Anims(1214).Name = "KART_R"
        Anims(1215)._Lib = "ped"
        Anims(1215).Name = "KD_left"
        Anims(1216)._Lib = "ped"
        Anims(1216).Name = "KD_right"
        Anims(1217)._Lib = "ped"
        Anims(1217).Name = "KO_shot_face"
        Anims(1218)._Lib = "ped"
        Anims(1218).Name = "KO_shot_front"
        Anims(1219)._Lib = "ped"
        Anims(1219).Name = "KO_shot_stom"
        Anims(1220)._Lib = "ped"
        Anims(1220).Name = "KO_skid_back"
        Anims(1221)._Lib = "ped"
        Anims(1221).Name = "KO_skid_front"
        Anims(1222)._Lib = "ped"
        Anims(1222).Name = "KO_spin_L"
        Anims(1223)._Lib = "ped"
        Anims(1223).Name = "KO_spin_R"
        Anims(1224)._Lib = "ped"
        Anims(1224).Name = "pass_Smoke_in_car"
        Anims(1225)._Lib = "ped"
        Anims(1225).Name = "phone_in"
        Anims(1226)._Lib = "ped"
        Anims(1226).Name = "phone_out"
        Anims(1227)._Lib = "ped"
        Anims(1227).Name = "phone_talk"
        Anims(1228)._Lib = "ped"
        Anims(1228).Name = "Player_Sneak"
        Anims(1229)._Lib = "ped"
        Anims(1229).Name = "Player_Sneak_walkstart"
        Anims(1230)._Lib = "ped"
        Anims(1230).Name = "roadcross"
        Anims(1231)._Lib = "ped"
        Anims(1231).Name = "roadcross_female"
        Anims(1232)._Lib = "ped"
        Anims(1232).Name = "roadcross_gang"
        Anims(1233)._Lib = "ped"
        Anims(1233).Name = "roadcross_old"
        Anims(1234)._Lib = "ped"
        Anims(1234).Name = "run_1armed"
        Anims(1235)._Lib = "ped"
        Anims(1235).Name = "run_armed"
        Anims(1236)._Lib = "ped"
        Anims(1236).Name = "run_civi"
        Anims(1237)._Lib = "ped"
        Anims(1237).Name = "run_csaw"
        Anims(1238)._Lib = "ped"
        Anims(1238).Name = "run_fat"
        Anims(1239)._Lib = "ped"
        Anims(1239).Name = "run_fatold"
        Anims(1240)._Lib = "ped"
        Anims(1240).Name = "run_gang1"
        Anims(1241)._Lib = "ped"
        Anims(1241).Name = "run_left"
        Anims(1242)._Lib = "ped"
        Anims(1242).Name = "run_old"
        Anims(1243)._Lib = "ped"
        Anims(1243).Name = "run_player"
        Anims(1244)._Lib = "ped"
        Anims(1244).Name = "run_right"
        Anims(1245)._Lib = "ped"
        Anims(1245).Name = "run_rocket"
        Anims(1246)._Lib = "ped"
        Anims(1246).Name = "Run_stop"
        Anims(1247)._Lib = "ped"
        Anims(1247).Name = "Run_stopR"
        Anims(1248)._Lib = "ped"
        Anims(1248).Name = "Run_Wuzi"
        Anims(1249)._Lib = "ped"
        Anims(1249).Name = "SEAT_down"
        Anims(1250)._Lib = "ped"
        Anims(1250).Name = "SEAT_idle"
        Anims(1251)._Lib = "ped"
        Anims(1251).Name = "SEAT_up"
        Anims(1252)._Lib = "ped"
        Anims(1252).Name = "SHOT_leftP"
        Anims(1253)._Lib = "ped"
        Anims(1253).Name = "SHOT_partial"
        Anims(1254)._Lib = "ped"
        Anims(1254).Name = "SHOT_partial_B"
        Anims(1255)._Lib = "ped"
        Anims(1255).Name = "SHOT_rightP"
        Anims(1256)._Lib = "ped"
        Anims(1256).Name = "Shove_Partial"
        Anims(1257)._Lib = "ped"
        Anims(1257).Name = "Smoke_in_car"
        Anims(1258)._Lib = "ped"
        Anims(1258).Name = "sprint_civi"
        Anims(1259)._Lib = "ped"
        Anims(1259).Name = "sprint_panic"
        Anims(1260)._Lib = "ped"
        Anims(1260).Name = "Sprint_Wuzi"
        Anims(1261)._Lib = "ped"
        Anims(1261).Name = "swat_run"
        Anims(1262)._Lib = "ped"
        Anims(1262).Name = "Swim_Tread"
        Anims(1263)._Lib = "ped"
        Anims(1263).Name = "Tap_hand"
        Anims(1264)._Lib = "ped"
        Anims(1264).Name = "Tap_handP"
        Anims(1265)._Lib = "ped"
        Anims(1265).Name = "turn_180"
        Anims(1266)._Lib = "ped"
        Anims(1266).Name = "Turn_L"
        Anims(1267)._Lib = "ped"
        Anims(1267).Name = "Turn_R"
        Anims(1268)._Lib = "ped"
        Anims(1268).Name = "WALK_armed"
        Anims(1269)._Lib = "ped"
        Anims(1269).Name = "WALK_civi"
        Anims(1270)._Lib = "ped"
        Anims(1270).Name = "WALK_csaw"
        Anims(1271)._Lib = "ped"
        Anims(1271).Name = "Walk_DoorPartial"
        Anims(1272)._Lib = "ped"
        Anims(1272).Name = "WALK_drunk"
        Anims(1273)._Lib = "ped"
        Anims(1273).Name = "WALK_fat"
        Anims(1274)._Lib = "ped"
        Anims(1274).Name = "WALK_fatold"
        Anims(1275)._Lib = "ped"
        Anims(1275).Name = "WALK_gang1"
        Anims(1276)._Lib = "ped"
        Anims(1276).Name = "WALK_gang2"
        Anims(1277)._Lib = "ped"
        Anims(1277).Name = "WALK_old"
        Anims(1278)._Lib = "ped"
        Anims(1278).Name = "WALK_player"
        Anims(1279)._Lib = "ped"
        Anims(1279).Name = "WALK_rocket"
        Anims(1280)._Lib = "ped"
        Anims(1280).Name = "WALK_shuffle"
        Anims(1281)._Lib = "ped"
        Anims(1281).Name = "WALK_start"
        Anims(1282)._Lib = "ped"
        Anims(1282).Name = "WALK_start_armed"
        Anims(1283)._Lib = "ped"
        Anims(1283).Name = "WALK_start_csaw"
        Anims(1284)._Lib = "ped"
        Anims(1284).Name = "WALK_start_rocket"
        Anims(1285)._Lib = "ped"
        Anims(1285).Name = "Walk_Wuzi"
        Anims(1286)._Lib = "ped"
        Anims(1286).Name = "WEAPON_crouch"
        Anims(1287)._Lib = "ped"
        Anims(1287).Name = "woman_idlestance"
        Anims(1288)._Lib = "ped"
        Anims(1288).Name = "woman_run"
        Anims(1289)._Lib = "ped"
        Anims(1289).Name = "WOMAN_runbusy"
        Anims(1290)._Lib = "ped"
        Anims(1290).Name = "WOMAN_runfatold"
        Anims(1291)._Lib = "ped"
        Anims(1291).Name = "woman_runpanic"
        Anims(1292)._Lib = "ped"
        Anims(1292).Name = "WOMAN_runsexy"
        Anims(1293)._Lib = "ped"
        Anims(1293).Name = "WOMAN_walkbusy"
        Anims(1294)._Lib = "ped"
        Anims(1294).Name = "WOMAN_walkfatold"
        Anims(1295)._Lib = "ped"
        Anims(1295).Name = "WOMAN_walknorm"
        Anims(1296)._Lib = "ped"
        Anims(1296).Name = "WOMAN_walkold"
        Anims(1297)._Lib = "ped"
        Anims(1297).Name = "WOMAN_walkpro"
        Anims(1298)._Lib = "ped"
        Anims(1298).Name = "WOMAN_walksexy"
        Anims(1299)._Lib = "ped"
        Anims(1299).Name = "WOMAN_walkshop"
        Anims(1300)._Lib = "ped"
        Anims(1300).Name = "XPRESSscratch"
        Anims(1301)._Lib = "PLAYER_DVBYS"
        Anims(1301).Name = "Plyr_DrivebyBwd"
        Anims(1302)._Lib = "PLAYER_DVBYS"
        Anims(1302).Name = "Plyr_DrivebyFwd"
        Anims(1303)._Lib = "PLAYER_DVBYS"
        Anims(1303).Name = "Plyr_DrivebyLHS"
        Anims(1304)._Lib = "PLAYER_DVBYS"
        Anims(1304).Name = "Plyr_DrivebyRHS"
        Anims(1305)._Lib = "PLAYIDLES"
        Anims(1305).Name = "shift"
        Anims(1306)._Lib = "PLAYIDLES"
        Anims(1306).Name = "shldr"
        Anims(1307)._Lib = "PLAYIDLES"
        Anims(1307).Name = "stretch"
        Anims(1308)._Lib = "PLAYIDLES"
        Anims(1308).Name = "strleg"
        Anims(1309)._Lib = "PLAYIDLES"
        Anims(1309).Name = "time"
        Anims(1310)._Lib = "POLICE"
        Anims(1310).Name = "CopTraf_Away"
        Anims(1311)._Lib = "POLICE"
        Anims(1311).Name = "CopTraf_Come"
        Anims(1312)._Lib = "POLICE"
        Anims(1312).Name = "CopTraf_Left"
        Anims(1313)._Lib = "POLICE"
        Anims(1313).Name = "CopTraf_Stop"
        Anims(1314)._Lib = "POLICE"
        Anims(1314).Name = "COP_getoutcar_LHS"
        Anims(1315)._Lib = "POLICE"
        Anims(1315).Name = "Cop_move_FWD"
        Anims(1316)._Lib = "POLICE"
        Anims(1316).Name = "crm_drgbst_01"
        Anims(1317)._Lib = "POLICE"
        Anims(1317).Name = "Door_Kick"
        Anims(1318)._Lib = "POLICE"
        Anims(1318).Name = "plc_drgbst_01"
        Anims(1319)._Lib = "POLICE"
        Anims(1319).Name = "plc_drgbst_02"
        Anims(1320)._Lib = "POOL"
        Anims(1320).Name = "POOL_ChalkCue"
        Anims(1321)._Lib = "POOL"
        Anims(1321).Name = "POOL_Idle_Stance"
        Anims(1322)._Lib = "POOL"
        Anims(1322).Name = "POOL_Long_Shot"
        Anims(1323)._Lib = "POOL"
        Anims(1323).Name = "POOL_Long_Shot_O"
        Anims(1324)._Lib = "POOL"
        Anims(1324).Name = "POOL_Long_Start"
        Anims(1325)._Lib = "POOL"
        Anims(1325).Name = "POOL_Long_Start_O"
        Anims(1326)._Lib = "POOL"
        Anims(1326).Name = "POOL_Med_Shot"
        Anims(1327)._Lib = "POOL"
        Anims(1327).Name = "POOL_Med_Shot_O"
        Anims(1328)._Lib = "POOL"
        Anims(1328).Name = "POOL_Med_Start"
        Anims(1329)._Lib = "POOL"
        Anims(1329).Name = "POOL_Med_Start_O"
        Anims(1330)._Lib = "POOL"
        Anims(1330).Name = "POOL_Place_White"
        Anims(1331)._Lib = "POOL"
        Anims(1331).Name = "POOL_Short_Shot"
        Anims(1332)._Lib = "POOL"
        Anims(1332).Name = "POOL_Short_Shot_O"
        Anims(1333)._Lib = "POOL"
        Anims(1333).Name = "POOL_Short_Start"
        Anims(1334)._Lib = "POOL"
        Anims(1334).Name = "POOL_Short_Start_O"
        Anims(1335)._Lib = "POOL"
        Anims(1335).Name = "POOL_Walk"
        Anims(1336)._Lib = "POOL"
        Anims(1336).Name = "POOL_Walk_Start"
        Anims(1337)._Lib = "POOL"
        Anims(1337).Name = "POOL_XLong_Shot"
        Anims(1338)._Lib = "POOL"
        Anims(1338).Name = "POOL_XLong_Shot_O"
        Anims(1339)._Lib = "POOL"
        Anims(1339).Name = "POOL_XLong_Start"
        Anims(1340)._Lib = "POOL"
        Anims(1340).Name = "POOL_XLong_Start_O"
        Anims(1341)._Lib = "POOR"
        Anims(1341).Name = "WINWASH_Start"
        Anims(1342)._Lib = "POOR"
        Anims(1342).Name = "WINWASH_Wash2Beg"
        Anims(1343)._Lib = "PYTHON"
        Anims(1343).Name = "python_crouchfire"
        Anims(1344)._Lib = "PYTHON"
        Anims(1344).Name = "python_crouchreload"
        Anims(1345)._Lib = "PYTHON"
        Anims(1345).Name = "python_fire"
        Anims(1346)._Lib = "PYTHON"
        Anims(1346).Name = "python_fire_poor"
        Anims(1347)._Lib = "PYTHON"
        Anims(1347).Name = "python_reload"
        Anims(1348)._Lib = "QUAD"
        Anims(1348).Name = "QUAD_back"
        Anims(1349)._Lib = "QUAD"
        Anims(1349).Name = "QUAD_driveby_FT"
        Anims(1350)._Lib = "QUAD"
        Anims(1350).Name = "QUAD_driveby_LHS"
        Anims(1351)._Lib = "QUAD"
        Anims(1351).Name = "QUAD_driveby_RHS"
        Anims(1352)._Lib = "QUAD"
        Anims(1352).Name = "QUAD_FWD"
        Anims(1353)._Lib = "QUAD"
        Anims(1353).Name = "QUAD_getoff_B"
        Anims(1354)._Lib = "QUAD"
        Anims(1354).Name = "QUAD_getoff_LHS"
        Anims(1355)._Lib = "QUAD"
        Anims(1355).Name = "QUAD_getoff_RHS"
        Anims(1356)._Lib = "QUAD"
        Anims(1356).Name = "QUAD_geton_LHS"
        Anims(1357)._Lib = "QUAD"
        Anims(1357).Name = "QUAD_geton_RHS"
        Anims(1358)._Lib = "QUAD"
        Anims(1358).Name = "QUAD_hit"
        Anims(1359)._Lib = "QUAD"
        Anims(1359).Name = "QUAD_kick"
        Anims(1360)._Lib = "QUAD"
        Anims(1360).Name = "QUAD_Left"
        Anims(1361)._Lib = "QUAD"
        Anims(1361).Name = "QUAD_passenger"
        Anims(1362)._Lib = "QUAD"
        Anims(1362).Name = "QUAD_reverse"
        Anims(1363)._Lib = "QUAD"
        Anims(1363).Name = "QUAD_ride"
        Anims(1364)._Lib = "QUAD"
        Anims(1364).Name = "QUAD_Right"
        Anims(1365)._Lib = "QUAD_DBZ"
        Anims(1365).Name = "Pass_Driveby_BWD"
        Anims(1366)._Lib = "QUAD_DBZ"
        Anims(1366).Name = "Pass_Driveby_FWD"
        Anims(1367)._Lib = "QUAD_DBZ"
        Anims(1367).Name = "Pass_Driveby_LHS"
        Anims(1368)._Lib = "QUAD_DBZ"
        Anims(1368).Name = "Pass_Driveby_RHS"
        Anims(1369)._Lib = "RAPPING"
        Anims(1369).Name = "Laugh_01"
        Anims(1370)._Lib = "RAPPING"
        Anims(1370).Name = "RAP_A_IN"
        Anims(1371)._Lib = "RAPPING"
        Anims(1371).Name = "RAP_A_Loop"
        Anims(1372)._Lib = "RAPPING"
        Anims(1372).Name = "RAP_A_OUT"
        Anims(1373)._Lib = "RAPPING"
        Anims(1373).Name = "RAP_B_IN"
        Anims(1374)._Lib = "RAPPING"
        Anims(1374).Name = "RAP_B_Loop"
        Anims(1375)._Lib = "RAPPING"
        Anims(1375).Name = "RAP_B_OUT"
        Anims(1376)._Lib = "RAPPING"
        Anims(1376).Name = "RAP_C_Loop"
        Anims(1377)._Lib = "RIFLE"
        Anims(1377).Name = "RIFLE_crouchfire"
        Anims(1378)._Lib = "RIFLE"
        Anims(1378).Name = "RIFLE_crouchload"
        Anims(1379)._Lib = "RIFLE"
        Anims(1379).Name = "RIFLE_fire"
        Anims(1380)._Lib = "RIFLE"
        Anims(1380).Name = "RIFLE_fire_poor"
        Anims(1381)._Lib = "RIFLE"
        Anims(1381).Name = "RIFLE_load"
        Anims(1382)._Lib = "RIOT"
        Anims(1382).Name = "RIOT_ANGRY"
        Anims(1383)._Lib = "RIOT"
        Anims(1383).Name = "RIOT_ANGRY_B"
        Anims(1384)._Lib = "RIOT"
        Anims(1384).Name = "RIOT_challenge"
        Anims(1385)._Lib = "RIOT"
        Anims(1385).Name = "RIOT_CHANT"
        Anims(1386)._Lib = "RIOT"
        Anims(1386).Name = "RIOT_FUKU"
        Anims(1387)._Lib = "RIOT"
        Anims(1387).Name = "RIOT_PUNCHES"
        Anims(1388)._Lib = "RIOT"
        Anims(1388).Name = "RIOT_shout"
        Anims(1389)._Lib = "ROB_BANK"
        Anims(1389).Name = "CAT_Safe_End"
        Anims(1390)._Lib = "ROB_BANK"
        Anims(1390).Name = "CAT_Safe_Open"
        Anims(1391)._Lib = "ROB_BANK"
        Anims(1391).Name = "CAT_Safe_Open_O"
        Anims(1392)._Lib = "ROB_BANK"
        Anims(1392).Name = "CAT_Safe_Rob"
        Anims(1393)._Lib = "ROB_BANK"
        Anims(1393).Name = "SHP_HandsUp_Scr"
        Anims(1394)._Lib = "ROCKET"
        Anims(1394).Name = "idle_rocket"
        Anims(1395)._Lib = "ROCKET"
        Anims(1395).Name = "RocketFire"
        Anims(1396)._Lib = "ROCKET"
        Anims(1396).Name = "run_rocket"
        Anims(1397)._Lib = "ROCKET"
        Anims(1397).Name = "walk_rocket"
        Anims(1398)._Lib = "ROCKET"
        Anims(1398).Name = "WALK_start_rocket"
        Anims(1399)._Lib = "RUSTLER"
        Anims(1399).Name = "Plane_align_LHS"
        Anims(1400)._Lib = "RUSTLER"
        Anims(1400).Name = "Plane_close"
        Anims(1401)._Lib = "RUSTLER"
        Anims(1401).Name = "Plane_getin"
        Anims(1402)._Lib = "RUSTLER"
        Anims(1402).Name = "Plane_getout"
        Anims(1403)._Lib = "RUSTLER"
        Anims(1403).Name = "Plane_open"
        Anims(1404)._Lib = "RYDER"
        Anims(1404).Name = "RYD_Beckon_01"
        Anims(1405)._Lib = "RYDER"
        Anims(1405).Name = "RYD_Beckon_02"
        Anims(1406)._Lib = "RYDER"
        Anims(1406).Name = "RYD_Beckon_03"
        Anims(1407)._Lib = "RYDER"
        Anims(1407).Name = "RYD_Die_PT1"
        Anims(1408)._Lib = "RYDER"
        Anims(1408).Name = "RYD_Die_PT2"
        Anims(1409)._Lib = "RYDER"
        Anims(1409).Name = "Van_Crate_L"
        Anims(1410)._Lib = "RYDER"
        Anims(1410).Name = "Van_Crate_R"
        Anims(1411)._Lib = "RYDER"
        Anims(1411).Name = "Van_Fall_L"
        Anims(1412)._Lib = "RYDER"
        Anims(1412).Name = "Van_Fall_R"
        Anims(1413)._Lib = "RYDER"
        Anims(1413).Name = "Van_Lean_L"
        Anims(1414)._Lib = "RYDER"
        Anims(1414).Name = "Van_Lean_R"
        Anims(1415)._Lib = "RYDER"
        Anims(1415).Name = "VAN_PickUp_E"
        Anims(1416)._Lib = "RYDER"
        Anims(1416).Name = "VAN_PickUp_S"
        Anims(1417)._Lib = "RYDER"
        Anims(1417).Name = "Van_Stand"
        Anims(1418)._Lib = "RYDER"
        Anims(1418).Name = "Van_Stand_Crate"
        Anims(1419)._Lib = "RYDER"
        Anims(1419).Name = "Van_Throw"
        Anims(1420)._Lib = "SCRATCHING"
        Anims(1420).Name = "scdldlp"
        Anims(1421)._Lib = "SCRATCHING"
        Anims(1421).Name = "scdlulp"
        Anims(1422)._Lib = "SCRATCHING"
        Anims(1422).Name = "scdrdlp"
        Anims(1423)._Lib = "SCRATCHING"
        Anims(1423).Name = "scdrulp"
        Anims(1424)._Lib = "SCRATCHING"
        Anims(1424).Name = "sclng_l"
        Anims(1425)._Lib = "SCRATCHING"
        Anims(1425).Name = "sclng_r"
        Anims(1426)._Lib = "SCRATCHING"
        Anims(1426).Name = "scmid_l"
        Anims(1427)._Lib = "SCRATCHING"
        Anims(1427).Name = "scmid_r"
        Anims(1428)._Lib = "SCRATCHING"
        Anims(1428).Name = "scshrtl"
        Anims(1429)._Lib = "SCRATCHING"
        Anims(1429).Name = "scshrtr"
        Anims(1430)._Lib = "SCRATCHING"
        Anims(1430).Name = "sc_ltor"
        Anims(1431)._Lib = "SCRATCHING"
        Anims(1431).Name = "sc_rtol"
        Anims(1432)._Lib = "SHAMAL"
        Anims(1432).Name = "SHAMAL_align"
        Anims(1433)._Lib = "SHAMAL"
        Anims(1433).Name = "SHAMAL_getin_LHS"
        Anims(1434)._Lib = "SHAMAL"
        Anims(1434).Name = "SHAMAL_getout_LHS"
        Anims(1435)._Lib = "SHAMAL"
        Anims(1435).Name = "SHAMAL_open"
        Anims(1436)._Lib = "SHOP"
        Anims(1436).Name = "ROB_2Idle"
        Anims(1437)._Lib = "SHOP"
        Anims(1437).Name = "ROB_Loop"
        Anims(1438)._Lib = "SHOP"
        Anims(1438).Name = "ROB_Loop_Threat"
        Anims(1439)._Lib = "SHOP"
        Anims(1439).Name = "ROB_Shifty"
        Anims(1440)._Lib = "SHOP"
        Anims(1440).Name = "ROB_StickUp_In"
        Anims(1441)._Lib = "SHOP"
        Anims(1441).Name = "SHP_Duck"
        Anims(1442)._Lib = "SHOP"
        Anims(1442).Name = "SHP_Duck_Aim"
        Anims(1443)._Lib = "SHOP"
        Anims(1443).Name = "SHP_Duck_Fire"
        Anims(1444)._Lib = "SHOP"
        Anims(1444).Name = "SHP_Gun_Aim"
        Anims(1445)._Lib = "SHOP"
        Anims(1445).Name = "SHP_Gun_Duck"
        Anims(1446)._Lib = "SHOP"
        Anims(1446).Name = "SHP_Gun_Fire"
        Anims(1447)._Lib = "SHOP"
        Anims(1447).Name = "SHP_Gun_Grab"
        Anims(1448)._Lib = "SHOP"
        Anims(1448).Name = "SHP_Gun_Threat"
        Anims(1449)._Lib = "SHOP"
        Anims(1449).Name = "SHP_HandsUp_Scr"
        Anims(1450)._Lib = "SHOP"
        Anims(1450).Name = "SHP_Jump_Glide"
        Anims(1451)._Lib = "SHOP"
        Anims(1451).Name = "SHP_Jump_Land"
        Anims(1452)._Lib = "SHOP"
        Anims(1452).Name = "SHP_Jump_Launch"
        Anims(1453)._Lib = "SHOP"
        Anims(1453).Name = "SHP_Rob_GiveCash"
        Anims(1454)._Lib = "SHOP"
        Anims(1454).Name = "SHP_Rob_HandsUp"
        Anims(1455)._Lib = "SHOP"
        Anims(1455).Name = "SHP_Rob_React"
        Anims(1456)._Lib = "SHOP"
        Anims(1456).Name = "SHP_Serve_End"
        Anims(1457)._Lib = "SHOP"
        Anims(1457).Name = "SHP_Serve_Idle"
        Anims(1458)._Lib = "SHOP"
        Anims(1458).Name = "SHP_Serve_Loop"
        Anims(1459)._Lib = "SHOP"
        Anims(1459).Name = "SHP_Serve_Start"
        Anims(1460)._Lib = "SHOP"
        Anims(1460).Name = "Smoke_RYD"
        Anims(1461)._Lib = "SHOTGUN"
        Anims(1461).Name = "shotgun_crouchfire"
        Anims(1462)._Lib = "SHOTGUN"
        Anims(1462).Name = "shotgun_fire"
        Anims(1463)._Lib = "SHOTGUN"
        Anims(1463).Name = "shotgun_fire_poor"
        Anims(1464)._Lib = "SILENCED"
        Anims(1464).Name = "CrouchReload"
        Anims(1465)._Lib = "SILENCED"
        Anims(1465).Name = "SilenceCrouchfire"
        Anims(1466)._Lib = "SILENCED"
        Anims(1466).Name = "Silence_fire"
        Anims(1467)._Lib = "SILENCED"
        Anims(1467).Name = "Silence_reload"
        Anims(1468)._Lib = "SKATE"
        Anims(1468).Name = "skate_idle"
        Anims(1469)._Lib = "SKATE"
        Anims(1469).Name = "skate_run"
        Anims(1470)._Lib = "SKATE"
        Anims(1470).Name = "skate_sprint"
        Anims(1471)._Lib = "SMOKING"
        Anims(1471).Name = "F_smklean_loop"
        Anims(1472)._Lib = "SMOKING"
        Anims(1472).Name = "M_smklean_loop"
        Anims(1473)._Lib = "SMOKING"
        Anims(1473).Name = "M_smkstnd_loop"
        Anims(1474)._Lib = "SMOKING"
        Anims(1474).Name = "M_smk_drag"
        Anims(1475)._Lib = "SMOKING"
        Anims(1475).Name = "M_smk_in"
        Anims(1476)._Lib = "SMOKING"
        Anims(1476).Name = "M_smk_loop"
        Anims(1477)._Lib = "SMOKING"
        Anims(1477).Name = "M_smk_out"
        Anims(1478)._Lib = "SMOKING"
        Anims(1478).Name = "M_smk_tap"
        Anims(1479)._Lib = "SNIPER"
        Anims(1479).Name = "WEAPON_sniper"
        Anims(1480)._Lib = "SPRAYCAN"
        Anims(1480).Name = "spraycan_fire"
        Anims(1481)._Lib = "SPRAYCAN"
        Anims(1481).Name = "spraycan_full"
        Anims(1482)._Lib = "STRIP"
        Anims(1482).Name = "PLY_CASH"
        Anims(1483)._Lib = "STRIP"
        Anims(1483).Name = "PUN_CASH"
        Anims(1484)._Lib = "STRIP"
        Anims(1484).Name = "PUN_HOLLER"
        Anims(1485)._Lib = "STRIP"
        Anims(1485).Name = "PUN_LOOP"
        Anims(1486)._Lib = "STRIP"
        Anims(1486).Name = "strip_A"
        Anims(1487)._Lib = "STRIP"
        Anims(1487).Name = "strip_B"
        Anims(1488)._Lib = "STRIP"
        Anims(1488).Name = "strip_C"
        Anims(1489)._Lib = "STRIP"
        Anims(1489).Name = "strip_D"
        Anims(1490)._Lib = "STRIP"
        Anims(1490).Name = "strip_E"
        Anims(1491)._Lib = "STRIP"
        Anims(1491).Name = "strip_F"
        Anims(1492)._Lib = "STRIP"
        Anims(1492).Name = "strip_G"
        Anims(1493)._Lib = "STRIP"
        Anims(1493).Name = "STR_A2B"
        Anims(1494)._Lib = "STRIP"
        Anims(1494).Name = "STR_B2A"
        Anims(1495)._Lib = "STRIP"
        Anims(1495).Name = "STR_B2C"
        Anims(1496)._Lib = "STRIP"
        Anims(1496).Name = "STR_C1"
        Anims(1497)._Lib = "STRIP"
        Anims(1497).Name = "STR_C2"
        Anims(1498)._Lib = "STRIP"
        Anims(1498).Name = "STR_C2B"
        Anims(1499)._Lib = "STRIP"
        Anims(1499).Name = "STR_Loop_A"
        Anims(1500)._Lib = "STRIP"
        Anims(1500).Name = "STR_Loop_B"
        Anims(1501)._Lib = "STRIP"
        Anims(1501).Name = "STR_Loop_C"
        Anims(1502)._Lib = "SUNBATHE"
        Anims(1502).Name = "batherdown"
        Anims(1503)._Lib = "SUNBATHE"
        Anims(1503).Name = "batherup"
        Anims(1504)._Lib = "SUNBATHE"
        Anims(1504).Name = "Lay_Bac_in"
        Anims(1505)._Lib = "SUNBATHE"
        Anims(1505).Name = "Lay_Bac_out"
        Anims(1506)._Lib = "SUNBATHE"
        Anims(1506).Name = "ParkSit_M_IdleA"
        Anims(1507)._Lib = "SUNBATHE"
        Anims(1507).Name = "ParkSit_M_IdleB"
        Anims(1508)._Lib = "SUNBATHE"
        Anims(1508).Name = "ParkSit_M_IdleC"
        Anims(1509)._Lib = "SUNBATHE"
        Anims(1509).Name = "ParkSit_M_in"
        Anims(1510)._Lib = "SUNBATHE"
        Anims(1510).Name = "ParkSit_M_out"
        Anims(1511)._Lib = "SUNBATHE"
        Anims(1511).Name = "ParkSit_W_idleA"
        Anims(1512)._Lib = "SUNBATHE"
        Anims(1512).Name = "ParkSit_W_idleB"
        Anims(1513)._Lib = "SUNBATHE"
        Anims(1513).Name = "ParkSit_W_idleC"
        Anims(1514)._Lib = "SUNBATHE"
        Anims(1514).Name = "ParkSit_W_in"
        Anims(1515)._Lib = "SUNBATHE"
        Anims(1515).Name = "ParkSit_W_out"
        Anims(1516)._Lib = "SUNBATHE"
        Anims(1516).Name = "SBATHE_F_LieB2Sit"
        Anims(1517)._Lib = "SUNBATHE"
        Anims(1517).Name = "SBATHE_F_Out"
        Anims(1518)._Lib = "SUNBATHE"
        Anims(1518).Name = "SitnWait_in_W"
        Anims(1519)._Lib = "SUNBATHE"
        Anims(1519).Name = "SitnWait_out_W"
        Anims(1520)._Lib = "SWAT"
        Anims(1520).Name = "gnstwall_injurd"
        Anims(1521)._Lib = "SWAT"
        Anims(1521).Name = "JMP_Wall1m_180"
        Anims(1522)._Lib = "SWAT"
        Anims(1522).Name = "Rail_fall"
        Anims(1523)._Lib = "SWAT"
        Anims(1523).Name = "Rail_fall_crawl"
        Anims(1524)._Lib = "SWAT"
        Anims(1524).Name = "swt_breach_01"
        Anims(1525)._Lib = "SWAT"
        Anims(1525).Name = "swt_breach_02"
        Anims(1526)._Lib = "SWAT"
        Anims(1526).Name = "swt_breach_03"
        Anims(1527)._Lib = "SWAT"
        Anims(1527).Name = "swt_go"
        Anims(1528)._Lib = "SWAT"
        Anims(1528).Name = "swt_lkt"
        Anims(1529)._Lib = "SWAT"
        Anims(1529).Name = "swt_sty"
        Anims(1530)._Lib = "SWAT"
        Anims(1530).Name = "swt_vent_01"
        Anims(1531)._Lib = "SWAT"
        Anims(1531).Name = "swt_vent_02"
        Anims(1532)._Lib = "SWAT"
        Anims(1532).Name = "swt_vnt_sht_die"
        Anims(1533)._Lib = "SWAT"
        Anims(1533).Name = "swt_vnt_sht_in"
        Anims(1534)._Lib = "SWAT"
        Anims(1534).Name = "swt_vnt_sht_loop"
        Anims(1535)._Lib = "SWAT"
        Anims(1535).Name = "swt_wllpk_L"
        Anims(1536)._Lib = "SWAT"
        Anims(1536).Name = "swt_wllpk_L_back"
        Anims(1537)._Lib = "SWAT"
        Anims(1537).Name = "swt_wllpk_R"
        Anims(1538)._Lib = "SWAT"
        Anims(1538).Name = "swt_wllpk_R_back"
        Anims(1539)._Lib = "SWAT"
        Anims(1539).Name = "swt_wllshoot_in_L"
        Anims(1540)._Lib = "SWAT"
        Anims(1540).Name = "swt_wllshoot_in_R"
        Anims(1541)._Lib = "SWAT"
        Anims(1541).Name = "swt_wllshoot_out_L"
        Anims(1542)._Lib = "SWAT"
        Anims(1542).Name = "swt_wllshoot_out_R"
        Anims(1543)._Lib = "SWEET"
        Anims(1543).Name = "ho_ass_slapped"
        Anims(1544)._Lib = "SWEET"
        Anims(1544).Name = "LaFin_Player"
        Anims(1545)._Lib = "SWEET"
        Anims(1545).Name = "LaFin_Sweet"
        Anims(1546)._Lib = "SWEET"
        Anims(1546).Name = "plyr_hndshldr_01"
        Anims(1547)._Lib = "SWEET"
        Anims(1547).Name = "sweet_ass_slap"
        Anims(1548)._Lib = "SWEET"
        Anims(1548).Name = "sweet_hndshldr_01"
        Anims(1549)._Lib = "SWEET"
        Anims(1549).Name = "Sweet_injuredloop"
        Anims(1550)._Lib = "SWIM"
        Anims(1550).Name = "Swim_Breast"
        Anims(1551)._Lib = "SWIM"
        Anims(1551).Name = "SWIM_crawl"
        Anims(1552)._Lib = "SWIM"
        Anims(1552).Name = "Swim_Dive_Under"
        Anims(1553)._Lib = "SWIM"
        Anims(1553).Name = "Swim_Glide"
        Anims(1554)._Lib = "SWIM"
        Anims(1554).Name = "Swim_jumpout"
        Anims(1555)._Lib = "SWIM"
        Anims(1555).Name = "Swim_Tread"
        Anims(1556)._Lib = "SWIM"
        Anims(1556).Name = "Swim_Under"
        Anims(1557)._Lib = "SWORD"
        Anims(1557).Name = "sword_1"
        Anims(1558)._Lib = "SWORD"
        Anims(1558).Name = "sword_2"
        Anims(1559)._Lib = "SWORD"
        Anims(1559).Name = "sword_3"
        Anims(1560)._Lib = "SWORD"
        Anims(1560).Name = "sword_4"
        Anims(1561)._Lib = "SWORD"
        Anims(1561).Name = "sword_block"
        Anims(1562)._Lib = "SWORD"
        Anims(1562).Name = "Sword_Hit_1"
        Anims(1563)._Lib = "SWORD"
        Anims(1563).Name = "Sword_Hit_2"
        Anims(1564)._Lib = "SWORD"
        Anims(1564).Name = "Sword_Hit_3"
        Anims(1565)._Lib = "SWORD"
        Anims(1565).Name = "sword_IDLE"
        Anims(1566)._Lib = "SWORD"
        Anims(1566).Name = "sword_part"
        Anims(1567)._Lib = "TANK"
        Anims(1567).Name = "TANK_align_LHS"
        Anims(1568)._Lib = "TANK"
        Anims(1568).Name = "TANK_close_LHS"
        Anims(1569)._Lib = "TANK"
        Anims(1569).Name = "TANK_doorlocked"
        Anims(1570)._Lib = "TANK"
        Anims(1570).Name = "TANK_getin_LHS"
        Anims(1571)._Lib = "TANK"
        Anims(1571).Name = "TANK_getout_LHS"
        Anims(1572)._Lib = "TANK"
        Anims(1572).Name = "TANK_open_LHS"
        Anims(1573)._Lib = "TATTOOS"
        Anims(1573).Name = "TAT_ArmL_In_O"
        Anims(1574)._Lib = "TATTOOS"
        Anims(1574).Name = "TAT_ArmL_In_P"
        Anims(1575)._Lib = "TATTOOS"
        Anims(1575).Name = "TAT_ArmL_In_T"
        Anims(1576)._Lib = "TATTOOS"
        Anims(1576).Name = "TAT_ArmL_Out_O"
        Anims(1577)._Lib = "TATTOOS"
        Anims(1577).Name = "TAT_ArmL_Out_P"
        Anims(1578)._Lib = "TATTOOS"
        Anims(1578).Name = "TAT_ArmL_Out_T"
        Anims(1579)._Lib = "TATTOOS"
        Anims(1579).Name = "TAT_ArmL_Pose_O"
        Anims(1580)._Lib = "TATTOOS"
        Anims(1580).Name = "TAT_ArmL_Pose_P"
        Anims(1581)._Lib = "TATTOOS"
        Anims(1581).Name = "TAT_ArmL_Pose_T"
        Anims(1582)._Lib = "TATTOOS"
        Anims(1582).Name = "TAT_ArmR_In_O"
        Anims(1583)._Lib = "TATTOOS"
        Anims(1583).Name = "TAT_ArmR_In_P"
        Anims(1584)._Lib = "TATTOOS"
        Anims(1584).Name = "TAT_ArmR_In_T"
        Anims(1585)._Lib = "TATTOOS"
        Anims(1585).Name = "TAT_ArmR_Out_O"
        Anims(1586)._Lib = "TATTOOS"
        Anims(1586).Name = "TAT_ArmR_Out_P"
        Anims(1587)._Lib = "TATTOOS"
        Anims(1587).Name = "TAT_ArmR_Out_T"
        Anims(1588)._Lib = "TATTOOS"
        Anims(1588).Name = "TAT_ArmR_Pose_O"
        Anims(1589)._Lib = "TATTOOS"
        Anims(1589).Name = "TAT_ArmR_Pose_P"
        Anims(1590)._Lib = "TATTOOS"
        Anims(1590).Name = "TAT_ArmR_Pose_T"
        Anims(1591)._Lib = "TATTOOS"
        Anims(1591).Name = "TAT_Back_In_O"
        Anims(1592)._Lib = "TATTOOS"
        Anims(1592).Name = "TAT_Back_In_P"
        Anims(1593)._Lib = "TATTOOS"
        Anims(1593).Name = "TAT_Back_In_T"
        Anims(1594)._Lib = "TATTOOS"
        Anims(1594).Name = "TAT_Back_Out_O"
        Anims(1595)._Lib = "TATTOOS"
        Anims(1595).Name = "TAT_Back_Out_P"
        Anims(1596)._Lib = "TATTOOS"
        Anims(1596).Name = "TAT_Back_Out_T"
        Anims(1597)._Lib = "TATTOOS"
        Anims(1597).Name = "TAT_Back_Pose_O"
        Anims(1598)._Lib = "TATTOOS"
        Anims(1598).Name = "TAT_Back_Pose_P"
        Anims(1599)._Lib = "TATTOOS"
        Anims(1599).Name = "TAT_Back_Pose_T"
        Anims(1600)._Lib = "TATTOOS"
        Anims(1600).Name = "TAT_Back_Sit_In_P"
        Anims(1601)._Lib = "TATTOOS"
        Anims(1601).Name = "TAT_Back_Sit_Loop_P"
        Anims(1602)._Lib = "TATTOOS"
        Anims(1602).Name = "TAT_Back_Sit_Out_P"
        Anims(1603)._Lib = "TATTOOS"
        Anims(1603).Name = "TAT_Bel_In_O"
        Anims(1604)._Lib = "TATTOOS"
        Anims(1604).Name = "TAT_Bel_In_T"
        Anims(1605)._Lib = "TATTOOS"
        Anims(1605).Name = "TAT_Bel_Out_O"
        Anims(1606)._Lib = "TATTOOS"
        Anims(1606).Name = "TAT_Bel_Out_T"
        Anims(1607)._Lib = "TATTOOS"
        Anims(1607).Name = "TAT_Bel_Pose_O"
        Anims(1608)._Lib = "TATTOOS"
        Anims(1608).Name = "TAT_Bel_Pose_T"
        Anims(1609)._Lib = "TATTOOS"
        Anims(1609).Name = "TAT_Che_In_O"
        Anims(1610)._Lib = "TATTOOS"
        Anims(1610).Name = "TAT_Che_In_P"
        Anims(1611)._Lib = "TATTOOS"
        Anims(1611).Name = "TAT_Che_In_T"
        Anims(1612)._Lib = "TATTOOS"
        Anims(1612).Name = "TAT_Che_Out_O"
        Anims(1613)._Lib = "TATTOOS"
        Anims(1613).Name = "TAT_Che_Out_P"
        Anims(1614)._Lib = "TATTOOS"
        Anims(1614).Name = "TAT_Che_Out_T"
        Anims(1615)._Lib = "TATTOOS"
        Anims(1615).Name = "TAT_Che_Pose_O"
        Anims(1616)._Lib = "TATTOOS"
        Anims(1616).Name = "TAT_Che_Pose_P"
        Anims(1617)._Lib = "TATTOOS"
        Anims(1617).Name = "TAT_Che_Pose_T"
        Anims(1618)._Lib = "TATTOOS"
        Anims(1618).Name = "TAT_Drop_O"
        Anims(1619)._Lib = "TATTOOS"
        Anims(1619).Name = "TAT_Idle_Loop_O"
        Anims(1620)._Lib = "TATTOOS"
        Anims(1620).Name = "TAT_Idle_Loop_T"
        Anims(1621)._Lib = "TATTOOS"
        Anims(1621).Name = "TAT_Sit_In_O"
        Anims(1622)._Lib = "TATTOOS"
        Anims(1622).Name = "TAT_Sit_In_P"
        Anims(1623)._Lib = "TATTOOS"
        Anims(1623).Name = "TAT_Sit_In_T"
        Anims(1624)._Lib = "TATTOOS"
        Anims(1624).Name = "TAT_Sit_Loop_O"
        Anims(1625)._Lib = "TATTOOS"
        Anims(1625).Name = "TAT_Sit_Loop_P"
        Anims(1626)._Lib = "TATTOOS"
        Anims(1626).Name = "TAT_Sit_Loop_T"
        Anims(1627)._Lib = "TATTOOS"
        Anims(1627).Name = "TAT_Sit_Out_O"
        Anims(1628)._Lib = "TATTOOS"
        Anims(1628).Name = "TAT_Sit_Out_P"
        Anims(1629)._Lib = "TATTOOS"
        Anims(1629).Name = "TAT_Sit_Out_T"
        Anims(1630)._Lib = "TEC"
        Anims(1630).Name = "TEC_crouchfire"
        Anims(1631)._Lib = "TEC"
        Anims(1631).Name = "TEC_crouchreload"
        Anims(1632)._Lib = "TEC"
        Anims(1632).Name = "TEC_fire"
        Anims(1633)._Lib = "TEC"
        Anims(1633).Name = "TEC_reload"
        Anims(1634)._Lib = "TRAIN"
        Anims(1634).Name = "tran_gtup"
        Anims(1635)._Lib = "TRAIN"
        Anims(1635).Name = "tran_hng"
        Anims(1636)._Lib = "TRAIN"
        Anims(1636).Name = "tran_ouch"
        Anims(1637)._Lib = "TRAIN"
        Anims(1637).Name = "tran_stmb"
        Anims(1638)._Lib = "TRUCK"
        Anims(1638).Name = "TRUCK_ALIGN_LHS"
        Anims(1639)._Lib = "TRUCK"
        Anims(1639).Name = "TRUCK_ALIGN_RHS"
        Anims(1640)._Lib = "TRUCK"
        Anims(1640).Name = "TRUCK_closedoor_LHS"
        Anims(1641)._Lib = "TRUCK"
        Anims(1641).Name = "TRUCK_closedoor_RHS"
        Anims(1642)._Lib = "TRUCK"
        Anims(1642).Name = "TRUCK_close_LHS"
        Anims(1643)._Lib = "TRUCK"
        Anims(1643).Name = "TRUCK_close_RHS"
        Anims(1644)._Lib = "TRUCK"
        Anims(1644).Name = "TRUCK_getin_LHS"
        Anims(1645)._Lib = "TRUCK"
        Anims(1645).Name = "TRUCK_getin_RHS"
        Anims(1646)._Lib = "TRUCK"
        Anims(1646).Name = "TRUCK_getout_LHS"
        Anims(1647)._Lib = "TRUCK"
        Anims(1647).Name = "TRUCK_getout_RHS"
        Anims(1648)._Lib = "TRUCK"
        Anims(1648).Name = "TRUCK_jackedLHS"
        Anims(1649)._Lib = "TRUCK"
        Anims(1649).Name = "TRUCK_jackedRHS"
        Anims(1650)._Lib = "TRUCK"
        Anims(1650).Name = "TRUCK_open_LHS"
        Anims(1651)._Lib = "TRUCK"
        Anims(1651).Name = "TRUCK_open_RHS"
        Anims(1652)._Lib = "TRUCK"
        Anims(1652).Name = "TRUCK_pullout_LHS"
        Anims(1653)._Lib = "TRUCK"
        Anims(1653).Name = "TRUCK_pullout_RHS"
        Anims(1654)._Lib = "TRUCK"
        Anims(1654).Name = "TRUCK_Shuffle"
        Anims(1655)._Lib = "UZI"
        Anims(1655).Name = "UZI_crouchfire"
        Anims(1656)._Lib = "UZI"
        Anims(1656).Name = "UZI_crouchreload"
        Anims(1657)._Lib = "UZI"
        Anims(1657).Name = "UZI_fire"
        Anims(1658)._Lib = "UZI"
        Anims(1658).Name = "UZI_fire_poor"
        Anims(1659)._Lib = "UZI"
        Anims(1659).Name = "UZI_reload"
        Anims(1660)._Lib = "VAN"
        Anims(1660).Name = "VAN_close_back_LHS"
        Anims(1661)._Lib = "VAN"
        Anims(1661).Name = "VAN_close_back_RHS"
        Anims(1662)._Lib = "VAN"
        Anims(1662).Name = "VAN_getin_Back_LHS"
        Anims(1663)._Lib = "VAN"
        Anims(1663).Name = "VAN_getin_Back_RHS"
        Anims(1664)._Lib = "VAN"
        Anims(1664).Name = "VAN_getout_back_LHS"
        Anims(1665)._Lib = "VAN"
        Anims(1665).Name = "VAN_getout_back_RHS"
        Anims(1666)._Lib = "VAN"
        Anims(1666).Name = "VAN_open_back_LHS"
        Anims(1667)._Lib = "VAN"
        Anims(1667).Name = "VAN_open_back_RHS"
        Anims(1668)._Lib = "VENDING"
        Anims(1668).Name = "VEND_Drink2_P"
        Anims(1669)._Lib = "VENDING"
        Anims(1669).Name = "VEND_Drink_P"
        Anims(1670)._Lib = "VENDING"
        Anims(1670).Name = "vend_eat1_P"
        Anims(1671)._Lib = "VENDING"
        Anims(1671).Name = "VEND_Eat_P"
        Anims(1672)._Lib = "VENDING"
        Anims(1672).Name = "VEND_Use"
        Anims(1673)._Lib = "VENDING"
        Anims(1673).Name = "VEND_Use_pt2"
        Anims(1674)._Lib = "VORTEX"
        Anims(1674).Name = "CAR_jumpin_LHS"
        Anims(1675)._Lib = "VORTEX"
        Anims(1675).Name = "CAR_jumpin_RHS"
        Anims(1676)._Lib = "VORTEX"
        Anims(1676).Name = "vortex_getout_LHS"
        Anims(1677)._Lib = "VORTEX"
        Anims(1677).Name = "vortex_getout_RHS"
        Anims(1678)._Lib = "WAYFARER"
        Anims(1678).Name = "WF_Back"
        Anims(1679)._Lib = "WAYFARER"
        Anims(1679).Name = "WF_drivebyFT"
        Anims(1680)._Lib = "WAYFARER"
        Anims(1680).Name = "WF_drivebyLHS"
        Anims(1681)._Lib = "WAYFARER"
        Anims(1681).Name = "WF_drivebyRHS"
        Anims(1682)._Lib = "WAYFARER"
        Anims(1682).Name = "WF_Fwd"
        Anims(1683)._Lib = "WAYFARER"
        Anims(1683).Name = "WF_getoffBACK"
        Anims(1684)._Lib = "WAYFARER"
        Anims(1684).Name = "WF_getoffLHS"
        Anims(1685)._Lib = "WAYFARER"
        Anims(1685).Name = "WF_getoffRHS"
        Anims(1686)._Lib = "WAYFARER"
        Anims(1686).Name = "WF_hit"
        Anims(1687)._Lib = "WAYFARER"
        Anims(1687).Name = "WF_jumponL"
        Anims(1688)._Lib = "WAYFARER"
        Anims(1688).Name = "WF_jumponR"
        Anims(1689)._Lib = "WAYFARER"
        Anims(1689).Name = "WF_kick"
        Anims(1690)._Lib = "WAYFARER"
        Anims(1690).Name = "WF_Left"
        Anims(1691)._Lib = "WAYFARER"
        Anims(1691).Name = "WF_passenger"
        Anims(1692)._Lib = "WAYFARER"
        Anims(1692).Name = "WF_pushes"
        Anims(1693)._Lib = "WAYFARER"
        Anims(1693).Name = "WF_Ride"
        Anims(1694)._Lib = "WAYFARER"
        Anims(1694).Name = "WF_Right"
        Anims(1695)._Lib = "WAYFARER"
        Anims(1695).Name = "WF_Still"
        Anims(1696)._Lib = "WEAPONS"
        Anims(1696).Name = "SHP_1H_Lift"
        Anims(1697)._Lib = "WEAPONS"
        Anims(1697).Name = "SHP_1H_Lift_End"
        Anims(1698)._Lib = "WEAPONS"
        Anims(1698).Name = "SHP_1H_Ret"
        Anims(1699)._Lib = "WEAPONS"
        Anims(1699).Name = "SHP_1H_Ret_S"
        Anims(1700)._Lib = "WEAPONS"
        Anims(1700).Name = "SHP_2H_Lift"
        Anims(1701)._Lib = "WEAPONS"
        Anims(1701).Name = "SHP_2H_Lift_End"
        Anims(1702)._Lib = "WEAPONS"
        Anims(1702).Name = "SHP_2H_Ret"
        Anims(1703)._Lib = "WEAPONS"
        Anims(1703).Name = "SHP_2H_Ret_S"
        Anims(1704)._Lib = "WEAPONS"
        Anims(1704).Name = "SHP_Ar_Lift"
        Anims(1705)._Lib = "WEAPONS"
        Anims(1705).Name = "SHP_Ar_Lift_End"
        Anims(1706)._Lib = "WEAPONS"
        Anims(1706).Name = "SHP_Ar_Ret"
        Anims(1707)._Lib = "WEAPONS"
        Anims(1707).Name = "SHP_Ar_Ret_S"
        Anims(1708)._Lib = "WEAPONS"
        Anims(1708).Name = "SHP_G_Lift_In"
        Anims(1709)._Lib = "WEAPONS"
        Anims(1709).Name = "SHP_G_Lift_Out"
        Anims(1710)._Lib = "WEAPONS"
        Anims(1710).Name = "SHP_Tray_In"
        Anims(1711)._Lib = "WEAPONS"
        Anims(1711).Name = "SHP_Tray_Out"
        Anims(1712)._Lib = "WEAPONS"
        Anims(1712).Name = "SHP_Tray_Pose"
        Anims(1713)._Lib = "WUZI"
        Anims(1713).Name = "CS_Dead_Guy"
        Anims(1714)._Lib = "WUZI"
        Anims(1714).Name = "CS_Plyr_pt1"
        Anims(1715)._Lib = "WUZI"
        Anims(1715).Name = "CS_Plyr_pt2"
        Anims(1716)._Lib = "WUZI"
        Anims(1716).Name = "CS_Wuzi_pt1"
        Anims(1717)._Lib = "WUZI"
        Anims(1717).Name = "CS_Wuzi_pt2"
        Anims(1718)._Lib = "WUZI"
        Anims(1718).Name = "Walkstart_Idle_01"
        Anims(1719)._Lib = "WUZI"
        Anims(1719).Name = "Wuzi_follow"
        Anims(1720)._Lib = "WUZI"
        Anims(1720).Name = "Wuzi_Greet_Plyr"
        Anims(1721)._Lib = "WUZI"
        Anims(1721).Name = "Wuzi_Greet_Wuzi"
        Anims(1722)._Lib = "WUZI"
        Anims(1722).Name = "Wuzi_grnd_chk"
        Anims(1723)._Lib = "WUZI"
        Anims(1723).Name = "Wuzi_stand_loop"
        Anims(1724)._Lib = "WUZI"
        Anims(1724).Name = "Wuzi_Walk"
    End Sub

#End Region

#Region "Weapons"

    Public Sub FillWeapons()
        FillWeaponsList()
        Main.TreeView6.Nodes.Add("None")
        Main.TreeView6.Nodes.Add("White")
        Main.TreeView6.Nodes.Add("Pistols")
        Main.TreeView6.Nodes.Add("Shotguns")
        Main.TreeView6.Nodes.Add("Sub Machine")
        Main.TreeView6.Nodes.Add("SMG")
        Main.TreeView6.Nodes.Add("Assault")
        Main.TreeView6.Nodes.Add("Rifles")
        Main.TreeView6.Nodes.Add("Special")
        Main.TreeView6.Nodes.Add("Thrown")
        Main.TreeView6.Nodes.Add("Other")
        Main.TreeView6.Nodes.Add("All")
        Dim count As Integer
        For Each weapon As Weap In Weapons
            If Not weapon.Img Is Nothing Then
                Select Case weapon.Type
                    Case WeaponType.Assault
                        Main.TreeView6.Nodes(6).Nodes.Add(weapon.Name)
                    Case WeaponType.None
                        Main.TreeView6.Nodes(0).Nodes.Add(weapon.Name)
                    Case WeaponType.Other
                        Main.TreeView6.Nodes(10).Nodes.Add(weapon.Name)
                    Case WeaponType.Pistol
                        Main.TreeView6.Nodes(2).Nodes.Add(weapon.Name)
                    Case WeaponType.Rifle
                        Main.TreeView6.Nodes(7).Nodes.Add(weapon.Name)
                    Case WeaponType.Shotgun
                        Main.TreeView6.Nodes(3).Nodes.Add(weapon.Name)
                    Case WeaponType.SMG
                        Main.TreeView6.Nodes(5).Nodes.Add(weapon.Name)
                    Case WeaponType.Special
                        Main.TreeView6.Nodes(8).Nodes.Add(weapon.Name)
                    Case WeaponType.SubMachine
                        Main.TreeView6.Nodes(4).Nodes.Add(weapon.Name)
                    Case WeaponType.Thrown
                        Main.TreeView6.Nodes(9).Nodes.Add(weapon.Name)
                    Case WeaponType.White
                        Main.TreeView6.Nodes(1).Nodes.Add(weapon.Name)
                End Select
                Main.TreeView6.Nodes(11).Nodes.Add(weapon.Name)
                Splash.Label1.Text = String.Format("Loading weapons ({0}/213)", count)
                Splash.ProgressBar1.Value += 1
                count += 1
            End If
        Next
    End Sub

    Private Sub FillWeaponsList()
        Splash.Label1.Text = "Loading weapons..."
        Weapons(0).ID = 0
        Weapons(0).Name = "Unarmed"
        Weapons(0).Img = My.Resources.Weapon_0
        Weapons(0).Slot = 0
        Weapons(0).Type = WeaponType.None
        Weapons(0).Def = "-"
        Weapons(1).ID = 1
        Weapons(1).Name = "Brass Knuckles"
        Weapons(1).Img = My.Resources.Weapon_1
        Weapons(1).Slot = 0
        Weapons(1).Type = WeaponType.None
        Weapons(1).Def = "WEAPON_BRASSKNUCKLE"
        Weapons(2).ID = 2
        Weapons(2).Name = "Golf"
        Weapons(2).Img = My.Resources.Weapon_2
        Weapons(2).Slot = 1
        Weapons(2).Type = WeaponType.White
        Weapons(2).Def = "WEAPON_GOLFCLUB"
        Weapons(3).ID = 3
        Weapons(3).Name = "Nite Stick"
        Weapons(3).Img = My.Resources.Weapon_3
        Weapons(3).Slot = 1
        Weapons(3).Type = WeaponType.White
        Weapons(3).Def = "WEAPON_NITESTICK"
        Weapons(4).ID = 4
        Weapons(4).Name = "Knife"
        Weapons(4).Img = My.Resources.Weapon_4
        Weapons(4).Slot = 1
        Weapons(4).Type = WeaponType.White
        Weapons(4).Def = "WEAPON_KNIFE"
        Weapons(5).ID = 5
        Weapons(5).Name = "Baseball Bat"
        Weapons(5).Img = My.Resources.Weapon_5
        Weapons(5).Slot = 1
        Weapons(5).Type = WeaponType.White
        Weapons(5).Def = "WEAPON_BAT"
        Weapons(6).ID = 6
        Weapons(6).Name = "Shovel"
        Weapons(6).Img = My.Resources.Weapon_6
        Weapons(6).Slot = 1
        Weapons(6).Type = WeaponType.White
        Weapons(6).Def = "WEAPON_SHOVEL"
        Weapons(7).ID = 7
        Weapons(7).Name = "Pool Cue"
        Weapons(7).Img = My.Resources.Weapon_7
        Weapons(7).Slot = 1
        Weapons(7).Type = WeaponType.White
        Weapons(7).Def = "WEAPON_POOLSTICK"
        Weapons(8).ID = 8
        Weapons(8).Name = "Katana"
        Weapons(8).Img = My.Resources.Weapon_8
        Weapons(8).Slot = 1
        Weapons(8).Type = WeaponType.White
        Weapons(8).Def = "WEAPON_KATANA"
        Weapons(9).ID = 9
        Weapons(9).Name = "Chainsaw"
        Weapons(9).Img = My.Resources.Weapon_9
        Weapons(9).Slot = 1
        Weapons(9).Type = WeaponType.White
        Weapons(9).Def = "WEAPON_CHAINSAW"
        Weapons(10).ID = 10
        Weapons(10).Name = "Purple Dildo"
        Weapons(10).Img = My.Resources.Weapon_10
        Weapons(10).Slot = 10
        Weapons(10).Type = WeaponType.White
        Weapons(10).Def = "WEAPON_DILDO"
        Weapons(11).ID = 11
        Weapons(11).Name = "Small White Vibrator"
        Weapons(11).Img = My.Resources.Weapon_11
        Weapons(11).Slot = 10
        Weapons(11).Type = WeaponType.White
        Weapons(11).Def = "WEAPON_DILDO2"
        Weapons(12).ID = 12
        Weapons(12).Name = "Large White Vibrator"
        Weapons(12).Img = My.Resources.Weapon_12
        Weapons(12).Slot = 10
        Weapons(12).Type = WeaponType.White
        Weapons(12).Def = "WEAPON_VIBRATOR"
        Weapons(13).ID = 13
        Weapons(13).Name = "Silver Vibrator"
        Weapons(13).Img = My.Resources.Weapon_13
        Weapons(13).Slot = 10
        Weapons(13).Type = WeaponType.White
        Weapons(13).Def = "WEAPON_VIBRATOR2"
        Weapons(14).ID = 14
        Weapons(14).Name = "Flowers"
        Weapons(14).Img = My.Resources.Weapon_14
        Weapons(14).Slot = 10
        Weapons(14).Type = WeaponType.White
        Weapons(14).Def = "WEAPON_FLOWER"
        Weapons(15).ID = 15
        Weapons(15).Name = "Cane"
        Weapons(15).Img = My.Resources.Weapon_15
        Weapons(15).Slot = 10
        Weapons(15).Type = WeaponType.White
        Weapons(15).Def = "WEAPON_CANE"
        Weapons(16).ID = 16
        Weapons(16).Name = "Grenade"
        Weapons(16).Img = My.Resources.Weapon_16
        Weapons(16).Slot = 8
        Weapons(16).Type = WeaponType.Thrown
        Weapons(16).Def = "WEAPON_GRENADE"
        Weapons(17).ID = 17
        Weapons(17).Name = "Tear Gas"
        Weapons(17).Img = My.Resources.Weapon_17
        Weapons(17).Slot = 8
        Weapons(17).Type = WeaponType.Thrown
        Weapons(17).Def = "WEAPON_TEARGAS"
        Weapons(18).ID = 18
        Weapons(18).Name = "Molotov Cocktail"
        Weapons(18).Img = My.Resources.Weapon_18
        Weapons(18).Slot = 8
        Weapons(18).Type = WeaponType.Thrown
        Weapons(18).Def = "WEAPON_MOLTOV"
        Weapons(19).ID = 22
        Weapons(19).Name = "9mm"
        Weapons(19).Img = My.Resources.Weapon_22
        Weapons(19).Slot = 2
        Weapons(19).Type = WeaponType.Pistol
        Weapons(19).Def = "WEAPON_COLT45"
        Weapons(20).ID = 23
        Weapons(20).Name = "Silenced 9mm"
        Weapons(20).Img = My.Resources.Weapon_23
        Weapons(20).Slot = 2
        Weapons(20).Type = WeaponType.Pistol
        Weapons(20).Def = "WEAPON_SILENCED"
        Weapons(21).ID = 24
        Weapons(21).Name = "Desert Eagle"
        Weapons(21).Img = My.Resources.Weapon_24
        Weapons(21).Slot = 2
        Weapons(21).Type = WeaponType.Pistol
        Weapons(21).Def = "WEAPON_DEAGLE"
        Weapons(22).ID = 25
        Weapons(22).Name = "Shotgun"
        Weapons(22).Img = My.Resources.Weapon_25
        Weapons(22).Slot = 3
        Weapons(22).Type = WeaponType.Shotgun
        Weapons(22).Def = "WEAPON_SHOTGUN"
        Weapons(23).ID = 26
        Weapons(23).Name = "Sawn-off Shotgun"
        Weapons(23).Img = My.Resources.Weapon_26
        Weapons(23).Slot = 3
        Weapons(23).Type = WeaponType.Shotgun
        Weapons(23).Def = "WEAPON_SAWEDOFF"
        Weapons(24).ID = 27
        Weapons(24).Name = "Combat Shotgun"
        Weapons(24).Img = My.Resources.Weapon_27
        Weapons(24).Slot = 3
        Weapons(24).Type = WeaponType.Shotgun
        Weapons(24).Def = "WEAPON_SHOTGSPA"
        Weapons(25).ID = 28
        Weapons(25).Name = "Micro SMG"
        Weapons(25).Img = My.Resources.Weapon_28
        Weapons(25).Slot = 4
        Weapons(25).Type = WeaponType.SubMachine
        Weapons(25).Def = "WEAPON_UZI"
        Weapons(26).ID = 29
        Weapons(26).Name = "MP5"
        Weapons(26).Img = My.Resources.Weapon_29
        Weapons(26).Slot = 4
        Weapons(26).Type = WeaponType.SMG
        Weapons(26).Def = "WEAPON_MP5"
        Weapons(27).ID = 30
        Weapons(27).Name = "Ak-47"
        Weapons(27).Img = My.Resources.Weapon_30
        Weapons(27).Slot = 5
        Weapons(27).Type = WeaponType.Assault
        Weapons(27).Def = "WEAPON_AK47"
        Weapons(28).ID = 31
        Weapons(28).Name = "M4"
        Weapons(28).Img = My.Resources.Weapon_31
        Weapons(28).Slot = 5
        Weapons(28).Type = WeaponType.Assault
        Weapons(28).Def = "WEAPON_M4"
        Weapons(29).ID = 32
        Weapons(29).Name = "Tec-9"
        Weapons(29).Img = My.Resources.Weapon_32
        Weapons(29).Slot = 4
        Weapons(29).Type = WeaponType.SubMachine
        Weapons(29).Def = "WEAPON_TEC9"
        Weapons(30).ID = 33
        Weapons(30).Name = "Country Rifle"
        Weapons(30).Img = My.Resources.Weapon_33
        Weapons(30).Slot = 6
        Weapons(30).Type = WeaponType.Rifle
        Weapons(30).Def = "WEAPON_RIFLE"
        Weapons(31).ID = 34
        Weapons(31).Name = "Sniper Rifle"
        Weapons(31).Img = My.Resources.Weapon_34
        Weapons(31).Slot = 6
        Weapons(31).Type = WeaponType.Rifle
        Weapons(31).Def = "WEAPON_SNIPER"
        Weapons(32).ID = 35
        Weapons(32).Name = "Rocket Launcher"
        Weapons(32).Img = My.Resources.Weapon_35
        Weapons(32).Slot = 7
        Weapons(32).Type = WeaponType.Special
        Weapons(32).Def = "WEAPON_ROCKETLAUNCHER"
        Weapons(33).ID = 36
        Weapons(33).Name = "HS Rocket Launcher"
        Weapons(33).Img = My.Resources.Weapon_36
        Weapons(33).Slot = 7
        Weapons(33).Type = WeaponType.Special
        Weapons(33).Def = "WEAPON_HEATSEEKER"
        Weapons(34).ID = 37
        Weapons(34).Name = "Flamethrower"
        Weapons(34).Img = My.Resources.Weapon_37
        Weapons(34).Slot = 7
        Weapons(34).Type = WeaponType.Special
        Weapons(34).Def = "WEAPON_FLAMETHROWER"
        Weapons(35).ID = 38
        Weapons(35).Name = "Minigun"
        Weapons(35).Img = My.Resources.Weapon_38
        Weapons(35).Slot = 7
        Weapons(35).Type = WeaponType.Special
        Weapons(35).Def = "WEAPON_MINIGUN"
        Weapons(36).ID = 39
        Weapons(36).Name = "Satchel Charge"
        Weapons(36).Img = My.Resources.Weapon_39
        Weapons(36).Slot = 8
        Weapons(36).Type = WeaponType.Thrown
        Weapons(36).Def = "WEAPON_SATCHEL"
        Weapons(37).ID = 40
        Weapons(37).Name = "Detonator"
        Weapons(37).Img = My.Resources.Weapon_40
        Weapons(37).Slot = 12
        Weapons(37).Type = WeaponType.Special
        Weapons(37).Def = "WEAPON_BOMB"
        Weapons(38).ID = 41
        Weapons(38).Name = "Spaycan"
        Weapons(38).Img = My.Resources.Weapon_41
        Weapons(38).Slot = 9
        Weapons(38).Type = WeaponType.Other
        Weapons(38).Def = "WEAPON_SPRAYCAN"
        Weapons(39).ID = 42
        Weapons(39).Name = "Fire Extinguisher"
        Weapons(39).Img = My.Resources.Weapon_42
        Weapons(39).Slot = 9
        Weapons(39).Type = WeaponType.Other
        Weapons(39).Def = "WEAPON_FIREEXTINGUISHER"
        Weapons(40).ID = 43
        Weapons(40).Name = "Camera"
        Weapons(40).Img = My.Resources.Weapon_43
        Weapons(40).Slot = 9
        Weapons(40).Type = WeaponType.Other
        Weapons(40).Def = "WEAPON_CAMERA"
        Weapons(41).ID = 44
        Weapons(41).Name = "Nightvision Googles"
        Weapons(41).Img = My.Resources.Weapon_44
        Weapons(41).Slot = 11
        Weapons(41).Type = WeaponType.Other
        Weapons(41).Def = "-"
        Weapons(42).ID = 45
        Weapons(42).Name = "Thermal Googles"
        Weapons(42).Img = My.Resources.Weapon_45
        Weapons(42).Slot = 11
        Weapons(42).Type = WeaponType.Other
        Weapons(42).Def = "-"
        Weapons(43).ID = 46
        Weapons(43).Name = "Parachute"
        Weapons(43).Img = My.Resources.Weapon_46
        Weapons(43).Slot = 11
        Weapons(43).Type = WeaponType.Other
        Weapons(43).Def = "WEAPON_PARACHUTE"
    End Sub

#End Region

#Region "Map Icons"

    Public Sub FillMapIcons()
        FillMapIconsList()
        Dim count As Integer
        For Each map In Maps
            If map.ID <> 0 Then
                Main.TreeView7.Nodes.Add(map.ID)
                Splash.Label1.Text = String.Format("Loading map icons ({0}/65)", count)
                Splash.ProgressBar1.Value += 1
                count += 1
            End If
        Next
    End Sub

    Private Sub FillMapIconsList()
        Splash.Label1.Text = "Loading map icons..."
        Maps(0).ID = 1
        Maps(0).Name = "White Square"
        Maps(0).Img = My.Resources.MapIcon_1
        Maps(1).ID = 2
        Maps(1).Name = "Player Position"
        Maps(1).Img = My.Resources.MapIcon_2
        Maps(2).ID = 3
        Maps(2).Name = "Player (Menu Map)"
        Maps(2).Img = My.Resources.MapIcon_3
        Maps(3).ID = 4
        Maps(3).Name = "North"
        Maps(3).Img = My.Resources.MapIcon_4
        Maps(4).ID = 5
        Maps(4).Name = "Air Yard"
        Maps(4).Img = My.Resources.MapIcon_5
        Maps(5).ID = 6
        Maps(5).Name = "Ammunation"
        Maps(5).Img = My.Resources.MapIcon_6
        Maps(6).ID = 7
        Maps(6).Name = "Barber"
        Maps(6).Img = My.Resources.MapIcon_7
        Maps(7).ID = 8
        Maps(7).Name = "Big Smoke"
        Maps(7).Img = My.Resources.MapIcon_8
        Maps(8).ID = 9
        Maps(8).Name = "Boat Yard"
        Maps(8).Img = My.Resources.MapIcon_9
        Maps(9).ID = 10
        Maps(9).Name = "Burger Shot"
        Maps(9).Img = My.Resources.MapIcon_10
        Maps(10).ID = 11
        Maps(10).Name = "Quarry"
        Maps(10).Img = My.Resources.MapIcon_11
        Maps(11).ID = 12
        Maps(11).Name = "Catalina"
        Maps(11).Img = My.Resources.MapIcon_12
        Maps(12).ID = 13
        Maps(12).Name = "Cesar"
        Maps(12).Img = My.Resources.MapIcon_13
        Maps(13).ID = 14
        Maps(13).Name = "Cluckin' Bell"
        Maps(13).Img = My.Resources.MapIcon_14
        Maps(14).ID = 15
        Maps(14).Name = "Carl Johnson"
        Maps(14).Img = My.Resources.MapIcon_15
        Maps(15).ID = 16
        Maps(15).Name = "C.R.A.S.H"
        Maps(15).Img = My.Resources.MapIcon_16
        Maps(16).ID = 17
        Maps(16).Name = "Diner"
        Maps(16).Img = My.Resources.MapIcon_17
        Maps(17).ID = 18
        Maps(17).Name = "Emmet"
        Maps(17).Img = My.Resources.MapIcon_18
        Maps(18).ID = 19
        Maps(18).Name = "Enemy Attack"
        Maps(18).Img = My.Resources.MapIcon_19
        Maps(19).ID = 20
        Maps(19).Name = "Fire"
        Maps(19).Img = My.Resources.MapIcon_20
        Maps(20).ID = 21
        Maps(20).Name = "Girlfriend"
        Maps(20).Img = My.Resources.MapIcon_21
        Maps(21).ID = 22
        Maps(21).Name = "Hospital"
        Maps(21).Img = My.Resources.MapIcon_22
        Maps(22).ID = 23
        Maps(22).Name = "Loco"
        Maps(22).Img = My.Resources.MapIcon_23
        Maps(23).ID = 24
        Maps(23).Name = "Madd Dogg"
        Maps(23).Img = My.Resources.MapIcon_24
        Maps(24).ID = 25
        Maps(24).Name = "Caligulas"
        Maps(24).Img = My.Resources.MapIcon_25
        Maps(25).ID = 26
        Maps(25).Name = "OG Loc"
        Maps(25).Img = My.Resources.MapIcon_26
        Maps(26).ID = 27
        Maps(26).Name = "Mod garage"
        Maps(26).Img = My.Resources.MapIcon_27
        Maps(27).ID = 28
        Maps(27).Name = "OG Loc"
        Maps(27).Img = My.Resources.MapIcon_28
        Maps(28).ID = 29
        Maps(28).Name = "Well Stacked Pizza Co"
        Maps(28).Img = My.Resources.MapIcon_29
        Maps(29).ID = 30
        Maps(29).Name = "Police"
        Maps(29).Img = My.Resources.MapIcon_30
        Maps(30).ID = 31
        Maps(30).Name = "Property"
        Maps(30).Img = My.Resources.MapIcon_31
        Maps(31).ID = 32
        Maps(31).Name = "Property"
        Maps(31).Img = My.Resources.MapIcon_32
        Maps(32).ID = 33
        Maps(32).Name = "Race"
        Maps(32).Img = My.Resources.MapIcon_33
        Maps(33).ID = 34
        Maps(33).Name = "Ryder"
        Maps(33).Img = My.Resources.MapIcon_34
        Maps(34).ID = 35
        Maps(34).Name = "Save Game"
        Maps(34).Img = My.Resources.MapIcon_35
        Maps(35).ID = 36
        Maps(35).Name = "School"
        Maps(35).Img = My.Resources.MapIcon_36
        Maps(36).ID = 37
        Maps(36).Name = "Unknown"
        Maps(36).Img = My.Resources.MapIcon_37
        Maps(37).ID = 38
        Maps(37).Name = "Sweet"
        Maps(37).Img = My.Resources.MapIcon_38
        Maps(38).ID = 39
        Maps(38).Name = "Tattoo"
        Maps(38).Img = My.Resources.MapIcon_39
        Maps(39).ID = 40
        Maps(39).Name = "The Truth"
        Maps(39).Img = My.Resources.MapIcon_40
        Maps(40).ID = 41
        Maps(40).Name = "Waypoint"
        Maps(40).Img = My.Resources.MapIcon_41
        Maps(41).ID = 42
        Maps(41).Name = "Toreno"
        Maps(41).Img = My.Resources.MapIcon_42
        Maps(42).ID = 43
        Maps(42).Name = "Triads"
        Maps(42).Img = My.Resources.MapIcon_43
        Maps(43).ID = 44
        Maps(43).Name = "Triads Casino"
        Maps(43).Img = My.Resources.MapIcon_44
        Maps(44).ID = 45
        Maps(44).Name = "Clothes"
        Maps(44).Img = My.Resources.MapIcon_45
        Maps(45).ID = 46
        Maps(45).Name = "Woozie"
        Maps(45).Img = My.Resources.MapIcon_46
        Maps(46).ID = 47
        Maps(46).Name = "Zero"
        Maps(46).Img = My.Resources.MapIcon_47
        Maps(47).ID = 48
        Maps(47).Name = "Disco"
        Maps(47).Img = My.Resources.MapIcon_48
        Maps(48).ID = 49
        Maps(48).Name = "Bar"
        Maps(48).Img = My.Resources.MapIcon_49
        Maps(49).ID = 50
        Maps(49).Name = "Restaurant"
        Maps(49).Img = My.Resources.MapIcon_50
        Maps(50).ID = 51
        Maps(50).Name = "Truck"
        Maps(50).Img = My.Resources.MapIcon_51
        Maps(51).ID = 52
        Maps(51).Name = "Robbery"
        Maps(51).Img = My.Resources.MapIcon_52
        Maps(52).ID = 53
        Maps(52).Name = "Race"
        Maps(52).Img = My.Resources.MapIcon_53
        Maps(53).ID = 54
        Maps(53).Name = "Gym"
        Maps(53).Img = My.Resources.MapIcon_54
        Maps(54).ID = 55
        Maps(54).Name = "Car"
        Maps(54).Img = My.Resources.MapIcon_55
        Maps(55).ID = 56
        Maps(55).Name = "Light"
        Maps(55).Img = My.Resources.MapIcon_56
        Maps(56).ID = 57
        Maps(56).Name = "Closest airport"
        Maps(56).Img = My.Resources.MapIcon_57
        Maps(57).ID = 58
        Maps(57).Name = "Varrios Los Aztecas"
        Maps(57).Img = My.Resources.MapIcon_58
        Maps(58).ID = 59
        Maps(58).Name = "Ballas"
        Maps(58).Img = My.Resources.MapIcon_59
        Maps(59).ID = 60
        Maps(59).Name = "Los Santos Vagos"
        Maps(59).Img = My.Resources.MapIcon_60
        Maps(60).ID = 61
        Maps(60).Name = "San Fierro Rifa"
        Maps(60).Img = My.Resources.MapIcon_61
        Maps(61).ID = 62
        Maps(61).Name = "Grove street"
        Maps(61).Img = My.Resources.MapIcon_62
        Maps(62).ID = 63
        Maps(62).Name = "Pay 'n' Spray"
        Maps(62).Img = My.Resources.MapIcon_63
    End Sub

#End Region

#Region "Images"

    Public Function ScaleImage(ByVal Img As Image, ByVal percent As Double, ByVal ss As ScaleType)
        Dim Width As Integer, Height As Integer, BitX As Bitmap
        If ss = ScaleType.Down Then
            Width = Img.Width - Img.Width * (percent / 100)
            Height = Img.Height - Img.Height * (percent / 100)
            BitX = New Bitmap(Width, Height)
            Dim Graph As Graphics = Graphics.FromImage(BitX)
            With Graph
                .InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
                .DrawImage(Img, New Rectangle(0, 0, Width, Height), New Rectangle(0, 0, Img.Width, Img.Height), GraphicsUnit.Pixel)
            End With
        Else
            Width = Img.Width + Img.Width * (percent / 100)
            Height = Img.Height + Img.Height * (percent / 100)
            BitX = New Bitmap(Width, Height)
            Dim Graph As Graphics = Graphics.FromImage(BitX)
            With Graph
                .InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
                .DrawImage(Img, New Rectangle(0, 0, Width, Height), New Rectangle(0, 0, Img.Width, Img.Height), GraphicsUnit.Pixel)
            End With
        End If
        Return BitX
    End Function

    Public Function LoadImageFromURL(ByVal url As String) As Image
        Try
            Dim Img As Byte() = Web.DownloadData(url)
            Dim mStream As New MemoryStream(Img)
            Return Image.FromStream(mStream)
        Catch ex As Exception
            Return My.Resources.N_A
        End Try
    End Function

    Public Function GetImageFromGui(ByVal type As GuiImage) As Image
        Dim bmp As Bitmap, g As Graphics, selection As Rectangle
        Select Case type
            Case GuiImage.Button
                selection = New Rectangle(2, 3, 174, 63)
                bmp = New Bitmap(selection.Width, selection.Height)
                g = Graphics.FromImage(bmp)
                g.DrawImage(My.Resources.sampgui, 0, 0, selection, GraphicsUnit.Pixel)
                g.Dispose()
            Case GuiImage.TextBox
                selection = New Rectangle(7, 83, 320, 50)
                bmp = New Bitmap(selection.Width, selection.Height)
                g = Graphics.FromImage(bmp)
                g.DrawImage(My.Resources.sampgui, 0, 0, selection, GraphicsUnit.Pixel)
                g.Dispose()
            Case GuiImage.Scroll
                Dim scroll(2) As Image, line(2) As Image
                selection = New Rectangle(243, 123, 29, 33)
                scroll(0) = New Bitmap(selection.Width, selection.Height)
                g = Graphics.FromImage(scroll(0))
                g.DrawImage(My.Resources.sampgui, 0, 0, selection, GraphicsUnit.Pixel)
                g.Dispose()
                selection = New Rectangle(243, 149.5, 29, 33)
                scroll(1) = New Bitmap(selection.Width, selection.Height)
                g = Graphics.FromImage(scroll(1))
                g.DrawImage(My.Resources.sampgui, 0, 0, selection, GraphicsUnit.Pixel)
                g.Dispose()
                selection = New Rectangle(243, 141, 3, 17)
                line(0) = New Bitmap(selection.Width, selection.Height)
                g = Graphics.FromImage(line(0))
                g.DrawImage(My.Resources.sampgui, 0, 0, selection, GraphicsUnit.Pixel)
                g.Dispose()
                selection = New Rectangle(262, 141, 3, 17)
                line(1) = New Bitmap(selection.Width, selection.Height)
                g = Graphics.FromImage(line(1))
                g.DrawImage(My.Resources.sampgui, 0, 0, selection, GraphicsUnit.Pixel)
                g.Dispose()
                bmp = New Bitmap(29, 202)
                g = Graphics.FromImage(bmp)
                g.DrawImage(scroll(0), New Point(0, 0))
                g.DrawImage(scroll(1), New Point(0, scroll(0).Height + 136))
                g.DrawImage(line(0), New Point(0, scroll(0).Height))
                g.DrawImage(line(0), New Point(0, scroll(0).Height + line(0).Height))
                g.DrawImage(line(0), New Point(0, scroll(0).Height + line(0).Height * 2))
                g.DrawImage(line(0), New Point(0, scroll(0).Height + line(0).Height * 3))
                g.DrawImage(line(0), New Point(0, scroll(0).Height + line(0).Height * 4))
                g.DrawImage(line(0), New Point(0, scroll(0).Height + line(0).Height * 5))
                g.DrawImage(line(0), New Point(0, scroll(0).Height + line(0).Height * 6))
                g.DrawImage(line(0), New Point(0, scroll(0).Height + line(0).Height * 7))
                g.DrawImage(line(1), New Point(scroll(0).Width - 3.51, scroll(0).Height))
                g.DrawImage(line(1), New Point(scroll(0).Width - 4, scroll(0).Height + line(1).Height))
                g.DrawImage(line(1), New Point(scroll(0).Width - 4, scroll(0).Height + line(1).Height * 2))
                g.DrawImage(line(1), New Point(scroll(0).Width - 4, scroll(0).Height + line(1).Height * 3))
                g.DrawImage(line(1), New Point(scroll(0).Width - 4, scroll(0).Height + line(1).Height * 4))
                g.DrawImage(line(1), New Point(scroll(0).Width - 4, scroll(0).Height + line(1).Height * 5))
                g.DrawImage(line(1), New Point(scroll(0).Width - 4, scroll(0).Height + line(1).Height * 6))
                g.DrawImage(line(1), New Point(scroll(0).Width - 4, scroll(0).Height + line(1).Height * 7))
                g.DrawLine(Pens.White, scroll(0).Width - 1, scroll(0).Height, scroll(0).Width - 1, scroll(0).Height + 136)
                g.Dispose()
            Case GuiImage.Dot
                Dim dot As Image
                selection = New Rectangle(56, 56, 30, 31)
                dot = New Bitmap(selection.Width, selection.Height)
                g = Graphics.FromImage(dot)
                g.DrawImage(My.Resources.sampgui, 0, 0, selection, GraphicsUnit.Pixel)
                g.Dispose()
                bmp = New Bitmap(456, 31)
                g = Graphics.FromImage(bmp)
                g.DrawImage(dot, New Point(0, 0))
                g.DrawImage(dot, New Point(dot.Width + 8, 0))
                g.DrawImage(dot, New Point((dot.Width + 8) * 2, 0))
                g.DrawImage(dot, New Point((dot.Width + 8) * 3, 0))
                g.DrawImage(dot, New Point((dot.Width + 8) * 4, 0))
                g.DrawImage(dot, New Point((dot.Width + 8) * 5, 0))
                g.DrawImage(dot, New Point((dot.Width + 8) * 6, 0))
                g.DrawImage(dot, New Point((dot.Width + 8) * 7, 0))
                g.DrawImage(dot, New Point((dot.Width + 8) * 8, 0))
                g.DrawImage(dot, New Point((dot.Width + 8) * 9, 0))
                g.DrawImage(dot, New Point((dot.Width + 8) * 10, 0))
                g.DrawImage(dot, New Point((dot.Width + 8) * 11, 0))
                g.Dispose()
            Case Else
                bmp = New Bitmap(1, 1)
        End Select
        Return bmp
    End Function

#End Region

#Region "Other"

    Public Function BoolToInt(ByVal value As Boolean) As Integer
        If value = True Then
            Return 1
        Else
            Return 0
        End If
    End Function

    Public Function IntToBool(ByVal value As Integer) As Boolean
        If value = 0 Then
            Return False
        Else
            Return True
        End If
    End Function

    Public Function LanguajeFromInt(ByVal value As Integer) As Lang
        Select Case value
            Case 0
                Return Lang.English
            Case 1
                Return Lang.Español
            Case 2
                Return Lang.Portugues
            Case Else
                Return Lang.English
        End Select
    End Function

    Public Function LanguajeToInt(ByVal languaje As Lang) As Integer
        Select Case languaje
            Case Lang.English
                Return 0
            Case Lang.Español
                Return 1
            Case Lang.Portugues
                Return 2
        End Select
    End Function

    Public Function GetNodeItem(ByVal nodes As TreeNodeCollection, ByVal pattern As String, Optional ByVal MatchWord As Boolean = False) As TreeNode
        On Error Resume Next
        Dim res As New TreeNode
        If MatchWord = False Then
            For Each node As TreeNode In nodes
                If Regex.Match(node.Text, pattern, RegexOptions.IgnoreCase).Success = True Then
                    res = node
                End If
            Next
        Else
            For Each node As TreeNode In nodes
                If pattern.Length < node.Text.Length Then
                    If node.Text.IndexOf(pattern, 0, pattern.Length, StringComparison.InvariantCultureIgnoreCase) >= 0 Then
                        res = node
                    End If
                Else
                    If node.Text.IndexOf(pattern, 0, node.Text.Length, StringComparison.InvariantCultureIgnoreCase) >= 0 Then
                        res = node
                    End If
                End If
            Next
        End If
        Return res
    End Function

#End Region

#End Region

End Module
