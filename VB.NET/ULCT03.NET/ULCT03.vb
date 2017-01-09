'==============================================================================

' File:                         ULCT03.VB

' Library Call Demonstrated:    9513 Counter Functions
'                               Mccdaq.MccBoard.C9513Config()
'                               Mccdaq.MccBoard.CStoreOnInt()

' Purpose:                      Operate the counter

' Demonstration:                Sets up 2 counters to store values in
'                               response to an interrupt
'

' Other Library Calls:          Mccdaq.MccBoard.C9513Init()
'                               Mccdaq.MccBoard.CLoad()
'                               Mccdaq.MccBoard.StopBackground()
'                               MccDaq.MccService.ErrHandling()

' Special Requirements:         Board 0 must have a 9513 counter.
'                               IRQ ENABLE must be tied low.
'                               User must a TTL signal to IRQ INPUT.

'==============================================================================
Option Strict Off
Option Explicit On

Public Class frm9513Int

    Inherits System.Windows.Forms.Form

    Const CounterType As Integer = CTR9513    ' type of counter compatible with this sample
    Private CounterNum, NumCtrs, LastCtr As Integer

    'Create a new MccBoard object for Board 0
    Private DaqBoard As MccDaq.MccBoard = New MccDaq.MccBoard(0)

    Const ChipNum As Short = 1      ' use chip 1 for CTR05 or for first
    '                                 chip on CTR10 or CTR20
    Const IntCount As Integer = 100 ' the windows buffer pointed to by MemHandle will 
    '                                 hold enough data for IntCount interrupts

    Dim DataBuffer() As UInt16      ' array to hold latest readings from each of the counters
    Dim CntrControl() As MccDaq.CounterControl ' array to control whether or not each counter 
    '                                            is enabled
    Dim MemHandle As IntPtr                    ' handle to windows data buffer that is large 
    '                                            enough to hold IntCount readings from each 
    '                                            of the counters
    Dim FirstPoint As Integer

    Private Sub frm9513Int_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        InitUL()
        NumCtrs = FindCountersOfType(DaqBoard, CounterType, CounterNum)
        If NumCtrs < 1 Then
            lblInstruction.Text = "Board " & DaqBoard.BoardNum.ToString() & _
            " has no 9513 counters."
            cmdStartInt.Enabled = False
        Else
            Dim RegName As MccDaq.CounterRegister
            Dim LoadValue As UInt32
            Dim OutputControl As MccDaq.C9513OutputControl
            Dim CountDirection As MccDaq.CountDirection
            Dim BCDMode As MccDaq.BCDMode
            Dim RecycleMode As MccDaq.RecycleMode
            Dim Reload As MccDaq.Reload
            Dim SpecialGate As MccDaq.OptionState
            Dim CountSource As MccDaq.CounterSource
            Dim CounterEdge As MccDaq.CountEdge
            Dim GateControl As MccDaq.GateControl
            Dim TimeOfDayCounting As MccDaq.TimeOfDay
            Dim Compare2 As MccDaq.CompareValue
            Dim Compare1 As MccDaq.CompareValue
            Dim Source As MccDaq.CounterSource
            Dim FOutDivider As Integer
            Dim ULStat As MccDaq.ErrorInfo
            ReDim DataBuffer(NumCtrs)
            ReDim CntrControl(NumCtrs)

            MemHandle = MccDaq.MccService.WinBufAllocEx(IntCount * NumCtrs) ' set aside memory to hold data
            If MemHandle = 0 Then Stop ' we're allocating enough for
            ' MaxNumCntrs in case actual NumCntrs
            ' had not been updated
            ' Initialize the board level features
            '  Parameters:
            '    ChipNum    :chip to be initialized (1 for CTR5, 1 or 2 for CTR10)
            '    FOutDivider:the F-Out divider (0-15)
            '    Source     :the signal source for F-Out
            '    Compare1   :status of comparator 1
            '    Compare2   :status of comparator 2
            '    TimeOfDayCounting  :time of day mode control

            FOutDivider = 10 ' sets up OSC OUT for 10Hz signal which can
            Source = MccDaq.CounterSource.Freq5 ' be used as interrupt source for this example
            Compare1 = MccDaq.CompareValue.Disabled
            Compare2 = MccDaq.CompareValue.Disabled
            TimeOfDayCounting = MccDaq.TimeOfDay.Disabled

            ULStat = DaqBoard.C9513Init(ChipNum, FOutDivider, Source, Compare1, Compare2, TimeOfDayCounting)
            If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then Stop

            ' Set the configurable operations of the counter
            '  Parameters:
            '    CounterNum     :the counter to be configured (1 to 5)
            '    GateControl    :gate control value
            '    CounterEdge    :which edge to count
            '    CountSource    :signal source
            '    SpecialGate    :status of special gate
            '    Reload         :method of reloading
            '    RecyleMode     :recyle mode
            '    BCDMode        :counting mode, Binary or BCD
            '    CountDirection :direction for the counting operation (COUNTUP or COUNTDOWN)
            '    OutputControl  :output signal type and level

            ' Initialize variables for the first of two counters

            CounterNum = 1 ' number of counter used
            GateControl = MccDaq.GateControl.NoGate
            CounterEdge = MccDaq.CountEdge.PositiveEdge
            CountSource = MccDaq.CounterSource.Freq3
            SpecialGate = MccDaq.OptionState.Disabled
            Reload = MccDaq.Reload.LoadReg
            RecycleMode = MccDaq.RecycleMode.Recycle
            BCDMode = MccDaq.BCDMode.Disabled
            CountDirection = MccDaq.CountDirection.CountUp
            OutputControl = MccDaq.C9513OutputControl.AlwaysLow

            ULStat = DaqBoard.C9513Config(CounterNum, GateControl, CounterEdge, CountSource, SpecialGate, Reload, RecycleMode, BCDMode, CountDirection, OutputControl)
            If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then Stop

            ' Initialize variables for the second counter

            CounterNum = 2 ' number of counter used
            ULStat = DaqBoard.C9513Config(CounterNum, GateControl, CounterEdge, CountSource, SpecialGate, Reload, RecycleMode, BCDMode, CountDirection, OutputControl)
            If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then Stop

            ' Load the 2 counters with starting values of zero with MccDaq.MccBoard.CLoad()
            '  Parameters:
            '    RegName    :the counter to be loaded with the starting value
            '    LoadValue  :the starting value to place in the counter

            LoadValue = Convert.ToUInt32(0)
            RegName = MccDaq.CounterRegister.LoadReg1 ' name of register in counter 1

            ULStat = DaqBoard.CLoad(RegName, LoadValue)
            If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then Stop

            RegName = MccDaq.CounterRegister.LoadReg2 ' name of register in counter 2

            ULStat = DaqBoard.CLoad(RegName, LoadValue)
            If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then Stop
            lblInstruction.Text = "Showing count at the time of each " & _
            "interrupt at IRQ input on board " & DaqBoard.BoardNum.ToString() & "."
        End If

    End Sub

    Private Sub cmdStartInt_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdStartInt.Click

        Dim ULStat As MccDaq.ErrorInfo
        Dim I As Short

        cmdStartInt.Enabled = False
        cmdStartInt.Visible = False
        cmdStopRead.Enabled = True
        cmdStopRead.Visible = True

        ' set the counters to store their values upon an interrupt
        '  Parameters:
        '    IntCount      :maximum number of interrupts
        '    CntrControl() :array which indicates the channels to be read
        '    DataBuffer()  :array that receives the count values for enabled
        '                    channels each time an interrupt occur

        ' set all channels to MccDaq.CounterControl.Disabled  and init DataBuffer
        For I = 0 To NumCtrs - 1
            CntrControl(I) = MccDaq.CounterControl.Disabled
            DataBuffer(I) = Convert.ToUInt16(0)
        Next I

        ' enable the channels to be monitored
        CntrControl(0) = MccDaq.CounterControl.Enabled
        CntrControl(1) = MccDaq.CounterControl.Enabled

        ULStat = DaqBoard.CStoreOnInt(IntCount, CntrControl, MemHandle)
        If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then Stop

        tmrReadStatus.Enabled = True
        FirstPoint = 0

    End Sub

    Private Sub tmrReadStatus_Tick(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tmrReadStatus.Tick

        Dim RealCount As UInt16
        Dim IntStatus As String
        Dim I As Short
        Dim ULStat As MccDaq.ErrorInfo
        Dim CurIndex As Integer
        Dim CurCount As Integer
        Dim Status As Short

        tmrReadStatus.Stop()

        ULStat = DaqBoard.GetStatus(Status, CurCount, CurIndex, MccDaq.FunctionType.CtrFunction)
        If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then Stop

        FirstPoint = 0
        'This line is NOT necessary for 32-bit library.
        CurIndex = (CurCount Mod IntCount) - 1

        'The calculation below requires that NumCntrs accurately reflects the number
        '  of counters onboard whether or not they are enabled or active.
        If CurIndex > 0 Then
            FirstPoint = NumCtrs * CurIndex
        End If

        ULStat = MccDaq.MccService.WinBufToArray(MemHandle, DataBuffer, FirstPoint, NumCtrs)
        If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then Stop

        For I = 0 To 4
            If CntrControl(I) = MccDaq.CounterControl.Disabled Then
                IntStatus = "DISABLED"
            Else
                IntStatus = "ENABLED "
            End If

            ' convert type int to type long

            RealCount = DataBuffer(I)

            lblCounterNum(I).Text = (I + 1).ToString("0")
            lblIntStatus(I).Text = IntStatus
            lblCount(I).Text = RealCount.ToString("0")

        Next I


        lblShowTotal.Text = CurCount.ToString("0")

        tmrReadStatus.Start()

    End Sub

    Private Sub cmdStopRead_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdStopRead.Click

        Dim ULStat As MccDaq.ErrorInfo

        ' the BACKGROUND operation must be explicitly stopped

        ' Parameters:
        '   FunctionType:counter operation (MccDaq.FunctionType.CtrFunction)

        ULStat = DaqBoard.StopBackground(MccDaq.FunctionType.CtrFunction)

        If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then Stop

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
    Public WithEvents cmdStartInt As System.Windows.Forms.Button
    Public WithEvents cmdStopRead As System.Windows.Forms.Button
    Public WithEvents tmrReadStatus As System.Windows.Forms.Timer
    Public WithEvents lblShowTotal As System.Windows.Forms.Label
    Public WithEvents lblIntTotal As System.Windows.Forms.Label
    Public WithEvents _lblCount_4 As System.Windows.Forms.Label
    Public WithEvents _lblIntStatus_4 As System.Windows.Forms.Label
    Public WithEvents _lblCounterNum_4 As System.Windows.Forms.Label
    Public WithEvents _lblCount_3 As System.Windows.Forms.Label
    Public WithEvents _lblIntStatus_3 As System.Windows.Forms.Label
    Public WithEvents _lblCounterNum_3 As System.Windows.Forms.Label
    Public WithEvents _lblCount_2 As System.Windows.Forms.Label
    Public WithEvents _lblIntStatus_2 As System.Windows.Forms.Label
    Public WithEvents _lblCounterNum_2 As System.Windows.Forms.Label
    Public WithEvents _lblCount_1 As System.Windows.Forms.Label
    Public WithEvents _lblIntStatus_1 As System.Windows.Forms.Label
    Public WithEvents _lblCounterNum_1 As System.Windows.Forms.Label
    Public WithEvents _lblCount_0 As System.Windows.Forms.Label
    Public WithEvents _lblIntStatus_0 As System.Windows.Forms.Label
    Public WithEvents _lblCounterNum_0 As System.Windows.Forms.Label
    Public WithEvents lblData As System.Windows.Forms.Label
    Public WithEvents lblStatCol As System.Windows.Forms.Label
    Public WithEvents lblCountCol As System.Windows.Forms.Label
    Public WithEvents lblDemoFunction As System.Windows.Forms.Label
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdStartInt = New System.Windows.Forms.Button
        Me.cmdStopRead = New System.Windows.Forms.Button
        Me.tmrReadStatus = New System.Windows.Forms.Timer(Me.components)
        Me.lblShowTotal = New System.Windows.Forms.Label
        Me.lblIntTotal = New System.Windows.Forms.Label
        Me._lblCount_4 = New System.Windows.Forms.Label
        Me._lblIntStatus_4 = New System.Windows.Forms.Label
        Me._lblCounterNum_4 = New System.Windows.Forms.Label
        Me._lblCount_3 = New System.Windows.Forms.Label
        Me._lblIntStatus_3 = New System.Windows.Forms.Label
        Me._lblCounterNum_3 = New System.Windows.Forms.Label
        Me._lblCount_2 = New System.Windows.Forms.Label
        Me._lblIntStatus_2 = New System.Windows.Forms.Label
        Me._lblCounterNum_2 = New System.Windows.Forms.Label
        Me._lblCount_1 = New System.Windows.Forms.Label
        Me._lblIntStatus_1 = New System.Windows.Forms.Label
        Me._lblCounterNum_1 = New System.Windows.Forms.Label
        Me._lblCount_0 = New System.Windows.Forms.Label
        Me._lblIntStatus_0 = New System.Windows.Forms.Label
        Me._lblCounterNum_0 = New System.Windows.Forms.Label
        Me.lblData = New System.Windows.Forms.Label
        Me.lblStatCol = New System.Windows.Forms.Label
        Me.lblCountCol = New System.Windows.Forms.Label
        Me.lblDemoFunction = New System.Windows.Forms.Label
        Me.lblInstruction = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'cmdStartInt
        '
        Me.cmdStartInt.BackColor = System.Drawing.SystemColors.Control
        Me.cmdStartInt.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdStartInt.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdStartInt.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdStartInt.Location = New System.Drawing.Point(272, 266)
        Me.cmdStartInt.Name = "cmdStartInt"
        Me.cmdStartInt.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdStartInt.Size = New System.Drawing.Size(57, 25)
        Me.cmdStartInt.TabIndex = 4
        Me.cmdStartInt.Text = "Start"
        Me.cmdStartInt.UseVisualStyleBackColor = False
        '
        'cmdStopRead
        '
        Me.cmdStopRead.BackColor = System.Drawing.SystemColors.Control
        Me.cmdStopRead.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdStopRead.Enabled = False
        Me.cmdStopRead.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdStopRead.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdStopRead.Location = New System.Drawing.Point(272, 266)
        Me.cmdStopRead.Name = "cmdStopRead"
        Me.cmdStopRead.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdStopRead.Size = New System.Drawing.Size(57, 25)
        Me.cmdStopRead.TabIndex = 3
        Me.cmdStopRead.Text = "Quit"
        Me.cmdStopRead.UseVisualStyleBackColor = False
        Me.cmdStopRead.Visible = False
        '
        'tmrReadStatus
        '
        Me.tmrReadStatus.Interval = 200
        '
        'lblShowTotal
        '
        Me.lblShowTotal.BackColor = System.Drawing.SystemColors.Window
        Me.lblShowTotal.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblShowTotal.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShowTotal.ForeColor = System.Drawing.Color.Blue
        Me.lblShowTotal.Location = New System.Drawing.Point(168, 274)
        Me.lblShowTotal.Name = "lblShowTotal"
        Me.lblShowTotal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblShowTotal.Size = New System.Drawing.Size(65, 17)
        Me.lblShowTotal.TabIndex = 18
        '
        'lblIntTotal
        '
        Me.lblIntTotal.BackColor = System.Drawing.SystemColors.Window
        Me.lblIntTotal.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblIntTotal.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblIntTotal.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblIntTotal.Location = New System.Drawing.Point(56, 274)
        Me.lblIntTotal.Name = "lblIntTotal"
        Me.lblIntTotal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblIntTotal.Size = New System.Drawing.Size(105, 17)
        Me.lblIntTotal.TabIndex = 22
        Me.lblIntTotal.Text = "Total Interrupts:"
        Me.lblIntTotal.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblCount_4
        '
        Me._lblCount_4.BackColor = System.Drawing.SystemColors.Window
        Me._lblCount_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblCount_4.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblCount_4.ForeColor = System.Drawing.Color.Blue
        Me._lblCount_4.Location = New System.Drawing.Point(216, 235)
        Me._lblCount_4.Name = "_lblCount_4"
        Me._lblCount_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblCount_4.Size = New System.Drawing.Size(65, 17)
        Me._lblCount_4.TabIndex = 17
        Me._lblCount_4.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblIntStatus_4
        '
        Me._lblIntStatus_4.BackColor = System.Drawing.SystemColors.Window
        Me._lblIntStatus_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblIntStatus_4.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblIntStatus_4.ForeColor = System.Drawing.Color.Blue
        Me._lblIntStatus_4.Location = New System.Drawing.Point(120, 235)
        Me._lblIntStatus_4.Name = "_lblIntStatus_4"
        Me._lblIntStatus_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblIntStatus_4.Size = New System.Drawing.Size(73, 17)
        Me._lblIntStatus_4.TabIndex = 12
        '
        '_lblCounterNum_4
        '
        Me._lblCounterNum_4.BackColor = System.Drawing.SystemColors.Window
        Me._lblCounterNum_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblCounterNum_4.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblCounterNum_4.ForeColor = System.Drawing.Color.Black
        Me._lblCounterNum_4.Location = New System.Drawing.Point(72, 235)
        Me._lblCounterNum_4.Name = "_lblCounterNum_4"
        Me._lblCounterNum_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblCounterNum_4.Size = New System.Drawing.Size(25, 17)
        Me._lblCounterNum_4.TabIndex = 8
        Me._lblCounterNum_4.Text = "5"
        Me._lblCounterNum_4.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblCount_3
        '
        Me._lblCount_3.BackColor = System.Drawing.SystemColors.Window
        Me._lblCount_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblCount_3.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblCount_3.ForeColor = System.Drawing.Color.Blue
        Me._lblCount_3.Location = New System.Drawing.Point(216, 211)
        Me._lblCount_3.Name = "_lblCount_3"
        Me._lblCount_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblCount_3.Size = New System.Drawing.Size(65, 17)
        Me._lblCount_3.TabIndex = 16
        Me._lblCount_3.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblIntStatus_3
        '
        Me._lblIntStatus_3.BackColor = System.Drawing.SystemColors.Window
        Me._lblIntStatus_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblIntStatus_3.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblIntStatus_3.ForeColor = System.Drawing.Color.Blue
        Me._lblIntStatus_3.Location = New System.Drawing.Point(120, 211)
        Me._lblIntStatus_3.Name = "_lblIntStatus_3"
        Me._lblIntStatus_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblIntStatus_3.Size = New System.Drawing.Size(73, 17)
        Me._lblIntStatus_3.TabIndex = 11
        '
        '_lblCounterNum_3
        '
        Me._lblCounterNum_3.BackColor = System.Drawing.SystemColors.Window
        Me._lblCounterNum_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblCounterNum_3.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblCounterNum_3.ForeColor = System.Drawing.Color.Black
        Me._lblCounterNum_3.Location = New System.Drawing.Point(72, 211)
        Me._lblCounterNum_3.Name = "_lblCounterNum_3"
        Me._lblCounterNum_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblCounterNum_3.Size = New System.Drawing.Size(25, 17)
        Me._lblCounterNum_3.TabIndex = 7
        Me._lblCounterNum_3.Text = "4"
        Me._lblCounterNum_3.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblCount_2
        '
        Me._lblCount_2.BackColor = System.Drawing.SystemColors.Window
        Me._lblCount_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblCount_2.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblCount_2.ForeColor = System.Drawing.Color.Blue
        Me._lblCount_2.Location = New System.Drawing.Point(216, 187)
        Me._lblCount_2.Name = "_lblCount_2"
        Me._lblCount_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblCount_2.Size = New System.Drawing.Size(65, 17)
        Me._lblCount_2.TabIndex = 15
        Me._lblCount_2.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblIntStatus_2
        '
        Me._lblIntStatus_2.BackColor = System.Drawing.SystemColors.Window
        Me._lblIntStatus_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblIntStatus_2.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblIntStatus_2.ForeColor = System.Drawing.Color.Blue
        Me._lblIntStatus_2.Location = New System.Drawing.Point(120, 187)
        Me._lblIntStatus_2.Name = "_lblIntStatus_2"
        Me._lblIntStatus_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblIntStatus_2.Size = New System.Drawing.Size(73, 17)
        Me._lblIntStatus_2.TabIndex = 10
        '
        '_lblCounterNum_2
        '
        Me._lblCounterNum_2.BackColor = System.Drawing.SystemColors.Window
        Me._lblCounterNum_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblCounterNum_2.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblCounterNum_2.ForeColor = System.Drawing.Color.Black
        Me._lblCounterNum_2.Location = New System.Drawing.Point(72, 187)
        Me._lblCounterNum_2.Name = "_lblCounterNum_2"
        Me._lblCounterNum_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblCounterNum_2.Size = New System.Drawing.Size(25, 17)
        Me._lblCounterNum_2.TabIndex = 6
        Me._lblCounterNum_2.Text = "3"
        Me._lblCounterNum_2.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblCount_1
        '
        Me._lblCount_1.BackColor = System.Drawing.SystemColors.Window
        Me._lblCount_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblCount_1.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblCount_1.ForeColor = System.Drawing.Color.Blue
        Me._lblCount_1.Location = New System.Drawing.Point(216, 163)
        Me._lblCount_1.Name = "_lblCount_1"
        Me._lblCount_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblCount_1.Size = New System.Drawing.Size(65, 17)
        Me._lblCount_1.TabIndex = 14
        Me._lblCount_1.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblIntStatus_1
        '
        Me._lblIntStatus_1.BackColor = System.Drawing.SystemColors.Window
        Me._lblIntStatus_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblIntStatus_1.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblIntStatus_1.ForeColor = System.Drawing.Color.Blue
        Me._lblIntStatus_1.Location = New System.Drawing.Point(120, 163)
        Me._lblIntStatus_1.Name = "_lblIntStatus_1"
        Me._lblIntStatus_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblIntStatus_1.Size = New System.Drawing.Size(73, 17)
        Me._lblIntStatus_1.TabIndex = 9
        '
        '_lblCounterNum_1
        '
        Me._lblCounterNum_1.BackColor = System.Drawing.SystemColors.Window
        Me._lblCounterNum_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblCounterNum_1.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblCounterNum_1.ForeColor = System.Drawing.Color.Black
        Me._lblCounterNum_1.Location = New System.Drawing.Point(72, 163)
        Me._lblCounterNum_1.Name = "_lblCounterNum_1"
        Me._lblCounterNum_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblCounterNum_1.Size = New System.Drawing.Size(25, 17)
        Me._lblCounterNum_1.TabIndex = 5
        Me._lblCounterNum_1.Text = "2"
        Me._lblCounterNum_1.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblCount_0
        '
        Me._lblCount_0.BackColor = System.Drawing.SystemColors.Window
        Me._lblCount_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblCount_0.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblCount_0.ForeColor = System.Drawing.Color.Blue
        Me._lblCount_0.Location = New System.Drawing.Point(216, 139)
        Me._lblCount_0.Name = "_lblCount_0"
        Me._lblCount_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblCount_0.Size = New System.Drawing.Size(65, 17)
        Me._lblCount_0.TabIndex = 13
        Me._lblCount_0.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblIntStatus_0
        '
        Me._lblIntStatus_0.BackColor = System.Drawing.SystemColors.Window
        Me._lblIntStatus_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblIntStatus_0.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblIntStatus_0.ForeColor = System.Drawing.Color.Blue
        Me._lblIntStatus_0.Location = New System.Drawing.Point(120, 139)
        Me._lblIntStatus_0.Name = "_lblIntStatus_0"
        Me._lblIntStatus_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblIntStatus_0.Size = New System.Drawing.Size(73, 17)
        Me._lblIntStatus_0.TabIndex = 2
        '
        '_lblCounterNum_0
        '
        Me._lblCounterNum_0.BackColor = System.Drawing.SystemColors.Window
        Me._lblCounterNum_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblCounterNum_0.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblCounterNum_0.ForeColor = System.Drawing.Color.Black
        Me._lblCounterNum_0.Location = New System.Drawing.Point(72, 139)
        Me._lblCounterNum_0.Name = "_lblCounterNum_0"
        Me._lblCounterNum_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblCounterNum_0.Size = New System.Drawing.Size(25, 17)
        Me._lblCounterNum_0.TabIndex = 1
        Me._lblCounterNum_0.Text = "1"
        Me._lblCounterNum_0.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblData
        '
        Me.lblData.BackColor = System.Drawing.SystemColors.Window
        Me.lblData.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblData.Font = New System.Drawing.Font("Arial", 8.25!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblData.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblData.Location = New System.Drawing.Point(208, 107)
        Me.lblData.Name = "lblData"
        Me.lblData.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblData.Size = New System.Drawing.Size(81, 17)
        Me.lblData.TabIndex = 21
        Me.lblData.Text = "Data Value"
        Me.lblData.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblStatCol
        '
        Me.lblStatCol.BackColor = System.Drawing.SystemColors.Window
        Me.lblStatCol.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblStatCol.Font = New System.Drawing.Font("Arial", 8.25!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStatCol.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblStatCol.Location = New System.Drawing.Point(128, 107)
        Me.lblStatCol.Name = "lblStatCol"
        Me.lblStatCol.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblStatCol.Size = New System.Drawing.Size(57, 17)
        Me.lblStatCol.TabIndex = 20
        Me.lblStatCol.Text = "Status"
        Me.lblStatCol.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblCountCol
        '
        Me.lblCountCol.BackColor = System.Drawing.SystemColors.Window
        Me.lblCountCol.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCountCol.Font = New System.Drawing.Font("Arial", 8.25!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCountCol.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblCountCol.Location = New System.Drawing.Point(56, 107)
        Me.lblCountCol.Name = "lblCountCol"
        Me.lblCountCol.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCountCol.Size = New System.Drawing.Size(57, 17)
        Me.lblCountCol.TabIndex = 19
        Me.lblCountCol.Text = "Counter"
        Me.lblCountCol.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblDemoFunction
        '
        Me.lblDemoFunction.BackColor = System.Drawing.SystemColors.Window
        Me.lblDemoFunction.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDemoFunction.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDemoFunction.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblDemoFunction.Location = New System.Drawing.Point(12, 16)
        Me.lblDemoFunction.Name = "lblDemoFunction"
        Me.lblDemoFunction.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDemoFunction.Size = New System.Drawing.Size(326, 19)
        Me.lblDemoFunction.TabIndex = 0
        Me.lblDemoFunction.Text = "Demonstration of 9513 Counter using Interrupts"
        Me.lblDemoFunction.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblInstruction
        '
        Me.lblInstruction.BackColor = System.Drawing.SystemColors.Window
        Me.lblInstruction.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblInstruction.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInstruction.ForeColor = System.Drawing.Color.Red
        Me.lblInstruction.Location = New System.Drawing.Point(24, 39)
        Me.lblInstruction.Name = "lblInstruction"
        Me.lblInstruction.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblInstruction.Size = New System.Drawing.Size(296, 55)
        Me.lblInstruction.TabIndex = 23
        Me.lblInstruction.Text = "User must supply a TTL signal to IRQ INPUT.  Also, IRQ ENABLE (if present) must b" & _
            "e tied low."
        Me.lblInstruction.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'frm9513Int
        '
        Me.AcceptButton = Me.cmdStopRead
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 13)
        Me.BackColor = System.Drawing.SystemColors.Window
        Me.ClientSize = New System.Drawing.Size(350, 316)
        Me.Controls.Add(Me.lblInstruction)
        Me.Controls.Add(Me.cmdStartInt)
        Me.Controls.Add(Me.cmdStopRead)
        Me.Controls.Add(Me.lblShowTotal)
        Me.Controls.Add(Me.lblIntTotal)
        Me.Controls.Add(Me._lblCount_4)
        Me.Controls.Add(Me._lblIntStatus_4)
        Me.Controls.Add(Me._lblCounterNum_4)
        Me.Controls.Add(Me._lblCount_3)
        Me.Controls.Add(Me._lblIntStatus_3)
        Me.Controls.Add(Me._lblCounterNum_3)
        Me.Controls.Add(Me._lblCount_2)
        Me.Controls.Add(Me._lblIntStatus_2)
        Me.Controls.Add(Me._lblCounterNum_2)
        Me.Controls.Add(Me._lblCount_1)
        Me.Controls.Add(Me._lblIntStatus_1)
        Me.Controls.Add(Me._lblCounterNum_1)
        Me.Controls.Add(Me._lblCount_0)
        Me.Controls.Add(Me._lblIntStatus_0)
        Me.Controls.Add(Me._lblCounterNum_0)
        Me.Controls.Add(Me.lblData)
        Me.Controls.Add(Me.lblStatCol)
        Me.Controls.Add(Me.lblCountCol)
        Me.Controls.Add(Me.lblDemoFunction)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Location = New System.Drawing.Point(7, 103)
        Me.Name = "frm9513Int"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Universal Library 9513 Counter Demo"
        Me.ResumeLayout(False)

    End Sub

    Public lblCount As System.Windows.Forms.Label()
    Public lblCounterNum As System.Windows.Forms.Label()
    Public lblIntStatus As System.Windows.Forms.Label()
    Public WithEvents lblInstruction As System.Windows.Forms.Label

#End Region

#Region "Universal Library Initialization - Expand this region to change error handling, etc."

    Private Sub InitUL()

        Dim ULStat As MccDaq.ErrorInfo

        ULStat = MccDaq.MccService.DeclareRevision(MccDaq.MccService.CurrentRevNum)

        ' Initiate error handling
        '  activating error handling will trap errors like
        '  bad channel numbers and non-configured conditions.
        '  Parameters:
        '    MccDaq.ErrorReporting.PrintAll :all warnings and errors encountered will be printed
        '    MccDaq.ErrorHandling.StopAll   :if any error is encountered, the program will stop


        ULStat = MccDaq.MccService.ErrHandling(MccDaq.ErrorReporting.PrintAll, MccDaq.ErrorHandling.StopAll)
        If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then
            Stop
        End If

        Me.lblCount = New System.Windows.Forms.Label(5) {}
        Me.lblCount.SetValue(_lblCount_4, 4)
        Me.lblCount.SetValue(_lblCount_3, 3)
        Me.lblCount.SetValue(_lblCount_2, 2)
        Me.lblCount.SetValue(_lblCount_1, 1)
        Me.lblCount.SetValue(_lblCount_0, 0)

        Me.lblCounterNum = New System.Windows.Forms.Label(5) {}
        Me.lblCounterNum.SetValue(_lblCounterNum_4, 4)
        Me.lblCounterNum.SetValue(_lblCounterNum_3, 3)
        Me.lblCounterNum.SetValue(_lblCounterNum_2, 2)
        Me.lblCounterNum.SetValue(_lblCounterNum_1, 1)
        Me.lblCounterNum.SetValue(_lblCounterNum_0, 0)

        Me.lblIntStatus = New System.Windows.Forms.Label(5) {}
        Me.lblIntStatus.SetValue(_lblIntStatus_4, 4)
        Me.lblIntStatus.SetValue(_lblIntStatus_3, 3)
        Me.lblIntStatus.SetValue(_lblIntStatus_2, 2)
        Me.lblIntStatus.SetValue(_lblIntStatus_1, 1)
        Me.lblIntStatus.SetValue(_lblIntStatus_0, 0)

    End Sub

#End Region

End Class