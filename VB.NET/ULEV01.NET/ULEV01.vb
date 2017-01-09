'==============================================================================


' File:                         ULEV01.VB

' Library Call Demonstrated:    Mccdaq.MccBoard.EnableEvent with EventType:
'                                   - MccDaq.EventType.OnExternalInterrupt
'
'                               Mccdaq.MccBoard.DisableEvent()
'
' Purpose:                      Generates an event for each pulse set at a
'                               digital or counter External Interrupt pin,
'                               and reads the digital input at FirstPortA
'                               every UpdateSize events.
'
'
' Demonstration:                Shows how to enable and respond to events.

' Other Library Calls:          MccDaq.MccService.ErrHandling()
'                               Mccdaq.MccBoard.DConfigPort()
'                               Mccdaq.MccBoard.DIn()
'
' Special Requirements:         Board 0 must have an external interrupt pin
'                               and support the MccDaq.EventType.OnExternalInterrupt event.
'
'==============================================================================

Option Strict Off
Option Explicit On 
Imports System.Runtime.InteropServices

<StructLayout(LayoutKind.Sequential)> _
Public Structure UserData
    Public ThisObj As Object
End Structure

Public Class frmEventDisplay

    Inherits System.Windows.Forms.Form

    'Create a new MccBoard object for Board 0
    Private DaqBoard As MccDaq.MccBoard = New MccDaq.MccBoard(0)

    Const NumPoints As Short = 10000     ' number of data points to collect
    Const SampleRate As Short = 1000     ' rate at which to sample each channel

    ' Specify the frequency for reading the digital port and updating the display
    Const UpdateSize As Short = 10

    Private PortNum As MccDaq.DigitalPortType
    Private NumPorts, NumBits, FirstBit As Integer
    Private PortType, ProgAbility As Integer
    Private NumEvents As Integer

    Private Direction As MccDaq.DigitalPortDirection
    Dim ptrMyCallback As MccDaq.EventCallback

    Dim EventCount As Long ' number of events handled since enabling events
    Dim UpdateCount As Long
    Public WithEvents lblInstruction As System.Windows.Forms.Label
    Public WithEvents lblDemoFunction As System.Windows.Forms.Label

    Dim WithEvents frmEventDisplay As System.Windows.Forms.Form

    Private Sub frmEventDisplay_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim ChannelType, EventType As Integer
        Dim PortName As String
        Dim ULStat As MccDaq.ErrorInfo

        InitUL()

        ChannelType = PORTIN
        'determine if digital port exists, its capabilities, etc
        PortType = PORTIN
        NumPorts = FindPortsOfType(DaqBoard, PortType, ProgAbility, PortNum, NumBits, FirstBit)

        EventType = INTEVENT
        NumEvents = FindEventsOfType(DaqBoard, EventType)
        If (NumPorts < 1) Then
            Me.lblInstruction.Text = _
                "There are no compatible digital ports on board " _
                & DaqBoard.BoardNum.ToString() & "."
        ElseIf (NumEvents <> EventType) Then
            Me.lblInstruction.Text = "Board " & _
                DaqBoard.BoardNum.ToString() & _
                " doesn't support the specified event types."
        Else
            ' if programmable, set direction of port to input
            ' configure the first port for digital input
            '  Parameters:
            '    PortNum        :the input port
            '//    Direction      :sets the port for input or output

            If ProgAbility = DigitalIO.PROGPORT Then
                Direction = MccDaq.DigitalPortDirection.DigitalIn
                ULStat = DaqBoard.DConfigPort(PortNum, Direction)
                If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then Stop
            End If
            PortName = PortNum.ToString
            lblInstruction.Text = "Trigger events by supplying a TTL " & _
            "pulse on the interrupt input of board " & DaqBoard.BoardNum.ToString() & _
            ". A read of digital inputs on " & PortName & " is done every " & _
            Format(UpdateSize, "0") & " interrupts."
            Me.cmdDisableEvent.Enabled = True
            Me.cmdEnableEvent.Enabled = True
        End If

    End Sub

    Public Sub OnEvent(ByVal bd As Short, _
        ByVal EventType As Integer, ByVal InterruptCount As Long)

        ' This gets called by MyCallback in mycallback.vb for
        ' each MccDaq.EventType.OnExternalInterrupt event. For 
        ' this event type, the EventData supplied curresponds
        ' to the number of interrupts that occurred since the
        ' event was last enabled.

        Dim ULStat As MccDaq.ErrorInfo
        Dim DigitalData As Short           ' digital input from FIRSTPORTA
        Dim InterruptsMissed As Long     ' number of interrupts missed since enabling events.

        EventCount += 1

        ' We only update the display every 'UpdateSize' events since the work below
        ' is "expensive." The longer we spend in this handler and the more frequent
        ' the interrupts occur, the more likely we'll miss interrupts.
        If (EventCount >= UpdateCount) Then
            UpdateCount = UpdateCount + UpdateSize
            InterruptsMissed = InterruptCount - EventCount

            lblInterruptCount.Text = Str(InterruptCount)
            lblInterruptsMissed.Text = Str(InterruptsMissed)

            ' read MccDaq.DigitalPortType.FirstPortA digital input and display
            '
            ' Parameters:
            '   PortNum      :the input port
            '   DigitalData  :the value read from the port
            ULStat = DaqBoard.DIn(PortNum, DigitalData)
            If ULStat.Value = MccDaq.ErrorInfo.ErrorCode.NoErrors _
                Then lblDigitalIn.Text = Hex(DigitalData)

        End If
        lblEventCount.Text = Str(EventCount)

    End Sub

    Sub OnScanError(ByVal bd As Short, ByVal EventType As Integer, ByVal InterruptCount As Integer)

        ' this is just a placeholder referenced in mycallback.bas

    End Sub

    Private Sub cmdDisableEvent_Click(ByVal eventSender _
        As System.Object, ByVal eventArgs As System.EventArgs) _
        Handles cmdDisableEvent.Click

        Dim ULStat As MccDaq.ErrorInfo

        ' Disable and disconnect all event types with MccDaq.MccBoard.DisableEvent()
        '
        ' Since disabling events that were never enabled is harmless,
        ' we can disable all the events at once.
        '
        ' Parameters:
        '   MccDaq.EventType.AllEventTypes  :all event types will be disabled
        ULStat = DaqBoard.DisableEvent(MccDaq.EventType.AllEventTypes)

    End Sub

    Public Sub cmdEnableEvent_Click(ByVal eventSender As System.Object, _
        ByVal eventArgs As System.EventArgs) Handles cmdEnableEvent.Click

        Dim ULStat As MccDaq.ErrorInfo
        Dim EventType As MccDaq.EventType
        Dim EventSize As Integer
        Dim userData As UserData
        Dim ptrUserData As IntPtr

        userData.ThisObj = frmEventDisplay

        ' Allocate a block of memory of UserData struct size 
        ptrUserData = Marshal.AllocCoTaskMem(Marshal.SizeOf(userData))
        Marshal.StructureToPtr(userData, ptrUserData, False)

        ' Enable and connect one or more event types to a single user callback
        ' function using MccDaq.MccBoard.EnableEvent().
        '
        ' Parameters:
        '   EventType   :the condition that will cause an event to fire
        '   EventSize   : not used for this event type
        '   AddressOf MyCallback  :the address of the user function or event handler
        '                          to call when above event type occurs
        '   frmEventDisplay        :to make sure that this form handles the event,
        '                          we supply a reference to it by name and dereference
        '                          it in the event handler. Note that the UserData type
        '                          in the event handler must match.
        EventType = MccDaq.EventType.OnExternalInterrupt ' event from external interrupt pin
        EventSize = 0               ' not used for this event type

        ULStat = DaqBoard.EnableEvent(EventType, EventSize, ptrMyCallback, ptrUserData)
        If ULStat.Value = MccDaq.ErrorInfo.ErrorCode.NoErrors Then
            ' reset all counts and displays
            EventCount = 0
            UpdateCount = UpdateSize

            lblEventCount.Text = Str(EventCount)
            lblInterruptCount.Text = "0"
            lblDigitalIn.Text = "NA"
            lblInterruptsMissed.Text = "0"

        End If

    End Sub

    Private Sub frmEventDisplay_Closed(ByVal eventSender As System.Object, _
        ByVal eventArgs As System.EventArgs) Handles MyBase.Closed

        Dim ULStat As MccDaq.ErrorInfo

        If Not GeneralError Then
            'Make sure all events are disabled before exiting
            ' Disable and disconnect all event types with MccDaq.MccBoard.DisableEvent()
            '
            ' Since disabling events that were never enabled is harmless,
            ' we can disable all the events at once.
            '
            ' Parameters:
            '   MccDaq.EventType.AllEventTypes  :all event types will be disabled
            If Me.cmdDisableEvent.Enabled Then _
                ULStat = DaqBoard.DisableEvent(MccDaq.EventType.AllEventTypes)
        End If

    End Sub

#Region "Windows Form Designer generated code "
    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        frmEventDisplay = Me

        ptrMyCallback = New MccDaq.EventCallback(AddressOf MyCallback)

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
    Public WithEvents cmdDisableEvent As System.Windows.Forms.Button
    Public WithEvents cmdEnableEvent As System.Windows.Forms.Button
    Public WithEvents Label4 As System.Windows.Forms.Label
    Public WithEvents Label3 As System.Windows.Forms.Label
    Public WithEvents Label2 As System.Windows.Forms.Label
    Public WithEvents label1 As System.Windows.Forms.Label
    Public WithEvents lblInterruptsMissed As System.Windows.Forms.Label
    Public WithEvents lblDigitalIn As System.Windows.Forms.Label
    Public WithEvents lblEventCount As System.Windows.Forms.Label
    Public WithEvents lblInterruptCount As System.Windows.Forms.Label
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdDisableEvent = New System.Windows.Forms.Button
        Me.cmdEnableEvent = New System.Windows.Forms.Button
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.label1 = New System.Windows.Forms.Label
        Me.lblInterruptsMissed = New System.Windows.Forms.Label
        Me.lblDigitalIn = New System.Windows.Forms.Label
        Me.lblEventCount = New System.Windows.Forms.Label
        Me.lblInterruptCount = New System.Windows.Forms.Label
        Me.lblInstruction = New System.Windows.Forms.Label
        Me.lblDemoFunction = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'cmdDisableEvent
        '
        Me.cmdDisableEvent.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDisableEvent.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDisableEvent.Enabled = False
        Me.cmdDisableEvent.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDisableEvent.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDisableEvent.Location = New System.Drawing.Point(8, 169)
        Me.cmdDisableEvent.Name = "cmdDisableEvent"
        Me.cmdDisableEvent.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDisableEvent.Size = New System.Drawing.Size(139, 47)
        Me.cmdDisableEvent.TabIndex = 4
        Me.cmdDisableEvent.Text = "DisableEvent"
        Me.cmdDisableEvent.UseVisualStyleBackColor = False
        '
        'cmdEnableEvent
        '
        Me.cmdEnableEvent.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEnableEvent.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEnableEvent.Enabled = False
        Me.cmdEnableEvent.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEnableEvent.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEnableEvent.Location = New System.Drawing.Point(8, 115)
        Me.cmdEnableEvent.Name = "cmdEnableEvent"
        Me.cmdEnableEvent.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEnableEvent.Size = New System.Drawing.Size(139, 47)
        Me.cmdEnableEvent.TabIndex = 3
        Me.cmdEnableEvent.Text = "EnableEvent"
        Me.cmdEnableEvent.UseVisualStyleBackColor = False
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.BackColor = System.Drawing.SystemColors.Control
        Me.Label4.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label4.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label4.Location = New System.Drawing.Point(160, 192)
        Me.Label4.Name = "Label4"
        Me.Label4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label4.Size = New System.Drawing.Size(74, 14)
        Me.Label4.TabIndex = 9
        Me.Label4.Text = "Digital Input:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.SystemColors.Control
        Me.Label3.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label3.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label3.Location = New System.Drawing.Point(164, 163)
        Me.Label3.Name = "Label3"
        Me.Label3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label3.Size = New System.Drawing.Size(71, 14)
        Me.Label3.TabIndex = 8
        Me.Label3.Text = "INT Missed:"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.SystemColors.Control
        Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label2.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(158, 142)
        Me.Label2.Name = "Label2"
        Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label2.Size = New System.Drawing.Size(76, 14)
        Me.Label2.TabIndex = 7
        Me.Label2.Text = "Event Count:"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'label1
        '
        Me.label1.AutoSize = True
        Me.label1.BackColor = System.Drawing.SystemColors.Control
        Me.label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.label1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.label1.Location = New System.Drawing.Point(170, 121)
        Me.label1.Name = "label1"
        Me.label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.label1.Size = New System.Drawing.Size(63, 14)
        Me.label1.TabIndex = 6
        Me.label1.Text = "INT Count:"
        Me.label1.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblInterruptsMissed
        '
        Me.lblInterruptsMissed.BackColor = System.Drawing.SystemColors.Control
        Me.lblInterruptsMissed.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblInterruptsMissed.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInterruptsMissed.ForeColor = System.Drawing.Color.Blue
        Me.lblInterruptsMissed.Location = New System.Drawing.Point(236, 161)
        Me.lblInterruptsMissed.Name = "lblInterruptsMissed"
        Me.lblInterruptsMissed.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblInterruptsMissed.Size = New System.Drawing.Size(121, 19)
        Me.lblInterruptsMissed.TabIndex = 5
        Me.lblInterruptsMissed.Text = "0"
        '
        'lblDigitalIn
        '
        Me.lblDigitalIn.BackColor = System.Drawing.SystemColors.Control
        Me.lblDigitalIn.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDigitalIn.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDigitalIn.ForeColor = System.Drawing.Color.Blue
        Me.lblDigitalIn.Location = New System.Drawing.Point(236, 189)
        Me.lblDigitalIn.Name = "lblDigitalIn"
        Me.lblDigitalIn.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDigitalIn.Size = New System.Drawing.Size(121, 19)
        Me.lblDigitalIn.TabIndex = 2
        Me.lblDigitalIn.Text = "NA"
        '
        'lblEventCount
        '
        Me.lblEventCount.BackColor = System.Drawing.SystemColors.Control
        Me.lblEventCount.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblEventCount.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEventCount.ForeColor = System.Drawing.Color.Blue
        Me.lblEventCount.Location = New System.Drawing.Point(236, 139)
        Me.lblEventCount.Name = "lblEventCount"
        Me.lblEventCount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblEventCount.Size = New System.Drawing.Size(121, 19)
        Me.lblEventCount.TabIndex = 1
        Me.lblEventCount.Text = "0"
        '
        'lblInterruptCount
        '
        Me.lblInterruptCount.BackColor = System.Drawing.SystemColors.Control
        Me.lblInterruptCount.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblInterruptCount.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInterruptCount.ForeColor = System.Drawing.Color.Blue
        Me.lblInterruptCount.Location = New System.Drawing.Point(236, 117)
        Me.lblInterruptCount.Name = "lblInterruptCount"
        Me.lblInterruptCount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblInterruptCount.Size = New System.Drawing.Size(121, 19)
        Me.lblInterruptCount.TabIndex = 0
        Me.lblInterruptCount.Text = "0"
        '
        'lblInstruction
        '
        Me.lblInstruction.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.lblInstruction.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblInstruction.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInstruction.ForeColor = System.Drawing.Color.Red
        Me.lblInstruction.Location = New System.Drawing.Point(32, 52)
        Me.lblInstruction.Name = "lblInstruction"
        Me.lblInstruction.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblInstruction.Size = New System.Drawing.Size(320, 46)
        Me.lblInstruction.TabIndex = 38
        Me.lblInstruction.Text = "Board must support event handling and interrupt input. For more information, see " & _
            "hardware documentation."
        Me.lblInstruction.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblDemoFunction
        '
        Me.lblDemoFunction.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.lblDemoFunction.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDemoFunction.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDemoFunction.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblDemoFunction.Location = New System.Drawing.Point(13, 12)
        Me.lblDemoFunction.Name = "lblDemoFunction"
        Me.lblDemoFunction.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDemoFunction.Size = New System.Drawing.Size(351, 33)
        Me.lblDemoFunction.TabIndex = 37
        Me.lblDemoFunction.Text = "Demonstration of OnExternalInterrupt Events"
        Me.lblDemoFunction.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'frmEventDisplay
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(377, 225)
        Me.Controls.Add(Me.lblInstruction)
        Me.Controls.Add(Me.lblDemoFunction)
        Me.Controls.Add(Me.cmdDisableEvent)
        Me.Controls.Add(Me.cmdEnableEvent)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.label1)
        Me.Controls.Add(Me.lblInterruptsMissed)
        Me.Controls.Add(Me.lblDigitalIn)
        Me.Controls.Add(Me.lblEventCount)
        Me.Controls.Add(Me.lblInterruptCount)
        Me.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Location = New System.Drawing.Point(4, 23)
        Me.Name = "frmEventDisplay"
        Me.Text = "Universal Library ULEV01"
        Me.ResumeLayout(False)
        Me.PerformLayout()

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
        HandleError = MccDaq.ErrorHandling.DontStop
        ULStat = MccDaq.MccService.ErrHandling(ReportError, HandleError)
        If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then
            Stop
        End If

    End Sub

#End Region

End Class