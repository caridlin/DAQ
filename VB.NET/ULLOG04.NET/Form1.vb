'ULLOG04.SLN================================================================

' File:                         ULLOG04.SLN

' Library Call Demonstrated:    logger.ConvertFile

' Purpose:                      Converts binary data from MCC logger 
'                               files to text.

' Demonstration:                Converts MCC data found in the
'                               specified file from binary to text.

' Other Library Calls:          cbErrHandling()

' Special Requirements:         There must be an MCC data file in
'                               the indicated parent directory.

' (c) Copyright 2005-2011, Measurement Computing Corp.
' All rights reserved.
'==========================================================================

Public Class frmConvFile

    Inherits System.Windows.Forms.Form

    Private m_Delimiter As MccDaq.FieldDelimiter = MccDaq.FieldDelimiter.Comma
    Private m_Filename As String
    Dim ULStat As MccDaq.ErrorInfo

    Private Sub frmConvFile_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        '  Initiate error handling
        '   activating error handling will trap errors like
        '   bad channel numbers and non-configured conditions.
        '   Parameters:
        '     MccDaq.ErrorReporting.DontPrint :all warnings and errors encountered will be handled locally
        '     MccDaq.ErrorHandling.DontStop   :if an error is encountered, the program will not stop,
        '                                      errors will be handled locally

        ULStat = MccDaq.MccService.ErrHandling _
            (MccDaq.ErrorReporting.DontPrint, MccDaq.ErrorHandling.DontStop)
        If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then Stop

        rbComma.Checked = True

    End Sub

    Private Sub btnConvert_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConvert.Click

        ' create an instance of the data logger
        Dim logger As MccDaq.DataLogger = New MccDaq.DataLogger(m_Filename)

        '  Get the sample info for the first file in the directory
        '   Parameters:
        '     sampleInterval					 :receives the sample interval (time between samples)
        '     sampleCount						 :receives the sample count
        '	   startDate						 :receives the start date
        '	   startTime						 :receives the start time
        Dim sampleInterval As Integer = 0
        Dim sampleCount As Integer = 0
        Dim startDate As Integer = 0
        Dim startTime As Integer = 0

        ULStat = logger.GetSampleInfo(sampleInterval, sampleCount, startDate, startTime)
        If Not ULStat.Value = MccDaq.ErrorInfo.ErrorCode.NoErrors Then
            lblResult.Text = ULStat.Message

        End If

        ' get the destination path from the source file name
        Dim index As Integer = m_Filename.LastIndexOf(".")
        Dim m_DestFilename As String = m_Filename.Substring(0, index + 1) & "csv"

        '  convert the file
        '   Parameters:
        '     m_DestFilename					 :destination file
        '     startSample						 :first sample to convert
        '     sampleCount						 :number of samples to convert
        '	   m_Delimiter						 :field seperator
        Dim startSample As Integer = 0
        If ULStat.Value = MccDaq.ErrorInfo.ErrorCode.NoErrors Then
            m_Filename = logger.FileName
            ULStat = logger.ConvertFile(m_DestFilename, startSample, sampleCount, m_Delimiter)
        End If

        If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then
            lblResult.Text = ULStat.Message
        Else
            lblResult.Text = logger.FileName & " converted to " & m_DestFilename & "."
        End If

    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Close()

    End Sub

    Private Sub Select_Delimiter()
        If rbComma.Checked = True Then
            m_Delimiter = MccDaq.FieldDelimiter.Comma
        ElseIf rbSemiColon.Checked = True Then
            m_Delimiter = MccDaq.FieldDelimiter.Semicolon
        ElseIf rbSpace.Checked = True Then
            m_Delimiter = MccDaq.FieldDelimiter.Space
        ElseIf rbTab.Checked = True Then
            m_Delimiter = MccDaq.FieldDelimiter.Tab
        End If
    End Sub

    Private Sub rbComma_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbComma.CheckedChanged
        Select_Delimiter()
    End Sub

    Private Sub rbSemiColon_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbSemiColon.CheckedChanged
        Select_Delimiter()
    End Sub

    Private Sub rbSpace_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbSpace.CheckedChanged
        Select_Delimiter()
    End Sub

    Private Sub rbTab_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbTab.CheckedChanged
        Select_Delimiter()
    End Sub

    Private Sub btnSelectFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelectFile.Click

        Dim openFileDlg As New OpenFileDialog()
        openFileDlg.InitialDirectory = "..\..\.."
        openFileDlg.Filter = "binary files (*.bin)|*.bin|All files (*.*)|*.*"
        openFileDlg.FilterIndex = 2
        openFileDlg.RestoreDirectory = True

        If openFileDlg.ShowDialog() = Windows.Forms.DialogResult.OK Then
            m_Filename = openFileDlg.FileName
            btnConvert.Enabled = True
        End If
    End Sub

#Region " Windows Form Designer generated code "

    Public Sub New()

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
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents groupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents rbTab As System.Windows.Forms.RadioButton
    Friend WithEvents rbSpace As System.Windows.Forms.RadioButton
    Friend WithEvents rbSemiColon As System.Windows.Forms.RadioButton
    Friend WithEvents rbComma As System.Windows.Forms.RadioButton
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents btnSelectFile As System.Windows.Forms.Button
    Friend WithEvents btnConvert As System.Windows.Forms.Button
    Friend WithEvents lblResult As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.btnConvert = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.groupBox2 = New System.Windows.Forms.GroupBox
        Me.rbTab = New System.Windows.Forms.RadioButton
        Me.rbSpace = New System.Windows.Forms.RadioButton
        Me.rbSemiColon = New System.Windows.Forms.RadioButton
        Me.rbComma = New System.Windows.Forms.RadioButton
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog
        Me.btnSelectFile = New System.Windows.Forms.Button
        Me.lblResult = New System.Windows.Forms.Label
        Me.groupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnConvert
        '
        Me.btnConvert.Enabled = False
        Me.btnConvert.Location = New System.Drawing.Point(12, 182)
        Me.btnConvert.Name = "btnConvert"
        Me.btnConvert.Size = New System.Drawing.Size(75, 23)
        Me.btnConvert.TabIndex = 8
        Me.btnConvert.Text = "Convert"
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(217, 182)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 9
        Me.btnCancel.Text = "Quit"
        '
        'groupBox2
        '
        Me.groupBox2.Controls.Add(Me.rbTab)
        Me.groupBox2.Controls.Add(Me.rbSpace)
        Me.groupBox2.Controls.Add(Me.rbSemiColon)
        Me.groupBox2.Controls.Add(Me.rbComma)
        Me.groupBox2.Location = New System.Drawing.Point(8, 8)
        Me.groupBox2.Name = "groupBox2"
        Me.groupBox2.Size = New System.Drawing.Size(192, 80)
        Me.groupBox2.TabIndex = 7
        Me.groupBox2.TabStop = False
        Me.groupBox2.Text = "Delimiter"
        '
        'rbTab
        '
        Me.rbTab.Location = New System.Drawing.Point(104, 48)
        Me.rbTab.Name = "rbTab"
        Me.rbTab.Size = New System.Drawing.Size(80, 24)
        Me.rbTab.TabIndex = 3
        Me.rbTab.Text = "Tab"
        '
        'rbSpace
        '
        Me.rbSpace.Location = New System.Drawing.Point(104, 24)
        Me.rbSpace.Name = "rbSpace"
        Me.rbSpace.Size = New System.Drawing.Size(80, 24)
        Me.rbSpace.TabIndex = 2
        Me.rbSpace.Text = "Space"
        '
        'rbSemiColon
        '
        Me.rbSemiColon.Location = New System.Drawing.Point(16, 48)
        Me.rbSemiColon.Name = "rbSemiColon"
        Me.rbSemiColon.Size = New System.Drawing.Size(80, 24)
        Me.rbSemiColon.TabIndex = 1
        Me.rbSemiColon.Text = "Semicolon"
        '
        'rbComma
        '
        Me.rbComma.Location = New System.Drawing.Point(16, 24)
        Me.rbComma.Name = "rbComma"
        Me.rbComma.Size = New System.Drawing.Size(80, 24)
        Me.rbComma.TabIndex = 0
        Me.rbComma.Text = "Comma"
        '
        'btnSelectFile
        '
        Me.btnSelectFile.Location = New System.Drawing.Point(217, 33)
        Me.btnSelectFile.Name = "btnSelectFile"
        Me.btnSelectFile.Size = New System.Drawing.Size(75, 23)
        Me.btnSelectFile.TabIndex = 10
        Me.btnSelectFile.Text = "Select file"
        '
        'lblResult
        '
        Me.lblResult.ForeColor = System.Drawing.Color.Blue
        Me.lblResult.Location = New System.Drawing.Point(12, 105)
        Me.lblResult.Name = "lblResult"
        Me.lblResult.Size = New System.Drawing.Size(280, 55)
        Me.lblResult.TabIndex = 11
        '
        'frmConvFile
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(304, 217)
        Me.Controls.Add(Me.lblResult)
        Me.Controls.Add(Me.btnSelectFile)
        Me.Controls.Add(Me.btnConvert)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.groupBox2)
        Me.Name = "frmConvFile"
        Me.Text = "Convert File"
        Me.groupBox2.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

End Class
