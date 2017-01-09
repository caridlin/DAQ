VERSION 5.00
Begin VB.Form frm9513Freq 
   Appearance      =   0  'Flat
   BackColor       =   &H80000005&
   Caption         =   "Universal Library 9513 Counter Demo"
   ClientHeight    =   3750
   ClientLeft      =   105
   ClientTop       =   1440
   ClientWidth     =   5460
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
   ScaleHeight     =   3750
   ScaleWidth      =   5460
   Begin VB.CommandButton cmdStopRead 
      Appearance      =   0  'Flat
      Caption         =   "Quit"
      Default         =   -1  'True
      Height          =   390
      Left            =   3960
      TabIndex        =   1
      Top             =   3120
      Width           =   780
   End
   Begin VB.CommandButton cmdMeasureFreq 
      Appearance      =   0  'Flat
      Caption         =   "Measure Frequency"
      Height          =   375
      Left            =   1440
      TabIndex        =   7
      Top             =   3120
      Width           =   1935
   End
   Begin VB.Label lblFreq 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Left            =   3000
      TabIndex        =   3
      Top             =   2400
      Width           =   975
   End
   Begin VB.Label lblCount 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Left            =   1320
      TabIndex        =   2
      Top             =   2400
      Width           =   975
   End
   Begin VB.Label lblFrequency 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Frequency"
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
      Left            =   2880
      TabIndex        =   5
      Top             =   2040
      Width           =   1215
   End
   Begin VB.Label lblCountNum 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Number of Counts"
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
      Left            =   960
      TabIndex        =   4
      Top             =   2040
      Width           =   1695
   End
   Begin VB.Label lblInstruct 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   $"ULCT04.frx":0000
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
      Height          =   615
      Left            =   120
      TabIndex        =   6
      Top             =   1080
      Width           =   5175
   End
   Begin VB.Label lblDemoFunction 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Demonstration of Frequency Measurement using 9513 Counter"
      ForeColor       =   &H80000008&
      Height          =   660
      Left            =   120
      TabIndex        =   0
      Top             =   240
      Width           =   5190
   End
End
Attribute VB_Name = "frm9513Freq"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
'ULCT04.VBP===============================================================

' File:                         ULCT04.VBP

' Library Call Demonstrated:    cbCFreqIn&()

' Purpose:                      Measure the frequency of a signal.

' Demonstration:                Initializes the counter and measures a frequency.

' Other Library Calls:          cbErrHandling&()

' Special Requirements:         Board 0 must have a 9513 Counter.
'                               External freq. at counter 1 input.
'                               (100Hz < freq < 330kHz)
'                               External connection from counter
'                               4 output to counter 5 gate.

' (c) Copyright 2005-2011, Measurement Computing Corp.
' All rights reserved.
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

   Dim NumCntrs As Long, TimeOfDay As Long
   Dim FOutDivider As Long
   Dim Source As Long, Compare1 As Long, Compare2 As Long
   
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
   
   NumCntrs& = FindCountersOfType(BoardNum, CounterType, CounterNum)
   If NumCntrs& = 0 Then
      lblInstruct.Caption = "Board " & Format(BoardNum, "0") & _
         " has no 9513 counters."
      cmdMeasureFreq.Enabled = False
   Else
      RegName = Choose(CounterNum + 1, LOADREG0, LOADREG1, _
         LOADREG2, LOADREG3, LOADREG4, LOADREG5)
    
      ' Initialize the board level features
      '  Parameters:
      '    BoardNum    :the number used by CBCONFIG to describe this board
      '    ChipNum&    :chip to be initialized (1 for CTR5, 1 or 2 for CTR10)
      '    FOutDivider&:the F-Out divider (0-15)
      '    Source&     :the signal source for F-Out
      '    Compare1&   :status of comparator 1
      '    Compare2&   :status of comparator 2
      '    TimeOfDay&  :time of day mode control
   
      FOutDivider& = 1         ' sets up OSC OUT for 10kHz signal which can
      Source& = FREQ3          ' be used as frequency source for this example
      Compare1& = CBDISABLED
      Compare2& = CBDISABLED
      TimeOfDay& = CBDISABLED
   
      ULStat& = cbC9513Init(BoardNum, ChipNum&, FOutDivider&, _
         Source&, Compare1&, Compare2&, TimeOfDay&)
      If ULStat& <> 0 Then Stop
      lblInstruct.Caption = _
         "There must be a TTL pulse at counter 1 input on board " & _
         Format(BoardNum, "0") & " with a frequency between " & _
         "100Hz and  600kHz. Also, connect the output of " & _
         "counter 4 to the gate of counter 5."
   End If
   
End Sub

Private Sub cmdMeasureFreq_Click()

   Dim SigSource As Long, GateInterval As Long
   Dim CBCount As Integer, Freq As Long, ActualCounts As Long
   
   ' Measure the frequency of the internally-generated signal
   '  Parameters:
   '    BoardNum      :the number used by CB.CFG to describe this board
   '    SigSource&    :the counter to be measured (1 to 5)
   '    GateInterval& :gating interval in millseconds
   '    CBCount%      :the raw count during GateInterval& is returned here
   '    Freq&         :the calculated frequency (Hz) is returned here

   GateInterval& = 100
   SigSource& = CTRINPUT1
  
   ULStat& = cbCFreqIn(BoardNum, SigSource&, GateInterval&, CBCount%, Freq&)
   If ULStat& <> 0 Then Stop

   If CBCount% < 0 Then
      ActualCounts& = 65535 + CBCount%
   Else
      ActualCounts& = CBCount%
   End If

   lblCount.Caption = Format$(ActualCounts&, "0")
   lblFreq.Caption = Format$(Freq&, "0\H\z")

End Sub

Private Sub cmdStopRead_Click()

   End

End Sub

