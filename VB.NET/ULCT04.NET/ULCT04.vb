'==============================================================================

' File:                         ULCT04.VB

' Library Call Demonstrated:    Mccdaq.MccBoard.CFreqIn()

' Purpose:                      Measure the frequency of a signal.

' Demonstration:                Initializes the counter and measures a frequency.

' Other Library Calls:          MccDaq.MccService.ErrHandling()

' Special Requirements:         Board 0 must have a 9513 Counter.
'                               External freq. at counter 1 input.
'                               (100Hz < freq < 330kHz)
'                               External connection from counter 4 output
'                               to counter 5 gate.

'==============================================================================
Option Strict Off
Option Explicit On

Friend Class frm9513Freq

    Inherits System.Windows.Forms.Form

    Const CounterType As Integer = CTR9513    ' type of counter compatible with this sample
    Private CounterNum, NumCtrs As Integer

    'Create a new MccBoard object for Board 0
    Private DaqBoard As MccDaq.MccBoard = New MccDaq.MccBoard(0)

    Const ChipNum As Short = 1  ' use chip 1 for CTR05 or for first
    '                             chip on CTR10 or CTR20

    Private Sub frm9513Freq_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim TimeOfDayCounting As MccDaq.TimeOfDay
        Dim Compare2 As MccDaq.CompareValue
        Dim Compare1 As MccDaq.CompareValue
        Dim Source As MccDaq.CounterSource
        Dim FOutDivider As Short
        Dim ULStat As MccDaq.ErrorInfo

        InitUL()

        NumCtrs = FindCountersOfType(DaqBoard, CounterType, CounterNum)
        If NumCtrs < 1 Then
            Me.lblInstruct.Text = "Board " & DaqBoard.BoardNum.ToString() & _
                " has no 9513 counters."
            Me.cmdMeasureFreq.Enabled = False
        Else
            ' Initialize the board level features
            '  Parameters:
            '    ChipNum    :chip to be initialized (1 for CTR5, 1 or 2 for CTR10)
            '    FOutDivider:the F-Out divider (0-15)
            '    Source     :the signal source for F-Out
            '    Compare1   :status of comparator 1
            '    Compare2   :status of comparator 2
            '    TimeOfDay  :time of day mode control

            FOutDivider = 1 ' sets up OSC OUT for 10kHz signal which can
            Source = MccDaq.CounterSource.Freq3 ' be used as frequency source for this example
            Compare1 = MccDaq.CompareValue.Disabled
            Compare2 = MccDaq.CompareValue.Disabled
            TimeOfDayCounting = MccDaq.TimeOfDay.Disabled

            ULStat = DaqBoard.C9513Init(ChipNum, FOutDivider, _
            Source, Compare1, Compare2, TimeOfDayCounting)
            If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then Stop
            lblInstruct.Text = "There must be a TTL pulse at counter 1 input " & _
            "with a frequency between  100Hz and  600kHz. Also, connect the " & _
            "output of counter 4 to the gate of counter 5 on board " & _
            DaqBoard.BoardNum.ToString() & "."
        End If

    End Sub

    Private Sub cmdMeasureFreq_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdMeasureFreq.Click

        Dim ULStat As MccDaq.ErrorInfo
        Dim Freq As Integer
        Dim Count As UInt16
        Dim SigSource As MccDaq.SignalSource
        Dim GateInterval As Short

        ' Measure the frequency of the internally-generated signal
        '  Parameters:
        '    SigSource    :the counter to be measured (1 to 5)
        '    GateInterval :gating interval in millseconds
        '    Count     :the raw count during GateInterval is returned here
        '    Freq         :the calculated frequency (Hz) is returned here

        GateInterval = 100
        SigSource = MccDaq.SignalSource.CtrInput1

        ULStat = DaqBoard.CFreqIn(SigSource, GateInterval, Count, Freq)
        If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then Stop

        lblCount.Text = Count.ToString("0")
        lblFreq.Text = Freq.ToString("0") & "Hz"

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
    Public WithEvents cmdMeasureFreq As System.Windows.Forms.Button
    Public WithEvents lblFreq As System.Windows.Forms.Label
    Public WithEvents lblCount As System.Windows.Forms.Label
    Public WithEvents lblFrequency As System.Windows.Forms.Label
    Public WithEvents lblCountNum As System.Windows.Forms.Label
    Public WithEvents lblInstruct As System.Windows.Forms.Label
    Public WithEvents lblDemoFunction As System.Windows.Forms.Label
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdStopRead = New System.Windows.Forms.Button
        Me.cmdMeasureFreq = New System.Windows.Forms.Button
        Me.lblFreq = New System.Windows.Forms.Label
        Me.lblCount = New System.Windows.Forms.Label
        Me.lblFrequency = New System.Windows.Forms.Label
        Me.lblCountNum = New System.Windows.Forms.Label
        Me.lblInstruct = New System.Windows.Forms.Label
        Me.lblDemoFunction = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'cmdStopRead
        '
        Me.cmdStopRead.BackColor = System.Drawing.SystemColors.Control
        Me.cmdStopRead.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdStopRead.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdStopRead.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdStopRead.Location = New System.Drawing.Point(264, 208)
        Me.cmdStopRead.Name = "cmdStopRead"
        Me.cmdStopRead.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdStopRead.Size = New System.Drawing.Size(52, 26)
        Me.cmdStopRead.TabIndex = 1
        Me.cmdStopRead.Text = "Quit"
        Me.cmdStopRead.UseVisualStyleBackColor = False
        '
        'cmdMeasureFreq
        '
        Me.cmdMeasureFreq.BackColor = System.Drawing.SystemColors.Control
        Me.cmdMeasureFreq.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdMeasureFreq.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdMeasureFreq.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdMeasureFreq.Location = New System.Drawing.Point(96, 208)
        Me.cmdMeasureFreq.Name = "cmdMeasureFreq"
        Me.cmdMeasureFreq.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdMeasureFreq.Size = New System.Drawing.Size(129, 25)
        Me.cmdMeasureFreq.TabIndex = 7
        Me.cmdMeasureFreq.Text = "Measure Frequency"
        Me.cmdMeasureFreq.UseVisualStyleBackColor = False
        '
        'lblFreq
        '
        Me.lblFreq.BackColor = System.Drawing.SystemColors.Window
        Me.lblFreq.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblFreq.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFreq.ForeColor = System.Drawing.Color.Blue
        Me.lblFreq.Location = New System.Drawing.Point(200, 139)
        Me.lblFreq.Name = "lblFreq"
        Me.lblFreq.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblFreq.Size = New System.Drawing.Size(65, 17)
        Me.lblFreq.TabIndex = 3
        Me.lblFreq.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblCount
        '
        Me.lblCount.BackColor = System.Drawing.SystemColors.Window
        Me.lblCount.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCount.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCount.ForeColor = System.Drawing.Color.Blue
        Me.lblCount.Location = New System.Drawing.Point(88, 139)
        Me.lblCount.Name = "lblCount"
        Me.lblCount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCount.Size = New System.Drawing.Size(65, 17)
        Me.lblCount.TabIndex = 2
        Me.lblCount.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblFrequency
        '
        Me.lblFrequency.BackColor = System.Drawing.SystemColors.Window
        Me.lblFrequency.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblFrequency.Font = New System.Drawing.Font("Arial", 8.25!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFrequency.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblFrequency.Location = New System.Drawing.Point(192, 115)
        Me.lblFrequency.Name = "lblFrequency"
        Me.lblFrequency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblFrequency.Size = New System.Drawing.Size(81, 17)
        Me.lblFrequency.TabIndex = 5
        Me.lblFrequency.Text = "Frequency"
        Me.lblFrequency.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblCountNum
        '
        Me.lblCountNum.BackColor = System.Drawing.SystemColors.Window
        Me.lblCountNum.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCountNum.Font = New System.Drawing.Font("Arial", 8.25!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCountNum.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblCountNum.Location = New System.Drawing.Point(64, 115)
        Me.lblCountNum.Name = "lblCountNum"
        Me.lblCountNum.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCountNum.Size = New System.Drawing.Size(113, 17)
        Me.lblCountNum.TabIndex = 4
        Me.lblCountNum.Text = "Number of Counts"
        Me.lblCountNum.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblInstruct
        '
        Me.lblInstruct.BackColor = System.Drawing.SystemColors.Window
        Me.lblInstruct.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblInstruct.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInstruct.ForeColor = System.Drawing.Color.Red
        Me.lblInstruct.Location = New System.Drawing.Point(12, 43)
        Me.lblInstruct.Name = "lblInstruct"
        Me.lblInstruct.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblInstruct.Size = New System.Drawing.Size(345, 54)
        Me.lblInstruct.TabIndex = 6
        Me.lblInstruct.Text = "There must be a TTL pulse at counter 1 input with a frequency between  100Hz and " & _
            " 600kHz. Also, connect the output of counter 4 to the gate of counter 5."
        Me.lblInstruct.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblDemoFunction
        '
        Me.lblDemoFunction.BackColor = System.Drawing.SystemColors.Window
        Me.lblDemoFunction.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDemoFunction.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDemoFunction.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblDemoFunction.Location = New System.Drawing.Point(8, 16)
        Me.lblDemoFunction.Name = "lblDemoFunction"
        Me.lblDemoFunction.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDemoFunction.Size = New System.Drawing.Size(349, 28)
        Me.lblDemoFunction.TabIndex = 0
        Me.lblDemoFunction.Text = "Demonstration of Frequency Measurement using 9513 Counter"
        Me.lblDemoFunction.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'frm9513Freq
        '
        Me.AcceptButton = Me.cmdStopRead
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 13)
        Me.BackColor = System.Drawing.SystemColors.Window
        Me.ClientSize = New System.Drawing.Size(369, 250)
        Me.Controls.Add(Me.cmdStopRead)
        Me.Controls.Add(Me.cmdMeasureFreq)
        Me.Controls.Add(Me.lblFreq)
        Me.Controls.Add(Me.lblCount)
        Me.Controls.Add(Me.lblFrequency)
        Me.Controls.Add(Me.lblCountNum)
        Me.Controls.Add(Me.lblInstruct)
        Me.Controls.Add(Me.lblDemoFunction)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Location = New System.Drawing.Point(7, 96)
        Me.Name = "frm9513Freq"
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