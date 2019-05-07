using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace SitecoreInsights
{
    public class AnalyticsViewRepository : IAnalyticsViewRepository
    {
        private readonly IConfiguration _configuration;

        public AnalyticsViewRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Add(AnalyticsView view)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            var fieldsList = new List<string>();
            var valuesList = new List<string>();

            using (var connection = GetConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    AddCreateParameter(command, fieldsList, valuesList, nameof(view.Date), view.Date);
                    AddCreateParameter(command, fieldsList, valuesList, nameof(view.MachineName), view.MachineName);
                    AddCreateParameter(command, fieldsList, valuesList, nameof(view.Username), view.Username);
                    AddCreateParameter(command, fieldsList, valuesList, nameof(view.Site), view.Site);
                    AddCreateParameter(command, fieldsList, valuesList, nameof(view.SessionId), view.SessionId);
                    AddCreateParameter(command, fieldsList, valuesList, nameof(view.Url), view.Url);
                    AddCreateParameter(command, fieldsList, valuesList, nameof(view.Duration), view.Duration);
                    AddCreateParameter(command, fieldsList, valuesList, nameof(view.IsError), view.IsError);
                    AddCreateParameter(command, fieldsList, valuesList, nameof(view.ErrorMessage), view.ErrorMessage);

                    var fieldsText = string.Join(", ", fieldsList);
                    var valuesText = string.Join(", ", valuesList);

                    command.CommandText = $"INSERT INTO AnalyticsViews ({fieldsText}) VALUES({valuesText}); SELECT SCOPE_IDENTITY()";
                    view.Id = Convert.ToInt64(command.ExecuteScalar());
                }
            }
        }

        public void EnsureSchema()
        {
            using (var connection = GetConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AnalyticsViews]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[AnalyticsViews](
	    [ID] [bigint] IDENTITY(1,1) NOT NULL,
        [Date] [datetime] NOT NULL,
        [MachineName] [varchar](100) NOT NULL,
        [Username] [varchar](100) NOT NULL,
	    [Site] [varchar](20) NOT NULL,
	    [SessionId] [varchar](24) NULL,
	    [Url] [nvarchar](1000) NOT NULL,
	    [Duration] [bigint] NOT NULL,
	    [IsError] [bit] NOT NULL,
	    [ErrorMessage] [nvarchar](max) NULL,
     CONSTRAINT [PK_AnalyticsViews] PRIMARY KEY CLUSTERED ([ID] ASC)
     WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
    ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY];

    ALTER TABLE [dbo].[AnalyticsViews] ADD CONSTRAINT [DF_AnalyticsViews_Duration]  DEFAULT ((0)) FOR [Duration];
    ALTER TABLE [dbo].[AnalyticsViews] ADD CONSTRAINT [DF_AnalyticsViews_IsError]  DEFAULT ((0)) FOR [IsError];
    CREATE NONCLUSTERED INDEX [IX_AnalyticsViews_Date] ON [dbo].[AnalyticsViews]([Date] DESC);
END";
                    command.ExecuteNonQuery();
                }
            }
        }

        private SqlConnection GetConnection()
        {
            var connectionStringKey = _configuration["SitecoreInsights.ConnectionString"];
            if (string.IsNullOrEmpty(connectionStringKey))
            {
                throw new InvalidOperationException("SitecoreInsights.ConnectionString not set in configuration");
            }

            var connectionString = _configuration.GetConnectionString(connectionStringKey);
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException($"ConnectionString {connectionStringKey} is null or empty");
            }

            var connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        }

        private void AddCreateParameter(SqlCommand command, ICollection<string> fieldList, ICollection<string> valueList, string key, object value)
        {
            command.Parameters.Add(new SqlParameter("@" + key, value ?? DBNull.Value));
            fieldList.Add($"[{key}]");
            valueList.Add($"@{key}");
        }
    }
}