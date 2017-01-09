VERSION 5.00
Begin VB.Form frm9513Int 
   Appearance      =   0  'Flat
   BackColor       =   &H80000005&
   Caption         =   "Universal Library 9513 Counter Demo"
   ClientHeight    =   5010
   ClientLeft      =   105
   ClientTop       =   1545
   ClientWidth     =   5250
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
   LinkMode        =   1  'Source
   LinkTopic       =   "Form1"
   PaletteMode     =   1  'UseZOrder
   ScaleHeight     =   5010
   ScaleWidth      =   5250
   Begin VB.CommandButton cmdStartInt 
      Appearance      =   0  'Flat
      Caption         =   "Start"
      Height          =   375
      Left            =   3180
      TabIndex        =   4
      Top             =   4440
      Width           =   855
   End
   Begin VB.CommandButton cmdStopRead 
      Appearance      =   0  'Flat
      Caption         =   "Quit"
      Default         =   -1  'True
      Height          =   375
      Left            =   4320
      TabIndex        =   3
      Top             =   4440
      Width           =   855
   End
   Begin VB.Timer tmrReadStatus 
      Enabled         =   0   'False
      Interval        =   200
      Left            =   4680
      Top             =   2400
   End
   Begin VB.Label lblInstruction 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "User must generate an interrupt at IRQ INPUT.  IRQ ENABLE must be tied low. "
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H000000FF&
      Height          =   495
      Left            =   360
      TabIndex        =   23
      Top             =   780
      Width           =   4575
      WordWrap        =   -1  'True
   End
   Begin VB.Label lblShowTotal 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Left            =   2520
      TabIndex        =   18
      Top             =   3840
      Width           =   975
   End
   Begin VB.Label lblIntTotal 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Total Interrupts:"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   840
      TabIndex        =   22
      Top             =   3840
      Width           =   1575
   End
   Begin VB.Label lblCount 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   4
      Left            =   3240
      TabIndex        =   17
      Top             =   3360
      Width           =   975
   End
   Begin VB.Label lblIntStatus 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   4
      Left            =   1800
      TabIndex        =   12
      Top             =   3360
      Width           =   1095
   End
   Begin VB.Label lblCounterNum 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "5"
      ForeColor       =   &H00000000&
      Height          =   255
      Index           =   4
      Left            =   1080
      TabIndex        =   8
      Top             =   3360
      Width           =   375
   End
   Begin VB.Label lblCount 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   3
      Left            =   3240
      TabIndex        =   16
      Top             =   3000
      Width           =   975
   End
   Begin VB.Label lblIntStatus 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   3
      Left            =   1800
      TabIndex        =   11
      Top             =   3000
      Width           =   1095
   End
   Begin VB.Label lblCounterNum 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "4"
      ForeColor       =   &H00000000&
      Height          =   255
      Index           =   3
      Left            =   1080
      TabIndex        =   7
      Top             =   3000
      Width           =   375
   End
   Begin VB.Label lblCount 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   2
      Left            =   3240
      TabIndex        =   15
      Top             =   2640
      Width           =   975
   End
   Begin VB.Label lblIntStatus 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   2
      Left            =   1800
      TabIndex        =   10
      Top             =   2640
      Width           =   1095
   End
   Begin VB.Label lblCounterNum 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "3"
      ForeColor       =   &H00000000&
      Height          =   255
      Index           =   2
      Left            =   1080
      TabIndex        =   6
      Top             =   2640
      Width           =   375
   End
   Begin VB.Label lblCount 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   1
      Left            =   3240
      TabIndex        =   14
      Top             =   2280
      Width           =   975
   End
   Begin VB.Label lblIntStatus 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   1
      Left            =   1800
      TabIndex        =   9
      Top             =   2280
      Width           =   1095
   End
   Begin VB.Label lblCounterNum 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "2"
      ForeColor       =   &H00000000&
      Height          =   255
      Index           =   1
      Left            =   1080
      TabIndex        =   5
      Top             =   2280
      Width           =   375
   End
   Begin VB.Label lblCount 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   0
      Left            =   3240
      TabIndex        =   13
      Top             =   1920
      Width           =   975
   End
   Begin VB.Label lblIntStatus 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   0
      Left            =   1800
      TabIndex        =   2
      Top             =   1920
      Width           =   1095
   End
   Begin VB.Label lblCounterNum 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "1"
      ForeColor       =   &H00000000&
      Height          =   255
      Index           =   0
      Left            =   1080
      TabIndex        =   1
      Top             =   1920
      Width           =   375
   End
   Begin VB.Label lblData 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Data Value"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   -1  'True
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   3120
      TabIndex        =   21
      Top             =   1440
      Width           =   1215
   End
   Begin VB.Label lblStatCol 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Status"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   -1  'True
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   1920
      TabIndex        =   20
      Top             =   1440
      Width           =   855
   End
   Begin VB.Label lblCountCol 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Counter"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   -1  'True
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   840
      TabIndex        =   19
      Top             =   1440
      Width           =   855
   End
   Begin VB.Label lblDemoFunction 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Demonstration of 9513 Counter using Interrupts"
      ForeColor       =   &H80000008&
      Height          =   495
      Left            =   180
      TabIndex        =   0
      Top             =   120
      Width           =   4875
   End
End
Attribute VB_Name = "frm9513Int"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
'===========================================================================

' File:                         ULCT03.VBP

' Library Call Demonstrated:    9513 Counter Functions
'                               cbC9513Config&()
'                               cbCStoreOnInt&()

' Purpose:                      Operate the counter

' Demonstration:                Sets up 2 counters to store values in
'                               response to an interrupt
'

' Other Library Calls:          cbC9513Init&()
'                               cbCLoad&()
'                               cbStopBackground&()
'                               cbErrHandling&()

' Special Requirements:         Board 0 must have a 9513 counter.
'                               IRQ ENABLE must be tied low.
'                               User must generate an interrupt at IRQ INPUT.

'==========================================================================
Option Explicit

Const BoardNum As Long = 0                ' Board number
Const CounterType As Integer = CTR9513    ' counter type compatible
                                          ' with this example program

Dim CounterNum As Long                    ' number of counter used
Dim RegName As Long                       ' register name of counter
Const ChipNum& = 1                        ' use chip 1 for CTR05 or for first
                                          ' chip on CTR10 or CTR20

Dim NumCntrs As Long                      ' number of counters on board
Const IntCount& = 100                     ' the windows buffer pointed to by MemHandle will hold
                                          ' enough data for IntCount interrupts

Dim DataBuffer%()                   ' array to hold latest readings from each of the counters
Dim CntrControl%()                  ' array to control whether or not each counter is enabled
Dim MemHandle&                      ' handle to windows data buffer that is large enough to hold
                                    ' IntCount readings from each of the NumCntrs counters
Dim FirstPoint As Long
Dim ULStat As Long

Private Sub Form_Load()

   Dim FOutDivider As Long
   Dim Source As Long, Compare1 As Long
   Dim Compare2 As Long, TimeOfDay As Long
   Dim GateControl As Long, CounterEdge As Long, CountSource As Long
   Dim SpecialGate As Long, Reload As Long, RecycleMode As Long
   Dim BCDMode As Long, CountDirection As Long, OutputControl As Long
   Dim LoadValue As Long
   
   ' declare revision level of Universal Library
   ULStat& = cbDeclareRevision(CURRENTREVNUM)
   
   ' Initiate error handling
   '  activating error handling will trap errors like
   '  bad channel numbers and non-configured conditions.
   '  Parameters:
   '    DONTPRINT   :all warnings and errors encountered will not be printed
   '    DONTSTOP    :if an error is encountered, the program will not stop,
   '                  errors must be handled locally
      
   ULStat& = cbErrHandling(DONTPRINT, DONTSTOP)
   If ULStat& <> 0 Then Stop
    
   ' If cbErrHandling& is set for STOPALL or STOPFATAL during the program
   ' design stage, Visual Basic will be unloaded when an error is encountered.
   ' We suggest trapping errors locally until the program is ready for compiling
   ' to avoid losing unsaved data during program design.  This can be done by
   ' setting cbErrHandling options as above and checking the value of ULStat&
   ' after a call to the library. If it is not equal to 0, an error has occurred.
    
   NumCntrs = FindCountersOfType(BoardNum, CounterType, CounterNum)
   If NumCntrs = 0 Then
      lblInstruction.Caption = "Board " & Format(BoardNum, "0") & _
         " has no 9513 counters."
      cmdStartInt.Enabled = False
   Else
      ReDim DataBuffer%(NumCntrs)
      ReDim CntrControl%(NumCntrs)
      
      RegName = Choose(CounterNum + 1, LOADREG0, LOADREG1, LOADREG2, LOADREG3, LOADREG4, LOADREG5)

      MemHandle& = cbWinBufAlloc(IntCount& * NumCntrs)  ' set aside memory to hold data
      If MemHandle& = 0 Then Stop
      
      ' Initialize the board level features
      '  Parameters:
      '    BoardNum    :the number used by CBCONFIG to describe this board
      '    ChipNum&    :chip to be initialized (1 for CTR5, 1 or 2 for CTR10)
      '    FOutDivider&:the F-Out divider (0-15)
      '    Source&     :the signal source for F-Out
      '    Compare1&   :status of comparator 1
      '    Compare2&   :status of comparator 2
      '    TimeOfDay&  :time of day mode control
       
      FOutDivider& = 10        ' sets up OSC OUT for 10Hz signal which can
      Source& = FREQ5          ' be used as interrupt source for this example
      Compare1& = CBDISABLED
      Compare2& = CBDISABLED
      TimeOfDay& = CBDISABLED
       
      ULStat& = cbC9513Init(BoardNum, ChipNum&, FOutDivider&, _
         Source&, Compare1&, Compare2&, TimeOfDay&)
      If ULStat& <> 0 Then Stop
       
      ' Set the configurable operations of the counter
      '  Parameters:
      '    BoardNum        :the number used by CBCONFIG to describe this board
      '    CounterNum     :the counter to be configured (1 to 5)
      '    GateControl&    :gate control value
      '    CounterEdge&    :which edge to count
      '    CountSource&    :signal source
      '    SpecialGate&    :status of special gate
      '    Reload&         :method of reloading
      '    RecyleMode&     :recyle mode
      '    BCDMode&        :counting mode, Binary or BCD
      '    CountDirection& :direction for the counting operation (COUNTUP or COUNTDOWN)
      '    OutputControl&  :output signal type and level
      
      ' Initialize variables for the first of two counters
       
      GateControl& = NOGATE
      CounterEdge& = POSITIVEEDGE
      CountSource& = FREQ3
      SpecialGate& = CBDISABLED
      Reload& = LOADREG
      RecycleMode& = RECYCLE
      BCDMode& = CBDISABLED
      CountDirection& = COUNTUP
      OutputControl& = ALWAYSLOW
       
      ULStat& = cbC9513Config(BoardNum, CounterNum, GateControl&, _
         CounterEdge&, CountSource&, SpecialGate&, Reload&, _
         RecycleMode&, BCDMode&, CountDirection&, OutputControl&)
      If ULStat& <> 0 Then Stop
       
      ' Initialize variables for the second counter
      
      ULStat& = cbC9513Config(BoardNum, CounterNum + 1, GateControl&, _
         CounterEdge&, CountSource&, SpecialGate&, Reload&, _
         RecycleMode&, BCDMode&, CountDirection&, OutputControl&)
      If ULStat& <> 0 Then Stop
       
      ' Load the 2 counters with starting values of zero with cbCLoad&()
      '  Parameters:
      '    BoardNum    :the number used by CBCONFIG to describe this board
      '    RegName     :the counter to be loaded with the starting value
      '    LoadValue&  :the starting value to place in the counter
         
      LoadValue& = 0
         
      ULStat& = cbCLoad(BoardNum, RegName, LoadValue&)
      If ULStat& <> 0 Then Stop
         
      ULStat& = cbCLoad(BoardNum, RegName + 1, LoadValue&)
      If ULStat& <> 0 Then Stop
      lblDemoFunction.Caption = "Demonstration of 9513 " & _
         "Counter using Interrupts on board " & _
         Format(BoardNum, "0") & "."
   End If
   
End Sub

Private Sub tmrReadStatus_Timer()

   Dim Status As Integer, CurCount As Long
   Dim I As Long, CurIndex As Long
   Dim IntStatus As String, RealCount As Long
   
   ULStat& = cbGetStatus(BoardNum, Status%, CurCount&, CurIndex&, CTRFUNCTION)
   If ULStat& <> 0 Then Stop

   FirstPoint& = 0
   
   ' The calculation below requires that NumCntrs accurately reflects the number
   ' of counters onboard whether or not they are enabled or active.
   If CurIndex& > 0 Then
      FirstPoint& = NumCntrs& * CurIndex&
   End If

   ULStat& = cbWinBufToArray(MemHandle&, DataBuffer%(0), FirstPoint&, NumCntrs&)
   If ULStat& <> 0 Then Stop

   For I& = 0 To 4
      If CntrControl%(I&) = CBDISABLED Then
         IntStatus$ = "DISABLED"
      Else
         IntStatus$ = "ENABLED "
      End If

      ' convert type int to type long

      RealCount& = DataBuffer%(I&)
      If RealCount& < 0 Then RealCount& = RealCount& + 65536

      lblCounterNum(I&).Caption = Format$(I& + 1, "0")
      lblIntStatus(I&).Caption = IntStatus$
      lblCount(I&).Caption = Format$(RealCount&, "0")

   Next I&
 
   lblShowTotal.Caption = Format$(CurCount&, "0")
   If Status = IDLE Then
      tmrReadStatus.Enabled = False
      cmdStartInt.Caption = "Start"
   End If

End Sub

Private Sub cmdStartInt_Click()

   Dim I As Long
   
   If tmrReadStatus.Enabled Then
      tmrReadStatus.Enabled = False
      cmdStartInt.Caption = "Start"
      ' Parameters:
      '   BoardNum   :the number used by CB.CFG to describe this board
      '   FunctionType:counter operation (CTRFUNCTION)
      
      ULStat& = cbStopBackground(BoardNum, CTRFUNCTION)
      If ULStat& <> 0 Then Stop
   Else
      cmdStartInt.Caption = "Stop"
   
      ' set the counters to store their values upon an interrupt
      '  Parameters:
      '    BoardNum       :the number used by CBCONFIG to describe this board
      '    IntCount&      :maximum number of interrupts
      '    CntrControl%() :array which indicates the channels to be read
      '    DataBuffer%()  :array that receives the count values for enabled
      '                    channels each time an interrupt occur
   
      ' set all channels to CBDISABLED  and init DataBuffer
      For I& = 0 To NumCntrs& - 1
         CntrControl%(I&) = CBDISABLED
         DataBuffer%(I&) = 0
      Next I&
   
      ' enable the channels to be monitored
      CntrControl%(0) = CBENABLED
      CntrControl%(1) = CBENABLED
   
      ULStat& = cbCStoreOnInt(BoardNum, IntCount&, CntrControl%(0), MemHandle&)
      If ULStat& <> 0 Then Stop
   
      tmrReadStatus.Enabled = True
      FirstPoint& = 0
   End If
   
End Sub

Private Sub cmdStopRead_Click()

   ' the BACKGROUND operation must be explicitly stopped

   ' Parameters:
   '   BoardNum   :the number used by CB.CFG to describe this board
   '   FunctionType:counter operation (CTRFUNCTION)
      
   ULStat& = cbStopBackground(BoardNum, CTRFUNCTION)

   ULStat& = cbWinBufFree(MemHandle&)      ' Free up memory for use by
   If ULStat& <> 0 Then Stop                ' other programs

   End
  
End Sub

