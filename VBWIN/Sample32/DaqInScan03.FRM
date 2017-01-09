VERSION 5.00
Begin VB.Form frmDataDisplay 
   Appearance      =   0  'Flat
   BackColor       =   &H80000005&
   Caption         =   "Universal Library Input Scan"
   ClientHeight    =   5910
   ClientLeft      =   105
   ClientTop       =   1545
   ClientWidth     =   7020
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
   ScaleHeight     =   5910
   ScaleWidth      =   7020
   Begin VB.CommandButton cmdStopConvert 
      Appearance      =   0  'Flat
      Caption         =   "Quit"
      Height          =   375
      Left            =   3960
      TabIndex        =   13
      Top             =   5340
      Width           =   855
   End
   Begin VB.CommandButton cmdStart 
      Appearance      =   0  'Flat
      Caption         =   "Read"
      Enabled         =   0   'False
      Height          =   375
      Left            =   3000
      TabIndex        =   46
      Top             =   5340
      Width           =   855
   End
   Begin VB.Label lblInstruction 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H000000FF&
      Height          =   675
      Left            =   120
      TabIndex        =   58
      Top             =   480
      Width           =   6795
   End
   Begin VB.Label Label1 
      Alignment       =   1  'Right Justify
      BackColor       =   &H80000005&
      Caption         =   "Channel:"
      Height          =   195
      Left            =   120
      TabIndex        =   57
      Top             =   1320
      Width           =   855
   End
   Begin VB.Label lblShowChanType 
      Alignment       =   2  'Center
      BackColor       =   &H80000005&
      Height          =   195
      Index           =   3
      Left            =   5400
      TabIndex        =   56
      Top             =   1620
      Width           =   1335
   End
   Begin VB.Label lblShowChanType 
      Alignment       =   2  'Center
      BackColor       =   &H80000005&
      Height          =   195
      Index           =   2
      Left            =   3960
      TabIndex        =   55
      Top             =   1620
      Width           =   1335
   End
   Begin VB.Label lblShowChanType 
      Alignment       =   2  'Center
      BackColor       =   &H80000005&
      Height          =   195
      Index           =   1
      Left            =   2520
      TabIndex        =   54
      Top             =   1620
      Width           =   1335
   End
   Begin VB.Label lblShowChanType 
      Alignment       =   2  'Center
      BackColor       =   &H80000005&
      Height          =   195
      Index           =   0
      Left            =   1080
      TabIndex        =   53
      Top             =   1620
      Width           =   1335
   End
   Begin VB.Label lblChanType 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Type:"
      ForeColor       =   &H80000008&
      Height          =   195
      Left            =   120
      TabIndex        =   52
      Top             =   1620
      Width           =   855
   End
   Begin VB.Label lblADData 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   195
      Index           =   39
      Left            =   5400
      TabIndex        =   45
      Top             =   4980
      Width           =   1335
   End
   Begin VB.Label lblADData 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   195
      Index           =   38
      Left            =   3960
      TabIndex        =   44
      Top             =   4980
      Width           =   1335
   End
   Begin VB.Label lblADData 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   195
      Index           =   37
      Left            =   2520
      TabIndex        =   43
      Top             =   4980
      Width           =   1335
   End
   Begin VB.Label lblADData 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   195
      Index           =   36
      Left            =   1080
      TabIndex        =   42
      Top             =   4980
      Width           =   1335
   End
   Begin VB.Label lblADData 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   195
      Index           =   35
      Left            =   5400
      TabIndex        =   41
      Top             =   4680
      Width           =   1335
   End
   Begin VB.Label lblADData 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   195
      Index           =   34
      Left            =   3960
      TabIndex        =   40
      Top             =   4680
      Width           =   1335
   End
   Begin VB.Label lblADData 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   195
      Index           =   33
      Left            =   2520
      TabIndex        =   39
      Top             =   4680
      Width           =   1335
   End
   Begin VB.Label lblADData 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   195
      Index           =   32
      Left            =   1080
      TabIndex        =   38
      Top             =   4680
      Width           =   1335
   End
   Begin VB.Label lblADData 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   195
      Index           =   31
      Left            =   5400
      TabIndex        =   37
      Top             =   4380
      Width           =   1335
   End
   Begin VB.Label lblADData 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   195
      Index           =   30
      Left            =   3960
      TabIndex        =   36
      Top             =   4380
      Width           =   1335
   End
   Begin VB.Label lblADData 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   195
      Index           =   29
      Left            =   2520
      TabIndex        =   35
      Top             =   4380
      Width           =   1335
   End
   Begin VB.Label lblADData 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   195
      Index           =   28
      Left            =   1080
      TabIndex        =   34
      Top             =   4380
      Width           =   1335
   End
   Begin VB.Label lblADData 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   195
      Index           =   27
      Left            =   5400
      TabIndex        =   33
      Top             =   4080
      Width           =   1335
   End
   Begin VB.Label lblADData 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   195
      Index           =   26
      Left            =   3960
      TabIndex        =   32
      Top             =   4080
      Width           =   1335
   End
   Begin VB.Label lblADData 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   195
      Index           =   25
      Left            =   2520
      TabIndex        =   31
      Top             =   4080
      Width           =   1335
   End
   Begin VB.Label lblADData 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   195
      Index           =   24
      Left            =   1080
      TabIndex        =   30
      Top             =   4080
      Width           =   1335
   End
   Begin VB.Label lblADData 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   195
      Index           =   23
      Left            =   5400
      TabIndex        =   29
      Top             =   3780
      Width           =   1335
   End
   Begin VB.Label lblADData 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   195
      Index           =   22
      Left            =   3960
      TabIndex        =   28
      Top             =   3780
      Width           =   1335
   End
   Begin VB.Label lblADData 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   195
      Index           =   21
      Left            =   2520
      TabIndex        =   27
      Top             =   3780
      Width           =   1335
   End
   Begin VB.Label lblADData 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   195
      Index           =   20
      Left            =   1080
      TabIndex        =   26
      Top             =   3780
      Width           =   1335
   End
   Begin VB.Label lblADData 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   195
      Index           =   11
      Left            =   5400
      TabIndex        =   17
      Top             =   2880
      Width           =   1335
   End
   Begin VB.Label lblADData 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   195
      Index           =   10
      Left            =   3960
      TabIndex        =   16
      Top             =   2880
      Width           =   1335
   End
   Begin VB.Label lblADData 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   195
      Index           =   9
      Left            =   2520
      TabIndex        =   15
      Top             =   2880
      Width           =   1335
   End
   Begin VB.Label lblADData 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   195
      Index           =   8
      Left            =   1080
      TabIndex        =   14
      Top             =   2880
      Width           =   1335
   End
   Begin VB.Label lblADData 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   195
      Index           =   19
      Left            =   5400
      TabIndex        =   25
      Top             =   3480
      Width           =   1335
   End
   Begin VB.Label lblADData 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   195
      Index           =   18
      Left            =   3960
      TabIndex        =   24
      Top             =   3480
      Width           =   1335
   End
   Begin VB.Label lblADData 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   195
      Index           =   17
      Left            =   2520
      TabIndex        =   23
      Top             =   3480
      Width           =   1335
   End
   Begin VB.Label lblADData 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   195
      Index           =   16
      Left            =   1080
      TabIndex        =   22
      Top             =   3480
      Width           =   1335
   End
   Begin VB.Label lblADData 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   195
      Index           =   15
      Left            =   5400
      TabIndex        =   21
      Top             =   3180
      Width           =   1335
   End
   Begin VB.Label lblADData 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   195
      Index           =   14
      Left            =   3960
      TabIndex        =   20
      Top             =   3180
      Width           =   1335
   End
   Begin VB.Label lblADData 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   195
      Index           =   13
      Left            =   2520
      TabIndex        =   19
      Top             =   3180
      Width           =   1335
   End
   Begin VB.Label lblADData 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   195
      Index           =   12
      Left            =   1080
      TabIndex        =   18
      Top             =   3180
      Width           =   1335
   End
   Begin VB.Label lblADData 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   195
      Index           =   7
      Left            =   5400
      TabIndex        =   12
      Top             =   2580
      Width           =   1335
   End
   Begin VB.Label lblADData 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   195
      Index           =   6
      Left            =   3960
      TabIndex        =   11
      Top             =   2580
      Width           =   1335
   End
   Begin VB.Label lblADData 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   195
      Index           =   5
      Left            =   2520
      TabIndex        =   10
      Top             =   2580
      Width           =   1335
   End
   Begin VB.Label lblADData 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   195
      Index           =   4
      Left            =   1080
      TabIndex        =   9
      Top             =   2580
      Width           =   1335
   End
   Begin VB.Label lblADData 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   195
      Index           =   3
      Left            =   5400
      TabIndex        =   8
      Top             =   2280
      Width           =   1335
   End
   Begin VB.Label lblADData 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   195
      Index           =   2
      Left            =   3960
      TabIndex        =   7
      Top             =   2280
      Width           =   1335
   End
   Begin VB.Label lblADData 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   195
      Index           =   1
      Left            =   2520
      TabIndex        =   6
      Top             =   2280
      Width           =   1335
   End
   Begin VB.Label lblADData 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   195
      Index           =   0
      Left            =   1080
      TabIndex        =   5
      Top             =   2280
      Width           =   1335
   End
   Begin VB.Label lblShowRange 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   -1  'True
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H00FF0000&
      Height          =   195
      Index           =   3
      Left            =   5400
      TabIndex        =   51
      Top             =   1920
      Width           =   1335
   End
   Begin VB.Label lblShowRange 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   -1  'True
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H00FF0000&
      Height          =   195
      Index           =   2
      Left            =   3960
      TabIndex        =   50
      Top             =   1920
      Width           =   1335
   End
   Begin VB.Label lblShowRange 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   -1  'True
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H00FF0000&
      Height          =   195
      Index           =   1
      Left            =   2520
      TabIndex        =   49
      Top             =   1920
      Width           =   1335
   End
   Begin VB.Label lblShowRange 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   -1  'True
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H00FF0000&
      Height          =   195
      Index           =   0
      Left            =   1080
      TabIndex        =   48
      Top             =   1920
      Width           =   1335
   End
   Begin VB.Label lblRange 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Range:"
      ForeColor       =   &H80000008&
      Height          =   195
      Left            =   120
      TabIndex        =   47
      Top             =   1920
      Width           =   855
   End
   Begin VB.Label lblChan3 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H80000008&
      Height          =   195
      Left            =   5400
      TabIndex        =   4
      Top             =   1320
      Width           =   1335
   End
   Begin VB.Label lblChan2 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H80000008&
      Height          =   195
      Left            =   3960
      TabIndex        =   3
      Top             =   1320
      Width           =   1335
   End
   Begin VB.Label lblChan1 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H80000008&
      Height          =   195
      Left            =   2520
      TabIndex        =   2
      Top             =   1320
      Width           =   1335
   End
   Begin VB.Label lblChan0 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H80000008&
      Height          =   195
      Left            =   1080
      TabIndex        =   1
      Top             =   1320
      Width           =   1335
   End
   Begin VB.Label lblDemoFunction 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Demonstration of cbDaqInScan() and cbGetTCValue()"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   120
      TabIndex        =   0
      Top             =   120
      Width           =   6825
   End
End
Attribute VB_Name = "frmDataDisplay"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
'==============================================================================

' File:                         DaqInScan03.VBP

' Library Call Demonstrated:    cbDaqInScan(), FOREGROUND mode and cbGetTCValues()
'
' Purpose:                      Synchronously scans Analog channels, Digital ports,
'                               and Thermocouple channels in the foreground.
'
' Demonstration:                Collects data on Analog Channel 5, FirstPortA,
'                               CJCs 0, 1  and Thermocouple channels 0, 1.
'
' Other Library Calls:          cbErrHandling()
'                               cbDConfigPort()
'                               cbCConfigScan()
'
' Special Requirements:         Board 0 must support cbDaqInScan.
'                               Thermocouples must be wired to
'                               temperature channels selected.

'==========================================================================
Option Explicit

Const BoardNum As Long = 0      ' Board number
Const ChanCount& = 6            ' Number of channels in scan
Const TcChanCount& = 2          ' Number of thermocouple channels
Const NumPoints& = 10           ' Number of data points to collect
Const NumElements& = ChanCount& * NumPoints&
Const NumTCElements& = TcChanCount& * NumPoints&

Dim ADData() As Integer          ' dimension an array to hold the input values
Dim MemHandle As Long            ' define a variable to contain the handle for
                                 ' memory allocated by Windows through cbWinBufAlloc()
Dim ChanArray() As Integer       ' array to hold channel queue information
Dim ChanTypeArray() As Integer   ' array to hold channel type information
Dim GainArray() As Integer       ' array to hold gain queue information
Dim ULStat As Long
Dim CBScale&, FirstPoint&
Dim TempValArray() As Single
Dim GeneralError As Boolean

Private Sub Form_Load()
    
   Dim DefaultTrig As Long, LowChan As Long
   Dim ChannelType As Long, CBRange As Long
   Dim CounterNum As Long, ADResolution As Long
   Dim ProgAbility As Long, PortNum As Long
   Dim NumBits As Long, FirstBit As Long, NumPorts As Long
   Dim NumCntrs As Long, NumAOChans As Long, NumAIChans As Long
   Dim ReportError As Long, HandleError As Long
   
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
   SetDigitalIODefaults ReportError, HandleError
   
   ' If cbErrHandling is set for STOPALL or STOPFATAL during the program
   ' design stage, Visual Basic will be unloaded when an error is encountered.
   ' We suggest trapping errors locally until the program is ready for compiling
   ' to avoid losing unsaved data during program design.  This can be done by
   ' setting cbErrHandling options as above and checking the value of ULStat
   ' after a call to the library. If it is not equal to 0, an error has occurred.
    
   ChannelType = CTRSCAN
   NumCntrs = FindCountersOfType(BoardNum, ChannelType, CounterNum)
   GeneralError = CtrGeneralError
   ChannelType = ANALOGINPUT
   If Not GeneralError Then _
      NumAIChans = FindAnalogChansOfType(BoardNum, ChannelType, _
      ADResolution, CBRange, LowChan, DefaultTrig)
   GeneralError = GeneralError Or AIOGeneralError
   ChannelType = PORTIN
   If Not GeneralError Then _
      NumPorts = FindPortsOfType(BoardNum, ChannelType, _
      ProgAbility, PortNum, NumBits, FirstBit)
   
   If NumCntrs = 0 Then
      lblInstruction.Caption = "Board " & Format(BoardNum, "0") & _
         " has no counter devices."
   ElseIf NumAIChans = 0 Then
      lblInstruction.Caption = "Board " & Format(BoardNum, "0") & _
         " has no analog input devices."
   ElseIf NumPorts = 0 Then
      lblInstruction.Caption = "Board " & Format(BoardNum, "0") & _
         " has no digital devices."
   Else
      cmdStart.Enabled = True
      MemHandle& = cbWinBufAlloc(NumElements&)      ' set aside memory to hold data
      If MemHandle& = 0 Then Stop
      ReDim ADData(NumElements& - 1)
      ReDim ChanArray(ChanCount - 1)
      ReDim ChanTypeArray(ChanCount - 1)
      ReDim GainArray(ChanCount - 1)
      ReDim TempValArray(NumTCElements - 1)
      'Get a list of valid ranges from the AnalogIO module
      Dim ValidRanges As Variant
      Dim RandomRange As Long, NumRanges As Long
      ValidRanges = GetRangeList()
      NumRanges = UBound(ValidRanges)
      
      'load the arrays with values
      ChanArray(0) = 4
      ChanTypeArray(0) = ANALOG
      GainArray(0) = CBRange
      
      lblChan0.Caption = "4"
      lblShowChanType(0).Caption = "ANALOG"
      lblShowRange(0).Caption = GetRangeString(CBRange)
   
      ChanArray(1) = FIRSTPORTA
      ChanTypeArray(1) = DIGITAL8
      GainArray(1) = NOTUSED
      
      lblChan1.Caption = "FIRSTPORTA"
      lblShowChanType(1).Caption = "DIGITAL8"
      lblShowRange(1).Caption = "NotUsed"
   
      'For accurate thermocouple readings, the CJC channels
      'and TC channels must be associated properly.
      'The TC channels must immediately follow their associated
      'CJCs in the channel array. Other channel types may be
      'placed in the channel array as long as they do not fall
      'between a CJC channel and its associated thermocouple channel.
   
      ChanArray(2) = 0
      ChanTypeArray(2) = CJC
      GainArray(2) = NOTUSED
      
      ChanArray(3) = 0
      ChanTypeArray(3) = TC
      GainArray(3) = NOTUSED
      
      lblChan2.Caption = "0"
      lblShowChanType(2).Caption = "TC"
      lblShowRange(2).Caption = "NotUsed"
      
      ChanArray(4) = 1
      ChanTypeArray(4) = CJC
      GainArray(4) = NOTUSED
      
      ChanArray(5) = 1
      ChanTypeArray(5) = TC
      GainArray(5) = NOTUSED
      
      lblChan3.Caption = "1"
      lblShowChanType(3).Caption = "TC"
      lblShowRange(3).Caption = "NotUsed"
   
      lblInstruction.Caption = "Board " & Format(BoardNum, "0") & _
         " collecting analog data on analog, digital, and temperature" & _
         " channels using DaqInScan with Range set to " & GetRangeString(CBRange) & "."
      lblInstruction.ForeColor = &HFF0000
   End If
   
End Sub

Private Sub cmdStart_Click()

   Dim CBRate As Long, PretrigCount As Long
   Dim CBCount As Long, Options As Long
   Dim PortNum As Long, Direction As Long
   Dim i As Integer, SampleNum As Long
   
   'configure FIRSTPORTA  for digital input
   PortNum& = ChanArray(1)
   Direction& = DIGITALIN
   ULStat = cbDConfigPort(BoardNum, PortNum, Direction)
   If ULStat <> 0 Then Stop
   
   ' Collect the values with cbDaqInScan()
   '  Parameters:
   '    BoardNum        :the number used by CB.CFG to describe this board
   '    ChanArray[]     :array of channel values
   '    ChanTypeArray[] : array of channel types
   '    GainArray[]     :array of gain values
   '    ChansCount&      :the number of elements in the arrays (0=disable queue)
   '    PretrigCount&    :number of pre-trigger A/D samples to collect
   '    CBCount&         :the total number of A/D samples to collect
   '    CBRate&          :sample rate in samples per second
   '    MemHandle&       :the array for the collected data values
   '    Options          :data collection options

   PretrigCount& = 0
   CBCount& = NumElements&           ' Number of data points to collect
   Options = CONVERTDATA
   CBRate& = 100                    ' Acquire data at 100 Hz

   If MemHandle& = 0 Then Stop       ' check that a handle to a memory buffer exists

   ULStat = cbDaqInScan(BoardNum, ChanArray(0), _
      ChanTypeArray(0), GainArray(0), ChanCount&, _
      CBRate&, PretrigCount&, CBCount&, MemHandle&, Options)
   If ULStat = BADCHANTYPE Then
      lblInstruction.Caption = "Board " & Format(BoardNum, "0") & _
         " reports bad channel type. Are TC channels configured?"
      lblInstruction.ForeColor = &HFF
      Exit Sub
   ElseIf ULStat <> 0 Then
      Stop
   End If

   ' Transfer the data from the memory buffer set up by Windows to an array for use by Visual Basic
   FirstPoint& = 0
   ULStat = cbWinBufToArray(MemHandle&, ADData(0), FirstPoint&, CBCount&)
   If ULStat <> 0 Then Stop
   
   ' Convert CJC  and TC reading to temperature
   ' Parameters:
   '         BoardNum    :the number used by CB.CFG to describe this board
   '         ChanArray[] :array of channel values
   '         ChanTypeArray[] : array of channel types
   '         ChanCount    :the number of channels
   '         MemHandle&   :the collected raw data values
   '         FirstPoint   :the scan index that holds the first sample of the first channel to be converted.
   '         CBCount      :the number of samples per channel to be converted.
   '         CBScale      :temperature scale (Celsius, Fahrenheit, Kelvin)
   '         TempValArray[]  :the array for the temperature data values */

    
   CBScale = CELSIUS
   CBCount& = NumPoints&

   ULStat = cbGetTCValues(BoardNum, ChanArray(0), ChanTypeArray(0), ChanCount&, MemHandle&, FirstPoint, CBCount&, CBScale&, TempValArray!(0))
   If (ULStat <> 0) And (ULStat <> OUTOFRANGE) Then Stop

   i = 0
   For SampleNum = 0 To NumPoints& - 1
        lblADData(i).Caption = Format$(ADData(ChanCount& * SampleNum), "0")
        i = i + 1
        lblADData(i).Caption = Format$(ADData(ChanCount& * SampleNum + 1), "0")
        i = i + 1
        lblADData(i).Caption = Format$(TempValArray!(TcChanCount& * SampleNum), "0.000\°\C")
        i = i + 1
        lblADData(i).Caption = Format$(TempValArray(TcChanCount& * SampleNum + 1), "0.000\°\C")
        i = i + 1
   Next SampleNum
End Sub

Private Sub cmdStopConvert_Click()
   
   ULStat = cbWinBufFree(MemHandle&)      ' Free up memory for use by
   If ULStat <> 0 Then Stop                ' other programs
   End
   
End Sub


