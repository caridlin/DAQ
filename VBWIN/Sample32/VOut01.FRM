VERSION 5.00
Begin VB.Form frmSendAData 
   Appearance      =   0  'Flat
   BackColor       =   &H80000005&
   Caption         =   "Universal Library Analog Output "
   ClientHeight    =   3045
   ClientLeft      =   105
   ClientTop       =   1545
   ClientWidth     =   5700
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
   ScaleHeight     =   3045
   ScaleWidth      =   5700
   Begin VB.CommandButton cmdEndProgram 
      Appearance      =   0  'Flat
      Caption         =   "Quit"
      Height          =   390
      Left            =   4740
      TabIndex        =   3
      Top             =   2460
      Width           =   825
   End
   Begin VB.TextBox txtVoltsToSet 
      Appearance      =   0  'Flat
      Height          =   375
      Left            =   3960
      TabIndex        =   0
      Top             =   1440
      Width           =   1455
   End
   Begin VB.Label lblPrompt 
      Alignment       =   2  'Center
      BackColor       =   &H00FFFFFF&
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
      Height          =   795
      Left            =   540
      TabIndex        =   6
      Top             =   480
      Width           =   4695
   End
   Begin VB.Label lblShowVoltage 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Left            =   3480
      TabIndex        =   4
      Top             =   1980
      Width           =   1995
   End
   Begin VB.Label lblVoltage 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   300
      TabIndex        =   5
      Top             =   1980
      Width           =   3015
   End
   Begin VB.Label lblUseScroll 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Enter a voltage within the DACs range:"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   180
      TabIndex        =   2
      Top             =   1500
      Width           =   3675
   End
   Begin VB.Label lblDemoFunction 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Demonstration of cbVOut()"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   420
      TabIndex        =   1
      Top             =   120
      Width           =   4935
   End
End
Attribute VB_Name = "frmSendAData"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
'VOut01.VBP================================================================

' File:                         VOut01.VBP

' Library Call Demonstrated:    cbVOut()

' Purpose:                      Writes to a D/A Output Channel.

' Demonstration:                Sends a digital output to D/A 0.

' Other Library Calls:          cbErrHandling()

' Special Requirements:         Board 0 must have a D/A converter.

' (c) Copyright 2005-2011, Measurement Computing Corp.
' All rights reserved.
'==========================================================================
Option Explicit

Const BoardNum As Long = 0         ' Board number

Dim CBRange As Long
Dim DAResolution As Long, NumAOChans As Long
Dim LowChan As Long, MaxChan As Long
Dim ULStat As Long

Const Chan As Long = 0             ' output channel
Dim RangeSelected As Long

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
   ChannelType = ANALOGOUTPUT
   NumAOChans = FindAnalogChansOfType(BoardNum, ChannelType, _
       DAResolution, CBRange, LowChan, DefaultTrig)

   If (NumAOChans = 0) Then
       lblPrompt.Caption = "Board " & Format(BoardNum, "0") & _
       " does not have analog output channels."
       txtVoltsToSet.Enabled = False
   Else
       lblPrompt.Caption = "Board " & Format(BoardNum, "0") & _
           " generating analog data using cbVOut()" & _
           " and Range of " & GetRangeString(CBRange) & "."
       MaxChan = LowChan + NumAOChans - 1
   End If

End Sub

Private Sub txtVoltsToSet_Change()

   Dim DataValue As Single, Options As Long
   Dim Chan As Long, CountValue As Integer
   Dim VoltsOut As Single
   
   ' send the digital output value to D/A 0 with cbVOut()
   DataValue! = Val(txtVoltsToSet.Text)
   Options& = DEFAULTOPTION

   ' Parameters:
   '   BoardNum    :the number used by CB.CFG to describe this board
   '   Chan       :the D/A output channel
   '   Range      :ignored if board does not have programmable rage
   '   DataValue  :the value to send to Chan
   
   ULStat = cbVOut(BoardNum, Chan, CBRange, DataValue!, Options&)
   If ULStat <> 0 Then Stop

   ULStat = cbFromEngUnits(BoardNum, CBRange, DataValue!, CountValue)
   VoltsOut = GetVoltsFromCounts(CountValue)
   lblVoltage.Caption = "The voltage at DAC channel " & Format$(Chan, "0") & " is:"
   lblShowVoltage.Caption = Format$(VoltsOut, "0.0####") & " Volts"

End Sub

Private Function GetVoltsFromCounts(ByVal CountVal As Integer) As Single

   Dim LongVal As Long, VoltRange As Single
   Dim LSB As Single, Voltage As Single
   
   LongVal = CountVal
   If CountVal < 0 Then LongVal = CountVal + 65536
   VoltRange = GetRangeVolts(CBRange)
   LSB = VoltRange / 2 ^ DAResolution
   Voltage = LSB * LongVal
   If CBRange < 100 Then Voltage = Voltage - (VoltRange / 2)
   GetVoltsFromCounts = Voltage
   
End Function

Private Sub cmdEndProgram_Click()
   
   End

End Sub


