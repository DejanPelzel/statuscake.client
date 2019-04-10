namespace StatusCake.Client.Models
{
    /// <summary>
    /// Contains data about deleting a response
    /// </summary>
    public class DeleteTest
    {
        /// <summary>
        /// The ID of the test that has been deleted
        /// </summary>
        public long TestID { get; set; }

        /// <summary>
        /// The number of rows deleted
        /// </summary>
        public long Affected { get; set; }

        /// <summary>
        /// If true, the deletion was successful
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// The message response
        /// </summary>
        public string Message { get; set; }
    }
}
