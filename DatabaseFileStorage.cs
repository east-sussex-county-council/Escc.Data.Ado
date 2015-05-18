#region Using Directives
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Web;
using Microsoft.ApplicationBlocks.Data;
#endregion

namespace EsccWebTeam.Data.Ado
{
    /// <summary>
    /// A controller to save and retrieve data from database storage
    /// </summary>
    public static class DatabaseFileStorage
    {
        #region MIME Content Types
        /// <summary>
        /// Converts a file extension into a MIME content type.
        /// </summary>
        /// <param name="fileExtension"></param>
        /// <returns></returns>
        public static string GetMIMETypeFromFileExtension(string fileExtension)
        {
            string MIMEContentType = string.Empty;
            switch (fileExtension.ToLower())
            {
                case ".bmp":
                    MIMEContentType = "image/bmp";
                    break;
                case ".doc":
                case ".docx":
                    MIMEContentType = "application/msword";
                    break;
                case ".gif":
                    MIMEContentType = "image/gif";
                    break;
                case ".jpe":
                case ".jpeg":
                case ".jpg":
                    MIMEContentType = "image/jpeg";
                    break;
                case ".mp3":
                    MIMEContentType = "audio/mpeg";
                    break;
                case ".pdf":
                    MIMEContentType = "application/pdf";
                    break;
                case ".png":
                    MIMEContentType = "image/png";
                    break;
                case ".pps":
                case ".ppt":
                case ".pptx":
                    MIMEContentType = "application/vnd.ms-powerpoint";
                    break;
                case ".rtf":
                    MIMEContentType = "application/rtf";
                    break;
                case ".tif":
                case ".tiff":
                    MIMEContentType = "image/tiff";
                    break;
                case ".wma":
                    MIMEContentType = "audio/x-ms-wma";
                    break;
                case ".wmv":
                    MIMEContentType = "audio/x-ms-wmv";
                    break;
                case ".xls":
                case ".xlsx":
                    MIMEContentType = "application/vnd.ms-excel";
                    break;
                case ".zip":
                    MIMEContentType = "application/zip";
                    break;
            }
            return MIMEContentType;
        }
        #endregion

        #region File saving
        /// <summary>
        /// Adds or amends a file data record in a database. The database record has to be of a standard format.
        /// </summary>
        /// <param name="connectionString">The connection string we need to connect to the database with.</param>
        /// <param name="storedProcedure">The named of the stored procedure to call once connected.</param>
        /// <param name="fileDataID">If updating, this is the record id for the stored image</param>
        /// <param name="fileData"></param>
        /// <param name="modifiedBy"></param>
        /// <returns></returns>
        public static int SaveFile(
            string connectionString,
            string storedProcedure,
            int fileDataID,
            DatabaseFileData fileData,
            string modifiedBy)
        {
            // Set a default for the stored procedure to use if one is not given.
            if (string.IsNullOrEmpty(storedProcedure)) storedProcedure = "usp_AddAmendFileData";

            // create a parameter to pass the file data to the database
            SqlParameter paramBLOBData = new SqlParameter(
                "@FileData", SqlDbType.Image, fileData.FileBLOBData.Length,
                ParameterDirection.Input, false, 0, 0, null,
                DataRowVersion.Current, fileData.FileBLOBData);

            try
            {
                // Add parameters in the order they appear in the stored procedure:
                object objReturned = SqlHelper.ExecuteScalar(connectionString, CommandType.StoredProcedure, storedProcedure,
                    new SqlParameter("@FileDataID", fileDataID),
                    new SqlParameter("@FileOriginalName", fileData.FileOriginalName),
                    new SqlParameter("@FileDescription", fileData.FileDescription),
                    new SqlParameter("@MIMEContentType", fileData.FileContentType),
                    paramBLOBData,
                    new SqlParameter("@Username", modifiedBy));

                // Convert the result
                fileDataID = Convert.ToInt32(objReturned);
            }
            catch (Exception err)
            {
                // We need a very detailed error message here because
                // the document is being shipped via Port 80 and we can
                // have intermittent problems. A full error message helps
                // give us some sort of clue as to what files fail to upload
                // and when
                throw new Exception(
                    "An error occurred when adding/amending the document '" + fileData.FileOriginalName + "'" +
                    " (MIMEContentType=" + fileData.FileContentType +
                    ";Length=" + Convert.ToString(fileData.FileBLOBData.LongLength) +
                    ";Time=" + DateTime.Now.ToString() + ")", err);
            }

            return fileDataID;
        }
        #endregion

        #region File removal
        /// <summary>
        /// Deletes a file data record from a database
        /// </summary>
        /// <param name="fileDataID">The record id for the stored file data</param>
        public static void DeleteFile(
            string connectionString,
            string storedProcedure,
            int fileDataID)
        {
            // Set a default for the stored procedure to use if one is not given.
            if (string.IsNullOrEmpty(storedProcedure)) storedProcedure = "usp_DeleteFileData";

            // Add parameters in the order they appear in the stored procedure:
            SqlHelper.ExecuteNonQuery(connectionString, CommandType.StoredProcedure, storedProcedure,
                new SqlParameter("@FileDataID", fileDataID));
        }
        #endregion

        #region File retrieving
        /// <summary>
        /// Gets a file data record from a database
        /// </summary>
        /// <param name="connectionString">The connection string we need to connect to the database with.</param>
        /// <param name="storedProcedure">The named of the stored procedure to call once connected.</param>
        /// <param name="fileDataID">The record id for the stored file data</param>
        /// <returns></returns>
        public static DataTable GetFile(
            string connectionString,
            string storedProcedure,
            int fileDataID)
        {
            // Set a default for the stored procedure to use if one is not given.
            if (string.IsNullOrEmpty(storedProcedure)) storedProcedure = "usp_GetFileData";

            // Add parameters in the order they appear in the stored procedure:
            DataSet ds = SqlHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, storedProcedure,
                new SqlParameter("@FileDataID", fileDataID));

            // Check if we have any data returned.
            DataTable dt = null;
            if (ds != null)
            {
                if (ds.Tables.Count > 0) dt = ds.Tables[0];
            }

            return dt;
        }
        #endregion
    }
}
