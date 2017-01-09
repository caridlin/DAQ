Module Counters

    Public Const CTR8254 As Integer = 1
    Public Const CTR9513 As Integer = 2
    Public Const CTR8536 As Integer = 3
    Public Const CTR7266 As Integer = 4
    Public Const CTREVENT As Integer = 5
    Public Const CTRSCAN As Integer = 6
    Public Const CTRTMR As Integer = 7
    Public Const CTRQUAD As Integer = 8
    Public Const CTRPULSE As Integer = 9

    Public Function FindCountersOfType(ByVal DaqBoard As MccDaq.MccBoard, _
    ByVal CounterType As Integer, ByRef DefaultCtr As Integer) As Integer

        Dim ThisType, CounterNum, CtrsFound As Integer
        Dim NumCounters As Integer
        Dim ULStat As MccDaq.ErrorInfo

        'check supported features by trial 
        'and error with error handling disabled
        ULStat = MccDaq.MccService.ErrHandling _
        (MccDaq.ErrorReporting.DontPrint, MccDaq.ErrorHandling.DontStop)

        ULStat = DaqBoard.BoardConfig.GetCiNumDevs(NumCounters)
        If Not ULStat.Value = MccDaq.ErrorInfo.ErrorCode.NoErrors Then
            DisplayError(ULStat)
            FindCountersOfType = CtrsFound
            Exit Function
        End If
        DefaultCtr = -1
        For CtrDev As Long = 0 To NumCounters - 1
            ULStat = DaqBoard.CtrConfig.GetCtrType(CtrDev, ThisType)
            If ThisType = CounterType Then
                ULStat = DaqBoard.CtrConfig.GetCtrNum(CtrDev, CounterNum)
                If ULStat.Value = MccDaq.ErrorInfo.ErrorCode.NoErrors Then
                    CtrsFound = CtrsFound + 1
                    If DefaultCtr = -1 Then DefaultCtr = CounterNum
                End If
            End If
        Next
        FindCountersOfType = CtrsFound

        ULStat = MccDaq.MccService.ErrHandling(ReportError, HandleError)

    End Function

End Module
