VERSION 5.00
Begin VB.Form frmEventDisplay 
   Caption         =   "Universal Library ULEV01"
   ClientHeight    =   3345
   ClientLeft      =   60
   ClientTop       =   345
   ClientWidth     =   5655
   LinkTopic       =   "Form1"
   ScaleHeight     =   3345
   ScaleWidth      =   5655
   StartUpPosition =   3  'Windows Default
   Begin VB.CommandButton Command1 
      Caption         =   "Quit"
      Height          =   375
      Left            =   4500
      TabIndex        =   12
      Top             =   2880
      Width           =   915
   End
   Begin VB.CommandButton cmdDisableEvent 
      Caption         =   "cbDisableEvent"
      Height          =   465
      Left            =   300
      TabIndex        =   4
      Top             =   2100
      Width           =   1485
   End
   Begin VB.CommandButton cmdEnableEvent 
      Caption         =   "cbEnableEvent"
      Height          =   465
      Left            =   300
      TabIndex        =   3
      Top             =   1410
      Width           =   1485
   End
   Begin VB.Label lblDemoFunction 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      Caption         =   "Demonstration of cbEnableEvent()"
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
      Height          =   255
      Left            =   360
      TabIndex        =   11
      Top             =   120
      Width           =   4875
   End
   Begin VB.Label lblInstruct 
      Alignment       =   2  'Center
      Caption         =   "Supply an interrupt signal to the interrupt input."
      ForeColor       =   &H00FF0000&
      Height          =   675
      Left            =   600
      TabIndex        =   10
      Top             =   540
      Width           =   4395
   End
   Begin VB.Label Label4 
      AutoSize        =   -1  'True
      Caption         =   "Digital Input:"
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
      Left            =   2400
      TabIndex        =   9
      Top             =   2445
      Width           =   1110
   End
   Begin VB.Label Label3 
      Alignment       =   1  'Right Justify
      AutoSize        =   -1  'True
      Caption         =   "INT Missed:"
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
      Left            =   2460
      TabIndex        =   8
      Top             =   2070
      Width           =   1035
   End
   Begin VB.Label Label2 
      Alignment       =   1  'Right Justify
      AutoSize        =   -1  'True
      Caption         =   "Event Count:"
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
      Left            =   2370
      TabIndex        =   7
      Top             =   1695
      Width           =   1125
   End
   Begin VB.Label label1 
      Alignment       =   1  'Right Justify
      AutoSize        =   -1  'True
      Caption         =   "INT Count:"
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
      Left            =   2550
      TabIndex        =   6
      Top             =   1380
      Width           =   945
   End
   Begin VB.Label lblInterruptsMissed 
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
      Height          =   285
      Left            =   3540
      TabIndex        =   5
      Top             =   2040
      Width           =   1815
   End
   Begin VB.Label lblDigitalIn 
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
      Left            =   3540
      TabIndex        =   2
      Top             =   2400
      Width           =   1815
   End
   Begin VB.Label lblEventCount 
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
      Height          =   285
      Left            =   3540
      TabIndex        =   1
      Top             =   1650
      Width           =   1815
   End
   Begin VB.Label lblInterruptCount 
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
      Height          =   285
      Left            =   3540
      TabIndex        =   0
      Top             =   1320
      Width           =   1815
   End
End
Attribute VB_Name = "frmEventDisplay"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
'ULEV01================================================================

' File:                         ULEV01

' Library Call Demonstrated:    cbEnableEvent - ON_EXTERNAL_INTERRUPT
'                               cbDisableEvent()
'
' Purpose:                      Generates an event for each pulse set at a
'                               digital or counter External Interrupt pin,
'                               and reads the digital input at FIRSTPORTA
'                               every UPDATE_SIZE events.
'
' Demonstration:                Shows how to enable and respond to events.

' Other Library Calls:          cbErrHandling()
'                               cbDConfigPort()
'                               cbDIn()
'
' Special Requirements:         Board 0 must have an external interrupt pin
'                               and support the ON_EXTERNAL_INTERRUPT event.
'
'
'==========================================================================
Option Explicit

Const BoardNum As Long = 0          ' Board number
Const Direction& = DIGITALIN ' set direction of PortNum to input
Const UPDATE_SIZE& = 10      ' controls how often to read FIRSTPORTA and update the display

Dim PortNum As Long
Dim EventCount As Long      ' number of events handled since enabling events
Dim UpdateCount As Long
Dim EventsSupported As Boolean
Dim ULStat As Long

Private Sub Form_Load()
  
   Dim ReportError As Long, HandleError As Long
   Dim PortType As Long, ProgAbility As Long
   Dim NumBits As Long, FirstBit As Long
   Dim NumPorts As Long, NumEvents As Long
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
   ULStat& = cbErrHandling(ReportError, HandleError)
   If ULStat& <> 0 Then Stop
   SetDigitalIODefaults ReportError, HandleError
  
   ' configure FIRSTPORTA for digital input
   '  Parameters:
   '    BoardNum     :the number used by CB.CFG to describe this board.
   '    PortNum      :the input port
   '    Direction    :sets the port for input
  
   'determine if digital port exists, its capabilities, etc
   PortType = PORTIN
   NumPorts = FindPortsOfType(BoardNum, PortType, _
      ProgAbility, PortNum, NumBits, FirstBit)
   EventMask = INTEVENT
   NumEvents = FindEventsOfType(BoardNum, EventMask)
   
   If NumPorts = 0 Then
       lblInstruct.Caption = "Board " & Format(BoardNum, "0") & _
         " has no compatible digital ports."
       lblInstruct.ForeColor = &HFF&
       cmdDisableEvent.Enabled = False
       cmdEnableEvent.Enabled = False
   ElseIf (NumEvents <> EventMask) Then
       lblInstruct.Caption = "Board " & Format(BoardNum, "0") & _
         " is not compatible with the specified event types."
       lblInstruct.ForeColor = &HFF&
       cmdDisableEvent.Enabled = False
       cmdEnableEvent.Enabled = False
   Else
       ' if programmable, set direction of port to input
       ' configure the first port for digital input
       '  Parameters:
       '    PortNum        :the input port
       '    Direction      :sets the port for input or output
   
       If ProgAbility = DigitalIO.PROGPORT Then
           ULStat = cbDConfigPort(BoardNum, PortNum, Direction)
           If Not (ULStat = 0) Then Stop
       End If
       lblInstruct.Caption = "The digital input on board " & _
       Format(BoardNum, "0") & " will be read every " & _
       Format(UPDATE_SIZE, "0") & " interrupts received."
   End If

End Sub

Private Sub cmdEnableEvent_Click()
  
   Dim ULStat As Integer
   Dim EventSize As Long, EventType As Long
  
   ' Enable and connect one or more event types to a single user callback
   ' function using cbEnableEvent().
   '
   ' Parameters:
   '   BoardNum    :the number used by CB.CFG to describe this board
   '   EventType   :the condition that will cause an event to fire
   '   EventSize   :only used for ON_DATA_AVAILABLE to determine how
   '                many samples to collect before firing an event
   '   AddressOf MyCallback  :the address of the user function or event handler
   '                          to call when above event type occurs
   '   frmEventDisplay        :to make sure that this form handles the event,
   '                          we supply a reference to it by name and dereference
   '                          it in the event handler. Note that the UserData type
   '                          in the event handler must match.
  
   EventType = ON_EXTERNAL_INTERRUPT ' event from external interrupt pin
   EventSize = 0                     ' not used for this event type
   
   ULStat = cbEnableEvent(BoardNum, EventType, EventSize, _
      AddressOf MyCallback, frmEventDisplay)
   
   If ULStat = NOERRORS Then
      ' reset all counts and displays
      EventCount = 0
      UpdateCount = UPDATE_SIZE
      
      lblEventCount.Caption = Str(EventCount)
      lblInterruptCount.Caption = "0"
      lblDigitalIn.Caption = "NA"
      lblInterruptsMissed.Caption = "0"
      EventsSupported = True
   End If
   
End Sub

Public Sub OnEvent(bd As Integer, EventType As Long, InterruptCount As Long)
  
   ' This gets called by MyCallback in mycallback.bas for each ON_EXTERNAL_INTERRUPT
   ' event. For this event type, the EventData supplied curresponds to the number of
   ' interrupts that occurred since the event was last enabled.
  
   Dim ULStat As Integer
   Dim DigitalData As Integer   ' digital input from FIRSTPORTA
   Dim InterruptsMissed As Long ' number of interrupts missed since enabling events.
   
   EventCount = EventCount + 1
   
   ' We only update the display every UPDATE_SIZE events since the work below
   ' is "expensive." The longer we spend in this handler and the more frequent
   ' the interrupts occur, the more likely we'll miss interrupts.
   If (EventCount >= UpdateCount) Then
      UpdateCount = UpdateCount + UPDATE_SIZE
      InterruptsMissed = InterruptCount - EventCount
      
      lblInterruptCount.Caption = Str(InterruptCount)
      lblInterruptsMissed.Caption = Str(InterruptsMissed)
      
      ' read FIRSTPORTA digital input and display
      '
      ' Parameters:
      '   bd           :the number used by CB.CFG to describe this board
      '   PortNum      :the input port
      '   DigitalData  :the value read from the port
      ULStat = cbDIn(bd, PortNum, DigitalData)
      If NOERRORS = ULStat Then lblDigitalIn.Caption = Hex(DigitalData)
      
   End If
   lblEventCount.Caption = Str(EventCount)
     
End Sub

Sub OnScanError(bd As Integer, EventType As Long, InterruptCount As Long)

   ' this is just a placeholder referenced in mycallback.bas

End Sub

Private Sub cmdDisableEvent_Click()
  
   Dim ULStat As Integer
   
   ' Disable and disconnect all event types with cbDisableEvent()
   '
   ' Since disabling events that were never enabled is harmless,
   ' we can disable all the events at once.
   '
   ' Parameters:
   '   BoardNum         :the number used by CB.CFG to describe this board
   '   ALL_EVENT_TYPES  :all event types will be disabled
   
   ULStat = cbDisableEvent(BoardNum, ALL_EVENT_TYPES)
   If ULStat = 0 Then EventsSupported = True
    
End Sub

Private Sub Form_Unload(Cancel As Integer)
   
   'Make sure all events are disabled before exiting
   ' Disable and disconnect all event types with cbDisableEvent()
   '
   ' Since disabling events that were never enabled is harmless,
   ' we can disable all the events at once.
   '
   ' Parameters:
   '   BoardNum         :the number used by CB.CFG to describe this board
   '   ALL_EVENT_TYPES  :all event types will be disabled
     
   If EventsSupported Then _
      ULStat = cbDisableEvent(BoardNum, ALL_EVENT_TYPES)

End Sub

Private Sub Command1_Click()

   End
   
End Sub


