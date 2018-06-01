int motp11 =9;
int motp12= 8;
int motp21= 11;
int motp22=10;

void setup() {
  pinMode (motp11, OUTPUT);
  pinMode (motp12, OUTPUT);
  pinMode (motp21, OUTPUT);
  pinMode (motp22, OUTPUT);
  Serial.begin(9600);
}

void loop() {


  while(Serial.available()){
    
    char inChar = (char)Serial.read();

    if (inChar == 'W') {
    digitalWrite(motp11, 1);
    digitalWrite(motp12, 0);
    digitalWrite(motp21, 1);
    digitalWrite(motp22, 0);
    
    }

     if (inChar == 'A') {
    digitalWrite(motp11, 1);
    digitalWrite(motp12, 0);
    digitalWrite(motp21, 0);
    digitalWrite(motp22, 1);
    delay(750);
    digitalWrite(motp11, 0);
    digitalWrite(motp12, 0);
    digitalWrite(motp21, 0);
    digitalWrite(motp22, 0);
  
    }

     if (inChar == 'D') {
    digitalWrite(motp11, 0);
    digitalWrite(motp12, 1);
    digitalWrite(motp21, 1);
    digitalWrite(motp22, 0);
    delay(750);
    digitalWrite(motp11, 0);
    digitalWrite(motp12, 0);
    digitalWrite(motp21, 0);
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

  

