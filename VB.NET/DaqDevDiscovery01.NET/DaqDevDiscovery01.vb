' ==============================================================================

'  File:                         DaqDevDiscovery01.VB

'  Library Call Demonstrated:    MccDaq.DaqDeviceManager.GetDaqDeviceInventory()
'                                MccDaq.DaqDeviceManager.CreateDaqDevice()
'                                MccDaq.DaqDeviceManager.ReleaseDaqDevice()

'  Purpose:                      Discovers DAQ devices and assigns 
'							     board number to the detected devices

'  Demonstration:                Displays the detected DAQ devices
'							      and flashes the LED of the selected device

'  Other Library Calls:          MccDaq.DaqDeviceManager.IgnoreInstaCal()
'                                MccDaq.MccService.ErrHandling()

' ==============================================================================
Option Strict Off
Option Explicit On


Namespace DaqDevDiscovery01
	Public Class frmDevDiscovery
		Inherits System.Windows.Forms.Form
        Private Sub frmDeviceDiscovery_Load(ByVal eventSender As Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
            InitUL()

            MccDaq.DaqDeviceManager.IgnoreInstaCal()
        End Sub

        Private Sub cmdDiscover_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdDiscover.Click

            ReleaseDAQDevices()

            lblDevID.Text = ""

            System.Windows.Forms.Cursor.Current = Cursors.WaitCursor

            ' Discover DAQ devices with GetDaqDeviceInventory()
            '  Parameters:
            '    InterfaceType   :interface type of DAQ devices to be discovered

            Dim inventory As MccDaq.DaqDeviceDescriptor() = MccDaq.DaqDeviceManager.GetDaqDeviceInventory(MccDaq.DaqDeviceInterface.Any)

            Dim numDevDiscovered As Integer = inventory.Length

            cmbBoxDiscoveredDevs.Items.Clear()

            lblStatus.Text = numDevDiscovered & " DAQ Device(s) Discovered"

            If numDevDiscovered > 0 Then
                For boardNum As Integer = 0 To numDevDiscovered - 1

                    Try
                        '    Create a new MccBoard object for Board and assign a board number 
                        '    to the specified DAQ device with CreateDaqDevice()

                        '    Parameters:
                        '        BoardNum			: board number to be assigned to the specified DAQ device
                        '        DeviceDescriptor	: device descriptor of the DAQ device 

                        Dim daqBoard As MccDaq.MccBoard = MccDaq.DaqDeviceManager.CreateDaqDevice(boardNum, inventory(boardNum))

                        ' Add the board to combobox
                        cmbBoxDiscoveredDevs.Items.Add(daqBoard)
                    Catch ule As MccDaq.ULException
                        lblStatus.Text = "Error occured: " + ule.Message
                    End Try
                Next
            End If


            If cmbBoxDiscoveredDevs.Items.Count > 0 Then
                cmbBoxDiscoveredDevs.Enabled = True
                cmbBoxDiscoveredDevs.SelectedIndex = 0
                cmdFlashLED.Enabled = True
            Else
                cmbBoxDiscoveredDevs.Enabled = False
                cmdFlashLED.Enabled = False
            End If

            System.Windows.Forms.Cursor.Current = Cursors.Default

        End Sub

        Private Sub cmbBoxDiscoveredDevs_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cmbBoxDiscoveredDevs.SelectedIndexChanged
            Dim daqBoard As MccDaq.MccBoard = DirectCast(cmbBoxDiscoveredDevs.SelectedItem, MccDaq.MccBoard)

            lblDevID.Text = daqBoard.Descriptor.UniqueID
        End Sub

        Private Sub cmdFlashLED_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdFlashLED.Click
            Dim daqBoard As MccDaq.MccBoard = DirectCast(cmbBoxDiscoveredDevs.SelectedItem, MccDaq.MccBoard)

            ' Flash the LED of the specified DAQ device with FlashLED()

            If daqBoard IsNot Nothing Then
                daqBoard.FlashLED()
            End If
        End Sub

        Private Sub ReleaseDAQDevices()
            For Each daqBoard As MccDaq.MccBoard In cmbBoxDiscoveredDevs.Items
                ' Release resources associated with the specified board number within the Universal Library with cbReleaseDaqDevice()
                '    Parameters:
                '     MccBoard:			Board object

                MccDaq.DaqDeviceManager.ReleaseDaqDevice(daqBoard)
            Next
        End Sub




        Private Sub cmdQuit_Click(ByVal eventSender As Object, ByVal eventArgs As System.EventArgs) Handles cmdQuit.Click
            ReleaseDAQDevices()

            Application.[Exit]()
        End Sub

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

#Region "Windows Form Designer generated code"

        Public Sub New()
            MyBase.New()

            'This call is required by the Windows Form Designer.
            InitializeComponent()

        End Sub

        'Form overrides dispose to clean up the component list.
        Protected Overloads Overrides Sub Dispose(ByVal Disposing As Boolean)

            If Disposing Then
                If Not components Is Nothing Then
                    components.Dispose()
                End If
            End If
            MyBase.Dispose(Disposing)

        End Sub

        Private Sub InitializeComponent()
            Me.components = New System.ComponentModel.Container
            Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
            Me.cmdQuit = New System.Windows.Forms.Button
            Me.lblDemoFunction = New System.Windows.Forms.Label
            Me.cmdDiscover = New System.Windows.Forms.Button
            Me.cmbBoxDiscoveredDevs = New System.Windows.Forms.ComboBox
            Me.groupBox1 = New System.Windows.Forms.GroupBox
            Me.cmdFlashLED = New System.Windows.Forms.Button
            Me.lblDevID = New System.Windows.Forms.Label
            Me.label1 = New System.Windows.Forms.Label
            Me.lblStatus = New System.Windows.Forms.Label
            Me.groupBox1.SuspendLayout()
            Me.SuspendLayout()
            '
            'cmdQuit
            '
            Me.cmdQuit.BackColor = System.Drawing.SystemColors.Control
            Me.cmdQuit.Cursor = System.Windows.Forms.Cursors.Default
            Me.cmdQuit.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.cmdQuit.ForeColor = System.Drawing.SystemColors.ControlText
            Me.cmdQuit.Location = New System.Drawing.Point(239, 303)
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
            Me.lblDemoFunction.Text = "Demonstration of DaqDeviceManager.GetDaqDeviceInventory() and DaqDeviceManager.Cr" & _
                "eateDaqDevice() "
            Me.lblDemoFunction.TextAlign = System.Drawing.ContentAlignment.TopCenter
            '
            'cmdDiscover
            '
            Me.cmdDiscover.BackColor = System.Drawing.SystemColors.Control
            Me.cmdDiscover.Font = New System.Drawing.Font("Arial", 8.0!)
            Me.cmdDiscover.Location = New System.Drawing.Point(81, 75)
            Me.cmdDiscover.Name = "cmdDiscover"
            Me.cmdDiscover.Size = New System.Drawing.Size(143, 23)
            Me.cmdDiscover.TabIndex = 11
            Me.cmdDiscover.Text = "Discover DAQ devices"
            Me.cmdDiscover.UseVisualStyleBackColor = False
            '
            'cmbBoxDiscoveredDevs
            '
            Me.cmbBoxDiscoveredDevs.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbBoxDiscoveredDevs.Enabled = False
            Me.cmbBoxDiscoveredDevs.FormattingEnabled = True
            Me.cmbBoxDiscoveredDevs.Location = New System.Drawing.Point(24, 31)
            Me.cmbBoxDiscoveredDevs.Name = "cmbBoxDiscoveredDevs"
            Me.cmbBoxDiscoveredDevs.Size = New System.Drawing.Size(238, 22)
            Me.cmbBoxDiscoveredDevs.TabIndex = 12
            '
            'groupBox1
            '
            Me.groupBox1.Controls.Add(Me.cmdFlashLED)
            Me.groupBox1.Controls.Add(Me.lblDevID)
            Me.groupBox1.Controls.Add(Me.label1)
            Me.groupBox1.Controls.Add(Me.cmbBoxDiscoveredDevs)
            Me.groupBox1.Location = New System.Drawing.Point(15, 150)
            Me.groupBox1.Name = "groupBox1"
            Me.groupBox1.Size = New System.Drawing.Size(276, 135)
            Me.groupBox1.TabIndex = 14
            Me.groupBox1.TabStop = False
            Me.groupBox1.Text = "Discovered Devices"
            '
            'cmdFlashLED
            '
            Me.cmdFlashLED.BackColor = System.Drawing.SystemColors.Control
            Me.cmdFlashLED.Enabled = False
            Me.cmdFlashLED.Font = New System.Drawing.Font("Arial", 8.0!)
            Me.cmdFlashLED.Location = New System.Drawing.Point(100, 101)
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
            Me.lblDevID.Location = New System.Drawing.Point(113, 69)
            Me.lblDevID.Name = "lblDevID"
            Me.lblDevID.Size = New System.Drawing.Size(0, 14)
            Me.lblDevID.TabIndex = 14
            '
            'label1
            '
            Me.label1.AutoSize = True
            Me.label1.Font = New System.Drawing.Font("Arial", 8.0!)
            Me.label1.Location = New System.Drawing.Point(24, 69)
            Me.label1.Name = "label1"
            Me.label1.Size = New System.Drawing.Size(87, 14)
            Me.label1.TabIndex = 13
            Me.label1.Text = "Device Identifier:"
            '
            'lblStatus
            '
            Me.lblStatus.AutoSize = True
            Me.lblStatus.ForeColor = System.Drawing.Color.Blue
            Me.lblStatus.Location = New System.Drawing.Point(18, 119)
            Me.lblStatus.Name = "lblStatus"
            Me.lblStatus.Size = New System.Drawing.Size(42, 14)
            Me.lblStatus.TabIndex = 15
            Me.lblStatus.Text = "Status"
            '
            'frmDevDiscovery
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(6, 13)
            Me.BackColor = System.Drawing.SystemColors.Window
            Me.ClientSize = New System.Drawing.Size(314, 341)
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


        ' Required by the Windows Form Designer
        Private components As System.ComponentModel.IContainer
        Public ToolTip1 As ToolTip
        Public WithEvents cmdQuit As Button
        Public lblDemoFunction As Label
        Private WithEvents cmdDiscover As Button
        Private WithEvents cmbBoxDiscoveredDevs As ComboBox
        Private groupBox1 As GroupBox
        Private WithEvents cmdFlashLED As Button
        Private lblDevID As Label
        Private label1 As Label
        Private lblStatus As Label

#End Region

	End Class
End Namespace
