'==============================================================================
'
' File:                         ULDI06.VB

' Library Call Demonstrated:    MccDaq.MccBoard.DConfigBit()

' Purpose:                      Reads the status of a single bit within a 
'                               digital port after configuring for input.

' Demonstration:                Configures a single bit (within a digital port)
'                               for input (if programmable) and reads the bit status.

' Other Library Calls:          MccDaq.MccBoard.DBitIn()
'                               MccDaq.MccService.ErrHandling()

' Special Requirements:         Board 0 must have a digital port that supports
'                               input or bits that can be configured for input.

'==============================================================================
Option Strict Off
Option Explicit On

Friend Class frmDigIn

    Inherits System.Windows.Forms.Form

    'Create a new MccBoard object for Board 0
    Private DaqBoard As MccDaq.MccBoard = New MccDaq.MccBoard(0)

    Private NumPorts, NumBits, FirstBit As Integer
    Private ProgAbility As Integer

    Private PortType As Integer
    Private PortNum As MccDaq.DigitalPortType
    Private Direction As MccDaq.DigitalPortDirection

    Private Sub frmDigIn_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim PortName, BitName As String
        Dim ULStat As MccDaq.ErrorInfo

        InitUL()    'initiate error handling, etc

        'determine if digital port exists, its capabilities, etc
        PortType = BITIN
        NumPorts = FindPortsOfType(DaqBoard, PortType, ProgAbility, PortNum, NumBits, FirstBit)
        If Not (ProgAbility = DigitalIO.PROGBIT) Then NumPorts = 0

        If NumPorts < 1 Then
            lblInstruct.Text = "Board " & DaqBoard.BoardNum.ToString() & _
                " has no programmable digital bits."
            lblBitNum.Text = ""
        Else
            ' if programmable, set direction of bit to input
            ' configure the first bit for digital input
            Direction = MccDaq.DigitalPortDirection.DigitalIn
            ULStat = DaqBoard.DConfigBit(PortNum, FirstBit, Direction)
            If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then Stop
            PortName = PortNum.ToString
            BitName = FirstBit.ToString
            Me.lblInstruct.Text = "You may change the bit state by applying a TTL high " & _
            "or a TTL low to the corresponding pin on " & PortName & ", bit " & BitName & _
            " on board " & DaqBoard.BoardNum.ToString() & "."
            tmrReadInputs.Enabled = True
        End If

    End Sub

    Private Sub tmrReadInputs_Tick(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tmrReadInputs.Tick

        Dim ULStat As MccDaq.ErrorInfo
        Dim BitValue As MccDaq.DigitalLogicState
        Dim BitPort As MccDaq.DigitalPortType
        Dim PortName, BitName As String

        tmrReadInputs.Stop()

        ' read a single bit status from the digital port

        '  Parameters:
        '    PortType   :the digital I/O port type (must be
        '                AUXPORT or FIRSTPORTA for bit read.
        '    BitNum     :the bit to read
        '    BitValue   :the value read from the port
        BitPort = MccDaq.DigitalPortType.AuxPort
        If PortNum > MccDaq.DigitalPortType.AuxPort _
        Then BitPort = MccDaq.DigitalPortType.FirstPortA

        ULStat = DaqBoard.DBitIn(BitPort, FirstBit, BitValue)
        If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then Stop

        PortName = BitPort.ToString()
        BitName = FirstBit.ToString()
        lblBitNum.Text = "The state of " & PortName & " bit " & BitName _
        & " is " & Convert.ToInt32(BitValue).ToString()

        tmrReadInputs.Start()

    End Sub

    Private Sub cmdStopRead_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdStopRead.Click

        tmrReadInputs.Enabled = False
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
    Public WithEvents tmrReadInputs As System.Windows.Forms.Timer
    Public WithEvents lblBitNum As System.Windows.Forms.Label
    Public WithEvents lblInstruct As System.Windows.Forms.Label
    Public WithEvents _lblShowBitNum_0 As System.Windows.Forms.Label
    Public WithEvents lblDemoFunction As System.Windows.Forms.Label
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdStopRead = New System.Windows.Forms.Button
        Me.tmrReadInputs = New System.Windows.Forms.Timer(Me.components)
        Me.lblBitNum = New System.Windows.Forms.Label
        Me.lblInstruct = New System.Windows.Forms.Label
        Me._lblShowBitNum_0 = New System.Windows.Forms.Label
        Me.lblDemoFunction = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'cmdStopRead
        '
        Me.cmdStopRead.BackColor = System.Drawing.SystemColors.Control
        Me.cmdStopRead.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdStopRead.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdStopRead.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdStopRead.Location = New System.Drawing.Point(248, 184)
        Me.cmdStopRead.Name = "cmdStopRead"
        Me.cmdStopRead.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdStopRead.Size = New System.Drawing.Size(57, 33)
        Me.cmdStopRead.TabIndex = 1
        Me.cmdStopRead.Text = "Quit"
        Me.cmdStopRead.UseVisualStyleBackColor = False
        '
        'tmrReadInputs
        '
        Me.tmrReadInputs.Interval = 200
        '
        'lblBitNum
        '
        Me.lblBitNum.BackColor = System.Drawing.SystemColors.Window
        Me.lblBitNum.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBitNum.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBitNum.ForeColor = System.Drawing.Color.Blue
        Me.lblBitNum.Location = New System.Drawing.Point(53, 126)
        Me.lblBitNum.Name = "lblBitNum"
        Me.lblBitNum.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBitNum.Size = New System.Drawing.Size(166, 17)
        Me.lblBitNum.TabIndex = 4
        Me.lblBitNum.Text = "Bit Number"
        Me.lblBitNum.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblInstruct
        '
        Me.lblInstruct.BackColor = System.Drawing.SystemColors.Window
        Me.lblInstruct.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblInstruct.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInstruct.ForeColor = System.Drawing.Color.Red
        Me.lblInstruct.Location = New System.Drawing.Point(12, 56)
        Me.lblInstruct.Name = "lblInstruct"
        Me.lblInstruct.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblInstruct.Size = New System.Drawing.Size(293, 49)
        Me.lblInstruct.TabIndex = 3
        Me.lblInstruct.Text = "You may change the bit state by applying a TTL high or a TTL low to the correspon" & _
            "ding pin on the port."
        Me.lblInstruct.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblShowBitNum_0
        '
        Me._lblShowBitNum_0.BackColor = System.Drawing.SystemColors.Window
        Me._lblShowBitNum_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblShowBitNum_0.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblShowBitNum_0.ForeColor = System.Drawing.Color.Blue
        Me._lblShowBitNum_0.Location = New System.Drawing.Point(232, 126)
        Me._lblShowBitNum_0.Name = "_lblShowBitNum_0"
        Me._lblShowBitNum_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblShowBitNum_0.Size = New System.Drawing.Size(19, 17)
        Me._lblShowBitNum_0.TabIndex = 2
        '
        'lblDemoFunction
        '
        Me.lblDemoFunction.BackColor = System.Drawing.SystemColors.Window
        Me.lblDemoFunction.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDemoFunction.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDemoFunction.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblDemoFunction.Location = New System.Drawing.Point(15, 16)
        Me.lblDemoFunction.Name = "lblDemoFunction"
        Me.lblDemoFunction.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDemoFunction.Size = New System.Drawing.Size(290, 25)
        Me.lblDemoFunction.TabIndex = 0
        Me.lblDemoFunction.Text = "Demonstration of MccDaq.MccBoard.DConfigBit()"
        Me.lblDemoFunction.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'frmDigIn
        '
        Me.AcceptButton = Me.cmdStopRead
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 13)
        Me.BackColor = System.Drawing.SystemColors.Window
        Me.ClientSize = New System.Drawing.Size(326, 232)
        Me.Controls.Add(Me.cmdStopRead)
        Me.Controls.Add(Me.lblBitNum)
        Me.Controls.Add(Me.lblInstruct)
        Me.Controls.Add(Me._lblShowBitNum_0)
        Me.Controls.Add(Me.lblDemoFunction)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Location = New System.Drawing.Point(7, 103)
        Me.Name = "frmDigIn"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Universal Library Digital Bit Input"
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

        lblShowBitNum = New System.Windows.Forms.Label(8) {}
        Me.lblShowBitNum.SetValue(_lblShowBitNum_0, 0)

    End Sub

    Public lblShowBitNum As System.Windows.Forms.Label()

#End Region

End Class