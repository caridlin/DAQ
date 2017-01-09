VERSION 5.00
Begin VB.Form frmShowFileData 
   Appearance      =   0  'Flat
   BackColor       =   &H80000005&
   Caption         =   "Universal Library Analog Input to File"
   ClientHeight    =   5505
   ClientLeft      =   105
   ClientTop       =   1545
   ClientWidth     =   5355
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
   ScaleHeight     =   5505
   ScaleWidth      =   5355
   Begin VB.CommandButton cmdStopConvert 
      Appearance      =   0  'Flat
      Caption         =   "Quit"
      Height          =   390
      Left            =   4440
      TabIndex        =   17
      Top             =   4920
      Width           =   690
   End
   Begin VB.CommandButton cmdShowData 
      Appearance      =   0  'Flat
      Caption         =   "Display Data"
      Enabled         =   0   'False
      Height          =   390
      Left            =   2880
      TabIndex        =   29
      Top             =   4920
      Visible         =   0   'False
      Width           =   1260
   End
   Begin VB.CommandButton cmdStartAcq 
      Appearance      =   0  'Flat
      Caption         =   "Start"
      Height          =   390
      Left            =   2880
      TabIndex        =   18
      Top             =   4920
      Width           =   1260
   End
   Begin VB.TextBox txtFileName 
      Appearance      =   0  'Flat
      Height          =   375
      Left            =   2760
      TabIndex        =   44
      Text            =   "DEMO.DAT"
      Top             =   4320
      Width           =   2415
   End
   Begin VB.Label lblFileInstruct 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Enter the name of the file in which to store the data."
      ForeColor       =   &H80000008&
      Height          =   615
      Left            =   120
      TabIndex        =   43
      Top             =   4080
      Width           =   2535
   End
   Begin VB.Label lblShowFile 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   270
      Left            =   1755
      TabIndex        =   25
      Top             =   3645
      Width           =   2610
   End
   Begin VB.Label lblFileName 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "File Name:"
      ForeColor       =   &H80000008&
      Height          =   210
      Left            =   510
      TabIndex        =   42
      Top             =   3645
      Width           =   1065
   End
   Begin VB.Label lblShowData 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   210
      Index           =   9
      Left            =   4440
      TabIndex        =   33
      Top             =   3270
      Width           =   690
   End
   Begin VB.Label lblData9 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Data Point 9:"
      ForeColor       =   &H80000008&
      Height          =   210
      Left            =   2955
      TabIndex        =   31
      Top             =   3270
      Width           =   1305
   End
   Begin VB.Label lblShowData 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   210
      Index           =   8
      Left            =   4440
      TabIndex        =   32
      Top             =   3075
      Width           =   690
   End
   Begin VB.Label lblData8 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Data Point 8:"
      ForeColor       =   &H80000008&
      Height          =   210
      Left            =   2955
      TabIndex        =   30
      Top             =   3075
      Width           =   1305
   End
   Begin VB.Label lblShowPreTrig 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   210
      Left            =   1755
      TabIndex        =   28
      Top             =   2970
      Width           =   690
   End
   Begin VB.Label lblPreTrig 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "PreTrig Count:"
      ForeColor       =   &H80000008&
      Height          =   210
      Left            =   315
      TabIndex        =   41
      Top             =   2970
      Width           =   1260
   End
   Begin VB.Label lblShowData 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   210
      Index           =   7
      Left            =   4440
      TabIndex        =   16
      Top             =   2880
      Width           =   690
   End
   Begin VB.Label lblData7 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Data Point 7:"
      ForeColor       =   &H80000008&
      Height          =   210
      Left            =   2955
      TabIndex        =   8
      Top             =   2880
      Width           =   1305
   End
   Begin VB.Label lblShowTotal 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   270
      Left            =   1755
      TabIndex        =   27
      Top             =   2790
      Width           =   690
   End
   Begin VB.Label lblTotal 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Total Count:"
      ForeColor       =   &H80000008&
      Height          =   210
      Left            =   510
      TabIndex        =   40
      Top             =   2790
      Width           =   1065
   End
   Begin VB.Label lblShowData 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   210
      Index           =   6
      Left            =   4440
      TabIndex        =   15
      Top             =   2685
      Width           =   690
   End
   Begin VB.Label lblData6 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Data Point 6:"
      ForeColor       =   &H80000008&
      Height          =   210
      Left            =   2955
      TabIndex        =   7
      Top             =   2685
      Width           =   1305
   End
   Begin VB.Label lblShowCount 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   270
      Left            =   1755
      TabIndex        =   26
      Top             =   2595
      Width           =   690
   End
   Begin VB.Label lblCount 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Count:"
      ForeColor       =   &H80000008&
      Height          =   210
      Left            =   600
      TabIndex        =   39
      Top             =   2595
      Width           =   975
   End
   Begin VB.Label lblShowData 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   210
      Index           =   5
      Left            =   4440
      TabIndex        =   14
      Top             =   2490
      Width           =   690
   End
   Begin VB.Label lblData5 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Data Point 5:"
      ForeColor       =   &H80000008&
      Height          =   210
      Left            =   2955
      TabIndex        =   6
      Top             =   2490
      Width           =   1305
   End
   Begin VB.Label lblShowGain 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   270
      Left            =   1755
      TabIndex        =   24
      Top             =   2400
      Width           =   690
   End
   Begin VB.Label lblGain 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Gain:"
      ForeColor       =   &H80000008&
      Height          =   210
      Left            =   600
      TabIndex        =   38
      Top             =   2400
      Width           =   975
   End
   Begin VB.Label lblShowData 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   210
      Index           =   4
      Left            =   4440
      TabIndex        =   13
      Top             =   2310
      Width           =   690
   End
   Begin VB.Label lblData4 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Data Point 4:"
      ForeColor       =   &H80000008&
      Height          =   210
      Left            =   2955
      TabIndex        =   5
      Top             =   2310
      Width           =   1305
   End
   Begin VB.Label lblShowOptions 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   270
      Left            =   1755
      TabIndex        =   23
      Top             =   2205
      Width           =   690
   End
   Begin VB.Label lblOptions 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Options:"
      ForeColor       =   &H80000008&
      Height          =   210
      Left            =   600
      TabIndex        =   37
      Top             =   2205
      Width           =   975
   End
   Begin VB.Label lblShowData 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   210
      Index           =   3
      Left            =   4440
      TabIndex        =   12
      Top             =   2115
      Width           =   690
   End
   Begin VB.Label lblData3 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Data Point 3:"
      ForeColor       =   &H80000008&
      Height          =   210
      Left            =   2955
      TabIndex        =   4
      Top             =   2115
      Width           =   1305
   End
   Begin VB.Label lblShowHiChan 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   270
      Left            =   1755
      TabIndex        =   22
      Top             =   2010
      Width           =   690
   End
   Begin VB.Label lblHiChan 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "High Channel:"
      ForeColor       =   &H80000008&
      Height          =   210
      Left            =   315
      TabIndex        =   36
      Top             =   2010
      Width           =   1260
   End
   Begin VB.Label lblShowData 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   210
      Index           =   2
      Left            =   4440
      TabIndex        =   11
      Top             =   1920
      Width           =   690
   End
   Begin VB.Label lblData2 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Data Point 2:"
      ForeColor       =   &H80000008&
      Height          =   210
      Left            =   2955
      TabIndex        =   3
      Top             =   1920
      Width           =   1305
   End
   Begin VB.Label lblShowLoChan 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   270
      Left            =   1755
      TabIndex        =   21
      Top             =   1830
      Width           =   690
   End
   Begin VB.Label lblLoChan 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Low Channel:"
      ForeColor       =   &H80000008&
      Height          =   210
      Left            =   315
      TabIndex        =   35
      Top             =   1830
      Width           =   1260
   End
   Begin VB.Label lblShowData 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   210
      Index           =   1
      Left            =   4440
      TabIndex        =   10
      Top             =   1725
      Width           =   690
   End
   Begin VB.Label lblData1 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Data Point 1:"
      ForeColor       =   &H80000008&
      Height          =   210
      Left            =   2955
      TabIndex        =   2
      Top             =   1725
      Width           =   1305
   End
   Begin VB.Label lblShowRate 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   270
      Left            =   1755
      TabIndex        =   20
      Top             =   1635
      Width           =   690
   End
   Begin VB.Label lblRate 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Rate:"
      ForeColor       =   &H80000008&
      Height          =   210
      Left            =   600
      TabIndex        =   34
      Top             =   1635
      Width           =   975
   End
   Begin VB.Label lblShowData 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   210
      Index           =   0
      Left            =   4440
      TabIndex        =   9
      Top             =   1530
      Width           =   690
   End
   Begin VB.Label lblData0 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Data Point 0:"
      ForeColor       =   &H80000008&
      Height          =   210
      Left            =   2955
      TabIndex        =   1
      Top             =   1530
      Width           =   1305
   End
   Begin VB.Label lblAcqStat 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   690
      Left            =   90
      TabIndex        =   19
      Top             =   750
      Width           =   5175
   End
   Begin VB.Label lblDemoFunction 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Demonstration of cbFileAInScan()"
      ForeColor       =   &H80000008&
      Height          =   555
      Left            =   90
      TabIndex        =   0
      Top             =   90
      Width           =   5145
   End
End
Attribute VB_Name = "frmShowFileData"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
'=========================================================================

' File:                         ULFI02.FRM

' Library Call Demonstrated:    File Operations:
'                               cbFileRead()
'                               cbFileGetInfo()

' Purpose:                      Read information & data from a streamer file.

' Demonstration:                Creates a file and scans analog data to the
'                               file. Displays the data in the file and the
'                               information in the file header.

' Other Library Calls:          cbFileAInScan()
'                               cbErrHandling()

' Special Requirements:         Board 0 must have an A/D converter and support cbFileAInScan.
'                               Analog signal on an input channel.

'==========================================================================
Option Explicit

Const BoardNum As Long = 0       ' Board number

Dim CBRange As Long
Dim ADResolution As Long, NumAIChans As Long
Dim HighChan As Long, LowChan As Long

Const NumPoints As Long = 2000   ' Number of data points to collect
Const ArraySize As Long = 10     ' size of Visual Basics data array

Dim DataBuffer() As Integer   ' declare data array
Dim Filename As String        ' name of file in which data will be stored
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
      cmdStartAcq.Visible = False
      cmdStopConvert.Visible = True
      cmdStopConvert.Enabled = True
      txtFileName.Enabled = False
   Else
      ReDim DataBuffer(ArraySize - 1)
      lblDemoFunction.Caption = _
         "Demonstration of cbFileAInScan() using board " _
         & Format(BoardNum, "0") & "."
   End If

End Sub

Private Sub cmdStartAcq_Click()

   Dim FileLowChan As Integer, FileHighChan As Integer
   Dim CBCount As Long, CBRate As Long, Options As Long
   Dim DataCount As String, PretrigCount As Long
   Dim TotalCount As Long, Range As Long
   
   cmdStartAcq.Enabled = False
   
   ' Parameters:
   CBCount& = NumPoints
   Filename$ = txtFileName.Text     ' it may be necessary to add path to
   CBRate& = 1000                  ' file name for data file to be found
   LowChan = 0
   HighChan = 0
   Options = DEFAULTOPTION

   DataCount$ = Format$(NumPoints, "0")
   lblAcqStat.Caption = "Collecting " & DataCount$ & " data points..."
   DoEvents
   lblShowRate.Caption = Format$(CBRate&, "0")
   lblShowLoChan.Caption = Format$(LowChan, "0")
   lblShowHiChan.Caption = Format$(HighChan, "0")
   lblShowOptions.Caption = Format$(Options, "0")
   lblShowGain.Caption = Format$(CBRange, "0")
   lblShowFile.Caption = Filename$
   lblShowCount.Caption = Format$(CBCount&, "0")

   ' Collect the values with cbFileAInScan()
   '  Parameters:
   '    BoardNum   :the number used by CB.CFG to describe this board
   '    LowChan    :first A/D channel of the scan
   '    HighChan   :last A/D channel of the scan
   '    CBCount&   :the total number of A/D samples to collect
   '    CBRate&    :Sample rate in samples per second
   '    CBRange    :the gain for the board
   '    FileName   :the filename for the collected data values
   '    Options    :data collection options

   ULStat = cbFileAInScan(BoardNum, LowChan, HighChan, _
      CBCount&, CBRate&, CBRange, Filename$, Options)
   If ULStat = 502 Then
      MsgBox "Enter the name of the file in which to store " & _
      "the data in the text box.", 0, "Bad File Name"
      cmdStartAcq.Enabled = True
      cmdStartAcq.Visible = True
      txtFileName.SetFocus
      Exit Sub
   ElseIf ULStat <> 0 Then
      Stop
   End If

   ' show how many data points were collected
   
   ULStat = cbFileGetInfo(Filename$, FileLowChan, FileHighChan, _
      PretrigCount&, TotalCount&, CBRate&, Range&)
   If ULStat <> 0 Then Stop

   lblShowRate.Caption = Format$(CBRate&, "0")
   lblShowLoChan.Caption = Format$(LowChan, "0")
   lblShowHiChan.Caption = Format$(HighChan, "0")
   lblShowOptions.Caption = Format$(Options, "0")
   lblShowGain.Caption = Format$(Range&, "0")
   lblShowFile.Caption = Filename$
   
   lblShowTotal.Caption = Format$(TotalCount&, "0")
   lblShowPreTrig.Caption = Format$(PretrigCount&, "0")
  
   cmdShowData.Enabled = True
   cmdShowData.Visible = True
   lblAcqStat.Caption = "Acquisition complete - the file can now be read."

End Sub

Private Sub cmdShowData_Click()

   Dim FirstPoint As Long, ReadPoints As Long
   Dim I As Integer
   
   ' show the data using cbFileRead()
   '  Parameters:
   '    FileName$      :the filename containing the data
   '    FirstPoint&    :index of the first data value to read
   '    NumPoints&     :the number of data values to read from the file
   '    DataBuffer()  :array to read data into

   FirstPoint& = 0                   ' start at the first point
   ReadPoints& = ArraySize&          ' specify number of data points to read
  
   ULStat = cbFileRead(Filename$, FirstPoint&, ReadPoints&, DataBuffer(0))
   If ULStat <> 0 Then Stop
   lblAcqStat.Caption = ""
   
   ' display the data values read from the file
  
   For I = FirstPoint& To ArraySize& - 1
      lblShowData(I).Caption = Format$(DataBuffer(I), "0")
   Next I

   cmdShowData.Visible = False
   cmdStartAcq.Enabled = True
   cmdStartAcq.Visible = True

End Sub

Private Sub cmdStopConvert_Click()
     
     End

End Sub


