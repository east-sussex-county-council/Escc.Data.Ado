#region Using Directives

using System;
using System.IO;
using System.Web;

#endregion

namespace Escc.Data.Ado
{
    /// <summary>
    /// A class that defines the data to be stored in a database record
    /// </summary>
    [Serializable]
    public class DatabaseFileData
    {
        #region Declarations
        /// <summary>
        /// The original filename of the file
        /// </summary>
        private string fileOriginalName;
        /// <summary>
        /// The description to go with the file.
        /// </summary>
        /// <remarks>
        /// For image attachments this will be the description used for the 'alt' attribute in an html image control.
        /// </remarks>
        private string fileDescription;
        /// <summary>
        /// The MIME content type of the file
        /// </summary>
        private string fileContentType;
        /// <summary>
        /// The binary data that represents the file
        /// </summary>
        private byte[] fileBLOBData;
        /// <summary>
        /// The size of the file
        /// </summary>
        private int fileSize;
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the database file id.
        /// </summary>
        /// <value>The database file id.</value>
        public int FileId { get; set; }

        /// <summary>
        /// The filename for the file
        /// </summary>
        public string FileOriginalName
        {
            get { return fileOriginalName; }
            set { fileOriginalName = value; }
        }

        /// <summary>
        /// The description to go with the file.
        /// </summary>
        /// <remarks>
        /// For image attachments this will be the description used for the 'alt' attribute in an html image control.
        /// </remarks>
        public string FileDescription
        {
            get { return fileDescription; }
            set { fileDescription = value; }
        }

        /// <summary>
        /// The MIME content type of the file
        /// </summary>
        public string FileContentType
        {
            get { return fileContentType; }
            set { fileContentType = value; }
        }

        /// <summary>
        /// The binary data that represents the file
        /// </summary>
        public byte[] FileBLOBData
        {
            get { return fileBLOBData; }
            set { fileBLOBData = value; }
        }

        /// <summary>
        /// The size of the file
        /// </summary>
        public int FileSize
        {
            get { return fileSize; }
            set { fileSize = value; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates an empty file data record
        /// </summary>
        public DatabaseFileData()
        {
        }

        /// <summary>
        /// Creates a file data record from a file posted via the web application
        /// </summary>
        /// <param name="postedFile"></param>
        /// <param name="description">The description to go with the stored file</param>
        public DatabaseFileData(HttpPostedFile postedFile, string description)
        {
            this.ReadFileFromStream(postedFile.InputStream, postedFile.FileName);
            this.fileDescription = description;
        }

        /// <summary>
        /// Creates a file data record from a file stream and the name of the file
        /// </summary>
        /// <param name="stream">A stream object that points to an uploaded file to prepare for reading the contents of the file</param>
        /// <param name="fileName">The name of the file to read</param>
        /// <param name="description">The description to go with the stored file</param>
        public DatabaseFileData(System.IO.Stream stream, string fileName, string description)
        {
            this.ReadFileFromStream(stream, fileName);
            this.fileDescription = description;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Creates a file data record from a file stream and the name of the file
        /// </summary>
        /// <param name="stream">A stream object that points to an uploaded file to prepare for reading the contents of the file</param>
        /// <param name="fileName">The name of the file to read</param>
        public void ReadFileFromStream(System.IO.Stream stream, string fileName)
        {
            if (stream == null) return;

            // Read the file into a byte array.
            this.fileBLOBData = new byte[stream.Length];
            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(this.fileBLOBData, 0, this.fileBLOBData.Length);

            // Get the properties of the file to upload
            this.fileOriginalName = System.IO.Path.GetFileName(fileName);
            string fileExtension = System.IO.Path.GetExtension(fileName);
            this.fileContentType = DatabaseFileStorage.GetMIMETypeFromFileExtension(fileExtension);
            this.fileSize = this.fileBLOBData.Length;
        }
        #endregion
    }
}
