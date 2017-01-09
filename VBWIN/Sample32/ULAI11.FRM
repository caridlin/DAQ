VERSION 5.00
Begin VB.Form frmManualConvert 
   Appearance      =   0  'Flat
   BackColor       =   &H80000005&
   Caption         =   "Universal Library Analog Data Convert"
   ClientHeight    =   4140
   ClientLeft      =   1650
   ClientTop       =   1515
   ClientWidth     =   6930
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
   ScaleWidth      =   6930
   Begin VB.TextBox txtNumChan 
      Appearance      =   0  'Flat
      Height          =   315
      Left            =   4200
      TabIndex        =   16
      Text            =   "0"
      Top             =   1320
      Width           =   375
   End
   Begin VB.CommandButton cmdStartConvert 
      Appearance      =   0  'Flat
      Caption         =   "Start"
      Default         =   -1  'True
      Height          =   396
      Left            =   4800
      TabIndex        =   3
      Top             =   3540
      Width           =   780
   End
   Begin VB.CommandButton cmdStopConvert 
      Appearance      =   0  'Flat
      Caption         =   "Quit"
      Height          =   396
      Left            =   5940
      TabIndex        =   4
      Top             =   3540
      Width           =   780
   End
   Begin VB.Timer tmrConvert 
      Enabled         =   0   'False
      Interval        =   300
      Left            =   6300
      Top             =   2820
   End
   Begin VB.Label lblChanPrompt 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Channel to display: "
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   1800
      TabIndex        =   17
      Top             =   1320
      Width           =   2295
   End
   Begin VB.Label lblShowCalcResults 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Left            =   3360
      TabIndex        =   11
      Top             =   3030
      Width           =   1575
   End
   Begin VB.Label lblCalcResult 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Value after conversion to voltage:"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   120
      TabIndex        =   10
      Top             =   3030
      Width           =   3135
   End
   Begin VB.Label lblShowData 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Left            =   3360
      TabIndex        =   2
      Top             =   2760
      Width           =   975
   End
   Begin VB.Label lblValueRead 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Value read from selected channel:"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   120
      TabIndex        =   1
      Top             =   2760
      Width           =   3135
   End
   Begin VB.Label lblGainCodeVal 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Left            =   5400
      TabIndex        =   7
      Top             =   2220
      Width           =   495
   End
   Begin VB.Label lblGainCode 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Left            =   3600
      TabIndex        =   6
      Top             =   2220
      Width           =   1335
   End
   Begin VB.Label lblMinVal 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Left            =   2640
      TabIndex        =   8
      Top             =   2220
      Width           =   640
   End
   Begin VB.Label lblFullScaleNeg 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Negative full scale voltage:"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   120
      TabIndex        =   13
      Top             =   2220
      Width           =   2415
   End
   Begin VB.Label lblMaxVal 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Left            =   2640
      TabIndex        =   9
      Top             =   1980
      Width           =   640
   End
   Begin VB.Label lblFullScalePos 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Positive full scale voltage:"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   120
      TabIndex        =   12
      Top             =   1980
      Width           =   2415
   End
   Begin VB.Label lblCVLabel 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Code Value"
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
      Left            =   5160
      TabIndex        =   15
      Top             =   1860
      Width           =   1215
   End
   Begin VB.Label lblGCLabel 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Gain Code"
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
      Left            =   3720
      TabIndex        =   14
      Top             =   1860
      Width           =   1095
   End
   Begin VB.Label lblInstruct 
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
      ForeColor       =   &H000000FF&
      Height          =   615
      Left            =   240
      TabIndex        =   5
      Top             =   480
      Width           =   6435
   End
   Begin VB.Label lblDemoFunction 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Demonstration of raw data conversion."
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   120
      TabIndex        =   0
      Top             =   60
      Width           =   6690
   End
End
Attribute VB_Name = "frmManualConvert"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
'ULAI11.VBP================================================================

' File:                         ULAI11.VBP

' Library Call Demonstrated:    cbAIn()
'                               cbToEngUnits()

' Purpose:                      Convert a raw data value to a voltage.

' Demonstration:                Displays a voltage on a user-specified
'                               channel.

' Other Library Calls:          cbErrHandling()

' Special Requirements:         Board 0 must have an A/D converter.
'                               Analog signal on specified input channel.
'                               If you are using a 16 bit board, see the
'                               CONVERT RAW DATA VALUES section for the
'                               correct lines of code to be used.

'==========================================================================
Option Explicit

Const BoardNum As Long = 0           ' Board number

Dim CBRange As Long
Dim ADResolution As Long, NumAIChans As Long
Dim LowChan As Long, MaxChan As Long
Dim ULStat As Long

Private Sub Form_Load()

   Dim ChannelType As Long
   Dim DefaultTrig As Long
   Dim RangeVolts As Single
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
      lblInstruct.Caption = "Board " & Format(BoardNum, "0") & _
         " does not have analog input channels."
      cmdStartConvert.Enabled = False
      txtNumChan.Enabled = False
   Else
      Dim CurFunc As String
      Dim CurRange As String
      CurRange = GetRangeString(CBRange)
      CurFunc = "cbAIn()"
      If (ADResolution > 16) Then CurFunc = "cbAIn32()"
      lblDemoFunction.Caption = "Demonstration of " & CurFunc
      lblInstruct.Caption = "Board " & Format(BoardNum, "0") & _
         " collecting analog data using " & CurFunc & _
         " and Range of " & CurRange & "."
      MaxChan = LowChan + NumAIChans - 1
      lblChanPrompt.Caption = "Enter a channel (" & _
         Format(LowChan, "0") & " - " & Format(MaxChan, "0") & "):"
      RangeVolts = GetRangeVolts(CBRange)
      If CBRange < 100 Then
         lblMaxVal.Caption = RangeVolts / 2
         lblMinVal.Caption = 0 - (RangeVolts / 2)
      Else
         lblMaxVal.Caption = RangeVolts
         lblMinVal.Caption = 0
      End If
      lblGainCode.Caption = CurRange
      lblGainCodeVal = Format(CBRange, "0")
   End If

End Sub

Private Sub cmdStartConvert_Click()

   tmrConvert.Enabled = True
   cmdStartConvert.Visible = False
   cmdStartConvert.Default = False
   cmdStopConvert.Visible = True
   cmdStopConvert.Default = True

End Sub

Private Sub tmrConvert_Timer()

   Dim Chan As Long
   Dim DataValue32 As Long
   Dim Options As Long
   Dim HighReEngUnits As Double
   Dim EngUnits As Single
   Dim DataValue As Integer
   
   ' Parameters:
   '   BoardNum   :the number used by CB.CFG to describe this board
   '   Chan       :the input channel number
   '   CBRange    :the gain for the board.
   '   DataValue  :the name for the value collected
        
   Chan = Val(txtNumChan.Text)
   If (Chan > MaxChan) Then Chan = MaxChan
   txtNumChan.Text = Format(Chan, "0")
   
   If ADResolution > 16 Then
       ULStat = cbAIn32(BoardNum, Chan, CBRange, DataValue32, Options)
       If ULStat <> 0 Then Stop
       
       '******* CONVERT RAW DATA VALUES TO VOLTAGES ********
       ULStat = cbToEngUnits32(BoardNum, CBRange, DataValue32&, HighReEngUnits)
       If ULStat <> 0 Then Stop
        
       lblShowData.Caption = Format$(DataValue32&, "0")
       lblShowCalcResults.Caption = Format$(HighReEngUnits#, "0.00000 \V\o\l\t\s")
   Else
       ULStat = cbAIn(BoardNum, Chan, CBRange, DataValue)
       If ULStat <> 0 Then Stop
   
       '******* CONVERT RAW DATA VALUES TO VOLTAGES ********
       ULStat = cbToEngUnits(BoardNum, CBRange, DataValue, EngUnits)
       If ULStat <> 0 Then Stop
    
       lblShowData.Caption = Format$(DataValue, "0")
       lblShowCalcResults.Caption = Format$(EngUnits!, "0.000 \V\o\l\t\s")
  End If

End Sub

Private Sub cmdStopConvert_Click()

   tmrConvert.Enabled = False
   End

End Sub


