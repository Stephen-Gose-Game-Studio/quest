﻿Public Class Launcher
    Public Event BrowseForGame()
    Public Event LaunchGame(filename As String)
    Public Event EditGame(filename As String)
    Public Event CreateNewGame()
    Public Event BrowseForGameEdit()
    Public Event Tutorial()

    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        ctlTabs.SelectedIndex = CInt(AxeSoftware.Utility.Registry.GetSetting("Quest", "Settings", "SelectedTab", 0))
        AddHandler ctlPlayBrowser.LaunchGame, AddressOf ctlPlayBrowser_LaunchGame
        AddHandler ctlPlayBrowser.GotUpdateData, AddressOf ctlPlayBrowser_GotUpdateData
        AddHandler ctlEditBrowser.EditGame, AddressOf ctlEditBrowser_EditGame
        AddHandler ctlEditBrowser.CreateNewGame, AddressOf ctlEditBrowser_CreateNewGame
        AddHandler ctlEditBrowser.OpenGame, AddressOf ctlEditBrowser_OpenGame
        AddHandler ctlEditBrowser.Tutorial, AddressOf ctlEditBrowser_Tutorial
    End Sub

    Public Sub AddToRecent(filename As String, name As String)
        ctlPlayBrowser.AddToRecent(filename, name)
    End Sub

    Public Sub AddToEditorRecent(filename As String, name As String)
        ctlEditBrowser.AddToRecent(filename, name)
    End Sub

    Private Sub ctlPlayBrowser_LaunchGame(filename As String)
        RaiseEvent LaunchGame(filename)
    End Sub

    Private Sub ctlEditBrowser_EditGame(filename As String)
        RaiseEvent EditGame(filename)
    End Sub

    Private Sub ctlEditBrowser_CreateNewGame()
        RaiseEvent CreateNewGame()
    End Sub

    Private Sub ctlEditBrowser_OpenGame()
        RaiseEvent BrowseForGameEdit()
    End Sub

    Private Sub ctlEditBrowser_Tutorial()
        RaiseEvent Tutorial()
    End Sub

    Private Sub ctlTabs_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ctlTabs.SelectedIndexChanged
        AxeSoftware.Utility.Registry.SaveSetting("Quest", "Settings", "SelectedTab", ctlTabs.SelectedIndex)
    End Sub

    Public Sub RefreshLists()
        ctlPlayBrowser.Populate()
        ctlEditBrowser.Populate()
    End Sub

    Public Sub MainWindowShown()
        ctlPlayBrowser.MainWindowShown()
    End Sub

    Private m_questVersion As Version

    Public Property QuestVersion As Version
        Get
            Return m_questVersion
        End Get
        Set(value As Version)
            m_questVersion = value
            WebClientFactory.QuestVersion = value.ToString
        End Set
    End Property

    Private Sub ctlPlayBrowser_GotUpdateData(data As UpdatesData)
        ctlVersionInfo.UpdateInfo = data
        If IsNewVersion(data) Then
            BeginInvoke(Sub()
                            ctlVersionInfo.UpdateInfo = data
                            ctlVersionInfo.Visible = True
                        End Sub)
        End If
    End Sub

    Private Function IsNewVersion(updateData As UpdatesData) As Boolean
        If updateData Is Nothing Then Return False
        Dim latestMajor As Integer
        Dim latestMinor As Integer
        Dim latestBuild As Integer
        Dim latestRevision As Integer

        Integer.TryParse(updateData.Major, latestMajor)
        Integer.TryParse(updateData.Minor, latestMinor)
        Integer.TryParse(updateData.Build, latestBuild)
        Integer.TryParse(updateData.Revision, latestRevision)

        Dim latestVersion As New Version(latestMajor, latestMinor, latestBuild, latestRevision)
        Return latestVersion > QuestVersion
    End Function

End Class
