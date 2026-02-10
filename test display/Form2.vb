Imports System.Collections.Concurrent
Imports System.Drawing
Imports System.IO
Imports Emgu.CV.Dnn

Public Class Form2
    Inherits Form
    Private ultimoId As String = "" ' guarda el último Id agregado

    Private webForm As New Form_webview

    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Conectar eventos de los botones del Designer
        AddHandler BtnAgregar.Click, AddressOf BtnAgregar_Click
        AddHandler BtnClear.Click, Sub() webForm.ClearAllElements()

        AddHandler BtnLoop.Click, AddressOf BtnSetVideoBucle_Click
        AddHandler BtnOpacidad.Click, AddressOf BtnCambiaOpacidad_Click
        AddHandler BtnOcultar.Click, AddressOf BtnOcultaObjeto_Click
        AddHandler BtnMostrar.Click, AddressOf BtnMostrarObjeto_Click
        AddHandler BtnEliminar.Click, AddressOf BtnEliminaObjeto_Click

        ' Posicionar ventana secundaria
        Me.Left = webForm.Width + 20
        Me.Top = 50
        webForm.StartPosition = FormStartPosition.Manual
        webForm.Left = 0
        webForm.Top = 0
        webForm.Width = 1366
        webForm.Height = 768
        webForm.ShowInTaskbar = False
        ComboBox1.SelectedIndex = 0
        ComboBox2.SelectedIndex = 0

        webForm.Show()
    End Sub

    Private Sub BtnAgregar_Click(sender As Object, e As EventArgs)

        Dim ofd As New OpenFileDialog With {
        .Filter = "Archivos multimedia|*.png;*.jpg;*.jpeg;*.gif;*.mp4;*.avi;*.webm|Todos los archivos|*.*"
    }

        If ofd.ShowDialog() <> DialogResult.OK Then Return

        Dim posX = CInt(NUDPosX.Value)
        Dim posY = CInt(NUDPosY.Value)
        Dim ancho = CInt(NUDAncho.Value)
        Dim alto = CInt(NUDAlto.Value)
        Dim opacidad = CInt(NUDOpacidad.Value)
        Dim objectFitSeleccionado As String = ComboBox2.SelectedItem.ToString()

        ultimoId = Guid.NewGuid().ToString()

        Dim filePath As String = ofd.FileName
        Dim ext As String = Path.GetExtension(filePath).ToLower()

        Dim urlFinal As String

        ' =========================
        ' 🖼️ IMAGEN → Bitmap → Base64
        ' =========================
        If {".png", ".jpg", ".jpeg", ".gif", ".bmp"}.Contains(ext) Then

            Using bmp As New Bitmap(filePath)
                urlFinal = Form_webview.BitmapToBase64(bmp, 75)
            End Using

        Else
            ' =========================
            ' 🎥 VIDEO → file://
            ' =========================
            urlFinal = New Uri(filePath).AbsoluteUri
        End If

        ' =========================
        ' 🚀 Enviar al WebView
        ' =========================
        webForm.AgregarObjetoDisplay(
        IdGrupo:="grupo1",
        Id:=ultimoId,
        Url:=urlFinal,
        Ancho:=ancho,
        Alto:=alto,
        PosX:=posX,
        PosY:=posY,
        NivelCapa:=1,
        Opacidad:=opacidad,
        Retraso:=2000,
        FadeIn:=2000,
        FadeOut:=4000,
        RetrasoOut:=0,
        ObjectFit:=objectFitSeleccionado,
        LoopVideo:=True,
        Replace:=True,
        Rotacion:=0,
        VoltearHorizontal:=True,
        VoltearVertical:=False
    )

    End Sub

    Private Sub BtnAgregarTest_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ' Abrir OpenFileDialog
        Dim ofd As New OpenFileDialog With {
            .Filter = "Archivos multimedia|*.png;*.jpg;*.gif;*.mp4;*.avi;*.webm|Todos los archivos|*.*"
        }
        If ofd.ShowDialog() <> DialogResult.OK Then Return

        ' Obtener valores de los NUD del Designer
        Dim posX = CInt(NUDPosX.Value)
        Dim posY = CInt(NUDPosY.Value)
        Dim ancho = CInt(NUDAncho.Value)
        Dim alto = CInt(NUDAlto.Value)
        Dim opacidad = CInt(NUDOpacidad.Value)
        Dim url = GetFileUrl(ofd.FileName)
        Dim objectFitSeleccionado As String = ComboBox2.SelectedItem.ToString()

        ' Hacer un loop de 1 a 10 para agregar múltiples objetos
        For i As Integer = 1 To 10
            webForm.AgregarObjetoDisplay(
            IdGrupo:="grupo1",
            Id:=Guid.NewGuid().ToString(),
            Url:=url,
            Ancho:=ancho,
            Alto:=alto,
            PosX:=0 + (i * 50),  ' desplazamiento horizontal
            PosY:=posY,
            NivelCapa:=1 + i,       ' capas ascendentes
            Opacidad:=opacidad,
            Retraso:=1000,
            FadeIn:=500,
            FadeOut:=0,
            ObjectFit:=objectFitSeleccionado
        )
        Next

        webForm.DLL_EliminarGrupo("grupo1", 5000, 0)
    End Sub

    Private Async Sub BtnAgregarTexto_Click(sender As Object, e As EventArgs) Handles BtnTexto.Click

        'Dim ruta As String = "C:\Users\Angelo\Downloads\archivos test v72\caminante.webm"
        '' 1. Obtener bitmap del frame
        'Dim bmp As String = Await webForm.GetFrame(ruta, 1.0)

        '    ' 2. Guardarlo como archivo temporal PNG
        '    Dim tempFile As String = Path.Combine(
        '    Path.GetTempPath(),
        '    "frame_" & Guid.NewGuid().ToString() & ".png"
        ')
        '    bmp.Save(tempFile, Imaging.ImageFormat.Png)

        '    ' (opcional pero recomendado)
        '    bmp.Dispose()

        '    ultimoId = Guid.NewGuid().ToString()
        '    webForm.AgregarObjetoDisplay(
        'IdGrupo:="grupo1",
        'Id:="cam1",
        'Url:="camera",
        'PosX:=100,
        'PosY:=100,
        'Ancho:=400,
        'Alto:=300
        ')
        Dim Efecto As Integer = CInt(ComboBox1.SelectedItem)

        '      webForm.AgregarObjetoDisplay(IdGrupo:="grupo1", Id:="id", Url:="", Texto:=New TextoConfig With {
        '.Contenido = "Linea 1" & vbLf & "Linea 2" & vbLf & "Linea 3",
        '.Color = "#F54927", '"#" & color.ToArgb.ToString("X6"),
        '.FontSize = 60,
        '.FontWeight = "bold",
        '.FontFamily = "Montserrat", .FontStyle = "normal",
        '.FontDecoration = "none",
        '.Align = "center",
        '.Efecto = Efecto},
        'Ancho:=0, Alto:=0,
        'PosX:=Left,
        'PosY:=Top,
        'NivelCapa:=12,
        'Opacidad:=100,
        'Retraso:=0, FadeIn:=0, FadeOut:=0)



        ultimoId = Guid.NewGuid().ToString()
        Dim rnd As New Random()

        Dim offsetX As Integer = rnd.Next(-100, 101) ' -100 a 100
        Dim offsetY As Integer = rnd.Next(-100, 101) ' -100 a 100

        Dim cfg2 As New TextoConfig With {
    .Contenido = " Coopper, este es un texto<br> largdsadadadaddasd ", .Color = "yellow",
    .FontSize = 50,
    .FontWeight = "bold",
    .FontFamily = "Montserrat",
    .Align = "left",
    .Efecto = Efecto,
    .PosX = 400 + offsetX,
    .PosY = 200 + offsetY,
    .FadeIn = 1000,      ' tiempo en ms
    .RetrasoIn = 1000,     ' ms antes de iniciar fade in
    .Minusculas = True,
    .Rotacion = 0,
    .FadeOut = 200,
    .Sombra = "3px 3px 6px black",
    .TextAlign = "right",   ' alineación dentro del recuadr
    .WhiteSpace = "nowrap"
}



        webForm.DLL_AgregarTexto(ultimoId, cfg2, True)



    End Sub

    Private Function GetFileUrl(filePath As String) As String
        ' Convierte una ruta de archivo local a una URL con el protocolo file://
        ' Esto maneja correctamente las barras y los caracteres especiales.
        Return New Uri(filePath).AbsoluteUri
    End Function

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged

    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged

    End Sub
    Private Sub BtnSetVideoBucle_Click(sender As Object, e As EventArgs)
        If ultimoId <> "" Then
            webForm.DLL_SetVideoBucle(ultimoId, False) ' True = con loop

        End If
    End Sub

    Private Sub BtnCambiaOpacidad_Click(sender As Object, e As EventArgs)
        If ultimoId <> "" Then
            webForm.DLL_CambiaOpacidad(ultimoId, 50) ' 50% de opacidad
        End If
    End Sub

    Private Sub BtnOcultaObjeto_Click(sender As Object, e As EventArgs)
        If ultimoId <> "" Then
            webForm.DLL_EliminarGrupo("grupo1", 0, 0)
        End If
    End Sub

    Private Sub BtnMostrarObjeto_Click(sender As Object, e As EventArgs)
        If ultimoId <> "" Then
            webForm.DLL_MostrarObjeto(ultimoId)
        End If
    End Sub

    Private Sub BtnEliminaObjeto_Click(sender As Object, e As EventArgs)
        If ultimoId <> "" Then
            webForm.DLL_EliminaObjeto(ultimoId, 1000)

        End If
    End Sub


    Private Sub AplicarButton_Click(sender As Object, e As EventArgs) Handles AplicarButton.Click
        ' Obtener valores de los NumericUpDown
        Dim nuevoAncho As Integer = CInt(DisplayAncho.Value)
        Dim nuevoAlto As Integer = CInt(DisplayAlto.Value)
        Dim nuevaPosX As Integer = CInt(DisplayX.Value)
        Dim nuevaPosY As Integer = CInt(DisplayY.Value)

        ' Cambiar tamaño y posición del WebView
        webForm.Width = nuevoAncho
        webForm.Height = nuevoAlto
        webForm.Left = nuevaPosX
        webForm.Top = nuevaPosY
    End Sub


    Private Sub NUDPosX_ValueChanged(sender As Object, e As EventArgs) Handles NUDPosX.ValueChanged
        ' si no existe aún un objeto agregado, no hagas nada
        If String.IsNullOrEmpty(ultimoId) Then Exit Sub

        ' Obtener valores actuales del formulario
        Dim posX = CInt(NUDPosX.Value)
        Dim posY = CInt(NUDPosY.Value)
        Dim ancho = CInt(NUDAncho.Value)
        Dim alto = CInt(NUDAlto.Value)
        Dim opacidad = CInt(NUDOpacidad.Value)
        Dim objectFitSeleccionado As String = ComboBox2.SelectedItem.ToString()

        webForm.DLL_PintarCuadro("guidesBox", 100, 100, 100, 100, "4px", "red")

        ' Usar nuevamente la misma URL almacenada del objeto previo
        ' (si no la guardaste, me dices y la agregamos a una variable global)

        ' Actualizar objeto existente
        '    webForm.AgregarObjetoDisplay(
        '    IdGrupo:="grupo1",
        '    Id:=ultimoId,
        '    Ancho:=ancho,
        '    Alto:=alto,
        '    PosX:=posX,
        '    PosY:=posY,
        '    NivelCapa:=1,
        '    Opacidad:=opacidad,
        '    Retraso:=0,
        '    FadeIn:=0,
        '    FadeOut:=0,
        '    ObjectFit:=objectFitSeleccionado
        ')

    End Sub


    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click

    End Sub

    Private Sub DisplayAncho_ValueChanged(sender As Object, e As EventArgs) Handles DisplayAncho.ValueChanged

    End Sub


End Class
