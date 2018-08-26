Imports HtmlAgilityPack
Imports System.Net
Imports System.IO
Imports SevenZip

Public Class Form1

    Dim FilesCount As Integer = 0
    Dim Lang As String = "Arabic"
    Dim LangCode As String = "Ara"
    Dim movieName As String = ""

    Public Shared Function isConnected() As Boolean
        Try
            Dim addresslist As IPAddress() = Dns.GetHostAddresses("www.google.com")
            ' | ' addresslist holds a list of ipadresses to google          |
            ' | ' e.g  173.194.40.112                                       |
            ' | '                .116                                       |
            If addresslist(0).ToString().Length > 6 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Sockets.SocketException
            ' | ' You are offline                   |
            ' | ' the host is unkonwn               |
            Return False
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Button1.Enabled = False
        Button2.Enabled = False
        GroupBox1.Enabled = False

        If ComboBox1.SelectedItem.ToString = "Arabic" Then
            Lang = "Arabic"
            LangCode = "Ara"

        ElseIf ComboBox1.SelectedItem.ToString = "Brazillian Portuguese" Then
            Lang = "Brazillian Portuguese"
            LangCode = "Por"

        ElseIf ComboBox1.SelectedItem.ToString = "Danish" Then
            Lang = "Danish"
            LangCode = "Dan"

        ElseIf ComboBox1.SelectedItem.ToString = "Dutch" Then
            Lang = "Dutch"
            LangCode = "Dut"

        ElseIf ComboBox1.SelectedItem.ToString = "English" Then
            Lang = "English"
            LangCode = "Eng"

        ElseIf ComboBox1.SelectedItem.ToString = "Farsi/Persian" Then
            Lang = "Farsi/Persian"
            LangCode = "Per"

        ElseIf ComboBox1.SelectedItem.ToString = "Finnish" Then
            Lang = "Finnish"
            LangCode = "Fin"

        ElseIf ComboBox1.SelectedItem.ToString = "French" Then
            Lang = "French"
            LangCode = "Fre"

        ElseIf ComboBox1.SelectedItem.ToString = "Hebrew" Then
            Lang = "Hebrew"
            LangCode = "Heb"

        ElseIf ComboBox1.SelectedItem.ToString = "Indonesian" Then
            Lang = "Indonesian"
            LangCode = "Ind"

        ElseIf ComboBox1.SelectedItem.ToString = "Italian" Then
            Lang = "Italian"
            LangCode = "Ita"

        ElseIf ComboBox1.SelectedItem.ToString = "Malay" Then
            Lang = "Malay"
            LangCode = "May"

        ElseIf ComboBox1.SelectedItem.ToString = "Norwegian" Then
            Lang = "Norwegian"
            LangCode = "Nor"

        ElseIf ComboBox1.SelectedItem.ToString = "Romanian" Then
            Lang = "Romanian"
            LangCode = "Rum"

        ElseIf ComboBox1.SelectedItem.ToString = "Spanish" Then
            Lang = "Spanish"
            LangCode = "Spa"

        ElseIf ComboBox1.SelectedItem.ToString = "Swedish" Then
            Lang = "Swedish"
            LangCode = "Swe"

        ElseIf ComboBox1.SelectedItem.ToString = "Turkish" Then
            Lang = "Turkish"
            LangCode = "Tur"

        ElseIf ComboBox1.SelectedItem.ToString = "Vietnamese" Then
            Lang = "Vietnamese"
            LangCode = "Vie"

        End If
        BackgroundWorker1.RunWorkerAsync()

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        If FolderBrowserDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            TextBox1.Text = FolderBrowserDialog1.SelectedPath
        End If

    End Sub

    Public Structure subtitle

        Public Language As String
        Public Title As String
        Public Owner As String
        Public Link As String
        Public positive As String

    End Structure

    Private Sub BackgroundWorker1_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork

        Dim strFile As String = Directory.GetCurrentDirectory & "\ErrorLog_" & DateTime.Today.ToString("dd-MMM-yyyy") & ".txt"
        Dim fileExists As Boolean = File.Exists(strFile)
        Dim c As Integer = 0

        Using writer As New StreamWriter(strFile, True)
            If Not fileExists Then
                writer.WriteLine("Start Error Log for today")
            End If

            If Not IO.Directory.Exists(TextBox1.Text) Then
                MsgBox("Folder doesn't exist !!")
                Exit Sub
            End If


            If Not isConnected() = True Then
                MsgBox("You Are Not Connected to the Internet !!")
                Exit Sub
            End If

            Dim subPath As String = FileIO.SpecialDirectories.Temp & "\SubSceneDownloader"

            Dim files = Directory.EnumerateFiles(TextBox1.Text, "*.*", SearchOption.AllDirectories) _
                        .Where(Function(s) s.EndsWith(".mp4", StringComparison.OrdinalIgnoreCase) _
                                    OrElse s.EndsWith(".mpg", StringComparison.OrdinalIgnoreCase) _
                                    OrElse s.EndsWith(".avi", StringComparison.OrdinalIgnoreCase) _
                                    OrElse s.EndsWith(".mov", StringComparison.OrdinalIgnoreCase) _
                                    OrElse s.EndsWith(".mkv", StringComparison.OrdinalIgnoreCase))

            FilesCount = files.Count

            For Each filename As String In files

                Dim skip As Boolean = False

                BackgroundWorker1.ReportProgress(c)
                movieName = Path.GetFileNameWithoutExtension(filename)
                subPath = Path.GetDirectoryName(filename)

                If CheckBox3.Checked And (CheckBox1.Checked = True) Then

                    If Directory.Exists(subPath.ToString + "\subtitles") Then

                        Dim subFiles = Directory.EnumerateFiles(subPath.ToString + "\subtitles", "*.srt", SearchOption.TopDirectoryOnly)

                        For i As Integer = 0 To subFiles.Count - 1
                            Dim subname As String = ""
                            subname = Path.GetFileNameWithoutExtension(subFiles(i)).Substring(0, Path.GetFileNameWithoutExtension(subFiles(i)).Length - 6)
                            If movieName Like subname Then
                                skip = True
                                Exit For
                            End If
                        Next

                        If skip = True Then
                            c += 1
                            Continue For
                        End If

                    End If

                ElseIf CheckBox3.Checked And (CheckBox1.Checked = False) Then

                    If Directory.Exists(subPath.ToString) Then

                        Dim subFiles = Directory.EnumerateFiles(subPath.ToString, "*.srt", SearchOption.TopDirectoryOnly)

                        For i As Integer = 0 To subFiles.Count - 1
                            Dim subname As String = ""
                            subname = Path.GetFileNameWithoutExtension(subFiles(i)).Substring(0, Path.GetFileNameWithoutExtension(subFiles(i)).Length - 6)
                            If movieName Like subname Then
                                skip = True
                                Exit For
                            End If
                        Next

                        If skip = True Then
                            c += 1
                            Continue For
                        End If

                    End If

                End If

                Try

                    Dim html = "https://subscene.com/subtitles/release?q=" + movieName + "&r=true"
                    Dim web As HtmlWeb = New HtmlWeb()
                    Dim htmlDoc = web.Load(html)
                    htmlDoc.OptionAutoCloseOnEnd = True
                    Dim htmlBody = htmlDoc.DocumentNode.SelectSingleNode("//body/div/div/div/div/table/tbody")
                    Dim childNodes As HtmlNodeCollection = htmlBody.ChildNodes
                    Dim n As Integer = 0
                    Dim subtitles As subtitle() = New subtitle(childNodes.Count) {}

                    For Each node In childNodes

                        If node.NodeType = HtmlNodeType.Element Then
                            Dim nNodes As HtmlNodeCollection = node.ChildNodes
                            Dim s As String = nNodes(1).InnerHtml
                            Dim i As Integer = s.IndexOf("<a href=""")
                            Dim f As String = s.Substring(i + 9, s.IndexOf(""">", i + 1) - i - 9)

                            subtitles(n).Link = "https://subscene.com" + f

                            i = s.IndexOf("<span class=")
                            f = s.Substring(i + 12, s.IndexOf("</span>", i + 1) - i - 12)

                            If f.ToLower.Contains(Lang.ToLower) Then
                                subtitles(n).Language = Lang
                            End If

                            If f.ToLower.Contains("positive") Then
                                subtitles(n).positive = "positive"
                            End If

                            i = s.IndexOf("<span>")
                            f = s.Substring(i + 12, s.IndexOf("</span>", i + 1) - i - 12)
                            subtitles(n).Title = f.TrimStart.TrimEnd

                            s = nNodes(7).InnerHtml

                            If s.TrimStart.TrimEnd.StartsWith("<a href=", StringComparison.OrdinalIgnoreCase) Then
                                i = s.IndexOf(">")
                                f = s.Substring(i + 1, s.IndexOf("</a>", i + 1) - i - 1)

                                subtitles(n).Owner = f.TrimStart.TrimEnd

                            Else
                                subtitles(n).Owner = s.TrimStart.TrimEnd

                            End If

                        End If
                        n += 1

                    Next

                    DataGridView1.Rows.Add(subtitles.Count)

                    For i As Integer = 0 To subtitles.Count - 1
                        DataGridView1.Rows(i).Cells(0).Value = subtitles(i).Title
                        DataGridView1.Rows(i).Cells(1).Value = subtitles(i).Link
                        DataGridView1.Rows(i).Cells(2).Value = subtitles(i).Owner
                        DataGridView1.Rows(i).Cells(3).Value = subtitles(i).Language
                        DataGridView1.Rows(i).Cells(4).Value = subtitles(i).positive
                    Next

                    For i As Integer = DataGridView1.RowCount - 1 To 0 Step -1

                        If DataGridView1.Rows(i).Cells(3).Value = "" Then
                            DataGridView1.Rows.RemoveAt(i)

                        ElseIf DataGridView1.Rows(i).Cells(0).Value.ToString.ToLower.Contains("trailer") Then
                            DataGridView1.Rows.RemoveAt(i)

                        End If
                    Next

                    DataGridView1.Sort(DataGridView1.Columns("Rating"), System.ComponentModel.ListSortDirection.Descending)

                    Dim b As Integer = 1
                    Dim sN As Integer = NumericUpDown1.Value
                    Dim tempPath As String = ""
                    tempPath = FileIO.SpecialDirectories.Temp & "\SubSceneDownloader\"

                    If DataGridView1.RowCount > 0 And DataGridView1.RowCount < (sN + 1) Then
                        'Try
                        For i As Integer = 0 To DataGridView1.RowCount - 1
                            Dim xhtml = DataGridView1.Rows(i).Cells(1).Value
                            Dim xweb As HtmlWeb = New HtmlWeb()
                            Dim xhtmlDoc = xweb.Load(xhtml)
                            Dim xf As String = ""

                            For Each node As HtmlNode In xhtmlDoc.DocumentNode.SelectNodes("//*[@id=""downloadButton""]")
                                Dim value As String = node.OuterHtml
                                Dim xs As String = value
                                Dim xi As Integer = xs.IndexOf("""")
                                xf = xs.Substring(xi + 1, xs.IndexOf("""", xi + 1) - xi - 1)

                            Next

                            If File.Exists(tempPath & "sub.zip") Then
                                IO.File.Delete(tempPath & "sub.zip")
                            End If

                            If Directory.Exists(tempPath & "sub") Then
                                IO.Directory.Delete(tempPath & "sub", True)
                            End If

                            My.Computer.Network.DownloadFile("https://subscene.com" + xf, tempPath & "sub.zip")

                            Dim _zLibraryPath As String = ""
                            If System.Environment.Is64BitProcess = True Then
                                _zLibraryPath = My.Computer.FileSystem.CurrentDirectory + "\x64\7z.dll"
                            Else
                                _zLibraryPath = My.Computer.FileSystem.CurrentDirectory + "\x86\7z.dll"
                            End If

                            SevenZipBase.SetLibraryPath(_zLibraryPath)

                            Dim Extractor As New SevenZipExtractor(tempPath & "sub.zip")
                            Extractor.ExtractArchive(tempPath & "sub")
                            'Extractor.Dispose()

                            Dim subFiles = Directory.EnumerateFiles(tempPath & "sub", "*.*", SearchOption.AllDirectories) _
                            .Where(Function(s) s.EndsWith(".srt", StringComparison.OrdinalIgnoreCase))

                            For Each subFilename As String In subFiles

                                Dim sPath As String = ""
                                If CheckBox1.Checked Then
                                    sPath = subPath.ToString + "\subtitles"
                                Else
                                    sPath = subPath.ToString
                                End If

                                If IO.Directory.Exists(sPath) = False Then
                                    IO.Directory.CreateDirectory(sPath)
                                End If

                                Try
                                    SetAttr(sPath.ToString + "\" + movieName.ToString + "." + LangCode + "." + b.ToString + ".srt", FileAttribute.Normal)
                                Catch ex As Exception
                                End Try
                                File.Copy(subFilename, sPath.ToString + "\" + movieName.ToString + "." + LangCode + "." + b.ToString + ".srt", True)

                                If CheckBox2.Checked Then
                                    Try
                                        SetAttr(sPath.ToString + "\" + movieName.ToString + "." + LangCode + "." + b.ToString + ".srt", FileAttribute.Hidden)
                                        SetAttr(subPath.ToString + "\subtitles", FileAttribute.Hidden)
                                    Catch ex As Exception
                                    End Try
                                End If

                                b += 1
                            Next

                            If File.Exists(tempPath & "sub.zip") Then
                                IO.File.Delete(tempPath & "sub.zip")
                            End If

                            If Directory.Exists(tempPath & "sub") Then
                                IO.Directory.Delete(tempPath & "sub", True)
                            End If

                        Next
                        'Catch ex As Exception
                        '    writer.WriteLine("Error Message in  Occured at-- " & DateTime.Now & vbNewLine & ex.ToString & vbNewLine)
                        'End Try

                    ElseIf DataGridView1.RowCount > sN Then

                        'Try
                        For i As Integer = 0 To (sN - 1)
                            Dim xhtml = DataGridView1.Rows(i).Cells(1).Value
                            Dim xweb As HtmlWeb = New HtmlWeb()
                            Dim xhtmlDoc = xweb.Load(xhtml)
                            Dim xf As String = ""

                            For Each node As HtmlNode In xhtmlDoc.DocumentNode.SelectNodes("//*[@id=""downloadButton""]")
                                Dim value As String = node.OuterHtml

                                Dim xs As String = value
                                Dim xi As Integer = xs.IndexOf("""")
                                xf = xs.Substring(xi + 1, xs.IndexOf("""", xi + 1) - xi - 1)

                            Next

                            If File.Exists(tempPath & "sub.zip") Then
                                IO.File.Delete(tempPath & "sub.zip")
                            End If

                            If Directory.Exists(tempPath & "sub") Then
                                IO.Directory.Delete(tempPath & "sub", True)
                            End If

                            My.Computer.Network.DownloadFile("https://subscene.com" + xf, tempPath & "sub.zip")

                            Dim _zLibraryPath As String = ""
                            If System.Environment.Is64BitProcess = True Then
                                _zLibraryPath = My.Computer.FileSystem.CurrentDirectory + "\x64\7z.dll"
                            Else
                                _zLibraryPath = My.Computer.FileSystem.CurrentDirectory + "\x86\7z.dll"
                            End If

                            SevenZipBase.SetLibraryPath(_zLibraryPath)
                            Dim Extractor As New SevenZipExtractor(tempPath & "sub.zip")
                            Extractor.ExtractArchive(tempPath & "sub")
                            'Extractor.Dispose()

                            Dim subFiles = Directory.EnumerateFiles(tempPath & "sub", "*.*", SearchOption.AllDirectories) _
                            .Where(Function(s) s.EndsWith(".srt", StringComparison.OrdinalIgnoreCase))

                            For Each subFilename As String In subFiles

                                Dim sPath As String = ""
                                If CheckBox1.Checked Then
                                    sPath = subPath.ToString + "\subtitles"
                                Else
                                    sPath = subPath.ToString
                                End If

                                If IO.Directory.Exists(sPath) = False Then
                                    IO.Directory.CreateDirectory(sPath)
                                End If

                                Try
                                    SetAttr(sPath.ToString + "\" + movieName.ToString + "." + LangCode + "." + b.ToString + ".srt", FileAttribute.Normal)
                                Catch ex As Exception
                                End Try
                                File.Copy(subFilename, sPath.ToString + "\" + movieName.ToString + "." + LangCode + "." + b.ToString + ".srt", True)

                                If CheckBox2.Checked Then
                                    Try
                                        SetAttr(sPath.ToString + "\" + movieName.ToString + "." + LangCode + "." + b.ToString + ".srt", FileAttribute.Hidden)
                                        SetAttr(subPath.ToString + "\subtitles", FileAttribute.Hidden)
                                    Catch ex As Exception
                                    End Try
                                End If

                                b += 1
                            Next

                            If File.Exists(tempPath & "sub.zip") Then
                                IO.File.Delete(tempPath & "sub.zip")
                            End If

                            If Directory.Exists(tempPath & "sub") Then
                                IO.Directory.Delete(tempPath & "sub", True)
                            End If

                        Next
                        'Catch ex As Exception
                        '    writer.WriteLine("Error Message in  Occured at-- " & DateTime.Now & vbNewLine & ex.ToString & vbNewLine)
                        'End Try

                    ElseIf DataGridView1.RowCount = 0 Then

                    End If
                Catch ex As Exception
                    writer.WriteLine("Error Message in  Occured at-- " & DateTime.Now & vbNewLine & ex.ToString & vbNewLine)
                End Try
                c += 1

            Next

        End Using
        BackgroundWorker1.ReportProgress(c)

    End Sub

    Private Sub BackgroundWorker1_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles BackgroundWorker1.ProgressChanged

        Label1.Text = e.ProgressPercentage.ToString + " / " & FilesCount.ToString

        ProgressBar1.Value = (e.ProgressPercentage / FilesCount) * 100

        Label6.Text = movieName

    End Sub

    Private Sub BackgroundWorker1_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted

        Try
            If ComboBox2.SelectedIndex = 1 Then
                Shell("Shutdown -s -f -t 5")
            ElseIf ComboBox2.SelectedIndex = 2 Then
                Shell("Shutdown -r -f -t 5")
            ElseIf ComboBox2.SelectedIndex = 3 Then
                Shell("shutdown -l -f")
            ElseIf ComboBox2.SelectedIndex = 4 Then
                Application.SetSuspendState(PowerState.Suspend, True, True)
            ElseIf ComboBox2.SelectedIndex = 5 Then
                Application.SetSuspendState(PowerState.Hibernate, True, True)
            End If
        Catch ex As Exception
        End Try

        MsgBox("DONE")

        Button1.Enabled = True
        Button2.Enabled = True
        GroupBox1.Enabled = True
        Label6.Text = ""
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Label6.Text = ""
        Label1.Text = ""
        ComboBox1.SelectedIndex = 0
        ComboBox2.SelectedIndex = 0
    End Sub

End Class
