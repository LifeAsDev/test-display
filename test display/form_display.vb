Public Class form_display
    Private Sub Display_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        l_hello.Parent = Background
        mask.Parent = Background
        lb_counter1.Parent = mask
        lb_counter2.Parent = mask
        icon1.Parent = mask
        Icon2.Parent = mask
    End Sub
End Class