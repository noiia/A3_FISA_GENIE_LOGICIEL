namespace Services
{
    public class ReturnCodes
    {
        public const int OK = 1;
        public const int BAD_ARGS = 2;
        public const int JOB_DOES_NOT_EXIST = 3;
        public const int SOURCE_DOES_NOT_EXIST = 4;
        public const int DESTINANTION_DOES_NOT_EXIST = 5;
        public const int TYPE_DOES_NOT_EXIST = 6;
        public const int MORE_THAN_5_SAVEJOB = 7;
        public const int NAME_ALREADY_USE = 8;
        public const string TYPE_FULL = "full";
        public const string TYPE_DIFF = "diff";
    }
}
