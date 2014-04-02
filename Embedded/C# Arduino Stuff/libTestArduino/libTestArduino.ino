#include <SHARPCOMARDUINO.h>

ADVCOM test(&Serial, "ARM");
String dataFromPC;
String tester;

void setup(){
  test.init(9600);
}

void loop(){
  if(test.newData(&dataFromPC)){  //Print parsed number information EX. (N: 45)
    dataFromPC.replace("N:","");
    int parsed = dataFromPC.toInt();
    Serial.println(parsed);
  }
  
  /*if(test.newData(&dataFromPC)){  //Print all information received
    Serial.println(dataFromPC);
  }*/
  
  /*String tempString = test.newData();  //Special ONLY ARDUINO (NOT TEENSY) direct-to-string assignment method.
  if(tempString != NULL){
   Serial.println(tempString); 
  }*/
  
  test.serialEvent();
}

