'==============================================================================

' File:                         ULDI05.VB

' Library Call Demonstrated:    MccDaq.MccBoard.DBitIn()

' Purpose:                      Reads the status of single digital input bits.

' Demonstration:                Configures the first compatible port 
'                               for input (if necessary) and then
'                               reads and displays the bit values.

' Other Library Calls:          MccDaq.MccService.ErrHandling()
'                               MccDaq.MccBoard.DConfigPort()

' Special Requirements:         Board 0 must have a digital input port
'                               or have digital ports programmable as input.

'==============================================================================
Option Strict Off
Option Explicit On

Friend Class frmDigIn

    Inherits System.Windows.Forms.Form

    'Create a new MccBoard object for Board 0
    Private DaqBoard As MccDaq.MccBoard = New MccDaq.MccBoard(0)

    Private NumPorts, NumBits, FirstBit As Integer
    Private ProgAbility As Integer

    Private PortType As MccDaq.DigitalPortType
    Private PortNum As MccDaq.DigitalPortType
    Private Direction As MccDaq.DigitalPortDirection

    Private Sub frmDigIn_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim PortName As String
        Dim ULStat As MccDaq.ErrorInfo

        InitUL()    'initiate error handling, etc

        'determine if digital port exists, its capabilities, etc
        PortType = PORTIN
        NumPorts = FindPortsOfType(DaqBoard, PortType, ProgAbility, PortNum, NumBits, FirstBit)
        If NumBits > 8 Then NumBits = 8
        For I As Integer = NumBits To 7
            lblShowBitVal(I).Visible = False
            lblShowBitNum(I).Visible = False
        Next I

        If NumPorts < 1 Then
            lblInstruct.Text = "Board " & DaqBoard.BoardNum.ToString() & _
                " has no compatible digital ports."
        Else
            ' if programmable, set direction of port to input
            ' configure the first port for digital input
            '  Parameters:
            '    PortNum        :the input port
            '    Direction      :sets the port for input or output

            If ProgAbility = DigitalIO.PROGPORT Then
                ULStat = DaqBoard.DConfigPort(PortNum, Direction)
                If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then Stop
            End If
            PortName = PortNum.ToString
            lblInstruct.Text = "You may change the bit value read by applying " & _
            "a TTL high or TTL low to digital inputs on " & PortName & _
            " on board " & DaqBoard.BoardNum.ToString() & "."
            lblDemoFunction.Text = _
            "Demonstration of MccDaq.MccBoard.DBitIn() reading " & PortName & "."
            lblBitNum.Text = "The first " & Format(NumBits, "0") & " bits are:"
            Me.tmrReadInputs.Enabled = True
        End If

    End Sub

    Private Sub tmrReadInputs_Tick(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tmrReadInputs.Tick

        Dim BitValue As MccDaq.DigitalLogicState
        Dim BitPort As MccDaq.DigitalPortType
        Dim PortName As String
        Dim BitNum, LastBit As Integer
        Dim i As Short
        Dim ULStat As MccDaq.ErrorInfo

        tmrReadInputs.Stop()

        ' read the input bits from the ports and display

        '  Parameters:
        '    BoardNum    :the number used by CB.CFG to describe this board
        '    PortType    :must be FIRSTPORTA or AUXPORT
        '    BitNum&     :the number of the bit to read from the port
        '    BitValue&   :the value read from the port

        ' For boards whose first port is not FIRSTPORTA (such as the USB-ERB08
        ' and the USB-SSR08) offset the BitNum by FirstBit

        BitPort = MccDaq.DigitalPortType.AuxPort
        If (PortNum > BitPort) Then BitPort = MccDaq.DigitalPortType.FirstPortA

        For i = 0 To NumBits - 1
            BitNum = i
            ULStat = DaqBoard.DBitIn(BitPort, FirstBit + BitNum, BitValue)
            If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then Stop

            lblShowBitVal(i).Text = Convert.ToInt32(BitValue).ToString("0")
        Next i

        PortName = BitPort.ToString()
        LastBit = FirstBit + (NumBits - 1)
        Me.lblBitVal.Text = PortName & ", bit " & FirstBit.ToString() & _
        " - " & LastBit.ToString() & " values:"
        tmrReadInputs.Start()

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
    Public WithEvents tmrReadInputs As System.Windows.Forms.Timer
    Public WithEvents _lblShowBitVal_0 As System.Windows.Forms.Label
    Public WithEvents _lblShowBitVal_1 As System.Windows.Forms.Label
    Public WithEvents _lblShowBitVal_2 As System.Windows.Forms.Label
    Public WithEvents _lblShowBitVal_3 As System.Windows.Forms.Label
    Public WithEvents _lblShowBitVal_4 As System.Windows.Forms.Label
    Public WithEvents _lblShowBitVal_5 As System.Windows.Forms.Label
    Public WithEvents _lblShowBitVal_6 As System.Windows.Forms.Label
    Public WithEvents _lblShowBitVal_7 As System.Windows.Forms.Label
    Public WithEvents lblBitVal As System.Windows.Forms.Label
    Public WithEvents _lblShowBitNum_7 As System.Windows.Forms.Label
    Public WithEvents _lblShowBitNum_6 As System.Windows.Forms.Label
    Public WithEvents _lblShowBitNum_5 As System.Windows.Forms.Label
    Public WithEvents _lblShowBitNum_4 As System.Windows.Forms.Label
    Public WithEvents _lblShowBitNum_3 As System.Windows.Forms.Label
    Public WithEvents _lblShowBitNum_2 As System.Windows.Forms.Label
    Public WithEvents _lblShowBitNum_1 As System.Windows.Forms.Label
    Public WithEvents _lblShowBitNum_0 As System.Windows.Forms.Label
    Public WithEvents lblBitNum As System.Windows.Forms.Label
    Public WithEvents lblInstruct As System.Windows.Forms.Label
    Public WithEvents lblDemoFunction As System.Windows.Forms.Label
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdStopRead = New System.Windows.Forms.Button
        Me.tmrReadInputs = New System.Windows.Forms.Timer(Me.components)
        Me._lblShowBitVal_0 = New System.Windows.Forms.Label
        Me._lblShowBitVal_1 = New System.Windows.Forms.Label
        Me._lblShowBitVal_2 = New System.Windows.Forms.Label
        Me._lblShowBitVal_3 = New System.Windows.Forms.Label
        Me._lblShowBitVal_4 = New System.Windows.Forms.Label
        Me._lblShowBitVal_5 = New System.Windows.Forms.Label
        Me._lblShowBitVal_6 = New System.Windows.Forms.Label
        Me._lblShowBitVal_7 = New System.Windows.Forms.Label
        Me.lblBitVal = New System.Windows.Forms.Label
        Me._lblShowBitNum_7 = New System.Windows.Forms.Label
        Me._lblShowBitNum_6 = New System.Windows.Forms.Label
        Me._lblShowBitNum_5 = New System.Windows.Forms.Label
        Me._lblShowBitNum_4 = New System.Windows.Forms.Label
        Me._lblShowBitNum_3 = New System.Windows.Forms.Label
        Me._lblShowBitNum_2 = New System.Windows.Forms.Label
        Me._lblShowBitNum_1 = New System.Windows.Forms.Label
        Me._lblShowBitNum_0 = New System.Windows.Forms.Label
        Me.lblBitNum = New System.Windows.Forms.Label
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
        Me.cmdStopRead.Location = New System.Drawing.Point(342, 191)
        Me.cmdStopRead.Name = "cmdStopRead"
        Me.cmdStopRead.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdStopRead.Size = New System.Drawing.Size(57, 29)
        Me.cmdStopRead.TabIndex = 20
        Me.cmdStopRead.Text = "Quit"
        Me.cmdStopRead.UseVisualStyleBackColor = False
        '
        'tmrReadInputs
        '
        Me.tmrReadInputs.Enabled = True
        Me.tmrReadInputs.Interval = 200
        '
        '_lblShowBitVal_0
        '
        Me._lblShowBitVal_0.BackColor = System.Drawing.SystemColors.Window
        Me._lblShowBitVal_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblShowBitVal_0.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblShowBitVal_0.ForeColor = System.Drawing.Color.Blue
        Me._lblShowBitVal_0.Location = New System.Drawing.Point(195, 151)
        Me._lblShowBitVal_0.Name = "_lblShowBitVal_0"
        Me._lblShowBitVal_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblShowBitVal_0.Size = New System.Drawing.Size(17, 17)
        Me._lblShowBitVal_0.TabIndex = 1
        Me._lblShowBitVal_0.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblShowBitVal_1
        '
        Me._lblShowBitVal_1.BackColor = System.Drawing.SystemColors.Window
        Me._lblShowBitVal_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblShowBitVal_1.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblShowBitVal_1.ForeColor = System.Drawing.Color.Blue
        Me._lblShowBitVal_1.Location = New System.Drawing.Point(219, 151)
        Me._lblShowBitVal_1.Name = "_lblShowBitVal_1"
        Me._lblShowBitVal_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblShowBitVal_1.Size = New System.Drawing.Size(17, 17)
        Me._lblShowBitVal_1.TabIndex = 2
        Me._lblShowBitVal_1.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblShowBitVal_2
        '
        Me._lblShowBitVal_2.BackColor = System.Drawing.SystemColors.Window
        Me._lblShowBitVal_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblShowBitVal_2.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblShowBitVal_2.ForeColor = System.Drawing.Color.Blue
        Me._lblShowBitVal_2.Location = New System.Drawing.Point(243, 151)
        Me._lblShowBitVal_2.Name = "_lblShowBitVal_2"
        Me._lblShowBitVal_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblShowBitVal_2.Size = New System.Drawing.Size(17, 17)
        Me._lblShowBitVal_2.TabIndex = 3
        Me._lblShowBitVal_2.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblShowBitVal_3
        '
        Me._lblShowBitVal_3.BackColor = System.Drawing.SystemColors.Window
        Me._lblShowBitVal_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblShowBitVal_3.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblShowBitVal_3.ForeColor = System.Drawing.Color.Blue
        Me._lblShowBitVal_3.Location = New System.Drawing.Point(267, 151)
        Me._lblShowBitVal_3.Name = "_lblShowBitVal_3"
        Me._lblShowBitVal_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblShowBitVal_3.Size = New System.Drawing.Size(17, 17)
        Me._lblShowBitVal_3.TabIndex = 4
        Me._lblShowBitVal_3.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblShowBitVal_4
        '
        Me._lblShowBitVal_4.BackColor = System.Drawing.SystemColors.Window
        Me._lblShowBitVal_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblShowBitVal_4.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblShowBitVal_4.ForeColor = System.Drawing.Color.Blue
        Me._lblShowBitVal_4.Location = New System.Drawing.Point(315, 151)
        Me._lblShowBitVal_4.Name = "_lblShowBitVal_4"
        Me._lblShowBitVal_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblShowBitVal_4.Size = New System.Drawing.Size(17, 17)
        Me._lblShowBitVal_4.TabIndex = 5
        Me._lblShowBitVal_4.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblShowBitVal_5
        '
        Me._lblShowBitVal_5.BackColor = System.Drawing.SystemColors.Window
        Me._lblShowBitVal_5.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblShowBitVal_5.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblShowBitVal_5.ForeColor = System.Drawing.Color.Blue
        Me._lblShowBitVal_5.Location = New System.Drawing.Point(339, 151)
        Me._lblShowBitVal_5.Name = "_lblShowBitVal_5"
        Me._lblShowBitVal_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblShowBitVal_5.Size = New System.Drawing.Size(17, 17)
        Me._lblShowBitVal_5.TabIndex = 6
        Me._lblShowBitVal_5.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblShowBitVal_6
        '
        Me._lblShowBitVal_6.BackColor = System.Drawing.SystemColors.Window
        Me._lblShowBitVal_6.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblShowBitVal_6.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblShowBitVal_6.ForeColor = System.Drawing.Color.Blue
        Me._lblShowBitVal_6.Location = New System.Drawing.Point(363, 151)
        Me._lblShowBitVal_6.Name = "_lblShowBitVal_6"
        Me._lblShowBitVal_6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblShowBitVal_6.Size = New System.Drawing.Size(17, 17)
        Me._lblShowBitVal_6.TabIndex = 7
        Me._lblShowBitVal_6.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblShowBitVal_7
        '
        Me._lblShowBitVal_7.BackColor = System.Drawing.SystemColors.Window
        Me._lblShowBitVal_7.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblShowBitVal_7.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblShowBitVal_7.ForeColor = System.Drawing.Color.Blue
        Me._lblShowBitVal_7.Location = New System.Drawing.Point(387, 151)
        Me._lblShowBitVal_7.Name = "_lblShowBitVal_7"
        Me._lblShowBitVal_7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblShowBitVal_7.Size = New System.Drawing.Size(17, 17)
        Me._lblShowBitVal_7.TabIndex = 0
        Me._lblShowBitVal_7.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblBitVal
        '
        Me.lblBitVal.BackColor = System.Drawing.SystemColors.Window
        Me.lblBitVal.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBitVal.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBitVal.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblBitVal.Location = New System.Drawing.Point(12, 151)
        Me.lblBitVal.Name = "lblBitVal"
        Me.lblBitVal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBitVal.Size = New System.Drawing.Size(168, 17)
        Me.lblBitVal.TabIndex = 8
        Me.lblBitVal.Text = "Bit Value:"
        Me.lblBitVal.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblShowBitNum_7
        '
        Me._lblShowBitNum_7.BackColor = System.Drawing.SystemColors.Window
        Me._lblShowBitNum_7.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblShowBitNum_7.Font = New System.Drawing.Font("Arial", 8.25!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblShowBitNum_7.ForeColor = System.Drawing.SystemColors.WindowText
        Me._lblShowBitNum_7.Location = New System.Drawing.Point(387, 127)
        Me._lblShowBitNum_7.Name = "_lblShowBitNum_7"
        Me._lblShowBitNum_7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblShowBitNum_7.Size = New System.Drawing.Size(17, 17)
        Me._lblShowBitNum_7.TabIndex = 17
        Me._lblShowBitNum_7.Text = "7"
        Me._lblShowBitNum_7.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblShowBitNum_6
        '
        Me._lblShowBitNum_6.BackColor = System.Drawing.SystemColors.Window
        Me._lblShowBitNum_6.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblShowBitNum_6.Font = New System.Drawing.Font("Arial", 8.25!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblShowBitNum_6.ForeColor = System.Drawing.SystemColors.WindowText
        Me._lblShowBitNum_6.Location = New System.Drawing.Point(363, 127)
        Me._lblShowBitNum_6.Name = "_lblShowBitNum_6"
        Me._lblShowBitNum_6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblShowBitNum_6.Size = New System.Drawing.Size(17, 17)
        Me._lblShowBitNum_6.TabIndex = 16
        Me._lblShowBitNum_6.Text = "6"
        Me._lblShowBitNum_6.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblShowBitNum_5
        '
        Me._lblShowBitNum_5.BackColor = System.Drawing.SystemColors.Window
        Me._lblShowBitNum_5.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblShowBitNum_5.Font = New System.Drawing.Font("Arial", 8.25!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblShowBitNum_5.ForeColor = System.Drawing.SystemColors.WindowText
        Me._lblShowBitNum_5.Location = New System.Drawing.Point(339, 127)
        Me._lblShowBitNum_5.Name = "_lblShowBitNum_5"
        Me._lblShowBitNum_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblShowBitNum_5.Size = New System.Drawing.Size(17, 17)
        Me._lblShowBitNum_5.TabIndex = 15
        Me._lblShowBitNum_5.Text = "5"
        Me._lblShowBitNum_5.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblShowBitNum_4
        '
        Me._lblShowBitNum_4.BackColor = System.Drawing.SystemColors.Window
        Me._lblShowBitNum_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblShowBitNum_4.Font = New System.Drawing.Font("Arial", 8.25!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblShowBitNum_4.ForeColor = System.Drawing.SystemColors.WindowText
        Me._lblShowBitNum_4.Location = New System.Drawing.Point(315, 127)
        Me._lblShowBitNum_4.Name = "_lblShowBitNum_4"
        Me._lblShowBitNum_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblShowBitNum_4.Size = New System.Drawing.Size(17, 17)
        Me._lblShowBitNum_4.TabIndex = 14
        Me._lblShowBitNum_4.Text = "4"
        Me._lblShowBitNum_4.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblShowBitNum_3
        '
        Me._lblShowBitNum_3.BackColor = System.Drawing.SystemColors.Window
        Me._lblShowBitNum_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblShowBitNum_3.Font = New System.Drawing.Font("Arial", 8.25!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblShowBitNum_3.ForeColor = System.Drawing.SystemColors.WindowText
        Me._lblShowBitNum_3.Location = New System.Drawing.Point(267, 127)
        Me._lblShowBitNum_3.Name = "_lblShowBitNum_3"
        Me._lblShowBitNum_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblShowBitNum_3.Size = New System.Drawing.Size(17, 17)
        Me._lblShowBitNum_3.TabIndex = 13
        Me._lblShowBitNum_3.Text = "3"
        Me._lblShowBitNum_3.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblShowBitNum_2
        '
        Me._lblShowBitNum_2.BackColor = System.Drawing.SystemColors.Window
        Me._lblShowBitNum_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblShowBitNum_2.Font = New System.Drawing.Font("Arial", 8.25!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblShowBitNum_2.ForeColor = System.Drawing.SystemColors.WindowText
        Me._lblShowBitNum_2.Location = New System.Drawing.Point(243, 127)
        Me._lblShowBitNum_2.Name = "_lblShowBitNum_2"
        Me._lblShowBitNum_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblShowBitNum_2.Size = New System.Drawing.Size(17, 17)
        Me._lblShowBitNum_2.TabIndex = 12
        Me._lblShowBitNum_2.Text = "2"
        Me._lblShowBitNum_2.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblShowBitNum_1
        '
        Me._lblShowBitNum_1.BackColor = System.Drawing.SystemColors.Window
        Me._lblShowBitNum_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblShowBitNum_1.Font = New System.Drawing.Font("Arial", 8.25!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblShowBitNum_1.ForeColor = System.Drawing.SystemColors.WindowText
        Me._lblShowBitNum_1.Location = New System.Drawing.Point(219, 127)
        Me._lblShowBitNum_1.Name = "_lblShowBitNum_1"
        Me._lblShowBitNum_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblShowBitNum_1.Size = New System.Drawing.Size(17, 17)
        Me._lblShowBitNum_1.TabIndex = 11
        Me._lblShowBitNum_1.Text = "1"
        Me._lblShowBitNum_1.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblShowBitNum_0
        '
        Me._lblShowBitNum_0.BackColor = System.Drawing.SystemColors.Window
        Me._lblShowBitNum_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblShowBitNum_0.Font = New System.Drawing.Font("Arial", 8.25!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblShowBitNum_0.ForeColor = System.Drawing.SystemColors.WindowText
        Me._lblShowBitNum_0.Location = New System.Drawing.Point(195, 127)
        Me._lblShowBitNum_0.Name = "_lblShowBitNum_0"
        Me._lblShowBitNum_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblShowBitNum_0.Size = New System.Drawing.Size(17, 17)
        Me._lblShowBitNum_0.TabIndex = 10
        Me._lblShowBitNum_0.Text = "0"
        Me._lblShowBitNum_0.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblBitNum
        '
        Me.lblBitNum.BackColor = System.Drawing.SystemColors.Window
        Me.lblBitNum.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBitNum.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBitNum.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblBitNum.Location = New System.Drawing.Point(12, 127)
        Me.lblBitNum.Name = "lblBitNum"
        Me.lblBitNum.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBitNum.Size = New System.Drawing.Size(168, 17)
        Me.lblBitNum.TabIndex = 9
        Me.lblBitNum.Text = "Bit Number:"
        Me.lblBitNum.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblInstruct
        '
        Me.lblInstruct.BackColor = System.Drawing.SystemColors.Window
        Me.lblInstruct.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblInstruct.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInstruct.ForeColor = System.Drawing.Color.Red
        Me.lblInstruct.Location = New System.Drawing.Point(26, 68)
        Me.lblInstruct.Name = "lblInstruct"
        Me.lblInstruct.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblInstruct.Size = New System.Drawing.Size(373, 46)
        Me.lblInstruct.TabIndex = 19
        Me.lblInstruct.Text = "Input a TTL logic level at digital inputs to change Bit Value:"
        Me.lblInstruct.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblDemoFunction
        '
        Me.lblDemoFunction.BackColor = System.Drawing.SystemColors.Window
        Me.lblDemoFunction.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDemoFunction.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDemoFunction.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblDemoFunction.Location = New System.Drawing.Point(17, 16)
        Me.lblDemoFunction.Name = "lblDemoFunction"
        Me.lblDemoFunction.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDemoFunction.Size = New System.Drawing.Size(387, 41)
        Me.lblDemoFunction.TabIndex = 18
        Me.lblDemoFunction.Text = "Demonstration of MccDaq.MccBoard.DBitIn()"
        Me.lblDemoFunction.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'frmDigIn
        '
        Me.AcceptButton = Me.cmdStopRead
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 13)
        Me.BackColor = System.Drawing.SystemColors.Window
        Me.ClientSize = New System.Drawing.Size(423, 232)
        Me.Controls.Add(Me.cmdStopRead)
        Me.Controls.Add(Me._lblShowBitVal_0)
        Me.Controls.Add(Me._lblShowBitVal_1)
        Me.Controls.Add(Me._lblShowBitVal_2)
        Me.Controls.Add(Me._lblShowBitVal_3)
        Me.Controls.Add(Me._lblShowBitVal_4)
        Me.Controls.Add(Me._lblShowBitVal_5)
        Me.Controls.Add(Me._lblShowBitVal_6)
        Me.Controls.Add(Me._lblShowBitVal_7)
        Me.Controls.Add(Me.lblBitVal)
        Me.Controls.Add(Me._lblShowBitNum_7)
        Me.Controls.Add(Me._lblShowBitNum_6)
        Me.Controls.Add(Me._lblShowBitNum_5)
        Me.Controls.Add(Me._lblShowBitNum_4)
        Me.Controls.Add(Me._lblShowBitNum_3)
        Me.Controls.Add(Me._lblShowBitNum_2)
        Me.Controls.Add(Me._lblShowBitNum_1)
        Me.Controls.Add(Me._lblShowBitNum_0)
        Me.Controls.Add(Me.lblBitNum)
        Me.Controls.Add(Me.lblInstruct)
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
        Me.lblShowBitNum.SetValue(_lblShowBitNum_7, 7)
        Me.lblShowBitNum.SetValue(_lblShowBitNum_6, 6)
        Me.lblShowBitNum.SetValue(_lblShowBitNum_5, 5)
        Me.lblShowBitNum.SetValue(_lblShowBitNum_4, 4)
        Me.lblShowBitNum.SetValue(_lblShowBitNum_3, 3)
        Me.lblShowBitNum.SetValue(_lblShowBitNum_2, 2)
        Me.lblShowBitNum.SetValue(_lblShowBitNum_1, 1)
        Me.lblShowBitNum.SetValue(_lblShowBitNum_0, 0)


        lblShowBitVal = New System.Windows.Forms.Label(8) {}

        Me.lblShowBitVal.SetValue(_lblShowBitVal_0, 0)
        Me.lblShowBitVal.SetValue(_lblShowBitVal_1, 1)
        Me.lblShowBitVal.SetValue(_lblShowBitVal_2, 2)
        Me.lblShowBitVal.SetValue(_lblShowBitVal_3, 3)
        Me.lblShowBitVal.SetValue(_lblShowBitVal_4, 4)
        Me.lblShowBitVal.SetValue(_lblShowBitVal_5, 5)
        Me.lblShowBitVal.SetValue(_lblShowBitVal_6, 6)
        Me.lblShowBitVal.SetValue(_lblShowBitVal_7, 7)

    End Sub

    Public lblShowBitNum As System.Windows.Forms.Label()
    Public lblShowBitVal As System.Windows.Forms.Label()

#End Region

End Class