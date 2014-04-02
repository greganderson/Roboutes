#include <SHARPCOMTEENSY.h>

ADVCOM test(&Serial, "ARM");
String dataFromPC;
String tester;

void setup(){
  test.init(9600);
}

void loop(){
  /*if(test.newData(&dataFromPC)){ //Print parsed number information EX. (N: 45)
    dataFromPC.replace("N:","");
    int parsed = dataFromPC.toInt();
    Serial.flush();
    String temp = parsed;
    String toSend = "P: "+temp;
    Serial.println(dataFromPC);
  }*/
  
  if(test.newData(&dataFromPC)){  //Print all information received
    Serial.println(dataFromPC);
  }
  
  test.serialEvent();
}

