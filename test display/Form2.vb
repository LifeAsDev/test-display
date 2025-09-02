
Public Class Form2
    Inherits Form
    Private webForm As New form_webview

    ' Controles
  

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Configurar NumericUpDowns
        NUDPosX.Location = New Point(10, 10) : NUDPosX.Maximum = 1920 : NUDPosX.Minimum = 0 : Me.Controls.Add(NUDPosX)
        NUDPosY.Location = New Point(120, 10) : NUDPosY.Maximum = 1080 : NUDPosY.Minimum = 0 : Me.Controls.Add(NUDPosY)
        NUDAncho.Location = New Point(10, 50) : NUDAncho.Maximum = 1920 : NUDAncho.Minimum = 0 : Me.Controls.Add(NUDAncho)
        NUDAlto.Location = New Point(120, 50) : NUDAlto.Maximum = 1080 : NUDAlto.Minimum = 0 : Me.Controls.Add(NUDAlto)
        NUDOpacidad.Location = New Point(10, 90) : NUDOpacidad.Maximum = 100 : NUDOpacidad.Minimum = 0 : Me.Controls.Add(NUDOpacidad)
        Me.Left = form_display.Width + 20
        Me.Top = 50

        webForm = New form_webview
        webForm.Left = Me.Left + Me.Width + 20
        webForm.Top = 50
        webForm.Show()
        ' Configurar botón
        BtnAgregar.Text = "Agregar Objeto"
        BtnAgregar.Location = New Point(10, 130)
        AddHandler BtnAgregar.Click, AddressOf BtnAgregar_Click
        Me.Controls.Add(BtnAgregar)
    End Sub

    Private Sub BtnAgregar_Click(sender As Object, e As EventArgs)
        ' Abrir OpenFileDialog
        Dim ofd As New OpenFileDialog()
        ofd.Filter = "Archivos multimedia|*.png;*.jpg;*.gif;*.mp4;*.avi;*.webm|Todos los archivos|*.*"
        If ofd.ShowDialog() <> DialogResult.OK Then Return

        ' Obtener valores de NumericUpDown
        Dim posX = CInt(NUDPosX.Value)
        Dim posY = CInt(NUDPosY.Value)
        Dim ancho = CInt(NUDAncho.Value)
        Dim alto = CInt(NUDAlto.Value)
        Dim opacidad = CInt(NUDOpacidad.Value)
        Dim filename = ofd.FileName
        Dim url = CopyToTempAndGetUrl(ofd.FileName)


        ' Llamar a tu función
        webForm.AgregarObjetoDisplay(
            IdGrupo:="grupo1",
            Id:=Guid.NewGuid().ToString(),
            Url:=url,
            Ancho:=ancho,
            Alto:=alto,
            PosX:=posX,
            PosY:=posY,
            NivelCapa:=1,
            Opacidad:=opacidad,
            Retraso:=500,
            FadeIn:=500,
            FadeOut:=0
        )

        MessageBox.Show("Objeto agregado: " & System.IO.Path.GetFileName(url))
    End Sub
End Class
