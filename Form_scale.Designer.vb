<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form_scale
    Inherits System.Windows.Forms.Form

    'Form 重写 Dispose，以清理组件列表。
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

    'Windows 窗体设计器所必需的
    Private components As System.ComponentModel.IContainer

    '注意: 以下过程是 Windows 窗体设计器所必需的
    '可以使用 Windows 窗体设计器修改它。  
    '不要使用代码编辑器修改它。
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        components = New ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form_scale))
        TextBox1 = New TextBox()
        TextBox2 = New TextBox()
        ContextMenu1 = New ContextMenuStrip(components)
        MenuItem1 = New ToolStripMenuItem()
        MenuItem2 = New ToolStripMenuItem()
        ComboBox1 = New ComboBox()
        Button2 = New Button()
        ListView1 = New ListView()
        ColumnHeader1 = New ColumnHeader()
        ColumnHeader2 = New ColumnHeader()
        ColumnHeader3 = New ColumnHeader()
        ColumnHeader4 = New ColumnHeader()
        ColumnHeader5 = New ColumnHeader()
        ColumnHeader6 = New ColumnHeader()
        Button3 = New Button()
        Label3 = New Label()
        Label5 = New Label()
        Label1 = New Label()
        Label2 = New Label()
        TextBox0 = New TextBox()
        Label4 = New Label()
        Label6 = New Label()
        TextBox5 = New TextBox()
        TextBox4 = New TextBox()
        Button5 = New Button()
        MenuStrip1 = New MenuStrip()
        OperationToolStripMenuItem = New ToolStripMenuItem()
        AddToolStripMenuItem = New ToolStripMenuItem()
        DeleteToolStripMenuItem = New ToolStripMenuItem()
        Button1 = New Button()
        ContextMenu1.SuspendLayout()
        MenuStrip1.SuspendLayout()
        SuspendLayout()
        ' 
        ' TextBox1
        ' 
        TextBox1.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        TextBox1.Location = New Point(120, 290)
        TextBox1.Margin = New Padding(3, 4, 3, 4)
        TextBox1.Name = "TextBox1"
        TextBox1.Size = New Size(74, 25)
        TextBox1.TabIndex = 11
        TextBox1.Text = "1"
        ' 
        ' TextBox2
        ' 
        TextBox2.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        TextBox2.Location = New Point(120, 324)
        TextBox2.Margin = New Padding(3, 4, 3, 4)
        TextBox2.Name = "TextBox2"
        TextBox2.Size = New Size(74, 25)
        TextBox2.TabIndex = 12
        TextBox2.Text = "10"
        ' 
        ' ContextMenu1
        ' 
        ContextMenu1.Items.AddRange(New ToolStripItem() {MenuItem1, MenuItem2})
        ContextMenu1.Name = "ContextMenu1"
        ContextMenu1.Size = New Size(124, 48)
        ' 
        ' MenuItem1
        ' 
        MenuItem1.Name = "MenuItem1"
        MenuItem1.Size = New Size(123, 22)
        MenuItem1.Text = "Delete"
        ' 
        ' MenuItem2
        ' 
        MenuItem2.Name = "MenuItem2"
        MenuItem2.Size = New Size(123, 22)
        MenuItem2.Text = "Rename"
        ' 
        ' ComboBox1
        ' 
        ComboBox1.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        ComboBox1.Items.AddRange(New Object() {"um", "mm", "cm", "px"})
        ComboBox1.Location = New Point(256, 323)
        ComboBox1.Margin = New Padding(3, 4, 3, 4)
        ComboBox1.Name = "ComboBox1"
        ComboBox1.Size = New Size(74, 27)
        ComboBox1.TabIndex = 21
        ' 
        ' Button2
        ' 
        Button2.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        Button2.Location = New Point(174, 367)
        Button2.Margin = New Padding(3, 4, 3, 4)
        Button2.Name = "Button2"
        Button2.Size = New Size(73, 31)
        Button2.TabIndex = 20
        Button2.Text = "Apply"
        ' 
        ' ListView1
        ' 
        ListView1.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        ListView1.Columns.AddRange(New ColumnHeader() {ColumnHeader1, ColumnHeader2, ColumnHeader3, ColumnHeader4, ColumnHeader5, ColumnHeader6})
        ListView1.ContextMenuStrip = ContextMenu1
        ListView1.FullRowSelect = True
        ListView1.HeaderStyle = ColumnHeaderStyle.Nonclickable
        ListView1.Location = New Point(0, 32)
        ListView1.Margin = New Padding(3, 4, 3, 4)
        ListView1.MultiSelect = False
        ListView1.Name = "ListView1"
        ListView1.Size = New Size(414, 218)
        ListView1.TabIndex = 16
        ListView1.UseCompatibleStateImageBehavior = False
        ListView1.View = View.Details
        ' 
        ' ColumnHeader1
        ' 
        ColumnHeader1.Text = "Title"
        ColumnHeader1.Width = 80
        ' 
        ' ColumnHeader2
        ' 
        ColumnHeader2.Text = "Pixel"
        ' 
        ' ColumnHeader3
        ' 
        ColumnHeader3.Text = "Length"
        ' 
        ' ColumnHeader4
        ' 
        ColumnHeader4.Text = "Unit"
        ColumnHeader4.Width = 50
        ' 
        ' ColumnHeader5
        ' 
        ColumnHeader5.Text = "Size"
        ColumnHeader5.Width = 50
        ' 
        ' ColumnHeader6
        ' 
        ColumnHeader6.Text = "Radio"
        ColumnHeader6.Width = 50
        ' 
        ' Button3
        ' 
        Button3.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        Button3.Location = New Point(11, 367)
        Button3.Margin = New Padding(3, 4, 3, 4)
        Button3.Name = "Button3"
        Button3.Size = New Size(73, 31)
        Button3.TabIndex = 22
        Button3.Text = "Reset"
        ' 
        ' Label3
        ' 
        Label3.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        Label3.AutoSize = True
        Label3.Location = New Point(205, 328)
        Label3.Name = "Label3"
        Label3.Size = New Size(35, 19)
        Label3.TabIndex = 24
        Label3.Text = "Unit"
        ' 
        ' Label5
        ' 
        Label5.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        Label5.AutoSize = True
        Label5.Location = New Point(8, 293)
        Label5.Name = "Label5"
        Label5.Size = New Size(93, 19)
        Label5.TabIndex = 25
        Label5.Text = "Width in pixel"
        ' 
        ' Label1
        ' 
        Label1.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        Label1.AutoSize = True
        Label1.Location = New Point(8, 328)
        Label1.Name = "Label1"
        Label1.Size = New Size(91, 19)
        Label1.TabIndex = 26
        Label1.Text = "Actual length"
        ' 
        ' Label2
        ' 
        Label2.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        Label2.AutoSize = True
        Label2.Location = New Point(205, 293)
        Label2.Name = "Label2"
        Label2.Size = New Size(33, 19)
        Label2.TabIndex = 27
        Label2.Text = "Size"
        ' 
        ' TextBox0
        ' 
        TextBox0.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        TextBox0.Location = New Point(51, 256)
        TextBox0.Margin = New Padding(3, 4, 3, 4)
        TextBox0.Name = "TextBox0"
        TextBox0.Size = New Size(142, 25)
        TextBox0.TabIndex = 28
        ' 
        ' Label4
        ' 
        Label4.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        Label4.AutoSize = True
        Label4.Location = New Point(8, 260)
        Label4.Name = "Label4"
        Label4.Size = New Size(34, 19)
        Label4.TabIndex = 29
        Label4.Text = "Title"
        ' 
        ' Label6
        ' 
        Label6.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        Label6.AutoSize = True
        Label6.Location = New Point(205, 260)
        Label6.Name = "Label6"
        Label6.Size = New Size(43, 19)
        Label6.TabIndex = 30
        Label6.Text = "Radio"
        ' 
        ' TextBox5
        ' 
        TextBox5.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        TextBox5.Location = New Point(256, 256)
        TextBox5.Margin = New Padding(3, 4, 3, 4)
        TextBox5.Name = "TextBox5"
        TextBox5.ReadOnly = True
        TextBox5.Size = New Size(74, 25)
        TextBox5.TabIndex = 31
        TextBox5.Text = "1"
        ' 
        ' TextBox4
        ' 
        TextBox4.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        TextBox4.Location = New Point(256, 290)
        TextBox4.Margin = New Padding(3, 4, 3, 4)
        TextBox4.Name = "TextBox4"
        TextBox4.Size = New Size(74, 25)
        TextBox4.TabIndex = 32
        TextBox4.Text = "10"
        ' 
        ' Button5
        ' 
        Button5.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        Button5.Location = New Point(93, 367)
        Button5.Margin = New Padding(3, 4, 3, 4)
        Button5.Name = "Button5"
        Button5.Size = New Size(73, 31)
        Button5.TabIndex = 33
        Button5.Text = "Save"
        ' 
        ' MenuStrip1
        ' 
        MenuStrip1.Font = New Font("微软雅黑", 10.5F, FontStyle.Regular, GraphicsUnit.Point)
        MenuStrip1.Items.AddRange(New ToolStripItem() {OperationToolStripMenuItem})
        MenuStrip1.Location = New Point(0, 0)
        MenuStrip1.Name = "MenuStrip1"
        MenuStrip1.Padding = New Padding(7, 2, 0, 2)
        MenuStrip1.Size = New Size(416, 28)
        MenuStrip1.TabIndex = 34
        MenuStrip1.Text = "MenuStrip1"
        ' 
        ' OperationToolStripMenuItem
        ' 
        OperationToolStripMenuItem.DropDownItems.AddRange(New ToolStripItem() {AddToolStripMenuItem, DeleteToolStripMenuItem})
        OperationToolStripMenuItem.Name = "OperationToolStripMenuItem"
        OperationToolStripMenuItem.Size = New Size(89, 24)
        OperationToolStripMenuItem.Text = "Operation"
        ' 
        ' AddToolStripMenuItem
        ' 
        AddToolStripMenuItem.Name = "AddToolStripMenuItem"
        AddToolStripMenuItem.Size = New Size(147, 24)
        AddToolStripMenuItem.Text = "New Scale"
        ' 
        ' DeleteToolStripMenuItem
        ' 
        DeleteToolStripMenuItem.Name = "DeleteToolStripMenuItem"
        DeleteToolStripMenuItem.Size = New Size(147, 24)
        DeleteToolStripMenuItem.Text = "Delete"
        ' 
        ' Button1
        ' 
        Button1.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        Button1.Location = New Point(255, 367)
        Button1.Margin = New Padding(3, 4, 3, 4)
        Button1.Name = "Button1"
        Button1.Size = New Size(73, 31)
        Button1.TabIndex = 35
        Button1.Text = "Close"
        ' 
        ' Form_scale
        ' 
        AutoScaleDimensions = New SizeF(8F, 19F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(416, 404)
        ControlBox = False
        Controls.Add(Button1)
        Controls.Add(Button5)
        Controls.Add(TextBox4)
        Controls.Add(TextBox5)
        Controls.Add(Label6)
        Controls.Add(Label4)
        Controls.Add(TextBox0)
        Controls.Add(Label2)
        Controls.Add(Label1)
        Controls.Add(Label5)
        Controls.Add(Label3)
        Controls.Add(TextBox1)
        Controls.Add(TextBox2)
        Controls.Add(Button3)
        Controls.Add(ComboBox1)
        Controls.Add(Button2)
        Controls.Add(ListView1)
        Controls.Add(MenuStrip1)
        Font = New Font("微软雅黑", 9.75F, FontStyle.Regular, GraphicsUnit.Point)
        FormBorderStyle = FormBorderStyle.FixedToolWindow
        Icon = CType(resources.GetObject("$this.Icon"), Icon)
        MainMenuStrip = MenuStrip1
        Margin = New Padding(3, 5, 3, 5)
        Name = "Form_scale"
        Text = "Scale"
        TopMost = True
        ContextMenu1.ResumeLayout(False)
        MenuStrip1.ResumeLayout(False)
        MenuStrip1.PerformLayout()
        ResumeLayout(False)
        PerformLayout()

    End Sub
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents TextBox2 As TextBox
    ' TODO ContextMenu is no longer supported. Use ContextMenuStrip instead. For more details see https://docs.microsoft.com/en-us/dotnet/core/compatibility/winforms#removed-controls
    Friend WithEvents ContextMenu1 As ContextMenuStrip
    ' TODO MenuItem is no longer supported. Use ToolStripMenuItem instead. For more details see https://docs.microsoft.com/en-us/dotnet/core/compatibility/winforms#removed-controls
    Friend WithEvents MenuItem1 As ToolStripMenuItem
    ' TODO MenuItem is no longer supported. Use ToolStripMenuItem instead. For more details see https://docs.microsoft.com/en-us/dotnet/core/compatibility/winforms#removed-controls
    Friend WithEvents MenuItem2 As ToolStripMenuItem
    Friend WithEvents ComboBox1 As ComboBox
    Friend WithEvents Button2 As Button
    Friend WithEvents ListView1 As ListView
    Friend WithEvents ColumnHeader1 As ColumnHeader
    Friend WithEvents ColumnHeader2 As ColumnHeader
    Friend WithEvents ColumnHeader3 As ColumnHeader
    Friend WithEvents ColumnHeader4 As ColumnHeader
    Friend WithEvents ColumnHeader5 As ColumnHeader
    Friend WithEvents ColumnHeader6 As ColumnHeader
    Friend WithEvents Button3 As Button
    Friend WithEvents Label3 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents TextBox0 As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents TextBox5 As TextBox
    Friend WithEvents TextBox4 As TextBox
    Friend WithEvents Button5 As Button
    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents OperationToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents AddToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents DeleteToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents Button1 As Button
End Class
