<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form2
    Inherits System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.BtnAgregar = New System.Windows.Forms.Button()
        Me.NUDPosX = New System.Windows.Forms.NumericUpDown()
        Me.NUDPosY = New System.Windows.Forms.NumericUpDown()
        Me.NUDAncho = New System.Windows.Forms.NumericUpDown()
        Me.NUDAlto = New System.Windows.Forms.NumericUpDown()
        Me.NUDOpacidad = New System.Windows.Forms.NumericUpDown()
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
        Me.BtnAgregar.Text = "Agregar Objeto"
        Me.BtnAgregar.UseVisualStyleBackColor = True
        '
        'NUDPosX
        '
        Me.NUDPosX.Location = New System.Drawing.Point(10, 10)
        Me.NUDPosX.Maximum = New Decimal(New Integer() {1920, 0, 0, 0})
        Me.NUDPosX.Name = "NUDPosX"
        Me.NUDPosX.Size = New System.Drawing.Size(100, 22)
        Me.NUDPosX.TabIndex = 1
        Me.NUDPosX.Value = New Decimal(New Integer() {100, 0, 0, 0})
        '
        'NUDPosY
        '
        Me.NUDPosY.Location = New System.Drawing.Point(120, 10)
        Me.NUDPosY.Maximum = New Decimal(New Integer() {1080, 0, 0, 0})
        Me.NUDPosY.Name = "NUDPosY"
        Me.NUDPosY.Size = New System.Drawing.Size(100, 22)
        Me.NUDPosY.TabIndex = 2
        Me.NUDPosY.Value = New Decimal(New Integer() {100, 0, 0, 0})
        '
        'NUDAncho
        '
        Me.NUDAncho.Location = New System.Drawing.Point(10, 50)
        Me.NUDAncho.Maximum = New Decimal(New Integer() {1920, 0, 0, 0})
        Me.NUDAncho.Name = "NUDAncho"
        Me.NUDAncho.Size = New System.Drawing.Size(100, 22)
        Me.NUDAncho.TabIndex = 3
        '
        'NUDAlto
        '
        Me.NUDAlto.Location = New System.Drawing.Point(120, 50)
        Me.NUDAlto.Maximum = New Decimal(New Integer() {1080, 0, 0, 0})
        Me.NUDAlto.Name = "NUDAlto"
        Me.NUDAlto.Size = New System.Drawing.Size(100, 22)
        Me.NUDAlto.TabIndex = 4
        '
        'NUDOpacidad
        '
        Me.NUDOpacidad.Location = New System.Drawing.Point(10, 90)
        Me.NUDOpacidad.Maximum = New Decimal(New Integer() {100, 0, 0, 0})
        Me.NUDOpacidad.Name = "NUDOpacidad"
        Me.NUDOpacidad.Size = New System.Drawing.Size(100, 22)
        Me.NUDOpacidad.TabIndex = 5
        Me.NUDOpacidad.Value = New Decimal(New Integer() {100, 0, 0, 0})
        '
        'Form2
        '
        Me.ClientSize = New System.Drawing.Size(250, 180)
        Me.Controls.Add(Me.NUDOpacidad)
        Me.Controls.Add(Me.NUDAlto)
        Me.Controls.Add(Me.NUDAncho)
        Me.Controls.Add(Me.NUDPosY)
        Me.Controls.Add(Me.NUDPosX)
        Me.Controls.Add(Me.BtnAgregar)
        Me.Name = "Form2"
        Me.Text = "Agregar Objeto Display"
        CType(Me.NUDPosX, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NUDPosY, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NUDAncho, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NUDAlto, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NUDOpacidad, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents BtnAgregar As Button
    Friend WithEvents NUDPosX As NumericUpDown
    Friend WithEvents NUDPosY As NumericUpDown
    Friend WithEvents NUDAncho As NumericUpDown
    Friend WithEvents NUDAlto As NumericUpDown
    Friend WithEvents NUDOpacidad As NumericUpDown
End Class
