namespace ASP.NETCore.Models
{
    public class CreateThreadViewModel
    {
        public Tuple<Thread, Post> models { get; set; }
        public ThreadCreateViewModel createModel { get; set; }
    }
}
