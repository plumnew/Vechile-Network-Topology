# Vechile-Network-Topology
A GUI for design the network topology
这是一个概念工具组件的原型
网络节点是逻辑控制器网络划分的基本单元，一个网络节点至少应该包含一个物理端口（包含收发能力的一组接口），物理端口的连接方式主要分为
1）端到端连接
2）多端连接
多端连接在形式上存在总线型，星型，环形，菊花链等，无论具体的物理连接是什么形式，逻辑上都是多端连接。
该UI提供的是逻辑上设计车辆网络的前端界面，为后端绑定和计算车辆网络特性服务。

此外在逻辑层构建完成后可以继续构建抽象层连接，例如以太网网的一个物理端口上可能存在多个应用端口，每个应用端口都有各自的可达范围等，以此为基础可以管理基于服务的通讯和基于信号矩阵的通讯，评估参数等目标。


![image](https://user-images.githubusercontent.com/26968391/194691804-4c102be5-7b25-4018-bba9-110253562121.png)

如何使用？

该组件基于Skiasharp，只需要在命名空间包含组件库，即可在xaml中使用

                <ScrollViewer  HorizontalScrollBarVisibility="Visible" VerticalScrollBarVisibility="Visible" Background="Gray">
                    <network:SkiaNetworkGraphic x:Name="NetworkGraphicView" Width="1024" Height="960"></network:SkiaNetworkGraphic>
                </ScrollViewer>
