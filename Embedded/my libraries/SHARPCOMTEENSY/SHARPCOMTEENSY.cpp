#include "SHARPCOMTEENSY.h" //include the declaration for this class

const byte LED_PIN = 13; //use the LED @ Arduino pin 13

//<<constructor>>
ADVCOM::ADVCOM(usb_serial_class *serialIn, String _ID){
    pinMode(LED_PIN, OUTPUT); //make that pin an OUTPUT
	SerialLine = serialIn;
	newDataAvailable = false;
	ID = _ID;
}

//<<destructor>>
ADVCOM::~ADVCOM(){/*nothing to destruct*/}

void ADVCOM::init(int baud){
	fromPC = "";
	SerialLine->begin(baud);
	TimerA, TimerB = millis();
}

bool ADVCOM::newData(String *data){
	if(newDataAvailable){
		*data = fromPC;
		newDataAvailable = false;
		fromPC = "";
		return true;
	}
	else{
		return false;
	}
}

//blink the LED in a period equal to parameter time.
void ADVCOM::blinky(int time){
	digitalWrite(LED_PIN,HIGH); //set the pin HIGH and thus turn LED on
	delay(time/2);  //wait half of the wanted period
	digitalWrite(LED_PIN,LOW); //set the pin LOW and thus turn LED off
	delay(time/2);  //wait the last half of the wanted period
}

void ADVCOM::serialEvent()
{
	int read;
	char toadd;

	while(Serial.available() > 0)
	{
		read = Serial.read();
		if(read == 1){
			Serial.flush();
			Serial.print("POLO->");
			Serial.println(ID);
			//TimerB = millis();
			return;
		}
		else if(read == 4){
			Serial.flush();
			Serial.println("|");
			//TimerB = millis();
		}
		else if(read == 3){
			newDataAvailable = true;
			//Serial.println("~");
			return;
		}
		else{
			toadd = (char)read;
			fromPC += toadd;
			//Serial.println("~");
		}
		//Serial.println("~");
	}
	Serial.flush();
	Serial.println("~");
}