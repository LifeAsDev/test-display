Imports System.Collections.Concurrent
Imports System.Diagnostics
Imports System.IO
Imports System.Net.Mime
Imports System.Text
Imports System.Threading
Imports EmbedIO
Imports EmbedIO.Actions
Imports EmbedIO.Files
Imports EmbedIO.Utilities
Imports EmbedIO.WebApi
Imports Microsoft.Web.WebView2.Core
Imports Microsoft.Web.WebView2.WinForms
Imports Newtonsoft.Json
Imports IOF = System.IO
Imports Accord.Video.FFMPEG
Imports System.Drawing
Imports LibVLCSharp.Shared

Public Enum ObjectFitOption
    Fill
    Contain
    Cover
    None
    ScaleDown
End Enum

Public Class TextoConfig
    Public Property Contenido As String
    Public Property Color As String
    Public Property FontSize As Integer
    Public Property FontWeight As String
    Public Property FontFamily As String
    Public Property Align As String
    Public Property Efecto As Integer


End Class

Public Class Form_webview
    Private web As Microsoft.Web.WebView2.WinForms.WebView2
    Private server As New MiniServer()




    Public Function GetFrame(videoPath As String, segundo As Double) As Bitmap
        ' Inicializar LibVLC
        Core.Initialize()

        ' Ruta a los binarios nativos VLC
        Dim vlcPath As String = Path.Combine(Application.StartupPath, "libvlc", "win-x64") ' o win-x86

        Using libVlc As New LibVLC("--no-video-title-show", "--quiet", "--intf", "dummy", "--no-xlib", "--no-video")
            Using media As New Media(libVlc, videoPath, FromType.FromPath)
                Using player As New MediaPlayer(media)
                    player.Play()
                    Thread.Sleep(200) ' esperar a que arranque
                    player.Time = CLng(segundo * 1000) ' mover al segundo deseado
                    Thread.Sleep(200) ' esperar a que decodifique

                    ' Snapshot temporal
                    Dim tmpFile As String = Path.Combine(Path.GetTempPath(), "frame.png")
                    player.TakeSnapshot(0, tmpFile, 0, 0) ' 0,0 = tamaño original
                    player.Stop()

                    Dim bmp As Bitmap = Nothing
                    Dim maxRetries As Integer = 10
                    Dim delay As Integer = 100 ' milisegundos

                    For i As Integer = 1 To maxRetries
                        Try
                            Using fs As New FileStream(tmpFile, FileMode.Open, FileAccess.Read, FileShare.Read)
                                bmp = New Bitmap(fs)
                            End Using

                            ' Si llegamos aquí, el archivo ya se pudo leer
                            File.Delete(tmpFile)
                            Exit For
                        Catch ex As IOException
                            Threading.Thread.Sleep(delay) ' esperar un poco y reintentar
                        End Try
                    Next

                    Return bmp

                End Using
            End Using
        End Using
    End Function

    Public Function AddDynamicFile(originalFile As String) As String
        Dim ext = IO.Path.GetExtension(originalFile).ToLowerInvariant()
        Dim fileId = Guid.NewGuid().ToString() & ext
        MiniServer.DynamicFiles(fileId) = originalFile
        Return $"http://localhost:5000/file/{fileId}"
    End Function

    Private Async Sub Form_webview_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Iniciar mini servidor

        Me.TransparencyKey = Color.Lime
        Me.BackColor = Color.Lime
        Me.TopMost = True
        server.StartServer()
        Me.FormBorderStyle = FormBorderStyle.None

        ' Crear WebView2
        Dim env = Await CoreWebView2Environment.CreateAsync(
    Nothing,
    Nothing,
    New CoreWebView2EnvironmentOptions("--enable-gpu-rasterization --enable-zero-copy --ignore-gpu-blocklist --use-gl=d3d11 --enable-accelerated-video-decode")
)
        web = New Microsoft.Web.WebView2.WinForms.WebView2 With {
            .Dock = DockStyle.Fill
        }
        Me.Controls.Add(web)
        Me.StartPosition = FormStartPosition.Manual
        Me.Location = New Point(0, 0)
        ' Inicializar WebView2 y navegar a localhost
        Await web.EnsureCoreWebView2Async(env)

        web.DefaultBackgroundColor = Color.Transparent ' <-- clave
        'web.CoreWebView2.Navigate("http://localhost:5000/")
        Dim htmlPath As String = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "index.html")
        web.CoreWebView2.AddWebResourceRequestedFilter("app://bitmap/*", Microsoft.Web.WebView2.Core.CoreWebView2WebResourceContext.All)
        AddHandler web.CoreWebView2.WebResourceRequested, AddressOf OnWebResourceRequested
        ' Navegar directamente al archivo local
        web.CoreWebView2.Navigate("file:///" & htmlPath.Replace("\", "/"))
        'Xpcom.Initialize("C:\Users\Angelo\Desktop\project\test display\test display\Firefox\") ' <-- Cambia por tu ruta

        ' Crear el GeckoWebBrowser
        '    browser = New GeckoWebBrowser() With {
        '    .Dock = DockStyle.Fill,
        '    .BackColor = Color.Lime ' <-- Establecer el color de fondo del control
        '}
        '    Me.Controls.Add(browser)

        '    browser.NoDefaultContextMenu = True

        '    ' Navegar a una página de prueba
        '    browser.Navigate("localhost:5000")
    End Sub


    ' Diccionario para almacenar Bitmaps en memoria
    Private bitmapsMemoria As New Dictionary(Of String, Bitmap)()

    ' Guardar un Bitmap en memoria con clave
    Public Function GuardarBitmap(nombre As String, bmp As Bitmap) As String
        ' Guardar en memoria
        bitmapsMemoria(nombre) = bmp

        ' Generar la URL para WebView
        Dim url As String = "app://bitmap/" & nombre
        Return url
    End Function

    ' Cuando WebView solicita app://bitmap/…
    Private Sub OnWebResourceRequested(sender As Object, e As CoreWebView2WebResourceRequestedEventArgs)
        Dim uri As String = e.Request.Uri
        Debug.WriteLine("Solicitud bitmap: " & e.Request.Uri)

        ' Extraer solo el path, ignorando query string
        Dim nombre As String = uri.Substring("app://bitmap/".Length)
        Dim qIndex As Integer = nombre.IndexOf("?")
        If qIndex >= 0 Then
            nombre = nombre.Substring(0, qIndex) ' quitar ?v=...
        End If
        Debug.WriteLine(nombre)
        ' Mostrar todas las claves de bitmapsMemoria en Output de Visual Studio


        If bitmapsMemoria.ContainsKey(nombre) Then
            Dim ms As New IO.MemoryStream()
            bitmapsMemoria(nombre).Save(ms, Imaging.ImageFormat.Png)
            ms.Position = 0
            e.Response = web.CoreWebView2.Environment.CreateWebResourceResponse(ms, 200, "OK", "Content-Type: image/png")
            ' No cerramos ms, WebView2 se encarga de leerlo
        End If
    End Sub
    Private Sub LogBitmapsMemoria()
        Debug.WriteLine("---- bitmapsMemoria ----")
        For Each kvp As KeyValuePair(Of String, Bitmap) In bitmapsMemoria
            Debug.WriteLine("Nombre: " & kvp.Key & " | Tamaño: " & kvp.Value.Width & "x" & kvp.Value.Height)
        Next
        Debug.WriteLine("------------------------")
    End Sub



    Public Async Sub AgregarObjetoDisplay(
        IdGrupo As String,
        Id As String,
        Optional Url As String = "",
        Optional Texto As TextoConfig = Nothing,
        Optional Ancho As Integer = 200,
        Optional Alto As Integer = 200,
        Optional PosX As Integer = 0,
        Optional PosY As Integer = 0,
        Optional NivelCapa As Integer = 0,
        Optional Opacidad As Integer = 100,
        Optional Retraso As Integer = 0,
        Optional FadeIn As Integer = 0,
        Optional FadeOut As Integer = 0,
        Optional ObjectFit As String = "contain")



        ' Crear objeto de configuración
        Dim config = New With {
        IdGrupo,
        Id,
        .Url = Url.Replace("\", "\\"),
        Texto,
        Ancho,
        Alto,
        PosX,
        PosY,
        NivelCapa,
        Opacidad,
        Retraso,
        FadeIn,
        FadeOut,
        ObjectFit
    }

        ' Serializar a JSON usando Newtonsoft
        Dim json As String = JsonConvert.SerializeObject(config)

        ' Ejecutar JS en WebView2
        Await web.CoreWebView2.ExecuteScriptAsync($"agregarObjetoDisplay({json});")


    End Sub
    Public Async Sub DLL_SetVideoBucle(id As String, valor As Boolean)
        Await web.CoreWebView2.ExecuteScriptAsync($"setVideoBucle('{id}', {valor.ToString().ToLower()});")
    End Sub

    Public Async Sub DLL_CambiaOpacidad(id As String, valor As Integer)
        Await web.CoreWebView2.ExecuteScriptAsync($"cambiaOpacidad('{id}', {valor});")
    End Sub

    Public Async Sub DLL_OcultaObjeto(id As String)
        Await web.CoreWebView2.ExecuteScriptAsync($"ocultaObjeto('{id}');")
    End Sub

    Public Async Sub DLL_MostrarObjeto(id As String)
        Await web.CoreWebView2.ExecuteScriptAsync($"mostrarObjeto('{id}');")
    End Sub

    Public Async Sub DLL_EliminaObjeto(id As String)
        Await web.CoreWebView2.ExecuteScriptAsync($"eliminaObjeto('{id}');")
    End Sub

    Public Async Sub DLL_EditarTexto(id As String, contenido As String, Optional color As String = Nothing, Optional fontSize As Integer = 0, Optional fontWeight As String = Nothing, Optional fontFamily As String = Nothing, Optional align As String = Nothing, Optional efecto As Integer = -1)
        Dim opciones = New System.Collections.Generic.Dictionary(Of String, Object)()

        If contenido IsNot Nothing Then opciones("Contenido") = contenido
        If Not String.IsNullOrEmpty(color) Then opciones("Color") = color
        If fontSize > 0 Then opciones("FontSize") = fontSize
        If Not String.IsNullOrEmpty(fontWeight) Then opciones("FontWeight") = fontWeight
        If Not String.IsNullOrEmpty(fontFamily) Then opciones("FontFamily") = fontFamily
        If Not String.IsNullOrEmpty(align) Then opciones("Align") = align
        If efecto >= 0 Then opciones("Efecto") = efecto


        Dim opcionesJson = Newtonsoft.Json.JsonConvert.SerializeObject(opciones, New Newtonsoft.Json.JsonSerializerSettings With {.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore})
        ' id correctamente escapado como string JSON:
        Dim js = $"editarTexto({Newtonsoft.Json.JsonConvert.ToString(id)}, {opcionesJson});"
        Await web.CoreWebView2.ExecuteScriptAsync(js)
    End Sub



    Public Async Sub ClearAllElements()


        Await web.CoreWebView2.ExecuteScriptAsync($"clearAllElements();")


    End Sub


    Private Sub Form_webview_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        server.StopServer()
    End Sub
End Class


Public Class MiniServer
    Private server As WebServer
    Private ReadOnly tempFolder As String = Path.Combine(Path.GetTempPath(), "MiniServerUploads")
    Public Shared ReadOnly DynamicFiles As New ConcurrentDictionary(Of String, String)

    Public Sub StartServer()
        Dim url As String = "http://localhost:5000/"
        Dim logPath As String = "C:\Users\Angelo\Downloads\archivos test v7\server_errors.log"

        Try
            server = New WebServer(HttpListenerMode.EmbedIO, url)
            server.WithModule(New ActionModule("/file", HttpVerbs.Get,
    Async Function(ctx As IHttpContext) As Task
        ' Extraer solo el nombre del archivo desde la URL


        Dim id = IO.Path.GetFileName(ctx.RequestedPath)
        Dim realPath As String = Nothing

        If dynamicFiles.TryGetValue(id, realPath) AndAlso IO.File.Exists(realPath) Then
            ' Detectar MIME según la extensión real del archivo
            Dim ext = IO.Path.GetExtension(realPath).ToLowerInvariant()
            Dim mime As String = "application/octet-stream"
            Select Case ext
                Case ".jpg", ".jpeg" : mime = "image/jpeg"
                Case ".png" : mime = "image/png"
                Case ".gif" : mime = "image/gif"
                Case ".webp" : mime = "image/webp"
                Case ".mp4" : mime = "video/mp4"
                Case ".webm" : mime = "video/webm"
                Case ".avi" : mime = "video/x-msvideo"
            End Select

            ctx.Response.ContentType = mime

            ' Stream manual
            Using fs As New IO.FileStream(realPath, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.Read)
                Await fs.CopyToAsync(ctx.Response.OutputStream)
            End Using

            ctx.Response.StatusCode = 200
        Else
            ctx.Response.StatusCode = 404
        End If

    End Function))


            server.WithStaticFolder("/", AppDomain.CurrentDomain.BaseDirectory, True)


            server.RunAsync()


        Catch ex As Exception
            Debug.WriteLine("Ya hay un servidor corriendo en " & url)
            ' No lo inicias, solo sigues con la app
        End Try
    End Sub


    Public Sub StopServer()
        server?.Dispose()
    End Sub
End Class

