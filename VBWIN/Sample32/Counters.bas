Attribute VB_Name = "Counters"
Global Const CTR8254 = 1
Global Const CTR9513 = 2
Global Const CTR8536 = 3
Global Const CTR7266 = 4
Global Const CTREVENT = 5
Global Const CTRSCAN = 6
Global Const CTRTMR = 7
Global Const CTRQUAD = 8
Global Const CTRPULSE = 9
Global CtrGeneralError As Boolean

Function FindCountersOfType(ByVal BoardNum As Long, ByVal CounterType As Long, DefaultCtr As Long) As Long

   ULStat& = cbGetConfig(BOARDINFO, BoardNum, 0, BICINUMDEVS, NumCounters&)
   If ULStat <> NOERRORS Then
      DisplayError ULStat
      FindCountersOfType = 0
      Exit Function
   End If
   DefaultCtr = -1
   For CtrDev& = 0 To NumCounters& - 1
      ULStat& = cbGetConfig(COUNTERINFO, BoardNum, CtrDev&, CICTRTYPE, ThisType&)
      If ThisType& = CounterType Then
         ULStat& = cbGetConfig(COUNTERINFO, BoardNum, CtrDev&, CICTRNUM, CounterNum&)
         If ULStat& = 0 Then
            CtrsFound& = CtrsFound& + 1
            If DefaultCtr = -1 Then DefaultCtr = CounterNum&
         End If
      End If
   Next
   FindCountersOfType = CtrsFound&
   
End Function

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
   CtrGeneralError = True
   
End Sub

