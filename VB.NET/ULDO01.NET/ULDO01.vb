'==============================================================================

' File:                         ULDO01.VB

' Library Call Demonstrated:    MccDaq.MccBoard.DOut()

' Purpose:                      Writes a byte to digital output ports.

' Demonstration:                Configures the first digital port for output 
'                               (if necessary) and writes a value to the port.

' Other Library Calls:          MccDaq.MccBoard.DConfigPort()
'                               MccDaq.MccService.ErrHandling()

' Special Requirements:         Board 0 must have a digital output port
'                               or have digital ports programmable as output.

'==============================================================================
Option Strict Off
Option Explicit On

Public Class frmSetDigOut

    Inherits System.Windows.Forms.Form

    'Create a new MccBoard object for Board 0
    Private DaqBoard As MccDaq.MccBoard = New MccDaq.MccBoard(0)

    Private PortType As Integer
    Private PortNum As MccDaq.DigitalPortType
    Private NumPorts, NumBits, MaxPortVal As Integer
    Private ProgAbility As Integer

    Private Direction As MccDaq.DigitalPortDirection

    Private Sub frmSetDigOut_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim PortName As String
        Dim FirstBit As Integer
        Dim ULStat As MccDaq.ErrorInfo

        InitUL()    'initiate error handling, etc

        'determine if digital port exists, its capabilities, etc
        PortType = PORTOUT
        NumPorts = FindPortsOfType(DaqBoard, PortType, ProgAbility, PortNum, NumBits, FirstBit)

        If NumPorts < 1 Then
            lblInstruct.Text = "Board " & DaqBoard.BoardNum.ToString() & _
                " has no compatible digital ports."
            hsbSetDOutVal.Enabled = False
            txtValSet.Enabled = False
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
            lblInstruct.Text = "Set the output value of " & PortName & _
            " on board " & DaqBoard.BoardNum.ToString() & _
            " using the scroll bar or enter a value in the 'Value set' box."
            lblValSet.Text = "Value set at " & PortName & ":"
            lblDataValOut.Text = "Value written to " & PortName & ":"
        End If

    End Sub

    Private Sub hsbSetDOutVal_Change(ByVal newScrollValue As Integer)

        Dim ULStat As MccDaq.ErrorInfo
        Dim DataValue As UInt16

        ' get a value to write to the port

        If newScrollValue > UInt16.MaxValue Then newScrollValue = UInt16.MaxValue
        DataValue = Convert.ToUInt16(newScrollValue)
        txtValSet.Text = DataValue.ToString("0")

        ' write the value to the output port
        '  Parameters:
        '    PortNum    :the output port
        '    DataValue  :the value written to the port

        ULStat = DaqBoard.DOut(PortNum, DataValue)

        If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then
            Stop
        Else
            lblShowValOut.Text = DataValue.ToString("0")
        End If

    End Sub

    Private Sub txtValSet_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtValSet.KeyUp

        Dim hsbVal As Integer = Integer.Parse(txtValSet.Text)
        hsbSetDOutVal_Change(hsbVal)

    End Sub

    Private Sub hsbSetDOutVal_Scroll(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.ScrollEventArgs) Handles hsbSetDOutVal.Scroll

        Select Case eventArgs.Type
            Case System.Windows.Forms.ScrollEventType.EndScroll
                hsbSetDOutVal_Change(eventArgs.NewValue)
        End Select

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
    Public WithEvents txtValSet As System.Windows.Forms.TextBox
    Public WithEvents hsbSetDOutVal As System.Windows.Forms.HScrollBar
    Public WithEvents lblShowValOut As System.Windows.Forms.Label
    Public WithEvents lblDataValOut As System.Windows.Forms.Label
    Public WithEvents lblValSet As System.Windows.Forms.Label
    Public WithEvents lblInstruct As System.Windows.Forms.Label
    Public WithEvents lblDemoFunction As System.Windows.Forms.Label
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdEndProgram = New System.Windows.Forms.Button
        Me.txtValSet = New System.Windows.Forms.TextBox
        Me.hsbSetDOutVal = New System.Windows.Forms.HScrollBar
        Me.lblShowValOut = New System.Windows.Forms.Label
        Me.lblDataValOut = New System.Windows.Forms.Label
        Me.lblValSet = New System.Windows.Forms.Label
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
        Me.cmdEndProgram.Location = New System.Drawing.Point(248, 216)
        Me.cmdEndProgram.Name = "cmdEndProgram"
        Me.cmdEndProgram.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEndProgram.Size = New System.Drawing.Size(57, 33)
        Me.cmdEndProgram.TabIndex = 7
        Me.cmdEndProgram.Text = "Quit"
        Me.cmdEndProgram.UseVisualStyleBackColor = False
        '
        'txtValSet
        '
        Me.txtValSet.AcceptsReturn = True
        Me.txtValSet.BackColor = System.Drawing.SystemColors.Window
        Me.txtValSet.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtValSet.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtValSet.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtValSet.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtValSet.Location = New System.Drawing.Point(227, 143)
        Me.txtValSet.MaxLength = 0
        Me.txtValSet.Name = "txtValSet"
        Me.txtValSet.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtValSet.Size = New System.Drawing.Size(41, 20)
        Me.txtValSet.TabIndex = 4
        Me.txtValSet.Text = "0"
        '
        'hsbSetDOutVal
        '
        Me.hsbSetDOutVal.Cursor = System.Windows.Forms.Cursors.Default
        Me.hsbSetDOutVal.LargeChange = 51
        Me.hsbSetDOutVal.Location = New System.Drawing.Point(59, 107)
        Me.hsbSetDOutVal.Maximum = 305
        Me.hsbSetDOutVal.Name = "hsbSetDOutVal"
        Me.hsbSetDOutVal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.hsbSetDOutVal.Size = New System.Drawing.Size(190, 16)
        Me.hsbSetDOutVal.TabIndex = 1
        Me.hsbSetDOutVal.TabStop = True
        '
        'lblShowValOut
        '
        Me.lblShowValOut.BackColor = System.Drawing.SystemColors.Window
        Me.lblShowValOut.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblShowValOut.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShowValOut.ForeColor = System.Drawing.Color.Blue
        Me.lblShowValOut.Location = New System.Drawing.Point(228, 170)
        Me.lblShowValOut.Name = "lblShowValOut"
        Me.lblShowValOut.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblShowValOut.Size = New System.Drawing.Size(57, 17)
        Me.lblShowValOut.TabIndex = 3
        '
        'lblDataValOut
        '
        Me.lblDataValOut.BackColor = System.Drawing.SystemColors.Window
        Me.lblDataValOut.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDataValOut.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDataValOut.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblDataValOut.Location = New System.Drawing.Point(36, 170)
        Me.lblDataValOut.Name = "lblDataValOut"
        Me.lblDataValOut.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDataValOut.Size = New System.Drawing.Size(185, 17)
        Me.lblDataValOut.TabIndex = 2
        Me.lblDataValOut.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblValSet
        '
        Me.lblValSet.BackColor = System.Drawing.SystemColors.Window
        Me.lblValSet.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblValSet.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblValSet.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblValSet.Location = New System.Drawing.Point(41, 143)
        Me.lblValSet.Name = "lblValSet"
        Me.lblValSet.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblValSet.Size = New System.Drawing.Size(181, 17)
        Me.lblValSet.TabIndex = 6
        Me.lblValSet.Text = "Value set:"
        Me.lblValSet.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblInstruct
        '
        Me.lblInstruct.BackColor = System.Drawing.SystemColors.Window
        Me.lblInstruct.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblInstruct.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInstruct.ForeColor = System.Drawing.Color.Red
        Me.lblInstruct.Location = New System.Drawing.Point(56, 39)
        Me.lblInstruct.Name = "lblInstruct"
        Me.lblInstruct.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblInstruct.Size = New System.Drawing.Size(201, 48)
        Me.lblInstruct.TabIndex = 5
        Me.lblInstruct.Text = "Set output value using scroll bar or enter value in Value Set box."
        Me.lblInstruct.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblDemoFunction
        '
        Me.lblDemoFunction.BackColor = System.Drawing.SystemColors.Window
        Me.lblDemoFunction.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDemoFunction.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDemoFunction.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblDemoFunction.Location = New System.Drawing.Point(24, 8)
        Me.lblDemoFunction.Name = "lblDemoFunction"
        Me.lblDemoFunction.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDemoFunction.Size = New System.Drawing.Size(289, 25)
        Me.lblDemoFunction.TabIndex = 0
        Me.lblDemoFunction.Text = "Demonstration of MccDaq.MccBoard.DOut()"
        Me.lblDemoFunction.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'frmSetDigOut
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 13)
        Me.BackColor = System.Drawing.SystemColors.Window
        Me.ClientSize = New System.Drawing.Size(331, 267)
        Me.Controls.Add(Me.cmdEndProgram)
        Me.Controls.Add(Me.txtValSet)
        Me.Controls.Add(Me.hsbSetDOutVal)
        Me.Controls.Add(Me.lblShowValOut)
        Me.Controls.Add(Me.lblDataValOut)
        Me.Controls.Add(Me.lblValSet)
        Me.Controls.Add(Me.lblInstruct)
        Me.Controls.Add(Me.lblDemoFunction)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Location = New System.Drawing.Point(7, 103)
        Me.Name = "frmSetDigOut"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Universal Library Digital Output"
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