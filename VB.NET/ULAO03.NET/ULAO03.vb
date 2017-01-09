'==============================================================================
'
' File:                         ULAO03.VB
'
' Library Call Demonstrated:    MccBoard.AOut()
'                               MccBoard.BoardConfig.SetDACUpdateMode()
'                               MccBoard.BoardConfig.DACUpdate()
'
' Purpose:                      Demonstrates difference between DACUpdate.Immediate
'                               and DACUpdate.OnCommand D/A Update modes
'
' Demonstration:                Delays outputs until user issues update command 
'                               DACUpdate().
'
' Other Library Calls:          MccService.ErrHandling()
'                               MccBoard.FromEngUnits()
'
' Special Requirements:         Board 0 must support BIDACUPDATEMODE settings,
'                               such as the PCI-DAC6700's.
'
'==============================================================================

Option Strict Off
Option Explicit On 

Public Class frmAOut

    Inherits System.Windows.Forms.Form

    ' Create a new MccBoard object for Board 0
    Private DaqBoard As MccDaq.MccBoard = New MccDaq.MccBoard(0)

    Private Range As MccDaq.Range
    Private ADResolution, NumAOChans, HighChan As Integer

    Const AllChannels As Integer = -1

    Private Sub frmAOut_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim LowChan As Integer
        Dim ChannelType As Integer
        Dim DefaultTrig As MccDaq.TriggerType

        InitUL()

        ' determine the number of analog channels and their capabilities
        ChannelType = ANALOGOUTPUT
        NumAOChans = FindAnalogChansOfType(DaqBoard, ChannelType, _
            ADResolution, Range, LowChan, DefaultTrig)

        If (NumAOChans = 0) Then
            lblInstruction.Text = "Board " & DaqBoard.BoardNum.ToString() & _
            " does not have analog output channels."
            Me.btnSendData.Enabled = False
            Me.btnUpdateDACs.Enabled = False
        Else
            lblDemoFunction.Text = "Demonstration of MccBoard.Aout with DacUpdate."
            lblInstruction.Text = "Board " & DaqBoard.BoardNum.ToString() & _
                " updating analog outputs using MccBoard.AOut with DacUpdate" & _
                " and Range of " & Range.ToString() & "."
            If NumAOChans > 4 Then NumAOChans = 4
            HighChan = LowChan + NumAOChans - 1
        End If

    End Sub

    Private Sub rdioOnCommand_CheckedChanged(ByVal sender As System.Object, _
    ByVal e As System.EventArgs) Handles rdioOnCommand.CheckedChanged

        Dim channel As Integer

        If (rdioOnCommand.Checked) Then
            ' Set DAC Update mode to hold off updating D/A's until command is sent
            ' Parameters
            '	 channel	: D/A channel whose update mode is to be configured. Note
            '				  that negative values selects all channels
            '   DACUpdate.OnCommand : delay D/A output updates from AOut or AOutScan until
            '                         DACUpdate command is issued.
            channel = AllChannels
            DaqBoard.BoardConfig.SetDACUpdateMode(channel, MccDaq.DACUpdate.OnCommand)
        End If

    End Sub

    Private Sub rdioImmediate_CheckedChanged(ByVal sender As System.Object, _
    ByVal e As System.EventArgs) Handles rdioImmediate.CheckedChanged

        Dim channel As Integer

        If (rdioImmediate.Checked) Then
            ' Set DAC Update mode to update immediately upon cbAOut or cbAOutScan
            ' Parameters
            '	 channel	: D/A channel whose update mode is to be configured. Note
            '				  that negative values selects all channels
            '   DACUpdate.Immediate : update D/A outputs immediately upon AOut or AOutScan
            channel = AllChannels
            DaqBoard.BoardConfig.SetDACUpdateMode(channel, MccDaq.DACUpdate.Immediate)
        End If

    End Sub

    Private Sub btnSendData_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSendData.Click

        Dim volts As Single = 0.0F
        Dim daCounts As Short = 0
        Dim chan As Integer

        For chan = 0 To 3
            If chan <= HighChan Then
                'get voltage to output
                volts = Val(txtAOVolts(chan).Text)

                ' convert from voltage to binary counts
                DaqBoard.FromEngUnits(Range, volts, daCounts)

                ' load D/A
                DaqBoard.AOut(chan, Range, daCounts)
            End If
        Next chan

    End Sub

    Private Sub btnUpdateDACs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdateDACs.Click

        ' Issue D/A update command
        DaqBoard.BoardConfig.DACUpdate()

    End Sub

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents label5 As System.Windows.Forms.Label
    Friend WithEvents label4 As System.Windows.Forms.Label
    Friend WithEvents label3 As System.Windows.Forms.Label
    Friend WithEvents label2 As System.Windows.Forms.Label
    Friend WithEvents btnSendData As System.Windows.Forms.Button
    Friend WithEvents btnUpdateDACs As System.Windows.Forms.Button
    Friend WithEvents rdioImmediate As System.Windows.Forms.RadioButton
    Friend WithEvents rdioOnCommand As System.Windows.Forms.RadioButton
    Friend WithEvents txtAOVolts3 As System.Windows.Forms.TextBox
    Friend WithEvents txtAOVolts2 As System.Windows.Forms.TextBox
    Friend WithEvents txtAOVolts1 As System.Windows.Forms.TextBox
    Friend WithEvents txtAOVolts0 As System.Windows.Forms.TextBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.label5 = New System.Windows.Forms.Label
        Me.label4 = New System.Windows.Forms.Label
        Me.label3 = New System.Windows.Forms.Label
        Me.label2 = New System.Windows.Forms.Label
        Me.btnSendData = New System.Windows.Forms.Button
        Me.btnUpdateDACs = New System.Windows.Forms.Button
        Me.rdioImmediate = New System.Windows.Forms.RadioButton
        Me.rdioOnCommand = New System.Windows.Forms.RadioButton
        Me.txtAOVolts3 = New System.Windows.Forms.TextBox
        Me.txtAOVolts2 = New System.Windows.Forms.TextBox
        Me.txtAOVolts1 = New System.Windows.Forms.TextBox
        Me.txtAOVolts0 = New System.Windows.Forms.TextBox
        Me.lblInstruction = New System.Windows.Forms.Label
        Me.lblDemoFunction = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'label5
        '
        Me.label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.label5.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.label5.Location = New System.Drawing.Point(288, 133)
        Me.label5.Name = "label5"
        Me.label5.Size = New System.Drawing.Size(80, 16)
        Me.label5.TabIndex = 13
        Me.label5.Text = "Channel 3"
        '
        'label4
        '
        Me.label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.label4.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.label4.Location = New System.Drawing.Point(200, 133)
        Me.label4.Name = "label4"
        Me.label4.Size = New System.Drawing.Size(80, 16)
        Me.label4.TabIndex = 12
        Me.label4.Text = "Channel 2"
        '
        'label3
        '
        Me.label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.label3.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.label3.Location = New System.Drawing.Point(112, 133)
        Me.label3.Name = "label3"
        Me.label3.Size = New System.Drawing.Size(80, 16)
        Me.label3.TabIndex = 11
        Me.label3.Text = "Channel 1"
        '
        'label2
        '
        Me.label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.label2.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.label2.Location = New System.Drawing.Point(24, 133)
        Me.label2.Name = "label2"
        Me.label2.Size = New System.Drawing.Size(80, 16)
        Me.label2.TabIndex = 10
        Me.label2.Text = "Channel 0"
        '
        'btnSendData
        '
        Me.btnSendData.BackColor = System.Drawing.SystemColors.Control
        Me.btnSendData.Location = New System.Drawing.Point(48, 93)
        Me.btnSendData.Name = "btnSendData"
        Me.btnSendData.Size = New System.Drawing.Size(136, 32)
        Me.btnSendData.TabIndex = 9
        Me.btnSendData.Text = "Send Data"
        Me.btnSendData.UseVisualStyleBackColor = False
        '
        'btnUpdateDACs
        '
        Me.btnUpdateDACs.BackColor = System.Drawing.SystemColors.Control
        Me.btnUpdateDACs.Location = New System.Drawing.Point(208, 93)
        Me.btnUpdateDACs.Name = "btnUpdateDACs"
        Me.btnUpdateDACs.Size = New System.Drawing.Size(136, 32)
        Me.btnUpdateDACs.TabIndex = 8
        Me.btnUpdateDACs.Text = "Update Outputs"
        Me.btnUpdateDACs.UseVisualStyleBackColor = False
        '
        'rdioImmediate
        '
        Me.rdioImmediate.Location = New System.Drawing.Point(104, 229)
        Me.rdioImmediate.Name = "rdioImmediate"
        Me.rdioImmediate.Size = New System.Drawing.Size(168, 24)
        Me.rdioImmediate.TabIndex = 15
        Me.rdioImmediate.Text = "Update Immediately"
        '
        'rdioOnCommand
        '
        Me.rdioOnCommand.Checked = True
        Me.rdioOnCommand.Location = New System.Drawing.Point(104, 197)
        Me.rdioOnCommand.Name = "rdioOnCommand"
        Me.rdioOnCommand.Size = New System.Drawing.Size(168, 24)
        Me.rdioOnCommand.TabIndex = 14
        Me.rdioOnCommand.TabStop = True
        Me.rdioOnCommand.Text = "Update On Command"
        '
        'txtAOVolts3
        '
        Me.txtAOVolts3.Location = New System.Drawing.Point(292, 157)
        Me.txtAOVolts3.Name = "txtAOVolts3"
        Me.txtAOVolts3.Size = New System.Drawing.Size(72, 20)
        Me.txtAOVolts3.TabIndex = 19
        Me.txtAOVolts3.Text = "0.00"
        '
        'txtAOVolts2
        '
        Me.txtAOVolts2.Location = New System.Drawing.Point(204, 157)
        Me.txtAOVolts2.Name = "txtAOVolts2"
        Me.txtAOVolts2.Size = New System.Drawing.Size(72, 20)
        Me.txtAOVolts2.TabIndex = 18
        Me.txtAOVolts2.Text = "0.00"
        '
        'txtAOVolts1
        '
        Me.txtAOVolts1.Location = New System.Drawing.Point(116, 157)
        Me.txtAOVolts1.Name = "txtAOVolts1"
        Me.txtAOVolts1.Size = New System.Drawing.Size(72, 20)
        Me.txtAOVolts1.TabIndex = 17
        Me.txtAOVolts1.Text = "0.00"
        '
        'txtAOVolts0
        '
        Me.txtAOVolts0.Location = New System.Drawing.Point(28, 157)
        Me.txtAOVolts0.Name = "txtAOVolts0"
        Me.txtAOVolts0.Size = New System.Drawing.Size(72, 20)
        Me.txtAOVolts0.TabIndex = 16
        Me.txtAOVolts0.Text = "0.00"
        '
        'lblInstruction
        '
        Me.lblInstruction.BackColor = System.Drawing.SystemColors.Window
        Me.lblInstruction.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblInstruction.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInstruction.ForeColor = System.Drawing.Color.Red
        Me.lblInstruction.Location = New System.Drawing.Point(52, 43)
        Me.lblInstruction.Name = "lblInstruction"
        Me.lblInstruction.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblInstruction.Size = New System.Drawing.Size(286, 40)
        Me.lblInstruction.TabIndex = 21
        Me.lblInstruction.Text = "Board must have a D/A converter that supports DacUpdate."
        Me.lblInstruction.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblDemoFunction
        '
        Me.lblDemoFunction.BackColor = System.Drawing.SystemColors.Window
        Me.lblDemoFunction.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDemoFunction.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDemoFunction.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblDemoFunction.Location = New System.Drawing.Point(51, 10)
        Me.lblDemoFunction.Name = "lblDemoFunction"
        Me.lblDemoFunction.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDemoFunction.Size = New System.Drawing.Size(286, 25)
        Me.lblDemoFunction.TabIndex = 20
        Me.lblDemoFunction.Text = "Demonstration of MccBoard.AOut using DacUpdate"
        Me.lblDemoFunction.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'frmAOut
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Window
        Me.ClientSize = New System.Drawing.Size(392, 272)
        Me.Controls.Add(Me.lblInstruction)
        Me.Controls.Add(Me.lblDemoFunction)
        Me.Controls.Add(Me.txtAOVolts3)
        Me.Controls.Add(Me.txtAOVolts2)
        Me.Controls.Add(Me.txtAOVolts1)
        Me.Controls.Add(Me.txtAOVolts0)
        Me.Controls.Add(Me.rdioImmediate)
        Me.Controls.Add(Me.rdioOnCommand)
        Me.Controls.Add(Me.label5)
        Me.Controls.Add(Me.label4)
        Me.Controls.Add(Me.label3)
        Me.Controls.Add(Me.label2)
        Me.Controls.Add(Me.btnSendData)
        Me.Controls.Add(Me.btnUpdateDACs)
        Me.Name = "frmAOut"
        Me.Text = "Universal Library Analog Output"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Public txtAOVolts As System.Windows.Forms.TextBox()
    Public WithEvents lblInstruction As System.Windows.Forms.Label
    Public WithEvents lblDemoFunction As System.Windows.Forms.Label

#End Region

#Region "Universal Library Initialization - Expand to change error handling, etc."

    Private Sub InitUL()

        Dim ULStat As MccDaq.ErrorInfo

        ' Initiate error handling
        '  activating error handling will trap errors like
        '  bad channel numbers and non-configured conditions.
        '  Parameters:
        '    MccDaq.ErrorReporting.PrintAll :all warnings and errors encountered will be printed
        '    MccDaq.ErrorHandling.StopAll   :if any error is encountered, the program will stop

        ReportError = MccDaq.ErrorReporting.PrintAll
        HandleError = MccDaq.ErrorHandling.DontStop
        ULStat = MccDaq.MccService.ErrHandling(ReportError, HandleError)
        If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then
            Stop
        End If

        ' Attach references of voltage textboxes to array for easier access
        txtAOVolts = New System.Windows.Forms.TextBox(4) {}

        txtAOVolts.SetValue(txtAOVolts3, 3)
        txtAOVolts.SetValue(txtAOVolts2, 2)
        txtAOVolts.SetValue(txtAOVolts1, 1)
        txtAOVolts.SetValue(txtAOVolts0, 0)

    End Sub

#End Region

End Class
