<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim DataGridViewCellStyle9 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle10 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.TableLayoutPanel3 = New System.Windows.Forms.TableLayoutPanel()
        Me.TableLayoutPanel4 = New System.Windows.Forms.TableLayoutPanel()
        Me.GBMachine1 = New System.Windows.Forms.GroupBox()
        Me.BtnMachine1 = New System.Windows.Forms.Button()
        Me.GBMachine2 = New System.Windows.Forms.GroupBox()
        Me.BtnMachine2 = New System.Windows.Forms.Button()
        Me.GBMachine3 = New System.Windows.Forms.GroupBox()
        Me.BtnMachine3 = New System.Windows.Forms.Button()
        Me.GBMachine4 = New System.Windows.Forms.GroupBox()
        Me.BtnMachine4 = New System.Windows.Forms.Button()
        Me.GBMachine5 = New System.Windows.Forms.GroupBox()
        Me.BtnMachine5 = New System.Windows.Forms.Button()
        Me.DGVInformation = New System.Windows.Forms.DataGridView()
        Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel()
        Me.LblDateTime = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.lbl_title = New System.Windows.Forms.Label()
        Me.TimerMonitoring = New System.Windows.Forms.Timer(Me.components)
        Me.ImageList2 = New System.Windows.Forms.ImageList(Me.components)
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.MonitorToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MesinStatusToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OPCStatusToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Timerinitiation = New System.Windows.Forms.Timer(Me.components)
        Me.NotifyIcon1 = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.TableLayoutPanel1.SuspendLayout()
        Me.TableLayoutPanel3.SuspendLayout()
        Me.TableLayoutPanel4.SuspendLayout()
        Me.GBMachine1.SuspendLayout()
        Me.GBMachine2.SuspendLayout()
        Me.GBMachine3.SuspendLayout()
        Me.GBMachine4.SuspendLayout()
        Me.GBMachine5.SuspendLayout()
        CType(Me.DGVInformation, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TableLayoutPanel2.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.[Single]
        Me.TableLayoutPanel1.ColumnCount = 1
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.TableLayoutPanel3, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.TableLayoutPanel2, 0, 0)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 23)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 2
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(1067, 568)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'TableLayoutPanel3
        '
        Me.TableLayoutPanel3.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel3.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Inset
        Me.TableLayoutPanel3.ColumnCount = 2
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 300.0!))
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel3.Controls.Add(Me.TableLayoutPanel4, 0, 0)
        Me.TableLayoutPanel3.Controls.Add(Me.DGVInformation, 1, 0)
        Me.TableLayoutPanel3.Location = New System.Drawing.Point(4, 65)
        Me.TableLayoutPanel3.Name = "TableLayoutPanel3"
        Me.TableLayoutPanel3.RowCount = 1
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel3.Size = New System.Drawing.Size(1059, 499)
        Me.TableLayoutPanel3.TabIndex = 1
        '
        'TableLayoutPanel4
        '
        Me.TableLayoutPanel4.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel4.ColumnCount = 1
        Me.TableLayoutPanel4.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel4.Controls.Add(Me.GBMachine1, 0, 0)
        Me.TableLayoutPanel4.Controls.Add(Me.GBMachine2, 0, 1)
        Me.TableLayoutPanel4.Controls.Add(Me.GBMachine3, 0, 2)
        Me.TableLayoutPanel4.Controls.Add(Me.GBMachine4, 0, 3)
        Me.TableLayoutPanel4.Controls.Add(Me.GBMachine5, 0, 4)
        Me.TableLayoutPanel4.Location = New System.Drawing.Point(5, 5)
        Me.TableLayoutPanel4.Name = "TableLayoutPanel4"
        Me.TableLayoutPanel4.RowCount = 5
        Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
        Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
        Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
        Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
        Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
        Me.TableLayoutPanel4.Size = New System.Drawing.Size(294, 489)
        Me.TableLayoutPanel4.TabIndex = 0
        '
        'GBMachine1
        '
        Me.GBMachine1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GBMachine1.Controls.Add(Me.BtnMachine1)
        Me.GBMachine1.Location = New System.Drawing.Point(3, 3)
        Me.GBMachine1.Name = "GBMachine1"
        Me.GBMachine1.Size = New System.Drawing.Size(288, 91)
        Me.GBMachine1.TabIndex = 0
        Me.GBMachine1.TabStop = False
        '
        'BtnMachine1
        '
        Me.BtnMachine1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BtnMachine1.BackColor = System.Drawing.Color.Gainsboro
        Me.BtnMachine1.Font = New System.Drawing.Font("Microsoft Sans Serif", 24.25!)
        Me.BtnMachine1.Location = New System.Drawing.Point(1, 6)
        Me.BtnMachine1.Name = "BtnMachine1"
        Me.BtnMachine1.Size = New System.Drawing.Size(287, 85)
        Me.BtnMachine1.TabIndex = 1
        Me.BtnMachine1.Text = "Button1"
        Me.BtnMachine1.UseVisualStyleBackColor = False
        '
        'GBMachine2
        '
        Me.GBMachine2.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GBMachine2.Controls.Add(Me.BtnMachine2)
        Me.GBMachine2.Location = New System.Drawing.Point(3, 100)
        Me.GBMachine2.Name = "GBMachine2"
        Me.GBMachine2.Size = New System.Drawing.Size(288, 91)
        Me.GBMachine2.TabIndex = 0
        Me.GBMachine2.TabStop = False
        '
        'BtnMachine2
        '
        Me.BtnMachine2.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BtnMachine2.BackColor = System.Drawing.Color.Gainsboro
        Me.BtnMachine2.Font = New System.Drawing.Font("Microsoft Sans Serif", 24.25!)
        Me.BtnMachine2.Location = New System.Drawing.Point(1, 7)
        Me.BtnMachine2.Name = "BtnMachine2"
        Me.BtnMachine2.Size = New System.Drawing.Size(287, 84)
        Me.BtnMachine2.TabIndex = 1
        Me.BtnMachine2.Text = "Button1"
        Me.BtnMachine2.UseVisualStyleBackColor = False
        '
        'GBMachine3
        '
        Me.GBMachine3.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GBMachine3.Controls.Add(Me.BtnMachine3)
        Me.GBMachine3.Location = New System.Drawing.Point(3, 197)
        Me.GBMachine3.Name = "GBMachine3"
        Me.GBMachine3.Size = New System.Drawing.Size(288, 91)
        Me.GBMachine3.TabIndex = 0
        Me.GBMachine3.TabStop = False
        '
        'BtnMachine3
        '
        Me.BtnMachine3.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BtnMachine3.BackColor = System.Drawing.Color.Gainsboro
        Me.BtnMachine3.Font = New System.Drawing.Font("Microsoft Sans Serif", 24.25!)
        Me.BtnMachine3.Location = New System.Drawing.Point(0, 6)
        Me.BtnMachine3.Name = "BtnMachine3"
        Me.BtnMachine3.Size = New System.Drawing.Size(287, 91)
        Me.BtnMachine3.TabIndex = 1
        Me.BtnMachine3.Text = "Button1"
        Me.BtnMachine3.UseVisualStyleBackColor = False
        '
        'GBMachine4
        '
        Me.GBMachine4.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GBMachine4.Controls.Add(Me.BtnMachine4)
        Me.GBMachine4.Location = New System.Drawing.Point(3, 294)
        Me.GBMachine4.Name = "GBMachine4"
        Me.GBMachine4.Size = New System.Drawing.Size(288, 91)
        Me.GBMachine4.TabIndex = 0
        Me.GBMachine4.TabStop = False
        '
        'BtnMachine4
        '
        Me.BtnMachine4.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BtnMachine4.BackColor = System.Drawing.Color.Gainsboro
        Me.BtnMachine4.Font = New System.Drawing.Font("Microsoft Sans Serif", 24.25!)
        Me.BtnMachine4.Location = New System.Drawing.Point(0, 6)
        Me.BtnMachine4.Name = "BtnMachine4"
        Me.BtnMachine4.Size = New System.Drawing.Size(287, 85)
        Me.BtnMachine4.TabIndex = 1
        Me.BtnMachine4.Text = "Button1"
        Me.BtnMachine4.UseVisualStyleBackColor = False
        '
        'GBMachine5
        '
        Me.GBMachine5.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GBMachine5.Controls.Add(Me.BtnMachine5)
        Me.GBMachine5.Location = New System.Drawing.Point(3, 391)
        Me.GBMachine5.Name = "GBMachine5"
        Me.GBMachine5.Size = New System.Drawing.Size(288, 95)
        Me.GBMachine5.TabIndex = 0
        Me.GBMachine5.TabStop = False
        '
        'BtnMachine5
        '
        Me.BtnMachine5.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BtnMachine5.BackColor = System.Drawing.Color.Gainsboro
        Me.BtnMachine5.Font = New System.Drawing.Font("Microsoft Sans Serif", 24.25!)
        Me.BtnMachine5.Location = New System.Drawing.Point(1, 0)
        Me.BtnMachine5.Name = "BtnMachine5"
        Me.BtnMachine5.Size = New System.Drawing.Size(287, 88)
        Me.BtnMachine5.TabIndex = 1
        Me.BtnMachine5.Text = "Button1"
        Me.BtnMachine5.UseVisualStyleBackColor = False
        '
        'DGVInformation
        '
        Me.DGVInformation.AllowUserToAddRows = False
        Me.DGVInformation.AllowUserToDeleteRows = False
        Me.DGVInformation.AllowUserToResizeRows = False
        Me.DGVInformation.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DGVInformation.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.DGVInformation.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells
        Me.DGVInformation.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight
        DataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.ActiveCaption
        DataGridViewCellStyle9.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DGVInformation.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle9
        Me.DGVInformation.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Menu
        DataGridViewCellStyle10.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.DGVInformation.DefaultCellStyle = DataGridViewCellStyle10
        Me.DGVInformation.Location = New System.Drawing.Point(307, 5)
        Me.DGVInformation.Name = "DGVInformation"
        Me.DGVInformation.Size = New System.Drawing.Size(747, 489)
        Me.DGVInformation.TabIndex = 1
        '
        'TableLayoutPanel2
        '
        Me.TableLayoutPanel2.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel2.ColumnCount = 2
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70.0!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30.0!))
        Me.TableLayoutPanel2.Controls.Add(Me.LblDateTime, 1, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.GroupBox1, 0, 0)
        Me.TableLayoutPanel2.Location = New System.Drawing.Point(4, 4)
        Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
        Me.TableLayoutPanel2.RowCount = 1
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(1059, 54)
        Me.TableLayoutPanel2.TabIndex = 0
        '
        'LblDateTime
        '
        Me.LblDateTime.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.LblDateTime.AutoSize = True
        Me.LblDateTime.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.0!)
        Me.LblDateTime.Location = New System.Drawing.Point(829, 11)
        Me.LblDateTime.Name = "LblDateTime"
        Me.LblDateTime.Size = New System.Drawing.Size(227, 31)
        Me.LblDateTime.TabIndex = 0
        Me.LblDateTime.Text = "28 February 2012"
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.AutoSize = True
        Me.GroupBox1.Controls.Add(Me.lbl_title)
        Me.GroupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.GroupBox1.Location = New System.Drawing.Point(3, 3)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(735, 48)
        Me.GroupBox1.TabIndex = 1
        Me.GroupBox1.TabStop = False
        '
        'lbl_title
        '
        Me.lbl_title.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lbl_title.AutoSize = True
        Me.lbl_title.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.0!)
        Me.lbl_title.Location = New System.Drawing.Point(6, 14)
        Me.lbl_title.Name = "lbl_title"
        Me.lbl_title.Size = New System.Drawing.Size(560, 31)
        Me.lbl_title.TabIndex = 1
        Me.lbl_title.Text = "Production Data Acquisition (Machine Status)"
        '
        'TimerMonitoring
        '
        Me.TimerMonitoring.Interval = 500
        '
        'ImageList2
        '
        Me.ImageList2.ImageStream = CType(resources.GetObject("ImageList2.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList2.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList2.Images.SetKeyName(0, "Aries_Small.png")
        Me.ImageList2.Images.SetKeyName(1, "Taurus_Small.png")
        Me.ImageList2.Images.SetKeyName(2, "Gemini_Small.png")
        Me.ImageList2.Images.SetKeyName(3, "Cancer_Small.png")
        Me.ImageList2.Images.SetKeyName(4, "Leo_Small.png")
        Me.ImageList2.Images.SetKeyName(5, "Virgo_Small.png")
        Me.ImageList2.Images.SetKeyName(6, "Libra_Small.png")
        Me.ImageList2.Images.SetKeyName(7, "Scorpio_Small.png")
        Me.ImageList2.Images.SetKeyName(8, "Sagittarius_Small.png")
        Me.ImageList2.Images.SetKeyName(9, "Capricorn_Small.png")
        Me.ImageList2.Images.SetKeyName(10, "Aquarius_Small.png")
        Me.ImageList2.Images.SetKeyName(11, "Pisces_Small.png")
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MonitorToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(1067, 24)
        Me.MenuStrip1.TabIndex = 1
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'MonitorToolStripMenuItem
        '
        Me.MonitorToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MesinStatusToolStripMenuItem, Me.OPCStatusToolStripMenuItem})
        Me.MonitorToolStripMenuItem.Name = "MonitorToolStripMenuItem"
        Me.MonitorToolStripMenuItem.Size = New System.Drawing.Size(62, 20)
        Me.MonitorToolStripMenuItem.Text = "Monitor"
        '
        'MesinStatusToolStripMenuItem
        '
        Me.MesinStatusToolStripMenuItem.Name = "MesinStatusToolStripMenuItem"
        Me.MesinStatusToolStripMenuItem.Size = New System.Drawing.Size(141, 22)
        Me.MesinStatusToolStripMenuItem.Text = "Mesin Status"
        '
        'OPCStatusToolStripMenuItem
        '
        Me.OPCStatusToolStripMenuItem.Name = "OPCStatusToolStripMenuItem"
        Me.OPCStatusToolStripMenuItem.Size = New System.Drawing.Size(141, 22)
        Me.OPCStatusToolStripMenuItem.Text = "OPC Status"
        '
        'Timerinitiation
        '
        '
        'NotifyIcon1
        '
        Me.NotifyIcon1.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info
        Me.NotifyIcon1.Icon = CType(resources.GetObject("NotifyIcon1.Icon"), System.Drawing.Icon)
        Me.NotifyIcon1.Text = "oee data acquisition"
        Me.NotifyIcon1.Visible = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1067, 594)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "Form1"
        Me.Text = "Philips OEE Data Acquisition"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel3.ResumeLayout(False)
        Me.TableLayoutPanel4.ResumeLayout(False)
        Me.GBMachine1.ResumeLayout(False)
        Me.GBMachine2.ResumeLayout(False)
        Me.GBMachine3.ResumeLayout(False)
        Me.GBMachine4.ResumeLayout(False)
        Me.GBMachine5.ResumeLayout(False)
        CType(Me.DGVInformation, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TableLayoutPanel2.ResumeLayout(False)
        Me.TableLayoutPanel2.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents TableLayoutPanel2 As TableLayoutPanel
    Friend WithEvents LblDateTime As Label
    Friend WithEvents TimerMonitoring As Timer
    Friend WithEvents TableLayoutPanel3 As TableLayoutPanel
    Friend WithEvents TableLayoutPanel4 As TableLayoutPanel
    Friend WithEvents GBMachine1 As GroupBox
    Friend WithEvents GBMachine2 As GroupBox
    Friend WithEvents GBMachine3 As GroupBox
    Friend WithEvents GBMachine4 As GroupBox
    Friend WithEvents GBMachine5 As GroupBox
    Friend WithEvents ImageList2 As ImageList
    Friend WithEvents BtnMachine2 As Button
    Friend WithEvents BtnMachine3 As Button
    Friend WithEvents BtnMachine4 As Button
    Friend WithEvents DGVInformation As DataGridView
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents lbl_title As Label
    Friend WithEvents BtnMachine5 As Button
    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents MonitorToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents MesinStatusToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents OPCStatusToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents BtnMachine1 As Button
    Friend WithEvents Timerinitiation As Timer
    Friend WithEvents NotifyIcon1 As NotifyIcon
End Class
