VERSION 5.00
Begin VB.Form frm8536Count 
   Appearance      =   0  'Flat
   BackColor       =   &H80000005&
   Caption         =   "Universal Library 8536 Counter Demo"
   ClientHeight    =   3630
   ClientLeft      =   1095
   ClientTop       =   1500
   ClientWidth     =   5130
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
   ScaleHeight     =   3630
   ScaleWidth      =   5130
   Begin VB.CommandButton cmdQuit 
      Appearance      =   0  'Flat
      Caption         =   "Quit"
      Height          =   495
      Left            =   3960
      TabIndex        =   2
      Top             =   3000
      Width           =   975
   End
   Begin VB.CommandButton cmdStart 
      Appearance      =   0  'Flat
      Caption         =   "Start"
      Height          =   495
      Left            =   2640
      TabIndex        =   0
      Top             =   3000
      Width           =   975
   End
   Begin VB.Timer tmrReadCounter 
      Enabled         =   0   'False
      Interval        =   500
      Left            =   4560
      Top             =   120
   End
   Begin VB.Label lblCount 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Left            =   3840
      TabIndex        =   1
      Top             =   2280
      Width           =   975
   End
   Begin VB.Label Label1 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Value read from Counter 1: "
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   120
      TabIndex        =   7
      Top             =   2280
      Width           =   3615
   End
   Begin VB.Label lblShowLoadVal 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Left            =   3840
      TabIndex        =   6
      Top             =   1800
      Width           =   975
   End
   Begin VB.Label lblCountLoaded 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   120
      TabIndex        =   5
      Top             =   1800
      Width           =   3615
   End
   Begin VB.Label lblNoteFreqIn 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "NOTE: There must be a TTL frequency at the counter 1 input."
      ForeColor       =   &H000000FF&
      Height          =   495
      Left            =   240
      TabIndex        =   4
      Top             =   1080
      Width           =   4695
   End
   Begin VB.Label lblDemoFunction 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Demonstration of 8536 Counter Functions"
      ForeColor       =   &H80000008&
      Height          =   615
      Left            =   360
      TabIndex        =   3
      Top             =   120
      Width           =   3735
   End
End
Attribute VB_Name = "frm8536Count"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
'ULCT05.VBP================================================================

' File:                         ULCT05.VBP

' Library Call Demonstrated:    8536 Counter Functions
'                               cbC8536Init&()
'                               cbC8536Config&()
'                               cbCLoad&()
'                               cbCIn&()

' Purpose:                      Operate the counter.

' Demonstration:                Configures, loads and reads the counter.

' Other Library Calls:          cbErrHandling&()

' Special Requirements:         Board 0 must have an 8536 Counter.
'                               There must be a TTL freq. at cntr. 1 input.
'                               Gate line must be tied high.

' (c) Copyright 2005-2011, Measurement Computing Corp.
' All rights reserved.
'==========================================================================
Option Explicit

Const BoardNum As Long = 0                ' Board number

Const CounterType As Integer = CTR8536    ' counter type compatible
                                          ' with this example program

Dim CounterNum As Long                    ' number of counter used
Dim RegName As Long                       ' register name of counter
Dim NumCntrs As Long
Dim ULStat As Long

Private Sub Form_Load()

   Dim ChipNum As Long, Ctr1Output As Long
   Dim OutputControl As Long, RecycleMode As Long, TrigType As Long
   
   ' declare revision level of Universal Library
   ULStat& = cbDeclareRevision(CURRENTREVNUM)
   
   ' Initiate error handling
   '  activating error handling will trap errors like
   '  bad channel numbers and non-configured conditions.
   '  Parameters:
   '    PRINTALL    :all warnings and errors encountered will not be printed
   '    DONTSTOP    :if an error is encountered, the program will not stop,
   '                 errors must be handled locally

   ULStat& = cbErrHandling(DONTPRINT, DONTSTOP)
   If ULStat& <> 0 Then Stop

   ' If cbErrHandling& is set for STOPALL or STOPFATAL during the program
   ' design stage, Visual Basic will be unloaded when an error is encountered.
   ' We suggest trapping errors locally until the program is ready for compiling
   ' to avoid losing unsaved data during program design.  This can be done by
   ' setting cbErrHandling options as above and checking the value of ULStat&
   ' after a call to the library. If it is not equal to 0, an error has occurred.
   
   NumCntrs& = FindCountersOfType(BoardNum, CounterType, CounterNum)
   If NumCntrs& = 0 Then
      Me.lblNoteFreqIn.Caption = "Board " & Format(BoardNum, "0") & _
         " has no 8536 counters."
      cmdStart.Enabled = False
   Else
      RegName = Choose(CounterNum + 1, LOADREG0, LOADREG1, _
      LOADREG2, LOADREG3, LOADREG4, LOADREG5)
      'Init the counter for desired operation
      '  Parameters:
      '     BoardNum    :the number used by CB.CFG to describe this board
      '     ChipNum     :the chip to be setup
      '     Ctr1Output  :how the counter output is used
   
      ChipNum& = 1
      Ctr1Output& = NOTLINKED
   
      ULStat& = cbC8536Init(BoardNum, ChipNum&, Ctr1Output&)
      If ULStat& <> 0 Then Stop
   
      'Configure the counter for desired operation
      '  Parameters:
      '     BoardNum      :the number used by CB.CFG to describe this board
      '     CounterNum    :which counter
      '     OutputControl :which counter output signal is used
      '     RecycleMode   :reload at 0 ?
      '     TrigType      :trigger type
   
      OutputControl& = TOGGLEONTC
      RecycleMode& = RECYCLE
      TrigType& = HW_START_TRIG
   
      ULStat& = cbC8536Config(BoardNum, CounterNum, _
      OutputControl&, RecycleMode&, TrigType&)
      If ULStat& <> 0 Then Stop
      lblNoteFreqIn.Caption = _
         "NOTE: There must be a TTL frequency at counter " & _
         "1 input on board " & Format(BoardNum, "0") & "."
   End If
   
End Sub

Private Sub cmdStart_Click()
   
   Dim LoadValue As Long
   
   'Send a starting value to the counter with cbCLoad&()
   '  Parameters:
   '     BoardNum    :the number used by CB.CFG to describe this board
   '     RegName&    :the counter to be loading with the starting value
   '     LoadValue&  :the starting value to place in the counter

   LoadValue& = 1000

   ULStat& = cbCLoad(BoardNum, RegName, LoadValue&)
   If ULStat& <> 0 Then Stop

   'Start a timer to read counter at 500ms interval

   lblCountLoaded.Caption = "The value loaded into LOADREG1 was: "
   lblShowLoadVal.Caption = Format$(LoadValue&, "0")

   tmrReadCounter.Enabled = -1

End Sub

Private Sub tmrReadCounter_Timer()

   Dim CBCount As Integer
   
   'use this timer to keep checking the counter value with cbCIn&()
   
   '  Parameters:
   '     BoardNum    :the number used by CB.CFG to describe this board
   '     CounterNum  :the counter to be setup
   '     CBCount&    :the count value in the counter

   ULStat& = cbCIn(BoardNum, CounterNum, CBCount)
   If ULStat& <> 0 Then Stop

   lblCount.Caption = Format$(CBCount, "0")

End Sub

Private Sub cmdQuit_Click()

   End

End Sub


