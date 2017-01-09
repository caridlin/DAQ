VERSION 5.00
Begin VB.Form frmLoggerData 
   Caption         =   "Logger Data"
   ClientHeight    =   3600
   ClientLeft      =   60
   ClientTop       =   345
   ClientWidth     =   8610
   LinkTopic       =   "Form1"
   ScaleHeight     =   3600
   ScaleWidth      =   8610
   StartUpPosition =   3  'Windows Default
   Begin VB.CommandButton cmdCJCData 
      Caption         =   "CJC Data"
      Height          =   375
      Left            =   7260
      TabIndex        =   4
      Top             =   1440
      Width           =   1095
   End
   Begin VB.CommandButton cmdDigitalData 
      Caption         =   "Digital Data"
      Height          =   375
      Left            =   7260
      TabIndex        =   3
      Top             =   780
      Width           =   1095
   End
   Begin VB.CommandButton cmdAnalogData 
      Caption         =   "Analog Data"
      Height          =   375
      Left            =   7260
      TabIndex        =   2
      Top             =   180
      Width           =   1095
   End
   Begin VB.TextBox txtData 
      ForeColor       =   &H00FF0000&
      Height          =   3195
      Left            =   120
      MultiLine       =   -1  'True
      ScrollBars      =   2  'Vertical
      TabIndex        =   1
      Top             =   120
      Width           =   6915
   End
   Begin VB.CommandButton btnOK 
      Caption         =   "Quit"
      Height          =   375
      Left            =   7260
      TabIndex        =   0
      Top             =   2940
      Width           =   1095
   End
End
Attribute VB_Name = "frmLoggerData"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
'ULLOG03.VBP================================================================

' File:                         ULLOG03.VBP

' Library Call Demonstrated:    cbLogReadAIChannels()
'                               cbLogReadDIOChannels()
'                               cbLogReadCJCChannels()

' Purpose:                      Reads data from MCC logger files.

' Demonstration:                Displays MCC data found in the
'                               specified file.

' Other Library Calls:          cbErrHandling()

' Special Requirements:         There must be an MCC data file in
'                               the indicated parent directory.

' (c) Copyright 2005-2011, Measurement Computing Corp.
' All rights reserved.
'==========================================================================
Option Explicit

Private Const Path = "..\\.."
Private Const MAX_PATH As Integer = 260
Dim Filename As String
Dim SampleCount As Long
Dim ULStat As Long

Private Sub Form_Load()

   Dim NullLocation As Long
   Dim SampleInterval As Long, SampleCount As Long
   Dim StartDate As Long, StartTime As Long
   Dim TimeFormat As Long, TimeZone As Long, Units As Long
   
   ' declare revision level of Universal Library
   ULStat = cbDeclareRevision(CURRENTREVNUM)

   ' Initiate error handling
   '  activating error handling will trap errors like
   '  bad channel numbers and non-configured conditions.
   '  Parameters:
   '    DONTPRINT    :all warnings and errors encountered will be handled locally
   '    DONTSTOP    :if an error is encountered, the program will not stop,
   '                  errors must be handled locally
  
   
   ULStat = cbErrHandling(DONTPRINT, DONTSTOP)
   If ULStat <> 0 Then Stop

   ' If cbErrHandling% is set for STOPALL or STOPFATAL during the program
   ' design stage, Visual Basic will be unloaded when an error is encountered.
   ' We suggest trapping errors locally until the program is ready for compiling
   ' to avoid losing unsaved data during program design.  This can be done by
   ' setting cbErrHandling options as above and checking the value of ULStat
   ' after a call to the library. If it is not equal to 0, an error has occurred.

   Dim FileNumber As Integer

   Filename = String(MAX_PATH, " ")

   FileNumber = GETFIRST
   ULStat = cbLogGetFileName(FileNumber, Path, Filename)
   If ULStat <> 0 Then
      txtData.Text = "Error " & Format(ULStat, "0") _
         & " - " & ErrorText(ULStat) & "."
   Else
      'Filename is returned with a null terminator
      'which must be removed for proper display
      Filename = Trim(Filename)
      NullLocation& = InStr(1, Filename, Chr(0))
      Filename = Left(Filename, NullLocation& - 1)
      txtData.Text = _
         "The name of the first file found is '" _
         & Filename & "'." & vbCrLf & vbCrLf
    
      ' Get the file information
      '  Parameters:
      '    Filename    :name of file to get information from
      '    Version     :version of the file
      '    Size        :size of the file
       
      Dim Version As Long
      Dim Size As Long
      ULStat = cbLogGetFileInfo(Filename, Version, Size)
      If ULStat <> 0 Then
         txtData.Text = txtData.Text & "Error " & Format(ULStat, "0") _
            & " - " & ErrorText(ULStat) & "."
      Else
         txtData.Text = txtData.Text & vbCrLf & vbCrLf & vbTab & _
            "The version of the file is " & Format(Version, "0") & _
            "." & vbCrLf & vbTab & "The file size is " & Format(Size, "0")
      End If
      ' Get the sample information
      '  Parameters:
      '    Filename            :name of file to get information from
      '    SampleInterval      :time between samples
      '    SampleCount         :number of samples in the file
      '    StartDate           :date of first sample
      '    StartTime           :time of first sample
    
      ULStat = cbLogGetSampleInfo(Filename, SampleInterval, SampleCount, StartDate, StartTime)
      If ULStat <> 0 Then
         txtData.Text = txtData.Text & "Error " & Format(ULStat, "0") _
            & " - " & ErrorText(ULStat) & "."
      Else
         txtData.Text = txtData.Text & vbCrLf & vbCrLf & Filename & _
         " contains " & Format(SampleCount, "0") & " samples."
      End If
      TimeFormat& = TIMEFORMAT_12HOUR
      TimeZone& = TIMEZONE_LOCAL
      Units& = UNITS_TEMPERATURE
      ULStat = cbLogSetPreferences(TimeFormat&, TimeZone&, Units&)
   End If

End Sub

Private Sub cmdAnalogData_Click()
   
   Dim Hour As Long, Minute As Long, Second As Long
   Dim Month As Long, Day As Long, Year As Long
   Dim SampleInterval As Long, ListSize As Long
   Dim StartDate As Long, StartTime As Long
   Dim Postfix As Long, DataListStr As String
   Dim PostfixStr As String, StartDateStr As String
   Dim StartTimeStr As String, lbDataStr As String
   Dim StartSample As Long, i As Long, j As Long
   Dim DateTags() As Long, AIChannelData() As Single
   Dim TimeTags() As Long, Index As Long
   Dim AIChannelCount As Long
   Dim ChannelNumbers() As Long
   Dim Units() As Long
   Dim ChansStr As String, UnitsStr As String
   Dim UnitList As String, ChanList As String
   
   ' Get the Analog channel count
   '  Parameters:
   '    Filename                :name of file to get information from
   '    AIChannelCount          :number of analog channels logged
    
   ULStat = cbLogGetAIChannelCount(Filename, AIChannelCount)
   If ULStat <> 0 Then
      txtData.Text = "Error " & Format(ULStat, "0") _
         & " - " & ErrorText(ULStat) & "."
   Else
    
      ' Get the Analog information
      '  Parameters:
      '    Filename                :name of file to get information from
      '    ChannelNumbers          :array containing channel numbers that were logged
      '    Units                   :array containing the units for each channel that was logged
      '    AIChannelCount          :number of analog channels logged
       
      If (AIChannelCount > 0) And (SampleCount > 0) Then
         ReDim ChannelNumbers(AIChannelCount - 1)
         ReDim Units(AIChannelCount - 1)
         ULStat = cbLogGetAIInfo(Filename, ChannelNumbers(0), Units(0))
         If ULStat <> 0 Then
            txtData.Text = "Error " & Format(ULStat, "0") _
               & " - " & ErrorText(ULStat) & "."
         Else
            For i = 0 To AIChannelCount - 1
               ChansStr = ChannelNumbers(i)
               UnitsStr = "Temp"
               If Units(i) = UNITS_RAW Then UnitsStr = "Raw"
               ChanList = ChanList & "Chan" & ChansStr & vbTab
               UnitList = UnitList & UnitsStr & vbTab
            Next i
         End If
         DataListStr = "Time" & vbTab & vbTab & ChanList & vbCrLf & _
            vbTab & vbTab & UnitList & vbCrLf & vbCrLf
      
         ReDim DateTags(SampleCount - 1)
         ReDim TimeTags(SampleCount - 1)
         ULStat = cbLogReadTimeTags(Filename, StartSample, SampleCount, DateTags(0), TimeTags(0))
         If ULStat <> 0 Then Stop
      
         ReDim AIChannelData((SampleCount * AIChannelCount) - 1)
         ULStat = cbLogReadAIChannels(Filename, StartSample, SampleCount, AIChannelData(0))
         If ULStat <> 0 Then Stop
         
         ListSize = SampleCount
         If ListSize > 50 Then ListSize = 50
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
            Index = i * AIChannelCount
            lbDataStr = ""
            For j = 0 To AIChannelCount - 1
               lbDataStr = lbDataStr & vbTab & Format(AIChannelData!(Index + j), "0.00")
            Next j
            DataListStr = DataListStr & StartDateStr & "  " & StartTimeStr & lbDataStr & vbCrLf
         Next
         txtData.Text = "Analog data from " & Filename & vbCrLf & vbCrLf & DataListStr
      Else
         txtData.Text = "There is no analog data in " & Filename & "."
      End If
   End If
   
End Sub

Private Sub cmdDigitalData_Click()
   
   Dim Hour As Long, Minute As Long, Second As Long
   Dim Month As Long, Day As Long, Year As Long
   Dim SampleInterval As Long
   Dim Postfix As Long, DataListStr As String
   Dim PostfixStr As String, StartDateStr As String
   Dim StartTimeStr As String, lbDataStr As String
   Dim StartSample As Long, i As Long, j As Long
   Dim DigChannelCount As Long, ListSize As Long
   Dim DateTags() As Long, DIOChannelData() As Long
   Dim TimeTags() As Long, Index As Long
   Dim ChanList As String, UnitList As String
   
   ' Get the Digital channel count
   '  Parameters:
   '    Filename                :name of file to get information from
   '    DigChannelCount          :number of Digital channels logged
    
   ULStat = cbLogGetDIOInfo(Filename, DigChannelCount)
   If ULStat <> 0 Then Stop
   
   If (DigChannelCount > 0) And (SampleCount > 0) Then
      DataListStr = "Time" & vbTab & vbTab & ChanList & vbCrLf & _
         vbTab & vbTab & UnitList & vbCrLf & vbCrLf
   
      ReDim DateTags(SampleCount - 1)
      ReDim TimeTags(SampleCount - 1)
      ULStat = cbLogReadTimeTags(Filename, StartSample, SampleCount, DateTags(0), TimeTags(0))
      If ULStat <> 0 Then Stop
   
      ReDim DIOChannelData((SampleCount * DigChannelCount) - 1)
      ULStat = cbLogReadDIOChannels(Filename, StartSample, SampleCount, DIOChannelData(0))
      If ULStat <> 0 Then Stop
      
      ListSize = SampleCount
      If ListSize > 50 Then ListSize = 50
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
         Index = i * DigChannelCount
         lbDataStr = ""
         For j = 0 To DigChannelCount - 1
            lbDataStr = lbDataStr & Format(DIOChannelData(Index + j), "0")
         Next j
         DataListStr = DataListStr & StartDateStr & "  " & StartTimeStr & lbDataStr & vbCrLf
      Next
      txtData.Text = "Digital data from " & Filename & vbCrLf & vbCrLf & DataListStr
   Else
      txtData.Text = "There is no digital data in " & Filename & "."
   End If
   
End Sub

Private Sub cmdCJCData_Click()
   
   Dim Hour As Long, Minute As Long, Second As Long
   Dim Month As Long, Day As Long, Year As Long
   Dim SampleInterval As Long
   Dim Postfix As Long, DataListStr As String
   Dim PostfixStr As String, StartDateStr As String
   Dim StartTimeStr As String, lbDataStr As String
   Dim StartSample As Long, i As Long, j As Long
   Dim CJCChannelCount As Long
   Dim DateTags() As Long, CJCChannelData() As Single
   Dim TimeTags() As Long, Index As Long
   Dim ChanList As String, UnitList As String
   Dim ListSize As Long
  
   ' Get the CJC information
   '  Parameters:
   '    Filename                :name of file to get information from
   '    CJCChannelCount         :number of CJC channels logged
    
   ULStat = cbLogGetCJCInfo(Filename, CJCChannelCount)
   If ULStat <> 0 Then Stop
   
   If (CJCChannelCount > 0) And (SampleCount > 0) Then
      DataListStr = "Time" & vbTab & vbTab & ChanList & vbCrLf & _
         vbTab & vbTab & UnitList & vbCrLf & vbCrLf
   
      ReDim DateTags(SampleCount - 1)
      ReDim TimeTags(SampleCount - 1)
      ULStat = cbLogReadTimeTags(Filename, StartSample, SampleCount, DateTags(0), TimeTags(0))
      If ULStat <> 0 Then Stop

      ReDim CJCChannelData((SampleCount * CJCChannelCount) - 1)
      ULStat = cbLogReadCJCChannels(Filename, StartSample, SampleCount, CJCChannelData(0))
      If ULStat <> 0 Then Stop
      
      ListSize = SampleCount
      If ListSize > 50 Then ListSize = 50
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
      txtData.Text = "CJC data from " & Filename & vbCrLf & vbCrLf & DataListStr
   Else
      txtData.Text = "There is no CJC data in " & Filename & "."
   End If

End Sub

Private Sub btnOK_Click()
    
    End

End Sub

Private Function ErrorText(ByVal ErrorNumber As Long) As String

   Dim ErrMsg As String
   Dim NullLocation As Long
   
   'Initialize a string large enough to hold
   'the error message returned by cbGetErrMsg()
   ErrMsg = String(ERRSTRLEN, " ")
   ULStat = cbGetErrMsg(ErrorNumber, ErrMsg)
   NullLocation& = InStr(1, ErrMsg, Chr(0))
   ErrorText = Left(ErrMsg, NullLocation& - 1)
   
End Function

