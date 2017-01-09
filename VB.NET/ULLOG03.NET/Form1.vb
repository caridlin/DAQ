'ULLOG03.SLN================================================================

' File:                         ULLOG03.SLN

' Library Call Demonstrated:    logger.ReadAIChannels()
'                               logger.ReadDIOChannels
'                               logger.ReadCJCChannels

' Purpose:                      Lists data from logger files.

' Demonstration:                Displays MCC data found in the
'                               specified file.

' Other Library Calls:          cbErrHandling()

' Special Requirements:         There must be an MCC data file in
'                               the indicated parent directory.

' (c) Copyright 2005-2011, Measurement Computing Corp.
' All rights reserved.
'==========================================================================

Public Class frmLoggerData
    Inherits System.Windows.Forms.Form

    Private Const MAX_PATH As Integer = 260
    Private Const m_Path As String = "..\..\.."
    Private logger As MccDaq.DataLogger
    Dim SampleCount As Long
    Dim ULStat As MccDaq.ErrorInfo

    Private Sub frmLoggerData_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim Filename As String = New String(" ", MAX_PATH)
        Dim NullLocation As Long
        Dim sampleInterval As Integer = 0
        Dim startDate As Integer = 0
        Dim startTime As Integer = 0
        Dim Version, Size As Integer

        '  Initiate error handling
        '   activating error handling will trap errors like
        '   bad channel numbers and non-configured conditions.
        '   Parameters:
        '     MccDaq.ErrorReporting.DontPrint :all warnings and errors encountered will be handled locally
        '     MccDaq.ErrorHandling.DontStop   :if an error is encountered, the program will not stop,
        '                                      errors will be handled locally

        ULStat = MccDaq.MccService.ErrHandling _
            (MccDaq.ErrorReporting.DontPrint, MccDaq.ErrorHandling.DontStop)

        '  Get the first file in the directory
        '   Parameters:
        '     MccDaq.GetFileOptions.GetFirst :first file
        '     m_Path						  :path to search
        '	   filename						  :receives name of file

        ULStat = MccDaq.DataLogger.GetFileName(MccDaq.GetFileOptions.GetFirst, m_Path, Filename)
        If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then
            txtData.Text = ULStat.Message
        Else
            'Filename is returned with a null terminator
            'which must be removed for proper display
            Filename = Trim(Filename)
            NullLocation = InStr(1, Filename, Chr(0))
            Filename = Strings.Left(Filename, NullLocation - 1)
            txtData.Text = _
               "The name of the first file found is '" _
               & Filename & "'." & vbCrLf & vbCrLf

            ' create an instance of the data logger
            logger = New MccDaq.DataLogger(Filename)

            '  Get the sample info for the first file in the directory
            '   Parameters:
            '     sampleInterval					 :receives the sample interval (time between samples)
            '     sampleCount						 :receives the sample count
            '	   startDate						 :receives the start date
            '	   startTime						 :receives the start time
            ULStat = logger.GetFileInfo(Version, Size)
            If ULStat.Value = MccDaq.ErrorInfo.ErrorCode.NoErrors Then
                txtData.Text = txtData.Text & vbCrLf & vbCrLf & vbTab & _
                "The version of the file is " & Format(Version, "0") & _
                "." & vbCrLf & vbTab & "The file size is " & Format(Size, "0")
            Else
                txtData.Text = txtData.Text & ULStat.Message
            End If
            ULStat = logger.GetSampleInfo(sampleInterval, SampleCount, startDate, startTime)
            If ULStat.Value = MccDaq.ErrorInfo.ErrorCode.NoErrors Then
                txtData.Text = txtData.Text & vbCrLf & logger.FileName & _
                " contains " & Format(SampleCount, "0") & " samples."
            Else
                txtData.Text = txtData.Text & ULStat.Message
            End If

            '  Set the preferences 
            '    Parameters
            '      timeFormat		:specifies times are 12 or 24 hour format
            '      timeZone			:specifies local time of GMT
            '      units			:specifies Fahrenheit, Celsius, or Kelvin

            Dim timeFormat As MccDaq.TimeFormat = MccDaq.TimeFormat.TwelveHour
            Dim timeZone As MccDaq.TimeZone = MccDaq.TimeZone.Local
            Dim units As MccDaq.TempScale = MccDaq.TempScale.Fahrenheit
            logger.SetPreferences(timeFormat, timeZone, units)
        End If

    End Sub

    Private Sub cmdAnalogData_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAnalogData.Click

        Dim aiChannelCount As Integer = 0
        Dim aiChannelData() As Single
        Dim ChannelNumbers() As Integer
        Dim Units() As Integer
        Dim i, j, ListSize, Index As Integer
        Dim DataListStr, StartTimeStr, lbDataStr As String
        Dim ChansStr, UnitsStr, ChanList, UnitList As String
        Dim PostfixStr, StartDateStr As String
        Dim DateTags() As Integer
        Dim TimeTags() As Integer
        Dim StartSample As Integer = 0
        Dim Hour, Minute, Second, Postfix As Integer
        Dim Month, Day, Year As Integer

        '  Get the ANALOG info for the first file in the directory
        '   Parameters:
        '     channelMask						:receives the channel mask to specify which channels were logged
        '     unitMask							:receives the unit mask to specify temp or raw data

        ULStat = logger.GetAIChannelCount(aiChannelCount)
        If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then
            txtData.Text = ULStat.Message
        Else
            ' Get the Analog information
            '  Parameters:
            '    Filename                :name of file to get information from
            '    ChannelNumbers          :array containing channel numbers that were logged
            '    Units                   :array containing the units for each channel that was logged
            '    AIChannelCount          :number of analog channels logged

            If (aiChannelCount > 0) And (SampleCount > 0) Then
                ReDim ChannelNumbers(aiChannelCount - 1)
                ReDim Units(aiChannelCount - 1)
                ULStat = logger.GetAIInfo(ChannelNumbers, Units)
                ChanList = ""
                UnitList = ""
                If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then
                    txtData.Text = ULStat.Message
                Else
                    For i = 0 To aiChannelCount - 1
                        ChansStr = ChannelNumbers(i)
                        UnitsStr = "Temp"
                        If Units(i) = MccDaq.LoggerUnits.Raw Then UnitsStr = "Raw"
                        ChanList = ChanList & "Chan" & ChansStr & vbTab
                        UnitList = UnitList & UnitsStr & vbTab
                    Next i
                End If
                DataListStr = "Time" & vbTab & vbTab & ChanList & vbCrLf & _
                vbTab & vbTab & UnitList & vbCrLf & vbCrLf
                ReDim DateTags(SampleCount - 1)
                ReDim TimeTags(SampleCount - 1)
                ULStat = logger.ReadTimeTags(StartSample, SampleCount, DateTags, TimeTags)
                If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then
                    txtData.Text = ULStat.Message
                End If
                ReDim aiChannelData((SampleCount * aiChannelCount) - 1)
                ULStat = logger.ReadAIChannels(StartSample, SampleCount, aiChannelData)
                ListSize = SampleCount
                If ListSize > 50 Then ListSize = 50
                PostfixStr = ""
                For i = 0 To ListSize
                    'Parse the date from the StartDate parameter
                    Month = (DateTags(i) / 256) And 255
                    Day = DateTags(i) And 255
                    Year = (DateTags(i) / 65536) And 65535
                    StartDateStr = Format(Month, "00") & "/" & _
                       Format(Day, "00") & "/" & Format(Year, "0000")

                    'Parse the time from the StartTime parameter
                    Hour = (TimeTags(i) / 65536) And 255
                    Minute = (TimeTags(i) / 256) And 255
                    Second = TimeTags(i) And 255
                    Postfix = (TimeTags(i) / 16777216) And 255
                    If Postfix = 0 Then PostfixStr = " AM"
                    If Postfix = 1 Then PostfixStr = " PM"
                    StartTimeStr = Format(Hour, "00") & ":" & _
                       Format(Minute, "00") & ":" & Format(Second, "00") _
                       & Format(PostfixStr, "0")
                    Index = i * aiChannelCount
                    lbDataStr = ""
                    For j = 0 To aiChannelCount - 1
                        lbDataStr = lbDataStr & vbTab & Format(aiChannelData!(Index + j), "0.00")
                    Next j
                    DataListStr = DataListStr & StartDateStr & "  " & StartTimeStr & lbDataStr & vbCrLf
                Next
                txtData.Text = "Analog data from " & logger.FileName & vbCrLf & vbCrLf & DataListStr
            Else
                txtData.Text = "There is no analog data in " & logger.FileName & "."
            End If
        End If

    End Sub

    Private Sub cmdDigitalData_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDigitalData.Click

        Dim Hour, Minute, Second As Integer
        Dim Month, Day, Year As Integer
        Dim Postfix As Integer
        Dim PostfixStr, DataListStr, StartDateStr As String
        Dim StartTimeStr, lbDataStr As String
        Dim StartSample, i, j As Integer
        Dim DateTags(), dioChannelData() As Integer
        Dim TimeTags(), Index As Integer
        Dim dioChannelCount As Integer = 0
        Dim UnitList, ChanList As String
        Dim ListSize As Integer

        ' Get the Digital channel count
        '   Parameters:
        '	   dioChannelCount		:receives the number of DIO chennels logged

        ULStat = logger.GetDIOInfo(dioChannelCount)
        ChanList = ""
        UnitList = ""
        If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then
            txtData.Text = ULStat.Message
        Else
            ReDim dioChannelData(SampleCount * dioChannelCount)
            If (dioChannelCount > 0) And (SampleCount > 0) Then
                DataListStr = "Time" & vbTab & vbTab & ChanList & vbCrLf & _
                   vbTab & vbTab & UnitList & vbCrLf & vbCrLf

                ReDim DateTags(SampleCount - 1)
                ReDim TimeTags(SampleCount - 1)
                ULStat = logger.ReadTimeTags(StartSample, SampleCount, DateTags, TimeTags)

                ReDim dioChannelData((SampleCount * dioChannelCount) - 1)
                ULStat = logger.ReadDIOChannels(StartSample, SampleCount, dioChannelData)

                ListSize = SampleCount
                If ListSize > 50 Then ListSize = 50
                PostfixStr = ""
                For i = 0 To ListSize - 1
                    'Parse the date from the StartDate parameter
                    Month = (DateTags(i) / 256) And 255
                    Day = DateTags(i) And 255
                    Year = (DateTags(i) / 65536) And 65535
                    StartDateStr = Format(Month, "00") & "/" & _
                       Format(Day, "00") & "/" & Format(Year, "0000")

                    'Parse the time from the StartTime parameter
                    Hour = (TimeTags(i) / 65536) And 255
                    Minute = (TimeTags(i) / 256) And 255
                    Second = TimeTags(i) And 255
                    Postfix = (TimeTags(i) / 16777216) And 255
                    If Postfix = 0 Then PostfixStr = " AM"
                    If Postfix = 1 Then PostfixStr = " PM"
                    StartTimeStr = Format(Hour, "00") & ":" & _
                       Format(Minute, "00") & ":" & Format(Second, "00") _
                       & PostfixStr & vbTab
                    Index = i * dioChannelCount
                    lbDataStr = ""
                    For j = 0 To dioChannelCount - 1
                        lbDataStr = lbDataStr & Format(dioChannelData(Index + j), "0")
                    Next j
                    DataListStr = DataListStr & StartDateStr & "  " & StartTimeStr & lbDataStr & vbCrLf
                Next
                txtData.Text = "Digital data from " & logger.FileName & vbCrLf & vbCrLf & DataListStr
            Else
                txtData.Text = "There is no digital data in " & logger.FileName & "."
            End If
        End If

    End Sub

    Private Sub cmdCJCData_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCJCData.Click

        Dim Hour, Minute, Second As Integer
        Dim Month, Day, Year As Integer
        Dim Postfix As Integer
        Dim PostfixStr, StartDateStr, DataListStr As String
        Dim StartTimeStr, lbDataStr As String
        Dim StartSample, i, j As Integer
        Dim DateTags() As Integer
        Dim TimeTags(), Index As Integer
        Dim CJCChannelData() As Single
        Dim CJCChannelCount As Integer = 0
        Dim UnitList, ChanList As String
        Dim ListSize As Integer

        ' Get the Digital channel count
        '   Parameters:
        '	   CJCChannelCount		:receives the number of DIO chennels logged

        ULStat = logger.GetCJCInfo(CJCChannelCount)
        ChanList = ""
        UnitList = ""
        If ULStat.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors Then
            txtData.Text = ULStat.Message
        Else
            ReDim CJCChannelData(SampleCount * CJCChannelCount)
            If (CJCChannelCount > 0) And (SampleCount > 0) Then
                DataListStr = "Time" & vbTab & vbTab & ChanList & vbCrLf & _
                   vbTab & vbTab & UnitList & vbCrLf & vbCrLf

                ReDim DateTags(SampleCount - 1)
                ReDim TimeTags(SampleCount - 1)
                ULStat = logger.ReadTimeTags(StartSample, SampleCount, DateTags, TimeTags)

                ReDim CJCChannelData((SampleCount * CJCChannelCount) - 1)
                ULStat = logger.ReadCJCChannels(StartSample, SampleCount, CJCChannelData)

                ListSize = SampleCount
                If ListSize > 50 Then ListSize = 50
                PostfixStr = ""
                For i = 0 To ListSize - 1
                    'Parse the date from the StartDate parameter
                    Month = (DateTags(i) / 256) And 255
                    Day = DateTags(i) And 255
                    Year = (DateTags(i) / 65536) And 65535
                    StartDateStr = Format(Month, "00") & "/" & _
                       Format(Day, "00") & "/" & Format(Year, "0000")

                    'Parse the time from the StartTime parameter
                    Hour = (TimeTags(i) / 65536) And 255
                    Minute = (TimeTags(i) / 256) And 255
                    Second = TimeTags(i) And 255
                    Postfix = (TimeTags(i) / 16777216) And 255
                    If Postfix = 0 Then PostfixStr = " AM"
                    If Postfix = 1 Then PostfixStr = " PM"
                    StartTimeStr = Format(Hour, "00") & ":" & _
                       Format(Minute, "00") & ":" & Format(Second, "00") _
                       & Format(PostfixStr, "0") & vbTab
                    Index = i * CJCChannelCount
                    lbDataStr = ""
                    For j = 0 To CJCChannelCount - 1
                        lbDataStr = lbDataStr & Format(CJCChannelData(Index + j), "0.00") & vbTab
                    Next j
                    DataListStr = DataListStr & StartDateStr & "  " & StartTimeStr & lbDataStr & vbCrLf
                Next
                txtData.Text = "CJC data from " & logger.FileName & vbCrLf & vbCrLf & DataListStr
            Else
                txtData.Text = "There is no CJC data in " & logger.FileName & "."
            End If
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

        'Add any initialization after the InitializeComponent() call

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
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents txtData As System.Windows.Forms.TextBox
    Friend WithEvents cmdAnalogData As System.Windows.Forms.Button
    Friend WithEvents cmdDigitalData As System.Windows.Forms.Button
    Friend WithEvents cmdCJCData As System.Windows.Forms.Button

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.btnOK = New System.Windows.Forms.Button
        Me.txtData = New System.Windows.Forms.TextBox
        Me.cmdAnalogData = New System.Windows.Forms.Button
        Me.cmdDigitalData = New System.Windows.Forms.Button
        Me.cmdCJCData = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'btnOK
        '
        Me.btnOK.Location = New System.Drawing.Point(629, 155)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(83, 23)
        Me.btnOK.TabIndex = 3
        Me.btnOK.Text = "Quit"
        '
        'txtData
        '
        Me.txtData.ForeColor = System.Drawing.Color.Blue
        Me.txtData.Location = New System.Drawing.Point(12, 12)
        Me.txtData.Multiline = True
        Me.txtData.Name = "txtData"
        Me.txtData.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtData.Size = New System.Drawing.Size(594, 166)
        Me.txtData.TabIndex = 4
        '
        'cmdAnalogData
        '
        Me.cmdAnalogData.Location = New System.Drawing.Point(629, 12)
        Me.cmdAnalogData.Name = "cmdAnalogData"
        Me.cmdAnalogData.Size = New System.Drawing.Size(83, 23)
        Me.cmdAnalogData.TabIndex = 5
        Me.cmdAnalogData.Text = "Analog Data"
        '
        'cmdDigitalData
        '
        Me.cmdDigitalData.Location = New System.Drawing.Point(629, 41)
        Me.cmdDigitalData.Name = "cmdDigitalData"
        Me.cmdDigitalData.Size = New System.Drawing.Size(83, 23)
        Me.cmdDigitalData.TabIndex = 6
        Me.cmdDigitalData.Text = "Digital Data"
        '
        'cmdCJCData
        '
        Me.cmdCJCData.Location = New System.Drawing.Point(629, 70)
        Me.cmdCJCData.Name = "cmdCJCData"
        Me.cmdCJCData.Size = New System.Drawing.Size(83, 23)
        Me.cmdCJCData.TabIndex = 7
        Me.cmdCJCData.Text = "CJC Data"
        '
        'frmLoggerData
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(773, 193)
        Me.Controls.Add(Me.cmdCJCData)
        Me.Controls.Add(Me.cmdDigitalData)
        Me.Controls.Add(Me.cmdAnalogData)
        Me.Controls.Add(Me.txtData)
        Me.Controls.Add(Me.btnOK)
        Me.Name = "frmLoggerData"
        Me.Text = "Logger Data"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

End Class
