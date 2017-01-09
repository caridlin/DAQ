VERSION 5.00
Begin VB.Form frmCountTest 
   Appearance      =   0  'Flat
   BackColor       =   &H80000005&
   Caption         =   "Universal Library 8254 Counter"
   ClientHeight    =   3915
   ClientLeft      =   105
   ClientTop       =   1545
   ClientWidth     =   4200
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
   ScaleHeight     =   3915
   ScaleWidth      =   4200
   Begin VB.CommandButton cmdStopRead 
      Appearance      =   0  'Flat
      Caption         =   "Quit"
      Default         =   -1  'True
      Height          =   495
      Left            =   3000
      TabIndex        =   6
      Top             =   3120
      Width           =   855
   End
   Begin VB.Timer tmrReadCount 
      Enabled         =   0   'False
      Interval        =   500
      Left            =   1800
      Top             =   3120
   End
   Begin VB.Label lblShowCountRead 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Left            =   3000
      TabIndex        =   5
      Top             =   2520
      Width           =   975
   End
   Begin VB.Label lblCountRead 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Value read from counter:"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   720
      TabIndex        =   3
      Top             =   2520
      Width           =   2175
   End
   Begin VB.Label lblShowLoadVal 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Left            =   3000
      TabIndex        =   4
      Top             =   2040
      Width           =   975
   End
   Begin VB.Label lblCountLoaded 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   240
      TabIndex        =   2
      Top             =   2040
      Width           =   2655
   End
   Begin VB.Label lblNoteFreqIn 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "NOTE: There must be a TTL frequency at the counter input."
      ForeColor       =   &H000000FF&
      Height          =   495
      Left            =   360
      TabIndex        =   1
      Top             =   1200
      Width           =   3495
   End
   Begin VB.Label lblDemoFunction 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Demonstration of 8254 Counter Functions"
      ForeColor       =   &H80000008&
      Height          =   615
      Left            =   240
      TabIndex        =   0
      Top             =   240
      Width           =   3735
   End
End
Attribute VB_Name = "frmCountTest"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
'ULCT01.VBP================================================================

' File:                         ULCT01.VBP

' Library Call Demonstrated:    8254 Counter Functions
'                               cbC8254Config&()
'                               cbCLoad&()
'                               cbCIn&()

' Purpose:                      Operate the counter.

' Demonstration:                Configures, loads and reads the counter.

' Other Library Calls:          cbErrHandling&()

' Special Requirements:         Board 0 must have an 8254 Counter.

'==========================================================================
Option Explicit

Const BoardNum As Long = 0              ' Board number
Const CounterType As Integer = CTR8254  ' counter type compatible
                                        ' with this example program

Dim CounterNum As Long                  ' number of counter used
Dim RegName As Long                     ' register name of counter
Dim ULStat As Long

Private Sub Form_Load()

   Dim NumCntrs As Long
   Dim Config As Long, LoadValue As Long
   
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
   
   NumCntrs& = FindCountersOfType(BoardNum, CounterType, CounterNum)
   If NumCntrs& = 0 Then
      lblNoteFreqIn.Caption = "Board " & Format(BoardNum, "0") & _
         " has no 8254 counters."
   Else
      RegName = Choose(CounterNum + 1, LOADREG0, LOADREG1, LOADREG2, _
         LOADREG3, LOADREG4, LOADREG5)
   
      ' Configure the counter for desired operation
      '  Parameters:
      '    BoardNum    :the number used by CB.CFG to describe this board
      '    CounterNum  :the counter to be setup
      '    Config&     :the operation mode of counter to be configured
      
      Config& = HIGHONLASTCOUNT
      
      ULStat& = cbC8254Config(BoardNum, CounterNum, Config&)
      If ULStat& <> 0 Then Stop
      lblNoteFreqIn.Caption = "NOTE: There must be a TTL frequency at " & _
      "counter " & Format(CounterNum, "0") & " input on board " & _
      Format(BoardNum, "0") & "."
      ' Send a starting value to the counter with cbCLoad&()
      '  Parameters:
      '    BoardNum    :the number used by CB.CFG to describe this board
      '    RegName     :the register of the counter to be loading with the starting value
      '    LoadValue&  :the starting value to place in the counter
      
      LoadValue& = 1000
       
      ULStat& = cbCLoad(BoardNum, RegName, LoadValue&)
      If ULStat& <> 0 Then Stop
   
      lblCountLoaded.Caption = "Counter starting value loaded:"
      
      lblShowLoadVal.Caption = Format$(LoadValue&, "0")
      Me.tmrReadCount.Enabled = True
   End If
   
End Sub

Private Sub tmrReadCount_Timer()

   Dim CBCount As Integer
   
   ' Parameters:
   '   BoardNum        :the number used by CB.CFG to describe this board
   '   CounterNum      :the counter to be read
   '   CBCount%        :the value read from the counter
    
   ULStat& = cbCIn(BoardNum, CounterNum, CBCount%)
   If ULStat& <> 0 Then Stop
     
   lblShowCountRead.Caption = Format$(CBCount%, "0")

End Sub

Private Sub cmdStopRead_Click()
   
   End

End Sub

