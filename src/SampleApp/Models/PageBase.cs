namespace SampleApp.Models
{
    public class PageBase
    {
        private bool _isAuthenticated = true;
        private string _username = "Anonymous";

        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                _isAuthenticated = true;
            }
        }

        public bool IsAuthenticated
        {
            get { return _isAuthenticated; }
        }
    }
}