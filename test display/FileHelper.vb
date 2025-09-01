Imports System.IO

Public Module FileHelper
    Private ReadOnly tempFolder As String = Path.Combine(Path.GetTempPath(), "MiniServerUploads")

    Public Function CopyToTempAndGetUrl(originalFile As String) As String
        If Not Directory.Exists(tempFolder) Then
            Directory.CreateDirectory(tempFolder)
        End If

        Dim tempFile = Path.Combine(tempFolder, Path.GetFileName(originalFile))
        File.Copy(originalFile, tempFile, True)

        Return "http://localhost:5000/uploads/" & Path.GetFileName(tempFile)
    End Function

    Public Sub CleanTempFolder()
        If Directory.Exists(tempFolder) Then
            Try
                Directory.Delete(tempFolder, True)
            Catch ex As Exception
                Console.WriteLine("Error limpiando temporales: " & ex.Message)
            End Try
        End If
    End Sub
End Module
