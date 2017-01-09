VERSION 5.00
Begin VB.Form frmStatusDisplay 
   Appearance      =   0  'Flat
   BackColor       =   &H80000005&
   Caption         =   "Universal Library Daq Output Scan"
   ClientHeight    =   4260
   ClientLeft      =   2820
   ClientTop       =   1620
   ClientWidth     =   5790
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
   ScaleWidth      =   5790
   Begin VB.CommandButton cmdQuit 
      Appearance      =   0  'Flat
      Caption         =   "Quit"
      Height          =   390
      Left            =   4800
      TabIndex        =   5
      Top             =   3660
      Width           =   780
   End
   Begin VB.Timer tmrCheckStatus 
      Enabled         =   0   'False
      Interval        =   200
      Left            =   5100
      Top             =   2340
   End
   Begin VB.CommandButton cmdStartBgnd 
      Appearance      =   0  'Flat
      Caption         =   "Start Background Operation"
      Height          =   396
      Left            =   1605
      TabIndex        =   4
      Top             =   1500
      Width           =   2700
   End
   Begin VB.CommandButton cmdStopConvert 
      Appearance      =   0  'Flat
      Caption         =   "Stop Background Operation"
      Enabled         =   0   'False
      Height          =   396
      Left            =   1605
      TabIndex        =   3
      Top             =   1500
      Visible         =   0   'False
      Width           =   2700
   End
   Begin VB.Label lblInstruction 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H000000FF&
      Height          =   735
      Left            =   660
      TabIndex        =   19
      Top             =   540
      Width           =   4455
   End
   Begin VB.Label lblChan1 
      Alignment       =   2  'Center
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Left            =   3360
      TabIndex        =   18
      Top             =   2100
      Width           =   1335
   End
   Begin VB.Label lblChan0 
      Alignment       =   2  'Center
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Left            =   1680
      TabIndex        =   17
      Top             =   2100
      Width           =   1215
   End
   Begin VB.Label lblChan 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Channel:"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   240
      TabIndex        =   16
      Top             =   2100
      Width           =   1215
   End
   Begin VB.Label lblChan1Gain 
      Alignment       =   2  'Center
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Left            =   3360
      TabIndex        =   15
      Top             =   2820
      Width           =   1335
   End
   Begin VB.Label lblChan0Gain 
      Alignment       =   2  'Center
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Left            =   1680
      TabIndex        =   14
      Top             =   2820
      Width           =   1215
   End
   Begin VB.Label lblChan1Type 
      Alignment       =   2  'Center
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Left            =   3360
      TabIndex        =   13
      Top             =   2460
      Width           =   1335
   End
   Begin VB.Label lblChan0Type 
      Alignment       =   2  'Center
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Left            =   1680
      TabIndex        =   12
      Top             =   2460
      Width           =   1215
   End
   Begin VB.Label lblShowCount 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   210
      Left            =   2970
      TabIndex        =   11
      Top             =   3870
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
      Top             =   3870
      Width           =   1545
   End
   Begin VB.Label lblShowIndex 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   210
      Left            =   2970
      TabIndex        =   10
      Top             =   3585
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
      Top             =   3585
      Width           =   1545
   End
   Begin VB.Label lblShowStat 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   210
      Left            =   3360
      TabIndex        =   7
      Top             =   3300
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
      Top             =   3300
      Width           =   3180
   End
   Begin VB.Label lblChan2 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Gain:"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   240
      TabIndex        =   2
      Top             =   2820
      Width           =   1215
   End
   Begin VB.Label lblType 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Type:"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   240
      TabIndex        =   1
      Top             =   2460
      Width           =   1215
   End
   Begin VB.Label lblDemoFunction 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Demonstration of cbDaqOutScan() in BACKGROUND mode"
      ForeColor       =   &H80000008&
      Height          =   315
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
'===============================================================================

' File:                         DaqOutScan01.VBP

' Library Call Demonstrated:    cbDaqOutScan(), BACKGROUND mode

' Purpose:                      Synchronously writes to an  Analog channel and
'                               a Digital port in the background.

' Demonstration:                Sends output to the analog and digital channels
'                               until a key is pressed.

' Other Library Calls:          cbDConfigPort()
'                               cbErrHandling()

' Special Requirements:         Board 0 must support cbDaqOutScan.
'

'==========================================================================
Option Explicit

Const BoardNum As Long = 0      ' Board number

Const ChanCount& = 2            ' Number of channels in scan
Const NumPoints& = 2            ' Number of data points to collect
Const NumElements& = ChanCount& * NumPoints&

Dim DAData() As Integer         ' dimension an array to hold the output values
Dim MemHandle&                  ' define a variable to contain the handle for
                                ' memory allocated by Windows through cbWinBufAlloc()
Dim ChanArray() As Integer       ' array to hold channel queue information
Dim ChanTypeArray() As Integer   ' array to hold channel type information
Dim GainArray() As Integer       ' array to hold gain queue information
Dim ULStat As Long
Dim UserTerm As Boolean
Dim GeneralError As Boolean

Private Sub Form_Load()
   
   Dim ReportError As Long, HandleError As Long
   Dim FirstPoint As Long, ChannelType As Long
   Dim ADResolution As Long, CBRange As Long
   Dim LowChan As Long, DefaultTrig As Long
   Dim ProgAbility As Long, PortNum As Long
   Dim NumBits As Long, FirstBit As Long, NumPorts As Long
   Dim NumAOChans As Long
   
   ' declare revision level of Universal Library
   ULStat = cbDeclareRevision(CURRENTREVNUM)

   ' Initiate error handling
   '  activating error handling will trap errors like
   '  bad channel numbers and non-configured conditions.
   '  Parameters:
   '    DONTPRINT   :all warnings and errors encountered will not be printed
   '    DONTSTOP    :if an error is encountered, the program will not stop,
   '                  errors must be handled locally

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

   ChannelType = ANALOGOUTPUT
   NumAOChans = FindAnalogChansOfType(BoardNum, ChannelType, _
      ADResolution, CBRange, LowChan, DefaultTrig)
   GeneralError = AIOGeneralError
   ChannelType = PORTOUT
   If Not GeneralError Then _
      NumPorts = FindPortsOfType(BoardNum, ChannelType, _
      ProgAbility, PortNum, NumBits, FirstBit)
   
   If NumAOChans = 0 Then
      lblInstruction.Caption = "Board " & Format(BoardNum, "0") & _
         " has no analog output devices."
      cmdStartBgnd.Enabled = False
   ElseIf NumPorts = 0 Then
      lblInstruction.Caption = "Board " & Format(BoardNum, "0") & _
         " has no digital devices."
      cmdStartBgnd.Enabled = False
   Else
      cmdStartBgnd.Enabled = True
      MemHandle& = cbWinBufAlloc(NumElements&)      ' set aside memory to hold data
      If MemHandle& = 0 Then Stop
      ReDim DAData(NumElements - 1)
      ReDim ChanArray(ChanCount - 1)
      ReDim ChanTypeArray(ChanCount - 1)
      ReDim GainArray(ChanCount - 1)
      
      ' output (low)
      DAData(0) = 0
      DAData(1) = 0
      
      ' output (high))
      DAData(2) = -1   'signed value equivalent to 65535 (unsigned)
      DAData(3) = -1
      
      FirstPoint& = 0
     
      ULStat = cbWinArrayToBuf(DAData(0), MemHandle&, FirstPoint&, NumElements)
      
      'load the arrays with values
      ChanArray(0) = 0
      ChanTypeArray(0) = ANALOG
      GainArray(0) = CBRange
      
      lblChan0.Caption = "0"
      lblChan0Type = "ANALOG"
      lblChan0Gain = GetRangeString(CBRange)
   
      ChanArray(1) = FIRSTPORTA
      ChanTypeArray(1) = DIGITAL16
      GainArray(1) = NOTUSED
      
      lblChan1.Caption = "FIRSTPORTA"
      lblChan1Type = "DIGITAL16"
      lblChan1Gain = "NotUsed"
      lblInstruction.Caption = "Board " & Format(BoardNum, "0") & _
         " generating data on analog and digital" & _
         " channels using DaqOutScan with Range set to " _
         & GetRangeString(CBRange) & "."
      lblInstruction.ForeColor = &HFF0000
   End If
   
End Sub

Private Sub cmdStartBgnd_Click()

   Dim PortNum As Long, Direction As Long
   Dim CBRate As Long, Options As Long
   Dim Status As Integer, CurCount As Long, CurIndex As Long
   
   cmdStartBgnd.Enabled = False
   cmdStartBgnd.Visible = False
   cmdStopConvert.Enabled = True
   cmdStopConvert.Visible = True
   cmdQuit.Enabled = False
   UserTerm = False
   ' initialize user terminate flag
   
   'FIRSTPORTA is treated as a 16-bit port; therefore
   'FIRSTPORTA and FIRSTPORTB must be configured for digital output
   PortNum& = FIRSTPORTA
   Direction& = DIGITALOUT
   ULStat = cbDConfigPort(BoardNum, PortNum, Direction)
   
   PortNum& = FIRSTPORTB
   Direction& = DIGITALOUT
   ULStat = cbDConfigPort(BoardNum, PortNum, Direction)
   
   ' Generate the values with cbDaqOutScan()
   '  Parameters:
   '    BoardNum        :the number used by CB.CFG to describe this board
   '    ChanArray[]     :array of channel values
   '    ChanTypeArray[] : array of channel types
   '    GainArray[]     :array of gain values
   '    ChansCount&     :the number of elements in the arrays
   '    NumElements     :the total number of samples to generate
   '    CBRate&         :sample rate in samples per second
   '    MemHandle       :Handle for Windows buffer from which data will be generated
   '    Options         :data generation options

   Options = BACKGROUND + CONTINUOUS + NONSTREAMEDIO
   CBRate& = 1000                    ' Acquire data at 1000 Hz

   ULStat = cbDaqOutScan(BoardNum, ChanArray(0), ChanTypeArray(0), GainArray(0), _
      ChanCount&, CBRate&, NumElements, MemHandle&, Options)
   If ULStat <> 0 Then Stop

   ULStat = cbGetStatus(BoardNum, Status, CurCount&, CurIndex&, DAQOFUNCTION)
   If ULStat <> 0 Then Stop

   If Status = RUNNING Then
      lblShowStat.Caption = "Running"
      lblShowCount.Caption = Format$(CurCount&, "0")
      lblShowIndex.Caption = Format$(CurIndex&, "0")
   End If

   tmrCheckStatus.Enabled = True

End Sub

Private Sub tmrCheckStatus_Timer()

   Dim Status As Integer, CurCount As Long, CurIndex As Long
   
   ' This timer will check the status of the background data collection
   
   ' Parameters:
   '   BoardNum    :the number used by CB.CFG to describe this board
   '   Status     :current status of the background data generation
   '   CurCount&   :current number of samples transfered
   '   CurIndex&   :index to the last data value transferred
   '   FunctionType: A/D operation (DAQOFUNCTION)

   ULStat = cbGetStatus(BoardNum, Status, CurCount&, CurIndex&, DAQOFUNCTION)
   If ULStat <> 0 Then Stop


   lblShowCount.Caption = Format$(CurCount&, "0")
   lblShowIndex.Caption = Format$(CurIndex&, "0")

   ' Check if the background operation has finished. If it has, then
   ' the BACKGROUND operation must be explicitly stopped

   If Status = RUNNING And Not UserTerm Then
      lblShowStat.Caption = "Running"
      ULStat = cbGetStatus(BoardNum, Status, CurCount&, CurIndex&, DAQOFUNCTION)
      If ULStat <> 0 Then Stop
      lblShowCount.Caption = Format$(CurCount&, "0")
      lblShowIndex.Caption = Format$(CurIndex&, "0")
   ElseIf Status = IDLE Or UserTerm Then
      lblShowStat.Caption = "Idle"
      tmrCheckStatus.Enabled = 0
      
      ULStat = cbStopBackground(BoardNum, DAQOFUNCTION)
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
   
   ULStat = cbWinBufFree(MemHandle&)      ' Free up memory for use by
                                            ' other programs
   If ULStat <> 0 Then Stop
   End
   
End Sub

