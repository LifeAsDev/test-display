Imports System.IO
Imports System.Net.Mime
Imports System.Text
Imports System.Threading
Imports EmbedIO
Imports EmbedIO.Files
Imports Microsoft.Web.WebView2.Core
Imports Microsoft.Web.WebView2.WinForms

Imports EmbedIO.Utilities
Imports EmbedIO.WebApi
Imports System.Diagnostics
Public Class form_webview
    Private web As Microsoft.Web.WebView2.WinForms.WebView2
    Private server As New MiniServer()

    Private Async Sub form_webview_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Iniciar mini servidor
        server.StartServer()
        Me.FormBorderStyle = FormBorderStyle.None

        ' Crear WebView2
        web = New Microsoft.Web.WebView2.WinForms.WebView2()
        web.Dock = DockStyle.Fill
        Me.Controls.Add(web)
        Me.StartPosition = FormStartPosition.Manual
        Me.Location = New Point(0, 0)
        ' Inicializar WebView2 y navegar a localhost
        Await web.EnsureCoreWebView2Async()
        web.CoreWebView2.Navigate("http://localhost:5000/index.html")
    End Sub

    Public Async Sub MostrarSaludo(nombre As String)

        Dim path = nombre.Replace("\", "\\")  ' ← escapamos

        Await web.CoreWebView2.ExecuteScriptAsync($"saludar('{path}');")
    End Sub

    Private Sub form_webview_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        server.StopServer()
    End Sub
End Class


Public Class MiniServer
    Private server As WebServer
    Private ReadOnly tempFolder As String = Path.Combine(Path.GetTempPath(), "MiniServerUploads")

    Public Sub StartServer()
        Dim url As String = "http://localhost:5000/"

        Try
            server = New WebServer(HttpListenerMode.EmbedIO, url)
            server.WithStaticFolder("/uploads", tempFolder, True)
            ' Módulo para servir archivos estáticos (index.html, css, js)
            server.WithStaticFolder("/", AppDomain.CurrentDomain.BaseDirectory, True)

            server.RunAsync()
        Catch ex As Exception
            Debug.WriteLine("Ya hay un servidor corriendo en " & url)
            ' No lo inicias, solo sigues con la app
        End Try
    End Sub

    Private Function GetMimeType(filePath As String) As String
        Select Case Path.GetExtension(filePath).ToLower()
            Case ".png" : Return "image/png"
            Case ".jpg", ".jpeg" : Return "image/jpeg"
            Case ".gif" : Return "image/gif"
            Case ".webp" : Return "image/webp"
            Case ".bmp" : Return "image/bmp"
            Case ".mp4" : Return "video/mp4"
            Case ".avi" : Return "video/x-msvideo"
            Case Else : Return "application/octet-stream"
        End Select
    End Function

    Public Sub StopServer()
        If server IsNot Nothing Then
            server.Dispose()
        End If
    End Sub
End Class