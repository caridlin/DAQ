'==============================================================================

' File:                         ULCT05.VB

' Library Call Demonstrated:    8536 Counter Functions
'                               Mccdaq.MccBoard.C8536Init()
'                               Mccdaq.MccBoard.C8536Config()
'                               Mccdaq.MccBoard.CLoad()
'                               Mccdaq.MccBoard.CIn()

' Purpose:                      Operate the counter.

' Demonstration:                Configures, loads and reads the counter.

' Other Library Calls:          MccDaq.MccService.ErrHandling()

' Special Requirements:         Board 0 must have an 8536 Counter.
'                               There must be a TTL freq. at cntr. 1 input.
'                               Gate line must be tied high.

'==============================================================================
Option Strict Off
Option Explicit On

Friend Class frm8536Count
    Inherits System.Windows.Forms.Form

    Const CounterType As Integer = CTR8536    ' type of counter compatible with this sample
    Private CounterNum, NumCtrs As Integer

    'Create a new MccBoard object for Board 0
    Private DaqBoard As MccDaq.MccBoard = New MccDaq.MccBoard(0)

    Private Sub frm8536Count_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim ULStat As MccDaq.ErrorInfo
        Dim TrigType As MccDaq.C8536TriggerType
        Dim RecycleMode As MccDaq.RecycleMode
        Dim OutputControl As MccDaq.C8536OutputControl
        Dim Ctr1Output As MccDaq.CtrlOutput
        Dim ChipNum As Short

        InitUL()
        NumCtrs = FindCountersOfType(DaqBoard, CounterType, CounterNum)
        If NumCtrs < 1 Then
            Me.lblNoteFreqIn.Text = "Board " & DaqBoard.BoardNum.ToString() & _
                " has no 8536 counters."
            Me.cmdStart.Enabled = False
        Else
            'Init the counter for desired operation
            '  Parameters:
            '     ChipNum     :the chip to be setup
            '     Ctr1Output  :how the counter output is used

            ChipNum = 1
            Ctr1Output = MccDaq.CtrlOutput.NotLinked

            ULStat = DaqBoard.C8536Init(ChipNum, Ctr1Output)
            If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then Stop

            'Configure the counter for desired operation
            '  Parameters:
            '     CounterNum    :which counter
            '     OutputControl :which counter output signal is used
            '     RecycleMode   :reload at 0 ?
            '     TrigType      :trigger type

            OutputControl = MccDaq.C8536OutputControl.ToggleOnTc
            RecycleMode = MccDaq.RecycleMode.Recycle
            TrigType = MccDaq.C8536TriggerType.HWStartTrig

            ULStat = DaqBoard.C8536Config(CounterNum, OutputControl, RecycleMode, TrigType)
            If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then Stop
            lblNoteFreqIn.Text = "NOTE: There must be a TTL frequency at " & _
            "counter 1 input on board " & DaqBoard.BoardNum.ToString() & "."
        End If

    End Sub

    Private Sub cmdStart_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdStart.Click

        Dim ULStat As MccDaq.ErrorInfo
        Dim RegName As MccDaq.CounterRegister
        Dim LoadValue As UInt32

        'Send a starting value to the counter with MccDaq.MccBoard.CLoad()
        '  Parameters:
        '     RegName    :the counter to be loading with the starting value
        '     LoadValue  :the starting value to place in the counter

        LoadValue = Convert.ToUInt32(1000)
        RegName = [Enum].Parse(GetType(MccDaq.CounterRegister), CounterNum)

        ULStat = DaqBoard.CLoad(RegName, LoadValue)
        If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then Stop

        'Start a timer to read counter at 500ms interval

        lblCountLoaded.Text = "The value loaded into LOADREG1 was: "
        lblShowLoadVal.Text = LoadValue.ToString("0")

        tmrReadCounter.Enabled = True

    End Sub

    Private Sub tmrReadCounter_Tick(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tmrReadCounter.Tick

        Dim ULStat As MccDaq.ErrorInfo
        Dim Count As UInt16

        tmrReadCounter.Stop()

        'use this timer to keep checking the counter value with MccDaq.MccBoard.CIn()

        '  Parameters:
        '     CounterNum :the counter to be setup
        '     Count      :the count value in the counter

        ULStat = DaqBoard.CIn(CounterNum, Count)
        If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then Stop

        lblCount.Text = Count.ToString("0")
        
        tmrReadCounter.Start()

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
    Public WithEvents cmdQuit As System.Windows.Forms.Button
    Public WithEvents cmdStart As System.Windows.Forms.Button
    Public WithEvents tmrReadCounter As System.Windows.Forms.Timer
    Public WithEvents lblCount As System.Windows.Forms.Label
    Public WithEvents Label1 As System.Windows.Forms.Label
    Public WithEvents lblShowLoadVal As System.Windows.Forms.Label
    Public WithEvents lblCountLoaded As System.Windows.Forms.Label
    Public WithEvents lblNoteFreqIn As System.Windows.Forms.Label
    Public WithEvents lblDemoFunction As System.Windows.Forms.Label
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdQuit = New System.Windows.Forms.Button
        Me.cmdStart = New System.Windows.Forms.Button
        Me.tmrReadCounter = New System.Windows.Forms.Timer(Me.components)
        Me.lblCount = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.lblShowLoadVal = New System.Windows.Forms.Label
        Me.lblCountLoaded = New System.Windows.Forms.Label
        Me.lblNoteFreqIn = New System.Windows.Forms.Label
        Me.lblDemoFunction = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'cmdQuit
        '
        Me.cmdQuit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdQuit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdQuit.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdQuit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdQuit.Location = New System.Drawing.Point(264, 200)
        Me.cmdQuit.Name = "cmdQuit"
        Me.cmdQuit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdQuit.Size = New System.Drawing.Size(65, 33)
        Me.cmdQuit.TabIndex = 2
        Me.cmdQuit.Text = "Quit"
        Me.cmdQuit.UseVisualStyleBackColor = False
        '
        'cmdStart
        '
        Me.cmdStart.BackColor = System.Drawing.SystemColors.Control
        Me.cmdStart.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdStart.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdStart.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdStart.Location = New System.Drawing.Point(176, 200)
        Me.cmdStart.Name = "cmdStart"
        Me.cmdStart.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdStart.Size = New System.Drawing.Size(65, 33)
        Me.cmdStart.TabIndex = 0
        Me.cmdStart.Text = "Start"
        Me.cmdStart.UseVisualStyleBackColor = False
        '
        'tmrReadCounter
        '
        Me.tmrReadCounter.Interval = 500
        '
        'lblCount
        '
        Me.lblCount.BackColor = System.Drawing.SystemColors.Window
        Me.lblCount.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCount.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCount.ForeColor = System.Drawing.Color.Blue
        Me.lblCount.Location = New System.Drawing.Point(256, 152)
        Me.lblCount.Name = "lblCount"
        Me.lblCount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCount.Size = New System.Drawing.Size(65, 17)
        Me.lblCount.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.SystemColors.Window
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Label1.Location = New System.Drawing.Point(8, 152)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(241, 17)
        Me.Label1.TabIndex = 7
        Me.Label1.Text = "Value read from Counter 1: "
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblShowLoadVal
        '
        Me.lblShowLoadVal.BackColor = System.Drawing.SystemColors.Window
        Me.lblShowLoadVal.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblShowLoadVal.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShowLoadVal.ForeColor = System.Drawing.Color.Blue
        Me.lblShowLoadVal.Location = New System.Drawing.Point(256, 120)
        Me.lblShowLoadVal.Name = "lblShowLoadVal"
        Me.lblShowLoadVal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblShowLoadVal.Size = New System.Drawing.Size(65, 17)
        Me.lblShowLoadVal.TabIndex = 6
        '
        'lblCountLoaded
        '
        Me.lblCountLoaded.BackColor = System.Drawing.SystemColors.Window
        Me.lblCountLoaded.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCountLoaded.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCountLoaded.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblCountLoaded.Location = New System.Drawing.Point(8, 120)
        Me.lblCountLoaded.Name = "lblCountLoaded"
        Me.lblCountLoaded.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCountLoaded.Size = New System.Drawing.Size(241, 17)
        Me.lblCountLoaded.TabIndex = 5
        Me.lblCountLoaded.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblNoteFreqIn
        '
        Me.lblNoteFreqIn.BackColor = System.Drawing.SystemColors.Window
        Me.lblNoteFreqIn.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblNoteFreqIn.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNoteFreqIn.ForeColor = System.Drawing.Color.Red
        Me.lblNoteFreqIn.Location = New System.Drawing.Point(32, 72)
        Me.lblNoteFreqIn.Name = "lblNoteFreqIn"
        Me.lblNoteFreqIn.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblNoteFreqIn.Size = New System.Drawing.Size(289, 33)
        Me.lblNoteFreqIn.TabIndex = 4
        Me.lblNoteFreqIn.Text = "NOTE: There must be a TTL frequency at the counter 1 input."
        Me.lblNoteFreqIn.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblDemoFunction
        '
        Me.lblDemoFunction.BackColor = System.Drawing.SystemColors.Window
        Me.lblDemoFunction.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDemoFunction.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDemoFunction.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblDemoFunction.Location = New System.Drawing.Point(44, 9)
        Me.lblDemoFunction.Name = "lblDemoFunction"
        Me.lblDemoFunction.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDemoFunction.Size = New System.Drawing.Size(249, 27)
        Me.lblDemoFunction.TabIndex = 3
        Me.lblDemoFunction.Text = "Demonstration of 8536 Counter Functions"
        Me.lblDemoFunction.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'frm8536Count
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 13)
        Me.BackColor = System.Drawing.SystemColors.Window
        Me.ClientSize = New System.Drawing.Size(342, 242)
        Me.Controls.Add(Me.cmdQuit)
        Me.Controls.Add(Me.cmdStart)
        Me.Controls.Add(Me.lblCount)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.lblShowLoadVal)
        Me.Controls.Add(Me.lblCountLoaded)
        Me.Controls.Add(Me.lblNoteFreqIn)
        Me.Controls.Add(Me.lblDemoFunction)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Location = New System.Drawing.Point(73, 100)
        Me.Name = "frm8536Count"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Universal Library 8536 Counter Demo"
        Me.ResumeLayout(False)

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


        ULStat = MccDaq.MccService.ErrHandling(MccDaq.ErrorReporting.PrintAll, MccDaq.ErrorHandling.StopAll)
        If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then
            Stop
        End If


    End Sub

#End Region

End Class