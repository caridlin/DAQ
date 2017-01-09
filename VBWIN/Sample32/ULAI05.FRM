VERSION 5.00
Begin VB.Form frmDataDisplay 
   Appearance      =   0  'Flat
   BackColor       =   &H80000005&
   Caption         =   "Universal Library Manual Conversion"
   ClientHeight    =   5805
   ClientLeft      =   2325
   ClientTop       =   1515
   ClientWidth     =   6000
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
   ScaleHeight     =   5805
   ScaleWidth      =   6000
   Begin VB.TextBox txtHighChan 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      Height          =   285
      Left            =   3720
      TabIndex        =   27
      Text            =   "0"
      Top             =   4440
      Width           =   375
   End
   Begin VB.CommandButton cmdQuit 
      Appearance      =   0  'Flat
      Caption         =   "Quit"
      Height          =   390
      Left            =   4920
      TabIndex        =   4
      Top             =   5160
      Width           =   780
   End
   Begin VB.CommandButton cmdStartConvert 
      Appearance      =   0  'Flat
      Caption         =   "Start"
      Default         =   -1  'True
      Height          =   390
      Left            =   3960
      TabIndex        =   3
      Top             =   5160
      Width           =   780
   End
   Begin VB.CommandButton cmdStopBackground 
      Appearance      =   0  'Flat
      Caption         =   "Stop Background"
      Enabled         =   0   'False
      Height          =   375
      Left            =   1200
      TabIndex        =   25
      Top             =   5160
      Width           =   2055
   End
   Begin VB.Timer tmrCheckStatus 
      Enabled         =   0   'False
      Interval        =   300
      Left            =   360
      Top             =   4680
   End
   Begin VB.Label lblInstruction 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   $"ULAI05.frx":0000
      ForeColor       =   &H000000FF&
      Height          =   975
      Left            =   240
      TabIndex        =   28
      Top             =   600
      Width           =   5535
   End
   Begin VB.Label lblMeas 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Measure Channels 0 to"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   1080
      TabIndex        =   26
      Top             =   4440
      Width           =   2535
   End
   Begin VB.Label lblConvData 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   9
      Left            =   4560
      TabIndex        =   24
      Top             =   3960
      Width           =   975
   End
   Begin VB.Label lblConvData 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   8
      Left            =   3480
      TabIndex        =   23
      Top             =   3960
      Width           =   975
   End
   Begin VB.Label lblConvData 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   7
      Left            =   2400
      TabIndex        =   22
      Top             =   3960
      Width           =   975
   End
   Begin VB.Label lblConvData 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   6
      Left            =   1320
      TabIndex        =   21
      Top             =   3960
      Width           =   975
   End
   Begin VB.Label lblConvData 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   5
      Left            =   240
      TabIndex        =   20
      Top             =   3960
      Width           =   975
   End
   Begin VB.Label lblConvData 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   4
      Left            =   4560
      TabIndex        =   19
      Top             =   3600
      Width           =   975
   End
   Begin VB.Label lblConvData 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   3
      Left            =   3480
      TabIndex        =   18
      Top             =   3600
      Width           =   975
   End
   Begin VB.Label lblConvData 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   2
      Left            =   2400
      TabIndex        =   17
      Top             =   3600
      Width           =   975
   End
   Begin VB.Label lblConvData 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   1
      Left            =   1320
      TabIndex        =   16
      Top             =   3600
      Width           =   975
   End
   Begin VB.Label lblConvData 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   0
      Left            =   240
      TabIndex        =   6
      Top             =   3600
      Width           =   975
   End
   Begin VB.Label lblVoltsRead 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Converted Value: the channel number and the 12-bit value after separation of channel bits from data bits:"
      ForeColor       =   &H80000008&
      Height          =   495
      Left            =   240
      TabIndex        =   5
      Top             =   3000
      Width           =   4815
   End
   Begin VB.Label lblShowRaw 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   9
      Left            =   4560
      TabIndex        =   15
      Top             =   2520
      Width           =   975
   End
   Begin VB.Label lblShowRaw 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   8
      Left            =   3480
      TabIndex        =   14
      Top             =   2520
      Width           =   975
   End
   Begin VB.Label lblShowRaw 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   7
      Left            =   2400
      TabIndex        =   13
      Top             =   2520
      Width           =   975
   End
   Begin VB.Label lblShowRaw 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   6
      Left            =   1320
      TabIndex        =   12
      Top             =   2520
      Width           =   975
   End
   Begin VB.Label lblShowRaw 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   5
      Left            =   240
      TabIndex        =   11
      Top             =   2520
      Width           =   975
   End
   Begin VB.Label lblShowRaw 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   4
      Left            =   4560
      TabIndex        =   10
      Top             =   2160
      Width           =   975
   End
   Begin VB.Label lblShowRaw 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   3
      Left            =   3480
      TabIndex        =   9
      Top             =   2160
      Width           =   975
   End
   Begin VB.Label lblShowRaw 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   2
      Left            =   2400
      TabIndex        =   8
      Top             =   2160
      Width           =   975
   End
   Begin VB.Label lblShowRaw 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   1
      Left            =   1320
      TabIndex        =   7
      Top             =   2160
      Width           =   975
   End
   Begin VB.Label lblShowRaw 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   0
      Left            =   240
      TabIndex        =   2
      Top             =   2160
      Width           =   975
   End
   Begin VB.Label lblRawData 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Value: the 16-bit integer read from the board:"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   240
      TabIndex        =   0
      Top             =   1800
      Width           =   3975
   End
   Begin VB.Label lblDemoFunction 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Demonstration of manual conversion of data"
      ForeColor       =   &H80000008&
      Height          =   375
      Left            =   120
      TabIndex        =   1
      Top             =   0
      Width           =   5730
   End
End
Attribute VB_Name = "frmDataDisplay"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
'ULAI05.VBP================================================================

' File:                         ULAI05.VBP

' Library Call Demonstrated:    cbAInScan() with manual data conversion

' Purpose:                      Demonstrate the manual conversion of data
'                               values after they have been collected.

' Demonstration:                Collects data from up to five analog input
'                               channels. Data are collected as 16-bit composite
'                               values containing channel numbers and 12-bit data
'                               (for devices that store data this way) and are
'                               separated into data and channel values using code.

' Other Library Calls:          cbGetStatus()
'                               cbStopBackground()
'                               cbErrHandling()

' Special Requirements:         Analog signal on up to 5 input channels.
'                               This example supports only devices that store channel
'                               tags, such as the CIO-DAS16 and CIO-DAS08 families.

' (c) Copyright 2005-2011, Measurement Computing Corp.
' All rights reserved.
'==========================================================================
Option Explicit

Const BoardNum As Long = 0         ' Board number
Dim CBRange As Long
Dim ADResolution As Long, NumAIChans As Long
Dim HighChan As Long, LowChan As Long, MaxChan As Long

Const NumPoints As Long = 120       ' Number of data points to collect
Const FirstPoint As Long = 0

Dim ADData() As Integer     'dimension an array to hold the input values
Dim ConvData() As Integer   'dimension an array to hold the converted data values

Dim MemHandle As Long
Dim ULStat As Long
Dim UserTerm As Boolean

Private Sub Form_Load()

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
      cmdStartConvert.Enabled = False
      txtHighChan.Enabled = False
   ElseIf ADResolution > 12 Then
      lblInstruction.Caption = "Board " & Format(BoardNum, "0") & _
         " resolution is greater than 12-bits. This sample applies " & _
         "only to 12-bit devices that contain composite data."
      cmdStartConvert.Enabled = False
      txtHighChan.Enabled = False
   Else
      ' set aside memory to hold data
      ReDim ADData(NumPoints)
      ReDim ConvData(NumPoints)
      MemHandle = cbWinBufAlloc(NumPoints)
      If MemHandle = 0 Then Stop
      If (NumAIChans > 5) Then NumAIChans = 5 'limit to 5 for display
      MaxChan = LowChan + NumAIChans - 1
      lblInstruction.Caption = "Board " & Format(BoardNum, "0") & _
         " collecting analog data on up to " & Format(NumAIChans, "0") & _
         " channels using AInScan with Range set to " & GetRangeString(CBRange) & _
         ".  For devices that store composite data, this sample separates " & _
         "channel tags from data manually. Most new designs do not support this."
   End If

End Sub

Private Sub cmdStartConvert_Click()
  
   Dim CBCount As Long
   Dim CBRate As Long
   Dim Options As Long
   
   cmdStartConvert.Enabled = 0
   cmdQuit.Enabled = 0
   cmdStopBackground.Enabled = -1

   'Collect the values with cbAInScan() in BACKGROUND mode
   '  Parameters:
   '     BoardNum   :the number used by CB.CFG to describe this board
   '     LowChan    :the first channel of the scan
   '     HighChan   :the last channel of the scan
   '     CBCount    :the total number of A/D samples to collect
   '     CBRate     :sample rate in samples per second
   '     CBRange    :the gain for the board
   '     ADData     :the array for the collected data values
   '     Options    :data collection options

   HighChan = Val(txtHighChan.Text) ' last channel to acquire
   If HighChan > MaxChan Then HighChan = MaxChan
   txtHighChan.Text = Format(HighChan, "0")

   CBCount = NumPoints                  ' total number of data points to collect
   CBRate = 1000 / ((HighChan - LowChan) + 1)
   Options = NOCONVERTDATA + BACKGROUND   ' collect data in BACKGROUND mode
                                          ' as 16-bit values (no conversion)

   ULStat = cbAInScan(BoardNum, LowChan, HighChan, CBCount, CBRate, CBRange, MemHandle, Options)
   If ULStat <> 0 Then Stop

   'during the BACKGROUND operation, check the status using a timer, then print the values
   tmrCheckStatus.Enabled = -1
   
End Sub

Private Sub tmrCheckStatus_Timer()

   Dim ULStat As Long
   Dim Status As Integer
   Dim CurCount As Long, CurIndex As Long
   
   'Check the current status of the background data collection
   '  Parameters:
   '     BoardNum   :the number used by CB.CFG to describe this board
   '     Status     :current status of the background data collection
   '     CurCount   :current number of samples collected
   '     CurIndex   :index to the data buffer pointing to the last value transferred
   '     FunctionType: A/D operation (AIFUNCTIOM)

   ULStat = cbGetStatus(BoardNum, Status, CurCount, CurIndex, AIFUNCTION)
   If ULStat <> 0 Then Stop

      ' check if the background operation has finished
   If Status = IDLE Or UserTerm Then
      
      'the BACKGROUND operation must be explicitly stopped
      ULStat = cbStopBackground(BoardNum, AIFUNCTION)
      If ULStat <> 0 Then Stop
      If MemHandle = 0 Then Stop
      ULStat = cbWinBufToArray(MemHandle, ADData(0), FirstPoint, NumPoints)
      If ULStat <> 0 Then Stop
      cmdStartConvert.Enabled = -1
      cmdQuit.Enabled = -1
      cmdStopBackground.Enabled = 0
      tmrCheckStatus.Enabled = 0
      Convert
   End If
   
   If UserTerm Then UserTerm = False
   
End Sub

Private Sub Convert()

   Dim K As Integer
   Dim channel As Integer
   
   'Show the data that were collected
   For K = 0 To 9
      lblShowRaw(K).Caption = Format$(ADData(K), "0")
   Next

   'Manually convert the 16-bit values to 12-bit values

   For K = 0 To 9
      'find the channel number in the first 4-bits
      channel = ADData(K) And 15   'AND Mask with 15

      'retrieve the data value from the next 12-bits
      ' and put them into ConvData() array
      ConvData(K) = ADData(K) \ 16 'div by 16 shifts 4 places to right

      'if the original value was negative, then we need to replace
      ' the highbit, which BASIC thought was a sign, rather than part
      ' of the value. Add 4096 to replace the highbit.
      If ADData(K) < 0 Then
         ConvData(K) = ConvData(K) + 4096
      End If

      lblConvData(K).Caption = Format$(channel, "0\,\ ") + Format$(ConvData(K), "0")
   Next

End Sub

Private Sub cmdStopBackground_Click()

   cmdQuit.Enabled = -1
   cmdStopBackground.Enabled = 0
   UserTerm = True

End Sub

Private Sub cmdQuit_Click()
   
   ULStat = cbWinBufFree(MemHandle)      ' Free up memory for use by
                                            ' other programs
   If ULStat <> 0 Then Stop
   End

End Sub


