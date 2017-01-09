'==============================================================================

' File:                         ULFI01.VB

' Library Call Demonstrated:    Mccdaq.MccBoard.FileAInScan()

' Purpose:                      Scan a range of A/D channels and 
'                               store the data in a disk file.

' Demonstration:                Collects data points from analog input
'                               channels and stores them in a file.

' Other Library Calls:          MccDaq.MccService.ErrHandling()

' Special Requirements:         Board 0 must have an A/D converter.
'                               Analog signal on an input channel.

'==============================================================================
Option Strict Off
Option Explicit On

Public Class frmDataDisplay

    Inherits System.Windows.Forms.Form

    'Create a new MccBoard object for DaqBoard 
    Private DaqBoard As MccDaq.MccBoard = New MccDaq.MccBoard(0)

    Private Range As MccDaq.Range
    Private ADResolution, NumAIChans As Integer
    Private HighChan, LowChan As Integer

    Const NumPoints As Integer = 2000   ' Number of data points to collect
    Dim Rate As Integer = 1000          ' per channel sampling rate

    Private Sub frmDataDisplay_Load(ByVal sender As System.Object, _
    ByVal e As System.EventArgs) Handles MyBase.Load

        Dim DefaultTrig As MccDaq.TriggerType

        InitUL()

        ' determine the number of analog channels and their capabilities
        Dim ChannelType As Integer = ANALOGINPUT
        NumAIChans = FindAnalogChansOfType(DaqBoard, ChannelType, _
            ADResolution, Range, LowChan, DefaultTrig)

        If (NumAIChans = 0) Then
            lblAcqStat.Text = "Board " & DaqBoard.BoardNum.ToString() & _
            " does not have analog input channels."
            lblAcqStat.ForeColor = Color.Red
            cmdStartAcq.Enabled = False
            cmdStopConvert.Enabled = True
        Else
            lblAcqStat.Text = "Reading analog inputs on board " & _
            DaqBoard.BoardNum.ToString() & " and writing data " & _
            "to file."
        End If

    End Sub

    Private Sub cmdStartAcq_Click(ByVal eventSender As System.Object, _
    ByVal eventArgs As System.EventArgs) Handles cmdStartAcq.Click

        Dim TotalCount As Integer
        Dim PretrigCount As Integer
        Dim ULStat As MccDaq.ErrorInfo
        Dim DataCount As String
        Dim Options As MccDaq.ScanOptions
        Dim HighChan As Integer
        Dim FileName As String
        Dim Count As Integer
        Dim FileLowChan As Short
        Dim FileHighChan As Short

        cmdStartAcq.Enabled = False
        cmdStopConvert.Enabled = False
        lblAcqStat.ForeColor = Color.Blue

        ' Parameters:
        '   LowChan    :first A/D channel of the scan
        '   HighChan   :last A/D channel of the scan
        '   Count      :the total number of A/D samples to collect
        '   Rate       :Sample rate in samples per second
        '   Range      :the gain for the board
        '   FileName   :the filename for the collected data values
        '   Options    :data collection options

        Count = NumPoints
        FileName = txtFileName.Text ' a full path may be required here
        LowChan = 0
        HighChan = 1
        Options = MccDaq.ScanOptions.Default
        Range = MccDaq.Range.Bip5Volts 'set the range

        DataCount = NumPoints.ToString("0")
        lblAcqStat.Text = "Collecting " & DataCount & " data points"
        lblShowRate.Text = Rate.ToString("0")
        lblShowLoChan.Text = LowChan.ToString("0")
        lblShowHiChan.Text = HighChan.ToString("0")
        lblShowOptions.Text = [Enum].Format(GetType(MccDaq.ScanOptions), Options, "d")
        lblShowGain.Text = Range.ToString()
        lblShowFile.Text = FileName
        lblShowCount.Text = Count.ToString("0")
        lblShowPreTrig.Text = "Not Applicable"
        System.Windows.Forms.Application.DoEvents()

        ' Collect the values with Collect the values by calling MccDaq.MccBoard.FileAInScan()

        ULStat = DaqBoard.FileAInScan(LowChan, HighChan, Count, Rate, Range, FileName, Options)
        If ULStat.Value = MccDaq.ErrorInfo.ErrorCode.BadFileName Then
            MsgBox("Enter the name of the file in which to store the data in text box.", _
            0, "Bad File Name")
            cmdStopConvert.Enabled = False
            cmdStartAcq.Enabled = True
            txtFileName.Focus()
            Exit Sub
        ElseIf ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then
            lblAcqStat.Text = ULStat.Message & "."
            lblAcqStat.ForeColor = Color.Red
            cmdStopConvert.Enabled = True
            Exit Sub
        End If
        lblAcqStat.Text = ""

        ' show how many data points were collected

        ULStat = MccDaq.MccService.FileGetInfo(FileName, FileLowChan, _
            FileHighChan, PretrigCount, TotalCount, Rate, Range)
        If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then
            lblAcqStat.Text = ULStat.Message & "."
            lblAcqStat.ForeColor = Color.Red
            cmdStopConvert.Enabled = True
        End If

        lblReadRate.Text = Rate.ToString("0")
        lblReadLoChan.Text = FileLowChan.ToString("0")
        lblReadHiChan.Text = FileHighChan.ToString("0")
        lblReadOptions.Text = [Enum].Format(GetType(MccDaq.ScanOptions), Options, "d")
        lblReadGain.Text = Range.ToString()
        lblReadFile.Text = FileName

        lblReadTotal.Text = TotalCount.ToString("0")
        lblReadPreTrig.Text = PretrigCount.ToString("0")
        cmdStopConvert.Enabled = True
        cmdStartAcq.Enabled = True

    End Sub

    Private Sub cmdStopConvert_Click(ByVal eventSender As System.Object, _
    ByVal eventArgs As System.EventArgs) Handles cmdStopConvert.Click

        End

    End Sub

#Region "Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal Disposing As Boolean)

        If Disposing Then
            If Not components Is Nothing Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(Disposing)

    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer
    Public ToolTip1 As System.Windows.Forms.ToolTip
    Public WithEvents cmdStopConvert As System.Windows.Forms.Button
    Public WithEvents cmdStartAcq As System.Windows.Forms.Button
    Public WithEvents txtFileName As System.Windows.Forms.TextBox
    Public WithEvents lblFileInstruct As System.Windows.Forms.Label
    Public WithEvents lblReadFile As System.Windows.Forms.Label
    Public WithEvents lblShowFile As System.Windows.Forms.Label
    Public WithEvents lblFileName As System.Windows.Forms.Label
    Public WithEvents lblReadPreTrig As System.Windows.Forms.Label
    Public WithEvents lblShowPreTrig As System.Windows.Forms.Label
    Public WithEvents lblPreTrig As System.Windows.Forms.Label
    Public WithEvents lblReadTotal As System.Windows.Forms.Label
    Public WithEvents lblShowCount As System.Windows.Forms.Label
    Public WithEvents lblCount As System.Windows.Forms.Label
    Public WithEvents lblReadGain As System.Windows.Forms.Label
    Public WithEvents lblShowGain As System.Windows.Forms.Label
    Public WithEvents lblGain As System.Windows.Forms.Label
    Public WithEvents lblReadOptions As System.Windows.Forms.Label
    Public WithEvents lblShowOptions As System.Windows.Forms.Label
    Public WithEvents lblOptions As System.Windows.Forms.Label
    Public WithEvents lblReadHiChan As System.Windows.Forms.Label
    Public WithEvents lblShowHiChan As System.Windows.Forms.Label
    Public WithEvents lblHiChan As System.Windows.Forms.Label
    Public WithEvents lblReadLoChan As System.Windows.Forms.Label
    Public WithEvents lblShowLoChan As System.Windows.Forms.Label
    Public WithEvents lblLoChan As System.Windows.Forms.Label
    Public WithEvents lblReadRate As System.Windows.Forms.Label
    Public WithEvents lblShowRate As System.Windows.Forms.Label
    Public WithEvents lblRate As System.Windows.Forms.Label
    Public WithEvents lblInCol As System.Windows.Forms.Label
    Public WithEvents lblOutCol As System.Windows.Forms.Label
    Public WithEvents lblAcqStat As System.Windows.Forms.Label
    Public WithEvents lblDemoFunction As System.Windows.Forms.Label
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdStopConvert = New System.Windows.Forms.Button
        Me.cmdStartAcq = New System.Windows.Forms.Button
        Me.txtFileName = New System.Windows.Forms.TextBox
        Me.lblFileInstruct = New System.Windows.Forms.Label
        Me.lblReadFile = New System.Windows.Forms.Label
        Me.lblShowFile = New System.Windows.Forms.Label
        Me.lblFileName = New System.Windows.Forms.Label
        Me.lblReadPreTrig = New System.Windows.Forms.Label
        Me.lblShowPreTrig = New System.Windows.Forms.Label
        Me.lblPreTrig = New System.Windows.Forms.Label
        Me.lblReadTotal = New System.Windows.Forms.Label
        Me.lblShowCount = New System.Windows.Forms.Label
        Me.lblCount = New System.Windows.Forms.Label
        Me.lblReadGain = New System.Windows.Forms.Label
        Me.lblShowGain = New System.Windows.Forms.Label
        Me.lblGain = New System.Windows.Forms.Label
        Me.lblReadOptions = New System.Windows.Forms.Label
        Me.lblShowOptions = New System.Windows.Forms.Label
        Me.lblOptions = New System.Windows.Forms.Label
        Me.lblReadHiChan = New System.Windows.Forms.Label
        Me.lblShowHiChan = New System.Windows.Forms.Label
        Me.lblHiChan = New System.Windows.Forms.Label
        Me.lblReadLoChan = New System.Windows.Forms.Label
        Me.lblShowLoChan = New System.Windows.Forms.Label
        Me.lblLoChan = New System.Windows.Forms.Label
        Me.lblReadRate = New System.Windows.Forms.Label
        Me.lblShowRate = New System.Windows.Forms.Label
        Me.lblRate = New System.Windows.Forms.Label
        Me.lblInCol = New System.Windows.Forms.Label
        Me.lblOutCol = New System.Windows.Forms.Label
        Me.lblAcqStat = New System.Windows.Forms.Label
        Me.lblDemoFunction = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'cmdStopConvert
        '
        Me.cmdStopConvert.BackColor = System.Drawing.SystemColors.Control
        Me.cmdStopConvert.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdStopConvert.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdStopConvert.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdStopConvert.Location = New System.Drawing.Point(324, 310)
        Me.cmdStopConvert.Name = "cmdStopConvert"
        Me.cmdStopConvert.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdStopConvert.Size = New System.Drawing.Size(75, 26)
        Me.cmdStopConvert.TabIndex = 1
        Me.cmdStopConvert.Text = "Quit"
        Me.cmdStopConvert.UseVisualStyleBackColor = False
        '
        'cmdStartAcq
        '
        Me.cmdStartAcq.BackColor = System.Drawing.SystemColors.Control
        Me.cmdStartAcq.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdStartAcq.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdStartAcq.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdStartAcq.Location = New System.Drawing.Point(229, 310)
        Me.cmdStartAcq.Name = "cmdStartAcq"
        Me.cmdStartAcq.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdStartAcq.Size = New System.Drawing.Size(84, 26)
        Me.cmdStartAcq.TabIndex = 2
        Me.cmdStartAcq.Text = "Start"
        Me.cmdStartAcq.UseVisualStyleBackColor = False
        '
        'txtFileName
        '
        Me.txtFileName.AcceptsReturn = True
        Me.txtFileName.BackColor = System.Drawing.SystemColors.Window
        Me.txtFileName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtFileName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtFileName.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFileName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtFileName.Location = New System.Drawing.Point(200, 279)
        Me.txtFileName.MaxLength = 0
        Me.txtFileName.Name = "txtFileName"
        Me.txtFileName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtFileName.Size = New System.Drawing.Size(161, 20)
        Me.txtFileName.TabIndex = 30
        Me.txtFileName.Text = "DEMO.DAT"
        '
        'lblFileInstruct
        '
        Me.lblFileInstruct.BackColor = System.Drawing.SystemColors.Window
        Me.lblFileInstruct.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblFileInstruct.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFileInstruct.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblFileInstruct.Location = New System.Drawing.Point(10, 270)
        Me.lblFileInstruct.Name = "lblFileInstruct"
        Me.lblFileInstruct.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblFileInstruct.Size = New System.Drawing.Size(169, 41)
        Me.lblFileInstruct.TabIndex = 31
        Me.lblFileInstruct.Text = "Enter the name of the file that you have created using MAKESTRM.EXE"
        Me.lblFileInstruct.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblReadFile
        '
        Me.lblReadFile.BackColor = System.Drawing.SystemColors.Window
        Me.lblReadFile.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblReadFile.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReadFile.ForeColor = System.Drawing.Color.Blue
        Me.lblReadFile.Location = New System.Drawing.Point(258, 238)
        Me.lblReadFile.Name = "lblReadFile"
        Me.lblReadFile.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblReadFile.Size = New System.Drawing.Size(119, 18)
        Me.lblReadFile.TabIndex = 18
        '
        'lblShowFile
        '
        Me.lblShowFile.BackColor = System.Drawing.SystemColors.Window
        Me.lblShowFile.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblShowFile.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShowFile.ForeColor = System.Drawing.Color.Blue
        Me.lblShowFile.Location = New System.Drawing.Point(114, 238)
        Me.lblShowFile.Name = "lblShowFile"
        Me.lblShowFile.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblShowFile.Size = New System.Drawing.Size(111, 18)
        Me.lblShowFile.TabIndex = 9
        '
        'lblFileName
        '
        Me.lblFileName.BackColor = System.Drawing.SystemColors.Window
        Me.lblFileName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblFileName.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFileName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblFileName.Location = New System.Drawing.Point(34, 238)
        Me.lblFileName.Name = "lblFileName"
        Me.lblFileName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblFileName.Size = New System.Drawing.Size(65, 17)
        Me.lblFileName.TabIndex = 25
        Me.lblFileName.Text = "File Name:"
        Me.lblFileName.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblReadPreTrig
        '
        Me.lblReadPreTrig.BackColor = System.Drawing.SystemColors.Window
        Me.lblReadPreTrig.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblReadPreTrig.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReadPreTrig.ForeColor = System.Drawing.Color.Blue
        Me.lblReadPreTrig.Location = New System.Drawing.Point(258, 206)
        Me.lblReadPreTrig.Name = "lblReadPreTrig"
        Me.lblReadPreTrig.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblReadPreTrig.Size = New System.Drawing.Size(63, 18)
        Me.lblReadPreTrig.TabIndex = 12
        '
        'lblShowPreTrig
        '
        Me.lblShowPreTrig.BackColor = System.Drawing.SystemColors.Window
        Me.lblShowPreTrig.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblShowPreTrig.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShowPreTrig.ForeColor = System.Drawing.Color.Blue
        Me.lblShowPreTrig.Location = New System.Drawing.Point(114, 206)
        Me.lblShowPreTrig.Name = "lblShowPreTrig"
        Me.lblShowPreTrig.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblShowPreTrig.Size = New System.Drawing.Size(121, 17)
        Me.lblShowPreTrig.TabIndex = 29
        '
        'lblPreTrig
        '
        Me.lblPreTrig.BackColor = System.Drawing.SystemColors.Window
        Me.lblPreTrig.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPreTrig.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPreTrig.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblPreTrig.Location = New System.Drawing.Point(2, 206)
        Me.lblPreTrig.Name = "lblPreTrig"
        Me.lblPreTrig.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPreTrig.Size = New System.Drawing.Size(97, 17)
        Me.lblPreTrig.TabIndex = 28
        Me.lblPreTrig.Text = "Pre-Trig Count:"
        Me.lblPreTrig.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblReadTotal
        '
        Me.lblReadTotal.BackColor = System.Drawing.SystemColors.Window
        Me.lblReadTotal.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblReadTotal.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReadTotal.ForeColor = System.Drawing.Color.Blue
        Me.lblReadTotal.Location = New System.Drawing.Point(258, 182)
        Me.lblReadTotal.Name = "lblReadTotal"
        Me.lblReadTotal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblReadTotal.Size = New System.Drawing.Size(63, 18)
        Me.lblReadTotal.TabIndex = 11
        '
        'lblShowCount
        '
        Me.lblShowCount.BackColor = System.Drawing.SystemColors.Window
        Me.lblShowCount.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblShowCount.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShowCount.ForeColor = System.Drawing.Color.Blue
        Me.lblShowCount.Location = New System.Drawing.Point(114, 182)
        Me.lblShowCount.Name = "lblShowCount"
        Me.lblShowCount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblShowCount.Size = New System.Drawing.Size(63, 18)
        Me.lblShowCount.TabIndex = 10
        '
        'lblCount
        '
        Me.lblCount.BackColor = System.Drawing.SystemColors.Window
        Me.lblCount.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCount.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCount.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblCount.Location = New System.Drawing.Point(34, 182)
        Me.lblCount.Name = "lblCount"
        Me.lblCount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCount.Size = New System.Drawing.Size(65, 17)
        Me.lblCount.TabIndex = 24
        Me.lblCount.Text = "Count:"
        Me.lblCount.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblReadGain
        '
        Me.lblReadGain.BackColor = System.Drawing.SystemColors.Window
        Me.lblReadGain.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblReadGain.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReadGain.ForeColor = System.Drawing.Color.Blue
        Me.lblReadGain.Location = New System.Drawing.Point(258, 166)
        Me.lblReadGain.Name = "lblReadGain"
        Me.lblReadGain.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblReadGain.Size = New System.Drawing.Size(55, 18)
        Me.lblReadGain.TabIndex = 17
        '
        'lblShowGain
        '
        Me.lblShowGain.BackColor = System.Drawing.SystemColors.Window
        Me.lblShowGain.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblShowGain.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShowGain.ForeColor = System.Drawing.Color.Blue
        Me.lblShowGain.Location = New System.Drawing.Point(114, 166)
        Me.lblShowGain.Name = "lblShowGain"
        Me.lblShowGain.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblShowGain.Size = New System.Drawing.Size(63, 18)
        Me.lblShowGain.TabIndex = 8
        '
        'lblGain
        '
        Me.lblGain.BackColor = System.Drawing.SystemColors.Window
        Me.lblGain.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblGain.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblGain.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblGain.Location = New System.Drawing.Point(34, 166)
        Me.lblGain.Name = "lblGain"
        Me.lblGain.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblGain.Size = New System.Drawing.Size(65, 17)
        Me.lblGain.TabIndex = 23
        Me.lblGain.Text = "Gain:"
        Me.lblGain.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblReadOptions
        '
        Me.lblReadOptions.BackColor = System.Drawing.SystemColors.Window
        Me.lblReadOptions.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblReadOptions.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReadOptions.ForeColor = System.Drawing.Color.Blue
        Me.lblReadOptions.Location = New System.Drawing.Point(258, 150)
        Me.lblReadOptions.Name = "lblReadOptions"
        Me.lblReadOptions.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblReadOptions.Size = New System.Drawing.Size(63, 18)
        Me.lblReadOptions.TabIndex = 16
        '
        'lblShowOptions
        '
        Me.lblShowOptions.BackColor = System.Drawing.SystemColors.Window
        Me.lblShowOptions.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblShowOptions.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShowOptions.ForeColor = System.Drawing.Color.Blue
        Me.lblShowOptions.Location = New System.Drawing.Point(114, 150)
        Me.lblShowOptions.Name = "lblShowOptions"
        Me.lblShowOptions.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblShowOptions.Size = New System.Drawing.Size(63, 18)
        Me.lblShowOptions.TabIndex = 7
        '
        'lblOptions
        '
        Me.lblOptions.BackColor = System.Drawing.SystemColors.Window
        Me.lblOptions.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblOptions.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOptions.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblOptions.Location = New System.Drawing.Point(34, 150)
        Me.lblOptions.Name = "lblOptions"
        Me.lblOptions.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblOptions.Size = New System.Drawing.Size(65, 17)
        Me.lblOptions.TabIndex = 22
        Me.lblOptions.Text = "Options:"
        Me.lblOptions.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblReadHiChan
        '
        Me.lblReadHiChan.BackColor = System.Drawing.SystemColors.Window
        Me.lblReadHiChan.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblReadHiChan.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReadHiChan.ForeColor = System.Drawing.Color.Blue
        Me.lblReadHiChan.Location = New System.Drawing.Point(258, 134)
        Me.lblReadHiChan.Name = "lblReadHiChan"
        Me.lblReadHiChan.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblReadHiChan.Size = New System.Drawing.Size(55, 18)
        Me.lblReadHiChan.TabIndex = 15
        '
        'lblShowHiChan
        '
        Me.lblShowHiChan.BackColor = System.Drawing.SystemColors.Window
        Me.lblShowHiChan.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblShowHiChan.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShowHiChan.ForeColor = System.Drawing.Color.Blue
        Me.lblShowHiChan.Location = New System.Drawing.Point(114, 134)
        Me.lblShowHiChan.Name = "lblShowHiChan"
        Me.lblShowHiChan.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblShowHiChan.Size = New System.Drawing.Size(63, 18)
        Me.lblShowHiChan.TabIndex = 6
        '
        'lblHiChan
        '
        Me.lblHiChan.BackColor = System.Drawing.SystemColors.Window
        Me.lblHiChan.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblHiChan.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblHiChan.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblHiChan.Location = New System.Drawing.Point(10, 134)
        Me.lblHiChan.Name = "lblHiChan"
        Me.lblHiChan.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblHiChan.Size = New System.Drawing.Size(89, 17)
        Me.lblHiChan.TabIndex = 21
        Me.lblHiChan.Text = "High Channel:"
        Me.lblHiChan.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblReadLoChan
        '
        Me.lblReadLoChan.BackColor = System.Drawing.SystemColors.Window
        Me.lblReadLoChan.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblReadLoChan.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReadLoChan.ForeColor = System.Drawing.Color.Blue
        Me.lblReadLoChan.Location = New System.Drawing.Point(258, 118)
        Me.lblReadLoChan.Name = "lblReadLoChan"
        Me.lblReadLoChan.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblReadLoChan.Size = New System.Drawing.Size(63, 18)
        Me.lblReadLoChan.TabIndex = 14
        '
        'lblShowLoChan
        '
        Me.lblShowLoChan.BackColor = System.Drawing.SystemColors.Window
        Me.lblShowLoChan.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblShowLoChan.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShowLoChan.ForeColor = System.Drawing.Color.Blue
        Me.lblShowLoChan.Location = New System.Drawing.Point(114, 118)
        Me.lblShowLoChan.Name = "lblShowLoChan"
        Me.lblShowLoChan.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblShowLoChan.Size = New System.Drawing.Size(63, 18)
        Me.lblShowLoChan.TabIndex = 5
        '
        'lblLoChan
        '
        Me.lblLoChan.BackColor = System.Drawing.SystemColors.Window
        Me.lblLoChan.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblLoChan.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLoChan.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblLoChan.Location = New System.Drawing.Point(10, 118)
        Me.lblLoChan.Name = "lblLoChan"
        Me.lblLoChan.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblLoChan.Size = New System.Drawing.Size(89, 17)
        Me.lblLoChan.TabIndex = 20
        Me.lblLoChan.Text = "Low Channel:"
        Me.lblLoChan.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblReadRate
        '
        Me.lblReadRate.BackColor = System.Drawing.SystemColors.Window
        Me.lblReadRate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblReadRate.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReadRate.ForeColor = System.Drawing.Color.Blue
        Me.lblReadRate.Location = New System.Drawing.Point(258, 102)
        Me.lblReadRate.Name = "lblReadRate"
        Me.lblReadRate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblReadRate.Size = New System.Drawing.Size(63, 18)
        Me.lblReadRate.TabIndex = 13
        '
        'lblShowRate
        '
        Me.lblShowRate.BackColor = System.Drawing.SystemColors.Window
        Me.lblShowRate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblShowRate.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShowRate.ForeColor = System.Drawing.Color.Blue
        Me.lblShowRate.Location = New System.Drawing.Point(114, 102)
        Me.lblShowRate.Name = "lblShowRate"
        Me.lblShowRate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblShowRate.Size = New System.Drawing.Size(63, 18)
        Me.lblShowRate.TabIndex = 4
        '
        'lblRate
        '
        Me.lblRate.BackColor = System.Drawing.SystemColors.Window
        Me.lblRate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRate.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblRate.Location = New System.Drawing.Point(34, 102)
        Me.lblRate.Name = "lblRate"
        Me.lblRate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRate.Size = New System.Drawing.Size(65, 17)
        Me.lblRate.TabIndex = 19
        Me.lblRate.Text = "Rate:"
        Me.lblRate.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblInCol
        '
        Me.lblInCol.BackColor = System.Drawing.SystemColors.Window
        Me.lblInCol.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblInCol.Font = New System.Drawing.Font("Arial", 8.25!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInCol.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblInCol.Location = New System.Drawing.Point(226, 78)
        Me.lblInCol.Name = "lblInCol"
        Me.lblInCol.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblInCol.Size = New System.Drawing.Size(145, 17)
        Me.lblInCol.TabIndex = 27
        Me.lblInCol.Text = "Params Read from File"
        Me.lblInCol.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblOutCol
        '
        Me.lblOutCol.BackColor = System.Drawing.SystemColors.Window
        Me.lblOutCol.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblOutCol.Font = New System.Drawing.Font("Arial", 8.25!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOutCol.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblOutCol.Location = New System.Drawing.Point(90, 78)
        Me.lblOutCol.Name = "lblOutCol"
        Me.lblOutCol.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblOutCol.Size = New System.Drawing.Size(121, 17)
        Me.lblOutCol.TabIndex = 26
        Me.lblOutCol.Text = "Params to Function"
        Me.lblOutCol.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblAcqStat
        '
        Me.lblAcqStat.BackColor = System.Drawing.SystemColors.Window
        Me.lblAcqStat.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAcqStat.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAcqStat.ForeColor = System.Drawing.Color.Blue
        Me.lblAcqStat.Location = New System.Drawing.Point(12, 28)
        Me.lblAcqStat.Name = "lblAcqStat"
        Me.lblAcqStat.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAcqStat.Size = New System.Drawing.Size(387, 41)
        Me.lblAcqStat.TabIndex = 3
        Me.lblAcqStat.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblDemoFunction
        '
        Me.lblDemoFunction.BackColor = System.Drawing.SystemColors.Window
        Me.lblDemoFunction.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDemoFunction.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDemoFunction.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblDemoFunction.Location = New System.Drawing.Point(8, 3)
        Me.lblDemoFunction.Name = "lblDemoFunction"
        Me.lblDemoFunction.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDemoFunction.Size = New System.Drawing.Size(391, 15)
        Me.lblDemoFunction.TabIndex = 0
        Me.lblDemoFunction.Text = "Demonstration of Mccdaq.MccBoard.FileAInScan()"
        Me.lblDemoFunction.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'frmDataDisplay
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 13)
        Me.BackColor = System.Drawing.SystemColors.Window
        Me.ClientSize = New System.Drawing.Size(411, 347)
        Me.Controls.Add(Me.cmdStartAcq)
        Me.Controls.Add(Me.txtFileName)
        Me.Controls.Add(Me.lblFileInstruct)
        Me.Controls.Add(Me.lblReadFile)
        Me.Controls.Add(Me.lblShowFile)
        Me.Controls.Add(Me.lblFileName)
        Me.Controls.Add(Me.lblReadPreTrig)
        Me.Controls.Add(Me.lblShowPreTrig)
        Me.Controls.Add(Me.lblPreTrig)
        Me.Controls.Add(Me.lblReadTotal)
        Me.Controls.Add(Me.lblShowCount)
        Me.Controls.Add(Me.lblCount)
        Me.Controls.Add(Me.lblReadGain)
        Me.Controls.Add(Me.lblShowGain)
        Me.Controls.Add(Me.lblGain)
        Me.Controls.Add(Me.lblReadOptions)
        Me.Controls.Add(Me.lblShowOptions)
        Me.Controls.Add(Me.lblOptions)
        Me.Controls.Add(Me.lblReadHiChan)
        Me.Controls.Add(Me.lblShowHiChan)
        Me.Controls.Add(Me.lblHiChan)
        Me.Controls.Add(Me.lblReadLoChan)
        Me.Controls.Add(Me.lblShowLoChan)
        Me.Controls.Add(Me.lblLoChan)
        Me.Controls.Add(Me.lblReadRate)
        Me.Controls.Add(Me.lblShowRate)
        Me.Controls.Add(Me.lblRate)
        Me.Controls.Add(Me.lblInCol)
        Me.Controls.Add(Me.lblOutCol)
        Me.Controls.Add(Me.lblAcqStat)
        Me.Controls.Add(Me.lblDemoFunction)
        Me.Controls.Add(Me.cmdStopConvert)
        Me.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ForeColor = System.Drawing.Color.Blue
        Me.Location = New System.Drawing.Point(7, 103)
        Me.Name = "frmDataDisplay"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Universal Library Analog Input to File"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

#Region "Universal Library Initialization - Expand this region to change error handling, etc."

    Private Sub InitUL()

        Dim ULStat As MccDaq.ErrorInfo

        ' declare revision level of Universal Library

        ULStat = MccDaq.MccService.DeclareRevision(MccDaq.MccService.CurrentRevNum)

        ' Initiate error handling
        '  activating error handling will trap errors like
        '  bad channel numbers and non-configured conditions.
        '  Parameters:
        '    MccDaq.ErrorReporting.PrintAll :all warnings and errors encountered will be handled locally
        '    MccDaq.ErrorHandling.StopAll   :if any error is encountered, the program will not stop

        ReportError = MccDaq.ErrorReporting.DontPrint
        HandleError = MccDaq.ErrorHandling.DontStop
        ULStat = MccDaq.MccService.ErrHandling(ReportError, HandleError)
        If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then
            Stop
        End If

    End Sub

#End Region

End Class