VERSION 5.00
Begin VB.Form frmPreTrig 
   Appearance      =   0  'Flat
   BackColor       =   &H80000005&
   Caption         =   "Universal Library Analog Input Scan"
   ClientHeight    =   6510
   ClientLeft      =   105
   ClientTop       =   1545
   ClientWidth     =   6150
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
   ScaleHeight     =   6510
   ScaleWidth      =   6150
   Begin VB.CommandButton cmdQuit 
      Appearance      =   0  'Flat
      Caption         =   "Quit"
      Height          =   390
      Left            =   5160
      TabIndex        =   17
      Top             =   6000
      Width           =   780
   End
   Begin VB.CommandButton cmdTrigEnable 
      Appearance      =   0  'Flat
      Caption         =   "Start Pre/Post Trigger background operation"
      Height          =   510
      Left            =   720
      TabIndex        =   18
      Top             =   1800
      Width           =   4575
   End
   Begin VB.Timer tmrCheckStatus 
      Enabled         =   0   'False
      Interval        =   200
      Left            =   5400
      Top             =   1920
   End
   Begin VB.Label lblResult 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00C00000&
      Height          =   615
      Left            =   240
      TabIndex        =   52
      Top             =   5760
      Width           =   4695
   End
   Begin VB.Label lblInstruction 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Board 0 must have analog inputs that support paced acquisition."
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
      Height          =   1095
      Left            =   480
      TabIndex        =   51
      Top             =   480
      Width           =   5235
   End
   Begin VB.Label lblShowCount 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   210
      Left            =   4920
      TabIndex        =   49
      Top             =   5160
      Width           =   510
   End
   Begin VB.Label lblCount 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Count:"
      ForeColor       =   &H80000008&
      Height          =   210
      Left            =   4050
      TabIndex        =   46
      Top             =   5160
      Width           =   825
   End
   Begin VB.Label lblShowIndex 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   210
      Left            =   3210
      TabIndex        =   50
      Top             =   5160
      Width           =   615
   End
   Begin VB.Label lblIndex 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Index:"
      ForeColor       =   &H80000008&
      Height          =   210
      Left            =   2400
      TabIndex        =   48
      Top             =   5160
      Width           =   825
   End
   Begin VB.Label lblShowStat 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00FF0000&
      Height          =   210
      Left            =   1200
      TabIndex        =   47
      Top             =   5160
      Width           =   1050
   End
   Begin VB.Label lblStatus 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Status:"
      ForeColor       =   &H80000008&
      Height          =   210
      Left            =   480
      TabIndex        =   45
      Top             =   5160
      Width           =   705
   End
   Begin VB.Label lblPostTrig 
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
      Height          =   210
      Index           =   10
      Left            =   4635
      TabIndex        =   42
      Top             =   4650
      Width           =   975
   End
   Begin VB.Label lblPost 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Trigger +9"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H00C00000&
      Height          =   210
      Index           =   9
      Left            =   3330
      TabIndex        =   40
      Top             =   4650
      Width           =   1335
   End
   Begin VB.Label lblPreTrig 
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
      Height          =   210
      Index           =   9
      Left            =   1920
      TabIndex        =   22
      Top             =   4650
      Width           =   975
   End
   Begin VB.Label lblPre 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Trigger -1"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H00C00000&
      Height          =   210
      Index           =   0
      Left            =   510
      TabIndex        =   20
      Top             =   4650
      Width           =   1335
   End
   Begin VB.Label lblPostTrig 
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
      Height          =   210
      Index           =   9
      Left            =   4635
      TabIndex        =   41
      Top             =   4470
      Width           =   975
   End
   Begin VB.Label lblPost 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Trigger +8"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H00C00000&
      Height          =   210
      Index           =   8
      Left            =   3330
      TabIndex        =   39
      Top             =   4470
      Width           =   1335
   End
   Begin VB.Label lblPreTrig 
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
      Height          =   210
      Index           =   8
      Left            =   1920
      TabIndex        =   21
      Top             =   4470
      Width           =   975
   End
   Begin VB.Label lblPre 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Trigger -2"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H00C00000&
      Height          =   210
      Index           =   1
      Left            =   510
      TabIndex        =   19
      Top             =   4470
      Width           =   1335
   End
   Begin VB.Label lblPostTrig 
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
      Height          =   210
      Index           =   8
      Left            =   4635
      TabIndex        =   38
      Top             =   4275
      Width           =   975
   End
   Begin VB.Label lblPost 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Trigger +7"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H00C00000&
      Height          =   210
      Index           =   7
      Left            =   3330
      TabIndex        =   37
      Top             =   4275
      Width           =   1335
   End
   Begin VB.Label lblPreTrig 
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
      Height          =   210
      Index           =   7
      Left            =   1920
      TabIndex        =   16
      Top             =   4275
      Width           =   975
   End
   Begin VB.Label lblPre 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Trigger -3"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H00C00000&
      Height          =   210
      Index           =   2
      Left            =   510
      TabIndex        =   8
      Top             =   4275
      Width           =   1335
   End
   Begin VB.Label lblPostTrig 
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
      Height          =   210
      Index           =   7
      Left            =   4635
      TabIndex        =   34
      Top             =   4080
      Width           =   975
   End
   Begin VB.Label lblPost 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Trigger +6"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H00C00000&
      Height          =   210
      Index           =   6
      Left            =   3330
      TabIndex        =   33
      Top             =   4080
      Width           =   1335
   End
   Begin VB.Label lblPreTrig 
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
      Height          =   210
      Index           =   6
      Left            =   1920
      TabIndex        =   15
      Top             =   4080
      Width           =   975
   End
   Begin VB.Label lblPre 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Trigger -4"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H00C00000&
      Height          =   210
      Index           =   3
      Left            =   510
      TabIndex        =   7
      Top             =   4080
      Width           =   1335
   End
   Begin VB.Label lblPostTrig 
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
      Height          =   210
      Index           =   6
      Left            =   4635
      TabIndex        =   30
      Top             =   3885
      Width           =   975
   End
   Begin VB.Label lblPost 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Trigger +5"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H00C00000&
      Height          =   210
      Index           =   5
      Left            =   3330
      TabIndex        =   29
      Top             =   3885
      Width           =   1335
   End
   Begin VB.Label lblPreTrig 
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
      Height          =   210
      Index           =   5
      Left            =   1920
      TabIndex        =   14
      Top             =   3885
      Width           =   975
   End
   Begin VB.Label lblPre 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Trigger -5"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H00C00000&
      Height          =   210
      Index           =   4
      Left            =   510
      TabIndex        =   6
      Top             =   3885
      Width           =   1335
   End
   Begin VB.Label lblPostTrig 
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
      Height          =   210
      Index           =   5
      Left            =   4635
      TabIndex        =   26
      Top             =   3690
      Width           =   975
   End
   Begin VB.Label lblPost 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Trigger +4"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H00C00000&
      Height          =   210
      Index           =   4
      Left            =   3330
      TabIndex        =   25
      Top             =   3690
      Width           =   1335
   End
   Begin VB.Label lblPreTrig 
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
      Height          =   210
      Index           =   4
      Left            =   1920
      TabIndex        =   13
      Top             =   3690
      Width           =   975
   End
   Begin VB.Label lblPre 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Trigger -6"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H00C00000&
      Height          =   210
      Index           =   5
      Left            =   510
      TabIndex        =   5
      Top             =   3690
      Width           =   1335
   End
   Begin VB.Label lblPostTrig 
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
      Height          =   210
      Index           =   4
      Left            =   4635
      TabIndex        =   36
      Top             =   3510
      Width           =   975
   End
   Begin VB.Label lblPost 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Trigger +3"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H00C00000&
      Height          =   210
      Index           =   3
      Left            =   3330
      TabIndex        =   35
      Top             =   3510
      Width           =   1335
   End
   Begin VB.Label lblPreTrig 
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
      Height          =   210
      Index           =   3
      Left            =   1920
      TabIndex        =   12
      Top             =   3510
      Width           =   975
   End
   Begin VB.Label lblPre 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Trigger -7"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H00C00000&
      Height          =   210
      Index           =   6
      Left            =   510
      TabIndex        =   4
      Top             =   3510
      Width           =   1335
   End
   Begin VB.Label lblPostTrig 
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
      Height          =   210
      Index           =   3
      Left            =   4635
      TabIndex        =   32
      Top             =   3315
      Width           =   975
   End
   Begin VB.Label lblPost 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Trigger +2"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H00C00000&
      Height          =   210
      Index           =   2
      Left            =   3330
      TabIndex        =   31
      Top             =   3315
      Width           =   1335
   End
   Begin VB.Label lblPreTrig 
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
      Height          =   210
      Index           =   2
      Left            =   1920
      TabIndex        =   11
      Top             =   3315
      Width           =   975
   End
   Begin VB.Label lblPre 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Trigger -8"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H00C00000&
      Height          =   210
      Index           =   7
      Left            =   510
      TabIndex        =   3
      Top             =   3315
      Width           =   1335
   End
   Begin VB.Label lblPostTrig 
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
      Height          =   210
      Index           =   2
      Left            =   4635
      TabIndex        =   28
      Top             =   3120
      Width           =   975
   End
   Begin VB.Label lblPost 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Trigger +1"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H00C00000&
      Height          =   210
      Index           =   1
      Left            =   3330
      TabIndex        =   27
      Top             =   3120
      Width           =   1335
   End
   Begin VB.Label lblPreTrig 
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
      Height          =   210
      Index           =   1
      Left            =   1920
      TabIndex        =   10
      Top             =   3120
      Width           =   975
   End
   Begin VB.Label lblPre 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Trigger -9"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H00C00000&
      Height          =   210
      Index           =   8
      Left            =   510
      TabIndex        =   2
      Top             =   3120
      Width           =   1335
   End
   Begin VB.Label lblPostTrig 
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
      Height          =   210
      Index           =   1
      Left            =   4635
      TabIndex        =   24
      Top             =   2925
      Width           =   975
   End
   Begin VB.Label lblPost 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Trigger"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H00C00000&
      Height          =   210
      Index           =   0
      Left            =   3330
      TabIndex        =   23
      Top             =   2925
      Width           =   1335
   End
   Begin VB.Label lblPreTrig 
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
      Height          =   210
      Index           =   0
      Left            =   1920
      TabIndex        =   9
      Top             =   2925
      Width           =   975
   End
   Begin VB.Label lblPre 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Trigger -10"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H00C00000&
      Height          =   210
      Index           =   9
      Left            =   510
      TabIndex        =   1
      Top             =   2925
      Width           =   1335
   End
   Begin VB.Label lblPostTrigData 
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
      Height          =   210
      Left            =   3240
      TabIndex        =   44
      Top             =   2640
      Width           =   2580
   End
   Begin VB.Label lblPreTrigData 
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
      Height          =   210
      Left            =   435
      TabIndex        =   43
      Top             =   2640
      Width           =   2655
   End
   Begin VB.Label lblDemoFunction 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Demonstration of cbAPreTrig() in BACKGROUND mode"
      ForeColor       =   &H80000008&
      Height          =   270
      Left            =   165
      TabIndex        =   0
      Top             =   90
      Width           =   5850
   End
End
Attribute VB_Name = "frmPreTrig"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
'ULAI09.VBP================================================================

' File:                         ULAI09.VBP

' Library Call Demonstrated:    cbAConvertPretrigData&()

' Purpose:                      Waits for a trigger with cbAPretrig&(), then
'                               uses cbAConvertPretrigData&() to convert
'                               the data.

' Demonstration:                Displays the analog input on one channel and
'                               waits for the trigger.

' Other Library Calls:          cbAPretrig&()
'                               cbGetStatus&()
'                               cbStopBackground&()
'                               cbErrHandling&()

' Special Requirements:         Board 0 must support cbAPretrig.

'==========================================================================
Option Explicit

Const BoardNum As Long = 0             ' Board number
Dim CBRange As Long
Dim ADResolution As Long, NumAIChans As Long
Dim HighChan As Long, LowChan As Long, MaxChan As Long

Const NumPoints& = 4096         ' Number of data points to collect
Const FirstPoint& = 0           ' set first element in buffer to transfer to array
Const PretrigCount& = 1000      ' number of data points before the trigger to store
Const TotalCount& = NumPoints&  ' total number of data points to collect
Const BufSize& = 4608           ' buffer must be large enough to hold all data

Dim DefaultTrig As Long
Dim MemHandle&                  ' define a variable to contain the handle for
                                ' memory allocated by Windows through cbWinBufAlloc&()
Dim ADData() As Integer         ' dimension an array to hold the input values
                                ' size must be TotalCount + 512 minimum
Dim ChanTags() As Integer       ' dimension an array to hold the channel tag values
Dim ULStat As Long

Private Sub Form_Load()

   Dim ReportError As Long, HandleError As Long
   
   ' declare revision level of Universal Library
   ULStat = cbDeclareRevision(CURRENTREVNUM)
   
   ' Initiate error handling
   ' activating error handling will trap errors like
   ' bad channel numbers and non-configured conditions.
   ' Parameters:
   '   DONTPRINT   :all warnings and errors encountered will be handled locally
   '   DONTSTOP    :if an error is encountered, the program will not stop,
   '                errors must be handled locally
     
   ReportError = DONTPRINT
   HandleError = DONTSTOP
   ULStat = cbErrHandling(ReportError, HandleError)
   If ULStat <> 0 Then Stop
   SetAnalogIODefaults ReportError, HandleError
   
   ' If cbErrHandling& is set for STOPALL or STOPFATAL during the program
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
   ElseIf ADResolution > 16 Then
       lblInstruction.Caption = "Board " & Format(BoardNum, "0") & _
       " is greater than 16-bit resolution. The AConvertPretrigData " & _
       "function does not support high resolution devices."
       cmdTrigEnable.Enabled = False
   Else
       ' set aside memory to hold 16-bit data and channel tags
       ReDim ADData(BufSize)
       ReDim ChanTags(BufSize)
       MemHandle = cbWinBufAlloc(BufSize)
       If MemHandle = 0 Then Stop
       If (NumAIChans > 8) Then NumAIChans = 8 'limit to 8 for display
       MaxChan = LowChan + NumAIChans - 1
       lblInstruction.Caption = "Board " & Format(BoardNum, "0") & _
           " collecting analog data on channel 0 using APretrig " & _
           "in background mode with Range set to " & GetRangeString(CBRange) & _
           ". Data is aligned after acquisition using AConvertPretrigData."
   End If

End Sub

Private Sub cmdTrigEnable_Click()

   Dim ULStat As Long
   Dim Options As Long
   Dim CBRate As Long
   Dim EngUnits As Single
  
   ' Monitor a range of channels for a trigger then collect the values
   ' with cbAPretrig&()
   
   ' Parameters:
   '   BoardNum&      :the number used by CB.CFG to describe this board
   '   LowChan&       :first A/D channel of the scan
   '   HighChan&      :last A/D channel of the scan
   '   PretrigCount&  :number of pre-trigger A/D samples to collect
   '   TotalCount&    :total number of A/D samples to collect
   '   CBRate&        :sample rate in samples per second
   '   CBRange        :the gain for the board
   '   ADData%        :the array for the collected data values
   '   Options        :data collection options

   lblResult.Caption = "Waiting for trigger on trigger input (Ctl-Break to abort)."
   
   HighChan = LowChan
   CBRate = 1000        ' per channel sampling rate ((samples per second) per channel)
   Options = BACKGROUND ' collect data in the background
 
   If DefaultTrig = TRIGABOVE Then
       'The default trigger configuration for most devices is
       'rising edge digital trigger, but some devices do not
       'support this type for pretrigger functions.
       Dim MidScale As Integer
       MidScale = ((2 ^ ADResolution) / 2) - 1
       ULStat = cbSetTrigger(BoardNum, DefaultTrig, MidScale, MidScale)
       ULStat = cbToEngUnits(BoardNum, CBRange, MidScale, EngUnits)
       lblResult.Caption = "Waiting for trigger on analog input above " _
           & Format(EngUnits, "0.00") & "V."
   End If
   
   ULStat = cbAPretrig(BoardNum, LowChan, HighChan, PretrigCount, _
   TotalCount, CBRate, CBRange, MemHandle, Options)
   If ULStat = BADBOARDTYPE Then
      lblResult.Caption = "Board " & Format(BoardNum, "0") & _
      " doesn't support the cbAPretrig function."
   ElseIf ULStat <> 0 Then
      lblResult.Caption = "Error " & Format(ULStat, "0") & " occurred."
   Else
      tmrCheckStatus.Enabled = True
      cmdTrigEnable.Enabled = False
   End If

End Sub

Private Sub tmrCheckStatus_Timer()

   Dim ULStat As Long
   Dim CurIndex As Long
   Dim CurCount As Long
   Dim DataElement As Long, TrigPoint As Long, SampleNum As Long
   Dim Status As Integer, i As Integer

   ' This timer will check the status of the background data collection
   
   ' Parameters:
   '   BoardNum&   :the number used by CB.CFG to describe this board
   '   Status&     :current status of the background data collection
   '   CurCount&   :current number of samples collected
   '   CurIndex&   :index to the data buffer pointing to the start of the
   '                most recently collected scan
   '   FunctionType:A/D operation (AIFUNCTIOM)

   ULStat = cbGetStatus(BoardNum, Status, CurCount, CurIndex, AIFUNCTION)
   If ULStat = TOOFEW Then
      lblResult.Caption = "Trigger occurred prematurely."
   ElseIf ULStat <> 0 Then
      Stop
   Else
      If CurCount > PretrigCount Then lblResult.Caption = ""
   End If

   lblShowCount.Caption = Format$(CurCount, "0")
   lblShowIndex.Caption = Format$(CurIndex, "0")
   
   ' Check if the background operation has finished. If it has, then
   ' transfer the data from the memory buffer set up by Windows to an
   ' array for use by Visual Basic
   ' The BACKGROUND operation must be explicitly stopped
    
   If Status = RUNNING Then
      lblShowStat.Caption = "Running"
   ElseIf Status = IDLE Then
      lblShowStat.Caption = "Idle"
      TrigPoint = PretrigCount - (NumPoints - CurCount) - 1
      'ULStat = cbGetStatus(BoardNum, Status, CurCount, CurIndex, AIFUNCTION)
      If ULStat = TOOFEW Then
          lblResult.Caption = "Premature trigger occurred at sample " _
          & Format(TrigPoint, "0") & "."
      Else
          If ULStat <> 0 Then
              lblResult.Caption = "Error " & Format(ULStat, "0") & " occurred."
              DoEvents
          End If
          If CurCount > PretrigCount Then lblResult.Caption = ""
      End If

      lblShowCount.Caption = Format$(CurCount, "0")
      lblShowIndex.Caption = Format$(CurIndex, "0")
     
      ULStat = cbStopBackground(BoardNum, AIFUNCTION)
      If ULStat <> 0 Then Stop

      tmrCheckStatus.Enabled = False
      cmdTrigEnable.Enabled = True

      ' Transfer the data from the memory buffer set up by Windows to an array for use by Visual Basic
      ULStat = cbWinBufToArray(MemHandle, ADData(0), FirstPoint, BufSize)
      If ULStat <> 0 Then Stop
            
      ' convert the data using cbAConvertPretrigData&()

      ' Parameters:
      '   PretrigCount&  :number of pre-trigger A/D samples collected
      '                   actual value is returned by cbAPretrig&()
      '   TotalCount&    :total number of A/D samples collected
      '                   actual value is returned by cbAPretrig&()
      '   ADData%        :the array containing the raw data values
      '   ChanTags%      :array that chan tags will be returned to

      ULStat = cbAConvertPretrigData(BoardNum, PretrigCount, TotalCount, ADData(0), ChanTags(0))
      If ULStat <> 0 Then Stop
      
      For i = 1 To 10
         DataElement = PretrigCount - (12 - i)
         lblPreTrig(i - 1).Caption = Format$(ADData%(DataElement), "0")
         SampleNum = TrigPoint - i
         lblPre(i - 1).Caption = ""
         If Not (SampleNum < 0) Then _
             lblPre(i - 1).Caption = "Sample " & Format(SampleNum, "0")
      Next i
      For i = 0 To 9
         DataElement = PretrigCount + i - 1
         lblPostTrig(i + 1).Caption = Format$(ADData(DataElement), "0")
         SampleNum = TrigPoint + i
         lblPost(i).Caption = "Sample " & Format(SampleNum, "0")
      Next i
   
   End If

End Sub

Private Sub cmdQuit_Click()
   
   ULStat = cbWinBufFree(MemHandle)      ' Free up memory for use by
   If ULStat <> 0 Then Stop                ' other programs
  
   ' The BACKGROUND operation must be explicitly stopped
  
   ULStat = cbStopBackground(BoardNum, AIFUNCTION)
   End

End Sub

