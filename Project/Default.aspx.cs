using Npgsql;
using System;
using System.Data;
using System.Linq;
using System.Web.Management;
using System.Web.UI.WebControls;

namespace CombinedProject
{
    public partial class Default : System.Web.UI.Page
    {
        private DatabaseHelper dbHelper = new DatabaseHelper();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserAuthenticated"] == null || !(bool)Session["UserAuthenticated"])
            {
                Response.Redirect("UserAuth.aspx");
            }
            if (!IsPostBack)
            {
                LoadServices();
                LoadInitialChartsData();
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Response.Redirect("UserAuth.aspx");
        }

        private void LoadInitialChartsData()
        {
            // Fetch data for initial charts and render them
            string query1 = "SELECT DISTINCT applicantgender, COUNT(applicantgender) FROM applicationdetails GROUP BY applicantgender";
            string query2 = "SELECT CAST(EXTRACT(YEAR FROM applicationdate) AS INTEGER) AS year, CAST (AVG(extract (epoch from (approveddate - applicationdate))/86400) AS INTEGER)  AS mean_days_difference FROM applicationdetails where applicationstatusid=16 GROUP BY EXTRACT(YEAR FROM applicationdate) ORDER BY year";
            string query3 = "SELECT  EXTRACT(YEAR FROM applicationdate)-EXTRACT(YEAR FROM applicantdob) AS year, COUNT(*) FROM applicationdetails GROUP BY year ORDER BY year";
            string query4 = "SELECT EXTRACT(YEAR FROM applicationdate) AS year, COUNT(*) AS totalcount, SUM(CASE WHEN applicationstatusid = 16 THEN 1 ELSE 0 END) AS approvedcount, COUNT(*) - SUM(CASE WHEN applicationstatusid = 16 THEN 1 ELSE 0 END) AS pendingcount FROM applicationdetails GROUP BY year ORDER BY year";

            DataTable dt1 = dbHelper.ExecuteQuery(query1);
            DataTable dt2 = dbHelper.ExecuteQuery(query2);
            DataTable dt3 = dbHelper.ExecuteQuery(query3);
            DataTable dt4 = dbHelper.ExecuteQuery(query4);

            RenderCharts(dt1, dt2, dt3, dt4);
        }


        private void LoadServices()
        {
            string query = "SELECT servicecode, servicename FROM servicemaster ORDER BY servicecode";
            DataTable dt = dbHelper.ExecuteQuery(query);

            // Create a new DataTable to hold combined service code and name
            DataTable dtCombined = new DataTable();
            dtCombined.Columns.Add("ServiceCodeName");
            dtCombined.Columns.Add("ServiceCode");

            foreach (DataRow row in dt.Rows)
            {
                DataRow newRow = dtCombined.NewRow();
                newRow["ServiceCodeName"] = row["servicecode"].ToString() + " : " + row["servicename"].ToString();
                newRow["ServiceCode"] = row["servicecode"];
                dtCombined.Rows.Add(newRow);
            }

            ddlServices.DataSource = dtCombined;
            ddlServices.DataTextField = "ServiceCodeName";
            ddlServices.DataValueField = "ServiceCode";
            ddlServices.DataBind();
            ddlServices.Items.Insert(0, new ListItem("Select Service", "0"));
        }

        protected void ddlServices_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(ddlServices.SelectedIndex == 0)
            {
                if(ddlDistricts.SelectedIndex != 0)
                {
                    if(ddlSubdivisions.SelectedIndex != 0)
                    {
                        int districtCode = int.Parse(ddlDistricts.SelectedValue);
                        int subdivCode = int.Parse(ddlSubdivisions.SelectedValue);

                        string query1 = "SELECT DISTINCT applicantgender, COUNT(applicantgender) FROM applicationdetails WHERE applicationdistrictcode = @DistrictCode AND applicationsubdivcode = @SubdivCode GROUP BY applicantgender";
                        string query2 = "SELECT CAST(EXTRACT(YEAR FROM applicationdate) AS INTEGER) AS year, CAST (AVG(extract (epoch from (approveddate - applicationdate))/86400) AS INTEGER)  AS mean_days_difference FROM applicationdetails WHERE applicationstatusid=16 and applicationdistrictcode = @DistrictCode AND applicationsubdivcode = @SubdivCode GROUP BY EXTRACT(YEAR FROM applicationdate) ORDER BY year";
                        string query3 = "SELECT  EXTRACT(YEAR FROM applicationdate)-EXTRACT(YEAR FROM applicantdob) AS year, COUNT(*) FROM applicationdetails WHERE applicationdistrictcode = @DistrictCode AND applicationsubdivcode = @SubdivCode GROUP BY year ORDER BY year";
                        string query4 = "SELECT EXTRACT(YEAR FROM applicationdate) AS year, COUNT(*) AS totalcount, SUM(CASE WHEN applicationstatusid = 16 THEN 1 ELSE 0 END) AS approvedcount, COUNT(*) - SUM(CASE WHEN applicationstatusid = 16 THEN 1 ELSE 0 END) AS pendingcount FROM applicationdetails WHERE applicationdistrictcode = @DistrictCode AND applicationsubdivcode = @SubdivCode GROUP BY year ORDER BY year";

                        NpgsqlParameter[] parameters = new NpgsqlParameter[]
                        {
                            new NpgsqlParameter("@DistrictCode", districtCode),
                            new NpgsqlParameter("@SubdivCode", subdivCode)
                        };

                        DataTable dt1 = dbHelper.ExecuteQuery(query1, parameters);
                        DataTable dt2 = dbHelper.ExecuteQuery(query2, parameters);
                        DataTable dt3 = dbHelper.ExecuteQuery(query3, parameters);
                        DataTable dt4 = dbHelper.ExecuteQuery(query4, parameters);

                        RenderCharts(dt1, dt2, dt3, dt4);
                    }
                    else
                    {
                        int districtCode = int.Parse(ddlDistricts.SelectedValue);

                        string query1 = "SELECT DISTINCT applicantgender, COUNT(applicantgender) FROM applicationdetails WHERE applicationdistrictcode = @DistrictCode GROUP BY applicantgender";
                        string query2 = "SELECT CAST(EXTRACT(YEAR FROM applicationdate) AS INTEGER) AS year, CAST (AVG(extract (epoch from (approveddate - applicationdate))/86400) AS INTEGER)  AS mean_days_difference FROM applicationdetails WHERE applicationstatusid=16 and applicationdistrictcode = @DistrictCode GROUP BY EXTRACT(YEAR FROM applicationdate) ORDER BY year";
                        string query3 = "SELECT  EXTRACT(YEAR FROM applicationdate)-EXTRACT(YEAR FROM applicantdob) AS year, COUNT(*) FROM applicationdetails WHERE applicationdistrictcode = @DistrictCode GROUP BY year ORDER BY year";
                        string query4 = "SELECT EXTRACT(YEAR FROM applicationdate) AS year, COUNT(*) AS totalcount, SUM(CASE WHEN applicationstatusid = 16 THEN 1 ELSE 0 END) AS approvedcount, COUNT(*) - SUM(CASE WHEN applicationstatusid = 16 THEN 1 ELSE 0 END) AS pendingcount FROM applicationdetails WHERE applicationdistrictcode = @DistrictCode GROUP BY year ORDER BY year";

                        NpgsqlParameter[] parameters = new NpgsqlParameter[]
                        {
                            new NpgsqlParameter("@DistrictCode", districtCode)
                        };

                        DataTable dt1 = dbHelper.ExecuteQuery(query1, parameters);
                        DataTable dt2 = dbHelper.ExecuteQuery(query2, parameters);
                        DataTable dt3 = dbHelper.ExecuteQuery(query3, parameters);
                        DataTable dt4 = dbHelper.ExecuteQuery(query4, parameters);

                        RenderCharts(dt1, dt2, dt3, dt4);
                    }
                }
                else
                {
                    LoadInitialChartsData();
                }
            }
            else
            {
                LoadDistricts();
                ddlSubdivisions.Items.Clear();
                ddlSubdivisions.Items.Insert(0, new ListItem("Select Subdivision", "0"));

                string query1 = "SELECT DISTINCT applicantgender, COUNT(applicantgender) FROM applicationdetails WHERE servicecode = @ServiceCode GROUP BY applicantgender";
                string query2 = "SELECT CAST(EXTRACT(YEAR FROM applicationdate) AS INTEGER) AS year, CAST (AVG(extract (epoch from (approveddate - applicationdate))/86400) AS INTEGER)  AS mean_days_difference FROM applicationdetails WHERE applicationstatusid=16 and servicecode = @ServiceCode GROUP BY EXTRACT(YEAR FROM applicationdate) ORDER BY year";
                string query3 = "SELECT  EXTRACT(YEAR FROM applicationdate)-EXTRACT(YEAR FROM applicantdob) AS year, COUNT(*) FROM applicationdetails WHERE servicecode = @ServiceCode GROUP BY year ORDER BY year";
                string query4 = "SELECT EXTRACT(YEAR FROM applicationdate) AS year, COUNT(*) AS totalcount, SUM(CASE WHEN applicationstatusid = 16 THEN 1 ELSE 0 END) AS approvedcount, COUNT(*) - SUM(CASE WHEN applicationstatusid = 16 THEN 1 ELSE 0 END) AS pendingcount FROM applicationdetails WHERE servicecode = @ServiceCode GROUP BY year ORDER BY year";

                NpgsqlParameter[] parameters = new NpgsqlParameter[]
                {
                        new NpgsqlParameter("@ServiceCode", int.Parse(ddlServices.SelectedValue))
                };

                DataTable dt1 = dbHelper.ExecuteQuery(query1, parameters);
                DataTable dt2 = dbHelper.ExecuteQuery(query2, parameters);
                DataTable dt3 = dbHelper.ExecuteQuery(query3, parameters);
                DataTable dt4 = dbHelper.ExecuteQuery(query4, parameters);

                RenderCharts(dt1, dt2, dt3, dt4);
            }
        }

        private void LoadDistricts()
        {
            string query = "SELECT distinct ad.applicationdistrictcode, dm.districtname FROM districtmaster dm, applicationdetails ad WHERE ad.applicationdistrictcode=dm.districtcode and ad.servicecode = @ServiceCode ORDER BY applicationdistrictcode";
            NpgsqlParameter[] parameters = new NpgsqlParameter[]
            {
                new NpgsqlParameter("@ServiceCode", int.Parse(ddlServices.SelectedValue))
            };
            DataTable dt = dbHelper.ExecuteQuery(query, parameters);
            ddlDistricts.DataSource = dt;
            ddlDistricts.DataTextField = "districtname";
            ddlDistricts.DataValueField = "applicationdistrictcode";
            ddlDistricts.DataBind();
            ddlDistricts.Items.Insert(0, new ListItem("Select District", "0"));
        }

        protected void ddlDistricts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlDistricts.SelectedIndex == 0)
            {
                ddlServices_SelectedIndexChanged(sender, e);
                ddlSubdivisions.Items.Clear();
                ddlSubdivisions.Items.Insert(0, new ListItem("Select Subdivision", "0"));

            }
            else if (ddlServices.SelectedIndex==0)
            {
                LoadSubdivisions();
                int districtCode = int.Parse(ddlDistricts.SelectedValue);

                string query1 = "SELECT DISTINCT applicantgender, COUNT(applicantgender) FROM applicationdetails WHERE applicationdistrictcode = @DistrictCode GROUP BY applicantgender";
                string query2 = "SELECT CAST(EXTRACT(YEAR FROM applicationdate) AS INTEGER) AS year, CAST (AVG(extract (epoch from (approveddate - applicationdate))/86400) AS INTEGER)  AS mean_days_difference FROM applicationdetails WHERE applicationstatusid=16 and applicationdistrictcode = @DistrictCode GROUP BY EXTRACT(YEAR FROM applicationdate) ORDER BY year";
                string query3 = "SELECT  EXTRACT(YEAR FROM applicationdate)-EXTRACT(YEAR FROM applicantdob) AS year, COUNT(*) FROM applicationdetails WHERE applicationdistrictcode = @DistrictCode GROUP BY year ORDER BY year";
                string query4 = "SELECT EXTRACT(YEAR FROM applicationdate) AS year, COUNT(*) AS totalcount, SUM(CASE WHEN applicationstatusid = 16 THEN 1 ELSE 0 END) AS approvedcount, COUNT(*) - SUM(CASE WHEN applicationstatusid = 16 THEN 1 ELSE 0 END) AS pendingcount FROM applicationdetails WHERE applicationdistrictcode = @DistrictCode GROUP BY year ORDER BY year";

                NpgsqlParameter[] parameters = new NpgsqlParameter[]
                {
                    new NpgsqlParameter("@DistrictCode", districtCode)
                };

                DataTable dt1 = dbHelper.ExecuteQuery(query1, parameters);
                DataTable dt2 = dbHelper.ExecuteQuery(query2, parameters);
                DataTable dt3 = dbHelper.ExecuteQuery(query3, parameters);
                DataTable dt4 = dbHelper.ExecuteQuery(query4, parameters);

                RenderCharts(dt1, dt2, dt3, dt4);
            }
            else
            {
                LoadSubdivisions();
                int serviceCode = int.Parse(ddlServices.SelectedValue);
                int districtCode = int.Parse(ddlDistricts.SelectedValue);

                string query1 = "SELECT DISTINCT applicantgender, COUNT(applicantgender) FROM applicationdetails WHERE servicecode = @ServiceCode AND applicationdistrictcode = @DistrictCode GROUP BY applicantgender";
                string query2 = "SELECT CAST(EXTRACT(YEAR FROM applicationdate) AS INTEGER) AS year, CAST (AVG(extract (epoch from (approveddate - applicationdate))/86400) AS INTEGER)  AS mean_days_difference FROM applicationdetails WHERE applicationstatusid=16 and servicecode = @ServiceCode AND applicationdistrictcode = @DistrictCode GROUP BY EXTRACT(YEAR FROM applicationdate) ORDER BY year";
                string query3 = "SELECT  EXTRACT(YEAR FROM applicationdate)-EXTRACT(YEAR FROM applicantdob) AS year, COUNT(*) FROM applicationdetails WHERE servicecode = @ServiceCode AND applicationdistrictcode = @DistrictCode GROUP BY year ORDER BY year";
                string query4 = "SELECT EXTRACT(YEAR FROM applicationdate) AS year, COUNT(*) AS totalcount, SUM(CASE WHEN applicationstatusid = 16 THEN 1 ELSE 0 END) AS approvedcount, COUNT(*) - SUM(CASE WHEN applicationstatusid = 16 THEN 1 ELSE 0 END) AS pendingcount FROM applicationdetails WHERE servicecode = @ServiceCode AND applicationdistrictcode = @DistrictCode GROUP BY year ORDER BY year";

                NpgsqlParameter[] parameters = new NpgsqlParameter[]
                {
                    new NpgsqlParameter("@ServiceCode", serviceCode),
                    new NpgsqlParameter("@DistrictCode", districtCode)
                };

                DataTable dt1 = dbHelper.ExecuteQuery(query1, parameters);
                DataTable dt2 = dbHelper.ExecuteQuery(query2, parameters);
                DataTable dt3 = dbHelper.ExecuteQuery(query3, parameters);
                DataTable dt4 = dbHelper.ExecuteQuery(query4, parameters);

                RenderCharts(dt1, dt2, dt3, dt4);
            }
        }

        private void LoadSubdivisions()
        {
            string query = "SELECT distinct ad.applicationsubdivcode, sdm.subdivdescription FROM subdivmaster sdm, applicationdetails ad WHERE ad.applicationsubdivcode=sdm.subdivcode AND ad.applicationdistrictcode = @DistrictCode ORDER BY applicationsubdivcode";
            NpgsqlParameter[] parameters = new NpgsqlParameter[]
            {
                new NpgsqlParameter("@DistrictCode", int.Parse(ddlDistricts.SelectedValue))
            };
            DataTable dt = dbHelper.ExecuteQuery(query, parameters);
            ddlSubdivisions.DataSource = dt;
            ddlSubdivisions.DataTextField = "subdivdescription";
            ddlSubdivisions.DataValueField = "applicationsubdivcode";
            ddlSubdivisions.DataBind();
            ddlSubdivisions.Items.Insert(0, new ListItem("Select Subdivision", "0"));
        }

        protected void ddlSubdivisions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(ddlSubdivisions.SelectedIndex == 0)
            {
                ddlDistricts_SelectedIndexChanged(sender, e);
            }
            else if (ddlServices.SelectedIndex == 0)
            {
                int districtCode = int.Parse(ddlDistricts.SelectedValue);
                int subdivCode = int.Parse(ddlSubdivisions.SelectedValue);

                string query1 = "SELECT DISTINCT applicantgender, COUNT(applicantgender) FROM applicationdetails WHERE applicationdistrictcode = @DistrictCode AND applicationsubdivcode = @SubdivCode GROUP BY applicantgender";
                string query2 = "SELECT CAST(EXTRACT(YEAR FROM applicationdate) AS INTEGER) AS year, CAST (AVG(extract (epoch from (approveddate - applicationdate))/86400) AS INTEGER)  AS mean_days_difference FROM applicationdetails WHERE applicationstatusid=16 and applicationdistrictcode = @DistrictCode AND applicationsubdivcode = @SubdivCode GROUP BY EXTRACT(YEAR FROM applicationdate) ORDER BY year";
                string query3 = "SELECT  EXTRACT(YEAR FROM applicationdate)-EXTRACT(YEAR FROM applicantdob) AS year, COUNT(*) FROM applicationdetails WHERE applicationdistrictcode = @DistrictCode AND applicationsubdivcode = @SubdivCode GROUP BY year ORDER BY year";
                string query4 = "SELECT EXTRACT(YEAR FROM applicationdate) AS year, COUNT(*) AS totalcount, SUM(CASE WHEN applicationstatusid = 16 THEN 1 ELSE 0 END) AS approvedcount, COUNT(*) - SUM(CASE WHEN applicationstatusid = 16 THEN 1 ELSE 0 END) AS pendingcount FROM applicationdetails WHERE applicationdistrictcode = @DistrictCode AND applicationsubdivcode = @SubdivCode GROUP BY year ORDER BY year";

                NpgsqlParameter[] parameters = new NpgsqlParameter[]
                {
                    new NpgsqlParameter("@DistrictCode", districtCode),
                    new NpgsqlParameter("@SubdivCode", subdivCode)
                };

                DataTable dt1 = dbHelper.ExecuteQuery(query1, parameters);
                DataTable dt2 = dbHelper.ExecuteQuery(query2, parameters);
                DataTable dt3 = dbHelper.ExecuteQuery(query3, parameters);
                DataTable dt4 = dbHelper.ExecuteQuery(query4, parameters);

                RenderCharts(dt1, dt2, dt3, dt4);
            }
            else
            {
                int serviceCode = int.Parse(ddlServices.SelectedValue);
                int districtCode = int.Parse(ddlDistricts.SelectedValue);
                int subdivCode = int.Parse(ddlSubdivisions.SelectedValue);

                string query1 = "SELECT DISTINCT applicantgender, COUNT(applicantgender) FROM applicationdetails WHERE servicecode = @ServiceCode AND applicationdistrictcode = @DistrictCode AND applicationsubdivcode = @SubdivCode GROUP BY applicantgender";
                string query2 = "SELECT CAST(EXTRACT(YEAR FROM applicationdate) AS INTEGER) AS year, CAST (AVG(extract (epoch from (approveddate - applicationdate))/86400) AS INTEGER)  AS mean_days_difference FROM applicationdetails WHERE applicationstatusid=16 and servicecode = @ServiceCode AND applicationdistrictcode = @DistrictCode AND applicationsubdivcode = @SubdivCode GROUP BY EXTRACT(YEAR FROM applicationdate) ORDER BY year";
                string query3 = "SELECT  EXTRACT(YEAR FROM applicationdate)-EXTRACT(YEAR FROM applicantdob) AS year, COUNT(*) FROM applicationdetails WHERE servicecode = @ServiceCode AND applicationdistrictcode = @DistrictCode AND applicationsubdivcode = @SubdivCode GROUP BY year ORDER BY year";
                string query4 = "SELECT EXTRACT(YEAR FROM applicationdate) AS year, COUNT(*) AS totalcount, SUM(CASE WHEN applicationstatusid = 16 THEN 1 ELSE 0 END) AS approvedcount, COUNT(*) - SUM(CASE WHEN applicationstatusid = 16 THEN 1 ELSE 0 END) AS pendingcount FROM applicationdetails WHERE servicecode = @ServiceCode AND applicationdistrictcode = @DistrictCode AND applicationsubdivcode = @SubdivCode GROUP BY year ORDER BY year";

                NpgsqlParameter[] parameters = new NpgsqlParameter[]
                {
                    new NpgsqlParameter("@ServiceCode", serviceCode),
                    new NpgsqlParameter("@DistrictCode", districtCode),
                    new NpgsqlParameter("@SubdivCode", subdivCode)
                };

                DataTable dt1 = dbHelper.ExecuteQuery(query1, parameters);
                DataTable dt2 = dbHelper.ExecuteQuery(query2, parameters);
                DataTable dt3 = dbHelper.ExecuteQuery(query3, parameters);
                DataTable dt4 = dbHelper.ExecuteQuery(query4, parameters);

                RenderCharts(dt1, dt2, dt3, dt4);
            }
        }

        private void RenderCharts(DataTable dt1, DataTable dt2, DataTable dt3, DataTable dt4)
        {
            // JavaScript function to render charts
            string script = @"
                function renderCharts() {
                    var ctxPie = document.getElementById('pieChart').getContext('2d');
                    var ctxLine1 = document.getElementById('lineChart1').getContext('2d');
                    var ctxLine2 = document.getElementById('lineChart2').getContext('2d');
                    var ctxBar = document.getElementById('barChart').getContext('2d');

                    // Gender labels mapping
                    var genderLabels = {
                        'F': 'Female',
                        'M': 'Male'
                    };

                    // Data for Pie Chart
                    var pieData = {
                        labels: [" + string.Join(",", dt1.Rows.Cast<DataRow>().Select(r => "'" + r["applicantgender"].ToString() + "'").Select(g => "'" + (g == "'F'" ? "Female" : g == "'M'" ? "Male" : g) + "'")) + @"],
                        datasets: [{
                            data: [" + string.Join(",", dt1.Rows.Cast<DataRow>().Select(r => r["count"].ToString())) + @"],
                            backgroundColor: ['#FF6384', '#36A2EB']
                        }]
                    };

                    // Data for Line Chart 1
                    var lineData1 = {
                        labels: [" + string.Join(",", dt2.Rows.Cast<DataRow>().Select(r => r["year"].ToString())) + @"],
                        datasets: [{
                            label: 'Days taken to approve',
                            data: [" + string.Join(",", dt2.Rows.Cast<DataRow>().Select(r => r["mean_days_difference"].ToString())) + @"],
                            borderColor: '#4b4bc0',
                            fill: true,
                            backgroundColor: '#4b4bc03f'
                        }]
                    };

                    // Data for Line Chart 2
                    var lineData2 = {
                        labels: [" + string.Join(",", dt3.Rows.Cast<DataRow>().Select(r => r["year"].ToString())) + @"],
                        datasets: [{
                            label: 'Number of applicants of this age',
                            data: [" + string.Join(",", dt3.Rows.Cast<DataRow>().Select(r => r["count"].ToString())) + @"],
                            borderColor: '#36A2EB',
                            fill: false
                        }]
                    };

                    // Data for Bar Chart
                    var barData = {
                        labels: [" + string.Join(",", dt4.Rows.Cast<DataRow>().Select(r => r["year"].ToString())) + @"],
                        datasets: [{
                            label: 'Total Applications',
                            data: [" + string.Join(",", dt4.Rows.Cast<DataRow>().Select(r => r["totalcount"].ToString())) + @"],
                            backgroundColor: '#FFCE56AA',
                            borderColor: '#FFCE56',
                            borderWidth: 1
                        }, {
                            label: 'Approved Applications',
                            data: [" + string.Join(",", dt4.Rows.Cast<DataRow>().Select(r => r["approvedcount"].ToString())) + @"],
                            backgroundColor: '#36A2EBAA',
                            borderColor: '#36A2EB',
                            borderWidth: 1
                        }, {
                            label: 'Pending Applications',
                            data: [" + string.Join(",", dt4.Rows.Cast<DataRow>().Select(r => r["pendingcount"].ToString())) + @"],
                            backgroundColor: '#FF6384AA',
                            borderColor: '#FF6384',
                            borderWidth: 1
                        }]
                    };

                    new Chart(ctxPie, {
                        type: 'pie',
                        data: pieData,
                        options: {
                            responsive: true,
                            maintainAspectRatio: false,
                            layout: {
                                padding: {
                                    left: 10,
                                    right: 10,
                                    top: 10,
                                    bottom: 10
                                }
                            }
                        }
                    });

                    new Chart(ctxLine1, {
                        type: 'line',
                        data: lineData1,
                        options: {
                            scales: {
                                x: {
                                    type: 'linear',
                                    position: 'bottom',
                                    title: {
                                        display: true,
                                        text: 'Year'
                                    },
                                    ticks: {
                                        callback: function (value, index, values) {
                                            return value; // Display the value without formatting
                                        }
                                    }
                                },
                                y: {
                                    title: {
                                        display: true,
                                        text: 'Number of Days'
                                    }
                                }
                            }
                        }
                    });

                    new Chart(ctxLine2, {
                        type: 'line',
                        data: lineData2,
                        options: {
                            scales: {
                                x: {
                                    type: 'linear',
                                    position: 'bottom',
                                    title: {
                                        display: true,
                                        text: 'Age of Applicants'
                                    }
                                },
                                y: {
                                    title: {
                                        display: true,
                                        text: 'Number of Applicants'
                                    }
                                }
                            }
                        }
                    });

                    new Chart(ctxBar, {
                        type: 'bar',
                        data: barData,
                        options: {
                            scales: {
                                x: {
                                    type: 'linear',
                                    position: 'bottom',
                                    title: {
                                        display: true,
                                        text: 'Year'
                                    },
                                    ticks: {
                                        callback: function (value, index, values) {
                                            return value; // Display the value without formatting
                                        }
                                    }
                                },
                                y: {
                                    title: {
                                        display: true,
                                        text: 'Number of Applications'
                                    }
                                }
                            }
                        }
                    });
                }
                renderCharts();
            ";

            ClientScript.RegisterStartupScript(this.GetType(), "renderCharts", script, true);
        }
    }
}
