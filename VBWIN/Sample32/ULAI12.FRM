VERSION 5.00
Begin VB.Form frmDataDisplay 
   Appearance      =   0  'Flat
   BackColor       =   &H80000005&
   Caption         =   "Universal Library Analog Input Scan"
   ClientHeight    =   4065
   ClientLeft      =   3075
   ClientTop       =   2025
   ClientWidth     =   5910
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
   ScaleHeight     =   4065
   ScaleWidth      =   5910
   Begin VB.TextBox txtHighChan 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      Height          =   285
      Left            =   3120
      TabIndex        =   21
      Text            =   "0"
      Top             =   1440
      Width           =   375
   End
   Begin VB.CommandButton cmdStart 
      Appearance      =   0  'Flat
      Caption         =   "Start"
      Height          =   375
      Left            =   3660
      TabIndex        =   19
      Top             =   3540
      Width           =   975
   End
   Begin VB.CommandButton cmdStopConvert 
      Appearance      =   0  'Flat
      Caption         =   "Quit"
      Height          =   375
      Left            =   4740
      TabIndex        =   17
      Top             =   3540
      Width           =   975
   End
   Begin VB.Timer tmrConvert 
      Enabled         =   0   'False
      Interval        =   500
      Left            =   360
      Top             =   3300
   End
   Begin VB.Label Label1 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Measure Channels 0 to"
      ForeColor       =   &H80000008&
      Height          =   240
      Left            =   960
      TabIndex        =   20
      Top             =   1485
      Width           =   2055
   End
   Begin VB.Label lblADData 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   7
      Left            =   3960
      TabIndex        =   16
      Top             =   2985
      Width           =   975
   End
   Begin VB.Label lblChan7 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Channel 7:"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   2760
      TabIndex        =   8
      Top             =   2985
      Width           =   1095
   End
   Begin VB.Label lblADData 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   3
      Left            =   1440
      TabIndex        =   12
      Top             =   2985
      Width           =   975
   End
   Begin VB.Label lblChan3 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Channel 3:"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   240
      TabIndex        =   4
      Top             =   2985
      Width           =   1095
   End
   Begin VB.Label lblADData 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   6
      Left            =   3960
      TabIndex        =   15
      Top             =   2595
      Width           =   975
   End
   Begin VB.Label lblChan6 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Channel 6:"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   2760
      TabIndex        =   7
      Top             =   2595
      Width           =   1095
   End
   Begin VB.Label lblADData 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   2
      Left            =   1440
      TabIndex        =   11
      Top             =   2595
      Width           =   975
   End
   Begin VB.Label lblChan2 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Channel 2:"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   240
      TabIndex        =   3
      Top             =   2595
      Width           =   1095
   End
   Begin VB.Label lblADData 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   5
      Left            =   3960
      TabIndex        =   14
      Top             =   2205
      Width           =   975
   End
   Begin VB.Label lblChan5 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Channel 5:"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   2760
      TabIndex        =   6
      Top             =   2205
      Width           =   1095
   End
   Begin VB.Label lblADData 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   1
      Left            =   1440
      TabIndex        =   10
      Top             =   2205
      Width           =   975
   End
   Begin VB.Label lblChan1 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Channel 1:"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   240
      TabIndex        =   2
      Top             =   2205
      Width           =   1095
   End
   Begin VB.Label lblADData 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   4
      Left            =   3960
      TabIndex        =   13
      Top             =   1830
      Width           =   975
   End
   Begin VB.Label lblChan4 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Channel 4:"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   2760
      TabIndex        =   5
      Top             =   1830
      Width           =   1095
   End
   Begin VB.Label lblADData 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   0
      Left            =   1440
      TabIndex        =   9
      Top             =   1830
      Width           =   975
   End
   Begin VB.Label lblChan0 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Channel 0:"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   240
      TabIndex        =   1
      Top             =   1845
      Width           =   1095
   End
   Begin VB.Label lblWarn 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "There must be a clock signal present at external clock input.  Otherwise, program will appear to hang."
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
      Height          =   915
      Left            =   360
      TabIndex        =   18
      Top             =   420
      Width           =   5250
   End
   Begin VB.Label lblDemoFunction 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Demonstration of cbAInScan() with EXTERNAL CLOCK"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   195
      TabIndex        =   0
      Top             =   90
      Width           =   5565
   End
End
Attribute VB_Name = "frmDataDisplay"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
'ULAI12.VBP================================================================

' File:                         ULAI12.VBP

' Library Call Demonstrated:    cbAInScan(), EXTCLOCK option

' Purpose:                      Scans a range of A/D Input Channels and stores
'                               the sample data in an array at a sample rate
'                               specified by an external clock.

' Demonstration:                Displays the analog input on two channels.

' Other Library Calls:          cbErrHandling()

' Special Requirements:         Board 0 must have an A/D converter and
'                               support the EXTCLOCK option.
'                               Analog signals on two input channels.
'                               Freq. on trigger 0 input.

'==========================================================================
Option Explicit

Const BoardNum As Long = 0          ' Board number

Dim CBRange As Long
Dim ADResolution As Long, NumAIChans As Long
Dim HighChan As Long, LowChan As Long, MaxChan As Long

Const NumPoints As Long = 8     ' Number of data points to collect
Const FirstPoint As Long = 0    ' set first element in buffer to transfer to array

Dim ADData() As Integer         ' dimension an array to hold the input values
Dim ADData32() As Long          ' dimension an array to hold the high resolution input values
Dim MemHandle As Long           ' define a variable to contain the handle for
                                ' memory allocated by Windows through cbWinBufAlloc()
Dim ULStat As Long

Private Sub Form_Load()

   Dim DefaultTrig As Long
   Dim ChannelType As Long
   Dim ReportError As Long, HandleError As Long
   
   ' declare revision level of Universal Library

   ULStat = cbDeclareRevision(CURRENTREVNUM)
   
   ' Initiate error handling
   '  activating error handling will trap errors like
   '  bad channel numbers and non-configured conditions.
   '  Parameters:
   '    PRINTALL    :all warnings and errors encountered will be printed
   '    DONTSTOP    :if an error is encountered, the program will not stop,
   '                 errors must be handled locally
     
   ReportError = PRINTALL
   HandleError = DONTSTOP
   ULStat = cbErrHandling(ReportError, HandleError)
   If ULStat <> 0 Then Stop
   SetAnalogIODefaults ReportError, HandleError
   
   ' If cbErrHandling is set for STOPALL or STOPFATAL during the program
   ' design stage, Visual Basic will be unloaded when an error is encountered.
   ' We suggest trapping errors locally until the program is ready for compiling
   ' to avoid losing unsaved data during program design.  This can be done by
   ' setting cbErrHandling options as above and checking the value of ULStat
   ' after a call to the library. If it is not equal to 0, an error has occurred.

   ' determine the number of analog channels and their capabilities
   ChannelType = ANALOGINPUT
   NumAIChans = FindAnalogChansOfType(BoardNum, ChannelType, _
      ADResolution, CBRange, LowChan, DefaultTrig)

   If (NumAIChans = 0) Then
      lblWarn.Caption = "Board " & Format(BoardNum, "0") & _
         " does not have analog input channels."
      cmdStart.Enabled = False
      txtHighChan.Enabled = False
   Else
      ' Check the resolution of the A/D data and allocate memory accordingly
      If ADResolution > 16 Then
         ' set aside memory to hold high resolution data
         ReDim ADData32(NumPoints - 1)
         MemHandle = cbWinBufAlloc32(NumPoints)
      Else
         ' set aside memory to hold data
         ReDim ADData(NumPoints - 1)
         MemHandle = cbWinBufAlloc(NumPoints)
      End If
      If MemHandle = 0 Then Stop
      If (NumAIChans > 8) Then NumAIChans = 8 'limit to 8 for display
      MaxChan = NumAIChans - 1
      lblWarn.Caption = "Board " & Format(BoardNum, "0") & _
         " collecting analog data on up to " & Format(NumAIChans, "0") & _
         " channels using AInScan paced by external clock with Range set to " _
         & GetRangeString(CBRange) & ". There must be a clock signal " & _
         "present at external clock input or program will appear to hang." & _
         " (Ctl-Break to abort.)"
      If MemHandle = 0 Then Stop
   End If

End Sub

Private Sub cmdStart_Click()

   tmrConvert.Enabled = True
   cmdStart.Enabled = False
   lblWarn.Caption = "Board " & Format(BoardNum, "0") & _
      " collecting analog data on up to " & Format(NumAIChans, "0") & _
      " channels using AInScan paced by external clock with Range set to " _
      & GetRangeString(CBRange) & ". There must be a clock signal " & _
      "present at external clock input or program will appear to hang." & _
      " (Ctl-Break to abort.)"

End Sub

Private Sub tmrConvert_Timer()

   Dim i As Integer, j As Integer
   Dim CBCount As Long, CBRate As Long, Options As Long
   
   ' Collect the values with cbAInScan()
   '  Parameters:
   '    BoardNum    :the number used by CB.CFG to describe this board
   '    LowChan    :the first channel of the scan
   '    HighChan   :the last channel of the scan
   '    CBCount&    :the total number of A/D samples to collect
   '    CBRate&     :sample rate
   '    CBRange        :the gain for the board
   '    ADData     :the array for the collected data values
   '    Options     :data collection options

   HighChan = Val(txtHighChan.Text) ' last channel to acquire
   If HighChan > MaxChan Then HighChan = MaxChan
   txtHighChan.Text = Format(HighChan, "0")

   CBCount& = NumPoints&      ' total number of data points to collect
   CBRate& = 100              ' sampling rate is ignored
   Options = CONVERTDATA + EXTCLOCK
                              ' return data as 12-bit values & use external clock
 
   ULStat = cbAInScan(BoardNum, LowChan, HighChan, _
      CBCount&, CBRate&, CBRange, MemHandle&, Options)
   If ULStat = INTERRUPTED Then
      Me.lblWarn.Caption = "Scan was interrupted using Ctl-Break. " & _
      "Click start to try again."
      tmrConvert.Enabled = False
      Me.cmdStart.Enabled = True
   ElseIf ULStat <> 0 Then
      Stop
   End If

   ' Transfer the data from the memory buffer set up by
   ' Windows to an array for use by this program
  
  If ADResolution > 16 Then
      ULStat = cbWinBufToArray32(MemHandle&, ADData32&(0), FirstPoint&, CBCount&)
      If ULStat <> 0 Then Stop
   
      For i = 0 To HighChan
         lblADData(i).Caption = Format$(ADData32&(i), "0")
      Next i
  Else
      ULStat = cbWinBufToArray(MemHandle&, ADData(0), FirstPoint&, CBCount&)
      If ULStat <> 0 Then Stop
   
      For i = 0 To HighChan
         lblADData(i).Caption = Format$(ADData(i), "0")
      Next i
   End If

   For j = HighChan + 1 To 7
        lblADData(j).Caption = ""
   Next j

End Sub

Private Sub cmdStopConvert_Click()
   
   ULStat = cbWinBufFree(MemHandle&)      ' Free up memory for use by
   If ULStat <> 0 Then Stop                ' other programs
   tmrConvert.Enabled = 0
   End
   
End Sub


