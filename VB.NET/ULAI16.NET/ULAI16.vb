'==============================================================================

' File:                         ULAI16.VB

' Library Call Demonstrated:    Mccdaq.MccBoard.AInScan() 

' Purpose:                      Scans a range of A/D Input Channels and stores
'                               the sample data in an array.

' Demonstration:                Displays the analog input on up to 8 channels.

' Other Library Calls:          MccDaq.MccService.ErrHandling()
'                               MccDaq.MccService.WinBufAlloc
'                               MccDaq.MccService.WinBufToArray()
'                               MccDaq.MccService.WinBufFree()

'  Special Requirements:        Board 0 must support bridge measurement and
'                               the shunt resistor is connected between
'                               AI+ and Ex- internally
'==============================================================================
Option Strict Off
Option Explicit On 
Public Class frmDataDisplay
    Inherits System.Windows.Forms.Form

    Const NumPoints As Integer = 1000    ' Number of data points to collect
    Const FirstPoint As Integer = 0     ' set first element in buffer to transfer to array

    'Create a new MccBoard object for Board 0
    Private DaqBoard As MccDaq.MccBoard = New MccDaq.MccBoard(0)

    Private ADData(NumPoints) As System.Double ' dimension an array to hold the input values

    ' define a variable to contain the handle for memory allocated by Windows through
    ' MccDaq.MccService.ScaledWinBufAlloc() 
    Private MemHandle As IntPtr
    Private WithEvents groupBox1 As System.Windows.Forms.GroupBox
    Public WithEvents lblOffsetMeasStrain As System.Windows.Forms.Label
    Public WithEvents lblOffset As System.Windows.Forms.Label
    Private WithEvents groupBox2 As System.Windows.Forms.GroupBox
    Public WithEvents lblGainSimStrain As System.Windows.Forms.Label
    Public WithEvents lblGainSim As System.Windows.Forms.Label
    Public WithEvents lblGainMeasStrain As System.Windows.Forms.Label
    Public WithEvents lblGainMeas As System.Windows.Forms.Label
    Public WithEvents lblGainAdjustmentFactor As System.Windows.Forms.Label
    Public WithEvents lblGainFactor As System.Windows.Forms.Label

    Public lblADData As System.Windows.Forms.Label()
    Private Enum StrainConfig
        FullBridgeI = 0
        FullBridgeII = 1
        FullBridgeIII = 2
        HalfBridgeI = 3
        HalfBridgeII = 4
        QuarterBridgeI = 5
        QuarterBridgeII = 6
    End Enum


    Private Sub cmdStart_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdStart.Click
        Dim i As Integer
        Dim ULStat As MccDaq.ErrorInfo
        Dim Range As MccDaq.Range
        Dim Options As MccDaq.ScanOptions
        Dim Rate As Integer
        Dim Count As Integer
        Dim Chan As Integer

        Dim StrainConfiguration As StrainConfig = StrainConfig.QuarterBridgeI

        Dim InitialVoltage As Double = 0.0 'Bridge output voltage in the unloaded condition. This value is subtracted from any measurements before scaling equations are applied. 		
        Dim VInitial As Double = 0.0
        Dim Total As Double = 0
        Dim VOffset As Double = 0.0
        Dim OffsetAdjustmentFactor As Double
        Dim GainAdjustmentFactor As Double
        Dim VActualBridge As Double       'Actual bridge voltage
        Dim VSimulatedBridge As Double    'Simulated bridge voltage
        Dim REffective As Double          'Effective resistance

        Dim RGage As Double = 350         'Gage Resistance
        Dim RShunt As Double = 100000     'Resistance of Shunt Resistor
        Dim VExcitation As Double = 2.5   'Excitation voltage
        Dim GageFactor As Double = 2
        Dim PoissonRatio As Double = 0

        Dim MeasuredStrain As Double
        Dim SimulatedStrain As Double

        cmdStart.Enabled = False

        ' Calculate the offset adjusment factor on a resting gage in software
        ' Parameters:
        '   LowChan    :the first channel of the scan
        '   HighChan   :the last channel of the scan
        '   Count      :the total number of A/D samples to collect
        '   Rate       :sample rate
        '   Range      :the range for the board
        '   MemHandle  :Handle for Windows buffer to store data in
        '   Options    :data collection options

        Chan = Integer.Parse(txtChan.Text) ' channel to acquire
        If (Chan > 3) Then Chan = 3
        txtChan.Text = Str(Chan)

        VInitial = InitialVoltage / VExcitation

        Count = NumPoints ' total number of data points to collect
        Rate = 1000 ' per channel sampling rate ((samples per second) per channel)

        ' return data as 12-bit values
        Options = MccDaq.ScanOptions.ScaleData
        Range = MccDaq.Range.NotUsed 'set the range

        If MemHandle = 0 Then Stop ' check that a handle to a memory buffer exists

        ULStat = DaqBoard.AInScan(Chan, Chan, Count, Rate, Range, MemHandle, Options)

        If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors And _
           ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.FreeRunning Then
            Stop
        End If

        ' Transfer the data from the memory buffer set up by Windows to an array for use by Visual Basic

        ULStat = MccDaq.MccService.ScaledWinBufToArray(MemHandle, ADData, FirstPoint, Count)
        If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then Stop

        For i = 0 To NumPoints - 1
            Total = Total + ADData(i)
        Next i

        VOffset = Total / Count

        VOffset = VOffset - VInitial

        OffsetAdjustmentFactor = CalculateStrain(StrainConfiguration, VOffset, GageFactor, PoissonRatio)

        lblOffsetMeasStrain.Text = OffsetAdjustmentFactor.ToString("F9")

        ' Enable Shunt Calibration Circuit and Collect the values and
        ' Calculate the Actual Bridge Voltage

        Options = MccDaq.ScanOptions.ScaleData + MccDaq.ScanOptions.ShuntCal
        ULStat = DaqBoard.AInScan(Chan, Chan, Count, Rate, Range, MemHandle, Options)

        If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors And _
           ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.FreeRunning Then
            Stop
        End If

        ' Transfer the data from the memory buffer set up by Windows to an array for use by Visual Basic

        ULStat = MccDaq.MccService.ScaledWinBufToArray(MemHandle, ADData, FirstPoint, Count)
        If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then Stop

        Total = 0

        For i = 0 To NumPoints - 1
            Total = Total + ADData(i)
        Next i

        VActualBridge = Total / NumPoints

        VActualBridge = VActualBridge - VInitial

        MeasuredStrain = CalculateStrain(StrainConfiguration, VActualBridge, GageFactor, PoissonRatio)

        lblGainMeasStrain.Text = MeasuredStrain.ToString("F9")

        ' Calculate the Simulated Bridge Strain with a shunt resistor

        REffective = (RGage * RShunt) / (RGage + RShunt)

        VSimulatedBridge = (REffective / (REffective + RGage) - 0.5)

        SimulatedStrain = CalculateStrain(StrainConfiguration, VSimulatedBridge, GageFactor, PoissonRatio)

        lblGainSimStrain.Text = SimulatedStrain.ToString("F9")

        GainAdjustmentFactor = SimulatedStrain / (MeasuredStrain - OffsetAdjustmentFactor)

        lblGainAdjustmentFactor.Text = GainAdjustmentFactor.ToString("F9")

        cmdStart.Enabled = True

    End Sub

    Private Function CalculateStrain(ByVal StrainCfg As StrainConfig, ByVal U As Double, ByVal GageFactor As Double, ByVal PoissonRatio As Double) As Double
        Dim starin As Double = 0
        Select Case StrainCfg
            Case StrainConfig.FullBridgeI
                starin = (-U) / GageFactor
            Case StrainConfig.FullBridgeII
                starin = (-2 * U) / (GageFactor * (1 + PoissonRatio))
            Case StrainConfig.FullBridgeIII
                starin = (-2 * U) / (GageFactor * ((PoissonRatio + 1) - (U * (PoissonRatio - 1))))
            Case StrainConfig.HalfBridgeI
                starin = (-4 * U) / (GageFactor * ((PoissonRatio + 1) - 2 * U * (PoissonRatio - 1)))
            Case StrainConfig.HalfBridgeII
                starin = (-2 * U) / GageFactor
            Case StrainConfig.QuarterBridgeI, StrainConfig.QuarterBridgeII
                starin = (-4 * U) / (GageFactor * ((1 + 2 * U)))
        End Select

        Return starin
    End Function

    Private Sub cmdStopConvert_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdStopConvert.Click
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

        InitUL()


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
    Public WithEvents cmdStart As System.Windows.Forms.Button
    Public WithEvents Label1 As System.Windows.Forms.Label
    Public WithEvents lblDemoFunction As System.Windows.Forms.Label
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    Public WithEvents txtChan As System.Windows.Forms.TextBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.txtChan = New System.Windows.Forms.TextBox
        Me.cmdStopConvert = New System.Windows.Forms.Button
        Me.cmdStart = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.lblDemoFunction = New System.Windows.Forms.Label
        Me.groupBox1 = New System.Windows.Forms.GroupBox
        Me.lblOffsetMeasStrain = New System.Windows.Forms.Label
        Me.lblOffset = New System.Windows.Forms.Label
        Me.groupBox2 = New System.Windows.Forms.GroupBox
        Me.lblGainSimStrain = New System.Windows.Forms.Label
        Me.lblGainSim = New System.Windows.Forms.Label
        Me.lblGainMeasStrain = New System.Windows.Forms.Label
        Me.lblGainMeas = New System.Windows.Forms.Label
        Me.lblGainAdjustmentFactor = New System.Windows.Forms.Label
        Me.lblGainFactor = New System.Windows.Forms.Label
        Me.groupBox1.SuspendLayout()
        Me.groupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtChan
        '
        Me.txtChan.AcceptsReturn = True
        Me.txtChan.BackColor = System.Drawing.SystemColors.Window
        Me.txtChan.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtChan.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtChan.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtChan.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtChan.Location = New System.Drawing.Point(152, 64)
        Me.txtChan.MaxLength = 0
        Me.txtChan.Name = "txtChan"
        Me.txtChan.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtChan.Size = New System.Drawing.Size(33, 19)
        Me.txtChan.TabIndex = 20
        Me.txtChan.Text = "0"
        Me.txtChan.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'cmdStopConvert
        '
        Me.cmdStopConvert.BackColor = System.Drawing.SystemColors.Control
        Me.cmdStopConvert.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdStopConvert.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdStopConvert.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdStopConvert.Location = New System.Drawing.Point(278, 293)
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
        Me.cmdStart.Location = New System.Drawing.Point(206, 293)
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
        Me.Label1.Location = New System.Drawing.Point(104, 66)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(48, 17)
        Me.Label1.TabIndex = 19
        Me.Label1.Text = "Channel:"
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
        Me.lblDemoFunction.Size = New System.Drawing.Size(337, 41)
        Me.lblDemoFunction.TabIndex = 0
        Me.lblDemoFunction.Text = "Demonstration of the bridge nulling and shunt calibration procedure for a specifi" & _
            "ed channel  "
        Me.lblDemoFunction.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'groupBox1
        '
        Me.groupBox1.Controls.Add(Me.lblOffsetMeasStrain)
        Me.groupBox1.Controls.Add(Me.lblOffset)
        Me.groupBox1.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.groupBox1.ForeColor = System.Drawing.Color.DarkBlue
        Me.groupBox1.Location = New System.Drawing.Point(7, 92)
        Me.groupBox1.Name = "groupBox1"
        Me.groupBox1.Size = New System.Drawing.Size(328, 64)
        Me.groupBox1.TabIndex = 23
        Me.groupBox1.TabStop = False
        Me.groupBox1.Text = "Offset Adjustment"
        '
        'lblOffsetMeasStrain
        '
        Me.lblOffsetMeasStrain.BackColor = System.Drawing.SystemColors.Window
        Me.lblOffsetMeasStrain.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblOffsetMeasStrain.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOffsetMeasStrain.ForeColor = System.Drawing.Color.Blue
        Me.lblOffsetMeasStrain.Location = New System.Drawing.Point(112, 32)
        Me.lblOffsetMeasStrain.Name = "lblOffsetMeasStrain"
        Me.lblOffsetMeasStrain.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblOffsetMeasStrain.Size = New System.Drawing.Size(128, 17)
        Me.lblOffsetMeasStrain.TabIndex = 9
        '
        'lblOffset
        '
        Me.lblOffset.BackColor = System.Drawing.SystemColors.Window
        Me.lblOffset.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblOffset.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOffset.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblOffset.Location = New System.Drawing.Point(16, 32)
        Me.lblOffset.Name = "lblOffset"
        Me.lblOffset.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblOffset.Size = New System.Drawing.Size(90, 17)
        Me.lblOffset.TabIndex = 1
        Me.lblOffset.Text = "Measured Strain:"
        '
        'groupBox2
        '
        Me.groupBox2.Controls.Add(Me.lblGainSimStrain)
        Me.groupBox2.Controls.Add(Me.lblGainSim)
        Me.groupBox2.Controls.Add(Me.lblGainMeasStrain)
        Me.groupBox2.Controls.Add(Me.lblGainMeas)
        Me.groupBox2.Controls.Add(Me.lblGainAdjustmentFactor)
        Me.groupBox2.Controls.Add(Me.lblGainFactor)
        Me.groupBox2.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.groupBox2.ForeColor = System.Drawing.Color.DarkBlue
        Me.groupBox2.Location = New System.Drawing.Point(7, 164)
        Me.groupBox2.Name = "groupBox2"
        Me.groupBox2.Size = New System.Drawing.Size(328, 112)
        Me.groupBox2.TabIndex = 24
        Me.groupBox2.TabStop = False
        Me.groupBox2.Text = "Gain Adjustment"
        '
        'lblGainSimStrain
        '
        Me.lblGainSimStrain.BackColor = System.Drawing.SystemColors.Window
        Me.lblGainSimStrain.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblGainSimStrain.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblGainSimStrain.ForeColor = System.Drawing.Color.Blue
        Me.lblGainSimStrain.Location = New System.Drawing.Point(104, 32)
        Me.lblGainSimStrain.Name = "lblGainSimStrain"
        Me.lblGainSimStrain.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblGainSimStrain.Size = New System.Drawing.Size(128, 17)
        Me.lblGainSimStrain.TabIndex = 14
        '
        'lblGainSim
        '
        Me.lblGainSim.BackColor = System.Drawing.SystemColors.Window
        Me.lblGainSim.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblGainSim.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblGainSim.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblGainSim.Location = New System.Drawing.Point(8, 32)
        Me.lblGainSim.Name = "lblGainSim"
        Me.lblGainSim.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblGainSim.Size = New System.Drawing.Size(88, 17)
        Me.lblGainSim.TabIndex = 13
        Me.lblGainSim.Text = "Simulated Strain:"
        '
        'lblGainMeasStrain
        '
        Me.lblGainMeasStrain.BackColor = System.Drawing.SystemColors.Window
        Me.lblGainMeasStrain.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblGainMeasStrain.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblGainMeasStrain.ForeColor = System.Drawing.Color.Blue
        Me.lblGainMeasStrain.Location = New System.Drawing.Point(104, 56)
        Me.lblGainMeasStrain.Name = "lblGainMeasStrain"
        Me.lblGainMeasStrain.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblGainMeasStrain.Size = New System.Drawing.Size(128, 17)
        Me.lblGainMeasStrain.TabIndex = 12
        '
        'lblGainMeas
        '
        Me.lblGainMeas.BackColor = System.Drawing.SystemColors.Window
        Me.lblGainMeas.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblGainMeas.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblGainMeas.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblGainMeas.Location = New System.Drawing.Point(8, 56)
        Me.lblGainMeas.Name = "lblGainMeas"
        Me.lblGainMeas.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblGainMeas.Size = New System.Drawing.Size(90, 17)
        Me.lblGainMeas.TabIndex = 11
        Me.lblGainMeas.Text = "Measured Strain:"
        '
        'lblGainAdjustmentFactor
        '
        Me.lblGainAdjustmentFactor.BackColor = System.Drawing.SystemColors.Window
        Me.lblGainAdjustmentFactor.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblGainAdjustmentFactor.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblGainAdjustmentFactor.ForeColor = System.Drawing.Color.Blue
        Me.lblGainAdjustmentFactor.Location = New System.Drawing.Point(136, 80)
        Me.lblGainAdjustmentFactor.Name = "lblGainAdjustmentFactor"
        Me.lblGainAdjustmentFactor.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblGainAdjustmentFactor.Size = New System.Drawing.Size(96, 17)
        Me.lblGainAdjustmentFactor.TabIndex = 10
        '
        'lblGainFactor
        '
        Me.lblGainFactor.BackColor = System.Drawing.SystemColors.Window
        Me.lblGainFactor.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblGainFactor.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblGainFactor.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblGainFactor.Location = New System.Drawing.Point(8, 80)
        Me.lblGainFactor.Name = "lblGainFactor"
        Me.lblGainFactor.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblGainFactor.Size = New System.Drawing.Size(128, 17)
        Me.lblGainFactor.TabIndex = 2
        Me.lblGainFactor.Text = "Gain Adjustment Factor:"
        '
        'frmDataDisplay
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 13)
        Me.BackColor = System.Drawing.SystemColors.Window
        Me.ClientSize = New System.Drawing.Size(349, 331)
        Me.Controls.Add(Me.groupBox1)
        Me.Controls.Add(Me.groupBox2)
        Me.Controls.Add(Me.txtChan)
        Me.Controls.Add(Me.cmdStopConvert)
        Me.Controls.Add(Me.cmdStart)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.lblDemoFunction)
        Me.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ForeColor = System.Drawing.Color.Blue
        Me.Location = New System.Drawing.Point(190, 108)
        Me.Name = "frmDataDisplay"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Universal Library Analog Input Scan"
        Me.groupBox1.ResumeLayout(False)
        Me.groupBox2.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
#End Region


#Region "Universal Library Initialization - Expand this region to change error handling, etc."

    Private Sub InitUL()

        Dim ULStat As MccDaq.ErrorInfo

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

        MemHandle = MccDaq.MccService.ScaledWinBufAllocEx(NumPoints) ' set aside memory to hold data
        If MemHandle = 0 Then Stop

    End Sub
#End Region

End Class