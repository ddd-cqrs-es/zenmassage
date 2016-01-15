using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zen.Massage.Site.Models
{
    public class BaseVm
    {
        private string _metaTitle;

        public string Title { get; set; }

        public string MetaTitle
        {
            get
            {
                if (!string.IsNullOrEmpty(_metaTitle))
                {
                    return _metaTitle;
                }

                if (string.IsNullOrEmpty(Title))
                {
                    return string.Empty;
                }

                return $"{Title} | Zen Massage";
            }
            set { _metaTitle = value; }
        }

        public string MetaDescription { get; set; }

        public string MetaKeywords { get; set; }
    }
}
