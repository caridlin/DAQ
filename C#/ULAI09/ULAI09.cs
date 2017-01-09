// ==============================================================================
//
//  File:                         ULAI09.CS
//
//  Library Call Demonstrated:    Mccdaq.MccBoard.AConvertPretrigData()
//
//  Purpose:                      Waits for a trigger with Mccdaq.MccBoard.APretrig(), then
//                                uses Mccdaq.MccBoard.AConvertPretrigData() to convert
//                                the data.
//
//  Demonstration:                Displays the analog input on one channel and
//                                waits for the trigger.
//
//  Other Library Calls:          Mccdaq.MccBoard.APretrig()
//                                Mccdaq.MccBoard.GetStatus()
//                                Mccdaq.MccBoard.StopBackground()
//                                MccDaq.MccService.ErrHandling()
//
//  Special Requirements:         Board 0 must support pre/post triggering
//
// ==============================================================================

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Diagnostics;

using AnalogIO;
using MccDaq;
using ErrorDefs;

namespace ULAI09
{
	public class frmPreTrig : System.Windows.Forms.Form
	{

        // Create a new MccBoard object for Board 0
        MccDaq.MccBoard DaqBoard = new MccDaq.MccBoard(0);

        MccDaq.Range Range;
        private int ADResolution, NumAIChans;
        private int LowChan, MaxChan;
        private MccDaq.TriggerType DefaultTrig;

        const int NumPoints = 4096;		    //  Number of data points to collect
		const int FirstPoint = 0;		    //  set first element in buffer to transfer to array
        const int BufSize = 4608;  		    //  buffer must be large enough to hold all data
        int PretrigCount;
        int TotalCount;

        private IntPtr MemHandle;	//  define a variable to contain the handle for memory allocated 
								    //  by Windows through MccDaq.MccService.WinBufAlloc()
		private ushort[] ADData;    //  size must be TotalCount + 512 minimum
		private ushort[] ChanTags;  //  array to hold the channel tag values

        AnalogIO.clsAnalogIO AIOProps = new AnalogIO.clsAnalogIO();

        private void frmPreTrig_Load(object sender, EventArgs e)
        {

            InitUL();

            // determine the number of analog channels and their capabilities
            int ChannelType = clsAnalogIO.PRETRIGIN;
            NumAIChans = AIOProps.FindAnalogChansOfType(DaqBoard, ChannelType,
                out ADResolution, out Range, out LowChan, out DefaultTrig);

            if (NumAIChans == 0)
            {
                lblInstruction.Text = "Board " + DaqBoard.BoardNum.ToString() +
                    " does not have analog input channels.";
                cmdTrigEnable.Enabled = false;
            }
            else if (ADResolution > 16)
            {
                lblInstruction.Text = "Board " + DaqBoard.BoardNum.ToString() +
                " is greater than 16-bit resolution. The AConvertPretrigData " +
                "function does not support high resolution devices.";
                cmdTrigEnable.Enabled = false;
            }
            else
            {
                // set aside memory to hold 16-bit data
                ADData = new ushort[BufSize];
                ChanTags = new ushort[BufSize];
                MemHandle = MccDaq.MccService.WinBufAllocEx(BufSize);
                if (MemHandle == IntPtr.Zero)
                {
                    cmdTrigEnable.Enabled = false;
                    NumAIChans = 0;
                }
                if (NumAIChans > 8) NumAIChans = 8;
                MaxChan = LowChan + NumAIChans - 1;
                lblInstruction.Text = "Board " + DaqBoard.BoardNum.ToString() +
                    " collecting analog data on channel 0 using APretrig " +
                    "in background mode with Range set to " + Range.ToString() +
                    ". Data is aligned after acquisition using AConvertPretrigData.";
            }

        }

        private void cmdTrigEnable_Click(object eventSender, System.EventArgs eventArgs) /* Handles cmdTrigEnable.Click */
        {
            MccDaq.ErrorInfo ULStat;
            MccDaq.ScanOptions Options;
            int Rate;
            int HighChan;
            float engUnits;

            lblResult.Text = "Waiting for trigger on trigger input.";

            //  Monitor a range of channels for a trigger then collect the values
            //  with MccDaq.MccBoard.APretrig()

            //  Parameters:
            //    LowChan       :first A/D channel of the scan
            //    HighChan      :last A/D channel of the scan
            //    PretrigCount  :number of pre-trigger A/D samples to collect
            //    TotalCount    :total number of A/D samples to collect
            //    Rate          :per channel sampling rate ((samples per second) per channel)
            //    Range         :the range for the board
            //    MemHandle     :Handle for Windows buffer to store data in
            //    Options       :data collection options

            HighChan = LowChan;
            PretrigCount = 1000;	    //  number of data points before the trigger to store
            TotalCount = NumPoints;   //  total number of data points to collect
            Rate = 1000;				//  per channel sampling rate ((samples per second) per channel)
            Options = MccDaq.ScanOptions.Background; //  collect data in the background

            if (DefaultTrig == MccDaq.TriggerType.TrigAbove)
            {
                //The default trigger configuration for most devices is
                //rising edge digital trigger, but some devices do not
                //support this type for pretrigger functions.
                short MidScale;
                MidScale = Convert.ToInt16((Math.Pow(2, ADResolution) / 2) - 1);
                ULStat = DaqBoard.SetTrigger(DefaultTrig, MidScale, MidScale);
                ULStat = DaqBoard.ToEngUnits(Range, MidScale, out engUnits);
                lblResult.Text = "Waiting for trigger on analog input above " +
                    engUnits.ToString("0.00") + "V.";
            }

            ULStat = DaqBoard.APretrig(LowChan, HighChan, ref PretrigCount,
                ref TotalCount, ref Rate, Range, MemHandle, Options);

            if (ULStat.Value == MccDaq.ErrorInfo.ErrorCode.BadBoardType)
            {
                lblInstruction.Text = "Board " + DaqBoard.BoardNum.ToString()
                    + " does not support the APretrig function.";
                lblResult.Text = "APretrig function aborted.";
                System.Windows.Forms.Application.DoEvents();
            }
            else if (!(ULStat.Value == MccDaq.ErrorInfo.ErrorCode.NoErrors))
            {
                lblInstruction.Text = ULStat.Message;
                lblResult.Text = "APretrig function aborted.";
                System.Windows.Forms.Application.DoEvents();
            }
            else
            {
                tmrCheckStatus.Enabled = true;
                cmdTrigEnable.Enabled = false;
            }

        }

        private void tmrCheckStatus_Tick(object eventSender, System.EventArgs eventArgs) /* Handles tmrCheckStatus.Tick */
        {
            short i;
            MccDaq.ErrorInfo ULStat;
            int CurIndex;
            int CurCount;
            short Status;
            int DataElement, TrigPoint, SampleNum;

            tmrCheckStatus.Stop();

            //  This timer will check the status of the background data collection

            //  Parameters:
            //    Status     :current status of the background data collection
            //    CurCount   :current number of samples collected
            //    CurIndex   :index to the data buffer pointing to the start of the
            //                 most recently collected scan
            //    FunctionType:A/D operation (MccDaq.FunctionType.AiFunction)

            ULStat = DaqBoard.GetStatus(out Status, out CurCount, out CurIndex, MccDaq.FunctionType.AiFunction);

            if (ULStat.Value == MccDaq.ErrorInfo.ErrorCode.TooFew)
                lblResult.Text = "Premature trigger occurred.";
            else if (!(ULStat.Value == MccDaq.ErrorInfo.ErrorCode.NoErrors))
                lblResult.Text = ULStat.Message;
            else
                if (CurCount > PretrigCount) lblResult.Text = "";

            lblShowCount.Text = CurCount.ToString("D");
            lblShowIndex.Text = CurIndex.ToString("D");

            //  Check if the background operation has finished. If it has, then
            //  transfer the data from the memory buffer set up by Windows to an
            //  array for use by Visual Basic
            //  The BACKGROUND operation must be explicitly stopped

            if (Status == MccDaq.MccBoard.Running)
            {
                lblShowStat.Text = "Running";
                tmrCheckStatus.Start();
            }
            else if (Status == MccDaq.MccBoard.Idle)
            {
                lblShowStat.Text = "Idle";
                TrigPoint = PretrigCount - (NumPoints - CurCount) - 1;
                if (ULStat.Value == MccDaq.ErrorInfo.ErrorCode.TooFew)
                {
                    lblResult.Text = "Premature trigger occurred at sample "
                        + TrigPoint.ToString() + ".";
                }
                else if (!(ULStat.Value == MccDaq.ErrorInfo.ErrorCode.NoErrors))
                {
                    lblResult.Text = ULStat.Message;
                }
                ULStat = DaqBoard.StopBackground(MccDaq.FunctionType.AiFunction);

                //  Transfer the data from the memory buffer set up by Windows to an array
                ULStat = MccDaq.MccService.WinBufToArray(MemHandle, ADData, FirstPoint, NumPoints);

                //  convert the data using MccDaq.MccBoard.AConvertPretrigData()

                //  Parameters:
                //    PretrigCount  :number of pre-trigger A/D samples collected
                //                    actual value is returned by Mccdaq.MccBoard.APretrig()
                //    TotalCount    :total number of A/D samples collected
                //                    actual value is returned by Mccdaq.MccBoard.APretrig()
                //    ADData        :the array containing the raw data values
                //    ChanTags      :array that chan tags will be returned to

                ULStat = DaqBoard.AConvertPretrigData(PretrigCount, TotalCount, ADData, ChanTags);

                for (i = 1; i <= 10; ++i)
                {
                    DataElement = PretrigCount - (12 - i);
                    if (!(DataElement < 0))
                        lblPreTrig[i - 1].Text = ADData[DataElement].ToString("D");
                    SampleNum = TrigPoint - i;
                    lblPreSamp[i - 1].Text = "";
                    if (!(SampleNum < 0))
                        lblPreSamp[i - 1].Text = "Sample " + SampleNum.ToString();
                }
                for (i = 0; i <= 9; ++i)
                {
                    DataElement = PretrigCount + i - 1;
                    lblPostTrig[i].Text = ADData[DataElement].ToString("D");
                    SampleNum = TrigPoint + i;
                    lblPostSamp[i].Text = "Sample " + SampleNum.ToString();
                }
                cmdTrigEnable.Enabled = true;
            }

        }

        private void cmdQuit_Click(object eventSender, System.EventArgs eventArgs) /* Handles cmdQuit.Click */
        {
            //  The BACKGROUND operation must be explicitly stopped
            MccDaq.ErrorInfo ULStat = DaqBoard.StopBackground(MccDaq.FunctionType.AiFunction);


            //  Free up memory for use by  other programs
            ULStat = MccDaq.MccService.WinBufFreeEx(MemHandle);

            MemHandle = IntPtr.Zero;

            Application.Exit();
        }

        private void InitUL()
        {

            //  Initiate error handling
            //   activating error handling will trap errors like
            //   bad channel numbers and non-configured conditions.
            //   Parameters:
            //     MccDaq.ErrorReporting.DontPrint :all warnings and errors will be handled locally
            //     MccDaq.ErrorHandling.DontPrint  :if any error is encountered, the program continues

            clsErrorDefs.ReportError = MccDaq.ErrorReporting.DontPrint;
            clsErrorDefs.HandleError = MccDaq.ErrorHandling.DontStop;
            MccDaq.ErrorInfo ULStat = MccDaq.MccService.ErrHandling
                (clsErrorDefs.ReportError, clsErrorDefs.HandleError);

            //  This gives us access to labels via an indexed array
            lblPostTrig = (new Label[] {_lblPostTrig_1, _lblPostTrig_2, 
                _lblPostTrig_3, _lblPostTrig_4, _lblPostTrig_5, 
                _lblPostTrig_6, _lblPostTrig_7, _lblPostTrig_8, 
                _lblPostTrig_9, _lblPostTrig_10});

            lblPreTrig = (new Label[] {_lblPreTrig_0, _lblPreTrig_1, 
                _lblPreTrig_2, _lblPreTrig_3, _lblPreTrig_4, 
                _lblPreTrig_5, _lblPreTrig_6, _lblPreTrig_7, 
                _lblPreTrig_8, _lblPreTrig_9});

            lblPreSamp = (new Label[] {lblPre1, lblPre2, lblPre3, lblPre4, 
                lblPre5, lblPre6, lblPre7, lblPre8, lblPre9, lblPre10});

            lblPostSamp = (new Label[] {lblPost1, lblPost2, lblPost3, lblPost4, 
                lblPost5, lblPost6, lblPost7, lblPost8, lblPost9, lblPost10});
        }

        #region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
	    
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.ToolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.cmdQuit = new System.Windows.Forms.Button();
            this.cmdTrigEnable = new System.Windows.Forms.Button();
            this.tmrCheckStatus = new System.Windows.Forms.Timer(this.components);
            this.lblShowCount = new System.Windows.Forms.Label();
            this.lblCount = new System.Windows.Forms.Label();
            this.lblShowIndex = new System.Windows.Forms.Label();
            this.lblIndex = new System.Windows.Forms.Label();
            this.lblShowStat = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this._lblPostTrig_10 = new System.Windows.Forms.Label();
            this.lblPost10 = new System.Windows.Forms.Label();
            this._lblPreTrig_9 = new System.Windows.Forms.Label();
            this.lblPre1 = new System.Windows.Forms.Label();
            this._lblPostTrig_9 = new System.Windows.Forms.Label();
            this.lblPost9 = new System.Windows.Forms.Label();
            this._lblPreTrig_8 = new System.Windows.Forms.Label();
            this.lblPre2 = new System.Windows.Forms.Label();
            this._lblPostTrig_8 = new System.Windows.Forms.Label();
            this.lblPost8 = new System.Windows.Forms.Label();
            this._lblPreTrig_7 = new System.Windows.Forms.Label();
            this.lblPre3 = new System.Windows.Forms.Label();
            this._lblPostTrig_7 = new System.Windows.Forms.Label();
            this.lblPost7 = new System.Windows.Forms.Label();
            this._lblPreTrig_6 = new System.Windows.Forms.Label();
            this.lblPre4 = new System.Windows.Forms.Label();
            this._lblPostTrig_6 = new System.Windows.Forms.Label();
            this.lblPost6 = new System.Windows.Forms.Label();
            this._lblPreTrig_5 = new System.Windows.Forms.Label();
            this.lblPre5 = new System.Windows.Forms.Label();
            this._lblPostTrig_5 = new System.Windows.Forms.Label();
            this.lblPost5 = new System.Windows.Forms.Label();
            this._lblPreTrig_4 = new System.Windows.Forms.Label();
            this.lblPre6 = new System.Windows.Forms.Label();
            this._lblPostTrig_4 = new System.Windows.Forms.Label();
            this.lblPost4 = new System.Windows.Forms.Label();
            this._lblPreTrig_3 = new System.Windows.Forms.Label();
            this.lblPre7 = new System.Windows.Forms.Label();
            this._lblPostTrig_3 = new System.Windows.Forms.Label();
            this.lblPost3 = new System.Windows.Forms.Label();
            this._lblPreTrig_2 = new System.Windows.Forms.Label();
            this.lblPre8 = new System.Windows.Forms.Label();
            this._lblPostTrig_2 = new System.Windows.Forms.Label();
            this.lblPost2 = new System.Windows.Forms.Label();
            this._lblPreTrig_1 = new System.Windows.Forms.Label();
            this.lblPre9 = new System.Windows.Forms.Label();
            this._lblPostTrig_1 = new System.Windows.Forms.Label();
            this.lblPost1 = new System.Windows.Forms.Label();
            this._lblPreTrig_0 = new System.Windows.Forms.Label();
            this.lblPre10 = new System.Windows.Forms.Label();
            this.lblPostTrigData = new System.Windows.Forms.Label();
            this.lblPreTrigData = new System.Windows.Forms.Label();
            this.lblDemoFunction = new System.Windows.Forms.Label();
            this.lblInstruction = new System.Windows.Forms.Label();
            this.lblResult = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cmdQuit
            // 
            this.cmdQuit.BackColor = System.Drawing.SystemColors.Control;
            this.cmdQuit.Cursor = System.Windows.Forms.Cursors.Default;
            this.cmdQuit.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdQuit.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmdQuit.Location = new System.Drawing.Point(328, 324);
            this.cmdQuit.Name = "cmdQuit";
            this.cmdQuit.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmdQuit.Size = new System.Drawing.Size(52, 26);
            this.cmdQuit.TabIndex = 17;
            this.cmdQuit.Text = "Quit";
            this.cmdQuit.UseVisualStyleBackColor = false;
            this.cmdQuit.Click += new System.EventHandler(this.cmdQuit_Click);
            // 
            // cmdTrigEnable
            // 
            this.cmdTrigEnable.BackColor = System.Drawing.SystemColors.Control;
            this.cmdTrigEnable.Cursor = System.Windows.Forms.Cursors.Default;
            this.cmdTrigEnable.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdTrigEnable.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmdTrigEnable.Location = new System.Drawing.Point(67, 88);
            this.cmdTrigEnable.Name = "cmdTrigEnable";
            this.cmdTrigEnable.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmdTrigEnable.Size = new System.Drawing.Size(256, 28);
            this.cmdTrigEnable.TabIndex = 18;
            this.cmdTrigEnable.Text = "Start Pre/Post Trigger background operation";
            this.cmdTrigEnable.UseVisualStyleBackColor = false;
            this.cmdTrigEnable.Click += new System.EventHandler(this.cmdTrigEnable_Click);
            // 
            // tmrCheckStatus
            // 
            this.tmrCheckStatus.Interval = 200;
            this.tmrCheckStatus.Tick += new System.EventHandler(this.tmrCheckStatus_Tick);
            // 
            // lblShowCount
            // 
            this.lblShowCount.BackColor = System.Drawing.SystemColors.Window;
            this.lblShowCount.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblShowCount.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblShowCount.ForeColor = System.Drawing.Color.Blue;
            this.lblShowCount.Location = new System.Drawing.Point(307, 285);
            this.lblShowCount.Name = "lblShowCount";
            this.lblShowCount.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblShowCount.Size = new System.Drawing.Size(34, 14);
            this.lblShowCount.TabIndex = 52;
            // 
            // lblCount
            // 
            this.lblCount.BackColor = System.Drawing.SystemColors.Window;
            this.lblCount.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblCount.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCount.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblCount.Location = new System.Drawing.Point(249, 285);
            this.lblCount.Name = "lblCount";
            this.lblCount.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblCount.Size = new System.Drawing.Size(55, 14);
            this.lblCount.TabIndex = 49;
            this.lblCount.Text = "Count:";
            // 
            // lblShowIndex
            // 
            this.lblShowIndex.BackColor = System.Drawing.SystemColors.Window;
            this.lblShowIndex.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblShowIndex.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblShowIndex.ForeColor = System.Drawing.Color.Blue;
            this.lblShowIndex.Location = new System.Drawing.Point(198, 285);
            this.lblShowIndex.Name = "lblShowIndex";
            this.lblShowIndex.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblShowIndex.Size = new System.Drawing.Size(41, 14);
            this.lblShowIndex.TabIndex = 53;
            // 
            // lblIndex
            // 
            this.lblIndex.BackColor = System.Drawing.SystemColors.Window;
            this.lblIndex.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblIndex.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIndex.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblIndex.Location = new System.Drawing.Point(144, 285);
            this.lblIndex.Name = "lblIndex";
            this.lblIndex.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblIndex.Size = new System.Drawing.Size(55, 14);
            this.lblIndex.TabIndex = 51;
            this.lblIndex.Text = "Index:";
            // 
            // lblShowStat
            // 
            this.lblShowStat.BackColor = System.Drawing.SystemColors.Window;
            this.lblShowStat.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblShowStat.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblShowStat.ForeColor = System.Drawing.Color.Blue;
            this.lblShowStat.Location = new System.Drawing.Point(64, 285);
            this.lblShowStat.Name = "lblShowStat";
            this.lblShowStat.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblShowStat.Size = new System.Drawing.Size(70, 14);
            this.lblShowStat.TabIndex = 50;
            // 
            // lblStatus
            // 
            this.lblStatus.BackColor = System.Drawing.SystemColors.Window;
            this.lblStatus.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblStatus.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblStatus.Location = new System.Drawing.Point(16, 285);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblStatus.Size = new System.Drawing.Size(47, 14);
            this.lblStatus.TabIndex = 48;
            this.lblStatus.Text = "Status:";
            // 
            // _lblPostTrig_10
            // 
            this._lblPostTrig_10.BackColor = System.Drawing.SystemColors.Window;
            this._lblPostTrig_10.Cursor = System.Windows.Forms.Cursors.Default;
            this._lblPostTrig_10.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblPostTrig_10.ForeColor = System.Drawing.Color.Blue;
            this._lblPostTrig_10.Location = new System.Drawing.Point(277, 258);
            this._lblPostTrig_10.Name = "_lblPostTrig_10";
            this._lblPostTrig_10.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._lblPostTrig_10.Size = new System.Drawing.Size(65, 14);
            this._lblPostTrig_10.TabIndex = 42;
            this._lblPostTrig_10.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblPost10
            // 
            this.lblPost10.BackColor = System.Drawing.SystemColors.Window;
            this.lblPost10.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblPost10.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPost10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.lblPost10.Location = new System.Drawing.Point(206, 258);
            this.lblPost10.Name = "lblPost10";
            this.lblPost10.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblPost10.Size = new System.Drawing.Size(73, 14);
            this.lblPost10.TabIndex = 40;
            this.lblPost10.Text = "Trigger +10";
            // 
            // _lblPreTrig_9
            // 
            this._lblPreTrig_9.BackColor = System.Drawing.SystemColors.Window;
            this._lblPreTrig_9.Cursor = System.Windows.Forms.Cursors.Default;
            this._lblPreTrig_9.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblPreTrig_9.ForeColor = System.Drawing.Color.Blue;
            this._lblPreTrig_9.Location = new System.Drawing.Point(96, 258);
            this._lblPreTrig_9.Name = "_lblPreTrig_9";
            this._lblPreTrig_9.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._lblPreTrig_9.Size = new System.Drawing.Size(65, 14);
            this._lblPreTrig_9.TabIndex = 22;
            this._lblPreTrig_9.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblPre1
            // 
            this.lblPre1.BackColor = System.Drawing.SystemColors.Window;
            this.lblPre1.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblPre1.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPre1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.lblPre1.Location = new System.Drawing.Point(18, 258);
            this.lblPre1.Name = "lblPre1";
            this.lblPre1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblPre1.Size = new System.Drawing.Size(73, 14);
            this.lblPre1.TabIndex = 20;
            this.lblPre1.Text = "Trigger -1";
            // 
            // _lblPostTrig_9
            // 
            this._lblPostTrig_9.BackColor = System.Drawing.SystemColors.Window;
            this._lblPostTrig_9.Cursor = System.Windows.Forms.Cursors.Default;
            this._lblPostTrig_9.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblPostTrig_9.ForeColor = System.Drawing.Color.Blue;
            this._lblPostTrig_9.Location = new System.Drawing.Point(277, 246);
            this._lblPostTrig_9.Name = "_lblPostTrig_9";
            this._lblPostTrig_9.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._lblPostTrig_9.Size = new System.Drawing.Size(65, 14);
            this._lblPostTrig_9.TabIndex = 41;
            this._lblPostTrig_9.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblPost9
            // 
            this.lblPost9.BackColor = System.Drawing.SystemColors.Window;
            this.lblPost9.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblPost9.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPost9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.lblPost9.Location = new System.Drawing.Point(206, 246);
            this.lblPost9.Name = "lblPost9";
            this.lblPost9.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblPost9.Size = new System.Drawing.Size(73, 14);
            this.lblPost9.TabIndex = 39;
            this.lblPost9.Text = "Trigger +9";
            // 
            // _lblPreTrig_8
            // 
            this._lblPreTrig_8.BackColor = System.Drawing.SystemColors.Window;
            this._lblPreTrig_8.Cursor = System.Windows.Forms.Cursors.Default;
            this._lblPreTrig_8.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblPreTrig_8.ForeColor = System.Drawing.Color.Blue;
            this._lblPreTrig_8.Location = new System.Drawing.Point(96, 246);
            this._lblPreTrig_8.Name = "_lblPreTrig_8";
            this._lblPreTrig_8.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._lblPreTrig_8.Size = new System.Drawing.Size(65, 14);
            this._lblPreTrig_8.TabIndex = 21;
            this._lblPreTrig_8.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblPre2
            // 
            this.lblPre2.BackColor = System.Drawing.SystemColors.Window;
            this.lblPre2.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblPre2.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPre2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.lblPre2.Location = new System.Drawing.Point(18, 246);
            this.lblPre2.Name = "lblPre2";
            this.lblPre2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblPre2.Size = new System.Drawing.Size(73, 14);
            this.lblPre2.TabIndex = 19;
            this.lblPre2.Text = "Trigger -2";
            // 
            // _lblPostTrig_8
            // 
            this._lblPostTrig_8.BackColor = System.Drawing.SystemColors.Window;
            this._lblPostTrig_8.Cursor = System.Windows.Forms.Cursors.Default;
            this._lblPostTrig_8.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblPostTrig_8.ForeColor = System.Drawing.Color.Blue;
            this._lblPostTrig_8.Location = new System.Drawing.Point(277, 233);
            this._lblPostTrig_8.Name = "_lblPostTrig_8";
            this._lblPostTrig_8.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._lblPostTrig_8.Size = new System.Drawing.Size(65, 14);
            this._lblPostTrig_8.TabIndex = 38;
            this._lblPostTrig_8.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblPost8
            // 
            this.lblPost8.BackColor = System.Drawing.SystemColors.Window;
            this.lblPost8.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblPost8.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPost8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.lblPost8.Location = new System.Drawing.Point(206, 233);
            this.lblPost8.Name = "lblPost8";
            this.lblPost8.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblPost8.Size = new System.Drawing.Size(73, 14);
            this.lblPost8.TabIndex = 37;
            this.lblPost8.Text = "Trigger +8";
            // 
            // _lblPreTrig_7
            // 
            this._lblPreTrig_7.BackColor = System.Drawing.SystemColors.Window;
            this._lblPreTrig_7.Cursor = System.Windows.Forms.Cursors.Default;
            this._lblPreTrig_7.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblPreTrig_7.ForeColor = System.Drawing.Color.Blue;
            this._lblPreTrig_7.Location = new System.Drawing.Point(96, 233);
            this._lblPreTrig_7.Name = "_lblPreTrig_7";
            this._lblPreTrig_7.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._lblPreTrig_7.Size = new System.Drawing.Size(65, 14);
            this._lblPreTrig_7.TabIndex = 16;
            this._lblPreTrig_7.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblPre3
            // 
            this.lblPre3.BackColor = System.Drawing.SystemColors.Window;
            this.lblPre3.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblPre3.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPre3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.lblPre3.Location = new System.Drawing.Point(18, 233);
            this.lblPre3.Name = "lblPre3";
            this.lblPre3.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblPre3.Size = new System.Drawing.Size(73, 14);
            this.lblPre3.TabIndex = 8;
            this.lblPre3.Text = "Trigger -3";
            // 
            // _lblPostTrig_7
            // 
            this._lblPostTrig_7.BackColor = System.Drawing.SystemColors.Window;
            this._lblPostTrig_7.Cursor = System.Windows.Forms.Cursors.Default;
            this._lblPostTrig_7.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblPostTrig_7.ForeColor = System.Drawing.Color.Blue;
            this._lblPostTrig_7.Location = new System.Drawing.Point(277, 220);
            this._lblPostTrig_7.Name = "_lblPostTrig_7";
            this._lblPostTrig_7.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._lblPostTrig_7.Size = new System.Drawing.Size(65, 14);
            this._lblPostTrig_7.TabIndex = 34;
            this._lblPostTrig_7.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblPost7
            // 
            this.lblPost7.BackColor = System.Drawing.SystemColors.Window;
            this.lblPost7.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblPost7.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPost7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.lblPost7.Location = new System.Drawing.Point(206, 220);
            this.lblPost7.Name = "lblPost7";
            this.lblPost7.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblPost7.Size = new System.Drawing.Size(73, 14);
            this.lblPost7.TabIndex = 33;
            this.lblPost7.Text = "Trigger +7";
            // 
            // _lblPreTrig_6
            // 
            this._lblPreTrig_6.BackColor = System.Drawing.SystemColors.Window;
            this._lblPreTrig_6.Cursor = System.Windows.Forms.Cursors.Default;
            this._lblPreTrig_6.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblPreTrig_6.ForeColor = System.Drawing.Color.Blue;
            this._lblPreTrig_6.Location = new System.Drawing.Point(96, 220);
            this._lblPreTrig_6.Name = "_lblPreTrig_6";
            this._lblPreTrig_6.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._lblPreTrig_6.Size = new System.Drawing.Size(65, 14);
            this._lblPreTrig_6.TabIndex = 15;
            this._lblPreTrig_6.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblPre4
            // 
            this.lblPre4.BackColor = System.Drawing.SystemColors.Window;
            this.lblPre4.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblPre4.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPre4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.lblPre4.Location = new System.Drawing.Point(18, 220);
            this.lblPre4.Name = "lblPre4";
            this.lblPre4.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblPre4.Size = new System.Drawing.Size(73, 14);
            this.lblPre4.TabIndex = 7;
            this.lblPre4.Text = "Trigger -4";
            // 
            // _lblPostTrig_6
            // 
            this._lblPostTrig_6.BackColor = System.Drawing.SystemColors.Window;
            this._lblPostTrig_6.Cursor = System.Windows.Forms.Cursors.Default;
            this._lblPostTrig_6.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblPostTrig_6.ForeColor = System.Drawing.Color.Blue;
            this._lblPostTrig_6.Location = new System.Drawing.Point(277, 207);
            this._lblPostTrig_6.Name = "_lblPostTrig_6";
            this._lblPostTrig_6.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._lblPostTrig_6.Size = new System.Drawing.Size(65, 14);
            this._lblPostTrig_6.TabIndex = 30;
            this._lblPostTrig_6.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblPost6
            // 
            this.lblPost6.BackColor = System.Drawing.SystemColors.Window;
            this.lblPost6.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblPost6.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPost6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.lblPost6.Location = new System.Drawing.Point(206, 207);
            this.lblPost6.Name = "lblPost6";
            this.lblPost6.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblPost6.Size = new System.Drawing.Size(73, 14);
            this.lblPost6.TabIndex = 29;
            this.lblPost6.Text = "Trigger +6";
            // 
            // _lblPreTrig_5
            // 
            this._lblPreTrig_5.BackColor = System.Drawing.SystemColors.Window;
            this._lblPreTrig_5.Cursor = System.Windows.Forms.Cursors.Default;
            this._lblPreTrig_5.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblPreTrig_5.ForeColor = System.Drawing.Color.Blue;
            this._lblPreTrig_5.Location = new System.Drawing.Point(96, 207);
            this._lblPreTrig_5.Name = "_lblPreTrig_5";
            this._lblPreTrig_5.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._lblPreTrig_5.Size = new System.Drawing.Size(65, 14);
            this._lblPreTrig_5.TabIndex = 14;
            this._lblPreTrig_5.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblPre5
            // 
            this.lblPre5.BackColor = System.Drawing.SystemColors.Window;
            this.lblPre5.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblPre5.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPre5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.lblPre5.Location = new System.Drawing.Point(18, 207);
            this.lblPre5.Name = "lblPre5";
            this.lblPre5.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblPre5.Size = new System.Drawing.Size(73, 14);
            this.lblPre5.TabIndex = 6;
            this.lblPre5.Text = "Trigger -5";
            // 
            // _lblPostTrig_5
            // 
            this._lblPostTrig_5.BackColor = System.Drawing.SystemColors.Window;
            this._lblPostTrig_5.Cursor = System.Windows.Forms.Cursors.Default;
            this._lblPostTrig_5.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblPostTrig_5.ForeColor = System.Drawing.Color.Blue;
            this._lblPostTrig_5.Location = new System.Drawing.Point(277, 194);
            this._lblPostTrig_5.Name = "_lblPostTrig_5";
            this._lblPostTrig_5.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._lblPostTrig_5.Size = new System.Drawing.Size(65, 14);
            this._lblPostTrig_5.TabIndex = 26;
            this._lblPostTrig_5.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblPost5
            // 
            this.lblPost5.BackColor = System.Drawing.SystemColors.Window;
            this.lblPost5.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblPost5.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPost5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.lblPost5.Location = new System.Drawing.Point(206, 194);
            this.lblPost5.Name = "lblPost5";
            this.lblPost5.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblPost5.Size = new System.Drawing.Size(73, 14);
            this.lblPost5.TabIndex = 25;
            this.lblPost5.Text = "Trigger +5";
            // 
            // _lblPreTrig_4
            // 
            this._lblPreTrig_4.BackColor = System.Drawing.SystemColors.Window;
            this._lblPreTrig_4.Cursor = System.Windows.Forms.Cursors.Default;
            this._lblPreTrig_4.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblPreTrig_4.ForeColor = System.Drawing.Color.Blue;
            this._lblPreTrig_4.Location = new System.Drawing.Point(96, 194);
            this._lblPreTrig_4.Name = "_lblPreTrig_4";
            this._lblPreTrig_4.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._lblPreTrig_4.Size = new System.Drawing.Size(65, 14);
            this._lblPreTrig_4.TabIndex = 13;
            this._lblPreTrig_4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblPre6
            // 
            this.lblPre6.BackColor = System.Drawing.SystemColors.Window;
            this.lblPre6.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblPre6.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPre6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.lblPre6.Location = new System.Drawing.Point(18, 194);
            this.lblPre6.Name = "lblPre6";
            this.lblPre6.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblPre6.Size = new System.Drawing.Size(73, 14);
            this.lblPre6.TabIndex = 5;
            this.lblPre6.Text = "Trigger -6";
            // 
            // _lblPostTrig_4
            // 
            this._lblPostTrig_4.BackColor = System.Drawing.SystemColors.Window;
            this._lblPostTrig_4.Cursor = System.Windows.Forms.Cursors.Default;
            this._lblPostTrig_4.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblPostTrig_4.ForeColor = System.Drawing.Color.Blue;
            this._lblPostTrig_4.Location = new System.Drawing.Point(277, 182);
            this._lblPostTrig_4.Name = "_lblPostTrig_4";
            this._lblPostTrig_4.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._lblPostTrig_4.Size = new System.Drawing.Size(65, 14);
            this._lblPostTrig_4.TabIndex = 36;
            this._lblPostTrig_4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblPost4
            // 
            this.lblPost4.BackColor = System.Drawing.SystemColors.Window;
            this.lblPost4.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblPost4.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPost4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.lblPost4.Location = new System.Drawing.Point(206, 182);
            this.lblPost4.Name = "lblPost4";
            this.lblPost4.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblPost4.Size = new System.Drawing.Size(73, 14);
            this.lblPost4.TabIndex = 35;
            this.lblPost4.Text = "Trigger +4";
            // 
            // _lblPreTrig_3
            // 
            this._lblPreTrig_3.BackColor = System.Drawing.SystemColors.Window;
            this._lblPreTrig_3.Cursor = System.Windows.Forms.Cursors.Default;
            this._lblPreTrig_3.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblPreTrig_3.ForeColor = System.Drawing.Color.Blue;
            this._lblPreTrig_3.Location = new System.Drawing.Point(96, 182);
            this._lblPreTrig_3.Name = "_lblPreTrig_3";
            this._lblPreTrig_3.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._lblPreTrig_3.Size = new System.Drawing.Size(65, 14);
            this._lblPreTrig_3.TabIndex = 12;
            this._lblPreTrig_3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblPre7
            // 
            this.lblPre7.BackColor = System.Drawing.SystemColors.Window;
            this.lblPre7.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblPre7.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPre7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.lblPre7.Location = new System.Drawing.Point(18, 182);
            this.lblPre7.Name = "lblPre7";
            this.lblPre7.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblPre7.Size = new System.Drawing.Size(73, 14);
            this.lblPre7.TabIndex = 4;
            this.lblPre7.Text = "Trigger -7";
            // 
            // _lblPostTrig_3
            // 
            this._lblPostTrig_3.BackColor = System.Drawing.SystemColors.Window;
            this._lblPostTrig_3.Cursor = System.Windows.Forms.Cursors.Default;
            this._lblPostTrig_3.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblPostTrig_3.ForeColor = System.Drawing.Color.Blue;
            this._lblPostTrig_3.Location = new System.Drawing.Point(277, 169);
            this._lblPostTrig_3.Name = "_lblPostTrig_3";
            this._lblPostTrig_3.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._lblPostTrig_3.Size = new System.Drawing.Size(65, 14);
            this._lblPostTrig_3.TabIndex = 32;
            this._lblPostTrig_3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblPost3
            // 
            this.lblPost3.BackColor = System.Drawing.SystemColors.Window;
            this.lblPost3.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblPost3.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPost3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.lblPost3.Location = new System.Drawing.Point(206, 169);
            this.lblPost3.Name = "lblPost3";
            this.lblPost3.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblPost3.Size = new System.Drawing.Size(73, 14);
            this.lblPost3.TabIndex = 31;
            this.lblPost3.Text = "Trigger +3";
            // 
            // _lblPreTrig_2
            // 
            this._lblPreTrig_2.BackColor = System.Drawing.SystemColors.Window;
            this._lblPreTrig_2.Cursor = System.Windows.Forms.Cursors.Default;
            this._lblPreTrig_2.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblPreTrig_2.ForeColor = System.Drawing.Color.Blue;
            this._lblPreTrig_2.Location = new System.Drawing.Point(96, 169);
            this._lblPreTrig_2.Name = "_lblPreTrig_2";
            this._lblPreTrig_2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._lblPreTrig_2.Size = new System.Drawing.Size(65, 14);
            this._lblPreTrig_2.TabIndex = 11;
            this._lblPreTrig_2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblPre8
            // 
            this.lblPre8.BackColor = System.Drawing.SystemColors.Window;
            this.lblPre8.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblPre8.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPre8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.lblPre8.Location = new System.Drawing.Point(18, 169);
            this.lblPre8.Name = "lblPre8";
            this.lblPre8.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblPre8.Size = new System.Drawing.Size(73, 14);
            this.lblPre8.TabIndex = 3;
            this.lblPre8.Text = "Trigger -8";
            // 
            // _lblPostTrig_2
            // 
            this._lblPostTrig_2.BackColor = System.Drawing.SystemColors.Window;
            this._lblPostTrig_2.Cursor = System.Windows.Forms.Cursors.Default;
            this._lblPostTrig_2.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblPostTrig_2.ForeColor = System.Drawing.Color.Blue;
            this._lblPostTrig_2.Location = new System.Drawing.Point(277, 156);
            this._lblPostTrig_2.Name = "_lblPostTrig_2";
            this._lblPostTrig_2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._lblPostTrig_2.Size = new System.Drawing.Size(65, 14);
            this._lblPostTrig_2.TabIndex = 28;
            this._lblPostTrig_2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblPost2
            // 
            this.lblPost2.BackColor = System.Drawing.SystemColors.Window;
            this.lblPost2.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblPost2.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPost2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.lblPost2.Location = new System.Drawing.Point(206, 156);
            this.lblPost2.Name = "lblPost2";
            this.lblPost2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblPost2.Size = new System.Drawing.Size(73, 14);
            this.lblPost2.TabIndex = 27;
            this.lblPost2.Text = "Trigger +2";
            // 
            // _lblPreTrig_1
            // 
            this._lblPreTrig_1.BackColor = System.Drawing.SystemColors.Window;
            this._lblPreTrig_1.Cursor = System.Windows.Forms.Cursors.Default;
            this._lblPreTrig_1.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblPreTrig_1.ForeColor = System.Drawing.Color.Blue;
            this._lblPreTrig_1.Location = new System.Drawing.Point(96, 156);
            this._lblPreTrig_1.Name = "_lblPreTrig_1";
            this._lblPreTrig_1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._lblPreTrig_1.Size = new System.Drawing.Size(65, 14);
            this._lblPreTrig_1.TabIndex = 10;
            this._lblPreTrig_1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblPre9
            // 
            this.lblPre9.BackColor = System.Drawing.SystemColors.Window;
            this.lblPre9.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblPre9.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPre9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.lblPre9.Location = new System.Drawing.Point(18, 156);
            this.lblPre9.Name = "lblPre9";
            this.lblPre9.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblPre9.Size = new System.Drawing.Size(73, 14);
            this.lblPre9.TabIndex = 2;
            this.lblPre9.Text = "Trigger -9";
            // 
            // _lblPostTrig_1
            // 
            this._lblPostTrig_1.BackColor = System.Drawing.SystemColors.Window;
            this._lblPostTrig_1.Cursor = System.Windows.Forms.Cursors.Default;
            this._lblPostTrig_1.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblPostTrig_1.ForeColor = System.Drawing.Color.Blue;
            this._lblPostTrig_1.Location = new System.Drawing.Point(277, 143);
            this._lblPostTrig_1.Name = "_lblPostTrig_1";
            this._lblPostTrig_1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._lblPostTrig_1.Size = new System.Drawing.Size(65, 14);
            this._lblPostTrig_1.TabIndex = 24;
            this._lblPostTrig_1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblPost1
            // 
            this.lblPost1.BackColor = System.Drawing.SystemColors.Window;
            this.lblPost1.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblPost1.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPost1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.lblPost1.Location = new System.Drawing.Point(206, 143);
            this.lblPost1.Name = "lblPost1";
            this.lblPost1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblPost1.Size = new System.Drawing.Size(73, 14);
            this.lblPost1.TabIndex = 23;
            this.lblPost1.Text = "Trigger +1";
            // 
            // _lblPreTrig_0
            // 
            this._lblPreTrig_0.BackColor = System.Drawing.SystemColors.Window;
            this._lblPreTrig_0.Cursor = System.Windows.Forms.Cursors.Default;
            this._lblPreTrig_0.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblPreTrig_0.ForeColor = System.Drawing.Color.Blue;
            this._lblPreTrig_0.Location = new System.Drawing.Point(96, 143);
            this._lblPreTrig_0.Name = "_lblPreTrig_0";
            this._lblPreTrig_0.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._lblPreTrig_0.Size = new System.Drawing.Size(65, 14);
            this._lblPreTrig_0.TabIndex = 9;
            this._lblPreTrig_0.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblPre10
            // 
            this.lblPre10.BackColor = System.Drawing.SystemColors.Window;
            this.lblPre10.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblPre10.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPre10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.lblPre10.Location = new System.Drawing.Point(18, 143);
            this.lblPre10.Name = "lblPre10";
            this.lblPre10.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblPre10.Size = new System.Drawing.Size(73, 14);
            this.lblPre10.TabIndex = 1;
            this.lblPre10.Text = "Trigger -10";
            // 
            // lblPostTrigData
            // 
            this.lblPostTrigData.BackColor = System.Drawing.SystemColors.Window;
            this.lblPostTrigData.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblPostTrigData.Font = new System.Drawing.Font("Arial", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPostTrigData.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblPostTrigData.Location = new System.Drawing.Point(200, 124);
            this.lblPostTrigData.Name = "lblPostTrigData";
            this.lblPostTrigData.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblPostTrigData.Size = new System.Drawing.Size(172, 14);
            this.lblPostTrigData.TabIndex = 44;
            this.lblPostTrigData.Text = "Data acquired after trigger";
            // 
            // lblPreTrigData
            // 
            this.lblPreTrigData.BackColor = System.Drawing.SystemColors.Window;
            this.lblPreTrigData.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblPreTrigData.Font = new System.Drawing.Font("Arial", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPreTrigData.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblPreTrigData.Location = new System.Drawing.Point(13, 124);
            this.lblPreTrigData.Name = "lblPreTrigData";
            this.lblPreTrigData.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblPreTrigData.Size = new System.Drawing.Size(177, 14);
            this.lblPreTrigData.TabIndex = 43;
            this.lblPreTrigData.Text = "Data acquired before trigger";
            // 
            // lblDemoFunction
            // 
            this.lblDemoFunction.BackColor = System.Drawing.SystemColors.Window;
            this.lblDemoFunction.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblDemoFunction.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDemoFunction.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblDemoFunction.Location = new System.Drawing.Point(11, 6);
            this.lblDemoFunction.Name = "lblDemoFunction";
            this.lblDemoFunction.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblDemoFunction.Size = new System.Drawing.Size(380, 20);
            this.lblDemoFunction.TabIndex = 0;
            this.lblDemoFunction.Text = "Demonstration of MccDaq.MccBoard.APreTrig() in Background mode";
            this.lblDemoFunction.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblInstruction
            // 
            this.lblInstruction.BackColor = System.Drawing.SystemColors.Window;
            this.lblInstruction.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblInstruction.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInstruction.ForeColor = System.Drawing.Color.Red;
            this.lblInstruction.Location = new System.Drawing.Point(55, 34);
            this.lblInstruction.Name = "lblInstruction";
            this.lblInstruction.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblInstruction.Size = new System.Drawing.Size(292, 49);
            this.lblInstruction.TabIndex = 59;
            this.lblInstruction.Text = "Board 0 must have analog inputs that support paced acquisition.";
            this.lblInstruction.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblResult
            // 
            this.lblResult.BackColor = System.Drawing.SystemColors.Window;
            this.lblResult.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblResult.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblResult.ForeColor = System.Drawing.Color.Blue;
            this.lblResult.Location = new System.Drawing.Point(23, 313);
            this.lblResult.Name = "lblResult";
            this.lblResult.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblResult.Size = new System.Drawing.Size(271, 38);
            this.lblResult.TabIndex = 60;
            // 
            // frmPreTrig
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 13);
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(403, 363);
            this.Controls.Add(this.lblResult);
            this.Controls.Add(this.lblInstruction);
            this.Controls.Add(this.cmdQuit);
            this.Controls.Add(this.cmdTrigEnable);
            this.Controls.Add(this.lblShowCount);
            this.Controls.Add(this.lblCount);
            this.Controls.Add(this.lblShowIndex);
            this.Controls.Add(this.lblIndex);
            this.Controls.Add(this.lblShowStat);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this._lblPostTrig_10);
            this.Controls.Add(this.lblPost10);
            this.Controls.Add(this._lblPreTrig_9);
            this.Controls.Add(this.lblPre1);
            this.Controls.Add(this._lblPostTrig_9);
            this.Controls.Add(this.lblPost9);
            this.Controls.Add(this._lblPreTrig_8);
            this.Controls.Add(this.lblPre2);
            this.Controls.Add(this._lblPostTrig_8);
            this.Controls.Add(this.lblPost8);
            this.Controls.Add(this._lblPreTrig_7);
            this.Controls.Add(this.lblPre3);
            this.Controls.Add(this._lblPostTrig_7);
            this.Controls.Add(this.lblPost7);
            this.Controls.Add(this._lblPreTrig_6);
            this.Controls.Add(this.lblPre4);
            this.Controls.Add(this._lblPostTrig_6);
            this.Controls.Add(this.lblPost6);
            this.Controls.Add(this._lblPreTrig_5);
            this.Controls.Add(this.lblPre5);
            this.Controls.Add(this._lblPostTrig_5);
            this.Controls.Add(this.lblPost5);
            this.Controls.Add(this._lblPreTrig_4);
            this.Controls.Add(this.lblPre6);
            this.Controls.Add(this._lblPostTrig_4);
            this.Controls.Add(this.lblPost4);
            this.Controls.Add(this._lblPreTrig_3);
            this.Controls.Add(this.lblPre7);
            this.Controls.Add(this._lblPostTrig_3);
            this.Controls.Add(this.lblPost3);
            this.Controls.Add(this._lblPreTrig_2);
            this.Controls.Add(this.lblPre8);
            this.Controls.Add(this._lblPostTrig_2);
            this.Controls.Add(this.lblPost2);
            this.Controls.Add(this._lblPreTrig_1);
            this.Controls.Add(this.lblPre9);
            this.Controls.Add(this._lblPostTrig_1);
            this.Controls.Add(this.lblPost1);
            this.Controls.Add(this._lblPreTrig_0);
            this.Controls.Add(this.lblPre10);
            this.Controls.Add(this.lblPostTrigData);
            this.Controls.Add(this.lblPreTrigData);
            this.Controls.Add(this.lblDemoFunction);
            this.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.Blue;
            this.Location = new System.Drawing.Point(7, 103);
            this.Name = "frmPreTrig";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Universal Library Analog Input Scan";
            this.Load += new System.EventHandler(this.frmPreTrig_Load);
            this.ResumeLayout(false);

		}

	#endregion

        #region Form initialization, variables, and entry point

        /// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new frmPreTrig());
		}

        public frmPreTrig()
        {
            // This call is required by the Windows Form Designer.
            InitializeComponent();

        }

        // Form overrides dispose to clean up the component list.
        protected override void Dispose(bool Disposing)
        {
            if (Disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }

                if (MemHandle != IntPtr.Zero)
                    MccDaq.MccService.WinBufFreeEx(MemHandle);
            }
            base.Dispose(Disposing);
        }

        // Required by the Windows Form Designer
        private System.ComponentModel.IContainer components;

        public ToolTip ToolTip1;
        public Button cmdQuit;
        public Button cmdTrigEnable;
        public Timer tmrCheckStatus;
        public Label lblShowCount;
        public Label lblCount;
        public Label lblShowIndex;
        public Label lblIndex;
        public Label lblShowStat;
        public Label lblStatus;
        public Label _lblPostTrig_10;
        public Label lblPost10;
        public Label _lblPreTrig_9;
        public Label lblPre1;
        public Label _lblPostTrig_9;
        public Label lblPost9;
        public Label _lblPreTrig_8;
        public Label lblPre2;
        public Label _lblPostTrig_8;
        public Label lblPost8;
        public Label _lblPreTrig_7;
        public Label lblPre3;
        public Label _lblPostTrig_7;
        public Label lblPost7;
        public Label _lblPreTrig_6;
        public Label lblPre4;
        public Label _lblPostTrig_6;
        public Label lblPost6;
        public Label _lblPreTrig_5;
        public Label lblPre5;
        public Label _lblPostTrig_5;
        public Label lblPost5;
        public Label _lblPreTrig_4;
        public Label lblPre6;
        public Label _lblPostTrig_4;
        public Label lblPost4;
        public Label _lblPreTrig_3;
        public Label lblPre7;
        public Label _lblPostTrig_3;
        public Label lblPost3;
        public Label _lblPreTrig_2;
        public Label lblPre8;
        public Label _lblPostTrig_2;
        public Label lblPost2;
        public Label _lblPreTrig_1;
        public Label lblPre9;
        public Label _lblPostTrig_1;
        public Label lblPost1;
        public Label _lblPreTrig_0;
        public Label lblPre10;
        public Label lblPostTrigData;
        public Label lblPreTrigData;
        public Label lblDemoFunction;

        public Label[] lblPostTrig;
        public Label[] lblPreTrig;
        public Label[] lblPreSamp;
        public Label[] lblPostSamp;

        public Label lblInstruction;
        public Label lblResult; 

#endregion

    }
}