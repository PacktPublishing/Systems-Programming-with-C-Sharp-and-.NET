
// Pin 13 is the pin to the default LED on the board
// We could have used the predefined LED_BUILTIN, which
// is the same. I just wanted to show this as well.
// And of course, now you can attach an external
// LED to the board, if you want.
#define LedPin 13
// This pin is triggered when a sound is detected
#define SoundPin 8

int _prevResult = LOW;

void setup() {
  // This runs once, at the startup

  // Set the direction for the LED to OUTPUT: 
  // We are writing to this.
  pinMode(LedPin, OUTPUT);

  // Set the direction for the sound detector to INPUT:
  // We are reading from this.
  pinMode(SoundPin, INPUT);

  Serial.begin(9600);
}

void loop() {

  // This code runs all the time...
  // Read the results from the detector.
  int soundPinData = digitalRead(SoundPin);
  // Detect a change...
  if(soundPinData != _prevResult){
      // There is a change. Let's notify Windows
    _prevResult = soundPinData;
    if(soundPinData == HIGH)
    {
      Serial.print();
      digitalWrite(LED_BUILTIN, HIGH);
    }    
    else{
      Serial.println("We do not hear anything anymore...");
      digitalWrite(LED_BUILTIN, LOW);
    }
    delay(100);
  }  

}
