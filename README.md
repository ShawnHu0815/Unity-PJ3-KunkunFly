<div align="center">
  <h1>坤坤勇敢飞 KunkunFly</h1>
  <h3>基于Unity的2D横版闯关游戏</h3>
  <img src="./docs/images/游戏演示.gif" width="300"/>
</div>

---

## 📜 项目概述  
《坤坤勇敢飞》是一款采用Unity引擎开发的2D横版闯关游戏，灵感来源于经典游戏《Flappy Bird》。项目创新性地融合了篮球机制与像素美术风格，实现了角色物理运动、动态障碍生成、跨平台适配等核心功能。通过组件化架构设计与代码优化，确保游戏在移动端和PC端的流畅运行。

---

## 🔧 技术亮点  

### 架构设计  
- **组件化开发**：采用高内聚低耦合的组件式架构，通过GameManager实现全局状态管理  
- **事件驱动机制**：通过事件系统处理碰撞检测与得分逻辑，降低模块间耦合度  
- **单例模式应用**：使用单例模式管理音频控制器及工具类，提升资源复用率  

### 核心功能实现  
- **物理系统**：基于Rigidbody2D实现角色跳跃与重力模拟，配合DOTween插件优化俯仰动画  
- **动态生成系统**：使用协程实现管道障碍物的随机生成与回收机制，结合对象池技术优化性能  
- **跨平台适配**：支持Windows、macOS、Android多平台部署，实现分辨率自适应  

### 视听体验优化  
- **动画系统**：通过Animator状态机控制角色动画过渡，结合粒子系统实现投篮特效  
- **音频管理**：构建双通道音频系统，实现背景音乐与音效的独立控制及事件触发机制  
- **UI交互**：设计完整的游戏流程界面，集成动态奖牌展示与本地化存档系统（PlayerPrefs）  

---

## 🛠️ 技术栈  
- **引擎**: Unity 2022.3  
- **编程**: C#  
- **插件**: DOTween（动画优化）、Unity Physics 2D  
- **工具链**: Git版本控制、Unity Animator、Particle System  
- **美术资源**: Aseprite像素绘制、Audacity音频处理  

---

## 🏆 项目成果  
- 完整实现10+核心功能模块，代码量超2000行  
- 在Redmi Note 10等低端设备上实现稳定运行  
- 掌握Unity物理系统、动画状态机、跨平台打包等工业化开发能力  

---

## 📥 安装与运行
### [下载地址](https://github.com/ShawnHu0815/Unity-PJ3-KunkunFly/releases) & 平台支持    
- **Windows**: 执行`KunkunFly-Windows.exe`  
- **macOS**: 运行`KunkunFly-MacOS.app`  
- **Android**: 安装`KunkunFly-Android.apk`  

---

## 📮 联系方式  
**项目开发者**: [ShawnHu](https://github.com/ShawnHu0815)  
**电子邮箱**: [xiaoyanghu18@fudan.edu.cn](mailto:xiaoyanghu18@fudan.edu.cn)  
**演示视频**: [Bilibili项目展示](https://www.bilibili.com/video/BV1cH4y1c7Jb/)  

---

<div align="center">
  <sub>本作品部分美术资源来源于网络，仅用于学习交流目的</sub>
</div>