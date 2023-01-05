# -*- coding: utf-8 -*- 
import rhinoscriptsyntax as rs

def SURExtrudeSurface():
    rs.Command('''! _ExtrudeSrf _Pause _DeleteInput=_Yes _Pause
''')

if( __name__ == "__main__" ):
    SURExtrudeSurface()