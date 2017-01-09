VERSION 5.00
Begin VB.Form frmStatusDisplay 
   Appearance      =   0  'Flat
   BackColor       =   &H80000005&
   Caption         =   "Simultaneous cbAInScan%() and cbAoutScan%() "
   ClientHeight    =   5130
   ClientLeft      =   2820
   ClientTop       =   1620
   ClientWidth     =   9165
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
   ScaleHeight     =   5130
   ScaleWidth      =   9165
   Begin VB.CommandButton cmdStopADConvert 
      Appearance      =   0  'Flat
      Caption         =   "Stop A/D Background Operation"
      Enabled         =   0   'False
      Height          =   396
      Left            =   840
      TabIndex        =   33
      Top             =   1620
      Visible         =   0   'False
      Width           =   3060
   End
   Begin VB.CommandButton cmdStartDABgnd 
      Appearance      =   0  'Flat
      Caption         =   "Start D/A Background Operation"
      Height          =   396
      Left            =   5280
      TabIndex        =   32
      Top             =   1620
      Width           =   3060
   End
   Begin VB.CommandButton cmdStopDAConvert 
      Appearance      =   0  'Flat
      Caption         =   "Stop D/A Background Operation"
      Enabled         =   0   'False
      Height          =   396
      Left            =   5280
      TabIndex        =   31
      Top             =   1620
      Visible         =   0   'False
      Width           =   3060
   End
   Begin VB.TextBox txtHighChan 
      Appearance      =   0  'Flat
      Height          =   285
      Left            =   5340
      TabIndex        =   25
      Text            =   "3"
      Top             =   2100
      Width           =   495
   End
   Begin VB.CommandButton cmdQuit 
      Appearance      =   0  'Flat
      Caption         =   "Quit"
      Height          =   390
      Left            =   4320
      TabIndex        =   18
      Top             =   4620
      Width           =   780
   End
   Begin VB.Timer tmrCheckStatus 
      Enabled         =   0   'False
      Interval        =   200
      Left            =   4380
      Top             =   1500
   End
   Begin VB.CommandButton cmdStartADBgnd 
      Appearance      =   0  'Flat
      Caption         =   "Start A/D Background Operation"
      Height          =   396
      Left            =   840
      TabIndex        =   17
      Top             =   1620
      Width           =   3060
   End
   Begin VB.Label lblCount 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Current A/D Count:"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   1050
      TabIndex        =   36
      Top             =   4110
      Width           =   1860
   End
   Begin VB.Label Label2 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Current D/A Count:"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   5400
      TabIndex        =   35
      Top             =   4110
      Width           =   2025
   End
   Begin VB.Label lblInstruction 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Board 0 must support simultaneous paced input and paced output. For more information, see hardware documentation."
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
      Left            =   900
      TabIndex        =   34
      Top             =   540
      Width           =   7515
   End
   Begin VB.Label Label4 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Status of D/A Background:"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   4605
      TabIndex        =   30
      Top             =   3480
      Width           =   2820
   End
   Begin VB.Label Label3 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Current D/A Index:"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   5400
      TabIndex        =   29
      Top             =   3825
      Width           =   2025
   End
   Begin VB.Label lblShowDACount 
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
      ForeColor       =   &H00FF0000&
      Height          =   255
      Left            =   7560
      TabIndex        =   28
      Top             =   4140
      Width           =   1095
   End
   Begin VB.Label lblShowDAIndex 
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
      ForeColor       =   &H00FF0000&
      Height          =   255
      Left            =   7560
      TabIndex        =   27
      Top             =   3825
      Width           =   1125
   End
   Begin VB.Label lblShowDAStat 
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
      ForeColor       =   &H00FF0000&
      Height          =   255
      Left            =   7560
      TabIndex        =   26
      Top             =   3480
      Width           =   1110
   End
   Begin VB.Label lblMeasure 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Measure Channels 0 to"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   2820
      TabIndex        =   24
      Top             =   2100
      Width           =   2415
   End
   Begin VB.Label lblShowADCount 
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
      ForeColor       =   &H00FF0000&
      Height          =   255
      Left            =   3030
      TabIndex        =   23
      Top             =   4140
      Width           =   1275
   End
   Begin VB.Label lblShowADIndex 
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
      ForeColor       =   &H00FF0000&
      Height          =   255
      Left            =   3030
      TabIndex        =   22
      Top             =   3825
      Width           =   1245
   End
   Begin VB.Label lblIndex 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Current A/D Index:"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   1005
      TabIndex        =   21
      Top             =   3825
      Width           =   1905
   End
   Begin VB.Label lblShowADStat 
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
      ForeColor       =   &H00FF0000&
      Height          =   255
      Left            =   3060
      TabIndex        =   20
      Top             =   3480
      Width           =   1215
   End
   Begin VB.Label lblStatus 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Status of A/D Background:"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   450
      TabIndex        =   19
      Top             =   3480
      Width           =   2460
   End
   Begin VB.Label lblADData 
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
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   7
      Left            =   8040
      TabIndex        =   16
      Top             =   2940
      Width           =   975
   End
   Begin VB.Label lblChan7 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Channel 7:"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   6960
      TabIndex        =   8
      Top             =   2940
      Width           =   975
   End
   Begin VB.Label lblADData 
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
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   3
      Left            =   3480
      TabIndex        =   12
      Top             =   2940
      Width           =   975
   End
   Begin VB.Label lblChan3 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Channel 3:"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   2400
      TabIndex        =   4
      Top             =   2940
      Width           =   975
   End
   Begin VB.Label lblADData 
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
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   6
      Left            =   8040
      TabIndex        =   15
      Top             =   2580
      Width           =   975
   End
   Begin VB.Label lblChan6 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Channel 6:"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   6960
      TabIndex        =   7
      Top             =   2580
      Width           =   975
   End
   Begin VB.Label lblADData 
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
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   2
      Left            =   3480
      TabIndex        =   11
      Top             =   2580
      Width           =   975
   End
   Begin VB.Label lblChan2 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Channel 2:"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   2400
      TabIndex        =   3
      Top             =   2580
      Width           =   975
   End
   Begin VB.Label lblADData 
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
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   5
      Left            =   5760
      TabIndex        =   14
      Top             =   2940
      Width           =   975
   End
   Begin VB.Label lblChan5 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Channel 5:"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   4680
      TabIndex        =   6
      Top             =   2940
      Width           =   975
   End
   Begin VB.Label lblADData 
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
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   1
      Left            =   1260
      TabIndex        =   10
      Top             =   2940
      Width           =   975
   End
   Begin VB.Label lblChan1 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Channel 1:"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   180
      TabIndex        =   2
      Top             =   2940
      Width           =   975
   End
   Begin VB.Label lblADData 
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
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   4
      Left            =   5760
      TabIndex        =   13
      Top             =   2580
      Width           =   975
   End
   Begin VB.Label lblChan4 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Channel 4:"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   4680
      TabIndex        =   5
      Top             =   2580
      Width           =   975
   End
   Begin VB.Label lblADData 
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
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   0
      Left            =   1260
      TabIndex        =   9
      Top             =   2580
      Width           =   975
   End
   Begin VB.Label lblChan0 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Channel 0:"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   180
      TabIndex        =   1
      Top             =   2580
      Width           =   975
   End
   Begin VB.Label lblDemoFunction 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Demonstration of Simultaneous cbAInScan() and cbAoutScan ()"
      ForeColor       =   &H80000008&
      Height          =   315
      Left            =   180
      TabIndex        =   0
      Top             =   120
      Width           =   8775
   End
End
Attribute VB_Name = "frmStatusDisplay"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
'ULAIO01.FRM================================================================

' File:                         ULAIO01.FRM

' Library Call Demonstrated:    cbGetStatus()
'                               cbStopBackground()

' Purpose:                      Run Simultaneous input/output functions using
'                               the same board.

' Demonstration:                cbAoutScan function generates a ramp signal
'                               while cbAinScan Displays the analog input on
'                               eight channels.

' Other Library Calls:          cbAinScan()
'                               cbAoutScan()
'                               cbErrHandling()

' Special Requirements:         Board 0 must support simultaneous paced input
'                               and paced output. See hardware documentation.

'==========================================================================
Option Explicit

Const BoardNum As Long = 0       ' Board number

Private NumAIChans As Long, NumAOChans As Long
Private LowChan As Long, HighChan As Long
Private ADRange As Long, DARange As Long

Const NumPoints As Long = 10000  ' Number of data points to collect
Const FirstPoint As Long = 0     ' set first element in buffer to transfer to array

Dim ADData() As Integer             ' dimension an array to hold the input values
Dim ADUserTerm As Boolean        ' flag to stop paced A/D manually
Dim ADMemHandle As Long          ' define a variable to contain the handle for
                                 ' memory allocated by Windows through cbWinBufAlloc()
Dim DAMemHandle As Long          ' define a variable to contain the handle for
Dim DAData() As Integer          ' dimension an array to hold the output values
                                 ' memory allocated by Windows through cbWinBufAlloc()
Dim DAUserTerm As Boolean        ' flag to stop paced D/A manually
Dim ULStat As Long
Dim ADResolution As Long, DAResolution As Long

Private Sub Form_Load()
   
   Dim DefaultTrig As Long
   Dim ReportError As Long, HandleError As Long
   
   ' declare revision level of Universal Library

   ULStat = cbDeclareRevision(CURRENTREVNUM)

   ' Initiate error handling
   '  activating error handling will trap errors like
   '  bad channel numbers and non-configured conditions.
   '  Parameters:
   '    PRINTALL    :all warnings and errors encountered will be printed
   '    DONTSTOP    :if an error is encountered, the program will not stop,
   '                  errors must be handled locally
  
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

   NumAIChans = FindAnalogChansOfType(BoardNum, ANALOGINPUT, _
      ADResolution, ADRange, LowChan, DefaultTrig)
   If Not AIOGeneralError Then _
      NumAOChans = FindAnalogChansOfType(BoardNum, ANALOGOUTPUT, _
      DAResolution, DARange, LowChan, DefaultTrig)
   
   If (NumAIChans = 0) Or (NumAOChans = 0) Then
      lblInstruction.Caption = "Board " & Format(BoardNum, "0") & _
      " does not have both input and output analog channels."
      cmdStartADBgnd.Enabled = False
      cmdStartDABgnd.Enabled = False
   Else
      Dim LongVal As Long
      Dim i As Long
      Dim DACount As Long

      ReDim ADData(NumPoints - 1)
      ReDim DAData(NumPoints - 1)

      If NumAIChans > 8 Then NumAIChans = 8
      txtHighChan.Text = Format(NumAIChans - 1, "0")
      
      ' set aside memory to hold A/D data
      ADMemHandle = cbWinBufAlloc(NumPoints)
      If ADMemHandle = 0 Then Stop

      ' set aside memory to hold D/A data
      Dim HalfScale As Long
      
      DAMemHandle = cbWinBufAlloc(NumPoints)
      If DAMemHandle = 0 Then Stop
      HalfScale& = (2 ^ DAResolution) / 2

      ' Generate D/A ramp data to be output via cbAoutScan function
      For i& = 0 To NumPoints - 1
      LongVal& = HalfScale& + i& / NumPoints * HalfScale& - HalfScale& / 2
        DAData(i&) = ULongValToInt(LongVal&)
      Next i&
      ULStat = cbWinArrayToBuf(DAData(0), DAMemHandle, FirstPoint, NumPoints)
   
      lblInstruction.Caption = "Board " & Format(BoardNum, "0") & _
          " collecting analog data on up to " & Format(NumAIChans, "0") & _
          " A/D channels using cbAInScan in background mode with Range set to " & _
          GetRangeString(ADRange) & " while generating a ramp output on " & _
          "D/A channel 0 using cbAOutScan in background mode with Range set to " & _
          GetRangeString(DARange) & "."
   End If

End Sub

Private Sub cmdStartADBgnd_Click()

   Dim NumChannels As Long, ADOptions As Long
   Dim ADCount As Long, ADRate As Long
   Dim Status As Integer, CurCount As Long, CurIndex As Long
   
   cmdStartADBgnd.Enabled = False
   cmdStartADBgnd.Visible = False
   cmdStopADConvert.Enabled = True
   cmdStopADConvert.Visible = True
   cmdQuit.Enabled = False
   ADUserTerm = False             ' initialize user terminate flag

   ' Collect the values with cbAInScan()
   '  Parameters:
   '    BoardNum   :the number used by CB.CFG to describe this board
   '    LowChan    :the first channel of the scan
   '    HighChan   :the last channel of the scan
   '    CBCount&    :the total number of A/D samples to collect
   '    CBRate&     :sample rate
   '    Gain        :the gain for the board
   '    ADData     :the array for the collected data values
   '    Options     :data collection options

   LowChan = 0                     ' first channel to acquire
   HighChan = Val(txtHighChan.Text) ' last channel to acquire
   If (HighChan > (NumAIChans - 1)) Then HighChan = (NumAIChans - 1)
   txtHighChan.Text = Format(HighChan, "0")
   
   NumChannels& = (HighChan - LowChan) + 1
   ADCount& = NumPoints            ' total number of data points to collect
   ADRate& = 1000 / NumChannels&   ' per channel sampling rate
   ADOptions& = CONVERTDATA + BACKGROUND
                                    ' return data as 12-bit values
                                    ' collect data in BACKGROUND mode

   If ADMemHandle = 0 Then Stop     ' check that a handle to a memory buffer exists

   ULStat = cbAInScan(BoardNum, LowChan, HighChan, ADCount&, ADRate&, _
      ADRange, ADMemHandle, ADOptions&)
   If ULStat <> 0 Then Stop

   ULStat = cbGetStatus(BoardNum, Status%, CurCount&, CurIndex&, AIFUNCTION)
   If ULStat <> 0 Then Stop

   If Status% = RUNNING Then
      lblShowADStat.Caption = "Running"
      lblShowADCount.Caption = Format$(CurCount&, "0")
      lblShowADIndex.Caption = Format$(CurIndex&, "0")
   End If

   tmrCheckStatus.Enabled = True

End Sub

Private Sub cmdStartDABgnd_Click()

   Dim LowDAChan As Long, HighDAChan As Long
   Dim DACount As Long, DARate As Long, DAOptions As Long
   Dim Status As Integer, CurCount As Long, CurIndex As Long
   
   cmdStartDABgnd.Enabled = False
   cmdStartDABgnd.Visible = False
   cmdStopDAConvert.Enabled = True
   cmdStopDAConvert.Visible = True
   cmdQuit.Enabled = False
   DAUserTerm = False                     ' initialize user terminate flag
  
   ' Collect the values with cbAoutnScan()
   '  Parameters:
   '    BoardNum     :the number used by CB.CFG to describe this board
   '    LowDAChan    :the first channel of the scan
   '    HighDAChan   :the last channel of the scan
   '    CBCount&     :the total number of D/A samples to output
   '    CBRate&      :sample rate
   '    DARange      :the gain for the board
   '    DAData       :array of values to send to the scanned channels
   '    Options      :data output options

   LowDAChan& = 0             ' first channel to output
   HighDAChan& = 0            ' last channel to output
   
   DACount& = NumPoints       ' total number of data points to output
   DARate& = 1000             ' output rate (samples per second)
   DAOptions& = BACKGROUND

   If DAMemHandle = 0 Then Stop      ' check that a handle to a memory buffer exists

   ULStat = cbAOutScan(BoardNum, LowDAChan&, HighDAChan&, DACount&, _
      DARate&, DARange, DAMemHandle, DAOptions&)
   If ULStat <> 0 Then Stop

   ULStat = cbGetStatus(BoardNum, Status%, CurCount&, CurIndex&, AOFUNCTION)
   If ULStat <> 0 Then Stop

   If Status% = RUNNING Then
      lblShowDAStat.Caption = "Running"
      lblShowDACount.Caption = Format$(CurCount&, "0")
      lblShowDAIndex.Caption = Format$(CurIndex&, "0")
   End If

   tmrCheckStatus.Enabled = True

End Sub

Private Sub tmrCheckStatus_Timer()

   Dim i As Long, j As Long
   Dim ADStatus As Integer, ADCurCount As Long, ADCurIndex As Long
   Dim DAStatus As Integer, DACurCount As Long, DACurIndex As Long
   
   ' This timer will check the status of the background data collection
   
   ' Parameters:
   '   BoardNum    :the number used by CB.CFG to describe this board
   '   Status     :current status of the background data collection
   '   CurCount&   :current number of samples collected
   '   CurIndex&   :index to the data buffer pointing to the start of the
   '                most recently collected scan

   ULStat = cbGetStatus(BoardNum, ADStatus%, ADCurCount&, ADCurIndex&, AIFUNCTION)
   If ULStat <> 0 Then Stop

   lblShowADCount.Caption = Format$(ADCurCount&, "0")
   lblShowADIndex.Caption = Format$(ADCurIndex&, "0")

   ' Check if the background operation has finished. If it has, then
   ' transfer the data from the memory buffer set up by Windows to an
   ' array for use by Visual Basic
   ' The BACKGROUND operation must be explicitly stopped

   If ADStatus% = RUNNING And Not ADUserTerm Then
      lblShowADStat.Caption = "Running"
      
      If ADCurIndex > 0 Then
         ULStat = cbWinBufToArray(ADMemHandle, ADData(ADCurIndex&), _
            ADCurIndex&, HighChan - LowChan + 1)
         If ULStat <> 0 Then Stop
         
         Dim TempCount As Long, CurrValue As Long
         For i = 0 To HighChan
            TempCount = i + ADCurIndex&
            CurrValue& = ADData(TempCount)
            If ADResolution = 16 Then _
               CurrValue& = (ADData(TempCount) Xor &H8000) + 32768
            lblADData(i).Caption = Format$(CurrValue&, "0")
         Next i
      End If
      
   ElseIf ADStatus% = IDLE Or ADUserTerm Then
      lblShowADStat.Caption = "Idle"
      ULStat = cbGetStatus(BoardNum, ADStatus%, ADCurCount&, ADCurIndex&, AIFUNCTION)
      If ULStat <> 0 Then Stop
      lblShowADCount.Caption = Format$(ADCurCount&, "0")
      lblShowADIndex.Caption = Format$(ADCurIndex&, "0")
      If ADMemHandle = 0 Then Stop
      ULStat = cbWinBufToArray(ADMemHandle, ADData(0), FirstPoint&, NumPoints)
      If ULStat <> 0 Then Stop
      
      For i = 0 To HighChan
         CurrValue& = ADData(i)
         If ADResolution = 16 Then _
            CurrValue& = (ADData(i) Xor &H8000) + 32768
         lblADData(i).Caption = Format$(CurrValue&, "0")
      Next i

      For j = HighChan + 1 To 7
        lblADData(j).Caption = ""
      Next j

      ULStat = cbStopBackground(BoardNum, AIFUNCTION)
      If ULStat <> 0 Then Stop
      cmdStartADBgnd.Enabled = True
      cmdStartADBgnd.Visible = True
      cmdStopADConvert.Enabled = False
      cmdStopADConvert.Visible = False
   End If
   
   '==========================================================
   ULStat = cbGetStatus(BoardNum, DAStatus%, DACurCount&, DACurIndex&, AOFUNCTION)
   If ULStat <> 0 Then Stop

   lblShowDACount.Caption = Format$(DACurCount&, "0")
   lblShowDAIndex.Caption = Format$(DACurIndex&, "0")

   ' Check if the background operation has finished.

   If DAStatus% = RUNNING And Not DAUserTerm Then
      lblShowDAStat.Caption = "Running"
   ElseIf DAStatus% = IDLE Or DAUserTerm Then
      lblShowDAStat.Caption = "Idle"
      ULStat = cbGetStatus(BoardNum, DAStatus%, DACurCount&, DACurIndex&, AOFUNCTION)
      If ULStat <> 0 Then Stop
      lblShowDACount.Caption = Format$(DACurCount&, "0")
      lblShowDAIndex.Caption = Format$(DACurIndex&, "0")
      
      If DAMemHandle = 0 Then Stop

      ULStat = cbStopBackground(BoardNum, AOFUNCTION)
      If ULStat <> 0 Then Stop
      cmdStartDABgnd.Enabled = True
      cmdStartDABgnd.Visible = True
      cmdStopDAConvert.Enabled = False
      cmdStopDAConvert.Visible = False
   End If
   
    If ADStatus% = IDLE And DAStatus% = IDLE Then
       tmrCheckStatus.Enabled = False
       cmdQuit.Enabled = True

    End If

End Sub


Private Function ULongValToInt(LongVal As Long) As Integer

   Select Case LongVal
      Case Is > 65535
         ULongValToInt = -1
      Case Is < 0
         ULongValToInt = 0
      Case Else
         ULongValToInt = (LongVal - 32768) Xor &H8000
   End Select

End Function

Private Sub cmdStopADConvert_Click()
   
   ADUserTerm = True

End Sub

Private Sub cmdStopDAConvert_Click()

    DAUserTerm = True
    
End Sub

Private Sub cmdQuit_Click()
   
   ' Free up memory for use by other programs
   ULStat = cbWinBufFree(ADMemHandle)
   ULStat = cbWinBufFree(DAMemHandle)
   If ULStat <> 0 Then Stop
   End
   
End Sub

