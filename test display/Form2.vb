Public Class Form2
    Inherits Form
    Private ultimoId As String = "" ' guarda el último Id agregado

    Private webForm As New form_webview

    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Conectar eventos de los botones del Designer
        AddHandler BtnAgregar.Click, AddressOf BtnAgregar_Click
        AddHandler BtnTexto.Click, AddressOf BtnAgregarTexto_Click
        AddHandler BtnClear.Click, Sub() webForm.clearAllElements()

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
        webForm.ShowInTaskbar = True
        ComboBox1.SelectedIndex = 0
        ComboBox2.SelectedIndex = 0

        webForm.Show()
    End Sub

    Private Sub BtnAgregar_Click(sender As Object, e As EventArgs)
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
        Dim url = CopyToTempAndGetUrl(ofd.FileName)
        Dim objectFitSeleccionado As String = ComboBox2.SelectedItem.ToString()
        ultimoId = Guid.NewGuid().ToString()

        'enviar al webview
        webForm.AgregarObjetoDisplay(
        IdGrupo:="grupo1",
        Id:=ultimoId,
        Url:=url,
           Ancho:=ancho,
        Alto:=alto,
        PosX:=posX,
        PosY:=posY,
        NivelCapa:=1,
          Opacidad:=opacidad,
        Retraso:=500,
        FadeIn:=500,
        FadeOut:=0,
        ObjectFit:=objectFitSeleccionado
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
        Dim url = CopyToTempAndGetUrl(ofd.FileName)
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
            Retraso:=500,
            FadeIn:=500,
            FadeOut:=0,
            ObjectFit:=objectFitSeleccionado
        )
        Next
    End Sub

    Private Sub BtnAgregarTexto_Click(sender As Object, e As EventArgs)



        'fondo
        webForm.AgregarObjetoDisplay(IdGrupo:="grupo0", Id:=Guid.NewGuid().ToString(), Url:=CopyToTempAndGetUrl("C:\Users\Angelo\Downloads\archivos test v7\fondo.jpeg"),
Ancho:=1248, Alto:=624,
PosX:=0, PosY:=0,
NivelCapa:=1, Opacidad:=100, Retraso:=0, FadeIn:=0, FadeOut:=0, ObjectFit:="fill")

        'mask
        webForm.AgregarObjetoDisplay(IdGrupo:="grupo1", Id:=Guid.NewGuid().ToString(), Url:=CopyToTempAndGetUrl("C:\Users\Angelo\Downloads\archivos test v7\bossDesign1.webp"),
Ancho:=1248, Alto:=624,
PosX:=0, PosY:=0,
NivelCapa:=2, Opacidad:=100, Retraso:=0, FadeIn:=0, FadeOut:=0)

        'prc
        webForm.AgregarObjetoDisplay(IdGrupo:="grupo1", Id:=Guid.NewGuid().ToString(), Url:=CopyToTempAndGetUrl("C:\Users\Angelo\Downloads\archivos test v7\v1.webm"),
Ancho:=450, Alto:=450,
PosX:=-90, PosY:=100,
NivelCapa:=5, Opacidad:=100, Retraso:=0, FadeIn:=1000, FadeOut:=0)

        'uda
        webForm.AgregarObjetoDisplay(IdGrupo:="grupo1", Id:=Guid.NewGuid().ToString(), Url:=CopyToTempAndGetUrl("C:\Users\Angelo\Downloads\archivos test v7\uda.webm"),
Ancho:=450, Alto:=450,
PosX:=870, PosY:=100,
NivelCapa:=5, Opacidad:=100, Retraso:=0, FadeIn:=1000, FadeOut:=0)


        'pepsi1
        webForm.AgregarObjetoDisplay(IdGrupo:="grupo1", Id:=Guid.NewGuid().ToString(), Url:=CopyToTempAndGetUrl("C:\Users\Angelo\Downloads\archivos test v7\pepsi.webm"),
Ancho:=80, Alto:=80,
PosX:=124, PosY:=10,
NivelCapa:=10, Opacidad:=100, Retraso:=0, FadeIn:=1000, FadeOut:=0)

        'pepsi2
        webForm.AgregarObjetoDisplay(IdGrupo:="grupo1", Id:=Guid.NewGuid().ToString(), Url:=CopyToTempAndGetUrl("C:\Users\Angelo\Downloads\archivos test v7\pepsi.webm"),
Ancho:=80, Alto:=80,
PosX:=1044, PosY:=10,
NivelCapa:=10, Opacidad:=100, Retraso:=0, FadeIn:=1000, FadeOut:=0)

        'animar
        webForm.AgregarObjetoDisplay(IdGrupo:="grupo1", Id:=Guid.NewGuid().ToString(), Url:=CopyToTempAndGetUrl("C:\Users\Angelo\Downloads\archivos test v7\gato.webm"),
Ancho:=1248, Alto:=300,
PosX:=0, PosY:=330,
NivelCapa:=10, Opacidad:=100, Retraso:=0, FadeIn:=1000, FadeOut:=0)


        '0
        webForm.AgregarObjetoDisplay(IdGrupo:="grupo1", Id:=Guid.NewGuid().ToString(), Texto:=New TextoConfig With {
        .Contenido = "0",
        .Color = "white",
        .FontSize = 330,
        .FontWeight = "bold",
        .FontFamily = "Montserrat",
        .Align = "center",
        .Efecto = 1},
    Ancho:=0, Alto:=0,
    PosX:=480,
    PosY:=480,
    NivelCapa:=12,
    Opacidad:=100,
    Retraso:=0, FadeIn:=400, FadeOut:=0)

        '0
        webForm.AgregarObjetoDisplay(IdGrupo:="grupo1", Id:=Guid.NewGuid().ToString(), Texto:=New TextoConfig With {
        .Contenido = "0",
        .Color = "white",
        .FontSize = 330,
        .FontWeight = "bold",
        .FontFamily = "Montserrat",
        .Align = "center",
        .Efecto = 1},
    Ancho:=0, Alto:=0,
    PosX:=760,
    PosY:=480,
    NivelCapa:=12,
    Opacidad:=100,
    Retraso:=0, FadeIn:=400, FadeOut:=0)

        '12:43
        webForm.AgregarObjetoDisplay(IdGrupo:="grupo1", Id:=Guid.NewGuid().ToString(), Texto:=New TextoConfig With {
        .Contenido = "12:43",
        .Color = "white",
        .FontSize = 100,
        .FontWeight = "bold",
        .FontFamily = "Montserrat",
        .Align = "center",
        .Efecto = 1},
    Ancho:=0, Alto:=0,
    PosX:=624,
    PosY:=100,
    NivelCapa:=12,
    Opacidad:=100,
    Retraso:=0, FadeIn:=400, FadeOut:=0)


        '' Obtener valores de los NUD del Designer
        'Dim posX = CInt(NUDPosX.Value)
        'Dim posY = CInt(NUDPosY.Value)
        'Dim ancho = CInt(NUDAncho.Value)
        'Dim alto = CInt(NUDAlto.Value)
        'Dim opacidad = CInt(NUDOpacidad.Value)
        'Dim textoContenido = TxtContenido.Text

        'If String.IsNullOrWhiteSpace(textoContenido) Then
        '    Return
        'End If
        'Dim efectoSeleccionado As Integer = ComboBox1.SelectedIndex
        'Dim objectFitSeleccionado As String = ComboBox2.SelectedItem.ToString()

        '' Enviar al WebView como objeto de texto
        'webForm.AgregarObjetoDisplay(
        '    IdGrupo:="grupo1",
        '    Id:=Guid.NewGuid().ToString(),
        '    Texto:=New TextoConfig With {
        '        .Contenido = textoContenido,
        '        .Color = "yellow",
        '        .FontSize = 48,
        '        .FontWeight = "bold",
        '        .FontFamily = "Dreams Adventure Co",
        '        .Align = "left",
        '        .Efecto = efectoSeleccionado
        '    },
        '    Ancho:=ancho,
        '    Alto:=alto,
        '    PosX:=posX,
        '    PosY:=posY,
        '    NivelCapa:=2,
        '    Opacidad:=opacidad,
        '    Retraso:=0,
        '    FadeIn:=400,
        '    FadeOut:=0,
        '    ObjectFit:=objectFitSeleccionado
        ')
    End Sub

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
            webForm.DLL_OcultaObjeto(ultimoId)
        End If
    End Sub

    Private Sub BtnMostrarObjeto_Click(sender As Object, e As EventArgs)
        If ultimoId <> "" Then
            webForm.DLL_MostrarObjeto(ultimoId)
        End If
    End Sub

    Private Sub BtnEliminaObjeto_Click(sender As Object, e As EventArgs)
        If ultimoId <> "" Then
            webForm.DLL_EliminaObjeto(ultimoId)
        End If
    End Sub

End Class
