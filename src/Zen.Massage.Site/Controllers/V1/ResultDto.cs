namespace Zen.Massage.Site.Controllers.V1
{
    public class ResultDto
    {
        public int StatusCode { get; set; }

        public string StatusDescription { get; set; }
    }

    public class ResultDto<T> : ResultDto
    {
        public T Result { get; set; }
    }
}