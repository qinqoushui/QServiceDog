﻿功能
1. 客户端守护
  1. 通过IP、端口、进程、服务状态、网址等检测手段，检测相应的程序、服务是否处于可用状态
  1. 程序、服务长时间不可用时，先停止服务或程序，再启动服务或程序
  1. 定期对服务或程序进行重启操作（先停后启）

1. 客户端上报
  1.  向云中心上报守护的状态：被守护的程序或服务的当前状态，停止或启动事件
1. 分析与告警
  1. 订阅规则管理：订阅人、事件订阅规则、事件发送方式
  1. 按规则推送事件
  1. 服务状态报告