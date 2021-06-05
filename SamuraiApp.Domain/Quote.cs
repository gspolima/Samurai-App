namespace SamuraiApp.Domain
{
    public class Quote
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public Samurai Samurai { get; set; }
        public int SamuraiId
        { 
            get
            {
                return _samuraiId;
            }
            set
            {
                _samuraiId = value;
                _hasValue = true;
            }
        }

        public bool SamuraiIdHasValue => _hasValue;

        private int _samuraiId;
        private bool _hasValue;
    }
}