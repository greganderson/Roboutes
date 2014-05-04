#include <SHARPCOMTEENSY.h>
#include <Servo.h> 

ADVCOM test(&Serial, "HAND");
String dataFromPC;
int upCommand = 127;
int rightCommand = 127;
int leftCommand = 127;

Servo upwrist;
Servo leftwrist;
Servo rightwrist;
bool newCommands  = false;

void setup(){
  test.init(9600);
  upwrist.attach(A0);
  leftwrist.attach(A1);
  rightwrist.attach(A2);
}

void loop(){
  if(test.newData(&dataFromPC)){
    newCommands = true;
    if(dataFromPC.startsWith("U:")){ //UP command data
      dataFromPC.replace("U:","");
      int parsed = dataFromPC.toInt();
      upCommand = parsed; 
      test.writeln("UP: "+(String)upCommand);
    }
    
    else if(dataFromPC.startsWith("R:")){ //UP command data
      dataFromPC.replace("R:","");
      int parsed = dataFromPC.toInt();
      rightCommand = parsed; 
      test.writeln("RIGHT: "+(String)rightCommand);
      //test.writeln("New RIGHT: "+(String)parsed);
    }
    
    else if(dataFromPC.startsWith("L:")){ //UP command data
      dataFromPC.replace("L:","");
      int parsed = dataFromPC.toInt();

      leftCommand = parsed; 
      test.writeln("LEFT: "+(String)leftCommand);
      //test.writeln("New LEFT: "+(String)parsed);
    }
  }
  //////////////////////////////////////By this point all commands should be updated (CONSIDER ADDING EMERGENCY STOP!)
  if(newCommands){
    executeCommands();
    newCommands = false;
  }
  
  
  test.serialEvent();//Always run this as the last command in the main loop()
}

void executeCommands(){
  
 upCommand = constrain(upCommand,0,100);
 
 leftCommand = constrain(leftCommand,0,100);
 //leftCommand+=50;
 
 rightCommand = constrain(rightCommand,0,100);
 //rightCommand+=50;
 
 
 /*upCommand = floatMap(upCommand,50,150,0,100);
 leftCommand = floatMap(leftCommand,50,150,0,100);
 rightCommand = floatMap(rightCommand,50,150,0,100);*/
 
 writeLA(upwrist, (100-upCommand));
 writeLA(leftwrist, (100-leftCommand));
 writeLA(rightwrist, (100-rightCommand));
}

void writeLA(Servo servo, double percentageExtended){
  double val = (percentageExtended*10)+1000;
  servo.writeMicroseconds(val);
}

float floatMap(float x, float inMin, float inMax, float outMin, float outMax){
  return (x-inMin)*(outMax-outMin)/(inMax-inMin)+outMin;
}

