VERSION 5.00
Begin VB.Form frmSendAData 
   Appearance      =   0  'Flat
   BackColor       =   &H80000005&
   Caption         =   "Universal Library Analog Output "
   ClientHeight    =   3180
   ClientLeft      =   105
   ClientTop       =   1545
   ClientWidth     =   5055
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
   LinkTopic       =   "Form2"
   PaletteMode     =   1  'UseZOrder
   ScaleHeight     =   3180
   ScaleWidth      =   5055
   Begin VB.CommandButton cmdEndProgram 
      Appearance      =   0  'Flat
      Caption         =   "Quit"
      Height          =   390
      Left            =   3960
      TabIndex        =   1
      Top             =   2700
      Width           =   825
   End
   Begin VB.CommandButton cmdSendData 
      Appearance      =   0  'Flat
      Caption         =   "Send Data"
      Height          =   375
      Left            =   1920
      TabIndex        =   2
      Top             =   2700
      Width           =   1215
   End
   Begin VB.Label lblAOutVolts 
      Alignment       =   2  'Center
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
      Left            =   3840
      TabIndex        =   15
      Top             =   2160
      Width           =   975
   End
   Begin VB.Label lblAOutVolts 
      Alignment       =   2  'Center
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
      Left            =   2640
      TabIndex        =   14
      Top             =   2160
      Width           =   975
   End
   Begin VB.Label lblAOutVolts 
      Alignment       =   2  'Center
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
      Left            =   1440
      TabIndex        =   13
      Top             =   2160
      Width           =   975
   End
   Begin VB.Label lblAOutVolts 
      Alignment       =   2  'Center
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
      Left            =   240
      TabIndex        =   12
      Top             =   2160
      Width           =   975
   End
   Begin VB.Label lblInstruction 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Board 0 must have at least one analog output."
      ForeColor       =   &H000000FF&
      Height          =   735
      Left            =   240
      TabIndex        =   11
      Top             =   600
      Width           =   4575
   End
   Begin VB.Label lblAOutData 
      Alignment       =   2  'Center
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
      Left            =   3840
      TabIndex        =   10
      Top             =   1800
      Width           =   975
   End
   Begin VB.Label lblAOutData 
      Alignment       =   2  'Center
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
      Left            =   2640
      TabIndex        =   9
      Top             =   1800
      Width           =   975
   End
   Begin VB.Label lblAOutData 
      Alignment       =   2  'Center
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
      Left            =   1440
      TabIndex        =   8
      Top             =   1800
      Width           =   975
   End
   Begin VB.Label lblAOutData 
      Alignment       =   2  'Center
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
      Left            =   240
      TabIndex        =   3
      Top             =   1800
      Width           =   975
   End
   Begin VB.Label lblChan3 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Channel 3"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   -1  'True
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   3840
      TabIndex        =   7
      Top             =   1440
      Width           =   975
   End
   Begin VB.Label lblChan2 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Channel 2"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   -1  'True
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   2640
      TabIndex        =   6
      Top             =   1440
      Width           =   975
   End
   Begin VB.Label lblChan1 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Channel 1"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   -1  'True
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   1440
      TabIndex        =   5
      Top             =   1440
      Width           =   975
   End
   Begin VB.Label lblChan0 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Channel 0"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   -1  'True
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   240
      TabIndex        =   4
      Top             =   1440
      Width           =   975
   End
   Begin VB.Label lblDemoFunction 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Demonstration of cbAOutScan()"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   120
      TabIndex        =   0
      Top             =   120
      Width           =   4815
   End
End
Attribute VB_Name = "frmSendAData"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
'ULAO02.VBP================================================================

' File:                         ULAO02.VBP

' Library Call Demonstrated:    cbAOutScan&()

' Purpose:                      Writes to a range of D/A Output Channels.

' Demonstration:                Sends a digital output to the D/A channels

' Other Library Calls:          cbErrHandling&()

' Special Requirements:         Board 0 must have at least one D/A converter.
'                               This function is designed for boards that
'                               support timed analog output.  It can be used
'                               for polled output boards but only for values
'                               of NumPoints up to the number of channels
'                               that the board supports (i.e., NumPoints =
'                               6 maximum for the six channel CIO-DDA06).

' (c) Copyright 2005-2011, Measurement Computing Corp.
' All rights reserved.
'==========================================================================
Option Explicit

Const BoardNum As Long = 0           ' Board number

Dim CBRange As Long
Dim DAResolution As Long, NumAOChans As Long
Dim LowChan As Long, MaxChan As Long
Dim NumPoints As Long

Dim DAData() As Integer
Dim MemHandle As Long        ' define a variable to contain the handle for
                             ' memory allocated by Windows through cbWinBufAlloc&()
Dim FirstPoint As Long
Dim ULStat As Long

Private Sub Form_Load()

   Dim ReportError As Long, HandleError As Long
   Dim DefaultTrig As Long, ChannelType As Long
   Dim FSCount As Long, ValueStep As Long
   Dim i As Long, StepCount As Long, CBCount As Long
   
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
   If ULStat <> 0 Then Stop
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
      Me.cmdSendData.Enabled = False
   Else
      If NumAOChans > 4 Then NumAOChans = 4
      NumPoints = NumAOChans
      MaxChan = NumAOChans - 1
      ReDim DAData(NumPoints)
      MemHandle = cbWinBufAlloc(NumPoints)      ' set aside memory to hold data
      If MemHandle = 0 Then Stop
      FSCount& = 2 ^ DAResolution
      ValueStep& = FSCount& / (NumAOChans + 1)
      For i& = 0 To NumPoints - 1
         StepCount& = ValueStep& * (i& + 1)
         If StepCount& > 32767 Then _
            StepCount& = StepCount& - 65536
         DAData(i&) = StepCount&
      Next i&
      
      FirstPoint = 0
      ULStat& = cbWinArrayToBuf(DAData(0), MemHandle, FirstPoint, CBCount&)
      lblInstruction.Caption = "Board " & Format(BoardNum, "0") & _
          " generating analog output on up to " & Format(NumAOChans, "0") _
          & " channels using cbAOutScan() " & _
          " at a Range of " & GetRangeString(CBRange) & "."
   End If

End Sub

Private Sub cmdSendData_Click()

   Dim HighChan As Long, CBRate As Long
   Dim Options As Long, i As Long
   Dim VoltValue As Single
   
   ' Parameters:
   '   BoardNum    :the number used by CB.CFG to describe this board
   '   LowChan&    :the lower channel of the scan
   '   HighChan&   :the upper channel of the scan
   '   CBCount&    :the number of D/A values to send
   '   CBRate&     :send rate in values/second (if supported by BoardNum)
   '   DAData     :array of values to send to the scanned channels
   '   Options&    :data send options
  
   FirstPoint = 0
   LowChan& = 0         'First analog output channel
   HighChan& = MaxChan  'Last analog output channel
   CBRate& = 100        'Rate of data update (ignored if board
                        'does not support timed analog output)
   Options& = DEFAULTOPTION

   ULStat& = cbAOutScan(BoardNum&, LowChan&, HighChan&, _
      NumPoints, CBRate&, CBRange, MemHandle, Options&)
   If ULStat& <> 0 Then Stop

   For i& = 0 To HighChan&
      lblAOutData(i&).Caption = Format$(DAData(i&), "0")
      VoltValue! = ConvertToVolts(DAData(i&))
      lblAOutVolts(i&).Caption = Format$(VoltValue!, "0.000V")
   Next i&
   For i& = HighChan& + 1 To 3
      lblAOutData(i&).Caption = ""
   Next i&

End Sub

Private Function ConvertToVolts(ByVal DataVal As Integer) As Single

   Dim LSBVal As Single, FSVolts As Single
   Dim LongVal As Long, OutVal As Single
   
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

   ULStat& = cbWinBufFree(MemHandle)      ' Free up memory for use by
                                            ' other programs
   End

End Sub
