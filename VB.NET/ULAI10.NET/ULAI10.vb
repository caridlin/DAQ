'==============================================================================

' File:                         ULAI10.VB

' Library Call Demonstrated:    Mccdaq.MccBoard.ALoadQueue()
'
' Purpose:                      Loads an A/D board's channel/gain queue.
'
' Demonstration:                Prepares a channel/gain queue and loads it
'                               to the board. An analog input function
'                               is then called to show how the queue
'                               values work.
'
' Other Library Calls:          MccDaq.MccService.ErrHandling()
'
' Special Requirements:         Board 0 must have an A/D converter and 
'                               channel gain queue hardware.
'==============================================================================
Option Strict Off
Option Explicit On

Public Class frmDataDisplay

    Inherits System.Windows.Forms.Form

    ' Create a new MccBoard object for Board 0
    Private DaqBoard As MccDaq.MccBoard = New MccDaq.MccBoard(0)

    Private Range As MccDaq.Range
    Private ADResolution, NumAIChans As Integer
    Private HighChan, LowChan, MaxChan As Integer

    Const NumPoints As Integer = 120 ' Number of data points to collect
    Const NumElements As Integer = 4 ' Number of elements in queue

    Dim ADData() As UInt16           ' dimension an array to hold the input values
    Dim ADData32() As System.UInt32  ' dimension an array to hold the high resolution input values
    Dim MemHandle As IntPtr          ' define a variable to contain the handle for memory
    '                                  allocated by Windows through MccDaq.MccService.WinBufAlloc()

    Dim ChanArray() As Short        ' array to hold channel queue information
    Dim GainArray() As MccDaq.Range ' array to hold gain queue information

    Private Sub frmDataDisplay_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim i As Short
        Dim DefaultTrig As Long

        InitUL()

        ' determine the number of analog channels and their capabilities
        Dim ChannelType As Integer = ANALOGINPUT
        NumAIChans = FindAnalogChansOfType(DaqBoard, ChannelType, _
            ADResolution, Range, LowChan, DefaultTrig)

        If (NumAIChans = 0) Then
            lblInstruction.Text = "Board " & DaqBoard.BoardNum.ToString() & _
            " does not have analog input channels."
            cmdLoadQueue.Enabled = False
        Else
            ' Check the resolution of the A/D data and allocate memory accordingly
            If ADResolution > 16 Then
                ' set aside memory to hold high resolution data
                ReDim ADData32(NumPoints)
                MemHandle = MccDaq.MccService.WinBufAlloc32Ex(NumPoints)
            Else
                ' set aside memory to hold 16-bit data
                ReDim ADData(NumPoints)
                MemHandle = MccDaq.MccService.WinBufAllocEx(NumPoints)
            End If
            If MemHandle = 0 Then Stop
            MaxChan = LowChan + NumAIChans - 1      'allow use of any channel for queue
            If (NumAIChans > 4) Then NumAIChans = 4 'limit to 4 channels for display
            ReDim ChanArray(NumElements - 1)
            ReDim GainArray(NumElements - 1)
            lblInstruction.Text = "Board " & DaqBoard.BoardNum.ToString() & _
                " collecting analog data on up to " & NumAIChans.ToString() & _
                " channels between channel 0 and channel " & MaxChan.ToString() & _
                " using AInScan and ALoadQueue."
            Me.tmrConvert.Enabled = True
        End If

        For i = 0 To 3
            lblShowRange(i).Text = Range.ToString()
        Next i

    End Sub

    Private Sub cmdLoadQueue_Click(ByVal eventSender As System.Object, _
    ByVal eventArgs As System.EventArgs) Handles cmdLoadQueue.Click

        Dim ULStat As MccDaq.ErrorInfo
        Dim ValidRanges() As MccDaq.Range
        Dim NumRanges As Integer
        Dim RandomSelect As New System.Random
        Dim x As Double

        'Get a list of valid ranges from the AnalogIO module
        ValidRanges = GetRangeList()
        NumRanges = ValidRanges.GetUpperBound(0)

        cmdLoadQueue.Enabled = False
        cmdLoadQueue.Visible = False
        cmdUnloadQ.Enabled = True
        cmdUnloadQ.Visible = True

        ' Set up the channel/gain queue for 4 channels - each 
        ' channel set to random valid A/D ranges. 
        ' Note: Some devices have limitations on the queue,
        ' such as not mixing Bipolar/Unipolar ranges or allowing 
        ' only unique contiguous channels - see hardware manual

        For i As Short = 0 To NumElements - 1
            If chkRanges.Checked Then
                x = RandomSelect.NextDouble
                GainArray(i) = ValidRanges(x * NumRanges)
            Else
                GainArray(i) = Range
            End If
            If chkChannels.Checked Then
                x = RandomSelect.NextDouble
                ChanArray(i) = x * MaxChan
            Else
                ChanArray(i) = i
            End If
        Next

        ' Load the channel/gain values into the queue
        '  Parameters:
        '    ChanArray[] :array of channel values
        '    GainArray[] :array of gain values
        '    NumElements :the number of elements in the arrays (0=disable queue)

        ULStat = DaqBoard.ALoadQueue(ChanArray, GainArray, NumElements)
        If ULStat.Value = MccDaq.ErrorInfo.ErrorCode.BadAdChan Then
            lblInstruction.Text = "Board " & DaqBoard.BoardNum.ToString() & _
                " doesn't support random channels. Queue was not changed."
        ElseIf ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then
            Me.tmrConvert.Enabled = False
            lblInstruction.Text = "Board " & DaqBoard.BoardNum.ToString() & _
                " error loading queue: " & ULStat.Message
        Else
            lblInstruction.Text = "Queue loaded on board " & _
                DaqBoard.BoardNum.ToString() & "."
            For i As Short = 0 To NumElements - 1
                lblShowRange(i).Text = GainArray(i).ToString()
                lblChan(i).Text = "Channel " & ChanArray(i).ToString()
            Next
        End If

    End Sub

    Private Sub cmdUnloadQ_Click(ByVal eventSender As System.Object, _
    ByVal eventArgs As System.EventArgs) Handles cmdUnloadQ.Click

        Dim ULStat As MccDaq.ErrorInfo
        Dim NoChans As Short
        Dim i As Short

        cmdUnloadQ.Enabled = False
        cmdUnloadQ.Visible = False
        cmdLoadQueue.Enabled = True
        cmdLoadQueue.Visible = True
        For i = 0 To 3
            lblShowRange(i).Text = Range.ToString()
            lblChan(i).Text = "Channel " & i.ToString()
        Next i

        NoChans = 0 ' set to zero to disable queue

        ULStat = DaqBoard.ALoadQueue(ChanArray, GainArray, NoChans)
        If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then
            lblInstruction.Text = "Board " & DaqBoard.BoardNum.ToString() & _
                " error unloading queue: " & ULStat.Message
        Else
            lblInstruction.Text = "Board " & DaqBoard.BoardNum.ToString() & _
                " scanning contiguous channels with with Range set to " & _
                Range.ToString() + "."
        End If

    End Sub

    Private Sub tmrConvert_Tick(ByVal eventSender As System.Object, _
    ByVal eventArgs As System.EventArgs) Handles tmrConvert.Tick

        Dim ChannelNum As Short
        Dim SampleNum As Short
        Dim i As Integer
        Dim FirstPoint As Integer
        Dim ULStat As MccDaq.ErrorInfo
        Dim Rate As Integer
        Dim Options As MccDaq.ScanOptions
        Dim Count As Integer
        Dim LastChan As Short
        Dim FirstChan As Short

        ' Call an analog input function to show how the gain queue values
        ' supercede those passed to the function.

        '' Collect the values by calling MccDaq.MccBoard.AInScan function
        '  Parameters:
        '    FirstChan  :the first channel of the scan
        '    LastChan   :the last channel of the scan
        '    Count      :the total number of A/D samples to collect
        '    Rate       :sample rate in samples per second
        '    Range      :the gain for the board
        '    MemHandle  :Handle for Windows buffer to store data in 
        '    Options    :data collection options

        tmrConvert.Stop()

        FirstChan = 0           ' This is ignored when queue is enabled
        LastChan = 3            ' This is ignored when queue is enabled
        Count = NumPoints       ' Number of data points to collect
        Options = MccDaq.ScanOptions.ConvertData   ' Return data as 12-bit values

        ' per channel sampling rate ((samples per second) per channel)
        Rate = 1000 / ((LastChan - FirstChan) + 1)

        ULStat = DaqBoard.AInScan(FirstChan, LastChan, Count, Rate, Range, MemHandle, Options)
        If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then Stop

        ' Transfer the data from the memory buffer set up by
        ' Windows to an array for use by this program
        i = 0
        If ADResolution > 16 Then
            ULStat = MccDaq.MccService.WinBufToArray32(MemHandle, ADData32, FirstPoint, Count)
        Else
            ULStat = MccDaq.MccService.WinBufToArray(MemHandle, ADData, FirstPoint, Count)
        End If
        If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then Stop

        For SampleNum = 0 To 9
            For ChannelNum = 0 To NumAIChans - 1
                If ADResolution > 16 Then
                    lblADData(i).Text = ADData32(i).ToString("D")
                Else
                    lblADData(i).Text = ADData(i).ToString("D")
                End If
                i = i + 1
            Next ChannelNum
        Next SampleNum

        tmrConvert.Start()

    End Sub

    Private Sub cmdStopConvert_Click(ByVal eventSender As System.Object, _
    ByVal eventArgs As System.EventArgs) Handles cmdStopConvert.Click

        Dim ULStat As MccDaq.ErrorInfo

        ULStat = MccDaq.MccService.WinBufFreeEx(MemHandle) ' Free up memory for use by other programs
        If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then Stop
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
    Public WithEvents cmdUnloadQ As System.Windows.Forms.Button
    Public WithEvents cmdLoadQueue As System.Windows.Forms.Button
    Public WithEvents tmrConvert As System.Windows.Forms.Timer
    Public WithEvents _lblADData_39 As System.Windows.Forms.Label
    Public WithEvents _lblADData_38 As System.Windows.Forms.Label
    Public WithEvents _lblADData_37 As System.Windows.Forms.Label
    Public WithEvents _lblADData_36 As System.Windows.Forms.Label
    Public WithEvents _lblADData_35 As System.Windows.Forms.Label
    Public WithEvents _lblADData_34 As System.Windows.Forms.Label
    Public WithEvents _lblADData_33 As System.Windows.Forms.Label
    Public WithEvents _lblADData_32 As System.Windows.Forms.Label
    Public WithEvents _lblADData_31 As System.Windows.Forms.Label
    Public WithEvents _lblADData_30 As System.Windows.Forms.Label
    Public WithEvents _lblADData_29 As System.Windows.Forms.Label
    Public WithEvents _lblADData_28 As System.Windows.Forms.Label
    Public WithEvents _lblADData_27 As System.Windows.Forms.Label
    Public WithEvents _lblADData_26 As System.Windows.Forms.Label
    Public WithEvents _lblADData_25 As System.Windows.Forms.Label
    Public WithEvents _lblADData_24 As System.Windows.Forms.Label
    Public WithEvents _lblADData_23 As System.Windows.Forms.Label
    Public WithEvents _lblADData_22 As System.Windows.Forms.Label
    Public WithEvents _lblADData_21 As System.Windows.Forms.Label
    Public WithEvents _lblADData_20 As System.Windows.Forms.Label
    Public WithEvents _lblADData_11 As System.Windows.Forms.Label
    Public WithEvents _lblADData_10 As System.Windows.Forms.Label
    Public WithEvents _lblADData_9 As System.Windows.Forms.Label
    Public WithEvents _lblADData_8 As System.Windows.Forms.Label
    Public WithEvents _lblADData_19 As System.Windows.Forms.Label
    Public WithEvents _lblADData_18 As System.Windows.Forms.Label
    Public WithEvents _lblADData_17 As System.Windows.Forms.Label
    Public WithEvents _lblADData_16 As System.Windows.Forms.Label
    Public WithEvents _lblADData_15 As System.Windows.Forms.Label
    Public WithEvents _lblADData_14 As System.Windows.Forms.Label
    Public WithEvents _lblADData_13 As System.Windows.Forms.Label
    Public WithEvents _lblADData_12 As System.Windows.Forms.Label
    Public WithEvents _lblADData_7 As System.Windows.Forms.Label
    Public WithEvents _lblADData_6 As System.Windows.Forms.Label
    Public WithEvents _lblADData_5 As System.Windows.Forms.Label
    Public WithEvents _lblADData_4 As System.Windows.Forms.Label
    Public WithEvents _lblADData_3 As System.Windows.Forms.Label
    Public WithEvents _lblADData_2 As System.Windows.Forms.Label
    Public WithEvents _lblADData_1 As System.Windows.Forms.Label
    Public WithEvents _lblADData_0 As System.Windows.Forms.Label
    Public WithEvents _lblShowRange_3 As System.Windows.Forms.Label
    Public WithEvents _lblShowRange_2 As System.Windows.Forms.Label
    Public WithEvents _lblShowRange_1 As System.Windows.Forms.Label
    Public WithEvents _lblShowRange_0 As System.Windows.Forms.Label
    Public WithEvents lblRange As System.Windows.Forms.Label
    Public WithEvents lblChan3 As System.Windows.Forms.Label
    Public WithEvents lblChan2 As System.Windows.Forms.Label
    Public WithEvents lblChan1 As System.Windows.Forms.Label
    Public WithEvents lblChan0 As System.Windows.Forms.Label
    Public WithEvents lblDemoFunction As System.Windows.Forms.Label
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdStopConvert = New System.Windows.Forms.Button
        Me.cmdUnloadQ = New System.Windows.Forms.Button
        Me.cmdLoadQueue = New System.Windows.Forms.Button
        Me.tmrConvert = New System.Windows.Forms.Timer(Me.components)
        Me._lblADData_39 = New System.Windows.Forms.Label
        Me._lblADData_38 = New System.Windows.Forms.Label
        Me._lblADData_37 = New System.Windows.Forms.Label
        Me._lblADData_36 = New System.Windows.Forms.Label
        Me._lblADData_35 = New System.Windows.Forms.Label
        Me._lblADData_34 = New System.Windows.Forms.Label
        Me._lblADData_33 = New System.Windows.Forms.Label
        Me._lblADData_32 = New System.Windows.Forms.Label
        Me._lblADData_31 = New System.Windows.Forms.Label
        Me._lblADData_30 = New System.Windows.Forms.Label
        Me._lblADData_29 = New System.Windows.Forms.Label
        Me._lblADData_28 = New System.Windows.Forms.Label
        Me._lblADData_27 = New System.Windows.Forms.Label
        Me._lblADData_26 = New System.Windows.Forms.Label
        Me._lblADData_25 = New System.Windows.Forms.Label
        Me._lblADData_24 = New System.Windows.Forms.Label
        Me._lblADData_23 = New System.Windows.Forms.Label
        Me._lblADData_22 = New System.Windows.Forms.Label
        Me._lblADData_21 = New System.Windows.Forms.Label
        Me._lblADData_20 = New System.Windows.Forms.Label
        Me._lblADData_11 = New System.Windows.Forms.Label
        Me._lblADData_10 = New System.Windows.Forms.Label
        Me._lblADData_9 = New System.Windows.Forms.Label
        Me._lblADData_8 = New System.Windows.Forms.Label
        Me._lblADData_19 = New System.Windows.Forms.Label
        Me._lblADData_18 = New System.Windows.Forms.Label
        Me._lblADData_17 = New System.Windows.Forms.Label
        Me._lblADData_16 = New System.Windows.Forms.Label
        Me._lblADData_15 = New System.Windows.Forms.Label
        Me._lblADData_14 = New System.Windows.Forms.Label
        Me._lblADData_13 = New System.Windows.Forms.Label
        Me._lblADData_12 = New System.Windows.Forms.Label
        Me._lblADData_7 = New System.Windows.Forms.Label
        Me._lblADData_6 = New System.Windows.Forms.Label
        Me._lblADData_5 = New System.Windows.Forms.Label
        Me._lblADData_4 = New System.Windows.Forms.Label
        Me._lblADData_3 = New System.Windows.Forms.Label
        Me._lblADData_2 = New System.Windows.Forms.Label
        Me._lblADData_1 = New System.Windows.Forms.Label
        Me._lblADData_0 = New System.Windows.Forms.Label
        Me._lblShowRange_3 = New System.Windows.Forms.Label
        Me._lblShowRange_2 = New System.Windows.Forms.Label
        Me._lblShowRange_1 = New System.Windows.Forms.Label
        Me._lblShowRange_0 = New System.Windows.Forms.Label
        Me.lblRange = New System.Windows.Forms.Label
        Me.lblChan3 = New System.Windows.Forms.Label
        Me.lblChan2 = New System.Windows.Forms.Label
        Me.lblChan1 = New System.Windows.Forms.Label
        Me.lblChan0 = New System.Windows.Forms.Label
        Me.lblDemoFunction = New System.Windows.Forms.Label
        Me.lblInstruction = New System.Windows.Forms.Label
        Me.chkChannels = New System.Windows.Forms.CheckBox
        Me.chkRanges = New System.Windows.Forms.CheckBox
        Me.SuspendLayout()
        '
        'cmdStopConvert
        '
        Me.cmdStopConvert.BackColor = System.Drawing.SystemColors.Control
        Me.cmdStopConvert.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdStopConvert.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdStopConvert.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdStopConvert.Location = New System.Drawing.Point(344, 348)
        Me.cmdStopConvert.Name = "cmdStopConvert"
        Me.cmdStopConvert.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdStopConvert.Size = New System.Drawing.Size(57, 33)
        Me.cmdStopConvert.TabIndex = 13
        Me.cmdStopConvert.Text = "Quit"
        Me.cmdStopConvert.UseVisualStyleBackColor = False
        '
        'cmdUnloadQ
        '
        Me.cmdUnloadQ.BackColor = System.Drawing.SystemColors.Control
        Me.cmdUnloadQ.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdUnloadQ.Enabled = False
        Me.cmdUnloadQ.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdUnloadQ.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdUnloadQ.Location = New System.Drawing.Point(225, 348)
        Me.cmdUnloadQ.Name = "cmdUnloadQ"
        Me.cmdUnloadQ.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdUnloadQ.Size = New System.Drawing.Size(97, 33)
        Me.cmdUnloadQ.TabIndex = 47
        Me.cmdUnloadQ.Text = "Unload Queue"
        Me.cmdUnloadQ.UseVisualStyleBackColor = False
        Me.cmdUnloadQ.Visible = False
        '
        'cmdLoadQueue
        '
        Me.cmdLoadQueue.BackColor = System.Drawing.SystemColors.Control
        Me.cmdLoadQueue.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdLoadQueue.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdLoadQueue.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdLoadQueue.Location = New System.Drawing.Point(225, 348)
        Me.cmdLoadQueue.Name = "cmdLoadQueue"
        Me.cmdLoadQueue.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdLoadQueue.Size = New System.Drawing.Size(97, 33)
        Me.cmdLoadQueue.TabIndex = 14
        Me.cmdLoadQueue.Text = "Load Queue"
        Me.cmdLoadQueue.UseVisualStyleBackColor = False
        '
        'tmrConvert
        '
        Me.tmrConvert.Interval = 1000
        '
        '_lblADData_39
        '
        Me._lblADData_39.BackColor = System.Drawing.SystemColors.Window
        Me._lblADData_39.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblADData_39.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblADData_39.ForeColor = System.Drawing.Color.Blue
        Me._lblADData_39.Location = New System.Drawing.Point(344, 305)
        Me._lblADData_39.Name = "_lblADData_39"
        Me._lblADData_39.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblADData_39.Size = New System.Drawing.Size(65, 17)
        Me._lblADData_39.TabIndex = 46
        Me._lblADData_39.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblADData_38
        '
        Me._lblADData_38.BackColor = System.Drawing.SystemColors.Window
        Me._lblADData_38.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblADData_38.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblADData_38.ForeColor = System.Drawing.Color.Blue
        Me._lblADData_38.Location = New System.Drawing.Point(253, 305)
        Me._lblADData_38.Name = "_lblADData_38"
        Me._lblADData_38.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblADData_38.Size = New System.Drawing.Size(65, 17)
        Me._lblADData_38.TabIndex = 45
        Me._lblADData_38.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblADData_37
        '
        Me._lblADData_37.BackColor = System.Drawing.SystemColors.Window
        Me._lblADData_37.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblADData_37.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblADData_37.ForeColor = System.Drawing.Color.Blue
        Me._lblADData_37.Location = New System.Drawing.Point(164, 305)
        Me._lblADData_37.Name = "_lblADData_37"
        Me._lblADData_37.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblADData_37.Size = New System.Drawing.Size(65, 17)
        Me._lblADData_37.TabIndex = 44
        Me._lblADData_37.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblADData_36
        '
        Me._lblADData_36.BackColor = System.Drawing.SystemColors.Window
        Me._lblADData_36.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblADData_36.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblADData_36.ForeColor = System.Drawing.Color.Blue
        Me._lblADData_36.Location = New System.Drawing.Point(75, 305)
        Me._lblADData_36.Name = "_lblADData_36"
        Me._lblADData_36.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblADData_36.Size = New System.Drawing.Size(65, 17)
        Me._lblADData_36.TabIndex = 43
        Me._lblADData_36.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblADData_35
        '
        Me._lblADData_35.BackColor = System.Drawing.SystemColors.Window
        Me._lblADData_35.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblADData_35.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblADData_35.ForeColor = System.Drawing.Color.Blue
        Me._lblADData_35.Location = New System.Drawing.Point(344, 289)
        Me._lblADData_35.Name = "_lblADData_35"
        Me._lblADData_35.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblADData_35.Size = New System.Drawing.Size(65, 17)
        Me._lblADData_35.TabIndex = 42
        Me._lblADData_35.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblADData_34
        '
        Me._lblADData_34.BackColor = System.Drawing.SystemColors.Window
        Me._lblADData_34.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblADData_34.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblADData_34.ForeColor = System.Drawing.Color.Blue
        Me._lblADData_34.Location = New System.Drawing.Point(253, 289)
        Me._lblADData_34.Name = "_lblADData_34"
        Me._lblADData_34.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblADData_34.Size = New System.Drawing.Size(65, 17)
        Me._lblADData_34.TabIndex = 41
        Me._lblADData_34.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblADData_33
        '
        Me._lblADData_33.BackColor = System.Drawing.SystemColors.Window
        Me._lblADData_33.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblADData_33.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblADData_33.ForeColor = System.Drawing.Color.Blue
        Me._lblADData_33.Location = New System.Drawing.Point(164, 289)
        Me._lblADData_33.Name = "_lblADData_33"
        Me._lblADData_33.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblADData_33.Size = New System.Drawing.Size(65, 17)
        Me._lblADData_33.TabIndex = 40
        Me._lblADData_33.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblADData_32
        '
        Me._lblADData_32.BackColor = System.Drawing.SystemColors.Window
        Me._lblADData_32.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblADData_32.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblADData_32.ForeColor = System.Drawing.Color.Blue
        Me._lblADData_32.Location = New System.Drawing.Point(75, 289)
        Me._lblADData_32.Name = "_lblADData_32"
        Me._lblADData_32.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblADData_32.Size = New System.Drawing.Size(65, 17)
        Me._lblADData_32.TabIndex = 39
        Me._lblADData_32.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblADData_31
        '
        Me._lblADData_31.BackColor = System.Drawing.SystemColors.Window
        Me._lblADData_31.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblADData_31.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblADData_31.ForeColor = System.Drawing.Color.Blue
        Me._lblADData_31.Location = New System.Drawing.Point(344, 273)
        Me._lblADData_31.Name = "_lblADData_31"
        Me._lblADData_31.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblADData_31.Size = New System.Drawing.Size(65, 17)
        Me._lblADData_31.TabIndex = 38
        Me._lblADData_31.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblADData_30
        '
        Me._lblADData_30.BackColor = System.Drawing.SystemColors.Window
        Me._lblADData_30.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblADData_30.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblADData_30.ForeColor = System.Drawing.Color.Blue
        Me._lblADData_30.Location = New System.Drawing.Point(253, 273)
        Me._lblADData_30.Name = "_lblADData_30"
        Me._lblADData_30.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblADData_30.Size = New System.Drawing.Size(65, 17)
        Me._lblADData_30.TabIndex = 37
        Me._lblADData_30.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblADData_29
        '
        Me._lblADData_29.BackColor = System.Drawing.SystemColors.Window
        Me._lblADData_29.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblADData_29.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblADData_29.ForeColor = System.Drawing.Color.Blue
        Me._lblADData_29.Location = New System.Drawing.Point(164, 273)
        Me._lblADData_29.Name = "_lblADData_29"
        Me._lblADData_29.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblADData_29.Size = New System.Drawing.Size(65, 17)
        Me._lblADData_29.TabIndex = 36
        Me._lblADData_29.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblADData_28
        '
        Me._lblADData_28.BackColor = System.Drawing.SystemColors.Window
        Me._lblADData_28.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblADData_28.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblADData_28.ForeColor = System.Drawing.Color.Blue
        Me._lblADData_28.Location = New System.Drawing.Point(75, 273)
        Me._lblADData_28.Name = "_lblADData_28"
        Me._lblADData_28.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblADData_28.Size = New System.Drawing.Size(65, 17)
        Me._lblADData_28.TabIndex = 35
        Me._lblADData_28.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblADData_27
        '
        Me._lblADData_27.BackColor = System.Drawing.SystemColors.Window
        Me._lblADData_27.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblADData_27.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblADData_27.ForeColor = System.Drawing.Color.Blue
        Me._lblADData_27.Location = New System.Drawing.Point(344, 257)
        Me._lblADData_27.Name = "_lblADData_27"
        Me._lblADData_27.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblADData_27.Size = New System.Drawing.Size(65, 17)
        Me._lblADData_27.TabIndex = 34
        Me._lblADData_27.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblADData_26
        '
        Me._lblADData_26.BackColor = System.Drawing.SystemColors.Window
        Me._lblADData_26.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblADData_26.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblADData_26.ForeColor = System.Drawing.Color.Blue
        Me._lblADData_26.Location = New System.Drawing.Point(253, 257)
        Me._lblADData_26.Name = "_lblADData_26"
        Me._lblADData_26.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblADData_26.Size = New System.Drawing.Size(65, 17)
        Me._lblADData_26.TabIndex = 33
        Me._lblADData_26.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblADData_25
        '
        Me._lblADData_25.BackColor = System.Drawing.SystemColors.Window
        Me._lblADData_25.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblADData_25.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblADData_25.ForeColor = System.Drawing.Color.Blue
        Me._lblADData_25.Location = New System.Drawing.Point(164, 257)
        Me._lblADData_25.Name = "_lblADData_25"
        Me._lblADData_25.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblADData_25.Size = New System.Drawing.Size(65, 17)
        Me._lblADData_25.TabIndex = 32
        Me._lblADData_25.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblADData_24
        '
        Me._lblADData_24.BackColor = System.Drawing.SystemColors.Window
        Me._lblADData_24.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblADData_24.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblADData_24.ForeColor = System.Drawing.Color.Blue
        Me._lblADData_24.Location = New System.Drawing.Point(75, 257)
        Me._lblADData_24.Name = "_lblADData_24"
        Me._lblADData_24.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblADData_24.Size = New System.Drawing.Size(65, 17)
        Me._lblADData_24.TabIndex = 31
        Me._lblADData_24.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblADData_23
        '
        Me._lblADData_23.BackColor = System.Drawing.SystemColors.Window
        Me._lblADData_23.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblADData_23.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblADData_23.ForeColor = System.Drawing.Color.Blue
        Me._lblADData_23.Location = New System.Drawing.Point(344, 241)
        Me._lblADData_23.Name = "_lblADData_23"
        Me._lblADData_23.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblADData_23.Size = New System.Drawing.Size(65, 17)
        Me._lblADData_23.TabIndex = 30
        Me._lblADData_23.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblADData_22
        '
        Me._lblADData_22.BackColor = System.Drawing.SystemColors.Window
        Me._lblADData_22.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblADData_22.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblADData_22.ForeColor = System.Drawing.Color.Blue
        Me._lblADData_22.Location = New System.Drawing.Point(253, 241)
        Me._lblADData_22.Name = "_lblADData_22"
        Me._lblADData_22.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblADData_22.Size = New System.Drawing.Size(65, 17)
        Me._lblADData_22.TabIndex = 29
        Me._lblADData_22.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblADData_21
        '
        Me._lblADData_21.BackColor = System.Drawing.SystemColors.Window
        Me._lblADData_21.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblADData_21.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblADData_21.ForeColor = System.Drawing.Color.Blue
        Me._lblADData_21.Location = New System.Drawing.Point(164, 241)
        Me._lblADData_21.Name = "_lblADData_21"
        Me._lblADData_21.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblADData_21.Size = New System.Drawing.Size(65, 17)
        Me._lblADData_21.TabIndex = 28
        Me._lblADData_21.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblADData_20
        '
        Me._lblADData_20.BackColor = System.Drawing.SystemColors.Window
        Me._lblADData_20.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblADData_20.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblADData_20.ForeColor = System.Drawing.Color.Blue
        Me._lblADData_20.Location = New System.Drawing.Point(75, 241)
        Me._lblADData_20.Name = "_lblADData_20"
        Me._lblADData_20.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblADData_20.Size = New System.Drawing.Size(65, 17)
        Me._lblADData_20.TabIndex = 27
        Me._lblADData_20.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblADData_11
        '
        Me._lblADData_11.BackColor = System.Drawing.SystemColors.Window
        Me._lblADData_11.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblADData_11.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblADData_11.ForeColor = System.Drawing.Color.Blue
        Me._lblADData_11.Location = New System.Drawing.Point(344, 225)
        Me._lblADData_11.Name = "_lblADData_11"
        Me._lblADData_11.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblADData_11.Size = New System.Drawing.Size(65, 17)
        Me._lblADData_11.TabIndex = 18
        Me._lblADData_11.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblADData_10
        '
        Me._lblADData_10.BackColor = System.Drawing.SystemColors.Window
        Me._lblADData_10.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblADData_10.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblADData_10.ForeColor = System.Drawing.Color.Blue
        Me._lblADData_10.Location = New System.Drawing.Point(253, 225)
        Me._lblADData_10.Name = "_lblADData_10"
        Me._lblADData_10.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblADData_10.Size = New System.Drawing.Size(65, 17)
        Me._lblADData_10.TabIndex = 17
        Me._lblADData_10.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblADData_9
        '
        Me._lblADData_9.BackColor = System.Drawing.SystemColors.Window
        Me._lblADData_9.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblADData_9.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblADData_9.ForeColor = System.Drawing.Color.Blue
        Me._lblADData_9.Location = New System.Drawing.Point(164, 225)
        Me._lblADData_9.Name = "_lblADData_9"
        Me._lblADData_9.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblADData_9.Size = New System.Drawing.Size(65, 17)
        Me._lblADData_9.TabIndex = 16
        Me._lblADData_9.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblADData_8
        '
        Me._lblADData_8.BackColor = System.Drawing.SystemColors.Window
        Me._lblADData_8.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblADData_8.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblADData_8.ForeColor = System.Drawing.Color.Blue
        Me._lblADData_8.Location = New System.Drawing.Point(75, 225)
        Me._lblADData_8.Name = "_lblADData_8"
        Me._lblADData_8.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblADData_8.Size = New System.Drawing.Size(65, 17)
        Me._lblADData_8.TabIndex = 15
        Me._lblADData_8.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblADData_19
        '
        Me._lblADData_19.BackColor = System.Drawing.SystemColors.Window
        Me._lblADData_19.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblADData_19.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblADData_19.ForeColor = System.Drawing.Color.Blue
        Me._lblADData_19.Location = New System.Drawing.Point(344, 209)
        Me._lblADData_19.Name = "_lblADData_19"
        Me._lblADData_19.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblADData_19.Size = New System.Drawing.Size(65, 17)
        Me._lblADData_19.TabIndex = 26
        Me._lblADData_19.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblADData_18
        '
        Me._lblADData_18.BackColor = System.Drawing.SystemColors.Window
        Me._lblADData_18.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblADData_18.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblADData_18.ForeColor = System.Drawing.Color.Blue
        Me._lblADData_18.Location = New System.Drawing.Point(253, 209)
        Me._lblADData_18.Name = "_lblADData_18"
        Me._lblADData_18.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblADData_18.Size = New System.Drawing.Size(65, 17)
        Me._lblADData_18.TabIndex = 25
        Me._lblADData_18.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblADData_17
        '
        Me._lblADData_17.BackColor = System.Drawing.SystemColors.Window
        Me._lblADData_17.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblADData_17.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblADData_17.ForeColor = System.Drawing.Color.Blue
        Me._lblADData_17.Location = New System.Drawing.Point(164, 209)
        Me._lblADData_17.Name = "_lblADData_17"
        Me._lblADData_17.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblADData_17.Size = New System.Drawing.Size(65, 17)
        Me._lblADData_17.TabIndex = 24
        Me._lblADData_17.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblADData_16
        '
        Me._lblADData_16.BackColor = System.Drawing.SystemColors.Window
        Me._lblADData_16.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblADData_16.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblADData_16.ForeColor = System.Drawing.Color.Blue
        Me._lblADData_16.Location = New System.Drawing.Point(75, 209)
        Me._lblADData_16.Name = "_lblADData_16"
        Me._lblADData_16.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblADData_16.Size = New System.Drawing.Size(65, 17)
        Me._lblADData_16.TabIndex = 23
        Me._lblADData_16.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblADData_15
        '
        Me._lblADData_15.BackColor = System.Drawing.SystemColors.Window
        Me._lblADData_15.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblADData_15.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblADData_15.ForeColor = System.Drawing.Color.Blue
        Me._lblADData_15.Location = New System.Drawing.Point(344, 193)
        Me._lblADData_15.Name = "_lblADData_15"
        Me._lblADData_15.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblADData_15.Size = New System.Drawing.Size(65, 17)
        Me._lblADData_15.TabIndex = 22
        Me._lblADData_15.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblADData_14
        '
        Me._lblADData_14.BackColor = System.Drawing.SystemColors.Window
        Me._lblADData_14.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblADData_14.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblADData_14.ForeColor = System.Drawing.Color.Blue
        Me._lblADData_14.Location = New System.Drawing.Point(253, 193)
        Me._lblADData_14.Name = "_lblADData_14"
        Me._lblADData_14.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblADData_14.Size = New System.Drawing.Size(65, 17)
        Me._lblADData_14.TabIndex = 21
        Me._lblADData_14.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblADData_13
        '
        Me._lblADData_13.BackColor = System.Drawing.SystemColors.Window
        Me._lblADData_13.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblADData_13.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblADData_13.ForeColor = System.Drawing.Color.Blue
        Me._lblADData_13.Location = New System.Drawing.Point(164, 193)
        Me._lblADData_13.Name = "_lblADData_13"
        Me._lblADData_13.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblADData_13.Size = New System.Drawing.Size(65, 17)
        Me._lblADData_13.TabIndex = 20
        Me._lblADData_13.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblADData_12
        '
        Me._lblADData_12.BackColor = System.Drawing.SystemColors.Window
        Me._lblADData_12.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblADData_12.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblADData_12.ForeColor = System.Drawing.Color.Blue
        Me._lblADData_12.Location = New System.Drawing.Point(75, 193)
        Me._lblADData_12.Name = "_lblADData_12"
        Me._lblADData_12.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblADData_12.Size = New System.Drawing.Size(65, 17)
        Me._lblADData_12.TabIndex = 19
        Me._lblADData_12.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblADData_7
        '
        Me._lblADData_7.BackColor = System.Drawing.SystemColors.Window
        Me._lblADData_7.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblADData_7.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblADData_7.ForeColor = System.Drawing.Color.Blue
        Me._lblADData_7.Location = New System.Drawing.Point(344, 177)
        Me._lblADData_7.Name = "_lblADData_7"
        Me._lblADData_7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblADData_7.Size = New System.Drawing.Size(65, 17)
        Me._lblADData_7.TabIndex = 12
        Me._lblADData_7.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblADData_6
        '
        Me._lblADData_6.BackColor = System.Drawing.SystemColors.Window
        Me._lblADData_6.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblADData_6.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblADData_6.ForeColor = System.Drawing.Color.Blue
        Me._lblADData_6.Location = New System.Drawing.Point(253, 177)
        Me._lblADData_6.Name = "_lblADData_6"
        Me._lblADData_6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblADData_6.Size = New System.Drawing.Size(65, 17)
        Me._lblADData_6.TabIndex = 11
        Me._lblADData_6.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblADData_5
        '
        Me._lblADData_5.BackColor = System.Drawing.SystemColors.Window
        Me._lblADData_5.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblADData_5.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblADData_5.ForeColor = System.Drawing.Color.Blue
        Me._lblADData_5.Location = New System.Drawing.Point(164, 177)
        Me._lblADData_5.Name = "_lblADData_5"
        Me._lblADData_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblADData_5.Size = New System.Drawing.Size(65, 17)
        Me._lblADData_5.TabIndex = 10
        Me._lblADData_5.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblADData_4
        '
        Me._lblADData_4.BackColor = System.Drawing.SystemColors.Window
        Me._lblADData_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblADData_4.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblADData_4.ForeColor = System.Drawing.Color.Blue
        Me._lblADData_4.Location = New System.Drawing.Point(75, 177)
        Me._lblADData_4.Name = "_lblADData_4"
        Me._lblADData_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblADData_4.Size = New System.Drawing.Size(65, 17)
        Me._lblADData_4.TabIndex = 9
        Me._lblADData_4.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblADData_3
        '
        Me._lblADData_3.BackColor = System.Drawing.SystemColors.Window
        Me._lblADData_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblADData_3.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblADData_3.ForeColor = System.Drawing.Color.Blue
        Me._lblADData_3.Location = New System.Drawing.Point(344, 161)
        Me._lblADData_3.Name = "_lblADData_3"
        Me._lblADData_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblADData_3.Size = New System.Drawing.Size(65, 17)
        Me._lblADData_3.TabIndex = 8
        Me._lblADData_3.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblADData_2
        '
        Me._lblADData_2.BackColor = System.Drawing.SystemColors.Window
        Me._lblADData_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblADData_2.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblADData_2.ForeColor = System.Drawing.Color.Blue
        Me._lblADData_2.Location = New System.Drawing.Point(253, 161)
        Me._lblADData_2.Name = "_lblADData_2"
        Me._lblADData_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblADData_2.Size = New System.Drawing.Size(65, 17)
        Me._lblADData_2.TabIndex = 7
        Me._lblADData_2.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblADData_1
        '
        Me._lblADData_1.BackColor = System.Drawing.SystemColors.Window
        Me._lblADData_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblADData_1.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblADData_1.ForeColor = System.Drawing.Color.Blue
        Me._lblADData_1.Location = New System.Drawing.Point(164, 161)
        Me._lblADData_1.Name = "_lblADData_1"
        Me._lblADData_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblADData_1.Size = New System.Drawing.Size(65, 17)
        Me._lblADData_1.TabIndex = 6
        Me._lblADData_1.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblADData_0
        '
        Me._lblADData_0.BackColor = System.Drawing.SystemColors.Window
        Me._lblADData_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblADData_0.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblADData_0.ForeColor = System.Drawing.Color.Blue
        Me._lblADData_0.Location = New System.Drawing.Point(75, 161)
        Me._lblADData_0.Name = "_lblADData_0"
        Me._lblADData_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblADData_0.Size = New System.Drawing.Size(65, 17)
        Me._lblADData_0.TabIndex = 5
        Me._lblADData_0.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblShowRange_3
        '
        Me._lblShowRange_3.BackColor = System.Drawing.SystemColors.Window
        Me._lblShowRange_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblShowRange_3.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblShowRange_3.ForeColor = System.Drawing.Color.Blue
        Me._lblShowRange_3.Location = New System.Drawing.Point(337, 129)
        Me._lblShowRange_3.Name = "_lblShowRange_3"
        Me._lblShowRange_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblShowRange_3.Size = New System.Drawing.Size(80, 17)
        Me._lblShowRange_3.TabIndex = 52
        Me._lblShowRange_3.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblShowRange_2
        '
        Me._lblShowRange_2.BackColor = System.Drawing.SystemColors.Window
        Me._lblShowRange_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblShowRange_2.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblShowRange_2.ForeColor = System.Drawing.Color.Blue
        Me._lblShowRange_2.Location = New System.Drawing.Point(246, 129)
        Me._lblShowRange_2.Name = "_lblShowRange_2"
        Me._lblShowRange_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblShowRange_2.Size = New System.Drawing.Size(80, 17)
        Me._lblShowRange_2.TabIndex = 51
        Me._lblShowRange_2.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblShowRange_1
        '
        Me._lblShowRange_1.BackColor = System.Drawing.SystemColors.Window
        Me._lblShowRange_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblShowRange_1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblShowRange_1.ForeColor = System.Drawing.Color.Blue
        Me._lblShowRange_1.Location = New System.Drawing.Point(157, 129)
        Me._lblShowRange_1.Name = "_lblShowRange_1"
        Me._lblShowRange_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblShowRange_1.Size = New System.Drawing.Size(80, 17)
        Me._lblShowRange_1.TabIndex = 50
        Me._lblShowRange_1.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblShowRange_0
        '
        Me._lblShowRange_0.BackColor = System.Drawing.SystemColors.Window
        Me._lblShowRange_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblShowRange_0.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblShowRange_0.ForeColor = System.Drawing.Color.Blue
        Me._lblShowRange_0.Location = New System.Drawing.Point(68, 129)
        Me._lblShowRange_0.Name = "_lblShowRange_0"
        Me._lblShowRange_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblShowRange_0.Size = New System.Drawing.Size(80, 17)
        Me._lblShowRange_0.TabIndex = 49
        Me._lblShowRange_0.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblRange
        '
        Me.lblRange.BackColor = System.Drawing.SystemColors.Window
        Me.lblRange.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRange.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRange.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblRange.Location = New System.Drawing.Point(14, 129)
        Me.lblRange.Name = "lblRange"
        Me.lblRange.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRange.Size = New System.Drawing.Size(49, 17)
        Me.lblRange.TabIndex = 48
        Me.lblRange.Text = "Range:"
        '
        'lblChan3
        '
        Me.lblChan3.BackColor = System.Drawing.SystemColors.Window
        Me.lblChan3.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblChan3.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblChan3.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblChan3.Location = New System.Drawing.Point(339, 105)
        Me.lblChan3.Name = "lblChan3"
        Me.lblChan3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblChan3.Size = New System.Drawing.Size(80, 17)
        Me.lblChan3.TabIndex = 4
        Me.lblChan3.Text = "Channel 3"
        Me.lblChan3.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblChan2
        '
        Me.lblChan2.BackColor = System.Drawing.SystemColors.Window
        Me.lblChan2.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblChan2.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblChan2.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblChan2.Location = New System.Drawing.Point(248, 105)
        Me.lblChan2.Name = "lblChan2"
        Me.lblChan2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblChan2.Size = New System.Drawing.Size(80, 17)
        Me.lblChan2.TabIndex = 3
        Me.lblChan2.Text = "Channel 2"
        Me.lblChan2.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblChan1
        '
        Me.lblChan1.BackColor = System.Drawing.SystemColors.Window
        Me.lblChan1.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblChan1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblChan1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblChan1.Location = New System.Drawing.Point(159, 105)
        Me.lblChan1.Name = "lblChan1"
        Me.lblChan1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblChan1.Size = New System.Drawing.Size(80, 17)
        Me.lblChan1.TabIndex = 2
        Me.lblChan1.Text = "Channel 1"
        Me.lblChan1.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblChan0
        '
        Me.lblChan0.BackColor = System.Drawing.SystemColors.Window
        Me.lblChan0.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblChan0.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblChan0.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblChan0.Location = New System.Drawing.Point(70, 105)
        Me.lblChan0.Name = "lblChan0"
        Me.lblChan0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblChan0.Size = New System.Drawing.Size(80, 17)
        Me.lblChan0.TabIndex = 1
        Me.lblChan0.Text = "Channel 0"
        Me.lblChan0.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblDemoFunction
        '
        Me.lblDemoFunction.BackColor = System.Drawing.SystemColors.Window
        Me.lblDemoFunction.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDemoFunction.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDemoFunction.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblDemoFunction.Location = New System.Drawing.Point(9, 5)
        Me.lblDemoFunction.Name = "lblDemoFunction"
        Me.lblDemoFunction.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDemoFunction.Size = New System.Drawing.Size(463, 19)
        Me.lblDemoFunction.TabIndex = 0
        Me.lblDemoFunction.Text = "Demonstration of MccDaq.MccBoard.ALoadQueue()"
        Me.lblDemoFunction.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblInstruction
        '
        Me.lblInstruction.BackColor = System.Drawing.SystemColors.Window
        Me.lblInstruction.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblInstruction.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInstruction.ForeColor = System.Drawing.Color.Red
        Me.lblInstruction.Location = New System.Drawing.Point(33, 31)
        Me.lblInstruction.Name = "lblInstruction"
        Me.lblInstruction.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblInstruction.Size = New System.Drawing.Size(413, 60)
        Me.lblInstruction.TabIndex = 53
        Me.lblInstruction.Text = "Board 0 must have analog inputs that support paced acquisition."
        Me.lblInstruction.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'chkChannels
        '
        Me.chkChannels.AutoSize = True
        Me.chkChannels.Location = New System.Drawing.Point(35, 367)
        Me.chkChannels.Name = "chkChannels"
        Me.chkChannels.Size = New System.Drawing.Size(126, 18)
        Me.chkChannels.TabIndex = 54
        Me.chkChannels.Text = "Random Channels"
        Me.chkChannels.UseVisualStyleBackColor = True
        '
        'chkRanges
        '
        Me.chkRanges.AutoSize = True
        Me.chkRanges.Checked = True
        Me.chkRanges.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkRanges.Location = New System.Drawing.Point(35, 346)
        Me.chkRanges.Name = "chkRanges"
        Me.chkRanges.Size = New System.Drawing.Size(115, 18)
        Me.chkRanges.TabIndex = 55
        Me.chkRanges.Text = "Random Ranges"
        Me.chkRanges.UseVisualStyleBackColor = True
        '
        'frmDataDisplay
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 13)
        Me.BackColor = System.Drawing.SystemColors.Window
        Me.ClientSize = New System.Drawing.Size(483, 397)
        Me.Controls.Add(Me.chkRanges)
        Me.Controls.Add(Me.chkChannels)
        Me.Controls.Add(Me.lblInstruction)
        Me.Controls.Add(Me.cmdStopConvert)
        Me.Controls.Add(Me.cmdLoadQueue)
        Me.Controls.Add(Me._lblADData_39)
        Me.Controls.Add(Me._lblADData_38)
        Me.Controls.Add(Me._lblADData_37)
        Me.Controls.Add(Me._lblADData_36)
        Me.Controls.Add(Me._lblADData_35)
        Me.Controls.Add(Me._lblADData_34)
        Me.Controls.Add(Me._lblADData_33)
        Me.Controls.Add(Me._lblADData_32)
        Me.Controls.Add(Me._lblADData_31)
        Me.Controls.Add(Me._lblADData_30)
        Me.Controls.Add(Me._lblADData_29)
        Me.Controls.Add(Me._lblADData_28)
        Me.Controls.Add(Me._lblADData_27)
        Me.Controls.Add(Me._lblADData_26)
        Me.Controls.Add(Me._lblADData_25)
        Me.Controls.Add(Me._lblADData_24)
        Me.Controls.Add(Me._lblADData_23)
        Me.Controls.Add(Me._lblADData_22)
        Me.Controls.Add(Me._lblADData_21)
        Me.Controls.Add(Me._lblADData_20)
        Me.Controls.Add(Me._lblADData_11)
        Me.Controls.Add(Me._lblADData_10)
        Me.Controls.Add(Me._lblADData_9)
        Me.Controls.Add(Me._lblADData_8)
        Me.Controls.Add(Me._lblADData_19)
        Me.Controls.Add(Me._lblADData_18)
        Me.Controls.Add(Me._lblADData_17)
        Me.Controls.Add(Me._lblADData_16)
        Me.Controls.Add(Me._lblADData_15)
        Me.Controls.Add(Me._lblADData_14)
        Me.Controls.Add(Me._lblADData_13)
        Me.Controls.Add(Me._lblADData_12)
        Me.Controls.Add(Me._lblADData_7)
        Me.Controls.Add(Me._lblADData_6)
        Me.Controls.Add(Me._lblADData_5)
        Me.Controls.Add(Me._lblADData_4)
        Me.Controls.Add(Me._lblADData_3)
        Me.Controls.Add(Me._lblADData_2)
        Me.Controls.Add(Me._lblADData_1)
        Me.Controls.Add(Me._lblADData_0)
        Me.Controls.Add(Me._lblShowRange_3)
        Me.Controls.Add(Me._lblShowRange_2)
        Me.Controls.Add(Me._lblShowRange_1)
        Me.Controls.Add(Me._lblShowRange_0)
        Me.Controls.Add(Me.lblRange)
        Me.Controls.Add(Me.lblChan3)
        Me.Controls.Add(Me.lblChan2)
        Me.Controls.Add(Me.lblChan1)
        Me.Controls.Add(Me.lblChan0)
        Me.Controls.Add(Me.lblDemoFunction)
        Me.Controls.Add(Me.cmdUnloadQ)
        Me.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ForeColor = System.Drawing.Color.Blue
        Me.Location = New System.Drawing.Point(7, 103)
        Me.Name = "frmDataDisplay"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Universal Library Gain Queue"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Public WithEvents lblInstruction As System.Windows.Forms.Label
    Public lblADData As System.Windows.Forms.Label()
    Public lblShowRange As System.Windows.Forms.Label()
    Public lblChan As System.Windows.Forms.Label()
    Friend WithEvents chkChannels As System.Windows.Forms.CheckBox
    Friend WithEvents chkRanges As System.Windows.Forms.CheckBox

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
        '    MccDaq.ErrorReporting.DontPrint :all warnings and errors will be handled locally
        '    MccDaq.ErrorHandling.DontStop   :if any error is encountered, the program continues

        ReportError = MccDaq.ErrorReporting.DontPrint
        HandleError = MccDaq.ErrorHandling.DontStop
        ULStat = MccDaq.MccService.ErrHandling(ReportError, HandleError)
        If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then
            Stop
        End If

        ' Note: Any change to label names requires a change to the corresponding array element
        lblADData = New System.Windows.Forms.Label(39) _
        {_lblADData_0, _lblADData_1, _lblADData_2, _lblADData_3, _
        _lblADData_4, _lblADData_5, _lblADData_6, _lblADData_7, _
        _lblADData_8, _lblADData_9, _lblADData_10, _lblADData_11, _
        _lblADData_12, _lblADData_13, _lblADData_14, _lblADData_15, _
        _lblADData_16, _lblADData_17, _lblADData_18, _lblADData_19, _
        _lblADData_20, _lblADData_21, _lblADData_22, _lblADData_23, _
        _lblADData_24, _lblADData_25, _lblADData_26, _lblADData_27, _
        _lblADData_28, _lblADData_29, _lblADData_30, _lblADData_31, _
        _lblADData_32, _lblADData_33, _lblADData_34, _lblADData_35, _
        _lblADData_36, _lblADData_37, _lblADData_38, _lblADData_39}

        lblShowRange = New System.Windows.Forms.Label(3) _
        {_lblShowRange_0, _lblShowRange_1, _lblShowRange_2, _lblShowRange_3}

        lblChan = New System.Windows.Forms.Label(3) _
        {lblChan0, lblChan1, lblChan2, lblChan3}

    End Sub

#End Region

End Class