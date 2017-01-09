'==============================================================================

' File:                         ULAO02.VB

' Library Call Demonstrated:    Mccdaq.MccBoard.AOutScan()

' Purpose:                      Writes to a range of D/A Output Channels.

' Demonstration:                Sends a digital output to the D/A channels

' Other Library Calls:          MccDaq.MccService.ErrHandling()

' Special Requirements:         Board 0 must have 2 or more D/A converters.
'                               This function is designed for boards that
'                               support timed analog output.  It can be used
'                               for polled output boards but only for values
'                               of NumPoints up to the number of channels
'                               that the board supports (i.e., NumPoints =
'                               6 maximum for the six channel CIO-DDA06).

'==============================================================================
Option Strict Off
Option Explicit On 

Friend Class frmSendAData

    Inherits System.Windows.Forms.Form

    'Create a new MccBoard object for Board 0
    Private DaqBoard As MccDaq.MccBoard = New MccDaq.MccBoard(0)

    Private Range As MccDaq.Range
    Private DAResolution, NumAOChans, HighChan As Integer
    Private MaxChan As Long

    Dim NumPoints As Integer
    Dim Count As Integer

    Dim DAData() As UInt16
    Dim MemHandle As IntPtr ' define a variable to contain the handle for
    ' memory allocated by Windows through MccDaq.MccService.WinBufAlloc()
    Dim FirstPoint As Integer
    Dim LowChan As Integer
    Private ULStat As MccDaq.ErrorInfo

    Private Sub frmSendAData_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim ChannelType As Integer
        Dim DefaultTrig As MccDaq.TriggerType

        InitUL()

        ' determine the number of analog channels and their capabilities
        ChannelType = ANALOGOUTPUT
        NumAOChans = FindAnalogChansOfType(DaqBoard, ChannelType, _
            DAResolution, Range, LowChan, DefaultTrig)

        If (NumAOChans = 0) Then
            lblInstruction.Text = "Board " & DaqBoard.BoardNum.ToString() & _
            " does not have analog input channels."
            cmdSendData.Enabled = False
        Else
            If NumAOChans > 4 Then NumAOChans = 4
            NumPoints = NumAOChans
            MaxChan = NumAOChans - 1
            MemHandle = MccDaq.MccService.WinBufAllocEx(NumPoints)
            If MemHandle = 0 Then Stop
            ReDim DAData(NumPoints - 1)
            Dim ValueStep As Long, FSCount As Long
            Dim StepCount As Long, i As Long

            FSCount& = 2 ^ DAResolution
            ValueStep& = FSCount& / (NumAOChans + 1)
            For i& = 0 To NumPoints - 1
                StepCount& = ValueStep& * (i& + 1)
                DAData(i&) = StepCount&
            Next i&
            FirstPoint = 0
            ULStat = MccDaq.MccService.WinArrayToBuf(DAData, MemHandle, FirstPoint, NumPoints)
            lblInstruction.Text = "Board " & DaqBoard.BoardNum.ToString("0") & _
                " generating analog output on up to " & Format(NumAOChans, "0") _
                & " channels using cbAOutScan() " & _
                " at a Range of " & Range.ToString() & "."
            HighChan = LowChan + NumAOChans - 1
        End If

    End Sub

    Private Sub cmdSendData_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSendData.Click

        Dim i As Integer
        Dim ULStat As MccDaq.ErrorInfo
        Dim Options As MccDaq.ScanOptions
        Dim Rate As Integer, VoltValue As Single

        ' Parameters:
        '   LowChan    :the lower channel of the scan
        '   HighChan   :the upper channel of the scan
        '   Count      :the number of D/A values to send
        '   Rate       :send rate in values/second (if supported by BoardNum)
        '   MemHandle  :Handle for Windows buffer from which data will be output
        '   Options    :data send options

        FirstPoint = 0
        Rate = 100 'Rate of data update (ignored if board does not
        Options = MccDaq.ScanOptions.Default 'support timed analog output)

        ULStat = DaqBoard.AOutScan(LowChan, HighChan, NumPoints, Rate, Range, MemHandle, Options)
        If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then Stop

        For i = 0 To NumPoints - 1
            lblAOutData(i).Text = DAData(i).ToString("0")
            VoltValue! = ConvertToVolts(DAData(i))
            lblAOutVolts(i).Text = Format$(VoltValue!, "0.000V")
        Next i
        For i = HighChan + 1 To 3
            lblAOutData(i).Text = ""
        Next i

    End Sub

    Private Function ConvertToVolts(ByVal DataVal As UShort) As Single

        Dim LSBVal As Single, FSVolts As Single
        Dim OutVal As Single

        FSVolts! = GetRangeVolts(Range)
        LSBVal! = FSVolts! / 2 ^ DAResolution
        OutVal! = LSBVal! * DataVal
        If Range < 100 Then OutVal! = OutVal! - (FSVolts! / 2)
        ConvertToVolts = OutVal!

    End Function

    Private Sub cmdEndProgram_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdEndProgram.Click

        Dim ULStat As MccDaq.ErrorInfo

        ULStat = MccDaq.MccService.WinBufFreeEx(MemHandle) ' Free up memory for use by
        ' other programs
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
    Public WithEvents cmdEndProgram As System.Windows.Forms.Button
    Public WithEvents cmdSendData As System.Windows.Forms.Button
    Public WithEvents lblRaw As System.Windows.Forms.Label
    Public WithEvents _lblAOutData_1 As System.Windows.Forms.Label
    Public WithEvents _lblAOutData_0 As System.Windows.Forms.Label
    Public WithEvents lblChan1 As System.Windows.Forms.Label
    Public WithEvents lblChan0 As System.Windows.Forms.Label
    Public WithEvents lblDemoFunction As System.Windows.Forms.Label
    Public WithEvents _lblAOutData_2 As System.Windows.Forms.Label
    Public WithEvents lblChan2 As System.Windows.Forms.Label
    Public WithEvents _lblAOutData_3 As System.Windows.Forms.Label
    Public WithEvents lblChan3 As System.Windows.Forms.Label
    Public WithEvents lblInstruction As System.Windows.Forms.Label
    Public WithEvents lblVolts3 As System.Windows.Forms.Label
    Public WithEvents lblVolts2 As System.Windows.Forms.Label
    Public WithEvents lblVolts1 As System.Windows.Forms.Label
    Public WithEvents lblVolts0 As System.Windows.Forms.Label

    Public lblAOutData As System.Windows.Forms.Label()
    Public WithEvents Label1 As System.Windows.Forms.Label
    Public lblAOutVolts As System.Windows.Forms.Label()

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdEndProgram = New System.Windows.Forms.Button
        Me.cmdSendData = New System.Windows.Forms.Button
        Me.lblRaw = New System.Windows.Forms.Label
        Me._lblAOutData_1 = New System.Windows.Forms.Label
        Me._lblAOutData_0 = New System.Windows.Forms.Label
        Me.lblChan1 = New System.Windows.Forms.Label
        Me.lblChan0 = New System.Windows.Forms.Label
        Me.lblDemoFunction = New System.Windows.Forms.Label
        Me._lblAOutData_2 = New System.Windows.Forms.Label
        Me.lblChan2 = New System.Windows.Forms.Label
        Me._lblAOutData_3 = New System.Windows.Forms.Label
        Me.lblChan3 = New System.Windows.Forms.Label
        Me.lblInstruction = New System.Windows.Forms.Label
        Me.lblVolts3 = New System.Windows.Forms.Label
        Me.lblVolts2 = New System.Windows.Forms.Label
        Me.lblVolts1 = New System.Windows.Forms.Label
        Me.lblVolts0 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'cmdEndProgram
        '
        Me.cmdEndProgram.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEndProgram.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEndProgram.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEndProgram.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEndProgram.Location = New System.Drawing.Point(317, 213)
        Me.cmdEndProgram.Name = "cmdEndProgram"
        Me.cmdEndProgram.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEndProgram.Size = New System.Drawing.Size(55, 26)
        Me.cmdEndProgram.TabIndex = 1
        Me.cmdEndProgram.Text = "Quit"
        Me.cmdEndProgram.UseVisualStyleBackColor = False
        '
        'cmdSendData
        '
        Me.cmdSendData.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSendData.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSendData.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSendData.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSendData.Location = New System.Drawing.Point(180, 213)
        Me.cmdSendData.Name = "cmdSendData"
        Me.cmdSendData.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSendData.Size = New System.Drawing.Size(81, 26)
        Me.cmdSendData.TabIndex = 2
        Me.cmdSendData.Text = "Send Data"
        Me.cmdSendData.UseVisualStyleBackColor = False
        '
        'lblRaw
        '
        Me.lblRaw.BackColor = System.Drawing.SystemColors.Window
        Me.lblRaw.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRaw.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRaw.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRaw.Location = New System.Drawing.Point(8, 124)
        Me.lblRaw.Name = "lblRaw"
        Me.lblRaw.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRaw.Size = New System.Drawing.Size(57, 25)
        Me.lblRaw.TabIndex = 7
        Me.lblRaw.Text = "Raw Data"
        Me.lblRaw.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        '_lblAOutData_1
        '
        Me._lblAOutData_1.BackColor = System.Drawing.SystemColors.Window
        Me._lblAOutData_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblAOutData_1.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblAOutData_1.ForeColor = System.Drawing.Color.Blue
        Me._lblAOutData_1.Location = New System.Drawing.Point(150, 132)
        Me._lblAOutData_1.Name = "_lblAOutData_1"
        Me._lblAOutData_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblAOutData_1.Size = New System.Drawing.Size(65, 17)
        Me._lblAOutData_1.TabIndex = 6
        Me._lblAOutData_1.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblAOutData_0
        '
        Me._lblAOutData_0.BackColor = System.Drawing.SystemColors.Window
        Me._lblAOutData_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblAOutData_0.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblAOutData_0.ForeColor = System.Drawing.Color.Blue
        Me._lblAOutData_0.Location = New System.Drawing.Point(70, 132)
        Me._lblAOutData_0.Name = "_lblAOutData_0"
        Me._lblAOutData_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblAOutData_0.Size = New System.Drawing.Size(65, 17)
        Me._lblAOutData_0.TabIndex = 3
        Me._lblAOutData_0.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblChan1
        '
        Me.lblChan1.BackColor = System.Drawing.SystemColors.Window
        Me.lblChan1.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblChan1.Font = New System.Drawing.Font("Arial", 8.25!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblChan1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblChan1.Location = New System.Drawing.Point(150, 108)
        Me.lblChan1.Name = "lblChan1"
        Me.lblChan1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblChan1.Size = New System.Drawing.Size(65, 17)
        Me.lblChan1.TabIndex = 5
        Me.lblChan1.Text = "Channel 1"
        Me.lblChan1.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblChan0
        '
        Me.lblChan0.BackColor = System.Drawing.SystemColors.Window
        Me.lblChan0.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblChan0.Font = New System.Drawing.Font("Arial", 8.25!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblChan0.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblChan0.Location = New System.Drawing.Point(70, 108)
        Me.lblChan0.Name = "lblChan0"
        Me.lblChan0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblChan0.Size = New System.Drawing.Size(65, 17)
        Me.lblChan0.TabIndex = 4
        Me.lblChan0.Text = "Channel 0"
        Me.lblChan0.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblDemoFunction
        '
        Me.lblDemoFunction.BackColor = System.Drawing.SystemColors.Window
        Me.lblDemoFunction.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDemoFunction.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDemoFunction.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblDemoFunction.Location = New System.Drawing.Point(24, 6)
        Me.lblDemoFunction.Name = "lblDemoFunction"
        Me.lblDemoFunction.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDemoFunction.Size = New System.Drawing.Size(348, 25)
        Me.lblDemoFunction.TabIndex = 0
        Me.lblDemoFunction.Text = "Demonstration of MccDaq.MccBoard.AOutScan()"
        Me.lblDemoFunction.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblAOutData_2
        '
        Me._lblAOutData_2.BackColor = System.Drawing.SystemColors.Window
        Me._lblAOutData_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblAOutData_2.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblAOutData_2.ForeColor = System.Drawing.Color.Blue
        Me._lblAOutData_2.Location = New System.Drawing.Point(226, 132)
        Me._lblAOutData_2.Name = "_lblAOutData_2"
        Me._lblAOutData_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblAOutData_2.Size = New System.Drawing.Size(65, 17)
        Me._lblAOutData_2.TabIndex = 9
        Me._lblAOutData_2.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblChan2
        '
        Me.lblChan2.BackColor = System.Drawing.SystemColors.Window
        Me.lblChan2.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblChan2.Font = New System.Drawing.Font("Arial", 8.25!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblChan2.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblChan2.Location = New System.Drawing.Point(226, 108)
        Me.lblChan2.Name = "lblChan2"
        Me.lblChan2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblChan2.Size = New System.Drawing.Size(65, 17)
        Me.lblChan2.TabIndex = 8
        Me.lblChan2.Text = "Channel 2"
        Me.lblChan2.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblAOutData_3
        '
        Me._lblAOutData_3.BackColor = System.Drawing.SystemColors.Window
        Me._lblAOutData_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblAOutData_3.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblAOutData_3.ForeColor = System.Drawing.Color.Blue
        Me._lblAOutData_3.Location = New System.Drawing.Point(307, 132)
        Me._lblAOutData_3.Name = "_lblAOutData_3"
        Me._lblAOutData_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblAOutData_3.Size = New System.Drawing.Size(65, 17)
        Me._lblAOutData_3.TabIndex = 11
        Me._lblAOutData_3.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblChan3
        '
        Me.lblChan3.BackColor = System.Drawing.SystemColors.Window
        Me.lblChan3.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblChan3.Font = New System.Drawing.Font("Arial", 8.25!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblChan3.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblChan3.Location = New System.Drawing.Point(307, 108)
        Me.lblChan3.Name = "lblChan3"
        Me.lblChan3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblChan3.Size = New System.Drawing.Size(65, 17)
        Me.lblChan3.TabIndex = 10
        Me.lblChan3.Text = "Channel 2"
        Me.lblChan3.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblInstruction
        '
        Me.lblInstruction.BackColor = System.Drawing.SystemColors.Window
        Me.lblInstruction.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblInstruction.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInstruction.ForeColor = System.Drawing.Color.Red
        Me.lblInstruction.Location = New System.Drawing.Point(52, 33)
        Me.lblInstruction.Name = "lblInstruction"
        Me.lblInstruction.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblInstruction.Size = New System.Drawing.Size(286, 52)
        Me.lblInstruction.TabIndex = 12
        Me.lblInstruction.Text = "Board 0 must have an D/A converter."
        Me.lblInstruction.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblVolts3
        '
        Me.lblVolts3.BackColor = System.Drawing.SystemColors.Window
        Me.lblVolts3.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblVolts3.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblVolts3.ForeColor = System.Drawing.Color.Blue
        Me.lblVolts3.Location = New System.Drawing.Point(307, 159)
        Me.lblVolts3.Name = "lblVolts3"
        Me.lblVolts3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblVolts3.Size = New System.Drawing.Size(65, 17)
        Me.lblVolts3.TabIndex = 16
        Me.lblVolts3.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblVolts2
        '
        Me.lblVolts2.BackColor = System.Drawing.SystemColors.Window
        Me.lblVolts2.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblVolts2.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblVolts2.ForeColor = System.Drawing.Color.Blue
        Me.lblVolts2.Location = New System.Drawing.Point(226, 159)
        Me.lblVolts2.Name = "lblVolts2"
        Me.lblVolts2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblVolts2.Size = New System.Drawing.Size(65, 17)
        Me.lblVolts2.TabIndex = 15
        Me.lblVolts2.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblVolts1
        '
        Me.lblVolts1.BackColor = System.Drawing.SystemColors.Window
        Me.lblVolts1.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblVolts1.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblVolts1.ForeColor = System.Drawing.Color.Blue
        Me.lblVolts1.Location = New System.Drawing.Point(150, 159)
        Me.lblVolts1.Name = "lblVolts1"
        Me.lblVolts1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblVolts1.Size = New System.Drawing.Size(65, 17)
        Me.lblVolts1.TabIndex = 14
        Me.lblVolts1.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblVolts0
        '
        Me.lblVolts0.BackColor = System.Drawing.SystemColors.Window
        Me.lblVolts0.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblVolts0.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblVolts0.ForeColor = System.Drawing.Color.Blue
        Me.lblVolts0.Location = New System.Drawing.Point(70, 159)
        Me.lblVolts0.Name = "lblVolts0"
        Me.lblVolts0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblVolts0.Size = New System.Drawing.Size(65, 17)
        Me.lblVolts0.TabIndex = 13
        Me.lblVolts0.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.SystemColors.Window
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(8, 155)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(57, 25)
        Me.Label1.TabIndex = 17
        Me.Label1.Text = "Volts"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'frmSendAData
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 13)
        Me.BackColor = System.Drawing.SystemColors.Window
        Me.ClientSize = New System.Drawing.Size(391, 253)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.lblVolts3)
        Me.Controls.Add(Me.lblVolts2)
        Me.Controls.Add(Me.lblVolts1)
        Me.Controls.Add(Me.lblVolts0)
        Me.Controls.Add(Me.lblInstruction)
        Me.Controls.Add(Me._lblAOutData_3)
        Me.Controls.Add(Me.lblChan3)
        Me.Controls.Add(Me._lblAOutData_2)
        Me.Controls.Add(Me.lblChan2)
        Me.Controls.Add(Me.cmdEndProgram)
        Me.Controls.Add(Me.cmdSendData)
        Me.Controls.Add(Me.lblRaw)
        Me.Controls.Add(Me._lblAOutData_1)
        Me.Controls.Add(Me._lblAOutData_0)
        Me.Controls.Add(Me.lblChan1)
        Me.Controls.Add(Me.lblChan0)
        Me.Controls.Add(Me.lblDemoFunction)
        Me.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ForeColor = System.Drawing.Color.Blue
        Me.Location = New System.Drawing.Point(7, 103)
        Me.Name = "frmSendAData"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Universal Library Analog Output "
        Me.ResumeLayout(False)

    End Sub

#End Region

#Region "Universal Library Initialization - Expand this region to change error handling, etc."

    Private Sub InitUL()

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

        lblAOutData = New System.Windows.Forms.Label(3) _
        {_lblAOutData_0, _lblAOutData_1, _lblAOutData_2, _lblAOutData_3}

        lblAOutVolts = New System.Windows.Forms.Label(3) _
        {lblVolts0, lblVolts1, lblVolts2, lblVolts3}

    End Sub

#End Region

End Class