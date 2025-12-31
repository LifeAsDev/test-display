Imports System.Collections.Concurrent
Imports System.IO
Imports System.IO.Pipelines
Imports System.Threading
Imports EmbedIO
Imports EmbedIO.Actions
Imports Emgu.CV
Imports Emgu.CV.CvEnum
Imports LibVLCSharp.Shared
Imports Microsoft.Web.WebView2.Core
Imports Newtonsoft.Json
Imports System.Drawing.Imaging


Public Class TextoConfig
    Public Property Contenido As String
    Public Property Color As String
    Public Property FontSize As Integer?
    Public Property FontWeight As String
    Public Property FontFamily As String
    Public Property Align As String
    Public Property Efecto As Integer?
    Public Property FontStyle As String
    Public Property FontDecoration As String
    Public Property Ancho As Integer = 0
    Public Property Alto As Integer = 0
    Public Property PosX As Integer = 0
    Public Property PosY As Integer = 0
    Public Property NivelCapa As Integer = 0
    Public Property Opacidad As Integer = 100
    Public Property FadeIn As Integer = 0
    Public Property FadeOut As Integer = 0
    Public Property RetrasoIn As Integer = 0
    Public Property RetrasoOut As Integer = 0
    Public Property Grupo As String = ""
    Public Property Rotacion As Integer? = Nothing              ' grados
    Public Property Mayusculas As Boolean = False
    Public Property Minusculas As Boolean = False
    Public Property Sombra As String = ""                      ' "3px 3px 6px #000"
    Public Property TextAlign As String = Nothing ' "left", "center", "right", "justify"

    Public Property WhiteSpace As String = Nothing ' "normal", "nowrap", "pre", "pre-wrap", "pre-line"

    Public Property GrupoId As String = "grupo1"
End Class


Public Class Form_webview
    Private web As Microsoft.Web.WebView2.WinForms.WebView2
    Private server As New MiniServer()

    Public Async Function GetFrame(rutaVideo As String, segundo As Double) As Task(Of String)
        Try
            Await web.EnsureCoreWebView2Async()

            Dim carpeta As String = IO.Path.GetDirectoryName(rutaVideo)
            Dim nombreArchivo As String = IO.Path.GetFileName(rutaVideo)
            MsgBox("carpeta: " & carpeta)

            ' 2️⃣ Mapear carpeta a host virtual
            web.CoreWebView2.SetVirtualHostNameToFolderMapping(
            "video.assets",
            carpeta,
          CoreWebView2HostResourceAccessKind.Allow
        )
            Await Task.Delay(50) ' esperar que WebView2 registre el host virtual

            ' 3️⃣ URL virtual para JS
            Dim urlVideo As String = "file:///" & rutaVideo.Replace("\", "/")
            MsgBox("urlVideo: " & urlVideo)

            Dim jsCall As String = $"
(async () => {{
console.log('{urlVideo}');
    const video = document.createElement('video');
    video.src = '{urlVideo}';
    video.crossOrigin = 'anonymous';
    
    await video.play().catch(()=>{{}}); // Necesario para que cargue algunos videos
    video.pause(); // Solo necesitamos cargar metadata
    
    await new Promise(resolve => video.onloadedmetadata = resolve);

    // Calculamos el tiempo del frame 10
    const fps = 30; // Ajusta según tu video
    video.currentTime = 10 / fps;

    await new Promise(resolve => video.onseeked = resolve);

    const canvas = document.createElement('canvas');
    canvas.width = video.videoWidth;
    canvas.height = video.videoHeight;
    const ctx = canvas.getContext('2d');
    ctx.drawImage(video, 0, 0, canvas.width, canvas.height);

    // Guardamos el frame 10 como Base64
    window._tmpResult = canvas.toDataURL('image/png');
    return window._tmpResult;
}})();
"

            Dim result As String = Await web.CoreWebView2.ExecuteScriptAsync(jsCall)
            MsgBox(result) 'Esto será un data:image/png;base64,…


            Return result

        Catch ex As Exception
            MsgBox("EXCEPCIÓN VB: " & ex.Message)
            Return Nothing
        End Try
    End Function

    Public Function AddDynamicFile(originalFile As String) As String
        Dim ext = IO.Path.GetExtension(originalFile).ToLowerInvariant()
        Dim fileId = Guid.NewGuid().ToString() & ext
        MiniServer.DynamicFiles(fileId) = originalFile
        Return $"http://localhost:5000/file/{fileId}"
    End Function

    Private Sub WebView2_PermissionRequested(sender As Object, e As CoreWebView2PermissionRequestedEventArgs)
        If e.PermissionKind = CoreWebView2PermissionKind.Camera Then
            e.State = CoreWebView2PermissionState.Allow   ' ✔ permitir cámara
        End If
    End Sub

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
        web.CoreWebView2.Settings.AreHostObjectsAllowed = True

        AddHandler web.CoreWebView2.PermissionRequested, AddressOf WebView2_PermissionRequested

        web.DefaultBackgroundColor = Color.Transparent ' <-- clave
        'web.CoreWebView2.Navigate("http://localhost:5000/")
        Dim htmlPath As String = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "index.html")
        web.CoreWebView2.AddWebResourceRequestedFilter("app://bitmap/*", Microsoft.Web.WebView2.Core.CoreWebView2WebResourceContext.All)
        AddHandler web.CoreWebView2.WebResourceRequested, AddressOf OnWebResourceRequested
        ' Navegar directamente al archivo local
        web.CoreWebView2.Navigate("file:///" & htmlPath.Replace("\", "/"))
        'web.CoreWebView2.Navigate("http://localhost:5000/")
        If web Is Nothing Then
            Throw New Exception("El control WebView2 es Nothing.")
        End If

        If web.CoreWebView2 Is Nothing Then
            Throw New Exception("CoreWebView2 aún no está inicializado.")
        End If
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

    Private Sub SoftResetWebView()
        If web?.CoreWebView2 IsNot Nothing Then
            web.CoreWebView2.Reload()
        End If
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


    Public Function BitmapToBase64(
    bmp As Bitmap,
    Optional calidadJpeg As Long = 80
) As String

        Using ms As New MemoryStream()

            Dim codec = ImageCodecInfo.GetImageEncoders().
            First(Function(c) c.FormatID = ImageFormat.Jpeg.Guid)

            Dim encParams As New EncoderParameters(1)
            encParams.Param(0) = New EncoderParameter(
            System.Drawing.Imaging.Encoder.Quality,
            calidadJpeg
        )

            bmp.Save(ms, codec, encParams)

            Dim base64 = Convert.ToBase64String(ms.ToArray())
            Return "data:image/jpeg;base64," & base64

        End Using

    End Function
    Public Async Sub AgregarObjetoDisplay(
        IdGrupo As String,
        Id As String,
        Optional Url As String = "",
        Optional Ancho As Integer = 200,
        Optional Alto As Integer = 200,
        Optional PosX As Integer = 0,
        Optional PosY As Integer = 0,
        Optional NivelCapa As Integer = 0,
        Optional Opacidad As Integer = 100,
        Optional Retraso As Integer = 0,
        Optional FadeIn As Integer = 0,
        Optional FadeOut As Integer = 0,
        Optional RetrasoOut As Integer = 0,
        Optional ObjectFit As String = "contain",
        Optional Replace As Boolean = False,
        Optional Mute As Boolean = False,
        Optional LoopVideo As Boolean = False,
        Optional CierrateAlAcabar As Boolean = True,
        Optional Rotacion As Integer = 0,
        Optional VoltearHorizontal As Boolean = False,
        Optional VoltearVertical As Boolean = False
)


        If web Is Nothing Then
            Throw New Exception("El control WebView2 es Nothing.")
        End If

        If web.CoreWebView2 Is Nothing Then
            Throw New Exception("CoreWebView2 aún no está inicializado.")
        End If

        ' Crear objeto de configuración
        Dim config = New With {
        IdGrupo,
        Id,
        .Url = Url.Replace("\", "\\"),
        Ancho,
        Alto,
        PosX,
        PosY,
        NivelCapa,
        Opacidad,
        Retraso,
        FadeIn,
        FadeOut,
        RetrasoOut,
        ObjectFit,
        Replace,
        Mute,
        LoopVideo,
        CierrateAlAcabar,
        Rotacion,
        VoltearHorizontal,
        VoltearVertical
    }

        ' Serializar a JSON usando Newtonsoft
        Dim json As String = JsonConvert.SerializeObject(config)

        ' Ejecutar JS en WebView2
        Await web.CoreWebView2.ExecuteScriptAsync($"agregarObjetoDisplay({json});")


    End Sub
    Public Async Sub DLL_SetVideoBucle(id As String, valor As Boolean)
        Await web.CoreWebView2.ExecuteScriptAsync($"setVideoBucle('{id}', {valor.ToString().ToLower()});")
    End Sub

    Public Async Sub DebugHighlight(id As String)
        Await web.CoreWebView2.ExecuteScriptAsync($"debugHighlight('{id}');")
    End Sub

    Public Async Sub DLL7_OcultaGrupo(idGrupo As String)
        If web.CoreWebView2 IsNot Nothing Then
            Await web.CoreWebView2.ExecuteScriptAsync(
            $"ocultaGrupo('{idGrupo}');"
        )
        End If
    End Sub

    Public Async Sub DLL7_MostrarGrupo(idGrupo As String)
        If web.CoreWebView2 IsNot Nothing Then
            Await web.CoreWebView2.ExecuteScriptAsync(
            $"mostrarGrupo('{idGrupo}');"
        )
        End If
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

    Public Async Sub DLL_EliminarGrupo(
    idgrupo As String,
    Optional retraso As Integer = 0,
    Optional FadeOut As Integer = 0)
        Await web.CoreWebView2.ExecuteScriptAsync(
            $"eliminarPorGrupoId('{idgrupo}',{retraso} , {FadeOut});")
    End Sub

    Public Async Sub DLL_EliminaObjeto(
    id As String,
    Optional Retraso As Integer = 0,
    Optional FadeOut As Integer = 0
)
        Await web.CoreWebView2.ExecuteScriptAsync(
        $"eliminaObjeto('{id}', {Retraso} , {FadeOut});"
    )
    End Sub

    Public Async Sub DLL_PintarPunto(id As String,
                                 posX As Integer,
                                 posY As Integer,
                                 ancho As Integer,
                                 alto As Integer,
                                 grosor As String,
                                 color As String,
                                 align As Integer,
                                 Optional duracionMs As Integer = 3000)

        Dim script As String =
        $"pintarPunto('{id}', {posX}, {posY}, {ancho}, {alto}, '{grosor}', '{color}',{align}, {duracionMs});"

        Await web.CoreWebView2.ExecuteScriptAsync(script)
    End Sub

    Public Async Sub DLL_PintarCuadro(id As String,
                                  posX As Integer,
                                  posY As Integer,
                                  ancho As Integer,
                                  alto As Integer,
                                  Optional grosor As String = "2px",
                                  Optional color As String = "blue",
                                  Optional duracionMs As Integer = 3000)

        Dim script As String =
        $"pintarCuadro('{id}', {posX}, {posY}, {ancho}, {alto}, '{grosor}', '{color}', {duracionMs});"

        Await web.CoreWebView2.ExecuteScriptAsync(script)
    End Sub

    Public Async Sub DLL_BorrarGuia(id As String)
        Dim script As String = $"borrarElemento('{id}');"
        Await web.CoreWebView2.ExecuteScriptAsync(script)
    End Sub
    Public Async Sub DLL_BorrarGuias()
        Dim script As String = "borrarTodos();"
        Await web.CoreWebView2.ExecuteScriptAsync(script)
    End Sub

    Public Async Sub DLL_AgregarTexto(id As String, cfg As TextoConfig, replace As Boolean)

        If cfg Is Nothing Then
            Throw New ArgumentNullException(NameOf(cfg))
        End If

        ' Serializar sin nulls
        Dim opcionesJson = Newtonsoft.Json.JsonConvert.SerializeObject(
        cfg,
        New Newtonsoft.Json.JsonSerializerSettings With {
            .NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore
        }
    )
        Dim replaceJson = Newtonsoft.Json.JsonConvert.SerializeObject(replace)

        ' id como string JSON seguro
        Dim js = $"agregarTexto({Newtonsoft.Json.JsonConvert.ToString(id)}, {opcionesJson}, {replaceJson});"

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

