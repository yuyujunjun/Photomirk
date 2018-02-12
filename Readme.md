这是我第一个正式的游戏代码，基本结构成型后放于github中进行管理。
## DeadDestory 
用于死亡特效粒子的销毁，于产生后4s销毁
## EnemyFramework
这是AI的大脑，用于权衡判断接下来做什么，然后发给AI的身体，由AI的身体实际行动
## EnemyManager
用于判断所有敌人的存活状态，如果所有敌人都死亡，则在一定时间内销毁敌人的场景
## EnemyPhotoMirk
管理敌人的光暗属性，光暗属性是本游戏的核心玩法属性
## FortuenManager
管理敌人死亡后掉落的战利品，战利品会自动飞向玩家，是一个减速飞行的过程
## GroundAttack
地面静态的炮台的攻击，攻击分为快速和慢速，具体实现会分别给出，这是一个基类，决定何时攻击
## GroundEnemy
地面炮台敌人的基本行为，以及死亡掉落战利品之类
## GroundFastAttack
地面炮台敌人的快速攻击的具体实现
## GroundSlowAttack
地面炮台敌人的慢速攻击的具体实现
## GunTransform
管理主角的炮台移动
## HitDestory
击中任何物体都会释放击中的粒子，这是负责击中粒子的销毁
## LevelManager
场景管理，实现场景的自动加载和卸载，一共有三种场景。
1. 最基本的场景，包含简单的地形以及必要的障碍物，相对较大，同一时间只会同时加载两个这样的场景
2. 结构性的场景，包含场景中复杂的障碍物建筑装饰等，相对较小，同一时间只会同时加载两个这样的场景
3. 敌人场景，只包含敌人相关物体，在所有敌人死亡后一定时间卸载
## NotificationCenter
负责脚本之间 物体之间的通信
## ParticlesManagerZ
接受各个物体发送的消息，负责相应的粒子的产生和消失
## PlayerBrain
负责主角的虚拟属性，例如明暗属性，生命值，攻击伤害等
## PlayerBulletManager
虽然名字中包含Player，实际后期用于所有子弹的管理，场景中所有子弹的产生，子弹的消亡，子弹是否会产生伤害，子弹对谁产生伤害，子弹的飞行等。主要通过利用射线模拟子弹
## PlayerFighting
负责玩家的攻击管理，改变镜头的拉伸，管理玩家的子弹射速，子弹伤害，枪口是否过热和冷却时间，具体子弹的实施会通知子弹管理器。
## Robot
敌人的身体，接受大脑EnemyFrameWork发出的指令，具体执行/
## RoundTableLight
场景中的射灯发出的光的范围和与主角的交互
## RushingManager
管理玩家的突刺动作，例如隐身，例如渲染粒子，改变碰撞盒的大小，给定速度
## SlowBoomScript
管理慢速子弹的爆炸，会偷偷放一个球形碰撞体于慢速子弹击中位置，以模拟爆炸冲击的效果，另外会通知主角爆炸点，由主角根据与爆炸点距离判断伤害
## SlowBUlletGround
地面炮台敌人的慢速子弹管理
## ThirdPersonCharacter
接受来自ThirdPersonUserControl的控制，执行移动、突刺、二段跳，攻击等行为
## ThirdPersonUserControl
响应玩家的控制

