using System;

namespace Zen.Massage.Site.Controllers.V1
{
    public class UserItem
    {
        public Guid UserId { get; set; }

        public string Name { get; set; }

        public Gender Gender { get; set; }
    }
}