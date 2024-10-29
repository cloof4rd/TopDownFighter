namespace YogiGameCore.Const
{
    public class ResourceTypeErrorException : System.Exception { }

    public class ResourcePathErrorException : System.Exception
    {
        public ResourcePathErrorException(string path) : base(path)
        {
        }
    }
    
}