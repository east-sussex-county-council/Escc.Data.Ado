using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections;

namespace EsccWebTeam.Data.Ado
{
	/// <summary>
	/// This class is to provide useful ADO.NET helper methods for manipulating ADO.Net objects and data.
	/// </summary>
	public class EsccAdoHelper
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="EsccAdoHelper"/> class.
        /// </summary>
		public EsccAdoHelper()
		{
			
		}

        /// <summary>
        /// This is a generic method for processing Likert tables. It relies on the fact that the question references and HTML are in sync.
        /// this method.
        /// </summary>
        /// <param name="controlToEvaluate">Control</param>
        /// <param name="page">Page</param>
        /// <param name="tableName">ID of the table e.g. LikertQ1</param>
        /// <param name="questionReferences">An array of string e.g. Q1a, Q1b, Q1c etc</param>
        /// <param name="startParameter">The sql parameter index e.g. parameter[0] = 0</param>
        /// <param name="parameters">Sql parameter list</param>
        /// <returns>SqlParameter[]</returns>
        public SqlParameter[] AssignLikertScaleValuesToSqlParameterArray(HtmlTable tableName, ArrayList questionReferences, int startParameter, SqlParameter[] parameters)
        {
            //Find the table with the likert scale radio buttons
            Control table = tableName;

            //Set an index to use with the sql parameter array
            int prmindex = startParameter;

            //Set the name of the radiobutton e.g. q1a to keep track of when we are on a new row of options
            string currentName = questionReferences[0].ToString();
            //Loop over the tables until you get down to the radio buttons
            foreach (HtmlTableRow tableRow in table.Controls)
            {
                foreach (HtmlTableCell tableCell in tableRow.Controls)
                {
                    foreach (Control control in tableCell.Controls)
                    {
                        if (control.GetType().FullName == "System.Web.UI.HtmlControls.HtmlInputRadioButton")
                        {

                            HtmlInputRadioButton radioButton = control as HtmlInputRadioButton;

                            //Compare the radio button to the list passed in for a match
                            if (questionReferences.Contains(radioButton.Name))
                            {
                                //Check if the radio button has moved on e.g. q1a is now q1b
                                if (currentName == radioButton.Name)
                                {

                                    //Only update the parameter value if checked
                                    if (radioButton.Checked)
                                    {
                                        parameters[prmindex].Value = radioButton.Value;
                                     

                                    }


                                }
                                else
                                {
                                 
                                 //When we move on to a new row we increment the parameter index
                                 prmindex++;

                                    //Set the name of the current radio button as we have moved on to the next row of radio buttons
                                    currentName = radioButton.Name;

                                    //Update the parameter with the value of the checked radio button
                                    if (radioButton.Checked)
                                    {
                                        parameters[prmindex].Value = radioButton.Value;
                                        

                                    }
                                }

                            }

                        }
                    }
                }
            }



            return parameters;
        }

        public SqlParameter[] AssignHtmlCheckBoxValuesToSqlParameterArray(HtmlTable tableName, ArrayList questionReferences, int startParameter, SqlParameter[] parameters)
        {
            //Find the table with the likert scale radio buttons
            Control table = tableName;

            //Set an index to use with the sql parameter array
            int prmindex = startParameter;

            //Set the name of the radiobutton e.g. q1a to keep track of when we are on a new row of options
            string currentName = questionReferences[0].ToString();
            //Loop over the tables until you get down to the radio buttons
            foreach (HtmlTableRow tableRow in table.Controls)
            {
                foreach (HtmlTableCell tableCell in tableRow.Controls)
                {
                    foreach (Control control in tableCell.Controls)
                    {
                        if (control.GetType().FullName == "System.Web.UI.HtmlControls.HtmlInputCheckBox")
                        {

                            HtmlInputCheckBox checkBox = control as HtmlInputCheckBox;

                            //Compare the radio button to the list passed in for a match
                            if (questionReferences.Contains(checkBox.UniqueID))
                            {
                                //Check if the radio button has moved on e.g. q1a is now q1b
                                if (currentName == checkBox.UniqueID)
                                {
                                    //Only update the parameter value if checked
                                    if (checkBox.Checked)
                                    {
                                        parameters[prmindex].Value = checkBox.Value;

                                    }
                                }
                                else
                                {
                                    //The radio button has changed e.g. q1a to q1b so we increment the sql paramater index
                                    prmindex++;

                                    //Set the name of the current radio button as we have moved on to the next row of radio buttons
                                    currentName = checkBox.UniqueID;

                                    //Update the parameter with the value of the checked radio button
                                    if (checkBox.Checked)
                                    {
                                        parameters[prmindex].Value = checkBox.Value;

                                    }
                                }
                            }
                        }

                    }
                }
            }

            return parameters;
        }

        public SqlParameter[] AssignHtmlRadioButtonValuesToSqlParameterArray(HtmlTable tableName, ArrayList questionReferences, int startParameter, SqlParameter[] parameters)
        {
            //Find the table with the likert scale radio buttons
            Control table = tableName;

            //Set an index to use with the sql parameter array
            int prmindex = startParameter;


            //Loop over the tables until you get down to the radio buttons
            foreach (HtmlTableRow tableRow in table.Controls)
            {
                foreach (HtmlTableCell tableCell in tableRow.Controls)
                {
                    foreach (Control control in tableCell.Controls)
                    {
                        if (control.GetType().FullName == "System.Web.UI.HtmlControls.HtmlInputRadioButton")
                        {

                            HtmlInputRadioButton radioButton = control as HtmlInputRadioButton;

                            //Compare the radio button to the list passed in for a match
                            if (questionReferences.Contains(radioButton.UniqueID))
                            {
                                //Check if the radio button has moved on e.g. q1a is now q1b

                                //Only update the parameter value if checked
                                if (radioButton.Checked)
                                {
                                    parameters[prmindex].Value = radioButton.Value;
                                    

                                }
                                else
                                {
                                    parameters[prmindex].Value = DBNull.Value;
                                   
                                }
                            }

                            prmindex++;
                        }
                    }

                }
            }

            return parameters;
        }





        /// <summary>
        /// Method loops through the checkboxlist passed in and using a start index for the sqlparameter collection assigns the values of each checkbox item selected to the matching sqlparameter.
        /// The sqlparameter collection is passed in as a reference so it modifies the actual collection not a copy, i.e no return datatype.
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="checkboxList"></param>
        /// <param name="parameters"></param>
        public void AssignCheckBoxListValuesToSqlParameterArray(int startIndex, CheckBoxList checkboxList, ref SqlParameter[] parameters)
        {
            for (int i = 0; i < checkboxList.Items.Count; i++)
            {
                if (checkboxList.Items[i].Selected)
                {
                    parameters[startIndex].Value = checkboxList.Items[i].Value;
                }
                else
                {
                    parameters[startIndex].Value = DBNull.Value;
                }
                startIndex++;
            }
        }
		
		/// <summary>
		/// Methods takes a unordered dataset and filters and or sorts it based
		/// on the parameter and then returns a new dataset with the orginal schema
		/// but reordered data.
		/// </summary>
		/// <remarks>The DataSet is only mandatory parameters, 
		/// if you do not want to use all the parmeters pass in nulls</remarks>
		/// <example>DataSet myDataSet = ReOrderDataSetWithSingleTable(myDataSet, "ref > 200", "columnName desc",mydataViewRowState.OriginalRows )"</example>
		/// <param name="originalDataSet"></param>
		/// <param name="filterExpression"></param>
		/// <param name="sortBy"></param>
		/// <param name="dataViewRowState"></param>
		/// <returns>DataSet</returns>
		public DataSet ReOrderDataSetWithSingleTable(DataSet originalDataSet, string filterExpression, string sortBy, DataViewRowState dataViewRowState)
		{
			//Clone the dataset schema (tables etc) but NO data
			DataSet modifiedDataSet = originalDataSet.Clone();	


			DataRow[] dataRowArray;
			dataRowArray = originalDataSet.Tables[0].Select(filterExpression, sortBy,dataViewRowState);

			foreach (DataRow originalRow in dataRowArray)
			{
				DataRow newDataRow = modifiedDataSet.Tables[0].NewRow();

				newDataRow.ItemArray = originalRow.ItemArray;

				modifiedDataSet.Tables[0].Rows.Add(newDataRow);
			}

			return modifiedDataSet;
		}
		
		/// <summary>
		/// Methods takes a unordered dataset, table index (eg. table[0] as an int) and filters and or sorts it based
		/// on the parameter and then returns a new dataset with the orginal schema
		/// but reordered data.
		/// </summary>
		/// <remarks>The DataSet and the int (tableIndex) are the only mandatory parameters, 
		/// if you do not want to use all the parmeters pass in nulls</remarks>
		/// <example>DataSet myDataSet = ReOrderDataSetWithSingleTable(myDataSet, 0, "ref > 200", "columnName desc",mydataViewRowState.OriginalRows )"</example>
		/// <param name="originalDataSet"></param>
		/// <param name="tableIndex"></param>
		/// <param name="filterExpression"></param>
		/// <param name="sortBy"></param>
		/// <param name="dataViewRowState"></param>
		/// <returns>DataSet</returns>
		public DataSet ReOrderDataSetWithSingleTable(DataSet originalDataSet, int tableIndex, string filterExpression, string sortBy, DataViewRowState dataViewRowState)
		{
			//Clone the dataset schema (tables etc) but NO data
			DataSet modifiedDataSet = originalDataSet.Clone();	


			DataRow[] dataRowArray;
			dataRowArray = originalDataSet.Tables[tableIndex].Select(filterExpression, sortBy,dataViewRowState);

			foreach (DataRow originalRow in dataRowArray)
			{
				DataRow newDataRow = modifiedDataSet.Tables[tableIndex].NewRow();

				newDataRow.ItemArray = originalRow.ItemArray;

				modifiedDataSet.Tables[tableIndex].Rows.Add(newDataRow);
			}

			return modifiedDataSet;
		}
		
		/// <summary>
		/// Methods takes a unordered dataset, an array of table indexes (eg. table[0], 
		/// table[2] as an int array) and filters and or sorts it based
		/// on the parameter and then returns a new dataset with the orginal schema
		/// but reordered data.
		/// </summary>
		/// <remarks>The DataSet and Int Array are the only mandatory parameters, 
		/// if you do not want to use all the parmeters pass in nulls</remarks>
		/// <example>DataSet myDataSet = ReOrderDataSetWithSingleTable(myDataSet, intArray, "ref > 200", "columnName desc",mydataViewRowState.OriginalRows )"</example>
		/// <param name="originalDataSet">DataSet</param>
		/// <param name="tableIndex">Array of Integers</param>
		/// <param name="filterExpression">String</param>
		/// <param name="sortBy">String</param>
		/// <param name="dataViewRowState">DataViewRowState</param>
		/// <returns>DataSet</returns>
		public DataSet ReOrderDataSetWithMultipleTables(DataSet originalDataSet, int[] tableIndex, string filterExpression, string sortBy, DataViewRowState dataViewRowState)
		{
			
			//Clone the dataset schema (tables etc) but NO data
				DataSet modifiedDataSet = originalDataSet.Clone();

			foreach (int tbIndex in tableIndex)
			{	
				DataRow[] dataRowArray;
				dataRowArray = originalDataSet.Tables[tbIndex].Select(filterExpression, sortBy,dataViewRowState);
			
				foreach (DataRow originalRow in dataRowArray)
				{
					DataRow newDataRow = modifiedDataSet.Tables[tbIndex].NewRow();

					newDataRow.ItemArray = originalRow.ItemArray;

					modifiedDataSet.Tables[tbIndex].Rows.Add(newDataRow);
				}
			}
			return modifiedDataSet;
		}

		/// <summary>
		/// This method creates a CSV file based on a SQL DataTable and saves it 
		/// at the path specified e.g. C:\test.csv
		/// </summary>
		/// <param name="dt">SQL DataTable</param>
		/// <param name="strFilePath">Location and file name e.g. C:\demo.csv</param>
		public  void CreateCSVFile(DataTable dt, string strFilePath)
		{
			StreamWriter sw = new StreamWriter(strFilePath, false);

			// First we create the column headers.

			int iColCount = dt.Columns.Count;

			for (int i = 0; i < iColCount; i++)
			{
				sw.Write(dt.Columns[i]);

				if (i < iColCount - 1)
				{
					sw.Write(",");
				}
			}


			sw.Write(sw.NewLine);

			// Now we write all the rows.

			foreach (DataRow dr in dt.Rows)
			{
				for (int i = 0; i < iColCount; i++)
				{

					if (!Convert.IsDBNull(dr[i]))
					{
                        string escapeCommas = "\"" + dr[i].ToString() + "\"";
                        sw.Write(escapeCommas);
					}


					if (i < iColCount - 1)
					{

						sw.Write(",");

					}
				}

				sw.Write(sw.NewLine);
			}
			sw.Close();

		}


	}
}
