VERSION 5.00
Begin VB.Form frmDataDisplay 
   Appearance      =   0  'Flat
   BackColor       =   &H80000005&
   Caption         =   "Universal Library Analog Input"
   ClientHeight    =   4140
   ClientLeft      =   2730
   ClientTop       =   1500
   ClientWidth     =   4905
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
   ScaleHeight     =   4140
   ScaleWidth      =   4905
   Begin VB.CommandButton cmdStartConvert 
      Appearance      =   0  'Flat
      Caption         =   "Start"
      Default         =   -1  'True
      Height          =   390
      Left            =   2760
      TabIndex        =   5
      Top             =   3600
      Width           =   780
   End
   Begin VB.CommandButton cmdStopConvert 
      Appearance      =   0  'Flat
      Caption         =   "Quit"
      Height          =   390
      Left            =   3960
      TabIndex        =   6
      Top             =   3600
      Width           =   780
   End
   Begin VB.TextBox txtNumChan 
      Appearance      =   0  'Flat
      Height          =   285
      Left            =   3480
      TabIndex        =   0
      Text            =   "0"
      Top             =   1890
      Width           =   495
   End
   Begin VB.Timer tmrConvert 
      Enabled         =   0   'False
      Interval        =   100
      Left            =   1560
      Top             =   3600
   End
   Begin VB.Label lblInstruction 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Board 0 must have analog inputs that support paced acquisition."
      ForeColor       =   &H000000FF&
      Height          =   735
      Left            =   240
      TabIndex        =   9
      Top             =   600
      Width           =   4455
   End
   Begin VB.Label lblShowVolts 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Left            =   3360
      TabIndex        =   8
      Top             =   3000
      Width           =   1335
   End
   Begin VB.Label lblVoltsRead 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Value converted to voltage:"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   480
      TabIndex        =   7
      Top             =   3000
      Width           =   2775
   End
   Begin VB.Label lblShowData 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Left            =   3360
      TabIndex        =   4
      Top             =   2520
      Width           =   1335
   End
   Begin VB.Label lblValueRead 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Value read from selected channel:"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   120
      TabIndex        =   3
      Top             =   2520
      Width           =   3135
   End
   Begin VB.Label lblChanPrompt 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Enter the Channel to display: "
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   720
      TabIndex        =   1
      Top             =   1920
      Width           =   2535
   End
   Begin VB.Label lblDemoFunction 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Demonstration of cbAIn()"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   120
      TabIndex        =   2
      Top             =   120
      Width           =   4650
   End
End
Attribute VB_Name = "frmDataDisplay"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
'ULAI01.VBP================================================================

' File:                         ULAI01.VBP

' Library Call Demonstrated:    cbAIn()

' Purpose:                      Reads an A/D Input Channel.

' Demonstration:                Displays the analog input on a user-specified
'                               channel.

' Other Library Calls:          cbErrHandling()

' Special Requirements:         Board 0 must have an A/D converter.
'                               Analog signal on an input channel.

'==========================================================================
Option Explicit

Const BoardNum As Long = 0             ' Board number
Dim CBRange As Long
Dim ADResolution As Long, NumAIChans As Long
Dim LowChan As Long, MaxChan As Long
Dim ULStat As Long

Private Sub Form_Load()
   
   Dim LowChan As Long
   Dim ChannelType As Long
   Dim DefaultTrig As Long
   Dim ULStat As Long
   Dim ReportError As Long, HandleError As Long

   ' declare revision level of Universal Library

   ULStat = cbDeclareRevision(CURRENTREVNUM)

   ' Initiate error handling
   '  activating error handling will trap errors like
   '  bad channel numbers and non-configured conditions.
   '  Parameters:
   '    PRINTALL    :all warnings and errors encountered will be printed
   '    DONTSTOP    :if an error is encountered, the program will not stop,
   '                 errors must be handled locally

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
   ChannelType = ANALOGINPUT
   NumAIChans = FindAnalogChansOfType(BoardNum, ChannelType, _
       ADResolution, CBRange, LowChan, DefaultTrig)

   If (NumAIChans = 0) Then
       lblInstruction.Caption = "Board " & Format(BoardNum, "0") & _
       " does not have analog input channels."
       cmdStartConvert.Enabled = False
       txtNumChan.Enabled = False
   Else
       Dim CurFunc As String
       CurFunc = "cbAIn()"
       If (ADResolution > 16) Then CurFunc = "cbAIn32()"
       lblDemoFunction.Caption = "Demonstration of " & CurFunc
       lblInstruction.Caption = "Board " & Format(BoardNum, "0") & _
           " collecting analog data using " & CurFunc & _
           " and Range of " & GetRangeString(CBRange) & "."
       MaxChan = LowChan + NumAIChans - 1
       lblChanPrompt.Caption = "Enter a channel (" & _
           Format(LowChan, "0") & " - " & Format(MaxChan, "0") & "):"
   End If

End Sub

Private Sub cmdStartConvert_Click()

   If tmrConvert.Enabled Then
      cmdStartConvert.Caption = "Start"
      tmrConvert.Enabled = False
   Else
      cmdStartConvert.Caption = "Stop"
      tmrConvert.Enabled = True
   End If
   
End Sub

Private Sub tmrConvert_Timer()

   Dim Chan As Long
   Dim DataValue32 As Long
   Dim Options As Long
   Dim DataValue As Integer
   Dim HighResEngUnits As Double
   Dim EngUnits As Single
   
   ' Collect the data with cbAIn()

   '  Parameters:
   '    BoardNum     :the number used by CB.CFG to describe this board
   '    Chan         :the input channel number
   '    CBRange      :the Range for the board.
   '    DataValue    :the name for the value collected
    
   Chan = Val(txtNumChan.Text)  ' set input channel
   If (Chan > MaxChan) Then Chan = MaxChan
   txtNumChan.Text = Format(Chan, "0")
   Options = 0 ' this is ignored
   
   If ADResolution > 16 Then
      ULStat = cbAIn32(BoardNum, Chan, CBRange, DataValue32, Options)
      If ULStat <> 0 Then Stop

      ULStat = cbToEngUnits32(BoardNum, CBRange, DataValue32, HighResEngUnits)
      If ULStat <> 0 Then Stop

      lblShowData.Caption = Format$(DataValue32, "0")   ' print the counts
      lblShowVolts.Caption = Format$(HighResEngUnits#, "0.00000") + " Volts"   ' print the voltage
   Else
      ULStat = cbAIn(BoardNum, Chan, CBRange, DataValue)
      If ULStat <> 0 Then Stop

      ULStat = cbToEngUnits(BoardNum, CBRange, DataValue, EngUnits)
      If ULStat <> 0 Then Stop

      lblShowData.Caption = Format$(DataValue, "0")   ' print the counts
      lblShowVolts.Caption = Format$(EngUnits!, "0.000") + " Volts"   ' print the voltage
   End If

End Sub

Private Sub cmdStopConvert_Click()
   
   tmrConvert.Enabled = False
   End

End Sub

