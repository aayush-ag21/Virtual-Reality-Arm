# Virtual Reality Arm

This project was made for the Summer Internship, GAIP at National University of Singapore under NUS and Hewlett Packard Enterprise faculty. It presents an IoT solution where a arm band is created with SensorTags, Raspberry Pi and Unity to track the motion of the arm using IMU sensors and mimic the same motion on the arm of a 3D model in Unity.
This project targets helping people with partial paralysis by regular physiotherapy gamification. It also aims towards muscle memory training to learn new skills.

## Project

The architecture of the projects is as follows, two sensor tags are places at the elbow and shoulder sending their Gyrosensor data to another SensorTag over Zigbee. The reciever sensortag is connected to a RaspberryPi over USB and acts like a Zigbee Dongle. Further this aggregated data is sent from Raspberry Pi to the server over UDP which is then forwarded to Unity. Unity uses these Gyro readings to actuate the 3D model Arm.

![Live Demo](https://github.com/aayush-ag21/Virtual-Reality-Arm/blob/master/Live_demo_still.jpeg)