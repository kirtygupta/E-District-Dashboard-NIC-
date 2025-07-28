# 🌐 E-District Dashboard – Data Analytics in Governance

![Status: Completed](https://img.shields.io/badge/Status-Completed-brightgreen.svg)
![Tech: ASP.NET](https://img.shields.io/badge/Tech-ASP.NET-blue)
![Database: PostgreSQL](https://img.shields.io/badge/Database-PostgreSQL-blue)
![Frontend: Chart.js](https://img.shields.io/badge/Visualization-Chart.js-orange)

> 🚀 A comprehensive data analytics dashboard for the **Delhi Government's E-District Portal**, enabling **data-driven governance** and real-time performance insights.

---

## 📌 Project Overview

This project was developed during a **summer internship** at the **National Informatics Centre (NIC), Delhi**, under the guidance of **Mr. Sandeep Jain**, Senior Technical Director. The dashboard focuses on visualizing key service-related metrics and providing deep insights into citizen engagement, demographic patterns, and service processing timelines.

---

## ✨ Features

✅ **Interactive Charts & Graphs**  
✅ **Real-time Filtering by Service, District, Subdivision**  
✅ **Responsive Design** (mobile & desktop)  
✅ **Key Metrics Tracked**:
- Gender Distribution
- Age Group Analysis
- Application Turnaround Time
- Service Usage by Type
- Application Status (Approved / Pending)

---

## 🧰 Tech Stack

| Layer       | Technologies                                |
|-------------|---------------------------------------------|
| Frontend    | HTML5, CSS3, JavaScript, ASP.NET Web Forms |
| Visuals     | [Chart.js](https://www.chartjs.org/)        |
| Backend     | C# (.NET Framework 4.7.2)                   |
| Database    | PostgreSQL 12+                              |
| Methodology | Agile                                       |

---

## 🏁 Getting Started

### 🔧 Prerequisites
- .NET Framework 4.7.2 or later
- PostgreSQL 12+

---
### ⚙️ Installation

```bash

# Clone the Repo
git clone https://github.com/kirtygupta/E-District-Dashboard-NIC-.git
```

---

## 🛠️ Step-by-Step Setup

### 🗄️ 1. Set Up the PostgreSQL Database

* Open your PostgreSQL client (e.g., pgAdmin)
* Create a new database (e.g., `edistrict_dashboard`)
* Run the SQL scripts located in the `/database` folder to create tables and seed data

### ⚙️ 2. Configure the Application

* Open the project in **Visual Studio**

* In `web.config`, update the connection string:

  ```xml
  <connectionStrings>
    <add name="PostgresDB" 
         connectionString="Host=localhost;Port=5432;Database=edistrict_dashboard;Username=your_user;Password=your_password;" 
         providerName="Npgsql" />
  </connectionStrings>
  ```

* Ensure that the required NuGet packages are installed (like `Npgsql` for PostgreSQL)

---

## 💻 How to Use the Dashboard

1. Open your browser and go to the URL you configured (e.g., `http://localhost/e-district-dashboard`)
2. Login via the authentication page
3. Use dropdown filters to select:

   * 📂 Service type
   * 🏙️ District
   * 🏘️ Subdivision
4. Watch the charts dynamically update
5. Export or analyze data as needed

---

## 📈 Insights from Data

| Category               | Insight                                           |
| ---------------------- | ------------------------------------------------- |
| Gender Participation   | Only **35%** of applicants were **female**        |
| Most Requested Service | SC & OBC caste certificates                       |
| Avg. Approval Time     | **6 months** from application to approval         |
| Age Group Trends       | **23–28 years** most active age demographic       |
| Regional Variation     | High disparity in application volumes by district |

---

## 💡 Strategic Recommendations

* 📣 Run awareness campaigns for women
* ⚙️ Prioritize automation for high-demand services
* 🧾 Simplify multi-step manual processes
* 🧭 Develop district-specific outreach programs
* 📲 Improve digital access for the youth demographic

---

## 🙌 Acknowledgments

* **Mr. Sandeep Jain**, Sr. Technical Director, NIC Delhi
* NIC Software & Network Teams

---

## 👩‍💻 Author

**Kirty Gupta**

- 📧 **Email:** [guptakirty11@gmail.com](mailto:guptakirty11@gmail.com)  
- 💻 **GitHub:** [@kirtygupta](https://github.com/kirtygupta)


---

> 🎓 *Project submitted as part of B.Tech (AI & Data Science) internship at NIC Delhi, under VIPS-TC.*
