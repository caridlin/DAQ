VERSION 5.00
Begin VB.Form frm9513Ctr 
   Appearance      =   0  'Flat
   BackColor       =   &H80000005&
   Caption         =   "Universal Library 9513 Counter Demo"
   ClientHeight    =   3645
   ClientLeft      =   105
   ClientTop       =   1545
   ClientWidth     =   5085
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
   ScaleHeight     =   3645
   ScaleWidth      =   5085
   Begin VB.CommandButton cmdStopRead 
      Appearance      =   0  'Flat
      Caption         =   "Quit"
      Default         =   -1  'True
      Height          =   396
      Left            =   3480
      TabIndex        =   5
      Top             =   2760
      Width           =   804
   End
   Begin VB.Timer tmrReadCounter 
      Enabled         =   0   'False
      Interval        =   500
      Left            =   4320
      Top             =   360
   End
   Begin VB.Label lblShowReadVal 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Left            =   3480
      TabIndex        =   2
      Top             =   1800
      Width           =   1095
   End
   Begin VB.Label lblReadValue 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   840
      TabIndex        =   4
      Top             =   1800
      Width           =   2415
   End
   Begin VB.Label lblShowLoadVal 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Left            =   3480
      TabIndex        =   1
      Top             =   1320
      Width           =   1095
   End
   Begin VB.Label lblLoadValue 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   840
      TabIndex        =   3
      Top             =   1320
      Width           =   2415
   End
   Begin VB.Label lblDemoFunction 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Demonstration of 9513 Counter Functions."
      ForeColor       =   &H80000008&
      Height          =   615
      Left            =   720
      TabIndex        =   0
      Top             =   240
      Width           =   3375
   End
End
Attribute VB_Name = "frm9513Ctr"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
'ULCT02.VBP===============================================================

' File:                         ULCT02.VBP

' Library Call Demonstrated:    9513 Counter Functions
'                               cbC9513Init&()
'                               cbC9513Config()
'                               cbCLoad&()
'                               cbCIn&()

' Purpose:                      Operate the counter.

' Demonstration:                Initializes, configures, loads and checks
'                               the counter

' Other Library Calls:          cbErrHandling&()

' Special Requirements:         Board 0 must have a 9513 Counter.
'                               Program uses internal clock to count.

'==========================================================================
Option Explicit

Const BoardNum As Long = 0                ' Board number
Const CounterType As Integer = CTR9513    ' counter type compatible
                                          ' with this example program

Dim CounterNum As Long                    ' number of counter used
Dim RegName As Long                       ' register name of counter
Const ChipNum& = 1                        ' use chip 1 for CTR05 or for first
                                          ' chip on CTR10 or CTR20
Dim ULStat As Long

Private Sub Form_Load()

   Dim CtrsFound As Long
   Dim FOutDivider As Long
   Dim FOutSource As Long, Compare1 As Long
   Dim Compare2 As Long, TimeOfDay As Long
   Dim GateControl As Long, CounterEdge As Long
   Dim CountSource As Long, SpecialGate As Long
   Dim Reload As Long, RecycleMode As Long
   Dim BCDMode As Long, CountDirection As Long, OutputControl As Long
   Dim LoadValue As Long
   
   ' declare revision level of Universal Library
   ULStat& = cbDeclareRevision(CURRENTREVNUM)
   
   ' Initiate error handling
   '  activating error handling will trap errors like
   '  bad channel numbers and non-configured conditions.
   '  Parameters:
   '    PRINTALL    :all warnings and errors encountered will not be printed
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
   
   ' Initialize the board level features
   '  Parameters:
   '    BoardNum       :the number used by CBCONFIG to describe this board
   '    ChipNum&       :Chip to be initialized
   '                   (1 for CTR05, up to 2 for CTR10 or 4 for CTR20)
   '    FOutDivider&   :the F-Out divider (0-15)
   '    FOutSource&    :the signal source for F-Out
   '    Compare1&      :status of comparator 1
   '    Compare2&      :status of comparator 2
   '    TimeOfDay&     :time of day mode control

   CtrsFound& = FindCountersOfType(BoardNum, CounterType, CounterNum)
   If Not CtrsFound& > 0 Then
      lblDemoFunction.Caption = "Board " & Format(BoardNum, "0") & _
         " has no 9513 counters."
      lblDemoFunction.ForeColor = &HFF&
   Else
      RegName = Choose(CounterNum + 1, LOADREG0, _
         LOADREG1, LOADREG2, LOADREG3, LOADREG4, LOADREG5)

      FOutDivider& = 0
      FOutSource& = FREQ4
      Compare1& = CBDISABLED
      Compare2& = CBDISABLED
      TimeOfDay& = CBDISABLED
   
      ULStat& = cbC9513Init(BoardNum, ChipNum&, _
         FOutDivider&, FOutSource&, Compare1&, Compare2&, TimeOfDay&)
      If ULStat& <> 0 Then Stop
   
      ' Set the configurable operations of the counter
      '  Parameters:
      '    BoardNum        :the number used by CBCONFIG to describe this board
      '    CounterNum      :the counter to be configured (1 to 5)
      '    GateControl&    :gate control value
      '    CounterEdge&    :which edge to count
      '    CountSource&    :signal source
      '    SpecialGate&    :status of special gate
      '    Reload&         :method of reloading
      '    RecyleMode&     :recyle mode
      '    BCDMode&        :counting mode, Binary or BCD
      '    CountDirection& :direction for the counting operation (COUNTUP or COUNTDOWN)
      '    OutputControl&  :output signal type and level
       
      GateControl& = NOGATE
      CounterEdge& = POSITIVEEDGE
      CountSource& = FREQ4
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
   
      ' Send a starting value to the counter with cbCLoad&()
      '  Parameters:
      '    BoardNum    :the number used by CBCONFIG to describe this board
      '    RegName     :the counter to be loaded with the starting value
      '    LoadValue&  :the starting value to place in the counter
       
      RegName = Choose(CounterNum + 1, LOADREG0, LOADREG1, _
         LOADREG2, LOADREG3, LOADREG4, LOADREG5)
      LoadValue& = 1000
     
      ULStat& = cbCLoad(BoardNum, RegName, LoadValue&)
      If ULStat& <> 0 Then Stop
   
      lblLoadValue.Caption = "Value loaded to counter " & Format(CounterNum, "0") & ":"
      lblShowLoadVal.Caption = Format$(LoadValue&, "0")
      Me.lblDemoFunction.Caption = _
         "Demonstration of 9513 Counter Functions using board " & _
         Format(BoardNum, "0") & "."
      tmrReadCounter.Enabled = True
   End If
   
End Sub

Private Sub tmrReadCounter_Timer()

   Dim CBCount As Integer
   
   ' Parameters:
   '   BoardNum    :the number used by CBCONFIG to describe this board
   '   CounterNum :the counter to be read
   '   CBCount%    :the count value in the counter
    
   ULStat& = cbCIn(BoardNum, CounterNum, CBCount%)
   If ULStat& <> 0 Then Stop

   lblReadValue.Caption = "Value read from counter " & Format(CounterNum, "0") & ":"
   lblShowReadVal.Caption = Format$(CBCount%, "0")

End Sub

Private Sub cmdStopRead_Click()
   
   End

End Sub
