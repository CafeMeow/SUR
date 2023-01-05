# -*- coding: utf-8 -*-
import rhinoscriptsyntax as rs

def SURCircle():
    rs.Command('_Circle')
    if rs.LastCommandResult() == 0:
        polyline = rs.LastCreatedObjects(select=False)
        if polyline:
            try:
                rs.Command('Noecho _PlanarSrf SelLast _Enter')
                rs.DeleteObjects(polyline)
            except:
                pass

if( __name__ == "__main__" ):
    SURCircle()