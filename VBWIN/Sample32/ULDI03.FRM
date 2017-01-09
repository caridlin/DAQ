VERSION 5.00
Begin VB.Form frmDScan 
   Appearance      =   0  'Flat
   BackColor       =   &H80000005&
   Caption         =   "Universal Library Digital Input"
   ClientHeight    =   5580
   ClientLeft      =   105
   ClientTop       =   1545
   ClientWidth     =   5415
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
   ScaleHeight     =   5580
   ScaleWidth      =   5415
   Begin VB.CommandButton cmdStopRead 
      Appearance      =   0  'Flat
      Caption         =   "Quit"
      Height          =   375
      Left            =   3840
      TabIndex        =   1
      Top             =   5040
      Width           =   1215
   End
   Begin VB.CommandButton cmdReadDIn 
      Appearance      =   0  'Flat
      Caption         =   "Read"
      Height          =   375
      Left            =   2160
      TabIndex        =   0
      Top             =   5040
      Width           =   1215
   End
   Begin VB.CommandButton cmdTemp 
      Appearance      =   0  'Flat
      Caption         =   "Stop"
      Height          =   375
      Left            =   360
      TabIndex        =   2
      Top             =   5040
      Width           =   1215
   End
   Begin VB.Timer tmrCheckStatus 
      Enabled         =   0   'False
      Interval        =   300
      Left            =   4800
      Top             =   4200
   End
   Begin VB.Label lblInstruct 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H000000FF&
      Height          =   735
      Left            =   360
      TabIndex        =   21
      Top             =   720
      Width           =   4695
   End
   Begin VB.Label lblDataRead 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   9
      Left            =   3240
      TabIndex        =   7
      Top             =   4440
      Width           =   855
   End
   Begin VB.Label lblDataRead 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   4
      Left            =   1320
      TabIndex        =   12
      Top             =   4440
      Width           =   855
   End
   Begin VB.Label lblDataRead 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   8
      Left            =   3240
      TabIndex        =   8
      Top             =   4080
      Width           =   855
   End
   Begin VB.Label lblDataRead 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   3
      Left            =   1320
      TabIndex        =   13
      Top             =   4080
      Width           =   855
   End
   Begin VB.Label lblDataRead 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   7
      Left            =   3240
      TabIndex        =   9
      Top             =   3720
      Width           =   855
   End
   Begin VB.Label lblDataRead 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   2
      Left            =   1320
      TabIndex        =   14
      Top             =   3720
      Width           =   855
   End
   Begin VB.Label lblDataRead 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   6
      Left            =   3240
      TabIndex        =   10
      Top             =   3360
      Width           =   855
   End
   Begin VB.Label lblDataRead 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   1
      Left            =   1320
      TabIndex        =   15
      Top             =   3360
      Width           =   855
   End
   Begin VB.Label lblDataRead 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   5
      Left            =   3240
      TabIndex        =   11
      Top             =   3000
      Width           =   855
   End
   Begin VB.Label lblDataRead 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   0
      Left            =   1320
      TabIndex        =   16
      Top             =   3000
      Width           =   855
   End
   Begin VB.Label lblBGStat 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Left            =   660
      TabIndex        =   3
      Top             =   2580
      Width           =   3915
   End
   Begin VB.Label lblShowIndex 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Left            =   3480
      TabIndex        =   4
      Top             =   2040
      Width           =   1215
   End
   Begin VB.Label lblShowCount 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Left            =   2040
      TabIndex        =   5
      Top             =   2040
      Width           =   1215
   End
   Begin VB.Label lblShowStat 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Left            =   600
      TabIndex        =   6
      Top             =   2040
      Width           =   1215
   End
   Begin VB.Label lblIndex 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Index"
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
      Height          =   255
      Left            =   3480
      TabIndex        =   20
      Top             =   1680
      Width           =   1215
   End
   Begin VB.Label lblCount 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Count"
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
      Height          =   255
      Left            =   2040
      TabIndex        =   19
      Top             =   1680
      Width           =   1215
   End
   Begin VB.Label lblStatus 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Status"
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
      Height          =   255
      Left            =   600
      TabIndex        =   18
      Top             =   1680
      Width           =   1215
   End
   Begin VB.Label lblFunction 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "cbDInScan() Example Program"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   360
      TabIndex        =   17
      Top             =   240
      Width           =   4695
   End
End
Attribute VB_Name = "frmDScan"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
'ULDI03.VBP================================================================

' File:                         ULDI03.VBP

' Library Call Demonstrated:    cbDInScan&()

' Purpose:                      Reads digital input port(s)
'                               at specified rate and number
'                               of samples.

' Demonstration:                Configures the first one or two digital
'                               scan ports for input (if programmable)
'                               and reads the value on the port.

' Other Library Calls:          cbDConfigPort&()
'                               cbErrHandling&()

' Special Requirements:         Board 0 must support paced Digital input.

'==========================================================================
Option Explicit

Const BoardNum As Long = 0              ' Board number

Const NumPoints& = 500
Const FirstPoint& = 0

Dim PortNum As Long
Dim NumPorts As Long, NumBits As Long, FirstBit As Long
Dim PortType As Long, ProgAbility As Long
Dim Direction As Long

Dim DataBuffer() As Integer
Dim MemHandle&, CBCount&, Force%
Dim ULStat As Long

Private Sub Form_Load()

   Dim ReportError As Long, HandleError As Long
   Dim NumberOfPort As Long, DigPort As Long, PortName As String
   Dim AndString As String
   
   ' declare revision level of Universal Library
   ULStat& = cbDeclareRevision(CURRENTREVNUM)
   
   ' Initiate error handling
   '  activating error handling will trap errors like
   '  bad channel numbers and non-configured conditions.
   '  Parameters:
   '    PRINTALL    :all warnings and errors encountered will not be printed
   '    DONTSTOP    :if an error is encountered, the program will not stop,
   '                  errors must be handled locally
    
   ReportError = DONTPRINT
   HandleError = DONTSTOP
   ULStat& = cbErrHandling(ReportError, HandleError)
   If ULStat& <> 0 Then Stop
   SetDigitalIODefaults ReportError, HandleError
    
   ' If cbErrHandling& is set for STOPALL or STOPFATAL during the program
   ' design stage, Visual Basic will be unloaded when an error is encountered.
   ' We suggest trapping errors locally until the program is ready for compiling
   ' to avoid losing unsaved data during program design.  This can be done by
   ' setting cbErrHandling options as above and checking the value of ULStat&
   ' after a call to the library. If it is not equal to 0, an error has occurred.
    
   'determine if digital port exists, its capabilities, etc
   PortType = PORTINSCAN
   NumPorts = FindPortsOfType(BoardNum, PortType, _
      ProgAbility, PortNum, NumBits, FirstBit)
   If NumPorts > 2 Then NumPorts = 2
   
   If NumPorts < 1 Then
      lblInstruct.Caption = "Board " & Format(BoardNum, "0") & _
         " has no compatible digital ports."
      cmdReadDIn.Enabled = False
      cmdTemp.Enabled = False
   Else
      'configure first one or two scan ports
      'for digital input (if programmable)
      '  Parameters:
      '     PortNum    :the input port
      '     Direction  :sets the port for input or output
   
      ReDim DataBuffer(NumPoints)
      MemHandle& = cbWinBufAlloc(NumPoints)
      If (MemHandle& = 0) Then Stop
      
      For NumberOfPort& = 0 To NumPorts - 1
         DigPort = PortNum + NumberOfPort
         PortName = PortName & AndString$ & GetPortString(DigPort)
         If ProgAbility = PROGPORT Then
            Direction = DIGITALIN
            ULStat = cbDConfigPort(BoardNum, DigPort, Direction)
            If ULStat& <> 0 Then
               ULStat& = cbWinBufFree(MemHandle&)
               Stop
            End If
         End If
         AndString = " and "
      Next
      lblInstruct.Caption = _
         "Scanning digital input port at " & PortName & _
         " on board " & Format(BoardNum, "0") & "."
      
   End If
   
   Force% = 0

End Sub

Private Sub cmdReadDIn_Click()

   Dim CBRate As Long, Options As Long
   
   'read the digital input and display
   '  Parameters:
   '     BoardNum      :the number used by CB.CFG to describe this board
   '     PortNum&      :the input port
   '     CBCount&      :number of times to read digital input
   '     CBRate&       :sample rate in samples/second
   '     DataBuffer%() :the array for the digital input values read from the port
   '     Options&      :data collection options

   CBCount& = NumPoints&
   CBRate& = 100
   
   Options& = WORDXFER + BACKGROUND

   ULStat& = cbDInScan(BoardNum&, PortNum&, CBCount&, CBRate&, MemHandle&, Options&)
   If ULStat& <> 0 Then
      ULStat& = cbWinBufFree(MemHandle&)
      Stop
   End If
   tmrCheckStatus.Enabled = True

End Sub

Private Sub tmrCheckStatus_Timer()
   
   Dim Status As Integer
   Dim CurCount As Long, CurIndex As Long
   
   ULStat& = cbGetStatus(BoardNum&, Status%, CurCount&, CurIndex&, DIFUNCTION)
   If ULStat& <> 0 Then Stop
   lblShowStat.Caption = Format$(Status%, "0")
   lblShowCount.Caption = Format$(CurCount&, "0")
   lblShowIndex.Caption = Format$(CurIndex&, "0")
   If Status% = RUNNING Then
      lblBGStat.Caption = "Background operation running"
   Else
      lblBGStat.Caption = "Background operation idle"
   End If
   If CurCount& = NumPoints& Or Status% = 0 Or Force% = 1 Then
      tmrCheckStatus.Enabled = False
      ULStat& = cbStopBackground(BoardNum&, DIFUNCTION)
      If ULStat& <> 0 Then
         ULStat& = cbWinBufFree(MemHandle&)
         Stop
      End If
      ShowData
   End If

End Sub

Private Sub ShowData()
   
   Dim I As Integer
   
   ULStat& = cbWinBufToArray(MemHandle&, DataBuffer%(0), FirstPoint&, CBCount&)
   If ULStat& <> 0 Then
      ULStat& = cbWinBufFree(MemHandle&)
      Stop
   End If

   For I% = 0 To 9
      lblDataRead(I%).Caption = Hex$(DataBuffer%(I%))
   Next I%

End Sub

Private Sub cmdTemp_Click()

   Force% = 1

End Sub

Private Sub cmdStopRead_Click()

   ULStat& = cbStopBackground(BoardNum&, DIFUNCTION)
   ULStat& = cbWinBufFree(MemHandle&)
   End

End Sub

