VERSION 5.00
Begin VB.Form frmStatusDisplay 
   Appearance      =   0  'Flat
   BackColor       =   &H80000005&
   Caption         =   "Universal Library Analog Input Scan"
   ClientHeight    =   4860
   ClientLeft      =   2820
   ClientTop       =   1620
   ClientWidth     =   5805
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
   ScaleHeight     =   4860
   ScaleWidth      =   5805
   Begin VB.TextBox txtHighChan 
      Appearance      =   0  'Flat
      Height          =   285
      Left            =   3480
      TabIndex        =   27
      Text            =   "0"
      Top             =   1920
      Width           =   495
   End
   Begin VB.CommandButton cmdQuit 
      Appearance      =   0  'Flat
      Caption         =   "Quit"
      Height          =   390
      Left            =   4680
      TabIndex        =   19
      Top             =   4320
      Width           =   780
   End
   Begin VB.Timer tmrCheckStatus 
      Enabled         =   0   'False
      Interval        =   200
      Left            =   4560
      Top             =   1560
   End
   Begin VB.CommandButton cmdStartBgnd 
      Appearance      =   0  'Flat
      Caption         =   "Start Background Operation"
      Height          =   396
      Left            =   1365
      TabIndex        =   18
      Top             =   1440
      Width           =   2700
   End
   Begin VB.CommandButton cmdStopConvert 
      Appearance      =   0  'Flat
      Caption         =   "Stop Background Operation"
      Enabled         =   0   'False
      Height          =   396
      Left            =   1365
      TabIndex        =   17
      Top             =   1440
      Visible         =   0   'False
      Width           =   2700
   End
   Begin VB.Label lblInstruction 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Board 0 must have analog inputs that support paced acquisition."
      ForeColor       =   &H000000FF&
      Height          =   855
      Left            =   600
      TabIndex        =   28
      Top             =   480
      Width           =   4455
   End
   Begin VB.Label lblMeas 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Measure Channels 0 to"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   720
      TabIndex        =   26
      Top             =   1920
      Width           =   2655
   End
   Begin VB.Label lblShowCount 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   210
      Left            =   3090
      TabIndex        =   25
      Top             =   4410
      Width           =   870
   End
   Begin VB.Label lblCount 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Current Count:"
      ForeColor       =   &H80000008&
      Height          =   210
      Left            =   1365
      TabIndex        =   23
      Top             =   4410
      Width           =   1545
   End
   Begin VB.Label lblShowIndex 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   210
      Left            =   3090
      TabIndex        =   24
      Top             =   4125
      Width           =   780
   End
   Begin VB.Label lblIndex 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Current Index:"
      ForeColor       =   &H80000008&
      Height          =   210
      Left            =   1365
      TabIndex        =   22
      Top             =   4125
      Width           =   1545
   End
   Begin VB.Label lblShowStat 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   210
      Left            =   3480
      TabIndex        =   21
      Top             =   3720
      Width           =   990
   End
   Begin VB.Label lblStatus 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Status of Background Operation:"
      ForeColor       =   &H80000008&
      Height          =   210
      Left            =   210
      TabIndex        =   20
      Top             =   3720
      Width           =   3180
   End
   Begin VB.Label lblADData 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   7
      Left            =   4440
      TabIndex        =   16
      Top             =   3210
      Width           =   975
   End
   Begin VB.Label lblChan7 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Channel 7:"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   3000
      TabIndex        =   8
      Top             =   3210
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
      Top             =   3210
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
      Top             =   3210
      Width           =   1215
   End
   Begin VB.Label lblADData 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   6
      Left            =   4440
      TabIndex        =   15
      Top             =   2925
      Width           =   975
   End
   Begin VB.Label lblChan6 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Channel 6:"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   3000
      TabIndex        =   7
      Top             =   2925
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
      Top             =   2925
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
      Top             =   2925
      Width           =   1215
   End
   Begin VB.Label lblADData 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   5
      Left            =   4440
      TabIndex        =   14
      Top             =   2640
      Width           =   975
   End
   Begin VB.Label lblChan5 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Channel 5:"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   3000
      TabIndex        =   6
      Top             =   2640
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
      Top             =   2640
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
      Top             =   2640
      Width           =   1215
   End
   Begin VB.Label lblADData 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   4
      Left            =   4440
      TabIndex        =   13
      Top             =   2355
      Width           =   975
   End
   Begin VB.Label lblChan4 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Channel 4:"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   3000
      TabIndex        =   5
      Top             =   2355
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
      Top             =   2355
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
      Top             =   2355
      Width           =   1215
   End
   Begin VB.Label lblDemoFunction 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Demonstration of cbAInScan() in BACKGROUND mode"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   90
      TabIndex        =   0
      Top             =   90
      Width           =   5655
   End
End
Attribute VB_Name = "frmStatusDisplay"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
'ULAI03.VBP================================================================
' File:                         ULAI03.VBP

' Library Call Demonstrated:    cbAInScan(), BACKGROUND mode

' Purpose:                      Scans a range of A/D Input Channels and stores
'                               the sample data in an array.

' Demonstration:                Displays the analog input on up to eight channels.

' Other Library Calls:          cbGetStatus()
'                               cbStopBackground()
'                               cbErrHandling()

' Special Requirements:         Board 0 must have an A/D converter.
'                               Analog signals on eight input channels.

' (c) Copyright 2005-2011, Measurement Computing Corp.
' All rights reserved.
'==========================================================================
Option Explicit

Const BoardNum As Long = 0          ' Board number
Dim CBRange As Long
Dim ADResolution As Long, NumAIChans As Long
Dim HighChan As Long, LowChan As Long, MaxChan As Long

Const NumPoints As Long = 6000     ' Number of data points to collect
Const FirstPoint As Long = 0       ' set first element in buffer to transfer to array

Dim ADData() As Integer      ' dimension an array to hold the input values
Dim ADData32() As Long       ' dimension an array to hold the high resolution input values
Dim UserTerm As Boolean      ' flag to stop acquisition manually
Dim MemHandle As Long        ' define a variable to contain the handle for
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
   '    PRINTALL    :all warnings and errors encountered will be printed
   '    DONTSTOP    :if an error is encountered, the program will not stop,
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
   ChannelType = ANALOGINPUT
   NumAIChans = FindAnalogChansOfType(BoardNum, ChannelType, _
      ADResolution, CBRange, LowChan, DefaultTrig)

   If (NumAIChans = 0) Then
      lblInstruction.Caption = "Board " & Format(BoardNum, "0") & _
         " does not have analog input channels."
      cmdStartBgnd.Enabled = False
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
   End If

End Sub

Private Sub cmdStartBgnd_Click()

   Dim ULStat As Long
   Dim CBCount As Long
   Dim Options As Long
   Dim Status As Integer
   Dim CBRate As Long
   Dim CurCount As Long, CurIndex As Long
   
   cmdStartBgnd.Enabled = False
   cmdStartBgnd.Visible = False
   cmdStopConvert.Enabled = True
   cmdStopConvert.Visible = True
   cmdQuit.Enabled = False
   
   UserTerm = False            ' initialize user terminate flag

   ' Collect the values with cbAInScan()
   '  Parameters:
   '    BoardNum   :the number used by CB.CFG to describe this board
   '    LowChan    :the first channel of the scan
   '    HighChan   :the last channel of the scan
   '    CBCount    :the total number of A/D samples to collect
   '    CBRate     :sample rate
   '    CBRange        :the gain for the board
   '    ADData     :the array for the collected data values
   '    Options     :data collection options

   HighChan = Val(txtHighChan.Text) ' last channel to acquire
   If (HighChan > MaxChan) Then HighChan = MaxChan
   txtHighChan.Text = Format(HighChan, "0")
   
   CBCount = NumPoints            ' total number of data points to collect
   
   ' per channel sampling rate ((samples per second) per channel)
   CBRate = 1000 / ((HighChan - LowChan) + 1)
   Options = CONVERTDATA + BACKGROUND
                                    ' return data as 12-bit values
                                    ' collect data in BACKGROUND mode

   ULStat = cbAInScan(BoardNum, LowChan, HighChan, _
      CBCount, CBRate, CBRange, MemHandle, Options)
   If ULStat <> 0 Then Stop
   
   ULStat = cbGetStatus(BoardNum, Status%, CurCount, CurIndex, AIFUNCTION)
   If ULStat <> 0 Then Stop

   If Status% = RUNNING Then
      lblShowStat.Caption = "Running"
      lblShowCount.Caption = Format$(CurCount, "0")
      lblShowIndex.Caption = Format$(CurIndex, "0")
      tmrCheckStatus.Enabled = True
   End If


End Sub

Private Sub tmrCheckStatus_Timer()

   Dim Status As Integer
   Dim CurCount As Long, CurIndex As Long
   Dim i As Long, j As Long
   
   ' This timer will check the status of the background data collection
   
   ' Parameters:
   '   BoardNum    :the number used by CB.CFG to describe this board
   '   Status%     :current status of the background data collection
   '   CurCount   :current number of samples collected
   '   CurIndex   :index to the data buffer pointing to the start of the
   '                most recently collected scan
   '   FunctionType: A/D operation (AIFUNCTIOM)

   ULStat = cbGetStatus(BoardNum, Status, CurCount, CurIndex, AIFUNCTION)
   If ULStat <> 0 Then Stop


   lblShowCount.Caption = Format$(CurCount, "0")
   lblShowIndex.Caption = Format$(CurIndex, "0")

   ' Check if the background operation has finished. If it has, then
   ' transfer the data from the memory buffer set up by Windows to an
   ' array for use by Visual Basic
   ' The BACKGROUND operation must be explicitly stopped

   If Status = RUNNING And Not UserTerm Then
      lblShowStat.Caption = "Running"
   ElseIf Status = IDLE Or UserTerm Then
      lblShowStat.Caption = "Idle"
   ULStat = cbGetStatus(BoardNum, Status%, CurCount, CurIndex, AIFUNCTION)
      If ULStat <> 0 Then Stop
      lblShowCount.Caption = Format$(CurCount, "0")
      lblShowIndex.Caption = Format$(CurIndex, "0")
      tmrCheckStatus.Enabled = False
      If MemHandle = 0 Then Stop
            
      If ADResolution > 16 Then
        ULStat = cbWinBufToArray32(MemHandle, ADData32(0), FirstPoint, NumPoints)
        If ULStat <> 0 Then Stop
        
        For i = 0 To HighChan
           lblADData(i).Caption = Format$(ADData32(i), "0")
        Next i
      Else
        ULStat = cbWinBufToArray(MemHandle, ADData(0), FirstPoint, NumPoints)
        If ULStat <> 0 Then Stop
        
        For i = 0 To HighChan
           lblADData(i).Caption = Format$(ADData(i), "0")
        Next i
      End If

      For j = HighChan + 1 To 7
        lblADData(j).Caption = Format$("", "0")
      Next j

      ULStat = cbStopBackground(BoardNum, AIFUNCTION)
      If ULStat <> 0 Then Stop
      cmdStartBgnd.Enabled = True
      cmdStartBgnd.Visible = True
      cmdStopConvert.Enabled = False
      cmdStopConvert.Visible = False
      cmdQuit.Enabled = True
   End If

End Sub

Private Sub cmdStopConvert_Click()

   UserTerm = True

End Sub

Private Sub cmdQuit_Click()
   
   ULStat = cbWinBufFree(MemHandle)      ' Free up memory for use by
                                            ' other programs
   If ULStat <> 0 Then Stop
   End
   
End Sub


