VERSION 5.00
Begin VB.Form frmDigitalIn 
   Appearance      =   0  'Flat
   BackColor       =   &H80000005&
   Caption         =   "Universal Library Digital In"
   ClientHeight    =   3375
   ClientLeft      =   105
   ClientTop       =   1545
   ClientWidth     =   4965
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
   ScaleHeight     =   3375
   ScaleWidth      =   4965
   Begin VB.CommandButton cmdEndProgram 
      Appearance      =   0  'Flat
      Caption         =   "Quit"
      Default         =   -1  'True
      Height          =   495
      Left            =   3840
      TabIndex        =   3
      Top             =   2640
      Width           =   855
   End
   Begin VB.Timer tmrReadInputs 
      Enabled         =   0   'False
      Interval        =   200
      Left            =   2880
      Top             =   2700
   End
   Begin VB.Label lblShowPortVal 
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
      Left            =   3600
      TabIndex        =   2
      Top             =   1920
      Width           =   975
   End
   Begin VB.Label lblPortVal 
      Alignment       =   1  'Right Justify
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
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   480
      TabIndex        =   1
      Top             =   1920
      Width           =   2955
   End
   Begin VB.Label lblInstruct 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Input a TTL high or low level to digital inputs to change Data Value."
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
      Height          =   855
      Left            =   480
      TabIndex        =   4
      Top             =   600
      Width           =   3975
   End
   Begin VB.Label lblDemoFunction 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Demonstration of cbDIn()"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   180
      TabIndex        =   0
      Top             =   120
      Width           =   4575
   End
End
Attribute VB_Name = "frmDigitalIn"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
'ULDI04.VBP===============================================================

' File:                         ULDI04.VBP

' Library Call Demonstrated:    cbDIn&()

' Purpose:                      Reads a digital input port.

' Demonstration:                Configures the first compatible port
'                               for input (if necessary) and then
'                               reads and displays the value on the port.

' Other Library Calls:          cbDConfigPort&()
'                               cbErrHandling&()

' Special Requirements:         Board 0 must have a digital input port
'                               or have digital ports programmable as input.

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
   Dim PortName As String
   
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
   PortType = PORTIN
   NumPorts = FindPortsOfType(BoardNum, PortType, ProgAbility, PortNum, NumBits, FirstBit)
   
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
       Me.lblPortVal.Caption = "The value read at " & PortName & " is:"
       tmrReadInputs.Enabled = True
   End If
   
End Sub

Private Sub tmrReadInputs_Timer()

   Dim DataValue As Integer
   
   ' read the digital input and display
     
   ' Parameters:
   '   BoardNum    :the number used by CB.CFG to describe this board
   '   PortNum&    :the input port
   '   DataValue&  :the value read from the port
    
   ULStat& = cbDIn(BoardNum, PortNum&, DataValue%)
   If ULStat& <> 0 Then Stop
      
   ' display the value collected from the port

   Me.lblShowPortVal.Caption = Format$(DataValue%, "0")

End Sub

Private Sub cmdEndProgram_Click()

   tmrReadInputs.Enabled = False
   End

End Sub


