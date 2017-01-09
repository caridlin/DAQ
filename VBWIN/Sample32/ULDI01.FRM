VERSION 5.00
Begin VB.Form frmDigIn 
   Appearance      =   0  'Flat
   BackColor       =   &H80000005&
   Caption         =   "Universal Library Digital Input"
   ClientHeight    =   3630
   ClientLeft      =   105
   ClientTop       =   1545
   ClientWidth     =   6555
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
   LinkTopic       =   "Form2"
   PaletteMode     =   1  'UseZOrder
   ScaleHeight     =   3630
   ScaleWidth      =   6555
   Begin VB.CommandButton cmdStopRead 
      Appearance      =   0  'Flat
      Caption         =   "Quit"
      Default         =   -1  'True
      Height          =   375
      Left            =   5160
      TabIndex        =   14
      Top             =   3000
      Width           =   855
   End
   Begin VB.Timer tmrReadInputs 
      Enabled         =   0   'False
      Interval        =   500
      Left            =   4560
      Top             =   3000
   End
   Begin VB.Label lblPortRead 
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
      Left            =   3840
      TabIndex        =   13
      Top             =   1440
      Width           =   495
   End
   Begin VB.Label lblPortsRead 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Port value read:"
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
      Left            =   240
      TabIndex        =   12
      Top             =   1440
      Width           =   3390
   End
   Begin VB.Label lblBitVal 
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
      Left            =   3000
      TabIndex        =   3
      Top             =   2520
      Width           =   255
   End
   Begin VB.Label lblBitVal 
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
      Left            =   3360
      TabIndex        =   4
      Top             =   2520
      Width           =   255
   End
   Begin VB.Label lblBitVal 
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
      Left            =   3720
      TabIndex        =   5
      Top             =   2520
      Width           =   255
   End
   Begin VB.Label lblBitVal 
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
      Left            =   4080
      TabIndex        =   6
      Top             =   2520
      Width           =   255
   End
   Begin VB.Label lblBitVal 
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
      Left            =   4440
      TabIndex        =   7
      Top             =   2520
      Width           =   255
   End
   Begin VB.Label lblBitVal 
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
      Left            =   4800
      TabIndex        =   8
      Top             =   2520
      Width           =   255
   End
   Begin VB.Label lblBitVal 
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
      Left            =   5160
      TabIndex        =   9
      Top             =   2520
      Width           =   255
   End
   Begin VB.Label lblBitVal 
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
      Left            =   5520
      TabIndex        =   10
      Top             =   2520
      Width           =   255
   End
   Begin VB.Label lblBitsRead 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Bit values read:"
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
      Left            =   240
      TabIndex        =   11
      Top             =   2520
      Width           =   2655
   End
   Begin VB.Label lblBitNum 
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
      Left            =   5520
      TabIndex        =   22
      Top             =   2160
      Width           =   255
   End
   Begin VB.Label lblBitNum 
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
      Left            =   5160
      TabIndex        =   21
      Top             =   2160
      Width           =   255
   End
   Begin VB.Label lblBitNum 
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
      Left            =   4800
      TabIndex        =   20
      Top             =   2160
      Width           =   255
   End
   Begin VB.Label lblBitNum 
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
      Left            =   4440
      TabIndex        =   19
      Top             =   2160
      Width           =   255
   End
   Begin VB.Label lblBitNum 
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
      Left            =   4080
      TabIndex        =   18
      Top             =   2160
      Width           =   255
   End
   Begin VB.Label lblBitNum 
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
      Left            =   3720
      TabIndex        =   17
      Top             =   2160
      Width           =   255
   End
   Begin VB.Label lblBitNum 
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
      Left            =   3360
      TabIndex        =   16
      Top             =   2160
      Width           =   255
   End
   Begin VB.Label lblBitNum 
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
      Left            =   3000
      TabIndex        =   15
      Top             =   2160
      Width           =   255
   End
   Begin VB.Label lblBitList 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "The first 8 bits are:"
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
      Left            =   240
      TabIndex        =   2
      Top             =   2160
      Width           =   2655
   End
   Begin VB.Label lblInstruct 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "You may change a bit by applying a TTL high or TTL low to digital inputs on FIRSTPORTA."
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
      Height          =   615
      Left            =   720
      TabIndex        =   1
      Top             =   600
      Width           =   4935
   End
   Begin VB.Label lblDemoFunction 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Demonstration of cbDIn()"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   240
      TabIndex        =   0
      Top             =   120
      Width           =   6015
   End
End
Attribute VB_Name = "frmDigIn"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
'ULDI01.VBP================================================================

' File:                         ULDI01.VBP

' Library Call Demonstrated:    cbDIn&()

' Purpose:                      Reads a digital input port.

' Demonstration:                Configures the first compatible port
'                               for input (if necessary) and then
'                               reads and displays the value on the port.

' Other Library Calls:          cbDConfigPort&()
'                               cbErrHandling&()

' Special Requirements:         Board 0 must have a digital input port
'                               or have digital ports programmable as input.

' (c) Copyright 2005-2011, Measurement Computing Corp.
' All rights reserved.
'==========================================================================
Option Explicit

Const BoardNum As Long = 0      ' Board number
Const One% = 1
Const Zero% = 0

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
   PortType = PORTIN
   NumPorts = FindPortsOfType(BoardNum, PortType, _
      ProgAbility, PortNum, NumBits, FirstBit)
   If NumBits > 8 Then NumBits = 8
   For I% = NumBits To 7
       lblBitVal(I%).Visible = False
       lblBitNum(I%).Visible = False
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
       lblBitList.Caption = "The first " & Format(NumBits, "0") & " bits are:"
       lblPortsRead.Caption = "Value read at " & PortName
       tmrReadInputs.Enabled = True
   End If
   
End Sub

Private Sub tmrReadInputs_Timer()

   Dim DataValue As Integer
   Dim I As Integer
   
   ' read the digital input and display
     
   ' Parameters:
   '   BoardNum    :the number used by CB.CFG to describe this board
   '   PortNum&    :the input port
   '   DataValue&  :the value read from the port
    
   ULStat& = cbDIn(BoardNum, PortNum&, DataValue%)
   If ULStat& <> 0 Then Stop
      
   ' display the value collected from the port

   lblPortRead.Caption = Format$(DataValue%, "0")

   ' parse DataValue% into bit values to indicate on/off status
      
   For I% = 0 To 7
      If (DataValue% And (2 ^ I%)) Then
         lblBitVal(I%).Caption = Format$(One%, "0")
      Else
         lblBitVal(I%).Caption = Format$(Zero%, "0")
      End If
   Next I%

End Sub

Private Sub cmdStopRead_Click()

   tmrReadInputs.Enabled = False
   End

End Sub


