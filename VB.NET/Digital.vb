Module DigitalIO

    Public Const PORTOUT As Integer = 1
    Public Const PORTIN As Integer = 2
    Public Const PORTOUTSCAN As Integer = 5
    Public Const PORTINSCAN As Integer = 10
    Public Const BITOUT As Integer = 17
    Public Const BITIN As Integer = 34
    Public Const FIXEDPORT As Integer = 0
    Public Const PROGPORT As Integer = 1
    Public Const PROGBIT As Integer = 2

    Public Function FindPortsOfType(ByVal DaqBoard As MccDaq.MccBoard, _
        ByVal PortType As Integer, ByRef ProgAbility As Integer, ByRef DefaultPort As _
        MccDaq.DigitalPortType, ByRef DefaultNumBits As Integer, ByRef FirstBit As Integer) As Integer

        Dim ThisType, NumPorts, NumBits As Integer
        Dim DefaultDev, InMask, OutMask As Integer
        Dim PortsFound, curCount, curIndex As Integer
        Dim status As Short
        Dim PortIsCompatible, CheckBitProg As Boolean
        Dim CurPort As MccDaq.DigitalPortType
        Dim DFunction As MccDaq.FunctionType
        Dim ULStat As MccDaq.ErrorInfo
        Dim ConnectionConflict As String

        ULStat = MccDaq.MccService.ErrHandling _
            (MccDaq.ErrorReporting.DontPrint, MccDaq.ErrorHandling.DontStop)

        ULStat = DaqBoard.BoardConfig.GetDiNumDevs(NumPorts)
        If Not ULStat.Value = MccDaq.ErrorInfo.ErrorCode.NoErrors Then
            DisplayError(ULStat)
            FindPortsOfType = PortsFound
            Exit Function
        End If
        DefaultPort = -1
        FirstBit = 0
        ConnectionConflict = "This network device is in use by another process or user." & _
           vbCrLf & vbCrLf & "Check for other users on the network and close any applications " & _
           vbCrLf & "(such as Instacal) that may be accessing the network device."

        If (PortType = BITOUT) Or (PortType = BITIN) Then CheckBitProg = True
        If (PortType = PORTOUTSCAN) Or (PortType = PORTINSCAN) Then
            If NumPorts > 0 Then
                'check scan capability by trial and error with error handling disabled
                DFunction = MccDaq.FunctionType.DiFunction
                If (PortType = PORTOUTSCAN) Then DFunction = MccDaq.FunctionType.DoFunction
                ULStat = DaqBoard.GetStatus(status, curCount, curIndex, DFunction)
                If Not ULStat.Value = MccDaq.ErrorInfo.ErrorCode.NoErrors Then NumPorts = 0
            End If
            PortType = PortType And (PORTOUT Or PORTIN)
        End If
        For DioDev As Long = 0 To NumPorts - 1
            ProgAbility = -1
            ULStat = DaqBoard.DioConfig.GetDInMask(DioDev, InMask)
            ULStat = DaqBoard.DioConfig.GetDOutMask(DioDev, OutMask)
            If (InMask And OutMask) > 0 Then ProgAbility = FIXEDPORT
            ULStat = DaqBoard.DioConfig.GetDevType(DioDev, ThisType)
            If ULStat.Value = MccDaq.ErrorInfo.ErrorCode.NoErrors Then
                CurPort = [Enum].Parse(GetType(MccDaq.DigitalPortType), ThisType)
            End If
            If (DioDev = 0) And (CurPort = MccDaq.DigitalPortType.FirstPortCL) Then
                'a few devices (USB-SSR08 for example)
                'start at FIRSTPORTCL and number the bits
                'as if FIRSTPORTA and FIRSTPORTB exist for 
                'compatibiliry with older digital peripherals 
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
                    Dim ConfigDirection As MccDaq.DigitalPortDirection
                    ConfigDirection = MccDaq.DigitalPortDirection.DigitalOut
                    If PortType = PORTIN Then ConfigDirection = _
                        MccDaq.DigitalPortDirection.DigitalIn
                    If (CurPort = MccDaq.DigitalPortType.AuxPort) And CheckBitProg Then
                        'if it's an AuxPort, check bit programmability
                        ULStat = DaqBoard.DConfigBit _
                            (MccDaq.DigitalPortType.AuxPort, FirstBit, ConfigDirection)
                        If ULStat.Value = MccDaq.ErrorInfo.ErrorCode.NoErrors Then
                            ProgAbility = PROGBIT
                            ULStat = DaqBoard.DConfigBit(MccDaq.DigitalPortType.AuxPort, _
                                FirstBit, MccDaq.DigitalPortDirection.DigitalIn)
                        Else
                            If ULStat.Value = MccDaq.ErrorInfo.ErrorCode.NetDevInUseByAnotherProc _
                                Or ULStat.Value = MccDaq.ErrorInfo.ErrorCode.NetDevInUse Then
                                MsgBox(ConnectionConflict$, vbCritical, "Device In Use")
                                Exit Function
                            End If
                        End If
                    End If
                    If ProgAbility = -1 Then
                        'check port programmability
                        ULStat = DaqBoard.DConfigPort(CurPort, ConfigDirection)
                        If ULStat.Value = MccDaq.ErrorInfo.ErrorCode.NoErrors Then
                            'return port to input mode
                            ProgAbility = PROGPORT
                            ULStat = DaqBoard.DConfigPort(CurPort, _
                                MccDaq.DigitalPortDirection.DigitalIn)
                        Else
                            If ULStat.Value = MccDaq.ErrorInfo.ErrorCode.NetDevInUseByAnotherProc _
                                Or ULStat.Value = MccDaq.ErrorInfo.ErrorCode.NetDevInUse Then
                                MsgBox(ConnectionConflict$, vbCritical, "Device In Use")
                                Exit Function
                            End If
                        End If
                    End If
                End If
                PortIsCompatible = Not (ProgAbility = -1)
            End If
            If PortIsCompatible Then
                PortsFound = PortsFound + 1
                Dim BitVals, BitWeight As Integer
                Dim TotalVal, CurBit As Integer
                If DefaultPort = -1 Then
                    ULStat = DaqBoard.DioConfig.GetNumBits(DioDev, NumBits)
                    If ProgAbility = FIXEDPORT Then
                        'could have different number of input and output bits
                        BitVals = OutMask
                        If PortType = PORTIN Then BitVals = InMask
                        Do
                            BitWeight = Math.Pow(2, CurBit)
                            TotalVal = BitWeight + TotalVal
                            CurBit = CurBit + 1
                        Loop While TotalVal < BitVals
                        NumBits = CurBit
                    End If
                    DefaultNumBits = NumBits
                    DefaultDev = DioDev
                    DefaultPort = CurPort
                End If
            End If
            If ProgAbility = PROGBIT Then Exit For
        Next
        ULStat = MccDaq.MccService.ErrHandling(ReportError, HandleError)

        FindPortsOfType = PortsFound

    End Function

End Module
