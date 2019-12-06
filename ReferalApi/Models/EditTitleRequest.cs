namespace codingchallenge.ReferalApi.Models
{
    public class EditTitleRequest
    {
        public string Title { get; set; }
        public string NewTitle { get; set; }

        public bool Validate() =>
            Title == null || NewTitle == null;
    }
}