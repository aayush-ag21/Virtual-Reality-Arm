# This script acts as a middle man between raspberry Pi and Unity server.
import socket
import time
import threading

UNITY_IP = "127.0.0.1"
RASPI_IP = '192.168.43.7'
MY_IP= '192.168.43.231'

RELAY_EXTERNAL = 5006
RASPI_PORT = 5006

UNITY_PORT = 5005
RELAY_INTERNAL=5004

UNITY  = (UNITY_IP, UNITY_PORT)
RASPI = (RASPI_IP,RASPI_PORT)

sock = socket.socket(family=socket.AF_INET, type=socket.SOCK_DGRAM)
sock.bind((MY_IP,RELAY_EXTERNAL))

sockUnity = socket.socket(family=socket.AF_INET, type=socket.SOCK_DGRAM) 
sockUnity.bind((UNITY_IP,RELAY_INTERNAL))   

def rec_UDP():
    while True:
        try:
            data,addr=sock.recvfrom(1024)
            print (f"received message from raspi{data}")
            sockUnity.sendto(data,UNITY)
            print("Sending to unity")
        except:
            break
listen_UDP=threading.Thread(target=rec_UDP)
listen_UDP.start()

def rec_Unity():
    while True:
        try:
            data,addr=sockUnity.recvfrom(1024)
            print (f"received message from unity {data}")
            sock.sendto(data,RASPI)
            print("Send to raspi")
        except:
            break
listen_UNITY=threading.Thread(target=rec_Unity)
listen_UNITY.start()


while True:
    try:
        time.sleep(1)
    except KeyboardInterrupt:
        sock.close()
        sockUnity.close()
        break