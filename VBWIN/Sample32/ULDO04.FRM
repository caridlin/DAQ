VERSION 5.00
Begin VB.Form frmSetDigOut 
   Appearance      =   0  'Flat
   BackColor       =   &H80000005&
   Caption         =   "Universal Library Digital Output"
   ClientHeight    =   4005
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
   ScaleHeight     =   4005
   ScaleWidth      =   4965
   Begin VB.CommandButton cmdEndProgram 
      Appearance      =   0  'Flat
      Caption         =   "Quit"
      Height          =   435
      Left            =   3720
      TabIndex        =   7
      Top             =   3360
      Width           =   855
   End
   Begin VB.TextBox txtValSet 
      Appearance      =   0  'Flat
      Height          =   315
      Left            =   3360
      MaxLength       =   6
      TabIndex        =   4
      Text            =   "0"
      Top             =   2280
      Width           =   915
   End
   Begin VB.HScrollBar hsbSetDOutVal 
      Height          =   315
      LargeChange     =   51
      Left            =   960
      Max             =   255
      TabIndex        =   1
      Top             =   1680
      Width           =   2835
   End
   Begin VB.Label lblShowValOut 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Left            =   3360
      TabIndex        =   3
      Top             =   2760
      Width           =   855
   End
   Begin VB.Label lblDataValOut 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   300
      TabIndex        =   2
      Top             =   2760
      Width           =   2955
   End
   Begin VB.Label lblValSet 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Value set:"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   300
      TabIndex        =   6
      Top             =   2280
      Width           =   2955
   End
   Begin VB.Label lblInstruct 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Set output value using scroll bar or enter value in Value Set box:"
      ForeColor       =   &H000000FF&
      Height          =   675
      Left            =   420
      TabIndex        =   5
      Top             =   600
      Width           =   4155
   End
   Begin VB.Label lblDemoFunction 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Demonstration of cbDOut()"
      ForeColor       =   &H80000008&
      Height          =   315
      Left            =   360
      TabIndex        =   0
      Top             =   120
      Width           =   4335
   End
End
Attribute VB_Name = "frmSetDigOut"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
'ULDO04.VBP================================================================

' File:                         ULDO04.VBP

' Library Call Demonstrated:    cbDOut&()

' Purpose:                      Writes a byte to digital output ports.

' Demonstration:                Configures the first digital port for output
'                               (if necessary) and writes a value to the port.

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
   Dim PortName As String
   
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
   PortType = PORTOUT
   NumPorts = FindPortsOfType(BoardNum, PortType, _
      ProgAbility, PortNum, NumBits, FirstBit)

   If NumPorts < 1 Then
       lblInstruct.Caption = "Board " & Format(BoardNum, "0") & _
         " has no compatible digital ports."
       hsbSetDOutVal.Enabled = False
       txtValSet.Enabled = False
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
       lblInstruct.Caption = "Set the output value of " & PortName & _
       " on board " & Format(BoardNum, "0") & _
       " using the scroll bar or enter a value in the 'Value set' box."
       lblValSet.Caption = "Value set at " & PortName & ":"
       lblDataValOut.Caption = "Value written to " & PortName & ":"
   End If
   
End Sub

Private Sub hsbSetDOutVal_Change()

   Dim DataValue As Integer
   
   ' get a value to write to the port
   DataValue% = hsbSetDOutVal.Value
   txtValSet.Text = Format$(DataValue%, "0")
   WritePortValue DataValue%

End Sub

Private Sub txtValSet_Change()
   
   Dim LongValue As Long, DataValue As Integer
   
   LongValue& = Val(txtValSet.Text)
   If LongValue& > 32767 Then
      DataValue% = 32767
   Else
      DataValue% = LongValue&
   End If
   WritePortValue DataValue%

End Sub

Private Sub WritePortValue(ByVal DataValue As Integer)

   ' write the value to the output port
   '  Parameters:
   '    BoardNum    :the number used by CB.CFG to describe this board
   '    PortNum&    :the output port
   '    DataValue%  :the value written to the port
  
   ULStat& = cbDOut(BoardNum, PortNum&, DataValue%)

   If ULStat& <> 0 Then
      Stop
   Else
      lblShowValOut.Caption = Format$(DataValue%, "0")
   End If

End Sub

Private Sub cmdEndProgram_Click()
   
   If NumPorts > 0 Then
      ULStat& = cbDOut(BoardNum, PortNum&, 0)
      If ULStat& <> 0 Then Stop
      
      ULStat& = cbDConfigPort(BoardNum, PortNum&, DIGITALIN)
   End If
   
   End

End Sub


