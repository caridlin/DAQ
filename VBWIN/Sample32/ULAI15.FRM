VERSION 5.00
Begin VB.Form frmDataDisplay 
   Appearance      =   0  'Flat
   BackColor       =   &H80000005&
   Caption         =   "Universal Library Analog Input Scan"
   ClientHeight    =   4290
   ClientLeft      =   2850
   ClientTop       =   1620
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
   ScaleHeight     =   4290
   ScaleWidth      =   5355
   Begin VB.TextBox txtHighChan 
      Appearance      =   0  'Flat
      Height          =   285
      Left            =   2940
      TabIndex        =   20
      Text            =   "0"
      Top             =   1860
      Width           =   495
   End
   Begin VB.CommandButton cmdStopConvert 
      Appearance      =   0  'Flat
      Caption         =   "Quit"
      Height          =   390
      Left            =   4200
      TabIndex        =   17
      Top             =   3780
      Width           =   870
   End
   Begin VB.CommandButton cmdStart 
      Appearance      =   0  'Flat
      Caption         =   "Start"
      Height          =   390
      Left            =   3120
      TabIndex        =   18
      Top             =   3780
      Width           =   870
   End
   Begin VB.Label lblInstruction 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Board 0 must have analog inputs that support paced acquisition."
      ForeColor       =   &H000000FF&
      Height          =   915
      Left            =   420
      TabIndex        =   21
      Top             =   720
      Width           =   4455
   End
   Begin VB.Label Label1 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Measure Channels 0 to"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   840
      TabIndex        =   19
      Top             =   1860
      Width           =   2055
   End
   Begin VB.Label lblADData 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   7
      Left            =   3960
      TabIndex        =   16
      Top             =   3390
      Width           =   1335
   End
   Begin VB.Label lblChan7 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Channel 7:"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   2880
      TabIndex        =   8
      Top             =   3390
      Width           =   975
   End
   Begin VB.Label lblADData 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   3
      Left            =   1440
      TabIndex        =   12
      Top             =   3390
      Width           =   1335
   End
   Begin VB.Label lblChan3 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Channel 3:"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   360
      TabIndex        =   4
      Top             =   3390
      Width           =   975
   End
   Begin VB.Label lblADData 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   6
      Left            =   3960
      TabIndex        =   15
      Top             =   3015
      Width           =   1335
   End
   Begin VB.Label lblChan6 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Channel 6:"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   2880
      TabIndex        =   7
      Top             =   3015
      Width           =   975
   End
   Begin VB.Label lblADData 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   2
      Left            =   1440
      TabIndex        =   11
      Top             =   3015
      Width           =   1335
   End
   Begin VB.Label lblChan2 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Channel 2:"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   360
      TabIndex        =   3
      Top             =   3015
      Width           =   975
   End
   Begin VB.Label lblADData 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   5
      Left            =   3960
      TabIndex        =   14
      Top             =   2625
      Width           =   1335
   End
   Begin VB.Label lblChan5 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Channel 5:"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   2880
      TabIndex        =   6
      Top             =   2625
      Width           =   975
   End
   Begin VB.Label lblADData 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   1
      Left            =   1440
      TabIndex        =   10
      Top             =   2625
      Width           =   1335
   End
   Begin VB.Label lblChan1 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Channel 1:"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   360
      TabIndex        =   2
      Top             =   2625
      Width           =   975
   End
   Begin VB.Label lblADData 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   4
      Left            =   3960
      TabIndex        =   13
      Top             =   2250
      Width           =   1335
   End
   Begin VB.Label lblChan4 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Channel 4:"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   2880
      TabIndex        =   5
      Top             =   2250
      Width           =   975
   End
   Begin VB.Label lblADData 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   0
      Left            =   1440
      TabIndex        =   9
      Top             =   2250
      Width           =   1335
   End
   Begin VB.Label lblChan0 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Channel 0:"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   360
      TabIndex        =   1
      Top             =   2250
      Width           =   975
   End
   Begin VB.Label lblDemoFunction 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Demonstration of cbAInScan() with scan option set to SCALEDATA"
      ForeColor       =   &H80000008&
      Height          =   495
      Left            =   120
      TabIndex        =   0
      Top             =   120
      Width           =   5055
   End
End
Attribute VB_Name = "frmDataDisplay"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
'ULAI15.VBP================================================================

' File:                         ULAI15.VBP

' Library Call Demonstrated:    cbAInScan() with scan option set to SCALEDATA

' Purpose:                      Scans a range of A/D Input Channels and
'                               stores the sample data in an array.

' Demonstration:                Displays the analog input on eight channels.

' Other Library Calls:          cbErrHandling()

' Special Requirements:         Board 0 must have an A/D converter.
'                               Analog signals on eight input channels.

' (c) Copyright 2005-2011, Measurement Computing Corp.
' All rights reserved.
'==========================================================================
Option Explicit

Const BoardNum As Long = 0       ' Board number

Dim CBRange As Long
Dim ADResolution As Long, NumAIChans As Long
Dim HighChan As Long, LowChan As Long, MaxChan As Long

Const NumPoints As Long = 600    ' Number of data points to collect
Const FirstPoint As Long = 0     ' set first element in buffer to transfer to array
Dim ADData() As Double           ' dimension an array to hold the input values
Dim MemHandle As Long            ' define a variable to contain the handle for
                                 ' memory allocated by Windows through cbWinBufAlloc()
Dim ULStat As Long

Private Sub Form_Load()

   Dim ChannelType As Long
   Dim DefaultTrig As Long
   Dim ReportError As Long, HandleError As Long
   
   ' declare revision level of Universal Library
   ULStat = cbDeclareRevision(CURRENTREVNUM)
   
   ' Initiate error handling
   '  activating error handling will trap errors like
   '  bad channel numbers and non-configured conditions.

   '  Parameters:
   '     PRINTALL    :all warnings and errors encountered will be printed
   '     DONTSTOP    :if an error is encountered, the program will not stop,
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

   ' determine the number of analog channels and their capabilities
   ChannelType = ANALOGINPUT
   NumAIChans = FindAnalogChansOfType(BoardNum, ChannelType, _
      ADResolution, CBRange, LowChan, DefaultTrig)

   If (NumAIChans = 0) Then
      lblInstruction.Caption = "Board " & Format(BoardNum, "0") & _
         " does not have analog input channels."
      cmdStart.Enabled = False
      txtHighChan.Enabled = False
   Else
      ' set aside memory to hold data
      ReDim ADData(NumPoints - 1)
      MemHandle = cbScaledWinBufAlloc(NumPoints)
      If MemHandle = 0 Then Stop
      If (NumAIChans > 8) Then NumAIChans = 8 'limit to 8 for display
      MaxChan = NumAIChans - 1
      lblInstruction.Caption = "Board " & Format(BoardNum, "0") & _
         " collecting analog data on up to " & Format(NumAIChans, "0") & _
         " channels using AInScan with Range set to " & GetRangeString(CBRange) _
         & " and converting the data using the SCALEDATA option."
   End If

End Sub

Private Sub cmdStart_Click()

   Dim Options As Long
   Dim CBRate As Long
   Dim i As Integer, j As Integer
   
   cmdStart.Enabled = False
   
   ' Collect the values with cbAInScan()
   ' Parameters:
   '   BoardNum   :the number used by CB.CFG to describe this board
   '   LowChan    :the first channel of the scan
   '   HighChan   :the last channel of the scan
   '   CBCount&   :the total number of A/D samples to collect
   '   CBRate&    :sample rate
   '   CBRange    :the gain for the board
   '   ADData     :the array for the collected data values
   '   Options    :data collection options
   
   HighChan = Val(txtHighChan.Text) ' last channel to acquire
   If (HighChan > MaxChan) Then HighChan = MaxChan
   txtHighChan.Text = Format(HighChan, "0")

   ' per channel sampling rate ((samples per second) per channel)
   CBRate = 1000 / ((HighChan - LowChan) + 1)
   Options = SCALEDATA              ' return scale data
   
   ULStat = cbAInScan(BoardNum, LowChan, HighChan, _
      NumPoints, CBRate, CBRange, MemHandle, Options)
   
   If ULStat <> 0 And ULStat <> 91 Then Stop
   
   ' Transfer the data from the memory buffer set up by Windows
   ' to an array for use by this program
   
   ULStat = cbScaledWinBufToArray(MemHandle, ADData(0), FirstPoint, NumPoints)
   If ULStat <> 0 Then Stop
   
   For i = 0 To HighChan
      lblADData(i).Caption = Format$(ADData(i), "0.00000000")
   Next i

   For j = HighChan + 1 To 7
        lblADData(j).Caption = ""
   Next j

   cmdStart.Enabled = True

End Sub

Private Sub cmdStopConvert_Click()
   
   ' Free up memory for use by other programs
   ULStat = cbWinBufFree(MemHandle)
   If ULStat <> 0 Then Stop
   
   End
   
End Sub

