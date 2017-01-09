Module EventSupport

    Public Const DATAEVENT As Integer = 1
    Public Const ENDEVENT As Integer = 2
    Public Const PRETRIGEVENT As Integer = 4
    Public Const ERREVENT As Integer = 8
    Public Const ENDOUTEVENT As Integer = 16
    Public Const DCHANGEEVENT As Integer = 32
    Public Const INTEVENT As Integer = 64

    Private TestBoard As MccDaq.MccBoard

    Public Function FindEventsOfType(ByVal DaqBoard As MccDaq.MccBoard, _
    ByVal EventType As Integer) As Integer

        Dim ULStat As MccDaq.ErrorInfo
        Dim EventsFound As Integer

        'check supported features by trial 
        'and error with error handling disabled
        ULStat = MccDaq.MccService.ErrHandling _
        (MccDaq.ErrorReporting.DontPrint, MccDaq.ErrorHandling.DontStop)

        TestBoard = DaqBoard
        ULStat = New MccDaq.ErrorInfo(0)

        'check support of event handling by trial
        'and error with error handling disabled
        If (EventType And DCHANGEEVENT) > 0 Then
            ULStat = DaqBoard.DisableEvent(MccDaq.EventType.OnChangeOfDigInput)
            If (ULStat.Value = MccDaq.ErrorInfo.ErrorCode.NoErrors) _
                Then EventsFound = (EventsFound Or DCHANGEEVENT)
        End If
        If (EventType And INTEVENT) > 0 Then
            ULStat = DaqBoard.DisableEvent(MccDaq.EventType.OnExternalInterrupt)
            If (ULStat.Value = MccDaq.ErrorInfo.ErrorCode.NoErrors) _
                Then EventsFound = (EventsFound Or INTEVENT)
        End If
        If (EventType And ERREVENT) > 0 Then
            ULStat = DaqBoard.DisableEvent(MccDaq.EventType.OnScanError)
            If (ULStat.Value = MccDaq.ErrorInfo.ErrorCode.NoErrors) _
                Then EventsFound = (EventsFound Or ERREVENT)
        End If
        If (EventType And DATAEVENT) > 0 Then
            ULStat = DaqBoard.DisableEvent(MccDaq.EventType.OnDataAvailable)
            If (ULStat.Value = MccDaq.ErrorInfo.ErrorCode.NoErrors) _
                Then EventsFound = (EventsFound Or DATAEVENT)
        End If
        If (EventType And ENDEVENT) > 0 Then
            ULStat = DaqBoard.DisableEvent(MccDaq.EventType.OnEndOfAiScan)
            If (ULStat.Value = MccDaq.ErrorInfo.ErrorCode.NoErrors) _
                Then EventsFound = (EventsFound Or ENDEVENT)
        End If
        If (EventType And PRETRIGEVENT) > 0 Then
            ULStat = DaqBoard.DisableEvent(MccDaq.EventType.OnPretrigger)
            If (ULStat.Value = MccDaq.ErrorInfo.ErrorCode.NoErrors) _
                Then EventsFound = (EventsFound Or PRETRIGEVENT)
        End If
        If (EventType And ENDOUTEVENT) > 0 Then
            ULStat = DaqBoard.DisableEvent(MccDaq.EventType.OnEndOfAoScan)
            If (ULStat.Value = MccDaq.ErrorInfo.ErrorCode.NoErrors) _
                Then EventsFound = (EventsFound Or PRETRIGEVENT)
        End If

        ULStat = MccDaq.MccService.ErrHandling(ReportError, HandleError)
        FindEventsOfType = EventsFound

    End Function

End Module
