<!-- default file list -->
*Files to look at*:

* [Default.aspx](./CS/Default.aspx) (VB: [Default.aspx](./VB/Default.aspx))
* [Default.aspx.cs](./CS/Default.aspx.cs) (VB: [Default.aspx.vb](./VB/Default.aspx.vb))
* [Default.aspx.vb](./CS/Default.aspx.vb) (VB: [Default.aspx.vb](./VB/Default.aspx.vb))
<!-- default file list end -->
# ASPxGridView - How to select group rows in batch edit mode with the Select All ASPxCheckBox


<p>This example shows how to add <strong><em>"Select All"</em></strong> ASPxCheckBox to select all rows, which are visible on ASPxGridView's current page, within some groups. </p>
<p>After checking the <strong><em>"Select All"</em></strong> ASPxCheckBox, the group items, which are visible within ASPxGridView's current page, will change its checked state. Otherwise, this checking has no effect on the invisible items within its group.</p>
<p>The<em> </em><strong><em>"Select All"</em></strong> ASPxCheckBox, which are placed inside <a href="https://documentation.devexpress.com/AspNet/DevExpress.Web.GridViewTemplates.GroupRowContent.property">ASPxGridView.Templates.GroupRowContent</a>, change its <a href="https://documentation.devexpress.com/AspNet/DevExpress.Web.ASPxCheckBox.CheckState.property">ASPxCheckBox.CheckState</a> according to the checked states of the items within its group. </p>

<br/>


