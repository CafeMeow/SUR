# -*- coding: utf-8 -*-
import rhinoscriptsyntax as rs
import scriptcontext as sc
def SURPencilOnSurface():
    rs.Command('_Noecho _CPlane w t ' )
    message = "请选择要绘制的面"
    obj_ref = rs.GetObject(message, rs.filter.surface, False, False, None, True)
    if obj_ref != None:
        
        ci = obj_ref.GeometryComponentIndex
    
        #独立的面
        if ci.Index < 0:
            obj_id = obj_ref.Object().Id
            rs.Command('Noecho _CPlane _Object SelID %s'%obj_id)
            if sc.escape_test(False):
                rs.Command('_Noecho _CPlane w t ' )
            rs.Command('polyline')
            rs.EnableRedraw(False)
            polyline = rs.LastCreatedObjects(select=False)#内存地址
            if polyline:
                try:
                    rs.Command('selNone splitface selid %s _Enter C selid %s _Enter'%(obj_id,polyline[0]))
                    rs.DeleteObject(polyline)
                    rs.Command('_Noecho _CPlane w t ' )
                    rs.EnableRedraw(True)
                except:
                    pass
        else:
            #多重曲面
            obj_id = obj_ref.Object().Id
            if obj_id:
                #抽离出来
                srf_id = rs.ExtractSurface(obj_id, ci.Index, copy=False)#id
                
                srf = rs.coercebrep(srf_id)
        
                rs.Command('Noecho _CPlane _Object SelID %s'%srf_id[0] )
                if sc.escape_test(False):
                    rs.Command('_Noecho _CPlane w t ' )
                rs.Command('polyline')
                polyline = rs.LastCreatedObjects(select=False)#内存地址
                if polyline:
                    try:
                        rs.EnableRedraw(False)
                        rs.Command('selNone splitface selid %s _Enter C selid %s _Enter'%(srf_id[0],polyline[0]))
                        face = rs.LastCreatedObjects(select=False)
                
                        rs.DeleteObjects(polyline)
                        rs.Command('join selid %s selid %s Enter'%(obj_id,face[0]))
                        rs.Command('_Noecho _CPlane w t ' )
                        rs.EnableRedraw(True)
                    except:
                        pass
if( __name__ == "__main__" ):
    SURPencilOnSurface()