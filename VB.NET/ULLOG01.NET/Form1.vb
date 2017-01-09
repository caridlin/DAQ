'ULLOG01.SLN================================================================

' File:                         ULLOG01.SLN

' Library Call Demonstrated:    DataLogger.GetFileName

' Purpose:                      Lists data logger files.

' Demonstration:                Displays MCC data files found in the
'                               specified directory.

' Other Library Calls:          MccService.ErrHandling

' Special Requirements:         There must be an MCC data file in
'                               the indicated parent directory.

' (c) Copyright 2005-2011, Measurement Computing Corp.
' All rights reserved.
'==========================================================================

Public Class frmLogFiles

    Inherits System.Windows.Forms.Form

    Private Const MAX_PATH As Integer = 260
    Private Const m_Path As String = "..\\..\\.."
    Private m_FileNumber As Integer = 0

    Private Sub frmLogFiles_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim ULStat As MccDaq.ErrorInfo

        '  Initiate error handling
        '   activating error handling will trap errors like
        '   bad channel numbers and non-configured conditions.
        '   Parameters:
        '     MccDaq.ErrorReporting.DontPrint :all warnings and errors encountered will be handled locally
        '     MccDaq.ErrorHandling.DontStop   :if an error is encountered, allow proceeding to local handler

        ULStat = MccDaq.MccService.ErrHandling _
            (MccDaq.ErrorReporting.DontPrint, MccDaq.ErrorHandling.DontStop)

    End Sub

    Private Sub btnFirstFile_Click(ByVal sender As System.Object, _
    ByVal e As System.EventArgs) Handles btnFirstFile.Click

        Dim filename As String = New String(" ", MAX_PATH)
        Dim errorInfo As MccDaq.ErrorInfo

        lstFileList.Items.Clear()
        lblComment.Text = "Get first file from directory " & m_Path

        '  Get the first file in the directory
        '   Parameters:
        '     MccDaq.GetFileOptions.GetFirst :first file
        '     m_Path						 :path to search
        '	  filename						 :receives name of file

        errorInfo = MccDaq.DataLogger.GetFileName _
            (MccDaq.GetFileOptions.GetFirst, m_Path, filename)

        If (errorInfo.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors) Then
            lblComment.Text = errorInfo.Message
        Else
            ListFiles(filename)
        End If

    End Sub

    Private Sub btnNextFile_Click(ByVal sender As System.Object, _
    ByVal e As System.EventArgs) Handles btnNextFile.Click

        Dim filename As String = New String(" ", MAX_PATH)
        Dim errorInfo As MccDaq.ErrorInfo

        lstFileList.Items.Clear()
        lblComment.Text = "Get next file from directory " & m_Path

        '  Get the next file in the directory
        '   Parameters:
        '     MccDaq.GetFileOptions.GetNext :next file
        '     m_Path						  :path to search
        '	   filename						  :receives name of file

        errorInfo = MccDaq.DataLogger.GetFileName _
            (MccDaq.GetFileOptions.GetNext, m_Path, filename)

        If (errorInfo.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors) Then
            lblComment.Text = errorInfo.Message
        Else
            ListFiles(filename)
        End If

    End Sub

    Private Sub btnFileNumber_Click(ByVal sender As System.Object, _
    ByVal e As System.EventArgs) Handles btnFileNumber.Click

        Dim filename As String = New String(" ", MAX_PATH)
        Dim errorInfo As MccDaq.ErrorInfo
        Dim ValidNumber As Boolean

        lstFileList.Items.Clear()
        ValidNumber = Integer.TryParse(txtFileNum.Text, m_FileNumber)
        lblComment.Text = "Get file number " & m_FileNumber.ToString() _
            & " from directory " & m_Path

        '  Get the Nth file in the directory
        '   Parameters:
        '     m_FileNumber					  :Nth file in the directory 
        '     m_Path						  :path to search
        '	   filename						  :receives name of file

        errorInfo = MccDaq.DataLogger.GetFileName(m_FileNumber, m_Path, filename)

        If (errorInfo.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors) Then
            lblComment.Text = errorInfo.Message
        Else
            ListFiles(filename)
        End If

    End Sub

    Private Sub btnAllFiles_Click(ByVal sender As System.Object, _
    ByVal e As System.EventArgs) Handles btnAllFiles.Click

        Dim filename As String = New String(" ", MAX_PATH)
        Dim errorInfo As MccDaq.ErrorInfo
        Dim ErrorOccurred As Boolean

        lstFileList.Items.Clear()
        lblComment.Text = "Get all files from directory " + m_Path

        '  Get the first file in the directory
        '   Parameters:
        '     MccDaq.GetFileOptions.GetFirst :first file
        '     m_Path						  :path to search
        '	   filename						  :receives name of file

        errorInfo = MccDaq.DataLogger.GetFileName _
            (MccDaq.GetFileOptions.GetFirst, m_Path, filename)

        If (errorInfo.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors) Then
            lblComment.Text = errorInfo.Message
        Else
            ListFiles(filename)
            While (errorInfo.Value <> MccDaq.ErrorInfo.ErrorCode.NoMoreFiles) _
                And Not ErrorOccurred
                '  Get the next file in the directory
                '   Parameters:
                '     MccDaq.GetFileOptions.GetNext :next file
                '     m_Path						  :path to search
                '	   filename						  :receives name of file

                errorInfo = MccDaq.DataLogger.GetFileName _
                    (MccDaq.GetFileOptions.GetNext, m_Path, filename)

                If (errorInfo.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors) Then
                    lblComment.Text = errorInfo.Message
                    ErrorOccurred = True
                Else
                    ListFiles(filename)
                End If
            End While
        End If

    End Sub

    Private Sub ListFiles(ByVal filename As String)

        'Filename is returned with a null terminator
        'which must be removed for proper display
        filename = filename.Trim()
        filename = filename.Trim(Chr(0))
        lstFileList.Items.Add(filename)

    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click

        Close()

    End Sub

#Region " Windows Form Designer generated code "

    Public Sub New()

        'Add any initialization after the InitializeComponent() call
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)

        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)

    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents lblComment As System.Windows.Forms.Label
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents lstFileList As System.Windows.Forms.ListBox
    Friend WithEvents btnAllFiles As System.Windows.Forms.Button
    Friend WithEvents btnFileNumber As System.Windows.Forms.Button
    Friend WithEvents btnNextFile As System.Windows.Forms.Button
    Friend WithEvents btnFirstFile As System.Windows.Forms.Button
    Friend WithEvents txtFileNum As System.Windows.Forms.TextBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.lblComment = New System.Windows.Forms.Label
        Me.btnOK = New System.Windows.Forms.Button
        Me.lstFileList = New System.Windows.Forms.ListBox
        Me.btnAllFiles = New System.Windows.Forms.Button
        Me.btnFileNumber = New System.Windows.Forms.Button
        Me.btnNextFile = New System.Windows.Forms.Button
        Me.btnFirstFile = New System.Windows.Forms.Button
        Me.txtFileNum = New System.Windows.Forms.TextBox
        Me.SuspendLayout()
        '
        'lblComment
        '
        Me.lblComment.ForeColor = System.Drawing.Color.Blue
        Me.lblComment.Location = New System.Drawing.Point(13, 189)
        Me.lblComment.Name = "lblComment"
        Me.lblComment.Size = New System.Drawing.Size(355, 48)
        Me.lblComment.TabIndex = 13
        Me.lblComment.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'btnOK
        '
        Me.btnOK.Location = New System.Drawing.Point(381, 217)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(54, 23)
        Me.btnOK.TabIndex = 12
        Me.btnOK.Text = "Quit"
        '
        'lstFileList
        '
        Me.lstFileList.ForeColor = System.Drawing.Color.Blue
        Me.lstFileList.Location = New System.Drawing.Point(8, 16)
        Me.lstFileList.Name = "lstFileList"
        Me.lstFileList.Size = New System.Drawing.Size(427, 121)
        Me.lstFileList.TabIndex = 11
        '
        'btnAllFiles
        '
        Me.btnAllFiles.Location = New System.Drawing.Point(200, 148)
        Me.btnAllFiles.Name = "btnAllFiles"
        Me.btnAllFiles.Size = New System.Drawing.Size(75, 23)
        Me.btnAllFiles.TabIndex = 10
        Me.btnAllFiles.Text = "All Files"
        '
        'btnFileNumber
        '
        Me.btnFileNumber.Location = New System.Drawing.Point(293, 148)
        Me.btnFileNumber.Name = "btnFileNumber"
        Me.btnFileNumber.Size = New System.Drawing.Size(75, 23)
        Me.btnFileNumber.TabIndex = 9
        Me.btnFileNumber.Text = "File Number"
        '
        'btnNextFile
        '
        Me.btnNextFile.Location = New System.Drawing.Point(104, 148)
        Me.btnNextFile.Name = "btnNextFile"
        Me.btnNextFile.Size = New System.Drawing.Size(75, 23)
        Me.btnNextFile.TabIndex = 8
        Me.btnNextFile.Text = "Next File"
        '
        'btnFirstFile
        '
        Me.btnFirstFile.Location = New System.Drawing.Point(8, 148)
        Me.btnFirstFile.Name = "btnFirstFile"
        Me.btnFirstFile.Size = New System.Drawing.Size(75, 23)
        Me.btnFirstFile.TabIndex = 7
        Me.btnFirstFile.Text = "First File"
        '
        'txtFileNum
        '
        Me.txtFileNum.Location = New System.Drawing.Point(381, 151)
        Me.txtFileNum.Name = "txtFileNum"
        Me.txtFileNum.Size = New System.Drawing.Size(32, 20)
        Me.txtFileNum.TabIndex = 14
        Me.txtFileNum.Text = "0"
        '
        'frmLogFiles
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(448, 246)
        Me.Controls.Add(Me.txtFileNum)
        Me.Controls.Add(Me.lblComment)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.lstFileList)
        Me.Controls.Add(Me.btnAllFiles)
        Me.Controls.Add(Me.btnFileNumber)
        Me.Controls.Add(Me.btnNextFile)
        Me.Controls.Add(Me.btnFirstFile)
        Me.Name = "frmLogFiles"
        Me.Text = "List Logger Files"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

End Class
