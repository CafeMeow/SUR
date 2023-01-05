# -*- coding: utf-8 -*-
import rhinoscriptsyntax as rs


command = '_Circle'

def SURCircleOnSurface():    
    rs.AddLayer("CurveOnFace",[255,127,0],False)
    #面的id
    obj_ref = rs.GetObject('请拾取绘制的面', rs.filter.surface, False, True, None, True)
    
    if obj_ref:
        rs.Command('Noecho _CPlane _Object ' )
        if rs.ObjectType(obj_ref) == 4096:
            rs.Command('_Noecho _CPlane w t ' )
            rs.MessageBox('不支持直接对图块进行操作，需进入图块编辑模式')
            return
        else:
            ci = obj_ref.GeometryComponentIndex
            #单一曲面
            if ci.Index < 0:
                srf_id = obj_ref.Object().Id

                rs.Command(command )
                rs.EnableRedraw(False)
                
                line = rs.LastCreatedObjects(select=False) 
                if line :
                    if rs.IsCurveClosed(line):
                        rs.Command('Noecho _splitface selid %s _Enter C selid %s _Enter'%(srf_id, line[0]))
                        rs.ObjectLayer(line,'CurveOnFace')
                    else:
                        pass
                        
                rs.EnableRedraw(True)
            else:
                #这个是物体的ID
                obj_id = obj_ref.Object().Id
                if obj_id:
                    rs.Command(command)
                    
                    line =  rs.LastCreatedObjects(select = False)
                    if line:
                        rs.Command('_Noecho _CPlane _Undo' )
                        rs.EnableRedraw(False)
                        if rs.IsCurveClosed(line):
                            if rs.Command('Noecho _split selid %s _Enter selid %s _Enter'%(obj_id, line[0])):
                                spface = rs.LastCreatedObjects(select = True)
                                if spface:
                                    rs.Command('Noecho join')                                    
                                    rs.ObjectLayer(line,'CurveOnFace')
                        else:
                            pass
                        rs.EnableRedraw(True)
                        rs.Redraw()

        rs.UnselectAllObjects()

if( __name__ == "__main__" ):
    SURCircleOnSurface()