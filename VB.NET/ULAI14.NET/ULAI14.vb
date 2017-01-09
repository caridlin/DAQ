'==============================================================================

' File:                         ULAI14.VB

' Library Call Demonstrated:    Mccdaq.MccBoard.SetTrigger()

' Purpose:                      Selects the Trigger source. This trigger is
'                               used to initiate A/D conversion using
'                               Mccdaq.MccBoard.AInScan(), with 
'                               MccDaq.ScanOptions.ExtTrigger Option.

' Demonstration:                Selects the trigger source
'                               Displays the analog input on up to eight channels.

' Other Library Calls:          MccDaq.MccService.ErrHandling()

' Special Requirements:         Board 0 must have software selectable
'                               triggering source and type.
'                               Board 0 must have an A/D converter.
'                               Analog signals on up to eight input channels.

'==============================================================================
Option Strict Off
Option Explicit On

Friend Class frmDataDisplay

    Inherits System.Windows.Forms.Form

    ' Create a new MccBoard object for Board 0
    Private DaqBoard As MccDaq.MccBoard = New MccDaq.MccBoard(0)

    Private Range As MccDaq.Range
    Private ADResolution, NumAIChans As Integer
    Private HighChan, LowChan, MaxChan As Integer
    Private DefaultTrig As MccDaq.TriggerType

    Const NumPoints As Integer = 600    ' Number of data points to collect
    Const FirstPoint As Integer = 0     ' set first element in buffer to transfer to array

    Dim ADData() As UInt16              ' dimension an array to hold the input values
    Dim ADData32() As System.UInt32     ' dimension an array to hold the high resolution input values
    Dim MemHandle As IntPtr             ' define a variable to contain the handle for
    '                                     memory allocated by Windows through MccDaq.MccService.WinBufAlloc()

    Private Sub frmDataDisplay_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        InitUL()

        ' determine the number of analog channels and their capabilities
        Dim ChannelType As Integer = ATRIGIN
        NumAIChans = FindAnalogChansOfType(DaqBoard, ChannelType, _
            ADResolution, Range, LowChan, DefaultTrig)

        If (NumAIChans = 0) Then
            lblInstruction.Text = "Board " & DaqBoard.BoardNum.ToString() & _
            " does not have analog input or it does not support analog trigger."
            cmdStart.Enabled = False
            txtHighChan.Enabled = False
        Else
            ' Check the resolution of the A/D data and allocate memory accordingly
            If ADResolution > 16 Then
                ' set aside memory to hold high resolution data
                ReDim ADData32(NumPoints)
                MemHandle = MccDaq.MccService.WinBufAlloc32Ex(NumPoints)
            Else
                ' set aside memory to hold data
                ReDim ADData(NumPoints)
                MemHandle = MccDaq.MccService.WinBufAllocEx(NumPoints)
            End If
            If MemHandle = 0 Then Stop
            If (NumAIChans > 8) Then NumAIChans = 8 'limit to 8 for display
            MaxChan = LowChan + NumAIChans - 1
            lblInstruction.Text = "Board " & DaqBoard.BoardNum.ToString() & _
                " collecting analog data on on up to " & NumAIChans.ToString() & _
                " channels using AInScan with Range set to " & Range.ToString() & "."
        End If

    End Sub

    Private Sub cmdStart_Click(ByVal eventSender As System.Object, _
    ByVal eventArgs As System.EventArgs) Handles cmdStart.Click

        Dim j As Integer
        Dim i As Integer
        Dim Options As MccDaq.ScanOptions
        Dim Rate As Integer
        Dim Count As Integer
        Dim TrigType As MccDaq.TriggerType
        Dim LowThreshold As UInt16
        Dim HighThreshold As UInt16
        Dim ULStat As MccDaq.ErrorInfo
        Dim highVal As Single 'high threshold in volts
        Dim lowVal As Single 'low threshold in volts
        Dim VoltageRange As Single
        Dim FSCounts As Integer
        Dim LSB As Single
        Dim ValidChan As Boolean
        Dim TrigSource As String

        cmdStart.Enabled = False

        ' Select the trigger source using Mccdaq.MccBoard.SetTrigger()
        ' Parameters:
        '   TrigType       :the type of triggering based on external trigger source
        '   LowThreshold   :Low threshold when the trigger input is analog
        '   HighThreshold  :High threshold when the trigger input is analog

        highVal = 1.53#
        lowVal = 0.1
        TrigType = MccDaq.TriggerType.TrigAbove

        TrigSource = "analog trigger input"
        If ATrigRes = 0 Then
            ULStat = DaqBoard.FromEngUnits(Range, highVal, HighThreshold)
            ULStat = DaqBoard.FromEngUnits(Range, lowVal, LowThreshold)
        Else
            'Use the value acquired from the AnalogIO module, since the resolution
            'of the input is different from the resolution of the trigger.
            'Calculate trigger based on resolution returned and trigger range.
            VoltageRange = ATrigRange
            If ATrigRange = -1 Then
                VoltageRange = GetRangeVolts(Range)
                TrigSource$ = "first channel in scan"
            End If
            FSCounts = Math.Pow(2, ATrigRes)
            LSB = VoltageRange / FSCounts
            LowThreshold = (lowVal / LSB) + (FSCounts / 2)
            HighThreshold = (highVal / LSB) + (FSCounts / 2)
        End If

        lblInstruction.Text = "Board " & DaqBoard.BoardNum.ToString() & _
            " collecting analog data on on up to " & NumAIChans.ToString() & _
            " channels using AInScan with Range set to " & Range.ToString() & "."

        lblResult.Text = "Waiting for a trigger at " & TrigSource & ". " & _
        "Trigger criterea: signal rising above " & Format(highVal, "0.00") & "V." & _
        " (Ctl-Break to abort.)"
        Application.DoEvents()

        ULStat = DaqBoard.SetTrigger(TrigType, LowThreshold, HighThreshold)
        If (ULStat.Value = MccDaq.ErrorInfo.ErrorCode.NoErrors) Then
            ' Collect the values with MccDaq.MccBoard.AInScan()
            ' Parameters:
            '   LowChan    :the first channel of the scan
            '   HighChan   :the last channel of the scan
            '   Count      :the total number of A/D samples to collect
            '   Rate       :sample rate
            '   Range      :the range for the board
            '   MemHandle  :Handle for Windows buffer to store data in
            '   Options    :data collection options

            ValidChan = Integer.TryParse(txtHighChan.Text, HighChan)
            If ValidChan Then
                If (HighChan > MaxChan) Then HighChan = MaxChan
                txtHighChan.Text = HighChan.ToString()
            End If

            Count = NumPoints ' total number of data points to collect

            ' per channel sampling rate ((samples per second) per channel)
            Rate = 1000 / ((HighChan - LowChan) + 1)
            Options = MccDaq.ScanOptions.ConvertData Or MccDaq.ScanOptions.ExtTrigger ' return data as 12-bit values

            ULStat = DaqBoard.AInScan(LowChan, HighChan, Count, Rate, Range, MemHandle, Options)
            lblResult.Text = ""

            If ULStat.Value = MccDaq.ErrorInfo.ErrorCode.Interrupted Then
                Me.lblInstruction.Text = "Scan interrupted while waiting " & _
                "for trigger on board " & DaqBoard.BoardNum.ToString() & _
                ". Click Start to try again."
                Me.cmdStart.Enabled = True
                Exit Sub
            ElseIf ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors _
                And ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.FreeRunning Then
                Stop
            End If

            ' Transfer the data from the memory buffer set up by Windows to an array for use by Visual Basic
            If ADResolution > 16 Then
                ULStat = MccDaq.MccService.WinBufToArray32(MemHandle, ADData32, FirstPoint, Count)
                If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then Stop

                For i = 0 To HighChan
                    lblADData(i).Text = ADData32(i).ToString("0")
                Next i
            Else
                ULStat = MccDaq.MccService.WinBufToArray(MemHandle, ADData, FirstPoint, Count)
                If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then Stop

                For i = 0 To HighChan
                    lblADData(i).Text = ADData(i).ToString("0")
                Next i
            End If

            For j = HighChan + 1 To 7
                lblADData(j).Text = ""
            Next j
        End If

            cmdStart.Enabled = True

    End Sub

    Private Sub cmdStopConvert_Click(ByVal eventSender As System.Object, _
    ByVal eventArgs As System.EventArgs) Handles cmdStopConvert.Click

        Dim ULStat As MccDaq.ErrorInfo

        ULStat = MccDaq.MccService.WinBufFreeEx(MemHandle) ' Free up memory for use by
        If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then Stop ' other programs

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
    Public WithEvents txtHighChan As System.Windows.Forms.TextBox
    Public WithEvents cmdStopConvert As System.Windows.Forms.Button
    Public WithEvents cmdStart As System.Windows.Forms.Button
    Public WithEvents Label1 As System.Windows.Forms.Label
    Public WithEvents _lblADData_7 As System.Windows.Forms.Label
    Public WithEvents lblChan7 As System.Windows.Forms.Label
    Public WithEvents _lblADData_3 As System.Windows.Forms.Label
    Public WithEvents lblChan3 As System.Windows.Forms.Label
    Public WithEvents _lblADData_6 As System.Windows.Forms.Label
    Public WithEvents lblChan6 As System.Windows.Forms.Label
    Public WithEvents _lblADData_2 As System.Windows.Forms.Label
    Public WithEvents lblChan2 As System.Windows.Forms.Label
    Public WithEvents _lblADData_5 As System.Windows.Forms.Label
    Public WithEvents lblChan5 As System.Windows.Forms.Label
    Public WithEvents _lblADData_1 As System.Windows.Forms.Label
    Public WithEvents lblChan1 As System.Windows.Forms.Label
    Public WithEvents _lblADData_4 As System.Windows.Forms.Label
    Public WithEvents lblChan4 As System.Windows.Forms.Label
    Public WithEvents _lblADData_0 As System.Windows.Forms.Label
    Public WithEvents lblChan0 As System.Windows.Forms.Label
    Public WithEvents lblDemoFunction As System.Windows.Forms.Label
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.txtHighChan = New System.Windows.Forms.TextBox
        Me.cmdStopConvert = New System.Windows.Forms.Button
        Me.cmdStart = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me._lblADData_7 = New System.Windows.Forms.Label
        Me.lblChan7 = New System.Windows.Forms.Label
        Me._lblADData_3 = New System.Windows.Forms.Label
        Me.lblChan3 = New System.Windows.Forms.Label
        Me._lblADData_6 = New System.Windows.Forms.Label
        Me.lblChan6 = New System.Windows.Forms.Label
        Me._lblADData_2 = New System.Windows.Forms.Label
        Me.lblChan2 = New System.Windows.Forms.Label
        Me._lblADData_5 = New System.Windows.Forms.Label
        Me.lblChan5 = New System.Windows.Forms.Label
        Me._lblADData_1 = New System.Windows.Forms.Label
        Me.lblChan1 = New System.Windows.Forms.Label
        Me._lblADData_4 = New System.Windows.Forms.Label
        Me.lblChan4 = New System.Windows.Forms.Label
        Me._lblADData_0 = New System.Windows.Forms.Label
        Me.lblChan0 = New System.Windows.Forms.Label
        Me.lblDemoFunction = New System.Windows.Forms.Label
        Me.lblInstruction = New System.Windows.Forms.Label
        Me.lblResult = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'txtHighChan
        '
        Me.txtHighChan.AcceptsReturn = True
        Me.txtHighChan.BackColor = System.Drawing.SystemColors.Window
        Me.txtHighChan.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtHighChan.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtHighChan.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtHighChan.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtHighChan.Location = New System.Drawing.Point(216, 112)
        Me.txtHighChan.MaxLength = 0
        Me.txtHighChan.Name = "txtHighChan"
        Me.txtHighChan.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtHighChan.Size = New System.Drawing.Size(25, 20)
        Me.txtHighChan.TabIndex = 20
        Me.txtHighChan.Text = "0"
        Me.txtHighChan.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'cmdStopConvert
        '
        Me.cmdStopConvert.BackColor = System.Drawing.SystemColors.Control
        Me.cmdStopConvert.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdStopConvert.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdStopConvert.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdStopConvert.Location = New System.Drawing.Point(293, 276)
        Me.cmdStopConvert.Name = "cmdStopConvert"
        Me.cmdStopConvert.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdStopConvert.Size = New System.Drawing.Size(58, 26)
        Me.cmdStopConvert.TabIndex = 17
        Me.cmdStopConvert.Text = "Quit"
        Me.cmdStopConvert.UseVisualStyleBackColor = False
        '
        'cmdStart
        '
        Me.cmdStart.BackColor = System.Drawing.SystemColors.Control
        Me.cmdStart.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdStart.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdStart.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdStart.Location = New System.Drawing.Point(293, 240)
        Me.cmdStart.Name = "cmdStart"
        Me.cmdStart.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdStart.Size = New System.Drawing.Size(58, 26)
        Me.cmdStart.TabIndex = 18
        Me.cmdStart.Text = "Start"
        Me.cmdStart.UseVisualStyleBackColor = False
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.SystemColors.Window
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Label1.Location = New System.Drawing.Point(72, 112)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(137, 16)
        Me.Label1.TabIndex = 19
        Me.Label1.Text = "Measure Channels 0 to "
        '
        '_lblADData_7
        '
        Me._lblADData_7.BackColor = System.Drawing.SystemColors.Window
        Me._lblADData_7.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblADData_7.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblADData_7.ForeColor = System.Drawing.Color.Blue
        Me._lblADData_7.Location = New System.Drawing.Point(264, 220)
        Me._lblADData_7.Name = "_lblADData_7"
        Me._lblADData_7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblADData_7.Size = New System.Drawing.Size(65, 17)
        Me._lblADData_7.TabIndex = 16
        '
        'lblChan7
        '
        Me.lblChan7.BackColor = System.Drawing.SystemColors.Window
        Me.lblChan7.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblChan7.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblChan7.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblChan7.Location = New System.Drawing.Point(192, 220)
        Me.lblChan7.Name = "lblChan7"
        Me.lblChan7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblChan7.Size = New System.Drawing.Size(65, 17)
        Me.lblChan7.TabIndex = 8
        Me.lblChan7.Text = "Channel 7:"
        Me.lblChan7.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblADData_3
        '
        Me._lblADData_3.BackColor = System.Drawing.SystemColors.Window
        Me._lblADData_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblADData_3.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblADData_3.ForeColor = System.Drawing.Color.Blue
        Me._lblADData_3.Location = New System.Drawing.Point(96, 220)
        Me._lblADData_3.Name = "_lblADData_3"
        Me._lblADData_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblADData_3.Size = New System.Drawing.Size(65, 17)
        Me._lblADData_3.TabIndex = 12
        '
        'lblChan3
        '
        Me.lblChan3.BackColor = System.Drawing.SystemColors.Window
        Me.lblChan3.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblChan3.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblChan3.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblChan3.Location = New System.Drawing.Point(24, 220)
        Me.lblChan3.Name = "lblChan3"
        Me.lblChan3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblChan3.Size = New System.Drawing.Size(65, 17)
        Me.lblChan3.TabIndex = 4
        Me.lblChan3.Text = "Channel 3:"
        Me.lblChan3.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblADData_6
        '
        Me._lblADData_6.BackColor = System.Drawing.SystemColors.Window
        Me._lblADData_6.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblADData_6.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblADData_6.ForeColor = System.Drawing.Color.Blue
        Me._lblADData_6.Location = New System.Drawing.Point(264, 195)
        Me._lblADData_6.Name = "_lblADData_6"
        Me._lblADData_6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblADData_6.Size = New System.Drawing.Size(65, 17)
        Me._lblADData_6.TabIndex = 15
        '
        'lblChan6
        '
        Me.lblChan6.BackColor = System.Drawing.SystemColors.Window
        Me.lblChan6.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblChan6.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblChan6.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblChan6.Location = New System.Drawing.Point(192, 195)
        Me.lblChan6.Name = "lblChan6"
        Me.lblChan6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblChan6.Size = New System.Drawing.Size(65, 17)
        Me.lblChan6.TabIndex = 7
        Me.lblChan6.Text = "Channel 6:"
        Me.lblChan6.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblADData_2
        '
        Me._lblADData_2.BackColor = System.Drawing.SystemColors.Window
        Me._lblADData_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblADData_2.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblADData_2.ForeColor = System.Drawing.Color.Blue
        Me._lblADData_2.Location = New System.Drawing.Point(96, 195)
        Me._lblADData_2.Name = "_lblADData_2"
        Me._lblADData_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblADData_2.Size = New System.Drawing.Size(65, 17)
        Me._lblADData_2.TabIndex = 11
        '
        'lblChan2
        '
        Me.lblChan2.BackColor = System.Drawing.SystemColors.Window
        Me.lblChan2.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblChan2.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblChan2.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblChan2.Location = New System.Drawing.Point(24, 195)
        Me.lblChan2.Name = "lblChan2"
        Me.lblChan2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblChan2.Size = New System.Drawing.Size(65, 17)
        Me.lblChan2.TabIndex = 3
        Me.lblChan2.Text = "Channel 2:"
        Me.lblChan2.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblADData_5
        '
        Me._lblADData_5.BackColor = System.Drawing.SystemColors.Window
        Me._lblADData_5.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblADData_5.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblADData_5.ForeColor = System.Drawing.Color.Blue
        Me._lblADData_5.Location = New System.Drawing.Point(264, 169)
        Me._lblADData_5.Name = "_lblADData_5"
        Me._lblADData_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblADData_5.Size = New System.Drawing.Size(65, 17)
        Me._lblADData_5.TabIndex = 14
        '
        'lblChan5
        '
        Me.lblChan5.BackColor = System.Drawing.SystemColors.Window
        Me.lblChan5.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblChan5.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblChan5.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblChan5.Location = New System.Drawing.Point(192, 169)
        Me.lblChan5.Name = "lblChan5"
        Me.lblChan5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblChan5.Size = New System.Drawing.Size(65, 17)
        Me.lblChan5.TabIndex = 6
        Me.lblChan5.Text = "Channel 5:"
        Me.lblChan5.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblADData_1
        '
        Me._lblADData_1.BackColor = System.Drawing.SystemColors.Window
        Me._lblADData_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblADData_1.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblADData_1.ForeColor = System.Drawing.Color.Blue
        Me._lblADData_1.Location = New System.Drawing.Point(96, 169)
        Me._lblADData_1.Name = "_lblADData_1"
        Me._lblADData_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblADData_1.Size = New System.Drawing.Size(65, 17)
        Me._lblADData_1.TabIndex = 10
        '
        'lblChan1
        '
        Me.lblChan1.BackColor = System.Drawing.SystemColors.Window
        Me.lblChan1.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblChan1.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblChan1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblChan1.Location = New System.Drawing.Point(24, 169)
        Me.lblChan1.Name = "lblChan1"
        Me.lblChan1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblChan1.Size = New System.Drawing.Size(65, 17)
        Me.lblChan1.TabIndex = 2
        Me.lblChan1.Text = "Channel 1:"
        Me.lblChan1.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblADData_4
        '
        Me._lblADData_4.BackColor = System.Drawing.SystemColors.Window
        Me._lblADData_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblADData_4.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblADData_4.ForeColor = System.Drawing.Color.Blue
        Me._lblADData_4.Location = New System.Drawing.Point(264, 144)
        Me._lblADData_4.Name = "_lblADData_4"
        Me._lblADData_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblADData_4.Size = New System.Drawing.Size(65, 17)
        Me._lblADData_4.TabIndex = 13
        '
        'lblChan4
        '
        Me.lblChan4.BackColor = System.Drawing.SystemColors.Window
        Me.lblChan4.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblChan4.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblChan4.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblChan4.Location = New System.Drawing.Point(192, 144)
        Me.lblChan4.Name = "lblChan4"
        Me.lblChan4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblChan4.Size = New System.Drawing.Size(65, 17)
        Me.lblChan4.TabIndex = 5
        Me.lblChan4.Text = "Channel 4:"
        Me.lblChan4.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblADData_0
        '
        Me._lblADData_0.BackColor = System.Drawing.SystemColors.Window
        Me._lblADData_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblADData_0.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblADData_0.ForeColor = System.Drawing.Color.Blue
        Me._lblADData_0.Location = New System.Drawing.Point(96, 144)
        Me._lblADData_0.Name = "_lblADData_0"
        Me._lblADData_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblADData_0.Size = New System.Drawing.Size(65, 17)
        Me._lblADData_0.TabIndex = 9
        '
        'lblChan0
        '
        Me.lblChan0.BackColor = System.Drawing.SystemColors.Window
        Me.lblChan0.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblChan0.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblChan0.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblChan0.Location = New System.Drawing.Point(24, 144)
        Me.lblChan0.Name = "lblChan0"
        Me.lblChan0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblChan0.Size = New System.Drawing.Size(65, 17)
        Me.lblChan0.TabIndex = 1
        Me.lblChan0.Text = "Channel 0:"
        Me.lblChan0.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblDemoFunction
        '
        Me.lblDemoFunction.BackColor = System.Drawing.SystemColors.Window
        Me.lblDemoFunction.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDemoFunction.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDemoFunction.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblDemoFunction.Location = New System.Drawing.Point(8, 8)
        Me.lblDemoFunction.Name = "lblDemoFunction"
        Me.lblDemoFunction.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDemoFunction.Size = New System.Drawing.Size(348, 19)
        Me.lblDemoFunction.TabIndex = 0
        Me.lblDemoFunction.Text = "Demonstration of MccDaq.MccBoard.SetTrigger() "
        Me.lblDemoFunction.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblInstruction
        '
        Me.lblInstruction.BackColor = System.Drawing.SystemColors.Window
        Me.lblInstruction.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblInstruction.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInstruction.ForeColor = System.Drawing.Color.Red
        Me.lblInstruction.Location = New System.Drawing.Point(44, 34)
        Me.lblInstruction.Name = "lblInstruction"
        Me.lblInstruction.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblInstruction.Size = New System.Drawing.Size(273, 75)
        Me.lblInstruction.TabIndex = 30
        Me.lblInstruction.Text = "Board 0 must have analog inputs that support paced acquisition."
        Me.lblInstruction.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblResult
        '
        Me.lblResult.BackColor = System.Drawing.SystemColors.Window
        Me.lblResult.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblResult.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblResult.ForeColor = System.Drawing.Color.Blue
        Me.lblResult.Location = New System.Drawing.Point(8, 255)
        Me.lblResult.Name = "lblResult"
        Me.lblResult.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblResult.Size = New System.Drawing.Size(271, 46)
        Me.lblResult.TabIndex = 56
        '
        'frmDataDisplay
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 13)
        Me.BackColor = System.Drawing.SystemColors.Window
        Me.ClientSize = New System.Drawing.Size(361, 309)
        Me.Controls.Add(Me.lblResult)
        Me.Controls.Add(Me.lblInstruction)
        Me.Controls.Add(Me.txtHighChan)
        Me.Controls.Add(Me.cmdStopConvert)
        Me.Controls.Add(Me.cmdStart)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me._lblADData_7)
        Me.Controls.Add(Me.lblChan7)
        Me.Controls.Add(Me._lblADData_3)
        Me.Controls.Add(Me.lblChan3)
        Me.Controls.Add(Me._lblADData_6)
        Me.Controls.Add(Me.lblChan6)
        Me.Controls.Add(Me._lblADData_2)
        Me.Controls.Add(Me.lblChan2)
        Me.Controls.Add(Me._lblADData_5)
        Me.Controls.Add(Me.lblChan5)
        Me.Controls.Add(Me._lblADData_1)
        Me.Controls.Add(Me.lblChan1)
        Me.Controls.Add(Me._lblADData_4)
        Me.Controls.Add(Me.lblChan4)
        Me.Controls.Add(Me._lblADData_0)
        Me.Controls.Add(Me.lblChan0)
        Me.Controls.Add(Me.lblDemoFunction)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ForeColor = System.Drawing.Color.Blue
        Me.Location = New System.Drawing.Point(189, 104)
        Me.Name = "frmDataDisplay"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Universal Library Analog Input Scan"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Public WithEvents lblInstruction As System.Windows.Forms.Label
    Public WithEvents lblResult As System.Windows.Forms.Label
    Public lblADData As System.Windows.Forms.Label()

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
        '    MccDaq.ErrorReporting.PrintAll :all warnings and errors encountered will be printed
        '    MccDaq.ErrorHandling.DontStop  :if any error is encountered, the program will not stop

        ReportError = MccDaq.ErrorReporting.PrintAll
        HandleError = MccDaq.ErrorHandling.DontStop
        ULStat = MccDaq.MccService.ErrHandling(ReportError, HandleError)
        If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then
            Stop
        End If

        lblADData = New System.Windows.Forms.Label(7) _
        {_lblADData_0, _lblADData_1, _lblADData_2, _lblADData_3, _
        _lblADData_4, _lblADData_5, _lblADData_6, _lblADData_7}

    End Sub

#End Region

End Class