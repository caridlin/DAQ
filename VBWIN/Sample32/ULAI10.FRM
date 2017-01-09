VERSION 5.00
Begin VB.Form frmDataDisplay 
   Appearance      =   0  'Flat
   BackColor       =   &H80000005&
   Caption         =   "Universal Library Gain Queue"
   ClientHeight    =   6360
   ClientLeft      =   105
   ClientTop       =   1545
   ClientWidth     =   6765
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
   ScaleHeight     =   6360
   ScaleWidth      =   6765
   Begin VB.CheckBox chkChannels 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Random Channels"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   600
      TabIndex        =   54
      Top             =   5880
      Width           =   2295
   End
   Begin VB.CheckBox chkRanges 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Random Ranges"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   600
      TabIndex        =   53
      Top             =   5520
      Value           =   1  'Checked
      Width           =   1815
   End
   Begin VB.CommandButton cmdStopConvert 
      Appearance      =   0  'Flat
      Caption         =   "Quit"
      Height          =   375
      Left            =   5640
      TabIndex        =   13
      Top             =   5640
      Width           =   855
   End
   Begin VB.CommandButton cmdLoadQueue 
      Appearance      =   0  'Flat
      Caption         =   "Load Queue"
      Height          =   375
      Left            =   3960
      TabIndex        =   14
      Top             =   5640
      Width           =   1455
   End
   Begin VB.Timer tmrConvert 
      Enabled         =   0   'False
      Interval        =   1000
      Left            =   240
      Top             =   2880
   End
   Begin VB.CommandButton cmdUnloadQ 
      Appearance      =   0  'Flat
      Caption         =   "Unload Queue"
      Enabled         =   0   'False
      Height          =   375
      Left            =   3960
      TabIndex        =   47
      Top             =   5640
      Visible         =   0   'False
      Width           =   1455
   End
   Begin VB.Label lblInstruction 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Board 0 must have analog inputs that support paced acquisition."
      ForeColor       =   &H000000FF&
      Height          =   855
      Left            =   360
      TabIndex        =   55
      Top             =   600
      Width           =   6015
   End
   Begin VB.Label lblADData 
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
      Index           =   39
      Left            =   5520
      TabIndex        =   46
      Top             =   4800
      Width           =   975
   End
   Begin VB.Label lblADData 
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
      Index           =   38
      Left            =   4080
      TabIndex        =   45
      Top             =   4800
      Width           =   975
   End
   Begin VB.Label lblADData 
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
      Index           =   37
      Left            =   2640
      TabIndex        =   44
      Top             =   4800
      Width           =   975
   End
   Begin VB.Label lblADData 
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
      Index           =   36
      Left            =   1200
      TabIndex        =   43
      Top             =   4800
      Width           =   975
   End
   Begin VB.Label lblADData 
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
      Index           =   35
      Left            =   5520
      TabIndex        =   42
      Top             =   4560
      Width           =   975
   End
   Begin VB.Label lblADData 
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
      Index           =   34
      Left            =   4080
      TabIndex        =   41
      Top             =   4560
      Width           =   975
   End
   Begin VB.Label lblADData 
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
      Index           =   33
      Left            =   2640
      TabIndex        =   40
      Top             =   4560
      Width           =   975
   End
   Begin VB.Label lblADData 
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
      Index           =   32
      Left            =   1200
      TabIndex        =   39
      Top             =   4560
      Width           =   975
   End
   Begin VB.Label lblADData 
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
      Index           =   31
      Left            =   5520
      TabIndex        =   38
      Top             =   4320
      Width           =   975
   End
   Begin VB.Label lblADData 
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
      Index           =   30
      Left            =   4080
      TabIndex        =   37
      Top             =   4320
      Width           =   975
   End
   Begin VB.Label lblADData 
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
      Index           =   29
      Left            =   2640
      TabIndex        =   36
      Top             =   4320
      Width           =   975
   End
   Begin VB.Label lblADData 
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
      Index           =   28
      Left            =   1200
      TabIndex        =   35
      Top             =   4320
      Width           =   975
   End
   Begin VB.Label lblADData 
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
      Index           =   27
      Left            =   5520
      TabIndex        =   34
      Top             =   4080
      Width           =   975
   End
   Begin VB.Label lblADData 
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
      Index           =   26
      Left            =   4080
      TabIndex        =   33
      Top             =   4080
      Width           =   975
   End
   Begin VB.Label lblADData 
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
      Index           =   25
      Left            =   2640
      TabIndex        =   32
      Top             =   4080
      Width           =   975
   End
   Begin VB.Label lblADData 
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
      Index           =   24
      Left            =   1200
      TabIndex        =   31
      Top             =   4080
      Width           =   975
   End
   Begin VB.Label lblADData 
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
      Index           =   23
      Left            =   5520
      TabIndex        =   30
      Top             =   3840
      Width           =   975
   End
   Begin VB.Label lblADData 
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
      Index           =   22
      Left            =   4080
      TabIndex        =   29
      Top             =   3840
      Width           =   975
   End
   Begin VB.Label lblADData 
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
      Index           =   21
      Left            =   2640
      TabIndex        =   28
      Top             =   3840
      Width           =   975
   End
   Begin VB.Label lblADData 
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
      Index           =   20
      Left            =   1200
      TabIndex        =   27
      Top             =   3840
      Width           =   975
   End
   Begin VB.Label lblADData 
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
      Index           =   11
      Left            =   5520
      TabIndex        =   18
      Top             =   3600
      Width           =   975
   End
   Begin VB.Label lblADData 
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
      Index           =   10
      Left            =   4080
      TabIndex        =   17
      Top             =   3600
      Width           =   975
   End
   Begin VB.Label lblADData 
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
      Index           =   9
      Left            =   2640
      TabIndex        =   16
      Top             =   3600
      Width           =   975
   End
   Begin VB.Label lblADData 
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
      Index           =   8
      Left            =   1200
      TabIndex        =   15
      Top             =   3600
      Width           =   975
   End
   Begin VB.Label lblADData 
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
      Index           =   19
      Left            =   5520
      TabIndex        =   26
      Top             =   3360
      Width           =   975
   End
   Begin VB.Label lblADData 
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
      Index           =   18
      Left            =   4080
      TabIndex        =   25
      Top             =   3360
      Width           =   975
   End
   Begin VB.Label lblADData 
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
      Index           =   17
      Left            =   2640
      TabIndex        =   24
      Top             =   3360
      Width           =   975
   End
   Begin VB.Label lblADData 
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
      Index           =   16
      Left            =   1200
      TabIndex        =   23
      Top             =   3360
      Width           =   975
   End
   Begin VB.Label lblADData 
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
      Index           =   15
      Left            =   5520
      TabIndex        =   22
      Top             =   3120
      Width           =   975
   End
   Begin VB.Label lblADData 
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
      Index           =   14
      Left            =   4080
      TabIndex        =   21
      Top             =   3120
      Width           =   975
   End
   Begin VB.Label lblADData 
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
      Index           =   13
      Left            =   2640
      TabIndex        =   20
      Top             =   3120
      Width           =   975
   End
   Begin VB.Label lblADData 
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
      Index           =   12
      Left            =   1200
      TabIndex        =   19
      Top             =   3120
      Width           =   975
   End
   Begin VB.Label lblADData 
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
      Index           =   7
      Left            =   5520
      TabIndex        =   12
      Top             =   2880
      Width           =   975
   End
   Begin VB.Label lblADData 
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
      Index           =   6
      Left            =   4080
      TabIndex        =   11
      Top             =   2880
      Width           =   975
   End
   Begin VB.Label lblADData 
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
      Index           =   5
      Left            =   2640
      TabIndex        =   10
      Top             =   2880
      Width           =   975
   End
   Begin VB.Label lblADData 
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
      Index           =   4
      Left            =   1200
      TabIndex        =   9
      Top             =   2880
      Width           =   975
   End
   Begin VB.Label lblADData 
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
      Left            =   5520
      TabIndex        =   8
      Top             =   2640
      Width           =   975
   End
   Begin VB.Label lblADData 
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
      Left            =   4080
      TabIndex        =   7
      Top             =   2640
      Width           =   975
   End
   Begin VB.Label lblADData 
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
      Left            =   2640
      TabIndex        =   6
      Top             =   2640
      Width           =   975
   End
   Begin VB.Label lblADData 
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
      Left            =   1200
      TabIndex        =   5
      Top             =   2640
      Width           =   975
   End
   Begin VB.Label lblShowRange 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   -1  'True
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   3
      Left            =   5400
      TabIndex        =   52
      Top             =   2160
      Width           =   1305
   End
   Begin VB.Label lblShowRange 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   -1  'True
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   2
      Left            =   3960
      TabIndex        =   51
      Top             =   2160
      Width           =   1305
   End
   Begin VB.Label lblShowRange 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   -1  'True
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   1
      Left            =   2520
      TabIndex        =   50
      Top             =   2160
      Width           =   1305
   End
   Begin VB.Label lblShowRange 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   -1  'True
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H00FF0000&
      Height          =   255
      Index           =   0
      Left            =   1080
      TabIndex        =   49
      Top             =   2160
      Width           =   1305
   End
   Begin VB.Label lblRange 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Range:"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   120
      TabIndex        =   48
      Top             =   2160
      Width           =   735
   End
   Begin VB.Label lblChan 
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
      Index           =   3
      Left            =   5520
      TabIndex        =   4
      Top             =   1800
      Width           =   975
   End
   Begin VB.Label lblChan 
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
      Index           =   2
      Left            =   4080
      TabIndex        =   3
      Top             =   1800
      Width           =   975
   End
   Begin VB.Label lblChan 
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
      Index           =   1
      Left            =   2640
      TabIndex        =   2
      Top             =   1800
      Width           =   975
   End
   Begin VB.Label lblChan 
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
      Index           =   0
      Left            =   1200
      TabIndex        =   1
      Top             =   1800
      Width           =   975
   End
   Begin VB.Label lblDemoFunction 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Demonstration of cbALoadQueue()"
      ForeColor       =   &H80000008&
      Height          =   255
      Left            =   120
      TabIndex        =   0
      Top             =   120
      Width           =   6585
   End
End
Attribute VB_Name = "frmDataDisplay"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
'ULAI10.VBP================================================================

' File:                         ULAI10.VBP

' Library Call Demonstrated:    cbALoadQueue()
'
' Purpose:                      Loads an A/D board's channel/gain queue.
'
' Demonstration:                Prepares a channel/gain queue and loads it
'                               to the board. An analog input function
'                               is then called to show how the queue
'                               values work.
'
' Other Library Calls:          cbErrHandling()
'
' Special Requirements:         Board 0 must have an A/D converter and
'                               channel gain queue hardware.

'==========================================================================
Option Explicit

Const BoardNum As Long = 0          ' Board number
Dim CBRange As Long
Dim ADResolution As Long, NumAIChans As Long
Dim HighChan As Long, LowChan As Long, MaxChan As Long
Dim DefaultRange As String

Const NumPoints As Long = 120           ' Number of data points to collect
Const NumElements As Long = 4

Dim ADData() As Integer       ' dimension an array to hold the input values
Dim ADData32() As Long        ' dimension an array to hold the high resolution input values
Dim MemHandle As Long         ' define a variable to contain the handle for
                              ' memory allocated by Windows through cbWinBufAlloc()
                              
Dim ChanArray() As Integer    ' array to hold channel queue information
Dim GainArray() As Integer    ' array to hold gain queue information
Dim ULStat As Long

Private Sub Form_Load()
    
   Dim DefaultTrig As Long
   Dim ReportError As Long, HandleError As Long
   Dim i As Integer
    
   ' declare revision level of Universal Library

   ULStat = cbDeclareRevision(CURRENTREVNUM)
   
   ' Initiate error handling
   '  activating error handling will trap errors like
   '  bad channel numbers and non-configured conditions.
   '  Parameters:
   '    PRINTALL    :all warnings and errors encountered will be printed
   '    DONTSTOP    :if an error is encountered, the program will not stop,
   '                 errors must be handled locally
     
   ReportError = DONTPRINT
   HandleError = DONTSTOP
   ULStat = cbErrHandling(ReportError, HandleError)
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
      lblInstruction.Caption = "Board " & Format(BoardNum, "0") & _
      " does not have analog input channels."
      Me.cmdLoadQueue.Enabled = False
   Else
      ' Check the resolution of the A/D data and allocate memory accordingly
      If ADResolution > 16 Then
         ' set aside memory to hold high resolution data
         ReDim ADData32(NumPoints)
         MemHandle = cbWinBufAlloc32(NumPoints)
      Else
         ' set aside memory to hold data
         ReDim ADData(NumPoints)
         MemHandle = cbWinBufAlloc(NumPoints)
      End If
      If MemHandle = 0 Then Stop
      MaxChan = LowChan + NumAIChans - 1      'allow use of any channel for queue
      If (NumAIChans > 4) Then NumAIChans = 4 'limit to 4 channels for display
      ReDim ChanArray(NumElements - 1)
      ReDim GainArray(NumElements - 1)
      lblInstruction.Caption = "Board " & Format(BoardNum, "0") & _
          " collecting analog data on up to " & Format(NumAIChans, "0") & _
          " channels between channel 0 and channel " & Format(MaxChan, "0") & _
          " using AInScan and ALoadQueue."
      Me.tmrConvert.Enabled = True
      DefaultRange = GetRangeString(CBRange)
   End If
   For i = 0 To 3
       lblShowRange(i).Caption = DefaultRange$
   Next i

End Sub

Private Sub cmdLoadQueue_Click()

   Dim ULStat As Long
   Dim ValidRanges() As Long
   Dim NumRanges As Integer
   Dim x As Double
   Dim i As Integer

   'Get a list of valid ranges from the AnalogIO module
   ValidRanges = GetRangeList()
   NumRanges = UBound(ValidRanges)

   cmdLoadQueue.Enabled = 0
   cmdLoadQueue.Visible = 0
   cmdUnloadQ.Enabled = 1
   cmdUnloadQ.Visible = 1

   ' Set up the channel/gain queue for 4 channels - each
   ' channel set to random valid A/D ranges.
   ' Note: Some devices have limitations on the queue,
   ' such as not mixing Bipolar/Unipolar ranges or allowing
   ' only unique contiguous channels - see hardware manual

   For i = 0 To NumElements - 1
       If (chkRanges.Value = 1) Then
           x = Rnd(Timer)
           GainArray(i) = ValidRanges(x * NumRanges)
       Else
           GainArray(i) = CBRange
       End If
       If (chkChannels.Value = 1) Then
           x = Rnd(Timer)
           ChanArray(i) = x * MaxChan
       Else
           ChanArray(i) = i
       End If
   Next

   ' Load the channel/gain values into the queue
   '  Parameters:
   '    BoardNum    :the number used by CB.CFG to describe this board
   '    ChanArray[] :array of channel values
   '    GainArray[] :array of gain values
   '    NumElements :the number of elements in the arrays (0=disable queue)
        
   ULStat = cbALoadQueue(BoardNum, ChanArray(0), GainArray(0), NumElements)
   If ULStat = BADADCHAN Then
      lblInstruction.Caption = "Board " & Format(BoardNum, "0") & _
          " doesn't support random channels. Queue was not changed."
   ElseIf ULStat <> 0 Then
      tmrConvert.Enabled = False
      Stop
   Else
      lblInstruction.Caption = "Queue loaded on board " & _
          Format(BoardNum, "0")
      For i = 0 To NumElements - 1
         lblShowRange(i).Caption = GetRangeString(GainArray(i))
         lblChan(i).Caption = "Channel " & Format(ChanArray(i), "0")
      Next
   End If

End Sub

Private Sub tmrConvert_Timer()

   Dim ChannelNum As Long
   Dim SampleNum As Long
   Dim i As Long
   Dim FirstPoint As Long
   Dim ULStat As Long
   Dim Rate As Long
   Dim Options As Long
   Dim Count As Long
   Dim LastChan As Long
   Dim FirstChan As Long
   Dim CBCount As Long, CBRate As Long
   
   ' Call an analog input function to show how the gain queue values
   ' supercede those passed to the function.
   
   ' Collect the values with cbAInScan()
   '  Parameters:
   '    BoardNum    :the number used by CB.CFG to describe this board
   '    LowChan     :the first channel of the scan
   '    HighChan    :the last channel of the scan
   '    CBCount     :the total number of A/D samples to collect
   '    CBRate      :sample rate in samples per second
   '    CBRange         :the gain for the board
   '    ADData[]    :the array for the collected data values
   '    Options      :data collection options

   FirstChan = 0           ' This is ignored when queue is enabled
   LastChan = 3            ' This is ignored when queue is enabled
   CBCount = NumPoints     ' Number of data points to collect
   Options = CONVERTDATA   ' Return data as 12-bit values

   ' per channel sampling rate ((samples per second) per channel)
   CBRate = 1000 / ((LastChan - FirstChan) + 1)

   ULStat = cbAInScan(BoardNum, FirstChan, LastChan, _
      CBCount, CBRate, CBRange, MemHandle, Options)
   If ULStat <> 0 Then Stop

   ' Transfer the data from the memory buffer set up by Windows to an array for use by Visual Basic
   i = 0
   If ADResolution > 16 Then
      ULStat = cbWinBufToArray32(MemHandle, ADData32(0), FirstPoint, CBCount)
   Else
      ULStat = cbWinBufToArray(MemHandle, ADData(0), FirstPoint, CBCount)
   End If
   If ULStat <> 0 Then Stop
        
   For SampleNum = 0 To 9
       For ChannelNum = 0 To NumAIChans - 1
          If ADResolution > 16 Then
             lblADData(i).Caption = Format$(ADData32(i), "0")
          Else
             lblADData(i).Caption = Format$(ADData(i), "0")
          End If
          i = i + 1
       Next ChannelNum
   Next SampleNum

End Sub

Private Sub cmdUnloadQ_Click()

   Dim i As Integer, NoChans As Integer
   
   cmdUnloadQ.Enabled = 0
   cmdUnloadQ.Visible = 0
   cmdLoadQueue.Enabled = 1
   cmdLoadQueue.Visible = 1
   
   For i = 0 To 3
      lblShowRange(i).Caption = GetRangeString(CBRange)
      lblChan(i).Caption = "Channel " & Format(i, "0")
   Next i

   NoChans = 0      ' set to zero to disable queue
        
   ULStat = cbALoadQueue(BoardNum, ChanArray(0), GainArray(0), NoChans)
   If ULStat <> NOERRORS Then
      lblInstruction.Caption = "Board " & Format(BoardNum, "0") & _
          " error unloading queue: "
      Stop
   Else
      lblInstruction.Caption = "Board " & Format(BoardNum, "0") & _
          " scanning contiguous channels with with Range set to " & _
          DefaultRange & "."
   End If

End Sub

Private Sub cmdStopConvert_Click()
   
   ULStat = cbWinBufFree(MemHandle)      ' Free up memory for use by
   If ULStat <> 0 Then Stop                ' other programs
   End
   
End Sub


