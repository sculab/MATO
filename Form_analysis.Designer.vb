<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form_analysis
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form_analysis))
        IdiogramToolStripMenuItem = New ToolStripMenuItem()
        SaveResultToolStripMenuItem = New ToolStripMenuItem()
        SaveGraphicToolStripMenuItem = New ToolStripMenuItem()
        SaveToolStripMenuItem = New ToolStripMenuItem()
        FileToolStripMenuItem = New ToolStripMenuItem()
        MenuStrip1 = New MenuStrip()
        ViewToolStripMenuItem = New ToolStripMenuItem()
        PictureBox1 = New PictureBox()
        MenuItem3 = New ToolStripMenuItem()
        MenuItem4 = New ToolStripMenuItem()
        MenuItem2 = New ToolStripMenuItem()
        ContextMenu2 = New ContextMenuStrip(components)
        MenuItem1 = New ToolStripMenuItem()
        ContextMenu1 = New ContextMenuStrip(components)
        RichTextBox1 = New RichTextBox()
        TextBox1 = New TextBox()
        Button1 = New Button()
        TextBox2 = New TextBox()
        GroupBox1 = New GroupBox()
        CheckBox1 = New CheckBox()
        SplitContainer1 = New SplitContainer()
        GroupBox2 = New GroupBox()
        NumericUpDown3 = New NumericUpDown()
        Label3 = New Label()
        Button2 = New Button()
        NumericUpDown2 = New NumericUpDown()
        NumericUpDown1 = New NumericUpDown()
        Label2 = New Label()
        Label1 = New Label()
        MenuStrip1.SuspendLayout()
        CType(PictureBox1, ComponentModel.ISupportInitialize).BeginInit()
        ContextMenu2.SuspendLayout()
        ContextMenu1.SuspendLayout()
        GroupBox1.SuspendLayout()
        CType(SplitContainer1, ComponentModel.ISupportInitialize).BeginInit()
        SplitContainer1.Panel1.SuspendLayout()
        SplitContainer1.Panel2.SuspendLayout()
        SplitContainer1.SuspendLayout()
        GroupBox2.SuspendLayout()
        CType(NumericUpDown3, ComponentModel.ISupportInitialize).BeginInit()
        CType(NumericUpDown2, ComponentModel.ISupportInitialize).BeginInit()
        CType(NumericUpDown1, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' IdiogramToolStripMenuItem
        ' 
        IdiogramToolStripMenuItem.Name = "IdiogramToolStripMenuItem"
        IdiogramToolStripMenuItem.Size = New Size(168, 28)
        IdiogramToolStripMenuItem.Text = "Idiogram"
        ' 
        ' SaveResultToolStripMenuItem
        ' 
        SaveResultToolStripMenuItem.Name = "SaveResultToolStripMenuItem"
        SaveResultToolStripMenuItem.Size = New Size(200, 28)
        SaveResultToolStripMenuItem.Text = "Save Result"
        ' 
        ' SaveGraphicToolStripMenuItem
        ' 
        SaveGraphicToolStripMenuItem.Name = "SaveGraphicToolStripMenuItem"
        SaveGraphicToolStripMenuItem.Size = New Size(200, 28)
        SaveGraphicToolStripMenuItem.Text = "Save Graphic"
        ' 
        ' SaveToolStripMenuItem
        ' 
        SaveToolStripMenuItem.DropDownItems.AddRange(New ToolStripItem() {SaveGraphicToolStripMenuItem, SaveResultToolStripMenuItem})
        SaveToolStripMenuItem.Name = "SaveToolStripMenuItem"
        SaveToolStripMenuItem.Size = New Size(132, 28)
        SaveToolStripMenuItem.Text = "Save"
        ' 
        ' FileToolStripMenuItem
        ' 
        FileToolStripMenuItem.DropDownItems.AddRange(New ToolStripItem() {SaveToolStripMenuItem})
        FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        FileToolStripMenuItem.Size = New Size(53, 27)
        FileToolStripMenuItem.Text = "File"
        ' 
        ' MenuStrip1
        ' 
        MenuStrip1.Font = New Font("微软雅黑", 9.75F, FontStyle.Regular, GraphicsUnit.Point)
        MenuStrip1.ImageScalingSize = New Size(20, 20)
        MenuStrip1.Items.AddRange(New ToolStripItem() {FileToolStripMenuItem, ViewToolStripMenuItem})
        MenuStrip1.Location = New Point(0, 0)
        MenuStrip1.Name = "MenuStrip1"
        MenuStrip1.Padding = New Padding(7, 2, 0, 2)
        MenuStrip1.Size = New Size(850, 31)
        MenuStrip1.TabIndex = 12
        MenuStrip1.Text = "MenuStrip1"
        ' 
        ' ViewToolStripMenuItem
        ' 
        ViewToolStripMenuItem.DropDownItems.AddRange(New ToolStripItem() {IdiogramToolStripMenuItem})
        ViewToolStripMenuItem.Name = "ViewToolStripMenuItem"
        ViewToolStripMenuItem.Size = New Size(64, 27)
        ViewToolStripMenuItem.Text = "View"
        ' 
        ' PictureBox1
        ' 
        PictureBox1.BackColor = Color.Transparent
        PictureBox1.BorderStyle = BorderStyle.FixedSingle
        PictureBox1.Location = New Point(0, 0)
        PictureBox1.Margin = New Padding(3, 4, 3, 4)
        PictureBox1.Name = "PictureBox1"
        PictureBox1.Size = New Size(818, 356)
        PictureBox1.TabIndex = 0
        PictureBox1.TabStop = False
        ' 
        ' MenuItem3
        ' 
        MenuItem3.Name = "MenuItem3"
        MenuItem3.Size = New Size(172, 24)
        MenuItem3.Text = "Refreash(&F)"
        ' 
        ' MenuItem4
        ' 
        MenuItem4.Name = "MenuItem4"
        MenuItem4.Size = New Size(172, 24)
        MenuItem4.Text = "Save Graphic"
        ' 
        ' MenuItem2
        ' 
        MenuItem2.Name = "MenuItem2"
        MenuItem2.Size = New Size(172, 24)
        MenuItem2.Text = "Idiogram"
        ' 
        ' ContextMenu2
        ' 
        ContextMenu2.ImageScalingSize = New Size(20, 20)
        ContextMenu2.Items.AddRange(New ToolStripItem() {MenuItem2, MenuItem4, MenuItem3})
        ContextMenu2.Name = "ContextMenu2"
        ContextMenu2.Size = New Size(173, 76)
        ' 
        ' MenuItem1
        ' 
        MenuItem1.Name = "MenuItem1"
        MenuItem1.Size = New Size(164, 24)
        MenuItem1.Text = "Save results"
        ' 
        ' ContextMenu1
        ' 
        ContextMenu1.ImageScalingSize = New Size(20, 20)
        ContextMenu1.Items.AddRange(New ToolStripItem() {MenuItem1})
        ContextMenu1.Name = "ContextMenu1"
        ContextMenu1.Size = New Size(165, 28)
        ' 
        ' RichTextBox1
        ' 
        RichTextBox1.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        RichTextBox1.ContextMenuStrip = ContextMenu1
        RichTextBox1.Location = New Point(272, 4)
        RichTextBox1.Margin = New Padding(3, 4, 3, 4)
        RichTextBox1.Name = "RichTextBox1"
        RichTextBox1.Size = New Size(574, 242)
        RichTextBox1.TabIndex = 9
        RichTextBox1.Text = ""
        ' 
        ' TextBox1
        ' 
        TextBox1.BorderStyle = BorderStyle.FixedSingle
        TextBox1.Font = New Font("宋体", 11F, FontStyle.Regular, GraphicsUnit.Point)
        TextBox1.Location = New Point(11, 25)
        TextBox1.Margin = New Padding(3, 4, 3, 4)
        TextBox1.Name = "TextBox1"
        TextBox1.Size = New Size(74, 28)
        TextBox1.TabIndex = 0
        ' 
        ' Button1
        ' 
        Button1.Location = New Point(91, 24)
        Button1.Margin = New Padding(3, 4, 3, 4)
        Button1.Name = "Button1"
        Button1.Size = New Size(88, 31)
        Button1.TabIndex = 1
        Button1.Text = "Swap"
        ' 
        ' TextBox2
        ' 
        TextBox2.BorderStyle = BorderStyle.FixedSingle
        TextBox2.Font = New Font("宋体", 11F, FontStyle.Regular, GraphicsUnit.Point)
        TextBox2.Location = New Point(185, 25)
        TextBox2.Margin = New Padding(3, 4, 3, 4)
        TextBox2.Name = "TextBox2"
        TextBox2.Size = New Size(72, 28)
        TextBox2.TabIndex = 0
        ' 
        ' GroupBox1
        ' 
        GroupBox1.Controls.Add(CheckBox1)
        GroupBox1.Controls.Add(TextBox1)
        GroupBox1.Controls.Add(Button1)
        GroupBox1.Controls.Add(TextBox2)
        GroupBox1.Location = New Point(3, 4)
        GroupBox1.Margin = New Padding(3, 4, 3, 4)
        GroupBox1.Name = "GroupBox1"
        GroupBox1.Padding = New Padding(3, 4, 3, 4)
        GroupBox1.Size = New Size(263, 102)
        GroupBox1.TabIndex = 8
        GroupBox1.TabStop = False
        GroupBox1.Text = "Manual adjustment"
        ' 
        ' CheckBox1
        ' 
        CheckBox1.AutoSize = True
        CheckBox1.Location = New Point(11, 63)
        CheckBox1.Margin = New Padding(3, 4, 3, 4)
        CheckBox1.Name = "CheckBox1"
        CheckBox1.Size = New Size(116, 27)
        CheckBox1.TabIndex = 2
        CheckBox1.Text = "re-arrange"
        CheckBox1.UseVisualStyleBackColor = True
        ' 
        ' SplitContainer1
        ' 
        SplitContainer1.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        SplitContainer1.Location = New Point(0, 30)
        SplitContainer1.Margin = New Padding(3, 4, 3, 4)
        SplitContainer1.Name = "SplitContainer1"
        SplitContainer1.Orientation = Orientation.Horizontal
        ' 
        ' SplitContainer1.Panel1
        ' 
        SplitContainer1.Panel1.Controls.Add(PictureBox1)
        ' 
        ' SplitContainer1.Panel2
        ' 
        SplitContainer1.Panel2.Controls.Add(GroupBox2)
        SplitContainer1.Panel2.Controls.Add(GroupBox1)
        SplitContainer1.Panel2.Controls.Add(RichTextBox1)
        SplitContainer1.Size = New Size(850, 613)
        SplitContainer1.SplitterDistance = 358
        SplitContainer1.SplitterWidth = 5
        SplitContainer1.TabIndex = 13
        ' 
        ' GroupBox2
        ' 
        GroupBox2.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left
        GroupBox2.Controls.Add(NumericUpDown3)
        GroupBox2.Controls.Add(Label3)
        GroupBox2.Controls.Add(Button2)
        GroupBox2.Controls.Add(NumericUpDown2)
        GroupBox2.Controls.Add(NumericUpDown1)
        GroupBox2.Controls.Add(Label2)
        GroupBox2.Controls.Add(Label1)
        GroupBox2.Location = New Point(3, 113)
        GroupBox2.Margin = New Padding(3, 4, 3, 4)
        GroupBox2.Name = "GroupBox2"
        GroupBox2.Padding = New Padding(3, 4, 3, 4)
        GroupBox2.Size = New Size(263, 133)
        GroupBox2.TabIndex = 10
        GroupBox2.TabStop = False
        GroupBox2.Text = "Paint"
        ' 
        ' NumericUpDown3
        ' 
        NumericUpDown3.Location = New Point(206, 96)
        NumericUpDown3.Margin = New Padding(3, 4, 3, 4)
        NumericUpDown3.Name = "NumericUpDown3"
        NumericUpDown3.Size = New Size(51, 29)
        NumericUpDown3.TabIndex = 10
        NumericUpDown3.Value = New Decimal(New Integer() {20, 0, 0, 0})
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Location = New Point(102, 98)
        Label3.Name = "Label3"
        Label3.Size = New Size(98, 23)
        Label3.TabIndex = 9
        Label3.Text = "Arm width:"
        ' 
        ' Button2
        ' 
        Button2.Location = New Point(6, 93)
        Button2.Margin = New Padding(3, 4, 3, 4)
        Button2.Name = "Button2"
        Button2.Size = New Size(85, 31)
        Button2.TabIndex = 8
        Button2.Text = "Refresh"
        Button2.UseVisualStyleBackColor = True
        ' 
        ' NumericUpDown2
        ' 
        NumericUpDown2.Location = New Point(206, 61)
        NumericUpDown2.Margin = New Padding(3, 4, 3, 4)
        NumericUpDown2.Name = "NumericUpDown2"
        NumericUpDown2.Size = New Size(51, 29)
        NumericUpDown2.TabIndex = 7
        NumericUpDown2.Value = New Decimal(New Integer() {30, 0, 0, 0})
        ' 
        ' NumericUpDown1
        ' 
        NumericUpDown1.Location = New Point(206, 25)
        NumericUpDown1.Margin = New Padding(3, 4, 3, 4)
        NumericUpDown1.Name = "NumericUpDown1"
        NumericUpDown1.Size = New Size(51, 29)
        NumericUpDown1.TabIndex = 6
        NumericUpDown1.Value = New Decimal(New Integer() {45, 0, 0, 0})
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Location = New Point(8, 63)
        Label2.Name = "Label2"
        Label2.Size = New Size(192, 23)
        Label2.TabIndex = 2
        Label2.Text = "Space between group:"
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(8, 25)
        Label1.Name = "Label1"
        Label1.Size = New Size(171, 23)
        Label1.TabIndex = 0
        Label1.Text = "Space within group:"
        ' 
        ' Form_analysis
        ' 
        AutoScaleDimensions = New SizeF(10F, 21F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(850, 642)
        Controls.Add(SplitContainer1)
        Controls.Add(MenuStrip1)
        Font = New Font("微软雅黑", 9.75F, FontStyle.Regular, GraphicsUnit.Point)
        Icon = CType(resources.GetObject("$this.Icon"), Icon)
        Margin = New Padding(3, 5, 3, 5)
        Name = "Form_analysis"
        Text = "Analysis"
        MenuStrip1.ResumeLayout(False)
        MenuStrip1.PerformLayout()
        CType(PictureBox1, ComponentModel.ISupportInitialize).EndInit()
        ContextMenu2.ResumeLayout(False)
        ContextMenu1.ResumeLayout(False)
        GroupBox1.ResumeLayout(False)
        GroupBox1.PerformLayout()
        SplitContainer1.Panel1.ResumeLayout(False)
        SplitContainer1.Panel2.ResumeLayout(False)
        CType(SplitContainer1, ComponentModel.ISupportInitialize).EndInit()
        SplitContainer1.ResumeLayout(False)
        GroupBox2.ResumeLayout(False)
        GroupBox2.PerformLayout()
        CType(NumericUpDown3, ComponentModel.ISupportInitialize).EndInit()
        CType(NumericUpDown2, ComponentModel.ISupportInitialize).EndInit()
        CType(NumericUpDown1, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()

    End Sub

    Friend WithEvents IdiogramToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SaveResultToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SaveGraphicToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SaveToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents FileToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents ViewToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents PictureBox1 As PictureBox
    ' TODO MenuItem is no longer supported. Use ToolStripMenuItem instead. For more details see https://docs.microsoft.com/en-us/dotnet/core/compatibility/winforms#removed-controls
    Friend WithEvents MenuItem3 As ToolStripMenuItem
    ' TODO MenuItem is no longer supported. Use ToolStripMenuItem instead. For more details see https://docs.microsoft.com/en-us/dotnet/core/compatibility/winforms#removed-controls
    Friend WithEvents MenuItem4 As ToolStripMenuItem
    ' TODO MenuItem is no longer supported. Use ToolStripMenuItem instead. For more details see https://docs.microsoft.com/en-us/dotnet/core/compatibility/winforms#removed-controls
    Friend WithEvents MenuItem2 As ToolStripMenuItem
    ' TODO ContextMenu is no longer supported. Use ContextMenuStrip instead. For more details see https://docs.microsoft.com/en-us/dotnet/core/compatibility/winforms#removed-controls
    Friend WithEvents ContextMenu2 As ContextMenuStrip
    ' TODO MenuItem is no longer supported. Use ToolStripMenuItem instead. For more details see https://docs.microsoft.com/en-us/dotnet/core/compatibility/winforms#removed-controls
    Friend WithEvents MenuItem1 As ToolStripMenuItem
    ' TODO ContextMenu is no longer supported. Use ContextMenuStrip instead. For more details see https://docs.microsoft.com/en-us/dotnet/core/compatibility/winforms#removed-controls
    Friend WithEvents ContextMenu1 As ContextMenuStrip
    Friend WithEvents RichTextBox1 As RichTextBox
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents Button1 As Button
    Friend WithEvents TextBox2 As TextBox
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents SplitContainer1 As SplitContainer
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents NumericUpDown2 As NumericUpDown
    Friend WithEvents NumericUpDown1 As NumericUpDown
    Friend WithEvents Button2 As Button
    Friend WithEvents NumericUpDown3 As NumericUpDown
    Friend WithEvents Label3 As Label
    Friend WithEvents CheckBox1 As CheckBox
End Class
