VERSION 5.00
Begin VB.Form frmEventDisplay 
   Caption         =   "Universal Library ULEV02"
   ClientHeight    =   4065
   ClientLeft      =   60
   ClientTop       =   345
   ClientWidth     =   5700
   LinkTopic       =   "Form1"
   ScaleHeight     =   4065
   ScaleWidth      =   5700
   StartUpPosition =   3  'Windows Default
   Begin VB.TextBox txtEventSize 
      Height          =   285
      Left            =   3330
      TabIndex        =   12
      Text            =   "100"
      Top             =   1830
      Width           =   2115
   End
   Begin VB.CommandButton cmdStop 
      Caption         =   "Stop"
      Height          =   375
      Left            =   150
      TabIndex        =   7
      Top             =   3480
      Width           =   1725
   End
   Begin VB.CheckBox chkAutoRestart 
      Caption         =   "Auto Restart"
      Height          =   195
      Left            =   2880
      TabIndex        =   3
      Top             =   3420
      Width           =   1425
   End
   Begin VB.CommandButton cmdStart 
      Caption         =   "Start"
      Height          =   375
      Left            =   150
      TabIndex        =   2
      Top             =   2970
      Width           =   1725
   End
   Begin VB.CommandButton cmdDisableEvent 
      Caption         =   "cbDisableEvent"
      Height          =   375
      Left            =   150
      TabIndex        =   1
      Top             =   2340
      Width           =   1725
   End
   Begin VB.CommandButton cmdEnableEvent 
      Caption         =   "cbEnableEvent"
      Height          =   375
      Left            =   150
      TabIndex        =   0
      Top             =   1830
      Width           =   1725
   End
   Begin VB.Label lblInstruct 
      Alignment       =   2  'Center
      Caption         =   "Device must support analog input scanning with events."
      ForeColor       =   &H00FF0000&
      Height          =   675
      Left            =   360
      TabIndex        =   14
      Top             =   780
      Width           =   4995
   End
   Begin VB.Label lblDemoFunction 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      Caption         =   "Demonstration of cbEnableEvent() using OnDataAvailable and OnEndOfAIScan event types"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H80000008&
      Height          =   495
      Left            =   240
      TabIndex        =   13
      Top             =   120
      Width           =   5235
   End
   Begin VB.Label Label4 
      Alignment       =   1  'Right Justify
      Caption         =   "Event Size:"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   195
      Left            =   1950
      TabIndex        =   11
      Top             =   1860
      Width           =   1335
   End
   Begin VB.Label Label3 
      Alignment       =   1  'Right Justify
      Caption         =   "Latest Sample:"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   195
      Left            =   1950
      TabIndex        =   10
      Top             =   2880
      Width           =   1335
   End
   Begin VB.Label Label2 
      Alignment       =   1  'Right Justify
      Caption         =   "Total Count:"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   195
      Left            =   1950
      TabIndex        =   9
      Top             =   2535
      Width           =   1335
   End
   Begin VB.Label Label1 
      Alignment       =   1  'Right Justify
      Caption         =   "Status:"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   195
      Left            =   1950
      TabIndex        =   8
      Top             =   2205
      Width           =   1335
   End
   Begin VB.Label lblLatestSample 
      Caption         =   "NA"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   9.75
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H00FF0000&
      Height          =   195
      Left            =   3330
      TabIndex        =   6
      Top             =   2880
      Width           =   2115
   End
   Begin VB.Label lblStatus 
      Caption         =   "IDLE"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   9.75
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H00FF0000&
      Height          =   195
      Left            =   3330
      TabIndex        =   5
      Top             =   2205
      Width           =   2115
   End
   Begin VB.Label lblSampleCount 
      Caption         =   "0"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   9.75
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H00FF0000&
      Height          =   195
      Left            =   3330
      TabIndex        =   4
      Top             =   2535
      Width           =   2115
   End
End
Attribute VB_Name = "frmEventDisplay"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
'======================================================================
' File:                         ULEV02

' Library Call Demonstrated:    cbEnableEvent - ON_SCAN_ERROR
'                                             - ON_DATA_AVAILABLE
'                                             - ON_END_OF_AI_SCAN
'                               cbDisableEvent()
'
' Purpose:                      Scans a single channel and displays the latest
'                               sample acquired every EventSize or more samples.
'                               Also updates the latest sample upon scan completion
'                               or end. Fatal errors such as OVERRUN errors, cause
'                               the scan to be aborted.

' Demonstration:                Shows how to enable and respond to events.

' Other Library Calls:          cbErrHandling()
'                               cbAInScan()
'
' Special Requirements:         Board 0 must support event handling and have
'                               paced analog inputs.
'==========================================================================
Option Explicit

Const BoardNum = 0                    ' board number
Const Channel = 0                     ' the channel to be sampled.
Const NumPoints = 5000                ' number of data points to collect
Const SampleRate = 1000               ' rate at which to sample each channel

Dim CBRange As Long, ULStat As Long
Dim Rate As Long                      ' sample rate for acquiring data.
Dim NumAIChans As Long, MaxChan As Long
Dim ADResolution As Long
Dim MemHandle As Long                 ' defines a variable to contain the handle for
                                      ' memory allocated by Windows through cbWinBufAlloc%()

Const Options = BACKGROUND + CONVERTDATA  ' Data collection options

Private Sub Form_Load()
  
   Dim ReportError As Long, HandleError As Long
   Dim LowChan As Long
   Dim NumEvents As Long, DefaultTrig As Long
   Dim EventMask As Long
  
   ' Initiate error handling
   '  activating error handling will trap errors like
   '  bad channel numbers and non-configured conditions.

   '  Parameters:
   '     DONTPRINT   :all warnings and errors encountered will be printed
   '     DONTSTOP    :if an error is encountered, the program will not stop,
   '                  errors must be handled locally
  
   ReportError = DONTPRINT
   HandleError = DONTSTOP
   ULStat = cbErrHandling(ReportError, HandleError)
   If ULStat <> 0 Then Stop
   SetAnalogIODefaults ReportError, HandleError
  
   ' determine the number of analog channels and their capabilities
   Dim ChannelType As Long
   ChannelType = ANALOGINPUT
   NumAIChans = FindAnalogChansOfType(BoardNum, ChannelType, _
      ADResolution, CBRange, LowChan, DefaultTrig)
   EventMask = DATAEVENT Or ENDEVENT
   NumEvents = FindEventsOfType(BoardNum, EventMask)

   If (NumAIChans = 0) Then
      lblInstruct.Caption = "Board " & Format(BoardNum, "0") & _
         " does not have analog input channels."
      cmdStart.Enabled = False
      lblInstruct.ForeColor = &HFF
      txtEventSize.Enabled = False
      cmdDisableEvent.Enabled = False
      cmdEnableEvent.Enabled = False
   ElseIf (NumEvents <> EventMask) Then
      lblInstruct.Caption = "Board " & Format(BoardNum, "0") & _
        " is not compatible with the specified event types."
      lblInstruct.ForeColor = &HFF
      txtEventSize.Enabled = False
      cmdDisableEvent.Enabled = False
      cmdEnableEvent.Enabled = False
   Else
      ' Check the resolution of the A/D data and allocate memory accordingly
      If ADResolution > 16 Then
         ' set aside memory to hold high resolution data
         ReDim ADData32(NumPoints)
         MemHandle = cbWinBufAlloc32(NumPoints)
      Else
         ' set aside memory to hold data
         ReDim ADData(NumPoints)
         MemHandle = cbWinBufAlloc(NumPoints)
      End If
      If MemHandle = 0 Then Stop
      If (NumAIChans > 8) Then NumAIChans = 8 'limit to 8 for display
      MaxChan = LowChan + NumAIChans - 1
      lblInstruct.Caption = "Board " & Format(BoardNum, "0") & _
         " collecting analog data on one channel using AInScan " & _
         "with Range set to " & GetRangeString(CBRange) & "."
   End If

End Sub

Private Sub cmdEnableEvent_Click()
  
   Dim EventSize As Long      ' Minimum number of samples to collect
                              ' between ON_DATA_AVAILABLE events.
   Dim EventType As Long      ' Type of event to enable
  
   ' Enable and connect one or more event types to a single user callback
   ' function using cbEnableEvent().
   '
   ' If we want to attach a single callback function to more than one event
   ' type, we can do it in a single call to cbEnableEvent, or we can do this in
   ' separate calls for each event type. The main disadvantage of doing this in a
   ' single call is that if the call generates an error, we will not know which
   ' event type caused the error. In addition, the same error condition could
   ' generate multiple error messages.
   '
   ' Parameters:
   '   BoardNum    :the number used by CB.CFG to describe this board
   '   EventType   :the condition that will cause an event to fire
   '   EventSize   :only used for ON_DATA_AVAILABLE to determine how
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
   EventType = ON_DATA_AVAILABLE Or ON_END_OF_AI_SCAN
   EventSize = Int(Val(txtEventSize.Text))
   ULStat = cbEnableEvent(BoardNum, EventType, EventSize, _
      AddressOf MyCallback, frmEventDisplay)
   If (ULStat = ALREADYACTIVE) Then Exit Sub
  
   ' Since ON_SCAN_ERROR event doesn't use the EventSize, we can set it to anything
   ' we choose without affecting the ON_DATA_AVAILABLE setting.
   EventType = ON_SCAN_ERROR
   EventSize = 0
   ULStat = cbEnableEvent(BoardNum, EventType, EventSize, _
      AddressOf OnErrorCallback, frmEventDisplay)
  
End Sub

Private Sub cmdStart_Click()
  
   ' Collect the values with cbAInScan%()
   ' Parameters:
   '   BoardNum%   :the number used by CB.CFG to describe this board
   '   Channel     :the channel of the scan
   '   NumPoints   :the total number of A/D samples to collect
   '   Rate        :sample rate
   '   CBRange     :the gain for the board
   '   MemHandle   :the handle to the buffer to hold the data
   '   Options     :data collection options
   
   ULStat = cbAInScan(BoardNum, Channel, Channel, NumPoints, _
      SampleRate, CBRange, MemHandle, Options)
   If (ULStat = NOERRORS) Then
      lblStatus.Caption = "RUNNING"
   Else
      Stop
   End If
  
End Sub

Private Sub cmdStop_Click()
  
  ' make sure we don't restart the scan ON_END_OF_AI_SCAN
  chkAutoRestart.Value = 0
  ULStat = cbStopBackground(BoardNum, AIFUNCTION)
  
End Sub

Public Sub OnEvent(ByVal bd As Integer, ByVal EventType As Long, ByVal SampleCount As Long)
   
   ' This gets called by MyCallback in mycallback.bas for each ON_DATA_AVAILABLE and
   ' ON_END_OF_AI_SCAN events. For these event types, the EventData supplied curresponds
   ' to the number of samples collected since the start of cbAInScan.
   Dim SampleIndex As Long
   Dim Data(1) As Integer
   Dim Data32(1) As Long
   Dim Value As Single
   Dim HigResValue As Double
   
   ' Get the latest sample from the buffer and convert to volts
   SampleIndex = SampleCount - 1
   lblSampleCount.Caption = Str(SampleCount)
   
   If ADResolution > 16 Then
      ULStat = cbWinBufToArray32(MemHandle, Data32(0), SampleIndex, 1)
      ULStat = cbToEngUnits32(bd, CBRange, Data32(0), HigResValue)
      lblLatestSample.Caption = Format(HigResValue, "#0.00000 V")
   Else
      ULStat = cbWinBufToArray(MemHandle, Data(0), SampleIndex, 1)
      ULStat = cbToEngUnits(bd, CBRange, Data(0), Value)
      lblLatestSample.Caption = Format(Value, "#0.0000 V")
   End If
   
   If (ON_END_OF_AI_SCAN = EventType) Then
      ' Give the library a chance to clean up
      ULStat = cbStopBackground(bd, AIFUNCTION)
      
      If (chkAutoRestart.Value) Then
         ' Start a new scan
         ULStat = cbAInScan(bd, Channel, Channel, NumPoints, _
            SampleRate, CBRange, MemHandle, Options)
      Else
         ' Reset the status display
         lblStatus = "IDLE"
      End If
   End If
   
End Sub

Public Sub OnScanError(ByVal bd As Integer, ByVal EventType As Long, ByVal ErrorNo As Long)
   
   ' A scan error occurred; so, abort and reset the controls.
   ' We don't need to update the display here since that will happen during
   ' the ON_END_OF_AI_SCAN and/or ON_DATA_AVAILABLE events to follow this event
   ' -- yes, this event is handled before any others and this event should be
   ' accompanied by a ON_END_OF_AI_SCAN
   ULStat = cbStopBackground(bd, AIFUNCTION)
   
   ' Reset the bAutoRestart such that the ON_END_OF_AI_SCAN event does
   ' not automatically start a new scan
   chkAutoRestart.Value = 0

End Sub

Private Sub cmdDisableEvent_Click()
  
   ' Disable and disconnect all event types with cbDisableEvent()
   '
   ' Since disabling events that were never enabled is harmless,
   ' we can disable all the events at once.
   '
   ' Parameters:
   '   BoardNum         :the number used by CB.CFG to describe this board
   '   ALL_EVENT_TYPES  :all event types will be disabled
   ULStat = cbDisableEvent(BoardNum, ALL_EVENT_TYPES)
  
End Sub

Private Sub Form_Unload(Cancel As Integer)
  
    ' make sure to shut down
   ULStat = cbStopBackground(BoardNum, AIFUNCTION)
    
    ' disable any active events
   ULStat = cbDisableEvent(BoardNum, ALL_EVENT_TYPES)
    
    ' and free the data buffer
   If (MemHandle <> 0) Then cbWinBufFree (MemHandle)
   MemHandle = 0
    
End Sub
