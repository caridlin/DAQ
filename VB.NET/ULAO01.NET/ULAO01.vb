'==============================================================================

' File:                         ULAO01.VB

' Library Call Demonstrated:    MccDaq.MccBoard.AOut()

' Purpose:                      Writes to a D/A Output Channel.

' Demonstration:                Sends a digital output to D/A 0.

' Other Library Calls:          MccDaq.MccService.ErrHandling()

' Special Requirements:         Board 0 must have a D/A converter.

'==============================================================================
Option Strict Off
Option Explicit On

Public Class frmSendAData

    Inherits System.Windows.Forms.Form

    ' Create a new MccBoard object for Board 0
    Private DaqBoard As MccDaq.MccBoard = New MccDaq.MccBoard(0)

    Private Chan As Integer = 0
    Private Range As MccDaq.Range
    Private DAResolution, NumAOChans, HighChan As Integer
    Public WithEvents lblInstruction As System.Windows.Forms.Label

    Private Sub frmSendAData_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim LowChan As Integer
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
            UpdateButton.Enabled = False
            txtVoltsToSet.Enabled = False
        Else
            lblInstruction.Text = "Board " & DaqBoard.BoardNum.ToString() & _
                " generating analog output on channel 0 using cbAOut()" & _
                " and Range of " & Range.ToString() & "."
            HighChan = LowChan + NumAOChans - 1
        End If

    End Sub

    Private Sub UpdateButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UpdateButton.Click

        Dim ULStat As MccDaq.ErrorInfo
        Dim DataValue As UInt16
        Dim EngUnits, OutVal As Single
        Dim IsValidNumber As Boolean = True

        ' send the digital output value to D/A 0 with MccDaq.MccBoard.AOut()

        IsValidNumber = Single.TryParse(txtVoltsToSet.Text, EngUnits)

        If (IsValidNumber) Then
            ' Parameters:
            '   Chan       :the D/A output channel
            '   Range      :ignored if board does not have programmable rage
            '   DataValue  :the value to send to Chan

            ULStat = DaqBoard.FromEngUnits(Range, EngUnits, DataValue)
            If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then Stop

            ULStat = DaqBoard.AOut(Chan, Range, DataValue)
            If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then Stop

            lblValueSent.Text = "The count sent to DAC channel " & Chan.ToString("0") & " was:"
            lblVoltage.Text = "The voltage at DAC channel " & Chan.ToString("0") & " is:"
            lblShowValue.Text = DataValue.ToString("0")
            OutVal = ConvertToVolts(DataValue)
            lblShowVoltage.Text = OutVal.ToString("0.0#####") & " Volts"
        End If

    End Sub

    Private Function ConvertToVolts(ByVal DataVal As UShort) As Single

        Dim LSBVal, FSVolts, OutVal As Single

        FSVolts! = GetRangeVolts(Range)
        LSBVal! = FSVolts! / Math.Pow(2, DAResolution)
        OutVal! = LSBVal! * DataVal
        If Range < 100 Then OutVal! = OutVal! - (FSVolts! / 2)
        ConvertToVolts = OutVal!

    End Function

    Private Sub cmdEndProgram_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdEndProgram.Click

        Me.Close()

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
    Public WithEvents cmdEndProgram As System.Windows.Forms.Button
    Public WithEvents txtVoltsToSet As System.Windows.Forms.TextBox
    Public WithEvents lblShowVoltage As System.Windows.Forms.Label
    Public WithEvents lblVoltage As System.Windows.Forms.Label
    Public WithEvents lblShowValue As System.Windows.Forms.Label
    Public WithEvents lblValueSent As System.Windows.Forms.Label
    Public WithEvents lblDemoFunction As System.Windows.Forms.Label
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    Friend WithEvents UpdateButton As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdEndProgram = New System.Windows.Forms.Button
        Me.txtVoltsToSet = New System.Windows.Forms.TextBox
        Me.lblShowVoltage = New System.Windows.Forms.Label
        Me.lblVoltage = New System.Windows.Forms.Label
        Me.lblShowValue = New System.Windows.Forms.Label
        Me.lblValueSent = New System.Windows.Forms.Label
        Me.lblDemoFunction = New System.Windows.Forms.Label
        Me.UpdateButton = New System.Windows.Forms.Button
        Me.lblInstruction = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'cmdEndProgram
        '
        Me.cmdEndProgram.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEndProgram.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEndProgram.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEndProgram.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEndProgram.Location = New System.Drawing.Point(248, 216)
        Me.cmdEndProgram.Name = "cmdEndProgram"
        Me.cmdEndProgram.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEndProgram.Size = New System.Drawing.Size(55, 26)
        Me.cmdEndProgram.TabIndex = 5
        Me.cmdEndProgram.Text = "Quit"
        Me.cmdEndProgram.UseVisualStyleBackColor = False
        '
        'txtVoltsToSet
        '
        Me.txtVoltsToSet.AcceptsReturn = True
        Me.txtVoltsToSet.BackColor = System.Drawing.SystemColors.Window
        Me.txtVoltsToSet.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtVoltsToSet.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtVoltsToSet.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtVoltsToSet.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtVoltsToSet.Location = New System.Drawing.Point(120, 104)
        Me.txtVoltsToSet.MaxLength = 0
        Me.txtVoltsToSet.Name = "txtVoltsToSet"
        Me.txtVoltsToSet.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtVoltsToSet.Size = New System.Drawing.Size(81, 20)
        Me.txtVoltsToSet.TabIndex = 0
        Me.txtVoltsToSet.Text = "0"
        '
        'lblShowVoltage
        '
        Me.lblShowVoltage.BackColor = System.Drawing.SystemColors.Window
        Me.lblShowVoltage.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblShowVoltage.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShowVoltage.ForeColor = System.Drawing.Color.Blue
        Me.lblShowVoltage.Location = New System.Drawing.Point(240, 176)
        Me.lblShowVoltage.Name = "lblShowVoltage"
        Me.lblShowVoltage.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblShowVoltage.Size = New System.Drawing.Size(81, 17)
        Me.lblShowVoltage.TabIndex = 6
        '
        'lblVoltage
        '
        Me.lblVoltage.BackColor = System.Drawing.SystemColors.Window
        Me.lblVoltage.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblVoltage.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblVoltage.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblVoltage.Location = New System.Drawing.Point(32, 176)
        Me.lblVoltage.Name = "lblVoltage"
        Me.lblVoltage.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblVoltage.Size = New System.Drawing.Size(201, 17)
        Me.lblVoltage.TabIndex = 7
        '
        'lblShowValue
        '
        Me.lblShowValue.BackColor = System.Drawing.SystemColors.Window
        Me.lblShowValue.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblShowValue.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShowValue.ForeColor = System.Drawing.Color.Blue
        Me.lblShowValue.Location = New System.Drawing.Point(264, 159)
        Me.lblShowValue.Name = "lblShowValue"
        Me.lblShowValue.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblShowValue.Size = New System.Drawing.Size(57, 17)
        Me.lblShowValue.TabIndex = 4
        '
        'lblValueSent
        '
        Me.lblValueSent.BackColor = System.Drawing.SystemColors.Window
        Me.lblValueSent.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblValueSent.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblValueSent.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblValueSent.Location = New System.Drawing.Point(32, 159)
        Me.lblValueSent.Name = "lblValueSent"
        Me.lblValueSent.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblValueSent.Size = New System.Drawing.Size(225, 17)
        Me.lblValueSent.TabIndex = 3
        '
        'lblDemoFunction
        '
        Me.lblDemoFunction.BackColor = System.Drawing.SystemColors.Window
        Me.lblDemoFunction.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDemoFunction.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDemoFunction.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblDemoFunction.Location = New System.Drawing.Point(12, 7)
        Me.lblDemoFunction.Name = "lblDemoFunction"
        Me.lblDemoFunction.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDemoFunction.Size = New System.Drawing.Size(308, 21)
        Me.lblDemoFunction.TabIndex = 1
        Me.lblDemoFunction.Text = "Demonstration of MccDaq.MccBoard.AOut()"
        Me.lblDemoFunction.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'UpdateButton
        '
        Me.UpdateButton.BackColor = System.Drawing.SystemColors.Control
        Me.UpdateButton.Location = New System.Drawing.Point(224, 104)
        Me.UpdateButton.Name = "UpdateButton"
        Me.UpdateButton.Size = New System.Drawing.Size(75, 23)
        Me.UpdateButton.TabIndex = 8
        Me.UpdateButton.Text = "Update"
        Me.UpdateButton.UseVisualStyleBackColor = False
        '
        'lblInstruction
        '
        Me.lblInstruction.BackColor = System.Drawing.SystemColors.Window
        Me.lblInstruction.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblInstruction.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInstruction.ForeColor = System.Drawing.Color.Red
        Me.lblInstruction.Location = New System.Drawing.Point(23, 37)
        Me.lblInstruction.Name = "lblInstruction"
        Me.lblInstruction.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblInstruction.Size = New System.Drawing.Size(286, 52)
        Me.lblInstruction.TabIndex = 11
        Me.lblInstruction.Text = "Board 0 must have an D/A converter."
        Me.lblInstruction.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'frmSendAData
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 13)
        Me.BackColor = System.Drawing.SystemColors.Window
        Me.ClientSize = New System.Drawing.Size(332, 251)
        Me.Controls.Add(Me.lblInstruction)
        Me.Controls.Add(Me.UpdateButton)
        Me.Controls.Add(Me.cmdEndProgram)
        Me.Controls.Add(Me.txtVoltsToSet)
        Me.Controls.Add(Me.lblShowVoltage)
        Me.Controls.Add(Me.lblVoltage)
        Me.Controls.Add(Me.lblShowValue)
        Me.Controls.Add(Me.lblValueSent)
        Me.Controls.Add(Me.lblDemoFunction)
        Me.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Location = New System.Drawing.Point(7, 103)
        Me.Name = "frmSendAData"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Universal Library Analog Output "
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

    End Sub

#End Region

End Class