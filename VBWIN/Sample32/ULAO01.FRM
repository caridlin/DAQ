VERSION 5.00
Begin VB.Form frmSendAData 
   Appearance      =   0  'Flat
   BackColor       =   &H80000005&
   Caption         =   "Universal Library Analog Output "
   ClientHeight    =   3765
   ClientLeft      =   105
   ClientTop       =   1545
   ClientWidth     =   6030
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
   ScaleHeight     =   3765
   ScaleWidth      =   6030
   Begin VB.CommandButton cmdEndProgram 
      Appearance      =   0  'Flat
      Caption         =   "Quit"
      Height          =   390
      Left            =   5040
      TabIndex        =   5
      Top             =   3240
      Width           =   825
   End
   Begin VB.TextBox txtVoltsToSet 
      Appearance      =   0  'Flat
      Height          =   285
      Left            =   4200
      TabIndex        =   0
      Top             =   1560
      Width           =   1215
   End
   Begin VB.Label lblInstruction 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Board 0 must have at least one analog output."
      ForeColor       =   &H000000FF&
      Height          =   735
      Left            =   360
      TabIndex        =   8
      Top             =   600
      Width           =   5295
   End
   Begin VB.Label lblShowVoltage 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Left            =   4080
      TabIndex        =   6
      Top             =   2640
      Width           =   1815
   End
   Begin VB.Label lblVoltage 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   360
      TabIndex        =   7
      Top             =   2640
      Width           =   3495
   End
   Begin VB.Label lblShowValue 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Left            =   4080
      TabIndex        =   4
      Top             =   2250
      Width           =   1815
   End
   Begin VB.Label lblValueSent 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   360
      TabIndex        =   3
      Top             =   2250
      Width           =   3495
   End
   Begin VB.Label lblUseScroll 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Enter a voltage within the DACs range:"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   240
      TabIndex        =   2
      Top             =   1560
      Width           =   3855
   End
   Begin VB.Label lblDemoFunction 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Demonstration of cbAOut()"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   120
      TabIndex        =   1
      Top             =   120
      Width           =   5775
   End
End
Attribute VB_Name = "frmSendAData"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
'ULAO01.VBP================================================================

' File:                         ULAO01.VBP

' Library Call Demonstrated:    cbAOut&()

' Purpose:                      Writes to a D/A Output Channel.

' Demonstration:                Sends a digital output to D/A 0.

' Other Library Calls:          cbErrHandling&()

' Special Requirements:         Board 0 must have a D/A converter.

' (c) Copyright 2005-2011, Measurement Computing Corp.
' All rights reserved.
'==========================================================================
Option Explicit

Const BoardNum As Long = 0          ' Board number

Dim CBRange As Long
Dim DAResolution As Long, NumAOChans As Long
Dim LowChan As Long, MaxChan As Long
Dim ULStat As Long

Private Sub Form_Load()

   Dim DefaultTrig As Long, ChannelType As Long
   Dim ReportError As Long, HandleError As Long
   
   ' declare revision level of Universal Library

   ULStat& = cbDeclareRevision(CURRENTREVNUM)
   
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
   If ULStat& <> 0 Then Stop
   SetAnalogIODefaults ReportError, HandleError

   ' If cbErrHandling& is set for STOPALL or STOPFATAL during the program
   ' design stage, Visual Basic will be unloaded when an error is encountered.
   ' We suggest trapping errors locally until the program is ready for compiling
   ' to avoid losing unsaved data during program design.  This can be done by
   ' setting cbErrHandling options as above and checking the value of ULStat&
   ' after a call to the library. If it is not equal to 0, an error has occurred.

   ' determine the number of analog channels and their capabilities
   ChannelType = ANALOGOUTPUT
   NumAOChans = FindAnalogChansOfType(BoardNum, ChannelType, _
       DAResolution, CBRange, LowChan, DefaultTrig)

   If (NumAOChans = 0) Then
       lblInstruction.Caption = "Board " & Format(BoardNum, "0") & _
       " does not have analog output channels."
       txtVoltsToSet.Enabled = False
   Else
       lblInstruction.Caption = "Board " & Format(BoardNum, "0") & _
           " generating analog output on channel 0 using cbAOut() " & _
           " at a Range of " & GetRangeString(CBRange) & "."
   End If

End Sub

Private Sub txtVoltsToSet_Change()

   Dim EngUnits As Single, OutVal As Single
   Dim DataValue As Integer, Chan As Long
   
   ' send the digital output value to D/A 0 with cbAOut&()
   EngUnits! = Val(txtVoltsToSet.Text)

   ' Parameters:
   '   BoardNum    :the number used by CB.CFG to describe this board
   '   Chan&       :the D/A output channel
   '   CBRange      :ignored if board does not have programmable rage
   '   DataValue%  :the value to send to Chan&
   
   
   ULStat& = cbFromEngUnits(BoardNum&, CBRange, EngUnits!, DataValue%)
   If ULStat& <> 0 Then Stop
         
   ULStat& = cbAOut(BoardNum&, Chan&, CBRange, DataValue%)
   If ULStat& <> 0 Then Stop
  
   lblValueSent.Caption = "The count sent to DAC channel " + Format$(Chan&, "0") + " was:"
   lblVoltage.Caption = "The voltage at DAC channel " + Format$(Chan&, "0") + " is:"
   lblShowValue.Caption = Format$(DataValue%, "0")
   OutVal! = ConvertToVolts(DataValue%)
   lblShowVoltage.Caption = Format$(OutVal!, "0.0#####") + " Volts"

End Sub

Private Function ConvertToVolts(ByVal DataVal As Integer) As Single

   Dim LSBVal As Single, FSVolts As Single, OutVal As Single
   Dim LongVal As Long

   FSVolts! = GetRangeVolts(CBRange)
   LSBVal! = FSVolts! / 2 ^ DAResolution
   If DataVal < 0 Then
      LongVal& = DataVal + 65536
   Else
      LongVal& = DataVal
   End If
   OutVal! = LSBVal! * LongVal&
   If CBRange < 100 Then OutVal! = OutVal! - (FSVolts! / 2)
   ConvertToVolts = OutVal!
   
End Function

Private Sub cmdEndProgram_Click()
   
   End

End Sub


