Attribute VB_Name = "EventSupport"
Public Const DATAEVENT As Long = 1
Public Const ENDEVENT As Long = 2
Public Const PRETRIGEVENT As Long = 4
Public Const ERREVENT As Long = 8
Public Const ENDOUTEVENT As Integer = 16
Public Const DCHANGEEVENT As Integer = 32
Public Const INTEVENT As Integer = 64

Global EventGeneralError As Boolean

Private ReportError As Long
Private HandleError As Long
Private TestBoard As Long

Function FindEventsOfType(ByVal BoardNum As Long, ByVal EventType As Long) As Long

   Dim EventsFound As Integer
   
   'check supported features by trial
   'and error with error handling disabled
   ULStat = cbErrHandling(DONTPRINT, DONTSTOP)
   
   TestBoard = BoardNum

   'check supported features by trial
   'and error with error handling disabled
   If (EventType And DCHANGEEVENT) > 0 Then
      ULStat = cbDisableEvent(TestBoard, ON_CHANGE_DI)
      If (ULStat = NOERRORS) Then _
         EventsFound = (EventsFound Or DCHANGEEVENT)
   End If
   If (EventType And INTEVENT) > 0 Then
      ULStat = cbDisableEvent(TestBoard, ON_EXTERNAL_INTERRUPT)
      If (ULStat = NOERRORS) Then _
         EventsFound = (EventsFound Or INTEVENT)
   End If
   If (EventType And ERREVENT) > 0 Then
      ULStat = cbDisableEvent(TestBoard, ON_SCAN_ERROR)
      If (ULStat = NOERRORS) Then _
         EventsFound = (EventsFound Or ERREVENT)
   End If
   If (EventType And DATAEVENT) > 0 Then
      ULStat = cbDisableEvent(TestBoard, ON_DATA_AVAILABLE)
      If (ULStat = NOERRORS) Then _
         EventsFound = (EventsFound Or DATAEVENT)
   End If
   If (EventType And ENDEVENT) > 0 Then
      ULStat = cbDisableEvent(TestBoard, ON_END_OF_AI_SCAN)
      If (ULStat = NOERRORS) Then _
         EventsFound = (EventsFound Or ENDEVENT)
   End If
   If (EventType And PRETRIGEVENT) > 0 Then
      ULStat = cbDisableEvent(TestBoard, ON_PRETRIGGER)
      If (ULStat = NOERRORS) Then _
         EventsFound = (EventsFound Or PRETRIGEVENT)
   End If
   If (EventType And ENDOUTEVENT) > 0 Then
       ULStat = cbDisableEvent(TestBoard, ON_END_OF_AO_SCAN)
      If (ULStat = NOERRORS) Then _
         EventsFound = (EventsFound Or ENDOUTEVENT)
   End If
   
   ULStat = cbErrHandling(ReportError, HandleError)
   FindEventsOfType = EventsFound

End Function

Public Sub SetEventDefaults(ByVal ReportErr As Long, ByVal HandleErr As Long)

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
   EventGeneralError = True
   
End Sub

