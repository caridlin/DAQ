'==============================================================================

'File:                         ULGT04.VB

'Library Call Demonstrated:     MccDaq.MccService.GetBoardName()


'Purpose:                      Prints a list of all boards installed in
'                              the system.  Prints a list of all supported
'                              boards.

'Other Library Calls:          MccDaq.MccService.ErrHandling()
'                              MccDaq.MccBoard.BoardName property
'                              MccDaq.GlobalConfig.NumBoards property  

'==============================================================================
Option Strict Off
Option Explicit On 

Public Class frmListBoards

    Inherits System.Windows.Forms.Form

    Dim ULStat As MccDaq.ErrorInfo
    Dim NumBoards As Integer

    Private Sub frmListBoards_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        InitUL()

        'Get the maximum number of boards allowed
        NumBoards = MccDaq.GlobalConfig.NumBoards

    End Sub

    Private Sub cmdListInstalled_Click(ByVal eventSender As System.Object, _
    ByVal eventArgs As System.EventArgs) Handles cmdListInstalled.Click
        Dim ULStat As MccDaq.ErrorInfo
        Dim BoardNum As Integer
        Dim typeVal As Integer
        Dim pBoard(NumBoards) As MccDaq.MccBoard

        'Get board type of each board currently installed

        txtListBoards.Text = "Currently installed boards:" & vbCrLf & vbCrLf
        For BoardNum = 0 To NumBoards - 1

            pBoard(BoardNum) = New MccDaq.MccBoard(BoardNum)
            ULStat = pBoard(BoardNum).BoardConfig.GetBoardType(typeVal)
            If typeVal <> 0 Then
                txtListBoards.Text = txtListBoards.Text & _
                    "Board #" & (BoardNum).ToString("0") & "= " & pBoard(BoardNum).BoardName & vbCrLf
            End If
        Next

    End Sub

    Private Sub cmdListSupported_Click(ByVal eventSender As System.Object, _
    ByVal eventArgs As System.EventArgs) Handles cmdListSupported.Click

        Dim BoardList As String
        Dim ULStat As MccDaq.ErrorInfo
        Dim BoardName As String

        txtListBoards.Text = ""

        'Get the first board type in list of supported boards
        BoardName = Space(MccDaq.MccService.BoardNameLen)

        ULStat = MccDaq.MccService.GetBoardName(MccDaq.MccService.GetFirst, BoardName)
        BoardList = "The first string in the board name list is:" & _
            vbCrLf & vbCrLf & BoardName & vbCrLf & vbCrLf & _
            "Using 'GetNext', the following list of boards is retrieved:" & vbCrLf & vbCrLf

        'Get each consecutive board type in list
        Do
            BoardName = Space(MccDaq.MccService.BoardNameLen)
            ULStat = MccDaq.MccService.GetBoardName(MccDaq.MccService.GetNext, BoardName)
            BoardList = BoardList & BoardName & vbCrLf
        Loop While Len(BoardName) > 3

        txtListBoards.Text = BoardList

    End Sub

    Private Sub cmdQuit_Click(ByVal eventSender As System.Object, _
    ByVal eventArgs As System.EventArgs) Handles cmdQuit.Click

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
    Public WithEvents cmdListSupported As System.Windows.Forms.Button
    Public WithEvents cmdListInstalled As System.Windows.Forms.Button
    Public WithEvents txtListBoards As System.Windows.Forms.TextBox
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdQuit = New System.Windows.Forms.Button
        Me.cmdListSupported = New System.Windows.Forms.Button
        Me.cmdListInstalled = New System.Windows.Forms.Button
        Me.txtListBoards = New System.Windows.Forms.TextBox
        Me.SuspendLayout()
        '
        'cmdQuit
        '
        Me.cmdQuit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdQuit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdQuit.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdQuit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdQuit.Location = New System.Drawing.Point(279, 312)
        Me.cmdQuit.Name = "cmdQuit"
        Me.cmdQuit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdQuit.Size = New System.Drawing.Size(65, 25)
        Me.cmdQuit.TabIndex = 2
        Me.cmdQuit.Text = "&Quit"
        Me.cmdQuit.UseVisualStyleBackColor = False
        '
        'cmdListSupported
        '
        Me.cmdListSupported.BackColor = System.Drawing.SystemColors.Control
        Me.cmdListSupported.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdListSupported.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdListSupported.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdListSupported.Location = New System.Drawing.Point(8, 312)
        Me.cmdListSupported.Name = "cmdListSupported"
        Me.cmdListSupported.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdListSupported.Size = New System.Drawing.Size(190, 25)
        Me.cmdListSupported.TabIndex = 1
        Me.cmdListSupported.Text = "List Supported Boards"
        Me.cmdListSupported.UseVisualStyleBackColor = False
        '
        'cmdListInstalled
        '
        Me.cmdListInstalled.BackColor = System.Drawing.SystemColors.Control
        Me.cmdListInstalled.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdListInstalled.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdListInstalled.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdListInstalled.Location = New System.Drawing.Point(8, 280)
        Me.cmdListInstalled.Name = "cmdListInstalled"
        Me.cmdListInstalled.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdListInstalled.Size = New System.Drawing.Size(190, 25)
        Me.cmdListInstalled.TabIndex = 0
        Me.cmdListInstalled.Text = "List Installed Boards"
        Me.cmdListInstalled.UseVisualStyleBackColor = False
        '
        'txtListBoards
        '
        Me.txtListBoards.AcceptsReturn = True
        Me.txtListBoards.BackColor = System.Drawing.SystemColors.Window
        Me.txtListBoards.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtListBoards.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtListBoards.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtListBoards.ForeColor = System.Drawing.Color.Blue
        Me.txtListBoards.Location = New System.Drawing.Point(8, 8)
        Me.txtListBoards.MaxLength = 0
        Me.txtListBoards.Multiline = True
        Me.txtListBoards.Name = "txtListBoards"
        Me.txtListBoards.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtListBoards.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtListBoards.Size = New System.Drawing.Size(336, 257)
        Me.txtListBoards.TabIndex = 3
        '
        'frmListBoards
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 13)
        Me.BackColor = System.Drawing.SystemColors.Window
        Me.ClientSize = New System.Drawing.Size(356, 349)
        Me.Controls.Add(Me.cmdQuit)
        Me.Controls.Add(Me.cmdListSupported)
        Me.Controls.Add(Me.cmdListInstalled)
        Me.Controls.Add(Me.txtListBoards)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Location = New System.Drawing.Point(7, 103)
        Me.Name = "frmListBoards"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Universal Library List Boards"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

#Region "Universal Library Initialization - Expand this region to change error handling, etc."

    Private Sub InitUL()

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