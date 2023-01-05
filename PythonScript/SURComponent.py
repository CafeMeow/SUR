# -*- coding: utf-8 -*-
import rhinoscriptsyntax as rs

def SURComponet():
    objs = rs.GetObjects("请选择要创建组件的物体",filter=4|8|16|4096,preselect = True,minimum_count=1)
    if not objs: return
    
    box = rs.BoundingBox(objs)
    pt=box[0]
    block=rs.AddBlock(objs,pt,None,False)
    rs.InsertBlock(block,pt)
    print ("组件创建完成")
    rs.DeleteObjects(objs)

if( __name__ == "__main__" ):
    SURComponet()