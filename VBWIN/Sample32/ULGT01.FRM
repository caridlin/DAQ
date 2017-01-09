VERSION 5.00
Begin VB.Form frmShowError 
   Appearance      =   0  'Flat
   BackColor       =   &H80000005&
   Caption         =   "Universal Library Error Handling"
   ClientHeight    =   3855
   ClientLeft      =   1095
   ClientTop       =   1485
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
   ScaleHeight     =   3855
   ScaleWidth      =   4965
   Begin VB.CommandButton cmdQuit 
      Appearance      =   0  'Flat
      Caption         =   "Quit"
      Enabled         =   0   'False
      Height          =   375
      Left            =   3600
      TabIndex        =   5
      Top             =   3120
      Visible         =   0   'False
      Width           =   855
   End
   Begin VB.CommandButton cmdCallFunction 
      Appearance      =   0  'Flat
      Caption         =   "Start"
      Default         =   -1  'True
      Height          =   375
      Left            =   3600
      TabIndex        =   1
      Top             =   3120
      Width           =   855
   End
   Begin VB.Label lblShowErrMsg 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Error message Shown here."
      ForeColor       =   &H00808080&
      Height          =   855
      Left            =   720
      TabIndex        =   3
      Top             =   1920
      Width           =   3495
   End
   Begin VB.Label lblShowErrCode 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H000000FF&
      Height          =   255
      Left            =   3240
      TabIndex        =   2
      Top             =   960
      Width           =   615
   End
   Begin VB.Label lblErrorCode 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Error message code:"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   1320
      TabIndex        =   4
      Top             =   960
      Width           =   1815
   End
   Begin VB.Label lblDemoFunction 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Demonstration of cbGetErrMsg()"
      ForeColor       =   &H80000008&
      Height          =   375
      Left            =   120
      TabIndex        =   0
      Top             =   240
      Width           =   4695
   End
End
Attribute VB_Name = "frmShowError"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
'ULGT01.VBP================================================================

' File:                         ULGT01.VBP

' Library Call Demonstrated:    cbGetErrMsg()

' Purpose:                      Returns the error message associated
'                               with an error code.

' Demonstration:                An invalid chan # is generated and the
'                               associated message is retrieved.

' Other Library Calls:          cbErrHandling()


' (c) Copyright 2005-2011, Measurement Computing Corp.
' All rights reserved.
'==========================================================================
Option Explicit

Dim ULStat As Long

Private Sub Form_Load()
   
   ' declare revision level of Universal Library

   ULStat = cbDeclareRevision(CURRENTREVNUM)

   ' Disable external error handling to demonstrate internal handling
   
   ULStat = cbErrHandling(DONTPRINT, DONTSTOP)
   If ULStat <> 0 Then Stop

End Sub

Private Sub cmdCallFunction_Click()

   Dim NullLocation As Long
   Dim BoardNum As Long, Chan As Long, Gain As Long
   Dim DataValue As Integer, ErrCode As Long
   Dim ErrMessage As String
  
   cmdCallFunction.Visible = 0
   cmdCallFunction.Enabled = 0
   cmdQuit.Visible = 1
   cmdQuit.Enabled = 1

   ' generate an error by passing an invalid variable to a routine

   BoardNum = 0
   Gain = BIP5VOLTS
   Chan = -1
   ErrCode = cbAIn(BoardNum, Chan, Gain, DataValue)

   ' trap the error and determine the message to print
   '  Parameters:
   '    ErrCode    :the error code returned by the invalid call statement
   '    ErrMessage$ :the text of the error message associated with ErrCode
   '                 must be dimensioned to at least the length of the
   '                 longest message using ERRSTRLEN
   
   ErrMessage$ = Space$(ERRSTRLEN)     ' fill ErrMessage$ with spaces

   ULStat = cbGetErrMsg(ErrCode, ErrMessage$)
   If ULStat <> 0 Then End
  
   'ErrMessage$ string is returned with a null terminator.
   'This should be removed to display properly.
   NullLocation = InStr(1, ErrMessage$, Chr(0))
   ErrMessage$ = Left(ErrMessage$, NullLocation - 1)
   
   Beep
   lblShowErrMsg.ForeColor = &HFF&
   lblShowErrCode.Caption = Format$(ErrCode, "0")
   lblShowErrMsg.Caption = ErrMessage$ & "."

End Sub

Private Sub cmdQuit_Click()

   End

End Sub

