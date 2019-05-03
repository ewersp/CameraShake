## CameraShakeKit ##
###简介
Unity的可扩展，轻量级基于噪音的相机抖动管理器。

该实现生成级联的相机矩阵，其允许多个相机抖动无缝地堆叠在一起。这也允许与现有的相机行为脚本轻松集成，因为它实际上并没有修改相机的变换。

它可用于短暂的剧烈震动，微妙的环境震动以及其间的一切。可以在检查器中快速调整所有抖动属性，以实现快速迭代循环。

高性能，零gc, 可数据配置。

###特色

1. 高性能，零gc, 可数据配置。
2. 轻量级库，简易接口，容易学习并使用。

### 噪波类型 
* Sin
* Perlin

###如何使用

1. 建议使用unity2018的版本打开，工程创建的版本为unity 2018.3.6
2. 将CameraShakeManager连接到相机。
3. 创建一些CameraShake资源并根据自己的喜好编辑属性。
4. 根据需要通过CameraShakeManager播放和停止效果。

###示例截图
![Alt text](http://i.imgur.com/SYmmdND.png "Unity Editor Screenshot")

###示例gif

![Alt text](http://i.imgur.com/0RRelTb.gif "Unity Editor GIF")


---
