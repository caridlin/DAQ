VERSION 5.00
Begin VB.Form frmDigIn 
   Appearance      =   0  'Flat
   BackColor       =   &H80000005&
   Caption         =   "Universal Library Digital Bit Input"
   ClientHeight    =   3480
   ClientLeft      =   105
   ClientTop       =   1545
   ClientWidth     =   6690
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
   ScaleHeight     =   3480
   ScaleWidth      =   6690
   Begin VB.CommandButton cmdStopRead 
      Appearance      =   0  'Flat
      Caption         =   "Quit"
      Default         =   -1  'True
      Height          =   375
      Left            =   5640
      TabIndex        =   20
      Top             =   2880
      Width           =   855
   End
   Begin VB.Timer tmrReadInputs 
      Enabled         =   0   'False
      Interval        =   200
      Left            =   5040
      Top             =   2880
   End
   Begin VB.Label lblShowBitVal 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   0
      Left            =   3120
      TabIndex        =   1
      Top             =   2160
      Width           =   255
   End
   Begin VB.Label lblShowBitVal 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   1
      Left            =   3480
      TabIndex        =   2
      Top             =   2160
      Width           =   255
   End
   Begin VB.Label lblShowBitVal 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   2
      Left            =   3840
      TabIndex        =   3
      Top             =   2160
      Width           =   255
   End
   Begin VB.Label lblShowBitVal 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   3
      Left            =   4200
      TabIndex        =   4
      Top             =   2160
      Width           =   255
   End
   Begin VB.Label lblShowBitVal 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   4
      Left            =   4920
      TabIndex        =   5
      Top             =   2160
      Width           =   255
   End
   Begin VB.Label lblShowBitVal 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   5
      Left            =   5280
      TabIndex        =   6
      Top             =   2160
      Width           =   255
   End
   Begin VB.Label lblShowBitVal 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   6
      Left            =   5640
      TabIndex        =   7
      Top             =   2160
      Width           =   255
   End
   Begin VB.Label lblShowBitVal 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   7
      Left            =   6000
      TabIndex        =   0
      Top             =   2160
      Width           =   255
   End
   Begin VB.Label lblBitVal 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Bit Value:"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   120
      TabIndex        =   8
      Top             =   2160
      Width           =   2775
   End
   Begin VB.Label lblShowBitNum 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "7"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   -1  'True
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H80000008&
      Height          =   255
      Index           =   7
      Left            =   6000
      TabIndex        =   17
      Top             =   1800
      Width           =   255
   End
   Begin VB.Label lblShowBitNum 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "6"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   -1  'True
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H80000008&
      Height          =   255
      Index           =   6
      Left            =   5640
      TabIndex        =   16
      Top             =   1800
      Width           =   255
   End
   Begin VB.Label lblShowBitNum 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "5"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   -1  'True
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H80000008&
      Height          =   255
      Index           =   5
      Left            =   5280
      TabIndex        =   15
      Top             =   1800
      Width           =   255
   End
   Begin VB.Label lblShowBitNum 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "4"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   -1  'True
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H80000008&
      Height          =   255
      Index           =   4
      Left            =   4920
      TabIndex        =   14
      Top             =   1800
      Width           =   255
   End
   Begin VB.Label lblShowBitNum 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "3"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   -1  'True
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H80000008&
      Height          =   255
      Index           =   3
      Left            =   4200
      TabIndex        =   13
      Top             =   1800
      Width           =   255
   End
   Begin VB.Label lblShowBitNum 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "2"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   -1  'True
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H80000008&
      Height          =   255
      Index           =   2
      Left            =   3840
      TabIndex        =   12
      Top             =   1800
      Width           =   255
   End
   Begin VB.Label lblShowBitNum 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "1"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   -1  'True
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H80000008&
      Height          =   255
      Index           =   1
      Left            =   3480
      TabIndex        =   11
      Top             =   1800
      Width           =   255
   End
   Begin VB.Label lblShowBitNum 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "0"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   -1  'True
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H80000008&
      Height          =   255
      Index           =   0
      Left            =   3120
      TabIndex        =   10
      Top             =   1800
      Width           =   255
   End
   Begin VB.Label lblBitNum 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Bit Number:"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   120
      TabIndex        =   9
      Top             =   1800
      Width           =   2775
   End
   Begin VB.Label lblInstruct 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Input a TTL logic level at the first port inputs to change Bit Value:"
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
      Left            =   840
      TabIndex        =   19
      Top             =   600
      Width           =   4935
   End
   Begin VB.Label lblDemoFunction 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Demonstration of cbDBitIn()"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   240
      TabIndex        =   18
      Top             =   120
      Width           =   6135
   End
End
Attribute VB_Name = "frmDigIn"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
'ULDI02.VBP================================================================

' File:                         ULDI02.VBP

' Library Call Demonstrated:    cbDBitIn&()

' Purpose:                      Reads the status of single digital input bit.

' Demonstration:                Configures the first compatible port
'                               for input (if necessary) and then
'                               reads and displays the bit values.

' Other Library Calls:          cbDConfigPort&()
'                               cbErrHandling&()

' Special Requirements:         Board 0 must have a digital input port
'                               or have digital ports programmable as input.

'==========================================================================
Option Explicit

Const BoardNum = 0              ' Board number

Dim PortNum As Long
Dim NumPorts As Long, NumBits As Long, FirstBit As Long
Dim PortType As Long, ProgAbility As Long
Dim Direction As Long
Dim ULStat As Long

Private Sub Form_Load()

   Dim ReportError As Long, HandleError As Long
   Dim I As Integer, PortName As String
   
   ' declare revision level of Universal Library
   ULStat& = cbDeclareRevision(CURRENTREVNUM)
   
   ' Initiate error handling
   '  activating error handling will trap errors like
   '  bad channel numbers and non-configured conditions.
   '  Parameters:
   '    PRINTALL    :all warnings and errors encountered will be printed
   '    DONTSTOP    :if an error is encountered, the program will not stop,
   '                 errors must be handled locally

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
   PortType = PORTIN
   NumPorts = FindPortsOfType(BoardNum, PortType, ProgAbility, PortNum, NumBits, FirstBit)
   If NumBits > 8 Then NumBits = 8
   For I% = NumBits To 7
       lblShowBitVal(I%).Visible = False
       lblShowBitNum(I%).Visible = False
   Next I%
   
   If NumPorts < 1 Then
       lblInstruct.Caption = "Board " & Format(BoardNum, "0") & _
         " has no compatible digital ports."
   Else
       ' if programmable, set direction of port to input
       ' configure the first port for digital input
       '  Parameters:
       '    PortNum        :the input port
       '    Direction      :sets the port for input or output
   
       If ProgAbility = DigitalIO.PROGPORT Then
           Direction = DIGITALIN
           ULStat = cbDConfigPort(BoardNum, PortNum, Direction)
           If Not (ULStat = 0) Then Stop
       End If
       PortName = GetPortString(PortNum)
       lblInstruct.Caption = "You may change the value read by applying " & _
       "a TTL high or TTL low to digital inputs on " & PortName & _
       " on board " & Format(BoardNum, "0") & "."
       lblBitNum.Caption = "The first " & Format(NumBits, "0") & " bits are:"
       tmrReadInputs.Enabled = True
   End If

End Sub

Private Sub tmrReadInputs_Timer()

   Dim BitPort As Long, I As Long
   Dim FirstBit As Long, BitNum As Long, BitValue As Integer
   Dim BitPortName As String
   
   ' read the input bits from the ports and display
   
   '  Parameters:
   '    BoardNum    :the number used by CB.CFG to describe this board
   '    PortType    :must be FIRSTPORTA or AUXPORT
   '    BitNum&     :the number of the bit to read from the port
   '    BitValue&   :the value read from the port
   
   ' For boards whose first port is not FIRSTPORTA (such as the USB-ERB08
   ' and the USB-SSR08) offset the BitNum by FirstBit
   
   BitPort& = AUXPORT
   If PortNum& > AUXPORT Then BitPort& = FIRSTPORTA

   For I& = 0 To NumBits - 1
      BitNum& = I&
   
      ULStat& = cbDBitIn(BoardNum, BitPort&, FirstBit& + BitNum&, BitValue%)
      If ULStat& <> 0 Then Stop
      lblShowBitVal(I&).Caption = Format$(BitValue%, "0")
   Next I&
   BitPortName$ = GetPortString(BitPort&)
   lblBitVal.Caption = BitPortName$ & ", bit " & Format(FirstBit&, "0") & _
   " - " & Format(FirstBit& + (NumBits - 1), "0") & " values:"

End Sub

Private Sub cmdStopRead_Click()

   tmrReadInputs.Enabled = False
   End

End Sub


