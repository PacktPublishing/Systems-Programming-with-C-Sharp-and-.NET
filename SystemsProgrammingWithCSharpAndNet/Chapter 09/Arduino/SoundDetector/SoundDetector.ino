
// Pin 13 is the pin to the default LED on the board
#define LedPin 13
// This pin is triggered when a sound is detected
#define SoundPin 8

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
  // put your main code here, to run repeatedly:

  int soundPinData = digitalRead(SoundPin);
  Serial.println(soundPinData);
  digitalWrite(LedPin, soundPinData);
  delay(100);

  // Blink.. just to see if we got things set up
  digitalWrite(LedPin, HIGH);
  delay(500);
  digitalWrite(LedPin, LOW);
  delay(250);
}
