VERSION 5.00
Begin VB.Form frmDataDisplay 
   Appearance      =   0  'Flat
   BackColor       =   &H80000005&
   Caption         =   "Universal Library Counter Input Scan"
   ClientHeight    =   6315
   ClientLeft      =   5400
   ClientTop       =   3495
   ClientWidth     =   4320
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
   ScaleHeight     =   6315
   ScaleWidth      =   4320
   StartUpPosition =   2  'CenterScreen
   Begin VB.TextBox txtEncoderValues 
      BeginProperty Font 
         Name            =   "Courier New"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H00FF0000&
      Height          =   4815
      Left            =   120
      MultiLine       =   -1  'True
      ScrollBars      =   2  'Vertical
      TabIndex        =   3
      Top             =   900
      Width           =   3975
   End
   Begin VB.CommandButton cmdStopConvert 
      Appearance      =   0  'Flat
      Caption         =   "Quit"
      Height          =   390
      Left            =   2160
      TabIndex        =   1
      Top             =   5820
      Width           =   870
   End
   Begin VB.CommandButton cmdStart 
      Appearance      =   0  'Flat
      Caption         =   "Start"
      Height          =   390
      Left            =   1080
      TabIndex        =   2
      Top             =   5820
      Width           =   870
   End
   Begin VB.Label lblDemoFunction 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Demonstration of cbCConfigScan() and cbCInScan() used with encoders"
      ForeColor       =   &H80000008&
      Height          =   735
      Left            =   120
      TabIndex        =   0
      Top             =   60
      Width           =   4035
   End
End
Attribute VB_Name = "frmDataDisplay"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
'=============================================================================

' File:                         CInScan03.vbp

' Library Call Demonstrated:    cbCConfigScan() and cbCInScan&(), FOREGROUND mode

' Purpose:                      Scans a Counter Input in encoder mode and stores
'                               the sample data in an array.

' Demonstration:                Displays counts from encoder as phase A, phase B,
'                               and totalizes the index on Z.

' Other Library Calls:          cbErrHandling&()
'                               MccDaq.MccService.WinBufAlloc32()
'                               MccDaq.MccService.WinBufToArray32()
'                               MccDaq.MccService.WinBufFree()

' Special Requirements:         Board 0 must support counter scans in encoder mode.
'                               Phase A from encode connected to counter 0 input.
'                               Phase B from encode connected to counter 1 input.
'                               Index Z from encode connected to counter 2 input.

'==========================================================================
Option Explicit

Const BoardNum As Long = 0     ' Board number

Dim CounterNum As Long          ' number of counter used
Const NumPoints As Long = 50    ' Number of data points to collect
Const FirstPoint As Long = 0    ' set first element in buffer to transfer to array
Dim CounterData() As Long       ' dimension an array to hold the input values
Dim MemHandle As Long           ' define a variable to contain the handle for
                                ' memory allocated by Windows through cbWinBufAlloc&()

Dim FirstCtr, LastCtr As Long
Dim ULStat As Long

Private Sub Form_Load()
   
   Dim NumCntrs As Long
   Dim CounterType As Integer

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
   
   CounterType = CTRQUAD  ' counter type compatible
                          ' with this example program
   NumCntrs = FindCountersOfType(BoardNum, CounterType, CounterNum)
   If NumCntrs = 0 Then
      CounterType = CTRSCAN
      NumCntrs = FindCountersOfType(BoardNum, CounterType, CounterNum)
      If NumCntrs > 0 Then _
         lblDemoFunction.Caption = "This device contains scan counters." & _
         " Make sure they are compatible with quadrature operations."
   End If
   If NumCntrs = 0 Then
      lblDemoFunction.ForeColor = &HFF&
      cmdStart.Enabled = False
   Else
      MemHandle = cbWinBufAlloc32(NumPoints)      ' set aside memory to hold data
      If MemHandle = 0 Then Stop
      ReDim CounterData(NumPoints - 1)
   End If
   
End Sub

Private Sub cmdStart_Click()
   
   Dim NumCtrs As Integer
   Dim FirstCtr As Long, Mode As Long
   Dim DebounceTime As Long, DebounceMode As Long
   Dim EdgeDetection As Long, TickSize As Long, MapChannel As Long
   Dim CBCount As Long, CBRate As Long, Options As Long
   Dim Element As Long
   
   cmdStart.Enabled = False
   FirstCtr = CounterNum
   LastCtr = CounterNum
   
   ' Setup Counters
   ' Parameters:
   '   BoardNum       :the number used by CB.CFG to describe this board
   '   CounterNum     :counter to set up
   '   Mode           :counter Mode
   '   DebounceTime   :debounce Time
   '   DebounceMode   :debounce Mode
   '   EdgeDetection  :determines whether the rising edge or falling edge is to be detected
   '   TickSize       :reserved.
   '   MapCounter     :mapped counter

   ' Setup Counter 0 (Phase A signal)
   
   Mode = ENCODER Or ENCODER_MODE_X1 Or CLEAR_ON_Z_ON
   DebounceTime = CTR_DEBOUNCE_NONE
   DebounceMode = 0
   EdgeDetection = CTR_RISING_EDGE
   TickSize = 0
   MapChannel = 2

   ULStat = cbCConfigScan(BoardNum, FirstCtr, Mode, _
      DebounceTime, DebounceMode, EdgeDetection, TickSize, MapChannel)
   If ULStat <> 0 Then Stop
    
   ' Collect the values with cbCInScan()
   ' Parameters:
   '   BoardNum      :the number used by CB.CFG to describe this board
   '   FirstCtr      :the first counter of the scan
   '   LastCtr       :the last counter of the scan
   '   CBCount       :the total number of counter samples to collect
   '   CBRate        :sample rate
   '   CounterData   :the array for the collected data values
   '   Options       :data collection options
   
   CBCount = NumPoints            ' total number of data points to collect
   CBRate = 10                    ' sampling rate (samples per second)
   Options = CTR32BIT
   
   If MemHandle = 0 Then Stop     ' check that a handle to a memory buffer exists
   
   ULStat = cbCInScan(BoardNum, FirstCtr, LastCtr, CBCount, CBRate, MemHandle, Options)
   If ULStat <> 0 Then Stop
   
   ' Transfer the data from the memory buffer set up
   ' by Windows to an array for use by this program
   ULStat = cbWinBufToArray32(MemHandle, CounterData(0), FirstPoint, CBCount)
   If ULStat <> 0 Then Stop
   
   txtEncoderValues.Text = "   Counter Data" & vbTab & vbCrLf & vbCrLf & vbTab
   For Element = 0 To NumPoints - 1
      Me.txtEncoderValues.Text = Me.txtEncoderValues.Text & _
      Format(CounterData(Element), "0") & vbTab & vbCrLf & vbTab
   Next

   cmdStart.Enabled = True

End Sub

Private Sub cmdStopConvert_Click()
   
   ' Free up memory for use by other programs
   ULStat = cbWinBufFree(MemHandle)
   If ULStat <> 0 Then Stop
   
   End
   
End Sub


