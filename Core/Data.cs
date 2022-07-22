// ***********************************************************************
// Assembly         : Core
// Author           : pbosch
// Created          : 02-18-2019
//
// Last Modified By : pbosch
// Last Modified On : 03-18-2019
// ***********************************************************************
// <copyright file="Data.cs" company="Highpoint Software Systems, LLC">
//     Copyright ©  2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using MySqlConnector;

namespace Core.Data
{
    /// <summary>
    /// Class DBConnection.
    /// </summary>
    public class DBConnection
    {
        /// <summary>
        /// Prevents a default instance of the <see cref="DBConnection"/> class from being created.
        /// </summary>
        private DBConnection()
        {
        }

        /// <summary>
        /// The database name
        /// </summary>
        private string databaseName = string.Empty;
        /// <summary>
        /// Gets or sets the name of the database.
        /// </summary>
        /// <value>The name of the database.</value>
        public string DatabaseName
        {
            get { return databaseName; }
            set { databaseName = value; }
        }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>The password.</value>
        public string Password { get; set; }
        /// <summary>
        /// The connection
        /// </summary>
        private MySqlConnection connection = null;
        /// <summary>
        /// Gets the connection.
        /// </summary>
        /// <value>The connection.</value>
        public MySqlConnection Connection
        {
            get { return connection; }
        }

        /// <summary>
        /// The instance
        /// </summary>
        private static DBConnection _instance = null;
        /// <summary>
        /// Instances this instance.
        /// </summary>
        /// <returns>DBConnection.</returns>
        public static DBConnection Instance()
        {
            if (_instance == null)
                _instance = new DBConnection();
            return _instance;
        }

        /// <summary>
        /// Determines whether this instance is connect.
        /// </summary>
        /// <returns><c>true</c> if this instance is connect; otherwise, <c>false</c>.</returns>
        public bool IsConnect()
        {
            if (Connection == null)
            {
                if (String.IsNullOrEmpty(databaseName))
                    return false;
                string connstring = string.Format("Server=localhost; database={0}; UID=root; password=grouse", databaseName);
                connection = new MySqlConnection(connstring);
                connection.Open();
            }

            return true;
        }

        /// <summary>
        /// Closes this instance.
        /// </summary>
        public void Close()
        {
            connection.Close();
        }
    }
}