# -*- coding: utf-8 -*-
import rhinoscriptsyntax as rs

def SUROffset():

    rs.AddLayer("CurveOnFace",[255,127,0],False)
    
    obj_ref = rs.GetObject('请选择要偏移的面', rs.filter.surface, False, True, None, True)
    if obj_ref:
        rs.Command('_CPlane _Object ',False )
        
        ci = obj_ref.GeometryComponentIndex
        if ci.Index < 0:
            obj_id = obj_ref.Object().Id
            srf_id = obj_ref.Object().Id

            rs.Command('Dupfaceborder selid %s Enter'%srf_id,False)
            edge = rs.LastCreatedObjects()
            area_list = []
            for i in edge:
                area = rs.Area(i)
                area_list.append(area)
            index = area_list.index(max(area_list))
            ed =  edge[index]
            rs.Command('_Offset selid %s'%ed,False)
            offline = rs.LastCreatedObjects()
            if offline:
                rs.ObjectColor(offline,[255,0,0])
                       
        else:
            obj_id = obj_ref.Object().Id

            if obj_id:
                #抽离出来
                srf_id = rs.ExtractSurface(obj_id, ci.Index, copy=False)#id
                border = rs.DuplicateSurfaceBorder(srf_id, type=1,)
                
                if rs.Command('_Offset selid %s'%border[0],False):
                    off_border = rs.LastCreatedObjects()
                    polyline = off_border
                    if polyline:
                        try:
                            rs.EnableRedraw(False)
                            rs.Command('selNone splitface selid %s _Enter C selid %s _Enter'%(srf_id[0],polyline[0]),False)
                            face = rs.LastCreatedObjects(select=False)
                    
                            rs.DeleteObjects(polyline)
                            
                            rs.Command('join selid %s selid %s Enter'%(obj_id,face[0]),False)
                            rs.Command('_Noecho _CPlane w t ',False )
                            rs.EnableRedraw(True)
                        except:
                            pass
                else:
                    a = rs.JoinSurfaces([obj_id,srf_id],True)
                rs.DeleteObject(border)
    rs.Command('_Noecho _CPlane w t ' )

if( __name__ == "__main__" ):
    SUROffset()