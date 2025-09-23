Imports System.Collections.Concurrent

Public Class Form2
    Inherits Form
    Private ultimoId As String = "" ' guarda el último Id agregado

    Private webForm As New Form_webview

    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Conectar eventos de los botones del Designer
        AddHandler BtnAgregar.Click, AddressOf BtnAgregar_Click
        AddHandler BtnTexto.Click, AddressOf BtnAgregarTexto_Click
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
        Dim fileUri = New Uri(ofd.FileName)
        Dim url = fileUri.AbsoluteUri
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
            Retraso:=500,
            FadeIn:=500,
            FadeOut:=0,
            ObjectFit:=objectFitSeleccionado
        )
        Next
    End Sub

    Private Sub BtnAgregarTexto_Click(sender As Object, e As EventArgs)

        Dim efectoSeleccionado As Integer = ComboBox1.SelectedIndex


        ''fondo
        'webForm.AgregarObjetoDisplay(IdGrupo:="grupo0", Id:=Guid.NewGuid().ToString(), Url:=GetFileUrl("C:\Users\Angelo\Downloads\archivos test v72\fondo.png"),
        'Ancho:=1248, Alto:=624,
        'PosX:=0, PosY:=0,
        'NivelCapa:=1, Opacidad:=100, Retraso:=0, FadeIn:=0, FadeOut:=0, ObjectFit:="fill")

        ''mask
        'webForm.AgregarObjetoDisplay(IdGrupo:="grupo1", Id:=Guid.NewGuid().ToString(), Url:=GetFileUrl("C:\Users\Angelo\Downloads\archivos test v72\mask.png"),
        'Ancho:=1248, Alto:=624,
        'PosX:=0, PosY:=0,
        'NivelCapa:=2, Opacidad:=100, Retraso:=0, FadeIn:=0, FadeOut:=0)

        ''prc
        'webForm.AgregarObjetoDisplay(IdGrupo:="grupo1", Id:=Guid.NewGuid().ToString(), Url:=GetFileUrl("C:\Users\Angelo\Downloads\archivos test v72\precioled.webm"),
        'Ancho:=450, Alto:=450,
        'PosX:=-90, PosY:=100,
        'NivelCapa:=5, Opacidad:=100, Retraso:=0, FadeIn:=1000, FadeOut:=0)

        ''uda
        'webForm.AgregarObjetoDisplay(IdGrupo:="grupo1", Id:=Guid.NewGuid().ToString(), Url:=GetFileUrl("C:\Users\Angelo\Downloads\archivos test v72\uda.webm"),
        'Ancho:=450, Alto:=450,
        'PosX:=870, PosY:=100,
        'NivelCapa:=5, Opacidad:=100, Retraso:=0, FadeIn:=1000, FadeOut:=0)


        ''pepsi1
        'webForm.AgregarObjetoDisplay(IdGrupo:="grupo1", Id:=Guid.NewGuid().ToString(), Url:=GetFileUrl("C:\Users\Angelo\Downloads\archivos test v72\pepsi.webm"),
        'Ancho:=80, Alto:=80,
        'PosX:=124, PosY:=10,
        'NivelCapa:=10, Opacidad:=100, Retraso:=0, FadeIn:=1000, FadeOut:=0)

        ''pepsi2
        'webForm.AgregarObjetoDisplay(IdGrupo:="grupo1", Id:=Guid.NewGuid().ToString(), Url:=GetFileUrl("C:\Users\Angelo\Downloads\archivos test v72\pepsi.webm"),
        'Ancho:=80, Alto:=80,
        'PosX:=1044, PosY:=10,
        'NivelCapa:=10, Opacidad:=100, Retraso:=0, FadeIn:=1000, FadeOut:=0)

        ''animar
        'webForm.AgregarObjetoDisplay(IdGrupo:="grupo1", Id:=Guid.NewGuid().ToString(), Url:=GetFileUrl("C:\Users\Angelo\Downloads\archivos test v72\animo.webm"),
        'Ancho:=1248, Alto:=300,
        'PosX:=0, PosY:=330,
        'NivelCapa:=10, Opacidad:=100, Retraso:=0, FadeIn:=1000, FadeOut:=0)


        ' Obtener valores de los NUD del Designer
        Dim posX = CInt(NUDPosX.Value)
        Dim posY = CInt(NUDPosY.Value)
        Dim ancho = CInt(NUDAncho.Value)
        Dim alto = CInt(NUDAlto.Value)
        Dim opacidad = CInt(NUDOpacidad.Value)
        Dim textoContenido = TxtContenido.Text

        'If String.IsNullOrWhiteSpace(textoContenido) Then
        '    Return
        'End If
        Dim objectFitSeleccionado As String = ComboBox2.SelectedItem.ToString()
        ultimoId = Guid.NewGuid().ToString()

        ' Enviar al WebView como objeto de texto
        webForm.AgregarObjetoDisplay(
            IdGrupo:="grupo1",
            Id:=ultimoId,
            Texto:=New TextoConfig With {
                .Contenido = "<span style='background-color:yellow;'>123456</span>",
                .Color = "black",
                .FontSize = 48,
                .FontWeight = "bold",
                .FontFamily = "Montserrat",
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
            FadeOut:=0,
            ObjectFit:=objectFitSeleccionado
        )
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
            webForm.DLL_EditarTexto(ultimoId, "<b>Nuevo</b>", efecto:=1)

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
