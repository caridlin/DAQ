VERSION 5.00
Begin VB.Form frmLogInfo 
   Caption         =   "Log File Information"
   ClientHeight    =   2535
   ClientLeft      =   60
   ClientTop       =   345
   ClientWidth     =   10155
   LinkTopic       =   "Form1"
   ScaleHeight     =   2535
   ScaleWidth      =   10155
   StartUpPosition =   3  'Windows Default
   Begin VB.CommandButton cmdFileInfo 
      Caption         =   "Digital Info"
      Enabled         =   0   'False
      Height          =   375
      Index           =   4
      Left            =   8400
      TabIndex        =   8
      Top             =   1500
      Width           =   1455
   End
   Begin VB.CommandButton cmdFileInfo 
      Caption         =   "CJC Info"
      Enabled         =   0   'False
      Height          =   375
      Index           =   3
      Left            =   8400
      TabIndex        =   7
      Top             =   1020
      Width           =   1455
   End
   Begin VB.CommandButton cmdFileInfo 
      Caption         =   "Analog Chan Info"
      Enabled         =   0   'False
      Height          =   375
      Index           =   2
      Left            =   6660
      TabIndex        =   6
      Top             =   1980
      Width           =   1455
   End
   Begin VB.CommandButton cmdFileInfo 
      Caption         =   "Get Sample Info"
      Enabled         =   0   'False
      Height          =   375
      Index           =   1
      Left            =   6660
      TabIndex        =   5
      Top             =   1500
      Width           =   1455
   End
   Begin VB.CommandButton cmdFileInfo 
      Caption         =   "Get File Info"
      Enabled         =   0   'False
      Height          =   375
      Index           =   0
      Left            =   6660
      TabIndex        =   3
      Top             =   1020
      Width           =   1455
   End
   Begin VB.CommandButton cmdGetFile 
      Caption         =   "Find File"
      Height          =   375
      Left            =   6660
      TabIndex        =   2
      Top             =   120
      Width           =   1455
   End
   Begin VB.TextBox txtResults 
      ForeColor       =   &H00C00000&
      Height          =   1575
      Left            =   120
      MultiLine       =   -1  'True
      ScrollBars      =   2  'Vertical
      TabIndex        =   1
      Top             =   120
      Width           =   6195
   End
   Begin VB.CommandButton btnOK 
      Cancel          =   -1  'True
      Caption         =   "Quit"
      Height          =   375
      Left            =   8400
      TabIndex        =   0
      Top             =   1980
      Width           =   1455
   End
   Begin VB.Label lblComment 
      ForeColor       =   &H00FF0000&
      Height          =   495
      Left            =   180
      TabIndex        =   4
      Top             =   1860
      Width           =   6135
   End
End
Attribute VB_Name = "frmLogInfo"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
'ULLOG02.VBP================================================================

' File:                         ULLOG02.VBP

' Library Call Demonstrated:    cbLogGetSampleInfo()
'                               cbLogGetAIInfo()
'                               cbLogGetCJCInfo()
'                               cbLogGetDIOInfo()

' Purpose:                      Lists data logger file info.

' Demonstration:                Displays MCC data files info
'                               found in the specified file.

' Other Library Calls:          cbErrHandling()

' Special Requirements:         There must be an MCC data file in
'                               the indicated parent directory.

' (c) Copyright 2005-2011, Measurement Computing Corp.
' All rights reserved.
'==========================================================================
Option Explicit

Private Const Path As String = "..\.."
Private Const MAX_PATH As Integer = 260

Private ULStat As Long
Private FileNumber As Long
Private Filename As String

Private Sub Form_Load()

   ' declare revision level of Universal Library

   ULStat = cbDeclareRevision(CURRENTREVNUM)


   ' Initiate error handling
   '  activating error handling will trap errors like
   '  bad channel numbers and non-configured conditions.
   '  Parameters:
   '    DONTPRINT   :all warnings and errors encountered will be handled locally
   '    DONTSTOP    :if an error is encountered, the program will not stop,
   '                  errors must be handled locally
  
   ULStat = cbErrHandling(DONTPRINT, DONTSTOP)
   If ULStat <> 0 Then Stop

   ' If cbErrHandling is set for STOPALL or STOPFATAL during the program
   ' design stage, Visual Basic will be unloaded when an error is encountered.
   ' We suggest trapping errors locally until the program is ready for compiling
   ' to avoid losing unsaved data during program design.  This can be done by
   ' setting cbErrHandling options as above and checking the value of ULStat
   ' after a call to the library. If it is not equal to 0, an error has occurred.
    
End Sub

Private Sub cmdGetFile_Click()

   Dim NullLocation As Long
   Dim i As Integer
   
   Filename = String(MAX_PATH, " ")
   FileNumber = GETFIRST
   
   ULStat = cbLogGetFileName(FileNumber, Path, Filename)
   If ULStat <> 0 Then
      lblComment.Caption = "Error " & Format(ULStat, "0") _
         & " - " & ErrorText(ULStat) & "."
   Else
      'Filename is returned with a null terminator
      'which must be removed for proper display
      Filename = Trim(Filename)
      NullLocation& = InStr(1, Filename, Chr(0))
      Filename = Left(Filename, NullLocation& - 1)
      txtResults.Text = _
         "The name of the first file found is '" _
         & Filename & "'."
      For i% = 0 To cmdFileInfo.Count - 1
         cmdFileInfo(i%).Enabled = True
      Next
   End If
   
End Sub

Private Sub cmdFileInfo_Click(Index As Integer)
   
   Select Case Index
      Case 0
         PrintFileInfo
      Case 1
         PrintSampleInfo
      Case 2
         PrintAnalogInfo
      Case 3
         PrintCJCInfo
      Case 4
         PrintDigitalInfo
   End Select
End Sub

Private Sub PrintSampleInfo()

   Dim Hour As Long, Minute As Long, Second As Long
   Dim Month As Long, Day As Long, Year As Long
   Dim SampleInterval As Long, SampleCount As Long
   Dim StartDate As Long, StartTime As Long
   Dim Postfix As Long
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
   
   ULStat = cbLogGetSampleInfo(Filename, SampleInterval, _
      SampleCount, StartDate, StartTime)
   If ULStat <> 0 Then
      lblComment.Caption = "Error " & Format(ULStat, "0") _
         & " - " & ErrorText(ULStat) & "."
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
         & Format(PostfixStr, "0")
        
      txtResults.Text = _
         "The sample properties of '" & Filename & "' are:" _
         & vbCrLf & vbCrLf & vbTab & "SampleInterval: " & vbTab & _
         Format(SampleInterval, "0") & vbCrLf & vbTab & "SampleCount: " _
         & vbTab & Format(SampleCount, "0") & vbCrLf & vbTab & _
         "Start Date: " & vbTab & StartDateStr & vbCrLf & vbTab & _
         "Start Time: " & vbTab & StartTimeStr
    
   End If

End Sub

Private Sub PrintAnalogInfo()
    
   Dim AIChannelCount As Long
   Dim ChannelNumbers() As Long
   Dim Units() As Long
   Dim ChansStr As String, UnitsStr As String
   Dim ChanList As String
   Dim i As Integer
    
   ' Get the Analog channel count
   '  Parameters:
   '    Filename                :name of file to get information from
   '    AIChannelCount          :number of analog channels logged
    
   ULStat = cbLogGetAIChannelCount(Filename, AIChannelCount)
   If ULStat <> 0 Then
      lblComment.Caption = "Error " & Format(ULStat, "0") _
         & " - " & ErrorText(ULStat) & "."
   Else
      ' Get the Analog information
      '  Parameters:
      '    Filename                :name of file to get information from
      '    ChannelNumbers          :array containing channel numbers that were logged
      '    Units                   :array containing the units for each channel that was logged
      '    AIChannelCount          :number of analog channels logged
       
      ReDim ChannelNumbers(AIChannelCount - 1)
      ReDim Units(AIChannelCount - 1)
       
      ULStat = cbLogGetAIInfo(Filename, ChannelNumbers(0), Units(0))
      If ULStat <> 0 Then
         lblComment.Caption = "Error " & Format(ULStat, "0") _
            & " - " & ErrorText(ULStat) & "."
      Else
         For i = 0 To AIChannelCount - 1
            ChansStr = ChannelNumbers(i)
            UnitsStr = "Temperature"
            If Units(i) = UNITS_RAW Then UnitsStr = "Raw"
            ChanList = ChanList & "Channel " & ChansStr & ": " & vbTab & UnitsStr & vbCrLf & vbTab
         Next i
      End If
      txtResults.Text = _
         "The analog channel properties of '" & Filename & "' are:" _
         & vbCrLf & vbCrLf & vbTab & "Number of channels: " & vbTab & _
         Format(AIChannelCount, "0") & vbCrLf & vbCrLf & vbTab & ChanList
   End If
   
End Sub

Private Sub PrintCJCInfo()
    
   Dim CJCChannelCount As Long
   
   ' Get the CJC information
   '  Parameters:
   '    Filename                :name of file to get information from
   '    CJCChannelCount         :number of CJC channels logged
    
   ULStat = cbLogGetCJCInfo(Filename, CJCChannelCount)
   If ULStat <> 0 Then
      lblComment.Caption = "Error " & Format(ULStat, "0") _
         & " - " & ErrorText(ULStat) & "."
   Else
      txtResults.Text = _
         "The CJC properties of '" & Filename & "' are:" _
         & vbCrLf & vbCrLf & vbTab & "Number of CJC channels: " _
         & vbTab & Format(CJCChannelCount, "0")
   End If

End Sub

Private Sub PrintDigitalInfo()
   
   Dim DIOChannelCount As Long
   
   ' Get the DIO information
   '  Parameters:
   '    Filename                :name of file to get information from
   '    DIOChannelCount         :number of DIO channels logged
   
   ULStat = cbLogGetDIOInfo(Filename, DIOChannelCount)
   If ULStat <> 0 Then
      lblComment.Caption = "Error " & Format(ULStat, "0") _
         & " - " & ErrorText(ULStat) & "."
   Else
      txtResults.Text = _
         "The Digital properties of '" & Filename & "' are:" _
         & vbCrLf & vbCrLf & vbTab & "Number of digital channels: " _
         & vbTab & Format(DIOChannelCount, "0")
   End If

End Sub

Private Sub btnOK_Click()
    
    End

End Sub

Private Sub PrintFileInfo()

   Dim Version As Long, Size As Long
   
   ' Get the file information
   '  Parameters:
   '    Filename    :name of file to get information from
   '    Version     :version of the file
   '    Size        :size of the file
    
   ULStat = cbLogGetFileInfo(Filename, Version, Size)
   If ULStat <> 0 Then
      lblComment.Caption = "Error " & Format(ULStat, "0") _
         & " - " & ErrorText(ULStat) & "."
   Else
      txtResults.Text = _
         "The file properties of '" & Filename & "' are:" _
         & vbCrLf & vbCrLf & vbTab & "Version: " & vbTab & _
         Format(Version, "0") & vbCrLf & vbTab & "Size: " _
         & vbTab & Format(Size, "0")
   End If

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
