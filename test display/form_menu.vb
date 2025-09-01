Public Class form_menu
    Private webForm As form_webview


    Private Sub form_menu_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'form_display.Show()
        ' form_display.Left = 0
        form_display.Top = 0

        Me.Left = form_display.Width + 20
        Me.Top = 50

        webForm = New form_webview
        webForm.Left = Me.Left + Me.Width + 20
        webForm.Top = 50
        webForm.Show()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        form_display.lb_counter1.Left = 100 + CInt(Int((400 * Rnd())))
        form_display.lb_counter1.Top = 100 + CInt(Int((200 * Rnd())))
        form_display.lb_counter2.Left = 100 + CInt(Int((400 * Rnd())))
        form_display.lb_counter2.Top = 100 + CInt(Int((200 * Rnd())))
        form_display.l_hello.Left = 50 + CInt(Int((400 * Rnd())))
        form_display.l_hello.Top = 50 + CInt(Int((200 * Rnd())))

        form_display.icon1.Left = 100 + CInt(Int((400 * Rnd())))
        form_display.icon1.Top = 100 + CInt(Int((200 * Rnd())))
        form_display.Icon2.Left = 100 + CInt(Int((400 * Rnd())))
        form_display.Icon2.Top = 100 + CInt(Int((200 * Rnd())))



    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        form_display.lb_counter1.Text = form_display.lb_counter1.Text + 1
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        form_display.lb_counter2.Text = form_display.lb_counter2.Text + 1
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged

    End Sub

    Private Sub TextBox1_Validated(sender As Object, e As EventArgs) Handles TextBox1.Validated
        form_display.l_hello.Text = TextBox1.Text
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Dim filename As String
        Dim openfiler As New OpenFileDialog
        With openfiler
            .InitialDirectory = "C:\"
            .Filter = "Imágenes y Videos (*.png;*.jpg;*.jpeg;*.gif;*.webm;*.mp4)|*.png;*.jpg;*.jpeg;*.gif;*.webm;*.mp4"
            .FilterIndex = 1
            .RestoreDirectory = True
        End With

        If openfiler.ShowDialog = Windows.Forms.DialogResult.OK Then
            filename = openfiler.FileName
            Dim url = CopyToTempAndGetUrl(openfiler.FileName)

            webForm.MostrarSaludo(url)

        End If

    End Sub

    Private Sub Form_menu_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        ' Limpieza al cerrar
        CleanTempFolder()
    End Sub


    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Dim filename As String
        Dim openfiler As New OpenFileDialog
        With openfiler
            .InitialDirectory = "C:\"
            .Filter = "Archivos de imágen(*.png)|*.png|Archivos de imágen(*.jpg)|*.jpg|All Files (*.*)|*.*"
            .FilterIndex = 1
            .RestoreDirectory = True
        End With

        If openfiler.ShowDialog = Windows.Forms.DialogResult.OK Then
            filename = openfiler.FileName
            form_display.icon1.Image = Image.FromFile(filename)
        End If
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Dim filename As String
        Dim openfiler As New OpenFileDialog
        With openfiler
            .InitialDirectory = "C:\"
            .Filter = "Archivos de imágen(*.png)|*.png|Archivos de imágen(*.jpg)|*.jpg|All Files (*.*)|*.*"
            .FilterIndex = 1
            .RestoreDirectory = True
        End With

        If openfiler.ShowDialog = Windows.Forms.DialogResult.OK Then
            filename = openfiler.FileName
            form_display.Icon2.Image = Image.FromFile(filename)
        End If
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click

        form_display.l_hello.Font = New Font("Arial", 5 + CInt(Int((40 * Rnd()))))
        form_display.lb_counter1.Font = New Font("Arial", 10 + CInt(Int((70 * Rnd()))))
        form_display.lb_counter2.Font = New Font("Arial", 10 + CInt(Int((70 * Rnd()))))


        form_display.icon1.Width = 50 + CInt(Int((200 * Rnd())))
        form_display.icon1.Height = 50 + CInt(Int((200 * Rnd())))
        form_display.Icon2.Width = 50 + CInt(Int((200 * Rnd())))
        form_display.Icon2.Height = 50 + CInt(Int((200 * Rnd())))

    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        form_display.icon1.Visible = Not form_display.icon1.Visible
        form_display.lb_counter1.Visible = form_display.icon1.Visible
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        form_display.Icon2.Visible = Not form_display.Icon2.Visible
        form_display.lb_counter2.Visible = form_display.Icon2.Visible
    End Sub
End Class
