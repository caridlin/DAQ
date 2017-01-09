'==============================================================================

'File:                         ULGT03.VB

'Library Call Demonstrated:    MccDaq.MccBoard.BoardConfig property
'                              MccDaq.MccBoard.DioConfig  property 
'                              MccDaq.MccBoard.ExpansionConfig property
'
'Purpose:                      Prints a list of all boards installed in
'                              the system and their base addresses.  Also
'                              prints the addresses of each digital and
'                              counter device on each board and any EXP
'                              boards that are connected to A/D channels.

'Other Library Calls:          MccDaq.MccBoard.GetBoardName()

'==============================================================================
Option Strict Off
Option Explicit On 

Friend Class frmInfoDisplay

    Inherits System.Windows.Forms.Form

    Dim CurrentBoard As Integer
    Dim MaxNumBoards, NumBoards As Integer
    Dim Info As String
    Dim ULStat As MccDaq.ErrorInfo

    Private Sub frmInfoDisplay_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim ConfigVal As Integer

        InitUL()

        'Get the maximum number of boards that can be installed in system
        ConfigVal = MccDaq.GlobalConfig.NumBoards()
        MaxNumBoards = ConfigVal
        txtBoardInfo.Text = vbCrLf & vbCrLf & Space(12) & _
            "Click on 'Print Info' to display board information."

        NumBoards = 0
        CurrentBoard = 0

    End Sub

    Private Sub cmdPrintInfo_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdPrintInfo.Click

        Dim pCurrentBoard As MccDaq.MccBoard
        Dim ConfigVal As Integer

        Info = ""
        'Loop through possible board numbers. If installed,
        '(ConfigVal <> 0), get the board information.
        Do
            pCurrentBoard = New MccDaq.MccBoard(CurrentBoard)
            pCurrentBoard.BoardConfig.GetBoardType(ConfigVal)
            CurrentBoard = CurrentBoard + 1
        Loop While (ConfigVal = 0) And (CurrentBoard < MaxNumBoards)

        If CurrentBoard > MaxNumBoards - 1 Then
            If NumBoards = 0 Then
                Info = vbCrLf & vbCrLf & Space(22) & _
                    "There are no boards installed." & vbCrLf & vbCrLf
                Info = Info & Space(12) & _
                    "You must run InstaCal to install the desired" & vbCrLf
                Info = Info & Space(18) & "boards before running this program."
            Else
                Info = vbCrLf & vbCrLf & Space(16) & _
                    "There are no additional boards installed."
            End If
            cmdPrintInfo.Text = "Print Info"
            CurrentBoard = 0
            NumBoards = 0
        Else
            PrintGenInfo(pCurrentBoard)
            PrintADInfo(pCurrentBoard)
            PrintDAInfo(pCurrentBoard)
            PrintDigInfo(pCurrentBoard)
            PrintCtrInfo(pCurrentBoard)
            PrintExpInfo(pCurrentBoard)
            cmdPrintInfo.Text = "Print Next"
            NumBoards = NumBoards + 1
        End If
        txtBoardInfo.Text = Info

    End Sub

    Private Sub PrintGenInfo(ByRef pBoard As MccDaq.MccBoard)

        Dim BaseAdrStr As String
        Dim ULStat As MccDaq.ErrorInfo
        Dim ConfigVal As Integer
        Dim DevNum As Integer

        DevNum = 0

        'Get board type of each board
        ULStat = pBoard.BoardConfig.GetBoardType(ConfigVal)

        If (ConfigVal > 0) Then 'If a board is installed
            Info = Info & "Board #" & pBoard.BoardNum.ToString("0") _
                & " = " & pBoard.BoardName & " at "

            'Get the board's base address
            ULStat = pBoard.BoardConfig.GetBaseAdr(DevNum, ConfigVal)

            BaseAdrStr = Hex(ConfigVal)
            Info = Info & "Base Address = " & BaseAdrStr _
                & " hex." & vbCrLf & vbCrLf
        End If

    End Sub

    Private Sub PrintADInfo(ByRef pBoard As MccDaq.MccBoard)

        Dim ConfigVal As Integer

        ULStat = pBoard.BoardConfig.GetNumAdChans(ConfigVal)
        If ConfigVal <> 0 Then Info = Info & Space(5) & _
            "Number of A/D channels: " & ConfigVal.ToString("0") _
            & vbCrLf & vbCrLf

    End Sub

    Private Sub PrintDAInfo(ByRef pBoard As MccDaq.MccBoard)

        Dim NumDAChans As Integer
        Dim ConfigVal As Integer

        ULStat = pBoard.BoardConfig.GetNumDaChans(ConfigVal)

        NumDAChans = ConfigVal
        If ConfigVal > 0 Then Info = Info & Space(5) & _
            "Number of D/A channels: " & ConfigVal.ToString("0") _
            & vbCrLf & vbCrLf

    End Sub

    Private Sub PrintDigInfo(ByVal pBoard As MccDaq.MccBoard)

        Dim NumBits As Integer
        Dim NumDevs As Integer
        Dim ConfigVal As Integer
        Dim DevNum As Integer

        'get the number of digital devices for this board
        ULStat = pBoard.BoardConfig.GetDiNumDevs(ConfigVal)
        NumDevs = ConfigVal

        For DevNum = 0 To NumDevs - 1
            'For each digital device, get the base address and number of bits
            ULStat = pBoard.DioConfig.GetNumBits(DevNum, ConfigVal)
            NumBits = ConfigVal
            Info = Info & Space(5) & "Digital Device #" & _
                DevNum.ToString("0") & " : " & NumBits.ToString("0") _
                & " bits" & vbCrLf
        Next
        If Len(Info) <> 0 Then Info = Info & vbCrLf

    End Sub

    Private Sub PrintCtrInfo(ByRef pBoard As MccDaq.MccBoard)

        Dim NumDevs As Integer
        Dim ConfigVal As Integer

        'Get the number of counter devices for this board
        ULStat = pBoard.BoardConfig.GetCiNumDevs(ConfigVal)
        NumDevs = ConfigVal

        If NumDevs > 0 Then Info = Info & Space(5) & "Counters : " _
            & NumDevs.ToString("0") & vbCrLf
        If Len(Info) <> 0 Then Info = Info & vbCrLf

    End Sub

    Private Sub PrintExpInfo(ByRef pBoard As MccDaq.MccBoard)

        Dim ADChan2 As Integer
        Dim ADChan1 As Integer
        Dim BoardType As Integer
        Dim NumDevs As Integer
        Dim ULStat As MccDaq.ErrorInfo
        Dim ConfigVal As Integer
        Dim DevNum As Integer

        ' Get the number of Exps attached to pBoard
        DevNum = 0

        ULStat = pBoard.BoardConfig.GetNumExps(ConfigVal)
        NumDevs = ConfigVal

        For DevNum = 0 To NumDevs - 1
            pBoard.ExpansionConfig.GetBoardType(DevNum, ConfigVal)
            BoardType = ConfigVal
            pBoard.ExpansionConfig.GetMuxAdChan1(DevNum, ConfigVal)
            ADChan1 = ConfigVal
            If BoardType = 770 Then
                'it's a CIO-EXP32
                pBoard.ExpansionConfig.GetMuxAdChan2(DevNum, ConfigVal)
                ADChan2 = ConfigVal
                Info = Info & Space(5) & "A/D channels " & ADChan1.ToString("0") _
                    & " and " & ADChan2.ToString("0") & " connected to EXP(devID=" _
                    & BoardType.ToString("0") & ")." & vbCrLf
            Else
                Info = Info & Space(5) & "A/D channel " & ADChan1.ToString("0") & _
                " connected to EXP(devID=" & BoardType.ToString("0") & ")." & vbCrLf
            End If
        Next
        If Len(Info) <> 0 Then Info = Info & vbCrLf

    End Sub

    Private Sub cmdQuit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdQuit.Click

        End

    End Sub

#Region "Windows Form Designer generated code "

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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer
    Public ToolTip1 As System.Windows.Forms.ToolTip
    Public WithEvents cmdQuit As System.Windows.Forms.Button
    Public WithEvents cmdPrintInfo As System.Windows.Forms.Button
    Public WithEvents txtBoardInfo As System.Windows.Forms.TextBox
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdQuit = New System.Windows.Forms.Button
        Me.cmdPrintInfo = New System.Windows.Forms.Button
        Me.txtBoardInfo = New System.Windows.Forms.TextBox
        Me.SuspendLayout()
        '
        'cmdQuit
        '
        Me.cmdQuit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdQuit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdQuit.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdQuit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdQuit.Location = New System.Drawing.Point(360, 304)
        Me.cmdQuit.Name = "cmdQuit"
        Me.cmdQuit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdQuit.Size = New System.Drawing.Size(57, 25)
        Me.cmdQuit.TabIndex = 1
        Me.cmdQuit.Text = "Quit"
        Me.cmdQuit.UseVisualStyleBackColor = False
        '
        'cmdPrintInfo
        '
        Me.cmdPrintInfo.BackColor = System.Drawing.SystemColors.Control
        Me.cmdPrintInfo.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdPrintInfo.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdPrintInfo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdPrintInfo.Location = New System.Drawing.Point(160, 304)
        Me.cmdPrintInfo.Name = "cmdPrintInfo"
        Me.cmdPrintInfo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdPrintInfo.Size = New System.Drawing.Size(76, 26)
        Me.cmdPrintInfo.TabIndex = 0
        Me.cmdPrintInfo.Text = "Print Info"
        Me.cmdPrintInfo.UseVisualStyleBackColor = False
        '
        'txtBoardInfo
        '
        Me.txtBoardInfo.AcceptsReturn = True
        Me.txtBoardInfo.BackColor = System.Drawing.SystemColors.Window
        Me.txtBoardInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtBoardInfo.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtBoardInfo.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBoardInfo.ForeColor = System.Drawing.Color.Blue
        Me.txtBoardInfo.Location = New System.Drawing.Point(16, 8)
        Me.txtBoardInfo.MaxLength = 0
        Me.txtBoardInfo.Multiline = True
        Me.txtBoardInfo.Name = "txtBoardInfo"
        Me.txtBoardInfo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtBoardInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtBoardInfo.Size = New System.Drawing.Size(401, 289)
        Me.txtBoardInfo.TabIndex = 2
        '
        'frmInfoDisplay
        '
        Me.AcceptButton = Me.cmdPrintInfo
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 13)
        Me.BackColor = System.Drawing.SystemColors.Window
        Me.ClientSize = New System.Drawing.Size(432, 337)
        Me.Controls.Add(Me.cmdQuit)
        Me.Controls.Add(Me.cmdPrintInfo)
        Me.Controls.Add(Me.txtBoardInfo)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Location = New System.Drawing.Point(7, 103)
        Me.Name = "frmInfoDisplay"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Universal Library Configuration Info"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

#Region "Universal Library Initialization - Expand this region to change error handling, etc."

    Private Sub InitUL()

        Dim ULStat As MccDaq.ErrorInfo

        ' declare revision level of Universal Library

        ULStat = MccDaq.MccService.DeclareRevision(MccDaq.MccService.CurrentRevNum)

        ' Initiate error handling
        '  activating error handling will trap errors like
        '  bad channel numbers and non-configured conditions.
        '  Parameters:
        '    MccDaq.ErrorReporting.PrintAll  :all warnings and errors encountered will be printed
        '    MccDaq.ErrorHandling.StopAll   :if any error is encountered, the program will stop

        ULStat = MccDaq.MccService.ErrHandling _
            (MccDaq.ErrorReporting.PrintAll, MccDaq.ErrorHandling.StopAll)
        If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then Stop

    End Sub

#End Region

End Class