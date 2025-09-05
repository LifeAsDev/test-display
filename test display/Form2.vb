Public Class Form2
    Inherits Form

    Private webForm As New form_webview

    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Conectar eventos de los botones del Designer
        AddHandler BtnAgregar.Click, AddressOf BtnAgregar_Click
        AddHandler BtnTexto.Click, AddressOf BtnAgregarTexto_Click
        AddHandler BtnClear.Click, Sub() webForm.clearAllElements()

        ' Posicionar ventana secundaria
        Me.Left = webForm.Width + 20
        Me.Top = 50
        webForm.StartPosition = FormStartPosition.Manual
        webForm.Left = 0
        webForm.Top = 0
        webForm.ShowInTaskbar = True
        ComboBox1.SelectedIndex = 0

        webForm.Show()
    End Sub

    Private Sub BtnAgregar_Click(sender As Object, e As EventArgs)
        ' Abrir OpenFileDialog
        Dim ofd As New OpenFileDialog()
        ofd.Filter = "Archivos multimedia|*.png;*.jpg;*.gif;*.mp4;*.avi;*.webm|Todos los archivos|*.*"
        If ofd.ShowDialog() <> DialogResult.OK Then Return

        ' Obtener valores de los NUD del Designer
        Dim posX = CInt(NUDPosX.Value)
        Dim posY = CInt(NUDPosY.Value)
        Dim ancho = CInt(NUDAncho.Value)
        Dim alto = CInt(NUDAlto.Value)
        Dim opacidad = CInt(NUDOpacidad.Value)
        'Dim url = CopyToTempAndGetUrl(ofd.FileName)

        Dim ruta As String = ofd.FileName
        Dim url2 As String = "http://localhost:5000/file?path=" & Uri.EscapeDataString(ruta)

        For i = 1 To 10
            ' Enviar ccola
            webForm.AgregarObjetoDisplay(IdGrupo:="grupo1", Id:=Guid.NewGuid().ToString(),
            Url:=CopyToTempAndGetUrl("C:\Users\Angelo\Downloads\archivos test v7\Pepsi.webm"),
            Ancho:=0,
            Alto:=0,
            PosX:=0 + (i * 50),
            PosY:=posY,
            NivelCapa:=2 + i,
            Opacidad:=100,
            Retraso:=i * 100,
            FadeIn:=500,
            FadeOut:=0)

        Next

        For i = 1 To 10
            ' Enviar ccola
            webForm.AgregarObjetoDisplay(IdGrupo:="grupo1", Id:=Guid.NewGuid().ToString(),
            Url:=CopyToTempAndGetUrl("C:\Users\Angelo\Downloads\archivos test v7\gato.webm"),
            Ancho:=0,
            Alto:=0,
            PosX:=0 + (i * 50),
            PosY:=posY,
            NivelCapa:=2 + i,
            Opacidad:=100,
            Retraso:=0,
            FadeIn:=500,
            FadeOut:=0)

        Next
        For i = 1 To 10
            ' Enviar ccola
            webForm.AgregarObjetoDisplay(IdGrupo:="grupo1", Id:=Guid.NewGuid().ToString(),
            Url:=CopyToTempAndGetUrl("C:\Users\Angelo\Downloads\archivos test v7\UDA.webm"),
            Ancho:=0,
            Alto:=0,
            PosX:=0 + (i * 50),
            PosY:=posY,
            NivelCapa:=2 + i,
            Opacidad:=100,
            Retraso:=0,
            FadeIn:=500,
            FadeOut:=0)

        Next
        'Enviar al WebView
        'webForm.AgregarObjetoDisplay(
        'IdGrupo:="grupo1",
        'Id:=Guid.NewGuid().ToString(),
        'Url:=url2,
        '   Ancho:=ancho,
        'Alto:=alto,
        'PosX:=posX,
        'PosY:=posY,
        'NivelCapa:=1,
        '  Opacidad:=opacidad,
        'Retraso:=500,
        'FadeIn:=500,
        'FadeOut:=0
        ')
    End Sub

    Private Sub BtnAgregarTexto_Click(sender As Object, e As EventArgs)
        ' Obtener valores de los NUD del Designer
        Dim posX = CInt(NUDPosX.Value)
        Dim posY = CInt(NUDPosY.Value)
        Dim ancho = CInt(NUDAncho.Value)
        Dim alto = CInt(NUDAlto.Value)
        Dim opacidad = CInt(NUDOpacidad.Value)
        Dim textoContenido = TxtContenido.Text

        If String.IsNullOrWhiteSpace(textoContenido) Then
            Return
        End If
        Dim efectoSeleccionado As Integer = ComboBox1.SelectedIndex

        ' Enviar al WebView como objeto de texto
        webForm.AgregarObjetoDisplay(
            IdGrupo:="grupo1",
            Id:=Guid.NewGuid().ToString(),
            Texto:=New TextoConfig With {
                .Contenido = textoContenido,
                .Color = "yellow",
                .FontSize = 24,
                .FontWeight = "bold",
                .FontFamily = "Dreams Adventure Co",
                .Align = "left",
                .Efecto = efectoSeleccionado
            },
            Ancho:=ancho,
            Alto:=alto,
            PosX:=posX,
            PosY:=posY,
            NivelCapa:=2,
            Opacidad:=opacidad,
            Retraso:=0,
            FadeIn:=400,
            FadeOut:=0
        )
    End Sub

End Class
