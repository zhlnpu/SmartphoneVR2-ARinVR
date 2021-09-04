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








 
