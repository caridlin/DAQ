VERSION 5.00
Begin VB.Form frmDevDiscovery 
   Appearance      =   0  'Flat
   BackColor       =   &H80000005&
   Caption         =   "Universal Library Analog Input"
   ClientHeight    =   5160
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
   ScaleHeight     =   5160
   ScaleWidth      =   4905
   Begin VB.Frame Frame1 
      BackColor       =   &H80000005&
      Caption         =   "Discovered Devices"
      Height          =   2175
      Left            =   240
      TabIndex        =   4
      Top             =   2280
      Width           =   4455
      Begin VB.CommandButton cmdFlashLED 
         Caption         =   "Flash LED"
         Enabled         =   0   'False
         Height          =   375
         Left            =   1440
         TabIndex        =   8
         Top             =   1560
         Width           =   1455
      End
      Begin VB.ComboBox cmbBoxDiscoveredDevs 
         Enabled         =   0   'False
         Height          =   315
         Left            =   240
         Style           =   2  'Dropdown List
         TabIndex        =   5
         Top             =   480
         Width           =   3975
      End
      Begin VB.Label lblDevID 
         BackColor       =   &H80000005&
         Caption         =   "ID"
         ForeColor       =   &H00008000&
         Height          =   255
         Left            =   1800
         TabIndex        =   7
         Top             =   1080
         Width           =   2295
      End
      Begin VB.Label Label1 
         BackColor       =   &H80000005&
         Caption         =   "Device Identifier:"
         Height          =   255
         Left            =   240
         TabIndex        =   6
         Top             =   1080
         Width           =   1575
      End
   End
   Begin VB.CommandButton cmdDiscover 
      Appearance      =   0  'Flat
      Caption         =   "Discover DAQ devices"
      Default         =   -1  'True
      Height          =   390
      Left            =   1200
      TabIndex        =   2
      Top             =   1080
      Width           =   2700
   End
   Begin VB.CommandButton cmdQuit 
      Appearance      =   0  'Flat
      Caption         =   "Quit"
      Height          =   390
      Left            =   3840
      TabIndex        =   3
      Top             =   4560
      Width           =   780
   End
   Begin VB.Label lblStatus 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Status: "
      ForeColor       =   &H00FF0000&
      Height          =   255
      Left            =   240
      TabIndex        =   0
      Top             =   1800
      Width           =   4455
   End
   Begin VB.Label lblDemoFunction 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Demonstration of DaqDeviceManager.GetDaqDeviceInventory() and DaqDeviceManager.CreateDaqDevice() "
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
'DaqDevDiscovery01.VBP================================================================

' File:                         DaqDevDiscovery01.VBP

' Library Call Demonstrated:    cbGetDaqDeviceInventory()
'                               cbCreateDaqDevice()
'                               cbReleaseDaqDevice()

' Purpose:                      Discovers DAQ devices and assigns
'                               board number to the detected devices
                              
' Demonstration:                Displays the detected DAQ devices
'                               and flashes the LED of the selected device

' Other Library Calls:          cbIgnoreInstaCal()
'


'==========================================================================
Option Explicit

Dim ULStat As Long
Dim Inventory(100) As DaqDeviceDescriptor

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

End Sub

Private Sub cmdDiscover_Click()
    Dim NumberOfDevices As Long
    Dim numDevDiscovered As Integer
    Dim BoardNum As Long
    Dim DeviceName As String
    
    NumberOfDevices = UBound(Inventory)
    
    Call ReleaseDaqDevices
    
    cmbBoxDiscoveredDevs.Clear
    
    lblDevID.Caption = ""

    Screen.MousePointer = vbHourglass

    ' Discover DAQ devices with cbGetDaqDeviceInventory()
    ' Parameters:
    '    InterfaceType   :interface type of DAQ devices to be discovered
    '    inventory()     :array for the discovered DAQ devices
    '    numberOfDevices :number of DAQ devices discovered

    ULStat = cbGetDaqDeviceInventory(ANY_IFC, Inventory(0), NumberOfDevices)

    numDevDiscovered = NumberOfDevices

    lblStatus.Caption = numDevDiscovered & " DAQ Device(s) Discovered"

    If numDevDiscovered > 0 Then
        For BoardNum = 0 To numDevDiscovered - 1
            
            '    Create a new MccBoard object for Board and assign a board number
            '    to the specified DAQ device with CreateDaqDevice()
            
            '    Parameters:
            '        BoardNum           : board number to be assigned to the specified DAQ device
            '        DeviceDescriptor   : device descriptor of the DAQ device
            
            ULStat = cbCreateDaqDevice(BoardNum, Inventory(BoardNum))
            
            If ULStat = 0 Then
                DeviceName = StrConv(Inventory(BoardNum).DevString, vbUnicode)
                
                ' Add the board to combobox
                cmbBoxDiscoveredDevs.AddItem (DeviceName)
            Else
            
                lblStatus.Caption = "cbCreateDaqDevice failed"
            End If
        Next
    End If
    
    If cmbBoxDiscoveredDevs.ListCount > 0 Then
        cmbBoxDiscoveredDevs.Enabled = True
        cmbBoxDiscoveredDevs.ListIndex = 0
        cmdFlashLED.Enabled = True
    Else
        cmbBoxDiscoveredDevs.Enabled = False
        cmdFlashLED.Enabled = False
    End If


    Screen.MousePointer = vbDefault
   
End Sub


Private Sub ReleaseDaqDevices()

    Dim BoardNum As Long

    For BoardNum = 0 To cmbBoxDiscoveredDevs.ListCount - 1
        ' Release resources associated with the specified board number within the Universal Library with cbReleaseDaqDevice()
        '    Parameters:
        '    BoardNum          : board number assigned to the DAQ
        cbReleaseDaqDevice (BoardNum)
    Next
   
End Sub

Private Sub cmbBoxDiscoveredDevs_Click()
    lblDevID.Caption = StrConv(Inventory(cmbBoxDiscoveredDevs.ListIndex).UniqueID, vbUnicode)
End Sub

Private Sub cmdFlashLED_Click()

    Dim BoardNum As Long
    BoardNum = cmbBoxDiscoveredDevs.ListIndex
    
    ' Flash the LED of the specified DAQ device with cbFlashLED()
    
    '   Parameters:
    '      BoardNum            : board number assigned to the DAQ device
    
    
    ULStat = cbFlashLED(BoardNum)
    
End Sub

Private Sub cmdQuit_Click()
    End
End Sub

