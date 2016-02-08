namespace Zen.Massage.Site.Controllers.V1
{
    public class ResultDto<T>
    {
        public int StatusCode { get; set; }
        
        public string StatusDescription { get; set; }

        public T Result { get; set; }
    }
}