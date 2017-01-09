VERSION 5.00
Begin VB.Form frmAnalogTrig 
   Appearance      =   0  'Flat
   BackColor       =   &H80000005&
   Caption         =   "Universal Library Analog Trigger"
   ClientHeight    =   4485
   ClientLeft      =   3315
   ClientTop       =   1485
   ClientWidth     =   6435
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
   ScaleHeight     =   4485
   ScaleWidth      =   6435
   Begin VB.CommandButton cmdStartConvert 
      Appearance      =   0  'Flat
      Caption         =   "Start"
      Default         =   -1  'True
      Enabled         =   0   'False
      Height          =   390
      Left            =   4080
      TabIndex        =   2
      Top             =   3960
      Width           =   900
   End
   Begin VB.TextBox txtShowTrigSet 
      Appearance      =   0  'Flat
      Enabled         =   0   'False
      Height          =   255
      Left            =   2820
      TabIndex        =   10
      Text            =   "1.25"
      Top             =   1920
      Width           =   615
   End
   Begin VB.OptionButton chkPosTrigger 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Trigger above this value"
      Enabled         =   0   'False
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   3540
      TabIndex        =   4
      Top             =   1800
      Width           =   2715
   End
   Begin VB.OptionButton chkNegTrigger 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Trigger below this value"
      Enabled         =   0   'False
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   3540
      TabIndex        =   3
      Top             =   2070
      Value           =   -1  'True
      Width           =   2715
   End
   Begin VB.TextBox txtShowChannel 
      Appearance      =   0  'Flat
      Enabled         =   0   'False
      Height          =   285
      Left            =   4200
      TabIndex        =   0
      Text            =   "0"
      Top             =   450
      Width           =   372
   End
   Begin VB.Timer tmrStartConvert 
      Enabled         =   0   'False
      Interval        =   200
      Left            =   3000
      Top             =   3960
   End
   Begin VB.CommandButton cmdStopConvert 
      Appearance      =   0  'Flat
      Caption         =   "Quit"
      Height          =   390
      Left            =   5340
      TabIndex        =   7
      Top             =   3960
      Width           =   900
   End
   Begin VB.Label lblVoltStatus 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   555
      Left            =   240
      TabIndex        =   13
      Top             =   3240
      Width           =   4575
   End
   Begin VB.Label lblShowVolts 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Left            =   4920
      TabIndex        =   11
      Top             =   3240
      Width           =   1275
   End
   Begin VB.Label lblShowTrigValue 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Left            =   4920
      TabIndex        =   5
      Top             =   2760
      Width           =   1275
   End
   Begin VB.Label lblTrigStatus 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Left            =   240
      TabIndex        =   6
      Top             =   2760
      Width           =   4575
   End
   Begin VB.Label lblEnterVal 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Enter a value in volts: "
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   240
      TabIndex        =   12
      Top             =   1920
      Width           =   2475
   End
   Begin VB.Label lblWarn 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Note: Channel above must have an input that meets the trigger conditions or program will appear to hang."
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
      Height          =   675
      Left            =   720
      TabIndex        =   8
      Top             =   900
      Width           =   4980
   End
   Begin VB.Label lblTriggerChan 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Enter the trigger input channel:"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   900
      TabIndex        =   1
      Top             =   480
      Width           =   3180
   End
   Begin VB.Label lblDemoFunction 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Demonstration of cbATrig()"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   120
      TabIndex        =   9
      Top             =   60
      Width           =   6195
   End
End
Attribute VB_Name = "frmAnalogTrig"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
'ULAI07.FRM ============================================================

' File:                         ULAI07.FRM

' Library Call Demonstrated:    cbATrig()

' Purpose:                      Waits for a specified analog input channel to
'                               go above or below a specified value.

' Demonstration:                Displays the digital value of a user-specified
'                               analog input channel when the user-specifed
'                               value is detected.

' Other Library Calls:          cbErrHandling()

' Special Requirements:         Board 0 must have an A/D converter.
'                               Analog signal on an input channel.

'==========================================================================
Option Explicit

Const BoardNum As Long = 0              ' Board number

Dim CBRange As Long
Dim ADResolution As Long, NumAIChans As Long
Dim HighChan As Long, LowChan As Long, MaxChan As Long
Dim ULStat As Long

Private Sub Form_Load()

   Dim DefaultTrig As Long
   Dim ReportError As Long, HandleError As Long
   
   ' declare revision level of Universal Library

   ULStat = cbDeclareRevision(CURRENTREVNUM)
   
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
   If ULStat <> 0 Then Stop
   SetAnalogIODefaults ReportError, HandleError
    
   ' If cbErrHandling is set for STOPALL or STOPFATAL during the program
   ' design stage, Visual Basic will be unloaded when an error is encountered.
   ' We suggest trapping errors locally until the program is ready for compiling
   ' to avoid losing unsaved data during program design.  This can be done by
   ' setting cbErrHandling options as above and checking the value of ULStat
   ' after a call to the library. If it is not equal to 0, an error has occurred.

   ' determine the number of analog channels and their capabilities
   Dim ChannelType As Long
   ChannelType = ANALOGINPUT
   NumAIChans = FindAnalogChansOfType(BoardNum, ChannelType, _
      ADResolution, CBRange, LowChan, DefaultTrig)

   If (NumAIChans = 0) Then
       lblWarn.Caption = "Board " & Format(BoardNum, "0") & _
         " does not have analog input channels."
   ElseIf (ADResolution > 16) Then
       lblWarn.Caption = "Board " & Format(BoardNum, "0") & _
       " resolution is greater than 16-bit. The ATrig function " & _
       "does not support high resolution devices."
       cmdStartConvert.Enabled = False
   Else
       lblWarn.Caption = "Board " & Format(BoardNum, "0") & _
           " collecting analog data meeting trigger criterea " & _
           " with input Range set to " & GetRangeString(CBRange) & "."
       HighChan = LowChan & NumAIChans - 1
       lblTriggerChan.Caption = "Enter a channel (" & _
           Format(LowChan, "0") & " - " & Format(HighChan, "0") & "):"
       UpdateTrigCriterea
       cmdStartConvert.Enabled = True
       txtShowChannel.Enabled = True
       txtShowTrigSet.Enabled = True
       chkNegTrigger.Enabled = True
       chkPosTrigger.Enabled = True
   End If
   
End Sub

Private Sub cmdStartConvert_Click()

   If tmrStartConvert.Enabled Then
      tmrStartConvert.Enabled = False
      cmdStartConvert.Caption = "Start"
      UpdateTrigCriterea
   Else
      tmrStartConvert.Enabled = True
      cmdStartConvert.Caption = "Stop"
      lblTrigStatus.Caption = "Waiting for trigger..."
   End If

End Sub

Private Sub tmrStartConvert_Timer()
   
   Dim Chan As Long
   Dim TrigType As Long
   Dim TrigValue As Integer
   Dim DataValue As Integer
   Dim EngUnits As Single
   
   ' Monitor the channel with cbATrig()
   '  The input value that meets the threshold will become DataValue
   '  The data value will be updated and displayed until a Stop event occurs.
   '  Parameters:
   '    BoardNum   :the number used by CB.CFG to describe this board
   '    Chan       :the input channel number
   '    TrigType   :specifies whether the trigger is to be above
   '                 or below TrigValue
   '    TrigValue  :the threshold value that will cause the trigger
   '    CBRange    :the gain for the board
   '    DataValue  :the input value read from Chan

   Chan = val(txtShowChannel.Text)
   If (Chan > MaxChan) Then Chan = MaxChan
   txtShowChannel.Text = Format(Chan, "0")
   
   EngUnits = val(txtShowTrigSet.Text)

   TrigValue = GetTrigCounts(BoardNum, CBRange, EngUnits)
   TrigType = TRIGABOVE
   If chkNegTrigger.Value Then TrigType = TRIGBELOW
   
   ULStat = cbATrig(BoardNum, Chan, TrigType, TrigValue, CBRange, DataValue)
   If ULStat <> 0 Then Stop
   
   ' print the value that meets the threshold
  
   lblTrigStatus.Caption = "The value that caused the last trigger was:"
   lblShowTrigValue.Caption = Format$(DataValue, "0")

   ULStat = cbToEngUnits(BoardNum, CBRange, DataValue, EngUnits)
   lblShowVolts.Caption = Format$(EngUnits, "0.00###\V")
   lblVoltStatus.Caption = "Trigger counts converted to voltage:"

End Sub

Private Function GetTrigCounts(ByVal bd As Long, ByVal BdRange As Long, ByVal val As Single) As Integer

   Dim fCounts As Single
   Dim FSCounts As Long, LongCounts As Long
   Dim FSEngUnits As Single
   Dim iFSCounts As Integer, IntCounts As Integer
   Dim RangeIsBipolar As Boolean

   RangeIsBipolar = False
   
   'check if range is bipolar or unipolar
   FSEngUnits = 0
   ULStat = cbToEngUnits(bd, BdRange, FSCounts&, FSEngUnits)
   If (FSEngUnits < 0) Then RangeIsBipolar = True
         
   FSCounts = (2 ^ ADResolution) - 1
   If FSCounts > 32767 Then
      iFSCounts = FSCounts - 65536
   Else
      iFSCounts = FSCounts
   End If
   ULStat = cbToEngUnits(bd, BdRange, iFSCounts, FSEngUnits)

   If RangeIsBipolar Then
       fCounts = CSng((FSCounts / 2#) * (1# + val / FSEngUnits))
   Else
       fCounts = FSCounts * val / FSEngUnits
   End If

   If fCounts > FSCounts Then fCounts = FSCounts
   If fCounts < 0 Then fCounts = 0

   LongCounts = CLng(fCounts!)
   'convert to signed integer
   If LongCounts > 32767 Then
      IntCounts = LongCounts - 65536
   Else
      IntCounts = LongCounts
   End If
   GetTrigCounts = IntCounts
   
End Function

Private Sub UpdateTrigCriterea()

    Dim TrigChan, TrigCondition, TrigVoltage As String

    TrigCondition = "below"
    If chkPosTrigger.Value Then TrigCondition = "above"
    TrigVoltage = txtShowTrigSet.Text
    If TrigVoltage = "" Then TrigVoltage = "0"
    TrigChan = txtShowChannel.Text
    If TrigChan = "" Then TrigChan = "0"
    lblVoltStatus.Caption = "Apply a voltage or signal to channel " & _
    Format(TrigChan, "0") & " that meets the trigger criterea  ' " & _
    TrigCondition & " " & TrigVoltage & " volts '."
    lblShowTrigValue.Caption = ""
    lblShowVolts.Caption = ""
    Me.lblTrigStatus.Caption = ""

End Sub

Private Sub chkNegTrigger_Click()

   UpdateTrigCriterea

End Sub

Private Sub chkPosTrigger_Click()

   UpdateTrigCriterea
   
End Sub

Private Sub txtShowTrigSet_Change()

   UpdateTrigCriterea
   
End Sub

Private Sub cmdStopConvert_Click()

   End

End Sub

