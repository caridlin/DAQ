VERSION 5.00
Begin VB.Form frmDataDisplay 
   Appearance      =   0  'Flat
   BackColor       =   &H80000005&
   Caption         =   "Universal Library Counter Input Scan"
   ClientHeight    =   4890
   ClientLeft      =   2850
   ClientTop       =   1620
   ClientWidth     =   5370
   BeginProperty Font 
      Name            =   "MS Sans Serif"
      Size            =   8.25
      Charset         =   0
      Weight          =   700
      Underline       =   0   'False
      Italic          =   0   'False
      Strikethrough   =   0   'False
   EndProperty
   ForeColor       =   &H00FF0000&
   LinkMode        =   1  'Source
   LinkTopic       =   "Form1"
   PaletteMode     =   1  'UseZOrder
   ScaleHeight     =   4890
   ScaleWidth      =   5370
   Begin VB.CommandButton cmdStopConvert 
      Appearance      =   0  'Flat
      Caption         =   "Quit"
      Height          =   390
      Left            =   4320
      TabIndex        =   2
      Top             =   4320
      Width           =   870
   End
   Begin VB.CommandButton cmdStart 
      Appearance      =   0  'Flat
      Caption         =   "Start"
      Height          =   390
      Left            =   3240
      TabIndex        =   3
      Top             =   4320
      Width           =   870
   End
   Begin VB.Label lblCounterData 
      Alignment       =   2  'Center
      BackColor       =   &H80000005&
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   4
      Left            =   1800
      TabIndex        =   9
      Top             =   2880
      Width           =   1575
   End
   Begin VB.Label lblCounterData 
      Alignment       =   2  'Center
      BackColor       =   &H80000005&
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   3
      Left            =   1800
      TabIndex        =   8
      Top             =   2520
      Width           =   1575
   End
   Begin VB.Label lblCounterData 
      Alignment       =   2  'Center
      BackColor       =   &H80000005&
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   2
      Left            =   1800
      TabIndex        =   7
      Top             =   2160
      Width           =   1575
   End
   Begin VB.Label lblCounterData 
      Alignment       =   2  'Center
      BackColor       =   &H80000005&
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   1
      Left            =   1800
      TabIndex        =   6
      Top             =   1800
      Width           =   1575
   End
   Begin VB.Label lblCounterData 
      Alignment       =   2  'Center
      BackColor       =   &H80000005&
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   0
      Left            =   1800
      TabIndex        =   5
      Top             =   1440
      Width           =   1575
   End
   Begin VB.Label lblNote 
      Alignment       =   2  'Center
      BackColor       =   &H80000005&
      Caption         =   $"CInScan02.frx":0000
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H00FF0000&
      Height          =   735
      Left            =   240
      TabIndex        =   4
      Top             =   3480
      Width           =   4935
   End
   Begin VB.Label lblChan0 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Counter 0"
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
      Left            =   2160
      TabIndex        =   1
      Top             =   960
      Width           =   975
   End
   Begin VB.Label lblDemoFunction 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Demonstration of cbCConfigScan() and cbCInScan()"
      ForeColor       =   &H80000008&
      Height          =   615
      Left            =   120
      TabIndex        =   0
      Top             =   120
      Width           =   5055
   End
End
Attribute VB_Name = "frmDataDisplay"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
'===========================================================================

' File:                         CInScan02.vbp

' Library Call Demonstrated:    cbCConfigScan() and cbCInScan&(), FOREGROUND mode

' Purpose:                      Scans a Counter Input in decrement mode and stores
'                               the sample data in an array.

' Demonstration:                Displays counts on counter 0.

' Other Library Calls:          cbErrHandling&()

' Special Requirements:         Board 0 must support counter scan function.
'                               TTL signal on counter one input.

'==========================================================================
Option Explicit

Const BoardNum As Long = 0             ' Board number
Const CounterType As Integer = CTRSCAN ' counter type compatible
                                       ' with this example program
Dim CounterNum As Long                 ' number of counter used
Const NumPoints As Long = 100          ' Number of data points to collect
Const FirstPoint As Long = 0           ' set first element in buffer to transfer to array
Dim CounterData() As Long              ' dimension an array to hold the input values
Dim MemHandle As Long                  ' define a variable to contain the handle for
                                       ' memory allocated by Windows through cbWinBufAlloc&()
Dim LastCtr As Long
Dim ULStat As Long

Private Sub Form_Load()

   Dim NumCntrs As Long
   
   ' declare revision level of Universal Library
   ULStat = cbDeclareRevision(CURRENTREVNUM)
   
   ' Initiate error handling
   '  activating error handling will trap errors like
   '  bad channel numbers and non-configured conditions.

   '  Parameters:
   '    DONTPRINT   :all warnings and errors encountered will not be printed
   '    DONTSTOP    :if an error is encountered, the program will not stop,
   '                  errors must be handled locally

   ULStat& = cbErrHandling(DONTPRINT, DONTSTOP)
   If ULStat <> 0 Then Stop

   ' If cbErrHandling& is set for STOPALL or STOPFATAL during the program
   ' design stage, Visual Basic will be unloaded when an error is encountered.
   ' We suggest trapping errors locally until the program is ready for compiling
   ' to avoid losing unsaved data during program design.  This can be done by
   ' setting cbErrHandling options as above and checking the value of ULStat&
   ' after a call to the library. If it is not equal to 0, an error has occurred.

   NumCntrs = FindCountersOfType(BoardNum, CounterType, CounterNum)
   If NumCntrs = 0 Then
      lblDemoFunction.Caption = "There are no scan counters on this device."
      lblDemoFunction.ForeColor = &HFF&
      cmdStart.Enabled = False
   Else
      MemHandle = cbWinBufAlloc32(NumPoints)      ' set aside memory to hold data
      If MemHandle = 0 Then Stop
      ReDim CounterData(NumPoints - 1)
   End If
   
End Sub

Private Sub cmdStart_Click()

   Dim FirstCtr As Long, LastCtr As Long
   Dim Mode As Long, DebounceTime As Long
   Dim DebounceMode As Long, EdgeDetection As Long
   Dim TickSize As Long, MapCounter As Long
   Dim CBCount As Long, CBRate As Long
   Dim Options As Long, i As Long
   
   cmdStart.Enabled = False
   FirstCtr = CounterNum
   LastCtr = CounterNum
   
   ' Setup Counter 0 for decrement mode mapped in from counter 1
   ' Parameters:
   '   BoardNum       :the number used by CB.CFG to describe this board
   '   CounterNum     :counter to set up
   '   Mode           :counter Mode
   '   DebounceTime   :debounce Time
   '   DebounceMode   :debounce Mode
   '   EdgeDetection  :determines whether the rising edge or falling edge is to be detected
   '   TickSize       :reserved.
   '   MapCounter     :mapped counter

    Mode = DECREMENT_ON
    DebounceTime = CTR_DEBOUNCE_NONE
    DebounceMode = 0
    EdgeDetection = CTR_RISING_EDGE
    TickSize = 0
    MapCounter = 1
    
    ULStat = cbCConfigScan(BoardNum, FirstCtr, Mode, DebounceTime, _
      DebounceMode, EdgeDetection, TickSize, MapCounter)
    If ULStat <> 0 Then Stop
    
   ' Collect the values with cbCInScan&()
   ' Parameters:
   '   BoardNum      :the number used by CB.CFG to describe this board
   '   FirstCtr      :the first counter of the scan
   '   LastCtr       :the last counter of the scan
   '   CBCount       :the total number of counter samples to collect
   '   CBRate        :sample rate
   '   CounterData   :the array for the collected data values
   '   Options       :data collection options

   CBCount = NumPoints     ' total number of data points to collect
   CBRate = 390            ' sampling rate (samples per second)
   Options = CTR32BIT
   
   ULStat = cbCInScan(BoardNum, FirstCtr, LastCtr, CBCount, CBRate, MemHandle, Options)
   
   If ULStat <> 0 And ULStat <> 91 Then Stop
   
   ' Transfer the data from the memory buffer set
   ' up by Windows to an array for use by this program
   
   ULStat = cbWinBufToArray32(MemHandle, CounterData(0), FirstPoint, CBCount)
   If ULStat <> 0 Then Stop
   
   For i = 0 To 4
      lblCounterData(i).Caption = Format$(CounterData(i), "0")
   Next i

   cmdStart.Enabled = True

End Sub

Private Sub cmdStopConvert_Click()
   
   ' Free up memory for use by other programs
   ULStat = cbWinBufFree(MemHandle)
   If ULStat <> 0 Then Stop
   
   End
   
End Sub

