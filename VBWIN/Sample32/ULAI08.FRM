VERSION 5.00
Begin VB.Form frmPreTrig 
   Appearance      =   0  'Flat
   BackColor       =   &H80000005&
   Caption         =   "Universal Library Analog Input Scan"
   ClientHeight    =   6075
   ClientLeft      =   105
   ClientTop       =   1545
   ClientWidth     =   6165
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
   ScaleHeight     =   6075
   ScaleWidth      =   6165
   Begin VB.CommandButton cmdQuit 
      Appearance      =   0  'Flat
      Caption         =   "Quit"
      Height          =   390
      Left            =   5280
      TabIndex        =   17
      Top             =   5520
      Width           =   780
   End
   Begin VB.CommandButton cmdTrigEnable 
      Appearance      =   0  'Flat
      Caption         =   "Start Pre/Post Trigger background operation"
      Height          =   495
      Left            =   600
      TabIndex        =   18
      Top             =   1800
      Width           =   4935
   End
   Begin VB.Label lblResult 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H00C00000&
      Height          =   615
      Left            =   240
      TabIndex        =   46
      Top             =   5160
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
      Left            =   840
      TabIndex        =   45
      Top             =   480
      Width           =   4455
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
      Left            =   4515
      TabIndex        =   42
      Top             =   4485
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
      Left            =   3210
      TabIndex        =   40
      Top             =   4485
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
      Left            =   1800
      TabIndex        =   22
      Top             =   4485
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
      Left            =   360
      TabIndex        =   20
      Top             =   4485
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
      Left            =   4515
      TabIndex        =   41
      Top             =   4290
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
      Left            =   3210
      TabIndex        =   39
      Top             =   4290
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
      Left            =   1800
      TabIndex        =   21
      Top             =   4290
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
      Left            =   390
      TabIndex        =   19
      Top             =   4290
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
      Left            =   4515
      TabIndex        =   38
      Top             =   4110
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
      Left            =   3240
      TabIndex        =   37
      Top             =   4110
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
      Left            =   1800
      TabIndex        =   16
      Top             =   4110
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
      Left            =   390
      TabIndex        =   8
      Top             =   4110
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
      Left            =   4515
      TabIndex        =   34
      Top             =   3915
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
      Left            =   3210
      TabIndex        =   33
      Top             =   3915
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
      Left            =   1800
      TabIndex        =   15
      Top             =   3915
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
      Left            =   390
      TabIndex        =   7
      Top             =   3915
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
      Left            =   4515
      TabIndex        =   30
      Top             =   3720
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
      Left            =   3210
      TabIndex        =   29
      Top             =   3720
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
      Left            =   1800
      TabIndex        =   14
      Top             =   3720
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
      Left            =   390
      TabIndex        =   6
      Top             =   3720
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
      Left            =   4515
      TabIndex        =   26
      Top             =   3525
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
      Left            =   3210
      TabIndex        =   25
      Top             =   3525
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
      Left            =   1800
      TabIndex        =   13
      Top             =   3525
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
      Left            =   390
      TabIndex        =   5
      Top             =   3525
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
      Left            =   4515
      TabIndex        =   36
      Top             =   3330
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
      Left            =   3210
      TabIndex        =   35
      Top             =   3330
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
      Left            =   1800
      TabIndex        =   12
      Top             =   3330
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
      Left            =   390
      TabIndex        =   4
      Top             =   3330
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
      Left            =   4515
      TabIndex        =   32
      Top             =   3150
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
      Left            =   3210
      TabIndex        =   31
      Top             =   3150
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
      Left            =   1800
      TabIndex        =   11
      Top             =   3150
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
      Left            =   390
      TabIndex        =   3
      Top             =   3150
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
      Left            =   4515
      TabIndex        =   28
      Top             =   2955
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
      Left            =   3210
      TabIndex        =   27
      Top             =   2955
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
      Left            =   1800
      TabIndex        =   10
      Top             =   2955
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
      Left            =   390
      TabIndex        =   2
      Top             =   2955
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
      Left            =   4515
      TabIndex        =   24
      Top             =   2760
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
      Left            =   3210
      TabIndex        =   23
      Top             =   2760
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
      Left            =   1800
      TabIndex        =   9
      Top             =   2760
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
      Left            =   390
      TabIndex        =   1
      Top             =   2760
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
      Left            =   3120
      TabIndex        =   44
      Top             =   2475
      Width           =   2460
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
      Left            =   315
      TabIndex        =   43
      Top             =   2475
      Width           =   2415
   End
   Begin VB.Label lblDemoFunction 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Demonstration of cbAPreTrig()"
      ForeColor       =   &H80000008&
      Height          =   330
      Left            =   240
      TabIndex        =   0
      Top             =   120
      Width           =   5655
   End
End
Attribute VB_Name = "frmPreTrig"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
'ULAI08.VBP================================================================

' File:                         ULAI08.VBP

' Library Call Demonstrated:    cbAPretrig()

' Purpose:                      Waits for a trigger, then returns a specified
'                               number of analog samples before and after
'                               the trigger.

' Demonstration:                Displays the analog input on one channel and
'                               waits for the trigger.

' Other Library Calls:          cbErrHandling()

' Special Requirements:         Board 0 must support cbAPretrig.
'                               Analog signal on an input channel.

' (c) Copyright 2005-2011, Measurement Computing Corp.
' All rights reserved.
'==========================================================================
Option Explicit

Const BoardNum As Long = 0             ' Board number
Dim CBRange As Long
Dim ADResolution As Long, NumAIChans As Long
Dim HighChan As Long, LowChan As Long, MaxChan As Long
Dim DefaultTrig As Long

Const NumPoints As Long = 4096         ' Number of data points to collect
Const FirstPoint As Long = 0           ' set first element in buffer to transfer to array
Const PretrigCount As Long = 10        ' number of data points before trigger to store
Const TotalCount As Long = NumPoints   ' total number of data points to collect
Const BufSize As Long = 4608           ' set buffer size large enough to hold all data

Dim MemHandle As Long             ' define a variable to contain the handle for
                                  ' memory allocated by Windows through cbWinBufAlloc()
Dim ADData() As Integer           ' dimension an array to hold the input values
                                  ' size must be TotalCount + 512 minimum
Dim ULStat As Long

Private Sub Form_Load()

   Dim ULStat As Long
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
   ChannelType = PRETRIGIN
   NumAIChans = FindAnalogChansOfType(BoardNum, ChannelType, _
      ADResolution, CBRange, LowChan, DefaultTrig)
   
   If (NumAIChans = 0) Then
       lblInstruction.Caption = "Board " & Format(BoardNum, "0") & _
       " does not have analog input channels that support pretrigger."
       cmdTrigEnable.Enabled = False
   Else
       ' set aside memory to hold 16-bit data and channel tags
       ReDim ADData(NumPoints)
       MemHandle = cbWinBufAlloc(BufSize)
       If MemHandle = 0 Then Stop
       If (NumAIChans > 8) Then NumAIChans = 8 'limit to 8 for display
       MaxChan = LowChan + NumAIChans - 1
       lblInstruction.Caption = "Board " & Format(BoardNum, "0") & _
           " collecting analog data on channel 0 using APretrig in " & _
           "foreground mode with Range set to " & GetRangeString(CBRange) & "."
   End If

End Sub

Private Sub cmdTrigEnable_Click()

   Dim ULStat As Long
   Dim Options As Long
   Dim Rate As Long, CBRate As Long
   Dim HighChan As Long, i As Integer
   Dim DataElement As Long, TrigPoint As Long, SampleNum As Long
   Dim TotalCount As Long   ' total number of data points to collect
   Dim PretrigCount As Long ' number of data points before trigger to store
   Dim EngUnits As Single
   Dim DataAvailable As Boolean
   
   lblResult.Caption = _
      "Waiting for trigger on trigger input " & _
      "and acquiring data (Ctl-Break to abort)."
   MousePointer = vbHourglass
   DoEvents
   DataAvailable = False

   ' Monitor a range of channels for a trigger then collect the values
   ' with cbAPretrig()
   ' Parameters:
   '   BoardNum      :the number used by CB.CFG to describe this board
   '   LowChan       :first A/D channel of the scan
   '   HighChan      :last A/D channel of the scan
   '   PretrigCount  :number of pre-trigger A/D samples to collect
   '   TotalCount    :total number of A/D samples to collect
   '   CBRate        :sample rate in samples per second
   '   CBRange           :the gain for the board
   '   ADData        :the array for the collected data values
   '   Options        :data collection options

   HighChan = LowChan
   CBRate = 1000                     ' sampling rate (samples per second)
   TotalCount = NumPoints
   PretrigCount = 1000
   Options = CONVERTDATA             ' return data aligned around the trigger point

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
   
   MousePointer = MousePointerConstants.vbDefault
   TrigPoint = PretrigCount - 1
   If ULStat = BADBOARDTYPE Then
      lblResult.Caption = "Board " & Format(BoardNum, "0") & _
      " doesn't support the cbAPretrig function."
   ElseIf ULStat = TOOFEW Then
       lblResult.Caption = "Premature trigger occurred at sample " _
       & Format(TrigPoint, "0") & "."
       DataAvailable = True
   ElseIf ULStat <> 0 Then
       lblResult.Caption = "Error " & Format(ULStat, "0") & " occurred."
       DoEvents
       Stop
   Else
       lblResult.Caption = ""
       DataAvailable = True
   End If

   ' Transfer the data from the memory buffer set up by Windows
   ' to an array for use by this program

   If DataAvailable Then
      ULStat = cbWinBufToArray(MemHandle, ADData(0), FirstPoint, NumPoints)
      If ULStat <> 0 Then Stop
   
      For i = 1 To 10
         DataElement = PretrigCount - (12 - i)
         If Not (DataElement < 0) Then _
            lblPreTrig(i - 1).Caption = Format$(ADData(DataElement), "0")
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
   If ULStat <> 0 Then End                 ' other programs
   End

End Sub


