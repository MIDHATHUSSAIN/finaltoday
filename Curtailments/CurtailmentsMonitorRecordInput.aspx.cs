using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Curtailments_CurtailmentsMonitorRecordInput : System.Web.UI.Page
{
    private DataTable DateTimeTable
    {
        get
        {
            if (ViewState["DateTimeTable"] == null)
            {
                DataTable DT = new DataTable();
                DT.Columns.Add("UserID", typeof(int));
                DT.Columns.Add("DateFrom", typeof(DateTime));
                DT.Columns.Add("DateTo", typeof(DateTime));
                DT.Columns.Add("WTGName", typeof(string));
                DT.Columns.Add("SetPoint", typeof(decimal));
                DT.Columns.Add("SystemNumbers", typeof(string));

                ViewState["DateTimeTable"] = DT;
            }

            return (DataTable)ViewState["DateTimeTable"];
        }
        set
        {
            ViewState["DateTimeTable"] = value;
        }
    }
    private DataTable SubDateTimeTable
    {
        get
        {
            if (ViewState["SubDateTimeTable"] == null)
            {
                DataTable SDT = new DataTable();
                SDT.Columns.Add("DateFrom", typeof(DateTime));
                SDT.Columns.Add("DateTo", typeof(DateTime));
                SDT.Columns.Add("WTGName", typeof(string));
                SDT.Columns.Add("SubSetPoint", typeof(decimal));
                SDT.Columns.Add("SystemNumbers", typeof(string));
                ViewState["SubDateTimeTable"] = SDT;
            }

            return (DataTable)ViewState["SubDateTimeTable"];
        }
        set
        {
            ViewState["SubDateTimeTable"] = value;
        }
    }




    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindWTG();
        }
    }

    //protected void btnAdd_Click(object sender, EventArgs e)
    //{
    //    DateTime dateFrom;
    //    DateTime dateTo;

    //    SubSetInputs.Visible = false;
    //    DateTime DateFrom;
    //    DateTime DateTo;
    //    decimal SetPoint = (decimal)SetPoit.Value;
    //    if (SetPoint > 50 || SetPoint <= 0)
    //    {
    //        ShowAlert("Set Point Must Be In The Range 1 to 50 ");
    //        return;
    //    }
    //    DateFrom = From.SelectedDate.Value;

    //    DateTo = To.SelectedDate.Value;


    //    if (string.IsNullOrEmpty(From.SelectedDate.ToString()) || string.IsNullOrEmpty(To.SelectedDate.ToString()))
    //    {
    //        ShowAlert("please select start and end date ");
    //        return;
    //    }
    //    if(From.SelectedDate.Value.Date > DateTime.Now.Date)
    //    {
    //        ShowAlert("Date must be current");
    //        return;
    //    }

    //    if (!ValidateDates(DateFrom, DateTo, out dateFrom, out dateTo))
    //        return;
    //    var collection = WTG.CheckedItems;

    //    if (collection.Count == 0)
    //    {
    //        ShowAlert("Please select at least one item from the WTG list.");
    //        return;
    //    }

    //    bool exist = false;
    //    for (int i = 0; i < DateTimeTable.Columns.Count; i++)
    //    {

    //        if (DateTimeTable.Columns[i].ColumnName == "WTGName")
    //        {
    //            exist = true;
    //        }
    //    }
    //    if (exist == false)
    //    {
    //        DateTimeTable.Columns.Add("WTGName", typeof(string));
    //    }


    //    foreach (var item in collection)
    //    {
    //        string wtgName = item.Text;
    //        string wtgId = item.Value;

    //        bool exists = DateTimeTable.AsEnumerable().Any(row =>
    //           row.Field<string>("SystemNumbers") == wtgId &&
    //           row.Field<DateTime>("DateFrom") == DateFrom &&
    //           row.Field<DateTime>("DateTo") == DateTo
    //       );

    //        if (exists)
    //        {
    //            //ShowAlert("The WTG " + wtgName + "with the specified date range already exists in the table.");
    //            ShowAlert("Some WTGs with the specified date range already exists in the table.");
    //            continue;
    //        }



    //        DataRow newRow = DateTimeTable.NewRow();
    //        newRow["UserID"] = 123; /*Session["ID"];*/
    //        newRow["SystemNumbers"] = wtgId;
    //        newRow["WTGName"] = wtgName;
    //        newRow["DateFrom"] = DateFrom;
    //        newRow["DateTo"] = DateTo;
    //        newRow["SetPoint"] = SetPoint;
    //        DateTimeTable.Rows.Add(newRow);

    //        //Confirem from rayan bhai

    //        ViewState["topDateFrom"] = DateFrom;
    //        gridView.DataSource = DateTimeTable;
    //        gridView.DataBind();

    //    }

    //}
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        DateTime dateFrom;
        DateTime dateTo;

        SubSetInputs.Visible = false;
        DateTime DateFrom;
        DateTime DateTo;
      
        if (!DateTime.TryParse(From.Value, out DateFrom) || !DateTime.TryParse(To.Value, out DateTo))
        {
            ShowAlert("Please select start and end dates.");
            return;
        }

        if (DateFrom.Date > DateTime.Now.Date)
        {
            ShowAlert("Date must be current or early");
            return;
        }
        if (DateTo.Date > DateTime.Now.Date)
        {
            ShowAlert("Date must be current or early.");
            return;
        }

        decimal SetPoint = (decimal)SetPoit.Value;
        if (SetPoint > 50 || SetPoint <= 0)
        {
            ShowAlert("Set Point Must Be In The Range 1 to 50 ");
            return;
        }
        if (!ValidateDates(DateFrom, DateTo, out dateFrom, out dateTo))
            return;

        var collection = WTG.CheckedItems;
        if (collection.Count == 0)
        {
            ShowAlert("Please select at least one item from the WTG list.");
            return;
        }

        bool exist = false;
        for (int i = 0; i < DateTimeTable.Columns.Count; i++)
        {
            if (DateTimeTable.Columns[i].ColumnName == "WTGName")
            {
                exist = true;
            }
        }
        if (!exist)
        {
            DateTimeTable.Columns.Add("WTGName", typeof(string));
        }

        foreach (var item in collection)
        {
            string wtgName = item.Text;
            string wtgId = item.Value;

            bool exists = DateTimeTable.AsEnumerable().Any(row =>
               row.Field<string>("SystemNumbers") == wtgId &&
               row.Field<DateTime>("DateFrom") == DateFrom &&
               row.Field<DateTime>("DateTo") == DateTo
           );

            if (exists)
            {
                ShowAlert("Some WTGs with the specified date range already exist in the table.");
                continue;
            }

            DataRow newRow = DateTimeTable.NewRow();
            newRow["UserID"] = 123; /*Session["ID"];*/
            newRow["SystemNumbers"] = wtgId;
            newRow["WTGName"] = wtgName;
            newRow["DateFrom"] = DateFrom;
            newRow["DateTo"] = DateTo;
            newRow["SetPoint"] = SetPoint;
            DateTimeTable.Rows.Add(newRow);

            ViewState["topDateFrom"] = DateFrom;
            gridView.DataSource = DateTimeTable;
            gridView.DataBind();
        }
    }
    protected void AddSubSetPoint(object sender, CommandEventArgs e)
    {
        int rowIndex = Convert.ToInt32(e.CommandArgument);

        ViewState["ClickedRowDate"] = DateTimeTable.Rows[rowIndex]["DateFrom"];

        ViewState["index"] = rowIndex;

        ViewState["ClickedRowSystemNumbers"] = DateTimeTable.Rows[rowIndex]["SystemNumbers"];

        ViewState["ClickedRowDateWTGName"] = DateTimeTable.Rows[rowIndex]["WTGName"];

        abc();
        SubSetInputs.Visible = true;

    }

    protected void BindWTG()
    {


        SqlConnection con = new SqlConnection();
        con.ConnectionString = ConfigurationManager.ConnectionStrings["APMSH"].ConnectionString;

        SqlDataAdapter DA = new SqlDataAdapter();
        DA.SelectCommand = new SqlCommand();
        DA.SelectCommand.Connection = con;

        DA.SelectCommand.CommandType = CommandType.StoredProcedure;

        DA.SelectCommand.CommandText = "SP_GetWTGsList";

        DataTable dT = new DataTable();
        con.Open();
        DA.Fill(dT);
        DA.Dispose();
        con.Close();
        con.Dispose();


        WTG.DataTextField = "NAME";
        WTG.DataValueField = "ID";

        WTG.DataSource = dT;
        WTG.DataBind();


    }

    protected void abc()
    {
        var uniqueWtg = DateTimeTable.AsEnumerable().Select(r => new WTGItem
        {
            Name = r.Field<string>("WTGName"),
            SystemNumber = r.Field<string>("SystemNumbers")
        }).GroupBy(i => i.Name).Select(g => g.First()).ToList();

        RadComboBox1.DataSource = uniqueWtg;
        RadComboBox1.DataTextField = "Name";
        RadComboBox1.DataValueField = "SystemNumber";
        RadComboBox1.DataBind();
    }
    protected void InsertVisibility()
    {
        if (DateTimeTable.Rows.Count != 0)
        {
            Insert.Visible = true;
        }
        else if (SubDateTimeTable.Rows.Count != 0)
        {
            Insert.Visible = true;
        }
        else
        {
            Insert.Visible = false;
        }
    }

    //protected void Button2_Click(object sender, EventArgs e)
    //{
    //    DateTime dateFrom;
    //    DateTime dateTo;
    //    DateTime SubSetPointDate = RadDateTimePicker1.SelectedDate.Value;
    //    DateTime SubSetPointEndDate = RadDateT.SelectedDate.Value;
    //    decimal SubSEtPoint = (decimal)RadNumericTextBox1.Value;

    //    if (SubSEtPoint > 50 || SubSEtPoint <= 0)
    //    {
    //        ShowAlert(" Sub Set Point Must Be In The Range 1 to 50 ");
    //        return;
    //    }

    //    if (string.IsNullOrEmpty(RadDateTimePicker1.SelectedDate.ToString()) || string.IsNullOrEmpty(RadDateT.SelectedDate.ToString()))
    //    {
    //        ShowAlert("Please choose the starting or Ending date for the Sub Set Point");
    //        return;
    //    }
    //    //var collection = RadComboBox1.CheckedItems;

    //    ViewState["SubStartDate"] = SubSetPointDate;
    //    ViewState["SubEndDate"] = SubSetPointEndDate;

    //    var collectionnn = RadComboBox1.CheckedItems;

    //    if (collectionnn.Count == 0)
    //    {
    //        ShowAlert("Please select at least one item from the WTG list.");
    //        return;
    //    }
    //    DateTime MainStartDate = (DateTime)ViewState["ClickedRowDate"];

    //    if (!ValidateDates(MainStartDate, SubSetPointDate, out dateFrom, out dateTo))
    //    {
    //        ShowAlert("Sub Set Point date can not be earlier than Set Point date");
    //        return;
    //    }

    //    else if (!ValidateDates(MainStartDate, SubSetPointEndDate, out dateFrom, out dateTo))
    //    {
    //        ShowAlert("Sub Set Point s date can not be earlier than Set Point date");
    //        return;
    //    }

    //    else if (!ValidateDates(SubSetPointDate, SubSetPointEndDate, out dateFrom, out dateTo))
    //    {
    //        ShowAlert("Sub Set Point ending date can't be earlier than Sub Set Point statrting date");
    //        return;
    //    }

    //    foreach (var items in collectionnn)
    //    {
    //        string wtgName = items.Text;
    //        string wtgId = items.Value;
    //        bool exists = SubDateTimeTable.AsEnumerable().Any(row =>
    //               row.Field<string>("SystemNumbers") == wtgId &&
    //               row.Field<DateTime>("DateFrom") == SubSetPointDate &&
    //               //row.Field<DateTime>("DateTo") == SubSetPointEndDate &&
    //               row.Field<string>("WTGName") == wtgName
    //           );

    //        if (exists)
    //        {
    //            //ShowAlert("The WTG " + wtgName + "with the specified date range already exists in the table.");
    //            ShowAlert("Some WTGs with the specified date range already exists in the table.");

    //            continue;
    //        }


    //        DataRow newRow = SubDateTimeTable.NewRow();

    //        newRow["SystemNumbers"] = wtgId;
    //        newRow["WTGName"] = wtgName;
    //        newRow["DateFrom"] = SubSetPointDate;
    //        newRow["DateTo"] = SubSetPointEndDate;
    //        newRow["SubSetPoint"] = SubSEtPoint;
    //        SubDateTimeTable.Rows.Add(newRow);
    //        RadGrid1.DataSource = SubDateTimeTable;
    //        RadGrid1.DataBind();

    //    }

    //    int rowindex = (int)ViewState["index"];
    //    DateTime abc = (DateTime)ViewState["ClickedRowDate"];



    //    //DateTimeTable.Rows[rowindex]["DateTo"] = ViewState["SubEndDate"];

    //    var selectedWTGNames = RadComboBox1.CheckedItems.Select(i => i.Text).ToList();


    //    foreach (DataRow row in DateTimeTable.Rows)
    //    {
    //        string wtgName = row["WTGName"].ToString();
    //        DateTime dateFromm = Convert.ToDateTime(row["DateFrom"]);

    //        // Agar row ka WTGName selected items me hai aur DateFrom `abc` ke barabar hai
    //        if (selectedWTGNames.Contains(wtgName) && dateFromm == abc)
    //        {
    //            row["DateTo"] = ViewState["SubEndDate"]; // `DateTo` ko update karna
    //        }
    //    }


    //    gridView.DataSource = DateTimeTable;
    //    gridView.DataBind();
    //    //}

    //    InsertVisibility();
    //}





    //protected void Button2_Click(object sender, EventArgs e)
    //{
    //    DateTime dateFrom;
    //    DateTime dateTo;
    //      DateTime SubSetPointDate = RadDateTimePicker1.SelectedDate.Value;
    //    DateTime SubSetPointEndDate = RadDateT.SelectedDate.Value;
    //    decimal SubSEtPoint = (decimal)RadNumericTextBox1.Value;

    //    if (SubSEtPoint > 50 || SubSEtPoint <= 0)
    //    {
    //        ShowAlert(" Sub Set Point Must Be In The Range 1 to 50 ");
    //        return;
    //    }

    //    if (string.IsNullOrEmpty(RadDateTimePicker1.SelectedDate.ToString()) || string.IsNullOrEmpty(RadDateT.SelectedDate.ToString()))
    //    {
    //        ShowAlert("Please choose the starting or Ending date for the Sub Set Point");
    //        return;
    //    }
    //    //var collection = RadComboBox1.CheckedItems;

    //    ViewState["SubStartDate"] = SubSetPointDate;
    //    ViewState["SubEndDate"] = SubSetPointEndDate;

    //    DateTime MainStartDate = (DateTime)ViewState["ClickedRowDate"];

    //    if (!ValidateDates(MainStartDate, SubSetPointDate, out dateFrom, out dateTo))
    //    {
    //        ShowAlert("Sub Set Point date can't be earlier than Set Point date.");
    //        return;
    //    }

    //    if(!ValidateDates(SubSetPointDate, SubSetPointEndDate, out dateFrom, out dateTo))
    //    {
    //        ShowAlert("Sub Set Point ending date can't be earlier than Sub Set Point statrting date.");
    //        return;
    //    }


    //    //bool exists = SubDateTimeTable.AsEnumerable().Any(row =>
    //    //       row.Field<string>("SystemNumbers") == wtgId &&
    //    //       row.Field<DateTime>("DateFrom") == DateFrom &&
    //    //       row.Field<DateTime>("DateTo") == DateTo
    //    //   );

    //    //if (exists)
    //    //{
    //    //    //ShowAlert("The WTG " + wtgName + "with the specified date range already exists in the table.");
    //    //    ShowAlert("Some WTGs with the specified date range already exists in the table.");
    //    //    return;
    //    //}

    //    DataRow newRow = SubDateTimeTable.NewRow();

    //    newRow["SystemNumbers"] = ViewState["ClickedRowSystemNumbers"];
    //    newRow["WTGName"] = ViewState["ClickedRowDateWTGName"];
    //    newRow["DateFrom"] = SubSetPointDate;

    //    newRow["SubSetPoint"] = SubSEtPoint;
    //    SubDateTimeTable.Rows.Add(newRow);

    //    RadGrid1.DataSource = SubDateTimeTable;
    //    RadGrid1.DataBind();

    //    //}

    //    InsertVisibility();
    //}

    protected void Button2_Click(object sender, EventArgs e)
    {
        DateTime dateFrom;
        DateTime dateTo;

        //DateTime SubSetPointDate = RadDateTimePicker1.SelectedDate.Value;
        //DateTime SubSetPointEndDate = RadDateT.SelectedDate.Value;
        DateTime SubSetPointDate;
        DateTime SubSetPointEndDate;
        if (RadNumericTextBox1.Value == null)
        {
            ShowAlert("Please Enetr Sub Set Point");
            return;
        }
        decimal SubSetPoint = (decimal)RadNumericTextBox1.Value;
         
        if (!DateTime.TryParse(RadDateTimePicker1.Value, out  SubSetPointDate) ||
            !DateTime.TryParse(RadDateT.Value, out  SubSetPointEndDate))
        {
            ShowAlert("Please choose the starting or ending date for the Sub Set Point");
            return;
        }

        

        if (SubSetPoint > 50 || SubSetPoint <= 0)
        {
            ShowAlert("Sub Set Point Must Be In The Range 1 to 50");
            return;
        }

        ViewState["SubStartDate"] = SubSetPointDate;
        ViewState["SubEndDate"] = SubSetPointEndDate;

        var collectionnn = RadComboBox1.CheckedItems;

        if (collectionnn.Count == 0)
        {
            ShowAlert("Please select at least one item from the WTG list.");
            return;
        }

        DateTime MainStartDate = (DateTime)ViewState["ClickedRowDate"];

        if (!ValidateDates(MainStartDate, SubSetPointDate, out dateFrom, out dateTo))
        {
            ShowAlert("Sub Set Point date cannot be earlier than Set Point date");
            return;
        }
        else if (!ValidateDates(MainStartDate, SubSetPointEndDate, out dateFrom, out dateTo))
        {
            ShowAlert("Sub Set Point's date cannot be earlier than Set Point date");
            return;
        }
        else if (!ValidateDates(SubSetPointDate, SubSetPointEndDate, out dateFrom, out dateTo))
        {
            ShowAlert("Sub Set Point ending date can't be earlier than Sub Set Point starting date");
            return;
        }

        foreach (var items in collectionnn)
        {
            string wtgName = items.Text;
            string wtgId = items.Value;

            bool exists = SubDateTimeTable.AsEnumerable().Any(row =>
                row.Field<string>("SystemNumbers") == wtgId &&
                row.Field<DateTime>("DateFrom") == SubSetPointDate &&
                row.Field<string>("WTGName") == wtgName
            );

            if (exists)
            {
                ShowAlert("Some WTGs with the specified date range already exist in the table.");
                continue;
            }

            DataRow newRow = SubDateTimeTable.NewRow();
            newRow["SystemNumbers"] = wtgId;
            newRow["WTGName"] = wtgName;
            newRow["DateFrom"] = SubSetPointDate;
            newRow["DateTo"] = SubSetPointEndDate;
            newRow["SubSetPoint"] = SubSetPoint;
            SubDateTimeTable.Rows.Add(newRow);

            RadGrid1.DataSource = SubDateTimeTable;
            RadGrid1.DataBind();
        }

        //int rowindex = (int)ViewState["index"];
        DateTime abc = (DateTime)ViewState["ClickedRowDate"];

        var selectedWTGNames = RadComboBox1.CheckedItems.Select(i => i.Text).ToList();

        foreach (DataRow row in DateTimeTable.Rows)
        {
            string wtgName = row["WTGName"].ToString();
            DateTime dateFromm = Convert.ToDateTime(row["DateFrom"]);

            if (selectedWTGNames.Contains(wtgName) && dateFromm == abc)
            {
                row["DateTo"] = ViewState["SubEndDate"]; 
            }
        }

        gridView.DataSource = DateTimeTable;
        gridView.DataBind();

        InsertVisibility();
    }
    protected void btnRemove_Command(object sender, CommandEventArgs e)
    {
        int rowIndex = Convert.ToInt32(e.CommandArgument);
        if (rowIndex >= 0 && rowIndex < DateTimeTable.Rows.Count)
        {
            DateTime geetindate = (DateTime)DateTimeTable.Rows[rowIndex]["DateFrom"];
            DateTimeTable.Rows.RemoveAt(rowIndex);
            gridView.DataSource = DateTimeTable;
            
            DateTime TopDate = (DateTime)ViewState["topDateFrom"];
            if (TopDate == geetindate)
            {
                if (DateTimeTable.Rows.Count != 0)
                {
                    ViewState["topDateFrom"] = (DateTime)DateTimeTable.Rows[rowIndex - 1]["DateFrom"];

                }
                else
                {
                    ViewState["topDateFrom"] = null;
                }
            }
            gridView.DataBind();
            InsertVisibility();
        }

    }

    protected void btnRemove(object sender, CommandEventArgs e)
    {
        int rowIndex = Convert.ToInt32(e.CommandArgument);
        if (rowIndex >= 0 && rowIndex < SubDateTimeTable.Rows.Count)
        {
            SubDateTimeTable.Rows.RemoveAt(rowIndex);
            DateTime geetindate = (DateTime)SubDateTimeTable.Rows[rowIndex]["DateFrom"];
            DateTime SubDate = (DateTime)ViewState["SubStartDate"];
            if (SubDate == geetindate)
            {
                if (SubDateTimeTable.Rows.Count != 0)
                {
                    ViewState["SubStartDate"] = (DateTime)SubDateTimeTable.Rows[rowIndex - 1]["DateFrom"];

                }
                else
                {
                    ViewState["SubStartDate"] = null;
                }
            }
            RadGrid1.DataSource = SubDateTimeTable;
            RadGrid1.DataBind();
            InsertVisibility();
        }

    }

    private void ShowAlert(string message)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('" + message + "');", true);
    }
    private bool ValidateDates(DateTime abc, DateTime xyz, out DateTime dateFrom, out DateTime dateTo)
    {

        dateFrom = abc;
        dateTo = xyz;


        if (dateFrom >= dateTo)
        {
            ShowAlert("Start date/time must be earlier than end date/time.");
            return false;
        }

        return true;
    }
    protected void Insert_Click1(object sender, EventArgs e)
    {
       

        for (int i = 0; i < DateTimeTable.Rows.Count; i++)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["midhat"].ConnectionString;

            SqlDataAdapter DA = new SqlDataAdapter();
            DA.SelectCommand = new SqlCommand();
            DA.SelectCommand.Connection = con;

            DA.SelectCommand.CommandType = CommandType.StoredProcedure;

            DA.SelectCommand.CommandText = "SP_DummyDataForCurtailmentT";
            

          
            string wtgName = DateTimeTable.Rows[i]["WTGName"].ToString();
            string sysnum = DateTimeTable.Rows[i]["SystemNumbers"].ToString();
            DateTime ParentDatee = (DateTime)DateTimeTable.Rows[i]["DateFrom"];

            DateTime SubSetPointDatee = (DateTime)DateTimeTable.Rows[i]["DateTo"];
            decimal PSetPoint = (decimal)DateTimeTable.Rows[i]["SetPoint"];

            //var exists = SubDateTimeTable.AsEnumerable().Any(row =>

            //  row.Field<DateTime>("DateTo") == SubSetPointDatee &&
            //  row.Field<string>("WTGName") == wtgName

            //  ).Select( r => new SubTableDataa{

            //      SDateFromm = r.Field<string>("DateFrom"),
            //      SubSetPoi = r.Field<string>("SubSetPoint")


            //  });
            bool www = true;
            DataTable aadd = new DataTable();

            for (int j = 0; j < SubDateTimeTable.Rows.Count; j++)
            {
                
                string aa = SubDateTimeTable.Rows[j]["WTGName"].ToString();
                
                DateTime SubSetd = (DateTime)SubDateTimeTable.Rows[j]["DateTo"];
            
                if ((aa == wtgName) && (SubSetd == SubSetPointDatee))
                {
                    string sysnumM = SubDateTimeTable.Rows[j]["SystemNumbers"].ToString();
                    DateTime SubFromtime = (DateTime)SubDateTimeTable.Rows[j]["DateFrom"];
                    decimal SubSetPoint = (decimal)SubDateTimeTable.Rows[j]["SubSetPoint"];
                    DA.SelectCommand.Parameters.AddWithValue("@SystemNumbers",sysnumM);
                    DA.SelectCommand.Parameters.AddWithValue("@WTGName", wtgName);
                    DA.SelectCommand.Parameters.AddWithValue("@PDateFrom", ParentDatee);
                    DA.SelectCommand.Parameters.AddWithValue("@PDateTo", SubSetPointDatee);
                    DA.SelectCommand.Parameters.AddWithValue("@SDateFrom", SubFromtime);
                    DA.SelectCommand.Parameters.AddWithValue("@SDateTo", SubSetPointDatee);
                    DA.SelectCommand.Parameters.AddWithValue("@SetPoint", PSetPoint);
                    DA.SelectCommand.Parameters.AddWithValue("@SubSetPoint", SubSetPoint);

                    con.Open();
                    DA.Fill(aadd);
                    DA.Dispose();
                    con.Close();
                    con.Dispose();
                    www = false;

                }



            }


            if (www)
            {
                DA.SelectCommand.Parameters.AddWithValue("@SystemNumbers", sysnum);
                DA.SelectCommand.Parameters.AddWithValue("@WTGName", wtgName);
                DA.SelectCommand.Parameters.AddWithValue("@PDateFrom", ParentDatee);
                DA.SelectCommand.Parameters.AddWithValue("@PDateTo", SubSetPointDatee);
                DA.SelectCommand.Parameters.AddWithValue("@SDateFrom", 0);
                DA.SelectCommand.Parameters.AddWithValue("@SDateTo", 0);
                DA.SelectCommand.Parameters.AddWithValue("@SetPoint", PSetPoint);
                DA.SelectCommand.Parameters.AddWithValue("@SubSetPoint", 0);
                con.Open();
                DA.Fill(aadd);
                DA.Dispose();
                con.Close();
                con.Dispose();
            }
     
        }
        






        //foreach (DataRow row in DateTimeTable.Rows)
        //{
        //    string wtgName = row["WTGName"].ToString();
        //    DateTime dateFromm = Convert.ToDateTime(row["DateFrom"]);

        //    if (SubDateTimeTable.Contains(wtgName) && )
        //    {
        //        row["DateTo"] = ViewState["SubEndDate"];
        //    }
        //}







        //DateTime dateFrom;
        //DateTime dateTo;

        //RadDateT.Visible = true;
        //labelEndDate.Visible = true;

        //if (string.IsNullOrEmpty(RadDateT.SelectedDate.ToString()))
        //{
        //    ShowAlert("please select end date");
        //    return;
        //}
        //else
        //{
        //    DateTime FinalEndDate = RadDateT.SelectedDate.Value;



        //    if (ViewState["topDateFrom"] != null)
        //    {
        //        DateTime TopStartDate = (DateTime)ViewState["topDateFrom"];
        //        if (!ValidateDates(TopStartDate, FinalEndDate, out dateFrom, out dateTo))
        //        {
        //            ShowAlert("End time must be after the start time ");
        //            return;
        //        }

        //    }

        //    if (ViewState["SubStartDate"] != null)
        //    {
        //        DateTime TopStartDate = (DateTime)ViewState["SubStartDate"];
        //        if (!ValidateDates(TopStartDate, FinalEndDate, out  dateFrom, out dateTo))
        //        {
        //            ShowAlert("End time must be after the start time ");
        //            return;
        //        }

        //    }





    }

}

public class WTGItem
{

    public string Name { get; set; }
    public string SystemNumber { get; set; }

}

public class SubTableDataa
{

    public DateTime SDateFromm { get; set; }
    //public DateTime SDateTo { get; set; }

    public decimal SubSetPoi { get; set; }

}

