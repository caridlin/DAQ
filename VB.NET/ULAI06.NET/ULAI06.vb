'==============================================================================

' File:                         ULAI06.VB

' Library Call Demonstrated:    Mccdaq.MccBoard.AInScan(), continuous background mode

' Purpose:                      Scans a range of A/D Input Channels continuously
'                               in the background and stores the data in an array.

' Demonstration:                Continuously collects data on up to eight channels.

' Other Library Calls:          Mccdaq.MccBoard.GetStatus()
'                               Mccdaq.MccBoard.StopBackground()
'                               Mccdaq.MccBoard.ErrHandling()

' Special Requirements:         Board 0 must have an A/D converter.
'                               Analog signals on up to eight input channels.

'==============================================================================
Option Strict Off
Option Explicit On

Friend Class frmStatusDisplay

    Inherits System.Windows.Forms.Form

    ' Create a new MccBoard object for Board 0
    Private DaqBoard As MccDaq.MccBoard = New MccDaq.MccBoard(0)

    Private Range As MccDaq.Range
    Private ADResolution, NumAIChans As Integer
    Private HighChan, LowChan, MaxChan As Integer

    Const NumPoints As Integer = 31744  ' Number of data points to collect
    '                                     For some devices, this number must be a
    '                                     multiple of the packet size. 31744 is a
    '                                     multiple of the most common packet sizes,
    '                                     so it satifies this requirement for most devices.

    Dim ADData() As UInt16              ' dimension an array to hold the input values
    Dim ADData32() As System.UInt32     ' dimension an array to hold the high resolution input values

    ' define a variable to contain the handle for memory allocated by Windows through
    ' MccDaq.MccService.WinBufAlloc() or MccDaq.MccService.WinBufAlloc32()
    Dim MemHandle As IntPtr

    Private Sub frmStatusDisplay_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim DefaultTrig As Long

        InitUL()

        ' determine the number of analog channels and their capabilities
        Dim ChannelType As Integer = ANALOGINPUT
        NumAIChans = FindAnalogChansOfType(DaqBoard, ChannelType, _
            ADResolution, Range, LowChan, DefaultTrig)

        If (NumAIChans = 0) Then
            lblInstruction.Text = "Board " & DaqBoard.BoardNum.ToString() & _
            " does not have analog input channels."
            cmdStartBgnd.Enabled = False
            txtHighChan.Enabled = False
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
            If (NumAIChans > 8) Then NumAIChans = 8 'limit to 8 for display
            MaxChan = LowChan + NumAIChans - 1
            lblInstruction.Text = "Board " & DaqBoard.BoardNum.ToString() & _
                " collecting analog data on up to " & NumAIChans.ToString() & _
                " channels using AInScan with Range set to " & Range.ToString() & "."
        End If

    End Sub

    Private Sub cmdStartBgnd_Click(ByVal eventSender As System.Object, _
    ByVal eventArgs As System.EventArgs) Handles cmdStartBgnd.Click

        Dim CurIndex As Integer
        Dim CurCount As Integer
        Dim Status As Short
        Dim ULStat As MccDaq.ErrorInfo
        Dim Options As MccDaq.ScanOptions
        Dim Rate As Integer
        Dim Count As Integer
        Dim ValidChan As Boolean

        cmdStartBgnd.Enabled = False
        cmdStartBgnd.Visible = False
        cmdStopConvert.Enabled = True
        cmdStopConvert.Visible = True
        cmdQuit.Enabled = False

        ' Collect the values by calling MccDaq.MccBoard.AInScan
        '  Parameters:
        '    LowChan    :the first channel of the scan
        '    HighChan   :the last channel of the scan
        '    Count      :the total number of A/D samples to collect
        '    Rate       :sample rate
        '    Range      :the range for the board
        '    MemHandle  :Handle for Windows buffer to store data in
        '    Options    :data collection options

        ValidChan = Integer.TryParse(txtHighChan.Text, HighChan)
        If ValidChan Then
            If (HighChan > MaxChan) Then HighChan = MaxChan
            txtHighChan.Text = Format(HighChan, "0")
        End If

        Count = NumPoints     ' total number of data points to collect

        ' per channel sampling rate ((samples per second) per channel)
        Rate = 1000 / ((HighChan - LowChan) + 1)

        Options = MccDaq.ScanOptions.Background Or MccDaq.ScanOptions.Continuous

        ULStat = DaqBoard.AInScan(LowChan, HighChan, Count, Rate, Range, MemHandle, Options)

        If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then Stop

        ULStat = DaqBoard.GetStatus(Status, CurCount, CurIndex, MccDaq.FunctionType.AiFunction)
        If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then Stop

        If Status = MccDaq.MccBoard.Running Then
            lblShowStat.Text = "Running"
            lblShowCount.Text = CurCount.ToString("D")
            lblShowIndex.Text = CurIndex.ToString("D")
        End If

        tmrCheckStatus.Enabled = True

    End Sub

    Private Sub tmrCheckStatus_Tick(ByVal eventSender As System.Object, _
    ByVal eventArgs As System.EventArgs) Handles tmrCheckStatus.Tick

        Dim j As Integer
        Dim i As Integer
        Dim FirstPoint, NumChans As Integer
        Dim ULStat As MccDaq.ErrorInfo
        Dim CurIndex As Integer
        Dim CurCount As Integer
        Dim Status As Short

        tmrCheckStatus.Stop()

        ' Check the status of the background data collection

        ' Parameters:
        '   Status      :current status of the background data collection
        '   CurCount    :current number of samples collected
        '   CurIndex    :index to the data buffer pointing to the start of the
        '                most recently collected scan
        '   FunctionType: A/D operation (MccDaq.FunctionType.AiFunction)

        ULStat = DaqBoard.GetStatus(Status, CurCount, CurIndex, MccDaq.FunctionType.AiFunction)
        If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then Stop

        lblShowCount.Text = CurCount.ToString("D")
        lblShowIndex.Text = CurIndex.ToString("D")

        ' Check the background operation.
        ' transfer the data from the memory buffer set up by Windows to an
        ' array for use by Visual Basic
        ' The background operation must be explicitly stopped

        ULStat = DaqBoard.GetStatus(Status, CurCount, CurIndex, MccDaq.FunctionType.AiFunction)
        If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then Stop

        tmrCheckStatus.Start()

        If Status = MccDaq.MccBoard.Running Then lblShowStat.Text = "Running"
        lblShowCount.Text = CurCount.ToString("D")
        lblShowIndex.Text = CurIndex.ToString("D")
        NumChans = (HighChan - LowChan) + 1
        If CurIndex > HighChan Then
            If MemHandle = 0 Then Stop
            FirstPoint = CurIndex 'start of latest channel scan in MemHandle buffer

            If ADResolution > 16 Then
                ULStat = MccDaq.MccService.WinBufToArray32(MemHandle, ADData32, FirstPoint, NumChans)
                If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then Stop

                For i = 0 To HighChan
                    lblADData(i).Text = ADData32(i).ToString("D")
                Next i
            Else
                ULStat = MccDaq.MccService.WinBufToArray(MemHandle, ADData, FirstPoint, NumChans)
                If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then Stop

                For i = 0 To HighChan
                    lblADData(i).Text = ADData(i).ToString("D")
                Next i

            End If

            For j = HighChan + 1 To 7
                lblADData(j).Text = ""
            Next j

        End If

    End Sub

    Private Sub cmdStopConvert_Click(ByVal eventSender As System.Object, _
    ByVal eventArgs As System.EventArgs) Handles cmdStopConvert.Click

        Dim CurIndex As Integer
        Dim CurCount As Integer
        Dim Status As Short
        Dim ULStat As MccDaq.ErrorInfo

        ULStat = DaqBoard.StopBackground(MccDaq.FunctionType.AiFunction)
        If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then Stop

        cmdStartBgnd.Enabled = True : cmdStartBgnd.Visible = True
        cmdStopConvert.Enabled = False : cmdStopConvert.Visible = False
        cmdQuit.Enabled = True
        tmrCheckStatus.Enabled = False

        ULStat = DaqBoard.GetStatus(Status, CurCount, CurIndex, MccDaq.FunctionType.AiFunction)
        If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then Stop

        If Status = MccDaq.MccBoard.Idle Then lblShowStat.Text = "Idle"
        lblShowCount.Text = CurCount.ToString("D")
        lblShowIndex.Text = CurIndex.ToString("D")

    End Sub

    Private Sub cmdQuit_Click(ByVal eventSender As System.Object, _
    ByVal eventArgs As System.EventArgs) Handles cmdQuit.Click

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

    Public WithEvents lblInstruction As System.Windows.Forms.Label
    Public lblADData As System.Windows.Forms.Label()

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer
    Public ToolTip1 As System.Windows.Forms.ToolTip
    Public WithEvents txtHighChan As System.Windows.Forms.TextBox
    Public WithEvents cmdQuit As System.Windows.Forms.Button
    Public WithEvents tmrCheckStatus As System.Windows.Forms.Timer
    Public WithEvents cmdStartBgnd As System.Windows.Forms.Button
    Public WithEvents cmdStopConvert As System.Windows.Forms.Button
    Public WithEvents Label1 As System.Windows.Forms.Label
    Public WithEvents lblShowCount As System.Windows.Forms.Label
    Public WithEvents lblCount As System.Windows.Forms.Label
    Public WithEvents lblShowIndex As System.Windows.Forms.Label
    Public WithEvents lblIndex As System.Windows.Forms.Label
    Public WithEvents lblShowStat As System.Windows.Forms.Label
    Public WithEvents lblStatus As System.Windows.Forms.Label
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
        Me.cmdQuit = New System.Windows.Forms.Button
        Me.tmrCheckStatus = New System.Windows.Forms.Timer(Me.components)
        Me.cmdStartBgnd = New System.Windows.Forms.Button
        Me.cmdStopConvert = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.lblShowCount = New System.Windows.Forms.Label
        Me.lblCount = New System.Windows.Forms.Label
        Me.lblShowIndex = New System.Windows.Forms.Label
        Me.lblIndex = New System.Windows.Forms.Label
        Me.lblShowStat = New System.Windows.Forms.Label
        Me.lblStatus = New System.Windows.Forms.Label
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
        Me.txtHighChan.Location = New System.Drawing.Point(210, 144)
        Me.txtHighChan.MaxLength = 0
        Me.txtHighChan.Name = "txtHighChan"
        Me.txtHighChan.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtHighChan.Size = New System.Drawing.Size(25, 20)
        Me.txtHighChan.TabIndex = 27
        Me.txtHighChan.Text = "0"
        Me.txtHighChan.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'cmdQuit
        '
        Me.cmdQuit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdQuit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdQuit.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdQuit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdQuit.Location = New System.Drawing.Point(273, 302)
        Me.cmdQuit.Name = "cmdQuit"
        Me.cmdQuit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdQuit.Size = New System.Drawing.Size(65, 26)
        Me.cmdQuit.TabIndex = 19
        Me.cmdQuit.Text = "Quit"
        Me.cmdQuit.UseVisualStyleBackColor = False
        '
        'tmrCheckStatus
        '
        Me.tmrCheckStatus.Interval = 200
        '
        'cmdStartBgnd
        '
        Me.cmdStartBgnd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdStartBgnd.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdStartBgnd.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdStartBgnd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdStartBgnd.Location = New System.Drawing.Point(84, 112)
        Me.cmdStartBgnd.Name = "cmdStartBgnd"
        Me.cmdStartBgnd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdStartBgnd.Size = New System.Drawing.Size(180, 27)
        Me.cmdStartBgnd.TabIndex = 18
        Me.cmdStartBgnd.Text = "Start Background Operation"
        Me.cmdStartBgnd.UseVisualStyleBackColor = False
        '
        'cmdStopConvert
        '
        Me.cmdStopConvert.BackColor = System.Drawing.SystemColors.Control
        Me.cmdStopConvert.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdStopConvert.Enabled = False
        Me.cmdStopConvert.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdStopConvert.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdStopConvert.Location = New System.Drawing.Point(84, 112)
        Me.cmdStopConvert.Name = "cmdStopConvert"
        Me.cmdStopConvert.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdStopConvert.Size = New System.Drawing.Size(180, 27)
        Me.cmdStopConvert.TabIndex = 17
        Me.cmdStopConvert.Text = "Stop Background Operation"
        Me.cmdStopConvert.UseVisualStyleBackColor = False
        Me.cmdStopConvert.Visible = False
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.SystemColors.Window
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Label1.Location = New System.Drawing.Point(70, 146)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(137, 17)
        Me.Label1.TabIndex = 26
        Me.Label1.Text = "Measure Channels 0 to"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblShowCount
        '
        Me.lblShowCount.BackColor = System.Drawing.SystemColors.Window
        Me.lblShowCount.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblShowCount.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShowCount.ForeColor = System.Drawing.Color.Blue
        Me.lblShowCount.Location = New System.Drawing.Point(199, 302)
        Me.lblShowCount.Name = "lblShowCount"
        Me.lblShowCount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblShowCount.Size = New System.Drawing.Size(58, 14)
        Me.lblShowCount.TabIndex = 25
        '
        'lblCount
        '
        Me.lblCount.BackColor = System.Drawing.SystemColors.Window
        Me.lblCount.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCount.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCount.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblCount.Location = New System.Drawing.Point(84, 302)
        Me.lblCount.Name = "lblCount"
        Me.lblCount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCount.Size = New System.Drawing.Size(103, 14)
        Me.lblCount.TabIndex = 23
        Me.lblCount.Text = "Current Count:"
        Me.lblCount.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblShowIndex
        '
        Me.lblShowIndex.BackColor = System.Drawing.SystemColors.Window
        Me.lblShowIndex.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblShowIndex.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShowIndex.ForeColor = System.Drawing.Color.Blue
        Me.lblShowIndex.Location = New System.Drawing.Point(199, 283)
        Me.lblShowIndex.Name = "lblShowIndex"
        Me.lblShowIndex.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblShowIndex.Size = New System.Drawing.Size(52, 14)
        Me.lblShowIndex.TabIndex = 24
        '
        'lblIndex
        '
        Me.lblIndex.BackColor = System.Drawing.SystemColors.Window
        Me.lblIndex.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblIndex.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblIndex.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblIndex.Location = New System.Drawing.Point(84, 283)
        Me.lblIndex.Name = "lblIndex"
        Me.lblIndex.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblIndex.Size = New System.Drawing.Size(103, 14)
        Me.lblIndex.TabIndex = 22
        Me.lblIndex.Text = "Current Index:"
        Me.lblIndex.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblShowStat
        '
        Me.lblShowStat.BackColor = System.Drawing.SystemColors.Window
        Me.lblShowStat.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblShowStat.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShowStat.ForeColor = System.Drawing.Color.Blue
        Me.lblShowStat.Location = New System.Drawing.Point(241, 256)
        Me.lblShowStat.Name = "lblShowStat"
        Me.lblShowStat.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblShowStat.Size = New System.Drawing.Size(65, 17)
        Me.lblShowStat.TabIndex = 21
        '
        'lblStatus
        '
        Me.lblStatus.BackColor = System.Drawing.SystemColors.Window
        Me.lblStatus.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblStatus.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStatus.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblStatus.Location = New System.Drawing.Point(9, 256)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblStatus.Size = New System.Drawing.Size(217, 17)
        Me.lblStatus.TabIndex = 20
        Me.lblStatus.Text = "Status of Background Operation:"
        Me.lblStatus.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblADData_7
        '
        Me._lblADData_7.BackColor = System.Drawing.SystemColors.Window
        Me._lblADData_7.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblADData_7.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblADData_7.ForeColor = System.Drawing.Color.Blue
        Me._lblADData_7.Location = New System.Drawing.Point(265, 230)
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
        Me.lblChan7.Location = New System.Drawing.Point(193, 230)
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
        Me._lblADData_3.Location = New System.Drawing.Point(97, 230)
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
        Me.lblChan3.Location = New System.Drawing.Point(25, 230)
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
        Me._lblADData_6.Location = New System.Drawing.Point(265, 211)
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
        Me.lblChan6.Location = New System.Drawing.Point(193, 211)
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
        Me._lblADData_2.Location = New System.Drawing.Point(97, 211)
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
        Me.lblChan2.Location = New System.Drawing.Point(25, 211)
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
        Me._lblADData_5.Location = New System.Drawing.Point(265, 192)
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
        Me.lblChan5.Location = New System.Drawing.Point(193, 192)
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
        Me._lblADData_1.Location = New System.Drawing.Point(97, 192)
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
        Me.lblChan1.Location = New System.Drawing.Point(25, 192)
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
        Me._lblADData_4.Location = New System.Drawing.Point(265, 173)
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
        Me.lblChan4.Location = New System.Drawing.Point(193, 173)
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
        Me._lblADData_0.Location = New System.Drawing.Point(97, 173)
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
        Me.lblChan0.Location = New System.Drawing.Point(25, 173)
        Me.lblChan0.Name = "lblChan0"
        Me.lblChan0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblChan0.Size = New System.Drawing.Size(65, 17)
        Me.lblChan0.TabIndex = 1
        Me.lblChan0.Text = "Channel 0:"
        '
        'lblDemoFunction
        '
        Me.lblDemoFunction.BackColor = System.Drawing.SystemColors.Window
        Me.lblDemoFunction.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDemoFunction.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDemoFunction.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblDemoFunction.Location = New System.Drawing.Point(28, 8)
        Me.lblDemoFunction.Name = "lblDemoFunction"
        Me.lblDemoFunction.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDemoFunction.Size = New System.Drawing.Size(284, 33)
        Me.lblDemoFunction.TabIndex = 0
        Me.lblDemoFunction.Text = "Demonstration of Mccdaq.MccBoard.AInScan() in Continuous Background mode"
        Me.lblDemoFunction.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblInstruction
        '
        Me.lblInstruction.BackColor = System.Drawing.SystemColors.Window
        Me.lblInstruction.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblInstruction.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInstruction.ForeColor = System.Drawing.Color.Red
        Me.lblInstruction.Location = New System.Drawing.Point(39, 49)
        Me.lblInstruction.Name = "lblInstruction"
        Me.lblInstruction.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblInstruction.Size = New System.Drawing.Size(273, 60)
        Me.lblInstruction.TabIndex = 29
        Me.lblInstruction.Text = "Board 0 must have analog inputs that support paced acquisition."
        Me.lblInstruction.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'frmStatusDisplay
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 13)
        Me.BackColor = System.Drawing.SystemColors.Window
        Me.ClientSize = New System.Drawing.Size(350, 337)
        Me.Controls.Add(Me.lblInstruction)
        Me.Controls.Add(Me.txtHighChan)
        Me.Controls.Add(Me.cmdQuit)
        Me.Controls.Add(Me.cmdStartBgnd)
        Me.Controls.Add(Me.cmdStopConvert)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.lblShowCount)
        Me.Controls.Add(Me.lblCount)
        Me.Controls.Add(Me.lblShowIndex)
        Me.Controls.Add(Me.lblIndex)
        Me.Controls.Add(Me.lblShowStat)
        Me.Controls.Add(Me.lblStatus)
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
        Me.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ForeColor = System.Drawing.Color.Blue
        Me.Location = New System.Drawing.Point(168, 104)
        Me.Name = "frmStatusDisplay"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Universal Library Analog Input Scan"
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
        '    MccDaq.ErrorReporting.PrintAll :all warnings and errors encountered will be printed
        '    MccDaq.ErrorHandling.StopAll   :if any error is encountered, the program will stop

        ReportError = MccDaq.ErrorReporting.PrintAll
        HandleError = MccDaq.ErrorHandling.StopAll
        ULStat = MccDaq.MccService.ErrHandling(ReportError, HandleError)
        If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then
            Stop
        End If

        ' Note: Any change to label names requires a change to the corresponding array element below
        lblADData = (New System.Windows.Forms.Label() {Me._lblADData_0, _
        Me._lblADData_1, Me._lblADData_2, Me._lblADData_3, Me._lblADData_4, _
        Me._lblADData_5, Me._lblADData_6, Me._lblADData_7})

    End Sub

#End Region

End Class