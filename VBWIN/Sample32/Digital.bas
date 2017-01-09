Attribute VB_Name = "DigitalIO"
Global Const PORTOUT As Long = 1
Global Const PORTIN As Long = 2
Global Const PORTOUTSCAN As Long = 5
Global Const PORTINSCAN As Long = 10
Global Const BITOUT As Long = 17
Global Const BITIN As Long = 34
Global Const FIXEDPORT As Long = 0
Global Const PROGPORT As Long = 1
Global Const PROGBIT As Long = 2
Global DIOGeneralError As Boolean

Private ReportError As Long
Private HandleError As Long

Function FindPortsOfType(ByVal BoardNum As Long, ByVal PortType As Long, _
ByRef ProgAbility As Long, ByRef DefaultPort As Long, _
ByRef DefaultNumBits As Long, ByRef FirstBit As Long) As Long

   Dim ThisType As Long, NumPorts As Long
   Dim PortsFound As Long, NumBits As Long
   Dim DefaultDev As Long, InMask As Long, OutMask As Long
   Dim CurCount As Long, CurIndex As Long, BitVals As Long
   Dim CurPort As Long, DFunction As Long, ULStat As Long
   Dim Status As Integer
   Dim PortIsCompatible As Boolean, CheckBitProg As Boolean
   
   ULStat& = cbErrHandling(DONTPRINT, DONTSTOP)
   If ULStat& <> 0 Then Stop
   
   ULStat& = cbGetConfig(BOARDINFO, BoardNum, 0, BIDINUMDEVS, NumPorts)
   If ULStat <> NOERRORS Then
      DisplayError ULStat
      FindPortsOfType = 0
      Exit Function
   End If
   DefaultPort = -1
   FirstBit = 0
   ConnectionConflict$ = "This network device is in use by another process or user." & _
      vbCrLf & vbCrLf & "Check for other users on the network and close any applications " & _
      vbCrLf & "(such as Instacal) that may be accessing the network device."
   
   If (PortType = BITOUT) Or (PortType = BITIN) Then CheckBitProg = True
   If (PortType = PORTOUTSCAN) Or (PortType = PORTINSCAN) Then
      If NumPorts > 0 Then
         'check scan capability by trial and error with error handling disabled
         DFunction& = DIFUNCTION
         If (PortType = PORTOUTSCAN) Then DFunction& = DOFUNCTION
         ULStat = cbGetStatus(BoardNum, Status, CurCount, CurIndex, DFunction&)
         If Not (ULStat = 0) Then NumPorts = 0
      End If
      PortType = PortType And (PORTOUT Or PORTIN)
   End If
   For DioDev& = 0 To NumPorts - 1
      ProgAbility = -1
      ULStat& = cbGetConfig(DIGITALINFO, BoardNum, DioDev&, DIINMASK, InMask)
      ULStat& = cbGetConfig(DIGITALINFO, BoardNum, DioDev&, DIOUTMASK, OutMask)
      If (InMask And OutMask) > 0 Then ProgAbility = FIXEDPORT
      ULStat& = cbGetConfig(DIGITALINFO, BoardNum, DioDev&, DIDEVTYPE, ThisType)
      If (ULStat = 0) Then CurPort = ThisType
      If (DioDev = 0) And (CurPort = FIRSTPORTCL) Then
         'a few devices (USB-SSR08 for example)
         'start at FIRSTPORTCL and number the bits
         'as if FIRSTPORTA and FIRSTPORTB exist for
         'compatibility with older digital peripherals
         FirstBit = 16
      End If

      'check if port is set for requested direction
      'or can be programmed for requested direction
      PortIsCompatible = False
      Select Case PortType
         Case PORTOUT
            If OutMask > 0 Then PortIsCompatible = True
         Case PORTIN
            If InMask > 0 Then PortIsCompatible = True
      End Select
      PortType = PortType And (PORTOUT Or PORTIN)
      If Not PortIsCompatible Then
         If (ProgAbility <> FIXEDPORT) Then
            'check programmability by trial and error with error handling disabled
            ULStat& = cbErrHandling(DONTPRINT, DONTSTOP)
            If Not (ULStat& = 0) Then Stop
            ConfigDirection = DIGITALOUT
            If PortType = PORTIN Then ConfigDirection = DIGITALIN
            If (CurPort = AUXPORT) And CheckBitProg Then
               'if it's an AuxPort, check bit programmability
               ULStat& = cbDConfigBit(BoardNum, CurPort, FirstBit, ConfigDirection)
               If (ULStat = 0) Then
                  'return bit to input mode
                  ProgAbility = PROGBIT
                  ULStat& = cbDConfigBit(BoardNum, CurPort, FirstBit, DIGITALIN)
               Else
                  If (ULStat& = NETDEVINUSEBYANOTHERPROC) Or (ULStat& = NETDEVINUSE) Then
                     MsgBox ConnectionConflict$, vbCritical, "Device In Use"
                     Exit Function
                  End If
               End If
            End If
            If ProgAbility = -1 Then
               'check port programmability
               ULStat& = cbDConfigPort(BoardNum, CurPort, ConfigDirection)
               If (ULStat = 0) Then
                  ProgAbility = PROGPORT
                  'return port to input mode
                  ULStat& = cbDConfigPort(BoardNum, CurPort, DIGITALIN)
               Else
                  If (ULStat& = NETDEVINUSEBYANOTHERPROC) Or (ULStat& = NETDEVINUSE) Then
                     MsgBox ConnectionConflict$, vbCritical, "Device In Use"
                     Exit Function
                  End If
               End If
            End If
            ULStat& = cbErrHandling(ReportError, HandleError)
         End If
         PortIsCompatible = Not (ProgAbility = -1)
      End If
      If PortIsCompatible Then
         PortsFound = PortsFound + 1
         If DefaultPort = -1 Then
            ULStat& = cbGetConfig(DIGITALINFO, BoardNum, DioDev&, DINUMBITS, NumBits)
            If ProgAbility = FIXEDPORT Then
               'could have different number of input and output bits
               BitVals = OutMask
               If PortType = PORTIN Then BitVals = InMask
               Do
                  BitWeight& = 2 ^ CurBit&
                  TotalVal& = BitWeight& + TotalVal&
                  CurBit& = CurBit& + 1
               Loop While TotalVal& < BitVals&
               NumBits = CurBit&
            End If
            DefaultNumBits = NumBits
            DefaultDev = DioDev
            DefaultPort = CurPort
         End If
      End If
      If ProgAbility = PROGBIT Then Exit For
   Next
   ULStat& = cbErrHandling(ReportError, HandleError)

   FindPortsOfType = PortsFound
   
End Function

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

Public Sub SetDigitalIODefaults(ByVal ReportErr As Long, ByVal HandleErr As Long)

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
   DIOGeneralError = True
   
End Sub


