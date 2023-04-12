# SketchURhino插件使用
## 使用场景
SketchURhino插件，全称SketchUp your Rhino，是为了方便SU使用者快速上手Rhino的所开发的一个插件。
在日常设计中，通常都会碰到这样的场景：
1、只对SU建模很熟悉，想用Rhino，却不知道如何快速上手，最终迫于项目压力，不得不放弃从零学习Rhino软件。
2、在Revit设计过程中，需要使用RhinoInsdie.Revit插件，进行Rhino和Revit交互设计时，基本都是简单方形体块推拉，没有异形工具使用场景，却不得不“杀鸡”用“牛刀”。
这样的场景时常会遇到，SketchURhino插件就是为了像SU一样快捷使用Rhino而生的。
## 插件命令
SketchURhino插件包含7组命令，图标样式仿制SketchUp的方式，让熟悉SU的使用者对这些命令的功能对一目了然。命令仿照SU可以通过按Ctrl切换同一类型命令不同功能的方式，在Rhino中可以通过左右键单击来切换同一类命令的不同功能。
 
### 1.	创建组件 | 独立组件
 
左键单击为创建组件。同SU一样，插件会自动计算所选图元的最小正交外包长方体，以左下顶点为原点，采用默认组件名创建组件（块）。省略了Rhino建块时候繁琐的设置原点、块名等步骤。

右键单击为独立组件。可以快速将所选组件（块）独立出来，成为新组件（块），方便独立编辑。这个功能是Rhino里原生没有的功能。
### 2.	铅笔 | 铅笔（OnSurface）
 
左键单击为铅笔。和原生Rhino多段线一样的绘制方式，但是新增了一个功能：当铅笔所画的多段线是闭合的时候，会自动封成面。

右键单击为面上画线。可以选择想要画线的面，在面上所画的线会最终成为面上的轮廓线，分割面域，方便后续推拉、缩放操作。
### 3.	矩形 | 矩形（OnSurface）
 
左键单击为矩形。可以通过对角线两点或通过长宽三点来绘制矩形面。

右键单击为面上画矩形。可以选择想要画矩形的面，在面上所画的矩形会分割所选的面，方便后续操作。
### 4.	圆形 | 圆形（OnSurface）
 
左键单击为圆形。原生Rhino只能绘制圆形线，想要绘制圆面还需进行封面操作。这里的圆形可以在绘制圆面，省去多余步骤。

右键单击为面上画圆形。通前面的矩形功能相似，在面上所画的圆形会分割所选的面，方便后续操作。
### 5.	移动 | 复制
 
左键单击为移动。就是原生Rhino移动命令的效果。

右键单击为复制，复刻了SU中移动命令按Ctrl的效果。在复制图元后，可通过输入“*n”或者“/n”进行阵列（n为数字）。“*n”为等距阵列，会以复制距离为单位，阵列出n个。“/n”为等分阵列，会以复制距离为总量，等分为n份进行整列。如果只输入“n”，则会修改原始复制距离，如果为正数，会沿原来方向修改，如果为负数，则会改向反方向。
### 6.	推拉 | 删除共面线
 
左键单击为推拉。仿照SU的推拉命令，可以将面推来成体，或在体上开洞。

右键单击为删除物件全部共面线，将多个同平面的合并为一个。为Rhino自带命令。
### 7.	偏移曲线 | 偏移面边线
 
左键单击为偏移曲线。为Rhino自带命令。

右键单击为偏移面边线。与SU选择面后使用偏移类似，可以偏移面边线，自动切割面，方便后续推拉等操作。
### 8.	辅助线 | 清理辅助线
 
左键单击为辅助线。仿SU辅助线的功能，可以策略距离或添加辅助线

右键单击为清理辅助线，清理全部辅助线。


