VERSION 5.00
Begin VB.Form frmInfoDisplay 
   Appearance      =   0  'Flat
   BackColor       =   &H80000005&
   Caption         =   "Universal Library Configuration Info"
   ClientHeight    =   5055
   ClientLeft      =   105
   ClientTop       =   1545
   ClientWidth     =   6480
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
   ScaleHeight     =   5055
   ScaleWidth      =   6480
   Begin VB.CommandButton cmdQuit 
      Appearance      =   0  'Flat
      Caption         =   "Quit"
      Height          =   375
      Left            =   5400
      TabIndex        =   1
      Top             =   4560
      Width           =   855
   End
   Begin VB.CommandButton cmdPrintInfo 
      Appearance      =   0  'Flat
      Caption         =   "Print Info"
      Default         =   -1  'True
      Height          =   390
      Left            =   2400
      TabIndex        =   0
      Top             =   4560
      Width           =   1140
   End
   Begin VB.TextBox txtBoardInfo 
      Appearance      =   0  'Flat
      ForeColor       =   &H00FF0000&
      Height          =   4335
      Left            =   240
      MultiLine       =   -1  'True
      ScrollBars      =   2  'Vertical
      TabIndex        =   2
      Top             =   120
      Width           =   6015
   End
End
Attribute VB_Name = "frmInfoDisplay"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
'ULGT03.VBP****************************************************************

'File:                         ULGT03.VBP

'Library Call Demonstrated:    cbGetConfig()

'Purpose:                      Prints a list of all boards installed in
'                              the system and their base addresses.  Also
'                              prints the addresses of each digital and
'                              counter device on each board and any EXP
'                              boards that are connected to A/D channels.

'Other Library Calls:          cbErrHandling(), cbGetBoardName()

'Special Requirements:         ---

'(c) Copyright 2005-2011, Measurement Computing Corp.
'All rights reserved.
'***************************************************************************
Option Explicit

Dim BoardNum As Long

Dim MaxNumBoards As Long, CurrentBoard As Long
Dim NumBoards As Long
Dim Info As String
Dim ULStat As Long

Private Sub Form_Load()
   
   Dim InfoType As Long, DevNum As Long
   Dim ConfigItem As Long, ConfigVal As Long
   
   ' declare revision level of Universal Library
   ULStat = cbDeclareRevision(CURRENTREVNUM)
   
   InfoType& = GLOBALINFO
   BoardNum = 0
   DevNum& = 0
   ConfigItem& = GINUMBOARDS

   'Get the number of boards installed in system
   ULStat = cbGetConfig(InfoType&, BoardNum, DevNum&, ConfigItem&, ConfigVal&)
   MaxNumBoards = ConfigVal&
   CurrentBoard = 0
   txtBoardInfo.Text = vbCrLf & vbCrLf & Space$(12) & _
      "Click on 'Print Info' to display board information."

   NumBoards = 0
   CurrentBoard = 0
   
End Sub

Private Sub cmdPrintInfo_Click()
   
   Dim InfoType As Long
   Dim DevNum As Long, ConfigItem As Long, ConfigVal As Long
   
   InfoType& = BOARDINFO
   DevNum& = 0
   ConfigItem& = BIBOARDTYPE
   
   'Loop through possible board numbers. If installed,
   '(ConfigVal& <> 0), get the board information.
   Do
      ULStat = cbGetConfig(InfoType&, CurrentBoard, DevNum&, ConfigItem&, ConfigVal&)
      CurrentBoard = CurrentBoard + 1
   Loop While (ConfigVal& = 0) And (CurrentBoard < MaxNumBoards)
   
   If CurrentBoard > MaxNumBoards - 1 Then
      If NumBoards = 0 Then
         Info = vbCrLf & vbCrLf & Space$(22) & _
            "There are no boards installed." & vbCrLf & vbCrLf
         Info = Info & Space$(12) & _
            "You must run InstaCal to install the desired" & vbCrLf
         Info = Info & Space$(18) & "boards before running this program."
      Else
         Info = vbCrLf & vbCrLf & Space$(16) & _
            "There are no additional boards installed."
      End If
      cmdPrintInfo.Caption = "Print Info"
      CurrentBoard = 0
      NumBoards = 0
   Else
      Info = ""
      BoardNum& = CurrentBoard - 1
      PrintGenInfo BoardNum&
      PrintADInfo BoardNum&
      PrintDAInfo BoardNum&
      PrintDigInfo BoardNum&
      PrintCtrInfo BoardNum&
      PrintExpInfo BoardNum&
      cmdPrintInfo.Caption = "Print Next"
      NumBoards = NumBoards + 1
   End If
   txtBoardInfo.Text = Info

End Sub

Private Sub PrintGenInfo(ByVal BoardNum As Long)

   Dim BoardName As String
   Dim StringSize As Long, ConfigItem As Long
   Dim InfoType As Long, DevNum As Long
   Dim ConfigVal As Long, BaseAdrStr As String
   
   'Get the board's name
   BoardName = Space$(BOARDNAMELEN)
   ULStat = cbGetBoardName(BoardNum, BoardName)
   StringSize& = InStr(1, BoardName, Chr(0)) - 1
   BoardName = Left$(BoardName, StringSize)   'remove chars from null to end
   Info = Info & "Board #" & Format$(BoardNum, "0") & " = " & BoardName & " at "
          
   'Get the board's base address
   ConfigItem& = BIBASEADR
   ULStat = cbGetConfig(InfoType&, BoardNum, DevNum&, ConfigItem&, ConfigVal&)
          
   BaseAdrStr$ = Hex$(ConfigVal&)
   Info = Info & "Base Address = " & BaseAdrStr$ & " hex." & vbCrLf & vbCrLf

End Sub

Private Sub PrintADInfo(ByVal BoardNum As Long)

   Dim InfoType As Long, DevNum As Long
   Dim ConfigItem As Long, ConfigVal As Long
   
   InfoType& = BOARDINFO
   DevNum& = 0
   ConfigItem& = BINUMADCHANS

   ULStat = cbGetConfig(InfoType&, BoardNum, DevNum&, ConfigItem&, ConfigVal&)
    
   If Not (ConfigVal& = 0) Then Info = Info & Space$(5) & "Number of A/D channels: " _
      & Format$(ConfigVal&, "0") & vbCrLf & vbCrLf

End Sub

Private Sub PrintDAInfo(ByVal BoardNum As Long)

   Dim InfoType As Long, DevNum As Long
   Dim ConfigItem As Long, ConfigVal As Long
   Dim NumDAChans As Long
   
   InfoType& = BOARDINFO
   DevNum& = 0
   ConfigItem& = BINUMDACHANS
   
   ULStat = cbGetConfig(InfoType&, BoardNum, DevNum&, ConfigItem&, ConfigVal&)
    
   NumDAChans = ConfigVal&
   If Not (ConfigVal& = 0) Then Info = Info & Space$(5) & "Number of D/A channels: " _
      & Format$(ConfigVal&, "0") & vbCrLf & vbCrLf

End Sub

Private Sub PrintDigInfo(ByVal BoardNum As Long)

   Dim InfoType As Long, DevNum As Long
   Dim ConfigItem As Long, ConfigVal As Long
   Dim NumDevs As Long, NumBits As Long
   
   'get the number of digital devices for this board
   InfoType& = BOARDINFO
   DevNum& = 0
   ConfigItem& = BIDINUMDEVS

   ULStat = cbGetConfig(InfoType&, BoardNum, DevNum&, ConfigItem&, ConfigVal&)
   
   NumDevs& = ConfigVal&
   
   For DevNum& = 0 To NumDevs& - 1
      'For each digital device, get the base address and number of bits
      
      InfoType& = DIGITALINFO
      ConfigItem& = DINUMBITS
      ULStat = cbGetConfig(InfoType&, BoardNum, DevNum&, ConfigItem&, ConfigVal&)
      
      NumBits = ConfigVal&
      Info = Info & Space$(5) & "Digital Device #" & Format$(DevNum&, "0") & _
         " : " & Format$(NumBits, "0") & " bits" & vbCrLf
   Next
   If Len(Info) Then Info = Info & vbCrLf
   
End Sub

Private Sub PrintCtrInfo(ByVal BoardNum As Long)

   Dim InfoType As Long, DevNum As Long
   Dim ConfigItem As Long, ConfigVal As Long
   Dim NumDevs As Long
   
   'Get the number of counter devices for this board
   InfoType& = BOARDINFO
   DevNum& = 0
   ConfigItem& = BICINUMDEVS
   
   ULStat = cbGetConfig(InfoType&, BoardNum, DevNum&, ConfigItem&, ConfigVal&)
   NumDevs& = ConfigVal&
   
   If NumDevs& > 0 Then Info = Info & Space$(5) & "Counters : " & Format$(NumDevs&, "0") & vbCrLf

   If Len(Info) Then Info = Info & vbCrLf
   
End Sub

Private Sub PrintExpInfo(ByVal BoardNum As Long)

   Dim InfoType As Long, DevNum As Long
   Dim ConfigItem As Long, ConfigVal As Long
   Dim NumDevs As Long, BoardType As Long
   Dim ADChan1 As Long, ADChan2 As Long
   
   ' Get the number of Exps attached to BoardNum
   InfoType& = BOARDINFO
   DevNum& = 0
   ConfigItem& = BINUMEXPS
   ULStat = cbGetConfig(InfoType&, BoardNum, DevNum&, ConfigItem&, ConfigVal&)
   NumDevs& = ConfigVal&
         
   For DevNum& = 0 To NumDevs& - 1
      ConfigItem& = XIBOARDTYPE
      InfoType& = EXPANSIONINFO
      ULStat = cbGetConfig(InfoType&, BoardNum, DevNum&, ConfigItem&, ConfigVal&)
      BoardType& = ConfigVal&

      ConfigItem& = XIMUXADCHAN1
      ULStat = cbGetConfig(InfoType&, BoardNum, DevNum&, ConfigItem&, ConfigVal&)
      ADChan1 = ConfigVal&
      If BoardType& = 770 Then
         'it's a CIO-EXP32
         ConfigItem& = XIMUXADCHAN2
         ULStat = cbGetConfig(InfoType&, BoardNum, DevNum&, ConfigItem&, ConfigVal&)
         ADChan2 = ConfigVal&
         Info = Info & Space$(5) & "A/D channels " & Format$(ADChan1, "0") & _
            " and " & Format$(ADChan2, "0") & " connected to EXP(devID=" & Format$(BoardType, 0) & ")." & vbCrLf
      Else
         Info = Info & Space$(5) & "A/D channel " & Format$(ADChan1, "0") & _
            " connected to EXP(devID=" & Format$(BoardType, 0) & ")." & vbCrLf
      End If
   Next
   If Len(Info) Then Info = Info & vbCrLf
   
End Sub

Private Sub cmdQuit_Click()

    End

End Sub

