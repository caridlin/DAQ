VERSION 5.00
Begin VB.Form frmSetBitOut 
   Appearance      =   0  'Flat
   BackColor       =   &H80000005&
   Caption         =   "Universal Library Digital Bit Out"
   ClientHeight    =   4425
   ClientLeft      =   105
   ClientTop       =   1545
   ClientWidth     =   5085
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
   ScaleHeight     =   4425
   ScaleWidth      =   5085
   Begin VB.CommandButton cmdEndProgram 
      Appearance      =   0  'Flat
      Caption         =   "Quit"
      Default         =   -1  'True
      Height          =   375
      Left            =   3960
      TabIndex        =   9
      Top             =   3840
      Width           =   855
   End
   Begin VB.CheckBox chkSetBit 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Set bit 7"
      ForeColor       =   &H80000008&
      Height          =   255
      Index           =   7
      Left            =   2880
      TabIndex        =   2
      Top             =   2700
      Value           =   2  'Grayed
      Width           =   1215
   End
   Begin VB.CheckBox chkSetBit 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Set bit 3"
      ForeColor       =   &H80000008&
      Height          =   255
      Index           =   3
      Left            =   720
      TabIndex        =   6
      Top             =   2700
      Value           =   2  'Grayed
      Width           =   1215
   End
   Begin VB.CheckBox chkSetBit 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Set bit 6"
      ForeColor       =   &H80000008&
      Height          =   255
      Index           =   6
      Left            =   2880
      TabIndex        =   3
      Top             =   2340
      Value           =   2  'Grayed
      Width           =   1215
   End
   Begin VB.CheckBox chkSetBit 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Set bit 2"
      ForeColor       =   &H80000008&
      Height          =   255
      Index           =   2
      Left            =   720
      TabIndex        =   7
      Top             =   2340
      Value           =   2  'Grayed
      Width           =   1215
   End
   Begin VB.CheckBox chkSetBit 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Set bit 5"
      ForeColor       =   &H80000008&
      Height          =   255
      Index           =   5
      Left            =   2880
      TabIndex        =   4
      Top             =   1980
      Value           =   2  'Grayed
      Width           =   1215
   End
   Begin VB.CheckBox chkSetBit 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Set bit 1"
      ForeColor       =   &H80000008&
      Height          =   255
      Index           =   1
      Left            =   720
      TabIndex        =   8
      Top             =   1980
      Value           =   2  'Grayed
      Width           =   1215
   End
   Begin VB.CheckBox chkSetBit 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Set bit 4"
      ForeColor       =   &H80000008&
      Height          =   255
      Index           =   4
      Left            =   2880
      TabIndex        =   5
      Top             =   1620
      Value           =   2  'Grayed
      Width           =   1215
   End
   Begin VB.CheckBox chkSetBit 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Set bit 0"
      ForeColor       =   &H80000008&
      Height          =   255
      Index           =   0
      Left            =   720
      TabIndex        =   1
      Top             =   1620
      Value           =   2  'Grayed
      Width           =   1215
   End
   Begin VB.Label lblValueSet 
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
      Left            =   360
      TabIndex        =   11
      Top             =   3240
      Width           =   4455
   End
   Begin VB.Label lblInstruct 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H000000FF&
      Height          =   735
      Left            =   600
      TabIndex        =   10
      Top             =   660
      Width           =   3855
   End
   Begin VB.Label lblDemoFunction 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Demonstration of cbDBitOut()"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   240
      TabIndex        =   0
      Top             =   240
      Width           =   4575
   End
End
Attribute VB_Name = "frmSetBitOut"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
'ULDO02.VBP================================================================

' File:                         ULDO02.VBP

' Library Call Demonstrated:    cbDBitOut&()

' Purpose:                      Sets the state of a single digital output bit.

' Demonstration:                Configures the first digital bit for output
'                               (if necessary) and writes a value to the bit.

' Other Library Calls:          cbDConfigPort&()
'                               cbErrHandling&()

' Special Requirements:         Board 0 must have a digital output port
'                               or have digital ports programmable as output.

'==========================================================================
Option Explicit

Const BoardNum As Long = 0              ' Board number

Dim PortNum As Long
Dim NumPorts As Long, NumBits As Long, FirstBit As Long
Dim PortType As Long, ProgAbility As Long
Dim Direction As Long, ULStat As Long

Private Sub Form_Load()
    
   Dim ReportError As Long, HandleError As Long
   Dim I As Integer, PortName As String
   
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
   
   ' If cbErrHandling& is set for STOPALL or STOPFATAL during the program
   ' design stage, Visual Basic will be unloaded when an error is encountered.
   ' We suggest trapping errors locally until the program is ready for compiling
   ' to avoid losing unsaved data during program design.  This can be done by
   ' setting cbErrHandling options as above and checking the value of ULStat&
   ' after a call to the library. If it is not equal to 0, an error has occurred.
   
   'determine if digital port exists, its capabilities, etc
   PortType = PORTOUT
   NumPorts = FindPortsOfType(BoardNum, PortType, _
      ProgAbility, PortNum, NumBits, FirstBit)
   If NumBits > 8 Then NumBits = 8
   For I% = NumBits To 7
       chkSetBit(I%).Visible = False
   Next I%

   If NumPorts < 1 Then
      lblInstruct.Caption = "Board " & Format(BoardNum, "0") & _
         " has no compatible digital ports."
   Else
      ' if programmable, set direction of port to output
      ' configure the first port for digital output
      '  Parameters:
      '    PortNum        :the output port
      '    Direction      :sets the port for input or output

      If ProgAbility = DigitalIO.PROGPORT Then
          Direction = DIGITALOUT
          ULStat = cbDConfigPort(BoardNum, PortNum, Direction)
          If Not (ULStat = 0) Then Stop
      End If
      PortName = GetPortString(PortNum)
      lblInstruct.Caption = "Set the output value of " & _
      PortName & " bits on board " & Format(BoardNum, "0") & _
      " using the check boxes."
   End If
   
End Sub

Private Sub chkSetBit_Click(Index As Integer)

   Dim BitPort As Long, FirstBit As Long, BitNum As Long
   Dim BitValue As Integer
   Dim PortName As String, BitName As String
   
   BitNum& = Index
   BitValue% = chkSetBit(Index).Value
   BitPort& = AUXPORT
   If PortNum > AUXPORT Then BitPort& = FIRSTPORTA
   
   ULStat& = cbDBitOut(BoardNum, BitPort&, FirstBit& + BitNum&, BitValue%)
   If ULStat& <> 0 Then Stop
   PortName$ = GetPortString(BitPort&)
   BitName$ = Format(FirstBit& + BitNum&, "0")
   Me.lblValueSet.Caption = PortName$ & ", bit " & _
   BitName$ & " value set to " & Format(BitValue%, "0")

End Sub

Private Sub cmdEndProgram_Click()

   If NumPorts > 0 Then
      ULStat& = cbDOut(BoardNum, PortNum, 0)
      If ULStat& <> 0 Then Stop
      
      ULStat& = cbDConfigPort(BoardNum, PortNum, DIGITALIN)
   End If
  
   End

End Sub

