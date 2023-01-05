# -*- coding: utf-8 -*-
import rhinoscriptsyntax as rs

def SURPencil():
    rs.Command('polyline')
    if rs.LastCommandResult() == 0:
        polyline = rs.LastCreatedObjects(select=False)
        if polyline and rs.IsCurveClosed(polyline) and rs.IsCurveInPlane(polyline):
            try:
                rs.Command('Noecho _PlanarSrf SelLast _Enter')
                rs.DeleteObjects(polyline)
            except:
                pass
        else:
            if not rs.IsCurveClosed(polyline):
                print('创建了非闭合曲线')
            elif not rs.IsCurveInPlane(polyline):
                print('创建了非共面曲线')
if( __name__ == "__main__" ):
    SURPencil()