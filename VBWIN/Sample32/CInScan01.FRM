VERSION 5.00
Begin VB.Form frmDataDisplay 
   Appearance      =   0  'Flat
   BackColor       =   &H80000005&
   Caption         =   "Universal Library Counter Input Scan"
   ClientHeight    =   3480
   ClientLeft      =   2850
   ClientTop       =   1620
   ClientWidth     =   5235
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
   ScaleHeight     =   3480
   ScaleWidth      =   5235
   Begin VB.TextBox txtHighChan 
      Appearance      =   0  'Flat
      Height          =   285
      Left            =   3450
      TabIndex        =   12
      Text            =   "3"
      Top             =   1320
      Width           =   495
   End
   Begin VB.CommandButton cmdStopConvert 
      Appearance      =   0  'Flat
      Caption         =   "Quit"
      Height          =   390
      Left            =   4200
      TabIndex        =   9
      Top             =   2880
      Width           =   870
   End
   Begin VB.CommandButton cmdStart 
      Appearance      =   0  'Flat
      Caption         =   "Start"
      Height          =   390
      Left            =   3120
      TabIndex        =   10
      Top             =   2880
      Width           =   870
   End
   Begin VB.Label lblInstruction 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "TTL signals must be present on selected counter inputs for count to increment."
      ForeColor       =   &H000000FF&
      Height          =   495
      Left            =   480
      TabIndex        =   13
      Top             =   600
      Width           =   4335
   End
   Begin VB.Label Label1 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Measure Channels 0 to"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   1290
      TabIndex        =   11
      Top             =   1320
      Width           =   2055
   End
   Begin VB.Label lblCounterData 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   3
      Left            =   3990
      TabIndex        =   8
      Top             =   2370
      Width           =   975
   End
   Begin VB.Label lblChan3 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Counter 3:"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   2910
      TabIndex        =   4
      Top             =   2370
      Width           =   975
   End
   Begin VB.Label lblCounterData 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   2
      Left            =   3990
      TabIndex        =   7
      Top             =   1995
      Width           =   975
   End
   Begin VB.Label lblChan2 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Counter 2:"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   2910
      TabIndex        =   3
      Top             =   1995
      Width           =   975
   End
   Begin VB.Label lblCounterData 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   1
      Left            =   1230
      TabIndex        =   6
      Top             =   2325
      Width           =   975
   End
   Begin VB.Label lblChan1 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Counter 1:"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   150
      TabIndex        =   2
      Top             =   2325
      Width           =   975
   End
   Begin VB.Label lblCounterData 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   0
      Left            =   1230
      TabIndex        =   5
      Top             =   1950
      Width           =   975
   End
   Begin VB.Label lblChan0 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Counter 0:"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   150
      TabIndex        =   1
      Top             =   1950
      Width           =   975
   End
   Begin VB.Label lblDemoFunction 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Demonstration of cbCInScan() in FOREGROUND mode"
      ForeColor       =   &H80000008&
      Height          =   255
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
'============================================================================

' File:                         CInScan01.vbp

' Library Call Demonstrated:    cbCInScan&(), FOREGROUND mode

' Purpose:                      Scans a range of Counter Input Channels and stores
'                               the sample data in an array.

' Demonstration:                Displays the counter input on four channels.

' Other Library Calls:          cbErrHandling&()

' Special Requirements:         Board 0 must support counter scan function.
'                               TTL signals on selected counter inputs.

'==========================================================================
Option Explicit

Const BoardNum As Long = 0              ' Board number

Const CounterType As Integer = CTRSCAN  ' counter type compatible
                                        ' with this example program
Dim NumCntrs As Long                    ' number of counters on device
Dim CounterNum As Long                  ' number of counter used
Const NumPoints As Long = 600           ' Number of data points to collect
Const FirstPoint As Long = 0            ' set first element in buffer to transfer to array
Dim CounterData() As Long               ' dimension an array to hold the input values
Dim MemHandle As Long                   ' define a variable to contain the handle for
                                        ' memory allocated by Windows through cbWinBufAlloc&()
Dim ULStat As Long
Dim MaxCtr As Long

Private Sub Form_Load()

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
   ' setting cbErrHandling options as above and checking the value of ULStat
   ' after a call to the library. If it is not equal to 0, an error has occurred.

   NumCntrs = FindCountersOfType(BoardNum, CounterType, CounterNum)
   If NumCntrs = 0 Then
      lblInstruction.Caption = "There are no scan counters on this device."
      cmdStart.Enabled = False
      txtHighChan.Enabled = False
   Else
      MemHandle = cbWinBufAlloc32(NumPoints)      ' set aside memory to hold data
      If MemHandle = 0 Then Stop
      ReDim CounterData(NumPoints - 1)
      MaxCtr = CounterNum + NumCntrs - 1
   End If
   
End Sub

Private Sub cmdStart_Click()

   Dim FirstCtr As Long, LastCtr As Long
   Dim CBCount As Long, CBRate As Long
   Dim Options As Long
   Dim i As Integer, j As Integer
   
   cmdStart.Enabled = False
   
   ' Collect the values with cbCInScan&()
   ' Parameters:
   '   BoardNum     :the number used by CB.CFG to describe this board
   '   FirstCtr     :the first counter of the scan
   '   LastCtr      :the last counter of the scan
   '   CBCount      :the total number of counter samples to collect
   '   CBRate       :sample rate
   '   CounterData  :the array for the collected data values
   '   Options      :data collection options
   
   FirstCtr = CounterNum           ' first channel to acquire
   LastCtr = Val(txtHighChan.Text) ' last channel to acquire
   If (LastCtr > MaxCtr) Then LastCtr = MaxCtr
   txtHighChan.Text = Format(LastCtr, "0")

   CBCount = NumPoints            ' total number of data points to collect
   CBRate = 390                    ' sampling rate (samples per second)
   Options = CTR32BIT
   
   ULStat = cbCInScan(BoardNum, FirstCtr, LastCtr, CBCount, CBRate, MemHandle, Options)
   
   If ULStat <> 0 And ULStat <> 91 Then Stop
   
   ' Transfer the data from the memory buffer set up by Windows to an array for use by Visual Basic
   
   ULStat = cbWinBufToArray32(MemHandle, CounterData(0), FirstPoint, CBCount)
   If ULStat <> 0 Then Stop
   
   For i = 0 To LastCtr
      lblCounterData(i).Caption = Format$(CounterData(CBCount - 1 - LastCtr + i), "0")
   Next i

   For j = LastCtr + 1 To 3
        lblCounterData(j).Caption = ""
   Next j

   cmdStart.Enabled = True

End Sub

Private Sub cmdStopConvert_Click()
   
   ULStat = cbWinBufFree(MemHandle)      ' Free up memory for use by
                                           ' other programs
   If ULStat <> 0 Then Stop
   
   End
   
End Sub

