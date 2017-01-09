' ==============================================================================

'  File:                         DaqDevDiscovery02.VB

'  Library Call Demonstrated:    MccDaq.DaqDeviceManager.GetNetDeviceDescriptor()
'                                MccDaq.DaqDeviceManager.CreateDaqDevice()
'                                MccDaq.DaqDeviceManager.ReleaseDaqDevice()

'  Purpose:                      Discovers a Network DAQ device and assigns 
'							      board number to the detected device

'  Demonstration:                Displays the detected DAQ device
'							      and flashes the LED of the selected device

'  Other Library Calls:          MccDaq.DaqDeviceManager.IgnoreInstaCal()
'                                MccDaq.MccService.ErrHandling()

' ==============================================================================
Option Strict Off
Option Explicit On

Namespace DaqDevDiscovery02
	Public Class frmDevDiscovery
		Inherits System.Windows.Forms.Form
		Private DaqBoard As MccDaq.MccBoard = Nothing

        Private Sub frmDeviceDiscovery_Load(ByVal eventSender As Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
            InitUL()

            MccDaq.DaqDeviceManager.IgnoreInstaCal()

            txtHost.ForeColor = Color.DarkGray
            txtHost.Text = default_txt
        End Sub

        Private Sub cmdDiscover_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdDiscover.Click

            cmdFlashLED.Enabled = False
            lblDevName.Text = ""
            lblDevID.Text = ""

            If DaqBoard IsNot Nothing Then
                MccDaq.DaqDeviceManager.ReleaseDaqDevice(DaqBoard)
            End If

            Dim host As String = txtHost.Text
            Dim portNum As Integer
            Dim timeout As Integer = 5000

            Dim validPortNum As Boolean = Integer.TryParse(txtPort.Text, portNum)

            If validPortNum Then
                System.Windows.Forms.Cursor.Current = Cursors.WaitCursor

                Try
                    ' Discover an Ethernet DAQ device with GetNetDeviceDescriptor()
                    '  Parameters:
                    '     Host				: Host name or IP address of DAQ device
                    '     Port				: Port Number
                    '     Timeout			: Timeout

                    Dim deviceDescriptor As MccDaq.DaqDeviceDescriptor = MccDaq.DaqDeviceManager.GetNetDeviceDescriptor(host, portNum, timeout)

                    If deviceDescriptor IsNot Nothing Then
                        lblStatus.Text = "DAQ Device Discovered"

                        lblDevName.Text = deviceDescriptor.ProductName
                        lblDevID.Text = deviceDescriptor.UniqueID


                        '    Create a new MccBoard object for Board and assign a board number 
                        '    to the specified DAQ device with CreateDaqDevice()

                        '    Parameters:
                        '        BoardNum			: board number to be assigned to the specified DAQ device
                        '        DeviceDescriptor	: device descriptor of the DAQ device 

                        Dim boardNum As Integer = 0

                        DaqBoard = MccDaq.DaqDeviceManager.CreateDaqDevice(boardNum, deviceDescriptor)

                        cmdFlashLED.Enabled = True
                    End If
                Catch ule As MccDaq.ULException
                    lblStatus.Text = "Error occured: " + ule.Message

                End Try

                System.Windows.Forms.Cursor.Current = Cursors.Default
            Else
                lblStatus.Text = "Invalid port number"
            End If

        End Sub


        Private Sub cmdFlashLED_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdFlashLED.Click

            ' Flash the LED of the specified DAQ device with FlashLED()

            If DaqBoard IsNot Nothing Then
                DaqBoard.FlashLED()
            End If
        End Sub

        Private Sub cmdQuit_Click(ByVal eventSender As Object, ByVal eventArgs As System.EventArgs) Handles cmdQuit.Click
            If DaqBoard IsNot Nothing Then
                MccDaq.DaqDeviceManager.ReleaseDaqDevice(DaqBoard)
            End If

            Application.[Exit]()
        End Sub

#Region "Windows Form Designer generated code"
        ''' <summary>
        ''' Required method for Designer support - do not modify
        ''' the contents of this method with the code editor.
        ''' </summary>

        Private Sub InitializeComponent()
            Me.components = New System.ComponentModel.Container
            Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
            Me.cmdQuit = New System.Windows.Forms.Button
            Me.lblDemoFunction = New System.Windows.Forms.Label
            Me.cmdDiscover = New System.Windows.Forms.Button
            Me.groupBox1 = New System.Windows.Forms.GroupBox
            Me.lblDevName = New System.Windows.Forms.Label
            Me.label1 = New System.Windows.Forms.Label
            Me.cmdFlashLED = New System.Windows.Forms.Button
            Me.lblDevID = New System.Windows.Forms.Label
            Me.label2 = New System.Windows.Forms.Label
            Me.lblStatus = New System.Windows.Forms.Label
            Me.lblHost = New System.Windows.Forms.Label
            Me.txtHost = New System.Windows.Forms.TextBox
            Me.lblPort = New System.Windows.Forms.Label
            Me.txtPort = New System.Windows.Forms.TextBox
            Me.groupBox1.SuspendLayout()
            Me.SuspendLayout()
            '
            'cmdQuit
            '
            Me.cmdQuit.BackColor = System.Drawing.SystemColors.Control
            Me.cmdQuit.Cursor = System.Windows.Forms.Cursors.Default
            Me.cmdQuit.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.cmdQuit.ForeColor = System.Drawing.SystemColors.ControlText
            Me.cmdQuit.Location = New System.Drawing.Point(239, 366)
            Me.cmdQuit.Name = "cmdQuit"
            Me.cmdQuit.RightToLeft = System.Windows.Forms.RightToLeft.No
            Me.cmdQuit.Size = New System.Drawing.Size(52, 26)
            Me.cmdQuit.TabIndex = 6
            Me.cmdQuit.Text = "Quit"
            Me.cmdQuit.UseVisualStyleBackColor = False
            '
            'lblDemoFunction
            '
            Me.lblDemoFunction.BackColor = System.Drawing.SystemColors.Window
            Me.lblDemoFunction.Cursor = System.Windows.Forms.Cursors.Default
            Me.lblDemoFunction.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblDemoFunction.ForeColor = System.Drawing.SystemColors.WindowText
            Me.lblDemoFunction.Location = New System.Drawing.Point(8, 9)
            Me.lblDemoFunction.Name = "lblDemoFunction"
            Me.lblDemoFunction.RightToLeft = System.Windows.Forms.RightToLeft.No
            Me.lblDemoFunction.Size = New System.Drawing.Size(294, 48)
            Me.lblDemoFunction.TabIndex = 2
            Me.lblDemoFunction.Text = "Demonstration of DaqDeviceManager.GetNetDeviceDescriptor() and DaqDeviceManager.C" & _
                "reateDaqDevice() "
            Me.lblDemoFunction.TextAlign = System.Drawing.ContentAlignment.TopCenter
            '
            'cmdDiscover
            '
            Me.cmdDiscover.BackColor = System.Drawing.SystemColors.Control
            Me.cmdDiscover.Font = New System.Drawing.Font("Arial", 8.0!)
            Me.cmdDiscover.Location = New System.Drawing.Point(81, 145)
            Me.cmdDiscover.Name = "cmdDiscover"
            Me.cmdDiscover.Size = New System.Drawing.Size(143, 23)
            Me.cmdDiscover.TabIndex = 11
            Me.cmdDiscover.Text = "Discover DAQ device"
            Me.cmdDiscover.UseVisualStyleBackColor = False
            '
            'groupBox1
            '
            Me.groupBox1.Controls.Add(Me.lblDevName)
            Me.groupBox1.Controls.Add(Me.label1)
            Me.groupBox1.Controls.Add(Me.cmdFlashLED)
            Me.groupBox1.Controls.Add(Me.lblDevID)
            Me.groupBox1.Controls.Add(Me.label2)
            Me.groupBox1.Location = New System.Drawing.Point(15, 222)
            Me.groupBox1.Name = "groupBox1"
            Me.groupBox1.Size = New System.Drawing.Size(276, 132)
            Me.groupBox1.TabIndex = 14
            Me.groupBox1.TabStop = False
            Me.groupBox1.Text = "Discovered Device"
            '
            'lblDevName
            '
            Me.lblDevName.AutoSize = True
            Me.lblDevName.ForeColor = System.Drawing.Color.Green
            Me.lblDevName.Location = New System.Drawing.Point(83, 28)
            Me.lblDevName.Name = "lblDevName"
            Me.lblDevName.Size = New System.Drawing.Size(0, 14)
            Me.lblDevName.TabIndex = 17
            '
            'label1
            '
            Me.label1.AutoSize = True
            Me.label1.Font = New System.Drawing.Font("Arial", 8.0!)
            Me.label1.Location = New System.Drawing.Point(9, 27)
            Me.label1.Name = "label1"
            Me.label1.Size = New System.Drawing.Size(73, 14)
            Me.label1.TabIndex = 16
            Me.label1.Text = "Device Name:"
            '
            'cmdFlashLED
            '
            Me.cmdFlashLED.BackColor = System.Drawing.SystemColors.Control
            Me.cmdFlashLED.Enabled = False
            Me.cmdFlashLED.Font = New System.Drawing.Font("Arial", 8.0!)
            Me.cmdFlashLED.Location = New System.Drawing.Point(100, 92)
            Me.cmdFlashLED.Name = "cmdFlashLED"
            Me.cmdFlashLED.Size = New System.Drawing.Size(75, 23)
            Me.cmdFlashLED.TabIndex = 15
            Me.cmdFlashLED.Text = "Flash LED"
            Me.cmdFlashLED.UseVisualStyleBackColor = False
            '
            'lblDevID
            '
            Me.lblDevID.AutoSize = True
            Me.lblDevID.ForeColor = System.Drawing.Color.Green
            Me.lblDevID.Location = New System.Drawing.Point(98, 59)
            Me.lblDevID.Name = "lblDevID"
            Me.lblDevID.Size = New System.Drawing.Size(0, 14)
            Me.lblDevID.TabIndex = 14
            '
            'label2
            '
            Me.label2.AutoSize = True
            Me.label2.Font = New System.Drawing.Font("Arial", 8.0!)
            Me.label2.Location = New System.Drawing.Point(9, 58)
            Me.label2.Name = "label2"
            Me.label2.Size = New System.Drawing.Size(87, 14)
            Me.label2.TabIndex = 13
            Me.label2.Text = "Device Identifier:"
            '
            'lblStatus
            '
            Me.lblStatus.AutoSize = True
            Me.lblStatus.ForeColor = System.Drawing.Color.Blue
            Me.lblStatus.Location = New System.Drawing.Point(18, 186)
            Me.lblStatus.Name = "lblStatus"
            Me.lblStatus.Size = New System.Drawing.Size(42, 14)
            Me.lblStatus.TabIndex = 15
            Me.lblStatus.Text = "Status"
            '
            'lblHost
            '
            Me.lblHost.AutoSize = True
            Me.lblHost.Font = New System.Drawing.Font("Arial", 8.25!)
            Me.lblHost.Location = New System.Drawing.Point(21, 76)
            Me.lblHost.Name = "lblHost"
            Me.lblHost.Size = New System.Drawing.Size(32, 14)
            Me.lblHost.TabIndex = 16
            Me.lblHost.Text = "Host:"
            '
            'txtHost
            '
            Me.txtHost.ForeColor = System.Drawing.Color.DarkGray
            Me.txtHost.Location = New System.Drawing.Point(57, 73)
            Me.txtHost.Name = "txtHost"
            Me.txtHost.Size = New System.Drawing.Size(228, 20)
            Me.txtHost.TabIndex = 17
            Me.txtHost.Text = "<Host name or IP address>"
            '
            'lblPort
            '
            Me.lblPort.AutoSize = True
            Me.lblPort.Font = New System.Drawing.Font("Arial", 8.25!)
            Me.lblPort.Location = New System.Drawing.Point(24, 109)
            Me.lblPort.Name = "lblPort"
            Me.lblPort.Size = New System.Drawing.Size(29, 14)
            Me.lblPort.TabIndex = 18
            Me.lblPort.Text = "Port:"
            '
            'txtPort
            '
            Me.txtPort.Location = New System.Drawing.Point(59, 106)
            Me.txtPort.Name = "txtPort"
            Me.txtPort.Size = New System.Drawing.Size(100, 20)
            Me.txtPort.TabIndex = 19
            Me.txtPort.Text = "54211"
            '
            'frmDevDiscovery
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(6, 13)
            Me.BackColor = System.Drawing.SystemColors.Window
            Me.ClientSize = New System.Drawing.Size(310, 403)
            Me.Controls.Add(Me.txtPort)
            Me.Controls.Add(Me.lblPort)
            Me.Controls.Add(Me.txtHost)
            Me.Controls.Add(Me.lblHost)
            Me.Controls.Add(Me.lblStatus)
            Me.Controls.Add(Me.groupBox1)
            Me.Controls.Add(Me.cmdDiscover)
            Me.Controls.Add(Me.cmdQuit)
            Me.Controls.Add(Me.lblDemoFunction)
            Me.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.ForeColor = System.Drawing.SystemColors.WindowText
            Me.Location = New System.Drawing.Point(182, 100)
            Me.Name = "frmDevDiscovery"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
            Me.Text = "Universal Library Device Discovery"
            Me.groupBox1.ResumeLayout(False)
            Me.groupBox1.PerformLayout()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub




        Public Sub New()

            ' This call is required by the Windows Form Designer.

            InitializeComponent()
        End Sub

        ' Form overrides dispose to clean up the component list.
        Protected Overrides Sub Dispose(ByVal Disposing As Boolean)
            If Disposing Then
                If components IsNot Nothing Then
                    components.Dispose()
                End If
            End If
            MyBase.Dispose(Disposing)
        End Sub

        ' Required by the Windows Form Designer
        Private components As System.ComponentModel.IContainer
        Public ToolTip1 As ToolTip
        Public WithEvents cmdQuit As Button
        Public lblDemoFunction As Label
        Private WithEvents cmdDiscover As Button
        Private groupBox1 As GroupBox
        Private WithEvents cmdFlashLED As Button
        Private lblDevID As Label
        Private label2 As Label
        Private lblHost As Label
        Private WithEvents txtHost As TextBox
        Private lblPort As Label
        Private txtPort As TextBox
        Private lblDevName As Label
        Private label1 As Label
        Private lblStatus As Label

#End Region

#Region "Universal Library Initialization - Expand to change error handling, etc."

        Private Sub InitUL()

            Dim ULStat As MccDaq.ErrorInfo

            ' Initiate error handling
            '  activating error handling will trap errors like
            '  bad channel numbers and non-configured conditions.
            '  Parameters:
            '    MccDaq.ErrorReporting.PrintAll :all warnings and errors encountered will be printed
            '    MccDaq.ErrorHandling.StopAll   :if any error is encountered, the program will stop

            ReportError = MccDaq.ErrorReporting.PrintAll
            HandleError = MccDaq.ErrorHandling.DontStop
            ULStat = MccDaq.MccService.ErrHandling(ReportError, HandleError)
            If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then
                Stop
            End If

        End Sub

#End Region

        Private default_txt As String = "<Host name or IP address>"

        Private Sub txtHost_Enter(ByVal sender As Object, ByVal e As EventArgs) Handles txtHost.Enter
            If txtHost.Text = default_txt Then
                txtHost.Text = ""
                txtHost.ForeColor = Color.Black
            End If
        End Sub

        Private Sub txtHost_Leave(ByVal sender As Object, ByVal e As EventArgs) Handles txtHost.Leave
            If txtHost.Text = "" Then
                txtHost.ForeColor = Color.DarkGray
                txtHost.Text = default_txt
            End If

        End Sub

	End Class
End Namespace
