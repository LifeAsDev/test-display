<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form2
    Inherits System.Windows.Forms.Form

    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.BtnAgregar = New System.Windows.Forms.Button()
        Me.NUDPosX = New System.Windows.Forms.NumericUpDown()
        Me.NUDPosY = New System.Windows.Forms.NumericUpDown()
        Me.NUDAncho = New System.Windows.Forms.NumericUpDown()
        Me.NUDAlto = New System.Windows.Forms.NumericUpDown()
        Me.NUDOpacidad = New System.Windows.Forms.NumericUpDown()
        Me.TxtContenido = New System.Windows.Forms.TextBox()
        Me.BtnTexto = New System.Windows.Forms.Button()
        Me.BtnClear = New System.Windows.Forms.Button()
        Me.ComboBox1 = New System.Windows.Forms.ComboBox()
        Me.Button1 = New System.Windows.Forms.Button()
        CType(Me.NUDPosX, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NUDPosY, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NUDAncho, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NUDAlto, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NUDOpacidad, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'BtnAgregar
        '
        Me.BtnAgregar.Location = New System.Drawing.Point(10, 130)
        Me.BtnAgregar.Name = "BtnAgregar"
        Me.BtnAgregar.Size = New System.Drawing.Size(120, 30)
        Me.BtnAgregar.TabIndex = 0
        Me.BtnAgregar.Text = "Agregar Media"
        Me.BtnAgregar.UseVisualStyleBackColor = True
        '
        'NUDPosX
        '
        Me.NUDPosX.Location = New System.Drawing.Point(10, 10)
        Me.NUDPosX.Maximum = New Decimal(New Integer() {1920, 0, 0, 0})
        Me.NUDPosX.Name = "NUDPosX"
        Me.NUDPosX.Size = New System.Drawing.Size(100, 20)
        Me.NUDPosX.TabIndex = 1
        Me.NUDPosX.Value = New Decimal(New Integer() {100, 0, 0, 0})
        '
        'NUDPosY
        '
        Me.NUDPosY.Location = New System.Drawing.Point(120, 10)
        Me.NUDPosY.Maximum = New Decimal(New Integer() {1080, 0, 0, 0})
        Me.NUDPosY.Name = "NUDPosY"
        Me.NUDPosY.Size = New System.Drawing.Size(100, 20)
        Me.NUDPosY.TabIndex = 2
        Me.NUDPosY.Value = New Decimal(New Integer() {100, 0, 0, 0})
        '
        'NUDAncho
        '
        Me.NUDAncho.Location = New System.Drawing.Point(10, 50)
        Me.NUDAncho.Maximum = New Decimal(New Integer() {1920, 0, 0, 0})
        Me.NUDAncho.Name = "NUDAncho"
        Me.NUDAncho.Size = New System.Drawing.Size(100, 20)
        Me.NUDAncho.TabIndex = 3
        '
        'NUDAlto
        '
        Me.NUDAlto.Location = New System.Drawing.Point(120, 50)
        Me.NUDAlto.Maximum = New Decimal(New Integer() {1080, 0, 0, 0})
        Me.NUDAlto.Name = "NUDAlto"
        Me.NUDAlto.Size = New System.Drawing.Size(100, 20)
        Me.NUDAlto.TabIndex = 4
        '
        'NUDOpacidad
        '
        Me.NUDOpacidad.Location = New System.Drawing.Point(10, 90)
        Me.NUDOpacidad.Name = "NUDOpacidad"
        Me.NUDOpacidad.Size = New System.Drawing.Size(100, 20)
        Me.NUDOpacidad.TabIndex = 5
        Me.NUDOpacidad.Value = New Decimal(New Integer() {100, 0, 0, 0})
        '
        'TxtContenido
        '
        Me.TxtContenido.Location = New System.Drawing.Point(10, 170)
        Me.TxtContenido.Name = "TxtContenido"
        Me.TxtContenido.Size = New System.Drawing.Size(210, 20)
        Me.TxtContenido.TabIndex = 6
        '
        'BtnTexto
        '
        Me.BtnTexto.Location = New System.Drawing.Point(10, 200)
        Me.BtnTexto.Name = "BtnTexto"
        Me.BtnTexto.Size = New System.Drawing.Size(120, 30)
        Me.BtnTexto.TabIndex = 7
        Me.BtnTexto.Text = "Agregar Texto"
        Me.BtnTexto.UseVisualStyleBackColor = True
        '
        'BtnClear
        '
        Me.BtnClear.Location = New System.Drawing.Point(136, 130)
        Me.BtnClear.Name = "BtnClear"
        Me.BtnClear.Size = New System.Drawing.Size(102, 30)
        Me.BtnClear.TabIndex = 0
        Me.BtnClear.Text = "Clear All"
        Me.BtnClear.UseVisualStyleBackColor = True
        '
        'ComboBox1
        '
        Me.ComboBox1.DisplayMember = "0"
        Me.ComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox1.FormattingEnabled = True
        Me.ComboBox1.Items.AddRange(New Object() {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12"})
        Me.ComboBox1.Location = New System.Drawing.Point(136, 206)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(102, 21)
        Me.ComboBox1.TabIndex = 8
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(10, 236)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(120, 27)
        Me.Button1.TabIndex = 9
        Me.Button1.Text = "Prueba Media x10"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Form2
        '
        Me.ClientSize = New System.Drawing.Size(250, 281)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.ComboBox1)
        Me.Controls.Add(Me.BtnTexto)
        Me.Controls.Add(Me.TxtContenido)
        Me.Controls.Add(Me.NUDOpacidad)
        Me.Controls.Add(Me.NUDAlto)
        Me.Controls.Add(Me.NUDAncho)
        Me.Controls.Add(Me.NUDPosY)
        Me.Controls.Add(Me.NUDPosX)
        Me.Controls.Add(Me.BtnAgregar)
        Me.Controls.Add(Me.BtnClear)
        Me.Name = "Form2"
        Me.Text = "Agregar Objeto Display"
        CType(Me.NUDPosX, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NUDPosY, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NUDAncho, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NUDAlto, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NUDOpacidad, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents BtnAgregar As Button
    Friend WithEvents NUDPosX As NumericUpDown
    Friend WithEvents NUDPosY As NumericUpDown
    Friend WithEvents NUDAncho As NumericUpDown
    Friend WithEvents NUDAlto As NumericUpDown
    Friend WithEvents NUDOpacidad As NumericUpDown
    Friend WithEvents TxtContenido As TextBox
    Friend WithEvents BtnTexto As Button
    Friend WithEvents BtnClear As Button
    Friend WithEvents ComboBox1 As ComboBox
    Friend WithEvents Button1 As Button
End Class
