#include <SHARPCOMTEENSY.h>


ADVCOM test(&Serial, "ARM");
String dataFromPC;
String tester;

void setup(){
  test.init(9600);
}

void loop(){
  if(test.newData(&dataFromPC)){
    dataFromPC.replace("N:","");
    int parsed = dataFromPC.toInt();
    Serial.flush();
    //Serial.print("Parsed: ");
    String temp = parsed;
    String toSend = "P: "+temp;
    Serial.println(toSend);
  }
  
  /*tester = test.newData();
  if(tester != NULL){
   Serial.flush();
   Serial.print(tester);
   //delay(50);
  }*/
  test.serialEvent();
}

