VERSION 5.00
Begin VB.Form frmFilePreTrig 
   Appearance      =   0  'Flat
   BackColor       =   &H80000005&
   Caption         =   "Universal Library Analog Input to File"
   ClientHeight    =   6585
   ClientLeft      =   105
   ClientTop       =   1545
   ClientWidth     =   5850
   BeginProperty Font 
      Name            =   "MS Sans Serif"
      Size            =   8.25
      Charset         =   0
      Weight          =   700
      Underline       =   0   'False
      Italic          =   0   'False
      Strikethrough   =   0   'False
   EndProperty
   ForeColor       =   &H00FF0000&
   LinkMode        =   1  'Source
   LinkTopic       =   "Form1"
   PaletteMode     =   1  'UseZOrder
   ScaleHeight     =   6585
   ScaleWidth      =   5850
   Begin VB.TextBox txtFileName 
      Appearance      =   0  'Flat
      Height          =   315
      Left            =   3300
      TabIndex        =   50
      Text            =   "DEMO.DAT"
      Top             =   5880
      Width           =   2415
   End
   Begin VB.CommandButton cmdTrigEnable 
      Appearance      =   0  'Flat
      Caption         =   "Enable Trigger"
      Height          =   345
      Left            =   4020
      TabIndex        =   18
      Top             =   4800
      Width           =   1695
   End
   Begin VB.CommandButton cmdQuit 
      Appearance      =   0  'Flat
      Caption         =   "Quit"
      Height          =   345
      Left            =   4020
      TabIndex        =   17
      Top             =   5280
      Width           =   1680
   End
   Begin VB.Label lblPreTrig 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   195
      Index           =   19
      Left            =   4200
      TabIndex        =   61
      Top             =   3900
      Width           =   1395
   End
   Begin VB.Label lblPreTrig 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   195
      Index           =   18
      Left            =   4200
      TabIndex        =   60
      Top             =   3660
      Width           =   1395
   End
   Begin VB.Label lblPreTrig 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   195
      Index           =   17
      Left            =   4200
      TabIndex        =   59
      Top             =   3420
      Width           =   1395
   End
   Begin VB.Label lblPreTrig 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   195
      Index           =   16
      Left            =   4200
      TabIndex        =   58
      Top             =   3180
      Width           =   1395
   End
   Begin VB.Label lblPreTrig 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   195
      Index           =   15
      Left            =   4200
      TabIndex        =   57
      Top             =   2940
      Width           =   1395
   End
   Begin VB.Label lblPreTrig 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   195
      Index           =   14
      Left            =   4200
      TabIndex        =   56
      Top             =   2700
      Width           =   1395
   End
   Begin VB.Label lblPreTrig 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   195
      Index           =   13
      Left            =   4200
      TabIndex        =   55
      Top             =   2460
      Width           =   1395
   End
   Begin VB.Label lblPreTrig 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   195
      Index           =   12
      Left            =   4200
      TabIndex        =   54
      Top             =   2220
      Width           =   1395
   End
   Begin VB.Label lblPreTrig 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   195
      Index           =   11
      Left            =   4200
      TabIndex        =   53
      Top             =   1980
      Width           =   1395
   End
   Begin VB.Label lblPreTrig 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   195
      Index           =   10
      Left            =   4200
      TabIndex        =   52
      Top             =   1740
      Width           =   1395
   End
   Begin VB.Label lblInstruction 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Board 0 must have analog inputs that support paced acquisition."
      ForeColor       =   &H00C00000&
      Height          =   675
      Left            =   360
      TabIndex        =   51
      Top             =   480
      Width           =   5055
   End
   Begin VB.Label lblFileInstruct 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Enter the name of the file in which to store the data."
      ForeColor       =   &H80000008&
      Height          =   615
      Left            =   420
      TabIndex        =   49
      Top             =   5820
      Width           =   2655
   End
   Begin VB.Label lblShowGain 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   210
      Left            =   2715
      TabIndex        =   48
      Top             =   5430
      Width           =   780
   End
   Begin VB.Label lblGain 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Gain:"
      ForeColor       =   &H80000008&
      Height          =   210
      Left            =   690
      TabIndex        =   41
      Top             =   5430
      Width           =   1935
   End
   Begin VB.Label lblShowRate 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   210
      Left            =   2715
      TabIndex        =   47
      Top             =   5235
      Width           =   780
   End
   Begin VB.Label lblRate 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Collection Rate:"
      ForeColor       =   &H80000008&
      Height          =   210
      Left            =   690
      TabIndex        =   40
      Top             =   5235
      Width           =   1935
   End
   Begin VB.Label lblShowNumSam 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   210
      Left            =   2715
      TabIndex        =   46
      Top             =   5040
      Width           =   780
   End
   Begin VB.Label lblNumSam 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "No. of Samples:"
      ForeColor       =   &H80000008&
      Height          =   210
      Left            =   690
      TabIndex        =   39
      Top             =   5040
      Width           =   1935
   End
   Begin VB.Label lblShowPT 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   210
      Left            =   2715
      TabIndex        =   45
      Top             =   4845
      Width           =   780
   End
   Begin VB.Label lblNumPTSam 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "No. of Pretrig Samples:"
      ForeColor       =   &H80000008&
      Height          =   210
      Left            =   450
      TabIndex        =   38
      Top             =   4845
      Width           =   2175
   End
   Begin VB.Label lblShowHiChan 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   210
      Left            =   2715
      TabIndex        =   44
      Top             =   4650
      Width           =   780
   End
   Begin VB.Label lblHiChan 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "High Channel:"
      ForeColor       =   &H80000008&
      Height          =   210
      Left            =   690
      TabIndex        =   37
      Top             =   4650
      Width           =   1935
   End
   Begin VB.Label lblShowLoChan 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   210
      Left            =   2715
      TabIndex        =   43
      Top             =   4470
      Width           =   780
   End
   Begin VB.Label lblLoChan 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Low Channel:"
      ForeColor       =   &H80000008&
      Height          =   210
      Left            =   690
      TabIndex        =   36
      Top             =   4470
      Width           =   1935
   End
   Begin VB.Label lblShowFileName 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   210
      Left            =   2715
      TabIndex        =   42
      Top             =   4275
      Width           =   2745
   End
   Begin VB.Label lblFileName 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Streamer File Name:"
      ForeColor       =   &H80000008&
      Height          =   210
      Left            =   690
      TabIndex        =   35
      Top             =   4275
      Width           =   1935
   End
   Begin VB.Label lblPre 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Trigger +9"
      ForeColor       =   &H00FF0000&
      Height          =   195
      Index           =   19
      Left            =   3000
      TabIndex        =   32
      Top             =   3900
      Width           =   1095
   End
   Begin VB.Label lblPreTrig 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   195
      Index           =   9
      Left            =   1380
      TabIndex        =   22
      Top             =   3900
      Width           =   1395
   End
   Begin VB.Label lblPre 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Trigger -1"
      ForeColor       =   &H00FF0000&
      Height          =   195
      Index           =   9
      Left            =   180
      TabIndex        =   20
      Top             =   3900
      Width           =   1095
   End
   Begin VB.Label lblPre 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Trigger +8"
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   18
      Left            =   3000
      TabIndex        =   31
      Top             =   3660
      Width           =   1095
   End
   Begin VB.Label lblPreTrig 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   195
      Index           =   8
      Left            =   1380
      TabIndex        =   21
      Top             =   3660
      Width           =   1395
   End
   Begin VB.Label lblPre 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Trigger -2"
      ForeColor       =   &H00FF0000&
      Height          =   195
      Index           =   8
      Left            =   180
      TabIndex        =   19
      Top             =   3660
      Width           =   1095
   End
   Begin VB.Label lblPre 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Trigger +7"
      ForeColor       =   &H00FF0000&
      Height          =   195
      Index           =   17
      Left            =   3000
      TabIndex        =   30
      Top             =   3420
      Width           =   1095
   End
   Begin VB.Label lblPreTrig 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   195
      Index           =   7
      Left            =   1380
      TabIndex        =   16
      Top             =   3420
      Width           =   1395
   End
   Begin VB.Label lblPre 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Trigger -3"
      ForeColor       =   &H00FF0000&
      Height          =   195
      Index           =   7
      Left            =   180
      TabIndex        =   8
      Top             =   3420
      Width           =   1095
   End
   Begin VB.Label lblPre 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Trigger +6"
      ForeColor       =   &H00FF0000&
      Height          =   210
      Index           =   16
      Left            =   3000
      TabIndex        =   28
      Top             =   3180
      Width           =   1095
   End
   Begin VB.Label lblPreTrig 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   195
      Index           =   6
      Left            =   1380
      TabIndex        =   15
      Top             =   3180
      Width           =   1395
   End
   Begin VB.Label lblPre 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Trigger -4"
      ForeColor       =   &H00FF0000&
      Height          =   195
      Index           =   6
      Left            =   180
      TabIndex        =   7
      Top             =   3180
      Width           =   1095
   End
   Begin VB.Label lblPre 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Trigger +5"
      ForeColor       =   &H00FF0000&
      Height          =   195
      Index           =   15
      Left            =   3000
      TabIndex        =   26
      Top             =   2940
      Width           =   1095
   End
   Begin VB.Label lblPreTrig 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   195
      Index           =   5
      Left            =   1380
      TabIndex        =   14
      Top             =   2940
      Width           =   1395
   End
   Begin VB.Label lblPre 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Trigger -5"
      ForeColor       =   &H00FF0000&
      Height          =   195
      Index           =   5
      Left            =   180
      TabIndex        =   6
      Top             =   2940
      Width           =   1095
   End
   Begin VB.Label lblPre 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Trigger +4"
      ForeColor       =   &H00FF0000&
      Height          =   195
      Index           =   14
      Left            =   3000
      TabIndex        =   24
      Top             =   2700
      Width           =   1095
   End
   Begin VB.Label lblPreTrig 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   195
      Index           =   4
      Left            =   1380
      TabIndex        =   13
      Top             =   2700
      Width           =   1395
   End
   Begin VB.Label lblPre 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Trigger -6"
      ForeColor       =   &H00FF0000&
      Height          =   195
      Index           =   4
      Left            =   180
      TabIndex        =   5
      Top             =   2700
      Width           =   1095
   End
   Begin VB.Label lblPre 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Trigger +3"
      ForeColor       =   &H00FF0000&
      Height          =   195
      Index           =   13
      Left            =   3000
      TabIndex        =   29
      Top             =   2460
      Width           =   1095
   End
   Begin VB.Label lblPreTrig 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   195
      Index           =   3
      Left            =   1380
      TabIndex        =   12
      Top             =   2460
      Width           =   1395
   End
   Begin VB.Label lblPre 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Trigger -7"
      ForeColor       =   &H00FF0000&
      Height          =   195
      Index           =   3
      Left            =   180
      TabIndex        =   4
      Top             =   2460
      Width           =   1095
   End
   Begin VB.Label lblPre 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Trigger +2"
      ForeColor       =   &H00FF0000&
      Height          =   195
      Index           =   12
      Left            =   3000
      TabIndex        =   27
      Top             =   2220
      Width           =   1095
   End
   Begin VB.Label lblPreTrig 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   195
      Index           =   2
      Left            =   1380
      TabIndex        =   11
      Top             =   2220
      Width           =   1395
   End
   Begin VB.Label lblPre 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Trigger -8"
      ForeColor       =   &H00FF0000&
      Height          =   195
      Index           =   2
      Left            =   180
      TabIndex        =   3
      Top             =   2220
      Width           =   1095
   End
   Begin VB.Label lblPre 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Trigger +1"
      ForeColor       =   &H00FF0000&
      Height          =   195
      Index           =   11
      Left            =   3000
      TabIndex        =   25
      Top             =   1980
      Width           =   1095
   End
   Begin VB.Label lblPreTrig 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   195
      Index           =   1
      Left            =   1380
      TabIndex        =   10
      Top             =   1980
      Width           =   1395
   End
   Begin VB.Label lblPre 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Trigger -9"
      ForeColor       =   &H00FF0000&
      Height          =   195
      Index           =   1
      Left            =   180
      TabIndex        =   2
      Top             =   1980
      Width           =   1095
   End
   Begin VB.Label lblPre 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Trigger"
      ForeColor       =   &H00FF0000&
      Height          =   195
      Index           =   10
      Left            =   3000
      TabIndex        =   23
      Top             =   1740
      Width           =   1095
   End
   Begin VB.Label lblPreTrig 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   195
      Index           =   0
      Left            =   1380
      TabIndex        =   9
      Top             =   1740
      Width           =   1395
   End
   Begin VB.Label lblPre 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Trigger -10"
      ForeColor       =   &H00FF0000&
      Height          =   195
      Index           =   0
      Left            =   180
      TabIndex        =   1
      Top             =   1740
      Width           =   1095
   End
   Begin VB.Label lblPostTrigData 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Data acquired after trigger"
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
      Height          =   330
      Left            =   3000
      TabIndex        =   34
      Top             =   1335
      Width           =   2640
   End
   Begin VB.Label lblPreTrigData 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Data acquired before trigger"
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
      Height          =   330
      Left            =   75
      TabIndex        =   33
      Top             =   1335
      Width           =   2715
   End
   Begin VB.Label lblDemoFunction 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Demonstration of cbFilePretrig()"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   330
      TabIndex        =   0
      Top             =   90
      Width           =   5175
   End
End
Attribute VB_Name = "frmFilePreTrig"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
'ULFI03================================================================

' File:                         ULFI03.FRM

' Library Call Demonstrated:    File Operations:
'                               cbFilePretrig()
'                               cbFileRead()
'                               cbFileGetInfo()

' Purpose:                      Stream data continuously to a streamer file
'                               until a trigger is received, continue data
'                               streaming until total number of samples minus
'                               the number of pretrigger samples is reached.

' Demonstration:                Creates a file and scans analog data to the
'                               file continuously, overwriting previous data.
'                               When a trigger is received, acquisition stops
'                               after (TotalCount& - PreTrigCount&) samples
'                               are stored. Displays the data in the file and
'                               the information in the file header. Prints
'                               data from PreTrigger-10 to PreTrigger+10.

' Other Library Calls:          cbErrHandling()

' Special Requirements:         Board 0 must have an A/D converter and support cbFilePretrig.
'                               Analog signal on an input channel.

' (c) Copyright 2005-2011, Measurement Computing Corp.
' All rights reserved.
'==========================================================================
Option Explicit

Const BoardNum As Long = 0          ' Board number

Dim CBRange As Long
Dim ADResolution As Long, NumAIChans As Long
Dim HighChan As Long, LowChan As Long
Dim DefaultTrig As Long

Const TestPoints As Long = 4096   ' Number of data points to collect
Dim DataBuffer() As Integer
Dim ULStat As Long

Private Sub Form_Load()

   ' declare revision level of Universal Library

   ULStat = cbDeclareRevision(CURRENTREVNUM)
   
   ' Initiate error handling
   ' activating error handling will trap errors like
   ' bad channel numbers and non-configured conditions.
   ' Parameters:
   '   PRINTALL    :all warnings and errors encountered will be handled locally
   '   DONTSTOP    :if an error is encountered, the program will not stop,
   '                errors must be handled locally
     
   ULStat = cbErrHandling(DONTPRINT, DONTSTOP)
   If ULStat <> 0 Then Stop
   
   ' If cbErrHandling is set for STOPALL or STOPFATAL during the program
   ' design stage, Visual Basic will be unloaded when an error is encountered.
   ' We suggest trapping errors locally until the program is ready for compiling
   ' to avoid losing unsaved data during program design.  This can be done by
   ' setting cbErrHandling options as above and checking the value of ULStat
   ' after a call to the library. If it is not equal to 0, an error has occurred.

   ' determine the number of analog channels and their capabilities
   Dim ChannelType As Long
   ChannelType = PRETRIGIN
   NumAIChans = FindAnalogChansOfType(BoardNum, ChannelType, _
      ADResolution, CBRange, LowChan, DefaultTrig)

   If (NumAIChans = 0) Then
      lblInstruction.Caption = "Board " & Format(BoardNum, "0") & _
      " does not have analog input channels that support pretrigger."
      cmdTrigEnable.Enabled = False
   Else
       lblInstruction.Caption = "Board " & Format(BoardNum, "0") & _
           " collecting analog data on channel 0 using cbFilePretrig in " & _
           "foreground mode with Range set to " & GetRangeString(CBRange) & "."
   End If

End Sub

Private Sub cmdTrigEnable_Click()

   Dim FileLowChan As Integer, FileHighChan As Integer
   Dim DataAvailable As Boolean, EngUnits As Single
   Dim PretrigCount As Long, TotalCount As Long, NumPoints As Long
   Dim CBRate As Long, Filename As String, Options As Long
   Dim FirstPoint As Long
   
   lblInstruction.Caption = _
      "Waiting for trigger on trigger input and acquiring data."
   cmdTrigEnable.Enabled = False
   MousePointer = vbHourglass
   DoEvents
   DataAvailable = False

   ' Monitor a range of channels for a trigger then collect the values
   ' with cbAPretrig()
   ' Parameters:
   '   BoardNum       :the number used by CB.CFG to describe this board
   '   FileName       :file where data will be stored
   '   LowChan        :first A/D channel of the scan
   '   HighChan       :last A/D channel of the scan
   '   PretrigCount&  :number of pre-trigger A/D samples to collect
   '   TotalCount&    :total number of A/D samples to collect
   '   CBRate&        :sample rate in samples per second
   '   CBRange        :the gain for the board
   '   Options        :data collection options

   TotalCount& = TestPoints&
   PretrigCount& = 200
   Filename$ = txtFileName.Text   ' it may be necessary to specify path here
   CBRate& = 1000
   FileLowChan = LowChan
   FileHighChan = LowChan
   HighChan = LowChan
   Options = DEFAULTOPTION
 
   If DefaultTrig = TRIGABOVE Then
       'The default trigger configuration for most devices is
       'rising edge digital trigger, but some devices do not
       'support this type for pretrigger functions.
       Dim MidScale As Integer
       MidScale = ((2 ^ ADResolution) / 2) - 1
       ULStat = cbSetTrigger(BoardNum, DefaultTrig, MidScale, MidScale)
       ULStat = cbToEngUnits(BoardNum, CBRange, MidScale, EngUnits)
       lblInstruction.Caption = "Waiting for trigger on analog input above " _
           & Format(EngUnits, "0.00") & "V."
   End If
   
   ULStat = cbFilePretrig(BoardNum, LowChan, HighChan, _
      PretrigCount&, TotalCount&, CBRate&, CBRange, Filename$, Options)
   MousePointer = MousePointerConstants.vbDefault
   
   If ULStat = 502 Then
      MsgBox "Enter the name of the file in which to store " & _
         "the data in the text box.", 0, "Bad File Name"
      cmdTrigEnable.Enabled = True
      txtFileName.SetFocus
      Exit Sub
   End If

   If ULStat = BADBOARDTYPE Then
      lblInstruction.Caption = "Board " & Format(BoardNum, "0") & _
      " doesn't support the cbAPretrig function."
      lblInstruction.ForeColor = &HFF&
      Exit Sub
   ElseIf ULStat = TOOFEW Then
       lblInstruction.Caption = "Premature trigger occurred at sample " _
       & Format(PretrigCount& - 1, "0") & "."
       DataAvailable = True
   ElseIf ULStat <> 0 Then
       lblInstruction.Caption = "Error " & Format(ULStat, "0") & " occurred."
       DoEvents
       Stop
   Else
       lblInstruction.Caption = ""
       DataAvailable = True
   End If
   
   ' show the information in the file header with cbFileGetInfo()
   '  Parameters:
   '    FileName$      :the filename containing the data
   '    LowChan        :first A/D channel of the scan
   '    HighChan       :last A/D channel of the scan
   '    PreTrigCount&  :the number of pretrigger samples in the file
   '    CBCount&       :the total number of A/D samples in the file
   '    CBRate&        :sample rate in samples per second
   '    CBRange        :the gain at which the samples were collected

   ULStat = cbFileGetInfo(Filename$, FileLowChan, FileHighChan, _
      PretrigCount&, TotalCount&, CBRate&, CBRange&)
   If ULStat <> 0 Then Stop

   lblShowFileName.Caption = Filename$
   lblShowLoChan.Caption = Format$(LowChan, "0")
   lblShowHiChan.Caption = Format$(HighChan, "0")
   lblShowPT.Caption = Format$(PretrigCount&, "0")
   lblShowNumSam.Caption = Format$(TotalCount&, "0")
   lblShowRate.Caption = Format$(CBRate&, "0")
   lblShowGain.Caption = Format$(CBRange&, "0")
 
   ' show the data using cbFileRead()
   '  Parameters:
   '    FileName$      :the filename containing the data
   '    NumPoints&     :the number of data values to read from the file
   '    FirstPoint&    :index of the first data value to read
   '    DataBuffer()   :array to read data into

   NumPoints& = 20                   ' read the first twenty data points
   FirstPoint& = PretrigCount& - 11  ' start at the trigger - 10
   If FirstPoint& < 0 Then FirstPoint& = 0
   ReDim DataBuffer(NumPoints&)
 
   ULStat = cbFileRead(Filename$, FirstPoint&, NumPoints&, DataBuffer(0))
   If ULStat <> 0 Then Stop
   
   Dim i As Integer
   For i = 0 To 19
      lblPreTrig(i).Caption = Format$(DataBuffer(i), "0")
      Me.lblPre(i).Caption = Format$(FirstPoint& + i, "0")
   Next i
   cmdTrigEnable.Enabled = True

End Sub

Private Sub cmdQuit_Click()
   
   End

End Sub


