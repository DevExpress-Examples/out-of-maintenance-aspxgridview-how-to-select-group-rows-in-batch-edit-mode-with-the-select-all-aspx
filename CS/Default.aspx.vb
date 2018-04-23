Option Infer On

Imports DevExpress.Web
Imports DevExpress.Web.Data
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls

Partial Public Class _Default
    Inherits System.Web.UI.Page

    Private Const GroupCheckBoxFieldName As String = "C2"

    Public ReadOnly Property Data() As List(Of Item)
        Get
            If Session("Data") Is Nothing Then
                Session("Data") = Enumerable.Range(0, 100).Select(Function(i) New Item(i, (New Date(2017, 6, 21)).AddDays(20 * i), i Mod 3 = 0)).ToList()
            End If

            Return DirectCast(Session("Data"), List(Of Item))
        End Get
    End Property

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs)
        If Not IsPostBack Then
            Session("Data") = Nothing
        End If

        Grid.DataSource = Data
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
        If Not IsPostBack Then
            Grid.DataBind()
            Grid.ExpandRow(0)
        End If
    End Sub

    Protected Sub GroupCheckBox_Load(ByVal sender As Object, ByVal e As EventArgs)
        Dim grouppedRowsCount As Integer = Nothing
        Dim checkedInvisibleIndecesCount As Integer = Nothing
        Dim checkedVisibleIndecesCount As Integer = Nothing
        Dim checkState As CheckState = Nothing
        Dim checkBox = DirectCast(sender, ASPxCheckBox)
        Dim container = CType(checkBox.NamingContainer, GridViewGroupRowTemplateContainer)
        Dim grid = container.Grid
        Dim visibleIndex = container.VisibleIndex
        Dim visibleChildDataRowIndices = GetVisibleChildDataRowIndices(grid, visibleIndex, grouppedRowsCount, checkedInvisibleIndecesCount, checkedVisibleIndecesCount, checkState)

        checkBox.CheckState = checkState
        checkBox.JSProperties("cpGroupValue") = Grid.GetRowValues(visibleIndex, "C1")
        checkBox.JSProperties("cpVisibleChildDataRowIndices") = visibleChildDataRowIndices
        checkBox.JSProperties("cpGrouppedRowsCount") = grouppedRowsCount
        checkBox.JSProperties("cpCheckedInvisibleIndecesCount") = checkedInvisibleIndecesCount
        checkBox.JSProperties("cpCheckedVisibleIndecesCount") = checkedVisibleIndecesCount

        If Not grid.IsRowExpanded(visibleIndex) Then
            checkBox.Enabled = False
        End If
    End Sub

    Protected Function GetVisibleChildDataRowIndices(ByVal grid As ASPxGridView, ByVal groupVisibleIndex As Integer, <System.Runtime.InteropServices.Out()> ByRef grouppedRowsCount As Integer, <System.Runtime.InteropServices.Out()> ByRef checkedInvisibleIndecesCount As Integer, <System.Runtime.InteropServices.Out()> ByRef checkedVisibleIndecesCount As Integer, <System.Runtime.InteropServices.Out()> ByRef checkState As CheckState) As List(Of Integer)
        Dim result = New List(Of Integer)()
        Dim groupLevel = grid.GetRowLevel(groupVisibleIndex)
        Dim visibleStart = grid.VisibleStartIndex
        Dim visibleEnd = visibleStart + Math.Min(grid.VisibleRowCount - visibleStart, grid.SettingsPager.PageSize) - 1
        Dim visibleIndex = groupVisibleIndex + 1

        grouppedRowsCount = 0
        checkedInvisibleIndecesCount = 0
        checkedVisibleIndecesCount = 0

        Do While grid.GetRowLevel(visibleIndex) > groupLevel
            Dim checkValue = CBool(grid.GetRowValues(visibleIndex, GroupCheckBoxFieldName))

            If Not grid.IsGroupRow(visibleIndex) AndAlso ((visibleIndex >= visibleStart AndAlso visibleIndex <= visibleEnd) AndAlso grid.GetRowLevel(visibleIndex) <> groupLevel) Then
                If checkValue Then
                    checkedVisibleIndecesCount += 1
                End If

                result.Add(visibleIndex)
            Else
                If checkValue Then
                    checkedInvisibleIndecesCount += 1
                End If
            End If

            visibleIndex += 1
            grouppedRowsCount += 1
        Loop

        checkState = GetCheckState(checkedVisibleIndecesCount + checkedInvisibleIndecesCount, grouppedRowsCount)

        Return result
    End Function

    Protected Function GetCheckState(ByVal checkedRowsCount As Integer, ByVal grouppedRowsCount As Integer) As CheckState
        If checkedRowsCount = 0 Then
            Return CheckState.Unchecked
        ElseIf checkedRowsCount = grouppedRowsCount Then
            Return CheckState.Checked
        Else
            Return CheckState.Indeterminate
        End If
    End Function

    Protected Sub Grid_BatchUpdate(ByVal sender As Object, ByVal e As DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs)
        For Each ASPxDataUpdateValues As ASPxDataUpdateValues In e.UpdateValues
            Data.FirstOrDefault(Function(item) item.ID = CInt((ASPxDataUpdateValues.Keys("ID")))).C2 = CBool(ASPxDataUpdateValues.NewValues("C2"))
        Next ASPxDataUpdateValues

        e.Handled = True
    End Sub

    Public Class Item
        Public Property ID() As Integer

        Public Property C1() As Date

        Public Property C2() As Boolean

        Public Sub New(ByVal id As Integer, ByVal c1 As Date, ByVal c2 As Boolean)
            Me.ID = id
            Me.C1 = c1
            Me.C2 = c2
        End Sub
    End Class
End Class