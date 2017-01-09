'==============================================================================


' File:                         ULEV02.VB

' Library Call Demonstrated:    Mccdaq.MccBoard.EnableEvent with event types:
'                                           - MccDaq.EventType.OnScanError
'                                           - MccDaq.EventType.OnDataAvailable
'                                           - MccDaq.EventType.OnEndOfAiScan
'                               Mccdaq.MccBoard.DisableEvent()

' Demonstration:                Scans a single channel and displays the latest
'                               sample acquired every EventSize or more samples.
'                               Also updates the latest sample upon scan completion
'                               or end. Fatal errors such as Overrun errors, cause
'                               the scan to be aborted.

' Purpose:                      Shows how to enable and respond to events.

' Other Library Calls:          MccDaq.MccService.ErrHandling()
'                               Mccdaq.MccBoard.AInScan()
'
' Special Requirements:         Board 0 must support event handling and have
'                               paced analog inputs.
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

    Const NumPoints As Short = 10000    ' number of data points to collect
    Const SampleRate As Short = 1000    ' rate at which to sample each channel

    ' Data collection options
    Const Options As MccDaq.ScanOptions = _
        MccDaq.ScanOptions.Background Or MccDaq.ScanOptions.ConvertData

    Private Range As MccDaq.Range         ' gain for the channel sampled.
    Private NumAIChans, HighChan As Integer
    Private Resolution As Integer
    Private NumEvents As Integer

    Private Channel As Integer        ' the channel to be sampled.
    Private ptrMyCallback, ptrOnErrorCallback As MccDaq.EventCallback

    Private userData As UserData
    Private ptrUserData As IntPtr

    Private Rate As Integer              ' sample rate for acquiring data.
    Private MemHandle As IntPtr          ' defines a variable to contain the handle to the data

    Private Sub frmEventDisplay_Load(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles MyBase.Load

        Dim EventType As Integer
        Dim LowChan, ChannelType As Integer
        Dim TrigType As MccDaq.TriggerType

        InitUL()

        'determine the number of analog channels and their capabilities
        ChannelType = ANALOGINPUT
        NumAIChans = FindAnalogChansOfType(DaqBoard, _
            ChannelType, Resolution, Range, LowChan, TrigType)

        EventType = DATAEVENT Or ENDEVENT Or ERREVENT
        NumEvents = FindEventsOfType(DaqBoard, EventType)

        If (NumAIChans = 0) Then
            lblInstruction.Text = "Board " & DaqBoard.BoardNum.ToString() & _
            " does not have analog input channels."
            cmdStart.Enabled = False
            cmdStop.Enabled = False
            cmdDisableEvent.Enabled = False
            cmdEnableEvent.Enabled = False
        ElseIf (NumEvents <> EventType) Then
            Me.lblInstruction.Text = "Board " & _
                DaqBoard.BoardNum.ToString() & _
                " doesn't support the specified event types."
        Else
            lblInstruction.Text = "Board " & DaqBoard.BoardNum.ToString() & _
                    "  - Demonstrating event callback functions."
            frmEventDisplay = Me
            ptrMyCallback = New MccDaq.EventCallback(AddressOf MyCallback)
            ptrOnErrorCallback = New MccDaq.EventCallback(AddressOf OnErrorCallback)

            If Resolution > 16 Then
                MemHandle = MccDaq.MccService.WinBufAlloc32Ex(NumPoints)  ' set aside memory to hold High resolution data
            Else
                MemHandle = MccDaq.MccService.WinBufAllocEx(NumPoints)    ' set aside memory to hold data
            End If
            If MemHandle = 0 Then Stop

            Rate = SampleRate

        End If

    End Sub

    Public Sub OnEvent(ByVal bd As Short, ByVal EventType As MccDaq.EventType, ByVal SampleCount As Long)

        ' This gets called by MyCallback in mycallback.vb for each MccDaq.EventType.OnDataAvailable and
        ' MccDaq.EventType.OnEndOfAiScan events. For these event types, the EventData supplied curresponds
        ' to the number of samples collected since the start of MccDaq.MccBoard.AInScan.

        Dim ULStat As MccDaq.ErrorInfo
        Dim SampleIndex As Long
        Dim Data(1) As Short
        Dim Data32(1) As Integer
        Dim Value As Single
        Dim HighResValue As Double

        ' Get the latest sample from the buffer and convert to volts
        SampleIndex = SampleCount - 1
        SampleIndex = SampleIndex Mod NumPoints

        lblSampleCount.Text = (SampleCount).ToString()

        If Resolution > 16 Then
            ULStat = MccDaq.MccService.WinBufToArray32(MemHandle, Data32, SampleIndex, 1)
            ULStat = DaqBoard.ToEngUnits32(Range, Data32(0), HighResValue)
            lblLatestSample.Text = HighResValue.ToString("#0.00000") + "V"
        Else
            ULStat = MccDaq.MccService.WinBufToArray(MemHandle, Data, SampleIndex, 1)
            ULStat = DaqBoard.ToEngUnits(Range, Data(0), Value)
            lblLatestSample.Text = Value.ToString("#0.0000") + "V"
        End If


        If (MccDaq.EventType.OnEndOfAiScan = EventType) Then
            ' Give the library a chance to clean up
            ULStat = DaqBoard.StopBackground(MccDaq.FunctionType.AiFunction)

            If (chkAutoRestart.CheckState = CheckState.Checked) Then
                ' Start a new scan
                Rate = SampleRate
                ULStat = DaqBoard.AInScan(Channel, Channel, _
                NumPoints, Rate, Range, MemHandle, Options)
            Else
                ' Reset the status display
                lblStatus.Text = "IDLE"
                cmdStart.Enabled = True
            End If
        End If

    End Sub

    Public Sub OnScanError(ByVal bd As Short, ByVal EventType As Integer, ByVal ErrorNo As Long)

        ' A scan error occurred; so, abort and reset the controls.

        Dim ULStat As MccDaq.ErrorInfo

        ' We don't need to update the display here since that will happen during the 
        ' MccDaq.EventType.OnEndOfAiScan and/or MccDaq.EventType.OnDataAvailable events 
        ' to follow this event -- yes, this event is handled before any others and this 
        ' event should be accompanied by a MccDaq.EventType.OnEndOfAiScan
        ULStat = DaqBoard.StopBackground(MccDaq.FunctionType.AiFunction)

        ' Reset the AutoRestart such that the MccDaq.EventType.OnEndOfAiScan event does
        ' not automatically start a new scan
        chkAutoRestart.CheckState = System.Windows.Forms.CheckState.Unchecked

    End Sub

    Private Sub cmdEnableEvent_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdEnableEvent.Click

        Dim ULStat As MccDaq.ErrorInfo
        Dim EventSize As Integer '  Minimum number of samples to collect
        '                           between MccDaq.EventType.OnDataAvailable events.

        Dim EventType As MccDaq.EventType ' Type of event to enable
        Dim ValidEntry As Boolean

        ' Enable and connect one or more event types to a single user callback
        ' function using MccDaq.MccBoard.EnableEvent().
        '
        ' If we want to attach a single callback function to more than one event
        ' type, we can do it in a single call to MccDaq.MccBoard.EnableEvent, or we can do this in
        ' separate calls for each event type. The main disadvantage of doing this in a
        ' single call is that if the call generates an error, we will not know which
        ' event type caused the error. In addition, the same error condition could
        ' generate multiple error messages.
        '
        ' Parameters:
        '   EventType   :the condition that will cause an event to fire
        '   EventSize   :only used for MccDaq.EventType.OnDataAvailable to determine how
        '                many samples to collect before firing an event
        '   AddressOf MyCallback  :the address of the user function or event handler
        '                          to call when above event type occurs.
        '                          Note that we can't provide the address of OnEvent directly
        '                          since Microsoft's calling convention for callback functions
        '                          requires that such functions be defined in a standard module
        '                          for Visual Basic. 'MyCallback' will forward the call to OnEvent.
        '   frmEventDisplay        :to make sure that this form handles the event that it set,
        '                          we supply a reference to it by name and dereference
        '                          it in the event handler. Note that the UserData type
        '                          in the event handler must match.

        userData.ThisObj = frmEventDisplay

        ptrUserData = Marshal.AllocCoTaskMem(Marshal.SizeOf(userData))
        Marshal.StructureToPtr(userData, ptrUserData, False)

        EventType = MccDaq.EventType.OnEndOfAiScan
        ULStat = DaqBoard.EnableEvent(EventType, EventSize, ptrMyCallback, ptrUserData)
        If Not ULStat.Value = MccDaq.ErrorInfo.ErrorCode.NoErrors Then Stop

        ValidEntry = Integer.TryParse(txtEventSize.Text, EventSize)
        If ValidEntry Then
            EventType = MccDaq.EventType.OnDataAvailable
            ULStat = DaqBoard.EnableEvent(EventType, EventSize, ptrMyCallback, ptrUserData)
            If Not ULStat.Value = MccDaq.ErrorInfo.ErrorCode.NoErrors Then Stop
        Else
            ULStat = DaqBoard.DisableEvent(MccDaq.EventType.OnDataAvailable)
            If Not ULStat.Value = MccDaq.ErrorInfo.ErrorCode.NoErrors Then Stop
        End If

        ' Since MccDaq.EventType.OnScanError event doesn't use the EventSize, we can set it to anything
        ' we choose without affecting the MccDaq.EventType.OnDataAvailable setting.
        EventType = MccDaq.EventType.OnScanError
        EventSize = 0
        ULStat = DaqBoard.EnableEvent(EventType, EventSize, ptrOnErrorCallback, ptrUserData)
        If Not ULStat.Value = MccDaq.ErrorInfo.ErrorCode.NoErrors Then Stop
        cmdEnableEvent.Enabled = False

    End Sub

    Private Sub cmdStart_Click(ByVal eventSender As System.Object, _
    ByVal eventArgs As System.EventArgs) Handles cmdStart.Click

        Dim ULStat As MccDaq.ErrorInfo

        ' Collect the values with MccDaq.MccBoard.AInScan
        ' Parameters:
        '   Channel     :the channel of the scan
        '   NumPoints   :the total number of A/D samples to collect
        '   Rate        :sample rate
        '   Range       :the gain for the board
        '   MemHandle :the handle to the buffer to hold the data
        '   Options     :data collection options
        Rate = SampleRate

        ULStat = DaqBoard.AInScan(Channel, Channel, NumPoints, Rate, Range, MemHandle, Options)
        If (ULStat.Value = MccDaq.ErrorInfo.ErrorCode.NoErrors) Then
            lblStatus.Text = "RUNNING"
            cmdStart.Enabled = False
        Else
            Stop
        End If

    End Sub

    Private Sub cmdDisableEvent_Click(ByVal eventSender As System.Object, _
    ByVal eventArgs As System.EventArgs) Handles cmdDisableEvent.Click

        Dim ULStat As MccDaq.ErrorInfo

        ' Disable and disconnect all event types with MccDaq.MccBoar.DisableEvent()
        '
        ' Since disabling events that were never enabled is harmless,
        ' we can disable all the events at once.
        '
        ' Parameters:
        '   MccDaq.EventType.AllEventTypes  :all event types will be disabled
        ULStat = DaqBoard.DisableEvent(MccDaq.EventType.AllEventTypes)
        cmdEnableEvent.Enabled = True

    End Sub

    Private Sub cmdStop_Click(ByVal eventSender As System.Object, _
    ByVal eventArgs As System.EventArgs) Handles cmdStop.Click

        Dim ULStat As MccDaq.ErrorInfo

        ' make sure we don't restart the scan MccDaq.EventType.OnEndOfAiScan
        chkAutoRestart.CheckState = System.Windows.Forms.CheckState.Unchecked

        ULStat = DaqBoard.StopBackground(MccDaq.FunctionType.AiFunction)
        cmdStart.Enabled = True

        ' Some devices generate an end of scan event after user
        ' explicitly stops background operations, but most do not
        ' When stopped manually, handle post-scan tasks here.
        cmdStart.Enabled = True
        lblStatus.Text = "IDLE"

    End Sub

    Private Sub frmEventDisplay_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing

        Dim ULStat As MccDaq.ErrorInfo

        If Not GeneralError Then
            If (NumAIChans > 0) Then
                ' make sure to shut down
                ULStat = DaqBoard.StopBackground(MccDaq.FunctionType.AiFunction)

                ' disable any active events
                If Me.cmdDisableEvent.Enabled Then _
                    ULStat = DaqBoard.DisableEvent(MccDaq.EventType.AllEventTypes)

                ' and free the data buffer
                If (MemHandle <> 0) Then MccDaq.MccService.WinBufFreeEx(MemHandle)
                MemHandle = 0
            End If
        End If

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
    Public WithEvents txtEventSize As System.Windows.Forms.TextBox
    Public WithEvents cmdStop As System.Windows.Forms.Button
    Public WithEvents chkAutoRestart As System.Windows.Forms.CheckBox
    Public WithEvents cmdStart As System.Windows.Forms.Button
    Public WithEvents cmdDisableEvent As System.Windows.Forms.Button
    Public WithEvents cmdEnableEvent As System.Windows.Forms.Button
    Public WithEvents Label4 As System.Windows.Forms.Label
    Public WithEvents Label3 As System.Windows.Forms.Label
    Public WithEvents Label2 As System.Windows.Forms.Label
    Public WithEvents Label1 As System.Windows.Forms.Label
    Public WithEvents lblLatestSample As System.Windows.Forms.Label
    Public WithEvents lblStatus As System.Windows.Forms.Label
    Public WithEvents lblSampleCount As System.Windows.Forms.Label
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.txtEventSize = New System.Windows.Forms.TextBox
        Me.cmdStop = New System.Windows.Forms.Button
        Me.chkAutoRestart = New System.Windows.Forms.CheckBox
        Me.cmdStart = New System.Windows.Forms.Button
        Me.cmdDisableEvent = New System.Windows.Forms.Button
        Me.cmdEnableEvent = New System.Windows.Forms.Button
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.lblLatestSample = New System.Windows.Forms.Label
        Me.lblStatus = New System.Windows.Forms.Label
        Me.lblSampleCount = New System.Windows.Forms.Label
        Me.lblInstruction = New System.Windows.Forms.Label
        Me.lblDemoFunction = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'txtEventSize
        '
        Me.txtEventSize.AcceptsReturn = True
        Me.txtEventSize.BackColor = System.Drawing.SystemColors.Window
        Me.txtEventSize.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtEventSize.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEventSize.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtEventSize.Location = New System.Drawing.Point(222, 116)
        Me.txtEventSize.MaxLength = 0
        Me.txtEventSize.Name = "txtEventSize"
        Me.txtEventSize.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtEventSize.Size = New System.Drawing.Size(141, 20)
        Me.txtEventSize.TabIndex = 12
        Me.txtEventSize.Text = "100"
        '
        'cmdStop
        '
        Me.cmdStop.BackColor = System.Drawing.SystemColors.Control
        Me.cmdStop.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdStop.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdStop.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdStop.Location = New System.Drawing.Point(10, 226)
        Me.cmdStop.Name = "cmdStop"
        Me.cmdStop.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdStop.Size = New System.Drawing.Size(115, 33)
        Me.cmdStop.TabIndex = 7
        Me.cmdStop.Text = "Stop"
        Me.cmdStop.UseVisualStyleBackColor = False
        '
        'chkAutoRestart
        '
        Me.chkAutoRestart.BackColor = System.Drawing.SystemColors.Control
        Me.chkAutoRestart.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkAutoRestart.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkAutoRestart.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkAutoRestart.Location = New System.Drawing.Point(192, 222)
        Me.chkAutoRestart.Name = "chkAutoRestart"
        Me.chkAutoRestart.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkAutoRestart.Size = New System.Drawing.Size(95, 21)
        Me.chkAutoRestart.TabIndex = 3
        Me.chkAutoRestart.Text = "Auto Restart"
        Me.chkAutoRestart.UseVisualStyleBackColor = False
        '
        'cmdStart
        '
        Me.cmdStart.BackColor = System.Drawing.SystemColors.Control
        Me.cmdStart.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdStart.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdStart.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdStart.Location = New System.Drawing.Point(10, 192)
        Me.cmdStart.Name = "cmdStart"
        Me.cmdStart.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdStart.Size = New System.Drawing.Size(115, 33)
        Me.cmdStart.TabIndex = 2
        Me.cmdStart.Text = "Start"
        Me.cmdStart.UseVisualStyleBackColor = False
        '
        'cmdDisableEvent
        '
        Me.cmdDisableEvent.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDisableEvent.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDisableEvent.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDisableEvent.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDisableEvent.Location = New System.Drawing.Point(10, 150)
        Me.cmdDisableEvent.Name = "cmdDisableEvent"
        Me.cmdDisableEvent.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDisableEvent.Size = New System.Drawing.Size(115, 33)
        Me.cmdDisableEvent.TabIndex = 1
        Me.cmdDisableEvent.Text = "DisableEvent"
        Me.cmdDisableEvent.UseVisualStyleBackColor = False
        '
        'cmdEnableEvent
        '
        Me.cmdEnableEvent.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEnableEvent.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEnableEvent.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEnableEvent.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEnableEvent.Location = New System.Drawing.Point(10, 116)
        Me.cmdEnableEvent.Name = "cmdEnableEvent"
        Me.cmdEnableEvent.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEnableEvent.Size = New System.Drawing.Size(115, 33)
        Me.cmdEnableEvent.TabIndex = 0
        Me.cmdEnableEvent.Text = "EnableEvent"
        Me.cmdEnableEvent.UseVisualStyleBackColor = False
        '
        'Label4
        '
        Me.Label4.BackColor = System.Drawing.SystemColors.Control
        Me.Label4.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label4.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label4.Location = New System.Drawing.Point(130, 118)
        Me.Label4.Name = "Label4"
        Me.Label4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label4.Size = New System.Drawing.Size(89, 21)
        Me.Label4.TabIndex = 11
        Me.Label4.Text = "Event Size:"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.SystemColors.Control
        Me.Label3.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label3.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label3.Location = New System.Drawing.Point(130, 186)
        Me.Label3.Name = "Label3"
        Me.Label3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label3.Size = New System.Drawing.Size(89, 21)
        Me.Label3.TabIndex = 10
        Me.Label3.Text = "Latest Sample:"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.SystemColors.Control
        Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label2.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(130, 164)
        Me.Label2.Name = "Label2"
        Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label2.Size = New System.Drawing.Size(89, 21)
        Me.Label2.TabIndex = 9
        Me.Label2.Text = "Total Count:"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(130, 141)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(89, 21)
        Me.Label1.TabIndex = 8
        Me.Label1.Text = "Status:"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblLatestSample
        '
        Me.lblLatestSample.BackColor = System.Drawing.SystemColors.Control
        Me.lblLatestSample.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblLatestSample.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLatestSample.ForeColor = System.Drawing.Color.Blue
        Me.lblLatestSample.Location = New System.Drawing.Point(222, 186)
        Me.lblLatestSample.Name = "lblLatestSample"
        Me.lblLatestSample.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblLatestSample.Size = New System.Drawing.Size(141, 21)
        Me.lblLatestSample.TabIndex = 6
        Me.lblLatestSample.Text = "NA"
        '
        'lblStatus
        '
        Me.lblStatus.BackColor = System.Drawing.SystemColors.Control
        Me.lblStatus.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblStatus.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStatus.ForeColor = System.Drawing.Color.Blue
        Me.lblStatus.Location = New System.Drawing.Point(222, 141)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblStatus.Size = New System.Drawing.Size(141, 21)
        Me.lblStatus.TabIndex = 5
        Me.lblStatus.Text = "IDLE"
        '
        'lblSampleCount
        '
        Me.lblSampleCount.BackColor = System.Drawing.SystemColors.Control
        Me.lblSampleCount.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSampleCount.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSampleCount.ForeColor = System.Drawing.Color.Blue
        Me.lblSampleCount.Location = New System.Drawing.Point(222, 164)
        Me.lblSampleCount.Name = "lblSampleCount"
        Me.lblSampleCount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSampleCount.Size = New System.Drawing.Size(141, 21)
        Me.lblSampleCount.TabIndex = 4
        Me.lblSampleCount.Text = "0"
        '
        'lblInstruction
        '
        Me.lblInstruction.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.lblInstruction.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblInstruction.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInstruction.ForeColor = System.Drawing.Color.Red
        Me.lblInstruction.Location = New System.Drawing.Point(31, 49)
        Me.lblInstruction.Name = "lblInstruction"
        Me.lblInstruction.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblInstruction.Size = New System.Drawing.Size(320, 46)
        Me.lblInstruction.TabIndex = 36
        Me.lblInstruction.Text = "Board 0 must support event handling and paced analog input. For more information," & _
            " see hardware documentation."
        Me.lblInstruction.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblDemoFunction
        '
        Me.lblDemoFunction.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.lblDemoFunction.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDemoFunction.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDemoFunction.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblDemoFunction.Location = New System.Drawing.Point(12, 9)
        Me.lblDemoFunction.Name = "lblDemoFunction"
        Me.lblDemoFunction.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDemoFunction.Size = New System.Drawing.Size(351, 33)
        Me.lblDemoFunction.TabIndex = 35
        Me.lblDemoFunction.Text = "Demonstration of OnDataAvailable and OnEndOfAiScan Events"
        Me.lblDemoFunction.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'frmEventDisplay
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(380, 278)
        Me.Controls.Add(Me.lblInstruction)
        Me.Controls.Add(Me.lblDemoFunction)
        Me.Controls.Add(Me.txtEventSize)
        Me.Controls.Add(Me.cmdStop)
        Me.Controls.Add(Me.chkAutoRestart)
        Me.Controls.Add(Me.cmdStart)
        Me.Controls.Add(Me.cmdDisableEvent)
        Me.Controls.Add(Me.cmdEnableEvent)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.lblLatestSample)
        Me.Controls.Add(Me.lblStatus)
        Me.Controls.Add(Me.lblSampleCount)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Location = New System.Drawing.Point(4, 23)
        Me.Name = "frmEventDisplay"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text = "Universal Library ULEV02"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Public WithEvents lblInstruction As System.Windows.Forms.Label
    Public WithEvents lblDemoFunction As System.Windows.Forms.Label
    Dim WithEvents frmEventDisplay As System.Windows.Forms.Form


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