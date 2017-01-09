VERSION 5.00
Begin VB.Form frmDataDisplay 
   Appearance      =   0  'Flat
   BackColor       =   &H80000005&
   Caption         =   "Universal Library Analog Input Scan"
   ClientHeight    =   5445
   ClientLeft      =   2520
   ClientTop       =   1545
   ClientWidth     =   6015
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
   ScaleHeight     =   5445
   ScaleWidth      =   6015
   Begin VB.TextBox txtHighChan 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      Height          =   285
      Left            =   3780
      TabIndex        =   34
      Text            =   "0"
      Top             =   2160
      Width           =   495
   End
   Begin VB.CommandButton cmdStopConvert 
      Appearance      =   0  'Flat
      Caption         =   "Quit"
      Height          =   396
      Left            =   4815
      TabIndex        =   17
      Top             =   4710
      Width           =   780
   End
   Begin VB.CommandButton cmdConvData 
      Appearance      =   0  'Flat
      Caption         =   "Convert Data"
      Enabled         =   0   'False
      Height          =   396
      Left            =   3330
      TabIndex        =   19
      Top             =   1545
      Width           =   1560
   End
   Begin VB.CommandButton cmdStartAcq 
      Appearance      =   0  'Flat
      Caption         =   "Acquire Data"
      Height          =   396
      Left            =   1320
      TabIndex        =   18
      Top             =   1545
      Width           =   1560
   End
   Begin VB.Timer tmrCheckStatus 
      Enabled         =   0   'False
      Interval        =   200
      Left            =   5160
      Top             =   1620
   End
   Begin VB.Label lblInstruction 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   $"ULAI04.frx":0000
      ForeColor       =   &H000000FF&
      Height          =   975
      Left            =   240
      TabIndex        =   36
      Top             =   420
      Width           =   5535
   End
   Begin VB.Label Label1 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Measure Channels 0 to"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   1260
      TabIndex        =   35
      Top             =   2220
      Width           =   2475
   End
   Begin VB.Label lblADData 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   7
      Left            =   4740
      TabIndex        =   16
      Top             =   4020
      Width           =   975
   End
   Begin VB.Label lblShowTag 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   7
      Left            =   4380
      TabIndex        =   27
      Top             =   4020
      Width           =   255
   End
   Begin VB.Label lblChan7 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Channel 7:"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   3120
      TabIndex        =   8
      Top             =   4020
      Width           =   1155
   End
   Begin VB.Label lblADData 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   3
      Left            =   1920
      TabIndex        =   12
      Top             =   4020
      Width           =   975
   End
   Begin VB.Label lblShowTag 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   3
      Left            =   1560
      TabIndex        =   23
      Top             =   4020
      Width           =   255
   End
   Begin VB.Label lblChan3 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Channel 3:"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   300
      TabIndex        =   4
      Top             =   4020
      Width           =   1155
   End
   Begin VB.Label lblADData 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   6
      Left            =   4740
      TabIndex        =   15
      Top             =   3780
      Width           =   975
   End
   Begin VB.Label lblShowTag 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   6
      Left            =   4380
      TabIndex        =   26
      Top             =   3780
      Width           =   255
   End
   Begin VB.Label lblChan6 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Channel 6:"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   3120
      TabIndex        =   7
      Top             =   3780
      Width           =   1155
   End
   Begin VB.Label lblADData 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   2
      Left            =   1920
      TabIndex        =   11
      Top             =   3780
      Width           =   975
   End
   Begin VB.Label lblShowTag 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   2
      Left            =   1560
      TabIndex        =   22
      Top             =   3780
      Width           =   255
   End
   Begin VB.Label lblChan2 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Channel 2:"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   300
      TabIndex        =   3
      Top             =   3780
      Width           =   1155
   End
   Begin VB.Label lblADData 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   5
      Left            =   4740
      TabIndex        =   14
      Top             =   3540
      Width           =   975
   End
   Begin VB.Label lblShowTag 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   5
      Left            =   4380
      TabIndex        =   25
      Top             =   3540
      Width           =   255
   End
   Begin VB.Label lblChan5 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Channel 5:"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   3120
      TabIndex        =   6
      Top             =   3540
      Width           =   1155
   End
   Begin VB.Label lblADData 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   1
      Left            =   1920
      TabIndex        =   10
      Top             =   3540
      Width           =   975
   End
   Begin VB.Label lblShowTag 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   1
      Left            =   1560
      TabIndex        =   21
      Top             =   3540
      Width           =   255
   End
   Begin VB.Label lblChan1 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Channel 1:"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   300
      TabIndex        =   2
      Top             =   3540
      Width           =   1155
   End
   Begin VB.Label lblADData 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   4
      Left            =   4740
      TabIndex        =   13
      Top             =   3300
      Width           =   975
   End
   Begin VB.Label lblShowTag 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   4
      Left            =   4380
      TabIndex        =   24
      Top             =   3300
      Width           =   255
   End
   Begin VB.Label lblChan4 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Channel 4:"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   3120
      TabIndex        =   5
      Top             =   3300
      Width           =   1155
   End
   Begin VB.Label lblADData 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   0
      Left            =   1920
      TabIndex        =   9
      Top             =   3300
      Width           =   975
   End
   Begin VB.Label lblShowTag 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   0
      Left            =   1560
      TabIndex        =   20
      Top             =   3300
      Width           =   255
   End
   Begin VB.Label lblChan0 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Channel 0:"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   300
      TabIndex        =   1
      Top             =   3300
      Width           =   1155
   End
   Begin VB.Label lblData2 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Data"
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
      Height          =   210
      Left            =   4890
      TabIndex        =   33
      Top             =   2955
      Width           =   585
   End
   Begin VB.Label lblTag4 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Tags"
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
      Height          =   210
      Left            =   4125
      TabIndex        =   32
      Top             =   2955
      Width           =   690
   End
   Begin VB.Label lblData1 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Data"
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
      Height          =   210
      Left            =   2280
      TabIndex        =   29
      Top             =   2955
      Width           =   585
   End
   Begin VB.Label lblTag2 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Tags"
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
      Height          =   210
      Left            =   1410
      TabIndex        =   28
      Top             =   2955
      Width           =   690
   End
   Begin VB.Label lblTag3 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Channel"
      ForeColor       =   &H80000008&
      Height          =   210
      Left            =   4035
      TabIndex        =   31
      Top             =   2760
      Width           =   870
   End
   Begin VB.Label lblTag1 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Channel"
      ForeColor       =   &H80000008&
      Height          =   210
      Left            =   1320
      TabIndex        =   30
      Top             =   2760
      Width           =   870
   End
   Begin VB.Label lblDemoFunction 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Demonstration of cbAConvertData()"
      ForeColor       =   &H80000008&
      Height          =   330
      Left            =   180
      TabIndex        =   0
      Top             =   60
      Width           =   5685
   End
End
Attribute VB_Name = "frmDataDisplay"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
'ULAI04.VBP================================================================

' File:                         ULAI04.VBP

' Library Call Demonstrated:    cbAInScan() with cbAConvertData()

' Purpose:                      Demonstrate the conversion of data values
'                               after they have been collected.

' Demonstration:                Collects data from up to eight analog inputs.
'                               Data points are collected as raw data values and
'                               (for devices that store channel tags) are converted
'                               from 16-bit composite data to channel numbers and
'                               12-bit data values using Mccdaq.MccBoard.AConvertData.

' Other Library Calls:          cbAConvertData()
'                               cbGetStatus()
'                               cbStopBackground()
'                               cbErrHandling()

' Special Requirements:         Analog signals on eight input channels.

'==========================================================================
Option Explicit

Const BoardNum As Long = 0      ' Board number
Dim CBRange As Long
Dim ADResolution As Long, NumAIChans As Long
Dim HighChan As Long, LowChan As Long, MaxChan As Long

Const NumPoints As Long = 120   ' Number of data points to collect
Const FirstPoint As Long = 0    ' set first element in buffer to transfer to array


Dim MemHandle As Long           ' define a variable to contain the handle for
                                ' memory allocated by Windows through cbWinBufAlloc()
Dim ADData() As Integer         ' dimension an array to hold the input values
Dim ChanTags() As Integer       ' dimension an array to hold the channel tags
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
   Dim ChannelType As Long
   ChannelType = ANALOGINPUT
   NumAIChans = FindAnalogChansOfType(BoardNum, ChannelType, _
      ADResolution, CBRange, LowChan, DefaultTrig)

   If (NumAIChans = 0) Then
      lblInstruction.Caption = "Board " & Format(BoardNum, "0") & _
         " does not have analog input channels."
      cmdStartAcq.Enabled = False
      txtHighChan.Enabled = False
   ElseIf ADResolution > 16 Then
      lblInstruction.Caption = "Board " & Format(BoardNum, "0") & _
      " resolution is greater that 16-bits. AConvertData does not " _
      & "support high resolution data."
      cmdStartAcq.Enabled = False
      txtHighChan.Enabled = False
   Else
      ' set aside memory to hold data
      ReDim ADData(NumPoints)
      ReDim ChanTags(NumPoints)
      MemHandle = cbWinBufAlloc(NumPoints)
      If MemHandle = 0 Then Stop
      If (NumAIChans > 8) Then NumAIChans = 8 'limit to 8 for display
      MaxChan = LowChan + NumAIChans - 1
      lblInstruction.Caption = "Board " & Format(BoardNum, "0") & _
          " collecting analog data on up to " & Format(NumAIChans, "0") & _
          " channels using AInScan with Range set to " & GetRangeString(CBRange) & _
          ".  Click the Convert Data button when complete. This separates " & _
          "channel tags from data for devices that support it but is " & _
          "ignored for most devices."
   End If

End Sub

Private Sub cmdStartAcq_Click()

   Dim CBCount As Long
   Dim CBRate As Long
   Dim Options As Long
   Dim i As Integer
   
   ' Collect the values with cbAInScan()
   '  Parameters:
   '    BoardNum   :the number used by CB.CFG to describe this board
   '    LowChan    :the first channel of the scan
   '    HighChan   :the last channel of the scan
   '    CBCount    :the total number of A/D samples to collect
   '    CBRate&    :sample rate
   '    CBRange    :the gain for the board
   '    ADData     :the array for the collected data values
   '    Options    :data collection options

   For i = 0 To 7
      lblShowTag(i).Caption = ""
   Next i

   HighChan = Val(txtHighChan.Text) ' last channel to acquire
   If HighChan > MaxChan Then HighChan = MaxChan
   txtHighChan.Text = Format(HighChan, "0")

   CBCount = NumPoints    ' total number of data points to collect

   ' per channel sampling rate ((samples per second) per channel)
   CBRate = 1000 / ((HighChan - LowChan) + 1)
   Options = NOCONVERTDATA + BACKGROUND
                                    ' collect data in BACKGROUND mode
                                    ' as 16-bit values (no conversion)

   ULStat = cbAInScan(BoardNum, LowChan, HighChan, _
      CBCount, CBRate, CBRange, MemHandle, Options)
   If ULStat <> 0 Then Stop
  
   tmrCheckStatus.Enabled = 1

End Sub

Private Sub tmrCheckStatus_Timer()

   Dim Status As Integer
   Dim CurCount As Long, CurIndex As Long
   Dim i As Integer, j As Integer
   
   ' Check the current status of the background data collection
   '  Parameters:
   '    BoardNum    :the number used by CB.CFG to describe this board
   '    Status      :current status of the background data collection
   '    CurCount&   :current number of samples collected
   '    CurIndex&   :index to the data buffer pointing to the last value transferred
   '   FunctionType: A/D operation (AIFUNCTIOM)

   ' Transfer the data from the memory buffer set up by Windows
   ' to an array for use by Visual Basic
    
     
   ULStat = cbGetStatus(BoardNum, Status, CurCount, CurIndex, AIFUNCTION)
   If ULStat <> 0 Then Stop

   If Status = IDLE Then
      tmrCheckStatus.Enabled = False
      ULStat = cbStopBackground(BoardNum, AIFUNCTION)
      If ULStat <> 0 Then Stop

      ULStat = cbWinBufToArray(MemHandle, ADData(0), FirstPoint, NumPoints)
      If ULStat <> 0 Then Stop

      For i = 0 To HighChan
         lblADData(i).Caption = Format$(ADData(i), "0")
      Next i

      For j = HighChan + 1 To 7
         lblADData(j).Caption = ""
      Next j
      cmdConvData.Enabled = True
   End If

End Sub

Private Sub cmdConvData_Click()

   Dim i As Integer
   
   ' Use cbAConvertData() to convert the 16-bit values
   ' in ADData() to 12-bit values
   '  Parameters:
   '    NumPoints&     :the number of data values to convert
   '    ADData        :the array holding the 16-bit data values to be converted
        
   ULStat = cbAConvertData(BoardNum, NumPoints, ADData(0), ChanTags(0))
   If ULStat <> 0 Then Stop
  
   For i = 0 To HighChan
      lblADData(i).Caption = Format$(ADData(i), "0")
      lblShowTag(i).Caption = Format$(ChanTags(i), "0")
   Next i
   cmdConvData.Enabled = 0

End Sub

Private Sub cmdStopConvert_Click()
   
   ULStat = cbWinBufFree(MemHandle&)      ' Free up memory for use by
                                            ' other programs
   If ULStat <> 0 Then Stop
   End
   
End Sub


