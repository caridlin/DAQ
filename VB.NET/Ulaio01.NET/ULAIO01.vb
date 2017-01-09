'==============================================================================

' File:                         ULAIO01.VB

' Library Call Demonstrated:    Mccdaq.MccBoard.GetStatus()
'                               Mccdaq.MccBoard.StopBackground()

' Purpose:                      Run Simultaneous input/output functions using
'                               the same board.

' Demonstration:                Mccdaq.MccBoard.AoutScan function generates a ramp signal
'                               while Mccdaq.MccBoard.AinScan Displays the analog input on
'                               up to eight channels.

' Other Library Calls:          Mccdaq.MccBoard.AinScan()
'                               Mccdaq.MccBoard.AoutScan()
'                               Mccdaq.MccBoard.ErrHandling()

' Special Requirements:         Board 0 must support simultaneous paced input
'                               and paced output. See hardware documentation.

'==============================================================================
Option Strict Off
Option Explicit On

Public Class frmStatusDisplay

    Inherits System.Windows.Forms.Form

    'Create a new MccBoard object for Board 0
    Private DaqBoard As MccDaq.MccBoard = New MccDaq.MccBoard(0)

    Private NumAIChans, NumAOChans, HighChan As Integer
    Private ADRange, DARange As MccDaq.Range

    Const NumPoints As Integer = 10000  ' Number of data points to collect
    Const FirstPoint As Integer = 0     ' set first element in buffer to transfer to array

    Dim ADData() As UInt16              ' dimension an array to hold the input values
    Dim ADUserTerm As Short             ' flag to stop paced A/D manually
    Dim ADMemHandle As IntPtr           ' define a variable to contain the handle for
    '                                     memory allocated by Windows through 
    '                                     MccDaq.MccService.WinBufAlloc()

    Dim DAMemHandle As IntPtr           ' define a variable to contain the handle for
    Dim DAData() As UInt16              ' dimension an array to hold the output values
    '                                     memory allocated by Windows through 
    '                                     MccDaq.MccService.WinBufAlloc()
    Dim DAUserTerm As Short             ' flag to stop paced D/A manually
    Dim ADResolution, DAResolution As Integer

    Private Sub frmStatusDisplay_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim LowChan As Integer
        Dim DefaultTrig As MccDaq.TriggerType

        InitUL()

        NumAIChans = FindAnalogChansOfType(DaqBoard, ANALOGINPUT, _
            ADResolution, ADRange, LowChan, DefaultTrig)
        If Not GeneralError Then _
            NumAOChans = FindAnalogChansOfType(DaqBoard, ANALOGOUTPUT, _
            DAResolution, DARange, LowChan, DefaultTrig)
        If (NumAIChans = 0) Or (NumAOChans = 0) Then
            lblInstruction.Text = "Board " & DaqBoard.BoardNum.ToString() & _
            " does not have both input and output analog channels."
            cmdStartADBgnd.Enabled = False
            cmdStartDABgnd.Enabled = False
            txtHighChan.Enabled = False
        Else
            Dim LongVal, HalfScale As Integer
            Dim i As Integer
            Dim DACount As Integer
            Dim ULStat As MccDaq.ErrorInfo

            ReDim ADData(NumPoints - 1)
            ReDim DAData(NumPoints - 1)

            If NumAIChans > 8 Then NumAIChans = 8
            Me.txtHighChan.Text = Format(NumAIChans - 1, "0")
            ' set aside memory to hold A/D data
            ADMemHandle = MccDaq.MccService.WinBufAllocEx(NumPoints)
            If ADMemHandle = 0 Then Stop

            ' set aside memory to hold D/A data
            DACount = NumPoints
            DAMemHandle = MccDaq.MccService.WinBufAllocEx(NumPoints)
            If DAMemHandle = 0 Then Stop
            HalfScale = (2 ^ DAResolution) / 2

            ' Generate D/A ramp data to be output via MccDaq.MccBoard.AOutScan function
            For i = 0 To NumPoints - 1
                LongVal = HalfScale + CInt(((i / NumPoints) * HalfScale)) - CInt(HalfScale / 2)
                DAData(i) = Convert.ToUInt16(LongVal)
            Next i
            ULStat = MccDaq.MccService.WinArrayToBuf(DAData, DAMemHandle, FirstPoint, DACount)
            lblInstruction.Text = "Board " & DaqBoard.BoardNum.ToString() & _
                " collecting analog data on up to " & NumAIChans.ToString() & _
                " A/D channels using cbAInScan in background mode with Range set to " & _
                ADRange.ToString() & " while generating a ramp output on " & _
                "D/A channel 0 using cbAOutScan in background mode with Range set to " & _
                DARange.ToString() & "."

        End If

    End Sub

    Private Sub cmdStartADBgnd_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdStartADBgnd.Click

        Dim CurIndex, CurCount, ADRate As Integer
        Dim ADCount, LowChan As Integer
        Dim Status As Short
        Dim ULStat As MccDaq.ErrorInfo
        Dim ADOptions As MccDaq.ScanOptions

        cmdStartADBgnd.Enabled = False
        cmdStartADBgnd.Visible = False
        cmdStopADConvert.Enabled = True
        cmdStopADConvert.Visible = True
        cmdQuit.Enabled = False
        ADUserTerm = 0 ' initialize user terminate flag

        ' Collect the values with MccDaq.MccBoard.AInScan()
        '  Parameters:
        '    LowChan        :the first channel of the scan
        '    HighChan       :the last channel of the scan
        '    Count          :the total number of A/D samples to collect
        '    Rate           :sample rate
        '    Range          :the range for the board
        '    ADMemHandle    :Handle for Windows buffer to store data in
        '    Options        :data collection options

        HighChan = Integer.Parse(txtHighChan.Text) ' last channel to acquire
        If (HighChan > (NumAIChans - 1)) Then HighChan = (NumAIChans - 1)
        txtHighChan.Text = HighChan.ToString()

        Dim NumChannels As Integer = (HighChan - LowChan) + 1
        ADCount = NumPoints          ' total number of data points to collect
        ADRate = 1000 / NumChannels  ' per channel sampling rate ((samples per second) per channel)
        ADOptions = MccDaq.ScanOptions.ConvertData Or MccDaq.ScanOptions.Background
        ' return data as 12-bit values
        ' collect data in Background mode

        ULStat = DaqBoard.AInScan(LowChan, HighChan, ADCount, ADRate, ADRange, ADMemHandle, ADOptions)
        If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then Stop

        ULStat = DaqBoard.GetStatus(Status, CurCount, CurIndex, MccDaq.FunctionType.AiFunction)
        If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then Stop

        If Status = MccDaq.MccBoard.Running Then
            lblShowADStat.Text = "Running"
            lblShowADCount.Text = CurCount.ToString("0")
            lblShowADIndex.Text = CurIndex.ToString("0")
        End If

        tmrCheckStatus.Enabled = True

    End Sub

    Private Sub cmdStartDABgnd_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdStartDABgnd.Click

        Dim CurIndex, CurCount As Integer
        Dim DARate, DACount, HighDAChan, LowDAChan As Integer
        Dim Status As Short
        Dim ULStat As MccDaq.ErrorInfo
        Dim DAOptions As MccDaq.ScanOptions

        cmdStartDABgnd.Enabled = False
        cmdStartDABgnd.Visible = False
        cmdStopDAConvert.Enabled = True
        cmdStopDAConvert.Visible = True
        cmdQuit.Enabled = False
        DAUserTerm = 0 ' initialize user terminate flag

        ' Collect the values with MccDaq.MccBoard.AOutScan()
        '  Parameters:
        '    LowDAChan      :the first channel of the scan
        '    HighDAChan     :the last channel of the scan
        '    DACount        :the total number of D/A samples to output
        '    DARate         :sample rate
        '    Range          :the range for the board
        '    DAMemHandle    :Handle for Windows buffer from which data will be output
        '    DAOptions      :data output options

        LowDAChan = 0       ' first channel to output
        HighDAChan = 0      ' last channel to output

        DACount = NumPoints ' total number of data points to output
        DARate = 1000       ' output rate (samples per second per channel)
        DAOptions = MccDaq.ScanOptions.Background

        If DAMemHandle = 0 Then Stop ' check that a handle to a memory buffer exists

        ULStat = DaqBoard.AOutScan(LowDAChan, HighDAChan, DACount, DARate, _
        DARange, DAMemHandle, DAOptions)

        If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then Stop

        ULStat = DaqBoard.GetStatus(Status, CurCount, CurIndex, MccDaq.FunctionType.AoFunction)
        If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then Stop

        If Status = MccDaq.MccBoard.Running Then
            lblShowDAStat.Text = "Running"
            lblShowDACount.Text = CurCount.ToString("0")
            lblShowDAIndex.Text = CurIndex.ToString("0")
        End If

        tmrCheckStatus.Enabled = True

    End Sub

    Private Sub tmrCheckStatus_Tick(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tmrCheckStatus.Tick

        Dim DACurIndex, DACurCount As Integer
        Dim ADCurIndex, ADCurCount As Integer
        Dim j, CurrValue, i, LowChan As Integer
        Dim ADStatus, DAStatus As Short
        Dim ULStat As MccDaq.ErrorInfo

        tmrCheckStatus.Stop()

        ' This timer will check the status of the background data collection

        ' Parameters:
        '   Status     :current status of the background data collection
        '   CurCount   :current number of samples collected
        '   CurIndex   :index to the data buffer pointing to the start of the
        '                most recently collected scan

        ULStat = DaqBoard.GetStatus(ADStatus, ADCurCount, _
        ADCurIndex, MccDaq.FunctionType.AiFunction)
        If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then Stop

        lblShowADCount.Text = ADCurCount.ToString("0")
        lblShowADIndex.Text = ADCurIndex.ToString("0")

        ' Check if the background operation has finished. If it has, then
        ' transfer the data from the memory buffer set up by Windows to an
        ' array for use by Visual Basic
        ' The BACKGROUND operation must be explicitly stopped

        If ADStatus = MccDaq.MccBoard.Running And ADUserTerm = 0 Then

            lblShowADStat.Text = "Running"

            If ADCurIndex > 0 Then
                ULStat = MccDaq.MccService.WinBufToArray(ADMemHandle, ADData, ADCurIndex, HighChan - LowChan + 1)
                If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then Stop

                For i = 0 To HighChan
                    CurrValue = Convert.ToInt32(ADData(i))
                    lblADData(i).Text = CurrValue.ToString("0")
                Next i
            End If

        ElseIf ADStatus = 0 Or ADUserTerm = 1 Then
            lblShowADStat.Text = "Idle"
            ADStatus = MccDaq.MccBoard.Idle
            ULStat = DaqBoard.GetStatus(ADStatus, ADCurCount, ADCurIndex, MccDaq.FunctionType.AiFunction)
            If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then Stop
            lblShowADCount.Text = ADCurCount.ToString("0")
            lblShowADIndex.Text = ADCurIndex.ToString("0")
            If ADMemHandle = 0 Then Stop
            ULStat = MccDaq.MccService.WinBufToArray(ADMemHandle, ADData, FirstPoint, NumPoints)
            If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then Stop

            For i = 0 To HighChan
                lblADData(i).Text = ADData(i).ToString("0")
            Next i

            For j = HighChan + 1 To 7
                lblADData(j).Text = ""
            Next j

            ULStat = DaqBoard.StopBackground(MccDaq.FunctionType.AiFunction)
            If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then Stop
            cmdStartADBgnd.Enabled = True
            cmdStartADBgnd.Visible = True
            cmdStopADConvert.Enabled = False
            cmdStopADConvert.Visible = False
        End If

        '==========================================================
        ULStat = DaqBoard.GetStatus(DAStatus, DACurCount, DACurIndex, MccDaq.FunctionType.AoFunction)
        If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then Stop

        lblShowDACount.Text = DACurCount.ToString("0")
        lblShowDAIndex.Text = DACurIndex.ToString("0")

        ' Check if the background operation has finished.

        If DAStatus = MccDaq.MccBoard.Running And DAUserTerm = 0 Then
            lblShowDAStat.Text = "Running"
        ElseIf DAStatus = 0 Or DAUserTerm = 1 Then
            lblShowDAStat.Text = "Idle"
            DAStatus = MccDaq.MccBoard.Idle
            ULStat = DaqBoard.GetStatus(DAStatus, DACurCount, DACurIndex, _
            MccDaq.FunctionType.AoFunction)
            If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then Stop
            lblShowDACount.Text = DACurCount.ToString("0")
            lblShowDAIndex.Text = DACurIndex.ToString("0")

            If DAMemHandle = 0 Then Stop

            ULStat = DaqBoard.StopBackground(MccDaq.FunctionType.AoFunction)
            If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then Stop
            cmdStartDABgnd.Enabled = True
            cmdStartDABgnd.Visible = True
            cmdStopDAConvert.Enabled = False
            cmdStopDAConvert.Visible = False
        End If

        If ADStatus = MccDaq.MccBoard.Idle And DAStatus = MccDaq.MccBoard.Idle Then
            tmrCheckStatus.Enabled = False
            cmdQuit.Enabled = True
        Else
            tmrCheckStatus.Start()
        End If

    End Sub

    Private Function ULongValToInt(ByRef LongVal As Integer) As UInt16

        Dim Tmp As Integer
        Tmp = (LongVal - 32768)
        Select Case LongVal
            Case Is > 65535
                ULongValToInt = Convert.ToUInt16(-1)
            Case Is < 0
                ULongValToInt = Convert.ToUInt16(0)
            Case Else
                ULongValToInt = Convert.ToUInt16(Tmp Xor &H8000S)
        End Select

    End Function

    Private Sub cmdStopADConvert_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdStopADConvert.Click

        ADUserTerm = 1

    End Sub

    Private Sub cmdStopDAConvert_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdStopDAConvert.Click

        DAUserTerm = 1

    End Sub

    Private Sub cmdQuit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdQuit.Click
        Dim ULStat As MccDaq.ErrorInfo

        ULStat = MccDaq.MccService.WinBufFreeEx(ADMemHandle) ' Free up memory for use by
        ' other programs

        ULStat = MccDaq.MccService.WinBufFreeEx(DAMemHandle)

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
    Public WithEvents cmdStopADConvert As System.Windows.Forms.Button
    Public WithEvents cmdStartDABgnd As System.Windows.Forms.Button
    Public WithEvents cmdStopDAConvert As System.Windows.Forms.Button
    Public WithEvents txtHighChan As System.Windows.Forms.TextBox
    Public WithEvents cmdQuit As System.Windows.Forms.Button
    Public WithEvents tmrCheckStatus As System.Windows.Forms.Timer
    Public WithEvents cmdStartADBgnd As System.Windows.Forms.Button
    Public WithEvents lblCount As System.Windows.Forms.Label
    Public WithEvents Label2 As System.Windows.Forms.Label
    Public WithEvents lblInstruction As System.Windows.Forms.Label
    Public WithEvents Label4 As System.Windows.Forms.Label
    Public WithEvents Label3 As System.Windows.Forms.Label
    Public WithEvents lblShowDACount As System.Windows.Forms.Label
    Public WithEvents lblShowDAIndex As System.Windows.Forms.Label
    Public WithEvents lblShowDAStat As System.Windows.Forms.Label
    Public WithEvents Label1 As System.Windows.Forms.Label
    Public WithEvents lblShowADCount As System.Windows.Forms.Label
    Public WithEvents lblShowADIndex As System.Windows.Forms.Label
    Public WithEvents lblIndex As System.Windows.Forms.Label
    Public WithEvents lblShowADStat As System.Windows.Forms.Label
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
        Me.cmdStopADConvert = New System.Windows.Forms.Button
        Me.cmdStartDABgnd = New System.Windows.Forms.Button
        Me.cmdStopDAConvert = New System.Windows.Forms.Button
        Me.txtHighChan = New System.Windows.Forms.TextBox
        Me.cmdQuit = New System.Windows.Forms.Button
        Me.tmrCheckStatus = New System.Windows.Forms.Timer(Me.components)
        Me.cmdStartADBgnd = New System.Windows.Forms.Button
        Me.lblCount = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.lblInstruction = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.lblShowDACount = New System.Windows.Forms.Label
        Me.lblShowDAIndex = New System.Windows.Forms.Label
        Me.lblShowDAStat = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.lblShowADCount = New System.Windows.Forms.Label
        Me.lblShowADIndex = New System.Windows.Forms.Label
        Me.lblIndex = New System.Windows.Forms.Label
        Me.lblShowADStat = New System.Windows.Forms.Label
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
        Me.SuspendLayout()
        '
        'cmdStopADConvert
        '
        Me.cmdStopADConvert.BackColor = System.Drawing.SystemColors.Control
        Me.cmdStopADConvert.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdStopADConvert.Enabled = False
        Me.cmdStopADConvert.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdStopADConvert.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdStopADConvert.Location = New System.Drawing.Point(56, 126)
        Me.cmdStopADConvert.Name = "cmdStopADConvert"
        Me.cmdStopADConvert.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdStopADConvert.Size = New System.Drawing.Size(204, 27)
        Me.cmdStopADConvert.TabIndex = 33
        Me.cmdStopADConvert.Text = "Stop A/D Background Operation"
        Me.cmdStopADConvert.UseVisualStyleBackColor = False
        Me.cmdStopADConvert.Visible = False
        '
        'cmdStartDABgnd
        '
        Me.cmdStartDABgnd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdStartDABgnd.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdStartDABgnd.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdStartDABgnd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdStartDABgnd.Location = New System.Drawing.Point(352, 126)
        Me.cmdStartDABgnd.Name = "cmdStartDABgnd"
        Me.cmdStartDABgnd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdStartDABgnd.Size = New System.Drawing.Size(204, 27)
        Me.cmdStartDABgnd.TabIndex = 32
        Me.cmdStartDABgnd.Text = "Start D/A Background Operation"
        Me.cmdStartDABgnd.UseVisualStyleBackColor = False
        '
        'cmdStopDAConvert
        '
        Me.cmdStopDAConvert.BackColor = System.Drawing.SystemColors.Control
        Me.cmdStopDAConvert.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdStopDAConvert.Enabled = False
        Me.cmdStopDAConvert.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdStopDAConvert.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdStopDAConvert.Location = New System.Drawing.Point(352, 126)
        Me.cmdStopDAConvert.Name = "cmdStopDAConvert"
        Me.cmdStopDAConvert.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdStopDAConvert.Size = New System.Drawing.Size(204, 27)
        Me.cmdStopDAConvert.TabIndex = 31
        Me.cmdStopDAConvert.Text = "Stop D/A Background Operation"
        Me.cmdStopDAConvert.UseVisualStyleBackColor = False
        Me.cmdStopDAConvert.Visible = False
        '
        'txtHighChan
        '
        Me.txtHighChan.AcceptsReturn = True
        Me.txtHighChan.BackColor = System.Drawing.SystemColors.Window
        Me.txtHighChan.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtHighChan.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtHighChan.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtHighChan.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtHighChan.Location = New System.Drawing.Point(350, 164)
        Me.txtHighChan.MaxLength = 0
        Me.txtHighChan.Name = "txtHighChan"
        Me.txtHighChan.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtHighChan.Size = New System.Drawing.Size(33, 20)
        Me.txtHighChan.TabIndex = 25
        Me.txtHighChan.Text = "3"
        '
        'cmdQuit
        '
        Me.cmdQuit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdQuit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdQuit.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdQuit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdQuit.Location = New System.Drawing.Point(288, 317)
        Me.cmdQuit.Name = "cmdQuit"
        Me.cmdQuit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdQuit.Size = New System.Drawing.Size(52, 26)
        Me.cmdQuit.TabIndex = 18
        Me.cmdQuit.Text = "Quit"
        Me.cmdQuit.UseVisualStyleBackColor = False
        '
        'tmrCheckStatus
        '
        Me.tmrCheckStatus.Interval = 200
        '
        'cmdStartADBgnd
        '
        Me.cmdStartADBgnd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdStartADBgnd.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdStartADBgnd.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdStartADBgnd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdStartADBgnd.Location = New System.Drawing.Point(56, 126)
        Me.cmdStartADBgnd.Name = "cmdStartADBgnd"
        Me.cmdStartADBgnd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdStartADBgnd.Size = New System.Drawing.Size(204, 27)
        Me.cmdStartADBgnd.TabIndex = 17
        Me.cmdStartADBgnd.Text = "Start A/D Background Operation"
        Me.cmdStartADBgnd.UseVisualStyleBackColor = False
        '
        'lblCount
        '
        Me.lblCount.BackColor = System.Drawing.SystemColors.Window
        Me.lblCount.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCount.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCount.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblCount.Location = New System.Drawing.Point(70, 284)
        Me.lblCount.Name = "lblCount"
        Me.lblCount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCount.Size = New System.Drawing.Size(124, 14)
        Me.lblCount.TabIndex = 36
        Me.lblCount.Text = "Current A/D Count:"
        Me.lblCount.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.SystemColors.Window
        Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label2.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Label2.Location = New System.Drawing.Point(360, 284)
        Me.Label2.Name = "Label2"
        Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label2.Size = New System.Drawing.Size(135, 14)
        Me.Label2.TabIndex = 35
        Me.Label2.Text = "Current D/A Count:"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblInstruction
        '
        Me.lblInstruction.BackColor = System.Drawing.SystemColors.Window
        Me.lblInstruction.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblInstruction.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInstruction.ForeColor = System.Drawing.Color.Red
        Me.lblInstruction.Location = New System.Drawing.Point(56, 31)
        Me.lblInstruction.Name = "lblInstruction"
        Me.lblInstruction.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblInstruction.Size = New System.Drawing.Size(500, 77)
        Me.lblInstruction.TabIndex = 34
        Me.lblInstruction.Text = "Board 0 must support simultaneous paced input and paced output. For more informat" & _
            "ion, see hardware documentation."
        Me.lblInstruction.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'Label4
        '
        Me.Label4.BackColor = System.Drawing.SystemColors.Window
        Me.Label4.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label4.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Label4.Location = New System.Drawing.Point(307, 246)
        Me.Label4.Name = "Label4"
        Me.Label4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label4.Size = New System.Drawing.Size(188, 14)
        Me.Label4.TabIndex = 30
        Me.Label4.Text = "Status of D/A Background:"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.SystemColors.Window
        Me.Label3.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label3.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Label3.Location = New System.Drawing.Point(360, 265)
        Me.Label3.Name = "Label3"
        Me.Label3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label3.Size = New System.Drawing.Size(135, 19)
        Me.Label3.TabIndex = 29
        Me.Label3.Text = "Current D/A Index:"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblShowDACount
        '
        Me.lblShowDACount.BackColor = System.Drawing.SystemColors.Window
        Me.lblShowDACount.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblShowDACount.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShowDACount.ForeColor = System.Drawing.Color.Blue
        Me.lblShowDACount.Location = New System.Drawing.Point(504, 286)
        Me.lblShowDACount.Name = "lblShowDACount"
        Me.lblShowDACount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblShowDACount.Size = New System.Drawing.Size(65, 14)
        Me.lblShowDACount.TabIndex = 28
        '
        'lblShowDAIndex
        '
        Me.lblShowDAIndex.BackColor = System.Drawing.SystemColors.Window
        Me.lblShowDAIndex.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblShowDAIndex.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShowDAIndex.ForeColor = System.Drawing.Color.Blue
        Me.lblShowDAIndex.Location = New System.Drawing.Point(496, 266)
        Me.lblShowDAIndex.Name = "lblShowDAIndex"
        Me.lblShowDAIndex.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblShowDAIndex.Size = New System.Drawing.Size(71, 13)
        Me.lblShowDAIndex.TabIndex = 27
        '
        'lblShowDAStat
        '
        Me.lblShowDAStat.BackColor = System.Drawing.SystemColors.Window
        Me.lblShowDAStat.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblShowDAStat.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShowDAStat.ForeColor = System.Drawing.Color.Blue
        Me.lblShowDAStat.Location = New System.Drawing.Point(496, 246)
        Me.lblShowDAStat.Name = "lblShowDAStat"
        Me.lblShowDAStat.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblShowDAStat.Size = New System.Drawing.Size(66, 14)
        Me.lblShowDAStat.TabIndex = 26
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.SystemColors.Window
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Label1.Location = New System.Drawing.Point(202, 166)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(144, 14)
        Me.Label1.TabIndex = 24
        Me.Label1.Text = "Measure Channels 0 to"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblShowADCount
        '
        Me.lblShowADCount.BackColor = System.Drawing.SystemColors.Window
        Me.lblShowADCount.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblShowADCount.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShowADCount.ForeColor = System.Drawing.Color.Blue
        Me.lblShowADCount.Location = New System.Drawing.Point(206, 286)
        Me.lblShowADCount.Name = "lblShowADCount"
        Me.lblShowADCount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblShowADCount.Size = New System.Drawing.Size(73, 14)
        Me.lblShowADCount.TabIndex = 23
        '
        'lblShowADIndex
        '
        Me.lblShowADIndex.BackColor = System.Drawing.SystemColors.Window
        Me.lblShowADIndex.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblShowADIndex.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShowADIndex.ForeColor = System.Drawing.Color.Blue
        Me.lblShowADIndex.Location = New System.Drawing.Point(206, 265)
        Me.lblShowADIndex.Name = "lblShowADIndex"
        Me.lblShowADIndex.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblShowADIndex.Size = New System.Drawing.Size(67, 14)
        Me.lblShowADIndex.TabIndex = 22
        '
        'lblIndex
        '
        Me.lblIndex.BackColor = System.Drawing.SystemColors.Window
        Me.lblIndex.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblIndex.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblIndex.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblIndex.Location = New System.Drawing.Point(67, 265)
        Me.lblIndex.Name = "lblIndex"
        Me.lblIndex.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblIndex.Size = New System.Drawing.Size(127, 14)
        Me.lblIndex.TabIndex = 21
        Me.lblIndex.Text = "Current A/D Index:"
        Me.lblIndex.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblShowADStat
        '
        Me.lblShowADStat.BackColor = System.Drawing.SystemColors.Window
        Me.lblShowADStat.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblShowADStat.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShowADStat.ForeColor = System.Drawing.Color.Blue
        Me.lblShowADStat.Location = New System.Drawing.Point(200, 246)
        Me.lblShowADStat.Name = "lblShowADStat"
        Me.lblShowADStat.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblShowADStat.Size = New System.Drawing.Size(81, 22)
        Me.lblShowADStat.TabIndex = 20
        '
        'lblStatus
        '
        Me.lblStatus.BackColor = System.Drawing.SystemColors.Window
        Me.lblStatus.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblStatus.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStatus.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblStatus.Location = New System.Drawing.Point(30, 246)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblStatus.Size = New System.Drawing.Size(164, 14)
        Me.lblStatus.TabIndex = 19
        Me.lblStatus.Text = "Status of A/D Background:"
        Me.lblStatus.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblADData_7
        '
        Me._lblADData_7.BackColor = System.Drawing.SystemColors.Window
        Me._lblADData_7.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblADData_7.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblADData_7.ForeColor = System.Drawing.Color.Blue
        Me._lblADData_7.Location = New System.Drawing.Point(535, 217)
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
        Me.lblChan7.Location = New System.Drawing.Point(463, 217)
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
        Me._lblADData_3.Location = New System.Drawing.Point(232, 217)
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
        Me.lblChan3.Location = New System.Drawing.Point(160, 217)
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
        Me._lblADData_6.Location = New System.Drawing.Point(535, 198)
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
        Me.lblChan6.Location = New System.Drawing.Point(463, 198)
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
        Me._lblADData_2.Location = New System.Drawing.Point(232, 198)
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
        Me.lblChan2.Location = New System.Drawing.Point(160, 198)
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
        Me._lblADData_5.Location = New System.Drawing.Point(383, 217)
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
        Me.lblChan5.Location = New System.Drawing.Point(311, 217)
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
        Me._lblADData_1.Location = New System.Drawing.Point(82, 217)
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
        Me.lblChan1.Location = New System.Drawing.Point(10, 217)
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
        Me._lblADData_4.Location = New System.Drawing.Point(383, 198)
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
        Me.lblChan4.Location = New System.Drawing.Point(311, 198)
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
        Me._lblADData_0.Location = New System.Drawing.Point(82, 198)
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
        Me.lblChan0.Location = New System.Drawing.Point(10, 198)
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
        Me.lblDemoFunction.Location = New System.Drawing.Point(12, 9)
        Me.lblDemoFunction.Name = "lblDemoFunction"
        Me.lblDemoFunction.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDemoFunction.Size = New System.Drawing.Size(587, 19)
        Me.lblDemoFunction.TabIndex = 0
        Me.lblDemoFunction.Text = "Demonstration of Simultaneous MccDaq.MccBoard.AInScan() and MccDaq.MccBoard.AoutS" & _
            "can "
        Me.lblDemoFunction.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'frmStatusDisplay
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 13)
        Me.BackColor = System.Drawing.SystemColors.Window
        Me.ClientSize = New System.Drawing.Size(611, 352)
        Me.Controls.Add(Me.cmdStartDABgnd)
        Me.Controls.Add(Me.cmdStopDAConvert)
        Me.Controls.Add(Me.txtHighChan)
        Me.Controls.Add(Me.cmdQuit)
        Me.Controls.Add(Me.cmdStartADBgnd)
        Me.Controls.Add(Me.lblCount)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.lblInstruction)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.lblShowDACount)
        Me.Controls.Add(Me.lblShowDAIndex)
        Me.Controls.Add(Me.lblShowDAStat)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.lblShowADCount)
        Me.Controls.Add(Me.lblShowADIndex)
        Me.Controls.Add(Me.lblIndex)
        Me.Controls.Add(Me.lblShowADStat)
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
        Me.Controls.Add(Me.cmdStopADConvert)
        Me.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Location = New System.Drawing.Point(188, 108)
        Me.Name = "frmStatusDisplay"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Simultaneous AInScan() and AoutScan() "
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

        ULStat = MccDaq.MccService.ErrHandling(MccDaq.ErrorReporting.PrintAll, _
        MccDaq.ErrorHandling.StopAll)
        If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then
            Stop
        End If

        lblADData = New System.Windows.Forms.Label(8) {}
        Me.lblADData.SetValue(Me._lblADData_7, 7)
        Me.lblADData.SetValue(Me._lblADData_6, 6)
        Me.lblADData.SetValue(Me._lblADData_5, 5)
        Me.lblADData.SetValue(Me._lblADData_4, 4)
        Me.lblADData.SetValue(Me._lblADData_3, 3)
        Me.lblADData.SetValue(Me._lblADData_2, 2)
        Me.lblADData.SetValue(Me._lblADData_1, 1)
        Me.lblADData.SetValue(Me._lblADData_0, 0)

    End Sub

    Public lblADData As System.Windows.Forms.Label()

#End Region

End Class