# -*- coding: utf-8 -*-
import rhinoscriptsyntax as rs
def SURPush():
    rs.Command('''! _ExtrudeSrf
_Pause
_Solid=_Yes
_DeleteInput=_Yes
_pause
_Setredrawoff
_SelLast
_MergeAllFaces
_selnone
_Setredrawon''')

if( __name__ == "__main__" ):
    SURPush()