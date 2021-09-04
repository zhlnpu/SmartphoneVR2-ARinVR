# SmartPhoneVR
 This project is  a revised versoin of the  ["VRPhone"](https://github.com/zhlnpu/SmartphoneVR2) project, which is fcused on bringing AR into VR.

本项目成果为SA‘21 Poster


## 用法：
1. 后台通信服务器必须打开
2. 手机AR只提供显示，3D定位追踪由手柄提供，实测传输手机位置到VR中延迟太大，手机位置对不上
3. VR虚拟模型初始位置由由手柄上挂在的平面确定，运行后先确定虚拟模型的位置，手柄放到码的中心位置，之后拖动plan脱离手柄，再把手柄拿起来和手机绑定
4. 手机射线位置基于相机位置偏移，与用手柄定位的手机位置对不上
5. grabble的尺寸保持为1，调整模型大小时直接设置单个模型，否则传入的模型位置出错




## Configuration

| What |  Description |
|--|--|
|Platform| Win 10|
| Unity| 2019.3.1f1|
|Headset| Quest 2|
| Camera |SR300
|Android SDK | >API 28
|Gradle| [6.5 ](https\://services.gradle.org/distributions/gradle-6.5-all.zip) |
|NDK| 21|
| JDK| jdk1.8.0_271, or so|
|librealsense| [2.44](https://github.com/IntelRealSense/librealsense/tree/v2.44.0)|
| Oculus Integration |  V28
|Phone| Huawei Mate 20|

![Representative2-01 - Copy](https://user-images.githubusercontent.com/55628470/119116797-2fb34480-ba5b-11eb-9f0e-fc3ca779bbbb.jpg)


## Settings on screen casting
1. Phone
Connect to the same local network as the PC, and get the IP address.

2. Scrcpy
Set the screen to landscape to allow a larger resolution rendered on the screen.
```
.\adb.exe devices
.\adb.exe tcpip 5554
.\adb.exe connect 192.168.3.125:5554
.\adb.exe devices
.\scrcpy.exe -s 192.168.3.125:5554  --lock-video-orientation 1 -b 80M  --window-title "AndroidPhoneScreen" --window-width 1920
```
3. OBS
- Install obs [virtual camera](https://obsproject.com/forum/resources/obs-virtualcam.539/)
- Set input and output resolution (stop virtual camera first)
![image](https://user-images.githubusercontent.com/55628470/119802164-e8243100-bf10-11eb-9d20-4ec26f6e6971.png)

The result should look like:
![image](https://user-images.githubusercontent.com/55628470/119802332-0c800d80-bf11-11eb-8f3c-827ae4d9ea9a.png)

- Then rotate the window in OBS:
![image](https://user-images.githubusercontent.com/55628470/119802447-1e61b080-bf11-11eb-8548-5777c38ebb35.png)

4. Unity 3D
Selece "OBS virtual camera"





# Issues

### HMD detection incorrect
in "OVRControllerHelper.cv"
'''
void Start()
	{
		OVRPlugin.SystemHeadset headset = OVRPlugin.GetSystemHeadsetType();

		switch (headset)
		{
			case OVRPlugin.SystemHeadset.Rift_CV1:
				activeControllerType = ControllerType.Rift;
				break;
			case OVRPlugin.SystemHeadset.Oculus_Quest_2:

				activeControllerType = ControllerType.Quest2;
				break;

			case OVRPlugin.SystemHeadset.Oculus_Link_Quest_2:

				activeControllerType = ControllerType.Quest2;
				break;
			default:
				activeControllerType = ControllerType.QuestAndRiftS;
				break;
		}

'''








 
