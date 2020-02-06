#This code runs on raspberry Pi, collects data from SensorTag and forwards to middle man
import socket
import threading
import time
import sys
import serial

ser = serial.Serial(
port='/dev/ttyACM0',
baudrate = 115200,
parity=serial.PARITY_NONE,
stopbits=serial.STOPBITS_ONE,
bytesize=serial.EIGHTBITS,
timeout=1
)
counter=0

UDP_IP = "192.168.43.231"
UDP_PORT = 5006

MY_IP="192.168.43.7"

#print "UDP target IP:", UDP_IP
#print "UDP target port:", UDP_PORT

sock = socket.socket(socket.AF_INET, # Internet
                     socket.SOCK_DGRAM) # UDP
sock.bind((MY_IP, UDP_PORT))
flag=True
def rec_UDP():
        while flag:
                try:
                        data,addr=sock.recvfrom(1024)
                        print ("received message from pc   "+data)
                except:
                        print("broke")
                        break
listen_UDP=threading.Thread(target=rec_UDP)
listen_UDP.start()

while 1:
        try:
                x=ser.readline()
                if(x):
                        MESSAGE=x
                        print (x)
                        sock.sendto(MESSAGE, (UDP_IP, UDP_PORT))
        except KeyboardInterrupt:
                print("breaking")
                break

print("broked")
sock.close()
flag=False
sys.exit()

