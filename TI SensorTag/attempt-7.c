/*---------------------------------------------------------------------------*/
#include "contiki.h"
#include "board-peripherals.h"
#include <stdio.h>
#include "net/rime/rime.h"
#include <stdio.h>
/*---------------------------------------------------------------------------*/
#define INTERVAL 0.02
#define INTERVAL1 0.1
#define INTERVAL2 0.025
#define CC26XX_ACCELEROMETER_LOOP_INTERVAL ((CLOCK_SECOND * INTERVAL)) 	//								1 => 1Hz; 0.1 => 10 to 11 Hz;	0.05 =>	21 Hz; 		0.025 =>  43 Hz			0.01 => 121 Hz
#define CC26XX_GYROSCOPE_LOOP_INTERVAL ((CLOCK_SECOND * INTERVAL)) 		//								1 => 1Hz; 0.1 => 10 Hz;			0.05 => 20Hz;		0.025 =>  43 Hz  		0.01 => 121 Hz
#define CC26XX_GYROSCOPE_LOOP_INTERVAL1 ((CLOCK_SECOND * INTERVAL1))
#define CC26XX_GYROSCOPE_LOOP_INTERVAL2 ((CLOCK_SECOND * INTERVAL2))  
#define ACCELEROMETER 0
#define GYROSCOPE 1
int s1 =0;
int s2= 0;
int s3 =0;
int flag =0;
int count1 = 0;
int x1,y1,z1;
/*---------------------------------------------------------------------------*/
static struct etimer et_accelerometer, et_gyroscope;
static int accelerometer_activated=0, gyroscope_activated=0;
static char message[50];

static void send(char message[], int size);

PROCESS(sensor_demo_process, "sensing at different rates and sending...");
AUTOSTART_PROCESSES(&sensor_demo_process);
/*
static const struct unicast_callbacks unicast_callbacks = {};
static struct unicast_conn uc;
*/
static int count=2;
//----------------------------------------------------------
static void recv_uc(struct unicast_conn *c, const linkaddr_t *from){
  char messageR[50];
  strcpy(messageR,(char *)packetbuf_dataptr());
  messageR[packetbuf_datalen()]='\0';
  printf("%s", messageR);
}

static const struct unicast_callbacks unicast_callbacks = {recv_uc};
static struct unicast_conn uc;
//--------------------------------------------------------------------
static void send(char message[], int size){
	linkaddr_t addr;
	char final_message[60];
	sprintf(final_message,"%d \0", count);
	strcat(final_message, message);
	printf("%s\n",final_message);
    packetbuf_copyfrom(final_message, strlen(final_message));
    // COMPUTE THE ADDRESS OF THE RECEIVER FROM ITS NODE ID, FOR EXAMPLE NODEID 47620 MAPS TO 186 AND 4 RESPECTIVELY
    addr.u8[0] = 0x26; 	// HIGH BYTE
    addr.u8[1] = 0x85;		// LOW BYTE
    if(!linkaddr_cmp(&addr, &linkaddr_node_addr)) {
      unicast_send(&uc, &addr);
    }

}

PROCESS_THREAD(sensor_demo_process, ev, data){

	PROCESS_EXITHANDLER(unicast_close(&uc);)
	PROCESS_BEGIN();
	unicast_open(&uc, 146, &unicast_callbacks);

	// ENABLE TIMER FOR THE DIFFERENT SENSORS
	if(ACCELEROMETER){
		etimer_set(&et_accelerometer, CC26XX_ACCELEROMETER_LOOP_INTERVAL);
	}

	if(GYROSCOPE){
		etimer_set(&et_gyroscope, CC26XX_GYROSCOPE_LOOP_INTERVAL);
	}
	x1= (mpu_9250_sensor.value(MPU_9250_SENSOR_TYPE_GYRO_X));
	y1= (mpu_9250_sensor.value(MPU_9250_SENSOR_TYPE_GYRO_Y));
	z1= (mpu_9250_sensor.value(MPU_9250_SENSOR_TYPE_GYRO_Z));


	while(1) {

		PROCESS_WAIT_EVENT();
		int x,y,z;
		char str[10];
		// TIMER EVENTS
		if(ev == PROCESS_EVENT_TIMER) {

			if(data == &et_accelerometer) {
				if(!accelerometer_activated){
					mpu_9250_sensor.configure(SENSORS_ACTIVE, MPU_9250_SENSOR_TYPE_ALL);
					accelerometer_activated=1;
				}
				etimer_set(&et_accelerometer, CC26XX_ACCELEROMETER_LOOP_INTERVAL);
				message[0]="\0";
				x = (mpu_9250_sensor.value(MPU_9250_SENSOR_TYPE_ACC_X));
				y = (mpu_9250_sensor.value(MPU_9250_SENSOR_TYPE_ACC_Y));
				z = (mpu_9250_sensor.value(MPU_9250_SENSOR_TYPE_ACC_Z));
				sprintf(message,"accel (x100),%d %d %d \0",  x , y, z);
				send(message, strlen(message));
			}

			if(data == &et_gyroscope) {
				if(!gyroscope_activated){
					mpu_9250_sensor.configure(SENSORS_ACTIVE, MPU_9250_SENSOR_TYPE_ALL);
					gyroscope_activated=1;
				}
				

				/*if(flag == 0){
				etimer_set(&et_gyroscope, CC26XX_GYROSCOPE_LOOP_INTERVAL);
					if((((x-x1)+(y-y1)+(z-z1))/3) >= 0.5 && (((x-x1)+(y-y1)+(z-z1))/3) <= 1){
						flag = 1;}
					else if((((x-x1)+(y-y1)+(z-z1))/3) > 1){
					flag = 2;}
				}
				else if(flag == 1){
					etimer_set(&et_gyroscope, CC26XX_GYROSCOPE_LOOP_INTERVAL1);
					if((((x-x1)+(y-y1)+(z-z1))/3) < 0.5 ){
						flag = 0;}
					else if((((x-x1)+(y-y1)+(z-z1))/3) > 1){
					flag = 2;}
				}
				else(flag == 2){
					etimer_set(&et_gyroscope, CC26XX_GYROSCOPE_LOOP_INTERVAL2);
					if((((x-x1)+(y-y1)+(z-z1))/3) >= 0.5 && (((x-x1)+(y-y1)+(z-z1))/3) <= 1){
						flag = 1;}
					else if((((x-x1)+(y-y1)+(z-z1))/3) < 0.5 ){
					flag = 0;}
					
				}*/
				message[0]="\0";
				etimer_set(&et_gyroscope, CC26XX_GYROSCOPE_LOOP_INTERVAL);
				x = (mpu_9250_sensor.value(MPU_9250_SENSOR_TYPE_GYRO_X));
				y = (mpu_9250_sensor.value(MPU_9250_SENSOR_TYPE_GYRO_Y));
				z = (mpu_9250_sensor.value(MPU_9250_SENSOR_TYPE_GYRO_Z));
				s1 = s1+ x;
				s2 = s2+y;
				s3 = s3+z;
				count1++;
				if(count1>14){
				sprintf(message,"%d %d %d",  s1/30 , s2/30, s3/30);
				send(message, strlen(message));
				s1 = 0;
				s2 = 0;
				s3 = 0;
				count1=0;
				}
				x1 = x;
				y1 = y;
				z1 = z;
			}

		}

    }

	PROCESS_END();
}

/*---------------------------------------------------------------------------*/

