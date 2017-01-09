VERSION 5.00
Begin VB.Form frmDataDisplay 
   Appearance      =   0  'Flat
   BackColor       =   &H80000005&
   Caption         =   "Universal Library Analog Input to File"
   ClientHeight    =   5205
   ClientLeft      =   105
   ClientTop       =   1545
   ClientWidth     =   6270
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
   ScaleHeight     =   5205
   ScaleWidth      =   6270
   Begin VB.CommandButton cmdStartAcq 
      Appearance      =   0  'Flat
      Caption         =   "Start"
      Height          =   390
      Left            =   4440
      TabIndex        =   2
      Top             =   4680
      Width           =   690
   End
   Begin VB.TextBox txtFileName 
      Appearance      =   0  'Flat
      Height          =   375
      Left            =   3000
      TabIndex        =   30
      Text            =   "DEMO.DAT"
      Top             =   4080
      Width           =   2415
   End
   Begin VB.CommandButton cmdStopConvert 
      Appearance      =   0  'Flat
      Caption         =   "Quit"
      Height          =   390
      Left            =   5400
      TabIndex        =   1
      Top             =   4680
      Width           =   690
   End
   Begin VB.Label lblFileInstruct 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Enter the name of the file in which to store the data."
      ForeColor       =   &H80000008&
      Height          =   615
      Left            =   240
      TabIndex        =   31
      Top             =   4080
      Width           =   2535
   End
   Begin VB.Label lblReadFile 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   270
      Left            =   3960
      TabIndex        =   18
      Top             =   3600
      Width           =   1785
   End
   Begin VB.Label lblShowFile 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   270
      Left            =   1800
      TabIndex        =   9
      Top             =   3600
      Width           =   1665
   End
   Begin VB.Label lblFileName 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "File Name:"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   600
      TabIndex        =   25
      Top             =   3600
      Width           =   975
   End
   Begin VB.Label lblReadPreTrig 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   270
      Left            =   3960
      TabIndex        =   12
      Top             =   3120
      Width           =   750
   End
   Begin VB.Label lblShowPreTrig 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Left            =   1800
      TabIndex        =   29
      Top             =   3120
      Width           =   1815
   End
   Begin VB.Label lblPreTrig 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Pre-Trig Count:"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   120
      TabIndex        =   28
      Top             =   3120
      Width           =   1455
   End
   Begin VB.Label lblReadTotal 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   270
      Left            =   3960
      TabIndex        =   11
      Top             =   2760
      Width           =   750
   End
   Begin VB.Label lblShowCount 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   270
      Left            =   1800
      TabIndex        =   10
      Top             =   2760
      Width           =   945
   End
   Begin VB.Label lblCount 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Count:"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   600
      TabIndex        =   24
      Top             =   2760
      Width           =   975
   End
   Begin VB.Label lblReadGain 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   270
      Left            =   3960
      TabIndex        =   17
      Top             =   2520
      Width           =   825
   End
   Begin VB.Label lblShowGain 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   270
      Left            =   1800
      TabIndex        =   8
      Top             =   2520
      Width           =   945
   End
   Begin VB.Label lblGain 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Gain:"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   600
      TabIndex        =   23
      Top             =   2520
      Width           =   975
   End
   Begin VB.Label lblReadOptions 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   270
      Left            =   3960
      TabIndex        =   16
      Top             =   2280
      Width           =   825
   End
   Begin VB.Label lblShowOptions 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   270
      Left            =   1800
      TabIndex        =   7
      Top             =   2280
      Width           =   945
   End
   Begin VB.Label lblOptions 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Options:"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   600
      TabIndex        =   22
      Top             =   2280
      Width           =   975
   End
   Begin VB.Label lblReadHiChan 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   270
      Left            =   3960
      TabIndex        =   15
      Top             =   2040
      Width           =   825
   End
   Begin VB.Label lblShowHiChan 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   270
      Left            =   1800
      TabIndex        =   6
      Top             =   2040
      Width           =   945
   End
   Begin VB.Label lblHiChan 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "High Channel:"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   240
      TabIndex        =   21
      Top             =   2040
      Width           =   1335
   End
   Begin VB.Label lblReadLoChan 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   270
      Left            =   3960
      TabIndex        =   14
      Top             =   1800
      Width           =   825
   End
   Begin VB.Label lblShowLoChan 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   270
      Left            =   1800
      TabIndex        =   5
      Top             =   1800
      Width           =   945
   End
   Begin VB.Label lblLoChan 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Low Channel:"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   240
      TabIndex        =   20
      Top             =   1800
      Width           =   1335
   End
   Begin VB.Label lblReadRate 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   270
      Left            =   3960
      TabIndex        =   13
      Top             =   1560
      Width           =   825
   End
   Begin VB.Label lblShowRate 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   270
      Left            =   1800
      TabIndex        =   4
      Top             =   1560
      Width           =   945
   End
   Begin VB.Label lblRate 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Rate:"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   600
      TabIndex        =   19
      Top             =   1560
      Width           =   975
   End
   Begin VB.Label lblInCol 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Params Read from File"
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
      Left            =   3480
      TabIndex        =   27
      Top             =   1200
      Width           =   2175
   End
   Begin VB.Label lblOutCol 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Params to Function"
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
      Left            =   1440
      TabIndex        =   26
      Top             =   1200
      Width           =   1815
   End
   Begin VB.Label lblAcqStat 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   450
      Left            =   120
      TabIndex        =   3
      Top             =   600
      Width           =   6135
   End
   Begin VB.Label lblDemoFunction 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Demonstration of cbFileAInScan()"
      ForeColor       =   &H80000008&
      Height          =   285
      Left            =   120
      TabIndex        =   0
      Top             =   120
      Width           =   6105
   End
End
Attribute VB_Name = "frmDataDisplay"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
'=======================================================================

' File:                         ULFI01.FRM

' Library Call Demonstrated:    cbFileAInScan()

' Purpose:                      Scan a range of A/D channels and
'                               store the data in a disk file.

' Demonstration:                Collects data points from analog input
'                               channels and stores them in a file.

' Other Library Calls:          cbErrHandling()

' Special Requirements:         Board 0 must have an A/D converter
'                               and support cbFileAInScan.
'                               Analog signal on an input channel.

'==========================================================================
Option Explicit

Const BoardNum As Long = 0       ' Board number

Dim CBRange As Long
Dim ADResolution As Long, NumAIChans As Long
Dim HighChan As Long, LowChan As Long
Const NumPoints As Long = 2000   ' Number of data points to collect
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
   '    DONTPRINT   :all warnings and errors encountered will not be printed
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
      lblAcqStat.Caption = "Board " & Format(BoardNum, "0") & _
      " does not have analog input channels."
      lblAcqStat.ForeColor = &HFF&
      cmdStartAcq.Enabled = False
      cmdStopConvert.Enabled = True
      txtFileName.Enabled = False
   Else
      lblDemoFunction.Caption = _
         "Demonstration of cbFileAInScan() using board " & _
         Format(BoardNum, "0") & "."
   End If
   
End Sub

Private Sub cmdStartAcq_Click()

   Dim FileLowChan As Integer, FileHighChan As Integer
   Dim CBCount As Long, CBRate As Long
   Dim Filename As String, Options As Long
   Dim DataCount As String, PretrigCount As Long, TotalCount As Long
   
   cmdStartAcq.Enabled = False
   cmdStopConvert.Enabled = False

   ' Parameters:
   '   BoardNum   :the number used by CB.CFG to describe this board
   '   LowChan    :first A/D channel of the scan
   '   HighChan   :last A/D channel of the scan
   '   CBCount&   :the total number of A/D samples to collect
   '   CBRate&    :Sample rate in samples per second
   '   CBRange       :the gain for the board
   '   FileName$  :the filename for the collected data values
   '   Options    :data collection options

   CBCount& = NumPoints&
   Filename$ = txtFileName.Text    ' a full path may be required here
   CBRate& = 1000
   LowChan = 0
   HighChan = 1
   Options = DEFAULTOPTION

   DataCount$ = Format$(NumPoints&, "0")
   lblAcqStat.Caption = "Collecting " + DataCount$ + " data points..."
   lblShowRate.Caption = Format$(CBRate&, "0")
   lblShowLoChan.Caption = Format$(LowChan, "0")
   lblShowHiChan.Caption = Format$(HighChan, "0")
   lblShowOptions.Caption = Format$(Options, "0")
   lblShowGain.Caption = Format$(CBRange, "0")
   lblShowFile.Caption = Filename$
   lblShowCount.Caption = Format$(CBCount&, "0")
   lblShowPreTrig.Caption = "Not Applicable"
   DoEvents

   ' Collect the values with cbFileAInScan()

   ULStat = cbFileAInScan(BoardNum, LowChan, HighChan, _
      CBCount&, CBRate&, CBRange, Filename$, Options)
   If ULStat = 502 Then
      MsgBox "Enter the name of the file in which to store " & _
         "the data in text box.", 0, "Bad File Name"
      cmdStartAcq.Enabled = True
      txtFileName.SetFocus
      Exit Sub
   ElseIf ULStat <> 0 Then
      Stop
   End If
   
   ' show how many data points were collected
   
   cmdStopConvert.Enabled = True
   ULStat = cbFileGetInfo(Filename$, FileLowChan, FileHighChan, _
      PretrigCount&, TotalCount&, CBRate&, CBRange)
   If ULStat <> 0 Then Stop

   lblReadRate.Caption = Format$(CBRate&, "0")
   lblReadLoChan.Caption = Format$(LowChan, "0")
   lblReadHiChan.Caption = Format$(HighChan, "0")
   lblReadOptions.Caption = Format$(Options, "0")
   lblReadGain.Caption = Format$(CBRange, "0")
   lblReadFile.Caption = Filename$
   
   lblReadTotal.Caption = Format$(TotalCount&, "0")
   lblReadPreTrig.Caption = Format$(PretrigCount&, "0")
   lblAcqStat.Caption = "Displaying data read from file."
   cmdStartAcq.Enabled = True
   
End Sub

Private Sub cmdStopConvert_Click()
   
   End
   
End Sub

