VERSION 5.00
Begin VB.Form frmEventDisplay 
   Caption         =   "Universal Library ULEV03"
   ClientHeight    =   5295
   ClientLeft      =   60
   ClientTop       =   345
   ClientWidth     =   6510
   LinkTopic       =   "Form1"
   ScaleHeight     =   5295
   ScaleWidth      =   6510
   StartUpPosition =   3  'Windows Default
   Begin VB.CheckBox chkAutoRestart 
      Caption         =   "Auto Restart"
      Height          =   315
      Left            =   330
      TabIndex        =   4
      Top             =   4620
      Width           =   1425
   End
   Begin VB.CommandButton cmdStop 
      Caption         =   "Stop"
      Height          =   495
      Left            =   120
      TabIndex        =   3
      Top             =   3180
      Width           =   1725
   End
   Begin VB.CommandButton cmdStart 
      Caption         =   "Start"
      Enabled         =   0   'False
      Height          =   495
      Left            =   120
      TabIndex        =   2
      Top             =   2670
      Width           =   1725
   End
   Begin VB.CommandButton cmdDisableEvent 
      Caption         =   "cbDisableEvent"
      Enabled         =   0   'False
      Height          =   495
      Left            =   120
      TabIndex        =   1
      Top             =   2160
      Width           =   1725
   End
   Begin VB.CommandButton cmdEnableEvent 
      Caption         =   "cbEnableEvent"
      Enabled         =   0   'False
      Height          =   495
      Left            =   120
      TabIndex        =   0
      Top             =   1650
      Width           =   1725
   End
   Begin VB.Label lblDemoFunction 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      Caption         =   "Demonstration of cbEnableEvent() using OnPretrigger, OnScanError, and OnEndOfAIScan event types"
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
      TabIndex        =   50
      Top             =   120
      Width           =   5955
   End
   Begin VB.Label lblInstruct 
      Alignment       =   2  'Center
      Caption         =   "Device must support analog input pretrigger scanning with events."
      ForeColor       =   &H00FF0000&
      Height          =   675
      Left            =   360
      TabIndex        =   49
      Top             =   780
      Width           =   5715
   End
   Begin VB.Label Label2 
      Alignment       =   1  'Right Justify
      AutoSize        =   -1  'True
      Caption         =   "PreCount"
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
      Left            =   30
      TabIndex        =   48
      Top             =   4200
      Width           =   795
   End
   Begin VB.Label Label1 
      Alignment       =   1  'Right Justify
      AutoSize        =   -1  'True
      Caption         =   "Satus:"
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
      Left            =   240
      TabIndex        =   47
      Top             =   3795
      Width           =   555
   End
   Begin VB.Label lblPreCount 
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
      Height          =   285
      Left            =   900
      TabIndex        =   46
      Top             =   4140
      Width           =   1155
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
      Height          =   285
      Left            =   900
      TabIndex        =   45
      Top             =   3750
      Width           =   1155
   End
   Begin VB.Label lblPosttriggerData 
      Height          =   255
      Index           =   9
      Left            =   5370
      TabIndex        =   44
      Top             =   4785
      Width           =   945
   End
   Begin VB.Label lblPosttriggerData 
      Height          =   255
      Index           =   8
      Left            =   5370
      TabIndex        =   43
      Top             =   4440
      Width           =   945
   End
   Begin VB.Label lblPosttriggerData 
      Height          =   255
      Index           =   7
      Left            =   5370
      TabIndex        =   42
      Top             =   4080
      Width           =   945
   End
   Begin VB.Label lblPosttriggerData 
      Height          =   255
      Index           =   6
      Left            =   5370
      TabIndex        =   41
      Top             =   3735
      Width           =   945
   End
   Begin VB.Label lblPosttriggerData 
      Height          =   255
      Index           =   5
      Left            =   5370
      TabIndex        =   40
      Top             =   3390
      Width           =   945
   End
   Begin VB.Label lblPosttriggerData 
      Height          =   255
      Index           =   4
      Left            =   5370
      TabIndex        =   39
      Top             =   3045
      Width           =   945
   End
   Begin VB.Label lblPosttriggerData 
      Height          =   255
      Index           =   3
      Left            =   5370
      TabIndex        =   38
      Top             =   2700
      Width           =   945
   End
   Begin VB.Label lblPosttriggerData 
      Height          =   255
      Index           =   2
      Left            =   5370
      TabIndex        =   37
      Top             =   2340
      Width           =   945
   End
   Begin VB.Label lblPosttriggerData 
      Height          =   255
      Index           =   1
      Left            =   5370
      TabIndex        =   36
      Top             =   1995
      Width           =   945
   End
   Begin VB.Label lblPosttriggerData 
      Height          =   255
      Index           =   0
      Left            =   5370
      TabIndex        =   35
      Top             =   1650
      Width           =   945
   End
   Begin VB.Label lblPretriggerData 
      Height          =   285
      Index           =   9
      Left            =   3120
      TabIndex        =   34
      Top             =   4755
      Width           =   915
   End
   Begin VB.Label lblPretriggerData 
      Height          =   285
      Index           =   8
      Left            =   3120
      TabIndex        =   33
      Top             =   4410
      Width           =   915
   End
   Begin VB.Label lblPretriggerData 
      Height          =   285
      Index           =   7
      Left            =   3120
      TabIndex        =   32
      Top             =   4050
      Width           =   915
   End
   Begin VB.Label lblPretriggerData 
      Height          =   285
      Index           =   6
      Left            =   3120
      TabIndex        =   31
      Top             =   3705
      Width           =   915
   End
   Begin VB.Label lblPretriggerData 
      Height          =   285
      Index           =   5
      Left            =   3120
      TabIndex        =   30
      Top             =   3360
      Width           =   915
   End
   Begin VB.Label lblPretriggerData 
      Height          =   285
      Index           =   4
      Left            =   3120
      TabIndex        =   29
      Top             =   3015
      Width           =   915
   End
   Begin VB.Label lblPretriggerData 
      Height          =   285
      Index           =   3
      Left            =   3120
      TabIndex        =   28
      Top             =   2670
      Width           =   915
   End
   Begin VB.Label lblPretriggerData 
      Height          =   285
      Index           =   2
      Left            =   3120
      TabIndex        =   27
      Top             =   2325
      Width           =   915
   End
   Begin VB.Label lblPretriggerData 
      Height          =   285
      Index           =   1
      Left            =   3120
      TabIndex        =   26
      Top             =   1980
      Width           =   915
   End
   Begin VB.Label lblPretriggerData 
      Height          =   285
      Index           =   0
      Left            =   3120
      TabIndex        =   25
      Top             =   1635
      Width           =   915
   End
   Begin VB.Label lbl 
      Caption         =   "Trigger +9"
      Height          =   255
      Index           =   19
      Left            =   4410
      TabIndex        =   24
      Top             =   4785
      Width           =   915
   End
   Begin VB.Label lbl 
      Caption         =   "Trigger +8"
      Height          =   255
      Index           =   18
      Left            =   4410
      TabIndex        =   23
      Top             =   4440
      Width           =   915
   End
   Begin VB.Label lbl 
      Caption         =   "Trigger +7"
      Height          =   255
      Index           =   17
      Left            =   4410
      TabIndex        =   22
      Top             =   4080
      Width           =   915
   End
   Begin VB.Label lbl 
      Caption         =   "Trigger +6"
      Height          =   255
      Index           =   16
      Left            =   4410
      TabIndex        =   21
      Top             =   3735
      Width           =   915
   End
   Begin VB.Label lbl 
      Caption         =   "Trigger +5"
      Height          =   255
      Index           =   15
      Left            =   4410
      TabIndex        =   20
      Top             =   3390
      Width           =   915
   End
   Begin VB.Label lbl 
      Caption         =   "Trigger +4"
      Height          =   255
      Index           =   14
      Left            =   4410
      TabIndex        =   19
      Top             =   3045
      Width           =   915
   End
   Begin VB.Label lbl 
      Caption         =   "Trigger +3"
      Height          =   255
      Index           =   13
      Left            =   4410
      TabIndex        =   18
      Top             =   2700
      Width           =   915
   End
   Begin VB.Label lbl 
      Caption         =   "Trigger +2"
      Height          =   255
      Index           =   12
      Left            =   4410
      TabIndex        =   17
      Top             =   2340
      Width           =   915
   End
   Begin VB.Label lbl 
      Caption         =   "Trigger +1"
      Height          =   255
      Index           =   11
      Left            =   4410
      TabIndex        =   16
      Top             =   1995
      Width           =   915
   End
   Begin VB.Label lbl 
      Caption         =   "Trigger +0"
      Height          =   255
      Index           =   10
      Left            =   4410
      TabIndex        =   15
      Top             =   1650
      Width           =   915
   End
   Begin VB.Label lbl 
      Caption         =   "Trigger -1"
      Height          =   255
      Index           =   9
      Left            =   2130
      TabIndex        =   14
      Top             =   4770
      Width           =   915
   End
   Begin VB.Label lbl 
      Caption         =   "Trigger -2"
      Height          =   255
      Index           =   8
      Left            =   2130
      TabIndex        =   13
      Top             =   4425
      Width           =   915
   End
   Begin VB.Label lbl 
      Caption         =   "Trigger -3"
      Height          =   255
      Index           =   7
      Left            =   2130
      TabIndex        =   12
      Top             =   4065
      Width           =   915
   End
   Begin VB.Label lbl 
      Caption         =   "Trigger -4"
      Height          =   255
      Index           =   6
      Left            =   2130
      TabIndex        =   11
      Top             =   3720
      Width           =   915
   End
   Begin VB.Label lbl 
      Caption         =   "Trigger -5"
      Height          =   255
      Index           =   5
      Left            =   2130
      TabIndex        =   10
      Top             =   3375
      Width           =   915
   End
   Begin VB.Label lbl 
      Caption         =   "Trigger -6"
      Height          =   255
      Index           =   4
      Left            =   2130
      TabIndex        =   9
      Top             =   3030
      Width           =   915
   End
   Begin VB.Label lbl 
      Caption         =   "Trigger -7"
      Height          =   255
      Index           =   3
      Left            =   2130
      TabIndex        =   8
      Top             =   2685
      Width           =   915
   End
   Begin VB.Label lbl 
      Caption         =   "Trigger -8"
      Height          =   255
      Index           =   2
      Left            =   2130
      TabIndex        =   7
      Top             =   2340
      Width           =   915
   End
   Begin VB.Label lbl 
      Caption         =   "Trigger -9"
      Height          =   255
      Index           =   1
      Left            =   2130
      TabIndex        =   6
      Top             =   1995
      Width           =   915
   End
   Begin VB.Label lbl 
      Caption         =   "Trigger -10"
      Height          =   255
      Index           =   0
      Left            =   2130
      TabIndex        =   5
      Top             =   1650
      Width           =   915
   End
End
Attribute VB_Name = "frmEventDisplay"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
'=======================================================================
' File:                         ULEV03

' Library Call Demonstrated:    cbEnableEvent - ON_SCAN_ERROR
'                                             - ON_PRETRIGGER
'                                             - ON_END_OF_AI_SCAN
'                               cbDisableEvent()
'                               cbAPretrig()

' Purpose:                      Scans a single channel with cbAPretrig and sets
'                               digital outputs high upon first trigger event.
'                               Upon scan completion, it displays immediate points
'                               before and after the trigger. Fatal errors such as
'                               OVERRUN errors, cause the scan to be aborted, but TOOFEW
'                               errors are ignored.
'
' Demonstration:                Shows how to enable and respond to events.

' Other Library Calls:          cbErrHandling()
'                               cbDOut()

' Special Requirements:         Board 0 must support event handling, cbAPretrig,
'                               and cbDOut.
'
'==========================================================================
Option Explicit

Const BoardNum = 0                  ' Board number
Const CHANNEL = 0                   ' The channel to be sampled.
Const NumPoints = 5000              ' Number of data points to collect
Const BUFFERSIZE = 5512             ' Buffer needs to be big enough to hold
                                    ' NumPoints plus up to 1 full blocksize
                                    ' of data -- 512 is sufficient
                                    ' for most boards.
Const PRECOUNT = 1000               ' Number of samples to acquire before the trigger
Const Options = BACKGROUND          ' Data collection options

Const SampleRate = 2000             ' rate at which to sample each channel

Dim CBRange As Long, ULStat As Long
Dim NumAIChans As Long, MaxChan As Long
Dim ADResolution As Long
Dim PortNum As Long
   
Dim VarPreCount As Long
Dim TotalCount As Long
Dim MemHandle As Long      ' Defines a variable to contain the handle for
                           ' memory allocated by Windows through cbWinBufAlloc%()

Dim GeneralError As Boolean

Dim Data(BUFFERSIZE) As Integer
Dim ChanTag(BUFFERSIZE) As Integer
Dim ActualPreCount As Long ' Actual number of samples acquired at time of trigger

Private Sub Form_Load()
  
   Dim ReportError As Long, HandleError As Long
   Dim PortType As Long, ProgAbility As Long
   Dim NumBits As Long, FirstBit As Long
   Dim NumPorts As Long, LowChan As Long
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
   GeneralError = AIOGeneralError
   EventMask = ERREVENT Or PRETRIGEVENT Or ENDEVENT
   If Not GeneralError Then _
      NumEvents = FindEventsOfType(BoardNum, EventMask)
   GeneralError = GeneralError Or EventGeneralError
   'determine if digital port exists, its capabilities, etc
   PortType = PORTOUT
   If Not GeneralError Then _
      NumPorts = FindPortsOfType(BoardNum, PortType, _
      ProgAbility, PortNum, NumBits, FirstBit)
   GeneralError = GeneralError Or DIOGeneralError

   If (NumAIChans = 0) Then
      lblInstruct.Caption = "Board " & Format(BoardNum, "0") & _
         " does not have analog input channels."
      lblInstruct.ForeColor = &HFF
   ElseIf (NumEvents <> EventMask) Then
      lblInstruct.Caption = "Board " & Format(BoardNum, "0") & _
        " is not compatible with the specified event types."
      lblInstruct.ForeColor = &HFF
   ElseIf NumPorts = 0 Then
      lblInstruct.Caption = "Board " & Format(BoardNum, "0") & _
        " has no compatible digital ports."
      lblInstruct.ForeColor = &HFF
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
      If ProgAbility = DigitalIO.PROGPORT Then
         ULStat = cbDConfigPort(BoardNum, PortNum, DIGITALOUT)
         If Not (ULStat = 0) Then Stop
      End If
      If (NumAIChans > 8) Then NumAIChans = 8 'limit to 8 for display
      MaxChan = LowChan + NumAIChans - 1
      lblInstruct.Caption = "Board " & Format(BoardNum, "0") & _
         " collecting analog data on one channel using AInScan " & _
         "with Range set to " & GetRangeString(CBRange) & "."
      cmdDisableEvent.Enabled = True
      cmdEnableEvent.Enabled = True
      cmdStart.Enabled = True
   End If

End Sub

Public Sub OnEvent(bd As Integer, EventType As Long, SampleCount As Long)
   ' This gets called by MyCallback in mycallback.bas for each ON_PRETRIGGER and
   ' ON_END_OF_AI_SCAN events. For the ON_PRETRIGGER event, the EventData supplied
   ' corresponds to the number of pretrigger samples available in the buffer. For the
   ' ON_END_OF_AI_SCAN event, the EventData supplied corresponds to the number of samples
   ' aquired since the start of cbAPretrig.
      
   Dim Value As Single
   Dim PreTriggerIndex As Long
   Dim PostTriggerIndex As Long
   Dim Offset As Long

   If (ON_PRETRIGGER = EventType) Then
      ' store actual number of pre-trigger samples collected
      ActualPreCount = SampleCount
      lblPreCount.Caption = Str(SampleCount)
      ' signal external device that trigger has been detected
      ULStat = cbDOut(bd, PortNum, &HFF&)
   ElseIf (ON_END_OF_AI_SCAN = EventType) Then
      ' Give the library a chance to clean up
      ULStat = cbStopBackground(bd, AIFUNCTION)
      lblStatus.Caption = "IDLE"
      
      ' Get the data and align it so that oldest data is first
      ULStat = cbWinBufToArray(MemHandle, Data(0), 0, BUFFERSIZE - 1)
      ULStat = cbAConvertPretrigData(bd, VarPreCount, _
         TotalCount, Data(0), ChanTag(0))
      
      ' Update the Pre- and Post- Trigger data displays
      For Offset = 0 To 9
         ' Determine the data index with respect to the trigger index
         PreTriggerIndex = VarPreCount - 10 + Offset
         PostTriggerIndex = VarPreCount + Offset
         
         ' Avoid indexing invalid pretrigger data
         If (10 - Offset < ActualPreCount) Then
            ULStat = cbToEngUnits(bd, CBRange, Data(PreTriggerIndex), Value)
            lblPretriggerData(Offset).Caption = Format(Value, "#0.0000 V")
         Else ' this index doesn't point to valid data
            lblPretriggerData(Offset).Caption = "NA"
         End If
         
         ULStat = cbToEngUnits(bd, CBRange, Data(PostTriggerIndex), Value)
         lblPosttriggerData(Offset).Caption = Format(Value, "#0.0000 V")
      Next Offset
      If (chkAutoRestart.Value) Then
         ' Start a new scan
         VarPreCount = PRECOUNT
         TotalCount = NumPoints
         ULStat = cbAPretrig(bd, CHANNEL, CHANNEL, _
            VarPreCount, TotalCount, SampleRate, _
            CBRange, MemHandle, Options)
         lblStatus.ForeColor = &HFF0000
         lblStatus.Caption = "RUNNING"
         lblPreCount.Caption = "NA"
      End If
      ' Deassert external device signal
      ULStat = cbDOut(bd, PortNum, 0)
   End If
 
End Sub

Public Sub OnScanError(bd As Integer, EventType As Long, ErrorNo As Long)
   ' A scan error occurred; if fatal(not TOOFEW), abort and reset the controls.
   
   ' We don't need to update the display here since that will happen during
   ' the ON_END_OF_AI_SCAN  event to follow this event -- yes, this event is
   ' handled before any others, and if fatal, this event should be accompanied
   ' by an ON_END_OF_AI_SCAN event.
   
   If (ErrorNo <> TOOFEW) Then
      ULStat = cbStopBackground(bd, AIFUNCTION)
      
      ' Reset the chkAutoRestart such that the ON_END_OF_AI_SCAN event does
      ' not automatically start a new scan
      chkAutoRestart.Value = 0
      lblStatus.ForeColor = &HFF&
      lblStatus.Caption = "FATAL ERROR!"
   Else
      lblStatus.ForeColor = &HFF&
      lblStatus.Caption = "TOOFEW"
   End If

End Sub

Private Sub cmdEnableEvent_Click()
  
   Dim EventType As Long           ' Type of event to enable
   
   ' Install event handlers for event conditions.
   '   If we want to attach a single callback function to more than one event
   '   type, we can do it in a single call to cbEnableEvent, or we can do it in
   '   separate calls for each event type. A disadvantage of doing it in a
   '   single call is that if the call generates an error, we will not know which
   '   event type caused the error. In addition, the same error condition could
   '   generate multiple error messages.
   '
   ' Parameters:
   '    BoardNum                       : The board for which the EventType conditions
   '                                     will generate an event.
   '    EventType = ON_PRETRIGGER+_    : Generate an event upon first trigger during a cbAPretrig scan
   '                ON_END_OF_AI_SCAN  : Generate an event upon scan completion or end
   '
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
   EventType = ON_PRETRIGGER + ON_END_OF_AI_SCAN
   ULStat = cbEnableEvent(BoardNum, EventType, 0, _
      AddressOf MyCallback, frmEventDisplay)
   
   ' Since ON_SCAN_ERROR event doesn't use the EventSize, we can set it to anything
   ' we choose without affecting the ON_DATA_AVAILABLE setting.
   ULStat = cbEnableEvent(BoardNum, ON_SCAN_ERROR, 0, _
      AddressOf OnErrorCallback, frmEventDisplay)
   If (ULStat <> NOERRORS) Then
     cmdEnableEvent.Enabled = True
   End If
  
End Sub

Private Sub cmdStart_Click()
  
   'start the scan
   ActualPreCount = 0
   VarPreCount = PRECOUNT
   TotalCount = NumPoints
   ULStat = cbAPretrig(BoardNum, CHANNEL, CHANNEL, _
      VarPreCount, TotalCount, SampleRate, _
      CBRange, MemHandle, Options)
   If (ULStat = NOERRORS) Then
      lblStatus.ForeColor = &HFF0000
      lblStatus.Caption = "RUNNING"
      lblPreCount.Caption = "NA"
   End If
   
End Sub

Private Sub cmdStop_Click()
  
   ' make sure we don't restart the scan ON_END_OF_AI_SCAN
   chkAutoRestart.Value = 0
   ULStat = cbStopBackground(BoardNum, AIFUNCTION)
   
End Sub

Private Sub cmdDisableEvent_Click()
  
   Dim EventTypes As Long
   
   ' we should stop any active scans before disabling events
   ULStat = cbStopBackground(BoardNum, AIFUNCTION)
   
   ' Disconnect and uninstall event handlers
   '   We can disable all the events at once, and disabling events
   '   that were never enabled is harmless
   '
   ' Parameters:
   '   BoardNum          : board for which scan conditions produce events.
   '   EventTypes        : the event types which are being disabled.
   EventTypes = ALL_EVENT_TYPES
   ULStat = cbDisableEvent(BoardNum, EventTypes)
  
End Sub

Private Sub Form_Unload(Cancel As Integer)
   
   If Not GeneralError Then
      ' make sure to shut down
      ULStat = cbStopBackground(BoardNum, AIFUNCTION)
      ' and diable any active events
      If Me.cmdDisableEvent.Enabled Then _
         ULStat = cbDisableEvent(BoardNum, ALL_EVENT_TYPES)
      If (MemHandle <> 0) Then cbWinBufFree (MemHandle)
      MemHandle = 0
   End If
   
End Sub
