Imports System.IO
Imports VB = Microsoft.VisualBasic
Imports System.Runtime.InteropServices

Module FuntctionSub

    Public DelayFlag As Integer
    Public Declare Function GetTickCount Lib "kernel32" () As Long
    Public Declare Sub Sleep Lib "kernel32" (ByVal dwMilliseconds As Long)
    Public Declare Function FindWindow Lib "user32" Alias "FindWindowA" (ByVal lpClassName As String, _
                                                                     ByVal lpWindowName As String) As Integer
    Public Declare Function BringWindowToTop Lib "user32" (ByVal hwnd As Integer) As Integer
    Declare Function ShellExecute Lib "shell32.dll" Alias "ShellExecuteA" (ByVal hwnd As Integer, ByVal lpOperation As String, ByVal lpFile As String, ByVal lpParameters As String, ByVal lpDirectory As String, ByVal nShowCmd As Integer) As Integer


#Region "功能：创建文件函数"
    ''' <summary>
    ''' 创建必要的文件夹
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub FileInit()
        Dim TempPath As String
        TempPath = "E:\BZ-Data"
        If Dir(TempPath, vbDirectory) = "" Then
            FileIO.FileSystem.CreateDirectory(TempPath)
        End If
        '创建数据保存文件夹
        If Dir(Path_Data, vbDirectory) = "" Then
            FileIO.FileSystem.CreateDirectory(Path_Data)
        End If
        '创建图像数据文件夹
        If Dir(Path_Image, vbDirectory) = "" Then
            FileIO.FileSystem.CreateDirectory(Path_Image)
        End If
        '创建Log日志文件夹
        If Dir(Path_Log, vbDirectory) = "" Then
            FileIO.FileSystem.CreateDirectory(Path_Log)
        End If
        '创建ErrLog日志文件夹
        If Dir(Path_Errlog, vbDirectory) = "" Then
            FileIO.FileSystem.CreateDirectory(Path_Errlog)
        End If

    End Sub
#End Region

#Region "功能：延时函数"
    Public Function Delay(delayTime As Long) As Boolean
        Static TT As Long
        If DelayFlag = 0 Then
            TT = GetTickCount
            Delay = True
            DelayFlag = 1
        End If
        Do
            My.Application.DoEvents()
            If GetTickCount - TT < 0 Then
                TT = GetTickCount
            End If
        Loop Until GetTickCount - TT >= delayTime
        DelayFlag = 0
        Delay = False
    End Function
#End Region

#Region "功能：超时判定"
    ''' <summary>
    ''' 超时判定
    ''' </summary>
    ''' <param name="startTime">开始时间</param>
    ''' <param name="interval">间隔时间 单位unit: ms</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function isTimeout(ByVal startTime As Long, ByVal interval As Long) As Boolean
        If GetTickCount - startTime > interval Then
            Return True
        Else
            Return False
        End If
    End Function
#End Region


#Region "写TXT文件"
    ''' <summary>
    ''' 写TXT文件 (带回车换行符)
    ''' </summary>
    ''' <param name="FilePath">FilePanth文件路径</param>
    ''' <param name="FileName">FileName文件名</param>
    ''' <param name="strTxt"></param>
    ''' <remarks>strTxt内容</remarks>
    Public Sub WriteTxt(ByVal FilePath As String, ByVal FileName As String, ByVal strTxt As String)

        Dim FileNo As Integer   '文件号
        'FilePath = FilePath & "\" & Format(Now.Date, "yyyyMMdd")
        If Dir(FilePath, vbDirectory) = "" Then
            MkDir(FilePath)
        End If
        FileNo = FreeFile()
        FileOpen(FileNo, FilePath & "\" & FileName & ".txt", OpenMode.Append)
        PrintLine(FileNo, Format(Now, "HH:mm:ss:ffff") & ": " & strTxt)
        FileClose(FileNo)
    End Sub
    ''' <summary>
    ''' 写不带回车换行符的TXT
    ''' </summary>
    ''' <param name="FileName">文件名称：包含路径</param>
    ''' <param name="WriteData">要写入的数据（String）</param>
    ''' <remarks></remarks>
    Public Sub WriteDataTxt(ByVal FileName As String, ByVal WriteData As String)
        Dim rs As New System.IO.StreamWriter(FileName, True)
        Try
            rs.Write(WriteData)
            rs.Close()
        Catch ex As Exception
            MsgBox("文件写入失败" & ex.Message, MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, "错误")
            rs.Close()
        End Try
    End Sub

#End Region

    Public Sub Write_Log(ByVal station As Integer, ByVal str As String, ByVal FilePath As String)
        Dim FileNo As Integer   '文件号
        FilePath = FilePath & Format(Now.Date, "yyyy-MM-dd")
        If Dir(FilePath, vbDirectory) = "" Then
            MkDir(FilePath)
        End If
        FileNo = FreeFile()
        FileOpen(FileNo, FilePath & "\S" & station & "-Log.txt", OpenMode.Append)
        PrintLine(FileNo, Format(Now, "HH:mm:ss:ffff") & ": " & str)
        FileClose(FileNo)
    End Sub

    Public Sub Write_Log(ByVal str As String, ByVal FilePath As String)
        Dim FileNo As Integer   '文件号
        FilePath = FilePath & Format(Now.Date, "yyyy-MM-dd")
        If Dir(FilePath, vbDirectory) = "" Then
            MkDir(FilePath)
        End If
        FileNo = FreeFile()
        FileOpen(FileNo, FilePath & "\Log.txt", OpenMode.Append)
        PrintLine(FileNo, Format(Now, "HH:mm:ss:ffff") & ": " & str)
        FileClose(FileNo)
    End Sub


#Region "Ini文件相关操作"
    '读取ini文件内容
    Public Function GetINI(ByVal Section As String, ByVal AppName As String, ByVal lpDefault As String, _
                           ByVal FileName As String) As String
        Dim Str As String = ""
        Str = LSet(Str, 256)
        GetPrivateProfileString(Section, AppName, lpDefault, Str, Len(Str), FileName)
        Return Microsoft.VisualBasic.Left(Str, InStr(Str, Chr(0)) - 1)
    End Function
    '写ini文件操作
    Public Function WriteINI(ByVal Section As String, ByVal AppName As String, ByVal lpDefault As String, _
                             ByVal FileName As String) As Long
        WriteINI = WritePrivateProfileString(Section, AppName, lpDefault, FileName)
    End Function
    '读ini API函数
    Private Declare Function GetPrivateProfileString Lib "kernel32" Alias "GetPrivateProfileStringA" _
        (ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal lpDefault As String, _
         ByVal lpReturnedString As String, ByVal nSize As Int32, ByVal lpFileName As String) As Int32
    '写ini API函数
    Private Declare Function WritePrivateProfileString Lib "kernel32" Alias "WritePrivateProfileStringA" _
        (ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal lpString As String, _
         ByVal lpFileName As String) As Int32
#End Region

#Region "创建INI文件"
    '创建INI文件，写入必要的默认参数
    Public Sub CreateIniFile(ByVal IniFilePath As String)
        Call WriteINI("Enabled", "SM", 1, IniFilePath)
        Call WriteINI("Enabled", "S1", 1, IniFilePath)
        Call WriteINI("Enabled", "S2", 1, IniFilePath)
        Call WriteINI("Enabled", "S3", 1, IniFilePath)
        Call WriteINI("Enabled", "S4", 1, IniFilePath)
        Call WriteINI("Enabled", "S5", 1, IniFilePath)
        'Call WriteINI("Enabled", "S5_VGA", 1, IniFilePath)
        'Call WriteINI("Enabled", "S5_PROX", 1, IniFilePath)

        Call WriteINI("统计信息", "产量统计", 0, IniFilePath)
        Call WriteINI("统计信息", "不良统计", 0, IniFilePath)
        Call WriteINI("统计信息", "计数1", 0, IniFilePath)
        Call WriteINI("统计信息", "计数2", 0, IniFilePath)
        Call WriteINI("统计信息", "备用", 0, IniFilePath)
        Call WriteINI("统计信息", "备用1", 0, IniFilePath)
        Call WriteINI("统计信息", "备用2", 0, IniFilePath)
        Call WriteINI("统计信息", "备用3", 0, IniFilePath)

        Call WriteINI("机台信息", "用户名", "BZ", IniFilePath)
        Call WriteINI("机台信息", "文件名", "BZ", IniFilePath)
        Call WriteINI("机台信息", "产品类型", 1, IniFilePath)
        Call WriteINI("机台信息", "异常信息", 1, IniFilePath)
        Call WriteINI("机台信息", "总工作时间", 0, IniFilePath)

    End Sub
#End Region

#Region "功能：用户名和密码写入和读取操作"

    ''' <summary>
    ''' 写DAT二进制文件
    ''' </summary>
    ''' <param name="FileName">参数1：文件名(包含文件路径)</param>
    ''' <param name="WriteData">参数2：写入的数据</param>
    ''' <remarks></remarks>
    Public Sub Write_Code(ByVal FileName As String, ByRef WriteData As UserCollect)
        Dim FileNo As Integer
        Try
            FileNo = FreeFile()                      '获取空闲可用的文件号
            FileOpen(FileNo, FileName, OpenMode.Binary)      '以二进制的形式打开文件
            'Write(FileNo, WriteData)
            FilePut(FileNo, WriteData)
            FileClose(FileNo)
        Catch ex As Exception
            FileClose(FileNo)                               '读取出错关闭当前打开的文件
            Frm_DialogAddMessage("账户密码文件写入失败")
            Frm_DialogAddMessage(ex.ToString)
        End Try
    End Sub

    ''' <summary>
    ''' 读DAT二进制文件
    ''' </summary>
    ''' <param name="FileName">参数1：文件名(包含文件路径)</param>
    ''' <param name="ReadData">参数2：读取的数据存储地址</param>
    ''' <remarks></remarks>
    Public Sub Read_Code(ByVal FileName As String, ByRef ReadData As UserCollect)
        Dim FileNo As Integer
        Try
            If IO.File.Exists(FileName) = False Then
                BOZHON.User1.Name = "OP"
                BOZHON.User1.Code = "999"
                BOZHON.User2.Name = "BOTECH"
                BOZHON.User2.Code = "999"
                BOZHON.User3.Name = "SW Engineering"
                BOZHON.User3.Code = "qwertyuiop"
                Call Write_Code(FileName, BOZHON)
            End If
            FileNo = FreeFile()                      '获取空闲可用的文件号
            FileOpen(FileNo, FileName, OpenMode.Binary)      '以二进制的形式打开文件
            FileGet(FileNo, ReadData)
            FileClose(FileNo)
        Catch ex As Exception
            FileClose(FileNo)                               '读取出错关闭当前打开的文件
            Frm_DialogAddMessage("账户密码文件读取失败")
            Frm_DialogAddMessage(ex.ToString)
        End Try
    End Sub
#End Region

#Region "功能：.CSV 文件相关操作"
    '写CSV逗号分隔值文件
    '参数1：文件名(包含文件路径)
    '参数2：需要写入的数据
    Public Function WriteCSV(ByVal FileName As String, ByVal Data As String, ByVal FilePath As String)
        Dim FileNo As Integer   '文件号
        FilePath = FilePath & Format(Now.Date, "yyyy-MM-dd")
        If Dir(FilePath, vbDirectory) = "" Then
            MkDir(FilePath)
        End If

        If IO.File.Exists(FilePath & "\" & FileName & ".csv") = False Then
            Call FileCopy("D:\BZ-Parameter\Template\PAM Template.csv", FilePath & "\" & FileName & ".csv")
        End If

        On Error GoTo WriteDataError                    '激活文件写操作错误检测机制
        FileNo = FreeFile()
        FileOpen(FileNo, FilePath & "\" & FileName & ".csv", OpenMode.Append)
        PrintLine(FileNo, Data)
        FileClose(FileNo)
WriteDataError:
        FileClose(FileNo)                           '写入出错关闭当前打开的文件
    End Function
#End Region

#Region "功能：XML创建，读写操作"

    Public Sub Read_par(ByVal path As String, ByVal data As Parameter)

        '结构体数组实例化
        Call par.Ini()

        Try
            If IO.File.Exists(path) = False Then
                Call Write_par(path, par)
            End If

            Dim reader As New System.Xml.Serialization.XmlSerializer(GetType(Parameter))
            Dim file As New System.IO.StreamReader(path)
            'par为定义的共用变量，用来保存读取的信息
            par = CType(reader.Deserialize(file), Parameter)
            file.Close()
        Catch ex As Exception
            MsgBox("Par XML文件读取失败:" & ex.Message)
        End Try
    End Sub

    Public Sub Write_par(ByVal FileName As String, ByRef WriteData As Parameter)
        Try
            Dim writer As New System.Xml.Serialization.XmlSerializer(GetType(Parameter))
            Dim file As New System.IO.StreamWriter(FileName)
            writer.Serialize(file, WriteData)
            file.Close()
        Catch ex As Exception
            MsgBox("Par XML文件创建失败:" & ex.Message)
        End Try
        '每次参数变化都参数备份
        Call ParSaveAs()
    End Sub


#End Region

#Region "删除N天以上的文件或文件夹"
    '*********************************************************************************
    '       参数1：Path，要删除的路径，例如"E:\log\"
    '       参数2：Pattern,文件的类型，例如"*.csv"
    '       参数3：N，删除多少天前的文件，例如N=15指删除15天前的文件
    '*********************************************************************************
    Public Sub FileDelete(ByVal Path As String, ByVal Pattern As String, ByVal N As Integer)
        Dim MyFile As String
        Dim tempstr As String

        MyFile = Dir(Path & Pattern)  ' Set the path.
        Do While MyFile <> ""   ' Start the loop.
            tempstr = Mid(MyFile, 11, 8)
            On Error Resume Next
            '判断文件是否是N天前的文件
            If tempstr <> "" And DateDiff("d", Mid(tempstr, 1, 4) & "-" & Mid(tempstr, 5, 2) & "-" & Mid(tempstr, 7, 2), Format(Now, "yyyy-MM-dd")) > N Then
                '删除N天前的文件
                Kill(Path & MyFile)
            End If
            MyFile = Dir()   ' Get next entry.
        Loop
    End Sub

    ''' <summary>
    ''' 删除N天前的文件夹 注：path不包含日期数据，要删除的文件夹命名要求为"yyyy-MM-dd"
    ''' </summary>
    ''' <param name="path">路径例如："E:\BZ-Data\Data\"</param>
    ''' <param name="N">天数</param>
    ''' <remarks></remarks>
    Public Sub Delete_Floder(ByVal path As String, ByVal N As Short)
        Dim tempstr As String

        Dim dirs As String() = Directory.GetDirectories(path, "2*")

        '删除用户临时文件夹下所有文件夹
        Dim dt As Date
        For Each foundFolder As String In My.Computer.FileSystem.GetDirectories(path, FileIO.SearchOption.SearchTopLevelOnly)
            If foundFolder <> "" Then
                Try
                    tempstr = Mid(foundFolder, path.Length + 1, 10)
                    dt = Date.ParseExact(tempstr, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture)
                    If DateDiff(DateInterval.Day, dt, Now) > N Then
                        'My.Computer.FileSystem.DeleteDirectory(foundFolder, FileIO.DeleteDirectoryOption.ThrowIfDirectoryNonEmpty)
                        Directory.Delete(foundFolder, True)
                    End If
                Catch
                    'MsgBox("捕获异常")
                End Try
            End If
        Next

    End Sub

#End Region

#Region "复制文件或文件夹到新路径下"
    '*********************************************************************************************************************
    '       参数1：num，操作的类型。0：复制文件夹到目标路径下；1：复制文件到目标路径下；2：复制文件到新路径下并赋予新名称
    '       参数2：SourcePath,源文件或文件夹的地址，例如"E:\11"，"E:\11\2015-05-19\Parameter.dat"
    '       参数3：TargetPath,目标文件或文件夹的地址，例如"E:\11"，"E:\11\2015-05-19\Parameter.dat"
    '*********************************************************************************************************************
    Public Sub FileOrFolderCopy(ByVal num As Integer, ByVal SourcePath As String, ByVal TargetPath As String)
        '定义一个对象
        Dim fs As Object
        '创建一个FileSystemObject
        fs = CreateObject("Scripting.FileSystemObject")

        Select Case num
            Case 0
                fs.CopyFolder(SourcePath, TargetPath)       '复制文件夹到目标目录下
            Case 1
                '复制源文件到目标目录下，注意是"E:\22\"，不是"E:\22"
                fs.CopyFile(SourcePath, TargetPath)
            Case 2
                FileCopy(SourcePath, TargetPath)
        End Select
    End Sub
#End Region

#Region "截屏并保存"
    Public Sub ScreenCut(ByVal ScreenCutFilePath As String)

        If Dir(ScreenCutFilePath, vbDirectory) = "" Then
            MkDir(ScreenCutFilePath)
        End If
        Dim p1 As New Point(0, 0)
        Dim p2 As New Point(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height)
        Dim PIC As New Bitmap(p2.X, p2.Y)
        Using g As Graphics = Graphics.FromImage(PIC)
            g.CopyFromScreen(p1, p1, p2)
            'Me.BackgroundImage = PIC
        End Using
        '保存全屏截图到本地，
        PIC.Save(ScreenCutFilePath & Format(Now, "yyyy-MM-dd HHmmss") & ".bmp")
    End Sub
#End Region

    ''' <summary>
    ''' 参数文件备份
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub ParSaveAs()
        Dim path As String
        path = "E:\BZ-Data\ParBackup"
        If Dir(path, vbDirectory) = "" Then
            MkDir(path)
        End If
        Call FileCopy("D:\BZ-Parameter\Par.xml", path & "\" & Format(Now, "yyyy-MM-dd HHmmss") & "-Par" & ".xml")
        Call FileCopy("D:\BZ-Parameter\Par_Position.xml", path & "\" & Format(Now, "yyyy-MM-dd HHmmss") & "-Par_Position" & ".xml")
    End Sub

    ''' <summary>
    ''' 蜂鸣器鸣叫提醒
    ''' </summary>
    ''' <param name="Type">类型</param>
    ''' <param name="interval">时间间隔</param>
    ''' <remarks></remarks>
    Public Sub Beep(ByVal Type As Short, ByVal interval As Integer)
        Select Case Type
            Case 1
                '鸣叫一次
                SetEXO(2, 15, True)
                Delay(interval)
                SetEXO(2, 15, False)

            Case Else

        End Select
    End Sub

End Module
