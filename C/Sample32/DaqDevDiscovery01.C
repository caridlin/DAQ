/*ULAI01.C****************************************************************

File:                         DaqDevDiscovery01.C

Library Calls Demonstrated:   cbGetDaqDeviceInventory()
							  cbCreateDaqDevice()
							  cbReleaseDaqDevice()
							  
Purpose:                      Discovers DAQ devices and assigns 
							  board number to the detected devices
							  
Demonstration:                Displays the detected DAQ devices
							  and flashes the LED of the selected device
                              
Other Library Calls:          cbIgnoreInstaCal()
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

	int numberOfDevices = MAXNUMDEVS;
	DaqDeviceDescriptor inventory[MAXNUMDEVS];

	DaqDeviceDescriptor DeviceDescriptor;

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
    printf ("Demonstration of cbGetDaqDeviceInventory() and cbCreateDaqDevice()\n\n");

	printf ("Press ENTER to Discover DAQ devices\n\n");
	getch();

	/* Discover DAQ devices with cbGetDaqDeviceInventory()
	Parameters:
            InterfaceType   :interface type of DAQ devices to be discovered
            inventory[]		:array for the discovered DAQ devices
            numberOfDevices	:number of DAQ devices discovered */

	UDStat = cbGetDaqDeviceInventory(ANY_IFC, inventory, &numberOfDevices);

	if(numberOfDevices > 0)
	{
		printf ("Discovered %d DAQ device(s).\n", numberOfDevices);

		for(BoardNum = 0; BoardNum < numberOfDevices; BoardNum++)
		{
			DeviceDescriptor = inventory[BoardNum];

			printf ("\nDevice Name: %s\n", DeviceDescriptor.ProductName);
			printf ("Device Identifier: %s\n", DeviceDescriptor.UniqueID);
			printf ("Assigned Board Number: %d\n\n", BoardNum);

			/* Creates a device object within the Universal Library and 
			   assign a board number to the specified DAQ device with cbCreateDaqDevice()

			Parameters:
				BoardNum			: board number to be assigned to the specified DAQ device
				DeviceDescriptor	: device descriptor of the DAQ device */

			UDStat = cbCreateDaqDevice(BoardNum, DeviceDescriptor);
		}

		/* Flash the LED of the selected device */

		printf ("Select a DAQ device from the discovered devices above to flash the LED\n\n");
		while (1)
        {
			// select a DAQ from the discovered devices above
			printf ("Enter a board number or (-1) to exit:");
			scanf_s("%i", &BoardNum);

			if(BoardNum >=0 && BoardNum < numberOfDevices)
			{
				/* Flash the LED of the specified DAQ device with cbFlashLED()

				Parameters:
				BoardNum			: board number assigned to the DAQ  */

				UDStat = cbFlashLED(BoardNum);
			}
			else
			{
				if(BoardNum == -1)
					break;
				else
					printf ("Invalid device number\n");
			}
        }

		for(BoardNum = 0; BoardNum < numberOfDevices; BoardNum++)
		{
			/* Release resources associated with the specified board number within the Universal Library with cbReleaseDaqDevice()
			Parameters:
				BoardNum			: board number assigned to the DAQ  */

			UDStat = cbReleaseDaqDevice(BoardNum);
		}
	}
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

