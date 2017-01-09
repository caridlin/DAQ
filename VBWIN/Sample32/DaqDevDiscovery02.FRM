VERSION 5.00
Begin VB.Form frmDevDiscovery 
   Appearance      =   0  'Flat
   BackColor       =   &H80000005&
   Caption         =   "Universal Library Analog Input"
   ClientHeight    =   6495
   ClientLeft      =   2730
   ClientTop       =   1500
   ClientWidth     =   4905
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
   ScaleHeight     =   6495
   ScaleWidth      =   4905
   Begin VB.TextBox txtPort 
      Height          =   285
      Left            =   960
      TabIndex        =   11
      Text            =   "54211"
      Top             =   1800
      Width           =   1335
   End
   Begin VB.TextBox txtHost 
      ForeColor       =   &H00808080&
      Height          =   285
      Left            =   960
      TabIndex        =   10
      Text            =   "<Host name or IP address>"
      Top             =   1200
      Width           =   3495
   End
   Begin VB.Frame Frame1 
      BackColor       =   &H80000005&
      Caption         =   "Discovered Device"
      Height          =   2175
      Left            =   240
      TabIndex        =   4
      Top             =   3720
      Width           =   4455
      Begin VB.CommandButton cmdFlashLED 
         Caption         =   "Flash LED"
         Enabled         =   0   'False
         Height          =   375
         Left            =   1440
         TabIndex        =   7
         Top             =   1560
         Width           =   1455
      End
      Begin VB.Label lblDevName 
         BackColor       =   &H80000005&
         Caption         =   "Name"
         ForeColor       =   &H00008000&
         Height          =   255
         Left            =   1800
         TabIndex        =   13
         Top             =   480
         Width           =   1575
      End
      Begin VB.Label Label2 
         Alignment       =   1  'Right Justify
         BackColor       =   &H80000005&
         Caption         =   "Device Name:"
         Height          =   255
         Left            =   240
         TabIndex        =   12
         Top             =   480
         Width           =   1455
      End
      Begin VB.Label lblDevID 
         BackColor       =   &H80000005&
         Caption         =   "ID"
         ForeColor       =   &H00008000&
         Height          =   255
         Left            =   1800
         TabIndex        =   6
         Top             =   1080
         Width           =   2295
      End
      Begin VB.Label Label1 
         BackColor       =   &H80000005&
         Caption         =   "Device Identifier:"
         Height          =   255
         Left            =   240
         TabIndex        =   5
         Top             =   1080
         Width           =   1575
      End
   End
   Begin VB.CommandButton cmdDiscover 
      Appearance      =   0  'Flat
      Caption         =   "Discover DAQ device"
      Default         =   -1  'True
      Height          =   390
      Left            =   1200
      TabIndex        =   2
      Top             =   2520
      Width           =   2700
   End
   Begin VB.CommandButton cmdQuit 
      Appearance      =   0  'Flat
      Caption         =   "Quit"
      Height          =   390
      Left            =   3840
      TabIndex        =   3
      Top             =   6000
      Width           =   780
   End
   Begin VB.Label lblPort 
      Alignment       =   1  'Right Justify
      BackColor       =   &H80000005&
      Caption         =   "Port:"
      Height          =   255
      Left            =   360
      TabIndex        =   9
      Top             =   1800
      Width           =   495
   End
   Begin VB.Label lblHost 
      Alignment       =   1  'Right Justify
      BackColor       =   &H80000005&
      Caption         =   "Host:"
      Height          =   375
      Left            =   360
      TabIndex        =   8
      Top             =   1200
      Width           =   495
   End
   Begin VB.Label lblStatus 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Status: "
      ForeColor       =   &H00FF0000&
      Height          =   255
      Left            =   240
      TabIndex        =   0
      Top             =   3240
      Width           =   4455
   End
   Begin VB.Label lblDemoFunction 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Demonstration of DaqDeviceManager.GetNetDeviceDescriptor() and DaqDeviceManager.CreateDaqDevice() "
      ForeColor       =   &H80000008&
      Height          =   735
      Left            =   120
      TabIndex        =   1
      Top             =   120
      Width           =   4650
   End
End
Attribute VB_Name = "frmDevDiscovery"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
'DaqDevDiscovery02.VBP================================================================

' File:                         DaqDevDiscovery02.VBP

' Library Call Demonstrated:    cbGetNetDeviceDescriptor()
'                               cbCreateDaqDevice()
'                               cbReleaseDaqDevice()

' Purpose:                      Discovers an ethernet DAQ device and assigns
'                               board number to the detected device
                              
' Demonstration:                Displays the detected DAQ device
'                               and flashes the LED of the device

' Other Library Calls:          cbReleaseDaqDevice()
'                               cbIgnoreInstaCal()
'                               cbErrHandling()
'


'==========================================================================
Option Explicit

Dim ULStat As Long
Const Default_txt As String = "<Host name or IP address>"
Dim BoardNum As Long

Private Sub Form_Load()
   
   Dim LowChan As Long
   Dim ChannelType As Long
   Dim DefaultTrig As Long
   Dim ULStat As Long
   Dim ReportError As Long, HandleError As Long

   ' declare revision level of Universal Library

   ULStat = cbDeclareRevision(CURRENTREVNUM)

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

   ' If cbErrHandling is set for STOPALL or STOPFATAL during the program
   ' design stage, Visual Basic will be unloaded when an error is encountered.
   ' We suggest trapping errors locally until the program is ready for compiling
   ' to avoid losing unsaved data during program design.  This can be done by
   ' setting cbErrHandling options as above and checking the value of ULStat
   ' after a call to the library. If it is not equal to 0, an error has occurred.
   
   ' Ignore InstaCal device discovery
   ULStat = cbIgnoreInstaCal()
   
   lblDevName.Caption = ""
    lblDevID.Caption = ""

End Sub

Private Sub cmdDiscover_Click()

    Dim DeviceName As String
    Dim DeviceDescriptor(1) As DaqDeviceDescriptor
    Dim Host As String
    Dim PortNum As Long
    Dim Timeout As Integer

    
    cmdFlashLED.Enabled = False
    lblDevName.Caption = ""
    lblDevID.Caption = ""
    
    If BoardNum <> -1 Then
        cbReleaseDaqDevice (BoardNum)
    End If
    
    BoardNum = -1
    Host = txtHost.Text
    Timeout = 5000

    
   If IsNumeric(txtPort.Text) Then

        Screen.MousePointer = vbHourglass
        
        PortNum = CLng(txtPort.Text)
        
        ' Discover an Ethernet DAQ device with cbGetNetDeviceDescriptor()
        '   Parameters:
        '       Host                : Host name or IP address of DAQ device
        '       Port:                                         Port Number
        '       DeviceDescriptor    : Descriptor of the dicovered device
        '       Timeout             : Timeout
        
        ULStat = cbGetNetDeviceDescriptor(Host, PortNum, DeviceDescriptor(0), Timeout)
        
        If ULStat = 0 Then
            lblStatus.Caption = "DAQ Device Discovered"
            lblDevName.Caption = StrConv(DeviceDescriptor(0).ProductName, vbUnicode)
            lblDevID.Caption = StrConv(DeviceDescriptor(0).UniqueID, vbUnicode)
            BoardNum = 0
                    
            '    Create a new MccBoard object for Board and assign a board number
            '    to the specified DAQ device with CreateDaqDevice()
            
            '    Parameters:
            '        BoardNum           : board number to be assigned to the specified DAQ device
            '        DeviceDescriptor   : device descriptor of the DAQ device
            
            ULStat = cbCreateDaqDevice(BoardNum, DeviceDescriptor(0))
                    
            If ULStat = 0 Then
                cmdFlashLED.Enabled = True
            Else
                lblStatus.Caption = "cbCreateDaqDevice failed"
                BoardNum = -1
            End If
                
        Else
            lblStatus.Caption = "Unable to discover the specified DAQ device"
        End If
    
    Else
        lblStatus.Caption = "Invalid port number"
    End If


    Screen.MousePointer = vbDefault
   
End Sub


Private Sub cmdFlashLED_Click()

    If BoardNum <> -1 Then
    
        ' Flash the LED of the specified DAQ device with cbFlashLED()
        
        '   Parameters:
        '      BoardNum            : board number assigned to the DAQ device
        
        
        ULStat = cbFlashLED(BoardNum)
    End If
    
End Sub

Private Sub cmdQuit_Click()
    End
End Sub
Private Sub txtHost_GotFocus()
    If txtHost.Text = Default_txt Then
        txtHost.Text = ""
        txtHost.ForeColor = &H0&
    End If
End Sub

Private Sub txtHost_LostFocus()
    If txtHost.Text = "" Then
        txtHost.ForeColor = &H808080
        txtHost.Text = Default_txt
    End If
End Sub
