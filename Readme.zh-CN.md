# KingdomMod - 现在支持真正的双冠了！

游戏《[王国：两位君主](https://store.steampowered.com/app/701160/)》的相关功能模组。

现已支持合作和在线模式！
现在同时支持`IL2CPP`和`Mono`版本的游戏了！

- 更好的支付升级 [BetterPayableUpgrade](https://github.com/vanton/KingdomMod#betterpayableupgrade)
- 开发者工具 [DevTools](https://github.com/vanton/KingdomMod#devtools)
- 活点地图 [OverlayMap](https://github.com/vanton/KingdomMod#overlaymap)
- 坐骑耐力条 [StaminaBar](https://github.com/vanton/KingdomMod#staminabar)

## 预览

![预览](https://github.com/vanton/KingdomMod/blob/master/preview.png?raw=true)

## 支持的语言

1. [中文](https://github.com/vanton/KingdomMod/blob/master/Readme.zh-CN.md)
2. [English](https://github.com/vanton/KingdomMod/blob/master/Readme.md)

## 安装

1. 首先确定你的游戏版本是`IL2CPP`版，还是`Mono`版。
   - `IL2CPP`版，游戏目录中含有文件夹`KingdomTwoCrowns_Data\il2cpp_data`
   - `Mono`版，游戏目录中含有文件夹`KingdomTwoCrowns_Data\Managed`
2. 根据您的游戏版本，下载特定版本的模组加载器。将所有文件解压至游戏根目录，确保 `BepInEx` 文件夹和 `winhttp.dll` 等文件与游戏主程序 `KingdomTwoCrowns.exe` 处在同一目录。
   - `IL2CPP`版，下载 [BepInEx-Unity.IL2CPP-win-x64-6.0.0-be.725](https://builds.bepinex.dev/projects/bepinex_be/725/BepInEx-Unity.IL2CPP-win-x64-6.0.0-be.725%2Be1974e2.zip)
   - `Mono`版，下载 [BepInEx-Unity.Mono-win-x64-6.0.0-be.725](https://builds.bepinex.dev/projects/bepinex_be/725/BepInEx-Unity.Mono-win-x64-6.0.0-be.725%2Be1974e2.zip)
3. 根据您的游戏版本，下载特定版本的模组。从 [Releases](https://github.com/vanton/KingdomMod/releases) 下载模组文件，将所有文件解压至 `Kingdom Two Crowns\BepInEx\plugins` 目录。
   - `IL2CPP`版，下载文件名中包含`BIE6_IL2CPP`的模组。
   - `Mono`版，下载文件名中包含`BIE6_Mono`的模组。
4. 现在已完成模组的安装，启动游戏后即可自动加载模组。

## BetterPayableUpgrade

更好的支付升级。调整了游戏内部分可支付升级对象的价格和下一级的对象以及建造的时间，使其更加合理和舒适。

### 调整详情

- 缩小了硬币的体积，现在你的钱袋可以容纳更多硬币了（大约 100）。

| 等级 | 名称           | 升级价格 | 建造时间 |
| ---- | -------------- | -------- | -------- |
| 0    | 巨石桩         | 2        | -        |
| 1    | 岩石平台 -     | -        | -        |
| 2    | 木制瞭望塔     | 3        | 20       |
| 3    | 石塔           | 3        | 20       |
| 4    | 三重塔 -       | -        | -        |
| 5    | 有屋顶的三重塔 | 4        | 30       |
| 6    | 四重塔         | 4        | 30       |
| 0    | 墙             | 1        | -        |
| 1    | 墙 -           | -        | -        |
| 2    | 墙             | 2        | 20       |
| 3    | 墙             | 2        | 20       |
| 4    | 墙 -           | -        | -        |
| 5    | 墙             | 3        | 30       |
| 0    | 城堡           | 3        | 10       |
| 1    | 城堡           | 3        | 10       |
| 2    | 城堡           | 4        | 20       |
| 3    | 城堡           | 4        | 20       |
| 4    | 城堡           | 4        | 20       |
| 5    | 城堡           | 5        | 30       |
| 6    | 城堡           | 5        | 30       |
| 7    | 城堡           | 5        | 30       |
| -    | 市民之家招募   | 5 => 3   | -        |
| -    | 制作油桶       | 5 => 3   | -        |
| -    | 制作面包       | 4 => 2   | -        |
| -    | 制作剑(铁匠铺) | 8 => 2   | -        |

- `-` 表示不可用或已移除。
- 现在已支持所有模式与 DLC。

## DevTools

开发者工具模组。包含一些方便制作 MOD 的功能。

### 详情

- 部分功能严重影响游戏平衡，强烈不建议普通玩家使用该模组。
- ~~出于平衡性考虑，所有功能仅在`Debug`构建版本中可用。~~

### 热键

1. `Home` 显示调试信息
2. `End` 显示对象信息
3. `Insert` 测试一些有趣的功能
4. `X` 转储游戏对象到 JSON 文件
5. `P` 打印预设件到控制台
6. `L` 打印关卡模块到控制台
7. `Delete` 砍伐当前选中的树木
8. `F1` 添加游民
9. `F2` 添加格里芬
10. `F9` 激活游戏开发者调试工具箱 (修复: 再按一次 F9 可以关闭界面)
11. `F10` 将硬币上限设置为 1000
12. `Space` 扔出巨石

## OverlayMap

地图模组。它标记了兴趣点，以及一些额外的细节。还有人口统计功能，它可以告诉你每种类型的人口有多少。

### 功能

1. 在屏幕上方添加一个地图浮层，并在上面标识游戏内的兴趣点，包括城堡、贫民窟、码头、悬崖等。
2. 默认情况下，只显示你已探索的区域内的标识，当然你也可以通过按下`F`键立即显示完整地图（不建议）。
3. 显示一些有用的统计信息，包括闲置的村民、工人、弓箭手、农民和农田的数量。
4. 显示额外的有用信息，比如当前时间、当前小岛、宝石数和金币数等等。
5. 一些快捷功能，比如快速保存或重载游戏，而不用退出游戏。
6. 现在已探索区域的进度可以随游戏存档保存了。

### 热键

1. `M` 显示地图浮层
2. `F` 显示完整地图（不建议使用，会降低游戏乐趣）
3. `F5` 重新加载已保存的游戏 (不用退出游戏): 体验电影蝴蝶效应的乐趣，直到有一天，你把 F5 错按成了 F8(> <)
4. `F8` 保存游戏 (不用退出游戏)

### 符号与颜色

| 符号或颜色            | 涵义                   |
| --------------------- | ---------------------- |
| `♜`                   | 城堡                   |
| `۩`                   | 城墙                   |
| `∧`                   | 土堆                   |
| `≈`                   | 河流                   |
| `♣`                   | 浆果丛                 |
| $\color{Cyan}{★}$     | 隐士                   |
| $\color{red}{红色}$   | 未解锁，未攻陷，已损坏 |
| $\color{blue}{蓝色}$  | 建造中                 |
| $\color{green}{绿色}$ | 已解锁，安全的         |

### 自定义风格和语言

现在地图模组已支持自定义风格和语言，您可以在游戏目录`Kingdom Two Crowns\BepInEx\config`找到它们的配置文件。

- `KingdomMod.OverlayMap.cfg`，配置当前使用的语言和风格文件名，
- `KingdomMod.OverlayMap.Style.cfg`，地图模组的风格文件。您可以修改其中的文本颜色及标记符号。
- `KingdomMod.OverlayMap.Language.en-US.cfg`，地图模组的语言文件。您可以以该文件为模板制作您的母语语言文件。复制该文件，将文件名的`en-US`部分改为您自己国家/地区的语言代码，比如`KingdomMod.OverlayMap.Language.ru-RU.cfg`，然后对文件内的字符串进行本地化翻译。
- 欢迎分享您的语言和风格文件：[分享入口](https://github.com/abevol/KingdomMod/issues/3)。

## StaminaBar

耐力条模组。它通过绘制可视化的图形来直观展示坐骑的耐力值。

### 详情

- 蓝色部分表示耐力值，跑步会消耗耐力值，行走和站立会恢复耐力值，站立时恢复耐力值的速度比行走时快很多。
- 不同坐骑的跑步消耗和奔跑速度不同，该模组通过耐力条的长度来体现它们。
- 黄色部分表示完全吃饱状态的持续时间，在该状态下行动不会消耗耐力值。
- 不同坐骑的完全吃饱状态的持续时间不同，该模组通过黄色部分的长度来体现它。
- 即使不行动，完全吃饱状态的持续时间也会随着时间逐渐减少。
- 红色部分表示疲劳状态的持续时间，在该状态下不能跑步。
- 不同坐骑的疲劳状态的持续时间不同。

### 热键

1. `N` 显示/隐藏耐力条
