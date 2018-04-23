<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.13.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>



<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        var GroupCheckBoxFieldName = "C2";

        function GroupCheckBox_Init(s, e) {
            RegisterGroupCheckBox(s);
        }

        function GroupCheckBox_CheckedChanged(s, e) {
            var visibleIndices = s.cpVisibleChildDataRowIndices;
            var checkedValue = (s.cpCheckedVisibleIndecesCount > 0) ? false : true;

            for (var i = 0; i < visibleIndices.length; i++)
                grid.batchEditApi.SetCellValue(visibleIndices[i], GroupCheckBoxFieldName, checkedValue);

            s.cpCheckedVisibleIndecesCount = (checkedValue === false) ? 0 : visibleIndices.length;
            s.SetCheckState(CalcGroupCheckBoxValue(s));
        }

        function ColumnCheck_CheckedChanged(s, e) {
            var visibleIndex = grid.batchEditApi.GetEditCellInfo().rowVisibleIndex;

            UpdateGroupCheckBoxCheckedState(visibleIndex, s.GetValue());
        }

        function UpdateGroupCheckBoxCheckedState(dataVisibleIndex, checkValue) {
            var checkBox = FindRelatedGroupCheckBox(dataVisibleIndex);

            if (checkBox) {
                if (!checkValue)
                    checkBox.cpCheckedVisibleIndecesCount--;
                else
                    checkBox.cpCheckedVisibleIndecesCount++;

                checkBox.SetCheckState(CalcGroupCheckBoxValue(checkBox));
            }
        }

        function CalcGroupCheckBoxValue(checkBox) {
            var totalCheckedIndecesCount = checkBox.cpCheckedVisibleIndecesCount + checkBox.cpCheckedInvisibleIndecesCount;

            if (totalCheckedIndecesCount == 0)
                return "Unchecked";
            else if (totalCheckedIndecesCount == checkBox.cpGrouppedRowsCount)
                return "Checked";
            else
                return "Indeterminate";
        }

        function FindRelatedGroupCheckBox(dataVisibleIndex) {
            for (var groupVisibleIndex in grid.groupCheckBoxes) {

                var checkBox = grid.groupCheckBoxes[groupVisibleIndex];

                if (checkBox.cpVisibleChildDataRowIndices.indexOf(dataVisibleIndex) > -1)
                    return checkBox;
            }
            return null;
        }

        function RegisterGroupCheckBox(checkBox) {
            if (!grid.groupCheckBoxes)
                grid.groupCheckBoxes = {};

            grid.groupCheckBoxes[checkBox.cpGroupValue] = checkBox;
        }

        function clearGroupCheckBoxes() {
            delete grid.groupCheckBoxes;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <dx:ASPxGridView ID="Grid" runat="server" KeyFieldName="ID" Width="800" EnableRowsCache="false" ClientInstanceName="grid" OnBatchUpdate="Grid_BatchUpdate">
            <ClientSideEvents RowExpanding="clearGroupCheckBoxes" RowCollapsing="clearGroupCheckBoxes" />
            <Columns>
                <dx:GridViewDataColumn FieldName="ID" ReadOnly="true" />
                <dx:GridViewDataColumn FieldName="C1" GroupIndex="0">
                    <Settings GroupInterval="DateYear" />
                </dx:GridViewDataColumn>
                <dx:GridViewDataCheckColumn FieldName="C2">
                    <PropertiesCheckEdit>
                        <ClientSideEvents CheckedChanged="ColumnCheck_CheckedChanged" />
                    </PropertiesCheckEdit>
                </dx:GridViewDataCheckColumn>
            </Columns>
            <Templates>
                <GroupRowContent>
                    <span><%# Container.GroupText %></span>
                    <span style="float: right">
                        <dx:ASPxCheckBox ID="GroupCheckBox" runat="server" AutoPostBack="false" Text="Check visible rows in group on current page" OnLoad="GroupCheckBox_Load" AllowGrayed="true" 
                            AllowGrayedByClick="false">
                            <ClientSideEvents Init="GroupCheckBox_Init" CheckedChanged="GroupCheckBox_CheckedChanged" />
                        </dx:ASPxCheckBox>
                    </span>
                </GroupRowContent>
            </Templates>
            <Settings ShowGroupPanel="true" VerticalScrollBarMode="Visible" VerticalScrollableHeight="400" />
            <SettingsBehavior AllowFixedGroups="true" />
            <SettingsEditing Mode="Batch" />
            <SettingsPager PageSize="20" />
        </dx:ASPxGridView>
    </form>
</body>
</html>
