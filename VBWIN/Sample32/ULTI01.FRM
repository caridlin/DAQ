VERSION 5.00
Begin VB.Form frmDataDisplay 
   Appearance      =   0  'Flat
   BackColor       =   &H80000005&
   Caption         =   "Universal Library Temperature Measurement"
   ClientHeight    =   2850
   ClientLeft      =   105
   ClientTop       =   1545
   ClientWidth     =   5400
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
   ScaleHeight     =   2850
   ScaleWidth      =   5400
   Begin VB.CommandButton cmdStopConvert 
      Appearance      =   0  'Flat
      Caption         =   "Quit"
      Height          =   375
      Left            =   4320
      TabIndex        =   6
      Top             =   2280
      Visible         =   0   'False
      Width           =   855
   End
   Begin VB.CommandButton cmdStartConvert 
      Appearance      =   0  'Flat
      Caption         =   "Start"
      Default         =   -1  'True
      Height          =   375
      Left            =   4320
      TabIndex        =   5
      Top             =   2280
      Width           =   855
   End
   Begin VB.TextBox txtExpChan 
      Appearance      =   0  'Flat
      Height          =   375
      Left            =   4680
      TabIndex        =   0
      Text            =   "0"
      Top             =   840
      Width           =   495
   End
   Begin VB.Timer tmrConvert 
      Enabled         =   0   'False
      Interval        =   250
      Left            =   4800
      Top             =   240
   End
   Begin VB.Label lblShowData 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Left            =   4200
      TabIndex        =   4
      Top             =   1560
      Width           =   975
   End
   Begin VB.Label lblValueRead 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Temperature read from selected channel:"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   360
      TabIndex        =   3
      Top             =   1560
      Width           =   3615
   End
   Begin VB.Label lblChanPrompt 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Enter the multiplexor channel to display (0 to 15): "
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   120
      TabIndex        =   1
      Top             =   960
      Width           =   4455
   End
   Begin VB.Label lblDemoFunction 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Demonstration of cbTIn()"
      ForeColor       =   &H80000008&
      Height          =   375
      Left            =   600
      TabIndex        =   2
      Top             =   240
      Width           =   4095
   End
End
Attribute VB_Name = "frmDataDisplay"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
'ULTI01.VBP================================================================

' File:                         ULTI01.VBP

' Library Call Demonstrated:    cbTIn()

' Purpose:                      Reads a temperature input channel.

' Demonstration:                Displays the temperature input.

' Other Library Calls:          cbErrHandling()

' Special Requirements:         Unless the board at BoardNum(=0) does not use
'                               EXP boards for temperature measurements(the
'                               CIO-DAS-TC or USB-2001-TC for example), it must
'                               have an A/D converter with an attached EXP
'                               board.  Thermocouples must be wired to EXP
'                               channels selected.

' (c) Copyright 2005-2011, Measurement Computing Corp.
' All rights reserved.
'==========================================================================
Option Explicit

Const BoardNum As Long = 0              ' Board number
Dim UsesEXPs As Long
Dim ULStat As Long

Private Sub Form_Load()

   Dim InfoType As Long
   Dim DevNum As Long, ConfigItem As Long, UsesEXPs As Long
   
   ' declare revision level of Universal Library

   ULStat = cbDeclareRevision(CURRENTREVNUM)
   
   ' Initiate error handling
   '  activating error handling will trap errors like
   '  bad channel numbers and non-configured conditions.
   '  Parameters:
   '    PRINTALL    :all warnings and errors encountered will be printed
   '    DONTSTOP    :if an error is encountered, the program will not stop,
   '                 errors must be handled locally

   ULStat = cbErrHandling(PRINTALL, DONTSTOP)
   If ULStat <> 0 Then Stop

   ' If cbErrHandling is set for STOPALL or STOPFATAL during the program
   ' design stage, Visual Basic will be unloaded when an error is encountered.
   ' We suggest trapping errors locally until the program is ready for compiling
   ' to avoid losing unsaved data during program design.  This can be done by
   ' setting cbErrHandling options as above and checking the value of ULStat
   ' after a call to the library. If it is not equal to 0, an error has occurred.

   ' Determine if the board uses EXP boards for temperature measurements
   InfoType = BOARDINFO
   DevNum = 0
   ConfigItem = BIUSESEXPS
   UsesEXPs = 0
   ULStat = cbGetConfig(InfoType, BoardNum, DevNum, ConfigItem, UsesEXPs)
   If (ULStat <> 0) Then UsesEXPs = 0

End Sub

Private Sub tmrConvert_Timer()

   Dim Chan As Long, ADChan As Long
   Dim CBScale As Long, Options As Long
   Dim TempValue As Single
   
   ' Collect the data with cbTIn()
    
   ' Parameters:
   '   Boardnum   :the number used by CB.CFG to describe this board
   '   Chan       :the A/D and channel number; starts at 16 if using
   '               an EXP (calculated by (ADChan + 1) * 16 + EXPChan)
   '   CBScale    :the temperature scale (F, C or K)
   '   DataValue  :the name for the value collected
   '   ADChan     :usually channel 0 for CIO-EXP16

   ADChan = 0         ' If using a device with an EXP attached,
                      ' allows access to 16 channels on the EXP board
                      ' increasing this number allows access to upper
                      ' bank on EXP32 and additional EXP boards
    
   CBScale = CELSIUS
   If UsesEXPs > 0 Then
        Chan = (ADChan + 1) * 16 + Val(txtExpChan.Text)
   Else
     Chan = Val(txtExpChan.Text)
   End If
   Options = FILTER
    
   ULStat = cbTIn(BoardNum, Chan, CBScale, TempValue!, Options)
   If ULStat <> 0 Then Stop

   lblShowData.Caption = Format$(TempValue!, "0\°\C")       ' print the value

End Sub

Private Sub cmdStartConvert_Click()

   If Val(txtExpChan.Text) < 0 Then txtExpChan.Text = "0"
   If Val(txtExpChan.Text) > 15 Then txtExpChan.Text = "15"

   cmdStartConvert.Visible = 0
   cmdStartConvert.Default = 0
   cmdStopConvert.Visible = 1
   cmdStopConvert.Default = 1
   tmrConvert.Enabled = 1

End Sub

Private Sub txtExpChan_Change()

   tmrConvert.Enabled = 0
   cmdStartConvert.Visible = 1
   cmdStartConvert.Default = 1
   cmdStopConvert.Visible = 0
   cmdStopConvert.Default = 0
  
End Sub

Private Sub cmdStopConvert_Click()
   
   End

End Sub

