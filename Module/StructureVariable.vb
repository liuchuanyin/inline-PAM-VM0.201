Public Module StructureVariable


#Region "   点位的定义"
    Public Structure Pos_X
        Dim Name As String
        Dim X As String
    End Structure

    Public Structure Pos_Y
        Dim Name As String
        Dim Y As String
    End Structure

    Public Structure Pos_Z
        Dim Name As String
        Dim Z As String
    End Structure

    Public Structure Pos_XY
        Dim Name As String
        Dim X As Double
        Dim Y As Double
    End Structure

    Public Structure Pos_XYZ
        Dim Name As String
        Dim X As Double
        Dim Y As Double
        Dim Z As Double
    End Structure

    Public Structure Pos_XYZR
        Dim Name As String
        Dim X As Double
        Dim Y As Double
        Dim Z As Double
        Dim R As Double
    End Structure

    Public Structure Pos_Space
        Dim Name As String
        Dim X As Double
        Dim Y As Double
        Dim Z As Double
        Dim R As Double
        Dim X1 As Double
        Dim Y1 As Double
    End Structure

#End Region

    Public Structure Dist_XY
        Dim X As Double
        Dim Y As Double
    End Structure

    Public Structure Dist_XYZ
        Dim X As Double
        Dim Y As Double
        Dim Z As Double
    End Structure

    '针尖到CCD视野中心的距离
    'dist_NeedleToCCDCenter = CCD Capture Position - Needle Position
    Public dist_NeedleToCCDCenter(1) As Dist_XY
    'Laser中心到CCD视野中心的距离
    'dist_LaserToCCDCenter = CCD Capture Position - Laser Position
    Public dist_LaserToCCDCenter As Dist_XY

    Public Structure Machine_Parameter
        '预固化轴
        Dim St_Cure() As Pos_X
        '右料盘供料轴
        Dim St_Feed() As Pos_Z
        '精补XY轴
        Dim St_FineCompensation() As Pos_XY
        '点胶工站
        Dim St_Glue() As Pos_XYZ
        '组装工站
        Dim St_Paste() As Pos_Space
        '预取料站
        Dim St_PreTaker() As Pos_Space
        '复检站
        Dim St_Recheck() As Pos_XY
        '料盘回收轴
        Dim St_Recycle() As Pos_Z

        Dim Needle_NeedCalibration() As Boolean
        Dim Probe1_Base() As Pos_XYZ  '校针基准点
        Dim Probe1_Diff() As Dist_XYZ '校针基准差值
        '镭射偏移值
        Dim Laser_offset As Double

        Sub Init()
            ReDim St_Cure(50)
            ReDim St_Feed(50)
            ReDim St_FineCompensation(50)
            ReDim St_Glue(50)
            ReDim St_Paste(50)
            ReDim St_PreTaker(50)
            ReDim St_Recheck(50)
            ReDim St_Recycle(50)
            '两个胶针，0 针1；1 针2.
            ReDim Needle_NeedCalibration(1)
            ReDim Probe1_Base(1)
            ReDim Probe1_Diff(1)
        End Sub
    End Structure

    ''' <summary>
    ''' 点位参数
    ''' </summary>
    ''' <remarks>##AABBCC</remarks>
    Public Par_Pos As Machine_Parameter

    ''' <summary>
    '''点位参数初始化
    ''' </summary>
    ''' <remarks>##AABBCC</remarks>
    Public Sub Load_Par_Pos()
        Call Par_Pos.Init()
    End Sub

    ''' <summary>
    ''' 流水线工作状态
    ''' </summary>
    ''' <remarks></remarks>
    Public Structure Line_Status
        ''' <summary>
        ''' 是否有载具：True有载具，False无载具
        ''' </summary>
        ''' <remarks></remarks>
        Dim isHaveTray As Boolean
        ''' <summary>
        ''' 是否有正常：True正常，False有异常
        ''' </summary>
        ''' <remarks></remarks>
        Dim isNormal As Boolean
        Dim isWorking As Boolean
        ''' <summary>
        ''' 正在干什么的状态标志
        ''' </summary>
        ''' <remarks></remarks>
        Dim workState As UShort
    End Structure

    ''' <summary>
    ''' 各工站工作状态
    ''' </summary>
    ''' <remarks></remarks>
    Public Structure Station_Status
        ''' <summary>
        ''' 是否有正常：True正常，False有异常
        ''' </summary>
        ''' <remarks></remarks>
        Dim isNormal As Boolean
        ''' <summary>
        ''' 是否正在工作中
        ''' </summary>
        ''' <remarks></remarks>
        Dim isWorking As Boolean
        ''' <summary>
        ''' 正在干什么的状态标志
        ''' </summary>
        ''' <remarks></remarks>
        Dim workState As UShort
    End Structure

    Public Structure Material_Status
        ''' <summary>
        ''' 是否有载具或者料盘
        ''' </summary>
        ''' <remarks></remarks>
        Dim isHaveMaterial As Boolean
        Dim isNormal As Boolean
        Dim isWorking As Boolean
    End Structure

    Public Line_Sta(4) As Line_Status
    Public GLue_Sta As Station_Status
    Public Paste_Sta As Station_Status
    Public PreTaker_Sta As Station_Status
    Public Recheck_Sta As Station_Status
    Public Feed_Sta As Station_Status
    Public Recycle_Sta As Station_Status
    Public CamCapRemove_Sta As Station_Status

#Region "   功能：XML创建，读写操作"
    Public Sub read_Par_Pos(ByVal path As String, ByVal data As Machine_Parameter)
        Call Load_Par_Pos()

        Try
            If IO.File.Exists(path) = False Then
                Call Write_Par_Pos(path, Par_Pos)
            End If

            Dim reader As New System.Xml.Serialization.XmlSerializer(GetType(Machine_Parameter))
            Dim file As New System.IO.StreamReader(path)
            'Par_Pos为定义的共用变量，用来保存读取的信息
            Par_Pos = CType(reader.Deserialize(file), Machine_Parameter)
            file.Close()
        Catch ex As Exception
            MsgBox("点位文件读取失败:" & ex.Message)
        End Try
    End Sub

    Public Sub Write_Par_Pos(ByVal FileName As String, ByRef WriteData As Machine_Parameter)
        Try
            Dim writer As New System.Xml.Serialization.XmlSerializer(GetType(Machine_Parameter))
            Dim file As New System.IO.StreamWriter(FileName)
            writer.Serialize(file, WriteData)
            file.Close()
        Catch ex As Exception
            MsgBox("点位文件创建/写入失败:" & ex.Message)
        End Try
        '每次点位更改都备份
        Call ParSaveAs()
    End Sub
#End Region

#Region "Tray矩阵计算"
    Structure sctTrayMatrix
        Dim TrayGlue() As Dist_XY
        Dim TrayFineCompensation() As Dist_XY
        Dim TrayPaste() As Dist_XY
        Dim TrayPreTaker() As Dist_XY
        Dim TrayRecheck() As Dist_XY
    End Structure

    Public TrayMatrix As sctTrayMatrix

    Public Function PointRefresh(P0 As Dist_XY, P1 As Dist_XY, P2 As Dist_XY, Row As Integer, Column As Integer) As Dist_XY()
        Dim tempRowPoint0() As Dist_XY
        Dim tempRowPoint1() As Dist_XY
        Dim tempColumnPoint() As Dist_XY
        Dim index As Integer
        Dim Point(Row * Column - 1) As Dist_XY

        Dim P3 As Dist_XY

        If Row < 2 Then Row = 2
        If Column < 2 Then Column = 2

        '求平行四边形的第4个点
        P3 = ParallelogramFourthPoint(P0, P1, P2)

        '求P0,P3的均等分点
        tempRowPoint0 = AveragePoint(P0, P3, Row)

        '求P1,P2的均等分点
        tempRowPoint1 = AveragePoint(P1, P2, Row)

        index = 0
        For i = 0 To tempRowPoint0.Length - 1
            tempColumnPoint = AveragePoint(tempRowPoint0(i), tempRowPoint1(i), Column)
            For j = 0 To tempColumnPoint.Length - 1
                Point(index) = tempColumnPoint(j)
                index = index + 1
            Next
        Next

        Return Point 
    End Function

    Private Function ParallelogramFourthPoint(tempP0 As Dist_XY, tempP1 As Dist_XY, tempP2 As Dist_XY) As Dist_XY
        Dim tempP3 As Dist_XY
        tempP3.X = tempP2.X - tempP1.X + tempP0.X
        tempP3.Y = tempP2.Y - tempP1.Y + tempP0.Y
        Return tempP3
    End Function

    Private Function AveragePoint(StartPoint As Dist_XY, EndPoint As Dist_XY, AveragenNum As Integer) As Dist_XY()
        '首先对输入的均分点数进行判断,包括起始点和终止点,所以均分点必须>=2
        If AveragenNum < 2 Then AveragenNum = 2

        Dim n As Integer = AveragenNum - 1

        Dim TempPoint(n) As Dist_XY

        For i = 0 To n
            TempPoint(i).X = (i * EndPoint.X + (n - i) * StartPoint.X) / n
            TempPoint(i).Y = (i * EndPoint.Y + (n - i) * StartPoint.Y) / n 
        Next

        Return TempPoint
    End Function
#End Region

End Module
