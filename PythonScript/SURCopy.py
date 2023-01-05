# -*- coding: utf-8 -*-
import rhinoscriptsyntax as rs



def SURCopy():

    def GetObjs():
        global _objectids
        _objectids = rs.GetObjects('select objects',0,False,True,minimum_count = 1)
        if _objectids:
            global _start
            _start = rs.GetPoint('set the start point')
            if _start:
                end = rs.GetPoint('set the end point', _start)
                if end:
                    global _translation
                    _translation = end-_start
                    global _copyobjectids
                    _copyobjectids= rs.CopyObjects( _objectids, _translation )
                    # global _gapLength
                    # _gapLength = rs.Distance(_start , end)
                    return _copyobjectids


    def is_number(s):  
        try:    # 如果能运⾏ float(s) 语句，返回 True（字符串 s 是浮点数）        
            float(s)        
            return True    
        except ValueError:  # ValueError 为 Python 的⼀种标准异常，表⽰"传⼊⽆效的参数"        
            pass  # 如果引发了 ValueError 这种异常，不做任何事情（pass：不做任何事情，⼀般⽤做占位语句）    
        try:        
            import unicodedata  # 处理 ASCII 码的包        
            unicodedata.numeric(s)  # 把⼀个表⽰数字的字符串转换为浮点数返回的函数        
            return True    
        except (TypeError, ValueError):        
            pass    
            return False

    def defaultStr():
        global _mutliBool
        global _translation
        global _gapnum
        if _mutliBool:
            mutli= " * "
        else:
            mutli= " / "
        wordStr = str(rs.VectorLength(_translation))+mutli+str(_gapnum)
        return wordStr
        
    def GetKeyWord():
        # global _gapLength
        global _mutliBool
        getstr = rs.GetString('Distance' ,defaultStr())
        # getstr = rs.GetString('Distance')
        if ((getstr == None) or (getstr == "")):
            return #end
        elif is_number(getstr):
            gap = float(getstr)
            copyTo(gap)
        elif (getstr[0] == "*" and is_number(getstr[1:]))or(getstr[-1] == "*" and is_number(getstr[0:-1])):
            intnum = getstr.strip("*")
            _mutliBool = True
            num = int(intnum)
            if num ==0:return
            arrayTo(num)
        elif (getstr[0] == "/" and is_number(getstr[1:]))or(getstr[-1] == "/" and is_number(getstr[0:-1])):
            intnum = getstr.strip("/")
            _mutliBool = False
            num = int(intnum)
            if num ==0:return
            arrayTo(num)
        else:
            return #end
    
    def ArrayObj():
        # global _translation
        global _copyobjectids
        global _mutliBool
        global _gapnum
        trans=[]
        if _mutliBool:
            for i in range(_gapnum):
                nexttran = rs.VectorScale(_translation,(i+1))
                trans.append(nexttran)
        else:
            unitgap = rs.VectorDivide(_translation, _gapnum)
            for i in range(_gapnum):
                nexttran = rs.VectorScale(unitgap,(i+1))
                trans.append(nexttran)
        if trans:
            rs.DeleteObjects(_copyobjectids)
            _copyobjectids=[]
            for i in trans:
                xform = rs.XformTranslation(i)
                newobj= rs.TransformObjects(_objectids, xform, True)
                _copyobjectids.extend(newobj)
        return _copyobjectids
 
    def copyTo(flot):
        global _gapLength
        global _translation
        if flot < 0:
            _gapLength = abs(flot)
            _translation =rs.VectorScale( rs.VectorUnitize(rs.VectorReverse(_translation)) , _gapLength)
            ArrayObj()
        else:
            _gapLength = abs(flot)
            _translation =rs.VectorScale( rs.VectorUnitize(_translation) , _gapLength)
            ArrayObj()
        GetKeyWord()
    
    def arrayTo(n):
        global _gapnum
        _gapnum = n
        ArrayObj()
        GetKeyWord()
    
    global _gapnum
    global _mutliBool
    _gapnum=1
    _mutliBool = True    
    if GetObjs():
        GetKeyWord()


if( __name__ == "__main__" ):
    SURCopy()