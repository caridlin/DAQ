VERSION 5.00
Begin VB.Form frm7266Ctr 
   Appearance      =   0  'Flat
   BackColor       =   &H80000005&
   Caption         =   "Universal Library 7266 Counter Demo"
   ClientHeight    =   4440
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
   ScaleHeight     =   4440
   ScaleWidth      =   5085
   Begin VB.CommandButton cmdStopRead 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Quit"
      Default         =   -1  'True
      Height          =   396
      Left            =   3600
      TabIndex        =   4
      Top             =   3960
      Width           =   804
   End
   Begin VB.Timer tmrReadCounter 
      Enabled         =   0   'False
      Interval        =   100
      Left            =   4320
      Top             =   360
   End
   Begin VB.Label lblShowDirection 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Left            =   3480
      TabIndex        =   9
      Top             =   3240
      Width           =   1095
   End
   Begin VB.Label lblDirection 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Direction = "
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   960
      TabIndex        =   8
      Top             =   3240
      Width           =   2415
   End
   Begin VB.Label lblShowLoadVal 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Left            =   3480
      TabIndex        =   7
      Top             =   1320
      Width           =   1095
   End
   Begin VB.Label lblShowMaxVal 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Left            =   3480
      TabIndex        =   6
      Top             =   1800
      Width           =   1095
   End
   Begin VB.Label lblMaxCount 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Maximum count:"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   840
      TabIndex        =   5
      Top             =   1800
      Width           =   2415
   End
   Begin VB.Label lblShowReadVal 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Left            =   3480
      TabIndex        =   1
      Top             =   2760
      Width           =   1095
   End
   Begin VB.Label lblReadValue 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Value read from counter:"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   840
      TabIndex        =   3
      Top             =   2760
      Width           =   2415
   End
   Begin VB.Label lblLoadValue 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Initial count for counter:"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   840
      TabIndex        =   2
      Top             =   1320
      Width           =   2415
   End
   Begin VB.Label lblDemoFunction 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Demonstration of 7266 Counter Functions."
      ForeColor       =   &H80000008&
      Height          =   615
      Left            =   720
      TabIndex        =   0
      Top             =   240
      Width           =   3375
   End
End
Attribute VB_Name = "frm7266Ctr"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
'ULCT06.VBP===============================================================

' File:                         ULCT06.VBP

' Library Call Demonstrated:    7266 Counter Functions
'                               cbC7266Config()
'                               cbCLoad32()
'                               cbCIn32()

' Purpose:                      Operate the counter.

' Demonstration:                Configures, loads and checks
'                               the counter

' Other Library Calls:          cbErrHandling&()

' Special Requirements:         Board 0 must have a 7266 Counter.
'
'                               These functions are only supported in the
'                               32 bit version of the Universal Library
'
'==========================================================================
Option Explicit

Const BoardNum As Long = 0                ' Board number
Const CounterType As Integer = CTR7266    ' counter type compatible
                                          ' with this example program

Dim CounterNum As Long                    ' number of counter used
Dim ULStat As Long

Private Sub Form_Load()

   Dim NumCntrs As Long
   Dim Quadrature As Long, CountingMode As Long
   Dim DataEncoding As Long, IndexMode As Long
   Dim InvertIndex As Long, FlagPins As Long, GateEnable As Long
   Dim LoadValue As Long, RegName As Long
   
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
      lblDemoFunction.Caption = "Board " & Format(BoardNum, "0") & _
         " has no 7266 counters."
      lblDemoFunction.ForeColor = &HFF&
   Else
      ' set the configurable operations of the counter
      ' Parameters:
      '         BoardNum       :the number used by CB.CFG to describe this board
      '         CounterNum     :the counter to be configured (0-5)
      '         Quadrature     :Select type of counter input
      '         CountingMode   :Slects how counter will operate
      '         IndexMode      :Selects what index signal will control
      '         InvertIndex    :Set to ENABLED id index signal is inverted
      '         FlagPins       :Select which signals will drive Flag pins
      '         GateEnable     :Set to ENABLED to use external gating signal */
      Quadrature& = X1_QUAD
      CountingMode& = MODULO_N
      DataEncoding& = BINARY_ENCODING
      IndexMode& = INDEX_DISABLED
      InvertIndex& = CBDISABLED
      FlagPins& = CARRY_BORROW
      GateEnable& = CBDISABLED
      ULStat& = cbC7266Config(BoardNum, CounterNum, Quadrature&, _
         CountingMode&, DataEncoding&, IndexMode&, InvertIndex&, FlagPins&, GateEnable&)
      If ULStat& <> 0 Then Stop
   
      ' Send a starting value to the counter with cbCLoad&()
      '  Parameters:
      '    BoardNum    :the number used by CBCONFIG to describe this board
      '    RegName&    :the counter to be loaded with the starting value
      '    LoadValue&  :the starting value to place in the counter
   
       LoadValue& = 1000
       RegName& = COUNT1 + CounterNum& - 1
       ULStat& = cbCLoad32(BoardNum, RegName&, LoadValue&)
       lblShowLoadVal.Caption = Format$(LoadValue&, "0")
   
       LoadValue& = 2000
       RegName& = PRESET1 + CounterNum& - 1
       ULStat& = cbCLoad32(BoardNum, RegName&, LoadValue&)
       lblShowMaxVal.Caption = Format$(LoadValue&, "0")
       lblDemoFunction.Caption = _
         "Demonstration of 7266 Counter Functions using board " & _
         Format(BoardNum, "0") & "."
       tmrReadCounter.Enabled = True
   End If
   
End Sub

Private Sub tmrReadCounter_Timer()

   Dim CBCount As Long, StatusBits As Long
   
   ' Parameters:
   '   BoardNum    :the number used by CBCONFIG to describe this board
   '   CounterNum& :the counter to be read
   '   CBCount&    :the count value in the counter
    
   ULStat& = cbCIn32(BoardNum, CounterNum, CBCount&)
   If ULStat& <> 0 Then Stop
   lblShowReadVal.Caption = Format$(CBCount&, "0")

   ULStat& = cbCStatus(BoardNum, CounterNum, StatusBits&)
   If ULStat& <> 0 Then Stop
   If (StatusBits& And C_UP_DOWN) Then
      lblShowDirection.Caption = "UP"
    Else
      lblShowDirection.Caption = "DOWN"
    End If

End Sub

Private Sub cmdStopRead_Click()
   
   End

End Sub
