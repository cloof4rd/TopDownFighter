## 名词

COR:
chain of responsibility
职责链模式

Content:
上下文数据

Filter:
过滤器 if(xxxx) return true;

Logic:
执行逻辑链

## 职责链的简介

调用逻辑入口, 把数据作为上下文传入不同职责逻辑中, 通过Filter过滤后,执行对应的逻辑事件

Content => Filter + Filter +... => Logic

这样做的主要原因是为了让程序代码更清晰,更容易增删改查代码(核心)!  
比如以后新增了逻辑链路只需要新构建Filter和Logic,然后使用部分以前的Filter or Logic 组合 就可以实现崭新的逻辑了

举例:

ContentA:{Name:A Age:10 Money:0}  
FilterA: return Age<5;  
FilterB: return Name=A;  
LogicA: Money += 5;  
组装逻辑就可以把以上各个东西串联起来,实现逻辑

详情请看同级目录的Example.cs文件即可 注释写的很全面

