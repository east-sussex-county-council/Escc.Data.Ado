using System;
using System.Data;
using System.Data.SqlClient;

namespace Escc.Data.Ado
{
	/// <summary>
	/// Wrapper for MS Data Access Application Block v2, adding extra support for returning a DataTable
	/// </summary>
	public sealed class EsccSqlHelper
	{
		#region private utility methods & constructors - copied directly from Application Block

		// Since this class provides only static methods, make the default constructor private to prevent 
		// instances from being created with "new SqlHelper()"
		private EsccSqlHelper() {}

		/// <summary>
		/// This method is used to attach array of SqlParameters to a SqlCommand.
		/// 
		/// This method will assign a value of DbNull to any parameter with a direction of
		/// InputOutput and a value of null.  
		/// 
		/// This behavior will prevent default values from being used, but
		/// this will be the less common case than an intended pure output parameter (derived as InputOutput)
		/// where the user provided no input value.
		/// </summary>
		/// <param name="command">The command to which the parameters will be added</param>
		/// <param name="commandParameters">An array of SqlParameters to be added to command</param>
		private static void AttachParameters(SqlCommand command, SqlParameter[] commandParameters)
		{
			if( command == null ) throw new ArgumentNullException( "command" );
			if( commandParameters != null )
			{
				foreach (SqlParameter p in commandParameters)
				{
					if( p != null )
					{
						// Check for derived output value with no value assigned
						if ( ( p.Direction == ParameterDirection.InputOutput || 
							p.Direction == ParameterDirection.Input ) && 
							(p.Value == null))
						{
							p.Value = DBNull.Value;
						}
						command.Parameters.Add(p);
					}
				}
			}
		}

		/// <summary>
		/// This method assigns dataRow column values to an array of SqlParameters
		/// </summary>
		/// <param name="commandParameters">Array of SqlParameters to be assigned values</param>
		/// <param name="dataRow">The dataRow used to hold the stored procedure's parameter values</param>
		private static void AssignParameterValues(SqlParameter[] commandParameters, DataRow dataRow)
		{
			if ((commandParameters == null) || (dataRow == null)) 
			{
				// Do nothing if we get no data
				return;
			}

			int i = 0;
			// Set the parameters values
			foreach(SqlParameter commandParameter in commandParameters)
			{
				// Check the parameter name
				if( commandParameter.ParameterName == null || 
					commandParameter.ParameterName.Length <= 1 )
					throw new Exception( 
						string.Format( 
						"Please provide a valid parameter name on the parameter #{0}, the ParameterName property has the following value: '{1}'.", 
						i, commandParameter.ParameterName ) );
				if (dataRow.Table.Columns.IndexOf(commandParameter.ParameterName.Substring(1)) != -1)
					commandParameter.Value = dataRow[commandParameter.ParameterName.Substring(1)];
				i++;
			}
		}

		/// <summary>
		/// This method assigns an array of values to an array of SqlParameters
		/// </summary>
		/// <param name="commandParameters">Array of SqlParameters to be assigned values</param>
		/// <param name="parameterValues">Array of objects holding the values to be assigned</param>
		private static void AssignParameterValues(SqlParameter[] commandParameters, object[] parameterValues)
		{
			if ((commandParameters == null) || (parameterValues == null)) 
			{
				// Do nothing if we get no data
				return;
			}

			// We must have the same number of values as we pave parameters to put them in
			if (commandParameters.Length != parameterValues.Length)
			{
				throw new ArgumentException("Parameter count does not match Parameter Value count.");
			}

			// Iterate through the SqlParameters, assigning the values from the corresponding position in the 
			// value array
			for (int i = 0, j = commandParameters.Length; i < j; i++)
			{
				// If the current array value derives from IDbDataParameter, then assign its Value property
				if (parameterValues[i] is IDbDataParameter)
				{
					IDbDataParameter paramInstance = (IDbDataParameter)parameterValues[i];
					if( paramInstance.Value == null )
					{
						commandParameters[i].Value = DBNull.Value; 
					}
					else
					{
						commandParameters[i].Value = paramInstance.Value;
					}
				}
				else if (parameterValues[i] == null)
				{
					commandParameters[i].Value = DBNull.Value;
				}
				else
				{
					commandParameters[i].Value = parameterValues[i];
				}
			}
		}

		/// <summary>
		/// This method opens (if necessary) and assigns a connection, transaction, command type and parameters 
		/// to the provided command
		/// </summary>
		/// <param name="command">The SqlCommand to be prepared</param>
		/// <param name="connection">A valid SqlConnection, on which to execute this command</param>
		/// <param name="transaction">A valid SqlTransaction, or 'null'</param>
		/// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">The stored procedure name or T-SQL command</param>
		/// <param name="commandParameters">An array of SqlParameters to be associated with the command or 'null' if no parameters are required</param>
		/// <param name="mustCloseConnection"><c>true</c> if the connection was opened by the method, otherwise is false.</param>
		private static void PrepareCommand(SqlCommand command, SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, SqlParameter[] commandParameters, out bool mustCloseConnection )
		{
			if( command == null ) throw new ArgumentNullException( "command" );
			if( commandText == null || commandText.Length == 0 ) throw new ArgumentNullException( "commandText" );

			// If the provided connection is not open, we will open it
			if (connection.State != ConnectionState.Open)
			{
				mustCloseConnection = true;
				connection.Open();
			}
			else
			{
				mustCloseConnection = false;
			}

			// Associate the connection with the command
			command.Connection = connection;

			// Set the command text (stored procedure name or SQL statement)
			command.CommandText = commandText;

			// If we were provided a transaction, assign it
			if (transaction != null)
			{
				if( transaction.Connection == null ) throw new ArgumentException( "The transaction was rollbacked or commited, please provide an open transaction.", "transaction" );
				command.Transaction = transaction;
			}

			// Set the command type
			command.CommandType = commandType;

			// Attach the command parameters if they are provided
			if (commandParameters != null)
			{
				AttachParameters(command, commandParameters);
			}
			return;
		}

		#endregion private utility methods & constructors - copied directly from Application Block


		
		/// <summary>
		/// Execute a SqlCommand (that returns a resultset) against the database specified in the connection string 
		/// using the provided parameters.
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  DataTable dt = ExecuteDatatable(connString, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
		/// </remarks>
		/// <param name="connectionString">A valid connection string for a SqlConnection</param>
		/// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">The stored procedure name or T-SQL command</param>
		/// <param name="commandParameters">An array of SqlParamters used to execute the command</param>
		/// <returns>A datatable containing the resultset generated by the command</returns>
        [Obsolete("Use the Microsoft Data Access Application Block or Enterprise Library data access classes instead")]
		public static DataTable ExecuteDatatable(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
		{
			if( connectionString == null || connectionString.Length == 0 ) throw new ArgumentNullException( "connectionString" );

			// Create & open a SqlConnection, and dispose of it after we are done
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();

				// Call the overload that takes a connection in place of the connection string
				return ExecuteDatatable(connection, commandType, commandText, commandParameters);
			}
		}

		/// <summary>
		/// Execute a SqlCommand (that returns a resultset) against the specified SqlConnection 
		/// using the provided parameters.
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  DataTable ds = ExecuteDatatable(conn, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
		/// </remarks>
		/// <param name="connection">A valid SqlConnection</param>
		/// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">The stored procedure name or T-SQL command</param>
		/// <param name="commandParameters">An array of SqlParamters used to execute the command</param>
		/// <returns>A datatable containing the resultset generated by the command</returns>
        [Obsolete("Use the Microsoft Data Access Application Block or Enterprise Library data access classes instead")]
        public static DataTable ExecuteDatatable(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
		{
			if( connection == null ) throw new ArgumentNullException( "connection" );

			// Create a command and prepare it for execution
			SqlCommand cmd = new SqlCommand();
			bool mustCloseConnection = false;
			PrepareCommand(cmd, connection, (SqlTransaction)null, commandType, commandText, commandParameters, out mustCloseConnection );
    			
			// Create the DataAdapter & DataSet
			using( SqlDataAdapter da = new SqlDataAdapter(cmd) )
			{
				DataTable dt = new DataTable();

				// Fill the DataTable 
				da.Fill(dt);
				
				// Detach the SqlParameters from the command object, so they can be used again
				cmd.Parameters.Clear();

				if( mustCloseConnection )
					connection.Close();

				// Return the dataset
				return dt;
			}	
		}
	
	}
}
