VERSION 5.00
Begin VB.Form frmStatusDisplay 
   Appearance      =   0  'Flat
   BackColor       =   &H80000005&
   Caption         =   "Universal Library Daq Input Scan"
   ClientHeight    =   4260
   ClientLeft      =   2820
   ClientTop       =   1620
   ClientWidth     =   6990
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
   ScaleHeight     =   4260
   ScaleWidth      =   6990
   Begin VB.CommandButton cmdQuit 
      Appearance      =   0  'Flat
      Caption         =   "Quit"
      Height          =   390
      Left            =   5940
      TabIndex        =   5
      Top             =   3720
      Width           =   780
   End
   Begin VB.Timer tmrCheckStatus 
      Enabled         =   0   'False
      Interval        =   200
      Left            =   5400
      Top             =   2700
   End
   Begin VB.CommandButton cmdStartBgnd 
      Appearance      =   0  'Flat
      Caption         =   "Start Background Operation"
      Height          =   396
      Left            =   2265
      TabIndex        =   4
      Top             =   2700
      Width           =   2700
   End
   Begin VB.CommandButton cmdStopConvert 
      Appearance      =   0  'Flat
      Caption         =   "Stop Background Operation"
      Enabled         =   0   'False
      Height          =   396
      Left            =   2265
      TabIndex        =   3
      Top             =   2700
      Visible         =   0   'False
      Width           =   2700
   End
   Begin VB.Label lblInstruction 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Board 0 must have at least one analog output."
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
      Height          =   735
      Left            =   1200
      TabIndex        =   20
      Top             =   540
      Width           =   4575
   End
   Begin VB.Label lblHighVal 
      Alignment       =   2  'Center
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   3
      Left            =   5460
      TabIndex        =   19
      Top             =   2040
      Width           =   1215
   End
   Begin VB.Label lblHighVal 
      Alignment       =   2  'Center
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   2
      Left            =   4140
      TabIndex        =   18
      Top             =   2040
      Width           =   1215
   End
   Begin VB.Label lblLowVal 
      Alignment       =   2  'Center
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   3
      Left            =   5460
      TabIndex        =   17
      Top             =   1680
      Width           =   1215
   End
   Begin VB.Label lblLowVal 
      Alignment       =   2  'Center
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   2
      Left            =   4140
      TabIndex        =   16
      Top             =   1680
      Width           =   1215
   End
   Begin VB.Label lblHighVal 
      Alignment       =   2  'Center
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   1
      Left            =   2820
      TabIndex        =   15
      Top             =   2040
      Width           =   1215
   End
   Begin VB.Label lblHighVal 
      Alignment       =   2  'Center
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   0
      Left            =   1500
      TabIndex        =   14
      Top             =   2040
      Width           =   1215
   End
   Begin VB.Label lblLowVal 
      Alignment       =   2  'Center
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   1
      Left            =   2820
      TabIndex        =   13
      Top             =   1680
      Width           =   1215
   End
   Begin VB.Label lblLowVal 
      Alignment       =   2  'Center
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   0
      Left            =   1500
      TabIndex        =   12
      Top             =   1680
      Width           =   1215
   End
   Begin VB.Label lblShowCount 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   210
      Left            =   2970
      TabIndex        =   11
      Top             =   3930
      Width           =   870
   End
   Begin VB.Label lblCount 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Current Count:"
      ForeColor       =   &H80000008&
      Height          =   210
      Left            =   1245
      TabIndex        =   9
      Top             =   3930
      Width           =   1545
   End
   Begin VB.Label lblShowIndex 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   210
      Left            =   2970
      TabIndex        =   10
      Top             =   3645
      Width           =   780
   End
   Begin VB.Label lblIndex 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Current Index:"
      ForeColor       =   &H80000008&
      Height          =   210
      Left            =   1245
      TabIndex        =   8
      Top             =   3645
      Width           =   1545
   End
   Begin VB.Label lblShowStat 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   210
      Left            =   3360
      TabIndex        =   7
      Top             =   3360
      Width           =   990
   End
   Begin VB.Label lblStatus 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Status of Background Operation:"
      ForeColor       =   &H80000008&
      Height          =   210
      Left            =   90
      TabIndex        =   6
      Top             =   3360
      Width           =   3180
   End
   Begin VB.Label lblOutputH 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Output(high):"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   60
      TabIndex        =   2
      Top             =   2040
      Width           =   1215
   End
   Begin VB.Label lblOutputL 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Output(low):"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   60
      TabIndex        =   1
      Top             =   1680
      Width           =   1215
   End
   Begin VB.Label lblDemoFunction 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Demonstration of cbAOutScan() with scan option set to SCALEDATA"
      ForeColor       =   &H80000008&
      Height          =   375
      Left            =   90
      TabIndex        =   0
      Top             =   90
      Width           =   6795
   End
End
Attribute VB_Name = "frmStatusDisplay"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
'ULAO0401.VBP================================================================

' File:                         ULAO04.VBP

' Library Call Demonstrated:    cbAOutScan() with SCALEDATA option

' Purpose:                      Synchronously writes to Analog
'                               output channels in the background.

' Demonstration:                Sends output to the D/A channels
'                               until a key is pressed.

'                               cbErrHandling()

' Special Requirements:         Board 0 must support cbDaqOutScan.
'

' (c) Copyright 2005-2011, Measurement Computing Corp.
' All rights reserved.
'==========================================================================
Option Explicit

Const BoardNum As Long = 0      ' Board number

Dim CBRange As Long
Dim DAResolution As Long, NumAOChans As Long
Dim LowChan As Long, MaxChan As Long
Dim NumPoints As Long

Dim DAData() As Double           ' dimension an array to hold the output values
Dim MemHandle As Long                  ' define a variable to contain the handle for
                                ' memory allocated by Windows through cbWinBufAlloc()
Dim UserTerm As Boolean
Dim ULStat As Long

Private Sub Form_Load()
   
   Dim ReportError As Long, HandleError As Long
   Dim DefaultTrig As Long, ChannelType As Long
   
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
   ChannelType = ANALOGOUTPUT
   NumAOChans = FindAnalogChansOfType(BoardNum, ChannelType, _
       DAResolution, CBRange, LowChan, DefaultTrig)

   If (NumAOChans = 0) Then
      lblInstruction.Caption = "Board " & Format(BoardNum, "0") & _
      " does not have analog output channels."
      cmdStartBgnd.Enabled = False
   Else
      If NumAOChans > 4 Then NumAOChans = 4
      NumPoints = NumAOChans * 2
      MaxChan = NumAOChans - 1
      ReDim DAData(NumPoints - 1)
      MemHandle& = cbScaledWinBufAlloc(NumPoints)      ' set aside memory to hold data
      If MemHandle = 0 Then Stop
      
      Dim FSRange As Single, i As Long
      Dim HighVal As Double, LowVal As Double
      Dim Offset As Double
      Offset = 0
      FSRange = GetRangeVolts(CBRange)
      If CBRange < 100 Then
         ' for bipolar, shift voltage by half range
         FSRange = FSRange / 2
         Offset = FSRange * -1
      End If
      For i = 0 To MaxChan
         LowVal = FSRange / 9 + Offset
         HighVal = FSRange - (FSRange / 9)
         DAData(i) = LowVal
         DAData(i + NumAOChans) = HighVal
         lblLowVal(i).Caption = Format$(DAData#(i), "0.00000000")
         lblHighVal(i).Caption = _
            Format$(DAData#(i + NumAOChans), "0.00000000")
      Next i
      
      Dim FirstPoint As Long
      FirstPoint = 0
      ULStat = cbScaledWinArrayToBuf(DAData#(0), MemHandle, FirstPoint, NumPoints)
      lblInstruction.Caption = "Board " & Format(BoardNum, "0") & _
          " generating analog output on up to " & Format(NumAOChans, "0") _
          & " channels using cbAOutScan() " & " at a Range of " & _
          GetRangeString(CBRange) & " using SCALEDATA option."
   End If
   
End Sub

Private Sub cmdStartBgnd_Click()

   Dim Options As Long, CBRate As Long
   Dim Status As Integer, CurCount As Long, CurIndex As Long
   
   cmdStartBgnd.Enabled = 0
   cmdStartBgnd.Visible = 0
   cmdStopConvert.Enabled = 1
   cmdStopConvert.Visible = 1
   cmdQuit.Enabled = 0
   UserTerm = False
   
   ' Collect the values with cbAOutScan()
   ' Parameters:
   '   BoardNum    :the number used by CB.CFG to describe this board
   '   LowChan    :the lower channel of the scan
   '   HighChan   :the upper channel of the scan
   '   CBCount&    :the number of D/A values to send
   '   CBRate&     :send rate in values/second (if supported by BoardNum)
   '   DAData     :array of values to send to the scanned channels
   '   Options    :data send options

   CBRate& = 1000       ' generate data at 1000 Hz
   Options = BACKGROUND + CONTINUOUS + SCALEDATA

   ULStat = cbAOutScan(BoardNum, LowChan, MaxChan, NumPoints, _
      CBRate&, CBRange, MemHandle, Options)
   If ULStat <> 0 Then Stop

   ULStat = cbGetStatus(BoardNum, Status, CurCount&, CurIndex&, AOFUNCTION)
   If ULStat <> 0 Then Stop

   If Status = RUNNING Then
      lblShowStat.Caption = "Running"
      lblShowCount.Caption = Format$(CurCount&, "0")
      lblShowIndex.Caption = Format$(CurIndex&, "0")
   End If

   tmrCheckStatus.Enabled = 1

End Sub

Private Sub tmrCheckStatus_Timer()

   Dim Status As Integer, CurCount As Long, CurIndex As Long

   ' This timer will check the status of the background data collection
   
   ' Parameters:
   '   BoardNum    :the number used by CB.CFG to describe this board
   '   Status     :current status of the background data collection
   '   CurCount&   :current number of samples transfered
   '   CurIndex&   :index to the last data value transferred
   '   FunctionType: A/D operation (AOFUNCTION)

   ULStat = cbGetStatus(BoardNum, Status, CurCount&, CurIndex&, AOFUNCTION)
   If ULStat <> 0 Then Stop

   lblShowCount.Caption = Format$(CurCount&, "0")
   lblShowIndex.Caption = Format$(CurIndex&, "0")

   ' Check if the background operation has finished. If it has, then
   ' transfer the data from the memory buffer set up by Windows to an
   ' array for use by Visual Basic
   ' The BACKGROUND operation must be explicitly stopped

   If Status = RUNNING And Not UserTerm Then
      lblShowStat.Caption = "Running"
      ULStat = cbGetStatus(BoardNum, Status, CurCount&, CurIndex&, AOFUNCTION)
      If ULStat <> 0 Then Stop
        lblShowCount.Caption = Format$(CurCount&, "0")
        lblShowIndex.Caption = Format$(CurIndex&, "0")
   ElseIf Status = IDLE Or UserTerm Then
      lblShowStat.Caption = "Idle"
      tmrCheckStatus.Enabled = 0

      ULStat = cbStopBackground(BoardNum, AOFUNCTION)
      If ULStat <> 0 Then Stop
      cmdStartBgnd.Enabled = 1
      cmdStartBgnd.Visible = 1
      cmdStopConvert.Enabled = 0
      cmdStopConvert.Visible = 0
      cmdQuit.Enabled = 1
  End If

End Sub

Private Sub cmdStopConvert_Click()

   UserTerm = True

End Sub

Private Sub cmdQuit_Click()
   
   ULStat = cbWinBufFree(MemHandle&)      ' Free up memory for use by
                                            ' other programs
   If ULStat <> 0 Then Stop
   End
   
End Sub


