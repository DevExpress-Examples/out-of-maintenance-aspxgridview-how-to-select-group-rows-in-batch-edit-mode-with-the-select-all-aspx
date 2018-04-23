using DevExpress.Web;
using DevExpress.Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{
    const string GroupCheckBoxFieldName = "C2";

    public List<Item> Data
    {
        get
        {
            if (Session["Data"] == null)
                Session["Data"] = Enumerable.Range(0, 100).Select(i => new Item(i, new DateTime(2017, 6, 21).AddDays(20 * i), i % 3 == 0)).ToList();

            return (List<Item>)Session["Data"];
        }
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        if (!IsPostBack)
            Session["Data"] = null;

        Grid.DataSource = Data;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Grid.DataBind();
            Grid.ExpandRow(0);
        }
    }

    protected void GroupCheckBox_Load(object sender, EventArgs e)
    {
        int grouppedRowsCount;
        int checkedInvisibleIndecesCount;
        int checkedVisibleIndecesCount;
        CheckState checkState;
        var checkBox = (ASPxCheckBox)sender;
        var container = (GridViewGroupRowTemplateContainer)checkBox.NamingContainer;
        var grid = container.Grid;
        var visibleIndex = container.VisibleIndex;
        var visibleChildDataRowIndices = GetVisibleChildDataRowIndices(grid, visibleIndex, out grouppedRowsCount, out checkedInvisibleIndecesCount, out checkedVisibleIndecesCount, out checkState);

        checkBox.CheckState = checkState;
        checkBox.JSProperties["cpGroupValue"] = Grid.GetRowValues(visibleIndex, "C1");
        checkBox.JSProperties["cpVisibleChildDataRowIndices"] = visibleChildDataRowIndices;
        checkBox.JSProperties["cpGrouppedRowsCount"] = grouppedRowsCount;
        checkBox.JSProperties["cpCheckedInvisibleIndecesCount"] = checkedInvisibleIndecesCount;
        checkBox.JSProperties["cpCheckedVisibleIndecesCount"] = checkedVisibleIndecesCount;

        if (!grid.IsRowExpanded(visibleIndex))
            checkBox.Enabled = false;
    }

    protected List<int> GetVisibleChildDataRowIndices(ASPxGridView grid, int groupVisibleIndex, out int grouppedRowsCount, out int checkedInvisibleIndecesCount, out int checkedVisibleIndecesCount,
                                                      out CheckState checkState)
    {
        var result = new List<int>();
        var groupLevel = grid.GetRowLevel(groupVisibleIndex);
        var visibleStart = grid.VisibleStartIndex;
        var visibleEnd = visibleStart + Math.Min(grid.VisibleRowCount - visibleStart, grid.SettingsPager.PageSize) - 1;
        var visibleIndex = groupVisibleIndex + 1;

        grouppedRowsCount = 0;
        checkedInvisibleIndecesCount = 0;
        checkedVisibleIndecesCount = 0;

        while (grid.GetRowLevel(visibleIndex) > groupLevel)
        {
            var checkValue = (bool)grid.GetRowValues(visibleIndex, GroupCheckBoxFieldName);

            if (!grid.IsGroupRow(visibleIndex) && ((visibleIndex >= visibleStart && visibleIndex <= visibleEnd) && grid.GetRowLevel(visibleIndex) != groupLevel))
            {
                if (checkValue)
                    checkedVisibleIndecesCount++;

                result.Add(visibleIndex);
            }
            else
            {
                if (checkValue)
                    checkedInvisibleIndecesCount++;
            }

            visibleIndex++;
            grouppedRowsCount++;
        }

        checkState = GetCheckState(checkedVisibleIndecesCount + checkedInvisibleIndecesCount, grouppedRowsCount);

        return result;
    }

    protected CheckState GetCheckState(int checkedRowsCount, int grouppedRowsCount)
    {
        if (checkedRowsCount == 0)
            return CheckState.Unchecked;
        else if (checkedRowsCount == grouppedRowsCount)
            return CheckState.Checked;
        else
            return CheckState.Indeterminate;
    }

    protected void Grid_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
    {
        foreach (ASPxDataUpdateValues ASPxDataUpdateValues in e.UpdateValues)
            Data.FirstOrDefault(item => item.ID == (int)ASPxDataUpdateValues.Keys["ID"]).C2 = (bool)ASPxDataUpdateValues.NewValues["C2"];

        e.Handled = true;
        Grid.CancelEdit();
    }

    public class Item
    {
        public int ID { get; set; }

        public DateTime C1 { get; set; }

        public bool C2 { get; set; }

        public Item(int id, DateTime c1, bool c2)
        {
            ID = id;
            C1 = c1;
            C2 = c2;
        }
    }
}