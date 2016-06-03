int inByte;

// the setup function runs once when you press reset or power the board
void setup() 
{
	// initialize digital pin 13 as an output.
	pinMode(13, OUTPUT);
	Serial.begin(9600);

	while (!Serial) 
	{
	    ; // wait for serial port to connect. Needed for native USB port only 
	}
}

// the loop function runs over and over again forever
void loop() 
{
	// if we get a valid byte, read analog ins:	  
	if (Serial.available() > 0) 
	{
		// get incoming byte:
		inByte = Serial.read();
		    
		if(inByte == '1')
		{  
			digitalWrite(13, HIGH);   
			// turn the LED on (HIGH is the voltage level)
			      
			delay(1000);              
			// wait for a second
		}
		else
		{     
			digitalWrite(13, LOW);    
			// turn the LED off by making the voltage LOW
			      
			delay(1000);              
			// wait for a second
		}
	}
	
	//ovdje cemo poslati neke podatke da imamo komunikaciju sa 
	//arduina na serijski port
	delay(2000);              // wait for a second
	// send sensor values:
  	Serial.write("Evo ti malo podataka od mene :D");
}
