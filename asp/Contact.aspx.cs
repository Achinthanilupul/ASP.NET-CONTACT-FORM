using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace asp
{
    public partial class Contact : System.Web.UI.Page
    {
        SqlConnection sqlCon = new SqlConnection(@"Data Source=SD3;Initial Catalog=ASPCRUD;Integrated Security=True");
        private object commandType;

        protected void Page_Load(object sender, EventArgs e)
        {
            sqlCon.Open();
            if (!IsPostBack)
            {
                btnDelete.Enabled = false;
                FillGridView();

            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        public void Clear()
        {
            hfContactID.Value = "";
            txtName.Text = txtMobile.Text = txtAddress.Text = "";
            lblSuccessMessage.Text = lblErrorMessage.Text = "";
            btnSave.Text = "save";
            btnDelete.Enabled = false;


        }

       protected void btnSave_Click(object sender, EventArgs e)
{
    if (sqlCon.State == ConnectionState.Closed)
        sqlCon.Open();

    SqlCommand sqlcmd = new SqlCommand("ContactCreateOrUpdate", sqlCon);
    sqlcmd.CommandType = CommandType.StoredProcedure;
    sqlcmd.Parameters.AddWithValue("@ContactID", (hfContactID.Value == "" ? 0 : Convert.ToInt32(hfContactID.Value)));
    sqlcmd.Parameters.AddWithValue("@Name", txtName.Text.Trim());
    sqlcmd.Parameters.AddWithValue("@Mobile", txtMobile.Text.Trim());
    sqlcmd.Parameters.AddWithValue("@Address", txtAddress.Text.Trim());
    sqlcmd.ExecuteNonQuery();
    sqlCon.Close();
            string contactID = hfContactID.Value;
    Clear();
    if (contactID == "")
        lblSuccessMessage.Text = "Saved Successfully";
    else
        lblSuccessMessage.Text = "Updated Successfully";
            FillGridView();

}
        void FillGridView()
        {
            if (sqlCon.State == ConnectionState.Closed)
                sqlCon.Open();
            SqlDataAdapter sqlDa = new SqlDataAdapter("ContactViewAll", sqlCon);
            sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataTable dtbl = new DataTable();
            sqlDa.Fill(dtbl);
            sqlCon.Close();
            gvContact.DataSource = dtbl;
            gvContact.DataBind();
        }
        protected void lnk_OnClick(object sender, EventArgs e)
        {
            int contactID = Convert.ToInt32((sender as LinkButton).CommandArgument);
            if (sqlCon.State == ConnectionState.Closed)
                sqlCon.Open();
            SqlDataAdapter sqlDa = new SqlDataAdapter("ContactViewByID", sqlCon);
            sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDa.SelectCommand.Parameters.AddWithValue("@ContactID", contactID);
            DataTable dtbl = new DataTable();
            sqlDa.Fill(dtbl);
            sqlCon.Close();
            hfContactID.Value = contactID.ToString();
            txtName.Text = dtbl.Rows[0]["Name"].ToString();
            txtMobile.Text = dtbl.Rows[0]["Mobile"].ToString();
            txtAddress.Text = dtbl.Rows[0]["Address"].ToString();
            btnSave.Text = "Update";
            btnDelete.Enabled = true;
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (sqlCon.State == ConnectionState.Closed)
                sqlCon.Open();
            SqlCommand sqlcmd = new SqlCommand("ContactDeleteByID", sqlCon);
            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.Parameters.AddWithValue("@ContactID", Convert.ToInt32(hfContactID.Value));
            sqlcmd.ExecuteNonQuery();
            sqlCon.Close();
            Clear();
            FillGridView();
            lblSuccessMessage.Text= "Successfully Deleted";

        }
    }
}
