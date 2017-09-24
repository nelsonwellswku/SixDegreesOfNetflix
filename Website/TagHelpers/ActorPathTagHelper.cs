using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Octogami.SixDegreesOfNetflix.Website.Models;

namespace Octogami.SixDegreesOfNetflix.Website.TagHelpers
{
    public class ActorPathTagHelper : TagHelper
    {
        public ActorPathViewModel ActorPath { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var stringBuilder = new StringBuilder();
            ActorPathViewModel nextInPath = ActorPath;


            while (nextInPath != null)
            {
                if (nextInPath.ActedIn?.With != null)
                {
                    stringBuilder.Append(
                        $"<h3>{nextInPath.Name} acted in {nextInPath.ActedIn.Title} with {nextInPath.ActedIn.With.Name}</h3>");
                }

                nextInPath = nextInPath?.ActedIn?.With;
            }

            output.Content.SetHtmlContent(stringBuilder.ToString());
        }
    }
}
