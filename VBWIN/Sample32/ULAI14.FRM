VERSION 5.00
Begin VB.Form frmDataDisplay 
   Appearance      =   0  'Flat
   BackColor       =   &H80000005&
   Caption         =   "Universal Library Analog Input Scan"
   ClientHeight    =   5145
   ClientLeft      =   2835
   ClientTop       =   1560
   ClientWidth     =   5895
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
   ScaleHeight     =   5145
   ScaleWidth      =   5895
   Begin VB.TextBox txtHighChan 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      Height          =   285
      Left            =   3840
      TabIndex        =   20
      Text            =   "0"
      Top             =   1710
      Width           =   375
   End
   Begin VB.CommandButton cmdStopConvert 
      Appearance      =   0  'Flat
      Caption         =   "Quit"
      Height          =   390
      Left            =   4800
      TabIndex        =   17
      Top             =   4560
      Width           =   870
   End
   Begin VB.CommandButton cmdStart 
      Appearance      =   0  'Flat
      Caption         =   "Start"
      Height          =   390
      Left            =   3720
      TabIndex        =   18
      Top             =   4560
      Width           =   870
   End
   Begin VB.Label lblResult 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00C00000&
      Height          =   615
      Left            =   240
      TabIndex        =   22
      Top             =   3720
      Width           =   5415
   End
   Begin VB.Label lblInstruction 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Board 0 must have analog inputs that support paced acquisition and support analog triggering."
      ForeColor       =   &H000000FF&
      Height          =   1095
      Left            =   720
      TabIndex        =   21
      Top             =   480
      Width           =   4455
   End
   Begin VB.Label lblMeas 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Measure Channels 0 to "
      ForeColor       =   &H80000008&
      Height          =   240
      Left            =   1200
      TabIndex        =   19
      Top             =   1710
      Width           =   2535
   End
   Begin VB.Label lblADData 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   7
      Left            =   4320
      TabIndex        =   16
      Top             =   3270
      Width           =   975
   End
   Begin VB.Label lblChan7 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Channel 7:"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   2880
      TabIndex        =   8
      Top             =   3270
      Width           =   1215
   End
   Begin VB.Label lblADData 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   3
      Left            =   1680
      TabIndex        =   12
      Top             =   3270
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
      Top             =   3270
      Width           =   1215
   End
   Begin VB.Label lblADData 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   6
      Left            =   4320
      TabIndex        =   15
      Top             =   2895
      Width           =   975
   End
   Begin VB.Label lblChan6 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Channel 6:"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   2880
      TabIndex        =   7
      Top             =   2895
      Width           =   1215
   End
   Begin VB.Label lblADData 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   2
      Left            =   1680
      TabIndex        =   11
      Top             =   2895
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
      Top             =   2895
      Width           =   1215
   End
   Begin VB.Label lblADData 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   5
      Left            =   4320
      TabIndex        =   14
      Top             =   2505
      Width           =   975
   End
   Begin VB.Label lblChan5 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Channel 5:"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   2880
      TabIndex        =   6
      Top             =   2505
      Width           =   1215
   End
   Begin VB.Label lblADData 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   1
      Left            =   1680
      TabIndex        =   10
      Top             =   2505
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
      Top             =   2505
      Width           =   1215
   End
   Begin VB.Label lblADData 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   4
      Left            =   4320
      TabIndex        =   13
      Top             =   2130
      Width           =   975
   End
   Begin VB.Label lblChan4 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Channel 4:"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   2880
      TabIndex        =   5
      Top             =   2130
      Width           =   1215
   End
   Begin VB.Label lblADData 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   0
      Left            =   1680
      TabIndex        =   9
      Top             =   2130
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
      Top             =   2130
      Width           =   1215
   End
   Begin VB.Label lblDemoFunction 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Demonstration of cbSetTrigger() "
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   120
      TabIndex        =   0
      Top             =   120
      Width           =   5655
   End
End
Attribute VB_Name = "frmDataDisplay"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
'ULAI14.VBP================================================================

' File:                         ULAI14.VBP

' Library Call Demonstrated:    cbSetTrigger()

' Purpose:                      Selects the Trigger source. This trigger is
'                               used to initiate A/D conversion using
'                               cbAinScan, with EXTTRIGGER selected.

' Demonstration:                Selects the trigger source
'                               Displays the analog input on eight channels.

' Other Library Calls:          cbErrHandling()

' Special Requirements:         Board 0 must have software selectable
'                               triggering source and type.
'                               Board 0 must have an A/D converter.
'                               Analog signals on eight input channels.

'==========================================================================
Option Explicit

Const BoardNum As Long = 0             ' Board number
Dim CBRange As Long
Dim ADResolution As Long, NumAIChans As Long
Dim HighChan As Long, LowChan As Long, MaxChan As Long

Const NumPoints As Long = 600 ' Number of data points to collect
Const FirstPoint As Long = 0  ' set first element in buffer to transfer to array
Dim ADData() As Integer       ' dimension an array to hold the input values
Dim ADData32() As Long        ' dimension an array to hold the high resolution input values
Dim MemHandle As Long         ' define a variable to contain the handle for
                              ' memory allocated by Windows through cbWinBufAlloc()
Dim ULStat As Long

Private Sub Form_Load()

   Dim ULStat As Long
   Dim DefaultTrig As Long
   Dim ReportError As Long, HandleError As Long
   
   ' declare revision level of Universal Library

   ULStat = cbDeclareRevision(CURRENTREVNUM)
   
   ' Initiate error handling
   '  activating error handling will trap errors like
   '  bad channel numbers and non-configured conditions.

   '  Parameters:
   '     PRINTALL    :all warnings and errors encountered will be printed
   '     DONTSTOP    :if an error is encountered, the program will not stop,
   '                  errors must be handled locally
  
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
   Dim ChannelType As Long
   ChannelType = ATRIGIN
   NumAIChans = FindAnalogChansOfType(BoardNum, ChannelType, _
      ADResolution, CBRange, LowChan, DefaultTrig)

   If (NumAIChans = 0) Then
      lblInstruction.Caption = "Board " & Format(BoardNum, "0") & _
         " does not have analog input or it does not support analog trigger."
      cmdStart.Enabled = False
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
      MaxChan = LowChan + NumAIChans - 1
      lblInstruction.Caption = "Board " & Format(BoardNum, "0") & _
         " collecting analog data on up to " & Format(NumAIChans, "0") & _
         " channels using AInScan with Range set to " & GetRangeString(CBRange) & "."
      Me.lblInstruction.ForeColor = &HFF0000
   End If

End Sub

Private Sub cmdStart_Click()
   
   Dim TrigType As Long, CBCount As Long
   Dim CBRate As Long, Options As Long
   Dim TrigSource As String
   Dim highVal As Single      'high threshold in volts
   Dim lowVal As Single       'low threshold in volts
   Dim LowThreshold As Integer, HighThreshold As Integer
   Dim i As Integer, j As Integer

   cmdStart.Enabled = 0
   
   
   ' Select the trigger source using cbSetTrigger()
   ' Parameters:
   '   BoardNum      :the number used by CB.CFG to describe this board
   '   TrigType      :the type of triggering based on external trigger source
   '   LowThreshold  :Low threshold when the trigger input is analog
   '   HighThreshold :High threshold when the trigger input is analog
        
   TrigType = TRIGABOVE
   highVal = 1.53
   lowVal = 0.1
   
   TrigSource$ = "analog trigger input"
   If ATrigRes = 0 Then
      ULStat = cbFromEngUnits(BoardNum, CBRange, highVal, HighThreshold)
      ULStat = cbFromEngUnits(BoardNum, CBRange, lowVal, LowThreshold)
   Else
      'Use the value acquired from the AnalogIO module, since the resolution
      'of the input is different from the resolution of the trigger.
      'Calculate trigger based on resolution returned and trigger range.
      Dim VoltageRange As Single, FSCounts As Long
      Dim LSB As Single
      VoltageRange! = ATrigRange
      If ATrigRange = -1 Then
         VoltageRange! = GetRangeVolts(CBRange)
         TrigSource$ = "first channel in scan"
      End If
      FSCounts& = (2 ^ ATrigRes)
      LSB! = VoltageRange! / FSCounts&
      LowThreshold = (lowVal / LSB!) + (FSCounts& / 2)
      HighThreshold = (highVal / LSB!) + (FSCounts& / 2)
   End If
   
   lblInstruction.Caption = "Board " & Format(BoardNum, "0") & _
      " collecting analog data on up to " & Format(NumAIChans, "0") & _
      " channels using AInScan with Range set to " & GetRangeString(CBRange) & "."

   lblResult.Caption = "Waiting for a trigger at " & TrigSource$ & ". " & _
   "Trigger criterea: signal rising above " & Format(highVal, "0.00") & "V." & _
   " (Ctl-Break to abort.)"
   DoEvents
   
   ULStat = cbSetTrigger(BoardNum, TrigType, LowThreshold, HighThreshold)
   If ULStat <> NOERRORS Then
      Stop
   Else
      ' Collect the values with cbAInScan()
      ' Parameters:
      '   BoardNum   :the number used by CB.CFG to describe this board
      '   LowChan    :the first channel of the scan
      '   HighChan   :the last channel of the scan
      '   CBCount    :the total number of A/D samples to collect
      '   CBRate     :sample rate
      '   CBRange       :the gain for the board
      '   ADData     :the array for the collected data values
      '   Options    :data collection options
   
      HighChan = Val(txtHighChan.Text) ' last channel to acquire
      If HighChan > MaxChan Then HighChan = MaxChan
      txtHighChan.Text = Format(HighChan, "0")

      CBCount = NumPoints            ' total number of data points to collect
      
      ' per channel sampling rate ((samples per second) per channel)
      CBRate& = 1000 / ((HighChan - LowChan) + 1)
      Options = CONVERTDATA Or EXTTRIGGER  ' return data as 12-bit values
   
      ULStat = cbAInScan(BoardNum, LowChan, HighChan, CBCount, _
         CBRate, CBRange, MemHandle, Options)
         lblResult.Caption = ""
      If ULStat = INTERRUPTED Then
         Me.lblInstruction.Caption = "Scan interrupted while waiting " & _
         "for trigger on board " & Format(BoardNum, "0") & _
         ". Click Start to try again."
         Me.cmdStart.Enabled = True
         Exit Sub
      ElseIf (ULStat <> NOERRORS) And (ULStat <> FREERUNNING) Then
         Stop
      End If
   
      ' Transfer the data from the memory buffer set up by Windows to an array for use by Visual Basic
   
      If ADResolution > 16 Then
         ULStat = cbWinBufToArray32(MemHandle, ADData32(0), FirstPoint, CBCount)
         If ULStat <> 0 Then Stop
         
         For i = 0 To HighChan
           lblADData(i).Caption = Format$(ADData32(i), "0")
         Next i
      Else
         ULStat = cbWinBufToArray(MemHandle, ADData(0), FirstPoint, CBCount)
         If ULStat <> 0 Then Stop
      
         For i = 0 To HighChan
            lblADData(i).Caption = Format$(ADData(i), "0")
         Next i
      End If

      For j = HighChan + 1 To 7
           lblADData(j).Caption = ""
      Next j
   End If
   cmdStart.Enabled = -1

End Sub

Private Sub cmdStopConvert_Click()
   
   ULStat = cbWinBufFree(MemHandle)      ' Free up memory for use by
                                            ' other programs
   If ULStat <> 0 Then Stop
   
   End
   
End Sub


