'==============================================================================

' File:                         ULAI04.VB

' Library Call Demonstrated:    Mccdaq.MccBoard.AInScan() with Mccdaq.MccBoard.AConvertData()

' Purpose:                      Demonstrates the conversion of data values
'                               after they have been collected.

' Demonstration:                Collects data points from up to 8 analog inputs.
'                               Data points are collected as raw data values and
'                               (for devices that store channel tags) are converted 
'                               from 16-bit composite data to channel numbers and 
'                               12-bit data values using Mccdaq.MccBoard.AConvertData.

' Other Library Calls:          Mccdaq.MccBoard.GetStatus()
'                               Mccdaq.MccBoard.StopBackground()
'                               Mccdaq.MccBoard.ErrHandling()

' Special Requirements:         Analog signals on up to eight input channels.

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

    Const NumPoints As Integer = 120    ' Number of data points to collect
    Const FirstPoint As Integer = 0     ' set first element in buffer to transfer to array

    Dim ADData() As UInt16              ' dimension an array to hold the input values
    Dim ChanTags() As UInt16            ' dimension an array to hold the channel tags

    Dim MemHandle As IntPtr     ' define a variable to contain the handle for
    '                             memory allocated by Windows through Mccdaq.MccBoard.WinBufAlloc()

    Private Sub frmDataDisplay_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim DefaultTrig As MccDaq.TriggerType

        InitUL()

        ' determine the number of analog channels and their capabilities
        Dim ChannelType As Integer = ANALOGINPUT
        NumAIChans = FindAnalogChansOfType(DaqBoard, ChannelType, _
            ADResolution, Range, LowChan, DefaultTrig)

        If (NumAIChans = 0) Then
            lblInstruction.Text = "Board " & DaqBoard.BoardNum.ToString() & _
            " does not have analog input channels."
            cmdStartAcq.Enabled = False
            txtHighChan.Enabled = False
        ElseIf ADResolution > 16 Then
            lblInstruction.Text = "Board " & DaqBoard.BoardNum.ToString() & _
            " resolution is greater that 16-bits. AConvertData does not " _
            & "support high resolution data."
            cmdStartAcq.Enabled = False
            txtHighChan.Enabled = False
        Else
            ' set aside memory to hold 16-bit data
            ReDim ChanTags(NumPoints)
            ReDim ADData(NumPoints)
            MemHandle = MccDaq.MccService.WinBufAllocEx(NumPoints)
            If MemHandle = 0 Then Stop
            If (NumAIChans > 8) Then NumAIChans = 8 'limit to 8 for display
            MaxChan = LowChan + NumAIChans - 1
            lblInstruction.Text = "Board " & DaqBoard.BoardNum.ToString() & _
                " collecting analog data on up to " & NumAIChans.ToString() & _
                " channels using AInScan with Range set to " & Range.ToString() & _
                ".  Click the Convert Data button when complete. This separates " & _
                "channel tags from data for devices that support it but is " & _
                "ignored for most devices."
        End If

    End Sub

    Private Sub cmdStartAcq_Click(ByVal eventSender As System.Object, _
    ByVal eventArgs As System.EventArgs) Handles cmdStartAcq.Click

        Dim ULStat As MccDaq.ErrorInfo
        Dim Options As MccDaq.ScanOptions
        Dim Rate As Integer
        Dim Count As Integer
        Dim i As Short
        Dim ValidChan As Boolean

        ' Collect the values by calling MccDaq.MccBoard.AInScan function
        '  Parameters:
        '    LowChan    :the first channel of the scan
        '    HighChan   :the last channel of the scan
        '    Count      :the total number of A/D samples to collect
        '    Rate       :sample rate
        '    Range      :the range for the board
        '    MemHanel   :the handle to windows buffer
        '    Options    :data collection options

        For i = 0 To 7
            lblShowTag(i).Text = ""
        Next i

        ValidChan = Integer.TryParse(txtHighChan.Text, HighChan)
        If ValidChan Then
            If (HighChan > MaxChan) Then HighChan = MaxChan
            txtHighChan.Text = Str(HighChan)
        End If

        Count = NumPoints           ' total number of data points to collect

        ' per channel sampling rate ((samples per second) per channel)
        Rate = 1000 / ((HighChan - LowChan) + 1)

        Options = MccDaq.ScanOptions.Background
        ' collect data in background mode as 16-bit values (no conversion)

        ULStat = DaqBoard.AInScan(LowChan, HighChan, Count, Rate, Range, MemHandle, Options)
        If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then Stop

        tmrCheckStatus.Enabled = True

    End Sub

    Private Sub tmrCheckStatus_Tick(ByVal eventSender As System.Object, _
    ByVal eventArgs As System.EventArgs) Handles tmrCheckStatus.Tick

        Dim j As Integer
        Dim i As Integer
        Dim ULStat As MccDaq.ErrorInfo
        Dim CurIndex As Integer
        Dim CurCount As Integer
        Dim Status As Short

        ' Check the current status of the background data collection
        '  Parameters:
        '    Status     :current status of the background data collection
        '    CurCount   :current number of samples collected
        '    CurIndex   :index to the data buffer pointing to the last value transferred
        '   FunctionType: A/D operation (MccDaq.FunctionType.AiFunction)

        ' Transfer the data from the memory buffer set up by Windows
        ' to an array for use by Visual this program

        tmrCheckStatus.Stop()

        ULStat = DaqBoard.GetStatus(Status, CurCount, CurIndex, MccDaq.FunctionType.AiFunction)
        If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then Stop

        If Status = MccDaq.MccBoard.Idle Then
            ULStat = DaqBoard.StopBackground(MccDaq.FunctionType.AiFunction)
            If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then Stop

            ULStat = MccDaq.MccService.WinBufToArray(MemHandle, ADData, FirstPoint, NumPoints)
            If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then Stop

            For i = 0 To HighChan
                lblADData(i).Text = ADData(i).ToString("D")
            Next i

            For j = HighChan + 1 To 7
                lblADData(j).Text = ""
            Next j
            cmdConvData.Enabled = True
        Else
            tmrCheckStatus.Start()
        End If

    End Sub

    Private Sub cmdConvData_Click(ByVal eventSender As System.Object, _
    ByVal eventArgs As System.EventArgs) Handles cmdConvData.Click

        Dim i As Integer
        Dim ULStat As MccDaq.ErrorInfo

        ' Use MccDaq.MccBoard.AConvertData() to convert the 16-bit values
        ' in ADData() to 12-bit values
        '  Parameters:
        '    NumPoints     :the number of data values to convert
        '    ADData        :the array holding the 16-bit data values to be converted
        '    ChanTags      : the arrey that will hold the channel tags 

        ULStat = DaqBoard.AConvertData(NumPoints, ADData, ChanTags)
        If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then Stop

        For i = 0 To HighChan
            lblADData(i).Text = ADData(i).ToString("D")
            lblShowTag(i).Text = ChanTags(i).ToString("D")
        Next i
        cmdConvData.Enabled = False

    End Sub

    Private Sub cmdStopConvert_Click(ByVal eventSender As System.Object, _
    ByVal eventArgs As System.EventArgs) Handles cmdStopConvert.Click

        Dim ULStat As MccDaq.ErrorInfo

        ' Free up memory for use by other programs
        ULStat = MccDaq.MccService.WinBufFreeEx(MemHandle)

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
    Public WithEvents txtHighChan As System.Windows.Forms.TextBox
    Public WithEvents cmdStopConvert As System.Windows.Forms.Button
    Public WithEvents cmdConvData As System.Windows.Forms.Button
    Public WithEvents cmdStartAcq As System.Windows.Forms.Button
    Public WithEvents tmrCheckStatus As System.Windows.Forms.Timer
    Public WithEvents lblMeasure As System.Windows.Forms.Label
    Public WithEvents _lblADData_7 As System.Windows.Forms.Label
    Public WithEvents _lblShowTag_7 As System.Windows.Forms.Label
    Public WithEvents lblChan7 As System.Windows.Forms.Label
    Public WithEvents _lblADData_3 As System.Windows.Forms.Label
    Public WithEvents _lblShowTag_3 As System.Windows.Forms.Label
    Public WithEvents lblChan3 As System.Windows.Forms.Label
    Public WithEvents _lblADData_6 As System.Windows.Forms.Label
    Public WithEvents _lblShowTag_6 As System.Windows.Forms.Label
    Public WithEvents lblChan6 As System.Windows.Forms.Label
    Public WithEvents _lblADData_2 As System.Windows.Forms.Label
    Public WithEvents _lblShowTag_2 As System.Windows.Forms.Label
    Public WithEvents lblChan2 As System.Windows.Forms.Label
    Public WithEvents _lblADData_5 As System.Windows.Forms.Label
    Public WithEvents _lblShowTag_5 As System.Windows.Forms.Label
    Public WithEvents lblChan5 As System.Windows.Forms.Label
    Public WithEvents _lblADData_1 As System.Windows.Forms.Label
    Public WithEvents _lblShowTag_1 As System.Windows.Forms.Label
    Public WithEvents lblChan1 As System.Windows.Forms.Label
    Public WithEvents _lblADData_4 As System.Windows.Forms.Label
    Public WithEvents _lblShowTag_4 As System.Windows.Forms.Label
    Public WithEvents lblChan4 As System.Windows.Forms.Label
    Public WithEvents _lblADData_0 As System.Windows.Forms.Label
    Public WithEvents _lblShowTag_0 As System.Windows.Forms.Label
    Public WithEvents lblChan0 As System.Windows.Forms.Label
    Public WithEvents lblData2 As System.Windows.Forms.Label
    Public WithEvents lblTag4 As System.Windows.Forms.Label
    Public WithEvents lblData1 As System.Windows.Forms.Label
    Public WithEvents lblTag2 As System.Windows.Forms.Label
    Public WithEvents lblTag3 As System.Windows.Forms.Label
    Public WithEvents lblTag1 As System.Windows.Forms.Label
    Public WithEvents lblDemoFunction As System.Windows.Forms.Label

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDataDisplay))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.txtHighChan = New System.Windows.Forms.TextBox
        Me.cmdStopConvert = New System.Windows.Forms.Button
        Me.cmdConvData = New System.Windows.Forms.Button
        Me.cmdStartAcq = New System.Windows.Forms.Button
        Me.tmrCheckStatus = New System.Windows.Forms.Timer(Me.components)
        Me.lblMeasure = New System.Windows.Forms.Label
        Me._lblADData_7 = New System.Windows.Forms.Label
        Me._lblShowTag_7 = New System.Windows.Forms.Label
        Me.lblChan7 = New System.Windows.Forms.Label
        Me._lblADData_3 = New System.Windows.Forms.Label
        Me._lblShowTag_3 = New System.Windows.Forms.Label
        Me.lblChan3 = New System.Windows.Forms.Label
        Me._lblADData_6 = New System.Windows.Forms.Label
        Me._lblShowTag_6 = New System.Windows.Forms.Label
        Me.lblChan6 = New System.Windows.Forms.Label
        Me._lblADData_2 = New System.Windows.Forms.Label
        Me._lblShowTag_2 = New System.Windows.Forms.Label
        Me.lblChan2 = New System.Windows.Forms.Label
        Me._lblADData_5 = New System.Windows.Forms.Label
        Me._lblShowTag_5 = New System.Windows.Forms.Label
        Me.lblChan5 = New System.Windows.Forms.Label
        Me._lblADData_1 = New System.Windows.Forms.Label
        Me._lblShowTag_1 = New System.Windows.Forms.Label
        Me.lblChan1 = New System.Windows.Forms.Label
        Me._lblADData_4 = New System.Windows.Forms.Label
        Me._lblShowTag_4 = New System.Windows.Forms.Label
        Me.lblChan4 = New System.Windows.Forms.Label
        Me._lblADData_0 = New System.Windows.Forms.Label
        Me._lblShowTag_0 = New System.Windows.Forms.Label
        Me.lblChan0 = New System.Windows.Forms.Label
        Me.lblData2 = New System.Windows.Forms.Label
        Me.lblTag4 = New System.Windows.Forms.Label
        Me.lblData1 = New System.Windows.Forms.Label
        Me.lblTag2 = New System.Windows.Forms.Label
        Me.lblTag3 = New System.Windows.Forms.Label
        Me.lblTag1 = New System.Windows.Forms.Label
        Me.lblDemoFunction = New System.Windows.Forms.Label
        Me.lblInstruction = New System.Windows.Forms.Label
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
        Me.txtHighChan.Location = New System.Drawing.Point(204, 177)
        Me.txtHighChan.MaxLength = 0
        Me.txtHighChan.Name = "txtHighChan"
        Me.txtHighChan.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtHighChan.Size = New System.Drawing.Size(33, 20)
        Me.txtHighChan.TabIndex = 34
        Me.txtHighChan.Text = "0"
        Me.txtHighChan.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'cmdStopConvert
        '
        Me.cmdStopConvert.BackColor = System.Drawing.SystemColors.Control
        Me.cmdStopConvert.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdStopConvert.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdStopConvert.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdStopConvert.Location = New System.Drawing.Point(269, 330)
        Me.cmdStopConvert.Name = "cmdStopConvert"
        Me.cmdStopConvert.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdStopConvert.Size = New System.Drawing.Size(52, 27)
        Me.cmdStopConvert.TabIndex = 17
        Me.cmdStopConvert.Text = "Quit"
        Me.cmdStopConvert.UseVisualStyleBackColor = False
        '
        'cmdConvData
        '
        Me.cmdConvData.BackColor = System.Drawing.SystemColors.Control
        Me.cmdConvData.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdConvData.Enabled = False
        Me.cmdConvData.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdConvData.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdConvData.Location = New System.Drawing.Point(199, 131)
        Me.cmdConvData.Name = "cmdConvData"
        Me.cmdConvData.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdConvData.Size = New System.Drawing.Size(84, 27)
        Me.cmdConvData.TabIndex = 19
        Me.cmdConvData.Text = "Convert Data"
        Me.cmdConvData.UseVisualStyleBackColor = False
        '
        'cmdStartAcq
        '
        Me.cmdStartAcq.BackColor = System.Drawing.SystemColors.Control
        Me.cmdStartAcq.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdStartAcq.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdStartAcq.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdStartAcq.Location = New System.Drawing.Point(77, 131)
        Me.cmdStartAcq.Name = "cmdStartAcq"
        Me.cmdStartAcq.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdStartAcq.Size = New System.Drawing.Size(84, 27)
        Me.cmdStartAcq.TabIndex = 18
        Me.cmdStartAcq.Text = "Acquire Data"
        Me.cmdStartAcq.UseVisualStyleBackColor = False
        '
        'tmrCheckStatus
        '
        Me.tmrCheckStatus.Interval = 200
        '
        'lblMeasure
        '
        Me.lblMeasure.BackColor = System.Drawing.SystemColors.Window
        Me.lblMeasure.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblMeasure.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMeasure.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblMeasure.Location = New System.Drawing.Point(63, 177)
        Me.lblMeasure.Name = "lblMeasure"
        Me.lblMeasure.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMeasure.Size = New System.Drawing.Size(137, 17)
        Me.lblMeasure.TabIndex = 35
        Me.lblMeasure.Text = "Measure Channels 0 to"
        Me.lblMeasure.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblADData_7
        '
        Me._lblADData_7.BackColor = System.Drawing.SystemColors.Window
        Me._lblADData_7.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblADData_7.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblADData_7.ForeColor = System.Drawing.Color.Blue
        Me._lblADData_7.Location = New System.Drawing.Point(272, 292)
        Me._lblADData_7.Name = "_lblADData_7"
        Me._lblADData_7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblADData_7.Size = New System.Drawing.Size(65, 17)
        Me._lblADData_7.TabIndex = 16
        Me._lblADData_7.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblShowTag_7
        '
        Me._lblShowTag_7.BackColor = System.Drawing.SystemColors.Window
        Me._lblShowTag_7.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblShowTag_7.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblShowTag_7.ForeColor = System.Drawing.Color.Blue
        Me._lblShowTag_7.Location = New System.Drawing.Point(248, 292)
        Me._lblShowTag_7.Name = "_lblShowTag_7"
        Me._lblShowTag_7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblShowTag_7.Size = New System.Drawing.Size(17, 17)
        Me._lblShowTag_7.TabIndex = 27
        Me._lblShowTag_7.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblChan7
        '
        Me.lblChan7.BackColor = System.Drawing.SystemColors.Window
        Me.lblChan7.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblChan7.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblChan7.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblChan7.Location = New System.Drawing.Point(176, 292)
        Me.lblChan7.Name = "lblChan7"
        Me.lblChan7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblChan7.Size = New System.Drawing.Size(65, 17)
        Me.lblChan7.TabIndex = 8
        Me.lblChan7.Text = "Channel 7:"
        '
        '_lblADData_3
        '
        Me._lblADData_3.BackColor = System.Drawing.SystemColors.Window
        Me._lblADData_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblADData_3.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblADData_3.ForeColor = System.Drawing.Color.Blue
        Me._lblADData_3.Location = New System.Drawing.Point(104, 292)
        Me._lblADData_3.Name = "_lblADData_3"
        Me._lblADData_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblADData_3.Size = New System.Drawing.Size(65, 17)
        Me._lblADData_3.TabIndex = 12
        Me._lblADData_3.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblShowTag_3
        '
        Me._lblShowTag_3.BackColor = System.Drawing.SystemColors.Window
        Me._lblShowTag_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblShowTag_3.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblShowTag_3.ForeColor = System.Drawing.Color.Blue
        Me._lblShowTag_3.Location = New System.Drawing.Point(72, 292)
        Me._lblShowTag_3.Name = "_lblShowTag_3"
        Me._lblShowTag_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblShowTag_3.Size = New System.Drawing.Size(17, 17)
        Me._lblShowTag_3.TabIndex = 23
        Me._lblShowTag_3.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblChan3
        '
        Me.lblChan3.BackColor = System.Drawing.SystemColors.Window
        Me.lblChan3.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblChan3.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblChan3.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblChan3.Location = New System.Drawing.Point(8, 292)
        Me.lblChan3.Name = "lblChan3"
        Me.lblChan3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblChan3.Size = New System.Drawing.Size(65, 17)
        Me.lblChan3.TabIndex = 4
        Me.lblChan3.Text = "Channel 3:"
        '
        '_lblADData_6
        '
        Me._lblADData_6.BackColor = System.Drawing.SystemColors.Window
        Me._lblADData_6.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblADData_6.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblADData_6.ForeColor = System.Drawing.Color.Blue
        Me._lblADData_6.Location = New System.Drawing.Point(272, 276)
        Me._lblADData_6.Name = "_lblADData_6"
        Me._lblADData_6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblADData_6.Size = New System.Drawing.Size(65, 17)
        Me._lblADData_6.TabIndex = 15
        Me._lblADData_6.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblShowTag_6
        '
        Me._lblShowTag_6.BackColor = System.Drawing.SystemColors.Window
        Me._lblShowTag_6.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblShowTag_6.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblShowTag_6.ForeColor = System.Drawing.Color.Blue
        Me._lblShowTag_6.Location = New System.Drawing.Point(248, 276)
        Me._lblShowTag_6.Name = "_lblShowTag_6"
        Me._lblShowTag_6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblShowTag_6.Size = New System.Drawing.Size(17, 17)
        Me._lblShowTag_6.TabIndex = 26
        Me._lblShowTag_6.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblChan6
        '
        Me.lblChan6.BackColor = System.Drawing.SystemColors.Window
        Me.lblChan6.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblChan6.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblChan6.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblChan6.Location = New System.Drawing.Point(176, 276)
        Me.lblChan6.Name = "lblChan6"
        Me.lblChan6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblChan6.Size = New System.Drawing.Size(65, 17)
        Me.lblChan6.TabIndex = 7
        Me.lblChan6.Text = "Channel 6:"
        '
        '_lblADData_2
        '
        Me._lblADData_2.BackColor = System.Drawing.SystemColors.Window
        Me._lblADData_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblADData_2.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblADData_2.ForeColor = System.Drawing.Color.Blue
        Me._lblADData_2.Location = New System.Drawing.Point(104, 276)
        Me._lblADData_2.Name = "_lblADData_2"
        Me._lblADData_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblADData_2.Size = New System.Drawing.Size(65, 17)
        Me._lblADData_2.TabIndex = 11
        Me._lblADData_2.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblShowTag_2
        '
        Me._lblShowTag_2.BackColor = System.Drawing.SystemColors.Window
        Me._lblShowTag_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblShowTag_2.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblShowTag_2.ForeColor = System.Drawing.Color.Blue
        Me._lblShowTag_2.Location = New System.Drawing.Point(72, 276)
        Me._lblShowTag_2.Name = "_lblShowTag_2"
        Me._lblShowTag_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblShowTag_2.Size = New System.Drawing.Size(17, 17)
        Me._lblShowTag_2.TabIndex = 22
        Me._lblShowTag_2.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblChan2
        '
        Me.lblChan2.BackColor = System.Drawing.SystemColors.Window
        Me.lblChan2.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblChan2.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblChan2.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblChan2.Location = New System.Drawing.Point(8, 276)
        Me.lblChan2.Name = "lblChan2"
        Me.lblChan2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblChan2.Size = New System.Drawing.Size(65, 17)
        Me.lblChan2.TabIndex = 3
        Me.lblChan2.Text = "Channel 2:"
        '
        '_lblADData_5
        '
        Me._lblADData_5.BackColor = System.Drawing.SystemColors.Window
        Me._lblADData_5.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblADData_5.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblADData_5.ForeColor = System.Drawing.Color.Blue
        Me._lblADData_5.Location = New System.Drawing.Point(272, 260)
        Me._lblADData_5.Name = "_lblADData_5"
        Me._lblADData_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblADData_5.Size = New System.Drawing.Size(65, 17)
        Me._lblADData_5.TabIndex = 14
        Me._lblADData_5.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblShowTag_5
        '
        Me._lblShowTag_5.BackColor = System.Drawing.SystemColors.Window
        Me._lblShowTag_5.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblShowTag_5.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblShowTag_5.ForeColor = System.Drawing.Color.Blue
        Me._lblShowTag_5.Location = New System.Drawing.Point(248, 260)
        Me._lblShowTag_5.Name = "_lblShowTag_5"
        Me._lblShowTag_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblShowTag_5.Size = New System.Drawing.Size(17, 17)
        Me._lblShowTag_5.TabIndex = 25
        Me._lblShowTag_5.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblChan5
        '
        Me.lblChan5.BackColor = System.Drawing.SystemColors.Window
        Me.lblChan5.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblChan5.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblChan5.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblChan5.Location = New System.Drawing.Point(176, 260)
        Me.lblChan5.Name = "lblChan5"
        Me.lblChan5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblChan5.Size = New System.Drawing.Size(65, 17)
        Me.lblChan5.TabIndex = 6
        Me.lblChan5.Text = "Channel 5:"
        '
        '_lblADData_1
        '
        Me._lblADData_1.BackColor = System.Drawing.SystemColors.Window
        Me._lblADData_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblADData_1.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblADData_1.ForeColor = System.Drawing.Color.Blue
        Me._lblADData_1.Location = New System.Drawing.Point(104, 260)
        Me._lblADData_1.Name = "_lblADData_1"
        Me._lblADData_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblADData_1.Size = New System.Drawing.Size(65, 17)
        Me._lblADData_1.TabIndex = 10
        Me._lblADData_1.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblShowTag_1
        '
        Me._lblShowTag_1.BackColor = System.Drawing.SystemColors.Window
        Me._lblShowTag_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblShowTag_1.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblShowTag_1.ForeColor = System.Drawing.Color.Blue
        Me._lblShowTag_1.Location = New System.Drawing.Point(72, 260)
        Me._lblShowTag_1.Name = "_lblShowTag_1"
        Me._lblShowTag_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblShowTag_1.Size = New System.Drawing.Size(17, 17)
        Me._lblShowTag_1.TabIndex = 21
        Me._lblShowTag_1.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblChan1
        '
        Me.lblChan1.BackColor = System.Drawing.SystemColors.Window
        Me.lblChan1.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblChan1.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblChan1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblChan1.Location = New System.Drawing.Point(8, 260)
        Me.lblChan1.Name = "lblChan1"
        Me.lblChan1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblChan1.Size = New System.Drawing.Size(65, 17)
        Me.lblChan1.TabIndex = 2
        Me.lblChan1.Text = "Channel 1:"
        '
        '_lblADData_4
        '
        Me._lblADData_4.BackColor = System.Drawing.SystemColors.Window
        Me._lblADData_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblADData_4.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblADData_4.ForeColor = System.Drawing.Color.Blue
        Me._lblADData_4.Location = New System.Drawing.Point(272, 244)
        Me._lblADData_4.Name = "_lblADData_4"
        Me._lblADData_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblADData_4.Size = New System.Drawing.Size(65, 17)
        Me._lblADData_4.TabIndex = 13
        Me._lblADData_4.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblShowTag_4
        '
        Me._lblShowTag_4.BackColor = System.Drawing.SystemColors.Window
        Me._lblShowTag_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblShowTag_4.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblShowTag_4.ForeColor = System.Drawing.Color.Blue
        Me._lblShowTag_4.Location = New System.Drawing.Point(248, 244)
        Me._lblShowTag_4.Name = "_lblShowTag_4"
        Me._lblShowTag_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblShowTag_4.Size = New System.Drawing.Size(17, 17)
        Me._lblShowTag_4.TabIndex = 24
        Me._lblShowTag_4.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblChan4
        '
        Me.lblChan4.BackColor = System.Drawing.SystemColors.Window
        Me.lblChan4.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblChan4.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblChan4.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblChan4.Location = New System.Drawing.Point(176, 244)
        Me.lblChan4.Name = "lblChan4"
        Me.lblChan4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblChan4.Size = New System.Drawing.Size(65, 17)
        Me.lblChan4.TabIndex = 5
        Me.lblChan4.Text = "Channel 4:"
        '
        '_lblADData_0
        '
        Me._lblADData_0.BackColor = System.Drawing.SystemColors.Window
        Me._lblADData_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblADData_0.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblADData_0.ForeColor = System.Drawing.Color.Blue
        Me._lblADData_0.Location = New System.Drawing.Point(104, 244)
        Me._lblADData_0.Name = "_lblADData_0"
        Me._lblADData_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblADData_0.Size = New System.Drawing.Size(65, 17)
        Me._lblADData_0.TabIndex = 9
        Me._lblADData_0.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblShowTag_0
        '
        Me._lblShowTag_0.BackColor = System.Drawing.SystemColors.Window
        Me._lblShowTag_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblShowTag_0.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblShowTag_0.ForeColor = System.Drawing.Color.Blue
        Me._lblShowTag_0.Location = New System.Drawing.Point(72, 244)
        Me._lblShowTag_0.Name = "_lblShowTag_0"
        Me._lblShowTag_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblShowTag_0.Size = New System.Drawing.Size(17, 17)
        Me._lblShowTag_0.TabIndex = 20
        Me._lblShowTag_0.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblChan0
        '
        Me.lblChan0.BackColor = System.Drawing.SystemColors.Window
        Me.lblChan0.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblChan0.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblChan0.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblChan0.Location = New System.Drawing.Point(8, 244)
        Me.lblChan0.Name = "lblChan0"
        Me.lblChan0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblChan0.Size = New System.Drawing.Size(65, 17)
        Me.lblChan0.TabIndex = 1
        Me.lblChan0.Text = "Channel 0:"
        '
        'lblData2
        '
        Me.lblData2.BackColor = System.Drawing.SystemColors.Window
        Me.lblData2.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblData2.Font = New System.Drawing.Font("Arial", 8.25!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblData2.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblData2.Location = New System.Drawing.Point(286, 225)
        Me.lblData2.Name = "lblData2"
        Me.lblData2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblData2.Size = New System.Drawing.Size(39, 14)
        Me.lblData2.TabIndex = 33
        Me.lblData2.Text = "Data"
        Me.lblData2.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblTag4
        '
        Me.lblTag4.BackColor = System.Drawing.SystemColors.Window
        Me.lblTag4.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTag4.Font = New System.Drawing.Font("Arial", 8.25!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTag4.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblTag4.Location = New System.Drawing.Point(235, 225)
        Me.lblTag4.Name = "lblTag4"
        Me.lblTag4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTag4.Size = New System.Drawing.Size(46, 14)
        Me.lblTag4.TabIndex = 32
        Me.lblTag4.Text = "Tags"
        Me.lblTag4.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblData1
        '
        Me.lblData1.BackColor = System.Drawing.SystemColors.Window
        Me.lblData1.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblData1.Font = New System.Drawing.Font("Arial", 8.25!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblData1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblData1.Location = New System.Drawing.Point(120, 225)
        Me.lblData1.Name = "lblData1"
        Me.lblData1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblData1.Size = New System.Drawing.Size(39, 14)
        Me.lblData1.TabIndex = 29
        Me.lblData1.Text = "Data"
        Me.lblData1.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblTag2
        '
        Me.lblTag2.BackColor = System.Drawing.SystemColors.Window
        Me.lblTag2.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTag2.Font = New System.Drawing.Font("Arial", 8.25!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTag2.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblTag2.Location = New System.Drawing.Point(62, 225)
        Me.lblTag2.Name = "lblTag2"
        Me.lblTag2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTag2.Size = New System.Drawing.Size(46, 14)
        Me.lblTag2.TabIndex = 28
        Me.lblTag2.Text = "Tags"
        Me.lblTag2.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblTag3
        '
        Me.lblTag3.BackColor = System.Drawing.SystemColors.Window
        Me.lblTag3.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTag3.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTag3.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblTag3.Location = New System.Drawing.Point(229, 212)
        Me.lblTag3.Name = "lblTag3"
        Me.lblTag3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTag3.Size = New System.Drawing.Size(58, 14)
        Me.lblTag3.TabIndex = 31
        Me.lblTag3.Text = "Channel"
        Me.lblTag3.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblTag1
        '
        Me.lblTag1.BackColor = System.Drawing.SystemColors.Window
        Me.lblTag1.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTag1.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTag1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblTag1.Location = New System.Drawing.Point(56, 212)
        Me.lblTag1.Name = "lblTag1"
        Me.lblTag1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTag1.Size = New System.Drawing.Size(58, 14)
        Me.lblTag1.TabIndex = 30
        Me.lblTag1.Text = "Channel"
        Me.lblTag1.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblDemoFunction
        '
        Me.lblDemoFunction.BackColor = System.Drawing.SystemColors.Window
        Me.lblDemoFunction.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDemoFunction.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDemoFunction.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblDemoFunction.Location = New System.Drawing.Point(16, 7)
        Me.lblDemoFunction.Name = "lblDemoFunction"
        Me.lblDemoFunction.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDemoFunction.Size = New System.Drawing.Size(319, 22)
        Me.lblDemoFunction.TabIndex = 0
        Me.lblDemoFunction.Text = "Demonstration of MccDaq.MccBoard.AConvertData()"
        Me.lblDemoFunction.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblInstruction
        '
        Me.lblInstruction.BackColor = System.Drawing.SystemColors.Window
        Me.lblInstruction.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblInstruction.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInstruction.ForeColor = System.Drawing.Color.Red
        Me.lblInstruction.Location = New System.Drawing.Point(12, 40)
        Me.lblInstruction.Name = "lblInstruction"
        Me.lblInstruction.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblInstruction.Size = New System.Drawing.Size(325, 76)
        Me.lblInstruction.TabIndex = 36
        Me.lblInstruction.Text = resources.GetString("lblInstruction.Text")
        Me.lblInstruction.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'frmDataDisplay
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 13)
        Me.BackColor = System.Drawing.SystemColors.Window
        Me.ClientSize = New System.Drawing.Size(349, 372)
        Me.Controls.Add(Me.lblInstruction)
        Me.Controls.Add(Me.txtHighChan)
        Me.Controls.Add(Me.cmdStopConvert)
        Me.Controls.Add(Me.cmdConvData)
        Me.Controls.Add(Me.cmdStartAcq)
        Me.Controls.Add(Me.lblMeasure)
        Me.Controls.Add(Me._lblADData_7)
        Me.Controls.Add(Me._lblShowTag_7)
        Me.Controls.Add(Me.lblChan7)
        Me.Controls.Add(Me._lblADData_3)
        Me.Controls.Add(Me._lblShowTag_3)
        Me.Controls.Add(Me.lblChan3)
        Me.Controls.Add(Me._lblADData_6)
        Me.Controls.Add(Me._lblShowTag_6)
        Me.Controls.Add(Me.lblChan6)
        Me.Controls.Add(Me._lblADData_2)
        Me.Controls.Add(Me._lblShowTag_2)
        Me.Controls.Add(Me.lblChan2)
        Me.Controls.Add(Me._lblADData_5)
        Me.Controls.Add(Me._lblShowTag_5)
        Me.Controls.Add(Me.lblChan5)
        Me.Controls.Add(Me._lblADData_1)
        Me.Controls.Add(Me._lblShowTag_1)
        Me.Controls.Add(Me.lblChan1)
        Me.Controls.Add(Me._lblADData_4)
        Me.Controls.Add(Me._lblShowTag_4)
        Me.Controls.Add(Me.lblChan4)
        Me.Controls.Add(Me._lblADData_0)
        Me.Controls.Add(Me._lblShowTag_0)
        Me.Controls.Add(Me.lblChan0)
        Me.Controls.Add(Me.lblData2)
        Me.Controls.Add(Me.lblTag4)
        Me.Controls.Add(Me.lblData1)
        Me.Controls.Add(Me.lblTag2)
        Me.Controls.Add(Me.lblTag3)
        Me.Controls.Add(Me.lblTag1)
        Me.Controls.Add(Me.lblDemoFunction)
        Me.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ForeColor = System.Drawing.Color.Blue
        Me.Location = New System.Drawing.Point(168, 103)
        Me.Name = "frmDataDisplay"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Universal Library Analog Input Scan"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Public lblADData As System.Windows.Forms.Label()
    Public WithEvents lblInstruction As System.Windows.Forms.Label
    Public lblShowTag As System.Windows.Forms.Label()

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
        '    MccDaq.ErrorHandling.StopAll   :if any error is encountered, the program will stop

        ReportError = MccDaq.ErrorReporting.PrintAll
        HandleError = MccDaq.ErrorHandling.StopAll
        ULStat = MccDaq.MccService.ErrHandling(ReportError, HandleError)
        If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then
            Stop
        End If

        lblADData = New System.Windows.Forms.Label(7) _
        {_lblADData_0, _lblADData_1, _lblADData_2, _lblADData_3, _
        _lblADData_4, _lblADData_5, _lblADData_6, _lblADData_7}

        lblShowTag = New System.Windows.Forms.Label(7) _
        {_lblShowTag_0, _lblShowTag_1, _lblShowTag_2, _lblShowTag_3, _
        _lblShowTag_4, _lblShowTag_5, _lblShowTag_6, _lblShowTag_7}

    End Sub

#End Region

End Class