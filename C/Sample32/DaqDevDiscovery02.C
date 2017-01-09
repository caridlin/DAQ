/*ULAI01.C****************************************************************

File:                         DaqDevDiscovery02.C

Library Calls Demonstrated:   cbGetNetDeviceDescriptor()
							  cbCreateDaqDevice()
							  cbReleaseDaqDevice()
							  
Purpose:                      Discovers an ethernet DAQ device and assigns 
							  board number to the detected device
							  
Demonstration:                Displays the detected DAQ device
							  and flashes the LED of the device
                              
Other Library Calls:          cbReleaseDaqDevice()
							  cbIgnoreInstaCal()
							  cbErrHandling()


Copyright (c) 1993-2002, Measurement Computing Corp.
All Rights Reserved.
***************************************************************************/

/* Include files */
#include <windows.h>
#include <stdio.h>
#include <conio.h>
#include "..\cbw.h"

/* Prototypes */
void ClearScreen (void);
void GetTextCursor (int *x, int *y);
void MoveCursor (int x, int y);

#define MAXNUMDEVS 100

void main ()
{
    /* Variable Declarations */
    int BoardNum = 0;
    int UDStat = 0;
	float    RevLevel = (float)CURRENTREVNUM;

	DaqDeviceDescriptor DeviceDescriptor;
	int Timeout = 5000;
	char Host[256];
	int Port = 54211;
	char BlinkLED;

	/* Declare UL Revision Level */
	UDStat = cbDeclareRevision(&RevLevel);


    /* Initiate error handling
       Parameters:
           PRINTALL :all warnings and errors encountered will be printed
           DONTSTOP :program will continue even if error occurs.
                     Note that STOPALL and STOPFATAL are only effective in 
                     Windows applications, not Console applications. 
   */
    cbErrHandling (PRINTALL, DONTSTOP);

	/* Ignore InstaCal device discovery */
	cbIgnoreInstaCal();

	 /* set up the screen */
    ClearScreen();
    printf ("Demonstration of cbGetNetDeviceDescriptor() and cbCreateDaqDevice()\n\n");

	printf("Please enter the host name or IP address of DAQ device: ");
	scanf_s("%s", Host, sizeof(Host));

	/* Discover an Ethernet DAQ device with cbGetNetDeviceDescriptor()
	Parameters:
            Host				: Host name or IP address of DAQ device
            Port				: Port Number
            DeviceDescriptor	: Descriptor of the dicovered device
			Timeout				: Timeout */

	UDStat = cbGetNetDeviceDescriptor(Host, Port, &DeviceDescriptor, Timeout);

	if(UDStat == NOERRORS)
	{
		printf ("\nDAQ device discovered\n");


		printf ("\nDevice Name: %s\n", DeviceDescriptor.ProductName);
		printf ("Device Identifier: %s\n", DeviceDescriptor.UniqueID);

		/* Creates a device object within the Universal Library and 
		   assign a board number to the specified DAQ device with cbCreateDaqDevice()

		Parameters:
			BoardNum			: board number to be assigned to the specified DAQ device
			DeviceDescriptor	: device descriptor of the DAQ device */

		UDStat = cbCreateDaqDevice(BoardNum, DeviceDescriptor);

		if(UDStat == NOERRORS)
		{
			fflush(stdin);

			printf ("Assigned Board Number: %d\n\n", BoardNum);

			printf ("Would you like to flash the LED of the discovered DAQ device (y/n):");
			scanf_s("%c", &BlinkLED, 1);

			if(BlinkLED == 'y' || BlinkLED == 'Y')
			{
				/* Flash the LED with cbFlashLED()

				Parameters:
				BoardNum			: board number assigned to the DAQ  */

				UDStat = cbFlashLED(BoardNum);

				Sleep(1000);

			
				/* Release resources associated with the specified board number within the Universal Library with cbReleaseDaqDevice()
				Parameters:
					BoardNum			: board number assigned to the DAQ  */
			}

			UDStat = cbReleaseDaqDevice(BoardNum);
		}
	}
	else
		printf ("Unable to discover the specified DAQ device.\n");
}


/***************************************************************************
*
* Name:      ClearScreen
* Arguments: ---
* Returns:   ---
*
* Clears the screen.
*
***************************************************************************/

#define BIOS_VIDEO   0x10

void
ClearScreen (void)
{
	COORD coordOrg = {0, 0};
	DWORD dwWritten = 0;
	HANDLE hConsole = GetStdHandle(STD_OUTPUT_HANDLE);
	if (INVALID_HANDLE_VALUE != hConsole)
		FillConsoleOutputCharacter(hConsole, ' ', 80 * 50, coordOrg, &dwWritten);

	MoveCursor(0, 0);

    return;
}


/***************************************************************************
*
* Name:      MoveCursor
* Arguments: x,y - screen coordinates of new cursor position
* Returns:   ---
*
* Positions the cursor on screen.
*
***************************************************************************/


void
MoveCursor (int x, int y)
{
	HANDLE hConsole = GetStdHandle(STD_OUTPUT_HANDLE);

	if (INVALID_HANDLE_VALUE != hConsole)
	{
		COORD coordCursor;
		coordCursor.X = (short)x;
		coordCursor.Y = (short)y;
		SetConsoleCursorPosition(hConsole, coordCursor);
	}

    return;
}


/***************************************************************************
*
* Name:      GetTextCursor
* Arguments: x,y - screen coordinates of new cursor position
* Returns:   *x and *y
*
* Returns the current (text) cursor position.
*
***************************************************************************/

void
GetTextCursor (int *x, int *y)
{
	HANDLE hConsole = GetStdHandle(STD_OUTPUT_HANDLE);
	CONSOLE_SCREEN_BUFFER_INFO csbi;

	*x = -1;
	*y = -1;
	if (INVALID_HANDLE_VALUE != hConsole)
	{
		GetConsoleScreenBufferInfo(hConsole, &csbi);
		*x = csbi.dwCursorPosition.X;
		*y = csbi.dwCursorPosition.Y;
	}

    return;
}

