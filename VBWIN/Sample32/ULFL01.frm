VERSION 5.00
Begin VB.Form frmLEDTest 
   Caption         =   "Universal Library LED Test"
   ClientHeight    =   2160
   ClientLeft      =   1140
   ClientTop       =   1380
   ClientWidth     =   5025
   LinkTopic       =   "Form1"
   PaletteMode     =   1  'UseZOrder
   ScaleHeight     =   2160
   ScaleWidth      =   5025
   Begin VB.CommandButton cmdFlashBtn 
      Caption         =   "Flash LED"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   555
      Left            =   1440
      TabIndex        =   0
      Top             =   240
      Width           =   1995
   End
   Begin VB.Label lblResult 
      Alignment       =   2  'Center
      ForeColor       =   &H00FF0000&
      Height          =   735
      Left            =   360
      TabIndex        =   1
      Top             =   1080
      Width           =   4335
   End
End
Attribute VB_Name = "frmLEDTest"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False

'ULFL01================================================================

' File:                         ULFL01.FRM

' Library Call Demonstrated:    cbFlashLED()

' Purpose:                      Flashes onboard LED for visual identification.

' Other Library Calls:          cbErrHandling()

' Special Requirements:         Board 0 must have an external LED,
'                               such as the miniLAB 1008 and PMD-1208LS.
'

' (c) Copyright 2005-2011, Measurement Computing Corp.
' All rights reserved.
'==========================================================================
Option Explicit

Const BoardNum As Long = 0      ' Board number
Dim ULStat As Long

Sub Form_Load()

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

End Sub

Private Sub cmdFlashBtn_Click()

   Dim ULStat As Long
   
   lblResult.Caption = "The LED on device " & _
   Format(BoardNum, "0") & " should flash several times."
   DoEvents
   
   ULStat = cbFlashLED(BoardNum)
   If Not (ULStat = 0) Then
      lblResult.Caption = ""
      Stop
   End If

End Sub
