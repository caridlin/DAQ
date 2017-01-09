'==============================================================================

' File:                         ULFI03.VB

' Library Call Demonstrated:    File Operations:
'                               Mccdaq.MccBoard.FilePretrig()
'                               MccDaq.MccService.FileRead()
'                               MccDaq.MccService.FileGetInfo()

' Purpose:                      Stream data continuously to a streamer file
'                               until a trigger is received, continue data
'                               streaming until total number of samples minus
'                               the number of pretrigger samples is reached.

' Demonstration:                Creates a file and scans analog data to the
'                               file continuously, overwriting previous data.
'                               When a trigger is received, acquisition stops
'                               after (TotalCount& - PreTrigCount&) samples
'                               are stored. Displays the data in the file and
'                               the information in the file header. Prints
'                               data from PreTrigger-10 to PreTrigger+10.

' Other Library Calls:          MccDaq.MccService.ErrHandling()

' Special Requirements:         Board 0 must be capable of Pretrigger.

'==============================================================================
Option Strict Off
Option Explicit On

Friend Class frmFilePreTrig

    Inherits System.Windows.Forms.Form

    'Create a new MccBoard object for Board 0
    Private DaqBoard As MccDaq.MccBoard = New MccDaq.MccBoard(0)

    Private Range As MccDaq.Range
    Private ADResolution, NumAIChans As Integer
    Private HighChan, LowChan As Integer
    Dim DefaultTrig As MccDaq.TriggerType

    ' set buffer size large enough to hold all data
    Const TestPoints As Integer = 4096 ' Number of data points to collect
    Private Rate As Integer = 1000

    Private Sub frmFilePreTrig_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        InitUL()

        ' determine the number of analog channels and their capabilities
        Dim ChannelType As Integer = PRETRIGIN
        NumAIChans = FindAnalogChansOfType(DaqBoard, ChannelType, _
            ADResolution, Range, LowChan, DefaultTrig)

        If (NumAIChans = 0) Then
            lblAcqStat.Text = "Board " & DaqBoard.BoardNum.ToString() & _
            " does not have analog input channels that support pretrigger."
            lblAcqStat.ForeColor = Color.Red
            cmdTrigEnable.Enabled = False
        Else
            lblAcqStat.Text = "Board " & DaqBoard.BoardNum.ToString() & _
                " collecting analog data on channel 0 using FilePretrig in " & _
                "foreground mode with Range set to " & Range.ToString() & "."
        End If

    End Sub

    Private Sub cmdTrigEnable_Click(ByVal eventSender As System.Object, _
    ByVal eventArgs As System.EventArgs) Handles cmdTrigEnable.Click

        Dim i As Short
        Dim FirstPoint As Integer
        Dim NumPoints As Integer
        Dim ULStat As MccDaq.ErrorInfo
        Dim Options As MccDaq.ScanOptions
        Dim HighChan As Short
        Dim LowChan As Short
        Dim FileName As String
        Dim PretrigCount As Integer
        Dim TotalCount As Integer
        Dim DataAvailable As Boolean

        cmdTrigEnable.Enabled = False
        lblAcqStat.ForeColor = Color.Blue
        lblAcqStat.Text = _
            "Waiting for trigger on trigger input and acquiring data."
        Cursor = Cursors.WaitCursor
        Application.DoEvents()
        DataAvailable = False

        If DefaultTrig = MccDaq.TriggerType.TrigAbove Then
            'The default trigger configuration for most devices is
            'rising edge digital trigger, but some devices do not
            'support this type for pretrigger functions.
            Dim EngUnits As Single
            Dim MidScale As Short
            MidScale = ((2 ^ ADResolution) / 2) - 1
            ULStat = DaqBoard.SetTrigger(DefaultTrig, MidScale, MidScale)
            ULStat = DaqBoard.ToEngUnits(Range, MidScale, EngUnits)
            lblAcqStat.Text = "Waiting for trigger on analog input above " _
                & Format(EngUnits, "0.00") & "V."
        End If

        ' Monitor a range of channels for a trigger then collect 
        ' the values with MccDaq.MccBoard.FilePretrig()
        ' Parameters:
        '   FileName      :file where data will be stored
        '   LowChan       :first A/D channel of the scan
        '   HighChan      :last A/D channel of the scan
        '   PretrigCount  :number of pre-trigger A/D samples to collect
        '   TotalCount    :total number of A/D samples to collect
        '   Rate          :sample rate in samples per second
        '   Gain          :the gain for the board
        '   Options       :data collection options

        TotalCount = TestPoints
        PretrigCount = 200
        FileName = txtFileName.Text ' it may be necessary to specify path here
        LowChan = 0
        HighChan = 0
        Options = MccDaq.ScanOptions.Default

        ULStat = DaqBoard.FilePretrig(LowChan, HighChan, PretrigCount, _
            TotalCount, Rate, Range, FileName, Options)
        If ULStat.Value = MccDaq.ErrorInfo.ErrorCode.BadFileName Then
            MsgBox("Enter the name of the file in which to store " & _
                "the data in the text box.", 0, "Bad File Name")
            cmdTrigEnable.Enabled = True
            txtFileName.Focus()
            Cursor = Cursors.Default
            Exit Sub
        End If
        Cursor = Cursors.Default

        If ULStat.Value = MccDaq.ErrorInfo.ErrorCode.BadBoardType Then
            lblAcqStat.Text = "Board " & DaqBoard.BoardNum.ToString() & _
            " doesn't support the cbAPretrig function."
            lblAcqStat.ForeColor = Color.Red
        ElseIf ULStat.Value = MccDaq.ErrorInfo.ErrorCode.TooFew Then
            lblAcqStat.Text = "Premature trigger occurred at sample " _
            & (PretrigCount - 1).ToString() & "."
            DataAvailable = True
        ElseIf ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then
            lblAcqStat.Text = ULStat.Message & "."
            lblAcqStat.ForeColor = Color.Red
            Application.DoEvents()
        Else
            lblAcqStat.Text = ""
            DataAvailable = True
        End If

        If DataAvailable Then
            ' show the information in the file header with MccDaq.MccService.FileGetInfo
            '  Parameters:
            '    FileName      :the filename containing the data
            '    LowChan       :first A/D channel of the scan
            '    HighChan      :last A/D channel of the scan
            '    PreTrigCount  :the number of pretrigger samples in the file
            '    Count       :the total number of A/D samples in the file
            '    Rate        :sample rate 
            '    Range          :the gain at which the samples were collected

            ULStat = MccDaq.MccService.FileGetInfo(FileName, LowChan, HighChan, PretrigCount, TotalCount, Rate, Range)
            If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then Stop

            lblShowFileName.Text = FileName
            lblShowLoChan.Text = LowChan.ToString("0")
            lblShowHiChan.Text = HighChan.ToString("0")
            lblShowPT.Text = PretrigCount.ToString("0")
            lblShowNumSam.Text = TotalCount.ToString("0")
            lblShowRate.Text = Rate.ToString("0")
            lblShowGain.Text = Range.ToString()

            ' show the data using MccDaq.MccService.FileRead()
            '  Parameters:
            '    FileName      :the filename containing the data
            '    NumPoints     :the number of data values to read from the file
            '    FirstPoint    :index of the first data value to read
            '    DataBuffer()  :array to read data into

            NumPoints = 20 ' read the first twenty data points
            FirstPoint = PretrigCount - 11 ' start at the trigger - 10
            If FirstPoint < 0 Then FirstPoint = 0
            Dim DataBuffer(NumPoints) As UInt16

            ULStat = MccDaq.MccService.FileRead(FileName, FirstPoint, NumPoints, DataBuffer)
            If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then Stop

            For i = 0 To 19
                lblPreTrig(i).Text = DataBuffer(i).ToString("0")
                lblPre(i).Text = (FirstPoint + i).ToString()
            Next i
        End If

        cmdTrigEnable.Enabled = True

    End Sub

    Private Sub cmdQuit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdQuit.Click

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
    Public WithEvents txtFileName As System.Windows.Forms.TextBox
    Public WithEvents cmdQuit As System.Windows.Forms.Button
    Public WithEvents cmdTrigEnable As System.Windows.Forms.Button
    Public WithEvents lblFileInstruct As System.Windows.Forms.Label
    Public WithEvents lblShowGain As System.Windows.Forms.Label
    Public WithEvents lblGain As System.Windows.Forms.Label
    Public WithEvents lblShowRate As System.Windows.Forms.Label
    Public WithEvents lblRate As System.Windows.Forms.Label
    Public WithEvents lblShowNumSam As System.Windows.Forms.Label
    Public WithEvents lblNumSam As System.Windows.Forms.Label
    Public WithEvents lblShowPT As System.Windows.Forms.Label
    Public WithEvents lblNumPTSam As System.Windows.Forms.Label
    Public WithEvents lblShowHiChan As System.Windows.Forms.Label
    Public WithEvents lblHiChan As System.Windows.Forms.Label
    Public WithEvents lblShowLoChan As System.Windows.Forms.Label
    Public WithEvents lblLoChan As System.Windows.Forms.Label
    Public WithEvents lblShowFileName As System.Windows.Forms.Label
    Public WithEvents lblFileName As System.Windows.Forms.Label
    Public WithEvents _lblPostTrig_10 As System.Windows.Forms.Label
    Public WithEvents lblPost10 As System.Windows.Forms.Label
    Public WithEvents _lblPreTrig_9 As System.Windows.Forms.Label
    Public WithEvents lblPre1 As System.Windows.Forms.Label
    Public WithEvents _lblPostTrig_9 As System.Windows.Forms.Label
    Public WithEvents lblPost9 As System.Windows.Forms.Label
    Public WithEvents _lblPreTrig_8 As System.Windows.Forms.Label
    Public WithEvents lblPre2 As System.Windows.Forms.Label
    Public WithEvents _lblPostTrig_8 As System.Windows.Forms.Label
    Public WithEvents lblPost8 As System.Windows.Forms.Label
    Public WithEvents _lblPreTrig_7 As System.Windows.Forms.Label
    Public WithEvents lblPre3 As System.Windows.Forms.Label
    Public WithEvents _lblPostTrig_7 As System.Windows.Forms.Label
    Public WithEvents lblPost7 As System.Windows.Forms.Label
    Public WithEvents _lblPreTrig_6 As System.Windows.Forms.Label
    Public WithEvents lblPre4 As System.Windows.Forms.Label
    Public WithEvents _lblPostTrig_6 As System.Windows.Forms.Label
    Public WithEvents lblPost6 As System.Windows.Forms.Label
    Public WithEvents _lblPreTrig_5 As System.Windows.Forms.Label
    Public WithEvents lblPre5 As System.Windows.Forms.Label
    Public WithEvents _lblPostTrig_5 As System.Windows.Forms.Label
    Public WithEvents lblPost5 As System.Windows.Forms.Label
    Public WithEvents _lblPreTrig_4 As System.Windows.Forms.Label
    Public WithEvents lblPre6 As System.Windows.Forms.Label
    Public WithEvents _lblPostTrig_4 As System.Windows.Forms.Label
    Public WithEvents lblPost4 As System.Windows.Forms.Label
    Public WithEvents _lblPreTrig_3 As System.Windows.Forms.Label
    Public WithEvents lblPre7 As System.Windows.Forms.Label
    Public WithEvents _lblPostTrig_2 As System.Windows.Forms.Label
    Public WithEvents lblPost3 As System.Windows.Forms.Label
    Public WithEvents _lblPreTrig_2 As System.Windows.Forms.Label
    Public WithEvents lblPre8 As System.Windows.Forms.Label
    Public WithEvents _lblPostTrig_3 As System.Windows.Forms.Label
    Public WithEvents lblPost2 As System.Windows.Forms.Label
    Public WithEvents _lblPreTrig_1 As System.Windows.Forms.Label
    Public WithEvents lblPre9 As System.Windows.Forms.Label
    Public WithEvents _lblPostTrig_1 As System.Windows.Forms.Label
    Public WithEvents lblPost1 As System.Windows.Forms.Label
    Public WithEvents _lblPreTrig_0 As System.Windows.Forms.Label
    Public WithEvents lblPre10 As System.Windows.Forms.Label
    Public WithEvents lblPostTrigData As System.Windows.Forms.Label
    Public WithEvents lblPreTrigData As System.Windows.Forms.Label
    Public WithEvents lblDemoFunction As System.Windows.Forms.Label
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.txtFileName = New System.Windows.Forms.TextBox
        Me.cmdQuit = New System.Windows.Forms.Button
        Me.cmdTrigEnable = New System.Windows.Forms.Button
        Me.lblFileInstruct = New System.Windows.Forms.Label
        Me.lblShowGain = New System.Windows.Forms.Label
        Me.lblGain = New System.Windows.Forms.Label
        Me.lblShowRate = New System.Windows.Forms.Label
        Me.lblRate = New System.Windows.Forms.Label
        Me.lblShowNumSam = New System.Windows.Forms.Label
        Me.lblNumSam = New System.Windows.Forms.Label
        Me.lblShowPT = New System.Windows.Forms.Label
        Me.lblNumPTSam = New System.Windows.Forms.Label
        Me.lblShowHiChan = New System.Windows.Forms.Label
        Me.lblHiChan = New System.Windows.Forms.Label
        Me.lblShowLoChan = New System.Windows.Forms.Label
        Me.lblLoChan = New System.Windows.Forms.Label
        Me.lblShowFileName = New System.Windows.Forms.Label
        Me.lblFileName = New System.Windows.Forms.Label
        Me._lblPostTrig_10 = New System.Windows.Forms.Label
        Me.lblPost10 = New System.Windows.Forms.Label
        Me._lblPreTrig_9 = New System.Windows.Forms.Label
        Me.lblPre1 = New System.Windows.Forms.Label
        Me._lblPostTrig_9 = New System.Windows.Forms.Label
        Me.lblPost9 = New System.Windows.Forms.Label
        Me._lblPreTrig_8 = New System.Windows.Forms.Label
        Me.lblPre2 = New System.Windows.Forms.Label
        Me._lblPostTrig_8 = New System.Windows.Forms.Label
        Me.lblPost8 = New System.Windows.Forms.Label
        Me._lblPreTrig_7 = New System.Windows.Forms.Label
        Me.lblPre3 = New System.Windows.Forms.Label
        Me._lblPostTrig_7 = New System.Windows.Forms.Label
        Me.lblPost7 = New System.Windows.Forms.Label
        Me._lblPreTrig_6 = New System.Windows.Forms.Label
        Me.lblPre4 = New System.Windows.Forms.Label
        Me._lblPostTrig_6 = New System.Windows.Forms.Label
        Me.lblPost6 = New System.Windows.Forms.Label
        Me._lblPreTrig_5 = New System.Windows.Forms.Label
        Me.lblPre5 = New System.Windows.Forms.Label
        Me._lblPostTrig_5 = New System.Windows.Forms.Label
        Me.lblPost5 = New System.Windows.Forms.Label
        Me._lblPreTrig_4 = New System.Windows.Forms.Label
        Me.lblPre6 = New System.Windows.Forms.Label
        Me._lblPostTrig_4 = New System.Windows.Forms.Label
        Me.lblPost4 = New System.Windows.Forms.Label
        Me._lblPreTrig_3 = New System.Windows.Forms.Label
        Me.lblPre7 = New System.Windows.Forms.Label
        Me._lblPostTrig_2 = New System.Windows.Forms.Label
        Me.lblPost3 = New System.Windows.Forms.Label
        Me._lblPreTrig_2 = New System.Windows.Forms.Label
        Me.lblPre8 = New System.Windows.Forms.Label
        Me._lblPostTrig_3 = New System.Windows.Forms.Label
        Me.lblPost2 = New System.Windows.Forms.Label
        Me._lblPreTrig_1 = New System.Windows.Forms.Label
        Me.lblPre9 = New System.Windows.Forms.Label
        Me._lblPostTrig_1 = New System.Windows.Forms.Label
        Me.lblPost1 = New System.Windows.Forms.Label
        Me._lblPreTrig_0 = New System.Windows.Forms.Label
        Me.lblPre10 = New System.Windows.Forms.Label
        Me.lblPostTrigData = New System.Windows.Forms.Label
        Me.lblPreTrigData = New System.Windows.Forms.Label
        Me.lblDemoFunction = New System.Windows.Forms.Label
        Me.lblAcqStat = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'txtFileName
        '
        Me.txtFileName.AcceptsReturn = True
        Me.txtFileName.BackColor = System.Drawing.SystemColors.Window
        Me.txtFileName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtFileName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtFileName.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFileName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtFileName.Location = New System.Drawing.Point(187, 361)
        Me.txtFileName.MaxLength = 0
        Me.txtFileName.Name = "txtFileName"
        Me.txtFileName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtFileName.Size = New System.Drawing.Size(161, 20)
        Me.txtFileName.TabIndex = 63
        Me.txtFileName.Text = "DEMO.DAT"
        '
        'cmdQuit
        '
        Me.cmdQuit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdQuit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdQuit.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdQuit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdQuit.Location = New System.Drawing.Point(292, 309)
        Me.cmdQuit.Name = "cmdQuit"
        Me.cmdQuit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdQuit.Size = New System.Drawing.Size(89, 26)
        Me.cmdQuit.TabIndex = 17
        Me.cmdQuit.Text = "Quit"
        Me.cmdQuit.UseVisualStyleBackColor = False
        '
        'cmdTrigEnable
        '
        Me.cmdTrigEnable.BackColor = System.Drawing.SystemColors.Control
        Me.cmdTrigEnable.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdTrigEnable.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdTrigEnable.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdTrigEnable.Location = New System.Drawing.Point(292, 276)
        Me.cmdTrigEnable.Name = "cmdTrigEnable"
        Me.cmdTrigEnable.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdTrigEnable.Size = New System.Drawing.Size(89, 26)
        Me.cmdTrigEnable.TabIndex = 18
        Me.cmdTrigEnable.Text = "Enable Trigger"
        Me.cmdTrigEnable.UseVisualStyleBackColor = False
        '
        'lblFileInstruct
        '
        Me.lblFileInstruct.BackColor = System.Drawing.SystemColors.Window
        Me.lblFileInstruct.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblFileInstruct.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFileInstruct.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblFileInstruct.Location = New System.Drawing.Point(11, 345)
        Me.lblFileInstruct.Name = "lblFileInstruct"
        Me.lblFileInstruct.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblFileInstruct.Size = New System.Drawing.Size(169, 41)
        Me.lblFileInstruct.TabIndex = 62
        Me.lblFileInstruct.Text = "Enter the name of the file that you have created using MAKESTRM.EXE"
        Me.lblFileInstruct.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblShowGain
        '
        Me.lblShowGain.BackColor = System.Drawing.SystemColors.Window
        Me.lblShowGain.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblShowGain.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShowGain.ForeColor = System.Drawing.Color.Blue
        Me.lblShowGain.Location = New System.Drawing.Point(184, 327)
        Me.lblShowGain.Name = "lblShowGain"
        Me.lblShowGain.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblShowGain.Size = New System.Drawing.Size(52, 14)
        Me.lblShowGain.TabIndex = 61
        '
        'lblGain
        '
        Me.lblGain.BackColor = System.Drawing.SystemColors.Window
        Me.lblGain.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblGain.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblGain.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblGain.Location = New System.Drawing.Point(49, 327)
        Me.lblGain.Name = "lblGain"
        Me.lblGain.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblGain.Size = New System.Drawing.Size(129, 14)
        Me.lblGain.TabIndex = 54
        Me.lblGain.Text = "Gain:"
        Me.lblGain.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblShowRate
        '
        Me.lblShowRate.BackColor = System.Drawing.SystemColors.Window
        Me.lblShowRate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblShowRate.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShowRate.ForeColor = System.Drawing.Color.Blue
        Me.lblShowRate.Location = New System.Drawing.Point(184, 314)
        Me.lblShowRate.Name = "lblShowRate"
        Me.lblShowRate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblShowRate.Size = New System.Drawing.Size(52, 14)
        Me.lblShowRate.TabIndex = 60
        '
        'lblRate
        '
        Me.lblRate.BackColor = System.Drawing.SystemColors.Window
        Me.lblRate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRate.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblRate.Location = New System.Drawing.Point(49, 314)
        Me.lblRate.Name = "lblRate"
        Me.lblRate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRate.Size = New System.Drawing.Size(129, 14)
        Me.lblRate.TabIndex = 53
        Me.lblRate.Text = "Collection Rate:"
        Me.lblRate.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblShowNumSam
        '
        Me.lblShowNumSam.BackColor = System.Drawing.SystemColors.Window
        Me.lblShowNumSam.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblShowNumSam.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShowNumSam.ForeColor = System.Drawing.Color.Blue
        Me.lblShowNumSam.Location = New System.Drawing.Point(184, 301)
        Me.lblShowNumSam.Name = "lblShowNumSam"
        Me.lblShowNumSam.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblShowNumSam.Size = New System.Drawing.Size(52, 14)
        Me.lblShowNumSam.TabIndex = 59
        '
        'lblNumSam
        '
        Me.lblNumSam.BackColor = System.Drawing.SystemColors.Window
        Me.lblNumSam.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblNumSam.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNumSam.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblNumSam.Location = New System.Drawing.Point(49, 301)
        Me.lblNumSam.Name = "lblNumSam"
        Me.lblNumSam.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblNumSam.Size = New System.Drawing.Size(129, 14)
        Me.lblNumSam.TabIndex = 52
        Me.lblNumSam.Text = "No. of Samples:"
        Me.lblNumSam.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblShowPT
        '
        Me.lblShowPT.BackColor = System.Drawing.SystemColors.Window
        Me.lblShowPT.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblShowPT.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShowPT.ForeColor = System.Drawing.Color.Blue
        Me.lblShowPT.Location = New System.Drawing.Point(184, 288)
        Me.lblShowPT.Name = "lblShowPT"
        Me.lblShowPT.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblShowPT.Size = New System.Drawing.Size(52, 14)
        Me.lblShowPT.TabIndex = 58
        '
        'lblNumPTSam
        '
        Me.lblNumPTSam.BackColor = System.Drawing.SystemColors.Window
        Me.lblNumPTSam.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblNumPTSam.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNumPTSam.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblNumPTSam.Location = New System.Drawing.Point(33, 288)
        Me.lblNumPTSam.Name = "lblNumPTSam"
        Me.lblNumPTSam.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblNumPTSam.Size = New System.Drawing.Size(145, 14)
        Me.lblNumPTSam.TabIndex = 51
        Me.lblNumPTSam.Text = "No. of Pretrig Samples:"
        Me.lblNumPTSam.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblShowHiChan
        '
        Me.lblShowHiChan.BackColor = System.Drawing.SystemColors.Window
        Me.lblShowHiChan.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblShowHiChan.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShowHiChan.ForeColor = System.Drawing.Color.Blue
        Me.lblShowHiChan.Location = New System.Drawing.Point(184, 275)
        Me.lblShowHiChan.Name = "lblShowHiChan"
        Me.lblShowHiChan.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblShowHiChan.Size = New System.Drawing.Size(52, 14)
        Me.lblShowHiChan.TabIndex = 57
        '
        'lblHiChan
        '
        Me.lblHiChan.BackColor = System.Drawing.SystemColors.Window
        Me.lblHiChan.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblHiChan.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblHiChan.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblHiChan.Location = New System.Drawing.Point(49, 275)
        Me.lblHiChan.Name = "lblHiChan"
        Me.lblHiChan.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblHiChan.Size = New System.Drawing.Size(129, 14)
        Me.lblHiChan.TabIndex = 50
        Me.lblHiChan.Text = "High Channel:"
        Me.lblHiChan.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblShowLoChan
        '
        Me.lblShowLoChan.BackColor = System.Drawing.SystemColors.Window
        Me.lblShowLoChan.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblShowLoChan.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShowLoChan.ForeColor = System.Drawing.Color.Blue
        Me.lblShowLoChan.Location = New System.Drawing.Point(184, 263)
        Me.lblShowLoChan.Name = "lblShowLoChan"
        Me.lblShowLoChan.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblShowLoChan.Size = New System.Drawing.Size(52, 14)
        Me.lblShowLoChan.TabIndex = 56
        '
        'lblLoChan
        '
        Me.lblLoChan.BackColor = System.Drawing.SystemColors.Window
        Me.lblLoChan.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblLoChan.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLoChan.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblLoChan.Location = New System.Drawing.Point(49, 263)
        Me.lblLoChan.Name = "lblLoChan"
        Me.lblLoChan.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblLoChan.Size = New System.Drawing.Size(129, 14)
        Me.lblLoChan.TabIndex = 49
        Me.lblLoChan.Text = "Low Channel:"
        Me.lblLoChan.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblShowFileName
        '
        Me.lblShowFileName.BackColor = System.Drawing.SystemColors.Window
        Me.lblShowFileName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblShowFileName.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShowFileName.ForeColor = System.Drawing.Color.Blue
        Me.lblShowFileName.Location = New System.Drawing.Point(184, 250)
        Me.lblShowFileName.Name = "lblShowFileName"
        Me.lblShowFileName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblShowFileName.Size = New System.Drawing.Size(183, 14)
        Me.lblShowFileName.TabIndex = 55
        '
        'lblFileName
        '
        Me.lblFileName.BackColor = System.Drawing.SystemColors.Window
        Me.lblFileName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblFileName.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFileName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblFileName.Location = New System.Drawing.Point(49, 250)
        Me.lblFileName.Name = "lblFileName"
        Me.lblFileName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblFileName.Size = New System.Drawing.Size(129, 14)
        Me.lblFileName.TabIndex = 48
        Me.lblFileName.Text = "Streamer File Name:"
        Me.lblFileName.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblPostTrig_10
        '
        Me._lblPostTrig_10.BackColor = System.Drawing.SystemColors.Window
        Me._lblPostTrig_10.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblPostTrig_10.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblPostTrig_10.ForeColor = System.Drawing.Color.Blue
        Me._lblPostTrig_10.Location = New System.Drawing.Point(299, 216)
        Me._lblPostTrig_10.Name = "_lblPostTrig_10"
        Me._lblPostTrig_10.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblPostTrig_10.Size = New System.Drawing.Size(65, 13)
        Me._lblPostTrig_10.TabIndex = 42
        Me._lblPostTrig_10.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblPost10
        '
        Me.lblPost10.BackColor = System.Drawing.SystemColors.Window
        Me.lblPost10.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPost10.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPost10.ForeColor = System.Drawing.Color.Blue
        Me.lblPost10.Location = New System.Drawing.Point(207, 216)
        Me.lblPost10.Name = "lblPost10"
        Me.lblPost10.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPost10.Size = New System.Drawing.Size(73, 13)
        Me.lblPost10.TabIndex = 40
        Me.lblPost10.Text = "Trigger +9"
        Me.lblPost10.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblPreTrig_9
        '
        Me._lblPreTrig_9.BackColor = System.Drawing.SystemColors.Window
        Me._lblPreTrig_9.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblPreTrig_9.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblPreTrig_9.ForeColor = System.Drawing.Color.Blue
        Me._lblPreTrig_9.Location = New System.Drawing.Point(106, 216)
        Me._lblPreTrig_9.Name = "_lblPreTrig_9"
        Me._lblPreTrig_9.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblPreTrig_9.Size = New System.Drawing.Size(65, 13)
        Me._lblPreTrig_9.TabIndex = 22
        Me._lblPreTrig_9.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblPre1
        '
        Me.lblPre1.BackColor = System.Drawing.SystemColors.Window
        Me.lblPre1.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPre1.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPre1.ForeColor = System.Drawing.Color.Blue
        Me.lblPre1.Location = New System.Drawing.Point(19, 216)
        Me.lblPre1.Name = "lblPre1"
        Me.lblPre1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPre1.Size = New System.Drawing.Size(73, 13)
        Me.lblPre1.TabIndex = 20
        Me.lblPre1.Text = "Trigger -1"
        Me.lblPre1.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblPostTrig_9
        '
        Me._lblPostTrig_9.BackColor = System.Drawing.SystemColors.Window
        Me._lblPostTrig_9.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblPostTrig_9.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblPostTrig_9.ForeColor = System.Drawing.Color.Blue
        Me._lblPostTrig_9.Location = New System.Drawing.Point(299, 202)
        Me._lblPostTrig_9.Name = "_lblPostTrig_9"
        Me._lblPostTrig_9.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblPostTrig_9.Size = New System.Drawing.Size(65, 13)
        Me._lblPostTrig_9.TabIndex = 41
        Me._lblPostTrig_9.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblPost9
        '
        Me.lblPost9.BackColor = System.Drawing.SystemColors.Window
        Me.lblPost9.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPost9.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPost9.ForeColor = System.Drawing.Color.Blue
        Me.lblPost9.Location = New System.Drawing.Point(207, 202)
        Me.lblPost9.Name = "lblPost9"
        Me.lblPost9.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPost9.Size = New System.Drawing.Size(73, 13)
        Me.lblPost9.TabIndex = 39
        Me.lblPost9.Text = "Trigger +8"
        Me.lblPost9.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblPreTrig_8
        '
        Me._lblPreTrig_8.BackColor = System.Drawing.SystemColors.Window
        Me._lblPreTrig_8.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblPreTrig_8.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblPreTrig_8.ForeColor = System.Drawing.Color.Blue
        Me._lblPreTrig_8.Location = New System.Drawing.Point(106, 202)
        Me._lblPreTrig_8.Name = "_lblPreTrig_8"
        Me._lblPreTrig_8.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblPreTrig_8.Size = New System.Drawing.Size(65, 13)
        Me._lblPreTrig_8.TabIndex = 21
        Me._lblPreTrig_8.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblPre2
        '
        Me.lblPre2.BackColor = System.Drawing.SystemColors.Window
        Me.lblPre2.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPre2.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPre2.ForeColor = System.Drawing.Color.Blue
        Me.lblPre2.Location = New System.Drawing.Point(19, 202)
        Me.lblPre2.Name = "lblPre2"
        Me.lblPre2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPre2.Size = New System.Drawing.Size(73, 13)
        Me.lblPre2.TabIndex = 19
        Me.lblPre2.Text = "Trigger -2"
        Me.lblPre2.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblPostTrig_8
        '
        Me._lblPostTrig_8.BackColor = System.Drawing.SystemColors.Window
        Me._lblPostTrig_8.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblPostTrig_8.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblPostTrig_8.ForeColor = System.Drawing.Color.Blue
        Me._lblPostTrig_8.Location = New System.Drawing.Point(299, 188)
        Me._lblPostTrig_8.Name = "_lblPostTrig_8"
        Me._lblPostTrig_8.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblPostTrig_8.Size = New System.Drawing.Size(65, 13)
        Me._lblPostTrig_8.TabIndex = 38
        Me._lblPostTrig_8.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblPost8
        '
        Me.lblPost8.BackColor = System.Drawing.SystemColors.Window
        Me.lblPost8.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPost8.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPost8.ForeColor = System.Drawing.Color.Blue
        Me.lblPost8.Location = New System.Drawing.Point(207, 188)
        Me.lblPost8.Name = "lblPost8"
        Me.lblPost8.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPost8.Size = New System.Drawing.Size(73, 13)
        Me.lblPost8.TabIndex = 37
        Me.lblPost8.Text = "Trigger +7"
        Me.lblPost8.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblPreTrig_7
        '
        Me._lblPreTrig_7.BackColor = System.Drawing.SystemColors.Window
        Me._lblPreTrig_7.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblPreTrig_7.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblPreTrig_7.ForeColor = System.Drawing.Color.Blue
        Me._lblPreTrig_7.Location = New System.Drawing.Point(106, 188)
        Me._lblPreTrig_7.Name = "_lblPreTrig_7"
        Me._lblPreTrig_7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblPreTrig_7.Size = New System.Drawing.Size(65, 13)
        Me._lblPreTrig_7.TabIndex = 16
        Me._lblPreTrig_7.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblPre3
        '
        Me.lblPre3.BackColor = System.Drawing.SystemColors.Window
        Me.lblPre3.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPre3.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPre3.ForeColor = System.Drawing.Color.Blue
        Me.lblPre3.Location = New System.Drawing.Point(19, 188)
        Me.lblPre3.Name = "lblPre3"
        Me.lblPre3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPre3.Size = New System.Drawing.Size(73, 13)
        Me.lblPre3.TabIndex = 8
        Me.lblPre3.Text = "Trigger -3"
        Me.lblPre3.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblPostTrig_7
        '
        Me._lblPostTrig_7.BackColor = System.Drawing.SystemColors.Window
        Me._lblPostTrig_7.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblPostTrig_7.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblPostTrig_7.ForeColor = System.Drawing.Color.Blue
        Me._lblPostTrig_7.Location = New System.Drawing.Point(299, 174)
        Me._lblPostTrig_7.Name = "_lblPostTrig_7"
        Me._lblPostTrig_7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblPostTrig_7.Size = New System.Drawing.Size(65, 13)
        Me._lblPostTrig_7.TabIndex = 34
        Me._lblPostTrig_7.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblPost7
        '
        Me.lblPost7.BackColor = System.Drawing.SystemColors.Window
        Me.lblPost7.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPost7.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPost7.ForeColor = System.Drawing.Color.Blue
        Me.lblPost7.Location = New System.Drawing.Point(207, 174)
        Me.lblPost7.Name = "lblPost7"
        Me.lblPost7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPost7.Size = New System.Drawing.Size(73, 14)
        Me.lblPost7.TabIndex = 33
        Me.lblPost7.Text = "Trigger +6"
        Me.lblPost7.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblPreTrig_6
        '
        Me._lblPreTrig_6.BackColor = System.Drawing.SystemColors.Window
        Me._lblPreTrig_6.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblPreTrig_6.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblPreTrig_6.ForeColor = System.Drawing.Color.Blue
        Me._lblPreTrig_6.Location = New System.Drawing.Point(106, 174)
        Me._lblPreTrig_6.Name = "_lblPreTrig_6"
        Me._lblPreTrig_6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblPreTrig_6.Size = New System.Drawing.Size(65, 13)
        Me._lblPreTrig_6.TabIndex = 15
        Me._lblPreTrig_6.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblPre4
        '
        Me.lblPre4.BackColor = System.Drawing.SystemColors.Window
        Me.lblPre4.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPre4.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPre4.ForeColor = System.Drawing.Color.Blue
        Me.lblPre4.Location = New System.Drawing.Point(19, 174)
        Me.lblPre4.Name = "lblPre4"
        Me.lblPre4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPre4.Size = New System.Drawing.Size(73, 13)
        Me.lblPre4.TabIndex = 7
        Me.lblPre4.Text = "Trigger -4"
        Me.lblPre4.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblPostTrig_6
        '
        Me._lblPostTrig_6.BackColor = System.Drawing.SystemColors.Window
        Me._lblPostTrig_6.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblPostTrig_6.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblPostTrig_6.ForeColor = System.Drawing.Color.Blue
        Me._lblPostTrig_6.Location = New System.Drawing.Point(299, 161)
        Me._lblPostTrig_6.Name = "_lblPostTrig_6"
        Me._lblPostTrig_6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblPostTrig_6.Size = New System.Drawing.Size(65, 13)
        Me._lblPostTrig_6.TabIndex = 30
        Me._lblPostTrig_6.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblPost6
        '
        Me.lblPost6.BackColor = System.Drawing.SystemColors.Window
        Me.lblPost6.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPost6.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPost6.ForeColor = System.Drawing.Color.Blue
        Me.lblPost6.Location = New System.Drawing.Point(207, 161)
        Me.lblPost6.Name = "lblPost6"
        Me.lblPost6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPost6.Size = New System.Drawing.Size(73, 13)
        Me.lblPost6.TabIndex = 29
        Me.lblPost6.Text = "Trigger +5"
        Me.lblPost6.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblPreTrig_5
        '
        Me._lblPreTrig_5.BackColor = System.Drawing.SystemColors.Window
        Me._lblPreTrig_5.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblPreTrig_5.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblPreTrig_5.ForeColor = System.Drawing.Color.Blue
        Me._lblPreTrig_5.Location = New System.Drawing.Point(106, 161)
        Me._lblPreTrig_5.Name = "_lblPreTrig_5"
        Me._lblPreTrig_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblPreTrig_5.Size = New System.Drawing.Size(65, 13)
        Me._lblPreTrig_5.TabIndex = 14
        Me._lblPreTrig_5.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblPre5
        '
        Me.lblPre5.BackColor = System.Drawing.SystemColors.Window
        Me.lblPre5.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPre5.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPre5.ForeColor = System.Drawing.Color.Blue
        Me.lblPre5.Location = New System.Drawing.Point(19, 161)
        Me.lblPre5.Name = "lblPre5"
        Me.lblPre5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPre5.Size = New System.Drawing.Size(73, 13)
        Me.lblPre5.TabIndex = 6
        Me.lblPre5.Text = "Trigger -5"
        Me.lblPre5.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblPostTrig_5
        '
        Me._lblPostTrig_5.BackColor = System.Drawing.SystemColors.Window
        Me._lblPostTrig_5.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblPostTrig_5.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblPostTrig_5.ForeColor = System.Drawing.Color.Blue
        Me._lblPostTrig_5.Location = New System.Drawing.Point(299, 148)
        Me._lblPostTrig_5.Name = "_lblPostTrig_5"
        Me._lblPostTrig_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblPostTrig_5.Size = New System.Drawing.Size(65, 14)
        Me._lblPostTrig_5.TabIndex = 26
        Me._lblPostTrig_5.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblPost5
        '
        Me.lblPost5.BackColor = System.Drawing.SystemColors.Window
        Me.lblPost5.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPost5.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPost5.ForeColor = System.Drawing.Color.Blue
        Me.lblPost5.Location = New System.Drawing.Point(207, 148)
        Me.lblPost5.Name = "lblPost5"
        Me.lblPost5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPost5.Size = New System.Drawing.Size(73, 13)
        Me.lblPost5.TabIndex = 25
        Me.lblPost5.Text = "Trigger +4"
        Me.lblPost5.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblPreTrig_4
        '
        Me._lblPreTrig_4.BackColor = System.Drawing.SystemColors.Window
        Me._lblPreTrig_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblPreTrig_4.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblPreTrig_4.ForeColor = System.Drawing.Color.Blue
        Me._lblPreTrig_4.Location = New System.Drawing.Point(106, 148)
        Me._lblPreTrig_4.Name = "_lblPreTrig_4"
        Me._lblPreTrig_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblPreTrig_4.Size = New System.Drawing.Size(65, 13)
        Me._lblPreTrig_4.TabIndex = 13
        Me._lblPreTrig_4.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblPre6
        '
        Me.lblPre6.BackColor = System.Drawing.SystemColors.Window
        Me.lblPre6.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPre6.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPre6.ForeColor = System.Drawing.Color.Blue
        Me.lblPre6.Location = New System.Drawing.Point(19, 148)
        Me.lblPre6.Name = "lblPre6"
        Me.lblPre6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPre6.Size = New System.Drawing.Size(73, 13)
        Me.lblPre6.TabIndex = 5
        Me.lblPre6.Text = "Trigger -6"
        Me.lblPre6.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblPostTrig_4
        '
        Me._lblPostTrig_4.BackColor = System.Drawing.SystemColors.Window
        Me._lblPostTrig_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblPostTrig_4.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblPostTrig_4.ForeColor = System.Drawing.Color.Blue
        Me._lblPostTrig_4.Location = New System.Drawing.Point(299, 135)
        Me._lblPostTrig_4.Name = "_lblPostTrig_4"
        Me._lblPostTrig_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblPostTrig_4.Size = New System.Drawing.Size(65, 14)
        Me._lblPostTrig_4.TabIndex = 36
        Me._lblPostTrig_4.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblPost4
        '
        Me.lblPost4.BackColor = System.Drawing.SystemColors.Window
        Me.lblPost4.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPost4.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPost4.ForeColor = System.Drawing.Color.Blue
        Me.lblPost4.Location = New System.Drawing.Point(207, 135)
        Me.lblPost4.Name = "lblPost4"
        Me.lblPost4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPost4.Size = New System.Drawing.Size(73, 13)
        Me.lblPost4.TabIndex = 35
        Me.lblPost4.Text = "Trigger +3"
        Me.lblPost4.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblPreTrig_3
        '
        Me._lblPreTrig_3.BackColor = System.Drawing.SystemColors.Window
        Me._lblPreTrig_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblPreTrig_3.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblPreTrig_3.ForeColor = System.Drawing.Color.Blue
        Me._lblPreTrig_3.Location = New System.Drawing.Point(106, 135)
        Me._lblPreTrig_3.Name = "_lblPreTrig_3"
        Me._lblPreTrig_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblPreTrig_3.Size = New System.Drawing.Size(65, 13)
        Me._lblPreTrig_3.TabIndex = 12
        Me._lblPreTrig_3.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblPre7
        '
        Me.lblPre7.BackColor = System.Drawing.SystemColors.Window
        Me.lblPre7.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPre7.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPre7.ForeColor = System.Drawing.Color.Blue
        Me.lblPre7.Location = New System.Drawing.Point(19, 135)
        Me.lblPre7.Name = "lblPre7"
        Me.lblPre7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPre7.Size = New System.Drawing.Size(73, 13)
        Me.lblPre7.TabIndex = 4
        Me.lblPre7.Text = "Trigger -7"
        Me.lblPre7.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblPostTrig_2
        '
        Me._lblPostTrig_2.BackColor = System.Drawing.SystemColors.Window
        Me._lblPostTrig_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblPostTrig_2.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblPostTrig_2.ForeColor = System.Drawing.Color.Blue
        Me._lblPostTrig_2.Location = New System.Drawing.Point(299, 122)
        Me._lblPostTrig_2.Name = "_lblPostTrig_2"
        Me._lblPostTrig_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblPostTrig_2.Size = New System.Drawing.Size(65, 13)
        Me._lblPostTrig_2.TabIndex = 28
        Me._lblPostTrig_2.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblPost3
        '
        Me.lblPost3.BackColor = System.Drawing.SystemColors.Window
        Me.lblPost3.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPost3.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPost3.ForeColor = System.Drawing.Color.Blue
        Me.lblPost3.Location = New System.Drawing.Point(207, 122)
        Me.lblPost3.Name = "lblPost3"
        Me.lblPost3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPost3.Size = New System.Drawing.Size(73, 13)
        Me.lblPost3.TabIndex = 31
        Me.lblPost3.Text = "Trigger +2"
        Me.lblPost3.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblPreTrig_2
        '
        Me._lblPreTrig_2.BackColor = System.Drawing.SystemColors.Window
        Me._lblPreTrig_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblPreTrig_2.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblPreTrig_2.ForeColor = System.Drawing.Color.Blue
        Me._lblPreTrig_2.Location = New System.Drawing.Point(106, 122)
        Me._lblPreTrig_2.Name = "_lblPreTrig_2"
        Me._lblPreTrig_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblPreTrig_2.Size = New System.Drawing.Size(65, 13)
        Me._lblPreTrig_2.TabIndex = 11
        Me._lblPreTrig_2.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblPre8
        '
        Me.lblPre8.BackColor = System.Drawing.SystemColors.Window
        Me.lblPre8.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPre8.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPre8.ForeColor = System.Drawing.Color.Blue
        Me.lblPre8.Location = New System.Drawing.Point(19, 122)
        Me.lblPre8.Name = "lblPre8"
        Me.lblPre8.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPre8.Size = New System.Drawing.Size(73, 13)
        Me.lblPre8.TabIndex = 3
        Me.lblPre8.Text = "Trigger -8"
        Me.lblPre8.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblPostTrig_3
        '
        Me._lblPostTrig_3.BackColor = System.Drawing.SystemColors.Window
        Me._lblPostTrig_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblPostTrig_3.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblPostTrig_3.ForeColor = System.Drawing.Color.Blue
        Me._lblPostTrig_3.Location = New System.Drawing.Point(299, 109)
        Me._lblPostTrig_3.Name = "_lblPostTrig_3"
        Me._lblPostTrig_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblPostTrig_3.Size = New System.Drawing.Size(65, 14)
        Me._lblPostTrig_3.TabIndex = 32
        Me._lblPostTrig_3.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblPost2
        '
        Me.lblPost2.BackColor = System.Drawing.SystemColors.Window
        Me.lblPost2.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPost2.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPost2.ForeColor = System.Drawing.Color.Blue
        Me.lblPost2.Location = New System.Drawing.Point(207, 109)
        Me.lblPost2.Name = "lblPost2"
        Me.lblPost2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPost2.Size = New System.Drawing.Size(73, 13)
        Me.lblPost2.TabIndex = 27
        Me.lblPost2.Text = "Trigger +1"
        Me.lblPost2.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblPreTrig_1
        '
        Me._lblPreTrig_1.BackColor = System.Drawing.SystemColors.Window
        Me._lblPreTrig_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblPreTrig_1.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblPreTrig_1.ForeColor = System.Drawing.Color.Blue
        Me._lblPreTrig_1.Location = New System.Drawing.Point(106, 109)
        Me._lblPreTrig_1.Name = "_lblPreTrig_1"
        Me._lblPreTrig_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblPreTrig_1.Size = New System.Drawing.Size(65, 13)
        Me._lblPreTrig_1.TabIndex = 10
        Me._lblPreTrig_1.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblPre9
        '
        Me.lblPre9.BackColor = System.Drawing.SystemColors.Window
        Me.lblPre9.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPre9.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPre9.ForeColor = System.Drawing.Color.Blue
        Me.lblPre9.Location = New System.Drawing.Point(19, 109)
        Me.lblPre9.Name = "lblPre9"
        Me.lblPre9.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPre9.Size = New System.Drawing.Size(73, 13)
        Me.lblPre9.TabIndex = 2
        Me.lblPre9.Text = "Trigger -9"
        Me.lblPre9.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblPostTrig_1
        '
        Me._lblPostTrig_1.BackColor = System.Drawing.SystemColors.Window
        Me._lblPostTrig_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblPostTrig_1.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblPostTrig_1.ForeColor = System.Drawing.Color.Blue
        Me._lblPostTrig_1.Location = New System.Drawing.Point(299, 96)
        Me._lblPostTrig_1.Name = "_lblPostTrig_1"
        Me._lblPostTrig_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblPostTrig_1.Size = New System.Drawing.Size(65, 13)
        Me._lblPostTrig_1.TabIndex = 24
        Me._lblPostTrig_1.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblPost1
        '
        Me.lblPost1.BackColor = System.Drawing.SystemColors.Window
        Me.lblPost1.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPost1.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPost1.ForeColor = System.Drawing.Color.Blue
        Me.lblPost1.Location = New System.Drawing.Point(207, 96)
        Me.lblPost1.Name = "lblPost1"
        Me.lblPost1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPost1.Size = New System.Drawing.Size(73, 13)
        Me.lblPost1.TabIndex = 23
        Me.lblPost1.Text = "Trigger"
        Me.lblPost1.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblPreTrig_0
        '
        Me._lblPreTrig_0.BackColor = System.Drawing.SystemColors.Window
        Me._lblPreTrig_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblPreTrig_0.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblPreTrig_0.ForeColor = System.Drawing.Color.Blue
        Me._lblPreTrig_0.Location = New System.Drawing.Point(106, 96)
        Me._lblPreTrig_0.Name = "_lblPreTrig_0"
        Me._lblPreTrig_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblPreTrig_0.Size = New System.Drawing.Size(65, 13)
        Me._lblPreTrig_0.TabIndex = 9
        Me._lblPreTrig_0.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblPre10
        '
        Me.lblPre10.BackColor = System.Drawing.SystemColors.Window
        Me.lblPre10.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPre10.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPre10.ForeColor = System.Drawing.Color.Blue
        Me.lblPre10.Location = New System.Drawing.Point(19, 96)
        Me.lblPre10.Name = "lblPre10"
        Me.lblPre10.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPre10.Size = New System.Drawing.Size(73, 13)
        Me.lblPre10.TabIndex = 1
        Me.lblPre10.Text = "Trigger -10"
        Me.lblPre10.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblPostTrigData
        '
        Me.lblPostTrigData.BackColor = System.Drawing.SystemColors.Window
        Me.lblPostTrigData.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPostTrigData.Font = New System.Drawing.Font("Arial", 8.25!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPostTrigData.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblPostTrigData.Location = New System.Drawing.Point(200, 77)
        Me.lblPostTrigData.Name = "lblPostTrigData"
        Me.lblPostTrigData.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPostTrigData.Size = New System.Drawing.Size(202, 14)
        Me.lblPostTrigData.TabIndex = 44
        Me.lblPostTrigData.Text = "Data acquired after trigger"
        Me.lblPostTrigData.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblPreTrigData
        '
        Me.lblPreTrigData.BackColor = System.Drawing.SystemColors.Window
        Me.lblPreTrigData.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPreTrigData.Font = New System.Drawing.Font("Arial", 8.25!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPreTrigData.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblPreTrigData.Location = New System.Drawing.Point(13, 77)
        Me.lblPreTrigData.Name = "lblPreTrigData"
        Me.lblPreTrigData.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPreTrigData.Size = New System.Drawing.Size(181, 14)
        Me.lblPreTrigData.TabIndex = 43
        Me.lblPreTrigData.Text = "Data acquired before trigger"
        Me.lblPreTrigData.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblDemoFunction
        '
        Me.lblDemoFunction.BackColor = System.Drawing.SystemColors.Window
        Me.lblDemoFunction.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDemoFunction.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDemoFunction.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblDemoFunction.Location = New System.Drawing.Point(22, 6)
        Me.lblDemoFunction.Name = "lblDemoFunction"
        Me.lblDemoFunction.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDemoFunction.Size = New System.Drawing.Size(345, 16)
        Me.lblDemoFunction.TabIndex = 0
        Me.lblDemoFunction.Text = "Demonstration of Mccdaq.MccBoard.FilePretrig()"
        Me.lblDemoFunction.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblAcqStat
        '
        Me.lblAcqStat.BackColor = System.Drawing.SystemColors.Window
        Me.lblAcqStat.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAcqStat.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAcqStat.ForeColor = System.Drawing.Color.Blue
        Me.lblAcqStat.Location = New System.Drawing.Point(27, 28)
        Me.lblAcqStat.Name = "lblAcqStat"
        Me.lblAcqStat.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAcqStat.Size = New System.Drawing.Size(354, 36)
        Me.lblAcqStat.TabIndex = 64
        Me.lblAcqStat.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'frmFilePreTrig
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 13)
        Me.BackColor = System.Drawing.SystemColors.Window
        Me.ClientSize = New System.Drawing.Size(414, 396)
        Me.Controls.Add(Me.lblAcqStat)
        Me.Controls.Add(Me.txtFileName)
        Me.Controls.Add(Me.cmdQuit)
        Me.Controls.Add(Me.cmdTrigEnable)
        Me.Controls.Add(Me.lblFileInstruct)
        Me.Controls.Add(Me.lblShowGain)
        Me.Controls.Add(Me.lblGain)
        Me.Controls.Add(Me.lblShowRate)
        Me.Controls.Add(Me.lblRate)
        Me.Controls.Add(Me.lblShowNumSam)
        Me.Controls.Add(Me.lblNumSam)
        Me.Controls.Add(Me.lblShowPT)
        Me.Controls.Add(Me.lblNumPTSam)
        Me.Controls.Add(Me.lblShowHiChan)
        Me.Controls.Add(Me.lblHiChan)
        Me.Controls.Add(Me.lblShowLoChan)
        Me.Controls.Add(Me.lblLoChan)
        Me.Controls.Add(Me.lblShowFileName)
        Me.Controls.Add(Me.lblFileName)
        Me.Controls.Add(Me._lblPostTrig_10)
        Me.Controls.Add(Me.lblPost10)
        Me.Controls.Add(Me._lblPreTrig_9)
        Me.Controls.Add(Me.lblPre1)
        Me.Controls.Add(Me._lblPostTrig_9)
        Me.Controls.Add(Me.lblPost9)
        Me.Controls.Add(Me._lblPreTrig_8)
        Me.Controls.Add(Me.lblPre2)
        Me.Controls.Add(Me._lblPostTrig_8)
        Me.Controls.Add(Me.lblPost8)
        Me.Controls.Add(Me._lblPreTrig_7)
        Me.Controls.Add(Me.lblPre3)
        Me.Controls.Add(Me._lblPostTrig_7)
        Me.Controls.Add(Me.lblPost7)
        Me.Controls.Add(Me._lblPreTrig_6)
        Me.Controls.Add(Me.lblPre4)
        Me.Controls.Add(Me._lblPostTrig_6)
        Me.Controls.Add(Me.lblPost6)
        Me.Controls.Add(Me._lblPreTrig_5)
        Me.Controls.Add(Me.lblPre5)
        Me.Controls.Add(Me._lblPostTrig_5)
        Me.Controls.Add(Me.lblPost5)
        Me.Controls.Add(Me._lblPreTrig_4)
        Me.Controls.Add(Me.lblPre6)
        Me.Controls.Add(Me._lblPostTrig_4)
        Me.Controls.Add(Me.lblPost4)
        Me.Controls.Add(Me._lblPreTrig_3)
        Me.Controls.Add(Me.lblPre7)
        Me.Controls.Add(Me._lblPostTrig_2)
        Me.Controls.Add(Me.lblPost3)
        Me.Controls.Add(Me._lblPreTrig_2)
        Me.Controls.Add(Me.lblPre8)
        Me.Controls.Add(Me._lblPostTrig_3)
        Me.Controls.Add(Me.lblPost2)
        Me.Controls.Add(Me._lblPreTrig_1)
        Me.Controls.Add(Me.lblPre9)
        Me.Controls.Add(Me._lblPostTrig_1)
        Me.Controls.Add(Me.lblPost1)
        Me.Controls.Add(Me._lblPreTrig_0)
        Me.Controls.Add(Me.lblPre10)
        Me.Controls.Add(Me.lblPostTrigData)
        Me.Controls.Add(Me.lblPreTrigData)
        Me.Controls.Add(Me.lblDemoFunction)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ForeColor = System.Drawing.Color.Blue
        Me.Location = New System.Drawing.Point(7, 103)
        Me.Name = "frmFilePreTrig"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
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

        lblPreTrig = New System.Windows.Forms.Label(19) _
        {_lblPreTrig_0, _lblPreTrig_1, _lblPreTrig_2, _lblPreTrig_3, _
        _lblPreTrig_4, _lblPreTrig_5, _lblPreTrig_6, _lblPreTrig_7, _
        _lblPreTrig_8, _lblPreTrig_9, _lblPostTrig_1, _lblPostTrig_2, _
        _lblPostTrig_3, _lblPostTrig_4, _lblPostTrig_5, _lblPostTrig_6, _
        _lblPostTrig_7, _lblPostTrig_8, _lblPostTrig_9, _lblPostTrig_10}

        lblPre = New System.Windows.Forms.Label(19) _
        {lblPre10, lblPre9, lblPre8, lblPre7, lblPre6, _
        lblPre5, lblPre4, lblPre3, lblPre2, lblPre1, _
        lblPost1, lblPost2, lblPost3, lblPost4, lblPost5, _
        lblPost6, lblPost7, lblPost8, lblPost9, lblPost10}

    End Sub

    Public lblPreTrig As System.Windows.Forms.Label()
    Public lblPre As System.Windows.Forms.Label()
    Public WithEvents lblAcqStat As System.Windows.Forms.Label
    Public lblPostTrig As System.Windows.Forms.Label()

#End Region

End Class