VERSION 5.00
Begin VB.Form frmDataDisplay 
   Appearance      =   0  'Flat
   BackColor       =   &H80000005&
   Caption         =   "Universal Library Analog Input Scan"
   ClientHeight    =   4515
   ClientLeft      =   2160
   ClientTop       =   1965
   ClientWidth     =   6390
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
   ScaleHeight     =   4515
   ScaleWidth      =   6390
   Begin VB.TextBox txtHighChan 
      Appearance      =   0  'Flat
      Height          =   285
      Left            =   3060
      TabIndex        =   25
      Text            =   "0"
      Top             =   4035
      Width           =   375
   End
   Begin VB.CommandButton cmdStopConvert 
      Appearance      =   0  'Flat
      Caption         =   "Quit"
      Height          =   390
      Left            =   5400
      TabIndex        =   17
      Top             =   3960
      Width           =   750
   End
   Begin VB.CommandButton cmdStart 
      Appearance      =   0  'Flat
      Caption         =   "Start"
      Height          =   390
      Left            =   4080
      TabIndex        =   23
      Top             =   3960
      Width           =   750
   End
   Begin VB.Timer tmrConvert 
      Enabled         =   0   'False
      Interval        =   500
      Left            =   2820
      Top             =   2760
   End
   Begin VB.Frame fraMode 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Select Mode"
      ForeColor       =   &H80000008&
      Height          =   1215
      Left            =   150
      TabIndex        =   18
      Top             =   1290
      Width           =   6060
      Begin VB.OptionButton optModeBlock 
         Appearance      =   0  'Flat
         BackColor       =   &H80000005&
         Caption         =   "Block IO"
         ForeColor       =   &H80000008&
         Height          =   300
         Left            =   4575
         TabIndex        =   21
         Top             =   375
         Width           =   1260
      End
      Begin VB.OptionButton optModeDMA 
         Appearance      =   0  'Flat
         BackColor       =   &H80000005&
         Caption         =   "DMA IO"
         ForeColor       =   &H80000008&
         Height          =   300
         Left            =   3135
         TabIndex        =   20
         Top             =   375
         Width           =   1260
      End
      Begin VB.OptionButton optModeSing 
         Appearance      =   0  'Flat
         BackColor       =   &H80000005&
         Caption         =   "Single IO"
         ForeColor       =   &H80000008&
         Height          =   300
         Left            =   1575
         TabIndex        =   19
         Top             =   375
         Width           =   1260
      End
      Begin VB.OptionButton optDefault 
         Appearance      =   0  'Flat
         BackColor       =   &H80000005&
         Caption         =   "Default"
         ForeColor       =   &H80000008&
         Height          =   300
         Left            =   192
         TabIndex        =   22
         Top             =   375
         Value           =   -1  'True
         Width           =   1260
      End
      Begin VB.Label lblOptCode 
         Alignment       =   1  'Right Justify
         Appearance      =   0  'Flat
         BackColor       =   &H80000005&
         Caption         =   "Option code (in Hex):"
         ForeColor       =   &H80000008&
         Height          =   255
         Left            =   1440
         TabIndex        =   27
         Top             =   840
         Width           =   2055
      End
      Begin VB.Label lblModeNo 
         Appearance      =   0  'Flat
         BackColor       =   &H80000005&
         ForeColor       =   &H00FF0000&
         Height          =   255
         Left            =   3600
         TabIndex        =   26
         Top             =   840
         Width           =   375
      End
   End
   Begin VB.Label lblInstruction 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Board 0 must have analog inputs that support paced acquisition."
      ForeColor       =   &H000000FF&
      Height          =   735
      Left            =   540
      TabIndex        =   28
      Top             =   480
      Width           =   5355
   End
   Begin VB.Label Label1 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Measure Channels 0 to "
      ForeColor       =   &H80000008&
      Height          =   240
      Left            =   900
      TabIndex        =   24
      Top             =   4035
      Width           =   2055
   End
   Begin VB.Label lblADData 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   7
      Left            =   4740
      TabIndex        =   16
      Top             =   3450
      Width           =   1005
   End
   Begin VB.Label lblChan7 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Channel 7:"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   3660
      TabIndex        =   8
      Top             =   3450
      Width           =   975
   End
   Begin VB.Label lblADData 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   3
      Left            =   1440
      TabIndex        =   12
      Top             =   3450
      Width           =   1005
   End
   Begin VB.Label lblChan3 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Channel 3:"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   360
      TabIndex        =   4
      Top             =   3450
      Width           =   975
   End
   Begin VB.Label lblADData 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   6
      Left            =   4740
      TabIndex        =   15
      Top             =   3210
      Width           =   1005
   End
   Begin VB.Label lblChan6 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Channel 6:"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   3660
      TabIndex        =   7
      Top             =   3210
      Width           =   975
   End
   Begin VB.Label lblADData 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   2
      Left            =   1440
      TabIndex        =   11
      Top             =   3210
      Width           =   1005
   End
   Begin VB.Label lblChan2 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Channel 2:"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   360
      TabIndex        =   3
      Top             =   3210
      Width           =   975
   End
   Begin VB.Label lblADData 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   5
      Left            =   4740
      TabIndex        =   14
      Top             =   2970
      Width           =   1005
   End
   Begin VB.Label lblChan5 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Channel 5:"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   3660
      TabIndex        =   6
      Top             =   2970
      Width           =   975
   End
   Begin VB.Label lblADData 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   1
      Left            =   1440
      TabIndex        =   10
      Top             =   2970
      Width           =   1005
   End
   Begin VB.Label lblChan1 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Channel 1:"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   360
      TabIndex        =   2
      Top             =   2970
      Width           =   975
   End
   Begin VB.Label lblADData 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   4
      Left            =   4740
      TabIndex        =   13
      Top             =   2730
      Width           =   1005
   End
   Begin VB.Label lblChan4 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Channel 4:"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   3660
      TabIndex        =   5
      Top             =   2730
      Width           =   975
   End
   Begin VB.Label lblADData 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   0
      Left            =   1440
      TabIndex        =   9
      Top             =   2730
      Width           =   1005
   End
   Begin VB.Label lblChan0 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Channel 0:"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   360
      TabIndex        =   1
      Top             =   2730
      Width           =   975
   End
   Begin VB.Label lblDemoFunction 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Demonstration of cbAInScan() with user selected mode"
      ForeColor       =   &H80000008&
      Height          =   315
      Left            =   240
      TabIndex        =   0
      Top             =   75
      Width           =   5955
   End
End
Attribute VB_Name = "frmDataDisplay"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
'ULAI13.VBP================================================================

' File:                         ULAI13.VBP

' Library Call Demonstrated:    cbAInScan(), sampling mode options.

' Purpose:                      Scans a range of A/D Input Channels and stores
'                               the sample data in an array using a user
'                               specified sampling mode.

' Demonstration:                Displays the analog input on 8 channels.

' Other Library Calls:          cbErrHandling()

' Special Requirements:         Board 0 must have an A/D converter.
'                               Analog signals on two input channels.
'                               Board must support the mode selected.

'==========================================================================
Option Explicit

Const BoardNum As Long = 0         ' Board number

Dim CBRange As Long
Dim ADResolution As Long, NumAIChans As Long
Dim HighChan As Long, LowChan As Long, MaxChan As Long

Const NumPoints As Long = 1000     ' Number of data points to collect
Const FirstPoint As Long = 0       ' set first element in buffer to transfer to array

Dim ADData() As Integer     ' dimension an array to hold the input values
Dim ADData32() As Long      ' dimension an array to hold the high resolution input values
Dim MemHandle As Long       ' define a variable to contain the handle for
                            ' memory allocated by Windows through cbWinBufAlloc()
Dim Options As Long         ' define cbAInScan() Options as Module level variable
Dim ULStat As Long

Private Sub Form_Load()

   Dim DefaultTrig As Long, ChannelType As Long
   Dim ReportError As Long, HandleError As Long
   
   ' declare revision level of Universal Library

   ULStat = cbDeclareRevision(CURRENTREVNUM)
   
   ' Initiate error handling
   '  activating error handling will trap errors like
   '  bad channel numbers and non-configured conditions.
   '  Parameters:
   '    PRINTALL    :all warnings and errors encountered will be handled locally
   '    DONTSTOP    :if an error is encountered, the program will not stop,
   '                 errors must be handled locally
  
   ReportError = DONTPRINT
   HandleError = DONTSTOP
   ULStat& = cbErrHandling(ReportError, HandleError)
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
      lblInstruction.Caption = "Board " & Format(BoardNum, "0") & _
         " does not have analog input channels."
      cmdStart.Enabled = False
      fraMode.Enabled = False
      txtHighChan.Enabled = False
   Else
      ' Check the resolution of the A/D data and allocate memory accordingly
      If ADResolution > 16 Then
         ' set aside memory to hold high resolution data
         ReDim ADData32(NumPoints)
         MemHandle = cbWinBufAlloc32(NumPoints)
      Else
         ' set aside memory to hold data
         ReDim ADData(NumPoints)
         MemHandle = cbWinBufAlloc(NumPoints)
      End If
      If MemHandle = 0 Then Stop
      If (NumAIChans > 8) Then NumAIChans = 8 'limit to 8 for display
      MaxChan = NumAIChans - 1
      lblInstruction.Caption = "Board " & Format(BoardNum, "0") & _
         " collecting analog data on up to " & Format(NumAIChans, "0") & _
         " channels using AInScan with Range set to " & GetRangeString(CBRange) & "."
   End If

   Options = CONVERTDATA
   ShowOptionVal

End Sub

Private Sub cmdStart_Click()

   If tmrConvert.Enabled Then
      tmrConvert.Enabled = False
      cmdStart.Enabled = True
      cmdStart.Caption = "Start"
   Else
      lblInstruction.Caption = "Board " & Format(BoardNum, "0") & _
         " collecting analog data on up to " & Format(NumAIChans, "0") & _
         " channels using AInScan with Range set to " & GetRangeString(CBRange) & "."
      tmrConvert.Enabled = True
      cmdStart.Caption = "Stop"
   End If

End Sub

Private Sub optDefault_Click()

   Options = CONVERTDATA
   ShowOptionVal

End Sub

Private Sub optModeBlock_Click()

   Options = CONVERTDATA + BLOCKIO
   ShowOptionVal

End Sub

Private Sub optModeDMA_Click()

   Options = CONVERTDATA + DMAIO
   ShowOptionVal

End Sub

Private Sub optModeSing_Click()

   Options = CONVERTDATA + SINGLEIO
   ShowOptionVal

End Sub

Private Sub tmrConvert_Timer()

   Dim CBCount As Long, CBRate As Long
   
   ' Collect the values with cbAInScan()
   '  Parameters:
   '    BoardNum     :the number used by CB.CFG to describe this board
   '    LowChan      :the first channel of the scan
   '    HighChan     :the last channel of the scan
   '    CBCount&     :the total number of A/D samples to collect
   '    CBRate&      :sample rate
   '    CBRange      :the gain for the board
   '    ADData       :the array for the collected data values
   '    Options      :data collection options (determined by
   '                 Set Mode option buttons in this program)

   HighChan = Val(txtHighChan.Text) ' last channel to acquire
   If HighChan > MaxChan Then HighChan = MaxChan
   txtHighChan.Text = Format(HighChan, "0")

   ' per channel sampling rate ((samples per second) per channel)
   CBRate = 1000 / ((HighChan - LowChan) + 1)
   CBCount& = NumPoints&            ' total number of data points to collect

   If MemHandle& = 0 Then Stop      ' check that a handle to a memory buffer exists

   ULStat = cbAInScan(BoardNum, LowChan, HighChan, _
      CBCount&, CBRate&, CBRange, MemHandle&, Options)
   
   If ULStat = 91 Then
      ULStat = cbErrHandling(DONTPRINT, DONTSTOP)
      'Turn off library error handling for subsequent calls
   ElseIf ULStat <> 0 Then
      tmrConvert.Enabled = False
      cmdStart.Enabled = True
      cmdStart.Caption = "Start"
      lblInstruction.Caption = ErrorText(ULStat)
   End If

   ' Transfer the data from the memory buffer set up
   ' by Windows to an array for use by this program
   If ADResolution > 16 Then
      ULStat = cbWinBufToArray32(MemHandle&, ADData32&(0), FirstPoint&, CBCount&)
      If ULStat <> 0 Then Stop
      
      Dim i As Integer
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

End Sub

Private Sub ShowOptionVal()

   lblModeNo.Caption = Hex$(Options)

End Sub

Private Sub cmdStopConvert_Click()
   
   ULStat = cbWinBufFree(MemHandle&)      ' Free up memory for use by
   If ULStat <> 0 Then Stop                ' other programs
   End
   
End Sub

Private Function ErrorText(ByVal ErrorNumber As Long) As String

   Dim ErrMsg As String, NullLocation As Long
   
   'Initialize a string large enough to hold
   'the error message returned by cbGetErrMsg()
   ErrMsg = String(ERRSTRLEN, " ")
   ULStat = cbGetErrMsg(ErrorNumber, ErrMsg)
   NullLocation& = InStr(1, ErrMsg, Chr(0))
   ErrorText = Left(ErrMsg, NullLocation& - 1)
   
End Function


