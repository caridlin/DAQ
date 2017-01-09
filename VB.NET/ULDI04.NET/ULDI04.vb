'==============================================================================

' File:                         ULDI04.VB

' Library Call Demonstrated:    MccDaq.MccBoard.DIn()

' Purpose:                      Reads a value from Digital Port.

' Demonstration:                Read MccDaq.DigitalPortType 
'

' Other Library Calls:          MccDaq.MccService.ErrHandling()
'                               MccDaq.MccBoard.DConfigPort()

' Special Requirements:         Board 0 must have a digital input port
'                               or digital ports programmable as input.

'==============================================================================
Option Strict Off
Option Explicit On

Public Class frmDigAuxIn

    Inherits System.Windows.Forms.Form

    'Create a new MccBoard object for Board 0
    Private DaqBoard As MccDaq.MccBoard = New MccDaq.MccBoard(0)

    Private NumPorts, NumBits, FirstBit As Integer
    Private ProgAbility As Integer
    Private PortName As String

    Private PortType As MccDaq.DigitalPortType
    Private PortNum As MccDaq.DigitalPortType
    Private Direction As MccDaq.DigitalPortDirection

    Private Sub frmDigAuxIn_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim ULStat As MccDaq.ErrorInfo

        InitUL()    'initiate error handling, etc

        'determine if digital port exists, its capabilities, etc
        PortType = PORTIN
        NumPorts = FindPortsOfType(DaqBoard, PortType, ProgAbility, PortNum, NumBits, FirstBit)

        If NumPorts < 1 Then
            lblInstruct.Text = "Board " & DaqBoard.BoardNum.ToString() & _
                " has no compatible digital ports."
        Else
            PortName = PortNum.ToString
            lblInstruct.Text = "Input a TTL high or low level to " & _
            PortName & " digital inputs on board " & DaqBoard.BoardNum.ToString() & _
            " to change Data Value."
            If ProgAbility = DigitalIO.PROGPORT Then
                ULStat = DaqBoard.DConfigPort(PortType, MccDaq.DigitalPortDirection.DigitalIn)
                If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then Stop
            End If
            tmrReadInputs.Enabled = True
        End If

    End Sub

    Private Sub tmrReadInputs_Tick(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tmrReadInputs.Tick

        Dim ULStat As MccDaq.ErrorInfo
        Dim DataValue As UInt16

        tmrReadInputs.Stop()

        ULStat = DaqBoard.DIn(PortNum, DataValue)
        If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then Stop

        lblPortVal.Text = "Value read at " & PortName & " digital inputs:"
        lblShowPortVal.Text = DataValue.ToString("0")

        tmrReadInputs.Start()

    End Sub

    Private Sub cmdEndProgram_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdEndProgram.Click

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
    Public WithEvents tmrReadInputs As System.Windows.Forms.Timer
    Public WithEvents lblShowPortVal As System.Windows.Forms.Label
    Public WithEvents lblPortVal As System.Windows.Forms.Label
    Public WithEvents lblInstruct As System.Windows.Forms.Label
    Public WithEvents lblDemoFunction As System.Windows.Forms.Label
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdEndProgram = New System.Windows.Forms.Button
        Me.tmrReadInputs = New System.Windows.Forms.Timer(Me.components)
        Me.lblShowPortVal = New System.Windows.Forms.Label
        Me.lblPortVal = New System.Windows.Forms.Label
        Me.lblInstruct = New System.Windows.Forms.Label
        Me.lblDemoFunction = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'cmdEndProgram
        '
        Me.cmdEndProgram.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEndProgram.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEndProgram.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEndProgram.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEndProgram.Location = New System.Drawing.Point(256, 200)
        Me.cmdEndProgram.Name = "cmdEndProgram"
        Me.cmdEndProgram.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEndProgram.Size = New System.Drawing.Size(57, 33)
        Me.cmdEndProgram.TabIndex = 3
        Me.cmdEndProgram.Text = "Quit"
        Me.cmdEndProgram.UseVisualStyleBackColor = False
        '
        'tmrReadInputs
        '
        Me.tmrReadInputs.Interval = 200
        '
        'lblShowPortVal
        '
        Me.lblShowPortVal.BackColor = System.Drawing.SystemColors.Window
        Me.lblShowPortVal.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblShowPortVal.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShowPortVal.ForeColor = System.Drawing.Color.Blue
        Me.lblShowPortVal.Location = New System.Drawing.Point(236, 152)
        Me.lblShowPortVal.Name = "lblShowPortVal"
        Me.lblShowPortVal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblShowPortVal.Size = New System.Drawing.Size(77, 17)
        Me.lblShowPortVal.TabIndex = 2
        '
        'lblPortVal
        '
        Me.lblPortVal.BackColor = System.Drawing.SystemColors.Window
        Me.lblPortVal.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPortVal.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPortVal.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblPortVal.Location = New System.Drawing.Point(12, 152)
        Me.lblPortVal.Name = "lblPortVal"
        Me.lblPortVal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPortVal.Size = New System.Drawing.Size(209, 17)
        Me.lblPortVal.TabIndex = 1
        Me.lblPortVal.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblInstruct
        '
        Me.lblInstruct.BackColor = System.Drawing.SystemColors.Window
        Me.lblInstruct.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblInstruct.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInstruct.ForeColor = System.Drawing.Color.Red
        Me.lblInstruct.Location = New System.Drawing.Point(48, 54)
        Me.lblInstruct.Name = "lblInstruct"
        Me.lblInstruct.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblInstruct.Size = New System.Drawing.Size(233, 33)
        Me.lblInstruct.TabIndex = 4
        Me.lblInstruct.Text = "Input a TTL high or low level to digital inputs to change Data Value."
        Me.lblInstruct.TextAlign = System.Drawing.ContentAlignment.TopCenter
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
        Me.lblDemoFunction.Size = New System.Drawing.Size(307, 24)
        Me.lblDemoFunction.TabIndex = 0
        Me.lblDemoFunction.Text = "Demonstration of MccDaq.MccBoard.DIn()"
        Me.lblDemoFunction.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'frmDigAuxIn
        '
        Me.AcceptButton = Me.cmdEndProgram
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 13)
        Me.BackColor = System.Drawing.SystemColors.Window
        Me.ClientSize = New System.Drawing.Size(331, 246)
        Me.Controls.Add(Me.cmdEndProgram)
        Me.Controls.Add(Me.lblShowPortVal)
        Me.Controls.Add(Me.lblPortVal)
        Me.Controls.Add(Me.lblInstruct)
        Me.Controls.Add(Me.lblDemoFunction)
        Me.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Location = New System.Drawing.Point(7, 103)
        Me.Name = "frmDigAuxIn"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Universal Library Digital In"
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

        ReportError = MccDaq.ErrorReporting.PrintAll
        HandleError = MccDaq.ErrorHandling.StopAll
        ULStat = MccDaq.MccService.ErrHandling(ReportError, HandleError)
        If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then
            Stop
        End If

    End Sub

#End Region

End Class