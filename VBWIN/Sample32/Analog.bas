Attribute VB_Name = "AnalogIO"
Global Const ANALOGINPUT As Long = 1
Global Const ANALOGOUTPUT As Long = 2
Global Const PRETRIGIN As Long = 9
Global Const ATRIGIN As Long = 17
Global ATrigRes As Long
Global ATrigRange As Single
Global AIOGeneralError As Boolean

Private ReportError As Long
Private HandleError As Long
Private TestBoard As Long
Private ADRes As Long
Private ValidRanges() As Long

Function FindAnalogChansOfType(ByVal BoardNum As Long, ByVal AnalogType As Long, _
ByRef Resolution As Long, ByRef DefaultRange As Long, ByRef DefaultChan As Long, _
ByRef DefaultTrig As Long) As Long

   Dim ChansFound, IOType As Integer
   Dim ULStat As Long
   Dim CheckOutputEvents, CheckInputEvents As Boolean
   Dim TestRange As Long
   Dim RangeFound As Boolean
   
   'check supported features by trial
   'and error with error handling disabled
   ULStat = cbErrHandling(DONTPRINT, DONTSTOP)
   
   TestBoard = BoardNum
   DefaultChan = 0
   DefaultRange = NOTUSED
   ATrigRes = 0
   ConnectionConflict$ = "This network device is in use by another process or user." & _
      vbCrLf & vbCrLf & "Check for other users on the network and close any applications " & _
      vbCrLf & "(such as Instacal) that may be accessing the network device."

   IOType = AnalogType And 3
   Select Case IOType
      Case ANALOGINPUT
         ' Get the number of analog input channels
         ULStat = cbGetConfig(BOARDINFO, TestBoard, 0, BINUMADCHANS, ChansFound)
         If ULStat <> NOERRORS Then
            DisplayError ULStat
            FindAnalogChansOfType = 0
            Exit Function
         End If
         If ChansFound > 0 Then
            ' Get the resolution of A/D
            ULStat = cbGetConfig(BOARDINFO, TestBoard, 0, BIADRES, ADRes)
            If ULStat = 0 Then Resolution = ADRes
            If (AnalogType And &HF00&) > 0 Then CheckInputEvents = True
            'check ranges for a valid default
            RangeFound = TestInputRanges(TestRange)
            If RangeFound Then DefaultRange = TestRange
         End If
      Case ANALOGOUTPUT
         ' Get the number of analog output channels
         ULStat = cbGetConfig(BOARDINFO, TestBoard, 0, BINUMDACHANS, ChansFound)
         If ULStat <> NOERRORS Then
            DisplayError ULStat
            ChansFound = 0
            Exit Function
         End If
         If ChansFound > 0 Then
            ULStat = cbGetConfig(BOARDINFO, TestBoard, 0, BIDACRES, DARes)
            Resolution = DARes
            If (AnalogType And &HF00&) > 0 Then CheckOutputEvents = True
            RangeFound = TestOutputRanges(TestRange)
            If RangeFound Then DefaultRange = TestRange
         End If
   End Select
   
   If (ChansFound > 0) And (CheckInputEvents Or CheckOutputEvents) Then
      'check supported features by trial
      'and error with error handling disabled
      Dim EventsSupported As Boolean
      'check support of event handling by trial and error
      If (AnalogType And ERREVENT) > 0 Then
         ULStat = cbDisableEvent(TestBoard, ON_SCAN_ERROR)
         EventsSupported = (ULStat = 0)
      End If
      If CheckInputEvents And EventsSupported Then
         If (AnalogType And DATAEVENT) > 0 Then
            ULStat = cbDisableEvent(TestBoard, ON_DATA_AVAILABLE)
            EventsSupported = (ULStat = 0)
         End If
         If EventsSupported And (AnalogType And ENDEVENT) > 0 Then
            ULStat = cbDisableEvent(TestBoard, ON_END_OF_AI_SCAN)
            EventsSupported = (ULStat = 0)
         End If
         If EventsSupported And (AnalogType And PRETRIGEVENT) > 0 Then
            ULStat = cbDisableEvent(TestBoard, ON_PRETRIGGER)
            EventsSupported = (ULStat = 0)
         End If
      End If
      If EventsSupported And CheckOutputEvents Then
         If (AnalogType And ENDEVENT) > 0 Then
             ULStat = cbDisableEvent(TestBoard, ON_END_OF_AO_SCAN)
             EventsSupported = (ULStat = 0)
         End If
      End If
      If Not EventsSupported Then ChansFound = 0
   End If
   
   CheckATrig = ((AnalogType And ATRIGIN) = ATRIGIN)
   If (ChansFound > 0) And CheckATrig Then
      ULStat = cbSetTrigger(TestBoard, TRIGABOVE, 0, 0)
      If ULStat = 0 Then
         DefaultTrig = TRIGABOVE
         GetTrigResolution
      Else
         ChansFound = 0
      End If
   End If
   
   CheckPretrig = ((AnalogType And PRETRIGIN) = PRETRIGIN)
   If (ChansFound > 0) And CheckPretrig Then
      ' if DaqSetTrigger supported, trigger type is analog
      ULStat = cbDaqSetTrigger(TestBoard, TRIG_IMMEDIATE, _
      ABOVE_LEVEL, 0, ANALOG, DefaultRange, 0!, 0.1!, START_EVENT)
      If ULStat = 0 Then
         DefaultTrig = TRIGABOVE
      Else
         ULStat = cbSetTrigger(TestBoard, TRIGPOSEDGE, 0, 0)
         If ULStat = 0 Then
            DefaultTrig = TRIGPOSEDGE
         Else
            ChansFound = 0
         End If
      End If
   End If
   
   ULStat = cbErrHandling(ReportError, HandleError)

   FindAnalogChansOfType = ChansFound

End Function

Private Function TestInputRanges(ByRef DefaultRange As Long) As Boolean

   Dim DataValue As Integer
   Dim dataHRValue As Long, AIOption As Long
   Dim ULStat As Long
   Dim TestRange As Long
   Dim Index As Integer
   ConnectionConflict$ = "This network device is in use by another process." & _
      vbCrLf & vbCrLf & "Check for other users on the network and close any applications " & _
      vbCrLf & "(such as Instacal) that may be accessing the network device."
   
   TestInputRanges = False
   DefaultRange = NOTUSED
   ReDim ValidRanges(0)
   For TestRange = BIP5VOLTS To BIP30VOLTS
      If ADRes > 16 Then
         ULStat = cbAIn32(TestBoard, 0, TestRange, dataHRValue, AIOption)
      Else
         ULStat = cbAIn(TestBoard, 0, TestRange, DataValue)
      End If
      If ULStat = 0 Then
         If DefaultRange = NOTUSED Then DefaultRange = TestRange
         TestInputRanges = True
         ReDim Preserve ValidRanges(Index)
         ValidRanges(Index) = TestRange
         Index = Index + 1
      Else
         If (ULStat& = NETDEVINUSEBYANOTHERPROC) Or (ULStat& = NETDEVINUSE) Then
            MsgBox ConnectionConflict$, vbCritical, "Device In Use"
            ReDim Preserve ValidRanges(Index)
            ValidRanges(Index) = NOTUSED
            Exit Function
         End If
      End If
   Next
   
   For TestRange = UNI10VOLTS To UNI4VOLTS
      ULStat = cbAIn(TestBoard, 0, TestRange, DataValue)
      If ULStat = 0 Then
         If DefaultRange = NOTUSED Then DefaultRange = TestRange
         TestInputRanges = True
         ReDim Preserve ValidRanges(Index)
         ValidRanges(Index) = TestRange
         Index = Index + 1
      Else
         If (ULStat& = NETDEVINUSEBYANOTHERPROC) Or (ULStat& = NETDEVINUSE) Then
            MsgBox ConnectionConflict$, vbCritical, "Device In Use"
            ReDim Preserve ValidRanges(Index)
            ValidRanges(Index) = NOTUSED
            Exit Function
         End If
      End If
   Next
   
   For TestRange = MA4TO20 To BIPPT025AMPS
      ULStat = cbAIn(TestBoard, 0, TestRange, DataValue)
      If ULStat = 0 Then
         If DefaultRange = NOTUSED Then DefaultRange = TestRange
         TestInputRanges = True
         ReDim Preserve ValidRanges(Index)
         ValidRanges(Index) = TestRange
         Index = Index + 1
      End If
   Next

End Function

Private Function TestOutputRanges(ByRef DefaultRange As Long) As Boolean

   Dim DataValue As Integer
   Dim ULStat As Long
   Dim TestRange As Long
   ConnectionConflict$ = "This network device is in use by another process or user." & _
      vbCrLf & vbCrLf & "Check for other users on the network and close any applications " & _
      vbCrLf & "(such as Instacal) that may be accessing the network device."
   
   TestOutputRanges = False
   DefaultRange = NOTUSED
   
   TestRange = -5
   ULStat = cbAOut(TestBoard, 0, TestRange, DataValue)
   If (ULStat& = NETDEVINUSEBYANOTHERPROC) Or (ULStat& = NETDEVINUSE) Then
      MsgBox ConnectionConflict$, vbCritical, "Device In Use"
      Exit Function
   End If
   If ULStat = 0 Then
      ULStat = cbGetConfig(BOARDINFO, TestBoard, 0, 114, TestRange)
      If ULStat = 0 Then
         DefaultRange = TestRange
         TestOutputRanges = True
      End If
   Else
      For TestRange = BIP5VOLTS To BIP30VOLTS
         ULStat = cbAOut(TestBoard, 0, TestRange, DataValue)
         If ULStat = 0 Then
            If DefaultRange = NOTUSED Then DefaultRange = TestRange
            TestOutputRanges = True
            Exit For
         End If
      Next
   End If
   
   If DefaultRange = NOTUSED Then
      For TestRange = UNI10VOLTS To UNI4VOLTS
         ULStat = cbAOut(TestBoard, 0, TestRange, DataValue)
         If ULStat = 0 Then
            If DefaultRange = NOTUSED Then DefaultRange = TestRange
            TestOutputRanges = True
            Exit For
         End If
      Next
   End If
   
   If DefaultRange = NOTUSED Then
      For TestRange = MA4TO20 To BIPPT025AMPS
         ULStat = cbAOut(TestBoard, 0, TestRange, DataValue)
         If ULStat = 0 Then
            If DefaultRange = NOTUSED Then DefaultRange = TestRange
            TestOutputRanges = True
            Exit For
         End If
      Next
   End If
   
End Function

Public Function GetRangeList() As Variant

   Dim DefaultRange As Long
   Dim ULStat As Long

   'check supported ranges by trial
   'and error with error handling disabled
   ULStat = cbErrHandling(DONTPRINT, DONTSTOP)

   TestInputRanges (DefaultRange)
   GetRangeList = ValidRanges()

   ULStat = cbErrHandling(ReportError, HandleError)

End Function

Function GetRangeString(ByVal Range As Long) As String
   
   GetRangeInfo Range, RangeString$, RangeVolts!
   GetRangeString = RangeString$
   
End Function

Function GetRangeVolts(ByVal Range As Long) As Single
   
   GetRangeInfo Range, RangeString$, RangeVolts!
   GetRangeVolts = RangeVolts!
   
End Function

Private Sub GetRangeInfo(ByVal Range As Long, RangeString As String, RangeVolts As Single)
   
   Select Case Range
      Case NOTUSED
         RangeString = "NOTUSED"
         RangeVolts = 0
      Case BIP5VOLTS
         RangeString = "BIP5VOLTS"
         RangeVolts = 10
      Case BIP10VOLTS
         RangeString = "BIP10VOLTS"
         RangeVolts = 20
      Case BIP2PT5VOLTS
         RangeString = "BIP2PT5VOLTS"
         RangeVolts = 5
      Case BIP1PT25VOLTS
         RangeString = "BIP1PT25VOLTS"
         RangeVolts = 2.5
      Case BIP1VOLTS
         RangeString = "BIP1VOLTS"
         RangeVolts = 2
      Case BIPPT625VOLTS
         RangeString = "BIPPT625VOLTS"
         RangeVolts = 1.25
      Case BIPPT5VOLTS
         RangeString = "BIPPT5VOLTS"
         RangeVolts = 1
      Case BIPPT1VOLTS
         RangeString = "BIPPT1VOLTS"
         RangeVolts = 0.2
      Case BIPPT05VOLTS
         RangeString = "BIPPT05VOLTS"
         RangeVolts = 0.1
      Case BIPPT01VOLTS
         RangeString = "BIPPT01VOLTS"
         RangeVolts = 0.02
      Case BIPPT005VOLTS
         RangeString = "BIPPT005VOLTS"
         RangeVolts = 0.01
      Case BIP1PT67VOLTS
         RangeString = "BIP1PT67VOLTS"
         RangeVolts = 3.34
      Case BIPPT312VOLTS
         RangeString = "BIPPT312VOLTS"
         RangeVolts = 0.625
      Case BIPPT156VOLTS
         RangeString = "BIPPT156VOLTS"
         RangeVolts = 0.3125
      Case BIPPT078VOLTS
         RangeString = "BIPPT078VOLTS"
         RangeVolts = 0.15625
      Case BIP60VOLTS
         RangeString = "BIP60VOLTS"
         RangeVolts = 120
      Case BIP15VOLTS
         RangeString = "BIP15VOLTS"
         RangeVolts = 30
      Case BIPPT125VOLTS
         RangeString = "BIPPT125VOLTS"
         RangeVolts = 0.25
      Case BIPPT25VOLTS
         RangeString = "BIPPT25VOLTS"
         RangeVolts = 0.5
      Case BIPPT2VOLTS
         RangeString = "BIPPT2VOLTS"
         RangeVolts = 0.4
      Case BIP2VOLTS
         RangeString = "BIP2VOLTS"
         RangeVolts = 4
      Case BIP20VOLTS
         RangeString = "BIP20VOLTS"
         RangeVolts = 40
      Case BIP4VOLTS
         RangeString = "BIP4VOLTS"
         RangeVolts = 8
      Case BIP30VOLTS
         RangeString = "BIP30VOLTS"
         RangeVolts = 60
      Case BIPPT025VOLTSPERVOLT
         RangeString = "BIPPT025VOLTSPERVOLT"
         RangeVolts = 0.05
      Case BIPPT073125VOLTS
         RangeString = "BIPPT073125VOLTS"
         RangeVolts = 0.14625
      Case UNI10VOLTS
         RangeString = "UNI10VOLTS"
         RangeVolts = 10
      Case UNI5VOLTS
         RangeString = "UNI5VOLTS"
         RangeVolts = 5
      Case UNI2PT5VOLTS
         RangeString = "UNI2PT5VOLTS"
         RangeVolts = 2.5
      Case UNI2VOLTS
         RangeString = "UNI2VOLTS"
         RangeVolts = 2
      Case UNI1PT25VOLTS
         RangeString = "UNI1PT25VOLTS"
         RangeVolts = 1.25
      Case UNI1VOLTS
         RangeString = "UNI1VOLTS"
         RangeVolts = 1
      Case UNIPT1VOLTS
         RangeString = "UNIPT1VOLTS"
         RangeVolts = 0.1
      Case UNIPT01VOLTS
         RangeString = "UNIPT01VOLTS"
         RangeVolts = 0.01
      Case UNIPT02VOLTS
         RangeString = "UNIPT02VOLTS"
         RangeVolts = 0.02
      Case UNI1PT67VOLTS
         RangeString = "UNI1PT67VOLTS"
         RangeVolts = 1.67
      Case UNIPT5VOLTS
         RangeString = "UNIPT5VOLTS"
         RangeVolts = 0.5
      Case UNIPT25VOLTS
         RangeString = "UNIPT25VOLTS"
         RangeVolts = 0.25
      Case UNIPT2VOLTS
         RangeString = "UNIPT2VOLTS"
         RangeVolts = 0.2
      Case UNIPT05VOLTS
         RangeString = "UNIPT05VOLTS"
         RangeVolts = 0.05
      Case UNI4VOLTS
         RangeString = "UNI4VOLTS"
         RangeVolts = 4.096
      Case MA4TO20
         RangeString = "MA4TO20"
         RangeVolts = 16
      Case MA2to10
         RangeString = "MA2to10"
         RangeVolts = 8
      Case MA1TO5
         RangeString = "MA1TO5"
         RangeVolts = 4
      Case MAPT5TO2PT5
         RangeString = "MAPT5TO2PT5"
         RangeVolts = 2
      Case MA0TO20
         RangeString = "MA0TO20"
         RangeVolts = 20
      Case BIPPT025AMPS
         RangeString = "BIPPT025AMPS"
         RangeVolts = 0.05
      Case BIPPT025VOLTSPERVOLT
         RangeString$ = "BIPPT025VOLTSPERVOLT"
         RangeVolts = 0.05
   End Select

End Sub

Function GetPortString(ByVal PortNum As Long) As String

   Select Case PortNum
      Case AUXPORT
         Reply$ = "AUXPORT"
      Case FIRSTPORTA
         Reply$ = "FIRSTPORTA"
      Case FIRSTPORTB
         Reply$ = "FIRSTPORTB"
      Case FIRSTPORTCL
         Reply$ = "FIRSTPORTCL"
      Case FIRSTPORTC
         Reply$ = "FIRSTPORTC"
      Case FIRSTPORTCH
         Reply$ = "FIRSTPORTCH"
      Case SECONDPORTA
         Reply$ = "SECONDPORTA"
      Case SECONDPORTB
         Reply$ = "SECONDPORTB"
      Case SECONDPORTCL
         Reply$ = "SECONDPORTCL"
      Case SECONDPORTCH
         Reply$ = "SECONDPORTCH"
      Case THIRDPORTA
         Reply$ = "THIRDPORTA"
      Case THIRDPORTB
         Reply$ = "THIRDPORTB"
      Case THIRDPORTCL
         Reply$ = "THIRDPORTCL"
      Case THIRDPORTCH
         Reply$ = "THIRDPORTCH"
      Case FOURTHPORTA
         Reply$ = "FOURTHPORTA"
      Case FOURTHPORTB
         Reply$ = "FOURTHPORTB"
      Case FOURTHPORTCL
         Reply$ = "FOURTHPORTCL"
      Case FOURTHPORTCH
         Reply$ = "FOURTHPORTCH"
      Case FIFTHPORTA
         Reply$ = "FIFTHPORTA"
      Case FIFTHPORTB
         Reply$ = "FIFTHPORTB"
      Case FIFTHPORTCL
         Reply$ = "FIFTHPORTCL"
      Case FIFTHPORTCH
         Reply$ = "FIFTHPORTCH"
      Case SIXTHPORTA
         Reply$ = "SIXTHPORTA"
      Case SIXTHPORTB
         Reply$ = "SIXTHPORTB"
      Case SIXTHPORTCL
         Reply$ = "SIXTHPORTCL"
      Case SIXTHPORTCH
         Reply$ = "SIXTHPORTCH"
      Case SEVENTHPORTA
         Reply$ = "SEVENTHPORTA"
      Case SEVENTHPORTB
         Reply$ = "SEVENTHPORTB"
      Case SEVENTHPORTCL
         Reply$ = "SEVENTHPORTCL"
      Case SEVENTHPORTCH
         Reply$ = "SEVENTHPORTCH"
      Case EIGHTHPORTA
         Reply$ = "EIGHTHPORTA"
      Case EIGHTHPORTB
         Reply$ = "EIGHTHPORTB"
      Case EIGHTHPORTCL
         Reply$ = "EIGHTHPORTCL"
      Case EIGHTHPORTCH
         Reply$ = "EIGHTHPORTCH"
      Case Else
         Reply$ = "INVALIDPORT"
   End Select
   GetPortString = Reply$
   
End Function

Private Sub GetTrigResolution()

   Dim ULStat As Long, BoardID As Long, TrigSource As Long

   ULStat = cbGetConfig(BOARDINFO, TestBoard, 0, 209, TrigSource)
   
   ULStat = cbGetConfig(BOARDINFO, TestBoard, 0, BIBOARDTYPE, BoardID)
   Select Case BoardID
      Case 95, 96, 97, 98, 102
         'PCI-DAS6030, 6031, 6032, 6033, 6052
         ATrigRes = 12
         ATrigRange = 20
         If TrigSource > 0 Then ATrigRange = -1
      Case 165, 166, 167, 168
          'PCI-2511, 2513, 2515, 2517
          ATrigRes = 12
          ATrigRange = 20
          If TrigSource > 0 Then ATrigRange = -1
      Case 177, 178, 179, 180
          'USB-2523, 2527, 2533, 2537
          ATrigRes = 12
          ATrigRange = 20
          If TrigSource > 0 Then ATrigRange = -1
      Case 203, 204, 205, 213, 214, 215, 216, 217
          'USB-1616HS, 1616HS-2, 1616HS-4, 1616HS-BNC
          'USB-1602HS, 1602HS-2AO, 1604HS, 1604HS-2AO
          ATrigRes = 12
          ATrigRange = 20
          If TrigSource > 0 Then ATrigRange = -1
      Case 101, 103, 104
         'PCI-DAS6040, 6070, 6071
         ATrigRes = 8
         ATrigRange = 20
         If TrigSource > 0 Then ATrigRange = -1
      Case Else
         ATrigRes = 0
   End Select
   
End Sub

Public Sub SetAnalogIODefaults(ByVal ReportErr As Long, ByVal HandleErr As Long)

   HandleError = HandleErr
   ReportError = ReportErr
   
End Sub

Private Sub DisplayError(ByVal ErrorNumber As Long)

   ErrMessage$ = Space$(ERRSTRLEN)     ' fill ErrMessage$ with spaces

   ULStat = cbGetErrMsg(ErrorNumber, ErrMessage$)
  
   'ErrMessage$ string is returned with a null terminator.
   'This should be removed to display properly.
   NullLocation = InStr(1, ErrMessage$, Chr(0))
   ErrMessage$ = Left(ErrMessage$, NullLocation - 1)

   MsgBox "Cannot run example program - error " & _
   ErrorNumber & " occurred. (" & ErrMessage$ & ")", _
   vbCritical, "Unexpected Library Error"
   AIOGeneralError = True
   
End Sub

