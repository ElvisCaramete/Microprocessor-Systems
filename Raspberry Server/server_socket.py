#!/usr/bin/python

import RPi.GPIO as GPIO
import socket
import sys
import Adafruit_DHT
import time


backlog=1
size = 1024
s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
s.bind(("168.154.1.1",50001))
s.listen(backlog)

humidity, temperature = Adafruit_DHT.read_retry(11, 4)
def callback(channel):
        if GPIO.input(channel):
                print "Low soil moisture"

        else:
                print "Good soil moisture"


# Set our GPIO numbering to BCM
GPIO.setmode(GPIO.BCM)

# Define the GPIO pin that we have our digital output from our sensor connected                                                                                                              to
channel = 17
# Set the GPIO pin to an input
GPIO.setup(channel, GPIO.IN)

# This line tells our script to keep an eye on our gpio pin and let us know when                                                                                                              the pin goes HIGH or LOW
GPIO.add_event_detect(channel, GPIO.BOTH, bouncetime=300)
# This line asigns a function to the GPIO pin so that when the above line tells                                                                                                              us there is a change on the pin, run this function

try:
    print ( "is waiting")
    client, address = s.accept()
    print "Connected"
    while 1:
        print humidity
        print temperature
        print "Wating for command"
        GPIO.add_event_callback(channel, callback)
        data = client.recv(size)

        if data:
            tabela = data
            if tabela == "PumpON":
                print "Turning the water pump ON"
                print "Please wait to read the next values from sensors"
            if tabela == "PumpOFF":
                print "Turning the water pump OFF"
                print "Please wait to read the next values from sensors"
            if tabela == "LedON":
                print "Turning the cooler ON"
                print "Please wait to read the next values from sensors"
            if tabela == "LedOFF":
                print "Turning the cooler OFF"
                print "Please wait to read the next values from sensors"
        humidity, temperature = Adafruit_DHT.read_retry(11, 4)

except:
    print("closing socket")
    client.close()
    s.close()