'==============================================================================

' File:                         ULCT02.VB

' Library Call Demonstrated:    9513 Counter Functions
'                               Mccdaq.MccBoard.C9513Init()
'                               Mccdaq.MccBoard.C9513Config()
'                               Mccdaq.MccBoard.CLoad()
'                               Mccdaq.MccBoard.CIn()

' Purpose:                      Operate the counter.

' Demonstration:                Initializes, configures, loads and checks
'                               the counter

' Other Library Calls:          MccDaq.MccService.ErrHandling()

' Special Requirements:         Board 0 must have a 9513 Counter.
'                               Program uses internal clock to count.

'==============================================================================
Option Strict Off
Option Explicit On

Public Class frm9513Ctr
    Inherits System.Windows.Forms.Form

    Const CounterType As Integer = CTR9513    ' type of counter compatible with this sample
    Private CounterNum, NumCtrs, LastCtr As Integer

    'Create a new MccBoard object for Board 0
    Private DaqBoard As MccDaq.MccBoard = New MccDaq.MccBoard(0)
    Public WithEvents lblNoteFreqIn As System.Windows.Forms.Label

    Const ChipNum As Short = 1  ' use chip 1 for CTR05 or for first
    '                             chip on CTR10 or CTR20

    Private Sub frm9513Ctr_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim LoadValue As UInt32
        Dim RegName As MccDaq.CounterRegister
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
        Dim FOutSource As MccDaq.CounterSource
        Dim FOutDivider As Short
        Dim ULStat As MccDaq.ErrorInfo

        InitUL()
        NumCtrs = FindCountersOfType(DaqBoard, CounterType, CounterNum)
        If NumCtrs < 1 Then
            lblDemoFunction.Text = "Board " & DaqBoard.BoardNum.ToString() & _
            " has no 9513 counters."
            lblDemoFunction.ForeColor = Color.Red
        Else

            ' Initialize the board level features
            '  Parameters:
            '    ChipNum       :Chip to be initialized (1 for CTR05, 1 or 2 for CTR10)
            '    FOutDivider   :the F-Out divider (0-15)
            '    FOutSource    :the signal source for F-Out
            '    Compare1      :status of comparator 1
            '    Compare2      :status of comparator 2
            '    TimeOfDay     :time of day mode control

            FOutDivider = 0
            FOutSource = MccDaq.CounterSource.Freq4
            Compare1 = MccDaq.CompareValue.Disabled
            Compare2 = MccDaq.CompareValue.Disabled
            TimeOfDayCounting = MccDaq.TimeOfDay.Disabled

            ULStat = DaqBoard.C9513Init(ChipNum, FOutDivider, FOutSource, _
            Compare1, Compare2, TimeOfDayCounting)
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

            GateControl = MccDaq.GateControl.NoGate
            CounterEdge = MccDaq.CountEdge.PositiveEdge
            CountSource = MccDaq.CounterSource.Freq4
            SpecialGate = MccDaq.OptionState.Disabled
            Reload = MccDaq.Reload.LoadReg
            RecycleMode = MccDaq.RecycleMode.Recycle
            BCDMode = MccDaq.BCDMode.Disabled

            CountDirection = MccDaq.CountDirection.CountUp
            OutputControl = MccDaq.C9513OutputControl.AlwaysLow

            ULStat = DaqBoard.C9513Config(CounterNum, GateControl, CounterEdge, _
            CountSource, SpecialGate, Reload, RecycleMode, BCDMode, CountDirection, OutputControl)
            If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then Stop

            ' Send a starting value to the counter with MccDaq.MccBoard.CLoad()
            '  Parameters:
            '    RegName    :the counter to be loaded with the starting value
            '    LoadValue  :the starting value to place in the counter

            RegName = [Enum].Parse(GetType(MccDaq.CounterRegister), CounterNum)
            LoadValue = Convert.ToUInt32(1000)

            ULStat = DaqBoard.CLoad(RegName, LoadValue)
            If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then Stop

            lblLoadValue.Text = "Value loaded to counter " & Format(CounterNum, "0") & ":"
            lblShowLoadVal.Text = LoadValue.ToString("0")
            lblNoteFreqIn.Text = "Reading value from counter on " & _
            " board " & DaqBoard.BoardNum.ToString() & "."

            Me.tmrReadCounter.Enabled = True
        End If

    End Sub

    Private Sub tmrReadCounter_Tick(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tmrReadCounter.Tick

        Dim ULStat As MccDaq.ErrorInfo
        Dim Count As UInt16

        ' Parameters:
        '   CounterNum :the counter to be read
        '   Count    :the count value in the counter

        tmrReadCounter.Stop()

        ULStat = DaqBoard.CIn(CounterNum, Count)
        If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then Stop

        tmrReadCounter.Start()

        lblReadValue.Text = "Value read from counter " & Format(CounterNum, "0") & ":"
        lblShowReadVal.Text = Count.ToString("0")

    End Sub

    Private Sub cmdStopRead_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdStopRead.Click

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
    Public WithEvents cmdStopRead As System.Windows.Forms.Button
    Public WithEvents tmrReadCounter As System.Windows.Forms.Timer
    Public WithEvents lblShowReadVal As System.Windows.Forms.Label
    Public WithEvents lblReadValue As System.Windows.Forms.Label
    Public WithEvents lblShowLoadVal As System.Windows.Forms.Label
    Public WithEvents lblLoadValue As System.Windows.Forms.Label
    Public WithEvents lblDemoFunction As System.Windows.Forms.Label
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdStopRead = New System.Windows.Forms.Button
        Me.tmrReadCounter = New System.Windows.Forms.Timer(Me.components)
        Me.lblShowReadVal = New System.Windows.Forms.Label
        Me.lblReadValue = New System.Windows.Forms.Label
        Me.lblShowLoadVal = New System.Windows.Forms.Label
        Me.lblLoadValue = New System.Windows.Forms.Label
        Me.lblDemoFunction = New System.Windows.Forms.Label
        Me.lblNoteFreqIn = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'cmdStopRead
        '
        Me.cmdStopRead.BackColor = System.Drawing.SystemColors.Control
        Me.cmdStopRead.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdStopRead.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdStopRead.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdStopRead.Location = New System.Drawing.Point(232, 184)
        Me.cmdStopRead.Name = "cmdStopRead"
        Me.cmdStopRead.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdStopRead.Size = New System.Drawing.Size(54, 27)
        Me.cmdStopRead.TabIndex = 5
        Me.cmdStopRead.Text = "Quit"
        Me.cmdStopRead.UseVisualStyleBackColor = False
        '
        'tmrReadCounter
        '
        Me.tmrReadCounter.Interval = 500
        '
        'lblShowReadVal
        '
        Me.lblShowReadVal.BackColor = System.Drawing.SystemColors.Window
        Me.lblShowReadVal.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblShowReadVal.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShowReadVal.ForeColor = System.Drawing.Color.Blue
        Me.lblShowReadVal.Location = New System.Drawing.Point(232, 136)
        Me.lblShowReadVal.Name = "lblShowReadVal"
        Me.lblShowReadVal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblShowReadVal.Size = New System.Drawing.Size(73, 17)
        Me.lblShowReadVal.TabIndex = 2
        '
        'lblReadValue
        '
        Me.lblReadValue.BackColor = System.Drawing.SystemColors.Window
        Me.lblReadValue.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblReadValue.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReadValue.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblReadValue.Location = New System.Drawing.Point(56, 136)
        Me.lblReadValue.Name = "lblReadValue"
        Me.lblReadValue.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblReadValue.Size = New System.Drawing.Size(161, 17)
        Me.lblReadValue.TabIndex = 4
        Me.lblReadValue.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblShowLoadVal
        '
        Me.lblShowLoadVal.BackColor = System.Drawing.SystemColors.Window
        Me.lblShowLoadVal.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblShowLoadVal.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShowLoadVal.ForeColor = System.Drawing.Color.Blue
        Me.lblShowLoadVal.Location = New System.Drawing.Point(232, 104)
        Me.lblShowLoadVal.Name = "lblShowLoadVal"
        Me.lblShowLoadVal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblShowLoadVal.Size = New System.Drawing.Size(73, 17)
        Me.lblShowLoadVal.TabIndex = 1
        '
        'lblLoadValue
        '
        Me.lblLoadValue.BackColor = System.Drawing.SystemColors.Window
        Me.lblLoadValue.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblLoadValue.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLoadValue.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblLoadValue.Location = New System.Drawing.Point(56, 104)
        Me.lblLoadValue.Name = "lblLoadValue"
        Me.lblLoadValue.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblLoadValue.Size = New System.Drawing.Size(161, 17)
        Me.lblLoadValue.TabIndex = 3
        Me.lblLoadValue.TextAlign = System.Drawing.ContentAlignment.TopRight
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
        Me.lblDemoFunction.Size = New System.Drawing.Size(315, 31)
        Me.lblDemoFunction.TabIndex = 0
        Me.lblDemoFunction.Text = "Demonstration of 9513 Counter Functions."
        Me.lblDemoFunction.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblNoteFreqIn
        '
        Me.lblNoteFreqIn.BackColor = System.Drawing.SystemColors.Window
        Me.lblNoteFreqIn.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblNoteFreqIn.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNoteFreqIn.ForeColor = System.Drawing.Color.Red
        Me.lblNoteFreqIn.Location = New System.Drawing.Point(15, 51)
        Me.lblNoteFreqIn.Name = "lblNoteFreqIn"
        Me.lblNoteFreqIn.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblNoteFreqIn.Size = New System.Drawing.Size(312, 33)
        Me.lblNoteFreqIn.TabIndex = 17
        Me.lblNoteFreqIn.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'frm9513Ctr
        '
        Me.AcceptButton = Me.cmdStopRead
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 13)
        Me.BackColor = System.Drawing.SystemColors.Window
        Me.ClientSize = New System.Drawing.Size(339, 243)
        Me.Controls.Add(Me.lblNoteFreqIn)
        Me.Controls.Add(Me.cmdStopRead)
        Me.Controls.Add(Me.lblShowReadVal)
        Me.Controls.Add(Me.lblReadValue)
        Me.Controls.Add(Me.lblShowLoadVal)
        Me.Controls.Add(Me.lblLoadValue)
        Me.Controls.Add(Me.lblDemoFunction)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Location = New System.Drawing.Point(7, 103)
        Me.Name = "frm9513Ctr"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Universal Library 9513 Counter Demo"
        Me.ResumeLayout(False)

    End Sub
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

    End Sub

#End Region

End Class