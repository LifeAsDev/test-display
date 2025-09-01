<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class form_display
    Inherits System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento
    'Se puede modificar usando el Diseñador de Windows Forms.  
    'No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(form_display))
        Me.l_hello = New System.Windows.Forms.Label()
        Me.lb_counter1 = New System.Windows.Forms.Label()
        Me.Background = New System.Windows.Forms.PictureBox()
        Me.icon1 = New System.Windows.Forms.PictureBox()
        Me.Icon2 = New System.Windows.Forms.PictureBox()
        Me.lb_counter2 = New System.Windows.Forms.Label()
        Me.mask = New System.Windows.Forms.PictureBox()
        CType(Me.Background, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.icon1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Icon2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.mask, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'l_hello
        '
        Me.l_hello.AutoSize = True
        Me.l_hello.BackColor = System.Drawing.Color.Transparent
        Me.l_hello.Font = New System.Drawing.Font("Microsoft Sans Serif", 45.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.l_hello.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.l_hello.Location = New System.Drawing.Point(140, 9)
        Me.l_hello.Name = "l_hello"
        Me.l_hello.Size = New System.Drawing.Size(348, 69)
        Me.l_hello.TabIndex = 0
        Me.l_hello.Text = "Hello World"
        '
        'lb_counter1
        '
        Me.lb_counter1.AutoSize = True
        Me.lb_counter1.BackColor = System.Drawing.Color.Transparent
        Me.lb_counter1.Font = New System.Drawing.Font("Microsoft Sans Serif", 70.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lb_counter1.ForeColor = System.Drawing.Color.Red
        Me.lb_counter1.Location = New System.Drawing.Point(337, 121)
        Me.lb_counter1.Name = "lb_counter1"
        Me.lb_counter1.Size = New System.Drawing.Size(151, 107)
        Me.lb_counter1.TabIndex = 1
        Me.lb_counter1.Text = "22"
        '
        'Background
        '
        Me.Background.Image = CType(resources.GetObject("Background.Image"), System.Drawing.Image)
        Me.Background.Location = New System.Drawing.Point(2, -2)
        Me.Background.Name = "Background"
        Me.Background.Size = New System.Drawing.Size(625, 451)
        Me.Background.TabIndex = 2
        Me.Background.TabStop = False
        '
        'icon1
        '
        Me.icon1.BackColor = System.Drawing.Color.Transparent
        Me.icon1.Image = CType(resources.GetObject("icon1.Image"), System.Drawing.Image)
        Me.icon1.Location = New System.Drawing.Point(124, 104)
        Me.icon1.Name = "icon1"
        Me.icon1.Size = New System.Drawing.Size(187, 133)
        Me.icon1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.icon1.TabIndex = 3
        Me.icon1.TabStop = False
        '
        'Icon2
        '
        Me.Icon2.BackColor = System.Drawing.Color.Transparent
        Me.Icon2.Image = CType(resources.GetObject("Icon2.Image"), System.Drawing.Image)
        Me.Icon2.Location = New System.Drawing.Point(124, 268)
        Me.Icon2.Name = "Icon2"
        Me.Icon2.Size = New System.Drawing.Size(187, 131)
        Me.Icon2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.Icon2.TabIndex = 4
        Me.Icon2.TabStop = False
        '
        'lb_counter2
        '
        Me.lb_counter2.AutoSize = True
        Me.lb_counter2.BackColor = System.Drawing.Color.Transparent
        Me.lb_counter2.Font = New System.Drawing.Font("Microsoft Sans Serif", 70.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lb_counter2.ForeColor = System.Drawing.Color.Red
        Me.lb_counter2.Location = New System.Drawing.Point(337, 279)
        Me.lb_counter2.Name = "lb_counter2"
        Me.lb_counter2.Size = New System.Drawing.Size(151, 107)
        Me.lb_counter2.TabIndex = 5
        Me.lb_counter2.Text = "45"
        '
        'mask
        '
        Me.mask.BackColor = System.Drawing.Color.Transparent
        Me.mask.Image = CType(resources.GetObject("mask.Image"), System.Drawing.Image)
        Me.mask.Location = New System.Drawing.Point(2, -2)
        Me.mask.Name = "mask"
        Me.mask.Size = New System.Drawing.Size(625, 451)
        Me.mask.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.mask.TabIndex = 6
        Me.mask.TabStop = False
        '
        'form_display
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(624, 441)
        Me.Controls.Add(Me.lb_counter2)
        Me.Controls.Add(Me.Icon2)
        Me.Controls.Add(Me.icon1)
        Me.Controls.Add(Me.lb_counter1)
        Me.Controls.Add(Me.l_hello)
        Me.Controls.Add(Me.mask)
        Me.Controls.Add(Me.Background)
        Me.Name = "form_display"
        Me.Text = "Display"
        CType(Me.Background, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.icon1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Icon2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.mask, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents l_hello As Label
    Friend WithEvents lb_counter1 As Label
    Friend WithEvents Background As PictureBox
    Friend WithEvents icon1 As PictureBox
    Friend WithEvents Icon2 As PictureBox
    Friend WithEvents lb_counter2 As Label
    Friend WithEvents mask As PictureBox
End Class
