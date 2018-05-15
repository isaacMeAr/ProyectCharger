int motp11 =7;
int motp12= 6;
int motp21= 5;
int motp22=4;

void setup() {
  pinMode (motp11, OUTPUT);
  pinMode (motp12, OUTPUT);
  pinMode (motp21, OUTPUT);
  pinMode (motp22, OUTPUT);
  Serial.begin(9600);
}

void loop() {

   char inChar = (char)Serial.read();


  while(Serial.available()){
  
    if (inChar == 'W') {
    digitalWrite(motp11, 1);
    digitalWrite(motp12, 0);
    digitalWrite(motp21, 1);
    digitalWrite(motp22, 0);
    
    }

     if (inChar == 'D') {
    digitalWrite(motp11, 1);
    digitalWrite(motp12, 0);
    digitalWrite(motp21, 0);
    digitalWrite(motp22, 1);
  
    }

     if (inChar == 'A') {
    digitalWrite(motp11, 0);
    digitalWrite(motp12, 1);
    digitalWrite(motp21, 1);
    digitalWrite(motp22, 0);
    
    }

     if (inChar == 'S') {
    digitalWrite(motp11, 0);
    digitalWrite(motp12, 1);
    digitalWrite(motp21, 0);
    digitalWrite(motp22, 1);
    
    }

    if (inChar == 'X') {
    digitalWrite(motp11, 0);
    digitalWrite(motp12, 0);
    digitalWrite(motp21, 0);
    digitalWrite(motp22, 0);
    
    }
  }
}

void serialEvent()
{
 
   
}
  

