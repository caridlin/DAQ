VERSION 5.00
Begin VB.Form frmDigIn 
   Appearance      =   0  'Flat
   BackColor       =   &H80000005&
   Caption         =   "Universal Library Digital Bit Input"
   ClientHeight    =   3480
   ClientLeft      =   105
   ClientTop       =   1545
   ClientWidth     =   4890
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
   ScaleWidth      =   4890
   Begin VB.CommandButton cmdStopRead 
      Appearance      =   0  'Flat
      Caption         =   "Quit"
      Default         =   -1  'True
      Height          =   375
      Left            =   3720
      TabIndex        =   1
      Top             =   2880
      Width           =   855
   End
   Begin VB.Timer tmrReadInputs 
      Enabled         =   0   'False
      Interval        =   200
      Left            =   2880
      Top             =   2760
   End
   Begin VB.Label lblBitNum 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Bit Number"
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
      Left            =   495
      TabIndex        =   3
      Top             =   2040
      Width           =   3915
   End
   Begin VB.Label lblInstruct 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "You may change the bit state by applying a TTL high or a TTL low to the corresponding pin on the port"
      ForeColor       =   &H000000FF&
      Height          =   735
      Left            =   285
      TabIndex        =   2
      Top             =   840
      Width           =   4335
   End
   Begin VB.Label lblDemoFunction 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Demonstration of cbDConfigBit()"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   165
      TabIndex        =   0
      Top             =   240
      Width           =   4575
   End
End
Attribute VB_Name = "frmDigIn"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
'ULDI06.VBP================================================================

' File:                         ULDI06.VBP

' Library Call Demonstrated:    cbDConfigBit&()

' Purpose:                      Reads the status of a single bit within a
'                               digital port after configuring for input.

' Demonstration:                Configures a single bit (within a digital port)
'                               for input (if programmable) and reads the bit status.

' Other Library Calls:          cbDBitIn&()
'                               cbErrHandling&()

' Special Requirements:         Board 0 must have a digital port that supports
'                               input or bits that can be configured for input.

'==========================================================================
Option Explicit

Const BoardNum = 0              ' Board number

Dim PortNum As Long
Dim NumPorts As Long, NumBits As Long, FirstBit As Long
Dim PortType As Long, ProgAbility As Long
Dim Direction As Long, ULStat As Long

Private Sub Form_Load()

   Dim ReportError As Long, HandleError As Long
   Dim DevNum As Long, PortName As String
   Dim PortType As Long, BitName As String
   Dim InMask, OutMask As Long
   ' declare revision level of Universal Library

   ULStat& = cbDeclareRevision(CURRENTREVNUM)
   
   ' Initiate error handling
   '  activating error handling will trap errors like
   '  bad channel numbers and non-configured conditions.
   '  Parameters:
   '    DONTPRINT   :all warnings and errors encountered will not be printed
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
   PortType = BITIN
   NumPorts = FindPortsOfType(BoardNum, PortType, _
      ProgAbility, PortNum, NumBits, FirstBit)
   If Not (ProgAbility = DigitalIO.PROGBIT) Then NumPorts = 0
   
   If NumPorts < 1 Then
      lblInstruct.Caption = "Board " & Format(BoardNum, "0") & _
         " has no compatible digital bits."
      lblBitNum.Caption = ""
   Else
      ' if programmable, set direction of bit to input
      ' configure the first bit for digital input
      Direction = DIGITALIN
      ULStat = cbDConfigBit(BoardNum, PortNum, FirstBit, Direction)
      If Not (ULStat = 0) Then Stop
      PortName$ = GetPortString(PortNum)
      BitName$ = Format(FirstBit, "0")
      lblInstruct.Caption = "You may change the bit state by applying a TTL high " & _
      "or a TTL low to the corresponding pin on " & PortName & ", bit " & BitName & _
      " on board " & Format(BoardNum, "0") & "."
      tmrReadInputs.Enabled = True
   End If

End Sub

Private Sub tmrReadInputs_Timer()

   Dim BitPort As Long, FirstBit As Long
   Dim BitValue As Integer
   Dim PortName As String, BitName As String
   
   ' read a single bit status from the digital port
   
   '  Parameters:
   '    BoardNum    :the number used by CB.CFG to describe this board
   '    BitPort&    :the digital I/O port type (must be
   '                 AUXPORT or FIRSTPORTA for bit input)
   '    FirstBit    :the bit to read
   '    BitValue%   :the value read from the port
   
   BitPort& = AUXPORT
   If PortNum > AUXPORT Then BitPort& = FIRSTPORTA
   
   ULStat& = cbDBitIn(BoardNum, BitPort&, FirstBit, BitValue%)
   If ULStat& <> 0 Then Stop
   
   PortName$ = GetPortString(BitPort&)
   BitName$ = Format(FirstBit, "0")
   lblBitNum.Caption = PortName$ & ", bit " & BitName$ _
   & " state is " & Format$(BitValue%, "0")

End Sub

Private Sub cmdStopRead_Click()

   tmrReadInputs.Enabled = False
   End

End Sub


