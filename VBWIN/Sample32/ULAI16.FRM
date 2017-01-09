VERSION 5.00
Begin VB.Form frmDataDisplay 
   Appearance      =   0  'Flat
   BackColor       =   &H80000005&
   Caption         =   "Universal Library Analog Input Scan"
   ClientHeight    =   5220
   ClientLeft      =   2850
   ClientTop       =   1620
   ClientWidth     =   5235
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
   ScaleHeight     =   5220
   ScaleWidth      =   5235
   Begin VB.Frame Frame2 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Gain Adjustment"
      ForeColor       =   &H80000008&
      Height          =   1815
      Left            =   240
      TabIndex        =   8
      Top             =   2640
      Width           =   4215
      Begin VB.Label lblGainAdjustmentFactor 
         BackColor       =   &H00FFFFFF&
         ForeColor       =   &H00FF0000&
         Height          =   255
         Left            =   2400
         TabIndex        =   14
         Top             =   1320
         Width           =   1455
      End
      Begin VB.Label lblGainFactor 
         BackColor       =   &H00FFFFFF&
         Caption         =   "Gain Adjustment Factor:"
         Height          =   255
         Left            =   240
         TabIndex        =   13
         Top             =   1320
         Width           =   2055
      End
      Begin VB.Label lblGainMeasStrain 
         BackColor       =   &H00FFFFFF&
         ForeColor       =   &H00FF0000&
         Height          =   255
         Left            =   1800
         TabIndex        =   12
         Top             =   960
         Width           =   2055
      End
      Begin VB.Label lblGainMeas 
         BackColor       =   &H00FFFFFF&
         Caption         =   "Measured Strain:"
         Height          =   255
         Left            =   240
         TabIndex        =   11
         Top             =   960
         Width           =   1455
      End
      Begin VB.Label lblGainSimStrain 
         BackColor       =   &H00FFFFFF&
         ForeColor       =   &H00FF0000&
         Height          =   255
         Left            =   1800
         TabIndex        =   10
         Top             =   600
         Width           =   2055
      End
      Begin VB.Label lblGainSim 
         BackColor       =   &H00FFFFFF&
         Caption         =   "Simulated Strain:"
         Height          =   255
         Left            =   240
         TabIndex        =   9
         Top             =   600
         Width           =   1455
      End
   End
   Begin VB.TextBox txtChan 
      Appearance      =   0  'Flat
      Height          =   285
      Left            =   2400
      TabIndex        =   4
      Text            =   "0"
      Top             =   960
      Width           =   495
   End
   Begin VB.CommandButton cmdStopConvert 
      Appearance      =   0  'Flat
      Caption         =   "Quit"
      Height          =   390
      Left            =   4200
      TabIndex        =   1
      Top             =   4680
      Width           =   870
   End
   Begin VB.CommandButton cmdStart 
      Appearance      =   0  'Flat
      Caption         =   "Start"
      Height          =   390
      Left            =   3120
      TabIndex        =   2
      Top             =   4680
      Width           =   870
   End
   Begin VB.Frame Frame1 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Offset Adjustment"
      ForeColor       =   &H80000008&
      Height          =   975
      Left            =   240
      TabIndex        =   5
      Top             =   1560
      Width           =   4215
      Begin VB.Label lblOffsetMeasStrain 
         BackColor       =   &H00FFFFFF&
         ForeColor       =   &H00FF0000&
         Height          =   255
         Left            =   1800
         TabIndex        =   7
         Top             =   480
         Width           =   2055
      End
      Begin VB.Label lblOffset 
         BackColor       =   &H80000005&
         Caption         =   "Measured Strain:"
         Height          =   255
         Left            =   240
         TabIndex        =   6
         Top             =   480
         Width           =   1575
      End
   End
   Begin VB.Label lblChan 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Channel:"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   360
      TabIndex        =   3
      Top             =   975
      Width           =   2055
   End
   Begin VB.Label lblDemoFunction 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Demonstration of the bridge nulling and shunt calibration procedure for a specified channel  "
      ForeColor       =   &H80000008&
      Height          =   615
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
'ULAI16.VBP================================================================

' File:                         ULAI16.VBP

' Library Call Demonstrated:    cbAInScan(), SHUNTCAL mode

' Purpose:                      Executes the bridge nulling and shunt calibration
'                               procedure for a specified channel

' Demonstration:                Displays the offset and gain adjustment factors.

' Other Library Calls:          cbErrHandling()

' Special Requirements:         Board 0 must support bridge measurement and
'                               the shunt resistor is connected between
'                               AI+ and Ex- internally

' (c) Copyright 2005-2011, Measurement Computing Corp.
' All rights reserved.
'==========================================================================
Option Explicit

Const BoardNum As Long = 0       ' Board number
Const NumPoints As Long = 1000   ' Number of data points to collect
Const FirstPoint As Long = 0     ' set first element in buffer to transfer to array
Dim ADData() As Double           ' declare an array to hold scaled input values
Dim MemHandle As Long            ' define a variable to contain the handle for
                                 ' memory allocated by Windows through cbScaledWinBufAlloc()
Private Enum StrainConfig
   FullBridgeI = 0
   FullBridgeII = 1
   FullBridgeIII = 2
   HalfBridgeI = 3
   HalfBridgeII = 4
   QuarterBridgeI = 5
   QuarterBridgeII = 6
End Enum

Dim ULStat As Long

Private Sub Form_Load()

   Dim ChannelType As Long
   Dim DefaultTrig As Long

   ' declare revision level of Universal Library
   ULStat = cbDeclareRevision(CURRENTREVNUM)
   
   ' Initiate error handling
   '  activating error handling will trap errors like
   '  bad channel numbers and non-configured conditions.

   '  Parameters:
   '     PRINTALL    :all warnings and errors encountered will be printed
   '     DONTSTOP    :if an error is encountered, the program will not stop,
   '                  errors must be handled locally
  
   ULStat& = cbErrHandling(PRINTALL, DONTSTOP)
   If ULStat <> 0 Then Stop

   ' If cbErrHandling is set for STOPALL or STOPFATAL during the program
   ' design stage, Visual Basic will be unloaded when an error is encountered.
   ' We suggest trapping errors locally until the program is ready for compiling
   ' to avoid losing unsaved data during program design.  This can be done by
   ' setting cbErrHandling options as above and checking the value of ULStat
   ' after a call to the library. If it is not equal to 0, an error has occurred.

   ReDim ADData(NumPoints - 1)
   MemHandle = cbScaledWinBufAlloc(NumPoints)
   If MemHandle = 0 Then Stop

End Sub

Private Sub cmdStart_Click()

   Dim StrainConfiguration As StrainConfig
   Dim InitialVoltage As Double, VOffset As Double
   Dim RGage As Double, RShunt As Double
   Dim VExcitation As Double, GageFactor As Double
   Dim PoissonRatio As Double, MeasuredStrain As Double
   Dim SimulatedStrain As Double, VInitial As Double
   Dim Chan As Long, CBCount As Long, CBRate As Long
   Dim Options As Long, Gain As Long, I As Long
   Dim Total As Double, OffsetAdjustmentFactor As Double
   Dim VActualBridge As Double, REffective As Double
   Dim VSimulatedBridge As Double, GainAdjustmentFactor As Double
   
   cmdStart.Enabled = False
   
   StrainConfiguration = StrainConfig.QuarterBridgeI

   InitialVoltage# = 0  'Bridge output voltage in the unloaded condition.
                        'This value is subtracted from any measurements
                        'before scaling equations are applied.
   VInitial# = 0
   VOffset# = 0
   RGage# = 350
   RShunt# = 100000
   VExcitation# = 2.5
   GageFactor# = 2
   PoissonRatio# = 0
   MeasuredStrain# = 0
   SimulatedStrain# = 0

   ' Calculate the offset adjusment factor on a resting gage in software
   
   ' Collect the values with cbAInScan()
   ' Parameters:
   '   BoardNum   :the number used by CB.CFG to describe this board
   '   LowChan    :the first channel of the scan
   '   HighChan   :the last channel of the scan
   '   CBCount&   :the total number of A/D samples to collect
   '   CBRate&    :sample rate
   '   Gain       :the gain for the board
   '   ADData     :the array for the collected data values
   '   Options    :data collection options
   
   Chan = Val(txtChan.Text) 'channel to acquire
   If (Chan > 3) Then Chan = 3
   txtChan.Text = Format(Chan, "0")
   
   VInitial# = InitialVoltage# / VExcitation#

   CBCount& = NumPoints&            ' total number of data points to collect
   CBRate& = 1000                   ' sampling rate (samples per second)
   Options = SCALEDATA
   Gain = NOTUSED                   ' set the gain
   
   If MemHandle& = 0 Then Stop      ' check that a handle to a memory buffer exists
   
   ULStat = cbAInScan(BoardNum, Chan, Chan, CBCount&, CBRate&, Gain, MemHandle&, Options)
   
   ' Transfer the data from the memory buffer set up
   ' by Windows to an array for use by this program
   
   ULStat = cbScaledWinBufToArray(MemHandle&, ADData#(0), FirstPoint&, CBCount&)
   If ULStat <> 0 Then Stop
 
   For I = 0 To NumPoints& - 1
         Total# = Total# + ADData#(I)
   Next I
    
   VOffset# = Total# / CBCount&

   VOffset# = VOffset# - VInitial#

   OffsetAdjustmentFactor# = CalculateStrain _
      (StrainConfiguration, VOffset#, GageFactor#, PoissonRatio#)

   lblOffsetMeasStrain = Format$(OffsetAdjustmentFactor#, "0.000000000")

   ' Enable Shunt Calibration Circuit and Collect the values and
   ' Calculate the Actual Bridge Voltage

   Options = SCALEDATA + SHUNTCAL
   ULStat = cbAInScan(BoardNum, Chan, Chan, CBCount&, CBRate&, Gain, MemHandle&, Options)

   ' Transfer the data from the memory buffer set up by
   ' Windows to an array for use by this program
   
   ULStat = cbScaledWinBufToArray(MemHandle&, ADData#(0), FirstPoint&, CBCount&)
   If ULStat <> 0 Then Stop
    
   Total# = 0
 
   For I = 0 To NumPoints& - 1
         Total# = Total# + ADData#(I)
   Next I

   VActualBridge# = Total# / NumPoints&

   VActualBridge# = VActualBridge# - VInitial#

   MeasuredStrain# = CalculateStrain _
      (StrainConfiguration, VActualBridge#, GageFactor#, PoissonRatio#)

   lblGainMeasStrain = Format$(MeasuredStrain#, "0.000000000")

   ' Calculate the Simulated Bridge Strain with a shunt resistor

   REffective# = (RGage# * RShunt#) / (RGage# + RShunt#)

   VSimulatedBridge# = (REffective# / (REffective# + RGage#) - 0.5)

   SimulatedStrain# = CalculateStrain _
      (StrainConfiguration, VSimulatedBridge#, GageFactor#, PoissonRatio#)

   lblGainSimStrain = Format$(SimulatedStrain#, "0.000000000")
   
   If MeasuredStrain# - OffsetAdjustmentFactor# <> 0 Then
       GainAdjustmentFactor# = SimulatedStrain# / (MeasuredStrain# - OffsetAdjustmentFactor#)
   Else
       GainAdjustmentFactor# = 1
   End If

   lblGainAdjustmentFactor = Format$(GainAdjustmentFactor#, "0.000000000")

    cmdStart.Enabled = True

End Sub

Private Function CalculateStrain(ByVal StrainCfg As StrainConfig, _
ByVal U As Double, ByVal GageFactor As Double, ByVal PoissonRatio As Double) As Double

   Dim strain As Double
   
   strain = 0
   Select Case StrainCfg
       Case StrainConfig.FullBridgeI
           strain = (-U) / GageFactor
       Case StrainConfig.FullBridgeII
           strain = (-2 * U) / (GageFactor * (1 + PoissonRatio))
       Case StrainConfig.FullBridgeIII
           strain = (-2 * U) / (GageFactor * _
            ((PoissonRatio + 1) - (U * (PoissonRatio - 1))))
       Case StrainConfig.HalfBridgeI
           strain = (-4 * U) / (GageFactor * _
            ((PoissonRatio + 1) - 2 * U * (PoissonRatio - 1)))
       Case StrainConfig.HalfBridgeII
           strain = (-2 * U) / GageFactor
       Case StrainConfig.QuarterBridgeI, StrainConfig.QuarterBridgeII
           strain = (-4 * U) / (GageFactor * ((1 + 2 * U)))
   End Select
   
   CalculateStrain# = strain

End Function

Private Sub cmdStopConvert_Click()
   
   ' Free up memory for use by other programs
   ULStat = cbWinBufFree(MemHandle)
   If ULStat <> 0 Then Stop
   
   End
   
End Sub

