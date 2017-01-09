VERSION 5.00
Begin VB.Form frmLogFiles 
   Caption         =   "List Logger Files"
   ClientHeight    =   3285
   ClientLeft      =   60
   ClientTop       =   345
   ClientWidth     =   7245
   LinkTopic       =   "Form1"
   ScaleHeight     =   3285
   ScaleWidth      =   7245
   StartUpPosition =   3  'Windows Default
   Begin VB.TextBox txtFileNum 
      Height          =   315
      Left            =   6060
      TabIndex        =   6
      Text            =   "0"
      Top             =   2160
      Width           =   495
   End
   Begin VB.CommandButton cmdFileNum 
      Caption         =   "File Number"
      Default         =   -1  'True
      Height          =   375
      Left            =   4620
      TabIndex        =   5
      Top             =   2100
      Width           =   1215
   End
   Begin VB.CommandButton cmdNext 
      Caption         =   "Next File"
      Height          =   375
      Left            =   1620
      TabIndex        =   4
      Top             =   2100
      Width           =   1215
   End
   Begin VB.CommandButton cmdFirst 
      Caption         =   "First File"
      Height          =   375
      Left            =   180
      TabIndex        =   3
      Top             =   2100
      Width           =   1215
   End
   Begin VB.CommandButton cmdQuit 
      Cancel          =   -1  'True
      Caption         =   "Quit"
      Height          =   375
      Left            =   6180
      TabIndex        =   2
      Top             =   2820
      Width           =   915
   End
   Begin VB.ListBox lstFiles 
      ForeColor       =   &H00FF0000&
      Height          =   1815
      ItemData        =   "ULLOG01.frx":0000
      Left            =   120
      List            =   "ULLOG01.frx":0002
      TabIndex        =   1
      Top             =   120
      Width           =   6975
   End
   Begin VB.CommandButton cmdGetFiles 
      Caption         =   "All Files"
      Height          =   375
      Left            =   3120
      TabIndex        =   0
      Top             =   2100
      Width           =   1215
   End
   Begin VB.Label lblComment 
      Alignment       =   1  'Right Justify
      ForeColor       =   &H00FF0000&
      Height          =   495
      Left            =   240
      TabIndex        =   7
      Top             =   2700
      Width           =   5595
   End
End
Attribute VB_Name = "frmLogFiles"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
'ULLOG01.VBP================================================================

' File:                         ULLOG01.VBP

' Library Call Demonstrated:    cbLogGetFileName()

' Purpose:                      Lists data logger files.

' Demonstration:                Displays MCC data files found in the
'                               specified directory.

' Other Library Calls:          cbErrHandling()

' Special Requirements:         There must be an MCC data file in
'                               the indicated parent directory.

' (c) Copyright 2005-2011, Measurement Computing Corp.
' All rights reserved.
'==========================================================================
Option Explicit

Private Const MAX_PATH As Integer = 260
Private Const m_Path As String = "..\.."
Private ULStat As Long

Private Sub Form_Load()

   ' Initiate error handling
   '  activating error handling will trap errors like
   '  bad channel numbers and non-configured conditions.
   '  Parameters:
   '    DONTPRINT   :all warnings and errors encountered will be handled locally
   '    DONTSTOP    :if an error is encountered, the program will not stop,
   '                  errors must be handled locally
  
   ULStat = cbErrHandling(DONTPRINT, DONTSTOP)
   If ULStat <> 0 Then Stop

End Sub

Private Sub cmdFirst_Click()

   Dim Filename As String
   Dim FileNumber As Long
   
   'Initialize a string large enough to hold
   'the file name returned by cbLogGetFileName()
   Filename = String(MAX_PATH, " ")

   lstFiles.Clear
   lblComment.Caption = "Get first file from directory " & m_Path

   '  Get the next file in the directory
   '   Parameters:
   '     GETFIRST :next file
   '     m_Path   :path to search
   '     filename :receives name of file

   FileNumber = GETFIRST
   ULStat = cbLogGetFileName(FileNumber, m_Path, Filename)

   If ULStat <> 0 Then
      lblComment.Caption = "Error " & Format(ULStat, "0") & _
      " - " & ErrorText(ULStat) & "."
   Else
      ListFiles Filename
   End If
   
End Sub

Private Sub cmdNext_Click()

   Dim Filename As String
   Dim FileNumber As Long
   
   'Initialize a string large enough to hold
   'the file name returned by cbLogGetFileName()
   Filename = String(MAX_PATH, " ")

   lstFiles.Clear
   lblComment.Caption = "Get next file from directory " & m_Path

   '  Get the next file in the directory
   '   Parameters:
   '     GETNEXT  :next file
   '     m_Path   :path to search
   '     filename :receives name of file

   FileNumber = GETNEXT
   ULStat = cbLogGetFileName(FileNumber, m_Path, Filename)

   If ULStat <> 0 Then
      lblComment.Caption = "Error " & Format(ULStat, "0") & _
      " - " & ErrorText(ULStat) & "."
   Else
      ListFiles Filename
   End If

End Sub

Private Sub cmdGetFiles_Click()

   Dim Filename As String
   Dim FileNumber As Long
   
   'Initialize a string large enough to hold
   'the file name returned by cbLogGetFileName()
   Filename = String(MAX_PATH, " ")
   
   lstFiles.Clear
   lblComment.Caption = "Get all files from directory " + m_Path
   
   FileNumber = GETFIRST
   ULStat = cbLogGetFileName(FileNumber, m_Path, Filename)
   If ULStat <> 0 Then
      lblComment.Caption = "Error " & Format(ULStat, "0") & _
      " - " & ErrorText(ULStat) & "."
   Else
      ListFiles Filename
      FileNumber = GETNEXT
      While ULStat <> NOMOREFILES
         ULStat = cbLogGetFileName(FileNumber, m_Path, Filename)
         If (ULStat <> 0) And (ULStat <> NOMOREFILES) Then
            lblComment.Caption = "Error " & Format(ULStat, "0") & _
            " - " & ErrorText(ULStat) & "."
         End If
         If ULStat <> NOMOREFILES Then ListFiles Filename
      Wend
   End If

End Sub

Private Sub cmdFileNum_Click()

   Dim Filename As String
   Dim FileNumber As Long
   
   'Initialize a string large enough to hold
   'the file name returned by cbLogGetFileName()
   Filename = String(MAX_PATH, " ")

   lstFiles.Clear
   FileNumber = Val(txtFileNum.Text)
   lblComment.Caption = "Get file number " & _
      Format(FileNumber, "0") & " from directory " & m_Path

   '  Get the next file in the directory
   '   Parameters:
   '     FileNumber  :next file
   '     m_Path      :path to search
   '     filename    :receives name of file

   ULStat = cbLogGetFileName(FileNumber, m_Path, Filename)

   If ULStat <> 0 Then
      lblComment.Caption = "Error " & Format(ULStat, "0") & _
      " - " & ErrorText(ULStat) & "."
   Else
      ListFiles Filename
   End If

End Sub

Private Sub ListFiles(ByVal Filename As String)

   Dim NullLocation As Long
   
   'Filename is returned with a null terminator
   'which must be removed for proper display
   Filename = Trim(Filename)
   NullLocation& = InStr(1, Filename, Chr(0))
   Filename = Left(Filename, NullLocation& - 1)
   lstFiles.AddItem (Filename)
   
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

Private Sub cmdQuit_Click()

   End
   
End Sub


