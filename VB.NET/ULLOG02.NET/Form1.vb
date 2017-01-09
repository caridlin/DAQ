'ULLOG02.SLN================================================================

' File:                         ULLOG02.SLN

' Library Call Demonstrated:    logger.GetSampleInfo
'                               logger.GetAIInfo
'                               logger.GetCJCInfo
'                               logger.GetDIOInfo

' Purpose:                      Lists data from logger files.

' Demonstration:                Displays MCC data found in the
'                               specified file.

' Other Library Calls:          cbErrHandling()

' Special Requirements:         There must be an MCC data file in
'                               the indicated parent directory.

' (c) Copyright 2005-2011, Measurement Computing Corp.
' All rights reserved.
'==========================================================================

Public Class frmLogInfo

    Inherits System.Windows.Forms.Form

    Private Const MAX_PATH As Integer = 260
    Private Const m_Path As String = "..\..\.."
    Dim ULStat As MccDaq.ErrorInfo
    Dim logger As MccDaq.DataLogger

    Private Sub frmLogInfo_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        'Add any initialization after the InitializeComponent() call

        '  Initiate error handling
        '   activating error handling will trap errors like
        '   bad channel numbers and non-configured conditions.
        '   Parameters:
        '     MccDaq.ErrorReporting.DontPrint : all warnings and errors encountered will be handled locally
        '     MccDaq.ErrorHandling.DontStop   : if an error is encountered, the program will not stop,
        '                                       errors must be handled locally

        ULStat = MccDaq.MccService.ErrHandling _
            (MccDaq.ErrorReporting.DontPrint, MccDaq.ErrorHandling.DontStop)

    End Sub

    Private Sub cmdGetFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdGetFile.Click

        Dim NullLocation As Integer
        Dim filename As String = New String(" ", MAX_PATH)

        '  Get the first file in the directory
        '   Parameters:
        '     MccDaq.GetFileOptions.GetFirst  :first file
        '     m_Path						  :path to search
        '	  filename						  :receives name of file

        ULStat = MccDaq.DataLogger.GetFileName(MccDaq.GetFileOptions.GetFirst, m_Path, filename)
        If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then
            lblComment.Text = ULStat.Message
        Else
            'Filename is returned with a null terminator
            'which must be removed for proper display
            filename = Strings.Trim(filename)
            NullLocation = InStr(1, filename, Chr(0))
            filename = Strings.Left(filename, NullLocation - 1)
            ' create an instance of the data logger
            logger = New MccDaq.DataLogger(filename)
            txtResults.Text = _
               "The name of the first file found is '" _
               & logger.FileName & "'."
            cmdFileInfo.Enabled = True
            cmdAnalogInfo.Enabled = True
            cmdCJCInfo.Enabled = True
            cmdDigitalInfo.Enabled = True
            cmdSampInfo.Enabled = True
        End If

    End Sub

    Private Sub cmdFileInfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdFileInfo.Click

        Dim version As Integer = 0
        Dim size As Integer = 0

        '  Get the file info for the first file in the directory
        '   Parameters:
        '     filename						  :file to retrieve information from
        '     version						  :receives the version of the file
        '	  size							  :receives the size of file

        ULStat = logger.GetFileInfo(version, size)
        If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then
            lblComment.Text = ULStat.Message
        Else
            txtResults.Text = _
               "The file properties of '" & logger.FileName & "' are:" _
               & vbCrLf & vbCrLf & vbTab & "Version: " & vbTab & _
               Format(version, "0") & vbCrLf & vbTab & "Size: " _
               & vbTab & Format(size, "0")
        End If

    End Sub

    Private Sub cmdSampInfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSampInfo.Click

        Dim Hour, Minute, Second As Integer
        Dim Month, Day, Year As Integer
        Dim SampleInterval, SampleCount As Integer
        Dim StartDate, StartTime As Integer
        Dim Postfix As Integer
        Dim PostfixStr As String, StartDateStr As String
        Dim StartTimeStr As String

        PostfixStr = ""

        ' Get the sample information
        '  Parameters:
        '    Filename            :name of file to get information from
        '    SampleInterval      :time between samples
        '    SampleCount         :number of samples in the file
        '    StartDate           :date of first sample
        '    StartTime           :time of first sample

        ULStat = logger.GetSampleInfo(SampleInterval, SampleCount, StartDate, StartTime)
        If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then
            lblComment.Text = ULStat.Message & "."
        Else
            'Parse the date from the StartDate parameter
            Month = (StartDate / 256) And 255
            Day = StartDate And 255
            Year = (StartDate / 65536) And 65535
            StartDateStr = Format(Month, "00") & "/" & _
               Format(Day, "00") & "/" & Format(Year, "0000")

            'Parse the time from the StartTime parameter
            Hour = (StartTime / 65536) And 255
            Minute = (StartTime / 256) And 255
            Second = StartTime And 255
            Postfix = (StartTime / 16777216) And 255
            If Postfix = 0 Then PostfixStr = " AM"
            If Postfix = 1 Then PostfixStr = " PM"
            StartTimeStr = Format(Hour, "00") & ":" & _
               Format(Minute, "00") & ":" & Format(Second, "00") _
               & PostfixStr

            txtResults.Text = _
               "The sample properties of '" & logger.FileName & "' are:" _
               & vbCrLf & vbCrLf & vbTab & "SampleInterval: " & vbTab & _
               Format(SampleInterval, "0") & vbCrLf & vbTab & "SampleCount: " _
               & vbTab & Format(SampleCount, "0") & vbCrLf & vbTab & _
               "Start Date: " & vbTab & StartDateStr & vbCrLf & vbTab & _
               "Start Time: " & vbTab & StartTimeStr

        End If

    End Sub

    Private Sub cmdAnalogInfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAnalogInfo.Click

        Dim AIChannelCount As Integer
        Dim ChannelNumbers() As Integer
        Dim Units() As MccDaq.LoggerUnits
        Dim ChansStr As String, UnitsStr As String
        Dim ChanList As String = ""
        Dim i As Short

        ' Get the Analog channel count
        '  Parameters:
        '    Filename                :name of file to get information from
        '    AIChannelCount          :number of analog channels logged

        ULStat = logger.GetAIChannelCount(AIChannelCount)
        If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then
            lblComment.Text = ULStat.Message & "."
        Else
            ' Get the Analog information
            '  Parameters:
            '    Filename                :name of file to get information from
            '    ChannelNumbers          :array containing channel numbers that were logged
            '    Units                   :array containing the units for each channel that was logged
            '    AIChannelCount          :number of analog channels logged

            ReDim ChannelNumbers(AIChannelCount - 1)
            ReDim Units(AIChannelCount - 1)

            ULStat = logger.GetAIInfo(ChannelNumbers, Units)
            If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then
                lblComment.Text = ULStat.Message & "."
            Else
                For i = 0 To AIChannelCount - 1
                    ChansStr = ChannelNumbers(i)
                    UnitsStr = "Temperature"
                    If Units(i) = MccDaq.LoggerUnits.Raw Then UnitsStr = "Raw"
                    ChanList = ChanList & "Channel " & ChansStr & ": " & vbTab & UnitsStr & vbCrLf & vbTab
                Next i
            End If
            txtResults.Text = _
               "The analog channel properties of '" & logger.FileName & "' are:" _
               & vbCrLf & vbCrLf & vbTab & "Number of channels: " & vbTab & _
               Format(AIChannelCount, "0") & vbCrLf & vbCrLf & vbTab & ChanList
        End If

    End Sub

    Private Sub cmdCJCInfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCJCInfo.Click

        Dim cjcChannelCount As Integer = 0

        ' Get the CJC information
        '  Parameters:
        '    CJCChannelCount         :number of CJC channels logged

        ULStat = logger.GetCJCInfo(cjcChannelCount)
        If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then
            lblComment.Text = ULStat.Message & "."
        Else
            txtResults.Text = _
               "The CJC properties of '" & logger.FileName & "' are:" _
               & vbCrLf & vbCrLf & vbTab & "Number of CJC channels: " _
               & vbTab & Format(cjcChannelCount, "0")
        End If

    End Sub

    Private Sub cmdDigitalInfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDigitalInfo.Click

        Dim dioChannelCount As Integer = 0

        ULStat = logger.GetDIOInfo(dioChannelCount)
        If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then
            lblComment.Text = ULStat.Message & "."
        Else
            txtResults.Text = _
               "The Digital properties of '" & logger.FileName & "' are:" _
               & vbCrLf & vbCrLf & vbTab & "Number of digital channels: " _
               & vbTab & Format(dioChannelCount, "0")
        End If

    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click

        End

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
    Friend WithEvents lblComment As System.Windows.Forms.Label
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents txtResults As System.Windows.Forms.TextBox
    Friend WithEvents cmdGetFile As System.Windows.Forms.Button
    Friend WithEvents cmdFileInfo As System.Windows.Forms.Button
    Friend WithEvents cmdSampInfo As System.Windows.Forms.Button
    Friend WithEvents cmdAnalogInfo As System.Windows.Forms.Button
    Friend WithEvents cmdCJCInfo As System.Windows.Forms.Button
    Friend WithEvents cmdDigitalInfo As System.Windows.Forms.Button

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.lblComment = New System.Windows.Forms.Label
        Me.btnOK = New System.Windows.Forms.Button
        Me.txtResults = New System.Windows.Forms.TextBox
        Me.cmdGetFile = New System.Windows.Forms.Button
        Me.cmdFileInfo = New System.Windows.Forms.Button
        Me.cmdSampInfo = New System.Windows.Forms.Button
        Me.cmdAnalogInfo = New System.Windows.Forms.Button
        Me.cmdCJCInfo = New System.Windows.Forms.Button
        Me.cmdDigitalInfo = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'lblComment
        '
        Me.lblComment.ForeColor = System.Drawing.Color.Blue
        Me.lblComment.Location = New System.Drawing.Point(16, 125)
        Me.lblComment.Name = "lblComment"
        Me.lblComment.Size = New System.Drawing.Size(374, 37)
        Me.lblComment.TabIndex = 81
        '
        'btnOK
        '
        Me.btnOK.Location = New System.Drawing.Point(535, 139)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(120, 23)
        Me.btnOK.TabIndex = 79
        Me.btnOK.Text = "Quit"
        '
        'txtResults
        '
        Me.txtResults.ForeColor = System.Drawing.Color.Blue
        Me.txtResults.Location = New System.Drawing.Point(12, 12)
        Me.txtResults.Multiline = True
        Me.txtResults.Name = "txtResults"
        Me.txtResults.Size = New System.Drawing.Size(378, 100)
        Me.txtResults.TabIndex = 85
        '
        'cmdGetFile
        '
        Me.cmdGetFile.Location = New System.Drawing.Point(405, 12)
        Me.cmdGetFile.Name = "cmdGetFile"
        Me.cmdGetFile.Size = New System.Drawing.Size(120, 23)
        Me.cmdGetFile.TabIndex = 86
        Me.cmdGetFile.Text = "Find File"
        Me.cmdGetFile.UseVisualStyleBackColor = True
        '
        'cmdFileInfo
        '
        Me.cmdFileInfo.Enabled = False
        Me.cmdFileInfo.Location = New System.Drawing.Point(405, 70)
        Me.cmdFileInfo.Name = "cmdFileInfo"
        Me.cmdFileInfo.Size = New System.Drawing.Size(120, 23)
        Me.cmdFileInfo.TabIndex = 87
        Me.cmdFileInfo.Text = "Get File Info"
        Me.cmdFileInfo.UseVisualStyleBackColor = True
        '
        'cmdSampInfo
        '
        Me.cmdSampInfo.Enabled = False
        Me.cmdSampInfo.Location = New System.Drawing.Point(404, 105)
        Me.cmdSampInfo.Name = "cmdSampInfo"
        Me.cmdSampInfo.Size = New System.Drawing.Size(120, 23)
        Me.cmdSampInfo.TabIndex = 88
        Me.cmdSampInfo.Text = "Get Sample Info"
        Me.cmdSampInfo.UseVisualStyleBackColor = True
        '
        'cmdAnalogInfo
        '
        Me.cmdAnalogInfo.Enabled = False
        Me.cmdAnalogInfo.Location = New System.Drawing.Point(404, 140)
        Me.cmdAnalogInfo.Name = "cmdAnalogInfo"
        Me.cmdAnalogInfo.Size = New System.Drawing.Size(120, 23)
        Me.cmdAnalogInfo.TabIndex = 89
        Me.cmdAnalogInfo.Text = "Get Analog Chan Info"
        Me.cmdAnalogInfo.UseVisualStyleBackColor = True
        '
        'cmdCJCInfo
        '
        Me.cmdCJCInfo.Enabled = False
        Me.cmdCJCInfo.Location = New System.Drawing.Point(535, 70)
        Me.cmdCJCInfo.Name = "cmdCJCInfo"
        Me.cmdCJCInfo.Size = New System.Drawing.Size(120, 23)
        Me.cmdCJCInfo.TabIndex = 90
        Me.cmdCJCInfo.Text = "Get CJC Info"
        Me.cmdCJCInfo.UseVisualStyleBackColor = True
        '
        'cmdDigitalInfo
        '
        Me.cmdDigitalInfo.Enabled = False
        Me.cmdDigitalInfo.Location = New System.Drawing.Point(535, 105)
        Me.cmdDigitalInfo.Name = "cmdDigitalInfo"
        Me.cmdDigitalInfo.Size = New System.Drawing.Size(120, 23)
        Me.cmdDigitalInfo.TabIndex = 91
        Me.cmdDigitalInfo.Text = "Get Digital Info"
        Me.cmdDigitalInfo.UseVisualStyleBackColor = True
        '
        'frmLogInfo
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(667, 178)
        Me.Controls.Add(Me.cmdDigitalInfo)
        Me.Controls.Add(Me.cmdCJCInfo)
        Me.Controls.Add(Me.cmdAnalogInfo)
        Me.Controls.Add(Me.cmdSampInfo)
        Me.Controls.Add(Me.cmdFileInfo)
        Me.Controls.Add(Me.cmdGetFile)
        Me.Controls.Add(Me.txtResults)
        Me.Controls.Add(Me.lblComment)
        Me.Controls.Add(Me.btnOK)
        Me.Name = "frmLogInfo"
        Me.Text = "Log File Information"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

End Class
