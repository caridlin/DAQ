VERSION 5.00
Begin VB.Form frmDataDisplay 
   Appearance      =   0  'Flat
   BackColor       =   &H80000005&
   Caption         =   "Universal Library Voltage Input"
   ClientHeight    =   3405
   ClientLeft      =   2730
   ClientTop       =   1500
   ClientWidth     =   6390
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
   ScaleHeight     =   3405
   ScaleWidth      =   6390
   Begin VB.CommandButton cmdStartConvert 
      Appearance      =   0  'Flat
      Caption         =   "Start"
      Default         =   -1  'True
      Height          =   390
      Left            =   4080
      TabIndex        =   5
      Top             =   2760
      Width           =   780
   End
   Begin VB.CommandButton cmdStopConvert 
      Appearance      =   0  'Flat
      Caption         =   "Quit"
      Height          =   390
      Left            =   5340
      TabIndex        =   6
      Top             =   2760
      Width           =   780
   End
   Begin VB.TextBox txtNumChan 
      Appearance      =   0  'Flat
      Height          =   315
      Left            =   3900
      TabIndex        =   0
      Text            =   "0"
      Top             =   1620
      Width           =   495
   End
   Begin VB.Timer tmrConvert 
      Enabled         =   0   'False
      Interval        =   100
      Left            =   2640
      Top             =   2760
   End
   Begin VB.Label lblInstruction 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Board 0 must have analog inputs."
      ForeColor       =   &H000000FF&
      Height          =   735
      Left            =   840
      TabIndex        =   7
      Top             =   480
      Width           =   4455
   End
   Begin VB.Label lblShowData 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Left            =   3900
      TabIndex        =   4
      Top             =   2100
      Width           =   2175
   End
   Begin VB.Label lblValueRead 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Voltage read from selected channel:"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   180
      TabIndex        =   3
      Top             =   2100
      Width           =   3615
   End
   Begin VB.Label lblChanPrompt 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Enter the Channel to display: "
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   540
      TabIndex        =   1
      Top             =   1620
      Width           =   3255
   End
   Begin VB.Label lblDemoFunction 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Demonstration of cbVIn()"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   180
      TabIndex        =   2
      Top             =   120
      Width           =   5970
   End
End
Attribute VB_Name = "frmDataDisplay"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
'ULAI01.VBP================================================================

' File:                         VIn01.VBP

' Library Call Demonstrated:    cbVIn&()

' Purpose:                      Reads an A/D Input Channel.

' Demonstration:                Displays the analog input on a user-specified
'                               channel.

' Other Library Calls:          cbErrHandling&()

' Special Requirements:         Board 0 must have an A/D converter.
'                               Analog signal on an input channel.

' (c) Copyright 2005-2011, Measurement Computing Corp.
' All rights reserved.
'==========================================================================
Option Explicit

Const BoardNum As Long = 0              ' Board number

Dim CBRange As Long
Dim ADResolution As Long, NumAIChans As Long
Dim LowChan As Long, MaxChan As Long
Dim ULStat As Long

Private Sub Form_Load()

   Dim DefaultTrig As Long
   Dim ChannelType As Long
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

   ' If cbErrHandling& is set for STOPALL or STOPFATAL during the program
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
   Else
       lblInstruction.Caption = "Board " & Format(BoardNum, "0") & _
           " collecting analog data using cbVIn()" & _
           " and Range of " & GetRangeString(CBRange) & "."
       MaxChan = LowChan + NumAIChans - 1
       lblChanPrompt.Caption = "Enter a channel (" & _
           Format(LowChan, "0") & " - " & Format(MaxChan, "0") & "):"
   End If

End Sub

Private Sub cmdStartConvert_Click()

   If tmrConvert.Enabled Then
      tmrConvert.Enabled = False
      cmdStartConvert.Caption = "Start"
   Else
      tmrConvert.Enabled = True
      cmdStartConvert.Caption = "Stop"
   End If
   
End Sub

Private Sub tmrConvert_Timer()

   Dim Chan As Long
   Dim Options As Long
   Dim DataValue As Single
   
   ' Collect the data with cbVIn&()

   '  Parameters:
   '    BoardNum     :the number used by CB.CFG to describe this board
   '    Chan&        :the input channel number
   '    Gain         :the gain for the board.
   '    DataValue&   :the name for the value collected
    
   Chan = Val(txtNumChan.Text)  ' set input channel
   If (Chan > MaxChan) Then Chan = MaxChan
   txtNumChan.Text = Format(Chan, "0")
   Options = DEFAULTOPTION

   ULStat = cbVIn(BoardNum, Chan, CBRange, DataValue, Options)
   If ULStat <> 0 Then Stop

   lblShowData.Caption = DataValue!

End Sub

Private Sub cmdStopConvert_Click()
   
   tmrConvert.Enabled = False
   End

End Sub

