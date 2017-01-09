'==============================================================================

' File:                         ULDO02.VB

' Library Call Demonstrated:    MccDaq.MccBoard.DBitOut()

' Purpose:                      Sets the state of a single digital output bit.

' Demonstration:                Configures the first digital bit for output
'                               (if necessary) and writes a value to the bit.

' Other Library Calls:          MccDaq.MccBoard.DConfigPort()
'                               MccDaq.MccService.ErrHandling()

' Special Requirements:         Board 0 must have a digital output port
'                               or have digital ports programmable as output.

'==============================================================================
Option Strict Off
Option Explicit On

Friend Class frmSetBitOut

    Inherits System.Windows.Forms.Form

    'Create a new MccBoard object for Board 0
    Private DaqBoard As MccDaq.MccBoard = New MccDaq.MccBoard(0)

    Private PortType As MccDaq.DigitalPortType
    Private PortNum As MccDaq.DigitalPortType
    Private NumPorts, NumBits, FirstBit As Integer
    Private ProgAbility As Integer
    Public WithEvents lblValueSet As System.Windows.Forms.Label

    Private Direction As MccDaq.DigitalPortDirection

    Private Sub frmSetBitOut_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim PortName As String
        Dim ULStat As MccDaq.ErrorInfo

        InitUL()    'initiate error handling, etc

        'determine if digital port exists, its capabilities, etc
        PortType = PORTOUT
        NumPorts = FindPortsOfType(DaqBoard, PortType, ProgAbility, PortNum, NumBits, FirstBit)

        If NumBits > 8 Then NumBits = 8
        For I As Integer = NumBits To 7
            chkSetBit(I).Visible = False
        Next I
        If NumPorts < 1 Then
            lblInstruct.Text = "Board " & DaqBoard.BoardNum.ToString() & _
                " has no compatible digital bits."
        Else
            ' if programmable, set direction of port to output
            ' configure the first port for digital output
            '  Parameters:
            '    PortNum        :the output port
            '    Direction      :sets the port for input or output

            If ProgAbility = DigitalIO.PROGPORT Then
                Direction = MccDaq.DigitalPortDirection.DigitalOut
                ULStat = DaqBoard.DConfigPort(PortNum, Direction)
                If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then Stop
            End If
            PortName = PortNum.ToString
            lblInstruct.Text = "Set the output value of " & _
            PortName & " on board " & DaqBoard.BoardNum.ToString() & _
            " bits using the check boxes."
        End If

    End Sub

    Private Sub chkSetBit_CheckStateChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs)

        Dim Index As Integer = Array.IndexOf(chkSetBit, eventSender)
        Dim ULStat As MccDaq.ErrorInfo
        Dim BitValue As MccDaq.DigitalLogicState
        Dim BitPort As MccDaq.DigitalPortType
        Dim BitNum As Integer
        Dim PortName, BitName As String

        BitNum = Index

        If (chkSetBit(Index).Checked) Then
            BitValue = MccDaq.DigitalLogicState.High
        Else
            BitValue = MccDaq.DigitalLogicState.Low
        End If


        BitPort = MccDaq.DigitalPortType.AuxPort
        If PortNum > MccDaq.DigitalPortType.AuxPort _
        Then BitPort = MccDaq.DigitalPortType.FirstPortA

        ULStat = DaqBoard.DBitOut(BitPort, FirstBit + BitNum, BitValue)
        If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then Stop

        PortName = BitPort.ToString
        BitName = Format(FirstBit + BitNum, "0")
        lblValueSet.Text = PortName$ & ", bit " & _
        BitName$ & " value set to " & BitValue.ToString()

    End Sub

    Private Sub cmdEndProgram_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdEndProgram.Click

        Dim ULStat As MccDaq.ErrorInfo
        Dim DataValue As UInt16

        If ProgAbility = DigitalIO.PROGPORT Then
            DataValue = Convert.ToUInt16(0)

            ULStat = DaqBoard.DOut(PortNum, DataValue)
            If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then Stop

            Direction = MccDaq.DigitalPortDirection.DigitalIn
            ULStat = DaqBoard.DConfigPort(PortNum, Direction)
            If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then Stop
        End If

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
    Public WithEvents _chkSetBit_7 As System.Windows.Forms.CheckBox
    Public WithEvents _chkSetBit_3 As System.Windows.Forms.CheckBox
    Public WithEvents _chkSetBit_6 As System.Windows.Forms.CheckBox
    Public WithEvents _chkSetBit_2 As System.Windows.Forms.CheckBox
    Public WithEvents _chkSetBit_5 As System.Windows.Forms.CheckBox
    Public WithEvents _chkSetBit_1 As System.Windows.Forms.CheckBox
    Public WithEvents _chkSetBit_4 As System.Windows.Forms.CheckBox
    Public WithEvents _chkSetBit_0 As System.Windows.Forms.CheckBox
    Public WithEvents lblDemoFunction As System.Windows.Forms.Label
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdEndProgram = New System.Windows.Forms.Button
        Me._chkSetBit_7 = New System.Windows.Forms.CheckBox
        Me._chkSetBit_3 = New System.Windows.Forms.CheckBox
        Me._chkSetBit_6 = New System.Windows.Forms.CheckBox
        Me._chkSetBit_2 = New System.Windows.Forms.CheckBox
        Me._chkSetBit_5 = New System.Windows.Forms.CheckBox
        Me._chkSetBit_1 = New System.Windows.Forms.CheckBox
        Me._chkSetBit_4 = New System.Windows.Forms.CheckBox
        Me._chkSetBit_0 = New System.Windows.Forms.CheckBox
        Me.lblDemoFunction = New System.Windows.Forms.Label
        Me.lblInstruct = New System.Windows.Forms.Label
        Me.lblValueSet = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'cmdEndProgram
        '
        Me.cmdEndProgram.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEndProgram.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEndProgram.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEndProgram.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEndProgram.Location = New System.Drawing.Point(251, 242)
        Me.cmdEndProgram.Name = "cmdEndProgram"
        Me.cmdEndProgram.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEndProgram.Size = New System.Drawing.Size(57, 25)
        Me.cmdEndProgram.TabIndex = 0
        Me.cmdEndProgram.Text = "Quit"
        Me.cmdEndProgram.UseVisualStyleBackColor = False
        '
        '_chkSetBit_7
        '
        Me._chkSetBit_7.BackColor = System.Drawing.SystemColors.Window
        Me._chkSetBit_7.Checked = True
        Me._chkSetBit_7.CheckState = System.Windows.Forms.CheckState.Indeterminate
        Me._chkSetBit_7.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkSetBit_7.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkSetBit_7.ForeColor = System.Drawing.SystemColors.WindowText
        Me._chkSetBit_7.Location = New System.Drawing.Point(192, 157)
        Me._chkSetBit_7.Name = "_chkSetBit_7"
        Me._chkSetBit_7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkSetBit_7.Size = New System.Drawing.Size(81, 17)
        Me._chkSetBit_7.TabIndex = 8
        Me._chkSetBit_7.Text = "Set bit 7"
        Me._chkSetBit_7.UseVisualStyleBackColor = False
        '
        '_chkSetBit_3
        '
        Me._chkSetBit_3.BackColor = System.Drawing.SystemColors.Window
        Me._chkSetBit_3.Checked = True
        Me._chkSetBit_3.CheckState = System.Windows.Forms.CheckState.Indeterminate
        Me._chkSetBit_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkSetBit_3.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkSetBit_3.ForeColor = System.Drawing.SystemColors.WindowText
        Me._chkSetBit_3.Location = New System.Drawing.Point(48, 157)
        Me._chkSetBit_3.Name = "_chkSetBit_3"
        Me._chkSetBit_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkSetBit_3.Size = New System.Drawing.Size(81, 17)
        Me._chkSetBit_3.TabIndex = 4
        Me._chkSetBit_3.Text = "Set bit 3"
        Me._chkSetBit_3.UseVisualStyleBackColor = False
        '
        '_chkSetBit_6
        '
        Me._chkSetBit_6.BackColor = System.Drawing.SystemColors.Window
        Me._chkSetBit_6.Checked = True
        Me._chkSetBit_6.CheckState = System.Windows.Forms.CheckState.Indeterminate
        Me._chkSetBit_6.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkSetBit_6.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkSetBit_6.ForeColor = System.Drawing.SystemColors.WindowText
        Me._chkSetBit_6.Location = New System.Drawing.Point(192, 133)
        Me._chkSetBit_6.Name = "_chkSetBit_6"
        Me._chkSetBit_6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkSetBit_6.Size = New System.Drawing.Size(81, 17)
        Me._chkSetBit_6.TabIndex = 7
        Me._chkSetBit_6.Text = "Set bit 6"
        Me._chkSetBit_6.UseVisualStyleBackColor = False
        '
        '_chkSetBit_2
        '
        Me._chkSetBit_2.BackColor = System.Drawing.SystemColors.Window
        Me._chkSetBit_2.Checked = True
        Me._chkSetBit_2.CheckState = System.Windows.Forms.CheckState.Indeterminate
        Me._chkSetBit_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkSetBit_2.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkSetBit_2.ForeColor = System.Drawing.SystemColors.WindowText
        Me._chkSetBit_2.Location = New System.Drawing.Point(48, 133)
        Me._chkSetBit_2.Name = "_chkSetBit_2"
        Me._chkSetBit_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkSetBit_2.Size = New System.Drawing.Size(81, 17)
        Me._chkSetBit_2.TabIndex = 3
        Me._chkSetBit_2.Text = "Set bit 2"
        Me._chkSetBit_2.UseVisualStyleBackColor = False
        '
        '_chkSetBit_5
        '
        Me._chkSetBit_5.BackColor = System.Drawing.SystemColors.Window
        Me._chkSetBit_5.Checked = True
        Me._chkSetBit_5.CheckState = System.Windows.Forms.CheckState.Indeterminate
        Me._chkSetBit_5.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkSetBit_5.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkSetBit_5.ForeColor = System.Drawing.SystemColors.WindowText
        Me._chkSetBit_5.Location = New System.Drawing.Point(192, 109)
        Me._chkSetBit_5.Name = "_chkSetBit_5"
        Me._chkSetBit_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkSetBit_5.Size = New System.Drawing.Size(81, 17)
        Me._chkSetBit_5.TabIndex = 6
        Me._chkSetBit_5.Text = "Set bit 5"
        Me._chkSetBit_5.UseVisualStyleBackColor = False
        '
        '_chkSetBit_1
        '
        Me._chkSetBit_1.BackColor = System.Drawing.SystemColors.Window
        Me._chkSetBit_1.Checked = True
        Me._chkSetBit_1.CheckState = System.Windows.Forms.CheckState.Indeterminate
        Me._chkSetBit_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkSetBit_1.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkSetBit_1.ForeColor = System.Drawing.SystemColors.WindowText
        Me._chkSetBit_1.Location = New System.Drawing.Point(48, 109)
        Me._chkSetBit_1.Name = "_chkSetBit_1"
        Me._chkSetBit_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkSetBit_1.Size = New System.Drawing.Size(81, 17)
        Me._chkSetBit_1.TabIndex = 2
        Me._chkSetBit_1.Text = "Set bit 1"
        Me._chkSetBit_1.UseVisualStyleBackColor = False
        '
        '_chkSetBit_4
        '
        Me._chkSetBit_4.BackColor = System.Drawing.SystemColors.Window
        Me._chkSetBit_4.Checked = True
        Me._chkSetBit_4.CheckState = System.Windows.Forms.CheckState.Indeterminate
        Me._chkSetBit_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkSetBit_4.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkSetBit_4.ForeColor = System.Drawing.SystemColors.WindowText
        Me._chkSetBit_4.Location = New System.Drawing.Point(192, 85)
        Me._chkSetBit_4.Name = "_chkSetBit_4"
        Me._chkSetBit_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkSetBit_4.Size = New System.Drawing.Size(81, 17)
        Me._chkSetBit_4.TabIndex = 5
        Me._chkSetBit_4.Text = "Set bit 4"
        Me._chkSetBit_4.UseVisualStyleBackColor = False
        '
        '_chkSetBit_0
        '
        Me._chkSetBit_0.BackColor = System.Drawing.SystemColors.Window
        Me._chkSetBit_0.Checked = True
        Me._chkSetBit_0.CheckState = System.Windows.Forms.CheckState.Indeterminate
        Me._chkSetBit_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkSetBit_0.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkSetBit_0.ForeColor = System.Drawing.SystemColors.WindowText
        Me._chkSetBit_0.Location = New System.Drawing.Point(48, 85)
        Me._chkSetBit_0.Name = "_chkSetBit_0"
        Me._chkSetBit_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkSetBit_0.Size = New System.Drawing.Size(81, 17)
        Me._chkSetBit_0.TabIndex = 1
        Me._chkSetBit_0.Text = "Set bit 0"
        Me._chkSetBit_0.UseVisualStyleBackColor = False
        '
        'lblDemoFunction
        '
        Me.lblDemoFunction.BackColor = System.Drawing.SystemColors.Window
        Me.lblDemoFunction.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDemoFunction.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDemoFunction.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblDemoFunction.Location = New System.Drawing.Point(16, 5)
        Me.lblDemoFunction.Name = "lblDemoFunction"
        Me.lblDemoFunction.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDemoFunction.Size = New System.Drawing.Size(305, 25)
        Me.lblDemoFunction.TabIndex = 0
        Me.lblDemoFunction.Text = "Demonstration of MccDaq.MccBoard.DBitOut()"
        Me.lblDemoFunction.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblInstruct
        '
        Me.lblInstruct.BackColor = System.Drawing.SystemColors.Window
        Me.lblInstruct.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblInstruct.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInstruct.ForeColor = System.Drawing.Color.Red
        Me.lblInstruct.Location = New System.Drawing.Point(17, 37)
        Me.lblInstruct.Name = "lblInstruct"
        Me.lblInstruct.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblInstruct.Size = New System.Drawing.Size(305, 46)
        Me.lblInstruct.TabIndex = 10
        Me.lblInstruct.Text = "Monitor the bit values at digital output port."
        Me.lblInstruct.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblValueSet
        '
        Me.lblValueSet.BackColor = System.Drawing.SystemColors.Window
        Me.lblValueSet.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblValueSet.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblValueSet.ForeColor = System.Drawing.Color.Blue
        Me.lblValueSet.Location = New System.Drawing.Point(17, 195)
        Me.lblValueSet.Name = "lblValueSet"
        Me.lblValueSet.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblValueSet.Size = New System.Drawing.Size(305, 18)
        Me.lblValueSet.TabIndex = 11
        Me.lblValueSet.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'frmSetBitOut
        '
        Me.AcceptButton = Me.cmdEndProgram
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 13)
        Me.BackColor = System.Drawing.SystemColors.Window
        Me.ClientSize = New System.Drawing.Size(339, 290)
        Me.Controls.Add(Me.lblValueSet)
        Me.Controls.Add(Me.lblInstruct)
        Me.Controls.Add(Me.cmdEndProgram)
        Me.Controls.Add(Me._chkSetBit_7)
        Me.Controls.Add(Me._chkSetBit_3)
        Me.Controls.Add(Me._chkSetBit_6)
        Me.Controls.Add(Me._chkSetBit_2)
        Me.Controls.Add(Me._chkSetBit_5)
        Me.Controls.Add(Me._chkSetBit_1)
        Me.Controls.Add(Me._chkSetBit_4)
        Me.Controls.Add(Me._chkSetBit_0)
        Me.Controls.Add(Me.lblDemoFunction)
        Me.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Location = New System.Drawing.Point(7, 103)
        Me.Name = "frmSetBitOut"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Universal Library Digital Bit Out"
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

        ReportError = MccDaq.ErrorReporting.PrintAll
        HandleError = MccDaq.ErrorHandling.StopAll
        ULStat = MccDaq.MccService.ErrHandling(ReportError, HandleError)
        If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then
            Stop
        End If

        AddHandler _chkSetBit_7.CheckStateChanged, AddressOf chkSetBit_CheckStateChanged
        AddHandler _chkSetBit_6.CheckStateChanged, AddressOf chkSetBit_CheckStateChanged
        AddHandler _chkSetBit_5.CheckStateChanged, AddressOf chkSetBit_CheckStateChanged
        AddHandler _chkSetBit_4.CheckStateChanged, AddressOf chkSetBit_CheckStateChanged
        AddHandler _chkSetBit_3.CheckStateChanged, AddressOf chkSetBit_CheckStateChanged
        AddHandler _chkSetBit_2.CheckStateChanged, AddressOf chkSetBit_CheckStateChanged
        AddHandler _chkSetBit_1.CheckStateChanged, AddressOf chkSetBit_CheckStateChanged
        AddHandler _chkSetBit_0.CheckStateChanged, AddressOf chkSetBit_CheckStateChanged

        chkSetBit = New System.Windows.Forms.CheckBox(8) {}
        Me.chkSetBit.SetValue(_chkSetBit_7, 7)
        Me.chkSetBit.SetValue(_chkSetBit_3, 3)
        Me.chkSetBit.SetValue(_chkSetBit_6, 6)
        Me.chkSetBit.SetValue(_chkSetBit_2, 2)
        Me.chkSetBit.SetValue(_chkSetBit_5, 5)
        Me.chkSetBit.SetValue(_chkSetBit_1, 1)
        Me.chkSetBit.SetValue(_chkSetBit_4, 4)
        Me.chkSetBit.SetValue(_chkSetBit_0, 0)

    End Sub

    Public chkSetBit As System.Windows.Forms.CheckBox()
    Public WithEvents lblInstruct As System.Windows.Forms.Label

#End Region

End Class