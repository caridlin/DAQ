VERSION 5.00
Begin VB.Form frmListBoards 
   Appearance      =   0  'Flat
   BackColor       =   &H80000005&
   Caption         =   "Universal Library List Boards"
   ClientHeight    =   4740
   ClientLeft      =   105
   ClientTop       =   1545
   ClientWidth     =   5385
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
   ScaleHeight     =   19.75
   ScaleMode       =   4  'Character
   ScaleWidth      =   44.875
   Begin VB.CommandButton cmdQuit 
      Appearance      =   0  'Flat
      Caption         =   "&Quit"
      Height          =   375
      Left            =   4320
      TabIndex        =   2
      Top             =   4200
      Width           =   975
   End
   Begin VB.CommandButton cmdListSupported 
      Appearance      =   0  'Flat
      Caption         =   "Supported Boards"
      Height          =   372
      Left            =   2100
      TabIndex        =   1
      Top             =   4200
      Width           =   2115
   End
   Begin VB.CommandButton cmdListInstalled 
      Appearance      =   0  'Flat
      Caption         =   "Installed Boards"
      Height          =   372
      Left            =   120
      TabIndex        =   0
      Top             =   4200
      Width           =   1875
   End
   Begin VB.TextBox txtListBoards 
      Appearance      =   0  'Flat
      ForeColor       =   &H00FF0000&
      Height          =   3852
      Left            =   120
      MultiLine       =   -1  'True
      ScrollBars      =   2  'Vertical
      TabIndex        =   3
      Top             =   120
      Width           =   5175
   End
End
Attribute VB_Name = "frmListBoards"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
'ULGT04.VBP*****************************************************************

'File:                         ULGT04.VBP

'Library Call Demonstrated:    cbGetBoardName()

'Purpose:                      Prints a list of all boards installed in
'                              the system.  Prints a list of all supported
'                              boards.

'Other Library Calls:          cbErrHandling(), cbGetConfig()

'Special Requirements:         ---

'(c) Copyright 2005-2011, Measurement Computing Corp.
'All rights reserved.
'***************************************************************************
Option Explicit

Dim NumBoards As Long
Dim ULStat As Long

Private Sub Form_Load()

   Dim InfoType As Long, BoardNum As Long, DevNum As Long
   Dim ConfigItem As Long, ConfigVal As Long
   
   ' declare revision level of Universal Library

   ULStat = cbDeclareRevision(CURRENTREVNUM)
   
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

   'Get the maximum number of boards  that can be installed in system
   InfoType = GLOBALINFO
   BoardNum = 0
   DevNum = 0
   ConfigItem = GINUMBOARDS

   ULStat = cbGetConfig(InfoType, BoardNum, DevNum, ConfigItem, ConfigVal)
   NumBoards = ConfigVal
   
End Sub

Private Sub cmdListInstalled_Click()
   
   Dim typeVal As Long, BoardNum As Long
   Dim BoardName As String
   Dim StringSize As Long

   'Get board type of each board currently installed
   txtListBoards.Text = "Currently installed boards:" & vbCrLf & vbCrLf
   For BoardNum = 0 To NumBoards - 1
      ULStat = cbGetConfig(BOARDINFO, BoardNum, 0, BIBOARDTYPE, typeVal)
      If typeVal <> 0 Then
         BoardName = Space$(BOARDNAMELEN)
         ULStat = cbGetBoardName(BoardNum, BoardName)
         'Drop the null character and white space at end of string
         StringSize = InStr(1, BoardName, Chr(0)) - 1
         BoardName = Left$(BoardName, StringSize)
         txtListBoards.Text = txtListBoards.Text & _
            "Board #" & Format$(BoardNum, "0") & _
            "= " & BoardName & vbCrLf
      End If
   Next

End Sub

Private Sub cmdListSupported_Click()
   
   Dim BoardNum As Long
   Dim BoardName As String, BoardList As String
   Dim StringSize As Long
   
   txtListBoards.Text = ""
   
   'Get the first board type in list of supported boards
   'The first string in the boardlist is "Not Supported"
   BoardName = Space$(BOARDNAMELEN)
   
   BoardNum = GETFIRST
   ULStat = cbGetBoardName(BoardNum, BoardName)
   'Drop the null character and white space at end of string
   StringSize = InStr(1, BoardName, Chr(0)) - 1
   BoardName = Left$(BoardName, StringSize)
   BoardList = "The first string in the board name list is:" _
      & vbCrLf & BoardName & vbCrLf & vbCrLf

   'Get each consecutive board type in list
   Do
      BoardName = Space$(BOARDNAMELEN)
      BoardNum = GETNEXT
      ULStat = cbGetBoardName(BoardNum, BoardName)
      'Drop the null character and white space at end of string
      StringSize = InStr(1, BoardName, Chr(0)) - 1
      BoardName = Left$(BoardName, StringSize)
      BoardList = BoardList & BoardName & vbCrLf
   Loop While Len(BoardName) > 3
   txtListBoards.Text = BoardList

End Sub

Private Sub cmdQuit_Click()

   End

End Sub

