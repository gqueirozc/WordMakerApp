namespace WordMakerDashboard.Models
{
    /// <summary>
    /// Represents a word and its attributes.
    /// </summary>
    public class Word
    {
        /// <summary>
        /// Represents the definition of the word.
        /// </summary>
        public string Definition { get; set; }

        /// <summary>
        /// Represents an example of usage of the word.
        /// </summary>
        public string Example { get; set; }
    }
}